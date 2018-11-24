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
    public class BllContent : IBllContent
    {
        private readonly IT_ContentRepository _repContent;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllContent(IUnitOfWork<VINASICEntities> unitOfWork, IT_ContentRepository repContent)
        {
            _unitOfWork = unitOfWork;
            _repContent = repContent;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckContentName(string contentName, int id)
        {
            var checkResult = false;
            var checkName = _repContent.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(contentName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelContent> GetListProduct()
        {
            var content = _repContent.GetMany(c => !c.IsDeleted).Select(c => new ModelContent()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return content;
        }

        public ModelContent GetContentByType(int code)
        {
            var content = _repContent.GetMany(c => c.Type == code).Select(c => new ModelContent()
            {
                Id = c.Id,
                Name = c.Name,
                Content = c.Content,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).FirstOrDefault();
            return content;
        }

        public ResponseBase Create(ModelContent obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckContentName(obj.Name, obj.Id))
                    {

                        var content = new T_Content();
                        Parse.CopyObject(obj, ref content);
                        content.CreatedDate = DateTime.Now.AddHours(14);
                        _repContent.Add(content);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Content", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Content", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Content", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelContent obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };

            T_Content content = _repContent.Get(x => x.Type == obj.Type && !x.IsDeleted);
            if (content != null)
            {
                content.Name = obj.Name;
                content.Content = obj.Content;
                content.Description = obj.Description;
                content.UpdatedDate = DateTime.Now.AddHours(14);
                content.UpdatedUser = obj.UpdatedUser;
                _repContent.Update(content);
                SaveChange();
                result.IsSuccess = true;
            }
            else
            {
                var contentInsert = new T_Content();
                Parse.CopyObject(obj, ref contentInsert);
                contentInsert.CreatedDate = DateTime.Now.AddHours(14);
                _repContent.Add(contentInsert);
                SaveChange();
                result.IsSuccess = true;
            }

            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var content = _repContent.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (content != null)
            {
                content.IsDeleted = true;
                content.DeletedUser = userId;
                content.DeletedDate = DateTime.Now.AddHours(14);
                _repContent.Update(content);
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
        public List<ModelSelectItem> GetListContent()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repContent.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelContent> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var contents = _repContent.GetMany(c => !c.IsDeleted).Select(c => new ModelContent()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelContent>(contents, pageNumber, pageSize);
        }
    }
}

