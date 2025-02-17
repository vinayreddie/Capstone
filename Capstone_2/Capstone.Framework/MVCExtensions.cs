using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Capstone.Framework
{
    public static class ModelStateErrorHandler
    {
        public static List<string> GetModelStateErrors
                                              (ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
        public static void RemoveError(ModelStateDictionary modelState, string errorKey)
        {
            if (modelState["model." + errorKey] != null)
                modelState["model." + errorKey].Errors.Clear();
            else if (modelState[errorKey] != null)
                modelState[errorKey].Errors.Clear();
        }
    }
}
