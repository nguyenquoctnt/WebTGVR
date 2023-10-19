using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using VDMMutiline.Core.UserSecurity;

namespace VDMMutiline.Core.Extensions
{
    public static class HtmlExtension
    {
        // Methods
        private static string BuildAttribute(object attr, bool isExcludeClass)
        {
            StringBuilder builder = new StringBuilder();
            PropertyInfo[] properties = attr.GetType().GetProperties();
            bool flag = false;
            foreach (PropertyInfo info in properties)
            {
                if (!isExcludeClass || !info.Name.ToLower().Equals("class"))
                {
                    if (!flag && info.Name.ToLower().Equals("type"))
                    {
                        flag = true;
                    }
                    builder.Append(info.Name).Append("=").Append("\"");
                    builder.Append(info.GetValue(attr)).Append("\" ");
                }
            }
            if (!flag)
            {
                builder.Append(" type=\"button\" ");
            }
            return builder.ToString();
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title)
        {
            return helper.Button(title, null, null);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title, object attr)
        {
            return helper.Button(title, null, attr);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title, string icon)
        {
            return helper.Button(title, icon, null);
        }

        public static MvcHtmlString Button<TModel, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, string title, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.Button<TModel, AuthorizedTProperty>(title, null, null, authorizedExpression);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title, string icon, object attr)
        {
            StringBuilder builder = new StringBuilder("<button");
            if (attr != null)
            {
                builder.Append(" ").Append(BuildAttribute(attr, false));
            }
            builder.Append(">");
            if (!string.IsNullOrEmpty(icon))
            {
                builder.Append("<i class='" + icon + "'></i> ");
                builder.Append(title);
            }
            builder.Append("</button>");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString Button<TModel, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, string title, object attr, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.Button<TModel, AuthorizedTProperty>(title, null, attr, authorizedExpression);
        }

