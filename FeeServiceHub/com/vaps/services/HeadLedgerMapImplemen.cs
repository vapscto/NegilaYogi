using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees.Tally;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.Fee.Tally;

namespace FeeServiceHub.com.vaps.services
{
    public class HeadLedgerMapImplemen : interfaces.FeeHeadLedMapInter
    {
        public FeeGroupContext _FeeGroupContext;
        public HeadLedgerMapImplemen(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public HeadLedgerCodeMapDTO deletedata(HeadLedgerCodeMapDTO data)
        {
            try
            {
                List<HeadLedgerMappingDMO> lorg = new List<HeadLedgerMappingDMO>();
                lorg = _FeeGroupContext.HeadLedgerMappingDMO.Where(t => t.FYGHLM_Id == data.FYGHLM_Id).ToList();

                if (lorg.Count > 0)
                {
                    try
                    {
                        if (lorg.Any())
                        {
                            _FeeGroupContext.Remove(lorg.ElementAt(0));
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = "true";
                            }
                            else
                            {
                                data.returnval = "false";
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HeadLedgerCodeMapDTO getgroupdetails(HeadLedgerCodeMapDTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _FeeGroupContext.FeeGroupDMO
                                        from b in _FeeGroupContext.Yearlygroups
                                        where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true)
                                        select new HeadLedgerCodeMapDTO
                                        {
                                            FMG_GroupName = a.FMG_GroupName,
                                            FMG_Id = a.FMG_Id
                                        }
 ).ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HeadLedgerCodeMapDTO getheaddetails(HeadLedgerCodeMapDTO data)
        {
            try
            {
                data.fillheaddata = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from b in _FeeGroupContext.FeeHeadDMO
                                  where (a.FMH_Id==b.FMH_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && b.FMH_ActiveFlag==true && a.FMG_Id==data.FMG_Id)
                                  select new HeadLedgerCodeMapDTO
                                  {
                                      FMH_FeeName = b.FMH_FeeName,
                                      FYGHM_Id = a.FYGHM_Id,
                                      FMH_Id = a.FMH_Id
                                  }
      ).ToArray();

                List<HeadLedgerCodeMapDTO> savedlist = new List<HeadLedgerCodeMapDTO>();
                savedlist = (from a in _FeeGroupContext.HeadLedgerMappingDMO
                                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FeeHeadDMO
                                  where (a.FYGHM_Id == b.FYGHM_Id && b.FMH_Id == c.FMH_Id && c.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_Id==data.FMG_Id)
                                  select new HeadLedgerCodeMapDTO
                                  {
                                      FMH_Id = b.FMH_Id,
                                      FYGHM_Id =b.FYGHM_Id,
                                      FYGHLM_Id = a.FYGHLM_Id,
                                      FMH_FeeName = c.FMH_FeeName,
                                      FYGHM_RVRegLedgerId = a.FYGHM_RVRegLedgerId,
                                      FYGHM_RVRegLedgerUnder = a.FYGHM_RVRegLedgerUnder,
                                      FYGHM_RVAdvanceLegderId = a.FYGHM_RVAdvanceLegderId,
                                      FYGHM_RVAdvanceLegderUnder = a.FYGHM_RVAdvanceLegderUnder,
                                      FYGHM_RVArrearLedgerId = a.FYGHM_RVArrearLedgerId,
                                      FYGHM_RVArrearLedgerUnder = a.FYGHM_RVArrearLedgerUnder,

                                      FYGHM_JVRegLedgerId = a.FYGHM_JVRegLedgerId,
                                      FYGHM_JVRegLedgerUnder = a.FYGHM_JVRegLedgerUnder,
                                      FYGHM_JVAdvanceLegderId = a.FYGHM_JVAdvanceLegderId,
                                      FYGHM_JVAdvanceLegderUnder = a.FYGHM_JVAdvanceLegderUnder,
                                      FYGHM_JVArrearLedgerId = a.FYGHM_JVArrearLedgerId,
                                      FYGHM_JVArrearLedgerUnder = a.FYGHM_JVArrearLedgerUnder
                                  }
       ).ToList();

                data.totaldata = savedlist.ToArray();

                List<long> savlist = new List<long>();
                foreach (var item in savedlist)
                {
                    savlist.Add(item.FYGHM_Id);
                }

                data.savednotsavedlist = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                     from b in _FeeGroupContext.FeeHeadDMO
                                     where (a.FMH_Id == b.FMH_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_ActiveFlag == true && a.FMG_Id == data.FMG_Id && !savlist.Contains(a.FYGHM_Id))
                                     select new HeadLedgerCodeMapDTO
                                     {
                                         FMH_FeeName = b.FMH_FeeName,
                                         FYGHM_Id = a.FYGHM_Id,
                                         FMH_Id = a.FMH_Id
                                     }
      ).ToArray();
                //company name
                data.FTMCOM_companyname = (from a in _FeeGroupContext.Fee_Tally_Master_CompanyDMO
                                           from b in _FeeGroupContext.Fee_FeeGroup_CompanyMappingDMO
                                           from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                           where (a.FTMCOM_Id == b.FTMCOM_Id && b.FMG_Id == c.FMG_Id && b.FMG_Id == data.FMG_Id && b.FFGCMA_ActiveId==true)
                                           select new Fee_Tally_Master_CompanyDTO
                                           {
                                               FTMCOM_Id = a.FTMCOM_Id,
                                               FTMCOM_CompanyName = a.FTMCOM_CompanyName,
                                               FTMCOM_CompanyCode = a.FTMCOM_CompanyCode,
                                           }

                                         ).Distinct().ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HeadLedgerCodeMapDTO loaddata(HeadLedgerCodeMapDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.academicdrp = allyear.Distinct().ToArray();

                data.fillmastergroup = (from a in _FeeGroupContext.FeeGroupDMO
                                     from b in _FeeGroupContext.Yearlygroups
                                     where (a.FMG_Id==b.FMG_Id && a.MI_Id==data.MI_Id && b.ASMAY_Id==data.ASMAY_Id && a.FMG_ActiceFlag==true)
                                     select new HeadLedgerCodeMapDTO
                                     {
                                         FMG_GroupName = a.FMG_GroupName,
                                         FMG_Id = a.FMG_Id
                                     }
   ).ToArray();

                List<FeeMasterConfigurationDMO> config = new List<FeeMasterConfigurationDMO>();
                config = _FeeGroupContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.fillconfig = config.ToArray();


                data.totaldata = (from a in _FeeGroupContext.HeadLedgerMappingDMO
                                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FeeHeadDMO
                                  from d in _FeeGroupContext.FeeGroupDMO
                                  where ( d.FMG_Id==b.FMG_Id && a.FYGHM_Id==b.FYGHM_Id && b.FMH_Id==c.FMH_Id && c.MI_Id==data.MI_Id && b.ASMAY_Id==data.ASMAY_Id && c.FMH_ActiveFlag==true)
                                  select new HeadLedgerCodeMapDTO
                                  {
                                      FMH_FeeName=c.FMH_FeeName,

                                      FMG_GroupName = d.FMG_GroupName,

                                      FYGHM_Id=b.FYGHM_Id,
                                      FYGHLM_Id=a.FYGHLM_Id,

                                      FYGHM_RVRegLedgerId =a.FYGHM_RVRegLedgerId,
                                      FYGHM_RVRegLedgerUnder=a.FYGHM_RVRegLedgerUnder,
                                      FYGHM_RVAdvanceLegderId=a.FYGHM_RVAdvanceLegderId,
                                      FYGHM_RVAdvanceLegderUnder=a.FYGHM_RVAdvanceLegderUnder,
                                      FYGHM_RVArrearLedgerId=a.FYGHM_RVArrearLedgerId,
                                      FYGHM_RVArrearLedgerUnder=a.FYGHM_RVArrearLedgerUnder,

                                      FYGHM_JVRegLedgerId=a.FYGHM_JVRegLedgerId,
                                      FYGHM_JVRegLedgerUnder=a.FYGHM_JVRegLedgerUnder,
                                      FYGHM_JVAdvanceLegderId=a.FYGHM_JVAdvanceLegderId,
                                      FYGHM_JVAdvanceLegderUnder=a.FYGHM_JVAdvanceLegderUnder,
                                      FYGHM_JVArrearLedgerId=a.FYGHM_JVArrearLedgerId,
                                      FYGHM_JVArrearLedgerUnder=a.FYGHM_JVArrearLedgerUnder
                                  }
       ).ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HeadLedgerCodeMapDTO savedata(HeadLedgerCodeMapDTO pgmod)
        {
            try
            {
                

                int j = 0;
                while (j < pgmod.savetmpdata.Count())
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Insert_head_ledger_Mapping";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FYGHLM_Id",
                           SqlDbType.BigInt)
                        {
                            Value =  (pgmod.savetmpdata[j].FYGHLM_Id==0) ? 0: pgmod.savetmpdata[j].FYGHLM_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FYGHM_Id",
                            SqlDbType.BigInt)
                        {
                            Value = (pgmod.savetmpdata[j].FYGHM_Id==0) ?0: pgmod.savetmpdata[j].FYGHM_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FYGHM_RVRegLedgerId",
                           SqlDbType.NVarChar)
                        {
                            Value = (pgmod.savetmpdata[j].FYGHM_RVRegLedgerId==null)?"" : pgmod.savetmpdata[j].FYGHM_RVRegLedgerId
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_RVRegLedgerUnder",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_RVRegLedgerUnder==null?"":pgmod.savetmpdata[j].FYGHM_RVRegLedgerUnder
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_RVAdvanceLegderId",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_RVAdvanceLegderId==null?"": pgmod.savetmpdata[j].FYGHM_RVAdvanceLegderId
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_RVAdvanceLegderUnder",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_RVAdvanceLegderUnder==null?"": pgmod.savetmpdata[j].FYGHM_RVAdvanceLegderUnder
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_RVArrearLedgerId",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_RVArrearLedgerId==null?"": pgmod.savetmpdata[j].FYGHM_RVArrearLedgerId
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_RVArrearLedgerUnder",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_RVArrearLedgerUnder==null?"": pgmod.savetmpdata[j].FYGHM_RVArrearLedgerUnder
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_JVRegLedgerId",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_JVRegLedgerId==null?"": pgmod.savetmpdata[j].FYGHM_JVRegLedgerId
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_JVRegLedgerUnder",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_JVRegLedgerUnder==null?"": pgmod.savetmpdata[j].FYGHM_JVRegLedgerUnder
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_JVAdvanceLegderId",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_JVAdvanceLegderId==null?"" : pgmod.savetmpdata[j].FYGHM_JVAdvanceLegderId
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_JVAdvanceLegderUnder",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_JVAdvanceLegderUnder ==null?"": pgmod.savetmpdata[j].FYGHM_JVAdvanceLegderUnder
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_JVArrearLedgerId",
                           SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_JVArrearLedgerId==null?"": pgmod.savetmpdata[j].FYGHM_JVArrearLedgerId
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYGHM_JVArrearLedgerUnder",
                        SqlDbType.NVarChar)
                        {
                            Value = pgmod.savetmpdata[j].FYGHM_JVArrearLedgerUnder==null?"": pgmod.savetmpdata[j].FYGHM_JVArrearLedgerUnder
                        });
                        //FTMCOM_Id
                        cmd.Parameters.Add(new SqlParameter("@FTMCOM_Id",
                        SqlDbType.BigInt)
                        {
                            Value = pgmod.FTMCOM_Id
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data = cmd.ExecuteNonQuery();

                        if (data >= 1)
                        {
                            pgmod.returnval = "true";
                        }
                        else
                        {
                            pgmod.returnval = "false";
                        }
                    }
                    j++;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pgmod;
        }
    }
}
