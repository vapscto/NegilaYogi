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
    public class MasterRouteImpl : Interfaces.MasterRouteInterface
    {
        public TransportContext _context;
        public ILogger<MasterRouteImpl> _log;
        public MasterRouteImpl(ILogger<MasterRouteImpl> log, TransportContext context)
        {
            _context = context;
            _log = log;
        }
        public MasterRouteDTO getdata(int id)
        {
            MasterRouteDTO data = new MasterRouteDTO();
            try
            {
                data.getroutemater = (from a in _context.MasterRouteDMO
                                      where ( a.MI_Id == id)
                                      select new MasterRouteDTO
                                      {
                                          TRMR_Id = a.TRMR_Id,
                                          TRMR_RouteName = a.TRMR_RouteName,
                                          TRMR_RouteDesc = a.TRMR_RouteDesc,
                                          TRMR_ActiveFlg = a.TRMR_ActiveFlg,
                                          TRMR_RouteNo = a.TRMR_RouteNo,
                                          TRMR_order = a.TRMR_order
                                      }).OrderByDescending(a => a.TRMR_Id).ToArray();

                //data.getroutemater = _context.MasterRouteDMO.Where(a => a.MI_Id == id).OrderByDescending(a=>a.TRMR_Id).ToArray();
                data.getzonearea = _context.MasterAreaDMO.Where(a => a.MI_Id == id && a.TRMA_ActiveFlg == true).ToArray();

                data.routedata = _context.MasterRouteDMO.Where(t => t.MI_Id == id && t.TRMR_ActiveFlg == true).ToArray();

                data.routearea = (from a in _context.MasterRouteAreaMappingDMO
                                  from b in _context.MasterRouteDMO
                                  from c in _context.MasterAreaDMO
                                  where (a.TRMR_Id == b.TRMR_Id && a.TRMA_Id == c.TRMA_Id && b.MI_Id == c.MI_Id && b.MI_Id == id)
                                  select new MasterRouteDTO
                                  {
                                      TRAR_Id = a.TRAR_Id,
                                      TRMR_RouteName = b.TRMR_RouteName,
                                      TRMR_Id = b.TRMR_Id,
                                      TRMA_AreaName = c.TRMA_AreaName,
                                      TRMA_Id = c.TRMA_Id,
                                      TRAR_ActiveFlg = a.TRAR_ActiveFlg
                                  }).ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Route getdata" + ex.Message);
            }
            return data;
        }

        public MasterRouteDTO savedata(MasterRouteDTO data)
        {
            try
            {
                if (data.TRMR_Id > 0)
                {
                    //var check_usedoenot = (from a in _context.Route_Location
                    //                       from b in _context.MasterRouteDMO
                    //                       where (a.TRMR_Id == b.TRMR_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id)
                    //                       select new MasterRouteDTO
                    //                       {
                    //                           TRMR_Id = a.TRMR_Id
                    //                       }).ToList();

                    //var check_useornot = (from a in _context.VehicleRouteDMo
                    //                      from b in _context.MasterRouteDMO
                    //                      where (a.TRMR_Id == b.TRMR_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id)
                    //                      select new MasterRouteDTO
                    //                      {
                    //                          TRMR_Id = a.TRMR_Id
                    //                      }).ToList();

                 

                    //if (check_usedoenot.Count == 0 && check_useornot.Count == 0)
                    //{

                        var checkrouename_duplicate = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_RouteName.Equals(data.TRMR_RouteName) && a.TRMR_Id != data.TRMR_Id ).ToList();


                        var checkroueno_duplicate = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_RouteNo.Equals(data.TRMR_RouteNo) && a.TRMR_Id != data.TRMR_Id ).ToList();

                        var checkroute_duplicate = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_order.Equals(data.TRMR_order) && a.TRMR_Id != data.TRMR_Id ).ToList();

                        if (checkrouename_duplicate.Count == 0 && checkroueno_duplicate.Count == 0 && checkroute_duplicate.Count == 0)
                        {
                            var result = _context.MasterRouteDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id);
                            result.TRMA_Id = data.TRMA_Id;
                            result.TRMR_RouteName = data.TRMR_RouteName;
                            result.TRMR_RouteNo = data.TRMR_RouteNo;
                            result.TRMR_RouteDesc = data.TRMR_RouteDesc;
                            result.TRMR_order = data.TRMR_order;

                            result.UpdatedDate = DateTime.Now;
                            _context.Update(result);
                            int n = _context.SaveChanges();
                            if (n > 0)
                            {
                                data.message = "Update";
                                data.returnval = true;
                            }
                            else
                            {
                                data.message = "Update";
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            data.message = "Duplicate";
                        }
                    //}
                    //else
                    //{
                    //    data.message = "Mapped";
                    //}
                }
                else
                {
                    MasterRouteDMO route = new MasterRouteDMO();

                    var checkrouename_duplicate = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_RouteName.Equals(data.TRMR_RouteName) ).ToList();

                    var checkroueno_duplicate = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_RouteNo.Equals(data.TRMR_RouteNo) ).ToList();


                    var checkorder_duplicate = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_order.Equals(data.TRMR_order) ).ToList();

                    if (checkrouename_duplicate.Count == 0 && checkroueno_duplicate.Count == 0 && checkorder_duplicate.Count == 0)
                    {
                        route.MI_Id = data.MI_Id;
                        route.TRMA_Id = data.TRMA_Id;
                        route.TRMR_RouteNo = data.TRMR_RouteNo;
                        route.TRMR_RouteName = data.TRMR_RouteName;
                        route.TRMR_RouteDesc = data.TRMR_RouteDesc;
                        route.TRMR_order = data.TRMR_order;
                        route.TRMR_ActiveFlg = true;
                        route.CreatedDate = DateTime.Now;
                        route.UpdatedDate = DateTime.Now;
                        _context.Add(route);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
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
                _log.LogInformation("Transport Error Master Route savedata" + ex.Message);
            }
            return data;
        }

        public MasterRouteDTO edit(MasterRouteDTO data)
        {
            try
            {
                data.geteditdata = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Route edit" + ex.Message);
            }
            return data;
        }

        public MasterRouteDTO activedeactive(MasterRouteDTO data)
        {
            try
            {
                var check_usedoenot = (from a in _context.Route_Location
                                       from b in _context.MasterRouteDMO
                                       where (a.TRMR_Id == b.TRMR_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id && a.TRMRL_ActiveFlag==true)
                                       select new MasterRouteDTO
                                       {
                                           TRMR_Id = a.TRMR_Id
                                       }).ToList();

                var check_useornot = (from a in _context.VehicleRouteDMo
                                      from b in _context.MasterRouteDMO
                                      where (a.TRMR_Id == b.TRMR_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id && a.TRVR_ActiveFlg==true)
                                      select new MasterRouteDTO
                                      {
                                          TRMR_Id = a.TRMR_Id
                                      }).ToList();

                var studentmap = (from t in _context.TR_Student_RouteDMO
                                 where (t.MI_Id == data.MI_Id && t.TRSR_ActiveFlg == true && (t.TRMR_Id == data.TRMR_Id || t.TRMR_Drop_Route == data.TRMR_Id))
                  select new MasterRouteDTO
                  {
                      TRMR_Id = t.TRSR_Id
                  }).ToList();

                if (check_usedoenot.Count == 0 && check_useornot.Count == 0 && studentmap.Count==0)
                {

                    var result = _context.MasterRouteDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id);
                    if (result.TRMR_ActiveFlg == true)
                    {
                        result.TRMR_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRMR_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.message = "You Can't Deactive This Record, Its Already Mapped";
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Route activedeactive" + ex.Message);
            }
            return data;
        }
        public MasterRouteDTO getstudentlistre(MasterRouteDTO data)
        {
            try
            {
                data.areawisedetails = (from a in _context.MasterRouteDMO
                                        from b in _context.MasterAreaDMO
                                        where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id)
                                        select new MasterRouteDTO
                                        {
                                            TRMR_Id = a.TRMR_Id,
                                            TRMA_Id = a.TRMA_Id,
                                            TRMR_RouteName = a.TRMR_RouteName,
                                            TRMR_RouteDesc = a.TRMR_RouteDesc,
                                            TRMR_ActiveFlg = a.TRMR_ActiveFlg,
                                            TRMA_AreaName = b.TRMA_AreaName,
                                            TRMR_RouteNo = a.TRMR_RouteNo,
                                            TRMR_order = a.TRMR_order
                                        }).OrderBy(a => a.TRMR_order).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Route getstudentlistre" + ex.Message);
            }
            return data;
        }


        public MasterRouteDTO saveroutearea(MasterRouteDTO data)
        {
            try
            {
                if (data.TRAR_Id > 0)
                {
           
                    var checkrouename_duplicate = _context.MasterRouteAreaMappingDMO.Where(a => a.TRAR_Id == data.TRAR_Id && a.TRMR_Id == data.TRMR_Id && a.TRMA_Id == data.TRMA_Id  && a.TRAR_ActiveFlg == true).ToList();

                    if (checkrouename_duplicate.Count == 0 )
                    {
                        var result = _context.MasterRouteAreaMappingDMO.Single(a => a.TRAR_Id == data.TRAR_Id);
                        result.TRMR_Id = data.TRMR_Id;
                        result.TRMA_Id = data.TRMA_Id;

                        result.TRAR_UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                    //}
                    //else
                    //{
                    //    data.message = "Mapped";
                    //}
                }
                else
                {
                    

                    for (int i = 0; i < data.arealistarr.Length; i++)
                    {

                        var checkrouename_duplicate = _context.MasterRouteAreaMappingDMO.Where(a => a.TRMR_Id == data.TRMR_Id && a.TRMA_Id == data.arealistarr[i].TRMA_Id && a.TRAR_ActiveFlg == true).ToList();


                        if (checkrouename_duplicate.Count == 0)
                        {
                            MasterRouteAreaMappingDMO route = new MasterRouteAreaMappingDMO();
                            route.TRMR_Id = data.TRMR_Id;
                            route.TRMA_Id = data.arealistarr[i].TRMA_Id;
                            route.TRAR_ActiveFlg = true;
                            route.TRAR_CreatedDate = DateTime.Now;
                            route.TRAR_UpdatedDate = DateTime.Now;
                            _context.Add(route);

                        }
                    }

                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Route savedata" + ex.Message);
            }
            return data;
        }

        public MasterRouteDTO activedeactiveroutearea(MasterRouteDTO data)
        {
            try
            {
                    var result = _context.MasterRouteAreaMappingDMO.Single(a => a.TRAR_Id == data.TRAR_Id);
                    if (result.TRAR_ActiveFlg == true)
                    {
                        result.TRAR_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRAR_ActiveFlg = true;
                    }
                    result.TRAR_UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
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
                _log.LogInformation("Transport Error Master Route Area Mapping activedeactive" + ex.Message);
            }
            return data;
        }

        public MasterRouteDTO saveorder(MasterRouteDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.temp_masterroute.Count(); i++)
                {
                    var reult = _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == data.temp_masterroute[i].TRMR_Id && t.TRMA_Id == data.temp_masterroute[i].TRMA_Id);
                    id = id + 1;

                    if (i == 0)
                    {
                        reult.TRMR_order = id;
                    }
                    else
                    {
                        reult.TRMR_order = id;
                    }
                    _context.Update(reult);
                    var flag = _context.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Route saveorder" + ex.Message);
            }
            return data;
        }

    }
}
