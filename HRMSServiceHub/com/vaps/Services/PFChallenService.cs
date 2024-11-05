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
    public class PFChallenService : Interfaces.PFChallenInterface
    {

    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public PFChallenService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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
            //List<PFReportsDTO> cumDTOList = new List<PFReportsDTO>();
            try
            {

                List<HR_Employee_Salary> employeeSalaryDetails = _HRMSContext.HR_Employee_Salary.Where(t => t.HRES_Month.Equals(dto.HRES_Month) && t.HRES_Year.Equals(dto.HRES_Year)).ToList();

                foreach (var empSalary in employeeSalaryDetails)
                {
                    PFReportsDTO ss = new PFReportsDTO();


                    var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                       from HRES in _HRMSContext.HR_Employee_Salary
                                       from hrme in _HRMSContext.MasterEmployee
                                           // from HREED in _HRMSContext.HR_Employee_EarningsDeductions
                                       from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                       where (HRESD.HRES_Id == HRES.HRES_Id && HRES.HRME_Id==hrme.HRME_Id && hrme.HRME_PFApplicableFlag==true &&
                                       // (HREED.HRMED_Id == HRESD.HRMED_Id && HREED.HRME_Id == HRES.HRME_Id) &&
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


                        decimal? PFAmount = currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount);

                        decimal? AmountofWages = BasicPayHRESD_Amount + DAHRESD_Amount;

                        ss.AmountofWages = AmountofWages;
                        ss.PFAmount = PFAmount;
                       
                        ss.HRES_EPF = empSalary.HRES_EPF;
                        ss.HRES_FPF = empSalary.HRES_FPF;

                        ss.HRES_Ac21 = empSalary.HRES_Ac21;
                        ss.HRES_Ac22 = empSalary.HRES_Ac22;
                        ss.HRES_Ac5 = empSalary.HRES_Ac5;

                        ss.HRES_Ac10 = 0;


                        alldata.Add(ss);

                    }
                    else
                    {
                        ss.AmountofWages = 0;
                        ss.PFAmount = 0;
                        ss.HRES_EPF = 0;
                        ss.HRES_FPF = 0;
                        ss.HRES_Ac21 = 0;
                        ss.HRES_Ac22 = 0;
                        ss.HRES_Ac5 = 0;
                        ss.HRES_Ac10 = 0;

                        alldata.Add(ss);

                    }
            }
                List<PFReportsDTO> pfchallen = new List<PFReportsDTO>();
                PFReportsDTO challen = new PFReportsDTO();

                challen.HRES_EPF = alldata.Sum(t=>t.HRES_EPF);
                challen.HRES_FPF = alldata.Sum(t => t.HRES_FPF);
                challen.HRES_Ac5 = alldata.Sum(t => t.HRES_Ac5);
                challen.HRES_Ac21 = alldata.Sum(t => t.HRES_Ac21);
                challen.HRES_Ac22 = alldata.Sum(t => t.HRES_Ac22);
                challen.HRES_Ac10 = alldata.Sum(t => t.HRES_Ac10);

               
                pfchallen.Add(challen);
                dto.pfreport = pfchallen.ToArray();

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
