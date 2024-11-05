using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTConsolidatedImpl : Interfaces.TTConsolidatedInterface
    {
        public TTContext _ttcontext;
        public PortalContext _PortalContext;
        public ActivateDeactivateContext _Context;

        public DomainModelMsSqlServerContext _db;

        public TTConsolidatedImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
           
        }

        public TTConsolidatedDTO getalldetails(int id)
        {
            TTConsolidatedDTO data = new TTConsolidatedDTO();
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == id && t.Is_Active == true).ToList().Distinct().OrderByDescending(r=>r.ASMAY_Order).ToArray();
                data.dayslst_fix = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(id) ).ToList().ToArray();

                data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                 from b in _ttcontext.TT_Final_GenerationDMO
                                 from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                 from d in _ttcontext.HR_Master_Employee_DMO
                                 where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                 )
                                 select new TTConsolidatedDTO
                                 {
                                     empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                     HRME_Id = c.HRME_Id
                                 }).Distinct().OrderBy(g=>g.empName).ToArray();

                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(a => a.MI_Id == id && a.TTMP_ActiveFlag == true).Distinct().ToArray();

                //data.categorylist = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                data.cateoglst = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                data.classlists = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && b.TTCC_ActiveFlag == true)
                                  select new TTConsolidatedDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName,
                                      TTMC_Id = a.TTMC_Id,
                                      TTMC_CategoryName = a.TTMC_CategoryName,
                                  }
   ).Distinct().GroupBy(c => c.ASMCL_Id).Select(c => c.First()).ToArray();

                //data.sectionlist = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList().Distinct().ToArray();

                data.sectionlists = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList().Distinct().ToArray();


                //List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                //mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList();
                //data.categorylist = mcat.Distinct().ToArray();

                //List<School_M_Section> allsetion = new List<School_M_Section>();
                //allsetion = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList();
                //data.sectionlist = allsetion.Distinct().ToArray();

                //List<School_M_Class> admcls = new List<School_M_Class>();
                //admcls = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                //data.classlist = admcls.ToArray();



                //List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                //mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList();
                //data.categorylist = mcat.Distinct().ToArray();

                //List<School_M_Section> allsetion = new List<School_M_Section>();
                //allsetion = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList();
                //data.sectionlist = allsetion.Distinct().ToArray();

                //List<School_M_Class> admcls = new List<School_M_Class>();
                //admcls = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                //data.classlist = admcls.ToArray();

                //List<School_M_Class> admclss = new List<School_M_Class>();
                //admclss = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                //data.classsslist = admcls.ToArray();


                //List<AcademicYear> year = new List<AcademicYear>();
                //year = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == id && t.Is_Active == true).ToList();
                //data.acayear = year.Distinct().OrderByDescending(l => l.ASMAY_Order).ToArray();

                //List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                //mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList();
                //data.categorylist = mcat.Distinct().ToArray();

                //List<School_M_Section> allsetion = new List<School_M_Section>();
                //allsetion = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList();
                //data.sectionlist = allsetion.Distinct().ToArray();

                //List<School_M_Class> admcls = new List<School_M_Class>();
                //admcls = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                //data.classlst = admcls.ToArray();

                //   data.classlist = (
                //from a in _ttcontext.School_M_Class
                //join b in _PortalContext.IVRM_ClassWorkDMO on a.ASMCL_Id equals b.ASMCL_Id
                //where a.MI_Id == data.MI_Id && b.ICW_ActiveFlag == true
                //select a).Distinct().OrderBy(a => a.ASMCL_Order).Distinct().ToArray();

                //   data.sectionlist = (from a in _Context.AdmSchoolMasterClassCatSec
                //                        from b in _PortalContext.IVRM_ClassWorkDMO
                //                        from c in _ttcontext.School_M_Section
                //                        where (a.ASMS_Id == b.ASMS_Id && b.ASMS_Id == c.ASMS_Id
                //                        && b.ASMCL_Id == data.ASMCL_Id
                //                        && a.ASMCCS_ActiveFlg == true)
                //                        select c).Distinct().OrderBy(a => a.ASMC_Order).Distinct().ToArray();


                // id.classlist = (
                //from a in _ttcontext.School_M_Class
                //join b in _ttcontext.IVRM_ClassWorkDMO on a.ASMCL_Id equals b.ASMCL_Id
                //where a.MI_Id == id.MI_Id && b.ICW_ActiveFlag == true
                //select a).Distinct().OrderBy(a => a.ASMCL_Order).Distinct().ToArray();

                // dto.Section_list = (from a in _ttcontext.AdmSchoolMasterClassCatSec
                //                     from b in _Context.IVRM_ClassWorkDMO
                //                     from c in _Context.School_M_Section
                //                     where (a.ASMS_Id == b.ASMS_Id && b.ASMS_Id == c.ASMS_Id
                //                     && b.ASMCL_Id == dto.ASMCL_Id
                //                     && a.ASMCCS_ActiveFlg == true)
                //                     select c).Distinct().OrderBy(a => a.ASMC_Order).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        //  public TTConsolidatedDTO getclass_catg(TTConsolidatedDTO data)
        //  {
        //      try
        //      {
        //          data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
        //                            from b in _ttcontext.TT_Category_Class_DMO
        //                            from c in _ttcontext.School_M_Class
        //                            where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
        //                            select new TTConsolidatedDTO
        //                            {
        //                                ASMCL_Id = c.ASMCL_Id,
        //                                ASMCL_ClassName = c.ASMCL_ClassName,
        //                                TTMC_Id = a.TTMC_Id,
        //                                TTMC_CategoryName = a.TTMC_CategoryName,
        //                            }
        //).Distinct().ToArray();


        //      }
        //      catch (Exception ee)
        //      {
        //          data.returnval = false;
        //          Console.WriteLine(ee.Message);
        //      }
        //      return data;

        //  }
        public TTConsolidatedDTO getclass_catg(TTConsolidatedDTO data)
        {
            try
            {
                data.classlists = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                  select new TTConsolidatedDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName,
                                      TTMC_Id = a.TTMC_Id,
                                      TTMC_CategoryName = a.TTMC_CategoryName,
                                  }
      ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTConsolidatedDTO getreport(TTConsolidatedDTO data)
        {

            try
            {
                if (data.TYPE == "ATG")
                {
                    //data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();


                    //data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    //data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                    //                 from b in _ttcontext.TT_Final_GenerationDMO
                    //                 from c in _ttcontext.TT_Final_Generation_DetailedDMO
                    //                 from d in _ttcontext.HR_Master_Employee_DMO
                    //                 where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                    //                 //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                    //                 )
                    //                 select new TTConsolidatedDTO
                    //                 {
                    //                     empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                    //                     HRME_Id = c.HRME_Id,
                    //                     TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                    //                 }).Distinct().ToArray();

                    data.HRME_Id = 0;
                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STAFFWORKLOAD_AFTER_TT_GENERATION";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.TYPE == "ST")
                {



                    //string catidss = "0";
                    //string claiss = "0";
                    //string secdss = "0";
                    //if (data.TYPE == "ST")
                    //{
                    //    if (data.albumNameArray1 != null)
                    //    {
                    //        foreach (var g in data.albumNameArray1)
                    //        {
                    //            claiss = claiss + "," + g.ASMCL_Id;
                    //        }
                    //    }
                    //    else if (data.albumNameArray2 != null)
                    //    {
                    //        foreach (var g in data.albumNameArray2)
                    //        {
                    //            secdss = secdss + "," + g.ASMS_Id;
                    //        }
                    //    }
                    //}

                    string catidss = "0";
                    string claiss = "0";
                    string secdss = "0";

                    //if (data.TTclassArray != null)
                    //{
                    //    foreach (var i in data.TTclassArray)
                    //    {
                    //        ttclaiss = ttclaiss + "," + i.ASMCL_Id;
                    //    }
                    //}
                    //if (data.TTsectionArray != null)
                    //{
                    //    foreach (var i in data.TTsectionArray)
                    //    {
                    //        ttsecdss = ttsecdss + "," + i.ASMS_Id;
                    //    }
                    //}

                    foreach (var i in data.classdss)
                    {
                        claiss = claiss + "," + i.ASMCL_Id;
                    }
                    foreach (var i in data.sectiondss)
                    {
                        secdss = secdss + "," + i.ASMS_Id;
                    }

                    //foreach (var i in data.clsidss)
                    //{
                    //    ttclsidss = ttclsidss + "," + i.ASMCL_Id;
                    //}
                    //foreach (var i in data.secidss)
                    //{
                    //    ttsecidss = ttclsidss + "," + i.ASMS_Id;
                    //}

                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();


                    data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().ToArray();


                   

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CONSOLIDATED_REPs";
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
                        cmd.Parameters.Add(new SqlParameter("@TTMC_Id",
                               SqlDbType.VarChar)
                        {
                            Value = data.TTMC_Id
                            // Value = catidss
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                               SqlDbType.VarChar)
                        {
                           /// Value = data.ASMCL_Id
                              Value= claiss
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                               SqlDbType.VarChar)
                        {
                            //Value = data.ASMS_Id

                            Value = secdss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.TYPE == "STD")
                {



                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_CONSOLIDATED_REPORT";
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
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().ToArray();

                }
                else if (data.TYPE == "CSUB" || data.TYPE == "STSUB")
                {

                    string sidss = "0";
                    string didss = "0";
                    string pidss = "0";
                    foreach (var item in data.stfidss)
                    {
                        sidss = sidss + "," + item.HRME_Id;
                    }
                    foreach (var item in data.dayidss)
                    {
                        didss = didss + "," + item.TTMD_Id;
                    }
                    foreach (var item in data.periodidss)
                    {
                        pidss = pidss + "," + item.TTMP_Id;
                    }


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_CONSOLIDATED_REPORT_CLASS_SUBJECT";
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

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@STFIDs",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@DAYIDs",
                               SqlDbType.VarChar)
                        {
                            Value = didss
                        });
                        cmd.Parameters.Add(new SqlParameter("@PERIODIDs",
                               SqlDbType.VarChar)
                        {
                            Value = pidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.subjectlist = (from b in _ttcontext.TT_Final_GenerationDMO
                                        from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                        from d in _ttcontext.IVRM_School_Master_SubjectsDMO
                                        where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1 && c.ISMS_Id == d.ISMS_Id && d.MI_Id == b.MI_Id)
                                        select new TTConsolidatedDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName
                                        }).Distinct().ToArray();

                    data.class_sectons = (from a in _ttcontext.TT_Final_GenerationDMO
                                          from b in _ttcontext.TT_Final_Generation_DetailedDMO
                                          from c in _ttcontext.School_M_Class
                                          from d in _ttcontext.School_M_Section
                                          where (a.TTFG_Id == b.TTFG_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTFG_ActiveFlag == true && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id)
                                          select new TTConsolidatedDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              ASMS_Id = d.ASMS_Id,
                                              ASMCL_ClassName = c.ASMCL_ClassName + " : " + d.ASMC_SectionName,
                                              ASMC_SectionName = d.ASMC_SectionName
                                          }).Distinct().ToArray();


                }
                else if (data.TYPE == "SSW")
                {
                    string sidss = "0";
                    string didss = "0";
                    string pidss = "0";

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_CONSOLIDATED_REPORT_CLASS_SUBJECT";
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

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@STFIDs",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@DAYIDs",
                               SqlDbType.VarChar)
                        {
                            Value = didss
                        });
                        cmd.Parameters.Add(new SqlParameter("@PERIODIDs",
                               SqlDbType.VarChar)
                        {
                            Value = pidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(e => e.empName).ToArray();
                }
                else if (data.TYPE == "SPW")
                {
                    string sidss = "0";
                    string didss = "0";
                    string pidss = "0";
                    foreach (var item in data.stfidss)
                    {
                        sidss = sidss + "," + item.HRME_Id;
                    }
                    foreach (var item in data.dayidss)
                    {
                        didss = didss + "," + item.TTMD_Id;
                    }
                    foreach (var item in data.periodidss)
                    {
                        pidss = pidss + "," + item.TTMP_Id;
                    }


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        //TT_Get_staff_workload
                        //TT_SCHOOL_CONSOLIDATED_REPORT_CLASS_SUBJECT
                        cmd.CommandText = "TT_SCHOOL_CONSOLIDATED_REPORT_CLASS_SUBJECT";
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

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@STFIDs",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@DAYIDs",
                               SqlDbType.VarChar)
                        {
                            Value = didss
                        });
                        cmd.Parameters.Add(new SqlParameter("@PERIODIDs",
                               SqlDbType.VarChar)
                        {
                            Value = pidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(e => e.empName).ToArray();
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
                }
                else if (data.TYPE == "STFP")
                {
                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_DAYWISE_STAFFFREEPERIODS";
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

                        cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMD_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(e => e.empName).ToArray();
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();


                }
                else if (data.TYPE == "PSTF")
                {
                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_DAY_PERIOD_FREE_STAFF";
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

                        cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMP_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.TYPE == "SPC")
                {
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().ToArray();


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_STAFF_CLASS_PERIODWISE_CONSOLIDATED_REPORT";
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
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    
                    
                }
                else if (data.TYPE == "CSPC")
                {
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.class_sectons = (from a in _ttcontext.TT_Final_GenerationDMO
                                          from b in _ttcontext.TT_Final_Generation_DetailedDMO
                                          from c in _ttcontext.School_M_Class
                                          from d in _ttcontext.School_M_Section
                                          where (a.TTFG_Id == b.TTFG_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTFG_ActiveFlag == true && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id)
                                          select new TTConsolidatedDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              ASMS_Id = d.ASMS_Id,
                                              ASMCL_ClassName = c.ASMCL_ClassName + " : " + d.ASMC_SectionName,
                                              ASMC_SectionName = d.ASMC_SectionName
                                          }).Distinct().ToArray();


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_CLASS_SECTION_PERIODWISE_CONSOLIDATED_REPORT";
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
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    
                    
                }

             else   if (data.TYPE == "RMR")
                {
                    data.roomlst = _ttcontext.TT_Master_RoomDMO.Where(c => c.TTMRM_ActiveFlg.Equals(true) && c.MI_Id.Equals(data.MI_Id)).Distinct().ToList().ToArray();

                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();


                    data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.TT_Break_list_all = (from a in _ttcontext.TTBreakTimeSettingsDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTMB_ActiveFlag == true)
                                              select new TTConsolidatedDTO
                                              {
                                                  ASMAY_Id = a.ASMAY_Id,
                                                  TTMD_Id = a.TTMD_Id,
                                                  TTMB_AfterPeriod = a.TTMB_AfterPeriod,
                                                  TTMB_BreakName = a.TTMB_BreakName,
                                              }).Distinct().ToArray();


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_CONSOLIDATED_REPORT_ROOMWISE";
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
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.rpttyp
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
            return data;
            
        }

     

    }
}
