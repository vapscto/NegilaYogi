using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlumniHub.Com.Service
{
    public class AlumnilettersImpl : Interface.AlumnilettersInterface
    {
        public AlumniContext _AlumniContext;
        private readonly DomainModelMsSqlServerContext _db;
        public AlumnilettersImpl(AlumniContext AlumniContext, DomainModelMsSqlServerContext db)
        {
            _AlumniContext = AlumniContext;
            _db = db;
        }
        public AlumnilettersDTO BindData(AlumnilettersDTO mas)
        {
            try
            {
                List<MasterAcademic> aya = new List<MasterAcademic>();
                aya = _AlumniContext.AcademicYear.Where(d => d.MI_Id == mas.MI_Id && d.Is_Active == true).ToList();
                mas.newuser1 = aya.OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _AlumniContext.School_M_Class.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.newuser2 = aya1.OrderBy(c => c.ASMCL_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return mas;
        }
        public AlumnilettersDTO ShowReport(AlumnilettersDTO stud)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumnistudentsearchReportLetter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = stud.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@clas", SqlDbType.VarChar) { Value = stud.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = stud.logoname });
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
                        stud.SearchstudentDetails = retObject.ToArray();
                        if (stud.SearchstudentDetails.Length > 0)
                        {
                            stud.count = stud.SearchstudentDetails.Length;
                        }
                        else
                        {
                            stud.count = 0;
                        }
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
            return stud;
        }
        public AlumnilettersDTO letterReport(AlumnilettersDTO stud)
        {
            try
            {
                string semids = "0";
                for (int d = 0; d < stud.studlistdata.Count(); d++)
                {
                    semids = semids + ',' + stud.studlistdata[d].AMST_ID;
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumnistudentsearchLetterReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stud.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = stud.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@clas", SqlDbType.VarChar) { Value = stud.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_IDs", SqlDbType.VarChar) { Value = semids });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = stud.logoname });
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
                        stud.searchstudentDetails2 = retObject.ToArray();
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
            return stud;
        }
    }
}
