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
    public class BllElementFormular : IBllElementFormular
    {
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllElementFormular(IUnitOfWork<VINASICEntities> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckElementFormularName(string elementFormularName, int id)
        {
            //var checkResult = false;
            //var checkName = _repElementFormular.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(elementFormularName.Trim().ToUpper())).FirstOrDefault();
            //if (checkName == null)
            //    checkResult = true;
            //return checkResult;
           return true;
        }
        public List<ModelElementFormular> GetListProduct()
        {
            //var elementFormular = _repElementFormular.GetMany(c => !c.IsDeleted).Select(c => new ModelElementFormular()
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Description = c.Description,
            //    CreatedDate = c.CreatedDate,
            //}).ToList();
            //return elementFormular;
            return null;
        }

        public ResponseBase Create(ModelElementFormular obj)
        {
            ResponseBase result = new ResponseBase {IsSuccess = false};
            //try
            //{
            //    if (obj != null)
            //    {
            //        if (CheckElementFormularName(obj.Name, obj.Id))
            //        {

            //            var elementFormular = new T_ElementFormular();
            //            Parse.CopyObject(obj, ref elementFormular);
            //            elementFormular.CreatedDate = DateTime.Now.AddHours(14);
            //            _repElementFormular.Add(elementFormular);
            //            SaveChange();
            //            result.IsSuccess = true;
            //        }
            //        else
            //        {
            //            result.IsSuccess = false;
            //            result.Errors.Add(new Error() { MemberName = "Create ElementFormular", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
            //        }
            //    }
            //    else
            //    {
            //        result.IsSuccess = false;
            //        result.Errors.Add(new Error() { MemberName = "Create ElementFormular", Message = "Đối Tượng Không tồn tại" });
            //    }
            //}
            //catch (Exception)
            //{
            //    result.IsSuccess = false;
            //    result.Errors.Add(new Error() { MemberName = "Create ElementFormular", Message = "Đã có lỗi xảy ra" });
            //}
            return result;
        }

        public ResponseBase Update(ModelElementFormular obj)
        {

            ResponseBase result = new ResponseBase {IsSuccess = false};
            //if (!CheckElementFormularName(obj.Name, obj.Id))
            //{
            //    result.IsSuccess = false;
            //    result.Errors.Add(new Error() { MemberName = "UpdateElementFormular", Message = "Trùng Tên. Vui lòng chọn lại" });
            //}
            //else
            //{
                //T_ElementFormular elementFormular = _repElementFormular.Get(x => x.Id == obj.Id && !x.IsDeleted);
                //if (elementFormular != null)
                //{
                //    elementFormular.Name = obj.Name;
                //    elementFormular.Description = obj.Description;
                //    elementFormular.DefaultValue = obj.DefaultValue;
                //    elementFormular.UpdatedDate = DateTime.Now.AddHours(14);
                //    elementFormular.UpdatedUser = obj.UpdatedUser;
                //    _repElementFormular.Update(elementFormular);
                //    SaveChange();
                //    result.IsSuccess = true;
                //}
                //else
                //{
                //    result.IsSuccess = false;
                //    result.Errors.Add(new Error() { MemberName = "UpdateElementFormular", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                //}
           // }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            //var elementFormular = _repElementFormular.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            //if (elementFormular != null)
            //{
            //    elementFormular.IsDeleted = true;
            //    elementFormular.DeletedUser = userId;
            //    elementFormular.DeletedDate = DateTime.Now.AddHours(14);
            //    _repElementFormular.Update(elementFormular);
            //    SaveChange();
            //    responResult.IsSuccess = true;
            //}
            //else
            //{
            //    responResult.IsSuccess = false;
            //    responResult.Errors.Add(new Error() { MemberName = "Delete", Message = "Đối Tượng Đã Bị Xóa,Vui Lòng Kiểm Tra Lại" });
            //}
            return responResult;
        }
        public List<ModelSelectItem> GetListElementFormular()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            //listModelSelect.AddRange(_repElementFormular.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelElementFormular> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
           // if (string.IsNullOrEmpty(sorting))
           // {
           //     sorting = "CreatedDate DESC";
           // }
           // var elementFormulars = _repElementFormular.GetMany(c => !c.IsDeleted).Select(c => new ModelElementFormular()
           // {
           //     Id = c.Id,
           //     Name = c.Name,
           //     Description = c.Description,
           //     DefaultValue = c.DefaultValue,
           //     IsSystem = c.IsSystem,
           //     CreatedDate = c.CreatedDate,
           // }).OrderBy(sorting);
           // var pageNumber = (startIndexRecord / pageSize) + 1;
           //var result= new PagedList<ModelElementFormular>(elementFormulars, pageNumber, pageSize);
           // foreach (var ele in result)
           // {
           //     ele.strDefaultValue = $"{ele.DefaultValue:0,0}";
           // }
            return null;
        }
    }
}

