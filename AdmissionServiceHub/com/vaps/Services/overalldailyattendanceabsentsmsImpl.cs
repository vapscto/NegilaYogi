using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using System.Linq;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using CommonLibrary;
using DomainModel.Model.com.vapstech.admission;
using AutoMapper;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using System.Globalization;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class overalldailyattendanceabsentsmsImpl : Interfaces.overalldailyattendanceabsentsmsInterface
    {
        public StudentAttendanceReportContext _db;
        ILogger<overalldailyattendanceabsentsmsImpl> _log;
        private DomainModelMsSqlServerContext _dbsms;
        private readonly UserManager<ApplicationUser> _UserManager;


        public overalldailyattendanceabsentsmsImpl(StudentAttendanceReportContext db, ILogger<overalldailyattendanceabsentsmsImpl> loggerFactor, DomainModelMsSqlServerContext dbsms,
            UserManager<ApplicationUser> UserManager)
        {
            _db = db;
            _log = loggerFactor;
            _dbsms = dbsms;
            _UserManager = UserManager;
        }
        public async Task<OveralldailyattendanceabsentsmsDTO> getInitailData(OveralldailyattendanceabsentsmsDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d => d.MI_Id == data.miid && d.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToListAsync();
                data.academicList = allyear.ToArray();

                List<MasterAcademic> allyear1 = new List<MasterAcademic>();
                allyear1 = await _db.academicYear.Where(d => d.MI_Id == data.miid && d.Is_Active == true && d.ASMAY_Id == data.ASMAY_Id).ToListAsync();
                data.academicListdefault = allyear1.ToArray();

                // logo
                var cat = _db.GenConfig.Where(g => g.MI_Id == data.miid && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    data.category_list = _db.category.Where(f => f.MI_Id == data.miid && f.AMC_ActiveFlag == 1).ToArray();
                    data.categoryflag = true;
                }
                else
                {
                    data.categoryflag = false;
                }
             //  data.category_list = _db.category.Where(f => f.MI_Id == data.miid && f.AMC_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }
        public async Task<OveralldailyattendanceabsentsmsDTO> getserdata(OveralldailyattendanceabsentsmsDTO data)
        {
            int k = 0;
            var asmclid = "";
            var asmsid = "";
            var check_rolename = (from a in _db.MasterRoleType
                                  where (a.IVRMRT_Id == data.roleId)
                                  select new OveralldailyattendanceabsentsmsDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }).ToList();

            int UserId = GetUserId(data);

            var empcode_check = (from a in _db.Staff_User_Login
                                 where (a.MI_Id == data.miid && a.Id.Equals(UserId))
                                 select new OveralldailyattendanceabsentsmsDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("STAFF") || check_rolename.FirstOrDefault().rolename.Equals("Staff"))
            {
                k = 1;

                if (empcode_check.Count > 0)
                {
                    var classlist12 = (from a in _db.Adm_SchAttLoginUserClass
                                       from b in _db.Adm_SchAttLoginUser
                                       from c in _db.admissionClass
                                       where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                       && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                       && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMCL_ActiveFlag == true)
                                       select new OveralldailyattendanceabsentsmsDTO
                                       {
                                           ASMCL_Id = c.ASMCL_Id,
                                           ASMCL_className = c.ASMCL_ClassName,
                                       }
                               ).Distinct().ToList();


                    var SectionList12 = (from a in _db.Adm_SchAttLoginUserClass
                                         from b in _db.Adm_SchAttLoginUser
                                         from c in _db.masterSection
                                         where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                         && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                         && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                         && c.ASMC_ActiveFlag == 1)
                                         select new OveralldailyattendanceabsentsmsDTO
                                         {
                                             ASMS_Id = c.ASMS_Id,
                                             asmC_SectionName = c.ASMC_SectionName,
                                         }
                                        ).Distinct().ToList();

                    for (int i = 0; i < classlist12.Count; i++)
                    {

                        if (i == 0)
                        {
                            asmclid = classlist12[i].ASMCL_Id.ToString();
                        }
                        else
                        {
                            asmclid = asmclid + ',' + classlist12[i].ASMCL_Id.ToString();
                        }
                    }

                    for (int i = 0; i < SectionList12.Count; i++)
                    {

                        if (i == 0)
                        {
                            asmsid = SectionList12[i].ASMS_Id.ToString();
                        }
                        else
                        {
                            asmsid = asmsid + ',' + SectionList12[i].ASMS_Id.ToString();
                        }
                    }

                }
                else
                {
                    //   data.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }
            else
            {
                asmclid = "0";
                asmsid = "0";
            }
            var amcid = "0";
            if (data.AMC_Id > 0)
            {
                amcid = data.AMC_Id.ToString();

                data.AMC_logo = _db.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.miid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();


            }

            string fromdate = "";
            if (data.fromdate != null)
            {
                //  fromdate = data.fromdate.Value.ToString("yyyy-MM-dd");
                fromdate = data.fromdate.Value.ToString("dd-MM-yyyy");
               // DateTime fromdate = DateTime.ParseExact(data.fromdate.Value.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
               //cmd.CommandText = "OverallDailyAttendance_NEW1";
                cmd.CommandText = "OverallDailyAttendance_NEW1_att_type";
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                {
                    //Value = Convert.ToInt32(data.ASMAY_Id)
                    Value = data.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    //Value = data.fromdate
                    Value = fromdate
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar)
                {
                    Value = data.miid
                });

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                {
                    Value = k
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar)
                {
                    Value = asmclid
                });
                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar)
                {
                    Value = asmsid
                });
                cmd.Parameters.Add(new SqlParameter("@att_type", SqlDbType.VarChar)
                {
                    Value = data.attType
                });
                cmd.Parameters.Add(new SqlParameter("@AMC_id", SqlDbType.VarChar)
                {
                    Value = data.AMC_Id
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
                    data.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    _log.LogInformation("Error in Over All Daily Attendance :" + ex.Message);
                }
            }

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";


            fromdatecon = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
            //fromdatecon = Convert.ToDateTime((DateTime.Now.AddDays(-4)).ToString("yyyy-MM-dd"));

            //DateTime fromdatecon = DateTime.ParseExact(data.fromdate.Value.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //confromdate = fromdatecon.ToString();

            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
               // cmd.CommandText = "Adm_Absent_SMS_Email_Datewise";
              cmd.CommandText = "Adm_Absent_SMS_Email_Datewise_att_type";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });

                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar)
                {
                    Value = data.miid
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    Value = confromdate

                });
                cmd.Parameters.Add(new SqlParameter("@att_type", SqlDbType.VarChar)
                {
                    Value = data.attType
                });
                cmd.Parameters.Add(new SqlParameter("@AMC_id", SqlDbType.VarChar)
                {
                    Value = data.AMC_Id
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
                    data.student_teacherList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public OveralldailyattendanceabsentsmsDTO getstudentDet(OveralldailyattendanceabsentsmsDTO data)
        {
            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";


            fromdatecon = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
            //confromdate = fromdatecon.ToString();
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                //cmd.CommandText = "Adm_Absent_SMS_Email_Datewise";
                cmd.CommandText = "Adm_Absent_SMS_Email_Datewise_att_type";
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });

                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = data.miid
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                {
                    Value = confromdate
                });
                cmd.Parameters.Add(new SqlParameter("@att_type", SqlDbType.DateTime)
                {
                    Value = data.attType
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
                    data.student_teacherList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }
        public OveralldailyattendanceabsentsmsDTO sendsms(OveralldailyattendanceabsentsmsDTO data)
        {
            try
            {
                int y = 0;
                string msg = "";
                string msg1 = "";
                var admConfig = _db.standarad.Single(t => t.MI_Id == data.miid);
                for (int k = 0; k < data.absentlist.Length; k++)
                {
                    try
                    {
                        var studDet = _db.admissionStduent.Where(t => t.MI_Id == data.miid && t.AMST_Id == data.absentlist[k].AMST_Id).ToList();

                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                        {

                            if(data.attType=="D")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MotherMobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyOnceSMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                            else if(data.attType == "H")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MotherMobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        if(data.absentlist[k].FirstHalf == true && data.absentlist[k].SecondHalf == false)
                                        {
                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyTwiceStuAbsent", data.absentlist[k].AMST_Id, "First Half", data.ASMAY_Id).Result;

                                        }
                                        else if(data.absentlist[k].SecondHalf == true && data.absentlist[k].FirstHalf == false)
                                        {

                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyTwiceStuAbsent", data.absentlist[k].AMST_Id, "Second Half", data.ASMAY_Id).Result;
                                        }
                                        else if (data.absentlist[k].FirstHalf == true && data.absentlist[k].SecondHalf == true)
                                        {
                                            string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyOnceSMS", data.absentlist[k].AMST_Id).Result;
                                        }
                                      
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                          

                        }
                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                        {



                            if (data.attType == "D")
                            {
                                if (studDet.FirstOrDefault().AMST_FatherMobleNo != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyOnceSMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }

                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                            else if (data.attType == "H")
                            {
                                if (studDet.FirstOrDefault().AMST_FatherMobleNo != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);


                                        if (data.absentlist[k].FirstHalf == true && data.absentlist[k].SecondHalf == false)
                                        {
                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyTwiceStuAbsent", data.absentlist[k].AMST_Id, "First Half", data.ASMAY_Id).Result;

                                        }
                                        else if (data.absentlist[k].SecondHalf == true && data.absentlist[k].FirstHalf == false)
                                        {

                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyTwiceStuAbsent", data.absentlist[k].AMST_Id, "Second Half", data.ASMAY_Id).Result;
                                        }
                                        else if (data.absentlist[k].FirstHalf == true && data.absentlist[k].SecondHalf == true)
                                        {
                                            string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyOnceSMS", data.absentlist[k].AMST_Id).Result;
                                        }


                                       // string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "Student_Absent_SMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }

                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                             
                        }
                        else
                        {
                            if (data.attType == "D")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);


                                        string s = sms.sendSms(data.miid, studDet.FirstOrDefault().AMST_MobileNo, "DailyOnceSMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                            else if (data.attType == "H")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);


                                        if (data.absentlist[k].FirstHalf == true && data.absentlist[k].SecondHalf == false)
                                        {
                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo), "DailyTwiceStuAbsent", data.absentlist[k].AMST_Id, "First Half", data.ASMAY_Id).Result;

                                        }
                                        else if (data.absentlist[k].SecondHalf == true && data.absentlist[k].FirstHalf == false)
                                        {

                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo), "DailyTwiceStuAbsent", data.absentlist[k].AMST_Id, "Second Half", data.ASMAY_Id).Result;
                                        }
                                        else if (data.absentlist[k].FirstHalf == true && data.absentlist[k].SecondHalf == true)
                                        {
                                            string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo), "DailyOnceSMS", data.absentlist[k].AMST_Id).Result;
                                        }

                                       // string s = sms.sendSms(data.miid, studDet.FirstOrDefault().AMST_MobileNo, "Student_Absent_SMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }

                            }

                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                int c = data.absentlist.Count();
                if (data.absentlist.Count() == y)
                {
                    data.message = "SMS Send Successfully";
                }
                else
                {
                    data.message = "SMS Send Successfully , And Failed List '" + msg1 + "'";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public OveralldailyattendanceabsentsmsDTO sendemail(OveralldailyattendanceabsentsmsDTO data)
        {
            try
            {
                int y = 0;
                string msg = "";
                string msg1 = "";
                var admConfig = _db.standarad.Single(t => t.MI_Id == data.miid);

                for (int k = 0; k < data.absentlist.Length; k++)
                {
                    try
                    {
                        var studDet = _db.admissionStduent.Where(t => t.MI_Id == data.miid && t.AMST_Id == data.absentlist[k].AMST_Id).ToList();

                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                        {
                            if (Convert.ToString(studDet.FirstOrDefault().AMST_MotherEmailId) != null)
                            {
                                y = y + 1;
                                try
                                {
                                    Email Email = new Email(_dbsms);
                                    string m = Email.sendmail(data.miid, studDet.FirstOrDefault().AMST_MotherEmailId, "DailyOnceSMS", data.absentlist[k].AMST_Id);
                                }
                                catch (Exception ex)
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;

                                    Console.WriteLine(ex.Message);
                                    continue;
                                }
                            }
                            else
                            {
                                msg = data.absentlist[k].studentname;
                                msg1 += msg;
                            }

                        }
                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                        {
                            if (studDet.FirstOrDefault().AMST_FatheremailId != null)
                            {
                                y = y + 1;
                                try
                                {
                                    Email Email = new Email(_dbsms);
                                    string m = Email.sendmail(data.miid, studDet.FirstOrDefault().AMST_FatheremailId, "DailyOnceSMS", data.absentlist[k].AMST_Id);
                                }
                                catch (Exception ex)
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;

                                    Console.WriteLine(ex.Message);
                                    continue;
                                }
                            }
                            else
                            {
                                msg = data.absentlist[k].studentname;
                                msg1 += msg;
                            }
                        }
                        else
                        {
                            if (Convert.ToString(studDet.FirstOrDefault().AMST_emailId) != null)
                            {
                                y = y + 1;
                                try
                                {
                                    Email Email = new Email(_dbsms);
                                    string m = Email.sendmail(data.miid, studDet.FirstOrDefault().AMST_emailId, "DailyOnceSMS", data.absentlist[k].AMST_Id);
                                }
                                catch (Exception ex)
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;

                                    Console.WriteLine(ex.Message);
                                    continue;
                                }
                            }
                            else
                            {
                                msg = data.absentlist[k].studentname;
                                msg1 += msg;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                }
                int c = data.absentlist.Count();
                if (data.absentlist.Count() == y)
                {
                    data.message = "Email Send Successfully";
                }
                else
                {
                    data.message = "Email Send Successfully , And Failed List '" + msg1 + "'";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public OveralldailyattendanceabsentsmsDTO smartcardatt(OveralldailyattendanceabsentsmsDTO data)
        {
            try
            {
                int counttt = 0;
                string confromdate = "";
                DateTime fromdatecon = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
                //confromdate = fromdatecon.ToString();
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                var attendanceentrytype = "";
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.miid).ToList();

                //  data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                {
                    attendanceentrytype = "Present";
                }
                else
                {
                    attendanceentrytype = "Absent";
                }


                _log.LogInformation("Enter in smart card attendance savee option for export ");
                //  Adm_studentAttendance objpge = Mapper.Map<Adm_studentAttendance>(data);

                // DateTime today = DateTime.Today;
                List<Studentattsmartcardabsent_class_sectionlist> classid = new List<Studentattsmartcardabsent_class_sectionlist>();
                List<Studentattsmartcardabsent_class_sectionlist> sectionid = new List<Studentattsmartcardabsent_class_sectionlist>();


                classid = (from a in _db.Attendance_Students_SmartCard
                           where (a.MI_Id == data.miid)
                           select new Studentattsmartcardabsent_class_sectionlist
                           {
                               ASMCL_Id = a.ASMCL_Id,
                           }).Distinct().ToList();
                //section id
                sectionid = (from b in _db.Attendance_Students_SmartCard
                             where (b.MI_Id == data.miid)
                             select new Studentattsmartcardabsent_class_sectionlist
                             {
                                 ASMS_Id = b.ASMS_Id,
                             }).Distinct().ToList();


                _log.LogInformation("Enter in class array ");
                if (classid != null)
                {
                    for (int i = 0; i < classid.Count(); i++)
                    {
                        _log.LogInformation("Enter in class array for loop ");
                        var classid1 = classid[i].ASMCL_Id;

                        for (int j = 0; j < sectionid.Count(); j++)
                        {
                            _log.LogInformation("Enter in Section array for loop ");
                            var secid = sectionid[j].ASMS_Id;

                            var smartcard = (from a in _db.Attendance_Students_SmartCard
                                             where (a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == data.fromdate && a.ASMCL_Id == classid1 && a.ASMS_Id == secid)
                                             select new Attendance_Students_SmartCard
                                             {
                                                 AMST_Id = a.AMST_Id
                                             }).ToList();

                            var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                              where (a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == data.fromdate && a.ASMCL_Id == classid1 && a.ASMS_Id == secid)
                                              select new Attendance_Students_SmartCard
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  ASALU_Id = a.ASALU_Id
                                              }).Distinct().ToList();

                            Adm_studentAttendance obj1 = new Adm_studentAttendance();
                            List<Studentattsmartcardabsent> studentList12 = new List<Studentattsmartcardabsent>();
                            if (smartcard.Count > 0)
                            {
                                //-Check attendance is transfered or not-----//
                                var checkattendance = _db.Adm_studentAttendance.Where(d => d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id &&
                                d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.fromdate && d.ASA_Activeflag == true).ToList();
                                if (checkattendance.Count() > 0)
                                {
                                    counttt = counttt + 1;
                                    // var updated = _db.Adm_studentAttendance.Single(d => d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id &&
                                    //d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.fromdate);

                                    // updated.ASA_Att_Type = "Dailyonce";
                                    // updated.ASA_Att_EntryType = attendanceentrytype;
                                    // updated.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                    // updated.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                    // updated.UpdatedDate = DateTime.Now;
                                    // _db.Adm_studentAttendance.Update(updated);
                                    // for (int k = 0; k < smartcard.Count(); k++)
                                    // {
                                    //     var checkamstid = _db.Adm_studentAttendanceStudents.Single(a => a.AMST_Id == smartcard[k].AMST_Id && a.ASA_Id == updated.ASA_Id);
                                    //     if (checkamstid.AMST_Id > 0)
                                    //     {
                                    //         checkamstid.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                    //         checkamstid.UpdatedDate = DateTime.Now;
                                    //         checkamstid.ASA_AttendanceFlag = "Present";
                                    //         _db.Adm_studentAttendanceStudents.Update(checkamstid);
                                    //     }
                                    //     else
                                    //     {
                                    //         Adm_studentAttendanceStudents obj4 = new Adm_studentAttendanceStudents();
                                    //         obj4.AMST_Id = smartcard[k].AMST_Id;
                                    //         obj4.ASA_Id = updated.ASA_Id;
                                    //         obj4.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                    //         obj4.ASA_AttendanceFlag = "Present";
                                    //         obj4.CreatedDate = DateTime.Now;
                                    //         obj4.UpdatedDate = DateTime.Now;
                                    //         _db.Adm_studentAttendanceStudents.Add(obj4);
                                    //     }
                                    // }

                                    // //absent list 
                                    // //new        
                                    // using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    // {
                                    //     _log.LogInformation("smart card stored procedure absent details ");
                                    //     cmd.CommandText = "Attendence_Student_SmartCard";
                                    //     cmd.CommandType = CommandType.StoredProcedure;
                                    //     cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                    //     cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                    //     cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.miid });
                                    //     cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                    //     cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.fromdate });                                      

                                    //     if (cmd.Connection.State != ConnectionState.Open)
                                    //         cmd.Connection.Open();
                                    //     List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();


                                    //     using (var dataReader = cmd.ExecuteReader())
                                    //     {
                                    //         while (dataReader.Read())
                                    //         {
                                    //             var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    //             for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    //             {
                                    //                 dataRow.Add(
                                    //                     dataReader.GetName(iFiled),
                                    //                     dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    //                 );
                                    //             }
                                    //             retObject.Add(new Studentattsmartcardabsent
                                    //             {
                                    //                 AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                    //             });
                                    //         }
                                    //     }
                                    //     _log.LogInformation("retobject is created  ");
                                    //     studentList12 = retObject.ToList();
                                    // }

                                    // for (int k = 0; k < studentList12.Count; k++)
                                    // {
                                    //     var checkamstidabs = _db.Adm_studentAttendanceStudents.Single(a => a.AMST_Id == studentList12[k].AMST_Id && a.ASA_Id == updated.ASA_Id);
                                    //     if (checkamstidabs.AMST_Id > 0)
                                    //     {
                                    //         checkamstidabs.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                    //         checkamstidabs.UpdatedDate = DateTime.Now;
                                    //         checkamstidabs.ASA_AttendanceFlag = "Absent";
                                    //         _db.Adm_studentAttendanceStudents.Update(checkamstidabs);
                                    //     }
                                    //     else
                                    //     {
                                    //         Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                    //         _log.LogInformation("obj3 for absent details ");
                                    //         obj3.AMST_Id = studentList12[k].AMST_Id;
                                    //         obj3.ASA_Id = updated.ASA_Id;
                                    //         obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                    //         obj3.ASA_AttendanceFlag = "Absent";
                                    //         obj3.CreatedDate = DateTime.Now;
                                    //         obj3.UpdatedDate = DateTime.Now;
                                    //         _db.Adm_studentAttendanceStudents.Add(obj3);
                                    //         _log.LogInformation("obj3 is added to attendance student table ");
                                    //     }
                                    // }
                                }

                                else
                                {
                                    _log.LogInformation("Enter in smart card student detais array  loop ");
                                    obj1.MI_Id = data.miid;
                                    obj1.ASMAY_Id = data.ASMAY_Id;
                                    obj1.ASA_Att_Type = "Dailyonce";
                                    obj1.ASA_Att_EntryType = attendanceentrytype;
                                    obj1.ASMCL_Id = Convert.ToInt64(classid1);
                                    obj1.ASMS_Id = Convert.ToInt64(secid);
                                    obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                    obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                    obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                    obj1.IMP_Id = 0;
                                    obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                    obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                    obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                    obj1.ASA_Network_IP = data.ASA_Network_IP;
                                    obj1.CreatedDate = DateTime.Now;
                                    obj1.UpdatedDate = DateTime.Now;
                                    obj1.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(obj1);
                                    _log.LogInformation("obj1 is added to attendance table ");


                                    // Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                    List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                    for (int k = 0; k < smartcard.Count(); k++)
                                    {
                                        Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                        _log.LogInformation("obj2 for loop enter ");
                                        obj2.AMST_Id = smartcard[k].AMST_Id;
                                        obj2.ASA_Id = obj1.ASA_Id;
                                        obj2.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                        obj2.ASA_AttendanceFlag = "Present";
                                        obj2.CreatedDate = DateTime.Now;
                                        obj2.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Add(obj2);
                                        _log.LogInformation("obj2 is added to attendance student table ");
                                    }

                                    //absent list 
                                    //new        
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        _log.LogInformation("smart card stored procedure absent details ");
                                        cmd.CommandText = "Attendence_Student_SmartCard";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.VarChar) { Value = classid1 });
                                        cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.VarChar) { Value = secid });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                retObject.Add(new Studentattsmartcardabsent
                                                {
                                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                });
                                            }
                                        }
                                        _log.LogInformation("retobject is created  ");
                                        studentList1 = retObject.ToList();
                                    }

                                    for (int k = 0; k < studentList1.Count; k++)
                                    {
                                        Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                        _log.LogInformation("obj3 for absent details ");
                                        obj3.AMST_Id = studentList1[k].AMST_Id;
                                        obj3.ASA_Id = obj1.ASA_Id;
                                        obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                        obj3.ASA_AttendanceFlag = "Absent";
                                        obj3.CreatedDate = DateTime.Now;
                                        obj3.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Add(obj3);
                                        _log.LogInformation("obj3 is added to attendance student table ");
                                    }
                                }

                                var result = _db.SaveChanges();
                                if (result >= 1)
                                {
                                    data.returnval = true;
                                    if (counttt > 0)
                                    {
                                        data.message = "Record Saved Successfully But Some Class Attendance Is already Collected";
                                    }
                                    else
                                    {
                                        data.message = "Record Saved Successfully";
                                    }

                                }
                                else
                                {
                                    data.returnval = false;
                                    if (counttt > 0)
                                    {
                                        data.message = "Attendance Is Already Collected For This Date";
                                    }
                                    else
                                    {
                                        data.message = "Failed To Saved Record";
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Failed To Saved Record";
            }
            return data;
        }

        public OveralldailyattendanceabsentsmsDTO createuser(OveralldailyattendanceabsentsmsDTO data)
        {
            string failname = "";
            string admno = "";
            try
            {


                var studentlist = (from a in _db.admissionStduent
                                   from b in _db.admissionyearstudent
                                   from c in _db.admissionClass
                                   from d in _db.masterSection
                                   from e in _db.academicYear
                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.ASMAY_Id == e.ASMAY_Id && a.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == 13 && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                   select new OveralldailyattendanceabsentsmsDTO
                                   {
                                       AMST_Id = b.AMST_Id,
                                   }).ToList();
                if (studentlist.Count() > 0)
                {
                    for (int kk = 0; kk < studentlist.Count(); kk++)
                    {
                        try
                        {

                            var amst_id = studentlist[kk].AMST_Id;
                            var checkstudent = (from a in _db.StudentAppUserLoginDMO
                                                where a.AMST_ID == amst_id
                                                select new OveralldailyattendanceabsentsmsDTO
                                                {
                                                    AMST_Id = a.AMST_ID
                                                }).ToList();
                            if (checkstudent.Count() == 0)
                            {
                                var studDet = _db.admissionStduent.Where(t => t.MI_Id == data.miid && t.AMST_Id == amst_id).ToList();
                                admno = studDet.FirstOrDefault().AMST_AdmNo;
                                long stduserid = 0;
                                long fatuserid = 0;
                                long motuserid = 0;
                                string res = "";
                                Dictionary<string, long> temp = new Dictionary<string, long>();
                                generateOTP otp = new generateOTP();
                                if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
                                {

                                    string studotp = otp.getFourDigitOTP();
                                    string StudentName = "";
                                    if (studDet.FirstOrDefault().AMST_FirstName.Length > 4)
                                    {
                                        StudentName = studDet.FirstOrDefault().AMST_FirstName.Substring(0, 3) + studotp;
                                    }
                                    else
                                    {
                                        StudentName = studDet.FirstOrDefault().AMST_FirstName + studotp;
                                    }
                                    StudentName = Regex.Replace(StudentName, @"\s+", "");
                                    ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.miid, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                    stduserid = response.useridapp;
                                    res = response.resp;
                                    if (stduserid == 0)
                                    {
                                        StudentName = StudentName + 1;
                                        ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.miid, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                        stduserid = response1.useridapp;
                                        res = response1.resp;
                                        temp.Add("studentid", stduserid);
                                    }
                                    else
                                    {
                                        temp.Add("studentid", stduserid);

                                    }
                                    bool val = AddStudentUserLogin(data.miid, StudentName, studDet.FirstOrDefault().AMST_Id);
                                    if (res == "Success" && val == true)
                                    {
                                        //if (studDet.FirstOrDefault().AMST_MobileNo.ToString() != "" && studDet.FirstOrDefault().AMST_MobileNo.ToString() != null)
                                        //{
                                        //    SmsWithoutTemplate sms = new SmsWithoutTemplate(_dbsms);
                                        //    string s = sms.sendSms(data.miid, studDet.FirstOrDefault().AMST_MobileNo, StudentName, "Password@123").Result;
                                        //}

                                        //EmailWithoutTemplate Email = new EmailWithoutTemplate(_dbsms);
                                        //string m = Email.EmailWthtTmp(data.miid, StudentName, studDet.FirstOrDefault().AMST_emailId, "Password@123");
                                    }
                                    studotp = "";
                                }
                                else
                                {
                                    temp.Add("studentid", 0);
                                }
                                if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
                                {
                                    string fathrotp = otp.getFourDigitOTPFather();
                                    string fathName = "";
                                    if (studDet.FirstOrDefault().AMST_FatherName.Length > 4)
                                    {
                                        fathName = studDet.FirstOrDefault().AMST_FatherName.Substring(0, 3) + fathrotp;
                                    }
                                    else
                                    {
                                        fathName = studDet.FirstOrDefault().AMST_FatherName + fathrotp;

                                    }
                                    // string FatherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + fathrotp;
                                    fathName = Regex.Replace(fathName, @"\s+", "");
                                    if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
                                    {
                                        data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
                                    }
                                    else
                                    {
                                        data.AMST_FatherMobleNo = 0;
                                    }
                                    ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.miid, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                    fatuserid = response.useridapp;
                                    res = response.resp;
                                    if (fatuserid == 0)
                                    {
                                        fathName = fathName + 1;
                                        ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.miid, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                        fatuserid = response1.useridapp;
                                        res = response1.resp;
                                        temp.Add("Fatherid", fatuserid);
                                    }
                                    else
                                    {
                                        temp.Add("Fatherid", fatuserid);
                                    }
                                    bool val = AddStudentUserLogin(data.miid, fathName, studDet.FirstOrDefault().AMST_Id);
                                    if (res == "Success" && val == true)
                                    {
                                        //if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "" && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null)
                                        //{
                                        //    SmsWithoutTemplate sms = new SmsWithoutTemplate(_dbsms);
                                        //    string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), fathName, "Password@123").Result;
                                        //}
                                        //EmailWithoutTemplate Email = new EmailWithoutTemplate(_dbsms);
                                        //string m = Email.EmailWthtTmp(data.miid, fathName, studDet.FirstOrDefault().AMST_FatheremailId, "Password@123");
                                    }
                                    fathrotp = "";
                                }
                                else
                                {
                                    temp.Add("Fatherid", 0);
                                }
                                if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
                                {
                                    string motherotp = otp.getFourDigitOTPMother();
                                    string MotherName = "";
                                    if (studDet.FirstOrDefault().AMST_FatherName.Length > 4)
                                    {
                                        MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0, 3) + motherotp;
                                    }
                                    else
                                    {
                                        MotherName = studDet.FirstOrDefault().AMST_FatherName + motherotp;

                                    }
                                    // string MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + motherotp;
                                    MotherName = Regex.Replace(MotherName, @"\s+", "");
                                    if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
                                    {
                                        data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
                                    }
                                    else
                                    {
                                        data.AMST_MotherMobileNo = 0;
                                    }
                                    ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.miid, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                    motuserid = response.useridapp;
                                    res = response.resp;
                                    if (motuserid == 0)
                                    {
                                        MotherName = MotherName + 1;
                                        ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.miid, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                        motuserid = response1.useridapp;
                                        res = response1.resp;
                                        temp.Add("motherid", motuserid);
                                    }
                                    else
                                    {
                                        temp.Add("motherid", motuserid);
                                    }
                                    bool val = AddStudentUserLogin(data.miid, MotherName, studDet.FirstOrDefault().AMST_Id);
                                    if (res == "Success" && val == true)
                                    {
                                        //if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "" && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null)
                                        //{
                                        //    SmsWithoutTemplate sms = new SmsWithoutTemplate(_dbsms);
                                        //    string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), MotherName, "Password@123").Result;
                                        //}
                                        //EmailWithoutTemplate Email = new EmailWithoutTemplate(_dbsms);
                                        //string m = Email.EmailWthtTmp(data.miid, MotherName, studDet.FirstOrDefault().AMST_MotherEmailId, "Password@123");
                                    }
                                    motherotp = "";
                                }
                                else
                                {
                                    temp.Add("motherid", 0);
                                }
                                if (temp.Count != 0)
                                {
                                    long uid = 0;
                                    long fid = 0;
                                    long mid = 0;
                                    if (temp["studentid"] != 0)
                                    {
                                        uid = temp["studentid"];
                                    }
                                    if (temp["Fatherid"] != 0)
                                    {
                                        fid = temp["Fatherid"];
                                    }
                                    if (temp["motherid"] != 0)
                                    {
                                        mid = temp["motherid"];
                                    }

                                    bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            failname += "," + admno;
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (failname != "")
            {
                data.message = "Failed Record " + admno + "";
            }
            else
            {
                data.message = "Record Saved";
            }
            return data;
        }
        public bool AddStudentApplogin(long userid, long fatherid, long motherid, long amstId)
        {
            StudentAppUserLoginDMO dmo = new StudentAppUserLoginDMO();
            dmo.AMST_ID = amstId;
            //  dmo.CreatedDate = DateTime.Now;
            dmo.STD_APP_ID = Convert.ToInt32(userid);
            dmo.FAT_APP_ID = Convert.ToInt32(fatherid);
            dmo.MOT_APP_ID = Convert.ToInt32(motherid);

            //   dmo.UpdatedDate = DateTime.Now;
            _dbsms.Add(dmo);
            var flag = _db.SaveChanges();
            if (flag > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ImportStudentWrapperDTO> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            ImportStudentWrapperDTO respdto = new ImportStudentWrapperDTO();
            //string resp = ""; 
            //Creating Student and parents login as well as Sending user name and password code starts.
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _UserManager.FindByNameAsync(name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = name, Email = emailid, PhoneNumber = mobile };
                    user.Entry_Date = DateTime.Now;
                    user.EmailConfirmed = true;
                    var result = await _UserManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        // Student Roles
                        string studentRole = roles;
                        var id = _dbsms.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = roletype;
                        var id2 = _dbsms.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _dbsms.appUserRole.Add(role);
                        _dbsms.SaveChanges();
                        respdto.useridapp = role.UserId;
                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        _dbsms.Add(mas1);
                        var res = _dbsms.SaveChanges();
                        if (res > 0)
                        {
                            respdto.resp = "Success";
                        }
                        else
                        {
                            respdto.resp = "";
                        }

                    }
                    else
                    {
                        respdto.resp = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Admission form error");
                _log.LogDebug(e.Message);
            }
            return respdto;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }

        public bool AddStudentUserLogin(long mi_id, string username, long amstId)
        {
            StudentUserLoginDMO dmo = new StudentUserLoginDMO();
            dmo.AMST_Id = amstId;
            dmo.CreatedDate = DateTime.Now;
            dmo.IVRMSTUUL_ActiveFlag = 1;
            dmo.IVRMSTUUL_Password = "Password@123";
            dmo.IVRMSTUUL_UserName = username;
            dmo.MI_Id = mi_id;
            dmo.UpdatedDate = DateTime.Now;
            _dbsms.Add(dmo);
            var flag = _dbsms.SaveChanges();
            if (flag > 0)
            {
                StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
                inst.AMST_Id = amstId;
                inst.CreatedDate = DateTime.Now;
                inst.IVRMSTUULI_ActiveFlag = 1;
                inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
                inst.UpdatedDate = DateTime.Now;
                _dbsms.Add(inst);
                var flag1 = _dbsms.SaveChanges();
                if (flag1 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int GetUserId(OveralldailyattendanceabsentsmsDTO mas)
        {
            var Get_UserId = _db.ApplicationUser.Where(a => a.UserName == mas.username).Select(a => a.Id).FirstOrDefault();
            return Get_UserId;
        }

        //baldwin ansentees sms scheduler

        public OveralldailyattendanceabsentsmsDTO sendsms_twice(OveralldailyattendanceabsentsmsDTO data)
        {
            try
            {
                int y = 0;
                string msg = "";
                string msg1 = "";
                var admConfig = _db.standarad.Single(t => t.MI_Id == data.miid);

                var acd_Id = _db.academicYear.Where(t => t.MI_Id.Equals(data.miid) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var acd_name = _db.academicYear.Where(t => t.ASMAY_Id == acd_Id).Select(d => d.ASMAY_Year).FirstOrDefault();

                data.ASMAY_Id = acd_Id;

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                //DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    indianTime = Convert.ToDateTime(indianTime.Date.ToString("yyyy-MM-dd"));
                    confromdate = indianTime.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                List<Absent_Student_List> absentlist = new List<Absent_Student_List>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    _log.LogInformation("entered cmd getdbconnection");
                    cmd.CommandText = "Adm_Get_Today_Absent_Details_test";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.miid) });
                    //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    //cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    _log.LogInformation("entered if block");

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            _log.LogInformation("entered in dataReader block");
                            while (dataReader.Read())
                            {
                                absentlist.Add(new Absent_Student_List
                                {
                                    Amst_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                    attType = Convert.ToString(dataReader["attType"]),
                                    FirstHalf = Convert.ToBoolean(dataReader["FirstHalf"]),
                                    SecondHalf = Convert.ToBoolean(dataReader["SecondHalf"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation("error:'" + ex.Message + "'");
                        Console.Write(ex.Message);
                    }
                }



                for (int k = 0; k < absentlist.Count; k++)
                {
                    try
                    {
                        var studDet = _db.admissionStduent.Where(t => t.MI_Id == data.miid && t.AMST_Id == absentlist[k].Amst_Id).ToList();

                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                        {

                            if (absentlist[k].attType == "D")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MotherMobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyOnceSMS",absentlist[k].Amst_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                            else if (absentlist[k].attType == "H")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MotherMobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        if (absentlist[k].FirstHalf == true && absentlist[k].SecondHalf == false)
                                        {
                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyTwiceStuAbsent", absentlist[k].Amst_Id, "First Half", data.ASMAY_Id).Result;

                                        }
                                        else if (absentlist[k].SecondHalf == true && absentlist[k].FirstHalf == false)
                                        {

                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyTwiceStuAbsent", absentlist[k].Amst_Id, "Second Half", data.ASMAY_Id).Result;
                                        }
                                        else if (absentlist[k].FirstHalf == true &&absentlist[k].SecondHalf == true)
                                        {
                                            string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "DailyOnceSMS", absentlist[k].Amst_Id).Result;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }


                        }
                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                        {



                            if (absentlist[k].attType == "D")
                            {
                                if (studDet.FirstOrDefault().AMST_FatherMobleNo != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyOnceSMS", absentlist[k].Amst_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }

                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                            else if (absentlist[k].attType == "H")
                            {
                                if (studDet.FirstOrDefault().AMST_FatherMobleNo != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);


                                        if (absentlist[k].FirstHalf == true && absentlist[k].SecondHalf == false)
                                        {
                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyTwiceStuAbsent", absentlist[k].Amst_Id, "First Half", data.ASMAY_Id).Result;

                                        }
                                        else if (absentlist[k].SecondHalf == true && absentlist[k].FirstHalf == false)
                                        {

                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyTwiceStuAbsent", absentlist[k].Amst_Id, "Second Half", data.ASMAY_Id).Result;
                                        }
                                        else if (absentlist[k].FirstHalf == true && absentlist[k].SecondHalf == true)
                                        {
                                            string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "DailyOnceSMS",absentlist[k].Amst_Id).Result;
                                        }


                                        // string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "Student_Absent_SMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }

                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }

                        }
                        else
                        {
                            if (absentlist[k].attType == "D")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);


                                        string s = sms.sendSms(data.miid, studDet.FirstOrDefault().AMST_MobileNo, "DailyOnceSMS", absentlist[k].Amst_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }
                            }
                            else if (absentlist[k].attType == "H")
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MobileNo) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);


                                        if (absentlist[k].FirstHalf == true && absentlist[k].SecondHalf == false)
                                        {
                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo), "DailyTwiceStuAbsent", absentlist[k].Amst_Id, "First Half", data.ASMAY_Id).Result;

                                        }
                                        else if (absentlist[k].SecondHalf == true && absentlist[k].FirstHalf == false)
                                        {

                                            string s = sms.sendSms_dailytwice(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo), "DailyTwiceStuAbsent", absentlist[k].Amst_Id, "Second Half", data.ASMAY_Id).Result;
                                        }
                                        else if (absentlist[k].FirstHalf == true && absentlist[k].SecondHalf == true)
                                        {
                                            string s = sms.sendSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo), "DailyOnceSMS", absentlist[k].Amst_Id).Result;
                                        }

                                        // string s = sms.sendSms(data.miid, studDet.FirstOrDefault().AMST_MobileNo, "Student_Absent_SMS", data.absentlist[k].AMST_Id).Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = data.absentlist[k].studentname;
                                        msg1 += msg;

                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = data.absentlist[k].studentname;
                                    msg1 += msg;
                                }

                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                int c = data.absentlist.Count();
                if (data.absentlist.Count() == y)
                {
                    data.message = "SMS Send Successfully";
                }
                else
                {
                    data.message = "SMS Send Successfully , And Failed List '" + msg1 + "'";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
