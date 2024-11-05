using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using AutoMapper;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonLibrary;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Collections;
using iTextSharp.text;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Transport;

namespace PortalHub.com.vaps.Employee.Services
{
    public class NoticeBoardImpl : Interfaces.NoticeBoardInterface
    {
        public PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;
        //public DomainModelMsSqlServerContext _db;
        public NoticeBoardImpl(PortalContext pContext, DomainModelMsSqlServerContext db)
        {
            _PortalContext = pContext;
            _db = db;
        }

        public IVRM_NoticeBoardDTO Getdetails(IVRM_NoticeBoardDTO data)
        {
            try
            {
                data.yearlist = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.academicyear = _db.AcademicYear.Where(a => a.ASMAY_Id == data.ASMAY_Id).ToArray();

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_NoticeBoard_List";
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.notice_details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //var notice = _PortalContext.IVRM_NoticeBoardDMO.Where(n => n.MI_Id == data.MI_Id).OrderByDescending(n => n.INTB_Id).Distinct().ToArray();
                //if (notice.Length > 0)
                //{
                //    data.notice_details = notice;
                //}
                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                else
                {
                    data.HRME_Id = 0;
                }
                //IVRM_StaffwiseClassStdata

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseClassStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //SqlDbType.BigInt)
                    //    {
                    //        Value = data.HRME_Id
                    //    });

                    cmd.Parameters.Add(new SqlParameter("@UserId",
                SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.classlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //var clsslst = _PortalContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                //if (clsslst.Length > 0)
                //{
                //    data.classlist = clsslst;
                //}

                //*********************************** User Department List ********************************// 
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_DepartmentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = "a"
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
SqlDbType.BigInt)
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
                        data.departmentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.route_list = (from a in _PortalContext.MasterRouteDMO
                                   where (a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg==true)
                                  select new MasterRouteDTO
                                  {
                                      TRMR_Id = a.TRMR_Id,
                                      TRMR_RouteName = a.TRMR_RouteName,
                                      TRMR_RouteNo = a.TRMR_RouteNo
                                  }).Distinct().ToArray();



                data.fee_group = (from a in _PortalContext.FeeGroupDMO
                                  where (a.MI_Id == data.MI_Id)
                                  select new IVRM_NoticeBoardDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_Name = a.FMG_GroupName
                                  }).Distinct().ToArray();



                data.fee_terms = (from a in _PortalContext.feeTr
                                  where (a.MI_Id == data.MI_Id)
                                  select new IVRM_NoticeBoardDTO
                                  {
                                      FMT_Id = a.FMT_Id,
                                      FMT_Name = a.FMT_Name
                                  }).Distinct().ToArray();



                data.fee_terms = (from a in _PortalContext.feeTr
                                  where (a.MI_Id == data.MI_Id)
                                  select new IVRM_NoticeBoardDTO
                                  {
                                      FMT_Id = a.FMT_Id,
                                      FMT_Name = a.FMT_Name
                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_NoticeBoardDTO savedetail(IVRM_NoticeBoardDTO data)
        {
            try
            {
                int returnval;
                var header_flg = "NoticeBoard";
                List<long> cls_ids = new List<long>();
                if (data.INTB_Id > 0)
                {

                    var resultobj = _PortalContext.IVRM_NoticeBoardDMO.Single(t => t.INTB_Id.Equals(data.INTB_Id) && t.MI_Id.Equals(data.MI_Id));

                    resultobj.INTB_Title = data.INTB_Title;
                    resultobj.INTB_Description = data.INTB_Description;
                    resultobj.INTB_StartDate = data.INTB_StartDate;
                    resultobj.INTB_EndDate = data.INTB_EndDate;
                    resultobj.INTB_DisplayDate = data.INTB_DisplayDate;
                    resultobj.INTB_Attachment = data.INTB_Attachment;
                    resultobj.INTB_FilePath = data.INTB_FilePath;
                    resultobj.NTB_TTSylabusFlg = data.NTB_TTSylabusFlg;
                    resultobj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
                    resultobj.INTB_ToStaffFlg = data.INTB_ToStaffFlg;
                    resultobj.INTB_ToStudentFlg = data.INTB_ToStudentFlg;
                    resultobj.INTB_ActiveFlag = true;
                    resultobj.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(resultobj);
                    if (data.getclass != true)
                    {
                        if (data.INTB_ToStudentFlg == true)
                        {

                            var resultclass = _PortalContext.IVRM_NoticeBoard_Class_DMO.Single(t => t.INTB_Id == data.INTB_Id);
                            resultclass.ASMCL_Id = data.ASMCL_Id;
                            resultclass.UpdatedDate = DateTime.Now;
                            resultclass.INTBC_ActiveFlag = true;
                            _PortalContext.Update(resultclass);




                            var resultsec = _PortalContext.IVRM_NoticeBoard_Class_Section_DMO_con.Where(a => a.INTBC_Id == resultclass.INTBC_Id).ToList();
                            foreach (var item in resultsec)
                            {
                                var secdlt = _PortalContext.IVRM_NoticeBoard_Class_Section_DMO_con.Single(a => a.INTBC_Id == resultclass.INTBC_Id && a.ASMS_Id == item.ASMS_Id);
                                _PortalContext.Remove(secdlt);
                            }

                            foreach (var sec in data.sectionlistarray)
                            {
                                IVRM_NoticeBoard_Class_Section_DMO sc = new IVRM_NoticeBoard_Class_Section_DMO();
                                sc.ASMS_Id = sec.ASMS_Id;
                                sc.INTBC_Id = resultclass.INTBC_Id;
                                sc.INTBC_ActiveFlag = true;
                                sc.CreatedDate = DateTime.Today;
                                sc.UpdatedDate = DateTime.Today;
                                sc.INTBC_CreatedBy = data.UserId;
                                sc.INTBC_UpdatedBy = data.UserId;
                                _PortalContext.Add(sc);
                            }
                            var dlt = _PortalContext.IVRM_NoticeBoard_Student_DMO_con.Where(a => a.INTB_Id == data.INTB_Id).ToList();
                            foreach (var item in dlt)
                            {
                                var r_dlt = _PortalContext.IVRM_NoticeBoard_Student_DMO_con.Single(a => a.INTB_Id == data.INTB_Id && a.AMST_Id == item.AMST_Id);
                                _PortalContext.Remove(r_dlt);

                            }
                            foreach (var stu in data.studentarray)
                            {
                                IVRM_NoticeBoard_Student_DMO st = new IVRM_NoticeBoard_Student_DMO();
                                st.AMST_Id = stu.AMST_Id;
                                st.INTB_Id = data.INTB_Id;
                                st.INTBCSTD_ActiveFlag = true;
                                st.CreatedDate = DateTime.Today;
                                st.UpdatedDate = DateTime.Today;
                                st.INTBCSTD_CreatedBy = data.UserId;
                                st.INTBCSTD_UpdatedBy = data.UserId;
                                _PortalContext.Add(st);
                            }
                        }
                    }
                    if (data.INTB_ToStaffFlg == true)
                    {
                        var resultstf = _PortalContext.IVRM_NoticeBoard_Staff_DMO_con.Where(a => a.INTB_Id == data.INTB_Id).ToList();
                        foreach (var item in resultstf)
                        {
                            var dltstf = _PortalContext.IVRM_NoticeBoard_Staff_DMO_con.Single(a => a.INTB_Id == data.INTB_Id && a.HRME_Id == item.HRME_Id);
                            _PortalContext.Remove(dltstf);
                        }

                        foreach (var emp in data.employeearraylist)
                        {
                            IVRM_NoticeBoard_Staff_DMO em = new IVRM_NoticeBoard_Staff_DMO();
                            em.INTB_Id = data.INTB_Id;
                            em.HRME_Id = emp.HRME_Id;
                            em.INTBCSTF_ActiveFlag = true;
                            em.CreatedDate = DateTime.Today;
                            em.UpdatedDate = DateTime.Today;
                            em.INTBCSTF_CreatedBy = data.UserId;
                            em.INTBCSTF_UpdatedBy = data.UserId;
                            _PortalContext.Add(em);

                        }
                    }

                    if (data.FilePath_Array.Length > 0)
                    {
                        var resultstf = _PortalContext.IVRM_NoticeBoard_FilesDMO_con.Where(a => a.INTB_Id == data.INTB_Id).ToList();
                        foreach (var item in resultstf)
                        {
                            var dltstf = _PortalContext.IVRM_NoticeBoard_FilesDMO_con.Single(a => a.INTB_Id == data.INTB_Id && a.INTBFL_Id == item.INTBFL_Id);
                            _PortalContext.Remove(dltstf);
                        }

                        foreach (var fl in data.FilePath_Array)
                        {
                            IVRM_NoticeBoard_FilesDMO em = new IVRM_NoticeBoard_FilesDMO();
                            em.INTB_Id = data.INTB_Id;
                            em.MI_Id = data.MI_Id;
                            em.INTBFL_FileName = fl.FileName;
                            em.INTBFL_FilePath = fl.INTBFL_FilePath;
                            em.INTBFL_ActiveFlag = true;
                            em.INTBFL_UpdatedDate = DateTime.Today;
                            em.INTBFL_UpdatedBy = data.UserId;
                            _PortalContext.Add(em);

                        }
                    }
                    returnval = _PortalContext.SaveChanges();

                    if (returnval > 0)
                    {
                        data.returnval = true;

                        if (data.INTB_ToStudentFlg == true)
                        {
                            if (data.getclass == true)
                            {
                                List<long> clsid = new List<long>();
                                foreach (var item in data.classlsttwo)
                                {
                                    clsid.Add(item.ASMCL_Id);
                                }
                                var devicelist = (from a in _PortalContext.Adm_M_Student
                                                  from b in _PortalContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && clsid.Contains(b.ASMCL_Id))
                                                  select new IVRM_Homework_DTO
                                                  {
                                                      AMST_MobileNo = a.AMST_MobileNo,
                                                      AMST_Id = a.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();


                                IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                dto.devicelist12 = devicelist;

                                data.deviceArray = devicelist.ToArray();




                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                              from b in _PortalContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PortalContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                        }
                                    }
                                }

                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;
                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }
                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, data.INTB_Id, data.MI_Id, dto, header_flg);
                                //        }

                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //    callnotification(devicenew, data.INTB_Id, data.MI_Id, dto, header_flg);
                                //}
                            }
                            else
                            {
                                //=========================================== Notification
                                var devicelist = (from a in _PortalContext.Adm_M_Student
                                                  from b in _PortalContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id)
                                                  select new IVRM_Homework_DTO
                                                  {
                                                      AMST_MobileNo = a.AMST_MobileNo,
                                                      AMST_Id = a.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();


                                IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                dto.devicelist12 = devicelist;

                                data.deviceArray = devicelist.ToArray();





                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                              from b in _PortalContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PortalContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                        }
                                    }
                                }

                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;
                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }
                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, data.INTB_Id, data.MI_Id, dto, header_flg);
                                //        }
                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //    //callnotification(devicenew, data.INTB_Id, data.MI_Id, dto, header_flg);
                                //}
                            }

                        }

