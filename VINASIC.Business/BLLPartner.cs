using System;
using System.Collections.Generic;
using System.Linq;
using Dynamic.Framework;
using Dynamic.Framework.Infrastructure.Data;
using Dynamic.Framework.Mvc;
using GPRO.Ultilities;
using PagedList;
using VINASIC.Business.Interface;
using VINASIC.Business.Interface.Model;
using VINASIC.Data;
using VINASIC.Data.Repositories;
using VINASIC.Object;

namespace VINASIC.Business
{
    public class BllPartner: IBllPartner
    {
        private readonly IT_PartnerRepository _repPartner;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllPartner(IUnitOfWork<VINASICEntities> unitOfWork, IT_PartnerRepository repPartner)
        {
            _unitOfWork = unitOfWork;
            _repPartner= repPartner;
        }
        private void SaveChange()
        {

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool CheckPartnerName(string partnerName, int id)
        {
            var checkResult = false;
            try
            {
                var checkName = _repPartner.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(partnerName.Trim().ToUpper())).FirstOrDefault();
                if (checkName == null)
                    checkResult = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return checkResult;
        }
        public List<ModelPartner> GetListProduct()
        {
            List<ModelPartner> partner;
            try
            {
                partner= _repPartner.GetMany(c => !c.IsDeleted).Select(c => new ModelPartner()
                {
                    Id = c.Id,

                    Name = c.Name,
                    CreatedDate = c.CreatedDate,
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return partner;
        }
        public ResponseBase Create(ModelPartner obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckPartnerName(obj.Name, obj.Id))
                    {

                        var partner= new T_Partner();
                        Parse.CopyObject(obj, ref partner);
                        partner.CreatedDate = DateTime.Now.AddHours(14);
                        _repPartner.Add(partner);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Partner", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Partner", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseBase Update(ModelPartner obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (!CheckPartnerName(obj.Name, obj.Id))
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdatePartner", Message = "Trùng Tên. Vui lòng chọn lại" });
                }
                else
                {
                    T_Partner partner= _repPartner.Get(x => x.Id == obj.Id && !x.IsDeleted);
                    if (partner!= null)
                    {
                        partner.Name = obj.Name;
                        partner.Address = obj.Address;
                        partner.Email = obj.Email;
                        partner.Mobile = obj.Mobile;
                        partner.TaxCode = obj.TaxCode;
                        partner.UpdatedDate = DateTime.Now.AddHours(14);
                        partner.UpdatedUser = obj.UpdatedUser;
                        _repPartner.Update(partner);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "UpdatePartner", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                    }
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
            ResponseBase responResult;

            try
            {
                responResult = new ResponseBase();
                var partner= _repPartner.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
                if (partner!= null)
                {
                    partner.IsDeleted = true;
                    partner.DeletedUser = userId;
                    partner.DeletedDate = DateTime.Now.AddHours(14);
                    _repPartner.Update(partner);
                    SaveChange();
                    responResult.IsSuccess = true;
                }
                else
                {
                    responResult.IsSuccess = false;
                    responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responResult;
        }
        public List<ModelSelectItem> GetListPartner()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Nhà cung cấp----"}
            };
            try
            {
                listModelSelect.AddRange(_repPartner.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModelSelect;
        }
        public PagedList<ModelPartner> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    sorting = "CreatedDate DESC";
                }
                var Partners = _repPartner.GetMany(c => !c.IsDeleted).Select(c => new ModelPartner()
                {
                    Id = c.Id,
                    Email = c.Address,
                    Address = c.Address,
                    Name = c.Name,
                    Mobile = c.Mobile,
                    TaxCode = c.TaxCode,
                    CreatedDate = c.CreatedDate
                }).OrderBy(sorting);
                var pageNumber = (startIndexRecord / pageSize) + 1;
                return new PagedList<ModelPartner>(Partners, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T_Partner GetPartnerById(int id)
        {
            var customer = _repPartner.Get(x => !x.IsDeleted && x.Id == id);
            return customer;
        }

    }
}

