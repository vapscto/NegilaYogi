using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class CTCReportService : Interfaces.CTCReportInterface
  {
    public HRMSContext _HRMSContext;
    public DomainModelMsSqlServerContext _Context;
    public CTCReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
    {
      _HRMSContext = HRMSContext;
      _Context = MsSqlServerContext;

    }
        public CTCReportDTO getBasicData(CTCReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public CTCReportDTO GetAllDropdownAndDatatableDetails(CTCReportDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();


            try
            {

                //leave year
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

            //emptype
            // dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

            //leave year
            leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
            dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

            // employee grouptype

            dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();
            //employee  
            //employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) ).ToList();
            //dto.employeedropdown = employe.ToArray();

            //departmentdropdown
            dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

            //designationdropdown 
            dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

            HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

            HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
            dto.configurationDetails = dmoObj;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public CTCReportDTO getEmployeedetailsBySelection(CTCReportDTO dto)
        {
            List<CumulativeSalaryReportDTO> cumDTOList = new List<CumulativeSalaryReportDTO>();
            CumulativeSalaryReportDTO cumDTO = new CumulativeSalaryReportDTO();

            List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
            //  List<HR_Master_EarningsDeductions> earningdeductiondatalist = new List<HR_Master_EarningsDeductions>();
            try
            {
                // var emp = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month)).ToList();

                if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                }

                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                {
                    //employee
                    employeSalary = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                }

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                foreach (var CurrentHRME_Id in employeSalary)
                {

                    decimal Lopdays = 0;
                    decimal LopAmount = 0;
                    //LOP Calculation

                    var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans_Details
                                  from B in _HRMSContext.HR_Master_Leave
                                  where (B.HRML_Id == A.HRML_Id &&
                                  A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == CurrentHRME_Id.HRME_Id &&
                                  B.HRML_LeaveType.Equals("LWP")
                                && ((A.HRELTD_FromDate >= CurrentHRME_Id.HRES_FromDate && A.HRELTD_FromDate <= CurrentHRME_Id.HRES_ToDate)
                                    || (A.HRELTD_ToDate >= CurrentHRME_Id.HRES_FromDate && A.HRELTD_ToDate <= CurrentHRME_Id.HRES_ToDate))
                                  )
                                  select A
                               ).ToList();
                    if (LOPcal.Count() > 0)
                    {
                        Lopdays = LOPcal.Sum(t => t.HRELTD_TotDays);

                        LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(CurrentHRME_Id.HRES_DailyRates);
                    }
                    else
                    {
                        Lopdays = 0;
                    }



                    //Employee salary Details for particular month

                    cumDTO = (from HRES in _HRMSContext.HR_Employee_Salary
                              from HRME in _HRMSContext.MasterEmployee

                              where (HRME.HRME_Id == HRES.HRME_Id
                              && HRES.HRES_Id == CurrentHRME_Id.HRES_Id //checking condition
                              )
                              select new CumulativeSalaryReportDTO
                              {
                                  HRES_Id = HRES.HRES_Id,
                                  HRME_Id = HRME.HRME_Id,
                                  HRES_WorkingDays = HRES.HRES_WorkingDays,
                                  HRES_FromDate = HRES.HRES_FromDate,
                                  HRES_ToDate = HRES.HRES_ToDate,
                                  HRES_ESIEmplr= (HRES.HRES_ESIEmplr != null ? HRES.HRES_ESIEmplr : 0),
                                  HRES_PFEmplr = ((HRES.HRES_EPF != null ? HRES.HRES_EPF: 0) + (HRES.HRES_FPF != null ? HRES.HRES_FPF : 0) + (HRES.HRES_Ac21 != null ? HRES.HRES_Ac21 : 0)  + (HRES.HRES_Ac22 != null ? HRES.HRES_Ac22 : 0) + (HRES.HRES_Ac5 != null ? HRES.HRES_Ac5 : 0)),
                                  HRME_PFAccNo=  HRME.HRME_PFAccNo,
                                  HRME_UINumber = HRME.HRME_UINumber,
                                  LOPDays = Lopdays,
                                  HRME_EmployeeFirstName = HRME.HRME_EmployeeFirstName,
                                  HRME_EmployeeMiddleName = HRME.HRME_EmployeeMiddleName,
                                  HRME_EmployeeLastName = HRME.HRME_EmployeeLastName,
                                  //   EmployeeName = (HRME.HRME_EmployeeFirstName + " " + HRME.HRME_EmployeeMiddleName + " " + HRME.HRME_EmployeeLastName),
                                  HRME_EmployeeCode = HRME.HRME_EmployeeCode,
                                   HRME_EmployeeOrder = HRME.HRME_EmployeeOrder


                                  }).FirstOrDefault();

                    List<HR_Employee_Salary_DetailsDTO> alldata = new List<HR_Employee_Salary_DetailsDTO>();




                    //Employee earning Deduction Details

                    var allhead = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToList();

                    if (allhead.Count() > 0)
                    {

                        foreach (var head in allhead)
                        {
                            HR_Employee_Salary_DetailsDTO ss = new HR_Employee_Salary_DetailsDTO();

                            if (!head.HRMED_Name.Equals("") && !head.HRMED_Name.Equals(null))
                            {
                                if (head.HRMED_EarnDedFlag.Equals("Arrear"))
                                {
                                    var currentdata = (//from HRES in _HRMSContext.HR_Employee_Salary
                                                       from HRME in _HRMSContext.MasterEmployee
                                                       from HRESD in _HRMSContext.HR_Employee_Arrear_Salary
                                                       from HREED in _HRMSContext.HR_Employee_EarningsDeductions
                                                       from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                                       where (HRME.HRME_Id == HRESD.HRME_Id && //HRESD.HRES_Id == HRES.HRES_Id &&
                                                       (HREED.HRMED_Id == HRESD.HRMED_Id && HREED.HRME_Id == HRESD.HRME_Id) &&
                                                       HRMED.HRMED_Id == HREED.HRMED_Id 
                                                       )
                                                     && HRMED.HRMED_Name == head.HRMED_Name //&& HRESD.HRES_Id == CurrentHRME_Id.HRES_Id //checking condition
                                                     &&
                                                     HRESD.MI_Id.Equals(dto.MI_Id) && HRESD.HREAS_Year.Equals(dto.HRES_Year) && HRESD.HREAS_Month.Equals(dto.HRES_Month) 
                                                     && HRESD.HRME_Id == CurrentHRME_Id.HRME_Id
                                                       select new HR_Employee_Salary_DetailsDTO
                                                       {

                                                           HRESD_Id = HRESD.HREAS_Id,
                                                           HRES_Id = 0,
                                                           HRMED_Id = HRESD.HRMED_Id,
                                                           HRMED_Name = HRMED.HRMED_Name,
                                                           HRESD_Amount = HRESD.HREAS_Amount,
                                                           HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                       }).ToList();

                                    if (currentdata.Count() > 0)
                                    {

                                        decimal? HRESD_Amount = currentdata.Sum(t => t.HRESD_Amount);

                                        ss.HRESD_Id = currentdata.FirstOrDefault().HRESD_Id;
                                        ss.HRMED_Id = head.HRMED_Id;
                                        ss.HRMED_Name = currentdata.FirstOrDefault().HRMED_Name;
                                        ss.HRESD_Amount = Math.Round(Convert.ToDecimal(HRESD_Amount), 0);
                                        ss.HRMED_EarnDedFlag = currentdata.FirstOrDefault().HRMED_EarnDedFlag;

                                        alldata.Add(ss);

                                    }
                                    else
                                    {

                                        ss.HRMED_Id = head.HRMED_Id;
                                        ss.HRMED_Name = head.HRMED_Name;
                                        ss.HRESD_Amount = 0;
                                        ss.HRMED_EarnDedFlag = head.HRMED_EarnDedFlag;


                                        alldata.Add(ss);

                                    }


                                }
                                else
                                {

                                    var currentdata = (from HRES in _HRMSContext.HR_Employee_Salary
                                                       from HRME in _HRMSContext.MasterEmployee
                                                       from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                                       from HREED in _HRMSContext.HR_Employee_EarningsDeductions
                                                       from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                                       where (HRME.HRME_Id == HRES.HRME_Id && HRESD.HRES_Id == HRES.HRES_Id &&
                                                       (HREED.HRMED_Id == HRESD.HRMED_Id && HREED.HRME_Id == HRES.HRME_Id) &&
                                                       HRMED.HRMED_Id == HREED.HRMED_Id
                                                       )
                                                     && HRMED.HRMED_Name == head.HRMED_Name && HRES.HRES_Id == CurrentHRME_Id.HRES_Id //checking condition
                                                       select new HR_Employee_Salary_DetailsDTO
                                                       {

                                                           HRESD_Id = HRESD.HRESD_Id,
                                                           HRES_Id = HRES.HRES_Id,
                                                           HRMED_Id = HRESD.HRMED_Id,
                                                           HRMED_Name = HRMED.HRMED_Name,
                                                           HRESD_Amount = HRESD.HRESD_Amount,
                                                           HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                       }).ToList();

                                    if (currentdata.Count() > 0)
                                    {

                                        decimal? HRESD_Amount = currentdata.Sum(t => t.HRESD_Amount);

                                        ss.HRESD_Id = currentdata.FirstOrDefault().HRESD_Id;
                                        ss.HRMED_Id = head.HRMED_Id;
                                        ss.HRMED_Name = currentdata.FirstOrDefault().HRMED_Name;
                                        ss.HRESD_Amount = Math.Round(Convert.ToDecimal(HRESD_Amount), 0);
                                        ss.HRMED_EarnDedFlag = currentdata.FirstOrDefault().HRMED_EarnDedFlag;

                                        alldata.Add(ss);

                                    }
                                    else
                                    {

                                        ss.HRMED_Id = head.HRMED_Id;
                                        ss.HRMED_Name = head.HRMED_Name;
                                        ss.HRESD_Amount = 0;
                                        ss.HRMED_EarnDedFlag = head.HRMED_EarnDedFlag;


                                        alldata.Add(ss);

                                    }


                                }
                                
                            }

                        }

                    }

                    cumDTO.arrearresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Arrear")).ToArray();
                    cumDTO.earningresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).ToArray();

                    cumDTO.grossEarning = 0;
                    foreach (var grsearn in cumDTO.earningresult)
                    {

                        decimal? HRESD_Amount = 0;

                        if (grsearn.HRESD_Amount != null)
                        {
                            HRESD_Amount = grsearn.HRESD_Amount;
                        }
                        else
                        {
                            HRESD_Amount = 0;
                        }

                        cumDTO.grossEarning = Math.Round(Convert.ToDecimal(cumDTO.grossEarning + HRESD_Amount), 0);


                    }

                    cumDTO.grossArrear = 0;
                    foreach (var grsearn in cumDTO.arrearresult)
                    {

                        decimal? HRESD_Amount = 0;

                        if (grsearn.HRESD_Amount != null)
                        {
                            HRESD_Amount = grsearn.HRESD_Amount;
                        }
                        else
                        {
                            HRESD_Amount = 0;
                        }

                        cumDTO.grossArrear = Math.Round(Convert.ToDecimal(cumDTO.grossArrear + HRESD_Amount), 0);


                    }

                    cumDTO.deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();


                    cumDTO.grossDeduction = 0;
                    foreach (var grossDeduction in cumDTO.deductionresult)
                    {

                        decimal? HRESD_Amount = 0;

                        if (grossDeduction.HRESD_Amount != null)
                        {
                            HRESD_Amount = grossDeduction.HRESD_Amount;
                        }
                        else
                        {
                            HRESD_Amount = 0;
                        }

                        cumDTO.grossDeduction = Math.Round(Convert.ToDecimal(cumDTO.grossDeduction + HRESD_Amount), 0);
                    }

                    if (PayrollStandard.HRC_PayMethodFlg.Equals("Method1"))
                    {
                        cumDTO.netSalary = (cumDTO.grossEarning - cumDTO.grossDeduction) - LopAmount;

                        cumDTO.netCTC = Math.Round(Convert.ToDecimal(cumDTO.netSalary + cumDTO.grossArrear + cumDTO.HRES_PFEmplr + cumDTO.HRES_ESIEmplr), 2);
                    }
                    else
                    {
                        cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 2);

                        cumDTO.netCTC = Math.Round(Convert.ToDecimal(cumDTO.netSalary + cumDTO.grossArrear + cumDTO.HRES_PFEmplr + cumDTO.HRES_ESIEmplr), 2);
                    }



                    cumDTOList.Add(cumDTO);
                }


                dto.employeeSalaryslipDetails = cumDTOList.OrderBy(t=>t.HRME_EmployeeOrder).ToArray();

              //  Console.WriteLine(dto.employeeSalaryslipDetails);

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

    }
}
