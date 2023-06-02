using System.Collections.Concurrent;

namespace APICatalogo.Logging
{
    // Classe Responsável pela instância(criar) do Logger - Armazenando em dictionary
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration loggerConfig;

        readonly ConcurrentDictionary<string, CustomerLogger> loggers =
                 new ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }
       
        // Cria o Logger customizado passando nome e categoria.
        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    
    }

}
