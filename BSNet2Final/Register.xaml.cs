using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.OleDb;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BSNet2Final
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        //Return to the login screen if they click the cancel button
        private void cancelRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }

        //Run an input query when the register button is clicked. This is very
        //Much like the validation query in the Login Screen, but will do an insert command
        //Instead of a Select command
        private void registerAccountButton_Click(object sender, RoutedEventArgs e)
        {
            //Have to do the connection string again

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
                    //open the connection
                    conn.Open();

                    //create an OleDbCommand with an insert command
                    //It uses parameters again because of extra security. Becuase of the parameters I do not have to put the parameters in quotes
                    //Since the parameters are reading the string already.
                    //Connect it to the connection object
                    using(OleDbCommand insert = new OleDbCommand("Insert into BSNet2 values (@UserAccount, @UserPassword)", conn))
                    {
                        insert.Parameters.AddWithValue("@UserAccount", registerUsernameTextBox.Text);
                        insert.Parameters.AddWithValue("@Password", registerPasswordPasswordBox.Password);

                        if (registerUsernameTextBox.Text.Contains(" ") || registerPasswordPasswordBox.Password.Contains(" "))
                        {
                            MessageBox.Show("Username or Password cannot contain a space");
                            registerPasswordPasswordBox.Clear();
                            registerUsernameTextBox.Clear();
                        }
                        else
                        {
                            //Create a reader object to actually run the query
                            using (OleDbDataReader insertReader = insert.ExecuteReader())
                            {
                                MessageBox.Show("Thank you for creating the account " + registerUsernameTextBox.Text);
                                //Take you back to the login screen so that you can access your brand new account
                                this.NavigationService.Navigate(new Login());
                            }
                        }
                    }
                }
                //this will sql command will error out if there is already an account made with the username
                //Since in my database I have set the username to be a primary key (meaning it can't be repeated).
                catch(Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                //close the database connection
                conn.Close();
            }
        }
    }
}
