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

        

        public MatrixOperations(Parser p)
        {
            p.parse();
            this.userDict = p.UserDict;
            this.productDict = p.ProductDict;
            this.rateList = p.RateList;
        }

        private double[] generateXu(double [][] products, int indexUser)
        {
            return this.PG(this.generateAu(indexUser,products),this.generateVu(products, indexUser))
        }

        private double[] generateVu(double[][] products, int indexUser )
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
        private double[] generateWp(double[][] users, int indexProduct)
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


        //DARIUSZ


        private double[,] generateBp(int product, double[,] usersMatrix)
        {
            //list that contains product index in productsMatrix
            List<int> userIndexList = new List<int>();

            //for every rate check if user id(int) == rate user id(int)
            // if YES add productIndex to list ONLY if NOT added already
            for (int i = 0; i < rateList.Count; i++)
            {
                Rate rate = rateList[i];
                if (rate.Product == product)
                {
                    int userIndex = rate.User;
                    if (!userIndexList.Contains(userIndex))
                        userIndexList.Add(userIndex);
                }
            }

            //sort by index ascending
            userIndexList.Sort();

            //reduces productMatrix into subMatrix piu that contains the products that the user rated 
            double[,] bip = new double[userIndexList.Count, usersMatrix.GetLength(1)];

            for (int i = 0; i < userIndexList.Count; i++)
            {
                for (int j = 0; j < usersMatrix.GetLength(0); j++)
                {
                    bip[i, j] = usersMatrix[userIndexList[i], j];
                }
            }
            //transposes piu matrix into new matrix variable
            double[,] transposedBip = transposeMatrix(bip);

            //multiplies two bips
            double[,] multipliedBipMatrixes = Times(bip, transposedBip);

            //generates diagonal 1 matrix and multiplies 1's by lambda
            double[,] lambdaMatrix = generateLambdaMatrix(0.1, multipliedBipMatrixes.GetLength(0), multipliedBipMatrixes.GetLength(1));

            //sum of lambda matrix and multiPius = au
            double[,] Bp = Plus(multipliedBipMatrixes, lambdaMatrix);

            //double[,] lambdaMatrix = generateLambdaMatrix
            return Bp;
        }

        private double[,] generateAu(int user, double[,] productsMatrix)
        {
            //list that contains product index in productsMatrix
            List<int> productIndexList = new List<int>();

            //for every rate check if user id(int) == rate user id(int)
            // if YES add productIndex to list ONLY if NOT added already
            for (int i = 0; i < rateList.Count; i++)
            {
                Rate rate = rateList[i];
                if (rate.User == user)
                {
                    int productIndex = rate.Product;
                    if (!productIndexList.Contains(productIndex))
                        productIndexList.Add(productIndex);
                }
            }

            //sort by index ascending
            productIndexList.Sort();

            //reduces productMatrix into subMatrix piu that contains the products that the user rated 
            double[,] piu = new double[productsMatrix.GetLength(0), productIndexList.Count];

            for (int i = 0; i < productIndexList.Count; i++)
            {
                for (int j = 0; j < productsMatrix.GetLength(0); j++)
                {
                    piu[j, i] = productsMatrix[j, productIndexList[i]];
                }
            }
            //transposes piu matrix into new matrix variable
            double[,] transposedPiu = transposeMatrix(piu);

            //multiplies two pius
            double[,] multipliedPiuMatrixes = Times(piu, transposedPiu);

            //generates diagonal 1 matrix and multiplies 1's by lambda
            double[,] lambdaMatrix = generateLambdaMatrix(0.1, multipliedPiuMatrixes.GetLength(0), multipliedPiuMatrixes.GetLength(1));

            //sum of lambda matrix and multiPius = au
            double[,] au = Plus(multipliedPiuMatrixes, lambdaMatrix);

            //double[,] lambdaMatrix = generateLambdaMatrix
            return au;
        }

        //transposes matrix, switches column indexs to row indexes (column 1 now becomes row 1, and row 1 becomes column 1)
        private double[,] transposeMatrix(double[,] matrix)
        {
            double[,] transposedMatrix = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }
            }
            return transposedMatrix;
        }

        //generates diagonal 1's across zeroed matrix and multiplys 1s by lamda
        private double[,] generateLambdaMatrix(double lambda, int n, int m)
        {
            double[,] lambdaMatrix = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (i == j)
                        lambdaMatrix[i, j] = lambda;
                    else
                        lambdaMatrix[i, j] = 0.0;
                }
            }
            return lambdaMatrix;
        }

        //matrix addition method
        private double[,] Plus(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new System.ArgumentException("Matrixes must be the same size when adding");

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] += b[i, j];
                }
            }
            return a;
        }
        //matrix multiplication method
        private double[,] Times(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0))
                throw new System.ArgumentException("Matrixes must be able to multiply");

            double[,] multipliedMatrix = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.GetLength(1); k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    multipliedMatrix[i, j] = sum;
                }

            }
            return multipliedMatrix;

        }


        public void printMatrix(double[,] matrix)
        {

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j].ToString() + " | ");
                }
                Console.WriteLine("\n+--------------------------------+");
            }

            Console.WriteLine("\n\n\n");
        }



        //DARIUSZ



        private double[] PG(double[][] tab1, double[] tab2)
        {
            GaussPG(tab1, tab2);

            return this.Reverse(this.Eliminate(tab1, tab2));
        }

        private double[] Eliminate(double[][] tab1, double[] tab2)
        {
            double[] x = new double[tab2.Length];
            for (int i = tab2.Length - 1; i >= 0; i--)
            {
                for (int j = tab2.Length - 1; j > i; j--)
                {
                    tab2[i] -= (dynamic)tab1[j][i] * x[tab2.Length - 1 - j];
                }
                x[tab2.Length - 1 - i] = (dynamic)tab2[i] / tab1[i][i];
            }
            return x;
        }
        private void GaussPG(double[][] tabA, double[] tabB)
        {
            for (int i = 0; i < tabA.Length; i++)
            {
                Point p = this.GetBiggestPG(tabA, i);
                RowSwap(tabA, tabB, i, p.Y);
                ResetColumn(i, tabA, tabB);
            }
        }

        private void RowSwap(double[][] tab1, double[] tab2, int i, int j)
        {
            if (i == j) return;
            for (int k = i; k < tab1.Length; k++)
            {
                double temp = tab1[k][i];
                tab1[k][i] = tab1[k][j];
                tab1[k][j] = temp;
            }
            double temp2 = tab2[i];
            tab2[i] = tab2[j];
            tab2[j] = temp2;
        }


        private void ResetColumn(int i, double[][] t1, double[] t2)
        {

            double resetVal = t1[i][i];
            for (int j = i + 1; j < t2.Length; j++) //j jest nastepna wartoscia w kolumnnie pod przekatna
            {
                double check = t1[i][j] / resetVal; // wyznaczamy iloczyn zerujacy
                for (int k = i; k < t2.Length; k++)// jedziemy po wierszu
                {
                    t1[k][j] = t1[k][j] - t1[k][i] * check;

                }

                t2[j] = t2[j] - t2[i] * check; // t2 macierz B 

            }

        }

        private Point GetBiggestPG(double[][] tab, int p)
        {
            Point point = new Point(p, p);
            double val = tab[p][p];
            for (int i = p; i < tab.Length; i++)
            {
                if (Math.Abs(tab[p][i]) > Math.Abs(val))
                {
                    val = tab[p][i];
                    point.Y = i;
                }
            }
            return point;
        }

        private double[] Reverse(double[] t)
        {
            double[] temp = new double[t.Length];
            for (int i = t.Length - 1; i >= 0; i--)
            {
                temp[t.Length - 1 - i] = t[i];
            }
            return temp;
        }
    }
}
