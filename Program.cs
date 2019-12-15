using System;
using System.Collections.Generic;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser("amazon-meta.txt","Book", 100, 5);
            foreach (KeyValuePair<String, int> kvp in p.ProductDict)
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
            }

            // MatrixOperations mo = new MatrixOperations(p, 10);
            // mo.ALS(0.0001);
        }
    }
}
