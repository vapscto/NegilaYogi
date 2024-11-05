using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fee;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model;
using AutoMapper;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class Clg_YearlyFeeGroupMappingImpl : Interfaces.Clg_YearlyFeeGroupMappingInterface
    {
        private static ConcurrentDictionary<string, CLG_YearlyFeeGroupHeadMapping_DTO> _login =
                   new ConcurrentDictionary<string, CLG_YearlyFeeGroupHeadMapping_DTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<Clg_YearlyFeeGroupMappingImpl> _logger;
       public Clg_YearlyFeeGroupMappingImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, ILogger<Clg_YearlyFeeGroupMappingImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public CLG_YearlyFeeGroupHeadMapping_DTO deleterec(int id)
        {
            bool returnresult = false;
            CLG_YearlyFeeGroupHeadMapping_DTO page = new CLG_YearlyFeeGroupHeadMapping_DTO();
            List<CLG_Fee_Yearly_Group_Head_Mapping> lorg = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
            lorg = _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.FYGHM_Id.Equals(id)).ToList();

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

                page.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == 10 && a.MI_Id == 2 && a.FMI_Id == d.FMI_Id)
                                select new CLG_YearlyFeeGroupHeadMapping_DTO
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

        public CLG_YearlyFeeGroupHeadMapping_DTO EditMasterscetionDetails(CLG_YearlyFeeGroupHeadMapping_DTO fee)
        {
            try
            {
                fee.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                               from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                               from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && a.FYGHM_Id == fee.FYGHM_Id)
                               select new CLG_YearlyFeeGroupHeadMapping_DTO
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

        public CLG_YearlyFeeGroupHeadMapping_DTO getdata(CLG_YearlyFeeGroupHeadMapping_DTO fee)
        {
            try
            {
                List<FeeHeadClgDMO> head = new List<FeeHeadClgDMO>();
                head = _YearlyFeeGroupMappingContext.FeeHeadClgDMO.Where(t => t.MI_Id == fee.MI_Id && t.FMH_ActiveFlag == true).OrderBy(t => t.FMH_Order).ToList();
                fee.fillmasterhead = head.ToArray();

                List<FeeGroupClgDMO> group = new List<FeeGroupClgDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupClgDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == fee.MI_Id).ToList();
                fee.fillmastergroup = group.ToArray();

                List<MasterCompanyDMO> company = new List<MasterCompanyDMO>();
                company = _YearlyFeeGroupMappingContext.MasterCompanyDMO.Where(t => t.MI_Id == fee.MI_Id).ToList();
                fee.fillcompany = company.ToArray();

                List<Clg_Fee_Installment_DMO> installment = new List<Clg_Fee_Installment_DMO>();
                installment = _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO.Where(t => t.MI_Id == fee.MI_Id && t.FMI_ActiceFlag == true).ToList();
                fee.fillinstallment = installment.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).ToList();
                fee.academicdrp = allyear.Distinct().ToArray();

                //         fee.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_MappingDMO
                //                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                //                        from c in _YearlyFeeGroupMappingContext.feehead
                //                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                //                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_ActiceFlag==true && c.FMH_ActiveFlag==true && d.FMI_ActiceFlag==true)
                //                        select new CLG_YearlyFeeGroupHeadMapping_DTO
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


        public CLG_YearlyFeeGroupHeadMapping_DTO getdataongroup(CLG_YearlyFeeGroupHeadMapping_DTO fee)
        {
            try
            {
                fee.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                               from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                               from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                               from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                               where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == fee.FMG_Id && c.FMH_ActiveFlag == true)
                               select new CLG_YearlyFeeGroupHeadMapping_DTO
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

        public CLG_YearlyFeeGroupHeadMapping_DTO getsearchdata(int id, CLG_YearlyFeeGroupHeadMapping_DTO org)
        {
            try
            {
                List<CLG_Fee_Yearly_Group_Head_Mapping> lorg = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
                if (org.FMH_FeeName == "Group Name")
                {
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_GroupName.Contains(org.FMG_GroupName))
                                   select new CLG_YearlyFeeGroupHeadMapping_DTO
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
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && c.FMH_FeeName.Contains(org.FMH_FeeName))
                                   select new CLG_YearlyFeeGroupHeadMapping_DTO
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
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id && d.FMI_Name.Contains(org.FMI_Name))
                                   select new CLG_YearlyFeeGroupHeadMapping_DTO
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
                    org.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                   from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                   where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == org.ASMAY_Id && a.MI_Id == org.MI_Id && a.FMI_Id == d.FMI_Id)
                                   select new CLG_YearlyFeeGroupHeadMapping_DTO
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

        public CLG_YearlyFeeGroupHeadMapping_DTO savedetails(CLG_YearlyFeeGroupHeadMapping_DTO pgmod)
        {
            CLG_YearlyFeeGroupHeadMapping_DTO someObj = new CLG_YearlyFeeGroupHeadMapping_DTO();
            try
            {
                bool recsettingval = false;
                var getcurrsett = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id.Equals(pgmod.MI_Id) && t.userid== pgmod.user_id).ToList();
                foreach (var value in getcurrsett)
                {
                    if (value.FMC_FineMapping == true)
                    {
                        recsettingval = true;
                    }
                    else
                    {
                        recsettingval = false;
                    }
                }

                CLG_YearlyFeeGroupHeadMapping_DTO pgmodule = Mapper.Map<CLG_YearlyFeeGroupHeadMapping_DTO>(pgmod);

                int finecnt = 0;
                if (pgmod.savetmpdata[0].FYGHM_Id > 0)
                {
                    if(recsettingval==true)
                    {
                        if (pgmod.savetmpdata != null)
                        {
                            for (int i = 0; i < pgmod.savetmpdata.Count(); i++)
                            {
                                var finecheck = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 where (a.FMH_Flag == "F" && a.FMH_Id == pgmod.savetmpdata[i].FMH_Id)
                                                 select new CLG_YearlyFeeGroupHeadMapping_DTO
                                                 {
                                                     FMH_Id = a.FMH_Id,
                                                 }
                       ).ToArray();

                                if (finecheck.Count() > 0)
                                {
                                    finecnt = finecnt + 1;
                                }
                            }

                            if (finecnt > 0)
                            {
                                int j = 0;
                                string fineflag = "0";
                                string commonamtflag = "0";
                                string acyiveflag = "0";

                                while (j < pgmod.savetmpdata.Count())
                                {
                                    CLG_Fee_Yearly_Group_Head_Mapping pmm = new CLG_Fee_Yearly_Group_Head_Mapping();
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

                                        var data = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id, pgmod.user_id);

                                        if (data >= 1)
                                        {
                                            pgmod.displaymessage = "Record Updated Successfully";
                                        }
                                        else
                                        {
                                            pgmod.displaymessage = "Record Not Updated Successfully";
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
                        int j = 0;
                        string fineflag = "0";
                        string commonamtflag = "0";
                        string acyiveflag = "0";

                        while (j < pgmod.savetmpdata.Count())
                        {
                            CLG_Fee_Yearly_Group_Head_Mapping pmm = new CLG_Fee_Yearly_Group_Head_Mapping();
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

                                var data = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id, pgmod.user_id);

                                if (data >= 1)
                                {
                                    pgmod.displaymessage = "Record Updated Successfully";
                                }
                                else
                                {
                                    pgmod.displaymessage = "Record Not Updated Successfully";
                                }
                            }
                            j++;
                        }
                    }
                }
                else
                {
                    if(recsettingval==true)
                    {
                        if (pgmod.savetmpdata != null)
                        {
                            for (int i = 0; i < pgmod.savetmpdata.Count(); i++)
                            {
                                var finecheck = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 where (a.FMH_Flag == "F" && a.FMH_Id == pgmod.savetmpdata[i].FMH_Id)
                                                 select new CLG_YearlyFeeGroupHeadMapping_DTO
                                                 {
                                                     FMH_Id = a.FMH_Id,
                                                 }
                       ).ToArray();

                                if (finecheck.Count() > 0)
                                {
                                    finecnt = finecnt + 1;
                                }
                            }

                            if (finecnt > 0)
                            {
                                int j = 0;
                                string fineflag = "0";
                                string commonamtflag = "0";
                                string acyiveflag = "0";

                                while (j < pgmod.savetmpdata.Count())
                                {
                                    CLG_Fee_Yearly_Group_Head_Mapping pmm = new CLG_Fee_Yearly_Group_Head_Mapping();
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

                                            var data = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id, pgmod.user_id);


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
                    else
                    {
                        int j = 0;
                        string fineflag = "0";
                        string commonamtflag = "0";
                        string acyiveflag = "0";

                        while (j < pgmod.savetmpdata.Count())
                        {
                            CLG_Fee_Yearly_Group_Head_Mapping pmm = new CLG_Fee_Yearly_Group_Head_Mapping();
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

                                    var data = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pmm.MI_Id, pmm.ASMAY_Id, pmm.FMG_Id, pmm.FMH_Id, pmm.FMI_Id, pmm.FYGHM_FineApplicableFlag, pmm.FYGHM_Common_AmountFlag, pmm.FYGHM_ActiveFlag, pmm.FYGHM_Id, pgmod.user_id);


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
                   
                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                 where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.MI_Id == pgmod.MI_Id && a.FMI_Id == d.FMI_Id)
                                 select new CLG_YearlyFeeGroupHeadMapping_DTO
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

        public CLG_YearlyFeeGroupHeadMapping_DTO selectacade(CLG_YearlyFeeGroupHeadMapping_DTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from b in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                        where (a.MI_Id == data.MI_Id && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id)
                                        select new CLG_YearlyFeeGroupHeadMapping_DTO
                                        {
                                            FMG_GroupName = a.FMG_GroupName,
                                            FMG_Id = a.FMG_Id,
                                        }
       ).ToArray();

                data.alldata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && b.FMG_ActiceFlag == true && c.FMH_ActiveFlag == true && d.FMI_ActiceFlag == true)
                                select new CLG_YearlyFeeGroupHeadMapping_DTO
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
