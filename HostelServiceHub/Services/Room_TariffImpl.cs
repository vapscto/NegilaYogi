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
    public class Room_TariffImpl:Interface.Room_TariffInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public Room_TariffImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }     
        public Room_Tariff_DTO loaddata(Room_Tariff_DTO data)
        {
            try
            {
                data.yeralist = (from a in _dbcontext.AcademicYear
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_ActiveFlag == 1)
                                 select new Room_Tariff_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new Room_Tariff_DTO
                                  {
                                      HRMRM_Id = a.HRMRM_Id,
                                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                                  }).Distinct().OrderBy(t => t.HRMRM_Id).ToArray();

                data.gridlistdata = (from a in _HostelContext.HL_Master_Room_Tariff_DMO
                                     from b in _HostelContext.HR_Master_Room_DMO
                                     from c in _HostelContext.AcademicYear
                                     where (a.MI_Id == data.MI_Id && a.HRMRM_Id == b.HRMRM_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id)
                                     select new Room_Tariff_DTO
                                     {
                                         HLMRTF_Id = a.HLMRTF_Id,
                                         HRMRM_Id = a.HRMRM_Id,
                                         ASMAY_Id = a.ASMAY_Id,
                                         ASMAY_Year = c.ASMAY_Year,
                                         HRMRM_RoomNo = b.HRMRM_RoomNo,
                                         HLMRTF_SORate = a.HLMRTF_SORate,
                                         HLMRTF_RoomRate = a.HLMRTF_RoomRate,
                                         HLMRTF_ActiveFlag = a.HLMRTF_ActiveFlag,

                                     }).Distinct().OrderByDescending(t => t.HLMRTF_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Room_Tariff_DTO savedata(Room_Tariff_DTO data)
        {
            try
            {
                if (data.HLMRTF_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Master_Room_Tariff_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                    && t.HRMRM_Id == data.HRMRM_Id && t.HLMRTF_SORate == data.HLMRTF_SORate && t.HLMRTF_RoomRate == data.HLMRTF_RoomRate).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Master_Room_Tariff_DMO obj1 = new HL_Master_Room_Tariff_DMO();

                        obj1.HLMRTF_Id = data.HLMRTF_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.ASMAY_Id = data.ASMAY_Id;
                        obj1.HRMRM_Id = data.HRMRM_Id;
                        obj1.HLMRTF_SORate = data.HLMRTF_SORate;
                        obj1.HLMRTF_RoomRate = data.HLMRTF_RoomRate;
                        obj1.HLMRTF_ActiveFlag = true;
                        obj1.HLMRTF_CreatedDate = DateTime.Now;
                        obj1.HLMRTF_UpdatedDate = DateTime.Now;
                        obj1.HLMRTF_CreatedBy = data.UserId;
                        obj1.HLMRTF_UpdatedBy = data.UserId;

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
                else if (data.HLMRTF_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Master_Room_Tariff_DMO.Where(t => t.HLMRTF_Id != data.HLMRTF_Id && t.MI_Id == data.MI_Id
                    && t.HLMRTF_Id != data.HLMRTF_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRMRM_Id == data.HRMRM_Id
                    && t.HLMRTF_SORate == data.HLMRTF_SORate && t.HLMRTF_RoomRate == data.HLMRTF_RoomRate).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Master_Room_Tariff_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRTF_Id == data.HLMRTF_Id).Single();

                        update.ASMAY_Id = data.ASMAY_Id;
                        update.HRMRM_Id = data.HRMRM_Id;
                        update.HLMRTF_SORate = data.HLMRTF_SORate;
                        update.HLMRTF_RoomRate = data.HLMRTF_RoomRate;
                        update.HLMRTF_UpdatedDate = DateTime.Now;
                        update.HLMRTF_UpdatedBy = data.UserId;

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
        public Room_Tariff_DTO editdata(Room_Tariff_DTO data)
        {
            try
            {
                data.editlist = _HostelContext.HL_Master_Room_Tariff_DMO.Where(T => T.HLMRTF_Id == data.HLMRTF_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Room_Tariff_DTO Ydeactive(Room_Tariff_DTO data)
        {
            try
            {
                var result = _HostelContext.HL_Master_Room_Tariff_DMO.Single(t => t.HLMRTF_Id == data.HLMRTF_Id && t.MI_Id == data.MI_Id);

                if (result.HLMRTF_ActiveFlag == true)
                {
                    result.HLMRTF_ActiveFlag = false;
                }
                else if (result.HLMRTF_ActiveFlag == false)
                {
                    result.HLMRTF_ActiveFlag = true;
                }
                result.HLMRTF_UpdatedDate = DateTime.Now;
                result.HLMRTF_UpdatedBy = data.UserId;

                _HostelContext.Update(result);
                int rowAffected = _HostelContext.SaveChanges();

                if (rowAffected > 0)
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
        public Room_Tariff_DTO get_bedcount(Room_Tariff_DTO data)
        {
            try
            {               

                data.HRMRM_BedCapacity = _HostelContext.HR_Master_Room_DMO.Where(t => t.HRMRM_Id == data.HRMRM_Id && t.MI_Id == data.MI_Id).SingleOrDefault().HRMRM_BedCapacity;
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}

