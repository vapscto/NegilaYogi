using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using System.IO;
using System.Globalization;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeCollectionReportImpl : interfaces.FeeCollectionReportInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public FeeCollectionReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data)
        {
            try
            {
                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == data.MI_ID).ToList();
                data.accountdetails = feemasnum.ToArray();


                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();


                List<School_M_Class> classes = new List<School_M_Class>();
                classes = _FeeGroupContext.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                data.fillclass = classes.ToArray();


                data.fillsection = _FeeGroupContext.school_M_Section.Where(d => d.MI_Id == data.MI_ID && d.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToArray();

                List<FeeInstallmentsyearlyDMO> installment = new List<FeeInstallmentsyearlyDMO>();
                data.installment = _FeeGroupContext.feeMIY.Where(t => t.MI_ID == data.MI_ID).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO getgrpterms(FeeTransactionPaymentDTO data)
        {
            try
            {
                if (data.reporttype.Equals("T"))
                {
                    data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                                            from b in _FeeGroupContext.feeTr
                                            from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                            where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID && c.User_Id == data.userid) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMT_Name = b.FMT_Name,
                                                FMT_Id = a.FMT_Id,
                                            }
                         ).Distinct().ToArray();



                    //data.installment = _FeeGroupContext.feeMIY.Where(t => t.FTI_Id == data.FTI_Id).Distinct().ToArray();

                    //List<FeeTransactionPaymentDTO> customlist = new List<FeeTransactionPaymentDTO>();

                    data.customlist = (from a in _FeeGroupContext.feegm
                                       from b in _FeeGroupContext.feeGGG
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID
                                       && c.User_Id == data.userid)
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
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
                                       where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID && c.User_Id == data.userid)
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
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
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

        public async Task<FeeTransactionPaymentDTO> getradiofiltereddata([FromBody] FeeTransactionPaymentDTO temp)
        {
            if (temp.reporttype == "year")
            {
                temp.From_Date = Convert.ToDateTime("01 / 01 / 2017");
                temp.To_Date = Convert.ToDateTime("01 / 01 / 2017");
                // temp.duedate = "null";
            }

            var fmg_ids = "";
            foreach (var x in temp.FMG_Ids)
            {
                fmg_ids += x + ",";
            }
            fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));

            var fmt_ids = "";
            if (temp.term_group == "T")
            {
                foreach (var x in temp.FMT_Ids)
                {
                    fmt_ids += x + ",";
                }

            }
            if (fmt_ids.Length > 0)
            {
                fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));
            }




            var From_date = temp.From_Date.ToString();
            var To_date = temp.To_Date.ToString();

            if (temp.radioval == "INMW")
            {

                var fti_ids = "";
                if (temp.FTI_ids.Length > 0)
                {
                    foreach (var x in temp.FTI_ids)
                    {
                        fti_ids += x + ",";
                    }
                }
                if (fti_ids.Length > 0)
                {
                    fti_ids = fti_ids.Substring(0, (fti_ids.Length - 1));
                }

               

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "installmentwise_collection_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;

                    cmd.Parameters.Add(new SqlParameter("@fmg_id",
                   SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@fti_id",
                     SqlDbType.VarChar)
                    {
                        Value = fti_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(temp.ASMAY_Id)
                    });


                    cmd.Parameters.Add(new SqlParameter("@type",
                   SqlDbType.VarChar)
                    {
                        Value = temp.reporttype
                    });
                    cmd.Parameters.Add(new SqlParameter("@option",
                       SqlDbType.VarChar)
                    {
                        Value = temp.radioval
                    });
                    cmd.Parameters.Add(new SqlParameter("@active",
                  SqlDbType.BigInt)
                    {
                        Value = temp.active
                    });
                    cmd.Parameters.Add(new SqlParameter("@deactive",
                  SqlDbType.BigInt)
                    {
                        Value = temp.deactive
                    });
                    cmd.Parameters.Add(new SqlParameter("@left",
                  SqlDbType.BigInt)
                    {
                        Value = temp.left
                    });
                    cmd.Parameters.Add(new SqlParameter("@date1",
            SqlDbType.DateTime)
                    {
                        Value = temp.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@date2",
               SqlDbType.DateTime)
                    {
                        Value = temp.To_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                    {
                        Value = temp.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@term_group",
                     SqlDbType.VarChar, 1)
                    {
                        Value = temp.term_group
                    });
                    cmd.Parameters.Add(new SqlParameter("@cheque",
                    SqlDbType.VarChar, 1)
                    {
                        Value = temp.FMCC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                    SqlDbType.VarChar)
                    {
                        Value = temp.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                    SqlDbType.VarChar)
                    {
                        Value = temp.AMSC_Id
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }
                        temp.installmentwisedata = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }


            }
            else
            {



                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "collection_report_New_1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    //cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                    //    SqlDbType.VarChar)
                    //{
                    //    Value = maxcol1
                    //});
                    cmd.Parameters.Add(new SqlParameter("@fmg_id",
                       SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = fmt_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(temp.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                   SqlDbType.VarChar)
                    {
                        Value = temp.reporttype
                    });
                    cmd.Parameters.Add(new SqlParameter("@option",
                       SqlDbType.VarChar)
                    {
                        Value = temp.radioval
                    });
                    cmd.Parameters.Add(new SqlParameter("@active",
                  SqlDbType.BigInt)
                    {
                        Value = temp.active
                    });
                    cmd.Parameters.Add(new SqlParameter("@deactive",
                  SqlDbType.BigInt)
                    {
                        Value = temp.deactive
                    });
                    cmd.Parameters.Add(new SqlParameter("@left",
                  SqlDbType.BigInt)
                    {
                        Value = temp.left
                    });

                    cmd.Parameters.Add(new SqlParameter("@date1",
               SqlDbType.DateTime)
                    {
                        Value = temp.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@date2",
               SqlDbType.DateTime)
                    {
                        Value = temp.To_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                    {
                        Value = temp.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@term_group",
                     SqlDbType.VarChar, 1)
                    {
                        Value = temp.term_group
                    });
                    cmd.Parameters.Add(new SqlParameter("@cheque",
                    SqlDbType.VarChar, 1)
                    {
                        Value = temp.FMCC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                    SqlDbType.VarChar)
                    {
                        Value = temp.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                    SqlDbType.VarChar)
                    {
                        Value = temp.AMSC_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

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
                        if (temp.radioval == "FSW")
                        {
                            temp.studentalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FGW")
                        {
                            temp.groupalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FHW")
                        {
                            temp.headalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FCW")
                        {
                            temp.classalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "CW")
                        {
                            temp.categorydata = retObject.ToArray();
                        }
                        else if (temp.radioval == "TRMW")
                        {
                            temp.classwisedata = retObject.ToArray();
                        }
                        else if (temp.radioval == "INMW")
                        {
                            temp.installmentwisedata = retObject.ToArray();
                        }
                        else if (temp.radioval == "STRMW")
                        {
                            temp.classwisedata = retObject.ToArray();
                        }
                        else if (temp.radioval == "CTW")
                        {
                            temp.classalldatainscon = retObject.ToArray();
                        }

                        else if (temp.radioval == "CTC")
                        {
                            temp.classalldatainsconN = retObject.ToArray();
                        }
                        else if (temp.radioval == "SWTPC")
                        {
                            temp.SWTPCount = retObject.ToArray();
                        }

                        else if (temp.radioval == "CWTPC")
                        {
                            temp.CWTPCount = retObject.ToArray();
                        }

                        else if (temp.radioval == "FPW")
                        {
                            temp.studentalldata = retObject.ToArray();
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }
            }




        }


        public FeeTransactionPaymentDTO get_groups(FeeTransactionPaymentDTO data)
        {
            try
            {
                if (data.reporttype.Equals("T"))
                {
                    //data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                    //                        from b in _FeeGroupContext.feeTr
                    //                        where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID) /*&& a.fmg_id.Contains(data.fmg_id)*/
                    //                        select new FeeStudentTransactionDTO
                    //                        {
                    //                            FMT_Name = b.FMT_Name,
                    //                            FMT_Id = a.FMT_Id,
                    //                        }
                    //     ).Distinct().ToArray();

                    //List<FeeTransactionPaymentDTO> customlist = new List<FeeTransactionPaymentDTO>();

                    //data.customlist = (from a in _FeeGroupContext.feegm
                    //                   from b in _FeeGroupContext.feeGGG
                    //                   from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                    //                   where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_GroupCode == "1" && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID)
                    //                   select new FeeStudentTransactionDTO
                    //                   {
                    //                       FMGG_Id = a.FMGG_Id,
                    //                       fmg_groupname = a.FMGG_GroupName
                    //                   }
                    //     ).Distinct().ToArray();


                    // data.customlist = customlist.ToArray();
                    //List<long> grpid = new List<long>();

                    //foreach (FeeStudentTransactionDTO item in data.customlist)
                    //{
                    //    grpid.Add(item.FMGG_Id);
                    //}

                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && data.FMGG_Ids.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                      ).Distinct().ToArray();


                }
                else
                {
                    //data.customlist = (from a in _FeeGroupContext.feegm
                    //                   from b in _FeeGroupContext.feeGGG
                    //                   from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                    //                   where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_GroupCode == "1" && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID)
                    //                   select new FeeStudentTransactionDTO
                    //                   {
                    //                       FMGG_Id = a.FMGG_Id,
                    //                       fmg_groupname = a.FMGG_GroupName
                    //                   }
                    // ).Distinct().ToArray();


                    // data.customlist = customlist.ToArray();
                    //List<long> grpid = new List<long>();

                    //foreach (FeeStudentTransactionDTO item in data.customlist)
                    //{
                    //    grpid.Add(item.FMGG_Id);
                    //}

                    data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                      from b in _FeeGroupContext.feeGGG
                                      from c in _FeeGroupContext.feegm
                                          //where (a.FMG_Id==b.FMG_Id.Equals(data.customlist.Equals()))
                                      where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && data.FMGG_Ids.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
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
    }
}