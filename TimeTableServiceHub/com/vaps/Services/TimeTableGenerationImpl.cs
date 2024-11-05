using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using AutoMapper;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TimeTableGenerationImpl : Interfaces.TimeTableGenerationInterface
    {
        private static ConcurrentDictionary<string, TT_Final_GenerationDMO> _login =
                new ConcurrentDictionary<string, TT_Final_GenerationDMO>();


        public TTContext _ttcontext;
        public TimeTableGenerationImpl(TTContext ttcategory)
        {
            _ttcontext = ttcategory;
        }
        public TT_Final_GenerationDTO getdetails(TT_Final_GenerationDTO data)
        {
            TT_Final_GenerationDTO TTMC = new TT_Final_GenerationDTO();

            try
            {
                TTMC.versionlist = (from a in _ttcontext.TT_Final_GenerationDMO
                                    from b in _ttcontext.AcademicYear
                                    from c in _ttcontext.TTMasterCategoryDMO
                                    from d in _ttcontext.institution
                                    where (a.ASMAY_Id == b.ASMAY_Id && a.TTMC_Id==c.TTMC_Id && a.MI_Id == d.MI_Id && a.MI_Id==data.MI_Id)
                                    select new TT_Final_GenerationDTO
                                    {
                                        TTFG_Id = a.TTFG_Id,
                                        Insname = d.MI_Name,
                                        asmayname = b.ASMAY_Year,
                                        categoryname = c.TTMC_CategoryName,
                                        TTFG_ActiveFlag=a.TTFG_ActiveFlag

                                    }).ToList().ToArray(); 


                TTMC.academiclist = _ttcontext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true).OrderByDescending(y=>y.ASMAY_Order).ToList().ToArray();
                TTMC.catelist = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();
            }
            catch (Exception ee)
            {
                TTMC.returnval = false;
            }
            return TTMC;
        }

        public TT_Final_GenerationDTO get_catg(TT_Final_GenerationDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new TT_Final_GenerationDTO
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
        public TT_Final_GenerationDTO get_count(TT_Final_GenerationDTO data)
        {
            try
            {
                List<TT_Final_GenerationDTO> result = new List<TT_Final_GenerationDTO>();
                List<TT_Final_GenerationDTO> result1 = new List<TT_Final_GenerationDTO>();
                List<TT_Final_GenerationDTO> result2 = new List<TT_Final_GenerationDTO>();

                List<long> catids = new List<long>();
                var ttmcs_ids = "";
                foreach (var x in data.ttmc_idslist)
                {
                    ttmcs_ids += x.TTMC_Id + ",";

                    catids.Add(x.TTMC_Id);
                }
                ttmcs_ids = ttmcs_ids.Substring(0, (ttmcs_ids.Length - 1));

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_Get_workload_and_count";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ttmcid", SqlDbType.VarChar) { Value = ttmcs_ids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TT_Final_GenerationDTO
                                {
                                    EmployeeName = dataReader["EmployeeName"].ToString(),
                                    ClassName = dataReader["ClassName"].ToString(),
                                    SubjectName = dataReader["SubjectName"].ToString(),
                                    SectionName = dataReader["SectionName"].ToString(),
                                    TotalNoOfPeriods = Convert.ToInt32(dataReader["TotalPeriods"].ToString())
                                });
                                data.Workloadpdf = result.ToArray();
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    result1.Add(new TT_Final_GenerationDTO
                                    {
                                        ClassName = dataReader["ClassName"].ToString(),
                                        SectionName = dataReader["SectionName"].ToString(),
                                        total_workload = Convert.ToInt32(dataReader["Total_Workload"].ToString())
                                    });
                                    data.Workloadpdf1 = result1.ToArray();
                                }
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    result2.Add(new TT_Final_GenerationDTO
                                    {
                                        totalpriodscount = Convert.ToInt64(dataReader["totalpriodscount"].ToString()),
                                        totalallotedcount = Convert.ToInt64(dataReader["totalallotedcount"].ToString()),
                                        totalnotallotedcount = Convert.ToInt64(dataReader["totalnotallotedcount"].ToString())
                                    });
                                    data.countArray = result2.ToArray();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.Time_Table_new = (from a in _ttcontext.TT_Master_DayDMO
                                       from b in _ttcontext.TT_Master_PeriodDMO
                                       from c in _ttcontext.School_M_Class
                                       from d in _ttcontext.School_M_Section
                                       from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcontext.HR_Master_Employee_DMO
                                       from g in _ttcontext.TT_Final_GenerationDMO
                                       from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcontext.TTMasterCategoryDMO
                                       from j in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && j.HRME_Id == f.HRME_Id)
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

                var fixingperiodcntlist = _ttcontext.TT_Fixing_Day_PeriodDMO.Where(ww => ww.MI_Id == data.MI_Id && ww.ASMAY_Id == data.ASMAY_Id && ww.TTFDP_ActiveFlag == true && ww.TTFDP_AllotedFlag == "No" && catids.Contains(ww.TTMC_Id)).ToList();

                var fixingperiodstaffcntlist = (from a in _ttcontext.TT_Fixing_Period_StaffDMO
                                                from b in _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO
                                                from c in _ttcontext.TT_Category_Class_DMO
                                                where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && catids.Contains(c.TTMC_Id) && a.TTFPS_ActiveFlag == true && b.TTFPSCC_ActiveFlag == true && c.TTCC_ActiveFlag == true && a.TTFPS_AllotedFlag == "No" && a.TTFPS_Id==b.TTFPS_Id
                                                select a).Distinct().ToList();


                var bifurcationcntlist = _ttcontext.TT_Bifurcation_DMO.Where(e => e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && catids.Contains(e.TTMC_Id) && e.TTB_ActiveFlag == true && e.TTB_AllotedFlag == "No").ToList();
                var consecutivecntlist = _ttcontext.TT_ConsecutiveDMO.Where(e => e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && catids.Contains(e.TTMC_Id) && e.TTC_ActiveFlag == true && e.TTC_AllotedFlag == "N").ToList();

                data.fixingperiodcnt = fixingperiodcntlist.Count;
                data.fixingperiodstaffcnt = fixingperiodstaffcntlist.Count;
                data.bifurcationcnt = bifurcationcntlist.Count;
                data.consecutivecnt = consecutivecntlist.Count;

            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        //generation
        public TT_Final_GenerationDTO generate(TT_Final_GenerationDTO _category)
        {
            try
            {
                var ttmcs_ids = "";
                foreach (var x in _category.ttmc_idslist)
                {
                    ttmcs_ids += x.TTMC_Id + ",";
                }
                ttmcs_ids = ttmcs_ids.Substring(0, (ttmcs_ids.Length - 1));

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "tt_final_gen_process";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = _category.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = _category.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ttmc_id", SqlDbType.VarChar) { Value = ttmcs_ids });
                    cmd.Parameters.Add(new SqlParameter("@FXPRD", SqlDbType.VarChar) { Value = _category.FXPRD });
                    cmd.Parameters.Add(new SqlParameter("@CLSFXPRD", SqlDbType.VarChar) { Value = _category.CLSFXPRD });
                    cmd.Parameters.Add(new SqlParameter("@BFPRD", SqlDbType.VarChar) { Value = _category.BFPRD });
                    cmd.Parameters.Add(new SqlParameter("@CNSPRD", SqlDbType.VarChar) { Value = _category.CNSPRD });
                    cmd.Parameters.Add(new SqlParameter("@THREESDC", SqlDbType.VarChar) { Value = _category.THREESDC });
                    cmd.Parameters.Add(new SqlParameter("@THREESDCREP", SqlDbType.VarChar) { Value = _category.THREESDCREP });
                    cmd.Parameters.Add(new SqlParameter("@TWOSDC", SqlDbType.VarChar) { Value = _category.TWOSDC });
                    cmd.Parameters.Add(new SqlParameter("@TWOSDCREP", SqlDbType.VarChar) { Value = _category.TWOSDCREP });
                    cmd.Parameters.Add(new SqlParameter("@ONESDC", SqlDbType.VarChar) { Value = _category.ONESDC });
                    cmd.Parameters.Add(new SqlParameter("@ONESDCREP", SqlDbType.VarChar) { Value = _category.ONESDCREP });
                    cmd.Parameters.Add(new SqlParameter("@NONSDC", SqlDbType.VarChar) { Value = _category.NONSDC });
                    cmd.Parameters.Add(new SqlParameter("@AVLPRD", SqlDbType.VarChar) { Value = _category.AVLPRD });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var a = cmd.ExecuteNonQuery();
                    _category.returnval = true;
                }
            }
            catch (Exception ee)
            {
                _category.returnval = false;
            }
            return _category;
        }
        public TT_Final_GenerationDTO getalldetailsviewrecords(TT_Final_GenerationDTO _category)
        {
            TT_Final_GenerationDTO objpge = Mapper.Map<TT_Final_GenerationDTO>(_category);
            List<TT_Final_GenerationDTO> list = new List<TT_Final_GenerationDTO>();
            try
            {
                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList();
                objpge.periodslst = allperiods.ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                for (int i = 0; i < 1; i++)
                {
                    var temp_stfid = objpge.StaffID;
                    objpge.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                         from b in _ttcontext.TT_Master_PeriodDMO
                                         from c in _ttcontext.School_M_Class
                                         from d in _ttcontext.School_M_Section
                                         from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                         from f in _ttcontext.HR_Master_Employee_DMO
                                         from g in _ttcontext.TT_Final_GenerationDMO
                                         from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                         from ii in _ttcontext.TTMasterCategoryDMO
                                         where (g.MI_Id == objpge.MI_Id
                                         && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id
                                         && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id &&
                                         h.HRME_Id == temp_stfid && g.ASMAY_Id == objpge.ASMAY_Id)
                                         select new TT_Final_GenerationDTO
                                         {
                                             TTFGD_Id = h.TTFGD_Id,
                                             TTFG_Id = g.TTFG_Id,
                                             ASMCL_Id = h.ASMCL_Id,
                                             ASMS_Id = h.ASMS_Id,
                                             HRME_Id = h.HRME_Id,
                                             ISMS_Id = h.ISMS_Id,
                                             TTMD_Id = h.TTMD_Id,
                                             TTMP_Id = Convert.ToInt64(h.TTMP_Id),
                                             TTMC_Id = g.TTMC_Id,
                                             TTMD_DayName = a.TTMD_DayName,
                                             TTMP_PeriodName = b.TTMP_PeriodName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = d.ASMC_SectionName,
                                             staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                             ISMS_SubjectName = e.ISMS_SubjectName,
                                             TTMC_CategoryName = ii.TTMC_CategoryName,

                                         }
                                  ).Distinct().ToArray();



                    foreach (TT_Final_GenerationDTO dto in objpge.Time_Table)
                    {
                        list.Add(dto);
                    }
                }
                objpge.TT = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;

        }
        public TT_Final_GenerationDTO saveTemptomain(TT_Final_GenerationDTO _category)
        {
            try
            {
                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                {
                    string clsId = _category.TempararyArrayList[i].ASMCL_Id.ToString();
                    string secId = _category.TempararyArrayList[i].ASMS_Id.ToString();
                    string empId = _category.TempararyArrayList[i].HRME_Id.ToString();
                    string subId = _category.TempararyArrayList[i].ISMS_Id.ToString();
                    string dayId = _category.TempararyArrayList[i].TTMD_Id.ToString();
                    string periodId = _category.TempararyArrayList[i].TTMP_Id.ToString();

                    var contactExists = _ttcontext.Database.ExecuteSqlCommand("TT_transfer_temp_to_main @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", clsId, secId, empId, subId, dayId, periodId, _category.MI_Id, _category.ASMAY_Id);
                    if (contactExists == 1)
                    {
                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                _category.returnval = false;
            }
            return _category;
        }
        public TT_Final_GenerationDTO Get_temp_data(TT_Final_GenerationDTO data)
        {
            List<TT_Final_GenerationDTO> result = new List<TT_Final_GenerationDTO>();
            try
            {

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_Get_tempData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@staffwise", SqlDbType.Int) { Value = data.STAFFSDKP });
                    cmd.Parameters.Add(new SqlParameter("@subwise", SqlDbType.Int) { Value = data.CLSSDKP });
                    cmd.Parameters.Add(new SqlParameter("@staff_sub_wise", SqlDbType.Int) { Value = data.STAFF_CONSDKP });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TT_Final_GenerationDTO
                                {
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"].ToString()),
                                    ASMS_Id = Convert.ToInt64(dataReader["ASMS_Id"].ToString()),
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"].ToString()),
                                    ISMS_Id = Convert.ToInt64(dataReader["ISMS_Id"].ToString()),
                                    TTMD_Id = Convert.ToInt64(dataReader["TTMD_Id"].ToString()),
                                    TTMP_Id = Convert.ToInt64(dataReader["TTMP_Id"].ToString()),
                                    TTMD_DayName = dataReader["TTMD_DayName"].ToString(),
                                    TTMP_PeriodName = dataReader["TTMP_PeriodName"].ToString(),
                                    ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                    ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                                    ISMS_SubjectName = dataReader["ISMS_SubjectName"].ToString(),
                                    staffName = dataReader["TTMSAB_Abbreviation"].ToString(),
                                });
                                data.Time_Table_new = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }
        public TT_Final_GenerationDTO getreplacemntdetailsviewrecords(TT_Final_GenerationDTO _category)
        {
            TT_Final_GenerationDTO objpge = Mapper.Map<TT_Final_GenerationDTO>(_category);
            List<TT_Final_GenerationDTO> list = new List<TT_Final_GenerationDTO>();
            try
            {
                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList();
                objpge.periodslst1 = allperiods.ToArray();


                objpge.gridweeks1 = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                long category_id = _ttcontext.TT_Category_Class_DMO.Single(t => t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.ASMCL_Id.Equals(objpge.ASMCL_Id)).TTMC_Id;

                objpge.TT1 = (from a in _ttcontext.TT_Master_DayDMO
                              from b in _ttcontext.TT_Master_PeriodDMO
                              from c in _ttcontext.School_M_Class
                              from d in _ttcontext.School_M_Section
                              from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                              from f in _ttcontext.HR_Master_Employee_DMO
                              from g in _ttcontext.TT_Final_GenerationDMO
                              from h in _ttcontext.TT_Final_Generation_DetailedDMO
                              from i in _ttcontext.TTMasterCategoryDMO
                              from j in _ttcontext.TT_Master_Staff_AbbreviationDMO
                              where (g.MI_Id == _category.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && g.TTMC_Id == category_id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == _category.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == _category.ASMCL_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == _category.ASMS_Id && j.HRME_Id == f.HRME_Id && j.MI_Id == _category.MI_Id)
                              select new TT_Final_GenerationDTO
                              {
                                  TTFGD_Id = h.TTFGD_Id,
                                  TTFG_Id = g.TTFG_Id,
                                  ASMCL_Id = h.ASMCL_Id,
                                  ASMS_Id = h.ASMS_Id,
                                  HRME_Id = h.HRME_Id,
                                  ISMS_Id = h.ISMS_Id,
                                  TTMD_Id = h.TTMD_Id,
                                  TTMP_Id = Convert.ToInt64(h.TTMP_Id),
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;

        }
        public TT_Final_GenerationDTO resetTT(TT_Final_GenerationDTO data)
        {
            try
            {
                List<TT_Final_GenerationDTO> result = new List<TT_Final_GenerationDTO>();

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "TT_RESET";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    //cmd.Parameters.Add(new SqlParameter("@ttmc_id", SqlDbType.VarChar) { Value = ttmcs_ids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var a = cmd.ExecuteNonQuery();
                    data.returnval = true;
                }


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }
        public TT_Final_GenerationDTO deactivate(TT_Final_GenerationDTO acd)
        {
            try
            {
                TT_Final_GenerationDMO pge = Mapper.Map<TT_Final_GenerationDMO>(acd);
                if (pge.TTFG_Id > 0)
                {
                    var result = _ttcontext.TT_Final_GenerationDMO.Single(t => t.TTFG_Id.Equals(pge.TTFG_Id));
                    if (result.TTFG_ActiveFlag.Equals(true))
                    {
                        result.TTFG_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTFG_ActiveFlag = true;
                    }
                    _ttcontext.Update(result);
                    var flag = _ttcontext.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}
