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
    public class MasterEventVenueImpl:Interfaces.MasterEventVenueInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterEventVenueImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterEventVenueDTO getDetails(MasterEventVenueDTO data)
        {
            try
            {
                var eventvenue = _context.MasterEventVenueDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                if (eventvenue.Count > 0)
                {
                    data.eventVenueList = eventvenue.ToArray();
                    data.count = eventvenue.Count;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MasterEventVenueDTO saveRecord(MasterEventVenueDTO obj)
        {
            try
            {
                if (obj.SPCCMEV_Id == 0)
                {
                    var checkduplicate = _context.MasterEventVenueDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMEV_EventVenue.Equals(obj.SPCCMEV_EventVenue)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterEventVenueDMO>(obj);
                        mapp.SPCCMEV_ActiveFlag = true;
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
                else if (obj.SPCCMEV_Id > 0)
                {
                    var checkduplicate = _context.MasterEventVenueDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMEV_EventVenue.Equals(obj.SPCCMEV_EventVenue) && d.SPCCMEV_Id != obj.SPCCMEV_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterEventVenueDMO.Where(d => d.SPCCMEV_Id == obj.SPCCMEV_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.MasterEventVenueDMO.Single(d => d.SPCCMEV_Id == obj.SPCCMEV_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCMEV_EventVenue = obj.SPCCMEV_EventVenue;
                            update.SPCCMEV_EventVenueDesc = obj.SPCCMEV_EventVenueDesc;
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

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public MasterEventVenueDTO EditDetails(int id)
        {
            MasterEventVenueDTO resp = new MasterEventVenueDTO();
            try
            {
                resp.editDetails = _context.MasterEventVenueDMO.Where(d => d.SPCCMEV_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterEventVenueDTO deactivate(MasterEventVenueDTO obj)
        {
            try
            {
                MasterEventVenueDTO enq = Mapper.Map<MasterEventVenueDTO>(obj);
                var query = _context.MasterEventVenueDMO.Where(d => d.SPCCMEV_Id == obj.SPCCMEV_Id).ToList();
                if (query.Count > 0)
                {
                    var result = _context.MasterEventVenueDMO.Single(t => t.SPCCMEV_Id == enq.SPCCMEV_Id);
                    if (result.SPCCMEV_ActiveFlag == true)
                    {
                        result.SPCCMEV_ActiveFlag = false;
                    }
                    else
                    {
                        result.SPCCMEV_ActiveFlag = true;
                    }
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    var flag = _context.SaveChanges();
                    if (flag == 1)
                    {
                        if (result.SPCCMEV_ActiveFlag == true)
                        {
                            obj.returnVal = "Event Venue Activated Successfully.";
                        }
                        else if (result.SPCCMEV_ActiveFlag == false)
                        {
                            obj.returnVal = "Event Venue Deactivated Successfully.";
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
