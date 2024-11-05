using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class Naac_MC_CR6Impl:Interface.Naac_MC_CR6Interface
    {
        public GeneralContext _context;
        public DocumentsContext _DocumentsContext;
        public Naac_MC_CR6Impl(GeneralContext w, DocumentsContext o)
        {
            _context = w;
            _DocumentsContext = o;
        }
        public Naac_MC_CR6_DTO loaddata(Naac_MC_CR6_DTO data)
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

                data.NAACSL_InstitutionTypeFlg = NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;

                data.yearlist = _DocumentsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<Naac_MC_CR6_DTO> MedFinancialSupport632Report(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_632_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_632_FinanceSupport_Files_DMO
                                            from b in _context.NAAC_AC_632_FinanceSupport_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC632FINSUP_Id == a.NCAC632FINSUP_Id)
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
        public async Task<Naac_MC_CR6_DTO> MedDevPrograms634634Report(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_634_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_634_DevPrograms_files_DMO
                                            from b in _context.NAAC_AC_634_DevPrograms_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC634DEVPRG_Id == a.NCAC634DEVPRG_Id)
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

        public async Task<Naac_MC_CR6_DTO> MedFunds643Report(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_643_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_642_Funds_files_DMO
                                            from b in _context.NAAC_AC_642_Funds_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC642FUND_Id == a.NCAC642FUND_Id)
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

        public async Task<Naac_MC_CR6_DTO> MedIQAC652Report(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_652_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_653_IQAC_files_DMO
                                            from b in _context.NAAC_AC_653_IQAC_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC653IQAC_Id == a.NCAC653IQAC_Id)
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

        public async Task<Naac_MC_CR6_DTO> MEDInternalQuality653(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_653_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_654_QualityAssurance_files_DMO
                                            from b in _context.NAAC_AC_654_QualityAssurance_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC654QUAS_Id == a.NCAC654QUAS_Id)
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
        public async Task<Naac_MC_CR6_DTO> MedEGovernance622Report(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_622_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_623_EGovernance_Files_DMO
                                            from b in _context.NAAC_AC_623_EGovernance_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC623EGOV_Id == a.NCAC623EGOV_Id)
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
        public async Task<Naac_MC_CR6_DTO> MedDevPrograms633Report(Naac_MC_CR6_DTO data)
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
                    // cmd.CommandText = "Naac_MED_Budget_414_report";
                    cmd.CommandText = "Naac_Medical_633_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                    {
                        Value = 1
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

                        data.reportlist2 = (from a in _context.NAAC_AC_634_DevPrograms_files_DMO
                                            from b in _context.NAAC_AC_634_DevPrograms_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC634DEVPRG_Id == a.NCAC634DEVPRG_Id)
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
