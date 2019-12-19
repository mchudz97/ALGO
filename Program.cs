using System;
using System.Collections.Generic;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Parser p = new Parser("amazon-meta.txt", "Book", 200, 5, 10);
             p.parse();
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
             }*/

            Parser p1 = new Parser("amazon-meta.txt", "Book", 200, 5, 10);
            p1.parse();
            for (int i = 5; i <= 35; i = i + 5)
            {

                MatrixOperations mo = new MatrixOperations(p1, i);
                mo.test(0.001, 0.1, 20, 10);
            }
            Parser p2 = new Parser("amazon-meta.txt", "Book", 400, 5, 100);
            p2.parse();
            for (int i = 5; i <= 35; i = i + 5)
            {

                MatrixOperations mo = new MatrixOperations(p2, i);
                mo.test(0.001, 0.1, 20, 100);
            }

            Parser p3 = new Parser("amazon-meta.txt", "Book", 1400, 5, 1000);
            p3.parse();
            for (int i = 5; i <= 35; i = i + 5)
            {

                MatrixOperations mo = new MatrixOperations(p3, i);
                mo.test(0.001, 0.1, 20, 1000);
            }

        }
    }
}
