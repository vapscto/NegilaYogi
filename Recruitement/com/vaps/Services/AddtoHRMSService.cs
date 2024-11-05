using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.RegularExpressions;

namespace Recruitment.com.vaps.Services
{
    public class AddtoHRMSService : Interfaces.AddtoHRMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AddtoHRMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
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
                                      HRCD_MiddleName = a.HRCD_MiddleName,
                                      HRCD_LastName = a.HRCD_LastName,
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
            try
            {
                dto.MasterEmployeetype = (from emp in _VMSContext.HR_Master_EmployeeType
                                    where emp.HRMET_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_Candidate_DetailsDTO
                                    {
                                        HRMET_Id = emp.HRMET_Id,
                                        HRMET_EmployeeType = emp.HRMET_EmployeeType
                                    }).ToArray();

                dto.GroupTypeList = (from a in _VMSContext.HR_Master_GroupTypeDMO
                                     where a.MI_Id == dto.MI_Id && a.HRMGT_ActiveFlag == true
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRMGT_Id = a.HRMGT_Id,
                                         HRMGT_EmployeeGroupType = a.HRMGT_EmployeeGroupType,
                                         HRMGT_Order = a.HRMGT_Order
                                     }).Distinct().OrderBy(t => t.HRMGT_Order).ToArray();

                dto.candidatelist = (from a in _VMSContext.HR_Candidate_DetailsDMO
                                     where (a.MI_Id == dto.MI_Id && a.HRCD_ActiveFlg == true && a.HRCD_RecruitmentStatus == "Selected")
                                     select new HR_Candidate_DetailsDTO
                                     {
                                         HRCD_Id=a.HRCD_Id,
                                         HRCD_FullName = ((a.HRCD_FirstName == null ? " " : a.HRCD_FirstName) + " " + (a.HRCD_MiddleName == null ? " " : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? " " : a.HRCD_LastName)).Trim(),
                                         HRCD_FirstName = a.HRCD_FirstName,
                                         HRCD_MiddleName = a.HRCD_MiddleName,
                                         HRCD_LastName = a.HRCD_LastName,
                                         HRCD_AddressPermanent = a.HRCD_AddressPermanent,
                                         HRCD_AddressLocal = a.HRCD_AddressLocal,
                                         IVRMMG_Id = a.IVRMMG_Id,
                                         HRCD_FatherName = a.HRCD_FatherName,
                                         HRCD_DOB = a.HRCD_DOB,
                                         HRCD_MobileNo = a.HRCD_MobileNo,
                                         HRCD_EmailId = a.HRCD_EmailId,
                                         HRCD_Photo = a.HRCD_Photo,
                                         HRCD_AadharNo = a.HRCD_AadharNo
                                     }).Distinct().OrderBy(t => t.HRCD_Id).ToArray();

