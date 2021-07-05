using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class ProductViewModel
    {
        public string TBL_Product_ID { get; set; }
        public string Generic_Name { get; set; }
        public string Brand_Name { get; set; }
        public string Dosage_Form { get; set; }
        public string Strength { get; set; }
        public string Drug_Registration_Number { get; set; }
        public string Pack_Size { get; set; }
        public string Other_Mention_Here { get; set; }
        public string Stax { get; set; }
        public string Scheme_Quantity { get; set; }
        public string Bonus { get; set; }
        public string Trade_Price { get; set; }
        public string MRP { get; set; }
        public string GroupName { get; set; }
        public string GroupID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyID { get; set; }

    }

    public class ProductViewModelList
    {
        public List<ProductViewModel> LstProductTableList;
        
    }
}