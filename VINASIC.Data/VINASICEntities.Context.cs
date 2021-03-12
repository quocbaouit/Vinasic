﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Dynamic.Framework;
using VINASIC.Object;

namespace VINASIC.Data
{
    public partial class VINASICEntities : DbContext
    {
        public VINASICEntities(string connectionString) :
            base(new System.Data.Objects.ObjectContext(new System.Data.EntityClient.EntityConnection(Dynamic.Framework.Infrastructure.Data.DataUltilities.GetMetaData("VINASICEntities"),
    			new System.Data.SqlClient.SqlConnection(connectionString))), true)
        {
    		(this as IObjectContextAdapter).ObjectContext.DefaultContainerName = "VINASICEntities";
    		this.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<T__Remind> T__Remind { get; set; }
        public DbSet<T_CommodityDictionary> T_CommodityDictionary { get; set; }
        public DbSet<T_Customer> T_Customer { get; set; }
        public DbSet<T_ErrorLog> T_ErrorLog { get; set; }
        public DbSet<T_Feature> T_Feature { get; set; }
        public DbSet<T_Machine> T_Machine { get; set; }
        public DbSet<T_Material> T_Material { get; set; }
        public DbSet<T_MaterialType> T_MaterialType { get; set; }
        public DbSet<T_Menu> T_Menu { get; set; }
        public DbSet<T_MenuCategory> T_MenuCategory { get; set; }
        public DbSet<T_Notification> T_Notification { get; set; }
        public DbSet<T_Organization> T_Organization { get; set; }
        public DbSet<T_Partner> T_Partner { get; set; }
        public DbSet<T_PaymentVoucher> T_PaymentVoucher { get; set; }
        public DbSet<T_Permission> T_Permission { get; set; }
        public DbSet<T_Position> T_Position { get; set; }
        public DbSet<T_Product> T_Product { get; set; }
        public DbSet<T_ProductType> T_ProductType { get; set; }
        public DbSet<T_Quittance> T_Quittance { get; set; }
        public DbSet<T_ReceiptVoucher> T_ReceiptVoucher { get; set; }
        public DbSet<T_RoLe> T_RoLe { get; set; }
        public DbSet<T_RolePermission> T_RolePermission { get; set; }
        public DbSet<T_Staff> T_Staff { get; set; }
        public DbSet<T_StockIn> T_StockIn { get; set; }
        public DbSet<T_StockInDetail> T_StockInDetail { get; set; }
        public DbSet<T_User> T_User { get; set; }
        public DbSet<T_UserRole> T_UserRole { get; set; }
        public DbSet<T_Timing> T_Timing { get; set; }
        public DbSet<T_OrderDetail> T_OrderDetail { get; set; }
        public DbSet<T_UserProduct> T_UserProduct { get; set; }
        public DbSet<T_Formular> T_Formular { get; set; }
        public DbSet<T_UserFormular> T_UserFormular { get; set; }
        public DbSet<T_Process> T_Process { get; set; }
        public DbSet<T_ProcessDetail> T_ProcessDetail { get; set; }
        public DbSet<T_StandardSale> T_StandardSale { get; set; }
        public DbSet<T_Order> T_Order { get; set; }
        public DbSet<T_Content> T_Content { get; set; }
        public DbSet<T_FormularDetail> T_FormularDetail { get; set; }
        public DbSet<T_SiteSetting> T_SiteSetting { get; set; }
        public DbSet<T_PaymentVoucherDetail> T_PaymentVoucherDetail { get; set; }
        public DbSet<T_Sticker> T_Sticker { get; set; }
        public DbSet<T_CustomerProduct> T_CustomerProduct { get; set; }
        public DbSet<T_OrderStatus> T_OrderStatus { get; set; }
        public DbSet<T_Status> T_Status { get; set; }
        public DbSet<T_Unit> T_Unit { get; set; }
        public DbSet<T_OrderDeposit> T_OrderDeposit { get; set; }
    }
}
