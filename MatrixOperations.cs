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

        public double[,] generateBp(int product, double[,] usersMatrix) {
            //list that contains product index in productsMatrix
            List<int> userIndexList = new List<int>();

            //for every rate check if user id(int) == rate user id(int)
            // if YES add productIndex to list ONLY if NOT added already
            for(int i = 0 ; i < rateList.Count; i++) {
                Rate rate  = rateList[i];
                if(rate.Product == product) {
                    int userIndex = rate.User;
                    if(!userIndexList.Contains(userIndex))
                        userIndexList.Add(userIndex);
                }
            }
            
            //sort by index ascending
            userIndexList.Sort();

            //reduces productMatrix into subMatrix piu that contains the products that the user rated 
            double[,] bip = new double[userIndexList.Count, usersMatrix.GetLength(1)];

            for(int i = 0; i < userIndexList.Count; i++) {
                for(int j = 0; j < usersMatrix.GetLength(1); j++) {
                    bip[i,j] = usersMatrix[userIndexList[i], j];
                }
            }
            printMatrix(bip);
            //transposes piu matrix into new matrix variable
            double[,] transposedBip = transposeMatrix(bip);

            //multiplies two bips
            double[,] multipliedBipMatrixes = Times(bip,transposedBip);

            //generates diagonal 1 matrix and multiplies 1's by lambda
            double[,] lambdaMatrix = generateLambdaMatrix(0.1, multipliedBipMatrixes.GetLength(0), multipliedBipMatrixes.GetLength(1));

            //sum of lambda matrix and multiPius = au
            double[,] Bp = Plus(multipliedBipMatrixes,lambdaMatrix);

            //double[,] lambdaMatrix = generateLambdaMatrix
            printMatrix(Bp);
            return Bp;
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
            double[,] piu = new double[productIndexList.Count, productsMatrix.GetLength(1)];

            for(int i = 0; i < productIndexList.Count; i++) {
                for(int j = 0; j < productsMatrix.GetLength(1); j++) {
                    piu[i,j] = productsMatrix[productIndexList[i], j];
                }
            }
            

            //transposes piu matrix into new matrix variable
            double[,] transposedPiu = transposeMatrix(piu);

            //multiplies two pius
            double[,] multipliedPiuMatrixes = Times(piu,transposedPiu);

            //generates diagonal 1 matrix and multiplies 1's by lambda
            double[,] lambdaMatrix = generateLambdaMatrix(0.1, multipliedPiuMatrixes.GetLength(0), multipliedPiuMatrixes.GetLength(1));

            //sum of lambda matrix and multiPius = au
            double[,] au = Plus(multipliedPiuMatrixes,lambdaMatrix);

            //double[,] lambdaMatrix = generateLambdaMatrix
           
            return au;

        }

        //transposes matrix, switches column indexs to row indexes (column 1 now becomes row 1, and row 1 becomes column 1)
        private double[,] transposeMatrix(double[,] matrix) {
            double[,] transposedMatrix = new double[matrix.GetLength(1),matrix.GetLength(0)];
            for(int i = 0; i < matrix.GetLength(1); i++) {
                for(int j = 0; j < matrix.GetLength(0); j++) {
                    transposedMatrix[i,j] = matrix[j,i];
                }
            }
            return transposedMatrix;
        }

        //generates diagonal 1's across zeroed matrix and multiplys 1s by lamda
        private double[,] generateLambdaMatrix(double lambda, int n, int m) {
            double[,] lambdaMatrix = new double[n,m];
            for(int i = 0; i < n; i++) {
                for(int j = 0; j < m; j++) {
                    if(i == j)
                        lambdaMatrix[i,j] = lambda;
                    else
                        lambdaMatrix[i,j] = 0.0;
                }
            }
            return lambdaMatrix;
        }

        //matrix addition method
        private double[,] Plus(double[,] a , double[,] b) {
            if(a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new System.ArgumentException("Matrixes must be the same size when adding");
            
            for(int i = 0; i < a.GetLength(0); i++) {
                for(int j = 0; j < a.GetLength(1); j++) {
                    a[i,j] += b[i,j];
                }
            }
            return a;
        }
        //matrix multiplication method
        private double[,] Times(double[,] a, double[,] b) {
            if(a.GetLength(1) != b.GetLength(0))
                            throw new System.ArgumentException("Matrixes must be able to multiply");
            
            double[,] multipliedMatrix = new double[a.GetLength(0), b.GetLength(1)];
            for(int i = 0; i < a.GetLength(0); i++) {
                for(int j = 0; j < b.GetLength(1); j++) {
                    double sum = 0;
                    for(int k = 0; k < a.GetLength(1); k++) {
                        sum+= a[i,k]*b[k,j];
                    }
                    multipliedMatrix[i,j] = sum;
                }

            }
            return multipliedMatrix;

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
