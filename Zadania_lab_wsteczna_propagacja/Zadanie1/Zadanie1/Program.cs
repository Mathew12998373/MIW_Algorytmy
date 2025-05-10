using System;
using System.Collections.Generic;

class SiecNeuronowa
{
    static List<(int neurony, int wejscia)> LiczbaNeuronow()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (2, 2),
            (1, 2)
        };
        foreach (var i in Liczba_neuronów)
        {
            Console.WriteLine("Warstwa : {0} , wejścia = {1}", i.Item1, i.Item2);
        }

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
    static List<double> Propagacja((List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag, List<double> wejscia, int beta)
    {
        List<double> wyjscia = new List<double>(wejscia);
        for (int l = 0; l < GenerowanieWag.Wagi.Count; l++)
        {
            List<double> noweWyjscia = new List<double>();
            for (int n = 0; n < GenerowanieWag.Wagi[l].Count; n++)
            {
                double suma = GenerowanieWag.Bias[l][n];
                for (int i = 0; i < wyjscia.Count; i++)
                {
                    suma += wyjscia[i] * GenerowanieWag.Wagi[l][n][i];
                }
                noweWyjscia.Add(Funkcja(suma, beta));
            }
            wyjscia = noweWyjscia;
        }
        return wyjscia;
    }
    static void Sieci(List<(int x1, int x2, int y)> probki, (List<List<List<double>>> Wagi, List<List<double>> Bias) siec, int beta, double wspolczynnik, int liczbaEpok)
    {
        int Liczba_warstw = siec.Wagi.Count;

        for (int epoka = 0; epoka < liczbaEpok; epoka++)
        {
            double sumarycznyBlad = 0.0;

            foreach (var (x1, x2, x3) in probki)
            {
                List<double> wejscia = new List<double> { x1, x2 };
                List<List<double>> wyjsciaWarstw = new List<List<double>>();
                List<double> aktualne = new List<double>(wejscia);
                for (int l = 0; l < Liczba_warstw; l++)
                {
                    List<double> Wyjscie_L = new List<double>();
                    for (int n = 0; n < siec.Wagi[l].Count; n++)
                    {
                        double suma = siec.Bias[l][n];
                        for (int i = 0; i < aktualne.Count; i++)
                        {
                            suma += aktualne[i] * siec.Wagi[l][n][i];
                        }
                        Wyjscie_L.Add(Funkcja(suma, beta));
                    }
                    wyjsciaWarstw.Add(Wyjscie_L);
                    aktualne = Wyjscie_L;
                }
                double y_Sieci = aktualne[0];
                double blad = x3 - y_Sieci;
                sumarycznyBlad += Math.Abs(blad);
                var D = new List<double>[Liczba_warstw];
                for (int i = 0; i < Liczba_warstw; i++)
                    D[i] = new List<double>(new double[siec.Wagi[i].Count]);

                int ostatnia = Liczba_warstw - 1;
                for (int j = 0; j < siec.Wagi[ostatnia].Count; j++)
                {
                    double o = wyjsciaWarstw[ostatnia][j];
                    D[ostatnia][j] = blad * beta * o * (1 - o);
                }

                for (int l = Liczba_warstw - 2; l >= 0; l--)
                {
                    for (int i = 0; i < siec.Wagi[l].Count; i++)
                    {
                        double Suma = 0.0;
                        for (int k = 0; k < siec.Wagi[l + 1].Count; k++)
                        {
                            Suma += siec.Wagi[l + 1][k][i] * D[l + 1][k];
                        }

                        double o = wyjsciaWarstw[l][i];
                        D[l][i] = Suma * beta * o * (1 - o);
                    }
                }
                for (int l = 0; l < Liczba_warstw; l++)
                {
                    List<double> poprzednie_wyjscie;
                    if (l == 0)
                        poprzednie_wyjscie = wejscia;
                    else
                        poprzednie_wyjscie = wyjsciaWarstw[l - 1];

                    for (int n = 0; n < siec.Wagi[l].Count; n++)
                    {
                        for (int i = 0; i < poprzednie_wyjscie.Count; i++)
                        {
                            siec.Wagi[l][n][i] += wspolczynnik * D[l][n] * poprzednie_wyjscie[i];
                        }
                        siec.Bias[l][n] += wspolczynnik * D[l][n];
                    }
                }
            }

            Wyswietlenie(epoka + 1, sumarycznyBlad);
            if (sumarycznyBlad < 0.3)
            {
                break;
            }
        }
    }
    static void TestowanieSieci(List<(int x1, int x2, int y)> probki, (List<List<List<double>>> Wagi, List<List<double>> Bias) siec, int beta)
    {
        foreach (var (x1, x2, y) in probki)
        {
            var output = Propagacja(siec, new List<double> { x1, x2 }, beta);
            Wyswietlenie(x1, x2, y, output[0]);
        }
    }
    static void Wyswietlenie(int wejscie1, int wejscie2, double wartość_wyjściowa, double wyjscie)
    {
        Console.WriteLine("Wejście: {0} : {1}   pożądana wartość wyjściowa: {2} \n Wyjście: {3:F2} ", wejscie1, wejscie2, wartość_wyjściowa, wyjscie);
    }
    static void Wyswietlenie(int epoka, double suma)
    {
        Console.WriteLine("Epoka: {0}, Błąd: {1:F4}", epoka + 1, suma);
    }

    static void Main()
    {
        int beta = 1;
        double wspolczynnik = 0.3;
        int liczbaEpok = 50000;
        List<(int, int, int)> probki = new List<(int, int, int)>
        {
            (0,0,0),
            (0,1,1),
            (1,0,1),
            (1,1,0)
        };
        var liczbaNeuronow = LiczbaNeuronow();
        var generowanieWag = GenerowanieWag(liczbaNeuronow);
        Console.WriteLine("Sieci: ");
        Sieci(probki, generowanieWag, beta, wspolczynnik, liczbaEpok);
        TestowanieSieci(probki, generowanieWag, beta);
    }
}