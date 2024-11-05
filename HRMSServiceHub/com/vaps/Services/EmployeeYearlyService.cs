using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.admission;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeYearlyService : Interfaces.EmployeeYearlyReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeYearlyService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public EmployeeYearlyReportDTO getBasicData(EmployeeYearlyReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public EmployeeYearlyReportDTO GetAllDropdownAndDatatableDetails(EmployeeYearlyReportDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            //   List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();



            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            List<MasterEmployee> emp = new List<MasterEmployee>();
            try
            {
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

                    emp = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = emp.ToArray();

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
                    dto.employeedropdown = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

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

               leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
               dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }
        public EmployeeYearlyReportDTO FilterEmployeedetailsBySelection(EmployeeYearlyReportDTO dto)
        {
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            try
            {
                if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();

                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                }

                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }

                dto.employeedropdown = employe.ToArray();
                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public async Task<EmployeeYearlyReportDTO> getEmployeedetailsBySelection(EmployeeYearlyReportDTO dto)
        {
            List<EmployeeYearlyReportDTO> cumDTOList = new List<EmployeeYearlyReportDTO>();
            EmployeeYearlyReportDTO cumDTO = new EmployeeYearlyReportDTO();
            List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
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
                                 from b in _HRMSContext.HR_Employee_Salary
                                 where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id))
                                 && b.HRES_Year.Equals(dto.HRES_Year) && a.HRME_Id == dto.HRME_Id && a.HRME_ActiveFlag == true
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
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) || dto.hrmdeS_IdList.Contains(t.HRMDES_Id) || dto.hrmD_IdList.Contains(t.HRMD_Id) || dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();


                    }
                    else
                    {

                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();

                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.groupTypeIdList.Contains(t.HRMGT_Id) && t.HRME_Id == dto.HRME_Id).ToList();
                        }

                    }

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));


                    //var hashSet = new HashSet<HR_Employee_Salary>(employeSalary);
                    //for (int x = 0; x < hashSet.Count; x++)
                    //{
                    //    var CurrentHRME_Id = hashSet.ElementAt(x);
                    //    Task tTemp = Task.Run(() =>
                    //    {

                    //        decimal Lopdays = 0;
                    //        decimal LopAmount = 0;
                    //        //LOP Calculation


                  


                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HRMS_Pivot";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                          SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
                        }); cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                            SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.HRME_Id)
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRES_Year",
                      SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.HRES_Year)
                        });

                        //  cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                        //SqlDbType.VarChar)
                        //  {
                        //      Value = Convert.ToString(dto.HRES_Month)
                        //  });

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


                dto.head = (from a in _HRMSContext.HR_Employee_EarningsDeductions
                                           from b in _HRMSContext.HR_Master_EarningsDeductions
                                          where (a.MI_Id == b.MI_Id && a.HREED_ActiveFlag==true && a.HRMED_Id == b.HRMED_Id && a.MI_Id==dto.MI_Id)
                                           select a).Distinct().ToArray();

               // dto.head = departmentdropdown;


            }  
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public EmployeeYearlyReportDTO get_depts(EmployeeYearlyReportDTO data)
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
        public EmployeeYearlyReportDTO get_desig(EmployeeYearlyReportDTO data)
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
        public async Task<EmployeeYearlyReportDTO> reportBetweenDatesBySelection(EmployeeYearlyReportDTO dto)
        {
            List<EmployeeYearlyReportDTO> cumDTOList = new List<EmployeeYearlyReportDTO>();
            EmployeeYearlyReportDTO cumDTO = new EmployeeYearlyReportDTO();
            List<HR_Employee_Salary> employeSalary = new List<HR_Employee_Salary>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            try
            {
                int frommonth = DateTime.Parse(dto.HRME_Fromdate).Month;
                int tomonth = DateTime.Parse(dto.HRME_Todate).Month;
                int fromyear = DateTime.Parse(dto.HRME_Fromdate).Year;
                int toyear = DateTime.Parse(dto.HRME_Todate).Year;

                //Institution Details

                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                dto.HRME_EmployeeFirstName = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).Select(t => t.HRME_EmployeeFirstName + " " + t.HRME_EmployeeMiddleName + " " + t.HRME_EmployeeLastName).FirstOrDefault();

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                //Employee Salary Details
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HRMS_Pivot_Between";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    }); cmd.Parameters.Add(new SqlParameter("@HRME_ID",SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRES_Year",SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",SqlDbType.Date)
                    {
                        Value = Convert.ToDateTime(dto.HRME_Fromdate)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",SqlDbType.Date)
                    {
                        Value = Convert.ToDateTime(dto.HRME_Todate)
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
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

                employeSalary = (from a in _HRMSContext.MasterEmployee
                                  from b in _HRMSContext.HR_Employee_Salary
                                  from c in _HRMSContext.Month
                                  where (b.HRME_Id == a.HRME_Id && b.MI_Id.Equals(dto.MI_Id)) && b.HRES_Month == c.IVRM_Month_Name
                                  && (Convert.ToInt32(b.HRES_Year) >= fromyear && c.IVRM_Month_Id >= frommonth) && a.HRME_Id == dto.HRME_Id && a.HRME_ActiveFlag == true && (Convert.ToInt32(b.HRES_Year) <= toyear && c.IVRM_Month_Id <= tomonth)
                                  select b).Distinct().ToList();

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
                        employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) || dto.hrmdeS_IdList.Contains(t.HRMDES_Id) || dto.hrmD_IdList.Contains(t.HRMD_Id) || dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                    }
                    else
                    {
                        if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.groupTypeIdList.Contains(t.HRMGT_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                        }
                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.groupTypeIdList.Count() == 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                        }

                        else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.groupTypeIdList.Count() > 0)
                        {
                            //employee
                            employeSalary = employeSalary.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.HRES_Year.Contains(t.HRES_Year) && dto.groupTypeIdList.Contains(t.HRMGT_Id) && t.HRME_Id == dto.HRME_Id).ToList();
                        }
                    }

                    HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));                    
                }

                dto.head = (from a in _HRMSContext.HR_Employee_EarningsDeductions
                            from b in _HRMSContext.HR_Master_EarningsDeductions
                            where (a.MI_Id == b.MI_Id && a.HREED_ActiveFlag == true && a.HRMED_Id == b.HRMED_Id && a.MI_Id == dto.MI_Id)
                            select a).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
