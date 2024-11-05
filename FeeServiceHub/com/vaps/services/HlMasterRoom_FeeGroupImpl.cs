using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class HlMasterRoom_FeeGroupImpl : interfaces.HlMasterRoom_FeeGroupInterface
    {

        public FeeGroupContext _context;
        //public DomainModelMsSqlServerContext _org;

        public HlMasterRoom_FeeGroupImpl(FeeGroupContext context)
        {

            _context = context;
        }
        public HlMasterRoom_FeeGroupDTO loaddata(HlMasterRoom_FeeGroupDTO data)
        {
            try
            {
                data.alldata1 = (from a in _context.HlMasterRoom_FeeGroupDMO
                                 from b in _context.HR_Master_Room_DMO
                                 from c in _context.FeeGroupDMO
                                 from d in _context.HL_Master_Hostel_DMO
                                 from e in _context.HR_Master_Floor_DMO
                                 let p = a.MI_Id == data.MI_Id && a.HRMRM_Id == b.HRMRM_Id && b.HLMH_Id == d.HLMH_Id && b.HLMF_Id == e.HLMF_Id && b.MI_Id==d.MI_Id && b.MI_Id==e.MI_Id
                                 where p && a.FMG_Id == c.FMG_Id && b.HRMRM_ActiveFlag == true
                                 && c.FMG_ActiceFlag == true
                                 select new HlMasterRoom_FeeGroupDTO
                                 {
                                     HRMRM_Id = a.HRMRM_Id,
                                     FMG_Id = a.FMG_Id,
                                     HLMRFG_Id = a.HLMRFG_Id,
                                     HLMRFG_ActiveFlag = a.HLMRFG_ActiveFlag,
                                     HRMRM_RoomNo = d.HLMH_Name + '-' +  e.HRMF_FloorName + '-' + b.HRMRM_RoomNo,
                                     FMG_GroupName = c.FMG_GroupName,
                                 }).Distinct().ToArray();

                data.roomid = (from a in _context.HR_Master_Room_DMO
                               from b in _context.HL_Master_Hostel_DMO
                               from c in _context.HR_Master_Floor_DMO
                               where (a.HLMH_Id==b.HLMH_Id && a.HLMF_Id==c.HLMF_Id && a.MI_Id==b.MI_Id && a.MI_Id==c.MI_Id && a.MI_Id==data.MI_Id && a.HRMRM_ActiveFlag== true && b.HLMH_ActiveFlag== true && c.HRMF_ActiveFlag==true)
                                 select new HlMasterRoom_FeeGroupDTO
                                 {
                                     HRMRM_Id = a.HRMRM_Id,
                                     HRMRM_RoomNo = b.HLMH_Name + '-' + c.HRMF_FloorName + '-'+ a.HRMRM_RoomNo,
                                 }).Distinct().ToArray();

                //data.roomid = _context.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

                data.groupid = _context.FeeGroupDMO.Where(t => t.MI_Id == data.MI_Id && t.FMG_HostelFlg==true && t.FMG_ActiceFlag==true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public HlMasterRoom_FeeGroupDTO save(HlMasterRoom_FeeGroupDTO data)
        {
            try
            {
                if (data.HLMRFG_Id == 0)
                {
                    {
                        var duplicate = _context.HlMasterRoom_FeeGroupDMO.Where(t => t.HRMRM_Id == data.HRMRM_Id && t.FMG_Id == data.FMG_Id && t.HLMRFG_Id != 0).Distinct().ToArray();
                        if (duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            var roomid = _context.HR_Master_Room_DMO.Where(t => t.HRMRM_Id == data.HRMRM_Id).ToList();

                            var groupid = _context.FeeGroupDMO.Where(t => t.FMG_Id == data.FMG_Id).ToList();


                            HlMasterRoom_FeeGroupDMO a = new HlMasterRoom_FeeGroupDMO();

                            a.HRMRM_Id = data.HRMRM_Id;
                            a.FMG_Id = data.FMG_Id;
                            a.MI_Id = data.MI_Id;
                            a.HLMRFG_ActiveFlag = true;
                            a.HLMRFG_CreatedDate = DateTime.Now;
                            a.HLMRFG_UpdatedDate = DateTime.Now;
                            a.HLMRFG_CreatedBy = data.UserId;
                            a.HLMRFG_UpdatedBy = data.UserId;

                            _context.Add(a);
                        }
                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "Saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.HLMRFG_Id > 0)
                {
                    var duplicate = _context.HlMasterRoom_FeeGroupDMO.Where(t => t.HLMRFG_Id != data.HLMRFG_Id && t.HRMRM_Id == data.HRMRM_Id && t.HLMRFG_Id == data.FMG_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var j = _context.HlMasterRoom_FeeGroupDMO.Where(t => t.HLMRFG_Id == data.HLMRFG_Id).SingleOrDefault();

                        j.HLMRFG_Id = data.HLMRFG_Id;
                        j.HRMRM_Id = data.HRMRM_Id;
                        j.FMG_Id = data.FMG_Id;
                        j.MI_Id = data.MI_Id;
                        j.HLMRFG_UpdatedDate = DateTime.Now;
                        j.HLMRFG_UpdatedBy = data.UserId;

                        _context.Update(j);

                        var r = _context.SaveChanges();
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
        public HlMasterRoom_FeeGroupDTO edittab1(HlMasterRoom_FeeGroupDTO data)
        {
            try
            {


                data.Editlist = (from d in _context.HlMasterRoom_FeeGroupDMO
                                 where (d.HLMRFG_Id == data.HLMRFG_Id && d.MI_Id == data.MI_Id)
                                 select d).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HlMasterRoom_FeeGroupDTO deactive(HlMasterRoom_FeeGroupDTO data)
        {
            try
            {
                var g = _context.HlMasterRoom_FeeGroupDMO.Where(t => t.HLMRFG_Id == data.HLMRFG_Id).SingleOrDefault();
                if (g.HLMRFG_ActiveFlag == true)
                {
                    g.HLMRFG_ActiveFlag = false;
                }
                else if (g.HLMRFG_ActiveFlag == false)
                {
                    g.HLMRFG_ActiveFlag = true;
                }

                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
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

    }
}


