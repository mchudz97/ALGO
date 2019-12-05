using System;
using System.Collections.Generic;
using Random;
namespace ALS_RECOMMENDATION_ALGORITHM
{
    static class Randomizer{
        
        static double[][] Randomize(int n, int m) {
            Random  random = new Random();
            double[][] matrix = new double[n][m];
            for(int i = 0 ; i < n; i++) {
                for(int j = 0; j < m; j++) {
                    matrix[i][j] = random.NextDouble();
                }
            }
            return matrix;
        }
        
    }
    
}