        public static MvcHtmlString Button<TModel, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, string title, string imageUrl, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.Button<TModel, AuthorizedTProperty>(title, imageUrl, null, authorizedExpression);
        }

        public static MvcHtmlString Button<TModel, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, string title, string imageUrl, object attr, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            StringBuilder builder = new StringBuilder("<button");
            if (attr != null)
            {
                builder.Append(" ").Append(BuildAttribute(attr, !string.IsNullOrEmpty(imageUrl)));
            }
            if (!GetValueFromExpression<TModel, AuthorizedTProperty>(htmlHelper, authorizedExpression))
            {
                return null;
            }
            if (string.IsNullOrEmpty(imageUrl))
            {
                builder.Append(">");
                builder.Append(title);
            }
            else
            {
                builder.Append("class=\"image-button image-left\">");
                UrlHelper helper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                builder.Append("<img src=\"").Append(helper.Content(imageUrl)).Append("\">");
                builder.Append(title);
            }
            builder.Append("</button>");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString ButtonExt(this HtmlHelper helper, string title, string permissionCode)
        {
            return helper.ButtonExt(title, null, null, permissionCode);
        }

        public static MvcHtmlString ButtonExt(this HtmlHelper helper, string title, object attr, string permissionCode)
        {
            return helper.ButtonExt(title, null, attr, permissionCode);
        }

        public static MvcHtmlString ButtonExt(this HtmlHelper helper, string title, string imageUrl, string permissionCode)
        {
            return helper.ButtonExt(title, imageUrl, null, permissionCode);
        }

        public static MvcHtmlString ButtonExt(this HtmlHelper helper, string title, string imageUrl, object attr, string roleName)
        {
            StringBuilder builder;
            if (!HasRole(roleName))
            {
                builder = new StringBuilder("<button disabled ");
            }
            else
            {
                builder = new StringBuilder("<button ");
            }
            if (attr != null)
            {
                builder.Append(" ").Append(BuildAttribute(attr, !string.IsNullOrEmpty(imageUrl)));
            }
            if (string.IsNullOrEmpty(imageUrl))
            {
                builder.Append(">");
                builder.Append(title);
            }
            else
            {
                builder.Append("class=\"image-button image-left\">");
                UrlHelper helper2 = new UrlHelper(helper.ViewContext.RequestContext);
                builder.Append("<img src=\"").Append(helper2.Content(imageUrl)).Append("\">");
                builder.Append(title);
            }
            builder.Append("</button>");
            return new MvcHtmlString(builder.ToString());
        }

        private static void ConvertAttrToDictionary(object attr, IDictionary<string, string> dicAttr)
        {
            PropertyInfo[] properties = attr.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if ((!info.Name.ToLower().Equals("id") && !info.Name.ToLower().Equals("name")) && !info.Name.ToLower().Equals("class"))
                {
                    if (!dicAttr.Keys.Contains(info.Name))
                    {
                        dicAttr.Add(info.Name, info.GetValue(attr).ToString());
                    }
                    else
                    {
                        dicAttr[info.Name] = info.GetValue(attr).ToString();
                    }
                }
            }
        }

        public static MvcHtmlString CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, bool isChecked, Expression<Func<TModel, bool>> authorizedExpression)
        {
            return htmlHelper.CheckBox<TModel>(name, isChecked, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object htmlAttributes, Expression<Func<TModel, bool>> authorizedExpression)
        {
            return htmlHelper.CheckBox<TModel>(name, false, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, bool isChecked, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, bool>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, bool>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.CheckBox(name, isChecked, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("disabled"))
            {
                dictionary.Add("disabled", "disabled");
            }
            return htmlHelper.CheckBox(name, isChecked, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, bool isChecked, object htmlAttributes, Expression<Func<TModel, bool>> authorizedExpression)
        {
            return htmlHelper.CheckBox<TModel>(name, isChecked, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, Expression<Func<TModel, bool>> authorizedExpression)
        {
            return htmlHelper.CheckBoxFor<TModel>(expression, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, bool>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, bool>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.CheckBoxFor<TModel>(expression, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("disabled"))
            {
                dictionary.Add("disabled", "disabled");
            }
            return htmlHelper.CheckBoxFor<TModel>(expression, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes, Expression<Func<TModel, bool>> authorizedExpression)
        {
            return htmlHelper.CheckBoxFor<TModel>(expression, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, DateTime? value)
        {
            return RenderCalendar(name, value, string.Empty, string.Empty, new object());
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, DateTime? value, object htmlAttributes)
        {
            return RenderCalendar(name, value, string.Empty, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, DateTime? value, string format, string locale)
        {
            object htmlAttributes = new object();
            return RenderCalendar(name, value, format, locale, htmlAttributes);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, DateTime? value, string format, string locale, object htmlAttributes)
        {
            return RenderCalendar(name, value, format, locale, htmlAttributes);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.DatePickerFor<TModel, TProperty>(expression, string.Empty, string.Empty, new object());
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.DatePickerFor<TModel, TProperty>(expression, string.Empty, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, string locale, object htmlAttributes)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            DateTime? nullable = null;
            object model = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;
            if (model != null)
            {
                nullable = new DateTime?(Convert.ToDateTime(model, CultureInfo.CurrentCulture));
            }
            return RenderCalendar(fullHtmlFieldName, nullable, format, locale, htmlAttributes);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, null, null, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<SelectListItem> selectList, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, selectList, null, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, string optionLabel, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, null, optionLabel, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, selectList, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, selectList, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, selectList, optionLabel, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, TProperty>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("disabled"))
            {
                dictionary.Add("disabled", "disabled");
            }
            return htmlHelper.DropDownList(name, selectList, optionLabel, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString DropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownList<TModel, TProperty>(name, selectList, optionLabel, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownListFor<TModel, TProperty, AuthorizedTProperty>(expression, selectList, null, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownListFor<TModel, TProperty, AuthorizedTProperty>(expression, selectList, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownListFor<TModel, TProperty, AuthorizedTProperty>(expression, selectList, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownListFor<TModel, TProperty, AuthorizedTProperty>(expression, selectList, optionLabel, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, AuthorizedTProperty>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.DropDownListFor<TModel, TProperty>(expression, selectList, optionLabel, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("disabled"))
            {
                dictionary.Add("disabled", "disabled");
            }
            return htmlHelper.DropDownListFor<TModel, TProperty>(expression, selectList, optionLabel, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.DropDownListFor<TModel, TProperty, AuthorizedTProperty>(expression, selectList, optionLabel, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static bool GetValueFromExpression<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return Convert.ToBoolean(ModelMetadata.FromLambdaExpression<TModel, TProperty>(authorizedExpression, htmlHelper.ViewData).Model, CultureInfo.CurrentCulture);
        }

        private static bool HasRole(string roleName)
        {
            return DependencyResolver.Current.GetService<IUserSecurity>().HasRole(roleName);
        }

        private static MvcHtmlString RenderCalendar(string name, DateTime? value, string format, string locale, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("div");
            TagBuilder builder2 = new TagBuilder("input");
            builder2.MergeAttribute("type", "text");
            builder.Attributes["style"] = "width: 150px;";
            if (!string.IsNullOrEmpty(name))
            {
                builder2.MergeAttribute("id", name.Replace(".", "_"));
                builder2.MergeAttribute("name", name);
            }
            PropertyInfo property = htmlAttributes.GetType().GetProperty("class", BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null)
            {
                string str2 = property.GetValue(htmlAttributes).ToString();
                builder.AddCssClass(str2);
                if (!str2.Contains("datepicker"))
                {
                    builder.AddCssClass("datepicker");
                }
                if (!str2.Contains("text"))
                {
                    builder.AddCssClass("text");
                }
                if (!str2.Contains("input-control"))
                {
                    builder.AddCssClass("input-control");
                }
            }
            else
            {
                builder.AddCssClass("input-control text datepicker");
            }
            builder.Attributes.Add("data-effect", "fade");
            if (!string.IsNullOrEmpty(format))
            {
                builder.Attributes.Add("data-format", format);
            }
            else
            {
                builder.Attributes.Add("data-format", "mm/dd/yyyy");
            }
            if (value.HasValue && (value.Value != DateTime.MinValue))
            {
                builder.Attributes.Add("data-date", value.Value.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(format))
                {
                    builder2.MergeAttribute("value", value.Value.ToString(format));
                }
                else
                {
                    builder2.MergeAttribute("value", value.Value.ToShortDateString());
                }
            }
            if (!string.IsNullOrEmpty(locale))
            {
                builder.MergeAttribute("data-locale", locale);
            }
            ConvertAttrToDictionary(htmlAttributes, builder.Attributes);
            StringBuilder builder3 = new StringBuilder();
            builder3.Append(builder2.ToString(TagRenderMode.SelfClosing));
            builder3.Append("<button class=\"btn-date\" type=\"button\" ></button>");
            builder.InnerHtml = builder3.ToString();
            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString TextAreaFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextAreaFor<TModel, TProperty, AuthorizedTProperty>(expression, 0, 0, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString TextAreaFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextAreaFor<TModel, TProperty, AuthorizedTProperty>(expression, 0, 0, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString TextAreaFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextAreaFor<TModel, TProperty, AuthorizedTProperty>(expression, 0, 0, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString TextAreaFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, AuthorizedTProperty>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.TextAreaFor<TModel, TProperty>(expression, rows, columns, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("readonly"))
            {
                dictionary.Add("readonly", "readonly");
            }
            return htmlHelper.TextAreaFor<TModel, TProperty>(expression, rows, columns, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString TextAreaFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, object htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextAreaFor<TModel, TProperty, AuthorizedTProperty>(expression, rows, columns, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.TextBox<TModel, TProperty>(name, null, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, object value, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.TextBox<TModel, TProperty>(name, value, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.TextBox<TModel, TProperty>(name, value, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, object value, object htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.TextBox<TModel, TProperty>(name, value, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, object value, string format, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.TextBox<TModel, TProperty>(name, value, format, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, object value, string format, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, TProperty>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.TextBox(name, value, format, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("readonly"))
            {
                dictionary.Add("readonly", "readonly");
            }
            return htmlHelper.TextBox(name, value, format, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string name, object value, string format, object htmlAttributes, Expression<Func<TModel, TProperty>> authorizedExpression)
        {
            return htmlHelper.TextBox<TModel, TProperty>(name, value, format, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString TextboxExt(this HtmlHelper helper, string permissionCode)
        {
            return helper.TextboxExt(null, null, permissionCode);
        }

        public static MvcHtmlString TextboxExt(this HtmlHelper helper, string title, string permissionCode)
        {
            return helper.TextboxExt(title, null, permissionCode);
        }

        public static MvcHtmlString TextboxExt(this HtmlHelper helper, string title, object attr, string roleName)
        {
            StringBuilder builder;
            if (!HasRole(roleName))
            {
                builder = new StringBuilder("<Input type='text' disabled='disabled' ");
            }
            else
            {
                builder = new StringBuilder("<Input type='text' ");
            }
            if (attr != null)
            {
                builder.Append(" ").Append(BuildAttribute(attr, false));
            }
            builder.Append(title);
            builder.Append("/>");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextBoxFor<TModel, TProperty, AuthorizedTProperty>(expression, null, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextBoxFor<TModel, TProperty, AuthorizedTProperty>(expression, null, htmlAttributes, authorizedExpression);
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextBoxFor<TModel, TProperty, AuthorizedTProperty>(expression, null, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextBoxFor<TModel, TProperty, AuthorizedTProperty>(expression, format, ((IDictionary<string, object>)new RouteValueDictionary()), authorizedExpression);
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            bool valueFromExpression = GetValueFromExpression<TModel, AuthorizedTProperty>(htmlHelper, authorizedExpression);
            if (valueFromExpression)
            {
                return htmlHelper.TextBoxFor<TModel, TProperty>(expression, format, htmlAttributes);
            }
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!htmlAttributes.ContainsKey("readonly"))
            {
                dictionary.Add("readonly", "readonly");
            }
            return htmlHelper.TextBoxFor<TModel, TProperty>(expression, format, ((IDictionary<string, object>)dictionary));
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty, AuthorizedTProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes, Expression<Func<TModel, AuthorizedTProperty>> authorizedExpression)
        {
            return htmlHelper.TextBoxFor<TModel, TProperty, AuthorizedTProperty>(expression, format, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), authorizedExpression);
        }
    }
}
