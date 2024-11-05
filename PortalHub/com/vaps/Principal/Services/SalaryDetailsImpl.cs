using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Services
{
    public class SalaryDetailsImpl : Interfaces.SalaryDetailsInterface
    {

        public PortalContext _PrincipalDashboardContext;
        public DomainModelMsSqlServerContext _db;
        // private object _PrincipalDashboardContext;

        //ILogger<SalaryDetailsImpl> _acdimpl;
        public SalaryDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
        }

        public SalaryDetailsDTO getBasicData(SalaryDetailsDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }

        public SalaryDetailsDTO Getdepartment(SalaryDetailsDTO dto)
        {
            List<Month> Monthlist = new List<Month>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            //GroupTypelist
           var GroupTypelist = _PrincipalDashboardContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id)  && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
            dto.groupTypedropdown = GroupTypelist.ToArray();



            var departmentdropdown = _PrincipalDashboardContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            dto.departmentdropdown = departmentdropdown.ToArray();

            Monthlist = _db.month.Where(t => t.Is_Active == true).ToList();
            dto.monthdropdown = Monthlist.ToArray();

            leaveyear = _PrincipalDashboardContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && Convert.ToBoolean(t.HRMLY_ActiveFlag) == true).ToList();
            dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
            return dto;
        }

        public SalaryDetailsDTO get_designation(SalaryDetailsDTO dto)
        {
            dto.designationdropdown = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                                       from b in _PrincipalDashboardContext.HR_Master_Designation
                                       from c in _PrincipalDashboardContext.HR_Master_Department
                                       where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                       && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                       && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == dto.MI_Id && dto.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                       select new SalaryDetailsDTO
                                       {
                                           HRMDES_Id = b.HRMDES_Id,
                                           HRMDES_DesignationName = b.HRMDES_DesignationName,
                                       }
                     ).Distinct().ToArray();

            return dto;
        }

        public SalaryDetailsDTO get_department(SalaryDetailsDTO dto)
        {
            //dto.departmentdropdown = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
            //                           from b in _PrincipalDashboardContext.HR_Master_GroupType
            //                           from c in _PrincipalDashboardContext.HR_Master_Department
            //                           where (a.HRMGT_Id == b.HRMGT_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
            //                           && b.HRMGT_ActiveFlag == true && a.HRME_ActiveFlag == true
            //                           && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == dto.MI_Id && dto.multipledep.ToString().Contains(Convert.ToString(b.HRMGT_Id)))
            //                           select new SalaryDetailsDTO
            //                           {
            //                               HRMGT_Id = b.HRMGT_Id,
            //                                HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
            //                           }
            //         ).Distinct().ToArray();

            dto.departmentdropdown = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                      from b in _PrincipalDashboardContext.HR_Master_Department
                   where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && dto.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(dto.MI_Id) && b.HRMD_ActiveFlag == true)
                          select b).Distinct().ToArray();

            return dto;
        }
        public SalaryDetailsDTO get_employee(SalaryDetailsDTO dto)
        {
            dto.stafflist = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                             where (a.MI_Id == dto.MI_Id && dto.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id)) && dto.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag==false)
                             select new SalaryDetailsDTO
                             {
                                 HRME_Id = a.HRME_Id,
                                 HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                 HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                 HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                             }
                     ).Distinct().OrderBy(t=>t.HRME_EmployeeFirstName).ToArray();
            return dto;
        }

        public SalaryDetailsDTO GetAllDropdownAndDatatableDetails(SalaryDetailsDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();

            try
            {
                //leave year
                Monthlist = _db.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();
                //employee

                //employe = (from a in _PrincipalDashboardContext.MasterEmployee
                //           from b in _PrincipalDashboardContext.HR_Employee_Salary
                //           where (a.HRME_Id == b.HRME_Id && a.MI_Id.Equals(dto.MI_Id) && a.HRME_ActiveFlag == true)
                //           select a).ToList();


                //  employe = _PrincipalDashboardContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).ToList();
                //dto.employeedropdown = employe.ToArray();

                //leave year
                leaveyear = _PrincipalDashboardContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();



                //dto.groupTypedropdown = _PrincipalDashboardContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();
                //employee  
                //employe = _PrincipalDashboardContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) ).ToList();
                //dto.employeedropdown = employe.ToArray();

                //departmentdropdown
                //dto.departmentdropdown = _PrincipalDashboardContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                //designationdropdown 
                //dto.designationdropdown = _PrincipalDashboardContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public SalaryDetailsDTO GetEmployeeDetailsByLeaveYearAndMonth(SalaryDetailsDTO dto)
        {

            //List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_Employee_DMO> employe = new List<HR_Master_Employee_DMO>();

            try
            {
                //employee

                long IVRM_Month_Id = _db.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).FirstOrDefault().IVRM_Month_Id;
                var config = _PrincipalDashboardContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();

                //employee list

                string selecteddate = "" + Convert.ToInt32(dto.HRES_Year) + "-" + IVRM_Month_Id + "-" + config.HRC_SalaryFromDay + "";


                employe = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                           from b in _PrincipalDashboardContext.HR_Employee_Salary
                           where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                           && b.HRES_Year.Equals(dto.HRES_Year) && b.HRES_Month.Equals(dto.HRES_Month) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag==false
                           select a).Distinct().ToList();
                if (employe.Count > 0)
                {
                    employe = employe.Where(a => a.HRME_LeftFlag == false && Convert.ToDateTime(a.HRME_DOJ) <= Convert.ToDateTime(selecteddate)).ToList();


                    if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }

                }
                dto.employeedropdown = employe.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public async Task<SalaryDetailsDTO> GenerateEmployeeSalarySlip(SalaryDetailsDTO dto)
        {
            dto = await getEmployeeSalarySlip(dto);

            return dto;
        }

        public async Task<SalaryDetailsDTO> getEmployeeSalarySlip(SalaryDetailsDTO dto)
        {
            try
            {

                if (dto.serchtype=="2")
                {
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmployeeSalarySlipGeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                            SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.HRME_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",
                      SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.HRES_Year)
                        });

                        cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                      SqlDbType.VarChar)
                        {
                            Value = Convert.ToString(dto.HRES_Month)
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {

                            // var data = cmd.ExecuteNonQuery();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
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
                else if (dto.serchtype=="1")
                {
                    //foreach (var item in dto.empids)
                    //{

                    //}

                    var fmt_ids = "";
                    
                        foreach (var x in dto.empids)
                        {
                            fmt_ids += x + ",";
                        }
                        fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmployeeSalarySlipGeneration_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                            SqlDbType.VarChar)
                        {
                            Value = fmt_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                           SqlDbType.VarChar)
                        {
                            Value =dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",
                      SqlDbType.VarChar)
                        {
                            Value =dto.HRES_Year
                        });

                        cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                      SqlDbType.VarChar)
                        {
                            Value = dto.HRES_Month
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {

                            // var data = cmd.ExecuteNonQuery();

                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
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
               

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public static string NumberToWords(long number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

    }
}
