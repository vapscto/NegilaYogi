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
    public class CurricularAspectsImpl : Interface.CurricularAspectsInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;
        public CurricularAspectsImpl(DocumentsContext DocumentsContext, GeneralContext praa)
        {
            _DocumentsContext = DocumentsContext;
            _GeneralContext = praa;
        }
        public CurricularAspects_DTO getdata(CurricularAspects_DTO data)
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

                data.yearlist = _DocumentsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CurricularAspects_DTO> get_report(CurricularAspects_DTO data)
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
                    cmd.CommandText = "naac_112_report";
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

                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_Programs_112_FilesDMO
                                            from b in _GeneralContext.NAAC_AC_Programs_112_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCACPR112_Id == a.NCACPR112_Id && a.NCACPR112F_ActiveFlag==true)
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
        public async Task<CurricularAspects_DTO> get_nCourse_report(CurricularAspects_DTO data)
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
                    cmd.CommandText = "NAAC_NewCourseIntroduce_Report";
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of 
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
        public async Task<CurricularAspects_DTO> get_report_113(CurricularAspects_DTO data)
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
                    cmd.CommandText = "NAAC_AC_TParticipation_113_Report_New";
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
                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_TParticipation_113_FilesDMO
                                            from b in _GeneralContext.NAAC_AC_TParticipation_113_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCACTP113_Id == a.NCACTP113_Id && a.NCACTP113F_ActiveFlg==true)
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
        public async Task<CurricularAspects_DTO> get_report_123(CurricularAspects_DTO data)
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
                    cmd.CommandText = "NAAC_AC_SParticipation_123_Report_New";
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
                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_SParticipation_123_DMO
                                            from b in _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO
                                            where (mid.Contains(a.MI_Id) && a.NCACSP123_Id == b.NCACSP123_Id && a.NCACSP123_ActiveFlg==true)
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
        public async Task<CurricularAspects_DTO> get_report_133(CurricularAspects_DTO data)
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
                    cmd.CommandText = "NAAC_AC_SProjects_133_Report_new23";
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
                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_SProjects_133_DMO
                                            from b in _GeneralContext.NAAC_AC_SProjects_133_FilesDMO
                                            where (mid.Contains(a.MI_Id) && a.NCACSPR133_Id == b.NCACSPR133_Id && a.NCACSPR133_ActiveFlg==true)
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
        public async Task<CurricularAspects_DTO> get_report_132(CurricularAspects_DTO data)
        {
            try
            {
                //string yeraids = "0";
                //List<long> asmyid = new List<long>();
                //if (data.selectedYear.Length > 0)
                //{
                //    foreach (var item in data.selectedYear)
                //    {
                //        asmyid.Add(item.ASMAY_Id);
                //    }
                //    for (int i = 0; i < asmyid.Count; i++)
                //    {
                //        yeraids = yeraids + "," + asmyid[i].ToString();
                //    }
                //}
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
                    cmd.CommandText = "NAAC_AC_VAC_132_Report_new32";
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

                        List<long> asmy_ids = new List<long>();                      

                        string NAACSL_InstitutionTypeFlg = "";
                        var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                        if (getinstitution.Count() > 0)
                        {
                            NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                        }
                        data.instsclist = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                           from b in _GeneralContext.NAAC_AC_VAC_132_Files_DMO
                                           where (mid.Contains(a.MI_Id) && a.NCACVAC132_Id == b.NCACVAC132_Id && b.NCACVAC132F_ActiveFlg==true)
                                           select b).Distinct().ToArray();

                        data.instsclistfiles = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                                from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                                from c in _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO
                                                where (mid.Contains(a.MI_Id) && a.NCACVAC132_Id == b.NCACVAC132_Id && b.NCACVAC132D_Id == c.NCACVAC132D_Id && c.NCACVAC132DF_ActiveFlg==true)
                                                select new CurricularAspects_DTO
                                                {
                                                    NCACVAC132_Id = a.NCACVAC132_Id,
                                                    NCACVAC132DF_Id = c.NCACVAC132DF_Id,
                                                    NCACVAC132D_Id = c.NCACVAC132D_Id,
                                                    NCACVAC132DF_FileName = c.NCACVAC132DF_FileName,
                                                    NCACVAC132DF_Filedesc = c.NCACVAC132DF_Filedesc,
                                                    NCACVAC132DF_FilePath = c.NCACVAC132DF_FilePath,
                                                }).Distinct().ToArray();

                        List<CurricularAspects_DTO> dto = new List<CurricularAspects_DTO>();

                        if (NAACSL_InstitutionTypeFlg.ToUpper() == "UNIVERSITY")
                        {

                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   from c in _GeneralContext.NAAC_Master_Trust_CycleDMO
                                   from d in _GeneralContext.NAAC_Master_Trust_Cycle_MappingDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && c.NCMATC_Id == d.NCMATC_Id && d.NCMACY_Id == a.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && c.NCMATC_ActiveFlg == true && c.NCMATC_Id == data.cycleid)
                                   select new CurricularAspects_DTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id
                                   }).Distinct().ToList();
                        }
                        else
                        {
                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && a.NCMACY_Id == data.cycleid)
                                   select new CurricularAspects_DTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id
                                   }).Distinct().ToList();
                        }

                        List<long> asmay_ids = new List<long>();

                        if (dto.Count>0)
                        {
                            foreach (var item in dto)
                            {
                                asmay_ids.Add(item.ASMAY_Id);
                            }
                        }
                        data.yearlist = (from a in _DocumentsContext.AcademicYear
                                         where(asmay_ids.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true) select a).Distinct().ToArray();

                      
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
        public async Task<CurricularAspects_DTO> get_122CBCSsystemReport(CurricularAspects_DTO data)
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
                    cmd.CommandText = "NAAC_122CBCSsystemReport";
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

                        List<long> asmy_ids = new List<long>();

                        string NAACSL_InstitutionTypeFlg = "";
                        var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                        if (getinstitution.Count() > 0)
                        {
                            NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                        }
                        data.instsclist = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                           from b in _GeneralContext.NAAC_AC_VAC_132_Files_DMO
                                           where (mid.Contains(a.MI_Id) && a.NCACVAC132_Id == b.NCACVAC132_Id && b.NCACVAC132F_ActiveFlg==true)
                                           select b).Distinct().ToArray();

                        data.instsclistfiles = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                                from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                                from c in _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO
                                                where (mid.Contains(a.MI_Id) && a.NCACVAC132_Id == b.NCACVAC132_Id && b.NCACVAC132D_Id == c.NCACVAC132D_Id && c.NCACVAC132DF_ActiveFlg==true)
                                                select new CurricularAspects_DTO
                                                {
                                                    NCACVAC132_Id = a.NCACVAC132_Id,
                                                    NCACVAC132DF_Id = c.NCACVAC132DF_Id,
                                                    NCACVAC132D_Id = c.NCACVAC132D_Id,
                                                    NCACVAC132DF_FileName = c.NCACVAC132DF_FileName,
                                                    NCACVAC132DF_Filedesc = c.NCACVAC132DF_Filedesc,
                                                    NCACVAC132DF_FilePath = c.NCACVAC132DF_FilePath,
                                                }).Distinct().ToArray();

                        List<CurricularAspects_DTO> dto = new List<CurricularAspects_DTO>();

                        if (NAACSL_InstitutionTypeFlg.ToUpper() == "UNIVERSITY")
                        {

                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   from c in _GeneralContext.NAAC_Master_Trust_CycleDMO
                                   from d in _GeneralContext.NAAC_Master_Trust_Cycle_MappingDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && c.NCMATC_Id == d.NCMATC_Id && d.NCMACY_Id == a.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && c.NCMATC_ActiveFlg == true && c.NCMATC_Id == data.cycleid)
                                   select new CurricularAspects_DTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id
                                   }).Distinct().ToList();
                        }
                        else
                        {
                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && a.NCMACY_Id == data.cycleid)
                                   select new CurricularAspects_DTO
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        
    }
}
