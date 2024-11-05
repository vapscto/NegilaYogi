using AutoMapper;
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
    public class TTMasterFacilitiesImpl : Interfaces.TTMasterFacilitiesInterface
    {
        private static ConcurrentDictionary<string, TTMasterFacilitiesDTO> _login =
       new ConcurrentDictionary<string, TTMasterFacilitiesDTO>();


        public TTContext _ttcontext;
        public TTMasterFacilitiesImpl(TTContext ttcontext)
        {
            _ttcontext = ttcontext;
        }

        public TTMasterFacilitiesDTO savedetail(TTMasterFacilitiesDTO data)
        {
            try
            {
                if (data.TTMFA_Id > 0)
                {
                    var result1 = _ttcontext.TT_Master_FacilitiesDMO.Where(t => t.TTMFA_FacilityName.Trim().ToLower()== data.TTMFA_FacilityName.Trim().ToLower() && t.MI_Id==data.MI_Id
                    && t.TTMFA_Id != data.TTMFA_Id);
                    if (result1.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcontext.TT_Master_FacilitiesDMO.Single(t => t.TTMFA_Id==data.TTMFA_Id);

                        result.TTMFA_FacilityName = data.TTMFA_FacilityName;
                        result.TTMFA_FacilityDesc = data.TTMFA_FacilityDesc;
                        result.TTMFA_UpdatedBy = data.User_Id;
                        result.TTMFA_UpdatedDate = DateTime.Now;
                         _ttcontext.Update(result);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _ttcontext.TT_Master_FacilitiesDMO.Where(t => t.TTMFA_FacilityName.Trim().ToLower() == data.TTMFA_FacilityName.Trim().ToLower() && t.MI_Id == data.MI_Id );
                    
                    if (result.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TT_Master_FacilitiesDMO obj = new TT_Master_FacilitiesDMO();
                        obj.TTMFA_FacilityName = data.TTMFA_FacilityName;
                        obj.TTMFA_FacilityDesc = data.TTMFA_FacilityDesc;
                        obj.TTMFA_UpdatedBy = data.User_Id;
                        obj.TTMFA_CreatedBy = data.User_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.TTMFA_UpdatedDate = DateTime.Now;
                        obj.TTMFA_CreatedDate = DateTime.Now;
                        obj.TTMFA_ActiveFlg = true;
                        _ttcontext.Add(obj);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
          
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TTMasterFacilitiesDTO getdetails(int id)
        {
            TTMasterFacilitiesDTO data = new TTMasterFacilitiesDTO();
            try
            {
                data.alldata = _ttcontext.TT_Master_FacilitiesDMO.Where(e => e.MI_Id == id).Distinct().OrderByDescending(r=>r.TTMFA_Id).ToArray();
                

            }
            catch (Exception ee)
            {
               Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTMasterFacilitiesDTO getpageedit(int id)
        {
            TTMasterFacilitiesDTO data= new TTMasterFacilitiesDTO();
            try
            {
                data.editlist = _ttcontext.TT_Master_FacilitiesDMO.Where(e => e.TTMFA_Id == id).Distinct().ToArray();
            }
            catch (Exception ee)
            {
              Console.WriteLine(ee.Message);
            }
            return data;
        }
     

        public TTMasterFacilitiesDTO deactivate(TTMasterFacilitiesDTO data)
        {
                try
                {

                    var checkmapping = _ttcontext.TT_Master_Room_FacilitiesDMO.Where(e => e.TTMFA_Id == data.TTMFA_Id && e.TTMRMFA_ActiveFlg==true).ToList();
                    if (checkmapping.Count == 0)
                    {
                        var delelist = _ttcontext.TT_Master_FacilitiesDMO.Single(e => e.TTMFA_Id == data.TTMFA_Id);

                        if (delelist.TTMFA_ActiveFlg == true)
                        {
                            delelist.TTMFA_ActiveFlg = false;
                        }
                        else
                        {
                            delelist.TTMFA_ActiveFlg = true;
                        }
                        delelist.TTMFA_UpdatedBy = data.User_Id;
                        delelist.TTMFA_UpdatedDate = DateTime.Now;

                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.returnduplicatestatus = "MAP";
                    }



                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                return data;
           
            
        }

    }
}
