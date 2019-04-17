using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFile
{
    class Program
    {
        static void Main(string[] args)
        {
              TextConnection connection = new TextConnection(
                @"C:\Users\Vika\source\repos\Task4\Task4\TextBase.txt");//path to file


            string[] newcolumns = new string[] { "discount_id", "name", "coef" };
            connection.CreateTable("Discount", newcolumns);

            Table table = connection.GetTable("Discount");
            table.Create("7001", "-10%", "0.9");
            table.Create("7002", "-30%", "0.7");
            table.Create("7003", "-60%", "0.4");
            table.Create("7004", "-50%", "0.5");

            //table.Delete("name", "-30%");
            Console.WriteLine(string.Join(" ", table.Read("discount_id", "7003")[0]));

            table.ExecuteChanges();//used to save changes to textbase

            Console.WriteLine("done");
            Console.Read();
        }
    }
}
