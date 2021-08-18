using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;



namespace Capstone.Framework
{
    public static class Utitlities
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "8U9jRGyJEqKdMlcq8HGp"; // Generated at https://www.random.org/strings

        public static string GenerateToken(string username, string password)
        {
            string hash = string.Join(":", new string[] { username, password });

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                hash = Convert.ToBase64String(hmac.Hash);
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
        }

        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));

                return Convert.ToBase64String(hmac.Hash);
            }
        }

        /// <summary>
        /// Converts given Collection to Datatable
        /// </summary>
        /// <typeparam name="T">Entity Name</typeparam>
        /// <param name="items">Collection object</param>
        /// <returns>Datatable that converted from given Collection</returns>
        public static DataTable ConvertToDataTable2<T>(List<T> items)
        {
            if (items != null)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            else
                return null;
        }

        /// <summary>
        /// Converts given Collection to Datatable
        /// </summary>
        /// <typeparam name="T">Entity Name</typeparam>
        /// <param name="items">Collection object</param>
        /// <returns>Datatable that converted from given Collection</returns>
        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            if (items != null)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        if (Props[i].PropertyType == typeof(DateTime))
                        {
                            // date time type
                            values[i] = Convert.ToDateTime(Props[i].GetValue(item, null)).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else if (Props[i].PropertyType.IsEnum)
                        {
                            // enum type, get the value of the enum
                            Type enumType = Type.GetType(Props[i].PropertyType.AssemblyQualifiedName);
                            Type enumValueType = Type.GetType(Enum.GetUnderlyingType(enumType).AssemblyQualifiedName);
                            var objectValue = Props[i].GetValue(item, null);

                            //// Get enum's integer value
                            //var enumValue = Convert.ChangeType(objectValue, enumValueType);

                            // Get enum's specific attribute value
                            var memInfo = enumType.GetMember(objectValue.ToString());
                            var attribute = memInfo.FirstOrDefault()?.GetCustomAttributes(false)
                                .OfType<EnumMemberAttribute>().FirstOrDefault();
                            var value = attribute == null ? objectValue : attribute.Value;
                            values[i] = value;
                        }
                        else
                        {
                            values[i] = Props[i].GetValue(item, null);
                        }
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            else
                return null;
        }


        #region SMS
        //public static bool SendSMS(string mobileNo, string message)
        //{
        //    //we creating the necessary URL string:
        //    string URL = "http://sms6.routesms.com:8080/bulksms/bulksms"; //where the SMS Gateway is running
        //    string port = "2352"; //port number where the SMS Gateway is listening
        //    String senderId = "COMMAG";
        //    string username = "aegistrans"; //username for successful login
        //    string password = "ifZ87Cjb"; //user's password
        //    int type = 0; //type of message
        //    int dlr = 1;
        //    string recipients = mobileNo; //who will get the message
        //    string messageData = message; //body of message

        //    String createdURL = "http://sms6.routesms.com:8080/bulksms/bulksms?username="
        //        + username + "&password=" + password + "&type=" + type + "&dlr=" + dlr +
        //        "&destination=" + recipients + "&source=" + senderId + "&message=" + messageData + "";



        //    try
        //    {
        //        //Create the request and send data to the SMS Gateway Server by HTTP connection
        //        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(createdURL);
        //        //Get response from the SMS Gateway Server and read the answer
        //        HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        //        System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        //        string responseString = respStreamReader.ReadToEnd();
        //        respStreamReader.Close();
        //        myResp.Close();
        //        //inform the user
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //if sending request or getting response is not successful the SMS Gateway Server may do not run
        //        return false;
        //    }
        //}


        public static bool SendSMS(string mobileNo, string message, out string deliveryStatus)
        {
            deliveryStatus = string.Empty;

            //we creating the necessary URL string:
            //string URL = "http://sms6.routesms.com:8080/bulksms/bulksms"; //where the SMS Gateway is running
            string URL = "http://sms6.rmlconnect.net:8080/bulksms/bulksms"; //where the SMS Gateway is running
            string port = "2352"; //port number where the SMS Gateway is listening
            String senderId = "COMMAG";
            string username = "aegistrans"; //username for successful login
            string password = "ifZ87Cjb"; //user's password
            int type = 0; //type of message
            int dlr = 1;
            string recipients = mobileNo; //who will get the message
            string messageData = message; //body of message

            //String createdURL = "http://sms6.routesms.com:8080/bulksms/bulksms?username="
            //    + username + "&password=" + password + "&type=" + type + "&dlr=" + dlr +
            //    "&destination=" + recipients + "&source=" + senderId + "&message=" + messageData + "";

            String createdURL = "http://sms6.rmlconnect.net:8080/bulksms/bulksms?username="
                + username + "&password=" + password + "&type=" + type + "&dlr=" + dlr +
                "&destination=" + recipients + "&source=" + senderId + "&message=" + messageData + "";



            try
            {
                //Create the request and send data to the SMS Gateway Server by HTTP connection
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(createdURL);
                //Get response from the SMS Gateway Server and read the answer
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();

                deliveryStatus = GetSMSDeliveryResult(responseString);

                return true;
            }
            catch (Exception ex)
            {
                //if sending request or getting response is not successful the SMS Gateway Server may do
                return false;
            }
        }

        private static string GetSMSDeliveryResult(string response)
        {
            if (string.IsNullOrEmpty(response))
                return "Response is empty";

            var responseElements = response.Split('|');
            var responseStatusMessage = string.Empty;

            #region Switch
            switch (responseElements.First())
            {
                case "1701":
                    {
                        responseStatusMessage = "Success";
                    }
                    break;
                case "1702":
                    {
                        responseStatusMessage = "Invalid URL Error";
                    }
                    break;
                case "1703":
                    {
                        responseStatusMessage = "Invalid credentials";
                    }
                    break;
                case "1704":
                    {
                        responseStatusMessage = "Invalid value in 'type' parameter";
                    }
                    break;
                case "1705":
                    {
                        responseStatusMessage = "Invalid message";
                    }
                    break;
                case "1706":
                    {
                        responseStatusMessage = "Invalid destination";
                    }
                    break;
                case "1707":
                    {
                        responseStatusMessage = "Invalid source(Sender)";
                    }
                    break;
                case "1708":
                    {
                        responseStatusMessage = "Invalid value for 'dlr' parameter";
                    }
                    break;
                case "1709":
                    {
                        responseStatusMessage = "User validation failed";
                    }
                    break;
                case "1710":
                    {
                        responseStatusMessage = "Internal Error";
                    }
                    break;
                case "1025":
                    {
                        responseStatusMessage = "Insufficient credit";
                    }
                    break;
                case "1715":
                    {
                        responseStatusMessage = "Response timeout";
                    }
                    break;
                default:
                    {
                        responseStatusMessage = "Unknown error";
                    }
                    break;
            }
            #endregion

            return responseStatusMessage;
        }

        public static bool SendEmail(string ToMail, string MailBody, string MailSubject)
        {
            string mail = "tamcehelpdesk@gmail.com"; //"aegisconsultingservice@gmail.com"; //<--Enter your gmail id here
            string password = "support@acs123"; //"aegis123";//<--Enter gmail password here
            string FromMail = "tamcehelpdesk@gmail.com"; //"aegisconsultingservice@gmail.com";
            using (MailMessage mm = new MailMessage(FromMail, ToMail))
            {
                mm.Subject = MailSubject;
                mm.Body = MailBody;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(mail, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                return true;
            }
        }


        #endregion


        #region GenerateRandomNumber
        public static string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 7; i++)
            {
                r += random.Next(0, 6).ToString();
            }
            return r;
        }

        #endregion

        #region Unicode or captcha
        public static string GenerateUniqueCode()
        {
            Random random = new Random();
                string combination = "0123456789abcdefghijklmnopqrstuvwxyz";
                StringBuilder captcha = new StringBuilder();
                for (int i = 0; i < 6; i++)
                    captcha.Append(combination[random.Next(combination.Length)]);
            return captcha.ToString();
            
        }
        #endregion


    }
}
