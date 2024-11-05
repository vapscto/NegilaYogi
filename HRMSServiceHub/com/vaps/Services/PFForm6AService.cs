using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class PFForm6AService : Interfaces.PFForm6AInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public PFForm6AService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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

                //    //emptype
                //    dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

                //    //employee  
                //    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                //    // employee grouptype
                //    dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                //    //departmentdropdown
                //    dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                //    //designationdropdown 
                //    dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).ToArray();


                //    // earning , deduction details

                //    //Earning list
                //    dto.earningdropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Earning") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();


                //    //Deduction List
                //    dto.detectiondropdown = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction") && t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToArray();

                //    //leave year
                //    dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).ToArray();

                

                //  dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();
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
            //List<PFReportsDTO> cumDTOList = new List<PFReportsDTO>();
            try
            {
                List<HR_Employee_Salary> employeeIds = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id == dto.MI_Id && (t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate)).ToList();

                var empIds = employeeIds.Select(t => t.HRME_Id).Distinct();

              //  var FinYearDetails = _HRMSContext.IVRM_Master_FinancialYear.Where(t => t.IMFY_FromDate >= Convert.ToDateTime(dto.FromDate) && t.IMFY_ToDate <= Convert.ToDateTime(dto.ToDate)).FirstOrDefault();

                var FinYearDetails = _HRMSContext.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(dto.FromDate) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(dto.ToDate)).FirstOrDefault();

                if (FinYearDetails != null)
                {
                    dto.finYearFromDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.IMFY_FromDate));

                    dto.finYearToDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.IMFY_ToDate));
                }
                else
                {
                    dto.finYearFromDate = "";
                    dto.finYearToDate = "";
                }

                foreach (var empId in empIds)
                {
                    PFReportsDTO ss = new PFReportsDTO();
                    List<PFReportsDTO> alldataInner = new List<PFReportsDTO>();



                    List<HR_Employee_Salary> employeeSalaryDetailss = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id == dto.MI_Id && (t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate) && t.HRME_Id == empId).ToList();

                    ss.HRES_EPF = employeeSalaryDetailss.Sum(t => t.HRES_EPF);
                    ss.HRES_FPF = employeeSalaryDetailss.Sum(t => t.HRES_FPF);
                    MasterEmployee employeeDetailss = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empId).FirstOrDefault();

                    ss.HRME_EmployeeFirstName = employeeDetailss.HRME_EmployeeFirstName;
                    ss.HRME_EmployeeMiddleName = employeeDetailss.HRME_EmployeeMiddleName;
                    ss.HRME_EmployeeLastName = employeeDetailss.HRME_EmployeeLastName;
                    ss.HRME_PFAccNo = employeeDetailss.HRME_PFAccNo;


                    foreach (var employeeSalary in employeeSalaryDetailss)
                    {

                        PFReportsDTO ssInner = new PFReportsDTO();

                        var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                           from HRES in _HRMSContext.HR_Employee_Salary
                                           from hrme in _HRMSContext.MasterEmployee
                                               // from HREED in _HRMSContext.HR_Employee_EarningsDeductions
                                           from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                           where (HRESD.HRES_Id == HRES.HRES_Id && HRES.HRME_Id == hrme.HRME_Id && hrme.HRME_PFApplicableFlag == true &&
                                           // (HREED.HRMED_Id == HRESD.HRMED_Id && HREED.HRME_Id == HRES.HRME_Id) &&
                                           HRESD.HRMED_Id == HRMED.HRMED_Id

                                         && HRESD.HRES_Id == employeeSalary.HRES_Id) //checking condition
                                           select new PFReportsDTO
                                           {

                                               HRESD_Id = HRESD.HRESD_Id,
                                               HRES_Id = HRES.HRES_Id,
                                               HRMED_Id = HRESD.HRMED_Id,
                                               HRMED_Name = HRMED.HRMED_Name,
                                               HRESD_Amount = HRESD.HRESD_Amount,
                                           }).ToList();



                        if (currentdata.Count() > 0)
                        {

                            decimal? BasicPayHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);
                            decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("DA")).Sum(t => t.HRESD_Amount);


                            decimal? PFAmount = currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount);

                            decimal? AmountofWages = BasicPayHRESD_Amount + DAHRESD_Amount;

                            ssInner.AmountofWages = AmountofWages;
                            ssInner.PFAmount = PFAmount;


                            alldataInner.Add(ssInner);

                        }
                        else
                        {
                            ssInner.AmountofWages = 0;
                            ssInner.PFAmount = 0;

                            alldataInner.Add(ssInner);

                        }

                    }

                    ss.AmountofWages = alldataInner.Sum(t=>t.AmountofWages);
                    ss.PFAmount = alldataInner.Sum(t => t.PFAmount);

                    alldata.Add(ss);


                }

                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();

                dto.pfreport = alldata.ToArray();
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
            //List<PFReportsDTO> cumDTOList = new List<PFReportsDTO>();
            try
            {
                List<HR_Employee_Salary> employeeIds = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id == dto.MI_Id && (t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate)).ToList();

                var empIds = employeeIds.Select(t => t.HRME_Id).Distinct();

                //  var FinYearDetails = _HRMSContext.IVRM_Master_FinancialYear.Where(t => t.IMFY_FromDate >= Convert.ToDateTime(dto.FromDate) && t.IMFY_ToDate <= Convert.ToDateTime(dto.ToDate)).FirstOrDefault();

                var FinYearDetails = _HRMSContext.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(dto.FromDate) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(dto.ToDate)).FirstOrDefault();

                if (FinYearDetails != null)
                {
                    dto.finYearFromDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.IMFY_FromDate));

                    dto.finYearToDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.IMFY_ToDate));
                }
                else
                {
                    dto.finYearFromDate = "";
                    dto.finYearToDate = "";
                }

                foreach (var empId in empIds)
                {
                    PFReportsDTO ss = new PFReportsDTO();
                    List<PFReportsDTO> alldataInner = new List<PFReportsDTO>();
                    List<HR_Employee_Salary> employeeSalaryDetailss = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id == dto.MI_Id && (t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate) && t.HRME_Id == empId).ToList();

                    ss.HRES_EPF = employeeSalaryDetailss.Sum(t => t.HRES_EPF);
                    ss.HRES_FPF = employeeSalaryDetailss.Sum(t => t.HRES_FPF);
                    MasterEmployee employeeDetailss = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empId).FirstOrDefault();

                    ss.HRME_EmployeeFirstName = employeeDetailss.HRME_EmployeeFirstName;
                    ss.HRME_EmployeeMiddleName = employeeDetailss.HRME_EmployeeMiddleName;
                    ss.HRME_EmployeeLastName = employeeDetailss.HRME_EmployeeLastName;
                    ss.HRME_PFAccNo = employeeDetailss.HRME_PFAccNo;
                    ss.HRME_DOJ = employeeDetailss.HRME_DOJ;
                    ss.HRME_EmployeeCode = employeeDetailss.HRME_EmployeeCode;
                    ss.HRMS_Age = DateTime.Now.Year - employeeDetailss.HRME_DOB.Value.Date.Year;
                    ss.HRME_FPFNotApplicableFlg = employeeDetailss.HRME_FPFNotApplicableFlg;


                    foreach (var employeeSalary in employeeSalaryDetailss)
                    {
                        PFReportsDTO ssInner = new PFReportsDTO();
                        var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                           from HRES in _HRMSContext.HR_Employee_Salary
                                           // from HREED in _HRMSContext.HR_Employee_EarningsDeductions
                                           from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                           where (HRESD.HRES_Id == HRES.HRES_Id &&
                                           // (HREED.HRMED_Id == HRESD.HRMED_Id && HREED.HRME_Id == HRES.HRME_Id) &&
                                           HRESD.HRMED_Id == HRMED.HRMED_Id
                                           && HRESD.HRES_Id == employeeSalary.HRES_Id) //checking condition
                                           select new PFReportsDTO
                                           {
                                               HRESD_Id = HRESD.HRESD_Id,
                                               HRES_Id = HRES.HRES_Id,
                                               HRMED_Id = HRESD.HRMED_Id,
                                               HRMED_Name = HRMED.HRMED_Name,
                                               HRESD_Amount = HRESD.HRESD_Amount,
                                           }).ToList();

                        if (currentdata.Count() > 0)
                        {
                            decimal? BasicPayHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);
                            decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("DA")).Sum(t => t.HRESD_Amount);
                            decimal? PP_Amount = currentdata.Where(t => t.HRMED_Name.Equals("PERSONAL PAY")).Sum(t => t.HRESD_Amount);
                            decimal? CL_Amount = currentdata.Where(t => t.HRMED_Name.Equals("CL AMT")).Sum(t => t.HRESD_Amount);
                            decimal? PFAmount = currentdata.Where(t => t.HRMED_Name.Equals("P F")).Sum(t => t.HRESD_Amount);
                            decimal? AmountofWages = BasicPayHRESD_Amount + DAHRESD_Amount + PP_Amount + CL_Amount;
                            ssInner.AmountofWages = AmountofWages;
                            ssInner.PFAmount = PFAmount;
                            alldataInner.Add(ssInner);
                        }
                        else
                        {
                            ssInner.AmountofWages = 0;
                            ssInner.PFAmount = 0;
                            alldataInner.Add(ssInner);
                        }
                    }

                    ss.AmountofWages = alldataInner.Sum(t => t.AmountofWages);
                    ss.PFAmount = alldataInner.Sum(t => t.PFAmount);
                    alldata.Add(ss);
                }

                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.pfreport = alldata.ToArray();
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
