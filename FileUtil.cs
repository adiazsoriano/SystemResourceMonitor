using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemResourceMonitor {
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
    }
}
