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
    public class Naac_HSU_CR6ReportImpl:Interface.Naac_HSU_CR6ReportInterface
    {

        public GeneralContext _context;
        public DocumentsContext _DocumentsContext;
        public Naac_HSU_CR6ReportImpl(GeneralContext w, DocumentsContext o)
        {
            _context = w;
            _DocumentsContext = o;
        }
        public Naac_HSU_CR6Report_DTO loaddata(Naac_HSU_CR6Report_DTO data)
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
        public async Task<Naac_HSU_CR6Report_DTO> HSUEGovernance623Report(Naac_HSU_CR6Report_DTO data)
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
                    cmd.CommandText = "Naac_HSU_623_Report";
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
        public async Task<Naac_HSU_CR6Report_DTO> HSUFinancialSupport632Report(Naac_HSU_CR6Report_DTO data)
        {
            try
            {


                List<long> yearid1 = new List<long>();

                NAAC_CommonDetails naaccomm11 = new NAAC_CommonDetails(_context);

                yearid1 = naaccomm11.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);










                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }

                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm1 = new NAAC_CommonDetails(_context);

                yrid = naaccomm1.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                data.yearlist1 = (from a in _context.Academic
                                  where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                  select new Naac_HSU_CR6Report_DTO
                                  {
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToArray();
                string mi_ids = "0";
                List<long> mid1 = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid1.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid1.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid1[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_HSU_632_Report";
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
                        data.govtsclist = retObject.ToArray();
                        data.govtsclistfiles = (from a in _context.NAAC_AC_632_FinanceSupport_Files_DMO
                                            from b in _context.NAAC_AC_632_FinanceSupport_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCAC632FINSUP_Id == a.NCAC632FINSUP_Id)
                                            select a).Distinct().ToArray();
                        data.yearlist = (from a in _context.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new Naac_HSU_CR6Report_DTO
                                         {
                                             ASMAY_Year = a.ASMAY_Year,
                                         }).Distinct().ToArray();
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

        public async Task<Naac_HSU_CR6Report_DTO> HSUDevPrograms633Report(Naac_HSU_CR6Report_DTO data)
        {
            try
            {
                List<long> yearid1 = new List<long>();

                NAAC_CommonDetails naaccomm11 = new NAAC_CommonDetails(_context);

                yearid1 = naaccomm11.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }
                }

                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm1 = new NAAC_CommonDetails(_context);

                yrid = naaccomm1.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                data.yearlist1 = (from a in _context.Academic
                                  where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                  select new Naac_HSU_CR6Report_DTO
                                  {
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToArray();
                string mi_ids = "0";
                List<long> mid1 = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid1.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid1.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid1[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_HSU_633_Report";
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
                        data.govtsclist = retObject.ToArray();
                        data.govtsclistfiles = (from a in _context.NAAC_AC_634_DevPrograms_files_DMO
                                                from b in _context.NAAC_AC_634_DevPrograms_DMO
                                                where (mid.Contains(b.MI_Id) && b.NCAC634DEVPRG_Id == a.NCAC634DEVPRG_Id)
                                                select a).Distinct().ToArray();
                        data.yearlist = (from a in _context.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new Naac_HSU_CR6Report_DTO
                                         {
                                             ASMAY_Year = a.ASMAY_Year,
                                         }).Distinct().ToArray();
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








        public async Task<Naac_HSU_CR6Report_DTO> HSUGovtFunding642Report(Naac_HSU_CR6Report_DTO data)
        {
            try
            {


                List<long> yearid1 = new List<long>();

                NAAC_CommonDetails naaccomm11 = new NAAC_CommonDetails(_context);

                yearid1 = naaccomm11.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);










                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                }

                List<long> yrid = new List<long>();

                NAAC_CommonDetails naaccomm1 = new NAAC_CommonDetails(_context);

                yrid = naaccomm1.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                data.yearlist1 = (from a in _context.Academic
                                  where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                  select new Naac_HSU_CR6Report_DTO
                                  {
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToArray();
                string mi_ids = "0";
                List<long> mid1 = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid1.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid1.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid1[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_HSU_642_Report";
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
                        data.govtsclist = retObject.ToArray();
                        data.govtsclistfiles = (from a in _context.NAAC_AC_642_Funds_files_DMO
                                                from b in _context.NAAC_AC_642_Funds_DMO
                                                where (mid.Contains(b.MI_Id) && b.NCAC642FUND_Id == a.NCAC642FUND_Id)
                                                select a).Distinct().ToArray();
                        data.yearlist = (from a in _context.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new Naac_HSU_CR6Report_DTO
                                         {
                                             ASMAY_Year = a.ASMAY_Year,
                                         }).Distinct().ToArray();
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










        public async Task<Naac_HSU_CR6Report_DTO> HSUDevPrograms634Report(Naac_HSU_CR6Report_DTO data)
        {
            try
            {
                List<long> yearid1 = new List<long>();
                NAAC_CommonDetails naaccomm11 = new NAAC_CommonDetails(_context);
                yearid1 = naaccomm11.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }
                }
                List<long> yrid = new List<long>();
                NAAC_CommonDetails naaccomm1 = new NAAC_CommonDetails(_context);

                yrid = naaccomm1.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                data.yearlist1 = (from a in _context.Academic
                                  where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                  select new Naac_HSU_CR6Report_DTO
                                  {
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToArray();
                string mi_ids = "0";
                List<long> mid1 = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid1.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid1.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid1[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_HSU_634_Report";
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
                        data.govtsclist = retObject.ToArray();
                        data.govtsclistfiles = (from a in _context.NAAC_AC_634_DevPrograms_files_DMO
                                                from b in _context.NAAC_AC_634_DevPrograms_DMO
                                                where (mid.Contains(b.MI_Id) && b.NCAC634DEVPRG_Id == a.NCAC634DEVPRG_Id)
                                                select a).Distinct().ToArray();
                        data.yearlist = (from a in _context.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new Naac_HSU_CR6Report_DTO
                                         {
                                             ASMAY_Year = a.ASMAY_Year,
                                         }).Distinct().ToArray();
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






        public async Task<Naac_HSU_CR6Report_DTO> HSUQualityAssurance652Report(Naac_HSU_CR6Report_DTO data)
        {
            try
            {
                List<long> yearid1 = new List<long>();
                NAAC_CommonDetails naaccomm11 = new NAAC_CommonDetails(_context);
                yearid1 = naaccomm11.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                List<long> mid = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }
                }
                List<long> yrid = new List<long>();
                NAAC_CommonDetails naaccomm1 = new NAAC_CommonDetails(_context);

                yrid = naaccomm1.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                data.yearlist1 = (from a in _context.Academic
                                  where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                  select new Naac_HSU_CR6Report_DTO
                                  {
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToArray();
                string mi_ids = "0";
                List<long> mid1 = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid1.Add(item.MI_Id);
                    }
                    for (int i = 0; i < mid1.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid1[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_HSU_652_Report";
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
                        data.govtsclist = retObject.ToArray();
                        data.govtsclistfiles = (from a in _context.NAAC_AC_654_QualityAssurance_files_DMO
                                                from b in _context.NAAC_AC_654_QualityAssurance_DMO
                                                where (mid.Contains(b.MI_Id) && b.NCAC654QUAS_Id == a.NCAC654QUAS_Id)
                                                select a).Distinct().ToArray();
                        data.yearlist = (from a in _context.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new Naac_HSU_CR6Report_DTO
                                         {
                                             ASMAY_Year = a.ASMAY_Year,
                                         }).Distinct().ToArray();
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
