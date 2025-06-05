using System;
using System.Collections.Generic;
using System.Globalization;

class Zadanie1
{

    static double Euklides(double[] a, double[] b)
    {
        double s = 0;
        for (int i = 0; i < a.Length; i++)
            s += (a[i] - b[i]) * (a[i] - b[i]);
        return Math.Sqrt(s);
    }

    static double odległość_czybyszewa(double[] a, double[] b)
    {
        double m = 0;
        for (int i = 0; i < a.Length; i++)
        {
            double d = Math.Abs(a[i] - b[i]);
            if (d > m) m = d;
        }
        return m;
    }
    static (double[] min, double[] max) Znajdź_Min_i_Maks(List<(double[] cechy, string kategoria)> dane)
    {
        double[] min = (double[])dane[0].cechy.Clone();
        double[] max = (double[])dane[0].cechy.Clone();

        for (int i = 1; i < dane.Count; i++)
        {
            double[] c = dane[i].cechy;
            for (int j = 0; j < c.Length; j++)
            {
                if (c[j] < min[j])
                    min[j] = c[j];
                if (c[j] > max[j])
                    max[j] = c[j];
            }
        }

        return (min, max);
    }

    static void Normalizuj(IEnumerable<double[]> wektory, double[] min, double[] max)
    {
        foreach (var v in wektory)
        {
            for (int i = 0; i < v.Length; i++)
            {
                double zakres = max[i] - min[i];
                if (zakres == 0.0)
                {
                    v[i] = 0.0;
                }
                else
                {
                    v[i] = (v[i] - min[i]) / zakres;
                }
            }
        }
    }

    static string Klasyfikuj(List<(double[] cechy, string kategoria)> dane, double[] wektor, int k, Func<double[], double[], double> metryka)
    {
        List<(double met, string kat)> odleglosci = new List<(double, string)>();
        for (int i = 0; i < dane.Count; i++)
        {
            double odleglosc = metryka(dane[i].cechy, wektor);
            odleglosci.Add((odleglosc, dane[i].kategoria));
        }

        List<string> kategorie = new List<string>();
        for (int i = 0; i < odleglosci.Count; i++)
        {
            string kat = odleglosci[i].kat;
            bool istnieje = false;
            for (int j = 0; j < kategorie.Count; j++)
            {
                if (kategorie[j] == kat)
                {
                    istnieje = true;
                    break;
                }
            }
            if (!istnieje)
            {
                kategorie.Add(kat);
            }
        }

        List<(double suma, string kat)> wyniki = new List<(double, string)>();
        for (int i = 0; i < kategorie.Count; i++)
        {
            string kat = kategorie[i];
            List<double> pasujace_odleglosci = new List<double>();

            for (int j = 0; j < odleglosci.Count; j++)
            {
                if (odleglosci[j].kat == kat)
                {
                    pasujace_odleglosci.Add(odleglosci[j].met);
                }
            }

            for (int m = 0; m < pasujace_odleglosci.Count - 1; m++)
            {
                for (int n = m + 1; n < pasujace_odleglosci.Count; n++)
                {
                    if (pasujace_odleglosci[m] > pasujace_odleglosci[n])
                    {
                        double a = pasujace_odleglosci[m];
                        pasujace_odleglosci[m] = pasujace_odleglosci[n];
                        pasujace_odleglosci[n] = a;
                    }
                }
            }

            double suma = 0.0;
            for (int j = 0; j < k && j < pasujace_odleglosci.Count; j++)
            {
                suma += pasujace_odleglosci[j];
            }

            wyniki.Add((suma, kat));
        }

        double najlepszaSuma = wyniki[0].suma;
        string najlepszaKat = wyniki[0].kat;
        int ileNajlepszych = 1;

        for (int i = 1; i < wyniki.Count; i++)
        {
            if (wyniki[i].suma < najlepszaSuma)
            {
                najlepszaSuma = wyniki[i].suma;
                najlepszaKat = wyniki[i].kat;
                ileNajlepszych = 1;
            }
            else if (Math.Abs(wyniki[i].suma - najlepszaSuma) < 1e-9)
            {
                ileNajlepszych++;
            }
        }

        if (ileNajlepszych > 1)
        {
            return "Nie można określić klasy";
        }
        else
        {
            return najlepszaKat;
        }
    }
    static void Main(string[] args)
    {
        int k = 3;
        string ścieżka = "dane.txt";
        var próbki = new List<(double[] cechy, string kategoria)>();
        foreach (var wiersz in File.ReadAllLines(ścieżka))
        {
            var części = wiersz.Split('\t');
            double[] cechy = new double[części.Length - 1];
            for (int i = 0; i < cechy.Length; i++)
            {
                cechy[i] = double.Parse(części[i], CultureInfo.InvariantCulture);
            }
            string kat = części[części.Length - 1];
            próbki.Add((cechy, kat));
        }

        double[] próba = { 3.4, 1.1, 1.5, 3.2 };
        var (min, max) = Znajdź_Min_i_Maks(próbki);

        List<double[]> tylkoCechy = new List<double[]>();
        for (int i = 0; i < próbki.Count; i++)
        {
            tylkoCechy.Add(próbki[i].cechy);
        }

        Normalizuj(tylkoCechy, min, max);
        Normalizuj(new List<double[]> { próba }, min, max);

        string wynik_Euklides = Klasyfikuj(próbki, próba, k, Euklides);
        string wynik_Czebyszew = Klasyfikuj(próbki, próba, k, odległość_czybyszewa);

        Console.WriteLine("Euklides\tklasa: {0}", wynik_Euklides);
        Console.WriteLine("Odległość czebyszew\tklasa: {0}", wynik_Czebyszew);

    }
}