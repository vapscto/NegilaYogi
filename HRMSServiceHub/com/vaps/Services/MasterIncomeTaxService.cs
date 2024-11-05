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
    public class MasterIncomeTaxService : Interfaces.MasterIncomeTaxInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterIncomeTaxService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_IncomeTaxDTO getBasicData(HR_Master_IncomeTaxDTO dto)
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

        public HR_Master_IncomeTaxDTO SaveUpdate(HR_Master_IncomeTaxDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_IncomeTaxDMO dmoObj = Mapper.Map<HR_Master_IncomeTaxDMO>(dto);
                HR_Master_IncomeTax_DetailsDMO dmoObjss = Mapper.Map<HR_Master_IncomeTax_DetailsDMO>(dto);

                if (dmoObj.HRMIT_Id > 0)
                {

                    var result = _HRMSContext.HR_MasterIncomeTax.Single(t => t.HRMIT_Id == dmoObj.HRMIT_Id);

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
                    ///
                    //var resultss = _HRMSContext.HR_MasterIncomeTaxDetails.Single(t => t.HRMIT_Id == dmoObjss.HRMIT_Id);

                    //dto.UpdatedDate = DateTime.Now;
                    //Mapper.Map(dto, result);
                    //_HRMSContext.Update(result);
                    //var flagss = _HRMSContext.SaveChanges();
                    //if (flagss > 0)
                    //{
                    //    dto.retrunMsg = "Update";
                    //}
                    //else
                    //{
                    //    dto.retrunMsg = "false";
                    //}



                }
                else
                {
                    var duplicatecountresult = _HRMSContext.HR_MasterIncomeTax.Where(t => t.MI_Id == dto.MI_Id && t.IMFY_Id == dto.IMFY_Id && t.HRMIT_GenderFlag == dto.HRMIT_GenderFlag && t.HRMIT_AgeFlag == dto.HRMIT_AgeFlag).Count();
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRMIT_ActiveFlag = true;
                        dmoObj.IMFY_Id = dto.IMFY_Id;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMIT_GenderFlag = dto.HRMIT_GenderFlag;
                        dmoObj.HRMIT_AgeFlag = dto.HRMIT_AgeFlag;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.HRMIT_Id = dmoObj.HRMIT_Id;
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                      //  HR_Master_IncomeTax_DetailsDMO dmoObjss = Mapper.Map<HR_Master_IncomeTax_DetailsDMO>(dto);

                        //var duplicatecountresultss = _HRMSContext.HR_MasterIncomeTaxDetails.Where(t => t.HRMIT_Id == dto.HRMIT_Id && t.HRMITD_AmountFrom == dto.HRMITD_AmountFrom && t.HRMITD_AmountTo == dto.HRMITD_AmountTo).Count();
                        //if (duplicatecountresultss == 0)
                        //{
                        //    dmoObjss.HRMITD_ActiveFlag = true;
                        //    dmoObjss.HRMIT_Id = dto.HRMIT_Id;
                        //    dmoObjss.HRMITD_AmountFrom = dto.incTaxDetail[0].HRMITD_AmountFrom;
                        //    dmoObjss.HRMITD_AmountTo = dto.incTaxDetail[0].HRMITD_AmountTo;
                        //    dmoObjss.HRMITD_IncomeTax = dto.incTaxDetail[0].HRMITD_IncomeTax;
                        //    dmoObjss.UpdatedDate = DateTime.Now;
                        //    dmoObjss.CreatedDate = DateTime.Now;
                        //    _HRMSContext.Add(dmoObjss);
                        //    var flagss = _HRMSContext.SaveChanges();
                        //    if (flagss == 1)
                        //    {
                        //        dto.HRMITD_Id = dmoObjss.HRMITD_Id;
                        //        dto.retrunMsg = "Add";
                        //    }
                        //    else
                        //    {
                        //        dto.retrunMsg = "false";
                        //    }
                        //}



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

        public HR_Master_IncomeTaxDTO addUpdateIncomeTaxDetails(HR_Master_IncomeTaxDTO dto)
        {
            foreach (var item in dto.incTaxDetail)
            {

            }


            return dto;
        }

        public HR_Master_IncomeTaxDTO editData(int id)
        {

            HR_Master_IncomeTaxDTO dto = new HR_Master_IncomeTaxDTO();
            HR_Master_IncomeTax_DetailsDTO dtos = new HR_Master_IncomeTax_DetailsDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_IncomeTaxDMO> lorg = new List<HR_Master_IncomeTaxDMO>();
                lorg = _HRMSContext.HR_MasterIncomeTax.AsNoTracking().Where(t => t.HRMIT_Id.Equals(id)).ToList();
                dto.incomeTaxList = lorg.ToArray();

                List<HR_Master_IncomeTax_DetailsDMO> log = new List<HR_Master_IncomeTax_DetailsDMO>();
                log = _HRMSContext.HR_MasterIncomeTaxDetails.AsNoTracking().Where(t => t.HRMITD_Id.Equals(id)).ToList();
                dto.incomeTaxDetailsList = log.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_IncomeTaxDTO deactivate(HR_Master_IncomeTaxDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMIT_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterIncomeTax.Single(t => t.HRMIT_Id == dto.HRMIT_Id);

                    if (result.HRMIT_ActiveFlag == true)
                    {
                        result.HRMIT_ActiveFlag = false;
                    }
                    else if (result.HRMIT_ActiveFlag == false)
                    {
                        result.HRMIT_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMIT_ActiveFlag == true)
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

        public HR_Master_IncomeTaxDTO GetAllDropdownAndDatatableDetails(HR_Master_IncomeTaxDTO dto)
        {
            List<HR_Master_IncomeTax_CessDMO> incomecesslist = new List<HR_Master_IncomeTax_CessDMO>();
            List<IVRM_Master_Gender> gender = new List<IVRM_Master_Gender>();
            // List<> leaveyear = new List<HR_Master_LeaveYearDMO>();
            try
            {
                


                    //Leaveyear
                    gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IVRMMG_ActiveFlag == true).ToList();
                    dto.genderdropdown = gender.ToArray();

                    //IncomeTax cess List

                    incomecesslist = _HRMSContext.HR_Master_IncomeTaxCess.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.incomeTaxCessdropdown = incomecesslist.ToArray();


                dto.incomeTaxList = (from ict in _HRMSContext.HR_MasterIncomeTax
                                     from gen in _HRMSContext.IVRM_Master_Gender
                                     from fin in _HRMSContext.IVRM_Master_FinancialYear
                                     where ict.HRMIT_GenderFlag == gen.IVRMMG_Id
                                     && ict.IMFY_Id == fin.IMFY_Id
                                     && ict.MI_Id == dto.MI_Id
                                     select new HR_Master_IncomeTaxDMO
                                     {
                                         HRMIT_GenderFlag = ict.HRMIT_GenderFlag,
                                         HRMIT_Id = ict.HRMIT_Id,
                                         MI_Id = ict.MI_Id,
                                         IMFY_Id = ict.IMFY_Id,
                                         HRMIT_AgeFlag = ict.HRMIT_AgeFlag,
                                         HRMIT_FromAge = ict.HRMIT_FromAge,
                                         HRMIT_ToAge = ict.HRMIT_ToAge,
                                         HRMIT_ActiveFlag = ict.HRMIT_ActiveFlag,
                                         gendername = ict.gendername,
                                         financilYear = ict.financilYear,
                                         CreatedDate = ict.CreatedDate,
                                         UpdatedDate = ict.UpdatedDate
                                     }).ToArray();



                dto.financialYeardropdown = _HRMSContext.IVRM_Master_FinancialYear.OrderBy(t => t.IMFY_OrderBy).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
