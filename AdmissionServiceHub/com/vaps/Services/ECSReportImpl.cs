using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class ECSReportImpl : Interfaces.ECSReportInterface
    {
        public ActivateDeactivateContext _context;
        ILogger<ECSReportImpl> _acdimpl;
        public ECSReportImpl(ActivateDeactivateContext _cont, ILogger<ECSReportImpl> _acdi)
        {
            _context = _cont;
            _acdimpl = _acdi;
        }
        public ECSReportDTO getloaddata(ECSReportDTO data)
        {
            try
            {
                data.getyearlist = _context.academicYear.Where(a => a.MI_Id == data.MI_id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ECSReportDTO getclass(ECSReportDTO data)
        {
            try
            {
                if (data.ASMAY_Id > 0)
                {
                    data.getclasslist = (from a in _context.academicYear
                                         from b in _context.admissionClass
                                         from c in _context.masterclasscategory
                                         where (a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.ASMCL_ActiveFlag == true
                                         && c.Is_Active == true && a.MI_Id == data.MI_id && c.MI_Id == data.MI_id && c.ASMAY_Id == data.ASMAY_Id)
                                         select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.getclasslist = _context.admissionClass.Where(a => a.MI_Id == data.MI_id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ECSReportDTO getsection(ECSReportDTO data)
        {
            try
            {
                if (data.ASMCL_Id > 0)
                {
                    data.getsectionlist = (from a in _context.academicYear
                                           from b in _context.admissionClass
                                           from c in _context.masterclasscategory
                                           from d in _context.AdmSchoolMasterClassCatSec
                                           from e in _context.masterSection
                                           where (a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && d.ASMCC_Id == c.ASMCC_Id && d.ASMS_Id == e.ASMS_Id
                                           && a.Is_Active == true && b.ASMCL_ActiveFlag == true && c.Is_Active == true && e.ASMC_ActiveFlag == 1
                                           && a.MI_Id == data.MI_id && c.MI_Id == data.MI_id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id
                                           && d.ASMCCS_ActiveFlg == true)
                                           select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
                else
                {
                    data.getsectionlist = _context.masterSection.Where(a => a.MI_Id == data.MI_id && a.ASMC_ActiveFlag == 1).OrderBy(a => a.ASMC_Order).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ECSReportDTO getreport(ECSReportDTO data)
        {
            try
            {
                string confromdate = "";
                DateTime fromdate = DateTime.Now;

                fromdate = Convert.ToDateTime(data.reportdate.Date.ToString("yyyy-MM-dd"));

                confromdate = fromdate.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_ECS_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    {
                        Value = confromdate
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
                        data.getreportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In ECS Report  :" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogInformation("Error In ECS Reportnew  :" + ex.Message);
            }
            return data;
        }
        public ECSReportDTO showecsdetails(ECSReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_ECS_Student_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_id
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
                        data.getreportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In ECS Student Details Report  :" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogInformation("Error In ECS Student Details Reportnew  :" + ex.Message);
            }
            return data;
        }

        public ECSReportDTO searchByColumn(ECSReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_ECS_Student_Details_Report_Search";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EnteredData", SqlDbType.VarChar)
                    {
                        Value = data.EnteredData
                    });
                    cmd.Parameters.Add(new SqlParameter("@SearchColumn", SqlDbType.VarChar)
                    {
                        Value = data.SearchColumn
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
                        data.getreportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In ECS Student Details Report  :" + ex.Message);
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
