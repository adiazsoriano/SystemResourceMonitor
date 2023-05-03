using MySqlConnector;
using System;
using System.Diagnostics;

namespace SystemResourceMonitor.util {

    /// <summary>
    /// Provides database utility functions
    /// </summary>
    static class DBUtil {

        public static MySqlConnection? Connection { get; set; }


        /// <summary>
        /// Attempts to establish a connection to a database, is nullable
        /// </summary>
        /// <param name="connectionString">Credentials for database access</param>
        /// <returns>a valid connection</returns>
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

        /// <summary>
        /// Executes a SQL statement
        /// </summary>
        /// <param name="statement">query or non query</param>
        /// <param name="isNonQuery">non query flag</param>
        /// <param name="args">tuple varargs for a string and object</param>
        /// <returns>A tuple of a data reader or int of affected rows</returns>
        public static (MySqlDataReader? result, int? affectedrows) ExecuteStatement(string statement, 
                                                                                    bool isNonQuery, 
                                                                                    params Tuple<string, object?>[] args) {
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
            } catch (Exception ex) {
                return (null, null);
            }
        }
    }
}
