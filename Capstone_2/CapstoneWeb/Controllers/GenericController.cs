using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Ionic.Zip;
using Capstone.Models;
using Capstone.BAL;

namespace Capstone.Controllers
{
    public class GenericController : Controller
    {
        // GET: Generic
        public ActionResult Index()
        {
            return View();
        }

        public FileResult DownloadFile(string path, string downloadName)
        {
            return File(Server.MapPath("~/Uploads/" + path), "multipart/form-data", downloadName);


        }

        //public FileResult DownloadMultipleFiles(int TransactionId)
        //{
        //    LicenseBAL LBAL = new LicenseBAL();
        //    PCPNDTViewModel objPCPNDTData = LBAL.GetPCPNDTData(TransactionId, "Transaction");
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //        zip.AddDirectoryByName("Files");
        //        List<string> docs = new List<string>();
        //        string path = Server.MapPath("~/Uploads/" + "Applicant\\6073\\2\\ApplicantDetails/chandu aadhar card_2018-02-22_11-34-26_2018-05-10_11-57-43.pdf");
        //        docs.Add(path);


        //        foreach (string doc in docs)
        //        {

        //                zip.AddFile(path, "Files/TextFiles");

        //        }
        //        Response.Clear();
        //        Response.BufferOutput = false;
        //        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //        Response.ContentType = "application/zip";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //        zip.Save(Response.OutputStream);
        //        Response.End();
        //        return File(Response.OutputStream, "multipart/form-data", "downloadName");

        //    }
        //}
        public ActionResult DownloadMultipleFiles(int TransactionId)
        {
            LicenseBAL LBAL = new LicenseBAL();
            PCPNDTViewModel objPCPNDTData = LBAL.GetPCPNDTData(TransactionId, "Transaction");
            using (ZipFile zip = new ZipFile())
            {
                //zip.AlternateEncodingUsage = ZipOption.AsNecessary;       // giving reference error
                zip.AddDirectoryByName("Files");
                List<string> docs = new List<string>();
                string path = Server.MapPath("~/Uploads/" + "Applicant\\6073\\2\\ApplicantDetails/chandu aadhar card_2018-02-22_11-34-26_2018-05-10_11-57-43.pdf");
                docs.Add(path);


                foreach (string doc in docs)
                {

                    zip.AddFile(path, "Files/TextFiles");

                }
                //MemoryStream output = new MemoryStream();
                //zip.Save(output);
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
                return File(Response.OutputStream, "application/zip", "downloadName.zip");

            }
        }

        public ActionResult DownloadZip()
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectoryByName("Files");
                List<string> docs = new List<string>();
                string path = Server.MapPath("~/Uploads/" + "Applicant\\6073\\2\\ApplicantDetails/chandu aadhar card_2018-02-22_11-34-26_2018-05-10_11-57-43.pdf");
                docs.Add(path);


                foreach (string doc in docs)
                {

                    zip.AddFile(path, "Files/TextFiles");

                }
                MemoryStream output = new MemoryStream();
                zip.Save(output);
                return File(output, "application/zip", "sample.zip");
            }
        }

        public ActionResult DownloadEnclosures(int transactionId)
        {

            using (ZipFile zip = new ZipFile())
            {
                //zip.AlternateEncodingUsage = ZipOption.AsNecessary;       // giving reference error
                zip.AddDirectoryByName("Files");
                List<string> docs = new List<string>();
                string path = Server.MapPath("~/Uploads/" + "Applicant\\6073\\2\\ApplicantDetails/chandu aadhar card_2018-02-22_11-34-26_2018-05-10_11-57-43.pdf");
                docs.Add(path);
                foreach (string doc in docs)
                {

                    zip.AddFile(doc, "Files");

                }
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }


        }
    }
}