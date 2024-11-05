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
    public class masterPriorityService : Interfaces.masterPriorityInterface
    {

        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public masterPriorityService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }

        public HR_Master_PriorityDTO getBasicData(HR_Master_PriorityDTO dto)
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

        public HR_Master_PriorityDTO SaveUpdate(HR_Master_PriorityDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_PriorityDMO dmoObj = Mapper.Map<HR_Master_PriorityDMO>(dto);
                var duplicatecountresult = _VMSContext.HR_Master_PriorityDMO.Where(t => t.HRMP_Name == dto.HRMP_Name && t.MI_Id == dto.MI_Id && t.HRMP_Order == dto.HRMP_Order).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMPR_Id > 0)
                    {
                        var result = _VMSContext.HR_Master_PriorityDMO.Single(t => t.HRMPR_Id == dmoObj.HRMPR_Id);
                        dmoObj.HRMP_Name = dto.HRMP_Name;
                        dmoObj.HRMP_Order = dto.HRMP_Order;
                        dmoObj.HRMP_UpdatedBy = dto.HRMP_UpdatedBy;
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
                        dmoObj.HRMP_ActiveFlag = true;
                        dmoObj.HRMP_Name = dto.HRMP_Name;
                        dmoObj.HRMP_Order = dto.HRMP_Order;
                        dmoObj.HRMP_CreatedBy = dto.HRMP_UpdatedBy;
                        dmoObj.HRMP_UpdatedBy = dto.HRMP_UpdatedBy;
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

        public HR_Master_PriorityDTO editData(int id)
        {

            HR_Master_PriorityDTO dto = new HR_Master_PriorityDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_PriorityDMO> lorg = new List<HR_Master_PriorityDMO>();
                lorg = _VMSContext.HR_Master_PriorityDMO.AsNoTracking().Where(t => t.HRMPR_Id.Equals(id)).ToList();
                dto.PriorityList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_PriorityDTO deactivate(HR_Master_PriorityDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMPR_Id > 0)
                {
                    var result = _VMSContext.HR_Master_PriorityDMO.Single(t => t.HRMPR_Id == dto.HRMPR_Id);

                    if (result.HRMP_ActiveFlag == true)
                    {
                        result.HRMP_ActiveFlag = false;
                    }
                    else if (result.HRMP_ActiveFlag == false)
                    {
                        result.HRMP_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMP_ActiveFlag == true)
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

        public HR_Master_PriorityDTO GetAllDropdownAndDatatableDetails(HR_Master_PriorityDTO dto)
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
        public HR_Master_PriorityDTO getdata(HR_Master_PriorityDTO dto)
        {

            List<HR_Master_PriorityDMO> datalist = new List<HR_Master_PriorityDMO>();
            try
            {
                datalist = _VMSContext.HR_Master_PriorityDMO.Where(t => t.MI_Id == dto.MI_Id).ToList();
                dto.PriorityList = datalist.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
    }
}
