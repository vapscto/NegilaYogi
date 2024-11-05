using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class AppointmentService : Interfaces.AppointmentInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AppointmentService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto)
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

        public HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Candidate_DetailsDMO dmoObj = Mapper.Map<HR_Candidate_DetailsDMO>(dto);
                HR_MRF_ListDMO mrfListObj = new HR_MRF_ListDMO();

                var duplicatecountresult = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMPT_Id == dto.HRMPT_Id && t.HRMC_Id == dto.HRMC_Id && t.HRCD_MRFNO == dto.HRCD_MRFNO && t.HRCD_FirstName == dto.HRCD_FirstName && t.HRCD_MiddleName == dto.HRCD_MiddleName && t.HRCD_LastName == dto.HRCD_LastName && t.HRMJ_Id == dto.HRMJ_Id && t.HRCD_Skills == dto.HRCD_Skills && t.HRCD_DOB == dto.HRCD_DOB && t.IVRMMG_Id == dto.IVRMMG_Id && t.HRCD_MobileNo == dto.HRCD_MobileNo && t.HRCD_EmailId == dto.HRCD_EmailId && t.HRCD_ExpFrom == dto.HRCD_ExpFrom && t.HRCD_ExpTo == dto.HRCD_ExpTo && t.HRCD_CurrentCompany == dto.HRCD_CurrentCompany && t.HRCD_ResumeSource == dto.HRCD_ResumeSource && t.HRCD_JobPortalName == dto.HRCD_JobPortalName && t.HRCD_RefCode == dto.HRCD_RefCode && t.HRCD_LastCTC == dto.HRCD_LastCTC && t.HRCD_ExpectedCTC == dto.HRCD_ExpectedCTC && t.HRCD_AppDate == dto.HRCD_AppDate && t.HRCD_InterviewDate == dto.HRCD_InterviewDate && t.HRCD_NoticePeriod == dto.HRCD_NoticePeriod && t.HRCD_Remarks == dto.HRCD_Remarks).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRCD_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dmoObj.HRCD_Id);
                        dto.UpdatedDate = DateTime.Now;

                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
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
                        dmoObj.HRCD_ActiveFlg = true;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMPT_Id = dto.HRMPT_Id;
                        dmoObj.HRMC_Id = dto.HRMC_Id;
                        dmoObj.HRCD_MRFNO = dto.HRCD_MRFNO;
                        dmoObj.HRCD_FirstName = dto.HRCD_FirstName;
                        dmoObj.HRCD_MiddleName = dto.HRCD_MiddleName;
                        dmoObj.HRCD_LastName = dto.HRCD_LastName;
                        dmoObj.HRMJ_Id = dto.HRMJ_Id;
                        dmoObj.HRCD_Skills = dto.HRCD_Skills;
                        dmoObj.HRCD_DOB = dto.HRCD_DOB;
                        dmoObj.IVRMMG_Id = dto.IVRMMG_Id;
                        dmoObj.HRCD_MobileNo = dto.HRCD_MobileNo;
                        dmoObj.HRCD_EmailId = dto.HRCD_EmailId;
                        dmoObj.HRCD_ExpFrom = dto.HRCD_ExpFrom;
                        dmoObj.HRCD_ExpTo = dto.HRCD_ExpTo;
                        dmoObj.HRCD_CurrentCompany = dto.HRCD_CurrentCompany;
                        dmoObj.HRCD_ResumeSource = dto.HRCD_ResumeSource;
                        dmoObj.HRCD_JobPortalName = dto.HRCD_JobPortalName;
                        dmoObj.HRCD_RefCode = dto.HRCD_RefCode;
                        dmoObj.HRCD_LastCTC = dto.HRCD_LastCTC;
                        dmoObj.HRCD_ExpectedCTC = dto.HRCD_ExpectedCTC;
                        dmoObj.HRCD_AppDate = dto.HRCD_AppDate;
                        dmoObj.HRCD_InterviewDate = dto.HRCD_InterviewDate;
                        dmoObj.HRCD_NoticePeriod = dto.HRCD_NoticePeriod;
                        dmoObj.HRCD_Remarks = dto.HRCD_Remarks;
                        dmoObj.HRCD_Resume = dto.HRCD_Resume;
                        dmoObj.HRCD_RecruitmentStatus = dto.HRCD_RecruitmentStatus;
                        dmoObj.HRCD_CreatedBy = dto.HRCD_CreatedBy;
                        dmoObj.HRCD_UpdatedBy = dto.HRCD_UpdatedBy;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
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
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
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

        public HR_Candidate_DetailsDTO editData(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSCandidateList = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                        from b in _VMSContext.HR_Master_PostionTypeDMO
                                        from c in _VMSContext.HR_Master_Course
                                        from d  in _VMSContext.IVRM_Master_Gender                                  
                                  where (a.HRCD_Id.Equals(id) && a.HRMPT_Id == b.HRMPT_Id && a.HRMC_Id == c.HRMC_Id && a.IVRMMG_Id == d.IVRMMG_Id)
                                  select new HR_Candidate_DetailsDTO
                                  {
                                      HRCD_Id = a.HRCD_Id,
                                      HRMPT_Id = a.HRMPT_Id,
                                      HRMPT_Name = b.HRMPT_Name,
                                      HRMC_Id = a.HRMC_Id,
                                      HRMC_QulaificationName = c.HRMC_QulaificationName,
                                      HRCD_MRFNO = a.HRCD_MRFNO,
                                      HRCD_FirstName = a.HRCD_FirstName,
                                      HRCD_MiddleName = (a.HRCD_MiddleName == null ? "":a.HRCD_MiddleName),
                                      HRCD_LastName = (a.HRCD_LastName == null? "":a.HRCD_LastName),
                                      HRMJ_Id = a.HRMJ_Id,
                                      HRCD_Skills = a.HRCD_Skills,
                                      HRCD_DOB = a.HRCD_DOB,
                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = d.IVRMMG_GenderName,
                                      HRCD_MobileNo = a.HRCD_MobileNo,
                                      HRCD_EmailId = a.HRCD_EmailId,
                                      HRCD_ExpFrom = a.HRCD_ExpFrom,
                                      HRCD_ExpTo = a.HRCD_ExpTo,
                                      HRCD_CurrentCompany = a.HRCD_CurrentCompany,
                                      HRCD_ResumeSource = a.HRCD_ResumeSource,
                                      HRCD_JobPortalName = a.HRCD_JobPortalName,
                                      HRCD_RefCode = a.HRCD_RefCode,
                                      HRCD_LastCTC = a.HRCD_LastCTC,
                                      HRCD_ExpectedCTC = a.HRCD_ExpectedCTC,
                                      HRCD_AppDate = a.HRCD_AppDate,
                                      HRCD_InterviewDate = a.HRCD_InterviewDate,
                                      HRCD_NoticePeriod = a.HRCD_NoticePeriod,
                                      HRCD_Remarks = a.HRCD_Remarks,
                                      HRCD_Resume = a.HRCD_Resume,
                                      HRCD_RecruitmentStatus = a.HRCD_RecruitmentStatus,
                                      CreatedDate = a.CreatedDate,
                                      HRCD_CreatedBy = a.HRCD_CreatedBy
                                  }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO deactivate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRCD_Id > 0)
                {
                    var result = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == dto.HRCD_Id);

                    if (result.HRCD_ActiveFlg == true)
                    {
                        result.HRCD_ActiveFlg = false;
                    }
                    else if (result.HRCD_ActiveFlg == false)
                    {
                        result.HRCD_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRCD_ActiveFlg == true)
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

        public HR_Candidate_DetailsDTO GetAllDropdownAndDatatableDetails(HR_Candidate_DetailsDTO dto)
        {
            List<HR_Candidate_DetailsDMO> datalist = new List<HR_Candidate_DetailsDMO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistArrear = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistGross = new List<HR_Master_EarningsDeductionsDTO>();
            try
            {
                dto.MasterGender = (from emp in _VMSContext.IVRM_Master_Gender
                                    where emp.IVRMMG_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_Candidate_DetailsDTO
                                    {
                                        IVRMMG_Id = emp.IVRMMG_Id,
                                        IVRMMG_GenderName = emp.IVRMMG_GenderName
                                    }).ToArray();

                //dto.departmenlist = (from a in _Context.HR_Master_Department
                //                     where a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true
                //                     select new HR_Candidate_DetailsDTO
                //                     {
                //                         HRMD_Id =a.HRMD_Id,
                //                         HRMD_DepartmentName = a.HRMD_DepartmentName,
                //                     }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

                dto.candidatelist = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     where (a.MI_Id == dto.MI_Id && a.HRCD_ActiveFlg == true && a.HRCD_RecruitmentStatus == "Selected")
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRCD_Id=a.HRCD_Id,
                                         HRCD_FirstName = ((a.HRCD_FirstName == null ? " " : a.HRCD_FirstName) + " " + (a.HRCD_MiddleName == null ? " " : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? " " : a.HRCD_LastName)).Trim(),
                                     }).Distinct().OrderBy(t => t.HRCD_Id).ToArray();

                dto.companylist = _VMSContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
                dto.MasterQualification = _VMSContext.HR_Master_Course.Where(t => t.MI_Id == dto.MI_Id && t.HRMC_ActiveFlag == true).ToArray();
                dto.masterCaste = _VMSContext.mastercasteDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.mastermaritalstatus = _VMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMMS_ActiveFlag == true).ToArray();

                dto.earingdeductionlist = (from a in _VMSContext.HR_Master_EarningsDeductions
                                           where (a.MI_Id == dto.MI_Id && a.HRMED_ActiveFlag == true)
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMED_Name=a.HRMED_Name,
                                               HRMED_AmountPercentFlag = a.HRMED_AmountPercentFlag,
                                           }).Distinct().ToArray();

                dto.desgnationlist = (from c in _Context.HR_Master_Designation
                                       where (c.HRMDES_ActiveFlag == true && c.MI_Id == dto.MI_Id)
                                       select new HR_Candidate_DetailsDTO
                                       {
                                           HRMDES_Id = c.HRMDES_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                       }).Distinct().OrderBy(t => t.HRMDES_Order).ToArray();

                dto.departmentlist = (from c in _Context.HR_Master_Department
                                      where (c.HRMD_ActiveFlag == true && c.MI_Id == dto.MI_Id)
                                      select new HR_Candidate_DetailsDTO
                                      {
                                          HRMD_Id = c.HRMD_Id,
                                          HRMD_DepartmentName = c.HRMD_DepartmentName
                                      }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

                List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
                //Earning list
                earningdatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning") && t.HRMED_ActiveFlag == true).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                dto.earningList = DTOdatalistEarning.ToArray();

                List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();
                //Deduction List
                deductiondatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).ToList();

                if (deductiondatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistDeduction.Add(phdto);

                    }

                }

                dto.detectionList = DTOdatalistDeduction.ToArray();

                List<HR_Master_EarningsDeductions> arreardatalist = new List<HR_Master_EarningsDeductions>();
                //Arrear list
                arreardatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Arrear") && t.HRMED_ActiveFlag == true).ToList();

                if (arreardatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in arreardatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);
                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();
                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();
                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();
                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }
                        DTOdatalistArrear.Add(phdto);
                    }
                }
                dto.arrearList = DTOdatalistArrear.ToArray();

                List<HR_Master_EarningsDeductions> grossdatalist = new List<HR_Master_EarningsDeductions>();
                //Gross list
                grossdatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Gross") && t.HRMED_ActiveFlag == true).ToList();

                if (grossdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in grossdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);
                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();
                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();
                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();
                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }
                        DTOdatalistGross.Add(phdto);
                    }
                }
                dto.grossList = DTOdatalistGross.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO Get_Desgination(HR_Candidate_DetailsDTO data)
        {
            try
            {
                data.desgnationlist = (from a in _Context.HR_Master_Employee_DMO
                                       from b in _Context.HR_Master_Department
                                       from c in _Context.HR_Master_Designation
                                       where (a.HRMD_Id == data.HRMD_Id && a.MI_Id==data.MI_Id && a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                       select new HR_Candidate_DetailsDTO
                                       {
                                           HRMDES_Id = c.HRMDES_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                       }).Distinct().OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead(HR_Candidate_DetailsDTO dto)
        {
            HR_Candidate_DetailsDTO empdto = new HR_Candidate_DetailsDTO();
            try
            {
                if (dto.HRCED_Id > 0)
                {
                    var Documentsresult = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Single(t => t.HRCED_Id == dto.HRCED_Id);
                    Mapper.Map(dto, Documentsresult);
                    _VMSContext.Update(Documentsresult);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        CalculateEmployeeEarnDeductionDetailsByHead(dto);
                    }
                    else
                    {
                        // dto.retrunMsg = "false";
                    }
                }

                List<HR_Candidate_DetailsDTO> EarningsDeductionsDetails = new List<HR_Candidate_DetailsDTO>();
                EarningsDeductionsDetails = (from emp in _VMSContext.HR_Candidate_EarningsDeductionsDMO
                                             from med in _VMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRCD_Id == dto.HRCD_Id
                                             select new HR_Candidate_DetailsDTO
                                             {
                                                 HRCED_Id = emp.HRCED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRCD_Id = emp.HRCD_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HRCED_Amount = emp.HRCED_Amount,
                                                 HRCED_Percentage = emp.HRCED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HRCED_ActiveFlag = emp.HRCED_ActiveFlag
                                             }).ToList();

                empdto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return empdto;
        }

        public HR_Candidate_DetailsDTO CalculateEmployeeEarnDeductionDetailsByHead(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CandidateSalaryDetailDynamicCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRCD_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRCD_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO saveAppointmentdata(HR_Candidate_DetailsDTO data)
        {
            try
            {
                saveCandidateDetails(data);
                AddUpdateEmployeeEarningDetails(data);
                AddUpdateEmployeeDeductionDetails(data);
                AddUpdateEmployeeArrearDetails(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public HR_Candidate_DetailsDTO savesalarydata(HR_Candidate_DetailsDTO data)
        {
            try
            {
                AddUpdateEmployeeEarningDetails(data);
                AddUpdateEmployeeDeductionDetails(data);
                AddUpdateEmployeeArrearDetails(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Candidate Earning details
        public HR_Candidate_DetailsDTO AddUpdateEmployeeEarningDetails(HR_Candidate_DetailsDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Candidate_EarningsDeductionsDTO DocumentsDTO in dto.EarningDTO)
                    {
                         DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRCD_Id = dto.Employeedto.HRCD_Id;
                        //HR_Candidate_EarningsDeductionsDMO Documents = Mapper.Map<HR_Candidate_EarningsDeductionsDMO>(DocumentsDTO);                    

                        if (DocumentsDTO.HRCED_Id > 0)
                        {
                            var Documentsresult = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Single(t => t.HRCED_Id == DocumentsDTO.HRCED_Id);

                            Documentsresult.HRCED_Amount = DocumentsDTO.HRCED_Amount;
                            Documentsresult.HRCED_Percentage = DocumentsDTO.HRCED_Percentage;
                            Documentsresult.HRMED_Id = DocumentsDTO.HRMED_Id;
                            // Mapper.Map(DocumentsDTO, Documentsresult);
                            _VMSContext.Update(Documentsresult);
                            var flag = _VMSContext.SaveChanges();
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
                            HR_Candidate_EarningsDeductionsDMO Documents = new HR_Candidate_EarningsDeductionsDMO();
                            Documents.MI_Id = DocumentsDTO.MI_Id;
                            Documents.HRCD_Id = DocumentsDTO.HRCD_Id;
                            Documents.HRMED_Id = DocumentsDTO.HRMED_Id;
                            Documents.HRCED_Amount = DocumentsDTO.HRCED_Amount;
                            Documents.HRCED_Percentage = DocumentsDTO.HRCED_Percentage;
                            Documents.HRCED_ActiveFlag = true;
                            _VMSContext.Add(Documents);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        //Candidate Arrear details
        public HR_Candidate_DetailsDTO AddUpdateEmployeeArrearDetails(HR_Candidate_DetailsDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Candidate_EarningsDeductionsDTO DocumentsDTO in dto.ArrearDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRCD_Id = dto.Employeedto.HRCD_Id;
                        HR_Candidate_EarningsDeductionsDMO Documents = Mapper.Map<HR_Candidate_EarningsDeductionsDMO>(DocumentsDTO);

                        if (Documents.HRCED_Id > 0)
                        {
                            var Documentsresult = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Single(t => t.HRCED_Id == DocumentsDTO.HRCED_Id);
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _VMSContext.Update(Documentsresult);
                            var flag = _VMSContext.SaveChanges();
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
                            _VMSContext.Add(Documents);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        //Candidate Deduction details
        public HR_Candidate_DetailsDTO AddUpdateEmployeeDeductionDetails(HR_Candidate_DetailsDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.DeductionDTO.Count() > 0)
                {
                    foreach (HR_Candidate_EarningsDeductionsDTO DocumentsDTO in dto.DeductionDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRCD_Id = dto.Employeedto.HRCD_Id;
                        HR_Candidate_EarningsDeductionsDMO Documents = Mapper.Map<HR_Candidate_EarningsDeductionsDMO>(DocumentsDTO);

                        if (Documents.HRCED_Id > 0)
                        {
                            var Documentsresult = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Single(t => t.HRCED_Id == DocumentsDTO.HRCED_Id);
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _VMSContext.Update(Documentsresult);
                            var flag = _VMSContext.SaveChanges();
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
                            _VMSContext.Add(Documents);
                            var flag = _VMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //Candidate details
        public HR_Candidate_DetailsDTO saveCandidateDetails(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                if (dto.Employeedto.HRCD_Id > 0)
                {
                    HR_Candidate_DetailsDMO Documents = Mapper.Map<HR_Candidate_DetailsDMO>(dto.Employeedto);

                    if (Documents.HRCD_Id > 0)
                    {
                        var Documentsresult = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == Documents.HRCD_Id);
                        //Mapper.Map(Documents, Documentsresult);
                        Documentsresult.MI_Id = Documents.MI_Id;
                        Documentsresult.HRCD_FatherName = Documents.HRCD_FatherName;
                        Documentsresult.HRCD_MaritalStatus = Documents.HRCD_MaritalStatus;
                        Documentsresult.HRCD_JoiningDate = Documents.HRCD_JoiningDate;
                        Documentsresult.HRCD_Designation = Documents.HRCD_Designation;
                        Documentsresult.HRCD_BondDuration = Documents.HRCD_BondDuration;
                        Documentsresult.HRCD_CasteId = Documents.HRCD_CasteId;
                        Documentsresult.HRMC_Id = Documents.HRMC_Id;
                        Documentsresult.IVRMMG_Id = Documents.IVRMMG_Id;
                        Documentsresult.HRCD_NatureOfWork = Documents.HRCD_NatureOfWork;
                        Documentsresult.HRCD_ScopeOfService = Documents.HRCD_ScopeOfService;
                        Documentsresult.HRCD_SHName = Documents.HRCD_SHName;
                        Documentsresult.HRCD_SHGender = Documents.HRCD_SHGender;
                        Documentsresult.HRCD_SHAddress = Documents.HRCD_SHAddress;
                        Documentsresult.HRCD_SHAddress2 = Documents.HRCD_SHAddress2;
                        Documentsresult.HRCD_SHContactNo = Documents.HRCD_SHContactNo;
                        Documentsresult.HRCD_Place = Documents.HRCD_Place;
                        Documentsresult.HRCD_PINCode = Documents.HRCD_PINCode;
                        Documentsresult.HRCD_Department = Documents.HRCD_Department;
                        _VMSContext.Update(Documentsresult);
                        var flag = _VMSContext.SaveChanges();
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
                        _VMSContext.Add(Documents);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }

                long previousmi_id = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Where(t => t.HRCD_Id == dto.Employeedto.HRCD_Id).Select(t=>t.MI_Id).Distinct().FirstOrDefault();
                if(previousmi_id != dto.Employeedto.MI_Id)
                {
                    List<HR_Candidate_EarningsDeductionsDMO> obj = new List<HR_Candidate_EarningsDeductionsDMO>();
                    obj = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Where(t => t.HRCD_Id == dto.Employeedto.HRCD_Id).ToList();
                    foreach(var a in obj)
                    {
                        _VMSContext.Remove(a);
                    }
                    _VMSContext.SaveChanges();
                    //dto.EarningDTO = null;
                    //dto.DeductionDTO = null;
                    //dto.ArrearDTO = null;
                    //dto.EarningDTO = null;
                    //dto.HRCED_Id = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO getcandidate(HR_Candidate_DetailsDTO dto)
        {
            List<HR_Candidate_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Candidate_EarningsDeductionsDTO>();
            dto.retrunMsg = "";
            try
            {
                dto.MI_Id = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_ActiveFlg == true && t.HRCD_Id == dto.HRCD_Id).Select(t => t.MI_Id).FirstOrDefault();

                dto.CandidateDetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_ActiveFlg == true && t.HRCD_Id == dto.HRCD_Id).ToArray();
                dto.MasterQualification = _VMSContext.HR_Master_Course.Where(t => t.MI_Id == dto.MI_Id && t.HRMC_ActiveFlag == true).ToArray();
                dto.masterCaste = _VMSContext.mastercasteDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.mastermaritalstatus = _VMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMMS_ActiveFlag == true).ToArray();

                dto.IVRMMG_GenderName = (from a in _VMSContext.IVRM_Master_Gender
                                         from b in _VMSContext.HR_Candidate_DetailsDMO
                                         where (a.IVRMMG_Id == b.IVRMMG_Id && a.MI_Id == b.MI_Id && b.HRCD_Id == dto.HRCD_Id)
                                         select a.IVRMMG_GenderName.ToUpper()).FirstOrDefault();

                dto.HRCD_SHGenderName = (from a in _VMSContext.IVRM_Master_Gender
                                         from b in _VMSContext.HR_Candidate_DetailsDMO
                                         where (a.IVRMMG_Id == b.HRCD_SHGender && a.MI_Id == b.MI_Id && b.HRCD_Id == dto.HRCD_Id)
                                         select a.IVRMMG_GenderName.ToUpper()).FirstOrDefault();

                dto.HRMJ_JobTiTle = (from a in _VMSContext.HR_Master_JobsDMO
                                     from b in _VMSContext.HR_Candidate_DetailsDMO
                                     where (a.HRMJ_Id == b.HRMJ_Id && b.HRCD_Id == dto.HRCD_Id)
                                     select a.HRMJ_JobTiTle).FirstOrDefault();

                EarningsDeductionsDetails = (from emp in _VMSContext.HR_Candidate_EarningsDeductionsDMO
                                             from med in _VMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRCD_Id == dto.HRCD_Id
                                             select new HR_Candidate_EarningsDeductionsDTO
                                             {
                                                 HRCED_Id = emp.HRCED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRMED_Name = med.HRMED_Name,
                                                 HRCD_Id = emp.HRCD_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HRCED_Amount = emp.HRCED_Amount,
                                                 HRCED_Percentage = emp.HRCED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HRCED_ActiveFlag = emp.HRCED_ActiveFlag
                                             }).ToList();
                dto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();

                dto.HRCD_AppDate = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_ActiveFlg == true && t.HRCD_Id == dto.HRCD_Id).Select(t => Convert.ToDateTime(t.HRCD_JoiningDate).AddYears(4)).FirstOrDefault();

                dto.companydetails = _VMSContext.Institution.Where(t => t.MI_Id == dto.MI_Id && t.MI_ActiveFlag == 1).ToArray();

                dto.interviewlist = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     from b in _VMSContext.HR_CandidateInterviewScheduleDMO
                                     from c in _VMSContext.HR_Candidate_InterviewStatusDMO
                                     from d in _VMSContext.IVRM_Staff_User_Login
                                     from e in _VMSContext.HR_Master_Employee_DMO
                                     where (a.HRCD_Id == dto.HRCD_Id && a.HRCD_Id == b.HRCD_Id && b.HRCISC_Interviewer == c.IVRMUL_Id && b.HRCD_Id == c.HRCD_Id && d.Id == c.IVRMUL_Id && e.HRME_Id == d.Emp_Code)
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRCD_Id = a.HRCD_Id,
                                         HRCISC_InterviewDateTime = b.HRCISC_InterviewDateTime,
                                         HRCISC_InterviewRounds = b.HRCISC_InterviewRounds,
                                         HRCISC_InterviewVenue = b.HRCISC_InterviewVenue,
                                         HRME_employeename = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                         HRCIS_InterviewFeedBack = c.HRCIS_InterviewFeedBack,
                                         HRCIS_Status = c.HRCIS_Status,
                                         HRCIS_CandidateStatus = c.HRCIS_CandidateStatus
                                     }).ToArray();

                dto.letterdetails = _VMSContext.HR_Candidate_AppointmentDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO getCandidateList(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                var institutionlist = (from a in _VMSContext.Institution
                                       where a.MI_ActiveFlag == 1 && a.MI_Id==dto.MI_Id
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                dto.institutionlist = institutionlist.ToArray();
                if (dto.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        dto.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
            return dto;
        }
        public HR_Candidate_DetailsDTO getcandidatename(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                dto.candidatelist = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     from b in _VMSContext.Institution
                                     where (a.MI_Id == dto.MI_Id && a.HRCD_ActiveFlg == true && a.MI_Id==b.MI_Id)
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRCD_Id = a.HRCD_Id,
                                         HRCD_FirstName = ((a.HRCD_FirstName == null ? " " : a.HRCD_FirstName) + " " + (a.HRCD_MiddleName == null ? " " : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? " " : a.HRCD_LastName)).Trim(),
                                     }).Distinct().OrderBy(t => t.HRCD_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
            return dto;
        }

        public HR_Candidate_DetailsDTO getcompanydetails(HR_Candidate_DetailsDTO dto)
        {
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistArrear = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistGross = new List<HR_Master_EarningsDeductionsDTO>();

            try
            {                
                dto.departmentlist = _VMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();
                dto.desgnationlist = _VMSContext.HR_Master_Designation.Where(t => t.MI_Id == dto.MI_Id && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();
                dto.MasterGender = _VMSContext.IVRM_Master_Gender.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMG_ActiveFlag == true).ToArray();
                dto.MasterQualification = _VMSContext.HR_Master_Course.Where(t => t.MI_Id == dto.MI_Id && t.HRMC_ActiveFlag == true).ToArray();
                dto.masterCaste = _VMSContext.mastercasteDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.mastermaritalstatus = _VMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMMS_ActiveFlag == true).ToArray();
                dto.companydetails = _VMSContext.Institution.Where(t => t.MI_Id == dto.MI_Id && t.MI_ActiveFlag == 1).ToArray();

                dto.earingdeductionlist = (from a in _VMSContext.HR_Master_EarningsDeductions
                                           where (a.MI_Id == dto.MI_Id && a.HRMED_ActiveFlag == true)
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMED_Name = a.HRMED_Name,
                                               HRMED_AmountPercentFlag = a.HRMED_AmountPercentFlag,
                                           }).Distinct().ToArray();

                List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
                //Earning list
                earningdatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning") && t.HRMED_ActiveFlag == true).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                dto.earningList = DTOdatalistEarning.ToArray();

                List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();
                //Deduction List
                deductiondatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).ToList();

                if (deductiondatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistDeduction.Add(phdto);

                    }

                }

                dto.detectionList = DTOdatalistDeduction.ToArray();

                List<HR_Master_EarningsDeductions> arreardatalist = new List<HR_Master_EarningsDeductions>();
                //Arrear list
                arreardatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Arrear") && t.HRMED_ActiveFlag == true).ToList();

                if (arreardatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in arreardatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);
                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();
                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();
                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();
                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }
                        DTOdatalistArrear.Add(phdto);
                    }
                }
                dto.arrearList = DTOdatalistArrear.ToArray();

                List<HR_Master_EarningsDeductions> grossdatalist = new List<HR_Master_EarningsDeductions>();
                //Gross list
                grossdatalist = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Gross") && t.HRMED_ActiveFlag == true).ToList();

                if (grossdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in grossdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);
                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _VMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();
                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();
                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _VMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();
                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }
                        DTOdatalistGross.Add(phdto);
                    }
                }
                dto.grossList = DTOdatalistGross.ToArray();
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
