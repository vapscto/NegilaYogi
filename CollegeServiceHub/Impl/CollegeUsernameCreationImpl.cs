using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeUsernameCreationImpl : Interface.CollegeUsernameCreationInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;
        ILogger<CollegeUsernameCreationImpl> _log;
        private readonly UserManager<ApplicationUser> _UserManager;
        public CollegeUsernameCreationImpl(ClgAdmissionContext clgadmctxt, DomainModelMsSqlServerContext db, ILogger<CollegeUsernameCreationImpl> log, UserManager<ApplicationUser> UserManager)
        {
            _clgadmctxt = clgadmctxt;
            _db = db;
            _log = log;
            _UserManager = UserManager;
        }

        public CollegeUsernameCreationDTO getalldetails(CollegeUsernameCreationDTO data)
        {
            try
            {
                data.yearlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeUsernameCreationDTO onyearchange(CollegeUsernameCreationDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   from b in _clgadmctxt.MasterCourseDMO
                                   from c in _clgadmctxt.AcademicYear
                                   where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id
                                   && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                   && c.Is_Active == true)
                                   select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeUsernameCreationDTO onCoursechange(CollegeUsernameCreationDTO data)
        {
            try
            {
                data.branchlist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   from b in _clgadmctxt.MasterCourseDMO
                                   from c in _clgadmctxt.AcademicYear
                                   from d in _clgadmctxt.ClgMasterBranchDMO
                                   from e in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                   where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && a.ASMAY_Id == c.ASMAY_Id
                                   && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                   && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.Is_Active == true && e.ACAYCB_ActiveFlag == true
                                   && d.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id)
                                   select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeUsernameCreationDTO onBranchchange(CollegeUsernameCreationDTO data)
        {
            try
            {
                if (data.AMB_Id == 0)
                {
                    data.semesterlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }
                else
                {
                    data.semesterlist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                         from b in _clgadmctxt.MasterCourseDMO
                                         from c in _clgadmctxt.AcademicYear
                                         from d in _clgadmctxt.ClgMasterBranchDMO
                                         from e in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                         from f in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                         from g in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                         where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == e.ACAYC_Id && f.ACAYCB_Id == e.ACAYCB_Id && d.AMB_Id == e.AMB_Id
                                         && a.ASMAY_Id == c.ASMAY_Id && g.AMSE_Id == f.AMSE_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id
                                         && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                         && c.Is_Active == true && e.ACAYCB_ActiveFlag == true && d.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id
                                         && e.AMB_Id == data.AMB_Id && f.ACAYCBS_ActiveFlag == true && g.AMSE_ActiveFlg == true)
                                         select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeUsernameCreationDTO onSemchange(CollegeUsernameCreationDTO data)
        {
            try
            {
                data.sectionlist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeUsernameCreationDTO get_Studentdetails(CollegeUsernameCreationDTO data)
        {
            try
            {
                List<long> branchid = new List<long>();
                List<long> semesterid = new List<long>();

                if (data.AMB_Id == 0)
                {
                    var branchlist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                      from b in _clgadmctxt.MasterCourseDMO
                                      from c in _clgadmctxt.AcademicYear
                                      from d in _clgadmctxt.ClgMasterBranchDMO
                                      from e in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && a.ASMAY_Id == c.ASMAY_Id
                                      && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                      && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.Is_Active == true && e.ACAYCB_ActiveFlag == true
                                      && d.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id)
                                      select d).Distinct().OrderBy(a => a.AMB_Order).ToList();
                    for (int k = 0; k < branchlist.Count(); k++)
                    {
                        branchid.Add(branchlist[k].AMB_Id);
                    }
                }
                else
                {
                    var branchlist1 = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                       from b in _clgadmctxt.MasterCourseDMO
                                       from c in _clgadmctxt.AcademicYear
                                       from d in _clgadmctxt.ClgMasterBranchDMO
                                       from e in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && a.ASMAY_Id == c.ASMAY_Id
                                       && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                       && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.Is_Active == true && e.ACAYCB_ActiveFlag == true
                                       && d.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id && e.AMB_Id == data.AMB_Id)
                                       select d).Distinct().OrderBy(a => a.AMB_Order).ToList();
                    for (int k = 0; k < branchlist1.Count(); k++)
                    {
                        branchid.Add(branchlist1[k].AMB_Id);
                    }
                }

                if (data.AMSE_Id == 0)
                {
                    var semesterlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).ToList();

                    for (int k = 0; k < semesterlist.Count(); k++)
                    {
                        semesterid.Add(semesterlist[k].AMSE_Id);
                    }
                }
                else
                {
                    var semesterlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true && a.AMSE_Id == data.AMSE_Id).ToList();

                    for (int k = 0; k < semesterlist.Count(); k++)
                    {
                        semesterid.Add(semesterlist[k].AMSE_Id);
                    }
                }


                string branchids = "0";
                if (branchid.Count > 0)
                {
                    foreach (var ue in branchid)
                    {
                        branchids = branchids + "," + ue;
                    }
                }
                else
                {
                    branchids = data.AMB_Id.ToString();
                }
                string semids = "0";
                if (semesterid.Count > 0)
                {
                    foreach (var ue in semesterid)
                    {
                        semids = semids + "," + ue;
                    }
                }
                else
                {
                    semids = data.AMSE_Id.ToString();
                }



                if (data.ACMS_Id == 0)
                {
                    if (data.Studenttype == "Student")
                    {
                        //var studentlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                        //                   from b in _clgadmctxt.Adm_College_Yearly_StudentDMO
                        //                   from c in _clgadmctxt.MasterCourseDMO
                        //                   from d in _clgadmctxt.ClgMasterBranchDMO
                        //                   from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                        //                   from f in _clgadmctxt.Adm_College_Master_SectionDMO
                        //                   from g in _clgadmctxt.AcademicYear
                        //                   where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                        //                   && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                        //                   && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                        //                   && b.AMCO_Id == data.AMCO_Id && semesterid.Contains(b.AMSE_Id) && branchid.Contains(b.AMB_Id) && b.ASMAY_Id == data.ASMAY_Id
                        //                   select new CollegeUsernameCreationDTO
                        //                   {
                        //                       AMCST_Id = a.AMCST_Id,
                        //                       studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                        //                       AMCST_Admno = a.AMCST_AdmNo,
                        //                       AMB_BranchName = d.AMB_BranchName,
                        //                       AMCO_CourseName = c.AMCO_CourseName,
                        //                       AMSE_SEMName = e.AMSE_SEMName,
                        //                       ACMS_SectionName = f.ACMS_SectionName

                        //                   }).Distinct().OrderBy(t => t.studentName).ToList();




                        using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "CLGActiveStudentReportUser";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                            cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = branchids });
                            cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = semids });
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
                                data.studentlist = retObject.ToArray();
                                //if (data.studentlist.Length > 0)
                                //{
                                //    data.count = data.studentlist.Length;
                                //}
                                //else
                                //{
                                //    data.count = 0;
                                //}
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                            //if (studentlist.Count > 0)
                            //{
                            //    data.studentlist = studentlist.ToArray();
                            //}
                        }

                    }
                    else
                    {
                        //var studentlist = (from b in _clgadmctxt.CLGAlumni_M_StudentDMO
                        //                   from c in _clgadmctxt.MasterCourseDMO
                        //                   from d in _clgadmctxt.ClgMasterBranchDMO
                        //                   from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                        //                   from g in _clgadmctxt.AcademicYear
                        //                   where b.AMCO_Left_Id == c.AMCO_Id && b.AMB_Id_Left == d.AMB_Id && b.AMSE_Id_Left == e.AMSE_Id
                        //                    && b.ASMAY_Id_Left == g.ASMAY_Id && b.MI_Id == data.MI_Id
                        //                   && b.AMCO_Left_Id == data.AMCO_Id && semesterid.Contains(Convert.ToInt64(b.AMSE_Id_Left)) && branchid.Contains(Convert.ToInt64(b.AMB_Id_Left)) && b.ASMAY_Id_Left == data.ASMAY_Id
                        //                   select new CollegeUsernameCreationDTO
                        //                   {
                        //                       AMCST_Id = b.ALCMST_Id,
                        //                       studentName = b.ALCMST_FirstName + (string.IsNullOrEmpty(b.ALCMST_FirstName) || b.ALCMST_MiddleName == "0" ? "" : ' ' + b.ALCMST_MiddleName) + (string.IsNullOrEmpty(b.ALCMST_LastName) || b.ALCMST_LastName == "0" ? "" : ' ' + b.ALCMST_LastName),
                        //                       AMCST_Admno = b.ALCMST_AdmNo,
                        //                       AMB_BranchName = d.AMB_BranchName,
                        //                       AMCO_CourseName = c.AMCO_CourseName,
                        //                       AMSE_SEMName = e.AMSE_SEMName
                        //                   }).Distinct().OrderBy(t => t.studentName).ToList();
                        //if (studentlist.Count > 0)
                        //{
                        //    data.studentlist = studentlist.ToArray();
                        //}

                        try
                        {


                            //string branchids = "0";
                            //if (branchid.Count > 0)
                            //{
                            //    foreach (var ue in branchid)
                            //    {
                            //        branchids = branchids + "," + ue;
                            //    }
                            //}
                            //else
                            //{
                            //    branchids = data.AMB_Id.ToString();
                            //}
                            //string semids = "0";
                            //if (semesterid.Count > 0)
                            //{
                            //    foreach (var ue in semesterid)
                            //    {
                            //        semids = semids + "," + ue;
                            //    }
                            //}
                            //else
                            //{
                            //    semids = data.AMSE_Id.ToString();
                            //}


                            using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLGAlumniStudentReportUser";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = branchids });
                                cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = semids });
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
                                    data.studentlist = retObject.ToArray();
                                    //if (data.studentlist.Length > 0)
                                    //{
                                    //    data.count = data.studentlist.Length;
                                    //}
                                    //else
                                    //{
                                    //    data.count = 0;
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }
                else
                {
                    //var studentlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                    //                   from b in _clgadmctxt.Adm_College_Yearly_StudentDMO
                    //                   from c in _clgadmctxt.MasterCourseDMO
                    //                   from d in _clgadmctxt.ClgMasterBranchDMO
                    //                   from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                    //                   from f in _clgadmctxt.Adm_College_Master_SectionDMO
                    //                   from g in _clgadmctxt.AcademicYear
                    //                   where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                    //                   && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                    //                   && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1
                    //                   && b.AMCO_Id == data.AMCO_Id && semesterid.Contains(b.AMSE_Id) && branchid.Contains(b.AMB_Id) && b.ACMS_Id == data.ACMS_Id
                    //                   && b.ASMAY_Id == data.ASMAY_Id
                    //                   select new CollegeUsernameCreationDTO
                    //                   {
                    //                       AMCST_Id = a.AMCST_Id,
                    //                       studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                    //                       AMCST_Admno = a.AMCST_AdmNo,
                    //                       AMB_BranchName = d.AMB_BranchName,
                    //                       AMCO_CourseName = c.AMCO_CourseName,
                    //                       AMSE_SEMName = e.AMSE_SEMName,
                    //                       ACMS_SectionName = f.ACMS_SectionName

                    //                   }).Distinct().OrderBy(t => t.studentName).ToList();
                    //if (studentlist.Count > 0)
                    //{
                    //    data.studentlist = studentlist.ToArray();
                    //}


                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLGActiveStudentReportUser";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = branchids });
                        cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = semids });
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
                            data.studentlist = retObject.ToArray();
                            //if (data.studentlist.Length > 0)
                            //{
                            //    data.count = data.studentlist.Length;
                            //}
                            //else
                            //{
                            //    data.count = 0;
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                        //if (studentlist.Count > 0)
                        //{
                        //    data.studentlist = studentlist.ToArray();
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeUsernameCreationDTO saveatt(CollegeUsernameCreationDTO data)
        {
            int sucesscount = 0;
            int failcount = 0;
            string failedmsg = "";
            string otpadmno = "";
            string admno = "";

            try
            {
                var checkotporadm = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).ToList();

                var virtualcode = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).Select(t => t.ivrm_school_code).FirstOrDefault();

                otpadmno = checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO;

                if (data.Temp_Student.Length > 0)
                {
                    try
                    {
                        if (data.Studenttype == "Student")
                        {
                            if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "Admno")
                            {
                                for (int kk = 0; kk < data.Temp_Student.Count(); kk++)
                                {
                                    try
                                    {
                                        var AMCST_Id = data.Temp_Student[kk].AMCST_Id;
                                        var checkstudent = (from a in _db.CollegeStudentlogin
                                                            where a.AMCST_Id == AMCST_Id
                                                            select new CollegeUsernameCreationDTO
                                                            {
                                                                AMCST_Id = a.AMCST_Id
                                                            }).ToList();
                                        if (checkstudent.Count() == 0)
                                        {
                                            string studotp = otpadmno;
                                            var studDet = _db.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == AMCST_Id).ToList();
                                            admno = studDet.FirstOrDefault().AMCST_AdmNo;
                                            long stduserid = 0;
                                            long fatuserid = 0;
                                            long motuserid = 0;
                                            string res = "";

                                            Dictionary<string, long> temp = new Dictionary<string, long>();

                                            if (studDet.FirstOrDefault().AMCST_emailId != "" && studDet.FirstOrDefault().AMCST_emailId != null)
                                            {
                                                string StudentName = virtualcode + "S" + admno.ToString();

                                                CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                                stduserid = response.useridapp;
                                                res = response.resp;
                                                if (stduserid == 0)
                                                {
                                                    StudentName = virtualcode + "S" + admno.ToString();

                                                    CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                                    stduserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("studentid", stduserid);
                                                }
                                                else
                                                {
                                                    temp.Add("studentid", stduserid);

                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMCST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }

                                            }
                                            else
                                            {
                                                temp.Add("studentid", 0);
                                            }

                                            if (temp.Count != 0)
                                            {
                                                long uid = 0;
                                                bool vall = false;
                                                if (temp["studentid"] != 0)
                                                {
                                                    uid = temp["studentid"];



                                                    vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "S");
                                                }
                                                if (vall == true)
                                                {
                                                    sucesscount = sucesscount + 1;
                                                }
                                                else
                                                {
                                                    failcount = failcount + 1;
                                                }
                                            }

                                            if (studDet.FirstOrDefault().AMCST_FatheremailId != "" && studDet.FirstOrDefault().AMCST_FatheremailId != null)
                                            {
                                                string fathrotp = admno;

                                                string fathName = virtualcode + "F" + fathrotp.ToString();

                                                fathName = Regex.Replace(fathName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != "")
                                                {
                                                    data.AMCST_FatherMobleNo = studDet.FirstOrDefault().AMCST_FatherMobleNo;
                                                }
                                                else
                                                {
                                                    data.AMCST_FatherMobleNo = 0;
                                                }
                                                CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                                fatuserid = response.useridapp;
                                                res = response.resp;
                                                if (fatuserid == 0)
                                                {
                                                    fathName = virtualcode + "F" + fathrotp.ToString();

                                                    CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                                    fatuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMCST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }
                                                fathrotp = "";
                                            }
                                            else
                                            {
                                                temp.Add("Fatherid", 0);
                                            }

                                            if (temp.Count != 0)
                                            {
                                                long uid = 0;
                                                bool vall = false;
                                                if (temp["Fatherid"] != 0)
                                                {
                                                    uid = temp["Fatherid"];


                                                    vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "F");
                                                }
                                                if (vall == true)
                                                {
                                                    sucesscount = sucesscount + 1;
                                                }
                                                else
                                                {
                                                    failcount = failcount + 1;
                                                }
                                            }



                                            if (studDet.FirstOrDefault().AMCST_MotheremailId != "" && studDet.FirstOrDefault().AMCST_MotheremailId != null)
                                            {
                                                string motherotp = admno;
                                                string MotherName = virtualcode + "M" + motherotp.ToString();
                                                MotherName = Regex.Replace(MotherName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != "")
                                                {
                                                    data.AMCST_MotherMobleNo = studDet.FirstOrDefault().AMCST_MotherMobleNo;
                                                }
                                                else
                                                {
                                                    data.AMCST_MotherMobleNo = 0;
                                                }
                                                CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                                motuserid = response.useridapp;
                                                res = response.resp;
                                                if (motuserid == 0)
                                                {
                                                    MotherName = virtualcode + "M" + motherotp.ToString();
                                                    CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                                    motuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("motherid", motuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("motherid", motuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMCST_Id);
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
                                                bool vall = false;
                                                long uid = 0;
                                                if (temp["motherid"] != 0)
                                                {
                                                    uid = temp["motherid"];


                                                    vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "M");
                                                }
                                                if (vall == true)
                                                {
                                                    sucesscount = sucesscount + 1;
                                                }
                                                else
                                                {
                                                    failcount = failcount + 1;
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        failedmsg += "," + admno;
                                        failcount = failcount + 1;
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                            }

                            else if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "OTP")
                            {
                                for (int kk = 0; kk < data.Temp_Student.Count(); kk++)
                                {
                                    try
                                    {
                                        var AMCST_Id = data.Temp_Student[kk].AMCST_Id;
                                        var checkstudent = (from a in _db.CollegeStudentlogin
                                                            where a.AMCST_Id == AMCST_Id
                                                            select new CollegeUsernameCreationDTO
                                                            {
                                                                AMCST_Id = a.AMCST_Id
                                                            }).ToList();
                                        if (checkstudent.Count() == 0)
                                        {
                                            generateOTP otp = new generateOTP();
                                            string studotp = otp.GeneratePassword();

                                            var studDet = _db.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == AMCST_Id).ToList();
                                            admno = studDet.FirstOrDefault().AMCST_AdmNo;
                                            long stduserid = 0;
                                            long fatuserid = 0;
                                            long motuserid = 0;
                                            string res = "";
                                            Dictionary<string, long> temp = new Dictionary<string, long>();

                                            if (studDet.FirstOrDefault().AMCST_emailId != "" && studDet.FirstOrDefault().AMCST_emailId != null)
                                            {
                                                string StudentName = virtualcode + "S" + studotp.ToString();

                                                CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                                stduserid = response.useridapp;
                                                res = response.resp;
                                                if (stduserid == 0)
                                                {
                                                    studotp = otp.GeneratePassword();
                                                    StudentName = virtualcode + "S" + studotp.ToString();
                                                    CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMCST_MobileNo.ToString()).Result;
                                                    stduserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("studentid", stduserid);
                                                }
                                                else
                                                {
                                                    temp.Add("studentid", stduserid);

                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMCST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }

                                            }
                                            else
                                            {
                                                temp.Add("studentid", 0);
                                            }

                                            if (temp.Count != 0)
                                            {
                                                long uid = 0;
                                                bool vall = false;
                                                if (temp["studentid"] != 0)
                                                {
                                                    uid = temp["studentid"];


                                                    vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "S");
                                                }
                                                if (vall == true)
                                                {
                                                    sucesscount = sucesscount + 1;
                                                }
                                                else
                                                {
                                                    failcount = failcount + 1;
                                                }
                                            }


                                            if (studDet.FirstOrDefault().AMCST_FatheremailId != "" && studDet.FirstOrDefault().AMCST_FatheremailId != null)
                                            {
                                                string fathrotp = studotp;
                                                string fathName = virtualcode + "F" + fathrotp.ToString();
                                                fathName = Regex.Replace(fathName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_FatherMobleNo.ToString() != "")
                                                {
                                                    data.AMCST_FatherMobleNo = studDet.FirstOrDefault().AMCST_FatherMobleNo;
                                                }
                                                else
                                                {
                                                    data.AMCST_FatherMobleNo = 0;
                                                }
                                                CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                                fatuserid = response.useridapp;
                                                res = response.resp;
                                                if (fatuserid == 0)
                                                {
                                                    fathrotp = otp.GeneratePassword();
                                                    fathName = virtualcode + "F" + fathrotp.ToString();
                                                    CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_FatherMobleNo.ToString()).Result;
                                                    fatuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("Fatherid", fatuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMCST_Id);
                                                if (res == "Success" && val == true)
                                                {
                                                }
                                                fathrotp = "";
                                            }
                                            else
                                            {
                                                temp.Add("Fatherid", 0);
                                            }

                                            if (temp.Count != 0)
                                            {
                                                long uid = 0;
                                                bool vall = false;
                                                if (temp["Fatherid"] != 0)
                                                {
                                                    uid = temp["Fatherid"];


                                                    vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "F");
                                                }
                                                if (vall == true)
                                                {
                                                    sucesscount = sucesscount + 1;
                                                }
                                                else
                                                {
                                                    failcount = failcount + 1;
                                                }
                                            }

                                            if (studDet.FirstOrDefault().AMCST_MotheremailId != "" && studDet.FirstOrDefault().AMCST_MotheremailId != null)
                                            {
                                                string motherotp = studotp;
                                                string MotherName = virtualcode + "M" + motherotp.ToString();
                                                MotherName = Regex.Replace(MotherName, @"\s+", "");
                                                if (studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != null && studDet.FirstOrDefault().AMCST_MotherMobleNo.ToString() != "")
                                                {
                                                    data.AMCST_MotherMobleNo = studDet.FirstOrDefault().AMCST_MotherMobleNo;
                                                }
                                                else
                                                {
                                                    data.AMCST_MotherMobleNo = 0;
                                                }
                                                CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                                motuserid = response.useridapp;
                                                res = response.resp;
                                                if (motuserid == 0)
                                                {
                                                    motherotp = otp.GeneratePassword_Mother();
                                                    MotherName = virtualcode + "M" + motherotp.ToString();
                                                    CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().AMCST_MotheremailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMCST_MotherMobleNo.ToString()).Result;
                                                    motuserid = response1.useridapp;
                                                    res = response1.resp;
                                                    temp.Add("motherid", motuserid);
                                                }
                                                else
                                                {
                                                    temp.Add("motherid", motuserid);
                                                }
                                                bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMCST_Id);
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
                                                bool vall = false;
                                                if (temp["motherid"] != 0)
                                                {
                                                    uid = temp["motherid"];


                                                    vall = AddStudentApplogin(uid, studDet.FirstOrDefault().AMCST_Id, "M");
                                                }
                                                if (vall == true)
                                                {
                                                    sucesscount = sucesscount + 1;
                                                }
                                                else
                                                {
                                                    failcount = failcount + 1;
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        failedmsg += "," + admno;
                                        failcount = failcount + 1;
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (data.Studenttype == "Alumni")
                        {
                            for (int kk = 0; kk < data.Temp_Student.Count(); kk++)
                            {
                                try
                                {
                                    var AMCST_Id = data.Temp_Student[kk].AMCST_Id;
                                    var checkstudent = (from a in _db.CLGAlumniUserRegistrationDMO
                                                        where a.AMCST_Id == AMCST_Id
                                                        select new CollegeUsernameCreationDTO
                                                        {
                                                            AMCST_Id = Convert.ToInt64(a.AMCST_Id)
                                                        }).ToList();
                                    if (checkstudent.Count() == 0)
                                    {
                                        generateOTP otp = new generateOTP();
                                        string studotp = otp.GeneratePassword();

                                        List<CLGAlumni_M_StudentDMO> studDet = new List<CLGAlumni_M_StudentDMO>();
                                        studDet = _db.CLGAlumni_M_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.ALCMST_Id == AMCST_Id).ToList();

                                        admno = studDet.FirstOrDefault().ALCMST_AdmNo;
                                        long stduserid = 0;
                                        //long fatuserid = 0;
                                        //long motuserid = 0;
                                        string res = "";
                                        Dictionary<string, long> temp = new Dictionary<string, long>();

                                        //if (studDet.FirstOrDefault().ALCMST_emailId != "" && studDet.FirstOrDefault().ALCMST_emailId != null)
                                        //{
                                        string StudentName = "ALUMNI" + studotp.ToString();

                                        CollegeImportStudentWrapDTO response = Createlogins(studDet.FirstOrDefault().ALCMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALCMST_MobileNo.ToString()).Result;
                                        stduserid = response.useridapp;
                                        res = response.resp;
                                        if (stduserid == 0)
                                        {
                                            studotp = otp.GeneratePassword();
                                            StudentName = "ALUMNIA" + studotp.ToString();
                                            CollegeImportStudentWrapDTO response1 = Createlogins(studDet.FirstOrDefault().ALCMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALCMST_MobileNo.ToString()).Result;
                                            stduserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("studentid", stduserid);
                                        }
                                        else
                                        {
                                            temp.Add("studentid", stduserid);

                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().ALCMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }

                                        //}
                                        //else
                                        //{
                                        //    temp.Add("studentid", 0);
                                        //}

                                        if (temp.Count != 0)
                                        {
                                            long uid = 0;
                                            if (temp["studentid"] != 0)
                                            {
                                                uid = temp["studentid"];
                                            }

                                            bool vall = AddAlumniStudentlogin(uid, studDet.FirstOrDefault().ALCMST_Id, "S", studDet);
                                            if (vall == true)
                                            {
                                                sucesscount = sucesscount + 1;
                                            }
                                            else
                                            {
                                                failcount = failcount + 1;
                                            }
                                        }

                                    }
                                    else
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {
                                    failedmsg += "," + admno;
                                    failcount = failcount + 1;
                                    Console.WriteLine(ex.Message);
                                    continue;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        failcount = failcount + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("College User Creation : " + ex.Message);
            }
            if (sucesscount == data.Temp_Student.Count())
            {
                data.msg = "Record Saved";
            }
            else
            {
                if (failedmsg != "")
                {
                    data.msg = "Failed Record " + admno + "";
                }
                else
                {
                    data.msg = "Record Saved";
                }
            }
            return data;
        }
        public async Task<CollegeImportStudentWrapDTO> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            CollegeImportStudentWrapDTO respdto = new CollegeImportStudentWrapDTO();
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

            }
            catch (Exception e)
            {
                _log.LogInformation("Student User Creation College form error");
                _log.LogDebug(e.Message);
            }
            return respdto;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }
        public bool AddStudentUserLogin(long mi_id, string username, long amstId)
        {
            //  StudentUserLoginDMO dmo = new StudentUserLoginDMO();
            //dmo.AMST_Id = amstId;
            //dmo.CreatedDate = DateTime.Now;
            //dmo.IVRMSTUUL_ActiveFlag = 1;
            //dmo.IVRMSTUUL_Password = "Password@123";
            //dmo.IVRMSTUUL_UserName = username;
            //dmo.MI_Id = mi_id;
            //dmo.UpdatedDate = DateTime.Now;
            //_db.Add(dmo);
            //var flag = _db.SaveChanges();
            //if (flag > 0)
            //{
            //    StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
            //    inst.AMST_Id = amstId;
            //    inst.CreatedDate = DateTime.Now;
            //    inst.IVRMSTUULI_ActiveFlag = 1;
            //    inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
            //    inst.UpdatedDate = DateTime.Now;
            //    _db.Add(inst);
            //    var flag1 = _db.SaveChanges();
            //    if (flag1 > 0)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
        public bool AddStudentApplogin(long userid, long amstId, string appflag)
        {
            CollegeStudentlogin dmo = new CollegeStudentlogin();
            dmo.AMCST_Id = amstId;
            dmo.IVRMUL_Id = Convert.ToInt32(userid);
            dmo.IVRMULSPGC_ActiveFlag = true;
            dmo.IVRMULSPGC_Flag = appflag;
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

        public bool AddAlumniStudentlogin(long userid, long amstId, string appflag, List<CLGAlumni_M_StudentDMO> reg)
        {

            CLGAlumniUserRegistrationDMO Alumni = new CLGAlumniUserRegistrationDMO();

            Alumni.MI_Id = reg.FirstOrDefault().MI_Id;
            Alumni.AMCST_Id = amstId;
            Alumni.ALCSREG_Photo = reg.FirstOrDefault().ALCMST_StudentPhoto;
            Alumni.ALCSREG_ApprovedFlag = true;
            Alumni.ALCSREG_MemberName = reg.FirstOrDefault().ALCMST_FirstName;
            Alumni.ALCSREG_EmailId = reg.FirstOrDefault().ALCMST_emailId;
            Alumni.ALCSREG_MobileNo = Convert.ToInt64(reg.FirstOrDefault().ALCMST_MobileNo);
            Alumni.ALCSREG_AdmittedYear = Convert.ToInt64(reg.FirstOrDefault().ASMAY_Id_Join);
            Alumni.ALCSREG_LeftYear = Convert.ToInt64(reg.FirstOrDefault().ASMAY_Id_Left);
            Alumni.ALCSREG_LeftCourse = Convert.ToInt64(reg.FirstOrDefault().AMCO_Left_Id);
            Alumni.ALCSREG_AdmittedCourse = Convert.ToInt64(reg.FirstOrDefault().AMCO_JOIN_Id);
            Alumni.ALCSREG_AdmisstedBranch = Convert.ToInt64(reg.FirstOrDefault().AMB_JOIN_Id);
            Alumni.ALCSREG_LeftBranch = Convert.ToInt64(reg.FirstOrDefault().AMB_Id_Left);
            Alumni.ALCSREG_AdmittedSemester = Convert.ToInt64(reg.FirstOrDefault().AMSE_Id_Left);
            Alumni.ALCSREG_LeftSemester = Convert.ToInt64(reg.FirstOrDefault().AMSE_JOIN_Id);
            Alumni.ALCSREG_Date = DateTime.Now;
            Alumni.CreatedDate = DateTime.Now;
            Alumni.UpdatedDate = DateTime.Now;
            Alumni.ALCSREG_CreatedBy = userid;
            Alumni.ALCSREG_UpdatedBy = userid;
            Alumni.ALCSREG_ActiveFlg = true;
            _db.Add(Alumni);
            _db.SaveChanges();

            CLGAlumni_User_LoginDMO dmo = new CLGAlumni_User_LoginDMO();
            dmo.ALCSREG_Id = Alumni.ALCSREG_Id;
            dmo.IVRMUL_Id = Convert.ToInt32(userid);
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



        // Sending SMS And Email User Creation
        public CollegeUsernameCreationDTO getStudentusername(CollegeUsernameCreationDTO data)
        {
            try
            {
                List<long> branchid = new List<long>();
                List<long> semesterid = new List<long>();
                List<long> courseid = new List<long>();
                if (data.AMCO_Id == 0)
                {
                    var courselist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                      from b in _clgadmctxt.MasterCourseDMO
                                      from c in _clgadmctxt.AcademicYear
                                      where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id
                                      && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                      && c.Is_Active == true)
                                      select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                    for (int k = 0; k < courselist.Count(); k++)
                    {
                        courseid.Add(courselist[k].AMCO_Id);
                    }
                }
                else
                {
                    var courselist1 = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                       from b in _clgadmctxt.MasterCourseDMO
                                       from c in _clgadmctxt.AcademicYear
                                       where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id
                                       && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                       && c.Is_Active == true && b.AMCO_Id == data.AMCO_Id)
                                       select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                    for (int k = 0; k < courselist1.Count(); k++)
                    {
                        courseid.Add(courselist1[k].AMCO_Id);
                    }
                }

                if (data.AMB_Id == 0)
                {
                    var branchlist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                      from b in _clgadmctxt.MasterCourseDMO
                                      from c in _clgadmctxt.AcademicYear
                                      from d in _clgadmctxt.ClgMasterBranchDMO
                                      from e in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && a.ASMAY_Id == c.ASMAY_Id
                                      && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                      && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.Is_Active == true && e.ACAYCB_ActiveFlag == true
                                      && d.AMB_ActiveFlag == true && courseid.Contains(a.AMCO_Id))
                                      select d).Distinct().OrderBy(a => a.AMB_Order).ToList();
                    for (int k = 0; k < branchlist.Count(); k++)
                    {
                        branchid.Add(branchlist[k].AMB_Id);
                    }
                }
                else
                {
                    var branchlist1 = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                       from b in _clgadmctxt.MasterCourseDMO
                                       from c in _clgadmctxt.AcademicYear
                                       from d in _clgadmctxt.ClgMasterBranchDMO
                                       from e in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == e.ACAYC_Id && d.AMB_Id == e.AMB_Id && a.ASMAY_Id == c.ASMAY_Id
                                       && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                       && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true && c.Is_Active == true && e.ACAYCB_ActiveFlag == true
                                       && d.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id && e.AMB_Id == data.AMB_Id)
                                       select d).Distinct().OrderBy(a => a.AMB_Order).ToList();
                    for (int k = 0; k < branchlist1.Count(); k++)
                    {
                        branchid.Add(branchlist1[k].AMB_Id);
                    }
                }

                if (data.AMSE_Id == 0)
                {
                    var semesterlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).ToList();

                    for (int k = 0; k < semesterlist.Count(); k++)
                    {
                        semesterid.Add(semesterlist[k].AMSE_Id);
                    }
                }
                else
                {
                    var semesterlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true && a.AMSE_Id == data.AMSE_Id).ToList();

                    for (int k = 0; k < semesterlist.Count(); k++)
                    {
                        semesterid.Add(semesterlist[k].AMSE_Id);
                    }
                }





                if (data.ACMS_Id == 0)
                {
                    if (data.Studenttype == "Student")
                    {
                        var studentlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                                           from b in _clgadmctxt.Adm_College_Yearly_StudentDMO
                                           from c in _clgadmctxt.MasterCourseDMO
                                           from d in _clgadmctxt.ClgMasterBranchDMO
                                           from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                           from f in _clgadmctxt.Adm_College_Master_SectionDMO
                                           from g in _clgadmctxt.AcademicYear
                                           from h in _clgadmctxt.ApplUser
                                           from i in _clgadmctxt.CollegeStudentlogin
                                           from j in _clgadmctxt.Institution
                                           where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                           && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && h.Id == i.IVRMUL_Id && i.AMCST_Id == b.AMCST_Id
                                           && i.AMCST_Id == a.AMCST_Id && a.MI_Id == data.MI_Id && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true
                                           && b.ACYST_ActiveFlag == 1 && j.MI_Id == a.MI_Id && courseid.Contains(b.AMCO_Id) && semesterid.Contains(b.AMSE_Id) && branchid.Contains(b.AMB_Id)
                                           && b.ASMAY_Id == data.ASMAY_Id && i.IVRMULSPGC_Flag == "S"
                                           select new CollegeUsernameCreationDTO
                                           {
                                               AMCST_Id = a.AMCST_Id,
                                               studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                               AMCST_Admno = a.AMCST_AdmNo,
                                               ACMS_SectionName = f.ACMS_SectionName,
                                               username = h.UserName,
                                               password = "Password@123",
                                               AMCST_MobileNo = a.AMCST_MobileNo,
                                               AMCST_emailId = a.AMCST_emailId,
                                               MI_Logo = j.MI_Logo,
                                           }).Distinct().OrderBy(t => t.studentName).ToList();
                        if (studentlist.Count > 0)
                        {
                            data.studentuserdetails = studentlist.ToArray();
                        }
                    }
                    else if (data.Studenttype == "Alumni")
                    {
                        try
                        {
                            string branchids = "0";
                            if (branchid.Count > 0)
                            {
                                foreach (var ue in branchid)
                                {
                                    branchids = branchids + "," + ue;
                                }
                            }
                            else
                            {
                                branchids = data.AMB_Id.ToString();
                            }
                            string semids = "0";
                            if (semesterid.Count > 0)
                            {
                                foreach (var ue in semesterid)
                                {
                                    semids = semids + "," + ue;
                                }
                            }
                            else
                            {
                                semids = data.AMSE_Id.ToString();
                            }
                            using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLGAlumniStudentReportUserCreated";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                                cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = branchids });
                                cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = semids });
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
                                    data.studentuserdetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }

                }
                else
                {
                    var studentlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                                       from b in _clgadmctxt.Adm_College_Yearly_StudentDMO
                                       from c in _clgadmctxt.MasterCourseDMO
                                       from d in _clgadmctxt.ClgMasterBranchDMO
                                       from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                       from f in _clgadmctxt.Adm_College_Master_SectionDMO
                                       from g in _clgadmctxt.AcademicYear
                                       from h in _clgadmctxt.ApplUser
                                       from i in _clgadmctxt.CollegeStudentlogin
                                       where a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                       && b.ACMS_Id == f.ACMS_Id && b.ASMAY_Id == g.ASMAY_Id && h.Id == i.IVRMUL_Id && i.AMCST_Id == b.AMCST_Id
                                       && i.AMCST_Id == a.AMCST_Id && a.MI_Id == data.MI_Id && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true
                                       && b.ACYST_ActiveFlag == 1 && b.AMCO_Id == data.AMCO_Id && semesterid.Contains(b.AMSE_Id) && branchid.Contains(b.AMB_Id)
                                       && b.ACMS_Id == data.ACMS_Id && b.ASMAY_Id == data.ASMAY_Id && i.IVRMULSPGC_Flag == "S"
                                       select new CollegeUsernameCreationDTO
                                       {
                                           AMCST_Id = a.AMCST_Id,
                                           studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                           AMCST_Admno = a.AMCST_AdmNo,
                                           ACMS_SectionName = f.ACMS_SectionName,
                                           username = h.UserName,
                                           password = "Password@123",
                                           AMCST_MobileNo = a.AMCST_MobileNo,
                                           AMCST_emailId = a.AMCST_emailId

                                       }).Distinct().OrderBy(t => t.studentName).ToList();
                    if (studentlist.Count > 0)
                    {
                        data.studentuserdetails = studentlist.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("College User Creation  : " + ex.Message);
            }
            return data;
        }
        public async Task<CollegeUsernameCreationDTO> SendSMS(CollegeUsernameCreationDTO data)
        {
            try
            {
                int smssuccesscount = 0;
                int smsfailcount = 0;

                int emailsuccesscount = 0;
                int emailfailcount = 0;

                string smsfaildetails = "";
                string emailfaildetails = "";

                if (data.Temp_Student_SMS.Count() > 0)
                {
                    long trnsno = 0;
                    SMS sms1 = new SMS(_db);
                    Email email = new Email(_db);
                    //trnsno = sms1.getsmsno(data.MI_Id);

                    for (int k = 0; k < data.Temp_Student_SMS.Length; k++)
                    {
                        try
                        {
                            if (data.SMSFlag == true)
                            {
                                string s = await sms1.sendSms(data.MI_Id, Convert.ToInt64(data.Temp_Student_SMS[k].AMCST_MobileNo),
                                    "Student_User_Creation_Password", data.Temp_Student_SMS[k].AMCST_Id);

                                if (s.Equals("success"))
                                {
                                    smssuccesscount = smssuccesscount + 1;
                                }
                                else
                                {
                                    smsfailcount = smsfailcount + 1;
                                    smsfaildetails = smsfaildetails + "," + data.Temp_Student_SMS[k].studentName;
                                }
                            }
                            if (data.EmailFlag == true)
                            {
                                string e = email.sendmail(data.MI_Id, data.Temp_Student_SMS[k].AMCST_emailId, "Student_User_Creation_Password", data.Temp_Student_SMS[k].AMCST_Id);

                                if (e.Equals("success"))
                                {
                                    emailsuccesscount = emailsuccesscount + 1;
                                }
                                else
                                {
                                    emailfailcount = emailfailcount + 1;
                                    emailfaildetails = emailfaildetails + "," + data.Temp_Student_SMS[k].studentName;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (data.SMSFlag == true)
                    {
                        data.smsmsg = "Sucess SMS Trigger Count : " + smssuccesscount + " Failed SMS Count : " + smsfailcount;
                    }
                    if (data.EmailFlag == true)
                    {
                        data.emailmsg = "Sucess Email Trigger Count : " + emailsuccesscount + " Failed Email Count : " + emailfailcount;
                    }

                    data.msg = data.smsmsg + " " + data.emailmsg;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.LogInformation("College User Creation  : " + ex.Message);
            }
            return data;
        }


    }
}
