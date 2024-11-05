using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Dynamic;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net.Mail;
using System.IO;
using DomainModel.Model.com.vaps.Fee;
using System.Net;
using TransportContext = DataAccessMsSqlServerProvider.com.vapstech.Transport.TransportContext;
using Newtonsoft.Json;
using System.Text;

namespace TransportServiceHub.Services
{
    public class UsrPwdImpl : Interfaces.UsrPwdInterface
    {

        public TransportContext _context;
        ILogger<SMSEmailSendImpl> _areaimpl;
        public DomainModelMsSqlServerContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _UserManager;

        public UsrPwdImpl(ILogger<SMSEmailSendImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> UserManager)
        {
            _areaimpl = areaimpl;
            _context = context;
            _db = db;
            _UserManager = UserManager;
            _signInManager = signInManager;
        }

        public SMSEmailSendDTO getdata(int id)
        {
            SMSEmailSendDTO data = new SMSEmailSendDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _context.School_M_Class.Where(y => y.ASMCL_ActiveFlag == true && y.MI_Id == id).ToList();
                data.classlist = allclass.Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                List<School_M_Section> allsec = new List<School_M_Section>();
                allsec = _context.School_M_Section.Where(s => s.MI_Id == id).ToList();
                data.sectionlist = allsec.Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }

