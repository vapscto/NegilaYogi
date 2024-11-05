using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class External_Training_ApprovalImpl : Interfaces.External_Training_ApprovalInterface
    {
        public VMSContext _context;
        public External_Training_ApprovalImpl(VMSContext _con)
        {
            _context = _con;
        }

        public External_Training_ApprovalDTO onloaddata(External_Training_ApprovalDTO data)
        {
            try
            {
                data.hrmeid = (from a in _context.ApplicationUserDMO
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_TRAINING_APPLY_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.Userid
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
                        data.getloaddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_External_Training_Approval_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.loadgrid = retObject.ToArray();
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


        public External_Training_ApprovalDTO approvalstatus(External_Training_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.hrmeid = (from a in _context.ApplicationUserDMO
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.Totalapprovaleval = (from a in _context.HR_PROCESSDMO
                                          from b in _context.HR_Process_Auth_OrderNoDMO
                                          where (a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "Training")
                                          select b).ToArray();
                var countoflevels = data.Totalapprovaleval.Length;


                data.approveflag = (from a in _context.HR_PROCESSDMO
                                    from b in _context.HR_Process_Auth_OrderNoDMO
                                    from c in _context.ApplicationUserDMO
                                    where (a.HRPA_Id == b.HRPA_Id &&  c.Id == b.IVRMUL_Id && a.HRPA_TypeFlag == "Training")
                                    select b).Select(R => R.HRPAON_FinalFlg).FirstOrDefault();


                var levelofapproval = (from a in _context.HR_PROCESSDMO
                                       from b in _context.HR_Process_Auth_OrderNoDMO
                                       from c in _context.ApplicationUserDMO
                                       where (a.HRPA_Id == b.HRPA_Id && c.Id == b.IVRMUL_Id && a.HRPA_TypeFlag == "Training")
                                       select b).Select(R => R.HRPAON_SanctionLevelNo).FirstOrDefault();




                if (data.multiapproval != null)
                {
                    for (int i = 0; i < data.multiapproval.Length; i++)
                    {
                        External_Training_ApprovalDMO obj = new External_Training_ApprovalDMO();
                        obj.HREXTTRN_Id = data.multiapproval[i].HREXTTRN_Id;
                        obj.HRME_Id = data.hrmeid;                        
                        obj.HREXTTRNAPP_ApprovedHrs = data.multiapproval[i].HREXTTRNAPP_ApprovedHrs;
                        obj.HREXTTRNAPP_ApproverRemarks = data.multiapproval[i].HREXTTRNAPP_ApproverRemarks;
                        obj.HREXTTRNAPP_ActiveFlag = true;
                        obj.HREXTTRNAPP_CreatedDate = DateTime.Now;
                        obj.HREXTTRNAPP_UpdatedDate = DateTime.Now;
                        obj.HREXTTRNAPP_CreatedBy = data.Userid;
                        obj.HREXTTRNAPP_UpdatedBy = data.Userid;
                        if (data.HREXTTRN_ApprovedFlg1 == "R")
                        {
                            obj.HREXTTRNAPP_ApprovalFlg = "Rejected";
                        }
                        else
                        {
                            obj.HREXTTRNAPP_ApprovalFlg = "Approved";
                        }
                        _context.Add(obj);

                        if (data.approveflag == true)
                        {
                            var list = _context.External_TrainingDMO.Where(r => r.HREXTTRN_Id == data.multiapproval[i].HREXTTRN_Id).SingleOrDefault();
                            if (data.HREXTTRN_ApprovedFlg1 == "R")
                            {
                                list.HREXTTRN_ApprovedFlg = "Rejected";
                            }
                            else
                            {
                                list.HREXTTRN_ApprovedFlg = "Approved";
                            }
                            _context.Update(list);
                        }
                    }
                    var a = _context.SaveChanges();
                    if (a > 0)
                    {


                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = "admin";

                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public External_Training_ApprovalDTO trainingdetails(External_Training_ApprovalDTO data)
        {
            try
            {
                data.trainingdetails = (from a in _context.External_TrainingDMO
                                        from b in _context.Master_External_TrainingTypeDMO
                                        from c in _context.Master_External_TrainingCentersDMO
                                        from g in _context.Hr_Master_Employee_con
                                        where (a.HREXTTRN_Id == data.HREXTTRN_Id && a.HRMETRTY_Id == b.HRMETRTY_Id && a.HRME_Id == g.HRME_Id && a.HREXTTRN_ActiveFlag == true && a.HRMETRCEN_Id == c.HRMETRCEN_Id)
                                        select new External_Training_ApprovalDTO
                                        {
                                            HREXTTRN_TrainingTopic = a.HREXTTRN_TrainingTopic,
                                            HREXTTRN_Id = a.HREXTTRN_Id,
                                            HREXTTRN_ApprovedFlg = a.HREXTTRN_ApprovedFlg,
                                            HRMETRTY_ExternalTrainingType = b.HRMETRTY_ExternalTrainingType,
                                            HRMETRCEN_TrainingCenterName = c.HRMETRCEN_TrainingCenterName,
                                            HREXTTRN_StartDate = a.HREXTTRN_StartDate,
                                            HREXTTRN_EndDate = a.HREXTTRN_EndDate,
                                            HREXTTRN_StartTime = a.HREXTTRN_StartTime,
                                            HREXTTRN_EndTime = a.HREXTTRN_EndTime,
                                            HREXTTRN_CertificateFilePath=a.HREXTTRN_CertificateFilePath,
                                            HREXTTRN_TotalHrs = a.HREXTTRN_TotalHrs,
                                            EmplYoeeName = g.HRME_EmployeeFirstName + (string.IsNullOrEmpty(g.HRME_EmployeeMiddleName) ? "" : ' ' + g.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(g.HRME_EmployeeLastName) ? "" : ' ' + g.HRME_EmployeeLastName),
                                            HRME_Id = g.HRME_Id
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                data.returnval = "admin";

                Console.WriteLine(ex.Message);
            }
            return data;
        }




        public External_Training_ApprovalDTO deactiveY(External_Training_ApprovalDTO data)
        {
            //try
            //{
            //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            //    var checkactivestatus = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRNAPP_Id == data.HREXTTRNAPP_Id);

            //    if (checkactivestatus.HRMETRCEN_ActiveFlag == true)
            //    {
            //        var resultdeactive = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRNAPP_Id == data.HREXTTRNAPP_Id);

            //        if (resultdeactive.HRMETRCEN_ActiveFlag == true)
            //        {
            //            resultdeactive.HRMETRCEN_ActiveFlag = false;
            //        }
            //        else
            //        {
            //            resultdeactive.HRMETRCEN_ActiveFlag = true;
            //        }

            //        resultdeactive.HRMETRCEN_UpdatedDate = indiantime0;
            //        resultdeactive.HRMETRCEN_UpdatedBy = data.Userid;
            //        _context.Update(resultdeactive);

            //        var i = _context.SaveChanges();
            //        if (i > 0)
            //        {
            //            data.returnval = true;
            //        }
            //        else
            //        {
            //            data.returnval = false;
            //        }

            //    }
            //    else
            //    {
            //        var resultdeactive = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRCEN_Id == data.HRMETRCEN_Id);

            //        if (resultdeactive.HRMETRCEN_ActiveFlag == true)
            //        {
            //            resultdeactive.HRMETRCEN_ActiveFlag = false;
            //        }
            //        else
            //        {
            //            resultdeactive.HRMETRCEN_ActiveFlag = true;
            //        }

            //        resultdeactive.HRMETRCEN_UpdatedDate = indiantime0;
            //        resultdeactive.HRMETRCEN_UpdatedBy = data.Userid;
            //        _context.Update(resultdeactive);

            //        var i = _context.SaveChanges();
            //        if (i > 0)
            //        {
            //            data.returnval = true;
            //        }
            //        else
            //        {
            //            data.returnval = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }




    }
}
