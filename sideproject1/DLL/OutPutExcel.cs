using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Drawing.Chart;

namespace sideproject1.DLL
{
    public class OutPutExcel
    {
        private DLL_SQL dll_sql = new DLL_SQL();
        public void CreateExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            DataTable StockData = dll_sql.QuerySQL("SELECT * FROM [TestDB].[dbo].[STOCK_PROFILE] where Status ='Y' ");
           
            
            //在這裡轉換交易量列為數值類型
            foreach (DataRow row in StockData.Rows)
            {
                if (long.TryParse(row["TRADE_AMOUNT"].ToString(), out long tradeAmount))
                {
                    row["TRADE_AMOUNT"] = tradeAmount;
                }
                else
                {
                    row["TRADE_AMOUNT"] = 0;// 或根據需求處理無法解析的情況
                }
            }

            string baseFilePath = @"..\..\..\SideProject";
            string fileExtension = ".xlsx";
            string datetime = DateTime.Now.ToString("yyyyMMdd");
            int fileIndex = 1;

            string filePath;
            do
            {
                filePath = $"{baseFilePath}-{datetime}-{fileIndex}{fileExtension}";
                fileIndex++;
            } while (File.Exists(filePath));
            var file = new FileInfo(filePath);

            using (var package = new ExcelPackage(file))
            {
                CreateExcelFileWithChart(StockData, package);
                package.SaveAs(file);
            }
            Console.WriteLine($"已成功將數據輸出至Excel，檔案路徑: {filePath}");




            // 現在 dataTable 包含了查詢結果
            // 接下來將 dataTable 的數據導出到 Excel 文件
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
           
            //using (var package = new ExcelPackage(file)) // 使用，使用完，釋放
            //{             
            //    CreateExcelFileWithChart(StockData, package);
            //    package.SaveAs(file);
            //}
            //Console.WriteLine("已成功將數據輸出至Excel!!");

        }
        private void CreateExcelFileWithChart(DataTable dataTable, ExcelPackage package)
        {
            //數據 工作表
            var DataWorksheet = package.Workbook.Worksheets.Add("Data");
            DataWorksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

            // 假設您的數據在第二列，並且您想對這些數據創建圖表
            // 創建柱狀圖
            //var columnSeries = columnchart.Series.Add(
            //            DataWorksheet.Cells[$"B2:B{dataTable.Rows.Count + 1}"], // Y 軸數據
            //            DataWorksheet.Cells[$"A2:A{dataTable.Rows.Count + 1}"]  // X 軸類別
            //        );
            //柱狀圖工作表
            var columnchartWorksheet = package.Workbook.Worksheets.Add("Column Chart");
            var columnchart = columnchartWorksheet.Drawings.AddChart("StockChart", eChartType.ColumnClustered);
            columnchart.Title.Text = "股票分析柱狀圖";

            // 假設數據範圍是從 B2 到 B[dataTable.Rows.Count + 1]
            // 添加柱狀圖的系列（假設數據在 B 列）
            var columnSerise = columnchart.Series.Add(
                DataWorksheet.Cells[$"I2:I{dataTable.Rows.Count + 1}"],//Y軸數據
                DataWorksheet.Cells["C2:C" + (dataTable.Rows.Count + 1)]);//X軸名稱

            columnchart.SetPosition(1, 0, 3, 0);
            columnchart.SetSize(2330, 600);

            //折線圖工作表
            //創建折線圖
            //var lineChartWorksheet = package.Workbook.Worksheets.Add("Line Chart");
            //var lineChart = lineChartWorksheet.Drawings.AddChart("StockLineChart", eChartType.Line);
            //lineChart.Title.Text = "股票分析折線圖";

            ////添加折線圖系列(假設數據在C列)
            //var lineSeries = lineChart.Series.Add(DataWorksheet.Cells[$"C2:C{dataTable.Rows.Count + 1}"],
            //    DataWorksheet.Cells["C2:C" + (dataTable.Rows.Count + 1)]);

            //lineChart.SetPosition(1, 0, 3, 0);
            //lineChart.SetSize(2330, 600);

        }
    }
}
