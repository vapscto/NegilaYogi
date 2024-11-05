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
    public class TTMasterRoomImpl : Interfaces.TTMasterRoomInterface
    {
        private static ConcurrentDictionary<string, TTMasterRoomDTO> _login =
       new ConcurrentDictionary<string, TTMasterRoomDTO>();


        public TTContext _ttcontext;
        public TTMasterRoomImpl(TTContext ttcontext)
        {
            _ttcontext = ttcontext;
        }
        public TTMasterRoomDTO getdetails(int id)
        {
            TTMasterRoomDTO data = new TTMasterRoomDTO();
            try
            {
                data.facilitylistall = _ttcontext.TT_Master_FacilitiesDMO.Where(e => e.MI_Id == id && e.TTMFA_ActiveFlg == true).Distinct().OrderBy(r => r.TTMFA_FacilityName).ToArray();
                data.alldata = _ttcontext.TT_Master_RoomDMO.Where(e => e.MI_Id == id).Distinct().OrderByDescending(t => t.TTMRM_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTMasterRoomDTO savedetail(TTMasterRoomDTO data)
        {
            try
            {
                if (data.TTMRM_Id > 0)
                {
                    var result1 = _ttcontext.TT_Master_RoomDMO.Where(t => t.TTMRM_RoomName.Trim().ToLower()== data.TTMRM_RoomName.Trim().ToLower() && t.MI_Id==data.MI_Id
                    && t.TTMRM_Id != data.TTMRM_Id);
                    if (result1.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcontext.TT_Master_RoomDMO.Single(t => t.TTMRM_Id==data.TTMRM_Id);

                        result.TTMRM_RoomName = data.TTMRM_RoomName;
                        result.TTMRM_RoomDetails = data.TTMRM_RoomDetails;
                        result.TTMRM_UpdatedBy = data.User_Id;
                        result.TTMRM_UpdatedDate = DateTime.Now;
                         _ttcontext.Update(result);

                        var facilitylist = _ttcontext.TT_Master_Room_FacilitiesDMO.Where(d => d.TTMRM_Id == data.TTMRM_Id).ToList();
                        if (facilitylist.Count>0)
                        {
                            foreach (var item in facilitylist)
                            {
                                _ttcontext.Remove(item);
                            }
                        }
                        if (data.flist.Length>0)
                        {
                            foreach (var item in data.flist)
                            {
                                TT_Master_Room_FacilitiesDMO obj = new TT_Master_Room_FacilitiesDMO();
                                obj.MI_Id = data.MI_Id;
                                obj.TTMRM_Id = data.TTMRM_Id;
                                obj.TTMFA_Id = item.TTMFA_Id;
                                obj.TTMRMFA_CreatedBy = data.User_Id;
                                obj.TTMRMFA_UpdatedBy = data.User_Id;
                                obj.TTMRMFA_CreatedDate = DateTime.Now;
                                obj.TTMRMFA_UpdatedDate = DateTime.Now;
                                obj.TTMRMFA_ActiveFlg = true;

                                _ttcontext.Add(obj);

                            }
                        }

                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists > 0)
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
                    var result = _ttcontext.TT_Master_RoomDMO.Where(t => t.TTMRM_RoomName.Trim().ToLower() == data.TTMRM_RoomName.Trim().ToLower() && t.MI_Id == data.MI_Id);

                    if (result.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TT_Master_RoomDMO obj2 = new TT_Master_RoomDMO();
                        obj2.TTMRM_RoomName = data.TTMRM_RoomName;
                        obj2.TTMRM_RoomDetails = data.TTMRM_RoomDetails;
                        obj2.TTMRM_UpdatedBy = data.User_Id;
                        obj2.TTMRM_UpdatedDate = DateTime.Now;
                        obj2.TTMRM_CreatedBy = data.User_Id;
                        obj2.MI_Id = data.MI_Id;
                        obj2.TTMRM_CreatedDate = DateTime.Now;
                        obj2.TTMRM_ActiveFlg = true;
                        _ttcontext.Add(obj2);
                        if (data.flist.Length > 0)
                        {
                            foreach (var item in data.flist)
                            {
                                TT_Master_Room_FacilitiesDMO obj = new TT_Master_Room_FacilitiesDMO();
                                obj.MI_Id = data.MI_Id;
                                obj.TTMRM_Id = obj2.TTMRM_Id;
                                obj.TTMFA_Id = item.TTMFA_Id;
                                obj.TTMRMFA_CreatedBy = data.User_Id;
                                obj.TTMRMFA_UpdatedBy = data.User_Id;
                                obj.TTMRMFA_CreatedDate = DateTime.Now;
                                obj.TTMRMFA_UpdatedDate = DateTime.Now;
                                obj.TTMRMFA_ActiveFlg = true;

                                _ttcontext.Add(obj);

                            }
                        }

                        var contactExists = _ttcontext.SaveChanges();
                            if (contactExists > 0)
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

     
        public TTMasterRoomDTO getpageedit(int id)
        {
            TTMasterRoomDTO data= new TTMasterRoomDTO();
            try
            {
                data.editlist = _ttcontext.TT_Master_RoomDMO.Where(e => e.TTMRM_Id == id).Distinct().ToArray();

                data.editfaclist= _ttcontext.TT_Master_Room_FacilitiesDMO.Where(e => e.TTMRM_Id == id && e.TTMRMFA_ActiveFlg==true).Distinct().ToArray();
            }
            catch (Exception ee)
            {
              Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TTMasterRoomDTO Viewfacility(int id)
        {
            TTMasterRoomDTO data= new TTMasterRoomDTO();
            try
            {
                data.viewdata = (from a in _ttcontext.TT_Master_FacilitiesDMO
                                 from b in _ttcontext.TT_Master_Room_FacilitiesDMO
                                 where a.TTMFA_Id == b.TTMFA_Id && b.TTMRM_Id == id
                                 select new TTMasterRoomDTO {
                                     TTMFA_Id = a.TTMFA_Id,
                                     TTMRM_Id = b.TTMRM_Id,
                                     TTMRMFA_Id = b.TTMRMFA_Id,
                                     TTMRMFA_ActiveFlg = b.TTMRMFA_ActiveFlg,
                                     TTMFA_FacilityName = a.TTMFA_FacilityName,
                                     TTMFA_FacilityDesc = a.TTMFA_FacilityDesc,
                                }).Distinct().OrderBy(e=>e.TTMFA_FacilityName).ToArray();
      
            }
            catch (Exception ee)
            {
              Console.WriteLine(ee.Message);
            }
            return data;
        }
     

        public TTMasterRoomDTO deactivate(TTMasterRoomDTO data)
        {
                try
                {

                    //var checkmapping = _ttcontext.TT_Master_Room_FacilitiesDMO.Where(e => e.TTMRM_Id == data.TTMRM_Id && e.TTMRMFA_ActiveFlg==true).ToList();
                    //if (checkmapping.Count == 0)
                    //{
                        var delelist = _ttcontext.TT_Master_RoomDMO.Single(e => e.TTMRM_Id == data.TTMRM_Id);

                        if (delelist.TTMRM_ActiveFlg == true)
                        {
                            delelist.TTMRM_ActiveFlg = false;


                        var facilitylist = _ttcontext.TT_Master_Room_FacilitiesDMO.Where(d => d.TTMRM_Id == data.TTMRM_Id).ToList();
                        if (facilitylist.Count > 0)
                        {
                            foreach (var item in facilitylist)
                            {
                                item.TTMRMFA_ActiveFlg = false;
                                item.TTMRMFA_UpdatedBy = data.User_Id;
                                item.TTMRMFA_UpdatedDate = DateTime.Now;
                                _ttcontext.Update(item);
                            }
                        }

                    }
                        else
                        {
                            delelist.TTMRM_ActiveFlg = true;

                        var facilitylist = _ttcontext.TT_Master_Room_FacilitiesDMO.Where(d => d.TTMRM_Id == data.TTMRM_Id).ToList();
                        if (facilitylist.Count > 0)
                        {
                            foreach (var item in facilitylist)
                            {
                                item.TTMRMFA_ActiveFlg = true;
                                item.TTMRMFA_UpdatedBy = data.User_Id;
                                item.TTMRMFA_UpdatedDate = DateTime.Now;
                                _ttcontext.Update(item);
                            }
                        }

                    }
                        delelist.TTMRM_UpdatedBy = data.User_Id;
                        delelist.TTMRM_UpdatedDate = DateTime.Now;

                    


                    var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    //}
                    //else
                    //{
                    //    data.returnduplicatestatus = "MAP";
                    //}



                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                return data;
           
            
        }

    }
}
