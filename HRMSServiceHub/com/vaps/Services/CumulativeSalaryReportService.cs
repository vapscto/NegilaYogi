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
    public class CumulativeSalaryReportService : Interfaces.CumulativeSalaryReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public CumulativeSalaryReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).OrderBy(t => t.HRMLY_LeaveYearOrder).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)
                               select pa).ToList();

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
                dto.earningdeductiondetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true).ToArray();




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
        public async Task<HR_Employee_SalaryDTO> getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto)
        {

            try
            {
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                List<CumulativeSalaryReportDTO> cumDTOList = new List<CumulativeSalaryReportDTO>();

                List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
                List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
                //List<HR_Employee_SalaryDTO> employeedetails = new List<HR_Employee_SalaryDTO>();

                employeSalary = (from a in _HRMSContext.MasterEmployee
                                 from b in _HRMSContext.HR_Employee_Salary
                                 where (dto.hrmdeS_IdList.Contains(b.HRMDES_Id) && dto.hrmD_IdList.Contains(b.HRMD_Id) && dto.groupTypeIdList.Contains(b.HRMGT_Id) && b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                 && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                                 select b).Distinct().ToList();

                if (employeSalary.Count > 0)
                {

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    var optionsBuilder = new DbContextOptionsBuilder<HRMSContext>();

                    optionsBuilder.UseSqlServer("Data Source = kusumavaps.database.windows.net, 1433; Initial Catalog = vapskusuma; Persist Security Info = False; User ID = vapskusuma; Password = @zure2021V@p$EcaMpU$; Connection Timeout = 30;");

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
                            List<HR_Employee_SalaryDTO> LOPcal = new List<HR_Employee_SalaryDTO>();
                            CumulativeSalaryReportDTO cumDTO = new CumulativeSalaryReportDTO();
                            //Employee salary Details for particular month
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "HR_EmployeeSalaryDetails";
                                cmd.CommandTimeout = 900000000;
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
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                                {
                                    Value = CurrentHRME_Id.HRME_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@HRES_Id", SqlDbType.BigInt)
                                {
                                    Value = CurrentHRME_Id.HRES_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                                {
                                    Value = CurrentHRME_Id.HRES_FromDate
                                });
                                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                                {
                                    Value = CurrentHRME_Id.HRES_ToDate
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

                                            cumDTO.HRES_Id = Convert.ToInt64(dataReader["hreS_Id"].ToString());
                                            cumDTO.HRME_Id = Convert.ToInt64(dataReader["hrmE_Id"].ToString());
                                            cumDTO.HRMD_Id = Convert.ToInt64(dataReader["hrmD_Id"].ToString());
                                            cumDTO.HRME_EmployeeFirstName = dataReader["hrmE_EmployeeFirstName"].ToString();
                                            cumDTO.HRMDES_Designationname = dataReader["hrmdeS_DesignationName"].ToString();
                                            cumDTO.HRME_EmployeeOrder = Convert.ToInt32(dataReader["hrmE_EmployeeOrder"].ToString());
                                            cumDTO.HRMG_ORDER = Convert.ToInt32(dataReader["HRMG_Order"].ToString());
                                            cumDTO.HRMG_GradeName = dataReader["hrmG_GradeName"].ToString();
                                            cumDTO.HRES_WorkingDays = Convert.ToDouble(dataReader["hreS_WorkingDays"].ToString());
                                            cumDTO.HRES_FromDate = Convert.ToDateTime(dataReader["hreS_FromDate"].ToString());
                                            cumDTO.HRES_ToDate = Convert.ToDateTime(dataReader["hreS_ToDate"].ToString());
                                            cumDTO.HRME_DOJ = Convert.ToDateTime(dataReader["hrmE_DOJ"].ToString());
                                            cumDTO.HRME_EmployeeCode = dataReader["hrmE_EmployeeCode"].ToString();
                                            cumDTO.LOPDays = Convert.ToInt64(dataReader["HRELT_TotDays"]);
                                            cumDTO.lopamount = Convert.ToInt64(dataReader["LOPAmount"]);
                                            cumDTO.plopDays = Convert.ToInt64(dataReader["Previousmonthlop"]);
                                            
                                            cumDTO.HRME_FPFNotApplicableFlg = Convert.ToBoolean(dataReader["hrmE_FPFNotApplicableFlg"].ToString());
                                            cumDTO.HRES_AccountNo = dataReader["hreS_AccountNo"].ToString();
                                            cumDTO.HRME_PFAccNo = dataReader["hrmE_PFAccNo"].ToString();
                                            cumDTO.HRME_age = Convert.ToInt64(dataReader["Age"]);
                                            cumDTO.HRES_WorkingDays = Convert.ToInt64(dataReader["HRES_WorkingDays"]);
                                            


                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                finally
                                {
                                    cmd.Connection.Close();
                                }
                            }


                            if (cumDTO.LOPDays > 0)
                            {
                                Lopdays = cumDTO.LOPDays;

                                LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(CurrentHRME_Id.HRES_DailyRates);
                            }
                            else
                            {
                                Lopdays = 0;
                            }


                           



                            var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                              from mas in _HRMSContext.HR_Master_EarningsDeductions
                                              where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id && mas.HRMED_EDTypeFlag == "Basic Pay")
                                              select new CumulativeSalaryReportDTO
                                              {
                                                  HRESD_Amount = emp.HREED_Amount
                                              }).ToList();

                            cumDTO.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                            var grosspayhead = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                                from mas in _HRMSContext.HR_Master_EarningsDeductions
                                                where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Gross" && mas.MI_Id == dto.MI_Id)
                                                select new CumulativeSalaryReportDTO
                                                {
                                                    HRESD_Amount = emp.HREED_Amount
                                                }).ToList();

                            if (grosspayhead.Count() > 0)
                            {
                                cumDTO.grosspayhead = Convert.ToDecimal(grosspayhead.Sum(t => t.HRESD_Amount));
                            }
                            else { }

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
                                                List<HR_Employee_Salary_DetailsDTO> currentdata = new List<HR_Employee_Salary_DetailsDTO>();

                                                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "HR_employeesalaryearningdeductiondetails";
                                                    cmd.CommandTimeout = 900000000;
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@HRMED_Id", SqlDbType.BigInt)
                                                    {
                                                        Value = head.HRMED_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@HRES_Id", SqlDbType.BigInt)
                                                    {
                                                        Value = CurrentHRME_Id.HRES_Id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@approvalflg", SqlDbType.VarChar)
                                                    {
                                                        Value = dto.comm
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
                                                                currentdata.Add(new HR_Employee_Salary_DetailsDTO
                                                                {
                                                                    HRESD_Id = Convert.ToInt64(dataReader["hresD_Id"].ToString()),
                                                                    HRES_Id = Convert.ToInt64(dataReader["hreS_Id"].ToString()),
                                                                    HRMED_Id = Convert.ToInt64(dataReader["hrmeD_Id"].ToString()),
                                                                    HRMED_Name = dataReader["hrmeD_Name"].ToString(),
                                                                    HRESD_Amount = Convert.ToDecimal(dataReader["hresD_Amount"].ToString()),
                                                                    HRMED_EarnDedFlag = dataReader["hrmeD_EarnDedFlag"].ToString()

                                                                });
                                                            }
                                                        }

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    finally
                                                    {
                                                        cmd.Connection.Close();
                                                    }
                                                }

                                                if (currentdata.Count() > 0)
                                                {

                                                    decimal? HRESD_Amount = currentdata.Sum(t => t.HRESD_Amount);

                                                    ss.HRESD_Id = currentdata.FirstOrDefault().HRESD_Id;
                                                    ss.HRMED_Id = head.HRMED_Id;
                                                    ss.HRMED_Name = currentdata.FirstOrDefault().HRMED_Name;
                                                    ss.HRESD_Amount = Math.Round(Convert.ToDecimal(HRESD_Amount), 2);
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

                                    cumDTO.grossEarning = Math.Round(Convert.ToDecimal(cumDTO.grossEarning + HRESD_Amount), 2);


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

                                    cumDTO.grossDeduction = Math.Round(Convert.ToDecimal(cumDTO.grossDeduction + HRESD_Amount), 2);
                                }

                                if (PayrollStandard.HRC_PayMethodFlg.Equals("Method1"))
                                {
                                        // cumDTO.netSalary = Math.Round(Convert.ToDecimal((cumDTO.grossEarning - cumDTO.grossDeduction) - LopAmount), 0);
                                        cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 2);
                                }
                                else
                                {
                                    cumDTO.netSalary = Math.Round(Convert.ToDecimal(cumDTO.grossEarning - cumDTO.grossDeduction), 2);
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
                    dto.employeeSalaryslipDetails = cumDTOList.OrderBy(t => t.HRME_EmployeeOrder).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> getCumulativeSalaryReport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report dto)
        {
            try
            {
                string HRMDES_Id = "0";
                if (dto.hrmdeS_IdList != null && dto.hrmdeS_IdList.Length > 0)
                {
                    foreach (var d in dto.hrmdeS_IdList)
                    {

                        HRMDES_Id = HRMDES_Id + ',' + d;
                    }

                }

                dto.earningdetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Earning").OrderBy(t => t.HRMED_Order).ToArray();
                dto.dectuiondetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Deduction").OrderBy(t => t.HRMED_Order).ToArray();


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Cumulative_Salary_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = HRMDES_Id
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeSalaryslipDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO data)
        {
            try
            {
                //data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                //                           from b in _HRMSContext.HR_Master_Department
                //                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                //                           select b).Distinct().ToArray();

                data.departmentdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO data)
        {
            try
            {
                //data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                //                            from b in _HRMSContext.HR_Master_Designation
                //                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                //                            select b).Distinct().ToArray();

                data.designationdropdown = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<HR_Employee_SalaryDTO> getEmployeedetailsByDepartment(HR_Employee_SalaryDTO dto)
        {
            List<CumulativeSalaryReportDTO> cumDTOList = new List<CumulativeSalaryReportDTO>();
            CumulativeSalaryReportDTO cumDTO = new CumulativeSalaryReportDTO();
            List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                //  Employee Salary Details
                employeSalary = (from a in _HRMSContext.MasterEmployee
                                 from b in _HRMSContext.HR_Employee_Salary
                                 where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                 && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true
                                 select b).Distinct().OrderBy(t => t.HRMD_Id).ToList();
                if (employeSalary.Count > 0)
                {
                    PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                   from pa in _HRMSContext.HR_PROCESSDMO
                                   from cc in _HRMSContext.Staff_User_Login
                                   where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId && pa.HRPA_TypeFlag == "Salary")
                                   select pa
                       ).ToList();

                    if (PROCESSList.Count() > 0)
                    {
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) || dto.hrmdeS_IdList.Contains(t.HRMDES_Id) || dto.hrmD_IdList.Contains(t.HRMD_Id) || dto.groupTypeIdList.Contains(t.HRMGT_Id)).OrderBy(t => t.HRMD_Id).ToList();
                    }
                    else
                    {
                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).OrderBy(t => t.HRMD_Id).ToList();

                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).OrderBy(t => t.HRMD_Id).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).OrderBy(t => t.HRMD_Id).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).OrderBy(t => t.HRMD_Id).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).OrderBy(t => t.HRMD_Id).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).OrderBy(t => t.HRMD_Id).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).OrderBy(t => t.HRMD_Id).ToList();
                        }

                    }

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                    var optionsBuilder = new DbContextOptionsBuilder<HRMSContext>();

                    optionsBuilder.UseSqlServer("Data Source = kusumavaps.database.windows.net, 1433; Initial Catalog = vapskusuma; Persist Security Info = False; User ID = vapskusuma; Password = @zure2021V@p$EcaMpU$; Connection Timeout = 30;");


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

                            var LOPcal = (from A in _HRMSContext.HR_Emp_Leave_Trans
                                          from B in _HRMSContext.HR_Master_Leave
                                          from C in _HRMSContext.HR_Emp_Leave_Trans_Details
                                          where (B.HRML_Id == A.HRELT_LeaveId &&
                                          A.MI_Id.Equals(dto.MI_Id) && A.HRME_Id == CurrentHRME_Id.HRME_Id &&
                                          A.HRELT_ActiveFlag == true && C.HRELT_Id == A.HRELT_Id && C.HRELTD_LWPFlag == true
                                          && ((A.HRELT_FromDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_FromDate <= CurrentHRME_Id.HRES_ToDate)
                                          || (A.HRELT_ToDate >= CurrentHRME_Id.HRES_FromDate && A.HRELT_ToDate <= CurrentHRME_Id.HRES_ToDate))
                                          )
                                          select A).ToList();
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
                                      from hrdes in _db.HR_Master_Designation
                                      from hrgrd in _db.HR_Master_Grade
                                      where (HRME.HRME_Id == HRES.HRME_Id
                                      && HRES.HRES_Id == CurrentHRME_Id.HRES_Id
                                      && hrdes.HRMDES_Id == HRES.HRMDES_Id
                                      && hrgrd.HRMG_Id == HRME.HRMG_Id//checking condition
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
                                          HRME_EmployeeOrder = HRME.HRME_EmployeeOrder,
                                          HRMDES_Designationname = hrdes.HRMDES_DesignationName,
                                          HRMG_GradeName = hrgrd.HRMG_GradeName,
                                          HRME_FPFNotApplicableFlg = HRME.HRME_FPFNotApplicableFlg,
                                          HRMG_ORDER = hrgrd.HRMG_Order,
                                          HRMD_Id = HRME.HRMD_Id,
                                          HRES_AccountNo = HRES.HRES_AccountNo
                                      }).FirstOrDefault();


                            var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                              from mas in _HRMSContext.HR_Master_EarningsDeductions
                                              where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Earning" && mas.MI_Id == dto.MI_Id && mas.HRMED_EDTypeFlag == "Basic Pay")
                                              select new CumulativeSalaryReportDTO
                                              {
                                                  HRESD_Amount = emp.HREED_Amount

                                              }).ToList();

                            cumDTO.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));

                            var grosspayhead = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                                from mas in _HRMSContext.HR_Master_EarningsDeductions
                                                where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == CurrentHRME_Id.HRME_Id && mas.HRMED_EarnDedFlag == "Gross" && mas.MI_Id == dto.MI_Id)
                                                select new CumulativeSalaryReportDTO
                                                {
                                                    HRESD_Amount = emp.HREED_Amount

                                                }).ToList();

                            if (grosspayhead.Count() > 0)
                            {

                                cumDTO.grosspayhead = Convert.ToDecimal(grosspayhead.Sum(t => t.HRESD_Amount));

                            }

                            else { }

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
                                                List<HR_Employee_Salary_DetailsDTO> currentdata = new List<HR_Employee_Salary_DetailsDTO>();
                                                if (dto.comm == "1")
                                                {
                                                    currentdata = (from HRES in _db.HR_Employee_Salary
                                                                   from HRESD in _db.HR_Employee_Salary_Details
                                                                   from HRMED in _db.HR_Master_EarningsDeductions

                                                                   where (HRESD.HRES_Id == HRES.HRES_Id &&
                                                                            HRESD.HRMED_Id == HRMED.HRMED_Id &&
                                                                            HRESD.HRES_Id == CurrentHRME_Id.HRES_Id &&
                                                                            HRESD.HRMED_Id == head.HRMED_Id && HRES.HRES_ApproveFlg == false
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
                                                }
                                                else
                                                {
                                                    currentdata = (from HRES in _db.HR_Employee_Salary
                                                                   from HRESD in _db.HR_Employee_Salary_Details
                                                                   from HRMED in _db.HR_Master_EarningsDeductions

                                                                   where (HRESD.HRES_Id == HRES.HRES_Id &&
                                                                            HRESD.HRMED_Id == HRMED.HRMED_Id &&
                                                                            HRESD.HRES_Id == CurrentHRME_Id.HRES_Id && HRES.HRES_ApproveFlg == true &&
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
                                                                   }).OrderBy(t=>t.HRMED_Order).ToList();
                                                }

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
                    dto.employeeSalaryslipDetails = cumDTOList.OrderBy(t => t.HRMD_Id).ToArray();
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

        public async Task<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> yearlyreport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report dto)
        {

            try
            {

                Institution institute = new Institution();
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();



                var monthnames = "";

                String monthselected = "";

                foreach (var x in dto.monthselected)
                {
                    monthselected += x.IVRM_Month_Id + ",";

                }
                monthselected = monthselected.Substring(0, (monthselected.Length - 1));



                string HRMDES_Id = "0";
                if (dto.hrmdeS_IdList != null && dto.hrmdeS_IdList.Length > 0)
                {
                    foreach (var d in dto.hrmdeS_IdList)
                    {

                        HRMDES_Id = HRMDES_Id + ',' + d;
                    }

                }

                dto.earningdetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Earning").OrderBy(t => t.HRMED_Order).ToArray();
                dto.dectuiondetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Deduction").OrderBy(t => t.HRMED_Order).ToArray();


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Cumulative_Salary_Yearly_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                    {
                        Value = dto.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                    {
                        Value = monthselected
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    {
                        Value = HRMDES_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeSalaryslipDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public async Task<HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report> headwisereport(HR_Employee_SalaryDTO.HR_Cumulative_Salary_Report dto)
        {

            try
            {

                Institution institute = new Institution();
                dto.institutionDetails = _Context.Institution.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();



                var monthnames = "";

                String monthselected = "";

                foreach (var x in dto.monthselected)
                {
                    monthselected += x.IVRM_Month_Id + ",";

                }
                monthselected = monthselected.Substring(0, (monthselected.Length - 1));



                string HRMDES_Id = "0";
                if (dto.hrmdeS_IdList != null && dto.hrmdeS_IdList.Length > 0)
                {
                    foreach (var d in dto.hrmdeS_IdList)
                    {

                        HRMDES_Id = HRMDES_Id + ',' + d;
                    }

                }

                dto.earningdetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Earning").OrderBy(t => t.HRMED_Order).ToArray();
                dto.dectuiondetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Deduction").OrderBy(t => t.HRMED_Order).ToArray();


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Headwise_Salary";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                    {
                        Value = dto.HRES_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                    {
                        Value = monthselected
                    });

                    //cmd.Parameters.Add(new SqlParameter("@HRMDES_Id", SqlDbType.VarChar)
                    //{
                    //    Value = HRMDES_Id
                    //});


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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeeSalaryslipDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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