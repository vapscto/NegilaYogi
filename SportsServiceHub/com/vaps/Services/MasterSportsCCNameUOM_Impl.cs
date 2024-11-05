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
    public class MasterSportsCCNameUOM_Impl:Interfaces.MasterSportsCCNameUOMInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterSportsCCNameUOM_Impl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterSportsCCNameUMO_DTO getDetails(MasterSportsCCNameUMO_DTO data)
        {
            try
            {
                var sportsccName = _context.MasterSportsCCNameDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCC_ActiveFlag == true).Select(d=>new MasterSportsCCNameDTO {SPCCMSCC_Id=d.SPCCMSCC_Id,SPCCMSCC_SportsCCName=d.SPCCMSCC_SportsCCName }).ToList();
                if(sportsccName.Count > 0)
                {
                    data.sportsCCNameList = sportsccName.ToArray();
                }
                var uom = _context.SportMasterUOMDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMUOM_ActiveFlag == true).Select(d => new SportMasterUOMDTO { SPCCMUOM_Id = d.SPCCMUOM_Id, SPCCMUOM_UOMName = d.SPCCMUOM_UOMName }).ToList();
                if (uom.Count > 0)
                {
                    data.uomList = uom.ToArray();
                }
                var sportsccUom = (from m in _context.MasterSportsCCNameUOM_DMO
                                   from n in _context.MasterSportsCCNameDMO
                                   from o in _context.SportMasterUOMDMO
                                   where m.SPCCMSCC_Id == n.SPCCMSCC_Id && m.SPCCMUOM_Id == o.SPCCMUOM_Id
                                   && m.MI_Id == data.MI_Id
                                   select new MasterSportsCCNameUMO_DTO
                                   {
                                       SPCCMSCCUOM_Id=m.SPCCMSCCUOM_Id,
                                       sportsCCName=n.SPCCMSCC_SportsCCName,
                                       uomName=o.SPCCMUOM_UOMName,
                                       SPCCMSCCUOM_ActiveFlag=m.SPCCMSCCUOM_ActiveFlag,
                                       SPCCMSCC_Id=m.SPCCMSCC_Id,
                                       SPCCMUOM_Id=m.SPCCMUOM_Id
                                   }
                                   ).ToList();
                if (sportsccUom.Count > 0)
                {
                    data.sportsCCNameUOMList = sportsccUom.ToArray();
                    data.count = sportsccUom.Count;
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
        public MasterSportsCCNameUMO_DTO saveRecord(MasterSportsCCNameUMO_DTO obj)
        {
            try
            {
                if (obj.SPCCMSCCUOM_Id == 0)
                {
                    var checkduplicate = _context.MasterSportsCCNameUOM_DMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCC_Id==obj.SPCCMSCC_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterSportsCCNameUOM_DMO>(obj);
                        mapp.SPCCMSCCUOM_ActiveFlag = true;
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
                else if (obj.SPCCMSCCUOM_Id > 0)
                {
                    var checkduplicate = _context.MasterSportsCCNameUOM_DMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCC_Id == obj.SPCCMSCC_Id && d.SPCCMSCCUOM_Id != obj.SPCCMSCCUOM_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterSportsCCNameUOM_DMO.Where(d => d.SPCCMSCCUOM_Id == obj.SPCCMSCCUOM_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.MasterSportsCCNameUOM_DMO.Single(d => d.SPCCMSCCUOM_Id == obj.SPCCMSCCUOM_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCMSCC_Id = obj.SPCCMSCC_Id;
                            update.SPCCMUOM_Id = obj.SPCCMUOM_Id;
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
        public MasterSportsCCNameUMO_DTO EditDetails(int id)
        {
            MasterSportsCCNameUMO_DTO resp = new MasterSportsCCNameUMO_DTO();
            try
            {
                resp.editDetails = _context.MasterSportsCCNameUOM_DMO.Where(d => d.SPCCMSCCUOM_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterSportsCCNameUMO_DTO deactivate(MasterSportsCCNameUMO_DTO obj)
        {
            try
            {
                var query = _context.MasterSportsCCNameUOM_DMO.Single(d => d.SPCCMSCCUOM_Id == obj.SPCCMSCCUOM_Id);
                if (query.SPCCMSCCUOM_ActiveFlag == true)
                {
                    query.SPCCMSCCUOM_ActiveFlag = false;
                }
                else if (query.SPCCMSCCUOM_ActiveFlag == false)
                {
                    query.SPCCMSCCUOM_ActiveFlag = true;
                }

                query.UpdatedDate = DateTime.Now;               
                _context.Update(query);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    obj.retval = true;
                }
                else
                {
                    obj.retval = false;
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
