using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor.util {

    /// <summary>
    /// Serves as the global layout for data in the application
    /// </summary>
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

        /// <summary>
        /// Loads the given user data into the object
        /// </summary>
        /// <param name="uid">UID of the user</param>
        /// <param name="username">username of the user</param>
        /// <param name="name">name of the user</param>
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

        /// <summary>
        /// Turns userdata into a list format for a DataGrid data context
        /// </summary>
        /// <param name="data">data of the user</param>
        /// <returns>A list of rows regarding user uploads</returns>
        public static List<UploadInfo> UserDataToUploadInfo(UserData data) {
            List<UploadInfo> list = new();

            for(int i = 1; i < data.Uploads.Count+1; i++) {
                UploadInfo uploadrecord = new() {
                    FileIndex = i.ToString(),
                    FileName = data.Uploads[i]["Filename"],
                    FileExt = data.Uploads[i]["Fileext"],
                    ComponentType = data.Uploads[i]["Component"],
                    FileId = data.Uploads[i]["FID"]

                };

                list.Add(uploadrecord);
            }

            return list;
        }

    }

    /// <summary>
    /// Serves as the information needed for the application to function with DB
    /// </summary>
    static class UserConfig {
        public static UserData? UserData { get; set; } = null;
        public static bool UserLoggedin { get; set; } = false;
    }

    /// <summary>
    /// Serves as the data context for the account page
    /// </summary>
    class UploadInfo {
        public string? FileIndex { get; set; }
        public string? FileName { get; set; }
        public string? FileExt { get; set; }
        public string? ComponentType { get; set; }

        public string? FileId { get; set; }
    }

    /// <summary>
    /// Serves as the utility class of functions for user information processing
    /// </summary>
    static class UserAccountUtil {
        private static readonly SHA256 hashtool = SHA256.Create();

        /// <summary>
        /// Hashes a string with SHA256
        /// </summary>
        /// <param name="s">string to be hashed</param>
        /// <returns>hashed version of the string as hexadecimal digits</returns>
        public static string HashString(string s) {
            byte[] passHash = hashtool.ComputeHash(Encoding.UTF8.GetBytes(s));
            return String.Join("", BitConverter.ToString(passHash).Split('-'));
        }

        /// <summary>
        /// Checks with the DB to see if a user exist
        /// </summary>
        /// <param name="username">username to be checked</param>
        /// <returns>true if the user exist</returns>
        public static bool UserExist(string username) {
            string query = "SELECT Username FROM Users WHERE Username=@user;";
            var (result, _) = DBUtil.ExecuteStatement(query,
                                                 false,
                                                 new Tuple<string, object?>("@user", username));
            bool doesUserExist = result != null && result.HasRows;
            result?.Close();
            return doesUserExist;
        }
    }
}
