using System;
using System.Collections.Generic;
using System.Linq;
using GPRO.Ultilities;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using Dynamic.Framework.Mvc;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;

namespace VINASIC.Business
{
    public class BllStandardSale : IBllStandardSale
    {
        private readonly IT_StandardSaleRepository _repStandardSale;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllStandardSale(IUnitOfWork<VINASICEntities> unitOfWork, IT_StandardSaleRepository repStandardSale)
        {
            _unitOfWork = unitOfWork;
            _repStandardSale = repStandardSale;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }
        public ResponseBase Create(ModelStandardSale obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    var standardSale = new T_StandardSale();
                    Parse.CopyObject(obj, ref standardSale);
                    standardSale.CreatedDate = DateTime.Now.AddHours(14);
                    _repStandardSale.Add(standardSale);
                    SaveChange();
                    result.IsSuccess = true;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create StandardSale", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create StandardSale", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelStandardSale obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            T_StandardSale standardSale = _repStandardSale.Get(x => x.Id == obj.Id && !x.IsDeleted);
            if (standardSale != null)
            {
                standardSale.BaseSalary = obj.BaseSalary;
                standardSale.Bonus = obj.Bonus;
                standardSale.Percent = obj.Percent;
                standardSale.Sales = obj.Sales;
                standardSale.IncomeTotal = obj.IncomeTotal;

                standardSale.UpdatedDate = DateTime.Now.AddHours(14);
                standardSale.UpdatedUser = obj.UpdatedUser;
                _repStandardSale.Update(standardSale);
                SaveChange();
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateStandardSale", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
            }

            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var standardSale = _repStandardSale.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (standardSale != null)
            {
                standardSale.IsDeleted = true;
                standardSale.DeletedUser = userId;
                standardSale.DeletedDate = DateTime.Now.AddHours(14);
                _repStandardSale.Update(standardSale);
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
        public List<ModelSelectItem> GetListStandardSale()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Percent----"}
            };
            listModelSelect.AddRange(_repStandardSale.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Percent.ToString() }));
            return listModelSelect;
        }
        public PagedList<ModelStandardSale> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "Sales ASC";
            }
            var standardSales = _repStandardSale.GetMany(c => !c.IsDeleted).Select(c => new ModelStandardSale()
            {
                Id = c.Id,
               BaseSalary = c.BaseSalary,
               Sales = c.Sales,
               Percent = c.Percent,
               Bonus = c.Bonus,
               IncomeTotal = c.IncomeTotal,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            
                var result= new PagedList<ModelStandardSale>(standardSales, pageNumber, pageSize);
            foreach (var standard in result)
            {
                standard.strBaseSalary = $"{standard.BaseSalary:0,0}";
                standard.strSales = $"{standard.Sales:0,0}";
                standard.strBonus = $"{standard.Bonus:0,0}";
                standard.strIncomeTotal = $"{standard.IncomeTotal:0,0}";
            }
            return result;
        }
    }
}

