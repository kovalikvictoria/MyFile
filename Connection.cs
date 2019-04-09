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

        public TextReader GetDataReader(string TableName)
        {
            return new TextReader(this, TableName);
        }

        public Table GetTable(string tableName)
        {
            return new Table(this, tableName);
        }
    }
}
