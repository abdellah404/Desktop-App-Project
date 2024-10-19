using FM6_H.Helpers;
using FM6_H.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using static FM6_H.admin_dash;

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for infoChangeTab.xaml
    /// </summary>
    public partial class infoChangeTab : Window
    {
        public infoChangeTab()
        {
            InitializeComponent();
            LoadNotifications();
        }



        private void LoadNotifications()
        {
            List<Notification> notifications = DatabaseHelper.GetspecifNotifications("طلب تجديد المعطيات"); // Change as needed
            NotificationsDataGrid.ItemsSource = notifications;
        }



        private void NotificationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationsDataGrid.ItemsSource = new ObservableCollection<Notification>();
        }
    }
}
