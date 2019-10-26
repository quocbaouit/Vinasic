﻿using System;
using System.Collections.Generic;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelContent : T_Content
    {
    }
    public class ProductPrice
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Price { get; set; }
        public bool isFixed { get; set; }
    }
    public class ListProductPrice
    {
        public string Code { get; set; }
        public List<ProductPrice> Products { get; set; }

        public ListProductPrice()
        {
            Products = new List<ProductPrice>();
        }
    }
}

