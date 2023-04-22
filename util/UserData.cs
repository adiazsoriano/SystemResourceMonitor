using MySqlConnector;
using System.Collections.Generic;

namespace SystemResourceMonitor.util {
    class UserData {
        private int Uid { get; set; }
        private string UserName { get; set; }
        private string Name { get; set; }
        private bool IsAdmin { get; set; }

        private Dictionary<string, Dictionary<string, string>> Uploads { get; set; }

        public UserData() {
            this.Uid = -1;
            this.UserName = string.Empty;
            this.Name = string.Empty;
            this.IsAdmin = false;
            this.Uploads = new();
        }

        public void LoadData(MySqlDataReader response) {

        }

    }

    static class UserConfig {
        public static UserData? UserData { get; set; } = null;
        public static bool UserLoggedin { get; set; } = false;
    }
}
