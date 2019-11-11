using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>
        /// The name of the server.
        /// </value>
        public string ServerName { get; set; }


        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        /// <value>
        /// The server port.
        /// </value>
        public int ServerPort { get; set; }


        /// <summary>
        /// Gets or sets the maximum size of the batch.
        /// </summary>
        /// <value>
        /// The maximum size of the batch.
        /// </value>
        public int MaxBatchSize { get; set; }


        /// <summary>
        /// Gets or sets the local server port.
        /// </summary>
        /// <value>
        /// The local server port.
        /// </value>
        public int LocalServerPort { get; set; }


        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public DitoConfiguration Configuration { get; }

        /// <summary>
        /// Persists the configuration.
        /// </summary>
        public void Save();
    }
}
