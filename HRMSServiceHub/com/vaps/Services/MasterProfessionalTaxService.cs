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
    public class MasterProfessionalTaxService : Interfaces.MasterProfessionalTaxInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterProfessionalTaxService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_ProfessionalTaxDTO getBasicData(HR_Master_ProfessionalTaxDTO dto)
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

        public HR_Master_ProfessionalTaxDTO SaveUpdate(HR_Master_ProfessionalTaxDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_ProfessionalTaxDMO dmoObj = Mapper.Map<HR_Master_ProfessionalTaxDMO>(dto);

                var duplicatecountresult = _HRMSContext.HR_MasterProfessionalTax.Where(t => t.MI_Id == dto.MI_Id && t.HRMPT_SalaryFrom == dto.HRMPT_SalaryFrom && t.HRMPT_SalaryTo == dto.HRMPT_SalaryTo && t.HRMPT_PTax == dto.HRMPT_PTax).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMPT_Id > 0)
                    {

                        var result = _HRMSContext.HR_MasterProfessionalTax.Single(t => t.HRMPT_Id == dmoObj.HRMPT_Id);

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

                        dmoObj.HRMPT_ActiveFlag = true;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.HRMPT_Id = dmoObj.HRMPT_Id;
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

        public HR_Master_ProfessionalTaxDTO editData(int id)
        {

            HR_Master_ProfessionalTaxDTO dto = new HR_Master_ProfessionalTaxDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_ProfessionalTaxDMO> lorg = new List<HR_Master_ProfessionalTaxDMO>();
                lorg = _HRMSContext.HR_MasterProfessionalTax.AsNoTracking().Where(t => t.HRMPT_Id.Equals(id)).ToList();
                dto.pTaxrList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_ProfessionalTaxDTO deactivate(HR_Master_ProfessionalTaxDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMPT_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterProfessionalTax.Single(t => t.HRMPT_Id == dto.HRMPT_Id);

                    if (result.HRMPT_ActiveFlag == true)
                    {
                        result.HRMPT_ActiveFlag = false;
                    }
                    else if (result.HRMPT_ActiveFlag == false)
                    {
                        result.HRMPT_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMPT_ActiveFlag == true)
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

        public HR_Master_ProfessionalTaxDTO GetAllDropdownAndDatatableDetails(HR_Master_ProfessionalTaxDTO dto)
        {
            List<HR_Master_ProfessionalTaxDMO> datalist = new List<HR_Master_ProfessionalTaxDMO>();
            try
            {
                
                    datalist = _HRMSContext.HR_MasterProfessionalTax.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.pTaxrList = datalist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

    }
}
