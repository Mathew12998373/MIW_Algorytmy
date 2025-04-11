using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeneticAlgorithmGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnUruchom_Click(object sender, EventArgs e)
        {
            int Min = 0;
            int Max = 100;
            int LBnP = 3;
            int liczba_parametrow = 2;
            int liczba_osobnikow = 9;
            int liczba_iteracji = 20;

            Dictionary<string, double> tablicaKodowania = Tablica_kodowania(Min, Max, LBnP);
            List<string> Pula = Pula_osobnikow(liczba_osobnikow, liczba_parametrow, LBnP);

            List<(string, double, double)> Pula_zdekodowana = Dekodowanie(tablicaKodowania, Pula, LBnP);
            List<(string, double)> Pula_oceniona = ocena_osobnika(Pula_zdekodowana);
            (string, double) najlepszy = najlepszy_z_puli(Pula_oceniona);
            double Srednia = srednia(Pula_oceniona, LBnP);

            listBoxWyniki.Items.Clear();
            foreach (var osobnik in Pula_oceniona)
            {
                listBoxWyniki.Items.Add($"Osobnik: {osobnik.Item1}, Ocena: {osobnik.Item2}");
            }
            labelNajlepszy.Text = $"Najlepszy: {najlepszy.Item1}, Wartość: {najlepszy.Item2}";
        }

        private Dictionary<string, double> Tablica_kodowania(int Min, int Max, int LBnP)
        {
            Dictionary<string, double> Tabela = new Dictionary<string, double>();
            double ZD = Max - Min;
            double krok = ZD / (Math.Pow(2, LBnP) - 1);

            Tabela.Add("0".PadLeft(LBnP, '0'), Min);
            double obecnyKrok = krok;

            for (int i = 1; i < Math.Pow(2, LBnP) - 1; i++)
            {
                string klucz = Convert.ToString(i, 2).PadLeft(LBnP, '0');
                Tabela.Add(klucz, Math.Round(obecnyKrok, 2));
                obecnyKrok += krok;
            }
            Tabela.Add(new string('1', LBnP), Max);

            return Tabela;
        }

        private List<string> Pula_osobnikow(int liczba_osobnikow, int liczba_parametrow, int LBnP)
        {
            List<string> Pula = new List<string>();
            Random rnd = new Random();
            int L_B_CH = LBnP * liczba_parametrow;

            for (int i = 0; i < liczba_osobnikow; i++)
            {
                string osobnik = "";
                for (int j = 0; j < L_B_CH; j++)
                {
                    osobnik += rnd.Next(0, 2).ToString();
                }
                Pula.Add(osobnik);
            }
            return Pula;
        }

        private List<(string, double, double)> Dekodowanie(Dictionary<string, double> Tabela, List<string> Pula, int LBnP)
        {
            List<(string, double, double)> Pula_wartosci = new List<(string, double, double)>();
            foreach (var osobnik in Pula)
            {
                string x1 = osobnik.Substring(0, LBnP);
                string x2 = osobnik.Substring(LBnP);
                Pula_wartosci.Add((osobnik, Tabela[x1], Tabela[x2]));
            }
            return Pula_wartosci;
        }
        private double Funkcja_przystosowania(double x1, double x2)
        {
            return Math.Round(Math.Sin(x1 * 0.05) + Math.Sin(x2 * 0.05) + 0.4 * Math.Sin(x1 * 0.15) * Math.Sin(x2 * 0.15), 2);
        }

        private List<(string, double)> ocena_osobnika(List<(string, double, double)> Pula_zdekodowana)
        {
            List<(string, double)> osobnicy = new List<(string, double)>();
            foreach (var krotka in Pula_zdekodowana)
            {
                osobnicy.Add((krotka.Item1, Funkcja_przystosowania(krotka.Item2, krotka.Item3)));
            }
            return osobnicy;
        }

        private (string, double) najlepszy_z_puli(List<(string, double)> Pula)
        {
            (string, double) najlepszy = Pula[0];
            foreach (var krotka in Pula)
            {
                if (krotka.Item2 > najlepszy.Item2)
                {
                    najlepszy = krotka;
                }
            }
            return najlepszy;
        }
        private double srednia(List<(string, double)> Pula, int liczba_osobnikow)
        {
            double średnia = 0;
            double suma = 0;
            for (int i = 0; i < liczba_osobnikow - 1; i++)
            {
                suma += Pula[i].Item2;
            }
            średnia = Math.Round(suma / liczba_osobnikow, 2);
            labelSrednia.Text = $"Średnia dostosowania: {średnia}";
            return średnia;  
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
