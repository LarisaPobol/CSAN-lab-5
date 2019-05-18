using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using file_storage.CommandProcessers;

namespace file_storage
{
    class MethodsParser
    {
        public MethodsParser()
        {
        }

        public ICommand GetCommand(string commandName)
        {
            switch (commandName)
            {
                case "GET":
                    {
                        return new Get();
                    }

                case "PUT":
                    {
                        return new Put();
                    }
                case "HEAD":
                    {
                        return new Head();
                    }
                case "DELETE":
                    {
                        return new Delete();
                    }
                default:
                    {
                        return new UnrecognizedCommand();
                    }
            }
        }
    }
}
