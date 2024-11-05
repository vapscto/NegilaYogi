using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace FeeServiceHub.com.vaps.services
{
    public class MasterNarrationImpl : interfaces.MasterNarrationInterface
    {
        private static ConcurrentDictionary<string, MasterNarrationDTO> _login =
       new ConcurrentDictionary<string, MasterNarrationDTO>();

        public FeeGroupContext _FeeGroupHeadContext;
        public DomainModelMsSqlServerContext _context;


        public MasterNarrationImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext db)
        {

            _FeeGroupHeadContext = frgContext;
            _context = db;

        }

        public MasterNarrationDTO SaveGroupData(MasterNarrationDTO FGpage)
        {
            bool returnresult = false;
            MasterNarrationDMO feepge = Mapper.Map<MasterNarrationDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FMNAR_Id > 0)
                {

                    //var result1 = _FeeGroupHeadContext.MasterNarrationDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.FMT_Id == feepge.FMT_Id && t.FMH_Order == feepge.FMH_Order).ToList();
                    var result1 = _FeeGroupHeadContext.MasterNarrationDMO.Where(t => t.FMNAR_Id != feepge.FMNAR_Id && t.MI_ID == feepge.MI_ID  && t.FMNAR_Narration==feepge.FMNAR_Narration && t.FMNAR_NarrationDesc==feepge.FMNAR_NarrationDesc).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {


                        var result = _FeeGroupHeadContext.MasterNarrationDMO.Single(t => t.FMNAR_Id == feepge.FMNAR_Id);
                        result.MI_ID = FGpage.MI_ID;
                        result.FMNAR_Narration = FGpage.FMNAR_Narration;
                        result.FMNAR_NarrationDesc = FGpage.FMNAR_NarrationDesc;
                        result.FMNAR_ActiveFlag = true;
                      
                      
                        _FeeGroupHeadContext.Update(result);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                            FGpage.message = "Update";
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
                    // var result = _FeeGroupHeadContext.MasterNarrationDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.FMT_Id== feepge.FMT_Id && t.FMH_Order==feepge.FMH_Order).ToList();
                    var result = _FeeGroupHeadContext.MasterNarrationDMO.Where(t => t.MI_ID == feepge.MI_ID && t.FMNAR_Narration == feepge.FMNAR_Narration && t.FMNAR_NarrationDesc == feepge.FMNAR_NarrationDesc && t.FMNAR_Id == feepge.FMNAR_Id).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.MI_ID = FGpage.MI_ID;
                        feepge.FMNAR_Narration = FGpage.FMNAR_Narration;
                        feepge.FMNAR_NarrationDesc = FGpage.FMNAR_NarrationDesc;
                        feepge.FMNAR_ActiveFlag = true;
                      

                        _FeeGroupHeadContext.Add(feepge);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {


                            returnresult = true;
                            FGpage.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }

                FGpage.GroupHeadData = (from A in _FeeGroupHeadContext.MasterNarrationDMO
                                       
                                        where (A.MI_ID == FGpage.MI_ID
                                        )
                                        select new MasterNarrationDTO
                                        {
                                            FMNAR_Id = A.FMNAR_Id,
                                            FMNAR_Narration = A.FMNAR_Narration,
                                            FMNAR_NarrationDesc = A.FMNAR_NarrationDesc,
                                            FMNAR_ActiveFlag=A.FMNAR_ActiveFlag



                                        }
                        ).ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }


        public MasterNarrationDTO getdetails(int id)
        {
            MasterNarrationDTO FGRDT = new MasterNarrationDTO();

            try
            {

                FGRDT.GroupHeadData = (from A in _FeeGroupHeadContext.MasterNarrationDMO

                                        where (A.MI_ID == id
                                        )
                                        select new MasterNarrationDTO
                                        {
                                            FMNAR_Id = A.FMNAR_Id,
                                            FMNAR_Narration = A.FMNAR_Narration,
                                            FMNAR_NarrationDesc = A.FMNAR_NarrationDesc,
                                            FMNAR_ActiveFlag = A.FMNAR_ActiveFlag



                                        }
                         ).ToArray();






            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public MasterNarrationDTO EditgroupDetails(int id)
        {
            MasterNarrationDTO FMG = new MasterNarrationDTO();
            try
            {
                List<MasterNarrationDMO> masterfeegroup = new List<MasterNarrationDMO>();
                masterfeegroup = _FeeGroupHeadContext.MasterNarrationDMO.Where(t => t.FMNAR_Id.Equals(id)).ToList();
                FMG.GroupHeadData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public MasterNarrationDTO GetGroupSearchData(MasterNarrationDTO mas)
        {

            MasterNarrationDTO FGRDT = new MasterNarrationDTO();
            try
            {
                List<MasterNarrationDMO> feegrp = new List<MasterNarrationDMO>();
                feegrp = _FeeGroupHeadContext.MasterNarrationDMO.OrderBy(t => t.FMNAR_Id).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public MasterNarrationDTO getpageedit(int id)
        {
            MasterNarrationDTO page = new MasterNarrationDTO();
            try
            {
                List<MasterNarrationDMO> lorg = new List<MasterNarrationDMO>();
                lorg = _FeeGroupHeadContext.MasterNarrationDMO.Where(t => t.FMNAR_Id.Equals(id)).ToList();
                page.GroupHeadData = lorg.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public MasterNarrationDTO deactivate(MasterNarrationDTO acd)
        {
            try
            {
                MasterNarrationDMO feepge = Mapper.Map<MasterNarrationDMO>(acd);
                if (feepge.FMNAR_Id > 0)
                {

                    var result = _FeeGroupHeadContext.MasterNarrationDMO.Single(t => t.FMNAR_Id == feepge.FMNAR_Id);



                    _FeeGroupHeadContext.Remove(result);
                    var flag = _FeeGroupHeadContext.SaveChanges();
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
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

    }
}
