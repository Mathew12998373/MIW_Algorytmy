using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XOR_Genetyczny
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnUruchom_Click(object sender, EventArgs e)
        {
            int Min = -10;
            int Max = 10;
            int liczba_chromosomow = 4;
            int liczba_osobnikow = 13;
            int liczba_iteracji = 100;
            int liczba_parametrow = 9;
            List<string> Pula = Pula_osobnikow(liczba_osobnikow, liczba_chromosomow, liczba_parametrow);
            Dictionary<string, double> Tablica = Tablica_kodowania(Min, Max, liczba_chromosomow);
            var Pula_zdekodowana = Dekodowanie(Tablica, Pula, liczba_chromosomow, liczba_parametrow);
            var Oceny = Ocen_osobnika(Pula_zdekodowana);

            var najlepszy = Najlepszy(Oceny);

            for (int i = 0; i < liczba_iteracji; i++)
            {
                var nowa_pula = Turniej(Oceny, liczba_osobnikow);
                Krzyzowanie(nowa_pula, liczba_chromosomow, liczba_parametrow);
                nowa_pula = Mutacja(nowa_pula);

                var Dekodowani = Dekodowanie(Tablica, nowa_pula, liczba_chromosomow, liczba_parametrow);
                var OcenyNowe = Ocen_osobnika(Dekodowani);

                OcenyNowe.Add(najlepszy);
                najlepszy = Najlepszy(OcenyNowe);
                Oceny = OcenyNowe;
            }

            listBoxWyniki.Items.Clear();
            foreach (var osobnik in Oceny)
            {
                listBoxWyniki.Items.Add($"Osobnik: {osobnik.Item1}, Ocena: {osobnik.Item2}");
            }
            labelNajlepszy.Text = $"Najlepszy: {najlepszy.Item1}, Wartość: {najlepszy.Item2}";
            labelSrednia.Text = $"Średnia dostosowania: {Srednia(Oceny)}";
        }

        private List<string> Pula_osobnikow(int liczba_osobnikow, int liczba_chromosomow, int liczba_parametrow)
        {
            List<string> Pula = new List<string>();
            Random rnd = new Random();
            int L_B_CH = liczba_chromosomow * liczba_parametrow;

            for (int i = 0; i < liczba_osobnikow; i++)
            {
                string osobnik = "";
                for (int j = 0; j < L_B_CH; j++)
                {
                    osobnik += rnd.Next(0, 2);
                }
                Pula.Add(osobnik);
            }
            return Pula;
        }

        private Dictionary<string, double> Tablica_kodowania(int Min, int Max, int liczba_chromosomow)
        {
            Dictionary<string, double> Tablica = new Dictionary<string, double>();
            double krok = (Max - Min) / (Math.Pow(2, liczba_chromosomow) - 1);

            for (int i = 0; i < Math.Pow(2, liczba_chromosomow); i++)
            {
                string klucz = Convert.ToString(i, 2).PadLeft(liczba_chromosomow, '0');
                Tablica.Add(klucz, Math.Round(Min + i * krok, 2));
            }
            return Tablica;
        }

        private List<(string, double[])> Dekodowanie(Dictionary<string, double> Tablica, List<string> Pula, int liczba_chromosomow, int liczba_parametrow)
        {
            List<(string, double[])> Zdekodowani = new List<(string, double[])>();

            foreach (var osobnik in Pula)
            {
                double[] wagi = new double[liczba_parametrow];
                for (int i = 0; i < liczba_parametrow; i++)
                {
                    string bin = osobnik.Substring(i * liczba_chromosomow, liczba_chromosomow);
                    wagi[i] = Tablica[bin];
                }
                Zdekodowani.Add((osobnik, wagi));
            }
            return Zdekodowani;
        }

        private double Funkcja_przystosowania(double[] wagi)
        {
            double[][] wejscia = {
                new double[] { 0, 0, 1 },
                new double[] { 0, 1, 1 },
                new double[] { 1, 0, 1 },
                new double[] { 1, 1, 1 }
            };
            double[] oczekiwane = { 0, 1, 1, 0 };
            double blad = 0.0;

            for (int i = 0; i < 4; i++)
            {
                double suma1 = 0.0;
                for (int j = 0; j < 3; j++)
                {
                    suma1 += wejscia[i][j] * wagi[j];
                }
                double neuron1 = 1.0 / (1.0 + Math.Exp(-suma1));

                double suma2 = 0.0;
                for (int j = 0; j < 3; j++)
                {
                    suma2 += wejscia[i][j] * wagi[j + 3];
                }
                double neuron2 = 1.0 / (1.0 + Math.Exp(-suma2));

                double sumaOut = neuron1 * wagi[6] + neuron2 * wagi[7] + wagi[8];
                double wyjscie = 1.0 / (1.0 + Math.Exp(-sumaOut));

                blad += Math.Pow(oczekiwane[i] - wyjscie, 2);
            }
            return blad;
        }

        private List<(string, double)> Ocen_osobnika(List<(string, double[])> Pula)
        {
            List<(string, double)> Oceny = new List<(string, double)>();
            foreach (var osobnik in Pula)
            {
                double blad = Funkcja_przystosowania(osobnik.Item2);
                Oceny.Add((osobnik.Item1, blad));
            }
            return Oceny;
        }

        private List<string> Turniej(List<(string, double)> Pula, int liczba_osobnikow)
        {
            List<string> NowaPula = new List<string>();
            Random rnd = new Random();

            for (int i = 0; i < liczba_osobnikow; i++)
            {
                var o1 = Pula[rnd.Next(Pula.Count)];
                var o2 = Pula[rnd.Next(Pula.Count)];
                var o3 = Pula[rnd.Next(Pula.Count)];

                var najlepszy = o1.Item2 <= o2.Item2 && o1.Item2 <= o3.Item2 ? o1 :
                                o2.Item2 <= o1.Item2 && o2.Item2 <= o3.Item2 ? o2 : o3;

                NowaPula.Add(najlepszy.Item1);
            }
            return NowaPula;
        }

        private void Krzyzowanie(List<string> Pula, int liczba_chromosomow, int liczba_parametrow)
        {
            Random rnd = new Random();

            for (int i = 0; i < Pula.Count - 1; i += 2)
            {
                int punkt = rnd.Next(1, liczba_chromosomow * liczba_parametrow - 1);
                string r1 = Pula[i];
                string r2 = Pula[i + 1];

                Pula[i] = r1.Substring(0, punkt) + r2.Substring(punkt);
                Pula[i + 1] = r2.Substring(0, punkt) + r1.Substring(punkt);
            }
        }

        private List<string> Mutacja(List<string> Pula)
        {
            Random rnd = new Random();

            for (int i = 4; i < Pula.Count; i++)
            {
                int indeks = rnd.Next(Pula[i].Length);
                char[] geny = Pula[i].ToCharArray();
                geny[indeks] = geny[indeks] == '0' ? '1' : '0';
                Pula[i] = new string(geny);
            }
            return Pula;
        }

        private (string, double) Najlepszy(List<(string, double)> Pula)
        {
            (string, double) najlepszy = Pula[0]; 
            foreach (var osobnik in Pula)
            {
                if (osobnik.Item2 < najlepszy.Item2) 
                {
                    najlepszy = osobnik;
                }
            }
            return najlepszy; 
        }
        private double Srednia(List<(string, double)> Pula)
        {
            double suma = 0;
            foreach (var osobnik in Pula)
            {
                suma += osobnik.Item2; 
            }
            return Math.Round(suma / Pula.Count, 4); 
        }

    }
}