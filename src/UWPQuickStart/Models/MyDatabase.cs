using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPQuickStart.Models
{
    public class MyDatabase
    {
        string path;
        SQLiteConnection conn;

        public MyDatabase()
        {
            path =
              Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "MyDatabase.sqlite");
            conn =
                new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            //create table
            conn.CreateTable<User>();
            conn.CreateTable<EventModel>();

        }
        public int Register(User user)
        {
            var query = conn.Table<User>().
                Where(u => u.UserName == user.UserName && u.Email == user.Email);
            if (query.Count() > 0)
            {
                return 0;

            }

            return conn.Insert(new User()
            {
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email
            });

        }

        public bool Login(string user, string password)
        {
            var query = conn.Table<User>().
                Where(u => u.UserName == user && u.Password == password);
            if (query.Count() > 0)
                return true;
            else
                return false;
        }
    }
}

