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
            List<string> pula = Pula_osobnikow(liczba_osobnikow, liczba_chromosomow, liczba_parametrow);
            Dictionary<string, double> tablica = Tablica_kodowania(Min, Max, liczba_chromosomow);
            List<(string, double[])> pula_zdekodowana = Dekodowanie(tablica, pula, liczba_chromosomow, liczba_parametrow);
            List<(string, double)> oceny = Ocen_osobnika(pula_zdekodowana);
            (string, double) najlepszy_osobnik = Najlepszy(oceny);

            for (int i = 0; i < liczba_iteracji; i++)
            {
                List<string> nowa_pula = Turniej(oceny, liczba_osobnikow);
                Krzyzowanie(nowa_pula, liczba_chromosomow, liczba_parametrow);
                nowa_pula = Mutacja(nowa_pula);
                List<(string, double[])> dekodowani = Dekodowanie(tablica, nowa_pula, liczba_chromosomow, liczba_parametrow);
                List<(string, double)> oceny_now = Ocen_osobnika(dekodowani);
                oceny_now.Add(najlepszy_osobnik);
                najlepszy_osobnik = Najlepszy(oceny_now);
                oceny = oceny_now;
            }

            listBoxWyniki.Items.Clear();
            foreach (var osobnik in oceny)
            {
                listBoxWyniki.Items.Add($"Osobnik: {osobnik.Item1}, Ocena: {osobnik.Item2}");
            }
            labelNajlepszy.Text = $"Najlepszy: {najlepszy_osobnik.Item1}, Wartość: {najlepszy_osobnik.Item2}";
            labelSrednia.Text = $"Średnia dostosowania: {Srednia(oceny)}";
        }

        static List<string> Pula_osobnikow(int liczba_osobnikow, int liczba_chromosomow, int liczba_parametrow)
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

        private List<(string, double[])> Dekodowanie(Dictionary<string, double> tablica, List<string> pula, int liczba_chromosomow, int liczba_parametrow)
        {
            List<(string, double[])> zdekodowane = new List<(string, double[])>();
            foreach (var osobnik in pula)
            {
                double[] wagi = new double[liczba_parametrow];
                for (int i = 0; i < liczba_parametrow; i++)
                {
                    string bin = osobnik.Substring(i * liczba_chromosomow, liczba_chromosomow);
                    wagi[i] = tablica[bin];
                }
                zdekodowane.Add((osobnik, wagi));
            }
            return zdekodowane;
        }

        private double Funkcja_przystosowania(double[] wagi)
        {
            double[][] wejscia = new double[4][]
            {
                new double[] {0, 0, 1},
                new double[] {0, 1, 1},
                new double[] {1, 0, 1},
                new double[] {1, 1, 1}
            };
            double[] oczekiwane = { 0, 1, 1, 0 };
            double blad = 0.0;

            for (int i = 0; i < 4; i++)
            {
                double suma1 = 0.0;
                for (int j = 0; j < 3; j++)
                    suma1 += wejscia[i][j] * wagi[j];
                double neuron1 = 1.0 / (1.0 + Math.Exp(-suma1));

                double suma2 = 0.0;
                for (int j = 0; j < 3; j++)
                    suma2 += wejscia[i][j] * wagi[j + 3];
                double neuron2 = 1.0 / (1.0 + Math.Exp(-suma2));

                double sumaOut = neuron1 * wagi[6] + neuron2 * wagi[7] + wagi[8];
                double wyjscie = 1.0 / (1.0 + Math.Exp(-sumaOut));

                blad += Math.Pow(oczekiwane[i] - wyjscie, 2);
            }
            return blad;
        }

        private List<(string, double)> Ocen_osobnika(List<(string, double[])> Pula)
        {
            List<(string, double)> osobnicy = new List<(string, double)>();
            foreach (var osobnik in Pula)
            {
                osobnicy.Add((osobnik.Item1, Funkcja_przystosowania(osobnik.Item2)));
            }
            return osobnicy;
        }

        private List<string> Turniej(List<(string, double)> pula, int liczba_osobnikow)
        {
            List<string> nowa_pula = new List<string>();
            Random rnd = new Random();

            for (int i = 0; i < liczba_osobnikow; i++)
            {
                var o1 = pula[rnd.Next(pula.Count)];
                var o2 = pula[rnd.Next(pula.Count)];
                var o3 = pula[rnd.Next(pula.Count)];

                var najlepszy = (string.Empty, double.MaxValue);

                if (o1.Item2 <= o2.Item2 && o1.Item2 <= o3.Item2)
                {
                    najlepszy = o1;
                }
                else if (o2.Item2 <= o1.Item2 && o2.Item2 <= o3.Item2)
                {
                    najlepszy = o2;
                }
                else
                {
                    najlepszy = o3;
                }

                nowa_pula.Add(najlepszy.Item1);
            }
            return nowa_pula;
        }

        private void Krzyzowanie(List<string> pula, int chrom_len, int param_count)
        {
            Random rnd = new Random();
            for (int i = 0; i < pula.Count - 1; i += 2)
            {
                int punkt = rnd.Next(1, chrom_len * param_count - 1);
                string r1 = pula[i];
                string r2 = pula[i + 1];
                string d1 = r1.Substring(0, punkt) + r2.Substring(punkt);
                string d2 = r2.Substring(0, punkt) + r1.Substring(punkt);
                pula[i] = d1;
                pula[i + 1] = d2;
            }
        }

        private List<string> Mutacja(List<string> pula)
        {
            Random rnd = new Random();
            for (int i = 4; i < pula.Count; i++)
            {
                int index = rnd.Next(pula[i].Length);
                char[] geny = pula[i].ToCharArray();
                if (geny[index] == '0')
                {
                    geny[index] = '1';
                }
                else
                {
                    geny[index] = '0';
                }
                pula[i] = new string(geny);
            }
            return pula;
        }

        private (string, double) Najlepszy(List<(string, double)> pula)
        {
            (string, double) najlepszy = pula[0];
            foreach (var i in pula)
            {
                if (i.Item2 < najlepszy.Item2)
                    najlepszy = i;
            }
            return najlepszy;
        }

        private double Srednia(List<(string, double)> pula)
        {
            double suma = 0;
            foreach (var i in pula)
                suma += i.Item2;
            return Math.Round(suma / pula.Count, 4);
        }

    }
}