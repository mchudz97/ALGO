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
        
    }
}
