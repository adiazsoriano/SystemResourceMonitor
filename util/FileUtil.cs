using System;
using System.Collections.Generic;
using System.IO;

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

        public static void HandleDBDownload(string FID) {

        }

        public static void HandleUtilDownload(List<double> xaxis, List<double> yaxis, string filename,string yaxisinfo) {
           
            try {
                using StreamWriter sr = new(filename);
                sr.WriteLine("seconds," + yaxisinfo);
                for (int i = 0; i < xaxis.Count; i++) {
                    sr.WriteLine(xaxis[i] + "," + yaxis[i]);
                }
                sr.Close();
            } catch(Exception) {}
        }
    }
}
