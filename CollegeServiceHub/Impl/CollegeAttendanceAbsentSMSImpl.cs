using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeAttendanceAbsentSMSImpl : Interface.CollegeAttendanceAbsentSMSInterface
    {


        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;
        public CollegeAttendanceAbsentSMSImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }
        public CollegeDailyAttendanceDTO getdetails(CollegeDailyAttendanceDTO data)
        {
            try
            {
                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public CollegeDailyAttendanceDTO onselectAcdYear(CollegeDailyAttendanceDTO data)
        {
            try
            {
                var check_rolename = (from a in _clgadmctxt.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeDailyAttendanceDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _clgadmctxt.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeDailyAttendanceDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    if (check_rolename.FirstOrDefault().rolename.Equals("Staff"))
                    {
                        data.courselist = (from a in _clgadmctxt.Adm_College_Atten_Login_UserDMO
                                           from b in _clgadmctxt.Adm_College_Atten_Login_DetailsDMO
                                           from c in _clgadmctxt.AcademicYear
                                           from d in _clgadmctxt.MasterCourseDMO
                                           where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                           select d).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                    }
                    else
                    {
                        data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                           from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                           where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                           select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                    }
                }
                else
                {
                    data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                       from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                       where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                       select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDailyAttendanceDTO onselectCourse(CollegeDailyAttendanceDTO data)
        {
            try
            {
                var check_rolename = (from a in _clgadmctxt.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeDailyAttendanceDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _clgadmctxt.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeDailyAttendanceDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (data.AMCO_Id == 0)
                {
                    if (empcode_check.Count > 0)
                    {
                        if (check_rolename.FirstOrDefault().rolename.Equals("Staff"))
                        {
                            data.branchlist = (from a in _clgadmctxt.Adm_College_Atten_Login_UserDMO
                                               from b in _clgadmctxt.Adm_College_Atten_Login_DetailsDMO
                                               from c in _clgadmctxt.AcademicYear
                                               from d in _clgadmctxt.MasterCourseDMO
                                               from e in _clgadmctxt.ClgMasterBranchDMO
                                               where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && e.AMB_Id == b.AMB_Id
                                               && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id
                                               && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                               select e).Distinct().OrderBy(t => t.AMB_Order).ToArray();
                        }
                        else
                        {
                            var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                              from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                              from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                              where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                              select new ClgMasterBranchDMO
                                              {
                                                  AMB_Id = a.AMB_Id,
                                                  AMB_BranchName = a.AMB_BranchName,
                                                  AMB_BranchCode = a.AMB_BranchCode,
                                                  AMB_BranchInfo = a.AMB_BranchInfo,
                                                  AMB_BranchType = a.AMB_BranchType,
                                                  AMB_StudentCapacity = a.AMB_StudentCapacity,
                                                  AMB_Order = a.AMB_Order,
                                                  AMB_AidedUnAided = a.AMB_AidedUnAided

                                              }).Distinct().ToList();
                            data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
                        }
                    }
                    else
                    {
                        var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                          from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                          from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                          where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                          select new ClgMasterBranchDMO
                                          {
                                              AMB_Id = a.AMB_Id,
                                              AMB_BranchName = a.AMB_BranchName,
                                              AMB_BranchCode = a.AMB_BranchCode,
                                              AMB_BranchInfo = a.AMB_BranchInfo,
                                              AMB_BranchType = a.AMB_BranchType,
                                              AMB_StudentCapacity = a.AMB_StudentCapacity,
                                              AMB_Order = a.AMB_Order,
                                              AMB_AidedUnAided = a.AMB_AidedUnAided

                                          }).Distinct().ToList();
                        data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
                    }
                }


                else
                {
                    if (empcode_check.Count > 0)
                    {
                        if (check_rolename.FirstOrDefault().rolename.Equals("Staff"))
                        {
                            data.branchlist = (from a in _clgadmctxt.Adm_College_Atten_Login_UserDMO
                                               from b in _clgadmctxt.Adm_College_Atten_Login_DetailsDMO
                                               from c in _clgadmctxt.AcademicYear
                                               from d in _clgadmctxt.MasterCourseDMO
                                               from e in _clgadmctxt.ClgMasterBranchDMO
                                               where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && e.AMB_Id == b.AMB_Id
                                               && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.ASMAY_Id == data.ASMAY_Id
                                               && b.AMCO_Id == data.AMCO_Id && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                               select e).Distinct().OrderBy(t => t.AMB_Order).ToArray();
                        }
                        else
                        {

                            var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                              from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                              from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                              where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                              && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                              && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                              select new ClgMasterBranchDMO
                                              {
                                                  AMB_Id = a.AMB_Id,
                                                  AMB_BranchName = a.AMB_BranchName,
                                                  AMB_BranchCode = a.AMB_BranchCode,
                                                  AMB_BranchInfo = a.AMB_BranchInfo,
                                                  AMB_BranchType = a.AMB_BranchType,
                                                  AMB_StudentCapacity = a.AMB_StudentCapacity,
                                                  AMB_Order = a.AMB_Order,
                                                  AMB_AidedUnAided = a.AMB_AidedUnAided

                                              }).Distinct().ToList();
                            data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

                        }
                    }
                    else
                    {
                        var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                          from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                          from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                          where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                          select new ClgMasterBranchDMO
                                          {
                                              AMB_Id = a.AMB_Id,
                                              AMB_BranchName = a.AMB_BranchName,
                                              AMB_BranchCode = a.AMB_BranchCode,
                                              AMB_BranchInfo = a.AMB_BranchInfo,
                                              AMB_BranchType = a.AMB_BranchType,
                                              AMB_StudentCapacity = a.AMB_StudentCapacity,
                                              AMB_Order = a.AMB_Order,
                                              AMB_AidedUnAided = a.AMB_AidedUnAided

                                          }).Distinct().ToList();
                        data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDailyAttendanceDTO onselectBranch(CollegeDailyAttendanceDTO data)
        {
            try
            {
                //List<long> GrpId = new List<long>();

                //foreach (var item in data.Temp_branchDTO)
                //{
                //    GrpId.Add(item.AMB_Id);
                //}

                var check_rolename = (from a in _clgadmctxt.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeDailyAttendanceDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _clgadmctxt.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeDailyAttendanceDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    if (check_rolename.FirstOrDefault().rolename.Equals("Staff"))
                    {
                        data.semlist = (from a in _clgadmctxt.Adm_College_Atten_Login_UserDMO
                                        from b in _clgadmctxt.Adm_College_Atten_Login_DetailsDMO
                                        from c in _clgadmctxt.AcademicYear
                                        from d in _clgadmctxt.MasterCourseDMO
                                        from e in _clgadmctxt.ClgMasterBranchDMO
                                        from f in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && e.AMB_Id == b.AMB_Id
                                        && f.AMSE_Id == b.AMSE_Id && a.MI_Id == data.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                        && a.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                        select f).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();
                    }
                    else
                    {
                        var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                            from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                            from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                            from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                            where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                            && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                            && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)

                                            select new CLG_Adm_Master_SemesterDMO
                                            {
                                                AMSE_Id = a.AMSE_Id,
                                                AMSE_SEMName = a.AMSE_SEMName,
                                                AMSE_SEMInfo = a.AMSE_SEMInfo,
                                                AMSE_SEMCode = a.AMSE_SEMCode,
                                                AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                                AMSE_SEMOrder = a.AMSE_SEMOrder,
                                                AMSE_Year = a.AMSE_Year,
                                                AMSE_EvenOdd = a.AMSE_EvenOdd
                                            }).Distinct().ToList();

                        data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                    }
                }
                else
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && c.ACAYC_Id == b.ACAYC_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id
                                        && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id
                                        && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                        && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)

                                        select new CLG_Adm_Master_SemesterDMO
                                        {
                                            AMSE_Id = a.AMSE_Id,
                                            AMSE_SEMName = a.AMSE_SEMName,
                                            AMSE_SEMInfo = a.AMSE_SEMInfo,
                                            AMSE_SEMCode = a.AMSE_SEMCode,
                                            AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                            AMSE_SEMOrder = a.AMSE_SEMOrder,
                                            AMSE_Year = a.AMSE_Year,
                                            AMSE_EvenOdd = a.AMSE_EvenOdd
                                        }).Distinct().ToList();

                    data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDailyAttendanceDTO getsection(CollegeDailyAttendanceDTO data)
        {
            try
            {
                //List<long> GrpId = new List<long>();

                //foreach (var item in data.Temp_branchDTO)
                //{
                //    GrpId.Add(item.AMB_Id);
                //}

                var check_rolename = (from a in _clgadmctxt.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeDailyAttendanceDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _clgadmctxt.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeDailyAttendanceDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    if (check_rolename.FirstOrDefault().rolename.Equals("Staff"))
                    {
                        data.seclist = (from a in _clgadmctxt.Adm_College_Atten_Login_UserDMO
                                        from b in _clgadmctxt.Adm_College_Atten_Login_DetailsDMO
                                        from c in _clgadmctxt.AcademicYear
                                        from d in _clgadmctxt.MasterCourseDMO
                                        from e in _clgadmctxt.ClgMasterBranchDMO
                                        from f in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        from g in _clgadmctxt.Adm_College_Master_SectionDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && e.AMB_Id == b.AMB_Id
                                        && f.AMSE_Id == b.AMSE_Id && g.ACMS_Id == b.ACMS_Id && a.MI_Id == data.MI_Id
                                        && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                        && a.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && d.AMCO_ActiveFlag == true
                                        && b.ACALD_ActiveFlag == true && b.AMSE_Id == data.AMSE_Id)
                                        select g).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
                    }
                    else
                    {
                        data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
                    }
                }
                else
                {
                    data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<CollegeDailyAttendanceDTO> getAttendetails(CollegeDailyAttendanceDTO data)
        {
            try
            {
                //data.Todates = "2019-03-21";
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "College_Attendace_Absent_Manual_Student_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMCO_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMB_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMSE_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ACMS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@Todates", SqlDbType.VarChar) { Value = Convert.ToString(data.Todates) });

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
                        data.studentreport = retObject.ToArray();
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
        public async Task<CollegeDailyAttendanceDTO> absentsendsms(CollegeDailyAttendanceDTO data)
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
                    if (data.Todates != "" && data.Todates != null)
                    {
                        confromdate = data.Todates;
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                for (int k = 0; k < data.absentlist.Length; k++)
                {
                    long MI_id = data.MI_Id;
                    string mobileno = data.absentlist[k].AMCST_MobileNo.ToString();
                    long AMCST_Id = data.absentlist[k].AMCST_Id;

                    if (mobileno.Length == 10)
                    {
                        y = y + 1;
                        sendSms(MI_id, mobileno, "Attendance_Auto_Schedular_EOD", AMCST_Id, confromdate, data.ASMAY_Id);
                    }
                    else
                    {
                        msg = data.absentlist[k].AMCST_FirstName;
                        msg1 += msg;
                    }
                }

                if (data.absentlist.Length == y)
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
        public async Task<string> sendSms(long MI_Id, string mobileNo, string Template, long UserID, string date, long ASMAY_Id)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

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
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Attendace_Auto_Scheduler";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ACSA_AttendanceDate", SqlDbType.VarChar) { Value = date });
                        cmd.Parameters.Add(new SqlParameter("@ACMST_Id", SqlDbType.BigInt) { Value = UserID });

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
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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
