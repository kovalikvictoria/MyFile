using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyFile
{
    public class Table
    {
        private Connection conn;
        public string TableName { get; }
        public string[] Columns { get; }
        public List<string[]> Rows { get; }
        private bool Changed = false;

        public Table(Connection conn, string table_name)
        {
            TableName = table_name;
            conn = this.conn;
            Rows = new List<string[]>();

            using (TextReader reader = new TextReader(this.conn, table_name))
            {
                Columns = reader.Columns;

                string[] row;
                int i = 0;

                while (reader.Read())
                {
                    row = new string[Columns.Length];

                    foreach (string column in Columns)
                    {
                        row[i] = reader[column];
                        i++;
                    }


                    Rows.Add(row);
                    i = 0;
                }
            }
        }
    }
}
