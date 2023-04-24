using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor.util {
    class UserData {
        public string Uid { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public Dictionary<int, Dictionary<string, string?>> Uploads { get; set; }

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

        public static List<UploadInfo> UserDataToUploadInfo(UserData data) {
            List<UploadInfo> list = new();

            return list;
        }

    }

    static class UserConfig {
        public static UserData? UserData { get; set; } = null;
        public static bool UserLoggedin { get; set; } = false;
    }

    class UploadInfo {
        public string? FileIndex { get; set; }
        public string? FileName { get; set; }
        public string? FileExt { get; set; }
        public string? ComponentType { get; set; }

        public string? FileId { get; set; }
    }
}
