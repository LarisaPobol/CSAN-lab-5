using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace file_storage
{
    interface ICommand
    {
        void CreateRequest(string directory, HttpListenerRequest request, HttpListenerResponse response);
    }
}
