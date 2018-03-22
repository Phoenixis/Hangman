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
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        //Declaring Path
        string path = Path.Combine(System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal), "Steve.db3");

        Button btnLogin; //Login
        Button Menu; //Menu

        EditText User; //Username
        EditText Pass; //Password

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LoginUser);

            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            Menu = FindViewById<Button>(Resource.Id.btnMenu);

            User = FindViewById<EditText>(Resource.Id.txtUser);
            Pass = FindViewById<EditText>(Resource.Id.txtPassword);

            btnLogin.Click += BtnLogin_Click;
            Menu.Click += Menu_Click;

            User.TextChanged += User_TextChanged;
            Pass.TextChanged += Pass_TextChanged;
        }

        private void Pass_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btnLogin.Enabled = true;
            if (Pass.Text == "" || User.Text == "")
            {
                btnLogin.Enabled = false;
            }
        }

        private void User_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            btnLogin.Enabled = true;
            if (Pass.Text == "" || User.Text == "")
            {
                btnLogin.Enabled = false;
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Intent NextActivity = new Intent(this, typeof(Menu));

            StartActivity(NextActivity);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {

            var db = new SQLiteConnection(path);
            var usercheck = db.Table<Accounts>().Where(p => p.ZUsername == User.Text && p.ZPassword == Pass.Text).ToList();

            if (usercheck.Count() > 0)
            {
                Intent NextActivity = new Intent(this, typeof(Menu));

                NextActivity.PutExtra("Username", usercheck[0].ZUsername);
                NextActivity.PutExtra("UserID", usercheck[0].UserID);

                StartActivity(NextActivity);
            }
        }
    }
}