        public SMSEmailSendDTO Getreportdetails(SMSEmailSendDTO data)
        {
            try
            {
                List<long> classid = new List<long>();
                List<long> sectionid = new List<long>();
                string sectionidd = "0";

                List<long> alumniids = new List<long>();
                if (data.ASMS_Id != 0)
                {
                    var sectionidlist = (from a in _db.AcademicYear
                                         from b in _db.Masterclasscategory
                                         from c in _db.School_M_Class
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from e in _db.School_M_Section
                                         where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                         && d.ASMCC_Id == b.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true
                                         && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && d.ASMS_Id == data.ASMS_Id)
                                         select e).Distinct().ToList();
                    foreach (var y in sectionidlist)
                    {
                        sectionid.Add(y.ASMS_Id);
                    }

                    foreach (var d in sectionidlist)
                    {
                        sectionidd = "0" + "," + d.ASMS_Id;

                    }
                }
                else
                {
                    var sectionidlist = (from a in _db.AcademicYear
                                         from b in _db.Masterclasscategory
                                         from c in _db.School_M_Class
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from e in _db.School_M_Section
                                         where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                         && d.ASMCC_Id == b.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true
                                         && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                         select e).Distinct().ToList();
                    foreach (var y in sectionidlist)
                    {
                        sectionid.Add(y.ASMS_Id);
                    }


                    foreach (var d in sectionidlist)
                    {
                        //sectionidd += "0" +","+ d.ASMS_Id;

                        sectionidd = sectionidd + "," + d.ASMS_Id;

                    }
                }



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "UserCreationStudents";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });



                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
               SqlDbType.VarChar)
                    {
                        Value = sectionidd
                    });
                    cmd.Parameters.Add(new SqlParameter("@filterdata",
            SqlDbType.VarChar)
                    {
                        Value = data.filterdata
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
                        data.messagelist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                //if (data.filterdata == "reg")
                //{
                //    data.messagelist = (from a in _context.AcademicYear
                //                        from b in _context.School_M_Class
                //                        from c in _context.School_M_Section
                //                        from d in _context.Adm_M_Student
                //                        from e in _context.School_Adm_Y_StudentDMO
                //                        where (d.AMST_Id == e.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id
                //                        && d.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && sectionid.Contains(e.ASMS_Id)
                //                        && d.AMST_SOL == "S" && d.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                //                        select new SMSEmailSendDTO
                //                        {
                //                            ASMCL_ClassName = b.ASMCL_ClassName,
                //                            ASMC_SectionName = c.ASMC_SectionName,
                //                            studentname = ((d.AMST_FirstName == null ? "" : d.AMST_FirstName) +
                //                            (d.AMST_MiddleName == null ? "" : " " + d.AMST_MiddleName) +
                //                            (d.AMST_LastName == null ? "" : " " + d.AMST_LastName)).Trim(),
                //                            AMST_AdmNo = d.AMST_AdmNo == null ? " " : d.AMST_AdmNo,
                //                            AMST_Id = d.AMST_Id
                //                        }).OrderBy(dd => dd.studentname).ToArray();
                //}
                //else if (data.filterdata == "new")
                //{
                //    data.messagelist = (from a in _context.AcademicYear
                //                        from b in _context.School_M_Class
                //                        from c in _context.School_M_Section
                //                        from d in _context.Adm_M_Student
                //                        from e in _context.School_Adm_Y_StudentDMO
                //                        where (d.AMST_Id == e.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id
                //                        && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && sectionid.Contains(e.ASMS_Id)
                //                        && d.AMST_SOL == "S" && d.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                //                        select new SMSEmailSendDTO
                //                        {
                //                            ASMCL_ClassName = b.ASMCL_ClassName,
                //                            ASMC_SectionName = c.ASMC_SectionName,
                //                            studentname = ((d.AMST_FirstName == null ? "" : d.AMST_FirstName) +
                //                            (d.AMST_MiddleName == null ? "" : " " + d.AMST_MiddleName) +
                //                            (d.AMST_LastName == null ? "" : " " + d.AMST_LastName)).Trim(),
                //                            AMST_AdmNo = d.AMST_AdmNo == null ? " " : d.AMST_AdmNo,
                //                            AMST_Id = d.AMST_Id
                //                        }).OrderBy(dd => dd.studentname).ToArray();
                //}
                //else
                //{

                //    var alumnilist = (from a in _db.AlumniUserRegistrationDMO
                //                      from b in _db.Alumni_User_LoginDMO
                //                      from d in _db.Alumni_M_StudentDMO
                //                      where (a.ALMST_Id == d.ALMST_Id && a.ALSREG_Id == b.ALSREG_Id
                //                      && d.MI_Id == data.MI_Id)
                //                      select new SMSEmailSendDTO
                //                      {
                //                          AMST_Id = d.ALMST_Id
                //                      }).ToList();

                //    foreach (var y in alumnilist)
                //    {
                //        alumniids.Add(y.AMST_Id);
                //    }

                //    data.messagelist = (from a in _context.AcademicYear
                //                        from b in _context.School_M_Class
                //                        from d in _context.Alumni_M_StudentDMO
                //                        where (a.ASMAY_Id == d.ASMAY_Id_Left && b.ASMCL_Id == d.ASMCL_Id_Left
                //                        && d.MI_Id == data.MI_Id && d.ASMAY_Id_Left == data.ASMAY_Id && d.ASMCL_Id_Left == data.ASMCL_Id && !alumniids.Contains(d.ALMST_Id))
                //                        select new SMSEmailSendDTO
                //                        {
                //                            ASMCL_ClassName = b.ASMCL_ClassName,
                //                            studentname = ((d.ALMST_FirstName == null ? "" : d.ALMST_FirstName) +
                //                            (d.ALMST_MiddleName == null ? "" : " " + d.ALMST_MiddleName) +
                //                            (d.ALMST_LastName == null ? "" : " " + d.ALMST_LastName)).Trim(),
                //                            AMST_AdmNo = d.ALMST_AdmNo == null ? " " : d.ALMST_AdmNo,
                //                            AMST_Id = d.ALMST_Id
                //                        }).OrderBy(dd => dd.studentname).ToArray();
                //}



            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }

        public async Task<SMSEmailSendDTO> smssend(SMSEmailSendDTO data)
        {
            SMS sms = new SMS(_db);

            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_MobileNo != 0)
                {
                    data.success = await sms.sendSms(data.MI_Id, data.data_array[i].AMST_MobileNo, "ADMINISTRATOR", data.data_array[i].AMST_Id);
                }
            }

            //data.success = await sms.sendSms(data.MI_Id, data.data_array[i].AMST_MobileNo, "ADMINISTRATOR", data.data_array[i].AMST_Id);

            return data;
        }
        public SMSEmailSendDTO emailsend(SMSEmailSendDTO data)
        {

            Email Email = new Email(_db);
            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_emailId != "null" && data.data_array[i].AMST_emailId != null && data.data_array[i].AMST_emailId != "")
                {
                    data.success = Email.sendmail(data.MI_Id, data.data_array[i].AMST_emailId, "ADMINISTRATOR", data.data_array[i].AMST_Id);
                }
            }
            return data;
        }

        public SMSEmailSendDTO creusrnme(SMSEmailSendDTO data)
        {
            try
            {

                data.success = createusername(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //public string createusername(SMSEmailSendDTO data)
        //{
        //    string failname = "";
        //    string admno = "";
        //    string email = "";
        //    long mobile = 0;
        //    string student_Name = "";
        //    int counts = 0;
        //    int countf = 0;
        //    string otpadmno = "";

        //    try
        //    {
        //        if (data.filterdata != "Alumni")
        //        {
        //            var credentials_check = _db.GenConfig.Where(s => s.MI_Id == data.MI_Id).ToList();
        //            var virtualcode = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).Select(t => t.ivrm_school_code).FirstOrDefault();

        //            for (int kk = 0; kk < data.data_array.Count(); kk++)
        //            {
        //                var username = "";
        //                var name_str = "";
        //                var mobileno_str = "";
        //                var admno_str = "";
        //                var reg_str = "";
        //                var email_str = "";

        //                long stduserid = 0;
        //                long fatuserid = 0;
        //                long motuserid = 0;
        //                long guruserid = 0;
        //                string res = "";

        //                long countincrementstudnet = 0;
        //                long countincrementfather = 0;
        //                long countincrementmother = 0;

        //                var amst_id = data.data_array[kk].AMST_Id;
        //                var checkstudent = (from a in _db.StudentAppUserLoginDMO
        //                                    where a.AMST_ID == amst_id
        //                                    select new SMSEmailSendDTO
        //                                    {
        //                                        AMST_Id = a.AMST_ID
        //                                    }).ToList();
        //                if (checkstudent.Count() == 0)
        //                {
        //                    var check_custom = _db.IVRM_Custom_UserName_PasswordDMO.Where(d => d.MI_Id == data.MI_Id).OrderBy(f => f.IVRMCUNP_Order).ToList();
        //                    var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amst_id).ToList();

        //                    Dictionary<string, long> temp = new Dictionary<string, long>();
        //                    if (check_custom.Count() > 0)
        //                    {
        //                        //if (credentials_check.FirstOrDefault().IVRMGC_UserNameOptionsFlg != "" && credentials_check.FirstOrDefault().IVRMGC_UserNameOptionsFlg != null)
        //                        //{
        //                        //    if (credentials_check.FirstOrDefault().IVRMGC_UserNameOptionsFlg == "Custom")
        //                        //    {


        //                        //        if (credentials_check.FirstOrDefault().IVRMGC_StudentLoginCred == true)
        //                        //        {

        //                        //            if ((studDet.FirstOrDefault().AMST_emailId.ToString() != null && studDet.FirstOrDefault().AMST_emailId.ToString() != "") || (studDet.FirstOrDefault().AMST_MobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MobileNo.ToString() != ""))
        //                        //            {
        //                        //                foreach (var item in check_custom.ToList())
        //                        //                {
        //                        //                    if (item.IVRMCUNP_Field == "Name")
        //                        //                    {


        //                        //                        var name = studDet.FirstOrDefault().AMST_FirstName;
        //                        //                        if (item.IVRMCUNP_Length > 0)
        //                        //                        {
        //                        //                            if (name.Length >= item.IVRMCUNP_Length)
        //                        //                            {
        //                        //                                int length = (int)(item.IVRMCUNP_Length);
        //                        //                                if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                {
        //                        //                                    username += name.Substring(0, length);
        //                        //                                }
        //                        //                                else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                {
        //                        //                                    username += name.Substring((name.Length - length), length);
        //                        //                                }
        //                        //                            }
        //                        //                        }
        //                        //                    }
        //                        //                    else if (item.IVRMCUNP_Field == "AdmnissionNo")
        //                        //                    {

        //                        //                        var AdmNo = studDet.FirstOrDefault().AMST_AdmNo;
        //                        //                        if (item.IVRMCUNP_Length > 0)
        //                        //                        {
        //                        //                            if (AdmNo.Length >= item.IVRMCUNP_Length)
        //                        //                            {
        //                        //                                int length = (int)(item.IVRMCUNP_Length);
        //                        //                                if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                {
        //                        //                                    username += AdmNo.Substring(0, length);
        //                        //                                }
        //                        //                                else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                {
        //                        //                                    username += AdmNo.Substring((AdmNo.Length - length), length);
        //                        //                                }
        //                        //                            }
        //                        //                        }
        //                        //                    }
        //                        //                    else if (item.IVRMCUNP_Field == "EmailId")
        //                        //                    {

        //                        //                        var emailId = studDet.FirstOrDefault().AMST_emailId;
        //                        //                        if (item.IVRMCUNP_Length > 0)
        //                        //                        {
        //                        //                            if (emailId.Length >= item.IVRMCUNP_Length)
        //                        //                            {
        //                        //                                int length = (int)(item.IVRMCUNP_Length);
        //                        //                                if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                {
        //                        //                                    username += emailId.Substring(0, length);
        //                        //                                }
        //                        //                                else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                {
        //                        //                                    username += emailId.Substring((emailId.Length - length), length);
        //                        //                                }
        //                        //                            }
        //                        //                        }
        //                        //                    }
        //                        //                    else if (item.IVRMCUNP_Field == "RegistrationNo")
        //                        //                    {

        //                        //                        var RegistrationNo = studDet.FirstOrDefault().AMST_RegistrationNo;
        //                        //                        if (item.IVRMCUNP_Length > 0)
        //                        //                        {
        //                        //                            int length = (int)(item.IVRMCUNP_Length);
        //                        //                            if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                            {
        //                        //                                username += RegistrationNo.Substring(0, length);
        //                        //                            }
        //                        //                            else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                            {
        //                        //                                username += RegistrationNo.Substring((RegistrationNo.Length - length), length);
        //                        //                            }

        //                        //                        }
        //                        //                    }

        //                        //                    else if (item.IVRMCUNP_Field == "MobileNo")
        //                        //                    {

        //                        //                        var MobileNo = studDet.FirstOrDefault().AMST_MobileNo.ToString();
        //                        //                        if (item.IVRMCUNP_Length > 0)
        //                        //                        {
        //                        //                            int length = (int)(item.IVRMCUNP_Length);
        //                        //                            if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                            {
        //                        //                                username += MobileNo.Substring(0, length);
        //                        //                            }
        //                        //                            else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                            {
        //                        //                                username += MobileNo.Substring((MobileNo.Length - length), length);
        //                        //                            }

        //                        //                        }
        //                        //                    }


        //                        //                }
        //                        //                username += "S";

        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, username, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                        //                stduserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (stduserid == 0)
        //                        //                {
        //                        //                    temp.Add("stduserid", stduserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("studentid", stduserid);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        if (credentials_check.FirstOrDefault().IVRMGC_FatherLoginCred == true)
        //                        //        {

        //                        //            if (studDet.FirstOrDefault().AMST_FatheremailId != null)
        //                        //            {
        //                        //                if (studDet.FirstOrDefault().AMST_FatherMobleNo != null)

        //                        //                {
        //                        //                    foreach (var item in check_custom.ToList())
        //                        //                    {
        //                        //                        if (item.IVRMCUNP_Field == "Name")
        //                        //                        {
        //                        //                            if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
        //                        //                            {

        //                        //                                var name = studDet.FirstOrDefault().AMST_FatherName;
        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    if (name.Length >= item.IVRMCUNP_Length)
        //                        //                                    {
        //                        //                                        int length = (int)(item.IVRMCUNP_Length);
        //                        //                                        if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                        {
        //                        //                                            username += name.Substring(0, length);
        //                        //                                        }
        //                        //                                        else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                        {
        //                        //                                            username += name.Substring((name.Length - length), length);
        //                        //                                        }
        //                        //                                    }
        //                        //                                }
        //                        //                            }
        //                        //                        }

        //                        //                        else if (item.IVRMCUNP_Field == "AdmnissionNo")
        //                        //                        {

        //                        //                            var AdmNo = studDet.FirstOrDefault().AMST_AdmNo;
        //                        //                            if (item.IVRMCUNP_Length > 0)
        //                        //                            {
        //                        //                                if (AdmNo.Length >= item.IVRMCUNP_Length)
        //                        //                                {
        //                        //                                    int length = (int)(item.IVRMCUNP_Length);
        //                        //                                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                    {
        //                        //                                        username += AdmNo.Substring(0, length);
        //                        //                                    }
        //                        //                                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                    {
        //                        //                                        username += AdmNo.Substring((AdmNo.Length - length), length);
        //                        //                                    }
        //                        //                                }
        //                        //                            }
        //                        //                        }
        //                        //                        else if (item.IVRMCUNP_Field == "EmailId")
        //                        //                        {
        //                        //                            if (studDet.FirstOrDefault().AMST_emailId != null)
        //                        //                            {
        //                        //                                var emailId = studDet.FirstOrDefault().AMST_emailId;
        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    if (emailId.Length >= item.IVRMCUNP_Length)
        //                        //                                    {
        //                        //                                        int length = (int)(item.IVRMCUNP_Length);
        //                        //                                        if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                        {
        //                        //                                            username += emailId.Substring(0, length);
        //                        //                                        }
        //                        //                                        else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                        {
        //                        //                                            username += emailId.Substring((emailId.Length - length), length);
        //                        //                                        }
        //                        //                                    }
        //                        //                                }
        //                        //                            }

        //                        //                        }
        //                        //                        else if (item.IVRMCUNP_Field == "RegistrationNo")
        //                        //                        {
        //                        //                            if (studDet.FirstOrDefault().AMST_RegistrationNo != null)
        //                        //                            {
        //                        //                                var RegistrationNo = studDet.FirstOrDefault().AMST_RegistrationNo;
        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    int length = (int)(item.IVRMCUNP_Length);
        //                        //                                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                    {
        //                        //                                        username += RegistrationNo.Substring(0, length);
        //                        //                                    }
        //                        //                                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                    {
        //                        //                                        username += RegistrationNo.Substring((RegistrationNo.Length - length), length);
        //                        //                                    }

        //                        //                                }
        //                        //                            }

        //                        //                        }

        //                        //                        else if (item.IVRMCUNP_Field == "MobileNo")
        //                        //                        {
        //                        //                            if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null)
        //                        //                            {
        //                        //                                var MobileNo = studDet.FirstOrDefault().AMST_FatherMobleNo.ToString();
        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    int length = (int)(item.IVRMCUNP_Length);
        //                        //                                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                    {
        //                        //                                        username += MobileNo.Substring(0, length);
        //                        //                                    }
        //                        //                                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                    {
        //                        //                                        username += MobileNo.Substring((MobileNo.Length - length), length);
        //                        //                                    }

        //                        //                                }
        //                        //                            }



        //                        //                        }
        //                        //                    }
        //                        //                    username += "F";

        //                        //                    ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, username, data.MI_Id, "PARENTS", "PARENTS", studDet.FirstOrDefault().AMST_FatherMobleNo.ToString()).Result;
        //                        //                    fatuserid = response.useridapp;
        //                        //                    res = response.resp;

        //                        //                    if (fatuserid == 0)
        //                        //                    {
        //                        //                        temp.Add("fatuserid", fatuserid);
        //                        //                        bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                        if (res == "Success" && val == true)
        //                        //                        {
        //                        //                        }
        //                        //                    }
        //                        //                    else
        //                        //                    {
        //                        //                        temp.Add("fatuserid", fatuserid);
        //                        //                    }

        //                        //                }
        //                        //            }

        //                        //        }
        //                        //        else if (credentials_check.FirstOrDefault().IVRMGC_MotherLoginCred == true)
        //                        //        {
        //                        //            if (studDet.FirstOrDefault().AMST_MotherEmailId != null)
        //                        //            {

        //                        //                if (studDet.FirstOrDefault().AMST_MotherMobileNo != null)
        //                        //                {
        //                        //                    foreach (var item in check_custom.ToList())
        //                        //                    {
        //                        //                        if (item.IVRMCUNP_Field == "Name")
        //                        //                        {
        //                        //                            if (studDet.FirstOrDefault().AMST_MotherName.ToString() != null && studDet.FirstOrDefault().AMST_MotherName.ToString() != "")
        //                        //                            {
        //                        //                                var name = "";
        //                        //                                name = studDet.FirstOrDefault().AMST_MotherName;
        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    if (name.Length >= item.IVRMCUNP_Length)
        //                        //                                    {
        //                        //                                        int length = (int)(item.IVRMCUNP_Length);
        //                        //                                        if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                        {
        //                        //                                            username += name.Substring(0, length);
        //                        //                                        }
        //                        //                                        else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                        {
        //                        //                                            username += name.Substring((name.Length - length), length);
        //                        //                                        }
        //                        //                                    }
        //                        //                                }
        //                        //                            }
        //                        //                        }
        //                        //                        else if (item.IVRMCUNP_Field == "AdmnissionNo")
        //                        //                        {

        //                        //                            var AdmNo = studDet.FirstOrDefault().AMST_AdmNo;
        //                        //                            if (item.IVRMCUNP_Length > 0)
        //                        //                            {
        //                        //                                if (AdmNo.Length >= item.IVRMCUNP_Length)
        //                        //                                {
        //                        //                                    int length = (int)(item.IVRMCUNP_Length);
        //                        //                                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                    {
        //                        //                                        username += AdmNo.Substring(0, length);
        //                        //                                    }
        //                        //                                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                    {
        //                        //                                        username += AdmNo.Substring((AdmNo.Length - length), length);
        //                        //                                    }
        //                        //                                }
        //                        //                            }
        //                        //                        }
        //                        //                        else if (item.IVRMCUNP_Field == "EmailId")
        //                        //                        {
        //                        //                            var emailId = "";
        //                        //                            if (studDet.FirstOrDefault().AMST_MotherEmailId != null)
        //                        //                            {
        //                        //                                emailId = studDet.FirstOrDefault().AMST_MotherEmailId;

        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    if (emailId.Length >= item.IVRMCUNP_Length)
        //                        //                                    {
        //                        //                                        int length = (int)(item.IVRMCUNP_Length);
        //                        //                                        if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                        {
        //                        //                                            username += emailId.Substring(0, length);
        //                        //                                        }
        //                        //                                        else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                        {
        //                        //                                            username += emailId.Substring((emailId.Length - length), length);
        //                        //                                        }
        //                        //                                    }
        //                        //                                }
        //                        //                            }

        //                        //                        }
        //                        //                        else if (item.IVRMCUNP_Field == "RegistrationNo")
        //                        //                        {
        //                        //                            var RegistrationNo = "";
        //                        //                            if (studDet.FirstOrDefault().AMST_RegistrationNo != null)
        //                        //                            {
        //                        //                                RegistrationNo = studDet.FirstOrDefault().AMST_RegistrationNo;

        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    int length = (int)(item.IVRMCUNP_Length);
        //                        //                                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                    {
        //                        //                                        username += RegistrationNo.Substring(0, length);
        //                        //                                    }
        //                        //                                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                    {
        //                        //                                        username += RegistrationNo.Substring((RegistrationNo.Length - length), length);
        //                        //                                    }

        //                        //                                }
        //                        //                            }

        //                        //                        }

        //                        //                        else if (item.IVRMCUNP_Field == "MobileNo")
        //                        //                        {
        //                        //                            var MobileNo = "";
        //                        //                            if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null)
        //                        //                            {
        //                        //                                MobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo.ToString();

        //                        //                                if (item.IVRMCUNP_Length > 0)
        //                        //                                {
        //                        //                                    int length = (int)(item.IVRMCUNP_Length);
        //                        //                                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //                                    {
        //                        //                                        username += MobileNo.Substring(0, length);
        //                        //                                    }
        //                        //                                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //                                    {
        //                        //                                        username += MobileNo.Substring((MobileNo.Length - length), length);
        //                        //                                    }

        //                        //                                }
        //                        //                            }



        //                        //                        }
        //                        //                    }
        //                        //                    username += "M";

        //                        //                    ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, username, data.MI_Id, "PARENTS", "PARENTS", studDet.FirstOrDefault().AMST_MotherMobileNo.ToString()).Result;
        //                        //                    motuserid = response.useridapp;
        //                        //                    res = response.resp;
        //                        //                    if (motuserid == 0)
        //                        //                    {
        //                        //                        temp.Add("motuserid", motuserid);

        //                        //                        bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                        if (res == "Success" && val == true)
        //                        //                        {
        //                        //                        }
        //                        //                    }
        //                        //                    else
        //                        //                    {
        //                        //                        temp.Add("motuserid", motuserid);
        //                        //                    }
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        //else if (credentials_check.FirstOrDefault().IVRMGC_GuardianLoginCred == true)
        //                        //        //{
        //                        //        //    var guardiandetails = _db.StudentGuardianDMO.Where(a => a.AMST_Id == amst_id && a.MI_Id == data.MI_Id).ToArray();

        //                        //        //    if ((guardiandetails.FirstOrDefault().AMSTG_emailid != null && guardiandetails.FirstOrDefault().AMSTG_emailid != "") || (guardiandetails.FirstOrDefault().AMSTG_GuardianPhoneNo.ToString() != null && guardiandetails.FirstOrDefault().AMSTG_GuardianPhoneNo.ToString() != ""))
        //                        //        //    {
        //                        //        //        if (item.IVRMCUNP_Field == "Name")
        //                        //        //        {
        //                        //        //            if (studDet.FirstOrDefault().AMST_MotherName.ToString() != null && studDet.FirstOrDefault().AMST_MotherName.ToString() != "")
        //                        //        //            {
        //                        //        //                var name = "";
        //                        //        //                name = studDet.FirstOrDefault().AMST_MotherName;
        //                        //        //                if (item.IVRMCUNP_Length > 0)
        //                        //        //                {
        //                        //        //                    if (name.Length >= item.IVRMCUNP_Length)
        //                        //        //                    {
        //                        //        //                        int length = (int)(item.IVRMCUNP_Length);
        //                        //        //                        if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //        //                        {
        //                        //        //                            username += name.Substring(0, length);
        //                        //        //                        }
        //                        //        //                        else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //        //                        {
        //                        //        //                            username += name.Substring((name.Length - length), length);
        //                        //        //                        }
        //                        //        //                    }
        //                        //        //                }
        //                        //        //            }
        //                        //        //        }
        //                        //        //        else if (item.IVRMCUNP_Field == "AdmnissionNo")
        //                        //        //        {

        //                        //        //            var AdmNo = studDet.FirstOrDefault().AMST_AdmNo;
        //                        //        //            if (item.IVRMCUNP_Length > 0)
        //                        //        //            {
        //                        //        //                if (AdmNo.Length >= item.IVRMCUNP_Length)
        //                        //        //                {
        //                        //        //                    int length = (int)(item.IVRMCUNP_Length);
        //                        //        //                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //        //                    {
        //                        //        //                        username += AdmNo.Substring(0, length);
        //                        //        //                    }
        //                        //        //                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //        //                    {
        //                        //        //                        username += AdmNo.Substring((AdmNo.Length - length), length);
        //                        //        //                    }
        //                        //        //                }
        //                        //        //            }
        //                        //        //        }
        //                        //        //        else if (item.IVRMCUNP_Field == "EmailId")
        //                        //        //        {
        //                        //        //            var emailId = "";
        //                        //        //            emailId = studDet.FirstOrDefault().AMST_MotherEmailId;
        //                        //        //            if (item.IVRMCUNP_Length > 0)
        //                        //        //            {
        //                        //        //                if (emailId.Length >= item.IVRMCUNP_Length)
        //                        //        //                {
        //                        //        //                    int length = (int)(item.IVRMCUNP_Length);
        //                        //        //                    if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //        //                    {
        //                        //        //                        username += emailId.Substring(0, length);
        //                        //        //                    }
        //                        //        //                    else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //        //                    {
        //                        //        //                        username += emailId.Substring((emailId.Length - length), length);
        //                        //        //                    }
        //                        //        //                }
        //                        //        //            }
        //                        //        //        }
        //                        //        //        else if (item.IVRMCUNP_Field == "RegistrationNo")
        //                        //        //        {
        //                        //        //            var RegistrationNo = "";
        //                        //        //            RegistrationNo = studDet.FirstOrDefault().AMST_RegistrationNo;
        //                        //        //            if (item.IVRMCUNP_Length > 0)
        //                        //        //            {
        //                        //        //                int length = (int)(item.IVRMCUNP_Length);
        //                        //        //                if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //        //                {
        //                        //        //                    username += RegistrationNo.Substring(0, length);
        //                        //        //                }
        //                        //        //                else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //        //                {
        //                        //        //                    username += RegistrationNo.Substring((RegistrationNo.Length - length), length);
        //                        //        //                }

        //                        //        //            }
        //                        //        //        }

        //                        //        //        else if (item.IVRMCUNP_Field == "MobileNo")
        //                        //        //        {
        //                        //        //            var MobileNo = "";
        //                        //        //            MobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo.ToString();
        //                        //        //            if (item.IVRMCUNP_Length > 0)
        //                        //        //            {
        //                        //        //                int length = (int)(item.IVRMCUNP_Length);
        //                        //        //                if (item.IVRMCUNP_FromOrderFlg == "Start")
        //                        //        //                {
        //                        //        //                    username += MobileNo.Substring(0, length);
        //                        //        //                }
        //                        //        //                else if (item.IVRMCUNP_FromOrderFlg == "End")
        //                        //        //                {
        //                        //        //                    username += MobileNo.Substring((MobileNo.Length - length), length);
        //                        //        //                }

        //                        //        //            }


        //                        //        //        }

        //                        //        //        username += "M";

        //                        //        //        ImportStudentWrapperDTO response = Createlogins(guardiandetails.FirstOrDefault().AMSTG_emailid, username, data.MI_Id, "STUDENT", "STUDENT", guardiandetails.FirstOrDefault().AMSTG_GuardianPhoneNo.ToString()).Result;

        //                        //        //        guruserid = response.useridapp;
        //                        //        //        res = response.resp;
        //                        //        //        if (guruserid == 0)
        //                        //        //        {
        //                        //        //            temp.Add("guruserid", guruserid);
        //                        //        //        }
        //                        //        //        else
        //                        //        //        {
        //                        //        //            temp.Add("guruserid", 0);
        //                        //        //        }
        //                        //        //    }
        //                        //        //}


        //                        //        if (temp.Count != 0)
        //                        //        {
        //                        //            long uid = 0;
        //                        //            long fid = 0;
        //                        //            long mid = 0;
        //                        //            long gid = 0;
        //                        //            if (temp["studentid"] != 0)
        //                        //            {
        //                        //                uid = temp["studentid"];
        //                        //            }
        //                        //            //if (temp["Fatherid"] != 0)
        //                        //            //{
        //                        //            //    fid = temp["Fatherid"];
        //                        //            //}
        //                        //            //if (temp["motherid"] != 0)
        //                        //            //{
        //                        //            //    mid = temp["motherid"];
        //                        //            //}
        //                        //            //if (temp["guruserid"] != 0)
        //                        //            //{
        //                        //            //    gid = temp["guruserid"];
        //                        //            //}

        //                        //            bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
        //                        //            if (vall == true)
        //                        //            {
        //                        //                counts = counts + 1;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                countf = countf + 1;
        //                        //            }
        //                        //        }
        //                        //    }

        //                        //    else if (credentials_check.FirstOrDefault().IVRMGC_UserNameOptionsFlg == "EmailId")
        //                        //    {

        //                        //        if (credentials_check.FirstOrDefault().IVRMGC_StudentLoginCred == true)
        //                        //        {
        //                        //            if ((studDet.FirstOrDefault().AMST_emailId.ToString() != null && studDet.FirstOrDefault().AMST_emailId.ToString() != "") || (studDet.FirstOrDefault().AMST_MobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MobileNo.ToString() != ""))
        //                        //            {
        //                        //                username = "";
        //                        //                username = studDet.FirstOrDefault().AMST_emailId + "S";


        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, username, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                        //                stduserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (stduserid == 0)
        //                        //                {
        //                        //                    temp.Add("stduserid", stduserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("studentid", stduserid);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        else if (credentials_check.FirstOrDefault().IVRMGC_FatherLoginCred == true)
        //                        //        {
        //                        //            if ((studDet.FirstOrDefault().AMST_FatheremailId.ToString() != null && studDet.FirstOrDefault().AMST_FatheremailId.ToString() != "") || (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != ""))
        //                        //            {
        //                        //                username = "";
        //                        //                username = studDet.FirstOrDefault().AMST_FatheremailId + "F";


        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, username, data.MI_Id, "PARENTS", "PARENTS", studDet.FirstOrDefault().AMST_FatherMobleNo.ToString()).Result;
        //                        //                fatuserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (fatuserid == 0)
        //                        //                {
        //                        //                    temp.Add("fatuserid", fatuserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("fatuserid", fatuserid);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        else if (credentials_check.FirstOrDefault().IVRMGC_MotherLoginCred == true)
        //                        //        {



        //                        //            if ((studDet.FirstOrDefault().AMST_MotherEmailId.ToString() != null && studDet.FirstOrDefault().AMST_MotherEmailId.ToString() != "") || (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != ""))
        //                        //            {
        //                        //                username = "";
        //                        //                username = studDet.FirstOrDefault().AMST_MotherEmailId + "M";


        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, username, data.MI_Id, "PARENTS", "PARENTS", studDet.FirstOrDefault().AMST_MotherMobileNo.ToString()).Result;
        //                        //                motuserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (motuserid == 0)
        //                        //                {
        //                        //                    temp.Add("motuserid", motuserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("motuserid", motuserid);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        if (temp.Count != 0)
        //                        //        {
        //                        //            long uid = 0;
        //                        //            long fid = 0;
        //                        //            long mid = 0;
        //                        //            long gid = 0;
        //                        //            if (temp["studentid"] != 0)
        //                        //            {
        //                        //                uid = temp["studentid"];
        //                        //            }
        //                        //            if (temp["Fatherid"] != 0)
        //                        //            {
        //                        //                fid = temp["Fatherid"];
        //                        //            }
        //                        //            if (temp["motherid"] != 0)
        //                        //            {
        //                        //                mid = temp["motherid"];
        //                        //            }
        //                        //            //if (temp["guruserid"] != 0)
        //                        //            //{
        //                        //            //    gid = temp["guruserid"];
        //                        //            //}

        //                        //            bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
        //                        //            if (vall == true)
        //                        //            {
        //                        //                counts = counts + 1;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                countf = countf + 1;
        //                        //            }
        //                        //        }


        //                        //    }

        //                        //    else if (credentials_check.FirstOrDefault().IVRMGC_UserNameOptionsFlg == "MobileNo")
        //                        //    {

        //                        //        if (credentials_check.FirstOrDefault().IVRMGC_StudentLoginCred == true)
        //                        //        {
        //                        //            if ((studDet.FirstOrDefault().AMST_emailId.ToString() != null && studDet.FirstOrDefault().AMST_emailId.ToString() != "") || (studDet.FirstOrDefault().AMST_MobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MobileNo.ToString() != ""))
        //                        //            {
        //                        //                username = "";
        //                        //                username = studDet.FirstOrDefault().AMST_MobileNo + "S";


        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, username, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                        //                stduserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (stduserid == 0)
        //                        //                {
        //                        //                    temp.Add("stduserid", stduserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("studentid", 0);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        if (credentials_check.FirstOrDefault().IVRMGC_FatherLoginCred == true)
        //                        //        {
        //                        //            if ((studDet.FirstOrDefault().AMST_FatheremailId.ToString() != null && studDet.FirstOrDefault().AMST_FatheremailId.ToString() != "") || (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != ""))
        //                        //            {
        //                        //                username = "";
        //                        //                username = studDet.FirstOrDefault().AMST_FatherMobleNo + "F";


        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, username, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_FatherMobleNo.ToString()).Result;
        //                        //                fatuserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (fatuserid == 0)
        //                        //                {
        //                        //                    temp.Add("fatuserid", fatuserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("fatuserid", fatuserid);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        if (credentials_check.FirstOrDefault().IVRMGC_MotherLoginCred == true)
        //                        //        {
        //                        //            if ((studDet.FirstOrDefault().AMST_MotherEmailId.ToString() != null && studDet.FirstOrDefault().AMST_MotherEmailId.ToString() != "") || (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != ""))
        //                        //            {
        //                        //                username = "";
        //                        //                username = studDet.FirstOrDefault().AMST_MotherMobileNo + "M";


        //                        //                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, username, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MotherMobileNo.ToString()).Result;
        //                        //                motuserid = response.useridapp;
        //                        //                res = response.resp;
        //                        //                if (motuserid == 0)
        //                        //                {
        //                        //                    temp.Add("motuserid", motuserid);
        //                        //                    bool val = AddStudentUserLogin(data.MI_Id, username, studDet.FirstOrDefault().AMST_Id);
        //                        //                    if (res == "Success" && val == true)
        //                        //                    {
        //                        //                    }
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    temp.Add("studentid", 0);
        //                        //                }
        //                        //            }
        //                        //        }
        //                        //        if (temp.Count != 0)
        //                        //        {
        //                        //            long uid = 0;
        //                        //            long fid = 0;
        //                        //            long mid = 0;
        //                        //            long gid = 0;
        //                        //            if (temp["studentid"] != 0)
        //                        //            {
        //                        //                uid = temp["studentid"];
        //                        //            }
        //                        //            if (temp["Fatherid"] != 0)
        //                        //            {
        //                        //                fid = temp["Fatherid"];
        //                        //            }
        //                        //            if (temp["motherid"] != 0)
        //                        //            {
        //                        //                mid = temp["motherid"];
        //                        //            }
        //                        //            //if (temp["guruserid"] != 0)
        //                        //            //{
        //                        //            //    gid = temp["guruserid"];
        //                        //            //}

        //                        //            bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
        //                        //            if (vall == true)
        //                        //            {
        //                        //                counts = counts + 1;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                countf = countf + 1;
        //                        //            }
        //                        //        }
        //                        //    }
        //                        //}
        //                        //  else
        //                        // {
        //                        var checkotporadm = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).ToList();
        //                        if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "Admno")
        //                        {
        //                            try
        //                            {
        //                                if (checkstudent.Count() == 0)
        //                                {
        //                                    //generateOTP otp = new generateOTP();
        //                                    //string studotp = otp.GeneratePassword();

        //                                    string studotp = otpadmno;

        //                                    admno = studDet.FirstOrDefault().AMST_AdmNo;

        //                                    if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
        //                                    {
        //                                        string StudentName = virtualcode + "S" + admno.ToString();

        //                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                                        stduserid = response.useridapp;
        //                                        res = response.resp;
        //                                        if (stduserid == 0)
        //                                        {
        //                                            countincrementstudnet += 1;
        //                                            StudentName = virtualcode + "S" + admno.ToString() + countincrementstudnet;

        //                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                                            stduserid = response1.useridapp;
        //                                            res = response1.resp;
        //                                            temp.Add("studentid", stduserid);
        //                                        }
        //                                        else
        //                                        {
        //                                            temp.Add("studentid", stduserid);

        //                                        }
        //                                        bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
        //                                        if (res == "Success" && val == true)
        //                                        {
        //                                        }

        //                                    }
        //                                    else
        //                                    {
        //                                        temp.Add("studentid", 0);
        //                                    }


        //                                    if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
        //                                    {
        //                                        string fathrotp = admno;

        //                                        string fathName = virtualcode + "F" + fathrotp.ToString();

        //                                        fathName = Regex.Replace(fathName, @"\s+", "");
        //                                        if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
        //                                        {
        //                                            data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
        //                                        }
        //                                        else
        //                                        {
        //                                            data.AMST_FatherMobleNo = 0;
        //                                        }
        //                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
        //                                        fatuserid = response.useridapp;
        //                                        res = response.resp;
        //                                        if (fatuserid == 0)
        //                                        {
        //                                            // fathrotp = otp.GeneratePassword();

        //                                            //fathName = virtualcode + data.MI_Id.ToString() + "F" + fathrotp.ToString();

        //                                            countincrementfather += 1;

        //                                            fathName = virtualcode + "F" + fathrotp.ToString() + countincrementfather;

        //                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
        //                                            fatuserid = response1.useridapp;
        //                                            res = response1.resp;
        //                                            temp.Add("Fatherid", fatuserid);
        //                                        }
        //                                        else
        //                                        {
        //                                            temp.Add("Fatherid", fatuserid);
        //                                        }
        //                                        bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
        //                                        if (res == "Success" && val == true)
        //                                        {
        //                                        }
        //                                        fathrotp = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        temp.Add("Fatherid", 0);
        //                                    }
        //                                    if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
        //                                    {

        //                                        //string motherotp = otp.getFourDigitOTPMother();
        //                                        //string motherotp = otp.GeneratePassword_Mother();

        //                                        string motherotp = admno;
        //                                        // string MotherName = virtualcode + data.MI_Id.ToString() + "M" + motherotp.ToString();
        //                                        string MotherName = virtualcode + "M" + motherotp.ToString();

        //                                        // string MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + motherotp;
        //                                        MotherName = Regex.Replace(MotherName, @"\s+", "");
        //                                        if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
        //                                        {
        //                                            data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
        //                                        }
        //                                        else
        //                                        {
        //                                            data.AMST_MotherMobileNo = 0;
        //                                        }
        //                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
        //                                        motuserid = response.useridapp;
        //                                        res = response.resp;
        //                                        if (motuserid == 0)
        //                                        {
        //                                            countincrementmother += 1;

        //                                            MotherName = virtualcode + "M" + motherotp.ToString() + countincrementmother;

        //                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
        //                                            motuserid = response1.useridapp;
        //                                            res = response1.resp;
        //                                            temp.Add("motherid", motuserid);
        //                                        }
        //                                        else
        //                                        {
        //                                            temp.Add("motherid", motuserid);
        //                                        }
        //                                        bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
        //                                        if (res == "Success" && val == true)
        //                                        {
        //                                        }
        //                                        motherotp = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        temp.Add("motherid", 0);
        //                                    }
        //                                    if (temp.Count != 0)
        //                                    {
        //                                        long uid = 0;
        //                                        long fid = 0;
        //                                        long mid = 0;
        //                                        if (temp["studentid"] != 0)
        //                                        {
        //                                            uid = temp["studentid"];
        //                                        }
        //                                        if (temp["Fatherid"] != 0)
        //                                        {
        //                                            fid = temp["Fatherid"];
        //                                        }
        //                                        if (temp["motherid"] != 0)
        //                                        {
        //                                            mid = temp["motherid"];
        //                                        }

        //                                        bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
        //                                        if (vall == true)
        //                                        {
        //                                            counts = counts + 1;
        //                                        }
        //                                        else
        //                                        {
        //                                            countf = countf + 1;
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {

        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                failname += "," + admno;
        //                                countf = countf + 1;
        //                                Console.WriteLine(ex.Message);
        //                                continue;
        //                            }

        //                        }
        //                        else if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "OTP")
        //                        {
        //                            try
        //                            {
        //                                if (checkstudent.Count() == 0)
        //                                {
        //                                    generateOTP otp = new generateOTP();
        //                                    string studotp = otp.GeneratePassword();
        //                                    admno = studDet.FirstOrDefault().AMST_AdmNo;

        //                                    if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
        //                                    {
        //                                        //string studotp = otp.getFourDigitOTPFather();
        //                                        //string studotp = otp.GeneratePassword();
        //                                        //string StudentName = virtualcode + data.MI_Id.ToString() + "S" + studotp.ToString();

        //                                        string StudentName = virtualcode + "S" + studotp.ToString();

        //                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                                        stduserid = response.useridapp;
        //                                        res = response.resp;
        //                                        if (stduserid == 0)
        //                                        {
        //                                            studotp = otp.GeneratePassword();
        //                                            // studotp = otp.getFourDigitOTPFather();
        //                                            //StudentName = virtualcode + data.MI_Id.ToString() + "S" + studotp.ToString();

        //                                            StudentName = virtualcode + "S" + studotp.ToString();

        //                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                                            stduserid = response1.useridapp;
        //                                            res = response1.resp;
        //                                            if (stduserid == 0)
        //                                            {
        //                                                while (stduserid != 0)
        //                                                {
        //                                                    studotp = otp.GeneratePassword();
        //                                                    StudentName = virtualcode + "S" + studotp.ToString();
        //                                                    ImportStudentWrapperDTO response11 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
        //                                                    stduserid = response11.useridapp;
        //                                                    res = response11.resp;
        //                                                }
        //                                            }


        //                                            temp.Add("studentid", stduserid);
        //                                        }
        //                                        else
        //                                        {
        //                                            temp.Add("studentid", stduserid);

        //                                        }
        //                                        bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
        //                                        if (res == "Success" && val == true)
        //                                        {
        //                                        }

        //                                    }
        //                                    else
        //                                    {
        //                                        temp.Add("studentid", 0);
        //                                    }


        //                                    if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
        //                                    {

        //                                        // string fathrotp = otp.getFourDigitOTPFather();
        //                                        //string fathrotp = otp.GeneratePassword();
        //                                        string fathrotp = studotp;

        //                                        // string fathName = virtualcode + data.MI_Id.ToString() + "F" + fathrotp.ToString();
        //                                        string fathName = virtualcode + "F" + fathrotp.ToString();

        //                                        fathName = Regex.Replace(fathName, @"\s+", "");
        //                                        if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
        //                                        {
        //                                            data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
        //                                        }
        //                                        else
        //                                        {
        //                                            data.AMST_FatherMobleNo = 0;
        //                                        }
        //                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
        //                                        fatuserid = response.useridapp;
        //                                        res = response.resp;
        //                                        if (fatuserid == 0)
        //                                        {
        //                                            fathrotp = otp.GeneratePassword();

        //                                            //fathName = virtualcode + data.MI_Id.ToString() + "F" + fathrotp.ToString();
        //                                            fathName = virtualcode + "F" + fathrotp.ToString();

        //                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
        //                                            fatuserid = response1.useridapp;
        //                                            res = response1.resp;
        //                                            temp.Add("Fatherid", fatuserid);
        //                                        }
        //                                        else
        //                                        {
        //                                            temp.Add("Fatherid", fatuserid);
        //                                        }
        //                                        bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
        //                                        if (res == "Success" && val == true)
        //                                        {
        //                                        }
        //                                        fathrotp = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        temp.Add("Fatherid", 0);
        //                                    }
        //                                    if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
        //                                    {

        //                                        //string motherotp = otp.getFourDigitOTPMother();
        //                                        //string motherotp = otp.GeneratePassword_Mother();

        //                                        string motherotp = studotp;
        //                                        // string MotherName = virtualcode + data.MI_Id.ToString() + "M" + motherotp.ToString();
        //                                        string MotherName = virtualcode + "M" + motherotp.ToString();

        //                                        // string MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + motherotp;
        //                                        MotherName = Regex.Replace(MotherName, @"\s+", "");
        //                                        if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
        //                                        {
        //                                            data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
        //                                        }
        //                                        else
        //                                        {
        //                                            data.AMST_MotherMobileNo = 0;
        //                                        }
        //                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
        //                                        motuserid = response.useridapp;
        //                                        res = response.resp;
        //                                        if (motuserid == 0)
        //                                        {
        //                                            motherotp = otp.GeneratePassword_Mother();
        //                                            //  MotherName = virtualcode + data.MI_Id.ToString() + "M" + motherotp.ToString();

        //                                            MotherName = virtualcode + "M" + motherotp.ToString();
        //                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
        //                                            motuserid = response1.useridapp;
        //                                            res = response1.resp;
        //                                            temp.Add("motherid", motuserid);
        //                                        }
        //                                        else
        //                                        {
        //                                            temp.Add("motherid", motuserid);
        //                                        }
        //                                        bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
        //                                        if (res == "Success" && val == true)
        //                                        {
        //                                        }
        //                                        motherotp = "";
        //                                    }
        //                                    else
        //                                    {
        //                                        temp.Add("motherid", 0);
        //                                    }
        //                                    if (temp.Count != 0)
        //                                    {
        //                                        long uid = 0;
        //                                        long fid = 0;
        //                                        long mid = 0;
        //                                        if (temp["studentid"] != 0)
        //                                        {
        //                                            uid = temp["studentid"];
        //                                        }
        //                                        if (temp["Fatherid"] != 0)
        //                                        {
        //                                            fid = temp["Fatherid"];
        //                                        }
        //                                        if (temp["motherid"] != 0)
        //                                        {
        //                                            mid = temp["motherid"];
        //                                        }

        //                                        bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
        //                                        if (vall == true)
        //                                        {
        //                                            counts = counts + 1;
        //                                        }
        //                                        else
        //                                        {
        //                                            countf = countf + 1;
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {

        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                failname += "," + admno;
        //                                countf = countf + 1;
        //                                Console.WriteLine(ex.Message);
        //                                continue;
        //                            }

        //                        }
        //                        //}
        //                    }

        //                }
        //            }

        //        }
        //        else if (data.filterdata == "Alumni")
        //        {
        //            for (int kk = 0; kk < data.data_array.Count(); kk++)
        //            {
        //                try
        //                {
        //                    var amst_id = data.data_array[kk].AMST_Id;

        //                    generateOTP otp = new generateOTP();
        //                    string studotp = otp.GeneratePassword();

        //                    var studDet = _db.Alumni_M_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.ALMST_Id == amst_id).ToList();
        //                    admno = studDet.FirstOrDefault().ALMST_AdmNo;
        //                    email = studDet.FirstOrDefault().ALMST_emailId;
        //                    mobile = Convert.ToInt64(studDet.FirstOrDefault().ALMST_MobileNo);
        //                    var firstname = studDet.FirstOrDefault().ALMST_FirstName;
        //                    var middlename = studDet.FirstOrDefault().ALMST_MiddleName;
        //                    var lastname = studDet.FirstOrDefault().ALMST_LastName;
        //                    student_Name = firstname + " " + middlename + " " + lastname;


        //                    long stduserid = 0;
        //                    string res = "";
        //                    Dictionary<string, long> temp = new Dictionary<string, long>();
        //                    if (studDet.FirstOrDefault().ALMST_emailId != "" && studDet.FirstOrDefault().ALMST_emailId != null)
        //                    {
        //                        string StudentName = "ALU" + studotp.ToString();
        //                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().ALMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALMST_MobileNo.ToString()).Result;
        //                        stduserid = response.useridapp;
        //                        res = response.resp;
        //                        if (stduserid == 0)
        //                        {
        //                            studotp = otp.GeneratePassword();
        //                            StudentName = "ALU" + studotp.ToString();
        //                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().ALMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALMST_MobileNo.ToString()).Result;
        //                            stduserid = response1.useridapp;
        //                            res = response1.resp;
        //                            temp.Add("studentid", stduserid);
        //                        }
        //                        else
        //                        {
        //                            temp.Add("studentid", stduserid);

        //                            AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();
        //                            Alumni.MI_Id = studDet.FirstOrDefault().MI_Id;
        //                            Alumni.ALSREG_Photo = studDet.FirstOrDefault().ALMST_StudentPhoto;
        //                            Alumni.ALSREG_ApprovedFlag = true;
        //                            Alumni.ALSREG_MemberName = studDet.FirstOrDefault().ALMST_FirstName;
        //                            Alumni.ALSREG_EmailId = studDet.FirstOrDefault().ALMST_emailId;
        //                            Alumni.ALSREG_MobileNo = Convert.ToInt64(studDet.FirstOrDefault().ALMST_MobileNo);
        //                            Alumni.ALSREG_AdmittedYear = Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id_Join);
        //                            Alumni.ALSREG_LeftYear = Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id_Left);
        //                            Alumni.ALSREG_LeftClass = Convert.ToInt64(studDet.FirstOrDefault().ASMCL_Id_Left);
        //                            Alumni.ALSREG_AdmittedClass = Convert.ToInt64(studDet.FirstOrDefault().ASMCL_Id_Join);
        //                            Alumni.ALSREG_Date = DateTime.Now;
        //                            Alumni.CreatedDate = DateTime.Now;
        //                            Alumni.UpdatedDate = DateTime.Now;
        //                            Alumni.ALMST_Id = studDet.FirstOrDefault().ALMST_Id;
        //                            Alumni.ALSREG_CreatedBy = stduserid;
        //                            Alumni.ALSREG_UpdatedBy = stduserid;
        //                            Alumni.ALSREG_ActiveFlg = true;
        //                            _db.Add(Alumni);
        //                            _db.SaveChanges();

        //                            Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
        //                            alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
        //                            alumniluser.IVRMUL_Id = stduserid;
        //                            _db.Add(alumniluser);
        //                            var sv = _db.SaveChanges();



        //                        }
        //                        var TemplateName = "AlumniUser";
        //                        var username = StudentName;
        //                        usermailtrigger(username, studotp, email, mobile, data.MI_Id, TemplateName, student_Name);
        //                        sendSalesSms(username, TemplateName, data.MI_Id, student_Name, mobile);

        //                    }
        //                    else
        //                    {
        //                        temp.Add("studentid", 0);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    failname += "," + admno;
        //                    countf = countf + 1;
        //                    Console.WriteLine(ex.Message);
        //                    continue;
        //                }
        //            }

        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    if (counts == data.data_array.Count())
        //    {
        //        data.message = "Record Saved";
        //    }
        //    else
        //    {
        //        if (failname != "")
        //        {
        //            data.message = "Failed Record " + admno + "";
        //        }
        //        else
        //        {
        //            data.message = "Record Saved";
        //        }
        //    }

        //    return data.message;
        //}


        //Newly added Ganesh    Numbering 

        public string createusername(SMSEmailSendDTO data)
        {
            string failname = "";
            string admno = "";
            string TransNo = "";
            string email = "";
            long mobile = 0;
            string student_Name = "";
            int counts = 0;
            int countf = 0;
            string otpadmno = "";

            try
            {
                if (data.filterdata != "Alumni")
                {
                    var credentials_check = _db.GenConfig.Where(s => s.MI_Id == data.MI_Id).ToList();
                    var virtualcode = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).Select(t => t.ivrm_school_code).FirstOrDefault();

                    for (int kk = 0; kk < data.data_array.Count(); kk++)
                    {
                        var username = "";
                        var name_str = "";
                        var mobileno_str = "";
                        var admno_str = "";
                        var reg_str = "";
                        var email_str = "";

                        long stduserid = 0;
                        long fatuserid = 0;
                        long motuserid = 0;
                        long guruserid = 0;
                        string res = "";
                        long NxtNumbering = 0;
                        long n = 1;

                        long countincrementstudnet = 0;
                        long countincrementfather = 0;
                        long countincrementmother = 0;

                        var amst_id = data.data_array[kk].AMST_Id;
                        var checkstudent = (from a in _db.StudentAppUserLoginDMO
                                            where a.AMST_ID == amst_id
                                            select new SMSEmailSendDTO
                                            {
                                                AMST_Id = a.AMST_ID
                                            }).ToList();
                        if (checkstudent.Count() == 0)
                        {
                            var check_custom = _db.IVRM_Custom_UserName_PasswordDMO.Where(d => d.MI_Id == data.MI_Id).OrderBy(f => f.IVRMCUNP_Order).ToList();
                            var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amst_id).ToList();



                            var Numbering = _db.Master_Numbering.Single(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "SPwdGenration");

                            Dictionary<string, long> temp = new Dictionary<string, long>();
                            if (check_custom.Count() > 0)
                            {

                                var checkotporadm = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).ToList();
                                if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "Admno")
                                {

                                    try
                                    {
                                        if (checkstudent.Count() == 0)
                                        {


                                            string studotp = otpadmno;

                                            admno = studDet.FirstOrDefault().AMST_AdmNo;


                                            if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
                                            {
                                                string StudentName = virtualcode + "S" + admno.ToString();

                                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                                stduserid = response.useridapp;
                                                res = response.resp;
                                                if (stduserid == 0)
                                                {
                                                    countincrementstudnet += 1;
                                                    StudentName = virtualcode + "S" + admno.ToString() + countincrementstudnet;

                                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                                    stduserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("studentid", stduserid);
                                                }
                                                else
                                                {
                                                    temp.Add("studentid", stduserid);

                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }

                                            }
                                            else
                                            {
                                                temp.Add("studentid", 0);
                                            }


                                            if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
                                            {
                                                string fathrotp = admno;

                                                string fathName = virtualcode + "F" + fathrotp.ToString();

                                                fathName = Regex.Replace(fathName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
                                                {
                                                    data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
                                                }
                                                else
                                                {
                                                    data.AMST_FatherMobleNo = 0;
                                                }
                                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                                fatuserid = response.useridapp;
                                                res = response.resp;
                                                if (fatuserid == 0)
                                                {


                                                    countincrementfather += 1;

                                                    fathName = virtualcode + "F" + fathrotp.ToString() + countincrementfather;

                                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                                    fatuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }
                                                fathrotp = "";
                                            }
                                            else
                                            {
                                                temp.Add("Fatherid", 0);
                                            }
                                            if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
                                            {



                                                string motherotp = admno;
                                                string MotherName = virtualcode + "M" + motherotp.ToString();

                                                MotherName = Regex.Replace(MotherName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
                                                {
                                                    data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
                                                }
                                                else
                                                {
                                                    data.AMST_MotherMobileNo = 0;
                                                }
                                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                                motuserid = response.useridapp;
                                                res = response.resp;
                                                if (motuserid == 0)
                                                {
                                                    countincrementmother += 1;

                                                    MotherName = virtualcode + "M" + motherotp.ToString() + countincrementmother;

                                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                                    motuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("motherid", motuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("motherid", motuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
                                                if (res == "Success" && val == true)
                                                {
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
                                                if (vall == true)
                                                {
                                                    counts = counts + 1;
                                                }
                                                else
                                                {
                                                    countf = countf + 1;
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        failname += "," + admno;
                                        countf = countf + 1;
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }


                                }
                                else if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "OTP")
                                {
                                    try
                                    {
                                        if (checkstudent.Count() == 0)
                                        {
                                            generateOTP otp = new generateOTP();
                                            string studotp = otp.GeneratePassword();
                                            admno = studDet.FirstOrDefault().AMST_AdmNo;

                                            TransNo = Numbering.IMN_StartingNo;

                                            if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
                                            {
                                                string StudentName = "";


                                                if (stduserid == 0)
                                                {

                                                    while (stduserid == 0)
                                                    {
                                                        NxtNumbering = Convert.ToInt64(TransNo) + 1;

                                                        TransNo = "00000" + NxtNumbering.ToString();

                                                        TransNo = TransNo.Substring(TransNo.Length - 5);

                                                        studotp = TransNo;
                                                        StudentName = virtualcode + "S" + studotp.ToString();
                                                        ImportStudentWrapperDTO response11 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                                        stduserid = response11.useridapp;
                                                        res = response11.resp;


                                                    }

                                                    Numbering.IMN_StartingNo = NxtNumbering.ToString();
                                                    _db.Update(Numbering);
                                                    int K = _db.SaveChanges();

                                                    temp.Add("studentid", stduserid);
                                                }
                                                else
                                                {
                                                    temp.Add("studentid", stduserid);

                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }

                                            }
                                            else
                                            {
                                                temp.Add("studentid", 0);
                                            }


                                            if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
                                            {

                                                string fathrotp = studotp;

                                                string fathName = virtualcode + "F" + fathrotp.ToString();

                                                fathName = Regex.Replace(fathName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
                                                {
                                                    data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
                                                }
                                                else
                                                {
                                                    data.AMST_FatherMobleNo = 0;
                                                }
                                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                                fatuserid = response.useridapp;
                                                res = response.resp;
                                                if (fatuserid == 0)
                                                {
                                                    fathrotp = otp.GeneratePassword();

                                                    fathName = virtualcode + "F" + fathrotp.ToString();

                                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                                    fatuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }
                                                fathrotp = "";
                                            }
                                            else
                                            {
                                                temp.Add("Fatherid", 0);
                                            }
                                            if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
                                            {



                                                string motherotp = studotp;
                                                string MotherName = virtualcode + "M" + motherotp.ToString();

                                                MotherName = Regex.Replace(MotherName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
                                                {
                                                    data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
                                                }
                                                else
                                                {
                                                    data.AMST_MotherMobileNo = 0;
                                                }
                                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                                motuserid = response.useridapp;
                                                res = response.resp;
                                                if (motuserid == 0)
                                                {
                                                    motherotp = otp.GeneratePassword_Mother();

                                                    MotherName = virtualcode + "M" + motherotp.ToString();
                                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                                    motuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("motherid", motuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("motherid", motuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
                                                if (res == "Success" && val == true)
                                                {
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
                                                if (vall == true)
                                                {
                                                    counts = counts + 1;
                                                }
                                                else
                                                {
                                                    countf = countf + 1;
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        failname += "," + admno;
                                        countf = countf + 1;
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }

                                }
                                //}
                            }

                        }
                    }

                }
                else if (data.filterdata == "Alumni")
                {
                    for (int kk = 0; kk < data.data_array.Count(); kk++)
                    {
                        try
                        {
                            var amst_id = data.data_array[kk].AMST_Id;

                            generateOTP otp = new generateOTP();
                            string studotp = otp.GeneratePassword();

                            var studDet = _db.Alumni_M_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.ALMST_Id == amst_id).ToList();
                            admno = studDet.FirstOrDefault().ALMST_AdmNo;
                            email = studDet.FirstOrDefault().ALMST_emailId;
                            mobile = Convert.ToInt64(studDet.FirstOrDefault().ALMST_MobileNo);
                            var firstname = studDet.FirstOrDefault().ALMST_FirstName;
                            var middlename = studDet.FirstOrDefault().ALMST_MiddleName;
                            var lastname = studDet.FirstOrDefault().ALMST_LastName;
                            student_Name = firstname + " " + middlename + " " + lastname;


                            long stduserid = 0;
                            string res = "";
                            Dictionary<string, long> temp = new Dictionary<string, long>();
                            if (studDet.FirstOrDefault().ALMST_emailId != "" && studDet.FirstOrDefault().ALMST_emailId != null)
                            {
                                string StudentName = "ALU" + studotp.ToString();
                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().ALMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALMST_MobileNo.ToString()).Result;
                                stduserid = response.useridapp;
                                res = response.resp;
                                if (stduserid == 0)
                                {
                                    studotp = otp.GeneratePassword();
                                    StudentName = "ALU" + studotp.ToString();
                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().ALMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALMST_MobileNo.ToString()).Result;
                                    stduserid = response1.useridapp;
                                    res = response1.resp;
                                    temp.Add("studentid", stduserid);
                                }
                                else
                                {
                                    temp.Add("studentid", stduserid);

                                    AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();
                                    Alumni.MI_Id = studDet.FirstOrDefault().MI_Id;
                                    Alumni.ALSREG_Photo = studDet.FirstOrDefault().ALMST_StudentPhoto;
                                    Alumni.ALSREG_ApprovedFlag = true;
                                    Alumni.ALSREG_MemberName = studDet.FirstOrDefault().ALMST_FirstName;
                                    Alumni.ALSREG_EmailId = studDet.FirstOrDefault().ALMST_emailId;
                                    Alumni.ALSREG_MobileNo = Convert.ToInt64(studDet.FirstOrDefault().ALMST_MobileNo);
                                    Alumni.ALSREG_AdmittedYear = Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id_Join);
                                    Alumni.ALSREG_LeftYear = Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id_Left);
                                    Alumni.ALSREG_LeftClass = Convert.ToInt64(studDet.FirstOrDefault().ASMCL_Id_Left);
                                    Alumni.ALSREG_AdmittedClass = Convert.ToInt64(studDet.FirstOrDefault().ASMCL_Id_Join);
                                    Alumni.ALSREG_Date = DateTime.Now;
                                    Alumni.CreatedDate = DateTime.Now;
                                    Alumni.UpdatedDate = DateTime.Now;
                                    Alumni.ALMST_Id = studDet.FirstOrDefault().ALMST_Id;
                                    Alumni.ALSREG_CreatedBy = stduserid;
                                    Alumni.ALSREG_UpdatedBy = stduserid;
                                    Alumni.ALSREG_ActiveFlg = true;
                                    _db.Add(Alumni);
                                    _db.SaveChanges();

                                    Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
                                    alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
                                    alumniluser.IVRMUL_Id = stduserid;
                                    _db.Add(alumniluser);
                                    var sv = _db.SaveChanges();



                                }
                                var TemplateName = "AlumniUser";
                                var username = StudentName;
                                usermailtrigger(username, studotp, email, mobile, data.MI_Id, TemplateName, student_Name);
                                sendSalesSms(username, TemplateName, data.MI_Id, student_Name, mobile);

                            }
                            else
                            {
                                temp.Add("studentid", 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            failname += "," + admno;
                            countf = countf + 1;
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
            if (counts == data.data_array.Count())
            {
                data.message = "Record Saved";
            }
            else
            {
                if (failname != "")
                {
                    data.message = "Failed Record " + admno + "";
                }
                else
                {
                    data.message = "Record Saved";
                }
            }

            return data.message;
        }


        public bool AddStudentApplogin(long userid, long fatherid, long motherid, long amstId)
        {
            StudentAppUserLoginDMO dmo = new StudentAppUserLoginDMO();
            dmo.AMST_ID = amstId;
            dmo.STD_APP_ID = Convert.ToInt32(userid);
            dmo.FAT_APP_ID = Convert.ToInt32(fatherid);
            dmo.MOT_APP_ID = Convert.ToInt32(motherid);

            _db.Add(dmo);
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
            _db.Add(dmo);
            var flag = _db.SaveChanges();
            if (flag > 0)
            {
                StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
                inst.AMST_Id = amstId;
                inst.CreatedDate = DateTime.Now;
                inst.IVRMSTUULI_ActiveFlag = 1;
                inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
                inst.UpdatedDate = DateTime.Now;
                _db.Add(inst);
                var flag1 = _db.SaveChanges();
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
                        var id = _db.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = roletype;
                        var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);


                        _db.SaveChanges();
                        respdto.useridapp = role.UserId;
                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        mas1.Activeflag = 1;
                        _db.Add(mas1);
                        var res = _db.SaveChanges();
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
                else
                {

                }

            }
            catch (Exception e)
            {
                _areaimpl.LogInformation("Student Admission form error");
                _areaimpl.LogDebug(e.Message);
            }
            return respdto;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }
        //=================email and sms trigger
        public string usermailtrigger(string username, string studotp, string email, long mobile, long MI_Id, string TemplateName, string student_Name)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();
                // val.Add("[PWD]", OSSPBOOK_Id);
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == TemplateName && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailBody;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "Alumniusename";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@UserName",
                        SqlDbType.VarChar)
                    {
                        Value = username
                    });
                    cmd.Parameters.Add(new SqlParameter("@PWD",
                        SqlDbType.BigInt)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = TemplateName
                    });

                    cmd.Parameters.Add(new SqlParameter("@StudentName",
                       SqlDbType.VarChar)
                    {
                        Value = student_Name
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
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }



                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {


                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
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

        public string sendSalesSms(string username, string TemplateName, long MI_Id, string student_Name, long mobile)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == TemplateName && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();


                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                string sms = template.FirstOrDefault().ISES_SMSMessage;


                string result = sms;





                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "Alumniusename";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@UserName",
                        SqlDbType.VarChar)
                    {
                        Value = username
                    });
                    cmd.Parameters.Add(new SqlParameter("@PWD",
                        SqlDbType.BigInt)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = TemplateName
                    });

                    cmd.Parameters.Add(new SqlParameter("@StudentName",
                       SqlDbType.VarChar)
                    {
                        Value = student_Name
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
                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                            sms = result;
                        }
                    }
                }
                sms = result;



                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobile.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == TemplateName && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
