using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class ClassWiseDailyAttendanceImpl : Interfaces.ClassWiseDailyAttendanceInterface
    {
        private static ConcurrentDictionary<string, castecategoryDTO> _login =
        new ConcurrentDictionary<string, castecategoryDTO>();

        private readonly ClassWiseDailyAttendanceContext _classWiseDailyAttendanceContext;
        public ClassWiseDailyAttendanceImpl(ClassWiseDailyAttendanceContext castecategoryContext)
        {
            _classWiseDailyAttendanceContext = castecategoryContext;
        }
        public SchoolYearWiseStudentDTO GetddlDatabind(SchoolYearWiseStudentDTO mas)
        {

            var check_rolename = (from a in _classWiseDailyAttendanceContext.MasterRoleType
                                  where (a.IVRMRT_Id == mas.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }).ToList();

            int UserId = GetUserId(mas);


            var empcode_check = (from a in _classWiseDailyAttendanceContext.Staff_User_Login
                                 where (a.MI_Id == mas.MI_Id && a.Id.Equals(UserId))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
            {
                if (empcode_check.Count > 0)
                {
                    mas.classList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                     from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                     from c in _classWiseDailyAttendanceContext.classs
                                     where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                     && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                     && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true)
                                     select new StudentAttendanceReportDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id,
                                         asmcL_ClassName = c.ASMCL_ClassName,
                                     }
                              ).Distinct().ToArray();


                    mas.sectionList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                       from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                       from c in _classWiseDailyAttendanceContext.section
                                       where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                       && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                       && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMC_ActiveFlag == 1)
                                       select new StudentAttendanceReportDTO
                                       {
                                           ASMS_Id = c.ASMS_Id,
                                           ASMC_SectionName = c.ASMC_SectionName,
                                       }
                                        ).Distinct().ToArray();



                }
                else
                {
                    //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }
            else
            {
                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _classWiseDailyAttendanceContext.classs.Where(s => s.MI_Id == mas.MI_Id && s.ASMCL_ActiveFlag == true).ToList();
                mas.classList = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = _classWiseDailyAttendanceContext.section.Where(y => y.MI_Id == mas.MI_Id && y.ASMC_ActiveFlag == 1).ToList();
                mas.sectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
            }

            List<AcademicYear> aya = new List<AcademicYear>();
            aya = _classWiseDailyAttendanceContext.academicyr.Where(d => d.MI_Id == mas.MI_Id && d.Is_Active == true).ToList();
            mas.YearList = aya.OrderByDescending(a => a.ASMAY_Order).ToArray();

            List<AcademicYear> defaultyear = new List<AcademicYear>();
            defaultyear = _classWiseDailyAttendanceContext.academicyr.Where(d => d.MI_Id == mas.MI_Id && d.ASMAY_Id == mas.ASMAY_Id && d.Is_Active == true).ToList();
            mas.defalutYearList = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();

            return mas;
        }
        public SchoolYearWiseStudentDTO getsection(SchoolYearWiseStudentDTO mas)
        {

            var check_rolename = (from a in _classWiseDailyAttendanceContext.MasterRoleType
                                  where (a.IVRMRT_Id == mas.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }).ToList();

            int UserId = GetUserId(mas);

            var empcode_check = (from a in _classWiseDailyAttendanceContext.Staff_User_Login
                                 where (a.MI_Id == mas.MI_Id && a.Id.Equals(UserId))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
            {
                if (empcode_check.Count > 0)
                {
                    mas.classList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                     from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                     from c in _classWiseDailyAttendanceContext.classs
                                     where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                     && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                     && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true)
                                     select new StudentAttendanceReportDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id,
                                         asmcL_ClassName = c.ASMCL_ClassName,
                                     }).Distinct().ToArray();


                    mas.sectionList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                       from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                       from c in _classWiseDailyAttendanceContext.section
                                       where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                       && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                       && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMC_ActiveFlag == 1)
                                       select new StudentAttendanceReportDTO
                                       {
                                           ASMS_Id = c.ASMS_Id,
                                           ASMC_SectionName = c.ASMC_SectionName,
                                       }).Distinct().ToArray();
                }
                else
                {
                    //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }
            else
            {
                mas.sectionList = (from a in _classWiseDailyAttendanceContext.AdmSchoolMasterClassCatSec
                                   from b in _classWiseDailyAttendanceContext.Masterclasscategory
                                   from c in _classWiseDailyAttendanceContext.classs
                                   from d in _classWiseDailyAttendanceContext.section
                                   where (a.ASMCC_Id == b.ASMCC_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCCS_ActiveFlg == true
                                   && b.Is_Active == true && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1 && b.MI_Id == mas.MI_Id
                                   && b.ASMCL_Id == mas.ASMCL_Id && b.ASMAY_Id == mas.ASMAY_Id)
                                   select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }

            return mas;

        }
        public SchoolYearWiseStudentDTO setfromdate(SchoolYearWiseStudentDTO mas)
        {
            var check_rolename = (from a in _classWiseDailyAttendanceContext.MasterRoleType
                                  where (a.IVRMRT_Id == mas.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }).ToList();

            int UserId = GetUserId(mas);

            var empcode_check = (from a in _classWiseDailyAttendanceContext.Staff_User_Login
                                 where (a.MI_Id == mas.MI_Id && a.Id.Equals(UserId))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
            {
                if (empcode_check.Count > 0)
                {
                    mas.classList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                     from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                     from c in _classWiseDailyAttendanceContext.classs
                                     where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                     && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                     && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true)
                                     select new StudentAttendanceReportDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id,
                                         asmcL_ClassName = c.ASMCL_ClassName,
                                     }).Distinct().ToArray();
                }
                else
                {
                    //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }
            else
            {
                mas.classList = (from a in _classWiseDailyAttendanceContext.classs
                                 from b in _classWiseDailyAttendanceContext.Masterclasscategory
                                 from c in _classWiseDailyAttendanceContext.academicyr
                                 where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true && c.Is_Active == true
                                 && b.ASMAY_Id == mas.ASMAY_Id)
                                 select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }


            return mas;
        }

        public int GetUserId(SchoolYearWiseStudentDTO mas)
        {
            var Get_UserId = _classWiseDailyAttendanceContext.ApplicationUser.Where(a => a.UserName == mas.username).Select(a => a.Id).FirstOrDefault();            
            return Get_UserId;
        }
        public async Task<SchoolYearWiseStudentDTO> absentsendsms(SchoolYearWiseStudentDTO data)
        {
            try
            {
                int y = 0;
                string msg = "";
                string msg1 = "";

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(data.FromDate.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                string e = string.Empty;
                for (int k = 0; k < data.absentlist.Length; k++)
                {
                    long MI_id = data.MI_Id;
                    string mobileno = data.absentlist[k].AMST_MobileNo.ToString();
                    long AMST_Id = data.absentlist[k].AMST_Id;

                    if (mobileno.Length == 10)
                    {
                        y = y + 1;
                        try
                        {

                            e = await sendSms(MI_id, mobileno, "Attendance_Absent", AMST_Id, confromdate);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        msg = data.absentlist[k].AMST_FirstName;
                        msg1 += msg;
                    }
                }

                if (data.absentlist.Length == y)
                {
                    data.message = "SMS Sent Successfully";
                }
                else
                {
                    data.message = "SMS Sent Successfully , And Failed List '" + msg1 + "'";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<string> sendSms(long MI_Id, string mobileNo, string Template , long UserID, string date)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _classWiseDailyAttendanceContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _classWiseDailyAttendanceContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _classWiseDailyAttendanceContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _classWiseDailyAttendanceContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {

                    using (var cmd = _classWiseDailyAttendanceContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }



                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _classWiseDailyAttendanceContext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _classWiseDailyAttendanceContext.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {

                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO =  mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                    if (url.Contains("entityid"))
                    {
                        if (insdeta[0].MI_EntityId.ToString() != null && insdeta[0].MI_EntityId.ToString() != "")
                        {
                            url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                        }
                    }
                    if (url.Contains("templateid"))
                    {
                        if (template.FirstOrDefault().ISES_TemplateId != null && template.FirstOrDefault().ISES_TemplateId != "")
                        {
                            url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                        }
                    }


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var template101 = _classWiseDailyAttendanceContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    var moduleid = _classWiseDailyAttendanceContext.Institution_Module.Where(i => template101.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    var modulename = _classWiseDailyAttendanceContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    using (var cmd = _classWiseDailyAttendanceContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                        SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                        SqlDbType.VarChar)
                        {
                            Value = messageid
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
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
