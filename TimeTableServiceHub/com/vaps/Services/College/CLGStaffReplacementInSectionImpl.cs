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
    public class CLGStaffReplacementInSectionImpl : Interfaces.CLGStaffReplacementInSectionInterface
    {

        private static ConcurrentDictionary<string, CLGStaffReplacementInSectionDTO> _login =
               new ConcurrentDictionary<string, CLGStaffReplacementInSectionDTO>();


        public TTContext _ttcategorycontext;
        public CLGStaffReplacementInSectionImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public CLGStaffReplacementInSectionDTO getdetails(CLGStaffReplacementInSectionDTO data)
        {
           
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true).OrderByDescending(r=>r.ASMAY_Order).ToList().ToArray();
                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();
                data.academiclist = _ttcategorycontext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.staffDrpDwn = (from f in _ttcategorycontext.HR_Master_Employee_DMO
                                    from TT_Master_Staff_AbbreviationDMO in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (f.MI_Id.Equals(data.MI_Id) && f.HRME_ActiveFlag.Equals(true) && TT_Master_Staff_AbbreviationDMO.HRME_Id == f.HRME_Id && TT_Master_Staff_AbbreviationDMO.MI_Id == data.MI_Id)
                                    select new TTStaffReplacementInUnallocatedPeriodDTO
                                    {
                                        HRME_Id = f.HRME_Id,
                                        staffNamelst = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                    }
                                ).ToArray();

              data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
                data.sectionlist = _ttcategorycontext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();
            

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CLGStaffReplacementInSectionDTO get_catg(CLGStaffReplacementInSectionDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new CLGStaffReplacementInSectionDTO
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

        public CLGStaffReplacementInSectionDTO getclass_catg(CLGStaffReplacementInSectionDTO data)
        {
      //      try
      //      {
      //          data.classlist = (from a in _ttcategorycontext.TTMasterCategoryDMO
      //                            from b in _ttcategorycontext.TT_Category_Class_DMO
      //                            from c in _ttcategorycontext.School_M_Class
      //                            where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
      //                            select new CLGStaffReplacementInSectionDTO
      //                            {
      //                                ASMCL_Id = c.ASMCL_Id,
      //                                ASMCL_ClassName = c.ASMCL_ClassName,
      //                                TTMC_Id = a.TTMC_Id,
      //                                TTMC_CategoryName = a.TTMC_CategoryName,
      //                            }
      //).Distinct().ToArray();


      //      }
      //      catch (Exception ee)
      //      {
      //          data.returnval = false;
      //          Console.WriteLine(ee.Message);
      //      }
            return data;

        }


        public CLGStaffReplacementInSectionDTO getreport(CLGStaffReplacementInSectionDTO data)
        {
            try
            {
      //data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.periodslst = (from a in _ttcategorycontext.TT_Master_PeriodDMO
                                   from b in _ttcategorycontext.ClgPeriodAllocation_Course_DMO
                                   where a.MI_Id == data.MI_Id && a.TTMP_Id == b.TTMP_Id && a.TTMP_ActiveFlag == true && b.TTMPC_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                   select a).Distinct().ToArray();


                data.gridweeks = (from a in _ttcategorycontext.TT_Master_DayDMO
                                  from b in _ttcategorycontext.CLGTT_Master_Day_CourseBranchDMO
                                   where a.MI_Id == data.MI_Id && a.TTMD_Id == b.TTMD_Id && a.TTMD_ActiveFlag == true && b.TTMDC_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                   select a).Distinct().ToArray();

              //  data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();


                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_SECTION_TT_FOR_REPLACEMENT";
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
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.TTMBC_ActiveFlag == true && a.TTMC_Id == data.TTMC_Id && a.AMSE_Id==data.AMSE_Id)
                                   select new CLGStaffReplacementInSectionDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMB_Id = a.AMB_Id,
                                       AMSE_Id = a.AMSE_Id,
                                       TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                                       TTMBC_BreakName = a.TTMBC_BreakName
                                   }).Distinct().OrderBy(x => x.TTMBC_AfterPeriod).ToArray();

 data.Break_list_all = _ttcategorycontext.CLGTT_Master_BreakDMO.AsNoTracking().Where(b => b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == data.TTMC_Id && b.MI_Id == data.MI_Id && b.TTMBC_ActiveFlag == true && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id).OrderBy(x => x.TTMBC_AfterPeriod).ToList().ToArray();

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }     

        public CLGStaffReplacementInSectionDTO getpossiblePeriod(CLGStaffReplacementInSectionDTO data)
        {
            
            List<CLGStaffReplacementInSectionDTO> result = new List<CLGStaffReplacementInSectionDTO>();


            using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "TT_CLG_GET_ALLPOSSIBILITIES_FOR_REPLACEMENT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@ttmc_id", SqlDbType.BigInt) { Value = data.TTMC_Id });
                cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.BigInt) { Value = data.AMCO_Id });
                cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.BigInt) { Value = data.AMB_Id });
                cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.BigInt) { Value = data.AMSE_Id });
                cmd.Parameters.Add(new SqlParameter("@acms_id", SqlDbType.BigInt) { Value = data.ACMS_Id });
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
                            result.Add(new CLGStaffReplacementInSectionDTO
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

        public CLGStaffReplacementInSectionDTO savedetail(CLGStaffReplacementInSectionDTO data)
        {
            try
            {
              
                if (data.TTMD_ID_from > 0 && data.TTMP_ID_from > 0 && data.TTMD_ID_to > 0 && data.TTMP_ID_to > 0)
                {


                    var Primary_ID1 = (from a in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                       from b in _ttcategorycontext.TT_Final_GenerationDMO
                                       where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_from && a.TTMP_Id == data.TTMP_ID_from && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == data.TTMC_Id)
                                       select a).Distinct().ToList();

                    var Primary_ID2 = (from a in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                        from b in _ttcategorycontext.TT_Final_GenerationDMO
                                        where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_to && a.TTMP_Id == data.TTMP_ID_to && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == data.TTMC_Id)
                                       select a).Distinct().ToList();

                    if (Primary_ID1.Count>0 && Primary_ID2.Count >0)
                    {
                        foreach (var item in Primary_ID1)
                        {
                            var First_value = _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO.Single(o => o.TTFGDC_Id.Equals(item.TTFGDC_Id));
                            First_value.TTMD_Id = data.TTMD_ID_to;
                            First_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_to);
                            _ttcategorycontext.Update(First_value);
                        }


                        foreach (var item1 in Primary_ID2)
                        {
                            var Second_value = _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO.Single(o => o.TTFGDC_Id.Equals(item1.TTFGDC_Id));
                            Second_value.TTMD_Id = data.TTMD_ID_from;
                            Second_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_from);
                            _ttcategorycontext.Update(Second_value);
                        }


                        var contactExists = _ttcategorycontext.SaveChanges();
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}

