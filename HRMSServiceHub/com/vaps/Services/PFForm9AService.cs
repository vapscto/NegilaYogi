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
    public class PFForm9AService : Interfaces.PFForm9AInterface
  {
    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public PFForm9AService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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


                employeeDetails = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_ActiveFlag == true).ToList();

                var professionltax = (from emp in _HRMSContext.HR_Employee_Salary
                                      from hh in _HRMSContext.HR_Employee_Salary_Details
                                      from mas in _HRMSContext.HR_Master_EarningsDeductions
                                      where (hh.HRMED_Id == mas.HRMED_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.MI_Id == dto.MI_Id && mas.HRMED_EDTypeFlag == "PT" && emp.HRES_Id == hh.HRES_Id && emp.HRES_Month==dto.HRES_Month && emp.HRES_Year==dto.HRES_Year)
                                      select new PFReportsDTO
                                      {

                                          HRESD_Amount = hh.HRESD_Amount

                                      }).ToList();

                dto.professionaltaxamount = Convert.ToDecimal(professionltax.Sum(t => t.HRESD_Amount));


               // dto.pfreport = alldata.ToArray();

                // institutionDetails
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();

                var bankdetails = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();
                dto.bankdetails = bankdetails;

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
