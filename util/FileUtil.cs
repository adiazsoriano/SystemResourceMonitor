using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SystemResourceMonitor.util {
    static class FileUtil {

        public static List<string>? GetFileContent(string path) {
            List<string> content = new List<string>();

            try {
                using StreamReader sr = new(path);
                string? line;
                while ((line = sr.ReadLine()) != null) {
                    content.Add(line);
                }
            } catch (Exception) {
                return null;
            }

            return content;
        }

        public static void HandleDBDownload(string? FID, string filename) {
            string query = "SELECT File FROM Uploads WHERE FID = @FID;";
            var (response, _) = DBUtil.ExecuteStatement(query,
                                                        false,
                                                        new Tuple<string,object?>("@FID",FID));

            try {
                if(response != null && response.HasRows) {
                    using StreamWriter sr = new(filename);
                    if(response.Read()) {
                        sr.Write(Encoding.UTF8.GetString((byte[])response["File"]));
                    }
                    sr.Close();
                }
            } catch (Exception) {}
            response?.Close();
        }

        public static void HandleUtilDownload(List<double> xaxis, List<double> yaxis, string filename,string header) {
           
            try {
                using StreamWriter sr = new(filename);
                sr.WriteLine(header);
                for (int i = 0; i < xaxis.Count; i++) {
                    sr.WriteLine(xaxis[i] + "," + yaxis[i]);
                }
                sr.Close();
            } catch(Exception) {}
        }

        public static string GraphDataToCSVString(List<double> xaxis, List<double> yaxis, string header) {
            string info = header + "\n";

            for(int i = 0; i < xaxis.Count; i++) {
                info += xaxis[i] + "," + yaxis[i] + "\n";
            }

            return info;
        }
    }
}
