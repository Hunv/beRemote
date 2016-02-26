using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace beRemote.Core.InterComm
{
    public class CommServer
    {
        private static CommServer _instance;
        public static CommServer Instance
        {
            get 
            { 
                if (_instance == null)
                    _instance = new CommServer(Kernel.KernelInstanceGuid);
                
                return _instance; 
            }
        }

        public Guid KernelInstanceGuid { get; private set; }
        public Boolean Running { get; private set; }

        private ServiceHost host;
        private Uri uri = new Uri("net.pipe://localhost");
        private Guid guid;
        
        public InterCommEvents Events = new InterCommEvents();

        public CommServer(Guid kernelInstnaceGuid)
        {
            host = new ServiceHost(typeof(CommService), uri);
            host.AddServiceEndpoint(typeof(ICommService), new NetNamedPipeBinding(), "beRemoteInterComm");

            

            KernelInstanceGuid = kernelInstnaceGuid;
        }

    

        public void Start()
        {
            Running = true;
            host.Faulted += host_Faulted;
            host.UnknownMessageReceived += host_UnknownMessageReceived;

            ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

            // if not found - add behavior with setting turned on 
            if (debug == null)
            {
                host.Description.Behaviors.Add(
                     new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                // make sure setting is turned ON
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }

            host.Open();
            
        }

        void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            
        }

        void host_Faulted(object sender, EventArgs e)
        {
            
        }

        public void Stop()
        {
            Running = false;
            host.Close();
        }
    }
}
//using Microsoft.Win32.SafeHandles;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;

//namespace beRemote.Core.AppInterComm
//{
//    public class Server
//    {
//        [DllImport("kernel32.dll", SetLastError = true)]
//        public static extern SafeFileHandle CreateNamedPipe(
//           String pipeName,
//           uint dwOpenMode,
//           uint dwPipeMode,
//           uint nMaxInstances,
//           uint nOutBufferSize,
//           uint nInBufferSize,
//           uint nDefaultTimeOut,
//           IntPtr lpSecurityAttributes);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        public static extern int ConnectNamedPipe(
//           SafeFileHandle hNamedPipe,
//           IntPtr lpOverlapped);

//        public const uint DUPLEX = (0x00000003);
//        public const uint FILE_FLAG_OVERLAPPED = (0x40000000);

//        public class Client
//        {
//            public SafeFileHandle handle;
//            public FileStream stream;
//        }

//        public delegate void MessageReceivedHandler(Client client, string message);

//        public event MessageReceivedHandler MessageReceived;
//        public const int BUFFER_SIZE = 4096;

//        string pipeName = @"\\.\pipe\beRemoteLocalpipe";
//        Thread listenThread;
//        bool running;
//        List<Client> clients;

//        public string PipeName
//        {
//            get { return this.pipeName; }
//            set { this.pipeName = value; }
//        }

//        public bool Running
//        {
//            get { return this.running; }
//        }
//        public Guid KernelInstanceGuid { get; private set; }
//        public Server(Guid kernelInstanceGuid)
//        {
//            KernelInstanceGuid = kernelInstanceGuid;
//            this.clients = new List<Client>();
//        }

//        /// <summary>
//        /// Starts the pipe server
//        /// </summary>
//        public void Start()
//        {
//            //start the listening thread
//            this.listenThread = new Thread(new ThreadStart(ListenForClients));
//            used_threads.Add(listenThread);
//            this.listenThread.Start();

//            this.running = true;
//        }

//        public void Stop()
//        {
//            abort = true;
//        }

//        private List<Thread> used_threads = new List<Thread>();
//        private bool abort = false;
//        /// <summary>
//        /// Listens for client connections
//        /// </summary>
//        private void ListenForClients()
//        {
//            while (true && abort == false)
//            {
//                SafeFileHandle clientHandle =
//                CreateNamedPipe(
//                     this.pipeName,
//                     DUPLEX | FILE_FLAG_OVERLAPPED,
//                     0,
//                     255,
//                     BUFFER_SIZE,
//                     BUFFER_SIZE,
//                     0,
//                     IntPtr.Zero);

//                //could not create named pipe
//                if (clientHandle.IsInvalid)
//                    return;

//                int success = ConnectNamedPipe(clientHandle, IntPtr.Zero);

//                //could not connect client
//                if (success == 0)
//                    return;

//                Client client = new Client();
//                client.handle = clientHandle;

//                lock (clients)
//                    this.clients.Add(client);

//                Thread readThread = new Thread(new ParameterizedThreadStart(Read));
//                used_threads.Add(readThread);
//                readThread.Start(client);
//            }
//        }

//        /// <summary>
//        /// Reads incoming data from connected clients
//        /// </summary>
//        /// <param name="clientObj"></param>
//        private void Read(object clientObj)
//        {
//            Client client = (Client)clientObj;
//            client.stream = new FileStream(client.handle, FileAccess.ReadWrite, BUFFER_SIZE, true);
//            byte[] buffer = new byte[BUFFER_SIZE];
//            ASCIIEncoding encoder = new ASCIIEncoding();

//            while (true && abort == false)
//            {
//                int bytesRead = 0;

//                try
//                {
//                    bytesRead = client.stream.Read(buffer, 0, BUFFER_SIZE);
//                }
//                catch
//                {
//                    //read error has occurred
//                    break;
//                }

//                //client has disconnected
//                if (bytesRead == 0)
//                    break;

//                String msg = encoder.GetString(buffer, 0, bytesRead);

//                if (msg.ToLower().Equals(KernelInstanceGuid.ToString() + "internal::shutdown"))
//                {
//                    abort = true;
//                    break;
//                }
//                //fire message received event
//                if (this.MessageReceived != null)
//                    this.MessageReceived(client, msg);
//            }

//            //clean up resources
//            client.stream.Close();
//            client.handle.Close();
//            lock (this.clients)
//                this.clients.Remove(client);

//            foreach (Thread th in used_threads)
//            {
//                try
//                {
//                    th.Abort();
//                }
//                catch { }
//            }
//        }

//        /// <summary>
//        /// Sends a message to all connected clients
//        /// </summary>
//        /// <param name="message">the message to send</param>
//        public void SendMessage(string message)
//        {
//            lock (this.clients)
//            {
//                ASCIIEncoding encoder = new ASCIIEncoding();
//                byte[] messageBuffer = encoder.GetBytes(message);
//                foreach (Client client in this.clients)
//                {
//                    client.stream.Write(messageBuffer, 0, messageBuffer.Length);
//                    client.stream.Flush();
//                }
//            }
//        }

       
//    }

//}
