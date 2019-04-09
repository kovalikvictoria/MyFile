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

        //returns all rows from table in special format
        public string GetContent()
        {
            string result = "";
            foreach (string[] row in Rows)
            {
                result += string.Join(сonn.separator, row) + "\n";
            }
            return result;
        }

        //INSERT INTO TableName VALUES (newObject)
        public void Create(params string[] newObject)
        {
            Changed = true;

            int len = Columns.Length;
            string[] row = new string[len];
            int i = 0;

            foreach (string item in newObject)
            {
                row[i] = item;
                i++;
                if (i == len) break;
            }
            while (i < len)
            {
                row[i] = "null";
                i++;
            }
            Rows.Add(row);
        }
        /// ///// //////// //////////////////////////////////////////////////////////
        public List<string[]> Read(string column, string condition)
        {
            List<string[]> result = new List<string[]>();
            if (!Columns.Contains(column)) throw new Exception("Column not found");

            int columnIndex = 0;
            while (Columns[columnIndex] != column) columnIndex++;

            foreach (string[] row in Rows)
            {
                if (row[columnIndex] == condition) result.Add(row);
            }

            return result;
        }

        //something like UPDATE in SQL
        public void Update(string column, string condition, params string[] newObject)
        {
            Changed = true;

            Delete(column, condition);
            Create(newObject);
        }

        //in SQL: DELETE FROM TableNane WHERE column = condition
        public void Delete(string column, string condition)
        {
            Changed = true;

            if (!Columns.Contains(column)) throw new Exception("Column not found");
            int columnIndex = 0;
            while (Columns[columnIndex] != column) columnIndex++;

            for (int i = 0; i < Rows.Count;)
            {
                if (Rows[i][columnIndex] == condition) Rows.Remove(Rows[i]);
                else i++;
            }
        }

        public override string ToString()
        {
            string result = "";
            result += string.Format("<{0}>\n", TableName);
            result += string.Join(conn.separator, Columns) + "\n";
            result += GetContent();
            result += "<end>";
            return result;
        }

        public void ExecuteChanges()
        {
            if (this.Changed)
            {
                Connection.DeleteTable(TableName);
                using (StreamWriter writer = new StreamWriter(conn.ConnectionString, true))
                {
                    
                    writer.WriteLine(string.Format("<{0}>\n", TableName));

                    writer.WriteLine(string.Join(conn.separator, Columns));

                    foreach (string[] row in Rows)
                    {
                        writer.WriteLine(string.Join(conn.separator, row));
                    }

                    writer.WriteLine("<end>");
                }
            }
        }

    }
}
