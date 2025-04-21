using System;

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
    static double Pochodna_Funkcji(double wyjscie, double Beta)
    {
        return Beta * wyjscie * (1.0 - wyjscie);
    }

    static void Main(string[] args)
    {
        int Beta = 1;
        double Współczynnik = 0.3;
        int liczba_Epok = 5000;
        Liczba_neuronów();
    }
}