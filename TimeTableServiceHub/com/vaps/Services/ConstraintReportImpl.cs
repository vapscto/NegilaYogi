using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class ConstraintReportImpl : Interfaces.ConstraintReportInterface
    {
        public TTContext _ttcontext;

        public ConstraintReportImpl(TTContext cnt)
        {
            _ttcontext = cnt;
        }
        public TT_ConstraintReportDTO getalldetails(int id)
        {
            TT_ConstraintReportDTO data = new TT_ConstraintReportDTO();
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == Convert.ToInt64(id) && t.Is_Active == true).ToList().Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TT_ConstraintReportDTO getpageedit(TT_ConstraintReportDTO data)
        {
            try
            {

                //string groupidss = "0";


                //for (int d = 0; d < data.staffarray.Count(); d++)
                //{
                //    groupidss = groupidss + ',' + data.staffarray[d].HRME_Id;
                //}

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_CONSTRAINT_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@Ctype",
                    SqlDbType.NVarChar)
                    {
                        Value = data.constype
                    });
                    cmd.Parameters.Add(new SqlParameter("@Vtype",
                    SqlDbType.Char)
                    {
                        Value = data.periodtype
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
                        data.getreportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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

      //      {
      //          if(page.constype== "fix")
      //          {

//              page.fix_day_period_list = (from a in _ttcontext.AcademicYear
//                                          from b in _ttcontext.TTMasterCategoryDMO
//                                          from c in _ttcontext.School_M_Class
//                                          from d in _ttcontext.School_M_Section
//                                          from e in _ttcontext.HR_Master_Employee_DMO
//                                          from f in _ttcontext.IVRM_School_Master_SubjectsDMO
//                                          from g in _ttcontext.TT_Master_PeriodDMO
//                                          from h in _ttcontext.TT_Fixing_Day_PeriodDMO
//                                          from i in _ttcontext.TT_Master_DayDMO
//                                          where (a.MI_Id == h.MI_Id && a.ASMAY_Id == h.ASMAY_Id && b.MI_Id == h.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == h.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == h.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == h.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == h.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == h.MI_Id && g.TTMP_Id == h.TTMP_Id && i.MI_Id == h.MI_Id && i.TTMD_Id == h.TTMD_Id && h.MI_Id == page.MI_ID && h.ASMAY_Id== page.ASMAY_Id && h.TTFDP_ActiveFlag==true)
//                                          select new TT_ConstraintReportDTO
//                                          {
//                                              TTMC_CategoryName = b.TTMC_CategoryName,
//                                              ASMCL_ClassName = c.ASMCL_ClassName,
//                                              ASMC_SectionName = d.ASMC_SectionName,
//                                              staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
//                                              ISMS_SubjectName = f.ISMS_SubjectName,
//                                              TTMP_PeriodName = g.TTMP_PeriodName,
//                                              TTMD_DayName = i.TTMD_DayName                                                  
//                                          }
//).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();
//          }
//          else if(page.constype == "restrict")
//          {
//              page.restrict_day_period_list = (from a in _ttcontext.AcademicYear
//                                                 from b in _ttcontext.TTMasterCategoryDMO
//                                                 from c in _ttcontext.School_M_Class
//                                                 from d in _ttcontext.School_M_Section
//                                                 from e in _ttcontext.HR_Master_Employee_DMO
//                                                 from f in _ttcontext.IVRM_School_Master_SubjectsDMO
//                                                 from g in _ttcontext.TT_Master_PeriodDMO
//                                                 from h in _ttcontext.TT_Restricting_Day_PeriodDMO
//                                                 from i in _ttcontext.TT_Master_DayDMO
//                                                 where (a.MI_Id == h.MI_Id && a.ASMAY_Id == h.ASMAY_Id && b.MI_Id == h.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == h.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == h.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == h.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == h.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == h.MI_Id && g.TTMP_Id == h.TTMP_Id && i.MI_Id == h.MI_Id && i.TTMD_Id == h.TTMD_Id && h.MI_Id == page.MI_ID && h.ASMAY_Id == page.ASMAY_Id && h.TTRDP_ActiveFlag == true)
//                                                 select new TT_ConstraintReportDTO
//                                                 {
//                                                     TTMC_CategoryName = b.TTMC_CategoryName,
//                                                     ASMCL_ClassName = c.ASMCL_ClassName,
//                                                     ASMC_SectionName = d.ASMC_SectionName,
//                                                     staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
//                                                     ISMS_SubjectName = f.ISMS_SubjectName,
//                                                     TTMP_PeriodName = g.TTMP_PeriodName,
//                                                     TTMD_DayName = i.TTMD_DayName
//                                                 }
//                                   ).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();
//          }
//          else if(page.constype == "cons")
//          {
//              page.consecutivelst = (from TT_Master_Category in _ttcontext.TTMasterCategoryDMO
//                                     from TT_Consecutive in _ttcontext.TT_ConsecutiveDMO
//                                     from Adm_School_M_Class in _ttcontext.School_M_Class
//                                     from Adm_School_M_Section in _ttcontext.School_M_Section
//                                     from e in _ttcontext.HR_Master_Employee_DMO
//                                     from IVRM_School_Master_SubjectsDMO in _ttcontext.IVRM_School_Master_SubjectsDMO
//                                     from Adm_School_M_Academic_Year in _ttcontext.AcademicYear
//                                     where (TT_Consecutive.TTMC_Id.Equals(TT_Master_Category.TTMC_Id) && Adm_School_M_Class.ASMCL_Id.Equals(TT_Consecutive.ASMCL_Id) &&
//                                           Adm_School_M_Section.ASMS_Id.Equals(TT_Consecutive.ASMS_Id) && e.HRME_Id.Equals(TT_Consecutive.HRME_Id) &&
//                                           IVRM_School_Master_SubjectsDMO.ISMS_Id.Equals(TT_Consecutive.ISMS_Id) && Adm_School_M_Academic_Year.ASMAY_Id.Equals(TT_Consecutive.ASMAY_Id) && TT_Consecutive.MI_Id == page.MI_ID && Adm_School_M_Academic_Year.ASMAY_Id == page.ASMAY_Id && TT_Consecutive.TTC_ActiveFlag == true)
//                                     select new TT_ConstraintReportDTO
//                                     {
//                                         CategoryName = TT_Master_Category.TTMC_CategoryName,
//                                         NoOfPeriods = TT_Consecutive.TTC_NoOfPeriods,
//                                         ClassName = Adm_School_M_Class.ASMCL_ClassName,
//                                         SectionName = Adm_School_M_Section.ASMC_SectionName,
//                                         staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
//                                         SubjectName = IVRM_School_Master_SubjectsDMO.ISMS_SubjectName
//                                     }
//                               ).ToArray();
//          }

//          else if(page.constype == "bif")
//          {
//              page.bif_detailslist = (from m in _ttcontext.TT_Bifurcation_DMO
//                                   from q in _ttcontext.AcademicYear
//                                   from s in _ttcontext.TTMasterCategoryDMO
//                                   where (m.MI_Id == page.MI_ID && m.ASMAY_Id == page.ASMAY_Id && m.ASMAY_Id == q.ASMAY_Id && m.TTMC_Id == s.TTMC_Id && m.TTB_ActiveFlag == true)
//                                   select new TT_ConstraintReportDTO
//                                   {
//                                       categoryName1 = s.TTMC_CategoryName,
//                                       bifricationName = m.TTB_BifurcationName,
//                                       periodname = m.TTB_NoOfPeriods.ToString()

//                                   }).ToArray();
//          }

//          else if(page.constype == "lab")
//          {
//              page.labdetailsarray = (from TT_Master_Category in _ttcontext.TTMasterCategoryDMO
//                                      from Adm_School_M_Academic_Year in _ttcontext.AcademicYear
//                                      from TT_LABLIB in _ttcontext.TT_LABLIB_DMO
//                                      where (TT_LABLIB.MI_Id == page.MI_ID && TT_LABLIB.ASMAY_Id.Equals(Adm_School_M_Academic_Year.ASMAY_Id) && TT_Master_Category.TTMC_Id.Equals(TT_LABLIB.TTMC_Id) && TT_LABLIB.ASMAY_Id == page.ASMAY_Id && TT_LABLIB.TTLAB_ActiveFlag == true)
//                                      select new TT_ConstraintReportDTO
//                                      {
//                                          ASMAYYear = Adm_School_M_Academic_Year.ASMAY_Year,
//                                          CategoryName2 = TT_Master_Category.TTMC_CategoryName,
//                                          TTLAB_LABLIBName = TT_LABLIB.TTLAB_LABLIBName
//                                      }
//                                ).ToArray();
//          }










