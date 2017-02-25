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
    public class BllMachine : IBllMachine
    {
        private readonly IT_MachineRepository _repMachine;
        private readonly IUnitOfWork<VINASICEntities> _unitOfWork;
        public BllMachine(IUnitOfWork<VINASICEntities> unitOfWork, IT_MachineRepository repMachine)
        {
            _unitOfWork = unitOfWork;
            _repMachine = repMachine;
        }
        private void SaveChange()
        {
            _unitOfWork.Commit();
        }

        private bool CheckMachineName(string machineName, int id)
        {
            var checkResult = false;
            var checkName = _repMachine.GetMany(c => !c.IsDeleted && c.Id != id && c.Name.Trim().ToUpper().Equals(machineName.Trim().ToUpper())).FirstOrDefault();
            if (checkName == null)
                checkResult = true;
            return checkResult;
        }
        public List<ModelMachine> GetListProduct()
        {
            var machine = _repMachine.GetMany(c => !c.IsDeleted).Select(c => new ModelMachine()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
            }).ToList();
            return machine;
        }

        public ResponseBase Create(ModelMachine obj)
        {
            ResponseBase result = new ResponseBase { IsSuccess = false };
            try
            {
                if (obj != null)
                {
                    if (CheckMachineName(obj.Name, obj.Id))
                    {

                        var machine = new T_Machine();
                        Parse.CopyObject(obj, ref machine);
                        machine.CreatedDate = DateTime.Now.AddHours(14);
                        _repMachine.Add(machine);
                        SaveChange();
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Errors.Add(new Error() { MemberName = "Create Machine", Message = "Tên Đã Tồn Tại,Vui Lòng Chọn Tên Khác" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "Create Machine", Message = "Đối Tượng Không tồn tại" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "Create Machine", Message = "Đã có lỗi xảy ra" });
            }
            return result;
        }

        public ResponseBase Update(ModelMachine obj)
        {

            ResponseBase result = new ResponseBase { IsSuccess = false };
            if (!CheckMachineName(obj.Name, obj.Id))
            {
                result.IsSuccess = false;
                result.Errors.Add(new Error() { MemberName = "UpdateMachine", Message = "Trùng Tên. Vui lòng chọn lại" });
            }
            else
            {
                T_Machine machine = _repMachine.Get(x => x.Id == obj.Id && !x.IsDeleted);
                if (machine != null)
                {
                    machine.Code = obj.Code;
                    machine.Name = obj.Name;
                    machine.Description = obj.Description;
                    machine.Tempo = obj.Tempo;
                    machine.UpdatedDate = DateTime.Now.AddHours(14);
                    machine.UpdatedUser = obj.UpdatedUser;
                    _repMachine.Update(machine);
                    SaveChange();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Errors.Add(new Error() { MemberName = "UpdateMachine", Message = "Thông tin nhập không đúng Vui lòng kiểm tra lại!" });
                }
            }
            return result;
        }
        public ResponseBase DeleteById(int id, int userId)
        {
            var responResult = new ResponseBase();
            var machine = _repMachine.GetMany(c => !c.IsDeleted && c.Id == id).FirstOrDefault();
            if (machine != null)
            {
                machine.IsDeleted = true;
                machine.DeletedUser = userId;
                machine.DeletedDate = DateTime.Now.AddHours(14);
                _repMachine.Update(machine);
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
        public List<ModelSelectItem> GetListMachine()
        {
            List<ModelSelectItem> listModelSelect = new List<ModelSelectItem>
            {
                new ModelSelectItem() {Value = 0, Name = "---Loại Dịch Vụ----"}
            };
            listModelSelect.AddRange(_repMachine.GetMany(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }));
            return listModelSelect;
        }
        public PagedList<ModelMachine> GetList(string keyWord, int startIndexRecord, int pageSize, string sorting)
        {
            if (string.IsNullOrEmpty(sorting))
            {
                sorting = "CreatedDate DESC";
            }
            var machines = _repMachine.GetMany(c => !c.IsDeleted).Select(c => new ModelMachine()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                Tempo = c.Tempo,
                CreatedDate = c.CreatedDate,
            }).OrderBy(sorting);
            var pageNumber = (startIndexRecord / pageSize) + 1;
            return new PagedList<ModelMachine>(machines, pageNumber, pageSize);
        }
    }
}

