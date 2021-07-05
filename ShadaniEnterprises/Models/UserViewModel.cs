using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShadaniEnterprises.Models
{
    public class UserViewModel
    {
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string ROLE_NAME { get; set; }

    }
    public class UserViewModelList
    {
        public List<UserViewModel> lstUserProfiles;
    }
}