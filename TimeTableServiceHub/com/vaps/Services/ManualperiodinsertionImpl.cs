using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
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


namespace TimeTableServiceHub.com.vaps.Services
{
    public class ManualperiodinsertionImpl : Interfaces.ManualperiodinsertionInterface
    {

        private static ConcurrentDictionary<string, ManualperiodinsertionDTO> _login =
               new ConcurrentDictionary<string, ManualperiodinsertionDTO>();


        public TTContext _ttcategorycontext;
        public ManualperiodinsertionImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public ManualperiodinsertionDTO getdetails(int id)
        {
            ManualperiodinsertionDTO data = new ManualperiodinsertionDTO();
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active == true).OrderByDescending(r => r.ASMAY_Order).ToList().ToArray();
                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                data.staffDrpDwn = (from f in _ttcategorycontext.HR_Master_Employee_DMO
                                    from TT_Master_Staff_AbbreviationDMO in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (f.MI_Id.Equals(id) && f.HRME_ActiveFlag.Equals(true) && TT_Master_Staff_AbbreviationDMO.HRME_Id == f.HRME_Id && TT_Master_Staff_AbbreviationDMO.MI_Id == id)
                                    select new TTStaffReplacementInUnallocatedPeriodDTO
                                    {
                                        HRME_Id = f.HRME_Id,
                                        staffNamelst = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == "  " || f.HRME_EmployeeMiddleName == "0" ? "  " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? "  " : f.HRME_EmployeeLastName),
                                    }
                                ).Distinct().OrderBy(f => f.staffNamelst).ToArray();

                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList().Distinct().ToArray();
                data.classlist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                  from b in _ttcategorycontext.TT_Category_Class_DMO
                                  from c in _ttcategorycontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && b.TTCC_ActiveFlag == true)
                                  select new ManualperiodinsertionDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName,
                                      TTMC_Id = a.TTMC_Id,
                                      TTMC_CategoryName = a.TTMC_CategoryName,
                                  }
     ).Distinct().GroupBy(c => c.ASMCL_Id).Select(c => c.First()).ToArray();
                data.sectionlist = _ttcategorycontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList().Distinct().ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public ManualperiodinsertionDTO get_catg(ManualperiodinsertionDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new ManualperiodinsertionDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        public ManualperiodinsertionDTO getclass_catg(ManualperiodinsertionDTO data)
        {
            try
            {
                data.classlist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                  from b in _ttcategorycontext.TT_Category_Class_DMO
                                  from c in _ttcategorycontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id && b.ASMAY_Id == data.ASMAY_Id) //&& b.TTCC_ActiveFlag==true
                                  select new ManualperiodinsertionDTO
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


        public ManualperiodinsertionDTO getreport(ManualperiodinsertionDTO data)
        {
            try
            {

                data.periodslst = (from a in _ttcategorycontext.TT_Master_PeriodDMO
                                   from b in _ttcategorycontext.TT_Master_Period_ClasswiseDMO
                                   where a.TTMP_Id == b.TTMP_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.TTMP_ActiveFlag.Equals(true) && b.TTMPC_ActiveFlag == true
                                   select a

                                  ).Distinct().ToArray();



                data.gridweeks = (from a in _ttcategorycontext.TT_Master_DayDMO
                                  from b in _ttcategorycontext.TT_Master_Day_ClasswiseDMO
                                  where a.TTMD_Id == b.TTMD_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.TTMD_ActiveFlag.Equals(true) && b.TTMDC_ActiveFlag == true
                                  select a


                               ).Distinct().ToArray();

                data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                   from b in _ttcategorycontext.TT_Master_PeriodDMO
                                   from c in _ttcategorycontext.School_M_Class
                                   from d in _ttcategorycontext.School_M_Section
                                   from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                   from f in _ttcategorycontext.HR_Master_Employee_DMO
                                   from g in _ttcategorycontext.TT_Final_GenerationDMO
                                   from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                   from i in _ttcategorycontext.TTMasterCategoryDMO
                                   from j in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                   where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && g.TTMC_Id == data.TTMC_Id && d.ASMS_Id == h.ASMS_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == data.ASMCL_Id && a.TTMD_Id == h.TTMD_Id && i.TTMC_Id == g.TTMC_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == data.ASMS_Id && j.HRME_Id == f.HRME_Id && f.HRME_Id == h.HRME_Id && j.MI_Id == data.MI_Id)

                                   select new ManualperiodinsertionDTO
                                   {
                                       ASMAY_Id = g.ASMAY_Id,
                                       TTFGD_Id = h.TTFGD_Id,
                                       TTFG_Id = g.TTFG_Id,
                                       ASMCL_Id = h.ASMCL_Id,
                                       ASMS_Id = h.ASMS_Id,
                                       HRME_Id = h.HRME_Id,
                                       ISMS_Id = h.ISMS_Id,
                                       TTMD_Id = h.TTMD_Id,
                                       TTMP_Id = h.TTMP_Id,
                                       TTMC_Id = g.TTMC_Id,
                                       TTMD_DayName = a.TTMD_DayName,
                                       TTMP_PeriodName = b.TTMP_PeriodName,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       ISMS_SubjectName = e.ISMS_SubjectName,
                                       staffName = j.TTMSAB_Abbreviation,
                                       TTMC_CategoryName = i.TTMC_CategoryName,
                                   }
                              ).Distinct().ToArray();


                data.Break_list = (from a in _ttcategorycontext.TTBreakTimeSettingsDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.TTMB_ActiveFlag == true && a.TTMC_Id == data.TTMC_Id)
                                   select new ManualperiodinsertionDTO
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                       TTMB_AfterPeriod = a.TTMB_AfterPeriod,
                                       TTMB_BreakName = a.TTMB_BreakName,
                                   }).Distinct().OrderBy(x => x.TTMB_AfterPeriod).ToArray();

                data.Break_list_all = _ttcategorycontext.TTBreakTimeSettingsDMO.AsNoTracking().Where(b => b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == data.TTMC_Id && b.MI_Id == data.MI_Id && b.TTMB_ActiveFlag == true).OrderBy(x => x.TTMB_AfterPeriod).ToList().Distinct().ToArray();

                data.subjectlist = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        //public ManualperiodinsertionDTO getreport(ManualperiodinsertionDTO data)
        //{
        //    try
        //    {

        //        data.periodslst = (from a in _ttcategorycontext.TT_Master_PeriodDMO
        //                           from b in _ttcategorycontext.TT_Master_Period_ClasswiseDMO
        //                           where a.TTMP_Id == b.TTMP_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.TTMP_ActiveFlag.Equals(true) && b.TTMPC_ActiveFlag == true
        //                           select a

        //                          ).Distinct().ToArray();



        //        data.gridweeks = (from a in _ttcategorycontext.TT_Master_DayDMO
        //                          from b in _ttcategorycontext.TT_Master_Day_ClasswiseDMO
        //                          where a.TTMD_Id == b.TTMD_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.TTMD_ActiveFlag.Equals(true) && b.TTMDC_ActiveFlag == true
        //                          select a


        //                       ).Distinct().ToArray();

        //        data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
        //                           from b in _ttcategorycontext.TT_Master_PeriodDMO
        //                           from c in _ttcategorycontext.School_M_Class
        //                           from d in _ttcategorycontext.School_M_Section
        //                           from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
        //                           from f in _ttcategorycontext.HR_Master_Employee_DMO
        //                           from g in _ttcategorycontext.TT_Final_GenerationDMO
        //                           from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
        //                           from i in _ttcategorycontext.TTMasterCategoryDMO
        //                           from j in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
        //                           where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && g.TTMC_Id == data.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == data.ASMCL_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == data.ASMS_Id && j.HRME_Id == f.HRME_Id && j.MI_Id == data.MI_Id)
        //                           select new ManualperiodinsertionDTO
        //                           {
        //                               TTFGD_Id = h.TTFGD_Id,
        //                               TTFG_Id = g.TTFG_Id,
        //                               ASMCL_Id = h.ASMCL_Id,
        //                               ASMS_Id = h.ASMS_Id,
        //                               HRME_Id = h.HRME_Id,
        //                               ISMS_Id = h.ISMS_Id,
        //                               TTMD_Id = h.TTMD_Id,
        //                               TTMP_Id = h.TTMP_Id,
        //                               TTMC_Id = g.TTMC_Id,
        //                               TTMD_DayName = a.TTMD_DayName,
        //                               TTMP_PeriodName = b.TTMP_PeriodName,
        //                               ASMCL_ClassName = c.ASMCL_ClassName,
        //                               ASMC_SectionName = d.ASMC_SectionName,
        //                               ISMS_SubjectName = e.ISMS_SubjectName,
        //                               staffName = j.TTMSAB_Abbreviation,
        //                               TTMC_CategoryName = i.TTMC_CategoryName,

        //                           }
        //                      ).Distinct().ToArray();


        //        data.Break_list = (from a in _ttcategorycontext.TTBreakTimeSettingsDMO
        //                           where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.TTMB_ActiveFlag == true && a.TTMC_Id == data.TTMC_Id)
        //                           select new ManualperiodinsertionDTO
        //                           {
        //                               ASMCL_Id = a.ASMCL_Id,
        //                               TTMB_AfterPeriod = a.TTMB_AfterPeriod,
        //                               TTMB_BreakName = a.TTMB_BreakName,
        //                           }).Distinct().OrderBy(x => x.TTMB_AfterPeriod).ToArray();
        //        data.Break_list_all = _ttcategorycontext.TTBreakTimeSettingsDMO.AsNoTracking().Where(b => b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == data.TTMC_Id && b.MI_Id == data.MI_Id && b.TTMB_ActiveFlag == true).OrderBy(x => x.TTMB_AfterPeriod).ToList().ToArray();


        //        data.subjectlist = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        data.returnval = false;
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;

        //}


        public ManualperiodinsertionDTO getpossiblePeriod(ManualperiodinsertionDTO data)
        {
            var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(e => e).FirstOrDefault();

            //  asmay_id = 39;
            List<ManualperiodinsertionDTO> result = new List<ManualperiodinsertionDTO>();
            long category_id = _ttcategorycontext.TT_Category_Class_DMO.Single(t => t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(asmay_id) && t.ASMCL_Id.Equals(data.ASMCL_Id)).TTMC_Id;


            using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "TT_Get_Allpossibilities_for_replacement";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = asmay_id });
                cmd.Parameters.Add(new SqlParameter("@ttmc_id", SqlDbType.BigInt) { Value = category_id });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                cmd.Parameters.Add(new SqlParameter("@ttmd_id", SqlDbType.BigInt) { Value = data.TTMD_Id });
                cmd.Parameters.Add(new SqlParameter("@ttmp_id", SqlDbType.BigInt) { Value = data.TTMP_Id });
                cmd.Parameters.Add(new SqlParameter("@staffwiseflag", SqlDbType.BigInt) { Value = data.staffSDK });
                cmd.Parameters.Add(new SqlParameter("@subjectwiseflag", SqlDbType.BigInt) { Value = data.subSDK });
                cmd.Parameters.Add(new SqlParameter("@Consecutivewiseflag", SqlDbType.BigInt) { Value = data.conSDK });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            result.Add(new ManualperiodinsertionDTO
                            {
                                TTMD_Id = Convert.ToInt64(dataReader["TTMD_Id"].ToString()),
                                TTMP_Id = Convert.ToInt32(dataReader["TTMP_Id"].ToString()),
                            });
                            data.Data_lst = result.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            return data;
        }

        public ManualperiodinsertionDTO savedetail(ManualperiodinsertionDTO data)
        {
            try
            {
                var checkver = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true && t.ASMAY_Id == data.ASMAY_Id && t.TTMC_Id == data.TTMC_Id).ToList();

                if (checkver.Count == 0)
                {
                    TT_Final_GenerationDMO obj1 = new TT_Final_GenerationDMO();
                    obj1.MI_Id = data.MI_Id;
                    obj1.TTFG_ActiveFlag = true;
                    obj1.ASMAY_Id = data.ASMAY_Id;
                    obj1.TTMC_Id = data.TTMC_Id;
                    obj1.TTFG_VersionNo = "1";
                    obj1.CreatedDate = DateTime.Now;
                    obj1.UpdatedDate = DateTime.Now;
                    _ttcategorycontext.Add(obj1);
                    int res = _ttcategorycontext.SaveChanges();
                }

                var TTFG_Ids = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true && t.ASMAY_Id == data.ASMAY_Id && t.TTMC_Id == data.TTMC_Id).Select(r => r.TTFG_Id).OrderByDescending(s => s).FirstOrDefault();

                var MIId = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true && t.ASMAY_Id == data.ASMAY_Id && t.TTMC_Id == data.TTMC_Id).Select(r => r.MI_Id).OrderByDescending(s => s).FirstOrDefault();

                var TTMCId = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true && t.ASMAY_Id == data.ASMAY_Id && t.TTMC_Id == data.TTMC_Id).Select(r => r.TTMC_Id).OrderByDescending(s => s).FirstOrDefault();

                var ASMAY_Id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && TTMCId == data.TTMC_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(s => s).FirstOrDefault();


                if (data.Time_Table.Length > 0)
                {
                    foreach (var item in data.Time_Table)
                    {
                        //var rstd = (from a in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                        //            from b in _ttcategorycontext.TT_Final_GenerationDMO
                        //            where (a.TTFG_Id == b.TTFG_Id && a.HRME_Id == data.HRME_Id && b.ASMAY_Id == data.ASMAY_Id 
                        //            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                        //            && a.TTMD_Id == item.TTMD_Id && a.TTMP_Id == item.TTMP_Id 
                        //            && b.TTMC_Id == data.TTMC_Id && b.MI_Id == data.MI_Id)
                        //            select a).Distinct().ToList();

                        var rstl = _ttcategorycontext.TT_Final_Generation_DetailedDMO.Where(R => R.HRME_Id == data.HRME_Id && R.ASMCL_Id == data.ASMCL_Id && R.TTMD_Id == item.TTMD_Id && R.TTMP_Id == item.TTMP_Id && R.ASMS_Id == data.ASMS_Id && TTMCId == data.TTMC_Id && MIId == data.MI_Id && R.TTFG_Id==TTFG_Ids).ToList();
                       
                        if (rstl.Count > 0)
                        {
                            Console.WriteLine("Same staff cannot able to insert in particular Period and Day !");
                        }
                        else
                        {
                            TT_Final_Generation_DetailedDMO obj = new TT_Final_Generation_DetailedDMO();

                            obj.TTFG_Id = TTFG_Ids;
                            obj.ASMCL_Id = data.ASMCL_Id;
                            obj.ASMS_Id = data.ASMS_Id;
                            obj.HRME_Id = data.HRME_Id;
                            obj.ISMS_Id = data.ISMS_Id;
                            obj.TTMD_Id = item.TTMD_Id;
                            obj.TTMP_Id = item.TTMP_Id;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _ttcategorycontext.Add(obj);
                            int res = _ttcategorycontext.SaveChanges();
                            if (res > 0)
                            {
                                data.returnval = true;
                                data.sscnt += 1;
                            }
                            else
                            {
                                data.returnval = false;
                                data.ffcnt += 1;
                            }
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
    }
}