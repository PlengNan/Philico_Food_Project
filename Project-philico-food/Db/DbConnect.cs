using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Project_philico_food.Db
{
    internal class DbConnect
    {
        public static SQLiteConnection SQLiteConnection = new SQLiteConnection("Data Source=Thaiscale_std.db;Version=3;");

        public static bool connect()
        {
            try
            {
                if (SQLiteConnection.State != ConnectionState.Open)
                    SQLiteConnection.Open();

                EnsureSchema();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        private static void EnsureSchema()
        {
            using (var cmd = new SQLiteCommand(@"
                CREATE TABLE IF NOT EXISTS Users(
                    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name      TEXT,
                    Email     TEXT,
                    Phone     TEXT,
                    Username  TEXT UNIQUE,
                    Password  TEXT,
                    IsActive  INTEGER NOT NULL DEFAULT 1
                );", SQLiteConnection))
            {
                cmd.ExecuteNonQuery();
            }

            using (var idx = new SQLiteCommand(
                "CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username);",
                SQLiteConnection))
            {
                idx.ExecuteNonQuery();
            }
        }
    }
}
