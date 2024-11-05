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
    public class EventsSponsorImpl : Interfaces.EventsSponsorInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public EventsSponsorImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public EventsSponsorDTO getDetails(EventsSponsorDTO data)
        {
            try
            {
                data.events = (from a in _context.ProgramMasterDMO
                               from b in _context.EventsMappingDMO
                               where/* a.SPCCPM_Id == b.SPCCPM_Id &&*/ b.MI_Id == data.MI_Id && a.SPCCPM_ActiveFlag == true
                               select new EventsSponsorDTO
                               {
                                   SPCCE_Id = b.SPCCE_Id,
                                   SPCCPM_Id = a.SPCCPM_Id,
                                   SPCCPM_Name = a.SPCCPM_Name
                               }
                               ).Distinct().ToArray();

                //_context.ProgramMasterDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCPM_ActiveFlag == true).Select(d => new EventsSponsorDTO { SPCCPM_Id = d.SPCCPM_Id, SPCCPM_Name = d.SPCCPM_Name }).ToArray();
                data.sponsorList = _context.MasterSponserDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSP_ActiveFlag == true).Select(d => new MasterSponserDTO
                {
                    SPCCMSP_Id = d.SPCCMSP_Id,
                    SPCCMSP_SponsorName = d.SPCCMSP_SponsorName
                }).ToArray();
                var sponsors = (from m in _context.EventsSponsorDMO
                                from n in _context.EventsMappingDMO
                                from o in _context.ProgramMasterDMO
                                from p in _context.MasterSponserDMO
                                where m.SPCCE_Id == n.SPCCE_Id && m.SPCCMSP_Id == p.SPCCMSP_Id && m.MI_Id == data.MI_Id /*&& n.SPCCPM_Id == o.SPCCPM_Id*/
                                select new EventsSponsorDTO
                                {
                                    SPCCESP_Id = m.SPCCESP_Id,
                                    SPCCPM_Name = o.SPCCPM_Name,
                                    sponsorName = p.SPCCMSP_SponsorName,
                                    SPCCESP_ActiveFlag = m.SPCCESP_ActiveFlag
                                }).ToList();
                if (sponsors.Count > 0)
                {
                    data.sponsormappingList = sponsors.ToArray();
                    data.count = sponsors.Count;
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
        public EventsSponsorDTO saveRecord(EventsSponsorDTO obj)
        {
            try
            {
                if (obj.SPCCESP_Id == 0)
                {
                    var checkduplicate = _context.EventsSponsorDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCE_Id == obj.SPCCE_Id && d.SPCCMSP_Id == obj.SPCCMSP_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<EventsSponsorDMO>(obj);
                        mapp.SPCCESP_ActiveFlag = true;
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
                else if (obj.SPCCESP_Id > 0)
                {
                    var checkduplicate = _context.EventsSponsorDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCE_Id == obj.SPCCE_Id && d.SPCCMSP_Id == obj.SPCCMSP_Id && d.SPCCESP_Id != obj.SPCCESP_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.EventsSponsorDMO.Where(d => d.SPCCESP_Id == obj.SPCCESP_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.EventsSponsorDMO.Single(d => d.SPCCESP_Id == obj.SPCCESP_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCE_Id = obj.SPCCE_Id;
                            update.SPCCMSP_Id = obj.SPCCMSP_Id;
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
        public EventsSponsorDTO EditDetails(int id)
        {
            EventsSponsorDTO resp = new EventsSponsorDTO();
            try
            {
                resp.editDetails = _context.EventsSponsorDMO.Where(d => d.SPCCESP_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public EventsSponsorDTO deactivate(EventsSponsorDTO obj)
        {
            try
            {
                var query = _context.EventsSponsorDMO.Where(d => d.SPCCESP_Id == obj.SPCCESP_Id).ToList();
                if (query.Count > 0)
                {
                    var update = _context.EventsSponsorDMO.Single(d => d.SPCCESP_Id == obj.SPCCESP_Id);
                    update.UpdatedDate = DateTime.Now;
                    update.SPCCESP_ActiveFlag = obj.SPCCESP_ActiveFlag;
                    _context.Update(update);
                    int s = _context.SaveChanges();
                    if (s > 0)
                    {
                        if (obj.SPCCESP_ActiveFlag == true)
                        {
                            obj.returnVal = "Record Activated Successfully";
                        }
                        else if (obj.SPCCESP_ActiveFlag == false)
                        {
                            obj.returnVal = "Record DeActivated Successfully";
                        }
                    }
                    else
                    {
                        obj.returnVal = "activationFailed";
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
