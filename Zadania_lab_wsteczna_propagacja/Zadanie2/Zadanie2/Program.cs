using System;
using System.Collections.Generic;

class Zadanie2
{
    static List<(int, int)> LiczbaNeuronow()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (2,2),(2,2),(2,2)
        };
        foreach (var i in Liczba_neuronów)
        {
            Console.WriteLine(i);
        }
        return Liczba_neuronów;
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
    }
}