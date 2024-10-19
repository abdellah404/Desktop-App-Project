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
    /// Interaction logic for demande.xaml
    /// </summary>
    public partial class demande : Window
    {

        private string _email;
        private int _idUser;

        public demande(string email)
        {
            InitializeComponent();
            _email = email;
            _idUser = GetUserIdByEmail(_email);
        }


        private int GetUserIdByEmail(string email)
        {
            int userId = -1;
            string query = "SELECT id_user FROM u_ser WHERE email = @Email";
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-NLLSHRK\\SQLEXPRESS;Initial Catalog=DB_FM6;Integrated Security=True"))
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













        private void btnChangeInfo_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            string subject = "طلب تجديد المعطيات";
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
                cmd.Parameters.AddWithValue("@Redacteur", _email);
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
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            this.Close();

        }

        private void mButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
