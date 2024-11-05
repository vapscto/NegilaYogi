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
    public class HRMasterLoanService:Interfaces.HRMasterLoanInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HRMasterLoanService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }
    public HRMasterLoanDTO getBasicData(HRMasterLoanDTO dto)
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

    public HRMasterLoanDTO SaveUpdate(HRMasterLoanDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
                HRMasterLoan dmoObj = Mapper.Map<HRMasterLoan>(dto);

                var duplicatecountresult = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id == dto.MI_Id && t.HRML_LoanType == dto.HRML_LoanType && t.HRML_Max == dto.HRML_Max).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.HRMLN_Id > 0)
                    {

                        var duplicatecount = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id == dto.MI_Id && t.HRML_LoanType == dto.HRML_LoanType && t.HRMLN_Id != dto.HRMLN_Id).Count();
                        if (duplicatecount == 0)
                            {
                            var result = _HRMSContext.HRMasterLoan.Single(t => t.HRMLN_Id == dmoObj.HRMLN_Id);

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
                        dto.retrunMsg = "Duplicate";
                        }
                    }
                    else
                    {
                        var duplicatecount = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id == dto.MI_Id && t.HRML_LoanType == dto.HRML_LoanType).Count();
                        if (duplicatecount == 0)
                            {


                            dmoObj.HRMLN_ActiveFlag = true;
                            dmoObj.HRML_LoanType = dto.HRML_LoanType;
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
                            }else
                            {
                            dto.retrunMsg = "Duplicate";
                            }
                    }
                }else
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

    public HRMasterLoanDTO editData(int id)
    {

      HRMasterLoanDTO dto = new HRMasterLoanDTO();
      dto.retrunMsg = "";
      try
      {
        List<HRMasterLoan> lorg = new List<HRMasterLoan>();
        lorg = _HRMSContext.HRMasterLoan.AsNoTracking().Where(t => t.HRMLN_Id.Equals(id)).ToList();
        dto.gmasterloanList = lorg.ToArray();
      }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }

    public HRMasterLoanDTO deactivate(HRMasterLoanDTO dto)
    {
      dto.retrunMsg = "";
      try
      {
        if (dto.HRMLN_Id > 0)
        {
          var result = _HRMSContext.HRMasterLoan.Single(t => t.HRMLN_Id == dto.HRMLN_Id);

          if (result.HRMLN_ActiveFlag == true)
          {
            result.HRMLN_ActiveFlag = false;
          }
          else if (result.HRMLN_ActiveFlag == false)
          {
            result.HRMLN_ActiveFlag = true;
          }
          result.UpdatedDate = DateTime.Now;

          _HRMSContext.Update(result);
          var flag = _HRMSContext.SaveChanges();
          if (flag > 0)
          {
            if (result.HRMLN_ActiveFlag == true)
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

    public HRMasterLoanDTO GetAllDropdownAndDatatableDetails(HRMasterLoanDTO dto)
    {
      List<HRMasterLoan> datalist = new List<HRMasterLoan>();
      try
      {
        
          datalist = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
          dto.gmasterloanList = datalist.ToArray();
      }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
      }
      return dto;
    }
  }
}
