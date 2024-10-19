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
using static FM6_H.admin_dash;

namespace FM6_H
{
    /// <summary>
    /// Interaction logic for acessApps.xaml
    /// </summary>
    /// 



    public partial class accessApps : Window
    {

        private int userId;
        SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString());


        private const int websiteAppId = 1;
        private const int fm6CarteAppId = 2;
        private const int projectAppId = 3;
        private const int mailAppId = 4;
        private const int financeAppId = 5;
        private const int boAppId = 6;
        private const int archiveAppId = 7;
        private const int servicesAppId = 8;
        private const int medigesAppId = 9;

        public accessApps(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserAccess();

        }




        private void LoadUserAccess()
        {
            try
            {
                conn.Open();
                string query = "SELECT id_app FROM acces WHERE id_user = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int appId = (int)reader["id_app"];
                    switch (appId)
                    {
                        case websiteAppId:
                            websiteBox.IsChecked = true;
                            break;
                        case fm6CarteAppId:
                            fm6CarteBox.IsChecked = true;
                            break;
                        case projectAppId:
                            projectBox.IsChecked = true;
                            break;
                        case mailAppId:
                            mailBox.IsChecked = true;
                            break;
                        case financeAppId:
                            financeBox.IsChecked = true;
                            break;
                        case boAppId:
                            boBox.IsChecked = true;
                            break;
                        case archiveAppId:
                            archiveBox.IsChecked = true;
                            break;
                        case servicesAppId:
                            servicesBox.IsChecked = true;
                            break;
                        case medigesAppId:
                            medigesBox.IsChecked = true;
                            break;
                    }
                }
                reader.Close();
            }
            finally
            {
                conn.Close();
            }
        }



        private void savebtn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                string deleteQuery = "DELETE FROM acces WHERE id_user = @UserID";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@UserID", userId);
                deleteCmd.ExecuteNonQuery();

                SaveAccess(websiteBox, websiteAppId);
                SaveAccess(fm6CarteBox, fm6CarteAppId);
                SaveAccess(projectBox, projectAppId);
                SaveAccess(mailBox, mailAppId);
                SaveAccess(financeBox, financeAppId);
                SaveAccess(boBox, boAppId);
                SaveAccess(archiveBox, archiveAppId);
                SaveAccess(servicesBox, servicesAppId);
                SaveAccess(medigesBox, medigesAppId);

                MessageBox.Show("تم حفظ الصلاحيات بنجاح");
            }
            finally
            {
                conn.Close();
            }


            this.Close();
        }


        private void SaveAccess(CheckBox checkBox, int appId)
        {
            if (checkBox.IsChecked == true)
            {
                string insertQuery = "INSERT INTO acces (id_user, id_app) VALUES (@UserID, @AppID)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@UserID", userId);
                insertCmd.Parameters.AddWithValue("@AppID", appId);
                //insertCmd.Parameters.AddWithValue("@DateEntrer", DateTime.Now); // Assuming you're storing the current date and time
                insertCmd.ExecuteNonQuery();
            }
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
