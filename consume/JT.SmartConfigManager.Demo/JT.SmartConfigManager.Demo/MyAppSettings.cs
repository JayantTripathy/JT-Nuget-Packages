namespace JT.SmartConfigManager.Demo
{
    public class MyAppSettings
    {
        public AppSettings? AppSettings { get; set; }
        public string? AppSecret { get; set; }
        public string? DbKey1 { get; set; }
    }
    public class AppSettings
    {
        public string? AppName { get; set; }
        public string? Environment { get; set; }
    }
    public class SqlConfigSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
    }

}
