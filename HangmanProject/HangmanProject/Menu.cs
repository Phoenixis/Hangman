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
using Android.OS;
using Android.Content.Res;
using System.IO;
using SQLite;

namespace HangmanProject
{
    [Activity(Label = "Hangman", MainLauncher = false, Icon = "@drawable/icon")]
    public class Menu : Activity
    {
        Button Start; //Start Game
        Button Login; //Returning User Login
        Button Create; //Create Account

        TextView Username;
        TextView MyScore;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Menu);

            Start = FindViewById<Button>(Resource.Id.btnGame);
            Login = FindViewById<Button>(Resource.Id.btnLogin);
            Create = FindViewById<Button>(Resource.Id.btnCreate);

            Username = FindViewById<TextView>(Resource.Id.txtName);
            MyScore = FindViewById<TextView>(Resource.Id.txtScoreD);

            Create.Click += Create_Click;
            Start.Click += Start_Click;
            Login.Click += Login_Click;
            Start.Enabled = false;

            //MyScore.Text = Intent.GetStringExtra("");
            //UserID = Intent.GetIntExtra("Id"), 0);

            // read from asset file 

            string word;

            // path to database
            string path = Path.Combine(System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal), "dbJohn.db3");

            // setup connection 
            var db = new SQLiteConnection(path);

            // create words table 
            db.CreateTable<Word>();

            var table = db.Table<Word>();

            AssetManager assets = this.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("Words.txt")))
            {
                while ((word = sr.ReadLine()) != null)
                {
                    // Add word to the database 
                    Word w = new Word();
                    w.hangmanword = word;
                    db.Insert(w);
                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();


            var test = Intent.GetStringExtra("Username");

            if (test == null)
            {


            }
            else
            {
                Username.Text = Intent.GetStringExtra("Username");
            }

            if (Username.Text == "No User Logged in.")
            {
                Start.Enabled = false;
            }
            else
            {
                Start.Enabled = true;
            }

            // get all scores from the database  and show it in a listview 

            string path = Path.Combine(System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal), "dbJohn.db3");

            // setup connection 
            var db = new SQLiteConnection(path);

            db.CreateTable<Scored>();

            var scores = db.Table<Scored>().OrderByDescending(p => p.Scores).Take(10).ToList();

            foreach (var score in scores)
            {
                MyScore.Text = MyScore.Text + "\n" + score.name + "  " + score.Scores;
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            Intent NextActivity = new Intent(this, typeof(Login));

            StartActivity(NextActivity);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Intent NextActivity = new Intent(this, typeof(MainActivity));
            NextActivity.PutExtra("Username", Username.Text);
            StartActivity(NextActivity);
        }

        private void Create_Click(object sender, EventArgs e)
        {
            Intent NextActivity = new Intent(this, typeof(Create));

            StartActivity(NextActivity);
        }
    }
}