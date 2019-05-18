using System.Net;
using System.IO;
using System.Configuration;
namespace file_storage.CommandProcessers
{
    class Put : ICommand
    {
        public void CreateRequest(string directory, HttpListenerRequest request, HttpListenerResponse response)
        {
            string fullPath = directory + request.RawUrl;
            try
            {
                const int BUF = 65535;
                string copyHeader = ConfigurationManager.AppSettings["Copy"];
                var copyPath = request.Headers[copyHeader];
                if (copyPath != null)
                {
                    if (File.Exists(fullPath))
                    {
                        string tempPath = directory + "/" + copyPath.TrimStart('/');
                        File.Copy(fullPath, tempPath);
                    }                                         
                    else
                    {
                        throw new DirectoryNotFoundException();
                    }                        
                }
                else
                {
                    var dirname = Path.GetDirectoryName(fullPath);

                    if (!Directory.Exists(dirname))
                    {
                        Directory.CreateDirectory(dirname);
                    }

                    using (var newFile = new FileStream(fullPath, FileMode.Create))
                    {
                        request.InputStream.CopyTo(newFile, BUF);
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
