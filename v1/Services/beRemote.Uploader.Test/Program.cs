using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace beRemote.Uploader.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            beRemote.Services.FTClient.Communicator com = new Services.FTClient.Communicator(Guid.NewGuid().ToString(), "test_build");
            com.Upload("F:\\_UserStore\\Downloads\\upl.jpg");
        }


    }
}
