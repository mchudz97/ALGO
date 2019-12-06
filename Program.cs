using System;
using System.Collections.Generic;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser("amazon-meta.txt", 30);
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
            foreach(Rate r in p.RateList)
            {
                Console.WriteLine(r);
            }*/

            double[,] U = Randomizer.Randomize(5,5);

            double[,] P = Randomizer.Randomize(5,5);

            MatrixOperations mo = new MatrixOperations();
            
            mo.printMatrix(P);

            mo.RateList = p.RateList;
            mo.UserDict = p.UserDict;
            mo.ProductDict = p.ProductDict;

            mo.generateAu(1, P);

            
        }
    }
}