                        if (data.INTB_ToStaffFlg == true)
                        {
                            List<long> hrmeid = new List<long>();
                            foreach (var item in data.employeearraylist)
                            {
                                hrmeid.Add(item.HRME_Id);
                            }
                            //=========================================== Notification
                            var devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && hrmeid.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                              select new IVRM_Homework_DTO
                                              {
                                                  AMST_MobileNo = Convert.ToInt64(a.HRME_MobileNo),
                                                  HRME_Id = a.HRME_Id,
                                                  AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToList();


                            IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                            dto.devicelist12 = devicelist;

                            data.deviceArray = devicelist.ToArray();


                            var deviceidsnew = "";
                            var devicenew = "";
                            var redirecturl = "";
                            long revieveduserid = 0;

                            if (devicelist.Count > 0)
                            {
                                foreach (var device_id in devicelist)
                                {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                    {


                                        revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                          from b in _PortalContext.ApplicationUser
                                                          where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();



                                        PushNotification push_noti = new PushNotification(_PortalContext);
                                        push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                    }
                                }
                            }

                            //var deviceidsnew = "";
                            //var devicenew = "";
                            //if (devicelist.Count > 0)
                            //{
                            //    int k = 0;
                            //    foreach (var deviceid in devicelist)
                            //    {
                            //        if (k == 0)
                            //        {
                            //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //            k = 1;
                            //        }
                            //        else
                            //        {
                            //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //        }
                            //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                            //        {
                            //            callnotification(deviceid.AMST_AppDownloadedDeviceId, data.INTB_Id, data.MI_Id, dto, header_flg);
                            //        }
                            //    }
                            //    devicenew = "[" + deviceidsnew + "]";

                            //   // callnotification(devicenew, data.INTB_Id, data.MI_Id, dto, header_flg);
                            //}

                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else
                {
                   
                    IVRM_NoticeBoardDMO obj = new IVRM_NoticeBoardDMO();

                    obj.MI_Id = data.MI_Id;
                    obj.INTB_Title = data.INTB_Title;
                    obj.INTB_Description = data.INTB_Description;
                    obj.INTB_StartDate = data.INTB_StartDate;
                    obj.INTB_EndDate = data.INTB_EndDate;
                    obj.INTB_DisplayDate = data.INTB_DisplayDate;
                    obj.INTB_Attachment = data.INTB_Attachment;
                    obj.INTB_FilePath = data.INTB_FilePath;
                    obj.NTB_TTSylabusFlg = data.NTB_TTSylabusFlg;
                    obj.INTB_DispalyDisableFlg = data.INTB_DispalyDisableFlg;
                    obj.INTB_ToStaffFlg = data.INTB_ToStaffFlg;
                    obj.INTB_ToStudentFlg = data.INTB_ToStudentFlg;
                    obj.INTB_ActiveFlag = true;

                    obj.CreatedDate = DateTime.Now;
                    _PortalContext.Add(obj);
                    returnval= _PortalContext.SaveChanges();
                    data.INTB_Id = obj.INTB_Id;
                    if (data.INTB_ToStudentFlg == true || data.INTB_ToStaffFlg == true)
                    {
                        if (data.INTB_ToStudentFlg == true)
                        {
                            if (data.getclass == true)
                            {
                                foreach (var cls in data.classlsttwo)
                                {
                                    IVRM_NoticeBoard_Class_DMO obj2 = new IVRM_NoticeBoard_Class_DMO();
                                    obj2.INTB_Id = data.INTB_Id;
                                    obj2.ASMCL_Id = cls.ASMCL_Id;
                                    obj2.INTBC_ActiveFlag = true;
                                    obj2.CreatedDate = DateTime.Now;
                                    obj2.UpdatedDate = DateTime.Now;
                                    _PortalContext.Add(obj2);

                                    var edit1 = (from a in _PortalContext.School_Adm_Y_StudentDMO
                                                 from b in _PortalContext.School_M_Section
                                                 where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == cls.ASMCL_Id)
                                                 select new IVRM_NoticeBoardDTO
                                                 {
                                                     ASMS_Id = a.ASMS_Id,
                                                     ASMC_SectionName = b.ASMC_SectionName

                                                 }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                                    foreach (var sec in edit1)
                                    {
                                        IVRM_NoticeBoard_Class_Section_DMO sc = new IVRM_NoticeBoard_Class_Section_DMO();
                                        sc.ASMS_Id = sec.ASMS_Id;
                                        sc.INTBC_Id = obj2.INTBC_Id;
                                        sc.INTBC_ActiveFlag = true;
                                        sc.CreatedDate = DateTime.Today;
                                        sc.INTBC_CreatedBy = data.UserId;
                                        _PortalContext.Add(sc);


                                        var student1 = (from a in _PortalContext.School_Adm_Y_StudentDMO
                                                        from b in _PortalContext.Adm_M_Student
                                                        where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == cls.ASMCL_Id
           && a.ASMS_Id == sec.ASMS_Id && b.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S")
                                                        select new IVRM_NoticeBoardDTO
                                                        {
                                                            AMST_Id = a.AMST_Id,
                                                        }).Distinct().ToList();

                                        foreach (var stu in student1)
                                        {
                                            IVRM_NoticeBoard_Student_DMO st = new IVRM_NoticeBoard_Student_DMO();
                                            st.AMST_Id = stu.AMST_Id;
                                            st.INTB_Id = data.INTB_Id;
                                            st.INTBCSTD_ActiveFlag = true;
                                            st.CreatedDate = DateTime.Today;
                                            st.INTBCSTD_CreatedBy = data.UserId;
                                            _PortalContext.Add(st);
                                        }
                                    }



                                }
                            }


                            //==================
                            else
                            {

                                int dcnt = 0;

                                foreach (var cls in data.classlsttwo)

                                {
                                    IVRM_NoticeBoard_Class_DMO obj2 = new IVRM_NoticeBoard_Class_DMO();

                                    obj2.INTB_Id = data.INTB_Id;

                                    obj2.ASMCL_Id = cls.ASMCL_Id;
                                    obj2.INTBC_ActiveFlag = true;
                                    obj2.CreatedDate = DateTime.Now;
                                    obj2.UpdatedDate = DateTime.Now;

                                    _PortalContext.Add(obj2);
                                    if (data.sectionlistarray != null)
                                    {
                                        foreach (var sec in data.sectionlistarray)
                                        {

                                            if (cls.ASMCL_Id == sec.ASMCL_Id)
                                            {
                                                IVRM_NoticeBoard_Class_Section_DMO sc = new IVRM_NoticeBoard_Class_Section_DMO();
                                                sc.ASMS_Id = sec.ASMS_Id;

                                                sc.INTBC_Id = obj2.INTBC_Id;
                                                sc.INTBC_ActiveFlag = true;
                                                sc.CreatedDate = DateTime.Today;
                                                sc.INTBC_CreatedBy = data.UserId;
                                                _PortalContext.Add(sc);
                                            }

                                        }
                                    }


                                }

                                if (data.studentarray != null && data.studentarray.Length > 0)
                                {
                                    foreach (var stu in data.studentarray)
                                    {
                                        IVRM_NoticeBoard_Student_DMO st = new IVRM_NoticeBoard_Student_DMO();
                                        st.AMST_Id = stu.AMST_Id;
                                        st.INTB_Id = data.INTB_Id;
                                        st.INTBCSTD_ActiveFlag = true;
                                        st.CreatedDate = DateTime.Today;
                                        st.INTBCSTD_CreatedBy = data.UserId;
                                        _PortalContext.Add(st);
                                    }
                                }
                            }
                        }

                        if (data.INTB_ToStaffFlg == true)
                        {
                            foreach (var emp in data.employeearraylist)
                            {
                                IVRM_NoticeBoard_Staff_DMO em = new IVRM_NoticeBoard_Staff_DMO();
                                em.INTB_Id = data.INTB_Id;
                                em.HRME_Id = emp.HRME_Id;
                                em.INTBCSTF_ActiveFlag = true;
                                em.CreatedDate = DateTime.Today;
                                em.INTBCSTF_CreatedBy = data.UserId;
                                _PortalContext.Add(em);
                            }
                        }
                        if (data.FilePath_Array.Length > 0)
                        {
                            foreach (var fl in data.FilePath_Array)
                            {
                                IVRM_NoticeBoard_FilesDMO em = new IVRM_NoticeBoard_FilesDMO();
                                em.INTB_Id = data.INTB_Id;
                                em.MI_Id = data.MI_Id;
                                em.INTBFL_FileName = fl.FileName;
                                em.INTBFL_FilePath = fl.INTBFL_FilePath;
                                em.INTBFL_ActiveFlag = true;
                                em.INTBFL_CreatedDate = DateTime.Today;
                                em.INTBFL_CreatedBy = data.UserId;
                                _PortalContext.Add(em);

                            }
                        }


                        returnval = _PortalContext.SaveChanges();
                    }
                    
                    if (returnval > 0)
                    {
                        data.returnval = true;
                        if (data.INTB_ToStudentFlg == true)
                        {
                            //=========================================== Notification
                            if (data.getclass == true)
                            {
                                List<long> cls_ids1 = new List<long>();
                                foreach (var item in data.classlsttwo)
                                {
                                    cls_ids1.Add(item.ASMCL_Id);
                                }
                                var devicelist = (from a in _PortalContext.Adm_M_Student
                                                  from b in _PortalContext.School_Adm_Y_StudentDMO
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id && cls_ids1.Contains(b.ASMCL_Id))
                                                  select new IVRM_Homework_DTO
                                                  {
                                                      AMST_MobileNo = a.AMST_MobileNo,
                                                      AMST_Id = a.AMST_Id,
                                                      AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                                  }).Distinct().ToList();


                                IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                dto.devicelist12 = devicelist;

                                data.deviceArray = devicelist.ToArray();

                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                              from b in _PortalContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PortalContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                        }
                                    }
                                }
                                //var deviceidsnew = "";
                                //var devicenew = "";
                                //if (devicelist.Count > 0)
                                //{
                                //    int k = 0;
                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }
                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, obj.INTB_Id, data.MI_Id, dto, header_flg);
                                //        }
                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                // //   callnotification(devicenew, obj.INTB_Id, data.MI_Id, dto, header_flg);
                                //}

                            }

