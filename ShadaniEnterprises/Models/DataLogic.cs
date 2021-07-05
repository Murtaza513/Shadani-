using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class DataLogic
    {


        private string SEDataSource = ConfigurationManager.AppSettings["SEDataSource"].ToString();
        private string SEInitialCatolog = ConfigurationManager.AppSettings["SEInitialCatolog"].ToString();
        private string SEUser = ConfigurationManager.AppSettings["SEUser"].ToString();
        private string SEPassword = ConfigurationManager.AppSettings["SEPassword"].ToString();
        DataConnection Dbobj = new DataConnection();
        List<string> ErrorMessageLogin = new List<string>();
        public List<string> ErrorMessage;
      
        public List<string> loginValidation(string username, string pass)
        {

            try
            {
                
                List<string> UsersList = new List<string>();

                string query = "select user_name, Password, USER_NAME, tbl_roles.TBL_ROLES_ID , tbl_roles.ROLE_NAME from Tbl_users INNER JOIN tbl_roles ON tbl_roles.TBL_ROLES_ID = Tbl_users.TBL_ROLES_ID where TBL_USERS.User_name = '" + username + "' and TBL_USERS.PASSWORD = '" + pass + "'";

                List<string> valid = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, query);

                if (valid[0] == username && valid[1] == pass) { UsersList.Add(valid[0]); UsersList.Add(valid[2]); UsersList.Add(valid[3]); UsersList.Add(valid[4]); return UsersList; }


                else
                {
                    ErrorMessageLogin.Add("Invalid user name or password");

                    return ErrorMessageLogin;
                }

            }

            catch (Exception)
            {

                return ErrorMessageLogin;

            }
            
        }

        public List<Purchases> FetchPurchasesAmountTotal(string startDate, string endDate, string type)
        {
            try
            {

                string columns = "SUM(CAST(Amount as decimal(10,2))) as totalAmount";

                string joins = "inner join tbl_purchases_overview on tbl_purchases.tbl_purchases_overview_id = tbl_purchases_overview.tbl_purchases_overview_id inner join Tbl_Product on TBL_Purchases.tbl_product_id = tbl_product.tbl_product_id inner join tbl_company on tbl_product.tbl_company_id = tbl_company.tbl_company_id";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23)";

                string QueryUser = "select " + columns + " from TBL_Purchases " + joins + " where " + whereParam;
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                var Companies = new List<Purchases>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.totalAmount = Convert.ToString(dr["totalAmount"]);
                        

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        //public DataTable FetchPurchasesAmountTotal(string startDate, string endDate, string type)
        //{
        //    try
        //    {
        //        string columns = "SUM(CAST(Amount as decimal(10,2))) as totalAmount";
                
        //        string joins = "inner join tbl_purchases_overview on tbl_purchases.tbl_purchases_overview_id = tbl_purchases_overview.tbl_purchases_overview_id inner join Tbl_Product on TBL_Purchases.tbl_product_id = tbl_product.tbl_product_id inner join tbl_company on tbl_product.tbl_company_id = tbl_company.tbl_company_id";
        //        string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23)";

        //        string QueryUser = "select " + columns + " from TBL_Purchases " + joins + " where " + whereParam;
        //        DataTable dt = new DataTable();
     
        //        DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                
        //        foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
        //        {
        //            dt = datTab.Clone();
        //            foreach (DataRow datRow in datTab.Rows)
        //            {

        //                if (datRow.IsNull(0)) //if empty first col go on to next sheet
        //                {
        //                    break;
        //                }
        //                else
        //                {
        //                    dt.ImportRow(datRow);
        //                    //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
        //                }
        //            }

        //        }
        //        //var row = dt.NewRow();
        //        //row["net_weight"] = sum;
        //        //dt.Rows.Add(row);

        //        return dt;
        //    }

        //    catch (Exception ex)
        //    {
        //        ErrorMessage.Add(ex.ToString() + Environment.NewLine);
        //        return null;
        //    }

        //}
        public DataTable FetchPurchasesDataForReport(string startDate, string endDate, string type)
        {
            try
            {
                string columns = "InvoiceNum,InvoiceDate,CompanyName,Brand_Name,Generic_Name,AcUnit,Pack,PurchaseQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Company Name,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join tbl_purchases_overview on tbl_purchases.tbl_purchases_overview_id = tbl_purchases_overview.tbl_purchases_overview_id inner join Tbl_Product on TBL_Purchases.tbl_product_id = tbl_product.tbl_product_id inner join tbl_company on tbl_product.tbl_company_id = tbl_company.tbl_company_id";
                string whereParam = "InvoiceDate Between convert(varchar, '"+ startDate + "', 23) And convert(varchar, '"+endDate+"', 23) ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Purchases " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchPurchasesDataForReportBatch(string startDate, string endDate, string type, string bachNum)
        {
            try
            {
                string columns = "InvoiceNum,InvoiceDate,CompanyName,Brand_Name,Generic_Name,AcUnit,Pack,PurchaseQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Company Name,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join tbl_purchases_overview on tbl_purchases.tbl_purchases_overview_id = tbl_purchases_overview.tbl_purchases_overview_id inner join Tbl_Product on TBL_Purchases.tbl_product_id = tbl_product.tbl_product_id inner join tbl_company on tbl_product.tbl_company_id = tbl_company.tbl_company_id";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) and Batch = '"+bachNum+"' ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Purchases " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchPurchasesDataForReportProduct(string startDate, string endDate, string type, string productName)
        {
            try
            {
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + productName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                string columns = "InvoiceNum,InvoiceDate,CompanyName,Brand_Name,Generic_Name,AcUnit,Pack,PurchaseQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Company Name,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join tbl_purchases_overview on tbl_purchases.tbl_purchases_overview_id = tbl_purchases_overview.tbl_purchases_overview_id inner join Tbl_Product on TBL_Purchases.tbl_product_id = tbl_product.tbl_product_id inner join tbl_company on tbl_product.tbl_company_id = tbl_company.tbl_company_id";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) and TBL_Product.TBL_Product_ID = '" + ProductID[0] + "' ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Purchases " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchPurchasesDataForReportCompany(string startDate, string endDate, string type, string CompanyName)
        {
            try
            {
                
                string columns = "InvoiceNum,InvoiceDate,CompanyName,Brand_Name,Generic_Name,AcUnit,Pack,PurchaseQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Company Name,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join tbl_purchases_overview on tbl_purchases.tbl_purchases_overview_id = tbl_purchases_overview.tbl_purchases_overview_id inner join Tbl_Product on TBL_Purchases.tbl_product_id = tbl_product.tbl_product_id inner join tbl_company on tbl_product.tbl_company_id = tbl_company.tbl_company_id";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) and CompanyName = '" + CompanyName + "' ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Purchases " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchSellDataForReport(string startDate, string endDate, string type)
        {
            try
            {
                string columns = "InvoiceNum,InvoiceDate,OrderDate,CustOrderNum,CustomerName,Address1,Brand_Name,Generic_Name,AcUnit,Pack,InvoiceQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Order Date,Customer Order Num,Customer Name,Address,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Invoice_Bills " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchSellDataForReportBatch(string startDate, string endDate, string type, string batchNum)
        {
            try
            {
                string columns = "InvoiceNum,InvoiceDate,OrderDate,CustOrderNum,CustomerName,Address1,Brand_Name,Generic_Name,AcUnit,Pack,InvoiceQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Order Date,Customer Order Num,Customer Name,Address,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) and Batch = '"+batchNum+"' ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Invoice_Bills " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchSellDataForReportProduct(string startDate, string endDate, string type, string productName)
        {
            try
            {
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + productName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                string columns = "InvoiceNum,InvoiceDate,OrderDate,CustOrderNum,CustomerName,Address1,Brand_Name,Generic_Name,AcUnit,Pack,InvoiceQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Order Date,Customer Order Num,Customer Name,Address,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) and TBL_Product.TBL_Product_ID = '" + ProductID[0] + "' ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Invoice_Bills " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public DataTable FetchSellDataForReportCustomer(string startDate, string endDate, string type, string customerName)
        {
            try
            {
                string[] custID = customerName.Split('.');
                string custIDtrim = custID[0].Trim();
                //String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + productName + "'";
                //List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                string columns = "InvoiceNum,InvoiceDate,OrderDate,CustOrderNum,CustomerName,Address1,Brand_Name,Generic_Name,AcUnit,Pack,InvoiceQty,PurchaseRate,Batch,MFG,EXP,Amount";
                string columnsPrint = "Invoice Num,Invoice Date,Order Date,Customer Order Num,Customer Name,Address,Product Name,Generic Name,Unit,Pack,Quantity,Rate,Batch,MFG,EXP,Amount";
                string joins = "inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID";
                string whereParam = "InvoiceDate Between convert(varchar, '" + startDate + "', 23) And convert(varchar, '" + endDate + "', 23) and tbl_customer.TBL_Customer_ID = '" + custIDtrim + "' ORDER by InvoiceDate DESC";

                string QueryUser = "select " + columns + " from TBL_Invoice_Bills " + joins + " where " + whereParam;
                DataTable dt = new DataTable();
                foreach (string column in columnsPrint.Split(','))
                {
                    dt.Columns.Add(column, typeof(string));
                }
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                Int64 sum = 0;
                foreach (DataTable datTab in UserFetch.Tables) // ds is extracted excel sheets in a dataset
                {
                    dt = datTab.Clone();
                    foreach (DataRow datRow in datTab.Rows)
                    {

                        if (datRow.IsNull(0)) //if empty first col go on to next sheet
                        {
                            break;
                        }
                        else
                        {
                            dt.ImportRow(datRow);
                            //sum = sum + (Convert.ToInt64(datRow["net_weight"]));
                        }
                    }

                }
                //var row = dt.NewRow();
                //row["net_weight"] = sum;
                //dt.Rows.Add(row);

                return dt;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchAllRefFromPurchases()
        {
            try
            {
                // Fetching all Refs whose Return is not filled
                string QueryComapny = "select CAST(TBL_Purchases_Overview.Ref as int) as RefInt from TBL_Purchases_Overview left join tbl_purchase_return_overview on TBL_Purchases_Overview.TBL_Purchases_Overview_id = tbl_purchase_return_overview.TBL_Purchases_Overview_id where tbl_purchase_return_overview.TBL_Purchases_Overview_id IS Null ORDER by RefInt DESC";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<Purchases>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.Ref = Convert.ToString(dr["RefInt"]);
                        

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<InvoiceBills> FetchAllInvoiceForReturn()
        {
            try
            {

                string QueryComapny = "select TBL_Invoice_Bills_Overview.InvoiceNum from TBL_Invoice_Bills_Overview left join TBL_Sell_Return_Overview on TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id = TBL_Sell_Return_Overview.TBL_Invoice_Bills_Overview_id where TBL_Sell_Return_Overview.TBL_Invoice_Bills_Overview_id IS Null";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<PurchaseOrder> FetchAllRefFromPUrchaseOrders()
        {
            try
            {

                string QueryComapny = "Select CAST(Ref as int) as RefInt, TBL_Purchase_Order_Overview_id from TBL_Purchase_Order_Overview ORDER by RefInt DESC";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<PurchaseOrder>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();
                        InstituteForAdminView.RefForVIew = Convert.ToString(dr["RefInt"]);
                        InstituteForAdminView.RefID = Convert.ToString(dr["TBL_Purchase_Order_Overview_id"]);

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<PurchaseOrder> FetchAllRef()
        {
            try
            {

                string QueryComapny = "Select CAST(Ref as int) as RefInt, TBL_Purchase_Order_Overview_id from TBL_Purchase_Order_Overview where Ref not in (Select ref from TBL_Purchases_Overview) ORDER by RefInt DESC";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<PurchaseOrder>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();
                        InstituteForAdminView.RefForVIew = Convert.ToString(dr["RefInt"]);
                        InstituteForAdminView.RefID = Convert.ToString(dr["TBL_Purchase_Order_Overview_id"]);

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }


        public List<InvoiceBills> FetchAllCustomers()
        {
            try
            {

                string QueryComapny = "select TBL_Customer_ID , CustomerName, Address1, Address2 from TBL_Customer";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.CustomeID = Convert.ToString(dr["TBL_Customer_ID"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }
        public List<PurchaseOrder> FetchCompanyBasedOnRef(string Company)
        {
            try
            {

                string Query = "Select DISTINCT CompanyName from TBL_Purchase_Order_Overview inner join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID  where Ref = '" + Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<PurchaseOrder>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();

                        InstituteForAdminView.CNameForVIew = Convert.ToString(dr["CompanyName"]);
                     
                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<Purchases> FetchCompanyBasedOnPurchasesRef(string Company)
        {
            try
            {

                string Query = "Select DISTINCT CompanyName from TBL_Purchases_Overview inner join TBL_Company on TBL_Purchases_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID  where Ref = '" + Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<Purchases>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();

                        InstituteForAdminView.Company = Convert.ToString(dr["CompanyName"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<DeliveryChallan> FetchDeliveryChallanDetails(string Company)
        {
            try
            {
                
                string Query = "Select DISTINCT OrderDate, InvoiceNum from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID where OrderNumRef ='" + Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<DeliveryChallan>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new DeliveryChallan();

                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        
                        Prods.Add(InstituteForAdminView);


                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<DeliveryChallan> FetchDeliveryDataBasedOnRef(string Company)
        {
            try
            {
                
                string Query = "Select DISTINCT Brand_Name from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID where OrderNumRef ='" + Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<DeliveryChallan>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new DeliveryChallan();

                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                       
                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<InvoiceBills> FetchInvoiceRef(string Company)
        {
            try
            {

                string Query = "select DISTINCT OrderNumRef from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id where InvoiceNum = '" + Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.Ref = Convert.ToString(dr["OrderNumRef"]);
                        


                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }
        public List<InvoiceBills> FetchInvoiceDataBasedOnInv(string Company, string Ref)
        {
            try
            {

                string Query = "select Brand_Name, Batch from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id where InvoiceNum = '" + Company + "' AND OrderNumRef = '"+Ref+"'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<InvoiceBills> FetchInvoiceDataBasedOnRef(string Company)
        {
            try
            {

                string Query = "Select DISTINCT Brand_Name, Batch from TBL_Purchases inner join TBL_Product on TBL_Purchases.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchases_Overview on TBL_Purchases.TBL_Purchases_Overview_id = TBL_Purchases_Overview.TBL_Purchases_Overview_id where Ref = '" + Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);

                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<Purchases> FetchProductsDataBasedOnPurchasesCompany(string Company, string Ref)
        {
            try
            {

                string Query = "Select DISTINCT Brand_Name from TBL_Purchases inner join TBL_Product on TBL_Purchases.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchases_Overview on TBL_Purchases.TBL_Purchases_Overview_id = TBL_Purchases_Overview.TBL_Purchases_Overview_id join TBL_Company on TBL_Purchases_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where CompanyName = '" + Company + "' AND Ref =  '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<Purchases>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();

                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<PurchaseOrder> FetchProductsDataBasedOnCompany(string Company, string Ref)
        {
            try
            {
                
                string Query = "Select DISTINCT Brand_Name from TBL_Purchase_Order inner join TBL_Product on TBL_Purchase_Order.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchase_Order_Overview on TBL_Purchase_Order.TBL_Purchase_Order_Overview_id = TBL_Purchase_Order_Overview.TBL_Purchase_Order_Overview_id join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where CompanyName = '" + Company + "' AND Ref =  '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<PurchaseOrder>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();

                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        
                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchProductsDataOfPurchases(string Company, string Ref)
        {
            try
            {

                string Query = "Select DISTINCT Brand_Name from TBL_Purchase_Order inner join TBL_Product on TBL_Purchase_Order.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchase_Order_Overview on TBL_Purchase_Order.TBL_Purchase_Order_Overview_id = TBL_Purchase_Order_Overview.TBL_Purchase_Order_Overview_id join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where CompanyName = '" + Company + "' AND Ref =  '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<PurchaseOrder>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();

                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<PurchaseOrder> FetchProductsDetailsForPurchases(string ProductName, string Ref)
        {
            try
            {

                string Query = "Select Pack, Qty, RatesAfterDisc, Value from TBL_Purchase_Order inner join TBL_Product on TBL_Purchase_Order.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchase_Order_Overview on TBL_Purchase_Order.TBL_Purchase_Order_Overview_id = TBL_Purchase_Order_Overview.TBL_Purchase_Order_Overview_id where Brand_Name =  '" + ProductName + "' AND Ref = '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<PurchaseOrder>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();

                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Qty"]);
                        InstituteForAdminView.Trade_Price = Convert.ToString(dr["RatesAfterDisc"]);
                        InstituteForAdminView.ProductValue = Convert.ToString(dr["Value"]);
                        
                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchProductsDetailsFromPurchaseReturn(string ProductName, string Ref)
        {
            try
            {

                string Query = "select InvoiceNum, InvoiceDate, Pack, AcUnit, PurchaseQty, Batch, PurchaseRate, MFG,EXP,Amount from TBL_Purchase_Return inner join TBL_Purchase_Return_Overview on TBL_Purchase_Return.TBL_Purchase_Return_Overview_id = TBL_Purchase_Return_Overview.TBL_Purchase_Return_Overview_id inner join TBL_Product on TBL_Purchase_Return.TBL_Product_ID = TBL_Product.TBL_Product_ID where Brand_Name =  '" + ProductName + "' AND Ref = '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<Purchases>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();

                        InstituteForAdminView.InvoiceNum = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.PackVal = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.prodQty = Convert.ToString(dr["PurchaseQty"]);
                        InstituteForAdminView.prodBatch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.purchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.prodMFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.prodEXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.amount = Convert.ToString(dr["Amount"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchProductsDetailsFromPurchases(string ProductName, string Ref)
        {
            try
            {

                string Query = "select InvoiceNum, InvoiceDate, Pack, AcUnit, PurchaseQty, Batch, PurchaseRate, MFG,EXP,Amount from TBL_Purchases inner join TBL_Purchases_Overview on TBL_Purchases.TBL_Purchases_Overview_id = TBL_Purchases_Overview.TBL_Purchases_Overview_id inner join TBL_Product on TBL_Purchases.TBL_Product_ID = TBL_Product.TBL_Product_ID where Brand_Name =  '" + ProductName + "' AND Ref = '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<Purchases>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();

                        InstituteForAdminView.InvoiceNum = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.PackVal = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.prodQty = Convert.ToString(dr["PurchaseQty"]);
                        InstituteForAdminView.prodBatch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.purchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.prodMFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.prodEXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.amount = Convert.ToString(dr["Amount"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<DeliveryChallan> FetchProductsDetailsForDelivery(string ProductName, string Ref)
        {
            try
            {
                
                string Query = "Select Pack, AcUnit, InvoiceQty, Batch, MFG, EXP, TenderCode, ProductRegNum from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID where Brand_Name = '" + ProductName + "' AND OrderNumRef = '" + Ref + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<DeliveryChallan>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new DeliveryChallan();

                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["InvoiceQty"]);
                        InstituteForAdminView.TenderCode = Convert.ToString(dr["TenderCode"]);
                        InstituteForAdminView.ProdRegNum = Convert.ToString(dr["ProductRegNum"]);

                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<InvoiceBills> FetchDetailsForInvoices(string invNumber)
        {
            try
            {
               
                string Query = "select TBL_Customer.TBL_Customer_ID,CustomerName, Address1,Address2, CustOrderNum, InvoiceDate, OrderDate, InvoiceType, Warranty, AdvTax from TBL_Invoice_Bills_Overview inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID where InvoiceNum = '" + invNumber + "' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.CustomeID = Convert.ToString(dr["TBL_Customer_ID"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);
                        InstituteForAdminView.CustOrderNum = Convert.ToString(dr["CustOrderNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.InvoiceType = Convert.ToString(dr["InvoiceType"]);
                        InstituteForAdminView.Warranty = Convert.ToString(dr["Warranty"]);
                        InstituteForAdminView.AdvTax = Convert.ToString(dr["AdvTax"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<InvoiceBills> FetchProductsDetailsForInvoicesWithInvNum(string ProductName, string InvoiceNum, string Ref)
        {
            try
            {

                string[] productName = ProductName.Split('~');
                string prodnameTrim = productName[0].Trim();

                string[] productBatch = ProductName.Split('~');
                string prodbatchTrim = productBatch[1].Trim();

                string Query = "select TenderCode,ProductRegNum,Pack,AcUnit,InvoiceQty,Batch,MFG,EXP,PurchaseRate,Amount from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id where Brand_Name = '" + prodnameTrim + "' AND InvoiceNum = '" + InvoiceNum + "' AND Batch = '" + prodbatchTrim + "' AND OrderNumRef = '"+Ref+"' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.TenderCode = Convert.ToString(dr["TenderCode"]);
                        InstituteForAdminView.ProdRegNum = Convert.ToString(dr["ProductRegNum"]);
                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["InvoiceQty"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.PurchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.Amout = Convert.ToString(dr["Amount"]);

                        Prods.Add(InstituteForAdminView);

                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<InvoiceBills> FetchProductsDetailsForInvoices(string ProductName, string Ref)
        {
            try
            {

                string[] productName = ProductName.Split('~');
                string prodnameTrim = productName[0].Trim();

                string[] productBatch = ProductName.Split('~');
                string prodbatchTrim = productBatch[1].Trim();

                string Query = "Select Pack, Drug_Registration_Number, AcUnit, PurchaseQty, Batch, MFG, EXP, PurchaseRate, Amount from TBL_Purchases inner join TBL_Product on TBL_Purchases.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchases_Overview on TBL_Purchases.TBL_Purchases_Overview_id = TBL_Purchases_Overview.TBL_Purchases_Overview_id where Brand_Name = '" + prodnameTrim + "' AND Ref = '" + Ref + "' AND Batch = '"+prodbatchTrim+"' ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<InvoiceBills>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.drugProdregNum = Convert.ToString(dr["Drug_Registration_Number"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.PurchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.Amout = Convert.ToString(dr["Amount"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["PurchaseQty"]);

                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<ProductViewModel> FetchProducts()
        {
            try
            {
                
                string QueryComapny = "select TBL_Product_ID , Generic_Name, Brand_Name, Dosage_Form, Strength, Drug_Registration_Number, Pack_Size from TBL_Product ORDER by Brand_Name ASC";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<ProductViewModel>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ProductViewModel();
                        InstituteForAdminView.TBL_Product_ID = Convert.ToString(dr["TBL_Product_ID"]);
                        InstituteForAdminView.Generic_Name = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Dosage_Form = Convert.ToString(dr["Dosage_Form"]);
                        InstituteForAdminView.Strength = Convert.ToString(dr["Strength"]);
                        InstituteForAdminView.Drug_Registration_Number = Convert.ToString(dr["Drug_Registration_Number"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack_Size"]);
                      
                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<ComapnyViewModel> FetchCompanyData()
        {
            try
            {
                
                string QueryComapny = "select TBL_Company_ID , CompanyName, Country, Person1, Person2, Phone1, Phone2, Email1, Email2, PaymentTerms, Address1, Address2 from TBL_Company order by TBL_Company_ID DESC";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var Companies = new List<ComapnyViewModel>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ComapnyViewModel();
                        InstituteForAdminView.TBL_Comapny_ID = Convert.ToString(dr["TBL_Company_ID"]);
                        InstituteForAdminView.Company_Name = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.Country = Convert.ToString(dr["Country"]);
                        InstituteForAdminView.Person1 = Convert.ToString(dr["Person1"]);
                        InstituteForAdminView.Person2 = Convert.ToString(dr["Person2"]);
                        InstituteForAdminView.Phone1 = Convert.ToString(dr["Phone1"]);
                        InstituteForAdminView.Phone2 = Convert.ToString(dr["Phone2"]);
                        InstituteForAdminView.Email1 = Convert.ToString(dr["Email1"]);
                        InstituteForAdminView.Email2 = Convert.ToString(dr["Email2"]);
                        InstituteForAdminView.Pay_Terms = Convert.ToString(dr["PaymentTerms"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);

                        Companies.Add(InstituteForAdminView);
                    }
                }

                return Companies;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<ProductViewModel> FetchProductsBasedOnCompany(string Company)
        {
            try
            {

                string Query = "Select TBL_Product.TBL_Product_ID, TBL_Product.Generic_Name, TBL_Product.Brand_Name, TBL_Product.Dosage_Form,TBL_Product.Strength,TBL_Product.Drug_Registration_Number,TBL_Product.Pack_Size,TBL_Product.Other_Mention_Here,TBL_Product.Stock,TBL_Product.Stax,TBL_Product.Scheme_Quantity,TBL_Product.Bonus,TBL_Product.Trade_Price,TBL_Product.MRP, TBL_Medicines_Groups.Group_Name,TBL_Company.CompanyName from TBL_Product join TBL_Medicines_Groups on TBL_Product.TBL_Medicines_Groups_ID = TBL_Medicines_Groups.TBL_Medicines_Groups_ID join TBL_Company on TBL_Product.TBL_Company_ID = TBL_Company.TBL_Company_ID where TBL_Company.CompanyName = '"+ Company + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<ProductViewModel>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ProductViewModel();
                        InstituteForAdminView.TBL_Product_ID = Convert.ToString(dr["TBL_Product_ID"]);
                        InstituteForAdminView.Generic_Name = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Dosage_Form = Convert.ToString(dr["Dosage_Form"]);
                        InstituteForAdminView.Strength = Convert.ToString(dr["Strength"]);
                        InstituteForAdminView.Drug_Registration_Number = Convert.ToString(dr["Drug_Registration_Number"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack_Size"]);
                        InstituteForAdminView.Other_Mention_Here = Convert.ToString(dr["Other_Mention_Here"]);
                        InstituteForAdminView.Stax = Convert.ToString(dr["Stax"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Scheme_Quantity"]);
                        InstituteForAdminView.Bonus = Convert.ToString(dr["Bonus"]);
                        InstituteForAdminView.Trade_Price = Convert.ToString(dr["Trade_Price"]);
                        InstituteForAdminView.MRP = Convert.ToString(dr["MRP"]);
                        InstituteForAdminView.GroupName = Convert.ToString(dr["Group_Name"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);

                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<ProductViewModel> FetchProductsDetails(string ProductName)
        {
            try
            {
                
                string Query = "Select TBL_Product.TBL_Product_ID, TBL_Product.Generic_Name, TBL_Product.Brand_Name, TBL_Product.Dosage_Form,TBL_Product.Strength,TBL_Product.Drug_Registration_Number,TBL_Product.Pack_Size,TBL_Product.Other_Mention_Here,TBL_Product.Stock,TBL_Product.Stax,TBL_Product.Scheme_Quantity,TBL_Product.Bonus,TBL_Product.Trade_Price,TBL_Product.MRP, TBL_Medicines_Groups.Group_Name, TBL_Company.CompanyName from TBL_Product join TBL_Medicines_Groups on TBL_Product.TBL_Medicines_Groups_ID = TBL_Medicines_Groups.TBL_Medicines_Groups_ID join TBL_Company on TBL_Product.TBL_Company_ID = TBL_Company.TBL_Company_ID where TBL_Product.Brand_Name = '" + ProductName + "'";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, Query);
                var Prods = new List<ProductViewModel>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ProductViewModel();
                        InstituteForAdminView.TBL_Product_ID = Convert.ToString(dr["TBL_Product_ID"]);
                        InstituteForAdminView.Generic_Name = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Dosage_Form = Convert.ToString(dr["Dosage_Form"]);
                        InstituteForAdminView.Strength = Convert.ToString(dr["Strength"]);
                        InstituteForAdminView.Drug_Registration_Number = Convert.ToString(dr["Drug_Registration_Number"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack_Size"]);
                        InstituteForAdminView.Other_Mention_Here = Convert.ToString(dr["Other_Mention_Here"]);
                        InstituteForAdminView.Stax = Convert.ToString(dr["Stax"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Scheme_Quantity"]);
                        InstituteForAdminView.Bonus = Convert.ToString(dr["Bonus"]);
                        InstituteForAdminView.Trade_Price = Convert.ToString(dr["Trade_Price"]);
                        InstituteForAdminView.MRP = Convert.ToString(dr["MRP"]);
                        InstituteForAdminView.GroupName = Convert.ToString(dr["Group_Name"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);

                        Prods.Add(InstituteForAdminView);
                        
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }
        
        public List<PurchaseOrder> FetchGetREF()
        {
            try
            {

                var InstituteForAdminView = new PurchaseOrder();
                var Prods = new List<PurchaseOrder>();
                string QueryREF = "select max(cast(Ref as int)) from TBL_Purchase_Order_Overview";
                List<string> REFFetch = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryREF);

                if (REFFetch[0] == "")
                {
                    string InitialRef = "1";
                    InstituteForAdminView.RefFetch = InitialRef;
                    Prods.Add(InstituteForAdminView);
                    return Prods;
                }

                else
                {
                    
                    int REFCount = Int32.Parse(REFFetch[0]);
                    int REFPLus = REFCount + 1;
                    string REFFinal = REFPLus.ToString();
                    InstituteForAdminView.RefFetch = REFFinal;
                    Prods.Add(InstituteForAdminView);
                    return Prods;

                }
                
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<FinancialOffer> FetchGetFinancialOfferNum()
        {
            try
            {

                var InstituteForAdminView = new FinancialOffer();
                var Prods = new List<FinancialOffer>();
                string QueryREF = "select max(Financial_Offer_Num) from TBL_FinancialOffer";
                List<string> REFFetch = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryREF);
                string Date = System.DateTime.Now.ToString("MM-yyyy");

                if (REFFetch[0] == "")
                {
                    string InitialRef = "1";
                    InstituteForAdminView.FinancialOfferNum = "FO-" + Date + "-" + InitialRef;
                    Prods.Add(InstituteForAdminView);
                    return Prods;
                }
                else
                {

                    string[] SplitNum = REFFetch[0].Split('-');
                    int numberAdd = Int32.Parse(SplitNum[3]);
                    numberAdd += 1;
                    string UpdatedInvoiceNumber = SplitNum[0] + "-" + SplitNum[1] + "-" + SplitNum[2] + "-" + numberAdd;
                    InstituteForAdminView.FinancialOfferNum = UpdatedInvoiceNumber;
                    Prods.Add(InstituteForAdminView);
                    return Prods;
                 

                }

            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<InvoiceBills> FetchGetInvoiceNum()
        {
            try
            {

                var InstituteForAdminView = new InvoiceBills();
                var Prods = new List<InvoiceBills>();
   
                string QueryNumber = "SELECT MAX(CAST(SUBSTRING(InvoiceNum,13, LEN(InvoiceNum)) AS INT)) from TBL_Invoice_Bills_Overview";
                List<string> InVFetch = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryNumber);
                string Date = System.DateTime.Now.ToString("MM-yyyy");

                if (InVFetch[0] == "")
                {
                    string InitialRef = "1";
                    InstituteForAdminView.InvoiceNumber = "INV-" + Date + "-" + InitialRef;
                    Prods.Add(InstituteForAdminView);
                    return Prods;
                }

                else
                {

                    int numberAdd = Int32.Parse(InVFetch[0]);
                    numberAdd += 1;
                    string UpdatedInvoiceNumber = "INV-" + Date + "-" + numberAdd;
                    InstituteForAdminView.InvoiceNumber = UpdatedInvoiceNumber;
                    Prods.Add(InstituteForAdminView);
                    return Prods;
             
                }
                
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }
        
        public List<ComapnyViewModel> FetchCompanyForEdit(string Edit)
        {
            try
            {
                string QueryComapny = "select TBL_Company_ID , CompanyName, Country, Person1, Person2, Phone1, Phone2, Email1, Email2, PaymentTerms, Address1, Address2 from TBL_Company where TBL_Company_ID = "+Edit+" ";
                DataSet ComapniesFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryComapny);
                var CompaniesEdit = new List<ComapnyViewModel>();
                foreach (DataTable table in ComapniesFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ComapnyViewModel();
                        InstituteForAdminView.TBL_Comapny_ID = Convert.ToString(dr["TBL_Company_ID"]);
                        InstituteForAdminView.Company_Name = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.Country = Convert.ToString(dr["Country"]);
                        InstituteForAdminView.Person1 = Convert.ToString(dr["Person1"]);
                        InstituteForAdminView.Person2 = Convert.ToString(dr["Person2"]);
                        InstituteForAdminView.Phone1 = Convert.ToString(dr["Phone1"]);
                        InstituteForAdminView.Phone2 = Convert.ToString(dr["Phone2"]);
                        InstituteForAdminView.Email1 = Convert.ToString(dr["Email1"]);
                        InstituteForAdminView.Email2 = Convert.ToString(dr["Email2"]);
                        InstituteForAdminView.Pay_Terms = Convert.ToString(dr["PaymentTerms"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);

                        CompaniesEdit.Add(InstituteForAdminView);
                    }
                }

                return CompaniesEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }


        public bool RemoveCompany(string Remove)
        {

            string RemoveCompanyMessage = null;
            try
            {
                
                string ProductCheckAgainstComapny = "select TBL_Product_ID from TBL_Product where TBL_Company_ID = '" + Remove + "'";
                string ProductCompanyCheck = Dbobj.ExecuteScalar(SEDataSource, SEInitialCatolog, SEUser, SEPassword, ProductCheckAgainstComapny);

                if (ProductCompanyCheck == null)
                {

                    string QueryRemoveCompany = "Delete from TBL_Company where TBL_Company_ID = '" + Remove + "'";
                    string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveCompany);
                    RemoveCompanyMessage = "Company Deleted Successfully";
                    return true;
                }
                else if (ProductCompanyCheck != null)
                {
                    RemoveCompanyMessage = "Product(s) are assigned for this company, Comapny  be delete";
                    return false;
                }

            }

            catch (Exception ex)
            {
                RemoveCompanyMessage = "Company cannot be deleted due to system error see inner exception:" + ex.ToString();
                return false;

            }
            return false;

        }

        public List<ProductViewModel> FetchProductTableData()
        {
            try
            {
                
                string QueryProduct = "Select TBL_Product.TBL_Product_ID, TBL_Product.Generic_Name, TBL_Product.Brand_Name, TBL_Product.Dosage_Form,TBL_Product.Strength,TBL_Product.Drug_Registration_Number,TBL_Product.Pack_Size,TBL_Product.Other_Mention_Here,TBL_Product.Stock,TBL_Product.Stax,TBL_Product.Scheme_Quantity,TBL_Product.Bonus,TBL_Product.Trade_Price,TBL_Product.MRP, TBL_Medicines_Groups.Group_Name, TBL_Company.CompanyName from TBL_Product join TBL_Medicines_Groups on TBL_Product.TBL_Medicines_Groups_ID = TBL_Medicines_Groups.TBL_Medicines_Groups_ID join TBL_Company on TBL_Product.TBL_Company_ID = TBL_Company.TBL_Company_ID order by TBL_Product.TBL_Product_ID DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryProduct);
                var Prods = new List<ProductViewModel>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ProductViewModel();
                        InstituteForAdminView.TBL_Product_ID = Convert.ToString(dr["TBL_Product_ID"]);
                        InstituteForAdminView.Generic_Name = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Dosage_Form = Convert.ToString(dr["Dosage_Form"]);
                        InstituteForAdminView.Strength = Convert.ToString(dr["Strength"]);
                        InstituteForAdminView.Drug_Registration_Number = Convert.ToString(dr["Drug_Registration_Number"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack_Size"]);
                        InstituteForAdminView.Other_Mention_Here = Convert.ToString(dr["Other_Mention_Here"]);
                        InstituteForAdminView.Stax = Convert.ToString(dr["Stax"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Scheme_Quantity"]);
                        InstituteForAdminView.Bonus = Convert.ToString(dr["Bonus"]);
                        InstituteForAdminView.Trade_Price = Convert.ToString(dr["Trade_Price"]);
                        InstituteForAdminView.MRP = Convert.ToString(dr["MRP"]);
                        InstituteForAdminView.GroupName = Convert.ToString(dr["Group_Name"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);

                        Prods.Add(InstituteForAdminView);
                    }
                }

                return Prods;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<ProductViewModel> FetchProductTableForEdit(string Edit)
        {
            try
            {
                
                string QueryProduct = "Select TBL_Product.TBL_Product_ID, TBL_Product.Generic_Name, TBL_Product.Brand_Name, TBL_Product.Dosage_Form,TBL_Product.Strength,TBL_Product.Drug_Registration_Number,TBL_Product.Pack_Size,TBL_Product.Other_Mention_Here,TBL_Product.Stock,TBL_Product.Stax,TBL_Product.Scheme_Quantity,TBL_Product.Bonus,TBL_Product.Trade_Price,TBL_Product.MRP, TBL_Medicines_Groups.Group_Name, TBL_Medicines_Groups.TBL_Medicines_Groups_ID, TBL_Company.CompanyName, TBL_Company.TBL_Company_ID from TBL_Product join TBL_Medicines_Groups on TBL_Product.TBL_Medicines_Groups_ID = TBL_Medicines_Groups.TBL_Medicines_Groups_ID join TBL_Company on TBL_Product.TBL_Company_ID = TBL_Company.TBL_Company_ID where TBL_Product.TBL_Product_ID = '" + Edit+"'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryProduct);
                var ProdsEdit = new List<ProductViewModel>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new ProductViewModel();
                        InstituteForAdminView.TBL_Product_ID = Convert.ToString(dr["TBL_Product_ID"]);
                        InstituteForAdminView.Generic_Name = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Dosage_Form = Convert.ToString(dr["Dosage_Form"]);
                        InstituteForAdminView.Strength = Convert.ToString(dr["Strength"]);
                        InstituteForAdminView.Drug_Registration_Number = Convert.ToString(dr["Drug_Registration_Number"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack_Size"]);
                        InstituteForAdminView.Other_Mention_Here = Convert.ToString(dr["Other_Mention_Here"]);
                        InstituteForAdminView.Stax = Convert.ToString(dr["Stax"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Scheme_Quantity"]);
                        InstituteForAdminView.Bonus = Convert.ToString(dr["Bonus"]);
                        InstituteForAdminView.Trade_Price = Convert.ToString(dr["Trade_Price"]);
                        InstituteForAdminView.MRP = Convert.ToString(dr["MRP"]);
                        InstituteForAdminView.GroupName = Convert.ToString(dr["Group_Name"]);
                        InstituteForAdminView.GroupID = Convert.ToString(dr["TBL_Medicines_Groups_ID"]);
                        InstituteForAdminView.CompanyID = Convert.ToString(dr["TBL_Company_ID"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);

                        ProdsEdit.Add(InstituteForAdminView);
                    }
                }

                return ProdsEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public string DeleteProductFromTableRow(string ProductName)
        {

            string RemoveProductMessage = null;
            try
            {
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                string QueryRemoveProduct = "delete from TBL_Purchase_Order where TBL_Product_ID = '" + ProductID[0] + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                RemoveProductMessage = "Product Deleted Successfully";
                return RemoveProductMessage;
                
            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Product cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string DeleteProductFromPurchaseReturnTableRow(string ProductName, string ProductBatch)
        {

            string RemoveProductMessage = null;
            try
            {
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                string QueryRemoveProduct = "delete from TBL_Purchase_Return where TBL_Product_ID = '" + ProductID[0] + "' and Batch = '" + ProductBatch + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                RemoveProductMessage = "Product Deleted Successfully";
                return RemoveProductMessage;

            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Product cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }

        }

        public string DeleteProductFromPurchasesTableRow(string ProductName, string ProductBatch)
        {

            string RemoveProductMessage = null;
            try
            {
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                string QueryRemoveProduct = "delete from TBL_Purchases where TBL_Product_ID = '" + ProductID[0] + "' and Batch = '"+ ProductBatch + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                RemoveProductMessage = "Product Deleted Successfully";
                return RemoveProductMessage;
                
            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Product cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string DeleteProductFromInvoiceDCTableRow(string Product, string Batch)
        {

            string RemoveProductMessage = null;
            try
            {
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + Product + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                string QueryRemoveProductDC = "delete from TBL_DeliveryChallan where TBL_Product_ID = '" + ProductID[0] + "' and Batch = '" + Batch + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProductDC);

                string QueryRemoveProductInv = "delete from TBL_Invoice_Bills where TBL_Product_ID = '" + ProductID[0] + "' and Batch = '" + Batch + "'";
                string InsertedUserList2 = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProductInv);
                RemoveProductMessage = "Product Deleted Successfully";
                return RemoveProductMessage;
                
            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Product cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string RemoveProductFromTable(string Remove)
        {

            string RemoveProductMessage = null;
            try
            {
                
                string QueryRemoveProduct = "Delete from TBL_Product where TBL_Product_ID = '" + Remove + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                RemoveProductMessage = "Product Deleted Successfully";
                return RemoveProductMessage;

            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Product cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string RemovePurchaseOrderData(string Remove, string Ref)
        {

            string RemoveProductMessage = null;
            try
            {
                String CheckInvoiceDelete = "select TBL_Purchase_Order_Overview_id from TBL_Purchases_Overview where TBL_Purchase_Order_Overview_id = '" + Remove + "'";
                DataSet invoiceDeleteEnab = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, CheckInvoiceDelete);

                if (invoiceDeleteEnab.Tables[0].Rows.Count == 0)
                {
              
                    string QueryRemovePOOverView = "delete from TBL_Purchase_Order where TBL_Purchase_Order_Overview_id = '" + Remove + "'";
                    string RemovePoOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemovePOOverView);

                    string QueryRemoveProduct = "Delete from TBL_Purchase_order_Overview where Ref = '" + Ref + "'";
                    string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                    
                    RemoveProductMessage = "Purchase Order Deleted Successfully";
                    return RemoveProductMessage;

                }

                else
                {
                    RemoveProductMessage = "Please remove Purchases First";
                    return RemoveProductMessage;
                }
                
            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Purchase Order cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string RemoveSellReturnData(string Remove)
        {

            string RemoveProductMessage = null;
            try
            {

                String GetOverviewID = "select TBL_Sell_Return_Overview_id from TBL_Sell_Return_Overview where InvoiceNum = '" + Remove + "'";
                List<string> OverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewID);
                
             
                string QueryRemovePOOverView = "delete from TBL_Sell_Return where TBL_Sell_Return_Overview_id = '" + OverviewID[0] + "'";
                string RemovePoOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemovePOOverView);

                string QueryRemoveProduct = "delete from TBL_Sell_Return_Overview where InvoiceNum = '" + Remove + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                RemoveProductMessage = "Sell Return Deleted Successfully";
                return RemoveProductMessage;
                

            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Sell Return cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }

        }

        public string RemoveInvoiceData(string Remove)
        {

            string RemoveProductMessage = null;
            try
            {
              
                String GetOverviewID = "select TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id from TBL_Invoice_Bills inner join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id where InvoiceNum = '" + Remove + "'";
                List<string> OverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewID);

                String CheckInvoiceDelete = " select TBL_invoice_bills_id from TBL_DeliveryChallan where TBL_DeliveryChallan_Overview_id = '" + OverviewID[0] + "'";
                DataSet invoiceDeleteEnab = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, CheckInvoiceDelete);

                if (invoiceDeleteEnab.Tables[0].Rows.Count == 0)
                {
                    string QueryRemovePOOverView = "delete from TBL_Invoice_Bills where TBL_Invoice_Bills_Overview_id = '" + OverviewID[0] + "'";
                    string RemovePoOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemovePOOverView);

                    string QueryRemoveProduct = "delete from TBL_Invoice_Bills_Overview where InvoiceNum = '" + Remove + "'";
                    string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                    RemoveProductMessage = "Invoice Deleted Successfully";
                    return RemoveProductMessage;

                }

                else
                {
                    RemoveProductMessage = "Please remove DC First";
                    return RemoveProductMessage;
                }

            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Invoice cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string RemoveDeliveryChallanData(string Remove)
        {

            string RemoveProductMessage = null;
            try
            {
                String GetOverviewID = "select TBL_DeliveryChallan_Overview.TBL_DeliveryChallan_Overview_id from TBL_DeliveryChallan inner join TBL_DeliveryChallan_Overview on TBL_DeliveryChallan.TBL_DeliveryChallan_Overview_id = TBL_DeliveryChallan_Overview.TBL_DeliveryChallan_Overview_id where DCNum = '" + Remove + "'";
                List<string> OverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewID);
                
                string QueryRemovePOOverView = "delete from TBL_DeliveryChallan where TBL_DeliveryChallan_Overview_id = '" + OverviewID[0] + "'";
                string RemovePoOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemovePOOverView);

                string QueryRemoveProduct = "delete from TBL_DeliveryChallan_Overview where DCNum = '" + Remove + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
                
                RemoveProductMessage = "DC Deleted Successfully";
                return RemoveProductMessage;
                
            }

            catch (Exception ex)
            {
                RemoveProductMessage = "DC cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public string RemovePurchaseReturnData(string Remove, string Ref)
        {

            string RemoveProductMessage = null;
            try
            {
                    string QueryRemovePOOverView = "delete from TBL_purchase_return where tbl_purchase_return_overview_id = '" + Remove + "'";
                    string RemovePoOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemovePOOverView);

                    string QueryRemoveProduct = "delete from tbl_purchase_return_overview where Ref = '" + Ref + "'";
                    string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);

                    RemoveProductMessage = "Purchase Return Deleted Successfully";
                    return RemoveProductMessage;
                

            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Purchase Return cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }

        }

        public string RemovePurchasesData(string Remove, string Ref)
        {

            string RemoveProductMessage = null;
            try
            {

                String CheckInvoiceDelete = "select TBL_Purchases_Overview_id from TBL_Invoice_Bills where TBL_Purchases_Overview_id = '" + Remove + "'";
                DataSet invoiceDeleteEnab = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, CheckInvoiceDelete);

                if (invoiceDeleteEnab.Tables[0].Rows.Count == 0)
                {

                    string QueryRemovePOOverView = "delete from TBL_Purchases where TBL_Purchases_Overview_id = '" + Remove + "'";
                    string RemovePoOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemovePOOverView);

                    string QueryRemoveProduct = "Delete from TBL_Purchases_Overview where Ref = '" + Ref + "'";
                    string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveProduct);
           
                    RemoveProductMessage = "Purchases Deleted Successfully";
                    return RemoveProductMessage;

                }

                else
                {
                    RemoveProductMessage = "Please remove Invoice First";
                    return RemoveProductMessage;
                }

            }

            catch (Exception ex)
            {
                RemoveProductMessage = "Purchases cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveProductMessage;

            }
            
        }

        public List<CustomerViewModel> FetchCustomerData()
        {
            try
            {
                
                string QueryCustomer = "select TBL_Customer_ID , CustomerName, Country, Person1, Person2, Phone1, Phone2, Email1, Email2, PaymentTerms, Address1, Address2 from TBL_Customer order by TBL_Customer_ID DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<CustomerViewModel>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new CustomerViewModel();
                        InstituteForAdminView.TBL_Customer_ID = Convert.ToString(dr["TBL_Customer_ID"]);
                        InstituteForAdminView.Customer_Name = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.Country = Convert.ToString(dr["Country"]);
                        InstituteForAdminView.Person1 = Convert.ToString(dr["Person1"]);
                        InstituteForAdminView.Person2 = Convert.ToString(dr["Person2"]);
                        InstituteForAdminView.Phone1 = Convert.ToString(dr["Phone1"]);
                        InstituteForAdminView.Phone2 = Convert.ToString(dr["Phone2"]);
                        InstituteForAdminView.Email1 = Convert.ToString(dr["Email1"]);
                        InstituteForAdminView.Email2 = Convert.ToString(dr["Email2"]);
                        InstituteForAdminView.Pay_Terms = Convert.ToString(dr["PaymentTerms"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);

                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<Purchases> FetchPurchasesViewData()
        {
            try
            {
                
                string QueryCustomer = "select Ref, CompanyName, InvoiceNum, InvoiceDate,TBL_Purchases_Overview_id, TBL_Purchase_Order_Overview_id from TBL_Purchases_Overview inner join TBL_Company on TBL_Purchases_Overview.TBL_Company_ID =  TBL_Company.TBL_Company_ID Order by CAST(Ref As Int) DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();
                        
                        InstituteForAdminView.Ref = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.PurchasesOverviewID = Convert.ToString(dr["TBL_Purchases_Overview_id"]);
                        InstituteForAdminView.PurchaseOrderOverviewID = Convert.ToString(dr["TBL_Purchase_Order_Overview_id"]);

                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchPurchaseReturnViewData()
        {
            try
            {

                string QueryCustomer = "select Ref, InvoiceNum, ReturnDate, CompanyName, ReturnType, InvoiceDate, TBL_purchases_overview_id,tbl_purchase_return_overview_id from tbl_purchase_return_overview inner join tbl_company on tbl_purchase_return_overview.tbl_company_id = tbl_company.tbl_company_id Order by CAST(Ref As Int) DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new Purchases();

                        InstituteForAdminView.Ref = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.PurchaseReturnDate = Convert.ToString(dr["ReturnDate"]);
                        InstituteForAdminView.ReturnType = Convert.ToString(dr["ReturnType"]);
                        InstituteForAdminView.PurchasesOverviewID = Convert.ToString(dr["TBL_Purchases_Overview_id"]);
                        InstituteForAdminView.PurchaseReturnOverviewID = Convert.ToString(dr["tbl_purchase_return_overview_id"]);

                        



                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<DeliveryChallan> FetchDCViewData()
        {
            try
            {

                string QueryCustomer = "select DISTINCT TBL_DeliveryChallan_Overview_id, CustomerName, Address1, OrderDate, DCNum, DCDate from TBL_DeliveryChallan_Overview inner join TBL_Customer on TBL_DeliveryChallan_Overview.TBL_Customer_ID =  TBL_Customer.TBL_Customer_ID Order by TBL_DeliveryChallan_Overview_id DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<DeliveryChallan>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new DeliveryChallan();
                        
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["DCNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["DCDate"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        
                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<FinancialOffer> FetchFinancialOfferViewData()
        {
            try
            {

                string QueryCustomer = "Select Financial_Offer_Num, CustomerName from TBL_FinancialOffer";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<FinancialOffer>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new FinancialOffer();

                        InstituteForAdminView.FinancialOfferNum = Convert.ToString(dr["Financial_Offer_Num"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                    
                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<InvoiceBills> FetchSellReturnViewData()
        {
            try
            {

                string QueryCustomer = "select DISTINCT TBL_Sell_Return_Overview_id, InvoiceNum, CustomerName, Address1, CustOrderNum, ReturnDate, ReturnType from TBL_Sell_Return_Overview inner join TBL_Customer on TBL_Sell_Return_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID Order by TBL_Sell_Return_Overview_id DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<InvoiceBills>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        
                        InstituteForAdminView.CustOrderNum = Convert.ToString(dr["CustOrderNum"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.SellReturnDate = Convert.ToString(dr["ReturnDate"]);
                        InstituteForAdminView.ReturnType = Convert.ToString(dr["ReturnType"]);

                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
               ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<InvoiceBills> FetchInvoicesViewData()
        {
            try
            {
                
                string QueryCustomer = "select DISTINCT TBL_Invoice_Bills_Overview_id, InvoiceNum, CustomerName, Address1, OrderDate, CustOrderNum from TBL_Invoice_Bills_Overview inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID Order by TBL_Invoice_Bills_Overview_id DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<InvoiceBills>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();

                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.CustOrderNum = Convert.ToString(dr["CustOrderNum"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);

                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchPurchaseOrderViewData()
        {
            try
            {

                string QueryCustomer = "select Ref, CompanyName, Date, TBL_Purchase_Order_Overview_id from TBL_Purchase_Order_Overview inner join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID Order by CAST(Ref As Int) DESC";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<PurchaseOrder>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();
                        
                            InstituteForAdminView.RefID = Convert.ToString(dr["Ref"]);
                            InstituteForAdminView.CNameForVIew = Convert.ToString(dr["CompanyName"]);
                            InstituteForAdminView.DateForVIew = Convert.ToString(dr["Date"]);
                            InstituteForAdminView.PurchaseOrderID = Convert.ToString(dr["TBL_Purchase_Order_Overview_id"]);
                        
                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchPurchaseOrderforView()
        {
            try
            {
                
                string QueryCustomer = "select DISTINCT Ref, CompanyName, Date from TBL_Purchase_Order_Overview inner join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var Customer = new List<PurchaseOrder>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new PurchaseOrder();
                        
                        InstituteForAdminView.RefID = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.CNameForVIew = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.DateForVIew = Convert.ToString(dr["Date"]);
                        InstituteForAdminView.PurchaseOrderID = Convert.ToString(dr["TBL_Purchase_Order_Overview_id"]);
                        
                        Customer.Add(InstituteForAdminView);
                    }
                }

                return Customer;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<Purchases> FetchPurchaseReturnForEdit(string Edit)
        {
            try
            {

                string QueryCustomer = "Select Distinct Ref, InvoiceDate, InvoiceNum, ReturnDate, ReturnType, CompanyName from tbl_purchase_return_overview inner join TBL_Company on tbl_purchase_return_overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where Ref = " + Edit + "";

                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.Ref = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.InvoiceNum = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.PurchaseReturnDate = Convert.ToString(dr["ReturnDate"]);
                        InstituteForAdminView.ReturnType = Convert.ToString(dr["ReturnType"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchPurchaseReturnForEdit2(string Edit)
        {
            try
            {

                string QueryCustomer = "select tbl_purchase_return_id, Brand_Name, Pack, AcUnit, PurchaseQty, Batch, MFG, EXP, PurchaseRate, Amount from tbl_purchase_return inner join TBL_Product on tbl_purchase_return.TBL_Product_ID = TBL_Product.TBL_Product_ID join tbl_purchase_return_overview on tbl_purchase_return.tbl_purchase_return_overview_id = tbl_purchase_return_overview.tbl_purchase_return_overview_id where Ref = " + Edit + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.PurchaseReturnID = Convert.ToString(dr["tbl_purchase_return_id"]);
                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.PackVal = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.prodQty = Convert.ToString(dr["PurchaseQty"]);
                        InstituteForAdminView.prodBatch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.prodMFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.prodEXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.purchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.amount = Convert.ToString(dr["Amount"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchPurchasesForEdit(string Edit)
        {
            try
            {

                string QueryCustomer = "Select Distinct Ref, InvoiceDate, InvoiceNum, CompanyName from TBL_Purchases_Overview inner join TBL_Company on TBL_Purchases_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where Ref = " + Edit + "";
             
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.Ref = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.InvoiceNum = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        
                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<Purchases> FetchPurchasesForEdit2(string Edit)
        {
            try
            {

                string QueryCustomer = "select TBL_Purchases_id, Brand_Name, Pack, AcUnit, PurchaseQty, Batch, MFG, EXP, PurchaseRate, Amount from TBL_Purchases inner join TBL_Product on TBL_Purchases.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchases_Overview on TBL_Purchases_Overview.TBL_Purchases_Overview_id = TBL_Purchases.TBL_Purchases_Overview_id where Ref = " + Edit + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.PurchasesID = Convert.ToString(dr["TBL_Purchases_id"]);
                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.PackVal = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.prodQty = Convert.ToString(dr["PurchaseQty"]);
                        InstituteForAdminView.prodBatch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.prodMFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.prodEXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.purchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.amount = Convert.ToString(dr["Amount"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchPurchaseOrderForEdit(string Edit)
        {
            try
            {

                string QueryCustomer = "Select Distinct Ref, Date, Remark, PlaceOfDelivery, DiscType, WHT, AdvTax,CompanyName from TBL_Purchase_Order_Overview inner join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where Ref = " + Edit + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<PurchaseOrder>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new PurchaseOrder();
                        InstituteForAdminView.RefID = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.CNameForVIew = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.Remark = Convert.ToString(dr["Remark"]);
                        InstituteForAdminView.PlaceofDelivery = Convert.ToString(dr["PlaceOfDelivery"]);
                        InstituteForAdminView.Date = Convert.ToString(dr["Date"]);
                        InstituteForAdminView.DiscType = Convert.ToString(dr["DiscType"]);
                        InstituteForAdminView.WHT = Convert.ToString(dr["WHT"]);
                        InstituteForAdminView.AdvTax = Convert.ToString(dr["AdvTax"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchPurchaseOrderForEdit2(string Edit)
        {
            try
            {

                string QueryCustomer = "Select TBL_Purchase_Order_id, Brand_Name, Pack, Qty, InstRates, OurPercentage, RatesAfterDisc, Value from TBL_Purchase_Order inner join TBL_Purchase_Order_Overview on TBL_Purchase_Order.TBL_Purchase_Order_Overview_id =  TBL_Purchase_Order_Overview.TBL_Purchase_Order_Overview_id inner join TBL_Product on TBL_Purchase_Order.TBL_Product_ID = TBL_Product.TBL_Product_ID where Ref = " + Edit + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<PurchaseOrder>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new PurchaseOrder();
                        InstituteForAdminView.PurchaseOrderID = Convert.ToString(dr["TBL_Purchase_Order_id"]);
                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Qty"]);
                        InstituteForAdminView.InstRates = Convert.ToString(dr["InstRates"]);
                        InstituteForAdminView.OurPercentage = Convert.ToString(dr["OurPercentage"]);
                        InstituteForAdminView.RatesAfterDisc = Convert.ToString(dr["RatesAfterDisc"]);
                        InstituteForAdminView.ProductValue = Convert.ToString(dr["Value"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<Purchases> FetchPurchaseReturnForPrint(string Print)
        {
            try
            {

                string QueryCustomer = "Select Distinct Ref, InvoiceDate, InvoiceNum, CompanyName, ReturnDate, ReturnType, Address1, Address2 from TBL_Purchase_Return_Overview inner join TBL_Company on TBL_Purchase_Return_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where Ref = " + Print + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.Ref = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.InvoiceNum = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);
                        InstituteForAdminView.PurchaseReturnDate = Convert.ToString(dr["ReturnDate"]);
                        InstituteForAdminView.ReturnType = Convert.ToString(dr["ReturnType"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchPurchaseReturnForPrint2(string Print)
        {
            try
            {

                string QueryCustomer = "select TBL_Purchase_Return_id, Brand_Name, Pack, AcUnit, PurchaseQty, Batch, MFG, EXP, PurchaseRate, Amount from TBL_Purchase_Return inner join TBL_Product on TBL_Purchase_Return.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchase_Return_Overview on TBL_Purchase_Return_Overview.TBL_Purchase_Return_Overview_id = TBL_Purchase_Return.TBL_Purchase_Return_Overview_id where Ref = " + Print + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.PurchaseReturnID = Convert.ToString(dr["TBL_Purchase_Return_id"]);
                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.PackVal = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.prodQty = Convert.ToString(dr["PurchaseQty"]);
                        InstituteForAdminView.prodBatch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.prodMFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.prodEXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.purchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.amount = Convert.ToString(dr["Amount"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<Purchases> FetchPurchasesForPrint(string Print)
        {
            try
            {

                string QueryCustomer = "Select Distinct Ref, InvoiceDate, InvoiceNum, CompanyName, Address1, Address2 from TBL_Purchases_Overview inner join TBL_Company on TBL_Purchases_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where Ref = " + Print + "";  
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.Ref = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.InvoiceNum = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<Purchases> FetchPurchasesForPrint2(string Print)
        {
            try
            {

                string QueryCustomer = "select TBL_Purchases_id, Brand_Name, Pack, AcUnit, PurchaseQty, Batch, MFG, EXP, PurchaseRate, Amount from TBL_Purchases inner join TBL_Product on TBL_Purchases.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Purchases_Overview on TBL_Purchases_Overview.TBL_Purchases_Overview_id = TBL_Purchases.TBL_Purchases_Overview_id where Ref = " + Print + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<Purchases>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new Purchases();
                        InstituteForAdminView.PurchasesID = Convert.ToString(dr["TBL_Purchases_id"]);
                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.PackVal = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.prodQty = Convert.ToString(dr["PurchaseQty"]);
                        InstituteForAdminView.prodBatch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.prodMFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.prodEXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.purchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.amount = Convert.ToString(dr["Amount"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchPurchaseOrderForPrint(string Print)
        {
            try
            {

                string QueryCustomer = "Select Distinct Ref, Date, Remark, PlaceOfDelivery, DiscType, WHT, AdvTax,CompanyName, Address1, Address2 from TBL_Purchase_Order_Overview inner join TBL_Company on TBL_Purchase_Order_Overview.TBL_Company_ID = TBL_Company.TBL_Company_ID where Ref = " + Print + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<PurchaseOrder>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new PurchaseOrder();
                        InstituteForAdminView.RefID = Convert.ToString(dr["Ref"]);
                        InstituteForAdminView.CNameForVIew = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.Remark = Convert.ToString(dr["Remark"]);
                        InstituteForAdminView.PlaceofDelivery = Convert.ToString(dr["PlaceOfDelivery"]);
                        InstituteForAdminView.Date = Convert.ToString(dr["Date"]);
                        InstituteForAdminView.DiscType = Convert.ToString(dr["DiscType"]);
                        InstituteForAdminView.WHT = Convert.ToString(dr["WHT"]);
                        InstituteForAdminView.AdvTax = Convert.ToString(dr["AdvTax"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<PurchaseOrder> FetchPurchaseOrderForPrint2(string Print)
        {
            try
            {

                string QueryCustomer = "Select TBL_Purchase_Order_id, Brand_Name, Pack, Qty, InstRates, OurPercentage, RatesAfterDisc, Value from TBL_Purchase_Order inner join TBL_Purchase_Order_Overview on TBL_Purchase_Order.TBL_Purchase_Order_Overview_id =  TBL_Purchase_Order_Overview.TBL_Purchase_Order_Overview_id inner join TBL_Product on TBL_Purchase_Order.TBL_Product_ID = TBL_Product.TBL_Product_ID where Ref = " + Print + "";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<PurchaseOrder>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new PurchaseOrder();
                        InstituteForAdminView.PurchaseOrderID = Convert.ToString(dr["TBL_Purchase_Order_id"]);
                        InstituteForAdminView.ProductName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Pack_Size = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.Scheme_Quantity = Convert.ToString(dr["Qty"]);
                        InstituteForAdminView.InstRates = Convert.ToString(dr["InstRates"]);
                        InstituteForAdminView.OurPercentage = Convert.ToString(dr["OurPercentage"]);
                        InstituteForAdminView.RatesAfterDisc = Convert.ToString(dr["RatesAfterDisc"]);
                        InstituteForAdminView.ProductValue = Convert.ToString(dr["Value"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<InvoiceBills> FetchInvoiceForEdit(string Edit)
        {
            try
            {

                string QueryCustomer = "Select DISTINCT InvoiceNum, OrderDate, CustOrderNum, AdvTax, InvoiceDate, TBL_Customer.TBL_Customer_ID,CustomerName, TBL_Customer.Address1, InvoiceType, Warranty from TBL_Invoice_Bills_Overview inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID where InvoiceNum = '" + Edit + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<InvoiceBills>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new InvoiceBills();
                    
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.CustOrderNum = Convert.ToString(dr["CustOrderNum"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.CustomeID = Convert.ToString(dr["TBL_Customer_ID"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.InvoiceType = Convert.ToString(dr["InvoiceType"]);
                        InstituteForAdminView.Warranty = Convert.ToString(dr["Warranty"]);
                        InstituteForAdminView.AdvTax = Convert.ToString(dr["AdvTax"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public List<InvoiceBills> FetchInvoiceForEdit2(string Edit)
        {
            try
            {

                string QueryCustomer = "select TBL_Invoice_Bills_id, OrderNumRef, TenderCode, ProductRegNum, Brand_Name, Pack, AcUnit, InvoiceQty, Batch, MFG, EXP, PurchaseRate, Amount from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id where InvoiceNum  =  '" + Edit + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<InvoiceBills>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new InvoiceBills();
                        InstituteForAdminView.InvoiceBillsID = Convert.ToString(dr["TBL_Invoice_Bills_id"]);
                        InstituteForAdminView.Ref = Convert.ToString(dr["OrderNumRef"]);
                        InstituteForAdminView.TenderCode = Convert.ToString(dr["TenderCode"]);
                        InstituteForAdminView.ProdRegNum = Convert.ToString(dr["ProductRegNum"]);
                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["InvoiceQty"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.PurchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.Amout = Convert.ToString(dr["Amount"]);
                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<InvoiceBills> FetchInvoicesForPrint(string Print)
        {
            try
            {

                string QueryCustomer = "Select DISTINCT InvoiceNum, OrderDate, CustOrderNum, AdvTax, InvoiceDate, CustomerName, Address1, Address2, InvoiceType, Warranty from TBL_Invoice_Bills_Overview inner join TBL_Customer on TBL_Invoice_Bills_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID where InvoiceNum = '" + Print + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<InvoiceBills>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new InvoiceBills();
          
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.CustOrderNum = Convert.ToString(dr["CustOrderNum"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["InvoiceNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["InvoiceDate"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.InvoiceType = Convert.ToString(dr["InvoiceType"]);
                        InstituteForAdminView.Warranty = Convert.ToString(dr["Warranty"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);
                        InstituteForAdminView.AdvTax = Convert.ToString(dr["AdvTax"]);


                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<InvoiceBills> FetchInvoiceForPrint2(string Print)
        {
            try
            {

                string QueryCustomer = "select TBL_Invoice_Bills_id, OrderNumRef, TenderCode, ProductRegNum, Brand_Name, Generic_Name, Pack, AcUnit, InvoiceQty, Batch, MFG, EXP, PurchaseRate, Amount from TBL_Invoice_Bills inner join TBL_Product on TBL_Invoice_Bills.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id where InvoiceNum = '" + Print + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<InvoiceBills>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new InvoiceBills();
                        InstituteForAdminView.InvoiceBillsID = Convert.ToString(dr["TBL_Invoice_Bills_id"]);
                        InstituteForAdminView.Ref = Convert.ToString(dr["OrderNumRef"]);
                        InstituteForAdminView.TenderCode = Convert.ToString(dr["TenderCode"]);
                        InstituteForAdminView.ProdRegNum = Convert.ToString(dr["ProductRegNum"]);
                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.ProductGenericName = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["InvoiceQty"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);
                        InstituteForAdminView.PurchaseRate = Convert.ToString(dr["PurchaseRate"]);
                        InstituteForAdminView.Amout = Convert.ToString(dr["Amount"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }

        }

        public List<FinancialOffer> FetchFinancialOfferForPrint(string Print)
        {
            try
            {

                string QueryCustomer = "Select DISTINCT Financial_Offer_Num, Desc1, Desc2, Desc3, CustomerName, Notes1, Notes2, Notes3 from TBL_FinancialOffer where Financial_Offer_Num = '" + Print + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<FinancialOffer>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new FinancialOffer();

                        InstituteForAdminView.FinancialOfferNum = Convert.ToString(dr["Financial_Offer_Num"]);
                        InstituteForAdminView.Desc1 = Convert.ToString(dr["Desc1"]);
                        InstituteForAdminView.Desc2 = Convert.ToString(dr["Desc2"]);
                        InstituteForAdminView.Desc3 = Convert.ToString(dr["Desc3"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.Notes1 = Convert.ToString(dr["Notes1"]);
                        InstituteForAdminView.Notes2 = Convert.ToString(dr["Notes2"]);
                        InstituteForAdminView.Notes3 = Convert.ToString(dr["Notes3"]);
                        
                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<FinancialOffer> FetchFinancialOfferForPrint2(string Print)
        {
            try
            {

                string QueryCustomer = "Select TBL_FinancialOffer_Overview_id, Item_Code, AcUnit, TBL_FinancialOfferOverview.Pack_Size, Tender_Quantity, Prod_Reg_Num, TBL_FinancialOfferOverview.Trade_Price, Cust_Bid_Rate, Percent_Quoted_Amount, Total_Amount, Percent_Calculated, Brand_Name, Generic_Name, Group_Name, CompanyName, Financial_Offer_Num from TBL_FinancialOfferOverview inner join TBL_FinancialOffer on TBL_FinancialOffer.TBL_FinancialOffer_id = TBL_FinancialOfferOverview.TBL_FinancialOffer_id  join TBL_Product on TBL_FinancialOfferOverview.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_Medicines_Groups on TBL_Product.TBL_Medicines_Groups_ID = TBL_Medicines_Groups.TBL_Medicines_Groups_ID join TBL_Company on TBL_Product.TBL_Company_ID = TBL_Company.TBL_Company_ID where Financial_Offer_Num = '" + Print + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<FinancialOffer>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new FinancialOffer();
                        InstituteForAdminView.FinancialOffer_id = Convert.ToString(dr["TBL_FinancialOffer_Overview_id"]);
                        InstituteForAdminView.ItemCode = Convert.ToString(dr["Item_Code"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack_Size"]);
                        InstituteForAdminView.TenderQuntity = Convert.ToString(dr["Tender_Quantity"]);
                        InstituteForAdminView.drugProdregNum = Convert.ToString(dr["Prod_Reg_Num"]);
                        InstituteForAdminView.TradePrice = Convert.ToString(dr["Trade_Price"]);
                        InstituteForAdminView.CustBidRate = Convert.ToString(dr["Cust_Bid_Rate"]);
                        InstituteForAdminView.PercentageOfQuotedItems = Convert.ToString(dr["Percent_Quoted_Amount"]);
                        InstituteForAdminView.TotalAmount = Convert.ToString(dr["Total_Amount"]);
                        InstituteForAdminView.PercentCalculated = Convert.ToString(dr["Percent_Calculated"]);
                        InstituteForAdminView.Product_Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Generic_Name = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Group_Name = Convert.ToString(dr["Group_Name"]);
                        InstituteForAdminView.CompanyName = Convert.ToString(dr["CompanyName"]);
                        InstituteForAdminView.FinancialOfferNum = Convert.ToString(dr["Financial_Offer_Num"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<DeliveryChallan> FetchDCForEdit(string Edit)
        {
            try
            {

                string QueryCustomer = "Select DISTINCT OrderDate, DCNum, DCDate, CustomerName, DcType, Warranty from TBL_DeliveryChallan_Overview inner join TBL_Customer on TBL_DeliveryChallan_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID where DCNum = '" + Edit + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<DeliveryChallan>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new DeliveryChallan();
                       
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.DcNum = Convert.ToString(dr["DCNum"]);
                        InstituteForAdminView.DcDate = Convert.ToString(dr["DCDate"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.DcType = Convert.ToString(dr["DcType"]);
                        InstituteForAdminView.Warranty = Convert.ToString(dr["Warranty"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<DeliveryChallan> FetchDCForEdit2(string Edit)
        {
            try
            {

                string QueryCustomer = "Select TBL_DeliveryChallan_id, OrderNumRef, TenderCode, ProductRegNum, Pack, AcUnit, DeliveryQty, Batch, MFG, EXP, Brand_Name from TBL_DeliveryChallan inner join TBL_Product on TBL_DeliveryChallan.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_DeliveryChallan_Overview on TBL_DeliveryChallan.TBL_DeliveryChallan_Overview_id = TBL_DeliveryChallan_Overview.TBL_DeliveryChallan_Overview_id where DCNum = '" + Edit + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<DeliveryChallan>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new DeliveryChallan();
                        InstituteForAdminView.DeliveryChallanID = Convert.ToString(dr["TBL_DeliveryChallan_id"]);
                        InstituteForAdminView.Ref = Convert.ToString(dr["OrderNumRef"]);
                        InstituteForAdminView.TenderCode = Convert.ToString(dr["TenderCode"]);
                        InstituteForAdminView.ProdRegNum = Convert.ToString(dr["ProductRegNum"]);
                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["DeliveryQty"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);
                       
                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        // Method for getting dataSet from Tbl_Delivery_Challan_Overview Table
        public List<DeliveryChallan> FetchDCForPrint(string Print)
        {
            try
            {

                string QueryCustomer = "Select DISTINCT TBL_DeliveryChallan_Overview.OrderDate, TBL_DeliveryChallan_Overview.DCNum, TBL_DeliveryChallan_Overview.DCDate, CustomerName, CustOrderNum, Address1, Address2, TBL_DeliveryChallan_Overview.DcType, TBL_DeliveryChallan_Overview.Warranty from TBL_DeliveryChallan_Overview inner join TBL_Customer on TBL_DeliveryChallan_Overview.TBL_Customer_ID = TBL_Customer.TBL_Customer_ID join TBL_DeliveryChallan on TBL_DeliveryChallan_Overview.TBL_DeliveryChallan_Overview_id = TBL_DeliveryChallan.TBL_DeliveryChallan_Overview_id join TBL_Invoice_Bills on TBL_DeliveryChallan.TBL_Invoice_Bills_id = TBL_Invoice_Bills.TBL_Invoice_Bills_id join TBL_Invoice_Bills_Overview on TBL_Invoice_Bills.TBL_Invoice_Bills_Overview_id = TBL_Invoice_Bills_Overview.TBL_Invoice_Bills_Overview_id  where DCNum = '" + Print + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<DeliveryChallan>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new DeliveryChallan();
                      
                        InstituteForAdminView.OrderDate = Convert.ToString(dr["OrderDate"]);
                        InstituteForAdminView.InvoiceNumber = Convert.ToString(dr["DCNum"]);
                        InstituteForAdminView.InvoiceDate = Convert.ToString(dr["DCDate"]);
                        InstituteForAdminView.CustomerName = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.CustOrderNum = Convert.ToString(dr["CustOrderNum"]);
                        InstituteForAdminView.DcType = Convert.ToString(dr["DcType"]);
                        InstituteForAdminView.Warranty = Convert.ToString(dr["Warranty"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);
                        
                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        // Method for getting dataSet from Tbl_Delivery_Challan Table
        public List<DeliveryChallan> FetchDCForPrint2(string Print)
        {
            try
            {

                string QueryCustomer = "Select TBL_DeliveryChallan_id, OrderNumRef, TenderCode, ProductRegNum, Pack, AcUnit, DeliveryQty, Batch, MFG, EXP, Brand_Name, Generic_Name from TBL_DeliveryChallan inner join TBL_Product on TBL_DeliveryChallan.TBL_Product_ID = TBL_Product.TBL_Product_ID join TBL_DeliveryChallan_Overview on TBL_DeliveryChallan.TBL_DeliveryChallan_Overview_id = TBL_DeliveryChallan_Overview.TBL_DeliveryChallan_Overview_id  where DCNum = '" + Print + "'";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<DeliveryChallan>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        var InstituteForAdminView = new DeliveryChallan();

                        InstituteForAdminView.DeliveryChallanID = Convert.ToString(dr["TBL_DeliveryChallan_id"]);
                        InstituteForAdminView.Ref = Convert.ToString(dr["OrderNumRef"]);
                        InstituteForAdminView.TenderCode = Convert.ToString(dr["TenderCode"]);
                        InstituteForAdminView.ProdRegNum = Convert.ToString(dr["ProductRegNum"]);
                        InstituteForAdminView.ProductBrandName = Convert.ToString(dr["Brand_Name"]);
                        InstituteForAdminView.ProductGenericName = Convert.ToString(dr["Generic_Name"]);
                        InstituteForAdminView.Pack = Convert.ToString(dr["Pack"]);
                        InstituteForAdminView.AcUnit = Convert.ToString(dr["AcUnit"]);
                        InstituteForAdminView.PurchasesQty = Convert.ToString(dr["DeliveryQty"]);
                        InstituteForAdminView.Batch = Convert.ToString(dr["Batch"]);
                        InstituteForAdminView.MFG = Convert.ToString(dr["MFG"]);
                        InstituteForAdminView.EXP = Convert.ToString(dr["EXP"]);

                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<CustomerViewModel> FetchCustomerForEdit(string Edit)
        {
            try
            {
                
                string QueryCustomer = "select TBL_Customer_ID , CustomerName, Country, Person1, Person2, Phone1, Phone2, Email1, Email2, PaymentTerms, Address1, Address2 from TBL_Customer where TBL_Customer_ID = "+Edit+" ";
                DataSet CustomerFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCustomer);
                var CustomerEdit = new List<CustomerViewModel>();
                foreach (DataTable table in CustomerFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        
                        var InstituteForAdminView = new CustomerViewModel();
                        InstituteForAdminView.TBL_Customer_ID = Convert.ToString(dr["TBL_Customer_ID"]);
                        InstituteForAdminView.Customer_Name = Convert.ToString(dr["CustomerName"]);
                        InstituteForAdminView.Country = Convert.ToString(dr["Country"]);
                        InstituteForAdminView.Person1 = Convert.ToString(dr["Person1"]);
                        InstituteForAdminView.Person2 = Convert.ToString(dr["Person2"]);
                        InstituteForAdminView.Phone1 = Convert.ToString(dr["Phone1"]);
                        InstituteForAdminView.Phone2 = Convert.ToString(dr["Phone2"]);
                        InstituteForAdminView.Email1 = Convert.ToString(dr["Email1"]);
                        InstituteForAdminView.Email2 = Convert.ToString(dr["Email2"]);
                        InstituteForAdminView.Pay_Terms = Convert.ToString(dr["PaymentTerms"]);
                        InstituteForAdminView.Address1 = Convert.ToString(dr["Address1"]);
                        InstituteForAdminView.Address2 = Convert.ToString(dr["Address2"]);
                        
                        CustomerEdit.Add(InstituteForAdminView);
                    }
                }

                return CustomerEdit;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public string RemoveCustomer(string Remove)
        {

            string RemoveCustomerMessage = null;
            try
            {
                // Check added for validating if Invoice is created against the customer if invoice is created then user cannot delete customer
                string QueryCheckCustInvoice = "Select TBL_Invoice_Bills_Overview_id from TBL_Invoice_Bills_Overview where TBL_Customer_ID = '" + Remove + "'";
                DataSet checkList = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCheckCustInvoice);

                if (checkList.Tables[0].Rows.Count > 0)
                {
                    RemoveCustomerMessage = "Invoice present";
                    return RemoveCustomerMessage;

                }

                else
                {
                    string QueryRemoveCompany = "Delete from TBL_Customer where TBL_Customer_ID = '" + Remove + "'";
                    string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveCompany);
                    RemoveCustomerMessage = "Customer Deleted Successfully";
                    return RemoveCustomerMessage;
                }
                
            }

            catch (Exception ex)
            {
                RemoveCustomerMessage = "Customer cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveCustomerMessage;

            }
            
        }


        public List<GroupViewModel> FetchProductData()
        {
            try
            {
                string QueryGroup = "select TBL_Medicines_Groups_ID , Group_Name from TBL_Medicines_Groups";
                DataSet GroupFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryGroup);
                var Groups = new List<GroupViewModel>();
                foreach (DataTable table in GroupFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new GroupViewModel();
                        InstituteForAdminView.TBL_Group_ID = Convert.ToString(dr["TBL_Medicines_Groups_ID"]);
                        InstituteForAdminView.Group_Name = Convert.ToString(dr["Group_Name"]);

                        Groups.Add(InstituteForAdminView);
                    }
                }

                return Groups;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<UserViewModel> FetchUsersData()
        {
            try
            {
                
                string QueryUser = "select TBL_USERS_ID, User_name, TBL_ROLES_ID from Tbl_users";
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                var User = new List<UserViewModel>();
                foreach (DataTable table in UserFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new UserViewModel();
                        InstituteForAdminView.USER_ID = Convert.ToString(dr["TBL_USERS_ID"]);
                        InstituteForAdminView.USER_NAME = Convert.ToString(dr["User_name"]);
                        InstituteForAdminView.ROLE_NAME = Convert.ToString(dr["TBL_ROLES_ID"]);
                        InstituteForAdminView.FIRST_NAME = Convert.ToString(dr["User_name"]);
                        InstituteForAdminView.LAST_NAME = Convert.ToString(dr["User_name"]);

                        User.Add(InstituteForAdminView);
                    }
                }

                return User;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }
            
        }

        public List<UserViewModel> FetchUsersDataForEdit(string Edit)
        {
            try
            {
                
                string QueryUser = "select TBL_USERS_ID, User_name, TBL_ROLES_ID from Tbl_users where TBL_USERS_ID = '" + Edit + "'";
                DataSet UserFetch = Dbobj.GetDataFromDatabaseDataSet(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUser);
                var User = new List<UserViewModel>();
                foreach (DataTable table in UserFetch.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var InstituteForAdminView = new UserViewModel();
                        InstituteForAdminView.USER_ID = Convert.ToString(dr["TBL_USERS_ID"]);
                        InstituteForAdminView.USER_NAME = Convert.ToString(dr["User_name"]);
                        InstituteForAdminView.ROLE_NAME = Convert.ToString(dr["TBL_ROLES_ID"]);
                        InstituteForAdminView.FIRST_NAME = Convert.ToString(dr["User_name"]);
                        InstituteForAdminView.LAST_NAME = Convert.ToString(dr["User_name"]);

                        User.Add(InstituteForAdminView);
                    }
                }

                return User;
            }

            catch (Exception ex)
            {
                ErrorMessage.Add(ex.ToString() + Environment.NewLine);
                return null;
            }


        }

        public string RemoveUser(string Remove)
        {

            string RemoveUserMessage = null;
            try
            {

                string QueryUserCount = "select count(*) from ShadaniEnterprises.Tbl_users where TBL_ROLES_ID = '2'";
                List<string> UserCount = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUserCount);

                if (UserCount[0] == "1")
                { return "Super user cannot be delete, property of NJ softwares :)"; }
                
                string QueryRemoveUser = "Delete from Tbl_users where TBL_USERS_ID = '" + Remove + "'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryRemoveUser);
                RemoveUserMessage = "User Deleted Successfully";
                return RemoveUserMessage;

            }

            catch (Exception ex)
            {
                RemoveUserMessage = "User cannot be deleted due to system error see inner exception:" + ex.ToString();
                return RemoveUserMessage;

            }
            
        }

        public string ChangePassword(string currentPassword, string confirmNewPassword, string userid)
        {

            string RemoveUserMessage = null;
            try
            {
               
                string QueryUserCount = "select TBL_USERS_ID from Tbl_users where User_name = '"+userid+"'";
                List<string> UserCount = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryUserCount);

                string QueryCheckCurrentPass = "select Password from Tbl_users where Password = '" + currentPassword + "' and TBL_USERS_ID = " + UserCount[0] + "";
                List<string> PassowrdCheck = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryCheckCurrentPass);
                
                if (UserCount[0] == "1" && PassowrdCheck[0] != null)
                { 
                string QueryChangePass = "update Tbl_users SET Password = '"+confirmNewPassword+"' where TBL_USERS_ID = '"+UserCount[0]+"'";
                string InsertedUserList = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryChangePass);
                RemoveUserMessage = "Password Changed Successfully";
                return RemoveUserMessage;
                }
                
                return RemoveUserMessage;
            }

            catch (Exception ex)
            {
                RemoveUserMessage = "Password cannot be Changed due to system error see inner exception:" + ex.ToString();
                return RemoveUserMessage;

            }
            
        }
        
    }
}