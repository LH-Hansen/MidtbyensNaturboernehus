using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.ComponentModel.Design;
using System.Net;
using System.CodeDom.Compiler;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Xml;

namespace MidtbyensBørnehus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime tidNu = new DateTime();

            int angivBarn = 0;

            string brugerinput, brugerValg, ankomst, afgang, ugedag;

            //Filsti for diverse .txt filer
            string nBørnFilsti = @"N:\NaturenBørn.txt";
            string tBørnFilsti = @"T:\TeknikkenBørn.txt";
            string mBørnFilsti = @"M:\MenneskerBørn.txt";

            string nPersonaleSygdomFilsti = @"N:\Personale\NaturenSygdom.txt";
            string tPersonaleSygdomFilsti = @"T:\Personale\TeknikkenSygdom.txt";
            string mPersonaleSygdomFilsti = @"M:\Personale\MenneskerSygdom.txt";

            string nPersonaleUdflugtFilsti = @"N:\Personale\NaturenOpslag.txt";
            string tPersonaleUdflugtFilsti = @"T:\Personale\TeknikkenOpslag.txt";
            string mPersonaleUdflugtFilsti = @"M:\Personale\MenneskerOpslag.txt";

            //Fremmøde statistik for børn
            string[,] nBørnStatistik = new string[50, 6];
            string[,] tBørnStatistik = new string[50, 6];
            string[,] mBørnStatistik = new string[50, 6];

            //Sygdom-status for personale
            string[,] nPersonaleSygdom = new string[10, 2];
            string[,] tPersonaleSygdom = new string[10, 2];
            string[,] mPersonaleSygdom = new string[10, 2];

            //Oprettede børn i børnehaven
            string[,] børnNaturen = new string[50, 4];
            string[,] børnTeknikken = new string[50, 4];
            string[,] børnMennesker = new string[50, 4];

            //Login informationer for brugere
            string[,] persoanleBrugere = new string[25, 2];
            string[,] forældreBrugere = new string[150, 2];

            //Standart bruger for fremvisning
            persoanleBrugere[0, 0] = "1"; persoanleBrugere[0, 1] = "1";
            forældreBrugere[0, 0] = "3"; forældreBrugere[0, 1] = "3";

            //Opret brugere
            OpretBrugerePersonale(persoanleBrugere);
            OpretBrugereForældre(forældreBrugere);

            //Reset arrays til predefinderet standart
            ArrayResetBørn(børnNaturen, børnTeknikken, børnMennesker);
            ArraysResetPersonale(nPersonaleSygdom, tPersonaleSygdom, mPersonaleSygdom);
            ArrayResetStatistik(nBørnStatistik, tBørnStatistik, mBørnStatistik);

            while (true)
            {
                bool personaleLoginKorrekt = false, forældreLoginKorrekt = false;

                //Menu for login
                Console.WriteLine("                       _   _      _ _                                          _   _ _                            \r\n                      | | | |    | | |                                        | | (_) |                           \r\n                      | | | | ___| | | _____  _ __ ___  _ __ ___   ___ _ __   | |_ _| |                           \r\n                      | | | |/ _ \\ | |/ / _ \\| '_ ` _ \\| '_ ` _ \\ / _ \\ '_ \\  | __| | |                           \r\n                      \\ \\_/ /  __/ |   < (_) | | | | | | | | | | |  __/ | | | | |_| | |                           \r\n                       \\___/ \\___|_|_|\\_\\___/|_| |_| |_|_| |_| |_|\\___|_| |_|  \\__|_|_|                           \r\n                                                                                                                  \r\n                                                                                                                  \r\n___  ____     _ _   _                            _   _       _              _                     _               \r\n|  \\/  (_)   | | | | |                          | \\ | |     | |            | |                   | |              \r\n| .  . |_  __| | |_| |__  _   _  ___ _ __  ___  |  \\| | __ _| |_ _   _ _ __| |__  _ __ _ __   ___| |__  _   _ ___ \r\n| |\\/| | |/ _` | __| '_ \\| | | |/ _ \\ '_ \\/ __| | . ` |/ _` | __| | | | '__| '_ \\| '__| '_ \\ / _ \\ '_ \\| | | / __|\r\n| |  | | | (_| | |_| |_) | |_| |  __/ | | \\__ \\ | |\\  | (_| | |_| |_| | |  | |_) | |  | | | |  __/ | | | |_| \\__ \\\r\n\\_|  |_/_|\\__,_|\\__|_.__/ \\__, |\\___|_| |_|___/ \\_| \\_/\\__,_|\\__|\\__,_|_|  |_.__/|_|  |_| |_|\\___|_| |_|\\__,_|___/\r\n                           __/ |                                                                                  \r\n                          |___/                                                                                   ");
                Console.ReadLine();
                Console.Clear();
                Console.Write("Login\n\n1. Forældre login\n2. Personale login\n\nValg:\t");
                brugerValg = Console.ReadLine();
                Console.Clear();

                switch (brugerValg)
                {
                    case "1":
                        Console.WriteLine("Forældre login\n");
                        forældreLoginKorrekt = Login(forældreBrugere);
                        break;

                    case "2":
                        Console.WriteLine("Personale login\n");
                        personaleLoginKorrekt = Login(persoanleBrugere);
                        break;

                    default:
                        Fejlmeddelelse();
                        break;
                }

                //Forældre menu
                while (forældreLoginKorrekt)
                {
                    Console.Clear();
                    Console.Write("Forældre menu\n\n1. Afhentning/aflevering\n2. Statistik for fremmøde\n3. Ændre kode\n4. Log ud\n\nValg:\t");

                    brugerValg = Console.ReadLine();

                    Console.Clear();

                    switch (brugerValg)
                    {
                        case "1":
                            Console.Write("Aflevering/afhentning\n\n1. Aflever barn\n2. Afhent barn\n\nValg:\t");

                            brugerValg = Console.ReadLine();

                            Console.Clear();

                            //Aflever barn
                            if (brugerValg == "1")
                            {
                                Console.Write("Hvor skal barnet registreres?\n\n1. Naturen\n2. Teknikken\n3. Mennesker\n4. tilbage\n\nAfdeling:\t");

                                brugerValg = Console.ReadLine();
                                Console.Clear();

                                switch (brugerValg)
                                {
                                    case "1":
                                        angivBarn = AfleverBarn(børnNaturen, nBørnFilsti, brugerValg);
                                        ankomst = DateTime.Now.ToString("HH:mm");

                                        børnNaturen[angivBarn, 3] = ankomst;
                                        nBørnStatistik[angivBarn, 0] = børnNaturen[angivBarn, 0];

                                        break;

                                    case "2":
                                        angivBarn = AfleverBarn(børnTeknikken, tBørnFilsti, brugerValg);
                                        ankomst = DateTime.Now.ToString("HH:mm");

                                        børnNaturen[angivBarn, 3] = ankomst;
                                        tBørnStatistik[angivBarn, 0] = børnTeknikken[angivBarn, 0];
                                        break;

                                    case "3":
                                        angivBarn = AfleverBarn(børnMennesker, mBørnFilsti, brugerValg);
                                        ankomst = DateTime.Now.ToString("HH:mm");

                                        børnMennesker[angivBarn, 3] = ankomst;
                                        mBørnStatistik[angivBarn, 0] = børnMennesker[angivBarn, 0];
                                        break;

                                    case "4":
                                        break;

                                    default:
                                        Fejlmeddelelse();
                                        break;
                                }
                            }

                            //Afhent barn
                            else if (brugerValg == "2")
                            {
                                Console.Write("Hvor skal barnet afhentes fra?\n1. Naturen\n2. Teknikken\n3. Mennesker\n4. tilbage\n\nAfdeling:\t");
                                brugerValg = Console.ReadLine();
                                Console.Clear();

                                switch (brugerValg)
                                {
                                    case "1":
                                        angivBarn = AfhentBarn(børnNaturen);
                                        afgang = DateTime.Now.ToString("HH:mm");
                                        ugedag = tidNu.DayOfWeek.ToString();
                                        Console.Clear();

                                        switch (ugedag)
                                        {
                                            case "Monday":
                                                nBørnStatistik[angivBarn, 1] = BeregnTid(børnNaturen, angivBarn, afgang);
                                                break;
                                            case "Tuesday":
                                                nBørnStatistik[angivBarn, 2] = BeregnTid(børnNaturen, angivBarn, afgang);
                                                break;
                                            case "Wednesday":
                                                nBørnStatistik[angivBarn, 3] = BeregnTid(børnNaturen, angivBarn, afgang);
                                                break;
                                            case "Thursday":
                                                nBørnStatistik[angivBarn, 4] = BeregnTid(børnNaturen, angivBarn, afgang);
                                                break;
                                            case "Friday":
                                                nBørnStatistik[angivBarn, 5] = BeregnTid(børnNaturen, angivBarn, afgang);
                                                break;
                                            default:
                                                Fejlmeddelelse();
                                                break;
                                        }

                                        break;
                                    case "2":
                                        angivBarn = AfhentBarn(børnTeknikken);
                                        afgang = DateTime.Now.ToString("HH:mm");
                                        ugedag = tidNu.DayOfWeek.ToString();

                                        switch (ugedag)
                                        {
                                            case "Monday":
                                                tBørnStatistik[angivBarn, 1] = BeregnTid(børnTeknikken, angivBarn, afgang);
                                                break;
                                            case "Tuesday":
                                                tBørnStatistik[angivBarn, 2] = BeregnTid(børnTeknikken, angivBarn, afgang);
                                                break;
                                            case "Wednesday":
                                                tBørnStatistik[angivBarn, 3] = BeregnTid(børnTeknikken, angivBarn, afgang);
                                                break;
                                            case "Thursday":
                                                tBørnStatistik[angivBarn, 4] = BeregnTid(børnTeknikken, angivBarn, afgang);
                                                break;
                                            case "Friday":
                                                tBørnStatistik[angivBarn, 5] = BeregnTid(børnTeknikken, angivBarn, afgang);
                                                break;
                                            default:
                                                Fejlmeddelelse();
                                                break;
                                        }

                                        break;
                                    case "3":
                                        angivBarn = AfhentBarn(børnMennesker);
                                        afgang = DateTime.Now.ToString("HH:mm");
                                        ugedag = tidNu.DayOfWeek.ToString();

                                        switch (ugedag)
                                        {
                                            case "Monday":
                                                mBørnStatistik[angivBarn, 1] = BeregnTid(børnMennesker, angivBarn, afgang);
                                                break;
                                            case "Tuesday":
                                                mBørnStatistik[angivBarn, 2] = BeregnTid(børnMennesker, angivBarn, afgang);
                                                break;
                                            case "Wednesday":
                                                mBørnStatistik[angivBarn, 3] = BeregnTid(børnMennesker, angivBarn, afgang);
                                                break;
                                            case "Thursday":
                                                mBørnStatistik[angivBarn, 4] = BeregnTid(børnMennesker, angivBarn, afgang);
                                                break;
                                            case "Friday":
                                                mBørnStatistik[angivBarn, 5] = BeregnTid(børnMennesker, angivBarn, afgang);
                                                break;
                                            default:
                                                Fejlmeddelelse();
                                                break;
                                        }

                                        break;
                                    case "4":
                                        break;
                                    default:
                                        Fejlmeddelelse();
                                        break;
                                }
                            }
                            break;

                        //Se fremmøde statistik
                        case "2":
                            Console.Write("Statistik for fremmøde\n\n1. Naturen\n2. Teknikken\n3. Mennesker\n\nValg:\t");
                            brugerValg = Console.ReadLine();
                            Console.Clear();

                            switch (brugerValg)
                            {
                                case "1":
                                    Console.Write("Statistik for; Naturen\n\nNavn\t\t\tMan\t\tTirs\t\tOns\t\tTor\t\tFre\n");
                                    PrintStatistik(nBørnStatistik);
                                    break;
                                case "2":
                                    Console.Write("Statistik for; Teknikken\n\nNavn\t\t\tMan\tTirs\tOns\tTor\tFre\n");
                                    PrintStatistik(tBørnStatistik);
                                    break;
                                case "3":
                                    Console.Write("Statistik for; Mennesker\n\nNavn\t\t\tMan\tTirs\tOns\tTor\tFre\n");
                                    PrintStatistik(mBørnStatistik);
                                    break;
                                default:
                                    Fejlmeddelelse();
                                    break;
                            }

                            break;

                        //Ændre kode, forældre
                        case "3":
                            Console.Write("Ændre kode\n\nDit brugernavn: ");
                            brugerinput = Console.ReadLine();
                            forældreBrugere[AktivBruger(forældreBrugere, brugerinput), 1] = Ændrekode(forældreBrugere, brugerinput);
                            break;

                        case "4":
                            forældreLoginKorrekt = false;
                            break;
                        default:
                            Fejlmeddelelse();
                            break;
                    }
                }

                //Personale login
                while (personaleLoginKorrekt == true)
                {
                    Console.Clear();
                    Console.Write("Personale menu\n\n1. Fremmødt\n2. Opret udflugt\n3. Sygemelding\n4. Ændre kode\n4. Log ud\n\nValg:\t");
                    brugerValg = Console.ReadLine();
                    Console.Clear();

                    switch (brugerValg)
                    {
                        //Print fremmødte børn + sygemeldt personale
                        case "1":
                            Console.Write("Hvilken afdeling?\n\n1. Naturen\n2. Teknikken\n3. Mennesker\n4. Personale\n5. Tilbage\n\nAfdeling:\t");
                            brugerValg = Console.ReadLine();

                            switch (brugerValg)
                            {
                                case "1":
                                    FremmødteBørn(børnNaturen);
                                    break;

                                case "2":
                                    FremmødteBørn(børnTeknikken);
                                    break;

                                case "3":
                                    FremmødteBørn(børnMennesker);
                                    break;

                                case "4":
                                    Console.Clear();
                                    Console.Write("Hvilken afdeling skal der printes sygdom for?\n\n1. Naturen\n2. Teknikken\n3. Mennesker\n\nAfdeling:\t");
                                    brugerValg = Console.ReadLine();

                                    if (brugerValg == "1")
                                        PrintSygemeldinger(nPersonaleSygdom);
                                    else if (brugerValg == "2")
                                        PrintSygemeldinger(nPersonaleSygdom);
                                    else if (brugerValg == "3")
                                        PrintSygemeldinger(nPersonaleSygdom);
                                    else
                                        Fejlmeddelelse();
                                    break;

                                case "5":
                                    break;

                                default:
                                    Fejlmeddelelse();
                                    break;
                            }
                            break;

                        //Opret udflugtinformation til personale
                        case "2":
                            Console.Write("Opret udlfugt information\n\n1. Naturen\n2. Teknikken\n3. Mennesker\n4. tilbage\n\nAfdeling:\t");
                            brugerValg = Console.ReadLine();

                            OpretUdlfugt(brugerValg, nPersonaleUdflugtFilsti, tPersonaleUdflugtFilsti, mPersonaleUdflugtFilsti);
                            break;

                        //Sygemelding, personale
                        case "3":
                            Console.Write("Andmeld sygdom\n\n1. Naturen\n2. Teknikken\n3. Mennesker\n4. tilbage\n\nAfdeling:\t");
                            brugerValg = Console.ReadLine();

                            switch (brugerValg)
                            {
                                case "1":
                                    Sygemelding(nPersonaleSygdom, nPersonaleSygdomFilsti);
                                    break;

                                case "2":
                                    Sygemelding(nPersonaleSygdom, tPersonaleSygdomFilsti);
                                    break;

                                case "3":
                                    Sygemelding(nPersonaleSygdom, mPersonaleSygdomFilsti);
                                    break;

                                case "4":
                                    break;

                                default:
                                    Fejlmeddelelse();
                                    break;
                            }
                            break;

                        //Ændre kode, personale
                        case "4":
                            Console.Write("Andre kode\n\nDit brugernavn: ");
                            brugerinput = Console.ReadLine();
                            persoanleBrugere[AktivBruger(persoanleBrugere, brugerinput), 1] = Ændrekode(persoanleBrugere, brugerinput);
                            break;

                        //Log ud
                        case "5":
                            personaleLoginKorrekt = false;
                            Console.Clear();
                            break;

                        default:
                            Fejlmeddelelse();
                            break;
                    }
                }
            }
        }
        //Kontrolerer om login er korrekt
        static bool Login(string[,] modtagetBruger)
        {
            string brugernavn, kodeord;
            bool loginKorrekt = false;

            Console.Write("Brugernavn:\t");
            brugernavn = Console.ReadLine();

            Console.Write("Kodeord:\t");
            kodeord = Console.ReadLine();

            Console.Clear();

            for (int i = 0; i < modtagetBruger.GetLength(0); i++)
            {
                if (modtagetBruger[i, 0] == brugernavn && modtagetBruger[i, 1] == kodeord)
                {

                    Console.Write("login succefuld");
                    loginKorrekt = true;

                    Thread.Sleep(1000);
                    Console.Clear();

                    break;
                }
            }
            return loginKorrekt;
        }

        //Funktion for aflevering af barn
        static int AfleverBarn(string[,] modtagetBørn, string modtagetFilpladsering, string brugerValg)
        {
            int printLinje = 0, linje = 0, angivBarn, barnNr = 0;
            string besked;
            DateTime aflevering;

            Console.Clear();
            List<string> lines = File.ReadAllLines(modtagetFilpladsering).ToList();
            //programmet læser fra en fil og sætter alt i en list                                                                   	 

            foreach (string line in lines)  //laver en foreach for at køre listen igennem
            {
                linje++; //registrer hvor mange linjer der er
                Console.WriteLine(linje + ". " + line); //printer linje nr og hvad der står på den linje
            }

            Console.Write("Hvilket barn der afleveres:\t");

            angivBarn = Convert.ToInt32(Console.ReadLine()); //gemmer valg af barn på baggrund af unik nr
            angivBarn--; //minusser med 1 for at få programmet til at forstå

            foreach (string line in lines) //endnu en foreach der kører listen igennem
            {
                string[] filLinje = line.Split('|'); //her gemmes lsiten i en array (can indeles i flere med |)

                if (angivBarn == printLinje)
                {
                    barnNr = printLinje; //gemmer hvilket barn der afleveres t
                    modtagetBørn[barnNr, 0] = filLinje[0]; //gemmer det afleverede barn i array for  registrerede børn
                }
                printLinje++; //holder styr på hvilken linje/barn programmet er på
            }

            Console.Write("Skal der oprettes en besked? (Y/N): ");
            brugerValg = Console.ReadLine(); //gemmer brugerens valg

            if (brugerValg == "y" || brugerValg == "Y") //kontrolerer om der skal oprettes en besked
            {
                Console.Write("\nAngiv besked: ");
                besked = Console.ReadLine(); //gemmer besked i en string
                modtagetBørn[barnNr, 2] = besked; //gemmer besked i array sammen med det registrerede barn
            }
            else if (brugerValg == "n" || brugerValg == "N")
            { }
            else
                Fejlmeddelelse();  //hvis der ikke indtastes de rigtige, meldes der fejl

            aflevering = DateTime.Now; //opdaterer tiden i en datetime
            modtagetBørn[barnNr, 1] = "Ankommet";  //gemmer ankosmt-tiden i array sammen med barnet

            Console.WriteLine("{0} registreret, kl {1}", modtagetBørn[barnNr, 0], aflevering.ToString("HH:mm"));
            Thread.Sleep(1000);
            Console.Clear();

            return angivBarn; //returnerer nummer på det registrerede barn
        }

        //Funktion for afhntening af barn
        static int AfhentBarn(string[,] modtagetBørn)
        {
            int angivBarn;
            string navn;

            Console.WriteLine("Hvilket barn skal afhentes?");

            for (int i = 0; i < modtagetBørn.GetLength(0); i++)
            {
                if (modtagetBørn[i, 1] == "Ankommet")
                {
                    Console.WriteLine("{0}. {1}", i + 1, modtagetBørn[i, 0]);
                }
            }

            Console.Write("Barn: ");

            angivBarn = Convert.ToInt32(Console.ReadLine());
            angivBarn--;
            navn = modtagetBørn[angivBarn, 0];

            modtagetBørn[angivBarn, 0] = "#";
            modtagetBørn[angivBarn, 1] = "Fraværende";
            modtagetBørn[angivBarn, 2] = "#";

            Console.WriteLine("{0} er nu afhentet", navn);

            return angivBarn;
        }

        //Beregner  tiden til fremmøde statistik
        static string BeregnTid(string[,] modtagetnBørnArray, int modtagetAngivBarn, string modtagetAfgang)
        {
            string tidTotal = "";

            DateTime ankomstDateTime = DateTime.Parse(modtagetnBørnArray[modtagetAngivBarn, 3]);
            DateTime afgangDateTime = DateTime.Parse(modtagetAfgang);
            TimeSpan tidTimespan = afgangDateTime.Subtract(ankomstDateTime);
            tidTotal = tidTimespan.ToString();

            return tidTotal;
        }

        //Printer statistik for fremmøæde inde for afdeling
        static void PrintStatistik(string[,] modtagetArray)
        {
            for (int i = 0; i < modtagetArray.GetLength(0); i++)
            {
                Console.WriteLine("{0}\t\t\t{1}\t{2}\t{3}\t{4}\t{5}", modtagetArray[i, 0], modtagetArray[i, 1], modtagetArray[i, 2], modtagetArray[i, 3], modtagetArray[i, 4], modtagetArray[i, 5]);
            }
            Console.Write("\nTryk enter for at gå tilbage: ");
            Console.ReadLine();
            Console.Clear();
        }

        //Printer registrerede børn inde for afdeling
        static void FremmødteBørn(string[,] modtagetBørn)
        {
            Console.Clear();
            Console.WriteLine("Registrerede børn; \n");
            for (int i = 0; i < modtagetBørn.GetLength(0); i++)
            {
                if (modtagetBørn[i, 1] == "Ankommet")
                {
                    Console.WriteLine("{0}. {1},\t{2},\t{3}.", i + 1, modtagetBørn[i, 0], modtagetBørn[i, 1], modtagetBørn[i, 2], modtagetBørn[1, 3]);
                }
            }
            Console.Write("\nTryk enter for at returene til menu");
            Console.ReadLine();
        }

        //Opretter udflugt information i en .txt fil på filserver
        static void OpretUdflugtInformation(string modtagetFilsti)
        {
            Console.Clear();
            string besked;
            Console.Write("Opret udflugt information\n\nInformation:\t");
            besked = Console.ReadLine(); //gemmer udflugt beksed i string

            List<string> lines = File.ReadAllLines(modtagetFilsti).ToList();
            //sætter alle linjer fra min fil i en list

            lines.Add(besked); //tager string og tilføjer den i listen fra før

            File.WriteAllLines(modtagetFilsti, lines); //tilføjer besked til txt filen
        }

        //Registrerer sygemldt personale
        static void Sygemelding(string[,] modtagetPersonale, string modtagetFilsti)
        {
            Console.Clear();

            int printLinje = 0, linje = 0, angivBarn, personaleNr = 0;

            Console.WriteLine("Hvem skal sygemeldes?\n");
            List<string> lines = File.ReadAllLines(modtagetFilsti).ToList();

            foreach (string line in lines)
            {
                linje++;
                Console.WriteLine(linje + ". " + line);
            }
            Console.Write("\nValg:\t");

            angivBarn = Convert.ToInt32(Console.ReadLine());
            angivBarn--;

            foreach (string line in lines)
            {
                string[] filLinje = line.Split('|');

                if (angivBarn == printLinje)
                {
                    personaleNr = printLinje;
                    modtagetPersonale[personaleNr, 0] = filLinje[0];
                }
                printLinje++;
            }


            Console.WriteLine("{0} registreret som 'sygemeldt'", modtagetPersonale[personaleNr, 0]);
            Thread.Sleep(1000);
            Console.Clear();
        }

        //Printer sygemeldt personale
        static void PrintSygemeldinger(string[,] modtagetPersonale)
        {
            Console.Clear();
            Console.WriteLine("Liste over sygemeldte");

            for (int i = 0; i < modtagetPersonale.GetLength(0); i++)
            {

                Console.WriteLine("{0}. {1}", i + 1, modtagetPersonale[i, 0]);
            }
            Console.Write("Tryk enter for at gå tilbage: ");
            Console.ReadLine();
            Console.Clear();
        }

        //Ændrer kode for brugere
        static string Ændrekode(string[,] modtagetBruger, string modtagetBrugernavn)
        {
            string brugerInput, nyKode, nyKodeGentag;
            int aktivBruger;

            while (true) //While loop for at man kan starte forfra hvis noget går galt
            {
                aktivBruger = AktivBruger(modtagetBruger, modtagetBrugernavn); //finde ud af hvem der er den aktive bruger

                Console.Clear();
                Console.WriteLine("Ændre kode for {0}\n", modtagetBruger[aktivBruger, 0]); //printer beksed til bruger
                Console.Write("nuværende kodeord: ");

                brugerInput = Console.ReadLine(); //gemmer nuværende kodeord

                Console.Clear();

                if (brugerInput == modtagetBruger[aktivBruger, 1])  //kontrolerer om angivet kodeord
                {                                                   //er det samme som aktive brugers kodeord
                    Console.WriteLine("Ændre kode for {0}\n", modtagetBruger[aktivBruger, 0]);
                    Console.Write("Nyt kordeord:\t");

                    nyKode = Console.ReadLine();    //gemmer ny kode i en string

                    Console.Clear();
                    Console.WriteLine("Ændre kode for {0}\n", modtagetBruger[aktivBruger, 0]);
                    Console.Write("Gentag nyt kodeord:\t"); //bruger skal gentage nyt kodeord
                                                            //for at sikre sig at de ikke taster forkert
                    nyKodeGentag = Console.ReadLine(); //nyt koreord gemmes i endnu en string

                    Console.Clear();
                    if (nyKode == nyKodeGentag) //kontrolerer om de 2 nye kodeord passer over ens
                    {
                        modtagetBruger[aktivBruger, 1] = nyKode; //hvis de gør overskrives det gamle kodeord med det nye

                        Console.WriteLine("Ændre kode for {0}\n", modtagetBruger[aktivBruger, 0]);
                        Console.WriteLine("Nyt kodeord gemt."); //giver bruger besked om at det løkkedes
                        Thread.Sleep(1000);
                        Console.Clear();

                        return nyKode; //returnere det nye kodord så det kan opdateres i array hvor brugere info gemmes
                    }
                    else
                        Fejlmeddelelse(); //hvis de 2 nye kodeord ikke passer overens meldes der fejl
                }
                else
                    Fejlmeddelelse(); //hvis brugernavn og gammel kodeord ikke passer meldes der fejl
                Console.Clear();
            }
        }

        //Gemmer hvilken bruger der er logget ind under
        static int AktivBruger(string[,] modtagetBruger, string modtagetBrugernavn)
        {
            int bruger = 0;

            for (int i = 0; i < modtagetBruger.GetLength(0); i++)
            {
                if (modtagetBrugernavn == modtagetBruger[i, 0])
                {
                    bruger = i;
                    break;
                }
            }
            return bruger;
        }

        //Resetter array Børn
        static void ArrayResetBørn(string[,] modtagetNBørn, string[,] modtagetTBørn, string[,] modtagetMBørn)
        {
            for (int i = 0; i < modtagetNBørn.GetLength(0); i++)
            {
                modtagetNBørn[i, 0] = "#";
                modtagetNBørn[i, 0] = "#";
                modtagetTBørn[i, 0] = "#";
                modtagetMBørn[i, 0] = "#";
                for (int k = 0; k < 4; k++)
                {
                    modtagetNBørn[k, 0] = "#";
                    modtagetNBørn[k, 0] = "#";
                    modtagetTBørn[k, 0] = "#";
                    modtagetMBørn[k, 0] = "#";
                }
            }
        }

        //resetter array Personale
        static void ArraysResetPersonale(string[,] modtagetNpersonale, string[,] modtagetTPersonale, string[,] modtagTPersonale)
        {
            for (int i = 0; i < modtagetNpersonale.GetLength(0); i++)
            {
                modtagetNpersonale[i, 0] = "#";
                modtagetTPersonale[i, 0] = "#";
                modtagTPersonale[i, 0] = "#";
                for (int k = 0; k < 2; k++)
                {
                    modtagetNpersonale[0, k] = "#";
                    modtagetTPersonale[0, k] = "#";
                    modtagTPersonale[0, k] = "#";
                }
            }
        }

        //resetter statistik for fremmøde
        static void ArrayResetStatistik(string[,] modtagetNBørnStatistik, string[,] modtagetTBørnStatistik, string[,] modtagetMBørnStatistik)
        {
            string standardTid = "##:##:##";

            for (int i = 0; i < modtagetNBørnStatistik.GetLength(0); i++)
            {
                modtagetNBørnStatistik[i, 0] = "#";
                modtagetNBørnStatistik[i, 1] = standardTid;
                modtagetNBørnStatistik[i, 2] = standardTid;
                modtagetNBørnStatistik[i, 3] = standardTid;
                modtagetNBørnStatistik[i, 4] = standardTid;
                modtagetNBørnStatistik[i, 5] = standardTid;

                modtagetTBørnStatistik[i, 0] = standardTid;
                modtagetTBørnStatistik[i, 1] = standardTid;
                modtagetTBørnStatistik[i, 2] = standardTid;
                modtagetTBørnStatistik[i, 3] = standardTid;
                modtagetTBørnStatistik[i, 4] = standardTid;
                modtagetTBørnStatistik[i, 5] = standardTid;

                modtagetMBørnStatistik[i, 0] = "#########";
                modtagetMBørnStatistik[i, 1] = standardTid;
                modtagetMBørnStatistik[i, 2] = standardTid;
                modtagetMBørnStatistik[i, 3] = standardTid;
                modtagetMBørnStatistik[i, 4] = standardTid;
                modtagetMBørnStatistik[i, 5] = standardTid;
            }
        }

        //Printer fejlmeddelelse
        static void Fejlmeddelelse()
        {
            Console.WriteLine("Der er sket en fejl, prøv igen.");
            Thread.Sleep(1000);
            Console.Clear();
        }

        //Opretter predefinerede brugre til fremvisning 
        static void OpretBrugerePersonale(string[,] modtagetBrugere)
        {
            for (int i = 0; i < modtagetBrugere.GetLength(0); i++)
            {
                modtagetBrugere[i, 1] = "P@ssw0rd";
            }

            modtagetBrugere[0, 0] = "admin";
            modtagetBrugere[1, 0] = "bjarne";
            modtagetBrugere[2, 0] = "amalie";
            modtagetBrugere[3, 0] = "michael";
            modtagetBrugere[4, 0] = "peter";
            modtagetBrugere[5, 0] = "lena";
            modtagetBrugere[6, 0] = "liv";
            modtagetBrugere[7, 0] = "jens";
        }

        //Opretter predefinerede brgere til fremvisning
        static void OpretBrugereForældre(string[,] modtagetBrugere)
        {
            for (int i = 0; i < modtagetBrugere.GetLength(0); i++)
            {
                modtagetBrugere[i, 1] = "P@ssw0rd";
            }

            modtagetBrugere[0, 0] = "admin";
            modtagetBrugere[1, 0] = "nele";
            modtagetBrugere[2, 0] = "lene";
            modtagetBrugere[3, 0] = "anja";
            modtagetBrugere[4, 0] = "annie";
            modtagetBrugere[5, 0] = "michele";
            modtagetBrugere[6, 0] = "trine";
            modtagetBrugere[7, 0] = "maj";
        }

        //Menu for hvilken afdeling udflugt-informationen skal oprettes
        static void OpretUdlfugt(string modtagetBrugerValg, string modtagetNPersonaleUdflugtFilsti, string modtagetTPersonaleUdflugtFilsti, string modtagetMPersonaleUdflugtFilsti)
        {
            switch (modtagetBrugerValg)
            {
                case "1":
                    OpretUdflugtInformation(modtagetNPersonaleUdflugtFilsti);
                    break;

                case "2":
                    OpretUdflugtInformation(modtagetTPersonaleUdflugtFilsti);
                    break;

                case "3":
                    OpretUdflugtInformation(modtagetMPersonaleUdflugtFilsti);
                    break;

                case "4":
                    break;

                default:
                    Fejlmeddelelse();
                    break;
            }
        }
    }
}
