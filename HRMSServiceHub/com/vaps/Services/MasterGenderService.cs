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
    public class MasterGenderService : Interfaces.MasterGenderInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterGenderService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public IVRM_Master_GenderDTO getBasicData(IVRM_Master_GenderDTO dto)
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

        public IVRM_Master_GenderDTO SaveUpdate(IVRM_Master_GenderDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                IVRM_Master_Gender dmoObj = Mapper.Map<IVRM_Master_Gender>(dto);


                var duplicatecountresult = _HRMSContext.IVRM_Master_Gender.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMG_GenderName == dto.IVRMMG_GenderName).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.IVRMMG_Id > 0)
                    {

                        var result = _HRMSContext.IVRM_Master_Gender.Single(t => t.IVRMMG_Id == dmoObj.IVRMMG_Id);

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
                            dmoObj.IVRMMG_ActiveFlag = true;
                            dmoObj.IVRMMG_GenderName = dto.IVRMMG_GenderName;
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

        public IVRM_Master_GenderDTO editData(int id)
        {

            IVRM_Master_GenderDTO dto = new IVRM_Master_GenderDTO();
            dto.retrunMsg = "";
            try
            {
                List<IVRM_Master_Gender> lorg = new List<IVRM_Master_Gender>();
                lorg = _HRMSContext.IVRM_Master_Gender.AsNoTracking().Where(t => t.IVRMMG_Id.Equals(id)).ToList();
                dto.genderList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public IVRM_Master_GenderDTO deactivate(IVRM_Master_GenderDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.IVRMMG_Id > 0)
                {
                    var result = _HRMSContext.IVRM_Master_Gender.Single(t => t.IVRMMG_Id == dto.IVRMMG_Id);

                    if (result.IVRMMG_ActiveFlag == true)
                    {
                        result.IVRMMG_ActiveFlag = false;
                    }
                    else if (result.IVRMMG_ActiveFlag == false)
                    {
                        result.IVRMMG_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.IVRMMG_ActiveFlag == true)
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

        public IVRM_Master_GenderDTO GetAllDropdownAndDatatableDetails(IVRM_Master_GenderDTO dto)
        {
            List<IVRM_Master_Gender> datalist = new List<IVRM_Master_Gender>();
            try
            {
                
                    datalist = _HRMSContext.IVRM_Master_Gender.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.genderList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
