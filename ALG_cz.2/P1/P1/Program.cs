using System;
using System.Collections.Generic;
using System.IO;
class Class
{
    static void Wczytanie(double[] daneX, double[] daneY)
    {
        
        try
        {
            StreamReader sr = new StreamReader("C:\\Users\\MATI\\Desktop\\Algorytm\\ALG_cz.2\\sinusik.txt");

            int index = 0;
            while (!sr.EndOfStream)
            {
                string linia = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(linia))
                {
                    string[] czesci = linia.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    daneX[index] = double.Parse(czesci[0]);
                    daneY[index] = double.Parse(czesci[1]);
                    index++;
                }
            }

            sr.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Błąd podczas wczytywania danych: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Wczytywanie danych zakończone.");
        }

    }
    static List<string> Pula_osobnikow (int liczba_osobnikow, int liczba_chromosomow, int liczba_parametrow)
    {
        List<string> Pula = new List<string>();
        int L_B_CH = liczba_chromosomow * liczba_parametrow;
        Random rnd = new Random();
        for (int i=0; i<liczba_osobnikow; i++)
        {
            string osobnik = "";
            for (int j=0; j<L_B_CH; j++)
            {
                int losowa = rnd.Next(0, 2);
                if (losowa == 1)
                {
                    osobnik += '1';
                }
                else if (losowa == 0)
                {
                    osobnik += '0';
                }
            }
            Pula.Add(osobnik);
        }
        foreach(var osobnik in Pula)
        {
            Console.WriteLine(osobnik);
        }
        return Pula;
    }

    static Dictionary<string,double> Tablica_kodowania(int Min, int Max, int liczba_chromosomow)
    {
        Dictionary<string, double> Tablica = new Dictionary<string, double>();
        double ZD = Max - Min;
        double krok = ZD / (Math.Pow(2, liczba_chromosomow) - 1);
        Tablica.Add("0".PadLeft(liczba_chromosomow, '0'), Min);
        double obecnykrok = krok;
        for (int i=1; i < Math.Pow(2,liczba_chromosomow)-1; i++)
        {
            string klucz = Convert.ToString(i, 2).PadLeft(liczba_chromosomow, '0');
            Tablica.Add(klucz, Math.Round(obecnykrok,2));
            obecnykrok += krok;
        }
        Tablica.Add(new string('1', liczba_chromosomow),Max);
        foreach (var i in Tablica)
        {
            Console.WriteLine("{0} = {1}", i.Key, i.Value);
        }
        return Tablica;
    }
    static List<(string, double,double,double)> Dekodowanie(Dictionary<string,double> Tablica, List<string> Pula, int liczba_chromosomow)
    {
        List<(string, string, string, string)> Pula_1 = new List<(string, string, string, string)>();
        List<(string, double, double, double)> Pula_wartosci = new List<(string, double, double, double)>();
        string KodBin_osobnika;
        string pa;
        string pb;
        string pc;
        double Pa;
        double Pb;
        double Pc;
        foreach(var i in Pula)
        {
            pa = i.Substring(0, liczba_chromosomow);
            pb = i.Substring(liczba_chromosomow, liczba_chromosomow);
            pc = i.Substring(liczba_chromosomow * 2, liczba_chromosomow);
            Pula_1.Add((i, pa, pb, pc));
        }
        foreach(var i in Pula_1)
        {
            KodBin_osobnika = i.Item1;
            Pa = Tablica[i.Item2];
            Pb = Tablica[i.Item3];
            Pc = Tablica[i.Item4];
            Pula_wartosci.Add((KodBin_osobnika, Pa, Pb, Pc));
        }
        foreach (var krotka in Pula_wartosci)
        {
            Console.WriteLine("{0}  {1}\t{2}\t{3}", krotka.Item1, krotka.Item2, krotka.Item3, krotka.Item4);
        }
        return Pula_wartosci;
    }

   static double Funkcja_przystosowania(double pa, double pb, double pc, double[] daneX, double[] daneY)
   {
        double suma = 0;
        double x;
        double y;
        double f;
        for (int i=0; i<daneX.Length; i++)
        {
            x = daneX[i];
            y = daneY[i];
            f = pa * Math.Sin(pb * x + pc);
            suma += Math.Pow((y - f), 2);
        }
        return suma;
    }
   
    static void Main(string[] args)
    {
        int Min = 0;
        int Max = 3;
        int liczba_chromosomow = 4;
        int liczba_iteracji = 100;
        int liczba_osobnikow = 13;
        int rozmiar_turnieju = 3;
        int liczba_parametrow = 3;
        double[] daneX = new double[36];
        double[] daneY = new double[36];

        List<string> Pula = Pula_osobnikow(liczba_osobnikow, liczba_chromosomow, liczba_parametrow);
        Dictionary<string, double> Tablica = Tablica_kodowania(Min, Max, liczba_chromosomow);
        List<(string, double, double, double)> Pula_zdekodowana = Dekodowanie(Tablica, Pula, liczba_chromosomow);
        Wczytanie(daneX, daneY);
    }
}