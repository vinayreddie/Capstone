using Capstone.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Framework
{
    public static class Logger
    {
        public static void LogError(ExceptionModel objExcEntity)
        {
            try
            {
                string ErrorLogLocation = ConfigurationManager.AppSettings["ErrorLogLocation"];
                if (ErrorLogLocation == "")
                {
                    ErrorLogLocation = System.Web.HttpContext.Current.Server.MapPath("~/Logs/ErrorLogs/");
                }

                string FileName = DateTime.Now.ToString("dd-MMM-yyyy") + " Errors.txt";
                StreamWriter sw = File.AppendText(ErrorLogLocation + FileName);
                sw.WriteLine("");
                sw.WriteLine("==================== " + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString() + " ====================");
                sw.WriteLine("==============");
                sw.WriteLine("Error Details }");
                sw.WriteLine("==============");
                sw.WriteLine("Request Page :" + objExcEntity.RequestPage);
                sw.WriteLine("Error Class Name : " + objExcEntity.ClassName);
                sw.WriteLine("Error Method Name: " + objExcEntity.MethodName);
                sw.WriteLine("Error Line No : " + objExcEntity.LineNo);
                sw.WriteLine("Error Description :" + objExcEntity.ErrorMessage);
                sw.WriteLine("Custom Message :" + objExcEntity.CustomMessage);
                sw.WriteLine("DB Object : " + objExcEntity.DbObject);
                sw.WriteLine("");
                sw.WriteLine("==================");
                sw.WriteLine("Parent Parameters }");
                sw.WriteLine("==================");
                Type ParentType;
                if (objExcEntity.ParentParamField != null)
                {
                    ParentType = objExcEntity.ParentParamField.GetType();
                    var parentArray = ParentType.GetProperties();
                    foreach (var prop in parentArray)
                    {
                        object objvalue = prop.GetValue(objExcEntity.ParentParamField, null);
                        string value = objvalue == null ? "null" : objvalue.ToString();
                        sw.WriteLine(prop.Name + ": " + value);
                    }
                }
                sw.WriteLine("");
                sw.WriteLine("==================");
                sw.WriteLine("Master Parameters }");
                sw.WriteLine("==================");
                Type MasterType;
                if (objExcEntity.MasterParamField != null)
                {
                    MasterType = objExcEntity.MasterParamField.GetType();
                    var masterArray = MasterType.GetProperties();
                    foreach (var prop in masterArray)
                    {
                        object objvalue = prop.GetValue(objExcEntity.MasterParamField, null);
                        string value = objvalue == null ? "null" : objvalue.ToString();
                        sw.WriteLine(prop.Name + ": " + value);
                    }
                }
                sw.WriteLine("");
                sw.WriteLine("=================");
                sw.WriteLine("Child Parameters }");

                Type ChildListType;
                if (objExcEntity.ChildParamField != null)
                {
                    ChildListType = objExcEntity.ChildParamField.GetType();
                    if (ChildListType.IsGenericType && ChildListType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type ChildEntityType = ChildListType.GetGenericArguments()[0];
                        IList objChildList = (IList)objExcEntity.ChildParamField;

                        var childPropertyArray = ChildEntityType.GetProperties();

                        // To Write Header Column Names & UnderLines
                        string HeaderColumns = "";
                        string HeaderBorder = "";
                        string RowSeperatorLine = "";

                        int[] MaxColumnLength = new int[childPropertyArray.Length];
                        int ColumnIndex = 0;

                        foreach (var prop in childPropertyArray)
                        {
                            int MaxLength = 0;
                            for (int i = 0; i < objChildList.Count; i++)
                            {
                                object objvalue = prop.GetValue(objChildList[i], null);
                                string value = objvalue == null ? "null" : objvalue.ToString();
                                int Length = value.Length;
                                if (Length > MaxLength)
                                {
                                    MaxLength = Length;
                                }
                            }
                            if (prop.Name.Length > MaxLength)
                            {
                                MaxLength = prop.Name.Length;
                            }
                            MaxColumnLength[ColumnIndex] = MaxLength;
                            ColumnIndex++;

                            HeaderBorder = HeaderBorder + "".PadRight(MaxLength, '=') + "===";
                            HeaderColumns = HeaderColumns + prop.Name.PadRight(MaxLength, ' ') + " | ";
                            RowSeperatorLine = RowSeperatorLine + "".PadRight(MaxLength, '-') + "---";
                        }

                        HeaderColumns = HeaderColumns.Trim().TrimEnd('|');
                        sw.WriteLine(HeaderBorder);
                        sw.WriteLine(HeaderColumns);
                        sw.WriteLine(HeaderBorder);

                        for (int i = 0; i < objChildList.Count; i++)
                        {
                            string DataLine = "";
                            ColumnIndex = 0;
                            foreach (var prop in childPropertyArray)
                            {
                                object objvalue = prop.GetValue(objChildList[i], null);
                                string value = objvalue == null ? "null" : objvalue.ToString();
                                DataLine = DataLine + value.PadRight(MaxColumnLength[ColumnIndex], ' ') + " | ";
                                ColumnIndex++;
                            }
                            DataLine = DataLine.Trim().TrimEnd('|');
                            sw.WriteLine(DataLine);
                            sw.WriteLine(RowSeperatorLine);
                        }
                    }
                }
                sw.WriteLine("");
                sw.WriteLine("====================");
                sw.WriteLine("Special Trace Fiels }");
                Type TraceListType;
                if (objExcEntity.SPTraceFields != null)
                {
                    TraceListType = objExcEntity.SPTraceFields.GetType();
                    if (TraceListType.IsGenericType && TraceListType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type TraceEntityType = TraceListType.GetGenericArguments()[0];
                        IList objTraceList = (IList)objExcEntity.SPTraceFields;

                        var tracePropertyArray = TraceEntityType.GetProperties();

                        string HeaderColumns = "";
                        string HeaderBorder = "";
                        string RowSeperatorLine = "";

                        int[] MaxColumnLength = new int[tracePropertyArray.Length];
                        int ColumnIndex = 0;

                        foreach (var prop in tracePropertyArray)
                        {
                            int MaxLength = 0;
                            for (int i = 0; i < objTraceList.Count; i++)
                            {
                                object objvalue = prop.GetValue(objTraceList[i], null);
                                string value = objvalue == null ? "null" : objvalue.ToString();
                                int Length = value.Length;
                                if (Length > MaxLength)
                                {
                                    MaxLength = Length;
                                }
                            }
                            if (prop.Name.Length > MaxLength)
                            {
                                MaxLength = prop.Name.Length;
                            }
                            MaxColumnLength[ColumnIndex] = MaxLength;
                            ColumnIndex++;

                            HeaderBorder = HeaderBorder + "".PadRight(MaxLength, '=') + "===";
                            HeaderColumns = HeaderColumns + prop.Name.PadRight(MaxLength, ' ') + " | ";
                            RowSeperatorLine = RowSeperatorLine + "".PadRight(MaxLength, '-') + "---";
                        }

                        HeaderColumns = HeaderColumns.Trim().TrimEnd('|');
                        sw.WriteLine(HeaderBorder);
                        sw.WriteLine(HeaderColumns);
                        sw.WriteLine(HeaderBorder);


                        for (int i = 0; i < objTraceList.Count; i++)
                        {
                            string DataLine = "";
                            ColumnIndex = 0;
                            foreach (var prop in tracePropertyArray)
                            {
                                object objvalue = prop.GetValue(objTraceList[i], null);
                                string value = objvalue == null ? "null" : objvalue.ToString();
                                DataLine = DataLine + value.PadRight(MaxColumnLength[ColumnIndex], ' ') + " | ";
                                ColumnIndex++;
                            }
                            DataLine = DataLine.Trim().TrimEnd('|');
                            sw.WriteLine(DataLine);
                            sw.WriteLine(RowSeperatorLine);
                        }
                    }
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //string SendMail = ConfigurationManager.AppSettings["ErrorReporting"].ToUpper();
            }

        }

        private static void ReportErrorViaEmail(ExceptionModel objExcEntity)
        {
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;

            string SupportGroupAddress = ConfigurationManager.AppSettings["SupportEmailGroup"];
            MailMessage msg = new MailMessage("errorreport.aegis@gmail.com", SupportGroupAddress);
            msg.IsBodyHtml = true;
            msg.Subject = "APCOB Application Error";
            string Date = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
            string ParentHTML = HTMLGenerator.ToHtmlTableFromItem(objExcEntity.ParentParamField, "", "", "", "");
            string MasterHTML = HTMLGenerator.ToHtmlTableFromItem(objExcEntity.MasterParamField, "", "", "", "");
            string ChildHTML = HTMLGenerator.ToHtmlTableFromList(objExcEntity.ChildParamField, "", "", "", "");
            string TraceHTML = HTMLGenerator.ToHtmlTableFromList(objExcEntity.SPTraceFields, "", "", "", "");

            string MsgBody = "";
            MsgBody += "<html>";
            MsgBody += "<head>";
            //MsgBody += "<style type='text/css'>";
            //MsgBody += ".name{background-color:#81AA33 ; font-weight:bold;}";
            //MsgBody += "</style>";
            MsgBody += "</head>";
            MsgBody += "<body>";
            MsgBody += "<b>Hi Support Team,</b>";
            MsgBody += "<br/> <br/>";
            MsgBody += "<div style='font-weight:bold; color:#990000;'>An Error Occured in APCOB Application </div>";
            MsgBody += "<br/>";
            MsgBody += "<div style='font-weight:bold; color:#424242;'>Error Details are as follows: </div>";
            MsgBody += "<br/> <br/>";

            MsgBody += @"<table style= 'color:#424242; font-weight:bold; width: 50%; border:thin; border-style:solid;'>                            
                            <tr>
                                <td>Created From : </td>
                                <td>" + objExcEntity.CreatedFrom + @"</td>
                            </tr>
                            <tr>
                                <td>Created By : </td>
                                <td>" + objExcEntity.CreatedBy + @"</td>
                            </tr>
                            <tr>
                                   <td>Created Date :</td>
                                   <td>" + Date + @"</td>
                            </tr>
                            <tr>
                                <td>Request Page : </td>
                                <td>" + objExcEntity.RequestPage + @"</td>
                            </tr>
                            <tr>
                                 <td>Error Class Name:</td>
                                 <td>" + objExcEntity.ClassName + @"</td>
                            </tr>
                            <tr>
                                 <td>Error Method Name:</td>
                                 <td>" + objExcEntity.MethodName + @"</td>
                            </tr>
                            <tr>
                                 <td>Error Line No. :</td>
                                 <td>" + objExcEntity.LineNo + @"</td>
                            </tr>
                            <tr>
                                  <td>Error Description : </td>
                                  <td>" + objExcEntity.ErrorMessage + @"</td>
                            </tr>                                                           
                         </table>";
            MsgBody += "<br/>";
            MsgBody += "<div style='font-weight:bold; color:#045FB4;'> <u>Parent Parameters : </u></div>";
            MsgBody += "<br/>";
            MsgBody += "<div style= 'color:#424242; font-weight:bold; width: 50%; border:thin; border-style:solid;'>" + ParentHTML + "</div>";
            MsgBody += "<br/>";
            MsgBody += "<div style='font-weight:bold; color:#045FB4;'><u> Master Parameters : </u></div>";
            MsgBody += "<br/>";
            MsgBody += "<div style= 'color:#424242; font-weight:bold; width: 50%; border:thin; border-style:solid;'>" + MasterHTML + "</div>";
            MsgBody += "<br/>";
            MsgBody += "<div style='font-weight:bold; color:#045FB4;'><u> Child Parameters : </u></div>";
            MsgBody += "<br/>";
            MsgBody += "<div style= 'color:#424242; font-weight:bold; width: 100%; border:thin; border-style:solid;'>" + ChildHTML + "</div>";
            MsgBody += "<br/>";
            MsgBody += "<div style='font-weight:bold; color:#045FB4;'><u> Special Trace Fields : </u></div>";
            MsgBody += "<br/>";
            MsgBody += "<div style= 'color:#424242; font-weight:bold; width: 50%; border:thin; border-style:solid;'>" + TraceHTML + "</div>";
            MsgBody += "</body>";
            MsgBody += "</html>";
            msg.Body = MsgBody;

            try
            {
                client.Send(msg);
            }
            catch (Exception Ex)
            {
                //Window.ShowAlert("No Internet Connectivity To Send Error Report To AEGIS.");
            }
        }

        public static void LogMessage(string Event, string Message)
        {
            string MessageLogLocation = ConfigurationManager.AppSettings["MessageLogs"];
            if (MessageLogLocation == "")
                MessageLogLocation = System.Web.HttpContext.Current.Server.MapPath("~/Logs/MessageLogs/");
            string FileName = Event + ".txt";
            StreamWriter sw = File.AppendText(MessageLogLocation + FileName);
            sw.WriteLine("");
            sw.WriteLine("====================" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            sw.WriteLine(Message.ToString());
            sw.Flush();
            sw.Close();
        }

        public static void PrintPDF(string Event, string Message)
        {
            string FileLocation = ConfigurationManager.AppSettings["InspectionFile"];
            if (FileLocation == "")
                FileLocation = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Department/InspectionReports/");
            string FileName = Event + ".txt";

            StreamWriter sw = File.AppendText(FileLocation + FileName);
            sw.WriteLine("");
            sw.WriteLine("====================" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            sw.WriteLine(Message.ToString());
            sw.Flush();
            sw.Close();
        }
    }
}
