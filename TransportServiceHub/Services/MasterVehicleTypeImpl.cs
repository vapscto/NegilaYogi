using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
namespace TransportServiceHub.Services
{
    public class MasterVehicleTypeImpl : Interfaces.MasterVehicleTypeInterface
    {
        public TransportContext _context;
        ILogger<MasterVehicleTypeImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public MasterVehicleTypeImpl(ILogger<MasterVehicleTypeImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public MasterVehicleTypeDTO getdata(int id)
        {
            MasterVehicleTypeDTO data = new MasterVehicleTypeDTO();
            try
            {

                data.getmastervehicle = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == id).OrderByDescending(a => a.TRMVT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Vehicle getdata" + ex.Message);
            }


            return data;
        }
        public MasterVehicleTypeDTO savedata(MasterVehicleTypeDTO data)
        {
            try
            {
                if (data.TRMVT_Id > 0)
                {
                    var check_duplicate_areaname_update = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMVT_VehicleType.Equals(data.TRMVT_VehicleType) && a.TRMVT_Id != data.TRMVT_Id).ToList();
                    var check_duplicate_areaaliasname_update = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMVT_VehicleDesc.Equals(data.TRMVT_VehicleDesc) && a.TRMVT_Id != data.TRMVT_Id).ToList();

                    if (check_duplicate_areaname_update.Count == 0 && check_duplicate_areaaliasname_update.Count == 0)
                    {
                        var result = _context.MasterVehicleTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMVT_Id == data.TRMVT_Id);
                        result.TRMVT_VehicleDesc = data.TRMVT_VehicleDesc;
                        result.TRMVT_VehicleType = data.TRMVT_VehicleType;
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
                    var check_duplicate_areaname = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMVT_VehicleDesc.Equals(data.TRMVT_VehicleDesc)).ToList();
                    var check_duplicate_areaaliasname = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMVT_VehicleType.Equals(data.TRMVT_VehicleType)).ToList();

                    if (check_duplicate_areaaliasname.Count == 0 && check_duplicate_areaname.Count == 0)
                    {
                        MasterVehicleTypeDMO areadmo = new MasterVehicleTypeDMO();
                        areadmo.TRMVT_VehicleDesc = data.TRMVT_VehicleDesc;
                        areadmo.TRMVT_VehicleType = data.TRMVT_VehicleType;
                        areadmo.MI_Id = data.MI_Id;
                        areadmo.TRMVT_ActiveFlg = true;
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
                _areaimpl.LogInformation("Transport Error Master Vehicle savedata" + ex.Message);
            }
            return data;
        }

        public MasterVehicleTypeDTO geteditdata(MasterVehicleTypeDTO data)
        {
            try
            {
                data.geteditdatavehicle = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMVT_Id == data.TRMVT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Vehicle geteditdata " + ex.Message);
            }
            return data;
        }

        public MasterVehicleTypeDTO activedeactive(MasterVehicleTypeDTO data)
        {
            try
            {
                var chekused = (from a in _context.Master_VehicleDMO
                                from b in _context.MasterVehicleTypeDMO
                                where (a.TRMVT_Id == b.TRMVT_Id && b.TRMVT_Id == data.TRMVT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRMV_ActiveFlag == true)
                                select new MasterVehicleTypeDTO
                                {
                                    TRMVT_Id = a.TRMVT_Id
                                }).ToList();

                if (chekused.Count == 0)
                {
                    var result = _context.MasterVehicleTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMVT_Id == data.TRMVT_Id);
                    if (result.TRMVT_ActiveFlg == false)
                    {
                        result.TRMVT_ActiveFlg = true;
                    }
                    else
                    {
                        result.TRMVT_ActiveFlg = false;
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
                    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                }
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Vehicle activedeactive" + ex.Message);
            }
            return data;
        }
    }

}
