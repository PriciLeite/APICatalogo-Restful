namespace APICatalogo.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning; // Destaca evento anormal mas não interrope.
        public int EventId { get; set; } = 0;    
    }
}
