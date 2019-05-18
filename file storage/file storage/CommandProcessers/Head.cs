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
                FileInfo info = new FileInfo(fullPath);
                response.Headers.Add("Date", info.CreationTime.ToString());
                response.Headers.Add("Name", info.Name);
                response.Headers.Add("readonly", info.IsReadOnly.ToString());
                response.Headers.Add("length", info.Length.ToString());
            }
            catch (FileNotFoundException)
            {
                response.StatusCode = 404;
            }
            catch
            {
                response.StatusCode = 400;
            }
            finally
            {
                response.OutputStream.Close();
            }
        }
    }
}
