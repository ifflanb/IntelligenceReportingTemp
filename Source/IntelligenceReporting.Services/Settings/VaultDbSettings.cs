namespace IntelligenceReporting.Settings
{
    public class VaultDbSettings
    {
        public string SshServer { get; set; } = "";
        public int SshPort { get; set; }
        public string SshUsername { get; set; } = "";
        public string SshPrivateKeyFilePath { get; set; } = "";
        public string SshPrivateKeyPassPhrase { get; set; } = "";

        public string DbServer { get; set; } = "";
        public uint DbPort { get; set; }
        public string DbName { get; set; } = "";
        public string DbUsername { get; set; } = "";
        public string DbPassword { get; set; } = "";
    }
}
