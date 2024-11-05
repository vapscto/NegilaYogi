using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeWizardImpl : interfaces.FeeWizardInterface
    {
        private static ConcurrentDictionary<string, FeeWizardDTO> _login =
        new ConcurrentDictionary<string, FeeWizardDTO>();

   

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeWizardImpl> _logger;
        public FeeWizardImpl(FeeGroupContext frgContext, ILogger<FeeWizardImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }
        public FeeWizardDTO getdetails(FeeWizardDTO FGRDT)
        {
            List<MasterAcademic> allyear = new List<MasterAcademic>();
             allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.Is_Active == true).OrderByDescending(o => o.ASMAY_Order).ToList();
            FGRDT.academicdrp = allyear.Distinct().ToArray();

            FGRDT.yearlygroup = (from a in _FeeGroupContext.Yearlygroups
                                  from b in _FeeGroupContext.feeGroup
                                  from c in _FeeGroupContext.AcademicYear
                                  where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == c.ASMAY_Id )
                                  select new FeeWizardDTO
                                  {
                                      FYG_Id = a.FYG_Id,
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = b.FMG_GroupName,
                                      ASMAY_Name = c.ASMAY_Year,
                                      FYG_ActiveFlag = a.FYG_ActiveFlag,

                                  }).ToArray();

            FGRDT.yearlygrouphead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                           from b in _FeeGroupContext.FeeGroupDMO
                           from c in _FeeGroupContext.FeeHeadDMO
                           from d in _FeeGroupContext.FeeInstallmentDMO
                           where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id  && a.MI_Id == FGRDT.MI_Id && a.FMI_Id == d.FMI_Id  && c.FMH_ActiveFlag == true)
                           select new FeeWizardDTO
                           {
                               FYGHM_Id = a.FYGHM_Id,
                               FMG_GroupName = b.FMG_GroupName,
                               FMH_FeeName = c.FMH_FeeName,
                               FMI_Name = d.FMI_Name,
                               FMH_Id = c.FMH_Id,
                               FMI_Id = d.FMI_Id,
                               FYGHM_ActiveFlag = a.FYGHM_ActiveFlag,
                               FYGHM_Common_AmountFlag = a.FYGHM_Common_AmountFlag,
                               FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                               FMG_Id=b.FMG_Id,
                           }
         ).ToArray();


            FGRDT.classcategorydata= (from a in _FeeGroupContext.feeCC
                                      from b in _FeeGroupContext.feeYCC
                                      from c in _FeeGroupContext.feeYCCC
                                      from d in _FeeGroupContext.admissioncls
                                      from e in _FeeGroupContext.AcademicYear
                                      where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == FGRDT.MI_Id && e.ASMAY_Id == b.ASMAY_Id && b.FYCC_ActiveFlag == true)
                                      select new FeeWizardDTO
                                      {
                                          FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                          ASMCL_ClassName = d.ASMCL_ClassName,
                                          ASMAY_Name = e.ASMAY_Year,
                                          FMCC_ActiveFlag = b.FYCC_ActiveFlag,

                                          FYCCC_Id = c.FYCCC_Id,
                                          FYCC_Id = c.FYCC_Id,
                                          FMCC_Id = c.FYCCC_Id,
                                      }
                          ).ToArray();


            FGRDT.amountentrydata = (from a in _FeeGroupContext.FeeAmountEntryDMO
                           from b in _FeeGroupContext.FeeGroupDMO
                           from c in _FeeGroupContext.FeeHeadDMO
                           from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
                           where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == FGRDT.MI_Id)
                           select new FeeWizardDTO
                           {
                               FMG_GroupName = b.FMG_GroupName,
                               FMH_FeeName = c.FMH_FeeName,
                               FTI_Name = d.FTI_Name,
                               FMA_Amount = a.FMA_Amount,
                               FMA_Id = a.FMA_Id
                           }
           ).OrderBy(t => t.FMH_FeeName).ToArray();


            FGRDT.autoreceiptdata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                             from d in _FeeGroupContext.AcademicYear
                             where (a.MI_Id == FGRDT.MI_Id && d.ASMAY_Id == a.ASMAY_Id)
                             select new FeeWizardDTO
                             {
                                 FGAR_PrefixName = a.FGAR_PrefixName,
                                 FGAR_SuffixName = a.FGAR_SuffixName,
                                 ASMAY_Name = d.ASMAY_Year,
                                 FGAR_Id = a.FGAR_Id,
                                 FGAR_Starting_No = a.FGAR_Starting_No,
                                 ASMAY_Id=a.ASMAY_Id,
                             }
         ).OrderByDescending(t => t.FGAR_Id).ToArray();




            return FGRDT;

        }

        public FeeWizardDTO SaveYearlyGroupData(FeeWizardDTO FGpage)
        {
            try
            {
                string retval = "";
                if (FGpage.resultData.Count() > 0)
                {
                    foreach (FeeWizardDTO ph in FGpage.resultData)
                    {
                       
                     
                            var result1 = _FeeGroupContext.Yearlygroups.Where(t=>t.ASMAY_Id == FGpage.ASMAY_Idnew && t.FMG_Id == ph.FMG_Id);
                            if (result1.Count() >= 1)
                            {
                                retval = "Duplicate";
                                FGpage.returnduplicatestatus = retval;
                            }
                          else
                        {
                            FeeYearGroupDMO feepge = new FeeYearGroupDMO();

                            feepge.CreatedDate = DateTime.Now;
                                feepge.UpdatedDate = DateTime.Now; 
                                feepge.FYG_CreatedBy = feepge.user_id;
                                feepge.FYG_UpdatedBy = feepge.user_id;
                            feepge.FMG_Id = ph.FMG_Id;
                            feepge.ASMAY_Id = FGpage.ASMAY_Idnew;
                            feepge.FYG_ActiveFlag = true;
                            feepge.MI_Id = FGpage.MI_Id;
                            feepge.user_id = FGpage.user_id;

                       

                                _FeeGroupContext.Add(feepge);
                                var contactExists = _FeeGroupContext.SaveChanges();

                                if (contactExists == 1)
                                {
                                    retval = "Save";
                                    FGpage.returnduplicatestatus = retval;
                                }
                                else
                                {
                                    retval = "NotSave";
                                    FGpage.returnduplicatestatus = retval;
                                }
                            }
                        
                    }
                }
               
                FGpage.yearlygroup= (from a in _FeeGroupContext.Yearlygroups
                                     from b in _FeeGroupContext.feeGroup
                                     from c in _FeeGroupContext.AcademicYear
                                     where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == FGpage.ASMAY_Idnew)
                                     select new FeeWizardDTO
                                     {
                                         FYG_Id=a.FYG_Id,
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = b.FMG_GroupName,
                                         ASMAY_Name = c.ASMAY_Year,
                                         FYG_ActiveFlag=a.FYG_ActiveFlag,


                                     }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return FGpage;
        }


        public FeeWizardDTO changacademicyear(FeeWizardDTO FGpage)
        {
            var asmayold = _FeeGroupContext.AcademicYear.Where(t => t.ASMAY_Id == FGpage.ASMAY_Id && t.MI_Id == FGpage.MI_Id).ToList();
            var order = asmayold[0].ASMAY_Order + FGpage.ASMAY_Order;
           
            var academicyearnew1 = _FeeGroupContext.AcademicYear.Where(t => t.ASMAY_Order == order && t.MI_Id == FGpage.MI_Id).ToList();
            FGpage.academicyearnew = academicyearnew1.ToArray();
            var FMG_idlist = _FeeGroupContext.Yearlygroups.Where(t => t.MI_Id == FGpage.MI_Id && t.ASMAY_Id == academicyearnew1[0].ASMAY_Id).ToList();
            List<long> FMG_IdS = new List<long>();
            if (FMG_idlist.Count > 0)
            {
                foreach (var d in FMG_idlist)
                {
                    FMG_IdS.Add(d.FMG_Id);
                }
            }                        
            FGpage.groupYearData = (from a in _FeeGroupContext.Yearlygroups
                                    from b in _FeeGroupContext.feeGroup
                                    from c in _FeeGroupContext.AcademicYear
                                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == FGpage.ASMAY_Id && a.FYG_ActiveFlag == true && !FMG_IdS.Contains(a.FMG_Id))
                                    select new FeeWizardDTO
                                    {
                                        FMG_Id = a.FMG_Id,
                                        FMG_GroupName = b.FMG_GroupName,
                                        ASMAY_Name = c.ASMAY_Year,


                                    }).ToArray();


            var FMG_idFGH = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.MI_Id == FGpage.MI_Id && t.ASMAY_Id == academicyearnew1[0].ASMAY_Id).ToList();
            List<long> FMG_IdFGH = new List<long>();
            if (FMG_idFGH.Count > 0)
            {
                foreach (var d in FMG_idFGH)
                {
                    FMG_IdFGH.Add(d.FMG_Id);
                }
            }



            FGpage.yearlygroupheaddata = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                     from b in _FeeGroupContext.FeeGroupDMO
                                     from c in _FeeGroupContext.FeeHeadDMO
                                     from d in _FeeGroupContext.FeeInstallmentDMO
                                     where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == FGpage.MI_Id && a.FMI_Id == d.FMI_Id  && c.FMH_ActiveFlag == true &&  a.ASMAY_Id== FGpage.ASMAY_Id && !FMG_IdFGH.Contains(a.FMG_Id))
                                     select new FeeWizardDTO
                                     {
                                         FYGHM_Id = a.FYGHM_Id,
                                         FMG_GroupName = b.FMG_GroupName,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FMI_Name = d.FMI_Name,
                                         FMH_Id = c.FMH_Id,
                                         FMI_Id = d.FMI_Id,
                                         FYGHM_ActiveFlag = a.FYGHM_ActiveFlag,
                                         FYGHM_Common_AmountFlag = a.FYGHM_Common_AmountFlag,
                                         FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                         FMG_Id=a.FMG_Id,
                                     }
         ).ToArray();

            var asmclid = (from a in _FeeGroupContext.feeCC
                           from b in _FeeGroupContext.feeYCC
                           from c in _FeeGroupContext.feeYCCC
                           from d in _FeeGroupContext.admissioncls
                           from e in _FeeGroupContext.AcademicYear
                           where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == FGpage.MI_Id && e.ASMAY_Id == b.ASMAY_Id && b.FYCC_ActiveFlag == true && b.ASMAY_Id == academicyearnew1[0].ASMAY_Id)
                           select new FeeWizardDTO
                           {

                               FMCC_Id = a.FMCC_Id,

                           }
                    ).ToList();
            List<long> fyccc = new List<long>();
            if (asmclid.Count > 0)
            {
                foreach (var d in asmclid)
                {
                    fyccc.Add(d.FMCC_Id);
                }
            }

            FGpage.classcategory = (from a in _FeeGroupContext.feeCC
                                       from b in _FeeGroupContext.feeYCC
                                       from c in _FeeGroupContext.feeYCCC
                                       from d in _FeeGroupContext.admissioncls
                                       from e in _FeeGroupContext.AcademicYear
                                       where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == FGpage.MI_Id && e.ASMAY_Id == b.ASMAY_Id && b.FYCC_ActiveFlag == true && !fyccc.Contains(a.FMCC_Id) && b.ASMAY_Id== FGpage.ASMAY_Id)
                                       select new FeeWizardDTO
                                       {
                                           FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           ASMAY_Name = e.ASMAY_Year,
                                           FMCC_ActiveFlag = a.FMCC_ActiveFlag,

                                           FYCCC_Id = c.FYCCC_Id,
                                           FYCC_Id = c.FYCC_Id,
                                           FMCC_Id = a.FMCC_Id,
                                           ASMCL_Id=c.ASMCL_Id,

                                       }
                   ).ToArray();
            var fmaid = _FeeGroupContext.FeeAmountEntryDMO.Where(t => t.MI_Id == FGpage.MI_Id && t.ASMAY_Id == academicyearnew1[0].ASMAY_Id).ToList();
            List<long> FMAID= new List<long>();
            if (fmaid.Count > 0)
            {
                foreach (var d in fmaid)
                {
                    FMAID.Add(d.FMG_Id);
                }
            }



            FGpage.amountentry = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                     from b in _FeeGroupContext.FeeGroupDMO
                                     from c in _FeeGroupContext.FeeHeadDMO
                                     from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                     where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == FGpage.MI_Id && a.ASMAY_Id== FGpage.ASMAY_Id && !FMAID.Contains(a.FMG_Id))
                                     select new FeeWizardDTO
                                     {
                                         FMG_GroupName = b.FMG_GroupName,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FMA_Amount = a.FMA_Amount,
                                         FMA_Id = a.FMA_Id,
                                         FMG_Id=b.FMG_Id,
                                         FMH_Id=c.FMH_Id,
                                         FTI_Id=d.FTI_Id,
                                         FMA_DueDate=a.FMA_DueDate,
                                         FMCC_Id=a.FMCC_Id

                                     }
           ).OrderBy(t => t.FMH_FeeName).ToArray();

            var res = (from x in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                       from y in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                       where (x.MI_Id == FGpage.MI_Id && x.FGAR_Id==y.FGAR_Id && x.ASMAY_Id == academicyearnew1[0].ASMAY_Id)
                       select y).ToList();
            List<long> fmgid = new List<long>();
            if (res.Count > 0)
            {
                foreach (var d in res)
                {
                    fmgid.Add(d.FMG_Id);
                }
            }

            FGpage.autoreceipt = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                     from b in _FeeGroupContext.AcademicYear
                                     from c in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                     from d in _FeeGroupContext.FeeGroupDMO
                                  where (a.MI_Id == FGpage.MI_Id && b.ASMAY_Id == a.ASMAY_Id && a.FGAR_Id==c.FGAR_Id && d.FMG_Id==c.FMG_Id && a.ASMAY_Id==FGpage.ASMAY_Id && !fmgid.Contains(c.FMG_Id))
                                     select new FeeWizardDTO
                                     {
                                         FGAR_PrefixName = a.FGAR_PrefixName,
                                         FGAR_SuffixName = a.FGAR_SuffixName,
                                         ASMAY_Name = b.ASMAY_Year,
                                         FGAR_Id = a.FGAR_Id,
                                         FGAR_Starting_No = a.FGAR_Starting_No,
                                         FGAR_PrefixFlag=a.FGAR_PrefixFlag,
                                         FGAR_SuffixFlag=a.FGAR_SuffixFlag,
                                         FGAR_Address=a.FGAR_Address,
                                         FGAR_Name=a.FGAR_Name,
                                         FGAR_Template_Name=a.FGAR_Template_Name,
                                         FMG_Id=c.FMG_Id,
                                         FMG_GroupName=d.FMG_GroupName,
                                         ASMAY_Id=a.ASMAY_Id
                                      

                                     }
      ).OrderByDescending(t => t.FGAR_Id).ToArray();



   


            return FGpage;
        }

        public FeeWizardDTO deactivateY(FeeWizardDTO acd)
        {
            try
            {
                FeeYearGroupDMO feepge = new FeeYearGroupDMO();
                if (acd.FYG_Id > 0)
                {

           

                    var deletegroup = (from a in _FeeGroupContext.Yearlygroups
                                       from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       where (a.FMG_Id == c.FMG_Id && c.MI_Id == acd.MI_Id && c.ASMAY_Id == acd.ASMAY_Id && a.ASMAY_Id == c.ASMAY_Id && c.FYGHM_ActiveFlag == "1" && a.FYG_Id == acd.FYG_Id && a.MI_Id == c.MI_Id)
                                       select new FeeGroupDTO
                                       {
                                           FMG_Id = Convert.ToInt32(a.FMG_Id)
                                       }
      ).OrderBy(t => t.FMG_Id).Take(1).Select(t => t.FMG_Id).ToList();

                    if (deletegroup.Count == 0)
                    {
                        var result = _FeeGroupContext.Yearlygroups.Single(t => t.FYG_Id == acd.FYG_Id);

                        result.UpdatedDate = DateTime.Now;

                        if (result.FYG_ActiveFlag == true)
                        {
                            result.FYG_ActiveFlag = false;
                        }
                        else
                        {
                            result.FYG_ActiveFlag = true;
                        }
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                    acd.yearlygroup = (from a in _FeeGroupContext.Yearlygroups
                                         from b in _FeeGroupContext.feeGroup
                                         from c in _FeeGroupContext.AcademicYear
                                         where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == c.ASMAY_Id )
                                         select new FeeWizardDTO
                                         {
                                             FYG_Id = a.FYG_Id,
                                             FMG_Id = a.FMG_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMAY_Name = c.ASMAY_Year,
                                             FYG_ActiveFlag = a.FYG_ActiveFlag,

                                         }).ToArray();


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }


        public FeeWizardDTO savedetailsFGH(FeeWizardDTO pgmod)
        {
            FeeWizardDTO someObj = new FeeWizardDTO();
            bool returnresult = false;
            bool finemappingflag = false;
            int finecnt = 0;
            try
            {
                if (pgmod.resultData.Count() > 0)
                {
                    foreach (FeeWizardDTO ph in pgmod.resultData)
                    {



                        var finecheck = (from a in _FeeGroupContext.FeeHeadDMO
                                         where (a.FMH_Flag == "F" && a.FMH_Id == ph.FMH_Id)
                                         select new FeeWizardDTO
                                         {
                                             FMH_Id = a.FMH_Id,
                                         }
               ).ToArray();

                        if (finecheck.Count() > 0)
                        {
                            finecnt = finecnt + 1;
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

                            ph.FYGHM_ActiveFlag = "1";
                            ph.FYGHM_Id = 0;
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Idnew != 0)
                            {
                      

                                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                                {

                                    var data = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_Yearly_Group_Head_Mapping @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", pgmod.MI_Id, pgmod.ASMAY_Idnew, ph.FMG_Id, ph.FMH_Id, ph.FMI_Id, ph.FYGHM_FineApplicableFlag, ph.FYGHM_Common_AmountFlag, ph.FYGHM_ActiveFlag, ph.FYGHM_Id, pgmod.user_id);


                                    if (data >= 1)
                                    {
                                        pgmod.returnduplicatestatus = "Save";
                                    }
                                    else
                                    {
                                        pgmod.returnduplicatestatus = "NotSave";
                                    }
                                }



                            }
                        }
                        else
                        {
                            pgmod.returnduplicatestatus = "FineHead";
                        }



                    }
                }


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public FeeWizardDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeWizardDTO page = new FeeWizardDTO();
            List<FeeYearlygroupHeadMappingDMO> lorg = new List<FeeYearlygroupHeadMappingDMO>();
            lorg = _FeeGroupContext.FeeYearlygroupHeadMappingDMO.Where(t => t.FYGHM_Id.Equals(id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _FeeGroupContext.Remove(lorg.ElementAt(0));

                    var contactExists = _FeeGroupContext.SaveChanges();
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

                   page.yearlygrouphead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                           from b in _FeeGroupContext.FeeGroupDMO
                           from c in _FeeGroupContext.FeeHeadDMO
                           from d in _FeeGroupContext.FeeInstallmentDMO
                           where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id  && a.MI_Id == page.MI_Id && a.FMI_Id == d.FMI_Id  && c.FMH_ActiveFlag == true)
                           select new FeeWizardDTO
                           {
                               FYGHM_Id = a.FYGHM_Id,
                               FMG_GroupName = b.FMG_GroupName,
                               FMH_FeeName = c.FMH_FeeName,
                               FMI_Name = d.FMI_Name,
                               FMH_Id = c.FMH_Id,
                               FMI_Id = d.FMI_Id,
                               FYGHM_ActiveFlag = a.FYGHM_ActiveFlag,
                               FYGHM_Common_AmountFlag = a.FYGHM_Common_AmountFlag,
                               FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                               FMG_Id=b.FMG_Id,
                           }
         ).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public FeeWizardDTO savedetailYCC(FeeWizardDTO pgmod)
        {
            string retval = "";
            bool returnresult = false;
            string dupl = "false";
            try
            {

                if (pgmod.resultData.Count() > 0)
                {
                    foreach (FeeWizardDTO ph in pgmod.resultData)
                    {

                        
                            var result = _FeeGroupContext.feeYCC.Where(t => t.FMCC_Id == ph.FMCC_Id && t.ASMAY_Id == pgmod.ASMAY_Idnew && t.MI_Id == pgmod.MI_Id);
                            if (result.Count() > 0)
                            {
                                List<FeeYearlyClassCategoryDMO> allrecords = new List<FeeYearlyClassCategoryDMO>();
                                allrecords = _FeeGroupContext.feeYCC.Where(t => t.FMCC_Id == ph.FMCC_Id && t.ASMAY_Id == pgmod.ASMAY_Idnew && t.MI_Id == pgmod.MI_Id).ToList();
                                if (allrecords.Count > 0)
                                {
                                    for (int i = 0; allrecords.Count > i; i++)
                                    {
                                        List<MasterYearlyClassCategoryClassDMO> allrecordscheck = new List<MasterYearlyClassCategoryClassDMO>();
                                        allrecordscheck = _FeeGroupContext.feeYCCC.Where(t => t.ASMCL_Id == pgmod.ASMCL_Id && t.FYCC_Id == allrecords[i].FYCC_Id).ToList();
                                        if (allrecordscheck.Count() > 0)
                                        {
                                            dupl = "false";
                                        }
                                        else
                                        {
                                            dupl = "true";
                                        }
                                    }
                                    if (dupl == "false")
                                    {
                                        retval = "Duplicate";
                                        pgmod.returnduplicatestatus = retval;
                                    }
                                    else
                                    {
                                        var conte = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_yearly_class_category @p0,@p1,@p2,@p3,@p4,@p5,@p6", pgmod.MI_Id,pgmod.ASMAY_Idnew, ph.FMCC_Id, ph.ASMCL_Id, ph.FYCC_ActiveFlag, ph.FYCC_Id, pgmod.user_id);

                                   
                                }
                                }
                            }
                            else
                            {

                                var conte = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_yearly_class_category @p0,@p1,@p2,@p3,@p4,@p5,@p6", pgmod.MI_Id, pgmod.ASMAY_Idnew, ph.FMCC_Id, ph.ASMCL_Id, ph.FYCC_ActiveFlag, ph.FYCC_Id, pgmod.user_id);


                            }
                      
                    }
                }


                pgmod.classcategorydata = (from a in _FeeGroupContext.feeCC
                                      from b in _FeeGroupContext.feeYCC
                                      from c in _FeeGroupContext.feeYCCC
                                      from d in _FeeGroupContext.admissioncls
                                      from e in _FeeGroupContext.AcademicYear
                                      where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == pgmod.MI_Id  && e.ASMAY_Id == b.ASMAY_Id)
                                      select new FeeClassCategoryDTO
                                      {
                                          FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                          ASMCL_ClassName = d.ASMCL_ClassName,
                                          ASMAY_Year = e.ASMAY_Year,
                                          FMCC_ActiveFlag = b.FYCC_ActiveFlag,

                                          FYCCC_Id = c.FYCCC_Id,
                                          FYCC_Id = c.FYCC_Id,
                                          FMCC_Id = c.FYCCC_Id,
                                      }
      ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return pgmod;


        }

        public FeeWizardDTO deleterecY(int id)
        {
            bool returnresult = false;
            FeeWizardDTO page = new FeeWizardDTO();
            List<MasterYearlyClassCategoryClassDMO> lorgcheck123 = new List<MasterYearlyClassCategoryClassDMO>();
            lorgcheck123 = _FeeGroupContext.feeYCCC.Where(t => t.FYCC_Id.Equals(id)).ToList();
            if (lorgcheck123.Count() > 0)
            {
                _FeeGroupContext.Remove(lorgcheck123.ElementAt(0));
                var contactExists = _FeeGroupContext.SaveChanges();
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
            else
            {
                returnresult = false;
                page.returnval = returnresult;
            }

            return page;
        }

        public FeeWizardDTO savedetailFMA(FeeWizardDTO pgmod)
        {
            string retval = "";
            bool returnresult = false;
            string dupl = "false";
            try
            {

                if (pgmod.resultData.Count() > 0)
                {
                    foreach (FeeWizardDTO ph in pgmod.resultData)
                    {

                      
                        

                           
                                    FeeAmountEntryDMO pmm = new FeeAmountEntryDMO();
                                    if (pgmod.MI_Id != 0 && pgmod.ASMAY_Idnew != 0)
                                    {
                                        pmm.FMH_Id = ph.FMH_Id;
                                        pmm.FTI_Id = ph.FTI_Id;
                                        pmm.FMA_Amount = ph.FMA_Amount;
                            pmm.ASMAY_Id = pgmod.ASMAY_Idnew;

                                        pmm.MI_Id = pgmod.MI_Id;
                                        pmm.ASMAY_Id = pgmod.ASMAY_Idnew;
                                        pmm.FMG_Id = ph.FMG_Id;
                                        pmm.FMCC_Id = ph.FMCC_Id;
                                        pmm.FMA_Flag = "1";

                                       

                                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "Insert_Fee_Amount_Entry";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                                SqlDbType.BigInt)
                                            {
                                                Value = pmm.MI_Id
                                            });
                                            cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                               SqlDbType.BigInt)
                                            {
                                                Value = pmm.FMG_Id
                                            });
                                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                           SqlDbType.BigInt)
                                            {
                                                Value = pmm.ASMAY_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@FMCC_Id",
                                        SqlDbType.BigInt)
                                            {
                                                Value = pmm.FMCC_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                              SqlDbType.BigInt)
                                            {
                                                Value = pmm.FTI_Id
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@FMA_Amount",
                                              SqlDbType.Decimal)
                                            {
                                                Value = pmm.FMA_Amount
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@FMA_Flag",
                                           SqlDbType.BigInt)
                                            {
                                                Value = pmm.FMA_Flag
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                           SqlDbType.BigInt)
                                            {
                                                Value = pmm.FMH_Id
                                            });

                                            if (ph.FMA_DueDate!= null)
                                            {
                                                cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                         SqlDbType.BigInt)
                                                {
                                                    Value = ph.FMA_DueDate.Value.Month
                                                });
                                            }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Month",
                                         SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }

                                            if (ph.FMA_DueDate != null)
                                            {
                                                cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                        SqlDbType.BigInt)
                                                {
                                                    Value = ph.FMA_DueDate.Value.Day
                                                });
                                            }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@FTDD_Day",
                                       SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });

                                }

                                            if (ph.FMA_DueDate!= null)
                                            {
                                                cmd.Parameters.Add(new SqlParameter("@FTDDE_Month",
                                          SqlDbType.BigInt)
                                                {
                                                    Value = ph.FMA_DueDate.Value.Month
                                                });
                                            }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@FTDDE_Month",
                                         SqlDbType.BigInt)
                                    {
                                        Value =0
                                    });

                                }
                                            if (ph.FMA_DueDate != null)
                                            {
                                                cmd.Parameters.Add(new SqlParameter("@FTDDE_Day",
                                        SqlDbType.BigInt)
                                                {
                                                    Value = ph.FMA_DueDate.Value.Day
                                                });
                                            }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@FTDDE_Day",
                                       SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }

                                            cmd.Parameters.Add(new SqlParameter("@FMA_ID",
                                    SqlDbType.BigInt)
                                            {
                                                Value = "0"
                                            });

                                            cmd.Parameters.Add(new SqlParameter("@User_id",
                                   SqlDbType.BigInt)
                                            {
                                                Value = pgmod.user_id
                                            });
                                if (ph.FMA_DueDate != null)
                                {
                                    cmd.Parameters.Add(new SqlParameter("@DueDate",
                                        SqlDbType.DateTime)
                                    {
                                        Value = ph.FMA_DueDate
                                    });
                                }
                                else
                                {
                                    cmd.Parameters.Add(new SqlParameter("@DueDate",
                                        SqlDbType.DateTime)
                                    {
                                        Value = DateTime.Now
                                    });

                                }
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();
                                            var data = cmd.ExecuteNonQuery();

                                            if (data >= 1)
                                            {
                                                pgmod.returnduplicatestatus = "true";
                                                pgmod.returnduplicatestatus = "Saved";
                                            }
                                            else
                                            {
                                                pgmod.returnduplicatestatus = "false";
                                            }
                                        }
                                    }
                                  
                                
                          

                     






                    }
                }


                pgmod.amountentrydata = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                         from b in _FeeGroupContext.FeeGroupDMO
                                         from c in _FeeGroupContext.FeeHeadDMO
                                         from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                         where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id)
                                         select new FeeWizardDTO
                                         {
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_FeeName = c.FMH_FeeName,
                                             FTI_Name = d.FTI_Name,
                                             FMA_Amount = a.FMA_Amount,
                                             FMA_Id = a.FMA_Id
                                         }
   ).OrderBy(t => t.FMH_FeeName).ToArray();
    
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return pgmod;


        }


        public FeeWizardDTO deleterecfma(FeeWizardDTO page)
        {
            try
            {
                
                    using (var transaction = _FeeGroupContext.Database.BeginTransaction())
                    {
                        // FeeWizardDTO page = new FeeWizardDTO();
                        List<FeeAmountEntryDMO> lorg = new List<FeeAmountEntryDMO>();
                        List<FeeTDueDateRegularDMO> lorgduedate = new List<FeeTDueDateRegularDMO>();
                        List<FeeTDueDateECSDMO> lorgduedateecs = new List<FeeTDueDateECSDMO>();
                        List<FeeTFineSlabDMO> fineslab = new List<FeeTFineSlabDMO>();
                        List<FeeTFineSlabECSDMO> fineslabecs = new List<FeeTFineSlabECSDMO>();

                        List<FeeStudentTransactionDMO> feestutrans = new List<FeeStudentTransactionDMO>();

                        lorg = _FeeGroupContext.FeeAmountEntryDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        lorgduedate = _FeeGroupContext.feeTDueDateRegularDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        lorgduedateecs = _FeeGroupContext.feeTDueDateECSDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        fineslab = _FeeGroupContext.feeTFineSlabDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        fineslabecs = _FeeGroupContext.feeTFineSlabECSDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id)).ToList();

                        feestutrans = _FeeGroupContext.FeeStudentTransactionDMO.Where(t => t.FMA_Id.Equals(page.FMA_Id) && t.FSS_PaidAmount > 0 && t.MI_Id == page.MI_Id && t.ASMAY_Id == page.ASMAY_Id).ToList();

                        try
                        {
                            if (feestutrans.Count < 1)
                            {
                                if (lorg.Any())
                                {
                                    if (lorgduedate.Count > 0)
                                    {
                                        _FeeGroupContext.Remove(lorgduedate.ElementAt(0));
                                        var contactExists = _FeeGroupContext.SaveChanges();
                                    }
                                    if (lorgduedateecs.Count > 0)
                                    {
                                        _FeeGroupContext.Remove(lorgduedateecs.ElementAt(0));
                                        var contactExists = _FeeGroupContext.SaveChanges();
                                    }
                                    if (fineslab.Count > 0)
                                    {
                                        _FeeGroupContext.Remove(fineslab.ElementAt(0));
                                        var contactExists = _FeeGroupContext.SaveChanges();
                                    }
                                    if (fineslabecs.Count > 0)
                                    {
                                        _FeeGroupContext.Remove(fineslabecs.ElementAt(0));
                                        var contactExists = _FeeGroupContext.SaveChanges();
                                    }
                                    if (lorg.Count > 0)
                                    {
                                        _FeeGroupContext.Remove(lorg.ElementAt(0));
                                        var contactExists = _FeeGroupContext.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            page.returnduplicatestatus = "true";
                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            page.returnduplicatestatus = "false";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                page.returnduplicatestatus = "RecordExists";
                            }

                            page.amountentrydata = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                            from b in _FeeGroupContext.FeeGroupDMO
                                            from c in _FeeGroupContext.FeeHeadDMO
                                            from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                            where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == page.MI_Id)
                                            select new FeeWizardDTO
                                            {
                                                FMG_GroupName = b.FMG_GroupName,
                                                FMH_FeeName = c.FMH_FeeName,
                                                FTI_Name = d.FTI_Name,
                                                FMA_Amount = a.FMA_Amount
                                            }
                            ).ToArray();

                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                            _logger.LogError(ee.Message);
                            page.returnduplicatestatus = "false";
                        }

                    }
           
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return page;
        }


        public FeeWizardDTO savedetailFMAG(FeeWizardDTO data)
        {
            string retval = "";
            bool returnresult = false;
            string dupl = "false";
            try
            {
                if (data.resultData.Count() > 0)
                {
                    foreach (FeeWizardDTO ph in data.resultData)
                    {

                        //extra
                        var checkduplicatwo = _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO.Where(t => t.ASMAY_Id == data.ASMAY_Idnew && t.FGAR_PrefixName == ph.FGAR_PrefixName && t.FGAR_SuffixName == ph.FGAR_SuffixName).ToList();
                        if (checkduplicatwo.Count > 0)
                        {
                            data.returnduplicatestatus = "duplicate";
                        }
                        else
                        {
                            Fee_Groupwise_AutoReceiptDMO FAGR = new Fee_Groupwise_AutoReceiptDMO();
                            FAGR.MI_Id = data.MI_Id;
                            FAGR.ASMAY_Id = data.ASMAY_Idnew;
                           

                            FAGR.FGAR_PrefixFlag = ph.FGAR_PrefixFlag;
                            FAGR.FGAR_PrefixName = ph.FGAR_PrefixName;

                            FAGR.FGAR_Starting_No = ph.FGAR_Starting_No;
                            FAGR.FGAR_Template_Name = ph.FGAR_Template_Name;
                            FAGR.FGAR_SuffixFlag = ph.FGAR_SuffixFlag;
                            FAGR.FGAR_SuffixName = ph.FGAR_SuffixName;
                            FAGR.FGAR_Name = ph.FGAR_Name;
                            FAGR.FGAR_Address = ph.FGAR_Address;

                            FAGR.FGAR_CreatedBy = data.user_id;
                            FAGR.FGAR_UpdatedBy = data.user_id;
                            FAGR.FGAR_CreatedDate = DateTime.Now;
                            FAGR.FGAR_UpdatedDate = DateTime.Now;

                            _FeeGroupContext.Add(FAGR);
                            var check_exist = 0;
                           
                                var res = (from x in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                           from y in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                           where (x.MI_Id == data.MI_Id && x.ASMAY_Id == data.ASMAY_Idnew && x.FGAR_Id == y.FGAR_Id && x.FGAR_PrefixName == ph.FGAR_PrefixName && x.FGAR_SuffixName == ph.FGAR_SuffixName && y.FMG_Id == ph.FMG_Id)
                                           select y).ToList();
                                if (res.Count == 0)
                                {
                                    check_exist += 1;
                                    Fee_Groupwise_AutoReceipt_GroupsDMO fgag = new Fee_Groupwise_AutoReceipt_GroupsDMO();

                                    fgag.FGAR_Id = FAGR.FGAR_Id;
                                    fgag.FMG_Id = ph.FMG_Id;

                                    _FeeGroupContext.Add(fgag);
                                }

                            var contactexisttransaction = 0;
                            using (var dbCtxTxn = _FeeGroupContext.Database.BeginTransaction())
                            {
                                if (check_exist > 0)
                                {
                                    try
                                    {
                                        contactexisttransaction = _FeeGroupContext.SaveChanges();
                                        dbCtxTxn.Commit();
                                        data.returnduplicatestatus = "true";

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        dbCtxTxn.Rollback();
                                        data.returnduplicatestatus = "false";
                                    }
                                }
                                else
                                {
                                    data.returnduplicatestatus = "duplicate";
                                }
                            }
                        }



                    }
                }




                data.autoreceiptdata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                         from d in _FeeGroupContext.AcademicYear
                                         where (a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id)
                                         select new FeeWizardDTO
                                         {
                                             FGAR_PrefixName = a.FGAR_PrefixName,
                                             FGAR_SuffixName = a.FGAR_SuffixName,
                                             ASMAY_Name = d.ASMAY_Year,
                                             FGAR_Id = a.FGAR_Id,
                                             FGAR_Starting_No = a.FGAR_Starting_No,
                                             ASMAY_Id = a.ASMAY_Id,
                                         }
          ).OrderByDescending(t => t.FGAR_Id).ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return data;


        }


        public FeeWizardDTO deletedta(FeeWizardDTO data)
        {
            try
            {
                var autoreceiptflag = _FeeGroupContext.FeeMasterConfiguration.Where(t => t.MI_Id == data.MI_Id);

                if (autoreceiptflag.FirstOrDefault().FMC_AutoReceiptFeeGroupFlag == 1)
                {
                    var groupdata = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(t => t.FGAR_Id == data.FGAR_Id).Select(t => t.FMG_Id).ToList();

                    var check_validation = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                            from b in _FeeGroupContext.FeeTransactionPaymentDMO
                                            from c in _FeeGroupContext.FeeAmountEntryDMO
                                            where (a.FYP_Id == b.FYP_Id && b.FMA_Id == c.FMA_Id && a.ASMAY_ID == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && groupdata.Contains(c.FMG_Id))
                                            select new Fee_Groupwise_AutoReceiptDTO
                                            {
                                                FGAR_PrefixName = a.FYP_Receipt_No
                                            }
              ).OrderByDescending(t => t.FGAR_Id).ToList();

                    if (check_validation.Count == 0)
                    {
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Delete_Auto_Receipt_Group";

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@FGAR_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.FGAR_Id
                            });

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

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            if (data1 > 0)
                            {
                                data.returnduplicatestatus = "1";
                            }
                            else
                            {
                                data.returnduplicatestatus = "2";
                            }
                        }
                    }
                    else
                    {
                        data.returnduplicatestatus = "3";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
