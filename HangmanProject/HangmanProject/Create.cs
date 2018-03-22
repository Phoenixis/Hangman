using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;

namespace HangmanProject
{
    [Activity(Label = "Hangman", MainLauncher = false)]
    public class Create : Activity
    {
        string path = Path.Combine(System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal), "Steve.db3");

        Button btnCreate; //Create Account
        Button Menu; //Menu 

        EditText Name; //Username
        EditText Pass; //Password
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NewUser);

            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            Menu = FindViewById<Button>(Resource.Id.btnMenu);

            Name = FindViewById<EditText>(Resource.Id.txtName);
            Pass = FindViewById<EditText>(Resource.Id.txtPass);

            btnCreate.Click += BtnCreate_Click;
            Menu.Click += Menu_Click;

            btnCreate.Enabled = false;

            Name.TextChanged += Name_TextChanged;
            Pass.TextChanged += Pass_TextChanged;
        }

        private void Pass_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btnCreate.Enabled = true;
            if (Pass.Text == "" || Name.Text == "")
            {
                btnCreate.Enabled = false;
            }
            
        }

        private void Name_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btnCreate.Enabled = true;
            if (Name.Text == "" || Pass.Text == "")
            {
                btnCreate.Enabled = false;
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Intent NextActivity = new Intent(this, typeof(Menu));
            
            StartActivity(NextActivity);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Are you sure you wish to make this account?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {
                Toast.MakeText(this, "Account Created! Welcome " + Name.Text + "!", ToastLength.Long).Show();

                Toast.MakeText(this, "Path to personal folder: " + path, ToastLength.Long).Show();

            //Setup Connection
            var db = new SQLiteConnection(path);

            //Setup Table
            db.CreateTable<Accounts>();

            //Create New Contact
            Accounts myAccount = new Accounts();

            myAccount.ZUsername = Name.Text;
            myAccount.ZPassword = Pass.Text;

            db.Insert(myAccount);

            db.CreateTable<Accounts>();

            Accounts newUser = new Accounts();

            newUser.ZUsername = Name.Text;
            newUser.ZPassword = Pass.Text;
            newUser.Highscore = 0;

            db.Insert(newUser); ;

            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
            
        }
    }
}