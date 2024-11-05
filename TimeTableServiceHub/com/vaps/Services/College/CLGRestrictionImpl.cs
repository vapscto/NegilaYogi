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
    public class CLGRestrictionImpl : Interfaces.CLGRestrictionInterface
    {
        private static ConcurrentDictionary<string, CLGRestrictionDTO> _login =
             new ConcurrentDictionary<string, CLGRestrictionDTO>();

        public TTContext _TTContext;
        ILogger<CLGRestrictionImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGRestrictionImpl(TTContext academiccontext, ILogger<CLGRestrictionImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }

        //TAB1 START FIXING DAY PERIOD
        #region LOAD ALL DATA
        public CLGRestrictionDTO getalldetails(CLGRestrictionDTO data)
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




                ///RESTRICTION DAY PERIODS GRID LOAD
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_LOAD_RESTRICTION_DAY_PERIOD_DETAILS";
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
                        data.restrict_day_period_list = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }

                //END

                //           /// FIXED DAY STAFF GRID LOAD

                data.all_restrict_day_staff_list = (from a in _TTContext.AcademicYear
                                                    from e in _TTContext.HR_Master_Employee_DMO
                                                    from g in _TTContext.TT_Restricting_Day_StaffDMO
                                                    from i in _TTContext.TT_Master_DayDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                    select new CLGRestrictionDTO
                                                    {
                                                        TTRDS_Id = g.TTRDS_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                        TTMD_Id = g.TTMD_Id,
                                                        TTMD_DayName = i.TTMD_DayName,
                                                        TTRDS_SUbSelcFlag = g.TTRDS_SUbSelcFlag,
                                                        TTRDS_ActiveFlag = g.TTRDS_ActiveFlag,
                                                    }
 ).Distinct().OrderBy(x => x.staffName).ToArray();


                //           /// END

                //           ///FIXING DAY SUBJECT       
                data.all_restrict_day_subject_list = (from a in _TTContext.AcademicYear
                                                 from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                 from g in _TTContext.TT_Restricting_Day_SubjectDMO
                                                 from i in _TTContext.TT_Master_DayDMO
                                                 where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                 select new CLGRestrictionDTO
                                                 {
                                                     TTRDSU_Id = g.TTRDSU_Id,
                                                     ASMAY_Year = a.ASMAY_Year,
                                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                                     TTMD_Id = g.TTMD_Id,
                                                     TTMD_DayName = i.TTMD_DayName,
                                                     TTRDSU_SUbSelcFlag = g.TTRDSU_SUbSelcFlag,
                                                     TTRDSU_ActiveFlag = g.TTRDSU_ActiveFlag,
                                                 }
        ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
                //           /// END

                //           ///FIXING PERIOD STAFF
                data.all_restrict_period_staff_list = (from a in _TTContext.AcademicYear
                                                  from e in _TTContext.HR_Master_Employee_DMO
                                                  from g in _TTContext.TT_Restricting_Period_StaffDMO
                                                  from b in _TTContext.TT_Master_PeriodDMO
                                                  where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                  select new CLGRestrictionDTO
                                                  {
                                                      TTRPS_Id = g.TTRPS_Id,
                                                      ASMAY_Year = a.ASMAY_Year,
                                                      staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                      TTMP_Id = g.TTMP_Id,
                                                      TTMP_PeriodName = b.TTMP_PeriodName,
                                                      TTRPS_SUbSelcFlag = g.TTRPS_SUbSelcFlag,
                                                      TTRPS_ActiveFlag = g.TTRPS_ActiveFlag,
                                                  }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
                //           ///END

                //           /// ///FIXING PERIOD SUBJECT

                data.all_restrict_period_subject_list = (from a in _TTContext.AcademicYear
                                                    from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                    from g in _TTContext.TT_Restricting_Period_SubjectDMO
                                                    from b in _TTContext.TT_Master_PeriodDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                    select new CLGRestrictionDTO
                                                    {
                                                        TTRPSU_Id = g.TTRPSU_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        ISMS_SubjectName = e.ISMS_SubjectName,
                                                        TTMP_Id = g.TTMP_Id,
                                                        TTMP_PeriodName = b.TTMP_PeriodName,
                                                        TTRPSU_SUbSelcFlag = g.TTRPSU_SUbSelcFlag,
                                                        TTRPSU_ActiveFlag = g.TTRPSU_ActiveFlag,
                                                    }
       ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
                //           ///END


            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE TAB1 DAY PERIODS
        public CLGRestrictionDTO savetab1(CLGRestrictionDTO data)
        {
            try
            {
                var fix_count = _TTContext.CLGTT_Fixing_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.TTMC_Id == data.TTMC_Id && r.AMCO_Id == data.AMCO_Id && r.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && r.ACMS_Id == data.ACMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTMP_Id == data.TTMP_Id && r.HRME_Id == data.HRME_Id && r.ISMS_Id == data.ISMS_Id && r.TTFDPC_ActiveFlag==true).Count();
                if (fix_count == 0)
                {
                    if (data.TTRDPC_Id > 0)
                    {
                        var resultCount = _TTContext.CLGTT_Restricting_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.TTMC_Id == data.TTMC_Id && r.AMCO_Id == data.AMCO_Id && r.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && r.ACMS_Id == data.ACMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTMP_Id == data.TTMP_Id && r.HRME_Id == data.HRME_Id && r.ISMS_Id == data.ISMS_Id && r.TTRDPC_Id != data.TTRDPC_Id).Count();
                        if (resultCount == 0)
                        {
                            var res = _TTContext.CLGTT_Restricting_Day_PeriodDMO.Single(f => f.TTRDPC_Id == data.TTRDPC_Id);
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
                            res.TTRDPC_AllotedFlag = "No";
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
                        var resultCount = _TTContext.CLGTT_Restricting_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.TTMC_Id == data.TTMC_Id && r.AMCO_Id == data.AMCO_Id && r.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && r.ACMS_Id == data.ACMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTMP_Id == data.TTMP_Id && r.HRME_Id == data.HRME_Id && r.ISMS_Id == data.ISMS_Id).Count();
                        if (resultCount == 0)
                        {
                            CLGTT_Restricting_Day_PeriodDMO obj = new CLGTT_Restricting_Day_PeriodDMO();
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
                            obj.TTRDPC_AllotedFlag = "No";
                            obj.TTRDPC_ActiveFlag = true;
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
                    data.returnfixstatus = "Fixed";
                    return data;
                }

                ///RELOAD THE DATA
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_LOAD_RESTRICTION_DAY_PERIOD_DETAILS";
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
                        data.restrict_day_period_list = retObject.ToArray();
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
        public CLGRestrictionDTO edittab1(CLGRestrictionDTO data)
        {

            try
            {
                data.restrict_day_period_edit = _TTContext.CLGTT_Restricting_Day_PeriodDMO.Where(x => x.MI_Id == data.MI_Id && x.TTRDPC_Id == data.TTRDPC_Id).ToArray();


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
        public CLGRestrictionDTO deactivatetab1(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRDPC_Id > 0)
                {
                    var result = _TTContext.CLGTT_Restricting_Day_PeriodDMO.Single(t => t.TTRDPC_Id == data.TTRDPC_Id);

                    if (result.TTRDPC_ActiveFlag == true)
                    {
                        result.TTRDPC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTRDPC_ActiveFlag = true;
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
        public CLGRestrictionDTO savetab2(CLGRestrictionDTO data)
        {
            try
            {
                var fix_count = _TTContext.TT_Fixing_Day_StaffDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.HRME_Id == data.HRME_Id && r.TTMD_Id == data.TTMD_Id && r.TTFDS_ActiveFlag==true).Count();
                if (fix_count == 0)
                {

                    if (data.TTRDS_SUbSelcFlag == false)
                    {
                        if (data.TTRDS_Id > 0)
                        {
                            var result0 = _TTContext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id && t.TTRDS_Id != data.TTRDS_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Day_StaffDMO.Single(t => t.TTRDS_Id == data.TTRDS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRDS_AllotedFlag = "No";
                                result.TTRDS_ActiveFlag = true;
                                result.TTRDS_SUbSelcFlag = false;
                                _TTContext.Update(result);
                                //REMOVE THE MAPPED COURSE DETAILS
                                var corresult = _TTContext.CLGTT_Restricting_Day_StaffDMO.Where(t => t.TTRDS_Id == data.TTRDS_Id).ToList();
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
                            var result0 = _TTContext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Day_StaffDMO objs = new TT_Restricting_Day_StaffDMO();
                                objs.ASMAY_Id = data.ASMAY_Id;
                                objs.MI_Id = data.MI_Id;
                                objs.HRME_Id = data.HRME_Id;
                                objs.TTMD_Id = data.TTMD_Id;
                                objs.TTRDS_AllotedFlag = "No";
                                objs.TTRDS_ActiveFlag = true;
                                objs.TTRDS_SUbSelcFlag = false;
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
                        if (data.TTRDS_Id > 0)
                        {
                            var result0 = _TTContext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id && t.TTRDS_Id != data.TTRDS_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Day_StaffDMO.Single(t => t.TTRDS_Id == data.TTRDS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRDS_AllotedFlag = "No";
                                result.TTRDS_ActiveFlag = true;
                                result.TTRDS_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                //REMOVE THE MAPPED COURSE DETAILS
                                var corresult = _TTContext.CLGTT_Restricting_Day_StaffDMO.Where(t => t.TTRDS_Id == data.TTRDS_Id).ToList();
                                if (corresult.Count > 0)
                                {
                                    foreach (var item1 in corresult)
                                    {
                                        _TTContext.Remove(item1);
                                    }
                                }
                                foreach (var item in data.TempararyArrayList)
                                {
                                    CLGTT_Restricting_Day_StaffDMO res = new CLGTT_Restricting_Day_StaffDMO();
                                    res.TTRDS_Id = data.TTRDS_Id;
                                    res.AMCO_Id = item.AMCO_Id;
                                    res.AMB_Id = item.AMB_Id;
                                    res.AMSE_Id = item.AMSE_Id;
                                    res.ACMS_Id = item.ACMS_Id;
                                    res.ISMS_Id = item.ISMS_Id;
                                    res.TTRPSCB_ActiveFlag = true;
                                    res.TTRPSCB_Periods = item.NOP;
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
                            var result0 = _TTContext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id).ToList();
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Day_StaffDMO objs = new TT_Restricting_Day_StaffDMO();
                                objs.ASMAY_Id = data.ASMAY_Id;
                                objs.MI_Id = data.MI_Id;
                                objs.HRME_Id = data.HRME_Id;
                                objs.TTMD_Id = data.TTMD_Id;
                                objs.TTRDS_AllotedFlag = "No";
                                objs.TTRDS_ActiveFlag = true;
                                objs.TTRDS_SUbSelcFlag = true;
                                objs.CreatedDate = DateTime.Now;
                                objs.UpdatedDate = DateTime.Now;
                                _TTContext.Add(objs);

                                foreach (var item in data.TempararyArrayList)
                                {
                                    CLGTT_Restricting_Day_StaffDMO res = new CLGTT_Restricting_Day_StaffDMO();
                                    res.TTRDS_Id = objs.TTRDS_Id;
                                    res.AMCO_Id = item.AMCO_Id;
                                    res.AMB_Id = item.AMB_Id;
                                    res.AMSE_Id = item.AMSE_Id;
                                    res.ACMS_Id = item.ACMS_Id;
                                    res.ISMS_Id = item.ISMS_Id;
                                    res.TTRPSCB_ActiveFlag = true;
                                    res.TTRPSCB_Periods = item.NOP;
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



                }
                else
                {
                    data.returnfixstatus = "Fixed";
                    return data;
                }
                data.all_restrict_day_staff_list = (from a in _TTContext.AcademicYear
                                               from e in _TTContext.HR_Master_Employee_DMO
                                               from g in _TTContext.TT_Restricting_Day_StaffDMO
                                               from i in _TTContext.TT_Master_DayDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                               select new CLGRestrictionDTO
                                               {
                                                   TTRDS_Id = g.TTRDS_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTMD_Id = g.TTMD_Id,
                                                   TTMD_DayName = i.TTMD_DayName,
                                                   TTRDS_SUbSelcFlag = g.TTRDS_SUbSelcFlag,
                                                   TTRDS_ActiveFlag = g.TTRDS_ActiveFlag,
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
        public CLGRestrictionDTO viewtab2grid(CLGRestrictionDTO data)
        {
            try
            {


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_RESTRICT_DAY_STAFF_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTRDS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTRDS_Id
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
        public CLGRestrictionDTO gettab2editdata(CLGRestrictionDTO data)
        {
            try
            {



                if (data.TTRDS_SUbSelcFlag == false)
                {
                    data.restrict_day_staff_edit = _TTContext.TT_Restricting_Day_StaffDMO.Where(a => a.TTRDS_Id == data.TTRDS_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_RESTRICT_DAY_STAFF_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTRDS_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTRDS_Id
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
                            data.restrict_day_staff_edit = retObject.ToArray();
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
        public CLGRestrictionDTO deactivatetab2(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRDS_Id > 0)
                {
                    var result = _TTContext.TT_Restricting_Day_StaffDMO.Single(t => t.TTRDS_Id == data.TTRDS_Id);

                    if (result.TTRDS_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Restricting_Day_StaffDMO.Where(t => t.TTRDS_Id == data.TTRDS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRPSCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTRDS_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Restricting_Day_StaffDMO.Where(t => t.TTRDS_Id == data.TTRDS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRPSCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTRDS_ActiveFlag = true;
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
        public CLGRestrictionDTO savetab3(CLGRestrictionDTO data)
        {
            try
            {
                var fix_count = _TTContext.TT_Fixing_Day_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.ISMS_Id == data.ISMS_Id && r.TTMD_Id == data.TTMD_Id && r.TTFDSU_ActiveFlag==true).Count();
                if (fix_count == 0)
                {
                    if (data.TTRDSU_SUbSelcFlag == false)
                    {
                        if (data.TTRDSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(data.ISMS_Id) && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id && t.TTRDSU_Id != data.TTRDSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Day_SubjectDMO.Single(t => t.TTRDSU_Id == data.TTRDSU_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRDSU_AllotedFlag = "No";
                                result.TTRDSU_ActiveFlag = true;
                                result.TTRDSU_SUbSelcFlag = false;
                                _TTContext.Update(result);

                                var corres = _TTContext.CLGTT_Restricting_Day_SubjectDMO.Where(e => e.TTRDSU_Id == data.TTRDSU_Id).ToList();

                                if (corres.Count > 0)
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
                            var result = _TTContext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Day_SubjectDMO obj = new TT_Restricting_Day_SubjectDMO();
                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = data.ASMAY_Id;
                                obj.ISMS_Id = data.ISMS_Id;
                                obj.TTMD_Id = data.TTMD_Id;
                                obj.TTRDSU_AllotedFlag = "No";
                                obj.TTRDSU_ActiveFlag = true;
                                obj.TTRDSU_SUbSelcFlag = false;
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
                    else if (data.TTRDSU_SUbSelcFlag == true)
                    {
                        if (data.TTRDSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id && t.TTRDSU_Id != data.TTRDSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Day_SubjectDMO.Single(t => t.TTRDSU_Id == data.TTRDSU_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMD_Id = data.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRDSU_AllotedFlag = "No";
                                result.TTRDSU_ActiveFlag = true;
                                result.TTRDSU_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                var corres = _TTContext.CLGTT_Restricting_Day_SubjectDMO.Where(e => e.TTRDSU_Id == data.TTRDSU_Id).ToList();

                                if (corres.Count > 0)
                                {
                                    foreach (var item in corres)
                                    {
                                        _TTContext.Remove(item);
                                    }

                                }
                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Restricting_Day_SubjectDMO ress = new CLGTT_Restricting_Day_SubjectDMO();

                                        ress.TTRDSU_Id = data.TTRDSU_Id;
                                        ress.AMCO_Id = item.AMCO_Id;
                                        ress.AMB_Id = item.AMB_Id;
                                        ress.AMSE_Id = item.AMSE_Id;
                                        ress.ACMS_Id = item.ACMS_Id;
                                        ress.HRME_Id = item.HRME_Id;
                                        ress.TTRDSUCB_Periods = item.NOP;
                                        ress.TTRDSUCB_ActiveFlag = true;
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
                            var result = _TTContext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMD_Id == data.TTMD_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Day_SubjectDMO resdata = new TT_Restricting_Day_SubjectDMO();
                                resdata.MI_Id = data.MI_Id;
                                resdata.ASMAY_Id = data.ASMAY_Id;
                                resdata.ISMS_Id = data.ISMS_Id;
                                resdata.TTMD_Id = data.TTMD_Id;
                                resdata.UpdatedDate = DateTime.Now;
                                resdata.UpdatedDate = DateTime.Now;
                                resdata.TTRDSU_AllotedFlag = "No";
                                resdata.TTRDSU_ActiveFlag = true;
                                resdata.TTRDSU_SUbSelcFlag = true;
                                _TTContext.Add(resdata);
                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Restricting_Day_SubjectDMO ress = new CLGTT_Restricting_Day_SubjectDMO();

                                        ress.TTRDSU_Id = resdata.TTRDSU_Id;
                                        ress.AMCO_Id = item.AMCO_Id;
                                        ress.AMB_Id = item.AMB_Id;
                                        ress.AMSE_Id = item.AMSE_Id;
                                        ress.ACMS_Id = item.ACMS_Id;
                                        ress.HRME_Id = item.HRME_Id;
                                        ress.TTRDSUCB_Periods = item.NOP;
                                        ress.TTRDSUCB_ActiveFlag = true;
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
                    }
                }
                else
                {
                    data.returnfixstatus = "Fixed";
                    return data;
                }
                data.all_restrict_day_subject_list = (from a in _TTContext.AcademicYear
                                                 from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                 from g in _TTContext.TT_Restricting_Day_SubjectDMO
                                                 from i in _TTContext.TT_Master_DayDMO
                                                 where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                 select new CLGRestrictionDTO
                                                 {
                                                     TTRDSU_Id = g.TTRDSU_Id,
                                                     ASMAY_Year = a.ASMAY_Year,
                                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                                     TTMD_Id = g.TTMD_Id,
                                                     TTMD_DayName = i.TTMD_DayName,
                                                     TTRDSU_SUbSelcFlag = g.TTRDSU_SUbSelcFlag,
                                                     TTRDSU_ActiveFlag = g.TTRDSU_ActiveFlag,
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
        public CLGRestrictionDTO viewtab3grid(CLGRestrictionDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_RESTRICT_DAY_SUBJECT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTRDSU_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTRDSU_Id
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
        public CLGRestrictionDTO edittab3(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRDSU_SUbSelcFlag == false)
                {
                    data.restrict_day_subject_edit = _TTContext.TT_Restricting_Day_SubjectDMO.Where(a => a.TTRDSU_Id == data.TTRDSU_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_RESTRICT_DAY_SUBJECT_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTRDSU_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTRDSU_Id
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
                            data.restrict_day_subject_edit = retObject.ToArray();
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
        public CLGRestrictionDTO deactivatetab3(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRDSU_Id > 0)
                {
                    var result = _TTContext.TT_Restricting_Day_SubjectDMO.Single(t => t.TTRDSU_Id == data.TTRDSU_Id);

                    if (result.TTRDSU_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Restricting_Day_SubjectDMO.Where(t => t.TTRDSU_Id == data.TTRDSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRDSUCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTRDSU_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Restricting_Day_SubjectDMO.Where(t => t.TTRDSU_Id == data.TTRDSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRDSUCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTRDSU_ActiveFlag = true;
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
        public CLGRestrictionDTO savetab4(CLGRestrictionDTO data)
        {
            try
            {
                var restrict_count = _TTContext.TT_Fixing_Period_StaffDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.HRME_Id == data.HRME_Id && r.TTMP_Id == data.TTMP_Id && r.TTFPS_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (data.TTRPS_SUbSelcFlag == false)
                    {
                        if (data.TTRPS_Id > 0)
                        {

                            var result0 = _TTContext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id && t.TTRPS_Id != data.TTRPS_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Period_StaffDMO.Single(t => t.TTRPS_Id == data.TTRPS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRPS_AllotedFlag = "No";
                                result.TTRPS_ActiveFlag = true;
                                result.TTRPS_SUbSelcFlag = false;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Restricting_Period_StaffDMO.Where(t => t.TTRPS_Id == data.TTRPS_Id).ToList();
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
                            var result = _TTContext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Period_StaffDMO ress = new TT_Restricting_Period_StaffDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.HRME_Id = data.HRME_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.CreatedDate = DateTime.Now;
                                ress.UpdatedDate = DateTime.Now;
                                ress.TTRPS_AllotedFlag = "No";
                                ress.TTRPS_ActiveFlag = true;
                                ress.TTRPS_SUbSelcFlag = false;
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
                    else if (data.TTRPS_SUbSelcFlag == true)
                    {
                        if (data.TTRPS_Id > 0)
                        {

                            var result0 = _TTContext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id.Equals(data.HRME_Id) && t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.TTMP_Id.Equals(data.TTMP_Id) && t.TTRPS_Id != data.TTRPS_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Period_StaffDMO.Single(t => t.TTRPS_Id == data.TTRPS_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.HRME_Id = data.HRME_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRPS_AllotedFlag = "No";
                                result.TTRPS_ActiveFlag = true;
                                result.TTRPS_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Restricting_Period_StaffDMO.Where(t => t.TTRPS_Id == data.TTRPS_Id).ToList();
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
                                        CLGTT_Restricting_Period_StaffDMO newres = new CLGTT_Restricting_Period_StaffDMO();

                                        newres.TTRPS_Id = data.TTRPS_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.ISMS_Id = item.ISMS_Id;
                                        newres.TTRPSCB_Days = item.NOD;
                                        newres.TTRPSCB_ActiveFlag = true;
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
                            var result = _TTContext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id == data.HRME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Period_StaffDMO ress = new TT_Restricting_Period_StaffDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.HRME_Id = data.HRME_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.UpdatedDate = DateTime.Now;
                                ress.CreatedDate = DateTime.Now;
                                ress.TTRPS_AllotedFlag = "No";
                                ress.TTRPS_ActiveFlag = true;
                                ress.TTRPS_SUbSelcFlag = true;
                                _TTContext.Add(ress);


                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Restricting_Period_StaffDMO newres = new CLGTT_Restricting_Period_StaffDMO();

                                        newres.TTRPS_Id = ress.TTRPS_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.ISMS_Id = item.ISMS_Id;
                                        newres.TTRPSCB_Days = item.NOD;
                                        newres.TTRPSCB_ActiveFlag = true;
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
                    data.returnfixstatus = "Fixed";
                    return data;
                }
                data.all_restrict_period_staff_list = (from a in _TTContext.AcademicYear
                                                  from e in _TTContext.HR_Master_Employee_DMO
                                                  from g in _TTContext.TT_Restricting_Period_StaffDMO
                                                  from b in _TTContext.TT_Master_PeriodDMO
                                                  where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                  select new CLGRestrictionDTO
                                                  {
                                                      TTRPS_Id = g.TTRPS_Id,
                                                      ASMAY_Year = a.ASMAY_Year,
                                                      staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                      TTMP_Id = g.TTMP_Id,
                                                      TTMP_PeriodName = b.TTMP_PeriodName,
                                                      TTRPS_SUbSelcFlag = g.TTRPS_SUbSelcFlag,
                                                      TTRPS_ActiveFlag = g.TTRPS_ActiveFlag,
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
        public CLGRestrictionDTO viewtab4(CLGRestrictionDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_RESTRICT_PERIOD_STAFF_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTRPS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTRPS_Id
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
        public CLGRestrictionDTO edittab4(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRPS_SUbSelcFlag == false)
                {
                    data.restrict_period_staff_edit = _TTContext.TT_Restricting_Period_StaffDMO.Where(a => a.TTRPS_Id == data.TTRPS_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_RESTRICT_PERIOD_STAFF_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTRPS_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTRPS_Id
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
                            data.restrict_period_staff_edit = retObject.ToArray();
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
        public CLGRestrictionDTO deactivatetab4(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRPS_Id > 0)
                {
                    var result = _TTContext.TT_Restricting_Period_StaffDMO.Single(t => t.TTRPS_Id == data.TTRPS_Id);

                    if (result.TTRPS_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Restricting_Period_StaffDMO.Where(t => t.TTRPS_Id == data.TTRPS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRPSCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTRPS_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Restricting_Period_StaffDMO.Where(t => t.TTRPS_Id == data.TTRPS_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRPSCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTRPS_ActiveFlag = true;
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
        public CLGRestrictionDTO savetab5(CLGRestrictionDTO data)
        {
           try
          {
             var restrict_count = _TTContext.TT_Fixing_Period_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id && r.ISMS_Id == data.ISMS_Id && r.TTMP_Id == data.TTMP_Id && r.TTFPSU_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (data.TTRPSU_SUbSelcFlag == false)
                    {
                        if (data.TTRPSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id && t.TTRPSU_Id != data.TTRPSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Period_SubjectDMO.Single(t => t.TTRPSU_Id == data.TTRPSU_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRPSU_AllotedFlag = "No";
                                result.TTRPSU_ActiveFlag = true;
                                result.TTRPSU_SUbSelcFlag = false;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Restricting_Period_SubjectDMO.Where(t => t.TTRPSU_Id == data.TTRPSU_Id).ToList();
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
                            var result = _TTContext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Period_SubjectDMO ress = new TT_Restricting_Period_SubjectDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.ISMS_Id = data.ISMS_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.CreatedDate = DateTime.Now;
                                ress.UpdatedDate = DateTime.Now;
                                ress.TTRPSU_AllotedFlag = "No";
                                ress.TTRPSU_ActiveFlag = true;
                                ress.TTRPSU_SUbSelcFlag = false;
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
                    else if (data.TTRPSU_SUbSelcFlag == true)
                    {
                        if (data.TTRPSU_Id > 0)
                        {

                            var result0 = _TTContext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(data.ISMS_Id) && t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.TTMP_Id.Equals(data.TTMP_Id) && t.TTRPSU_Id != data.TTRPSU_Id);
                            if (result0.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _TTContext.TT_Restricting_Period_SubjectDMO.Single(t => t.TTRPSU_Id == data.TTRPSU_Id && t.MI_Id == data.MI_Id);
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.ISMS_Id = data.ISMS_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRPSU_AllotedFlag = "No";
                                result.TTRPSU_ActiveFlag = true;
                                result.TTRPSU_SUbSelcFlag = true;
                                _TTContext.Update(result);
                                var rescor = _TTContext.CLGTT_Restricting_Period_SubjectDMO.Where(t => t.TTRPSU_Id == data.TTRPSU_Id).ToList();
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
                                        CLGTT_Restricting_Period_SubjectDMO newres = new CLGTT_Restricting_Period_SubjectDMO();

                                        newres.TTRPSU_Id = data.TTRPSU_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.HRME_Id = item.HRME_Id;
                                        newres.TTRPSUCB_Days = item.NOD;
                                        newres.TTRPSUCB_ActiveFlag = true;
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
                            var result = _TTContext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.TTMP_Id == data.TTMP_Id);
                            if (result.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                TT_Restricting_Period_SubjectDMO ress = new TT_Restricting_Period_SubjectDMO();
                                ress.MI_Id = data.MI_Id;
                                ress.ASMAY_Id = data.ASMAY_Id;
                                ress.ISMS_Id = data.ISMS_Id;
                                ress.TTMP_Id = data.TTMP_Id;
                                ress.UpdatedDate = DateTime.Now;
                                ress.CreatedDate = DateTime.Now;
                                ress.TTRPSU_AllotedFlag = "No";
                                ress.TTRPSU_ActiveFlag = true;
                                ress.TTRPSU_SUbSelcFlag = true;
                                _TTContext.Add(ress);


                                if (data.TempararyArrayList.Length > 0)
                                {
                                    foreach (var item in data.TempararyArrayList)
                                    {
                                        CLGTT_Restricting_Period_SubjectDMO newres = new CLGTT_Restricting_Period_SubjectDMO();

                                        newres.TTRPSU_Id = ress.TTRPSU_Id;
                                        newres.AMCO_Id = item.AMCO_Id;
                                        newres.AMB_Id = item.AMB_Id;
                                        newres.AMSE_Id = item.AMSE_Id;
                                        newres.ACMS_Id = item.ACMS_Id;
                                        newres.HRME_Id = item.HRME_Id;
                                        newres.TTRPSUCB_Days = item.NOD;
                                        newres.TTRPSUCB_ActiveFlag = true;
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
                    data.returnfixstatus = "Fixed";
                    return data;
                }
                data.all_restrict_period_subject_list = (from a in _TTContext.AcademicYear
                                                    from e in _TTContext.IVRM_School_Master_SubjectsDMO
                                                    from g in _TTContext.TT_Restricting_Period_SubjectDMO
                                                    from b in _TTContext.TT_Master_PeriodDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                    select new CLGRestrictionDTO
                                                    {
                                                        TTRPSU_Id = g.TTRPSU_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        ISMS_SubjectName = e.ISMS_SubjectName,
                                                        TTMP_Id = g.TTMP_Id,
                                                        TTMP_PeriodName = b.TTMP_PeriodName,
                                                        TTRPSU_SUbSelcFlag = g.TTRPSU_SUbSelcFlag,
                                                        TTRPSU_ActiveFlag = g.TTRPSU_ActiveFlag,
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
        public CLGRestrictionDTO viewtab5(CLGRestrictionDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_RESTRICT_PERIOD_SUBJECT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTRPSU_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTRPSU_Id
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
        public CLGRestrictionDTO edittab5(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRPSU_SUbSelcFlag == false)
                {
                    data.restrict_period_subject_edit = _TTContext.TT_Restricting_Period_SubjectDMO.Where(a => a.TTRPSU_Id == data.TTRPSU_Id).ToArray();
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_EDIT_RESTRICT_PERIOD_SUBJECT_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTRPSU_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.TTRPSU_Id
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
                            data.restrict_period_subject_edit = retObject.ToArray();
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
        public CLGRestrictionDTO deactivatetab5(CLGRestrictionDTO data)
        {
            try
            {

                if (data.TTRPSU_Id > 0)
                {
                    var result = _TTContext.TT_Restricting_Period_SubjectDMO.Single(t => t.TTRPSU_Id == data.TTRPSU_Id);

                    if (result.TTRPSU_ActiveFlag == true)
                    {

                        var subresult = _TTContext.CLGTT_Restricting_Period_SubjectDMO.Where(t => t.TTRPSU_Id == data.TTRPSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRPSUCB_ActiveFlag = false;
                            _TTContext.Update(item);
                        }

                        result.TTRPSU_ActiveFlag = false;
                    }
                    else
                    {
                        var subresult = _TTContext.CLGTT_Restricting_Period_SubjectDMO.Where(t => t.TTRPSU_Id == data.TTRPSU_Id).ToList();

                        foreach (var item in subresult)
                        {
                            item.TTRPSUCB_ActiveFlag = true;
                            _TTContext.Update(item);
                        }

                        result.TTRPSU_ActiveFlag = true;
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
