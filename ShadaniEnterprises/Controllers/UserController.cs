using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShadaniEnterprises.Models;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using ShadaniEnterprises.Reports;
using iTextSharp.text;
using System.Data;

namespace ShadaniEnterprises.Controllers
{
    public class UserController : Controller
    {


      
        [HttpGet]
        public ActionResult Login()
        {
            Session.Abandon();
            //FormsAuthentication.SignOut();
            return View();

        }
 
        [HttpPost]

        public ActionResult Login(string username, string password)

        {

            DataLogic DLOBJ = new DataLogic();
            List<string> ValidatedUser = DLOBJ.loginValidation(username, password);
           
            if (ValidatedUser.Contains(username))
            {
                Session["User"] = ValidatedUser[0];
                if (ValidatedUser[2] == "1" && ValidatedUser[3].ToUpper() == "ADMIN")
                {
                    Session["Role"] = ValidatedUser[2];
                    Session["UserID"] = ValidatedUser[0];
                    FormsAuthentication.SetAuthCookie(username, false);

                    return RedirectToAction("Dashboard", "User");
                }

                else if (ValidatedUser[2] == "2" && ValidatedUser[3].ToUpper() == "USER")
                {
                    Session["UserID"] = ValidatedUser[0];
                    FormsAuthentication.SetAuthCookie(username, false);
                    return RedirectToAction("Dashboard", "User");
                }

            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View();
        }
      
        public JsonResult GetCompany() {
            
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<ComapnyViewModel> InstitutionForAdminData = DLOBJ.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = InstitutionForAdminData;
            var CompaniesList = new List<string>();
            
                foreach(var name in objListCompanyView.LstCompanyList)
            {
                CompaniesList.Add(name.Company_Name);
                //CompaniesList.Add(name.TBL_Comapny_ID);
            }
           
            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllProducts()
        {

           
            DataLogic DLOBJ = new DataLogic();
            List<ProductViewModel> InstitutionForAdminData = DLOBJ.FetchProducts();
            ProductViewModelList objListCompanyView = new ProductViewModelList();
            objListCompanyView.LstProductTableList = InstitutionForAdminData;
            var CompaniesList = new List<string>();

            foreach (var name in objListCompanyView.LstProductTableList)
            {
                CompaniesList.Add(name.Brand_Name);
                //CompaniesList.Add(name.TBL_Comapny_ID);
            }

            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetProductsBasedOnCompany(string Company)
        {
            DataLogic DLOBJ = new DataLogic();
            List<ProductViewModel> InstitutionForAdminData = DLOBJ.FetchProductsBasedOnCompany(Company);
            ProductViewModelList objListProductView = new ProductViewModelList();
            objListProductView.LstProductTableList = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstProductTableList)
            {
                ProdName.Add(name.Brand_Name);
                
            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetProdDetails(string ProductName)
        {
            DataLogic DLOBJ = new DataLogic();
            List<ProductViewModel> InstitutionForAdminData = DLOBJ.FetchProductsDetails(ProductName);
            ProductViewModelList objListProductView = new ProductViewModelList();
            objListProductView.LstProductTableList = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstProductTableList)
            {
                ProdDetails.Add(name.Pack_Size);
                ProdDetails.Add(name.Scheme_Quantity);
                ProdDetails.Add(name.Trade_Price);
                ProdDetails.Add(name.Drug_Registration_Number);
                ProdDetails.Add(name.Dosage_Form);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);
          
        }
      
        [HttpPost]
        public JsonResult DeleteProductFromTable(string ProductName)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.DeleteProductFromTableRow(ProductName);

            if (RemUser == "Product Deleted Successfully")
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }

        [HttpPost]
        public JsonResult DeleteProductFromTablePurchaseReturn(string ProductName, string ProductBatch)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.DeleteProductFromPurchaseReturnTableRow(ProductName, ProductBatch);

            if (RemUser == "Product Deleted Successfully")
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }

        [HttpPost]
        public JsonResult DeleteProductFromTablePurchases(string ProductName, string ProductBatch)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.DeleteProductFromPurchasesTableRow(ProductName, ProductBatch);

            if (RemUser == "Product Deleted Successfully")
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }
      
        [HttpPost]
        public JsonResult DeleteProductFromTableInvoiceDC(string ProductName)
        {
            DataLogic DatlogAdm = new DataLogic();

            char[] spearator = {'-'};
            string[] SplitProductBatch = ProductName.Split(spearator);

            string Product = SplitProductBatch[0];
            string Batch = SplitProductBatch[1];

            string RemUser = DatlogAdm.DeleteProductFromInvoiceDCTableRow(Product, Batch);

            if (RemUser == "Product Deleted Successfully")
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");
        }
      
        public JsonResult GetREF()
        {
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchGetREF();
            PurchaseOrderList objListProductView = new PurchaseOrderList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProductOverviewDetail = new List<string>();
           
            foreach (var name in objListProductView.LstReffetch)
            {
                ProductOverviewDetail.Add(name.RefFetch);


            }
            return Json(ProductOverviewDetail, JsonRequestBehavior.AllowGet);

        }
      
        public JsonResult GetInvoiceNum()
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchGetInvoiceNum();
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProductOverviewDetail = new List<string>();

