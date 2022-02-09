using System.Data.Common;
using System.Diagnostics;
using IntelligenceReporting.Settings;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Renci.SshNet;

namespace IntelligenceReporting.Databases
{
    public class VaultDatabase : IVaultDatabase, IVaultDatabase2, IDisposable
    {
        private const string LocalHostAddress = "127.0.0.1";
        private static uint _nextLocalHostPort = 13306;
        private uint _localHostPort;
        private SshClient? _sshClient;
        private ForwardedPortLocal? _forwardedPortLocal;
        private MySqlConnection? _dbConnection;
        private readonly VaultDbSettings _settings;
        private readonly ILogger _logger;

        public VaultDatabase(VaultDbSettings settings, ILogger<VaultDatabase> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public MySqlConnection? DbConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    string connectionString;
                    if (string.IsNullOrEmpty(_settings.SshServer))
                    {
                        _logger.LogInformation("Connecting directly to MySql database on {DbServer}", _settings.DbServer);
                        connectionString = $"server={_settings.DbServer};port={_settings.DbPort};" +
                            $"database={_settings.DbName};user={_settings.DbUsername};password={_settings.DbPassword};" +
                            "Pooling=False;SslMode=none;Convert Zero Datetime=True;";
                    }
                    else
                    {
                        _logger.LogInformation("Setting up an SSH tunnel to MySql database on {DbServer} on local port {LocalHostPort}", _settings.DbServer, _localHostPort);
                        StartSshTunnel(_settings);

                        _logger.LogInformation($"Connecting through tunnel to MySql database");
                        connectionString = $"server={LocalHostAddress};port={_localHostPort};" +
                            $"database={_settings.DbName};user={_settings.DbUsername};password={_settings.DbPassword};" +
                            "Pooling=False;SslMode=none;Convert Zero Datetime=True;";
                    }

                    _dbConnection = new MySqlConnection(connectionString);
                    _dbConnection.Open();
                }
                return _dbConnection;
            }
        }

        private static readonly object SshTunnelLock = new();
        private void StartSshTunnel(VaultDbSettings settings)
        {
            lock (SshTunnelLock)
            {
                _localHostPort = _nextLocalHostPort++;

                var privateKeyFile = new PrivateKeyFile(settings.SshPrivateKeyFilePath, settings.SshPrivateKeyPassPhrase);
                var authenticationMethod = new PrivateKeyAuthenticationMethod(settings.SshUsername, privateKeyFile);
                var connectionInfo = new ConnectionInfo(settings.SshServer, settings.SshPort, settings.SshUsername, authenticationMethod);
                _sshClient = new SshClient(connectionInfo);
                _sshClient.Connect();
                if (!_sshClient.IsConnected) throw new InvalidOperationException("MySql SSH tunnel connection failed");

                _forwardedPortLocal = new ForwardedPortLocal(LocalHostAddress, _localHostPort, settings.DbServer, settings.DbPort);
                _sshClient.AddForwardedPort(_forwardedPortLocal);

                _forwardedPortLocal.Start();
                if (!_forwardedPortLocal.IsStarted) throw new InvalidOperationException("MySql forwardedPortLocal Connection failed");
            }
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
            _sshClient?.RemoveForwardedPort(_forwardedPortLocal);
            _forwardedPortLocal?.Dispose();
            _sshClient?.Disconnect();
            _sshClient?.Dispose();

            GC.SuppressFinalize(this);
        }

        public async Task<T[]> Query<T>(string sql, Func<DbDataReader, T> readRow)
        {
            var results = new List<T>();
            await Query(sql, row =>
            {
                results.Add(readRow(row));
            });
            return results.ToArray();
        }

        public async Task Query(string sql, Action<DbDataReader> readRow)
        {
            await Query(sql, (reader) =>
            {
                readRow(reader);
                return Task.CompletedTask;
            });
        }

        public async Task Query(string sql, Func<DbDataReader, Task> readRow)
        {
            _logger.LogInformation("Executing \r\n{sql}", sql);
            var sw = Stopwatch.StartNew();
            var rows = 0;
            await using (var command = new MySqlCommand(sql, DbConnection))
            {
                command.CommandTimeout = 600;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    rows++;
                    await readRow(reader);
                }
            }
            _logger.LogInformation(" - Read {rows} rows in {seconds:#,##0.000}.", rows, sw.Elapsed.TotalSeconds);
        }
    }
}
