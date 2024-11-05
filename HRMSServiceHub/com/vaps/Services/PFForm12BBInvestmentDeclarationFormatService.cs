using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class PFForm12BBInvestmentDeclarationFormatService :Interfaces.PFForm12BBInvestmentDeclarationFormatInterface
    {
    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public PFForm12BBInvestmentDeclarationFormatService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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
        //List<PFReportsDTO> cumDTOList = new List<PFReportsDTO>();
        //List<MasterEmployeeDTO> employeeDetails = new List<MasterEmployeeDTO>();
      try
      {
            if (dto.HRME_Id > 0)
                {
                    dto.employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id == dto.HRME_Id).ToArray();
                    dto.contactnum = _HRMSContext.Emp_MobileNo.Where(t => t.HRME_Id == dto.HRME_Id && t.HRMEMNO_DeFaultFlag == "default").ToArray();
                }

                //if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() > 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() > 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() == 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.designationselected.Contains(t.HRMDES_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() > 0 && dto.groupTypeselected.Count() == 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.departmentselected.Contains(t.HRMD_Id)).ToList();
                //}
                //else if (dto.designationselected.Count() == 0 && dto.departmentselected.Count() == 0 && dto.groupTypeselected.Count() > 0)
                //{
                //  //employee
                //  employeeDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeselected.Contains(t.HRMGT_Id)).ToList();
                //}
                //if (dto.MonthBetweenDates.Equals("Month"))
                //{
                //  employeeDetails = employeeDetails.Where(t => t.HRES_Month.Equals(dto.hreS_Month) && t.HRES_Year.Equals(dto.hreS_Year)).ToList();
                //}
                //else if (dto.MonthBetweenDates.Equals("BetweenDates"))
                //{
                //  employeeDetails = employeeDetails.Where(t => t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate).ToList();
                //}
                //if (dto.FormatType.Equals("Format1"))
                //{
                //  employeeDetails = employeeDetails.Where(t => t.HRME_Id == dto.HRME_Id).ToList();
                //}
                //if (employeeDetails.Count() > 0)
                //{
                //  long? headId = 0;
                //  if (dto.EarningDeduction.Equals("Earning"))
                //  {
                //    headId = dto.EarningHead;
                //  }
                //  else if (dto.EarningDeduction.Equals("Deduction"))
                //  {
                //    headId = dto.DeductionHead;
                //  }
                //  foreach (var emp in employeeDetails)
                //  {
                //    var HRESD_Amount = _HRMSContext.HR_Employee_Salary_Details.Where(t => t.HRES_Id == emp.HRES_Id && t.HRMED_Id == headId).ToList();
                //    decimal? Amount = HRESD_Amount.Sum(t => t.HRESD_Amount);
                //    PFReportsDTO employe = new PFReportsDTO();
                //    employe = (from a in _HRMSContext.MasterEmployee
                //               from b in _HRMSContext.HR_Employee_Salary
                //               from d in _HRMSContext.HR_Master_EarningsDeductions
                //               from e in _HRMSContext.HR_Master_Department
                //               from f in _HRMSContext.HR_Master_Designation
                //               where (a.HRME_Id == b.HRME_Id &&
                //            b.MI_Id.Equals(dto.MI_Id) &&
                //            e.HRMD_Id == b.HRMD_Id &&
                //            f.HRMDES_Id == b.HRMDES_Id &&
                //            b.HRES_Id.Equals(emp.HRES_Id))
                //               select new PFReportsDTO
                //               {
                //                 EmployeeCode = a.HRME_EmployeeCode,
                //                 HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                //                 HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                //                 HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                //                 departmentName = e.HRMD_DepartmentName,
                //                 designationName = f.HRMDES_DesignationName,
                //                 selectedHeadAmount = Amount
                //               }).FirstOrDefault();
                //    cumDTOList.Add(employe);
                //  }
                //}
                //dto.employeeDetails = cumDTOList.ToArray();
            }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
      return dto;
    }


  }
}
