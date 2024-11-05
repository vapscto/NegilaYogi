using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Services
{
    public class NaacCriteria4ReportImpl : Interface.NaacCriteria4ReportInterface
    {
        public GeneralContext _context;
        public DocumentsContext _DocumentsContext;
        public NaacCriteria4ReportImpl(GeneralContext w, DocumentsContext o)
        {
            _context = w;
            _DocumentsContext = o;
        }
        public NaacCriteria4ReportDTO loaddata(NaacCriteria4ReportDTO data)
        {
            try
            {
                var getinstitution = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                string NAACSL_InstitutionTypeFlg = "";
                List<long> miid = new List<long>();
                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                data.getinstitutioncycle = naaccomm.get_cycle_list(data.MI_Id, data.UserId);

                data.getinstitution = naaccomm.get_Institution_list(data.MI_Id, data.UserId);

                #region
                //if (NAACSL_InstitutionTypeFlg.ToUpper() == "UNIVERSITY")
                //{
                //    data.naactype = "University";
                //    var getorganization = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).Distinct().ToList();

                //    data.MO_Id = getorganization.FirstOrDefault().MO_Id;
                //    //data.getinstitutioncycle = _GeneralContext.NAAC_Master_Trust_CycleDMO.Where(a => a.MO_Id == data.MO_Id).Distinct().OrderByDescending(a => a.NCMATC_Order).ToArray();

                //    data.getinstitutioncycle = (from a in _GeneralContext.NAAC_Master_Trust_CycleDMO
                //                                where (a.MO_Id == data.MO_Id && a.NCMATC_ActiveFlg == true)
                //                                select new CurricularAspects_DTO
                //                                {
                //                                    cycleid = a.NCMATC_Id,
                //                                    cyclename = a.NCMATC_NAACCycle,
                //                                    cycleorder = a.NCMATC_Order
                //                                }).Distinct().OrderByDescending(a => a.cycleorder).ToArray();

                //}
                //else
                //{
                //    data.naactype = "Others";

                //    var getmiid = (from a in _GeneralContext.NAAC_User_PrivilegeDMO
                //                   from b in _GeneralContext.NAAC_User_Privilege_InstitutionDMO
                //                   where (a.NAACUPRI_Id == b.NAACUPRI_Id && a.IVRMUL_Id == data.UserId && a.NAACUPRI_ActiveFlag == true && b.NAACUPRIIN_ActiveFlag == true)
                //                   select new CurricularAspects_DTO
                //                   {
                //                       MI_Id = b.MI_Id
                //                   }).Distinct().ToList();

                //    foreach (var c in getmiid)
                //    {
                //        miid.Add(c.MI_Id);
                //    }

                //    data.getinstitutioncycle = (from a in _GeneralContext.NAAC_Master_CycleDMO
                //                                where (a.MI_Id == data.MI_Id && a.NCMACY_ActiveFlg == true)
                //                                select new CurricularAspects_DTO
                //                                {
                //                                    cycleid = a.NCMACY_Id,
                //                                    cyclename = a.NCMACY_NAACCycle,
                //                                    cycleorder = a.NCMACY_Order
                //                                }).Distinct().OrderByDescending(a => a.cycleorder).ToArray();
                //}   

                #endregion

                data.yearlist = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NaacCriteria4ReportDTO> Report(NaacCriteria4ReportDTO data)
        {
            try
            {
                string mi_ids = "0";
                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NaacExpenditure_424_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = yearid
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.reportlist = retObject.ToArray();

                        data.reportlist2 = (from a in _context.NAAC_AC_424_Expenditure_Files_DMO
                                            from b in _context.NAAC_AC_424_Expenditure_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC424EXP_Id == a.NCAC424EXP_Id)
                                            select a).Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NaacCriteria4ReportDTO> ExpAcaReport(NaacCriteria4ReportDTO data)
        {
            try
            {
                string mi_ids = "0";
                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NaacExpAcaFacility_441_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = yearid
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.reportlist = retObject.ToArray();

                        data.reportlist2 = (from a in _context.NAAC_AC_441_ExpAcaFacility_Files_DMO
                                            from b in _context.NAAC_AC_441_ExpAcaFacility_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC441ExAcFc_Id == a.NCAC441ExAcFc_Id)
                                            select a).Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacCriteria4ReportDTO ReportCriteria4(NaacCriteria4ReportDTO data)
        {
            try
            {
                data.alldata5 = _context.NAAC_AC_434_EContent_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

                data.alldata434 = (from a in _context.NAAC_AC_434_EContent_Files_DMO
                                   from b in _context.NAAC_AC_434_EContent_DMO
                                   where (b.MI_Id == data.MI_Id && a.NCAC434ECT_Id == b.NCAC434ECT_Id)
                                   select a).Distinct().ToArray();

                data.alldata8 = (from a in _context.NAAC_AC_423_Memberships_DMO
                                 where (a.MI_Id == data.MI_Id)
                                 select new NaacCriteria4ReportDTO
                                 {
                                     NCAC423MEM_Membership = a.NCAC423MEM_Membership,
                                     NCAC423MEM_Subscription = a.NCAC423MEM_Subscription,
                                     NCAC423MEM_NoOfEResources = a.NCAC423MEM_NoOfEResources,
                                     NCAC423MEM_ValidityPeriod = a.NCAC423MEM_ValidityPeriod,
                                     NCAC423MEM_UsageReport = a.NCAC423MEM_UsageReport,
                                     NCAC423MEM_RemoteAccessFlg = a.NCAC423MEM_RemoteAccessFlg,
                                     NCAC423MEM_Id = a.NCAC423MEM_Id,
                                 }).Distinct().ToArray();
                data.alldata82 = (from a in _context.NAAC_AC_423_Memberships_DMO
                                  from b in _context.NAAC_AC_423_Memberships_Files_DMO
                                  where (a.MI_Id == data.MI_Id && a.NCAC423MEM_Id == b.NCAC423MEM_Id)
                                  select b).Distinct().ToArray();

                data.alldata7 = _context.NAAC_AC_413_ICT_DMO.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.alldata72 = (from a in _context.NAAC_AC_413_ICT_DMO
                                  from b in _context.NAAC_AC_413_ICT_FilesDMO
                                  where (a.MI_Id == data.MI_Id && a.NCAC413ICT_Id == b.NCAC413ICT_Id)
                                  select b
                                 ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
       

    }
}
