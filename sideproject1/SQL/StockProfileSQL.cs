using sideproject1.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sideproject1.SQL
{

    internal class StockProfileSQL
    {
        private DLL_SQL dll_sql = new DLL_SQL();

        public DataTable GetStockProfile()
        {
            DataTable dataTable = dll_sql.QuerySQL(@"
                           SELECT [STOCK_GUID]
                          ,[STOCK_NO]
                          ,[STOCK_NAME]
                          ,[CURRENT_PRICE]     
                           FROM [TestDB].[dbo].[STOCK_PROFILE]");
            return dataTable;
        }

 
    }
}
