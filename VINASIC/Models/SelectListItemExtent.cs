using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VINASIC.Models
{
    public class SelectListItemExtent: SelectListItem
    {
        public int Type { get; set; }
    }
}