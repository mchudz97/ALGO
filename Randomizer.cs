using System;
using System.Collections.Generic;
namespace ALS_RECOMMENDATION_ALGORITHM
{
    static class Randomizer{
        //returns new matrix as row amount n and columns amount m with randomized values between 0 , 1)        
        public static double[,] Randomize(int n, int m) {
            Random  random = new Random();
            double[,] matrix = new double[n,m];
            for(int i = 0 ; i < n; i++) {
                for(int j = 0; j < m; j++) {
                    matrix[i,j] = random.NextDouble();
                }
            }
            return matrix;
        }
    }
}