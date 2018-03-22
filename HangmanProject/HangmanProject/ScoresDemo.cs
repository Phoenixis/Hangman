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
    class Scored
    {   
        //Score
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public double Scores { get; set; }
        
        public string name { get; set; }
    }
}