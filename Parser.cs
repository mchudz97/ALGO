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
        private Dictionary<String, int> userDict;
        private Dictionary<String, int> productDict;
        private int productCounter=0;
        private int userCounter = 0;
        private List<Rate> rateList;
        private String category;

        public Parser(String path,String category, int limit)
        {
            this.path = path;
            sr = new StreamReader(path);
            this.recordLimiter = limit+1;
            this.userDict = new Dictionary<String, int>();
            this.productDict = new Dictionary<String, int>();
            this.rateList = new List<Rate>(0);
            this.category = category;

            this.parse();
        }

        public Dictionary<string, int> UserDict { get => userDict; set => userDict = value; }
        public Dictionary<string, int> ProductDict { get => productDict; set => productDict = value; }
        public List<Rate> RateList { get => rateList; set => rateList = value; }

        public void parse()
        {
            
            String ln = "";
            while ((ln = sr.ReadLine()) != null)
            {
               
                //ln=ln.Replace(" ", "");
                if (productDict.Count==recordLimiter)
                {
                    break;
                }

                if (Regex.IsMatch(ln,  "Id:[ ]*[1-9].*"))
                {

                    
                    String productASIN = "";
                    bool correctCategory = false;        
                    while ((ln = sr.ReadLine()) != "")
                    {
                        
                        String customerASIN = "";
                        if(Regex.IsMatch(ln, "group: " + this.category))
                        {
                            correctCategory = true;
                        }
                        if (Regex.IsMatch(ln, "^ASIN.*"))
                        {
                            productCounter++;
                            ln =ln.Replace(" ", "");
                            String[] parts = ln.Split(":");
                            productASIN = parts[1];
                            
                        }
                        if (correctCategory == true)
                        {
                            if (!productDict.ContainsKey(productASIN))
                            {
                                productDict.Add(productASIN, productCounter);
                            }
                            
                            if(Regex.IsMatch(ln, ".*cutomer"))
                            {
                                int uIndex = 0;
                                String tmp = Regex.Match(ln, "cutomer:.*rating:").Value;
                                customerASIN= tmp[8..^7].Trim();

                                if (!userDict.ContainsKey(customerASIN))
                                {
                                    userCounter++;
                                    userDict.Add(customerASIN, userCounter);
                                    uIndex = userCounter;
                                }
                                else
                                {
                                    uIndex=userDict[customerASIN];
                                }
                                tmp = Regex.Match(ln, "rating:.*votes:").Value;
                                double rate = Double.Parse(tmp[7..^7].Trim());
                                rateList.Add(new Rate(rate, productCounter, uIndex));
                            }



                        }
                        
                    }

                }

            }

        }

        
    }
}
