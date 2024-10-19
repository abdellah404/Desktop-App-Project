using FM6_H.Helpers;
using System;
using System.Collections.Generic;
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

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for admin_history.xaml
    /// </summary>
    public partial class admin_history : Window
    {
        public admin_history(ClickCountModel clickCountModel)

        {

            DataContext = new { ClickCountModel = clickCountModel };
            InitializeComponent();
            Loaded += AdminWindow_Loaded;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            admin_history1 admin_History1 = new admin_history1();
            admin_History1.Show();

        }









        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    connection.Open();

                    // Load click counts for all applications
                    LoadClickCountsFromDatabase(connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading click counts: {ex.Message}");
            }
        }





        private void LoadClickCountsFromDatabase(SqlConnection connection)
        {
            websiteAppCountTextBox.Text = GetClickCountFromDatabase(connection, "websiteApp").ToString();
            fm6carteAppCountTextBox.Text = GetClickCountFromDatabase(connection, "fm6carteApp").ToString();
            mailAppCountTextBox.Text = GetClickCountFromDatabase(connection, "mailApp").ToString();
            financeAppCountTextBox.Text = GetClickCountFromDatabase(connection, "financeApp").ToString();
            archiveAppCountTextBox.Text = GetClickCountFromDatabase(connection, "archiveApp").ToString();
            servicesAppCountTextBox.Text = GetClickCountFromDatabase(connection, "servicesApp").ToString();
        }






        private int GetClickCountFromDatabase(SqlConnection connection, string applicationName)
        {
            // Example method to fetch click count for the given application from database
            string query = $"SELECT ClickCount FROM ClickCounts WHERE ApplicationName = @AppName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppName", applicationName);
            object result = command.ExecuteScalar();

            if (result != null && int.TryParse(result.ToString(), out int clickCount))
            {
                return clickCount;
            }
            return 0; // Default to 0 if no count found or error
        }





    }
}
