using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SystemResourceMonitor.util {

    /// <summary>
    /// Provides a utility for Files / IO
    /// </summary>
    static class FileUtil {


        /// <summary>
        /// Returns a list of strings for file content given a valid filepath
        /// </summary>
        /// <param name="path">A path to a file</param>
        /// <returns>List of strings as file content</returns>
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

        /// <summary>
        /// Handles downloading from a DB
        /// </summary>
        /// <param name="FID">File ID of a file in the DB</param>
        /// <param name="filename">filename of the file</param>
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

        /// <summary>
        /// Handles utility download
        /// </summary>
        /// <param name="xaxis">X Axis information</param>
        /// <param name="yaxis">Y Axis information</param>
        /// <param name="filename">filename of the file</param>
        /// <param name="header">header to file (first line of file)</param>
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


        /// <summary>
        /// Turns graph data into a csv string
        /// </summary>
        /// <param name="xaxis">X Axis information</param>
        /// <param name="yaxis">Y Axis information</param>
        /// <param name="header">header to file (first line of file)</param>
        /// <returns>CSV encoded string</returns>
        public static string GraphDataToCSVString(List<double> xaxis, List<double> yaxis, string header) {
            string info = header + "\n";

            for(int i = 0; i < xaxis.Count; i++) {
                info += xaxis[i] + "," + yaxis[i] + "\n";
            }

            return info;
        }
    }
}
