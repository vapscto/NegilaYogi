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
    public class CLGStaffRplInTheirTTImpl : Interfaces.CLGStaffRplInTheirTTInterface
    {

        private static ConcurrentDictionary<string, CLGStaffRplInTheirTTDTO> _login =
               new ConcurrentDictionary<string, CLGStaffRplInTheirTTDTO>();


        public TTContext _ttcategorycontext;
        public CLGStaffRplInTheirTTImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public CLGStaffRplInTheirTTDTO getdetails(CLGStaffRplInTheirTTDTO data)
        {
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id==data.MI_Id && t.Is_Active == true).OrderByDescending(u=>u.ASMAY_Order).ToList().ToArray();

                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();


                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag == true && c.MI_Id == data.MI_Id).ToList().ToArray();

                data.staffDrpDwn = (from b in _ttcategorycontext.HR_Master_Employee_DMO
                                    from a in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    from c in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from d in _ttcategorycontext.TT_Final_GenerationDMO
                                    where (b.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                    select new CLGStaffRplInTheirTTDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        staffNamelst = b.HRME_EmployeeFirstName +" "+ (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) +" "+ (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName)
                                    }).Distinct().OrderBy(r=>r.staffNamelst).ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        public CLGStaffRplInTheirTTDTO get_catg(CLGStaffRplInTheirTTDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new CLGStaffRplInTheirTTDTO
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


        public CLGStaffRplInTheirTTDTO getreport(CLGStaffRplInTheirTTDTO data)
        {
            try
            {
                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag==true && c.MI_Id==data.MI_Id).ToList().ToArray();

                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id==data.MI_Id && t.TTMD_ActiveFlag == true).ToArray();


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



            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }      
        public CLGStaffRplInTheirTTDTO getpossiblePeriod(CLGStaffRplInTheirTTDTO data)
        {     
            List<CLGStaffRplInTheirTTDTO> result = new List<CLGStaffRplInTheirTTDTO>();

            using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "TT_CLG_GET_ALLPOSSIBILITIES_FOR_STAFFTHEIR_TT_REPLACEMENT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = data.ASMAY_Id }); 
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
                            result.Add(new CLGStaffRplInTheirTTDTO
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
        public CLGStaffRplInTheirTTDTO savedetail(CLGStaffRplInTheirTTDTO data)
        {
            try
            {
                if (data.TTMD_ID_from > 0 && data.TTMP_ID_from > 0 && data.TTMD_ID_to > 0 && data.TTMP_ID_to > 0)
                {

                    var cls_sect = (from a in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                    from b in _ttcategorycontext.TT_Final_GenerationDMO
                                    where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_from && a.TTMP_Id == data.TTMP_ID_from && a.HRME_Id == data.HRME_Id && b.ASMAY_Id==data.ASMAY_Id)
                                    select new CLGStaffRplInTheirTTDTO
                                    {
                                        AMCO_Id = a.AMCO_Id,
                                        AMB_Id = a.AMB_Id,
                                        AMSE_Id = a.AMSE_Id,
                                        ACMS_Id = a.ACMS_Id,
                                        TTMC_Id = b.TTMC_Id
                                    }).Distinct().ToList();
                    if (cls_sect.Count>0)
                    {
                        foreach (var item in cls_sect)
                        {
                            var Primary_ID1 = (from a in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                                from b in _ttcategorycontext.TT_Final_GenerationDMO
                                                where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.AMCO_Id == item.AMCO_Id && a.AMB_Id == item.AMB_Id && a.AMSE_Id == item.AMSE_Id && a.ACMS_Id == item.ACMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_from && a.TTMP_Id == data.TTMP_ID_from && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == item.TTMC_Id)
                                                select a).Distinct().ToList();



                            var Primary_ID2 = (from a in _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO
                                                from b in _ttcategorycontext.TT_Final_GenerationDMO
                                                where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true &&  a.AMCO_Id == item.AMCO_Id && a.AMB_Id == item.AMB_Id && a.AMSE_Id == item.AMSE_Id && a.ACMS_Id == item.ACMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_to && a.TTMP_Id == data.TTMP_ID_to && b.ASMAY_Id ==data.ASMAY_Id && b.TTMC_Id == item.TTMC_Id)
                                                select a).Distinct().ToList();

                            if (Primary_ID1.Count > 0 && Primary_ID2.Count > 0)
                            {
                                foreach (var item3 in Primary_ID1)
                                {
                                    var First_value = _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO.Single(o => o.TTFGDC_Id.Equals(item3.TTFGDC_Id));
                                    First_value.TTMD_Id = data.TTMD_ID_to;
                                    First_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_to);
                                    _ttcategorycontext.Update(First_value);
                                }

                                foreach (var item4 in Primary_ID2)
                                {
                                    var Second_value = _ttcategorycontext.CLGTT_Final_Generation_DetailedDMO.Single(o => o.TTFGDC_Id.Equals(item4.TTFGDC_Id));
                                    Second_value.TTMD_Id = data.TTMD_ID_from;
                                    Second_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_from);
                                    _ttcategorycontext.Update(Second_value);
                                }

                            }



                        }
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
