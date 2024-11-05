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
    public class DisabilityStudentImpl : Interface.DisabilityStudentInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;

        public DisabilityStudentImpl(DocumentsContext DocumentsContext, GeneralContext praa)
        {
            _DocumentsContext = DocumentsContext;
            _GeneralContext = praa;

        }

        public Criteria2_DTO getdata(Criteria2_DTO data)
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


                //string NAACSL_InstitutionTypeFlg = "";
                //List<long> miid = new List<long>();
                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }

                //NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                data.getinstitutioncycle = naaccomm.get_cycle_list(data.MI_Id, data.UserId);

                data.getinstitution = naaccomm.get_Institution_list(data.MI_Id, data.UserId);

                data.yeardata = _DocumentsContext.HR_Employee_Awards_DMO.Where(t => t.MI_Id == data.MI_Id && t.HREAW_ActiveFlg == true).Distinct().ToArray();

                data.deptlist = _DocumentsContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<Criteria2_DTO> get_report(Criteria2_DTO data)
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
                    for (int i = 0; i < mid.Count(); i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_Disability_STUDENT_REPORT";
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
        public async Task<Criteria2_DTO> Demand_Ratio_212_Report(Criteria2_DTO data)
        {
            try
            {
                string yerds = "0";
                List<long> yrid = new List<long>();
                if (data.selectedYear.Length > 0)
                {
                    foreach (var item in data.selectedYear)
                    {
                        yrid.Add(item.ASMAY_Id);
                    }
                    for (int i = 0; i < yrid.Count(); i++)
                    {
                        yerds = yerds + "," + yrid[i].ToString();
                    }
                }
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_Demand_Ratio_212_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = yerds
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
        public async Task<Criteria2_DTO> Exm_P_Stud_Report(Criteria2_DTO data)
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
                    for (int i = 0; i < mid.Count(); i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_P_Stud_Report_263";
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
        public async Task<Criteria2_DTO> EMPLOYEE_AWARD_REPORT244(Criteria2_DTO data)
        {
            try
            {
                //string yerds = "0";
                //List<long> yrid = new List<long>();
                //if (data.selectedYear.Length > 0)
                //{
                //    foreach (var item in data.selectedYear)
                //    {
                //        yrid.Add(item.HREAW_AwardYear);
                //    }
                //    for (int i = 0; i < yrid.Count(); i++)
                //    {
                //        yerds = yerds + "," + yrid[i].ToString();
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
                    for (int i = 0; i < mid.Count(); i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_EMPLOYEE_AWARD_REPORT244";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year",
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
        public Criteria2_DTO get_desination(Criteria2_DTO data)
        {
            try
            {
                List<long?> deptid = new List<long?>();
                if (data.selecteddept.Length > 0)
                {
                    foreach (var item in data.selecteddept)
                    {
                        deptid.Add(item.HRMD_Id);
                    }
                }
                data.desglist = (from a in _DocumentsContext.MasterEmployee
                                 from b in _DocumentsContext.HR_Master_Designation
                                 where (a.MI_Id == b.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.MI_Id == data.MI_Id && deptid.Contains(a.HRMD_Id))
                                 select new Criteria2_DTO
                                 {
                                     HRMDES_Id = b.HRMDES_Id,
                                     HRMDES_DesignationName = b.HRMDES_DesignationName,
                                 }).Distinct().OrderBy(t => t.HRMDES_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<Criteria2_DTO> Teacher_Recognised_242_Report(Criteria2_DTO data)
        {
            try
            {
                string hrmdids = "0";
                string hrmdesids = "0";
                List<long> hrmd = new List<long>();
                List<long> hrmdesid = new List<long>();
                if (data.selecteddept.Length > 0)
                {
                    foreach (var item in data.selecteddept)
                    {
                        hrmd.Add(item.HRMD_Id);
                    }
                    for (int i = 0; i < hrmd.Count(); i++)
                    {
                        hrmdids = hrmdids + "," + hrmd[i].ToString();
                    }
                }
                if (data.selecteddesg.Length > 0)
                {
                    foreach (var item in data.selecteddesg)
                    {
                        hrmdesid.Add(item.HRMDES_Id);
                    }
                    for (int s = 0; s < hrmdesid.Count; s++)
                    {
                        hrmdesids = hrmdesids + "," + hrmdesid[s].ToString();
                    }
                }
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_T_Recognised_242";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmdids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmdesids
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
        public async Task<Criteria2_DTO> T_ProfileAndQuality_Report24(Criteria2_DTO data)
        {
            try
            {
                string hrmdids = "0";
                string hrmdesids = "0";
                List<long> hrmd = new List<long>();
                List<long> hrmdesid = new List<long>();
                if (data.selecteddept.Length > 0)
                {
                    foreach (var item in data.selecteddept)
                    {
                        hrmd.Add(item.HRMD_Id);
                    }
                    for (int i = 0; i < hrmd.Count(); i++)
                    {
                        hrmdids = hrmdids + "," + hrmd[i].ToString();
                    }
                }
                if (data.selecteddesg.Length > 0)
                {
                    foreach (var item in data.selecteddesg)
                    {
                        hrmdesid.Add(item.HRMDES_Id);
                    }
                    for (int s = 0; s < hrmdesid.Count; s++)
                    {
                        hrmdesids = hrmdesids + "," + hrmdesid[s].ToString();
                    }
                }
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_T_ProfileAndQuality_Report24";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmdids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmdesids
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
        public async Task<Criteria2_DTO> Student_Enrolment_Profile_Report21(Criteria2_DTO data)
        {
            try
            {
                //string yerds = "0";
                //List<long> yrid = new List<long>();
                //if (data.selectedYear.Length > 0)
                //{
                //    foreach (var item in data.selectedYear)
                //    {
                //        yrid.Add(item.ASMAY_Id);
                //    }
                //    for (int i = 0; i < yrid.Count(); i++)
                //    {
                //        yerds = yerds + "," + yrid[i].ToString();
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
                    for (int i = 0; i < mid.Count(); i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_StudentEnrolmentAndProfile_OtherState_Report21";
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

                        data.otherstatereportlist = retObject.ToArray();
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

                #region THIS PROCEDURE WILL WORKS INSIDE ABOVE PROCEDURE
                //using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "NAAC_StudentEnrolmentAndProfile_OtherCountry_Report21";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //    SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //   SqlDbType.VarChar)
                //    {
                //        Value = yerds
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.othercountryreportlist = retObject.ToArray();

                //    }


                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                #endregion

                var othercountry = (from a in _DocumentsContext.Naac_Temp_OTState_OTCntry_Report21_DMO
                                    where (a.NoofOTCStudents != 0)
                                    select new Criteria2_DTO
                                    {
                                        IVRMMC_CountryName = a.IVRMMC_CountryName,
                                        NoofOTCStudents = a.NoofOTCStudents,
                                        ASMAY_Id = a.ASMAY_Id,
                                        ASMAY_Year = a.ASMAY_Year,
                                    }).Distinct().ToList();

                data.othercountrycount = othercountry.ToArray();

                var otherstate = (from a in _DocumentsContext.Naac_Temp_OTState_OTCntry_Report21_DMO
                                  select new Criteria2_DTO
                                  {
                                      studentname = a.studentname,
                                      IVRMMS_Name = a.IVRMMS_Name,
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToList();

                data.otherstatecount = otherstate.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<Criteria2_DTO> StudentSat_Survey_Report27(Criteria2_DTO data)
        {
            try
            {
                //string yerds = "0";
                //List<long> yrid = new List<long>();
                //if (data.selectedYear.Length > 0)
                //{
                //    foreach (var item in data.selectedYear)
                //    {
                //        yrid.Add(item.ASMAY_Id);
                //    }
                //    for (int i = 0; i < yrid.Count(); i++)
                //    {
                //        yerds = yerds + "," + yrid[i].ToString();
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
                    for (int i = 0; i < mid.Count(); i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);


                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_StudentSatisfaction_Survey_Report27";
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
        public async Task<Criteria2_DTO> sanctioned_posts_Report245(Criteria2_DTO data)
        {
            try
            {
                string hrmdids = "0";
                string hrmdesids = "0";
                List<long> hrmd = new List<long>();
                List<long> hrmdesid = new List<long>();
                if (data.selecteddept.Length > 0)
                {
                    foreach (var item in data.selecteddept)
                    {
                        hrmd.Add(item.HRMD_Id);
                    }
                    for (int i = 0; i < hrmd.Count(); i++)
                    {
                        hrmdids = hrmdids + "," + hrmd[i].ToString();
                    }
                }
                if (data.selecteddesg.Length > 0)
                {
                    foreach (var item in data.selecteddesg)
                    {
                        hrmdesid.Add(item.HRMDES_Id);
                    }
                    for (int s = 0; s < hrmdesid.Count; s++)
                    {
                        hrmdesids = hrmdesids + "," + hrmdesid[s].ToString();
                    }
                }
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_sanctioned_posts_Report245";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmdids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmdesids
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
        public async Task<Criteria2_DTO> DeclrofResult_Report251(Criteria2_DTO data)
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
                    for (int i = 0; i < mid.Count(); i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);
                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_DeclrofResult_Report251";
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                #region THIS PROCEDURE WILL WORKS INSIDE ABOVE PROCEDURE
                //using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "NAAC_StudentEnrolmentAndProfile_OtherCountry_Report21";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //    SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //   SqlDbType.VarChar)
                //    {
                //        Value = yerds
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.othercountryreportlist = retObject.ToArray();

                //    }


                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                #endregion

                var othercountry = (from a in _DocumentsContext.Naac_Temp_OTState_OTCntry_Report21_DMO
                                    where (a.NoofOTCStudents != 0)
                                    select new Criteria2_DTO
                                    {
                                        IVRMMC_CountryName = a.IVRMMC_CountryName,
                                        NoofOTCStudents = a.NoofOTCStudents,
                                        ASMAY_Id = a.ASMAY_Id,
                                        ASMAY_Year = a.ASMAY_Year,
                                    }).Distinct().ToList();

                data.othercountrycount = othercountry.ToArray();

                var otherstate = (from a in _DocumentsContext.Naac_Temp_OTState_OTCntry_Report21_DMO
                                  select new Criteria2_DTO
                                  {
                                      studentname = a.studentname,
                                      IVRMMS_Name = a.IVRMMS_Name,
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMAY_Year = a.ASMAY_Year,
                                  }).Distinct().ToList();

                data.otherstatecount = otherstate.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
