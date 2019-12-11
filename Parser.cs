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
        private Dictionary<String, int> userDict;
        private Dictionary<String, int> productDict;
        private int productCounter=0;
        private int userCounter = 0;
        private List<Rate> rateList;
        private String category;
        private HashSet<Rate> rateSet;
        public Parser(String path,String category, int productLimit, int rateAmount)
        {
            this.path = path;
            sr = new StreamReader(path);
            this.recordLimiter = productLimit;
            this.rateAmount = rateAmount;
            this.userDict = new Dictionary<String, int>();
            this.productDict = new Dictionary<String, int>();
            this.rateList = new List<Rate>(0);
            this.category = category;
            this.rateSet = new HashSet<Rate>(0);
            this.parse();
        }

        public Dictionary<string, int> UserDict { get => userDict; set => userDict = value; }
        public Dictionary<string, int> ProductDict { get => productDict; set => productDict = value; }
        public List<Rate> RateList { get => rateList; set => rateList = value; }
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
                                rateList.Add(new Rate(rate, productCounterTMP, uIndex));
                                rateSet.Add(new Rate(rate, productCounterTMP, uIndex));
                                
                            }



                        }
                        
                    }

                }

            }
            
        }
        
        
    }
}
