using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class MasterEventsImpl : Interfaces.MasterEventsInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterEventsImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterEventsDTO getDetails(MasterEventsDTO data)
        {
            try
            {
                data.eventList = _context.MasterEventsDMO.Where(d => d.MI_Id == data.MI_Id /*&& d.SPCCME_Flag == data.radiotype*/).Distinct().OrderBy(t=>t.SPCCME_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MasterEventsDTO saveRecord(MasterEventsDTO obj)
        {
            try
            {
                if (obj.SPCCME_Id > 0)
                {
                    var checkduplicate = _context.MasterEventsDMO.Where(d => d.MI_Id == obj.MI_Id /*&& d.SPCCME_Flag == obj.SPCCME_Flag*/ && d.SPCCME_EventName.Equals(obj.SPCCME_EventName) && d.SPCCME_Id != obj.SPCCME_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterEventsDMO.Where(d => d.SPCCME_Id == obj.SPCCME_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.MasterEventsDMO.Single(d => d.SPCCME_Id == obj.SPCCME_Id);
                           
                            update.SPCCME_EventName = obj.SPCCME_EventName;
                            update.SPCCME_EventNameDesc = obj.SPCCME_EventNameDesc;
                            //update.SPCCME_Flag = obj.SPCCME_Flag;
                            //update.SPCCME_ActiveFlag = true;
                            update.UpdatedDate = DateTime.Now;
                            _context.Update(update);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "updated";
                            }
                            else
                            {
                                obj.returnVal = "updateFailed";
                            }
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.MasterEventsDMO.Where(d => d.MI_Id == obj.MI_Id /*&& d.SPCCME_Flag == obj.SPCCME_Flag*/ && d.SPCCME_EventName.Equals(obj.SPCCME_EventName)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterEventsDMO>(obj);
                        mapp.SPCCME_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                       
                        _context.Add(mapp);
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public MasterEventsDTO EditDetails(MasterEventsDTO id)
        {
            MasterEventsDTO resp = new MasterEventsDTO();
            try
            {
                resp.editDetails = _context.MasterEventsDMO.Where(d => d.MI_Id == id.MI_Id && d.SPCCME_Id==id.SPCCME_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterEventsDTO deactivate(MasterEventsDTO obj)
        {
            try
            {
                MasterEventsDTO enq = Mapper.Map<MasterEventsDTO>(obj);
                var query = _context.MasterEventsDMO.Where(d => d.SPCCME_Id == obj.SPCCME_Id).ToList();
                if (query.Count > 0)
                {
                    var result = _context.MasterEventsDMO.Single(t => t.SPCCME_Id == enq.SPCCME_Id);
                    if (result.SPCCME_ActiveFlag == true)
                    {
                        result.SPCCME_ActiveFlag = false;
                    }
                    else
                    {
                        result.SPCCME_ActiveFlag = true;
                    }
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    var flag = _context.SaveChanges();
                    if (flag == 1)
                    {
                        if (result.SPCCME_ActiveFlag == true)
                        {
                            obj.returnVal = "Event Activated Successfully.";
                        }
                        else if (result.SPCCME_ActiveFlag == false)
                        {
                            obj.returnVal = "Event Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        obj.returnVal = "Activation Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}
