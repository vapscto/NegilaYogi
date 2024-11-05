using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EPFcontributionRegisterService : Interfaces.EPFcontributionRegisterInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EPFcontributionRegisterService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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
                //  dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
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

                // List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_Year.Equals(dto.HRES_Year) && t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                List<HR_Employee_Salary> employeeSalaryDetails = (from a in _HRMSContext.HR_Employee_Salary
                                                                  from b in _HRMSContext.MasterEmployee
                                                                  where (a.HRME_Id == b.HRME_Id && a.HRES_Month.Equals(dto.HRES_Month) && a.HRES_Year.Equals(dto.HRES_Year) && a.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(a.HRMDES_Id) && dto.hrmD_IdList.Contains(a.HRMD_Id) && dto.groupTypeIdList.Contains(a.HRMGT_Id) && b.HRME_PFApplicableFlag == true)
                                                                  select a).ToList();

                foreach (var empSalary in employeeSalaryDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id).ToList();
                    // var agefac = employeeDetails.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year);
                    var agefactor = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => Convert.ToInt32(t.HRC_RetirementYrs));

                    dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                    dto.HRC_RetirementYrs = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).Select(t => t.HRC_RetirementYrs).FirstOrDefault();

                    //   int hhhh = _HRMSContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year && t.);


                    var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                      from mas in _HRMSContext.HR_Master_EarningsDeductions
                                      where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == empSalary.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id)
                                      select new PFReportsDTO
                                      {
                                          HRESD_Amount = emp.HREED_Amount

                                      }).ToList();

                    dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));


                    var birthdays = (from emp in _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id && t.MI_Id == dto.MI_Id)
                                     select new PFReportsDTO
                                     {
                                         H_DOB = DateTime.Now.Year - emp.HRME_DOB.Value.Date.Year

                                     }).ToList();

                    dto.abc = Convert.ToInt32(birthdays.Sum(t => t.H_DOB));

                    //if (dto.abc <= 58)
                    if (dto.abc <= dto.HRC_RetirementYrs)
                    {
                        var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                           from HRES in _HRMSContext.HR_Employee_Salary
                                           from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                           where (HRESD.HRES_Id == HRES.HRES_Id &&
                                           HRESD.HRMED_Id == HRMED.HRMED_Id
                                         && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
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

                            long netsalary = Convert.ToInt64(dto.empGrossSal);
                            long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount));

                            if (PFAmount >= 0)
                            {

                                long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);

                                ss.basicvalue = BasicPayHRESD_Amount;
                                ss.davalue = DAHRESD_Amount;

                                ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;

                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = AmountofWages;
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                if (ss.AmountofWages <= 15000)
                                {
                                    var adg = 0.01;
                                    ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                }


                                else if (ss.AmountofWages > 15000)
                                {
                                    //var adg = 0.1;
                                    ss.HRES_Ac5 = 150;
                                }
                                ss.PFAmount = PFAmount;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_FPF);
                                ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                ss.HRES_Ac22 = (empSalary.HRES_Ac22);


                                // ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                alldata.Add(ss);


                            }
                            else { }
                        }
                        else
                        {
                            ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                            ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                            ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                            ss.netsalary = dto.empGrossSal;
                            ss.basicvalue = 0;
                            ss.davalue = 0;
                            ss.AmountofWages = 0;
                            ss.PFAmount = 0;
                            ss.HRES_EPF = (empSalary.HRES_EPF);
                            ss.HRES_FPF = (empSalary.HRES_FPF);
                            ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                            ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                            ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                            ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                            alldata.Add(ss);

                        }
                    }
                    else
                    {
                        var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                           from HRES in _HRMSContext.HR_Employee_Salary
                                           from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                           where (HRESD.HRES_Id == HRES.HRES_Id &&
                                           HRESD.HRMED_Id == HRMED.HRMED_Id
                                         && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
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

                            long netsalary = Convert.ToInt64(dto.empGrossSal);
                            long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount));

                            if (PFAmount > 0)
                            {

                                long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);
                                ss.basicvalue = BasicPayHRESD_Amount;
                                ss.davalue = DAHRESD_Amount;

                                ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;

                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = AmountofWages;
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);

                                if (ss.AmountofWages <= 15000)
                                {
                                    var adg = 0.01;
                                    ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                }


                                else if (ss.AmountofWages > 15000)
                                {
                                    //var adg = 0.1;
                                    ss.HRES_Ac5 = 150;
                                }
                                ss.PFAmount = PFAmount;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_FPF);
                                //ss.HRES_EPF = PFAmount;
                                //ss.HRES_FPF = PFAmount;
                                ss.HRES_Ac21 = 0;
                                ss.HRES_Ac22 = 0;
                                //ss.HRES_Ac5 = 0;
                                alldata.Add(ss);
                            }
                            else { }
                        }
                        else
                        {
                            ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                            ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                            ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                            ss.netsalary = dto.empGrossSal;
                            ss.basicvalue = 0;
                            ss.davalue = 0;
                            ss.AmountofWages = 0;
                            ss.PFAmount = 0;
                            ss.HRES_EPF = (empSalary.HRES_EPF);
                            ss.HRES_FPF = (empSalary.HRES_EPF);
                            ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                            ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                            ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                            ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                            alldata.Add(ss);

                        }
                    }
                }

                dto.pfreport = alldata.OrderBy(t => t.HRME_PFAccNo).ToArray();

                // dto.FatherHusbandName = 
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public PFReportsDTO getEmployeedetailsBySelectionBBKV(PFReportsDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();

            try
            {

                List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_Year.Equals(dto.HRES_Year) && t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                foreach (var empSalary in employeeSalaryDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id).ToList();
                    // var agefac = employeeDetails.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year);
                    var agefactor = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => Convert.ToInt32(t.HRC_RetirementYrs));

                    dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                    dto.HRC_RetirementYrs = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).Select(t => t.HRC_RetirementYrs).FirstOrDefault();

                    //   int hhhh = _HRMSContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year && t.);


                    var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                      from mas in _HRMSContext.HR_Master_EarningsDeductions
                                      where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == empSalary.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id)
                                      select new PFReportsDTO
                                      {
                                          HRESD_Amount = emp.HREED_Amount

                                      }).ToList();

                    dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));


                    var birthdays = (from emp in _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id && t.MI_Id == dto.MI_Id)
                                     select new PFReportsDTO
                                     {
                                         H_DOB = DateTime.Now.Year - emp.HRME_DOB.Value.Date.Year

                                     }).ToList();

                    dto.abc = Convert.ToInt32(birthdays.Sum(t => t.H_DOB));

                    //if (dto.abc <= 58)
                    if (dto.abc <= dto.HRC_RetirementYrs)
                    {
                        var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                           from HRES in _HRMSContext.HR_Employee_Salary
                                           from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                           where (HRESD.HRES_Id == HRES.HRES_Id &&
                                           HRESD.HRMED_Id == HRMED.HRMED_Id
                                         && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
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

                            long netsalary = Convert.ToInt64(dto.empGrossSal);
                            long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount));

                            if (PFAmount >= 0)
                            {

                                long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);

                                ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;

                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = AmountofWages;
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                if (ss.AmountofWages <= 15000)
                                {
                                    var adg = 0.01;
                                    ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                }


                                else if (ss.AmountofWages > 15000)
                                {
                                    //var adg = 0.1;
                                    ss.HRES_Ac5 = 150;
                                    ss.AmountofWages = 15000;
                                }
                                ss.PFAmount = PFAmount;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_FPF);
                                ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                ss.HRES_Ac22 = (empSalary.HRES_Ac22);


                                // ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                alldata.Add(ss);


                            }
                            else { }
                        }
                        else
                        {
                            ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                            ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                            ss.netsalary = dto.empGrossSal;
                            ss.AmountofWages = 0;
                            ss.PFAmount = 0;
                            ss.HRES_EPF = (empSalary.HRES_EPF);
                            ss.HRES_FPF = (empSalary.HRES_FPF);
                            ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                            ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                            ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                            ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                            alldata.Add(ss);

                        }
                    }


                    else
                    {
                        var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                           from HRES in _HRMSContext.HR_Employee_Salary
                                           from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                           where (HRESD.HRES_Id == HRES.HRES_Id &&
                                           HRESD.HRMED_Id == HRMED.HRMED_Id
                                         && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
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

                            long netsalary = Convert.ToInt64(dto.empGrossSal);
                            long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount));

                            if (PFAmount > 0)
                            {

                                long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);

                                ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;

                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = AmountofWages;
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);

                                if (ss.AmountofWages <= 15000)
                                {
                                    var adg = 0.01;
                                    ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                }


                                else if (ss.AmountofWages > 15000)
                                {
                                    //var adg = 0.1;
                                    ss.HRES_Ac5 = 150;
                                    ss.AmountofWages = 15000;
                                }
                                ss.PFAmount = PFAmount;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_FPF);
                                //ss.HRES_EPF = PFAmount;
                                //ss.HRES_FPF = PFAmount;
                                ss.HRES_Ac21 = 0;
                                ss.HRES_Ac22 = 0;
                                //ss.HRES_Ac5 = 0;
                                alldata.Add(ss);

                            }
                            else { }
                        }
                        else
                        {
                            ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                            ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                            ss.netsalary = dto.empGrossSal;
                            ss.AmountofWages = 0;
                            ss.PFAmount = 0;
                            ss.HRES_EPF = (empSalary.HRES_EPF);
                            ss.HRES_FPF = (empSalary.HRES_EPF);
                            ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                            ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                            ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                            ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                            alldata.Add(ss);

                        }
                    }



                }


                dto.pfreport = alldata.OrderBy(t => t.HRME_PFAccNo).ToArray();

                // dto.FatherHusbandName = 
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public PFReportsDTO getEmployeedetailsBySelectionStJames(PFReportsDTO dto)
        {
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();

            try
            {

                List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_Year.Equals(dto.HRES_Year) && t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                foreach (var empSalary in employeeSalaryDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();

                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id && t.HRME_PFAccNo != null).ToList();
                    if (employeeDetails.Count > 0)
                    {

                        // var agefac = employeeDetails.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year);
                        var agefactor = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => Convert.ToInt32(t.HRC_RetirementYrs));

                        string departmentname = _HRMSContext.HR_Master_Department.Where(t => t.HRMD_Id == employeeDetails[0].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();

                        dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                        dto.HRC_RetirementYrs = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).Select(t => t.HRC_RetirementYrs).FirstOrDefault();

                        //   int hhhh = _HRMSContext.MasterEmployee.Where(t => t.HRME_DOB.Value.Date.Year == DateTime.Now.Year && t.);

                        var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                          from mas in _HRMSContext.HR_Master_EarningsDeductions
                                          where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == empSalary.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id)
                                          select new PFReportsDTO
                                          {
                                              HRESD_Amount = emp.HREED_Amount
                                          }).ToList();

                        dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                        var emptotDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                         from mas in _HRMSContext.HR_Master_EarningsDeductions
                                         where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == empSalary.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.MI_Id == dto.MI_Id)
                                         select new PFReportsDTO
                                         {
                                             HRESD_Amount = emp.HREED_Amount
                                         }).ToList();

                        dto.emptotdedSal = Convert.ToDecimal(emptotDed.Sum(t => t.HRESD_Amount));

                        var birthdays = (from emp in _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empSalary.HRME_Id && t.MI_Id == dto.MI_Id)
                                         select new PFReportsDTO
                                         {
                                             H_DOB = DateTime.Now.Year - emp.HRME_DOB.Value.Date.Year
                                         }).ToList();

                        dto.abc = Convert.ToInt32(birthdays.Sum(t => t.H_DOB));
                        //DateTime dt1 = DateTime.Parse(employeeDetails.FirstOrDefault().HRME_DOB.ToString());
                        //DateTime dt2 = DateTime.Parse("01/" + dto.HRES_Month + "/" + dto.HRES_Year);
                        //ss.HRME_Age = (dt2 - dt1).Days / 365;


                        
                        //ss.HRME_Age = empSalary.HRES_FromDate.Value.Date.Year - employeeDetails.FirstOrDefault().HRME_DOB.Value.Date.Year;
                        //if (dto.abc <= 58)
                        if (dto.abc <= dto.HRC_RetirementYrs)
                        {
                            var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                               from HRES in _HRMSContext.HR_Employee_Salary
                                               from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                               where (HRESD.HRES_Id == HRES.HRES_Id && HRESD.HRMED_Id == HRMED.HRMED_Id && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
                                               select new PFReportsDTO
                                               {
                                                   HRESD_Id = HRESD.HRESD_Id,
                                                   HRES_Id = HRES.HRES_Id,
                                                   HRMED_Id = HRESD.HRMED_Id,
                                                   HRMED_Name = HRMED.HRMED_Name,
                                                   HRESD_Amount = HRESD.HRESD_Amount
                                               }).ToList();

                            if (currentdata.Count() > 0)
                            {
                                decimal? BasicPayHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);
                                decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("DA")).Sum(t => t.HRESD_Amount);
                                decimal? OTHERS_Amount = currentdata.Where(t => t.HRMED_Name.Equals("PERSONAL PAY")).Sum(t => t.HRESD_Amount);
                                decimal? CLPAY = currentdata.Where(t => t.HRMED_Name.Equals("CL AMT")).Sum(t => t.HRESD_Amount);
                                dto.empGrossSal = BasicPayHRESD_Amount + DAHRESD_Amount + OTHERS_Amount + CLPAY;
                                long netsalary = Convert.ToInt64(dto.empGrossSal);
                                long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("P F")).Sum(t => t.HRESD_Amount));
                                long VPFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("V PF")).Sum(t => t.HRESD_Amount));

                                ss.basicamount = Convert.ToInt64(BasicPayHRESD_Amount);
                                ss.DAamount = Convert.ToInt64(DAHRESD_Amount);
                                ss.Othersamount = Convert.ToInt64(OTHERS_Amount + CLPAY);
                                ss.VPFAmount = VPFAmount;

                                if (netsalary > 15000) { ss.PFAmount = 15000; }
                                else { ss.PFAmount = netsalary; }

                                if (PFAmount >= 0)
                                {
                                    long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);
                                    ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;
                                    ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                    ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                    ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                    ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                    ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                    ss.departmentname = departmentname;
                                    ss.netsalary = dto.empGrossSal;
                                    ss.emptotdedSal = dto.emptotdedSal;
                                    ss.AmountofWages = AmountofWages;
                                    ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                    ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                    ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                    ss.HRES_WorkingDays = empSalary.HRES_WorkingDays;
                                    if (ss.AmountofWages <= 15000)
                                    {
                                        var adg = 0.01;
                                        ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                    }


                                    else if (ss.AmountofWages > 15000)
                                    {
                                        //var adg = 0.1;
                                        ss.HRES_Ac5 = 150;
                                    }
                                    ss.STJOwnPF = PFAmount;
                                    ss.HRES_EPF = (empSalary.HRES_EPF);
                                    ss.HRES_FPF = (empSalary.HRES_FPF);
                                    ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                    ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                                    ss.FatherHusbandName = dto.abc.ToString();
                                    // ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                    alldata.Add(ss);


                                }
                                else { }
                            }
                            else
                            {
                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = 0;
                                ss.PFAmount = 0;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_FPF);
                                ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                                ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                alldata.Add(ss);

                            }
                        }
                        else
                        {
                            var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                               from HRES in _HRMSContext.HR_Employee_Salary
                                               from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                               where (HRESD.HRES_Id == HRES.HRES_Id &&
                                               HRESD.HRMED_Id == HRMED.HRMED_Id
                                             && HRESD.HRES_Id == empSalary.HRES_Id) //checking condition
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
                                decimal? OTHERS_Amount = currentdata.Where(t => t.HRMED_Name.Equals("PERSONAL PAY")).Sum(t => t.HRESD_Amount);
                                decimal? CLPAY = currentdata.Where(t => t.HRMED_Name.Equals("CL AMT")).Sum(t => t.HRESD_Amount);
                                dto.empGrossSal = BasicPayHRESD_Amount + DAHRESD_Amount + OTHERS_Amount + CLPAY;
                                long netsalary = Convert.ToInt64(dto.empGrossSal);
                                long PFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("P F")).Sum(t => t.HRESD_Amount));
                                long VPFAmount = Convert.ToInt64(currentdata.Where(t => t.HRMED_Name.Equals("V PF")).Sum(t => t.HRESD_Amount));

                                ss.basicamount = Convert.ToInt64(BasicPayHRESD_Amount);
                                ss.DAamount = Convert.ToInt64(DAHRESD_Amount);
                                ss.Othersamount = Convert.ToInt64(OTHERS_Amount + CLPAY);
                                ss.VPFAmount = VPFAmount;
                                ss.STJOwnPF = PFAmount;
                                ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;

                                if (netsalary > 15000) { ss.PFAmount = 15000; }
                                else { ss.PFAmount = netsalary; }

                                if (PFAmount > 0)
                                {

                                    long AmountofWages = Convert.ToInt64(BasicPayHRESD_Amount + DAHRESD_Amount);

                                    ss.HRME_PFuAN = employeeDetails.FirstOrDefault().HRME_UINumber;
                                    ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                    ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                    ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                    ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                    ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                    ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                    ss.netsalary = dto.empGrossSal;
                                    ss.AmountofWages = AmountofWages;
                                    ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                    ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                    ss.VPFAmount = VPFAmount;

                                    if (ss.AmountofWages <= 15000)
                                    {
                                        var adg = 0.01;
                                        ss.HRES_Ac5 = ss.AmountofWages * Convert.ToDecimal(adg);
                                    }

                                    else if (ss.AmountofWages > 15000)
                                    {
                                        //var adg = 0.1;
                                        ss.HRES_Ac5 = 150;
                                    }
                                    //ss.PFAmount = PFAmount;
                                    ss.HRES_EPF = (empSalary.HRES_EPF);
                                    ss.HRES_FPF = (empSalary.HRES_FPF);
                                    //ss.HRES_EPF = PFAmount;
                                    //ss.HRES_FPF = PFAmount;
                                    ss.HRES_Ac21 = 0;
                                    ss.HRES_Ac22 = 0;
                                    //ss.HRES_Ac5 = 0;
                                    ss.FatherHusbandName = dto.abc.ToString();
                                    alldata.Add(ss);

                                }
                                else { }
                            }
                            else
                            {
                                ss.HRME_PFAccNo = employeeDetails.FirstOrDefault().HRME_PFAccNo;
                                ss.HRME_EmployeeFirstName = employeeDetails.FirstOrDefault().HRME_EmployeeFirstName;
                                ss.HRME_EmployeeMiddleName = employeeDetails.FirstOrDefault().HRME_EmployeeMiddleName;
                                ss.HRME_EmployeeLastName = employeeDetails.FirstOrDefault().HRME_EmployeeLastName;
                                ss.HRME_FPFNotApplicableFlg = employeeDetails.FirstOrDefault().HRME_FPFNotApplicableFlg;
                                ss.HRME_DOJ = employeeDetails.FirstOrDefault().HRME_DOJ;
                                ss.netsalary = dto.empGrossSal;
                                ss.AmountofWages = 0;
                                ss.PFAmount = 0;
                                ss.HRES_EPF = (empSalary.HRES_EPF);
                                ss.HRES_FPF = (empSalary.HRES_EPF);
                                ss.HRES_Ac21 = (empSalary.HRES_Ac21);
                                ss.HRES_Ac22 = (empSalary.HRES_Ac22);
                                ss.HRES_Ac5 = (empSalary.HRES_Ac5);
                                ss.abc = Convert.ToInt32(empSalary.HRES_WorkingDays);
                                ss.HRME_EmployeeCode = employeeDetails.FirstOrDefault().HRME_EmployeeCode;
                                alldata.Add(ss);
                            }
                        }


                        using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "HR_Employee_Age_Calculation";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                            {
                                Value = dto.HRES_Year
                            });
                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                            {
                                Value = dto.HRES_Month
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                            {
                                Value = empSalary.HRME_Id
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                            );
                                        }
                                        //retObject.Add((ExpandoObject)dataRow);


                                        ss.HRME_Age = Convert.ToInt64(dataReader["Age"]);
                                        ss.HRES_WorkingDays = Convert.ToInt64(dataReader["HRES_WorkingDays"]);


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

                dto.pfreport = alldata.OrderBy(t => t.departmentname).ToArray();

                // dto.FatherHusbandName = 
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }
    }
}
