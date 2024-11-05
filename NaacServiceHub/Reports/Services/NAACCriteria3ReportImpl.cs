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
    public class NAACCriteria3ReportImpl : Interface.NAACCriteria3ReportInterface
    {
        public GeneralContext _GeneralContext;
        public DocumentsContext _DocumentsContext;
        public NAACCriteria3ReportImpl(GeneralContext DocumentsContext, DocumentsContext f)
        {
            _GeneralContext = DocumentsContext;
            _DocumentsContext = f;
        }
        public NAACCriteria3ReportDTO getdata(NAACCriteria3ReportDTO data)
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


                data.yearlist = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public async Task<NAACCriteria3ReportDTO> get_report(NAACCriteria3ReportDTO data)
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
                    cmd.CommandText = "NaacStudentActivities_363_report";
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

                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO
                                            from b in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCACSA343_Id == a.NCACSA343_Id)
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


        //public async Task<NAACCriteria3ReportDTO> get_report(NAACCriteria3ReportDTO data)
        //{
        //    try
        //    {
        //        string yerds = "0";
        //        List<long> yrid = new List<long>();
        //        if (data.selectedYear.Length > 0)
        //        {
        //            foreach (var item in data.selectedYear)
        //            {
        //                yrid.Add(item.ASMAY_Id);
        //            }
        //            for (int i = 0; i < yrid.Count(); i++)
        //            {
        //                yerds = yerds + "," + yrid[i].ToString();
        //            }
        //        }

        //        data.govtsclist = (from a in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
        //                           from b in _GeneralContext.Academic
        //                           where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.NCACSA343_Year == b.ASMAY_Id && a.NCACSA343_ActiveFlg == true && b.Is_Active == true && yrid.Contains(a.NCACSA343_Year)
        //                           select new NAACCriteria3ReportDTO
        //                           {
        //                               NCACSA343_Id = a.NCACSA343_Id,
        //                               actname = a.NCACSA343_TypeOfActivity,
        //                               noofstd = a.NCACSA343_NoOfStudents,
        //                               agency = a.NCACSA343_OrgAgency,
        //                               ASMAY_Year = b.ASMAY_Year,
        //                               ASMAY_Order = b.ASMAY_Order
        //                           }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




        //        data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_343_StudentActivities_DMO
        //                                from b in _GeneralContext.NAAC_AC_343_StudentActivities_Files_DMO
        //                                where a.MI_Id == data.MI_Id && a.NCACSA343_ActiveFlg == true && a.NCACSA343_Id == b.NCACSA343_Id && yrid.Contains(a.NCACSA343_Year)
        //                                select b).Distinct().ToArray();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        //public async Task<NAACCriteria3ReportDTO> get_report364(NAACCriteria3ReportDTO data)
        //{
        //    try
        //    {
        //        string yerds = "0";
        //        List<long> yrid = new List<long>();
        //        if (data.selectedYear.Length > 0)
        //        {
        //            foreach (var item in data.selectedYear)
        //            {
        //                yrid.Add(item.ASMAY_Id);
        //            }
        //            for (int i = 0; i < yrid.Count(); i++)
        //            {
        //                yerds = yerds + "," + yrid[i].ToString();
        //            }
        //        }

        //        data.govtsclist = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
        //                           from b in _GeneralContext.Academic
        //                           where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.NCACET343_Year == b.ASMAY_Id && a.NCACET343_ActiveFlg == true && b.Is_Active == true && yrid.Contains(a.NCACET343_Year)
        //                           select new NAACCriteria3ReportDTO
        //                           {
        //                               NCACET343_Id = a.NCACET343_Id,
        //                               actname = a.NCACET343_TypeOfActivity,
        //                               noofstd = a.NCACET343_NoOfStudents,
        //                               agency = a.NCACET343_OrgAgency,
        //                               scheme = a.NCACET343_SchemeName,
        //                               ASMAY_Year = b.ASMAY_Year,
        //                               ASMAY_Order = b.ASMAY_Order
        //                           }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();




        //        data.govtsclistfiles = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
        //                                from b in _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO
        //                                where a.MI_Id == data.MI_Id && a.NCACET343_ActiveFlg == true && a.NCACET343_Id == b.NCACET343_Id && yrid.Contains(a.NCACET343_Year)
        //                                select b).Distinct().ToArray();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}

        public async Task<NAACCriteria3ReportDTO> get_report364(NAACCriteria3ReportDTO data)
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
                    cmd.CommandText = "NaacExternalActivities_363_report";
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

                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_344_ExtnActivities_Files_DMO
                                            from b in _GeneralContext.NAAC_AC_344_ExtnActivities_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCACET343_Id == a.NCACET343_Id)
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



        public async Task<NAACCriteria3ReportDTO> reportIPR(NAACCriteria3ReportDTO data)
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
                    cmd.CommandText = "NaacIPR_332_report";
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

                        data.reportlist2 = (from a in _GeneralContext.NAAC_AC_IPR_322_Files_DMO
                                            from b in _GeneralContext.NAAC_AC_IPR_322_DMO
                                            where (mid.Contains(b.MI_Id) && b.NCACIPR322_Id == a.NCACIPR322_Id)
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
