using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShadaniEnterprises.Models
{
    public class MyAuthorize : AuthorizeAttribute
    {


        private string SEDataSource = ConfigurationManager.AppSettings["SEDataSource"].ToString();
        private string SEInitialCatolog = ConfigurationManager.AppSettings["SEInitialCatolog"].ToString();
        private string SEUser = ConfigurationManager.AppSettings["SEUser"].ToString();
        private string SEPassword = ConfigurationManager.AppSettings["SEPassword"].ToString();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {


            try { 
            string url = ((System.Web.HttpRequestWrapper)((System.Web.HttpContextWrapper)httpContext).Request).AppRelativeCurrentExecutionFilePath;



            string userId = httpContext.Session["UserID"].ToString();
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            DataConnection Dbobj = new DataConnection();
            string QueryURLROLEID = "SELECT TBL_ROLES_id,tbl_permission.permission_url FROM tbl_permission INNER JOIN tbl_role_permission ON tbl_role_permission.TBL_PERMISSION_ID = tbl_permission.TBL_PERMISSION_ID where TBL_ROLES_id in (select TBL_ROLES_id from TBL_USERS where User_name = '" + userId + "') and tbl_permission.PERMISSION_URL = '" + url + "'";
            List<string> URL_ROLEIDFetch = Dbobj.ConnectToData(SEDataSource, SEInitialCatolog, SEUser, SEPassword, QueryURLROLEID);
            if (URL_ROLEIDFetch.Count == 2)

            {
                return true;
            }
            }
            catch (Exception ex) { return false;  }


            // Make sure the user is authenticated.
            if (httpContext.User.Identity.IsAuthenticated == false) return false;

            // Do you own custom stuff hereEW
            //bool allow = CheckIfAllowedToAccessStuff();

            return false;
        }
    }
}
