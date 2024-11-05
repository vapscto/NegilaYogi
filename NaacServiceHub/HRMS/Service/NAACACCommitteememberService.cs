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
    public class NAACACCommitteememberService : Interface.NAACACCommitteememberInterface
    {

        public NaacHRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HRMSContext _HRMS;

        public NAACACCommitteememberService(NaacHRMSContext HRMSContext, DomainModelMsSqlServerContext Context, HRMSContext HRMS)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
            _HRMS = HRMS;
        }

        public NAACACCommitteeMembersDTO getBasicData(NAACACCommitteeMembersDTO dto)
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

        public NAACACCommitteeMembersDTO SaveUpdate(NAACACCommitteeMembersDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                NAACACCommitteeMembersDMO dmoObj = Mapper.Map<NAACACCommitteeMembersDMO>(dto);
                var duplicatecountresult = _HRMSContext.NAACACCommitteeMembersDMO.Where(t => t.NCACCOMMM_MemberName == dto.NCACCOMMM_MemberName && t.NCACCOMM_Id == dto.NCACCOMM_Id).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.NCACCOMMM_Id > 0)
                    {
                        var result = _HRMSContext.NAACACCommitteeMembersDMO.Single(t => t.NCACCOMMM_Id == dmoObj.NCACCOMMM_Id);
                        dto.NCACCOMMM_UpdatedDate = DateTime.Now;
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
                        dmoObj.NCACCOMMM_ActiveFlg = true;
                        dmoObj.NCACCOMM_Id = dto.NCACCOMM_Id;
                        dmoObj.HRME_Id = dto.HRME_Id;
                        dmoObj.NCACCOMMM_MemberName = dto.NCACCOMMM_MemberName;
                        dmoObj.NCACCOMMM_MemberDetails = dto.NCACCOMMM_MemberDetails;
                        dmoObj.NCACCOMMM_MemberPhoneNo = dto.NCACCOMMM_MemberPhoneNo;
                        dmoObj.NCACCOMMM_MemberEmailId = dto.NCACCOMMM_MemberEmailId;
                        dmoObj.NCACCOMMM_Role = dto.NCACCOMMM_Role;
                        dmoObj.NCACCOMMM_FileName = dto.NCACCOMMM_FileName;
                        dmoObj.NCACCOMMM_FilePath = dto.NCACCOMMM_FilePath;
                        dmoObj.NCACCOMMM_CreatedBy = dto.roleId;
                        dmoObj.NCACCOMMM_UpdatedBy = dto.roleId;
                        dmoObj.NCACCOMMM_CreatedDate = DateTime.Now;
                        dmoObj.NCACCOMMM_UpdatedDate = DateTime.Now;
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

        public NAACACCommitteeMembersDTO editData(int id)
        {
            NAACACCommitteeMembersDTO dto = new NAACACCommitteeMembersDTO();
            dto.retrunMsg = "";
            try
            {
                List<NAACACCommitteeMembersDMO> lorg = new List<NAACACCommitteeMembersDMO>();
                lorg = _HRMSContext.NAACACCommitteeMembersDMO.AsNoTracking().Where(t => t.NCACCOMMM_Id.Equals(id)).ToList();
                dto.commetteeList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public NAACACCommitteeMembersDTO deactivate(NAACACCommitteeMembersDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.NCACCOMMM_Id > 0)
                {
                    var result = _HRMSContext.NAACACCommitteeMembersDMO.Single(t => t.NCACCOMMM_Id == dto.NCACCOMMM_Id);

                    if (result.NCACCOMMM_ActiveFlg == true)
                    {
                        result.NCACCOMMM_ActiveFlg = false;
                    }
                    else if (result.NCACCOMMM_ActiveFlg == false)
                    {
                        result.NCACCOMMM_ActiveFlg = true;
                    }

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.NCACCOMMM_ActiveFlg == true)
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

        public NAACACCommitteeMembersDTO GetAllDropdownAndDatatableDetails(NAACACCommitteeMembersDTO dto)
        {
            List<NAACACCommitteeMembersDMO> datalist = new List<NAACACCommitteeMembersDMO>();
            try
            {
                //datalist = _HRMSContext.NAACACCommitteeMembersDMO.ToList();
                //dto.commetteeList = datalist.ToArray();

                dto.commetteeList = (from a in _HRMSContext.NAACACCommitteeMembersDMO
                                     from b in _HRMSContext.MasterEmployee
                                     from c in _HRMSContext.NAAC_AC_CommitteeDMO
                                     where (a.HRME_Id == b.HRME_Id && a.NCACCOMM_Id == c.NCACCOMM_Id && b.MI_Id == dto.MI_Id && c.MI_Id == dto.MI_Id)
                                     select new NAACACCommitteeMembersDTO
                                     {
                                         NCACCOMMM_Id = a.NCACCOMMM_Id,
                                         NCACCOMM_Id = a.NCACCOMM_Id,
                                         NCACCOMM_CommitteeName = c.NCACCOMM_CommitteeName,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + ' ' + b.HRME_EmployeeMiddleName + ' ' + b.HRME_EmployeeLastName,
                                         NCACCOMMM_MemberName = a.NCACCOMMM_MemberName,
                                         NCACCOMMM_MemberDetails = a.NCACCOMMM_MemberDetails,
                                         NCACCOMMM_MemberPhoneNo = a.NCACCOMMM_MemberPhoneNo,
                                         NCACCOMMM_MemberEmailId = a.NCACCOMMM_MemberEmailId
                                     }).ToArray();

                dto.committeedropdownlist = _HRMSContext.NAAC_AC_CommitteeDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.empdropdownlist = (from a in _HRMS.MasterEmployee
                                       where (a.MI_Id == dto.MI_Id) select new NAACACCommitteeMembersDTO
                                       {
                                           HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + ' ' + a.HRME_EmployeeMiddleName + ' ' + a.HRME_EmployeeLastName,
                                           HRME_Id = a.HRME_Id
                                        }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
