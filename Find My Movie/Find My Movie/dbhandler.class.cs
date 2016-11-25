using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace Find_My_Movie {
    class dbhandler {
        public dbhandler() {

            // creating new SQLite DB file
            //SQLiteConnection.CreateFile("FMMDb.sqlite");

            // create new SQLiteConnection to database
            SQLiteConnection connection = new SQLiteConnection("Data Source=FMMDb.db;Version=3;");
            // open connection to db, if the db doesn't exists, it will be created
            connection.Open();

            // create tables for db
            string sql = "create table highscores (name varchar(20), score int)";

            SQLiteCommand command = new SQLiteCommand(sql, connection);

            //command.ExecuteNonQuery();


            sql = "insert into highscores (name, score) values ('Me', 9001)";
            command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();

            sql = "select * from highscores order by score desc";
            command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            MessageBox.Show(reader.Read().ToString());
            while (reader.Read()) {
                MessageBox.Show(reader["id"].ToString() + " Name : " + reader["name"] + " Score : " + reader["score"]);
            }

            connection.Close();
        }
    }
}
