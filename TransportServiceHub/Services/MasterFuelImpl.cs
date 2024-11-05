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
    public class MasterFuelImpl : Interfaces.MasterFuelInterface
    {
        public TransportContext _context;
        ILogger<MasterFuelImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public MasterFuelImpl(ILogger<MasterFuelImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public MasterFuelDTO getdata(int id)
        {
            MasterFuelDTO data = new MasterFuelDTO();
           
            try
            {
                data.getmasterfuel = _context.MasterFuelDMO.Where(a => a.MI_Id == id).OrderByDescending(a => a.TRMFT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Fuel getdata" + ex.Message);
            }
            return data;
           
        }
        public MasterFuelDTO savedata(MasterFuelDTO data)
        {
            try
            {
                if (data.TRMFT_Id > 0)
                {
                    var check_duplicate_areaname_update = _context.MasterFuelDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMFT_FuelType.Equals(data.TRMFT_FuelType) && a.TRMFT_Id != data.TRMFT_Id).ToList();
                  

                    if (check_duplicate_areaname_update.Count == 0)
                    {
                        var result = _context.MasterFuelDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMFT_Id == data.TRMFT_Id);
                        result.TRMFT_FuelType = data.TRMFT_FuelType;
                      
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
                    var check_duplicate_areaname = _context.MasterFuelDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMFT_FuelType.Equals(data.TRMFT_FuelType)).ToList();
                   
                    if (check_duplicate_areaname.Count == 0 )
                    {
                        MasterFuelDMO areadmo = new MasterFuelDMO();
                        areadmo.TRMFT_FuelType = data.TRMFT_FuelType;
                       
                        areadmo.MI_Id = data.MI_Id;
                        areadmo.TRMFT_ActiveFlg = true;
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
                _areaimpl.LogInformation("Transport Error Master Fuel savedata" + ex.Message);
            }
            return data;
        }

        public MasterFuelDTO geteditdata(MasterFuelDTO data)
        {
            try
            {
                data.geteditdatafuel = _context.MasterFuelDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMFT_Id == data.TRMFT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Fuel geteditdata " + ex.Message);
            }
            return data;
        }

        public MasterFuelDTO activedeactive(MasterFuelDTO data)
        {
            try
            {
                var check_Area_Used = (from a in _context.MasterFuelDMO
                                       from b in _context.Master_VehicleDMO
                                       where (a.TRMFT_Id==b.TRMFT_Id && a.TRMFT_Id == data.TRMFT_Id && a.MI_Id == data.MI_Id && b.TRMV_ActiveFlag == true && a.TRMFT_ActiveFlg == true)
                                       select new MasterFuelDTO
                                       {
                                           TRMFT_Id = a.TRMFT_Id
                                       }).ToList();
                if (check_Area_Used.Count == 0)
                {
                    var result = _context.MasterFuelDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMFT_Id == data.TRMFT_Id);
                    if (result.TRMFT_ActiveFlg == false)
                    {
                        result.TRMFT_ActiveFlg = true;
                    }
                    else
                    {
                        result.TRMFT_ActiveFlg = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrval = true;
                    }
                }
                else
                {
                    data.message = "You Can Not Deactive This Record Its Already Mapped";
                }
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Fuel activedeactive" + ex.Message);
            }
            return data;
        }
    }
}


