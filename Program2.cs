using System;
using System.Collections.Generic;

class Class
{
    static Dictionary<string, double> Tablica_kodowania(int Min, int Max, int LBnP)
    {
        Dictionary<string, double> Tabela = new Dictionary<string, double>();
        double ZD = Max - Min;
        double krok = ZD / (Math.Pow(2, LBnP) - 1);

        Tabela.Add("0".PadLeft(LBnP, '0'), Min);
        double obecnyKrok = krok;

        for (int i = 1; i < Math.Pow(2, LBnP) - 1; i++)
        {
            string klucz = Convert.ToString(i, 2).PadLeft(LBnP, '0');
            Tabela.Add(klucz, Math.Round(obecnyKrok, 2));
            obecnyKrok += krok;
        }
        Tabela.Add(new string('1', LBnP), Max);

        foreach (var val in Tabela)
        {
            Console.WriteLine("{0} = {1}", val.Key, val.Value);
        }
        return Tabela;
    }

    static double Funkcja_przystosowania(double x1, double x2)
    {
        return Math.Round(Math.Sin(x1 * 0.05) + Math.Sin(x2 * 0.05) + 0.4 * Math.Sin(x1 * 0.15) * Math.Sin(x2 * 0.15), 2);
    }
    static List<string> Pula_osobnikow(int liczba_osobnikow, int liczba_parametrow, int LBnP)
    {
        List<string> Pula = new List<string>();
        Random rnd = new Random();
        int L_B_CH = LBnP * liczba_parametrow;

        for (int i=0; i<liczba_osobnikow; i++)
        {
            string osobnik = "";
            for (int j=0; j<L_B_CH; j++)
            {
                int losowanie = rnd.Next(0, 2);
                if (losowanie == 1)
                {
                    osobnik += "1";
                }
                else if(losowanie == 0)
                {
                    osobnik += "0";
                }
            }
            Pula.Add(osobnik);
        }
        return Pula;

    }
    static List<(string,double,double)> Dekodowanie (Dictionary<string,double> Tabela, List<string> Pula, int LBnP)
    {
        List<(string, string, string)> PulaX = new List<(string, string, string)>();
        List<(string, double, double)> Pula_wartosci = new List<(string, double, double)>();
        string KodBin_osobnika;
        string x1;
        string x2;
        double X1;
        double X2;

        foreach(var osobnik in Pula)
        {
            x1 = osobnik.Substring(0, LBnP);
            x2 = osobnik.Substring(LBnP);
            PulaX.Add((osobnik, x1, x2));
        }
        foreach(var osobnik in PulaX)
        {
            KodBin_osobnika = osobnik.Item1;
            X1 = Tabela[osobnik.Item2];
            X2 = Tabela[osobnik.Item3];
            Pula_wartosci.Add((KodBin_osobnika, X1, X2));
        }
        return Pula_wartosci;
    }
    static List<(string,double)> ocena_osobnika (List<(string,double,double)> Pula_zdekodowana)
    {
        List<(string, double)> osobnicy = new List<(string, double)>();
        foreach(var krotka in Pula_zdekodowana)
        {
            osobnicy.Add((krotka.Item1, Funkcja_przystosowania(krotka.Item2, krotka.Item3)));
        }
        return osobnicy;
    }
    static (string, double) najlepszy_z_puli(List<(string,double)> Pula)
    {
        (string, double) najlepszy = Pula[0];
        foreach(var krotka in Pula)
        {
            if (krotka.Item2 > najlepszy.Item2)
            {
                najlepszy = krotka;
            }
        }
        Console.WriteLine(najlepszy);
        return najlepszy;
    }
    static List<(string,double)> Turniej (List<(string,double)> Pula, int liczba_osobnikow)
    {
        List<(string, double)> nowa_pula = new List<(string, double)>();
        Random rnd = new Random();

        for (int i=0; i<liczba_osobnikow-1; i++)
        {
            int osobnik1 = rnd.Next(0, liczba_osobnikow);
            int osobnik2 = rnd.Next(0, liczba_osobnikow);
            if (Pula[osobnik1].Item2 >= Pula[osobnik2].Item2)
            {
                nowa_pula.Add(Pula[osobnik1]);
            }
            else if (Pula[osobnik1].Item2< Pula[osobnik2].Item2)
            {
                nowa_pula.Add(Pula[osobnik2]);
            }
        }
        return nowa_pula;
    }
    static List<string> Mutator (List<(string,double)> Pula)
    {
        List<string> zmutowane = new List<string>();
        Random rnd = new Random();

        foreach(var osobnik in Pula)
        {
            int b = rnd.Next(0, osobnik.Item1.Length);
            string początek = osobnik.Item1.Substring(0, b);
            string koniec = osobnik.Item1.Substring(b + 1);
            char nowy_Bit = new char();
            if (osobnik.Item1[b] == '1')
            {
                nowy_Bit = '0';
            }
            else if (osobnik.Item1[b] == '0')
            {
                nowy_Bit = '0';
            }
            string zmutowanyOsobnik = początek + nowy_Bit + koniec;
            zmutowane.Add(zmutowanyOsobnik);
        }
        return zmutowane;
    }
    static double srednia (List<(string,double)> Pula, int liczba_osobnikow)
    {
        double średnia = 0;
        double suma = 0;
        for (int i=0; i<liczba_osobnikow-1; i++)
        {
            suma += Pula[i].Item2;
        }
        średnia = Math.Round(suma / liczba_osobnikow, 2);
        return średnia;
    }
    static void wyswietl_osobnikow(List<string> Pula)
    {
        foreach (var i in Pula)
        {
            Console.WriteLine("osobnik: {0}", i);
        }
    }
    static void wyswietl_osobnikow(List<(string, double, double)> Pula_osobnikow)
    {
        foreach (var krotka in Pula_osobnikow)
        {
            Console.WriteLine("{0}  {1}\t{2}", krotka.Item1, krotka.Item2, krotka.Item3);
        }
    }
    static void wyswietl_osobnikow(List<(string, double)> osobnicy)
    {
        foreach (var krotka in osobnicy)
        {
            Console.WriteLine("osobnik: {0}, ocena: {1}", krotka.Item1, krotka.Item2);
        }
    }
    static void Main(string[] args)
    {
        int Min = 0;
        int Max = 100;
        int LBnP = 3;
        int liczba_parametrow = 2;
        int liczba_osobnikow = 9;
        int liczba_iteracji = 20;

        Dictionary<string, double> tablicaKodowania = Tablica_kodowania(Min, Max, LBnP);
        List<string> Pula = Pula_osobnikow(liczba_osobnikow, liczba_parametrow, LBnP);
        Console.WriteLine("Pula osobników\t");
        wyswietl_osobnikow(Pula);
        Console.WriteLine("\t");
        List<(string, double, double)> Pula_zdekodowana = Dekodowanie(tablicaKodowania, Pula, LBnP);
        Console.WriteLine("Pula osobników zdekodowananych\t");
        wyswietl_osobnikow(Pula_zdekodowana);
        Console.WriteLine("\t");
        List<(string, double)> Pula_oceniona = ocena_osobnika(Pula_zdekodowana);
        Console.WriteLine("Pula oceniona\t");
        wyswietl_osobnikow(Pula_oceniona);
        Console.WriteLine("\t");
        (string, double) najlepszy = najlepszy_z_puli(Pula_oceniona);
        Console.WriteLine("Najlepiej dostosowany: {0}, {1} \t średnia dostosowania: {2}\n", najlepszy.Item1, najlepszy.Item2, srednia(Pula_oceniona, liczba_osobnikow));

        for (int i = 0; i < liczba_iteracji; i++)
        {
            List<(string, double)> nowa_Pula = Turniej(Pula_oceniona, liczba_osobnikow);
            List<string> nowa_po_mutacji = Mutator(nowa_Pula);
            List<(string, double, double)> nowa_Pula_dekodowanie = Dekodowanie(tablicaKodowania, nowa_po_mutacji, LBnP);
            List<(string, double)> nowa_Pula_oceniona = ocena_osobnika(nowa_Pula_dekodowanie);
            nowa_Pula_oceniona.Add(najlepszy);
            wyswietl_osobnikow(nowa_Pula_oceniona);
            najlepszy = najlepszy_z_puli(nowa_Pula_oceniona);
            Console.WriteLine("Najlepiej dostosowany: {0}, {1} \t średnia dostosowania: {2}\n", najlepszy.Item1, najlepszy.Item2, srednia(nowa_Pula_oceniona, liczba_osobnikow));
            Pula_oceniona = nowa_Pula_oceniona;
        }
    }
}