﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            int Min = 0, Max = 3, liczba_chromosomow = 4, liczba_iteracji = 100, liczba_osobnikow = 13, liczba_parametrow = 3;
            double[] daneX = new double[36];
            double[] daneY = new double[36];

            // Wczytywanie danych
            Wczytanie(daneX, daneY);

            // Generowanie początkowej puli
            List<string> Pula = Pula_osobnikow(liczba_osobnikow, liczba_chromosomow, liczba_parametrow);
            Dictionary<string, double> Tablica = Tablica_kodowania(Min, Max, liczba_chromosomow);
            List<(string, double, double, double)> Pula_zdekodowana = Dekodowanie(Tablica, Pula, liczba_chromosomow);
            List<(string, double)> ocena_osobnika = ocen_osobnika(Pula_zdekodowana, daneX, daneY);

            (string, double) najlepszy = najlepszy_z_puli(ocena_osobnika);

            for (int i = 0; i < liczba_iteracji; i++)
            {
                List<string> nowa_pula = Turniej(ocena_osobnika, liczba_osobnikow);
                Krzyżowanie(nowa_pula, liczba_chromosomow, liczba_parametrow);
                List<string> nowa_po_mutacji = Mutator(nowa_pula);
                List<(string, double, double, double)> nowa_Pula_dekodowana = Dekodowanie(Tablica, nowa_po_mutacji, liczba_chromosomow);
                List<(string, double)> nowa_Pula_oceniona = ocen_osobnika(nowa_Pula_dekodowana, daneX, daneY);
                nowa_Pula_oceniona.Add(najlepszy);
                najlepszy = najlepszy_z_puli(nowa_Pula_oceniona);
                ocena_osobnika = nowa_Pula_oceniona;
            }

            listBoxWyniki.Items.Clear();
            foreach (var osobnik in ocena_osobnika)
            {
                listBoxWyniki.Items.Add($"Osobnik: {osobnik.Item1}, Ocena: {osobnik.Item2}");
            }
            labelNajlepszy.Text = $"Najlepszy: {najlepszy.Item1}, Wartość: {najlepszy.Item2}";
            labelSrednia.Text = $"Średnia dostosowania: {srednia(ocena_osobnika, liczba_osobnikow)}";
        }

        private void Wczytanie(double[] daneX, double[] daneY)
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\MATI\\Desktop\\new\\A1\\sinusik.txt"))
                {
                    int index = 0;
                    while (!sr.EndOfStream)
                    {
                        string linia = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(linia))
                        {
                            string[] czesci = linia.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            daneX[index] = double.Parse(czesci[0], CultureInfo.InvariantCulture);
                            daneY[index] = double.Parse(czesci[1], CultureInfo.InvariantCulture);
                            index++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Błąd podczas wczytywania danych: {e.Message}");
            }
        }

        private List<string> Pula_osobnikow(int liczba_osobnikow, int liczba_chromosomow, int liczba_parametrow)
        {
            List<string> Pula = new List<string>();
            int L_B_CH = liczba_chromosomow * liczba_parametrow;
            Random rnd = new Random();
            for (int i = 0; i < liczba_osobnikow; i++)
            {
                string osobnik = "";
                for (int j = 0; j < L_B_CH; j++)
                {
                    int losowa = rnd.Next(0, 2);
                    if (losowa == 1)
                    {
                        osobnik += '1';
                    }
                    else if (losowa == 0)
                    {
                        osobnik += '0';
                    }
                }
                Pula.Add(osobnik);
            }
            return Pula;
        }

        private Dictionary<string, double> Tablica_kodowania(int Min, int Max, int liczba_chromosomow)
        {
            Dictionary<string, double> Tablica = new Dictionary<string, double>();
            double ZD = Max - Min;
            double krok = ZD / (Math.Pow(2, liczba_chromosomow) - 1);
            Tablica.Add("0".PadLeft(liczba_chromosomow, '0'), Min);
            double obecnykrok = krok;
            for (int i = 1; i < Math.Pow(2, liczba_chromosomow) - 1; i++)
            {
                string klucz = Convert.ToString(i, 2).PadLeft(liczba_chromosomow, '0');
                Tablica.Add(klucz, Math.Round(obecnykrok, 2));
                obecnykrok += krok;
            }
            Tablica.Add(new string('1', liczba_chromosomow), Max);
            foreach (var i in Tablica)
            {
                Console.WriteLine("{0} = {1}", i.Key, i.Value);
            }
            return Tablica;
        }

        private List<(string, double, double, double)> Dekodowanie(Dictionary<string, double> Tablica, List<string> Pula, int liczba_chromosomow)
        {
            List<(string, string, string, string)> Pula_1 = new List<(string, string, string, string)>();
            List<(string, double, double, double)> Pula_wartosci = new List<(string, double, double, double)>();
            string KodBin_osobnika;
            string pa;
            string pb;
            string pc;
            double Pa;
            double Pb;
            double Pc;
            foreach (var i in Pula)
            {
                pa = i.Substring(0, liczba_chromosomow);
                pb = i.Substring(liczba_chromosomow, liczba_chromosomow);
                pc = i.Substring(liczba_chromosomow * 2, liczba_chromosomow);
                Pula_1.Add((i, pa, pb, pc));
            }
            foreach (var i in Pula_1)
            {
                KodBin_osobnika = i.Item1;
                Pa = Tablica[i.Item2];
                Pb = Tablica[i.Item3];
                Pc = Tablica[i.Item4];
                Pula_wartosci.Add((KodBin_osobnika, Pa, Pb, Pc));
            }
            return Pula_wartosci;
        }
        private double Funkcja_przystosowania(double pa, double pb, double pc, double[] X, double[] Y)
        {
            double suma = 0;
            double x;
            double y;
            double f;
            for (int i = 0; i < X.Length; i++)
            {
                x = X[i];
                y = Y[i];
                f = pa * Math.Sin(pb * x + pc);
                suma += Math.Pow((y - f), 2);
            }
            return suma;
        }
        private List<(string, double)> ocen_osobnika(List<(string, double, double, double)> Dekodowanie, double[] X, double[] Y)
        {
            List<(string, double)> osobnicy = new List<(string, double)>();
            foreach (var i in Dekodowanie)
            {
                double Z = Funkcja_przystosowania(i.Item2, i.Item3, i.Item4, X, Y);
                osobnicy.Add((i.Item1, Z));
            }
            return osobnicy;
        }

        private List<string> Turniej(List<(string, double)> Pula, int liczba_osobnikow)
        {
            List<(string, double)> osobnicy_po_turnieju = new List<(string, double)>();
            List<string> nowa_pula = new List<string>();
            Random rnd = new Random();
            for (int i = 0; i < liczba_osobnikow - 1; i++)
            {
                int osobnik1 = rnd.Next(0, liczba_osobnikow);
                int osobnik2 = rnd.Next(0, liczba_osobnikow);
                int osobnik3 = rnd.Next(0, liczba_osobnikow);
                if (Pula[osobnik1].Item2 <= Pula[osobnik2].Item2 && Pula[osobnik1].Item2 <= Pula[osobnik3].Item2)
                {
                    osobnicy_po_turnieju.Add(Pula[osobnik1]);
                }
                else if (Pula[osobnik2].Item2 <= Pula[osobnik1].Item2 && Pula[osobnik2].Item2 <= Pula[osobnik3].Item2)
                {
                    osobnicy_po_turnieju.Add(Pula[osobnik2]);
                }
                else if (Pula[osobnik3].Item2 <= Pula[osobnik1].Item2 && Pula[osobnik3].Item2 <= Pula[osobnik2].Item2)
                {
                    osobnicy_po_turnieju.Add(Pula[osobnik3]);
                }
            }
            foreach (var i in osobnicy_po_turnieju)
            {
                nowa_pula.Add(i.Item1);
            }
            return nowa_pula;
        }
        private void Krzyżowanie(List<string> nowa_Pula, int liczba_chromosomow, int liczba_parametrow)
        {
            Random rnd = new Random();
            int[] indeksy = { 0, 2, 8, 10 };
            foreach (var i in indeksy)
            {
                int b = rnd.Next(1, liczba_chromosomow * liczba_parametrow - 1);
                string rodzic1 = nowa_Pula[i];
                string rodzic2 = nowa_Pula[i + 1];
                string poczatek1 = rodzic1.Substring(0, b);
                string poczatek2 = rodzic2.Substring(0, b);
                string koniec1 = rodzic1.Substring(b);
                string koniec2 = rodzic2.Substring(b);
                string krzyzowanie1 = poczatek1 + koniec2;
                string krzyzowanie2 = poczatek2 + koniec1;
                nowa_Pula[i] = krzyzowanie1;
                nowa_Pula[i + 1] = krzyzowanie2;
            }
        }
        private List<string> Mutator(List<string> Pula)
        {
            List<string> zmutowane = new List<string>();
            Random rnd = new Random();
            int licznik = 0;
            foreach (var i in Pula)
            {
                if (licznik < 4)
                {
                    zmutowane.Add(i);
                    licznik++;
                    continue;
                }
                int b = rnd.Next(0, i.Length);
                string poczatek = i.Substring(0, b);
                string koniec = i.Substring(b + 1);
                char nowy_Bit = new char();
                if (i[b] == '1')
                {
                    nowy_Bit = '0';
                }
                else if (i[b] == '0')
                {
                    nowy_Bit = '1';
                }
                string zmutowanyOsobnik = poczatek + nowy_Bit + koniec;
                zmutowane.Add(zmutowanyOsobnik);
            }
            return zmutowane;
        }
        private (string, double) najlepszy_z_puli(List<(string, double)> Pula)
        {
            (string, double) najlepszy = Pula[0];
            foreach (var krotka in Pula)
            {
                if (krotka.Item2 < najlepszy.Item2)
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
            return średnia;
        }
    }
}
