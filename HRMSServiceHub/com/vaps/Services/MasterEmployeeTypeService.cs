using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class MasterEmployeeTypeService : Interfaces.MasterEmployeeTypeInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterEmployeeTypeService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_EmployeeTypeDTO getBasicData(HR_Master_EmployeeTypeDTO dto)
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

        public HR_Master_EmployeeTypeDTO SaveUpdate(HR_Master_EmployeeTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_EmployeeType dmoObj = Mapper.Map<HR_Master_EmployeeType>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id == dto.MI_Id && t.HRMET_EmployeeType == dto.HRMET_EmployeeType).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMET_Id > 0)
                    {

                        var result = _HRMSContext.HR_Master_EmployeeType.Single(t => t.HRMET_Id == dmoObj.HRMET_Id);

                        dto.UpdatedDate = DateTime.Now;
                        Mapper.Map(dto, result);
                        _HRMSContext.Update(result);
                        var flag = _HRMSContext.SaveChanges();
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
                        dmoObj.HRMET_ActiveFlag = true;
                        dmoObj.HRMET_EmployeeType = dto.HRMET_EmployeeType;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
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
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
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

        public HR_Master_EmployeeTypeDTO editData(int id)
        {

            HR_Master_EmployeeTypeDTO dto = new HR_Master_EmployeeTypeDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_EmployeeType> lorg = new List<HR_Master_EmployeeType>();
                lorg = _HRMSContext.HR_Master_EmployeeType.AsNoTracking().Where(t => t.HRMET_Id.Equals(id)).ToList();
                dto.employeeTypeList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_EmployeeTypeDTO deactivate(HR_Master_EmployeeTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMET_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_EmployeeType.Single(t => t.HRMET_Id == dto.HRMET_Id);

                    if (result.HRMET_ActiveFlag == true)
                    {
                        result.HRMET_ActiveFlag = false;
                    }
                    else if (result.HRMET_ActiveFlag == false)
                    {
                        result.HRMET_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMET_ActiveFlag == true)
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

        public HR_Master_EmployeeTypeDTO GetAllDropdownAndDatatableDetails(HR_Master_EmployeeTypeDTO dto)
        {
            List<HR_Master_EmployeeType> datalist = new List<HR_Master_EmployeeType>();
            try
            {
                
                    datalist = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.employeeTypeList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
