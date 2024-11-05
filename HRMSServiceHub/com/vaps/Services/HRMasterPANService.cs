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
    public class HRMasterPANService : Interfaces.HRMasterPANInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public HRMasterPANService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
    {
      _HRMSContext = HRMSContext;
      _Context = Context;
    }
    public HRMasterPANDTO getBasicData(HRMasterPANDTO dto)
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

        public HRMasterPANDTO SaveUpdate(HRMasterPANDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HRMasterPAN dmoObj = Mapper.Map<HRMasterPAN>(dto);

                if (dmoObj.HRMPN_Id > 0)
                {
                    var result = _HRMSContext.HRMasterPAN.Single(t => t.HRMPN_Id == dmoObj.HRMPN_Id);
                    dmoObj.HRMPN_ActiveFlag = true;
                   // dmoObj.CreatedDate = DateTime.Now;
                    dmoObj.UpdatedDate = DateTime.Now;
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
                    dmoObj.MI_Id = dto.MI_Id;
                    dmoObj.HRMPN_ActiveFlag = true;
                    dmoObj.CreatedDate = DateTime.Now;
                    dmoObj.UpdatedDate = DateTime.Now;
                    _HRMSContext.Add(dmoObj);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag == 1)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "False";
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

    public HRMasterPANDTO editData(int id)
    {

            HRMasterPANDTO dto = new HRMasterPANDTO();
      dto.retrunMsg = "";
      try
      {
        List<HRMasterPAN> lorg = new List<HRMasterPAN>();
        lorg = _HRMSContext.HRMasterPAN.AsNoTracking().Where(t => t.HRMPN_Id.Equals(id)).ToList();
        dto.gmasterloanList = lorg.ToArray();
      }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
        dto.retrunMsg = "Error occured";
      }

      return dto;
    }

    public HRMasterPANDTO deactivate(HRMasterPANDTO dto)
    {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMPN_Id > 0)
                {
                    var result = _HRMSContext.HRMasterPAN.Single(t => t.HRMPN_Id == dto.HRMPN_Id);

                    if (result.HRMPN_ActiveFlag == true)
                    {
                        result.HRMPN_ActiveFlag = false;
                    }
                    else if (result.HRMPN_ActiveFlag == false)
                    {
                        result.HRMPN_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMPN_ActiveFlag == true)
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

    public HRMasterPANDTO GetAllDropdownAndDatatableDetails(HRMasterPANDTO dto)
    {
      List<HRMasterPAN> datalist = new List<HRMasterPAN>();
      try
      {
        
          datalist = _HRMSContext.HRMasterPAN.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
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
