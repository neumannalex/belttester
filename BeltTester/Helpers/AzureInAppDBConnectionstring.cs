using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Helpers
{
    public class AzureInAppDBConnectionstring
    {
        public string ServerWithPort { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ToPomeloConnectionString()
        {
            return $"Server={ServerWithPort};Database={Database};User={Username};Password={Password};";
        }

        public string ToMySqlConnectionString()
        {
            return $"server={Server};port={Port};user={Username};password={Password};database={Database}";
        }

        public static AzureInAppDBConnectionstring FromString(string conn)
        {
            AzureInAppDBConnectionstring azure = new AzureInAppDBConnectionstring();

            List<string> settings = conn.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach(var setting in settings)
            {
                if (setting.Contains("="))
                {
                    string[] pair = setting.Split("=");

                    string key = pair[0].ToLowerInvariant();
                    string value = pair.Length > 1 ? pair[1] : string.Empty;

                    switch(key)
                    {
                        case "data source":
                            azure.ServerWithPort = value;
                            if(value.Contains(":"))
                            {
                                string[] server = value.Split(":");
                                azure.Server = server[0];
                                azure.Port = server[1];
                            }
                            break;
                        case "database":
                            azure.Database = value;
                            break;
                        case "user id":
                            azure.Username = value;
                            break;
                        case "password":
                            azure.Password = value;
                            break;
                    }
                }
            }

            return azure;
        }
        public static AzureInAppDBConnectionstring FromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"File {filename} not found");

            var content = File.ReadAllText(filename);
            return AzureInAppDBConnectionstring.FromString(content);
        }
    }
}
