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
    public class MasterIncomeTaxDetailsCessService : Interfaces.MasterIncomeTaxDetailsCessInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterIncomeTaxDetailsCessService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_IncomeTax_Details_CessDTO getBasicData(HR_Master_IncomeTax_Details_CessDTO dto)
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

        public HR_Master_IncomeTax_Details_CessDTO SaveUpdate(HR_Master_IncomeTax_Details_CessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_IncomeTax_Details_CessDMO dmoObj = Mapper.Map<HR_Master_IncomeTax_Details_CessDMO>(dto);

                if (dmoObj.HRMITDC_Id > 0)
                {
                    var duplicateresult = _HRMSContext.HR_MasterIncomeTaxDetailsCess.Where(t => t.HRMITD_Id == dto.HRMITD_Id && t.HRMITC_Id == dto.HRMITC_Id).Count();
                    if (duplicateresult > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                    else
                    {
                        var result = _HRMSContext.HR_MasterIncomeTaxDetailsCess.Single(t => t.HRMITDC_Id == dmoObj.HRMITDC_Id);

                        result.UpdatedDate = DateTime.Now;
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
                }
                else
                {
                    var duplicatecountresult = _HRMSContext.HR_MasterIncomeTaxDetailsCess.Where(t => t.HRMITD_Id == dto.HRMITD_Id && t.HRMITC_Id == dto.HRMITC_Id && t.HRMITDC_Amount == dto.HRMITDC_Amount).Count();
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRMITDC_ActiveFlag = true;
                        dmoObj.HRMITD_Id = dto.HRMITD_Id;
                        dmoObj.HRMITC_Id = dto.HRMITC_Id;
                        dmoObj.HRMITDC_Amount = dto.HRMITDC_Amount;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.HRMITDC_Id = dmoObj.HRMITDC_Id;
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
        public HR_Master_IncomeTax_Details_CessDTO editData(int id)
        {

            HR_Master_IncomeTax_Details_CessDTO dto = new HR_Master_IncomeTax_Details_CessDTO();
            dto.retrunMsg = "";
            try
            {
                //List<HR_Master_IncomeTax_Details_CessDMO> lorg = new List<HR_Master_IncomeTax_Details_CessDMO>();
                //lorg = _HRMSContext.HR_MasterIncomeTaxDetailsCess.AsNoTracking().Where(t => t.HRMITDC_Id.Equals(id)).ToList();
                //dto.incomeTaxDetailsCessList = lorg.ToArray();

                dto.incomeTaxDetailsCessList = (from mitd in _HRMSContext.HR_MasterIncomeTaxDetailsCess
                                                from mit in _HRMSContext.HR_Master_IncomeTaxCess
                                                from abc in _HRMSContext.HR_MasterIncomeTaxDetails
                                                from xyz in _HRMSContext.HR_MasterIncomeTax
                                                where mitd.HRMITC_Id == mit.HRMITC_Id
                                                && abc.HRMITD_Id == mitd.HRMITD_Id
                                                && xyz.HRMIT_Id == abc.HRMIT_Id && mitd.HRMITDC_Id.Equals(id)
                                                select new HR_Master_IncomeTax_Details_CessDTO
                                                {
                                                    HRMITC_Id = mitd.HRMITC_Id,
                                                    HRMIT_AgeFlag = xyz.HRMIT_AgeFlag,
                                                    HRMITD_Id = abc.HRMITD_Id,
                                                    HRMITDC_Id=mitd.HRMITDC_Id,
                                                    HRMIT_Id = xyz.HRMIT_Id,
                                                    HRMITDC_ActiveFlag = mitd.HRMITDC_ActiveFlag,
                                                    HRMITDC_Amount = mitd.HRMITDC_Amount,
                                                    HRMITC_CessName = mit.HRMITC_CessName
                                                }).OrderBy(mitd => mitd.CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Master_IncomeTax_Details_CessDTO deactivate(HR_Master_IncomeTax_Details_CessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMITDC_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterIncomeTaxDetailsCess.Single(t => t.HRMITDC_Id == dto.HRMITDC_Id);

                    if (result.HRMITDC_ActiveFlag == true)
                    {
                        result.HRMITDC_ActiveFlag = false;
                    }
                    else if (result.HRMITDC_ActiveFlag == false)
                    {
                        result.HRMITDC_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMITDC_ActiveFlag == true)
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

        public HR_Master_IncomeTax_Details_CessDTO GetAllDropdownAndDatatableDetails(HR_Master_IncomeTax_Details_CessDTO dto)
        {
            List<HR_Master_IncomeTax_CessDMO> leavelist = new List<HR_Master_IncomeTax_CessDMO>();
            List<HR_Master_IncomeTaxDMO> cessname = new List<HR_Master_IncomeTaxDMO>();
            try
            {
               
                    //LeavePolicy List

                    leavelist = _HRMSContext.HR_Master_IncomeTaxCess.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.incomeTaxList = leavelist.ToArray();

                    //Leaveyear
                    cessname = _HRMSContext.HR_MasterIncomeTax.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMIT_ActiveFlag == true).ToList();
                    dto.cessnamedropdown = cessname.ToArray();



                dto.incomeTaxDetailsCessList = (from mitd in _HRMSContext.HR_MasterIncomeTaxDetailsCess
                                     from mit in _HRMSContext.HR_Master_IncomeTaxCess
                                     from abc in _HRMSContext.HR_MasterIncomeTaxDetails
                                     from xyz in _HRMSContext.HR_MasterIncomeTax
                                     where mitd.HRMITC_Id == mit.HRMITC_Id
                                     && abc.HRMITD_Id==mitd.HRMITD_Id
                                     && xyz.HRMIT_Id==abc.HRMIT_Id
                                     select new HR_Master_IncomeTax_Details_CessDTO
                                     {
                                         HRMITC_Id = mitd.HRMITC_Id,
                                         HRMIT_AgeFlag = xyz.HRMIT_AgeFlag,
                                         HRMITD_Id = abc.HRMITD_Id,
                                         HRMIT_Id = xyz.HRMIT_Id,
                                         HRMITDC_Id = mitd.HRMITDC_Id,
                                         HRMITDC_ActiveFlag = mitd.HRMITDC_ActiveFlag,
                                         HRMITDC_Amount = mitd.HRMITDC_Amount,
                                         HRMITC_CessName = mit.HRMITC_CessName
                                     }).OrderBy(mitd => mitd.CreatedDate).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
