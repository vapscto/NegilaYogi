using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeAccountsPositionReportImpl:Interfaces.CollegeAccountsPositionReportInterface
    {

        public CollFeeGroupContext _ClgContext;
        readonly ILogger<CollegeAccountsPositionReportImpl> _logger;
        public CollegeAccountsPositionReportImpl(CollFeeGroupContext ClgContext, ILogger<CollegeAccountsPositionReportImpl> log)
        {
            _logger = log;
            _ClgContext = ClgContext;

        }
        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)
        {
           // CollegeConcessionDTO obj = new CollegeConcessionDTO();
            try
            {
                data.yearlst = _ClgContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();
                data.courselist = _ClgContext.MasterCourseDMO.Where(d => d.MI_Id == data.MI_Id && d.AMCO_ActiveFlag == true).OrderBy(d => d.AMCO_Order).ToArray();
                data.branchlist = _ClgContext.ClgMasterBranchDMO.Where(d => d.MI_Id == data.MI_Id && d.AMB_ActiveFlag == true).OrderBy(d => d.AMB_Order).ToArray();
                data.semisterlist = _ClgContext.CLG_Adm_Master_SemesterDMO.Where(d => d.MI_Id == data.MI_Id && d.AMSE_ActiveFlg == true).OrderBy(d => d.AMSE_SEMOrder).ToArray();


                data.customgrpList = (from a in _ClgContext.FeeGroupMappingDMO
                                     from b in _ClgContext.FeeGroupGroupingDMO 
                                     from c in _ClgContext.FEeGroupLoginPreviledgeDMO
                                     where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                     select new CollegeConcessionDTO
                                     {
                                         FMGG_Id = a.FMGG_Id,
                                         FMG_GroupName = a.FMGG_GroupName
                                     }
                                        ).Distinct().ToArray();


                // data.customlist = customlist.ToArray();
                List<long> grpid = new List<long>();

                foreach (CollegeConcessionDTO item in data.customgrpList)
                {
                    grpid.Add(item.FMGG_Id);
                }

                data.grouplist = (from a in _ClgContext.FeeGroupClgDMO
                                 from b in _ClgContext.FeeGroupGroupingDMO
                                 from c in _ClgContext.FeeGroupMappingDMO
                                 where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
                                 select new CollegeConcessionDTO
                                 {
                                     FMG_Id = a.FMG_Id,
                                     FMG_GroupName = a.FMG_GroupName
                                 }
                                  ).Distinct().ToArray();


                //if (query.Count > 0)
                //{
                //    obj.groupList = query.ToArray();
                //}
                var feeconfig = _ClgContext.feemastersettings.Where(d => d.MI_Id == data.MI_Id).ToList();


                if (feeconfig.Count > 0)
                {
                    data.feeconfiguration = feeconfig.ToArray();
                    if (feeconfig.FirstOrDefault().FMC_GroupOrTermFlg.Equals("T"))
                    {

                        data.termsList = (from a in _ClgContext.feeMTH
                                         from b in _ClgContext.feeTr
                                         from c in _ClgContext.FEeGroupLoginPreviledgeDMO
                                         where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id && c.User_Id == data.User_Id)
                                         select new CollegeConcessionDTO
                                         {
                                             FMT_Name = b.FMT_Name,
                                             FMT_Id = a.FMT_Id,
                                         }
                      ).Distinct().ToArray();
                        //  obj.termsList = _ClgContext.feeTr.Where(d => d.MI_Id == id && d.FMT_ActiveFlag == true).ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CollegeConcessionDTO getgroupByCG(CollegeConcessionDTO data)
        {
            try
            {

                var fmggIds = data.selectedCGList.Select(d => d.FMGG_Id).ToList();
             
                data.grouplist = (from a in _ClgContext.FeeGroupClgDMO
                                  from b in _ClgContext.FeeGroupGroupingDMO
                                  from c in _ClgContext.FeeGroupMappingDMO
                                  where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && fmggIds.Contains(c.FMGG_Id) && a.MI_Id == data.MI_Id)
                                  select new CollegeConcessionDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                                 ).Distinct().ToArray();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CollegeConcessionDTO getReport(CollegeConcessionDTO data)
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
                        foreach (CollegeConcessionDTO actv in data.selectedCGList)
                        {
                            fmgg_id = fmgg_id + "," + actv.FMGG_Id;
                        }
                    }
                    if (data.selectedGroup != null)
                    {
                        foreach (CollegeConcessionDTO actv in data.selectedGroup)
                        {
                            fmg_id = fmg_id + "," + actv.FMG_Id;
                        }
                    }

                    foreach (var x in data.FMT_Ids)
                    {
                        fmt_ids += x + ",";
                    }
                    if (fmt_ids.Length>0)
                    {
                        fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

                    }

                    if (data.asondate == null)
                    {
                        using (var cmd = _ClgContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 500000;
                            cmd.CommandText = "Fee_DetailedAccountPositionCollege_AsonDate";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            
                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                              SqlDbType.BigInt)
                                {
                                    Value = 0
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                                      SqlDbType.BigInt)
                                {
                                    Value = 0
                                });
                             cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                              SqlDbType.BigInt)
                                {
                                    Value = 0
                                });
                                cmd.Parameters.Add(new SqlParameter("@FMGG_Id",
                                      SqlDbType.BigInt)
                                {
                                    Value = fmgg_id
                                });
                            
                            cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                SqlDbType.VarChar)
                            {
                                Value = fmg_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmg_id",
                               SqlDbType.VarChar)
                            {
                                Value = fmg_id
                            });
                            if (data.Date != null)
                            {
                                var d = data.Date.Date.ToString("dd-MM-yyyy");

                                cmd.Parameters.Add(new SqlParameter("@Date",
                                                       SqlDbType.VarChar)
                                {
                                    Value = d
                                });
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@Date",
                                                      SqlDbType.VarChar)
                                {
                                    Value = ""
                                });
                            }

                            if (data.Fromdate != null)
                            {
                                var d = data.Fromdate.Value.Date.ToString("dd-MM-yyyy");
                                cmd.Parameters.Add(new SqlParameter("@Fromdate",
                           SqlDbType.VarChar)
                                {
                                    Value = d
                                });
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@Fromdate",
                           SqlDbType.VarChar)
                                {
                                    Value = ""
                                });
                            }
                            if (data.Todate != null)
                            {
                                var d = data.Todate.Value.Date.ToString("dd-MM-yyyy");
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

                            cmd.Parameters.Add(new SqlParameter("@FTI_Id",
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
                                var du = data.asondate.Date.ToString("dd-MM-yyyy");

                                cmd.Parameters.Add(new SqlParameter("@AsOnduedate",
                                                       SqlDbType.VarChar)
                                {
                                    Value = data.asondate
                                });
                            }

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            List<CollegeConcessionDTO> result = new List<CollegeConcessionDTO>();
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
                                                result.Add(new CollegeConcessionDTO
                                                {
                                                    admNo = dataReader["admno"].ToString(),
                                                    studentname = dataReader["StudentName"].ToString(),
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
                                            result.Add(new CollegeConcessionDTO
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
                                            result.Add(new CollegeConcessionDTO
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
                                            result.Add(new CollegeConcessionDTO
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
                        using (var cmd = _ClgContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 500000;
                            cmd.CommandText = "Fee_DetailedAccountPositionCollege_AsonDate";
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
                          
                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                              SqlDbType.VarChar)
                                {
                                    Value = data.AMCO_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                                      SqlDbType.VarChar)
                                {
                                    Value =data.AMB_Id
                                });
                            
                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                              SqlDbType.VarChar)
                                {
                                    Value = data.AMSE_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@FMGG_Id",
                                      SqlDbType.VarChar)
                                {
                                    Value = fmgg_id
                                });
                            
                            cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                SqlDbType.VarChar)
                            {
                                Value = fmg_id
                            });
                          
                            if (data.Date != null)
                            {
                                var d = data.Date.Date.ToString("dd-MM-yyyy");

                                cmd.Parameters.Add(new SqlParameter("@Date",
                                                       SqlDbType.VarChar)
                                {
                                    Value = d
                                });
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@Date",
                                                      SqlDbType.VarChar)
                                {
                                    Value = ""
                                });
                            }

                            if (data.Fromdate != null)
                            {
                                var d = data.Fromdate.Value.Date.ToString("dd-MM-yyyy");
                                cmd.Parameters.Add(new SqlParameter("@Fromdate",
                           SqlDbType.VarChar)
                                {
                                    Value = d
                                });
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@Fromdate",
                           SqlDbType.VarChar)
                                {
                                    Value = ""
                                });
                            }
                            if (data.Todate != null)
                            {
                                var d = data.Todate.Value.Date.ToString("dd-MM-yyyy");
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

                            cmd.Parameters.Add(new SqlParameter("@FTI_Id",
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
                                var du = data.asondate.Date.ToString("dd-MM-yyyy");

                                cmd.Parameters.Add(new SqlParameter("@AsOnduedate",
                                                       SqlDbType.VarChar)
                                {
                                    Value = data.asondate
                                });
                            }

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            List<CollegeConcessionDTO> result = new List<CollegeConcessionDTO>();
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
                                                    result.Add(new CollegeConcessionDTO
                                                    {
                                                        admNo = dataReader["admno"].ToString(),
                                                        studentname = dataReader["StudentName"].ToString(),
                                                         charges= dataReader["Charges"].ToString(),
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
                                                    result.Add(new CollegeConcessionDTO
                                                    {
                                                        admNo = dataReader["admno"].ToString(),
                                                        studentname = dataReader["StudentName"].ToString(),
                                                        charges = dataReader["Charges"].ToString(),
                                                        concession = dataReader["Concession"].ToString(),
                                                        rebate = dataReader["Rebate/Schlorship"].ToString(),
                                                        waiveOff = dataReader["Waive Off"].ToString(),
                                                        fine = dataReader["Fine"].ToString(),
                                                        collection = Convert.ToInt32(dataReader["Collection"].ToString()),
                                                        PFY_EndDate_DebitBalance = dataReader["Debit Balance"].ToString(),
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
                                            result.Add(new CollegeConcessionDTO
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
                                            result.Add(new CollegeConcessionDTO
                                            {
                                                className = dataReader["AMCO_CourseName"].ToString()+"  "+ dataReader["AMB_BranchName"].ToString(),
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
                                            result.Add(new CollegeConcessionDTO
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

                                    data.studentdata = result.ToArray();
                                    if (data.studentdata.Length > 0)
                                    {
                                        data.count = data.studentdata.Length;
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
