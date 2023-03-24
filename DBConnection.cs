using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace SystemResourceMonitor {
    static class DBConnection {

        public static MySqlConnection? Connection { get; set; }

        public static MySqlConnection? Connect(string? connectionString) {

            if(connectionString == null) {
                return null;
            }

            var conn = new MySqlConnection(connectionString);
            try {
                conn.Open();
            } catch (Exception) {
                return null;
            }
            return conn;    
        }
    }
}
