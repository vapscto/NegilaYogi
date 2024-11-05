using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class HostelAllotForGuestImpl : Interface.HostelAllotForGuestInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;

        public HostelAllotForGuestImpl(HostelContext w, DomainModelMsSqlServerContext v)
        {
            _HostelContext = w;
            _dbcontext = v;
        }
        public HostelAllotForGuest_DTO loaddata(HostelAllotForGuest_DTO data)
        {
            try
            {
                data.hstllist = _HostelContext.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_ActiveFlag == true).Distinct().ToArray();
                data.categorylist = _HostelContext.HL_Master_Room_Category_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMRCA_ActiveFlag == true).Distinct().ToArray();
                data.roomlist = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_ActiveFlag == true).Distinct().ToArray();
                data.alldata1 = (from a in _HostelContext.HL_Hostel_Guest_Allot_DMO
                                 from b in _HostelContext.HL_Master_Hostel_DMO
                                 from c in _HostelContext.HR_Master_Room_DMO
                                 from d in _HostelContext.HL_Master_Room_Category_DMO
                                 where (a.HLMH_Id == b.HLMH_Id && c.HRMRM_Id == a.HRMRM_Id && d.HLMRCA_Id == a.HLMRCA_Id && a.MI_Id == data.MI_Id)
                                 select new HostelAllotForGuest_DTO
                                 {
                                     HLHGSTALT_Id = a.HLHGSTALT_Id,
                                     HLHGSTALT_GuestName = a.HLHGSTALT_GuestName,
                                     HLHGSTALT_GuestPhoneNo = a.HLHGSTALT_GuestPhoneNo,
                                     HLHGSTALT_GuestEmailId = a.HLHGSTALT_GuestEmailId,
                                     HLMH_Id = a.HLMH_Id,
                                     HRMRM_Id = a.HRMRM_Id,
                                     HLMH_Name = b.HLMH_Name,
                                     HRMRM_RoomNo = c.HRMRM_RoomNo,
                                     HLMRCA_Id = a.HLMRCA_Id,
                                     HLMRCA_RoomCategory = d.HLMRCA_RoomCategory,
                                     HLHGSTALT_NoOfBeds = a.HLHGSTALT_NoOfBeds,
                                     HLHGSTALT_ActiveFlag = a.HLHGSTALT_ActiveFlag
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HostelAllotForGuest_DTO save(HostelAllotForGuest_DTO data)
        {
            try
            {
                if (data.HLHGSTALT_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Hostel_Guest_Allot_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHGSTALT_GuestName == data.HLHGSTALT_GuestName && t.HLMH_Id == data.HLMH_Id && t.HLHGSTALT_GuestPhoneNo == data.HLHGSTALT_GuestPhoneNo && t.HLHGSTALT_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        HL_Hostel_Guest_Allot_DMO u = new HL_Hostel_Guest_Allot_DMO();
                        u.MI_Id = data.MI_Id;
                        u.HLHGSTALT_AllotmentDate = data.HLHGSTALT_AllotmentDate;
                        u.HLMH_Id = data.HLMH_Id;
                        u.HLMRCA_Id = data.HLMRCA_Id;
                        u.HRMRM_Id = data.HRMRM_Id;
                        u.HLHGSTALT_GuestName = data.HLHGSTALT_GuestName;
                        u.HLHGSTALT_GuestPhoneNo = data.HLHGSTALT_GuestPhoneNo;
                        u.HLHGSTALT_GuestEmailId = data.HLHGSTALT_GuestEmailId;
                        u.HLHGSTALT_GuestAddress = data.HLHGSTALT_GuestAddress;
                        u.HLHGSTALT_GuestPhoto = data.HLHGSTALT_GuestPhoto;
                        u.HLHGSTALT_AddressProof = data.HLHGSTALT_AddressProof;
                        u.HLHGSTALT_NoOfBeds = data.HLHGSTALT_NoOfBeds;
                        u.HLHGSTALT_AllotRemarks = data.HLHGSTALT_AllotRemarks;
                        u.HLHGSTALT_VacateRemarks = "";
                        u.HLHGSTALT_VacatedDate = DateTime.Now;
                        u.HLHGSTALT_VacateFlg = false;
                        u.HLHGSTALT_CreatedBy = data.UserId;
                        u.HLHGSTALT_UpdatedBy = data.UserId;
                        u.HLHGSTALT_CreatedDate = DateTime.Now;
                        u.HLHGSTALT_UpdatedDate = DateTime.Now;
                        u.HLHGSTALT_ActiveFlag = true;
                        _HostelContext.Add(u);
                        var w = _HostelContext.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }
                else if (data.HLHGSTALT_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Hostel_Guest_Allot_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHGSTALT_Id != data.HLHGSTALT_Id && t.HLHGSTALT_GuestName == data.HLHGSTALT_GuestName && t.HLHGSTALT_GuestPhoneNo == data.HLHGSTALT_GuestPhoneNo && t.HLMH_Id == data.HLMH_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var jj = _HostelContext.HL_Hostel_Guest_Allot_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHGSTALT_Id == data.HLHGSTALT_Id).SingleOrDefault();
                        jj.MI_Id = data.MI_Id;
                        jj.HLHGSTALT_AllotmentDate = data.HLHGSTALT_AllotmentDate;
                        jj.HLMH_Id = data.HLMH_Id;
                        jj.HLMRCA_Id = data.HLMRCA_Id;
                        jj.HRMRM_Id = data.HRMRM_Id;
                        jj.HLHGSTALT_GuestName = data.HLHGSTALT_GuestName;
                        jj.HLHGSTALT_GuestPhoneNo = data.HLHGSTALT_GuestPhoneNo;
                        jj.HLHGSTALT_GuestEmailId = data.HLHGSTALT_GuestEmailId;
                        jj.HLHGSTALT_GuestAddress = data.HLHGSTALT_GuestAddress;
                        jj.HLHGSTALT_GuestPhoto = data.HLHGSTALT_GuestPhoto;
                        jj.HLHGSTALT_AddressProof = data.HLHGSTALT_AddressProof;
                        jj.HLHGSTALT_NoOfBeds = data.HLHGSTALT_NoOfBeds;
                        jj.HLHGSTALT_AllotRemarks = data.HLHGSTALT_AllotRemarks;
                        jj.HLHGSTALT_VacateRemarks = "";
                        jj.HLHGSTALT_VacatedDate = DateTime.Now;
                        jj.HLHGSTALT_VacateFlg = false;
                        jj.HLHGSTALT_UpdatedBy = data.UserId;
                        jj.HLHGSTALT_UpdatedDate = DateTime.Now;
                        _HostelContext.Update(jj);
                        var r = _HostelContext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HostelAllotForGuest_DTO deactive(HostelAllotForGuest_DTO data)
        {
            try
            {
                var g = _HostelContext.HL_Hostel_Guest_Allot_DMO.Where(t => t.HLHGSTALT_Id == data.HLHGSTALT_Id).SingleOrDefault();
                if (g.HLHGSTALT_ActiveFlag == true)
                {
                    g.HLHGSTALT_ActiveFlag = false;
                }
                else
                {
                    g.HLHGSTALT_ActiveFlag = true;
                }
                g.HLHGSTALT_UpdatedDate = DateTime.Now;
                g.HLHGSTALT_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _HostelContext.Update(g);
                int s = _HostelContext.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HostelAllotForGuest_DTO EditData(HostelAllotForGuest_DTO data)
        {
            try
            {
                data.editlist = (from a in _HostelContext.HL_Hostel_Guest_Allot_DMO
                                 from b in _HostelContext.HL_Master_Hostel_DMO
                                 from c in _HostelContext.HR_Master_Room_DMO
                                 from d in _HostelContext.HL_Master_Room_Category_DMO
                                 where (a.HLMH_Id == b.HLMH_Id && c.HRMRM_Id == a.HRMRM_Id && d.HLMRCA_Id == a.HLMRCA_Id && a.MI_Id == data.MI_Id && a.HLHGSTALT_Id == data.HLHGSTALT_Id)
                                 select new HostelAllotForGuest_DTO
                                 {
                                     HLHGSTALT_Id = a.HLHGSTALT_Id,
                                     MI_Id = a.MI_Id,
                                     HLHGSTALT_AllotmentDate = a.HLHGSTALT_AllotmentDate,
                                     HLHGSTALT_GuestName = a.HLHGSTALT_GuestName,
                                     HLHGSTALT_GuestPhoneNo = a.HLHGSTALT_GuestPhoneNo,
                                     HLHGSTALT_GuestEmailId = a.HLHGSTALT_GuestEmailId,
                                     HLHGSTALT_GuestAddress = a.HLHGSTALT_GuestAddress,
                                     HLHGSTALT_GuestPhoto = a.HLHGSTALT_GuestPhoto,
                                     HLHGSTALT_AddressProof = a.HLHGSTALT_AddressProof,
                                     HLHGSTALT_AllotRemarks = a.HLHGSTALT_AllotRemarks,
                                     HLMH_Id = a.HLMH_Id,
                                     HRMRM_Id = a.HRMRM_Id,
                                     HLMH_Name = b.HLMH_Name,
                                     HRMRM_RoomNo = c.HRMRM_RoomNo,
                                     HLMRCA_Id = a.HLMRCA_Id,
                                     HLMRCA_RoomCategory = d.HLMRCA_RoomCategory,
                                     HLHGSTALT_NoOfBeds = a.HLHGSTALT_NoOfBeds,
                                     HLHGSTALT_ActiveFlag = a.HLHGSTALT_ActiveFlag
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HostelAllotForGuest_DTO get_roomdetails(HostelAllotForGuest_DTO data)
        {
            try
            {
                data.HLHGSTALT_NoOfBeds = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id).Single().HRMRM_BedCapacity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
