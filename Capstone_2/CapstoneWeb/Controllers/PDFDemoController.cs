using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Web.UI;
using HiQPdf;
using WkHtmlToXSharp;
using System.Text;
using SelectPdf;

namespace Capstone.Controllers
{
    public class PDFDemoController : Controller
    {
        // GET: PDFDemo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DownloadPDF()
        {
            return View();
        }

        public void GetAndReturnSerializedString()
        {
            //http://jsbin.com/ejuru
            string serializedString = Request.Params["serializedString"];
            string pdfString = Decode(serializedString);
            Session["pdf"] = pdfString;
        }

        public void RenderPdf()
        {
            string htmlData = Session["pdf"].ToString();
            string path = Server.MapPath("Pdf");
            string attachment = "inline; filename=Article.pdf";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/pdf";

            StringWriter stw = new StringWriter(new System.Text.StringBuilder(htmlData));

            HtmlTextWriter htextw = new HtmlTextWriter(stw);

            Document document = new Document();

            PdfWriter.GetInstance(document, Response.OutputStream);

            document.Open();

            StringReader str = new StringReader(stw.ToString());

            HTMLWorker htmlworker = new HTMLWorker(document);

            try
            {
              //  HeaderFooter footer = new HeaderFooter(new Phrase("You are in Page "), true) { Border = Rectangle.NO_BORDER };
               // document.Footer = footer;
                htmlworker.Parse(str);
            }
            catch (Exception exception)
            {
                Paragraph paragraph = new Paragraph("Error! " + exception.Message);
               // paragraph.SetAlignment("center");
                
                Chunk text = paragraph.Chunks[0] as Chunk;
                //if (text != null)
                //{
                //    text.Font.Color = Color.RED;
                //}
                document.Add(paragraph);
            }
            if (document.IsOpen())
                document.Close();

            Response.Write(document);

            Response.End();

        }

        public void iTextPDF()
        {
            string htmlData = Session["pdf"].ToString();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            // this.Page.RenderControl(hw);
            StringReader sr = new StringReader(htmlData);
            // string htmlToConvert = RenderViewAsString("Test", null);
            //StringReader sr = new StringReader(htmlToConvert);

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            HTMLWorker htmltext = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            PdfWriter writer = null;
            iTextSharp.text.pdf.PdfDocument doc = null;
            //PdfReader reader = new PdfReader(content);
            // reader.Info["Title"]

            //doc = new PdfDocument();
            //    doc.SetPageSize(PageSize.LETTER);
            //    writer = PdfWriter.GetInstance(doc, null);

            //    writer.CloseStream = false;
            //    doc.Open();
            //    doc.NewPage();

            //    foreach (IElement element in iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlData), null))
            //    {
            //        doc.Add(element);
            //    }

            //htmlparser.Parse(sr);
            using (TextReader sReader = new StringReader(htmlData))
            {
                List<IElement> list = HTMLWorker.ParseToList(sReader, new StyleSheet());
                foreach (IElement elm in list)
                {
                    doc.Add(elm);
                }
            }

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }


        string Decode(string input)
        {
            input = Server.HtmlDecode(input);
            return input;
        }

        StyleSheet LoadStyleSheet()
        {
            StyleSheet styleSheet = new StyleSheet();
            return styleSheet;
        }

        public virtual void printpdf(string html)
        {
           // string htmlText = Session["pdf"].ToString();
           string htmlText="< table id = 'tblReport' style = 'border:1px solid black;background-color:red' >< tr >< td >< i > Sample Table Data</ i ></ td ></ tr ></ table > ";
           // String htmlText = html.ToString();
            Document document = new Document();
            string filePath = Server.MapPath("~/Uploads/");
            PdfWriter.GetInstance(document, new FileStream(filePath + "\\pdf-" + "pdfNew" + ".pdf", FileMode.Create));

            document.Open();
            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(htmlText));
            document.Close();
        }
        public ActionResult ConvertAboutPageToPdf()
        {
            // get the About view HTML code
            string htmlToConvert = Session["pdf"].ToString();

            // the base URL to resolve relative images and css
            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertAboutPageToPdf".Length);

            // instantiate the HiQPdf HTML to PDF converter
            HiQPdf.HtmlToPdf htmlToPdfConverter = new HiQPdf.HtmlToPdf();
           // htmlToPdfConverter.ConvertedHtmlElementSelector = "#demoTable";
            htmlToPdfConverter.Document.Header.Enabled = false;

            // render the HTML code as PDF in memory
            byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "AboutMvcViewToPdf2.pdf";
            var path = Path.Combine(Server.MapPath("~/Uploads"), fileResult.FileDownloadName);
            ToFile(fileResult, path);
           
            return fileResult;
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

        public  void wkPDF()
        {

            WkHtmlToPdfConverter wol = new WkHtmlToPdfConverter();

            // Get PDF in bytes
            //Byte[] bufferPDF = wol.GetUrlPDF(url);
            Byte[] bufferPDF =   Encoding.ASCII.GetBytes("Siva");

            // Convert bytes to stream
            System.IO.FileStream writeStream = new System.IO.FileStream("sample.pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            writeStream.Write(bufferPDF, 0, Convert.ToInt32(bufferPDF.Length));
            writeStream.Close();

            // Open PDF file
            System.Diagnostics.Process.Start(@"sample.pdf");

            
        }

        public ActionResult selectPDF()
        {
            // read parameters from the webpage
            string htmlString = Session["pdf"].ToString();
            //string baseUrl = collection["TxtBaseUrl"];

            //string pdf_page_size = collection["DdlPageSize"];
          // SelectPdf.PdfPageSize pageSize = (SelectPdf.PdfPageSize)Enum.Parse(typeof(SelectPdf.PdfPageSize),
              //  pdf_page_size, true);

            //string pdf_orientation = collection["DdlPageOrientation"];
           // SelectPdf.PdfPageOrientation pdfOrientation =
              //  (SelectPdf.PdfPageOrientation)Enum.Parse(typeof(SelectPdf.PdfPageOrientation),
              //  pdf_orientation, true);

            //int webPageWidth = 1024;
            //try
            //{
            //    webPageWidth = Convert.ToInt32(collection["TxtWidth"]);
            //}
            //catch { }

            //int webPageHeight = 0;
            //try
            //{
            //    webPageHeight = Convert.ToInt32(collection["TxtHeight"]);
            //}
            //catch { }

            // instantiate a html to pdf converter object
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            // set converter options
            //converter.Options.PdfPageSize = pageSize;
            //converter.Options.PdfPageOrientation = pdfOrientation;
            //converter.Options.WebPageWidth = webPageWidth;
            //converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            //SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString);

        // save pdf document
        byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            return fileResult;

        }
    }
        
}