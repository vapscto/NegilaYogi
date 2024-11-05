using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Model.com.vapstech.Hostel;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using Microsoft.EntityFrameworkCore;

namespace HostelServiceHub.Services
{
    public class HS_Master_IMPL : Interface.HS_Master_Interface
    {


        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;

        HR_Master_Floor_DMO MF = new HR_Master_Floor_DMO();
        HR_Master_Room_DMO MR = new HR_Master_Room_DMO();
        public HS_Master_IMPL(HostelContext Para, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = Para;
            _dbcontext = para2;
        }

        #region ================== MASTER FLOOR ===============

        public HR_Master_Floor_DTO get_floordata(HR_Master_Floor_DTO data)
        {

            try
            {

                data.houstel_list = (from a in _HostelContext.HL_Master_Hostel_DMO
                                     where (a.MI_Id == data.MI_Id && a.HLMH_ActiveFlag == true)
                                     select new HR_Master_Floor_DTO
                                     {
                                         HLMH_Id = a.HLMH_Id,
                                         HLMH_Name = a.HLMH_Name,
                                     }).Distinct().OrderByDescending(t => t.HLMH_Id).ToArray();

                data.facilities_list = (from a in _HostelContext.HL_Master_Facility_DMO
                                        where (a.MI_Id == data.MI_Id && a.HLMFTY_ActiveFlag == true)
                                        select new HR_Master_Floor_DTO
                                        {
                                            HLMFTY_Id = a.HLMFTY_Id,
                                            HLMFTY_FacilityName = a.HLMFTY_FacilityName,
                                        }).Distinct().OrderBy(t => t.HLMFTY_Id).ToArray();

                data.grid_Alldataforfloor = (from f in _HostelContext.HR_Master_Floor_DMO
                                             from b in _HostelContext.HL_Master_Hostel_DMO
                                             where (f.HLMH_Id == b.HLMH_Id && f.MI_Id == b.MI_Id && f.MI_Id == data.MI_Id)
                                             select new HR_Master_Floor_DTO
                                             {
                                                 HLMF_Id = f.HLMF_Id,
                                                 HRMF_FloorName = f.HRMF_FloorName,
                                                 HRMF_ActiveFlag = f.HRMF_ActiveFlag,
                                                 HLMH_Id = f.HLMH_Id,
                                                 HLMH_Name = b.HLMH_Name,
                                                 HRMF_TotalRooms = f.HRMF_TotalRooms,
                                                 HRMF_FloorDesc = f.HRMF_FloorDesc,

                                             }).OrderByDescending(a => a.HLMF_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HR_Master_Floor_DTO save_Floordata(HR_Master_Floor_DTO data)
        {


            try
            {
                if (data.HLMF_Id > 0)
                {
                    var duplicate = _HostelContext.HR_Master_Floor_DMO.Where(t => t.HRMF_FloorName == data.HRMF_FloorName && t.HLMF_Id != data.HLMF_Id && t.MI_Id == data.MI_Id && t.HLMH_Id == data.HLMH_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var result = _HostelContext.HR_Master_Floor_DMO.Single(a => a.HLMF_Id == data.HLMF_Id && a.MI_Id == data.MI_Id);
                        result.HRMF_FloorName = data.HRMF_FloorName;
                        result.HLMH_Id = data.HLMH_Id;
                        result.HRMF_TotalRooms = data.HRMF_TotalRooms;
                        result.HRMF_FloorDesc = data.HRMF_FloorDesc;

                        result.UpdatedDate = DateTime.Now;
                        result.HRMF_UpdatedBy = data.UserId;
                        _HostelContext.Update(result);

                        var remove = _HostelContext.HL_Master_Floor_Facilities_DMO.Where(t => t.HLMF_Id == data.HLMF_Id).ToList();
                        if (remove.Count > 0)
                        {
                            foreach (var item in remove)
                            {
                                _HostelContext.Remove(item);
                            }
                            if (data.selected_facilities.Length > 0)
                            {

                                for (int s = 0; s < data.selected_facilities.Length; s++)
                                {
                                    HL_Master_Floor_Facilities_DMO obj2 = new HL_Master_Floor_Facilities_DMO();

                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HLMF_Id = result.HLMF_Id;
                                    obj2.HLMFTY_Id = data.selected_facilities[s].HLMFTY_Id;
                                    obj2.HLMFF_ActiveFlg = true;
                                    _HostelContext.Add(obj2);
                                }
                            }
                        }
                        else
                        {
                            if (data.selected_facilities.Length > 0)
                            {

                                for (int s = 0; s < data.selected_facilities.Length; s++)
                                {
                                    HL_Master_Floor_Facilities_DMO obj2 = new HL_Master_Floor_Facilities_DMO();

                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HLMF_Id = result.HLMF_Id;
                                    obj2.HLMFTY_Id = data.selected_facilities[s].HLMFTY_Id;
                                    obj2.HLMFF_ActiveFlg = true;
                                    _HostelContext.Add(obj2);
                                }
                            }
                        }

                        int returnvalue = _HostelContext.SaveChanges();

                        if (returnvalue > 0)
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
                    var duplicate = _HostelContext.HR_Master_Floor_DMO.Where(t => t.HRMF_FloorName == data.HRMF_FloorName && t.MI_Id == data.MI_Id && t.HLMH_Id == data.HLMH_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HR_Master_Floor_DMO mfcon = new HR_Master_Floor_DMO();
                        mfcon.HRMF_FloorName = data.HRMF_FloorName;
                        mfcon.HLMH_Id = data.HLMH_Id;
                        mfcon.MI_Id = data.MI_Id;
                        mfcon.HRMF_TotalRooms = data.HRMF_TotalRooms;
                        mfcon.HRMF_FloorDesc = data.HRMF_FloorDesc;
                        mfcon.HRMF_ActiveFlag = true;
                        mfcon.CreatedDate = DateTime.Now;
                        mfcon.UpdatedDate = DateTime.Now;
                        mfcon.HRMF_CreatedBy = data.UserId;
                        mfcon.HRMF_UpdatedBy = data.UserId;
                        _HostelContext.Add(mfcon);
                        if (data.selected_facilities.Length > 0)
                        {

                            for (int s = 0; s < data.selected_facilities.Length; s++)
                            {
                                HL_Master_Floor_Facilities_DMO obj2 = new HL_Master_Floor_Facilities_DMO();

                                obj2.MI_Id = data.MI_Id;
                                obj2.HLMF_Id = mfcon.HLMF_Id;
                                obj2.HLMFTY_Id = data.selected_facilities[s].HLMFTY_Id;
                                obj2.HLMFF_ActiveFlg = true;
                                _HostelContext.Add(obj2);
                            }
                        }

                        int returnvalue = _HostelContext.SaveChanges();
                        if (returnvalue > 0)
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
        public HR_Master_Floor_DTO edit_floordata(HR_Master_Floor_DTO data)
        {

            try
            {
                data.edit_floorlist = _HostelContext.HR_Master_Floor_DMO.Where(a => a.HLMF_Id == data.HLMF_Id).Distinct().ToArray();
                data.facilty_list = (from a in _HostelContext.HL_Master_Floor_Facilities_DMO
                                     from b in _HostelContext.HL_Master_Facility_DMO
                                     where (a.HLMF_Id == data.HLMF_Id && a.MI_Id == data.MI_Id && a.HLMFTY_Id == b.HLMFTY_Id)
                                     select new HR_Master_Floor_DTO
                                     {
                                         HLMFTY_Id = a.HLMFTY_Id,
                                         HLMFTY_FacilityName = b.HLMFTY_FacilityName,
                                     }).Distinct().OrderByDescending(t => t.HLMH_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HR_Master_Floor_DTO deactivate_Floordata(HR_Master_Floor_DTO data)
        {
            try
            {
                var result = _HostelContext.HR_Master_Floor_DMO.Single(t => t.HLMF_Id == data.HLMF_Id && t.MI_Id == data.MI_Id);
                if (data.HRMF_ActiveFlag == true)
                {
                    result.HRMF_ActiveFlag = false;
                }
                else
                {
                    result.HRMF_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }


            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;


        }
        public HR_Master_Floor_DTO get_Mappedfacility(HR_Master_Floor_DTO data)
        {
            try
            {
                data.mappedfacilitylist = (from a in _HostelContext.HR_Master_Floor_DMO
                                           from b in _HostelContext.HL_Master_Floor_Facilities_DMO
                                           from c in _HostelContext.HL_Master_Hostel_DMO
                                           from d in _HostelContext.HL_Master_Facility_DMO
                                           where (a.HLMF_Id == data.HLMF_Id && b.HLMFTY_Id == d.HLMFTY_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.HLMF_Id == b.HLMF_Id && a.HLMH_Id == c.HLMH_Id && d.HLMFTY_ActiveFlag == true)
                                           select new HR_Master_Floor_DTO
                                           {
                                               HRMF_FloorName = a.HRMF_FloorName,
                                               HLMF_Id = a.HLMF_Id,
                                               HRMF_TotalRooms = a.HRMF_TotalRooms,
                                               HRMF_FloorDesc = a.HRMF_FloorDesc,
                                               HLMH_Name = c.HLMH_Name,
                                               HLMFTY_FacilityName = d.HLMFTY_FacilityName
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        #endregion

        #region ================== MASTER ROOM ================

        public HR_Master_Room_DTO get_Roomloaddata(HR_Master_Room_DTO data)
        {

            try
            {
              

                data.hostellist = (from t in _HostelContext.HL_Master_Hostel_DMO
                                   where (t.MI_Id == data.MI_Id && t.HLMH_ActiveFlag == true)
                                   select new HR_Master_Room_DTO
                                   {
                                       HLMH_Id = t.HLMH_Id,
                                       HLMH_Name = t.HLMH_Name,
                                   }).Distinct().OrderBy(t => t.HLMH_Name).ToArray();

                //data.floor_list = (from a in _HostelContext.HR_Master_Floor_DMO
                //                   where (a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                //                   select new HR_Master_Room_DTO
                //                   {
                //                       HLMF_Id = a.HLMF_Id,
                //                       HRMF_FloorName = a.HRMF_FloorName,
                //                   }).Distinct().OrderBy(a => a.HRMF_FloorName).ToArray();

                

                data.get_room_list = (from f in _HostelContext.HR_Master_Floor_DMO
                                      from b in _HostelContext.HL_Master_Hostel_DMO
                                      from c in _HostelContext.HR_Master_Room_DMO
                                      from d in _HostelContext.HL_Master_Room_Category_DMO
                                      where (f.MI_Id == data.MI_Id && f.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && c.HLMH_Id == b.HLMH_Id && c.HLMF_Id == f.HLMF_Id && c.HLMRCA_Id == d.HLMRCA_Id && f.HRMF_ActiveFlag == true && b.HLMH_ActiveFlag == true && d.HLMRCA_ActiveFlag == true)
                                      select new HR_Master_Room_DTO
                                      {
                                          HLMH_Id = b.HLMH_Id,
                                          HLMH_Name = b.HLMH_Name,
                                          HLMF_Id = f.HLMF_Id,
                                          HRMF_FloorName = f.HRMF_FloorName,
                                          HRMRM_RoomNo = c.HRMRM_RoomNo,
                                          HRMRM_ActiveFlag = c.HRMRM_ActiveFlag,
                                          HRMRM_RoomDesc = c.HRMRM_RoomDesc,
                                          HRMRM_RoomForGuestFlg = c.HRMRM_RoomForGuestFlg,
                                          HRMRM_RoomForStaffFlg = c.HRMRM_RoomForStaffFlg,
                                          HRMRM_RoomForStudentFlg = c.HRMRM_RoomForStudentFlg,
                                          HRMRM_Id = c.HRMRM_Id,
                                          HRMRM_SharingFlg = c.HRMRM_SharingFlg,
                                          HRMRM_ACFlg = c.HRMRM_ACFlg,
                                          HRMRM_BedCapacity = c.HRMRM_BedCapacity,
                                          HLMRCA_RoomCategory = d.HLMRCA_RoomCategory,

                                      }).OrderByDescending(a => a.HRMRM_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO save_Roomdata(HR_Master_Room_DTO data)
        {
            try
            {
                if (data.HRMRM_Id == 0)
                {
                    var duplicate = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_RoomNo == data.HRMRM_RoomNo && t.HLMF_Id == data.HLMF_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HR_Master_Room_DMO obj1 = new HR_Master_Room_DMO();

                        obj1.HRMRM_RoomNo = data.HRMRM_RoomNo;
                        obj1.HLMH_Id = data.HLMH_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMF_Id = data.HLMF_Id;
                        obj1.HRMRM_RoomNo = data.HRMRM_RoomNo;
                        obj1.HRMRM_SharingFlg = data.HRMRM_SharingFlg;
                        obj1.HRMRM_ACFlg = data.HRMRM_ACFlg;
                        obj1.HRMRM_BedCapacity = data.HRMRM_BedCapacity;
                        obj1.HRMRM_RoomDesc = data.HRMRM_RoomDesc;
                        obj1.HRMRM_RoomForStudentFlg = data.HRMRM_RoomForStudentFlg;
                        obj1.HRMRM_RoomForStaffFlg = data.HRMRM_RoomForStaffFlg;
                        obj1.HRMRM_RoomForGuestFlg = data.HRMRM_RoomForGuestFlg;
                        obj1.HLMRCA_Id = data.HLMRCA_Id;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.HRMRM_CreatedBy = data.UserId;
                        obj1.HRMRM_UpdatedBy = data.UserId;
                        obj1.HRMRM_ActiveFlag = true;

                        _HostelContext.Add(obj1);
                        if (data.selected_facilities.Length > 0)
                        {
                            for (int s = 0; s < data.selected_facilities.Length; s++)
                            {
                                HL_Master_Room_Facilities_DMO obj2 = new HL_Master_Room_Facilities_DMO();
                                obj2.MI_Id = data.MI_Id;
                                obj2.HRMRM_Id = obj1.HRMRM_Id;
                                obj2.HLMFTY_Id = data.selected_facilities[s].HLMFTY_Id;
                                obj2.HRMRMF_ActiveFlg = true;
                                _HostelContext.Add(obj2);
                            }
                        }

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.HRMRM_Id > 0)
                {
                    var duplicate = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id != data.HRMRM_Id && t.HRMRM_RoomNo == data.HRMRM_RoomNo && t.HLMF_Id == data.HLMF_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var result = _HostelContext.HR_Master_Room_DMO.Single(a => a.HRMRM_Id == data.HRMRM_Id);

                        result.HRMRM_RoomNo = data.HRMRM_RoomNo;
                        result.HLMH_Id = data.HLMH_Id;
                        result.HLMF_Id = data.HLMF_Id;
                        result.HRMRM_RoomNo = data.HRMRM_RoomNo;
                        result.HRMRM_SharingFlg = data.HRMRM_SharingFlg;
                        result.HRMRM_ACFlg = data.HRMRM_ACFlg;
                        result.HRMRM_BedCapacity = data.HRMRM_BedCapacity;
                        result.HRMRM_RoomDesc = data.HRMRM_RoomDesc;
                        result.HRMRM_RoomForStudentFlg = data.HRMRM_RoomForStudentFlg;
                        result.HRMRM_RoomForStaffFlg = data.HRMRM_RoomForStaffFlg;
                        result.HRMRM_RoomForGuestFlg = data.HRMRM_RoomForGuestFlg;
                        result.HLMRCA_Id = data.HLMRCA_Id;
                        result.UpdatedDate = DateTime.Now;
                        result.HRMRM_UpdatedBy = data.UserId;

                        _HostelContext.Update(result);

                        var remove = _HostelContext.HL_Master_Room_Facilities_DMO.Where(t => t.HRMRM_Id == data.HRMRM_Id).ToList();
                        if (remove.Count > 0)
                        {
                            foreach (var item in remove)
                            {
                                _HostelContext.Remove(item);
                            }
                            if (data.selected_facilities.Length > 0)
                            {
                                for (int s = 0; s < data.selected_facilities.Length; s++)
                                {
                                    HL_Master_Room_Facilities_DMO obj2 = new HL_Master_Room_Facilities_DMO();

                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HRMRM_Id = result.HRMRM_Id;
                                    obj2.HLMFTY_Id = data.selected_facilities[s].HLMFTY_Id;
                                    obj2.HRMRMF_ActiveFlg = true;
                                    _HostelContext.Add(obj2);
                                }
                            }
                        }
                        else
                        {
                            if (data.selected_facilities.Length > 0)
                            {
                                for (int s = 0; s < data.selected_facilities.Length; s++)
                                {
                                    HL_Master_Room_Facilities_DMO obj2 = new HL_Master_Room_Facilities_DMO();

                                    obj2.MI_Id = data.MI_Id;
                                    obj2.HRMRM_Id = result.HRMRM_Id;
                                    obj2.HLMFTY_Id = data.selected_facilities[s].HLMFTY_Id;
                                    obj2.HRMRMF_ActiveFlg = true;
                                    _HostelContext.Add(obj2);
                                }
                            }
                        }
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                try
                {
                    var MapHostelFee = _HostelContext.Database.ExecuteSqlCommand("HL_Master_Room_FeeGroup_AutoInsert @p0", data.MI_Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public HR_Master_Room_DTO edit_Roomdata(HR_Master_Room_DTO data)
        {

            try
            {
                data.edit_Roomdatalist = (from f in _HostelContext.HR_Master_Floor_DMO
                                          from b in _HostelContext.HL_Master_Hostel_DMO
                                          from c in _HostelContext.HR_Master_Room_DMO
                                          where (c.HRMRM_Id == data.HRMRM_Id && b.HLMH_Id == c.HLMH_Id && c.HLMF_Id == f.HLMF_Id && f.MI_Id == b.MI_Id && f.MI_Id == data.MI_Id)
                                          select new HR_Master_Room_DTO
                                          {
                                              HRMRM_Id = c.HRMRM_Id,
                                              HRMRM_RoomNo = c.HRMRM_RoomNo,
                                              HRMF_FloorName = f.HRMF_FloorName,
                                              HLMF_Id = f.HLMF_Id,
                                              HRMRM_ActiveFlag = c.HRMRM_ActiveFlag,
                                              HLMH_Id = b.HLMH_Id,
                                              HRMRM_SharingFlg = c.HRMRM_SharingFlg,
                                              HRMRM_ACFlg = c.HRMRM_ACFlg,
                                              HRMRM_BedCapacity = c.HRMRM_BedCapacity,
                                              HRMRM_RoomDesc = c.HRMRM_RoomDesc,
                                              HRMRM_RoomForGuestFlg = c.HRMRM_RoomForGuestFlg,
                                              HRMRM_RoomForStaffFlg = c.HRMRM_RoomForStaffFlg,
                                              HRMRM_RoomForStudentFlg = c.HRMRM_RoomForStudentFlg,
                                              HLMRCA_Id = c.HLMRCA_Id,

                                          }).Distinct().ToArray();

                data.editfaclist = (from a in _HostelContext.HL_Master_Room_Facilities_DMO
                                    from b in _HostelContext.HL_Master_Facility_DMO
                                    where (a.HRMRM_Id == data.HRMRM_Id && a.MI_Id == data.MI_Id && a.HLMFTY_Id == b.HLMFTY_Id)
                                    select new HR_Master_Room_DTO
                                    {
                                        HLMFTY_Id = b.HLMFTY_Id,
                                        HLMFTY_FacilityName = b.HLMFTY_FacilityName,
                                    }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO deactive_Roomdata(HR_Master_Room_DTO data)
        {
            try
            {
                var result = _HostelContext.HR_Master_Room_DMO.Single(t => t.HRMRM_Id == data.HRMRM_Id);
                if (data.HRMRM_ActiveFlag == true)
                {
                    result.HRMRM_ActiveFlag = false;
                }
                else if (data.HRMRM_ActiveFlag == false)
                {
                    result.HRMRM_ActiveFlag = true;
                }

                result.UpdatedDate = DateTime.Now;
                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;


        }
        public HR_Master_Room_DTO get_MappedfacilityforRoom(HR_Master_Room_DTO data)
        {
            try
            {
                data.get_MappedfacilityforRoom = (from a in _HostelContext.HR_Master_Room_DMO
                                                  from b in _HostelContext.HL_Master_Room_Facilities_DMO
                                                  from c in _HostelContext.HL_Master_Hostel_DMO
                                                  from d in _HostelContext.HL_Master_Facility_DMO
                                                  from f in _HostelContext.HR_Master_Floor_DMO
                                                  where (a.HRMRM_Id == data.HRMRM_Id && a.HRMRM_Id == b.HRMRM_Id && b.HLMFTY_Id == d.HLMFTY_Id
                                                  && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.HLMH_Id == c.HLMH_Id
                                                  && a.HLMF_Id == f.HLMF_Id && d.HLMFTY_ActiveFlag == true)
                                                  select new HR_Master_Room_DTO
                                                  {
                                                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                                                      HLMF_Id = a.HLMF_Id,
                                                      HRMRM_SharingFlg = a.HRMRM_SharingFlg,
                                                      HRMRM_ACFlg = a.HRMRM_ACFlg,
                                                      HRMRM_BedCapacity = a.HRMRM_BedCapacity,
                                                      HRMRM_RoomDesc = a.HRMRM_RoomDesc,
                                                      HRMRM_RoomForStudentFlg = a.HRMRM_RoomForStudentFlg,
                                                      HRMRM_RoomForStaffFlg = a.HRMRM_RoomForStaffFlg,
                                                      HRMRM_RoomForGuestFlg = a.HRMRM_RoomForGuestFlg,
                                                      HLMH_Name = c.HLMH_Name,
                                                      HLMFTY_FacilityName = d.HLMFTY_FacilityName,
                                                      HRMF_FloorName = f.HRMF_FloorName,
                                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Master_Room_DTO floor(HR_Master_Room_DTO data)
        {
            try
            {
                data.floor_list = (from a in _HostelContext.HR_Master_Floor_DMO                                   
                                   where (a.HLMH_Id==data.HLMH_Id && a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                                   select new HR_Master_Room_DTO
                                   {
                                       HLMF_Id = a.HLMF_Id,
                                       HRMF_FloorName = a.HRMF_FloorName,
                                   }).Distinct().OrderBy(a => a.HRMF_FloorName).ToArray();

                data.room_catlist = (from a in _HostelContext.HL_Master_Room_Category_DMO
                                     where (a.HLMH_Id == data.HLMH_Id && a.MI_Id == data.MI_Id && a.HLMRCA_ActiveFlag == true)
                                     select new HR_Master_Room_DTO
                                     {
                                         HLMRCA_Id = a.HLMRCA_Id,
                                         HLMRCA_RoomCategory = a.HLMRCA_RoomCategory,

                                     }).Distinct().OrderByDescending(a => a.HLMRCA_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public HR_Master_Room_DTO floordetails(HR_Master_Room_DTO data)
        {
            try
            {
                data.facilities_list = (from a in _HostelContext.HL_Master_Floor_Facilities_DMO
                                        from b in _HostelContext.HL_Master_Facility_DMO
                                        where (a.HLMF_Id == data.HLMF_Id && a.HLMFTY_Id == b.HLMFTY_Id && a.MI_Id == data.MI_Id && a.HLMFF_ActiveFlg == true && b.HLMFTY_ActiveFlag == true)
                                        select new HR_Master_Room_DTO
                                        {
                                            HLMF_Id = a.HLMF_Id,
                                            HLMFTY_Id = a.HLMFTY_Id,
                                            HLMFTY_FacilityName = b.HLMFTY_FacilityName,
                                        }).Distinct().OrderBy(a => a.HLMFTY_FacilityName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public HR_Master_Room_DTO categorydetails(HR_Master_Room_DTO data)
        {
            try
            {
                

              
                data.categorydetails = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.HLMRCA_Id == data.HLMRCA_Id && t.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.HLMRCA_Id).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        #endregion

        #region ================== MASTER FACILITY ============

        public MasterFacility_DTO get_facilitydata(MasterFacility_DTO data)
        {
            try
            {
                data.get_facilitylist = _HostelContext.HL_Master_Facility_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(a => a.HLMFTY_FacilityName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterFacility_DTO save_facilitydata(MasterFacility_DTO data)
        {
            try
            {
                if (data.HLMFTY_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_Facility_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMFTY_FacilityName == data.HLMFTY_FacilityFileName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_Facility_DMO obj1 = new HL_Master_Facility_DMO();

                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMFTY_FacilityName = data.HLMFTY_FacilityName;
                        obj1.HLMFTY_FacilityDesc = data.HLMFTY_FacilityDesc;
                        obj1.HLMFTY_FacilityFileName = data.HLMFTY_FacilityFileName;
                        obj1.HLMFTY_FacilityFilePath = data.HLMFTY_FacilityFilePath;
                        obj1.HLMFTY_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.HLMFTY_CreatedBy = data.UserId;
                        obj1.HLMFTY_UpdatedBy = data.UserId;

                        _HostelContext.Add(obj1);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }

                }
                else if (data.HLMFTY_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_Facility_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMFTY_Id != data.HLMFTY_Id && t.HLMFTY_FacilityName == data.HLMFTY_FacilityFileName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_Facility_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMFTY_Id == data.HLMFTY_Id).Single();

                        update.HLMFTY_FacilityName = data.HLMFTY_FacilityName;
                        update.HLMFTY_FacilityDesc = data.HLMFTY_FacilityDesc;
                        update.HLMFTY_FacilityFileName = data.HLMFTY_FacilityFileName;
                        update.HLMFTY_FacilityFilePath = data.HLMFTY_FacilityFilePath;
                        update.UpdatedDate = DateTime.Now;
                        update.HLMFTY_UpdatedBy = data.UserId;

                        _HostelContext.Update(update);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterFacility_DTO edit_faclitydata(MasterFacility_DTO data)
        {
            try
            {
                data.edit_facilitylist = _HostelContext.HL_Master_Facility_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMFTY_Id == data.HLMFTY_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterFacility_DTO deactiveY_faclitydata(MasterFacility_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_Facility_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMFTY_Id == data.HLMFTY_Id).Single();
                if (data.HLMFTY_ActiveFlag == true)
                {
                    result.HLMFTY_ActiveFlag = false;
                }
                else if (data.HLMFTY_ActiveFlag == false)
                {
                    result.HLMFTY_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.HLMFTY_UpdatedBy = data.UserId;

                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        #endregion

        #region ================== MESS CATEGORY ==============
        public HL_Master_MessCategory_DTO get_messCategorydata(HL_Master_MessCategory_DTO data)
        {
            try
            {
                data.get_messCategorylist = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(a => a.HLMMC_Name).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_MessCategory_DTO save_messCategorydata(HL_Master_MessCategory_DTO data)
        {
            try
            {
                if (data.HLMMC_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMC_Name == data.HLMMC_Name).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_MessCategory_DMO obj1 = new HL_Master_MessCategory_DMO();

                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMMC_Name = data.HLMMC_Name;
                        obj1.HLMMC_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.HLMMC_CreatedBy = data.UserId;
                        obj1.HLMMC_UpdatedBy = data.UserId;

                        _HostelContext.Add(obj1);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }

                }
                else if (data.HLMMC_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMC_Id != data.HLMMC_Id && t.HLMMC_Name == data.HLMMC_Name).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMC_Id == data.HLMMC_Id).Single();

                        update.HLMMC_Name = data.HLMMC_Name;
                        update.UpdatedDate = DateTime.Now;
                        update.HLMMC_UpdatedBy = data.UserId;
                        _HostelContext.Update(update);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_MessCategory_DTO deactiveY_messCategorydata(HL_Master_MessCategory_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMC_Id == data.HLMMC_Id).Single();
                if (data.HLMMC_ActiveFlag == true)
                {
                    result.HLMMC_ActiveFlag = false;
                }
                else if (data.HLMMC_ActiveFlag == false)
                {
                    result.HLMMC_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.HLMMC_UpdatedBy = data.UserId;

                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        #endregion

        #region ================= Master MESS =================

        public HL_Master_Mess_DTO get_Mmessdata(HL_Master_Mess_DTO data)
        {
            try
            {
                data.get_messCategorylist = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMC_ActiveFlag == true).Distinct().OrderBy(a => a.HLMMC_Name).ToArray();

                data.get_messlist = (from a in _HostelContext.HL_Master_Mess_DMO
                                         //from b in _HostelContext.HL_Master_MessCategory_DMO
                                     where (a.MI_Id == data.MI_Id /*&& a.HLMMC_Id == b.HLMMC_Id && a.MI_Id == b.MI_Id*/)
                                     select new HL_Master_Mess_DTO
                                     {
                                         HLMM_Id = a.HLMM_Id,
                                         HLMM_Name = a.HLMM_Name,
                                         //HLMMC_Id = b.HLMMC_Id,
                                         //HLMMC_Name = b.HLMMC_Name,
                                         HLMM_VegFlg = a.HLMM_VegFlg,
                                         HLMM_NonVegFlg = a.HLMM_NonVegFlg,
                                         HLMM_BFSStartTime = a.HLMM_BFSStartTime,
                                         HLMM_BFSEndTime = a.HLMM_BFSEndTime,
                                         HLMM_LNStartTime = a.HLMM_LNStartTime,
                                         HLMM_LNEndTime = a.HLMM_LNEndTime,
                                         HLMM_LNTSStartTime = a.HLMM_LNTSStartTime,
                                         HLMM_LNTSEndTime = a.HLMM_LNTSEndTime,
                                         HLMM_DNSStartTime = a.HLMM_DNSStartTime,
                                         HLMM_DNSEndTime = a.HLMM_DNSEndTime,
                                         HLMM_ActiveFlag = a.HLMM_ActiveFlag,

                                     }).Distinct().OrderBy(a => a.HLMM_Name).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Mess_DTO save_Mmessdata(HL_Master_Mess_DTO data)
        {
            try
            {
                if (data.HLMM_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_Mess_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMM_Name == data.HLMM_Name).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_Mess_DMO obj1 = new HL_Master_Mess_DMO();

                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMM_Name = data.HLMM_Name;
                        //obj1.HLMMC_Id = data.HLMMC_Id;
                        obj1.HLMM_VegFlg = data.HLMM_VegFlg;
                        obj1.HLMM_NonVegFlg = data.HLMM_NonVegFlg;
                        obj1.HLMM_BFSStartTime = data.HLMM_BFSStartTime;
                        obj1.HLMM_BFSEndTime = data.HLMM_BFSEndTime;
                        obj1.HLMM_LNStartTime = data.HLMM_LNStartTime;
                        obj1.HLMM_LNEndTime = data.HLMM_LNEndTime;
                        obj1.HLMM_LNTSStartTime = data.HLMM_LNTSStartTime;
                        obj1.HLMM_LNTSEndTime = data.HLMM_LNTSEndTime;
                        obj1.HLMM_DNSStartTime = data.HLMM_DNSStartTime;
                        obj1.HLMM_DNSEndTime = data.HLMM_DNSEndTime;
                        obj1.HLMM_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.HLMM_CreatedBy = data.UserId;
                        obj1.HLMM_UpdatedBy = data.UserId;

                        _HostelContext.Add(obj1);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }

                }
                else if (data.HLMM_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_Mess_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMM_Id != data.HLMM_Id && t.HLMM_Name == data.HLMM_Name).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_Mess_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMM_Id == data.HLMM_Id).Single();

                        update.HLMM_Name = data.HLMM_Name;
                        //update.HLMMC_Id = data.HLMMC_Id;
                        update.HLMM_VegFlg = data.HLMM_VegFlg;
                        update.HLMM_NonVegFlg = data.HLMM_NonVegFlg;
                        update.HLMM_BFSStartTime = data.HLMM_BFSStartTime;
                        update.HLMM_BFSEndTime = data.HLMM_BFSEndTime;
                        update.HLMM_LNStartTime = data.HLMM_LNStartTime;
                        update.HLMM_LNEndTime = data.HLMM_LNEndTime;
                        update.HLMM_LNTSStartTime = data.HLMM_LNTSStartTime;
                        update.HLMM_LNTSEndTime = data.HLMM_LNTSEndTime;
                        update.HLMM_DNSStartTime = data.HLMM_DNSStartTime;
                        update.HLMM_DNSEndTime = data.HLMM_DNSEndTime;
                        update.UpdatedDate = DateTime.Now;
                        update.HLMM_UpdatedBy = data.UserId;

                        _HostelContext.Update(update);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Mess_DTO edit_Mmessdata(HL_Master_Mess_DTO data)
        {
            try
            {
                data.edit_Messlist = (from t in _HostelContext.HL_Master_Mess_DMO
                                      where (t.MI_Id == data.MI_Id && t.HLMM_Id == data.HLMM_Id)
                                      select new HL_Master_Mess_DTO
                                      {
                                          HLMM_Id = t.HLMM_Id,
                                          MI_Id = t.MI_Id,
                                          HLMM_Name = t.HLMM_Name,
                                          HLMM_VegFlg = t.HLMM_VegFlg,
                                          HLMM_NonVegFlg = t.HLMM_NonVegFlg,
                                          HLMM_BFSStartTime = t.HLMM_BFSStartTime,
                                          HLMM_BFSEndTime = t.HLMM_BFSEndTime,
                                          HLMM_LNStartTime = t.HLMM_LNStartTime,
                                          HLMM_LNEndTime = t.HLMM_LNEndTime,
                                          HLMM_LNTSStartTime = t.HLMM_LNTSStartTime,
                                          HLMM_LNTSEndTime = t.HLMM_LNTSEndTime,
                                          HLMM_DNSStartTime = t.HLMM_DNSStartTime,
                                          HLMM_DNSEndTime = t.HLMM_DNSEndTime,

                                      }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Mess_DTO deactiveY_Mmessdata(HL_Master_Mess_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_Mess_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMM_Id == data.HLMM_Id).Single();
                if (data.HLMM_ActiveFlag == true)
                {
                    result.HLMM_ActiveFlag = false;
                }
                else if (data.HLMM_ActiveFlag == false)
                {
                    result.HLMM_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;

                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        #endregion

        #region ================= MESS MENU ===================

        public HL_Master_MessMenu_DTO get_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            try
            {
                data.get_messCategorylist = _HostelContext.HL_Master_MessCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMC_ActiveFlag == true).Distinct().OrderBy(a => a.HLMMC_Name).ToArray();

                data.get_messlist = (from a in _HostelContext.HL_Master_Mess_DMO
                                     where (a.MI_Id == data.MI_Id && a.HLMM_ActiveFlag == true)
                                     select new HL_Master_MessMenu_DTO
                                     {
                                         HLMM_Id = a.HLMM_Id,
                                         HLMM_Name = a.HLMM_Name,
                                     }).Distinct().OrderBy(a => a.HLMM_Name).ToArray();

                data.griddata = (from a in _HostelContext.HL_Master_Mess_DMO
                                 from b in _HostelContext.HL_Master_MessCategory_DMO
                                 from c in _HostelContext.HL_Master_MessMenu_DMO
                                 where (c.MI_Id == data.MI_Id && c.HLMM_Id == a.HLMM_Id && c.HLMMC_Id == b.HLMMC_Id && b.HLMMC_ActiveFlag == true
                                 && a.HLMM_ActiveFlag == true)
                                 select new HL_Master_MessMenu_DTO
                                 {
                                     HLMMN_Id = c.HLMMN_Id,
                                     HLMM_Id = a.HLMM_Id,
                                     HLMM_Name = a.HLMM_Name,
                                     HLMMN_MenuName = c.HLMMN_MenuName,
                                     HLMMN_MenuDesc = c.HLMMN_MenuDesc,
                                     HLMMC_Name = b.HLMMC_Name,
                                     HLMMC_Id = b.HLMMC_Id,
                                     HLMMN_ActiveFlag = c.HLMMN_ActiveFlag,
                                 }).Distinct().OrderBy(a => a.HLMM_Name).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_MessMenu_DTO save_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            try
            {
                if (data.HLMMN_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_MessMenu_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMN_MenuName == data.HLMMN_MenuName && t.HLMM_Id == data.HLMM_Id && t.HLMMC_Id == data.HLMMC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_MessMenu_DMO obj1 = new HL_Master_MessMenu_DMO();

                        obj1.HLMMN_Id = data.HLMMN_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMM_Id = data.HLMM_Id;
                        obj1.HLMMC_Id = data.HLMMC_Id;
                        obj1.HLMMN_MenuName = data.HLMMN_MenuName;
                        obj1.HLMMN_MenuDesc = data.HLMMN_MenuDesc;
                        obj1.HLMMN_ActiveFlag = true;
                        obj1.HLMMN_CreatedDate = DateTime.Now;
                        obj1.HLMMN_UpdatedDate = DateTime.Now;
                        obj1.HLMMN_UpdatedBy = data.UserId;
                        obj1.HLMMN_CreatedBy = data.UserId;

                        _HostelContext.Add(obj1);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }

                }
                else if (data.HLMMN_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_Mess_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMM_Id != data.HLMM_Id && t.HLMM_Name == data.HLMM_Name).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_MessMenu_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMN_Id == data.HLMMN_Id).Single();

                        update.HLMM_Id = data.HLMM_Id;
                        update.HLMMC_Id = data.HLMMC_Id;
                        update.HLMMN_MenuName = data.HLMMN_MenuName;
                        update.HLMMN_MenuDesc = data.HLMMN_MenuDesc;
                        update.HLMMN_UpdatedDate = DateTime.Now;
                        update.HLMMN_UpdatedBy = data.UserId;

                        _HostelContext.Update(update);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_MessMenu_DTO edit_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            try
            {
                data.edit_MessMenulist = _HostelContext.HL_Master_MessMenu_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMN_Id == data.HLMMN_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_MessMenu_DTO deactiveY_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_MessMenu_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMMN_Id == data.HLMMN_Id).Single();
                if (data.HLMMN_ActiveFlag == true)
                {
                    result.HLMMN_ActiveFlag = false;
                }
                else if (data.HLMMN_ActiveFlag == false)
                {
                    result.HLMMN_ActiveFlag = true;
                }
                result.HLMMN_UpdatedDate = DateTime.Now;
                result.HLMMN_UpdatedBy = data.UserId;

                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region ================= ROOM CATEGORY ===============

        public HL_Master_Room_Category_DTO get_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            try
            {
                

                //data.Feegroupe = _HostelContext.FeeGroupDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.FMG_GroupName).ToArray();

                data.Feegroupe = _HostelContext.FeeGroupDMO.Where(w => w.MI_Id == data.MI_Id && w.FMG_ActiceFlag == true && w.FMG_HostelFlg == true).Distinct().ToArray();

                data.griddata = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.HLMRCA_Id).ToArray();


                data.hostellist= _HostelContext.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(a => a.HLMH_Name).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Room_Category_DTO save_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            try
            {
                if (data.HLMRCA_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_RoomCategory == data.HLMRCA_RoomCategory).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_Room_Category_DMO obj1 = new HL_Master_Room_Category_DMO();

                        obj1.HLMRCA_Id = data.HLMRCA_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.HLMRCA_RoomCategory = data.HLMRCA_RoomCategory;
                        obj1.HLMRCA_Description = data.HLMRCA_Description;
                        obj1.HLMRCA_MaxCapacity = data.HLMRCA_MaxCapacity;
                        obj1.HLMRCA_ACFlg = data.HLMRCA_ACFlg;
                        obj1.HLMRCA_SharingFlg = data.HLMRCA_SharingFlg;
                        obj1.HLMRCA_SORate = data.HLMRCA_SORate;
                        obj1.HLMRCA_RoomRate = data.HLMRCA_RoomRate;
                        obj1.HLMH_Id = data.HLMH_Id;
                        obj1.FMG_Id = data.FMG_Id;
                        obj1.HLMRCA_ActiveFlag = true;
                        obj1.HLMRCA_CreatedDate = DateTime.Now;
                        obj1.HLMRCA_UpdatedDate = DateTime.Now;
                        obj1.HLMRCA_CreatedBy = data.UserId;
                        obj1.HLMRCA_UpdatedBy = data.UserId;

                        _HostelContext.Add(obj1);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }

                }
                else if (data.HLMRCA_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id != data.HLMRCA_Id && t.HLMRCA_RoomCategory == data.HLMRCA_RoomCategory).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id).Single();
                        update.HLMRCA_RoomCategory = data.HLMRCA_RoomCategory;
                        update.HLMRCA_Description = data.HLMRCA_Description;
                        update.HLMRCA_MaxCapacity = data.HLMRCA_MaxCapacity;
                        update.HLMRCA_ACFlg = data.HLMRCA_ACFlg;
                        update.HLMRCA_SharingFlg = data.HLMRCA_SharingFlg;
                        update.HLMRCA_SORate = data.HLMRCA_SORate;
                        update.HLMRCA_RoomRate = data.HLMRCA_RoomRate;
                        update.HLMH_Id = data.HLMH_Id;
                        update.FMG_Id = data.FMG_Id;
                        update.HLMRCA_UpdatedDate = DateTime.Now;
                        update.HLMRCA_UpdatedBy = data.UserId;
                        _HostelContext.Update(update);

                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Room_Category_DTO edit_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            try
            {
                data.edit_Roomcatlist = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Master_Room_Category_DTO deactiveY_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id).Single();
                if (data.HLMRCA_ActiveFlag == true)
                {
                    result.HLMRCA_ActiveFlag = false;
                }
                else if (data.HLMRCA_ActiveFlag == false)
                {
                    result.HLMRCA_ActiveFlag = true;
                }
                result.HLMRCA_UpdatedDate = DateTime.Now;
                result.HLMRCA_UpdatedBy = data.UserId;

                _HostelContext.Update(result);
                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

    }
}
