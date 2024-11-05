using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
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

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AttendanceReportImpl : Interfaces.AttendanceReportInterface
    {
        public StudentAttendanceReportContext _db;

        ILogger<AttendanceReportImpl> _acdimpl;
        public AttendanceReportImpl(StudentAttendanceReportContext db, ILogger<AttendanceReportImpl> acdimpl)
        {
            _db = db;
            _acdimpl = acdimpl;
        }
        public async Task<StudentAttendanceReportDTO> getInitailData(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d => d.MI_Id == ctdo.miid && d.Is_Active == true).ToListAsync();
                ctdo.academicList = allyear.OrderByDescending(y => y.ASMAY_Order).ToArray();

                List<MasterAcademic> defaultyear = new List<MasterAcademic>();
                defaultyear = _db.academicYear.Where(t => t.MI_Id == ctdo.miid && t.Is_Active == true && t.ASMAY_Id == ctdo.ASMAY_Id).ToList();
                ctdo.academicListdefault = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();


               // logo
                var cat = _db.GenConfig.Where(g => g.MI_Id == ctdo.miid && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {
                    ctdo.category_list = _db.category.Where(f => f.MI_Id == ctdo.miid && f.AMC_ActiveFlag == 1).ToArray();
                    ctdo.categoryflag = true;
                }
                else
                {
                    ctdo.categoryflag = false;
                }

                var photopath = _db.standarad.Where(t => t.MI_Id == ctdo.miid).ToList();
                if (photopath.Count > 0)
                {
                    ctdo.photopathname = photopath.FirstOrDefault().ASC_Logo_Path;
                }


                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.Id.Equals(ctdo.userId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                  ).Distinct().ToArray();


                        ctdo.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
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
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO ctdo)
        {
            var asmclid = "";
            var asmsid = "";
            int k = 0;
            var check_rolename = (from a in _db.MasterRoleType
                                  where (a.IVRMRT_Id == ctdo.roleId)
                                  select new StudentAttendanceReportDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }
                                 ).ToList();

            var empcode_check = (from a in _db.Staff_User_Login
                                 where (a.MI_Id == ctdo.miid && a.Id.Equals(ctdo.userId))
                                 select new StudentAttendanceReportDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("STAFF") || check_rolename.FirstOrDefault().rolename.Equals("staff") || check_rolename.FirstOrDefault().rolename.Equals("Staff"))
            {
                k = 1;
                if (ctdo.type == '1' || ctdo.type == 1)
                {
                    if (empcode_check.Count > 0)
                    {
                        var classlist1 = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                  ).Distinct().ToList();

                        for (int i = 0; i < classlist1.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmclid = classlist1[i].ASMCL_Id.ToString();
                            }
                            else
                            {
                                asmclid = asmclid + ',' + classlist1[i].ASMCL_Id.ToString();
                            }
                        }

                        var sectionlist1 = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }
                                        ).Distinct().ToList();

                        for (int i = 0; i < sectionlist1.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmsid = sectionlist1[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + sectionlist1[i].ASMS_Id.ToString();
                            }
                        }
                    }

                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    if (ctdo.ASMC_Id == 0)
                    {
                        asmclid = ctdo.ASMCL_Id.ToString();

                        var sectiondata = (from a in _db.admissionClass
                                           from b in _db.masterSection
                                           from c in _db.academicYear
                                           from d in _db.Masterclasscategory
                                           from e in _db.AdmSchoolMasterClassCatSec
                                           from f in _db.Adm_SchAttLoginUser
                                           from g in _db.Adm_SchAttLoginUserClass
                                           where (f.ASALU_Id == g.ASALU_Id && a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id && d.MI_Id == ctdo.miid && d.ASMAY_Id == ctdo.ASMAY_Id && d.ASMCL_Id == ctdo.ASMCL_Id && f.ASMAY_Id == ctdo.ASMAY_Id && g.ASMCL_Id == ctdo.ASMCL_Id && e.ASMCCS_ActiveFlg == true)
                                           select b
                                 ).Distinct().OrderBy(g => g.ASMC_Order).ToList();


                        if (sectiondata.Count > 0)
                        {
                            for (int i = 0; i < sectiondata.Count; i++)
                            {
                                if (i == 0)
                                {
                                    asmsid = sectiondata[i].ASMS_Id.ToString();
                                }
                                else
                                {
                                    asmsid = asmsid + ',' + sectiondata[i].ASMS_Id.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        asmclid = ctdo.ASMCL_Id.ToString();
                        asmsid = ctdo.ASMC_Id.ToString();
                    }
                }
            }
            else
            {
                if (ctdo.ASMC_Id == 0)
                {
                    asmclid = ctdo.ASMCL_Id.ToString();

                    var sectiondata = (from a in _db.admissionClass
                                       from b in _db.masterSection
                                       from c in _db.academicYear
                                       from d in _db.Masterclasscategory
                                       from e in _db.AdmSchoolMasterClassCatSec
                                       where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id && d.MI_Id == ctdo.miid && d.ASMAY_Id == ctdo.ASMAY_Id && d.ASMCL_Id == ctdo.ASMCL_Id && e.ASMCCS_ActiveFlg == true)
                                       select b
                             ).Distinct().OrderBy(g => g.ASMC_Order).ToList();


                    if (sectiondata.Count > 0)
                    {
                        for (int i = 0; i < sectiondata.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmsid = sectiondata[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + sectiondata[i].ASMS_Id.ToString();
                            }
                        }
                    }
                }
                else
                {
                    asmclid = ctdo.ASMCL_Id.ToString();
                    asmsid = ctdo.ASMC_Id.ToString();
                }
                //asmclid = ctdo.ASMCL_Id.ToString();
                //asmsid = ctdo.ASMC_Id.ToString();
            }

            if (ctdo.AMC_Id == null || ctdo.AMC_Id == 0)
            {
                ctdo.AMC_Id = 0;

            }
            var amcid = ctdo.AMC_Id.ToString();

            ctdo.AMC_logo = _db.category.Where(p => p.AMC_Id == ctdo.AMC_Id && p.MI_Id == ctdo.miid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();


            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                //cmd.CommandText = "AttendanceReport_NEW";
                cmd.CommandText = "AttendanceReport_perc_test";
                // AttendanceReport_NEW
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 90000000;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(ctdo.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.NVarChar)
                {
                    Value = asmclid
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.NVarChar)
                {
                    Value = asmsid
                });
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(ctdo.type)
                });

                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(ctdo.miid)
                });
                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt)
                {
                    Value = k
                });

                cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar)
                {
                    Value = ctdo.AMC_Id
                });
                cmd.Parameters.Add(new SqlParameter("@Percentage", SqlDbType.VarChar)
                {
                    Value = ctdo.percentage
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
                    ctdo.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Error In Attendance Report 100% :" + ex.Message);
                }
                return ctdo;
            }
        }
        public async Task<StudentAttendanceReportDTO> getsection(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {

                var photopath = _db.standarad.Where(t => t.MI_Id == ctdo.miid).ToList();
                if (photopath.Count > 0)
                {
                    ctdo.photopathname = photopath.FirstOrDefault().ASC_Logo_Path;
                }


                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.Id.Equals(ctdo.userId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }).Distinct().ToArray();


                        ctdo.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }).Distinct().ToArray();
                    }
                }
                else
                {
                    ctdo.SectionList = (from a in _db.AdmSchoolMasterClassCatSec
                                        from b in _db.Masterclasscategory
                                        from c in _db.admissionClass
                                        from d in _db.masterSection
                                        where (a.ASMCC_Id == b.ASMCC_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCCS_ActiveFlg == true
                                        && b.Is_Active == true && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1 && b.MI_Id == ctdo.miid
                                        && b.ASMCL_Id == ctdo.ASMCL_Id && b.ASMAY_Id == ctdo.ASMAY_Id)
                                        select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public async Task<StudentAttendanceReportDTO> getclass(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {

                var photopath = _db.standarad.Where(t => t.MI_Id == ctdo.miid).ToList();
                if (photopath.Count > 0)
                {
                    ctdo.photopathname = photopath.FirstOrDefault().ASC_Logo_Path;
                }


                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.Id.Equals(ctdo.userId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        if(ctdo.AMC_Id!=0)
                        {
                            

                            ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                              from b in _db.Adm_SchAttLoginUser
                                              from c in _db.admissionClass
                                              from d in _db.Masterclasscategory
                                              where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                              && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                              && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                              && c.ASMCL_ActiveFlag == true && c.ASMCL_Id == d.ASMCL_Id && d.AMC_Id==ctdo.AMC_Id)
                                              select new StudentAttendanceReportDTO
                                              {
                                                  ASMCL_Id = c.ASMCL_Id,
                                                  asmcL_ClassName = c.ASMCL_ClassName,
                                              }).Distinct().ToArray();
                        }
                        else
                        {
                            ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                              from b in _db.Adm_SchAttLoginUser
                                              from c in _db.admissionClass
                                              where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                              && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                              && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                              && c.ASMCL_ActiveFlag == true  )
                                              select new StudentAttendanceReportDTO
                                              {
                                                  ASMCL_Id = c.ASMCL_Id,
                                                  asmcL_ClassName = c.ASMCL_ClassName,
                                              }).Distinct().ToArray();
                        }
                        
                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    if (ctdo.AMC_Id != 0)
                    {
                        ctdo.classlist = (from a in _db.admissionClass
                                          from b in _db.Masterclasscategory
                                          from c in _db.academicYear
                                          from d in _db.Masterclasscategory
                                          where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true
                                          && c.Is_Active == true && b.ASMAY_Id == ctdo.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && d.AMC_Id == ctdo.AMC_Id)
                                          select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                    else
                    {
                        ctdo.classlist = (from a in _db.admissionClass
                                          from b in _db.Masterclasscategory
                                          from c in _db.academicYear
                                          where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true
                                          && c.Is_Active == true && b.ASMAY_Id == ctdo.ASMAY_Id)
                                          select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public async Task<StudentAttendanceReportDTO> shortageOfAttendanceAlert(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<studentAttendanceList_shoratgae1> stutList = new List<studentAttendanceList_shoratgae1>();
            

                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())                    //{                    //    cmd.CommandText = "Adm_Student_Attendanceshortage_Insertion";                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    //@ASMAY_Id
                    //    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    //    {
                    //        Value = Convert.ToInt64(ctdo.ASMAY_Id)
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    //    {
                    //        Value = Convert.ToInt64(ctdo.miid)
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.VarChar)
                    //    {
                    //        Value = ctdo.userId
                    //    });                    //    if (cmd.Connection.State != ConnectionState.Open)                    //        cmd.Connection.Open();                    //    var retObject = new List<dynamic>();                    //    try                    //    {                    //        using (var dataReader = cmd.ExecuteReader())                    //        {                    //            while (dataReader.Read())                    //            {                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                    //                {                    //                    dataRow.Add(                    //                        dataReader.GetName(iFiled),                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                    //                    );                    //                }                    //                retObject.Add((ExpandoObject)dataRow);                    //                stutList.Add(new studentAttendanceList_shoratgae1                    //                {                    //                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),                    //                    AMST_Name = Convert.ToString(dataReader["name"]),                    //                    ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),                    //                    ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),                    //                    Percentage = Convert.ToString(dataReader["per"]),                    //                    AMST_MobileNo = Convert.ToInt64(dataReader["AMST_MobileNo"])                    //                });                    //            }                    //        }
                    //        // data.alldata = retObject.ToArray();
                    //    }                    //    catch (Exception ex)                    //    {                    //        Console.Write(ex.Message);                    //    }                    //}



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                   
                    cmd.CommandText = "Adm_Student_Attendanceshortage_Insertion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.miid)
                    });

                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.VarChar)
                    {
                        Value = ctdo.userId
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
                        ctdo.studentAttendanceList = retObject.ToArray();
                        if (ctdo.studentAttendanceList.Length > 0)
                        {
                            //ctdo.count = ctdo.studentAttendanceList.Length;
                        }
                        else
                        {
                          //  ctdo.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                if (stutList.Count > 0)
                    {

                        //    ctdo.stutList = ctdo.stutList;
                        for (int k = 0; k < stutList.Count; k++)
                        {
                            long MI_id = ctdo.miid;
                            string mobileno = stutList[k].AMST_MobileNo.ToString();
                            long AMST_Id = stutList[k].AMST_Id;

                            if (mobileno.Length == 10)
                            {

                                try
                                {
                                    //  sendSms(MI_id, mobileno, "Attendance_Auto_Schedular_EOD", AMST_Id, confromdate, ctdo.ASMAY_Id);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                           
                        }

                        //if (ctdo.studentAttendanceList.Length == y)
                        //{
                        //    ctdo.message = "SMS Sent Successfully";
                        //}
                        //else
                        //{
                        //    ctdo.message = "SMS Sent Successfully , And Failed List '" + msg1 + "'";
                        //}
                    }






                

            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }


        public async Task<string> sendSms(long MI_Id, string mobileNo, string Template, long UserID, string date, long ASMAY_Id)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
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
                        var template1010 = _db.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

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
