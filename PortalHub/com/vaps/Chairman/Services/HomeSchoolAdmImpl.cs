

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class HomeSchoolAdmImpl : Interfaces.HomeSchoolAdmInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public HomeSchoolAdmImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public HomeSchoolAdmDTO Getdetails(HomeSchoolAdmDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {


              



                List<HomeSchoolAdmDTO> result = new List<HomeSchoolAdmDTO>();
                List<HomeSchoolAdmDTO> result1 = new List<HomeSchoolAdmDTO>();
                List<HomeSchoolAdmDTO> result2 = new List<HomeSchoolAdmDTO>();
                List<HomeSchoolAdmDTO> result3 = new List<HomeSchoolAdmDTO>();
                List<HomeSchoolAdmDTO> result4 = new List<HomeSchoolAdmDTO>();



                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true ).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.classarray = _db.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag==true).OrderBy(a=>a.ASMCL_Order).ToArray();

                data.sectionarray=_db.Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag==1).ToArray();
                data.fillregstd = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           from d in _db.AcademicYear


                                           where (a.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id &&  a.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.MI_Id == c.MI_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                           select new
                                           {
                                               year = d.ASMAY_Year,
                                               stud_count = a.AMST_Id
                                           }).Distinct().GroupBy(id => id.year).Select(g => new HomeSchoolAdmDTO { year = g.Key, stud_count = g.Count() }).ToArray();


                data.fillnewadmstd = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           from d in _db.AcademicYear


                                           where (a.AMST_Id == c.AMST_Id && a.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.MI_Id == c.MI_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                           select new
                                           {
                                               year = d.ASMAY_Year,
                                               stud_count = a.AMST_Id
                                           }).Distinct().GroupBy(id => id.year).Select(g => new HomeSchoolAdmDTO { year = g.Key, stud_count = g.Count() }).ToArray();



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CH_CLASS_SECTION_STRENGTH_GRID_REG";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@withtc",
                      SqlDbType.Bit)
                    {
                        Value = data.withtc
                    });
                    cmd.Parameters.Add(new SqlParameter("@withdeactive",
                    SqlDbType.BigInt)
                    {
                        Value = data.withdeactive
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
                        data.sectionwisestrenth = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CH_CLASS_SECTION_STRENGTH_GRID_NEW";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@withtc",
                      SqlDbType.Bit)
                    {
                        Value = data.withtc
                    });
                    cmd.Parameters.Add(new SqlParameter("@withdeactive",
                    SqlDbType.BigInt)
                    {
                        Value = data.withdeactive
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
                        data.sectionwisestrenthnewadm = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }











    }
}
