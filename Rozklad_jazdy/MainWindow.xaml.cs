using System;
using System.Collections.Generic;
using System.Windows;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace Rozklad_jazdy
{
    public partial class MainWindow : Window                                                                        ////////////////////////////////////////////
    {                                                                                                               ///////////////////ew. godzina do porownywania, np. lista busow po 15.20////////////////////
        string godzina = DateTime.Now.ToShortTimeString();                                                          ////////////////////////////////////////////
        float godz;                                                                                                 ////////////////////////////////////////////    
                                                                                                                    ////////////////////////////////////////////
        DispatcherTimer timer = new DispatcherTimer();                                                              ////////////////////////////////////////////



        List<string> czesc_linku_do_ulicy = new List<string>();                                                     /////Deklaracja części Linków///////////////
        List<string> czesc_linku_do_destynacji = new List<string>();                                                ////////////////////////////////////////////


        public MainWindow()
        {
            timer.Interval = TimeSpan.FromSeconds(1);                                                               ////////////////////////////////////////////
            timer.Tick += timer_Tick;                                                                               //////////////////Zegar, ruszanie sie///////
            timer.Start();                                                                                          ////////////////////////////////////////////

            godzina = godzina.Replace(":", ".");                                                                    //////////////////format porownywanej godziny, z '.' na ':' /////////////
            godz = float.Parse(godzina, System.Globalization.CultureInfo.InvariantCulture)+0.1F;                    ////////////////////////////////////////////











            InitializeComponent();                                                                                  ////załadowywanie się okna///////////////////












        }
        private void generator_ulic()                                                                                                           ////////////////////generator przystanków początkowych////////////////////////
        {                                                                                                                                       //////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
            try                                                                                                                                 ///////////////////////////////////////////////////////////////////////////////
            {                                                                                                                                   ///////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                WebClient stronaPierwsza = new WebClient();                                                                                     ///////////////////////////////////////////////////////////////////////////////
                String kolekcjoner_zawartosci_strony = stronaPierwsza.DownloadString("http://www2.zkmgdynia.pl/rozklady/przysta0.htm");         ///////////////////////////////pobieranie źlódła strony 1//////////////////////
                MatchCollection matcher = Regex.Matches(kolekcjoner_zawartosci_strony, @"gif\W> <A HREF=\W*(.+?)</A");                          ///////////////////////////////wybieranie z źródła naw ulic początkowych///////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                button.Visibility = Visibility.Hidden;                                                                                          ///////////////////////////////////////////////////////////////////////////////
                listBox1.Visibility = Visibility.Visible;                                                                                       ///////////pojawianie sie nowych przyciskow i listy////////////////////////////
                Button_wybierz1.Visibility = Visibility.Visible;                                                                                ///////////////////////////////////////////////////////////////////////////////
                Button_powrot.Visibility = Visibility.Visible;                                                                                  ///////////////////////////////////////////////////////////////////////////////


                foreach (Match GODZ in matcher)                                                                                                 ///////////////////////////////////////////////////////////////////////////////
                {                                                                                                                               ////////////////////////////////////Polskie znaki i wstawianie nazw do listy///
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                    String wartosc = GODZ.Groups[1].Value;                                                                                      ///////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                    czesc_linku_do_ulicy.Add(wartosc.Substring(0, 7));                                                                          ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Remove(0, 30);                                                                                            ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('±', 'ą');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('¶', 'ś');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('¦', 'Ś');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('¬', 'Ź');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                    listBox1.Items.Add(wartosc);                                                                                                ///////////////////////////////////////////////////////////////////////////////
                }
            }
            catch (System.Net.WebException)                                                                                                     ///////////////////////////////////////////////////////////////////////////////
            {                                                                                                                                   /////////////////////////////////////sprawdzanie łącza/////////////////////////
                MessageBox.Show("Nie mozna sie polaczyc");                                                                                      ///////////////////////////////////////////////////////////////////////////////
            }                                                                                                                                   ///////////////////////////////////////////////////////////////////////////////
        }

        private void generator_linii_autobusowych()                                                                                             //pobieranie prierwszego przystanku iGenerator listy przystanków-destynacji/////
        {                                                                                                                                       ////////////////////////////////////////////////////////////////////////////////
            try                                                                                                                                 ////////////////////////////////////////////////////////////////////////////////
            {                                                                                                                                   ////////////////////////////////////////////////////////////////////////////////
                string ulica = listBox1.SelectedValue.ToString();                                                                               /////////////////////////////////Wybranie przystanku pierwszego/////////////////
                int nr = listBox1.SelectedIndex;                                                                                                //////nr wybranego przystanku pierwszego na liscie przystankow//////////////////
                string link = "http://www2.zkmgdynia.pl/rozklady/" + czesc_linku_do_ulicy[nr] + "0.htm";                                        ////////kompozytor linku do listy przystanków-destynacji////////////////////////
                WebClient stronaPierwsza = new WebClient();                                                                                     /////////////////wybieranie strony z listą przystanków-destynacji///////////////
                String kolekcjoner_zawartosci_strony = stronaPierwsza.DownloadString(link);                                                     ////////////pobieranie źródła strony z listą przystanków-destynacji/////////////
                                                                                                                                                ////////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ////////////////////////////////////////////////////////////////////////////////
                MessageBox.Show(ulica);                                                                                                         /////////////////////////////////pokazanie wybranej ulicy początkowej///////////
                listBox1.Items.Clear();                                                                                                         /////////////////////////////////czysczenie ttablicy ulic początkowych//////////


                MatchCollection matcher = Regex.Matches(kolekcjoner_zawartosci_strony, @"</A>\s<A HREF=\W\W\W\W*(.+?)</A></TD>");               //////////////Polskie znaki i wstawianie nazw destynacji do listy//////////////
                foreach (Match linia_autobusowa in matcher)                                                                                     ///////////////////////////////////////////////////////////////////////////////
                {                                                                                                                               ///////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                    String wartosc = linia_autobusowa.Groups[1].Value;                                                                          ///////////////////////////////////////////////////////////////////////////////
                                                                                                                                                ///////////////////////////////////////////////////////////////////////////////
                    czesc_linku_do_destynacji.Add(wartosc.Substring(0, 17));                                                                    ///////////////////////////////////////////////////////////////////////////////    
                    wartosc = wartosc.Remove(0, 36);                                                                                            ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('±', 'ą');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('¶', 'ś');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('¦', 'Ś');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    wartosc = wartosc.Replace('¬', 'Ź');                                                                                        ///////////////////////////////////////////////////////////////////////////////
                    listBox1.Items.Add(wartosc);                                                                                                ///////////////////////////////////////////////////////////////////////////////


                }

                Button_wybierz1.Visibility = Visibility.Hidden;                                                                                 ///////////////////////////////////wymiana gozikow/////////////////////////////
                Button_wybierz2.Visibility = Visibility.Visible;                                                                                ///////////////////////////////////////////////////////////////////////////////
            }
            catch (System.Net.WebException)                                                                                                     ////////////////sprawdzanie łącza//////////////////////////////////////////////
            {                                                                                                                                   ///////////////////////////////////////////////////////////////////////////////
                MessageBox.Show("Nie mozna sie polaczyc");                                                                                      ///////////////////////////////////////////////////////////////////////////////
            }
            catch (System.NullReferenceException)                                                                                               ///////////////////////////////////////////////////////////////////////////////
            {                                                                                                                                   ////////////////////////////////////sprawdzanie wyboru/////////////////////////
                MessageBox.Show("Nic nie wybrano");                                                                                             ///////////////////////////////////////////////////////////////////////////////
            }
        }



        private void timer_Tick(object sender, EventArgs e)                                                                                     ////////////zegar :wyzwalacz////////////////////////////////////////////////////
        {                                                                                                                                       ///////////////////////////////////////////////////////////////////////////////
            label.Content = DateTime.Now.ToLongTimeString();                                                                                    ///////////////////////////////////////////////////////////////////////////////
        }

        private void button_Click(object sender, RoutedEventArgs e)                                                                             ///////////////////////////////////////////////////////////////////////////////
        {                                                                                                                                       /////////////button 1/ generowanie list przystanków początkowych////////////////
            generator_ulic();                                                                                                                   ///////////////////////////////////////////////////////////////////////////////
        }                                                                                                                                       ///////////////////////////////////////////////////////////////////////////////

        private void wyjscie_Click(object sender, RoutedEventArgs e)                                                                            ///////////////////////////////////////////////////////////////////////////////
        {                                                                                                                                       ///////////////////////////////exit///////////////////////////////////////////
            Application.Current.Shutdown();                                                                                                     ///////////////////////////////////////////////////////////////////////////////
        }

   

        private void Button_wybierz1_Click(object sender, RoutedEventArgs e)                                                                     ///////////////////////////////////////////////////////////////////////////////
        {                                                                                                                                        /////////////////////////////////button2/ wybieranie przystanku początkowego/////
            generator_linii_autobusowych();                                                                                                      ///////////////////////////////////////////////////////////////////////////////
        }                                                                                                                                        ///////////////////////////////////////////////////////////////////////////////

        private void Button_wybierz2_Click(object sender, RoutedEventArgs e)                                                                     ///////////////////////////////////////////////////////////////////////////////                                                                                                           
        {                                                                                                                                        //////////////////////////////button 3/ wybieranie przystanku destynacji///////
            try                                                                                                                                  /////////////////////////////////////////////////////////////////////////////////
            {                                                                                                                                    ///////////////////////////////////////////////////////////////////////////////
                string destynacja = listBox1.SelectedValue.ToString();                                                                           //////////////////////////////wybieranie destynacji z listy////////////////////
                int nr = listBox1.SelectedIndex;                                                                                                 ////////////////////////nr destynacji na liscie destynacji/////////////////////
                czesc_linku_do_destynacji[nr] = czesc_linku_do_destynacji[nr].Replace('r', 't');                                                 ///////////////////////////////////////////////////////////////////////////////
                string link = "http://www2.zkmgdynia.pl/rozklady/" + czesc_linku_do_destynacji[nr];                                              ////////////////generator nowego linku z ktorego pobierane są godziny/////////
                MessageBox.Show(destynacja);                                                                                                     //////////////////////////////pokazanie nazwy destynacji//////////////////////
                Button_wybierz1.Visibility = Visibility.Hidden;                                                                                  ///chowanie przyciska////////////////////////////////////////////////////////


                int LICZNIK_PRZYJAZDOW = 1;
                int LACZNA_ILOSC_PRZYJAZDOW = 1;
                WebClient web = new WebClient();
                String kolekcjoner_zawartosci_strony = web.DownloadString(link);

                MatchCollection matcher = Regex.Matches(kolekcjoner_zawartosci_strony, @"><A HREF=\Wjavascript:t\W\W\d\d\d\W\W\W\w\w\W\W\W\W\s*(.+?)<");
                float[] TABLICA_PRZYJAZDOW = new float[matcher.Count + 1];

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                String wartosc;
                foreach (Match GODZ in matcher)
                {
                    float KONWERSJA_MATCHY = float.Parse(GODZ.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                    TABLICA_PRZYJAZDOW[LACZNA_ILOSC_PRZYJAZDOW] = KONWERSJA_MATCHY;

                    LACZNA_ILOSC_PRZYJAZDOW++;


                }


                while (TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW - 1] <= TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW])
                {
                    wartosc = Convert.ToString(TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW]).Replace(",", ":");
                    if (wartosc.Length <= 2) wartosc = wartosc + ":00";
                    string[] sprawdz_wartosc = wartosc.Split(':');
                    if (sprawdz_wartosc[1].Length == 1) wartosc += "0";
                    listBox2.Items.Add(wartosc);
                    LICZNIK_PRZYJAZDOW++;
                    if (LICZNIK_PRZYJAZDOW == TABLICA_PRZYJAZDOW.Length) break;

                }





                if (LICZNIK_PRZYJAZDOW < TABLICA_PRZYJAZDOW.Length)
                {
                    TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW - 1] = 0.1F;

                    while (TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW - 1] <= TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW])
                    {
                        wartosc = Convert.ToString(TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW]).Replace(",", ":");
                        if (wartosc.Length <= 2) wartosc = wartosc + ":00";
                        string[] sprawdz_wartosc = wartosc.Split(':');
                        if (sprawdz_wartosc[1].Length == 1) wartosc += "0";
                        listBox3.Items.Add(wartosc);

                        LICZNIK_PRZYJAZDOW++;
                        if (LICZNIK_PRZYJAZDOW == TABLICA_PRZYJAZDOW.Length) break;

                    }
                }
                while (LICZNIK_PRZYJAZDOW < LACZNA_ILOSC_PRZYJAZDOW)
                {
                    wartosc = Convert.ToString(TABLICA_PRZYJAZDOW[LICZNIK_PRZYJAZDOW]).Replace(",", ":");
                    if (wartosc.Length <= 2) wartosc = wartosc + ":00";
                    string[] sprawdz_wartosc = wartosc.Split(':');
                    if (sprawdz_wartosc[1].Length == 1) wartosc += "0";
                    listBox4.Items.Add(wartosc);
                    LICZNIK_PRZYJAZDOW++;

               
                }
                listBox1.Visibility = Visibility.Hidden;
                Button_wybierz2.Visibility = Visibility.Hidden;

                listBox2.Visibility = Visibility.Visible;
                listBox3.Visibility = Visibility.Visible;
                listBox4.Visibility = Visibility.Visible;

                textBlock1.Visibility = Visibility.Visible;
                textBlock2.Visibility = Visibility.Visible;
                textBlock3.Visibility = Visibility.Visible;
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Nie mozna sie polaczyc");
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Nic nie wybrano");
            }


            
        }


        private void Button_powrot_Click(object sender, RoutedEventArgs e)
        {
            if (Button_wybierz1.Visibility == Visibility.Visible)
            {
                listBox1.Items.Clear();
                button.Visibility = Visibility.Visible;
                listBox1.Visibility = Visibility.Hidden;
                Button_wybierz1.Visibility = Visibility.Hidden;
                Button_powrot.Visibility = Visibility.Hidden;
            }

            if(Button_wybierz2.Visibility == Visibility.Visible)
            {
                listBox1.Items.Clear();
                generator_ulic();
                Button_wybierz2.Visibility = Visibility.Hidden;
            }

            if(textBlock1.Visibility == Visibility.Visible)
            {
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();

                listBox2.Visibility = Visibility.Hidden;
                listBox3.Visibility = Visibility.Hidden;
                listBox4.Visibility = Visibility.Hidden;

                textBlock1.Visibility = Visibility.Hidden;
                textBlock2.Visibility = Visibility.Hidden;
                textBlock3.Visibility = Visibility.Hidden;

                Button_wybierz2.Visibility = Visibility.Visible;
                listBox1.Visibility = Visibility.Visible;
            }
        }

    }

}
