using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class CLGFixingImpl : Interfaces.CLGFixingInterface
    {
        private static ConcurrentDictionary<string, CLGFixingDTO> _login =
             new ConcurrentDictionary<string, CLGFixingDTO>();

        public TTContext _TTContext;
        ILogger<CLGFixingImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGFixingImpl(TTContext academiccontext, ILogger<CLGFixingImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }

        //TAB1 START FIXING DAY PERIOD
        #region LOAD ALL DATA
        public CLGFixingDTO getalldetails(CLGFixingDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.categorylist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.academiclist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();
                data.daydropdown = _TTContext.TT_Master_DayDMO.Where(u => u.MI_Id == data.MI_Id && u.TTMD_ActiveFlag == true).Distinct().ToArray();
                data.periodlist = _TTContext.TT_Master_PeriodDMO.Where(u => u.MI_Id == data.MI_Id && u.TTMP_ActiveFlag == true).Distinct().ToArray();

                data.period_count = data.periodlist.Length;

                data.sectionlist = _TTContext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();

                data.daylist = _TTContext.TT_Master_DayDMO.AsNoTracking().Where(d => d.MI_Id == data.MI_Id && d.TTMD_ActiveFlag == true).ToArray();
                data.day_count = data.daylist.Length;

                //TAB2 LOAD




                ///FIXED DAY PERIODS GRID LOAD
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_LOAD_FIXING_DAY_PERIOD_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.fix_day_period_list = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
                //END

                /// FIXED DAY STAFF GRID LOAD
                data.all_fix_day_staff_list = (from a in _TTContext.AcademicYear
                                               from e in _TTContext.HR_Master_Employee_DMO
                                               from g in _TTContext.TT_Fixing_Day_StaffDMO
                                               from i in _TTContext.TT_Master_DayDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                               select new CLGFixingDTO
                                               {
                                                   TTFDS_Id = g.TTFDS_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTMD_Id = g.TTMD_Id,
                                                   TTMD_DayName = i.TTMD_DayName,
                                                   TTFDS_SUbSelcFlag = g.TTFDS_SUbSelcFlag,
                                                   TTFDS_ActiveFlag = g.TTFDS_ActiveFlag,
                                               }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
                /// END

                ///FIXING DAY SUBJECT       
              data.all_fix_day_subject_list = (from a in _TTContext.AcademicYear
                from e in _TTContext.IVRM_School_Master_SubjectsDMO
                from g in _TTContext.TT_Fixing_Day_SubjectDMO
                from i in _TTContext.TT_Master_DayDMO
                where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                select new CLGFixingDTO
                {
                    TTFDSU_Id = g.TTFDSU_Id,
                    ASMAY_Year = a.ASMAY_Year,
                    ISMS_SubjectName = e.ISMS_SubjectName,
                    TTMD_Id = g.TTMD_Id,
                    TTMD_DayName = i.TTMD_DayName,
                    TTFDSU_SUbSelcFlag = g.TTFDSU_SUbSelcFlag,
                    TTFDSU_ActiveFlag = g.TTFDSU_ActiveFlag,
                }
      ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
                /// END

                ///FIXING PERIOD STAFF
                data.all_fix_period_staff_list = (from a in _TTContext.AcademicYear
                                                  from e in _TTContext.HR_Master_Employee_DMO
                                                  from g in _TTContext.TT_Fixing_Period_StaffDMO
                                                  from b in _TTContext.TT_Master_PeriodDMO
                                                  where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                  select new CLGFixingDTO
                                                  {
                                                      TTFPS_Id = g.TTFPS_Id,
                                                      ASMAY_Year = a.ASMAY_Year,
                                                      staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                      TTMP_Id = g.TTMP_Id,
                                                      TTMP_PeriodName = b.TTMP_PeriodName,
                                                      TTFPS_SUbSelcFlag = g.TTFPS_SUbSelcFlag,
                                                      TTFPS_ActiveFlag = g.TTFPS_ActiveFlag,
                                                  }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
                ///END
              
                /// ///FIXING PERIOD SUBJECT

                data.all_fix_period_subject_list = (from a in _TTContext.AcademicYear
                                                    from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                    from g in _TTContext.TT_Fixing_Period_SubjectDMO
                                                    from b in _TTContext.TT_Master_PeriodDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                    select new CLGFixingDTO
                                                    {
                                                        TTFPSU_Id = g.TTFPSU_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        ISMS_SubjectName = e.ISMS_SubjectName,
                                                        TTMP_Id = g.TTMP_Id,
                                                        TTMP_PeriodName = b.TTMP_PeriodName,
                                                        TTFPSU_SUbSelcFlag = g.TTFPSU_SUbSelcFlag,
                                                        TTFPSU_ActiveFlag = g.TTFPSU_ActiveFlag,
                                                    }
       ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
                ///END


            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE TAB1 DAY PERIODS
        public CLGFixingDTO savetab1(CLGFixingDTO data)
        {
            try
            {
                var restrict_count = _TTContext.CLGTT_Restricting_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.TTMC_Id == data.TTMC_Id && r.AMCO_Id == data.AMCO_Id && r.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && r.ACMS_Id == data.ACMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTMP_Id == data.TTMP_Id && r.HRME_Id == data.HRME_Id && r.ISMS_Id == data.ISMS_Id && r.TTRDPC_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (data.TTFDPC_Id > 0)
                    {
                        var resultCount = _TTContext.CLGTT_Fixing_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.TTMC_Id == data.TTMC_Id && r.AMCO_Id == data.AMCO_Id && r.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && r.ACMS_Id == data.ACMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTMP_Id == data.TTMP_Id && r.HRME_Id == data.HRME_Id && r.ISMS_Id == data.ISMS_Id && r.TTFDPC_Id != data.TTFDPC_Id).Count();
                        if (resultCount == 0)
                        {
                            var res = _TTContext.CLGTT_Fixing_Day_PeriodDMO.Single(f => f.TTFDPC_Id == data.TTFDPC_Id);
                            res.ASMAY_Id = data.ASMAY_Id;
                            res.TTMC_Id = data.TTMC_Id;
                            res.AMCO_Id = data.AMCO_Id;
                            res.AMB_Id = data.AMB_Id;
                            res.AMSE_Id = data.AMSE_Id;
                            res.ACMS_Id = data.ACMS_Id;
                            res.TTMD_Id = data.TTMD_Id;
                            res.TTMP_Id = data.TTMP_Id;
                            res.HRME_Id = data.HRME_Id;
                            res.ISMS_Id = data.ISMS_Id;
                            res.TTFDPC_AllotedFlag = "No";
                            res.UpdatedDate = DateTime.Now;
                            _TTContext.Update(res);
                            var contactExists = _TTContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            data.returnduplicatestatus = "Duplicate";
                            return data;
                        }

                    }
                    else
                    {
                        var resultCount = _TTContext.CLGTT_Fixing_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.TTMC_Id == data.TTMC_Id && r.AMCO_Id == data.AMCO_Id && r.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && r.ACMS_Id == data.ACMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTMP_Id == data.TTMP_Id && r.HRME_Id == data.HRME_Id && r.ISMS_Id == data.ISMS_Id).Count();
                        if (resultCount == 0)
                        {
                            CLGTT_Fixing_Day_PeriodDMO obj = new CLGTT_Fixing_Day_PeriodDMO();
                            obj.MI_Id = data.MI_Id;
                            obj.ASMAY_Id = data.ASMAY_Id;
                            obj.TTMC_Id = data.TTMC_Id;
                            obj.AMCO_Id = data.AMCO_Id;
                            obj.AMB_Id = data.AMB_Id;
                            obj.AMSE_Id = data.AMSE_Id;
                            obj.ACMS_Id = data.ACMS_Id;
                            obj.TTMD_Id = data.TTMD_Id;
                            obj.TTMP_Id = data.TTMP_Id;
                            obj.HRME_Id = data.HRME_Id;
                            obj.ISMS_Id = data.ISMS_Id;
                            obj.TTFDPC_AllotedFlag = "No";
                            obj.TTFDPC_ActiveFlag = true;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _TTContext.Add(obj);
                            var contactExists = _TTContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            data.returnduplicatestatus = "Duplicate";
                            return data;
                        }

                    }
                }
                else
                {
                    data.returnrestrictstatus = "Restricted";
                    return data;
                }

                ///RELOAD THE DATA
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_LOAD_FIXING_DAY_PERIOD_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.fix_day_period_list = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        #endregion

        #region EDIT 
        public CLGFixingDTO edittab1(CLGFixingDTO data)
        {

            try
            {
                data.fix_day_period_edit = _TTContext.CLGTT_Fixing_Day_PeriodDMO.Where(x => x.MI_Id == data.MI_Id && x.TTFDPC_Id == data.TTFDPC_Id).ToArray();


                data.courselist = (from a in _TTContext.CLGTT_Category_CourseBranchDMO
                                   from b in _TTContext.MasterCourseDMO
                                   where b.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == data.TTMC_Id && a.TTCC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                   select b
                                ).Distinct().ToArray();

                data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _TTContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                     from b in _TTContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _TTContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select a).Distinct().ToArray();

                data.stafflist = (from a in _TTContext.HR_Master_Employee_DMO
                                  from b in _TTContext.TT_Master_Staff_AbbreviationDMO
                                  from c in _TTContext.TT_Final_Period_DistributionDMO
                                  from d in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                  where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && c.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id && d.ACMS_Id == data.ACMS_Id && d.TTMC_Id == data.TTMC_Id && c.TTFPD_Id == d.TTFPD_Id && c.TTFPD_ActiveFlag == true && b.TTMSAB_ActiveFlag == true)
                                  select new CLGPRDDistributionDTO
                                  {
                                      empName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                      HRME_Id = b.HRME_Id,
                                      TTMSAB_Abbreviation = b.TTMSAB_Abbreviation
                                  }).Distinct().OrderBy(j => j.empName).ToArray();

                data.subjectlist = (from a in _TTContext.IVRM_School_Master_SubjectsDMO
                                    from b in _TTContext.TT_Master_Subject_AbbreviationDMO
                                    from c in _TTContext.TT_Final_Period_DistributionDMO
                                    from d in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                    where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && c.HRME_Id == data.HRME_Id && c.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id && d.ACMS_Id == data.ACMS_Id && d.TTMC_Id == data.TTMC_Id && c.TTFPD_Id == d.TTFPD_Id && c.TTFPD_ActiveFlag == true && b.TTMSUAB_ActiveFlag == true && a.ISMS_Id == b.ISMS_Id && a.ISMS_Id == d.ISMS_Id)
                                    select new CLGPRDDistributionDTO
                                    {
                                        ISMS_Id = a.ISMS_Id,
                                        TTMSUAB_Abbreviation = b.TTMSUAB_Abbreviation,
                                        ISMS_SubjectName = a.ISMS_SubjectName
                                    }).Distinct().OrderBy(j => j.ISMS_SubjectName).ToArray();

                data.daydropdown = (from a in _TTContext.CLGTT_Master_Day_CourseBranchDMO
                                    from b in _TTContext.TT_Master_DayDMO
                                    where b.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.TTMD_Id == b.TTMD_Id && a.TTMDC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                    select b
                         ).Distinct().ToArray();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVE 
        public CLGFixingDTO deactivatetab1(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFDPC_Id > 0)
                {
                    var result = _TTContext.CLGTT_Fixing_Day_PeriodDMO.Single(t => t.TTFDPC_Id == data.TTFDPC_Id);

                    if (result.TTFDPC_ActiveFlag == true)
                    {
                        result.TTFDPC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTFDPC_ActiveFlag = true;
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag > 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

      
        //TAB1 END FIXING DAY PERIOD

        //TAB2 START FIXING DAY STAFF



        #region SAVE TAB2 DAY STAFF
        public CLGFixingDTO savetab2(CLGFixingDTO data)
        {
            try
            {
                var restrict_count = _TTContext.TT_Restricting_Day_StaffDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.HRME_Id == data.HRME_Id && r.TTMD_Id == data.TTMD_Id).Count();
                if (restrict_count == 0)
                {

                    if (data.TTFDS_SUbSelcFlag == false)
                    {
                        if (data.TTFDS_Id > 0)
                        {
                            var result0 = _TTContext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id && t.TTFDS_Id != data.TTFDS_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Day_StaffDMO.Single(t => t.TTFDS_Id == data.TTFDS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFDS_AllotedFlag = "No";
                                result.TTFDS_ActiveFlag = true;
                                result.TTFDS_SUbSelcFlag = false;
                                _TTContext.Update(result);
                                //REMOVE THE MAPPED COURSE DETAILS
                                var corresult = _TTContext.CLGTT_Fixing_Day_StaffDMO.Where(t => t.TTFDS_Id == data.TTFDS_Id).ToList();
                                if (corresult.Count > 0)
                                {
                                    foreach (var item in corresult)
                                    {
                                        _TTContext.Remove(item);
                                    }
                                }

                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                        else
                        {
                            var result0 = _TTContext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Day_StaffDMO objs = new TT_Fixing_Day_StaffDMO();
                                objs.ASMAY_Id = data.ASMAY_Id;
                                objs.MI_Id = data.MI_Id;
                                objs.HRME_Id = data.HRME_Id;
                                objs.TTMD_Id = data.TTMD_Id;
                                objs.TTFDS_AllotedFlag = "No";
                                objs.TTFDS_ActiveFlag = true;
                                objs.TTFDS_SUbSelcFlag = false;
                                objs.CreatedDate = DateTime.Now;
                                objs.UpdatedDate = DateTime.Now;
                                _TTContext.Add(objs);

                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }
                        }


                    }
                    else
                    {
                        if (data.TTFDS_Id > 0)
                        {
                            var result0 = _TTContext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id && t.TTFDS_Id != data.TTFDS_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Day_StaffDMO.Single(t => t.TTFDS_Id == data.TTFDS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFDS_AllotedFlag = "No";
                                result.TTFDS_ActiveFlag = true;
                                result.TTFDS_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                //REMOVE THE MAPPED COURSE DETAILS
                                var corresult = _TTContext.CLGTT_Fixing_Day_StaffDMO.Where(t => t.TTFDS_Id == data.TTFDS_Id).ToList();
                                if (corresult.Count > 0)
                                {
                                    foreach (var item1 in corresult)
                                    {
                                        _TTContext.Remove(item1);
                                    }
                                }
                                foreach (var item in data.TempararyArrayList)
                                {
                                    CLGTT_Fixing_Day_StaffDMO res = new CLGTT_Fixing_Day_StaffDMO();
                                    res.TTFDS_Id = data.TTFDS_Id;
                                    res.AMCO_Id = item.AMCO_Id;
                                    res.AMB_Id = item.AMB_Id;
                                    res.AMSE_Id = item.AMSE_Id;
                                    res.ACMS_Id = item.ACMS_Id;
                                    res.ISMS_Id = item.ISMS_Id;
                                    res.TTFPSCB_ActiveFlag = true;
                                    res.TTFPSCB_Periods = item.NOP;
                                    res.CreatedDate = DateTime.Now;
                                    res.UpdatedDate = DateTime.Now;
                                    _TTContext.Add(res);
                                }
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                        else
                        {
                            var result0 = _TTContext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Day_StaffDMO objs = new TT_Fixing_Day_StaffDMO();
                                objs.ASMAY_Id = data.ASMAY_Id;
                                objs.MI_Id = data.MI_Id;
                                objs.HRME_Id = data.HRME_Id;
                                objs.TTMD_Id = data.TTMD_Id;
                                objs.TTFDS_AllotedFlag = "No";
                                objs.TTFDS_ActiveFlag = true;
                                objs.TTFDS_SUbSelcFlag = true;
                                objs.CreatedDate = DateTime.Now;
                                objs.UpdatedDate = DateTime.Now;
                                _TTContext.Add(objs);

                                foreach (var item in data.TempararyArrayList)
                                {
                                    CLGTT_Fixing_Day_StaffDMO res = new CLGTT_Fixing_Day_StaffDMO();
                                    res.TTFDS_Id = objs.TTFDS_Id;
                                    res.AMCO_Id = item.AMCO_Id;
                                    res.AMB_Id = item.AMB_Id;
                                    res.AMSE_Id = item.AMSE_Id;
                                    res.ACMS_Id = item.ACMS_Id;
                                    res.ISMS_Id = item.ISMS_Id;
                                    res.TTFPSCB_ActiveFlag = true;
                                    res.TTFPSCB_Periods = item.NOP;
                                    res.CreatedDate = DateTime.Now;
                                    res.UpdatedDate = DateTime.Now;
                                    _TTContext.Add(res);
                                }

                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }



                            }
                        }
                    }

                    ///RELOAD THE DATA
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_LOAD_FIXING_DAY_PERIOD_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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
                            data.fix_day_period_list = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else
                {
                    data.returnrestrictstatus = "Restricted";
                    return data;

                }


                data.all_fix_day_staff_list = (from a in _TTContext.AcademicYear
                                               from e in _TTContext.HR_Master_Employee_DMO
                                               from g in _TTContext.TT_Fixing_Day_StaffDMO
                                               from i in _TTContext.TT_Master_DayDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                               select new CLGFixingDTO
                                               {
                                                   TTFDS_Id = g.TTFDS_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTMD_Id = g.TTMD_Id,
                                                   TTMD_DayName = i.TTMD_DayName,
                                                   TTFDS_SUbSelcFlag = g.TTFDS_SUbSelcFlag,
                                                   TTFDS_ActiveFlag = g.TTFDS_ActiveFlag,
                                               }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        #endregion


        #region VIEW TAB2 DAY STAFF
        public CLGFixingDTO viewtab2grid(CLGFixingDTO data)
        {
            try
            {


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_FIXIBG_DAY_STAFF_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTFDS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTFDS_Id
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
                        data.detailspopuparray2 = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion
        
        #region EDIT TAB2 DAY STAFF
        public CLGFixingDTO gettab2editdata(CLGFixingDTO data)
        {
            try
            {



                if (data.TTFDS_SUbSelcFlag == false)
                {
                    data.fix_day_staff_edit = _TTContext.TT_Fixing_Day_StaffDMO.Where(a => a.TTFDS_Id == data.TTFDS_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_FIXIBG_DAY_STAFF_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTFDS_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTFDS_Id
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
                            data.fix_day_staff_edit = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                    }
                }

               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion
        #region ACTIVE/DEACTIVE 
        public CLGFixingDTO deactivatetab2(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFDS_Id > 0)
                {
                    var result = _TTContext.TT_Fixing_Day_StaffDMO.Single(t => t.TTFDS_Id == data.TTFDS_Id);

                    if (result.TTFDS_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Fixing_Day_StaffDMO.Where(t => t.TTFDS_Id == data.TTFDS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFPSCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTFDS_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Fixing_Day_StaffDMO.Where(t => t.TTFDS_Id == data.TTFDS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFPSCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTFDS_ActiveFlag = true;
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag > 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        //TAB2 END FIXING DAY STAFF

        //TAB3 FIXING DAY SUBJECT

        #region SAVE TAB3 DAY SUBJECT
        public CLGFixingDTO savetab3(CLGFixingDTO data)
        {
            try
            {
                var restrict_count = _TTContext.TT_Restricting_Day_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.ISMS_Id == data.ISMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTRDSU_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (data.TTFDSU_SUbSelcFlag == false)
                    {
                        if (data.TTFDSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(data.ISMS_Id) && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMD_Id==data.TTMD_Id && t.TTFDSU_Id != data.TTFDSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Day_SubjectDMO.Single(t => t.TTFDSU_Id==data.TTFDSU_Id && t.MI_Id==data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFDSU_AllotedFlag = "No";
                                result.TTFDSU_ActiveFlag = true;
                                result.TTFDSU_SUbSelcFlag = false;
                                _TTContext.Update(result);

                                var corres = _TTContext.CLGTT_Fixing_Day_SubjectDMO.Where(e => e.TTFDSU_Id == data.TTFDSU_Id).ToList();

                                if (corres.Count>0)
                                {
                                    foreach (var item in corres)
                                    {
                                        _TTContext.Remove(item);
                                    }

                                }

                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                        else
                        {
                            var result = _TTContext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id==data.ISMS_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMD_Id==data.TTMD_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Day_SubjectDMO obj = new TT_Fixing_Day_SubjectDMO();
                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = data.ASMAY_Id;
                                obj.ISMS_Id = data.ISMS_Id;
                                obj.TTMD_Id = data.TTMD_Id;                               
                                obj.TTFDSU_AllotedFlag = "No";
                                obj.TTFDSU_ActiveFlag = true;
                                obj.TTFDSU_SUbSelcFlag = false;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _TTContext.Add(obj);
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }

                    }
                    else if (data.TTFDSU_SUbSelcFlag == true)
                    {
                        if (data.TTFDSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id==data.ISMS_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMD_Id==data.TTMD_Id && t.TTFDSU_Id != data.TTFDSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Day_SubjectDMO.Single(t => t.TTFDSU_Id==data.TTFDSU_Id && t.MI_Id==data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFDSU_AllotedFlag = "No";
                                result.TTFDSU_ActiveFlag = true;
                                result.TTFDSU_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                var corres = _TTContext.CLGTT_Fixing_Day_SubjectDMO.Where(e => e.TTFDSU_Id == data.TTFDSU_Id).ToList();

                                if (corres.Count > 0)
                                {
                                    foreach (var item in corres)
                                    {
                                        _TTContext.Remove(item);
                                    }

                                }
                                if (data.TempararyArrayList.Length>0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Fixing_Day_SubjectDMO ress = new CLGTT_Fixing_Day_SubjectDMO();

                                        ress.TTFDSU_Id = data.TTFDSU_Id;
                                        ress.AMCO_Id = item.AMCO_Id;
                                        ress.AMB_Id = item.AMB_Id;
                                        ress.AMSE_Id = item.AMSE_Id;
                                        ress.ACMS_Id = item.ACMS_Id;
                                        ress.HRME_Id = item.HRME_Id;
                                        ress.TTFDSUCB_Periods = item.NOP;
                                        ress.TTFDSUCB_ActiveFlag = true;
                                        ress.CreatedDate = DateTime.Now;
                                        ress.UpdatedDate = DateTime.Now;
                                        _TTContext.Add(ress);

                                    }

                                }


                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                        else
                        {
                            var result = _TTContext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id==data.ISMS_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMD_Id==data.TTMD_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Day_SubjectDMO resdata = new TT_Fixing_Day_SubjectDMO();
                                resdata.MI_Id = data.MI_Id;
                                resdata.ASMAY_Id = data.ASMAY_Id;
                                resdata.ISMS_Id = data.ISMS_Id;
                                resdata.TTMD_Id = data.TTMD_Id;
                                resdata.UpdatedDate = DateTime.Now;
                                resdata.UpdatedDate = DateTime.Now;
                                resdata.TTFDSU_AllotedFlag = "No";
                                resdata.TTFDSU_ActiveFlag = true;
                                resdata.TTFDSU_SUbSelcFlag = true;
                                _TTContext.Add(resdata);
                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Fixing_Day_SubjectDMO ress = new CLGTT_Fixing_Day_SubjectDMO();

                                        ress.TTFDSU_Id = resdata.TTFDSU_Id;
                                        ress.AMCO_Id = item.AMCO_Id;
                                        ress.AMB_Id = item.AMB_Id;
                                        ress.AMSE_Id = item.AMSE_Id;
                                        ress.ACMS_Id = item.ACMS_Id;
                                        ress.HRME_Id = item.HRME_Id;
                                        ress.TTFDSUCB_Periods = item.NOP;
                                        ress.TTFDSUCB_ActiveFlag = true;
                                        ress.CreatedDate = DateTime.Now;
                                        ress.UpdatedDate = DateTime.Now;
                                        _TTContext.Add(ress);

                                    }

                                }
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists >0 )
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    data.returnrestrictstatus = "Restricted";
                    return data;
                }
                data.all_fix_day_subject_list = (from a in _TTContext.AcademicYear
                                                      from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                      from g in _TTContext.TT_Fixing_Day_SubjectDMO
                                                      from i in _TTContext.TT_Master_DayDMO
                                                      where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                      select new CLGFixingDTO
                                                      {
                                                          TTFDSU_Id = g.TTFDSU_Id,
                                                          ASMAY_Year = a.ASMAY_Year,
                                                          ISMS_SubjectName = e.ISMS_SubjectName,
                                                          TTMD_Id = g.TTMD_Id,
                                                          TTMD_DayName = i.TTMD_DayName,
                                                          TTFDSU_SUbSelcFlag = g.TTFDSU_SUbSelcFlag,
                                                          TTFDSU_ActiveFlag = g.TTFDSU_ActiveFlag,
                                                      }
      ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region VIEW TAB3 DAY SUBJECT
        public CLGFixingDTO viewtab3grid(CLGFixingDTO data)
        {
            try
            {
                
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_FIXIBG_DAY_SUBJECT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTFDSU_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTFDSU_Id
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
                        data.detailspopuparray3 = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion


        #region EDIT TAB3 DAY SUBJECT
        public CLGFixingDTO edittab3(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFDSU_SUbSelcFlag == false)
                {
                    data.fix_day_subject_edit = _TTContext.TT_Fixing_Day_SubjectDMO.Where(a => a.TTFDSU_Id == data.TTFDSU_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_FIXIBG_DAY_SUBJECT_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTFDSU_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTFDSU_Id
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
                            data.fix_day_subject_edit = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        
        #region ACTIVE/DEACTIVE 
        public CLGFixingDTO deactivatetab3(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFDSU_Id > 0)
                {
                    var result = _TTContext.TT_Fixing_Day_SubjectDMO.Single(t => t.TTFDSU_Id == data.TTFDSU_Id);

                    if (result.TTFDSU_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Fixing_Day_SubjectDMO.Where(t => t.TTFDSU_Id == data.TTFDSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFDSUCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTFDSU_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Fixing_Day_SubjectDMO.Where(t => t.TTFDSU_Id == data.TTFDSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFDSUCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTFDSU_ActiveFlag = true;
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag > 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion
        //TAB3 END FIXING DAY SUBJECT


        //TAB4 START FIXING PERIOD STAFF

        #region SAVE TAB 4 FIXING PERIOD STAFF 
        public CLGFixingDTO savetab4(CLGFixingDTO data)
        {
            try
            {
                var restrict_count = _TTContext.TT_Restricting_Period_StaffDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.HRME_Id == data.HRME_Id && r.TTMP_Id == data.TTMP_Id && r.TTRPS_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (data.TTFPS_SUbSelcFlag == false)
                    {
                        if (data.TTFPS_Id > 0)
                        {

                            var result0 = _TTContext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id==data.HRME_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMP_Id==data.TTMP_Id && t.TTFPS_Id != data.TTFPS_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Period_StaffDMO.Single(t => t.TTFPS_Id==data.TTFPS_Id && t.MI_Id==data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPS_AllotedFlag = "No";
                                result.TTFPS_ActiveFlag = true;
                                result.TTFPS_SUbSelcFlag = false;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Fixing_Period_StaffDMO.Where(t => t.TTFPS_Id == data.TTFPS_Id).ToList();
                                if (rescor.Count>0)
                                {
                                    foreach (var item in rescor)
                                    {
                                        _TTContext.Remove(item);
                                    }
                                }
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                        else
                        {
                            var result = _TTContext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id==data.HRME_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMP_Id==data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Period_StaffDMO ress = new TT_Fixing_Period_StaffDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.HRME_Id = data.HRME_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.CreatedDate = DateTime.Now;
                                ress.UpdatedDate = DateTime.Now;
                                ress.TTFPS_AllotedFlag = "No";
                                ress.TTFPS_ActiveFlag = true;
                                ress.TTFPS_SUbSelcFlag = false;
                                _TTContext.Add(ress);
                                var contactExists = _TTContext.SaveChanges();
                              
                                if (contactExists >0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }

                    }
                    else if (data.TTFPS_SUbSelcFlag == true)
                    {
                        if (data.TTFPS_Id > 0)
                        {

                            var result0 = _TTContext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id.Equals(data.HRME_Id) && t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.TTMP_Id.Equals(data.TTMP_Id) && t.TTFPS_Id != data.TTFPS_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Period_StaffDMO.Single(t => t.TTFPS_Id == data.TTFPS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPS_AllotedFlag = "No";
                                result.TTFPS_ActiveFlag = true;
                                result.TTFPS_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Fixing_Period_StaffDMO.Where(t => t.TTFPS_Id == data.TTFPS_Id).ToList();
                                if (rescor.Count > 0)
                                {
                                    foreach (var item in rescor)
                                    {
                                        _TTContext.Remove(item);
                                    }
                                }
                                if (data.TempararyArrayList.Length>0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Fixing_Period_StaffDMO newres = new CLGTT_Fixing_Period_StaffDMO();

                                        newres.TTFPS_Id = data.TTFPS_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.ISMS_Id = item.ISMS_Id;
                                        newres.TTFPSCB_Days = item.NOD;
                                        newres.TTFPSCB_ActiveFlag = true;
                                        newres.CreatedDate = DateTime.Now;
                                        newres.UpdatedDate = DateTime.Now;
                                        _TTContext.Add(newres);

                                    }
                                }

                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                               
                            }
                        }
                        else
                        {
                            var result = _TTContext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id==data.HRME_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTMP_Id==data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Period_StaffDMO ress = new TT_Fixing_Period_StaffDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.HRME_Id = data.HRME_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.UpdatedDate = DateTime.Now;
                                ress.CreatedDate = DateTime.Now;
                                ress.TTFPS_AllotedFlag = "No";
                                ress.TTFPS_ActiveFlag = true;
                                ress.TTFPS_SUbSelcFlag = true;
                                _TTContext.Add(ress);
                            

                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Fixing_Period_StaffDMO newres = new CLGTT_Fixing_Period_StaffDMO();

                                        newres.TTFPS_Id = ress.TTFPS_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.ISMS_Id = item.ISMS_Id;
                                        newres.TTFPSCB_Days = item.NOD;
                                        newres.TTFPSCB_ActiveFlag = true;
                                        newres.CreatedDate = DateTime.Now;
                                        newres.UpdatedDate = DateTime.Now;
                                        _TTContext.Add(newres);

                                    }
                                }
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists>0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    data.returnrestrictstatus = "Restricted";
                    return data;
                }
                data.all_fix_period_staff_list = (from a in _TTContext.AcademicYear
                                                  from e in _TTContext.HR_Master_Employee_DMO
                                                  from g in _TTContext.TT_Fixing_Period_StaffDMO
                                                  from b in _TTContext.TT_Master_PeriodDMO
                                                  where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                  select new CLGFixingDTO
                                                  {
                                                      TTFPS_Id = g.TTFPS_Id,
                                                      ASMAY_Year = a.ASMAY_Year,
                                                      staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                      TTMP_Id = g.TTMP_Id,
                                                      TTMP_PeriodName = b.TTMP_PeriodName,
                                                      TTFPS_SUbSelcFlag = g.TTFPS_SUbSelcFlag,
                                                      TTFPS_ActiveFlag = g.TTFPS_ActiveFlag,
                                                  }
      ).Distinct().OrderBy(x => x.staffName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion
        #region VIEW TAB 4 FIXING PERIOD STAFF 
        public CLGFixingDTO viewtab4(CLGFixingDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_FIXING_PERIOD_STAFF_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTFPS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTFPS_Id
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
                        data.detailspopuparray4 = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        #region EDIT TAB4 PERIOD STAFF
        public CLGFixingDTO edittab4(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFPS_SUbSelcFlag == false)
                {
                    data.fix_period_staff_edit = _TTContext.TT_Fixing_Period_StaffDMO.Where(a => a.TTFPS_Id == data.TTFPS_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_FIXIBG_PERIOD_STAFF_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTFPS_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTFPS_Id
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
                            data.fix_period_staff_edit = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion
        #region ACTIVE/DEACTIVE 
        public CLGFixingDTO deactivatetab4(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFPS_Id > 0)
                {
                    var result = _TTContext.TT_Fixing_Period_StaffDMO.Single(t => t.TTFPS_Id == data.TTFPS_Id);

                    if (result.TTFPS_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Fixing_Period_StaffDMO.Where(t => t.TTFPS_Id == data.TTFPS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFPSCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTFPS_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Fixing_Period_StaffDMO.Where(t => t.TTFPS_Id == data.TTFPS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFPSCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTFPS_ActiveFlag = true;
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag > 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        //TAB4 END FIXING PERIOD STAFF
        //TAB5 START FIXING PERIOD SUBJECT

        #region SAVE TAB 4 FIXING PERIOD STAFF 
        public CLGFixingDTO savetab5(CLGFixingDTO data)
        {
            try
            {
                var restrict_count = _TTContext.TT_Restricting_Period_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.ISMS_Id == data.ISMS_Id && r.TTMP_Id == data.TTMP_Id && r.TTRPSU_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (data.TTFPSU_SUbSelcFlag == false)
                    {
                        if (data.TTFPSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id && t.TTFPSU_Id != data.TTFPSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Period_SubjectDMO.Single(t => t.TTFPSU_Id == data.TTFPSU_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPSU_AllotedFlag = "No";
                                result.TTFPSU_ActiveFlag = true;
                                result.TTFPSU_SUbSelcFlag = false;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Fixing_Period_SubjectDMO.Where(t => t.TTFPSU_Id == data.TTFPSU_Id).ToList();
                                if (rescor.Count > 0)
                                {
                                    foreach (var item in rescor)
                                    {
                                        _TTContext.Remove(item);
                                    }
                                }
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                        else
                        {
                            var result = _TTContext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Period_SubjectDMO ress = new TT_Fixing_Period_SubjectDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.ISMS_Id = data.ISMS_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.CreatedDate = DateTime.Now;
                                ress.UpdatedDate = DateTime.Now;
                                ress.TTFPSU_AllotedFlag = "No";
                                ress.TTFPSU_ActiveFlag = true;
                                ress.TTFPSU_SUbSelcFlag = false;
                                _TTContext.Add(ress);
                                var contactExists = _TTContext.SaveChanges();

                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }

                    }
                    else if (data.TTFPSU_SUbSelcFlag == true)
                    {
                        if (data.TTFPSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(data.ISMS_Id) && t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.TTMP_Id.Equals(data.TTMP_Id) && t.TTFPSU_Id != data.TTFPSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Fixing_Period_SubjectDMO.Single(t => t.TTFPSU_Id == data.TTFPSU_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPSU_AllotedFlag = "No";
                                result.TTFPSU_ActiveFlag = true;
                                result.TTFPSU_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Fixing_Period_SubjectDMO.Where(t => t.TTFPSU_Id == data.TTFPSU_Id).ToList();
                                if (rescor.Count > 0)
                                {
                                    foreach (var item in rescor)
                                    {
                                        _TTContext.Remove(item);
                                    }
                                }
                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Fixing_Period_SubjectDMO newres = new CLGTT_Fixing_Period_SubjectDMO();

                                        newres.TTFPSU_Id = data.TTFPSU_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.HRME_Id = item.HRME_Id;
                                        newres.TTFPSUCB_Days = item.NOD;
                                        newres.TTFPSUCB_ActiveFlag = true;
                                        newres.CreatedDate = DateTime.Now;
                                        newres.UpdatedDate = DateTime.Now;
                                        _TTContext.Add(newres);

                                    }
                                }

                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }
                        }
                        else
                        {
                            var result = _TTContext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Fixing_Period_SubjectDMO ress = new TT_Fixing_Period_SubjectDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.ISMS_Id = data.ISMS_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.UpdatedDate = DateTime.Now;
                                ress.CreatedDate = DateTime.Now;
                                ress.TTFPSU_AllotedFlag = "No";
                                ress.TTFPSU_ActiveFlag = true;
                                ress.TTFPSU_SUbSelcFlag = true;
                                _TTContext.Add(ress);


                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Fixing_Period_SubjectDMO newres = new CLGTT_Fixing_Period_SubjectDMO();

                                        newres.TTFPSU_Id = ress.TTFPSU_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.HRME_Id = item.HRME_Id;
                                        newres.TTFPSUCB_Days = item.NOD;
                                        newres.TTFPSUCB_ActiveFlag = true;
                                        newres.CreatedDate = DateTime.Now;
                                        newres.UpdatedDate = DateTime.Now;
                                        _TTContext.Add(newres);

                                    }
                                }
                                var contactExists = _TTContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    data.returnrestrictstatus = "Restricted";
                    return data;
                }
                data.all_fix_period_subject_list = (from a in _TTContext.AcademicYear
                                                         from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                         from g in _TTContext.TT_Fixing_Period_SubjectDMO
                                                         from b in _TTContext.TT_Master_PeriodDMO
                                                         where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                         select new CLGFixingDTO
                                                         {
                                                             TTFPSU_Id = g.TTFPSU_Id,
                                                             ASMAY_Year = a.ASMAY_Year,
                                                             ISMS_SubjectName = e.ISMS_SubjectName,
                                                             TTMP_Id = g.TTMP_Id,
                                                             TTMP_PeriodName = b.TTMP_PeriodName,
                                                             TTFPSU_SUbSelcFlag = g.TTFPSU_SUbSelcFlag,
                                                             TTFPSU_ActiveFlag = g.TTFPSU_ActiveFlag,
                                                         }
         ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        #region VIEW TAB 5 FIXING PERIOD SUBJECT 
        public CLGFixingDTO viewtab5(CLGFixingDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_FIXING_PERIOD_SUBJECT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTFPSU_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTFPSU_Id
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
                        data.detailspopuparray5 = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion


        #region EDIT TAB5 PERID SUBJECT
        public CLGFixingDTO edittab5(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFPSU_SUbSelcFlag == false)
                {
                    data.fix_period_subject_edit = _TTContext.TT_Fixing_Period_SubjectDMO.Where(a => a.TTFPSU_Id == data.TTFPSU_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_FIXING_PERIOD_SUBJECT_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTFPSU_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTFPSU_Id
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
                            data.fix_period_subject_edit = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion
        //TAB5 END FIXING PERIOD SUBJECT

        #region ACTIVE/DEACTIVE 
        public CLGFixingDTO deactivatetab5(CLGFixingDTO data)
        {
            try
            {

                if (data.TTFPSU_Id > 0)
                {
                    var result = _TTContext.TT_Fixing_Period_SubjectDMO.Single(t => t.TTFPSU_Id == data.TTFPSU_Id);

                    if (result.TTFPSU_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Fixing_Period_SubjectDMO.Where(t => t.TTFPSU_Id == data.TTFPSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFPSUCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTFPSU_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Fixing_Period_SubjectDMO.Where(t => t.TTFPSU_Id == data.TTFPSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTFPSUCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTFPSU_ActiveFlag = true;
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag > 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

    }
}
