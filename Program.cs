using System;
using System.Collections.Generic;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Program
    {
        static void Main(string[] args)
        {

            // foreach (KeyValuePair<String, int> kvp in p.ProductDict)
            // {
            //     //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //     Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            // }
            // foreach (KeyValuePair<String, int> kvp in p.UserDict)
            // {
            //     //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //     Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            // }
            // foreach (Rate r in p.RateSet)
            // {
            //     Console.WriteLine(r);
            // }


            double lambda = 100;
            for(int i = 50; i <= 250; i = i + 50) {
                for(int j = 0; j < 15; j++ ) {
                    Parser p = new Parser("amazon-meta.txt","Book", i, 5, 500);
                    MatrixOperations mo = new MatrixOperations(p, 5);
                    mo.test(0.0001,lambda,10);
                    lambda = lambda / 10;
                }

            }
        }
    }
}
