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
    public class masterPositionService : Interfaces.masterPositionInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public masterPositionService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }

        public HR_Master_PositionDTO getBasicData(HR_Master_PositionDTO dto)
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

        public HR_Master_PositionDTO SaveUpdate(HR_Master_PositionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_PositionDMO dmoObj = Mapper.Map<HR_Master_PositionDMO>(dto);
                var duplicatecountresult = _VMSContext.HR_Master_PositionDMO.Where(t => t.HRMP_Position == dto.HRMP_Position && t.HRMP_Skills == dto.HRMP_Skills && t.MI_Id == dto.MI_Id && t.HRMP_Desc == dto.HRMP_Desc).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMP_Id > 0)
                    {
                        var result = _VMSContext.HR_Master_PositionDMO.Single(t => t.HRMP_Id == dmoObj.HRMP_Id);
                        dmoObj.HRMP_Position = dto.HRMP_Position;
                        dmoObj.HRMP_Skills = dto.HRMP_Skills;
                        dmoObj.HRMP_Desc = dto.HRMP_Desc;
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
                        dmoObj.HRMP_ActiveFlg = true;
                        dmoObj.HRMP_Position = dto.HRMP_Position;
                        dmoObj.HRMP_Skills = dto.HRMP_Skills;
                        dmoObj.HRMP_Desc = dto.HRMP_Desc;
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

        public HR_Master_PositionDTO editData(int id)
        {
            HR_Master_PositionDTO dto = new HR_Master_PositionDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_PositionDMO> lorg = new List<HR_Master_PositionDMO>();
                lorg = _VMSContext.HR_Master_PositionDMO.AsNoTracking().Where(t => t.HRMP_Id.Equals(id)).ToList();
                dto.PositionList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_PositionDTO deactivate(HR_Master_PositionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMP_Id > 0)
                {
                    var result = _VMSContext.HR_Master_PositionDMO.Single(t => t.HRMP_Id == dto.HRMP_Id);

                    if (result.HRMP_ActiveFlg == true)
                    {
                        result.HRMP_ActiveFlg = false;
                    }
                    else if (result.HRMP_ActiveFlg == false)
                    {
                        result.HRMP_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMP_ActiveFlg == true)
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

        public HR_Master_PositionDTO GetAllDropdownAndDatatableDetails(HR_Master_PositionDTO dto)
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
        public HR_Master_PositionDTO getdata(HR_Master_PositionDTO dto)
        {
            List<HR_Master_PositionDMO> datalist = new List<HR_Master_PositionDMO>();
            try
            {
                datalist = _VMSContext.HR_Master_PositionDMO.Where(t => t.MI_Id == dto.MI_Id).ToList();
                dto.PositionList = datalist.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
    }
}
