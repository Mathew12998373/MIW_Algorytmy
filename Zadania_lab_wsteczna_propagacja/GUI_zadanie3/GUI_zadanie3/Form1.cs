using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SiecNeuronowaGUI
{
    public partial class Form1 : Form
    {
        private (List<List<List<double>>> Wagi, List<List<double>> Bias) Generowanie_wag;
        private List<(int x1, int x2, int x3, int suma, int wyjscie)> probki;
        private int beta = 1;
        private double współczynnik = 0.5;
        private int liczbaEpok = 20000;

        public Form1()
        {
            InitializeComponent();

            probki = new List<(int, int, int, int, int)>
            {
                (0,0,0, 0,0),
                (0,0,1, 1,0),
                (0,1,0, 1,0),
                (0,1,1, 0,1),
                (1,0,0, 1,0),
                (1,0,1, 0,1),
                (1,1,0, 0,1),
                (1,1,1, 1,1)
            };
            var liczbaNeuronow = new List<(int, int)>
            {
                (3, 3),
                (2, 3),
                (2, 2)
            };
            Generowanie_wag = GenerowanieWag(liczbaNeuronow);
            Sieci(probki, Generowanie_wag.Wagi, Generowanie_wag.Bias, beta, współczynnik, liczbaEpok);
        }

        private void btnWyswietl_Click(object sender, EventArgs e)
        {
            string wyniki = "";
            foreach (var (x1, x2, x3, Suma_ostatni, wyjscie_ostatni) in probki)
            {
                var output = Propagacja(Generowanie_wag.Wagi, Generowanie_wag.Bias, new List<double> { x1, x2, x3 }, beta);
                var koniec = output[output.Count - 1];

                wyniki += $"Wejście: {x1} : {x2} : {x3} \t wyjscie1: {Suma_ostatni} wyjscie2: {wyjscie_ostatni} \t pożądana wartość wyjściowa1: {koniec[0]:F2} , pożądana wartość wyjściowa2 {koniec[1]:F2}\n";
            }

            outputBox.Text = wyniki;
        }
        
        private double Funkcja(double x, int beta)
        {
            return 1.0 / (1.0 + Math.Exp(-beta * x));
        }
        private (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag(List<(int neurony, int wejscia)> LiczbaNeuronow)
        {
            List<List<List<double>>> Wagi = new List<List<List<double>>>();
            List<List<double>> Bias = new List<List<double>>();
            var rnd = new Random();

            foreach (var (neurony, wejścia) in LiczbaNeuronow)
            {
                List<List<double>> wagiWarstwy = new List<List<double>>();
                List<double> biasWarstwy = new List<double>();
                for (int i = 0; i < neurony; i++)
                {
                    List<double> wagiNeuronu = new List<double>();
                    for (int j = 0; j < wejścia; j++)
                    {
                        wagiNeuronu.Add(rnd.Next(-5, 6));
                    }
                    wagiWarstwy.Add(wagiNeuronu);
                    biasWarstwy.Add(rnd.Next(-5, 6));
                }
                Wagi.Add(wagiWarstwy);
                Bias.Add(biasWarstwy);
            }
            return (Wagi, Bias);
        }
        private List<List<double>> Propagacja(List<List<List<double>>> Wagi, List<List<double>> Bias, List<double> wejscia, int beta)
        {
            List<List<double>> wyjscia = new List<List<double>>();
            List<double> do_wejsc = new List<double>(wejscia);
            wyjscia.Add(new List<double>(do_wejsc));

            for (int l = 0; l < Wagi.Count; l++)
            {
                List<double> kolejne = new List<double>();
                for (int n = 0; n < Wagi[l].Count; n++)
                {
                    double suma = Bias[l][n];
                    for (int i = 0; i < do_wejsc.Count; i++)
                    {
                        suma += do_wejsc[i] * Wagi[l][n][i];
                    }

                    kolejne.Add(Funkcja(suma, beta));
                }
                wyjscia.Add(kolejne);
                do_wejsc = kolejne;
            }
            return wyjscia;
        }
        private void Sieci(List<(int x1, int x2, int x3, int Suma_ostatni, int wyjscie_ostatni)> probki, List<List<List<double>>> Wagi, List<List<double>> Bias, int beta, double współczynnik, int liczbaEpok)
        {
            int Liczba_warstw = Wagi.Count;
            for (int epoka = 0; epoka < liczbaEpok; epoka++)
            {
                double sumarycznyBlad1 = 0.0;
                double sumarycznyBlad2 = 0.0;

                foreach (var (x1, x2, x3, Suma_ostatni, wyjscie_ostatni) in probki)
                {
                    List<double> wejscia = new List<double> { x1, x2, x3 };
                    List<double> koniec = new List<double> { Suma_ostatni, wyjscie_ostatni };
                    var wyjscia = Propagacja(Wagi, Bias, wejscia, beta);
                    List<List<double>> D = new List<List<double>>();
                    for (int i = 0; i < wyjscia.Count; i++)
                    {
                        D.Add(new List<double>(new double[wyjscia[i].Count]));
                    }
                    int ostatni_element = Liczba_warstw;
                    for (int n = 0; n < wyjscia[ostatni_element].Count; n++)
                    {
                        double a1 = wyjscia[ostatni_element][n];
                        double a2 = koniec[n] - a1;
                        D[ostatni_element][n] = a2 * beta * a1 * (1 - a1);
                        if (n == 0)
                        {
                            sumarycznyBlad1 += Math.Abs(a2);
                        }
                        else
                        {
                            sumarycznyBlad2 += Math.Abs(a2);
                        }
                    }

                    for (int l = Liczba_warstw - 1; l >= 0; l--)
                    {
                        for (int n = 0; n < wyjscia[l].Count; n++)
                        {
                            double Suma = 0.0;
                            for (int k = 0; k < D[l + 1].Count; k++)
                            {
                                Suma += D[l + 1][k] * Wagi[l][k][n];
                            }
                            D[l][n] = Suma * beta * wyjscia[l][n] * (1 - wyjscia[l][n]);
                        }
                    }

                    for (int l = 0; l < Liczba_warstw; l++)
                    {
                        var poprzednie_wyjscie = wyjscia[l];
                        for (int n = 0; n < Wagi[l].Count; n++)
                        {
                            for (int i = 0; i < poprzednie_wyjscie.Count; i++)
                            {
                                Wagi[l][n][i] += współczynnik * D[l + 1][n] * poprzednie_wyjscie[i];
                            }

                            Bias[l][n] += współczynnik * D[l + 1][n];
                        }
                    }
                }
                if (sumarycznyBlad1 < 0.01 && sumarycznyBlad2 < 0.01)
                {
                    break;
                }
            }
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
