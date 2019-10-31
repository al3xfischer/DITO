using System.Net.Http;
using System.Windows;
using Grpc.Net.Client;
using Server;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Reply { get; set; }

        public MainWindow()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000/");
            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHello(new HelloRequest { Name = "Test" });
            this.Reply = reply.Message;

            InitializeComponent();
            this.DataContext = this;
        }
    }
}
