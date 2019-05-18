using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace file_storage.CommandProcessers
{
    class UnrecognizedCommand : ICommand
    {
        public void CreateRequest(string directory, HttpListenerRequest request, HttpListenerResponse response)
        {
            response.StatusCode = 501;
            response.OutputStream.Close();
        }
    }
}
