namespace CERSWebApi.Models
{
    public class AppVersionDetails_Get
    {
        public string PackageName { get; set; }
        public string Platform { get; set; }
        public string VersionNumber { get; set; }
        public string WhatsNew { get; set; }
        public string StoreLink { get; set; }
        public string Mandatory { get; set; }
        public string UpdatedOn { get; set; }

    }
}