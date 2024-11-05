using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
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

namespace FeeServiceHub.com.vaps.services
{
    public class FeeSpecialHeadImpl : interfaces.FeeSpecialHeadInterface
    {

        private static ConcurrentDictionary<string, FeeSpecialFeeGroupDTO> _login =
        new ConcurrentDictionary<string, FeeSpecialFeeGroupDTO>();
        public FeeGroupContext _FeeGroupContext;
        public FeeSpecialHeadImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeSpecialFeeGroupDTO SaveYearlyGroupDataY(FeeSpecialFeeGroupDTO FGpage)
        {
            string dupl = "";
            string retval = "";
            bool returnresult = false;
            int Id = 0;
            FeeSpecialFeeGroupDMO feepge = Mapper.Map<FeeSpecialFeeGroupDMO>(FGpage);

            try
            {
                if (FGpage.FMSFHFH_Id > 0)
                {
                    List<FeeSpecialFeeGroupsGroupingDMO> allrecords = new List<FeeSpecialFeeGroupsGroupingDMO>();
                    allrecords = _FeeGroupContext.feeSGGG.Where(t => t.FMSFHFH_Id == FGpage.FMSFHFH_Id).ToList();
                    if (allrecords.Count > 0)
                    {
                        var result = _FeeGroupContext.feespecialHead.Single(t => t.FMSFH_Id == allrecords[0].FMSFH_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMSFH_Name = feepge.FMSFH_Name;
                        result.FMSFH_ActiceFlag = feepge.FMSFH_ActiceFlag;
                        result.IVRMSTAUL_Id = feepge.IVRMSTAUL_Id;
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        result.UpdatedDate = indianTime;
                      //  result.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Update(result);
                        var contactExistsnew = _FeeGroupContext.SaveChanges();
                        if (contactExistsnew == 1)
                        {
                            for (int j = 0; j < FGpage.TempararyArrayList.Length; j++)
                            {
                               
                              var  allrecordsnew = _FeeGroupContext.feeSGGG.Where(t => t.FMSFHFH_Id == FGpage.FMSFHFH_Id).ToList();
                                if (allrecordsnew.Count> 0 )
                                {
                                    foreach (var item in allrecordsnew)
                                    {
                                        _FeeGroupContext.Remove(item);
                                    }
                                    _FeeGroupContext.SaveChanges();
                                }
                                FeeSpecialFeeGroupsGroupingDMO res1 = new FeeSpecialFeeGroupsGroupingDMO();
                                TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime indianTime1 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                var checkexist = _FeeGroupContext.feeSGGG.Where(r => r.FMSFH_Id == result.FMSFH_Id && r.FMH_Id == FGpage.TempararyArrayList[j].FMH_ID).ToList();
                                if (checkexist.Count==0)
                                {
                                    res1.UpdatedDate = indianTime1;
                                    res1.CreatedDate = indianTime1;
                                    res1.FMSFH_Id = result.FMSFH_Id;
                                    res1.FMH_Id = FGpage.TempararyArrayList[j].FMH_ID;
                                    res1.FMSFHFH_ActiceFlag = true;
                                    _FeeGroupContext.Add(res1);
                                    var con = _FeeGroupContext.SaveChanges();
                                    if (con == 1)
                                    {
                                        returnresult = true;
                                        FGpage.returnval = returnresult;
                                        FGpage.returnduplicatestatus = "Updated";
                                    }
                                    else
                                    {
                                        returnresult = false;
                                        FGpage.returnval = returnresult;
                                        FGpage.returnduplicatestatus = "Not Updated";
                                    }
                                }
                                

                                
                             //   Id = Convert.ToInt32(FGpage.TempararyArrayList[j].FMH_ID);
                             //   List<FeeSpecialFeeGroupsGroupingDMO> allrecordsnew = new List<FeeSpecialFeeGroupsGroupingDMO>();
                             //allrecordsnew = _FeeGroupContext.feeSGGG.Where(t => t.FMSFHFH_Id == FGpage.FMSFHFH_Id).ToList();
                             //   if (allrecordsnew.Count > 0)
                             //   {
                             //       var newresult = _FeeGroupContext.feeSGGG.Single(t => t.FMSFHFH_Id == FGpage.FMSFHFH_Id);
                             //       newresult.FMH_Id = Id;
                             //       TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                             //       DateTime indianTime1 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                             //       newresult.UpdatedDate = indianTime1;
                             //       _FeeGroupContext.Update(newresult);
                             //       var con = _FeeGroupContext.SaveChanges();
                             //       if (con == 1)
                             //       {
                             //           returnresult = true;
                             //           FGpage.returnval = returnresult;
                             //           FGpage.returnduplicatestatus = "Updated";
                             //       }
                             //       else
                             //       {
                             //           returnresult = false;
                             //           FGpage.returnval = returnresult;
                             //           FGpage.returnduplicatestatus = "Not Updated";
                             //       }
                             //   }
                            }
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                else
                {

                    var result = _FeeGroupContext.feespecialHead.Where(t => t.FMSFH_Name == feepge.FMSFH_Name);
                    if (result.Count() > 0)
                    {
                        List<FeeSpecialFeeGroupDMO> allrecordsbase = new List<FeeSpecialFeeGroupDMO>();
                        allrecordsbase = _FeeGroupContext.feespecialHead.Where(t => t.FMSFH_Name == feepge.FMSFH_Name).ToList();

                        for (int j = 0; j < FGpage.TempararyArrayList.Length; j++)
                        {
                            Id = Convert.ToInt32(FGpage.TempararyArrayList[j].FMH_ID);
                            List<FeeSpecialFeeGroupsGroupingDMO> allrecords = new List<FeeSpecialFeeGroupsGroupingDMO>();
                            allrecords = _FeeGroupContext.feeSGGG.Where(t => t.FMH_Id == Id).ToList();
                            if (allrecords.Count > 0)
                            {
                                for (int i = 0; allrecords.Count > i; i++)
                                {
                                    List<FeeSpecialFeeGroupsGroupingDMO> allrecordscheck = new List<FeeSpecialFeeGroupsGroupingDMO>();
                                    allrecordscheck = _FeeGroupContext.feeSGGG.Where(t => t.FMH_Id == Id && t.FMSFH_Id == allrecordsbase[0].FMSFH_Id).ToList();
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
                                    FGpage.returnduplicatestatus = retval;
                                }
                                else
                                {
                                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                    
                                    var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_Master_SpecialFeeHead_FeeHead @p0,@p1,@p2,@p3,@p4,@p5,@p6", feepge.MI_Id, feepge.FMSFH_Name, feepge.IVRMSTAUL_Id, Id, feepge.FMSFH_ActiceFlag, feepge.FMSFH_Id, indianTime);
                                    if (contactExists >= 1)
                                    {
                                        returnresult = true;
                                        FGpage.returnval = returnresult;
                                         FGpage.returnduplicatestatus = "Saved";
                                    }
                                    else
                                    {
                                        returnresult = false;
                                        FGpage.returnval = returnresult;
                                        FGpage.returnduplicatestatus = "Not Saved";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < FGpage.TempararyArrayList.Length; j++)
                        {
                            Id = Convert.ToInt32(FGpage.TempararyArrayList[j].FMH_ID);
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                            var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_Master_SpecialFeeHead_FeeHead @p0,@p1,@p2,@p3,@p4,@p5,@p6", feepge.MI_Id, feepge.FMSFH_Name, feepge.IVRMSTAUL_Id, Id, feepge.FMSFH_ActiceFlag, feepge.FMSFH_Id, indianTime);
                            if (contactExists >= 1)
                            {
                                returnresult = true;
                                FGpage.returnval = returnresult;
                                FGpage.returnduplicatestatus = "Saved";
                            }
                            else
                            {
                                returnresult = false;
                                FGpage.returnval = returnresult;
                                FGpage.returnduplicatestatus = "Not Saved";
                            }
                        }
                    }
                }
                FGpage.RetrivefeeHeadData = (from a in _FeeGroupContext.feehead
                                             from b in _FeeGroupContext.feeSGGG
                                             from c in _FeeGroupContext.feespecialHead
                                             where (a.FMH_Id == b.FMH_Id && b.FMSFH_Id == c.FMSFH_Id && c.MI_Id == FGpage.MI_Id)
                                             select new FeeSpecialFeeGroupDTO
                                             {
                                                 FMSFH_Name = c.FMSFH_Name,
                                                 FMH_Name = a.FMH_FeeName,
                                                 FMSFHFH_Id = b.FMSFHFH_Id

                                             }
      ).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            //   }
            return FGpage;
        }
        public FeeSpecialFeeGroupDTO getdetailsY(int id)
        {
            FeeSpecialFeeGroupDTO FGRDT = new FeeSpecialFeeGroupDTO();
            try
            {
                List<FeeHeadDMO> feeheads = new List<FeeHeadDMO>();
                feeheads = _FeeGroupContext.feehead.Where(t => t.MI_Id == id && t.FMH_SpecialFeeFlag==true && t.FMH_ActiveFlag==true).OrderBy(t=>t.FMH_Order).ToList();
                FGRDT.GroupHeadData = feeheads.ToArray();

                List<FeeSpecialFeeGroupDMO> feegrp = new List<FeeSpecialFeeGroupDMO>();
                feegrp = _FeeGroupContext.feespecialHead.Where(t => t.MI_Id == id).ToList();
                FGRDT.RetrivefeeHeadData = feegrp.ToArray();

                FGRDT.newarydatah = (from a in _FeeGroupContext.feehead
                                     from b in _FeeGroupContext.feeSGGG
                                     from c in _FeeGroupContext.feespecialHead
                                     where (a.FMH_Id == b.FMH_Id && b.FMSFH_Id == c.FMSFH_Id && c.MI_Id == id)
                                     select new FeeSpecialFeeGroupDTO
                                     {
                                         FMSFH_Name = c.FMSFH_Name,
                                         FMH_Name = a.FMH_FeeName,
                                         FMSFHFH_Id = b.FMSFHFH_Id,
                                         FMSFHFH_ActiceFlag = b.FMSFHFH_ActiceFlag
                                     }
       ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeSpecialFeeGroupDTO deactivateY(FeeSpecialFeeGroupDTO acd)
        {
            try
            {
                FeeSpecialFeeGroupsGroupingDMO lorg1 = new FeeSpecialFeeGroupsGroupingDMO();
                if (acd.FMSFHFH_Id > 0)               {
                   
                    var result = _FeeGroupContext.feeSGGG.Single(t => t.FMSFHFH_Id == acd.FMSFHFH_Id);

                    var feestutrans = _FeeGroupContext.FeeStudentTransactionDMO.Where(t => t.FMH_Id == result.FMH_Id).ToList();
                    if (feestutrans.Count > 0)
                    {
                        acd.returnduplicatestatus = "used";
                        return acd;
                    }
                    else
                    {
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                        result.UpdatedDate = indianTime;
                        // result.UpdatedDate = DateTime.Now;

                        if (result.FMSFHFH_ActiceFlag == true)
                        {
                            result.FMSFHFH_ActiceFlag = false;
                        }
                        else
                        {
                            result.FMSFHFH_ActiceFlag = true;
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
                }
                else
                {
                    acd.returnval = false;
                }


                List<FeeSpecialFeeGroupDMO> allorganisation = new List<FeeSpecialFeeGroupDMO>();
                allorganisation = _FeeGroupContext.feespecialHead.ToList();
                acd.RetrivefeeHeadData = allorganisation.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public FeeSpecialFeeGroupDTO getpageeditY(int id)
        {
            FeeSpecialFeeGroupDTO page = new FeeSpecialFeeGroupDTO();
            try
            {
                List<FeeSpecialFeeGroupsGroupingDMO> Allname101 = new List<FeeSpecialFeeGroupsGroupingDMO>();
                Allname101 = _FeeGroupContext.feeSGGG.AsNoTracking().Where(t => t.FMSFHFH_Id.Equals(id)).ToList();

                List<FeeSpecialFeeGroupDMO> lorg = new List<FeeSpecialFeeGroupDMO>();
                lorg = _FeeGroupContext.feespecialHead.AsNoTracking().Where(t => t.FMSFH_Id.Equals(Allname101[0].FMSFH_Id)).ToList();

                page.FMSFH_Name = lorg[0].FMSFH_Name;
                page.editidh = Allname101[0].FMH_Id;
                page.FMSFHFH_Id = id;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeSpecialFeeGroupDTO deleterecY(FeeSpecialFeeGroupDTO data)
        {

            List<FeeSpecialFeeGroupsGroupingDMO> lorg1 = new List<FeeSpecialFeeGroupsGroupingDMO>();
            var result = _FeeGroupContext.feeSGGG.Single(t => t.FMSFHFH_Id.Equals(data.FMSFHFH_Id));
            var dd = result.FMSFH_Id;
            if (dd > 0)
            {
                deleterec(data.FMSFHFH_Id);
            }
            else
            {

            }

            bool returnresult = false;
            FeeSpecialFeeGroupDTO page = new FeeSpecialFeeGroupDTO();
            List<FeeSpecialFeeGroupDMO> lorg = new List<FeeSpecialFeeGroupDMO>();
            lorg = _FeeGroupContext.feespecialHead.Where(t => t.FMSFH_Id.Equals(dd)).ToList();



            var result1 = _FeeGroupContext.feeSGGG.Where(t => t.FMSFH_Id == dd);

            if (result1.Count() > 0)
            {
                returnresult = true;
                page.returnval = returnresult;
            }

            else
            {
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

                    page.RetrivefeeHeadData = (from a in _FeeGroupContext.feehead
                                               from b in _FeeGroupContext.feeSGGG
                                               from c in _FeeGroupContext.feespecialHead
                                               where (a.FMH_Id == b.FMH_Id && b.FMSFH_Id == c.FMSFH_Id && c.MI_Id ==data.MI_Id)
                                               select new FeeSpecialFeeGroupDTO
                                               {
                                                   FMSFH_Name = c.FMSFH_Name,
                                                   FMH_Name = a.FMH_FeeName,
                                                   FMSFHFH_Id = b.FMSFHFH_Id

                                               }
        ).ToArray();


                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            return page;
        }
        public FeeSpecialFeeGroupsGroupDTO saveyeardata(long grpid, long ggrpid)
        {
            FeeSpecialFeeGroupsGroupDTO te = new FeeSpecialFeeGroupsGroupDTO();
            FeeSpecialFeeGroupsGroupingDMO feepgeY = Mapper.Map<FeeSpecialFeeGroupsGroupingDMO>(te);

            try
            {
                if (te.FMSFHFH_Id > 0)
                {
                    var result = _FeeGroupContext.feeSGGG.Single(t => t.FMSFHFH_Id == feepgeY.FMSFHFH_Id);
                    result.FMH_Id = grpid;  // head id
                    result.FMSFH_Id = ggrpid;   // special head id
                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        te.returnval = true;
                        te.returnduplicatestatus = "Updated";
                    }
                    else
                    {
                        te.returnval = false;
                        te.returnduplicatestatus = "Not Updated";
                    }

                }
                else
                {
                    feepgeY.FMH_Id = grpid; // head id
                    feepgeY.FMSFH_Id = ggrpid;// special head id
                    _FeeGroupContext.Add(feepgeY);
                    var contactExists = _FeeGroupContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        te.returnval = true;
                        te.returnduplicatestatus = "Saved";
                    }
                    else
                    {
                        te.returnval = false;
                        te.returnduplicatestatus = "Not Saved";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            te.FeeSpecialgroupgrping = (from a in _FeeGroupContext.feehead
                                        from b in _FeeGroupContext.feeSGGG
                                        from c in _FeeGroupContext.feespecialHead
                                        where (a.FMH_Id == b.FMH_Id && b.FMSFH_Id == c.FMSFH_Id && c.MI_Id == 2)
                                        select new FeeSpecialFeeGroupDTO
                                        {
                                            FMSFH_Name = c.FMSFH_Name,
                                            FMH_Name = a.FMH_FeeName

                                        }
      ).ToArray();

            return te;
        }
        public FeeSpecialFeeGroupDTO[] getdata()
        {
            List<FeeSpecialFeeGroupDTO> AllInOne = new List<FeeSpecialFeeGroupDTO>();
            List<FeeSpecialFeeGroupDMO> Allrows = new List<FeeSpecialFeeGroupDMO>();
            Allrows = _FeeGroupContext.feespecialHead.ToList();
            for (int i = 0; i < Allrows.Count; i++)
            {
                FeeSpecialFeeGroupDTO temp = new FeeSpecialFeeGroupDTO();
                temp.FMSFH_Id = Allrows[i].FMSFH_Id;
                temp.FMSFH_Name = Allrows[i].FMSFH_Name;
                List<FeeSpecialFeeGroupsGroupingDMO> Allname1011 = new List<FeeSpecialFeeGroupsGroupingDMO>();
                Allname1011 = _FeeGroupContext.feeSGGG.Where(t => t.FMSFH_Id.Equals(Allrows[i].FMSFH_Id)).ToList().ToList();  // head id 
                List<FeeHeadDMO> Allname101 = new List<FeeHeadDMO>();
                Allname101 = _FeeGroupContext.feehead.Where(t => t.FMH_Id.Equals(Allname1011[0].FMH_Id)).ToList().ToList();  // head id                 
                temp.hdname = Allname101[0].FMH_FeeName;
                temp.FMSFH_ActiceFlag = Allrows[i].FMSFH_ActiceFlag;
                AllInOne.Add(temp);
            }
            return AllInOne.ToArray();
        }

        //public FeeSpecialFeeGroupsGroupDTO deleterec(long grpid)
        //{

        //    List<FeeSpecialFeeGroupsGroupingDMO> lorg1 = new List<FeeSpecialFeeGroupsGroupingDMO>();
        //    var result = _FeeGroupContext.feeSGGG.Single(t => t.FMSFHFH_Id.Equals(grpid));
        //    var dd = result.FMSFH_Id;

        //    FeeSpecialFeeGroupsGroupDTO te = new FeeSpecialFeeGroupsGroupDTO();
        //    FeeSpecialFeeGroupsGroupingDMO feepgeY = Mapper.Map<FeeSpecialFeeGroupsGroupingDMO>(te);
        //    bool returnresult = false;
        //    FeeSpecialFeeGroupDTO page = new FeeSpecialFeeGroupDTO();
        //    List<FeeSpecialFeeGroupsGroupingDMO> lorg = new List<FeeSpecialFeeGroupsGroupingDMO>();
        //    lorg = _FeeGroupContext.feeSGGG.Where(t => t.FMSFHFH_Id.Equals(grpid)).ToList();
        //    try
        //    {
        //        if (lorg.Any())
        //        {
        //            _FeeGroupContext.Remove(lorg.ElementAt(0));

        //            var contactExists = _FeeGroupContext.SaveChanges();
        //            if (contactExists == 1)
        //            {
        //                returnresult = true;
        //                page.returnval = returnresult;
        //            }
        //            else
        //            {
        //                returnresult = false;
        //                page.returnval = returnresult;
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    List<FeeSpecialFeeGroupsGroupingDMO> allpages = new List<FeeSpecialFeeGroupsGroupingDMO>();
        //    allpages = _FeeGroupContext.feeSGGG.OrderBy(t => t.FMSFHFH_Id).ToList();
        //    te.Feegroupgrping = allpages.ToArray();
        //    return te;
        //}

        public string deleterec(long grpid)
        {

            List<FeeSpecialFeeGroupsGroupingDMO> lorg1 = new List<FeeSpecialFeeGroupsGroupingDMO>();
            var result = _FeeGroupContext.feeSGGG.Single(t => t.FMSFHFH_Id.Equals(grpid));
            var dd = result.FMSFH_Id.ToString();



            FeeSpecialFeeGroupsGroupDTO page1 = new FeeSpecialFeeGroupsGroupDTO();
            FeeSpecialFeeGroupsGroupingDMO feepgeY = Mapper.Map<FeeSpecialFeeGroupsGroupingDMO>(page1);
            bool returnresult = false;

            FeeSpecialFeeGroupDTO page = new FeeSpecialFeeGroupDTO();

            List<FeeSpecialFeeGroupsGroupingDMO> lorg = new List<FeeSpecialFeeGroupsGroupingDMO>();
            lorg = _FeeGroupContext.feeSGGG.Where(t => t.FMSFHFH_Id.Equals(grpid)).ToList();
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            List<FeeSpecialFeeGroupsGroupingDMO> allpages = new List<FeeSpecialFeeGroupsGroupingDMO>();
            allpages = _FeeGroupContext.feeSGGG.OrderBy(t => t.FMSFHFH_Id).ToList();
            page.Feegroupgrping = allpages.ToArray();
            return dd;
        }
    }
}
