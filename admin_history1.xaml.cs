using FM6_H.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace FM6_H
{
    public partial class admin_history1 : Window
    {
        SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString());

        public admin_history1()
        {
            InitializeComponent();
            this.Loaded += Admin_history1_Loaded;
        }

        private void Admin_history1_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
        }

        private void FillComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "SELECT ID_APP, NOM FROM APP";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Applicationn> applications = new List<Applicationn>();
                while (reader.Read())
                {
                    applications.Add(new Applicationn
                    {
                        ID = (int)reader["ID_APP"],
                        AppName = reader["NOM"].ToString()
                    });
                }

                reader.Close();
                SearchableComboBox.ItemsSource = applications;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void SearchableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchableComboBox.SelectedItem is Applicationn selectedApp)
            {
                DateTime? selectedDate = datePicker.SelectedDate;
                if (selectedDate.HasValue)
                {
                    LoadAppData(selectedApp.ID, selectedDate.Value);
                }
                else
                {
                    appDataGrid.ItemsSource = null; // Clear the DataGrid if no date is selected
                }
            }
        }

        private void btnSearch(object sender, RoutedEventArgs e)
        {
            if (SearchableComboBox.SelectedItem is Applicationn selectedApp)
            {
                DateTime? selectedDate = datePicker.SelectedDate;
                if (selectedDate.HasValue)
                {
                    LoadAppData(selectedApp.ID, selectedDate.Value);
                }
                else
                {
                    MessageBox.Show("Please select a date.");
                }
            }
            else
            {
                MessageBox.Show("Please select an application from the list.");
            }
        }

        private void LoadAppData(int appId, DateTime selectedDate)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = @"
                    SELECT registre.date_entrer, registre.date_sortie, u_ser.nom, u_ser.prenom
                    FROM registre
                    INNER JOIN u_ser ON registre.id_user = u_ser.id_user
                    WHERE registre.id_user = u_ser.id_user AND CAST(registre.date_entrer AS DATE) = @SelectedDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AppId", appId);
                cmd.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                appDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }

    public class Applicationn
    {
        public int ID { get; set; }
        public string AppName { get; set; }

        public override string ToString()
        {
            return $"{ID} - {AppName}";
        }
    }
}
