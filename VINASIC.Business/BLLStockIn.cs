using System;
using System.Collections.Generic;
using System.Linq;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using Dynamic.Framework.Mvc;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Business.Interface.Enum;
using VINASIC.Object;

namespace VINASIC.Business
{
    public class BllStockIn:IBllStockIn
    {
        private readonly IT_StockInRepository _repStockIn;      
        private readonly IT_StockInDetailRepository _repStockInDetail;
        private readonly IT_MaterialRepository _repMaterialRepository;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllStockIn(IUnitOfWork<VINASICEntities> unitOfWork, IT_StockInRepository repStockIn, IT_StockInDetailRepository repStockInDetail, IT_MaterialRepository repMaterialRepository)
        {
            _unitOfWork = unitOfWork;
            _repStockIn = repStockIn;
            _repStockInDetail = repStockInDetail;
            _repMaterialRepository = repMaterialRepository;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public PagedList<ModelStockIn> GetList(int employee, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var stockIns = _repStockIn.GetMany(c => !c.IsDeleted).Select(c => new ModelStockIn()
            {
  
                Id = c.Id,
                Name = c.Name,
                PartnerId = c.PartnerId,
                CustomerPhone = c.T_Partner.Mobile,
                CustomerEmail = c.T_Partner.Email,
                CustomerAddress = c.T_Partner.Address,
                CustomerTaxCode = c.T_Partner.TaxCode,
                Description = c.Description,
                SubTotal = c.SubTotal,
                IsPayment = c.IsPayment,
                IsApproval = c.IsApproval,
                CreatedUser = c.CreatedUser,
                StockInDate = c.StockInDate,
                CreatedDate = c.CreatedDate,
                T_StockInDetail = c.T_StockInDetail
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelStockIn>(stockIns, pageNumber, pageSize);
        }

        public List<ModelStockInDetail> GetListStockInDetailByStockInId(int stockInId)
        {
            var ordeDetails = _repStockInDetail.GetMany(o => !o.IsDeleted && o.StockInId == stockInId).Select(o => new
                ModelStockInDetail()
            {
                Id = o.Id,
                MateriaName = o.MateriaName,
                MaterialId = o.MaterialId,
                StockInId = o.StockInId,
                Description = o.Description,
                Quantity = o.Quantity,
                Price = o.Price,
                SubTotal = o.SubTotal,              
                CreatedDate = o.CreatedDate,
            }).ToList();
            return ordeDetails;
        }
        public ResponseBase CreateStockIn(ModelSaveStockIn obj, int userId)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    var stockIn = new T_StockIn
                    {
                        PartnerId = obj.PartnerId,
                        Name = obj.CustomerName,
                        Description = obj.Description,
                        SubTotal = obj.OrderTotal,
                        StockInDate = obj.DateDelivery,
                        IsPayment = false,
                        IsApproval = false,
                        IsDeleted = false,
                        CreatedUser = userId,
                        CreatedDate = DateTime.Now.AddHours(14)
                    };
                    _repStockIn.Add(stockIn);
                    SaveChange();
                    foreach (var detail in obj.Detail)
                    {
                        var stockInDetail = new T_StockInDetail
                        {
                            Index = detail.Index,
                            MaterialId = int.Parse(detail.MaterialId),
                            StockInId = stockIn.Id,
                            Quantity = detail.Quantity,
                            Price = detail.Price,
                            Description = detail.Description,
                            IsDeleted = false,
                            CreatedUser = userId,
                            CreatedDate = DateTime.Now.AddHours(14),
                            MateriaName = detail.MateriaName,
                            SubTotal = detail.SubTotal,
                        };
                        _repStockInDetail.Add(stockInDetail);
                        SaveChange();
                    }
                   // var listMaterial=_repMaterialRepository.GetMany(stockI)
                    result.IsSuccess = true;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase UpdatedStockIn(ModelSaveStockIn obj, int userId)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {                   
                    var stockIn  = _repStockIn.Get(x => x.Id == obj.StockInId);
                    stockIn.Name = obj.CustomerName;
                    stockIn.Description = obj.Description;
                    stockIn.StockInDate = obj.DateDelivery;
                    stockIn.SubTotal = obj.OrderTotal;
                    stockIn.IsPayment = false;
                    stockIn.IsApproval = false;
                    stockIn.IsDeleted = false;
                    stockIn.UpdatedUser = userId;
                    stockIn.UpatedDate = DateTime.Now.AddHours(14);
                    _repStockIn.Update(stockIn);
                    SaveChange();
                    var baseStockInDetail = _repStockInDetail.GetMany(x => x.StockInId == stockIn.Id).ToList();
                    foreach (var detail in baseStockInDetail)
                    {
                        _repStockInDetail.Delete(detail);
                        SaveChange();
                    }
                    foreach (var detail in obj.Detail)
                    {
                        var stockInDetail = new T_StockInDetail
                        {
                            Index = detail.Index,
                            MaterialId = int.Parse(detail.MaterialId),
                            StockInId = stockIn.Id,
                            Quantity = detail.Quantity,
                            Price = detail.Price,
                            Description = detail.Description,
                            IsDeleted = false,
                            CreatedUser = userId,
                            CreatedDate = DateTime.Now.AddHours(14),
                            MateriaName = detail.MateriaName,
                            SubTotal = detail.SubTotal,
                        };
                        _repStockInDetail.Add(stockInDetail);
                        SaveChange();
                    }
                    result.IsSuccess = true;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create ProductType", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var stockin = _repStockIn.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (stockin != null)
            {
                stockin.IsDeleted = true;
                stockin.DeletedUser = userId;
                stockin.DeletedDate = DateTime.Now.AddHours(14);
                _repStockIn.Update(stockin);
                SaveChange();
                responResult.IsSuccess = true;
            }
            else
            {
                responResult.IsSuccess = false;
                responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
            }
            return responResult;
        }
        public List<ModelViewStockDetail> ExportReport(DateTime fromDate, DateTime toDate, string keyWord)
        {
            var frDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0, 0);
            var tDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59, 999);
            var orders =
                _repStockInDetail.GetMany(c => !c.IsDeleted && !c.T_StockIn.IsDeleted )
                    .Select(c => new ModelViewStockDetail()
                    {
                        CreatedDate = c.CreatedDate,
                        Id = c.Id,
                        StockId = c.StockInId,        
                        MateriaName=c.MateriaName,
                        Name=c.T_StockIn.Name,
                        Description = c.Description,                       
                        Quantity = c.Quantity,
                        Price = c.Price,
                        SubTotal = c.SubTotal,
                        Total = c.T_StockIn.SubTotal,
                    }).ToList();
            if (!string.IsNullOrEmpty(keyWord))
            {
                orders = orders.Where(c => c.T_StockIn.Name.Trim().ToLower().Contains(keyWord.Trim().ToLower()) || c.MateriaName.Contains(keyWord)).ToList();
            }        
            var sum = orders.Sum(x => x.SubTotal);
            if (orders.Count > 0)
            {
                orders[0].Total = sum;
            }
            return orders;

        }
    }
}
