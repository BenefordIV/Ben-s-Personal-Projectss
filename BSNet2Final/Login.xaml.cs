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
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Register());
        }

        //When login button is clicked it will validate with the database to see if the information is correct
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            //Attempt to set up a relative path to the database
            //This came from StackOverflow. Not my own code.
            //But the executable string will be popuplated by the current location of the application executable (in the Bin/Debug folder of the BSNet2Final project folder)
            //The path string will then get all of the information and store it into a nother string based on the location of the executable string
            //Finally using AppDomain I set a specific string to the path. In this case DataDirectory is the absolute path of the database.
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);


            //connection string to connect to the DB using the absolute path of DataDirectory made from my AppDomain 
            string ConnString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|\BSNet2Final.accdb";
            //connection object to fully connect to the database
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                //Try catch block to validate user login information
                try
                {
                    //open the connection to the database
                    conn.Open();
                    //Create an OleDbCommand sql command and parameters (the parameters add a level of security to the code) it also has to connect
                    //to the connectino statement in order to work.
                    using (OleDbCommand login = new OleDbCommand("Select * from BSNet2 where AccountName = @Account and Password = @Password", conn))
                    {
                        //Add parameters into the sql statement
                        login.Parameters.AddWithValue("@Account", usernameTextBox.Text);
                        login.Parameters.AddWithValue("@Password", passwordPasswordBox.Password);

                        //This will check to see if the text in the username textbox is equal to the word admin. 
                        //if true it will run the query like normal but instead take them to the admin page rather
                        //than the welcome page
                        if (usernameTextBox.Text.Equals("admin"))
                        {
                            using (OleDbDataReader loginReader = login.ExecuteReader())
                            {
                                //if the username and password match go to the welcome page
                                if (loginReader.HasRows)
                                {
                                    //Go to the correct Admin Page
                                    this.NavigationService.Navigate(new Admin());
                                }
                                //If the username or password is incorrect it will be caught here
                                else
                                {
                                    MessageBox.Show("Invalid Username or Password");
                                    usernameTextBox.Clear();
                                    passwordPasswordBox.Clear();
                                }
                            }
                        }
                        //If the username is not admin. Take them to the normal welcome page
                        else
                        {
                            //Create a OleDbDataReader object to read the sql command and run the query
                            using (OleDbDataReader loginReader = login.ExecuteReader())
                            {
                                //if the username and password match go to the welcome page
                                if (loginReader.HasRows)
                                {
                                    //Go to the correct welcome page
                                    this.NavigationService.Navigate(new Welcome());
                                }
                                //If the username or password is incorrect it will be caught here
                                else
                                {
                                    MessageBox.Show("Invalid Username or Password");
                                    usernameTextBox.Clear();
                                    passwordPasswordBox.Clear();
                                }
                            }
                        }
                    }
                }
                
                catch(Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                //close the connection after the query is ran. Don't want the connection to stay constantly open!
                conn.Close();
            }
        }

        private void forgotPWButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ForgottenPassword());
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is a WPF Application that uses the OleDb library to create a fully functioning\n" +
                "login screen. It does this by hard coding a connection string in every query. \n" +
                "This application is only one Window but uses a Frame object to change the pages depending on buttons clicked. This makes the program run smoother and not be bogged down by multiple threads. \n" +
                "\nWhen the user creates an through the Register window, it will do an insert query into the database that is\n" +
                "in this file (the coding can probably be better to link the file so it's not 100% looking for the database on my F:\\n" +
                "Drive). In the database I only have two columns, a UserAccount column (that is a primary key [this means\n" +
                "that a user cannot insert the same username twice]) and a Password column that is used for reference in many queries.\n" +
                "\nIf the user has forgotten their password I have a seperate page that will ask for their username and then run a query that\n" +
                "selects the password from the table where the username = textinput. Then displays that password in an uneditable textbox.\n" +
                "\nLogin screen is a simple select query validation. If the username and password exist in the same row. Go to the next page.\n" +
                "\nI also have added the ability for an Admin page. In the login it will look to see if the username text is equal to admin, and if it is\n" +
                "it will run the login query and take them to a specific page where they can delete accounts based on an account name.\n"+
                "\nThe delete page is simple enough as it is. It will take the username as a parameter in a query and then will delete the account from the table.\n" +
                "\nAuthor and Programmer: Benjamin Charles Steier\n" +
                "Date: 12/10/17 11:18 PM\n" +
                ".Net II Final Project");
        }
    }
}
