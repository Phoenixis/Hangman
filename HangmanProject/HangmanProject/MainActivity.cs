using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace HangmanProject
{
    [Activity(Label = "HangmanProject", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //Declaring Path
            string path = Path.Combine(System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal), "Steve.db3");

        Button Letter; //Submit Letter
        Button Word; //Submit Word
        Button Menu; //Menu
        Button NewGame; //New Game

        TextView Answer; //Answer box (Incorrect""/Correct"")
        TextView Letsdone; //Letter box ("a")
        TextView Hangman; //Hangman ("Mystery word")
        TextView YesScore; //Current Score
        TextView Livez; //Lives left

        EditText Abc; //Type Letter
        EditText txtWord; //Type word

        //Hangman Code
        Random random = new Random((int)DateTime.Now.Ticks);

        string wordToGuess;
        string Username;

        string wordToGuessUppercase;

        StringBuilder displayToPlayer;
           
        List<char> correctGuesses = new List<char>();
        List<char> incorrectGuesses = new List<char>();

        int lives = 8;
        bool won = false;
        int lettersRevealed = 0;
        int CurrentScore = 0;

        string input;
        char guess;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            string path = Path.Combine(System.Environment.GetFolderPath
               (System.Environment.SpecialFolder.Personal), "dbJohn.db3");

            //Get a random word 

            var db = new SQLiteConnection(path);
            var table = db.Table<Word>().ToList();

            Random r = new Random();
            int num = r.Next(1, table.Count());

            wordToGuess = table.ElementAt(num).hangmanword;

            wordToGuessUppercase = wordToGuess.ToUpper();
            displayToPlayer = new StringBuilder(wordToGuess.Length);

            for (int i = 0; i < wordToGuess.Length; i++)
                displayToPlayer.Append('*');

            //Declaring buttons
            Letter = FindViewById<Button>(Resource.Id.btnLetter);
            Word = FindViewById<Button>(Resource.Id.btnWord);
            Menu = FindViewById<Button>(Resource.Id.btnMenu);
            NewGame = FindViewById<Button>(Resource.Id.btnNew);

            //Declaring TextView
            Answer = FindViewById<TextView>(Resource.Id.txtAnswer);
            Letsdone = FindViewById<TextView>(Resource.Id.txtLetsdone);
            Hangman = FindViewById<TextView>(Resource.Id.txtHangman);
            YesScore = FindViewById<TextView>(Resource.Id.txtScoreGame);
            Livez = FindViewById<TextView>(Resource.Id.txtLives);

            //Declaring EditText
            Abc = FindViewById<EditText>(Resource.Id.txtLetter);
            txtWord = FindViewById<EditText>(Resource.Id.txtWord);

            Hangman.Text = displayToPlayer.ToString();
            
            //Click voids
            Letter.Click += Letter_Click;
            Word.Click += Word_Click;
            Menu.Click += Menu_Click;
            NewGame.Click += NewGame_Click;

            Abc.TextChanged += Abc_TextChanged;
            Letter.Enabled = false;

            txtWord.TextChanged += txtWord_TextChanged;
            Word.Enabled = false;
        }

        private void txtWord_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Word.Enabled = true;
            if (txtWord.Text == " ")
            {
                Word.Enabled = false;
            }
            if (txtWord.Text == "")
            {
                Word.Enabled = false;
            }
        }

        private void Abc_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Letter.Enabled = true;
            if (Abc.Text == " ")
            {
                Letter.Enabled = false;
            }
            if(Abc.Text == "")
            {
                Letter.Enabled = false;
            }
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            Recreate();
            Abc.Text = "";
            txtWord.Text = "";
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Intent NextActivity = new Intent(this, typeof(Menu));
            NextActivity.PutExtra("Username", Username);
            StartActivity(NextActivity);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Username= Intent.GetStringExtra("Username");
        }

        private void Word_Click(object sender, EventArgs e)
        {
            if (txtWord.Text == wordToGuess)
            {
                won = true;
                Toast.MakeText(this, "You Won!", ToastLength.Long).Show();
                Hangman.Text = wordToGuess;
                CurrentScore = CurrentScore + 100;
                SaveScore();
            }
            else
            {
                won = false;
                Toast.MakeText(this, "You Lost!", ToastLength.Long).Show();

                Abc.Enabled = false;
                txtWord.Enabled = false;
                Word.Enabled = false;
                Letter.Enabled = false;
                Hangman.Text = wordToGuess;
                CurrentScore = CurrentScore + 0;
                SaveScore();
            }
            Word.Enabled = false;
            txtWord.Text = "";
        }

        private void checkhangman()
        {

           if(!won && lives > 0)
            {
                input = Abc.Text.ToUpper();
                guess = input[0];

                if (correctGuesses.Contains(guess))
                { 
                }
                else if (incorrectGuesses.Contains(guess))
                {
                }

                if (wordToGuessUppercase.Contains(guess))
                {
                    correctGuesses.Add(guess);
                    CurrentScore = CurrentScore + 20;
                    YesScore.Text = CurrentScore.ToString();
                    Livez.Text = lives.ToString();

                    Letsdone.Text = Letsdone.Text + " " + Convert.ToString(input);
                    Answer.Text = "Correct!";

                    for (int i = 0; i < wordToGuess.Length; i++)
                    {
                        if (wordToGuessUppercase[i] == guess)
                        {
                            displayToPlayer[i] = wordToGuess[i];
                            lettersRevealed++;
                        }

                        Hangman.Text = displayToPlayer.ToString();
                    }

                    if (lettersRevealed == wordToGuess.Length)
                    {
                        won = true;
                        Toast.MakeText(this, "You Won!", ToastLength.Long).Show();
                        SaveScore();
                    }
                }
                else
                {
                    CurrentScore = CurrentScore - 10;
                    YesScore.Text = CurrentScore.ToString();
                    Letsdone.Text = Letsdone.Text + " " + Convert.ToString(input);
                    Answer.Text = "Incorrect";
                    incorrectGuesses.Add(guess);
                    lives--;
                    Livez.Text = lives.ToString();
                }

                if(lives <= 0)
                {
                    won = false;
                    Toast.MakeText(this, "You Lost!", ToastLength.Long).Show();

                    Hangman.Text = wordToGuess;

                    Abc.Enabled = false;
                    txtWord.Enabled = false;
                    Word.Enabled = false;
                    Letter.Enabled = false;

                    SaveScore();
                }
            }
        }

        private void Letter_Click(object sender, EventArgs e)
        {
            checkhangman();
            Abc.Text = "";
        }

        public void SaveScore()
        {
            string path = Path.Combine(System.Environment.GetFolderPath
            (System.Environment.SpecialFolder.Personal), "dbJohn.db3");

            // setup connection 
            var db = new SQLiteConnection(path);

            // create words table 
            db.CreateTable<Scored>();

          
            Scored s = new Scored();

            s.name = Username;
            s.Scores = CurrentScore;

            db.Insert(s);
        }
    }
}

