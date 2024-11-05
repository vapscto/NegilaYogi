using AlumniHub.Com.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace AlumniHub.Com.Service
{
    public class SchoolALUDASHImpl : SchoolALUDASHInterface
    {
        private static ConcurrentDictionary<string, AlumniStudentDTO> _login =
     new ConcurrentDictionary<string, AlumniStudentDTO>();

        public AlumniContext _AlumniContext;
        private readonly DomainModelMsSqlServerContext _db;
        public SchoolALUDASHImpl(AlumniContext AlumniContext, DomainModelMsSqlServerContext db)
        {
            _AlumniContext = AlumniContext;
            _db = db;
        }
        public AlumniStudentDTO getloaddata(AlumniStudentDTO dto)
        {
            try
            {
                string rolename = _AlumniContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == dto.roleId).IVRMRT_Role;

                dto.rolename = rolename;

                long ALMST_Id = 0;

                if (dto.rolename== "Alumni")
                {
                    var uid = _AlumniContext.Alumni_User_LoginDMO.Where(a => a.IVRMUL_Id == dto.userid).ToList();

                    //var yid = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.ALSREG_Id == uid[0].ALSREG_Id && a.MI_Id == dto.MI_Id).ToList();
                    var yid = (from a in _AlumniContext.AlumniUserRegistrationDMO
                               from b in _AlumniContext.Alumni_M_StudentDMO
                               where (a.ALMST_Id == b.ALMST_Id && a.ALSREG_Id == uid[0].ALSREG_Id)
                               select new AlumniStudentDTO
                               {
                                   ALMST_Id =Convert.ToInt64(a.ALMST_Id),
                                   ASMAY_Id_Left = b.ASMAY_Id_Left
                               }).ToList();

                    dto.birthdaylist = _db.Alumni_M_StudentDMO.Where(t => t.ALMST_DOB.Date.Day == DateTime.Today.Day && t.ALMST_DOB.Date.Month == DateTime.Today.Month && t.ALMST_ActiveFlag == true && t.MI_Id == dto.MI_Id && t.ASMAY_Id_Left == yid[0].ASMAY_Id_Left).Distinct().ToArray();

                    var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                                   from b in _AlumniContext.Alumni_User_LoginDMO
                                   where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == dto.userid)
                                   select new AlumniStudentDTO
                                   {
                                       ALMST_Id = Convert.ToInt64(a.ALMST_Id),
                                       ALSREG_Id = b.ALSREG_Id
                                   }).ToList();

                    ALMST_Id = details[0].ALMST_Id;

                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AlumniRequestList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.BigInt) { Value = details[0].ALMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Request" });

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
                            dto.friendrequestlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AlumniDonationAmount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ALSREG_Id", SqlDbType.BigInt) { Value = details[0].ALSREG_Id });

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
                            dto.totaldonation = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                #region  CALENDER 
                try
                {
                    int month = DateTime.Now.Month;
                    dto.calenderlist = (from m in _AlumniContext.COE_Master_EventsDMO
                                        from n in _AlumniContext.COE_EventsDMO
                                        where (m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id  && n.COEE_AlumniFlag==true && n.COEE_EStartDate.Value.Month == month)
                                        select new AlumniStudentDTO
                                        {
                                            COEME_EventName = m.COEME_EventName,
                                            COEME_EventDesc = m.COEME_EventDesc,
                                            COEE_EStartDate = n.COEE_EStartDate,
                                            COEE_EEndDate = n.COEE_EEndDate,
                                            COEE_ReminderDate = n.COEE_ReminderDate
                                        }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniDashboard1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
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
                        dto.batch = retObject.ToArray();
                        if (dto.batch.Length > 0)
                        {
                            dto.count = dto.batch.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniDashboard2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
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
                        dto.alumnilist = retObject.ToArray();
                        if (dto.alumnilist.Length > 0)
                        {
                            dto.count = dto.alumnilist.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }               

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ALUMNI_BirthdayList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){ Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.VarChar){ Value = ALMST_Id });
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
                        dto.alumnibirthday = retObject.ToArray();

                        if (dto.alumnibirthday.Length > 0)
                        {
                            dto.count = dto.alumnibirthday.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
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
            return dto;
        }

        public AlumniStudentDTO yearwiselist(AlumniStudentDTO data)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniDashboardYearwise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
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
                        data.yearwiselist = retObject.ToArray();
                        if (data.yearwiselist.Length > 0)
                        {
                            data.count = data.yearwiselist.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO classwisestudent(AlumniStudentDTO data)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniDashboardClasswise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
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
                        data.classwiselist = retObject.ToArray();
                        if (data.classwiselist.Length > 0)
                        {
                            data.count = data.classwiselist.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO AluminiBirthday(AlumniStudentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var studentDetails = (from a in _db.Alumni_M_StudentDMO
                                      where (a.MI_Id == data.MI_Id && a.ALMST_DOB.Date.Day == indianTime.Date.Day && a.ALMST_DOB.Date.Month == indianTime.Date.Month)
                                      group a by a.ALMST_Id into g
                                      select new AlumniStudentDTO
                                      {
                                          ALMST_Id = g.FirstOrDefault().ALMST_Id,
                                          ALMST_FirstName = ((g.FirstOrDefault().ALMST_FirstName == null ? " " : g.FirstOrDefault().ALMST_FirstName) + (g.FirstOrDefault().ALMST_MiddleName == null ? " " : g.FirstOrDefault().ALMST_MiddleName) + (g.FirstOrDefault().ALMST_LastName == null ? " " : g.FirstOrDefault().ALMST_LastName)).Trim(),
                                          ALMST_emailId = g.FirstOrDefault().ALMST_emailId,
                                          ALMST_MobileNo = g.FirstOrDefault().ALMST_MobileNo
                                      }).Distinct().ToArray();

                if(studentDetails.Length>0)
                {
                    for(int i=0;i< studentDetails.Length;i++)
                    {
                        SMS sms = new SMS(_db);

                        var s = sms.sendSms(data.MI_Id,Convert.ToInt64(studentDetails[i].ALMST_MobileNo), "AlumniBirthday", studentDetails[i].ALMST_Id);

                        Email Email = new Email(_db);

                        string m = Email.sendmail(data.MI_Id, studentDetails[i].ALMST_emailId, "AlumniBirthday", studentDetails[i].ALMST_Id);
                    }
                }
                else
                {
                    data.returnval = "No Records";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO getgallery(AlumniStudentDTO data)
        {
            try
            {
                data.Alumnigallerygrid = _AlumniContext.Alumni_Gallery_DMO_con.Where(a => a.MI_Id == data.MI_Id).ToArray();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO viewgallery(AlumniStudentDTO data)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Alumni_GalleryFilesDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ALGA_Id", SqlDbType.BigInt) { Value = data.ALGA_Id });

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
                        data.viewgallery = retObject.ToArray();
                     
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO alumninotice(AlumniStudentDTO dto)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "alumni_noticeboardStudent";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });

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
                            dto.alumninoticeboardlist = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public AlumniStudentDTO viewnotice(AlumniStudentDTO dto)
        {
            try
            {
                dto.attachementlist = _AlumniContext.Alumni_NoticeBoard_Files_DMO_con.Where(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id && a.ALNTBFL_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
