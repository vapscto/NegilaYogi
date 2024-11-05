using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class HealthCardDetailsIMPL : Interfaces.HealthCardDetailsInterface
    {
        public HRMSContext _hRMSContext;
        public HealthCardDetailsIMPL(HRMSContext _hRMS)
        {
            _hRMSContext = _hRMS;
        }
        public HealthCardDetailsDTO loaddata(HealthCardDetailsDTO data)
        {
            try
            {
                data.masterdetails = _hRMSContext.HM_T_PolicyDetailsDMO.Where(P => P.HMTPD_ActiveFlag == true).Distinct().ToArray();
                data.policydeatail = _hRMSContext.HM_T_ReimbursementClaim_DetailsDMO.Where(R => R.HMTRSCD_ActiveFLag == true).Distinct().ToArray();

                data.getemployeelist = (from a in _hRMSContext.MasterEmployee
                                        from b in _hRMSContext.HM_T_PolicyDetailsDMO
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id && b.HMTPD_ActiveFlag == true)
                                        select new HealthCardDetailsDTO
                                        {
                                            HRME_Id = a.HRME_Id,
                                            HMTPD_Id = b.HMTPD_Id,
                                            HMTPD_MemberId = b.HMTPD_MemberId,
                                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                           || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                             (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                             (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                             (b.HMTPD_MemberId == null || b.HMTPD_MemberId == "" ? "" : " : " + b.HMTPD_MemberId)).Trim(),
                                        }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();
                data.getemployeelistttt = (from a in _hRMSContext.MasterEmployee

                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                           select new HealthCardDetailsDTO
                                           {
                                               HRME_Id = a.HRME_Id,


                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                              || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                                (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                                (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                                (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),
                                           }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();

                data.getreport = (from a in _hRMSContext.MasterEmployee
                                  from b in _hRMSContext.HM_T_PolicyDetailsDMO
                                  where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id
                                  && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                  )
                                  select new HealthCardMasterDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                            || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                              (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                              (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                      HMTPD_PolicyName = b.HMTPD_PolicyName,
                                      HMTPD_PlanStartDate = b.HMTPD_PlanStartDate,
                                      HMTPD_PlanEndDate = b.HMTPD_PlanEndDate,
                                      HMTPD_PlanName = b.HMTPD_PlanName,
                                      HMTPD_PolicyProvider = b.HMTPD_PolicyProvider,
                                      HMTPD_Id = b.HMTPD_Id,
                                      HMTPD_ActiveFlag = b.HMTPD_ActiveFlag,
                                      HMTPD_MemberId = b.HMTPD_MemberId
                                  }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();
                data.getsavedetails = (from a in _hRMSContext.MasterEmployee
                                       from b in _hRMSContext.HM_T_PolicyDetailsDMO
                                       from c in _hRMSContext.HM_T_ReimbursementClaim_DetailsDMO
                                       where (a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id
                                       && b.HMTPD_Id == c.HMTPD_Id)
                                       select new HealthCardDetailsDTO
                                       {
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                            || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                              (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                              (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                           HMTPD_MemberId = b.HMTPD_MemberId,
                                           HMTRSCD_SumOfInsuredAmt = c.HMTRSCD_SumOfInsuredAmt,
                                           HMTRSCD_CompanyName = c.HMTRSCD_CompanyName,
                                           HMTRSCD_Patientname = c.HMTRSCD_Patientname,
                                           HMTRSCD_NameofHospital = c.HMTRSCD_NameofHospital,
                                           HMTPD_Id = c.HMTPD_Id,
                                           HMTRSCD_ActiveFLag = c.HMTRSCD_ActiveFLag,
                                           HMTRSCD_Id=c.HMTRSCD_Id,
                                           HMTRSCD_Occupation=c.HMTRSCD_Occupation,
                                        HMTRSCD_hospitalizationexpenses=c.HMTRSCD_hospitalizationexpenses,
                                           HMTRSCD_Address=c.HMTRSCD_Address

                                       }
                                     ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //OnChangeEmployee
        public HealthCardDetailsDTO OnChangeEmployee(HealthCardDetailsDTO data)
        {
            try
            {
                data.getemployeedetails = (from a in _hRMSContext.MasterEmployee
                                           from b in _hRMSContext.HR_Master_Department
                                           from c in _hRMSContext.HR_Master_Designation
                                           where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                           && a.HRME_LeftFlag == false && a.HRME_Id == data.HRME_Id)
                                           select new HealthCardDetailsDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " +
                                                 (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " +
                                                 (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                                           }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //editmaster
        public HealthCardMasterDTO editmaster(HealthCardMasterDTO data)
        {
            try
            {
                data.editarray = (from a in _hRMSContext.MasterEmployee
                                  from b in _hRMSContext.HM_T_PolicyDetailsDMO
                                  from c in _hRMSContext.HM_T_ReimbursementClaim_DetailsDMO
                                  where (a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id
                                  && b.HMTPD_Id == c.HMTPD_Id && c.HMTRSCD_ActiveFLag == true && c.HMTRSCD_Id == data.HMTRSCD_Id)
                                  select new HealthCardDetailsDTO
                                  {
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null
                                           || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) + " " +
                                             (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) + " " +
                                             (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                             (b.HMTPD_MemberId == null || b.HMTPD_MemberId == "" ? "" : " : " + b.HMTPD_MemberId)).Trim(),
                                      HMTPD_MemberId = b.HMTPD_MemberId,
                                      HMTRSCD_MemberId=c.HMTRSCD_MemberId,
                                      HMTRSCD_CompanyName = c.HMTRSCD_CompanyName,
                                      HMTRSCD_DOB = c.HMTRSCD_DOB,
                                      HMTRSCD_Patientname = c.HMTRSCD_Patientname,
                                      HMTRSCD_Gender = c.HMTRSCD_Gender,
                                      HMTRSCD_PatientPhNo = c.HMTRSCD_PatientPhNo,
                                      HMTRSCD_DateOfTreatment = c.HMTRSCD_DateOfTreatment,
                                      HMTRSCD_DateOfAdmission = c.HMTRSCD_DateOfAdmission,
                                      HMTRSCD_DateOfDischarge = c.HMTRSCD_DateOfDischarge,
                                      HMTRSCD_Symptomspresented = c.HMTRSCD_Symptomspresented,
                                      HMTRSCD_RlptoPrimaryinsured = c.HMTRSCD_RlptoPrimaryinsured,
                                      HMTRSCD_Occupation = c.HMTRSCD_Occupation,
                                      HMTRSCD_NameofHospital = c.HMTRSCD_NameofHospital,
                                      HMTRSCD_RoomCategory = c.HMTRSCD_RoomCategory,
                                      HMTRSCD_hospitalizationexpenses = c.HMTRSCD_hospitalizationexpenses,
                                      HMTRSCD_Address = c.HMTRSCD_Address,
                                      HMTRSCD_Pincode = c.HMTRSCD_Pincode,
                                      HMTRSCD_EmailId = c.HMTRSCD_EmailId,
                                      HMTRSCD_ClaimDocFilePath = c.HMTRSCD_ClaimDocFilePath,
                                      HMTRSCD_CurrentlyCoveredInsuranceFlag = c.HMTRSCD_CurrentlyCoveredInsuranceFlag,
                                      HMTRSCD_RClaimNo = c.HMTRSCD_RClaimNo,
                                      HMTRSCD_SumOfInsuredAmt = c.HMTRSCD_SumOfInsuredAmt,
                                      HMTRSCD_RCCompanyName = c.HMTRSCD_RCCompanyName,
                                      HMTRSCD_Diagnosis = c.HMTRSCD_Diagnosis,
                                      HMTRSCD_Id=c.HMTRSCD_Id,
                                      HRME_Id=b.HRME_Id,
                                      HMTPD_Id=b.HMTPD_Id,
                                  }
                                     ).Distinct().ToArray();

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //deactiveM
        public HealthCardMasterDTO deactiveM(HealthCardMasterDTO data)
        {
            try
            {
                if (data.HMTPD_Id > 0)
                {
                    var resultone = _hRMSContext.HM_T_PolicyDetailsDMO.Where(t => t.HMTPD_Id == data.HMTPD_Id && t.MI_Id == data.MI_Id).FirstOrDefault();
                    if (resultone.HMTPD_ActiveFlag == true)
                    {
                        resultone.HMTPD_ActiveFlag = false;
                    }
                    else
                    {
                        resultone.HMTPD_ActiveFlag = true;
                    }
                    resultone.HMTPD_UpdatedDate = DateTime.Now;
                    _hRMSContext.Update(resultone);
                    var i = _hRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.return_val = "delete";
                    }
                    else
                    {
                        data.return_val = "delete";
                    }
                }
                if (data.HMTRSCD_Id > 0)
                {
                    var resultwo = _hRMSContext.HM_T_ReimbursementClaim_DetailsDMO.Where(t => t.HMTRSCD_Id == data.HMTRSCD_Id).FirstOrDefault();
                    if (resultwo.HMTRSCD_ActiveFLag == true)
                    {
                        resultwo.HMTRSCD_ActiveFLag = false;
                    }
                    else
                    {
                        resultwo.HMTRSCD_ActiveFLag = true;
                    }
                    resultwo.HMTRSCD_UpdatedDate = DateTime.Now;

                    _hRMSContext.Update(resultwo);
                    var i = _hRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.return_val = "delete";
                    }
                    else
                    {
                        data.return_val = "delete";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //Savemaster
        public HealthCardMasterDTO Savemaster(HealthCardMasterDTO data)
        {
            try
            {
                var result = _hRMSContext.HM_T_PolicyDetailsDMO.Where(t => t.HMTPD_MemberId == data.HMTPD_MemberId && t.MI_Id == data.MI_Id && t.HMTPD_PolicyName == data.HMTPD_PolicyName && t.HMTPD_PlanName == data.HMTPD_PlanName && t.HMTPD_PolicyProvider == data.HMTPD_PolicyProvider && t.HMTPD_PlanStartDate == data.HMTPD_PlanStartDate && t.HMTPD_PlanEndDate == data.HMTPD_PlanEndDate && t.HRME_Id == data.HRME_Id).ToList();
                var resultone = _hRMSContext.HM_T_PolicyDetailsDMO.Where(t => t.HMTPD_Id == data.HMTPD_Id && t.MI_Id == data.MI_Id && t.HMTPD_ActiveFlag == true).FirstOrDefault();
                if (result.Count > 0)
                {
                    data.return_val = "RecordExist";
                }
                else
                {
                    if (data.HMTPD_Id > 0 && resultone.HMTPD_Id > 0)
                    {
                        resultone.HMTPD_MemberId = data.HMTPD_MemberId;
                        resultone.HMTPD_PlanStartDate = data.HMTPD_PlanStartDate;
                        resultone.HMTPD_PlanEndDate = data.HMTPD_PlanEndDate;
                        resultone.HMTPD_PolicyName = data.HMTPD_PolicyName;
                        resultone.HMTPD_PlanName = data.HMTPD_PlanName;
                        resultone.HMTPD_PolicyProvider = data.HMTPD_PolicyProvider;
                        resultone.HMTPD_UpdatedDate = DateTime.Now;
                        _hRMSContext.Update(resultone);
                        var i = _hRMSContext.SaveChanges();
                        if (i > 0)
                        {
                            data.return_val = "Update";
                        }
                        else
                        {
                            data.return_val = "Notupdate";
                        }

                    }
                    else
                    {
                        HM_T_PolicyDetailsDMO obj = new HM_T_PolicyDetailsDMO();
                        obj.HMTPD_Id = data.HMTPD_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.HMTPD_MemberId = data.HMTPD_MemberId;
                        obj.HMTPD_PlanStartDate = data.HMTPD_PlanStartDate;
                        obj.HMTPD_PlanEndDate = data.HMTPD_PlanEndDate;
                        obj.HMTPD_PolicyName = data.HMTPD_PolicyName;
                        obj.HMTPD_PlanName = data.HMTPD_PlanName;
                        obj.HMTPD_PolicyProvider = data.HMTPD_PolicyProvider;
                        obj.HMTPD_ActiveFlag = true;
                        obj.HMTPD_CreatedDate = DateTime.Now;
                        obj.HMTPD_UpdatedDate = DateTime.Now;
                        _hRMSContext.Add(obj);
                        var i = _hRMSContext.SaveChanges();
                        if (i > 0)
                        {
                            data.return_val = "save";
                        }
                        else
                        {
                            data.return_val = "Nosave";
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
        public HealthCardDetailsDTO SaveDetails(HealthCardDetailsDTO data)
        {
            try
            {
               
                if (data.HMTRSCD_RlptoPrimaryinsured == "Self")
                {
                    data.HMTRSCD_Self = true;
                }
                else
                {
                    data.HMTRSCD_Self = false;
                }
                if (data.HMTRSCD_RlptoPrimaryinsured == "Spouse")
                {
                    data.HMTRSCD_Spouse = true;
                }
                else
                {
                    data.HMTRSCD_Spouse = false;
                }
                if (data.HMTRSCD_RlptoPrimaryinsured == "Child")
                {
                    data.HMTRSCD_Child = true;
                }
                else
                {
                    data.HMTRSCD_Child = false;
                }
                if (data.HMTRSCD_RlptoPrimaryinsured == "Mother")
                {
                    data.HMTRSCD_Mother = true;
                }
                else
                {
                    data.HMTRSCD_Mother = false;
                }
                if (data.HMTRSCD_RlptoPrimaryinsured == "Father")
                {
                    data.HMTRSCD_Father = true;
                }
                else
                {
                    data.HMTRSCD_Father = false;
                }
                if (data.HMTRSCD_RlptoPrimaryinsured == "Other")
                {
                    data.HMTRSCD_Other = true;
                }
                else
                {
                    data.HMTRSCD_Other = false;
                }
                var resultone = _hRMSContext.HM_T_ReimbursementClaim_DetailsDMO.Where(t => t.HMTRSCD_Id == data.HMTRSCD_Id && t.HMTRSCD_ActiveFLag == true).FirstOrDefault();
                if (data.HMTRSCD_Id > 0)
                {
                    resultone.HMTRSCD_MemberId = data.HMTRSCD_MemberId;
                    resultone.HMTRSCD_CompanyName = data.HMTRSCD_CompanyName;
                    resultone.HMTRSCD_DOB = data.HMTRSCD_DOB;
                    resultone.HMTRSCD_Patientname = data.HMTRSCD_Patientname;
                    resultone.HMTRSCD_Gender = data.HMTRSCD_Gender;
                    resultone.HMTRSCD_PatientPhNo = data.HMTRSCD_PatientPhNo;
                    resultone.HMTRSCD_DateOfTreatment = data.HMTRSCD_DateOfTreatment;
                    resultone.HMTRSCD_DateOfAdmission = data.HMTRSCD_DateOfAdmission;
                    resultone.HMTRSCD_DateOfDischarge = data.HMTRSCD_DateOfDischarge;
                    resultone.HMTRSCD_Symptomspresented = data.HMTRSCD_Symptomspresented;
                    resultone.HMTRSCD_RlptoPrimaryinsured = data.HMTRSCD_RlptoPrimaryinsured;
                    resultone.HMTRSCD_Self = data.HMTRSCD_Self;
                    resultone.HMTRSCD_Spouse = data.HMTRSCD_Spouse;
                    resultone.HMTRSCD_Child = data.HMTRSCD_Child;
                    resultone.HMTRSCD_Father = data.HMTRSCD_Father;
                    resultone.HMTRSCD_Mother = data.HMTRSCD_Mother;
                    resultone.HMTRSCD_Other = data.HMTRSCD_Other;
                    resultone.HMTRSCD_Occupation = data.HMTRSCD_Occupation;
                    resultone.HMTRSCD_NameofHospital = data.HMTRSCD_NameofHospital;
                    resultone.HMTRSCD_RoomCategory = data.HMTRSCD_RoomCategory;
                    resultone.HMTRSCD_hospitalizationexpenses = data.HMTRSCD_hospitalizationexpenses;
                    resultone.HMTRSCD_Address = data.HMTRSCD_Address;
                    resultone.HMTRSCD_Pincode = data.HMTRSCD_Pincode;
                    resultone.HMTRSCD_EmailId = data.HMTRSCD_EmailId;
                    resultone.HMTRSCD_ClaimDocFilePath = data.HMTRSCD_ClaimDocFilePath;
                    resultone.HMTRSCD_CurrentlyCoveredInsuranceFlag = data.HMTRSCD_CurrentlyCoveredInsuranceFlag;
                    resultone.HMTRSCD_RClaimNo = data.HMTRSCD_RClaimNo;
                    resultone.HMTRSCD_SumOfInsuredAmt = data.HMTRSCD_SumOfInsuredAmt;
                    resultone.HMTRSCD_RCCompanyName = data.HMTRSCD_RCCompanyName;
                    resultone.HMTRSCD_Diagnosis = data.HMTRSCD_Diagnosis;
                    resultone.HMTRSCD_UpdatedDate = DateTime.Now;
                    _hRMSContext.Update(resultone);
                    var i = _hRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.return_val = "Update";
                    }
                    else
                    {
                        data.return_val = "Notupdate";
                    }
                }
                else
                {
                    HM_T_ReimbursementClaim_DetailsDMO obj = new HM_T_ReimbursementClaim_DetailsDMO();
                    obj.HMTRSCD_Id = data.HMTRSCD_Id;
                    obj.HMTPD_Id = data.HMTPD_Id;
                    obj.HMTRSCD_MemberId = data.HMTRSCD_MemberId;
                    obj.HMTRSCD_CompanyName = data.HMTRSCD_CompanyName;
                    obj.HMTRSCD_DOB = data.HMTRSCD_DOB;
                    obj.HMTRSCD_Patientname = data.HMTRSCD_Patientname;
                    obj.HMTRSCD_Gender = data.HMTRSCD_Gender;
                    obj.HMTRSCD_PatientPhNo = data.HMTRSCD_PatientPhNo;
                    obj.HMTRSCD_DateOfTreatment = data.HMTRSCD_DateOfTreatment;
                    obj.HMTRSCD_DateOfAdmission = data.HMTRSCD_DateOfAdmission;
                    obj.HMTRSCD_DateOfDischarge = data.HMTRSCD_DateOfDischarge;
                    obj.HMTRSCD_Symptomspresented = data.HMTRSCD_Symptomspresented;
                    obj.HMTRSCD_RlptoPrimaryinsured = data.HMTRSCD_RlptoPrimaryinsured;
                    obj.HMTRSCD_Self = data.HMTRSCD_Self;
                    obj.HMTRSCD_Spouse = data.HMTRSCD_Spouse;
                    obj.HMTRSCD_Child = data.HMTRSCD_Child;
                    obj.HMTRSCD_Father = data.HMTRSCD_Father;
                    obj.HMTRSCD_Mother = data.HMTRSCD_Mother;
                    obj.HMTRSCD_Other = data.HMTRSCD_Other;
                    obj.HMTRSCD_Occupation = data.HMTRSCD_Occupation;
                    obj.HMTRSCD_NameofHospital = data.HMTRSCD_NameofHospital;
                    obj.HMTRSCD_RoomCategory = data.HMTRSCD_RoomCategory;
                    obj.HMTRSCD_hospitalizationexpenses = data.HMTRSCD_hospitalizationexpenses;
                    obj.HMTRSCD_Address = data.HMTRSCD_Address;
                    obj.HMTRSCD_Pincode = data.HMTRSCD_Pincode;
                    obj.HMTRSCD_EmailId = data.HMTRSCD_EmailId;
                    obj.HMTRSCD_ClaimDocFilePath = data.HMTRSCD_ClaimDocFilePath;
                    obj.HMTRSCD_CurrentlyCoveredInsuranceFlag = data.HMTRSCD_CurrentlyCoveredInsuranceFlag;
                    obj.HMTRSCD_RClaimNo = data.HMTRSCD_RClaimNo;
                    obj.HMTRSCD_SumOfInsuredAmt = data.HMTRSCD_SumOfInsuredAmt;
                    obj.HMTRSCD_RCCompanyName = data.HMTRSCD_RCCompanyName;
                    obj.HMTRSCD_Diagnosis = data.HMTRSCD_Diagnosis;
                    obj.HMTRSCD_ActiveFLag = true;
                    obj.HMTRSCD_CreatedDate = DateTime.Now;
                    obj.HMTRSCD_UpdatedDate = DateTime.Now;
                    _hRMSContext.Add(obj);
                    var i = _hRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.return_val = "save";
                    }
                    else
                    {
                        data.return_val = "Nosave";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
