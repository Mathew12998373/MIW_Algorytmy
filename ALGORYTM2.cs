using System;

public class Class1
{
    static int ilosc_bitow = 3;
    static int liczba_iteracji = 20;
    static int liczba_osobnikow = 9;
    static int rozmiar_turnieju = 2;
    
    static string znalezienie_parametrow(double x1, double x2)
	{
		double F =0;
		return F = Math.sin(x1 * 0.05) + Math.sin(x2 * 0.05) + 0.4 * Math.sin(x1 * 0.15) * Math.sin(x2 * 0.15);
	}
	static int Kodowanie()
	{
        double ZDMin = -1;
        double ZDMAX = 2;
        double LBnP = 3;
        double ZD = ZDMAX - ZDMin;
		string kod = "";
		pm = Math.Max(pm, ZDMAX);
		pm = Math.Min(pm, ZDMin);
		int ctmp = (int)Math.Round((pm - ZDMin) / ZD * (2 * *(LBnP) - 1);
		for (int i=0; i< LBnP; i++)
		{
			kod = (ctmp % 2) + kod;
			ctmp /= 2;
		}
		for (int i=0; i<kod; i++)
		{
			console.WriteLine("Oto następujący kod:{0}", i);
		}
	}
	static int dekodowanie()
	{

	}
	static int Pula_osobnikow()
	{

	}
	static void Main()
	{
		

	}
}
