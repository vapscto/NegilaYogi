using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class FORM12Service: Interfaces.FORM12Interface
  {
    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public FORM12Service(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
    {
      _HRMSContext = HRMSContext;
      _Context = MsSqlServerContext;

    }
    public FORM12DTO getBasicData(FORM12DTO dto)
    {
      dto = GetAllDropdownAndDatatableDetails(dto);
      return dto;
    }

    public FORM12DTO GetAllDropdownAndDatatableDetails(FORM12DTO dto)
    {
      try
      {

      }
      catch (Exception ee)
      {
        Console.WriteLine(ee.Message);
      }
      return dto;
    }

    public FORM12DTO FilterEmployeeData(FORM12DTO dto)
    {
      return dto;
    }

    public FORM12DTO getEmployeedetailsBySelection(FORM12DTO dto)
    {

      //List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
      //Institution InstitutionDetails = new Institution();
      //List<HR_Employee_SalaryDTO> employeeSalaryDetailsDTO = new List<HR_Employee_SalaryDTO>();
      //List<FORM12DTO> alldata = new List<FORM12DTO>();
      ////List<FORM12DTO> cumDTOList = new List<FORM12DTO>();
      //try
      //{
      //  List<HR_Employee_Salary> employeeIds = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id == dto.MI_Id && (t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate)).ToList();

      //  var empIds = employeeIds.Select(t => t.HRME_Id).Distinct();

      //  var FinYearDetails = _HRMSContext.IVRM_Master_FinancialYear.Where(t => t.IMFY_FromDate >= Convert.ToDateTime(dto.FromDate) && t.IMFY_ToDate <= Convert.ToDateTime(dto.ToDate)).FirstOrDefault();
      //  if (FinYearDetails != null)
      //  {
      //    dto.finYearFromDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.IMFY_FromDate));

      //    dto.finYearToDate = string.Format(new CustomDateProvider(), "{0}", Convert.ToDateTime(FinYearDetails.IMFY_ToDate));
      //  }
      //  else
      //  {
      //    dto.finYearFromDate = "";
      //    dto.finYearToDate = "";
      //  }

      //  foreach (var empId in empIds)
      //  {
      //    FORM12DTO ss = new FORM12DTO();
      //    List<FORM12DTO> alldataInner = new List<FORM12DTO>();



      //    List<HR_Employee_Salary> employeeSalaryDetailss = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id == dto.MI_Id && (t.HRES_FromDate >= dto.FromDate && t.HRES_ToDate <= dto.ToDate) && t.HRME_Id == empId).ToList();

      //    ss.HRES_EPF = employeeSalaryDetailss.Sum(t => t.HRES_EPF);
      //    ss.HRES_FPF = employeeSalaryDetailss.Sum(t => t.HRES_FPF);
      //    MasterEmployee employeeDetailss = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == empId).FirstOrDefault();

      //    ss.HRME_EmployeeFirstName = employeeDetailss.HRME_EmployeeFirstName;
      //    ss.HRME_EmployeeMiddleName = employeeDetailss.HRME_EmployeeMiddleName;
      //    ss.HRME_EmployeeLastName = employeeDetailss.HRME_EmployeeLastName;
      //    ss.HRME_PFAccNo = employeeDetailss.HRME_PFAccNo;


      //    foreach (var employeeSalary in employeeSalaryDetailss)
      //    {

      //      FORM12DTO ssInner = new FORM12DTO();

      //      var currentdata = (from HRESD in _HRMSContext.HR_Employee_Salary_Details
      //                         from HRES in _HRMSContext.HR_Employee_Salary
      //                           // from HREED in _HRMSContext.HR_Employee_EarningsDeductions
      //                         from HRMED in _HRMSContext.HR_Master_EarningsDeductions
      //                         where (HRESD.HRES_Id == HRES.HRES_Id &&
      //                         // (HREED.HRMED_Id == HRESD.HRMED_Id && HREED.HRME_Id == HRES.HRME_Id) &&
      //                         HRESD.HRMED_Id == HRMED.HRMED_Id

      //                       && HRESD.HRES_Id == employeeSalary.HRES_Id) //checking condition
      //                         select new FORM12DTO
      //                         {

      //                           HRESD_Id = HRESD.HRESD_Id,
      //                           HRES_Id = HRES.HRES_Id,
      //                           HRMED_Id = HRESD.HRMED_Id,
      //                           HRMED_Name = HRMED.HRMED_Name,
      //                           HRESD_Amount = HRESD.HRESD_Amount,
      //                         }).ToList();



      //      if (currentdata.Count() > 0)
      //      {

      //        decimal? BasicPayHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("Basic Pay")).Sum(t => t.HRESD_Amount);
      //        decimal? DAHRESD_Amount = currentdata.Where(t => t.HRMED_Name.Equals("DA")).Sum(t => t.HRESD_Amount);


      //        decimal? PFAmount = currentdata.Where(t => t.HRMED_Name.Equals("PF")).Sum(t => t.HRESD_Amount);

      //        decimal? AmountofWages = BasicPayHRESD_Amount + DAHRESD_Amount;

      //        ssInner.AmountofWages = AmountofWages;
      //        ssInner.PFAmount = PFAmount;


      //        alldataInner.Add(ssInner);

      //      }
      //      else
      //      {
      //        ssInner.AmountofWages = 0;
      //        ssInner.PFAmount = 0;

      //        alldataInner.Add(ssInner);

      //      }

      //    }

      //    ss.AmountofWages = alldataInner.Sum(t => t.AmountofWages);
      //    ss.PFAmount = alldataInner.Sum(t => t.PFAmount);

      //    alldata.Add(ss);


      //  }

      //  dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id == dto.MI_Id).ToArray();

      //  dto.pfreport = alldata.ToArray();
      //  dto.PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();


      //}
      //catch (Exception ex)
      //{
      //  Console.WriteLine(ex);
      //}
      return dto;
    }
  }
}
