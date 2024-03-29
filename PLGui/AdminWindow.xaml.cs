﻿using BlAPI;
using System;
using System.ComponentModel;
using System.Windows;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        #region reset
        private IBL bl;
        private AddUser addUser;
        private BO.User userNow = new BO.User();
        #endregion

        #region constructor
        [Obsolete("not using",true)]
        public AdminWindow()
        {
            InitializeComponent();


        }

        public AdminWindow(IBL _bl, BO.User users)
        {
            InitializeComponent();
            userNow = users;
            this.bl = _bl;
            NameTextBlock.Text = userNow.UserName;
        }
        #endregion

        #region Button click
        private void buses_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = (new BusWindowP(bl));
        }

        private void line_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = (new LineWindowP(bl));
        }

        private void station_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = (new StationWindowP(bl));
        }

        private void user_Click_1(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow(bl, userNow);
            userWindow.Show();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoBack)
                frame.GoBack();
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoForward)
                frame.GoForward();
        }

        private void Disengagement_Click(object sender, RoutedEventArgs e)
        {

            MainWindow wnd = new MainWindow();
            this.Close();
            wnd.Show();
        }

        private void contantUs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(":)מצטערים, חלון בבנייה! נתראה בהמשך", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void accountDatiels_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = (new AccountDetails(bl,userNow));
        }

        private void AddManeger_Click(object sender, RoutedEventArgs e)
        {
            addUser = new AddUser(bl);

            bool? result = addUser.ShowDialog();

        }
        #endregion

        #region More func
        private void AdminWindow_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        #endregion
    }
}
