using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;

using System.Collections.Generic;
using System.Linq;
using System;
using DomainModel.Model.com.vaps.admission;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;


namespace FeeServiceHub.com.vaps.services
{
    public class MonthlyCollectionReportImpl : interfaces.MonthlyCollectionReportInterface
    {
        public FeeGroupContext _FeeGroupContext;

        string IVRM_CLM_coloumn = "";
        public MonthlyCollectionReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        private readonly UserManager<ApplicationUser> _UserManager;

        public MonthlyCollectionReportImpl(FeeGroupContext FeeGroupContext, UserManager<ApplicationUser> UserManager)
        {
            _FeeGroupContext = FeeGroupContext;
            _UserManager = UserManager;
        }
        public MonthlyCollectionReportDTO getdetails(MonthlyCollectionReportDTO data)
        {
           // FeeTransactionPaymentDTO data = new FeeTransactionPaymentDTO();
            try
            {

                //List<FeeGroupDMO> feegrp = new List<FeeGroupDMO>();              
                //feegrp = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id==data.mid).ToList();
                //data.fillfeegroup = feegrp.GroupBy(g=>g.FMG_GroupName).Select(g=>g.First()).ToArray();
                //// var SourceIds = mas.SelectedSourceDetails.Select(d => d.PAMS_Id).ToArray();

                ////   var SourceIds = mas.TempararyArrayListstring.Select(d => d.FMG_Id).ToArray();

                ////  !SourceIds.Contains(t.PAMS_Id);
                data.studentlist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.mid && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S")
                                    select new MonthlyCollectionReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
              ).Distinct().ToArray();





                if (data.reporttype.Equals("T"))
                {
                    data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                                            from b in _FeeGroupContext.feeTr
                                            where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.mid) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMT_Name = b.FMT_Name,
                                                FMT_Id = a.FMT_Id,
                                            }
                         ).Distinct().ToArray();

                    //List<FeeTransactionPaymentDTO> customlist = new List<FeeTransactionPaymentDTO>();

                    data.customlist = (from a in _FeeGroupContext.feegm
                                       from b in _FeeGroupContext.feeGGG
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id  && a.FMGG_ActiveFlag == true && a.MI_Id == data.mid)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMGG_Id = a.FMGG_Id,
                                           fmg_groupname = a.FMGG_GroupName
                                       }
                         ).Distinct().ToArray();


                    // data.customlist = customlist.ToArray();
                    List<long> grpid = new List<long>();

                    foreach (FeeStudentTransactionDTO item in data.customlist)
                    {
                        grpid.Add(item.FMGG_Id);
                    }

                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.mid)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();


                }
                else
                {
                    data.customlist = (from a in _FeeGroupContext.feegm
                                       from b in _FeeGroupContext.feeGGG
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.mid)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMGG_Id = a.FMGG_Id,
                                           fmg_groupname = a.FMGG_GroupName
                                       }
                     ).Distinct().ToArray();


                    // data.customlist = customlist.ToArray();
                    List<long> grpid = new List<long>();

                    foreach (FeeStudentTransactionDTO item in data.customlist)
                    {
                        grpid.Add(item.FMGG_Id);
                    }

                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.mid)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();

                }



            


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public MonthlyCollectionReportDTO getstuddet(MonthlyCollectionReportDTO data)
        {
            try
            {
                if (data.regornamedetails == "admno")
                {
                    data.studentlist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.mid && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S")
                                        select new MonthlyCollectionReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,

                                        }
             ).GroupBy(s=>s.AMST_Id).Select(s=>s.First()).ToArray();

                }

                else if (data.regornamedetails == "name")
                {
                    data.studentlist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.mid && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S")
                                        select new MonthlyCollectionReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                        }
             ).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }

        public async Task<MonthlyCollectionReportDTO> getreport(MonthlyCollectionReportDTO datare)
        {
            string name = "";
            string IVRM_CLM_coloumn = "";
            string confromdate = "";
            string confromdate2 = "";
            try
            {
                //for (int i = 0; i < datare.Tempgroupid.Length; i++)
                //{
                //    name = datare.Tempgroupid[i].columnID;
                //    if (name != null)
                //    {

                //        IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                //    }
                //}

                //string coloumns = "";
                //coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                DateTime date1 = DateTime.Now;

                 date1 = Convert.ToDateTime(datare.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
                // confromdate = fromdatecon.ToString();
                confromdate = date1.ToString("dd/MM/yyyy");

                DateTime date2 = DateTime.Now;

                date2 = Convert.ToDateTime(datare.Todate.Value.Date.ToString("yyyy-MM-dd"));
                // confromdate = fromdatecon.ToString();
                confromdate2 = date2.ToString("dd/MM/yyyy");



                // for column head binding


                var fmg_ids = "";
                foreach (var x in datare.FMG_Ids)
                {
                    fmg_ids += x + ",";
                }
                fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));

                var fmt_ids = "";
                if (datare.term_group == "T")
                {
                    foreach (var x in datare.FMT_Ids)
                    {
                        fmt_ids += x + ",";
                    }
                    fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));
                }


                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Month_year_headbinding";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                            SqlDbType.VarChar)
                    {
                        Value = confromdate2
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                          SqlDbType.VarChar)
                    {
                        Value = datare.mid
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                         SqlDbType.VarChar)
                    {
                        Value = datare.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@chequedate",
                    SqlDbType.BigInt)
                    {
                        Value = datare.chequedate
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

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
                        datare.alldatagridreportheads = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                // for  column head binding end here


                // for data binding
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                     cmd.CommandText = "Fee_Montly_collection2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                            SqlDbType.VarChar)
                    {
                        Value = confromdate2
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                          SqlDbType.VarChar)
                    {
                        Value = "BO"
                    });


                    cmd.Parameters.Add(new SqlParameter("@allorind",
                        SqlDbType.VarChar)
                    {
                        Value = datare.allorindivflag
                    });

                    cmd.Parameters.Add(new SqlParameter("@amstid",
                     SqlDbType.VarChar)
                    {
                        Value = datare.idamstid
                    });

                    cmd.Parameters.Add(new SqlParameter("@groupids",
                 SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@termids",
                SqlDbType.VarChar)
                    {
                        Value = fmt_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@left",
                SqlDbType.VarChar)
                    {
                        Value = datare.studenttype
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                         SqlDbType.VarChar)
                    {
                        Value = datare.mid
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                         SqlDbType.VarChar)
                    {
                        Value = datare.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@term_group",
                SqlDbType.VarChar, 1)
                    {
                        Value = datare.term_group
                    });
                    cmd.Parameters.Add(new SqlParameter("@chequedate",
                    SqlDbType.BigInt)
                    {
                        Value = datare.chequedate
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {

                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled),
                                        dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                       // datare.alldatagridreportheads = retObject1.ToArray();
                        datare.alldatagridreport = retObject1.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return datare;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return datare;
        }
    }
}
