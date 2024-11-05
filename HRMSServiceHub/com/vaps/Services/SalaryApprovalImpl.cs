using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace HRMSServicesHub.com.vaps.Services
{
    public class SalaryApprovalImpl : Interfaces.SalaryApprovalInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;

        public SalaryApprovalImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HR_Employee_SalaryDTO getBasicData(HR_Employee_SalaryDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
    



        public HR_Employee_SalaryDTO GetAllDropdownAndDatatableDetails(HR_Employee_SalaryDTO dto)
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
                //employee

                employe = (from a in _HRMSContext.MasterEmployee
                           from b in _HRMSContext.HR_Employee_Salary
                           where (a.HRME_Id == b.HRME_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_ActiveFlag == true)
                           select a).ToList();


                dto.employeedropdown = employe.ToArray();

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();






                //HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                //    HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //    dto.configurationDetails = dmoObj;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

     

        public HR_Employee_SalaryDTO getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto)
        {
            List<CumulativeSalaryReportDTO> cumDTOList = new List<CumulativeSalaryReportDTO>();
            CumulativeSalaryReportDTO cumDTO = new CumulativeSalaryReportDTO();
            List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();

            try
            {

                //  Inatitution Details

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;


                //  Employee Salary Details

                employeSalary = (from a in _HRMSContext.MasterEmployee
                                 from b in _HRMSContext.HR_Employee_Salary
                                 where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                 && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                                 select b).Distinct().ToList();
                if (employeSalary.Count > 0)
                {

                    if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }


                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    var optionsBuilder = new DbContextOptionsBuilder<HRMSContext>();
                    optionsBuilder.UseSqlServer("Data Source =VAPS-PC;Initial Catalog = baldwinbackup; User ID = sa; Password =vts@123; Connection Timeout = 30");

                    var hashSet = new HashSet<HR_Employee_Salary>(employeSalary);

                    for (int x = 0; x < hashSet.Count; x++)
                    {
                        var CurrentHRME_Id = hashSet.ElementAt(x);
                        Task tTemp = Task.Run(() =>
                        {

                            decimal Lopdays = 0;
                            decimal LopAmount = 0;
                            //LOP Calculation

                            var _db = new HRMSContext(optionsBuilder.Options);

                            var LOPcal = (from A in _db.HR_Emp_Leave_Trans
                                          from B in _db.HR_Master_Leave
                                          where (B.HRML_Id == A.HRELT_LeaveId &&
                                          A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == CurrentHRME_Id.HRME_Id &&
                                          B.HRML_LeaveType.Equals("LWP") && A.HRELT_ActiveFlag == true
                                        && ((A.HRELT_FromDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_FromDate <= CurrentHRME_Id.HRES_ToDate)
                                            || (A.HRELT_ToDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_ToDate <= CurrentHRME_Id.HRES_ToDate))
                                          )
                                          select A
                                       ).ToList();
                            if (LOPcal.Count() > 0)
                            {
                                Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                                LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(CurrentHRME_Id.HRES_DailyRates);
                            }
                            else
                            {
                                Lopdays = 0;
                            }

                            //Employee salary Details for particular month

                            cumDTO = (from HRES in _db.HR_Employee_Salary
                                      from HRME in _db.MasterEmployee
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
                                          LOPDays = Lopdays,
                                          HRME_EmployeeFirstName = HRME.HRME_EmployeeFirstName,
                                          HRME_EmployeeMiddleName = HRME.HRME_EmployeeMiddleName,
                                          HRME_EmployeeLastName = HRME.HRME_EmployeeLastName,
                                          HRME_EmployeeCode = HRME.HRME_EmployeeCode,
                                          HRME_EmployeeOrder = HRME.HRME_EmployeeOrder

                                      }).FirstOrDefault();

                            List<HR_Employee_Salary_DetailsDTO> alldata = new List<HR_Employee_Salary_DetailsDTO>();


                            if (cumDTO != null)
                            {
                                //Employee earning Deduction Details

                                var allhead = _db.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).OrderBy(t => t.HRMED_Order).ToList();

                                if (allhead.Count() > 0)
                                {

                                    foreach (var head in allhead)
                                    {
                                        Task tTemp1 = Task.Run(() =>
                                        {
                                            HR_Employee_Salary_DetailsDTO ss = new HR_Employee_Salary_DetailsDTO();

                                            if (!head.HRMED_Name.Equals("") && head.HRMED_Name != null)
                                            {

                                                var currentdata = (from HRES in _db.HR_Employee_Salary
                                                                   from HRESD in _db.HR_Employee_Salary_Details
                                                                   from HRMED in _db.HR_Master_EarningsDeductions
                                                                   where (HRESD.HRES_Id == HRES.HRES_Id &&
                                                                            HRESD.HRMED_Id == HRMED.HRMED_Id &&
                                                                            HRESD.HRES_Id == CurrentHRME_Id.HRES_Id &&
                                                                            HRESD.HRMED_Id == head.HRMED_Id
                                                                            ) //checking condition
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

                                        });
                                        tTemp1.Wait();

                                    }


                                }

                                var earningresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Earning")).ToArray();
                                cumDTO.earningresult = earningresult;

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
                                var deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();
                                cumDTO.deductionresult = deductionresult;

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
                                    cumDTO.netSalary = Math.Round(Convert.ToDecimal((cumDTO.grossEarning - cumDTO.grossDeduction) - LopAmount), 0);
                                }
                                else
                                {
                                    cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 0);
                                }


                                cumDTOList.Add(cumDTO);
                            }

                        });
                        tTemp.Wait();
                    }



                    dto.employeeSalaryslipDetails = cumDTOList.OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                }
                else
                {
                    dto.employeeSalaryslipDetails = cumDTOList.ToArray();
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}

