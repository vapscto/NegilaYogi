using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    //
    public class PFForm5stopPensionSTJamesReportIMPL : Interfaces.PFForm5stopPensionSTJamesReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public PFForm5stopPensionSTJamesReportIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public PFReportsDTO getBasicData(PFReportsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public PFReportsDTO GetAllDropdownAndDatatableDetails(PFReportsDTO dto)
        {
            try
            {

                //employee  
                dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                //leave year
                dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public PFReportsDTO FilterEmployeeData(PFReportsDTO dto)
        {
            //List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();
            //if (dto.FormatType.Equals("Format1"))
            //{
            //  if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();

            //  }
            //  else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
            //  }
            //  else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
            //  }
            //  else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
            //  }
            //  else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id)).ToList();
            //  }
            //  else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
            //  }
            //  else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
            //  {
            //    //employee
            //    employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
            //  }
            //  if (dto.MonthBetweenDates.Equals("Month"))
            //  {
            //    employeeDetails = employeeDetails.Where(t => t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();
            //  }
            //  else if (dto.MonthBetweenDates.Equals("BetweenDates"))
            //  {
            //    employeeDetails = employeeDetails.Where(t => t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
            //  }
            //  if (employeeDetails.Count() > 0)
            //  {
            //    var empIdList = employeeDetails.Select(t => t.HRME_Id);
            //    var employeedropdown = (from a in _HRMSContext.MasterEmployee
            //                            from b in _HRMSContext.HR_Employee_Salary
            //                            where a.HRME_Id.Equals(b.HRME_Id)
            //                            && b.MI_Id.Equals(dto.MI_Id)
            //                            && a.HRME_ActiveFlag == true && empIdList.Contains(a.HRME_Id)
            //                            orderby a.HRME_EmployeeOrder
            //                            select a).Distinct();
            //    dto.employeedropdown = employeedropdown.ToArray();
            //  }
            //}

            return dto;
        }

        public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto)
        {

            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();
            try
            {
                //get month id by month name
                long IVRM_Month_Id = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).FirstOrDefault().IVRM_Month_Id;


                //employeeDetails = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_ActiveFlag == true).ToList();
                employeeDetails = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_PensionStoppedDate).Month + 1 == IVRM_Month_Id && Convert.ToDateTime(u.HRME_PensionStoppedDate).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_ActiveFlag == true && u.HRME_LeftFlag == false && u.HRME_RetiredFlg == true).ToList();
                foreach (var employee in employeeDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();
                    string FatherHusbandName = "";

                    //Employee Details

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == employee.HRME_Id).ToList();
                    var gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_Id.Equals(employee.IVRMMG_Id)).Select(t => t.IVRMMG_GenderName);

                    if (gender.FirstOrDefault().Equals("Female"))
                    {
                        var married = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_Id == employee.IVRMMMS_Id).Select(t => t.IVRMMMS_MaritalStatus);

                        if (married.FirstOrDefault().Equals("Married"))
                        {
                            FatherHusbandName = employee.HRME_SpouseName;
                        }
                        else
                        {
                            FatherHusbandName = employee.HRME_FatherName;
                        }
                    }
                    else
                    {
                        FatherHusbandName = employee.HRME_FatherName;
                    }

                    ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                    ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                    ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                    ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                    ss.HRME_RetiredFlg = employeeDetails.FirstOrDefault().HRME_RetiredFlg;
                    ss.FatherHusbandName = FatherHusbandName;

                    //ss.HRME_DOL = employeeDetails.FirstOrDefault().HRME_DOL;
                    ss.HRME_LeavingReason = employeeDetails.FirstOrDefault().HRME_LeavingReason;


                    alldata.Add(ss);

                }

                dto.pfreport = alldata.ToArray();

                // institutionDetails
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();


                dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public PFReportsDTO getEmployeedetailsBySelectionStjames(PFReportsDTO dto)
        {

            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();
            try
            {
                //get month id by month name
                long IVRM_Month_Id = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).FirstOrDefault().IVRM_Month_Id;

                //employeeDetails = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_DOL).Month + 1 == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_ActiveFlag == true && (u.HRME_LeavingReason == "Retired" || u.HRME_LeavingReason == "Resigned")).ToList();

                employeeDetails = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_PensionStoppedDate).Month + 1 == IVRM_Month_Id && Convert.ToDateTime(u.HRME_PensionStoppedDate).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_ActiveFlag == true && u.HRME_LeftFlag == false && u.HRME_RetiredFlg == true).ToList();

                List<long> empids = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_PensionStoppedDate).Month + 1 == IVRM_Month_Id && Convert.ToDateTime(u.HRME_PensionStoppedDate).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == false && u.HRME_ActiveFlag == true).Select(t => t.HRME_Id).ToList();

                dto.HRC_RetirementYrs = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).FirstOrDefault().HRC_RetirementYrs;

                foreach (var employee in employeeDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();
                    string FatherHusbandName = "";

                    //Employee Details
                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == employee.HRME_Id).ToList();
                    var gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_Id.Equals(employee.IVRMMG_Id)).Select(t => t.IVRMMG_GenderName);

                    if (gender.FirstOrDefault().Equals("Female"))
                    {
                        var married = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_Id == employee.IVRMMMS_Id).Select(t => t.IVRMMMS_MaritalStatus);
                        if (married.FirstOrDefault().Equals("Married"))
                        {
                            FatherHusbandName = employee.HRME_SpouseName;
                        }
                        else
                        {
                            FatherHusbandName = employee.HRME_FatherName;
                        }
                    }
                    else
                    {
                        FatherHusbandName = employee.HRME_FatherName;
                    }

                    ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                    ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                    ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                    ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                    ss.FatherHusbandName = FatherHusbandName;
                    ss.HRME_RetiredFlg = employeeDetails.FirstOrDefault().HRME_RetiredFlg;
                    ss.HRME_PensionStoppedDate = employeeDetails.FirstOrDefault().HRME_PensionStoppedDate;
                    //ss.HRME_DOL = employeeDetails.FirstOrDefault().HRME_DOL;
                    ss.HRME_LeavingReason = employeeDetails.FirstOrDefault().HRME_LeavingReason;
                    //ss.HRME_EmployeeCode = "Left";
                    alldata.Add(ss);
                }

                //List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_Year.Equals(dto.HRES_Year) && t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id) && !empids.Contains(t.HRME_Id)).ToList();
                string alternatemonth = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Id == IVRM_Month_Id - 1).FirstOrDefault().IVRM_Month_Name;

                List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(alternatemonth) && t.HRES_Year.Equals(dto.HRES_Year) && t.MI_Id.Equals(dto.MI_Id) && !empids.Contains(t.HRME_Id)).ToList();

                foreach (var employee in employeeSalaryDetails)
                {
                    var birthdayss = (from emp in _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == employee.HRME_Id && t.MI_Id == dto.MI_Id)
                                      select new PFReportsDTO
                                      {
                                          H_DOB = DateTime.Now.Year - emp.HRME_DOB.Value.Date.Year,
                                          H_Month = IVRM_Month_Id - emp.HRME_DOB.Value.Date.Month
                                      }).ToList();

                    dto.abc = Convert.ToInt32(birthdayss.Sum(t => t.H_DOB));
                    dto.H_Month = Convert.ToInt32(birthdayss.Sum(t => t.H_Month));

                    if (dto.abc == dto.HRC_RetirementYrs && dto.H_Month == 1)
                    {
                        employeeDetails = _HRMSContext.MasterEmployee.Where(u => u.HRME_Id == employee.HRME_Id).ToList();

                        PFReportsDTO sss = new PFReportsDTO();
                        string FatherHusbandName = "";

                        employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == employee.HRME_Id).ToList();
                        var gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_Id.Equals(employeeDetails[0].IVRMMG_Id)).Select(t => t.IVRMMG_GenderName);

                        if (gender.FirstOrDefault().Equals("Female"))
                        {
                            var married = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_Id == employeeDetails[0].IVRMMMS_Id).Select(t => t.IVRMMMS_MaritalStatus);
                            if (married.FirstOrDefault().Equals("Married"))
                            {
                                FatherHusbandName = employeeDetails[0].HRME_SpouseName;
                            }
                            else
                            {
                                FatherHusbandName = employeeDetails[0].HRME_FatherName;
                            }
                        }
                        else
                        {
                            FatherHusbandName = employeeDetails[0].HRME_FatherName;
                        }

                        sss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                        sss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                        sss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                        sss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                        sss.FatherHusbandName = FatherHusbandName;
                       // sss.HRME_DOL = employeeDetails.FirstOrDefault().HRME_DOL;
                        sss.HRME_LeavingReason = employeeDetails.FirstOrDefault().HRME_LeavingReason;

                        alldata.Add(sss);
                    }
                }

                dto.pfreport = alldata.ToArray();
                // institutionDetails
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

    }
}
