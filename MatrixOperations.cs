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

        //private double[][]

        private double[] generateVu(double[][] products, int indexUser )
        {
            int matrixWidth = this.getProductsAmount(indexUser);
            double[] vu= new double[indexUser];
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
        private int getProductsAmount(int indexUser)
        {
            int amount=0;
            for(int i=0; i < rateList.Count; i++)
            {
                if (rateList[i].User == i)
                {
                    amount++;
                }
            }
            return amount;
        }

    }
}
