using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

class Class
{
    static List<(int,int)> Liczba_neuronów()
    {
        List<(int, int)> Liczba_neuronów = new List<(int, int)>
        {
            (2,2),(2,1)
        };
        foreach(var i in Liczba_neuronów)
        {
            Console.WriteLine(i);
        }
        return Liczba_neuronów;
    }
    static double Funkcja(double x, int Beta)
    {
        return 1.0 / (1.0 + Math.Exp(-Beta * x));
    }
    
    static( List<List<List<double>>> Wagi, List<List<double>> Wyjscia) Generowanie_wag (List<(int,int)> Liczba_neuronów)
    {
        List<List<List<double>>> Wagi = new List<List<List<double>>>();
        List<List<double>> wyjscie = new List<List<double>>();
        Random rnd = new Random();
        foreach(var i in Liczba_neuronów)
        {
            List<List<double>> warstwy_dla_wagi = new List<List<double>>();
            List<double> warstwy_dla_wyjscia = new List<double>();

            for (int j = 0; j < i.Item1; j++)
            {
                List<double> wagi_neuronów = new List<double>();
                warstwy_dla_wyjscia.Add(0);
                for(int k=0; k<= i.Item2; k++)
                {
                    wagi_neuronów.Add(rnd.Next(-5, 6));
                }
                warstwy_dla_wagi.Add(wagi_neuronów);
            }
            Wagi.Add(warstwy_dla_wagi);
            wyjscie.Add(warstwy_dla_wyjscia);
        }
        Console.WriteLine("Wagi:");

        foreach (var i in Wagi)
        {
            foreach (var j in i)
            {
                foreach (var k in j)
                {
                    Console.Write("{0} ", k);
                }
            }
        }
        Console.WriteLine("\t");
        Console.WriteLine("wyjscia\t");
        foreach (var i in wyjscie)
        {
            foreach (var j in i)
            {
                Console.Write("{0} ", j);
            }
           
        }
        return (Wagi, wyjscie);
    }
    
    static void Main(string[] args)
    {
        int Beta = 1;
        double Współczynnik = 0.3;
        int liczba_Epok = 5000;
        List<(int, int, int)> probki = new List<(int, int, int)>
        {
            (0,0,0),
            (0,1,1),
            (1,0,1),
            (1,1,0)
        };
        List<(int, int)> liczba_neuronów = Liczba_neuronów();
        (List<List<List<double>>> Wagi, List< List<double> > Wyjscia) generowanie_wag = Generowanie_wag(liczba_neuronów);
        Console.WriteLine("\t");
        
    }
}