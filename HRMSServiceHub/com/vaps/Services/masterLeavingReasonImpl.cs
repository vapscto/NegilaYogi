using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class masterLeavingReasonImpl:Interfaces.masterLeavingReasonInterface
    {
        public HRMSContext _Context;
        public masterLeavingReasonImpl(HRMSContext hh)
        {
            _Context = hh;
        }
        public masterLeavingReasonDTO loaddata(masterLeavingReasonDTO data)
        {
            try
            {
                data.alldata = _Context.masterLeavingReasonDMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLREA_Id != 0).Distinct().ToArray();
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public masterLeavingReasonDTO savedata(masterLeavingReasonDTO data)
        {
            try
            {
                if (data.HRMLREA_Id == 0)
                {
                    var dupplicate = _Context.masterLeavingReasonDMO.Where(t => t.HRMLREA_Id != 0 && t.MI_Id == data.MI_Id && t.HRMLREA_LeavingReason == data.HRMLREA_LeavingReason && t.HRMLREA_TransferredFlg==data.HRMLREA_TransferredFlg).ToArray();
                    if (dupplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        masterLeavingReasonDMO obj = new masterLeavingReasonDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRMLREA_LeavingReason = data.HRMLREA_LeavingReason;
                        obj.HRMLREA_ActiveFlg = true;
                        obj.HRMLREA_TransferredFlg = data.HRMLREA_TransferredFlg;
                        obj.HRMLREA_CreatedBy = data.UserId;
                        obj.HRMLREA_UpdatedBy = data.UserId;
                        obj.HRMLREA_CreatedDate = DateTime.Now;
                        obj.HRMLREA_UpdatedDate = DateTime.Now;
                        _Context.Add(obj);
                        int y = _Context.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "Saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.HRMLREA_Id > 0)
                {
                    var duplicate = _Context.masterLeavingReasonDMO.Where(t => t.HRMLREA_Id != data.HRMLREA_Id && t.MI_Id == data.MI_Id && t.HRMLREA_LeavingReason == data.HRMLREA_LeavingReason && t.HRMLREA_TransferredFlg==data.HRMLREA_TransferredFlg).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var ss = _Context.masterLeavingReasonDMO.Where(t => t.HRMLREA_Id == data.HRMLREA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();
                        ss.MI_Id = data.MI_Id;
                        ss.HRMLREA_LeavingReason = data.HRMLREA_LeavingReason;
                        ss.HRMLREA_TransferredFlg = data.HRMLREA_TransferredFlg;
                        ss.HRMLREA_UpdatedBy = data.UserId;
                        ss.HRMLREA_UpdatedDate = DateTime.Now;
                        _Context.Update(ss);
                        int s = _Context.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "Updated";
                        }
                        else
                        {
                            data.msg = "Updation Failed";
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
        public masterLeavingReasonDTO EditData(masterLeavingReasonDTO data)
        {
            try
            {
                data.editlist = (from a in _Context.masterLeavingReasonDMO
                                 where a.HRMLREA_Id == data.HRMLREA_Id && a.MI_Id == data.MI_Id
                                 select new masterLeavingReasonDMO
                                 {
                                     HRMLREA_Id = a.HRMLREA_Id,
                                     MI_Id = a.MI_Id,
                                     HRMLREA_LeavingReason = a.HRMLREA_LeavingReason,
                                     HRMLREA_TransferredFlg = a.HRMLREA_TransferredFlg
                                 }).Distinct().ToArray();
            }
            catch (Exception exe)
            {
                Console.WriteLine(exe.Message);
            }
            return data;
        }
        public masterLeavingReasonDTO masterDecative(masterLeavingReasonDTO data)
        {
            try
            {
                var u = _Context.masterLeavingReasonDMO.Where(t => t.HRMLREA_Id == data.HRMLREA_Id).SingleOrDefault();
                if (u.HRMLREA_ActiveFlg == true)
                {
                    u.HRMLREA_ActiveFlg = false;
                }
                else if (u.HRMLREA_ActiveFlg == false)
                {
                    u.HRMLREA_ActiveFlg = true;
                }
                _Context.Update(u);
                int o = _Context.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
