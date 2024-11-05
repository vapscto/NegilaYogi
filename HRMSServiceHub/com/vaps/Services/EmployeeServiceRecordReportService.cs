using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeServiceRecordReportService : Interfaces.EmployeeServiceRecordReportInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeServiceRecordReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public EmployeeServiceRecordReportDTO getBasicData(EmployeeServiceRecordReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public EmployeeServiceRecordReportDTO FilterEmployeeData(EmployeeServiceRecordReportDTO dto)
        {
            try
            {
                //Working , Left

                if (dto.FormatType.Equals("Format1"))
                {

                    if (dto.Left.Equals(true) && dto.Working.Equals(true))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                    else if (dto.Left.Equals(true) && dto.Working.Equals(false))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == dto.Left && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                    else if (dto.Left.Equals(false) && dto.Working.Equals(true))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag ==dto.Left && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                    else if (dto.Left.Equals(false) && dto.Working.Equals(false))
                    {
                       // dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(null) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                }
                else if (dto.FormatType.Equals("Format2"))
                {

                    if (dto.AWL.Equals("AllF2"))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                    if (dto.AWL.Equals("WorkingF2"))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(false) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                    if (dto.AWL.Equals("LeftF2"))
                    {
                        dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(true) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return dto;
        }
        public EmployeeServiceRecordReportDTO GetAllDropdownAndDatatableDetails(EmployeeServiceRecordReportDTO dto)
        {
            try
            {
                

                    //emptype
                    dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

                    //employee  
                    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public async Task<EmployeeServiceRecordReportDTO> getEmployeedetailsBySelection(EmployeeServiceRecordReportDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
            {
               

                if (dto.FormatType.Equals("Format1"))
                {
                    //Employee details
                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id == dto.HRME_Id).ToList();

                    if (employeeDetails.Count() > 0)
                    {
                        List<long> empIds = employeeDetails.Select(c => c.HRME_Id).Distinct().ToList();

                        EmployeeServiceRecordsByFormatOne(empIds, dto);
                    }

                }
                else if (dto.FormatType.Equals("Format2"))
                {

                    if (dto.AllOrIndividual.Equals("Individual"))
                    {
                        if (dto.AWL.Equals("AllF2"))
                        {
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        if (dto.AWL.Equals("WorkingF2"))
                        {
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(false) && t.HRME_ActiveFlag == true).ToList();
                        }
                        if (dto.AWL.Equals("LeftF2"))
                        {
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(true) && t.HRME_ActiveFlag == true).ToList();
                        }


                        if (dto.TypeOrEmployee.Equals("Type"))
                        {
                            //Employee details
                            employeeDetails = employeeDetails.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_Id == dto.HRMET_Id && t.HRME_ActiveFlag == true).ToList();

                        }
                        if (dto.TypeOrEmployee.Equals("Employee"))
                        {
                            //Employee details
                             employeeDetails = employeeDetails.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id == dto.HRME_Id).ToList();

                        }
                    }
                    else if (dto.AllOrIndividual.Equals("All"))
                    {
                        if (dto.AWL.Equals("AllF2"))
                        {
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToList();
                        }
                        if (dto.AWL.Equals("WorkingF2"))
                        {
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(false) && t.HRME_ActiveFlag == true).ToList();
                        }
                        if (dto.AWL.Equals("LeftF2"))
                        {
                            employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag.Equals(true) && t.HRME_ActiveFlag == true).ToList();
                        }
                    }
                    if (employeeDetails.Count() > 0)
                    {
                        List<long> empIds = employeeDetails.Select(c => c.HRME_Id).Distinct().ToList();

                        dto= EmployeeServiceRecordsByFormatTWO(empIds, dto);

                    }
                }

                Institution institute = new Institution();
                institute = _Context.Institution.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }


        public EmployeeServiceRecordReportDTO EmployeeServiceRecordsByFormatOne(List<long> empIds, EmployeeServiceRecordReportDTO dto)
        {

            List<EmployeeServiceRecordReportDTO> AllEmp = new List<EmployeeServiceRecordReportDTO>();
            try
            {

                foreach (long empId in empIds)
                {
                    EmployeeServiceRecordReportDTO currentEmp = new EmployeeServiceRecordReportDTO();

                    String MobileNumbers = GetEmployeeMobileNumbers(empId);
                    String EmailIds = GetEmployeeEmailIds(empId);


                    currentEmp = (from a in _HRMSContext.MasterEmployee
                                  from b in _HRMSContext.HR_Master_GroupType

                                  from c in _HRMSContext.HR_Master_Designation
                                  from d in _HRMSContext.HR_Master_Department
                                  from e in _HRMSContext.IVRM_Master_Gender
                                  from f in _HRMSContext.Caste
                                  from g in _HRMSContext.Religion
                                      // from h in _Context.State
                                      //  from i in _Context.country


                                  where (
                                   a.HRMGT_Id == b.HRMGT_Id &&
                                   a.HRMDES_Id == c.HRMDES_Id &&
                                   a.HRMD_Id == d.HRMD_Id &&
                                   a.IVRMMG_Id == e.IVRMMG_Id &&
                                   a.CasteId == f.IMC_Id &&
                                   a.ReligionId == g.IVRMMR_Id &&
                                   //h.IVRMMS_Id == a.HRME_LocStateId &&
                                   //h.IVRMMS_Id == a.HRME_PerStateId &&
                                   //i.IVRMMC_Id == a.HRME_LocCountryId &&
                                   //i.IVRMMC_Id == a.HRME_PerCountryId &&
                                   a.MI_Id.Equals(dto.MI_Id) &&
                                   a.HRME_Id == empId //&&
                                                      //a.HRME_ActiveFlag == true
                                )
                                  select new EmployeeServiceRecordReportDTO
                                  {
                                      // a,b,c,d,e,f,g//,h,i

                                      HRME_Id = a.HRME_Id,
                                      MI_Id = a.MI_Id,
                                      HRMET_Id = a.HRMET_Id,

                                      HRMGT_Id = a.HRMGT_Id,
                                      HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
                                      HRMD_Id = a.HRMD_Id,
                                      HRMD_DepartmentName = d.HRMD_DepartmentName,
                                      HRMDES_Id = a.HRMDES_Id,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName,
                                      HRMG_Id = a.HRMG_Id,

                                      HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                      HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_BiometricCode = a.HRME_BiometricCode,
                                      HRME_RFCardId = a.HRME_RFCardId,
                                      EmployeeContactNo = MobileNumbers,
                                      EmployeeEmailId = EmailIds,

                                      HRME_PerStreet = a.HRME_PerStreet,
                                      HRME_PerArea = a.HRME_PerArea,
                                      HRME_PerCity = a.HRME_PerCity,
                                      HRME_PerStateId = a.HRME_PerStateId,
                                    
                                      HRME_PerCountryId = a.HRME_PerCountryId,

                                      HRME_PerPincode = a.HRME_PerPincode,
                                      HRME_LocStreet = a.HRME_LocStreet,
                                      HRME_LocArea = a.HRME_LocArea,
                                      HRME_LocCity = a.HRME_LocCity,


                                      HRME_LocStateId = a.HRME_LocStateId,
                                      HRME_LocCountryId = a.HRME_LocCountryId,


                                      HRME_LocPincode = a.HRME_LocPincode,
                                      //  IVRMMMS_Id = a.IVRMMMS_Id,

                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = e.IVRMMG_GenderName,
                                      CasteId = a.CasteId,
                                      IMC_CasteName = f.IMC_CasteName,

                                      ReligionId = a.ReligionId,
                                      IVRMMR_Name = g.IVRMMR_Name,

                                      HRME_FatherName = a.HRME_FatherName,
                                      HRME_MotherName = a.HRME_MotherName,

                                      HRME_DOB = a.HRME_DOB,
                                      HRME_DOJ = a.HRME_DOJ,
                                      HRME_ExpectedRetirementDate = a.HRME_ExpectedRetirementDate,
                                      HRME_PFDate = a.HRME_PFDate,
                                      HRME_ESIDate = a.HRME_ESIDate,
                                      HRME_MobileNo = a.HRME_Id,
                                      HRME_EmailId = a.HRME_EmailId,
                                      HRME_BloodGroup = a.HRME_BloodGroup,
                                      HRME_PaymentType = a.HRME_PaymentType,
                                      HRME_BankAccountNo = a.HRME_BankAccountNo,
                                      HRME_PFApplicableFlag = a.HRME_PFApplicableFlag,
                                      HRME_PFMaxFlag = a.HRME_PFMaxFlag,
                                      HRME_PFFixedFlag = a.HRME_PFFixedFlag,
                                      HRME_PFAccNo = a.HRME_PFAccNo,
                                      HRME_ESIAccNo = a.HRME_ESIAccNo,
                                      HRME_GratuityAccNo = a.HRME_GratuityAccNo,
                                      HRME_ESIApplicableFlag = a.HRME_ESIApplicableFlag,
                                      HRME_Photo = a.HRME_Photo,
                                      HRME_LeftFlag = a.HRME_LeftFlag,
                                      HRME_DOL = a.HRME_DOL,
                                      HRME_LeavingReason = a.HRME_LeavingReason,
                                      HRME_Height = a.HRME_Height,
                                      HRME_HeightUOM = a.HRME_HeightUOM,
                                      HRME_Weight = a.HRME_Weight,
                                      HRME_WeightUOM = a.HRME_WeightUOM,
                                      HRME_IdentificationMark = a.HRME_IdentificationMark,
                                      HRME_ApprovalNo = a.HRME_ApprovalNo,
                                      HRME_PANCardNo = a.HRME_PANCardNo,
                                      HRME_AadharCardNo = a.HRME_AadharCardNo,
                                      HRME_SubstituteFlag = a.HRME_SubstituteFlag,
                                      HRME_NationalSSN = a.HRME_NationalSSN,
                                      HRME_SalaryType = a.HRME_SalaryType,
                                      HRME_EmployeeOrder = a.HRME_EmployeeOrder

                                  }).FirstOrDefault();


                    AllEmp.Add(currentEmp);
                }



                dto.employeeDetails = AllEmp.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dto;

        }

        public EmployeeServiceRecordReportDTO EmployeeServiceRecordsByFormatTWO(List<long> empIds, EmployeeServiceRecordReportDTO dto)
        {
            List<EmployeeServiceRecordReportDTO> AllEmp = new List<EmployeeServiceRecordReportDTO>();
            try
            {

                foreach (long empId in empIds)
                {
                    EmployeeServiceRecordReportDTO currentEmp = new EmployeeServiceRecordReportDTO();

                   String MobileNumbers =  GetEmployeeMobileNumbers(empId);
                    String EmailIds = GetEmployeeEmailIds(empId);


                    currentEmp = (from a in _HRMSContext.MasterEmployee
                                  from b in _HRMSContext.HR_Master_GroupType

                                  from c in _HRMSContext.HR_Master_Designation
                                  from d in _HRMSContext.HR_Master_Department


                                  from e in _HRMSContext.IVRM_Master_Gender
                                  from f in _HRMSContext.Caste
                                  from g in _HRMSContext.Religion
                                      // from h in _Context.State
                                      //  from i in _Context.country


                                  where (
                                   
                                   a.HRMGT_Id == b.HRMGT_Id &&
                                   a.HRMDES_Id == c.HRMDES_Id &&
                                   a.HRMD_Id == d.HRMD_Id &&
                                   a.IVRMMG_Id == e.IVRMMG_Id &&
                                   a.CasteId == f.IMC_Id &&
                                   a.ReligionId == g.IVRMMR_Id &&
                                   //h.IVRMMS_Id == a.HRME_LocStateId &&
                                   //h.IVRMMS_Id == a.HRME_PerStateId &&
                                   //i.IVRMMC_Id == a.HRME_LocCountryId &&
                                   //i.IVRMMC_Id == a.HRME_PerCountryId &&
                                   a.MI_Id.Equals(dto.MI_Id) &&
                                   a.HRME_Id == empId //&&
                                    //a.HRME_ActiveFlag == true
                                )
                                  select new EmployeeServiceRecordReportDTO
                                  {
                                      // a,b,c,d,e,f,g//,h,i

                                      HRME_Id = a.HRME_Id,
                                      MI_Id = a.MI_Id,
                                      HRMET_Id = a.HRMET_Id,

                                      HRMGT_Id = a.HRMGT_Id,
                                      HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
                                      HRMD_Id = a.HRMD_Id,
                                      HRMD_DepartmentName = d.HRMD_DepartmentName,
                                      HRMDES_Id = a.HRMDES_Id,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName,
                                      HRMG_Id = a.HRMG_Id,

                                      HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                      HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_BiometricCode = a.HRME_BiometricCode,
                                      HRME_RFCardId = a.HRME_RFCardId,

                                      EmployeeContactNo = MobileNumbers,
                                      EmployeeEmailId = EmailIds,


                                      HRME_PerStreet = a.HRME_PerStreet,
                                      HRME_PerArea = a.HRME_PerArea,
                                      HRME_PerCity = a.HRME_PerCity,
                                      HRME_PerStateId = a.HRME_PerStateId,
                                      
                                      HRME_PerCountryId = a.HRME_PerCountryId,

                                      HRME_PerPincode = a.HRME_PerPincode,
                                      HRME_LocStreet = a.HRME_LocStreet,
                                      HRME_LocArea = a.HRME_LocArea,
                                      HRME_LocCity = a.HRME_LocCity,


                                      HRME_LocStateId = a.HRME_LocStateId,
                                      HRME_LocCountryId = a.HRME_LocCountryId,


                                      HRME_LocPincode = a.HRME_LocPincode,
                                      //  IVRMMMS_Id = a.IVRMMMS_Id,

                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = e.IVRMMG_GenderName,
                                      CasteId = a.CasteId,
                                      IMC_CasteName = f.IMC_CasteName,

                                      ReligionId = a.ReligionId,
                                      IVRMMR_Name = g.IVRMMR_Name,

                                      HRME_FatherName = a.HRME_FatherName,
                                      HRME_MotherName = a.HRME_MotherName,

                                      HRME_DOB = a.HRME_DOB,
                                      HRME_DOJ = a.HRME_DOJ,
                                      HRME_ExpectedRetirementDate = a.HRME_ExpectedRetirementDate,
                                      HRME_PFDate = a.HRME_PFDate,
                                      HRME_ESIDate = a.HRME_ESIDate,
                                      HRME_MobileNo = a.HRME_Id,
                                      HRME_EmailId = a.HRME_EmailId,
                                      HRME_BloodGroup = a.HRME_BloodGroup,
                                      HRME_PaymentType = a.HRME_PaymentType,
                                      HRME_BankAccountNo = a.HRME_BankAccountNo,
                                      HRME_PFApplicableFlag = a.HRME_PFApplicableFlag,
                                      HRME_PFMaxFlag = a.HRME_PFMaxFlag,
                                      HRME_PFFixedFlag = a.HRME_PFFixedFlag,
                                      HRME_PFAccNo = a.HRME_PFAccNo,
                                      HRME_ESIAccNo = a.HRME_ESIAccNo,
                                      HRME_GratuityAccNo = a.HRME_GratuityAccNo,
                                      HRME_ESIApplicableFlag = a.HRME_ESIApplicableFlag,
                                      HRME_Photo = a.HRME_Photo,
                                      HRME_LeftFlag = a.HRME_LeftFlag,
                                      HRME_DOL = a.HRME_DOL,
                                      HRME_LeavingReason = a.HRME_LeavingReason,
                                      HRME_Height = a.HRME_Height,
                                      HRME_HeightUOM = a.HRME_HeightUOM,
                                      HRME_Weight = a.HRME_Weight,
                                      HRME_WeightUOM = a.HRME_WeightUOM,
                                      HRME_IdentificationMark = a.HRME_IdentificationMark,
                                      HRME_ApprovalNo = a.HRME_ApprovalNo,
                                      HRME_PANCardNo = a.HRME_PANCardNo,
                                      HRME_AadharCardNo = a.HRME_AadharCardNo,
                                      HRME_SubstituteFlag = a.HRME_SubstituteFlag,
                                      HRME_NationalSSN = a.HRME_NationalSSN,
                                      HRME_SalaryType = a.HRME_SalaryType,
                                      HRME_EmployeeOrder = a.HRME_EmployeeOrder

                                  }).FirstOrDefault();


                    AllEmp.Add(currentEmp);
                }

               

                dto.employeeDetails = AllEmp.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dto;

        }


        public string GetEmployeeMobileNumbers(long empId)
        {
            string mobileNumbers = "";

            List<long> temparr = new List<long>();
            //getting all mobilenumbers
            try
            {
                var Phone_Noresult = _HRMSContext.Emp_MobileNo.Where(t => t.HRME_Id == empId).ToList();
                foreach (Multiple_Mobile_DMO ph1 in Phone_Noresult)
                {
                    temparr.Add(ph1.HRMEMNO_MobileNo);
                }
                string combindedString = 

                mobileNumbers = string.Join(", ", temparr.ToArray());
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee);
            }
           
           

            return mobileNumbers;
        }

        public string GetEmployeeEmailIds(long empId)
        {
            string EmailIds = "";
            List<string> temparr = new List<string>();
            try
            {
                var Email_Idresult = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == empId).ToList();
                foreach (Multiple_Email_DMO ph1 in Email_Idresult)
                {
                    temparr.Add(ph1.HRMEM_EmailId);
                }
                EmailIds = string.Join(", ", temparr.ToArray());
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee);
            }

            return EmailIds;
        }
        public EmployeeServiceRecordReportDTO get_depts(EmployeeServiceRecordReportDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public EmployeeServiceRecordReportDTO get_desig(EmployeeServiceRecordReportDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
