﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Util;
using Kwikee.Droid.DataModel;
using SQLite;

namespace Kwikee.Droid.Helpers
{
    public class DbHelper
    {
        private readonly string _path;
        private DbHelper _instance;
        private const string TAG = "DB_HELPER";

        private DbHelper ()
        {
            _path = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
            CreateDatabase ();
        }

        public DbHelper Instance
        {
            get
            {
                _instance = _instance ?? new DbHelper ();
                return _instance;
            }
        }

        private async void CreateDatabase ()
        {
            try
            {
                var connection = new SQLiteAsyncConnection (_path);
                await connection.CreateTableAsync<UserVideo> ();

            }
            catch (SQLiteException ex)
            {
                Log.Error(TAG, ex.Message);
            }
        }

        public async Task<string> InsertUpdateDataAsync (UserVideo data)
        {
            try
            {
                var db = new SQLiteAsyncConnection (_path);

                if (await db.InsertAsync (data) != 0)
                    await db.UpdateAsync (data);
                
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                Log.Error (TAG, ex.Message);
                return string.Empty;
            }
        }

        public async Task<int> GetNumberOfFavoriteVideosAsync ()
        {
            try
            {
                var db = new SQLiteAsyncConnection (_path);

                // this counts all records in the database, it can be slow depending on the size of the database
                var count = await db.ExecuteScalarAsync<int> ("SELECT Count(*) FROM FavVideoDataModel");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return count;
            }
            catch (SQLiteException ex)
            {
                Log.Error (TAG, ex.Message);
                return -1;
            }
        }

        public async Task<IEnumerable<string>> GetAllFavoriteVideosAsync ()
        {
            try
            {
                var db = new SQLiteAsyncConnection (_path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var list = await db.ExecuteScalarAsync<IEnumerable<string>> ("SELECT Id FROM UserVideo WHERE IsFavorite = true");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return list;
            }
            catch (SQLiteException ex)
            {
                Log.Error (TAG, ex.Message);
                return new List<string>();
            }
        }

        public async Task<IEnumerable<string>> GetAllWatchlistVideosAsync ()
        {
            try
            {
                var db = new SQLiteAsyncConnection (_path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var list = await db.ExecuteScalarAsync<IEnumerable<string>> ("SELECT Id FROM UserVideo WHERE IsInWatchlist = true");

                return list;
            }
            catch (SQLiteException ex)
            {
                Log.Error (TAG, ex.Message);
                return new List<string> ();
            }
        }
    }
}

