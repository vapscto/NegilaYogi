using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeLeaveApplyImpl : Interfaces.EmployeeLeaveApplyInterface
    {
        public LMContext _lmContext;
        public ExamContext _exm;
        public EmployeeLeaveApplyImpl(LMContext ttcategory, ExamContext exm)
        {
            _lmContext = ttcategory;
            _exm = exm;
        }

        public EmployeeDashboardDTO getonlineLeave(EmployeeDashboardDTO data)
        {
            data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

            data.leave_name = (from x in _lmContext.HR_Master_Leave_DMO
                               from y in _lmContext.HR_Emp_Leave_StatusDMO

                               where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && x.HRML_LeaveCreditFlg == true && y.HRME_Id == data.HRME_Id)
                               select new EmployeeDashboardDTO
                               {

                                   HRELS_Id = y.HRELS_Id,
                                   HRML_LeaveName = x.HRML_LeaveName,
                                   HRELS_TotalLeaves = y.HRELS_TotalLeaves,
                                   HRELS_CreditedLeaves = y.HRELS_CreditedLeaves,
                                   HRELS_TransLeaves = y.HRELS_TransLeaves,
                                   HRELS_CBLeaves = y.HRELS_CBLeaves,
                                   HRML_Id = y.HRML_Id

                               }
                     ).Distinct().ToArray();


            data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                 from b in _lmContext.HR_Master_Designation_DMO
                                     // from c in _lmContext.HR_Emp_Leave_StatusDMO
                                 where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id)
                                 select new EmployeeDashboardDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_DOJ = a.HRME_DOJ,
                                     HRME_MobileNo = a.HRME_MobileNo,
                                     HRME_EmailId = a.HRME_EmailId,
                                     HRMDES_DesignationName = b.HRMDES_DesignationName
                                 }
                     ).Distinct().ToArray();



            data.mobile = (from a in _lmContext.Emp_MobileNo
                           where (a.HRME_Id == data.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                           select new EmployeeDashboardDTO
                           {
                               HRME_MobileNo = a.HRMEMNO_MobileNo,
                           }).Distinct().ToArray();


            data.email = (from a in _lmContext.Emp_Email_Id

                          where (a.HRME_Id == data.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                          select new EmployeeDashboardDTO
                          {
                              HRME_EmailId = a.HRMEM_EmailId,
                          }).Distinct().ToArray();

            return data;
        }
        public EmployeeDashboardDTO saveonlineLeave(EmployeeDashboardDTO _category)
        {
            HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(_category);
            try
            {
                _category.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == _category.UserId && c.MI_Id == _category.MI_Id).Emp_Code;

                if (objpge.HRELAP_Id > 0)
                {


                    var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                    if (resultCount == 0)
                    {
                        var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                        result.MI_Id = objpge.MI_Id;
                        //result.HRME_Id = objpge.HRME_Id;
                        result.HRME_Id = _category.HRME_Id;
                        result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                        result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                        result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                        result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                        result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                        result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                        result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;


                        result.UpdatedDate = DateTime.Now;
                        _lmContext.Update(result);
                        var contactExists = _lmContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {


                    var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == _category.MI_Id && d.HRME_Id == _category.HRME_Id && d.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id).ToList();
                    var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                    var fromdate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                    var todate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");
                    if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0)
                    {
                        var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                              from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                              where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == _category.HRME_Id
                                              && m.HRELAP_FromDate.Value.Date == _category.frmToDates.FirstOrDefault().HRELAP_FromDate.Value.Date
                                              && m.HRELAP_ToDate.Value.Date == _category.frmToDates.FirstOrDefault().HRELAP_ToDate.Value.Date
                                              && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                              select new EmployeeDashboardDTO
                                              {
                                                  HRELAP_Id = m.HRELAP_Id
                                              }
                                              ).ToList();
                        if (checkduplicate.Count == 0)
                        {
                            objpge.HRME_Id = _category.HRME_Id;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.HRELAP_ActiveFlag = true;
                            objpge.HRELAP_ApplicationID = _category.temp_table_data.FirstOrDefault().HRELAP_ApplicationID;
                            objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                            objpge.HRELAP_ApplicationStatus = "Applied";
                            objpge.HRELAP_SanctioningLevel = "1";
                            objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                            objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);


                            _lmContext.Add(objpge);
                            var contactExists = _lmContext.SaveChanges();


                            HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);


                            if (objpge.HRELAP_Id > 0)
                            {
                                var resultempltransDetails = _lmContext.HR_Emp_Leave_Appl_DetailsDMO.Where(t => t.HRELAP_Id == objpge1.HRELAP_Id && t.HRELAPD_FromDate <= objpge1.HRELAPD_FromDate && t.HRELAPD_ToDate == objpge1.HRELAPD_ToDate).ToList();


                                Mapper.Map(objpge1, resultempltransDetails);
                                _lmContext.Update(resultempltransDetails);

                            }


                            else
                            {


                            }
                            if (contactExists > 0)
                            {
                                _category.returnval = true;

                                var statusUpdate = _lmContext.HR_Emp_Leave_StatusDMO.Single(d => d.HRELS_Id == _category.temp_table_data.FirstOrDefault().HRELS_Id);
                                statusUpdate.HRELS_TransLeaves = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                statusUpdate.HRELS_CBLeaves = bal_leave;
                                _lmContext.Update(statusUpdate);
                                _lmContext.SaveChanges();
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                        else
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }

                    }
                    else
                    {
                        _category.message = "You Crossed Your Leave Limits";
                    }

                }
                // }

                List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                _category.master_eventlist = m_events.ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        //======================== TC Class teacher approval

        public Adm_TC_Approval_DTO getdata_CTA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_student_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value ="CT"
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
                        dto.student_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "CT"
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
                        dto.tc_ct_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO SaveEdit_CTA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                if (dto.ATCCTAPP_Id > 0)
                {
                    var result = _exm.Adm_TC_CT_Approval_DMO_con.Single(a => a.ATCCTAPP_Id == dto.ATCCTAPP_Id && a.MI_Id == dto.MI_Id);
                    result.AMST_Id = dto.AMST_Id;
                    result.ATCCTAPP_ApprovedDate = dto.ATCCTAPP_ApprovedDate;
                    result.ATCCTAPP_AttendanceApprovalFlg = dto.ATCCTAPP_AttendanceApprovalFlg;
                    result.ATCCTAPP_ExamApprovalFlg = dto.ATCCTAPP_ExamApprovalFlg;
                    result.ATCCTAPP_ActiveFlg = true;
                    result.ATCCTAPP_Remarks = dto.ATCCTAPP_Remarks;
                    result.UpdatedDate = DateTime.Now;
                    result.ATCCTAPP_UpdatedBy = dto.UserId;
                    result.ATCCTAPP_ApprovedBy = dto.UserId;
                    _exm.Update(result);
                   var vv= _exm.SaveChanges();
                    if(vv>0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    Adm_TC_CT_Approval_DMO DT = new Adm_TC_CT_Approval_DMO();

                    DT.AMST_Id = dto.AMST_Id;
                    DT.MI_Id = dto.MI_Id;
                    DT.ATCCTAPP_ApprovedDate = dto.ATCCTAPP_ApprovedDate;
                    DT.ATCCTAPP_AttendanceApprovalFlg = dto.ATCCTAPP_AttendanceApprovalFlg;
                    DT.ATCCTAPP_ExamApprovalFlg = dto.ATCCTAPP_ExamApprovalFlg;
                    DT.ATCCTAPP_Remarks = dto.ATCCTAPP_Remarks;
                    DT.CreatedDate = DateTime.Now;
                    DT.ATCCTAPP_CreatedBy = dto.UserId;
                    DT.ATCCTAPP_ApprovedBy = dto.UserId;
                    _exm.Add(DT);
                   var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO details_CTA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                     cmd.Parameters.Add(new SqlParameter("@APP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ATCCTAPP_Id
                     });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "CT"
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
                        dto.tc_ct_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO deactivate_CTA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                var result = _exm.Adm_TC_CT_Approval_DMO_con.Single(a => a.ATCCTAPP_Id == dto.ATCCTAPP_Id && a.MI_Id == dto.MI_Id);
                
                if (dto.ATCCTAPP_ActiveFlg == true)
                {
                    result.ATCCTAPP_ActiveFlg = false;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ATCCTAPP_ActiveFlg = true;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        public Adm_TC_Approval_DTO getstudetails_CTA(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "CT"
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
                        dto.libstudetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "CTEXM"
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
                        dto.exmdetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }
        //======================== TC Library approval

        public Adm_TC_Approval_DTO getdata_LIB(Adm_TC_Approval_DTO dto)
        {
            try
            {
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_student_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "LIB"
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
                        dto.student_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "LIB"
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
                        dto.tc_library_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO SaveEdit_LIB(Adm_TC_Approval_DTO dto)
        {
            try
            {
                if (dto.ATCLIBAPP_Id > 0)
                {
                    var result = _exm.Adm_TC_Library_Approval_DMO_con.Single(a => a.ATCLIBAPP_Id == dto.ATCLIBAPP_Id && a.MI_Id == dto.MI_Id);
                    result.AMST_Id = dto.AMST_Id;
                    result.ATCLIBAPP_ApprovedDate = dto.ATCLIBAPP_ApprovedDate;
                    result.ATCLIBAPP_ApprovalFlg = dto.ATCLIBAPP_ApprovalFlg;
                    result.ATCLIBAPP_ActiveFlg = true;
                     result.ATCLIBAPP_Remarks = dto.ATCLIBAPP_Remarks;
                    result.ATCLIBAPP_ApprovedBy = dto.UserId;
                    result.UpdatedDate = DateTime.Now;
                    result.ATCLIBAPP_UpdatedBy = dto.UserId;
                   
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    Adm_TC_Library_Approval_DMO DT = new Adm_TC_Library_Approval_DMO();

                    DT.AMST_Id = dto.AMST_Id;
                    DT.MI_Id = dto.MI_Id;
                    DT.ATCLIBAPP_ApprovedDate = dto.ATCLIBAPP_ApprovedDate;
                    DT.ATCLIBAPP_ApprovalFlg = dto.ATCLIBAPP_ApprovalFlg;
                    DT.ATCLIBAPP_Remarks = dto.ATCLIBAPP_Remarks;
                    DT.ATCLIBAPP_ApprovedBy = dto.UserId;
                    DT.ATCLIBAPP_ActiveFlg = true;
                    DT.CreatedDate = DateTime.Now;
                    DT.ATCLIBAPP_CreatedBy = dto.UserId;
                    _exm.Add(DT);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO details_LIB(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ATCLIBAPP_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "LIB"
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
                        dto.tc_library_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO deactivate_LIB(Adm_TC_Approval_DTO dto)
        {
            try
            {
                var result = _exm.Adm_TC_Library_Approval_DMO_con.Single(a => a.ATCLIBAPP_Id == dto.ATCLIBAPP_Id && a.MI_Id == dto.MI_Id);

                if (dto.ATCLIBAPP_ActiveFlg == true)
                {
                    result.ATCLIBAPP_ActiveFlg = false;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ATCLIBAPP_ActiveFlg = true;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        public Adm_TC_Approval_DTO getstudetails_LIB(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "LIB"
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
                        dto.libstudetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        //======================== TC FEE approval

        public Adm_TC_Approval_DTO getdata_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_student_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FEE"
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
                        dto.student_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_list_proc"; 
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FEE"
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
                        dto.tc_fee_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO SaveEdit_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {
                if (dto.ATCFAPP_Id > 0)
                {
                    var result = _exm.Adm_TC_Fee_Approval_DMO_con.Single(a => a.ATCFAPP_Id == dto.ATCFAPP_Id && a.MI_Id == dto.MI_Id);
                    result.AMST_Id = dto.AMST_Id;
                    result.ATCFAPP_ApprovedDate = dto.ATCFAPP_ApprovedDate;
                    result.ATCFAPP_ActiveFlg = true;
                    result.ATCFAPP_ApprovalFlg = false;
                     result.ATCFAPP_Remarks = dto.ATCFAPP_Remarks;
                    result.ATCFAPP_ApprovedBy = dto.UserId;
                    result.UpdatedDate = DateTime.Now;
                    result.ATCFAPP_UpdatedBy = dto.UserId;
                   
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
               
                    foreach (var item in dto.feeblcarray)
                    {

                        Adm_TC_Fee_Approval_DMO DT = new Adm_TC_Fee_Approval_DMO();
                        DT.AMST_Id = dto.AMST_Id;
                        DT.MI_Id = dto.MI_Id;
                        DT.ATCFAPP_FeeGroupId = item.FMG_Id;
                        DT.ATCFAPP_ApprovedDate = dto.ATCFAPP_ApprovedDate;
                        DT.ATCFAPP_ActiveFlg = true;
                        DT.ATCFAPP_ApprovalFlg = false;
                        DT.ATCFAPP_Remarks = dto.ATCFAPP_Remarks;
                        DT.ATCFAPP_ApprovedBy = dto.UserId;
                        DT.CreatedDate = DateTime.Now;
                        DT.ATCFAPP_CreatedBy = dto.UserId;
                        _exm.Add(DT);
                    }
                   var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fee_Approval_Insert";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(dto.MI_Id)
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                            {
                                Value = dto.AMST_Id
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
                              var aa=retObject.ToArray();
                                if (aa.Length>0)
                                {
                                    dto.returndata = "Add";
                                }
                                   
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                           
                        }
                       
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO details_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ATCLIBAPP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FEE"
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
                        dto.tc_fee_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO deactivate_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {
                var result = _exm.Adm_TC_Fee_Approval_DMO_con.Single(a => a.ATCFAPP_Id == dto.ATCFAPP_Id && a.MI_Id == dto.MI_Id);

                if (dto.ATCLIBAPP_ActiveFlg == true)
                {
                    result.ATCFAPP_ActiveFlg = false;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ATCFAPP_ActiveFlg = true;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        public Adm_TC_Approval_DTO getstudetails_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FEE"
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
                        dto.libstudetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        public Adm_TC_Approval_DTO feeheaddetails_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FEEHEAD"
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
                        dto.feehead_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public Adm_TC_Approval_DTO feenot_approval_FEE(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FEE"
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
                        dto.libstudetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //======================== FDA Class teacher approval

        public Adm_TC_Approval_DTO getdata_PDA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_student_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "FDA"
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
                        dto.student_dd = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "PDA"
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
                        dto.tc_fda_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO SaveEdit_PDA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                if (dto.ATCPDAAPP_Id > 0)
                {
                    var result = _exm.Adm_TC_PDA_Approval_DMO_con.Single(a => a.ATCPDAAPP_Id == dto.ATCCTAPP_Id && a.MI_Id == dto.MI_Id);
                    result.AMST_Id = dto.AMST_Id;
                    result.ATCPDAAPP_ApprovedDate = dto.ATCPDAAPP_ApprovedDate;
                    result.ATCPDAAPP_ApprovalFlg = dto.ATCPDAAPP_ApprovalFlg;
                     result.ATCPDAAPP_ActiveFlg = true;
                    result.ATCPDAAPP_Remarks = dto.ATCPDAAPP_Remarks;
                    result.ATCPDAAPP_UpdatedDate = DateTime.Now;
                    result.ATCPDAAPP_UpdatedBy = dto.UserId;
                    result.ATCPDAAPP_ApprovedBy = dto.UserId;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Update";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    Adm_TC_PDA_Approval_DMO pda = new Adm_TC_PDA_Approval_DMO();

                    pda.AMST_Id = dto.AMST_Id;
                    pda.MI_Id = dto.MI_Id;
                    pda.ATCPDAAPP_ApprovedDate = dto.ATCPDAAPP_ApprovedDate;
                    pda.ATCPDAAPP_ApprovalFlg = dto.ATCPDAAPP_ApprovalFlg;
                    pda.ATCPDAAPP_ActiveFlg = true;
                    pda.ATCPDAAPP_Remarks = dto.ATCPDAAPP_Remarks;
                    pda.ATCPDAAPP_CreatedDate = DateTime.Now;
                    pda.ATCPDAAPP_CreatedBy = dto.UserId;
                    pda.ATCPDAAPP_ApprovedBy = dto.UserId;
                    _exm.Add(pda);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO details_PDA(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@APP_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ATCCTAPP_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "CT"
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
                        dto.tc_ct_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Adm_TC_Approval_DTO deactivate_PDA(Adm_TC_Approval_DTO dto)
        {
            try
            {
                var result = _exm.Adm_TC_CT_Approval_DMO_con.Single(a => a.ATCCTAPP_Id == dto.ATCCTAPP_Id && a.MI_Id == dto.MI_Id);

                if (dto.ATCCTAPP_ActiveFlg == true)
                {
                    result.ATCCTAPP_ActiveFlg = false;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();

                    if (vv > 0)
                    {
                        dto.returndata = "false";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }
                }
                else
                {
                    result.ATCCTAPP_ActiveFlg = true;
                    _exm.Update(result);
                    var vv = _exm.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "true";
                    }
                    else
                    {
                        dto.returndata = "Error";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        public Adm_TC_Approval_DTO getstudetails_PDA(Adm_TC_Approval_DTO dto)
        {
            try
            {

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "PDA"
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
                        dto.libstudetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _exm.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_approval_stu_blc_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@APPFLG", SqlDbType.VarChar)
                    {
                        Value = "CTEXM"
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
                        dto.exmdetails = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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
