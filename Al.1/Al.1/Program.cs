using System;

public class Class1
{
    static double Znalezienie_parametrow(double x1, double x2)
    {
        double F = Math.Sin(x1 * 0.05) + Math.Sin(x2 * 0.05) + 0.4 * Math.Sin(x1 * 0.15) * Math.Sin(x2 * 0.15);
        for (int i = 0; i < F; i++)
        {
            Console.WriteLine("{0}", i);
        }
        return F;
    }
    static string Kodowanie(int ZDMin, int ZDMAX, int LBnP, double pm)
    {
        double ZD = ZDMAX - ZDMin;
        string kod = "";
        pm = Math.Max(pm, ZDMin);
        pm = Math.Min(pm, ZDMAX);
        var ctmp = (int)(Math.Round(pm - ZDMin) / ZD * (Math.Pow(2, LBnP) - 1));

        for (int i = 0; i < LBnP; i++)
        {
            kod = (ctmp % 2) + kod;
            ctmp /= 2;
        }
        for (int i = 0; i < kod.Length; i++)
        {
            Console.WriteLine("Oto następujący kod:{0}", i);
        }
        return kod;
    }
    static double Dekodowanie(string kod, int ZDMin, int ZDMAX, int LBnP)
    {
        double ZD = ZDMAX - ZDMin;
        int ctmp = 0;
        for (int i = 0; i < kod.Length; i++)
        {
            int bit = 0;
            if (kod[i] == '1')
            {
                bit = 1;
            }
            else if (kod[i] == '0')
            {
                bit = 0;
            }
            ctmp += bit * (int)Math.Pow(2, i);
        }
        double pm = ZDMin + (ctmp / (int)Math.Pow(2, LBnP) - 1) * ZD;
        return pm;
    }
    static string[] Pula_osobnikow(int liczba_parametrow, int liczba_osobnikow, int LBnP)
    {
        string[] pula = new string[liczba_osobnikow];
        Random rnd = new Random();
        int chromosomy = LBnP * liczba_parametrow;
        for (int i = 0; i < liczba_osobnikow; i++)
        {
            pula[i] = " ";

            for (int j = 0; j < chromosomy; j++)
            {
                if (rnd.Next(0, 2) == 1)
                {
                    pula[i] += '1';
                }
                else if (rnd.Next(0, 2) == 0)
                {
                    pula[i] += '0';
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
    
    static void Main()
    {
        int ilosc_bitow = 3;
        int liczba_iteracji = 20;
        int liczba_osobnikow = 9;
        int rozmiar_turnieju = 2;
        int liczba_parametrow = 2;
        int ZDMin = -1;
        int ZDMAX = 2;
        int LBnP = 3;
        string[] pula = Pula_osobnikow(liczba_parametrow, liczba_osobnikow, LBnP);
        
    }
}
