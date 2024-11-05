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
    public class CLGManualperiodinsertionImpl : Interfaces.CLGManualperiodinsertionInterface
    {

        private static ConcurrentDictionary<string, CLGManualperiodinsertionDTO> _login =
               new ConcurrentDictionary<string, CLGManualperiodinsertionDTO>();


        public TTContext _ttcategorycontext;
        public CLGManualperiodinsertionImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public CLGManualperiodinsertionDTO getdetails(CLGManualperiodinsertionDTO data)
        {
           
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true).OrderByDescending(r=>r.ASMAY_Order).ToList().ToArray();
                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                data.staffDrpDwn = (from f in _ttcategorycontext.HR_Master_Employee_DMO
                                    from TT_Master_Staff_AbbreviationDMO in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (f.MI_Id.Equals(data.MI_Id) && f.HRME_ActiveFlag.Equals(true) && TT_Master_Staff_AbbreviationDMO.HRME_Id == f.HRME_Id && TT_Master_Staff_AbbreviationDMO.MI_Id == data.MI_Id)
                                    select new TTStaffReplacementInUnallocatedPeriodDTO
                                    {
                                        HRME_Id = f.HRME_Id,
                                        staffNamelst = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == "  " || f.HRME_EmployeeMiddleName == "0" ? "  " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? "  " : f.HRME_EmployeeLastName),
                                    }
                                ).Distinct().OrderBy(f=> f.staffNamelst).ToArray();

              data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
               
    data.sectionlist = _ttcategorycontext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToList().Distinct().ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CLGManualperiodinsertionDTO get_catg(CLGManualperiodinsertionDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new CLGManualperiodinsertionDTO
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

        public CLGManualperiodinsertionDTO getclass_catg(CLGManualperiodinsertionDTO data)
        {
            try
         {
      //          data.classlist = (from a in _ttcategorycontext.TTMasterCategoryDMO
      //                            from b in _ttcategorycontext.TT_Category_Class_DMO
      //                            from c in _ttcategorycontext.School_M_Class
      //                            where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
      //                            select new CLGManualperiodinsertionDTO
      //                            {
      //                                ASMCL_Id = c.ASMCL_Id,
      //                                ASMCL_ClassName = c.ASMCL_ClassName,
      //                                TTMC_Id = a.TTMC_Id,
      //                                TTMC_CategoryName = a.TTMC_CategoryName,
      //                            }
      //).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public CLGManualperiodinsertionDTO getreport(CLGManualperiodinsertionDTO data)
        {
            try
            {

                data.periodslst = (from a in _ttcategorycontext.TT_Master_PeriodDMO
                                   from b in _ttcategorycontext.ClgPeriodAllocation_Course_DMO
                                   where a.TTMP_Id == b.TTMP_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id==data.AMB_Id && b.AMSE_Id==data.AMSE_Id && a.TTMP_ActiveFlag.Equals(true) && b.TTMPC_ActiveFlag == true
                                   select a

                                  ).Distinct().ToArray();

   

                data.gridweeks = (from a in _ttcategorycontext.TT_Master_DayDMO
                                   from b in _ttcategorycontext.CLGTT_Master_Day_CourseBranchDMO
                                   where a.TTMD_Id == b.TTMD_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && a.TTMD_ActiveFlag.Equals(true) && b.TTMDC_ActiveFlag == true
                                   select a


                               ).Distinct().ToArray();

                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_SECTION_TT_FOR_INSERTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCO_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.AMB_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.AMSE_Id
                    });



                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ACMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMC_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TTMC_Id
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
                        data.Time_Table = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.Break_list = (from a in _ttcategorycontext.CLGTT_Master_BreakDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.TTMBC_ActiveFlag == true && a.TTMC_Id == data.TTMC_Id)
                                   select new CLGManualperiodinsertionDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMB_Id = a.AMB_Id,
                                       AMSE_Id = a.AMSE_Id,
                                       TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                                       TTMBC_BreakName = a.TTMBC_BreakName
                                   }).Distinct().OrderBy(x => x.TTMBC_AfterPeriod).ToArray();

                data.Break_list_all = _ttcategorycontext.CLGTT_Master_BreakDMO.AsNoTracking().Where(b => b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == data.TTMC_Id && b.MI_Id == data.MI_Id && b.TTMBC_ActiveFlag == true && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id).OrderBy(x => x.TTMBC_AfterPeriod).ToList().ToArray();


                data.subjectlist = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }     

        public CLGManualperiodinsertionDTO getpossiblePeriod(CLGManualperiodinsertionDTO data)
        {
          //  var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(e=>e).FirstOrDefault();

          ////  asmay_id = 39;
          //  List<CLGManualperiodinsertionDTO> result = new List<CLGManualperiodinsertionDTO>();
          //  long category_id = _ttcategorycontext.TT_Category_Class_DMO.Single(t => t.MI_Id.Equals(data.MI_Id) && t.ASMAY_Id.Equals(asmay_id) && t.ASMCL_Id.Equals(data.ASMCL_Id)).TTMC_Id;
            

          //  using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
          //  {
          //      cmd.CommandText = "TT_Get_Allpossibilities_for_replacement";
          //      cmd.CommandType = CommandType.StoredProcedure;
          //      cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
          //      cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = asmay_id });
          //      cmd.Parameters.Add(new SqlParameter("@ttmc_id", SqlDbType.BigInt) { Value = category_id });
          //      cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
          //      cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.BigInt) { Value = data.ASMS_Id });
          //      cmd.Parameters.Add(new SqlParameter("@ttmd_id", SqlDbType.BigInt) { Value = data.TTMD_Id });
          //      cmd.Parameters.Add(new SqlParameter("@ttmp_id", SqlDbType.BigInt) { Value = data.TTMP_Id });
          //      cmd.Parameters.Add(new SqlParameter("@staffwiseflag", SqlDbType.BigInt) { Value = data.staffSDK });
          //      cmd.Parameters.Add(new SqlParameter("@subjectwiseflag", SqlDbType.BigInt) { Value = data.subSDK });
          //      cmd.Parameters.Add(new SqlParameter("@Consecutivewiseflag", SqlDbType.BigInt) { Value = data.conSDK });

          //      if (cmd.Connection.State != ConnectionState.Open)
          //          cmd.Connection.Open();

          //      var retObject = new List<dynamic>();
          //      try
          //      {
          //          using (var dataReader = cmd.ExecuteReader())
          //          {
          //              while (dataReader.Read())
          //              {
          //                  result.Add(new CLGManualperiodinsertionDTO
          //                  {
          //                      TTMD_Id = Convert.ToInt64(dataReader["TTMD_Id"].ToString()),
          //                      TTMP_Id = Convert.ToInt32(dataReader["TTMP_Id"].ToString()),   
          //                  });
          //                  data.Data_lst = result.ToArray();
          //              }
          //          }
          //      }
          //      catch (Exception ex)
          //      {
          //          Console.Write(ex.Message);
          //      }
          //  }

            return data;
        }

        public CLGManualperiodinsertionDTO savedetail(CLGManualperiodinsertionDTO data)
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

                if (data.periods.Length > 0)
                {
                    foreach (var item in data.periods)
                    {
                        CLGTT_Final_Generation_DetailedDMO obj = new CLGTT_Final_Generation_DetailedDMO();

                        obj.TTFG_Id = TTFG_Ids;
                        obj.AMCO_Id = data.AMCO_Id;
                        obj.AMB_Id = data.AMB_Id;
                        obj.AMSE_Id = data.AMSE_Id;
                        obj.ACMS_Id = data.ACMS_Id;
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
            catch (Exception ee)
            {
                 Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}

