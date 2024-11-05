//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using PreadmissionDTOs;
//using DataAccessMsSqlServerProvider;
//using FeeServiceHub.com.vaps.interfaces;
//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using DomainModel.Model;
//using System.Collections.Concurrent;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using System.Data;
//using System.Data.SqlClient;
//using PreadmissionDTOs.com.vaps.Fees;
//using DomainModel.Model.com.vaps.Fee;
//using Microsoft.Extensions.Logging;

//namespace FeeServiceHub.com.vaps.services
//{
//    public class YearlyFeeGroupMappingImpl : interfaces.
//    {
//        private static ConcurrentDictionary<string, FeeYearlygroupHeadMappingDTO> _login =
//            new ConcurrentDictionary<string, FeeYearlygroupHeadMappingDTO>();

//        public FeeGroupContext _YearlyFeeGroupMappingContext;
//        readonly ILogger<YearlyFeeGroupMappingImpl> _logger;
//        public YearlyFeeGroupMappingImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<YearlyFeeGroupMappingImpl> log)
//        {
//            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
//            _logger = log;
//        }

//        public FeeYearlygroupHeadMappingDTO deleterec(int id)
//        {
//            bool returnresult = false;
//            FeeYearlygroupHeadMappingDTO page = new FeeYearlygroupHeadMappingDTO();
//            List<FeeYearlygroupHeadMappingDMO> lorg = new List<FeeYearlygroupHeadMappingDMO>();
//            lorg = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FYGHM_Id.Equals(id)).ToList();

//            try
//            {
//                if (lorg.Any())
//                {
//                    _YearlyFeeGroupMappingContext.Remove(lorg.ElementAt(0));

//                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
//                    if (contactExists == 1)
//                    {
//                        returnresult = true;
//                        page.returnval = returnresult;
//                    }
//                    else
//                    {
//                        returnresult = false;
//                        page.returnval = returnresult;
//                    }
//                }

//                page.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == 10 && a.MI_Id == 2 && a.FMI_Id == d.FMI_Id)
//                                 select new FeeYearlygroupHeadMappingDTO
//                                 {
//                                     FYGHM_Id = a.FYGHM_Id,
//                                     FMG_GroupName = b.FMG_GroupName,
//                                     FMH_FeeName = c.FMH_FeeName,
//                                     FMI_Name = d.FMI_Name,
//                                 }
//        ).ToArray();

//            }
//            catch (Exception ee)
//            {
//                Console.WriteLine(ee.Message);
//            }
//            return page;
//        }

//        public FeeYearlygroupHeadMappingDTO EditMasterscetionDetails(FeeYearlygroupHeadMappingDTO fee)
//        {
//            try
//            {
//                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && a.FYGHM_Id==fee.FYGHM_Id)
//                               select new FeeYearlygroupHeadMappingDTO
//                               {
//                                   FMH_Id=a.FMH_Id,
//                                   FMG_Id=a.FMG_Id,
//                                   FYGHM_Id = a.FYGHM_Id,
//                                   FMG_GroupName = b.FMG_GroupName,
//                                   FMH_FeeName = c.FMH_FeeName,
//                                   FMI_Name = d.FMI_Name,
//                                   FMI_Id=d.FMI_Id,
//                                   FYGHM_FineApplicableFlag=a.FYGHM_FineApplicableFlag,
//                                   FYGHM_Common_AmountFlag=a.FYGHM_Common_AmountFlag,
//                                   FYGHM_ActiveFlag=a.FYGHM_ActiveFlag,
//                               }
//       ).ToArray();
//            }

//            catch (Exception ee)
//            {
//                Console.WriteLine(ee.Message);
//            }

//            return fee;
//        }

//        public FeeYearlygroupHeadMappingDTO getdata(FeeYearlygroupHeadMappingDTO fee)
//        {
//            try
//            {
//                List<FeeHeadDMO> head = new List<FeeHeadDMO>();
//                head =  _YearlyFeeGroupMappingContext.FeeHeadDMO.Where(t=>t.MI_Id==fee.MI_Id).ToList();
//                fee.fillmasterhead = head.ToArray();

//                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
//                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t=>t.FMG_ActiceFlag==true && t.MI_Id==fee.MI_Id).ToList();
//                fee.fillmastergroup = group.ToArray();

//                List<MasterCompanyDMO> company = new List<MasterCompanyDMO>();
//                company = _YearlyFeeGroupMappingContext.MasterCompanyDMO.ToList();
//                fee.fillcompany = company.ToArray();

//                List<FeeInstallmentDMO> installment = new List<FeeInstallmentDMO>();
//                installment = _YearlyFeeGroupMappingContext.FeeInstallmentDMO.Where(t=>t.MI_Id==fee.MI_Id).ToList();
//                fee.fillinstallment = installment.ToArray();

