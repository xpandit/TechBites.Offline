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

namespace ListviewWithRefresh
{
    public class ElementModel
    {
        [PrimaryKey, AutoIncrement]
        public int InternalId { get; set; }
        public int Id { get; set; }
        public DateTime Altered { get; set; }
        public String Abstract { get; set; }
    }
}