                dto.GradeList = (from a in _VMSContext.HR_Master_Grade
                                           where (a.MI_Id == dto.MI_Id && a.HRMG_ActiveFlag == true)
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMG_Id = a.HRMG_Id,
                                               HRMG_GradeName = a.HRMG_GradeName,
                                           }).Distinct().ToArray();

                dto.maritalstatuslist = (from c in _VMSContext.IVRM_Master_Marital_Status
                                       where (c.IVRMMMS_ActiveFlag == true && c.MI_Id == dto.MI_Id)
                                       select new HR_Candidate_DetailsDTO
                                       {
                                           IVRMMMS_Id = c.IVRMMMS_Id,
                                           IVRMMMS_MaritalStatus = c.IVRMMMS_MaritalStatus,
                                       }).Distinct().ToArray();

                dto.castecategorylist = (from c in _VMSContext.castecategoryDMO
                                      select new HR_Candidate_DetailsDTO
                                      {
                                          IMCC_Id = c.IMCC_Id,
                                          IMCC_CategoryName = c.IMCC_CategoryName
                                      }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

                dto.castelist = (from c in _VMSContext.mastercasteDMO
                                 select new HR_Candidate_DetailsDTO
                                {
                                     IMC_Id = c.IMC_Id,
                                     IMC_CasteName = c.IMC_CasteName
                                 }).Distinct().ToArray();

                dto.masterreligionlist = (from c in _VMSContext.MasterReligionDMO
                                          where (c.Is_Active == true)
                                          select new HR_Candidate_DetailsDTO
                                            {
                                              IVRMMR_Id = c.IVRMMR_Id,
                                              IVRMMR_Name = c.IVRMMR_Name
                                          }).Distinct().ToArray();

                dto.employeedepartmentlist = (from c in _VMSContext.HR_Master_Department
                                          where (c.HRMD_ActiveFlag == true && c.MI_Id == dto.MI_Id)
                                          select new HR_Candidate_DetailsDTO
                                          {
                                              HRMD_Id = c.HRMD_Id,
                                              HRMD_DepartmentName = c.HRMD_DepartmentName
                                          }).Distinct().ToArray();

                dto.employeedesignationlist = (from c in _VMSContext.HR_Master_Designation
                                               where (c.HRMDES_ActiveFlag == true && c.MI_Id == dto.MI_Id)
                                          select new HR_Candidate_DetailsDTO
                                          {
                                              HRMDES_Id = c.HRMDES_Id,
                                              HRMDES_DesignationName = c.HRMDES_DesignationName
                                          }).Distinct().ToArray();
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
                                             }
                                ).ToList();

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

        
        public HR_Candidate_DetailsDTO savetohrms(HR_Candidate_DetailsDTO data)
        {
            try
            {
                data = saveEmployeeDetails(data);
                if (data.retrunMsg == "Add")
                {
                    AddSalaryDetails(data);
                    SendEmailWelcome(data, "Welcome Letter", "Welcome Letter", data.MI_Id);
                }
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
                        Documentsresult.HRCD_FatherName = Documents.HRCD_FatherName;
                        Documentsresult.HRCD_AadharNo = Documents.HRCD_AadharNo;
                        Documentsresult.HRCD_JoiningDate = Documents.HRCD_JoiningDate;
                        Documentsresult.HRCD_Designation = Documents.HRCD_Designation;
                        Documentsresult.HRCD_BondDuration = Documents.HRCD_BondDuration;
                        Documentsresult.HRCD_AddressLocal = Documents.HRCD_AddressLocal;
                        Documentsresult.HRCD_AddressPermanent = Documents.HRCD_AddressPermanent;
                        Documentsresult.HRCD_NatureOfWork = Documents.HRCD_NatureOfWork;
                        Documentsresult.HRCD_ScopeOfService = Documents.HRCD_ScopeOfService;
                        Documentsresult.HRCD_SHName = Documents.HRCD_SHName;
                        Documentsresult.HRCD_SHGender = Documents.HRCD_SHGender;
                        Documentsresult.HRCD_SHAddress = Documents.HRCD_SHAddress;
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
                dto.CandidateDetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_ActiveFlg == true && t.MI_Id == dto.MI_Id && t.HRCD_Id == dto.HRCD_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Candidate_DetailsDTO saveEmployeeDetails(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                if (dto.Employeedto.HRCD_Id > 0)
                {
                    MasterEmployee Documents = new MasterEmployee();
                    HR_Candidate_DetailsDTO Candidate = Mapper.Map<HR_Candidate_DetailsDTO>(dto.Employeedto);
                    if (Candidate.HRCD_Id > 0)
                    {
                        var duplicate = _VMSContext.HR_Master_Employee_DMO.Where(t => t.HRME_EmployeeFirstName == Candidate.HRCD_FirstName && t.HRME_EmployeeMiddleName == Candidate.HRCD_MiddleName && t.HRME_EmployeeLastName == Candidate.HRCD_LastName && t.HRME_FatherName == Candidate.HRCD_FatherName && t.HRME_MobileNo == Candidate.HRCD_MobileNo && t.HRME_EmailId == Candidate.HRCD_EmailId && t.HRME_AadharCardNo == Candidate.HRCD_AadharNo).ToArray();

                        //&& t.HRME_DOB == Candidate.HRCD_DOB 

                        if (duplicate.Length > 0)
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                        else
                        {
                            var Documentsresult = _VMSContext.HR_Candidate_DetailsDMO.Single(t => t.HRCD_Id == Candidate.HRCD_Id);
                            Documents.MI_Id = dto.MI_Id;
                            Documents.HRMET_Id = Candidate.HRMET_Id;
                            Documents.HRMGT_Id = Candidate.HRMGT_Id;
                            Documents.HRMD_Id = Candidate.HRMD_Id;
                            Documents.HRMDES_Id = Candidate.HRMDES_Id;
                            Documents.HRMG_Id = Candidate.HRMG_Id;
                            Documents.HRME_EmployeeFirstName = Candidate.HRCD_FirstName;
                            Documents.HRME_EmployeeMiddleName = Candidate.HRCD_MiddleName;
                            Documents.HRME_EmployeeLastName = Candidate.HRCD_LastName;
                            Documents.HRME_EmployeeCode = Candidate.HRME_EmployeeCode;
                            Documents.HRME_PerArea = Candidate.HRCD_AddressPermanent;
                            Documents.HRME_LocArea = Candidate.HRCD_AddressLocal;
                            Documents.IVRMMMS_Id = Candidate.IVRMMMS_Id;
                            Documents.IVRMMG_Id = Candidate.IVRMMG_Id;
                            Documents.CasteCategoryId = Candidate.casteCategoryId;
                            Documents.CasteId = Candidate.casteId;
                            Documents.ReligionId = Candidate.religionId;
                            Documents.HRME_FatherName = Candidate.HRCD_FatherName;
                            Documents.HRME_DOB = Candidate.HRCD_DOB;
                            Documents.HRME_DOJ = DateTime.Today;
                            Documents.HRME_ExpectedRetirementDate = DateTime.Today.AddYears(60);
                            Documents.HRME_MobileNo = Candidate.HRCD_MobileNo;
                            Documents.HRME_EmailId = Candidate.HRCD_EmailId;
                            Documents.HRME_Photo = Candidate.HRCD_Photo;
                            Documents.HRME_LeftFlag = false;
                            Documents.HRME_AadharCardNo = Candidate.HRCD_AadharNo;
                            Documents.HRME_ActiveFlag = true;
                            _VMSContext.Update(Documents);
                            var flag = _VMSContext.SaveChanges();
                            dto.HRME_ID = Documents.HRME_Id;
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

        public HR_Candidate_DetailsDTO AddSalaryDetails(HR_Candidate_DetailsDTO dto)
        {
            try
            {
                List<HR_Candidate_EarningsDeductionsDMO> salarylist = new List<HR_Candidate_EarningsDeductionsDMO>();
                salarylist = _VMSContext.HR_Candidate_EarningsDeductionsDMO.Where(t => t.HRCD_Id == dto.Employeedto.HRCD_Id).ToList();

                foreach(HR_Candidate_EarningsDeductionsDMO obj in salarylist)
                {
                    HR_Employee_EarningsDeductions obj2 = new HR_Employee_EarningsDeductions();
                    obj2.MI_Id = dto.MI_Id;
                    obj2.HRME_Id = dto.HRME_ID;
                    obj2.HRMED_Id = obj.HRMED_Id;
                    obj2.HREED_Amount = obj.HRCED_Amount;
                    obj2.HREED_Percentage = obj.HRCED_Percentage;
                    obj2.HREED_ActiveFlag = obj.HRCED_ActiveFlag;
                    _VMSContext.Add(obj2);
                    _VMSContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                dto.retrunMsg = "Something Went Wrong!!";
            }
            return dto;
        }

        public void WelcomeNotification(HR_Candidate_DetailsDTO obj, string subject, string body, long id)
        {
            try
            {
                string mailid = obj.Employeedto.HRCD_EmailId;
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();
                string Mailmsg = "Dear Sir/Madam, Please find enclosed PDF attachment of Welcome Letter.Thanking You.";

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(mailid);
                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    StringBuilder sb = new StringBuilder(obj.Template);
                    StringReader sr = new StringReader(sb.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();

                        htmlparser.Parse(sr);
                        pdfDoc.Close();

                        byte[] bytess = memoryStream.ToArray();
                        memoryStream.Close();

                        var file = Convert.ToBase64String(bytess);
                        string emp;
                        emp = Convert.ToString(sr);
                        string c = "";
                        string v = emp.Replace("System.IO.StringReader", "WelcomeLetter.Pdf");
                        message.AddAttachment(v, file);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Selected_Notification_Rec", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendEmailWelcome(HR_Candidate_DetailsDTO obj, string subject, string body, long id)
        {
            try
            {
                var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == obj.Employeedto.HRCD_Id).FirstOrDefault();
                string imagepath = "";
                if (id == 16) { imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-2.jpg"; }
                else if (id == 17) { imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-1.jpg"; }
                else if (id == 20) { imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-4.jpg"; }
                else if (id == 21) { imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-5.jpg"; }
                else if (id == 22) { imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-3.jpg"; }
                else if (id == 24) { imagepath = ""; }
                var jobpost = _VMSContext.HR_Master_JobsDMO.Where(t => t.HRMJ_Id == candidatedetails.HRMJ_Id).FirstOrDefault();

                string mailid = candidatedetails.HRCD_EmailId;
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Welcomemail_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = "hr@vapsknowledge.com";
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[IMAGEPATH]", imagepath);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", candidatedetails.HRCD_FirstName + " " + (candidatedetails.HRCD_MiddleName == null ? "" : candidatedetails.HRCD_MiddleName) + " " + (candidatedetails.HRCD_LastName == null ? "" : candidatedetails.HRCD_LastName));
                        message.HtmlContent = message.HtmlContent.Replace("[ADDRESS]", candidatedetails.HRCD_AddressPermanent);
                        message.HtmlContent = message.HtmlContent.Replace("[JOBPOST]", jobpost.HRMJ_JobTiTle);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Welcomemail_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
