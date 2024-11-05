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
    public class CLGConsecutiveImpl : Interfaces.CLGConsecutiveInterface
    {
        private static ConcurrentDictionary<string, CLGConsecutiveDTO> _login =
             new ConcurrentDictionary<string, CLGConsecutiveDTO>();

        public TTContext _TTContext;
        ILogger<CLGConsecutiveImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGConsecutiveImpl(TTContext academiccontext, ILogger<CLGConsecutiveImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        #region LOAD ALL DATA
        public CLGConsecutiveDTO getalldetails(CLGConsecutiveDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.catelist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.academiclist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();
                //   data.daydropdown = _TTContext.TT_Master_DayDMO.Where(u => u.MI_Id == data.MI_Id && u.TTMD_ActiveFlag == true).Distinct().ToArray();

                data.sectionlist = _TTContext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_LOAD_CONSECUTIVE_DETAILS";
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
                        data.consecutivelst = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE CONSECATIVE
        public CLGConsecutiveDTO savedetail(CLGConsecutiveDTO data)
        {

            try
            {
                if (data.TTCC_Id > 0)
                {
                    var resultw = _TTContext.CLGTT_ConsecutiveDMO.Where(t => t.AMCO_Id==data.AMCO_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.AMB_Id==data.AMB_Id
                    && t.HRME_Id==data.HRME_Id && t.ISMS_Id==data.ISMS_Id && t.TTCC_NoOfPeriods==data.TTCC_NoOfPeriods && t.TTCC_RemPeriods==data.TTCC_RemPeriods 
                    && t.TTCC_NoOfConPeriods==data.TTCC_NoOfConPeriods  && t.TTCC_NoOfConDays == data.TTCC_NoOfConDays
                    && t.TTCC_BefAftApplFlag == data.TTCC_BefAftApplFlag && t.TTCC_BefAftFalg == data.TTCC_BefAftFalg && t.TTCC_BefAftPeriod == data.TTCC_BefAftPeriod && t.AMSE_Id==data.AMSE_Id
                     && t.ACMS_Id==data.ACMS_Id && t.TTCC_Id !=data.TTCC_Id
                   ).ToList();
                    if (resultw.Count>0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _TTContext.CLGTT_ConsecutiveDMO.Single(t => t.TTCC_Id == data.TTCC_Id);
                        result.AMCO_Id = data.AMCO_Id;
                        result.AMB_Id = data.AMB_Id;
                        result.AMSE_Id = data.AMSE_Id;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.TTMC_Id = data.TTMC_Id;
                        result.ACMS_Id = data.ACMS_Id;
                        result.HRME_Id = data.HRME_Id;
                        result.ISMS_Id = data.ISMS_Id;
                        result.TTCC_NoOfPeriods = data.TTCC_NoOfPeriods;
                        result.TTCC_AllotPeriods = 0;
                        result.TTCC_RemPeriods = data.TTCC_RemPeriods;
                        result.TTCC_NoOfConPeriods = data.TTCC_NoOfConPeriods;
                        result.TTCC_NoOfConDays = data.TTCC_NoOfConDays;
                        result.TTCC_BefAftApplFlag = data.TTCC_BefAftApplFlag;
                        result.TTCC_BefAftFalg = data.TTCC_BefAftFalg;
                        result.TTCC_BefAftPeriod = data.TTCC_BefAftPeriod;
                        result.TTCC_AllotedFlag = data.TTCC_AllotedFlag;
                        result.UpdatedDate = DateTime.Now;
                        _TTContext.Update(result);
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
                   
                }
                else
                {
                    var resultw = _TTContext.CLGTT_ConsecutiveDMO.Where(t => t.AMCO_Id == data.AMCO_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMB_Id == data.AMB_Id  && t.HRME_Id == data.HRME_Id && t.ISMS_Id == data.ISMS_Id && t.AMSE_Id == data.AMSE_Id && t.ACMS_Id == data.ACMS_Id && t.TTMC_Id==data.TTMC_Id
                  ).ToList();
                    if (resultw.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        CLGTT_ConsecutiveDMO result = new CLGTT_ConsecutiveDMO();

                        result.MI_Id = data.MI_Id;
                        result.AMCO_Id = data.AMCO_Id;
                        result.AMB_Id = data.AMB_Id;
                        result.AMSE_Id = data.AMSE_Id;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.TTMC_Id = data.TTMC_Id;
                        result.ACMS_Id = data.ACMS_Id;
                        result.HRME_Id = data.HRME_Id;
                        result.ISMS_Id = data.ISMS_Id;
                        result.TTCC_NoOfPeriods = data.TTCC_NoOfPeriods;
                        result.TTCC_AllotPeriods = 0;
                        result.TTCC_RemPeriods = data.TTCC_RemPeriods;
                        result.TTCC_NoOfConPeriods = data.TTCC_NoOfConPeriods;
                        result.TTCC_NoOfConDays = data.TTCC_NoOfConDays;
                        result.TTCC_BefAftApplFlag = data.TTCC_BefAftApplFlag;
                        result.TTCC_BefAftFalg = data.TTCC_BefAftFalg;
                        result.TTCC_BefAftPeriod = data.TTCC_BefAftPeriod;
                        result.TTCC_AllotedFlag = data.TTCC_AllotedFlag;
                        result.TTCC_ActiveFlag = true;
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
                        _TTContext.Add(result);
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

                  

                }



                //if (data.TTMD_Id > 0)
                //{
                //    var res = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower()) && t.TTMD_Id != data.TTMD_Id).ToList();
                //    if (res.Count > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        var result = _TTContext.TT_Master_DayDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.TTMD_Id);
                //        result.TTMD_DayCode = data.TTMD_DayCode.ToUpper();
                //        result.TTMD_DayName = data.TTMD_DayName.ToUpper();
                //        result.UpdatedDate = DateTime.Now;
                //        result.TTMD_ActiveFlag = true;
                //        _TTContext.Update(result);
                //        var contactExists = _TTContext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }

                //    }
                //}
                //else
                //{
                //    var res = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower())).ToList();
                //    if (res.Count() > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        long OID = 0;
                //        var orderId = _TTContext.TT_Master_DayDMO.Where(f => f.MI_Id == data.MI_Id).ToList();
                //        if (orderId.Count==0)
                //        {
                //            OID = 1;
                //        }
                //        else
                //        {
                //         long ooid= orderId.Select(r => r.Order_Id).Max();
                //            OID = ooid + 1;
                //        }

                //        TT_Master_DayDMO obj = new TT_Master_DayDMO();
                //        obj.MI_Id = data.MI_Id;
                //        obj.TTMD_DayName = data.TTMD_DayName.Trim().ToUpper();
                //        obj.TTMD_DayCode = data.TTMD_DayCode.Trim().ToUpper(); ;
                //        obj.Order_Id = OID;
                //        obj.TTMD_ActiveFlag = true;
                //        obj.CreatedDate = DateTime.Now;
                //        obj.UpdatedDate = DateTime.Now;
                //        _TTContext.Add(obj);
                //        var contactExists = _TTContext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        #endregion

        #region EDIT 
        public CLGConsecutiveDTO editconv(CLGConsecutiveDTO data)
        {

            try
            {
                var consecutivelstedit = _TTContext.CLGTT_ConsecutiveDMO.Where(t => t.MI_Id == data.MI_Id && t.TTCC_Id == data.TTCC_Id).ToList(); 
                data.consecutivelstedit = _TTContext.CLGTT_ConsecutiveDMO.Where(t => t.MI_Id == data.MI_Id && t.TTCC_Id == data.TTCC_Id).ToArray();

                data.courselist = (from a in _TTContext.CLGTT_Category_CourseBranchDMO
                                   from b in _TTContext.MasterCourseDMO
                                   where b.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == consecutivelstedit[0].ASMAY_Id && a.TTMC_Id == consecutivelstedit[0].TTMC_Id && a.TTCC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                   select b
                                ).Distinct().ToArray();

                data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _TTContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == consecutivelstedit[0].ASMAY_Id && a.AMCO_Id == consecutivelstedit[0].AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                     from b in _TTContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _TTContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == consecutivelstedit[0].ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == consecutivelstedit[0].AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == consecutivelstedit[0].AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
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
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVE 
        public CLGConsecutiveDTO deactivate(CLGConsecutiveDTO data)
        {
            try
            {

                if (data.TTCC_Id > 0)
                {
                    var result = _TTContext.CLGTT_ConsecutiveDMO.Single(t => t.TTCC_Id == data.TTCC_Id);

                    if (result.TTCC_ActiveFlag == true)
                    {
                        result.TTCC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTCC_ActiveFlag = true;
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag.Equals(1))
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
