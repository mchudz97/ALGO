using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ALS_RECOMMENDATION_ALGORITHM
{
    class Parser
    {
        private String path;
        private StreamReader sr;
        private int recordLimiter;
        private int rateAmount;
        private int realLimit;
        private Dictionary<String, int> userDict;
        private Dictionary<String, int> productDict;
        private int productCounter=0;
        private int userCounter = 0;
        
        private String category;
        private HashSet<Rate> rateSet;
        public Parser(String path,String category, int productLimit, int rateAmount,int realLimit)
        {
            this.path = path;
            sr = new StreamReader(path);
            this.recordLimiter = productLimit;
            this.realLimit = realLimit;
            this.rateAmount = rateAmount;
            this.userDict = new Dictionary<String, int>();
            this.productDict = new Dictionary<String, int>();

            this.category = category;
            this.rateSet = new HashSet<Rate>(0);
            this.parse();
        }

        public Dictionary<string, int> UserDict { get => userDict; set => userDict = value; }
        public Dictionary<string, int> ProductDict { get => productDict; set => productDict = value; }

        public HashSet<Rate> RateSet { get => rateSet; set => rateSet = value; }

        public void parse()
        {
            
            String ln = "";
            while ((ln = sr.ReadLine()) != null)
            {

                //ln=ln.Replace(" ", "");
                if (productDict.Count == recordLimiter)
                {
                    break;
                }

                if (Regex.IsMatch(ln,  "Id:[ ]*[1-9].*"))
                {

                    int productCounterTMP = productCounter;
                    String productASIN = "";
                    bool correctCategory = false;
                    bool correctAmount = false;
                    while ((ln = sr.ReadLine()) != "")
                    {
                        
                        String customerASIN = "";
                        if(Regex.IsMatch(ln, "group: " + this.category))
                        {

                            correctCategory = true;
                        }

                        if (Regex.IsMatch(ln, "total: [0-9]*"))
                        {
                            String amount=Regex.Match(ln, "total:.*downloaded:").Value;
                            int rateAm = Int32.Parse(amount[6..^11].Trim());
                            if (rateAm >= this.rateAmount)
                            {
                                correctAmount = true;
                            }
                        }

                        if (Regex.IsMatch(ln, "^ASIN.*"))
                        {
                            
                            ln =ln.Replace(" ", "");
                            String[] parts = ln.Split(":");
                            productASIN = parts[1];
                            
                        }

                        if (correctCategory == true && correctAmount==true)
                        {
                            if (!productDict.ContainsKey(productASIN))
                            {
                                productDict.Add(productASIN, productCounter);
                                productCounter++;
                            }
                            
                            if(Regex.IsMatch(ln, ".*cutomer"))
                            {
                                int uIndex = 0;
                                String tmp = Regex.Match(ln, "cutomer:.*rating:").Value;
                                customerASIN= tmp[8..^7].Trim();

                                if (!userDict.ContainsKey(customerASIN))
                                {
                                    
                                    userDict.Add(customerASIN, userCounter);
                                    uIndex = userCounter;
                                    userCounter++;
                                }
                                else
                                {
                                    uIndex=userDict[customerASIN];
                                }
                                tmp = Regex.Match(ln, "rating:.*votes:").Value;
                                double rate = Double.Parse(tmp[7..^7].Trim());

                                
                                this.addRate(new Rate(rate, productCounterTMP, uIndex));                                
                            }



                        }
                        
                    }

                }

            }
            this.deleteUsersProducts();
            this.newDictionariesSetter(this.realLimit);
        }

        public void addRate(Rate newRate){
            bool doUpdate = false;
            foreach(Rate oldRate in this.rateSet){
                if(oldRate.User.Equals(newRate.User) && oldRate.Product.Equals(newRate.Product)){
                    oldRate.Value = newRate.Value;
                    doUpdate = true;
                }
            }
            if(doUpdate.Equals(false)){
                rateSet.Add(newRate);
            }
        }

        public Dictionary<String,int> findUsersToDelete(HashSet<Rate> rates){
            Dictionary<String,int> usersToDelete = new Dictionary<String, int>();
            foreach(KeyValuePair<String,int> kvp in userDict){
                int findNext=0;
                foreach(Rate rate in rates){
                    if(rate.User.Equals(kvp.Value)){
                        findNext++;
                    }
                }
                if(findNext < 2){
                    usersToDelete.Add(kvp.Key,kvp.Value);
                }
            }
            return usersToDelete;
        }

        public Dictionary<String,int> findProductsToDelete(HashSet<Rate> rates){
            Dictionary<String,int> productsToDelete = new Dictionary<String,int>();
            foreach(KeyValuePair<String,int> kvp in productDict){
                int findNext=0;
                foreach(Rate rate in rates){
                    if(rate.Product.Equals(kvp.Value)){
                        findNext++;
                    }
                }
                if(findNext<1){
                    productsToDelete.Add(kvp.Key,kvp.Value);
                }
            }
            return productsToDelete;
        }

        public HashSet<Rate> findRatesToDelete(HashSet<Rate> rates){
            Dictionary<String,int> usersToDelete = findUsersToDelete(this.rateSet);
            HashSet<Rate> ratesToDelete = new HashSet<Rate>();
            foreach(KeyValuePair<String,int> user in usersToDelete){
                foreach(Rate rate in rates){
                    if(user.Value.Equals(rate.User)){
                        ratesToDelete.Add(rate);
                    }
                }
            }
            return ratesToDelete;   
        } 

        public void deleteUsersProducts(){
            Dictionary<String,int> usersToDelete = findUsersToDelete(this.rateSet);
            HashSet<Rate> ratesToDelete = findRatesToDelete(this.rateSet);
            foreach(KeyValuePair<String,int> kvp in usersToDelete){
                userDict.Remove(kvp.Key);
            }
            foreach(Rate rate in ratesToDelete){
                this.rateSet.Remove(rate);
            }
            Dictionary<String,int> productsToDelete = findProductsToDelete(this.rateSet);
            foreach(KeyValuePair<String,int> kvp in productsToDelete){
                productDict.Remove(kvp.Key);
            }
        }

        public Dictionary<String,int> productLimiter(int numberOfUsers){
            int index=0;
            Dictionary<String,int> productLimitDictionary = new Dictionary<String, int>();
            foreach(KeyValuePair<String,int> kvp in productDict){
                if(index.Equals(numberOfUsers)){
                    break;
                } else{
                    productLimitDictionary.Add(kvp.Key,kvp.Value);
                    index++;
                }
            }
            return productLimitDictionary;
        }

        public HashSet<Rate> rateLimiter(int numberOfUsers){
            HashSet<Rate> rateLimiterHashSet = new HashSet<Rate>();
            Dictionary<String,int> productLimitDictionary = productLimiter(numberOfUsers);
            foreach(KeyValuePair<String,int> kvp in productLimitDictionary){
                foreach(Rate rate in rateSet){
                    if(rate.Product.Equals(kvp.Value)){
                        rateLimiterHashSet.Add(rate);
                    }
                }
            }
            return rateLimiterHashSet;
        }

        public Dictionary<String,int> userLimiter(int numberOfUsers){
            HashSet<Rate> rateLimiterHashSet = rateLimiter(numberOfUsers);
            Dictionary<String,int> productLimitDictionary = new Dictionary<String, int>();
            foreach(KeyValuePair<String,int> kvp in userDict){
                foreach(Rate rate in rateLimiterHashSet){
                    if(rate.User.Equals(kvp.Value)){
                        productLimitDictionary.Add(kvp.Key,kvp.Value);
                        break;
                    }
                }
            }
            return productLimitDictionary;
        }

        public void newDictionariesSetter(int numberOfUsers){
            
            Dictionary<String,int> productLimitDictionary = productLimiter(numberOfUsers);
            HashSet<Rate> rateLimiterHashSet = rateLimiter(numberOfUsers);
            Dictionary<String,int> userLimitDictionary = userLimiter(numberOfUsers);
            
            productDict.Clear();
            RateSet.Clear();
            userDict.Clear();
            this.productDict = productLimitDictionary;
            this.rateSet = rateLimiterHashSet;
            this.userDict = userLimitDictionary;
        }
    }
}
