using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Telegram.Bot;

namespace Tg_Bot.ServiceClass
{
    class ScheduledMsg
    {
        public async void SenderAllNewUsers(DateTime dateTime, TelegramBotClient client, string msg)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (dateTime <= DateTime.Now)
                    {
                        using (SqliteConnection connection = new SqliteConnection($"Data Source ={FileName.DBName}"))
                        {
                            connection.Open();
                            SqliteCommand command = new SqliteCommand();

                            command.Connection = connection;

                            command.CommandText = "SELECT * FROM NewUsers";

                            using (SqliteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        client.SendTextMessageAsync(long.Parse(reader["Tg_id"].ToString()), $"{msg}, {reader["Name"]}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            });

        }
    }
}
