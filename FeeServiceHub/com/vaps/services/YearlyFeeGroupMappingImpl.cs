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
using Microsoft.Extensions.Logging;

namespace FeeServiceHub.com.vaps.services
{
    public class YearlyFeeGroupMappingImpl : interfaces.YearlyFeeGroupMappingInterfaces
    {
        private static ConcurrentDictionary<string, FeeYearlygroupHeadMappingDTO> _login =
            new ConcurrentDictionary<string, FeeYearlygroupHeadMappingDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<YearlyFeeGroupMappingImpl> _logger;
        public YearlyFeeGroupMappingImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<YearlyFeeGroupMappingImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public FeeYearlygroupHeadMappingDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeYearlygroupHeadMappingDTO page = new FeeYearlygroupHeadMappingDTO();
            List<FeeYearlygroupHeadMappingDMO> lorg = new List<FeeYearlygroupHeadMappingDMO>();
            lorg = _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FYGHM_Id.Equals(id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _YearlyFeeGroupMappingContext.Remove(lorg.ElementAt(0));

                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }

                page.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == 10 && a.MI_Id == 2 && a.FMI_Id == d.FMI_Id)
                                select new FeeYearlygroupHeadMappingDTO
                                {
                                    FYGHM_Id = a.FYGHM_Id,
                                    FMG_GroupName = b.FMG_GroupName,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FMI_Name = d.FMI_Name,
                                }
        ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public FeeYearlygroupHeadMappingDTO EditMasterscetionDetails(FeeYearlygroupHeadMappingDTO fee)
        {
            try
            {
                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && a.FYGHM_Id == fee.FYGHM_Id)
                               select new FeeYearlygroupHeadMappingDTO
                               {
                                   FMH_Id = a.FMH_Id,
                                   FMG_Id = a.FMG_Id,
                                   FYGHM_Id = a.FYGHM_Id,
                                   FMG_GroupName = b.FMG_GroupName,
                                   FMH_FeeName = c.FMH_FeeName,
                                   FMI_Name = d.FMI_Name,
                                   FMI_Id = d.FMI_Id,
                                   FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                   FYGHM_Common_AmountFlag = a.FYGHM_Common_AmountFlag,
                                   FYGHM_ActiveFlag = a.FYGHM_ActiveFlag,
                               }
       ).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;
        }

        public FeeYearlygroupHeadMappingDTO getdata(FeeYearlygroupHeadMappingDTO fee)
        {
            try
            {
                List<FeeHeadDMO> head = new List<FeeHeadDMO>();
                head = _YearlyFeeGroupMappingContext.FeeHeadDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMH_ActiveFlag==true).OrderBy(t=>t.FMH_Order).ToList();
                fee.fillmasterhead = head.ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == fee.MI_Id).ToList();
                fee.fillmastergroup = group.ToArray();

                List<MasterCompanyDMO> company = new List<MasterCompanyDMO>();
                company = _YearlyFeeGroupMappingContext.MasterCompanyDMO.Where(t=>t.MI_Id==fee.MI_Id).ToList();
                fee.fillcompany = company.ToArray();

                List<FeeInstallmentDMO> installment = new List<FeeInstallmentDMO>();
                installment = _YearlyFeeGroupMappingContext.FeeInstallmentDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMI_ActiceFlag==true).ToList();
                fee.fillinstallment = installment.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                fee.academicdrp = allyear.Distinct().ToArray();

       //         fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
       //                        from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
       //                        from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
       //                        from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
       //                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_ActiceFlag==true && c.FMH_ActiveFlag==true && d.FMI_ActiceFlag==true)
       //                        select new FeeYearlygroupHeadMappingDTO
       //                        {
       //                            FYGHM_Id = a.FYGHM_Id,
       //                            FMG_GroupName = b.FMG_GroupName,
       //                            FMH_FeeName = c.FMH_FeeName,
       //                            FMI_Name = d.FMI_Name,
       //                        }
       //).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;

        }

        public FeeYearlygroupHeadMappingDTO getdataongroup(FeeYearlygroupHeadMappingDTO fee)
        {
            try
            {
                fee.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == fee.FMG_Id && c.FMH_ActiveFlag==true)
                               select new FeeYearlygroupHeadMappingDTO
                               {
                                   FYGHM_Id = a.FYGHM_Id,
                                   FMG_GroupName = b.FMG_GroupName,
                                   FMH_FeeName = c.FMH_FeeName,
                                   FMI_Name = d.FMI_Name,
                                   FMH_Id = c.FMH_Id,
                                   FMI_Id = d.FMI_Id,
                                   FYGHM_ActiveFlag = a.FYGHM_ActiveFlag,
                                   FYGHM_Common_AmountFlag = a.FYGHM_Common_AmountFlag,
                                   FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                               }
          ).ToArray();

                //if (fee.alldata.Length <= 0)
                //{
                //    List<MasterCompanyDMO> company = new List<MasterCompanyDMO>();
                //    company = _YearlyFeeGroupMappingContext.MasterCompanyDMO.Where(t=>t.MI_Id == fee.MI_Id).ToList();
                //    fee.alldata = company.ToArray();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fee;

        }

        public FeeYearlygroupHeadMappingDTO getsearchdata(int id, FeeYearlygroupHeadMappingDTO org)
        {
            try
            {
                List<FeeYearlygroupHeadMappingDMO> lorg = new List<FeeYearlygroupHeadMappingDMO>();
                if (org.FMH_FeeName == "Group Name")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_GroupName.Contains(org.FMG_GroupName))
                                   select new FeeYearlygroupHeadMappingDTO
                                   {
                                       FYGHM_Id = a.FYGHM_Id,
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FMI_Name = d.FMI_Name,
                                   }
       ).ToArray();

                }
                if (org.FMH_FeeName == "Head Name")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && c.FMH_FeeName.Contains(org.FMH_FeeName))
                                   select new FeeYearlygroupHeadMappingDTO
                                   {
                                       FYGHM_Id = a.FYGHM_Id,
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FMI_Name = d.FMI_Name,
                                   }
      ).ToArray();

                }

                if (org.FMH_FeeName == "Installment")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && d.FMI_Name.Contains(org.FMI_Name))
                                   select new FeeYearlygroupHeadMappingDTO
                                   {
                                       FYGHM_Id = a.FYGHM_Id,
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FMI_Name = d.FMI_Name,
                                   }
      ).ToArray();

                }

                if (org.FMH_FeeName == "All")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id)
                                   select new FeeYearlygroupHeadMappingDTO
                                   {
                                       FYGHM_Id = a.FYGHM_Id,
                                       FMG_GroupName = b.FMG_GroupName,
                                       FMH_FeeName = c.FMH_FeeName,
                                       FMI_Name = d.FMI_Name,
                                   }
       ).ToArray();

                }

                // org.thirdgriddata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public FeeYearlygroupHeadMappingDTO savedetails(FeeYearlygroupHeadMappingDTO pgmod)
        {
            FeeYearlygroupHeadMappingDTO someObj = new FeeYearlygroupHeadMappingDTO();
            try
            {
                List<FeeMasterConfigurationDMO> installment = new List<FeeMasterConfigurationDMO>();
                installment = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == pgmod.MI_Id && t.userid==pgmod.userid).ToList();
                bool finemappingflag = false;

                foreach (var value in installment)
                {
                    if (value.FMC_FineMapping == true)
                    {
                        finemappingflag = true;
                    }
                    else
                    {
                        finemappingflag = false;
                    }
                }

                FeeYearlygroupHeadMappingDTO pgmodule = Mapper.Map<FeeYearlygroupHeadMappingDTO>(pgmod);

                bool returnresult = false;
                int finecnt = 0;

                if (pgmod.savetmpdata[0].FYGHM_Id > 0)
                {
                    if (pgmod.savetmpdata != null)
                    {
                        for (int i = 0; i < pgmod.savetmpdata.Count(); i++)
                        {
                            var finecheck = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             //from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                             where (a.FMH_Flag == "F" && a.FMH_Id == pgmod.savetmpdata[i].FMH_Id)
                                             select new FeeYearlygroupHeadMappingDTO
                                             {
                                                 FMH_Id = a.FMH_Id,
                                             }
                   ).ToArray();

                            if (finecheck.Count() > 0)
                            {
                                finecnt = finecnt + 1;
                            }
                        }

                        if (finemappingflag == true)
                        {
                            finecnt = finecnt;
                        }
                        else if (finemappingflag == false)
                        {
                            finecnt = 1;
                        }

                        if (finecnt > 0)
                        {

                            int j = 0;
                            string fineflag = "0";
                            string commonamtflag = "0";
                            string acyiveflag = "0";

                            while (j < pgmod.savetmpdata.Count())
                            {
                                FeeYearlygroupHeadMappingDMO pmm = new FeeYearlygroupHeadMappingDMO();
                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                    pmm.FMI_Id = pgmod.savetmpdata[j].FMI_Id;
                                    pmm.FYGHM_Id = pgmod.savetmpdata[j].FYGHM_Id;

                                    fineflag = pgmod.savetmpdata[j].FYGHM_FineApplicableFlag;
                                    commonamtflag = pgmod.savetmpdata[j].FYGHM_Common_AmountFlag;
                                    acyiveflag = pgmod.savetmpdata[j].FYGHM_ActiveFlag;

                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    pmm.FMG_Id = pgmod.FMG_Id;
                                    pmm.FYGHM_FineApplicableFlag = fineflag;
                                    pmm.FYGHM_Common_AmountFlag = commonamtflag;
                                    pmm.FYGHM_ActiveFlag = acyiveflag;

                                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "Insert_Fee_Yearly_Group_Head_Mapping";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                            SqlDbType.BigInt)
                                        {
                                            Value = pmm.MI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.ASMAY_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                         SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMG_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                    SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMH_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FMI_Id",
                                          SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FYGHM_FineApplicableFlag",
                                          SqlDbType.Decimal)
                                        {
                                            Value = pmm.FYGHM_FineApplicableFlag
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FYGHM_Common_AmountFlag",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FYGHM_Common_AmountFlag
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FYGHM_ActiveFlag",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FYGHM_ActiveFlag
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FYGHM_Id",
                                    SqlDbType.BigInt)
                                        {
                                            Value = pmm.FYGHM_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@User_id",
                                    SqlDbType.BigInt)
                                        {
                                            Value = pgmod.userid
                                        });


                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        //var data = cmd.ExecuteNonQuery();

                                        var data = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id,pgmod.userid);

                                        if (data >= 1)
                                        {
                                            pgmod.displaymessage = "Record Updated Successfully";
                                        }
                                        else
                                        {
                                            pgmod.displaymessage = "Record Not Updated Successfully";
                                        }
                                    }

                                }
                                j++;
                            }
                        }

                        else
                        {
                            pgmod.displaymessage = "Add Fine Head to Selected Group";
                        }
                    }
                }
                else
                {
                  
                    if (pgmod.savetmpdata != null)
                    {
                        for(int i=0;i< pgmod.savetmpdata.Count();i++)
                        {
                            var finecheck = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.FMH_Flag=="F" && a.FMH_Id== pgmod.savetmpdata[i].FMH_Id)
                                           select new FeeYearlygroupHeadMappingDTO
                                           {
                                               FMH_Id = a.FMH_Id,
                                           }
                   ).ToArray();

                            if(finecheck.Count()>0)
                            {
                                finecnt = finecnt + 1;
                            }
                        }

                        if (finemappingflag == true)
                        {
                            finecnt = finecnt;
                        }
                        else if (finemappingflag == false)
                        {
                            finecnt = 1;
                        }

                        if (finecnt>0)
                        {
                            int j = 0;
                            string fineflag = "0";
                            string commonamtflag = "0";
                            string acyiveflag = "0";

                            while (j < pgmod.savetmpdata.Count())
                            {
                                FeeYearlygroupHeadMappingDMO pmm = new FeeYearlygroupHeadMappingDMO();
                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                    pmm.FMI_Id = pgmod.savetmpdata[j].FMI_Id;
                                    pmm.FYGHM_Id = pgmod.savetmpdata[j].FYGHM_Id;

                                    fineflag = pgmod.savetmpdata[j].FYGHM_FineApplicableFlag;
                                    commonamtflag = pgmod.savetmpdata[j].FYGHM_Common_AmountFlag;
                                    acyiveflag = pgmod.savetmpdata[j].FYGHM_ActiveFlag;

                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    pmm.FMG_Id = pgmod.FMG_Id;
                                    pmm.FYGHM_FineApplicableFlag = fineflag;
                                    pmm.FYGHM_Common_AmountFlag = commonamtflag;
                                    pmm.FYGHM_ActiveFlag = acyiveflag;

                                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                    {

                                        var data = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id,pgmod.userid);


                                        if (data >= 1)
                                        {
                                            pgmod.displaymessage = "Record Saved Successfully";
                                        }
                                        else
                                        {
                                            pgmod.displaymessage = "Record Not Saved Successfully";
                                        }
                                    }

                                }
                                j++;
                            }
                        }
                        else
                        {
                            pgmod.displaymessage = "Add Fine Head to Selected Group";
                        }
                    }
                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && a.FMI_Id == d.FMI_Id)
                                 select new FeeYearlygroupHeadMappingDTO
                                 {
                                     FYGHM_Id = a.FYGHM_Id,
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FMI_Name = d.FMI_Name,
                                 }
        ).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public FeeYearlygroupHeadMappingDTO selectacade(FeeYearlygroupHeadMappingDTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                        from b in _YearlyFeeGroupMappingContext.Yearlygroups
                               where (a.MI_Id==data.MI_Id && a.FMG_Id==b.FMG_Id && b.ASMAY_Id==data.ASMAY_Id)
                               select new FeeYearlygroupHeadMappingDTO
                               {
                                   FMG_GroupName = a.FMG_GroupName,
                                   FMG_Id = a.FMG_Id,
                               }
       ).ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                               from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from d in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_ActiceFlag == true && c.FMH_ActiveFlag == true && d.FMI_ActiceFlag == true)
                               select new FeeYearlygroupHeadMappingDTO
                               {
                                   FYGHM_Id = a.FYGHM_Id,
                                   FMG_GroupName = b.FMG_GroupName,
                                   FMH_FeeName = c.FMH_FeeName,
                                   FMI_Name = d.FMI_Name,
                               }
   ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
