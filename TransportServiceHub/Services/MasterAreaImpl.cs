using DataAccessMsSqlServerProvider;
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
    public class MasterAreaImpl : Interfaces.MasterAreaInterface
    {
        public TransportContext _context;
        ILogger<MasterAreaImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public MasterAreaImpl(ILogger<MasterAreaImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public MasterAreaDTO getdata(int id)
        {
            MasterAreaDTO data = new MasterAreaDTO();
            try
            {
                data.getmasterarea = _context.MasterAreaDMO.Where(a => a.MI_Id == id).OrderByDescending(a=>a.TRMA_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public MasterAreaDTO savedata(MasterAreaDTO data)
        {
            try
            {
                if (data.TRMA_Id > 0)
                {
                    var check_duplicate_areaname_update = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AreaName.Equals(data.TRMA_AreaName) && a.TRMA_Id != data.TRMA_Id).ToList();
                    var check_duplicate_areaaliasname_update = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AliasName.Equals(data.TRMA_AliasName) && a.TRMA_Id != data.TRMA_Id).ToList();

                    if (check_duplicate_areaname_update.Count == 0 && check_duplicate_areaaliasname_update.Count == 0)
                    {
                        var result = _context.MasterAreaDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id);
                        result.TRMA_AreaName = data.TRMA_AreaName;
                        result.TRMA_AliasName = data.TRMA_AliasName;
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_duplicate_areaname = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AreaName.Equals(data.TRMA_AreaName)).ToList();
                    var check_duplicate_areaaliasname = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AliasName.Equals(data.TRMA_AliasName)).ToList();

                    if (check_duplicate_areaaliasname.Count == 0 && check_duplicate_areaname.Count == 0)
                    {
                        MasterAreaDMO areadmo = new MasterAreaDMO();
                        areadmo.TRMA_AreaName = data.TRMA_AreaName;
                        areadmo.TRMA_AliasName = data.TRMA_AliasName;
                        areadmo.MI_Id = data.MI_Id;
                        areadmo.TRMA_ActiveFlg = true;
                        areadmo.CreatedDate = DateTime.Now;
                        areadmo.UpdatedDate = DateTime.Now;

                        _context.Add(areadmo);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrval = true;
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
                _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }

        public MasterAreaDTO geteditdata(MasterAreaDTO data)
        {
            try
            {
                data.geteditdataarea = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public MasterAreaDTO activedeactive(MasterAreaDTO data)
        {
            try
            {
                var check_Area_Used = (from a in _context.MasterRouteDMO
                                       from b in _context.MasterAreaDMO
                                       where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMA_ActiveFlg == true && a.TRMA_Id==data.TRMA_Id && a.TRMR_ActiveFlg==true)
                                       select new MasterAreaDTO
                                       {
                                           TRMA_Id = a.TRMA_Id
                                       }).ToList();                  
                    
                 
                if (check_Area_Used.Count > 0)
                {
                    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                }
                else
                {
                    var result = _context.MasterAreaDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMA_Id == data.TRMA_Id);
                    if (result.TRMA_ActiveFlg == false)
                    {
                        result.TRMA_ActiveFlg = true;
                    }
                    else
                    {
                        result.TRMA_ActiveFlg = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }
    }
}
