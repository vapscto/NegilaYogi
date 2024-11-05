
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
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
using CommonLibrary;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;
using SendGrid;
using Newtonsoft.Json;
using System.Text;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeDailyCollectionImpl : interfaces.FeeDailyCollectionInterface
    {

        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _dbsms;

        // private readonly UserManager<ApplicationUser> _UserManager;

        public FeeDailyCollectionImpl(FeeGroupContext FeeGroupContext, DomainModelMsSqlServerContext _db)
        {
            _FeeGroupContext = FeeGroupContext;
            //_UserManager = UserManager;
            _dbsms = _db;
        }

        // public FeeTransactionPaymentDTO mas = new FeeTransactionPaymentDTO();
        public DailyCollectionReportDTO getdetails(DailyCollectionReportDTO dt)
        {
            DailyCollectionReportDTO data = new DailyCollectionReportDTO();
            try
            {
                //List<MasterAcademic> year = new List<MasterAcademic>();
                //year = _FeeGroupContext.AcademicYear.Where(y=>y.Is_Active==true && y.MI_Id==dt.MI_ID && y.ASMAY_Id==dt.yearid).ToList();
                //dt.fillyear = year.GroupBy(y=>y.ASMAY_Year).Select(y=>y.First()).ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == dt.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                dt.fillyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _FeeGroupContext.admissioncls.Where(c => c.ASMCL_ActiveFlag == true && c.MI_Id == dt.MI_ID).ToList();
                dt.fillclass = classname.GroupBy(c => c.ASMCL_ClassName).Select(c => c.First()).ToArray();


                dt.fillfeegroup = (from a in _FeeGroupContext.FeeGroupDMO
                                   from b in _FeeGroupContext.Yearlygroups
                                   from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                   where (a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_ID && b.MI_Id == dt.MI_ID && c.User_Id == dt.userid && b.ASMAY_Id == dt.yearid)
                                   select new DailyCollectionReportDTO
                                   {
                                       FMG_Id = a.FMG_Id,
                                       fmG_GroupName = a.FMG_GroupName,
                                       FMG_Order = a.FMG_Order,
                                   }
                    ).OrderBy(t => t.FMG_Order).Distinct().ToArray();


                List<FeeHeadDMO> feelisthead = new List<FeeHeadDMO>();
                feelisthead = _FeeGroupContext.feehead.Where(h => h.FMH_ActiveFlag == true && h.MI_Id == dt.MI_ID).ToList();
                dt.fillfeehead = feelisthead.ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }
        public DailyCollectionReportDTO getgroupmappedheads(DailyCollectionReportDTO dto)
        {

            List<long> GrpId = new List<long>();
            try
            {
                //foreach (var item in dto.TempararyArrayList)
                //{
                //    GrpId.Add(item.FMG_Id);
                //}
                //dto.alldata = (from a in _FeeGroupContext.FeeGroupDMO
                //               from b in _FeeGroupContext.FeeHeadDMO
                //               from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                //               from d in _FeeGroupContext.FeeAmountEntryDMO
                //               from e in _FeeGroupContext.FeeTransactionPaymentDMO
                //               from f in _FeeGroupContext.FeePaymentDetailsDMO

                //               where (a.FMG_Id == c.FMG_Id && b.FMH_Id == c.FMH_Id && c.MI_Id == dto.mid && c.ASMAY_Id == dto.ASMAY_Id && GrpId.Contains(c.FMG_Id) && a.FMG_Id==d.FMG_Id && b.FMH_Id==d.FMH_Id && d.ASMAY_Id==dto.ASMAY_Id && d.MI_Id==dto.mid && d.FMA_Id==e.FMA_Id && e.FYP_Id==f.FYP_Id && f.FYP_Date>=dto.Fromdate && f.FYP_Date<=dto.Todate)
                //               select new DailyCollectionReportDTO
                //               {
                //                   FMH_Id = b.FMH_Id,
                //                   FMH_FeeName = b.FMH_FeeName,

                //               }
                //    ).Distinct().OrderBy(h => h.FMH_FeeName).ToArray();

                string fmgid = "0";
                foreach (var item in dto.TempararyArrayList)
                {
                    //    GrpId.Add(item.FMG_Id);
                    fmgid += "," + item.FMG_Id;
                }

                DateTime fdate = Convert.ToDateTime(dto.Fromdate);
                string frmdate = fdate.ToString("yyyy-MM-dd");
                DateTime tdate = Convert.ToDateTime(dto.Todate);
                string todate = tdate.ToString("yyyy-MM-dd");

                using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "FeeheadSelection";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 90000000;
                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.mid
                    });
                    cmd1.Parameters.Add(new SqlParameter("@FMG_Id",
                    SqlDbType.VarChar)
                    {
                        Value = fmgid
                    });

                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = frmdate
                    });
                    cmd1.Parameters.Add(new SqlParameter("@Todate",
                    SqlDbType.VarChar)
                    {
                        Value = todate
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd1.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.alldata = retObject1.ToArray();
                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                dto.studentlist = _FeeGroupContext.feespecialHead.Where(y => y.FMSFH_ActiceFlag == true && y.MI_Id == dto.mid).Distinct().ToArray();

                dto.allgroupheaddata = (from a in _FeeGroupContext.feespecialHead
                                        from b in _FeeGroupContext.feeSGGG
                                        where (a.FMSFH_Id == b.FMSFH_Id && a.MI_Id == dto.mid && a.FMSFH_ActiceFlag == true && b.FMSFHFH_ActiceFlag == true)
                                        select new DailyCollectionReportDTO
                                        {
                                            FMSFHFH_Id = b.FMSFHFH_Id,
                                            FMH_Id = b.FMH_Id,
                                            FMSFH_Id = b.FMSFH_Id
                                        }
                       ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return dto;
        }
        public DailyCollectionReportDTO getgroupheadsid(DailyCollectionReportDTO dto)
        {
            List<string> headId = new List<string>();
            List<long> GrpIdhead = new List<long>();
            try
            {
                foreach (var item in dto.TempararyArrayheadList)
                {
                    headId.Add(item.FMH_FeeName);
                    GrpIdhead.Add(item.FMH_Id);
                }
                dto.alldatahead = (
                               from b in _FeeGroupContext.FeeHeadDMO
                               where (GrpIdhead.Contains(b.FMH_Id))

                               select new DailyCollectionReportDTO
                               {
                                   FMH_Id = b.FMH_Id,
                                   FMH_FeeName = b.FMH_FeeName,

                               }
                   ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public async Task<DailyCollectionReportDTO> Getreportdetails(DailyCollectionReportDTO dto)
        {

            //st james requirement
            DateTime fdate = Convert.ToDateTime(dto.Fromdate);
            string frmdate = fdate.ToString("yyyy-MM-dd");
            DateTime tdate = Convert.ToDateTime(dto.Todate);
            string todate = tdate.ToString("yyyy-MM-dd");

            if (dto.headwise == true && dto.paymentwise == false)
            {
                using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Fee_DateWiseTotalAmount";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 90000000;
                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.mid
                    });

                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.AMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = frmdate
                    });
                    cmd1.Parameters.Add(new SqlParameter("@Todate",
                    SqlDbType.VarChar)
                    {
                        Value = todate
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.headwisecollection = retObject1.ToArray();
                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else if (dto.paymentwise == true)
            {
                using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Fee_PaymentModeWiseTotalAmount";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 90000000;
                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.mid
                    });

                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.AMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = frmdate
                    });
                    cmd1.Parameters.Add(new SqlParameter("@Todate",
                    SqlDbType.VarChar)
                    {
                        Value = todate
                    });


                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.headwisecollection = retObject1.ToArray();
                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }





            }
            else
            {
                string name = "";
                string coloumns = "";
                long cls;

                string IVRM_CLM_coloumn = "";
                for (int i = 0; i < dto.All_List.Length; i++)
                {
                    name = dto.All_List[i].columnID;
                    if (name != null)
                    {
                        IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                    }
                }


                coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
                // coloumns = "10219,10218";



                if (dto.classflag == "1")
                {
                    cls = dto.classid;
                }
                else
                {
                    cls = 0;
                }






                dto.studentlist = _FeeGroupContext.feespecialHead.Where(y => y.FMSFH_ActiceFlag == true && y.MI_Id == dto.mid).Distinct().ToArray();

                dto.allgroupheaddata = (from a in _FeeGroupContext.feespecialHead
                                        from b in _FeeGroupContext.feeSGGG
                                        where (a.FMSFH_Id == b.FMSFH_Id && a.MI_Id == dto.mid && a.FMSFH_ActiceFlag == true && b.FMSFHFH_ActiceFlag == true)
                                        select new DailyCollectionReportDTO
                                        {
                                            FMSFHFH_Id = b.FMSFHFH_Id,
                                            FMH_Id = b.FMH_Id,
                                            FMSFH_Id = b.FMSFH_Id
                                        }
                       ).Distinct().ToArray();


                try
                {


                    if ((dto.regornamedetails == "students") && (dto.allorstdorothersflag == "multiplepaymentmode"))

                    {
                        using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Student_Multiple_PaymentMode";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.CommandTimeout = 90000000;
                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = dto.AMAY_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                            SqlDbType.BigInt)
                            {
                                Value = dto.mid
                            });

                            cmd1.Parameters.Add(new SqlParameter("@from_date",
                             SqlDbType.DateTime)
                            {
                                Value = dto.Fromdate
                            });
                            cmd1.Parameters.Add(new SqlParameter("@to_date",
                             SqlDbType.DateTime)
                            {
                                Value = dto.Todate
                            });

                            cmd1.Parameters.Add(new SqlParameter("@fmg_id",
                              SqlDbType.VarChar)
                            {
                                Value = coloumns

                            });

                            if (cmd1.Connection.State != ConnectionState.Open)
                                cmd1.Connection.Open();

                            var retObject1 = new List<dynamic>();
                            try
                            {
                                using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                                {
                                    while (await dataReader1.ReadAsync())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader1.GetName(iFiled1),
                                                dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                            );
                                        }

                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                dto.alllist = retObject1.ToArray();
                            }


                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    else if ((dto.regornamedetails == "students") || (dto.regornamedetails == "All") && (dto.allorstdorothersflag != "multiplepaymentmode"))

                    {
                        using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Daily_Collection_Report_All_1";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.CommandTimeout = 90000000;
                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = dto.AMAY_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                            SqlDbType.BigInt)
                            {
                                Value = dto.mid
                            });

                            cmd1.Parameters.Add(new SqlParameter("@from_date",
                             SqlDbType.DateTime)
                            {
                                Value = dto.Fromdate
                            });
                            cmd1.Parameters.Add(new SqlParameter("@to_date",
                             SqlDbType.DateTime)
                            {
                                Value = dto.Todate
                            });
                            cmd1.Parameters.Add(new SqlParameter("@asmcl_id",
                            SqlDbType.VarChar)
                            {
                                Value = cls

                            });
                            cmd1.Parameters.Add(new SqlParameter("@fmg_id",
                              SqlDbType.VarChar)
                            {
                                Value = coloumns

                            });
                            cmd1.Parameters.Add(new SqlParameter("@type",
                              SqlDbType.VarChar)
                            {
                                Value = dto.allorindivflag
                            });
                            cmd1.Parameters.Add(new SqlParameter("@done_by",
                              SqlDbType.VarChar)
                            {
                                Value = dto.allorstdorothersflag
                            });
                            cmd1.Parameters.Add(new SqlParameter("@trans_by",
                              SqlDbType.VarChar)
                            {
                                Value = dto.allorcorchoronlineflag
                            });
                            cmd1.Parameters.Add(new SqlParameter("@cheque",
                                SqlDbType.BigInt)
                            {
                                Value = dto.cheque
                            });
                            cmd1.Parameters.Add(new SqlParameter("@userid",
                               SqlDbType.BigInt)
                            {
                                Value = dto.userid
                            });
                            cmd1.Parameters.Add(new SqlParameter("@datetype",
                               SqlDbType.VarChar)
                            {
                                Value = dto.TempararyArrayListstring
                            });


                            cmd1.Parameters.Add(new SqlParameter("@acdyr",
                          SqlDbType.VarChar)
                            {
                                Value = dto.regornamedetails
                            });

                            cmd1.Parameters.Add(new SqlParameter("@yrflag",
                         SqlDbType.VarChar)
                            {
                                Value = dto.groupflag
                            });


                            if (cmd1.Connection.State != ConnectionState.Open)
                                cmd1.Connection.Open();

                            var retObject1 = new List<dynamic>();
                            try
                            {
                                using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                                {
                                    while (await dataReader1.ReadAsync())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader1.GetName(iFiled1),
                                                dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                            );
                                        }

                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                dto.alllist = retObject1.ToArray();
                            }


                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {

                        using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Daily_Collection_Report_All_staffothers";
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = dto.AMAY_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                            SqlDbType.BigInt)
                            {
                                Value = dto.mid
                            });

                            cmd1.Parameters.Add(new SqlParameter("@from_date",
                             SqlDbType.DateTime)
                            {
                                Value = dto.Fromdate
                            });
                            cmd1.Parameters.Add(new SqlParameter("@to_date",
                             SqlDbType.DateTime)
                            {
                                Value = dto.Todate
                            });
                            cmd1.Parameters.Add(new SqlParameter("@asmcl_id",
                            SqlDbType.VarChar)
                            {
                                Value = cls

                            });
                            cmd1.Parameters.Add(new SqlParameter("@fmg_id",
                              SqlDbType.VarChar)
                            {
                                Value = coloumns

                            });
                            cmd1.Parameters.Add(new SqlParameter("@type",
                              SqlDbType.VarChar)
                            {
                                Value = dto.allorindivflag
                            });
                            cmd1.Parameters.Add(new SqlParameter("@done_by",
                              SqlDbType.VarChar)
                            {
                                Value = dto.allorstdorothersflag
                            });
                            cmd1.Parameters.Add(new SqlParameter("@trans_by",
                              SqlDbType.VarChar)
                            {
                                Value = dto.allorcorchoronlineflag
                            });
                            cmd1.Parameters.Add(new SqlParameter("@cheque",
                                SqlDbType.BigInt)
                            {
                                Value = dto.cheque
                            });
                            cmd1.Parameters.Add(new SqlParameter("@userid",
                               SqlDbType.BigInt)
                            {
                                Value = dto.userid
                            });
                            cmd1.Parameters.Add(new SqlParameter("@option",
                               SqlDbType.VarChar)
                            {
                                Value = dto.regornamedetails
                            });
                            if (cmd1.Connection.State != ConnectionState.Open)
                                cmd1.Connection.Open();

                            var retObject1 = new List<dynamic>();
                            try
                            {
                                using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                                {
                                    while (await dataReader1.ReadAsync())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader1.GetName(iFiled1),
                                                dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                            );
                                        }

                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                dto.alllist = retObject1.ToArray();
                            }


                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return dto;
        }
        public DailyCollectionReportDTO getdata(DailyCollectionReportDTO data)
        {
            if (data.ASMAY_Id == 0)
            {
                data.fillfeegroup = (from a in _FeeGroupContext.FeeGroupDMO

                                     from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                     where (a.FMG_Id == c.FMG_ID && a.MI_Id == data.mid && c.User_Id == data.userid && c.MI_ID == data.mid)
                                     select new DailyCollectionReportDTO
                                     {
                                         FMG_Id = a.FMG_Id,
                                         fmG_GroupName = a.FMG_GroupName,
                                         FMG_Order = a.FMG_Order,
                                     }
                   ).OrderBy(t => t.FMG_Order).Distinct().ToArray();
            }
            else
            {

                data.fillfeegroup = (from a in _FeeGroupContext.FeeGroupDMO
                                     from b in _FeeGroupContext.Yearlygroups
                                     from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                     where (a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_ID && b.MI_Id == data.mid && c.User_Id == data.userid && b.ASMAY_Id == data.ASMAY_Id)
                                     select new DailyCollectionReportDTO
                                     {
                                         FMG_Id = a.FMG_Id,
                                         fmG_GroupName = a.FMG_GroupName,
                                         FMG_Order = a.FMG_Order,
                                     }
                      ).OrderBy(t => t.FMG_Order).Distinct().ToArray();
            }
            return data;
        }
        public async Task<DailyCollectionReportDTO> FeeAccountDetailsReport(DailyCollectionReportDTO data)
        {
            using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "FeeAccountDetailsReport";
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                SqlDbType.VarChar)
                {
                    Value = data.mid
                });
                cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                SqlDbType.VarChar)
                {
                    Value = data.AMAY_Id
                });

                cmd1.Parameters.Add(new SqlParameter("@from_date",
                 SqlDbType.DateTime)
                {
                    Value = data.Fromdate
                });
                cmd1.Parameters.Add(new SqlParameter("@to_date",
                 SqlDbType.DateTime)
                {
                    Value = data.Todate
                });

                cmd1.Parameters.Add(new SqlParameter("@userid",
                   SqlDbType.BigInt)
                {
                    Value = data.userid
                });

                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject1 = new List<dynamic>();
                try
                {
                    using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                    {
                        while (await dataReader1.ReadAsync())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                );
                            }

                            retObject1.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.alllist = retObject1.ToArray();
                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public void ChairmanSMS(DailyCollectionReportDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //var mobileno = (from a in _dbsms.Institution_MobileNo
                //                where (a.MI_Id == data.MI_ID)
                //                select new DailyCollectionReportDTO
                //                {
                //                    MIMN_MobileNo = a.MIMN_MobileNo
                //                }
                //      ).Distinct().ToArray();

                //var Emailid = (from a in _dbsms.Institution_EmailId
                //                where (a.MI_Id == data.MI_ID)
                //                select new DailyCollectionReportDTO
                //                {
                //                    MIE_EmailId = a.MIE_EmailId
                //                }
                //     ).Distinct().ToArray();

                //SMS sms = new SMS(_dbsms);

                //sendSms(data.MI_ID, Convert.ToInt64(mobileno.FirstOrDefault().MIMN_MobileNo), data.TemplateString, data.userid);
                sendSms(data.MI_ID, data.MIMN_MobileNo, data.TemplateString, 0);

                //Email email = new Email(_dbsms);

                //sendmail(data.MI_ID, Convert.ToString(Emailid.FirstOrDefault().MIE_EmailId), data.TemplateString , data.userid);
                sendmail(data.MI_ID, data.MIE_EmailId, data.TemplateString, 0);

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _dbsms.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _dbsms.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _dbsms.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _dbsms.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _dbsms.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                        SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _dbsms.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _dbsms.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

                        //var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        //if (img.Count > 0)
                        //{
                        // for (int i = 0; i < img.Count; i++)
                        // {
                        // if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                        // {
                        // var webClient = new WebClient();
                        // byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                        // string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                        // message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                        // }
                        // }
                        //}

                        var img = _dbsms.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }

                        if (template[0].ISES_EnableMailCCFlg == true && template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');
                            foreach (var c in ccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddCc(c);
                                }
                            }
                        }
                        if (template[0].ISES_EnableMailBCCFlg == true && template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] bccmaildetails = template[0].ISES_MailBCCId.Split(',');
                            foreach (var c in bccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddBcc(c);
                                }
                            }
                        }
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };
                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;
                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                if (Attechement.Equals("1"))
                                {
                                    var img = _dbsms.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }
                                if (template[0].ISES_EnableMailCCFlg == true && template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            emailMessage.CC.Add(c);
                                        }
                                    }
                                }
                                if (template[0].ISES_EnableMailBCCFlg == true && template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    string[] bccmaildetails = template[0].ISES_MailBCCId.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            emailMessage.Bcc.Add(c);
                                        }
                                    }
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }
                    }
                    using (var cmd = _dbsms.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _dbsms.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _dbsms.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _dbsms.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                        SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                        SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _dbsms.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }
                var institutionName = _dbsms.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _dbsms.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _dbsms.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string sms = template.FirstOrDefault().ISES_SMSMessage;
                string result = sms;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _dbsms.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                        SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                sms = result;
                            }
                        }
                    }
                    sms = result;
                }
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _dbsms.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _dbsms.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = mobileNo.ToString();
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    if (template[0].ISES_EnableSMSCCFlg == true && template[0].ISES_SMSCCMobileNo != "" && template[0].ISES_SMSCCMobileNo != null)
                    {
                        string[] ccmobileno = template[0].ISES_SMSCCMobileNo.Split(',');
                        foreach (var c in ccmobileno)
                        {
                            if (c != PHNO)
                            {
                                string urlcc = alldetails[0].IVRMSD_URL.ToString();
                                string PHNOcc = c.ToString();
                                urlcc = urlcc.Replace("PHNO", PHNOcc);
                                urlcc = urlcc.Replace("MESSAGE", sms);

                                urlcc = urlcc.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                                urlcc = urlcc.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                                System.Net.HttpWebRequest requestcc = System.Net.WebRequest.Create(urlcc) as HttpWebRequest;
                                System.Net.HttpWebResponse responsecc = await requestcc.GetResponseAsync() as System.Net.HttpWebResponse;
                                Stream streamcc = responsecc.GetResponseStream();
                                StreamReader readStreamcc = new StreamReader(streamcc, Encoding.UTF8);
                                string responseparameterscc = readStreamcc.ReadToEnd();
                                var myContentcc = JsonConvert.SerializeObject(responseparameterscc);
                                dynamic responsedatacc = JsonConvert.DeserializeObject(myContentcc);
                            }
                        }

                    }

                    using (var cmd = _dbsms.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _dbsms.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _dbsms.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _dbsms.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                        SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                        SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@status",
                        SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                        SqlDbType.VarChar)
                        {
                            Value = messageid
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


        public async Task<DailyCollectionReportDTO> UserWisereportdetails(DailyCollectionReportDTO dto)
        {

            DateTime fdate = Convert.ToDateTime(dto.Fromdate);
            string frmdate = fdate.ToString("yyyy-MM-dd");
            DateTime tdate = Convert.ToDateTime(dto.Todate);
            string todate = tdate.ToString("yyyy-MM-dd");

            string name = "";
            string coloumns = "";

            string IVRM_CLM_coloumn = "";
            for (int i = 0; i < dto.All_List.Length; i++)
            {
                name = dto.All_List[i].columnID;
                if (name != null)
                {
                    IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                }
            }

            coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "UserWise_DailyFeeCollection_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300000;

                cmd.Parameters.Add(new SqlParameter("@MI_ID",
                   SqlDbType.BigInt)
                {
                    Value = dto.mid
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                 SqlDbType.BigInt)
                {
                    Value = dto.AMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                SqlDbType.VarChar)
                {
                    Value = coloumns
                });

                cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                 SqlDbType.Date)
                {
                    Value = frmdate
                });
                cmd.Parameters.Add(new SqlParameter("@TODATE",
                 SqlDbType.Date)
                {
                    Value = todate
                });             
                cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.VarChar)
                {
                    Value = dto.regornamedetails
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
                    if (dto.regornamedetails == "STW")
                    {
                        dto.studentalldata = retObject.ToArray();
                    }
                    else if (dto.regornamedetails == "TCW")
                    {
                        dto.totalcollection = retObject.ToArray();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return dto;
            }

        }


        public async Task<DailyCollectionReportDTO> getreport(DailyCollectionReportDTO dto)
        {
            DateTime fdate = Convert.ToDateTime(dto.Fromdate);
            string frmdate = fdate.ToString("yyyy-MM-dd");
            DateTime tdate = Convert.ToDateTime(dto.Todate);
            string todate = tdate.ToString("yyyy-MM-dd");

            try
            {
                using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "usp_s_DailyCollectionforVidya";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 90000000;
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.AMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.mid
                    });

                    cmd1.Parameters.Add(new SqlParameter("@FromDate",
                     SqlDbType.DateTime)
                    {
                        Value = dto.Fromdate
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.DateTime)
                    {
                        Value = dto.Todate
                    });                                 
                    cmd1.Parameters.Add(new SqlParameter("@PaymentMode",
                      SqlDbType.VarChar)
                    {
                        Value = dto.allorcorchoronlineflag
                    });
                                     
                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.dailycollreport = retObject1.ToArray();
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
            return dto;
        }
    }
}


