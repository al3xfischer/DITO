using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IClientServerService
    {
        public void Start();
        public Task Stop();
    }
}