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
    public class EmployeeProfileReportService : Interfaces.EmployeeProfileReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeProfileReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public EmployeeProfileReportDTO getBasicData(EmployeeProfileReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public EmployeeProfileReportDTO GetAllDropdownAndDatatableDetails(EmployeeProfileReportDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            //   List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            List<MasterEmployee> emp = new List<MasterEmployee>();
            try
            {
                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                            ).ToList();

                if (PROCESSList.Count() > 0)
                {
                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    emp = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = emp.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {
                    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();
                }


                //emp = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                //dto.employeedropdown = emp.ToArray();


                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

                //// employee grouptype
                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public EmployeeProfileReportDTO FilterEmployeedetailsBySelection(EmployeeProfileReportDTO dto)
        {
            List<MasterEmployee> employe = new List<MasterEmployee>();
            try
            {
                if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();

                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                }

                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }

                dto.employeedropdown = employe.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public async Task<EmployeeProfileReportDTO> getEmployeedetailsBySelection(EmployeeProfileReportDTO dto)
        {
            try
            {
                List<EmployeeProfileReportDTO> AllInOnet = new List<EmployeeProfileReportDTO>();
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                for (int empid = 0; empid < dto.hrmE_multiId.Length;empid ++)
                {
                    dto.HRME_Id = dto.hrmE_multiId[empid];
                    EmployeeProfileReportDTO objprofile = new EmployeeProfileReportDTO();
                    var employequalification = (from qualification in _HRMSContext.Master_Employee_Qulaification
                                                from course in _HRMSContext.HR_Master_Course
                                                where (qualification.MI_Id.Equals(dto.MI_Id) &&
                                                course.HRMC_Id.Equals(qualification.HRMC_Id) &&
                                                qualification.HRME_Id == dto.HRME_Id)
                                                select new EmployeeProfileReportDTO
                                                {
                                                    HRMC_QulaificationName = course.HRMC_QulaificationName,
                                                    HRME_QualificationName = qualification.HRME_QualificationName,
                                                    HRMEQ_UniversityName = qualification.HRMEQ_UniversityName,
                                                    HRMEQ_CollegeName = qualification.HRMEQ_CollegeName,
                                                    HRMEQ_AreaOfSpecialisation = qualification.HRMEQ_AreaOfSpecialisation,
                                                    HRMEQ_YearOfPassing = qualification.HRMEQ_YearOfPassing
                                                }).ToList();

                    var employeedocument = (from doc in _HRMSContext.Master_Employee_Documents
                                            where (doc.MI_Id.Equals(dto.MI_Id) && doc.HRME_Id.Equals(dto.HRME_Id))
                                            select new EmployeeProfileReportDTO
                                            {
                                                HRMEDS_DocumentName = doc.HRMEDS_DocumentName,
                                                HRMEDS_DocumentImageName = doc.HRMEDS_DocumentImageName,
                                                HRMEDS_DucumentDescription = doc.HRMEDS_DucumentDescription
                                            }).ToList();

                    var asmay_iid = _HRMSContext.AcademicYear.Where(t => t.MI_Id == dto.MI_Id && t.Is_Active == true && t.ASMAY_From_Date <= DateTime.Today && t.ASMAY_To_Date >= DateTime.Today).FirstOrDefault().ASMAY_Id;

                    var employeeclasssubject = (from a in _HRMSContext.TT_Final_GenerationDMO
                                                from b in _HRMSContext.TT_Final_Generation_DetailedDMO
                                                from c in _HRMSContext.AdmissionClass
                                                from d in _HRMSContext.School_M_Section
                                                from e in _HRMSContext.subjectmasterDMO
                                                where (a.MI_Id == dto.MI_Id && a.ASMAY_Id == asmay_iid && a.TTFG_Id == b.TTFG_Id && b.HRME_Id == dto.HRME_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && e.ISMS_Id == b.ISMS_Id)
                                                select new EmployeeProfileReportDTO
                                                {
                                                    ASMCL_Id = c.ASMCL_Id,
                                                    ASMCL_ClassName = c.ASMCL_ClassName,
                                                    ASMS_Id = d.ASMS_Id,
                                                    ASMC_SectionName = d.ASMC_SectionName,
                                                    ISMS_Id = e.ISMS_Id,
                                                    ISMS_SubjectName = e.ISMS_SubjectName
                                                }).Distinct().ToList();

                    String MobileNumbers = GetEmployeeMobileNumbers(dto.HRME_Id);
                    String EmailIds = GetEmployeeEmailIds(dto.HRME_Id);
                    var currentEmp = (from a in _HRMSContext.MasterEmployee
                                      from b in _HRMSContext.HR_Master_GroupType
                                      from c in _HRMSContext.HR_Master_Designation
                                      from d in _HRMSContext.HR_Master_Department
                                      from e in _HRMSContext.IVRM_Master_Gender
                                      from f in _HRMSContext.Caste
                                      from g in _HRMSContext.Religion
                                      where (a.HRMGT_Id == b.HRMGT_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMD_Id == d.HRMD_Id && a.IVRMMG_Id == e.IVRMMG_Id && a.CasteId == f.IMC_Id && a.ReligionId == g.IVRMMR_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_Id == dto.HRME_Id)
                                      select new EmployeeServiceRecordReportDTO
                                      {
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

                    objprofile.currentemployeeDetails = currentEmp;
                    var DesignationName = _HRMSContext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_Id.Equals(currentEmp.HRMDES_Id)).HRMDES_DesignationName;
                    objprofile.DesignationName = DesignationName;

                    objprofile.employequalification = employequalification.ToArray();
                    objprofile.employeedocument = employeedocument.ToArray();
                    objprofile.employeeclasssubject = employeeclasssubject.ToArray();
                    AllInOnet.Add(objprofile);
                }
                dto.ArrayempsList = AllInOnet.ToArray();
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
        public EmployeeProfileReportDTO get_depts(EmployeeProfileReportDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id  && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeProfileReportDTO get_desig(EmployeeProfileReportDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id  && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
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
