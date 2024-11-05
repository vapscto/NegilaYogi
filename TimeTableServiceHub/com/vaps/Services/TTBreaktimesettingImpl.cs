using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTBreaktimesettingImpl : Interfaces.BreaktimesettingsInterface
    {
        private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
         new ConcurrentDictionary<string, TTMasterCategoryDTO>();


        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _context;
        public TTBreaktimesettingImpl(TTContext ttcntx, DomainModelMsSqlServerContext masterclassContext)
        {
            _context = masterclassContext;
            _ttcontext = ttcntx;
        }

        public TTBreakTimesettingDTO savedetail(TTBreakTimesettingDTO _category)
        {
            TTBreakTimeSettingsDMO objpge = Mapper.Map<TTBreakTimeSettingsDMO>(_category);
            try
            {
                if (objpge.TTMB_Id > 0)
                {
                    _category.comparevlue = "NotDuplicateHere";
                    for (int i = 0; i < _category.ArrayClassList.Length; i++)
                    {
                        for (int j = 0; j < _category.ArrayDayList.Length; j++)
                        {
                            var result1 = _ttcontext.TTBreakTimeSettingsDMO.Where(t => t.TTMB_BreakName.Equals(objpge.TTMB_BreakName) && t.MI_Id.Equals(objpge.MI_Id)
                            && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.ASMCL_Id.Equals(_category.ArrayClassList[i].ASMCL_Id) && t.TTMD_Id.Equals(_category.ArrayDayList[j].TTMD_Id) && t.TTMB_AfterPeriod.Equals(objpge.TTMB_AfterPeriod) && t.TTMB_BreakStartTime.Equals(objpge.TTMB_BreakStartTime) && t.TTMB_BreakEndTime.Equals(objpge.TTMB_BreakEndTime));
                            if (result1.Count() > 0)
                            {
                                _category.comparevlue = "DuplicateHere";
                            }
                        }
                    }

                    if (_category.comparevlue.Equals("DuplicateHere"))
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcontext.TTBreakTimeSettingsDMO.Single(t => t.TTMB_Id.Equals(objpge.TTMB_Id) && t.MI_Id.Equals(objpge.MI_Id));
                        result.TTMB_BreakName = objpge.TTMB_BreakName;
                        result.ASMCL_Id = _category.ArrayClassList[0].ASMCL_Id;
                        result.TTMD_Id = _category.ArrayDayList[0].TTMD_Id;
                        result.TTMB_ActiveFlag = true;
                        result.TTMB_AfterPeriod = objpge.TTMB_AfterPeriod;
                        result.TTMB_BreakStartTime = objpge.TTMB_BreakStartTime;
                        result.TTMB_BreakEndTime = objpge.TTMB_BreakEndTime;
                        result.ASMAY_Id = objpge.ASMAY_Id;
                        result.UpdatedDate = DateTime.Now;
                        _ttcontext.Update(result);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            //  *********************before periods delete***************start**************************
                         
                            List<TT_Master_Break_BefPeriodsDMO> lorg = new List<TT_Master_Break_BefPeriodsDMO>();
                            lorg = _ttcontext.TT_Master_Break_BefPeriodsDMO.Where(t => t.TTMB_Id.Equals(objpge.TTMB_Id)).ToList();
                            if (lorg.Any())
                            {
                                for (int i = 0; i < lorg.Count; i++)
                                {
                                    _ttcontext.Remove(lorg.ElementAt(i));
                                    var contactExists2 = _ttcontext.SaveChanges();
                                    if (contactExists2.Equals(1))
                                    {
                                        _category.returnval = true;
                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
                            }
                            //  *********************before periods delete***************end**************************

                            //  *********************after periods delete***************start**************************
                         

                            List<TT_Master_Break_AftPeriodsDMO> lorg2 = new List<TT_Master_Break_AftPeriodsDMO>();
                            lorg2 = _ttcontext.TT_Master_Break_AftPeriodsDMO.Where(t => t.TTMB_Id.Equals(objpge.TTMB_Id)).ToList();
                            if (lorg2.Any())
                            {
                                for (int i = 0; i < lorg2.Count; i++)
                                {
                                    _ttcontext.Remove(lorg2.ElementAt(i));
                                    var contactExists2 = _ttcontext.SaveChanges();
                                    if (contactExists2.Equals(1))
                                    {
                                        _category.returnval = true;
                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
                            }
                            //  *********************after periods delete***************end**************************

                            //  *********************before periods Insertion***************start**************************
                            for (int k = 0; k < _category.ArraybeforeperiodsList.Length; k++)
                            {
                                TT_Master_Break_BefPeriodsDTO ttmbbpdto = new TT_Master_Break_BefPeriodsDTO();
                                TT_Master_Break_BefPeriodsDMO ttmbbpdmo = Mapper.Map<TT_Master_Break_BefPeriodsDMO>(ttmbbpdto);
                                ttmbbpdmo.TTMB_Id = objpge.TTMB_Id;
                                ttmbbpdmo.TTMP_ID =_ttcontext.TT_Master_PeriodDMO.Single(g => g.MI_Id == _category.MI_Id && g.TTMP_PeriodName == _category.ArraybeforeperiodsList[k].TTPeriodnameB).TTMP_Id;
                                ttmbbpdmo.CreatedDate = DateTime.Now;
                                ttmbbpdmo.UpdatedDate = DateTime.Now;
                                _ttcontext.Add(ttmbbpdmo);
                                var contactExists1 = _ttcontext.SaveChanges();
                                if (contactExists1.Equals(1))
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                            //  *********************before periods Insertion***************end**************************

                            //  *********************after periods Insertion***************start**************************
                            for (int y = 0; y < _category.ArrayafterperiodsList.Length; y++)
                            {
                                TT_Master_Break_AftPeriodsDTO ttmbapdto = new TT_Master_Break_AftPeriodsDTO();
                                TT_Master_Break_AftPeriodsDMO ttmbapdmo = Mapper.Map<TT_Master_Break_AftPeriodsDMO>(ttmbapdto);
                                ttmbapdmo.TTMB_Id = objpge.TTMB_Id;
                                ttmbapdmo.TTMP_ID =_ttcontext.TT_Master_PeriodDMO.Single(g => g.MI_Id == _category.MI_Id && g.TTMP_PeriodName == _category.ArrayafterperiodsList[y].TTPeriodnameA).TTMP_Id;
                               
                                ttmbapdmo.CreatedDate = DateTime.Now;
                                ttmbapdmo.UpdatedDate = DateTime.Now;
                                _ttcontext.Add(ttmbapdmo);
                                var contactExists1 = _ttcontext.SaveChanges();
                                if (contactExists1.Equals(1))
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                            //  *********************after periods Insertion***************end**************************
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                else
                {
                    _category.comparevlue = "NotDuplicateHere";
                    for (int i = 0; i < _category.ArrayClassList.Length; i++)
                    {
                        for (int j = 0; j < _category.ArrayDayList.Length; j++)
                        {
                            var result1 = _ttcontext.TTBreakTimeSettingsDMO.Where(t => t.TTMB_BreakName.Equals(objpge.TTMB_BreakName) && t.MI_Id.Equals(objpge.MI_Id)
                            && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.ASMCL_Id.Equals(_category.ArrayClassList[i].ASMCL_Id) && t.TTMD_Id.Equals(_category.ArrayDayList[j].TTMD_Id));
                            if (result1.Count() > 0)
                            {
                                _category.comparevlue = "DuplicateHere";
                                break;
                            }
                        }
                    }

                    if (_category.comparevlue.Equals("DuplicateHere"))
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        _category.returnduplicatestatus = "NotDuplicate";
                        for (int i = 0; i < _category.ArrayClassList.Length; i++)
                        {
                            objpge.ASMCL_Id = _category.ArrayClassList[i].ASMCL_Id;
                            for (int j = 0; j < _category.ArrayDayList.Length; j++)
                            {
                                TTBreakTimesettingDTO objpge111 = new TTBreakTimesettingDTO();
                                TTBreakTimeSettingsDMO objpge1 = Mapper.Map<TTBreakTimeSettingsDMO>(objpge111);
                                objpge1.TTMB_AfterPeriod = _category.TTMB_AfterPeriod;
                                objpge1.TTMB_BreakName = _category.TTMB_BreakName;
                                objpge1.TTMB_BreakStartTime = _category.TTMB_BreakStartTime;
                                objpge1.TTMB_BreakEndTime = _category.TTMB_BreakEndTime;
                                objpge1.TTMB_ActiveFlag = true;
                                objpge1.MI_Id = _category.MI_Id;
                                objpge1.ASMAY_Id = _category.ASMAY_Id;
                                objpge1.TTMC_Id = _category.TTMC_Id;
                                objpge1.ASMCL_Id = _category.ArrayClassList[i].ASMCL_Id;
                                objpge1.TTMD_Id = _category.ArrayDayList[j].TTMD_Id;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                _ttcontext.Add(objpge1);
                                var contactExists = _ttcontext.SaveChanges();
                                var result123 = _ttcontext.TTBreakTimeSettingsDMO.Max(t => t.TTMB_Id);
                                if (contactExists.Equals(1))
                                {
                                    //  *********************before periods Insertion***************start**************************
                                    for (int k = 0; k < _category.ArraybeforeperiodsList.Length; k++)
                                    {
                                        TT_Master_Break_BefPeriodsDTO ttmbbpdto = new TT_Master_Break_BefPeriodsDTO();
                                        TT_Master_Break_BefPeriodsDMO ttmbbpdmo = Mapper.Map<TT_Master_Break_BefPeriodsDMO>(ttmbbpdto);
                                        ttmbbpdmo.TTMB_Id = result123;
                                        ttmbbpdmo.TTMP_ID =_ttcontext.TT_Master_PeriodDMO.Single(g => g.MI_Id == _category.MI_Id && g.TTMP_PeriodName == _category.ArraybeforeperiodsList[k].TTPeriodnameB).TTMP_Id;
                                        ttmbbpdmo.CreatedDate = DateTime.Now;
                                        ttmbbpdmo.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(ttmbbpdmo);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1.Equals(1))
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
                                    //  *********************before periods Insertion***************end**************************

                                    //  *********************after periods Insertion***************start**************************
                                    for (int y = 0; y < _category.ArrayafterperiodsList.Length; y++)
                                    {
                                        TT_Master_Break_AftPeriodsDTO ttmbapdto = new TT_Master_Break_AftPeriodsDTO();
                                        TT_Master_Break_AftPeriodsDMO ttmbapdmo = Mapper.Map<TT_Master_Break_AftPeriodsDMO>(ttmbapdto);
                                        ttmbapdmo.TTMB_Id = result123;
                                        ttmbapdmo.TTMP_ID =_ttcontext.TT_Master_PeriodDMO.Single(g => g.MI_Id == _category.MI_Id && g.TTMP_PeriodName == _category.ArrayafterperiodsList[y].TTPeriodnameA).TTMP_Id;
                                      
                                        ttmbapdmo.CreatedDate = DateTime.Now;
                                        ttmbapdmo.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(ttmbapdmo);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1.Equals(1))
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
                                    //  *********************after periods Insertion***************end**************************
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                _category.returnval = false;

            }
            return _category;
        }
        public TTBreakTimesettingDTO getmaximumperiodscount(TTBreakTimesettingDTO _category)
        {
            TTBreakTimeSettingsDMO objpge = Mapper.Map<TTBreakTimeSettingsDMO>(_category);
            try
            {
                if (_category.classidscount > 1)
                {
                    var lorg = _ttcontext.TT_Master_PeriodDMO.Count(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMP_ActiveFlag.Equals(true));
                    _category.classidscountreturn = lorg;
                }
                else
                {
                    var lorg = _ttcontext.TT_Master_PeriodDMO.Count(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMP_ActiveFlag.Equals(true));
                    _category.classidscountreturn = lorg;
                }
            }
            catch (Exception ee)
            {
                _category.returnval = false;

            }
            return _category;
        }
        public TTBreakTimesettingDTO getdetails(int id)
        {
            TTBreakTimesettingDTO objTTMC = new TTBreakTimesettingDTO();
            try
            {
                List<AcademicYear> acad = new List<AcademicYear>();
                acad = _ttcontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active==true).ToList();
                objTTMC.academiclist = acad.OrderByDescending(t=>t.ASMAY_Order).ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList();
                objTTMC.catelist = mcat.ToArray();

                List<School_M_Class> allClass = new List<School_M_Class>();
                allClass = _ttcontext.School_M_Class.Where(c => c.ASMCL_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList();
                objTTMC.classDrpDwn = allClass.ToArray();

                List<TT_Master_DayDMO> alldays = new List<TT_Master_DayDMO>();
                alldays = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList();
                objTTMC.daysDrpDwn = alldays.ToArray();

                objTTMC.breaktimelist = (from Adm_School_M_Class in _ttcontext.School_M_Class
                                         from Adm_School_M_Academic_Year in _ttcontext.AcademicYear
                                         from TT_Master_Break in _ttcontext.TTBreakTimeSettingsDMO
                                         from TT_Master_Day in _ttcontext.TT_Master_DayDMO
                                         where (TT_Master_Break.ASMAY_Id.Equals(Adm_School_M_Academic_Year.ASMAY_Id) && Adm_School_M_Class.ASMCL_Id.Equals(TT_Master_Break.ASMCL_Id)
                                          && TT_Master_Break.MI_Id.Equals(id) && TT_Master_Break.TTMD_Id.Equals(TT_Master_Day.TTMD_Id))
                                         select new TTBreakTimesettingDTO
                                         {
                                             TTMB_Id = TT_Master_Break.TTMB_Id,
                                             ASMAYYear = Adm_School_M_Academic_Year.ASMAY_Year,
                                             DayName = TT_Master_Day.TTMD_DayName,
                                             ClassName = Adm_School_M_Class.ASMCL_ClassName,
                                             TTMB_BreakName = TT_Master_Break.TTMB_BreakName,
                                             TTMB_BreakStartTime = TT_Master_Break.TTMB_BreakStartTime,
                                             TTMB_BreakEndTime = TT_Master_Break.TTMB_BreakEndTime,
                                             TTMB_AfterPeriod = TT_Master_Break.TTMB_AfterPeriod,
                                             TTMB_ActiveFlag = TT_Master_Break.TTMB_ActiveFlag
                                         }
                                     ).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return objTTMC;

        }
        public TTBreakTimesettingDTO getpageedit(int id)
        {
            TTBreakTimesettingDTO page = new TTBreakTimesettingDTO();
            try
            {
                List<TTBreakTimeSettingsDMO> lorg = new List<TTBreakTimeSettingsDMO>();
                lorg = _ttcontext.TTBreakTimeSettingsDMO.AsNoTracking().Where(t => t.TTMB_Id.Equals(id)).ToList();
                page.breaktimelistedit = lorg.ToArray();

                TT_Category_Class_DTO ttmbbpdto = new TT_Category_Class_DTO();
                TT_Category_Class_DMO ttmbbpdmo = Mapper.Map<TT_Category_Class_DMO>(ttmbbpdto);



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TTBreakTimesettingDTO deleterec(int id)
        {
            TTBreakTimesettingDTO page = new TTBreakTimesettingDTO();
            try
            {
                List<TTBreakTimeSettingsDMO> lorgmain = new List<TTBreakTimeSettingsDMO>();
                lorgmain = _ttcontext.TTBreakTimeSettingsDMO.Where(t => t.TTMB_Id.Equals(id)).ToList();
                if (lorgmain.Any())
                {
                    _ttcontext.Remove(lorgmain.ElementAt(0));
                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists == 1)
                    {  //  *********************before periods delete***************start**************************
                        List<TT_Master_Break_BefPeriodsDMO> lorg = new List<TT_Master_Break_BefPeriodsDMO>();
                        lorg = _ttcontext.TT_Master_Break_BefPeriodsDMO.Where(t => t.TTMB_Id.Equals(id)).ToList();
                        if (lorg.Any())
                        {
                            for (int i = 0; i < lorg.Count; i++)
                            {
                                _ttcontext.Remove(lorg.ElementAt(i));
                                var contactExists2 = _ttcontext.SaveChanges();
                                if (contactExists2 == 1)
                                {
                                    page.returnval = true;
                                }
                                else
                                {
                                    page.returnval = false;
                                }
                            }
                        }
                        //  *********************before periods delete***************end**************************

                        //  *********************after periods delete***************start**************************
                        List<TT_Master_Break_AftPeriodsDMO> lorg2 = new List<TT_Master_Break_AftPeriodsDMO>();
                        lorg2 = _ttcontext.TT_Master_Break_AftPeriodsDMO.Where(t => t.TTMB_Id.Equals(id)).ToList();
                        if (lorg2.Any())
                        {
                            for (int i = 0; i < lorg2.Count; i++)
                            {
                                _ttcontext.Remove(lorg2.ElementAt(i));
                                var contactExists2 = _ttcontext.SaveChanges();
                                if (contactExists2 == 1)
                                {
                                    page.returnval = true;
                                }
                                else
                                {
                                    page.returnval = false;
                                }
                            }
                        }
                        //  *********************after periods delete***************end**************************
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                page.returnval = false;           
            }
            return page;
        }

        public TTBreakTimesettingDTO getclass_catg(TTBreakTimesettingDTO data)
        {
            try
            {
                data.classbycategory = (from a in _ttcontext.TTMasterCategoryDMO
                                        from b in _ttcontext.TT_Category_Class_DMO
                                        from c in _ttcontext.School_M_Class
                                        where (a.MI_Id == b.MI_Id && a.MI_Id.Equals(c.MI_Id) && a.MI_Id.Equals(data.MI_Id) && a.TTMC_Id.Equals(b.TTMC_Id) && b.ASMCL_Id.Equals(c.ASMCL_Id) && a.TTMC_Id.Equals(data.TTMC_Id)) //&& b.TTCC_ActiveFlag.Equals(true)
                                        select new TTBreakTimesettingDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                            TTMC_Id = a.TTMC_Id,
                                            TTMC_CategoryName = a.TTMC_CategoryName,
                                        }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }
        public TTBreakTimesettingDTO get_catg(TTBreakTimesettingDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new TTBreakTimesettingDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        public TTBreakTimesettingDTO deactivate(TTBreakTimesettingDTO acd)
        {
            try
            {
                TTBreakTimeSettingsDMO pge = Mapper.Map<TTBreakTimeSettingsDMO>(acd);
                if (pge.TTMB_Id > 0)
                {
                    var result = _ttcontext.TTBreakTimeSettingsDMO.Single(t => t.TTMB_Id.Equals(pge.TTMB_Id));
                    if (result.TTMB_ActiveFlag.Equals(true))
                    {
                        result.TTMB_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMB_ActiveFlag = true;
                    }
                    _ttcontext.Update(result);
                    var flag = _ttcontext.SaveChanges();
                    if (flag.Equals(1))
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
                acd.returnval = false;
            }
            return acd;
        }
    }
}
