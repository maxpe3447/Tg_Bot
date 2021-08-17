using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        public string Ip { get; set; } = "192.168.1.10";
        public int Port { get; set; } = 8085;

        public string Result { get; private set; }
        public string ClientStatusOperation { get; private set; }
        private Socket listener;
        private string dir = "ServiceFiles/";
        private string OkCode { get; set; } = "\0";
        private Socket tcpSocket;
        public Server()
        {
            
        }
        public void TurnOn()
        {
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Any, Port);

            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(4);
            
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
                default:
                    break;
            }
        }
        private void GetFilesName()
        {
            string[] files = Directory.GetFiles(dir);
            string answer = string.Join('|', files);

            SendData(answer);
        }
        private void SendTextFromFile()
        {
            SendData(OkCode);

            string fileName = AnswerFromClient();
            Console.WriteLine($"Server -> File name:\t{fileName}\n");

            string text;
            using (FileStream stream = new FileStream(dir + fileName, FileMode.Open))
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

            using (FileStream fstream = new FileStream(dir + fileName, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fstream))
                await writer.WriteAsync(text);
        }

        private void CreateNewFile()
        {
            SendData(OkCode);


            string fileName = AnswerFromClient();
            Console.WriteLine($"Server -> File name for Create:\t{fileName}\n");

            if (File.Exists(dir + fileName))
            {
                SendData("Server -> File Exist!");
                return;
            }

            using (File.Create(dir + fileName))
                SendData(OkCode);
        }

        private void CreateEvent()
        {
            SendData(OkCode);

            string date = AnswerFromClient();

            SendData(OkCode);

            string msg = AnswerFromClient();

            Console.WriteLine($"Server -> dateTime: {date}\nmsg:\n{msg}\n****\n");

            Result = date + "|" + msg;
        }
        private void EditFileName()
        {
            SendData(OkCode);

            string oldFileName = AnswerFromClient();

            SendData(OkCode);

            string newFileName = AnswerFromClient();

            File.Move(dir+oldFileName, dir+newFileName);

        }
        private void DeleteFile()
        {
            SendData(OkCode);
            string fileName = AnswerFromClient();

            File.Delete(dir+fileName);
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
        None, ForConnect, GetFiles, GetTextFromFile, SetTextFromFile, CreateNewFile, CreateEvent, EditFileName, DeleteFile
    }
}
