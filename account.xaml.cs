using FM6_H.Helpers;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
using System.Xml.Linq;


namespace FM6_H
{
    /// <summary>
    /// Interaction logic for account.xaml
    /// </summary>
    public partial class account : Window
    {

        SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString());
        string SEXE;
        private string userEmail;
        private string imagePath;


        public account(string email)
        {
            InitializeComponent();
            userEmail = email; // Store the email
            LoadUserData(email); // Load user data based on the email


        }


        //public class User
        //{
        //    public int ID_USER { get; set; }
        //    public string NOM { get; set; }
        //    public string PRENOM { get; set; }
        //    public string EMAIL { get; set; }
        //    public string SEXE { get; set; }
        //    public string CNI { get; set; }
        //    public string NumLoyer { get; set; }
        //    public string Filiere { get; set; }
        //    public string Service { get; set; }
        //    public string PHOTO { get; set; }


        //}


        private void btn_male_Checked(object sender, RoutedEventArgs e)
        {
            SEXE = "MALE";
        }

        private void btn_female_Checked(object sender, RoutedEventArgs e)
        {
            SEXE = "FEMAL";
        }




        private void LoadUserData(string email)
        {
            // Add your code to load user data based on the email
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                string query = "SELECT * FROM U_SER WHERE EMAIL = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    txtName.Text = reader["NOM"].ToString();
                    txtLastname.Text = reader["PRENOM"].ToString();
                    txtEmail.Text = reader["EMAIL"].ToString();
                    string mof = reader["SEXE"].ToString();
                    if (mof == "MALE")
                    {
                        btn_male.IsChecked = true;
                        btn_female.IsChecked = false;
                    }
                    else if (mof == "FEMAL")
                    {
                        btn_female.IsChecked = true;
                        btn_male.IsChecked = false;
                    }
                    txtCNI.Text = reader["CNI"].ToString();
                    txtNumLoyer.Text = reader["NumLoyer"].ToString();
                    txtFiliere.Text = reader["Filiere"].ToString();
                    txtService.Text = reader["Service"].ToString();

                    //Load photo from database


                    string photoPath = reader["PHOTO"].ToString();
                    if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                    {
                        imgBoxUser.Source = new BitmapImage(new Uri(photoPath));
                    }
                    else
                    {
                        imgBoxUser.Source = null; // Clear the image if path is invalid
                    }

                }



            }
        }





        private void Button_Click(object sender, RoutedEventArgs e)
        {
            demande dem = new demande(txtEmail.Text);
            dem.Show();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            resetpass1 resetpass1 = new resetpass1(userEmail);
            resetpass1.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn(object sender, RoutedEventArgs e)
        {

        }

        private void btnBrowse_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                imgBoxUser.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {


            try
            {

                conn.Open();
                string query = "UPDATE U_SER SET PHOTO=@PHOTO WHERE EMAIL=@EMAIL";

                SqlCommand cmd = new SqlCommand(query, conn);


                cmd.Parameters.AddWithValue("@PHOTO", imagePath);

                cmd.Parameters.AddWithValue("@EMAIL", txtEmail.Text);


                cmd.ExecuteNonQuery();


                MessageBox.Show("تم التحديث بنجاح");
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: ");
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
}
