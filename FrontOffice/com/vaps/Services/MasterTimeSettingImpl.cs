using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class MasterTimeSettingImpl:Interfaces.MasterTimeSettingInterface
    {
        //private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
        // new ConcurrentDictionary<string, TTMasterCategoryDTO>();

        public FOContext _FOContext;


        public MasterTimeSettingImpl(FOContext ttcntx)
        {
            _FOContext = ttcntx;
        }
        public MasterTimeSettingDTO savedetail(MasterTimeSettingDTO _category)
        {
            // MasterTimeSettingDTO objpge = Mapper.Map<MasterTimeSettingDTO>(_category);
            try
            {
                if (_category.FOMTS_Id > 0)
                {
                    var res = _FOContext.MasterTimeSetting.Where(t => t.FOMTS_FDWHrMin == _category.FOMTS_FDWHrMin && t.MI_Id == _category.MI_Id && t.FOMTS_HDWHrMin == _category.FOMTS_HDWHrMin && t.FOMTS_IHalfLoginTime == _category.FOMTS_IHalfLoginTime && t.FOMTS_IhalfLogoutTime == _category.FOMTS_IhalfLogoutTime && t.FOMTS_IIHalfLoginTime == _category.FOMTS_IIHalfLoginTime && t.FOMTS_IIHalfLogoutTime == _category.FOMTS_IIHalfLogoutTime).ToList();
                    if (res.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        //var result = _FOContext.MasterTimeSetting.Single(t => t.TTMDPT_Id == objpge.TTMDPT_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id);
                        //result.TTMC_Id = objpge.TTMC_Id;
                        //result.TTMD_Id = objpge.TTMD_Id;
                        //result.TTMP_Id = objpge.TTMP_Id;
                        //result.TTMDPT_StartTime = objpge.TTMDPT_StartTime;
                        //result.TTMDPT_EndTime = objpge.TTMDPT_EndTime;
                        //result.ASMAY_Id = objpge.ASMAY_Id;
                        //// result.UpdatedDate = DateTime.Now;
                        //_ttcontext.Update(result);
                        //var contactExists = _ttcontext.SaveChanges();
                        //if (contactExists == 1)
                        //{
                        //    _category.returnval = true;
                        //}
                        //else
                        //{
                        //    _category.returnval = false;
                        //}
                    }
                }
                else
                {
                    var res = _FOContext.MasterTimeSetting.Where(t => t.FOMTS_FDWHrMin == _category.FOMTS_FDWHrMin && t.MI_Id == _category.MI_Id && t.FOMTS_HDWHrMin == _category.FOMTS_HDWHrMin && t.FOMTS_IHalfLoginTime == _category.FOMTS_IHalfLoginTime && t.FOMTS_IhalfLogoutTime == _category.FOMTS_IhalfLogoutTime && t.FOMTS_IIHalfLoginTime == _category.FOMTS_IIHalfLoginTime && t.FOMTS_IIHalfLogoutTime == _category.FOMTS_IIHalfLogoutTime).ToList();
                    if (res.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        MasterTimeSettingDMO objpge = Mapper.Map<MasterTimeSettingDMO>(_category);
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.FOMHWD_ActiveFlg = true;
                        _FOContext.Add(objpge);
                        var contactExists = _FOContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public MasterTimeSettingDTO getdetails(int id)
        {
            MasterTimeSettingDTO TTMC = new MasterTimeSettingDTO();
            try
            {
                List<MasterTimeSettingDMO> gettttttt = new List<MasterTimeSettingDMO>();
                gettttttt = _FOContext.MasterTimeSetting.Where(t => t.MI_Id == id && t.FOMHWD_ActiveFlg == true).ToList();
                TTMC.getlist = gettttttt.ToArray();
                if (TTMC.getlist.Length > 0)
                {
                    TTMC.count = TTMC.getlist.Length;
                }
                else
                {
                    TTMC.count = 0;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public MasterTimeSettingDTO getpageedit(int id)
        {
            MasterTimeSettingDTO page = new MasterTimeSettingDTO();
            //try
            //{
            //    List<TT_Master_Day_Period_TimeDMO> lorg = new List<TT_Master_Day_Period_TimeDMO>();
            //    lorg = _ttcontext.TT_Master_Day_Period_TimeDMO.AsNoTracking().Where(t => t.TTMDPT_Id.Equals(id)).ToList();
            //    page.periodlistedit = lorg.ToArray();
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return page;
        }
        public MasterTimeSettingDTO deleterec(int id)
        {
            MasterTimeSettingDTO page = new MasterTimeSettingDTO();
            //try
            //{
            //    List<TT_Master_Day_Period_TimeDMO> lorg = new List<TT_Master_Day_Period_TimeDMO>();
            //    lorg = _ttcontext.TT_Master_Day_Period_TimeDMO.Where(t => t.TTMDPT_Id.Equals(id)).ToList();
            //    if (lorg.Any())
            //    {
            //        _ttcontext.Remove(lorg.ElementAt(0));
            //        var contactExists = _ttcontext.SaveChanges();
            //        if (contactExists == 1)
            //        {
            //            page.returnval = true;
            //        }
            //        else
            //        {
            //            page.returnval = false;
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return page;
        }
    }
}
