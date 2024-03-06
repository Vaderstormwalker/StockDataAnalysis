using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sideproject1.DLL
{
    public class DLL_SQL
    {
        public DataTable NoQuerySQL(string sql)
        {
            DataTable dt = new DataTable();
            //string connectionString = "";
            SqlConnection con = new SqlConnection(@"Data Source = ; Initial Catalog =; User=your user name;Password=******");

            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();

            //SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            //adpt.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable QuerySQL(string sql)
        {
            DataTable dt = new DataTable();
            //string connectionString = "";
            SqlConnection con = new SqlConnection(@"Data Source = ; Initial Catalog = ; User= your user name;Password=*******");

            con.Open();
            //SqlCommand command = new SqlCommand(sql, con);
            //command.ExecuteReader();

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            adpt.Fill(dt);
            con.Close();
            return dt;
        }
    }
}