            foreach (var name in objListProductView.LstInvoices)
            {
                ProductOverviewDetail.Add(name.InvoiceNumber);

            }
            return Json(ProductOverviewDetail, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetFinancialOfferNum()
        {
            DataLogic DLOBJ = new DataLogic();
            List<FinancialOffer> InstitutionForAdminData = DLOBJ.FetchGetFinancialOfferNum();
            FinancialOfferList objListProductView = new FinancialOfferList();
            objListProductView.LstFinancialOffer = InstitutionForAdminData;
            var ProductOverviewDetail = new List<string>();

            foreach (var name in objListProductView.LstFinancialOffer)
            {
                ProductOverviewDetail.Add(name.FinancialOfferNum);

            }
            return Json(ProductOverviewDetail, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetRefFromInvoiceNum()
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchGetInvoiceNum();
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProductOverviewDetail = new List<string>();

            foreach (var name in objListProductView.LstInvoices)
            {
                ProductOverviewDetail.Add(name.InvoiceNumber);


            }
            return Json(ProductOverviewDetail, JsonRequestBehavior.AllowGet);

        }

      
        [HttpPost]
        public JsonResult SavePurchaseOrder(List<RoleModel> customers)
        {
            DataAddition DLOBJ = new DataAddition();

            string PlaceOfDelivery = customers[0].PlaceOfDelivery;
            string PurchaseOrderDate = customers[0].PurchaseOrderDate;
            string Remarks = customers[0].Remarks;
            string Ref = customers[0].PurchaseRef;
            string DiscType = customers[0].DiscType;
            string WHT = customers[0].WHT;
            string advTax = customers[0].advTax;
            string companyName = customers[0].CompanyName;
            //............ DataAddition DLOBJ = new DataAddition();
            string ValidatedOverview = DLOBJ.PurchaseOverViewAddition(Ref , PurchaseOrderDate , Remarks , PlaceOfDelivery, DiscType, WHT, advTax, companyName);

            

            for (int i = 0; i < customers.Count; i++)
            {    

               
                string instantRate =  customers[i].InstRate;
                string OurPercente = customers[i].OurPercent;
                string PackValue = customers[i].PackVal;
                string placedelivery = customers[i].PlaceOfDelivery;
                string ProductName = customers[i].ProductName;
                string purchaseorderdate = customers[i].PurchaseOrderDate;
                string Quantity = customers[i].Quanty;
                string RateAfterDiscount = customers[i].RateAfterDiscount;
                string remarks = customers[i].Remarks;
                string Value = customers[i].Value;

                string ValidatedUser = DLOBJ.PurchaseOrderAddition(PackValue , Quantity , instantRate , OurPercente , RateAfterDiscount , Value, ProductName, Ref);

            }

           
            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");
           
        }
      
        [HttpPost]
        public JsonResult addNewRowInPurchaseOrder(List<RoleModel> customers)
        {
            DataAddition DLOBJ = new DataAddition();

            try
            {
                for (int i = 0; i < customers.Count; i++)
                {
                    string Ref = customers[i].PurchaseRef;
                    string companyName = customers[i].CompanyName;
                    string instantRate = customers[i].InstRate;
                    string OurPercente = customers[i].OurPercent;
                    string PackValue = customers[i].PackVal;
                    string placedelivery = customers[i].PlaceOfDelivery;
                    string ProductName = customers[i].ProductName;
                    string purchaseorderdate = customers[i].PurchaseOrderDate;
                    string Quantity = customers[i].Quanty;
                    string RateAfterDiscount = customers[i].RateAfterDiscount;
                    string remarks = customers[i].Remarks;
                    string Value = customers[i].Value;

                    string ValidatedUser = DLOBJ.addNewProductinPurchaseOrder(PackValue, Quantity, instantRate, OurPercente, RateAfterDiscount, Value, companyName, ProductName, Ref);

                }


                foreach (var value in customers)
                {
                    string valuetest = value.ToString();

                }

                if (customers != null)
                {
                    return Json("success");
                    // data = new List<string>();
                }
            }
            //var ProdDetails = new List<string>();
            //Loop and insert records.
            catch (Exception ex) { 
            return Json("fail" + ex.ToString());
            }
            return Json("fail");

        }

        [HttpPost]
        public JsonResult addNewRowInPurchaseReturn(List<SavePurchases> customers)
        {
            DataAddition DLOBJ = new DataAddition();

            try
            {

                for (int i = 0; i < customers.Count; i++)
                {
                    string Ref = customers[i].Ref;
                    string InvoiceDate = customers[i].InvoiceDate;
                    string InvoiceNum = customers[i].InvoiceNum;
                    string Company = customers[i].Company;
                    string returnDate = customers[i].PurchaseReturnDate;
                    string returnType = customers[i].ReturnType;
                    string ProductName = customers[i].ProductName;
                    string PackVal = customers[i].PackVal;
                    string AcUnit = customers[i].AcUnit;
                    string prodQty = customers[i].prodQty;
                    string prodBatch = customers[i].prodBatch;
                    string prodMFG = customers[i].prodMFG;
                    string prodEXP = customers[i].prodEXP;
                    string purchaseRate = customers[i].purchaseRate;
                    string amount = customers[i].amount;

                    string ValidatedUser = DLOBJ.addNewProductinPurchaseReturn(Ref, InvoiceDate, InvoiceNum, Company, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount, returnDate, returnType);

                }


                foreach (var value in customers)
                {
                    string valuetest = value.ToString();

                }


                if (customers != null)
                {
                    return Json("success");
                    // data = new List<string>();
                }
            }

            //Loop and insert records.
            catch (Exception ex)
            {
                return Json("fail" + ex.ToString());
            }
            return Json("fail");
        }

        [HttpPost]
        public JsonResult addNewRowInPurchases(List<SavePurchases> customers)
        {
            DataAddition DLOBJ = new DataAddition();

            try { 

            for (int i = 0; i < customers.Count; i++)
            {
                string Ref = customers[i].Ref;
                string InvoiceDate = customers[i].InvoiceDate;
                string InvoiceNum = customers[i].InvoiceNum;
                string Company = customers[i].Company;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;

                string ValidatedUser = DLOBJ.addNewProductinPurchases(Ref, InvoiceDate, InvoiceNum, Company, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }
        

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            }

            //Loop and insert records.
            catch (Exception ex) { 
            return Json("fail" + ex.ToString());
            }
            return Json("fail");
        }
      
        [HttpPost]
        public JsonResult addNewRowInInvoiceDC(List<SaveInvoices> customers)
        {
            DataAddition DLOBJ = new DataAddition();

            try
            {
               

                for (int i = 0; i < customers.Count; i++)
                {
                    string Ref = customers[i].Ref;
                    string InvoiceDate = customers[i].InvoiceDate;
                    string InvoiceNum = customers[i].InvoiceNum;
                    string CustomerName = customers[i].CustomerName;
                    string ProductName = customers[i].ProductName;
                    string TenderCode = customers[i].TenderCode;
                    string ProdRegNum = customers[i].ProdRegNum;
                    string PackVal = customers[i].PackVal;
                    string AcUnit = customers[i].AcUnit;
                    string prodQty = customers[i].prodQty;
                    string prodBatch = customers[i].prodBatch;
                    string prodMFG = customers[i].prodMFG;
                    string prodEXP = customers[i].prodEXP;
                    string purchaseRate = customers[i].purchaseRate;
                    string amount = customers[i].amount;

                    string[] SplitNum = customers[0].InvoiceNum.Split('-');

                    string UpdatedDC = "DC" + "-" + SplitNum[1] + "-" + SplitNum[2] + "-" + SplitNum[3];

                    string ValidatedUser = DLOBJ.addNewProductinInvoiceDC(Ref, InvoiceDate, InvoiceNum, UpdatedDC, CustomerName, ProductName, TenderCode, ProdRegNum, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

                }


                foreach (var value in customers)
                {
                    string valuetest = value.ToString();

                }


                if (customers != null)
                {
                    return Json("success");
                    // data = new List<string>();
                }
            }

            //Loop and insert records.
            catch (Exception ex)
            {
                return Json("fail" + ex.ToString());
            }
            return Json("fail");
        }
      
        [HttpPost]
        public JsonResult SaveUpdatedPurchaseOrder(List<RoleModel> customers)
        {
            DataAddition DLOBJ = new DataAddition();

            string PlaceOfDelivery = customers[0].PlaceOfDelivery;
            string PurchaseOrderDate = customers[0].PurchaseOrderDate;
            string Remarks = customers[0].Remarks;
            string Ref = customers[0].PurchaseRef;
            string DiscType = customers[0].DiscType;
            string WHT = customers[0].WHT;
            string advTax = customers[0].advTax;
            string companyName = customers[0].CompanyName;
            //............ DataAddition DLOBJ = new DataAddition();
            string ValidatedOverview = DLOBJ.PurchaseOverViewUpdate(Ref, PurchaseOrderDate, Remarks, PlaceOfDelivery, DiscType, WHT, advTax, companyName);



            for (int i = 0; i < customers.Count; i++)
            {
                string PurchaseID = customers[i].PurchaseOrderID;

                string instantRate = customers[i].InstRate;
                string OurPercente = customers[i].OurPercent;
                string PackValue = customers[i].PackVal;
                string placedelivery = customers[i].PlaceOfDelivery;
                string ProductName = customers[i].ProductName;
                string purchaseorderdate = customers[i].PurchaseOrderDate;
                string Quantity = customers[i].Quanty;
                string RateAfterDiscount = customers[i].RateAfterDiscount;
                string remarks = customers[i].Remarks;
                string Value = customers[i].Value;

                string ValidatedUser = DLOBJ.PurchaseOrderUpdate(PurchaseID, PackValue, Quantity, instantRate, OurPercente, RateAfterDiscount, Value, ProductName, Ref);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult frmAddPurchases()
        {
            return View();
        }

        [HttpGet]
        public ActionResult frmAddPurchaseReturn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult frmAddSellReturn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SellReturnsView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchSellReturnViewData();
            InvoiceBillsList objListCompanyView = new InvoiceBillsList();
            objListCompanyView.LstInvoices = InstitutionForAdminData;

            return View("SellReturnsView", objListCompanyView);
        }

        public JsonResult GetAllRef()
        {

           
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchAllRef();
            PurchaseOrderList objListCompanyView = new PurchaseOrderList();
            objListCompanyView.LstReffetch = InstitutionForAdminData;
            var CompaniesList = new List<string>();

            foreach (var name in objListCompanyView.LstReffetch)
            {
                CompaniesList.Add(name.RefForVIew);
                //CompaniesList.Add(name.TBL_Comapny_ID);
            }

            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllPurchasesRef()
        {


            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchAllRefFromPurchases();
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstReffetch = InstitutionForAdminData;
            var CompaniesList = new List<string>();

            foreach (var name in objListCompanyView.LstReffetch)
            {
                CompaniesList.Add(name.Ref);
              
            }

            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetPurchaseOrderRef()
        {

            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchAllRefFromPUrchaseOrders();
            PurchaseOrderList objListCompanyView = new PurchaseOrderList();
            objListCompanyView.LstReffetch = InstitutionForAdminData;
            var CompaniesList = new List<string>();

            foreach (var name in objListCompanyView.LstReffetch)
            {
                CompaniesList.Add(name.RefForVIew);
                //CompaniesList.Add(name.TBL_Comapny_ID);
            }

            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllinvoiceForReturn()
        {

            
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchAllInvoiceForReturn();
            InvoiceBillsList objListCompanyView = new InvoiceBillsList();
            objListCompanyView.LstInvoices = InstitutionForAdminData;
            var CompaniesList = new List<string>();

            foreach (var name in objListCompanyView.LstInvoices)
            {
                CompaniesList.Add(name.InvoiceNumber);
                //CompaniesList.Add(name.TBL_Comapny_ID);
            }

            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetDataBasedOnRef(string Company)
        {
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchCompanyBasedOnRef(Company);
            PurchaseOrderList objListProductView = new PurchaseOrderList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdName.Add(name.CNameForVIew);

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPurchasesDataBasedOnRef(string Company)
        {
            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchCompanyBasedOnPurchasesRef(Company);
            PurchasesList objListProductView = new PurchasesList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdName.Add(name.Company);

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }
  

        [HttpPost]
        public JsonResult GetProductsDataBasedOnCompany(string Company,  string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchProductsDataBasedOnCompany(Company, Ref);
            PurchaseOrderList objListProductView = new PurchaseOrderList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdName.Add(name.ProductName);

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductsDataBasedOnPurchasesCompany(string Company, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchProductsDataBasedOnPurchasesCompany(Company, Ref);
            PurchasesList objListProductView = new PurchasesList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdName.Add(name.ProductName);

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProdDetailsBasedOnCompany(string ProductName, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchProductsDetailsForPurchases(ProductName, Ref);
            PurchaseOrderList objListProductView = new PurchaseOrderList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdDetails.Add(name.Pack_Size);
                ProdDetails.Add(name.Scheme_Quantity);
                ProdDetails.Add(name.Trade_Price);
                ProdDetails.Add(name.ProductValue);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetProdDetailsForPurchaseReturn(string ProductName, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchProductsDetailsFromPurchases(ProductName, Ref);
            PurchasesList objListProductView = new PurchasesList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdDetails.Add(name.InvoiceNum);
                ProdDetails.Add(name.InvoiceDate);
                ProdDetails.Add(name.PackVal);
                ProdDetails.Add(name.AcUnit);
                ProdDetails.Add(name.prodQty);
                ProdDetails.Add(name.prodBatch);
                ProdDetails.Add(name.purchaseRate);
                ProdDetails.Add(name.prodMFG);
                ProdDetails.Add(name.prodEXP);
                ProdDetails.Add(name.amount);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetProdDetailsForPurchases(string ProductName, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchProductsDetailsFromPurchases(ProductName, Ref);
            PurchasesList objListProductView = new PurchasesList();
            objListProductView.LstReffetch = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstReffetch)
            {
                ProdDetails.Add(name.InvoiceNum);
                ProdDetails.Add(name.InvoiceDate);
                ProdDetails.Add(name.PackVal);
                ProdDetails.Add(name.AcUnit);
                ProdDetails.Add(name.prodQty);
                ProdDetails.Add(name.prodBatch);
                ProdDetails.Add(name.purchaseRate);
                ProdDetails.Add(name.prodMFG);
                ProdDetails.Add(name.prodEXP);
                ProdDetails.Add(name.amount);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SavePurchaseReturn(List<SavePurchases> customers)
        {

            DataAddition DLOBJ = new DataAddition();
            try
            {
                string Ref = customers[0].Ref;
                string InvoiceDate = customers[0].InvoiceDate;
                string InvoiceNum = customers[0].InvoiceNum;
                string ReturnDate = customers[0].PurchaseReturnDate;
                string Company = customers[0].Company;
                string ReturnType = customers[0].ReturnType;

                string ValidatedOverview = DLOBJ.PurchaseReturnOverViewAddition(Ref, InvoiceDate, InvoiceNum, ReturnDate, Company, ReturnType);

                for (int i = 0; i < customers.Count; i++)
                {


                    string ProductName = customers[i].ProductName;
                    string PackVal = customers[i].PackVal;
                    string AcUnit = customers[i].AcUnit;
                    string prodQty = customers[i].prodQty;
                    string prodBatch = customers[i].prodBatch;
                    string prodMFG = customers[i].prodMFG;
                    string prodEXP = customers[i].prodEXP;
                    string purchaseRate = customers[i].purchaseRate;
                    string amount = customers[i].amount;


                    string ValidatedUser = DLOBJ.PurchasesReturnAddition(Ref, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

                }


                foreach (var value in customers)
                {
                    string valuetest = value.ToString();

                }

                if (customers != null)
                {
                    return Json("success");
                    // data = new List<string>();
                }
            }
            //var ProdDetails = new List<string>();
            //Loop and insert records.
            catch (Exception ex)
            {
                return Json("fail");
            }
            return Json("fail");
        }

        [HttpPost]
        public JsonResult SavePurchases(List<SavePurchases> customers)
        {
            
            DataAddition DLOBJ = new DataAddition();
            try { 
            string Ref = customers[0].Ref;
            string InvoiceDate = customers[0].InvoiceDate;
            string InvoiceNum = customers[0].InvoiceNum;
            string Company = customers[0].Company;

            string ValidatedOverview = DLOBJ.PurchasesOverViewAddition(Ref, InvoiceDate, InvoiceNum, Company);

            for (int i = 0; i < customers.Count; i++)
            {

                
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;
            

                string ValidatedUser = DLOBJ.PurchasesAddition(Ref, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            }
            //var ProdDetails = new List<string>();
            //Loop and insert records.
            catch (Exception ex) { 
            return Json("fail");
            }
            return Json("fail");
        }

        [HttpPost]
        public JsonResult SaveUpdatedPurchaseReturn(List<SavePurchases> customers)
        {
            DataAddition DLOBJ = new DataAddition();


            string Ref = customers[0].Ref;
            string InvoiceDate = customers[0].InvoiceDate;
            string InvoiceNum = customers[0].InvoiceNum;
            string Company = customers[0].Company;
            string returnDate = customers[0].PurchaseReturnDate;
            string returnType = customers[0].ReturnType;

            string ValidatedOverview = DLOBJ.PurchaseReturnOverViewUpdation(Ref, InvoiceNum, InvoiceDate, Company, returnDate, returnType);

            for (int i = 0; i < customers.Count; i++)
            {
                string PurchaseReturnID = customers[i].PurchaseReturnID;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;


                string ValidatedUser = DLOBJ.PurchaseReturnUpdate(PurchaseReturnID, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }

        [HttpPost]
        public JsonResult SaveUpdatedPurchases(List<SavePurchases> customers)
        {
            DataAddition DLOBJ = new DataAddition();

           
            string Ref = customers[0].Ref;
            string InvoiceDate = customers[0].InvoiceDate;
            string InvoiceNum = customers[0].InvoiceNum;
            string Company = customers[0].Company;

            string ValidatedOverview = DLOBJ.PurchasesOverViewUpdation(Ref, InvoiceNum, InvoiceDate, Company);

            for (int i = 0; i < customers.Count; i++)
            {
                string PurchasesID = customers[i].PurchasesID;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;


                string ValidatedUser = DLOBJ.PurchasesUpdate(PurchasesID, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }

        [HttpGet]
        public ActionResult PurchaseReturnsView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchPurchaseReturnViewData();
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstPurchases = InstitutionForAdminData;

            return View("PurchaseReturnsView", objListCompanyView);
        }

        [HttpPost]
        public ActionResult RemovePurchaseReturn(string Remove, string Ref)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemovePurchaseReturnData(Remove, Ref);




            if (RemUser == "Purchase Return Deleted Successfully")
            {
                ViewBag.Message = "Purchase Return Deleted Successfully";
                return View("Dashboard");
            }


            else if (RemUser.Contains("Purchase Return cannot be deleted"))
            {
                ViewBag.Message = "Error Purchase Return not deleted see inner exception";
                return View("Dashboard");
            }
            return View("Dashboard");


        }

        [HttpGet]
        public ActionResult EditPurchaseReturn(string Edit)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<Purchases> EditPO = DatlogAdm.FetchPurchaseReturnForEdit(Edit);
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstPurchases = EditPO;

            List<Purchases> EditPO2 = DatlogAdm.FetchPurchaseReturnForEdit2(Edit);
            objListCompanyView.LstPurchasesDetails = EditPO2;


            return View("EditPurchaseReturn", objListCompanyView);
        }

        [HttpGet]
        public ActionResult PrintPurchaseReturn(string Print)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<Purchases> EditPO = DatlogAdm.FetchPurchaseReturnForPrint(Print);
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstPurchases = EditPO;

            List<Purchases> EditPO2 = DatlogAdm.FetchPurchaseReturnForPrint2(Print);
            objListCompanyView.LstPurchasesDetails = EditPO2;


            return View("PrintPurchaseReturn", objListCompanyView);
        }


        [MyAuthorize]
        [HttpGet]
        public ActionResult PurchasesView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<Purchases> InstitutionForAdminData = DLOBJ.FetchPurchasesViewData();
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstPurchases = InstitutionForAdminData;

            return View("PurchasesView", objListCompanyView);
        }

      
        [HttpPost]
        public ActionResult RemovePurchases(string Remove, string Ref)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemovePurchasesData(Remove, Ref);




            if (RemUser == "Purchases Deleted Successfully")
            {
                ViewBag.Message = "Purchases Deleted Successfully";
                return View("Dashboard");
            }

            else if (RemUser.Contains("Please remove Invoice First"))
            {
                ViewBag.Message = "Error Please remove Invoice First";
                return View("Dashboard");
            }

            else if (RemUser.Contains("Purchases cannot be deleted"))
            {
                ViewBag.Message = "Error Purchases not deleted see inner exception";
                return View("Dashboard");
            }
            return View("Dashboard");


        }
      
        [HttpGet]
        public ActionResult EditPurchases(string Edit)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<Purchases> EditPO = DatlogAdm.FetchPurchasesForEdit(Edit);
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstPurchases = EditPO;

            List<Purchases> EditPO2 = DatlogAdm.FetchPurchasesForEdit2(Edit);
            objListCompanyView.LstPurchasesDetails = EditPO2;


            return View("EditPurchases", objListCompanyView);
        }
      
        [HttpGet]
        public ActionResult PrintPurchases(string Print)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<Purchases> EditPO = DatlogAdm.FetchPurchasesForPrint(Print);
            PurchasesList objListCompanyView = new PurchasesList();
            objListCompanyView.LstPurchases = EditPO;

            List<Purchases> EditPO2 = DatlogAdm.FetchPurchasesForPrint2(Print);
            objListCompanyView.LstPurchasesDetails = EditPO2;


            return View("PrintPurchases", objListCompanyView);
        }
      
        [HttpPost]
        public JsonResult GetStates1(string country)
        {
            var States = new List<string>();
            if (!string.IsNullOrWhiteSpace(country))
            {
                if (country.Equals("test"))
                {
                    States.Add("Sydney");
                    States.Add("Perth");
                }
                if (country.Equals("Delhi"))
                {
                    States.Add("Delhi");
                    States.Add("Mumbai");
                }
                if (country.Equals("Russia"))
                {
                    States.Add("Minsk");
                    States.Add("Moscow");
                }
            }
            return Json(States, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult Dashboard()
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult frmFinancialOffer()
        {
            return View();
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult frmFinancialOffer(string CustName, string OfferTitle, string OfferDueDate, string GenericName, string BrandName, string ManufacturerName, int ApproxQuantity, float tradePriceUnit, float bidRateUnit, float PercentAmount)
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult FinancialOffersView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<FinancialOffer> InstitutionForAdminData = DLOBJ.FetchFinancialOfferViewData();
            FinancialOfferList objListCompanyView = new FinancialOfferList();
            objListCompanyView.LstFinancialOffer = InstitutionForAdminData;

            return View("FinancialOffersView", objListCompanyView);
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult TechnicalOffersView()
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult formTender()
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult frmPurchaseOrder()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<ComapnyViewModel> InstitutionForAdminData = DLOBJ.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = InstitutionForAdminData;

            List<GroupViewModel> GroupData = DLOBJ.FetchProductData();
            objListCompanyView.LstGroupList = GroupData;

            return View("frmPurchaseOrder", objListCompanyView);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult frmPurchaseOrder(string demo)
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult PurchaseOrdersView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<PurchaseOrder> InstitutionForAdminData = DLOBJ.FetchPurchaseOrderViewData();
            PurchaseOrderList objListCompanyView = new PurchaseOrderList();
            objListCompanyView.LstReffetch = InstitutionForAdminData;

            return View("PurchaseOrdersView", objListCompanyView);
        }
      
        [HttpPost]
        public ActionResult RemovePurchaseOrder(string Remove, string Ref)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemovePurchaseOrderData(Remove, Ref);

           


            if (RemUser == "Purchase Order Deleted Successfully")
            {
                ViewBag.Message = "Purchase Order Deleted Successfully";
                return View("Dashboard");
            }

            else if (RemUser.Contains("Please remove Purchases First"))
            {
                ViewBag.Message = "Error Please remove Purchases First";
                return View("Dashboard");
            }

            else if (RemUser.Contains("Purchase Order cannot be deleted"))
            {
                ViewBag.Message = "Error Purchase Order not deleted see inner exception";
                return View("Dashboard");
            }
            return View("Dashboard");

       
        }
      
        [HttpGet]
        public ActionResult EditPurchaseOrder(string Edit)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<PurchaseOrder> EditPO = DatlogAdm.FetchPurchaseOrderForEdit(Edit);
            PurchaseOrderList objListCompanyView = new PurchaseOrderList();
            objListCompanyView.LstReffetch = EditPO;

            List<PurchaseOrder> EditPO2 = DatlogAdm.FetchPurchaseOrderForEdit2(Edit);
            objListCompanyView.LstPODataFetch = EditPO2;


            return View("EditPurchaseOrder", objListCompanyView);
            
        }
      
        [HttpGet]
        public ActionResult PrintPurchaseOrder(string Print)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<PurchaseOrder> EditPO = DatlogAdm.FetchPurchaseOrderForPrint(Print);
            PurchaseOrderList objListCompanyView = new PurchaseOrderList();
            objListCompanyView.LstReffetch = EditPO;

            List<PurchaseOrder> EditPO2 = DatlogAdm.FetchPurchaseOrderForPrint2(Print);
            objListCompanyView.LstPODataFetch = EditPO2;


            return View("PrintPurchaseOrder", objListCompanyView);
        }
      
        [HttpGet]
   
        public ActionResult frmDeliveryChallan()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<CustomerViewModel> InstitutionForAdminData = DLOBJ.FetchCustomerData();
            CustomerViewModelList objListCompanyView = new CustomerViewModelList();
            objListCompanyView.LstCustomerList = InstitutionForAdminData;

            return View("frmDeliveryChallan", objListCompanyView);
        }
      
        [HttpPost]

        public ActionResult frmDeliveryChallan(string demo)
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult DeliveryChallanView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<DeliveryChallan> InstitutionForAdminData = DLOBJ.FetchDCViewData();
            DeliveryChallanList objListCompanyView = new DeliveryChallanList();
            objListCompanyView.LstDeliveryChallan = InstitutionForAdminData;

            return View("DeliveryChallanView", objListCompanyView);
        }


      
        [HttpPost]
        public ActionResult RemoveDeliveryChallan(string Remove)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemoveDeliveryChallanData(Remove);




            if (RemUser == "DC Deleted Successfully")
            {
                ViewBag.Message = "DC Deleted Successfully";
                return View("Dashboard");
            }

            else if (RemUser.Contains("DC cannot be deleted"))
            {
                ViewBag.Message = "Error DC not deleted see inner exception";
                return View("Dashboard");
            }
            return View("Dashboard");
        }
      
        [HttpGet]
        public ActionResult EditDeliveryChallan(string Edit)
        {
            DataLogic DatlogAdm = new DataLogic();
            List<DeliveryChallan> EditPO = DatlogAdm.FetchDCForEdit(Edit);
            DeliveryChallanList objListCompanyView = new DeliveryChallanList();
            objListCompanyView.LstDeliveryChallan = EditPO;

            List<DeliveryChallan> EditPO2 = DatlogAdm.FetchDCForEdit2(Edit);
            objListCompanyView.LstDeliveryChallanDetails = EditPO2;


            return View("EditDeliveryChallan", objListCompanyView);
        }
      
        [HttpGet]
        public ActionResult PrintDeliveryChallan(string Print)
        {
            DataLogic DatlogAdm = new DataLogic();
            List<DeliveryChallan> EditPO = DatlogAdm.FetchDCForPrint(Print);
            DeliveryChallanList objListCompanyView = new DeliveryChallanList();
            objListCompanyView.LstDeliveryChallan = EditPO;

            List<DeliveryChallan> EditPO2 = DatlogAdm.FetchDCForPrint2(Print);
            objListCompanyView.LstDeliveryChallanDetails = EditPO2;


            return View("PrintDeliveryChallan", objListCompanyView);
        }
      [MyAuthorize]
        [HttpGet]
        public ActionResult frmInvoiceBills()
        {
           
            return View();
        }
      [MyAuthorize]
        [HttpPost]
        public ActionResult frmInvoiceBills(string demo)
        {
            return View();
        }



        public JsonResult GetAllCustomers()
        {

            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchAllCustomers();
            InvoiceBillsList objListCompanyView = new InvoiceBillsList();
            objListCompanyView.LstInvoices = InstitutionForAdminData;
            var CompaniesList = new List<string>();

            foreach (var name in objListCompanyView.LstInvoices)
            {
                CompaniesList.Add(name.CustomeID + "." + name.CustomerName + "," + name.Address1);
              
                
                //CompaniesList.Add(name.TBL_Comapny_ID);
            }

            return Json(CompaniesList, JsonRequestBehavior.AllowGet);

        }
      
        [HttpPost]
        public JsonResult GetDeliveryChallanDetails(string Company)
        {
            //This Method is used to fetch OrderDate and DC Number from InvoicesTable to be used in DeliveryChallan
            DataLogic DLOBJ = new DataLogic();
            List<DeliveryChallan> InstitutionForAdminData = DLOBJ.FetchDeliveryChallanDetails(Company);
            DeliveryChallanList objListProductView = new DeliveryChallanList();
            objListProductView.LstDeliveryChallan = InstitutionForAdminData;
            var ProdName = new List<string>();

            foreach (var name in objListProductView.LstDeliveryChallan)
            {
                ProdName.Add(name.InvoiceDate);
                ProdName.Add(name.InvoiceNumber);

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public JsonResult GetDeliveryDataBasedOnRef(string Company)
        {
            DataLogic DLOBJ = new DataLogic();
            List<DeliveryChallan> InstitutionForAdminData = DLOBJ.FetchDeliveryDataBasedOnRef(Company);
            DeliveryChallanList objListProductView = new DeliveryChallanList();
            objListProductView.LstDeliveryChallan = InstitutionForAdminData;
            var ProdName = new List<string>();
        
            foreach (var name in objListProductView.LstDeliveryChallan)
            {
                ProdName.Add(name.ProductBrandName);
             
            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceRef(string Company)
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchInvoiceRef(Company);
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstInvoices)
            {
                ProdName.Add(name.Ref);


            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceDataBasedOnInvoiceNum(string Company, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchInvoiceDataBasedOnInv(Company, Ref);
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstInvoices)
            {
                ProdName.Add(name.ProductBrandName + " ~ " + name.Batch);
               

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceDataBasedOnRef(string Company)
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchInvoiceDataBasedOnRef(Company);
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProdName = new List<string>();
            foreach (var name in objListProductView.LstInvoices)
            {
                ProdName.Add(name.ProductBrandName + " ~ " + name.Batch);

            }
            return Json(ProdName, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetProdDetailsForDelivery(string ProductName, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<DeliveryChallan> InstitutionForAdminData = DLOBJ.FetchProductsDetailsForDelivery(ProductName, Ref);
            DeliveryChallanList objListProductView = new DeliveryChallanList();
            objListProductView.LstDeliveryChallan = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstDeliveryChallan)
            {
                ProdDetails.Add(name.Pack);
                ProdDetails.Add(name.AcUnit);
                ProdDetails.Add(name.PurchasesQty);
                ProdDetails.Add(name.Batch);
                ProdDetails.Add(name.MFG);
                ProdDetails.Add(name.EXP);
                ProdDetails.Add(name.TenderCode);
                ProdDetails.Add(name.ProdRegNum);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult getInvoiceDetails(string invNumber)
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchDetailsForInvoices(invNumber);
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstInvoices)
            {
                ProdDetails.Add(name.CustomeID);
                ProdDetails.Add(name.CustomerName);
                ProdDetails.Add(name.Address1);
                ProdDetails.Add(name.Address2);
                ProdDetails.Add(name.CustOrderNum);
                ProdDetails.Add(name.InvoiceDate);
                ProdDetails.Add(name.OrderDate);
                ProdDetails.Add(name.InvoiceType);
                ProdDetails.Add(name.Warranty);
                ProdDetails.Add(name.AdvTax);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetProdDetailsForInvoiceBasedOnInv(string ProductName, string InvoiceNum, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchProductsDetailsForInvoicesWithInvNum(ProductName, InvoiceNum, Ref);
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstInvoices)
            {
                ProdDetails.Add(name.TenderCode);
                ProdDetails.Add(name.ProdRegNum);
                ProdDetails.Add(name.Pack);
                ProdDetails.Add(name.AcUnit);
                ProdDetails.Add(name.PurchasesQty);
                ProdDetails.Add(name.Batch);
                ProdDetails.Add(name.MFG);
                ProdDetails.Add(name.EXP);
                ProdDetails.Add(name.PurchaseRate);
                ProdDetails.Add(name.Amout);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetProdDetailsForInvoice(string ProductName, string Ref)
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchProductsDetailsForInvoices(ProductName, Ref);
            InvoiceBillsList objListProductView = new InvoiceBillsList();
            objListProductView.LstInvoices = InstitutionForAdminData;
            var ProdDetails = new List<string>();
            foreach (var name in objListProductView.LstInvoices)
            {
                ProdDetails.Add(name.Pack);
                ProdDetails.Add(name.drugProdregNum);
                ProdDetails.Add(name.AcUnit);
                ProdDetails.Add(name.PurchasesQty);
                ProdDetails.Add(name.Batch);
                ProdDetails.Add(name.MFG);
                ProdDetails.Add(name.EXP);
                ProdDetails.Add(name.PurchaseRate);
                ProdDetails.Add(name.Amout);

            }
            return Json(ProdDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveFinancialOffer(List<FinancialOffer> customers)
        {

            DataAddition DLOBJ = new DataAddition();




            string FinancialOfferNum = customers[0].FinancialOfferNum;
            string CustomerName = customers[0].CustomerName;
            string Percent = customers[0].PercentageOfQuotedItems;
            string Desc1 = customers[0].Desc1;
            string Desc2 = customers[0].Desc2;
            string Desc3 = customers[0].Desc3;
            string Notes1 = customers[0].Notes1;
            string Notes2 = customers[0].Notes2;
            string Notes3 = customers[0].Notes3;

            string ValidatedOverview = DLOBJ.FinancialOfferAddition(FinancialOfferNum, CustomerName, Desc1, Desc2, Desc3, Notes1, Notes2, Notes3);

            for (int i = 0; i < customers.Count; i++)
            {


                string itemCode = customers[i].ItemCode;
                string AcUnit = customers[i].AcUnit;
                string TenderQuntity = customers[i].TenderQuntity;
                string Pack = customers[i].Pack;
                string drugProdregNum = customers[i].drugProdregNum;
                string ProductName = customers[i].Product_Brand_Name;
                string TradePrice = customers[i].TradePrice;
                string CustBidRate = customers[i].CustBidRate;
                string TotalAmount = customers[i].TotalAmount;
                string PercentCalculated = customers[i].PercentCalculated;
           

                string ValidatedUser = DLOBJ.FinancialOfferOverviewAddition(Percent, FinancialOfferNum, itemCode, AcUnit, TenderQuntity, Pack, drugProdregNum, ProductName, TradePrice, CustBidRate, TotalAmount, PercentCalculated);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }

        [HttpPost]
        public JsonResult SaveSellReturn(List<SaveInvoices> customers)
        {

            DataAddition DLOBJ = new DataAddition();
            
            string OrderDate = customers[0].OrderDate;
            string InvoiceNum = customers[0].InvoiceNum;
            string CustomerName = customers[0].CustomerName;
            string InvoiceDate = customers[0].InvoiceDate;
            string InvoiceType = customers[0].InvoiceType;
            string Warranty = customers[0].Warranty;
            string CustOrderNum = customers[0].CustOrderNum;
            string ReturnDate = customers[0].SellReturnDate;
            string ReturnType = customers[0].ReturnType;
            string advTax = customers[0].AdvTax;

            string[] SplitNum = customers[0].InvoiceNum.Split('-');

            string UpdatedDC = "DC" + "-" + SplitNum[1] + "-" + SplitNum[2] + "-" + SplitNum[3];

            string ValidatedOverview = DLOBJ.SellReturnOverViewAddition(OrderDate, InvoiceNum, UpdatedDC, CustomerName, InvoiceDate, InvoiceType, Warranty, CustOrderNum, advTax, ReturnDate, ReturnType);

            for (int i = 0; i < customers.Count; i++)
            {
                
                string Ref = customers[i].Ref;
                string TenderCode = customers[i].TenderCode;
                string ProdRegNum = customers[i].ProdRegNum;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;
                
                string ValidatedUser = DLOBJ.SellReturnAddition(Ref, OrderDate, InvoiceNum, UpdatedDC, CustomerName, InvoiceDate, InvoiceType, Warranty, CustOrderNum, TenderCode, ProdRegNum, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }

        [HttpPost]
        public JsonResult SaveInvoices(List<SaveInvoices> customers)
        {

            DataAddition DLOBJ = new DataAddition();
            
            string OrderDate = customers[0].OrderDate;
            string InvoiceNum = customers[0].InvoiceNum;
            string CustomerName = customers[0].CustomerName;
            string InvoiceDate = customers[0].InvoiceDate;
            string InvoiceType = customers[0].InvoiceType;
            string Warranty = customers[0].Warranty;
            string CustOrderNum = customers[0].CustOrderNum;
            string advTax = customers[0].AdvTax;

            string[] SplitNum = customers[0].InvoiceNum.Split('-');
           
            string UpdatedDC = "DC" + "-" + SplitNum[1] + "-" + SplitNum[2] + "-" + SplitNum[3];

            string ValidatedOverview = DLOBJ.InvoiceOverViewAddition(OrderDate, InvoiceNum, UpdatedDC, CustomerName, InvoiceDate, InvoiceType, Warranty, CustOrderNum, advTax);
            
            for (int i = 0; i < customers.Count; i++)
            {
                
                string Ref = customers[i].Ref;
                string TenderCode = customers[i].TenderCode;
                string ProdRegNum = customers[i].ProdRegNum;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;
                
                string ValidatedUser = DLOBJ.InvoiceAddition(Ref, OrderDate, InvoiceNum, UpdatedDC, CustomerName, InvoiceDate, InvoiceType, Warranty, CustOrderNum, TenderCode, ProdRegNum, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }
      
        [HttpPost]
        public JsonResult SaveDeliveryChallan(List<SaveDeliveryChallan> customers)
        {

            DataAddition DLOBJ = new DataAddition();

            for (int i = 0; i < customers.Count; i++)
            {

                string Ref = customers[i].Ref;
                string OrderDate = customers[i].OrderDate;
                string DcNum = customers[i].DcNum;
                string CustomerName = customers[i].CustomerName;
                string DcDate = customers[i].DcDate;
                string DcType = customers[i].DcType;
                string Warranty = customers[i].Warranty;
                string TenderCode = customers[i].TenderCode;
                string ProdRegNum = customers[i].ProdRegNum;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
            


                string ValidatedUser = DLOBJ.DeliveryChallanAddition(Ref, OrderDate, DcNum, CustomerName, DcDate, DcType, Warranty, TenderCode, ProdRegNum, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }
      
        [HttpPost]
        public JsonResult SaveUpdatedDeliveryChallan(List<SaveDeliveryChallan> customers)
        {
            
                DataAddition DLOBJ = new DataAddition();

            string OrderDate = customers[0].OrderDate;
            string DcNum = customers[0].DcNum;
            string CustomerName = customers[0].CustomerName;
            string DcDate = customers[0].DcDate;
            string DcType = customers[0].DcType;
            string Warranty = customers[0].Warranty;

            string ValidatedOverview = DLOBJ.DCOverViewUpdate(OrderDate, DcNum, CustomerName, DcDate, DcType, Warranty);

            //for (int i = 0; i < customers.Count; i++)
            //{

            //    string DeliveryChallanID = customers[i].DeliveryChallanID;
            //    string Ref = customers[i].Ref;
             
            //    string TenderCode = customers[i].TenderCode;
            //    string ProdRegNum = customers[i].ProdRegNum;
            //    string ProductName = customers[i].ProductName;
            //    string PackVal = customers[i].PackVal;
            //    string AcUnit = customers[i].AcUnit;
            //    string prodQty = customers[i].prodQty;
            //    string prodBatch = customers[i].prodBatch;
            //    string prodMFG = customers[i].prodMFG;
            //    string prodEXP = customers[i].prodEXP;



            //    string ValidatedUser = DLOBJ.DeliveryChallanUpdate(DeliveryChallanID, Ref, OrderDate, DcNum, CustomerName, DcDate, DcType, Warranty, TenderCode, ProdRegNum, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP);

            //}


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult InvoiceView()
        {
            DataLogic DLOBJ = new DataLogic();
            List<InvoiceBills> InstitutionForAdminData = DLOBJ.FetchInvoicesViewData();
            InvoiceBillsList objListCompanyView = new InvoiceBillsList();
            objListCompanyView.LstInvoices = InstitutionForAdminData;

            return View("InvoiceView", objListCompanyView);
        }

        [HttpPost]
        public ActionResult RemoveSellReturn(string Remove)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemoveSellReturnData(Remove);


            if (RemUser == "Sell Return Deleted Successfully")
            {
                ViewBag.Message = "Sell Return Successfully";
                return View("Dashboard");
            }

          

            else if (RemUser.Contains("Sell Return cannot be deleted"))
            {
                ViewBag.Message = "Error Sell Return not deleted see inner exception";
                return View("Dashboard");
            }
            return View("Dashboard");
        }

        [HttpPost]
        public ActionResult RemoveInvoices(string Remove)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemoveInvoiceData(Remove);
            

            if (RemUser == "Invoice Deleted Successfully")
            {
                ViewBag.Message = "Invoice Deleted Successfully";
                return View("Dashboard");
            }

            else if (RemUser.Contains("Please remove DC First"))
            {
                ViewBag.Message = "Error Please remove DC First";
                return View("Dashboard");
            }

            else if (RemUser.Contains("Invoice cannot be deleted"))
            {
                ViewBag.Message = "Error Invoice not deleted see inner exception";
                return View("Dashboard");
            }
            return View("Dashboard");
        }
      
        [HttpGet]
        public ActionResult EditInvoices(string Edit)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<InvoiceBills> EditInvoice = DatlogAdm.FetchInvoiceForEdit(Edit);
            InvoiceBillsList objListCompanyView = new InvoiceBillsList();
            objListCompanyView.LstInvoices = EditInvoice;

            List<InvoiceBills> EditInvoice2 = DatlogAdm.FetchInvoiceForEdit2(Edit);
            objListCompanyView.LstInvoicesDetails = EditInvoice2;


            return View("EditInvoices", objListCompanyView);
        }
      
        [HttpPost]
        public JsonResult SaveUpdatedInvoiceBills(List<SaveInvoices> customers)
        {

            DataAddition DLOBJ = new DataAddition();

            
         
            string OrderDate = customers[0].OrderDate;
            string InvoiceNum = customers[0].InvoiceNum;
            string CustomerName = customers[0].CustomerName;
            string InvoiceDate = customers[0].InvoiceDate;
            string InvoiceType = customers[0].InvoiceType;
            string Warranty = customers[0].Warranty;
            string CustOrderNum = customers[0].CustOrderNum;
            string advTax = customers[0].AdvTax;

            string[] SplitNum = customers[0].InvoiceNum.Split('-');

            string UpdatedDC = "DC" + "-" + SplitNum[1] + "-" + SplitNum[2] + "-" + SplitNum[3];

            string ValidatedOverview = DLOBJ.InvoiceOverViewUpdate(OrderDate, InvoiceNum, UpdatedDC, CustomerName, InvoiceDate, InvoiceType, Warranty, CustOrderNum, advTax);

            for (int i = 0; i < customers.Count; i++)
            {
                string Ref = customers[i].Ref;
                string InvoiceBillsID = customers[i].InvoiceBillsID;
                string TenderCode = customers[i].TenderCode;
                string ProdRegNum = customers[i].ProdRegNum;
                string ProductName = customers[i].ProductName;
                string PackVal = customers[i].PackVal;
                string AcUnit = customers[i].AcUnit;
                string prodQty = customers[i].prodQty;
                string prodBatch = customers[i].prodBatch;
                string prodMFG = customers[i].prodMFG;
                string prodEXP = customers[i].prodEXP;
                string purchaseRate = customers[i].purchaseRate;
                string amount = customers[i].amount;


                string ValidatedUser = DLOBJ.InvoiceUpdate(InvoiceBillsID, Ref, OrderDate, CustOrderNum, InvoiceNum, UpdatedDC, CustomerName, InvoiceDate, InvoiceType, Warranty, TenderCode, ProdRegNum, ProductName, PackVal, AcUnit, prodQty, prodBatch, prodMFG, prodEXP, purchaseRate, amount);

            }


            foreach (var value in customers)
            {
                string valuetest = value.ToString();

            }

            if (customers != null)
            {
                return Json("success");
                // data = new List<string>();
            }
            var ProdDetails = new List<string>();
            //Loop and insert records.

            return Json("fail");

        }
      
        [HttpGet]
        public ActionResult PrintInvoices(string Print)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<InvoiceBills> EditPO = DatlogAdm.FetchInvoicesForPrint(Print);
            InvoiceBillsList objListCompanyView = new InvoiceBillsList();
            objListCompanyView.LstInvoices = EditPO;

            List<InvoiceBills> EditPO2 = DatlogAdm.FetchInvoiceForPrint2(Print);
            objListCompanyView.LstInvoicesDetails = EditPO2;


            return View("PrintInvoices", objListCompanyView);
        }

        [HttpGet]
        public ActionResult PrintFinancialOffer(string Print)
        {

            DataLogic DatlogAdm = new DataLogic();
            List<FinancialOffer> EditPO = DatlogAdm.FetchFinancialOfferForPrint(Print);
            FinancialOfferList objListCompanyView = new FinancialOfferList();
            objListCompanyView.LstFinancialOffer = EditPO;

            List<FinancialOffer> EditPO2 = DatlogAdm.FetchFinancialOfferForPrint2(Print);
            objListCompanyView.LstFinancialOfferDetails = EditPO2;


            return View("PrintFinancialOffer", objListCompanyView);
        }

        [HttpPost]
        public ActionResult formTender(string customerName, int invoiceNo, string date, int tendorNo, string finanYear, string dueDate, string tenderCity, string tenderState)
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult TenderView()
        {
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult formAddCustomer()
        {
            return View();
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult formAddCustomer(string custName, string custCountry, string ContPerson1, string ContPerson2, string CustPhone1, string CustPhone2, string CustEmail1, string CustEmail2, string PayTerms, string custAddress1, string custAddress2)
        {


            DataAddition DLOBJ = new DataAddition();
            string ValidatedUser = DLOBJ.CustomerAddition(custName, custCountry, ContPerson1, ContPerson2, CustPhone1, CustPhone2, CustEmail1, CustEmail2, PayTerms, custAddress1, custAddress2);


            if (ValidatedUser.Contains("Successfully"))
            {
                ViewBag.Message = "Customer added successfully";
                //return View("", );
            }

            else if (ValidatedUser.Contains("Failed"))
            {
                ViewBag.Message = "Error Customer not added see inner exception";
                // return View("", );
            }
            return View();
            //return View("", );
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult CustomerView()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<CustomerViewModel> InstitutionForAdminData = DLOBJ.FetchCustomerData();
            CustomerViewModelList objListCompanyView = new CustomerViewModelList();
            objListCompanyView.LstCustomerList = InstitutionForAdminData;

            return View("CustomerView", objListCompanyView);
        }
      
        [HttpPost]
        public ActionResult RemoveCustomer(string Remove)
        {
            
            
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemoveCustomer(Remove);

            DataLogic DataLogicObj = new DataLogic();
            List<CustomerViewModel> CompData = DataLogicObj.FetchCustomerData();
            CustomerViewModelList objListCompanyView = new CustomerViewModelList();
            objListCompanyView.LstCustomerList = CompData;

            if (RemUser == "Customer Deleted Successfully")
            {
                ViewBag.Message = "Customer Deleted Successfully";
                return View("CustomerView", objListCompanyView);
            }

            else if (RemUser == "Invoice present")
            {
                ViewBag.Message = "Customer Cannot be deleted as Invoice is created against this customer";
                return View("CustomerView", objListCompanyView);
            }

            else if (RemUser.Contains("Customer cannot be deleted"))
            {
                ViewBag.Message = "Error customer cannot be deleted see inner exception";
                return View("CustomerView", objListCompanyView);
            }
            return View("CustomerView", objListCompanyView);
        }
      
        [HttpGet]
        public ActionResult EditCustomer(string Edit)
        {
            
            DataLogic DatlogAdm = new DataLogic();
            List <CustomerViewModel> EditUser = DatlogAdm.FetchCustomerForEdit(Edit);
            CustomerViewModelList objListCompanyView = new CustomerViewModelList();
            objListCompanyView.LstCustomerList = EditUser;
            
            return View("EditCustomer", objListCompanyView);
        }
      
        [HttpPost]
        public ActionResult EditCustomer(string custID, string custName, string custCountry, string ContPerson1, string ContPerson2, string CustPhone1, string CustPhone2, string CustEmail1, string CustEmail2, string PayTerms, string custAddress1, string custAddress2)
        {
            
            

            DataAddition DAOBJ = new DataAddition();
            string ValidatedUser = DAOBJ.CustomerUpdate(custID, custName, custCountry, ContPerson1, ContPerson2, CustPhone1, CustPhone2, CustEmail1, CustEmail2, PayTerms, custAddress1, custAddress2);
            
            if (ValidatedUser.Contains("Successfully"))
            {
              
              
                ViewBag.Message = "Customer updated successfully";
                //return View("", );
            }

            else if (ValidatedUser.Contains("Failed"))
            {
                ViewBag.Message = "Error Customer not updated see inner exception";
                // return View("", );
            }

            DataLogic DLOBJ = new DataLogic();
            List<CustomerViewModel> InstitutionForAdminData = DLOBJ.FetchCustomerData();
            CustomerViewModelList objListCompanyView = new CustomerViewModelList();
            objListCompanyView.LstCustomerList = InstitutionForAdminData;
            return View("CustomerView", objListCompanyView);
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult formAddCompany()
        {
            return View();
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult formAddCompany(string compName, string compCountry, string CompContPerson1, string CompContPerson2, string CompPhone1, string CompPhone2, string CompEmail1, string CompEmail2, string CompPayTerms, string CompAddress1, string CompAddress2)
        {

            DataAddition DLOBJ = new DataAddition();
            string ValidatedUser = DLOBJ.CompanyAddition(compName, compCountry, CompContPerson1, CompContPerson2, CompPhone1, CompPhone2, CompEmail1, CompEmail2, CompPayTerms, CompAddress1, CompAddress2);


            if (ValidatedUser.Contains("Successfully"))
            {
                ViewBag.Message = "Company added successfully";
                //return View("", );
            }

            else if (ValidatedUser.Contains("Failed"))
            {
                ViewBag.Message = "Error Company not added see inner exception";
                // return View("", );
            }
            return View();

        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult CompanyView()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<ComapnyViewModel> InstitutionForAdminData = DLOBJ.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = InstitutionForAdminData;

            return View("CompanyView", objListCompanyView);
        }
   
        [HttpGet]
        public ActionResult EditCompany(string Edit)
        {
            DataLogic DatlogAdm = new DataLogic();
            List<ComapnyViewModel> EditCompany = DatlogAdm.FetchCompanyForEdit(Edit);
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = EditCompany;

            return View("EditCompany", objListCompanyView);
        }
   
        [HttpPost]
        public ActionResult EditCompany(string CompID, string compName, string compCountry, string CompContPerson1, string CompContPerson2, string CompPhone1, string CompPhone2, string CompEmail1, string CompEmail2, string CompPayTerms, string CompAddress1, string CompAddress2)
        {
            DataAddition DAOBJ = new DataAddition();
            string ValidatedUser = DAOBJ.CompanyUpdate(CompID, compName, compCountry, CompContPerson1, CompContPerson2, CompPhone1, CompPhone2, CompEmail1, CompEmail2, CompPayTerms, CompAddress1, CompAddress2);

            if (ValidatedUser.Contains("Successfully"))
            {


                ViewBag.Message = "Company updated successfully";
                //return View("", );
            }

            else if (ValidatedUser.Contains("Failed"))
            {
                ViewBag.Message = "Error Company not updated see inner exception";
                // return View("", );
            }

            
            DataLogic DLOBJ = new DataLogic();
            List<ComapnyViewModel> InstitutionForAdminData = DLOBJ.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = InstitutionForAdminData;

            return View("CompanyView", objListCompanyView);
        }
   
        [HttpPost]
        public ActionResult RemoveCompany(string Remove)
        {
            DataLogic DatlogAdm = new DataLogic();
            bool RemUser = DatlogAdm.RemoveCompany(Remove);

            DataLogic DataLogicObj = new DataLogic();
            List<ComapnyViewModel> CompData = DataLogicObj.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = CompData;


            if (RemUser == true)
            {
                ViewBag.Message = "Company Deleted Successfully";
                return View("CompanyView", objListCompanyView);
            }

            else if (RemUser == false)
            {
                ViewBag.Message = "Company cannot be deleted, Delete Product(s) first or see inner exception";
                return View("CompanyView", objListCompanyView);
            }
            return View("CompanyView", objListCompanyView);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult frmAddProduct(string CompanySelect, string prodGenericName, string prodBrandName, string dosageForm, string prodStrength, string drugRegNo, string packSize, string group, string groupOther, string prodStax, string prodSchemeQty, string prodBonus, string prodTP, string prodMRP)
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();



            string AddIMDDataReponse = DAOBJ.ProductAddition(CompanySelect, prodGenericName, prodBrandName, dosageForm, prodStrength, drugRegNo, packSize, group, groupOther, prodStax, prodSchemeQty, prodBonus, prodTP, prodMRP);

            List<ComapnyViewModel> InstitutionForAdminData = DLOBJ.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = InstitutionForAdminData;

            List<GroupViewModel> GroupData = DLOBJ.FetchProductData();
            objListCompanyView.LstGroupList = GroupData;



            if (AddIMDDataReponse == "Product added successfully")
            {
                ViewBag.Message = "Product added successfully";
                return View("frmAddProduct", objListCompanyView);
            }

            else if (AddIMDDataReponse.Contains("Product addition Failed"))
            {
                ViewBag.Message = "Product cannot be added due to system level error see inner exception";
                return View("frmAddProduct", objListCompanyView);
            }
            return View("frmAddProduct", objListCompanyView);


        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult frmAddProduct()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<ComapnyViewModel> InstitutionForAdminData = DLOBJ.FetchCompanyData();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
            objListCompanyView.LstCompanyList = InstitutionForAdminData;

            List<GroupViewModel> GroupData = DLOBJ.FetchProductData();
            objListCompanyView.LstGroupList = GroupData;

            return View("frmAddProduct", objListCompanyView);
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult ProductsView()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<ProductViewModel> InstitutionForAdminData = DLOBJ.FetchProductTableData();
            ProductViewModelList objListProductsView = new ProductViewModelList();
            objListProductsView.LstProductTableList = InstitutionForAdminData;

            return View("ProductsView", objListProductsView);
        }
   
        [HttpPost]
        public ActionResult RemoveProduct(string Remove)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemoveProductFromTable(Remove);

            DataLogic DataLogicObj = new DataLogic();
            List<ProductViewModel> CompData = DataLogicObj.FetchProductTableData();
            ProductViewModelList objListProductsView = new ProductViewModelList();
            objListProductsView.LstProductTableList = CompData;


            if (RemUser == "Product Deleted Successfully")
            {
                ViewBag.Message = "Product Deleted Successfully";
                return View("ProductsView", objListProductsView);
            }

            else if (RemUser.Contains("Product cannot be deleted"))
            {
                ViewBag.Message = "Error Product not deleted see inner exception";
                return View("ProductsView", objListProductsView);
            }
            return View("ProductsView", objListProductsView);
        }
   
        [HttpGet]
        public ActionResult EditProduct(string Edit)
        {
           
            DataLogic DatlogAdm = new DataLogic();
            List<ProductViewModel> EditProduct = DatlogAdm.FetchProductTableForEdit(Edit);
            ProductViewModelList objListProductView = new ProductViewModelList();
            objListProductView.LstProductTableList = EditProduct;

            return View("EditProduct", objListProductView);
        }
   
        [HttpPost]
        public ActionResult EditProduct(string ProdID, string CompanySelect, string CompanyName1, string prodGenericName, string prodBrandName, string dosageForm, string prodStrength, string drugRegNo, string packSize, string group, string groupOther, string prodStax, string prodSchemeQty, string prodBonus, string prodTP, string prodMRP)
        {
            DataAddition DAOBJ = new DataAddition();
            string ValidatedUser = DAOBJ.ProductUpdate(ProdID, CompanySelect, CompanyName1, prodGenericName, prodBrandName, dosageForm, prodStrength, drugRegNo, packSize, group, groupOther, prodStax, prodSchemeQty, prodBonus, prodTP, prodMRP);

            if (ValidatedUser.Contains("Successfully"))
            {
                
                ViewBag.Message = "Product updated successfully";
                //return View("", );
            }

            else if (ValidatedUser.Contains("Failed"))
            {
                ViewBag.Message = "Error Product not updated see inner exception";
                // return View("", );
            }

            DataLogic DLOBJ = new DataLogic();
            ComapnyViewModelList objListCompanyView = new ComapnyViewModelList();
           
            List<GroupViewModel> GroupData = DLOBJ.FetchProductData();
            objListCompanyView.LstGroupList = GroupData;
            List<ProductViewModel> InstitutionForAdminData = DLOBJ.FetchProductTableData();
            ProductViewModelList objListProdView = new ProductViewModelList();
            objListProdView.LstProductTableList = InstitutionForAdminData;

            return View("ProductsView", objListProdView);
        }

        public ActionResult reportPurchases()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reportPurchases(object sender, EventArgs e, string option, string fromDate, string toDate, string outputFormat, string reportOf, float totalRupees, string fromDateFiscal, string toDateFiscal)
        {
            outputFormat.ToLower();
            try
            {
                string fileName = "Report_";
                DataLogic _DataLogic = new DataLogic();
                if (option == "custom date") {
                    fileName = "Report_" + option + fromDate + "-" + toDate;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReport(fromDate, toDate, option.ToLower());
                    //List<Purchases> dtTotalSum = _DataLogic.FetchPurchasesAmountTotal(fromDate, toDate, option.ToLower());
                    //PurchasesList objPurchasesList = new PurchasesList();
                    //objPurchasesList.LstPurchases = dtTotalSum

                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }

                if (option == "fiscal year")

                {
                    fileName = "Report_" + option + fromDateFiscal + "-" + toDateFiscal;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReport(fromDateFiscal, toDateFiscal, option.ToLower());
                    
                    
                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }

         
            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult reportPurchasesBatch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reportPurchasesBatch(object sender, EventArgs e, string option, string fromDate, string toDate, string outputFormat, string reportOf, float totalRupees, string fromDateFiscal, string toDateFiscal, string batchNum)
        {
            outputFormat.ToLower();
            try
            {
                string fileName = "Report_";
                DataLogic _DataLogic = new DataLogic();
                if (option == "custom date")
                {
                    fileName = "Report_" + option + fromDate + "-" + toDate;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReportBatch(fromDate, toDate, option.ToLower(), batchNum);
                    //List<Purchases> dtTotalSum = _DataLogic.FetchPurchasesAmountTotal(fromDate, toDate, option.ToLower());
                    //PurchasesList objPurchasesList = new PurchasesList();
                    //objPurchasesList.LstPurchases = dtTotalSum

                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }

                if (option == "fiscal year")

                {
                    fileName = "Report_" + option + fromDateFiscal + "-" + toDateFiscal;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReportBatch(fromDateFiscal, toDateFiscal, option.ToLower(), batchNum);


                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }


            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult reportPurchasesProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reportPurchasesProduct(object sender, EventArgs e, string option, string fromDate, string toDate, string outputFormat, string reportOf, float totalRupees, string fromDateFiscal, string toDateFiscal, string productName)
        {
            outputFormat.ToLower();
            try
            {
                string fileName = "Report_";
                DataLogic _DataLogic = new DataLogic();
                if (option == "custom date")
                {
                    fileName = "Report_" + option + fromDate + "-" + toDate;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReportProduct(fromDate, toDate, option.ToLower(), productName);
                    //List<Purchases> dtTotalSum = _DataLogic.FetchPurchasesAmountTotal(fromDate, toDate, option.ToLower());
                    //PurchasesList objPurchasesList = new PurchasesList();
                    //objPurchasesList.LstPurchases = dtTotalSum

                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }

                if (option == "fiscal year")

                {
                    fileName = "Report_" + option + fromDateFiscal + "-" + toDateFiscal;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReportProduct(fromDateFiscal, toDateFiscal, option.ToLower(), productName);


                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }


            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult reportPurchasesCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reportPurchasesCompany(object sender, EventArgs e, string option, string fromDate, string toDate, string outputFormat, string reportOf, float totalRupees, string fromDateFiscal, string toDateFiscal, string CompanySelect)
        {
            outputFormat.ToLower();
            try
            {
                string fileName = "Report_";
                DataLogic _DataLogic = new DataLogic();
                if (option == "custom date")
                {
                    fileName = "Report_" + option + fromDate + "-" + toDate;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReportCompany(fromDate, toDate, option.ToLower(), CompanySelect);
                    //List<Purchases> dtTotalSum = _DataLogic.FetchPurchasesAmountTotal(fromDate, toDate, option.ToLower());
                    //PurchasesList objPurchasesList = new PurchasesList();
                    //objPurchasesList.LstPurchases = dtTotalSum

                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }

                if (option == "fiscal year")

                {
                    fileName = "Report_" + option + fromDateFiscal + "-" + toDateFiscal;
                    DataTable dt = _DataLogic.FetchPurchasesDataForReportCompany(fromDateFiscal, toDateFiscal, option.ToLower(), CompanySelect);


                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }


            }
            catch (Exception)
            {
            }
            return View();
        }

        public ActionResult reportSellCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reportSellCustomer(object sender, EventArgs e, string option, string fromDate, string toDate, string outputFormat, string reportOf, float totalRupees, string fromDateFiscal, string toDateFiscal, string CustomerSelect)
        {
            outputFormat.ToLower();
            try
            {
                string fileName = "Report_";
                DataLogic _DataLogic = new DataLogic();
                if (option == "custom date")
                {
                    fileName = "Report_" + option + fromDate + "-" + toDate;
                    DataTable dt = _DataLogic.FetchSellDataForReportCustomer(fromDate, toDate, option.ToLower(), CustomerSelect);
                    //List<Purchases> dtTotalSum = _DataLogic.FetchPurchasesAmountTotal(fromDate, toDate, option.ToLower());
                    //PurchasesList objPurchasesList = new PurchasesList();
                    //objPurchasesList.LstPurchases = dtTotalSum

                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDate, toDate, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }

                if (option == "fiscal year")

                {
                    fileName = "Report_" + option + fromDateFiscal + "-" + toDateFiscal;
                    DataTable dt = _DataLogic.FetchSellDataForReportCustomer(fromDateFiscal, toDateFiscal, option.ToLower(), CustomerSelect);


                    ReportLogic RL = new ReportLogic();
                    if (outputFormat.Equals("pdf"))
                    {
                        Document pdfDoc = RL.GenerateInvoicePDF(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".pdf", reportOf, totalRupees);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.Flush();
                    }
                    else
                    {
                        string excelDoc = RL.GenerateInvoiceExcel(sender, e, option.ToUpper(), fromDateFiscal, toDateFiscal, dt, Response.OutputStream, fileName + ".xls", reportOf, totalRupees);

                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Output.Write(excelDoc);
                        Response.Flush();
                    }
                    Response.End();
                }


            }
            catch (Exception)
            {
            }
            return View();
        }

        [MyAuthorize]
        [HttpGet]
        public ActionResult formAddUser()
        {
            return View();
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult formAddUser(string userNameAdd, string userFirstName, string userLastName, int role, string password, string confirmpassword)
        {
            DataAddition DLOBJ = new DataAddition();
            string ValidatedUser = DLOBJ.UserAddition(userNameAdd, userFirstName, userLastName, role, password, confirmpassword);


            if (ValidatedUser.Contains("Successfully"))
            {
                ViewBag.Message = "User added successfully";
                //return View("", );
            }

            else if (ValidatedUser.Contains("Failed"))
            {
                ViewBag.Message = "Error User not added see inner exception";
                // return View("", );
            }
            return View();
        }
        [MyAuthorize]
        [HttpGet]
        public ActionResult UsersView()
        {
            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<UserViewModel> InstitutionForAdminData = DLOBJ.FetchUsersData();
            UserViewModelList objListUserView = new UserViewModelList();
            objListUserView.lstUserProfiles = InstitutionForAdminData;

            return View("UsersView", objListUserView);
        }
   
        [HttpGet]
        public ActionResult EditUser(string Edit)
        {

            DataAddition DAOBJ = new DataAddition();
            DataLogic DLOBJ = new DataLogic();
            List<UserViewModel> InstitutionForAdminData = DLOBJ.FetchUsersDataForEdit(Edit);
            UserViewModelList objListUserView = new UserViewModelList();
            objListUserView.lstUserProfiles = InstitutionForAdminData;

            return View("EditUser", objListUserView);
        }
   
        [HttpPost]
        public ActionResult EditUser()
        {

            return View();
        }
   
        [HttpPost]
        public ActionResult RemoveUser(string Remove)
        {
            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.RemoveUser(Remove);

            DataLogic DataLogicObj = new DataLogic();
            List<UserViewModel> UsersData = DataLogicObj.FetchUsersData();
            UserViewModelList objListUserView = new UserViewModelList();
            objListUserView.lstUserProfiles = UsersData;


            if (RemUser == "User Deleted Successfully")
            {
                ViewBag.Message = "User Deleted Successfully";
                return View("UsersView", objListUserView);
            }

            else if (RemUser.Contains("User cannot be deleted"))
            {
                ViewBag.Message = "User cannot be deleted see inner exception";
                return View("UsersView", objListUserView);
            }
            else if (RemUser.Contains("Super user cannot be delete"))
            {
                ViewBag.Message = RemUser;
                return View("UsersView", objListUserView);
            }

            return View("UsersView", objListUserView);
        }
   
        [HttpGet]
        public ActionResult ChangePassword()
        {
           
            return View();
        }
   
        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string confirmNewPassword)
        {

            string userid = Session["User"].ToString();

            DataLogic DatlogAdm = new DataLogic();
            string RemUser = DatlogAdm.ChangePassword(currentPassword, confirmNewPassword, userid);

            if (RemUser.Contains("Successfully"))
            {
                ViewBag.Message = "password changed successfully please login with your new password";
                return View("Login");
            }

            else if (RemUser.Contains("Failed"))
            {
                ViewBag.Message = "Error password cannot be changed see inner exception";
                return View("ChangePassword");
            }

            ViewBag.Message = "Error password cannot be changed see inner exception";
            return View();
            
         
        }

        [MyAuthorize]
        public ActionResult InvoiceDeliveryChalan()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult InboxMessages()
        {
            return View();
        }
    }
}