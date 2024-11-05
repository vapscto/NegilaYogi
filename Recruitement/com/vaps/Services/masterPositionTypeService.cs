using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Recruitment.com.vaps.Services
{
    public class masterPositionTypeService : Interfaces.masterPositionTypeInterface
    {

        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public masterPositionTypeService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }

        public HR_Master_PostionTypeDTO getBasicData(HR_Master_PostionTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }



            return dto;
        }

        public HR_Master_PostionTypeDTO SaveUpdate(HR_Master_PostionTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_PostionTypeDMO dmoObj = Mapper.Map<HR_Master_PostionTypeDMO>(dto);
                var duplicatecountresult = _VMSContext.HR_Master_PostionTypeDMO.Where(t => t.HRMPT_Name == dto.HRMPT_Name && t.MI_Id == dto.MI_Id && t.HRMPT_Desc == dto.HRMPT_Desc).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMPT_Id > 0)
                    {
                        var result = _VMSContext.HR_Master_PostionTypeDMO.Single(t => t.HRMPT_Id == dmoObj.HRMPT_Id);
                        dmoObj.HRMPT_Name = dto.HRMPT_Name;
                        dmoObj.HRMPT_Desc = dto.HRMPT_Desc;
                        dmoObj.HRMPT_UpdatedBy = dto.HRMPT_UpdatedBy;
                        dto.UpdatedDate = DateTime.Now;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMPT_ActiveFlg = true;
                        dmoObj.HRMPT_Name = dto.HRMPT_Name;
                        dmoObj.HRMPT_Desc = dto.HRMPT_Desc;
                        dmoObj.HRMPT_CreatedBy = dto.HRMPT_UpdatedBy;
                        dmoObj.HRMPT_UpdatedBy = dto.HRMPT_UpdatedBy;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }

                }
                if (duplicatecountresult > 0)
                {
                    dto.retrunMsg = "Duplicate";
                }

                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_PostionTypeDTO editData(int id)
        {

            HR_Master_PostionTypeDTO dto = new HR_Master_PostionTypeDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_PostionTypeDMO> lorg = new List<HR_Master_PostionTypeDMO>();
                lorg = _VMSContext.HR_Master_PostionTypeDMO.AsNoTracking().Where(t => t.HRMPT_Id.Equals(id)).ToList();
                dto.PositionTypeList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_PostionTypeDTO deactivate(HR_Master_PostionTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMPT_Id > 0)
                {
                    var result = _VMSContext.HR_Master_PostionTypeDMO.Single(t => t.HRMPT_Id == dto.HRMPT_Id);

                    if (result.HRMPT_ActiveFlg == true)
                    {
                        result.HRMPT_ActiveFlg = false;
                    }
                    else if (result.HRMPT_ActiveFlg == false)
                    {
                        result.HRMPT_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMPT_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_PostionTypeDTO GetAllDropdownAndDatatableDetails(HR_Master_PostionTypeDTO dto)
        {

            try
            {
                dto.institutionlist = _VMSContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public HR_Master_PostionTypeDTO getdata(HR_Master_PostionTypeDTO dto)
        {
            List<HR_Master_PostionTypeDMO> datalist = new List<HR_Master_PostionTypeDMO>();
            try
            {
                datalist = _VMSContext.HR_Master_PostionTypeDMO.Where(t => t.MI_Id == dto.MI_Id).ToList();
                dto.PositionTypeList = datalist.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }

    }
}
