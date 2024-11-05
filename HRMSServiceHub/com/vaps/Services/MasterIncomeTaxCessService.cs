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
    public class MasterIncomeTaxCessService : Interfaces.MasterIncomeTaxCessInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterIncomeTaxCessService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }
        public HR_Master_IncomeTax_CessDTO getBasicData(HR_Master_IncomeTax_CessDTO dto)
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
        public HR_Master_IncomeTax_CessDTO SaveUpdate(HR_Master_IncomeTax_CessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_IncomeTax_CessDMO dmoObj = Mapper.Map<HR_Master_IncomeTax_CessDMO>(dto);

                if (dmoObj.HRMITC_Id > 0)
                {

                    var result = _HRMSContext.HR_Master_IncomeTaxCess.Single(t => t.HRMITC_Id == dmoObj.HRMITC_Id);

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
                    var duplicatecountresult = _HRMSContext.HR_Master_IncomeTaxCess.Where(t => t.MI_Id == dto.MI_Id && t.HRMITC_CessName == dto.HRMITC_CessName).Count();
                    if (duplicatecountresult == 0)
                    {

                        dmoObj.HRMITC_ActiveFlag = true;
                        dmoObj.HRMITC_CessName = dto.HRMITC_CessName;
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
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

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

        public HR_Master_IncomeTax_CessDTO editData(int id)
        {

            HR_Master_IncomeTax_CessDTO dto = new HR_Master_IncomeTax_CessDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_IncomeTax_CessDMO> lorg = new List<HR_Master_IncomeTax_CessDMO>();
                lorg = _HRMSContext.HR_Master_IncomeTaxCess.AsNoTracking().Where(t => t.HRMITC_Id.Equals(id)).ToList();
                dto.incometax_cessList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_IncomeTax_CessDTO deactivate(HR_Master_IncomeTax_CessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMITC_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_IncomeTaxCess.Single(t => t.HRMITC_Id == dto.HRMITC_Id);

                    if (result.HRMITC_ActiveFlag == true)
                    {
                        result.HRMITC_ActiveFlag = false;
                    }
                    else if (result.HRMITC_ActiveFlag == false)
                    {
                        result.HRMITC_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMITC_ActiveFlag == true)
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

        public HR_Master_IncomeTax_CessDTO GetAllDropdownAndDatatableDetails(HR_Master_IncomeTax_CessDTO dto)
        {
            List<HR_Master_IncomeTax_CessDMO> datalist = new List<HR_Master_IncomeTax_CessDMO>();
            try
            {
                
                    datalist = _HRMSContext.HR_Master_IncomeTaxCess.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.incometax_cessList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
