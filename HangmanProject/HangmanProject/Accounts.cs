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

namespace HangmanProject
{
    class Accounts
    {
        [PrimaryKey, AutoIncrement]

        public int UserID { get; set; }

        public string ZUsername { get; set; }

        public string ZPassword { get; set; }

        public int Highscore { get; set; }

        public Accounts()
        {
            ZUsername = "";
            ZPassword = "";
        }
    }
}