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
    public class TransportStatusReportImpl : Interfaces.TransportStatusReportInterface
    {
        private static ConcurrentDictionary<string, TransportStatusReportDTO> _login =
        new ConcurrentDictionary<string, TransportStatusReportDTO>();

        public TransportContext _context;
        ILogger<TransportStatusReportImpl> _areaimpl;
        public TransportStatusReportImpl(ILogger<TransportStatusReportImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public TransportStatusReportDTO getdata(int id)
        {
            TransportStatusReportDTO data = new TransportStatusReportDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id && y.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.masterclass = _context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();
                data.routendetails = _context.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg == true).OrderBy(mm=>mm.TRMR_order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }

        public TransportStatusReportDTO Getreportdetails1(TransportStatusReportDTO data)
        {
            try
            {
                if (data.onclickloaddata == "All")
                {
                    if (data.cnt12 == true && data.regorname_map == "both")
                    {
                        data.countlist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                          where (a.MI_Id == data.MI_Id)
                                          select new TransportStatusReportDTO
                                          {

                                              ASTA_ApplStatus = a.ASTA_ApplStatus,
                                              ASTA_Id = a.ASTA_Id,
                                              stud_count = a.ASTA_Id

                                          }).Distinct().GroupBy(id => new { id.ASTA_ApplStatus }).Select(g => new TransportStatusReportDTO { ASTA_ApplStatus = g.Key.ASTA_ApplStatus, stud_count = g.Count() }).ToArray();
                    }
                    else if (data.cnt12 == true && data.regorname_map == "new")
                    {
                        data.countlist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                          where (a.MI_Id == data.MI_Id && a.ASTA_Regnew == "new")
                                          select new TransportStatusReportDTO
                                          {

                                              ASTA_ApplStatus = a.ASTA_ApplStatus,
                                              ASTA_Regnew = a.ASTA_Regnew,
                                              ASTA_Id = a.ASTA_Id,
                                              stud_count = a.ASTA_Id

                                          }).Distinct().GroupBy(id => new { id.ASTA_ApplStatus, id.ASTA_Regnew }).Select(g => new TransportStatusReportDTO { ASTA_ApplStatus = g.Key.ASTA_ApplStatus, ASTA_Regnew = g.Key.ASTA_ApplStatus, stud_count = g.Count() }).ToArray();
                    }
                    else if (data.cnt12 == true && data.regorname_map == "regular")
                    {
                        data.countlist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                          where (a.MI_Id == data.MI_Id && a.ASTA_Regnew == "regular")
                                          select new TransportStatusReportDTO
                                          {

                                              ASTA_ApplStatus = a.ASTA_ApplStatus,
                                              ASTA_Regnew = a.ASTA_Regnew,
                                              ASTA_Id = a.ASTA_Id,
                                              stud_count = a.ASTA_Id

                                          }).Distinct().GroupBy(id => new { id.ASTA_ApplStatus, id.ASTA_Regnew }).Select(g => new TransportStatusReportDTO { ASTA_ApplStatus = g.Key.ASTA_ApplStatus, ASTA_Regnew = g.Key.ASTA_Regnew, stud_count = g.Count() }).ToArray();
                    }


                    //date

                    else if (data.cnt12 == false && data.regorname_map == "new" && data.cnt11 == "apd")
                    {

                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student

                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "new" && a.ASTA_ApplicationDate >= data.FMCB_fromDATE && a.ASTA_ApplicationDate <= data.FMCB_toDATE)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTA_ApplicationDate = a.ASTA_ApplicationDate,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                ASTA_Id = a.ASTA_Id,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    else if (data.cnt12 == false && data.regorname_map == "regular" && data.cnt11 == "apd")
                    {

                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student

                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "regular" && a.ASTA_ApplicationDate >= data.FMCB_fromDATE && a.ASTA_ApplicationDate <= data.FMCB_toDATE)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTA_ApplicationDate = a.ASTA_ApplicationDate,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                ASTA_Id = a.ASTA_Id,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    else if (data.cnt12 == false && data.regorname_map == "both" && data.cnt11 == "apd")
                    {

                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student

                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplicationDate >= data.FMCB_fromDATE && a.ASTA_ApplicationDate <= data.FMCB_toDATE)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTA_ApplicationDate = a.ASTA_ApplicationDate,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
                    }


                    else if (data.cnt12 == false && data.regorname_map == "new" && data.cnt11 == "aprd")
                    {

                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            from e in _context.TransportApprovedDMO

                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "new" && e.ASTAA_Date >= data.FMCB_fromDATE && e.ASTAA_Date <= data.FMCB_toDATE && e.ASTA_Id == a.ASTA_Id)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTAA_Date = e.ASTAA_Date,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                ASTA_Id = a.ASTA_Id,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    else if (data.cnt12 == false && data.regorname_map == "regular" && data.cnt11 == "aprd")
                    {

                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            from e in _context.TransportApprovedDMO

                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "regular" && e.ASTAA_Date >= data.FMCB_fromDATE && e.ASTAA_Date <= data.FMCB_toDATE && e.ASTA_Id == a.ASTA_Id)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTAA_Date = e.ASTAA_Date,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                ASTA_Id = a.ASTA_Id,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    else if (data.cnt12 == false && data.regorname_map == "both" && data.cnt11 == "aprd")
                    {

                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            from e in _context.TransportApprovedDMO


                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && e.ASTAA_Date >= data.FMCB_fromDATE && e.ASTAA_Date <= data.FMCB_toDATE && e.ASTA_Id == a.ASTA_Id)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTAA_Date = e.ASTAA_Date,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
                    }




                    //end






                }
                else if (data.onclickloaddata == "indi")
                {
                    if (data.cnt12 == true && data.regorname_map == "both")
                    {
                        data.countlist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                          where (a.MI_Id == data.MI_Id && a.ASTA_ApplStatus.Equals(data.ASTA_ApplStatus))
                                          select new TransportStatusReportDTO
                                          {

                                              ASTA_ApplStatus = a.ASTA_ApplStatus,
                                              ASTA_Id = a.ASTA_Id,
                                              stud_count = a.ASTA_Id

                                          }).Distinct().GroupBy(id => new { id.ASTA_ApplStatus }).Select(g => new TransportStatusReportDTO { ASTA_ApplStatus = g.Key.ASTA_ApplStatus, stud_count = g.Count() }).ToArray();
                    }
                    else if (data.cnt12 == true && data.regorname_map == "new")
                    {
                        data.countlist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                          where (a.MI_Id == data.MI_Id && a.ASTA_ApplStatus.Equals(data.ASTA_ApplStatus) && a.ASTA_Regnew == "new")
                                          select new TransportStatusReportDTO
                                          {

                                              ASTA_ApplStatus = a.ASTA_ApplStatus,
                                              ASTA_Regnew = a.ASTA_Regnew,
                                              ASTA_Id = a.ASTA_Id,
                                              stud_count = a.ASTA_Id

                                          }).Distinct().GroupBy(id => new { id.ASTA_ApplStatus, id.ASTA_Regnew }).Select(g => new TransportStatusReportDTO { ASTA_ApplStatus = g.Key.ASTA_ApplStatus, ASTA_Regnew = g.Key.ASTA_Regnew, stud_count = g.Count() }).ToArray();
                    }
                    else if (data.cnt12 == true && data.regorname_map == "regular")
                    {
                        data.countlist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                          where (a.MI_Id == data.MI_Id && a.ASTA_ApplStatus.Equals(data.ASTA_ApplStatus) && a.ASTA_Regnew == "regular")
                                          select new TransportStatusReportDTO
                                          {

                                              ASTA_ApplStatus = a.ASTA_ApplStatus,
                                              ASTA_Regnew = a.ASTA_Regnew,
                                              ASTA_Id = a.ASTA_Id,
                                              stud_count = a.ASTA_Id

                                          }).Distinct().GroupBy(id => new { id.ASTA_ApplStatus, id.ASTA_Regnew }).Select(g => new TransportStatusReportDTO { ASTA_ApplStatus = g.Key.ASTA_ApplStatus, ASTA_Regnew = g.Key.ASTA_Regnew, stud_count = g.Count() }).ToArray();
                    }



                    else if (data.cnt12 == false && data.regorname_map == "both" && data.cnt11 == "apd")

                    {
                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && a.ASTA_ApplicationDate >= data.FMCB_fromDATE && a.ASTA_ApplicationDate <= data.FMCB_toDATE)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTA_ApplicationDate = a.ASTA_ApplicationDate,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();


                    }

                    else if (data.cnt12 == false && data.regorname_map == "new" && data.cnt11 == "apd")
                    {
                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && a.ASTA_Regnew == "new" && a.ASTA_ApplicationDate >= data.FMCB_fromDATE && a.ASTA_ApplicationDate <= data.FMCB_toDATE)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTA_ApplicationDate = a.ASTA_ApplicationDate,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    else if (data.cnt12 == false && data.regorname_map == "regular" && data.cnt11 == "apd")
                    {
                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && a.ASTA_Regnew == "regular" && a.ASTA_ApplicationDate >= data.FMCB_fromDATE && a.ASTA_ApplicationDate <= data.FMCB_toDATE)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTA_ApplicationDate = a.ASTA_ApplicationDate,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }

                    //Date

                    else if (data.cnt12 == false && data.regorname_map == "both" && data.cnt11 == "aprd")

                    {
                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            from e in _context.TransportApprovedDMO

                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && e.ASTAA_Date >= data.FMCB_fromDATE && e.ASTAA_Date <= data.FMCB_toDATE && e.ASTA_Id == a.ASTA_Id)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTAA_Date = e.ASTAA_Date,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();


                    }

                    else if (data.cnt12 == false && data.regorname_map == "new" && data.cnt11 == "aprd")
                    {
                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            from e in _context.TransportApprovedDMO
                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && a.ASTA_Regnew == "new" && e.ASTAA_Date >= data.FMCB_fromDATE && e.ASTAA_Date <= data.FMCB_toDATE && e.ASTA_Id == a.ASTA_Id)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTAA_Date = e.ASTAA_Date,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    else if (data.cnt12 == false && data.regorname_map == "regular" && data.cnt11 == "aprd")
                    {
                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
                                            from b in _context.MasterRouteDMO
                                            from c in _context.MasterLocationDMO
                                            from d in _context.Adm_M_Student
                                            from e in _context.TransportApprovedDMO
                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && a.ASTA_Regnew == "regular" && e.ASTAA_Date >= data.FMCB_fromDATE && e.ASTAA_Date <= data.FMCB_toDATE && e.ASTA_Id == a.ASTA_Id)
                                            select new TransportStatusReportDTO
                                            {
                                                ASTAA_Date = e.ASTAA_Date,
                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
                                                AMST_Id = a.AMST_Id,
                                                ASTA_Id = a.ASTA_Id,
                                                ASTA_Regnew = a.ASTA_Regnew,
                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"

                                            }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

                    }
                    //    end



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TransportStatusReportDTO Getreportdetails(TransportStatusReportDTO data)
        {
            try
            {
                if (data.cnt12 == false && data.updatedstu == false)
                {
                    string datefrom = Convert.ToDateTime(data.FMCB_fromDATE).ToString("yyyy-MM-dd");
                    string dateto = Convert.ToDateTime(data.FMCB_toDATE).ToString("yyyy-MM-dd");
                    var flag = 0;
                    if (data.cnt12 == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "TR_Ttansport_status_report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@allorindi",
                           SqlDbType.VarChar)
                        {
                            Value = data.onclickloaddata
                        });
                        cmd.Parameters.Add(new SqlParameter("@applicationdate",
                           SqlDbType.VarChar)
                        {
                            Value = data.cnt11
                        });
                        cmd.Parameters.Add(new SqlParameter("@approveddate",
                          SqlDbType.VarChar)
                        {
                            Value = data.cnt11
                        });
                        cmd.Parameters.Add(new SqlParameter("@newregboth",
                      SqlDbType.VarChar)
                        {
                            Value = data.regorname_map
                        });

                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                      SqlDbType.VarChar)
                        {
                            Value = datefrom
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                        {
                            Value = dateto
                        });
                        cmd.Parameters.Add(new SqlParameter("@countflag",
                  SqlDbType.VarChar)
                        {
                            Value = flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@statusflg",
               SqlDbType.VarChar)
                        {
                            Value = data.ASTA_ApplStatus
                        });
                        cmd.Parameters.Add(new SqlParameter("@payment",
              SqlDbType.VarChar)
                        {
                            Value = data.paymentoption
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
             SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TRMR_Id",
             SqlDbType.VarChar)
                        {
                            Value = data.TRMR_Id
                        });



                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.messagelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if(data.updatedstu==true)
                {
                    string datefrom = Convert.ToDateTime(data.FMCB_fromDATE).ToString("yyyy-MM-dd");
                    string dateto = Convert.ToDateTime(data.FMCB_toDATE).ToString("yyyy-MM-dd");
                    var flag = 0;
                    if (data.cnt12 == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TR_Ttansport_status_Update_report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@class",
                          SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                      SqlDbType.VarChar)
                        {
                            Value = datefrom
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                        {
                            Value = dateto
                        });
                        cmd.Parameters.Add(new SqlParameter("@updatedtype",
                   SqlDbType.VarChar)
                        {
                            Value = data.updatedtype
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.messagelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    string datefrom = Convert.ToDateTime(data.FMCB_fromDATE).ToString("yyyy-MM-dd");
                    string dateto = Convert.ToDateTime(data.FMCB_toDATE).ToString("yyyy-MM-dd");
                    var flag = 0;
                    if (data.cnt12 == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TR_Ttansport_status_Count_report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                      SqlDbType.VarChar)
                        {
                            Value = datefrom
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                        {
                            Value = dateto
                        });                  
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.messagelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
