using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Services
{
    public class NAAC_HSU_323_ResearchProjectsRatioImpl:Interface.NAAC_HSU_323_ResearchProjectsRatioInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;
        public NAAC_HSU_323_ResearchProjectsRatioImpl(DocumentsContext DocumentsContext, GeneralContext praa)
        {
            _DocumentsContext = DocumentsContext;
            _GeneralContext = praa;
        }
        public HSU_323_ResearchProjectsRatioDTO getdata(HSU_323_ResearchProjectsRatioDTO data)
        {
            try
            {
                var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();
                string NAACSL_InstitutionTypeFlg = "";
                List<long> miid = new List<long>();
                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);
                data.getinstitutioncycle = naaccomm.get_cycle_list(data.MI_Id, data.UserId);
                data.getinstitution = naaccomm.get_Institution_list(data.MI_Id, data.UserId);
                data.NAACSL_InstitutionTypeFlg = NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                data.yearlist = _DocumentsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<HSU_323_ResearchProjectsRatioDTO> get_323U_report(HSU_323_ResearchProjectsRatioDTO data)
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                List<long> yearid2 = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_323U_report";
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

                        data.reportlist2 = (from a in _GeneralContext.HSU_323_ResearchProjectsRatio_FilesDMO
                                            from b in _GeneralContext.HSU_323_ResearchProjectsRatioDMO
                                            where (mid.Contains(b.MI_Id) && b.NC323RPR_Id == a.NC323RPR_Id)
                                            select a).Distinct().ToArray();
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where (yearid2.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
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
        public async Task<HSU_323_ResearchProjectsRatioDTO> get_334U_report(HSU_323_ResearchProjectsRatioDTO data)
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                List<long> yearid2 = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_334U_report";
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

                        data.reportlist2 = (from a in _GeneralContext.HSU_334_CampusStartUps_FilesDMO
                                            from b in _GeneralContext.HSU_334_CampusStartUpsDMO
                                            where (mid.Contains(b.MI_Id) && b.NCHSU324CS_Id == a.NCHSU324CS_Id)
                                            select a).Distinct().ToArray();
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where (yearid2.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
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
        public async Task<HSU_323_ResearchProjectsRatioDTO> get_333U_report(HSU_323_ResearchProjectsRatioDTO data)
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);
                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                List<long> yearid2 = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_333U_report";
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

                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_Awards_342_Files_DMO
                                            from b in _GeneralContext.NAAC_AC_Awards_342_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCACAW342_Id == a.NCACAW342_Id)
                                            select a).Distinct().ToArray();
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where (yearid2.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
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
        public async Task<HSU_323_ResearchProjectsRatioDTO> get_344U_report(HSU_323_ResearchProjectsRatioDTO data)
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
               // List<long> yearid2 = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_344_REPORT";
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
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = "A"
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
         public async Task<HSU_323_ResearchProjectsRatioDTO> get_349U_report(HSU_323_ResearchProjectsRatioDTO data)
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                List<long> yearid2 = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_349HSU_REPORT";
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
                        data.reportlist2 = (from a in _GeneralContext.HSU_346_EmpApprovedJournalLists_Files_DMO
                                            from b in _GeneralContext.HSU_346_EMPApprovedJournalList_DMO
                                            where mid.Contains(b.MI_Id) && b.NCHSU346EAJL_Id == a.NCHSU346EAJL_Id
                                            select a).Distinct().ToArray();
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where (yearid2.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
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
          public async Task<HSU_323_ResearchProjectsRatioDTO> get_348U_report(HSU_323_ResearchProjectsRatioDTO data)
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                List<long> yearid2 = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_348HSU_REPORT";
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
                        data.reportlist2 = (from a in _GeneralContext.HSU_346_EmpApprovedJournalLists_Files_DMO
                                            from b in _GeneralContext.HSU_346_EMPApprovedJournalList_DMO
                                            where mid.Contains(b.MI_Id) && b.NCHSU346EAJL_Id == a.NCHSU346EAJL_Id
                                            select a).Distinct().ToArray();
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where (yearid2.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
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

      
    }
}
