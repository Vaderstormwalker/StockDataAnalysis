using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sideproject1.DLL;

namespace sideproject1
{
    public partial class Form1 : Form
    {
        private Parser dll_Parser = new Parser();
        private DLL_SQL dll_SQL = new DLL_SQL();
        //private ExcelMod dll_ExcelMod = new ExcelMod();
        private OutPutExcel dll_OutPutExcel = new OutPutExcel();
        public Form1()
        {

            Load += async (sender, e) => await ParseDataAsync();// 非同步  executing


            InitializeComponent(); // excutes
        }
        private async Task ParseDataAsync()
        {
            dll_SQL.NoQuerySQL(@"update [TestDB].[dbo].[STOCK_PROFILE]
                                set STATUS='D'
                                where  STATUS='Y'");
            await dll_Parser.ParseHtmlAsync();
            dll_OutPutExcel.CreateExcel();
            //dll_ExcelMod.ExportDataAndCreatChart();

            // ... 進一步處理或更新 UI
        }


    }
}


//string SQL = @"SELECT [STOCK_NO]
//                    ,[STOCK_NAME]
//                    ,[INSERT_DATE]
//                    ,[STOCK_PRICE]
//                     FROM [TestDB].[dbo].[STOCK_PROFILE]
//                      WHERE    (STOCK_NO = '1234')";

// string SQL =

// var dt = dll_SQL.Query(SQL);