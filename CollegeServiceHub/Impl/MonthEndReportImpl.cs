using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
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
    public class MonthEndReportImpl : MonthEndReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public MonthEndReportImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }


        public MonthEndReportDTO getdata(MonthEndReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.ToArray();


                List<Month> mnth = new List<Month>();
                mnth = _clgadmctxt.mnth.Where(t => t.Is_Active == true).ToList();
                data.Month_array = mnth.ToArray();

                data.getcategory = _clgadmctxt.mastercategory.Where(a => a.MI_Id == data.MI_ID && a.ACMC_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<MonthEndReportDTO> getreport(MonthEndReportDTO data)
        {
            List<MonthEndReportDTO> AllInOne = new List<MonthEndReportDTO>();
            try
            {

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_Adm_MonthEndReport_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Year",
                       SqlDbType.VarChar)
                    {
                        Value = data.acayid
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@amay",
                  SqlDbType.VarChar)
                    {
                        Value = data.year
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
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
                        data.reportdatelist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MonthEndReportDTO getyear(MonthEndReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.ToArray();

                List<MasterCourseDMO> course = new List<MasterCourseDMO>();
                course = _clgadmctxt.MasterCourseDMO.Where(t => t.MI_Id == data.MI_ID && t.AMCO_ActiveFlag == true).OrderByDescending(t => t.AMCO_Order).ToList();
                data.courselist = course.ToArray();

                List<ClgMasterBranchDMO> branch = new List<ClgMasterBranchDMO>();
                branch = _clgadmctxt.ClgMasterBranchDMO.Where(t => t.MI_Id == data.MI_ID && t.AMB_ActiveFlag == true).OrderByDescending(t => t.AMB_Order).ToList();
                data.branchlist = branch.ToArray();

                List<CLG_Adm_Master_SemesterDMO> semester = new List<CLG_Adm_Master_SemesterDMO>();
                semester = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == data.MI_ID && t.AMSE_ActiveFlg == true).OrderByDescending(t => t.AMSE_SEMOrder).ToList();
                data.semesterlist = semester.ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public async Task<MonthEndReportDTO> Studdetails(MonthEndReportDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<MonthEndReportDTO> classlist = new List<MonthEndReportDTO>();

            try
            {
                if (stuDTO.AMCST_SOL == "O")
                {
                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Total_strength_count_All_Clg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.BigInt) { Value = stuDTO.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@courseid", SqlDbType.BigInt) { Value = stuDTO.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@branchid", SqlDbType.BigInt) { Value = stuDTO.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@semesterid", SqlDbType.BigInt) { Value = stuDTO.AMSE_Id });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = stuDTO.MI_ID });
                        cmd.Parameters.Add(new SqlParameter("@status_flag", SqlDbType.VarChar) { Value = stuDTO.Status_Flag });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            stuDTO.studentlist = retObject.ToArray();

                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Total_Strength_new_Clg";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                        cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMCST_SOL });
                        cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.BigInt) { Value = stuDTO.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@courseid", SqlDbType.BigInt) { Value = stuDTO.AMCO_Id });
                        cmd.Parameters.Add(new SqlParameter("@branchid", SqlDbType.BigInt) { Value = stuDTO.AMB_Id });
                        cmd.Parameters.Add(new SqlParameter("@semesterid", SqlDbType.BigInt) { Value = stuDTO.AMSE_Id });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = stuDTO.MI_ID });
                        cmd.Parameters.Add(new SqlParameter("@status_flag", SqlDbType.VarChar) { Value = stuDTO.Status_Flag });
                        cmd.Parameters.Add(new SqlParameter("@withtc", SqlDbType.VarChar) { Value = stuDTO.withtc });
                        cmd.Parameters.Add(new SqlParameter("@withdeactive", SqlDbType.VarChar) { Value = stuDTO.withdeactive });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            stuDTO.studentlist = retObject.ToArray();

                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                var data = _clgadmctxt.AdmissionStandardDMO.Where(t => t.MI_Id == stuDTO.MI_ID).ToList();
                if (data.Count > 0)
                {
                    stuDTO.gender1 = data.FirstOrDefault().ASC_Default_Gender;
                }

                var distinctgender = _clgadmctxt.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == stuDTO.MI_ID).Select(t => t.AMCST_Sex).Distinct().ToList();
                if (distinctgender.Count > 0)
                {
                    stuDTO.totalgender = distinctgender.Count;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }

        public MonthEndReportDTO getbranch(MonthEndReportDTO data)
        {
            try
            {


                data.branchlist = (from a in _clgadmctxt.Adm_Course_Branch_MappingDMO
                                   from b in _clgadmctxt.ClgMasterBranchDMO
                                   where (a.AMB_Id == b.AMB_Id && a.AMCOBM_ActiveFlg == true && a.MI_Id == data.MI_ID && a.AMCO_Id == data.AMCO_Id)
                                   select new MonthEndReportDTO
                                   {
                                       AMCOBM_Id = a.AMCOBM_Id,
                                       AMB_Id=a.AMB_Id,
                                       AMB_BranchName = b.AMB_BranchName
                                   }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public MonthEndReportDTO getsemester(MonthEndReportDTO data)
        {
            try
            {
                data.semesterlist = (from a in _clgadmctxt.AdmCourseBranchSemesterMappingDMO
                                     from b in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                     from c in _clgadmctxt.Adm_Course_Branch_MappingDMO
                                     where (a.AMSE_Id == b.AMSE_Id && a.AMCOBMS_ActiveFlg == true && a.MI_Id == data.MI_ID && a.AMCOBM_Id == data.AMB_Id
                                     && a.AMCOBM_Id==c.AMCOBM_Id)
                                     select new MonthEndReportDTO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = b.AMSE_SEMName
                                     }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
