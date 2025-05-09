using System;
using System.Collections.Generic;

class Zadanie2
{
    static List<(int, int)> LiczbaNeuronow()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (2,2),
            (2,2),
            (2,2)
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
                for (int j = 0; j < warstwa.Item2; j++)
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


    static void Main(string[] args)
    {
        int Beta = 1;
        double Współczynnik = 0.3;
        int liczbaEpok = 50000;
        List<(int, int, int, int)> probki = new List<(int, int, int, int)>
        {
            (0,0,0,1),
            (0,1,1,0),
            (1,0,1,0),
            (1,1,0,0)
        };

        var liczbaNeuronow = LiczbaNeuronow();
        var generowanieWag = GenerowanieWag(liczbaNeuronow);
        Console.WriteLine("Sieci:");
    }
}