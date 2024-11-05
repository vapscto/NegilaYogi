using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.LeaveManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{

    public class AdminondutyapplyIMPL : Interfaces.AdminondutyapplyInterface
    {

        public LMContext _lmContext;
        public DomainModelMsSqlServerContext _db;

        public AdminondutyapplyIMPL(LMContext lmContext,DomainModelMsSqlServerContext _domain)
        
        {
            _lmContext = lmContext;
            _db = _domain;
        }
        public AdminondutyapplyDTO getdata(AdminondutyapplyDTO data)
        {
            try
            {

                data.employeeList = _lmContext.HR_Master_Employee_DMO.Where(R => R.MI_Id == data.MI_Id && R.HRME_ActiveFlag == true && R.HRME_LeftFlag == false).ToArray();

                //var qyery1 = (from q in _db.Staff_User_Login
                //              from r in _db.HR_Master_Employee_DMO
                //              where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                //              select new LeaveCreditDTO
                //              {
                //                  HRME_Id = r.HRME_Id,
                //              }).Distinct().ToArray();


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LEAVE_NAME_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        data.leave_name = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public AdminondutyapplyDTO employeedetails(AdminondutyapplyDTO data)
        {

            try
            {

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LEAVE_NAME_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.leave_name = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_MASTER_EMP_DETAILS"; 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.EmployeeDeatils = retObject.ToArray();
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
            return data;
        }

        public AdminondutyapplyDTO viewcomment(AdminondutyapplyDTO data)
        {
            try
            {
                //data.commentlist = (from a in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                //                    from b in _lmContext.HR_Master_Employee_DMO
                //                    from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                //                    where (a.HRME_Id == b.HRME_Id && a.HRELAP_Id == data.HRELAP_Id && a.HRELAP_Id == c.HRELAP_Id)
                //                    select new LeaveCreditDTO
                //                    {
                //                        HRME_Id = a.HRME_Id,
                //                        HRELAPA_Remarks = a.HRELAPA_Remarks,
                //                        HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                //                        HRELAPA_TotalDays = a.HRELAPA_TotalDays,
                //                        HRELAPA_FromDate = a.HRELAPA_FromDate,
                //                        HRELAPA_ToDate = a.HRELAPA_ToDate,
                //                        HRELAPA_LeaveStatus = a.HRELAPA_LeaveStatus,
                //                        HRELAPA_SanctioningLevel = a.HRELAPA_SanctioningLevel,
                //                        HRELAP_FromDate = c.HRELAP_FromDate,
                //                        HRELAP_ToDate = c.HRELAP_ToDate,
                //                        CreatedDate = a.CreatedDate,
                //                        HRELAP_SupportingDocument = c.HRELAP_SupportingDocument
                //                    }).OrderBy(a => a.HRELAPA_SanctioningLevel).ToArray();


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Levelwise_Leave_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRELAP_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRELAP_Id
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
                        data.commentlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public AdminondutyapplyDTO requestleave(AdminondutyapplyDTO data)
        {
            try
            {
                data.returnmsg = "";
                data.returnval = false;
                HR_Emp_Leave_ApplicationDMO objpge = new HR_Emp_Leave_ApplicationDMO();
                if (data.HRML_LeaveType == "COMPOFF")
                {
                    data.HRML_Id = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveType == "Comp_Off" && t.MI_Id == data.MI_Id).Select(t => t.HRML_Id).FirstOrDefault();
                }
                else if (data.HRML_LeaveType == "OD")
                {
                    data.HRML_Id = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveCode == "OD" && t.MI_Id == data.MI_Id).Select(t => t.HRML_Id).FirstOrDefault();
                }

                if (data.HRML_Id > 0)
                {
                    //var qyery1 = (from q in _db.Staff_User_Login
                    //              from r in _db.HR_Master_Employee_DMO
                    //              where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                    //              select new LeaveCreditDTO
                    //              {
                    //                  HRME_Id = r.HRME_Id,
                    //              }).Distinct().ToArray();

                    var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                          from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                          where (m.HRELAP_Id == n.HRELAP_Id && n.HRML_Id == data.HRML_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_Id && m.HRELAP_FromDate == data.HRELAP_FromDate && m.HRELAP_ToDate == data.HRELAP_ToDate && m.HRELAP_LeaveReason==data.HRELAP_LeaveReason && m.HRELAP_ApplicationStatus != "Rejected")
                                          select new LeaveCreditDTO
                                          {
                                              HRELAP_Id = m.HRELAP_Id
                                          }).ToList();

                    if (checkduplicate.Count == 0)
                    {
                        //Leave Id Creation from transaction numbering.
                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                        if (transnumconfigsettings.Count > 0)
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            Master_NumberingDTO num = new Master_NumberingDTO();
                            num.MI_Id = data.MI_Id;
                            num.ASMAY_Id = data.asmay_id;
                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                            num.IMN_Flag = "LeaveNo";
                            data.HRELAP_ApplicationID = a.GenerateNumber(num);
                        }


                        //LEAVE APPLICATION
                        objpge.HRME_Id = data.HRME_Id;
                        objpge.MI_Id = data.MI_Id;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.HRELAP_CreatedBy = data.UserId;
                        objpge.HRELAP_UpdatedBy = data.UserId;
                        objpge.HRELAP_LeaveReason = data.HRELAP_LeaveReason;
                        objpge.HRELAP_ActiveFlag = true;
                        objpge.HRELAP_ApplicationID = data.HRELAP_ApplicationID != null ? data.HRELAP_ApplicationID : "";
                        objpge.HRELAP_ApplicationStatus = "Requested";
                        objpge.HRELAP_SanctioningLevel = "1";
                        objpge.HRELAP_CompOffCreditApplFlg = false;
                        objpge.HRELAP_FromDate = Convert.ToDateTime(data.HRELAP_FromDate);
                        objpge.HRELAP_ToDate = Convert.ToDateTime(data.HRELAP_ToDate);
                        objpge.HRELAP_SupportingDocument = data.HRELT_SupportingDocument;
                        _lmContext.Add(objpge);
                        _lmContext.SaveChanges();

                        //LEAVE APPLICATION DETAILS
                        if (objpge.HRELAP_Id > 0)
                        {
                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                            updatedet.HRELAPD_FromDate = data.HRELAP_FromDate;
                            updatedet.HRELAPD_LeaveStatus = "Applied";
                            updatedet.HRELAPD_ToDate = data.HRELAP_ToDate;
                            updatedet.HRELAPD_TotalDays = data.HRELAP_TotalDays;
                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                            updatedet.HRML_Id = data.HRML_Id;
                            updatedet.UpdatedDate = DateTime.Now;
                            updatedet.CreatedDate = DateTime.Now;
                            updatedet.HRELAPD_UpdatedBy = data.UserId;
                            updatedet.HRELAPD_CreatedBy = data.UserId;
                            updatedet.HRELAPD_ActiveFlag = true;
                            updatedet.HRELAPD_InTime = data.HRELAPD_InTime;
                            updatedet.HRELAPD_OutTime = data.HRELAPD_OutTime;
                            _lmContext.Add(updatedet);
                            _lmContext.SaveChanges();
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnmsg = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdminondutyapplyDTO ActiveDeactiveRecord(AdminondutyapplyDTO dto)
        {
            dto.returnmsg = "";
            try
            {
                var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_Employee_Leave_Delete @p0", dto.HRELAP_Id);
                if (contactExistsP > 0)
                {
                    dto.returnmsg = "updated";
                }
                else
                {
                    dto.returnmsg = "notUpdated";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.returnmsg = "Error occured";
            }

            return dto;
        }

        public AdminondutyapplyDTO editData(AdminondutyapplyDTO id)
        {

            AdminondutyapplyDTO dto = new AdminondutyapplyDTO();
           
            dto.returnmsg = "";
            try
            {
                List<HR_Emp_Leave_Appl_DetailsDMO> lorg = new List<HR_Emp_Leave_Appl_DetailsDMO>();
                lorg = _lmContext.HR_Emp_Leave_Appl_DetailsDMO.AsNoTracking().Where(t => t.HRELAP_Id.Equals(id.HRELAP_Id)).ToList();
                dto.editresult = lorg.ToArray();

                List<HR_Emp_Leave_ApplicationDMO> log = new List<HR_Emp_Leave_ApplicationDMO>();
                log = _lmContext.HR_Emp_Leave_ApplicationDMO.AsNoTracking().Where(t => t.HRELAP_Id.Equals(id.HRELAP_Id)).ToList();
                dto.editresults = log.ToArray();              

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returnmsg = "Error occured";
            }

            return dto;
        }
    }
}
        
