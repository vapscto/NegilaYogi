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
    public class HSU_CR1_ReportImpl:Interface.HSU_CR1_ReportInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;
        public HSU_CR1_ReportImpl(DocumentsContext para1, GeneralContext para2)
        {
            _DocumentsContext = para1;
            _GeneralContext = para2;
        }

        public HSU_CR1_Report_DTO getdata(HSU_CR1_Report_DTO data)
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
        public async Task<HSU_CR1_Report_DTO> HSU_112_Report(HSU_CR1_Report_DTO data)
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

                        var listofdata= retObject.ToList();
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
                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_Programs_112_DMO
                                            from b in _GeneralContext.NAAC_AC_Programs_112_FilesDMO                                          
                                            where (mid.Contains(a.MI_Id) && b.NCACPR112_Id == a.NCACPR112_Id )
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
        public async Task<HSU_CR1_Report_DTO> HSU_132_133_Report(HSU_CR1_Report_DTO data)
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
                    cmd.CommandText = "NAAC_HSU_132_133_Report";
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

                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO
                                            from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                            from c in _GeneralContext.NAAC_AC_VAC_132_DMO
                                            where (mid.Contains(c.MI_Id) && b.NCACVAC132D_Id == a.NCACVAC132D_Id && c.NCACVAC132_Id==b.NCACVAC132_Id)
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
        public async Task<HSU_CR1_Report_DTO> HSU_141_Report(HSU_CR1_Report_DTO data)
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
                    cmd.CommandText = "Naac_HSU_141_Report";
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
        public async Task<HSU_CR1_Report_DTO> HSU_142_Report(HSU_CR1_Report_DTO data)
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
                    cmd.CommandText = "Naac_HSU_142_Report";
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
        public async Task<HSU_CR1_Report_DTO> HSU_121_Report(HSU_CR1_Report_DTO data)
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
                    cmd.CommandText = "Naac_HSU_142_Report";
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
        public async Task<HSU_CR1_Report_DTO> HSU_122_Report(HSU_CR1_Report_DTO data)
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
                    cmd.CommandText = "Naac_HSU_122_Report";
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

                        data.yearlist1 = (from a in _GeneralContext.Academic
                                            where (asmay_ids.Contains(a.ASMAY_Id) && midss.Contains(a.MI_Id))
                                            select a).Distinct().ToArray();

                        data.reportlist2 = (from a in _GeneralContext.NAAC_HSU_Course_StaffMapping_122DMO
                                            from b in _GeneralContext.NAAC_HSU_Course_StaffMapping_122_FilesDMO
                                            from c in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                            where (midss.Contains(c.MI_Id) && asmay_ids.Contains(c.ASMAY_Id) && a.ACAYC_Id == c.ACAYC_Id 
                                            && a.NCHSUSM122_Id == b.NCHSUSM122_Id && a.NCHSUSM122_ActiveFlag == true)
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
        //public async Task<HSU_CR1_Report_DTO> HSU_123_Report(HSU_CR1_Report_DTO data)
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
        //            cmd.CommandText = "Naac_HSU_123_Report";
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
        //                data.reportlist = retObject.ToArray();


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
        public async Task<HSU_CR1_Report_DTO> HSU_123_Report(HSU_CR1_Report_DTO data)
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
                                  select new HSU_CR1_Report_DTO
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
                    cmd.CommandText = "Naac_HSU_123_Report";
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
                        data.govtsclistfiles = (from a in _GeneralContext.NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO
                                                from b in _GeneralContext.NAAC_HSU_InterdisciplinaryProgrammes_123_DMO
                                                where (mid.Contains(b.MI_Id) && b.NCHSUIP123_Id == a.NCHSUIP123_Id)
                                                select a).Distinct().ToArray();
                        data.yearlist = (from a in _GeneralContext.Academic
                                         where a.Is_Active == true && mid.Contains(a.MI_Id) && yearid1.Contains(a.ASMAY_Id)
                                         select new HSU_CR1_Report_DTO
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
