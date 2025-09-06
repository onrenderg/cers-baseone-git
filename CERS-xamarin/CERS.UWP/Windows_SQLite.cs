using SQLite;
using System;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;
using CERS.UWP;

[assembly: Dependency(typeof(Windows_SQLite))]
namespace CERS.UWP
{
    public class Windows_SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "CERS.db";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
           // Console.WriteLine(path);
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}
