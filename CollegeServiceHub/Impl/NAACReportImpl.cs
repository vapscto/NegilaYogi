using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
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
    public class NAACReportImpl : NAACReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public NAACReportImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public NAACReportDTO getdetails(NAACReportDTO data)
        {
            try
            {

                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public NAACReportDTO onreport(NAACReportDTO data)
        {
            try
            {
                var year_id = "";
                var yearid = "";

                for (int i = 0; i < data.TempararyArrayListcoloumn.Length; i++)
                {
                    string Id = data.TempararyArrayListcoloumn[i].ASMAY_Id.ToString();
                    if (Id != "0" && Id != null)
                    {
                        yearid = Id + "," + yearid;
                    }
                }
                year_id = yearid.TrimEnd(',');

                List<NAACReportDTO> result = new List<NAACReportDTO>();
                List<NAACReportDTO> result1 = new List<NAACReportDTO>();
                List<NAACReportDTO> result2 = new List<NAACReportDTO>();

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Teresian_Naac_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = year_id });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.flag });
                    cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.Int) { Value = data.AMCO_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        if (data.flag == "StdAdm" || data.flag == "CatStd")
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                //while (dataReader.Read())
                                //{
                                //    result.Add(new NAACReportDTO
                                //    {
                                //        ACQC_CategoryName = dataReader["ACQC_CategoryName"].ToString(),
                                //        No_of_Seats = Convert.ToInt32(dataReader["No_of_Seats"].ToString()),
                                //        ACQC_Id = Convert.ToInt32(dataReader["ACQC_Id"].ToString()),
                                //        AMCST_Sex = dataReader["AMCST_Sex"].ToString(),
                                //        ASMAY_Id = Convert.ToInt32(dataReader["ASMAY_Id"].ToString()),
                                //        ASMAY_Year = dataReader["ASMAY_Year"].ToString()

                                //    });
                                //    data.datareport = result.ToArray();
                                //}
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
                                data.datareport = retObject.ToArray();
                            }
                        }
                        else if (data.flag == "StdEnrol")
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                //while (dataReader.Read())
                                //{
                                //    result.Add(new NAACReportDTO
                                //    {
                                //        ACQ_Id = Convert.ToInt32(dataReader["ACQ_Id"].ToString()),
                                //        AMCOC_Id = Convert.ToInt32(dataReader["AMCOC_Id"].ToString()),
                                //        No_of_Seats = Convert.ToInt32(dataReader["No_of_Seats"].ToString()),
                                //        ACQ_QuotaName = dataReader["ACQ_QuotaName"].ToString(),
                                //        AMCOC_Name = dataReader["AMCOC_Name"].ToString()

                                //    });
                                //    data.datareport = result.ToArray();
                                //}
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
                                data.datareport = retObject.ToArray();

                                //dataReader.NextResult();
                                //while (dataReader.Read())
                                //{
                                //    result1.Add(new NAACReportDTO
                                //    {
                                //        ACQ_Id = Convert.ToInt32(dataReader["ACQ_Id"].ToString()),
                                //        ACQ_QuotaName = dataReader["ACQ_QuotaName"].ToString()
                                //    });
                                //    data.datareport1 = result1.ToArray();
                                //}
                                //dataReader.NextResult();
                                //while (dataReader.Read())
                                //{
                                //    result2.Add(new NAACReportDTO
                                //    {
                                //        AMCOC_Id = Convert.ToInt32(dataReader["AMCOC_Id"].ToString()),
                                //        AMCOC_Name = dataReader["AMCOC_Name"].ToString()
                                //    });
                                //    data.datareport2 = result2.ToArray();
                                //}
                            }

                        }

                        else if (data.flag == "ProgOffer")
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
                                data.datareport = retObject.ToArray();
                            }
                        }

                        else if (data.flag == "DeptList")
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
                                data.datareport = retObject.ToArray();
                            }
                        }
                        else if (data.flag == "CasteRep")
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
                                data.datareport = retObject.ToArray();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.datareport2 = _clgadmctxt.mastercategory.Where(a => a.MI_Id == data.MI_Id && a.ACMC_ActiveFlag == true).ToArray();
                data.religion = _clgadmctxt.Religion.Where(a => a.Is_Active == true).ToArray();
                data.castecateogry = _clgadmctxt.CasteCategory.ToArray();

                data.semester = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                 where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true)
                                 select new NAACReportDTO
                                 {
                                     AMSE_Year = a.AMSE_Year

                                 }).Distinct().ToArray();

                //data.semester = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).Select(a => a.AMSE_Year).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
