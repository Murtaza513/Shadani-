using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class InvoiceBills
    {
        public string OrderDate { get; set; }
        public string Ref { get; set; }
        public string CustomeID { get; set; }
        public string CustomerName { get; set; }
        public string CustOrderNum { get; set; }
        public string InvoiceType { get; set; }
        public string Warranty { get; set; }
        public string InvoiceBillsID { get; set; }
        public string ProductBrandName { get; set; }
        public string ProductGenericName { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string TenderCode { get; set; }
        public string ProdRegNum { get; set; }
        public string Pack { get; set; }
        public string drugProdregNum { get; set; }
        public string AcUnit { get; set; }
        public string PurchasesQty { get; set; }
        public string Batch { get; set; }
        public string MFG { get; set; }
        public string EXP { get; set; }
        public string PurchaseRate { get; set; }
        public string Amout { get; set; }
        public string PurchasesTotalAmountPerRef { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AdvTax { get; set; }
        public string SellReturnDate { get; set; }

        public string ReturnType { get; set; }


    }

    public class InvoiceBillsList
    {
        public List<InvoiceBills> LstInvoices;
        public List<InvoiceBills> LstInvoicesDetails;
    }

    public class SaveInvoices
    {
        public string InvoiceBillsID { get; set; }
        public string Ref { get; set; }
        public string OrderDate { get; set; }
        public string InvoiceNum { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string Warranty { get; set; }
        public string CustOrderNum { get; set; }
        public string TenderCode { get; set; }
        public string ProdRegNum { get; set; }
        //public string ProductName { get; set; }
        public string PackVal { get; set; }
        public string AcUnit { get; set; }
        public string prodQty { get; set; }
        public string prodBatch { get; set; }
        public string prodMFG { get; set; }
        public string prodEXP { get; set; }
        public string purchaseRate { get; set; }
        public string amount { get; set; }

        public string SellReturnDate { get; set; }

        public string ReturnType { get; set; }
        public string AdvTax { get; set; }

    }
}