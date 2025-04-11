using System;
using System.Collections.Generic;

class XOR_Genetyczny
{
    static List<string> Pula_osobnikow(int liczba_osobnikow, int liczba_chromosomow, int liczba_parametrow)
    {
        List<string> Pula = new List<string>();
        int L_B_CH = liczba_chromosomow * liczba_parametrow;
        Random rnd = new Random();
        for (int i = 0; i < liczba_osobnikow; i++)
        {
            string osobnik = "";
            for (int j = 0; j < L_B_CH; j++)
            {
                osobnik += rnd.Next(0, 2);
            }
            Pula.Add(osobnik);
        }
        return Pula;
    }

    static Dictionary<string, double> Tablica_kodowania(int Min, int Max, int liczba_chromosomow)
    {
        Dictionary<string, double> Tablica = new Dictionary<string, double>();
        double krok = (Max - Min) / (Math.Pow(2, liczba_chromosomow) - 1);

        for (int i = 0; i < Math.Pow(2, liczba_chromosomow); i++)
        {
            string klucz = Convert.ToString(i, 2).PadLeft(liczba_chromosomow, '0');
            double wartosc = Min + i * krok;
            Tablica[klucz] = Math.Round(wartosc, 2);
        }

        return Tablica;
    }

    static List<(string, double[])> Dekodowanie(Dictionary<string, double> Tablica, List<string> Pula, int liczba_chromosomow, int liczba_parametrow)
    {
        List<(string, double[])> Wynik = new List<(string, double[])>();

        foreach (var osobnik in Pula)
        {
            double[] wagi = new double[liczba_parametrow];
            for (int i = 0; i < liczba_parametrow; i++)
            {
                string bin = osobnik.Substring(i * liczba_chromosomow, liczba_chromosomow);
                wagi[i] = Tablica[bin];
            }
            Wynik.Add((osobnik, wagi));
        }

        return Wynik;
    }

    static double Funkcja_przystosowania(double[] wagi)
    {
        double[][] wejscia = new double[4][]
        {
            new double[] {0, 0, 1},
            new double[] {0, 1, 1},
            new double[] {1, 0, 1},
            new double[] {1, 1, 1}
        };
        double[] oczekiwane = { 0, 1, 1, 0 };
        double blad = 0.0;

        for (int i = 0; i < 4; i++)
        {
            double suma = 0.0;
            for (int j = 0; j < 3; j++)
                suma += wejscia[i][j] * wagi[j]; 

            for (int j = 0; j < 3; j++)
                suma += wejscia[i][j] * wagi[j + 3]; 

            for (int j = 0; j < 3; j++)
                suma += wejscia[i][j] * wagi[j + 6]; 

            double wyjscie = 1.0 / (1.0 + Math.Exp(-suma)); 
            blad += Math.Pow(oczekiwane[i] - wyjscie, 2);
        }

        return blad;
    }

    static List<(string, double)> Ocen_osobnika(List<(string, double[])> Pula)
    {
        List<(string, double)> Oceny = new List<(string, double)>();
        foreach (var osobnik in Pula)
        {
            double blad = Funkcja_przystosowania(osobnik.Item2);
            Oceny.Add((osobnik.Item1, blad));
        }
        return Oceny;
    }

    static List<string> Turniej(List<(string, double)> Pula, int liczba_osobnikow)
    {
        List<string> nowa_pula = new List<string>();
        Random rnd = new Random();

        for (int i = 0; i < liczba_osobnikow; i++)
        {
            var o1 = Pula[rnd.Next(Pula.Count)];
            var o2 = Pula[rnd.Next(Pula.Count)];
            var o3 = Pula[rnd.Next(Pula.Count)];

            var najlepszy = (string.Empty, double.MaxValue);

            if (o1.Item2 <= o2.Item2 && o1.Item2 <= o3.Item2)
            {
                najlepszy = o1;
            }
            else if (o2.Item2 <= o1.Item2 && o2.Item2 <= o3.Item2)
            {
                najlepszy = o2;
            }
            else
            {
                najlepszy = o3;
            }


            nowa_pula.Add(najlepszy.Item1);
        }

        return nowa_pula;
    }

    static void Krzyzowanie(List<string> pula, int chrom_len, int param_count)
    {
        Random rnd = new Random();
        for (int i = 0; i < pula.Count - 1; i += 2)
        {
            int punkt = rnd.Next(1, chrom_len * param_count - 1);
            string r1 = pula[i];
            string r2 = pula[i + 1];
            string d1 = r1.Substring(0, punkt) + r2.Substring(punkt);
            string d2 = r2.Substring(0, punkt) + r1.Substring(punkt);
            pula[i] = d1;
            pula[i + 1] = d2;
        }
    }

    static List<string> Mutacja(List<string> pula)
    {
        Random rnd = new Random();
        for (int i = 4; i < pula.Count; i++) 
        {
            int index = rnd.Next(pula[i].Length);
            char[] geny = pula[i].ToCharArray();
            if (geny[index] == '0')
            {
                geny[index] = '1';
            }
            else
            {
                geny[index] = '0';
            }

            pula[i] = new string(geny);
        }
        return pula;
    }

    static (string, double) Najlepszy(List<(string, double)> Pula)
    {
        (string, double) najlepszy = Pula[0];
        foreach (var osobnik in Pula)
        {
            if (osobnik.Item2 < najlepszy.Item2)
                najlepszy = osobnik;
        }
        return najlepszy;
    }

    static double Srednia(List<(string, double)> Pula)
    {
        double suma = 0;
        foreach (var o in Pula)
            suma += o.Item2;
        return Math.Round(suma / Pula.Count, 4);
    }

    static void Main(string[] args)
    {
        int Min = -10;
        int Max = 10;
        int liczba_chromosomow = 6;
        int liczba_osobnikow = 13;
        int liczba_iteracji = 100;
        int liczba_parametrow = 9;

        List<string> Pula = Pula_osobnikow(liczba_osobnikow, liczba_chromosomow, liczba_parametrow);
        Dictionary<string, double> Tablica = Tablica_kodowania(Min, Max, liczba_chromosomow);
        var Pula_zdekodowana = Dekodowanie(Tablica, Pula, liczba_chromosomow, liczba_parametrow);
        var Oceny = Ocen_osobnika(Pula_zdekodowana);
        var Najlepszy_osobnik = Najlepszy(Oceny);

        Console.WriteLine($"Najlepszy: {Najlepszy_osobnik.Item2}, Średnia: {Srednia(Oceny)}");

        for (int i = 0; i < liczba_iteracji; i++)
        {
            var nowa_pula = Turniej(Oceny, liczba_osobnikow);
            Krzyzowanie(nowa_pula, liczba_chromosomow, liczba_parametrow);
            nowa_pula = Mutacja(nowa_pula);

            var dekodowani = Dekodowanie(Tablica, nowa_pula, liczba_chromosomow, liczba_parametrow);
            var oceny = Ocen_osobnika(dekodowani);
            oceny.Add(Najlepszy_osobnik);

            Najlepszy_osobnik = Najlepszy(oceny);
            Console.WriteLine($"Iteracja {i + 1}  Najlepszy: {Najlepszy_osobnik.Item2}, Średnia: {Srednia(oceny)}");
            Oceny = oceny;
        }

        Console.WriteLine($"Najlepszy osobnik: {Najlepszy_osobnik.Item1}, Przystosowanie: {Najlepszy_osobnik.Item2}");
    }
}
