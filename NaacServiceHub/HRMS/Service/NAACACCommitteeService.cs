using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.HRMS;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NaacServiceHub.HRMS.Services
{
    public class NAACACCommitteeService : Interface.NAACACCommitteeInterface
    {

        public NaacHRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public NAACACCommitteeService(NaacHRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public NAACACCommitteeDTO getBasicData(NAACACCommitteeDTO dto)
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

        public NAACACCommitteeDTO SaveUpdate(NAACACCommitteeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                NAACACCommitteeDMO dmoObj = Mapper.Map<NAACACCommitteeDMO>(dto);
                var duplicatecountresult = _HRMSContext.NAAC_AC_CommitteeDMO.Where(t => t.MI_Id == dto.MI_Id && t.NCACCOMM_CommitteeName == dto.NCACCOMM_CommitteeName).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.NCACCOMM_Id > 0)
                    {
                        var result = _HRMSContext.NAAC_AC_CommitteeDMO.Single(t => t.NCACCOMM_Id == dmoObj.NCACCOMM_Id);
                        dto.NCACCOMM_UpdatedDate = DateTime.Now;
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
                        dmoObj.NCACCOMM_ActiveFlg = true;
                        dmoObj.NCACCOMM_CommitteeName = dto.NCACCOMM_CommitteeName;
                        dmoObj.NCACCOMM_Flg = dto.NCACCOMM_Flg;
                        dmoObj.NCACCOMM_Year = dto.NCACCOMM_Year;
                        dmoObj.NCACCOMM_FileName = dto.NCACCOMM_FileName;
                        dmoObj.NCACCOMM_FilePath = dto.NCACCOMM_FilePath;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.NCACCOMM_CreatedBy = dto.roleId;
                        dmoObj.NCACCOMM_UpdatedBy = dto.roleId;
                        dmoObj.NCACCOMM_CreatedDate = DateTime.Now;
                        dmoObj.NCACCOMM_UpdatedDate = DateTime.Now;
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

        public NAACACCommitteeDTO editData(int id)
        {
            NAACACCommitteeDTO dto = new NAACACCommitteeDTO();
            dto.retrunMsg = "";
            try
            {
                List<NAACACCommitteeDMO> lorg = new List<NAACACCommitteeDMO>();
                lorg = _HRMSContext.NAAC_AC_CommitteeDMO.AsNoTracking().Where(t => t.NCACCOMM_Id.Equals(id)).ToList();
                dto.commetteeList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public NAACACCommitteeDTO deactivate(NAACACCommitteeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.NCACCOMM_Id > 0)
                {
                    var result = _HRMSContext.NAAC_AC_CommitteeDMO.Single(t => t.NCACCOMM_Id == dto.NCACCOMM_Id);

                    if (result.NCACCOMM_ActiveFlg == true)
                    {
                        result.NCACCOMM_ActiveFlg = false;
                    }
                    else if (result.NCACCOMM_ActiveFlg == false)
                    {
                        result.NCACCOMM_ActiveFlg = true;
                    }

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.NCACCOMM_ActiveFlg == true)
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

        public NAACACCommitteeDTO GetAllDropdownAndDatatableDetails(NAACACCommitteeDTO dto)
        {
            List<NAACACCommitteeDMO> datalist = new List<NAACACCommitteeDMO>();
            try
            {
                datalist = _HRMSContext.NAAC_AC_CommitteeDMO.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.commetteeList = datalist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
