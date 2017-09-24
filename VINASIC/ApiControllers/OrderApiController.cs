using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;
using Dynamic.Framework.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Infrastructure.ActionExtention;


namespace VINASIC.Controllers
{
    [AllowAnonymous]
    public class OrderApiController : ApiController
    {
        private readonly IBllOrder _bllOrder;
        private readonly IBllProductType _bllProductType;
        private readonly IBllProduct _bllProduct;
        private readonly IBllEmployee _bllEmployee;
        private readonly IBllCustomer _bllCustomer;
        /// <summary>
        /// 
        /// </summary>
        /// 

        public OrderApiController(IBllOrder bllOrder, IBllEmployee bllEmployee, IBllCustomer bllCustomer, IBllProductType bllProductType, IBllProduct bllProduct)
        {
            _bllOrder = bllOrder;
            _bllEmployee = bllEmployee;
            _bllCustomer = bllCustomer;
            _bllProductType = bllProductType;
            _bllProduct = bllProduct;
        }
        [HttpGet]
        public string Get(string id)
        {
            return "Test";
        }
        [HttpGet]
        [ActionName("GetById")]
        public string GetById(int Id)
        {
            return "Test";
        }
    }
}

