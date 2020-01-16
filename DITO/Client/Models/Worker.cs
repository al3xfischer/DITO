using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Worker<TInParameter, TResult>
    {
        public event EventHandler ResultReady;

        private readonly Func<TInParameter, Host, Task<TResult>> workingFunction;
        private readonly Host host;

        public Worker(Func<TInParameter, Host, Task<TResult>> workingFunction, Host host)
        {
            this.workingFunction = workingFunction ?? throw new ArgumentNullException(nameof(workingFunction));
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public bool Running { get; private set; }

        public async void Execute(TInParameter parameter)
        {
            this.Running = true;
            this.Result = await this.workingFunction(parameter, this.host);
            this.Running = false;
            this.ResultReady.Invoke(this, EventArgs.Empty);
        }

        public TResult Result { get; private set; }


        public static IEnumerable<Worker<TInParameter, TResult>> GetWorkers(IEnumerable<Host> hosts, Func<TInParameter, Host, Task<TResult>> workingFunction)
        {
            List<Worker<TInParameter, TResult>> result = new List<Worker<TInParameter, TResult>>();

            foreach (var host in hosts)
            {
                result.Add(new Worker<TInParameter, TResult>(workingFunction, host));
            }

            return result;
        }
    }
}
