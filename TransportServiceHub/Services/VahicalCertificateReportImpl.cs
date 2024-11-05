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
    public class VahicalCertificateReportImpl : Interfaces.VahicalCertificateReportInterface
    {
        public TransportContext _context;
        public ILogger<VahicalCertificateReportDTO> _log;
        DomainModelMsSqlServerContext _db;
        public VahicalCertificateReportImpl(ILogger<VahicalCertificateReportDTO> log, TransportContext context, DomainModelMsSqlServerContext db)
        {
            _log = log;
            _context = context;
            _db = db;
        }

        public VahicalCertificateReportDTO getdata(int id)
        {
            VahicalCertificateReportDTO data = new VahicalCertificateReportDTO();
            try
            {

                data.fillvahicletype = _context.MasterVehicleTypeDMO.Where(t => t.MI_Id == id && t.TRMVT_ActiveFlg == true).ToArray();



                //data.fillvahicleno = _context.Master_VehicleDMO.Where(t => t.MI_Id == id && t.TRMV_ActiveFlag == true).ToArray();
                //var paymentMode = _db.IVRM_ModeOfPaymentDMO.Where(d => d.MI_Id == id && d.IVRMMOD_ActiveFlag == true).ToList();
                //if (paymentMode.Count > 0)
                //{
                //    data.modeOfPaymentList = paymentMode.ToArray();
                //}


                //data.getloaddata = (from a in _context.VahicalCertificateDMO
                //                    from b in _context.Master_VehicleDMO

                //                    where a.MI_Id == b.MI_Id && a.MI_Id == id && a.TRMV_Id == b.TRMV_Id && b.TRMV_ActiveFlag == true
                //                    select new VahicalCertificateReportDTO
                //                    {
                //                        TRVCT_Id = a.TRVCT_Id,
                //                        TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                        TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                        TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                        TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                        TRVCT_Remarks = a.TRVCT_Remarks,
                //                        TRVCT_AmountPaid = a.TRVCT_AmountPaid,
                //                        TRVCT_ChequeDDDate = a.TRVCT_ChequeDDDate,
                //                        TRVCT_ModeOfPayment = a.TRVCT_ModeOfPayment,
                //                        TRVCT_ChequeDDNo = a.TRVCT_ChequeDDNo,
                                      

                //                    }).OrderByDescending(j=>j.TRVCT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }


        public VahicalCertificateReportDTO Onvahiclechange(VahicalCertificateReportDTO data)
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


        public VahicalCertificateReportDTO deleterecord(VahicalCertificateReportDTO data)
        {
            try
            {
                var result = _context.VahicalCertificateDMO.Single(t => t.MI_Id == data.MI_Id && t.TRVCT_Id == data.TRVCT_Id);
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
        public VahicalCertificateReportDTO savedata(VahicalCertificateReportDTO data)
        {
            try
            {
                List<long> vhid = new List<long>();
                if (data.vhlid.Length>0)
                {

                    foreach (var item in data.vhlid)
                    {
                        vhid.Add(item.TRMV_Id);
                    }
                }
                List<string> certtype = new List<string>();
                if (data.ctype.Length > 0)
                {

                    foreach (var item in data.ctype)
                    {
                        certtype.Add(item.type);
                    }
                }
                if (data.TRVCT_ObtainedDate==null && data.TRVCT_ValidTillDate==null )
                {
                    data.getloaddata = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO

                                        where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMV_Id == b.TRMV_Id && b.TRMV_ActiveFlag == true 
                                       // && a.TRVCT_CertificateType.Contains(data.TRVCT_CertificateType) 
                                        && vhid.Contains(a.TRMV_Id)  && certtype.Contains(a.TRVCT_CertificateType)
                                        select new VahicalCertificateReportDTO
                                        {
                                            TRVCT_Id = a.TRVCT_Id,
                                            TRMV_Id=b.TRMV_Id,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                            TRVCT_Remarks = a.TRVCT_Remarks,
                                            TRVCT_AmountPaid = a.TRVCT_AmountPaid,
                                            TRVCT_ChequeDDDate = a.TRVCT_ChequeDDDate,
                                            TRVCT_ModeOfPayment = a.TRVCT_ModeOfPayment,
                                            TRVCT_ChequeDDNo = a.TRVCT_ChequeDDNo,
                                            TRMV_CompanyName = b.TRMV_CompanyName,
                                            TRVCT_InsuranceCompany = a.TRVCT_InsuranceCompany,
                                            TRVCT_PolicyNo = a.TRVCT_PolicyNo,
                                            TRVCT_VECompanyName = a.TRVCT_VECompanyName,
                                            TRVCT_RTOName = a.TRVCT_RTOName

                                        }).Distinct().OrderByDescending(j => j.TRVCT_ObtainedDate).ToArray();
                }


                else
                {
                    data.getloaddata = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO

                                        where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMV_Id == b.TRMV_Id
                                        && b.TRMV_ActiveFlag == true
                                         // && a.TRVCT_CertificateType.Contains(data.TRVCT_CertificateType) 
                                         && vhid.Contains(a.TRMV_Id) && certtype.Contains(a.TRVCT_CertificateType)
                                        && a.TRVCT_ObtainedDate>=data.TRVCT_ObtainedDate && a.TRVCT_ObtainedDate<= data.TRVCT_ValidTillDate
                                        select new VahicalCertificateReportDTO
                                        {
                                            TRVCT_Id = a.TRVCT_Id,
                                            TRMV_Id = b.TRMV_Id,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                            TRVCT_Remarks = a.TRVCT_Remarks,
                                            TRVCT_AmountPaid = a.TRVCT_AmountPaid,
                                            TRVCT_ChequeDDDate = a.TRVCT_ChequeDDDate,
                                            TRVCT_ModeOfPayment = a.TRVCT_ModeOfPayment,
                                            TRVCT_ChequeDDNo = a.TRVCT_ChequeDDNo,
                                            TRMV_CompanyName = b.TRMV_CompanyName,
                                            TRVCT_InsuranceCompany = a.TRVCT_InsuranceCompany,
                                            TRVCT_PolicyNo = a.TRVCT_PolicyNo,
                                            TRVCT_VECompanyName = a.TRVCT_VECompanyName,
                                            TRVCT_RTOName = a.TRVCT_RTOName

                                        }).Distinct().OrderByDescending(j => j.TRVCT_ObtainedDate).ToArray();
                }
             
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }

        public VahicalCertificateReportDTO edit(VahicalCertificateReportDTO data)
        {
            try
            {
               data.geteditdata = _context.VahicalCertificateDMO.Where(a => a.MI_Id == data.MI_Id && a.TRVCT_Id == data.TRVCT_Id).OrderByDescending(a => a.TRVCT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }
    }
}
