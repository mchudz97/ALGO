namespace ALS_RECOMMENDATION_ALGORITHM
{
    internal class Rate
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

        public double Value { get => value; set => this.value = value; }
        public int Product { get => product; set => product = value; }
        public int User { get => user; set => user = value; }

        public override string ToString()
        {
            return "Rate: " + this.value + " Product: " + this.product + " User: " + this.user;
        }
    }
}