using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    class Program
    {
        static void Main(string[] args)
        {

            int liczbaPopulacji = 40;

            string pathWynik = @"D:\Users\Adrian\Desktop\ww.txt";

           //  int [,] tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\berlin52.txt");

            //   int[,] tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\bays29.txt");

            //   int[,] tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\eil51.txt");

            //  int[,] tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\gr120.txt");

              int[,] tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\pa561.txt");



            //  tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\a280.txt");

            //  tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\eil76.txt");

            //  tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\pr1002.txt");

            int[] ocenyTablicInty = new int[liczbaPopulacji];
 
            int[,] nowaTablica = new int[liczbaPopulacji, tablicaZPliku.GetLength(0)];           

            nowaTablica = StworzTablice(liczbaPopulacji, tablicaZPliku.GetLength(0));

            nowaTablica = PomieszajTablice(nowaTablica);

            ocenyTablicInty = OcenaTablicIntow(liczbaPopulacji, tablicaZPliku, nowaTablica);

            string wynikIntyKoncowy = wynikInty(liczbaPopulacji, nowaTablica, tablicaZPliku, ocenyTablicInty);

            File.WriteAllText(pathWynik, wynikIntyKoncowy);

            Console.WriteLine(wynikIntyKoncowy);

            Console.ReadLine();
        }

        // FUNKCJE


        public static string wynikInty(int liczbaPopulacji, int[,] pop1, int[,] tablicaZPliku, int[] oceny)
        {
            int[,] tablicaPoKrzyzowaniu = new int[pop1.GetLength(0), pop1.GetLength(1)];
            int[,] tablicaPoMutowaniu = new int[pop1.GetLength(0), pop1.GetLength(1)];
            string wynik = "";

       //     var startTime = DateTime.UtcNow;

       //     while(DateTime.UtcNow - startTime < TimeSpan.FromMinutes(2))
            for (int i = 0; i < 20000; i++)
            {

                int[,] pop2 = Selekcja(pop1, oceny, liczbaPopulacji);

                tablicaPoKrzyzowaniu = Krzyzowanie(pop2);

                tablicaPoMutowaniu = Mutacja(tablicaPoKrzyzowaniu);
               
                oceny = OcenaTablicIntow(liczbaPopulacji, tablicaZPliku, tablicaPoMutowaniu);

                pop1 = tablicaPoMutowaniu;

                wynik = theBest(oceny, tablicaPoMutowaniu);

            }

            return wynik;

        }

        public static int [,] WczytajTabliceZPliku(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string liniaTekstuZPliku;
                string[] wczytanaTablicaZPliku;
                int wiersz = 0; 
                
                liniaTekstuZPliku = sr.ReadLine(); // po to co by zniknąć pierwsza liczbe która oznacza liczbe wierszy.

                int[,] gotowaTablicaZPliku = new int[int.Parse(liniaTekstuZPliku), int.Parse(liniaTekstuZPliku)]; // tutaj tworze tablice która będzie gotową wczytaną tablicą z pliku, jej rozmiar to pierwsza linia w pliku

                while ((liniaTekstuZPliku = sr.ReadLine()) != null) //tutaj wczytuje tablice z odleglościami
                {

                    wczytanaTablicaZPliku = liniaTekstuZPliku.Trim().Split(' ');
                    for (int i = 0; i < wczytanaTablicaZPliku.Length; i++)
                    {
                        gotowaTablicaZPliku[wiersz, i] = int.Parse(wczytanaTablicaZPliku[i]);
                        gotowaTablicaZPliku[i, wiersz] = gotowaTablicaZPliku[wiersz, i];
                    }
                    wiersz++;
                }

                return gotowaTablicaZPliku;

            }
        }

        public static int WczytajLiczbePunktow(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string liniaTekstuZPliku;
               
                liniaTekstuZPliku = sr.ReadLine(); // po to co by zniknąć pierwsza liczbe która oznacza liczbe wierszy.

                int liczbaPunktow = Int32.Parse(liniaTekstuZPliku); 

                return liczbaPunktow;

            }
        }

        public static int [,] StworzTablice(int liczbaKolumn, int liczbaWierszy)
        {
            int[,] nowaTablica = new int[liczbaKolumn, liczbaWierszy];

            for (int i = 0; i < nowaTablica.GetLength(0); i++) // tutaj wypełniam ta nowa tablice 
            {
                for (int j = 0; j < nowaTablica.GetLength(1); j++)
                {
                    nowaTablica[i, j] = j;
                }
            }

            return nowaTablica;
        }

        public static int [,] PomieszajTablice(int [,] tablica)
        {
            Random r = new Random();
            for (int i = 0; i < tablica.GetLength(0); i++) // tutaj mieszam ta nowa tablice 
            {
                for (int j = 0; j < tablica.GetLength(1); j++)
                {
                    int randomowaPozycja = r.Next(0,8);
                    int wartoscNaRandomowejPozycji = tablica[i, randomowaPozycja];
                    tablica[i, randomowaPozycja] = tablica[i, j];
                    tablica[i, j] = wartoscNaRandomowejPozycji;
                }
            }
            return tablica;
        }


        public static int[] OcenaTablicIntow(int liczbaPopulacji, int[,] tablicaZPliku, int[,] nowaTablica)
        {
            int pozycjaStartowa = 0;
            int pozycjaKoncowa = 0;
            int ocena = 0;
            int[] ocenyTablicIntow = new int[liczbaPopulacji];

            for (int i = 0; i < liczbaPopulacji; i++)
            {
                for (int j = 0; j < tablicaZPliku.GetLength(0); j++)
                {
                    if (j + 1 < tablicaZPliku.GetLength(1))
                    {
                        pozycjaStartowa = nowaTablica[i, j];
                        pozycjaKoncowa = nowaTablica[i, j + 1];
                    }
                    else
                    {
                        pozycjaStartowa = nowaTablica[i, j];
                        pozycjaKoncowa = nowaTablica[i, 0];
                    }
                    ocena += tablicaZPliku[pozycjaStartowa, pozycjaKoncowa];
                    
                }
                ocenyTablicIntow[i] = ocena;        
                ocena = 0;
            }
            return ocenyTablicIntow;
        }
        
        public static int[,] Selekcja(int[,] inty, int[] inty2,int wielkoscPopulacji)
        {
            int[,] nowaTablicaIntow = new int[wielkoscPopulacji, inty.GetLength(1)];
            int liczbaWylosowana = 0;
            int min = int.MaxValue;
            int indexTablicy = 0;

            Random r = new Random();
            for (int j = 0; j < wielkoscPopulacji; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    int losowyIndex = r.Next(0, wielkoscPopulacji);

                    liczbaWylosowana = inty2[losowyIndex];
                    
                    if(liczbaWylosowana < min)
                    {
                        min = liczbaWylosowana;
                        indexTablicy = losowyIndex;
                    }
                }
               for (int i = 0; i < inty.GetLength(1); i++)
                {
                    nowaTablicaIntow[j, i] = inty[indexTablicy, i];
                }
                min = int.MaxValue;
            }
            return nowaTablicaIntow;
        }

        public static int[,] Krzyzowanie(int[,] inty)
        {
            Random r = new Random();
            int licznik = 0;
            int licznik2 = 0;

                for (int i = 0; i < inty.GetLength(0); i++)
                {
                    if (r.Next(0, 100) < 50)
                    {


                    int losowyPkt = r.Next(inty.GetLength(1));
                    int wielkoscTymczasowejTablicy = inty.GetLength(1) - losowyPkt;
                    int[] tymczasowyMalyInty = new int[wielkoscTymczasowejTablicy];

                    for (int j = losowyPkt; j < inty.GetLength(1); j++)
                    {
                        tymczasowyMalyInty[licznik] = inty[i, j];
                        licznik++;
                    }
                    licznik = 0;
                    Array.Reverse(tymczasowyMalyInty);

                    for (int jj = losowyPkt; jj < inty.GetLength(1); jj++)
                    {
                        inty[i, jj] = tymczasowyMalyInty[licznik2];
                        licznik2++;
                    }
                    licznik2 = 0;
                    }
                }
                return inty;
        }
        
        public static int[,] Mutacja(int[,] indexyDroga2)
        {
            Random r = new Random();
        
            for (int i = 0; i < indexyDroga2.GetLength(0); i++)
            {
                if (r.Next(0, 100) < 90)
                {
                    
                    int losujemyPierwszyIndex = r.Next(10);// lepsze wyniki !! r.Next(indexyDroga2.GetLength(1));

                    int losujemyDrugiIndex = r.Next(10);// lepsze wyniki !! r.Next(indexyDroga2.GetLength(1));

                    int pierwszaWarszosc = indexyDroga2[i, losujemyPierwszyIndex];

                    int drugaWartosc = indexyDroga2[i, losujemyDrugiIndex];

                    indexyDroga2[i, losujemyPierwszyIndex] = drugaWartosc;

                    indexyDroga2[i, losujemyDrugiIndex] = pierwszaWarszosc;

                }
                
            }
          
            return indexyDroga2;
        }

        
        public static string theBest(int[] tablicaDoOceny, int[,] inty)
        {
            int max = int.MaxValue;
            int index = 0;
            string winer = "";

            string[] zwyciezca = new string[1];
            int[] zwyciezca2 = new int[inty.GetLength(1)];

            for(int i = 0; i < tablicaDoOceny.Length; i++)
            {
                if (max > tablicaDoOceny[i])
                {
                    max = tablicaDoOceny[i];
                    index = i;
                }
            }

            for(int i = 0; i < inty.GetLength(1); i++)
            {
                winer += inty[index, i] + "-";
            }

            winer = winer.Substring(0,winer.Length -1) + " " + max;
            return winer;
        }
    }
}