//                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id)
//                                 select new FeeYearlygroupHeadMappingDTO
//                                 {
//                                     FYGHM_Id = a.FYGHM_Id,
//                                     FMG_GroupName = b.FMG_GroupName,
//                                     FMH_FeeName = c.FMH_FeeName,
//                                     FMI_Name = d.FMI_Name,
//                                 }
//       ).ToArray();
//            }
         
//            catch (Exception ee)
//            {
//                Console.WriteLine(ee.Message);
//            }

//            return fee;

//        }

//        public FeeYearlygroupHeadMappingDTO getdataongroup(FeeYearlygroupHeadMappingDTO fee)
//        {
//            try
//            {
//                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id  && a.FMG_Id==fee.FMG_Id)
//                               select new FeeYearlygroupHeadMappingDTO
//                               {
//                                   FYGHM_Id = a.FYGHM_Id,
//                                   FMG_GroupName = b.FMG_GroupName,
//                                   FMH_FeeName = c.FMH_FeeName,
//                                   FMI_Name = d.FMI_Name,
//                                   FMH_Id=c.FMH_Id,
//                                   FMI_Id=d.FMI_Id,
//                                   FYGHM_ActiveFlag=a.FYGHM_ActiveFlag,
//                                   FYGHM_Common_AmountFlag=a.FYGHM_Common_AmountFlag,
//                                   FYGHM_FineApplicableFlag=a.FYGHM_FineApplicableFlag
//                               }
//          ).ToArray();

//                if(fee.alldata.Length<=0)
//                {
//                    List<MasterCompanyDMO> company = new List<MasterCompanyDMO>();
//                    company = _YearlyFeeGroupMappingContext.MasterCompanyDMO.ToList();
//                    fee.alldata = company.ToArray();
//                }
//            }
//            catch(Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }

//            return fee;

//        }

//        public FeeYearlygroupHeadMappingDTO getsearchdata(int id, FeeYearlygroupHeadMappingDTO org)
//        {
//            try
//            {
//                List<FeeYearlygroupHeadMappingDMO> lorg = new List<FeeYearlygroupHeadMappingDMO>();
//                if (org.FMH_FeeName == "Group Name")
//                {
//                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                     from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                     where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_GroupName.Contains(org.FMG_GroupName))
//                                     select new FeeYearlygroupHeadMappingDTO
//                                     {
//                                         FYGHM_Id = a.FYGHM_Id,
//                                         FMG_GroupName = b.FMG_GroupName,
//                                         FMH_FeeName = c.FMH_FeeName,
//                                         FMI_Name = d.FMI_Name,
//                                     }
//       ).ToArray();

//                }
//                if (org.FMH_FeeName == "Head Name")
//                {
//                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && c.FMH_FeeName.Contains(org.FMH_FeeName))
//                                   select new FeeYearlygroupHeadMappingDTO
//                                   {
//                                       FYGHM_Id = a.FYGHM_Id,
//                                       FMG_GroupName = b.FMG_GroupName,
//                                       FMH_FeeName = c.FMH_FeeName,
//                                       FMI_Name = d.FMI_Name,
//                                   }
//      ).ToArray();

//                }

//                if (org.FMH_FeeName == "Installment")
//                {
//                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && d.FMI_Name.Contains(org.FMI_Name))
//                                   select new FeeYearlygroupHeadMappingDTO
//                                   {
//                                       FYGHM_Id = a.FYGHM_Id,
//                                       FMG_GroupName = b.FMG_GroupName,
//                                       FMH_FeeName = c.FMH_FeeName,
//                                       FMI_Name = d.FMI_Name,
//                                   }
//      ).ToArray();

//                }

//                if (org.FMH_FeeName == "All")
//                {
//                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                     from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                     from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                     from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                     where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id)
//                                     select new FeeYearlygroupHeadMappingDTO
//                                     {
//                                         FYGHM_Id = a.FYGHM_Id,
//                                         FMG_GroupName = b.FMG_GroupName,
//                                         FMH_FeeName = c.FMH_FeeName,
//                                         FMI_Name = d.FMI_Name,
//                                     }
//       ).ToArray();

//                }

//                // org.thirdgriddata = lorg.ToArray();
//            }
//            catch (Exception ee)
//            {
//                Console.WriteLine(ee.Message);
//            }
//            return org;
//        }

//        public FeeYearlygroupHeadMappingDTO savedetails(FeeYearlygroupHeadMappingDTO pgmod)
//        {
//            FeeYearlygroupHeadMappingDTO someObj = new FeeYearlygroupHeadMappingDTO();
//            try
//            {
//                FeeYearlygroupHeadMappingDTO pgmodule = Mapper.Map<FeeYearlygroupHeadMappingDTO>(pgmod);

