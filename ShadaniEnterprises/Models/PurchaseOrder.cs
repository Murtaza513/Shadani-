using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class PurchaseOrder
    {
        public string PurchaseOrderID { get; set; }
        public string Company_ID { get; set; }
        public string CNameForVIew { get; set; }
        public string RefForVIew { get; set; }
        public string DateForVIew { get; set; }
        public string TotalAmount { get; set; }
        public string RefID { get; set; }
        public string ProductName { get; set; }
        public string Pack_Size { get; set; }
        public string Scheme_Quantity { get; set; }
        public string DiscType { get; set; }
        public string Trade_Price { get; set; }
        public string ProductValue { get; set; }
        public List<RoleModel> Roles { get; set; }
        public string RefFetch { get; set; }
        public string OurPercentage { get; set; }
        public string RatesAfterDisc { get; set; }
        public string InstRates { get; set; }
        public string Date { get; set; }
        public string WHT { get; set; }

        public string AdvTax { get; set; }
        public string Remark { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PlaceofDelivery { get; set; }

    }


    public class PurchaseOrderList
    {
        public List<PurchaseOrder> LstReffetch;
        public List<PurchaseOrder> LstPODataFetch;
    }


    public class RoleModel
    {
        public string DiscType { get; set; }
        public string CompanyName { get; set; }
        public string PurchaseOrderID { get; set; }
        public string PurchaseRef { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string Remarks { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string WHT { get; set; }
        public string ProductName { get; set; }
        public string PackVal { get; set; }
        public string Quanty { get; set; }
        public string InstRate { get; set; }
        public string OurPercent { get; set; }
        public string RateAfterDiscount { get; set; }
        public string Value { get; set; }
        public string advTax { get; set; }
    }
}