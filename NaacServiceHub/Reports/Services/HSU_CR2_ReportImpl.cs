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
    public class HSU_CR2_ReportImpl : Interface.HSU_CR2_ReportInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;
        public HSU_CR2_ReportImpl(DocumentsContext para1, GeneralContext para2)
        {
            _DocumentsContext = para1;
            _GeneralContext = para2;
        }

        public HSU_CR2_Report_DTO getdata(HSU_CR2_Report_DTO data)
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
        public async Task<HSU_CR2_Report_DTO> HSU_211_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_112_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_212_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_212_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_213_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_213_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();
                        List<long> asmy_ids = new List<long>();

                        string NAACSL_InstitutionTypeFlg = "";
                        var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                        if (getinstitution.Count() > 0)
                        {
                            NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                        }

                        List<Criteria2_DTO> dto = new List<Criteria2_DTO>();

                        if (NAACSL_InstitutionTypeFlg.ToUpper() == "UNIVERSITY")
                        {

                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   from c in _GeneralContext.NAAC_Master_Trust_CycleDMO
                                   from d in _GeneralContext.NAAC_Master_Trust_Cycle_MappingDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && c.NCMATC_Id == d.NCMATC_Id && d.NCMACY_Id == a.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && c.NCMATC_ActiveFlg == true && c.NCMATC_Id == data.cycleid)
                                   select new Criteria2_DTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id
                                   }).Distinct().ToList();
                        }
                        else
                        {
                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && a.NCMACY_Id == data.cycleid)
                                   select new Criteria2_DTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id
                                   }).Distinct().ToList();
                        }

                        List<long> asmay_ids = new List<long>();

                        if (dto.Count > 0)
                        {
                            foreach (var item in dto)
                            {
                                asmay_ids.Add(item.ASMAY_Id);
                            }
                        }
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where (asmay_ids.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
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
        public async Task<HSU_CR2_Report_DTO> HSU_221_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_221_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_222_Report(HSU_CR2_Report_DTO data)
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

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_222_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_232_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_232_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_234_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_234_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_241_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_241_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_242_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_242_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_243_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_243_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_244_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_244_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();
                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);
                        data.yearlist1 = (from a in _GeneralContext.Academic
                                          where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
                                          select a).Distinct().ToArray();
                        data.reportlist2 = (from a in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_DMO
                                            from b in _GeneralContext.NAAC_MC_EmpTrainedDevelopment244_files_DMO
                                            where (asmay_ids.Contains(a.NCMCETD244_Year) 
                                            && midss.Contains(a.MI_Id) && a.NCMCETD244_Id==b.NCMCETD244_Id)
                                            select b).Distinct().ToArray();

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
        public async Task<HSU_CR2_Report_DTO> HSU_245_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_245_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_251_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_251_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.yearlist1 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        //public async Task<HSU_CR2_Report_DTO> HSU_252_Report(HSU_CR2_Report_DTO data)
        //{
        //    try
        //    {
        //        string mi_ids = "0";
        //        List<long> mid = new List<long>();

        //        if (data.selected_Inst.Length > 0)
        //        {
        //            foreach (var item in data.selected_Inst)
        //            {
        //                mid.Add(item.MI_Id);
        //            }
        //            for (int i = 0; i < mid.Count; i++)
        //            {
        //                mi_ids = mi_ids + "," + mid[i].ToString();
        //            }
        //        }
        //        string yearid = "";
        //        NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);
        //        yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);


        //        using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "NAAC_HSU_252_Report";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //            SqlDbType.VarChar)
        //            {
        //                Value = mi_ids
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //           SqlDbType.VarChar)
        //            {
        //                Value = yearid
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@FLAG",
        //           SqlDbType.VarChar)
        //            {
        //                Value = "A"
        //            });
        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();
        //            var retObject = new List<dynamic>();
        //            try
        //            {
        //                using (var dataReader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await dataReader.ReadAsync())
        //                    {
        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                        {
        //                            dataRow.Add(
        //                                dataReader.GetName(iFiled),
        //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                            );
        //                        }
        //                        retObject.Add((ExpandoObject)dataRow);
        //                    }
        //                }

        //                //var listofdata = retObject.ToList();
        //                //List<string> year_name = new List<string>();

        //                //if (listofdata.Count > 0)
        //                //{
        //                //    foreach (var item in listofdata)
        //                //    {
        //                //        year_name.Add(item.ASMAY_Year);
        //                //    }
        //                //}

        //                data.reportlist = retObject.ToArray();

        //                List<long> asmay_ids = new List<long>();

        //                asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

        //                List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

        //                data.reportlist2 = (from a in _GeneralContext.Academic
        //                                    where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
        //                                    select a).Distinct().ToArray();

        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        public async Task<HSU_CR2_Report_DTO> HSU_253_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_253_Report";
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

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.NAAC_HSU_EvaluationRelated_253_DMO
                                            from b in _GeneralContext.NAAC_HSU_EvaluationRelated_253_Files_DMO
                                            where (midss.Contains(a.MI_Id) && a.NCHSU253ER_Id==b.NCHSU253ER_Id)
                                            select b).Distinct().ToArray();

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
        public async Task<HSU_CR2_Report_DTO> HSU_255_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_255_Report";
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

                        var listofdata = retObject.ToList();
                        List<string> year_name = new List<string>();

                        if (listofdata.Count > 0)
                        {
                            foreach (var item in listofdata)
                            {
                                year_name.Add(item.ASMAY_Year);
                            }
                        }

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_262_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_262_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_271_Report(HSU_CR2_Report_DTO data)
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


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_271_Report";
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

                        //var listofdata = retObject.ToList();
                        //List<string> year_name = new List<string>();

                        //if (listofdata.Count > 0)
                        //{
                        //    foreach (var item in listofdata)
                        //    {
                        //        year_name.Add(item.ASMAY_Year);
                        //    }
                        //}

                        data.reportlist = retObject.ToArray();

                        List<long> asmay_ids = new List<long>();

                        asmay_ids = naaccomm.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        List<long> midss = naaccomm.get_Institution_User_MI_Id_list(data.MI_Id, data.UserId);

                        data.reportlist2 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
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
        public async Task<HSU_CR2_Report_DTO> HSU_252_Report(HSU_CR2_Report_DTO data)
        {
            try
            {


                List<long> yearid1 = new List<long>();

                NAAC_CommonDetails naaccomm11 = new NAAC_CommonDetails(_GeneralContext);

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

                NAAC_CommonDetails naaccomm1 = new NAAC_CommonDetails(_GeneralContext);

                yrid = naaccomm1.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);
                data.yearlist1 = (from a in _GeneralContext.Academic
                                  where a.Is_Active == true && mid.Contains(a.MI_Id) && yrid.Contains(a.ASMAY_Id)
                                  select new HSU_CR2_Report_DTO
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
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_HSU_252_Report";
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
                        data.govtsclistfiles = (from a in _GeneralContext.NAAC_HSU_StudentComplaints_252_Files_DMO
                                                from b in _GeneralContext.NAAC_HSU_StudentComplaints_252_DMO
                                                where (mid.Contains(b.MI_Id) && b.NCHSU252SC_Id == a.NCHSU252SC_Id)
                                                select a).Distinct().ToArray();
                        data.yearlist = (from a in _GeneralContext.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new HSU_CR2_Report_DTO
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
