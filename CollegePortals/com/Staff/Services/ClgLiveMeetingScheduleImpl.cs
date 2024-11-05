using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using DataAccessMsSqlServerProvider;
using System.Net.NetworkInformation;
using System.Net;
using DomainModel.Model.com.vapstech.Portals.Employee;

namespace CollegePortals.com.Staff.Services
{
    public class ClgLiveMeetingScheduleImpl : Interfaces.ClgLiveMeetingScheduleInterface
    {
        private static ConcurrentDictionary<string, ClgLiveMeetingScheduleDTO> _login =
           new ConcurrentDictionary<string, ClgLiveMeetingScheduleDTO>();
        private CollegeportalContext _ClgPortalContext;
        public DomainModelMsSqlServerContext _db;
        public ClgLiveMeetingScheduleImpl(CollegeportalContext ClgPortalContext, DomainModelMsSqlServerContext db)
        {
            _ClgPortalContext = ClgPortalContext;
            _db = db;
        }
        //SCHEDULE START
        public ClgLiveMeetingScheduleDTO getloaddata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                data.stafflist = (from a in _db.Staff_User_Login
                                  from b in _db.HR_Master_Employee_DMO
                                  where a.MI_Id == b.MI_Id && a.Emp_Code == b.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1 && b.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && b.HRME_Id != data.HRME_Id
                                  select new ClgLiveMeetingScheduleDTO
                                  {
                                      UserId = a.Id,
                                      HRME_Id = b.HRME_Id,
                                      IVRMSTAUL_UserName = a.IVRMSTAUL_UserName,
                                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) +" "+ (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                  }
                                ).Distinct().OrderBy(r => r.HRME_EmployeeFirstName).ToArray();



                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.UserId).ToList();

                var searchlog = _db.HOD_DMO.Where(a => a.HRME_Id == data.HRME_Id).ToList();

