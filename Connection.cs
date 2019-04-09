using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MyFile
{
    public class Connection
    {
        public readonly string separator = "|";

        public string ConnectionString { get; } //path 

        public Connection(string connString, string separator = "|")
        {
            ConnectionString = connString;
        }

        public TextDReader GetDataReader(string TableName)
        {
            return new TextDReader(this, TableName);
        }

        public Table GetTable(string tableName)
        {
            return new Table(this, tableName);
        }

        
        public void CreateTable(string tableName, string[] columns)
        {
            //need checking if table exists
            using (StreamWriter streamWriter = new StreamWriter(ConnectionString, true))
            {
                streamWriter.WriteLine(string.Format("<{0}>", tableName));
                streamWriter.WriteLine(string.Join(separator, columns));
                streamWriter.WriteLine("<end>");
            }
        }

        public void DeleteTable(string tableName)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(ConnectionString))
            {
                string line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (line != string.Format("<{0}>", tableName))
                    {
                        lines.Add(line);
                    }
                    else
                    {
                        while (reader.ReadLine() != "<end>") ;
                    }
                }
            }
            using (StreamWriter writer = new StreamWriter(ConnectionString))
            {
                foreach (string _line in lines)
                {
                    writer.WriteLine(_line);
                }
            }
        }
    }
}
