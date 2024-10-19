using FM6_H.Helpers;
using FM6_H.Models;
using FM6_H.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace FM6_H
{
    public partial class NotificationTab : Window
    {
        private string _email;
        private int userId;
        private string _role;
        private string currentEtatFilter;
        private string currentSujetFilter;

        public NotificationTab(string email)
        {
            InitializeComponent();
            _email = email;
            _role = DetermineRole(email);
            userId = GetUserIdByEmail(email);
            currentEtatFilter = "VU";
            LoadNotifications(currentEtatFilter);



        }

        private string DetermineRole(string email)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TYPE FROM u_ser WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);
                var role = command.ExecuteScalar() as string;
                return role;
            }
        }

        private int GetUserIdByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id_user FROM u_ser WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);
                var userId = command.ExecuteScalar();

                if (userId != null && userId != DBNull.Value)
                {
                    return (int)userId;
                }
                else
                {
                    MessageBox.Show("No notifications found for this user.");
                    return -1;
                }
            }
        }

        private void HandleNotificationButtonClicked(object sender, EventArgs e)
        {
            currentEtatFilter = "VU";
            LoadNotifications(currentEtatFilter, currentSujetFilter);
        }

        private void LoadNotifications(string etat = null, string sujet = null)
        {
            currentEtatFilter = etat;
            currentSujetFilter = sujet;

            List<Notification> notifications = DatabaseHelper.GetNotifications(_role, userId, etat, sujet);
            NotificationsDataGrid.ItemsSource = notifications;

            if (_role == "admin")
            {
                Valide.Visibility = Visibility.Visible;
                Refuse.Visibility = Visibility.Visible;
                AcceptButton.Visibility = Visibility.Collapsed;
                ExecuteButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                Valide.Visibility = Visibility.Collapsed;
                Refuse.Visibility = Visibility.Collapsed;
                AcceptButton.Visibility = Visibility.Visible;
                ExecuteButton.Visibility = Visibility.Visible;
            }
        }







        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedEtat = (EtatComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string selectedSujet = (SujetComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            LoadNotifications(selectedEtat, selectedSujet);
        }

        private void ValiderButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeEtat("Validé");
        }

        private void RefuserButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeEtat("Refusé");
        }

        private void AccepterButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeEtat("Accepté");
        }

        private void ExecuterButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeEtat("Executé");
        }

        private void ChangeEtat(string newEtat)
        {
            if (NotificationsDataGrid.SelectedItem is Notification selectedNotification)
            {
                try
                {
                    selectedNotification.Etat = newEtat;
                    DatabaseHelper.UpdateNotificationEtat(selectedNotification);
                    MessageBox.Show("تم التحديث بنجاح");
                    LoadNotifications(currentEtatFilter, currentSujetFilter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating ETAT: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("المرجو اختيار إشعار");
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            SujetComboBox.SelectedIndex = -1;
            EtatComboBox.SelectedIndex = -1;
            LoadNotifications();
        }

        private void NotificationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
