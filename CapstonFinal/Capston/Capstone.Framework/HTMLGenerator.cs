using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Capstone.Framework
{
    public static class HTMLGenerator
    {
        public static string ToHtmlTable<EntityType>(this EntityType objEntity, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle)
        {
            try
            {
                var result = new StringBuilder();
                if (String.IsNullOrEmpty(tableSyle))
                {
                    result.Append("<table id=\"" + typeof(EntityType).Name + "Table\">");
                }
                else
                {
                    result.Append("<table id=\"" + typeof(EntityType).Name + "Table\" class=\"" + tableSyle + "\">");
                }

                if (String.IsNullOrEmpty(headerStyle))
                {
                    result.AppendFormat("<tr><th>Entity Name </th><th>Entity Value</th></tr>");
                }
                else
                {
                    result.AppendFormat("<tr><th class=\"{0}\">Entity Name</th><th>Entity Value</th></tr>", headerStyle);
                }

                var propertyArray = typeof(EntityType).GetProperties();

                foreach (var prop in propertyArray)
                {
                    object value = prop.GetValue(objEntity, null);
                    result.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", prop.Name, value ?? String.Empty);
                }
                result.Append("</table>");

                return result.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string ToHtmlTable<T>(this IEnumerable<T> list, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle)
        {
            try
            {
                var result = new StringBuilder();
                if (String.IsNullOrEmpty(tableSyle))
                {
                    result.Append("<table id=\"" + typeof(T).Name + "Table\">");
                }
                else
                {
                    result.Append("<table id=\"" + typeof(T).Name + "Table\" class=\"" + tableSyle + "\">");
                }

                var propertyArray = typeof(T).GetProperties();

                foreach (var prop in propertyArray)
                {
                    if (String.IsNullOrEmpty(headerStyle))
                    {
                        result.AppendFormat("<th>{0}</th>", prop.Name);
                    }
                    else
                    {
                        result.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, prop.Name);
                    }
                }


                for (int i = 0; i < list.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(rowStyle) && !String.IsNullOrEmpty(alternateRowStyle))
                    {
                        result.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);
                    }

                    else
                    {
                        result.AppendFormat("<tr>");
                    }
                    foreach (var prop in propertyArray)
                    {
                        object value = prop.GetValue(list.ElementAt(i), null);
                        result.AppendFormat("<td>{0}</td>", value ?? String.Empty);
                    }
                    result.AppendLine("</tr>");
                }

                result.Append("</table>");

                return result.ToString();
            }
            catch
            {
                return null;
            }
        }

        // If we dont want to use Generics the following 2 methods can be used
        public static string ToHtmlTableFromItem(this object objEntity, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle)
        {
            try
            {
                var result = new StringBuilder();
                if (objEntity != null)
                {
                    Type EntityType = objEntity.GetType();
                    if (String.IsNullOrEmpty(tableSyle))
                    {
                        result.Append("<table id=\"" + EntityType.Name + "Table\">");
                    }
                    else
                    {
                        result.Append("<table id=\"" + EntityType.Name + "Table\" class=\"" + tableSyle + "\">");
                    }

                    if (String.IsNullOrEmpty(headerStyle))
                    {
                        result.AppendFormat("<tr><th>Entity Name </th><th>Entity Value</th></tr>");
                    }
                    else
                    {
                        result.AppendFormat("<tr><th class=\"{0}\">Entity Name</th><th>Entity Value</th></tr>", headerStyle);
                    }

                    var propertyArray = EntityType.GetProperties();

                    foreach (var prop in propertyArray)
                    {
                        object value = prop.GetValue(objEntity, null);
                        result.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", prop.Name, value ?? String.Empty);
                    }
                    result.Append("</table>");
                }
                return result.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string ToHtmlTableFromList(this object objEntitylist, string tableSyle, string headerStyle, string rowStyle, string alternateRowStyle)
        {
            try
            {
                var result = new StringBuilder();
                if (objEntitylist != null)
                {
                    Type ChildListType = objEntitylist.GetType();
                    Type ChildEntityType = ChildListType.GetGenericArguments()[0];
                    IList list = (IList)objEntitylist;

                    if (String.IsNullOrEmpty(tableSyle))
                    {
                        result.Append("<table id=\"" + ChildEntityType.Name + "Table\">");
                    }
                    else
                    {
                        result.Append("<table id=\"" + ChildEntityType.Name + "Table\" class=\"" + tableSyle + "\">");
                    }

                    var propertyArray = ChildEntityType.GetProperties();

                    foreach (var prop in propertyArray)
                    {
                        if (String.IsNullOrEmpty(headerStyle))
                        {
                            result.AppendFormat("<th>{0}</th>", prop.Name);
                        }
                        else
                        {
                            result.AppendFormat("<th class=\"{0}\">{1}</th>", headerStyle, prop.Name);
                        }
                    }


                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(rowStyle) && !String.IsNullOrEmpty(alternateRowStyle))
                        {
                            result.AppendFormat("<tr class=\"{0}\">", i % 2 == 0 ? rowStyle : alternateRowStyle);
                        }

                        else
                        {
                            result.AppendFormat("<tr>");
                        }
                        foreach (var prop in propertyArray)
                        {
                            object value = prop.GetValue(list[i], null);
                            result.AppendFormat("<td>{0}</td>", value ?? String.Empty);
                        }
                        result.AppendLine("</tr>");
                    }

                    result.Append("</table>");
                }
                return result.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
