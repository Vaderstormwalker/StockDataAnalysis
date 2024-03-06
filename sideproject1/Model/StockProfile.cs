using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sideproject1.Model
{
    public class StockData
    {
        public string Stock_No { get; set; } = "";
        public string Name { get; set; } = "";
        public string StockPrice { get; set; } = "";
        public string PriceChange { get; set; } = "";
        public string ChangePercent { get; set; } = "";
        public string Opening_Price { get; set; } = "";
        public string Last_Close { get; set; } = "";
        public string Volume { get; set; } = "";
        public string DetailInfo { get; set; } = "";
        public StockDetail Detail { get; set; } // 新增屬性來儲存詳細資訊
    }
    public class StockDetail
    {
        public string Three_days { get; set; } = "";
        public string Biweekly { get; set; } = "";
        public string Monthly { get; set; } = "";
        public string Season { get; set; } = "";
        public string Six_month { get; set; } = "";

    }
}
