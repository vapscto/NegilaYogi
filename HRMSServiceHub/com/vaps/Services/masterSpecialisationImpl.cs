using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class masterSpecialisationImpl:Interfaces.masterSpecialisationInterface
    {
        public HRMSContext _Context;
        public masterSpecialisationImpl(HRMSContext hh)
        {
            _Context = hh;
        }
        public masterSpecialisationDTO loaddata(masterSpecialisationDTO data)
        {
            try
            {
                data.alldata = _Context.masterSpecialisationDMO.Where(t => t.MI_Id == data.MI_Id && t.HRMSPL_Id != 0).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public masterSpecialisationDTO savedata(masterSpecialisationDTO data)
        {
            try
            {
                if (data.HRMSPL_Id == 0)
                {
                    var dupplicate = _Context.masterSpecialisationDMO.Where(t => t.HRMSPL_Id != 0 && t.MI_Id == data.MI_Id && t.HRMSPL_SpecialisationName == data.HRMSPL_SpecialisationName).ToArray();
                    if (dupplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        masterSpecialisationDMO obj = new masterSpecialisationDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRMSPL_SpecialisationName = data.HRMSPL_SpecialisationName;
                        obj.HRMSPL_ActiveFlg = true;
                        obj.HRMSPL_CreatedBy = data.UserId;
                        obj.HRMSPL_UpdatedBy = data.UserId;
                        obj.HRMSPL_CreatedDate = DateTime.Now;
                        obj.HRMSPL_UpdatedDate = DateTime.Now;
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
                else if (data.HRMSPL_Id > 0)
                {
                    var duplicate = _Context.masterSpecialisationDMO.Where(t => t.HRMSPL_Id != data.HRMSPL_Id && t.MI_Id == data.MI_Id && t.HRMSPL_SpecialisationName==data.HRMSPL_SpecialisationName).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var ss = _Context.masterSpecialisationDMO.Where(t => t.HRMSPL_Id == data.HRMSPL_Id && t.MI_Id == data.MI_Id).SingleOrDefault();
                        ss.MI_Id = data.MI_Id;
                        ss.HRMSPL_SpecialisationName = data.HRMSPL_SpecialisationName;                       
                        ss.HRMSPL_UpdatedBy = data.UserId;
                        ss.HRMSPL_UpdatedDate = DateTime.Now;
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public masterSpecialisationDTO EditData(masterSpecialisationDTO data)
        {
            try
            {
                data.editlist = (from a in _Context.masterSpecialisationDMO
                                 where a.HRMSPL_Id == data.HRMSPL_Id && a.MI_Id == data.MI_Id
                                 select new masterSpecialisationDMO
                                 {
                                     HRMSPL_Id = a.HRMSPL_Id,
                                     MI_Id = a.MI_Id,
                                     HRMSPL_SpecialisationName = a.HRMSPL_SpecialisationName                                    
                                 }).Distinct().ToArray();
            }
            catch (Exception exe)
            {
                Console.WriteLine(exe.Message);
            }
            return data;
        }
        public masterSpecialisationDTO masterDecative(masterSpecialisationDTO data)
        {
            try
            {
                var u = _Context.masterSpecialisationDMO.Where(t => t.HRMSPL_Id == data.HRMSPL_Id).SingleOrDefault();
                if (u.HRMSPL_ActiveFlg == true)
                {
                    u.HRMSPL_ActiveFlg = false;
                }
                else if (u.HRMSPL_ActiveFlg == false)
                {
                    u.HRMSPL_ActiveFlg = true;
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
