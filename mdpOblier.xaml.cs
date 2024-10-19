using FM6_H.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Interaction logic for mdpOblier.xaml
    /// </summary>
    /// 



    public partial class mdpOblier : Window
    {
        public mdpOblier()
        {

            InitializeComponent();


        }





        private void cancelbtn(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            string ap = Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(new ProcessStartInfo
            {
                FileName = ap,
                UseShellExecute = true,
            });

            Application.Current.Shutdown();
        }




        private int GetUserIdByEmail(string email)
        {


            int userId = -1;
            string query = "SELECT id_user FROM u_ser WHERE email = @Email";
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    userId = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            return userId;
        }





        private void sendbtn(object sender, RoutedEventArgs e)
        {
            int _idUser;
            string _email = txtEmailOublier.Text;
            _idUser = GetUserIdByEmail(_email);
            string message = "EMAIL : " + txtEmailOublier.Text + "\n" + "MOT DE PASSE : " + txtNvMDP.Text;
            string subject = "نسيت كلمة المرور";
            string state = "Rédiger";
            string admin = "Admin";

            DateTime executionDate = DateTime.Now;
            DateTime notificationDate = DateTime.Now;

            string query = @"
        INSERT INTO notification (id_user, destinataire,Redacteur , message, sujet, etat, date_execution, date_notification)
        SELECT @UserId, @destinataire,@Redacteur, @Message, @Subject, @State, @ExecutionDate, @NotificationDate
        FROM u_ser
        WHERE type = 'admin'";



            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", _idUser);
                cmd.Parameters.AddWithValue("@destinataire", admin);
                cmd.Parameters.AddWithValue("@Redacteur", _email);
                cmd.Parameters.AddWithValue("@Message", message);
                cmd.Parameters.AddWithValue("@Subject", subject);
                cmd.Parameters.AddWithValue("@State", state);
                cmd.Parameters.AddWithValue("@ExecutionDate", executionDate);
                cmd.Parameters.AddWithValue("@NotificationDate", notificationDate);



                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Notification sent to admins.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }
    }
}
