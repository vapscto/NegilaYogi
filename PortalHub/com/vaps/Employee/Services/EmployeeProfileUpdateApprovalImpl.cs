using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeProfileUpdateApprovalImpl : Interfaces.EmployeeProfileUpdateApprovalInterface
    {

        public PortalContext _portalContext;
        public EmployeeProfileUpdateApprovalImpl(PortalContext _portal)
        {
            _portalContext = _portal;
        }
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdate(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                data.GetMartialStatusList = _portalContext.IVRM_Master_Marital_Status.Where(a => a.MI_Id == data.MI_Id && a.IVRMMMS_ActiveFlag == true).ToArray();

                data.GetGenderList = _portalContext.IVRM_Master_Gender.Where(a => a.MI_Id == data.MI_Id && a.IVRMMG_ActiveFlag == true).ToArray();

                data.GetCountryList = _portalContext.country.ToArray();

                data.GetReligionList = _portalContext.MasterReligionDMO.Where(a => a.Is_Active == true).ToArray();

                var GetHRME_Id = _portalContext.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.UserId).ToList();
                if (GetHRME_Id.Count > 0)
                {
                    data.HRME_Id = GetHRME_Id.FirstOrDefault().Emp_Code;

                    var CheckRequestedData = _portalContext.HR_Master_Employee_Update_RequestDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id
                     && a.HRMEREQREQ_ReqStatus == "In Progress").ToList();

                    if (CheckRequestedData.Count > 0)
                    {
                        data.HRMEREQ_Id = CheckRequestedData.FirstOrDefault().HRMEREQ_Id;

                        data.GetEmployeeDetails = CheckRequestedData.ToArray();

                        data.GetEmployeEmailIdDetails = _portalContext.HR_Master_Employee_Update_Request_EmailIdDMO.Where(a => a.HRMEREQ_Id == data.HRMEREQ_Id).ToArray();

                        data.GetEmployeMobileNoDetails = _portalContext.HR_Master_Employee_Update_Request_MobileNoDMO.Where(a => a.HRMEREQ_Id == data.HRMEREQ_Id).ToArray();

                        if (CheckRequestedData.FirstOrDefault().HRMEREQ_LocCountryId != null)
                        {
                            data.GetLocalStateList = _portalContext.state.Where(a => a.IVRMMC_Id == CheckRequestedData.FirstOrDefault().HRMEREQ_LocCountryId).ToArray();
                        }

                        if (CheckRequestedData.FirstOrDefault().HRMEREQ_PerCountryId != null)
                        {
                            data.GetPerStateList = _portalContext.state.Where(a => a.IVRMMC_Id == CheckRequestedData.FirstOrDefault().HRMEREQ_PerCountryId).ToArray();
                        }

                        if (CheckRequestedData.FirstOrDefault().ReligionId != null)
                        {
                            data.GetCasteCategoryList = (from a in _portalContext.ReligionCategory_MappingDMO
                                                         from b in _portalContext.CasteCategory
                                                         where (a.IVRMMR_Id == CheckRequestedData.FirstOrDefault().ReligionId && a.IMCC_Id == b.IMCC_Id
                                                         && a.IRCC_ActiveFlg == true)
                                                         select b).Distinct().ToArray();
                        }

                        if (CheckRequestedData.FirstOrDefault().CasteCategoryId != null)
                        {
                            var AllCastess = (from a in _portalContext.Caste
                                              from b in _portalContext.CasteCategory
                                              where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == data.MI_Id
                                              && a.IMCC_Id == CheckRequestedData.FirstOrDefault().CasteCategoryId)
                                              select a).ToArray();

                            data.GetCasteList = AllCastess.Distinct().ToArray();
                        }
                    }

                    else
                    {
                        var EmployeeDetails = _portalContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id).ToList();

                        var EmployeeDetails_temp = (from a in _portalContext.HR_Master_Employee_DMO
                                                    where (a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                                    select new EmployeeProfileUpdateApprovalDTO
                                                    {

                                                        HRME_Id = a.HRME_Id,
                                                        HRMEREQ_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                        HRMEREQ_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                        HRMEREQ_EmployeeLastName = a.HRME_EmployeeLastName,
                                                        HRMEREQ_PerStreet = a.HRME_PerStreet,
                                                        HRMEREQ_PerArea = a.HRME_PerArea,
                                                        HRMEREQ_PerCity = a.HRME_PerCity,
                                                        HRMEREQ_PerStateId = a.HRME_PerStateId,
                                                        HRMEREQ_PerCountryId = a.HRME_PerCountryId,
                                                        HRMEREQ_PerPincode = a.HRME_PerPincode,
                                                        HRMEREQ_LocStreet = a.HRME_LocStreet,
                                                        HRMEREQ_LocArea = a.HRME_LocArea,
                                                        HRMEREQ_LocCity = a.HRME_LocCity,
                                                        HRMEREQ_LocStateId = a.HRME_LocStateId,
                                                        HRMEREQ_LocCountryId = a.HRME_LocCountryId,
                                                        HRMEREQ_LocPincode = a.HRME_PerPincode,
                                                        IVRMMMS_Id = a.IVRMMMS_Id,
                                                        IVRMMG_Id = a.IVRMMG_Id,
                                                        CasteCategoryId = a.CasteCategoryId,
                                                        CasteId = a.CasteId,
                                                        ReligionId = a.ReligionId,
                                                        HRMEREQ_FatherName = a.HRME_FatherName,
                                                        HRMEREQ_MotherName = a.HRME_MotherName,
                                                        HRMEREQ_SpouseName = a.HRME_SpouseName,
                                                        HRMEREQ_SpouseOccupation = a.HRME_SpouseOccupation,
                                                        HRMEREQ_SpouseMobileNo = a.HRME_SpouseMobileNo,
                                                        HRMEREQ_SpouseEmailId = a.HRME_SpouseEmailId,
                                                        HRMEREQ_SpouseAddress = a.HRME_SpouseAddress,
                                                        HRMEREQ_DOB = a.HRME_DOB,
                                                        HRMEREQ_BloodGroup = a.HRME_BloodGroup,
                                                        HRMEREQ_Photo = a.HRME_Photo,
                                                    }).Distinct().ToArray();

                        data.GetEmployeeDetails = EmployeeDetails_temp.ToArray();

                        data.GetEmployeEmailIdDetails = (from a in _portalContext.Multiple_Email_DMO
                                                         where (a.HRME_Id == data.HRME_Id)
                                                         select new EmployeeProfileUpdateApprovalDTO
                                                         {
                                                             HRMEREQEM_EmailId = a.HRMEM_EmailId,
                                                             HRMEREQEM_Flag = a.HRMEM_DeFaultFlag,

                                                         }).Distinct().ToArray();

                        data.GetEmployeMobileNoDetails = (from a in _portalContext.Multiple_Mobile_DMO
                                                          where (a.HRME_Id == data.HRME_Id)
                                                          select new EmployeeProfileUpdateApprovalDTO
                                                          {
                                                              HRMEREQMN_MobileNo = a.HRMEMNO_MobileNo,
                                                              HRMEREQMN_Flag = a.HRMEMNO_DeFaultFlag,
                                                          }).Distinct().ToArray();

                        if (EmployeeDetails.FirstOrDefault().HRME_LocCountryId != null)
                        {
                            data.GetLocalStateList = _portalContext.state.Where(a => a.IVRMMC_Id == EmployeeDetails.FirstOrDefault().HRME_LocCountryId).ToArray();
                        }

                        if (EmployeeDetails.FirstOrDefault().HRME_PerCountryId != null)
                        {
                            data.GetPerStateList = _portalContext.state.Where(a => a.IVRMMC_Id == EmployeeDetails.FirstOrDefault().HRME_PerCountryId).ToArray();
                        }

                        if (EmployeeDetails.FirstOrDefault().ReligionId != null)
                        {
                            data.GetCasteCategoryList = (from a in _portalContext.ReligionCategory_MappingDMO
                                                         from b in _portalContext.CasteCategory
                                                         where (a.IVRMMR_Id == EmployeeDetails.FirstOrDefault().ReligionId && a.IMCC_Id == b.IMCC_Id
                                                         && a.IRCC_ActiveFlg == true)
                                                         select b).Distinct().ToArray();
                        }

                        if (EmployeeDetails.FirstOrDefault().CasteCategoryId != null)
                        {
                            var AllCastess = (from a in _portalContext.Caste
                                              from b in _portalContext.CasteCategory
                                              where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == data.MI_Id && a.IMCC_Id == EmployeeDetails.FirstOrDefault().CasteCategoryId)
                                              select a).ToArray();

                            data.GetCasteList = AllCastess.Distinct().ToArray();
                        }
                    }

                    data.GetRequestedData = (from a in _portalContext.HR_Master_Employee_Update_RequestDMO
                                             where (a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                             select new EmployeeProfileUpdateApprovalDTO
                                             {
                                                 CreatedDate = a.CreatedDate,
                                                 UpdatedDate = a.UpdatedDate,
                                                 HRMEREQREQ_ReqStatus = a.HRMEREQREQ_ReqStatus,
                                                 HRMEREQREQ_ApprovedFlg = a.HRMEREQREQ_ApprovedFlg,
                                                 username = a.HRMEREQREQ_ApprovedBy > 0 ? _portalContext.ApplicationUser.Where(c => c.Id == a.HRMEREQREQ_ApprovedBy).FirstOrDefault().UserName : "",


                                             }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO Getcastecatgory(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                data.GetCasteCategoryList = (from a in _portalContext.ReligionCategory_MappingDMO
                                             from b in _portalContext.CasteCategory
                                             where (a.IVRMMR_Id == data.ReligionId && a.IMCC_Id == b.IMCC_Id
                                             && a.IRCC_ActiveFlg == true)
                                             select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO Getcaste(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                var AllCastess = (from a in _portalContext.Caste
                                  from b in _portalContext.CasteCategory
                                  where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == data.MI_Id && a.IMCC_Id == data.CasteCategoryId)
                                  select a).ToArray();

                data.GetCasteList = AllCastess.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO SaveData(EmployeeProfileUpdateApprovalDTO data)
        {

            try
            {
                long? MobileNo = null;
                string emailid = "";

                if (data.Temp_MobileNo != null && data.Temp_MobileNo.Length > 0)
                {
                    foreach (var d in data.Temp_MobileNo)
                    {
                        if (d.HRMEREQMN_Flag == "default")
                        {
                            MobileNo = d.HRMEREQMN_MobileNo;
                        }
                    }
                }

                if (data.Temp_EmailId != null && data.Temp_EmailId.Length > 0)
                {
                    foreach (var d in data.Temp_EmailId)
                    {
                        if (d.HRMEREQEM_Flag == "default")
                        {
                            emailid = d.HRMEREQEM_EmailId;
                        }
                    }
                }


                data.returnval = false;
                if (data.HRMEREQ_Id > 0)
                {
                    data.message = "Update";

                    var result = _portalContext.HR_Master_Employee_Update_RequestDMO.Single(a => a.HRMEREQ_Id == data.HRMEREQ_Id);
                    result.HRMEREQ_EmployeeFirstName = data.HRMEREQ_EmployeeFirstName;
                    result.HRMEREQ_EmployeeMiddleName = data.HRMEREQ_EmployeeMiddleName;
                    result.HRMEREQ_EmployeeLastName = data.HRMEREQ_EmployeeLastName;
                    result.HRMEREQ_PerStreet = data.HRMEREQ_PerStreet;
                    result.HRMEREQ_PerArea = data.HRMEREQ_PerArea;
                    result.HRMEREQ_PerCity = data.HRMEREQ_PerCity;
                    result.HRMEREQ_PerStateId = data.HRMEREQ_PerStateId;
                    result.HRMEREQ_PerCountryId = data.HRMEREQ_PerCountryId;
                    result.HRMEREQ_PerPincode = data.HRMEREQ_PerPincode;
                    result.HRMEREQ_LocStreet = data.HRMEREQ_LocStreet;
                    result.HRMEREQ_LocArea = data.HRMEREQ_LocArea;
                    result.HRMEREQ_LocCity = data.HRMEREQ_LocCity;
                    result.HRMEREQ_LocStateId = data.HRMEREQ_LocStateId;
                    result.HRMEREQ_LocCountryId = data.HRMEREQ_LocCountryId;
                    result.HRMEREQ_LocPincode = data.HRMEREQ_LocPincode;
                    result.IVRMMMS_Id = data.IVRMMMS_Id;
                    result.IVRMMG_Id = data.IVRMMG_Id;
                    result.CasteCategoryId = data.CasteCategoryId;
                    result.CasteId = data.CasteId;
                    result.ReligionId = data.ReligionId;
                    result.HRMEREQ_FatherName = data.HRMEREQ_FatherName;
                    result.HRMEREQ_MotherName = data.HRMEREQ_MotherName;
                    result.HRMEREQ_SpouseName = data.HRMEREQ_SpouseName;
                    result.HRMEREQ_SpouseOccupation = data.HRMEREQ_SpouseOccupation;
                    result.HRMEREQ_SpouseMobileNo = data.HRMEREQ_SpouseMobileNo;
                    result.HRMEREQ_SpouseEmailId = data.HRMEREQ_SpouseEmailId;
                    result.HRMEREQ_SpouseAddress = data.HRMEREQ_SpouseAddress;
                    result.HRMEREQ_DOB = data.HRMEREQ_DOB;
                    result.HRMEREQ_BloodGroup = data.HRMEREQ_BloodGroup;
                    result.HRMEREQ_Photo = data.HRMEREQ_Photo;
                    result.HRMEREQ_MobileNo = MobileNo;
                    result.HRMEREQ_EmailId = emailid;
                    result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    _portalContext.Update(result);
                    SaveUpdateMobileNo(data);
                    SaveUpdateEmailId(data);
                    var i = _portalContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }
                else
                {
                    data.message = "Add";
                    HR_Master_Employee_Update_RequestDMO hR_Master_Employee_Update_RequestDMO = new HR_Master_Employee_Update_RequestDMO();

                    hR_Master_Employee_Update_RequestDMO.HRME_Id = data.HRME_Id;
                    hR_Master_Employee_Update_RequestDMO.MI_Id = data.MI_Id;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_EmployeeFirstName = data.HRMEREQ_EmployeeFirstName;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_EmployeeMiddleName = data.HRMEREQ_EmployeeMiddleName;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_EmployeeLastName = data.HRMEREQ_EmployeeLastName;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_PerStreet = data.HRMEREQ_PerStreet;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_PerArea = data.HRMEREQ_PerArea;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_PerCity = data.HRMEREQ_PerCity;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_PerStateId = data.HRMEREQ_PerStateId;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_PerCountryId = data.HRMEREQ_PerCountryId;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_PerPincode = data.HRMEREQ_PerPincode;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_LocStreet = data.HRMEREQ_LocStreet;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_LocArea = data.HRMEREQ_LocArea;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_LocCity = data.HRMEREQ_LocCity;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_LocStateId = data.HRMEREQ_LocStateId;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_LocCountryId = data.HRMEREQ_LocCountryId;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_LocPincode = data.HRMEREQ_LocPincode;
                    hR_Master_Employee_Update_RequestDMO.IVRMMMS_Id = data.IVRMMMS_Id;
                    hR_Master_Employee_Update_RequestDMO.IVRMMG_Id = data.IVRMMG_Id;
                    hR_Master_Employee_Update_RequestDMO.CasteCategoryId = data.CasteCategoryId;
                    hR_Master_Employee_Update_RequestDMO.CasteId = data.CasteId;
                    hR_Master_Employee_Update_RequestDMO.ReligionId = data.ReligionId;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_FatherName = data.HRMEREQ_FatherName;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_MotherName = data.HRMEREQ_MotherName;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_SpouseName = data.HRMEREQ_SpouseName;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_SpouseOccupation = data.HRMEREQ_SpouseOccupation;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_SpouseMobileNo = data.HRMEREQ_SpouseMobileNo;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_SpouseEmailId = data.HRMEREQ_SpouseEmailId;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_SpouseAddress = data.HRMEREQ_SpouseAddress;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_DOB = data.HRMEREQ_DOB;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_BloodGroup = data.HRMEREQ_BloodGroup;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_Photo = data.HRMEREQ_Photo;
                    hR_Master_Employee_Update_RequestDMO.CreatedDate = DateTime.Now;
                    hR_Master_Employee_Update_RequestDMO.UpdatedDate = DateTime.Now;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQREQ_ReqStatus = "In Progress";
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_MobileNo = MobileNo;
                    hR_Master_Employee_Update_RequestDMO.HRMEREQ_EmailId = emailid;
                    _portalContext.Add(hR_Master_Employee_Update_RequestDMO);
                    data.HRMEREQ_Id = hR_Master_Employee_Update_RequestDMO.HRMEREQ_Id;
                    SaveUpdateMobileNo(data);
                    SaveUpdateEmailId(data);
                    var i = _portalContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO SaveUpdateMobileNo(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                if (data.Temp_MobileNo.Length > 0)
                {
                    foreach (var d in data.Temp_MobileNo)
                    {
                        if (d.HRMEREQMN_Id > 0)
                        {
                            ids.Add(d.HRMEREQMN_Id);
                        }
                    }

                    Array getdetails = _portalContext.HR_Master_Employee_Update_Request_MobileNoDMO.Where(a => a.HRMEREQ_Id == data.HRMEREQ_Id
                    && !ids.Contains(a.HRMEREQMN_Id)).ToArray();

                    foreach (var d in getdetails)
                    {
                        _portalContext.Remove(d);
                    }

                    foreach (var c in data.Temp_MobileNo)
                    {
                        if (c.HRMEREQMN_Id > 0)
                        {
                            var result_mobile = _portalContext.HR_Master_Employee_Update_Request_MobileNoDMO.Single(a => a.HRMEREQMN_Id == c.HRMEREQMN_Id
                            && a.HRMEREQ_Id == data.HRMEREQ_Id);

                            result_mobile.HRMEREQMN_MobileNo = c.HRMEREQMN_MobileNo;
                            result_mobile.HRMEREQMN_Flag = c.HRMEREQMN_Flag;
                            _portalContext.Update(result_mobile);
                        }
                        else
                        {
                            HR_Master_Employee_Update_Request_MobileNoDMO MobileNoDMO = new HR_Master_Employee_Update_Request_MobileNoDMO();

                            MobileNoDMO.HRMEREQ_Id = data.HRMEREQ_Id;
                            MobileNoDMO.HRMEREQMN_MobileNo = c.HRMEREQMN_MobileNo;
                            MobileNoDMO.HRMEREQMN_Flag = c.HRMEREQMN_Flag;
                            MobileNoDMO.HRMEREQMN_ActiveFlg = true;
                            _portalContext.Add(MobileNoDMO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO SaveUpdateEmailId(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                if (data.Temp_EmailId.Length > 0)
                {
                    foreach (var d in data.Temp_EmailId)
                    {
                        if (d.HRMEREQEM_Id > 0)
                        {
                            ids.Add(d.HRMEREQEM_Id);
                        }
                    }

                    Array getdetails = _portalContext.HR_Master_Employee_Update_Request_EmailIdDMO.Where(a => a.HRMEREQ_Id == data.HRMEREQ_Id
                    && !ids.Contains(a.HRMEREQEM_Id)).ToArray();

                    foreach (var d in getdetails)
                    {
                        _portalContext.Remove(d);
                    }

                    foreach (var c in data.Temp_EmailId)
                    {
                        if (c.HRMEREQEM_Id > 0)
                        {
                            var result_emailid = _portalContext.HR_Master_Employee_Update_Request_EmailIdDMO.Single(a => a.HRMEREQEM_Id == c.HRMEREQEM_Id
                            && a.HRMEREQ_Id == data.HRMEREQ_Id);

                            result_emailid.HRMEREQEM_EmailId = c.HRMEREQEM_EmailId;
                            result_emailid.HRMEREQEM_Flag = c.HRMEREQEM_Flag;
                            _portalContext.Update(result_emailid);
                        }
                        else
                        {
                            HR_Master_Employee_Update_Request_EmailIdDMO EmailiD = new HR_Master_Employee_Update_Request_EmailIdDMO();

                            EmailiD.HRMEREQ_Id = data.HRMEREQ_Id;
                            EmailiD.HRMEREQEM_EmailId = c.HRMEREQEM_EmailId;
                            EmailiD.HRMEREQEM_Flag = c.HRMEREQEM_Flag;
                            EmailiD.HRMEREQEM_ActiveFlg = true;
                            _portalContext.Add(EmailiD);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //Approval Details
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdateapproval(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                data.GetRequestedData = (from a in _portalContext.HR_Master_Employee_Update_RequestDMO
                                         from b in _portalContext.HR_Master_Employee_DMO
                                         where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMEREQREQ_ReqStatus == "In Progress")
                                         select new EmployeeProfileUpdateApprovalDTO
                                         {
                                             employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName) +
                                            (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                            (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName) +
                                            (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " " + b.HRME_EmployeeCode)).Trim(),
                                             HRME_Id = b.HRME_Id,
                                             CreatedDate = a.CreatedDate,
                                         }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();



                data.GetRequestedDataList = (from a in _portalContext.HR_Master_Employee_Update_RequestDMO
                                             from b in _portalContext.HR_Master_Employee_DMO
                                             where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                             select new EmployeeProfileUpdateApprovalDTO
                                             {
                                                 employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName) +
                                                (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                                (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName) +
                                                (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " " + b.HRME_EmployeeCode)).Trim(),
                                                 HRME_Id = b.HRME_Id,
                                                 CreatedDate = a.CreatedDate,
                                                 HRMEREQREQ_ReqStatus=a.HRMEREQREQ_ReqStatus,
                                                 UpdatedDate=a.UpdatedDate,
                                                 username = a.HRMEREQREQ_ApprovedBy > 0 ? _portalContext.ApplicationUser.Where(c => c.Id == a.HRMEREQREQ_ApprovedBy).FirstOrDefault().UserName : "",
                                             }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO OnChangeOfEmployee(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                data.GetMartialStatusList = _portalContext.IVRM_Master_Marital_Status.Where(a => a.MI_Id == data.MI_Id && a.IVRMMMS_ActiveFlag == true).ToArray();

                data.GetGenderList = _portalContext.IVRM_Master_Gender.Where(a => a.MI_Id == data.MI_Id && a.IVRMMG_ActiveFlag == true).ToArray();

                data.GetCountryList = _portalContext.country.ToArray();

                data.GetReligionList = _portalContext.MasterReligionDMO.Where(a => a.Is_Active == true).ToArray();

                var CheckRequestedData = _portalContext.HR_Master_Employee_Update_RequestDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id
                 && a.HRMEREQREQ_ReqStatus == "In Progress").ToList();

                if (CheckRequestedData.Count > 0)
                {
                    data.HRMEREQ_Id = CheckRequestedData.FirstOrDefault().HRMEREQ_Id;

                    data.GetEmployeeDetails = CheckRequestedData.ToArray();

                    data.GetEmployeEmailIdDetails = _portalContext.HR_Master_Employee_Update_Request_EmailIdDMO.Where(a => a.HRMEREQ_Id == data.HRMEREQ_Id).ToArray();

                    data.GetEmployeMobileNoDetails = _portalContext.HR_Master_Employee_Update_Request_MobileNoDMO.Where(a => a.HRMEREQ_Id == data.HRMEREQ_Id).ToArray();

                    if (CheckRequestedData.FirstOrDefault().HRMEREQ_LocCountryId != null)
                    {
                        data.GetLocalStateList = _portalContext.state.Where(a => a.IVRMMC_Id == CheckRequestedData.FirstOrDefault().HRMEREQ_LocCountryId).ToArray();
                    }

                    if (CheckRequestedData.FirstOrDefault().HRMEREQ_PerCountryId != null)
                    {
                        data.GetPerStateList = _portalContext.state.Where(a => a.IVRMMC_Id == CheckRequestedData.FirstOrDefault().HRMEREQ_PerCountryId).ToArray();
                    }

                    if (CheckRequestedData.FirstOrDefault().ReligionId != null)
                    {
                        data.GetCasteCategoryList = (from a in _portalContext.ReligionCategory_MappingDMO
                                                     from b in _portalContext.CasteCategory
                                                     where (a.IVRMMR_Id == CheckRequestedData.FirstOrDefault().ReligionId && a.IMCC_Id == b.IMCC_Id
                                                     && a.IRCC_ActiveFlg == true)
                                                     select b).Distinct().ToArray();
                    }

                    if (CheckRequestedData.FirstOrDefault().CasteCategoryId != null)
                    {
                        var AllCastess = (from a in _portalContext.Caste
                                          from b in _portalContext.CasteCategory
                                          where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == data.MI_Id
                                          && a.IMCC_Id == CheckRequestedData.FirstOrDefault().CasteCategoryId)
                                          select a).ToArray();

                        data.GetCasteList = AllCastess.Distinct().ToArray();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileUpdateApprovalDTO SaveApprovedData(EmployeeProfileUpdateApprovalDTO data)
        {
            try
            {
                data.returnval = false;

                long? MobileNo = null;
                string emailid = "";

                if (data.Temp_MobileNo != null && data.Temp_MobileNo.Length > 0)
                {
                    foreach (var d in data.Temp_MobileNo)
                    {
                        if (d.HRMEREQMN_Flag == "default")
                        {
                            MobileNo = d.HRMEREQMN_MobileNo;
                        }
                    }
                }

                if (data.Temp_EmailId != null && data.Temp_EmailId.Length > 0)
                {
                    foreach (var d in data.Temp_EmailId)
                    {
                        if (d.HRMEREQEM_Flag == "default")
                        {
                            emailid = d.HRMEREQEM_EmailId;
                        }
                    }
                }

                if (data.HRMEREQ_Id > 0)
                {
                    var result = _portalContext.HR_Master_Employee_Update_RequestDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMEREQ_Id == data.HRMEREQ_Id);

                    result.HRMEREQREQ_ApprovedBy = data.UserId;
                    result.HRMEREQREQ_ApprovedFlg = true;
                    result.HRMEREQREQ_ReqStatus = "Approved";
                    result.HRMEREQREQ_ChangeConfirmFlg = "Approved";
                    result.HRMEREQREQ_ConformBy = data.UserId;
                    result.HRMEREQREQ_ConformFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _portalContext.Update(result);

                    var resulthrmeid = _portalContext.HR_Master_Employee_DMO.Single(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id);
                    resulthrmeid.HRME_EmployeeFirstName = data.HRMEREQ_EmployeeFirstName;
                    resulthrmeid.HRME_EmployeeMiddleName = data.HRMEREQ_EmployeeMiddleName == null ? "" : data.HRMEREQ_EmployeeMiddleName;
                    resulthrmeid.HRME_EmployeeLastName = data.HRMEREQ_EmployeeLastName == null ? "" : data.HRMEREQ_EmployeeLastName;
                    resulthrmeid.HRME_PerStreet = data.HRMEREQ_PerStreet;
                    resulthrmeid.HRME_PerArea = data.HRMEREQ_PerArea;
                    resulthrmeid.HRME_PerCity = data.HRMEREQ_PerCity;
                    resulthrmeid.HRME_PerStateId = data.HRMEREQ_PerStateId;
                    resulthrmeid.HRME_PerCountryId = data.HRMEREQ_PerCountryId;
                    resulthrmeid.HRME_PerPincode = data.HRMEREQ_PerPincode;

                    resulthrmeid.HRME_LocStreet = data.HRMEREQ_LocStreet;
                    resulthrmeid.HRME_LocArea = data.HRMEREQ_LocArea;
                    resulthrmeid.HRME_LocCity = data.HRMEREQ_LocCity;
                    resulthrmeid.HRME_LocStateId = data.HRMEREQ_LocStateId;
                    resulthrmeid.HRME_LocCountryId = data.HRMEREQ_LocCountryId;
                    resulthrmeid.HRME_LocPincode = data.HRMEREQ_LocPincode;

                    resulthrmeid.IVRMMMS_Id = data.IVRMMMS_Id;
                    resulthrmeid.IVRMMG_Id = data.IVRMMG_Id;
                    resulthrmeid.CasteCategoryId = data.CasteCategoryId;
                    resulthrmeid.CasteId = data.CasteId;
                    resulthrmeid.ReligionId = data.ReligionId;

                    resulthrmeid.HRME_FatherName = data.HRMEREQ_FatherName;
                    resulthrmeid.HRME_MotherName = data.HRMEREQ_MotherName;
                    resulthrmeid.HRME_SpouseName = data.HRMEREQ_SpouseName;
                    resulthrmeid.HRME_SpouseOccupation = data.HRMEREQ_SpouseOccupation;
                    resulthrmeid.HRME_SpouseMobileNo = data.HRMEREQ_SpouseMobileNo;
                    resulthrmeid.HRME_SpouseEmailId = data.HRMEREQ_SpouseEmailId;
                    resulthrmeid.HRME_SpouseAddress = data.HRMEREQ_SpouseAddress;
                    resulthrmeid.HRME_DOB = data.HRMEREQ_DOB;
                    resulthrmeid.HRME_BloodGroup = data.HRMEREQ_BloodGroup;
                    resulthrmeid.HRME_Photo = data.HRMEREQ_Photo;
                    resulthrmeid.HRME_MobileNo = MobileNo;
                    resulthrmeid.HRME_EmailId = emailid;
                    _portalContext.Update(result);


                    Array GetEmployeeMobileNos = _portalContext.Multiple_Mobile_DMO.Where(a => a.HRME_Id == data.HRME_Id).ToArray();
                    if (GetEmployeeMobileNos.Length > 0)
                    {
                        foreach (var d in GetEmployeeMobileNos)
                        {
                            _portalContext.Remove(d);
                        }
                    }

                    foreach (var c in data.Temp_MobileNo)
                    {
                        Multiple_Mobile_DMO multiple_Mobile_DMO = new Multiple_Mobile_DMO();
                        multiple_Mobile_DMO.HRME_Id = data.HRME_Id;
                        multiple_Mobile_DMO.HRMEMNO_MobileNo = Convert.ToInt64(c.HRMEREQMN_MobileNo);
                        multiple_Mobile_DMO.HRMEMNO_DeFaultFlag = c.HRMEREQMN_Flag;
                        multiple_Mobile_DMO.CreatedDate = DateTime.Now;
                        multiple_Mobile_DMO.UpdatedDate = DateTime.Now;
                        _portalContext.Add(multiple_Mobile_DMO);
                    }

                    Array GetEmployeeEmailIds = _portalContext.Multiple_Email_DMO.Where(a => a.HRME_Id == data.HRME_Id).ToArray();
                    if (GetEmployeeEmailIds.Length > 0)
                    {
                        foreach (var d in GetEmployeeEmailIds)
                        {
                            _portalContext.Remove(d);
                        }
                    }

                    foreach (var c in data.Temp_EmailId)
                    {
                        Multiple_Email_DMO multiple_Email_DMO = new Multiple_Email_DMO();
                        multiple_Email_DMO.HRME_Id = data.HRME_Id;
                        multiple_Email_DMO.HRMEM_EmailId = c.HRMEREQEM_EmailId;
                        multiple_Email_DMO.HRMEM_DeFaultFlag = c.HRMEREQEM_Flag;
                        multiple_Email_DMO.CreatedDate = DateTime.Now;
                        multiple_Email_DMO.UpdatedDate = DateTime.Now;
                        _portalContext.Add(multiple_Email_DMO);
                    }

                    var GetUserId = _portalContext.Staff_User_Login.Where(a => a.Emp_Code == data.HRME_Id).ToList();

                    if (GetUserId.Count > 0)
                    {
                        var Applicationuser_result = _portalContext.ApplicationUser.Single(a => a.Id == GetUserId.FirstOrDefault().Id);
                        Applicationuser_result.UserImagePath = data.HRMEREQ_Photo;
                        Applicationuser_result.PhoneNumber = MobileNo.ToString();
                        Applicationuser_result.Email = emailid;
                        Applicationuser_result.NormalizedEmail = emailid;
                        _portalContext.Update(Applicationuser_result);
                    }

                    var i = _portalContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}