                            else
                            {
                                string def = "";
                                string select_student = "";

                                string asmcl_id1 = "0";
                                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                                {
                                    foreach (var item in data.classlsttwo)
                                    {

                                        asmcl_id1 = asmcl_id1 + "," + item.ASMCL_Id;
                                    }
                                }
                                string ASMS_Id1 = "0";
                                if (data.sectionlistarray != null && data.sectionlistarray.Length > 0)
                                {
                                    foreach (var item in data.sectionlistarray)
                                    {

                                        ASMS_Id1 = ASMS_Id1 + "," + item.ASMS_Id;
                                    }
                                }
                                string AMST_Ids = "0";

                                if (data.studentarray != null && data.studentarray.Length > 0)
                                {

                                    foreach (var item in data.studentarray)
                                    {

                                        AMST_Ids = AMST_Ids + "," + item.AMST_Id;
                                    }
                                    select_student = "1";
                                }
                                else
                                {
                                    select_student = "0";
                                }


                                string FMG_Id1 = "0";
                                string FMT_Id1 = "0";


                                if (data.fee_def == true)
                                {
                                    if (data.defgrparray != null && data.defgrparray.Length > 0)
                                    {
                                        foreach (var item in data.defgrparray)
                                        {

                                            FMG_Id1 = FMG_Id1 + "," + item.FMG_Id;
                                        }
                                    }
                                    if (data.defarray != null && data.defarray.Length > 0)
                                    {
                                        foreach (var item in data.defarray)
                                        {

                                            FMT_Id1 = FMT_Id1 + "," + item.FMT_Id;
                                        }
                                    }
                                    def = "1";
                                }
                                else
                                {
                                    def = "0";
                                }


                                List<IVRM_Homework_DTO> devicelist = new List<IVRM_Homework_DTO>();
                                //var ddata = new List();
                                var ddata = new { };
                                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "FEE_TERMS_STUDENTS";
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = data.MI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                                  SqlDbType.VarChar)
                                    {
                                        // Value = dto.ASMCL_Id
                                        Value = asmcl_id1
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                                    {
                                        Value = data.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                                    SqlDbType.VarChar)
                                    {
                                        Value = ASMS_Id1
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                 SqlDbType.VarChar)
                                    {
                                        Value = FMG_Id1
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                                 SqlDbType.VarChar)
                                    {
                                        Value = FMT_Id1
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@type",
                                   SqlDbType.BigInt)
                                    {
                                        Value = def
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@student_flag",
                                 SqlDbType.BigInt)
                                    {
                                        Value = select_student
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                               SqlDbType.VarChar)
                                    {
                                        Value = AMST_Ids
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
                                                devicelist.Add(new IVRM_Homework_DTO
                                                {

                                                    AMST_MobileNo = Convert.ToInt64(dataReader["AMST_MobileNo"]),
                                                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                    AMST_AppDownloadedDeviceId = Convert.ToString(dataReader["AMST_AppDownloadedDeviceId"])
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }


                                var deviceidsnew = "";
                                var devicenew = "";
                                var redirecturl = "";
                                long revieveduserid = 0;

                                if (devicelist.Count > 0)
                                {
                                    foreach (var device_id in devicelist)
                                    {
                                        if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                        {


                                            revieveduserid = (from a in _PortalContext.StudentUserLoginDMO
                                                              from b in _PortalContext.ApplicationUser
                                                              where (a.IVRMSTUUL_UserName == b.UserName && a.AMST_Id == device_id.AMST_Id)
                                                              select b).Select(t => t.Id).FirstOrDefault();



                                            PushNotification push_noti = new PushNotification(_PortalContext);
                                            push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                        }
                                    }
                                }
                                //
                                //var deviceidsnew = "";
                                //string devicenew = "";
                                //IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                                //dto.devicelist12 = devicelist.ToList();
                                ////old code//
                                //if (ddata != null)


                                //{
                                //    int k = 0;
                                //    foreach (var deviceid in devicelist)
                                //    {
                                //        if (k == 0)
                                //        {
                                //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //            k = 1;
                                //        }
                                //        else
                                //        {
                                //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                //        }

                                //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                                //        {
                                //            callnotification(deviceid.AMST_AppDownloadedDeviceId, obj.INTB_Id, data.MI_Id, dto, header_flg);
                                //        }


                                //    }
                                //    devicenew = "[" + deviceidsnew + "]";

                                //   // callnotification(devicenew, obj.INTB_Id, data.MI_Id, dto, header_flg);
                                //}
                            }
                        }

                        if (data.INTB_ToStaffFlg == true)
                        {
                            List<long> hrmeid = new List<long>();
                            foreach (var item in data.employeearraylist)
                            {
                                hrmeid.Add(item.HRME_Id);
                            }
                            //=========================================== Notification
                            var devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && hrmeid.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                              select new IVRM_Homework_DTO
                                              {
                                                  AMST_MobileNo = Convert.ToInt64(a.HRME_MobileNo),
                                                  HRME_Id = a.HRME_Id,
                                                  AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToList();


                            IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                            dto.devicelist12 = devicelist;

                            data.deviceArray = devicelist.ToArray();

                            var deviceidsnew = "";
                            var devicenew = "";
                            var redirecturl = "";
                            long revieveduserid = 0;

                            if (devicelist.Count > 0)
                            {
                                foreach (var device_id in devicelist)
                                {
                                    if (device_id.AMST_AppDownloadedDeviceId != null && device_id.AMST_AppDownloadedDeviceId != "")
                                    {


                                        revieveduserid = (from a in _PortalContext.IVRM_Staff_User_Login
                                                          from b in _PortalContext.ApplicationUser
                                                          where (a.IVRMSTAUL_UserName == b.UserName && a.Emp_Code == device_id.HRME_Id)
                                                          select b).Select(t => t.Id).FirstOrDefault();



                                        PushNotification push_noti = new PushNotification(_PortalContext);
                                        push_noti.Call_PushNotificationGeneral(device_id.AMST_AppDownloadedDeviceId, data.MI_Id, data.UserId, revieveduserid, data.INTB_Id, data.INTB_Title, "NoticeBoard", "NoticeBoard");

                                    }
                                }
                            }
                            //var deviceidsnew = "";
                            //var devicenew = "";
                            //if (devicelist.Count > 0)
                            //{
                            //    int k = 0;
                            //    foreach (var deviceid in devicelist)
                            //    {
                            //        if (k == 0)
                            //        {
                            //            deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //            k = 1;
                            //        }
                            //        else
                            //        {
                            //            deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            //        }

                            //        if (deviceid.AMST_AppDownloadedDeviceId != "" && deviceid.AMST_AppDownloadedDeviceId != null)
                            //        {
                            //            callnotification(deviceid.AMST_AppDownloadedDeviceId, obj.INTB_Id, data.MI_Id, dto, header_flg);
                            //        }

                            //    }
                            //    devicenew = "[" + deviceidsnew + "]";

                            //    //callnotification(devicenew, obj.INTB_Id, data.MI_Id, dto, header_flg);
                            //}

                        }




                    }
                    else
                    {
                        data.returnval = false;
                    }
                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public string callnotification(string devicenew, long intb_Id, long mi_id, IVRM_Homework_DTO dto, string header_flg)

        {


            try
            {
                var key = _PortalContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                IVRM_ClassWorkDTO data = new IVRM_ClassWorkDTO();
                var notice = _PortalContext.IVRM_NoticeBoardDMO.Where(h => h.MI_Id == mi_id && h.INTB_ActiveFlag == true && h.INTB_Id == intb_Id).Distinct().ToList();
                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + notice.FirstOrDefault().INTB_Title + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + notice.FirstOrDefault().INTB_Description + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                string sound = "default";
                string notId = "2";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + notice.FirstOrDefault().INTB_Title + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' +
                // +'"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , "
                // + '"' + "body" + '"' + ":" + '"' + notice.FirstOrDefault().INTB_Description + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

            //    daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
            //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Notice Board" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notice.FirstOrDefault().INTB_Title + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " }" + "," + '"' + "data" + '"' + ":" + "{" + '"' + "page" + '"' + ":" + '"' + "noticeBoard" + '"' + "}" + "}";

            //    notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);


                Dictionary<string, object> input = new Dictionary<string, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("body", notice.FirstOrDefault().INTB_Title);
                transfersnotes.Add("title", "Notice Board");

                input.Add("to", devicenew);
                input.Add("notification", transfersnotes);


                var myContent = JsonConvert.SerializeObject(input);
                String postdata = myContent;

                //var mycontent = notificationparams[0];
                //string postdata = mycontent.ToString();


                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
                PushNotification push_noti = new PushNotification(_PortalContext);

                push_noti.Insert_PushNotification_noticeboard(intb_Id, mi_id, dto, header_flg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }


        public IVRM_NoticeBoardDTO getsection(IVRM_NoticeBoardDTO dto)
        {
            try
            {

                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }
                //added by roopa//
               
                var asmcl_ids = "0";
                if (dto.classlsttwo.Length > 0)
                {
                    foreach (var ue in dto.classlsttwo)
                    {
                        asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                    
                    }

                }

                //
                //var edit = (from a in _PortalContext.School_Adm_Y_StudentDMO
                //            from b in _PortalContext.School_M_Section
                //            from c in _PortalContext.School_M_Class
                //            where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == dto.ASMAY_Id && asmcl_ids.Contains(a.ASMCL_Id))
                //            select new IVRM_NoticeBoardDTO
                //            {
                //                ASMS_Id = a.ASMS_Id,
                //                ASMCL_Id = c.ASMCL_Id,
                //                ASMC_SectionName = b.ASMC_SectionName,
                //                ASMCL_ClassName = c.ASMCL_ClassName

                //            }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                //if (edit.Length > 0)
                //{
                //    dto.sectionlist = edit.ToArray();
                //}

                //added by roopa
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseSectionStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //SqlDbType.BigInt)
                    //    {
                    //        Value = dto.HRME_Id
                    //    });

                    cmd.Parameters.Add(new SqlParameter("@UserId",
                SqlDbType.BigInt)
                    {
                        Value = dto.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
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
                        dto.sectionlist = retObject.ToArray();
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
            return dto;
        }
        public IVRM_NoticeBoardDTO editdetails(IVRM_NoticeBoardDTO data)
        {
            try
            {
                var edit = (from a in _PortalContext.IVRM_NoticeBoardDMO
                            where (a.INTB_Id == data.INTB_Id && a.MI_Id == data.MI_Id)
                            select new IVRM_NoticeBoardDTO
                            {
                                INTB_Id = a.INTB_Id,
                                INTB_Title = a.INTB_Title,
                                INTB_Description = a.INTB_Description,
                                INTB_StartDate = a.INTB_StartDate,
                                INTB_EndDate = a.INTB_EndDate,
                                INTB_DisplayDate = a.INTB_DisplayDate,
                                INTB_Attachment = a.INTB_Attachment,
                                INTB_FilePath = a.INTB_FilePath,
                                NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                INTB_DispalyDisableFlg = a.INTB_DispalyDisableFlg,
                                INTB_ToStaffFlg = a.INTB_ToStaffFlg,
                                INTB_ToStudentFlg = a.INTB_ToStudentFlg,

                            }).Distinct().OrderBy(t => t.INTB_Id).ToArray();
                if (edit.Length > 0)
                {
                    data.editdetails = edit;
                }

                data.attachementlist = (from a in _PortalContext.IVRM_NoticeBoardDMO
                                        from b in _PortalContext.IVRM_NoticeBoard_FilesDMO_con
                                        where a.INTB_Id == data.INTB_Id && a.INTB_Id == b.INTB_Id
                                        select new IVRM_NoticeBoardDTO
                                        {
                                            INTBFL_FileName = b.INTBFL_FileName,
                                            INTBFL_FilePath = b.INTBFL_FilePath,
                                            INTB_Attachment = a.INTB_Attachment,
                                            INTB_Id = a.INTB_Id
                                        }).ToArray();

                var check = _PortalContext.IVRM_NoticeBoardDMO.Single(a => a.MI_Id == data.MI_Id && a.INTB_Id == data.INTB_Id);
                if (check.INTB_ToStudentFlg == true)
                {
                   // int asmsid = 0;

                    var cls = _PortalContext.IVRM_NoticeBoard_Class_DMO.Where(a => a.INTB_Id == data.INTB_Id).ToList();
                    data.editclasslist = cls.ToArray();

                    List<long> INTBC_Ids = new List<long>();
                    List<long> asmclids = new List<long>();
                    foreach (var item in cls)
                    {
                        INTBC_Ids.Add(item.INTBC_Id);
                        asmclids.Add(item.ASMCL_Id);
                    }

                    string asmcl_id = "0";
                    foreach (var item in cls)
                    {
                        // asmsid1.Add(item.ASMS_Id);
                        asmcl_id = asmcl_id + "," + item.ASMCL_Id;
                    }

                    //var cls = (from a in _PortalContext.IVRM_NoticeBoard_Class_DMO
                    //           from b in _PortalContext.School_M_Class
                    //           where(a.ASMCL_Id==b.ASMCL_Id && a.INTB_Id==data.INTB_Id)
                    //           select new IVRM_NoticeBoardDTO
                    //           {
                    //               ASMCL_Id = a.ASMCL_Id,
                    //               ASMCL_ClassName = b.ASMCL_ClassName

                    //           }).Distinct().OrderBy(t => t.ASMCL_ClassName).ToList();
                    //List<long> clss = new List<long>();
                    //if (cls.Count > 0)
                    //{
                    //    data.editclasslist = cls.ToArray();

                    //    foreach (var c in cls)
                    //    {
                    //        clss.Add(c.ASMCL_Id);
                    //    }
                    //}

                    //var sect = (
                    //            from c in _PortalContext.IVRM_NoticeBoard_Class_Section_DMO_con
                    //            from d in _PortalContext.School_M_Section
                    //            where ( clss.Contains(c.INTBC_Id) && c.ASMS_Id==d.ASMS_Id )
                    //            select new IVRM_NoticeBoardDTO
                    //            {
                    //                ASMS_Id = c.ASMS_Id,                                 
                    //                ASMC_SectionName = d.ASMC_SectionName

                    //            }).Distinct().OrderBy(t => t.ASMC_SectionName).ToList();
                    //data.sect = sect.ToArray();



                    //var section = _PortalContext.IVRM_NoticeBoard_Class_Section_DMO_con.Where(a => a.INTBC_Id == cls[0].INTBC_Id).Select(a => a.ASMS_Id).ToList();
                    //asmsid = String.Join(",", section);
                    //roopa
                    //var stcnt = _PortalContext.IVRM_NoticeBoard_Student_DMO_con.Where(a => a.INTB_Id == data.INTB_Id).ToList();
                    //if(stcnt.Count>0)
                    //{
                    //    data.stu = (from a in _PortalContext.IVRM_NoticeBoardDMO
                    //                 from d in _PortalContext.IVRM_NoticeBoard_Student_DMO_con
                    //                 from e in _PortalContext.Adm_M_Student
                    //                 where (a.INTB_Id==d.INTB_Id && a.MI_Id == data.MI_Id && d.AMST_Id==e.AMST_Id && d.INTB_Id==data.INTB_Id)
                    //                 select new IVRM_NoticeBoardDTO
                    //                 {
                    //                     AMST_Id = d.INTBCSTD_Id,
                    //                     studentname = e.AMST_FirstName + (string.IsNullOrEmpty(e.AMST_MiddleName) ? "" : ' ' + e.AMST_MiddleName) + (string.IsNullOrEmpty(e.AMST_LastName) ? "" : ' ' + e.AMST_LastName)

                    //                 }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();

                    //}
                    //roopa end


                   // var section1 = _PortalContext.IVRM_NoticeBoard_Class_DMO.Where(a => INTBC_Ids.Contains(a.INTBC_Id)).ToArray();

                    var section1 = (from a in _PortalContext.IVRM_NoticeBoard_Class_DMO
                                    from b in _PortalContext.School_Adm_Y_StudentDMO
                                    from c in _PortalContext.IVRM_NoticeBoardDMO
                                    from d in _PortalContext.School_M_Section
                                    from e in _PortalContext.School_M_Class
                                    where (c.MI_Id == data.MI_Id && a.INTB_Id == c.INTB_Id && b.ASMS_Id==d.ASMS_Id && a.INTB_Id == data.INTB_Id && b.ASMCL_Id == e.ASMCL_Id)
                                    select new IVRM_NoticeBoardDTO
                                    {
                                        ASMS_Id = b.ASMS_Id,
                                        ASMC_SectionName = d.ASMC_SectionName,
                                        ASMCL_ClassName = e.ASMCL_ClassName
                                    }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();

                    if (section1.Length > 0)
                    {
                        data.editsection = section1;
                    }

                   // List<long> asmsid1 = new List<long>();

                    string asmsid1 = "0";
                    foreach (var item in section1)
                    {
                        // asmsid1.Add(item.ASMS_Id);
                        asmsid1 = asmsid1 + "," + item.ASMS_Id;

                    }
                   
                    var student = _PortalContext.IVRM_NoticeBoard_Student_DMO_con.Where(a => a.INTB_Id == data.INTB_Id).ToArray();
                    if (student.Length > 0)
                    {
                        data.editstudent = student;
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "NoticeBoard_Student";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar)
                        {
                            Value = asmcl_id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar)
                        {
                            Value = asmsid1
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
                            data.studentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //old code
                    var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                    if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                    {

                        data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    else
                    {
                        data.HRME_Id = 0;
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_StaffwiseClassStdata";

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INTB_Id", SqlDbType.BigInt)
                        {
                            Value = data.INTB_Id
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
                            data.classlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    var edit1 = (from a in _PortalContext.IVRM_NoticeBoard_Class_DMO
                                    from b in _PortalContext.School_Adm_Y_StudentDMO
                                    from c in _PortalContext.IVRM_NoticeBoardDMO
                                    from d in _PortalContext.School_M_Section
                                    from e in _PortalContext.School_M_Class
                                    where (c.MI_Id == data.MI_Id && a.INTB_Id == c.INTB_Id && b.ASMS_Id == d.ASMS_Id && a.INTB_Id == data.INTB_Id 
                                    && b.ASMAY_Id==data.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id)
                                    select new IVRM_NoticeBoardDTO
                                    {
                                        ASMS_Id = b.ASMS_Id,
                                        ASMCL_Id = e.ASMCL_Id,
                                        ASMC_SectionName = d.ASMC_SectionName,
                                        ASMCL_ClassName = e.ASMCL_ClassName

                                    }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();

                    //var edit1 = (from a in _PortalContext.School_Adm_Y_StudentDMO
                    //            from b in _PortalContext.School_M_Section
                    //            from c in _PortalContext.School_M_Class
                    //            where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMCL_Id == c.ASMCL_Id 
                    //            && a.ASMAY_Id == data.ASMAY_Id && asmclids.Contains(a.ASMCL_Id))
                    //            select new IVRM_NoticeBoardDTO
                    //            {
                    //                ASMS_Id = a.ASMS_Id,
                    //                ASMCL_Id = c.ASMCL_Id,
                    //                ASMC_SectionName = b.ASMC_SectionName,
                    //                ASMCL_ClassName = c.ASMCL_ClassName

                    //            }).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                    if (edit1.Length > 0)
                    {
                        data.sectionlist = edit1.ToArray();
                    }


                    //old code end
                }
                //================= staff======
                if (check.INTB_ToStaffFlg == true)
                {
                    var emp = (from a in _PortalContext.HR_Master_Employee_DMO
                               from b in _PortalContext.IVRM_NoticeBoard_Staff_DMO_con
                               where a.HRME_Id == b.HRME_Id && b.INTB_Id == data.INTB_Id
                               select new IVRM_NoticeBoardDTO
                               {
                                   HRME_Id = a.HRME_Id,
                                   employeename = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) || a.HRME_EmployeeMiddleName == "0" ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) || a.HRME_EmployeeLastName == "0" ? "" : ' ' + a.HRME_EmployeeLastName)
                               }).ToArray();
                    if (emp.Length > 0)
                    {
                        data.editstaff = emp;
                    }


                    List<long> hrmeid = new List<long>();
                    foreach (var ue in emp)
                    {
                        hrmeid.Add(ue.HRME_Id);
                    }
                    var desg = (from a in _PortalContext.HR_Master_Designation
                                from b in _PortalContext.Institute
                                from c in _PortalContext.HR_Master_Employee_DMO
                                where a.MI_Id == b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && hrmeid.Contains(c.HRME_Id)
                                select new IVRM_NoticeBoardDTO
                                {
                                    HRMDES_Id = a.HRMDES_Id,
                                    HRMDES_DesignationName = a.HRMDES_DesignationName,
                                    MI_Name = b.MI_Subdomain
                                }).ToArray();
                    if (desg.Length > 0)
                    {
                        data.editdesignation = desg;
                    }


                    List<long> hrmdesid = new List<long>();
                    foreach (var item in desg)
                    {
                        hrmdesid.Add(item.HRMDES_Id);
                    }
                    var dept = (from a in _PortalContext.HR_Master_DepartmentCode_DMO
                                from b in _PortalContext.HR_Master_Department
                                from c in _PortalContext.HR_Master_Designation
                                where a.HRMDC_ID == b.HRMDC_ID && c.HRMDC_ID == a.HRMDC_ID && b.MI_Id == data.MI_Id && hrmdesid.Contains(c.HRMDES_Id)
                                select new IVRM_NoticeBoardDTO
                                {
                                    HRMDC_Name = a.HRMDC_Name,
                                    HRMDC_ID = a.HRMDC_ID
                                }).ToArray();
                    if (dept.Length > 0)
                    {
                        data.editdepartment = dept;
                    }


                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_DepartmentList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@role",
          SqlDbType.VarChar)
                        {
                            Value = "a"
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
    SqlDbType.BigInt)
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
                            data.departmentList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    string departments = "0";
                    if (dept.Length > 0)
                    {
                        foreach (var ue in dept)
                        {
                            departments = departments + "," + ue.HRMDC_ID;
                        }
                    }
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_DepartmentChange";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@departments",
                        SqlDbType.VarChar)
                        {
                            Value = departments
                        });

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.designation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    string desigids = "0";
                    if (desg.Length > 0)
                    {
                        foreach (var ue in desg)
                        {
                            desigids = desigids + "," + ue.HRMDES_Id;
                        }

                    }
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DesignationChange";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@departments",
                        SqlDbType.VarChar)
                        {
                            Value = desigids
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
                            data.get_userEmplist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_NoticeBoardDTO deactivate(IVRM_NoticeBoardDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_NoticeBoardDMO.Single(s => s.MI_Id == data.MI_Id && s.INTB_Id == data.INTB_Id);

                if (query.INTB_ActiveFlag == true)
                {
                    query.INTB_ActiveFlag = false;
                }
                else
                {
                    query.INTB_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PortalContext.Update(query);
                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_NoticeBoardDTO getstudent(IVRM_NoticeBoardDTO data)
        {
            try
            {
                //added by roopa//
                string asmcl_ids = "0", asmsid = "0", fmg_ids = "0", fmt_ids = "0", flag = "", trmr_ids = "0";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    if (data.classlsttwo.Length > 0)
                    {
                        foreach (var ue in data.classlsttwo)
                        {
                            asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }

                if (data.sectionlistarray != null && data.sectionlistarray.Length > 0)
                {
                    foreach (var item in data.sectionlistarray)
                    {
                        asmsid = asmsid + "," + item.ASMS_Id;
                    }
                }
                if (data.fee_def == true)
                {
                    flag = "F";
                }
                else if(data.routeflag == true)
                {
                    flag = "R";
                }
                else
                {
                    flag = "S";
                }
                if (data.fee_def == true)
                {

                    if (data.defgrparray != null && data.defgrparray.Length > 0)
                    {
                        foreach (var item in data.defgrparray)
                        {
                            fmg_ids = fmg_ids + "," + item.FMG_Id;
                        }
                    }

                    if (data.defarray != null && data.defarray.Length > 0)
                    {
                        foreach (var item in data.defarray)
                        {
                            fmt_ids = fmt_ids + "," + item.FMT_Id;
                        }
                    }
                }

                if (data.routeflag == true)
                {

                    if (data.routearray != null && data.routearray.Length > 0)
                    {
                        foreach (var item in data.routearray)
                        {
                            trmr_ids = trmr_ids + "," + item.TRMR_Id;
                        }
                    }
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FEE_TERMS_STUDENTS_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                    {
                        // Value = dto.ASMCL_Id
                        Value = asmcl_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                 SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                 SqlDbType.VarChar)
                    {
                        Value = fmt_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMR_Id",
                 SqlDbType.VarChar)
                    {
                        Value = trmr_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = flag
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
                        data.studentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //else
                //{
                //    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "NoticeBoard_Student";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //      SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });

                //        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //      SqlDbType.VarChar)
                //        {
                //            // Value = dto.ASMCL_Id
                //            Value = asmcl_ids
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //     SqlDbType.BigInt)
                //        {
                //            Value = data.ASMAY_Id
                //        });

                //        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                //        SqlDbType.VarChar)
                //        {
                //            Value = asmsid
                //        });
                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();
                //        var retObject = new List<dynamic>();
                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //                while (dataReader.Read())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                        );
                //                    }
                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            data.studentlist = retObject.ToArray();
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }
                //}

                //old code//
                //List<long> asmsid = new List<long>();
                //foreach (var item in dto.sectionlistarray)
                //{
                //    asmsid.Add(item.ASMS_Id);
                //}

                //var student = (from a in _PortalContext.School_Adm_Y_StudentDMO
                //               from b in _PortalContext.Adm_M_Student
                //               from c in _PortalContext.School_M_Section
                //               where a.AMST_Id == b.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && asmsid.Contains(a.ASMS_Id) && a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMS_Id == c.ASMS_Id
                //               select new IVRM_NoticeBoardDTO
                //               {
                //                   studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName) + ":" + c.ASMC_SectionName,
                //                   AMST_Id = b.AMST_Id,
                //                   ASMS_Id = c.ASMS_Id

                //               }).Distinct().ToArray();
                //if (student.Length > 0)
                //{
                //    dto.studentlist = student;
                //}
                //old code//
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_NoticeBoardDTO Deptselectiondetails(IVRM_NoticeBoardDTO dto)
        {
            try
            {
                string departments = "0";
                if (dto.departmentlist.Length > 0)
                {
                    foreach (var ue in dto.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_DepartmentChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
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
                        dto.designation = retObject.ToArray();
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
            return dto;
        }

        public IVRM_NoticeBoardDTO Desgselectiondetails(IVRM_NoticeBoardDTO dto)
        {
            try
            {
                string departments = "0";
                string designations = "0";
                if (dto.designationlist.Length > 0)
                {
                    foreach (var ue in dto.designationlist)
                    {
                        designations = designations + "," + ue.HRMDES_Id;
                    }

                }
               
                if (dto.departmentlist.Length > 0)
                {
                    foreach (var ue in dto.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }

                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DesignationChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
                    });
                    cmd.Parameters.Add(new SqlParameter("@designations",
                   SqlDbType.VarChar)
                    {
                        Value = designations
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
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
                        dto.get_userEmplist = retObject.ToArray();
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
            return dto;
        }

        public IVRM_NoticeBoardDTO viewData(IVRM_NoticeBoardDTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _PortalContext.IVRM_NoticeBoardDMO
                                       from b in _PortalContext.IVRM_NoticeBoard_FilesDMO_con
                                       where a.INTB_Id == dto.INTB_Id && a.INTB_Id == b.INTB_Id
                                       select new IVRM_NoticeBoardDTO
                                       {
                                           INTBFL_FileName = b.INTBFL_FileName,
                                           INTBFL_FilePath = b.INTBFL_FilePath,
                                           INTB_Attachment = a.INTB_Attachment,
                                           INTB_Id = a.INTB_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        public IVRM_NoticeBoardDTO viewrecords(IVRM_NoticeBoardDTO dto)
        {

            dto.notice_Name = dto.INTB_Title;

            if (dto.INTB_ToStaffFlg == true)
            {
                dto.flag = "Staff"; 

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NoticeView_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.INTB_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag
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
                        dto.viewlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NoticeNotView_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.INTB_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag
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
                        dto.notViewlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (dto.INTB_ToStudentFlg == true)
            {
                dto.flag1 = "Student";
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NoticeView_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.INTB_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag1
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
                        dto.stuviewlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NoticeNotView_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                 SqlDbType.BigInt)
                    {
                        Value = dto.INTB_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = dto.flag1
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
                        dto.stuNotViewlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }            

            return dto;
        }
    }
}
