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
    public class MasterSponserImpl:Interfaces.MasterSponserInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterSponserImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterSponserDTO getDetails(MasterSponserDTO data)
        {
            try
            {
                var sponsers = _context.MasterSponserDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                if(sponsers.Count > 0)
                {
                    data.sponserList = sponsers.ToArray();
                    data.count = sponsers.Count;
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
        public MasterSponserDTO saveRecord(MasterSponserDTO obj)
        {
            try
            {
                if (obj.SPCCMSP_Id == 0)
                {
                    var checkduplicate = _context.MasterSponserDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSP_SponsorName.Equals(obj.SPCCMSP_SponsorName)).ToList();
                    if(checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterSponserDMO>(obj);
                        mapp.SPCCMSP_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        _context.Add(mapp);
                        int s= _context.SaveChanges();
                        if(s > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                }else if(obj.SPCCMSP_Id > 0)
                {
                    var checkduplicate = _context.MasterSponserDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSP_SponsorName.Equals(obj.SPCCMSP_SponsorName) && d.SPCCMSP_Id!=obj.SPCCMSP_Id).ToList();
                    if(checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterSponserDMO.Where(d => d.SPCCMSP_Id == obj.SPCCMSP_Id).ToList();
                        if(query.Count > 0)
                        {
                            var update = _context.MasterSponserDMO.Single(d => d.SPCCMSP_Id == obj.SPCCMSP_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCMSP_ContactNo = obj.SPCCMSP_ContactNo;
                            update.SPCCMSP_ContactPerson = obj.SPCCMSP_ContactPerson;
                            update.SPCCMSP_SponsorDetails = obj.SPCCMSP_SponsorDetails;
                            update.SPCCMSP_SponsorName = obj.SPCCMSP_SponsorName;
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

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public MasterSponserDTO EditDetails(int id)
        {
            MasterSponserDTO resp = new MasterSponserDTO();
            try
            {
                resp.editDetails = _context.MasterSponserDMO.Where(d => d.SPCCMSP_Id == id).ToArray();

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterSponserDTO deactivateSponser(MasterSponserDTO obj)
        {
            try
            {
               
                var result = _context.MasterSponserDMO.Single(t => t.SPCCMSP_Id == obj.SPCCMSP_Id && t.MI_Id == obj.MI_Id);

                if (result.SPCCMSP_ActiveFlag == true)
                {
                    result.SPCCMSP_ActiveFlag = false;
                }
                else if (result.SPCCMSP_ActiveFlag == false)
                {
                    result.SPCCMSP_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                int rowAffected = _context.SaveChanges();
                if (rowAffected > 0)
                {
                    obj.returnval = true;
                }
                else
                {
                    obj.returnval = false;
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}
