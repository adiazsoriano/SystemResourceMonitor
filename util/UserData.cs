using MySqlConnector;
using System;
using System.Collections.Generic;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor.util {
    class UserData {
        private string Uid { get; set; }
        private string UserName { get; set; }
        private string Name { get; set; }
        private bool IsAdmin { get; set; }

        private Dictionary<int, Dictionary<string, string?>> Uploads { get; set; }

        public UserData() {
            this.Uid = string.Empty;
            this.UserName = string.Empty;
            this.Name = string.Empty;
            this.IsAdmin = false;
            this.Uploads = new();
        }

        public void LoadData(string uid, string username, string name) {
            string uploadquery = "SELECT FID, Component, Filename, Fileext FROM Uploads WHERE UID=@uid";
            var (fileinfo, _) = DBUtil.ExecuteStatement(uploadquery,
                                                        false,
                                                        new Tuple<string, object?>("@uid",uid));

            this.Uid = uid;
            this.UserName = username;
            this.Name = name;

            if(fileinfo != null) {
                int i = 0;
                while (fileinfo.Read()) {
                    Dictionary<string, string?> rowcontent = new() {
                        { "FID", fileinfo["FID"].ToString() },
                        { "Filename", fileinfo["Filename"].ToString() },
                        { "Fileext", fileinfo["Fileext"].ToString() },
                        { "Component", fileinfo["Component"].ToString() } 
                    };

                    i++;
                    Uploads.Add(i, rowcontent);
                }
            }

            fileinfo?.Close();
        }

    }

    static class UserConfig {
        public static UserData? UserData { get; set; } = null;
        public static bool UserLoggedin { get; set; } = false;
    }
}
