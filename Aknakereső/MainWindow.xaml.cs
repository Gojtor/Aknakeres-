using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Aknakereső
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Globális változók definiálása
        int mezoNagysag = 5;
        int aknakSzama = 5;
        int timerErtek = 0;
        int[,] tablaTomb;
        
        
        Button kezdoGomb;
        Random random = new Random();
        DispatcherTimer timer;
        public MainWindow()
        {
             InitializeComponent();

            //Időzítő definiálása és beállítása
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
        }

        //Start gomb megnyomásának eredménye
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            startBtn.IsEnabled = false;

            //Grid sor cellák generálása dinamikusan mezoNagysag értéke alapján
            for (int i = 0; i < mezoNagysag; i++) { 
                aknaMezo.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                }
                );
            }

            //Grid oszlop cellák generálása dinamikusan mezoNagysag értéke alapján
            for (int i = 0; i < mezoNagysag; i++)
            {
                aknaMezo.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                }     
                );
            }
            
            //Grid gombjainak generlálása dinamikusan mezoNagysag értéke alapján
            for (int sor = 0; sor < mezoNagysag; sor++)
                {
                    for (int oszlop = 0; oszlop < mezoNagysag; oszlop++)
                    {
                        kezdoGomb = new Button();
                        //kezdoGomb.Name = "kezdoGomb"+sor+oszlop;
                        kezdoGomb.Click += gomb_Click;
                        Grid.SetRow(kezdoGomb, sor);
                        Grid.SetColumn(kezdoGomb, oszlop);
                        aknaMezo.Children.Add(kezdoGomb);
                    }
                }

            //Aknák random helyének generálása és eltárolása a tablaTomb-be
            
            for (int i = 0; i < aknakSzama; i++)
            {
                int oszlop = random.Next(0, mezoNagysag);
                int sor = random.Next(0, mezoNagysag);
                // Ha már teljesen a tömbből megy a program működése erre kell lecserélni hogy ne legyen ugyanott akna
                if (tablaTomb[sor,oszlop]==-1)
                {
                    i--;
                }
                else
                {
                    tablaTomb[sor, oszlop] = -1;
                }
   
            }
            mennyiSzomszedAkna();
        }

        private void mennyiSzomszedAkna()
        {
            for (int sor = 0; sor < mezoNagysag; sor++)
            {
                for (int oszlop = 0; oszlop < mezoNagysag; oszlop++)
                {
                    if (tablaTomb[sor,oszlop]==-1)
                    {
                        for (int szomszedSor = -1; szomszedSor < 2; szomszedSor++)
                        {
                            for (int szomszedOszlop = -1; szomszedOszlop < 2; szomszedOszlop++)
                            {
                                if ((sor+szomszedSor>=0) && (oszlop+szomszedOszlop>=0) && (mezoNagysag>sor + szomszedSor) && (mezoNagysag > oszlop+szomszedOszlop))
                                {
                                    if (szomszedOszlop==0 && szomszedSor==0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if(tablaTomb[sor + szomszedSor, (oszlop + szomszedOszlop)]==-1)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            tablaTomb[(sor + szomszedSor), (oszlop + szomszedOszlop)]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //Időzítő felhasználása
        private void timer_Tick(object sender, EventArgs e)
        {  
            this.Dispatcher.Invoke(() =>
            {
                timerLabel.Content = ("Eltelt idő: "+timerErtek+" mp");
            });
            timerErtek++;
        }

        //Ha rákkatintunk egy gombra akkor annak a content property-ét megtudjuk a tablaTomb tartalma alapján
        public void gomb_Click(object sender, RoutedEventArgs e)
        {          
            var pozicio = (UIElement)e.Source;
            int oszlop = Grid.GetColumn(pozicio); ;
            int sor = Grid.GetRow(pozicio);
            pozicio.SetValue(Button.ContentProperty,tablaTomb[sor,oszlop]);
        }

        //Megadott akna mennyiségének megszerzése
        private void aknaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem tartalom = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            aknakSzama = int.Parse(tartalom.Content.ToString());
        }

        //ComboBoxban megadott mező nagyságának értékének megszerzése és az alapján a tablaTomb feltöltése
        private void mezoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem tartalom = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            mezoNagysag = int.Parse(tartalom.Content.ToString());
            tablaTomb = new int[mezoNagysag,mezoNagysag];
            for (int sor = 0; sor < mezoNagysag; sor++)
            {
                for (int oszlop = 0; oszlop < mezoNagysag; oszlop++)
                {
                    tablaTomb[sor, oszlop] = 0;
                }
            }
        }
    }
}
