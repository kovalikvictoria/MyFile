using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFile
{
    public class TextReader
    {
        private Connection conn;
        private StreamReader streamReader;

        private bool disposed = false;

        public string[] Columns { get; }

        private string[] thisLine;

        public TextReader(Connection conn, string table_name)
        {
            this.conn = conn;
            streamReader = new StreamReader(conn.ConnectionString);

            string line = "";
            while (line != string.Format("<{0}>", table_name))
            {
                line = streamReader.ReadLine();
            }
            if (streamReader.EndOfStream) throw new Exception(string.Format("Cannot find table \"{0}\"",
                table_name));
            else
            {
                Columns = streamReader.ReadLine().Split(conn.separator[0]);
            }
        }

        public bool Read()
        {
            thisLine = streamReader.ReadLine().Split(Connection.separator[0]);
            if (thisLine[0] == "<end>") return false;
            else return true;
        }

        public string this[string column]
        {
            get
            {
                int i = 0;
                foreach (string Column in Columns)
                {
                    if (Column.Equals(column)) return thisLine[i];
                    i++;
                }
                throw new Exception(string.Format("Current object haven't column \"{0}\"",
                    column));
            }
        }
    }
}
