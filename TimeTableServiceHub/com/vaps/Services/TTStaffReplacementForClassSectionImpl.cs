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
    public class TTStaffReplacementForClassSectionImpl : Interfaces.StaffReplacementForClassSectionInterface
    {

        private static ConcurrentDictionary<string, TTStaffReplacementForClassSectionDTO> _login =
               new ConcurrentDictionary<string, TTStaffReplacementForClassSectionDTO>();


        public TTContext _ttcategorycontext;
        public TTStaffReplacementForClassSectionImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public TTStaffReplacementForClassSectionDTO getdetails(int id)
        {
            TTStaffReplacementForClassSectionDTO data = new TTStaffReplacementForClassSectionDTO();
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active == true).OrderByDescending(r=>r.ASMAY_Order).ToList().ToArray();
                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                data.staffDrpDwn = (from f in _ttcategorycontext.HR_Master_Employee_DMO
                                    from TT_Master_Staff_AbbreviationDMO in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (f.MI_Id.Equals(id) && f.HRME_ActiveFlag.Equals(true) && TT_Master_Staff_AbbreviationDMO.HRME_Id == f.HRME_Id && TT_Master_Staff_AbbreviationDMO.MI_Id == id)
                                    select new TTStaffReplacementInUnallocatedPeriodDTO
                                    {
                                        HRME_Id = f.HRME_Id,
                                        staffNamelst = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                    }
                                ).ToArray();

              data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList().ToArray();
                data.classlist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                  from b in _ttcategorycontext.TT_Category_Class_DMO
                                  from c in _ttcategorycontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && b.TTCC_ActiveFlag == true)
                                  select new TTStaffReplacementForClassSectionDTO
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

        public TTStaffReplacementForClassSectionDTO get_catg(TTStaffReplacementForClassSectionDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new TTStaffReplacementForClassSectionDTO
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

        public TTStaffReplacementForClassSectionDTO getclass_catg(TTStaffReplacementForClassSectionDTO data)
        {
            try
            {
                data.classlist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                  from b in _ttcategorycontext.TT_Category_Class_DMO
                                  from c in _ttcategorycontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                  select new TTStaffReplacementForClassSectionDTO
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


        public TTStaffReplacementForClassSectionDTO getreport(TTStaffReplacementForClassSectionDTO data)
        {
            try
            {
      data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag==true).Select(r => r.ASMAY_Id).OrderByDescending(s=>s).FirstOrDefault();

               // asmay_id = 39;
                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

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
                                   where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && g.TTMC_Id == data.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == asmay_id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == data.ASMCL_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == data.ASMS_Id && j.HRME_Id == f.HRME_Id && j.MI_Id == data.MI_Id)
                                   select new TTStaffReplacementForClassSectionDTO
                                   {
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
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == asmay_id && a.ASMCL_Id == data.ASMCL_Id && a.TTMB_ActiveFlag == true && a.TTMC_Id == data.TTMC_Id)
                                   select new TTStaffReplacementForClassSectionDTO
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                       TTMB_AfterPeriod = a.TTMB_AfterPeriod,
                                       TTMB_BreakName = a.TTMB_BreakName,
                                   }).Distinct().OrderBy(x => x.TTMB_AfterPeriod).ToArray();
 data.Break_list_all = _ttcategorycontext.TTBreakTimeSettingsDMO.AsNoTracking().Where(b => b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == asmay_id && b.TTMC_Id == data.TTMC_Id && b.MI_Id == data.MI_Id && b.TTMB_ActiveFlag == true).OrderBy(x => x.TTMB_AfterPeriod).ToList().ToArray();

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }     

        public TTStaffReplacementForClassSectionDTO getpossiblePeriod(TTStaffReplacementForClassSectionDTO data)
        {
            var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(e=>e).FirstOrDefault();

          //  asmay_id = 39;
            List<TTStaffReplacementForClassSectionDTO> result = new List<TTStaffReplacementForClassSectionDTO>();
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
                            result.Add(new TTStaffReplacementForClassSectionDTO
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

        public TTStaffReplacementForClassSectionDTO savedetail(TTStaffReplacementForClassSectionDTO data)
        {
            try
            {
                var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(s => s).FirstOrDefault();

                long category_id = _ttcategorycontext.TT_Category_Class_DMO.Single(t => t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(asmay_id) && t.ASMCL_Id.Equals(data.ASMCL_Id)).TTMC_Id;
                if (data.TTMD_ID_from > 0 && data.TTMP_ID_from > 0 && data.TTMD_ID_to > 0 && data.TTMP_ID_to > 0)
                {


                    long Primary_ID1 = (from a in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                        from b in _ttcategorycontext.TT_Final_GenerationDMO
                                        where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id &&  b.MI_Id == data.MI_Id && a.TTMD_Id==data.TTMD_ID_from && a.TTMP_Id==data.TTMP_ID_from && b.ASMAY_Id==asmay_id && b.TTMC_Id== category_id)
                                        select new { a.TTFGD_Id }).Distinct().Select(r => r.TTFGD_Id).FirstOrDefault();

                    long Primary_ID2 = (from a in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                        from b in _ttcategorycontext.TT_Final_GenerationDMO
                                        where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_to && a.TTMP_Id == data.TTMP_ID_to && b.ASMAY_Id == asmay_id && b.TTMC_Id == category_id) 
                                        select new  { a.TTFGD_Id }).Distinct().Select(r => r.TTFGD_Id).FirstOrDefault();


                    var First_value = _ttcategorycontext.TT_Final_Generation_DetailedDMO.Single(o => o.TTFGD_Id.Equals(Primary_ID1));
                    var Second_value = _ttcategorycontext.TT_Final_Generation_DetailedDMO.Single(o => o.TTFGD_Id.Equals(Primary_ID2));

                    First_value.TTMD_Id = data.TTMD_ID_to;
                    First_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_to);
                    _ttcategorycontext.Update(First_value);
                    var contactExists= _ttcategorycontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        Second_value.TTMD_Id = data.TTMD_ID_from;
                        Second_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_from);
                        _ttcategorycontext.Update(Second_value);
                        _ttcategorycontext.SaveChanges();
                        data.returnval = true;

                    }
                    else
                    {
                        data.returnval = false;
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

