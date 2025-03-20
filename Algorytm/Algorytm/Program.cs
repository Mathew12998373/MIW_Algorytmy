using System;

public class Algorytm
{
    static string[] Pula_osobnikow(int liczba_osobnikow)
    {
        string[] pula = new string[liczba_osobnikow];
        Random rnd = new Random();
        int chromosomy = 3; // przykład
        for (int i=0; i< liczba_osobnikow; i++)
        {
            pula[i] = " ";
            for (int j=0; j< chromosomy; j++)
            {
                if(rnd.Next(0,2) == 1)
                {
                    pula[j] += '1';
                }
                else if(rnd.Next(0,2) == 0)
                {
                    pula[j] += '0';
                }
            }
        }
        Console.WriteLine("Pula osobnikow\n");
        for (int i = 0; i < pula.Length; i++)
        {
            Console.WriteLine(pula[i]);
        }
        return pula;

    }
    static string Kodowanie(int ZDMin, int ZDMAX, int LBnP, double pm)
    {
        double ZD = ZDMAX - ZDMin;
        pm = Math.Max(pm, ZDMin);
        pm = Math.Min(pm, ZDMAX);
        var ctmp = (int)(Math.Round(pm - ZDMin) / ZD * (Math.Pow(2, LBnP) - 1));
        string cb = "";
        for (int i = 0; i < LBnP; i++)
        {
            if (ctmp %2 == 1)
            {
                cb += '1' + cb;
            }
            else if (ctmp %2 == 0)
            {
                cb += '0' + cb;
            }
            ctmp /= 2;
        }
        for (int i = 0; i < cb.Length; i++)
        {
            Console.WriteLine("Oto następujący kod:{0}", i);
        }
        return cb;
    }
    //gdzie pm to wartość parametru modelu, cb to ciąg bitów (ciąg chromosomów).  
    static double Dekodowanie(string cb,int ZDMin, int ZDMAX, int LBnP)
    {
        double ZD = ZDMAX - ZDMin;
        int ctmp = 0;
        int cb_b = 0;
        for (int i=0; i< cb.Length; i++)
        {
            if (cb[i] == '1')
            {
                cb_b += 1;
            }
            else if (cb[i] == '0')
            {
                cb_b += 0;
            }
            ctmp += cb_b * (int)Math.Pow(2, i);
        }
        double pm = ZDMin + (ctmp / (int)Math.Pow(2,LBnP) - 1);
        return pm;
    }
    
    static void Main()
    {

        int liczba_osobnikow = 9;
        int ZDMin = -1;
        int ZDMAX = 2;
        int LBnP = 3;
        // dodać chromosomy - chromosomy to cb

        string[] pula = Pula_osobnikow(liczba_osobnikow);

    }
}