using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SiecNeuronowaGUI
{
    public partial class Form1 : Form
    {
        private (List<List<List<double>>> Wagi, List<List<double>> Bias) Generowanie_Wag;
        private List<(int x1, int x2, int y)> probki;
        private int beta = 1;
        private double wspolczynnik = 0.3;
        private int liczbaEpok = 50000;

        public Form1()
        {
            InitializeComponent();

            probki = new List<(int, int, int)>
            {
                (0,0,0),
                (0,1,1),
                (1,0,1),
                (1,1,0)
            };

            var liczbaNeuronow = new List<(int, int)>
            {
                (2, 2),
                (1, 2)
            };

            Generowanie_Wag = GenerowanieWag(liczbaNeuronow);
            Sieci(probki, Generowanie_Wag, beta, wspolczynnik, liczbaEpok);
        }

        private void btnWyswietl_Click(object sender, EventArgs e)
        {
            string wyniki = "";

            foreach (var (x1, x2, y) in probki)
            {
                var output = Propagacja(Generowanie_Wag, new List<double> { x1, x2 }, beta);
                wyniki += $"Wejście: {x1}, {x2} pożądana wartość wyjściowa: {y} wyjście: {output[0]:F2}\n";
            }

            outputBox.Text = wyniki;
        }



        private double Funkcja(double x, int beta)
        {
            return 1.0 / (1.0 + Math.Exp(-beta * x));
        }

        private (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag(List<(int neurony, int wejscia)> liczbaNeuronow)
        {
            List<List<List<double>>> Wagi = new List<List<List<double>>>();
            List<List<double>> Bias = new List<List<double>>();
            Random rnd = new Random();

            foreach (var (neurony, wejścia) in liczbaNeuronow)
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

        private List<double> Propagacja((List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag, List<double> wejscia, int beta)
        {
            List<double> wyjscia = new List<double>(wejscia);
            for (int l = 0; l < GenerowanieWag.Wagi.Count; l++)
            {
                List<double> noweWyjscia = new List<double>();
                for (int n = 0; n < GenerowanieWag.Wagi[l].Count; n++)
                {
                    double suma = GenerowanieWag.Bias[l][n];
                    for (int i = 0; i < wyjscia.Count; i++)
                    {
                        suma += wyjscia[i] * GenerowanieWag.Wagi[l][n][i];
                    }
                    noweWyjscia.Add(Funkcja(suma, beta));
                }
                wyjscia = noweWyjscia;
            }
            return wyjscia;
        }

        private void Sieci(List<(int x1, int x2, int y)> probki, (List<List<List<double>>> Wagi, List<List<double>> Bias) GenerowanieWag, int beta, double wspolczynnik, int liczbaEpok)
        {
            int Liczba_warstw = GenerowanieWag.Wagi.Count;

            for (int epoka = 0; epoka < liczbaEpok; epoka++)
            {
                double sumarycznyBlad = 0.0;

                foreach (var (x1, x2, x3) in probki)
                {
                    List<double> wejscia = new List<double> { x1, x2 };
                    List<List<double>> wyjsciaWarstw = new List<List<double>>();
                    List<double> aktualne = new List<double>(wejscia);
                    for (int l = 0; l < Liczba_warstw; l++)
                    {
                        List<double> Wyjscie_L = new List<double>();
                        for (int n = 0; n < GenerowanieWag.Wagi[l].Count; n++)
                        {
                            double suma = GenerowanieWag.Bias[l][n];
                            for (int i = 0; i < aktualne.Count; i++)
                            {
                                suma += aktualne[i] * GenerowanieWag.Wagi[l][n][i];
                            }
                            Wyjscie_L.Add(Funkcja(suma, beta));
                        }
                        wyjsciaWarstw.Add(Wyjscie_L);
                        aktualne = Wyjscie_L;
                    }
                    double y_Sieci = aktualne[0];
                    double blad = x3 - y_Sieci;
                    sumarycznyBlad += Math.Abs(blad);
                    var D = new List<double>[Liczba_warstw];
                    for (int i = 0; i < Liczba_warstw; i++)
                    {
                        D[i] = new List<double>(new double[GenerowanieWag.Wagi[i].Count]);
                    }

                    int ostatnia = Liczba_warstw - 1;
                    for (int j = 0; j < GenerowanieWag.Wagi[ostatnia].Count; j++)
                    {
                        D[ostatnia][j] = blad * beta * wyjsciaWarstw[ostatnia][j] * (1 - wyjsciaWarstw[ostatnia][j]);
                    }

                    for (int l = Liczba_warstw - 2; l >= 0; l--)
                    {
                        for (int i = 0; i < GenerowanieWag.Wagi[l].Count; i++)
                        {
                            double Suma = 0.0;
                            for (int k = 0; k < GenerowanieWag.Wagi[l + 1].Count; k++)
                            {
                                Suma += GenerowanieWag.Wagi[l + 1][k][i] * D[l + 1][k];
                            }
                            D[l][i] = Suma * beta * wyjsciaWarstw[l][i] * (1 - wyjsciaWarstw[l][i]);
                        }
                    }
                    for (int l = 0; l < Liczba_warstw; l++)
                    {
                        List<double> poprzednie_wyjscie;
                        if (l == 0)
                            poprzednie_wyjscie = wejscia;
                        else
                            poprzednie_wyjscie = wyjsciaWarstw[l - 1];

                        for (int n = 0; n < GenerowanieWag.Wagi[l].Count; n++)
                        {
                            for (int i = 0; i < poprzednie_wyjscie.Count; i++)
                            {
                                GenerowanieWag.Wagi[l][n][i] += wspolczynnik * D[l][n] * poprzednie_wyjscie[i];
                            }
                            GenerowanieWag.Bias[l][n] += wspolczynnik * D[l][n];
                        }
                    }
                }
                if (sumarycznyBlad < 0.3)
                {
                    break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
