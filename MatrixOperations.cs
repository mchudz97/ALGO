using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class MatrixOperations
    {
        private Dictionary<String, int> userDict;
        private Dictionary<String, int> productDict;
        private List<Rate> rateList;

        public Dictionary<string, int> UserDict { get => userDict; set => userDict = value; }
        public Dictionary<string, int> ProductDict { get => productDict; set => productDict = value; }
        public List<Rate> RateList { get => rateList; set => rateList = value; }

        public double[] generateVu(double[][] products, int indexUser )
        {
         
            double[] vu= new double[products[0].Length];
            foreach(Rate r in rateList)
            {
                if (r.User == indexUser)
                {
                    for (int i = 0; i < products[r.Product].Length; i++)
                    {
                        vu[i] = vu[i] + products[r.Product][i] * r.Value;

                    }
                }
            }
            return vu;
        }
        public double[] generateWp(double[][] users, int indexProduct)
        {
            
            double[] wp = new double[users[0].Length];
            foreach(Rate r in rateList)
            {
                if (r.Product == indexProduct)
                {
                    for(int i = 0; i < users[r.User].Length; i++)
                    {
                        wp[i] = wp[i] + users[r.User][i] * r.Value;
                    }
                }
            }
            return wp;
        }

        public double[,] generateAu(int user, double[,] productsMatrix) {
            //list that contains product index in productsMatrix
            List<int> productIndexList = new List<int>();

            //for every rate check if user id(int) == rate user id(int)
            // if YES add productIndex to list ONLY if NOT added already
            for(int i = 0 ; i < rateList.Count; i++) {
                Rate rate  = rateList[i];
                if(rate.User == user) {
                    int productIndex = rate.Product;
                    if(!productIndexList.Contains(productIndex))
                        productIndexList.Add(productIndex);
                }
            }
            
            //sort by index ascending
            productIndexList.Sort();

            //reduces productMatrix into subMatrix piu that contains the products that the user rated 
            double[,] piu = new double[productsMatrix.GetLength(0),productIndexList.Count];
            for(int i = 0; i < productIndexList.Count; i++) {
                for(int j = 0; j < productsMatrix.GetLength(0); j++) {
                    piu[j,i] = productsMatrix[j,productIndexList[i]];
                }
            }
            //transposes piu matrix into new matrix variable
            double[,] transposedPiu = transposeMatrix(piu);
            Console.WriteLine("MATRIX Piu");
            printMatrix(piu);
            Console.WriteLine("MATRIX Piu^T");
            printMatrix(transposedPiu);
            

            return new double[2,2];
        }

        //transposes matrix, switches column indexs to row indexes (column 1 now becomes row 1, and row 1 becomes column 1)
        public double[,] transposeMatrix(double[,] matrix) {
            double[,] transposedMatrix = new double[matrix.GetLength(1),matrix.GetLength(0)];
            for(int i = 0; i < matrix.GetLength(1); i++) {
                for(int j = 0; j < matrix.GetLength(0); j++) {
                    transposedMatrix[i,j] = matrix[j,i];
                }
            }
            return transposedMatrix;
        }


        public void printMatrix(double[,] matrix) {
            
            for(int i = 0; i < matrix.GetLength(0); i++) {
                for(int j = 0; j < matrix.GetLength(1); j++) {
                    Console.Write(matrix[i,j].ToString()+" | ");
                }
                Console.WriteLine("\n+--------------------------------+");
            }

            Console.WriteLine("\n\n\n");
        }       
    }
}
