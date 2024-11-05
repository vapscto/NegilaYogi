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
    public class RTODetailsImpl : Interfaces.RTODetailsInterface
    {
        public TransportContext _context;
        public ILogger<RTODetailsDTO> _log;

        public RTODetailsImpl(ILogger<RTODetailsDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }

        public RTODetailsDTO getdata(int id)
        {
            RTODetailsDTO data = new RTODetailsDTO();
            try
            {


                data.fillvahicleno = _context.Master_VehicleDMO.Where(t => t.MI_Id == id && t.TRMV_ActiveFlag == true).ToArray();

                // data.getloaddata = _context.DriverChartDMO.Where(a => a.MI_Id == id).OrderByDescending(a => a.TRDC_Id).ToArray();

                data.getloaddata = (from a in _context.RTODetailsDMO
                                    from b in _context.Master_VehicleDMO
                                  
                                    where a.MI_Id == b.MI_Id  && a.MI_Id == id && a.TRMV_Id == b.TRMV_Id &&  b.TRMV_ActiveFlag == true 
                                    select new RTODetailsDTO
                                    {
                   TRRTO_Id = a.TRRTO_Id,
                   TRMV_VehicleNo = b.TRMV_VehicleNo,
                TRRTO_GPS_GPRS_Fitted_date = a.TRRTO_GPS_GPRS_Fitted_date,
                TRRTO_Insurance_FromDate = a.TRRTO_Insurance_FromDate,
                TRRTO_Insurance_Todate = a.TRRTO_Insurance_Todate,
                TRRTO_Company_Name = a.TRRTO_Company_Name,
                TRRTO_Tax_FromDate = a.TRRTO_Tax_FromDate,
                TRRTO_Tax_ToDate = a.TRRTO_Tax_ToDate,
                TRRTO_FC_FromDate = a.TRRTO_FC_FromDate,
                TRRTO_FC_ToDate = a.TRRTO_FC_ToDate,
                TRRTO_Permit_FromDate = a.TRRTO_Permit_FromDate,
                TRRTO_Permit_ToDate = a.TRRTO_Permit_ToDate,
                TRRTO_Emission_FromDate = a.TRRTO_Emission_FromDate,
                TRRTO_Emission_ToDate = a.TRRTO_Emission_ToDate,
                TRRTO_Ceasefire_FromDate = a.TRRTO_Ceasefire_FromDate,
                TRRTO_Ceasefire_ToDate = a.TRRTO_Ceasefire_ToDate,

            }).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }


        public RTODetailsDTO Onvahiclechange(RTODetailsDTO data)
        {
            try
            {
                //data.filldrivrname = (from a in _context.MasterDriverDMO
                //                      from b in _context.VehicleDriver
                //                      where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMD_Id == b.TRMD_Id && a.TRMD_ActiveFlg == true && b.TRMV_Id == data.TRMV_Id && b.TRVD_ActiveFlg == true)
                //                      select a).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }


        public RTODetailsDTO deleterecord(RTODetailsDTO data)
        {
            try
            {
                var result = _context.RTODetailsDMO.Single(t => t.MI_Id == data.MI_Id && t.TRRTO_Id == data.TRRTO_Id);
                _context.Remove(result);
                var i = _context.SaveChanges();
                if (i==1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false ;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public RTODetailsDTO savedata(RTODetailsDTO data)
        {
            try
            {
                if (data.TRRTO_Id > 0)
                {
                   
                    var dup = _context.RTODetailsDMO.Where(a => a.MI_Id == data.MI_Id  && a.TRMV_Id == data.TRMV_Id && a.TRRTO_GPS_GPRS_Fitted_date == data.TRRTO_GPS_GPRS_Fitted_date && a.TRRTO_Insurance_FromDate == data.TRRTO_Insurance_FromDate && a.TRRTO_Insurance_Todate == data.TRRTO_Insurance_Todate && a.TRRTO_Company_Name == data.TRRTO_Company_Name && a.TRRTO_Tax_FromDate == data.TRRTO_Tax_FromDate && a.TRRTO_Tax_ToDate == data.TRRTO_Tax_ToDate && a.TRRTO_FC_FromDate == data.TRRTO_FC_FromDate && a.TRRTO_FC_ToDate == data.TRRTO_FC_ToDate && a.TRRTO_Permit_FromDate == data.TRRTO_Permit_FromDate && a.TRRTO_Permit_ToDate == data.TRRTO_Permit_ToDate  && a.TRRTO_Emission_FromDate == data.TRRTO_Emission_FromDate && a.TRRTO_Emission_ToDate == data.TRRTO_Emission_ToDate && a.TRRTO_Ceasefire_FromDate == data.TRRTO_Ceasefire_FromDate && a.TRRTO_Ceasefire_ToDate == data.TRRTO_Ceasefire_ToDate && a.TRRTO_Id != data.TRRTO_Id).ToList();
                    if (dup.Count == 0 )
                    {
                        var result = _context.RTODetailsDMO.Single(a => a.MI_Id == data.MI_Id && a.TRRTO_Id == data.TRRTO_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRRTO_GPS_GPRS_Fitted_date = data.TRRTO_GPS_GPRS_Fitted_date;
                        result.TRRTO_Insurance_FromDate = data.TRRTO_Insurance_FromDate;
                        result.TRRTO_Insurance_Todate = data.TRRTO_Insurance_Todate;
                        result.TRRTO_Company_Name = data.TRRTO_Company_Name;
                        result.TRRTO_Tax_FromDate = data.TRRTO_Tax_FromDate;
                        result.TRRTO_Tax_ToDate = data.TRRTO_Tax_ToDate;
                        result.TRRTO_FC_FromDate = data.TRRTO_FC_FromDate;
                        result.TRRTO_FC_ToDate = data.TRRTO_FC_ToDate;
                        result.TRRTO_Permit_FromDate = data.TRRTO_Permit_FromDate;
                        result.TRRTO_Permit_ToDate = data.TRRTO_Permit_ToDate;
                  
                        result.TRRTO_Emission_FromDate = data.TRRTO_Emission_FromDate;
                        result.TRRTO_Emission_ToDate = data.TRRTO_Emission_ToDate;
                        result.TRRTO_Ceasefire_FromDate = data.TRRTO_Ceasefire_FromDate;
                        result.TRRTO_Ceasefire_ToDate = data.TRRTO_Ceasefire_ToDate;
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
                    RTODetailsDMO chat = new RTODetailsDMO();
                    var chek_duplicate =_context.RTODetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_Id == data.TRMV_Id && a.TRRTO_GPS_GPRS_Fitted_date == data.TRRTO_GPS_GPRS_Fitted_date && a.TRRTO_Insurance_FromDate == data.TRRTO_Insurance_FromDate && a.TRRTO_Insurance_Todate == data.TRRTO_Insurance_Todate && a.TRRTO_Company_Name == data.TRRTO_Company_Name && a.TRRTO_Tax_FromDate == data.TRRTO_Tax_FromDate && a.TRRTO_Tax_ToDate == data.TRRTO_Tax_ToDate && a.TRRTO_FC_FromDate == data.TRRTO_FC_FromDate && a.TRRTO_FC_ToDate == data.TRRTO_FC_ToDate && a.TRRTO_Permit_FromDate == data.TRRTO_Permit_FromDate && a.TRRTO_Permit_ToDate == data.TRRTO_Permit_ToDate && a.TRRTO_Emission_FromDate == data.TRRTO_Emission_FromDate && a.TRRTO_Emission_ToDate == data.TRRTO_Emission_ToDate && a.TRRTO_Ceasefire_FromDate == data.TRRTO_Ceasefire_FromDate && a.TRRTO_Ceasefire_ToDate == data.TRRTO_Ceasefire_ToDate).ToList();
                    if (chek_duplicate.Count == 0)
                    {
                        chat.MI_Id = data.MI_Id;
                        chat.TRMV_Id = data.TRMV_Id;
                        chat.TRRTO_GPS_GPRS_Fitted_date = data.TRRTO_GPS_GPRS_Fitted_date;
                        chat.TRRTO_Insurance_FromDate = data.TRRTO_Insurance_FromDate;
                        chat.TRRTO_Insurance_Todate = data.TRRTO_Insurance_Todate;
                        chat.TRRTO_Company_Name = data.TRRTO_Company_Name;
                        chat.TRRTO_Tax_FromDate = data.TRRTO_Tax_FromDate;
                        chat.TRRTO_Tax_ToDate = data.TRRTO_Tax_ToDate;
                        chat.TRRTO_FC_FromDate = data.TRRTO_FC_FromDate;
                        chat.TRRTO_FC_ToDate = data.TRRTO_FC_ToDate;
                        chat.TRRTO_Permit_FromDate = data.TRRTO_Permit_FromDate;
                        chat.TRRTO_Permit_ToDate = data.TRRTO_Permit_ToDate;
                   
                        chat.TRRTO_Emission_FromDate = data.TRRTO_Emission_FromDate;
                        chat.TRRTO_Emission_ToDate = data.TRRTO_Emission_ToDate;
                        chat.TRRTO_Ceasefire_FromDate = data.TRRTO_Ceasefire_FromDate;
                        chat.TRRTO_Ceasefire_ToDate = data.TRRTO_Ceasefire_ToDate;

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

        public RTODetailsDTO edit(RTODetailsDTO data)
        {
            try
            {
               data.geteditdata = _context.RTODetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRTO_Id == data.TRRTO_Id).OrderByDescending(a => a.TRRTO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }
    }
}
