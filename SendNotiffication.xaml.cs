using FM6_H.Helpers;
using FM6_H.Models;
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
using static FM6_H.admin_dash;

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for SendNotiffication.xaml
    /// </summary>
    public partial class SendNotiffication : Window
    {

        private string _email;
        private int userId;

        public SendNotiffication(string email)
        {
            InitializeComponent();
            _email = email;
            userId = GetUserIdByEmail(email);

            PopulateDestinatairesComboBox();
        }

        private void cancelbtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private int GetUserIdByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id_user FROM u_ser WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", _email);

                var userId = command.ExecuteScalar();
                return (int)userId;
            }
        }



        private void sendbtn(object sender, RoutedEventArgs e)
        {
            // Read user inputs
            string subject = SujetTextBox.Text;
            string message = MessageTextBox.Text;
            string selectedDestinataire = DestinataireComboBox.SelectedItem?.ToString();

            // Validate inputs
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(selectedDestinataire))
            {
                MessageBox.Show("Please enter both subject, message, and select a destinataire.");
                return;
            }

            // Split the selected Destinataire into nom and prenom
            string[] parts = selectedDestinataire.Split(' ');
            string nom = parts[0];
            string prenom = parts[1];

            // Query database to get the ID_USER of the selected Destinataire
            int destinataireUserId = GetDestinataireUserId(nom, prenom);

            // Create a new notification
            Notification newNotification = new Notification
            {
                Destinataire = selectedDestinataire,
                Redacteur = "admin",
                Sujet = subject,
                Message = message,
                ID_USER = destinataireUserId,
                Etat = "Rédiger",
                DATE_NOTIFICATION = DateTime.Now
            };

            // Insert the new notification into the database
            DatabaseHelper.InsertNotification(newNotification);



            // Show confirmation message
            MessageBox.Show("Notification sent.");

            // Refresh the DataGrid to show the new notification


            this.Close();
        }

        private int GetDestinataireUserId(string nom, string prenom)
        {
            int userId = 0;

            // Query database to get ID_USER based on nom and prenom
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id_user FROM u_ser WHERE nom = @Nom AND prenom = @Prenom", connection);
                command.Parameters.AddWithValue("@Nom", nom);
                command.Parameters.AddWithValue("@Prenom", prenom);

                var result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out userId))
                {
                    return userId;
                }
                else
                {
                    // Handle case where user is not found or userId could not be parsed
                    return 0; // Or another appropriate value or throw an exception
                }
            }
        }






        private void PopulateDestinatairesComboBox()
        {
            // Clear existing items
            DestinataireComboBox.Items.Clear();

            // Query database to get nom and prenom from u_ser table
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT nom, prenom FROM u_ser", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string nom = reader["nom"].ToString();
                    string prenom = reader["prenom"].ToString();
                    string fullName = $"{nom} {prenom}";
                    DestinataireComboBox.Items.Add(fullName);
                }

                reader.Close();
            }
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
