using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
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
    public class CollegeAdmssionCancelProcessImpl : Interface.CollegeAdmssionCancelProcessInterface
    {

        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;
        ILogger<CollegeUsernameCreationImpl> _log;
        private readonly UserManager<ApplicationUser> _UserManager;
        public CollegeAdmssionCancelProcessImpl(ClgAdmissionContext clgadmctxt, DomainModelMsSqlServerContext db, ILogger<CollegeUsernameCreationImpl> log, UserManager<ApplicationUser> UserManager)
        {
            _clgadmctxt = clgadmctxt;
            _db = db;
            _log = log;
            _UserManager = UserManager;
        }
        public CollegeAdmssionCancelProcessDTO getalldetails(CollegeAdmssionCancelProcessDTO data)
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
        public CollegeAdmssionCancelProcessDTO onyearchange(CollegeAdmssionCancelProcessDTO data)
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
        public CollegeAdmssionCancelProcessDTO onCoursechange(CollegeAdmssionCancelProcessDTO data)
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
        public CollegeAdmssionCancelProcessDTO onBranchchange(CollegeAdmssionCancelProcessDTO data)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeAdmssionCancelProcessDTO onSemchange(CollegeAdmssionCancelProcessDTO data)
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
        public CollegeAdmssionCancelProcessDTO get_Studentdetails(CollegeAdmssionCancelProcessDTO data)
        {
            try
            {
                var studentlist = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                                   from c in _clgadmctxt.MasterCourseDMO
                                   from d in _clgadmctxt.ClgMasterBranchDMO
                                   from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                   from g in _clgadmctxt.AcademicYear
                                   where a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id
                                    && a.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id
                                   && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true
                                   && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id && a.AMB_Id == data.AMB_Id
                                   && a.ASMAY_Id == data.ASMAY_Id
                                   select new CollegeAdmssionCancelProcessDTO
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                       studentName = a.AMCST_FirstName + (string.IsNullOrEmpty(a.AMCST_FirstName) || a.AMCST_MiddleName == "0" ? "" : ' ' + a.AMCST_MiddleName) + (string.IsNullOrEmpty(a.AMCST_LastName) || a.AMCST_LastName == "0" ? "" : ' ' + a.AMCST_LastName),
                                       AMCST_Admno = a.AMCST_AdmNo,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMCO_CourseName = c.AMCO_CourseName,
                                       AMSE_SEMName = e.AMSE_SEMName,

                                   }).Distinct().OrderBy(t => t.studentName).ToList();
                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeAdmssionCancelProcessDTO saveatt(CollegeAdmssionCancelProcessDTO data)
        {
            try
            {
                var confirmstatus = _clgadmctxt.Database.ExecuteSqlCommand("College_Admission_Cancel_Process_Save_StudentDetails_WithFee @p0,@p1,@p2,@p3 ,@p4,@p5,@p6,@p7,@p8,@p9",data.MI_Id ,data.ASMAY_Id ,data.AMCO_Id,data.AMB_Id,data.AMSE_Id,data.AMCST_Id, data.refundper,data.cancelper,data.reason,data.userid);

                if (confirmstatus > 0)
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
                data.returnval = false;
                _log.LogInformation("College Admission Cancel Process Save : " + ex.Message);
            }
            return data;
        }
        public CollegeAdmssionCancelProcessDTO getStudentdetails(CollegeAdmssionCancelProcessDTO data)
        {
            try
            {
                var getdoa = (from a in _clgadmctxt.Adm_Master_College_StudentDMO
                              where (a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true)
                              select new CollegeAdmssionCancelProcessDTO
                              {
                                  AMCST_Date = a.AMCST_Date
                              }).Distinct().ToList();




                var checksectionalltoment = _clgadmctxt.Adm_College_Yearly_StudentDMO.Where(a => a.AMCST_Id == data.AMCST_Id && a.ACYST_ActiveFlag == 1
                 /*&& a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id && a.AMB_Id == data.AMB_Id*/).ToArray();

                data.count = checksectionalltoment.Length;

                var checkaccyear = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true).ToList();

                var cancelconfigurationdetails = _clgadmctxt.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                if (cancelconfigurationdetails.FirstOrDefault().ACACC_DOAFlg == 0)
                {
                    data.opendate = getdoa.FirstOrDefault().AMCST_Date;
                    int totalDays = Convert.ToInt32((DateTime.UtcNow.Date - data.opendate.Date).TotalDays);
                    data.todays = totalDays;
                    data.cancellationtype = "Date Of Admission(DOA)";
                }
                else
                {
                    data.opendate = Convert.ToDateTime(checkaccyear.FirstOrDefault().ASMAY_From_Date);
                    int totalDays = Convert.ToInt32((DateTime.UtcNow.Date - data.opendate.Date).TotalDays);
                    data.todays = totalDays;
                    data.cancellationtype = "Date Of College Opening";
                }

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Student_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@totdays",
                    SqlDbType.VarChar)
                    {
                        Value = data.todays
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.studentdetails = retObject.ToArray();

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
    }
}
