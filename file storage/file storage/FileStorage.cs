using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Configuration;
using file_storage.CommandProcessers;
namespace file_storage
{
    class FileStorage
    {
        public string StorageFolder;
        public string Url;

        private MethodsParser CommandParser;

        private HttpListener Listener;
        public FileStorage()
        {
            StorageFolder = ConfigurationManager.AppSettings["StorageDirectory"];
            Url = ConfigurationManager.AppSettings["Url"];
            CommandParser = new MethodsParser();
        }

        public void Initialize()
        {
            Listener = new HttpListener();
            try
            {
                Listener.Prefixes.Add(Url);
                Listener.Start();
                while (true)
                {
                    HttpListenerContext context = Listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    string method = request.HttpMethod;
                    Console.WriteLine(method);
                    ICommand requestCommand = CommandParser.GetCommand(method);
                    HttpListenerResponse response = context.Response;

                    // requestCommand.CreateRequest(StorageFolder, request, response);

                    Thread ClientThread = new Thread(() => { requestCommand.CreateRequest(StorageFolder, request, response); });
                    ClientThread.IsBackground = true;
                    ClientThread.Start();
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
            finally
            {
                Listener.Close();
            }
        }

    }
}
