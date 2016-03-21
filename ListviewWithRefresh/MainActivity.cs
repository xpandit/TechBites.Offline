using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Java.Lang;
using System.IO;
using SQLite;
using System.Linq;
using ListviewWithRefresh.Utils;

namespace ListviewWithRefresh
{
    [Activity(Label = "ListviewWithRefresh", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, SwipeRefreshLayout.IOnRefreshListener
    {
        private List<ElementModel> bindedList = new List<ElementModel>();
        private SwipeListAdapter adapter;
        private SwipeRefreshLayout swipeToRefreshLayout;

        Random rnd = new Random();
        bool ApplicationStarted = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            createDatabase();

            SetContentView(Resource.Layout.Main);

            ListView listView = FindViewById<ListView>(Resource.Id.listview);
            swipeToRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);

            bindedList = new List<ElementModel>() { newElementModel(),newElementModel(), newElementModel(), };

            adapter = new SwipeListAdapter(this, bindedList);
            listView.Adapter = adapter;

            swipeToRefreshLayout.SetOnRefreshListener(this);
            swipeToRefreshLayout.Post(refreshFromDatabase);
        }

        public void OnRefresh()
        {
            swipeToRefreshLayout.Refreshing = true;

            try
            {
                refreshFromDatabase();
            }
            catch (UnexpectedNetworkErrorException e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
            }
            adapter.NotifyDataSetChanged();
            swipeToRefreshLayout.Refreshing = false;
        }

        public void addToBindedList()
        {
            bindedList.Add(newElementModel());
        }

        #region get all from services
        public void refreshAllFromServices()
        {
            bindedList = simulateGetAll();
            ApplicationStarted = true;
        }
        #endregion

        #region database

        string dbPath = "database.db";

        public void createDatabase()
        {
            dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbPath);

            if (File.Exists(dbPath))
                return;

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Execute("PRAGMA encoding='UTF-8'");
                connection.CreateTable<ElementModel>();

                connection.Commit();
                
                connection.InsertAll(new List<ElementModel>() { newElementModel(), newElementModel(), newElementModel() });
                connection.Commit();
            }
        }

        public void refreshFromDatabase()
        {
            insertDifferences();
            using (var connection = new SQLiteConnection(dbPath))
            {
                bindedList.Clear();
                bindedList.AddRange(connection.Table<ElementModel>().ToList());
            }
            ApplicationStarted = true;
        }

        #endregion

        #region database with diffs
        DateTime lastDifferenceUpdated = DateTime.Now;
        public void insertDifferences()
        {
            if (DateTime.Now.Subtract(lastDifferenceUpdated) < new TimeSpan(0, 0, 2))
                return;

            var diff = simulateGetDifferences(lastDifferenceUpdated);

            using (var connection = new SQLiteConnection(dbPath))
            {
                foreach(var a in diff)
                {
                    connection.InsertOrReplace(a);
                }

                connection.Commit();
            }
            lastDifferenceUpdated = DateTime.Now;
        }
        #endregion

        #region WebService Simulation

        public List<ElementModel> simulateGetAll()
        {
            Thread.Sleep(rnd.Next(200, 1500));

            if (ApplicationStarted && rnd.Next(3) == 1)
            {
                throw new UnexpectedNetworkErrorException("Unnexpected error in request");
            }
            bindedList.Add(newElementModel());
            return bindedList;
        }

        #region other methods
        private List<ElementModel> simulateGetDifferences(DateTime lastDifferenceUpdate)
        {
            Thread.Sleep(rnd.Next(200, 400));

            if (ApplicationStarted && rnd.Next(3) == 1)
            {
                throw new UnexpectedNetworkErrorException("Unnexpected error in request");
            }

            List<ElementModel> differences = new List<ElementModel>()
            {
                bindedList.Skip(rnd.Next(1, bindedList.Count - 1)).First(),
                newElementModel()
            };
            differences[0].Altered = DateTime.Now;

            return differences;
        }
        #endregion

        private int elem = 1;
        public ElementModel newElementModel()
        {
            return new ElementModel() { Id = elem, Abstract = "Element " + (elem++), Altered = DateTime.Now };
        }
        #endregion
    }
}