//                bool returnresult = false;
//                if (pgmod.savetmpdata[0].FYGHM_Id > 0)
//                {

//                    if (pgmod.savetmpdata != null)
//                    {
//                        int j = 0;
//                        string fineflag = "0";
//                        string commonamtflag = "0";
//                        string acyiveflag = "0";

//                        //var result = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Single(t => t.FYGHM_Id == pgmod.FYGHM_Id);

//                        while (j < pgmod.savetmpdata.Count())
//                        {
//                            FeeYearlygroupHeadMappingDMO pmm = new FeeYearlygroupHeadMappingDMO();
//                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
//                            {
//                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
//                                pmm.FMI_Id = pgmod.savetmpdata[j].FMI_Id;
//                                pmm.FYGHM_Id = pgmod.savetmpdata[j].FYGHM_Id;

//                                fineflag = pgmod.savetmpdata[j].FYGHM_FineApplicableFlag;
//                                commonamtflag = pgmod.savetmpdata[j].FYGHM_Common_AmountFlag;
//                                acyiveflag = pgmod.savetmpdata[j].FYGHM_ActiveFlag;

//                                //if (pgmod.savetmpdata[j].FYGHM_FineApplicableFlag == "true")
//                                //{
//                                //    fineflag = "Y";
//                                //}
//                                //else
//                                //{
//                                //    fineflag = "N";
//                                //}
//                                //if (pgmod.savetmpdata[j].FYGHM_Common_AmountFlag == "true")
//                                //{
//                                //    commonamtflag = "Y";
//                                //}
//                                //else
//                                //{
//                                //    commonamtflag = "N";
//                                //}
//                                //if (pgmod.savetmpdata[j].FYGHM_ActiveFlag == "1")
//                                //{
//                                //    acyiveflag = "1";
//                                //}
//                                //else
//                                //{
//                                //    acyiveflag = "0";
//                                //}

//                                pmm.MI_Id = pgmod.MI_Id;
//                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
//                                pmm.FMG_Id = pgmod.FMG_Id;
//                                pmm.FYGHM_FineApplicableFlag = fineflag;
//                                pmm.FYGHM_Common_AmountFlag = commonamtflag;
//                                pmm.FYGHM_ActiveFlag = acyiveflag;

//                                //_YearlyFeeGroupMappingContext.Add(pmm);
//                                //var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

//                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
//                                {
//                                    cmd.CommandText = "Insert_Fee_Yearly_Group_Head_Mapping";
//                                    cmd.CommandType = CommandType.StoredProcedure;
//                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
//                                        SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.MI_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
//                                   SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.ASMAY_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
//                                     SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FMG_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
//                                SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FMH_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FMI_Id",
//                                      SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FMI_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_FineApplicableFlag",
//                                      SqlDbType.Decimal)
//                                    {
//                                        Value = pmm.FYGHM_FineApplicableFlag
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_Common_AmountFlag",
//                                   SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FYGHM_Common_AmountFlag
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_ActiveFlag",
//                                   SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FYGHM_ActiveFlag
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_Id",
//                                SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FYGHM_Id
//                                    });


//                                    if (cmd.Connection.State != ConnectionState.Open)
//                                        cmd.Connection.Open();
//                                    //var data = cmd.ExecuteNonQuery();

//                                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag,pmm.FYGHM_Id);

//                                    //if (data >= 1)
//                                    //{
//                                    //    returnresult = true;
//                                    //    pgmod.returnval = returnresult;
//                                    //}
//                                    //else
//                                    //{
//                                    //    returnresult = false;
//                                    //    pgmod.returnval = returnresult;
//                                    //}
//                                }

//                            }
//                            j++;
//                        }
//                    }
//                }
//                else
//                {
//                    if (pgmod.savetmpdata != null)
//                    {

//                        int j = 0;
//                        string fineflag = "0";
//                        string commonamtflag = "0";
//                        string acyiveflag = "0";

//                        while (j < pgmod.savetmpdata.Count())
//                        {
//                            FeeYearlygroupHeadMappingDMO pmm = new FeeYearlygroupHeadMappingDMO();
//                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
//                            {
//                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
//                                pmm.FMI_Id = pgmod.savetmpdata[j].FMI_Id;
//                                pmm.FYGHM_Id = pgmod.savetmpdata[j].FYGHM_Id;

