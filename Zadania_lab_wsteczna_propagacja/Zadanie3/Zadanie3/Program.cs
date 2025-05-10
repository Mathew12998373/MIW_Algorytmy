using System;
using System.Collections.Generic;

class Zadanie3
{
    static List<(int, int)> LiczbaNeuronow()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (3, 3),
            (2, 3),
            (2, 2)  
        };
        return Liczba_neuronów;
    }

    static double Funkcja(double x, int beta)
    {
        return 1.0 / (1.0 + Math.Exp(-beta * x));
    }

    static (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag(List<(int neurony, int wejscia)> LiczbaNeuronow)
    {
        List<List<List<double>>> Wagi = new List<List<List<double>>>();
        List<List<double>> Bias = new List<List<double>>();
        var rnd = new Random();

        foreach (var (neurony, wejścia) in LiczbaNeuronow)
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
    static List<List<double>> Propagacja(List<List<List<double>>> Wagi,List<List<double>> Bias,List<double> wejscia, int beta)
    {
        List<List<double>> wyjscia = new List<List<double>>();
        List<double> do_wejsc = new List<double>(wejscia);
        wyjscia.Add(new List<double>(do_wejsc));

        for (int l = 0; l < Wagi.Count; l++)
        {
            List<double> kolejne = new List<double>();
            for (int n = 0; n < Wagi[l].Count; n++)
            {
                double suma = Bias[l][n];
                for (int i = 0; i < do_wejsc.Count; i++)
                {
                    suma += do_wejsc[i] * Wagi[l][n][i];
                }
                    
                kolejne.Add(Funkcja(suma, beta));
            }
            wyjscia.Add(kolejne);
            do_wejsc = kolejne;
        }
        return wyjscia;
    }
    static void Sieci(List<(int x1, int x2, int x3, int Suma_ostatni, int wyjscie_ostatni)> probki,List<List<List<double>>> Wagi,List<List<double>> Bias, int beta, double współczynnik, int liczbaEpok)
    {
        int Liczba_warstw = Wagi.Count;
        for (int epoka = 0; epoka < liczbaEpok; epoka++)
        {
            double sumarycznyBlad1 = 0.0;
            double sumarycznyBlad2 = 0.0;

            foreach (var (x1, x2, x3, Suma_ostatni, wyjscie_ostatni) in probki)
            {
                List<double> wejscia = new List<double> { x1, x2, x3 };
                List<double> koniec = new List<double> { Suma_ostatni, wyjscie_ostatni };
                var wyjscia = Propagacja(Wagi, Bias, wejscia, beta);
                List<List<double>> D = new List<List<double>>();
                for (int i = 0; i < wyjscia.Count; i++)
                {
                    D.Add(new List<double>(new double[wyjscia[i].Count]));
                }
                int ostatni_element = Liczba_warstw;
                for (int n = 0; n < wyjscia[ostatni_element].Count; n++)
                {
                    double a1 = wyjscia[ostatni_element][n];
                    double a2 = koniec[n] - a1;
                    D[ostatni_element][n] = a2 * beta * a1 * (1 - a1);
                    if (n == 0)
                    {
                        sumarycznyBlad1 += Math.Abs(a2);
                    }
                    else
                    {
                        sumarycznyBlad2 += Math.Abs(a2);
                    }
                }

                for (int l = Liczba_warstw - 1; l >= 0; l--)
                {
                    for (int n = 0; n < wyjscia[l].Count; n++)
                    {
                        double Suma = 0.0;
                        for (int k = 0; k < D[l + 1].Count; k++)
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
            Wyswietlenie(epoka + 1);
            if (sumarycznyBlad1 < 0.01 && sumarycznyBlad2 < 0.01)
            {
                break;
            }
        }
    }
    static void TestowanieSieci(List<(int x1, int x2, int x3, int Suma_ostatni, int wyjscie_ostatni)> probki,List<List<List<double>>> Wagi,List<List<double>> Bias, int beta)
    {
        foreach ((int x1, int x2, int x3, int Suma_ostatni, int wyjscie_ostatni) in probki)
        {
            List<double> wejscia = new List<double> { x1, x2, x3 };
            var output = Propagacja(Wagi, Bias, wejscia, beta);
            var koniec = output[output.Count - 1];
            Wyswietlenie(x1, x2, x3, Suma_ostatni, wyjscie_ostatni);
            Console.WriteLine("pożądana wartość wyjściowa1: {0:F2} , pożądana wartość wyjściowa2: {1:F2}", koniec[0], koniec[1]);
        }

    }
    static void Wyswietlenie(int wejscie1, int wejscie2, int wejscie3,int Suma_ostatni, int wyjscie_ostatni)
    {
        Console.WriteLine("Wejście: {0} : {1} : {2}  wyjscie1: {3}  wyjscie2: {4}", wejscie1, wejscie2, wejscie3, Suma_ostatni, wyjscie_ostatni);
    }
    static void Wyswietlenie(int epoka)
    {
        Console.WriteLine("Epoka: {0}", epoka + 1);
    }

    static void Main()
    {
        int beta = 1;
        double współczynnik = 0.5;
        int liczbaEpok = 20000;
        var probki = new List<(int, int, int, int, int)>
        {
            (0,0,0, 0,0),
            (0,0,1, 1,0),
            (0,1,0, 1,0),
            (0,1,1, 0,1),
            (1,0,0, 1,0),
            (1,0,1, 0,1),
            (1,1,0, 0,1),
            (1,1,1, 1,1)
        };

        var liczbaNeuronow = LiczbaNeuronow();
        var (Wagi, Bias) = GenerowanieWag(liczbaNeuronow);
        Console.WriteLine("Sieci: ");
        Sieci(probki, Wagi, Bias, beta, współczynnik, liczbaEpok);
        TestowanieSieci(probki, Wagi, Bias, beta);
    }
}
