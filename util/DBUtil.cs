using MySqlConnector;
using System;

namespace SystemResourceMonitor.util {
    static class DBUtil {

        public static MySqlConnection? Connection { get; set; }

        public static MySqlConnection? Connect(string? connectionString) {

            if (connectionString == null) {
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

        public static (MySqlDataReader? result, int? affectedrows) ExecuteStatement(string statement, bool isNonQuery, params Tuple<string, object?>[] args) {
            try {
                using MySqlCommand command = new(statement, Connection);

                foreach (Tuple<string, object?> item in args) {
                    command.Parameters.AddWithValue(item.Item1, item.Item2);
                }
                command.Prepare();

                if (isNonQuery) {
                    return (null, command.ExecuteNonQuery());
                } else {
                    return (command.ExecuteReader(), null);
                }
            } catch (Exception) {
                return (null, null);
            }
        }
    }
}
