

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
    public class ADMClassSectionStrengthImpl : Interfaces.ADMClassSectionStrengthInterface
    {
        private static ConcurrentDictionary<string, ADMClassSectionStrengthDTO> _login =
         new ConcurrentDictionary<string, ADMClassSectionStrengthDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ADMClassSectionStrengthImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public ADMClassSectionStrengthDTO Getdetails(ADMClassSectionStrengthDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {

                List<ADMClassSectionStrengthDTO> result3 = new List<ADMClassSectionStrengthDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active==true ).ToList();
                data.yearlist = list.OrderByDescending(e=>e.ASMAY_Order).ToArray();
                data.classarray = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true )


                                   select new ADMClassSectionStrengthDTO
                                   {
                                       Class_Name = b.ASMCL_ClassName,
                                       asmcL_Id = b.ASMCL_Id
                                   }).Distinct().OrderBy(t => t.asmcL_Id).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CH_CLASS_SECTION_STRENGTH_GRID";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmcL_Id
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

                //    if (data.withtc==false && data.withdeactive==false)
                //{
                //    data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                //                               from b in _db.admissioncls
                //                               from c in _db.Adm_M_Student
                //                               where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id &&c.AMST_SOL.Equals("S") 
                //                             && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag==true
                //                               )
                //                               select new
                //                               {
                //                                   asmayid = a.ASMAY_Id,
                //                                   classid = b.ASMCL_Id,
                //                                   Class_Name = b.ASMCL_ClassName,
                //                                   stud_count = a.AMST_Id
                //                               }).Distinct().GroupBy(id => new { id.classid, id.Class_Name, id.asmayid }).Select(g => new ADMClassSectionStrengthDTO { classid = g.Key.classid, Class_Name = g.Key.Class_Name, stud_count = g.Count(), ASMAY_Id = g.Key.asmayid }).ToArray();
                //}
                //data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                //                           from b in _db.admissioncls
                //                           from c in _db.Adm_M_Student
                //                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id  && (c.AMST_SOL.Equals("S") || c.AMST_SOL.Equals("L")) 
                //                           //&& c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag==true
                //                           )
                //                           select new
                //                           {   asmayid=a.ASMAY_Id,
                //                               classid = b.ASMCL_Id,
                //                               Class_Name = b.ASMCL_ClassName,
                //                               stud_count = a.AMST_Id
                //                           }).Distinct().GroupBy(id => new { id.classid, id.Class_Name ,id.asmayid }).Select(g => new ADMClassSectionStrengthDTO { classid = g.Key.classid, Class_Name = g.Key.Class_Name, stud_count = g.Count(),ASMAY_Id=g.Key.asmayid }).ToArray();



                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Get_sectionwise_Reg_student";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@mi_id",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@yearId",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.ASMAY_Id
                //    });





                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();


                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                result3.Add(new ADMClassSectionStrengthDTO
                //                {
                //                    Class_Name = dataReader["class"].ToString(),
                //                    sectionname = dataReader["section"].ToString(),
                //                    stud_count = int.Parse(dataReader["total"].ToString()),
                //                    asmS_Id = int.Parse(dataReader["sectionid"].ToString()),
                //                    asmcL_Id = int.Parse(dataReader["classid"].ToString())
                //                });
                //                data.sectionwisestrenth = result3.ToArray();
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write(ex.Message);
                //    }
              }
                
          }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public ADMClassSectionStrengthDTO Getsectioncount(ADMClassSectionStrengthDTO data)
        {
            try
            {
                //data.fillsectioncount = (from a in _db.School_Adm_Y_StudentDMO
                //                           from b in _db.admissioncls
                //                           from c in _db.Adm_M_Student
                //                           from d in _db.Section
                //                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && (c.AMST_SOL.Equals("S") || c.AMST_SOL.Equals("L"))
                //                         //  && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1  
                //                           && a.ASMCL_Id == data.classid && d.ASMS_Id == a.ASMS_Id && c.MI_Id==b.MI_Id)
                //                           select new
                //                           {
                //                               section = d.ASMC_SectionName,
                //                               stud_count = a.AMST_Id
                //                           }).Distinct().GroupBy(id => id.section).Select(g => new ADMClassSectionStrengthDTO { section = g.Key, stud_count = g.Count() }).ToArray();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CH_CLASS_SECTION_STRENGTH_GRID";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.classid
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
                        data.fillsectioncount = retObject.ToArray();
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


        public ADMClassSectionStrengthDTO Getsection(ADMClassSectionStrengthDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                //List<ADMClassSectionStrengthDTO> result3 = new List<ADMClassSectionStrengthDTO>();
               
                

                data.Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from c in _db.Adm_M_Student
                                           from d in _db.Section
                                           where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && (c.AMST_SOL.Equals("S")|| c.AMST_SOL.Equals("L"))
                                         //  && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 
                                           && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.classid && d.ASMS_Id == a.ASMS_Id)
                                           select new
                                           {
                                               section = d.ASMC_SectionName,
                                               stud_count = a.AMST_Id
                                           }).Distinct().GroupBy(id => id.section).Select(g => new ADMClassSectionStrengthDTO { section = g.Key, stud_count = g.Count() }).ToArray();


                //get class-sectionwise  student
                


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public ADMClassSectionStrengthDTO getclass(ADMClassSectionStrengthDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                List<ADMClassSectionStrengthDTO> result3 = new List<ADMClassSectionStrengthDTO>();
               

                data.classarray = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)


                                   select new ADMClassSectionStrengthDTO
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
