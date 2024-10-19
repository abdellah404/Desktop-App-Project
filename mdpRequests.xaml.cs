using FM6_H.Helpers;
using FM6_H.Models;
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
using System.Windows.Shapes;

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for mdpRequests.xaml
    /// </summary>
    public partial class mdpRequests : Window
    {
        public mdpRequests()
        {
            InitializeComponent();
        }

        private void LoadNotifications()
        {
            List<Notification> notifications = DatabaseHelper.GetspecifNotifications("طلب تجديد كلمة المرور"); // Change as needed
            NotificationsDataGridd.ItemsSource = notifications;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
