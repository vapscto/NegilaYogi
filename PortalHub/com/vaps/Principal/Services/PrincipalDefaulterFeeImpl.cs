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
using PreadmissionDTOs.com.vaps.Portals.Principal;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
//using PreadmissionDTOs.com.vaps.Portals.Chirman;

namespace PortalHub.com.vaps.Principal.Services
{
    public class PrincipalDefaulterFeeImpl : Interfaces.PrincipalDefaulterFeeInterface
    {
        private static ConcurrentDictionary<string, PrincipalDefaulterFeeDTO> _login =
        new ConcurrentDictionary<string, PrincipalDefaulterFeeDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<PrincipalDefaulterFeeImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public PrincipalDefaulterFeeImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
        }

        public PrincipalDefaulterFeeDTO Getdetails(PrincipalDefaulterFeeDTO data)
        {

            try
            {

                List<PrincipalDefaulterFeeDTO> result3 = new List<PrincipalDefaulterFeeDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true ).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public PrincipalDefaulterFeeDTO Getreport(PrincipalDefaulterFeeDTO data)
        {
            try
            {
                List<PrincipalDefaulterFeeDTO> result1 = new List<PrincipalDefaulterFeeDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Principle_Sectionwise_Fee_Ballance";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmcL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmS_Id
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
                                result1.Add(new PrincipalDefaulterFeeDTO
                                {
                                    name = (dataReader["name"].ToString()),
                                    admno = (dataReader["admno"].ToString()),
                                    regno = (dataReader["regno"].ToString()),
                                    mobile = (dataReader["mob"].ToString()),
                                    Class_Name = (dataReader["class"].ToString()),
                                    section = (dataReader["section"].ToString()),
                                    balance = Convert.ToInt32(dataReader["ballance"]),
                                    paid = Convert.ToDecimal(dataReader["callected"]),
                                    receivable = Convert.ToDecimal(dataReader["receivable"]),
                                    concession = Convert.ToDecimal(dataReader["concession"]),

                                });
                                data.studbal = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

        public PrincipalDefaulterFeeDTO Getstudentdetails(PrincipalDefaulterFeeDTO data)
        {
            try
            {

                //data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                //                           from b in _db.admissioncls
                //                           from c in _db.Adm_M_Student
                //                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                //                           select new
                //                           {
                //                               Class_Name = b.ASMCL_ClassName,
                //                               classid=b.ASMCL_Id,
                //                               stud_count = a.AMST_Id
                //                           }).Distinct().GroupBy(id => id.Class_Name).Select(g => new PrincipalDefaulterFeeDTO { Class_Name = g.Key, stud_count = g.Count() }).ToArray();

                data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == c.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                           select new
                                           {
                                               asmayid = a.ASMAY_Id,
                                               classid = b.ASMCL_Id,
                                               Class_Name = b.ASMCL_ClassName,
                                               stud_count = a.AMST_Id
                                           }).Distinct().GroupBy(id => new { id.classid, id.Class_Name, id.asmayid }).Select(g => new PrincipalDefaulterFeeDTO { classid = g.Key.classid, Class_Name = g.Key.Class_Name, stud_count = g.Count(), ASMAY_Id = g.Key.asmayid }).ToArray();


                List<PrincipalDefaulterFeeDTO> result1 = new List<PrincipalDefaulterFeeDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Principal_Get_Classwise_fee_defaulter";
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


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new PrincipalDefaulterFeeDTO
                                {
                                    classid = Convert.ToInt32(dataReader["classid"]),
                                    Class_Name = (dataReader["class"].ToString()),
                                    balance = Convert.ToInt32(dataReader["ballance"]),


                                });
                                data.studbal = result1.ToArray();
                            }
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

                throw ex;
            }
            return data;
        }
        public PrincipalDefaulterFeeDTO Getsection(PrincipalDefaulterFeeDTO data)
        {

            try
            {

                data.fillsection = (from a in _db.School_Adm_Y_StudentDMO
                                    from b in _db.admissioncls
                                    //from c in _db.Adm_M_Student
                                    from d in _db.Section
                                    where (//a.AMST_Id == c.AMST_Id && 
                                    a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == a.ASMCL_Id && a.ASMCL_Id == data.asmcL_Id && d.ASMS_Id == a.ASMS_Id && d.MI_Id == data.MI_Id )
                                    select new PrincipalDefaulterFeeDTO
                                    {
                                        sectionname = d.ASMC_SectionName,
                                        asmS_Id = d.ASMS_Id
                                    }).Distinct().OrderBy(t => t.asmS_Id).ToArray();



            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public PrincipalDefaulterFeeDTO getclass(PrincipalDefaulterFeeDTO data)//int IVRMM_Id
        {

            try
            {
                List<PrincipalDefaulterFeeDTO> result3 = new List<PrincipalDefaulterFeeDTO>();


                data.classarray = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)


                                   select new PrincipalDefaulterFeeDTO
                                   {
                                       Class_Name = b.ASMCL_ClassName,
                                       asmcL_Id = b.ASMCL_Id
                                   }).Distinct().OrderBy(t => t.asmcL_Id).ToArray();





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
