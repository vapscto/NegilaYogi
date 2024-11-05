
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class TeresianReportImpl : Interface.TeresianReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public TeresianReportImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public TeresianReportDTO getdetails(TeresianReportDTO data)
        {
            try
            {

                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
                data.quotalist = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQ_Id).ToArray();
                data.check_list = _clgadmctxt.IVRM_College_ReportDMO.ToArray();

                data.feegrouparray = _clgadmctxt.FeeGroupDMO.Where(a => a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true).ToArray();

                data.mastercategory = _clgadmctxt.mastercategory.Where(a => a.MI_Id == data.MI_Id && a.ACMC_ActiveFlag == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TeresianReportDTO onselectAcdYear(TeresianReportDTO data)
        {
            try
            {

                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TeresianReportDTO onselectCourse(TeresianReportDTO data)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TeresianReportDTO onselectBranch(TeresianReportDTO data)
        {
            try
            {
                if (data.AMB_Id > 0)
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
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
                else
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true)
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
        public async Task<TeresianReportDTO> onreport(TeresianReportDTO data)
        {
            try
            {
                if (data.Flag == "aidedunadied")
                {
                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Teresian_Report_Adied_Unadied_Stength_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@evenorodd",
                           SqlDbType.VarChar)
                        {
                            Value = data.Evenorodd
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCOC_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.AMCOC_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        //var data = cmd.ExecuteNonQuery();

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
                            data.studentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    data.getcourselist = (from a in _clgadmctxt.MasterCourseDMO
                                          join b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO on a.AMCO_Id equals b.AMCO_Id
                                          join c in _clgadmctxt.AcademicYear on b.ASMAY_Id equals c.ASMAY_Id
                                          join d in _clgadmctxt.ClgMasterCourseCategorycategoryMap on a.AMCO_Id equals d.AMCO_Id
                                          join e in _clgadmctxt.mastercategory on d.AMCOC_Id equals e.AMCOC_Id
                                          where (b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true && d.AMCOC_Id == data.AMCOC_Id)
                                          select a).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                    data.getbranchlist = (from a in _clgadmctxt.MasterCourseDMO
                                          join b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO on a.AMCO_Id equals b.AMCO_Id
                                          join c in _clgadmctxt.AcademicYear on b.ASMAY_Id equals c.ASMAY_Id
                                          join d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO on b.ACAYC_Id equals d.ACAYC_Id
                                          join e in _clgadmctxt.ClgMasterBranchDMO on d.AMB_Id equals e.AMB_Id
                                          join f in _clgadmctxt.ClgMasterCourseCategorycategoryMap on a.AMCO_Id equals f.AMCO_Id
                                          join g in _clgadmctxt.mastercategory on f.AMCOC_Id equals g.AMCOC_Id

                                          where (b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true && f.AMCOC_Id == data.AMCOC_Id)
                                          select new TeresianReportDTO
                                          {
                                              AMCO_Id = a.AMCO_Id,
                                              AMB_Id = e.AMB_Id,
                                              AMB_Order = e.AMB_Order,
                                              AMCO_Order = a.AMCO_Order,
                                              AMCO_CourseName = a.AMCO_CourseName,
                                              AMB_BranchName = e.AMB_BranchName
                                          }).Distinct().OrderBy(a => (a.AMCO_Order)).ThenBy(b => b.AMB_Order).ToArray();


                    data.categorynoyear = (from a in _clgadmctxt.mastercategory
                                           from b in _clgadmctxt.ClgMasterCourseCategorycategoryMap
                                           from c in _clgadmctxt.MasterCourseDMO
                                           where (a.AMCOC_Id == b.AMCOC_Id && b.AMCO_Id == c.AMCO_Id && a.AMCOC_Id == data.AMCOC_Id && b.AMCOC_Id == data.AMCOC_Id
                                           && c.AMCO_ActiveFlag == true && b.AMCOCM_ActiveFlg == true)
                                           select new TeresianReportDTO
                                           {
                                               AMCO_NoOfYears = c.AMCO_NoOfYears

                                           }).Distinct().ToArray();
                }

                else if (data.Flag == "iiyear")
                {
                    string feegroup = "0";

                    //string qoutacategory = "0";

                    for (int i = 0; i < data.Temp_FeeGroup.Count(); i++)
                    {
                        feegroup = feegroup + "," + data.Temp_FeeGroup[i].FMG_Id.ToString();
                    }

                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Teresian_Report_I_And_II_Year_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Mi_Id",
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

                        cmd.Parameters.Add(new SqlParameter("@Feegroup",
                           SqlDbType.VarChar)
                        {
                            Value = feegroup
                        });

                        cmd.Parameters.Add(new SqlParameter("@category",
                          SqlDbType.VarChar)
                        {
                            Value = data.AMCOC_Id
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
                            data.studentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.Flag == "iiisem")
                {
                    string feegroup = "0";

                    //string qoutacategory = "0";

                    for (int i = 0; i < data.Temp_FeeGroup.Count(); i++)
                    {
                        feegroup = feegroup + "," + data.Temp_FeeGroup[i].FMG_Id.ToString();
                    }

                    //for (int j = 0; j < data.Temp_Quotacategory.Count(); j++)
                    //{
                    //    qoutacategory = qoutacategory + "," + data.Temp_Quotacategory[j].ACQ_Id.ToString();
                    //}


                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Teresian_Report_I_And_II_Sem_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Mi_Id",
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

                        cmd.Parameters.Add(new SqlParameter("@Feegroup",
                           SqlDbType.VarChar)
                        {
                            Value = feegroup
                        });

                        cmd.Parameters.Add(new SqlParameter("@category",
                          SqlDbType.VarChar)
                        {
                            Value = data.AMCOC_Id
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
                            data.studentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.Flag == "iiiyear")
                {
                    string feegroup = "0";

                    //string qoutacategory = "0";

                    for (int i = 0; i < data.Temp_FeeGroup.Count(); i++)
                    {
                        feegroup = feegroup + "," + data.Temp_FeeGroup[i].FMG_Id.ToString();
                    }

                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Teresian_Report_I_And_III_Year_Report ";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Mi_Id",
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

                        cmd.Parameters.Add(new SqlParameter("@Feegroup",
                           SqlDbType.VarChar)
                        {
                            Value = feegroup
                        });

                        cmd.Parameters.Add(new SqlParameter("@category",
                          SqlDbType.VarChar)
                        {
                            Value = data.AMCOC_Id
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
                            data.studentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.Flag == "categorycombination")
                {

                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Teresian_Report_Category_Combination_Report";
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
                            data.studentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TeresianReportDTO onselectcategory(TeresianReportDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.mastercategory
                                   from b in _clgadmctxt.ClgMasterCourseCategorycategoryMap
                                   from c in _clgadmctxt.MasterCourseDMO
                                   where (a.AMCOC_Id == b.AMCOC_Id && b.AMCO_Id == c.AMCO_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && b.AMCOC_Id == data.AMCOC_Id)
                                   select c).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
