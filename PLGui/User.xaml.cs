﻿using BlAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PLGui
{
    /// <summary>
    /// Interaction logic for User.xaml. when we search on path between 2 stations
    /// </summary>
    public partial class User : Page
    {
        #region varieble
        IBL bl = factoryBL.GetBl();
        private ObservableCollection<Object> temp = new ObservableCollection<Object>();
        private ObservableCollection<BO.Line> line1 = new ObservableCollection<BO.Line>();
        private ObservableCollection<BO.Line> line2 = new ObservableCollection<BO.Line>();
        private ObservableCollection<BO.Line> empty = new ObservableCollection<BO.Line>();

        BO.Station stationData1 = new BO.Station();
        private IEnumerable<BO.Line> temp1;
        BO.Station stationData2 = new BO.Station();
        private IEnumerable<BO.Line> temp2;

        private ObservableCollection<BO.Station> stations1 = new ObservableCollection<BO.Station>();
        private ObservableCollection<BO.Station> stations2 = new ObservableCollection<BO.Station>();
        #endregion

        #region constructor
        /// <summary>
        /// defult constructor
        /// </summary>
        public User()
        {
            InitializeComponent();

            //to unsert data to the window
            RefreshStation();
            RefreshStationall();
            realTime.Visibility = Visibility.Hidden;

        }
        #endregion


        #region comboboxChoos
        /// <summary>
        /// to get station1
        /// </summary>
        private void station1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string a = station1.SelectedItem.ToString();
                int codStation = getNum1(a);
                temp1 = bl.GetAllLineIndStation(codStation);
            }
            catch (BO.BadIdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        /// <summary>
        /// get station2
        /// </summary>
        private void station2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string a = station2.SelectedItem.ToString();
                int codStation = getNum1(a);
                temp2 = bl.GetAllLineIndStation(codStation);
            }
            catch (BO.BadIdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// to take out the number of the station from the string
        /// </summary>
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
        #endregion


        #region button
        /// <summary>
        /// if the station is diffrent go to find travel
        /// </summary>
        private void checkOkey_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                temp = null;
                int cod1 = getNum1(station1.SelectedItem.ToString());
                int cod2 = getNum1(station2.SelectedItem.ToString());
                if (cod1 == cod2)
                {
                    MessageBox.Show("אופס...תחנות המוצא והיעד קרובות מידי", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else temp = ConvertList(bl.TravelPath(cod1, cod2));

                OpsiaLine.ItemsSource = temp;
            }
            catch (BO.BadIdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// to start simulation
        /// </summary>
        private void button_Click(object sender, RoutedEventArgs e)
        {

            RealTimeStation r = new RealTimeStation(bl, stationData1);
            r.ShowDialog();


        }


        /// <summary>
        /// search after station
        /// </summary>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (numberText != null)
                {
                    int Sera = Convert.ToInt32(numberText);
                    BO.Station SearchResult = bl.GetStationByCode(Sera);
                    if (SearchResult != null)
                    {
                        ObservableCollection<BO.Station> a = new ObservableCollection<BO.Station>();
                        a.Add(SearchResult);
                        ListOfStations.ItemsSource = a;

                    }
                    else
                    {
                        ListOfStations.ItemsSource = stations;

                    }
                }
            }
            catch (BO.BadIdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ListOfStations_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            add = false;
            var list = (ListView)sender; //to get the line
            stationData1 = list.SelectedItem as BO.Station;

            RefreshLineInStation();
            if (codeTextBox.Text != "")
            {
                realTime.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region refresh

        private ObservableCollection<BO.Station> stations = new ObservableCollection<BO.Station>();
        private IEnumerable<BO.Line> lineStation;
        int oldCode;
        private bool add = false;

        /// <summary>
        /// refresh station lust
        /// </summary>
        private void RefreshStationall()
        {
            stations = ConvertList(bl.GetAllStations());//to make ObservableCollection
            ListOfStations.ItemsSource = stations;
        }

        /// <summary>
        /// refresh the line at the station
        /// </summary>
        public void RefreshLineInStation()
        {

            StationDataGrid.DataContext = stationData1;

            lineStation = null;
            if (stationData1 != null)
            {
                try
                {
                    oldCode = stationData1.Code;
                    lineStation = bl.GetAllLineIndStation(stationData1.Code);
                }
                catch (BO.BadIdException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
            LineInStation.ItemsSource = lineStation;

        }

        /// <summary>
        /// refresh the window of station
        /// </summary>
        private void RefreshStation()
        {
            //we need 2 list in order to do the 2 combobox not to depent
            stations1 = ConvertList(bl.GetAllStations());//to make ObservableCollection
            stations2 = ConvertList(bl.GetAllStations());
            foreach (var item in stations1)//creat the first combobox
            {
                ComboBoxItem newItem1 = new ComboBoxItem();
                newItem1.Content = item.Code + "   " + item.Name;
                station1.Items.Add(newItem1);

            }
            foreach (var item in stations2)//creat the secont combobox
            {
                ComboBoxItem newItem2 = new ComboBoxItem();
                newItem2.Content = item.Code + "   " + item.Name;
                station2.Items.Add(newItem2);
            }

        }

        #endregion


        #region textboxInput
        /// <summary>
        /// textbox search input.  
        /// </summary>
        private void textBoxTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return) //when enter
                {

                    TextBox text = sender as TextBox;
                    int lineSteation = int.Parse(text.Text);
                    BO.Station SearchResult = bl.GetStationByCode(lineSteation);
                    if (SearchResult != null)
                    {
                        ObservableCollection<BO.Station> a = new ObservableCollection<BO.Station>();
                        a.Add(SearchResult);
                        ListOfStations.ItemsSource = a;

                    }
                    else
                    {
                        ListOfStations.ItemsSource = stations;
                        //NotExist.Visibility = Visibility.Visible;
                    }


                }
                if (e.Key == Key.Back)
                {
                    // NotExist.Visibility = Visibility.Hidden;
                    ListOfStations.ItemsSource = stations;
                }
            }
            catch (BO.BadIdException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void textBoxTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
        }




        private void textBoxTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ListOfStations.ItemsSource = stations;
            textBoxTextBox.Text = null;
            ListOfStations.SelectedIndex = -1;
            stationData1 = null;
            realTime.Visibility = Visibility.Hidden;
            RefreshLineInStation();

        }
        string numberText;

        private void textBoxTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxTextBox.Text != "Search Station here...." && textBoxTextBox.Text != "")
                numberText = textBoxTextBox.Text;
            textBoxTextBox.Text = "Search Station here....";
        }

        #endregion


        #region moreFunc

        /// <summary>
        /// convert the ienumerable from BO to be a collection observer
        /// </summary>
        public ObservableCollection<T> ConvertList<T>(IEnumerable<T> listFromBO)
        {
            return new ObservableCollection<T>(listFromBO);
        }
        #endregion

    }
}
