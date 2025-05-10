using System;
using System.Collections.Generic;

class Zadanie2
{
    static List<(int neurony, int wejscia)> LiczbaNeuronow()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (2, 2),
            (2, 2),
            (2, 2)
        };
        return Liczba_neuronów;
    }
    static double Funkcja(double x, int beta)
    {
        return 1.0 / (1.0 + Math.Exp(-beta * x));
    }
    static (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag(List<(int neurony, int wejscia)> liczbaNeuronow)
    {
        List<List<List<double>>> Wagi = new List<List<List<double>>>();
        List<List<double>> Bias = new List<List<double>>();
        var rnd = new Random();

        foreach (var (neurony, wejścia) in liczbaNeuronow)
        {
            List<List<double>> wagiWarstwy = new List<List<double>>();
            List<double> biasWarstwy = new List<double>();
            for (int i = 0; i < neurony; i++)
            {
                List<double> wagiNeuronu = new List<double>();
                for (int j = 0; j < wejścia; j++)
                {
                    wagiNeuronu.Add(rnd.Next(-5, 6));
                }
                wagiWarstwy.Add(wagiNeuronu);
                biasWarstwy.Add(rnd.Next(-5, 6));
            }
            Wagi.Add(wagiWarstwy);
            Bias.Add(biasWarstwy);
        }
        return (Wagi, Bias);
    }
    static List<List<double>> Propagacja(List<List<List<double>>> Wagi, List<List<double>> Bias, List<double> wejscia, int beta)
    {
        List<List<double>> wyjscia = new List<List<double>>();
        List<double> do_wejsc = new List<double>(wejscia);
        wyjscia.Add(new List<double>(do_wejsc));

        for (int l = 0; l < Wagi.Count; l++)
        {
            var next = new List<double>();
            for (int n = 0; n < Wagi[l].Count; n++)
            {
                double suma = Bias[l][n];
                for (int i = 0; i < do_wejsc.Count; i++)
                {
                    suma += do_wejsc[i] * Wagi[l][n][i];
                }
                next.Add(Funkcja(suma, beta));
            }
            wyjscia.Add(next);
            do_wejsc = next;
        }
        return wyjscia;
    }
    static void Sieci(List<(int x1, int x2, int y1, int y2)> probki, List<List<List<double>>> Wagi, List<List<double>> Bias, int beta, double współczynnik, int liczbaEpok)
    {
        int Liczba_warstw = Wagi.Count;
        for (int epoka = 0; epoka < liczbaEpok; epoka++)
        {
            double sumarycznyBlad = 0.0;

            foreach (var (x1, x2, y1, y2) in probki)
            {
                List<double> wejscia = new List<double> { x1, x2 };
                List<double> koniec = new List<double> { y1, y2 };

                var wyjscia = Propagacja(Wagi, Bias, wejscia, beta);

                List<List<double>> D = new List<List<double>>();
                for (int i = 0; i < wyjscia.Count; i++)
                {
                    D.Add(new List<double>(new double[wyjscia[i].Count]));
                }

                int ostatni_element = Liczba_warstw;
                for (int n = 0; n < wyjscia[ostatni_element].Count; n++)
                {
                    double a2 = koniec[n] - wyjscia[ostatni_element][n];
                    D[ostatni_element][n] = a2 * beta * wyjscia[ostatni_element][n] * (1 - wyjscia[ostatni_element][n]);
                    sumarycznyBlad += Math.Abs(a2);
                }

                for (int l = Liczba_warstw - 1; l >= 1; l--)
                {
                    for (int n = 0; n < wyjscia[l].Count; n++)
                    {
                        double Suma = 0.0;
                        for (int k = 0; k < Wagi[l].Count; k++)
                        {
                            Suma += D[l + 1][k] * Wagi[l][k][n];
                        }

                        D[l][n] = Suma * beta * wyjscia[l][n] * (1 - wyjscia[l][n]);
                    }
                }

                for (int l = 0; l < Liczba_warstw; l++)
                {
                    var poprzednie_wyjscie = wyjscia[l];
                    for (int n = 0; n < Wagi[l].Count; n++)
                    {
                        for (int i = 0; i < poprzednie_wyjscie.Count; i++)
                        {
                            Wagi[l][n][i] += współczynnik * D[l + 1][n] * poprzednie_wyjscie[i];
                        }

                        Bias[l][n] += współczynnik * D[l + 1][n];
                    }
                }
            }

            Wyswietlenie(epoka + 1, sumarycznyBlad);
            if (sumarycznyBlad < 0.4)
            {
                break;
            }
        }
    }
    static void TestowanieSieci(List<(int x1, int x2, int y1, int y2)> probki, List<List<List<double>>> Wagi, List<List<double>> Bias, int beta)
    {
        foreach ((int x1, int x2, int y1, int y2) in probki)
        {
            var output = Propagacja(Wagi, Bias, new List<double> { x1, x2 }, beta);
            var koniec = output[output.Count - 1];
            Wyswietlenie(x1, x2, y1, y2);
            Console.WriteLine("sieć: {0:F2},{1:F2}", koniec[0], koniec[1]);

        }

    }
    static void Wyswietlenie(int wejscie1, int wejscie2, int wyjscie1, int wyjscie2)
    {
        Console.WriteLine("Wejście: {0} : {1}   pożądana wartość wyjściowa: {2} {3} ", wejscie1, wejscie2, wyjscie1, wyjscie2);
    }
    static void Wyswietlenie(int epoka, double suma)
    {
        Console.WriteLine("Epoka: {0}, Błąd: {1:F4}", epoka + 1, suma);
    }

    static void Main()
    {
        int beta = 1;
        double współczynnik = 0.3;
        int liczbaEpok = 50000;

        var probki = new List<(int, int, int, int)>
        {
            (0,0, 0,1),
            (0,1, 1,0),
            (1,0, 1,0),
            (1,1, 0,0)
        };

        var liczbaNeuronow = LiczbaNeuronow();
        var (Wagi, Bias) = GenerowanieWag(liczbaNeuronow);

        Sieci(probki, Wagi, Bias, beta, współczynnik, liczbaEpok);
        TestowanieSieci(probki, Wagi, Bias, beta);
    }
}