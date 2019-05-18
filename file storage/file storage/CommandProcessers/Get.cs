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
            try
            {
                if (!File.Exists(fullPath))
                {
                    directories = Directory.GetDirectories(fullPath);
                    WriteJsonToStream(streamWriter, directories);
                    files = Directory.GetFiles(fullPath);
                    WriteJsonToStream(streamWriter, files);
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

        private void WriteJsonToStream(StreamWriter streamWriter, IEnumerable<string> list)
        {
            foreach (string entry in list)
            {
                string json = JsonConvert.SerializeObject(entry);
                streamWriter.Write(json);
            }
        }

    }
}
