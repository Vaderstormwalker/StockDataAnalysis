using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using sideproject1.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sideproject1.DLL
{
    public class Parser
    {
        private DLL_SQL dll_SQL = new DLL_SQL();
        public async Task<List<StockData>> ParseHtmlAsync()
        {
            List<StockData> stockData = new List<StockData>();

            string url = "https://histock.tw/global/globalclass.aspx?mid=0&id=137"; // URL // GET POST
            var config = Configuration.Default.WithDefaultLoader(); // 載入設定
            var dom = await BrowsingContext.New(config).OpenAsync(url); //開啟網站
            var page = dom.QuerySelectorAll("ul.stock-list.lh24.top-line>li");

            foreach (var item in page)
            {
                StockData stockTmp = new StockData();

                string priceChangeValue= item.QuerySelector("span:nth-child(6)")?.Text() ?? "";
                //string PriceChange = item.QuerySelector("span:nth-child(6)")?.Text() ?? "";
                //股票漲跌變化
                //string priceChangeText = priceChangeElement?.Text() ?? "";
                string priceChangeIndicator = "";

                if (priceChangeValue.Contains("▲"))
                    priceChangeIndicator = "+";
                else if (priceChangeValue.Contains("▼"))
                    priceChangeIndicator = "-";
                else if (priceChangeValue.Contains("--"))
                    priceChangeIndicator = "0";

                stockTmp.Stock_No = item.QuerySelector("span.w70.lft-p.stockno")?.Text() ?? "";
                stockTmp.Name = item.QuerySelector(".w160.lft-p")?.Text() ?? "";


                string ChangePercent = item.QuerySelector("span:nth-child(8)")?.Text() ?? "";
                string Volume = item.QuerySelector("span:nth-child(14)")?.Text() ?? "";

                //漲跌幅變化  
                string changePercentIndicator = "";

                if (ChangePercent.Contains(""))
                    changePercentIndicator = "+";
                else if (ChangePercent.Contains("-"))
                    changePercentIndicator = "-";
                else if (ChangePercent.Contains("--"))
                    changePercentIndicator = "0";


                stockTmp.Stock_No = item.QuerySelector("span.w70.lft-p.stockno")?.Text() ?? "";
                stockTmp.Name = item.QuerySelector(".w160.lft-p")?.Text() ?? ""; // if(A==NULL) { "" } else { A }
                stockTmp.StockPrice = item.QuerySelector("span.w85")?.Text() ?? "";
                stockTmp.PriceChange = priceChangeIndicator + ExtractNumber(priceChangeValue);                
                stockTmp.ChangePercent = changePercentIndicator + ExtractNumber(ChangePercent);//底部有ExtractNumber
                stockTmp.Opening_Price = item.QuerySelector("span:nth-child(10)")?.Text() ?? "";
                stockTmp.Last_Close = item.QuerySelector(" span:nth-child(12)")?.Text() ?? "";
                stockTmp.Volume = Volume.Replace(",","");
                stockTmp.DetailInfo = item.QuerySelector("a")?.GetAttribute("href") ?? "";

                //stockTmp.ChangePercent = ExtractNumber(ChangePercent) + changePercentIndicator;

                if (!string.IsNullOrEmpty(stockTmp.DetailInfo))
                {
                    //ParseDetail(stockTmp.DetailInfo);
                    string detailPageUrl = "https://histock.tw" + stockTmp.DetailInfo;

                    stockTmp.Detail = await ParseDetailAsync(detailPageUrl);
                    StockDetail stockDetail = await ParseDetailAsync(detailPageUrl);

                    //InsertStockDetail(stockDetail);

                    await Task.Delay(6000);

                     

                    string SQL = $@"INSERT INTO [dbo].[STOCK_PROFILE]
                    (
                         [STOCK_NO]
                        ,[STOCK_NAME]
                        ,[CURRENT_PRICE]
                        ,[SUB_PRICE]
                        ,[SUB_PERCENT]
                        ,[OPEN_PRICE]
                        ,[LAST_PRICE]
                        ,[TRADE_AMOUNT]
                        ,[INSERT_DATE]
                        ,[THREE_DAYS]
                        ,[BIWEEKLY]
                        ,[MONTHLY]
                        ,[SEASON]
                        ,[SIX_MONTH]
                        ,[STATUS]
                    )
                    VALUES
                    (
                    '{stockTmp.Stock_No}',
                    N'{stockTmp.Name}',
                    '{stockTmp.StockPrice}',
                    '{stockTmp.PriceChange}',
                    '{stockTmp.ChangePercent}',
                    '{stockTmp.Opening_Price}',
                    '{stockTmp.Last_Close}',
                    '{stockTmp.Volume}',
                    GETDATE(),
                    '{stockDetail.Three_days}',
                    '{stockDetail.Biweekly}',
                    '{stockDetail.Monthly}',
                    '{stockDetail.Season}',
                    '{stockDetail.Six_month}',
                    'Y'
                    )";

                    dll_SQL.NoQuerySQL(SQL);
                    stockData.Add(stockTmp);
                }


                
            }

            
            return stockData;
        }
        private async Task<StockDetail> ParseDetailAsync(string detailPageUrl)
        {
            // DetailInfo 進行解析詳細頁面的邏輯
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(detailPageUrl);

            //var specificData = document.QuerySelector("#LBlock_3 > table")?.Text()?? "Data not found";

            StockDetail stockDetail = new StockDetail
            {
                Three_days = document.QuerySelector("tr:nth-child(2) > td:nth-child(2) > span")?.Text() ?? "",
                Biweekly = document.QuerySelector("tr:nth-child(4) > td:nth-child(2) > span")?.Text() ?? "",
                Monthly = document.QuerySelector("tr:nth-child(6) > td:nth-child(2) > span")?.Text() ?? "",
                Season = document.QuerySelector("tr:nth-child(7) > td:nth-child(2) > span")?.Text() ?? "",
                Six_month = document.QuerySelector("tr:nth-child(8) > td:nth-child(2) > span")?.Text() ?? "", 

            };
            return stockDetail;

        }
       public void InsertStockDetail(StockDetail stockDetail)
        {
            string SQL = $@"INSERT INTO [dbo].[STOCK_PROFILE]
                    ([THREE_DAYS], [BIWEEKLY], [MONTHLY], [SEASON],[SIX_MONTH])
            VALUES
                   (
                    '{stockDetail.Three_days}',
                    '{stockDetail.Biweekly}',
                    '{stockDetail.Monthly}',
                    '{stockDetail.Season}',
                    '{stockDetail.Six_month}'
                   )";
            Console.WriteLine(SQL);
            dll_SQL.NoQuerySQL(SQL);
            
        }
        
        public string ExtractNumber (string InputString)
        {
            var match = Regex.Match(InputString, @"[+]?\d+(\.\d+)?%?");
            return match.Success ? match.Value : null;

            //string resultString = Regex.Match(InputString, @"\d+(\.\d+)?").Value;
            //return resultString;
        }
    }

}
//Console.WriteLine(item.QuerySelector("span:nth-child(14)")?.Text()?? "not found");


//string test = item.QuerySelector("span:nth-child(14)")?.Text() ?? "";
//string TEST = 1 == 1 ? "true" : "false";
//if (1 == 1)
//    TEST = "true";
//else
//    TEST = "false";

//foreach(var stock in stockData)
//{
//    string SQL = $@"INSERT INTO [dbo].[STOCK_PROFILE]
//   ([STOCK_NO],[STOCK_NAME],[STOCK],[PRICE_CHANGE],[CHANGE_PERCENT]
//   ,[OPENING_PRICE],[LAST_CLOSE],[VOLUNE],[INSERT_DATE])  
//    VALUES               
//    ('{stock.DetailInfo.Replace("/","").Replace("stock","")}',
//    N'{stock.Name}',
//    '', 
//    '',
//    '',
//    '',
//    '',
//    '',
//    GETDATE()";

//    dll_SQL.Query(SQL);
//}