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
    public class TTStaffReplacementInUnallocatedPeriodImpl : Interfaces.StaffReplacementInUnallocatedPeriodInterface
    {

        private static ConcurrentDictionary<string, TTStaffReplacementInUnallocatedPeriodDTO> _login =
               new ConcurrentDictionary<string, TTStaffReplacementInUnallocatedPeriodDTO>();


        public TTContext _ttcategorycontext;
        public TTStaffReplacementInUnallocatedPeriodImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public TTStaffReplacementInUnallocatedPeriodDTO getdetails(int id)
        {
            TTStaffReplacementInUnallocatedPeriodDTO TTMC = new TTStaffReplacementInUnallocatedPeriodDTO();
            try
            {
                TTMC.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active == true).OrderByDescending(o=>o.ASMAY_Order).ToList().ToArray();

                TTMC.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();
                TTMC.staffDrpDwn = (from b in _ttcategorycontext.HR_Master_Employee_DMO
                                    from a in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    from c in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                    from d in _ttcategorycontext.TT_Final_GenerationDMO
                                    where (b.MI_Id.Equals(id) && b.HRME_ActiveFlag.Equals(true) && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                    select new StaffReplacementUnalocatedPeriodDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        staffNamelst = b.HRME_EmployeeFirstName + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName)
                                    }).Distinct().ToArray();

                TTMC.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                TTMC.returnval = false;
            }
            return TTMC;

        }

        public TTStaffReplacementInUnallocatedPeriodDTO get_catg(TTStaffReplacementInUnallocatedPeriodDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new TTStaffReplacementInUnallocatedPeriodDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public TTStaffReplacementInUnallocatedPeriodDTO getreport(TTStaffReplacementInUnallocatedPeriodDTO data)
        {
            try
            {
                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
                var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(y=>y).FirstOrDefault();

                data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                   from b in _ttcategorycontext.TT_Master_PeriodDMO
                                   from c in _ttcategorycontext.School_M_Class
                                   from d in _ttcategorycontext.School_M_Section
                                   from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                   from f in _ttcategorycontext.HR_Master_Employee_DMO
                                   from g in _ttcategorycontext.TT_Final_GenerationDMO
                                   from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                   from i in _ttcategorycontext.TTMasterCategoryDMO
                                   where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == asmay_id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id)
                                   // && g.TTMC_Id==data.TTMC_Id 
                                   select new TTStaffReplacementInUnallocatedPeriodDTO
                                   {
                                       TTFGD_Id = h.TTFGD_Id,
                                       TTFG_Id = g.TTFG_Id,
                                       ASMCL_Id = h.ASMCL_Id,
                                       ASMS_Id = h.ASMS_Id,
                                       HRME_Id = h.HRME_Id,
                                       ISMS_Id = h.ISMS_Id,
                                       TTMD_Id = h.TTMD_Id,
                                       TTMP_Id = h.TTMP_Id,
                                       TTMC_Id = g.TTMC_Id,
                                       TTMD_DayName = a.TTMD_DayName,
                                       TTMP_PeriodName = b.TTMP_PeriodName,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       ISMS_SubjectName = e.ISMS_SubjectName,
                                       staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                       TTMC_CategoryName = i.TTMC_CategoryName,

                                   }
                              ).Distinct().ToArray();





            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTStaffReplacementInUnallocatedPeriodDTO savedetail(TTStaffReplacementInUnallocatedPeriodDTO data)
        {
            try
            {

                if (data.TTMD_ID_from > 0 && data.TTMP_ID_from > 0 && data.TTMD_ID_to > 0 && data.TTMP_ID_to > 0)
                {
                    var cls_sect = (from a in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                    from b in _ttcategorycontext.TT_Final_GenerationDMO
                                    where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_from && a.TTMP_Id == data.TTMP_ID_from && a.HRME_Id == data.HRME_Id)
                                    select new TTStaffReplacementInUnallocatedPeriodDTO
                                    {
                                        ASMCL_Id = a.ASMCL_Id,
                                        ASMS_Id = a.ASMS_Id,
                                        ASMAY_ID = b.ASMAY_Id,
                                        TTMC_Id = b.TTMC_Id
                                    }).ToList();


                    var count = (from a in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                 from b in _ttcategorycontext.TT_Final_GenerationDMO
                                 where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.ASMCL_Id == cls_sect[0].ASMCL_Id && a.ASMS_Id == cls_sect[0].ASMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_to && a.TTMP_Id == data.TTMP_ID_to && b.ASMAY_Id== cls_sect[0].ASMAY_ID && b.TTMC_Id == cls_sect[0].TTMC_Id)
                                 select new TTStaffReplacementInUnallocatedPeriodDTO { }).ToList().Count();
                    if (count > 0)
                    {
                        data.returnval_cls_notfree = true;
                    }
                    else
                    {
                        List<TTStaffReplacementInUnallocatedPeriodDTO> result = new List<TTStaffReplacementInUnallocatedPeriodDTO>();
                        using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "TT_Get_Allpossibilities_for_Unaloted_replacement";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = cls_sect[0].ASMAY_ID });
                            cmd.Parameters.Add(new SqlParameter("@ttmc_id", SqlDbType.BigInt) { Value = cls_sect[0].TTMC_Id });
                            cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.BigInt) { Value = cls_sect[0].ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.BigInt) { Value = cls_sect[0].ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@ttmd_id_from", SqlDbType.BigInt) { Value = data.TTMD_ID_from });
                            cmd.Parameters.Add(new SqlParameter("@ttmp_id_from", SqlDbType.BigInt) { Value = data.TTMP_ID_from });
                            cmd.Parameters.Add(new SqlParameter("@ttmd_id_to", SqlDbType.BigInt) { Value = data.TTMD_ID_to });
                            cmd.Parameters.Add(new SqlParameter("@ttmp_id_to", SqlDbType.BigInt) { Value = data.TTMP_ID_to });
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
                                        result.Add(new TTStaffReplacementInUnallocatedPeriodDTO
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

                        if (data.Data_lst.Length > 0)
                        {
                            if (result[0].TTMP_Id == 0 && result[0].TTMD_Id == 0)
                            {
                                data.returnval_cls_notfree = true;
                            }
                            else
                            {
                                long Primary_ID1 = (from a in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                                    from b in _ttcategorycontext.TT_Final_GenerationDMO
                                                    where (a.TTFG_Id == b.TTFG_Id && b.TTFG_ActiveFlag == true && a.ASMCL_Id == cls_sect[0].ASMCL_Id && a.ASMS_Id == cls_sect[0].ASMS_Id && b.MI_Id == data.MI_Id && a.TTMD_Id == data.TTMD_ID_from && a.TTMP_Id == data.TTMP_ID_from && b.ASMAY_Id == cls_sect[0].ASMAY_ID && b.TTMC_Id == cls_sect[0].TTMC_Id)
                                                    select new { a.TTFGD_Id }).Distinct().Select(r => r.TTFGD_Id).FirstOrDefault();
                                var First_value = _ttcategorycontext.TT_Final_Generation_DetailedDMO.Single(o => o.TTFGD_Id.Equals(Primary_ID1));

                                First_value.TTMD_Id = data.TTMD_ID_to;
                                First_value.TTMP_Id = Convert.ToInt32(data.TTMP_ID_to);
                                _ttcategorycontext.Update(First_value);
                                var contactExists = _ttcategorycontext.SaveChanges();
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
