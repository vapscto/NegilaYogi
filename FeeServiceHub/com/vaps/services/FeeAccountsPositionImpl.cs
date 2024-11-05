using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeAccountsPositionImpl : interfaces.FeeAccountsPositionInterface
    {
        FeeGroupContext _feecontext;
        public FeeAccountsPositionImpl(FeeGroupContext feecontext)
        {
            _feecontext = feecontext;
        }
        public FeeAccountsPositionReportDTO getdata(FeeAccountsPositionReportDTO data)
        {
            FeeAccountsPositionReportDTO obj = new FeeAccountsPositionReportDTO();
            try
           {
                obj.academicYearList = _feecontext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).OrderByDescending(d=>d.ASMAY_Order).ToArray();
                obj.classList = _feecontext.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToArray();
                obj.sectionList = _feecontext.school_M_Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToArray();
                //obj.customgrpList = _feecontext.feegm.Where(d => d.MI_Id == data.MI_Id && d.FMGG_ActiveFlag == true && ).ToArray();
                obj.financialyear = _feecontext.IVRM_Master_FinancialYear.OrderBy(d => d.IMFY_OrderBy).ToArray();

                //var query = (from m in _feecontext.feeGGG
                //             from n in _feecontext.feegm
                //             from o in _feecontext.FeeGroupDMO
                //             where m.FMGG_Id == n.FMGG_Id && m.FMG_Id == o.FMG_Id && n.MI_Id == data.MI_Id && o.FMG_ActiceFlag == true
                //             select new FeeDemandRegisterDTO
                //             {
                //                 FMG_Id = o.FMG_Id,
                //                 FMG_GroupName = o.FMG_GroupName
                //             }).Distinct().ToList();

                obj.customgrpList = (from a in _feecontext.feegm
                                   from b in _feecontext.feeGGG
                                   from c in _feecontext.FEeGroupLoginPreviledgeDMO
                                   where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_Id && c.User_Id == data.User_Id)
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMGG_Id = a.FMGG_Id,
                                       fmg_groupname = a.FMGG_GroupName
                                   }
                                        ).Distinct().ToArray();


                // data.customlist = customlist.ToArray();
                List<long> grpid = new List<long>();

                foreach (FeeStudentTransactionDTO item in obj.customgrpList)
                {
                    grpid.Add(item.FMGG_Id);
                }

                obj.groupList = (from a in _feecontext.FeeGroupDMO
                                  from b in _feecontext.feeGGG
                                  from c in _feecontext.feegm
                                  where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                                  ).Distinct().ToArray();

                
                //if (query.Count > 0)
                //{
                //    obj.groupList = query.ToArray();
                //}
                var feeconfig = _feecontext.feemastersettings.Where(d => d.MI_Id == data.MI_Id).ToList();


                if (feeconfig.Count > 0)
                {
                    obj.feeconfiguration = feeconfig.ToArray();
                    if (feeconfig.FirstOrDefault().FMC_GroupOrTermFlg.Equals("T"))
                    {

                        obj.termsList = (from a in _feecontext.feeMTH
                                         from b in _feecontext.feeTr
                                         from c in _feecontext.FEeGroupLoginPreviledgeDMO
                                         where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id && c.User_Id == data.User_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FMT_Name = b.FMT_Name,
                                             FMT_Id = a.FMT_Id,
                                         }
                      ).Distinct().ToArray();
                      //  obj.termsList = _feecontext.feeTr.Where(d => d.MI_Id == id && d.FMT_ActiveFlag == true).ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public FeeAccountsPositionReportDTO getgroupByCG(FeeAccountsPositionReportDTO data)
        {
            try
            {

                var fmggIds = data.selectedCGList.Select(d => d.FMGG_Id).ToList();
                //var query = (from m in _feecontext.feeGGG
                //             from n in _feecontext.feegm
                //             from o in _feecontext.FeeGroupDMO
                //             where m.FMGG_Id == n.FMGG_Id && m.FMG_Id == o.FMG_Id && n.MI_Id == data.MI_Id && o.FMG_ActiceFlag == true &&
                //             fmggIds.Contains(m.FMGG_Id)
                //             select new FeeAccountsPositionReportDTO
                //             {
                //                 FMG_Id = o.FMG_Id,
                //                 FMG_GroupName = o.FMG_GroupName
                //             }).Distinct().ToList();




                //var query = (from a in _feecontext.feegm
                //             from b in _feecontext.feeGGG
                //             from c in _feecontext.FEeGroupLoginPreviledgeDMO
                //             where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_Id && c.User_Id == data.User_Id &&
                //             fmggIds.Contains(b.FMGG_Id))
                //             select new FeeDemandRegisterDTO
                //             {
                //                 FMGG_Id = a.FMGG_Id,
                //                 FMG_GroupName = a.FMGG_GroupName
                //             }).Distinct().ToList();

                data.groupList = (from a in _feecontext.FeeGroupDMO
                                 from b in _feecontext.feeGGG
                                 from c in _feecontext.feegm
                                 where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && fmggIds.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_Id = a.FMG_Id,
                                     FMG_GroupName = a.FMG_GroupName
                                 }
                                 ).Distinct().ToArray();



                //if (query.Count > 0)
                //{
                //    data.groupList = query.ToArray();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeAccountsPositionReportDTO getReport(FeeAccountsPositionReportDTO data)
        {
            try
            {
                string fmgg_id = "0";
                string fmg_id = "0";
                var fmt_ids = "";
                try
                {
                    if (data.selectedCGList != null)
                    {
                        foreach (FeeAccountsPositionReportDTO actv in data.selectedCGList)
                        {
                            fmgg_id = fmgg_id + "," + actv.FMGG_Id;
                        }
                    }
                    if (data.selectedGroup != null)
                    {
                        foreach (FeeAccountsPositionReportDTO actv in data.selectedGroup)
                        {
                            fmg_id = fmg_id + "," + actv.FMG_Id;
                        }
                    }

                    foreach (var x in data.FMT_Ids)
                    {
                        fmt_ids += x + ",";
                    }
                    fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

                    if (data.yeartype == "academic")
                    {
                        if (data.asondate == null)
                        {
                            using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 500000;
                                cmd.CommandText = "Fee_DetailedAccountPosition";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mi_id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@asmay_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                if (data.type.Equals("individual"))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = data.ASMCL_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = data.ASMS_Id
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }


                                cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                                    SqlDbType.VarChar)
                                {
                                    Value = fmgg_id
                                });
                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                   SqlDbType.VarChar)
                                {
                                    Value = fmg_id
                                });
                                if (data.Date != null)
                                {
                                    var d = data.Date.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                           SqlDbType.VarChar)
                                    {
                                        Value = d
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                          SqlDbType.VarChar)
                                    {
                                        Value = ""
                                    });
                                }

                                if (data.FromDate != null)
                                {
                                    var d = data.FromDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                                    {
                                        Value = d
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                                    {
                                        Value = ""
                                    });
                                }
                                if (data.ToDate != null)
                                {
                                    var d = data.ToDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                                                         SqlDbType.VarChar)
                                    {
                                        Value = d

                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                             SqlDbType.VarChar)
                                    {
                                        Value = ""
                                    });
                                }
                                cmd.Parameters.Add(new SqlParameter("@Type",
                             SqlDbType.VarChar)
                                {
                                    Value = data.type
                                });

                                cmd.Parameters.Add(new SqlParameter("@fmt_id",
                                  SqlDbType.VarChar)
                                {
                                    Value = fmt_ids
                                });

                                cmd.Parameters.Add(new SqlParameter("@status",
                                SqlDbType.VarChar)
                                {
                                    Value = data.Status
                                });


                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                List<FeeAccountsPositionReportDTO> result = new List<FeeAccountsPositionReportDTO>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        if (data.type.Equals("individual"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                try
                                                {
                                                    result.Add(new FeeAccountsPositionReportDTO
                                                    {
                                                        admNo = dataReader["admno"].ToString(),
                                                        studentName = dataReader["StudentName"].ToString(),
                                                        charges = dataReader["Charges"].ToString(),
                                                        concession = dataReader["Concession"].ToString(),
                                                        rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                        waiveOff = dataReader["Waive Off"].ToString(),
                                                        fine = dataReader["Fine"].ToString(),
                                                        collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                        debitBalance = dataReader["Debit Balance"].ToString(),
                                                        lastYearDue = dataReader["Last Year Due"].ToString()

                                                    });
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }

                                            }
                                        }
                                        else if (data.type.Equals("headwise"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    FeeName = dataReader["FeeName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }
                                        else if (data.type.Equals("All"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    className = dataReader["ClassName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        else
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    RouteName = dataReader["RouteName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        data.feeaccountsPositionReport = result.ToArray();
                                        if (data.feeaccountsPositionReport.Length > 0)
                                        {
                                            data.count = data.feeaccountsPositionReport.Length;
                                        }
                                        else
                                        {
                                            data.count = 0;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        else
                        {
                            using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 500000;
                                cmd.CommandText = "Fee_DetailedAccountPosition_AsonDate_25Aug2021";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mi_id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@asmay_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                if (data.type.Equals("individual"))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = data.ASMCL_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = data.ASMS_Id
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }


                                cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                                    SqlDbType.VarChar)
                                {
                                    Value = fmgg_id
                                });
                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                   SqlDbType.VarChar)
                                {
                                    Value = fmg_id
                                });
                                if (data.Date != null)
                                {
                                    var d = data.Date.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                           SqlDbType.VarChar)
                                    {
                                        Value = d
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                          SqlDbType.VarChar)
                                    {
                                        Value = ""
                                    });
                                }

                                if (data.FromDate != null)
                                {
                                    var d = data.FromDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                                    {
                                        Value = d
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                                    {
                                        Value = ""
                                    });
                                }
                                if (data.ToDate != null)
                                {
                                    var d = data.ToDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                                                         SqlDbType.VarChar)
                                    {
                                        Value = d

                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                             SqlDbType.VarChar)
                                    {
                                        Value = ""
                                    });
                                }
                                cmd.Parameters.Add(new SqlParameter("@Type",
                             SqlDbType.VarChar)
                                {
                                    Value = data.type
                                });

                                cmd.Parameters.Add(new SqlParameter("@fmt_id",
                                  SqlDbType.VarChar)
                                {
                                    Value = fmt_ids
                                });

                                cmd.Parameters.Add(new SqlParameter("@status",
                                SqlDbType.VarChar)
                                {
                                    Value = data.Status
                                });

                                if (data.asondate != null)
                                {
                                    var du = data.asondate.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@asonduedate",
                                                           SqlDbType.VarChar)
                                    {
                                        Value = du
                                    });
                                }


                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                List<FeeAccountsPositionReportDTO> result = new List<FeeAccountsPositionReportDTO>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        if (data.type.Equals("individual"))
                                        {
                                            if (data.asondate == null)
                                            {
                                                while (dataReader.Read())
                                                {
                                                    try
                                                    {
                                                        result.Add(new FeeAccountsPositionReportDTO
                                                        {
                                                            admNo = dataReader["admno"].ToString(),
                                                            studentName = dataReader["StudentName"].ToString(),
                                                            charges = dataReader["Charges"].ToString(),
                                                            concession = dataReader["Concession"].ToString(),
                                                            rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                            waiveOff = dataReader["Waive Off"].ToString(),
                                                            fine = dataReader["Fine"].ToString(),
                                                            collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                            debitBalance = dataReader["Debit Balance"].ToString(),
                                                            lastYearDue = dataReader["Last Year Due"].ToString()

                                                        });
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                while (dataReader.Read())
                                                {
                                                    try
                                                    {
                                                        result.Add(new FeeAccountsPositionReportDTO
                                                        {
                                                            admNo = dataReader["admno"].ToString(),
                                                            studentName = dataReader["StudentName"].ToString(),
                                                            charges = dataReader["Charges"].ToString(),
                                                            concession = dataReader["Concession"].ToString(),
                                                            rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                            waiveOff = dataReader["Waive Off"].ToString(),
                                                            fine = dataReader["Fine"].ToString(),
                                                            collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                            PFY_EndDate_DebitBalance = dataReader["PFY_EndDate_DebitBalance"].ToString(),
                                                            lastYearDue = dataReader["Last Year Due"].ToString(),

                                                            CFY_PaidAmount = Convert.ToInt32(dataReader["CFY_PaidAmount"].ToString()),
                                                            CFY_BalanceAmount = dataReader["CFY_BalanceAmount"].ToString(),
                                                            ExcessAmount = dataReader["ExcessAmount"].ToString()

                                                        });
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }

                                        }
                                        else if (data.type.Equals("headwise"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    FeeName = dataReader["FeeName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }
                                        else if (data.type.Equals("All"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    className = dataReader["ClassName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        else
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    RouteName = dataReader["RouteName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        data.feeaccountsPositionReport = result.ToArray();
                                        if (data.feeaccountsPositionReport.Length > 0)
                                        {
                                            data.count = data.feeaccountsPositionReport.Length;
                                        }
                                        else
                                        {
                                            data.count = 0;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                    else if (data.yeartype == "financial")
                    {
                        string fromdate = "", todate = "", date = "", asondate = "";

                        if (data.FromDate != null)
                        {
                       
                            fromdate = data.FromDate.Value.ToString("dd-MM-yyyy");

                        }
                        if (data.ToDate != null)
                        {
                            todate = data.ToDate.Value.ToString("dd-MM-yyyy");


                        }
                        if (data.Date != null)
                        {
                            date = data.Date.Value.ToString("dd-MM-yyyy");


                        }
                        if (data.asondate != null)
                        {
                            asondate = data.asondate.Value.ToString("dd-MM-yyyy");


                        }

                        if (data.asondate == null)
                        {
                           
                            using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 500000;
                                cmd.CommandText = "Fee_DetailedAccountPosition_AsonDate_FY_temp";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mi_id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@asmay_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                if (data.type.Equals("individual"))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = data.ASMCL_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = data.ASMS_Id
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }


                                cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                                    SqlDbType.VarChar)
                                {
                                    Value = fmgg_id
                                });
                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                   SqlDbType.VarChar)
                                {
                                    Value = fmg_id
                                });
                                if (data.Date != null)
                                {
                                    //date = data.Date.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                           SqlDbType.VarChar)
                                    {
                                        Value = date
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                          SqlDbType.VarChar)
                                    {
                                        Value = date
                                    });
                                }

                                if (data.FromDate != null)
                                {
                                 //   fromdate = data.FromDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                                    {
                                        Value = fromdate
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.VarChar)
                                    {
                                        Value =fromdate
                                    });
                                }
                                if (data.ToDate != null)
                                {
                                   // var d = data.ToDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                                                         SqlDbType.VarChar)
                                    {
                                        Value =todate

                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                             SqlDbType.VarChar)
                                    {
                                        Value = todate
                                    });
                                }
                                cmd.Parameters.Add(new SqlParameter("@Type",
                             SqlDbType.VarChar)
                                {
                                    Value = data.type
                                });

                                cmd.Parameters.Add(new SqlParameter("@fmt_id",
                                  SqlDbType.VarChar)
                                {
                                    Value = fmt_ids
                                });

                                cmd.Parameters.Add(new SqlParameter("@status",
                                SqlDbType.VarChar)
                                {
                                    Value = data.Status
                                });
                                if (data.asondate != null)
                                {
                                    //asondate = data.asondate.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@asonduedate",
                                                           SqlDbType.VarChar)
                                    {
                                        Value = asondate
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asonduedate",
                             SqlDbType.VarChar)
                                    {
                                        Value = asondate
                                    });
                                }


                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                List<FeeAccountsPositionReportDTO> result = new List<FeeAccountsPositionReportDTO>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        if (data.type.Equals("individual"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                try
                                                {
                                                    result.Add(new FeeAccountsPositionReportDTO
                                                    {
                                                        admNo = dataReader["admno"].ToString(),
                                                        studentName = dataReader["StudentName"].ToString(),
                                                        charges = dataReader["Charges"].ToString(),
                                                        concession = dataReader["Concession"].ToString(),
                                                        rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                        waiveOff = dataReader["Waive Off"].ToString(),
                                                        fine = dataReader["Fine"].ToString(),
                                                        collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                        debitBalance = dataReader["Debit Balance"].ToString(),
                                                        lastYearDue = dataReader["Last Year Due"].ToString()

                                                    });
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }

                                            }
                                        }
                                        else if (data.type.Equals("headwise"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    FeeName = dataReader["FeeName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }
                                        else if (data.type.Equals("All"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    className = dataReader["ClassName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        else
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    RouteName = dataReader["RouteName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        data.feeaccountsPositionReport = result.ToArray();
                                        if (data.feeaccountsPositionReport.Length > 0)
                                        {
                                            data.count = data.feeaccountsPositionReport.Length;
                                        }
                                        else
                                        {
                                            data.count = 0;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        else
                        {
                            using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandTimeout = 500000;
                                cmd.CommandText = "Fee_DetailedAccountPosition_AsonDate_FY_temp";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mi_id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@asmay_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                if (data.type.Equals("individual"))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = data.ASMCL_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = data.ASMS_Id
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@amsc_id",
                                          SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }


                                cmd.Parameters.Add(new SqlParameter("@fmgg_id",
                                    SqlDbType.VarChar)
                                {
                                    Value = fmgg_id
                                });
                                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                   SqlDbType.VarChar)
                                {
                                    Value = fmg_id
                                });
                                if (data.Date != null)
                                {
                                   // var d = data.Date.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                           SqlDbType.DateTime)
                                    {
                                        Value = date
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@date",
                                                          SqlDbType.DateTime)
                                    {
                                        Value = date
                                    });
                                }

                                if (data.FromDate != null)
                                {
                                   // var d = data.FromDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.DateTime)
                                    {
                                        Value = fromdate
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.DateTime)
                                    {
                                        Value = fromdate
                                    });
                                }
                                if (data.ToDate != null)
                                {
                                    // todate = data.ToDate.Value.Date.ToString("dd-MM-yyyy");
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                            SqlDbType.DateTime)
                                    {
                                        Value = todate
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@todate",
                             SqlDbType.DateTime)
                                    {
                                        Value = todate
                                    });
                                }
                                cmd.Parameters.Add(new SqlParameter("@Type",
                             SqlDbType.VarChar)
                                {
                                    Value = data.type
                                });

                                cmd.Parameters.Add(new SqlParameter("@fmt_id",
                                  SqlDbType.VarChar)
                                {
                                    Value = fmt_ids
                                });

                                cmd.Parameters.Add(new SqlParameter("@status",
                                SqlDbType.VarChar)
                                {
                                    Value = data.Status
                                });

                                if (data.asondate != null)
                                {
                                   // asondate = data.asondate.Value.Date.ToString("dd-MM-yyyy");

                                    cmd.Parameters.Add(new SqlParameter("@asonduedate",
                                                           SqlDbType.DateTime)
                                    {
                                        Value = asondate
                                        //Value = du
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@asonduedate",
                             SqlDbType.DateTime)
                                    {
                                        Value = asondate
                                    });
                                }


                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                List<FeeAccountsPositionReportDTO> result = new List<FeeAccountsPositionReportDTO>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        if (data.type.Equals("individual"))
                                        {
                                            if (data.asondate == null)
                                            {
                                                while (dataReader.Read())
                                                {
                                                    try
                                                    {
                                                        result.Add(new FeeAccountsPositionReportDTO
                                                        {
                                                            admNo = dataReader["admno"].ToString(),
                                                            studentName = dataReader["StudentName"].ToString(),
                                                            charges = dataReader["Charges"].ToString(),
                                                            concession = dataReader["Concession"].ToString(),
                                                            rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                            waiveOff = dataReader["Waive Off"].ToString(),
                                                            fine = dataReader["Fine"].ToString(),
                                                            collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                            debitBalance = dataReader["Debit Balance"].ToString(),
                                                            lastYearDue = dataReader["Last Year Due"].ToString()

                                                        });
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                while (dataReader.Read())
                                                {
                                                    try
                                                    {
                                                        result.Add(new FeeAccountsPositionReportDTO
                                                        {
                                                            admNo = dataReader["admno"].ToString(),
                                                            studentName = dataReader["StudentName"].ToString(),
                                                            charges = dataReader["Charges"].ToString(),
                                                            concession = dataReader["Concession"].ToString(),
                                                            rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                            waiveOff = dataReader["Waive Off"].ToString(),
                                                            fine = dataReader["Fine"].ToString(),
                                                            collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                            //PFY_EndDate_DebitBalance = dataReader["PFY_EndDate_DebitBalance"].ToString(),
                                                            lastYearDue = dataReader["Last Year Due"].ToString(),

                                                            //CFY_PaidAmount = Convert.ToInt32(dataReader["CFY_PaidAmount"].ToString()),
                                                            //CFY_BalanceAmount = dataReader["CFY_BalanceAmount"].ToString(),
                                                            //ExcessAmount = dataReader["ExcessAmount"].ToString()

                                                        });
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }

                                        }
                                        else if (data.type.Equals("headwise"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    FeeName = dataReader["FeeName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }
                                        else if (data.type.Equals("All"))
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    className = dataReader["ClassName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        else
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new FeeAccountsPositionReportDTO
                                                {
                                                    RouteName = dataReader["RouteName"].ToString(),
                                                    charges = dataReader["Charges"].ToString(),
                                                    concession = dataReader["Concession"].ToString(),
                                                    rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                    waiveOff = dataReader["Waive Off"].ToString(),
                                                    fine = dataReader["Fine"].ToString(),
                                                    collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                    debitBalance = dataReader["Debit Balance"].ToString(),
                                                    lastYearDue = dataReader["Last Year Due"].ToString()

                                                });

                                            }
                                        }

                                        data.feeaccountsPositionReport = result.ToArray();
                                        if (data.feeaccountsPositionReport.Length > 0)
                                        {
                                            data.count = data.feeaccountsPositionReport.Length;
                                        }
                                        else
                                        {
                                            data.count = 0;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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
