using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SRKVSSportsReportIMPL : Interfaces.SRKVSSportsReportInterface
    {
        private static ConcurrentDictionary<string, SRKVSSportsReportDTO> _login = new ConcurrentDictionary<string, SRKVSSportsReportDTO>();

        private readonly SportsContext _sportcontext;

        public SRKVSSportsReportIMPL(SportsContext sportcontext)
        {
            _sportcontext = sportcontext;

        }


        public SRKVSSportsReportDTO Getdetails(SRKVSSportsReportDTO data)//int IVRMM_Id
        {

            try
            {
                //SPCC_Events_Students_DMO
                List<long> SPCCMCC_Id = new List<long>();
                List<long> SPCCMCL_Id = new List<long>();
                List<long> SPCCMSCCG_Id = new List<long>();
                List<long> SPCCME_Id = new List<long>();
                //
                List<SRKVSSportsReportDTO> Tasklist = new List<SRKVSSportsReportDTO>();
                var Eventlist = _sportcontext.SPCC_Events_Students_DMO.Where(R => R.SPCCEST_ActiveFlag == true && R.MI_Id == data.MI_Id && R.ASMAY_Id==data.ASMAY_Id).ToList();
                if (Eventlist != null && Eventlist.Count > 0)
                {
                    foreach(var d  in Eventlist)
                    {
                        SPCCMCC_Id.Add(d.SPCCMCC_Id);
                        SPCCMCL_Id.Add(d.SPCCMCL_Id);
                        SPCCME_Id.Add(d.SPCCME_Id);
                    }
                }

                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                data.categoryList = (from a in _sportcontext.MasterCompitionCategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true && SPCCMCC_Id.Contains(a.SPCCMCC_Id))
                                     select new EventsStudentRecordDTO
                                     {
                                         SPCCMCC_Id = a.SPCCMCC_Id,
                                         SPCCMCC_CompitionCategory = a.SPCCMCC_CompitionCategory,
                                     }).Distinct().ToArray();
                //CompetetionLevel
                data.CompetetionLevel = _sportcontext.SportMasterCompitionLevelDMO.Where(R => R.SPCCMCL_ActiveFlag == true && R.MI_Id == data.MI_Id && SPCCMCL_Id.Contains(R.SPCCMCL_Id)).Distinct().ToArray();
                //MasterEventsDMO
                data.MasterEvent = _sportcontext.MasterEventsDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCME_ActiveFlag == true && SPCCME_Id.Contains(R.SPCCME_Id)).Distinct().ToArray();


                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_StudentTarnsAlldata_SRKVS_Master";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                   

                   
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                                Tasklist.Add(new SRKVSSportsReportDTO
                                {
                                    SPCCMSCCG_Id = Convert.ToInt64(dataReader["mainID"]),                                 
                                });
                            }
                        }
                       
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                if(Tasklist.Count > 0)
                {
                    foreach(var d  in Tasklist)
                    {
                        SPCCMSCCG_Id.Add(d.SPCCMSCCG_Id);
                    }
                    data.GetMasterEvent = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == 0 && SPCCMSCCG_Id.Contains(d.SPCCMSCCG_Id)).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public SRKVSSportsReportDTO get_class(SRKVSSportsReportDTO data)
        {
            try
            {
                List<SRKVSSportsReportDTO> Tasklist = new List<SRKVSSportsReportDTO>();
                List<long> SPCCMCC_Id = new List<long>();
                List<long> SPCCMCL_Id = new List<long>();
                List<long> SPCCME_Id = new List<long>();
                List<long> SPCCMSCCG_Id = new List<long>();
                //
                var Eventlist = _sportcontext.SPCC_Events_Students_DMO.Where(R => R.SPCCEST_ActiveFlag == true && R.MI_Id == data.MI_Id && R.ASMAY_Id == data.ASMAY_Id).ToList();
                if (Eventlist != null && Eventlist.Count > 0)
                {
                    foreach (var d in Eventlist)
                    {
                        SPCCMCC_Id.Add(d.SPCCMCC_Id);
                        SPCCMCL_Id.Add(d.SPCCMCL_Id);
                        SPCCME_Id.Add(d.SPCCME_Id);
                    }
                }

                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                data.categoryList = (from a in _sportcontext.MasterCompitionCategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true && SPCCMCC_Id.Contains(a.SPCCMCC_Id))
                                     select new EventsStudentRecordDTO
                                     {
                                         SPCCMCC_Id = a.SPCCMCC_Id,
                                         SPCCMCC_CompitionCategory = a.SPCCMCC_CompitionCategory,
                                     }).Distinct().ToArray();
                //CompetetionLevel
                data.CompetetionLevel = _sportcontext.SportMasterCompitionLevelDMO.Where(R => R.SPCCMCL_ActiveFlag == true && R.MI_Id == data.MI_Id && SPCCMCL_Id.Contains(R.SPCCMCL_Id)).Distinct().ToArray();
                //MasterEventsDMO
                data.MasterEvent = _sportcontext.MasterEventsDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCME_ActiveFlag == true && SPCCME_Id.Contains(R.SPCCME_Id)).Distinct().ToArray();
              //  data.GetMasterEvent = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == 0).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();
                data.sportsCCList = _sportcontext.MasterSportsCCNameDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCC_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMSCC_SportsCCName).ToArray();
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_StudentTarnsAlldata_SRKVS_Master";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                                Tasklist.Add(new SRKVSSportsReportDTO
                                {
                                    SPCCMSCCG_Id = Convert.ToInt64(dataReader["mainID"]),
                                });
                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                if (Tasklist.Count > 0)
                {
                    foreach (var d in Tasklist)
                    {
                        SPCCMSCCG_Id.Add(d.SPCCMSCCG_Id);
                    }
                    data.GetMasterEvent = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == 0 && SPCCMSCCG_Id.Contains(d.SPCCMSCCG_Id)).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }



        public SRKVSSportsReportDTO get_classs(SRKVSSportsReportDTO data)
        {
            try
            {
                List<SRKVSSportsReportDTO> Tasklist = new List<SRKVSSportsReportDTO>();
                List<long> SPCCMCC_Id = new List<long>();
                List<long> SPCCMCL_Id = new List<long>();
                List<long> SPCCME_Id = new List<long>();
                List<long> SPCCMSCCG_Id = new List<long>();
                //
                var Eventlist = _sportcontext.SPCC_Events_Students_DMO.Where(R => R.SPCCEST_ActiveFlag == true && R.MI_Id == data.MI_Id && R.ASMAY_Id == data.ASMAY_Id).ToList();
                if (Eventlist != null && Eventlist.Count > 0)
                {
                    foreach (var d in Eventlist)
                    {
                        SPCCMCC_Id.Add(d.SPCCMCC_Id);
                        SPCCMCL_Id.Add(d.SPCCMCL_Id);
                        SPCCME_Id.Add(d.SPCCME_Id);
                    }
                }

                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                data.categoryList = (from a in _sportcontext.MasterCompitionCategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true && SPCCMCC_Id.Contains(a.SPCCMCC_Id))
                                     select new EventsStudentRecordDTO
                                     {
                                         SPCCMCC_Id = a.SPCCMCC_Id,
                                         SPCCMCC_CompitionCategory = a.SPCCMCC_CompitionCategory,
                                     }).Distinct().ToArray();
                //CompetetionLevel
                data.CompetetionLevelRecord = _sportcontext.SportMasterCompitionLevelDMO.Where(R => R.SPCCMCL_ActiveFlag == true && R.MI_Id == data.MI_Id).Distinct().ToArray();

                data.CompetetionLevel = _sportcontext.SportMasterCompitionLevelDMO.Where(R => R.SPCCMCL_ActiveFlag == true && R.MI_Id == data.MI_Id && SPCCMCL_Id.Contains(R.SPCCMCL_Id)).Distinct().ToArray();
                //MasterEventsDMO
                data.categoryListRecord = _sportcontext.MasterCompitionCategoryDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCMCC_ActiveFlag == true).Distinct().ToArray();

                data.MasterEvent = _sportcontext.MasterEventsDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCME_ActiveFlag == true && SPCCME_Id.Contains(R.SPCCME_Id)).Distinct().ToArray();
                //  data.GetMasterEvent = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == 0).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();
                data.sportsCCList = _sportcontext.MasterSportsCCNameDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCC_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMSCC_SportsCCName).ToArray();
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_StudentTarnsAlldata_SRKVS_Master";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                                Tasklist.Add(new SRKVSSportsReportDTO
                                {
                                    SPCCMSCCG_Id = Convert.ToInt64(dataReader["mainID"]),
                                });
                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                if (Tasklist.Count > 0)
                {
                    foreach (var d in Tasklist)
                    {
                        SPCCMSCCG_Id.Add(d.SPCCMSCCG_Id);
                    }
                    data.GetMasterEvent = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == 0 && SPCCMSCCG_Id.Contains(d.SPCCMSCCG_Id)).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }



        public SRKVSSportsReportDTO get_section(SRKVSSportsReportDTO dto)
        {
            try
            {

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public SRKVSSportsReportDTO showdetails(SRKVSSportsReportDTO data)
        {
            try
            {

                data.name = _sportcontext.MasterEventsDMO.Where(R => R.SPCCME_Id == data.SPCCME_Id).Select(R => R.SPCCME_EventName).FirstOrDefault();
                data.logo = _sportcontext.Institution.Where(R => R.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();
                string SPCCMCC_Id = "0";
                string SPCCMCL_Id = "0";
                string SPCCME_Id = "0";
                string SPCCMSCCG_Id = "0";
                if (data.Categorylists != null && data.Categorylists.Length > 0)
                {
                    foreach (var d in data.Categorylists)
                    {
                        SPCCMCC_Id = SPCCMCC_Id + ',' + d.SPCCMCC_Id;
                    }
                }
                if (data.CompetetionLevels != null && data.CompetetionLevels.Length > 0)
                {
                    foreach (var d in data.CompetetionLevels)
                    {
                        SPCCMCL_Id = SPCCMCL_Id + ',' + d.SPCCMCL_Id;
                    }
                }
                if (data.Sportleveltemps != null && data.Sportleveltemps.Length > 0)
                {
                    foreach (var d in data.Sportleveltemps)
                    {
                        SPCCME_Id = SPCCME_Id + ',' + d.SPCCME_Id;
                    }

                }
                //SubEventLists
                if (data.SubEventLists != null && data.SubEventLists.Length > 0)
                {
                    foreach (var d in data.SubEventLists)
                    {
                        SPCCMSCCG_Id = SPCCMSCCG_Id + ',' + d.SPCCMSCCG_Id;
                    }

                }

                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Aquatic_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id", SqlDbType.VarChar) { Value = SPCCMCC_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCL_Id", SqlDbType.VarChar) { Value = SPCCMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.Type });
                    cmd.Parameters.Add(new SqlParameter("@SPCCME_Id ", SqlDbType.VarChar) { Value = SPCCME_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMSCCG_Id ", SqlDbType.VarChar) { Value = SPCCMSCCG_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.GetReport = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //SPC_Aquatic_Master_report
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Aquatic_Master_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id", SqlDbType.VarChar) { Value = SPCCMCC_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCL_Id", SqlDbType.VarChar) { Value = SPCCMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.Type });
                    cmd.Parameters.Add(new SqlParameter("@SPCCME_Id ", SqlDbType.VarChar) { Value = SPCCME_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMSCCG_Id ", SqlDbType.VarChar) { Value = SPCCMSCCG_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );

                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.gettsreport = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Aquatic_report_Finish";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id", SqlDbType.VarChar) { Value = SPCCMCC_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCL_Id", SqlDbType.VarChar) { Value = SPCCMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.Type });
                    cmd.Parameters.Add(new SqlParameter("@SPCCME_Id ", SqlDbType.VarChar) { Value = SPCCME_Id });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMSCCG_Id ", SqlDbType.VarChar) { Value = SPCCMSCCG_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.GetReportfinish = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
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





//Select DISTINCT E.SPCCME_EventName,H.SPCCMSCC_SportsCCName,  L.AMAY_RollNo , A.SPCCESTR_Points  ,          
//CONCAT(D.AMST_FirstName,' ', D.AMST_MiddleName,' ', D.AMST_LastName) AMST_FirstName,D.AMST_AdmNo,C.SPCCEST_Record,K.SPCCMEV_EventVenue,C.ASMAY_Id
//  from Spc.SPCC_Events_Students_Record A
//inner JOIN Spc.SPCC_Events_Students B ON A.SPCCEST_Id=B.SPCCEST_Id
//left JOIN[SPC].[SPCC_Students_Master_Record] C ON   B.MI_Id= A.MI_Id
//AND B.SPCCMCC_Id= C.SPCCMCC_Id AND B.SPCCMCL_Id= C.SPCCMCL_Id AND B.SPCCMUOM_Id= C.SPCCMUOM_Id AND B.SPCCMSCC_Id= C.SPCCMSCC_Id
//inner JOIN Adm_M_Student D ON D.AMST_Id= A.AMST_Id
//inner join Adm_School_Y_Student L ON L.AMST_Id= D.AMST_Id--AND L.ASMAY_Id= 190
//inner JOIN SPC.SPCC_Master_Events E ON E.SPCCME_Id= B.SPCCME_Id
//inner JOIN [SPC].[SPCC_Master_Compition_Category] F ON F.SPCCMCC_Id= C.SPCCMCC_Id
//inner JOIN [SPC].[SPCC_Master_Compition_Level] G ON G.SPCCMCL_Id= C.SPCCMCL_Id
//inner JOIN [SPC].[SPCC_Master_SportsCCName] H ON H.SPCCMSCC_Id= C.SPCCMSCC_Id
//inner JOIN [SPC].[SPCC_Master_UOM] I ON I.SPCCMUOM_Id= C.SPCCMUOM_Id
//inner JOIN [SPC].[SPCC_Events] J ON J.SPCCME_Id= E.SPCCME_Id
//inner JOIN [SPC].[SPCC_Master_EventVenue] K ON K.SPCCMEV_Id= J.SPCCMEV_Id
//WHERE A.MI_Id= 10 AND L.ASMAY_ID= 190 AND C.SPCCMCC_Id IN (0,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26) AND C.SPCCMCL_Id IN (0,22,23,24,25,26,27,28,29,30)
//AND E.SPCCME_Id in (0,10052) and D.AMST_ActiveFlag=1 and D.AMST_SOL= 'S'
