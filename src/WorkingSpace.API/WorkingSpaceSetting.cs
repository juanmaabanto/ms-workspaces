namespace Sofisoft.Accounts.WorkingSpace.API
{
    public class WorkingSpaceSetting
    {
        public ServicesSetting Services { get; set; }
    }

    public class ServicesSetting
    {
        public string LoggingUrl { get; set; }
    }
}