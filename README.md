專案概述
本專案旨在從網站抓取股票相關數據，對數據進行解析和處理，最終將數據存儲至數據庫並輸出為帶有圖表的 Excel 報告。

主要功能與類別Parser 類
功能: 從網站抓取股票數據，解析後存入對象，並存儲至數據庫。
主要方法:ParseHtmlAsync(): 從網站抓取股票數據。
ParseDetailAsync(): 解析股票詳細數據頁面。
InsertStockDetail(): 將股票詳細數據存儲至數據庫。
DLL_SQL 類
功能: 負責與 SQL Server 數據庫的交互。
主要方法:NonQuerySQL(): 執行非查詢 SQL 指令。
QuerySQL(): 執行查詢 SQL 指令並返回結果。
OutPutExcel 類
功能: 從數據庫讀取數據並將其輸出為 Excel 文件。
主要方法:CreateExcel(): 創建並儲存 Excel 文件。
CreateExcelFileWithChart(): 在 Excel 中創建帶有圖表的工作表。
StockDbContext 類
功能: 作為 Entity Framework 的數據庫上下文，用於 ORM 映射。
屬性:StockDatas: 映射股票數據表。
StockDetails: 映射股票詳細數據表。
Form1 類
功能: 應用程序的主界面，用於觸發和管理數據解析和 Excel 報告生成過程。
主要方法:ParseDataAsync(): 啟動數據解析和 Excel 文件生成。
使用的第三方庫和框架
AngleSharp: 用於 HTML 內容解析和 DOM 操作。
EntityFramework: 用於 ORM 映射和數據庫操作。
EPPlus: 用於創建和操作 Excel 文件。
開發環境
.NET Framework
Visual Studio
