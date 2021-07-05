using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class GroupViewModel
    {
        public string TBL_Group_ID { get; set; }
        public string Group_Name { get; set; }
    }

    public class GroupViewModelList
    {
        public List<GroupViewModelList> lstGroupView;
    }
}