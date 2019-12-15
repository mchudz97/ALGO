using System;
using System.Collections.Generic;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser("amazon-meta.txt", "Book", 1500, 5, 10);
            p.parse();
            /*foreach (KeyValuePair<String, int> kvp in p.ProductDict)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<String, int> kvp in p.UserDict)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            foreach (Rate r in p.RateSet)
            {
                Console.WriteLine(r);
            }*/
            for(int i=5; i <= 30; i = i + 5)
            {

             MatrixOperations mo = new MatrixOperations(p, i);
             mo.test(0.001, 0.1, 10);
            }
        }
    }
}
