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
    /// Interaction logic for resetpass1.xaml
    /// </summary>
    public partial class resetpass1 : Window
    {

        private string _userEmail;
        private int _idUser;

        public resetpass1(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail;
            _idUser = GetUserIdByEmail(_userEmail);
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









        private void mButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }




        private void btnChangePassword(object sender, RoutedEventArgs e)
        {
            string message = "mot de passe actuel : " + txtMDP_Actuel.Text + "nouveau Mot de Passe est :" + txtMDP_Nouveau.Text;
            string subject = "طلب تجديد كلمة المرور";
            string state = "Rédiger";
            string admin = "Admin";

            DateTime notificationDate = DateTime.Now;

            string query = @"
        INSERT INTO notification (id_user, destinataire,Redacteur , message, sujet, etat, date_notification)
        SELECT @UserId, @destinataire,@Redacteur, @Message, @Subject, @State, @NotificationDate
        FROM u_ser
        WHERE type = 'admin'";

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", _idUser);
                cmd.Parameters.AddWithValue("@destinataire", admin);
                cmd.Parameters.AddWithValue("@Redacteur", _userEmail);
                cmd.Parameters.AddWithValue("@Message", message);
                cmd.Parameters.AddWithValue("@Subject", subject);
                cmd.Parameters.AddWithValue("@State", state);
                cmd.Parameters.AddWithValue("@NotificationDate", notificationDate);



                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم الإرسال بنجاح");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ" + ex.Message);
                }
            }


            this.Close();
        }
    }
}
