using System;
using System.Collections.Generic;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelDashBoard
    {
        public ModelDashBoardOrder ModelDashBoardOrder { get; set; }
        public ModelDashBoardPayment ModelDashBoardPayment { get; set; }
        public ModelDashBoardSum ModelDashBoardSum { get; set; }
        public ModelDashBoard()
        {
            ModelDashBoardOrder = new ModelDashBoardOrder();
            ModelDashBoardPayment = new ModelDashBoardPayment();
            ModelDashBoardSum = new ModelDashBoardSum();
        }
    }
    public class ModelDashBoardOrder
    {
        public double ? Value1 { get; set; }
        public double? Value2 { get; set; }
        public double? Value3 { get; set; }
    }
    public class ModelDashBoardPayment
    {
        public double? Value1 { get; set; }
        public double? Value2 { get; set; }
    }
    public class ModelDashBoardSum
    {
        public double? Value1 { get; set; }
        public double? Value2 { get; set; }
        public double? Value3 { get; set; }
        public double? Value4 { get; set; }
    }
}

