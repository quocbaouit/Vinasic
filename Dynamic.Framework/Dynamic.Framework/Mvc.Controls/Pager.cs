using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Dynamic.Framework.Mvc.Controls
{
    public class Pager
    {
        private string _controllerName = string.Empty;
        private string _actionName = string.Empty;
        private int _pageSize = 10;
        private int _pageIndex = 1;
        private int _totalRecords = 0;
        private int _pageRange = 10;
        private string _totalText = "Total";
        private string _totalRecordsText = "records";
        private string _firstText = "First";
        private string _previousText = "Previous";
        private string _nextText = "Next";
        private string _lastText = "Last";
        private string _pageText = "Pages";
        private bool _showNote = true;
        private string _nextRangeText = "...";
        private string _previousRangeText = "...";
        private object _routeValues;
        private AjaxOptions _ajaxOption;

        public string ActionName
        {
            get
            {
                return this._actionName;
            }
            set
            {
                this._actionName = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                this._pageSize = value;
            }
        }

        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }
            set
            {
                this._pageIndex = value;
            }
        }

        public int TotalRecords
        {
            get
            {
                return this._totalRecords;
            }
            set
            {
                this._totalRecords = value;
            }
        }

        public int PageRange
        {
            get
            {
                return this._pageRange;
            }
            set
            {
                this._pageRange = value;
            }
        }

        public AjaxOptions AjaxOption
        {
            get
            {
                return this._ajaxOption;
            }
            set
            {
                this._ajaxOption = value;
            }
        }

        public string ControllerName
        {
            get
            {
                return this._controllerName;
            }
            set
            {
                this._controllerName = value;
            }
        }

        public object RouteValues
        {
            get
            {
                return this._routeValues;
            }
            set
            {
                this._routeValues = value;
            }
        }

        public string TotalRecordsText
        {
            get
            {
                return this._totalRecordsText;
            }
            set
            {
                this._totalRecordsText = value;
            }
        }

        public string TotalText
        {
            get
            {
                return this._totalText;
            }
            set
            {
                this._totalText = value;
            }
        }

        public string FirstText
        {
            get
            {
                return this._firstText;
            }
            set
            {
                this._firstText = value;
            }
        }

        public string PreviousText
        {
            get
            {
                return this._previousText;
            }
            set
            {
                this._previousText = value;
            }
        }

        public string NextText
        {
            get
            {
                return this._nextText;
            }
            set
            {
                this._nextText = value;
            }
        }

        public string LastText
        {
            get
            {
                return this._lastText;
            }
            set
            {
                this._lastText = value;
            }
        }

        public string PageText
        {
            get
            {
                return this._pageText;
            }
            set
            {
                this._pageText = value;
            }
        }

        public bool ShowNote
        {
            get
            {
                return this._showNote;
            }
            set
            {
                this._showNote = value;
            }
        }

        public string NextRangeText
        {
            get
            {
                return this._nextRangeText;
            }
            set
            {
                this._nextRangeText = value;
            }
        }

        public string PreviousRangeText
        {
            get
            {
                return this._previousRangeText;
            }
            set
            {
                this._previousRangeText = value;
            }
        }

        public Pager(string actionName, string controllerName, int pageIndex = 1, int pageSize = 10, int totalRecords = 0, AjaxOptions ajaxOption = null, object routeValues = null)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
            this.PageIndex = this.PageIndex;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.AjaxOption = ajaxOption;
            this.RouteValues = routeValues;
        }

        public Pager()
        {
        }

        public MvcHtmlString InitPagerHtml(HtmlHelper htmlHelper)
        {
            if (this.TotalRecords == 0)
                return MvcHtmlString.Empty;
            int num1 = this.TotalRecords / this.PageSize;
            if (this.TotalRecords % this.PageSize > 0)
                ++num1;
            string newValue1 = "<a href='#'>1</a>";
            string str1 = "<span>{totalText}:</span> <span>{totalRecords}</span> <span>{totalRecordsText}</span>  <span>{first}</span> <span> {previous}</span> <span> {numberRange}</span> <span>{next}</span> <span>{last}</span>  <span>{pageCount} pages</span>";
            if (string.IsNullOrEmpty(this.ControllerName) || string.IsNullOrEmpty(this.ActionName))
                throw new Exception("controllerName and actionName must be specified for PageLinks.");
            RouteValueDictionary routeValues = new RouteValueDictionary(this.RouteValues);
            routeValues.Add("controller", (object)this.ControllerName);
            routeValues.Add("action", (object)this.ActionName);
            routeValues.Add("page", (object)1);
            string newValue2 = LinkExtensions.RouteLink(htmlHelper, this.FirstText, routeValues).ToString();
            routeValues["page"] = (object)num1;
            string newValue3 = LinkExtensions.RouteLink(htmlHelper, this.LastText, routeValues).ToString();
            routeValues["page"] = (object)(this.PageIndex + 1);
            string newValue4 = LinkExtensions.RouteLink(htmlHelper, this.NextText, routeValues).ToString();
            routeValues["page"] = (object)(this.PageIndex - 1);
            string newValue5 = LinkExtensions.RouteLink(htmlHelper, this.PreviousText, routeValues).ToString();
            if (this.PageIndex == 1)
            {
                newValue2 = this.FirstText;
                newValue5 = this.PreviousText;
            }
            if (this.PageIndex == num1)
            {
                newValue4 = this.NextText;
                newValue3 = this.LastText;
            }
            if (num1 <= this.PageRange)
            {
                newValue1 = "";
                for (int index = 1; index <= num1; ++index)
                {
                    string str2;
                    if (this.PageIndex == index)
                    {
                        str2 = newValue1 + index.ToString();
                    }
                    else
                    {
                        routeValues["page"] = (object)index;
                        str2 = newValue1 + LinkExtensions.RouteLink(htmlHelper, index.ToString(), routeValues).ToString();
                    }
                    newValue1 = str2 + "  ";
                }
            }
            else
            {
                for (int index1 = 0; index1 <= num1 / this.PageRange; ++index1)
                {
                    int num2 = this.PageRange * index1;
                    int num3 = this.PageRange * (index1 + 1);
                    if (this.PageIndex > num2 && this.PageIndex <= num3)
                    {
                        newValue1 = "";
                        for (int index2 = num2 + 1; index2 <= num3; ++index2)
                        {
                            string str2;
                            if (this.PageIndex == index2)
                            {
                                str2 = newValue1 + index2.ToString();
                            }
                            else
                            {
                                routeValues["page"] = (object)index2;
                                str2 = newValue1 + LinkExtensions.RouteLink(htmlHelper, index2.ToString(), routeValues).ToString();
                            }
                            newValue1 = str2 + "  ";
                        }
                    }
                }
            }
            StringBuilder stringBuilder = new StringBuilder(str1);
            stringBuilder.Replace("{totalText}", this.TotalText).Replace("{totalRecords}", this.TotalRecords.ToString()).Replace("{totalRecordsText}", this.TotalRecordsText).Replace("{first}", newValue2).Replace("{totalRecordsText}", this.TotalRecordsText);
            stringBuilder.Replace("{previous}", newValue5).Replace("{numberRange}", newValue1).Replace("{next}", newValue4).Replace("{last}", newValue3).Replace("{pageCount}", num1.ToString());
            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        public MvcHtmlString InitPagerAjax(AjaxHelper ajaxHelper)
        {
            int num1 = this.TotalRecords / this.PageSize;
            if (this.TotalRecords % this.PageSize > 0)
                ++num1;
            RouteValueDictionary routeValues = new RouteValueDictionary(this.RouteValues);
            routeValues.Add("controller", (object)this.ControllerName);
            routeValues.Add("action", (object)this.ActionName);
            string firstText = this.FirstText;
            string previousText = this.PreviousText;
            string nextText = this.NextText;
            string lastText = this.LastText;
            routeValues.Add("page", (object)1);
            string str1 = this.PageIndex == 1 ? this.FirstText + " " : AjaxExtensions.RouteLink(ajaxHelper, this.FirstText, routeValues, this.AjaxOption).ToHtmlString() + " ";
            routeValues["page"] = (object)num1;
            string str2 = this.PageIndex == num1 ? this.LastText + " " : AjaxExtensions.RouteLink(ajaxHelper, this.LastText, routeValues, this.AjaxOption).ToHtmlString() + " ";
            routeValues["page"] = (object)(this.PageIndex + 1);
            string str3 = this.PageIndex == num1 ? this.NextText + " " : AjaxExtensions.RouteLink(ajaxHelper, this.NextText, routeValues, this.AjaxOption).ToHtmlString() + " ";
            routeValues["page"] = (object)(this.PageIndex - 1);
            string str4 = this.PageIndex == 1 ? this.PreviousText + " " : AjaxExtensions.RouteLink(ajaxHelper, this.PreviousText, routeValues, this.AjaxOption).ToHtmlString() + " ";
            string str5 = "";
            if (num1 <= this.PageRange)
            {
                str5 = "";
                for (int index = 1; index <= num1; ++index)
                {
                    string str6;
                    if (this.PageIndex == index)
                    {
                        str6 = str5 + index.ToString();
                    }
                    else
                    {
                        routeValues["page"] = (object)index;
                        str6 = str5 + AjaxExtensions.RouteLink(ajaxHelper, index.ToString(), routeValues, this.AjaxOption).ToHtmlString();
                    }
                    str5 = str6 + "  ";
                }
            }
            else
            {
                for (int index1 = 0; index1 <= num1 / this.PageRange; ++index1)
                {
                    int num2 = this.PageRange * index1;
                    int num3 = this.PageRange * (index1 + 1) > num1 ? num1 : this.PageRange * (index1 + 1);
                    if (this.PageIndex > num2 && this.PageIndex <= num3)
                    {
                        str5 = "";
                        if (num2 > 0)
                        {
                            routeValues["page"] = (object)num2;
                            str5 = str5 + AjaxExtensions.RouteLink(ajaxHelper, this.PreviousRangeText, routeValues, this.AjaxOption).ToHtmlString() + "  ";
                        }
                        for (int index2 = num2 + 1; index2 <= num3; ++index2)
                        {
                            string str6;
                            if (this.PageIndex == index2)
                            {
                                str6 = str5 + index2.ToString();
                            }
                            else
                            {
                                routeValues["page"] = (object)index2;
                                str6 = str5 + AjaxExtensions.RouteLink(ajaxHelper, index2.ToString(), routeValues, this.AjaxOption).ToHtmlString();
                            }
                            str5 = str6 + "  ";
                        }
                        if (num3 < num1)
                        {
                            routeValues["page"] = (object)(num3 + 1);
                            str5 = str5 + AjaxExtensions.RouteLink(ajaxHelper, this.NextRangeText, routeValues, this.AjaxOption).ToHtmlString() + "  ";
                        }
                    }
                }
            }
            string str7;
            if (!this.ShowNote)
                str7 = "";
            else
                str7 = string.Format("{0}:{1} {2} - {3}/{4} {5}: ", (object)this.TotalText, (object)this.TotalRecords, (object)this.TotalRecordsText, (object)this.PageIndex, (object)num1, (object)this.PageText);
            return MvcHtmlString.Create(str7 + str1 + str4 + str5 + str3 + str2);
        }
    }
}
