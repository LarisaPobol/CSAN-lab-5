using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace file_storage
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStorage fileStorage = new FileStorage();
            fileStorage.Initialize();
            Console.ReadKey();
        }
    }
}
