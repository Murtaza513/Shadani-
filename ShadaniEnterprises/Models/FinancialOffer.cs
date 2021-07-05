using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class FinancialOffer
    {
        public string FinancialOffer_id { get; set; }
        public string FinancialOfferNum { get; set; }
        public string Product_Brand_Name { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string CustomerName { get; set; }
        public string ItemCode { get; set; }
        public string AcUnit { get; set; }
        public string Pack { get; set; }
        public string TenderQuntity { get; set; }
        public string drugProdregNum { get; set; }
        public string TradePrice { get; set; }
        public string CustBidRate { get; set; }
        public string CustBidRateInWords { get; set; }
        public string PercentageOfQuotedItems { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string Notes3 { get; set; }
        public string TotalAmount { get; set; }
        public string PercentCalculated { get; set; }
        public string SumTotalAmount { get; set; }
        public string SumPercentCalculated { get; set; }
        public string Generic_Name { get; set; }
        public string Group_Name { get; set; }
        public string CompanyName { get; set; }

    }

    public class FinancialOfferList
    {
        public List<FinancialOffer> LstFinancialOffer;
        public List<FinancialOffer> LstFinancialOfferDetails;

    }

    public class SaveFinancialOffers
    {
        public string FinancialOffer_id { get; set; }
        public string FinancialOfferNum { get; set; }
        public string Product_Brand_Name { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string CustomerName { get; set; }
        public string ItemCode { get; set; }
        public string AcUnit { get; set; }
        public string Pack { get; set; }
        public string TenderQuntity { get; set; }
        public string drugProdregNum { get; set; }
        public string TradePrice { get; set; }
        public string CustBidRate { get; set; }
        public string PercentageOfQuotedItems { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string Notes3 { get; set; }
        public string TotalAmount { get; set; }
        public string PercentCalculated { get; set; }
        public string Generic_Name { get; set; }
        public string Group_Name { get; set; }
        public string CompanyName { get; set; }



    }
}