//                                //if (pgmod.savetmpdata[j].FYGHM_FineApplicableFlag=="true")
//                                //{
//                                //    fineflag = "Y";
//                                //}
//                                //else
//                                //{
//                                //    fineflag = "N";
//                                //}
//                                //if(pgmod.savetmpdata[j].FYGHM_Common_AmountFlag == "true")
//                                //{
//                                //    commonamtflag = "Y";
//                                //}
//                                //else
//                                //{
//                                //    commonamtflag = "N";
//                                //}
//                                //if (pgmod.savetmpdata[j].FYGHM_ActiveFlag == "true")
//                                //{
//                                //    acyiveflag = "1";
//                                //}
//                                //else
//                                //{
//                                //    acyiveflag = "0";
//                                //}

//                                fineflag = pgmod.savetmpdata[j].FYGHM_FineApplicableFlag;
//                                commonamtflag = pgmod.savetmpdata[j].FYGHM_Common_AmountFlag;
//                                acyiveflag = pgmod.savetmpdata[j].FYGHM_ActiveFlag;

//                                pmm.MI_Id = pgmod.MI_Id;
//                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
//                                pmm.FMG_Id = pgmod.FMG_Id;
//                                pmm.FYGHM_FineApplicableFlag = fineflag;
//                                pmm.FYGHM_Common_AmountFlag = commonamtflag;
//                                pmm.FYGHM_ActiveFlag = acyiveflag;

//                                //_YearlyFeeGroupMappingContext.Add(pmm);
//                                //var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

//                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
//                                {
//                                    cmd.CommandText = "Insert_Fee_Yearly_Group_Head_Mapping";
//                                    cmd.CommandType = CommandType.StoredProcedure;
//                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
//                                        SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.MI_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
//                                   SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.ASMAY_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
//                                     SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FMG_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
//                                SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FMH_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FMI_Id",
//                                      SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FMI_Id
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_FineApplicableFlag",
//                                      SqlDbType.Decimal)
//                                    {
//                                        Value = pmm.FYGHM_FineApplicableFlag
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_Common_AmountFlag",
//                                   SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FYGHM_Common_AmountFlag
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_ActiveFlag",
//                                   SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FYGHM_ActiveFlag
//                                    });

//                                    cmd.Parameters.Add(new SqlParameter("@FYGHM_Id",
//                                  SqlDbType.BigInt)
//                                    {
//                                        Value = pmm.FYGHM_Id
//                                    });

//                                    if (cmd.Connection.State != ConnectionState.Open)
//                                        cmd.Connection.Open();
//                                    //var data = cmd.ExecuteNonQuery();

//                                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id);

//                                    //if (data >= 1)
//                                    //{
//                                    //    returnresult = true;
//                                    //    pgmod.returnval = returnresult;
//                                    //}
//                                    //else
//                                    //{
//                                    //    returnresult = false;
//                                    //    pgmod.returnval = returnresult;
//                                    //}
//                                }

//                            }
//                            j++;
//                        }
//                    }
//                }

//                //List<MasterRolePreviledgeDMO> rolemodulep = new List<MasterRolePreviledgeDMO>();
//                //rolemodulep = _MasterRolePreviledgesContext.masterRolePreviledgeDMO.ToList();
//                //pgmod.thirdgriddata = rolemodulep.ToArray();

//                //someObj.enq = (from a in _MasterRolePreviledgesContext.masterPageModuleMapping
//                //               from b in _MasterRolePreviledgesContext.masterPage
//                //               from c in _MasterRolePreviledgesContext.masterModule
//                //               from d in _MasterRolePreviledgesContext.masterRolePreviledgeDMO
//                //               where (d.IVRMMP_Id == a.IVRMMP_Id && a.IVRMM_Id==c.IVRMM_Id && a.IVRMP_Id==b.IVRMP_Id)
//                //               select new MasterRolePreviledgeDTO
//                //               {
//                //                   ivrmmP_PageName = b.IVRMMP_PageName,
//                //                   ivrmM_ModuleName = c.IVRMM_ModuleName
//                //               }
//                // ).ToArray();


//                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
//                                       from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
//                                       from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
//                                       from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
//                                       where (a.FMH_Id==c.FMH_Id && a.FMG_Id==b.FMG_Id && a.ASMAY_Id== pgmod.ASMAY_Id && a.MI_Id== pgmod.MI_Id && a.FMI_Id==d.FMI_Id)
//                                       select new FeeYearlygroupHeadMappingDTO
//                                       {
//                                           FYGHM_Id=a.FYGHM_Id,
//                                           FMG_GroupName = b.FMG_GroupName,
//                                           FMH_FeeName = c.FMH_FeeName,
//                                           FMI_Name = d.FMI_Name,
//                                       }
//        ).ToArray();

//            }

//            catch (Exception ee)
//            {
//                Console.WriteLine(ee.Message);
//                _logger.LogError(ee.Message);
//            }

//            return pgmod;
//        }
//    }
//}
