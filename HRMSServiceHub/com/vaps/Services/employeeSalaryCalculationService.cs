using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class employeeSalaryCalculationService : Interfaces.employeeSalaryCalculationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public employeeSalaryCalculationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
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
            // List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            List<Month> Monthlist = new List<Month>();

            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();

            try
            {



                //leave year
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).OrderBy(t=>t.HRMLY_LeaveYearOrder).ToList();
                dto.leaveyeardropdown = leaveyear.ToArray();

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


                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();

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
                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = employe.ToArray();

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





                //emptype
                //dto.employeeTypedropdown = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToArray();

                //employee


                //leave year


                //departmentdropdown
                //dto.departmentdropdown = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToArray();

                ////designationdropdown 
                //dto.designationdropdown = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToArray();

                //// employee grouptype
                //dto.groupTypedropdown = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToArray();

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


        public HR_Employee_SalaryDTO getEmployeedetailsBySelection(HR_Employee_SalaryDTO dto)
        {
            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            // List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
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

                    employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employe = employe.ToArray();

                    dto.employeedropdown = employe.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    ////Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();

                }
                else
                {

                    //EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    //dto.employeedetailList = EmployeeList.ToArray();

                    ////GroupTypelist
                    //GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    //dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    ////Departmentlist
                    //Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    //dto.departmentdropdownlist = Departmentlist.ToArray();

                    ////Designationlist
                    //Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    //dto.designationdropdownlist = Designationlist.ToArray();


                    // }


                    var ee = _HRMSContext.HR_Employee_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id)).Select(t => t.HRME_Id).Distinct();


                    if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();

                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() == 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();
                    }
                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();
                    }

                    else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                    {
                        //employee
                        employe = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true && ee.Contains(t.HRME_Id)).ToList();
                    }


                    if (Convert.ToInt32(dto.HRES_Year) > 0 && dto.HRES_Month != "")
                    {
                        //get month id by month name
                        var Month = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).ToList();
                        var config = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();
                        int IVRM_Month_Id = 0;
                        if (Month.Count > 0)
                        {
                            if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) < 12)
                            {

                                IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) + 1;
                            }
                            else if (config.HRC_SalaryFromDay > 1 && Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id) == 12)
                            {
                                IVRM_Month_Id = 01;
                                dto.HRES_Year = (Convert.ToInt32(dto.HRES_Year) + 1).ToString();
                            }
                            else
                            {
                                IVRM_Month_Id = Convert.ToInt32(Month.FirstOrDefault().IVRM_Month_Id);
                                // var days = getNumberOfDays(Convert.ToInt32(dto.HRES_Year), IVRM_Month_Id);
                                var days = DateTime.DaysInMonth(Convert.ToInt32(dto.HRES_Year), IVRM_Month_Id);
                                config.HRC_SalaryToDay = days;
                            }

                            //employee list
                            //   employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                            // string selectedFromdate = "" + config.HRC_SalaryFromDay +"-"+ IVRM_Month_Id +"-" + Convert.ToInt32(dto.HRES_Year) + "";
                            DateTime selectedFromdate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryFromDay, 0, 0, 0, 0);

                            // string selectedTodate = "" + config.HRC_SalaryToDay + "-" + IVRM_Month_Id + "-" + Convert.ToInt32(dto.HRES_Year) + "";
                            DateTime selectedTodate = new DateTime(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id), config.HRC_SalaryToDay, 0, 0, 0, 0);


                            var leftEmp = employe.Where(t => t.HRME_LeftFlag == true && Convert.ToDateTime(t.HRME_DOL) < Convert.ToDateTime(selectedFromdate)).Select(t => t.HRME_Id);
                            if (leftEmp.Count() > 0)
                            {
                                employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && !leftEmp.Contains(t.HRME_Id) == true).ToList();
                            }
                            else
                            {
                                employe = employe.Where(t => Convert.ToDateTime(t.HRME_DOJ) <= Convert.ToDateTime(selectedTodate) && t.HRME_ActiveFlag == true).ToList();
                            }

                        }

                    }

                    dto.employeedropdown = employe.ToArray();
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        //Salary calculation

        public async Task<HR_Employee_SalaryDTO> calculateSelectedEmployeeSalary(HR_Employee_SalaryDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));
                if (PayrollStandard != null)
                {
                    if (PayrollStandard.HRC_PayMethodFlg == "Method1")
                    {
                        dto = await calculateEmployeeSalaryByMethods(dto, "EmployeeSalaryGenerationMethodOne");
                    }
                    if (PayrollStandard.HRC_PayMethodFlg == "Method2")
                    {
                        dto = await calculateEmployeeSalaryByMethods(dto, "EmployeeSalaryGenerationMethodTwo");
                    }
                    if (PayrollStandard.HRC_PayMethodFlg == "Method3")
                    {
                        dto = await calculateEmployeeSalaryByMethods(dto, "EmployeeSalaryGenerationMethodThree");
                    }
                    if (PayrollStandard.HRC_PayMethodFlg == "Method4")
                    {
                        dto = await calculateEmployeeSalaryByMethods(dto, "EmployeeSalaryGenerationMethodFour");
                    }
                    if (PayrollStandard.HRC_PayMethodFlg == "Method5")
                    {
                        dto = await calculateEmployeeSalaryByMethods(dto, "EmployeeSalaryGenerationMethodFive");
                    }
                    if (PayrollStandard.HRC_PayMethodFlg == "Method6")
                    {
                        dto = await calculateEmployeeSalaryByMethods(dto, "EmployeeSalaryGenerationMethodSix");
                    }
                }
                else
                {
                    dto.retrunMsg = "ConfigurationMissing";
                    return dto;
                }
                dto.retrunMsg = "Generated";
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //Methods
        public async Task<HR_Employee_SalaryDTO> calculateEmployeeSalaryByMethods(HR_Employee_SalaryDTO dto, string ProcedureName)
        {

            try
            {
                foreach (MasterEmployeeDTO Employee in dto.masterEmployeeList)
                {
                    dto.HRES_AccountNo = Employee.HRME_BankAccountNo;
                    dto.HRES_BankCashFlag = Employee.HRME_PaymentType;
                    dto.HRMGT_Id = Employee.HRMGT_Id;
                    dto.HRMD_Id = Employee.HRMD_Id;
                    dto.HRMDES_Id = Employee.HRMDES_Id;

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = ProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                            SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(Employee.HRME_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                           SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_ID",
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
                            // retObject.Add((ExpandoObject)dataRow);


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




        public HR_Employee_SalaryDTO get_depts(HR_Employee_SalaryDTO data)
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



        public HR_Employee_SalaryDTO get_desig(HR_Employee_SalaryDTO data)
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
