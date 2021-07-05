using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace ShadaniEnterprises.Models
{
    public class DataConnection
    {
        String Message = null;
        //Sql DB Execution
        public string ExecuteSqlQuery(string ProdDataSource, string ProdInitial_Catalog, string ProdDbUser, string ProdDbPassword, string query)
        { 
            
                Console.WriteLine("\r\n" + "---------- EXECUTING QUERY ----------" + Environment.NewLine + query + Environment.NewLine + " at Datebase SID:" + ProdDataSource + " at Datebase Name:" + ProdInitial_Catalog + " at Datebase UserID:" + ProdDbUser + " at Datebase Password:" + ProdDbPassword);
                SqlConnection Sql_con = new SqlConnection(GenerateSqlConnectionString(ProdDataSource, ProdInitial_Catalog, ProdDbUser, ProdDbPassword));
                Sql_con.Open();
                SqlCommand Sql_command = Sql_con.CreateCommand();
                Sql_command.CommandText = query;
                Sql_command.ExecuteNonQuery();
                Sql_con.Close();
                return Message = "Query Executed Successfully";
          

           
        }

        //Sql DB connection string
        private string GenerateSqlConnectionString(string ProdDataSource, string ProdInitial_Catalog, string ProdDbUser, string ProdDbPassword)
        {
            //return "Data Source=" + ProdDataSource + "; User Id=" + ProdDbUser + "; Password =" + ProdDbPassword + ";";
            return @"Server=" + ProdDataSource + ";Database=" + ProdInitial_Catalog + ";User Id=" + ProdDbUser + ";Password=" + ProdDbPassword + ";";
        }

        //Sql DB /
        public List<string> ConnectToData(string ProdDataSource, string ProdInitial_Catalog, string ProdDbUser, string ProdDbPassword, string query)
        {
            Console.WriteLine("\r\n" + "---------- EXECUTING QUERY ----------" + Environment.NewLine + query + Environment.NewLine + " at Datebase SID:" + ProdDataSource + " at Datebase Name:" + ProdInitial_Catalog + " at Datebase UserID:" + ProdDbUser + " at Datebase Password:" + ProdDbPassword);
            string connString = GenerateSqlConnectionString(ProdDataSource, ProdInitial_Catalog, ProdDbUser, ProdDbPassword);

            DataSet ds = new DataSet();
            SqlConnection Sql_con = new SqlConnection(connString);
            SqlCommand Sql_command = Sql_con.CreateCommand();
            Sql_command.CommandText = query;
            SqlDataAdapter da = new SqlDataAdapter(Sql_command);
            da.Fill(ds);

            List<string> result = new List<string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    result.Add(ds.Tables[0].Rows[i][j].ToString());
                }
            }
            Sql_con.Close();
            Console.WriteLine("\r\n" + "QUERY RESULTS: " + result + "\r\n");
            return result;

        }

        public SqlDataReader GetDataFromDatabase(string DBHost, string DataSource, string dbuserid, string dbpassword, string query)
        {
            Console.WriteLine("Executing Query:" + Environment.NewLine + query + " at Database SID:" + DataSource + " userid:" + dbuserid + " password:" + dbpassword);
            string connString = GenerateSqlConnectionString(DBHost, DataSource, dbuserid, dbpassword);
            //DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            SqlDataReader dr = cmd.ExecuteReader();
            // OracleDataAdapter da = new OracleDataAdapter(cmd);
            // da.Fill(ds);
            return dr;
        }



        
        public DataSet GetDataFromDatabaseDataSet(string DBHost, string DataSource, string dbuserid, string dbpassword, string query)
        {
            Console.WriteLine("Executing Query:" + Environment.NewLine + query + " at Database SID:" + DataSource + " userid:" + dbuserid + " password:" + dbpassword);
            string connString = GenerateSqlConnectionString(DBHost, DataSource, dbuserid, dbpassword);
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            // OracleDataReader dr = cmd.ExecuteReader();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();
            return ds;
        }



        public string ExecuteScalar(string ProdDataSource, string ProdInitial_Catalog, string ProdDbUser, string ProdDbPassword, string query)
        {
            Console.WriteLine("\r\n" + "---------- EXECUTING QUERY ----------" + Environment.NewLine + query + Environment.NewLine + " at Datebase SID:" + ProdDataSource + " at Datebase Name:" + ProdInitial_Catalog + " at Datebase UserID:" + ProdDbUser + " at Datebase Password:" + ProdDbPassword);
            string connString = GenerateSqlConnectionString(ProdDataSource, ProdInitial_Catalog, ProdDbUser, ProdDbPassword);
            DataSet ds = new DataSet();
            SqlConnection Sql_con = new SqlConnection(connString);
            Sql_con.Open();
            SqlCommand Sql_command = Sql_con.CreateCommand();
            Sql_command.CommandText = query;
            string NullChecking = Sql_command.ExecuteScalar() as string;
            Sql_con.Close();

            if (NullChecking == null) { NullChecking = null; }
            Console.WriteLine("Result :" + NullChecking);
            return NullChecking;

        }

        public void RefreshSqlMV(string ProdDataSource, string ProdInitial_Catalog, string ProdDbUser, string ProdDbPassword, string query)
        {
            string connString = GenerateSqlConnectionString(ProdDataSource, ProdInitial_Catalog, ProdDbUser, ProdDbPassword);
            SqlConnection Sql_con = new SqlConnection(connString);

            Sql_con.Open();
            SqlCommand Sql_command = Sql_con.CreateCommand();
            Sql_command.CommandText = query;
            Sql_command.ExecuteNonQuery();
            Sql_con.Close();
        }

        /* ORACLE 
        public void ExecuteQuery(string DataSource, string dbuserid, string dbpassword, string query)
        {
            OracleConnection conn = new OracleConnection(GenerateConnectionString(DataSource, dbuserid, dbpassword));
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private string GenerateConnectionString(string DataSource, string dbuserid, string dbpassword)
        {
            return "Data Source=" + DataSource + "; User Id=" + dbuserid + "; Password =" + dbpassword + ";";
        }

        public List<string> ConnectToData(string DataSource, string dbuserid, string dbpassword, string query)
        {
            Console.WriteLine("Executing Query:" + Environment.NewLine + query + " at Database SID:" + DataSource + " at USERID:" + dbuserid + " at Database Password:" + dbpassword);
            string connString = GenerateConnectionString(DataSource, dbuserid, dbpassword);
            DataSet ds = new DataSet();
            OracleConnection conn = new OracleConnection(connString);
            OracleCommand cmd = conn.CreateCommand();

            cmd.CommandText = query;

            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(ds);

            List<string> result = new List<string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    result.Add(ds.Tables[0].Rows[i][j].ToString());
                }
            }
            conn.Close();
            Console.WriteLine("the output is:" +result);
            return result;

        }

        public void RefreshMV(string DataSource, string dbuserid, string dbpassword, string query)
        {
            string connString = GenerateConnectionString(DataSource, dbuserid, dbpassword);
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        } */



        //////////////// ORACLE CONNECTION 
        //static protected string GetConnectionString(string dbHost, string dbPort, string dbsid, string dbuserid, string dbpassword)
        //{
        //    string host = dbHost;
        //    string sid = dbsid;
        //    string user = dbuserid;
        //    string pass = dbpassword;
        //    string port = dbPort;
        //    string sConStr = "";
        //    // sConStr = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + ip + ")(PORT=1521)))(CONNECT_DATA=(SID=" + sid + ")(SERVER=DEDICATED)));user id=" + user + ";pwd=" + pass + ";";


        //    //  sConStr = "Data Source=" + dbHost + ":" + dbPort + "/" + dbsid + ";User Id=" + dbuserid + ";Password=" + dbpassword + ";";
        //    sConStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + dbHost + ")(PORT=" + dbPort + "))(CONNECT_DATA=(SERVICE_NAME=" + dbsid + ")));User Id=" + dbuserid + ";Password=" + dbpassword + "";
        //    Console.WriteLine("This is the connection string: " + sConStr);
        //    return sConStr;

        //}

 

        ////////////////
    }

}
