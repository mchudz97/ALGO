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
        private HashSet<Rate> rateSet;
        private int factorsAmount;
        Randomizer randomizer;
        

        public MatrixOperations(Parser p, int factorsAmount)
        {
            p.parse();
            this.userDict = p.UserDict;
            this.productDict = p.ProductDict;
            
            this.rateSet = p.RateSet;
            this.factorsAmount = factorsAmount;
            this.randomizer = new Randomizer();
        }
        public void ALS(double regDegree)
        {
            double[,] productMatrix = randomizer.Randomize(productDict.Count,this.factorsAmount);
            double[,] userMatrix = randomizer.Randomize(userDict.Count, this.factorsAmount);
           
            double funcOld = 0;

            while (true)
            {
                
                for(int i=0; i < userDict.Count; i++)
                {
                    double[] temp = this.generateXu(productMatrix, i);
                    for(int j = 0; j < factorsAmount; j++)
                    {
                        userMatrix[i, j] = temp[j];
                    }
                }
                for (int i = 0; i < productDict.Count; i++)
                {
                    double[] temp = this.generateXp(userMatrix, i);
                    for (int j = 0; j < factorsAmount; j++)
                    {
                        productMatrix[i, j] = temp[j];
                    }
                }
                double funcNew = this.targetFunc(productMatrix, userMatrix);
                Console.WriteLine(funcNew);
                 
                if((funcOld/funcNew)<=(1+regDegree) && (funcOld/funcNew) >= (1 - regDegree))
                {
                    
                    break;
                }
 
                funcOld = funcNew;
                
            }
            printRmatrixes(productMatrix, userMatrix);
        }
        private double[] generateXu(double [,] products, int indexUser)
        {
            return this.PG(this.generateAu(indexUser, products), this.generateVu(products, indexUser));
        }
        private double[] generateXp(double [,] users, int indexProduct)
        {
            return this.PG(this.generateBp(indexProduct, users), this.generateWp(users, indexProduct));
        }

        private double[] generateVu(double[,] products, int indexUser )
        {
         
            double[] vu= new double[products.GetLength(1)];
            foreach(Rate r in rateSet)
            {
                if (r.User == indexUser)
                {
                    for (int i = 0; i < products.GetLength(1); i++)
                    {
                        vu[i] = vu[i] + products[r.Product,i] * r.Value;

                    }
                    
                }
            }
            return vu;
        }
        private double[] generateWp(double[,] users, int indexProduct)
        {
            
            double[] wp = new double[users.GetLength(1)];
            foreach(Rate r in rateSet)
            {
                if (r.Product == indexProduct)
                {
                    for(int i = 0; i < users.GetLength(1); i++)
                    {
                        wp[i] = wp[i] + users[r.User,i] * r.Value;
                    }
                    
                }
            }
            return wp;
        }


        //DARIUSZ


        public double[,] generateBp(int product, double[,] usersMatrix)
        {
            //list that contains product index in productsMatrix
            List<int> userIndexList = new List<int>();

            //for every rate check if user id(int) == rate user id(int)
            // if YES add productIndex to list ONLY if NOT added already
           /* for (int i = 0; i < rateList.Count; i++)
            {
                Rate rate = rateList[i];
                if (rate.Product == product)
                {
                    int userIndex = rate.User;
                    if (!userIndexList.Contains(userIndex))
                        userIndexList.Add(userIndex);
                }
            }*/
            foreach(Rate r in rateSet)
            {
                if (r.Product == product)
                {
                    int userIndex = r.User;
                    if (!userIndexList.Contains(userIndex))
                        userIndexList.Add(userIndex);
                }
            }

            //sort by index ascending
            userIndexList.Sort();

            //reduces productMatrix into subMatrix piu that contains the products that the user rated 
            double[,] bip = new double[userIndexList.Count, this.factorsAmount];

            for (int i = 0; i < userIndexList.Count; i++)
            {
                for (int j = 0; j < this.factorsAmount; j++)
                {
                    bip[i, j] = usersMatrix[userIndexList[i], j];
                }
            }

            
            double[,] multiplied = multiplyMat(bip);
            double[,] bp = addLambda(0.1, multiplied);
            return bp;
        }

        public double[,] generateAu(int user, double[,] productsMatrix)
        {
            //list that contains product index in productsMatrix
            List<int> productIndexList = new List<int>();

            //for every rate check if user id(int) == rate user id(int)
            // if YES add productIndex to list ONLY if NOT added already
            /*for (int i = 0; i < rateList.Count; i++)
            {
                Rate rate = rateList[i];
                if (rate.User == user)
                {
                    int productIndex = rate.Product;
                    if (!productIndexList.Contains(productIndex))
                        productIndexList.Add(productIndex);
                }
            }*/
            foreach (Rate r in rateSet)
            {
                if (r.User == user)
                {
                    int productIndex = r.Product;
                    if (!productIndexList.Contains(productIndex))
                        productIndexList.Add(productIndex);
                }
            }

            //sort by index ascending
            productIndexList.Sort();

            //reduces productMatrix into subMatrix piu that contains the products that the user rated 
            double[,] piu = new double[productIndexList.Count, this.factorsAmount];

            for (int i = 0; i < productIndexList.Count; i++)
            {
                for (int j = 0; j < this.factorsAmount; j++)
                {
                    piu[i, j] = productsMatrix[productIndexList[i], j ];
                }
            }


            double[,] multiplied = multiplyMat(piu);
            double[,] au = addLambda(0.1, multiplied);

            return au;

        }

        private double[,] multiplyMat(double[,] mat)
        {
            double[,] tempMat = new double[mat.GetLength(1), mat.GetLength(1)];
            for (int i = 0; i < mat.GetLength(1); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    for (int k = 0; k < mat.GetLength(0); k++)
                    {
                        tempMat[j, i] += mat[k, i] * mat[k, j];
                    }
                }
            }
            return tempMat;
        }



        private double[,] addLambda(double lambda, double[,] mat)
        {
            for(int i=0; i < mat.GetLength(1); i++)
            {
                mat[i, i] += lambda;
            }
            return mat;
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



        private double[] PG(double[,] tab1, double[] tab2)
        {
            GaussPG(tab1, tab2);

            return this.Reverse(this.Eliminate(tab1, tab2));
        }

        private double[] Eliminate(double[,] tab1, double[] tab2)
        {
            double[] x = new double[tab2.Length];
            for (int i = tab2.Length - 1; i >= 0; i--)
            {
                for (int j = tab2.Length - 1; j > i; j--)
                {
                    tab2[i] -= (dynamic)tab1[j,i] * x[tab2.Length - 1 - j];
                }
                x[tab2.Length - 1 - i] = (dynamic)tab2[i] / tab1[i,i];
            }
            return x;
        }
        private void GaussPG(double[,] tabA, double[] tabB)
        {
            for (int i = 0; i < tabB.Length; i++)
            {
                Point p = this.GetBiggestPG(tabA, i);
                RowSwap(tabA, tabB, i, p.Y);
                ResetColumn(i, tabA, tabB);
            }
        }

        private void RowSwap(double[,] tab1, double[] tab2, int i, int j)
        {
            if (i == j) return;
            for (int k = i; k < tab1.GetLength(1); k++)
            {
                double temp = tab1[k,i];
                tab1[k,i] = tab1[k,j];
                tab1[k,j] = temp;
            }
            double temp2 = tab2[i];
            tab2[i] = tab2[j];
            tab2[j] = temp2;
        }


        private void ResetColumn(int i, double[,] t1, double[] t2)
        {

            double resetVal = t1[i,i];
            for (int j = i + 1; j < t2.Length; j++) //j jest nastepna wartoscia w kolumnnie pod przekatna
            {
                double check = t1[i,j] / resetVal; // wyznaczamy iloczyn zerujacy
                for (int k = i; k < t2.Length; k++)// jedziemy po wierszu
                {
                    t1[k,j] = t1[k,j] - t1[k,i] * check;

                }

                t2[j] = t2[j] - t2[i] * check; // t2 macierz B 

            }

        }

        private Point GetBiggestPG(double[,] tab, int p)
        {
            Point point = new Point(p, p);
            double val = tab[p,p];
            for (int i = p; i < tab.GetLength(1); i++)
            {
                if (Math.Abs(tab[p,i]) > Math.Abs(val))
                {
                    val = tab[p,i];
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
        private double targetFunc(double[,] products, double[,] users)
        {
            return this.rateDiff(products,users)+0.1 * this.matrixNorm(products) * this.matrixNorm(users);
        }
        private double rateDiff(double[,] prod, double[,] user)
        {
            double sum = 0;
            for (int i = 0; i < prod.GetLength(0); i++)
            {
                for (int j = 0; j < user.GetLength(0); j++)
                {
                    sum+=(this.getRealRate(i, j) - getCountedRate(i, j, prod, user))* (this.getRealRate(i, j) - getCountedRate(i, j, prod, user));
                }
            }
            return sum;
        }
        private double matrixNorm(double[,] tab)
        {
            double norm=0;
            for (int j = 0; j < tab.GetLength(0); j++)
            {
                for (int i = 0; i < tab.GetLength(1); i++)
                {
                    norm += tab[j, i] * tab[j, i];
                }
            }
            return norm;
        }
        private double getCountedRate(int prodID, int userID, double[,] prodMat, double[,] userMat)
        {
            double rate=0;
            for(int i=0; i < this.factorsAmount; i++)
            {
                rate += prodMat[prodID, i] * userMat[userID, i];
            }
            return rate;
        }
        private void printRmatrixes(double[,] prodMat, double[,] userMat)
        {
            for (int i = 0; i < userMat.GetLength(0); i++)
            {
                for (int j = 0; j < prodMat.GetLength(0); j++)
                {
                    Console.Write(Math.Round(getCountedRate(j, i, prodMat, userMat),2, MidpointRounding.ToEven) + " ");
                }
                Console.WriteLine();

            }
            Console.WriteLine("####################");
            for (int i = 0; i < userMat.GetLength(0); i++)
            {
                for (int j = 0; j < prodMat.GetLength(0); j++)
                {
                    Console.Write(getRealRate(j, i) + " ");
                }
                Console.WriteLine();
            }
        }
        private double getRealRate(int prodID, int userID)
        {
            foreach(Rate r in rateSet)
            {
                if(r.User==userID && r.Product == prodID)
                {
                    return r.Value;
                }
            }
            return 0;
        }
    }
}
