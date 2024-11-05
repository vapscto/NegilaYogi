using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class SeatallotmentReportImpl : Interface.SeatallotmentReportInterface
    {
        public GeneralContext _GeneralContext;
        public DocumentsContext _DocumentsContext;    

        public SeatallotmentReportImpl(GeneralContext _general, DocumentsContext parameter)
        {
            _GeneralContext = _general;
            _DocumentsContext = parameter;
        }
        public SeatallotmentReportDTO getdetails(SeatallotmentReportDTO data)
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

                data.getyearlist = _DocumentsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SeatallotmentReportDTO onreport(SeatallotmentReportDTO data)
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

                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_213SeatAllotmentReport";
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
                            }
                        }
                        data.getdetails = retObject.ToArray();
                        //List<long> yearids = new List<long>();
                        //NAAC_CommonDetails naaccommn = new NAAC_CommonDetails(_GeneralContext);
                        //yearids = naaccommn.get_Year_listLong(data.MI_Id, data.UserId, data.cycleid);

                        //data.getyearlist = yearids.ToArray();


                        List<long> asmy_ids = new List<long>();

                        string NAACSL_InstitutionTypeFlg = "";
                        var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                        if (getinstitution.Count() > 0)
                        {
                            NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg.ToUpper();
                        }
                        List<SeatallotmentReportDTO> dto = new List<SeatallotmentReportDTO>();

                        if (NAACSL_InstitutionTypeFlg.ToUpper() == "UNIVERSITY")
                        {

                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   from c in _GeneralContext.NAAC_Master_Trust_CycleDMO
                                   from d in _GeneralContext.NAAC_Master_Trust_Cycle_MappingDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && c.NCMATC_Id == d.NCMATC_Id && d.NCMACY_Id == a.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && c.NCMATC_ActiveFlg == true && c.NCMATC_Id == data.cycleid)
                                   select new SeatallotmentReportDTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id
                                   }).Distinct().ToList();
                        }
                        else
                        {
                            dto = (from a in _GeneralContext.NAAC_Master_CycleDMO
                                   from b in _GeneralContext.NAAC_Master_Cycle_YearDMO
                                   where (a.NCMACY_Id == b.NCMACY_Id && a.NCMACY_ActiveFlg == true && b.NCMACYYR_ActiveFlg == true && a.NCMACY_Id == data.cycleid)
                                   select new SeatallotmentReportDTO
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
                        data.getyearlist = (from a in _DocumentsContext.AcademicYear
                                         where (asmay_ids.Contains(a.ASMAY_Id) && a.MI_Id == data.MI_Id && a.Is_Active == true)
                                         select a).Distinct().ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                #region
                //using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Naac_Admission_Seat_Allotement_Report";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                //    cmd.Parameters.Add(new SqlParameter("@NOOFYEARS", SqlDbType.VarChar) { Value = data.Noofyears });
                //    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = 2 });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
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
                //        data.getyearlist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write(ex.Message);
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
