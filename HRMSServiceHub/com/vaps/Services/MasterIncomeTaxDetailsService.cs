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
    public class MasterIncomeTaxDetailsService : Interfaces.MasterIncomeTaxDetailsInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterIncomeTaxDetailsService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_IncomeTax_DetailsDTO getBasicData(HR_Master_IncomeTax_DetailsDTO dto)
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

        public HR_Master_IncomeTax_DetailsDTO SaveUpdate(HR_Master_IncomeTax_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_IncomeTax_DetailsDMO dmoObj = Mapper.Map<HR_Master_IncomeTax_DetailsDMO>(dto);
                //if (dmoObj.HRMITD_Id > 0)
                //{
                    var duplicateresult = _HRMSContext.HR_MasterIncomeTaxDetails.Where(t =>t.HRMITD_Id == dto.HRMITD_Id && t.HRMIT_Id == dto.HRMIT_Id && t.HRMITD_AmountFrom == dto.HRMITD_AmountFrom && t.HRMITD_AmountTo == dto.HRMITD_AmountTo && t.HRMITD_IncomeTax == dto.HRMITD_IncomeTax).Count();
                if (duplicateresult > 0)
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
                else
                {
                    var duplicatecountresults = _HRMSContext.HR_MasterIncomeTaxDetails.Where(t => t.HRMITD_Id == dto.HRMITD_Id).Count();//&& t.HRMIT_Id == dto.HRMIT_Id && t.HRMITD_AmountFrom == dto.HRMITD_AmountFrom && t.HRMITD_AmountTo == dto.HRMITD_AmountTo && t.HRMITD_IncomeTax == dto.HRMITD_IncomeTax).Count();
                    if (duplicatecountresults == 0)
                    {
                        dmoObj.HRMITD_ActiveFlag = true;
                        dmoObj.HRMIT_Id = dto.HRMIT_Id;
                        dmoObj.HRMITD_AmountFrom = dto.HRMITD_AmountFrom;
                        dmoObj.HRMITD_AmountTo = dto.HRMITD_AmountTo;
                        dmoObj.HRMITD_IncomeTax = dto.HRMITD_IncomeTax;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.HRMITD_Id = dmoObj.HRMITD_Id;
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        var result = _HRMSContext.HR_MasterIncomeTaxDetails.Single(t => t.HRMITD_Id == dmoObj.HRMITD_Id);
                        dto.UpdatedDate = DateTime.Now;
                        dto.CreatedDate = result.CreatedDate;
                        dto.HRMITD_ActiveFlag = true;
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
                

                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Master_IncomeTax_DetailsDTO editData(int id)
        {

            HR_Master_IncomeTax_DetailsDTO dto = new HR_Master_IncomeTax_DetailsDTO();
            dto.retrunMsg = "";
            try
            {
                //List<HR_Master_IncomeTax_DetailsDMO> lorg = new List<HR_Master_IncomeTax_DetailsDMO>();
                //lorg = _HRMSContext.HR_MasterIncomeTaxDetails.AsNoTracking().Where(t => t.HRMIT_Id.Equals(id)).ToList();
                //dto.incomeTaxDetailsList = lorg.ToArray();

                //List<HR_Master_IncomeTaxDMO> lor = new List<HR_Master_IncomeTaxDMO>();
                //lor = _HRMSContext.HR_MasterIncomeTax.AsNoTracking().Where(t => t.HRMIT_Id.Equals(id)).ToList();
                //dto.incomeTax = lor.ToArray();


                dto.incomeTaxDetailsList = (from mitd in _HRMSContext.HR_MasterIncomeTaxDetails
                                            from mit in _HRMSContext.HR_MasterIncomeTax
                                            where mitd.HRMIT_Id == mit.HRMIT_Id && mitd.HRMITD_Id.Equals(id)
                                            select new HR_Master_IncomeTax_DetailsDTO
                                            {
                                                HRMIT_Id = mitd.HRMIT_Id,
                                                HRMIT_AgeFlag = mit.HRMIT_AgeFlag,
                                                HRMITD_ActiveFlag = mitd.HRMITD_ActiveFlag,
                                                HRMITD_AmountFrom = mitd.HRMITD_AmountFrom,
                                                HRMITD_AmountTo = mitd.HRMITD_AmountTo,
                                                HRMITD_IncomeTax = mitd.HRMITD_IncomeTax
                                                //MasterIncomeTax = mitd.MasterIncomeTax
                                            }).OrderBy(mitd => mitd.CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_IncomeTax_DetailsDTO deactivate(HR_Master_IncomeTax_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMIT_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterIncomeTaxDetails.Single(t => t.HRMITD_Id == dto.HRMIT_Id);

                    if (result.HRMITD_ActiveFlag == true)
                    {
                        result.HRMITD_ActiveFlag = false;
                    }
                    else if (result.HRMITD_ActiveFlag == false)
                    {
                        result.HRMITD_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMITD_ActiveFlag == true)
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

        public HR_Master_IncomeTax_DetailsDTO GetAllDropdownAndDatatableDetails(HR_Master_IncomeTax_DetailsDTO dto)
        {
            List<HR_Master_IncomeTax_DetailsDMO> datalist = new List<HR_Master_IncomeTax_DetailsDMO>();
            //List<HR_Master_IncomeTax_DetailsDMO> dropdownIncTax = new List<HR_Master_IncomeTax_DetailsDMO>();
            try
            {
                    //datalist = _HRMSContext.HR_MasterIncomeTaxDetails.ToList();
                    //dto.incomeTaxDetailsList = datalist.ToArray();
                    dto.incomeTaxList = (from a in _HRMSContext.HR_MasterIncomeTax
                                        from b in _HRMSContext.IVRM_Master_Gender
                                        where a.MI_Id.Equals(dto.MI_Id) && b.IVRMMG_Id == a.HRMIT_GenderFlag && a.HRMIT_ActiveFlag==true && b.IVRMMG_ActiveFlag==true
                                        select 
                                        new HR_Master_IncomeTax_DetailsDTO
                                        {
                                            HRMIT_Id = a.HRMIT_Id,
                                            HRMIT_AgeFlag = a.HRMIT_AgeFlag + " " + b.IVRMMG_GenderName,


                                       
                                            
                                        }).OrderBy(mitd => mitd.CreatedDate).ToArray();



                dto.incomeTaxDetailsList = (from mitd in _HRMSContext.HR_MasterIncomeTaxDetails
                                     from mit in _HRMSContext.HR_MasterIncomeTax
                                     from abc in _HRMSContext.IVRM_Master_Gender
                                     where mitd.HRMIT_Id == mit.HRMIT_Id && mit.MI_Id==dto.MI_Id && abc.IVRMMG_Id==mit.HRMIT_GenderFlag
                                     select new HR_Master_IncomeTax_DetailsDTO
                                     {
                                         HRMIT_Id = mitd.HRMIT_Id,
                                         HRMIT_AgeFlag=mit.HRMIT_AgeFlag +" "+ abc.IVRMMG_GenderName,
                                         HRMITD_Id = mitd.HRMITD_Id,
                                         HRMITD_ActiveFlag = mitd.HRMITD_ActiveFlag,
                                         HRMITD_AmountFrom=mitd.HRMITD_AmountFrom,
                                         HRMIT_GenderFlag=mit.HRMIT_GenderFlag,
                                         IVRMMG_GenderName=abc.IVRMMG_GenderName,
                                         HRMITD_AmountTo =mitd.HRMITD_AmountTo,
                                         HRMITD_IncomeTax=mitd.HRMITD_IncomeTax

                                         //MasterIncomeTax = mitd.MasterIncomeTax
                                     }).OrderBy(mitd => mitd.CreatedDate).ToArray();

               // hrmE_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
