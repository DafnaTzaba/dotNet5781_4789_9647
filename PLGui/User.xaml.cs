﻿using BlAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Page
    {
        IBL bl = factoryBL.GetBl();
        private ObservableCollection<BO.Line> temp = new ObservableCollection<BO.Line>();
        private ObservableCollection<BO.Line> line1 = new ObservableCollection<BO.Line>();
        private ObservableCollection<BO.Line> line2 = new ObservableCollection<BO.Line>();
        private ObservableCollection<BO.Line> empty = new ObservableCollection<BO.Line>();
        BO.Station stationData1 = new BO.Station();
        private IEnumerable<BO.Line> temp1;
        BO.Station stationData2 = new BO.Station();
        private IEnumerable<BO.Line> temp2;
        private ObservableCollection<BO.Station> stations1 = new ObservableCollection<BO.Station>();
        private ObservableCollection<BO.Station> stations2 = new ObservableCollection<BO.Station>();




        public User()
        {
            InitializeComponent();
            
            RefreshStation();
           


        }

        public ObservableCollection<T> ConvertList<T>(IEnumerable<T> listFromBO)
        {
            return new ObservableCollection<T>(listFromBO);
        }

        private void RefreshStation()
        {
          stations1 = ConvertList(bl.GetAllStations());//to make ObservableCollection
            stations2= ConvertList(bl.GetAllStations());
            foreach (var item in stations1)
            {
                ComboBoxItem newItem1 = new ComboBoxItem();
                newItem1.Content = item.Code + "   " +item.Name;
        
                station1.Items.Add(newItem1);
                
            }
            foreach (var item in stations2)
            {
                ComboBoxItem newItem2 = new ComboBoxItem();
              
                newItem2.Content = item.Code + "   " + item.Name;
   
                station2.Items.Add(newItem2);
            }






        }

        private void station1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string a= station1.SelectedItem.ToString();

            int codStation = getNum1(a);

            temp1 = bl.GetAllLineIndStation(codStation);
           // LineInStation1.ItemsSource = temp1;

        }
        private int getNum1(string a)
        {
            
            int b = a.Length;
            string c = "";

            for (int i = 0; i < b; i++)
            {
                if (a.ElementAt(i) <= '9' && a.ElementAt(i) >= '0')
                    c += a.ElementAt(i);
            }
            int codStation = Convert.ToInt32(c);
            return codStation;
        }

        private void station2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string a = station2.SelectedItem.ToString();
    
            int codStation = getNum2(a);

            temp2 = bl.GetAllLineIndStation(codStation);
          //  LineInStation2.ItemsSource = temp2;

        }
        private int getNum2(string a)
        {
            int b = a.Length;
            string c = "";

            for (int i = 0; i < b; i++)
            {
                if (a.ElementAt(i) <= '9' && a.ElementAt(i) >= '0')
                    c += a.ElementAt(i);
            }
            int codStation = Convert.ToInt32(c);
            return codStation;
        }

        private void Grid_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void ee_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void checkOkey_Click(object sender, RoutedEventArgs e)
        {
            int cod1 = getNum1(station1.SelectedItem.ToString());
            int cod2 = getNum2(station2.SelectedItem.ToString());
            
            temp = ConvertList(bl.TravelPath(cod1, cod2));
            if (temp.Count()>0)
            {
                BO.Line help = temp2.FirstOrDefault(b => b.IdNumber == temp.ElementAt(0).IdNumber);
                if (help != null) //so we have a dierect line cetween the station
                {
                    tryoneline.ItemsSource = empty;
                    tryoneline.ItemsSource = temp;
                    tryotowline.ItemsSource = empty;
                    checkOkey.IsChecked = false;
                }
                else
                {
                    tryoneline.ItemsSource = empty;

                    for (int i = 0; i < temp.Count(); i++)
                    {
                        if (i % 2 == 0 && line1.FirstOrDefault(b => b.IdNumber == temp.ElementAt(i).IdNumber) == null)
                            line1.Add(temp.ElementAt(i));
                        if (i % 2 != 0 && line2.FirstOrDefault(b => b.IdNumber == temp.ElementAt(i).IdNumber) == null)
                            line2.Add(temp.ElementAt(i));
                    }
                    tryotowline.ItemsSource = line2;
                    tryoneline.ItemsSource = line1;
                    checkOkey.IsChecked = false;

                }
            }
            else
            {
                tryoneline.ItemsSource = empty;
                tryotowline.ItemsSource = empty;
                checkOkey.IsChecked = false;
            }
        }

        private void tryoneline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}