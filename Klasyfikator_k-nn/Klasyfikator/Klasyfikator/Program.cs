using System;
using System.Collections.Generic;
using System.Globalization;

class Klasyfikator
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
    static void Main(string[] args)
    {
        int k = 3;
        string ścieżka = "data.txt";
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

    }
}