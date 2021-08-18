using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;


namespace Capstone.Framework
{
    public class ExceptionHandling
    {
        public static ExceptionModel GetExceptionDetails(Exception ex)
        {
            StackTrace objStackTrace = new StackTrace(ex, true);
            StackFrame objStackFrame = objStackTrace.GetFrame(objStackTrace.FrameCount - 1);
            ExceptionModel objExcEntity = new ExceptionModel();

            try
            {
                if (System.Web.HttpContext.Current.Handler.GetType().Namespace == "ASP") // Checks whether Handler is from Page or service
                {
                    Page currentPage = (System.Web.UI.Page)HttpContext.Current.Handler;
                    if (currentPage.Title != null && currentPage.Title != "")
                    {
                        objExcEntity.RequestPage = currentPage.Title;
                    }
                    else
                    {
                        objExcEntity.RequestPage = currentPage.ToString();
                    }
                }
                objExcEntity.ClassName = objStackFrame.GetFileName();
                objExcEntity.MethodName = objStackFrame.GetMethod().Name;
                objExcEntity.LineNo = objStackFrame.GetFileLineNumber();
                objExcEntity.ErrorMessage = ex.Message;

               
            }
            catch
            {
                //Nothing to do;
            }
            return objExcEntity;
        }
    }
}
