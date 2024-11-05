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
    public class CLGPRDDistributionImpl : Interfaces.CLGPRDDistributionInterface
    {
        private static ConcurrentDictionary<string, CLGPRDDistributionDTO> _login =
             new ConcurrentDictionary<string, CLGPRDDistributionDTO>();

        public TTContext _TTContext;
        ILogger<CLGPRDDistributionImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGPRDDistributionImpl(TTContext academiccontext, ILogger<CLGPRDDistributionImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        #region LOAD ALL DATA
        public CLGPRDDistributionDTO getalldetails(CLGPRDDistributionDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.categorylist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.yearlist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();

                data.sectionlist = _TTContext.Adm_College_Master_SectionDMO.Where(w => w.MI_Id == data.MI_Id && w.ACMS_ActiveFlag == true).ToArray();
                data.subjectlist = (from a in _TTContext.TT_Master_Subject_AbbreviationDMO
                                    from b in _TTContext.IVRM_School_Master_SubjectsDMO
                                    where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TTMSUAB_ActiveFlag == true && b.ISMS_ActiveFlag == 1 && a.ISMS_Id == b.ISMS_Id
                                    select new CLGPRDDistributionDTO
                                    {
                                        ISMS_Id = a.ISMS_Id,
                                        ISMS_SubjectName = b.ISMS_SubjectName,
                                        TTMSUAB_Abbreviation = a.TTMSUAB_Abbreviation,
                                    }).Distinct().ToArray();

                data.stafflist = (from a in _TTContext.TT_Master_Staff_AbbreviationDMO
                                   from b in _TTContext.HR_Master_Employee_DMO
                                   where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true  && b.HRME_ActiveFlag == true 
                                   )
                                   select new CLGPRDDistributionDTO
                                   {
                                       empName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName)+" " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                                       HRME_Id = b.HRME_Id,
                                       TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                   }).Distinct().OrderBy(j=>j.empName).ToArray();


                data.all_period_distri_list = (from a in _TTContext.AcademicYear
                                               from e in _TTContext.HR_Master_Employee_DMO
                                               from g in _TTContext.TT_Final_Period_DistributionDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id)
                                               select new CLGPRDDistributionDTO
                                               {
                                                   TTFPD_Id = g.TTFPD_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName +" "+ (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTFPD_TotWeekPeriods = g.TTFPD_TotWeekPeriods,
                                                   TTFPD_ActiveFlag = g.TTFPD_ActiveFlag,
                                                   ASMAY_Order= a.ASMAY_Order

                                               }
    ).Distinct().OrderByDescending(x => x.ASMAY_Order).ThenBy(x => x.staffName).ToArray();


            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE PERIOD DESTRIBUTION
        public CLGPRDDistributionDTO savedetail(CLGPRDDistributionDTO data)
        {

            try
            {
                if (data.TTFPD_Id > 0)
                {
                    var result0 = _TTContext.TT_Final_Period_DistributionDMO.Where(t => t.HRME_Id==data.HRME_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.TTFPD_Id != data.TTFPD_Id);
                    if (result0.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {

                        var existdetails = _TTContext.CLGTT_PRDDistributionDetailsDMO.Where(f => f.TTFPD_Id == data.TTFPD_Id).ToList();
                        if (existdetails.Count>0)
                        {
                            foreach (var item in existdetails)
                            {
                                _TTContext.Remove(item);
                            }

                        }

                        var result = _TTContext.TT_Final_Period_DistributionDMO.Single(t => t.TTFPD_Id==data.TTFPD_Id && t.MI_Id==data.MI_Id);
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.HRME_Id = data.HRME_Id;
                        result.TTFPD_TotWeekPeriods = data.TTFPD_TotWeekPeriods;
                        result.TTFPD_ActiveFlag = true;
                        result.TTFPD_StaffConsecutive = data.TTFPD_StaffConsecutive;
                        result.UpdatedDate = DateTime.Now;
                        _TTContext.Update(result);
                        if (data.TempararyArrayList.Length > 0)
                        {
                            foreach (var item in data.TempararyArrayList)
                            {
                                CLGTT_PRDDistributionDetailsDMO dtobj = new CLGTT_PRDDistributionDetailsDMO();
                                dtobj.TTFPD_Id = data.TTFPD_Id;
                                dtobj.TTMC_Id = item.TTMC_Id;
                                dtobj.AMCO_Id = item.AMCO_Id;
                                dtobj.AMB_Id = item.AMB_Id;
                                dtobj.AMSE_Id = item.AMSE_Id;
                                dtobj.ACMS_Id = item.ACMS_Id;
                                dtobj.ISMS_Id = item.ISMS_Id;
                                dtobj.TTFPDC_TotalPeriods = Convert.ToInt32(item.NOP);
                                dtobj.TTFPDC_AllotedPeriods = 0;
                                dtobj.TTFPDC_AvailablePeriods = Convert.ToInt32(item.NOP);
                                dtobj.TTFPDC_ActiveFlag = true;
                                dtobj.CreatedDate = DateTime.Now;
                                dtobj.UpdatedDate = DateTime.Now;
                                _TTContext.Add(dtobj);
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
                else
                {
                    var result = _TTContext.TT_Final_Period_DistributionDMO.Where(t => t.HRME_Id==data.HRME_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id);
                    if (result.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TT_Final_Period_DistributionDMO obj = new TT_Final_Period_DistributionDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.TTFPD_TotWeekPeriods = data.TTFPD_TotWeekPeriods;
                        obj.TTFPD_StaffConsecutive = data.TTFPD_StaffConsecutive;
                        obj.TTFPD_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.TTFPD_ActiveFlag = true;
                        _TTContext.Add(obj);
                        if (data.TempararyArrayList.Length>0)
                        {
                            foreach (var item in data.TempararyArrayList)
                            {
                                CLGTT_PRDDistributionDetailsDMO dtobj = new CLGTT_PRDDistributionDetailsDMO();
                                dtobj.TTFPD_Id = obj.TTFPD_Id;
                                dtobj.TTMC_Id = item.TTMC_Id;
                                dtobj.AMCO_Id = item.AMCO_Id;
                                dtobj.AMB_Id = item.AMB_Id;
                                dtobj.AMSE_Id = item.AMSE_Id;
                                dtobj.ACMS_Id = item.ACMS_Id;
                                dtobj.ISMS_Id = item.ISMS_Id;
                                dtobj.TTFPDC_TotalPeriods = Convert.ToInt32(item.NOP);
                                dtobj.TTFPDC_AllotedPeriods = 0;
                                dtobj.TTFPDC_AvailablePeriods = Convert.ToInt32(item.NOP);
                                dtobj.TTFPDC_ActiveFlag = true;
                                dtobj.CreatedDate = DateTime.Now;
                                dtobj.UpdatedDate = DateTime.Now;
                                _TTContext.Add(dtobj);
                            }

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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        #endregion

        #region VIEW SELECTED EMPLOYEE PERIOD DETAILS
        public CLGPRDDistributionDTO viewperiods(CLGPRDDistributionDTO data)
        {

            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_PERIOD_DESTRIBUTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTFPD_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTFPD_Id
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
                        data.detailspopuparray = retObject.ToArray();
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
        #endregion

        #region ACTIVE/DEACTIVE PERIOD DESTRIBUTION
        public CLGPRDDistributionDTO deactivate(CLGPRDDistributionDTO data)
        {
            try
            {

                if (data.TTFPD_Id > 0)
                {
                    var result = _TTContext.TT_Final_Period_DistributionDMO.Single(t => t.TTFPD_Id==data.TTFPD_Id);
                    

                    var existlist = _TTContext.CLGTT_PRDDistributionDetailsDMO.Where(t => t.TTFPD_Id == result.TTFPD_Id).ToList();
                    if (existlist.Count > 0)
                    {

                        foreach (var item in existlist)
                        {
                            if (result.TTFPD_ActiveFlag==true)
                            {
                                item.TTFPDC_ActiveFlag = false;

                            }
                            else
                            {
                                item.TTFPDC_ActiveFlag = true;
                            }
                            _TTContext.Update(item);
                        }

                        if (result.TTFPD_ActiveFlag == true)
                        {
                            result.TTFPD_ActiveFlag = false;
                        }
                        else
                        {
                            result.TTFPD_ActiveFlag = true;
                        }
                       
                    }
                    _TTContext.Update(result);
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
        public CLGPRDDistributionDTO getBranch(CLGPRDDistributionDTO data)
        {
            try
            {


                ////data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                //                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                //                   from c in _TTContext.ClgMasterBranchDMO
                //                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                //                   select c
                //                 ).Distinct().ToArray();
            

              

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region GET THE EDIT DATA
        public CLGPRDDistributionDTO editprddestr(CLGPRDDistributionDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_EDIT_PERIOD_DESTRIBUTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTFPD_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTFPD_Id
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
                        data.period_distri_edit = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }

                var count = _TTContext.CLGTT_PRDDistributionDetailsDMO.Where(x => x.TTFPD_Id.Equals(data.TTFPD_Id) && x.TTFPDC_ActiveFlag == true).Sum(t => t.TTFPDC_TotalPeriods);
                data.edit_count = count;
            }
            catch (Exception ee)
            {
                _dataimpl.LogError(ee.Message);
                _dataimpl.LogDebug(ee.Message);
            }

            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVATE COURSE  DAY
        public CLGPRDDistributionDTO deactivecrsday(CLGPRDDistributionDTO data)
        {
            try
            {
               //var result = _TTContext.CLGTT_Master_Day_CourseBranchDMO.Single(t => t.TTMDC_Id == data.TTMDC_Id);

               // if (result.TTMDC_ActiveFlag.Equals(true))
               // {
               //     result.TTMDC_ActiveFlag = false;
               // }
               // else
               // {
               //     result.TTMDC_ActiveFlag = true;
               // }
               // _TTContext.Update(result);
               // var flag = _TTContext.SaveChanges();
               // if (flag.Equals(1))
               // {
               //     data.returnval = true;
               // }
               // else
               // {
               //     data.returnval = false;
               // }

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
