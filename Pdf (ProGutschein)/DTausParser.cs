using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTaus
{
    public class DTausCreator
    {
        public DTausCreator()
        {
            System.IO.File.Delete(@"E:\DTAUS0.txt");
            DTausParser parser = new DTausParser();
            string satz = parser.PrepareASatz("0128", "A", "GK", "25050180", "", "Lars Herrmann", "050509", "", "1900411121", "", "", "1");
            
            satz += parser.PrepareCSatz(
                "0187", "C", "25050180", "25050180", "1900411121", "", "51", "000", "", "", "25050180", "1900411121", "121215", "", "Andreas Klocke", "",
                "ANDREAS KLOCKE", "TEST", "1", "", "");

            satz += parser.PrepareESatz("0128", "E", "", "1", "0", "1900411121", "25050180", "121215", "");

            System.IO.File.AppendAllText(@"E:\DTAUS0.txt", satz);
            System.Diagnostics.Debug.WriteLine("FERTIG");
        }

        
    }

    public class DTausParser
    {
        public DTausParser()
        {

        }

        public string PrepareESatz(params string[] CSatz)
        {
            CSatz[0] = CSatz[0].PadRight(4, '0').ToUpper();     //SATZLAENGE
            CSatz[1] = CSatz[1].PadLeft(1, ' ').ToUpper();      //SATZART
            CSatz[2] = CSatz[2].PadLeft(5, ' ').ToUpper();      //RESERVE
            CSatz[3] = CSatz[3].PadLeft(7, '0').ToUpper();     //ABSTIMMUNGSUNTERLAGE
            CSatz[4] = CSatz[4].PadRight(13, '0').ToUpper();    //SUMME DEM BETRAEGE
            CSatz[5] = CSatz[5].PadLeft(17, '0').ToUpper();    //SUMME DER KONTONUMMERN
            CSatz[6] = CSatz[6].PadLeft(17, '0').ToUpper();     //SUMME DER BLZ
            CSatz[7] = CSatz[7].PadLeft(13, '0').ToUpper();     //SUMME DER EURO BETRAEGE
            CSatz[8] = CSatz[8].PadLeft(51, ' ').ToUpper();      //LEERZEICHEN 51

            string CS = string.Empty;
            foreach (string s in CSatz)
                CS += s;

            return CS;
        }

        public string PrepareCSatz(params string[] CSatz)
        {
            CSatz[0] = CSatz[0].PadRight(4, '0').ToUpper();     //SATZLAENGE
            CSatz[1] = CSatz[1].PadLeft(1, ' ').ToUpper();      //SATZART
            CSatz[2] = CSatz[2].PadRight(8, '0').ToUpper();     //BLZ
            CSatz[3] = CSatz[3].PadRight(8, '0').ToUpper();     //BLZ
            CSatz[4] = CSatz[4].PadRight(10, '0').ToUpper();    //KONTONUMMER
            CSatz[5] = CSatz[5].PadRight(13, '0').ToUpper();    //INTERNE KUNDENNR
            CSatz[6] = CSatz[6].PadRight(2, '0').ToUpper();     //TEXTSCHLUESSEL
            CSatz[7] = CSatz[7].PadRight(3, '0').ToUpper();     //ERGAENZUNG
            CSatz[8] = CSatz[8].PadLeft(1, ' ').ToUpper();     //LEERZEICHEN
            CSatz[9] = CSatz[9].PadRight(11, '0').ToUpper();    //BETRAG IN DEM
            CSatz[10] = CSatz[10].PadRight(8, '0').ToUpper();   //BLZ
            CSatz[11] = CSatz[11].PadRight(10, '0').ToUpper();  //KONTO NR
            CSatz[12] = CSatz[12].PadLeft(11, '0').ToUpper();  //BETRAG IN EUR
            CSatz[13] = CSatz[13].PadLeft(3, ' ').ToUpper();    //LEERZEICHEN
            CSatz[14] = CSatz[14].PadRight(27, ' ').ToUpper();   //NAME
            CSatz[15] = CSatz[15].PadLeft(8, ' ').ToUpper();    //LEERZEICHEN

            CSatz[16] = CSatz[16].PadRight(27, ' ').ToUpper();    //NAME
            CSatz[17] = CSatz[17].PadRight(27, ' ').ToUpper();    //VERWENDUNGSZWECK
            CSatz[18] = CSatz[18].PadLeft(1, ' ').ToUpper();    //WAEHRUNG
            CSatz[19] = CSatz[19].PadLeft(2, ' ').ToUpper();    //RESERVE LEERZEICHEN
            CSatz[20] = CSatz[20].PadRight(2, '0').ToUpper();    //ERWEITERUNG
            
            string CS = string.Empty;
            foreach (string s in CSatz)
                CS += s;

            CS += "                                                                     ";

            return CS;
        }

        public string PrepareASatz(params string[] ASatz)
        {
            ASatz[0] = ASatz[0].PadRight(4, '0').ToUpper();
            ASatz[1] = ASatz[1].PadLeft(1, ' ').ToUpper();
            ASatz[2] = ASatz[2].PadLeft(2, ' ').ToUpper();
            ASatz[3] = ASatz[3].PadRight(8, '0').ToUpper();
            ASatz[4] = ASatz[4].PadRight(8, '0').ToUpper();
            ASatz[5] = ASatz[5].PadLeft(27, ' ').ToUpper();
            ASatz[6] = ASatz[6].PadRight(6, '0').ToUpper();
            ASatz[7] = ASatz[7].PadLeft(4, ' ').ToUpper();
            ASatz[8] = ASatz[8].PadRight(10, '0').ToUpper();
            ASatz[9] = ASatz[9].PadRight(10, '0').ToUpper();
            ASatz[10] = ASatz[10].PadLeft(47, ' ').ToUpper();
            ASatz[11] = ASatz[11].PadLeft(1, ' ').ToUpper();

            string AS = string.Empty;
            foreach (string s in ASatz)
                AS += s;

            return AS;
        }



        /// <summary>
        /// Länge = Feldlänge Typ = Feldtyp  
        /// alpha = alphanumerisch, linksbündig nicht belegte Stellen X´20´(Leerzeichen, ASCII 32) 
        /// numerisch = numerische Daten, ungepackt, rechtsbündig mit vorlaufenden Nullen)
        /// </summary>

        public struct ASatz
        {
            public string Satzlaenge;
            public string Satzart;
            public string Kennzeichen;
            public string BLZ;
            public string nullen1;
            public string absender;
            public string erstellungsdatum;
            public string leer1;
            public string kontoAuftrag;
            public string referenz;
            public string leer2;
            public string waehrung;
        }

        public struct CSatz
        {

        }

        public struct ESatz
        {

        }
    }
}
