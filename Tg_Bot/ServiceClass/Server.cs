﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Server
{
    class Server
    {
        //public string Ip { get; set; } = "192.168.1.10";
        public delegate void CreateEventHandler();
        public event CreateEventHandler NewEvent;
        public int Port { get; set; } = 8085;

        public string Result { get; private set; }
        public string ClientStatusOperation { get; private set; }
        private Socket listener;
        private string mainDir = Tg_Bot.ServiceClass.FileName.MainDir;
        private string OkCode { get; set; } = "\0";
        private char Separator { get; } = '|';
        private Socket tcpSocket;
        public Server() {}
        public async void  TurnOnAsync()
        {
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Any, Port);

            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(4);
            await Task.Run(()=>
            {
                while (true)
                {
                    listener = tcpSocket.Accept();

                    string data = AnswerFromClient();   // for code
                    Console.WriteLine($"Server -> {(CodeForServer)byte.Parse(data.ToString())}");
                    if (data != string.Empty)
                        ChoicOfAction((CodeForServer)byte.Parse(data.ToString()));

                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();

                }
            });
            
        }
        private string AnswerFromClient()
        {
            int size = listener.ReceiveBufferSize;
            var buffer = new byte[size];

            StringBuilder data = new StringBuilder();

            do
            {
                size = listener.Receive(buffer);

                data.Append(Encoding.UTF8.GetString(buffer, 0, size));

            } while (listener.Available > 0);

            return data.ToString();
        }
        private void ChoicOfAction(CodeForServer code)
        {
            switch (code)
            {
                case CodeForServer.GetFiles:
                    GetFilesName();
                    break;
                case CodeForServer.GetTextFromFile:
                    SendTextFromFile();
                    break;
                case CodeForServer.SetTextFromFile:
                    SetTextFromFile();
                    break;
                case CodeForServer.CreateNewFile:
                    CreateNewFile();
                    break;
                case CodeForServer.CreateEvent:
                    CreateEvent();
                    break;
                case CodeForServer.ForConnect:
                    ReConnect();
                    break;
                case CodeForServer.EditFileName:
                    EditFileName();
                    break;
                case CodeForServer.DeleteFile:
                    DeleteFile();
                    break;
                case CodeForServer.GetFilesFromDir:
                    GetFilesFromDir();
                    break;
                case CodeForServer.GetTableNames:
                    GetTablesFromDB();
                    break;
                case CodeForServer.GetUsers:
                    GetUsers();
                    break;
                default:
                    break;
            }
        }
        private void GetFilesName()
        {
            string[] dir = Directory.GetDirectories(mainDir);
            for(int i = 0; i < dir.Length; i++)
                dir[i] = dir[i].Insert(dir[i].Length, "...");

            string dirAnswer = string.Join(Separator, dir);
            SendData(dirAnswer);

            AnswerFromClient();

            string[] files = Directory.GetFiles(mainDir);
            string answer = string.Join(Separator, files);

            SendData(answer);
            Console.WriteLine($"Server -> GetFileName\n");
        }
        private void GetUsers()
        {
            SendData(OkCode);

            var result = new List<string>();
            using (var connection = new SqliteConnection($"Data Source={Tg_Bot.ServiceClass.FileName.DBName}"))
            {
                connection.Open();
                var commond = connection.CreateCommand();

                commond.CommandText = "SELECT * FROM  NewUsers";

                using (var reader = commond.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"].ToString());
                        }
                    }
                }

            }
            SendData(string.Join(Separator, result.ToArray()));

        }
        private void SendTextFromFile()
        {
            SendData(OkCode);

            string fileName = AnswerFromClient();
            Console.WriteLine($"Server -> File name:\t{fileName}\n");

            string text;
            using (FileStream stream = new FileStream(mainDir + @fileName, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
                text = reader.ReadToEnd();

            SendData(text);

        }
        private async void SetTextFromFile()
        {
            SendData(OkCode);


            string fileName = AnswerFromClient();
            Console.WriteLine($"Server -> File name:\t{fileName}\n");

            SendData(OkCode);

            string text = AnswerFromClient();
            Console.WriteLine($"Server -> Text:\n{text}\n****\n");

            //using (File.Create(fileName)) { }

            using (FileStream fstream = new FileStream(mainDir + fileName, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fstream))
                await writer.WriteAsync(text);
        }

        private void CreateNewFile()
        {
            SendData(OkCode);


            string fileName = AnswerFromClient();
            Console.WriteLine($"Server -> File name for Create:\t{fileName}\n");

            if (File.Exists(mainDir + fileName))
            {
                SendData("Server -> File Exist!");
                return;
            }

            using (File.Create(mainDir + fileName))
                SendData(OkCode);
        }
        
        public (string, string) LastEvent { get; private set; }
        private void CreateEvent()
        {
            SendData(OkCode);

            string date = AnswerFromClient();

            SendData(OkCode);

            string msg = AnswerFromClient();
            LastEvent = (date: date, msg: msg);
            NewEvent?.Invoke();
            
            //Console.WriteLine($"Server -> dateTime: {date}\nmsg:\n{msg}\n****\n");
            //Console.WriteLine($"Server -> dateTime: {DateTime.Now.ToUniversalTime()}\nmsg:\n{msg}\n****\n");

            Result = date + Separator + msg;
        }
        private void EditFileName()
        {
            SendData(OkCode);

            string oldFileName = AnswerFromClient();

            SendData(OkCode);

            string newFileName = AnswerFromClient();

            File.Move(mainDir+oldFileName, mainDir+newFileName);

        }
        private void DeleteFile()
        {
            SendData(OkCode);
            string fileName = AnswerFromClient();

            File.Delete(mainDir+fileName);
        }
        private void GetFilesFromDir()
        {
            SendData(OkCode);

            string getDir = AnswerFromClient();
            string[] dir = Directory.GetDirectories(mainDir + getDir);
            for (int i = 0; i < dir.Length; i++)
                dir[i] = dir[i].Insert(dir[i].Length, "...");

            string dirAnswer = string.Join(Separator, dir);

            if (dirAnswer != string.Empty)
                SendData(dirAnswer);
            else
                SendData(OkCode);

                AnswerFromClient();

            string[] files = Directory.GetFiles(mainDir + getDir);
            string answer = string.Join(Separator, files);

            SendData(answer);
            Console.WriteLine($"Server -> GetFileName\n");
        }
        private void GetTablesFromDB()
        {
            SendData(OkCode);

            var result = new List<string>();
            using (var connection = new SqliteConnection($"Data Source={Tg_Bot.ServiceClass.FileName.DBName}"))
            {
                connection.Open();
                var commond = connection.CreateCommand();

                commond.CommandText = "SELECT * FROM sqlite_sequence";

                using(var reader = commond.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["name"].ToString());
                        }
                    }
                }

            }
            SendData(string.Join(Separator, result.ToArray()));
        }
        private void SendData(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            listener.SendBufferSize = byteData.Length;
            listener.Send(byteData);
        }
        private void ReConnect()
        {
            SendData(OkCode);
        }
    }

    enum CodeForServer
    {
        None, ForConnect, GetFiles, GetTextFromFile, SetTextFromFile, CreateNewFile, CreateEvent, EditFileName, DeleteFile, GetFilesFromDir, GetTableNames, GetUsers
    }
}
