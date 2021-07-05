using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class ComapnyViewModel
    {
        public string TBL_Comapny_ID { get; set; }
        public string Company_Name { get; set; }
        public string Country { get; set; }
        public string Person1 { get; set; }
        public string Person2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Pay_Terms { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }

    public class ComapnyViewModelList
    {
        public List<ComapnyViewModel> LstCompanyList;

        public List<GroupViewModel> LstGroupList;
    }

}