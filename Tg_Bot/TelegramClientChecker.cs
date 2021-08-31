using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using Telegram.Bot;
using Tg_Bot.ServiceClass;

namespace Tg_Bot
{
    static class TelegramClientCheck
    {
        //static string DBName { get; } = FileName.MainDir+"db.db";
        public static bool NewCheckOfUser_Result
        {
            get { return NewCheckOfUser_Result; }
            private set { NewCheckOfUser_Result = value; }
        }

        public static bool IsFriend(Telegram.Bot.Types.User user)
        {

            bool isFriend = false;

            if (!File.Exists(FileName.DBName))
                throw new KNTHelperBotException($"no Exist {FileName.DBName}", "create or add this file");

            using (SqliteConnection connection = new SqliteConnection($"Data Source={FileName.DBName}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                
                command.CommandText = 
                    $"SELECT * FROM FriendUsers " +
                    $"WHERE Tg_id = '{user.Id}'";
                
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    isFriend = reader.HasRows;
                }
                if (!isFriend)
                {
                    command.CommandText =
                        $"INSERT INTO BlackListUsers " +
                        $"(Tg_id, Name, UserName) Values ('{user.Id}','{user.FirstName}','{user.Username}')";

                    command.ExecuteNonQuery();
                }
            }

            return isFriend;
        }

        public static bool InBlackList(Telegram.Bot.Types.User user)
        {
            bool hasInBL = false;

            using (SqliteConnection connection = new SqliteConnection($"Data Source ={FileName.DBName}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;

                command.CommandText = 
                    $"SELECT * FROM BlackListUsers " +
                    $"WHERE Tg_id = '{user.Id}'";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    hasInBL = reader.HasRows;
                }

            }
            return hasInBL;
        }

        public static bool IsAdmins(Telegram.Bot.Types.User user)
        {
            bool isAdmin = false;

            using (SqliteConnection connection = new SqliteConnection($"Data Source = {FileName.DBName}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;

                command.CommandText = 
                    $"SELECT * FROM AdminUsers " +
                    $"WHERE Tg_id = {user.Id}";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    isAdmin = reader.HasRows;
                }
            }

            return isAdmin;
        }
        public static void AddToNewUsers(Telegram.Bot.Types.User user)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source = {FileName.DBName}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;

                bool isHas = false;
                command.CommandText =
                    $"SELECT * FROM NewUsers " +
                    $"WHERE Tg_id = {user.Id}";

                using (SqliteDataReader reader = command.ExecuteReader())
                    isHas = reader.HasRows;

                if (!isHas)
                {
                    if (user.Username != null || user.Username != string.Empty)
                        command.CommandText =
                            $"INSERT INTO NewUsers(Tg_id, Name, Username) " +
                            $"VALUES ('{user.Id}', '{user.FirstName}', '{user.Username}')";
                    else
                        command.CommandText =
                            $"INSERT INTO NewUsers(Tg_id, Name) " +
                            $"VALUES ('{user.Id}', '{user.FirstName}')";
                }
                command.ExecuteNonQuery();
            }
        }
    }
}
