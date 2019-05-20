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
                string copyHeader = ConfigurationManager.AppSettings["Copy"];
                var copyPath = request.Headers[copyHeader];
                if (copyPath != null)
                {
                    string tempPath = directory + "/" + copyPath.TrimStart('/');
                    if (File.Exists(tempPath))
                    {
                        
                        File.Copy(tempPath, fullPath);
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

                    using (var newFile = new FileStream(fullPath, FileMode.OpenOrCreate))
                    {
                        request.InputStream.CopyTo(newFile);
                        
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
