using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class MasterLocationImpl : Interfaces.MasterLocationInterface
    {
        public TransportContext _context;
        ILogger<MasterLocationImpl> _logloc;

        public MasterLocationImpl(ILogger<MasterLocationImpl> log, TransportContext context)
        {
            _context = context;
            _logloc = log;
        }
        public MasterLocationDTO getdata(int id)
        {

            MasterLocationDTO data = new MasterLocationDTO();
            try
            {
                data.getlocationareadata = (from a in _context.MasterLocationDMO                                            
                                            where (a.MI_Id == id)
                                            select new MasterLocationDTO
                                            {
                                                TRML_Latitude = a.TRML_Latitude,
                                                TRML_Longitude = a.TRML_Longitude,
                                                TRML_ActiveFlg = a.TRML_ActiveFlg,
                                                TRML_Id = a.TRML_Id,                                               
                                                TRML_LocationName=a.TRML_LocationName
                                            }).OrderByDescending(a => a.TRML_Id).ToArray();

                //data.getzonearea = _context.MasterAreaDMO.Where(a => a.MI_Id == id && a.TRMA_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                _logloc.LogInformation("Transport Errror Master Location getdata" + ex.Message);
            }
            return data;
        }

        public MasterLocationDTO savedata(MasterLocationDTO data)
        {
            try
            {
                if (data.TRML_Id > 0)
                {
                    //var check_it_used = (from a in _context.Route_Location
                    //                     from b in _context.MasterLocationDMO
                    //                     where (a.TRML_Id == b.TRML_Id && a.MI_Id == data.MI_Id && b.TRML_Id == data.TRML_Id
                    //                     )
                    //                     select new MasterLocationDTO
                    //                     {
                    //                         TRML_Id = a.TRML_Id
                    //                     }).ToList();
                    //if (check_it_used.Count > 0)
                    //{
                    //    data.message = "Mapped";
                    //}
                    //else
                    //{
                        //var check_location_duplicate = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_Longitude == data.TRML_Longitude && a.TRML_Id !=data.TRML_Id).ToList();
                        //var check_location_duplicate1 = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_Latitude == data.TRML_Latitude && a.TRML_Id != data.TRML_Id).ToList();
                        var check_location_duplicate2 = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_LocationName == data.TRML_LocationName && a.TRML_Id != data.TRML_Id).ToList();
                        if (check_location_duplicate2.Count==0)
                        {
                            var result = _context.MasterLocationDMO.Single(a => a.MI_Id == data.MI_Id && a.TRML_Id == data.TRML_Id);
                            result.TRML_Longitude = data.TRML_Longitude;
                            result.TRML_Latitude = data.TRML_Latitude;                            
                            result.TRML_LocationName = data.TRML_LocationName;
                            result.UpdatedDate = DateTime.Now;
                            _context.Update(result);
                            int n = _context.SaveChanges();
                            if (n > 0)
                            {
                                data.message = "Update";
                                data.retrunval = true;
                            }
                            else
                            {
                                data.message = "Update";
                                data.retrunval = false;
                            }
                        }
                        else
                        {
                            data.message = "Duplicate";
                        }                        
                    //}
                }
                else
                {
                    MasterLocationDMO loc = new MasterLocationDMO();

                    //var check_location_duplicate = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_Longitude == data.TRML_Longitude).ToList();
                    //var check_location_duplicate1 = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_Latitude == data.TRML_Latitude).ToList();
                    var check_location_duplicate2 = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_LocationName == data.TRML_LocationName).ToList();
                    if (check_location_duplicate2.Count==0)
                    {
                        loc.MI_Id = data.MI_Id;
                        loc.TRML_Longitude = data.TRML_Longitude;
                        loc.TRML_Latitude = data.TRML_Latitude;                        
                        loc.TRML_ActiveFlg = true;
                        loc.TRML_LocationName = data.TRML_LocationName;
                        loc.CreatedDate = DateTime.Now;
                        loc.UpdatedDate = DateTime.Now;
                        _context.Add(loc);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                _logloc.LogInformation("Transport Error Master Location savedata" + ex.Message);
            }
            return data;
        }

        public MasterLocationDTO activedeactive(MasterLocationDTO data)
        {
            try
            {
                var check_it_used = (from a in _context.Route_Location
                                     from b in _context.MasterLocationDMO
                                     where (a.TRML_Id == b.TRML_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRML_Id==data.TRML_Id && b.TRML_ActiveFlg == true && a.TRMRL_ActiveFlag==true)
                                     select new MasterLocationDTO
                                     {
                                         TRML_Id = a.TRML_Id
                                     }).ToList();
                if (check_it_used.Count > 0)
                {
                    data.message = "You Can Not Deactive This Record It Already Mapped";
                }
                else
                {
                    var result = _context.MasterLocationDMO.Single(a => a.MI_Id == data.MI_Id && a.TRML_Id == data.TRML_Id);
                    if (result.TRML_ActiveFlg == true)
                    {
                        result.TRML_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRML_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrunval = true;
                    }
                    else
                    {
                        data.retrunval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logloc.LogInformation("Transport Error Master Location activedeactive" + ex.Message);
            }
            return data;
        }

        public MasterLocationDTO edit(MasterLocationDTO data)
        {
            try
            {
                data.geteditdata = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_Id == data.TRML_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logloc.LogInformation("Transport Error Master Location edit" + ex.Message);
            }
            return data;
        }
    }
}
