using FM6_H.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Borders;
using iText.Pdfa;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using Microsoft.Xaml.Behaviors.Layout;
using iText.IO.Font.Constants;



namespace FM6_H
{
    public partial class admin_dash : Window
    {
        SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString());
        string SEXE;
        private List<string> items = new List<string>();

        public admin_dash()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }










        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
        }

        private void btn_female_Checked(object sender, RoutedEventArgs e)
        {
            SEXE = "FEMAL";
        }

        private void btn_male_Checked(object sender, RoutedEventArgs e)
        {
            SEXE = "MALE";
        }

        public DataTable loadusertable()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM DB_FM6";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        private string imagePath;

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




        private void btnAjouter(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtLastname.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCNI.Text) ||
                string.IsNullOrWhiteSpace(txtNumLoyer.Text) ||
                string.IsNullOrWhiteSpace(txtFiliere.Text) ||
                string.IsNullOrWhiteSpace(txtService.Text) ||
                string.IsNullOrWhiteSpace(txtMDP.Text) || 
                string.IsNullOrWhiteSpace(txtType.Text) || 
                string.IsNullOrWhiteSpace(imagePath)) 
            {
                MessageBox.Show("المرجو ملء جميع المعلومات");
            }
            else
            {
                try
                {
                    conn.Open();
                    SEXE = btn_male.IsChecked == true ? "MALE" : "FEMAL"; 
                    string query = "INSERT INTO U_SER (NOM, PRENOM, SEXE, EMAIL, MDP, TYPE, CNI, NumLoyer, FILIERE, SERVICE, PHOTO) " +
                                   "VALUES (@NOM, @PRENOM, @SEXE, @EMAIL, @MDP, @TYPE, @CNI, @NumLoyer, @FILIERE, @SERVICE, @PHOTO)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NOM", txtName.Text);
                    cmd.Parameters.AddWithValue("@PRENOM", txtLastname.Text);
                    cmd.Parameters.AddWithValue("@SEXE", SEXE);
                    cmd.Parameters.AddWithValue("@EMAIL", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@MDP", txtMDP.Text); 
                    cmd.Parameters.AddWithValue("@TYPE", txtType.Text);
                    cmd.Parameters.AddWithValue("@CNI", txtCNI.Text);
                    cmd.Parameters.AddWithValue("@NumLoyer", txtNumLoyer.Text);
                    cmd.Parameters.AddWithValue("@FILIERE", txtFiliere.Text);
                    cmd.Parameters.AddWithValue("@SERVICE", txtService.Text);
                    cmd.Parameters.AddWithValue("@PHOTO", imagePath); 

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم إضافة المستخدم بنجاح");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ ما ! المرجو التحقق من المعطيات جيدا"/* + ex.Message*/);
                }
                finally
                {
                    conn.Close();
                }


                // Clear the ComboBox selection and items
                SearchableComboBox.SelectedItem = null;

                // Clear the text fields

                txtName.Clear();
                txtLastname.Clear();
                txtEmail.Clear();
                btn_male.IsChecked = false;
                btn_female.IsChecked = false;
                txtCNI.Clear();
                txtNumLoyer.Clear();
                txtFiliere.Clear();
                txtService.Clear();
                txtType.Clear();
                txtMDP.Clear();
                imgBoxUser.Source = null;

                // Refill the ComboBox with updated data

                FillComboBox();
            }
        }















        public class User
        {
            public int ID_USER { get; set; }
            public string NOM { get; set; }
            public string PRENOM { get; set; }
            public string EMAIL { get; set; }
            public string SEXE { get; set; }
            public string CNI { get; set; }
            public string NumLoyer { get; set; }
            public string Filiere { get; set; }
            public string Service { get; set; }
            public string MDP { get; set; }
            public string TYPE { get; set; }
            public string PHOTO { get; set; }

            public override string ToString()
            {
                return $"{NOM} {PRENOM}";
            }

        }

        private void FillComboBox()
        {



            try
            {

                // Check if the connection is already open
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }



                conn.Open();
                string query = "SELECT ID_USER, NOM, PRENOM FROM U_SER";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                SearchableComboBox.Items.Clear();
                items.Clear();
                while (reader.Read())
                {
                    User user = new User()
                    {
                        ID_USER = (int)reader["ID_USER"],
                        NOM = reader["NOM"].ToString(),
                        PRENOM = reader["PRENOM"].ToString()
                    };
                    SearchableComboBox.Items.Add(user);
                    items.Add(user.NOM + " " + user.PRENOM);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" حدث خطأ ما ! " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filteredUsers = SearchableComboBox.Items.OfType<User>()
                .Where(user => (user.NOM + " " + user.PRENOM).ToLower().Contains(searchText))
                .ToList();

            SearchableComboBox.ItemsSource = filteredUsers;
            SearchableComboBox.IsDropDownOpen = true;
        }


        private void SearchableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (SearchableComboBox.SelectedItem is User selectedUser)
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM U_SER WHERE ID_USER = @ID_USER";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID_USER", selectedUser.ID_USER);
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
                        txtMDP.Text = reader["MDP"].ToString();
                        txtType.Text = reader["TYPE"].ToString();

                        // Load photo from database
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
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ ما ! " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }



        }


        /*******   UPDATE ******/



        private void btnUpdate(object sender, RoutedEventArgs e)
        {
            if (SearchableComboBox.SelectedItem is User selectedUser)
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE U_SER SET NOM=@NOM, PRENOM=@PRENOM, SEXE=@SEXE, EMAIL=@EMAIL, CNI=@CNI, FILIERE=@FILIERE, SERVICE=@SERVICE,MDP=@MDP,TYPE=@TYPE,PHOTO=@PHOTO, NumLoyer=@NumLoyer WHERE ID_USER=@ID_USER";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@NOM", txtName.Text);
                    cmd.Parameters.AddWithValue("@MDP", txtMDP.Text);
                    cmd.Parameters.AddWithValue("@TYPE", txtType.Text);
                    cmd.Parameters.AddWithValue("@PHOTO", imagePath);
                    cmd.Parameters.AddWithValue("@PRENOM", txtLastname.Text);
                    cmd.Parameters.AddWithValue("@SEXE", (btn_male.IsChecked == true) ? "MALE" : "FEMAL");
                    cmd.Parameters.AddWithValue("@EMAIL", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@CNI", txtCNI.Text);
                    cmd.Parameters.AddWithValue("@FILIERE", txtFiliere.Text);
                    cmd.Parameters.AddWithValue("@SERVICE", txtService.Text);
                    cmd.Parameters.AddWithValue("@NumLoyer", txtNumLoyer.Text);
                    cmd.Parameters.AddWithValue("@ID_USER", selectedUser.ID_USER);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("تم تحديث المستخدم بنجاح");


                    // Clear the ComboBox selection and items
                    SearchableComboBox.SelectedItem = null;

                    // Clear the text fields
                    txtName.Clear();
                    txtLastname.Clear();
                    txtEmail.Clear();
                    btn_male.IsChecked = false;
                    btn_female.IsChecked = false;
                    txtCNI.Clear();
                    txtNumLoyer.Clear();
                    txtFiliere.Clear();
                    txtService.Clear();
                    txtType.Clear();
                    txtMDP.Clear();
                    imgBoxUser.Source = null;
                    // Refill the ComboBox with updated data
                    FillComboBox();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ ما ! " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("المرجو اختيار مستخدم من القائمة");
            }
        }





        /*******   DELETE ******/
        private void btnDelete(object sender, RoutedEventArgs e)
        {
            if (SearchableComboBox.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show("هل أنت متأكد من أنك تريد حذف هذا المستخدم؟", "تحذير", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM U_SER WHERE ID_USER=@ID_USER";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID_USER", selectedUser.ID_USER);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("تم حذف المستخدم بنجاح");



                        // Remove the deleted user from the ComboBox
                        SearchableComboBox.Items.Remove(selectedUser);

                        //clear the text fields
                        txtName.Clear();
                        txtLastname.Clear();
                        txtEmail.Clear();
                        btn_male.IsChecked = false;
                        btn_female.IsChecked = false;
                        txtCNI.Clear();
                        txtNumLoyer.Clear();
                        txtFiliere.Clear();
                        txtService.Clear();
                        txtType.Clear();
                        txtMDP.Clear();
                        imgBoxUser.Source = null;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("المرجو اختيار مستخدم من القائمة");
            }
        }

        private void appsbtn_Click(object sender, RoutedEventArgs e)
        {


            if (SearchableComboBox.SelectedItem is User selectedUser)
            {
                accessApps accessAppsWindow = new accessApps(selectedUser.ID_USER);
                accessAppsWindow.Show();


            }


            //accessApps acessApps = new accessApps(selectedUser.ID_USER);
            //acessApps.Show();

        }

        private void infochange_Click(object sender, RoutedEventArgs e)
        {
            infoChangeTab infoChangeTab = new infoChangeTab();
            infoChangeTab.Show();
        }

        private void pswdchange_Click(object sender, RoutedEventArgs e)
        {
            mdpRequests mdpRequests = new mdpRequests();
            mdpRequests.Show();
        }


      



        // Call this method when needed, such as after a button click:
        private void BtnCreatePDF_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                List<string> textElements = new List<string>
        {
            "User Name: " + txtName.Text,
            "Last Name: " + txtLastname.Text,
            "Email: " + txtEmail.Text,
            "CNI: " + txtCNI.Text,
            "NumLoyer: " + txtNumLoyer.Text,
            "Filiere: " + txtFiliere.Text,
            "Service: " + txtService.Text,
            "Type: " + txtType.Text
        };

                string outputDirectory = @"C:\Users\User\Downloads";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                string fileName = @"C:\Users\User\Downloads\myyuserDetails_.Pdf";
                PdfWriter writer = new PdfWriter(fileName);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("This is a test PDF document."));

                document.Close();


                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }
        }




    }

}
