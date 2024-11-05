using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace TransportServiceHub.Services
{
    public class TransportReportImpl : Interfaces.TransportReportInterface
    {
        private static ConcurrentDictionary<string, TransportReportDTO> _login =
          new ConcurrentDictionary<string, TransportReportDTO>();

        public TransportContext _context;
        ILogger<TransportReportImpl> _areaimpl;
        public TransportReportImpl(ILogger<TransportReportImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }
        public TransportReportDTO Getreportdetails(TransportReportDTO data)
        {
            try
            {

                if (data.onclickloaddata == "All")
                {

                    if (data.regorname_m == "masterarea")
                    {
                        data.messagelist = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_ActiveFlg==true).OrderByDescending(a => a.TRMA_Id).ToArray();
                    }

                    else if (data.regorname_m == "masterdriver")
                    {
                        data.messagelist = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_ActiveFlg==true).OrderByDescending(a => a.TRMD_Id).ToArray();
                    }

                    else if (data.regorname_m == "masterfueltype")
                    {
                        data.messagelist = _context.MasterFuelDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMFT_ActiveFlg==true).OrderByDescending(a => a.TRMFT_Id).ToArray();
                    }

                    else if (data.regorname_m == "masterlocation")
                    {
                        data.messagelist = _context.MasterLocationDMO.Where(a => a.MI_Id == data.MI_Id && a.TRML_ActiveFlg==true).OrderByDescending(a => a.TRML_Id).ToArray();
                    }

                    else if (data.regorname_m == "masterroute")
                    {
                        data.messagelist = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg==true).OrderBy(a => a.TRMR_order).ToArray();
                    }

                    else if (data.regorname_m == "mastervehical")
                    {
                        data.messagelist = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_ActiveFlag==true).OrderByDescending(a => a.TRMV_Id).ToArray();
                    }

                    else if (data.regorname_m == "mastervehicaltype")
                    {
                        data.messagelist = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMVT_ActiveFlg == true && a.TRMVT_ActiveFlg==true).ToArray();
                    }

                    else if (data.regorname_m == "mastersession")
                    {
                        data.messagelist = _context.MsterSessionDMO.Where(a => a.MI_Id == data.MI_Id &&a.TRMS_ActiveFlg==true).OrderByDescending(a => a.TRMS_Id).ToArray();
                    }


                    else if (data.regorname_m == "masterrouteschedule")
                    {
                        //data.messagelist = (from a in _context.TR_Route_ScheduleDMO
                        //                    from b in _context.MasterRouteDMO
                        //                    where (a.TRMR_Id == b.TRMR_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRRSC_ActiveFlag==true && b.TRMR_ActiveFlg==true)
                        //                    select new TransportReportDTO
                        //                    {
                        //                        TRRSC_Id = a.TRRSC_Id,
                        //                        TRRSC_ScheduleName = a.TRRSC_ScheduleName,
                        //                        TRRSC_Date = a.TRRSC_Date,
                        //                        TRMR_RouteName = b.TRMR_RouteName,
                        //                        TRRSC_ActiveFlag = a.TRRSC_ActiveFlag
                        //                    }).OrderByDescending(a => a.TRRSC_Id).ToArray();

                        data.messagelist = (from a in _context.TR_Route_ScheduleDMO
                                            where ( a.MI_Id == data.MI_Id && a.TRRSC_ActiveFlag == true )
                                            select new TransportReportDTO
                                            {
                                                TRRSC_Id = a.TRRSC_Id,
                                                TRRSC_ScheduleName = a.TRRSC_ScheduleName,
                                               // TRRSC_Date = a.TRRSC_Date,
                                               // TRMR_RouteName = b.TRMR_RouteName,
                                                TRRSC_ActiveFlag = a.TRRSC_ActiveFlag
                                            }).OrderByDescending(a => a.TRRSC_Id).ToArray();
                    }

                }


                else if (data.onclickloaddata == "indi")
                {

                    if (data.regorname_map == "routelocmap")
                    {

                        data.messagelist = (from a in _context.Route_Location
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == data.MI_Id && a.TRMRL_ActiveFlag==true && b.TRMR_ActiveFlg==true)
                                            select new TransportReportDTO
                                            {
                                                TRMRL_Id = a.TRMRL_Id,
                                                routename = b.TRMR_RouteName,
                                                locationname = c.TRML_LocationName,
                                                TRML_Id = c.TRML_Id,
                                                TRMR_Id = b.TRMR_Id,
                                                TRMR_RouteNo = b.TRMR_RouteNo,
                                                TRMR_order = b.TRMR_order,
                                                TRMRL_ActiveFlag = a.TRMRL_ActiveFlag
                                            }).Distinct().OrderBy(a => a.TRMR_order).ToArray();
                    }

                    else if (data.regorname_map == "vehicalroutemap")
                    {
                        data.messagelist = (from a in _context.VehicleRouteDMo
                                            from b in _context.VehicleRouteSessionDMO
                                            from c in _context.Master_VehicleDMO
                                            from d in _context.MasterRouteDMO
                                            from e in _context.MsterSessionDMO
                                            where (a.TRVR_Id == b.TRVR_Id && a.TRMV_Id == c.TRMV_Id && a.TRMR_Id == d.TRMR_Id
                                             && b.TRMS_Id == e.TRMS_Id && a.MI_Id == data.MI_Id && a.TRVR_ActiveFlg==true)
                                            select new TransportReportDTO
                                            {
                                                TRVR_Id = a.TRVR_Id,
                                                TRMV_VehicleName = c.TRMV_VehicleName,
                                                TRMR_RouteName = d.TRMR_RouteName,
                                                TRMS_SessionName = e.TRMS_SessionName,
                                                TRMS_Flag = e.TRMS_Flag,
                                                TRVR_Date = a.TRVR_Date,
                                                TRVR_ActiveFlg = a.TRVR_ActiveFlg
                                            }).ToArray();
                    }

                    else if (data.regorname_map == "vehicaldrivermap")
                    {
                        data.messagelist = (from a in _context.Master_VehicleDMO
                                            from b in _context.MasterDriverDMO
                                            from c in _context.VehicleDriver
                                            from d in _context.MsterSessionDMO
                                            from e in _context.VehicleDriverSessionDMO
                                            where (a.TRMV_Id == c.TRMV_Id && c.TRMD_Id == b.TRMD_Id && c.TRVD_Id == e.TRVD_Id && e.TRMS_Id == d.TRMS_Id && c.MI_Id == data.MI_Id && c.TRVD_ActiveFlg==true && a.TRMV_ActiveFlag==true && b.TRMD_ActiveFlg==true)
                                            select new TransportReportDTO
                                            {
                                                TRVD_Id = c.TRVD_Id,
                                                TRMV_VehicleName = a.TRMV_VehicleName,
                                                TRMD_DriverName = b.TRMD_DriverName,
                                                TRVD_Date = c.TRVD_Date,
                                                TRMS_Id = e.TRMS_Id,
                                                TRMS_Flag = d.TRMS_Flag,
                                                TRMS_SessionName = d.TRMS_SessionName,
                                                TRVD_ActiveFlg = c.TRVD_ActiveFlg


                                            }).ToArray();
                    }

                    else if (data.regorname_map == "vehicaldriversubstitute")
                    {
                        data.messagelist = (from a in _context.TR_Vehicle_Driver_SubstituteDMO
                                               where (a.MI_Id == data.MI_Id)
                                            select new VehicalDriverSubstituteDTO
                                            {
                                                TRVDST_Id = a.TRVDST_Id,
                                                TRMV_Id = a.TRMV_Id,
                                                TRVDS_AbsentDriverId = a.TRVDST_AbsentDriverId,
                                                TRVDS_SubstituteDriverId = a.TRVDST_SubstituteDriverId,
                                                Absent_Driver = _context.MasterDriverDMO.Single(e => e.TRMD_Id == a.TRVDST_AbsentDriverId).TRMD_DriverName,
                                                Substitute_Driver = _context.MasterDriverDMO.Single(e => e.TRMD_Id == a.TRVDST_SubstituteDriverId).TRMD_DriverName,
                                                TRVDS_FromDate = a.TRVDST_FromDate,
                                                TRVDS_ToDate = a.TRVDST_ToDate,
                                                TRMV_VehicleName = _context.Master_VehicleDMO.Single(f => f.TRMV_Id == a.TRMV_Id).TRMV_VehicleName
                                            }).ToArray();
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }
    }
}