                data.academicList = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.subjlist = _db.MasterSubjectList.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).OrderBy(q => q.ISMS_SubjectName).ToArray();


                data.meetinglist = (from a in _db.LMS_Live_MeetingDMO
                                    from b in _db.HR_Master_Employee_DMO
                                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                                    select a
                                  ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ClgLiveMeetingScheduleDTO> getcoursedata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }


                var courselist = (from a in _db.MasterCourseDMO
                                  from b in _db.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                data.course_list = courselist.ToArray();
            
               
                //  using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                //  {
                //      cmd.CommandText = "CLG_PORTAL_STAFFWISE_COURSE";
                //      cmd.CommandType = CommandType.StoredProcedure;

                //      cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //  SqlDbType.BigInt)
                //      {
                //          Value = data.MI_Id
                //      });
                //      cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                // SqlDbType.BigInt)
                //      {
                //          Value = data.ASMAY_Id
                //      });
                //      cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //SqlDbType.BigInt)
                //      {
                //          Value = data.HRME_Id
                //      });


                //      if (cmd.Connection.State != ConnectionState.Open)
                //          cmd.Connection.Open();

                //      var retObject = new List<dynamic>();
                //      try
                //      {
                //          using (var dataReader = await cmd.ExecuteReaderAsync())
                //          {
                //              while (await dataReader.ReadAsync())
                //              {
                //                  var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                  for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                  {
                //                      dataRow.Add(
                //                          dataReader.GetName(iFiled),
                //                          dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                      );
                //                  }

                //                  retObject.Add((ExpandoObject)dataRow);
                //              }
                //          }
                //          data.course_list = retObject.ToArray();
                //      }
                //      catch (Exception ex)
                //      {
                //          Console.WriteLine(ex.Message);
                //      }
                //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> getbranchdata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {


                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                List<long> cdrid = new List<long>();
                if (data.selectedcourse_list !=null)
                {
                    foreach (var item in data.selectedcourse_list)
                    {
                        cdrid.Add(item.AMCO_Id);
                    }
                }

                var branchlist = (from a in _db.ClgMasterBranchDMO
                                  from b in _db.CLG_Adm_College_AY_CourseDMO
                                  from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && cdrid.Contains(b.AMCO_Id) && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select a).Distinct().ToList();
                data.branch_list = branchlist.OrderBy(t => t.AMB_Order).ToArray();

                //    data.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //    using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "CLG_PORTAL_STAFF_COURSEWISE_BRANCH";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //    SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //   SqlDbType.BigInt)
                //        {
                //            Value = data.ASMAY_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //  SqlDbType.BigInt)
                //        {
                //            Value = data.HRME_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                //SqlDbType.BigInt)
                //        {
                //            Value = data.AMCO_Id
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();
                //        try
                //        {
                //            using (var dataReader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await dataReader.ReadAsync())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                    }

                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            data.branch_list = retObject.ToArray();
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> getsemdata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                List<long> cdrid = new List<long>();
                if (data.selectedcourse_list != null)
                {
                    foreach (var item in data.selectedcourse_list)
                    {
                        cdrid.Add(item.AMCO_Id);
                    }
                }

                List<long> brid = new List<long>();
                if (data.selectedbranch != null)
                {
                    foreach (var item in data.selectedbranch)
                    {
                        brid.Add(item.AMB_Id);
                    }
                }

                var semisterlist = (from a in _db.CLG_Adm_Master_SemesterDMO
                                    from b in _db.CLG_Adm_College_AY_CourseDMO
                                    from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _db.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && cdrid.Contains(b.AMCO_Id)  && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && brid.Contains(c.AMB_Id) && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select  a).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

                //    string ids = "0";
                //    if (data.branchArray != null)
                //    {
                //        foreach (var b in data.branchArray)
                //        {
                //            ids = ids + "," + b.AMB_Id;
                //        }
                //    }
                //    data.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //    using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "CLG_PORTAL_STAFF_BRANCHWISE_SEMESTER";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //    SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //   SqlDbType.BigInt)
                //        {
                //            Value = data.ASMAY_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //  SqlDbType.BigInt)
                //        {
                //            Value = data.HRME_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                //SqlDbType.VarChar)
                //        {
                //            Value = ids
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();
                //        try
                //        {
                //            using (var dataReader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await dataReader.ReadAsync())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                    }

                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            data.sem_list = retObject.ToArray();
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> getsection(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                List<long> cdrid = new List<long>();
                if (data.selectedcourse_list != null)
                {
                    foreach (var item in data.selectedcourse_list)
                    {
                        cdrid.Add(item.AMCO_Id);
                    }
                }

                List<long> brid = new List<long>();
                if (data.selectedbranch != null)
                {
                    foreach (var item in data.selectedbranch)
                    {
                        brid.Add(item.AMB_Id);
                    }
                }

                List<long> smid = new List<long>();
                if (data.selectedsem != null)
                {
                    foreach (var item in data.selectedsem)
                    {
                        smid.Add(item.AMSE_Id);
                    }
                }

                var sectionlist = (from a in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                   from b in _ClgPortalContext.Adm_College_Master_SectionDMO
                                   where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && brid.Contains(a.AMB_Id) && cdrid.Contains(a.AMCO_Id) && smid.Contains(a.AMSE_Id )
                                   select new ClgLiveMeetingScheduleDTO
                                   {
                                       ACMS_Id = b.ACMS_Id,
                                       ACMS_SectionName = b.ACMS_SectionName,
                                       ACMS_SectionCode = b.ACMS_SectionCode,
                                       ACMS_Order = b.ACMS_Order
                                   }).Distinct().ToList();

                data.SectionList = sectionlist.OrderBy(t => t.ACMS_Order).ToArray();

                //    string ids = "0";
                //    if (data.branchArray != null)
                //    {
                //        foreach (var b in data.branchArray)
                //        {
                //            ids = ids + "," + b.AMB_Id;
                //        }
                //    }
                //    data.HRME_Id = _ClgPortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                //    using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "CLG_PORTAL_STAFF_BRANCHWISE_SEMESTER";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //    SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //   SqlDbType.BigInt)
                //        {
                //            Value = data.ASMAY_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                //  SqlDbType.BigInt)
                //        {
                //            Value = data.HRME_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                //SqlDbType.VarChar)
                //        {
                //            Value = ids
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();
                //        try
                //        {
                //            using (var dataReader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await dataReader.ReadAsync())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                    }

                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            data.sem_list = retObject.ToArray();
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> savedata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.LMSLMEET_Id == 0)
                {
                    if (data.HRME_Id == 0)
                    {
                        data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    string meetingid = Getmetingid(data);
                    //GET THE IP ADDRESS
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    String sMacAddress = string.Empty;
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (sMacAddress == String.Empty)// only return MAC Address from first card
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                        }
                    }
                    var remoteIpAddress = "";
                    //string netip = remoteIpAddress.ToString();

                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();

                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    IPAddress[] addr = ipEntry.AddressList;

                    remoteIpAddress = addr[addr.Length - 1].ToString();



                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    //  string myIP1 = ip_list.ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

                    LMS_Live_MeetingDMO obj = new LMS_Live_MeetingDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.LMSLMEET_MeetingId = meetingid;
                    obj.LMSLMEET_PlannedDate = data.LMSLMEET_PlannedDate;
                    obj.LMSLMEET_PlannedEndTime = data.LMSLMEET_PlannedEndTime;
                    obj.LMSLMEET_PlannedStartTime = data.LMSLMEET_PlannedStartTime;
                    obj.LMSLMEET_MeetingTopic = data.LMSLMEET_MeetingTopic;
                    obj.LMSLMEET_PMACAddress = sMacAddress;
                    obj.LMSLMEET_PIPAddress = myIP1;
                    obj.User_Id = data.UserId;
                    obj.HRME_Id = data.HRME_Id;
                    obj.HRME_Id = data.HRME_Id;
                    obj.LMSLMEET_CreatedBy = data.UserId;
                    obj.LMSLMEET_UpdatedBy = data.UserId;
                    obj.LMSLMEET_CreatedDate = DateTime.Now;
                    obj.LMSLMEET_UpdatedDate = DateTime.Now;
                    obj.LMSLMEET_ActiveFlg = true;
                    _db.Add(obj);


                    if (data.studflag == true)
                    {
                        if (data.selectedcourse_list.Length > 0)
                        {
                            foreach (var cls in data.selectedcourse_list)
                            {
                                foreach (var br in data.selectedbranch)
                                {


                                    var branchlist = (from a in _db.ClgMasterBranchDMO
                                                      from b in _db.CLG_Adm_College_AY_CourseDMO
                                                      from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                                      where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == cls.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && a.AMB_Id == br.AMB_Id)
                                                      select a).Distinct().ToList();
                                    if (branchlist.Count > 0)
                                    {
                                        foreach (var sm in data.selectedsem)
                                        {

                                            var semisterlist = (from a in _db.CLG_Adm_Master_SemesterDMO
                                                                from b in _db.CLG_Adm_College_AY_CourseDMO
                                                                from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                                                from d in _db.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                                                where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == cls.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == br.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && a.AMSE_Id == sm.AMSE_Id)
                                                                select a).Distinct().ToList();
                                            if (semisterlist.Count > 0)
                                            {
                                                foreach (var sec in data.secids)
                                                {
                                                    var sectionlist = (from a in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                                                                      from b in _ClgPortalContext.Adm_College_Master_SectionDMO
                                                                                      where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id==br.AMB_Id && a.AMCO_Id==cls.AMCO_Id && a.AMSE_Id==sm.AMSE_Id && a.ACMS_Id==sec.ACMS_Id
                                                                                      select new ClgLiveMeetingScheduleDTO
                                                                                      {
                                                                                          ACMS_Id = b.ACMS_Id,
                                                                                          ACMS_SectionName = b.ACMS_SectionName,
                                                                                          ACMS_SectionCode = b.ACMS_SectionCode,
                                                                                          ACMS_Order = b.ACMS_Order
                                                                                      }).Distinct().ToList();
                                                    if (sectionlist.Count>0)
                                                    {
                                                        foreach (var sub in data.subids)
                                                        {
                                                            LMS_Live_Meeting_CourseBranchDMO obj1 = new LMS_Live_Meeting_CourseBranchDMO();

                                                            obj1.LMSLMEET_Id = obj.LMSLMEET_Id;
                                                            obj1.ASMAY_Id = data.ASMAY_Id;
                                                            obj1.AMCO_Id = cls.AMCO_Id;
                                                            obj1.AMB_Id = br.AMB_Id;
                                                            obj1.AMSE_Id = sm.AMSE_Id;
                                                            obj1.ACMS_Id = sec.ACMS_Id;
                                                            obj1.ISMS_Id = sub.ISMS_Id;
                                                            obj1.LMSLMEETCOBR_ActiveFlg = true;
                                                            obj1.LMSLMEETCOBR_CreatedDate = DateTime.Now;
                                                            obj1.LMSLMEETCOBR_UpdatedDate = DateTime.Now;
                                                            obj1.LMSLMEETCOBR_CreatedBy = data.UserId;
                                                            obj1.LMSLMEETCOBR_UpdatedBy = data.UserId;

                                                            _db.Add(obj1);
                                                        }
                                                    }

                                                    
                                                    
                                                }
                                            }

                                        }

                                    }

                                }
                            }


                        }
                    }

                    if (data.stafflag == true)
                    {
                        if (data.stfids.Length > 0)
                        {
                            foreach (var item in data.stfids)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();

                                sobj.LMSLMEET_Id = obj.LMSLMEET_Id;

                                sobj.User_Id = item.UserId;
                                sobj.HRME_Id = item.HRME_Id;
                                sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                _db.Add(sobj);
                            }
                        }
                    }

                    int res = _db.SaveChanges();
                    if (res > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }




                }

                else
                {
                    if (data.HRME_Id == 0)
                    {
                        data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    string meetingid = Getmetingid(data);
                    //GET THE IP ADDRESS
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    String sMacAddress = string.Empty;
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (sMacAddress == String.Empty)// only return MAC Address from first card
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                        }
                    }
                    var remoteIpAddress = "";
                    //string netip = remoteIpAddress.ToString();

                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();

                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    IPAddress[] addr = ipEntry.AddressList;

                    remoteIpAddress = addr[addr.Length - 1].ToString();



                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    //  string myIP1 = ip_list.ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();


                    var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id);
                    ress.LMSLMEET_PlannedDate = data.LMSLMEET_PlannedDate;
                    ress.LMSLMEET_PlannedEndTime = data.LMSLMEET_PlannedEndTime;
                    ress.LMSLMEET_PlannedStartTime = data.LMSLMEET_PlannedStartTime;
                    ress.LMSLMEET_MeetingTopic = data.LMSLMEET_MeetingTopic;
                    ress.LMSLMEET_PMACAddress = sMacAddress;
                    ress.LMSLMEET_PIPAddress = myIP1;
                    ress.User_Id = data.UserId;
                    ress.HRME_Id = data.HRME_Id;
                    ress.LMSLMEET_UpdatedBy = data.UserId;
                    ress.LMSLMEET_UpdatedDate = DateTime.Now;
                    _db.Update(ress);

                    var remove = _db.LMS_Live_Meeting_CourseBranchDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                    if (remove.Count > 0)
                    {
                        foreach (var item in remove)
                        {
                            _db.Remove(item);
                        }
                    }

                    var removestf = _db.LMS_Live_Meeting_StaffOthersDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                    if (removestf.Count > 0)
                    {
                        foreach (var item in removestf)
                        {
                            _db.Remove(item);
                        }
                    }


                    if (data.studflag == true)
                    {
                        if (data.selectedcourse_list.Length > 0)
                        {
                            foreach (var cls in data.selectedcourse_list)
                            {
                                foreach (var br in data.selectedbranch)
                                {


                                    var branchlist = (from a in _db.ClgMasterBranchDMO
                                                      from b in _db.CLG_Adm_College_AY_CourseDMO
                                                      from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                                      where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id==cls.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && a.AMB_Id==br.AMB_Id)
                                                      select a).Distinct().ToList();
                                    if (branchlist.Count>0)
                                    {
                                        foreach (var sm in data.selectedsem)
                                        {

                                            var semisterlist = (from a in _db.CLG_Adm_Master_SemesterDMO
                                                                from b in _db.CLG_Adm_College_AY_CourseDMO
                                                                from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                                                from d in _db.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                                                where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id==cls.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id==br.AMB_Id  && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && a.AMSE_Id==sm.AMSE_Id)
                                                                select a).Distinct().ToList();
                                            if (semisterlist.Count>0)
                                            {
                                                foreach (var sec in data.secids)
                                                {
                                                    var sectionlist = (from a in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                                                       from b in _ClgPortalContext.Adm_College_Master_SectionDMO
                                                                       where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == br.AMB_Id && a.AMCO_Id == cls.AMCO_Id && a.AMSE_Id == sm.AMSE_Id && a.ACMS_Id == sec.ACMS_Id
                                                                       select new ClgLiveMeetingScheduleDTO
                                                                       {
                                                                           ACMS_Id = b.ACMS_Id,
                                                                           ACMS_SectionName = b.ACMS_SectionName,
                                                                           ACMS_SectionCode = b.ACMS_SectionCode,
                                                                           ACMS_Order = b.ACMS_Order
                                                                       }).Distinct().ToList();
                                                    if (sectionlist.Count > 0)
                                                    {

                                                        foreach (var sub in data.subids)
                                                    {
                                                        LMS_Live_Meeting_CourseBranchDMO obj1 = new LMS_Live_Meeting_CourseBranchDMO();

                                                        obj1.LMSLMEET_Id = data.LMSLMEET_Id;
                                                        obj1.ASMAY_Id = data.ASMAY_Id;
                                                        obj1.AMCO_Id = cls.AMCO_Id;
                                                        obj1.AMB_Id = br.AMB_Id;
                                                        obj1.AMSE_Id = sm.AMSE_Id;
                                                        obj1.ACMS_Id = sec.ACMS_Id;
                                                        obj1.ISMS_Id = sub.ISMS_Id;
                                                        obj1.LMSLMEETCOBR_ActiveFlg = true;
                                                        obj1.LMSLMEETCOBR_CreatedDate = DateTime.Now;
                                                        obj1.LMSLMEETCOBR_UpdatedDate = DateTime.Now;
                                                        obj1.LMSLMEETCOBR_CreatedBy = data.UserId;
                                                        obj1.LMSLMEETCOBR_UpdatedBy = data.UserId;

                                                        _db.Add(obj1);
                                                    }
                                                    }



                                                }
                                            }
                                            
                                        }

                                        }
                                    
                            }
                            }


                        }
                    }

                    if (data.stafflag == true)
                    {
                        if (data.stfids.Length > 0)
                        {
                            foreach (var item in data.stfids)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();

                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;

                                sobj.User_Id = item.UserId;
                                sobj.HRME_Id = item.HRME_Id;
                                sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                _db.Add(sobj);
                            }
                        }
                    }

                    int res = _db.SaveChanges();
                    if (res > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgLiveMeetingScheduleDTO editdata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                data.editlist = _db.LMS_Live_MeetingDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToArray();

                var details = _db.LMS_Live_Meeting_CourseBranchDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();

                data.Emp_punchDetails = details.ToArray();

                if (details.Count > 0)
                {
                    data.studflag = true;

                    var courselist = (from a in _db.MasterCourseDMO
                                      from b in _db.CLG_Adm_College_AY_CourseDMO
                                      where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == details[0].ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                      select a).Distinct().OrderBy(t => t.AMCO_Order).ToList();
                    data.course_list = courselist.ToArray();


                    if (courselist.Count>0)
                    {

                        var branchlist = (from a in _db.ClgMasterBranchDMO
                                          from b in _db.CLG_Adm_College_AY_CourseDMO
                                          from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                          from d in _db.LMS_Live_Meeting_CourseBranchDMO
                                          where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == details[0].ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id==d.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag &&  d.LMSLMEET_Id == data.LMSLMEET_Id && c.AMB_Id==d.AMB_Id )
                                          select a).Distinct().ToList();
                        data.branch_list = branchlist.OrderBy(t => t.AMB_Order).ToArray();


                        var semisterlist = (from a in _db.CLG_Adm_Master_SemesterDMO
                                            from b in _db.CLG_Adm_College_AY_CourseDMO
                                            from c in _db.CLG_Adm_College_AY_Course_BranchDMO
                                            from d in _db.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                            from e in _db.LMS_Live_Meeting_CourseBranchDMO
                                            where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == details[0].ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id==e.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id==e.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && e.LMSLMEET_Id == data.LMSLMEET_Id)
                                            select a).Distinct().ToList();
                        data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();



                        data.SectionList = (from a in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                            from b in _ClgPortalContext.Adm_College_Master_SectionDMO
                                           // from e in _db.LMS_Live_Meeting_CourseBranchDMO
                                            where a.ASMAY_Id == details[0].ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id 
                                            //&& a.AMB_Id==e.AMB_Id && a.AMCO_Id==e.AMCO_Id && a.AMSE_Id==e.AMSE_Id && e.LMSLMEET_Id == data.LMSLMEET_Id
                                            select new ClgLiveMeetingScheduleDTO
                                            {
                                                ACMS_Id = b.ACMS_Id,
                                                ACMS_SectionName = b.ACMS_SectionName,
                                                ACMS_SectionCode = b.ACMS_SectionCode,
                                                ACMS_Order = b.ACMS_Order
                                            }).Distinct().ToArray();

                    }




                }
                else
                {
                    data.studflag = false;
                }



                var stfdetails = _db.LMS_Live_Meeting_StaffOthersDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();

                data.empdetails = stfdetails.ToArray();

                if (stfdetails.Count > 0)
                {
                    data.stafflag = true;
                }
                else
                {
                    data.stafflag = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> deactive (ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                var activelist = _db.LMS_Live_MeetingDMO.Single(w => w.LMSLMEET_Id == data.LMSLMEET_Id);
                var classlist = _db.LMS_Live_Meeting_CourseBranchDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                var stafflist = _db.LMS_Live_Meeting_StaffOthersDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();

                if (activelist.LMSLMEET_ActiveFlg == true)
                {
                    activelist.LMSLMEET_ActiveFlg = false;


                    if (classlist.Count > 0)
                    {
                        foreach (var item in classlist)
                        {
                            item.LMSLMEETCOBR_ActiveFlg = false;
                            item.LMSLMEETCOBR_UpdatedBy = data.UserId;
                            item.LMSLMEETCOBR_UpdatedDate = DateTime.Now;

                            _db.Update(item);
                        }
                    }
                    if (stafflist.Count > 0)
                    {
                        foreach (var item1 in stafflist)
                        {
                            item1.LMSLMEETSTFOTH_ActiveFlg = false;
                            item1.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                            item1.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;

                            _db.Update(item1);
                        }
                    }

                }
                else
                {
                    activelist.LMSLMEET_ActiveFlg = true;
                    if (classlist.Count > 0)
                    {
                        foreach (var item in classlist)
                        {
                            item.LMSLMEETCOBR_ActiveFlg = true;
                            item.LMSLMEETCOBR_UpdatedBy = data.UserId;
                            item.LMSLMEETCOBR_UpdatedDate = DateTime.Now;
                            _db.Update(item);
                        }
                    }


                    if (stafflist.Count > 0)
                    {
                        foreach (var item1 in stafflist)
                        {
                            item1.LMSLMEETSTFOTH_ActiveFlg = true;
                            item1.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                            item1.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;

                            _db.Update(item1);
                        }
                    }
                }

                activelist.LMSLMEET_UpdatedBy = data.UserId;
                activelist.LMSLMEET_UpdatedDate = DateTime.Now;

                _db.Update(activelist);

                int res = _db.SaveChanges();
                if (res > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }

        public String Getmetingid(ClgLiveMeetingScheduleDTO data)
        {
            string meetingid = "";
            try
            {

                var MeetingId = "";

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                var EmpCode = _db.HR_Master_Employee_DMO.Single(e => e.HRME_Id == data.HRME_Id && e.MI_Id == data.MI_Id).HRME_EmployeeCode;
                if (EmpCode != null && EmpCode != "")
                {
                    MeetingId = EmpCode;
                }
                var date = String.Format("{0:m}", data.LMSLMEET_PlannedDate);

                if (data.selectedcourse_list.Length > 0)
                {
                    var cnt = 0;
                    foreach (var item in data.selectedcourse_list)
                    {
                        if (item.AMCO_CourseCode != null && item.AMCO_CourseCode != "")
                        {

                            if (cnt == 0)
                            {
                                if (MeetingId != "")
                                {
                                    MeetingId = MeetingId + "-" + item.AMCO_CourseCode;
                                }
                                else
                                {
                                    MeetingId = item.AMCO_CourseCode;
                                }

                                cnt += 1;
                            }


                        }
                    }



                }


                if (data.selectedbranch.Length > 0)
                {
                    var cnt = 0;
                    foreach (var item in data.selectedbranch)
                    {
                        if (item.AMB_BranchCode != null && item.AMB_BranchCode != "")
                        {

                            if (cnt == 0)
                            {
                                if (MeetingId != "")
                                {
                                    MeetingId = MeetingId + "-" + item.AMB_BranchCode;
                                }
                                else
                                {
                                    MeetingId = item.AMB_BranchCode;
                                }

                                cnt += 1;
                            }


                        }
                    }



                }

                if (data.subids.Length > 0)
                {
                    var cnt = 0;
                    foreach (var item in data.subids)
                    {
                        if (item.ISMS_SubjectCode != null && item.ISMS_SubjectCode != "")
                        {

                            if (cnt == 0)
                            {
                                if (MeetingId != "")
                                {
                                    MeetingId = MeetingId + "-" + item.ISMS_SubjectCode;
                                }
                                else
                                {
                                    MeetingId = item.ISMS_SubjectCode;
                                }

                                cnt += 1;
                            }


                        }
                    }

                }

                if (date != null && date != "")
                {
                    if (MeetingId != "")
                    {
                        MeetingId = MeetingId + "-" + date;
                    }
                    else
                    {
                        MeetingId = date;
                    }
                }

                var Meetinglist = _db.LMS_Live_MeetingDMO.OrderByDescending(e => e.LMSLMEET_Id).Take(1).ToList();
                if (Meetinglist.Count > 0)
                {
                    var id = Meetinglist[0].LMSLMEET_Id + 1;
                    if (MeetingId != "")
                    {

                        MeetingId = MeetingId + "-" + id;
                    }
                    else
                    {
                        MeetingId = id.ToString();
                    }
                }
                else
                {
                    if (MeetingId != "")
                    {
                        MeetingId = MeetingId + "-" + "1";
                    }
                    else
                    {
                        MeetingId = "1";
                    }
                }


                meetingid = MeetingId.Replace(" ", String.Empty);

            }
            catch (Exception ex)
            {

                throw;
            }

            return meetingid;
        }
        //SCHEDULE END

        //STAFF PROFILE START
        public async Task<ClgLiveMeetingScheduleDTO> getempdetails(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");
                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }

                data.stafflist = (from a in _db.HR_Master_Employee_DMO
                                  from c in _db.HR_Master_Designation
                                  from d in _db.HR_Master_Department
                                  from e in _db.IVRM_Master_Gender
                                  where (a.HRMDES_Id == c.HRMDES_Id && a.HRMD_Id == d.HRMD_Id && a.IVRMMG_Id == e.IVRMMG_Id && a.MI_Id.Equals(data.MI_Id) && a.HRME_Id == data.HRME_Id)
                                  select new ClgLiveMeetingScheduleDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      MI_Id = a.MI_Id,
                                      HRMD_DepartmentName = d.HRMD_DepartmentName,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                      HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_BiometricCode = a.HRME_BiometricCode,
                                      HRME_PerStreet = a.HRME_PerStreet,
                                      HRME_PerArea = a.HRME_PerArea,
                                      HRME_PerCity = a.HRME_PerCity,
                                      HRME_PerStateId = a.HRME_PerStateId,
                                      HRME_PerCountryId = a.HRME_PerCountryId,
                                      HRME_PerPincode = a.HRME_PerPincode,
                                      HRME_LocStreet = a.HRME_LocStreet,
                                      HRME_LocArea = a.HRME_LocArea,
                                      HRME_LocCity = a.HRME_LocCity,
                                      HRME_LocStateId = a.HRME_LocStateId,
                                      HRME_LocCountryId = a.HRME_LocCountryId,
                                      HRME_LocPincode = a.HRME_LocPincode,
                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = e.IVRMMG_GenderName,
                                      CasteId = a.CasteId,

                                      HRME_FatherName = a.HRME_FatherName,
                                      HRME_MotherName = a.HRME_MotherName,
                                      HRME_DOB = a.HRME_DOB,
                                      HRME_DOJ = a.HRME_DOJ,
                                      HRME_ExpectedRetirementDate = a.HRME_ExpectedRetirementDate,
                                      HRME_PFDate = a.HRME_PFDate,
                                      HRME_ESIDate = a.HRME_ESIDate,
                                      HRME_MobileNo = a.HRME_Id,
                                      HRME_EmailId = a.HRME_EmailId,
                                      HRME_BloodGroup = a.HRME_BloodGroup,

                                      HRME_Photo = a.HRME_Photo,

                                  }).Distinct().ToArray();



                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.UserId).ToList();

                var searchlog = _db.HOD_DMO.Where(a => a.HRME_Id == data.HRME_Id).ToList();




                data.meetinglist = (from a in _db.LMS_Live_MeetingDMO
                                    from b in _db.HR_Master_Employee_DMO
                                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && a.LMSLMEET_PlannedDate == indianTime.Date && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_ActiveFlg == true
                                    select a
                                  ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();



                data.joinmeetinglist = (from a in _db.LMS_Live_MeetingDMO
                                        from b in _db.HR_Master_Employee_DMO
                                        from c in _db.LMS_Live_Meeting_StaffOthersDMO
                                        where a.HRME_Id == b.HRME_Id && c.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && (a.LMSLMEET_PlannedDate == indianTime.Date || a.LMSLMEET_MeetingDate == indianTime.Date) && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_Id == c.LMSLMEET_Id && a.LMSLMEET_ActiveFlg == true && (c.LMSLMEETSTFOTH_LogoutTime == null || c.LMSLMEETSTFOTH_LogoutTime == "")
                                        select new ClgLiveMeetingScheduleDTO
                                        {
                                            HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = b.HRME_Id,
                                            LMSLMEET_Id = a.LMSLMEET_Id,
                                            LMSLMEET_PlannedDate = a.LMSLMEET_PlannedDate,
                                            LMSLMEET_PlannedStartTime = a.LMSLMEET_PlannedStartTime,
                                            LMSLMEET_PlannedEndTime = a.LMSLMEET_PlannedEndTime,
                                            LMSLMEET_MeetingDate = a.LMSLMEET_MeetingDate,
                                            LMSLMEET_EndTime = a.LMSLMEET_EndTime,
                                            LMSLMEET_StartedTime = a.LMSLMEET_StartedTime,
                                            LMSLMEET_MeetingId = a.LMSLMEET_MeetingId,
                                            LMSLMEET_MeetingTopic = a.LMSLMEET_MeetingTopic,
                                        }
                                 ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();




            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public async Task<ClgLiveMeetingScheduleDTO> ondatechange(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }


                data.meetinglist = (from a in _db.LMS_Live_MeetingDMO
                                    from b in _db.HR_Master_Employee_DMO
                                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && a.LMSLMEET_PlannedDate == data.LMSLMEET_PlannedDate.Date && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_ActiveFlg == true
                                    select a
                                  ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();




                data.joinmeetinglist = (from a in _db.LMS_Live_MeetingDMO
                                        from b in _db.HR_Master_Employee_DMO
                                        from c in _db.LMS_Live_Meeting_StaffOthersDMO
                                        where a.HRME_Id == b.HRME_Id && c.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && (a.LMSLMEET_PlannedDate == data.LMSLMEET_PlannedDate.Date || a.LMSLMEET_MeetingDate == data.LMSLMEET_PlannedDate.Date) && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_Id == c.LMSLMEET_Id && a.LMSLMEET_ActiveFlg == true
                                        select new ClgLiveMeetingScheduleDTO
                                        {
                                            HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = b.HRME_Id,
                                            LMSLMEET_Id = a.LMSLMEET_Id,
                                            LMSLMEET_PlannedDate = a.LMSLMEET_PlannedDate,
                                            LMSLMEET_PlannedStartTime = a.LMSLMEET_PlannedStartTime,
                                            LMSLMEET_PlannedEndTime = a.LMSLMEET_PlannedEndTime,
                                            LMSLMEET_MeetingDate = a.LMSLMEET_MeetingDate,
                                            LMSLMEET_EndTime = a.LMSLMEET_EndTime,
                                            LMSLMEET_StartedTime = a.LMSLMEET_StartedTime
                                        }
                                  ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();




            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> onstartmeeting(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");



                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();
                var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id);
                ress.LMSLMEET_MeetingDate = indianTime.Date;
                ress.LMSLMEET_StartedTime = time;

                ress.LMSLMEET_MACAddress = sMacAddress;
                ress.LMSLMEET_IPAddress = myIP1;
                ress.LMSLMEET_UpdatedBy = data.UserId;
                ress.LMSLMEET_UpdatedDate = DateTime.Now;
                _db.Update(ress);
                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }



            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> endmainmeeting(ClgLiveMeetingScheduleDTO data)
        {
            try
            {


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                if (data.HRML_LeaveType == "M")
                {
                    var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id);
                    ress.LMSLMEET_EndTime = time;
                    ress.LMSLMEET_MACAddress = sMacAddress;
                    ress.LMSLMEET_IPAddress = myIP1;
                    ress.LMSLMEET_UpdatedBy = data.UserId;
                    ress.LMSLMEET_UpdatedDate = DateTime.Now;
                    _db.Update(ress);
                    int ss = _db.SaveChanges();
                    if (ss > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
                else if (data.HRML_LeaveType == "ST")
                {

                    if (data.HRME_Id == 0)
                    {
                        data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }

                    var ress = _db.LMS_Live_Meeting_StaffOthersDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.HRME_Id == data.HRME_Id);
                    ress.LMSLMEETSTFOTH_LogoutTime = time;
                    ress.LMSLMEETSTFOTH_MACAddress = sMacAddress;
                    ress.LMSLMEETSTFOTH_MACAddress = myIP1;
                    ress.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                    ress.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                    _db.Update(ress);
                    int ss = _db.SaveChanges();
                    if (ss > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }

                }


            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> joinmeeting(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");



                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                var ress = _db.LMS_Live_Meeting_StaffOthersDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.HRME_Id == data.HRME_Id);
                ress.LMSLMEETSTFOTH_LoginTime = time;
                ress.LMSLMEETSTFOTH_MACAddress = sMacAddress;
                ress.LMSLMEETSTFOTH_IPAddress = myIP1;
                ress.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                ress.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                ress.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                ress.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                _db.Update(ress);
                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //STAFF PROFILE END


        //STUDENT

        public async Task<ClgLiveMeetingScheduleDTO> getstudentdetails(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                data.fillstudentalldetails = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                                              from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                              where ( b.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_Id==b.AMCST_Id && a.MI_Id==data.MI_Id)
                                              select new ClgLiveMeetingScheduleDTO
                                              {

                                                  amst_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                                
                                                  amst_RegistrationNo = a.AMCST_RegistrationNo,
                                                  amst_AdmNo = a.AMCST_AdmNo,
                                                  amay_RollNo = b.ACYST_RollNo,
                                                  amst_sex = a.AMCST_Sex,
                                                  AMST_Photoname = a.AMCST_StudentPhoto
                                              }
                    ).Distinct().ToArray();


               

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);



                var joinedmeeting = (from a in _db.LMS_Live_Meeting_Student_CollegeDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && a.AMCST_Id == data.AMCST_Id && (a.LMSLMEETSTDCOL_LogoutTime == null || a.LMSLMEETSTDCOL_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == indianTime.Date || b.LMSLMEET_MeetingDate == indianTime.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();


                data.joinedmeeting = joinedmeeting.ToArray();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GETStudent_Meeting_Profile_College";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MeetingDate",
                    SqlDbType.Date)
                    {
                        Value = indianTime.Date.ToString("yyyy-MM-dd")
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
                        data.joinmeetinglist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> endmainmeetingstudent(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");



                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();


                var ress = _db.LMS_Live_Meeting_Student_CollegeDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.AMCST_Id == data.AMCST_Id).ToList();
                foreach (var item in ress)
                {
                    item.LMSLMEETSTDCOL_LogoutTime = time;
                    item.LMSLMEETSTDCOL_MACAddress = sMacAddress;
                    item.LMSLMEETSTDCOL_ISPAddress = myIP1;
                    //item.LMSLMEETSTDCOL_UpdatedBy = data.UserId;
                    item.LMSLMEETSTDCOL_UpdatedDate = DateTime.Now;
                    _db.Update(item);
                }



                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }




            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> joinmeetingStudent(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");



                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                LMS_Live_Meeting_Student_CollegeDMO ress = new LMS_Live_Meeting_Student_CollegeDMO();

                ress.AMCST_Id = data.AMCST_Id;
                ress.LMSLMEET_Id = data.LMSLMEET_Id;
                ress.LMSLMEETSTDCOL_LoginTime = time;
                ress.LMSLMEETSTDCOL_MACAddress = sMacAddress;
                ress.LMSLMEETSTDCOL_ISPAddress = myIP1;
                //ress.LMSLMEETSTDCOL_UpdatedBy = data.UserId;
               // ress.LMSLMEETSTDCOL_CreatedBy = data.UserId;
                ress.LMSLMEETSTDCOL_CreatedDate = DateTime.Now;
                ress.LMSLMEETSTDCOL_UpdatedDate = DateTime.Now;
                ress.LMSLMEETSTDCOL_ActiveFlg = true;
                _db.Add(ress);
                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<ClgLiveMeetingScheduleDTO> ondatechangestudent(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                var joinedmeeting = (from a in _db.LMS_Live_Meeting_Student_CollegeDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && a.AMCST_Id == data.AMCST_Id && (a.LMSLMEETSTDCOL_LogoutTime == null || a.LMSLMEETSTDCOL_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == data.LMSLMEET_PlannedDate.Date || b.LMSLMEET_MeetingDate == data.LMSLMEET_PlannedDate.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();


                data.joinedmeeting = joinedmeeting.ToArray();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GETStudent_Meeting_Profile_College";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MeetingDate",
                    SqlDbType.Date)
                    {
                        Value = data.LMSLMEET_PlannedDate.ToString("yyyy-MM-dd")
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
                        data.joinmeetinglist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        ///REPORT


        public async Task<ClgLiveMeetingScheduleDTO> getschrptdetails(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                if (data.RoleId > 0)
                {
                    var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                    if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                    {
                        if (data.HRME_Id == 0)
                        {
                            data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                        }

                        data.stafflist = (from a in _db.LMS_Live_MeetingDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                                          select new ClgLiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                                ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }
                    else
                    {
                        data.stafflist = (from a in _db.LMS_Live_MeetingDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id
                                          select new ClgLiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                              ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public async Task<ClgLiveMeetingScheduleDTO> getschedulereport(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                string stafids = "";

                if (data.stfids != null)
                {
                    var cnt = 0;
                    foreach (var item in data.stfids)
                    {
                        if (cnt == 0)
                        {
                            stafids = item.HRME_Id.ToString();
                        }
                        else
                        {
                            stafids = stafids + "," + item.HRME_Id.ToString();
                        }
                        cnt += 1;
                    }
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STAFF_MEETING_SCHEDULE_COURSE_REPORT_COLLEGE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Staffid",
                      SqlDbType.VarChar)
                    {
                        Value = stafids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = data.rtype
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
                        data.meetinglist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Meeting_Schedule_Staff_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Staffid",
                      SqlDbType.VarChar)
                    {
                        Value = stafids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = data.rtype
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
                        data.meetingliststaff = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public async Task<ClgLiveMeetingScheduleDTO> getschrptdetailsprofile(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                if (data.RoleId > 0)
                {
                    var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                    if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                    {
                        if (data.HRME_Id == 0)
                        {
                            data.HRME_Id = _db.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                        }

                        data.stafflist = (from a in _db.LMS_Live_Meeting_StaffOthersDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && b.MI_Id == data.MI_Id
                                          select new ClgLiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                                ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }
                    else
                    {
                        data.stafflist = (from a in _db.LMS_Live_Meeting_StaffOthersDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && b.MI_Id == data.MI_Id
                                          select new ClgLiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                              ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }



                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }

        public async Task<ClgLiveMeetingScheduleDTO> getstaffprofilereport(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                string stafids = "";

                if (data.stfids != null)
                {
                    var cnt = 0;
                    foreach (var item in data.stfids)
                    {
                        if (cnt == 0)
                        {
                            stafids = item.HRME_Id.ToString();
                        }
                        else
                        {
                            stafids = stafids + "," + item.HRME_Id.ToString();
                        }
                        cnt += 1;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STAFF_MEETING_PROFILE_STAFF_REPORT_COLLAGE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Staffid",
                      SqlDbType.VarChar)
                    {
                        Value = stafids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = data.rtype
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
                        data.meetingliststaff = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }

        public async Task<ClgLiveMeetingScheduleDTO> getstudentprofiledata(ClgLiveMeetingScheduleDTO data)
        {
            try
            {

                if (data.RoleId > 0)
                {
                    var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                    if (data.roletype.Equals("student", StringComparison.OrdinalIgnoreCase))
                    {
                        data.fillstudentalldetails = (from a in _db.Adm_Master_College_StudentDMO
                                                      from b in _db.Adm_College_Yearly_StudentDMO
                                                      from c in _db.LMS_Live_MeetingDMO
                                                      from d in _db.LMS_Live_Meeting_CourseBranchDMO
                                                      where (a.AMCST_Id == b.AMCST_Id && b.ASMAY_Id == d.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && b.AMB_Id == d.AMB_Id && c.LMSLMEET_Id == d.LMSLMEET_Id && c.LMSLMEET_Id == d.LMSLMEET_Id && b.AMCST_Id == data.AMCST_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id & b.AMSE_Id == d.AMSE_Id && b.ACMS_Id == d.ACMS_Id)
                                                      select new ClgLiveMeetingScheduleDTO
                                                      {
                                                          amst_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                                          Amst_Id = a.AMCST_Id,
                                                      }
                    ).Distinct().ToArray();

                        data.academicList = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();


                    }
                    else
                    {
                        //data.stafflist = (from a in _db.LMS_Live_MeetingDMO
                        //                  from b in _db.HR_Master_Employee_DMO
                        //                  where a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id
                        //                  select new LiveMeetingScheduleDTO
                        //                  {
                        //                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                        //                      HRME_Id = b.HRME_Id,

                        //                  }
                        //      ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
                return data;
            
            }

        public async Task<ClgLiveMeetingScheduleDTO> getstudentprofilereport(ClgLiveMeetingScheduleDTO data)
        {
            try
            {
                string stafids = "";

                if (data.stids != null)
                {
                    var cnt = 0;
                    foreach (var item in data.stids)
                    {
                        if (cnt == 0)
                        {
                            stafids = item.amst_Id.ToString();
                        }
                        else
                        {
                            stafids = stafids + "," + item.amst_Id.ToString();
                        }
                        cnt += 1;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Student_Meeting_Profile_Student_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@Studentid",
                      SqlDbType.VarChar)
                    {
                        Value = stafids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = data.rtype
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
                        data.meetingliststaff = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
            
            }



    }
}
