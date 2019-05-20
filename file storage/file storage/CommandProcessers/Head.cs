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
    class Head : ICommand
    {
        public void CreateRequest(string directory, HttpListenerRequest request, HttpListenerResponse response)
        {
            Stream outputStream = response.OutputStream;
            string fullPath = directory + request.RawUrl;
            try
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                response.Headers.Add("length", fileInfo.Length.ToString());
                response.Headers.Add("Name", fileInfo.Name);
                response.Headers.Add("Date", fileInfo.CreationTime.ToString());
                response.Headers.Add("LastAcsessTime", fileInfo.LastAccessTime.ToString());
                
                response.Headers.Add("readonly", fileInfo.IsReadOnly.ToString());
                response.Headers.Add("LastWriteTime", fileInfo.LastWriteTime.ToString());
                response.Headers.Add("Extension", fileInfo.Extension.ToString());
                response.Headers.Add("LastAccessTimeUtc", fileInfo.LastAccessTimeUtc.ToString());
                response.Headers.Add("LastWriteTimeUtc", fileInfo.LastWriteTimeUtc.ToString());

            }
            catch (FileNotFoundException)
            {
                response.StatusCode = 404;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.StatusCode = 400;
            }
            finally
            {
                response.OutputStream.Close();
            }
        }
    }
}
