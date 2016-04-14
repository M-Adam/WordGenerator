using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordGenerator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Podaj oddzielone spacją litery, z których mają składać się słowa:");
            string input = Console.ReadLine();
            bool? compareToDictionary=null;

            string[] literki = input.Split(' ');
            
            int mozliwychKombinacji = Silnia(literki.Length);
            Console.WriteLine("Możliwych słów do ułożenia: " + mozliwychKombinacji.ToString() + 
                "\nCzy chcesz porównywać generowane słowa ze słownikiem?\n1. Tak\n2. Nie");

            while (compareToDictionary == null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        compareToDictionary = true;
                        break;
                    case ConsoleKey.D2:
                        compareToDictionary = false;
                        break;
                    default:
                        compareToDictionary = null;
                        Console.WriteLine("\nWybierz 1 albo 2.\n");
                        break;
                }
            }



            Console.WriteLine("Trwa układanie wszelkich możliwych słów.");

            Random r = new Random();
            List<int> juzWylosowane = new List<int>();
            StringBuilder sb = new StringBuilder(String.Empty);
            List<string> ulozoneSlowa = new List<string>();

            for (int j = 0; j < mozliwychKombinacji; j++)
            {
                sb.Clear();
                juzWylosowane.Clear();

                for (int i = 0; i < literki.Length; i++)
                {
                    int los = r.Next(0, literki.Length);
                    if (juzWylosowane.Contains(los))
                        i--;
                    else
                    {
                        juzWylosowane.Add(los);
                        sb.Append(literki[los]);
                    }
                }

                if (ulozoneSlowa.Contains(sb.ToString()))
                    j--;
                else
                    ulozoneSlowa.Add(sb.ToString());
            }

            ulozoneSlowa.Sort();

            if (compareToDictionary == true)
            {
                List<string> slownik = new List<string>(2709887);
                Console.WriteLine("Trwa wczytywanie slownika...");
                wczytajSlownik(ref slownik);
                Console.WriteLine("Koniec wczytywania, szukanie powtórzeń");
                List<string> powtorzenia = new List<string>();
                foreach (string slowo in ulozoneSlowa)
                {
                    if (slownik.Contains(slowo))
                        powtorzenia.Add(slowo);
                }
                Console.WriteLine("Ułożone słowa: ");
                foreach (string slowo in powtorzenia)
                    Console.WriteLine(slowo);
            }
            else
            {
                Console.WriteLine("Ułożone słowa: ");
                foreach (string slowo in ulozoneSlowa)
                    Console.WriteLine(slowo);
            }
        }

        private static void wczytajSlownik(ref List<string> tabliczka)
        {
            using (StreamReader sr = new StreamReader("slowa-win.txt"))
            {
                string slowo;
                while ((slowo = sr.ReadLine()) != null)
                {
                    tabliczka.Add(slowo);
                }
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static Func<int, int> Silnia = x => x < 0 ? -1 : x == 1 || x == 0 ? 1 : x * Silnia(x - 1);
    }
}
