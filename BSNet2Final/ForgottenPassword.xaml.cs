using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.OleDb;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BSNet2Final
{/**
    This section of code will get the password if a user has forgotten their password is forgotten
        I still am working on an updating password 
    */
    /// <summary>
    /// Interaction logic for ForgottenPassword.xaml
    /// </summary>
    public partial class ForgottenPassword : Page
    {
        public ForgottenPassword()
        {
            InitializeComponent();
        }

        private void getPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            //Set up relative path
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            //Connect to the database

            //connection string to connect to the DB using the absolute path of DataDirectory made from my AppDomain 
            string ConnString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|\BSNet2Final.accdb";
            //connection object to fully connect to the database
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                try
                {
                    //open connection
                    conn.Open();

                    //Create an OleDbCommand that selects the password from the BSNet2 Table where the username = the text input
                    using(OleDbCommand forgotPassword = new OleDbCommand("Select Password from BSNet2 where AccountName = @Account", conn))
                    {
                        //add the parameter
                        forgotPassword.Parameters.AddWithValue("@Account", forgotPWUsernameTextBox.Text);
                        
                        //Create the reader object to run the query
                        using(OleDbDataReader forgotPWReader = forgotPassword.ExecuteReader())
                        {
                            //If this is all correct, while the reader object is able to read it will populate the 
                            //listbox with the password that the user has corresponding with the account.
                            if (forgotPWReader.Read())
                            {
                                forgottenPasswordDisplay.Items.Add(forgotPWReader["Password"].ToString());
                                forgotPWUsernameTextBox.Clear();
                            }

                            else
                            {
                                MessageBox.Show("Invalid Username");
                                forgotPWUsernameTextBox.Clear();
                            }
                                
                        }
                    }
                }
                catch(Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                //close data connection
                conn.Close();
            }
        }

        //Navigate back to the login screen
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
