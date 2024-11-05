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
    public class CLGBifurcationImpl : Interfaces.CLGBifurcationInterface
    {
        private static ConcurrentDictionary<string, CLGBifurcationDTO> _login =
             new ConcurrentDictionary<string, CLGBifurcationDTO>();

        public TTContext _TTContext;
        ILogger<CLGBifurcationImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGBifurcationImpl(TTContext academiccontext, ILogger<CLGBifurcationImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        #region LOAD ALL DATA
        public CLGBifurcationDTO getalldetails(CLGBifurcationDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.categorylist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.yearlist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();

                data.periodlist = _TTContext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id==data.MI_Id && t.TTMP_ActiveFlag == true).ToArray();

                data.sectionlist = _TTContext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id==data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();



                data.detailslist = (from m in _TTContext.TT_Bifurcation_DMO
                                     from q in _TTContext.AcademicYear
                                     from s in _TTContext.TTMasterCategoryDMO
                                     where (m.MI_Id == data.MI_Id && m.ASMAY_Id == q.ASMAY_Id && m.TTMC_Id == s.TTMC_Id && m.MI_Id==q.MI_Id && m.MI_Id==s.MI_Id)
                                     select new TT_Bifurcation_DTO
                                     {
                                         AcdYear = q.ASMAY_Year,
                                         categoryName = s.TTMC_CategoryName,
                                         bifricationName = m.TTB_BifurcationName,
                                         periodname = m.TTB_NoOfPeriods.ToString(),
                                         TTB_Id = m.TTB_Id,
                                         TTB_ActiveFlag = m.TTB_ActiveFlag

                                     }).OrderByDescending(d => d.UpdatedDate).ToArray();


            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE BIFURCATION
        public CLGBifurcationDTO savedetailBiff(CLGBifurcationDTO data)
        {
            
            try
            {
                if (data.TTB_Id > 0)
                {
                    var res = _TTContext.TT_Bifurcation_DMO.Where(t => t.MI_Id == data.MI_Id && t.TTB_BifurcationName.Trim() == data.TTB_BifurcationName.Trim() && t.TTB_ActiveFlag == true && t.TTB_Id !=data.TTB_Id).ToList();
                    if (res.Count>0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {

                        var result1 = _TTContext.TT_Bifurcation_DMO.Where(t => t.MI_Id == data.MI_Id && t.TTB_Id == data.TTB_Id).Single();
                        result1.ASMAY_Id = data.ASMAY_Id;
                        result1.TTMC_Id = data.TTMC_Id;
                        result1.TTB_BifurcationName = data.TTB_BifurcationName;
                        result1.TTB_NoOfPeriods = data.TTB_NoOfPeriods;
                        result1.TTB_RemPeriods = data.TTB_NoOfPeriods;
                        result1.TTB_ConsecutiveFlag = data.TTB_ConsecutiveFlag;
                        if (data.TTB_ConsecutiveFlag == 1)
                        {
                            result1.TTB_NoOfConPeriods = data.TTB_NoOfConPeriods;
                            result1.TTB_NoOfConDays = data.TTB_NoOfConDays;
                        }
                        else
                        {
                            result1.TTB_NoOfConPeriods = 0;
                            result1.TTB_NoOfConDays = 0;
                        }
                        result1.TTB_BefAftApplFlag = data.TTB_BefAftApplFlag;
                        if (data.TTB_BefAftApplFlag == 1)
                        {
                            result1.TTB_BefAftFalg = data.TTB_BefAftFalg;
                        }
                        else
                        {
                            result1.TTB_BefAftFalg = "";
                        }
                        result1.TTMP_Id = data.TTMP_Id;
                        result1.TTB_AllotedFlag = "No";
                        result1.TTB_ActiveFlag = true;
                        result1.UpdatedDate = DateTime.Now;
                        _TTContext.Update(result1);

                        var details = _TTContext.CLGBifurcationDetailsDMO.Where(t=>t.TTB_Id == data.TTB_Id).ToList();

                        if (details.Count>0)
                        {
                            foreach (var item1 in details)
                            {
                                _TTContext.Remove(item1);
                            }
                        }

                        foreach (var item in data.combinationlist)
                        {
                            CLGBifurcationDetailsDMO detobj = new CLGBifurcationDetailsDMO();
                            detobj.TTB_Id = data.TTB_Id;
                            detobj.AMCO_Id = item.AMCO_Id;
                            detobj.AMB_Id = item.AMB_Id;
                            detobj.AMSE_Id = item.AMSE_Id;
                            detobj.ACMS_Id = item.ACMS_Id;
                            detobj.HRME_Id = item.HRME_Id;
                            detobj.ISMS_Id = item.ISMS_Id;
                            detobj.TTBDC_ActiveFlag = true;
                            detobj.CreatedDate = DateTime.Now;
                            detobj.UpdatedDate = DateTime.Now;
                            _TTContext.Add(detobj);
                        }
                        var flag1 = _TTContext.SaveChanges();
                        if (flag1 > 0)
                        {
                            data.returnMsg = "Add";
                        }
                    }
                }
                else
                {
                    var res = _TTContext.TT_Bifurcation_DMO.Where(t => t.MI_Id == data.MI_Id && t.TTB_BifurcationName.Trim() == data.TTB_BifurcationName.Trim() && t.TTB_ActiveFlag==true).ToList();
                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TT_Bifurcation_DMO bobj = new TT_Bifurcation_DMO();
                        bobj.MI_Id = data.MI_Id;
                        bobj.ASMAY_Id = data.ASMAY_Id;
                        bobj.TTMC_Id = data.TTMC_Id;
                        bobj.TTB_BifurcationName = data.TTB_BifurcationName;
                        bobj.TTB_NoOfPeriods = data.TTB_NoOfPeriods;
                        bobj.TTB_RemPeriods = data.TTB_NoOfPeriods;
                        bobj.TTB_ConsecutiveFlag = data.TTB_ConsecutiveFlag;
                        if (data.TTB_ConsecutiveFlag == 1)
                        {
                            bobj.TTB_NoOfConPeriods = data.TTB_NoOfConPeriods;
                            bobj.TTB_NoOfConDays = data.TTB_NoOfConDays;
                        }
                        else
                        {
                            bobj.TTB_NoOfConPeriods = 0;
                            bobj.TTB_NoOfConDays = 0;
                        }
                        bobj.TTB_BefAftApplFlag = data.TTB_BefAftApplFlag;
                        if (data.TTB_BefAftApplFlag == 1)
                        {
                            bobj.TTB_BefAftFalg = data.TTB_BefAftFalg;
                        }
                        else
                        {
                            bobj.TTB_BefAftFalg = "";
                        }
                        bobj.TTMP_Id = data.TTMP_Id;
                        bobj.TTB_AllotedFlag = "No";
                        bobj.TTB_ActiveFlag = true;
                        bobj.CreatedDate = DateTime.Now;
                        bobj.UpdatedDate = DateTime.Now;
                        _TTContext.Add(bobj);

                        foreach (var item in data.combinationlist)
                        {
                            CLGBifurcationDetailsDMO detobj = new CLGBifurcationDetailsDMO();
                            detobj.TTB_Id = bobj.TTB_Id;
                            detobj.AMCO_Id = item.AMCO_Id;
                            detobj.AMB_Id = item.AMB_Id;
                            detobj.AMSE_Id = item.AMSE_Id;
                            detobj.ACMS_Id = item.ACMS_Id;
                            detobj.HRME_Id = item.HRME_Id;
                            detobj.ISMS_Id = item.ISMS_Id;
                            detobj.TTBDC_ActiveFlag = true;
                            detobj.CreatedDate = DateTime.Now;
                            detobj.UpdatedDate = DateTime.Now;
                            _TTContext.Add(detobj);
                        }
                        var flag1 = _TTContext.SaveChanges();
                        if (flag1 >0)
                        {
                            data.returnMsg = "Add";
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
        #endregion

        #region EDIT DAY
        public CLGBifurcationDTO editDay(CLGBifurcationDTO data)
        {

            try
            {
                data.Daylistedit = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.TTMD_Id).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVE BIFURCATION
        public CLGBifurcationDTO deactivatebiff(CLGBifurcationDTO data)
        {
            try
            {

                if (data.TTB_Id > 0)
                {
                    var result = _TTContext.TT_Bifurcation_DMO.Single(t => t.TTB_Id==data.TTB_Id);


                    if (result.TTB_ActiveFlag==true)
                    {

                        var exist = _TTContext.CLGBifurcationDetailsDMO.Where(t => t.TTB_Id == result.TTB_Id).ToList();
                        if (exist.Count>0)
                        {
                            foreach (var item in exist)
                            {
                                item.TTBDC_ActiveFlag = false;
                                item.UpdatedDate = DateTime.Now;
                                _TTContext.Update(item);
                            }

                            result.TTB_ActiveFlag = false;
                            result.UpdatedDate = DateTime.Now;
                            _TTContext.Update(result);
                        }

                    }
                    else
                    {
                        var exist = _TTContext.CLGBifurcationDetailsDMO.Where(t => t.TTB_Id == result.TTB_Id).ToList();
                        if (exist.Count > 0)
                        {
                            foreach (var item in exist)
                            {
                                item.TTBDC_ActiveFlag = true;
                                item.UpdatedDate = DateTime.Now;
                                _TTContext.Update(item);
                            }

                            result.TTB_ActiveFlag = true;
                            result.UpdatedDate = DateTime.Now;
                            _TTContext.Update(result);
                        }
                    }

                    var flag = _TTContext.SaveChanges();
                    if (flag>0)
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

        #region GET BRANCH
        public CLGBifurcationDTO getBranch(CLGBifurcationDTO data)
        {
            try
            {


                data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _TTContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
            

              

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region EDIT BIFURCATION DETAILS
        public CLGBifurcationDTO editbiff(CLGBifurcationDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_EDIT_BIF_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTB_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTB_Id
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
                        data.editdetailslist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                _dataimpl.LogError(ee.Message);
                _dataimpl.LogDebug(ee.Message);
            }

            return data;
        }
        #endregion

        #region VIEW BIFURCATION DETAILS
        public CLGBifurcationDTO viewrecordspopup(CLGBifurcationDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_BIF_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTB_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTB_Id
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
                        data.viewdata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                //data.message = "Sorry...You Can't delete this record because it is used in other operation.";
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion
    }
}
