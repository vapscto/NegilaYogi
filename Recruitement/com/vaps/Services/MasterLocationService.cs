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
    public class MasterLocationService : Interfaces.MasterLocationInterface
    {

        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterLocationService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }

        public HR_Master_LocationDTO getBasicData(HR_Master_LocationDTO dto)
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

        public HR_Master_LocationDTO SaveUpdate(HR_Master_LocationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_LocationDMO dmoObj = Mapper.Map<HR_Master_LocationDMO>(dto);
                var duplicatecountresult = _VMSContext.HR_Master_LocationDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMLO_LocationName == dto.HRMLO_LocationName && t.HRMLO_LocationDesc == dto.HRMLO_LocationDesc).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMLO_Id > 0)
                    {
                        var result = _VMSContext.HR_Master_LocationDMO.Single(t => t.HRMLO_Id == dmoObj.HRMLO_Id);
                        dmoObj.HRMLO_LocationName = dto.HRMLO_LocationName;
                        dmoObj.HRMLO_LocationDesc = dto.HRMLO_LocationDesc;
                        dmoObj.HRMLO_UpdatedBy = dto.HRMLO_UpdatedBy;
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
                        dmoObj.HRMLO_ActiveFlg = true;
                        dmoObj.HRMLO_LocationName = dto.HRMLO_LocationName;
                        dmoObj.HRMLO_LocationDesc = dto.HRMLO_LocationDesc;
                        dmoObj.HRMLO_CreatedBy = dto.HRMLO_UpdatedBy;
                        dmoObj.HRMLO_UpdatedBy = dto.HRMLO_UpdatedBy;
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

        public HR_Master_LocationDTO editData(int id)
        {

            HR_Master_LocationDTO dto = new HR_Master_LocationDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_LocationDMO> lorg = new List<HR_Master_LocationDMO>();
                lorg = _VMSContext.HR_Master_LocationDMO.AsNoTracking().Where(t => t.HRMLO_Id.Equals(id)).ToList();
                dto.locationList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_LocationDTO deactivate(HR_Master_LocationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMLO_Id > 0)
                {
                    var result = _VMSContext.HR_Master_LocationDMO.Single(t => t.HRMLO_Id == dto.HRMLO_Id);

                    if (result.HRMLO_ActiveFlg == true)
                    {
                        result.HRMLO_ActiveFlg = false;
                    }
                    else if (result.HRMLO_ActiveFlg == false)
                    {
                        result.HRMLO_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMLO_ActiveFlg == true)
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

       
        public HR_Master_LocationDTO GetAllDropdownAndDatatableDetails(HR_Master_LocationDTO dto)
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

        public HR_Master_LocationDTO getdata(HR_Master_LocationDTO dto)
        {
            try
            {
                List<HR_Master_LocationDMO> datalist = new List<HR_Master_LocationDMO>();
                datalist = _VMSContext.HR_Master_LocationDMO.Where(t => t.MI_Id == dto.MI_Id).ToList();
                dto.locationList = datalist.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
    }
}
