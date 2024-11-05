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
    public class CLGStaffPeriodTransformImpl : Interfaces.CLGStaffPeriodTransformInterface
    {

        private static ConcurrentDictionary<string, CLGStaffPeriodTransformDTO> _login =
               new ConcurrentDictionary<string, CLGStaffPeriodTransformDTO>();


        public TTContext _ttcategorycontext;
        public CLGStaffPeriodTransformImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public CLGStaffPeriodTransformDTO getdetails(CLGStaffPeriodTransformDTO data)
        {
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true).OrderByDescending(u=>u.ASMAY_Order).ToList().ToArray();

                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                data.staffDrpDwn = (from b in _ttcategorycontext.HR_Master_Employee_DMO
                                    from a in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    from c in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from d in _ttcategorycontext.TT_Final_GenerationDMO
                                    where (b.MI_Id.Equals(data.MI_Id) //&& b.HRME_ActiveFlag.Equals(true)
                                    && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                    select new StaffReplacementUnalocatedPeriodDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        staffNamelst = b.HRME_EmployeeFirstName+" " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "  " || b.HRME_EmployeeMiddleName == "0" ? "  " : b.HRME_EmployeeMiddleName)+" " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "  " || b.HRME_EmployeeLastName == "0" ? "  " : b.HRME_EmployeeLastName)
                                    }).Distinct().OrderBy(r=>r.staffNamelst).ToArray();

                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        public CLGStaffPeriodTransformDTO get_catg(CLGStaffPeriodTransformDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new CLGStaffPeriodTransformDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public CLGStaffPeriodTransformDTO getreport(CLGStaffPeriodTransformDTO data)
        {
            try
            {
                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();


                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_STAFF_TT_FOR_REPLACEMENT";
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


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.NVarChar)
                    {
                        Value = data.HRME_Id
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


                data.courselist = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.MasterCourseDMO
                                    where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == c.AMCO_Id && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();

                data.branchlist = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.ClgMasterBranchDMO
                                    where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == c.AMB_Id  && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();

                data.semisterlist = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.CLG_Adm_Master_SemesterDMO
                                    where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == c.AMSE_Id && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();
                data.secdetails = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.Adm_College_Master_SectionDMO
                                    where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == c.ACMS_Id && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();

                data.subjectdet = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                    where a.MI_Id == c.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ISMS_Id == c.ISMS_Id  && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();





                data.staffDrpDwnto = (from b in _ttcategorycontext.HR_Master_Employee_DMO
                                    from a in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (b.MI_Id.Equals(data.MI_Id) && b.HRME_ActiveFlag.Equals(true) && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == a.HRME_Id && b.HRME_Id!=data.HRME_Id)
                                    select new StaffReplacementUnalocatedPeriodDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        staffNamelst = b.HRME_EmployeeFirstName+" " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "  " || b.HRME_EmployeeMiddleName == "0" ? "  " : b.HRME_EmployeeMiddleName)+" " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "  " || b.HRME_EmployeeLastName == "0" ? "   " : b.HRME_EmployeeLastName)
                                    }).Distinct().OrderBy(t=>t.staffNamelst).ToArray();

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }



            return data;

        }
        public CLGStaffPeriodTransformDTO gettimetable(CLGStaffPeriodTransformDTO data)
        {
            try
            {
                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                string coursidss = "0";
                string branchidss = "0";
                string semdss = "0";
                string secidss = "0";
                string subdss = "0";
                foreach (var item in data.AMCO_Idss)
                {
                    coursidss = coursidss + "," + item.AMCO_Id;
                }
                foreach (var item in data.AMB_Idss)
                {
                    branchidss = branchidss + "," + item.AMB_Id;
                }
                foreach (var item in data.AMSE_Idss)
                {
                    semdss = semdss + "," + item.AMSE_Id;
                }
                foreach (var item in data.ACMS_Idss)
                {
                    secidss = secidss + "," + item.ACMS_Id;
                }
                foreach (var item in data.ISMS_Idss)
                {
                    subdss = subdss + "," + item.ISMS_Id;
                }



                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_STAFF_TT_FOR_PERIOD_TRANSFORM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.Char)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.Char)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.Char)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCOIDs",
                      SqlDbType.Char)
                    {
                        Value = coursidss
                    });
                     cmd.Parameters.Add(new SqlParameter("@AMBIDs",
                      SqlDbType.Char)
                    {
                        Value = branchidss
                     });
                    cmd.Parameters.Add(new SqlParameter("@AMSEIDs",
                      SqlDbType.Char)
                    {
                        Value = semdss
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMSIDs",
                      SqlDbType.Char)
                    {
                        Value = secidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSIDs",
                      SqlDbType.Char)
                    {
                        Value = subdss
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

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }



            return data;

        }      
        public CLGStaffPeriodTransformDTO getpossiblePeriod(CLGStaffPeriodTransformDTO data)
        {
            var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(t=>t).FirstOrDefault();
                    
            List<CLGStaffPeriodTransformDTO> result = new List<CLGStaffPeriodTransformDTO>();

            using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "TT_Get_Allpossibilities_for_StaffTheir_TT_replacement";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = asmay_id });             
                cmd.Parameters.Add(new SqlParameter("@ttmd_id", SqlDbType.BigInt) { Value = data.TTMD_Id });
                cmd.Parameters.Add(new SqlParameter("@ttmp_id", SqlDbType.BigInt) { Value = data.TTMP_Id });
                cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
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
                            result.Add(new CLGStaffPeriodTransformDTO
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
        public CLGStaffPeriodTransformDTO savedetail(CLGStaffPeriodTransformDTO data)
        {
            try
            {

                if (data.periods.Length > 0)
                {


                    foreach (var item in data.periods)
                    {
                        var ressult = _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO.Single(f => f.TTFGDC_Id == item.TTFGDC_Id);
                        ressult.HRME_Id = data.HRME_IdTO;
                        ressult.UpdatedDate = DateTime.Now;

                        _ttcategorycontext.Update(ressult);
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
        public CLGStaffPeriodTransformDTO deleteperiod(CLGStaffPeriodTransformDTO data)
        {
            try
            {

                if (data.periods.Length > 0)
                {


                    foreach (var item in data.periods)
                    {
                        var ressult = _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO.Single(f => f.TTFGDC_Id == item.TTFGDC_Id);
                        _ttcategorycontext.Remove(ressult);
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
