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
    public class RouteLocationMappingImpl : Interfaces.RouteLocationMappingInterface
    {
        public TransportContext _context;
        public ILogger<RouteLocationMappingDTO> _log;

        public RouteLocationMappingImpl(TransportContext context, ILogger<RouteLocationMappingDTO> log)
        {
            _context = context;
            _log = log;
        }
        public RouteLocationMappingDTO getdata(int id)
        {
            RouteLocationMappingDTO data = new RouteLocationMappingDTO();
            try
            {
                data.routedetailsarea = _context.MasterAreaDMO.Where(a => a.MI_Id == id && a.TRMA_ActiveFlg == true).ToArray();
                data.locationdetails = _context.MasterLocationDMO.Where(a => a.MI_Id == id && a.TRML_ActiveFlg == true).ToArray();
                data.getdetails = (from a in _context.Route_Location
                                   from b in _context.MasterRouteDMO
                                   from c in _context.MasterLocationDMO
                                   where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == id)
                                   select new RouteLocationMappingDTO
                                   {
                                       TRMRL_Id = a.TRMRL_Id,
                                       routename = b.TRMR_RouteName,
                                       locationname = c.TRML_LocationName,
                                       TRML_Id = c.TRML_Id,
                                       TRMR_Id = b.TRMR_Id,
                                       TRMRL_ActiveFlag = a.TRMRL_ActiveFlag,
                                       TRMRL_Order = a.TRMRL_Order,
                                       TRMR_order = b.TRMR_order,

                                   }).Distinct().OrderByDescending(a => a.TRMR_order).ThenBy(f=>f.TRMRL_Order).Distinct().ToArray();

                data.getdetails1 = (from a in _context.Route_Location
                                    from b in _context.MasterRouteDMO
                                    from c in _context.MasterLocationDMO
                                    where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == id)
                                    select new RouteLocationMappingDTO
                                    {
                                        TRMRL_Id = a.TRMRL_Id,
                                        routename = b.TRMR_RouteName,
                                        locationname = c.TRML_LocationName,
                                        TRML_Id = c.TRML_Id,
                                        TRMR_Id = b.TRMR_Id,
                                        TRMRL_ActiveFlag = a.TRMRL_ActiveFlag,
                                         TRMRL_Order = a.TRMRL_Order,
                                        TRMR_order = b.TRMR_order,
                                    }).Distinct().OrderBy(a => a.TRMR_order).ThenBy(f=>f.TRMRL_Order).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Route Location Mapping Error Getdata" + ex.Message);
            }

            return data;
        }

        public RouteLocationMappingDTO savedata(RouteLocationMappingDTO data)
        {

            try
            {
                if (data.TRMRL_Id > 0)
                {

                }
                else
                {

                    for (int k = 0; k < data.selectedlocations.Length; k++)
                    {
                        Route_Location routlo = new Route_Location();
                        var checkduplicate = _context.Route_Location.Where(a => a.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id
                        && a.TRML_Id == data.selectedlocations[k].TRML_Id).ToList();
                        if (checkduplicate.Count == 0)
                        {
                            routlo.TRML_Id = data.selectedlocations[k].TRML_Id;
                            routlo.TRMR_Id = data.TRMR_Id;
                            routlo.MI_Id = data.MI_Id;
                            routlo.CreatedDate = DateTime.Now;
                            routlo.UpdatedDate = DateTime.Now;
                            routlo.TRMRL_ActiveFlag = true;
                            _context.Add(routlo);
                        }
                        else
                        {
                            data.msg = "Duplicate";
                        }
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                        data.message = "Save";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Save";
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error in savedata Routeloaction mapping " + ex.Message);
            }
            return data;
        }

        public RouteLocationMappingDTO getlocations(RouteLocationMappingDTO data)
        {
            try
            {
                List<long> GrpId = new List<long>();

                data.selectedroute = (from a in _context.Route_Location
                                      where (a.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id)
                                      select new selected_route
                                      {
                                          TRML_Id = a.TRML_Id
                                      }).Distinct().ToArray();

                foreach (var item in data.selectedroute)
                {
                    GrpId.Add(item.TRML_Id);
                }


                data.locationdetails = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && !GrpId.Contains(a.TRML_Id) && a.TRML_ActiveFlg == true).ToArray();


            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error in Route location getlocations " + ex.Message);
            }
            return data;
        }
        public RouteLocationMappingDTO activedeactive(RouteLocationMappingDTO data)
        {
            try
            {
                var checkid = _context.Route_Location.Single(a => a.MI_Id == data.MI_Id && a.TRMRL_Id == data.TRMRL_Id);
                if (checkid.TRMRL_ActiveFlag == true)
                {
                    checkid.TRMRL_ActiveFlag = false;
                }
                else
                {
                    checkid.TRMRL_ActiveFlag = true;
                }
                checkid.CreatedDate = DateTime.Now;
                _context.Update(checkid);
                var i = _context.SaveChanges();
                if (i > 0)
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
                data.returnval = false;
                _log.LogInformation("Transport Error Activedeactive Route Location Mapping " + ex.Message);
            }
            return data;
        }
        public RouteLocationMappingDTO getOrder(RouteLocationMappingDTO data)
        {
            try
            {
                int id = 0;
                Route_Location order = new Route_Location();
                for (int i = 0; i < data.selectedorder.Count(); i++)
                {
                    var reult = _context.Route_Location.Single(t => t.MI_Id == data.MI_Id && t.TRMRL_Id == data.selectedorder[i].TRMRL_Id);
                    id = id + 1;
                    reult.TRMRL_Order = id;
                    reult.UpdatedDate = DateTime.Now;
                    _context.Update(reult);
                }
                var idd = _context.SaveChanges();
                if (idd > 0)
                {
                    data.returnval = true;
                    data.message = "Update";
                }
                else
                {
                    data.returnval = false;
                    data.message = "Update";
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in Transport route location getorder " + ex.Message);
            }
            return data;
        }

        public RouteLocationMappingDTO getlocationsarea(RouteLocationMappingDTO data)
        {
            try
            {
                //data.routedetails = (from a in _context.MasterRouteDMO
                //                     from b in _context.MasterAreaDMO
                //                     where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg == true && a.TRMA_Id == data.TRMA_Id)
                //                     select new RouteLocationMappingDTO
                //                     {
                //                         TRMR_Id = a.TRMR_Id,
                //                         //trmR_RouteName = a.TRMR_RouteName +':'+a.TRMR_RouteNo
                //                         trmR_RouteName = ((a.TRMR_RouteNo == null ? " " : a.TRMR_RouteNo) + ':' +a.TRMR_RouteName  ).Trim(),
                //                         TRMR_order = a.TRMR_order

                //                     }).OrderBy(r=>r.TRMR_order).ToArray();

                data.routedetails = (from a in _context.MasterRouteDMO
                                     from b in _context.MasterAreaDMO
                                     from c in _context.MasterRouteAreaMappingDMO
                                     where (a.TRMR_Id == c.TRMR_Id && b.TRMA_Id == c.TRMA_Id && a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg == true && c.TRMA_Id == data.TRMA_Id && c.TRAR_ActiveFlg == true)
                                     select new RouteLocationMappingDTO
                                     {
                                         TRMR_Id = a.TRMR_Id,
                                         //trmR_RouteName = a.TRMR_RouteName +':'+a.TRMR_RouteNo
                                         trmR_RouteName = ((a.TRMR_RouteNo == null ? " " : a.TRMR_RouteNo) + ':' + a.TRMR_RouteName).Trim(),
                                         TRMR_order = a.TRMR_order

                                     }).OrderBy(r => r.TRMR_order).ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in Transport route location getorder " + ex.Message);
            }
            return data;
        }

    }
}
