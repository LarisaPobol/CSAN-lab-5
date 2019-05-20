using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace file_storage.CommandProcessers
{
    class Get : ICommand
    {
        public void CreateRequest(string directory, HttpListenerRequest request, HttpListenerResponse response)
        {
            Stream outputStream = response.OutputStream;
            StreamWriter streamWriter = new StreamWriter(outputStream);
            string fullPath = directory + request.RawUrl;
            string[] directories;
            string[] files;
            List<string> jsonEl = new List<string>();
            try
            {
                if (!File.Exists(fullPath))
                {
                    directories = Directory.GetDirectories(fullPath);
                    AddToJsonList(directory, directories, jsonEl);
                    files = Directory.GetFiles(fullPath);                       
                    AddToJsonList(directory, files, jsonEl);
                    string json = JsonConvert.SerializeObject(jsonEl);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                else
                {
                    try
                    {
                        using (var file = File.Open(fullPath, FileMode.Open))
                        {
                            file.CopyTo(outputStream);
                            file.Close();
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        response.StatusCode = 404;
                    }
                    catch
                    {
                        response.StatusCode = 400;
                    }
                }
            }
            catch
            {
                response.StatusCode = 404;
            }
            finally
            {
                outputStream.Close();
                streamWriter.Close();
            }
        }

        private void AddToJsonList(string directory, string[] items, List<string> list)
        {
            foreach (string item in items)
            {
                string temp = item.Substring(directory.Length);
                list.Add(temp);
            }
        }
        private void WriteJsonToStream(string directory, StreamWriter streamWriter, IEnumerable<string> list)
        {
            //foreach (string entry in list)
            //{              
            //    string temp = entry.Substring(directory.Length);
            //    string json = JsonConvert.SerializeObject(temp);               
            //    streamWriter.Write(json);                       
            //}
            string json = JsonConvert.SerializeObject(list);
            streamWriter.Write(json);

        }

    }
}
