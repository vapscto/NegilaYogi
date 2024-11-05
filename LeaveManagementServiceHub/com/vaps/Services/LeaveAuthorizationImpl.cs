using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.HRMS;
using System.Dynamic;
using System.Data;
using System.Data.SqlClient;
using DataAccessMsSqlServerProvider;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveAuthorizationImpl : LeaveAuthorizationInterface
    {
        public LMContext _lmContext;
        public DomainModelMsSqlServerContext _Context;
        public LeaveAuthorizationImpl(LMContext ttcategory, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _lmContext = ttcategory;
            _Context = MsSqlServerContext;
        }



        public LeaveCreditDTO getAuthLeave(LeaveCreditDTO data)
        {
            if (data.onchangeoronload == "OnLoad")
            {
                var getuserinstitution = _lmContext.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.get_institution = _lmContext.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
            }

            List<HR_Master_Grade_DMO> grade_name = new List<HR_Master_Grade_DMO>();
            grade_name = _lmContext.HR_Master_Grade_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMG_ActiveFlag == true).ToList();
            data.grade_name = grade_name.Distinct().ToArray();

            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
            data.stf_types = staf_types.Distinct().ToArray();

            List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
            Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.Department_types = Department_types.Distinct().ToArray();

            List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
            Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToList();
            data.Designation_types = Designation_types.Distinct().ToArray();



            data.get_emp = (from a in _Context.ApplicationUser
                            from b in _lmContext.IVRM_User_Login_InstitutionwiseDMO
                            where (a.Id == b.Id && a.RoleTypeFlag == "Staff" && b.MI_Id == data.MI_Id && b.Activeflag == 1)
                            select a).ToArray();








            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Leave_Authorization_Get_Details";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                try
                {
                    var retObject1 = new List<dynamic>();
                    using (var dataReader1 = cmd.ExecuteReader())
                    {
                        while (dataReader1.Read())
                        {

                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                );
                            }
                            retObject1.Add((ExpandoObject)dataRow1);
                        }
                    }

                    data.get_auth = retObject1.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            data.employeelist = (from t in _lmContext.HR_Master_Employee_DMO
                                 where (t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id == data.MI_Id)
                                 select new HR_Master_Employee_DMO
                                 {
                                     HRME_Id = t.HRME_Id,
                                     HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (t.HRME_EmployeeMiddleName == null ? "" : " " + t.HRME_EmployeeMiddleName) + (t.HRME_EmployeeLastName == null ? "" : " " + t.HRME_EmployeeLastName)
                                 }).ToArray();

            return data;
        }
        public LeaveCreditDTO saveauthdata(LeaveCreditDTO data)
        {
            try
            {
                long HRLA_Id = 0;

                var result = _lmContext.HR_Leave_Authorisation_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRMG_Id == data.HRMG_Id && a.HRMGT_Id == data.HRMGT_Id
                              && a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id && a.HRML_Id == data.HRML_Id).ToList();

                if (result.Count > 0)
                {
                    HRLA_Id = result[0].HRLA_Id;

                    var result_update = _lmContext.HR_Leave_Authorisation_DMO.Single(a => a.MI_Id == data.MI_Id && a.HRMG_Id == data.HRMG_Id
                    && a.HRMGT_Id == data.HRMGT_Id && a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id && a.HRML_Id == data.HRML_Id
                    && a.HRLA_Id == HRLA_Id);

                    result_update.HRLA_UpdatedBy = data.UserId;
                    result_update.UpdatedDate = DateTime.Now;
                    _lmContext.Update(result_update);
                }
                else
                {
                    HR_Leave_Authorisation_DMO objpge = new HR_Leave_Authorisation_DMO
                    {
                        HRML_Id = data.HRML_Id,
                        HRMDES_Id = data.HRMDES_Id,
                        HRMD_Id = data.HRMD_Id,
                        HRMGT_Id = data.HRMGT_Id,
                        HRMG_Id = data.HRMG_Id,
                        MI_Id = data.MI_Id,
                        HRLA_EmailTo = "",
                        HRLA_EmailCC = "",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        HRLA_CreatedBy = data.UserId,
                        HRLA_UpdatedBy = data.UserId
                    };

                    _lmContext.Add(objpge);
                    HRLA_Id = objpge.HRLA_Id;
                }


                foreach (var c in data.approvaluser_array)
                {
                    var Approved_HRME_Id = c.Approval_HRME_Id;
                    //  foreach (var d in data.emp_array)
                    //  {
                    // var HRME_Id = d.HRME_Id;
                    if (HRLA_Id > 0)
                    {
                        var check_emp_user_mapping = _lmContext.HR_Leave_Auth_OrderNo_DMO.Where(a => a.HRLA_Id == HRLA_Id).ToList();
                        if (check_emp_user_mapping.Count <= data.approvaluser_array.Length)
                        {
                            if (check_emp_user_mapping.Count > 0)
                            {
                                //var HRLAON_Id = check_emp_user_mapping.FirstOrDefault().HRLAON_Id;
                                //var result_emp_user_mapping = _lmContext.HR_Leave_Auth_OrderNo_DMO.Single(a => a.HRLA_Id == HRLA_Id && a.HRLAON_Id == HRLAON_Id);
                                // && a.IVRMUL_Id == Approved_HRME_Id  

                                for (var h = 0; h < check_emp_user_mapping.Count; h++)
                                {
                                    var updateresult = _lmContext.HR_Leave_Auth_OrderNo_DMO.Single(a => a.HRLA_Id == HRLA_Id && a.HRLAON_Id == check_emp_user_mapping[h].HRLAON_Id);
                                    updateresult.HRME_Id = Approved_HRME_Id;
                                    updateresult.IVRMUL_Id = Approved_HRME_Id;
                                    updateresult.UpdatedDate = DateTime.Now;
                                    updateresult.HRLAON_SanctionLevelNo = c.ApprovalLevelNo;
                                    updateresult.HRLAON_FinalFlg = c.ApprovalFinalFlag;
                                    updateresult.HRLAON_UpdatedBy = data.UserId;
                                    _lmContext.Update(updateresult);
                                }
                            }
                        }
                        else
                        {

                            var query = _lmContext.HR_Leave_Authorisation_DMO.FirstOrDefault(q => q.HRLA_Id == HRLA_Id);
                            HR_Leave_Auth_OrderNo_DMO obj = new HR_Leave_Auth_OrderNo_DMO();
                            var HRLAON_Id = check_emp_user_mapping.FirstOrDefault().HRLAON_Id;
                            var s = _lmContext.HR_Leave_Auth_OrderNo_DMO.Single(w => w.HRLA_Id == query.HRLA_Id && w.HRLAON_Id== HRLAON_Id);
                            if (s.HRLA_Id > 0)
                            {
                                _lmContext.Remove(s);
                            }
                            HR_Leave_Auth_OrderNo_DMO hR_Leave_Auth_OrderNo_DMO = new HR_Leave_Auth_OrderNo_DMO
                            {
                                HRME_Id = Approved_HRME_Id,
                                IVRMUL_Id = Approved_HRME_Id,
                                HRLA_Id = HRLA_Id,
                                HRLAON_SanctionLevelNo = c.ApprovalLevelNo,
                                HRLAON_FinalFlg = c.ApprovalFinalFlag,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                HRLAON_CreatedBy = data.UserId,
                                HRLAON_UpdatedBy = data.UserId,
                            };
                            _lmContext.Add(hR_Leave_Auth_OrderNo_DMO);
                        }


                    }
                    else
                    {
                        HR_Leave_Auth_OrderNo_DMO hR_Leave_Auth_OrderNo_DMO = new HR_Leave_Auth_OrderNo_DMO
                        {
                            HRME_Id = Approved_HRME_Id,
                            IVRMUL_Id = Approved_HRME_Id,
                            HRLA_Id = HRLA_Id,
                            HRLAON_SanctionLevelNo = c.ApprovalLevelNo,
                            HRLAON_FinalFlg = c.ApprovalFinalFlag,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            HRLAON_CreatedBy = data.UserId,
                            HRLAON_UpdatedBy = data.UserId,
                        };
                        _lmContext.Add(hR_Leave_Auth_OrderNo_DMO);

                    }
                    //  }
                }

                var i = _lmContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Leave_Authorization_Get_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.get_auth = retObject1.ToArray();
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
        public LeaveCreditDTO getauthdata(LeaveCreditDTO data)
        {
            //    List<long> selected_auth = new List<long>();
            //    foreach (var itm in data.emptypes)
            //    {
            //        selected_auth.Add(itm.HRLA_Id);
            //    }

            //    data.get_auth = (from a in _lmContext.HR_Leave_Authorisation_DMO
            //                     from b in _lmContext.HR_Leave_Auth_OrderNo_DMO
            //                     from c in _lmContext.HR_Master_Employee_DMO
            //                     from d in _lmContext.HR_Master_Grade_DMO
            //                     from e in _lmContext.HR_Master_Leave_DMO
            //                     where (a.HRLA_Id == b.HRLA_Id && a.HRMG_Id == d.HRMG_Id && a.HRML_Id == e.HRML_Id && b.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id)
            //                     select new LeaveCreditDTO
            //                     {
            //                         HRMG_GradeName = d.HRMG_GradeName,
            //                         HRML_LeaveType = e.HRML_LeaveType,
            //                         HRME_EmployeeFirstName = c.HRME_EmployeeFirstName,
            //                         HRME_EmailId = c.HRME_EmailId,
            //                         HRLAON_SanctionLevelNo = b.HRLAON_SanctionLevelNo
            //                     }).ToArray();

            return data;
        }
        public LeaveCreditDTO editdetails(int id)
        {
            LeaveCreditDTO page = new LeaveCreditDTO();
            try
            {
                //var GetDetails = _lmContext.HR_Leave_Authorisation_DMO.Where(a => a.HRLA_Id == id).ToList();

                //var HRMD_Id = GetDetails.FirstOrDefault().HRMD_Id;
                //var HRMG_Id = GetDetails.FirstOrDefault().HRMG_Id;
                //var HRMGT_Id = GetDetails.FirstOrDefault().HRMGT_Id;
                //var HRMDES_Id = GetDetails.FirstOrDefault().HRMDES_Id;
                //var MI_Id = GetDetails.FirstOrDefault().MI_Id;

                page.edit_auth = (from a in _lmContext.HR_Leave_Authorisation_DMO
                                  from b in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                  where (a.HRLA_Id == b.HRLA_Id && b.HRLAON_Id == id)
                                  select new LeaveCreditDTO
                                  {
                                      HRLA_Id = a.HRLA_Id,
                                      HRMG_Id = a.HRMG_Id,
                                      HRME_Id = b.HRME_Id,
                                      IVRMUL_Id = b.IVRMUL_Id,
                                      HRLAON_SanctionLevelNo = b.HRLAON_SanctionLevelNo,
                                      HRMGT_Id = a.HRMGT_Id,
                                      HRMD_Id = a.HRMD_Id,
                                      HRMDES_Id = a.HRMDES_Id,
                                      HRML_Id = a.HRML_Id,
                                      HRLAON_FinalFlg = b.HRLAON_FinalFlg
                                  }).ToArray();

                //page.employeelist = (from t in _lmContext.HR_Master_Employee_DMO
                //                     where (t.HRMG_Id == HRMG_Id && t.HRMGT_Id == HRMGT_Id && t.HRMD_Id == HRMD_Id && t.HRMDES_Id == HRMDES_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id == MI_Id)
                //                     select new LeaveCreditDTO
                //                     {
                //                         HRME_Id = t.HRME_Id,
                //                         HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (t.HRME_EmployeeMiddleName == null || t.HRME_EmployeeMiddleName == " " ? " " : t.HRME_EmployeeMiddleName) + (t.HRME_EmployeeLastName == null || t.HRME_EmployeeLastName == " " ? " " : t.HRME_EmployeeLastName)
                //                     }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public LeaveCreditDTO deleteauth(LeaveCreditDTO id)
        {
            LeaveCreditDTO page = new LeaveCreditDTO();
            try
            {
                //var query = _lmContext.HR_Leave_Authorisation_DMO.Single(q => q.HRLA_Id == id.HRLA_Id);
                //HR_Leave_Auth_OrderNo_DMO obj = new HR_Leave_Auth_OrderNo_DMO();
                //var s = _lmContext.HR_Leave_Auth_OrderNo_DMO.Single(w => w.HRLA_Id == query.HRLA_Id);
                //if (s.HRLA_Id > 0)
                //{
                //    _lmContext.Remove(s);
                //}

                List<HR_Leave_Auth_OrderNo_DMO> lorg = new List<HR_Leave_Auth_OrderNo_DMO>();
                lorg = _lmContext.HR_Leave_Auth_OrderNo_DMO.Where(t => t.HRLA_Id.Equals(id.HRLA_Id) && t.HRLAON_Id == id.HRLAON_Id).ToList();
                if (lorg.Any())
                {
                    _lmContext.Remove(lorg.ElementAt(0));
                    var contactExists = _lmContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        page.returnval = true;

                        var checkresult = _lmContext.HR_Leave_Auth_OrderNo_DMO.Where(a => a.HRLA_Id == id.HRLA_Id).Count();
                        if (checkresult == 0)
                        {
                            try
                            {
                                Array details = _lmContext.HR_Leave_Authorisation_DMO.Where(a => a.HRLA_Id == id.HRLA_Id).ToArray();

                                if (details.Length > 0)
                                {
                                    foreach (var d in details)
                                    {
                                        _lmContext.Remove(d);
                                    }

                                    var contactExists1 = _lmContext.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }
                List<HR_Leave_Authorisation_DMO> allpages = new List<HR_Leave_Authorisation_DMO>();
                allpages = _lmContext.HR_Leave_Authorisation_DMO.ToList();
                page.authData = allpages.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public LeaveCreditDTO getemployeelist(LeaveCreditDTO data)
        {
            data.employeelist = (from t in _lmContext.HR_Master_Employee_DMO
                                 where (t.HRMG_Id == data.HRMG_Id && t.HRMGT_Id == data.HRMGT_Id && t.HRMD_Id == data.HRMD_Id && t.HRMDES_Id == data.HRMDES_Id && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id == data.MI_Id)
                                 select new LeaveCreditDTO
                                 {
                                     HRME_Id = t.HRME_Id,
                                     HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (t.HRME_EmployeeMiddleName == null || t.HRME_EmployeeMiddleName == " " ? " " : t.HRME_EmployeeMiddleName) + (t.HRME_EmployeeLastName == null || t.HRME_EmployeeLastName == " " ? " " : t.HRME_EmployeeLastName)
                                 }).ToArray();

            return data;
        }
        public LeaveCreditDTO saveauthdata_backup(LeaveCreditDTO data)
        {
            try
            {
                if (data.HRLA_Id > 0)
                {
                    for (int empcnt = 0; empcnt <= data.emp_array.Length; empcnt++)
                    {
                        var resultCount = (from a in _lmContext.HR_Leave_Authorisation_DMO
                                           from b in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                           from c in _lmContext.HR_Master_Employee_DMO
                                           where (a.HRLA_Id == b.HRLA_Id && b.HRME_Id == c.HRME_Id && a.HRMGT_Id == c.HRMGT_Id && a.HRMD_Id == c.HRMD_Id
                                           && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == c.HRMG_Id
                                           && a.MI_Id == data.MI_Id && a.HRMG_Id == data.HRMG_Id && a.HRMGT_Id == data.HRMGT_Id
                                           && a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id && b.HRME_Id == data.emp_array[empcnt].HRME_Id
                                           && b.IVRMUL_Id == data.IVRMUL_Id)
                                           select b.HRLA_Id).ToList();
                        var resultCount1 = 0;
                        if (resultCount.Count() > 0)
                        {
                            resultCount1 = _lmContext.HR_Leave_Auth_OrderNo_DMO.Where(t => t.IVRMUL_Id == data.IVRMUL_Id && t.HRLA_Id == resultCount[0]).Count();
                        }

                        if (resultCount1 == 0)
                        {
                            var result = _lmContext.HR_Leave_Authorisation_DMO.Single(t => t.HRLA_Id == data.HRLA_Id && t.MI_Id == data.MI_Id);
                            List<Multiple_Email_DMO> get_emp = new List<Multiple_Email_DMO>();
                            get_emp = _lmContext.Multiple_Email_DMO.Where(t => t.HRME_Id == data.IVRMUL_Id && t.HRMEM_DeFaultFlag == "default").ToList();

                            for (int i = 0; i < data.auth_array.Count(); i++)
                            {
                                var temp_class = data.auth_array[i].HRML_Id;
                                HR_Leave_Authorisation_DMO objpge = new HR_Leave_Authorisation_DMO();
                                objpge.HRML_Id = temp_class;
                                objpge.HRMDES_Id = data.HRMDES_Id;
                                objpge.HRMD_Id = data.HRMD_Id;
                                objpge.HRMGT_Id = data.HRMGT_Id;
                                objpge.HRMG_Id = data.HRMG_Id;
                                objpge.MI_Id = data.MI_Id;
                                objpge.HRLA_EmailTo = get_emp.FirstOrDefault().HRMEM_EmailId;
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.HRLA_CreatedBy = data.UserId;
                                objpge.HRLA_UpdatedBy = data.UserId;


                                _lmContext.Add(objpge);

                                HR_Leave_Auth_OrderNo_DMO ob1 = new HR_Leave_Auth_OrderNo_DMO();
                                ob1.HRLAON_SanctionLevelNo = data.HRLAON_SanctionLevelNo;
                                ob1.CreatedDate = DateTime.Now;
                                ob1.HRME_Id = data.emp_array[empcnt].HRME_Id;
                                ob1.IVRMUL_Id = data.IVRMUL_Id;
                                ob1.UpdatedDate = DateTime.Now;
                                ob1.HRLA_Id = objpge.HRLA_Id;
                                ob1.HRLAON_FinalFlg = true;
                                ob1.HRLAON_CreatedBy = data.UserId;
                                ob1.HRLAON_UpdatedBy = data.UserId;
                                _lmContext.Add(ob1);
                            }
                            result.UpdatedDate = DateTime.Now;
                            _lmContext.Update(result);
                            var contactExists = _lmContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            data.returnduplicatestatus = "Duplicate";
                            return data;
                        }
                    }
                }
                else
                {
                    List<Multiple_Email_DMO> get_emp = new List<Multiple_Email_DMO>();
                    var emp = _lmContext.Multiple_Email_DMO.Where(t => t.HRME_Id == data.IVRMUL_Id && t.HRMEM_DeFaultFlag == "default").Select(d => d.HRMEM_EmailId).FirstOrDefault();

                    long HRLA_Id = 0;

                    var result = (from a in _lmContext.HR_Leave_Authorisation_DMO
                                  from b in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                  from c in _lmContext.HR_Master_Employee_DMO
                                  where (a.HRLA_Id == b.HRLA_Id && b.HRME_Id == c.HRME_Id && a.HRMGT_Id == c.HRMGT_Id && a.HRMD_Id == c.HRMD_Id
                                  && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == c.HRMG_Id
                                  && a.MI_Id == data.MI_Id && a.HRMG_Id == data.HRMG_Id && a.HRMGT_Id == data.HRMGT_Id
                                  && a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id && a.HRML_Id == data.HRML_Id
                                  //&& b.IVRMUL_Id == data.IVRMUL_Id 
                                  )
                                  select b.HRLA_Id).ToList();

                    if (result.Count > 0)
                    {
                        HRLA_Id = result[0];
                    }
                    else
                    {
                        HR_Leave_Authorisation_DMO objpge = new HR_Leave_Authorisation_DMO
                        {
                            HRML_Id = data.HRML_Id,
                            HRMDES_Id = data.HRMDES_Id,
                            HRMD_Id = data.HRMD_Id,
                            HRMGT_Id = data.HRMGT_Id,
                            HRMG_Id = data.HRMG_Id,
                            MI_Id = data.MI_Id,
                            HRLA_EmailTo = emp,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            HRLA_CreatedBy = data.UserId,
                            HRLA_UpdatedBy = data.UserId
                        };

                        _lmContext.Add(objpge);
                        HRLA_Id = objpge.HRLA_Id;
                    }

                    for (int empcnt = 0; empcnt < data.emp_array.Length; empcnt++)
                    {
                        var HRME_Id = data.emp_array[empcnt].HRME_Id;

                        var result11 = (from a in _lmContext.HR_Leave_Authorisation_DMO
                                        from b in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                        from c in _lmContext.HR_Master_Employee_DMO
                                        where (a.HRLA_Id == b.HRLA_Id && b.HRME_Id == c.HRME_Id && a.HRMGT_Id == c.HRMGT_Id && a.HRMD_Id == c.HRMD_Id
                                        && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == c.HRMG_Id
                                        && a.MI_Id == data.MI_Id && a.HRMG_Id == data.HRMG_Id && a.HRMGT_Id == data.HRMGT_Id
                                        && a.HRMD_Id == data.HRMD_Id && a.HRMDES_Id == data.HRMDES_Id && b.HRME_Id == HRME_Id
                                        && b.IVRMUL_Id == data.IVRMUL_Id && a.HRML_Id == data.HRML_Id)
                                        select b.HRLA_Id).ToList();

                        if (result.Count > 0)
                        {
                            var checkresult = _lmContext.HR_Leave_Auth_OrderNo_DMO.Single(t => t.IVRMUL_Id == data.IVRMUL_Id && t.HRLA_Id == result[0]);
                            checkresult.UpdatedDate = DateTime.Now;
                            _lmContext.Update(checkresult);
                        }
                        else
                        {
                            // var temp_class = data.auth_array[i].HRML_Id;


                            HR_Leave_Auth_OrderNo_DMO ob1 = new HR_Leave_Auth_OrderNo_DMO();
                            ob1.HRLAON_SanctionLevelNo = data.HRLAON_SanctionLevelNo;
                            ob1.CreatedDate = DateTime.Now;
                            ob1.HRME_Id = HRME_Id;
                            ob1.IVRMUL_Id = data.IVRMUL_Id;
                            ob1.UpdatedDate = DateTime.Now;
                            ob1.HRLAON_CreatedBy = data.UserId;
                            ob1.HRLAON_UpdatedBy = data.UserId;
                            //ob1.HRLA_Id = objpge.HRLA_Id;
                            ob1.HRLAON_FinalFlg = true;
                            _lmContext.Add(ob1);
                        }

                    }

                    var contactExists = _lmContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                List<HR_Leave_Authorisation_DMO> m_events = new List<HR_Leave_Authorisation_DMO>();
                m_events = _lmContext.HR_Leave_Authorisation_DMO.Where(e => e.MI_Id == data.MI_Id).ToList();
                data.master_loplist = m_events.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}