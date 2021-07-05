using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class DataAddition
    {

        private string SEDataSource = ConfigurationManager.AppSettings["SEDataSource"].ToString();
        private string SEInitialCatolog = ConfigurationManager.AppSettings["SEInitialCatolog"].ToString();
        private string SEUser = ConfigurationManager.AppSettings["SEUser"].ToString();
        private string SEPassword = ConfigurationManager.AppSettings["SEPassword"].ToString();
        public String CustomerAdditionMessage = null;
        DataConnection Dbobj = new DataConnection();
        public List<string> ErrorMessage;

        public string CustomerAddition(string CustomerName, string CustomerCountry, string ContactPerson1, string ContactPerson2, string CustomerPhone1, string CustomerPhone2, string CustomerEmail1, string CustomerEmail2, string CustomerPayTerms, string CustomerAddress1, string CustomerAddress2)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_CUSTOMER_ID", "tbl_customer", 1);
                
                string queryAddCustomer = "INSERT INTO [TBL_Customer] ([TBL_Customer_ID],[CustomerName],[Country],[Person1],[Person2],[Phone1],[Phone2],[Email1],[Email2],[PaymentTerms],[Address1],[Address2])VALUES ('" + count + "', '" + CustomerName + "', '" + CustomerCountry + "' , '" + ContactPerson1 + "' , '" + ContactPerson2 + "' , '" + CustomerPhone1 + "' , '" + CustomerPhone2 + "' , '" + CustomerEmail1 + "' , '" + CustomerEmail1 + "' , '" + CustomerPayTerms + "' , '" + CustomerAddress1 + "' , '" + CustomerAddress2 + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }
            
        }

        public string PurchaseOverViewUpdate(string Ref, string Dated, string Remarks, string Placedelivery, string DiscType, string WHT, string advTax, string companyName)
        {

            String CustomerAdditionMessage = null;
            try
            {

                String GetCompanyQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + companyName + "'";
                List<string> CompanyID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCompanyQuery);
                
                string queryAddPurchaseOverview = "Update TBL_Purchase_Order_Overview SET Ref = '" + Ref + "', Date = '" + Dated + "', Remark = '" + Remarks + "', PlaceOfDelivery = '" + Placedelivery + "', DiscType = '" + DiscType + "', WHT = '" + WHT + "', AdvTax = '" + advTax + "',TBL_Company_ID = '" + CompanyID[0] + "' where Ref = '" + Ref +"'";
                string ExecuteQueryPurchaseOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddPurchaseOverview);
                ExecuteQueryPurchaseOverview.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }
            
        }

        public string PurchaseReturnOverViewAddition(string Ref, string InvoiceDate, string InvoiceNum, string ReturnDate, string Company, string ReturnType)
        {

            String CustomerAdditionMessage = null;
            try
            {

                int count = CountProvider("TBL_Purchase_Return_Overview_id", "TBL_Purchase_Return_Overview", 1);

                String GetCompanyQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + Company + "'";
                List<string> CompanyID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCompanyQuery);

                String GetOverviewIDQuery = "Select TBL_Purchases_Overview_id from TBL_Purchases_Overview where Ref = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                string queryAddPurchaseOverview = "INSERT INTO [TBL_Purchase_Return_Overview]([TBL_Purchase_Return_Overview_id],[Ref],[InvoiceNum],[ReturnDate],[TBL_Company_ID],[ReturnType],[InvoiceDate],[TBL_Purchases_Overview_id]) VALUES('" + count + "' , '" + Ref + "' , '" + InvoiceNum + "' , '" + ReturnDate + "' , '" + CompanyID[0] + "', '" + ReturnType + "', '" + InvoiceDate + "', '" + OverViewID[0] + "')";
                string ExecuteQueryPurchaseOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddPurchaseOverview);
                ExecuteQueryPurchaseOverview.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchase return added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchase return addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }

        }

        public string PurchasesOverViewAddition(string Ref, string InvoiceDate, string InvoiceNum, string Company)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_Purchases_Overview_id", "TBL_Purchases_Overview", 1);

                String GetCompanyQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + Company + "'";
                List<string> CompanyID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCompanyQuery);

                String GetOverviewIDQuery = "Select TBL_Purchase_Order_Overview_id from TBL_Purchase_Order_Overview where Ref = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                string queryAddPurchaseOverview = "INSERT INTO [TBL_Purchases_Overview]([TBL_Purchases_Overview_id],[Ref],[InvoiceNum],[InvoiceDate],[TBL_Company_ID],[TBL_Purchase_Order_Overview_id]) VALUES('" + count + "' , '" + Ref + "' , '" + InvoiceNum + "' , '" + InvoiceDate + "' , '" + CompanyID[0] + "', '" + OverViewID[0] + "')";
                string ExecuteQueryPurchaseOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddPurchaseOverview);
                ExecuteQueryPurchaseOverview.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchases added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchases addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }
            
        }

        public string PurchaseOverViewAddition(string Ref , string Dated, string Remarks, string Placedelivery, string DiscType, string WHT, string advTax, string companyName)
        {

            String CustomerAdditionMessage = null;
            try
            {
                if (WHT ==  null ){

                    WHT = "0";
                }

                if (advTax == null)
                {

                    advTax = "0";
                }

                int count = CountProvider("TBL_Purchase_Order_Overview_id", "TBL_Purchase_Order_Overview", 1);

                String GetCompanyQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + companyName + "'";
                List<string> CompanyID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCompanyQuery);

                string queryAddPurchaseOverview = "INSERT INTO.[TBL_Purchase_Order_Overview]  ([Ref],[Date],[Remark],[PlaceOfDelivery],[TBL_Purchase_Order_Overview_id],[DiscType],[WHT],[AdvTax],[TBL_Company_ID]) VALUES('" + Ref + "' , '"+ Dated + "' , '"+ Remarks + "' , '"+Placedelivery+"' , '"+count+ "', '" + DiscType + "', '" + WHT + "', '" + advTax + "', '" + CompanyID[0] + "')";
                string ExecuteQueryPurchaseOverview = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddPurchaseOverview);
                ExecuteQueryPurchaseOverview.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }


        public string DCOverViewUpdate(string OrderDate, string DcNum, string CustomerName, string DcDate, string DcType, string Warranty)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                string UpdateDC = "Update TBL_DeliveryChallan_Overview SET OrderDate = '" + OrderDate + "', DCNum = '" + DcNum + "', DCDate = '" + DcDate + "', DcType = '" + DcType + "', Warranty = '" + Warranty + "' where DCNum = '" + DcNum + "'";
                string ExecuteDCquery = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, UpdateDC);

                ExecuteDCquery.Contains("Sucessfully");
               
                CustomerAdditionMessage = "DeliveryChallan updated Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "DeliveryChallan updated Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }

        }

        // Below method is commented because we are updating DC data through Invoice update User can only update DC overview data just in case needed

        //public string DeliveryChallanUpdate(string DeliveryChallanID, string Ref, string OrderDate, string DcNum, string CustomerName, string DcDate, string DcType, string Warranty, string TenderCode, string ProdRegNum, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP)
        //{

        //    String CustomerAdditionMessage = null;
        //    try
        //    {

                
        //        String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
        //        List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

        //        String GetInvoiceBillsIDQuery = "select TBL_Invoice_Bills_id from TBL_DeliveryChallan where OrderNumRef = '" + Ref + "'";
        //        List<string> InvoiceID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetInvoiceBillsIDQuery);

        //        String GetDCOverviewIDQuery = "Select TBL_DeliveryChallan_Overview_id from TBL_DeliveryChallan_Overview where DCNum = '" + DcNum + "'";
        //        List<string> DCOverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetDCOverviewIDQuery);

        //        string queryAddCustomer = " Update TBL_DeliveryChallan SET TenderCode = '" + TenderCode + "', ProductRegNum = '" + ProdRegNum + "', Pack = '" + PackVal + "', AcUnit = '" + AcUnit + "', DeliveryQty = '" + prodQty + "', Batch = '" + prodBatch + "', MFG = '" + prodMFG + "', EXP = '" + prodEXP + "', TBL_Product_ID = '" + ProductID[0] + "', OrderNumRef = '" + Ref + "', TBL_Invoice_Bills_id = '" + InvoiceID[0] + "', TBL_DeliveryChallan_Overview_id = '"+ DCOverviewID[0] + "' where TBL_DeliveryChallan_id = '" + DeliveryChallanID + "'";
        //        string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
        //        ExecuteQueryCustomer.Contains("Sucessfully");
        //        CustomerAdditionMessage = "Customer added Successfully";
        //        return CustomerAdditionMessage;

        //    }

        //    catch (Exception ex)
        //    {

        //        CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
        //        return CustomerAdditionMessage;
        //    }


        //}

        public string DeliveryChallanAddition(string Ref, string OrderDate, string DcNum, string CustomerName, string DcDate, string DcType, string Warranty, string TenderCode, string ProdRegNum, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP)
        {

            String CustomerAdditionMessage = null;
            try
            {

               

                int count = CountProvider("TBL_DeliveryChallan_id", "TBL_DeliveryChallan", 1);

                string queryAddServerIDMAXCount = "Select MAX(TBL_CUSTOMER_ID) from tbl_customer";
                List<string> ServerIDMAXCount = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddServerIDMAXCount);
                int ServerIDCount = Int32.Parse(ServerIDMAXCount[0]);
                int serveridCountone = ServerIDCount;
                ++serveridCountone;


                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);


                String GetCustomerQuery = "select TBL_Customer_ID from TBL_Customer where CustomerName = '" + CustomerName + "'";
                List<string> CustID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCustomerQuery);

                String GetOverviewIDQuery = "Select TBL_Invoice_Bills_id from TBL_Invoice_Bills where OrderNumRef = '" + Ref + "'";
                List<string> PurchasesID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);
                                                                                                                                                                                                                                                                                                                                         //TBL_DeliveryChallan_id	OrderNumRef	OrderDate	DCNum	DCDate	TenderCode	ProductRegNum	Pack	AcUnit	DeliveryQty	Batch	MFG	EXP	TBL_Customer_ID	TBL_Product_ID	TBL_Invoice_Bills_id
                string queryAddCustomer = "INSERT INTO [TBL_DeliveryChallan]([TBL_DeliveryChallan_id],[OrderNumRef],[OrderDate],[DCNum],[DCDate],[TenderCode],[ProductRegNum],[Pack],[AcUnit],[DeliveryQty],[Batch],[MFG],[EXP],[TBL_Customer_ID],[TBL_Product_ID],[TBL_Invoice_Bills_id],[DcType],[Warranty]) VALUES ('" + count + "', '" + Ref + "', '" + OrderDate + "' , '" + DcNum + "' ,  '" + DcDate + "' , '" + TenderCode + "' , '" + ProdRegNum + "' , '" + PackVal + "' , '" + AcUnit + "' , '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' , '" + CustID[0] + "' , '" + ProductID[0] + "', '" + PurchasesID[0] + "', '" + DcType + "', '" + Warranty + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string InvoiceUpdate(string InvoiceBillsID, string Ref, string OrderDate, string CustOrderNum, string InvoiceNum, string UpdatedDC, string CustomerName, string InvoiceDate, string InvoiceType, string Warranty, string TenderCode, string ProdRegNum, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {

                string[] productName = ProductName.Split('~');
                string prodnameTrim = productName[0].Trim();

                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + prodnameTrim + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                String GetOverviewIDQuery = "Select TBL_Purchases_Overview_id from TBL_Purchases_Overview where Ref = '" + Ref + "'";
                List<string> PurchasesID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                String GetInvoiceOverviewIDQuery = "Select TBL_Invoice_Bills_Overview_id from TBL_Invoice_Bills_Overview where InvoiceNum = '" + InvoiceNum + "'";
                List<string> InvoiceOverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetInvoiceOverviewIDQuery);

                string queryAddCustomer = "Update TBL_Invoice_Bills SET TBL_Product_ID = '" + ProductID[0] + "', TenderCode = '" + TenderCode + "', ProductRegNum = '" + ProdRegNum +"', Pack = '" + PackVal +"', AcUnit = '" + AcUnit +"', InvoiceQty = '" + prodQty + "', Batch = '" + prodBatch + "', MFG = '" + prodMFG + "', EXP = '" + prodEXP + "', PurchaseRate = '" + purchaseRate + "', Amount = '" + amount + "', OrderNumRef = '" + Ref + "', TBL_Purchases_Overview_id = '" + PurchasesID[0] + "', TBL_Invoice_Bills_Overview_id = '" + InvoiceOverviewID[0] + "' where TBL_Invoice_Bills_id = '" + InvoiceBillsID + "'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);

                String GetInvoiceBillsIDQuery = "select TBL_Invoice_Bills_id from TBL_DeliveryChallan where OrderNumRef = '" + Ref + "'";
                List<string> InvoiceID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetInvoiceBillsIDQuery);

                String GetDCOverviewIDQuery = "Select TBL_DeliveryChallan_Overview_id from TBL_DeliveryChallan_Overview where DCNum = '" + UpdatedDC + "'";
                List<string> DCOverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetDCOverviewIDQuery);

                String GetDCId = "Select TBL_DeliveryChallan_id from TBL_DeliveryChallan where TBL_Invoice_Bills_id = '" + InvoiceBillsID + "'";
                List<string> DCId = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetDCId);

                string UpdateDC = "Update TBL_DeliveryChallan SET TenderCode = '" + TenderCode + "', ProductRegNum = '" + ProdRegNum + "', Pack = '" + PackVal + "', AcUnit = '" + AcUnit + "', DeliveryQty = '" + prodQty + "', Batch = '" + prodBatch + "', MFG = '" + prodMFG + "', EXP = '" + prodEXP + "', TBL_Product_ID = '" + ProductID[0] + "', OrderNumRef = '" + Ref + "', TBL_Invoice_Bills_id = '" + InvoiceID[0] + "', TBL_DeliveryChallan_Overview_id = '" + DCOverviewID[0] + "' where TBL_DeliveryChallan_id = '" + DCId[0] + "'";
                string ExecuteDCquery = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, UpdateDC);

                ExecuteQueryCustomer.Contains("Sucessfully");
                ExecuteDCquery.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string FinancialOfferAddition(string FinancialOfferNum, string CustomerName, string Desc1, string Desc2, string Desc3, string Notes1, string Notes2, string Notes3)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_FinancialOffer_id", "TBL_FinancialOffer", 1);
                
                string queryAddCustomer = "INSERT INTO TBL_FinancialOffer([TBL_FinancialOffer_id],[Financial_Offer_Num],[Desc1],[Desc2],[Desc3],[CustomerName],[Notes1],[Notes2],[Notes3]) VALUES ('" + count + "', '" + FinancialOfferNum   + "' , '" + Desc1 + "' , '" + Desc2 + "' ,  '" + Desc3 + "' , '" + CustomerName + "' , '" + Notes1 + "' , '" + Notes2 + "', '" + Notes3 + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                
                CustomerAdditionMessage = "Data added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Data addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string FinancialOfferOverviewAddition(string Percent, string FinancialOfferNumm, string itemCode, string AcUnit, string TenderQuntity, string Pack, string drugProdregNum, string ProductName, string TradePrice, string CustBidRate, string TotalAmount, string PercentCalculated)
        {

            String CustomerAdditionMessage = null;
            try
            {

                int count = CountProvider("TBL_FinancialOffer_Overview_id", "TBL_FinancialOfferOverview", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);


                String GetInvoiceOverviewIDQuery = "Select TBL_FinancialOffer_id from TBL_FinancialOffer where Financial_Offer_Num = '" + FinancialOfferNumm + "'";
                List<string> InvoiceOverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetInvoiceOverviewIDQuery);



                string queryAddCustomer = "INSERT INTO TBL_FinancialOfferOverview ([TBL_FinancialOffer_Overview_id],[Item_Code],[AcUnit],[Pack_Size],[Tender_Quantity],[Prod_Reg_Num],[Trade_Price],[Cust_Bid_Rate],[Percent_Quoted_Amount],[Total_Amount],[Percent_Calculated],[TBL_Product_ID],[TBL_FinancialOffer_id]) VALUES ('" + count + "', '" + itemCode + "', '" + AcUnit + "' , '" + Pack + "' , '" + TenderQuntity + "' ,  '" + drugProdregNum + "' , '" + TradePrice + "' , '" + CustBidRate + "' , '" + Percent + "' , '" + TotalAmount + "' , '" + PercentCalculated + "', '" + ProductID[0] + "' , '" + InvoiceOverviewID[0] + "')";

                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");

               
                CustomerAdditionMessage = "Financial Offer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Financial Offer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string SellReturnOverViewAddition(string OrderDate, string InvoiceNum, string UpdatedDC, string CustomerName, string InvoiceDate, string InvoiceType, string Warranty, string CustOrderNum, string advTax, string ReturnDate, string ReturnType)
        {

            String CustomerAdditionMessage = null;
            try
            {

                if (advTax == null)
                {

                    advTax = "0";
                }

                string[] customerIDSplitedFromName = CustomerName.Split('.');

               

                int count = CountProvider("TBL_Sell_Return_Overview_id", "TBL_Sell_Return_Overview", 1);

                String GetOverviewIDQuery = "select TBL_Invoice_Bills_Overview_id from TBL_Invoice_Bills_Overview where InvoiceNum = '" + InvoiceNum + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);


                string queryAddCustomer = "INSERT INTO TBL_Sell_Return_Overview ([TBL_Sell_Return_Overview_id],[ReturnDate],[CustOrderNum],[InvoiceNum],[InvoiceDate],[TBL_Customer_ID],[ReturnType],[TBL_Invoice_Bills_Overview_id]) VALUES ('" + count + "', '" + ReturnDate + "' , '" + CustOrderNum + "' , '" + InvoiceNum + "' ,  '" + InvoiceDate + "' , '" + customerIDSplitedFromName[0] + "' , '" + ReturnType + "', '"+ OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                
                
                CustomerAdditionMessage = "Data added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Data addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string SellReturnAddition(string Ref, string OrderDate, string InvoiceNum, string UpdatedDC, string CustomerName, string InvoiceDate, string InvoiceType, string Warranty, string CustOrderNum, string TenderCode, string ProdRegNum, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {


                string[] productName = ProductName.Split('~');
                string prodnameTrim = productName[0].Trim();


                int count = CountProvider("TBL_Sell_Return_id", "TBL_Sell_Return", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + prodnameTrim + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                String GetOverviewIDQuery = "Select TBL_Sell_Return_Overview_id from TBL_Sell_Return_Overview where InvoiceNum = '" + InvoiceNum + "'";
                List<string> PurchasesID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

               
                string queryAddCustomer = "INSERT INTO TBL_Sell_Return ([TBL_Sell_Return_id],[OrderNumRef],[TenderCode],[ProductRegNum],[Pack],[AcUnit],[InvoiceQty],[ReturnQty],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[TBL_Product_id],[TBL_Sell_Return_Overview_id]) VALUES ('" + count + "', '" + Ref + "', '" + TenderCode + "' , '" + ProdRegNum + "' , '" + PackVal + "' ,  '" + AcUnit + "' , '" + prodQty + "' , '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' , '" + purchaseRate + "' , '" + amount + "' , '" + ProductID[0] + "' , '" + PurchasesID[0] + "')";

                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");

                
                CustomerAdditionMessage = "Sell Return added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Sell Return addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string InvoiceOverViewAddition(string OrderDate, string InvoiceNum, string UpdatedDC, string CustomerName, string InvoiceDate, string InvoiceType, string Warranty, string CustOrderNum, string advTax)
        {

            String CustomerAdditionMessage = null;
            try
            {

                if (advTax == null)
                {

                    advTax = "0";
                }

                int count = CountProvider("TBL_Invoice_Bills_Overview_id", "TBL_Invoice_Bills_Overview", 1);

                int countDC = CountProvider("TBL_DeliveryChallan_Overview_id", "TBL_DeliveryChallan_Overview", 1);

                // Below code is commented because we are now taking customer ID from viewList instead of getting customerID from customerName which was causing multiple returns as customer name could be identical

                //string[] customerNameSplitedFromAddress = CustomerName.Split(',');
                //String GetCompanyQuery = "select TBL_Customer_ID from TBL_Customer where CustomerName = '" + customerNameSplitedFromAddress[0] + "'";
                //List<string> CustomerID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCompanyQuery);

                string[] customerIDSplitedFromName = CustomerName.Split('.');


                string queryAddCustomer = "INSERT INTO TBL_Invoice_Bills_Overview ([TBL_Invoice_Bills_Overview_id],[OrderDate],[CustOrderNum],[InvoiceNum],[InvoiceDate],[InvoiceType],[TBL_Customer_ID],[Warranty],[AdvTax]) VALUES ('" + count + "', '" + OrderDate + "' , '" + CustOrderNum + "' , '" + InvoiceNum + "' ,  '" + InvoiceDate + "' , '" + InvoiceType + "' , '" + customerIDSplitedFromName[0] + "' , '" + Warranty + "','" + advTax + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");

            
                string queryAddDeliveryChallan = "INSERT INTO TBL_DeliveryChallan_Overview ([TBL_DeliveryChallan_Overview_id],[OrderDate],[DCNum],[DCDate],[TBL_Customer_ID],[DcType],[Warranty]) VALUES ('" + countDC + "', '" + OrderDate + "' , '" + UpdatedDC + "' ,  '" + InvoiceDate + "' , '" + customerIDSplitedFromName[0] + "' , '" + InvoiceType + "' , '" + Warranty + "')";
                string ExecuteQueryDeliveryChallan = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddDeliveryChallan);

                

                ExecuteQueryDeliveryChallan.Contains("Sucessfully");
                CustomerAdditionMessage = "Data added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Data addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string InvoiceAddition(string Ref, string OrderDate, string InvoiceNum, string UpdatedDC, string CustomerName, string InvoiceDate, string InvoiceType, string Warranty, string CustOrderNum, string TenderCode, string ProdRegNum, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {
      
            String CustomerAdditionMessage = null;
            try
            {


                string[] productName = ProductName.Split('~');
                string prodnameTrim = productName[0].Trim();


                int count = CountProvider("TBL_Invoice_Bills_id", "TBL_Invoice_Bills", 1);

             

                int countDC = CountProvider("TBL_DeliveryChallan_id", "TBL_DeliveryChallan", 1);

           
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + prodnameTrim + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                String GetOverviewIDQuery = "Select TBL_Purchases_Overview_id from TBL_Purchases_Overview where Ref = '" + Ref + "'";
                List<string> PurchasesID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                String GetInvoiceOverviewIDQuery = "Select TBL_Invoice_Bills_Overview_id from TBL_Invoice_Bills_Overview where InvoiceNum = '" + InvoiceNum + "'";
                List<string> InvoiceOverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetInvoiceOverviewIDQuery);

               

                string queryAddCustomer = "INSERT INTO TBL_Invoice_Bills ([TBL_Invoice_Bills_id],[TBL_Product_ID],[TenderCode],[ProductRegNum],[Pack],[AcUnit],[InvoiceQty],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[OrderNumRef],[TBL_Purchases_Overview_id],[TBL_Invoice_Bills_Overview_id]) VALUES ('" + count + "', '" + ProductID[0] + "', '" + TenderCode + "' , '" + ProdRegNum + "' , '" + PackVal + "' ,  '" + AcUnit + "' , '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' , '" + purchaseRate + "' , '" + amount + "' , '" + Ref + "' , '" + PurchasesID[0] + "' , '" + InvoiceOverviewID[0] + "')";
  
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");

                String GetDCIDQuery = "Select TBL_DeliveryChallan_Overview_id from TBL_DeliveryChallan_Overview where DCNum = '" + UpdatedDC + "'";
                List<string> DcID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetDCIDQuery);

                string queryAddDeliveryChallan = "INSERT INTO TBL_DeliveryChallan ([TBL_DeliveryChallan_id],[TenderCode],[ProductRegNum],[Pack],[AcUnit],[DeliveryQty],[Batch],[MFG],[EXP],[TBL_Product_ID],[OrderNumRef],[TBL_Invoice_Bills_id],[TBL_DeliveryChallan_Overview_id]) VALUES ('" + countDC + "', '" + TenderCode + "', '" + ProdRegNum + "' , '" + PackVal + "' ,  '" + AcUnit + "' , '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' , '" + ProductID[0] + "', '" + Ref + "', '" + count + "' , '" + DcID[0] + "')";
                string ExecuteQueryDeliveryChallan = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddDeliveryChallan);
                
                ExecuteQueryDeliveryChallan.Contains("Sucessfully");
                CustomerAdditionMessage = "Invoice & DC added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "DC addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }


        public string InvoiceOverViewUpdate(string OrderDate, string InvoiceNum, string UpdatedDC, string CustomerName, string InvoiceDate, string InvoiceType, string Warranty, string CustOrderNum, string advTax)
        {

            String CustomerAdditionMessage = null;
            try
            {

                string[] custID = CustomerName.Split('.');
                string custIDtrim = custID[0].Trim();
                string queryAddCustomer = "Update TBL_Invoice_Bills_Overview SET OrderDate = '" + OrderDate + "', CustOrderNum = '" + CustOrderNum + "', TBL_Customer_ID = '" + custIDtrim + "', InvoiceNum = '" + InvoiceNum + "', InvoiceDate = '" + InvoiceDate + "', InvoiceType = '" + InvoiceType + "', Warranty = '" + Warranty + "', AdvTax = '" + advTax + "' where InvoiceNum = '" + InvoiceNum + "'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                
                string UpdateDC = "Update TBL_DeliveryChallan_Overview SET OrderDate = '" + OrderDate + "',  DCNum = '" + UpdatedDC + "', DCDate = '" + InvoiceDate + "',TBL_Customer_ID = '" + custIDtrim + "', DcType = '" + InvoiceType + "', Warranty = '" + Warranty + "' where DCNum = '" + UpdatedDC + "'";
                string ExecuteDCquery = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, UpdateDC);

                ExecuteQueryCustomer.Contains("Sucessfully");
                ExecuteDCquery.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }
            
        }

        public string PurchasesReturnAddition(string Ref, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {

                int count = CountProvider("TBL_Purchase_Return_id", "TBL_Purchase_Return", 1);

                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                String GetOverviewIDQuery = "Select TBL_Purchase_Return_Overview_id from TBL_Purchase_Return_Overview where Ref = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                string queryAddCustomer = "INSERT INTO TBL_Purchase_Return ([TBL_Purchase_Return_id],[Pack],[AcUnit],[PurchaseQty],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[TBL_Product_ID],[TBL_Purchase_Return_Overview_id]) VALUES ('" + count + "', '" + PackVal + "' , '" + AcUnit + "' ,  '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' ,'" + purchaseRate + "' , '" + amount + "' , '" + ProductID[0] + "' , '" + OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchase Return added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchase Return addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string PurchasesAddition(string Ref, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {
               
                int count = CountProvider("TBL_Purchases_id", "TBL_Purchases", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                
                String GetOverviewIDQuery = "Select TBL_Purchases_Overview_id from TBL_Purchases_Overview where Ref = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);
                  
                string queryAddCustomer = "INSERT INTO TBL_Purchases ([TBL_Purchases_id],[AcUnit],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[TBL_Product_ID],[Pack],[PurchaseQty],[TBL_Purchases_Overview_id]) VALUES ('" + count + "', '" + AcUnit + "' , '" + prodBatch + "' ,  '" + prodMFG + "' , '" + prodEXP + "' , '" + purchaseRate + "' , '" + amount + "' ,'" + ProductID[0] + "' , '" + PackVal + "' , '" + prodQty + "' , '" + OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string PurchaseReturnOverViewUpdation(string Ref, string InvoiceNum, string InvoiceDate, string Company, string returnDate, string returnType)
        {

            String CustomerAdditionMessage = null;
            try
            {

                String GetCustomerQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + Company + "'";
                List<string> CustID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCustomerQuery);

                string queryAddCustomer = "Update TBL_Purchase_Return_Overview SET Ref = '" + Ref + "', InvoiceNum = '" + InvoiceNum + "',ReturnDate = '" + returnDate + "', InvoiceDate = '" + InvoiceDate + "', ReturnType = '" + returnType + "', TBL_Company_ID = '" + CustID[0] + "' where Ref = '" + Ref + "'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchase Return Updated Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchase Return Updated Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string PurchaseReturnUpdate(string PurchaseReturnID, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {

                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                string queryAddCustomer = "Update TBL_Purchase_Return SET AcUnit = '" + AcUnit + "', Batch = '" + prodBatch + "', MFG = '" + prodMFG + "', EXP = '" + prodEXP + "', PurchaseRate = '" + purchaseRate + "', Amount = '" + amount + "', TBL_Product_ID = '" + ProductID[0] + "', Pack = '" + PackVal + "', PurchaseQty = '" + prodQty + "' where TBL_Purchase_Return_id = '" + PurchaseReturnID + "'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string PurchasesOverViewUpdation(string Ref, string InvoiceNum, string InvoiceDate, string Company)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                String GetCustomerQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + Company + "'";
                List<string> CustID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCustomerQuery);
                
                string queryAddCustomer = "Update TBL_Purchases_Overview SET Ref = '" + Ref + "', InvoiceNum = '" + InvoiceNum + "', InvoiceDate = '" + InvoiceDate + "', TBL_Company_ID = '" + CustID[0] + "' where Ref = '" + Ref + "'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchases Updated Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchases Updated Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string PurchasesUpdate(string PurchasesID, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {

                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                
                string queryAddCustomer = "Update TBL_Purchases SET AcUnit = '"+AcUnit+ "', Batch = '" + prodBatch + "', MFG = '" + prodMFG + "', EXP = '" + prodEXP + "', PurchaseRate = '" + purchaseRate + "', Amount = '" + amount + "', TBL_Product_ID = '" + ProductID[0] + "', Pack = '" + PackVal + "', PurchaseQty = '" + prodQty + "' where TBL_Purchases_id = '" + PurchasesID +"'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string PurchaseOrderAddition(string Pack, string Quantit, string InstantRate, string OurPercentage , string RateDiscount,  string Valuelast , string Product , string Refrence)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_Purchase_Order_id", "TBL_Purchase_Order", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + Product + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                
                String GetOverviewIDQuery = "select TBL_Purchase_Order_Overview_id from TBL_Purchase_Order_Overview where ref  = '"+Refrence+"'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);                                                             

                string queryAddCustomer = "INSERT INTO [TBL_Purchase_Order]([TBL_Purchase_Order_id],[Pack],[Qty],[InstRates],[OurPercentage],[RatesAfterDisc],[Value],[TBL_Product_ID],[TBL_Purchase_Order_Overview_id]) VALUES ('" + count + "', '" + Pack + "', '" + Quantit + "' , '" + InstantRate + "' ,  '" + OurPercentage + "' , '" + RateDiscount + "' , '" + Valuelast + "', '" + ProductID[0] + "' , '" + OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;
                
            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }
      
        public string addNewProductinPurchaseOrder(string PackValue, string Quantity, string instantRate, string OurPercente, string RateAfterDiscount, string Value, string companyName, string ProductName, string Ref)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_Purchase_Order_id", "TBL_Purchase_Order", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                

                String GetOverviewIDQuery = "select TBL_Purchase_Order_Overview_id from TBL_Purchase_Order_Overview where ref  = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                string queryAddCustomer = "INSERT INTO [TBL_Purchase_Order]([TBL_Purchase_Order_id],[Pack],[Qty],[InstRates],[OurPercentage],[RatesAfterDisc],[Value],[TBL_Product_ID],[TBL_Purchase_Order_Overview_id]) VALUES ('" + count + "', '" + PackValue + "', '" + Quantity + "' , '" + instantRate + "' ,  '" + OurPercente + "' , '" + RateAfterDiscount + "' , '" + Value + "' , '" + ProductID[0] + "' , '" + OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);  //, Quantity, instantRate, OurPercente, RateAfterDiscount, Value, , , 
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string addNewProductinPurchaseReturn(string Ref, string InvoiceDate, string InvoiceNum, string Company, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount, string returnDate, string returnType)
        {

            String CustomerAdditionMessage = null;
            try
            {

                int count = CountProvider("TBL_Purchase_Return_id", "TBL_Purchase_Return", 1);

                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                String GetOverviewIDQuery = "select TBL_Purchase_Return_Overview_id from TBL_Purchase_Return_Overview where ref  = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                string queryAddCustomer = "INSERT INTO [TBL_Purchase_Return]([TBL_Purchase_Return_id],[AcUnit],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[TBL_Product_ID],[Pack],[PurchaseQty],[TBL_Purchase_Return_Overview_id]) VALUES ('" + count + "', '" + AcUnit + "', '" + prodBatch + "' , '" + prodMFG + "' ,  '" + prodEXP + "' , '" + purchaseRate + "' , '" + amount + "' , '" + ProductID[0] + "', '" + PackVal + "', '" + prodQty + "' , '" + OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);  //, Quantity, instantRate, OurPercente, RateAfterDiscount, Value, , , 
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchase Return added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchase Return addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string addNewProductinPurchases(string Ref, string InvoiceDate, string InvoiceNum, string Company, string ProductName, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_Purchases_id", "TBL_Purchases", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + ProductName + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                
                String GetOverviewIDQuery = "select TBL_Purchases_Overview_id from TBL_Purchases_Overview where ref  = '" + Ref + "'";
                List<string> OverViewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                string queryAddCustomer = "INSERT INTO [TBL_Purchases]([TBL_Purchases_id],[AcUnit],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[TBL_Product_ID],[Pack],[PurchaseQty],[TBL_Purchases_Overview_id]) VALUES ('" + count + "', '" + AcUnit + "', '" + prodBatch + "' , '" + prodMFG + "' ,  '" + prodEXP + "' , '" + purchaseRate + "' , '" + amount + "' , '" + ProductID[0] + "', '" + PackVal + "', '" + prodQty + "' , '" + OverViewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);  //, Quantity, instantRate, OurPercente, RateAfterDiscount, Value, , , 
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Purchases added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Purchases addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string addNewProductinInvoiceDC(string Ref, string InvoiceDate, string InvoiceNum, string UpdatedDC, string Company, string ProductName, string TenderCode, string ProdRegNum, string PackVal, string AcUnit, string prodQty, string prodBatch, string prodMFG, string prodEXP, string purchaseRate, string amount)
        {

            String CustomerAdditionMessage = null;
            try
            {

                string[] productName = ProductName.Split('~');
                string prodnameTrim = productName[0].Trim();
                int count = CountProvider("TBL_Invoice_Bills_id", "TBL_Invoice_Bills", 1);
                int countDC = CountProvider("TBL_DeliveryChallan_id", "TBL_DeliveryChallan", 1);
                
                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + prodnameTrim + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);

                String GetOverviewIDQuery = "Select TBL_Purchases_Overview_id from TBL_Purchases_Overview where Ref = '" + Ref + "'";
                List<string> PurchasesID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetOverviewIDQuery);

                String GetInvoiceOverviewIDQuery = "Select TBL_Invoice_Bills_Overview_id from TBL_Invoice_Bills_Overview where InvoiceNum = '" + InvoiceNum + "'";
                List<string> InvoiceOverviewID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetInvoiceOverviewIDQuery);

                string queryAddCustomer = "INSERT INTO TBL_Invoice_Bills ([TBL_Invoice_Bills_id],[TBL_Product_ID],[TenderCode],[ProductRegNum],[Pack],[AcUnit],[InvoiceQty],[Batch],[MFG],[EXP],[PurchaseRate],[Amount],[OrderNumRef],[TBL_Purchases_Overview_id],[TBL_Invoice_Bills_Overview_id]) VALUES ('" + count + "', '" + ProductID[0] + "', '" + TenderCode + "' , '" + ProdRegNum + "' , '" + PackVal + "' ,  '" + AcUnit + "' , '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' , '" + purchaseRate + "' , '" + amount + "' , '" + Ref + "' , '" + PurchasesID[0] + "' , '" + InvoiceOverviewID[0] + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");

                String GetDCIDQuery = "Select TBL_DeliveryChallan_Overview_id from TBL_DeliveryChallan_Overview where DCNum = '" + UpdatedDC + "'";
                List<string> DcID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetDCIDQuery);

                string queryAddDeliveryChallan = "INSERT INTO TBL_DeliveryChallan ([TBL_DeliveryChallan_id],[TenderCode],[ProductRegNum],[Pack],[AcUnit],[DeliveryQty],[Batch],[MFG],[EXP],[TBL_Product_ID],[OrderNumRef],[TBL_Invoice_Bills_id],[TBL_DeliveryChallan_Overview_id]) VALUES ('" + countDC + "', '" + TenderCode + "', '" + ProdRegNum + "' , '" + PackVal + "' ,  '" + AcUnit + "' , '" + prodQty + "' , '" + prodBatch + "' , '" + prodMFG + "' , '" + prodEXP + "' , '" + ProductID[0] + "', '" + Ref + "', '" + count + "' , '" + DcID[0] + "')";
                string ExecuteQueryDeliveryChallan = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddDeliveryChallan);

                ExecuteQueryDeliveryChallan.Contains("Sucessfully");
                CustomerAdditionMessage = "DC added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "DC addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }
            
        }

        public string PurchaseOrderUpdate(string PurchaseID, string Pack, string Quantit, string InstantRate, string OurPercentage, string RateDiscount, string Valuelast, string Product, string Refrence)
        {

            String CustomerAdditionMessage = null;
            try
            {

                String GetProductQuery = "select TBL_Product_ID from TBL_Product where Brand_Name = '" + Product + "'";
                List<string> ProductID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetProductQuery);
                
                string queryAddCustomer = "Update TBL_Purchase_Order SET Pack = '"+Pack+"', Qty = '"+Quantit+"', InstRates = '"+InstantRate+"', OurPercentage = '"+OurPercentage+"', RatesAfterDisc = '"+RateDiscount+"', Value = '"+Valuelast+"', TBL_Product_ID = '"+ProductID[0]+"' where TBL_Purchase_Order_id = '"+PurchaseID+"'";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string CustomerUpdate(string custID, string CustomerName, string CustomerCountry, string ContactPerson1, string ContactPerson2, string CustomerPhone1, string CustomerPhone2, string CustomerEmail1, string CustomerEmail2, string CustomerPayTerms, string CustomerAddress1, string CustomerAddress2)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                string queryAddCustomer = "UPDATE TBL_Customer SET CustomerName = '"+CustomerName+"', Country = '"+CustomerCountry+"', Person1 = '"+ContactPerson1+"', Person2 = '"+ContactPerson2+"', Phone1 = '"+CustomerPhone1+"', Phone2 = '"+CustomerPhone2+"', Email1 = '"+CustomerEmail1+"', Email2 = '"+CustomerEmail2+"', PaymentTerms = '"+CustomerPayTerms+"', Address1 = '"+CustomerAddress1+"' ,Address2 = '"+CustomerAddress2+"'  where TBL_Customer_ID = '"+custID+"' ";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Customer updated Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Customer updation Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string CompanyAddition(string ComapnyName, string ComapnyCountry, string ContactPerson1, string ContactPerson2, string CompanyPhone1, string CompanyPhone2, string CompanyEmail1, string CompanyEmail2, string CompanyPayTerms, string CompanyAddress1, string CompanyAddress2)
        {

            String CustomerAdditionMessage = null;
            try
            {
                
                int count = CountProvider("TBL_Company_ID", "tbl_company", 1);

                string queryAddCustomer = "INSERT INTO [TBL_Company] ([TBL_Company_ID],[CompanyName],[Country],[Person1],[Person2],[Phone1],[Phone2],[Email1],[Email2],[PaymentTerms],[Address1],[Address2])VALUES ('" + count + "', '" + ComapnyName + "', '" + ComapnyCountry + "' , '" + ContactPerson1 + "' , '" + ContactPerson2 + "' , '" + CompanyPhone1 + "' , '" + CompanyPhone2 + "' , '" + CompanyEmail1 + "' , '" + CompanyEmail1 + "' , '" + CompanyPayTerms + "' , '" + CompanyAddress1 + "' , '" + CompanyAddress2 + "')";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Company added Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Company addition Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }
            
        }

        public string CompanyUpdate(string CompID, string ComapnyName, string ComapnyCountry, string ContactPerson1, string ContactPerson2, string CompanyPhone1, string CompanyPhone2, string CompanyEmail1, string CompanyEmail2, string CompanyPayTerms, string CompanyAddress1, string CompanyAddress2)
        {

            String CustomerAdditionMessage = null;
            try
            { 

                string queryUpdateCustomer = "UPDATE TBL_Company SET CompanyName = '" + ComapnyName + "', Country = '" + ComapnyCountry + "', Person1 = '" + ContactPerson1 + "', Person2 = '" + ContactPerson2 + "', Phone1 = '" + CompanyPhone1 + "', Phone2 = '" + CompanyPhone2 + "', Email1 = '" + CompanyEmail1 + "', Email2 = '" + CompanyEmail2 + "', PaymentTerms = '" + CompanyPayTerms + "', Address1 = '" + CompanyAddress1 + "' ,Address2 = '" + CompanyAddress2 + "'  where TBL_Company_ID = '" + CompID + "' ";
                string ExecuteQueryCustomer = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryUpdateCustomer);
                ExecuteQueryCustomer.Contains("Sucessfully");
                CustomerAdditionMessage = "Company updated Successfully";
                return CustomerAdditionMessage;

            }

            catch (Exception ex)
            {

                CustomerAdditionMessage = "Company update Failed see inner Exception:" + ex.ToString();
                return CustomerAdditionMessage;
            }


        }

        public string UserAddition(string userNameAdd, string userFirstName, string userLastName, int role, string password, string confirmpassword)
        {

            String UserAdditionMessage = null;
            try
            {
                int count = CountProvider("TBL_USERS_ID", "Tbl_users", 1);
                
                return UserAdditionMessage;

            }

            catch (Exception ex)
            {

                UserAdditionMessage = "User addition Failed see inner Exception:" + ex.ToString();
                return UserAdditionMessage;
            }
            
        }
        
        public string ProductAddition(string Companyselect, string ProdGenericName, string ProdBrandName, string DosageForm, string ProdStrength, string DrugRegNo, string PackSize, string Group, string GroupOther, string ProdStax, string ProdSchemeQty, string ProdBonus, string ProdTP, string ProdMRP)
        {

            String ProductAdditionMessage = null;
            try
            {

                int count = CountProvider("TBL_Product_ID", "TBL_Product", 1);

                string ProdStock = ""; // Product Stock was removed from UI so

                if (PackSize.ToString().Contains("'"))
                {
                    string PackSizeDoubleQuotes = PackSize.Replace("'", "''");
                    string queryAddProduct = "INSERT INTO [TBL_Product]([TBL_Product_ID],[Generic_Name],[Brand_Name],[Dosage_Form],[Strength],[Drug_Registration_Number],[Pack_Size],[Other_Mention_Here],[Stock],[Stax],[Scheme_Quantity],[Bonus],[Trade_Price],[MRP],[TBL_Medicines_Groups_ID] , [TBL_Company_ID]) VALUES ('" + count + "', '" + ProdGenericName + "', '" + ProdBrandName + "', '" + DosageForm + "', '" + ProdStrength + "', '" + DrugRegNo + "', '" + PackSizeDoubleQuotes + "', '" + GroupOther + "' , '" + ProdStock + "' , '" + ProdStax + "' , '" + ProdSchemeQty + "' , '" + ProdBonus + "' , '" + ProdTP + "' , '" + ProdMRP + "' , '" + Group + "' , '" + Companyselect + "')";
                    string ExecuteQueryProduct = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddProduct);

                    ProductAdditionMessage = "Product added successfully";
                    return ProductAdditionMessage;
                }

                else
                {
                    string queryAddProduct = "INSERT INTO [TBL_Product]([TBL_Product_ID],[Generic_Name],[Brand_Name],[Dosage_Form],[Strength],[Drug_Registration_Number],[Pack_Size],[Other_Mention_Here],[Stock],[Stax],[Scheme_Quantity],[Bonus],[Trade_Price],[MRP],[TBL_Medicines_Groups_ID] , [TBL_Company_ID]) VALUES ('" + count + "', '" + ProdGenericName + "', '" + ProdBrandName + "', '" + DosageForm + "', '" + ProdStrength + "', '" + DrugRegNo + "', '" + PackSize + "', '" + GroupOther + "' , '" + ProdStock + "' , '" + ProdStax + "' , '" + ProdSchemeQty + "' , '" + ProdBonus + "' , '" + ProdTP + "' , '" + ProdMRP + "' , '" + Group + "' , '" + Companyselect + "')";
                    string ExecuteQueryProduct = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddProduct);

                    ProductAdditionMessage = "Product added successfully";
                    return ProductAdditionMessage;
                }
                

            }

            catch (Exception ex)
            {

                ProductAdditionMessage = "Product addition Failed see inner Exception:" + ex.ToString();
                return ProductAdditionMessage;
            }


        }

        public string ProductUpdate(string ProdID, string CompanySelect, string CompanyName1, string ProdGenericName, string ProdBrandName, string DosageForm, string ProdStrength, string DrugRegNo, string PackSize, string Group, string GroupOther, string ProdStax, string ProdSchemeQty, string ProdBonus, string ProdTP, string ProdMRP)
        {

            String ProductAdditionMessage = null;
            try

            {
                string ProdStock = ""; //Product Stock was Removed from UI so everytime null (by default) will be updated

                if (!CompanySelect.ToString().Contains("Select"))
                {
                    String GetCustomerQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + CompanySelect + "'";
                    List<string> CustID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCustomerQuery);

                    if (PackSize.ToString().Contains("'"))
                    {
                        string PackSizeDoubleQuotes = PackSize.Replace("'", "''");
                        string queryAddProduct = "Update TBL_Product SET Generic_Name = '" + ProdGenericName + "', Brand_Name = '" + ProdBrandName + "', Dosage_Form = '" + DosageForm + "', Strength = '" + ProdStrength + "', Drug_Registration_Number = '" + DrugRegNo + "', Pack_Size = '" + PackSizeDoubleQuotes + "', Other_Mention_Here = '" + GroupOther + "', Stock = '" + ProdStock + "', Stax = '" + ProdStax + "', Scheme_Quantity = '" + ProdSchemeQty + "', Bonus = '" + ProdBonus + "', Trade_Price = '" + ProdTP + "', MRP = '" + ProdMRP + "', TBL_Medicines_Groups_ID = '" + Group + "', TBL_Company_ID = '" + CustID[0] + "' where TBL_Product_ID = '" + ProdID + "'";
                        string ExecuteQueryProduct = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddProduct);

                        ProductAdditionMessage = "Product updated successfully";
                        return ProductAdditionMessage;
                    }

                    else
                    {
                        string queryAddProduct = "Update TBL_Product SET Generic_Name = '" + ProdGenericName + "', Brand_Name = '" + ProdBrandName + "', Dosage_Form = '" + DosageForm + "', Strength = '" + ProdStrength + "', Drug_Registration_Number = '" + DrugRegNo + "', Pack_Size = '" + PackSize + "', Other_Mention_Here = '" + GroupOther + "', Stock = '" + ProdStock + "', Stax = '" + ProdStax + "', Scheme_Quantity = '" + ProdSchemeQty + "', Bonus = '" + ProdBonus + "', Trade_Price = '" + ProdTP + "', MRP = '" + ProdMRP + "', TBL_Medicines_Groups_ID = '" + Group + "', TBL_Company_ID = '" + CustID[0] + "' where TBL_Product_ID = '" + ProdID + "'";
                        string ExecuteQueryProduct = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddProduct);

                        ProductAdditionMessage = "Product updated successfully";
                        return ProductAdditionMessage;
                    }

                      
                }

                else
                {
                    String GetCustomerQuery = "select TBL_Company_ID from TBL_Company where CompanyName = '" + CompanyName1 + "'";
                    List<string> CustID = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, GetCustomerQuery);


                    if (PackSize.ToString().Contains("'"))
                    {
                        string PackSizeDoubleQuotes = PackSize.Replace("'", "''");
                        string queryAddProduct = "Update TBL_Product SET Generic_Name = '" + ProdGenericName + "', Brand_Name = '" + ProdBrandName + "', Dosage_Form = '" + DosageForm + "', Strength = '" + ProdStrength + "', Drug_Registration_Number = '" + DrugRegNo + "', Pack_Size = '" + PackSizeDoubleQuotes + "', Other_Mention_Here = '" + GroupOther + "', Stock = '" + ProdStock + "', Stax = '" + ProdStax + "', Scheme_Quantity = '" + ProdSchemeQty + "', Bonus = '" + ProdBonus + "', Trade_Price = '" + ProdTP + "', MRP = '" + ProdMRP + "', TBL_Medicines_Groups_ID = '" + Group + "', TBL_Company_ID = '" + CustID[0] + "' where TBL_Product_ID = '" + ProdID + "'";
                        string ExecuteQueryProduct = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddProduct);

                        ProductAdditionMessage = "Product updated successfully";
                        return ProductAdditionMessage;
                    }

                    else
                    {
                        string queryAddProduct = "Update TBL_Product SET Generic_Name = '" + ProdGenericName + "', Brand_Name = '" + ProdBrandName + "', Dosage_Form = '" + DosageForm + "', Strength = '" + ProdStrength + "', Drug_Registration_Number = '" + DrugRegNo + "', Pack_Size = '" + PackSize + "', Other_Mention_Here = '" + GroupOther + "', Stock = '" + ProdStock + "', Stax = '" + ProdStax + "', Scheme_Quantity = '" + ProdSchemeQty + "', Bonus = '" + ProdBonus + "', Trade_Price = '" + ProdTP + "', MRP = '" + ProdMRP + "', TBL_Medicines_Groups_ID = '" + Group + "', TBL_Company_ID = '" + CustID[0] + "' where TBL_Product_ID = '" + ProdID + "'";
                        string ExecuteQueryProduct = Dbobj.ExecuteSqlQuery(SEDataSource, SEInitialCatolog, SEUser, SEPassword, queryAddProduct);

                        ProductAdditionMessage = "Product updated successfully";
                        return ProductAdditionMessage;
                    }
                }

              
            }

            catch (Exception ex)
            {

                ProductAdditionMessage = "Product updation Failed see inner Exception:" + ex.ToString();
                return ProductAdditionMessage;
            }


        }

        public int CountProvider(string col_name, string table_name, int countrequired)

        {
            string QueryMAXCount = "Select MAX(" + col_name + ")from " + table_name + "";
            List<string> nullcheck = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryMAXCount);

            if (nullcheck[0] == "") { return 1; }

            else
            {

                List<string> MAXCount = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryMAXCount);
                int ServerIDCount = Int32.Parse(MAXCount[0]);
                int finalCount = 0;
                for (int i = 0; i <= countrequired; i++)
                {
                    finalCount = ServerIDCount;
                    ++finalCount;
                }


                return finalCount;
            }



        }
    }
}