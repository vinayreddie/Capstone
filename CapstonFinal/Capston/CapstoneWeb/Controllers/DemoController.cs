using System;
using System.IO;
using System.Web.Mvc;
using HiQPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Drawing;
using System.Web.UI;
using System.Web;
using System.Text;
using iTextSharp.text.pdf.parser;
using System.Net;
using System.Net.Mail;


namespace Capstone.Controllers
{
    public class DemoController : Controller
    {
        public object Page { get; private set; }
        static iTextSharp.text.pdf.PdfStamper stamper = null;
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            Session["MySessionVariable"] = "My Session Variable Value assigned in Index";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public string RenderViewAsString(string viewName, object model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
            // create a context to render a view based on a model


            ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    new ViewDataDictionary(model),
                    new TempDataDictionary(),
                    stringWriter
                    );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }
        [HttpPost]
        public ActionResult ConvertThisPageToPdf()
        {
            // get the HTML code of this view
            string htmlToConvert = RenderViewAsString("Test", null);

            // the base URL to resolve relative images and css
            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertThisPageToPdf".Length);

            // instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // hide the button in the created PDF
            htmlToPdfConverter.HiddenHtmlElements = new string[] { "#test" };
            htmlToPdfConverter.ConvertedHtmlElementSelector = "#testdiv";
            // render the HTML code as PDF in memory
            byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "ThisMvcViewToPdf.pdf";

            return fileResult;
        }

        [HttpPost]
        public ActionResult ConvertAboutPageToPdf()
        {
            // get the About view HTML code
            string htmlToConvert = RenderViewAsString("Test", null);

            // the base URL to resolve relative images and css
            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertAboutPageToPdf".Length);

            // instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.ConvertedHtmlElementSelector = "#testdiv";
            htmlToPdfConverter.Document.Header.Enabled = false;

            // render the HTML code as PDF in memory
            byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "AboutMvcViewToPdf2.pdf";
            var path = Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);
            ToFile(fileResult, path);
            VerySimpleReplaceText(path, path, "HiQPdf", "MOunika");
            return fileResult;
        }
        void VerySimpleReplaceText(string OrigFile, string ResultFile, string origText, string replaceText)
        {
            using (PdfReader reader = new PdfReader(OrigFile))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    byte[] contentBytes = reader.GetPageContent(i);
                    string contentString = PdfEncodings.ConvertToString(contentBytes, iTextSharp.text.pdf.PdfObject.TEXT_PDFDOCENCODING);
                    contentString = contentString.Replace(origText, replaceText);
                    reader.SetPageContent(i, PdfEncodings.ConvertToBytes(contentString, iTextSharp.text.pdf.PdfObject.TEXT_PDFDOCENCODING));
                }
                new PdfStamper(reader, new FileStream(ResultFile, FileMode.Create, FileAccess.Write)).Close();
            }
        }


        public static void ToFile(FileResult fileResult, string fileName)
        {
            if (fileResult is FileContentResult)
            {
                System.IO.File.WriteAllBytes(fileName, ((FileContentResult)fileResult).FileContents);
            }
            else if (fileResult is FilePathResult)
            {
                System.IO.File.Copy(((FilePathResult)fileResult).FileName, fileName, true); //overwrite file if it already exists
            }
            else if (fileResult is FileStreamResult)
            {
                //from http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file-in-c
                using (var fileStream = System.IO.File.Create(fileName))
                {
                    var fileStreamResult = (FileStreamResult)fileResult;
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                    fileStreamResult.FileStream.CopyTo(fileStream);
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin); //reset position to beginning. If there's any chance the FileResult will be used by a future method, this will ensure it gets left in a usable state - Suggestion by Steven Liekens
                }
            }
            else
            {
                throw new ArgumentException("Unsupported FileResult type");
            }
        }

        public void iTextPDF()
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            // this.Page.RenderControl(hw);
            StringReader sr = new StringReader("<p>To learn more about ASP.NET MVC visit ASP.NET MVC Website</ p >");
            // string htmlToConvert = RenderViewAsString("Test", null);
            //StringReader sr = new StringReader(htmlToConvert);

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            HTMLWorker htmltext = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            htmlparser.Parse(sr);

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        public void DownloadPDF()
        {

            string HTMLContent = "Hello <b>World</b>";
            // string HTMLContent =  RenderViewAsString("Test", null); 
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile1.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(HTMLContent));
            Response.End();
        }
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.ConvertedHtmlElementSelector = "#testdiv";

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }


        public ActionResult SendEmail()
        {
            return View("sendemail");
        }

        [HttpPost]
        public bool SendEmail(string Name, string Email, string subject, string Message)
        {
            string mail = "sivakatta.acs@gmail.com"; //<--Enter your gmail id here
            string password = "sivas1031";//<--Enter gmail password here
            string FromMail = "sivakatta.acs@gmail.com";//< --Enter from mail
            using (MailMessage mm = new MailMessage(FromMail, Email))
            {
                mm.Subject = subject;
                mm.Body = Message;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(mail, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm)
;
                return true;
            }
        }
    }
}



