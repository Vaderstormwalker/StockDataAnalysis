using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sideproject1;
using sideproject1.DLL;
using sideproject1.Model;

namespace sideproject1
{
    public class StockDbContext : DbContext
    {
        // 如果您的連接字符串在 app.config 或 web.config 文件中定義
        public StockDbContext() : base("name=YourConnectionStringName")
        {
        }

        public DbSet<StockData> StockDatas { get; set; } // 映射到股票資料的表
        public DbSet<StockDetail> StockDetails { get; set; } // 映射到股票詳細資料的表

        // 其他 DbContext 配置或方法
    }

}
