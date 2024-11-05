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
    public class MasterMaritalStatusService : Interfaces.MasterMaritalStatusInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterMaritalStatusService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public IVRM_Master_Marital_StatusDTO getBasicData(IVRM_Master_Marital_StatusDTO dto)
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

        public IVRM_Master_Marital_StatusDTO SaveUpdate(IVRM_Master_Marital_StatusDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                IVRM_Master_Marital_Status dmoObj = Mapper.Map<IVRM_Master_Marital_Status>(dto);

                var duplicatecountresult = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMMS_MaritalStatus == dto.IVRMMMS_MaritalStatus).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.IVRMMMS_Id > 0)
                    {

                        var result = _HRMSContext.IVRM_Master_Marital_Status.Single(t => t.IVRMMMS_Id == dmoObj.IVRMMMS_Id);

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

                        dmoObj.IVRMMMS_ActiveFlag = true;
                        dmoObj.IVRMMMS_MaritalStatus = dto.IVRMMMS_MaritalStatus;
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

        public IVRM_Master_Marital_StatusDTO editData(int id)
        {

            IVRM_Master_Marital_StatusDTO dto = new IVRM_Master_Marital_StatusDTO();
            dto.retrunMsg = "";
            try
            {
                List<IVRM_Master_Marital_Status> lorg = new List<IVRM_Master_Marital_Status>();
                lorg = _HRMSContext.IVRM_Master_Marital_Status.AsNoTracking().Where(t => t.IVRMMMS_Id.Equals(id)).ToList();
                dto.maritalstatusList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public IVRM_Master_Marital_StatusDTO deactivate(IVRM_Master_Marital_StatusDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.IVRMMMS_Id > 0)
                {
                    var result = _HRMSContext.IVRM_Master_Marital_Status.Single(t => t.IVRMMMS_Id == dto.IVRMMMS_Id);

                    if (result.IVRMMMS_ActiveFlag == true)
                    {
                        result.IVRMMMS_ActiveFlag = false;
                    }
                    else if (result.IVRMMMS_ActiveFlag == false)
                    {
                        result.IVRMMMS_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.IVRMMMS_ActiveFlag == true)
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
        public IVRM_Master_Marital_StatusDTO GetAllDropdownAndDatatableDetails(IVRM_Master_Marital_StatusDTO dto)
        {
            List<IVRM_Master_Marital_Status> datalist = new List<IVRM_Master_Marital_Status>();

            try
            {
                
                    datalist = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.maritalstatusList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
