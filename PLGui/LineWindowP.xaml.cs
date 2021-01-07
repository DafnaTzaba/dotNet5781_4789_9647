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
    /// Interaction logic for LineWindowP.xaml
    /// </summary>
    public partial class LineWindowP : Page
    {
        private IBL bl;
        private BO.Line line;

        private ObservableCollection<BO.Line> egged = new ObservableCollection<BO.Line>();
        private ObservableCollection<object> lineStationOfLine = new ObservableCollection<object>();


        private BO.AREA area;
        public LineWindowP()
        {
            InitializeComponent();
        }

        public LineWindowP(IBL bl)
        {
            InitializeComponent();

            this.bl = bl;
            //   RefreshLine();
            comboBoxArea.ItemsSource = Enum.GetValues(typeof(BO.AREA));
            StationLineList.ItemsSource = lineStationOfLine;
            NewLooz.Visibility = Visibility.Hidden;
            NewLooz.DataContext = new BO.LineTrip();
            



        }
        public ObservableCollection<T> Convert<T>(IEnumerable<T> listFromBO)
        {
            return new ObservableCollection<T>(listFromBO);
        }

        private void RefreshLine()
        {
            egged = Convert(bl.GetLineByArea(area));//to make ObservableCollection
            ListOfLine.ItemsSource = egged;


        }
        private void RefreshStationListView()
        {
            if (line != null)
            {
                lineStationOfLine = Convert(bl.DetailsOfStation(line.StationsOfBus));

                Looz.ItemsSource = bl.GetLineByLine(line.IdNumber).TimeLineTrip;
                Looz.Items.Refresh();
                NewLooz.DataContext = new BO.LineTrip();
            }
            else
            {
                lineStationOfLine = null;
                Looz.ItemsSource = null;
            }
            StationLineList.ItemsSource = lineStationOfLine;
            GridDataLine.DataContext = line;

            Looz.Visibility = Visibility.Visible;

            NewLooz.Visibility = Visibility.Hidden;




        }

        private void comboBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            area = (BO.AREA)(comboBoxArea.SelectedItem);
            RefreshLine();
            line = null;
            RefreshStationListView();
            //  busesData.DataContext = bus;
        }

        private void ListOfLine_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list = (ListView)sender; //to get the line
            line = list.SelectedItem as BO.Line;
            RefreshStationListView();

        }

        private void DeleteLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                MessageBoxResult result = MessageBox.Show("You sure you want to delete that line?", "Delete Line Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            var fxElt = sender as FrameworkElement; //get the licence of the bus to refulling. 
                            BO.Line lineToDelete = fxElt.DataContext as BO.Line;
                            bl.DeleteLine(lineToDelete.IdNumber);
                            RefreshLine();
                            line = null;
                            RefreshStationListView();
                            MessageBox.Show("The line was successfully deleted from the system", "Success Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                }

            }
            catch (BO.BadIdException a)
            {
                MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        private void AddLine_Click(object sender, RoutedEventArgs e)
        {

            AddLine addline = new AddLine(bl);
            bool? result = addline.ShowDialog();
            if (result != null)
            {
                BO.Line newline = addline.NewLine;
                line = newline;
                RefreshStationListView();
                comboBoxArea.SelectedItem = newline.Area;
                RefreshLine();




            }
        }

        private void UpdateLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (line != null)
                    bl.UpdateLine(line);
                int index = comboBoxArea.SelectedIndex;
                RefreshLine();
                comboBoxArea.SelectedIndex = index;



                MessageBox.Show("Line details saved successfully", "Success Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (BO.BadIdException a)
            {
                MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStationLine_Click(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement; //get the licence of the bus to refulling. 
            String ToDel = fxElt.DataContext.ToString();
            convertFromObbject(ToDel);
            bl.DeleteStation(line.IdNumber, TempLineStation.StationCode);
            RefreshStationListView();
        }
        BO.LineStation TempLineStation = new BO.LineStation();
        private void convertFromObbject(string StationLineData)
        {
            StationLineData = StationLineData.Remove(0, 16);
            int index = StationLineData.IndexOf(",");
            TempLineStation.StationCode = int.Parse(StationLineData.Substring(0, index));
            index = StationLineData.IndexOf("= ");
            StationLineData = StationLineData.Remove(0, index + 2);
            TempLineStation.LineStationIndex = int.Parse(StationLineData.Substring(0, StationLineData.IndexOf(",")));


        }
        private void UpdataLineStation_Click(object sender, RoutedEventArgs e)
        {
            var fxElt = sender as FrameworkElement; //get the licence of the bus to refulling. 
            string StationLineData = fxElt.DataContext.ToString();//to get the line
            convertFromObbject(StationLineData);

            UpdataStationLineIndex updataStationLineIndex = new UpdataStationLineIndex(line, TempLineStation, bl);


            bool? result = updataStationLineIndex.ShowDialog();
            if (result != null)
            {
                BO.Line newline = updataStationLineIndex.NewLine;
                line = bl.GetLineByLine(newline.IdNumber);
                RefreshStationListView();
                comboBoxArea.SelectedItem = newline.Area;

            }


        }

        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            AddLine addStationTotheLine = new AddLine(line, bl);
            bool? result = addStationTotheLine.ShowDialog();
            if (result != null)
            {
                RefreshStationListView();
            }



        }

        private void AddSchedules_Click(object sender, RoutedEventArgs e)
        {
            NewLooz.Visibility = Visibility.Visible;
            Looz.Visibility = Visibility.Hidden;
            // RefreshStationListView();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineTripViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineTripViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineTripViewSource.Source = [generic data source]
        }

        private void tripLineExistCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (finishAtTextBox.Text != "" && frequencyTextBox.Text != "" && frequencyTextBox.Text != "" && startAtTextBox.Text != "")
                {
                    MessageBoxResult result = MessageBox.Show("You sure you want to add this line trip?", "Add Line trip Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {

                                BO.LineTrip addLineTrip = new BO.LineTrip();
                                addLineTrip = NewLooz.DataContext as BO.LineTrip;
                                addLineTrip.KeyId = line.IdNumber;
                                if (addLineTrip.FinishAt < addLineTrip.StartAt)
                                {
                                    int hour = 24 + addLineTrip.FinishAt.Hours;
                                    TimeSpan toChange = new TimeSpan(hour, 0, 0);
                                    addLineTrip.FinishAt = toChange;
                                }
                                if (!isUpdateLooz)
                                {

                                    bl.AddOneTripLine(addLineTrip);

                                }
                                else
                                {
                                    isUpdateLooz = false;
                                    bl.UpdateLineTrip(indexLineTripOld, addLineTrip);

                                }

                                MessageBox.Show("The line was successfully add to the system", "Success Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                Looz.Visibility = Visibility.Visible;
                                NewLooz.Visibility = Visibility.Hidden;
                                NewLooz.DataContext = new BO.LineTrip();
                                RefreshStationListView();
                                break;


                            }
                        case MessageBoxResult.No:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("To add a new schedule you must fill in all the fields.", "Add Line trip Message", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            {
                                break;
                            }
                        case MessageBoxResult.Cancel:
                            {

                                NewLooz.DataContext = null;
                                NewLooz.Visibility = Visibility.Hidden;
                                RefreshStationListView();
                                break;
                            }
                    }

                }
            }
            catch (BO.BadIdException a)
            {
                MessageBox.Show(a.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                NewLooz.DataContext = null;
                NewLooz.Visibility = Visibility.Hidden;
                Looz.Visibility = Visibility.Visible;
             
                RefreshStationListView();


            }
        }
        private void cancleTripLineAdd_Checked(object sender, RoutedEventArgs e)
        {
            Looz.Visibility = Visibility.Visible;
            AddSchedules.Visibility = Visibility.Visible;
            NewLooz.Visibility = Visibility.Hidden;
            cancleTripLineAdd.IsChecked = false;
           RefreshStationListView();
        }

        BO.LineTrip thisLooz = new BO.LineTrip();
        BO.LineTrip thisLooz1 = new BO.LineTrip();
        int indexLineTripOld;
        private bool isUpdateLooz = false;

        private void UpdataLineTrip_Click(object sender, RoutedEventArgs e)
        {
            Button toconvert = sender as Button;
            thisLooz = toconvert.DataContext as BO.LineTrip;
            indexLineTripOld = line.TimeLineTrip.ToList().FindIndex(b => thisLooz.StartAt == b.StartAt);
            thisLooz.TripLineExist = false;
            isUpdateLooz = true;
            Looz.Visibility = Visibility.Hidden;
            NewLooz.DataContext = thisLooz;

            NewLooz.Visibility = Visibility.Visible;


        }

        private void DeleteTripLine_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("You sure you want to add this line trip?", "Add Line trip Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        Button toconvert = sender as Button;
                        thisLooz = toconvert.DataContext as BO.LineTrip;
                        bl.DeleteLineTrip(thisLooz);
                        RefreshStationListView();
                        break;
                    }
                case MessageBoxResult.No:
                    {
                        break;
                    }
            }
        }

        private void EndTimeLooz_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x) || ':'.Equals(x));
        }

        private void frequencyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x) || '.'.Equals(x));
        }

        private void startAtTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x) || ':'.Equals(x));
        }
    }
}
