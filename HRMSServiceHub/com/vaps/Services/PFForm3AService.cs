using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class PFForm3AService : Interfaces.PFForm3AInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public PFForm3AService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public PFReportsDTO getBasicData(PFReportsDTO dto)
        {
            CultureInfo us = new CultureInfo("en-US");
            var startDate = DateTime.ParseExact("04/01/2017", "MM/dd/yyyy", us);
            var endDate = DateTime.ParseExact("03/31/2018", "MM/dd/yyyy", us);

         

            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public static IEnumerable<Tuple<string, int>> MonthsBetween(
           DateTime startDate,
           DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year);
                iterator = iterator.AddMonths(1);
            }
        }

        public PFReportsDTO GetAllDropdownAndDatatableDetails(PFReportsDTO dto)
        {
            List<MasterEmployee> employe = new List<MasterEmployee>();
            try
            {
               


                    employe = (from a in _HRMSContext.MasterEmployee
                               from b in _HRMSContext.HR_Employee_Salary
                               where (a.HRME_Id == b.HRME_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_ActiveFlag == true)
                               select a).Distinct().ToList();


                    //  employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToList();
                    dto.employeedropdown = employe.ToArray();
                    //employee  
                   // dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();
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


            var FinancialYear = _HRMSContext.IVRM_Master_FinancialYear.ToList();




            //  List<HR_Employee_Salary> employeeDetails = new List<HR_Employee_Salary>();
            //  if (dto.FormatType.Equals("Format1"))
            //  {
            //    if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();

            //    }
            //    else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
            //    }
            //    else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
            //    }
            //    else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
            //    }
            //    else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id)).ToList();
            //    }
            //    else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
            //    }

            //    else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
            //    {
            //      //employee
            //      employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
            //    }

            //    if (dto.MonthBetweenDates.Equals("Month"))
            //    {
            //      employeeDetails = employeeDetails.Where(t => t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();

            //    }
            //    else if (dto.MonthBetweenDates.Equals("BetweenDates"))
            //    {
            //      employeeDetails = employeeDetails.Where(t => t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
            //    }

            //    if (employeeDetails.Count() > 0)
            //    {
            //      var empIdList = employeeDetails.Select(t => t.HRME_Id);

            //      var employeedropdown = (from a in _HRMSContext.MasterEmployee
            //                              from b in _HRMSContext.HR_Employee_Salary
            //                              where a.HRME_Id.Equals(b.HRME_Id)
            //                              && b.MI_Id.Equals(dto.MI_Id)
            //                              && a.HRME_ActiveFlag == true && empIdList.Contains(a.HRME_Id)

            //                              orderby a.HRME_EmployeeOrder

            //                              select a).Distinct();

            //      dto.employeedropdown = employeedropdown.ToArray();

            //    }

            //  }

            return dto;
            //}
        }

        public PFReportsDTO getEmployeedetailsBySelection(PFReportsDTO dto)
        {

            List < MasterEmployee> employeeDetails = new List<MasterEmployee>();
            Institution InstitutionDetails = new Institution();
            List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
            List<PFReportsDTO> alldata = new List<PFReportsDTO>();
            //List<PFReportsDTO> cumDTOList = new List<PFReportsDTO>();
            try
            {

                int leaveyear = Convert.ToInt32(dto.HRES_Year);
                int Nextleaveyear = leaveyear + 1;
                string finYear = leaveyear + "-" + Nextleaveyear;
                var FinYearDetails = _HRMSContext.IVRM_Master_FinancialYear.Where(t => t.IMFY_FinancialYear.Equals(finYear));
                if (FinYearDetails.Count() > 0)
                {
                    dto.finYearFromDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.FirstOrDefault().IMFY_FromDate));

                    dto.finYearToDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.FirstOrDefault().IMFY_ToDate));


                    var months = MonthsBetween(Convert.ToDateTime(FinYearDetails.FirstOrDefault().IMFY_FromDate), Convert.ToDateTime(FinYearDetails.FirstOrDefault().IMFY_ToDate));


                    foreach (var month in months)
                    {
                        string mon = month.Item1;
                        string year = month.Item2.ToString();
                        PFReportsDTO ss = new PFReportsDTO();
                        HR_Employee_Salary employeeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(mon) && t.HRES_Year.Equals(year) && t.HRME_Id == dto.HRME_Id).FirstOrDefault();
                        if (employeeSalary != null)
                        {

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
                                               HRMED_EDTypeFlag=   HRMED.HRMED_EDTypeFlag
                                               }).ToList();

                            if (currentdata.Count() > 0)
                            {

                                decimal? BasicPayHRESD_Amount = currentdata.Where(t=>t.HRMED_EDTypeFlag.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);

                                if (BasicPayHRESD_Amount ==null)
                                {
                                    BasicPayHRESD_Amount = 0;
                                }

                                decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_EDTypeFlag.Equals("DA")).Sum(t => t.HRESD_Amount);

                                if (DAHRESD_Amount == null)
                                {
                                    DAHRESD_Amount = 0;
                                }


                                decimal? PFAmount = currentdata.Where(t => t.HRMED_EDTypeFlag.Equals("PF")).Sum(t => t.HRESD_Amount);
                                if (PFAmount == null)
                                {
                                    PFAmount = 0;
                                }

                                decimal? AmountofWages = BasicPayHRESD_Amount + DAHRESD_Amount;

                                ss.AmountofWages = AmountofWages;
                                ss.PFAmount = PFAmount;
                                ss.HRES_EPF = employeeSalary.HRES_EPF;
                                ss.HRES_FPF = employeeSalary.HRES_FPF;
                                ss.HRES_Month = mon;
                                ss.HRES_Year = year;
                                alldata.Add(ss);

                            }
                            else
                            {
                                ss.AmountofWages = 0;
                                ss.PFAmount = 0;
                                ss.HRES_EPF = 0;
                                ss.HRES_FPF = 0;
                                ss.HRES_Month = mon;
                                ss.HRES_Year = year;

                                alldata.Add(ss);

                            }

                        }
                        else
                        {
                            ss.AmountofWages = 0;
                            ss.PFAmount = 0;
                            ss.HRES_EPF = 0;
                            ss.HRES_FPF = 0;
                            ss.HRES_Month = mon;
                            ss.HRES_Year = year;

                            alldata.Add(ss);
                        }


                    }
                   
                    employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).ToList();

                    dto.employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).ToArray();

                    var gender = _HRMSContext.IVRM_Master_Gender.Where(t => t.IVRMMG_Id.Equals(employeeDetails.FirstOrDefault().IVRMMG_Id)).Select(t=>t.IVRMMG_GenderName);

                    if (gender.FirstOrDefault().Equals("Female"))
                    {
                        var married = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.IVRMMMS_Id == employeeDetails.FirstOrDefault().IVRMMMS_Id).Select(t=>t.IVRMMMS_MaritalStatus);

                        if (married.FirstOrDefault().Equals("Married"))
                        {
                            dto.FatherHusbandName = employeeDetails.FirstOrDefault().HRME_SpouseName;
                        }
                        else
                        {
                            dto.FatherHusbandName = employeeDetails.FirstOrDefault().HRME_FatherName;
                        }
                    }else
                    {
                        dto.FatherHusbandName = employeeDetails.FirstOrDefault().HRME_FatherName;
                    }


                   // dto.FatherHusbandName = 
                    dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();

                    dto.pfreport = alldata.ToArray();
                    dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                }
                else
                {
                    dto.retrunMsg = "NoFinYear";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                dto.retrunMsg = "Error";
            }
            return dto;
        }
    }
}
