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
            string osobnik = " ";
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
        foreach(var osobnik in Pula)
        {
            Console.WriteLine("osobnik = {0}", osobnik);
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
        foreach (var i in osobnicy)
        {
            Console.WriteLine("osobnik: {0}", i);
        }
        return osobnicy;
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
        List<(string, double, double)> Pula_zdekodowana = Dekodowanie(tablicaKodowania, Pula, LBnP);
        List<(string, double)> Pula_oceniona = ocena_osobnika(Pula_zdekodowana);
    }
}