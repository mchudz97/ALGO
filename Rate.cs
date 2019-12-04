using System;
using System.Collections.Generic;
using System.Text;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Rate
    {
        private double value;
        private int product;
        private int user;

        public Rate(double value, int product, int user)
        {
            this.value = value;
            this.product = product;
            this.user = user;


        }
        public override string ToString()
        {
            return "Rate: " + this.value + " Product: " + this.product+ " User: "+this.user;
        }
    }
}
