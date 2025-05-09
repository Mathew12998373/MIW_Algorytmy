using System;
using System.Collections.Generic;

class SiecNeuronowa
{
    static List<(int, int)> LiczbaNeuronow()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (2,2),(2,1)
        };
        foreach (var i in Liczba_neuronów)
        {
            Console.WriteLine(i);
        }
        return Liczba_neuronów;
    }

    static double Funkcja(double x, int Beta)
    {
        return 1.0 / (1.0 + Math.Exp(-Beta * x));
    }

    static (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag(List<(int, int)> liczbaNeuronow)
    {
        List<List<List<double>>> Wagi = new List<List<List<double>>>();
        List<List<double>> Bias = new List<List<double>>();
        Random rnd = new Random();

        foreach (var warstwa in liczbaNeuronow)
        {
            List<List<double>> wagiNeuronow = new List<List<double>>();
            List<double> biasNeuronow = new List<double>();

            for (int i = 0; i < warstwa.Item1; i++)
            {
                List<double> wagi = new List<double>();
                for (int j = 0; j <= warstwa.Item2; j++)
                {
                    wagi.Add(rnd.Next(-5, 6));

                }
                wagiNeuronow.Add(wagi);
                biasNeuronow.Add(rnd.Next(-5, 6));
            }
            Wagi.Add(wagiNeuronow);
            Bias.Add(biasNeuronow);
        }
        return (Wagi, Bias);
    }
    static List<double> Propagacja((List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag, List<double> wejscia, int Beta)
    {
        foreach (var warstwa in GenerowanieWag.Wagi)
        {
            List<double> noweWyjscia = new List<double>();

            for (int i = 0; i < warstwa.Count; i++)
            {
                double suma = GenerowanieWag.Bias[GenerowanieWag.Wagi.IndexOf(warstwa)][i];
                for (int j = 0; j < wejscia.Count; j++)
                {
                    suma += wejscia[j] * warstwa[i][j];
                }
                noweWyjscia.Add(Funkcja(suma, Beta));
            }

            wejscia = noweWyjscia;
        }
        return wejscia;
    }

    static void Sieci(List<(int, int, int)> probki, (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag, int Beta, double Współczynnik, int liczbaEpok)
    {
        for (int epoka = 0; epoka < liczbaEpok; epoka++)
        {
            double sumarycznyBlad = 0;

            foreach (var probka in probki)
            {
                List<double> wejscia = new List<double> { probka.Item1, probka.Item2 };
                double oczekiwaneWyjscie = probka.Item3;

                List<List<double>> wszystkieWyjscia = new List<List<double>>();
                List<double> aktualne = wejscia;

                foreach (var warstwa in GenerowanieWag.Wagi)
                {
                    List<double> nowe = new List<double>();
                    int warstwaIndex = GenerowanieWag.Wagi.IndexOf(warstwa);

                    for (int i = 0; i < warstwa.Count; i++)
                    {
                        double suma = 0;
                        for (int j = 0; j < aktualne.Count; j++)
                        {
                            suma += aktualne[j] * warstwa[i][j];
                        }
                        suma += GenerowanieWag.Bias[warstwaIndex][i];
                        nowe.Add(Funkcja(suma, Beta));
                    }
                    wszystkieWyjscia.Add(nowe);
                    aktualne = nowe;
                }

                int ostatnia_waga = GenerowanieWag.Wagi.Count - 1;
                List<double> wyjscieSieci = aktualne;
                double blad = oczekiwaneWyjscie - wyjscieSieci[0];
                sumarycznyBlad += Math.Abs(blad);

                for (int i = 0; i < GenerowanieWag.Wagi[ostatnia_waga].Count; i++)
                {
                    double wyjscie = wszystkieWyjscia[ostatnia_waga][i];
                    double pochodna = blad * Beta * wyjscie * (1.0-wyjscie);

                    for (int j = 0; j < GenerowanieWag.Wagi[ostatnia_waga][i].Count; j++)
                    {
                        double wejscie_z_Poprzedniej = wszystkieWyjscia[ostatnia_waga - 1][j];
                        GenerowanieWag.Wagi[ostatnia_waga][i][j] += Współczynnik * pochodna * wejscie_z_Poprzedniej;
                    }

                    GenerowanieWag.Bias[ostatnia_waga][i] += Współczynnik * pochodna;
                }
            }
            Wyswietlenie(epoka + 1, sumarycznyBlad);
            if (sumarycznyBlad < 0.3)
            {
                break;
            }
        }
    }
    

    static void TestowanieSieci(List<(int, int, int)> probki, (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag, int Beta)
    {

        foreach (var probka in probki)
        {
            List<double> wejscia = new List<double> { probka.Item1, probka.Item2 };
            double wartość_wyjściowa = probka.Item3;

            List<double> wyjscieSieci = Propagacja(GenerowanieWag, wejscia, Beta);
            double blad = Math.Abs(wartość_wyjściowa - wyjscieSieci[0]);
            Wyswietlenie(probka.Item1, probka.Item2, wartość_wyjściowa, wyjscieSieci[0], blad);
        }
    }
    static void Wyswietlenie(int wejscie1, int wejscie2, double wartość_wyjściowa, double wyjscie, double blad)
    {
        Console.WriteLine("Wejście: {0} : {1}   pożądana wartość wyjściowa: {2} \n Wyjście: {3:F2} Błąd: {4:F2}", wejscie1, wejscie2, wartość_wyjściowa, wyjscie, blad);
    }
    static void Wyswietlenie(int epoka, double suma)
    {
        Console.WriteLine("Epoka: {0}, Błąd: {1:F4}", epoka + 1, suma);
    }

    static void Main()
    {
        int Beta = 1;
        double Współczynnik = 0.3;
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
        Console.WriteLine("Sieci:");
        Sieci(probki, generowanieWag, Beta, Współczynnik, liczbaEpok);
        TestowanieSieci(probki, generowanieWag, Beta);
    }
}