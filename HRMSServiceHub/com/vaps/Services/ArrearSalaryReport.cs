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
    public class ArrearSalaryReport : Interfaces.ArrearSalaryReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public ArrearSalaryReport(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public HR_Arrear_SalaryDTO getBasicData(HR_Arrear_SalaryDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }


        public HR_Arrear_SalaryDTO GetAllDropdownAndDatatableDetails(HR_Arrear_SalaryDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();

            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {

                //leave year
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                //leave year
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t=>t.HRMLY_LeaveYearOrder).ToArray();



                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                         ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {


                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();


                }





                //// employee grouptype
                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

                ////departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

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


        public async Task<HR_Arrear_SalaryDTO> getEmployeedetailsBySelection(HR_Arrear_SalaryDTO dto)
        {
            List<HR_Arrear_SalaryDTO> cumDTOList = new List<HR_Arrear_SalaryDTO>();
            HR_Arrear_SalaryDTO cumDTO = new HR_Arrear_SalaryDTO();
            List<HR_Employee_Increment> employeSalary = new List<HR_Employee_Increment>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {

                //  Inatitution Details

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;


                //  Employee Salary Details

                employeSalary = (from a in _HRMSContext.MasterEmployee
                                 from b in _HRMSContext.HR_Employee_IncrementDMO
                                 where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                 //&& b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) 
                                 && a.HRME_ActiveFlag == true
                                 select b).Distinct().ToList();
                if (employeSalary.Count > 0)
                {

                    ///

                    PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                   from pa in _HRMSContext.HR_PROCESSDMO
                                   from cc in _HRMSContext.Staff_User_Login
                                   where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId && pa.HRPA_TypeFlag == "Salary")


                                   select pa
                       ).ToList();

                    if (PROCESSList.Count() > 0)
                    {
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();


                    }
                    else
                    {

                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();

                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                        }

                    }

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    // var optionsBuilder = new DbContextOptionsBuilder<HRMSContext>();
                    // optionsBuilder.UseSqlServer("Data Source = VAPS20 - PC; Initial Catalog = Dcampus; Persist Security Info = False; User ID = sa; Password = vts@123; Connection Timeout=30;");
                    var hashSet = new HashSet<HR_Employee_Increment>(employeSalary);
                    for (int x = 0; x < hashSet.Count; x++)
                    {
                        var CurrentHRME_Id = hashSet.ElementAt(x);
                        Task tTemp = Task.Run(() =>
                        {

                            decimal Lopdays = 0;
                            decimal LopAmount = 0;
                            //LOP Calculation

                        //    var _db = new HRMSContext(optionsBuilder.Options);

                            //var LOPcal = (from A in _db.HR_Emp_Leave_Trans
                            //              from B in _db.HR_Master_Leave
                            //              where (B.HRML_Id == A.HRELT_LeaveId &&
                            //              A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == CurrentHRME_Id.HRME_Id &&
                            //              B.HRML_LeaveType.Equals("LWP") && A.HRELT_ActiveFlag == true
                            //            && ((A.HRELT_FromDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_FromDate <= CurrentHRME_Id.HRES_ToDate)
                            //                || (A.HRELT_ToDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_ToDate <= CurrentHRME_Id.HRES_ToDate))
                            //              )
                            //              select A
                            //           ).ToList();




                            //var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                            //              from B in _HRMSContext.HR_Master_Leave
                            //              from C in _HRMSContext.HR_Emp_Leave_Trans_Details
                            //              where (B.HRML_Id == A.HRELT_LeaveId &&
                            //              A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == CurrentHRME_Id.HRME_Id &&
                            //              A.HRELT_ActiveFlag == true && C.HRELT_Id == A.HRELT_Id && C.HRELTD_LWPFlag == true
                            //              && ((A.HRELT_FromDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_FromDate <= CurrentHRME_Id.HRES_ToDate)
                            //              || (A.HRELT_ToDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_ToDate <= CurrentHRME_Id.HRES_ToDate))
                            //              )
                            //              select A).ToList();
                            //if (LOPcal.Count() > 0)
                            //{
                            //    Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                            //    LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(CurrentHRME_Id.HRES_DailyRates);
                            //}
                            //else
                            //{
                            //    Lopdays = 0;
                            //}

                            //Employee salary Details for particular month

                            cumDTO = (from HRES in _HRMSContext.HR_Employee_IncrementDMO
                                      from HRME in _HRMSContext.MasterEmployee
                                      from hrdes in _HRMSContext.HR_Master_Designation
                                      from hrgrd in _HRMSContext.HR_Master_Grade
                                      where (HRME.HRME_Id == HRES.HRME_Id
                                      && HRES.HREIC_Id == CurrentHRME_Id.HREIC_Id
                                     
                                      && hrgrd.HRMG_Id == HRME.HRMG_Id//checking condition
                                      )
                                      select new HR_Arrear_SalaryDTO
                                      {
                                          HREIC_Id = HRES.HREIC_Id,
                                          HRME_Id = HRME.HRME_Id,
                                        
                                         // LOPDays = Lopdays,
                                          HRME_EmployeeFirstName = HRME.HRME_EmployeeFirstName,
                                          HRME_EmployeeMiddleName = HRME.HRME_EmployeeMiddleName,
                                          HRME_EmployeeLastName = HRME.HRME_EmployeeLastName,
                                          HRME_EmployeeCode = HRME.HRME_EmployeeCode,
                                          HRME_EmployeeOrder = HRME.HRME_EmployeeOrder,
                                          HRMDES_Designationname = hrdes.HRMDES_DesignationName,
                                          HRMG_GradeName = hrgrd.HRMG_GradeName,
                                          HRMG_ORDER = hrgrd.HRMG_Order,
                                        
                                      }).FirstOrDefault();


                            var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                              from mas in _HRMSContext.HR_Master_EarningsDeductions
                                              where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id && mas.HRMED_EDTypeFlag == "Basic Pay")
                                              select new HR_Arrear_SalaryDTO
                                              {
                                                  HRESD_Amount = emp.HREED_Amount

                                              }).ToList();

                            cumDTO.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                            var grosspayhead = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                                from mas in _HRMSContext.HR_Master_EarningsDeductions
                                                where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Gross" && mas.MI_Id == dto.MI_Id)
                                                select new HR_Arrear_SalaryDTO
                                                {
                                                    HRESD_Amount = emp.HREED_Amount

                                                }).ToList();

                            if (grosspayhead.Count() > 0)
                            {

                                cumDTO.grosspayhead = Convert.ToDecimal(grosspayhead.Sum(t => t.HRESD_Amount));

                            }

                            else { }

                            List<HR_Emp_IncrementDTO> alldata = new List<HR_Emp_IncrementDTO>();


                            if (cumDTO != null)
                            {
                                //Employee earning Deduction Details

                                var allhead = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).OrderBy(t => t.HRMED_Order).ToList();

                                if (allhead.Count() > 0)
                                {

                                    foreach (var head in allhead)
                                    {
                                        Task tTemp1 = Task.Run(() =>
                                        {
                                            HR_Emp_IncrementDTO ss = new HR_Emp_IncrementDTO();

                                            if (!head.HRMED_Name.Equals("") && head.HRMED_Name != null)
                                            {
                                                List<HR_Emp_IncrementDTO> currentdata = new List<HR_Emp_IncrementDTO>();
                                                if (dto.comm == "1")
                                                {
                                                    currentdata = (from HRES in _HRMSContext.HR_Employee_Increment_EDHeadsDMO
                                                                   from HRESD in _HRMSContext.HR_Employee_IncrementDMO
                                                                   from HRMED in _HRMSContext.HR_Master_EarningsDeductions

                                                                   where (HRESD.HREIC_Id == HRES.HREIC_Id &&
                                                                            HRES.HRMED_Id == HRMED.HRMED_Id &&
                                                                          
                                                                            HRES.HRMED_Id == head.HRMED_Id && HRESD.HREIC_ActiveFlag==true 
                                                                            ) //checking condition
                                                                   select new HR_Emp_IncrementDTO
                                                                   {

                                                                       HREIC_Id = HRESD.HREIC_Id,
                                                                       HREICED_Id = HRES.HREICED_Id,
                                                                       HRMED_Id = HRES.HRMED_Id,
                                                                       HRMED_Name = HRMED.HRMED_Name,
                                                                       HRESD_Amount = HRES.HREICED_Amount,
                                                                       HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                                   }).ToList();
                                                }
                                                else
                                                {
                                                    currentdata = (from HRES in _HRMSContext.HR_Employee_Increment_EDHeadsDMO
                                                                   from HRESD in _HRMSContext.HR_Employee_IncrementDMO
                                                                   from HRMED in _HRMSContext.HR_Master_EarningsDeductions

                                                                   where (HRESD.HREIC_Id == HRES.HREIC_Id &&
                                                                            HRES.HRMED_Id == HRMED.HRMED_Id &&

                                                                            HRES.HRMED_Id == head.HRMED_Id && HRESD.HREIC_ActiveFlag == true
                                                                            ) //checking condition
                                                                   select new HR_Emp_IncrementDTO
                                                                   {

                                                                       HREIC_Id = HRESD.HREIC_Id,
                                                                       HREICED_Id = HRES.HREICED_Id,
                                                                       HRMED_Id = HRES.HRMED_Id,
                                                                       HRMED_Name = HRMED.HRMED_Name,
                                                                       HREICED_Amount = HRES.HREICED_Amount,
                                                                       HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                                                   }).ToList();
                                                }

                                                if (currentdata.Count() > 0)
                                                {

                                                    decimal? HREICED_Amount = currentdata.Sum(t => t.HREICED_Amount);

                                                    ss.HREICED_Id = currentdata.FirstOrDefault().HREICED_Id;
                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = currentdata.FirstOrDefault().HRMED_Name;
                                                    ss.HREICED_Amount = Math.Round(Convert.ToDecimal(HREICED_Amount), 0);
                                                    ss.HRMED_EarnDedFlag = currentdata.FirstOrDefault().HRMED_EarnDedFlag;

                                                    alldata.Add(ss);

                                                }
                                                else
                                                {

                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = head.HRMED_Name;
                                                    ss.HREICED_Amount = 0;
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

                                    decimal? HREICED_Amount = 0;

                                    if (grsearn.HREICED_Amount != null)
                                    {
                                        HREICED_Amount = grsearn.HREICED_Amount;
                                    }
                                    else
                                    {
                                        HREICED_Amount = 0;
                                    }

                                    cumDTO.grossEarning = Math.Round(Convert.ToDecimal(cumDTO.grossEarning + HREICED_Amount), 0);


                                }
                                var deductionresult = alldata.Where(t => t.HRMED_EarnDedFlag.Equals("Deduction")).ToArray();
                                cumDTO.deductionresult = deductionresult;

                                cumDTO.grossDeduction = 0;
                                foreach (var grossDeduction in cumDTO.deductionresult)
                                {

                                    decimal? HREICED_Amount = 0;

                                    if (grossDeduction.HREICED_Amount != null)
                                    {
                                        HREICED_Amount = grossDeduction.HREICED_Amount;
                                    }
                                    else
                                    {
                                        HREICED_Amount = 0;
                                    }

                                    cumDTO.grossDeduction = Math.Round(Convert.ToDecimal(cumDTO.grossDeduction + HREICED_Amount), 0);
                                }

                                if (PayrollStandard.HRC_PayMethodFlg.Equals("Method1"))
                                {
                                    // cumDTO.netSalary = Math.Round(Convert.ToDecimal((cumDTO.grossEarning - cumDTO.grossDeduction) - LopAmount), 0);
                                    cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 0);
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


        public HR_Arrear_SalaryDTO get_depts(HR_Arrear_SalaryDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public HR_Arrear_SalaryDTO get_desig(HR_Arrear_SalaryDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}