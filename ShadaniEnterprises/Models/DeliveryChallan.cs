using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class DeliveryChallan
    {
        public string DeliveryChallanID { get; set; }
        public string OrderDate { get; set; }
        public string DcType { get; set; }
        public string Warranty { get; set; }
        public string Ref { get; set; }
        public string CustomeID { get; set; }
        public string CustomerName { get; set; }
        public string ProductBrandName { get; set; }
        public string ProductGenericName { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string TenderCode { get; set; }
        public string ProdRegNum { get; set; }
        public string Pack { get; set; }
        public string AcUnit { get; set; }
        public string PurchasesQty { get; set; }
        public string Batch { get; set; }
        public string MFG { get; set; }
        public string EXP { get; set; }
        public string DcDate { get; set; }
        public string DcNum { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CustOrderNum { get; set; }
        
    }

    public class DeliveryChallanList
    {
        public List<DeliveryChallan> LstDeliveryChallan;
        public List<DeliveryChallan> LstDeliveryChallanDetails;

    }

    public class SaveDeliveryChallan
    {
        public string DeliveryChallanID { get; set; }
        public string Ref { get; set; }
        public string OrderDate { get; set; }
        public string DcNum { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string DcDate { get; set; }
        public string DcType { get; set; }
        public string Warranty { get; set; }
        public string TenderCode { get; set; }
        public string ProdRegNum { get; set; }
        public string PackVal { get; set; }
        public string AcUnit { get; set; }
        public string prodQty { get; set; }
        public string prodBatch { get; set; }
        public string prodMFG { get; set; }
        public string prodEXP { get; set; }
   



    }


}