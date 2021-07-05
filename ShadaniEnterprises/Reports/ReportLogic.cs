using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using ShadaniEnterprises.Models;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.tool.xml;

namespace ShadaniEnterprises.Reports
{
    public class ReportLogic : System.Web.UI.Page
    {
        public StringBuilder Template(object sender, EventArgs e, string reportType, string startDate, string endDate, DataTable dt, Stream stream, string fileName, string reportOf)
        {
            string companyName = "Shadani Enterprises";
            fileName = "_" + fileName;
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html />");
            sb.Append("<html>");
            sb.Append(@"<style>
                                table {
                                      font-family: arial, sans-serif;
                                      border-collapse: collapse;
                                      width: 100%;
                                    }

                                    td, th {
                                      border: 1px solid #000;
                                      text-align: left;
                                      padding: 5px;
font-size:10px;
                                    }

                                    th {
                                      background-color: #C0C0C0;
                                    }
                                tr:nth-child(odd){
                                  background-color: #dddddd;
                                }
                                </ style > ");
            sb.Append("<body style='padding-left: 80px; width:100%'>");

            sb.Append("<h1 align='left' style='background-color: #fff;'><b>" + companyName + "</b></h1>");
            sb.Append("<h1 align='left' style='background-color: #fff;'><b>" + reportOf + "</b></h1>");
            sb.Append("<div style='position:relative;width:100%'><span style='position:absolute;right:0px;'><b>Date: </b>" + DateTime.Now + "</span></div>");
            sb.Append("<div style='position:relative;width:100%'><span style='position:absolute;right:0px;'><b>Report: </b>" + reportType + "</span></div>");
            sb.Append("<div style='position:relative;width:100%'><span style='position:absolute;right:0px;'><b>Start Date: </b>" + startDate + "</span></div>");
            sb.Append("<div style='position:relative;width:100%'><span style='position:absolute;right:0px;'><b>End Date: </b>" + endDate + "</span></div>");
         
           sb.Append("<div style='position:relative;width:100%'><span style='position:absolute;right:0px;'><b>Total Rs: </b>" + "0" + "</span></div>");
            
            
            sb.Append("<br />");
            sb.Append("<table>");
            sb.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append("<th style='font-weight:bold'>");
                sb.Append(column.ColumnName);
                sb.Append("</th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<td>");
                    sb.Append(row[column]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb;
        }

        public Document GenerateInvoicePDF(object sender, EventArgs e, string reportType, string startDate, string endDate, DataTable dt, Stream stream, string fileName, string reportOf, float totalRupees)
        {
            string companyName = "Shadani Enterprises";
            fileName = "_" + fileName;
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = Template(sender, e, reportType, startDate, endDate, dt, stream, fileName, reportOf);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 30f);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    using (TextReader reader = new StringReader(sb.ToString()))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, pdfDoc, reader);
                    }
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    return pdfDoc;
                }
            }
        }
        public string GenerateInvoiceExcel(object sender, EventArgs e, string reportType, string startDate, string endDate, DataTable dt, Stream stream, string fileName, string reportOf, float totalRupees)
        {
            string companyName = "Shadani Enterprises";
            fileName = "_" + fileName;

            StringBuilder sb = Template(sender, e, reportType, startDate, endDate, dt, stream, fileName, reportOf);
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            return sw.ToString();

        }


        public void WritePageNumber(string fileIn)
        {
            byte[] bytes = File.ReadAllBytes(fileIn);
            Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(i.ToString(), blackFont), 568f, 15f, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(fileIn, bytes);
        }
    }
}