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
    public class DriverChartImpl : Interfaces.DriverChartInterface
    {
        public TransportContext _context;
        public ILogger<DriverChartDTO> _log;

        public DriverChartImpl(ILogger<DriverChartDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }

        public DriverChartDTO getdata(int id)
        {
            DriverChartDTO data = new DriverChartDTO();
            try
            {


                data.fillvahicleno = _context.Master_VehicleDMO.Where(t => t.MI_Id == id && t.TRMV_ActiveFlag == true).ToArray();

               // data.getloaddata = _context.DriverChartDMO.Where(a => a.MI_Id == id).OrderByDescending(a => a.TRDC_Id).ToArray();

                data.getloaddata = (from a in _context.DriverChartDMO
                                    from b in _context.MasterDriverDMO
                                    from c in _context.Master_VehicleDMO
                                    where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id ==id && a.TRMD_Id == b.TRMD_Id && a.TRMV_Id == c.TRMV_Id && b.TRMD_ActiveFlg == true && c.TRMV_ActiveFlag == true
                                    select new DriverChartDTO
                                    {   TRDC_Id=a.TRDC_Id,
                                        TRMD_DriverName=b.TRMD_DriverName,
                                        TRMV_VehicleNo=c.TRMV_VehicleNo,
                                        TRDC_FromKM=a.TRDC_FromKM,
                                        TRDC_ToKM=a.TRDC_ToKM,
                                        TRDC_Indent_BillNo=a.TRDC_Indent_BillNo,
                                        TRDC_Emission=a.TRDC_Emission,
                                        TRDC_Date=a.TRDC_Date,
                                        TRDC_GrossAmount=a.TRDC_GrossAmount,
                                        TRDC_NoofLtr=a.TRDC_NoofLtr,
                                        TRDC_RateKm=a.TRDC_RateKm


                                    }).ToArray();
                data.filldrivrname = _context.MasterDriverDMO.Where(t => t.MI_Id == id && t.TRMD_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }


        public DriverChartDTO Onvahiclechange(DriverChartDTO data)
        {
            try
            {
                //data.filldrivrname = (from a in _context.MasterDriverDMO
                //                      from b in _context.VehicleDriver
                //                      where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMD_Id == b.TRMD_Id && a.TRMD_ActiveFlg == true && b.TRMV_Id == data.TRMV_Id && b.TRVD_ActiveFlg == true)
                //                      select a).ToArray();

                var filloppeningkm = _context.DriverChartDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMV_Id == data.TRMV_Id).Distinct().OrderByDescending(t => t.TRDC_ToKM).ToList();
                if (filloppeningkm.Count>0)
                {
                    data.TRDC_FromKM = filloppeningkm[0].TRDC_ToKM;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }


        public DriverChartDTO deleterecord(DriverChartDTO data)
        {
            if (data.TRDC_Id > 0)
            {
                var result = _context.DriverChartDMO.Single(t => t.TRDC_Id == data.TRDC_Id);
                _context.Remove(result);

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
        public DriverChartDTO savedata(DriverChartDTO data)
        {
            try
            {
                if (data.TRDC_Id > 0)
                {
                    var chek_duplicate_fromkm = _context.DriverChartDMO.Where(a => a.MI_Id == data.MI_Id && a.TRDC_FromKM.Equals(data.TRDC_FromKM) && a.TRDC_AddtAmt == data.TRDC_AddtAmt && a.TRDC_Date == data.TRDC_Date && a.TRDC_Emission == data.TRDC_Emission && a.TRDC_NoofLtr == data.TRDC_NoofLtr && a.TRDC_RateKm == data.TRDC_RateKm && a.TRDC_Indent_BillNo == data.TRDC_Indent_BillNo && a.TRMD_Id == data.TRMD_Id && a.TRMV_Id == data.TRMV_Id && a.TRDC_Id != data.TRDC_Id).ToList();
                    var chek_duplicate_tokm = _context.DriverChartDMO.Where(a => a.MI_Id == data.MI_Id && a.TRDC_ToKM.Equals(data.TRDC_ToKM) && a.TRDC_AddtAmt == data.TRDC_AddtAmt && a.TRDC_Date == data.TRDC_Date && a.TRDC_Emission == data.TRDC_Emission && a.TRDC_NoofLtr == data.TRDC_NoofLtr && a.TRDC_RateKm == data.TRDC_RateKm && a.TRDC_Indent_BillNo == data.TRDC_Indent_BillNo && a.TRMD_Id == data.TRMD_Id && a.TRMV_Id == data.TRMV_Id && a.TRDC_Id != data.TRDC_Id).ToList();
                    if (chek_duplicate_fromkm.Count == 0 && chek_duplicate_tokm.Count == 0)
                    {
                        var result = _context.DriverChartDMO.Single(a => a.MI_Id == data.MI_Id && a.TRDC_Id == data.TRDC_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRMD_Id = data.TRMD_Id;
                        result.TRDC_FromKM = data.TRDC_FromKM;
                        result.TRDC_ToKM = data.TRDC_ToKM;
                        result.TRDC_TotalKM = data.TRDC_TotalKM;
                        result.TRDC_NoofLtr = data.TRDC_NoofLtr;
                        result.TRDC_TotalMileage = data.TRDC_TotalMileage;
                        result.TRDC_TotalAmount = data.TRDC_TotalAmount;
                        result.TRDC_RateKm = data.TRDC_RateKm;
                        result.TRDC_Date = data.TRDC_Date;
                        result.TRDC_Emission = data.TRDC_Emission;
                        result.TRDC_AddtAmt = data.TRDC_AddtAmt;
                        result.TRDC_Remarks = data.TRDC_Remarks;
                        result.TRDC_GrossAmount = data.TRDC_GrossAmount;
                        result.TRDC_Indent_BillNo = data.TRDC_Indent_BillNo;
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
                    DriverChartDMO chat = new DriverChartDMO();
                    var chek_duplicate_fromkm = _context.DriverChartDMO.Where(a => a.MI_Id == data.MI_Id && a.TRDC_FromKM.Equals(data.TRDC_FromKM)&& chat.TRDC_AddtAmt==data.TRDC_AddtAmt && chat.TRDC_Date==data.TRDC_Date && chat.TRDC_Emission==data.TRDC_Emission && chat.TRDC_NoofLtr==data.TRDC_NoofLtr &&chat.TRDC_RateKm==data.TRDC_RateKm && chat.TRDC_Indent_BillNo==data.TRDC_Indent_BillNo &&chat.TRMD_Id==data.TRMD_Id && chat.TRMV_Id==data.TRMV_Id).ToList();
                    var chek_duplicate_tokm = _context.DriverChartDMO.Where(a => a.MI_Id == data.MI_Id && a.TRDC_ToKM.Equals(data.TRDC_ToKM)&& chat.TRDC_AddtAmt==data.TRDC_AddtAmt && chat.TRDC_Date==data.TRDC_Date && chat.TRDC_Emission==data.TRDC_Emission && chat.TRDC_NoofLtr==data.TRDC_NoofLtr &&chat.TRDC_RateKm==data.TRDC_RateKm && chat.TRDC_Indent_BillNo==data.TRDC_Indent_BillNo &&chat.TRMD_Id==data.TRMD_Id && chat.TRMV_Id==data.TRMV_Id).ToList();
                    if (chek_duplicate_fromkm.Count == 0 && chek_duplicate_tokm.Count == 0)
                    {
                        chat.MI_Id = data.MI_Id;
                        chat.TRMV_Id = data.TRMV_Id;
                        chat.TRMD_Id = data.TRMD_Id;
                        chat.TRDC_FromKM = data.TRDC_FromKM;
                        chat.TRDC_ToKM = data.TRDC_ToKM;
                        chat.TRDC_TotalKM = data.TRDC_TotalKM;
                        chat.TRDC_NoofLtr = data.TRDC_NoofLtr;
                        chat.TRDC_TotalMileage = data.TRDC_TotalMileage;
                        chat.TRDC_TotalAmount = data.TRDC_TotalAmount;
                        chat.TRDC_RateKm = data.TRDC_RateKm;
                        chat.TRDC_Date = data.TRDC_Date;
                        chat.TRDC_Emission = data.TRDC_Emission;
                        chat.TRDC_AddtAmt = data.TRDC_AddtAmt;
                        chat.TRDC_Remarks = data.TRDC_Remarks;
                        chat.TRDC_GrossAmount = data.TRDC_GrossAmount;
                        chat.TRDC_Indent_BillNo = data.TRDC_Indent_BillNo;


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
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }

        public DriverChartDTO edit(DriverChartDTO data)
        {
            try
            {
                data.geteditdata = _context.DriverChartDMO.Where(a => a.MI_Id == data.MI_Id && a.TRDC_Id == data.TRDC_Id).OrderByDescending(a => a.TRDC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }
    }
}
