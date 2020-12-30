﻿using BlAPI;
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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl = factoryBL.GetBL();
        AdminWindow wnd; 
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // You do not need to log in to this stage yet so we will go straight to the admin window
            wnd = new AdminWindow(bl);
            wnd.Show();

        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This feature is not valid for this step", "Soon...", MessageBoxButton.OK, MessageBoxImage.Information);
               
        }
    }
}