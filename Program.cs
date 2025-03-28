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

    static void Main(string[] args)
    {
        int Min = 0;
        int Max = 100;
        int LBnP = 3;
        int liczba_parametrow = 2;
        int liczba_osobnikow = 9;
        int liczba_iteracji = 20;

        Dictionary<string, double> tabelaKodowania = Tablica_kodowania(Min, Max, LBnP);
    }
}