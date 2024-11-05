using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class Lib_stu_punch_reportIMPL :Interfaces.Lib_stu_punch_reportInterface
    {
       // public FOContext _FOContext;
        public LibraryContext _context;
        public Lib_stu_punch_reportIMPL(LibraryContext _cont )
        {
            _context = _cont;
        }

        public Lib_stu_punch_reportDTO Getdetails(Lib_stu_punch_reportDTO data)
        {
            try
            {
                 data.biometricdevice = _context.FO_Biometric_DeviceDMO.Where(R =>R.MI_Id==data.MI_Id && R.FOBD_ActiveFlg == true).ToArray();

                //data.biometricdevice = (from x in _context.FO_Biometric_DeviceDMO
                //                        where (x.MI_Id == data.MI_Id && x.FOBD_ActiveFlg == true)
                //                        select new Lib_stu_punch_reportDTO
                //                        {
                //                            biometricname = x.FOBD_DeviceName,
                //                            FOBD_Id=x.FOBD_Id
                //                        }).ToArray();



                data.getyearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //TO get the class names
        public Lib_stu_punch_reportDTO get_classes(Lib_stu_punch_reportDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.IVRM_Role_Type
                                      where (a.IVRMRT_Id == data.Roleid)
                                      select new Lib_stu_punch_reportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new Lib_stu_punch_reportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                //data.getclasslist = (from i in _context.AcademicYearDMO
                //                     from j in _context.Adm_School_M_ClassDMO
                //                     where i.MI_Id == j.MI_Id && i.ASMAY_ActiveFlag == 1 && i.MI_Id == data.MI_Id
                //                     select new Lib_stu_punch_reportDTO
                //                     {
                //                         classname = j.ASMCL_ClassName,
                //                         ASMCLId = j.ASMCL_Id
                //                     }).ToArray();
                data.getclasslist = (from a in _context.Masterclasscategory
                                 from b in _context.AcademicYearDMO
                                     from c in _context.School_M_Class
                                 where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && c.ASMCL_ActiveFlag == true
                                 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                 select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO get_sections(Lib_stu_punch_reportDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.IVRM_Role_Type
                                      where (a.IVRMRT_Id == data.Roleid)
                                      select new Lib_stu_punch_reportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();
                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new Lib_stu_punch_reportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                data.getsectionlist = (from a in _context.Masterclasscategory
                                       from b in _context.AcademicYearDMO
                                   from c in _context.School_M_Class
                                   from d in _context.School_M_Section
                                   from e in _context.AdmSchoolMasterClassCatSec
                                       where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && d.ASMC_ActiveFlag == 1
                                   && c.ASMCL_ActiveFlag == true && a.ASMCC_Id == e.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMCCS_ActiveFlg == true
                                   && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                   select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public Lib_stu_punch_reportDTO get_report(Lib_stu_punch_reportDTO data)
        {


            try
            {
                data.columnnames = (from a in Enumerable.Range(0, (Convert.ToDateTime(data.todate) - Convert.ToDateTime(data.fromdate)).Days + 1)
                                    let columndate = Convert.ToDateTime(data.fromdate).AddDays(a)
                                    select columndate).Distinct().ToArray();


                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var x in data.Temp_AmstIds)
                    {
                        ids.Add(x.AMST_Id);
                        amstids = amstids + "," + x.AMST_Id;
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Studentwise_library_Punch_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = amstids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = data.todate
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
                        data.getstupunchreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO get_students_category_grade(Lib_stu_punch_reportDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();
                data.getstudentlist = (from a in _context.School_Adm_Y_StudentDMO
                                       from b in _context.Adm_M_Student
                                       from c in _context.AcademicYearDMO
                                       from d in _context.Adm_School_M_ClassDMO
                                       from e in _context.School_M_Section
                                       where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                       && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMAY_ActiveFlag)
                                       && sol.Contains(b.AMST_SOL) && ids.Contains(b.AMST_ActiveFlag) && b.MI_Id == data.MI_Id)
                                       select new Lib_stu_punch_reportDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName)
                                           + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName)
                                           + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)
                                           + (b.AMST_AdmNo == null ? "" : " :" + b.AMST_AdmNo)).Trim()
                                       }).Distinct().OrderBy(a => a.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO biometric_install(Lib_stu_punch_reportDTO data)
        {
            try
            {
                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {

                    object B_DTO = new FO_Emp_PunchDTO2
                    {
                        MI_Id = data.temp1[i].MI_Id.ToString(),
                        HRME_BiometricCode = data.temp1[i].HRME_BiometricCode.ToString(),
                        FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                        FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm")
                    };
                    arrtest.Add(B_DTO);

                }
                var item = new
                {
                    temp1 = arrtest
                };
               
                data.MI_Id = data.temp1[0].MI_Id;
                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "_" + data.MI_Id;
                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {
                        //fetch amst_id based on biometric_id
                        var amstid = _context.Adm_M_Student.Where(d => d.AMST_BiometricId == data.temp1[i].HRME_BiometricCode && d.AMST_ActiveFlag == 1 && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.AMST_Id).FirstOrDefault();
                        if (amstid > 0)
                        {
                            //fetch the puncth date if exist for that student
                            var existdate = _context.Adm_Student_PunchDMO.Where(d => d.AMST_Id == amstid && d.ASPU_PunchDate.Value.ToString("dd/MM/yyyy") == data.temp1[i].FOEP_PunchDate.Value.ToString("dd/MM/yyyy") && d.MI_Id == data.temp1[i].MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
                            if (existdate.Count == 0)
                            {
                                Adm_Student_PunchDMO dmo = new Adm_Student_PunchDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.ASPU_ActiveFlg = true;
                                dmo.ASPU_PunchDate = data.temp1[i].FOEP_PunchDate;
                                dmo.AMST_Id = amstid;
                                dmo.ASPU_ManualEntryFlg = false;
                                dmo.MI_Id = data.temp1[i].MI_Id;
                                _context.Add(dmo);
                                _context.SaveChanges();

                                Adm_Student_Punch_DetailsDMO aspd = new Adm_Student_Punch_DetailsDMO();
                                aspd.MI_Id = data.temp1[i].MI_Id;
                                aspd.ASPU_Id = dmo.ASPU_Id;
                                aspd.ASPUD_InOutFlg = "I";
                                aspd.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                _context.Add(aspd);
                                var flag = _context.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }
                            else if (existdate.Count > 0)
                            {
                                //check the replica of the punch time 
                                List<Adm_Student_PunchDTO> punchdata = new List<Adm_Student_PunchDTO>();
                                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.CommandText = "FO_getStudentPunchDetails";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@ASPU_PunchDate", SqlDbType.DateTime)
                                    {
                                        Value = Convert.ToDateTime(data.temp1[i].FOEP_PunchDate)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASPUD_PunchTime", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(data.temp1[i].FOEPD_PunchTime)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(amstid)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(data.temp1[i].MI_Id)
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
                                                punchdata.Add(new Adm_Student_PunchDTO
                                                {
                                                    ASPUD_Id = Convert.ToInt64(dataReader["ASPUD_Id"]),
                                                    ASPUD_InOutFlg = Convert.ToString(dataReader["ASPUD_InOutFlg"]),
                                                    ASPUD_PunchTime = Convert.ToString(dataReader["ASPUD_PunchTime"]),
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                }
                                if (punchdata.Count == 0)
                                {
                                    //check the time of last punch
                                    var lastpunch = _context.Adm_Student_Punch_DetailsDMO.Where(d => d.ASPU_Id == existdate[0].ASPU_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.ASPUD_Id).ToList();
                                    if (lastpunch.Count > 0)
                                    {
                                        DateTime lastlog = Convert.ToDateTime(lastpunch.FirstOrDefault().ASPUD_PunchTime);
                                        DateTime stime1 = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime);
                                        TimeSpan diff = stime1.Subtract(lastlog);
                                        double totalMinutes = diff.TotalMinutes;
                                        //if last punch is greater than 3 MIN time diff should be greater than 3min
                                        if (totalMinutes > 3.0)
                                        {
                                            //check last punch IN or OUT
                                            if (lastpunch.FirstOrDefault().ASPUD_InOutFlg == "I")
                                            {
                                                var query15 = _context.Adm_Student_Punch_DetailsDMO.Where(d => d.ASPU_Id == existdate[0].ASPU_Id && d.MI_Id == data.temp1[i].MI_Id && d.ASPUD_PunchTime == data.temp1[i].FOEPD_PunchTime.Substring(0, 5).ToString()).ToList();
                                                if (query15.Count == 0)
                                                {
                                                    Adm_Student_Punch_DetailsDMO aspd = new Adm_Student_Punch_DetailsDMO();
                                                    aspd.MI_Id = data.temp1[i].MI_Id;
                                                    aspd.ASPU_Id = lastpunch.FirstOrDefault().ASPU_Id;
                                                    aspd.ASPUD_InOutFlg = "O";
                                                    aspd.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                    _context.Add(aspd);
                                                }
                                                var flag = _context.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                            else if (lastpunch.FirstOrDefault().ASPUD_InOutFlg == "O")
                                            {
                                                Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
                                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                                dmo2.ASPU_Id = lastpunch.FirstOrDefault().ASPU_Id;
                                                dmo2.ASPUD_InOutFlg = "I";
                                                dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                _context.Add(dmo2);
                                                var flag = _context.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public class FO_Student_PunchDTO2
        {
            public string MI_Id { get; set; }
            public string ASPU_PunchDate { get; set; }
            public string ASPUD_PunchTime { get; set; }
            public string AMST_BiometricId { get; set; }
        }
        public class FO_Emp_PunchDTO2
        {
            public string MI_Id { get; set; }
            public string FOEP_PunchDate { get; set; }
            public string FOEPD_PunchTime { get; set; }
            public string HRME_BiometricCode { get; set; }
            public string HRME_RFCardId { get; set; }
            // public FO_Emp_PunchDTO2[] punch_details { get; set; }
        }
        
        public Lib_stu_punch_reportDTO get_biometric_deviceclg(Lib_stu_punch_reportDTO data)
        {
            try
            {
                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {

                    object B_DTO = new FO_Emp_PunchDTO2
                    {
                        MI_Id = data.temp1[i].MI_Id.ToString(),
                        HRME_BiometricCode = data.temp1[i].HRME_BiometricCode.ToString(),
                        FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                        FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm")
                    };
                    arrtest.Add(B_DTO);

                }
                var item = new
                {
                    temp1 = arrtest
                };

                data.MI_Id = data.temp1[0].MI_Id;
                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "_" + data.MI_Id;
                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {
                        //fetch amst_id based on biometric_id
                        var amstid = _context.Adm_Master_College_StudentDMO.Where(d => d.AMCST_BiometricId == data.temp1[i].HRME_BiometricCode && d.AMCST_ActiveFlag == true && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.AMCST_Id).FirstOrDefault();
                        if (amstid > 0)
                        {
                            //fetch the puncth date if exist for that student
                            var existdate = _context.Adm_Student_Punch_CollegeDMO.Where(d => d.AMCST_Id == amstid && d.ASPUC_PunchDate.Value.ToString("dd/MM/yyyy") == data.temp1[i].FOEP_PunchDate.Value.ToString("dd/MM/yyyy") && d.MI_Id == data.temp1[i].MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
                            if (existdate.Count == 0)
                            {
                                Adm_Student_Punch_CollegeDMO dmo = new Adm_Student_Punch_CollegeDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.ASPUC_ActiveFlg = true;
                                dmo.ASPUC_PunchDate = data.temp1[i].FOEP_PunchDate;
                                dmo.AMCST_Id = amstid;
                                dmo.ASPUC_ManualEntryFlg = false;
                                dmo.MI_Id = data.temp1[i].MI_Id;
                                _context.Add(dmo);
                                _context.SaveChanges();

                                Adm_Student_Punch_College_DetailsDMO aspd = new Adm_Student_Punch_College_DetailsDMO();
                                aspd.MI_Id = data.temp1[i].MI_Id;
                                aspd.ASPUC_Id = dmo.ASPUC_Id;
                                aspd.ASPUDC_InOutFlg = "I";
                                aspd.ASPUDC_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                _context.Add(aspd);
                                var flag = _context.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }
                            else if (existdate.Count > 0)
                            {
                                //check the replica of the punch time 
                                List<Adm_Student_College_PunchDTO> punchdata = new List<Adm_Student_College_PunchDTO>();
                                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.CommandText = "FO_getStudentPunchDetails";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@ASPU_PunchDate", SqlDbType.DateTime)
                                    {
                                        Value = Convert.ToDateTime(data.temp1[i].FOEP_PunchDate)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASPUD_PunchTime", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(data.temp1[i].FOEPD_PunchTime)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(amstid)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(data.temp1[i].MI_Id)
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
                                                punchdata.Add(new Adm_Student_College_PunchDTO
                                                {
                                                    ASPUD_Id = Convert.ToInt64(dataReader["ASPUD_Id"]),
                                                    ASPUD_InOutFlg = Convert.ToString(dataReader["ASPUD_InOutFlg"]),
                                                    ASPUD_PunchTime = Convert.ToString(dataReader["ASPUD_PunchTime"]),
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                }
                                if (punchdata.Count == 0)
                                {
                                    //check the time of last punch
                                    var lastpunch = _context.Adm_Student_Punch_College_DetailsDMO.Where(d => d.ASPUC_Id == existdate[0].ASPUC_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.ASPUDC_Id).ToList();
                                    if (lastpunch.Count > 0)
                                    {
                                        DateTime lastlog = Convert.ToDateTime(lastpunch.FirstOrDefault().ASPUDC_PunchTime);
                                        DateTime stime1 = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime);
                                        TimeSpan diff = stime1.Subtract(lastlog);
                                        double totalMinutes = diff.TotalMinutes;
                                        //if last punch is greater than 3 MIN time diff should be greater than 3min
                                        if (totalMinutes > 3.0)
                                        {
                                            //check last punch IN or OUT
                                            if (lastpunch.FirstOrDefault().ASPUDC_InOutFlg == "I")
                                            {
                                                var query15 = _context.Adm_Student_Punch_College_DetailsDMO.Where(d => d.ASPUC_Id == existdate[0].ASPUC_Id && d.MI_Id == data.temp1[i].MI_Id && d.ASPUDC_PunchTime == data.temp1[i].FOEPD_PunchTime.Substring(0, 5).ToString()).ToList();
                                                if (query15.Count == 0)
                                                {
                                                    Adm_Student_Punch_College_DetailsDMO aspd = new Adm_Student_Punch_College_DetailsDMO();
                                                    aspd.MI_Id = data.temp1[i].MI_Id;
                                                    aspd.ASPUC_Id = lastpunch.FirstOrDefault().ASPUC_Id;
                                                    aspd.ASPUDC_InOutFlg = "O";
                                                    aspd.ASPUDC_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                    _context.Add(aspd);
                                                }
                                                var flag = _context.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                            else if (lastpunch.FirstOrDefault().ASPUDC_InOutFlg == "O")
                                            {
                                                Adm_Student_Punch_College_DetailsDMO dmo2 = new Adm_Student_Punch_College_DetailsDMO();
                                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                                dmo2.ASPUC_Id = lastpunch.FirstOrDefault().ASPUC_Id;
                                                dmo2.ASPUDC_InOutFlg = "I";
                                                dmo2.ASPUDC_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                _context.Add(dmo2);
                                                var flag = _context.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        //FOR COLLEGE REPORT Adde by --ADARSh
        public Lib_stu_punch_reportDTO onloadpage(Lib_stu_punch_reportDTO data)
        {
            try
            {
                data.clg_academicyear = _context.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.semisterlist = _context.CLG_Adm_Master_SemesterDMO.Where(s => s.MI_Id == data.MI_Id && s.AMSE_ActiveFlg == true).Distinct().OrderBy(S => S.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO loadcourse(Lib_stu_punch_reportDTO data)
        {
            try
            {
                data.courselist = (from a in _context.MasterCourseDMO
                                   from b in _context.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true
                                   && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && b.ACAYC_ActiveFlag == true && b.AMCO_Id == a.AMCO_Id)
                                   select new Lib_stu_punch_reportDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName
                                   }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO loadbranch(Lib_stu_punch_reportDTO data)
        {
            try
            {
               var branchlist = (from a in _context.ClgMasterBranchDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.AMB_ActiveFlag == true
                                  && a.AMB_Id == c.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id 
                                  && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                  select new Lib_stu_punch_reportDTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_Order = a.AMB_Order,
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO loadsemester(Lib_stu_punch_reportDTO data)
        {
            try
            {
                data.semisterlist = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                     from b in _context.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from d in _context.CLG_Adm_Master_SemesterDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == c.AMSE_Id && c.ACAYCBS_ActiveFlag == true && d.AMSE_ActiveFlg == true)
                                     select new Lib_stu_punch_reportDTO
                                     {
                                         AMSE_Id = d.AMSE_Id,
                                         AMSE_SEMName = d.AMSE_SEMName,
                                         AMSE_SEMOrder = d.AMSE_SEMOrder

                                     }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO loaadsection(Lib_stu_punch_reportDTO data)
        {
            try
            {
                data.getsection = _context.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lib_stu_punch_reportDTO loadstudents(Lib_stu_punch_reportDTO data)
        {
            try
            {
                data.studentlist = (from a in _context.Adm_Master_College_StudentDMO
                                    from b in _context.Adm_College_Yearly_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.AMCST_ActiveFlag == true && a.AMCST_Id == b.AMCST_Id
                                    && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id
                                    && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id && b.ASMAY_Id==data.ASMAY_Id )
                                    select new Lib_stu_punch_reportDTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_Name = ((a.AMCST_FirstName == null && a.AMCST_FirstName == "" ? "" : a.AMCST_FirstName) +
                                        (a.AMCST_MiddleName == null && a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                        (a.AMCST_LastName == null && a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName)),
                                        ASMAY_Id = b.ASMAY_Id,
                                        MI_Id = a.MI_Id
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Lib_stu_punch_reportDTO clgpunchreport(Lib_stu_punch_reportDTO data)
        {
            try
            {
                data.columnnames = (from a in Enumerable.Range(0, (Convert.ToDateTime(data.todate) - Convert.ToDateTime(data.fromdate)).Days + 1)
                                    let columndate = Convert.ToDateTime(data.fromdate).AddDays(a)
                                    select columndate).Distinct().ToArray();

                string AMCST_Id = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmcstIds != null && data.Temp_AmcstIds.Length > 0)
                {
                    foreach (var x in data.Temp_AmcstIds)
                    {
                        ids.Add(x.AMCST_Id);
                        AMCST_Id = AMCST_Id + "," + x.AMCST_Id;
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Studentwise_library_Punch_Details_College";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar)
                    {
                        Value = AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = data.todate
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
                        data.clgstupunchreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
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
