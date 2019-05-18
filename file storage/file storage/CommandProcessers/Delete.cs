using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace file_storage.CommandProcessers
{
    class Delete : ICommand
    {
        public void CreateRequest(string directory, HttpListenerRequest request,  HttpListenerResponse response)
        {
            try
            {
                string fullPath = directory + request.RawUrl;
                if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath);
                }
                else
                {
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                       
                }                
            }
            catch (FileNotFoundException)
            {
                response.StatusCode = 404;
            }
            catch (DirectoryNotFoundException)
            {
                response.StatusCode = 404;
            }
            catch (Exception)
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
