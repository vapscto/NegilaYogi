using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Exit;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using Recruitment.com.vaps.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Exit_Employee_IMPL : Interfaces.Exit_Employee_Interface
    {
        public VMSContext _vmsconte;

        public Exit_Employee_IMPL(VMSContext vm)
        {
            _vmsconte = vm;
        }
        public ISM_Resignation_Master_Reasons_DTO Get_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                dto.get_reason_list = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMRE_Id > 0)
                {
                    var result = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Single(a => a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id);
                    result.ISMRESGMRE_ResignationReasons = dto.ISMRESGMRE_ResignationReasons;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMRE_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Update";
                }
                else
                {
                    ISM_Resignation_Master_Reasons_DMO rmr = new ISM_Resignation_Master_Reasons_DMO();
                    rmr.ISMRESGMRE_ResignationReasons = dto.ISMRESGMRE_ResignationReasons;
                    rmr.MI_Id = dto.MI_Id;
                    rmr.ISMRESGMRE_ActiveFlag = true;
                    rmr.CreatedDate = DateTime.Now;
                    rmr.ISMRESGMRE_CreatedBy = dto.userId;
                    _vmsconte.Add(rmr);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMRE_ActiveFlag == true)
                {
                    var result = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Single(a => a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id);
                    result.ISMRESGMRE_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMRE_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "false";
                }
                else
                {
                    var result = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Single(a => a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id);
                    result.ISMRESGMRE_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMRE_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "true";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_Reasons_DTO get_details_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                dto.get_details_reason_list = (from a in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                               where a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id
                                               select new ISM_Resignation_Master_Reasons_DTO
                                               {
                                                   ISMRESGMRE_ResignationReasons = a.ISMRESGMRE_ResignationReasons,
                                                   ISMRESGMRE_Id = a.ISMRESGMRE_Id
                                               }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        //==========================================End=============================================
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                dto.department_lisd_dd = _vmsconte.HR_Master_Department.Where(a => a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true).ToList().ToArray();
                //  dto.designation_list_dd = _vmsconte.HR_Master_Designation_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMDES_ActiveFlag == true).ToList().ToArray();
                dto.check_list = (from a in _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con
                                  from b in _vmsconte.HR_Master_Department
                                      // from c in _vmsconte.HR_Master_Designation_con
                                  where a.HRMD_Id == b.HRMD_Id && a.MI_Id == dto.MI_Id
                                  select new ISM_Resignation_Master_CheckLists_DTO
                                  {
                                      ISMRESGMCL_Id = a.ISMRESGMCL_Id,
                                      ISMRESGMCL_CheckListName = a.ISMRESGMCL_CheckListName,
                                      HRMD_DepartmentName = b.HRMD_DepartmentName,
                                      ISMRESGMCL_Remarks = a.ISMRESGMCL_Remarks,
                                      // HRMDES_DesignationName = c.HRMDES_DesignationName,
                                      ISMRESGMCL_ActiveFlag = a.ISMRESGMCL_ActiveFlag
                                  }).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMCL_Id > 0)
                {
                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Single(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id);
                    result.ISMRESGMCL_CheckListName = dto.ISMRESGMCL_CheckListName;
                    result.HRMD_Id = dto.HRMD_Id;
                    //result.HRMDES_Id = dto.HRMDES_Id;
                    result.ISMRESGMCL_Template = dto.ISMRESGMCL_Template;
                    result.ISMRESGMCL_Remarks = dto.ISMRESGMCL_Remarks;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMCL_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Update";
                }
                else
                {
                    ISM_Resignation_Master_CheckLists_DMO rmr = new ISM_Resignation_Master_CheckLists_DMO();
                    rmr.ISMRESGMCL_CheckListName = dto.ISMRESGMCL_CheckListName;
                    rmr.HRMD_Id = dto.HRMD_Id;
                    // rmr.HRMDES_Id = dto.HRMDES_Id;
                    rmr.ISMRESGMCL_Template = dto.ISMRESGMCL_Template;
                    rmr.ISMRESGMCL_ActiveFlag = true;
                    rmr.MI_Id = dto.MI_Id;
                    rmr.ISMRESGMCL_Remarks = dto.ISMRESGMCL_Remarks;
                    rmr.CreatedDate = DateTime.Now;
                    rmr.ISMRESGMCL_CreatedBy = dto.userId;
                    _vmsconte.Add(rmr);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMCL_ActiveFlag == true)
                {
                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Single(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id);
                    result.ISMRESGMCL_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMCL_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "false";
                }
                else
                {
                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Single(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id);
                    result.ISMRESGMCL_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMCL_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "true";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                dto.get_details_check_list = (from a in _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con
                                              from b in _vmsconte.HR_Master_Department
                                                  //from c in _vmsconte.HR_Master_Designation_con
                                              where a.HRMD_Id == b.HRMD_Id && a.MI_Id == dto.MI_Id && a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id
                                              select new ISM_Resignation_Master_CheckLists_DTO
                                              {
                                                  ISMRESGMCL_Id = a.ISMRESGMCL_Id,
                                                  ISMRESGMCL_CheckListName = a.ISMRESGMCL_CheckListName,
                                                  HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                  HRMD_Id = a.HRMD_Id,
                                                  //HRMDES_DesignationName = c.HRMDES_DesignationName,
                                                  // HRMDES_Id = a.HRMDES_Id,
                                                  ISMRESGMCL_Remarks = a.ISMRESGMCL_Remarks,
                                                  ISMRESGMCL_Template = a.ISMRESGMCL_Template
                                              }).ToList().ToArray();
                dto.department_lisd_dd = _vmsconte.HR_Master_Department.Where(a => a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true).ToList().ToArray();
                // dto.designation_list_dd = _vmsconte.HR_Master_Designation_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMDES_ActiveFlag == true).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        //==================================Exit Employee Process Start=================================


        public ISM_Resignation_DTO Load_all_data(ISM_Resignation_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                dto.DepartmentCodeName = (from a in _vmsconte.HR_Master_DepartmentCode_HeadDMO
                                          from b in _vmsconte.HR_Master_DepartmentCodeDMO
                                          where (a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == userhrme_id)
                                          select b).Select(t => t.HRMDC_Code).FirstOrDefault();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_employee_list_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.userId)
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employee_list_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                dto.reason_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMRESGMRE_ActiveFlag == true).ToList().ToArray();

                dto.exit_employee_list = (from c in _vmsconte.ISM_Resignation_DMO_con
                                          from b in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                          from a in _vmsconte.Hr_Master_Employee_con
                                          from d in _vmsconte.HR_Master_DepartmentCode_HeadDMO
                                          from e in _vmsconte.HR_Master_Department
                                          where (a.MI_Id == dto.MI_Id && b.MI_Id == dto.MI_Id && c.ISMRESGMRE_Id == b.ISMRESGMRE_Id && a.HRME_Id == c.HRME_Id && d.HRME_ID == userhrme_id && d.HRMDC_ID == e.HRMDC_ID && e.HRMD_Id == a.HRMD_Id)
                                          select new ISM_Resignation_DTO
                                          {
                                              employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                              + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                              + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = a.HRME_Id,
                                              ISMRESG_Id = c.ISMRESG_Id,
                                              ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                                              ISMRESG_TentativeLeavingDate = c.ISMRESG_TentativeLeavingDate,
                                              ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag,
                                              ISMRESGMRE_ResignationReasons = b.ISMRESGMRE_ResignationReasons,
                                              ISMRESG_AccRejDate = c.ISMRESG_AccRejDate,
                                              ISMRESG_MgmtApprRejFlg = c.ISMRESG_MgmtApprRejFlg,
                                              ISMRESG_Print_Flg = c.ISMRESG_Print_Flg
                                          }).Distinct().ToList().ToArray();

                dto.relieving_emp_dd = (from c in _vmsconte.ISM_Resignation_DMO_con
                                        from a in _vmsconte.Hr_Master_Employee_con
                                        where (a.MI_Id == dto.MI_Id && a.HRME_Id == c.HRME_Id && c.ISMRESG_MgmtApprRejFlg == "ACCEPT" && c.ISMRESG_Print_Flg == 1 && c.ISMRESG_Status_Flg == 0)
                select new ISM_Resignation_DTO
                                        {
                                            employeename1 = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                              + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                              + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = a.HRME_Id,
                                        }).Distinct().ToList().ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = dto.userId
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_emp_dd1 = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "relieving_check_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_check_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                dto.exit_print_list = (from c in _vmsconte.ISM_Resignation_DMO_con
                                       from a in _vmsconte.Hr_Master_Employee_con
                                       from b in _vmsconte.ISM_Resignation_RelievingLetter_DMO_con
                                       where c.HRME_Id == a.HRME_Id && c.ISMRESG_Id == b.ISMRESG_Id && c.MI_Id == dto.MI_Id && a.MI_Id == dto.MI_Id
                                       select new ISM_Resignation_DTO
                                       {
                                           employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                           + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                           + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                           HRME_Id = a.HRME_Id,
                                           ISMRESG_Id = c.ISMRESG_Id,
                                           ISMRESGRL_RLDate = b.ISMRESGRL_RLDate,
                                           ISMRESGRL_ActiveFlag = b.ISMRESGRL_ActiveFlag,
                                           ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                                           ISMRESG_Remarks = c.ISMRESG_Remarks,
                                           ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag
                                       }).Distinct().ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }


        public ISM_Resignation_DTO GetAllRelData(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.getrelievingchecklist = (from c in _vmsconte.ISM_Resignation_DMO_con
                                             from a in _vmsconte.Hr_Master_Employee_con
                                             from b in _vmsconte.HR_Master_Department
                                             from d in _vmsconte.HR_Master_Designation_con
                                             from e in _vmsconte.Institution
                                             where a.MI_Id == dto.MI_Id && a.HRME_Id == c.HRME_Id && c.HRME_Id == dto.HRME_Id && a.HRME_Id == dto.HRME_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id==e.MI_Id
                                             select new ISM_Resignation_DTO
                                             {
                                                 HRME_Id = a.HRME_Id,
                                                 HRME_DOJ = a.HRME_DOJ,
                                                 HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                 HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                 ISMRESG_TentativeLeavingDate = Convert.ToDateTime(c.ISMRESG_TentativeLeavingDate),
                                                 ISMRESG_Id = c.ISMRESG_Id,
                                                 MI_Name = e.MI_Name

                                             }).Distinct().ToList().ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = dto.userId
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.doc_list = retObject.ToArray();
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

        public ISM_Resignation_DTO GetAllRelData1(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.getrelievingchecklist1 = (from c in _vmsconte.ISM_Resignation_DMO_con
                                              from a in _vmsconte.Hr_Master_Employee_con
                                              from b in _vmsconte.HR_Master_Department
                                              from d in _vmsconte.HR_Master_Designation_con
                                              from e in _vmsconte.Institution
                                              where a.MI_Id == dto.MI_Id && a.HRME_Id == c.HRME_Id && c.HRME_Id == dto.HRME_Id && a.HRME_Id == dto.HRME_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id==e.MI_Id
                                              select new ISM_Resignation_DTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_DOJ = a.HRME_DOJ,
                                                  HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                  HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                  ISMRESG_TentativeLeavingDate = Convert.ToDateTime(c.ISMRESG_TentativeLeavingDate),
                                                  ISMRESG_Id = c.ISMRESG_Id,
                                                  MI_Name=e.MI_Name
                                              }).Distinct().ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public ISM_Resignation_DTO Exit_empl_SaveEdit(ISM_Resignation_DTO dto)
        {
            try
            {
                if (dto.ISMRESG_Id > 0)
                {
                    var result = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                    result.HRME_Id = dto.HRME_Id;
                    result.ISMRESG_ResignationDate = dto.ISMRESG_ResignationDate;
                    result.ISMRESGMRE_Id = dto.ISMRESGMRE_Id;
                    result.ISMRESG_NoticePeriod = dto.ISMRESG_NoticePeriod;
                    result.ISMRESG_TentativeLeavingDate = dto.ISMRESG_TentativeLeavingDate;
                    result.ISMRESG_Remarks = dto.ISMRESG_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMRESG_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Update";
                }
                else
                {
                    ISM_Resignation_DMO rd = new ISM_Resignation_DMO();
                    var result = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.HRME_Id == dto.HRME_Id && a.ISMRESG_MgmtApprRejFlg == "OPEN").ToList();
                    if (result.Count>0) 
                    {
                        dto.returnval = "Exist";
                    }
                    else
                    {
                        rd.HRME_Id = dto.HRME_Id;
                        rd.ISMRESG_ResignationDate = dto.ISMRESG_ResignationDate;
                        rd.ISMRESGMRE_Id = dto.ISMRESGMRE_Id;
                        rd.ISMRESG_NoticePeriod = dto.ISMRESG_NoticePeriod;
                        rd.ISMRESG_TentativeLeavingDate = dto.ISMRESG_TentativeLeavingDate;
                        rd.ISMRESG_Remarks = dto.ISMRESG_Remarks;
                        rd.MI_Id = dto.MI_Id;
                        rd.ISMRESG_Print_Flg = 1;
                        rd.ISMRESG_Status_Flg = 0;
                        rd.ISMRESG_MgmtApprRejFlg = "OPEN";
                        rd.ISMRESG_ActiveFlag = true;
                        rd.ISMRESG_CreatedBy = dto.userId;
                        rd.CreatedDate = DateTime.Now;
                        _vmsconte.Add(rd);
                        _vmsconte.SaveChanges();
                        dto.returnval = "Add";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO Exit_empl_AccRej(ISM_Resignation_DTO dto)
        {
            try
            {

              var dlt = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).ToList();
                foreach (var item in dlt)
                {
                    var res= _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Single(a => a.ISMRESGCL_Id == item.ISMRESGCL_Id);
                    _vmsconte.Remove(res);
                }
                var result = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                result.ISMRESG_MgmtApprRejFlg = dto.ISMRESG_MgmtApprRejFlg;
                result.ISMRESG_ManagementRemarks = dto.ISMRESG_ManagementRemarks;
                result.ISMRESG_AccRejDate = dto.ISMRESG_AccRejDate;
                _vmsconte.Update(result);
                _vmsconte.SaveChanges();
                dto.returnval = "Update";
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO c_approve_new(ISM_Resignation_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Resignation_RelievingLetter_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).Count();
                if (result> 0)
                {
                    dto.returnval = "Duplicate";
                }
                else
                {
                    dto.ISMRESG_Id1 = dto.ISMRESG_Id;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO Edit_Employee(ISM_Resignation_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Resignation_RelievingLetter_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).Count();                 
                if (result > 0)
                {
                    dto.returnval = "Duplicate";
                }
                else                 
                    {
                        dto.exit_employee_details = (from b in _vmsconte.ISM_Resignation_DMO_con
                                                     from a in _vmsconte.Hr_Master_Employee_con
                                                     from c in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                                     where a.MI_Id == dto.MI_Id && a.HRME_Id == b.HRME_Id && a.MI_Id == dto.MI_Id && b.ISMRESGMRE_Id == c.ISMRESGMRE_Id && c.MI_Id == dto.MI_Id && b.ISMRESG_Id == dto.ISMRESG_Id
                                                     select new ISM_Resignation_DTO
                                                     {
                                                         HRME_Id = a.HRME_Id,
                                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                      + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                         ISMRESG_TentativeLeavingDate = Convert.ToDateTime(b.ISMRESG_TentativeLeavingDate),
                                                         ISMRESG_NoticePeriod = b.ISMRESG_NoticePeriod,
                                                         ISMRESGMRE_Id = b.ISMRESGMRE_Id,
                                                         ISMRESGMRE_ResignationReasons = c.ISMRESGMRE_ResignationReasons,
                                                         ISMRESG_Remarks = b.ISMRESG_Remarks,
                                                         ISMRESG_ResignationDate = Convert.ToDateTime(b.ISMRESG_ResignationDate),
                                                         ISMRESG_Id = b.ISMRESG_Id
                                                     }).Distinct().ToList().ToArray();
                    }
                }
            
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Resignation_DTO Savedata_td(ISM_Resignation_DTO dto)
        {
            try
            {
                //var result = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).ToList();
                //foreach (var it in result)
                //{
                //    var result1 = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Single(a => a.ISMRESGCL_Id == it.ISMRESGCL_Id);
                //    _vmsconte.Remove(result1);
                //}

                //_vmsconte.SaveChanges();

                foreach (var p in dto.doc_list2)
                {
                    //var result = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(a => a.ISMSLEDM_Id == p.ISMSLEDM_Id && a.ISMSMPR_Id == p.ISMSMPR_Id);
                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_resignation_checklist_update_proc @p0,@p1,@p2,@p3,@p4,@p5", dto.ISMRESG_Id, p.ISMRESGMCL_Id, p.ISMRESGCL_FileName, p.document_Path, dto.MI_Id, dto.userId);
                    if (contactExistsP > 0)
                    {
                        dto.returnval = "Add";
                    }
                    else
                    {
                        dto.returnval = "notUpdated";
                    }
                    //SqlConnection con = new SqlConnection("Data Source=demovaps.database.windows.net,1433;Initial Catalog=DevelopmentDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;");
                    // //var con = _vmsconte.Database.GetDbConnection().CreateCommand())

                    //SqlCommand cmd = new SqlCommand("ISM_resignation_checklist_update_proc", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ISMRESG_Id", dto.ISMRESG_Id);
                    //cmd.Parameters.AddWithValue("@ISMRESGMCL_Id", p.ISMRESGMCL_Id);
                    //cmd.Parameters.AddWithValue("@ISMRESGCL_FileName", p.ISMRESGCL_FileName);
                    //cmd.Parameters.AddWithValue("@ISMRESGCL_FilePath", p.document_Path);
                    //cmd.Parameters.AddWithValue("@MI_Id", dto.MI_Id);
                    //cmd.Parameters.AddWithValue("@userId", dto.userId);

                    //con.Open();
                    //cmd.ExecuteNonQuery();
                    //con.Close();

                    //ISM_Resignation_ChecKLists_DMO rc = new ISM_Resignation_ChecKLists_DMO();
                    //rc.ISMRESG_Id = dto.ISMRESG_Id;
                    //rc.ISMRESGMCL_Id = item.ISMRESGMCL_Id;
                    //rc.ISMRESGCL_FileName = item.ISMRESGCL_FileName;
                    //rc.ISMRESGCL_FilePath = item.document_Path;
                    //rc.MI_Id = dto.MI_Id;
                    //rc.ISMRESGCL_ActiveFlag = true;
                    //rc.CreatedDate = DateTime.Now;
                    //rc.ISMRESGCL_CreatedBy = dto.userId;
                    //_vmsconte.Add(rc);

                }
               // _vmsconte.SaveChanges();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Resignation_DTO edit_relieving(ISM_Resignation_DTO dto)
        {
            List<ISM_Resignation_DTO> result = new List<ISM_Resignation_DTO>();
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "edit_relieving_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_check_edit = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.relieving_check_edit_dd = (from a in _vmsconte.ISM_Resignation_DMO_con
                                               from b in _vmsconte.Hr_Master_Employee_con
                                               from c in _vmsconte.HR_Master_Department
                                               from d in _vmsconte.HR_Master_Designation_con
                                               where a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == d.HRMDES_Id && a.HRME_Id == dto.HRME_Id
                                               select new ISM_Resignation_DTO
                                               {
                                                   employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName)
                                                 + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName)
                                                 + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_DOJ = b.HRME_DOJ,
                                                   HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                   ISMRESG_TentativeLeavingDate = Convert.ToDateTime(a.ISMRESG_TentativeLeavingDate),
                                                   ISMRESG_Id = a.ISMRESG_Id
                                               }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<ISM_Resignation_DTO> Savedata_printAsync(ISM_Resignation_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.HRME_Id == dto.HRME_Id);
                var result1 = _vmsconte.ISM_Resignation_RelievingLetter_DMO_con.Where(a => a.ISMRESG_Id == result.ISMRESG_Id).ToList();
                if (result1.Count > 0)
                {
                    dto.return_p = "Duplicate";
                }
                else
                {
                    //SqlConnection con = new SqlConnection("Data Source=demovaps.database.windows.net,1433;Initial Catalog=DevelopmentDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;");

                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Savedata_print_proc";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.VarChar)
                        {
                            Value = dto.userId
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    dto.return_p = "Update";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO download_doc(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.download_photo = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).ToArray();
            }
            catch(Exception ee)

            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO print_exit_employee(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.exit_print_list2 = (from c in _vmsconte.ISM_Resignation_DMO_con
                                        from a in _vmsconte.Hr_Master_Employee_con
                                        from b in _vmsconte.ISM_Resignation_RelievingLetter_DMO_con
                                        where c.HRME_Id == a.HRME_Id && c.ISMRESG_Id == b.ISMRESG_Id && c.MI_Id == dto.MI_Id && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id
                                        select new ISM_Resignation_DTO
                                        {
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                            + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                            + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = a.HRME_Id,
                                            ISMRESG_Id = c.ISMRESG_Id,
                                            ISMRESGRL_RLDate = b.ISMRESGRL_RLDate,
                                            ISMRESGRL_ActiveFlag = b.ISMRESGRL_ActiveFlag,
                                            ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                                            ISMRESG_Remarks = c.ISMRESG_Remarks,
                                            ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag
                                        }).Distinct().ToList().ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_employee_print_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.Exit_employee_print_report = retObject.ToArray();
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

        public ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.department_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                         from b in _vmsconte.Hr_Master_Employee_con
                                         from c in _vmsconte.HR_Master_Department
                                         where a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == dto.MI_Id

                                         select new ISM_Resignation_DTO
                                         {
                                             HRMD_DepartmentName = c.HRMD_DepartmentName,
                                             HRMD_Id = c.HRMD_Id
                                         }).Distinct().OrderBy(a=>a.HRMD_Id).ToList().ToArray();

                dto.designation_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                          from b in _vmsconte.Hr_Master_Employee_con
                                          from c in _vmsconte.HR_Master_Designation_con
                                          where a.HRME_Id == b.HRME_Id && b.HRMDES_Id == c.HRMDES_Id && a.MI_Id == dto.MI_Id

                                          select new ISM_Resignation_DTO
                                          {
                                              HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              HRMDES_Id = c.HRMDES_Id
                                          }).Distinct().OrderBy(a=>a.HRMDES_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.department_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                         from b in _vmsconte.Hr_Master_Employee_con
                                         from c in _vmsconte.HR_Master_Department
                                         where a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == dto.MI_Id

                                         select new ISM_Resignation_DTO
                                         {
                                             HRMD_DepartmentName = c.HRMD_DepartmentName,
                                             HRMD_Id = c.HRMD_Id
                                         }).Distinct().OrderBy(t=>t.HRMDES_Id).ToList().ToArray();

                dto.designation_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                          from b in _vmsconte.Hr_Master_Employee_con
                                          from c in _vmsconte.HR_Master_Designation_con
                                          where a.HRME_Id == b.HRME_Id && b.HRMDES_Id == c.HRMDES_Id && a.MI_Id == dto.MI_Id

                                          select new ISM_Resignation_DTO
                                          {
                                              HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              HRMDES_Id = c.HRMDES_Id
                                          }).Distinct().OrderBy(a=>a.HRMD_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public async Task<ISM_Resignation_DTO> showdetails_R(ISM_Resignation_DTO dto)
        {
            try
            {
                string dept_id = "0";
                string des_id = "0";
                List<long> dept_ids = new List<long>();
                List<long> des_ids = new List<long>();
                dto.imagepath = _vmsconte.Institution.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.MI_Logo).FirstOrDefault();

                foreach (var item in dto.selectedept_list)
                {
                    dept_ids.Add(item.HRMD_Id);

                }
                foreach (var item in dto.selectedesig_list)
                {

                    des_ids.Add(item.HRMDES_Id);
                }

                for (int s = 0; s < dept_ids.Count(); s++)
                {
                    dept_id = dept_id + ',' + dept_ids[s].ToString();
                }

                for (int s = 0; s < des_ids.Count(); s++)
                {
                    des_id = des_id + ',' + des_ids[s].ToString();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "exit_employee_report_pro";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dept_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                     SqlDbType.VarChar)
                    {
                        Value = des_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                    SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.FROMDATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                   SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.TODATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACCEPT",
                  SqlDbType.BigInt)
                    {
                        Value = dto.ACCEPT
                    });
                    cmd.Parameters.Add(new SqlParameter("@REJECT",
                  SqlDbType.BigInt)
                    {
                        Value = dto.REJECT
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
                        dto.exi_employee_print_list = retObject.ToArray();
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

        public async Task<ISM_Resignation_DTO> showdetails_relieving_R(ISM_Resignation_DTO dto)
        {
            try
            {
                string dept_id = "0";
                string des_id = "0";

                List<long> dept_ids = new List<long>();
                List<long> des_ids = new List<long>();

                foreach (var item in dto.selectedept_list)
                {
                    dept_ids.Add(item.HRMD_Id);

                }
                foreach (var item in dto.selectedesig_list)
                {

                    des_ids.Add(item.HRMDES_Id);
                }

                for (int s = 0; s < dept_ids.Count(); s++)
                {
                    dept_id = dept_id + ',' + dept_ids[s].ToString();
                }

                for (int s = 0; s < des_ids.Count(); s++)
                {
                    des_id = des_id + ',' + des_ids[s].ToString();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "exit_employee_relieving_report_pro";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dept_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                     SqlDbType.VarChar)
                    {
                        Value = des_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                    SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.FROMDATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                   SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.TODATE)
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
                        dto.exi_employee_print_list = retObject.ToArray();
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

        public ISM_Resignation_DTO relieving_exit_employee_view(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.exit_print_list1 = (from c in _vmsconte.ISM_Resignation_DMO_con
                                        from a in _vmsconte.Hr_Master_Employee_con
                                        from b in _vmsconte.ISM_Resignation_ChecKLists_DMO_con
                                        where c.HRME_Id == a.HRME_Id && c.ISMRESG_Id == b.ISMRESG_Id && c.MI_Id == dto.MI_Id && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id
                                        select new ISM_Resignation_DTO
                                        {
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                            + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                            + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = a.HRME_Id,
                                            ISMRESG_Id = c.ISMRESG_Id,
                                            ISMRESGRL_RLDate = c.ISMRESG_TentativeLeavingDate,
                                            ISMRESGRL_ActiveFlag = b.ISMRESGCL_ActiveFlag,
                                            ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                                            ISMRESG_Remarks = c.ISMRESG_Remarks,
                                            ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag
                                        }).Distinct().ToList().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //GAUTAM
        public ISM_Resignation_DTO loadEmployeeData(ISM_Resignation_DTO data)
        {
            try
            {
                long hrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();

                data.companydetails = _vmsconte.Institution.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToArray();

                if(hrme_id > 0)
                {
                    data.employeedata = (from a in _vmsconte.HR_Master_Employee_DMO
                                         from b in _vmsconte.HR_Master_Designation
                                         from d in _vmsconte.Multiple_Email_DMO
                                         from e in _vmsconte.Multiple_Mobile_DMO
                                         where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == hrme_id && d.HRME_Id == a.HRME_Id && e.HRME_Id == a.HRME_Id && d.HRMEM_DeFaultFlag == "default" && e.HRMEMNO_DeFaultFlag == "default")
                                         select new ISM_Resignation_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             employeename = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                             HRME_DOJ = a.HRME_DOJ,
                                             HRME_MobileNo = e.HRMEMNO_MobileNo,
                                             HRME_EmailId = d.HRMEM_EmailId,
                                             HRMDES_DesignationName = b.HRMDES_DesignationName,
                                             HRME_PerStreet = a.HRME_PerStreet,
                                             HRME_PerArea = a.HRME_PerArea,
                                             HRME_PerCity = a.HRME_PerCity,
                                             HRME_PerStateId = a.HRME_PerStateId,
                                             HRME_PerPincode = a.HRME_PerPincode,
                                             IVRMMG_Id = a.IVRMMG_Id
                                         }).Distinct().ToArray();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation_Deviation_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    {
                        Value = DateTime.Today.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 1
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getdeviationreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.reason_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a =>a.ISMRESGMRE_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public ISM_Resignation_DTO sendResignationMail(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();
                dto.ISMRESG_ResignationDate = DateTime.Today;
                dto.ISMRESG_NoticePeriod = 90;
                dto.ISMRESG_TentativeLeavingDate = DateTime.Today.AddDays(90);

                dto = Exit_empl_SaveEdit(dto);
                if(dto.returnval == "Add")
                {
                    string mailid = dto.ManagerMailid;
                    var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                    string Mailmsg = "Dear Sir/Madam, Please find enclosed PDF attachment of Resignation Letter.Thanking You.";

                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        string mailcc = alldetails[0].IVRM_mailcc;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        string Subject = "Resignation Letter";
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mailid);
                        if (alldetails[0].IVRM_mailcc != "")
                        {
                            string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');
                            if (mail_id.Length > 0)
                            {
                                for (int i = 0; i < mail_id.Length; i++)
                                {
                                    message.AddCc(mail_id[i]);
                                }
                                if (dto.TeamLeadMailid != "") { message.AddCc(dto.TeamLeadMailid); }
                            }
                        }

                        StringBuilder sb = new StringBuilder(dto.Template);
                        StringReader sr = new StringReader(sb.ToString());
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                            pdfDoc.Open();

                            htmlparser.Parse(sr);
                            pdfDoc.Close();

                            byte[] bytess = memoryStream.ToArray();
                            memoryStream.Close();

                            var file = Convert.ToBase64String(bytess);
                            string emp;
                            emp = Convert.ToString(sr);
                            string c = "";
                            string v = emp.Replace("System.IO.StringReader", "ResignationLetter.Pdf");
                            message.AddAttachment(v, file);
                            message.HtmlContent = Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();
                        }

                        using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name.Equals("Resignation_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing_1";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                            {
                                Value = mailid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                            {
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                            {
                                Value = "Resignation Letter"
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                            {
                                Value = "Staff"
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                }
                            }
                            catch (Exception ex)
                            {
                                //return ex.Message;
                            }
                        }
                    }

                    dto.retrunMsg = "Success";
                }

            }
            catch (Exception ex)
            {
                dto.retrunMsg = "False";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        //GAUTAM
    }
}



