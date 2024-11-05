using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
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
    public class MonthEndReportService : Interfaces.MonthEndReportInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MonthEndReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public MonthEndReportDTO getBasicData(MonthEndReportDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public MonthEndReportDTO GetAllDropdownAndDatatableDetails(MonthEndReportDTO dto)
        {
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            try
            {
                //leave year
                dto.leaveyeardropdown = _HRMSContext.HR_MasterLeaveYear.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();

                GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                dto.groupTypedropdown = GroupTypelist.ToArray();

                //Departmentlist
                Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public async Task<MonthEndReportDTO> getEmployeedetailsBySelection(MonthEndReportDTO dto)
        {
            List<HR_Employee_Salary> employeeSalaryDetails = new List<HR_Employee_Salary>();
            List<MasterEmployee> employeeDetails = new List<MasterEmployee>();
            try
            {
                Institution institute = new Institution();
                institute = _Context.Institution.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                dto.institutionDetails = dmoObj;

                //get month id by month name
                long IVRM_Month_Id = _Context.month.Where(t => t.Is_Active == true && t.IVRM_Month_Name.Equals(dto.HRES_Month)).FirstOrDefault().IVRM_Month_Id;

                //employee list
                //   employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.empid = _HRMSContext.MasterEmployee.Where(t => dto.groupTypeIdList.Contains(t.HRMGT_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && t.MI_Id == dto.MI_Id && t.HRME_ActiveFlag == true).Select(t => t.HRME_Id).ToArray();

                string date = dto.HRES_Year + "-" + IVRM_Month_Id + "-" + DateTime.DaysInMonth(Convert.ToInt32(dto.HRES_Year), Convert.ToInt32(IVRM_Month_Id));

                //workingEmployee
                //List<MasterEmployee> workingEmployee = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= IVRM_Month_Id && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToInt32(dto.HRES_Year) && t.HRME_ActiveFlag == true && dto.empid.Contains(t.HRME_Id)).ToList();
                List<MasterEmployee> workingEmployee = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_DOJ <= Convert.ToDateTime(date) && t.HRME_LeftFlag == false && t.HRME_ActiveFlag == true && dto.empid.Contains(t.HRME_Id)).ToList();

                dto.workingEmployee = workingEmployee.Count();

                // Missing Details of working Employees
                //missingPhoto

                dto.missingPhoto = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= IVRM_Month_Id && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToInt32(dto.HRES_Year) && t.HRME_Photo == null && t.HRME_ActiveFlag == true && dto.empid.Contains(t.HRME_Id)).Count();

                //missingEmailId
               // dto.missingEmailId = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= IVRM_Month_Id && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToInt32(dto.HRES_Year) && (t.HRME_EmailId == null || t.HRME_EmailId == "") && t.HRME_ActiveFlag == true).Count();

                var EmplForEmail = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= IVRM_Month_Id && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToInt32(dto.HRES_Year) && t.HRME_ActiveFlag == true && dto.empid.Contains(t.HRME_Id)).ToList();
                if (EmplForEmail.Count() > 0)
                {
                    var EmplForEmailCount = _HRMSContext.Emp_Email_Id.Where(t => EmplForEmail.Select(a=>a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForEmailCount.Count() > 0)
                    {
                        dto.missingEmailId = EmplForEmail.Where(u=>!EmplForEmailCount.Contains(u.HRME_Id)).Count();
                    }else
                    {
                        dto.missingEmailId = EmplForEmail.Count();
                    }
                }else
                {
                    dto.missingEmailId = 0;
                }

                //missingContactNumber

                // dto.missingContactNumber = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= IVRM_Month_Id && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToInt32(dto.HRES_Year) && t.HRME_MobileNo == 0 && t.HRME_ActiveFlag == true).Count();

                var EmplForContactNumber = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= IVRM_Month_Id && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToInt32(dto.HRES_Year) && t.HRME_ActiveFlag == true && dto.empid.Contains(t.HRME_Id)).ToList();

                if (EmplForContactNumber.Count() > 0)
                {

                    var EmplForContactNumberCount = _HRMSContext.Emp_MobileNo.Where(t => EmplForContactNumber.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForContactNumberCount.Count() > 0)
                    {
                        dto.missingContactNumber = EmplForContactNumber.Where(u => !EmplForContactNumberCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        dto.missingContactNumber = EmplForContactNumber.Count();
                    }


                }
                else
                {
                    dto.missingContactNumber = 0;
                }



                //leftEmployee
                List<MasterEmployee> leftEmployee = _HRMSContext.MasterEmployee.Where(u => Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).ToList();

                dto.leftEmployee = leftEmployee.Count();
                // Missing Details of left Employees

                //missingPhoto

                dto.missingPhotoleft = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_Photo == null && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).Count();


                //missingEmailId
              //  dto.missingEmailIdleft = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && (u.HRME_EmailId == null || u.HRME_EmailId == "") && u.HRME_ActiveFlag == true).Count();


                var EmplForEmailleft = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true  && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).ToList();

                if (EmplForEmailleft.Count() > 0)
                {

                    var EmplForEmailleftCount = _HRMSContext.Emp_Email_Id.Where(t => EmplForEmailleft.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForEmailleftCount.Count() > 0)
                    {
                        dto.missingEmailIdleft = EmplForEmailleft.Where(u => !EmplForEmailleftCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        dto.missingEmailIdleft = EmplForEmailleft.Count();
                    }
                }
                else
                {
                    dto.missingEmailIdleft = 0;
                }


                //missingContactNumber

                //  dto.missingContactNumberleft = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_MobileNo == 0 && u.HRME_ActiveFlag == true).Count();


                var EmplForContactNumberleft = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOL).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOL).Year == Convert.ToInt32(dto.HRES_Year) && Convert.ToBoolean(u.HRME_LeftFlag) == true && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).ToList();

                if (EmplForContactNumberleft.Count() > 0)
                {

                    var EmplForContactNumberleftCount = _HRMSContext.Emp_MobileNo.Where(t => EmplForContactNumberleft.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForContactNumberleftCount.Count() > 0)
                    {
                        dto.missingContactNumberleft = EmplForContactNumberleft.Where(u => !EmplForContactNumberleftCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        dto.missingContactNumberleft = EmplForContactNumber.Count();
                    }
                }
                else
                {
                    dto.missingContactNumberleft = 0;
                }





                //newEmployee
                List<MasterEmployee> newEmployee = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOJ).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOJ).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).ToList();
                dto.newEmployee = newEmployee.Count();
                // Missing Details of new Employees

                //missingPhoto

                dto.missingPhotonew = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOJ).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOJ).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_Photo == null && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).Count();


                //missingEmailId
                //  dto.missingEmailIdnew = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOJ).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOJ).Year == Convert.ToInt32(dto.HRES_Year) && (u.HRME_EmailId == null || u.HRME_EmailId == "") && u.HRME_ActiveFlag == true).Count();

               var EmplForEmailnew = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOJ).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOJ).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).ToList();

                if (EmplForEmailnew.Count() > 0)
                {

                    var EmplForEmailnewCount = _HRMSContext.Emp_Email_Id.Where(t => EmplForEmailnew.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForEmailnewCount.Count() > 0)
                    {
                        dto.missingEmailIdnew = EmplForEmailnew.Where(u => !EmplForEmailnewCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        dto.missingEmailIdnew = EmplForEmailnew.Count();
                    }
                }
                else
                {
                    dto.missingEmailIdnew = 0;
                }


                //missingContactNumber

               // dto.missingContactNumbernew = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOJ).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOJ).Year == Convert.ToInt32(dto.HRES_Year) && u.HRME_MobileNo == 0 && u.HRME_ActiveFlag == true).Count();

               var EmplForContactNumbernew = _HRMSContext.MasterEmployee.Where(u => u.MI_Id.Equals(dto.MI_Id) && Convert.ToDateTime(u.HRME_DOJ).Month == IVRM_Month_Id && Convert.ToDateTime(u.HRME_DOJ).Year == Convert.ToInt32(dto.HRES_Year)  && u.HRME_ActiveFlag == true && dto.empid.Contains(u.HRME_Id)).ToList();

                if (EmplForContactNumbernew.Count() > 0)
                {

                    var EmplForContactNumbernewCount = _HRMSContext.Emp_MobileNo.Where(t => EmplForContactNumbernew.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForContactNumbernewCount.Count() > 0)
                    {
                        dto.missingContactNumbernew = EmplForContactNumbernew.Where(u => !EmplForContactNumbernewCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        dto.missingContactNumbernew = EmplForContactNumbernew.Count();
                    }
                }
                else
                {
                    dto.missingContactNumbernew = 0;
                }



                //salaryGenerated
                dto.salaryGenerated = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.empid.Contains(t.HRME_Id)).Count();

                //salaryslipGenerated
                dto.salaryslipGenerated = _HRMSContext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRES_Year.Equals(dto.HRES_Year) && t.HRES_Month.Equals(dto.HRES_Month) && dto.empid.Contains(t.HRME_Id)).Count();

                //dto.salaryslipsent = (from a in _HRMSContext.MasterEmployee
                //                      from b in _HRMSContext.Emp_Email_Id
                //                      from c in _HRMSContext.ivrm_email_sentbox
                //                      where (a.MI_Id == dto.MI_Id && dto.empid.Contains(a.HRME_Id) && a.HRME_Id == b.HRME_Id && a.MI_Id == c.MI_Id && c.Module_Name == "HRMS" && Convert.ToDateTime(c.Datetime).Month == IVRM_Month_Id && Convert.ToDateTime(c.Datetime).Year == Convert.ToInt32(dto.HRES_Year) && b.HRMEM_EmailId == c.Email_Id)
                //                      select c).Count();
                string multipleHRME_ID = "";
                if (dto.empid.Length > 0)
                {
                    for (int i = 0; i < dto.empid.Length; i++)
                    {
                        if (multipleHRME_ID == "") { multipleHRME_ID = dto.empid[i].ToString(); }
                        else { multipleHRME_ID = multipleHRME_ID + "," + dto.empid[i].ToString(); }
                    }
                }

                try
                {
                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HRMS_EMAILSENTCOUNT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@month",
                            SqlDbType.BigInt)
                        {
                            Value = IVRM_Month_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
                           SqlDbType.BigInt)
                        {
                            Value = dto.HRES_Year
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                      SqlDbType.VarChar)
                        {
                            Value = multipleHRME_ID
                        });

                        cmd.Parameters.Add(new SqlParameter("@miid",
                      SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
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
                                dto.emailcount = retObject.ToArray();
                                //dto.salaryslipsent = Convert.ToInt32(dto.emailcount[0].tos);
                            }
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


                //dto.salaryslipsent=_HRMSContext.ivrm_email_sentbox.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.Module_Name=="HRMS" && Convert.ToDateTime(t.Datetime).Month == IVRM_Month_Id && Convert.ToDateTime(t.Datetime).Year == Convert.ToInt32(dto.HRES_Year)).Count();

                //dto.salaryslipsmssent = (from a in _HRMSContext.MasterEmployee
                //                      from b in _HRMSContext.Emp_MobileNo
                //                         from c in _HRMSContext.IVRM_sms_sentBoxDMO
                //                         where (a.MI_Id == dto.MI_Id && dto.empid.Contains(a.HRME_Id) && a.HRME_Id == b.HRME_Id && a.MI_Id == c.MI_Id && c.Module_Name == "HRMS" && Convert.ToDateTime(c.Datetime).Month == IVRM_Month_Id && Convert.ToDateTime(c.Datetime).Year == Convert.ToInt32(dto.HRES_Year) && b.HRMEMNO_MobileNo.ToString().Trim() == c.Mobile_no)
                //                      select c).Count();

                try
                {
                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HRMS_SMSSENTCOUNT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@month",
                            SqlDbType.BigInt)
                        {
                            Value = IVRM_Month_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
                           SqlDbType.BigInt)
                        {
                            Value = dto.HRES_Year
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                      SqlDbType.VarChar)
                        {
                            Value = multipleHRME_ID
                        });

                        cmd.Parameters.Add(new SqlParameter("@miid",
                      SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
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
                                dto.smscount = retObject.ToArray();
                                //dto.salaryslipsmssent = retObject[0];
                            }
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

                //dto.salaryslipsmssent = _HRMSContext.IVRM_sms_sentBoxDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.Module_Name == "HRMS" && Convert.ToDateTime(t.Datetime).Month == IVRM_Month_Id && Convert.ToDateTime(t.Datetime).Year == Convert.ToInt32(dto.HRES_Year)).Count();

                //monthendDate

                dto.monthendDate = "VAPS/IVRM/HRMS/" + IVRM_Month_Id + "/" + dto.HRES_Year;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

    }
}
