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
    public class KMLogBookImpl : Interfaces.KMLogBookInterface
    {
        public TransportContext _context;
        public ILogger<KMLogBookDTO> _log;

        public KMLogBookImpl(ILogger<KMLogBookDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }

        public KMLogBookDTO getdata(int id)
        {
            KMLogBookDTO data = new KMLogBookDTO();
            try
            {


                data.fillvahicleno = _context.Master_VehicleDMO.Where(t => t.MI_Id == id && t.TRMV_ActiveFlag == true).ToArray();

               // data.getloaddata = _context.DriverChartDMO.Where(a => a.MI_Id == id).OrderByDescending(a => a.TRDC_Id).ToArray();

                data.getloaddata = (from a in _context.TR_KM_LogBookDMO
                                    from c in _context.Master_VehicleDMO
                                    where  a.MI_Id == c.MI_Id && a.MI_Id ==id && a.TRMV_Id == c.TRMV_Id && c.TRMV_ActiveFlag == true
                                    select new KMLogBookDTO
                                    {   TRKMLB_Id=a.TRKMLB_Id,
                                        TRMV_VehicleNo=c.TRMV_VehicleNo,
                                        TRKMLB_EntryDate=a.TRKMLB_EntryDate,
                                        TRKMLB_FromDate=a.TRKMLB_FromDate,
                                        TRKMLB_ToDate=a.TRKMLB_ToDate,
                                        TRKMLB_OpeningReading=a.TRKMLB_OpeningReading,
                                        TRKMLB_ClosingReading=a.TRKMLB_ClosingReading,
                                        TRKMLB_NoOfKM=a.TRKMLB_NoOfKM,
                                        TRKMLB_FromTime=a.TRKMLB_FromTime,
                                        TRKMLB_ToTime=a.TRKMLB_ToTime,
                                        TRKMLB_Remarks=a.TRKMLB_Remarks,
                                        TRKMLB_ActiveFlag = a.TRKMLB_ActiveFlag,

                                    }).Distinct().OrderBy(x=>x.TRKMLB_ToDate).ToArray();
                //data.filldrivrname = _context.MasterDriverDMO.Where(t => t.MI_Id == id && t.TRMD_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }


        public KMLogBookDTO Onvahiclechange(KMLogBookDTO data)
        {
            try
            {
                //data.filldrivrname = (from a in _context.MasterDriverDMO
                //                      from b in _context.VehicleDriver
                //                      where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMD_Id == b.TRMD_Id && a.TRMD_ActiveFlg == true && b.TRMV_Id == data.TRMV_Id && b.TRVD_ActiveFlg == true)
                //                      select a).ToArray();

                var filloppeningkm = _context.TR_KM_LogBookDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMV_Id == data.TRMV_Id && f.TRKMLB_ActiveFlag==true).Distinct().OrderByDescending(t => t.TRKMLB_ClosingReading).ToList();
                if (filloppeningkm.Count>0)
                {
                    data.TRKMLB_ClosingReading = filloppeningkm[0].TRKMLB_ClosingReading;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }


        public KMLogBookDTO deleterecord(KMLogBookDTO data)
        {
            if (data.TRKMLB_Id > 0)
            {
                var result = _context.TR_KM_LogBookDMO.Single(t => t.TRKMLB_Id == data.TRKMLB_Id);

                if (result.TRKMLB_ActiveFlag==true)
                {
                    result.TRKMLB_ActiveFlag = false;
                }
                else
                {
                    result.TRKMLB_ActiveFlag = true;
                }
                _context.Update(result);

                var flag = _context.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }
        public KMLogBookDTO savedata(KMLogBookDTO data)
        {
            try
            {
                if (data.TRKMLB_Id > 0)
                {
                    var chek_duplicate_fromkm = _context.TR_KM_LogBookDMO.Where(a => a.MI_Id == data.MI_Id &&  a.TRKMLB_ToDate == data.TRKMLB_ToDate && a.TRKMLB_FromDate == data.TRKMLB_FromDate &&  a.TRMV_Id == data.TRMV_Id && a.TRKMLB_Id != data.TRKMLB_Id).ToList();
                   
                    if (chek_duplicate_fromkm.Count == 0)
                    {
                        var result = _context.TR_KM_LogBookDMO.Single(a => a.MI_Id == data.MI_Id && a.TRKMLB_Id == data.TRKMLB_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRKMLB_FromDate = data.TRKMLB_FromDate;
                        result.TRKMLB_ToDate = data.TRKMLB_ToDate;
                        result.TRKMLB_NoOfKM = data.TRKMLB_NoOfKM;
                        result.TRKMLB_OpeningReading = data.TRKMLB_OpeningReading;
                        result.TRKMLB_ClosingReading = data.TRKMLB_ClosingReading;
                        result.TRKMLB_FromTime = data.TRKMLB_FromTime;
                        result.TRKMLB_ToTime = data.TRKMLB_ToTime;
                        result.TRKMLB_Remarks = data.TRKMLB_Remarks;
                        result.TRKMLB_EntryDate = data.TRKMLB_EntryDate;
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
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    TR_KM_LogBookDMO chat = new TR_KM_LogBookDMO();
                    var chek_duplicate_fromkm = _context.TR_KM_LogBookDMO.Where(a => a.MI_Id == data.MI_Id && a.TRKMLB_ToDate == data.TRKMLB_ToDate && a.TRKMLB_FromDate == data.TRKMLB_FromDate && a.TRMV_Id == data.TRMV_Id ).ToList();
                    if (chek_duplicate_fromkm.Count == 0)
                    {
                        chat.MI_Id = data.MI_Id;
                        chat.TRMV_Id = data.TRMV_Id;
                        chat.TRKMLB_FromDate = data.TRKMLB_FromDate;
                        chat.TRKMLB_ToDate = data.TRKMLB_ToDate;
                        chat.TRKMLB_NoOfKM = data.TRKMLB_NoOfKM;
                        chat.TRKMLB_OpeningReading = data.TRKMLB_OpeningReading;
                        chat.TRKMLB_ClosingReading = data.TRKMLB_ClosingReading;
                        chat.TRKMLB_FromTime = data.TRKMLB_FromTime;
                        chat.TRKMLB_ToTime = data.TRKMLB_ToTime;
                        chat.TRKMLB_Remarks = data.TRKMLB_Remarks;
                        chat.TRKMLB_EntryDate = data.TRKMLB_EntryDate;
                        chat.TRKMLB_ActiveFlag = true;


                        chat.CreatedDate = DateTime.Now;
                        chat.UpdatedDate = DateTime.Now;
                        _context.Add(chat);
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
                _log.LogInformation("Transport Error " + ex.Message);
            }
            return data;
        }

        public KMLogBookDTO edit(KMLogBookDTO data)
        {
            try
            {
                data.geteditdata = _context.TR_KM_LogBookDMO.Where(a => a.MI_Id == data.MI_Id && a.TRKMLB_Id == data.TRKMLB_Id).OrderByDescending(a => a.TRKMLB_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error " + ex.Message);
            }
            return data;
        }

        ////REPORT METHODS////
        public KMLogBookDTO getreportdata(int id)
        {
            KMLogBookDTO data = new KMLogBookDTO();
            try
            {


                data.fillvehicletype = _context.MasterVehicleTypeDMO.Where(t => t.MI_Id == id && t.TRMVT_ActiveFlg == true).ToArray();


                data.monthlist = _context.MonthDMO.Where(t => t.Is_Active == true).ToArray();
                data.servicestlist = _context.TR_Master_ServStationDMO.Where(t => t.TRMSST_ActiveFlag == true && t.MI_Id==id).ToArray();


                

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }

        public KMLogBookDTO vehicletypechange(KMLogBookDTO data)
        {
            try
            {
                if (data.TRMVT_Id == 0)
                {
                    data.fillvahicleno = (from a in _context.MasterVehicleTypeDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMVT_Id == b.TRMVT_Id && a.TRMVT_ActiveFlg == true && b.TRMV_ActiveFlag == true && a.MI_Id==data.MI_Id
                                          select b).ToArray();
                }
                else
                {
                    data.fillvahicleno = (from a in _context.MasterVehicleTypeDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMVT_Id == b.TRMVT_Id && a.TRMVT_ActiveFlg == true && b.TRMV_ActiveFlag == true && a.TRMVT_Id == data.TRMVT_Id && a.MI_Id==data.MI_Id
                                          select b).ToArray();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public KMLogBookDTO getkmreport(KMLogBookDTO data)
        {
            try
            {
                if (data.vhlid.Length>0)
                {
                    List<long> HeadId = new List<long>();


                    foreach (var item in data.vhlid)
                    {
                        HeadId.Add(item.TRMV_Id);

                    }
                    data.fillkmreport = (from a in _context.TR_KM_LogBookDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMV_Id == b.TRMV_Id && HeadId.Contains(a.TRMV_Id) && a.TRKMLB_ActiveFlag == true && a.TRKMLB_FromDate>=data.TRKMLB_FromDate && a.TRKMLB_FromDate<=data.TRKMLB_ToDate
                                         select new KMLogBookDTO
                                         {
                                             TRMV_VehicleNo=b.TRMV_VehicleNo,
                                             TRKMLB_FromDate=a.TRKMLB_FromDate,
                                             TRKMLB_ToDate=a.TRKMLB_ToDate,
                                             TRKMLB_OpeningReading=a.TRKMLB_OpeningReading,
                                             TRKMLB_ClosingReading=a.TRKMLB_ClosingReading,
                                             TRKMLB_NoOfKM=a.TRKMLB_NoOfKM
                                         }

                                       ).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

    }
}
