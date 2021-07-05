using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class Purchases
    {
        public string PurchasesID { get; set; }
        public string PurchasesOverviewID { get; set; }
        public string PurchaseReturnID { get; set; }
        public string PurchaseReturnOverviewID { get; set; }
        public string PurchaseOrderOverviewID { get; set; }
        public string Ref { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string CompanyName { get; set; }
        public string PurchasesAmountPerRef { get; set; }
        public string ProductName { get; set; }
        public string PackVal { get; set; }
        public string AcUnit { get; set; }
        public string prodQty { get; set; }
        public string prodBatch { get; set; }
        public string prodMFG { get; set; }
        public string prodEXP { get; set; }
        public string purchaseRate { get; set; }
        public string amount { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Company { get; set; }
        public string InvoiceNum { get; set; }

        public string PurchaseReturnDate { get; set; }

        public string ReturnType { get; set; }

        public string totalAmount { get; set; }

    }
    public class PurchasesList
    {
        public List<Purchases> LstPurchases;
        public List<Purchases> LstPurchasesDetails;
        public List<Purchases> LstReffetch;

    }

    public class SavePurchases
    {
        public string PurchasesID { get; set; }
        public string ProductName { get; set; }
        public string PackVal { get; set; }
        public string AcUnit { get; set; }
        public string prodQty { get; set; }
        public string prodBatch { get; set; }
        public string prodMFG { get; set; }
        public string prodEXP { get; set; }
        public string purchaseRate { get; set; }
        public string amount { get; set; }
        public string Ref { get; set; }
        public string Company { get; set; }
        public string InvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public string PurchaseReturnDate { get; set; }
        public string PurchaseReturnID { get; set; }
        
        public string ReturnType { get; set; }
    }
}