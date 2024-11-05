using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class VahicalCertificateImpl : Interfaces.VahicalCertificateInterface
    {
        public TransportContext _context;
        public ILogger<VahicalCertificateDTO> _log;
        DomainModelMsSqlServerContext _db;
        public VahicalCertificateImpl(ILogger<VahicalCertificateDTO> log, TransportContext context, DomainModelMsSqlServerContext db)
        {
            _log = log;
            _context = context;
            _db = db;
        }

        public VahicalCertificateDTO getdata(int id)
        {
            VahicalCertificateDTO data = new VahicalCertificateDTO();
            try
            {


                data.fillvahicleno = _context.Master_VehicleDMO.Where(t => t.MI_Id == id && t.TRMV_ActiveFlag == true).ToArray();
                var paymentMode = _db.IVRM_ModeOfPaymentDMO.Where(d => d.MI_Id == id && d.IVRMMOD_ActiveFlag == true).ToList();
                if (paymentMode.Count > 0)
                {
                    data.modeOfPaymentList = paymentMode.ToArray();
                }


                data.getloaddata = (from a in _context.VahicalCertificateDMO
                                    from b in _context.Master_VehicleDMO

                                    where a.MI_Id == b.MI_Id && a.MI_Id == id && a.TRMV_Id == b.TRMV_Id && b.TRMV_ActiveFlag == true
                                    select new VahicalCertificateDTO
                                    {
                                        TRVCT_Id = a.TRVCT_Id,
                                        TRMV_VehicleNo = b.TRMV_VehicleNo,
                                        TRVCT_CertificateType = a.TRVCT_CertificateType,
                                        TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                        TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                        TRVCT_Remarks = a.TRVCT_Remarks,
                                        TRVCT_AmountPaid = a.TRVCT_AmountPaid,
                                        TRVCT_ChequeDDDate = a.TRVCT_ChequeDDDate,
                                        TRVCT_ModeOfPayment = a.TRVCT_ModeOfPayment,
                                        TRVCT_ChequeDDNo = a.TRVCT_ChequeDDNo,
                                      

                                    }).OrderByDescending(j=>j.TRVCT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }


        public VahicalCertificateDTO Onvahiclechange(VahicalCertificateDTO data)
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


        public VahicalCertificateDTO deleterecord(VahicalCertificateDTO data)
        {
            try
            {
                var docresult = _context.VahicalCertificateDocumentDMO.Single(a => a.TRVCT_Id == data.TRVCT_Id);
                _context.Remove(docresult);
                _context.SaveChanges();
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
        public VahicalCertificateDTO savedata(VahicalCertificateDTO data)
        {
            try
            {
                string mob = "";
                string eml = "";
                

                for (int i = 0; i < data.mobile_list_dto.Length; i++)
                {
                    if (i==0)
                    {
                        mob = data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                    }
                    else
                    {
                        mob = mob+","+data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                    }
                }

                for (int i = 0; i < data.email_list_dto.Length; i++)
                {
                    if (i == 0)
                    {
                        eml = data.email_list_dto[i].HRMEM_EmailId.ToString();
                    }
                    else
                    {
                        eml = eml + "," + data.email_list_dto[i].HRMEM_EmailId.ToString();
                    }
                }


                if (data.TRVCT_Id > 0)
                {
                  


                    var dup = _context.VahicalCertificateDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_Id == data.TRMV_Id && a.TRVCT_CertificateType == data.TRVCT_CertificateType && a.TRVCT_ObtainedDate == data.TRVCT_ObtainedDate && a.TRVCT_ValidTillDate == data.TRVCT_ValidTillDate  && a.TRVCT_Id != data.TRVCT_Id).ToList();
                    if (dup.Count == 0)
                    {
                        var result = _context.VahicalCertificateDMO.Single(a => a.MI_Id == data.MI_Id && a.TRVCT_Id == data.TRVCT_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRVCT_AmountPaid = data.TRVCT_AmountPaid;
                        result.TRVCT_CertificateType = data.TRVCT_CertificateType;
                        result.TRVCT_ChequeDDDate = data.TRVCT_ChequeDDDate;
                        result.TRVCT_ChequeDDNo = data.TRVCT_ChequeDDNo;
                        result.TRVCT_ModeOfPayment = data.TRVCT_ModeOfPayment;
                        result.TRVCT_ObtainedDate = data.TRVCT_ObtainedDate;
                        result.TRVCT_PaymentReferenceNo = data.TRVCT_PaymentReferenceNo;
                        result.TRVCT_Remarks = data.TRVCT_Remarks;
                        result.TRVCT_ValidTillDate = data.TRVCT_ValidTillDate;
                        result.TRVCT_VECompanyName = data.TRVCT_VECompanyName;
                        result.TRVCT_RTOName = data.TRVCT_RTOName;
                        result.TRVCT_InsuranceCompany = data.TRVCT_InsuranceCompany;
                        result.TRVCT_PolicyNo = data.TRVCT_PolicyNo;
                        result.TRVCT_eMailAlertTo = eml;
                        result.TRVCT_SMSAlertToNo = mob;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);

                        if (data.uploaddocments != null && data.uploaddocments.Length > 0)
                        {
                            foreach (var c in data.uploaddocments)
                            {
                                var contactExistsP = _context.Database.ExecuteSqlCommand("TR_Vehicle_Certificates_Documents_Insert @p0,@p1,@p2,@p3", data.TRVCT_Id, c.TRVCTD_Id, c.TRVCTD_FileName, c.TRVCTD_FileLocation);
                                if (contactExistsP > 0)
                                {
                                    data.message = "Add";
                                }
                                else
                                {
                                    data.message = "notAdded";
                                }

                            }
                        }


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
                    VahicalCertificateDMO chat = new VahicalCertificateDMO();
                    var chek_duplicate = _context.VahicalCertificateDMO.Where(a => a.MI_Id == data.MI_Id  && a.TRVCT_CertificateType == data.TRVCT_CertificateType && a.TRVCT_ObtainedDate == data.TRVCT_ObtainedDate && a.TRVCT_ValidTillDate == data.TRVCT_ValidTillDate).ToList();
                    if (chek_duplicate.Count == 0)
                    {
                        chat.MI_Id = data.MI_Id;
                        chat.TRMV_Id = data.TRMV_Id;
                       chat.TRVCT_AmountPaid = data.TRVCT_AmountPaid;
                       chat.TRVCT_CertificateType = data.TRVCT_CertificateType;
                       chat.TRVCT_ChequeDDDate = data.TRVCT_ChequeDDDate;
                       chat.TRVCT_ChequeDDNo = data.TRVCT_ChequeDDNo;
                       chat.TRVCT_ModeOfPayment = data.TRVCT_ModeOfPayment;
                       chat.TRVCT_ObtainedDate = data.TRVCT_ObtainedDate;
                       chat.TRVCT_PaymentReferenceNo = data.TRVCT_PaymentReferenceNo;
                       chat.TRVCT_Remarks = data.TRVCT_Remarks;
                        chat.TRVCT_ValidTillDate = data.TRVCT_ValidTillDate;
                        chat.TRVCT_VECompanyName = data.TRVCT_VECompanyName;
                        chat.TRVCT_RTOName = data.TRVCT_RTOName;
                        chat.TRVCT_InsuranceCompany = data.TRVCT_InsuranceCompany;
                        chat.TRVCT_PolicyNo = data.TRVCT_PolicyNo;
                        chat.CreatedDate = DateTime.Now;
                        chat.UpdatedDate = DateTime.Now;
                        chat.TRVCT_eMailAlertTo = eml;
                        chat.TRVCT_SMSAlertToNo = mob;
                        _context.Add(chat);

                        int n = _context.SaveChanges();

                        if (data.uploaddocments != null && data.uploaddocments.Length > 0)
                        {
                            foreach (var c in data.uploaddocments)
                            {
                                var contactExistsP = _context.Database.ExecuteSqlCommand("TR_Vehicle_Certificates_Documents_Insert @p0,@p1,@p2,@p3", chat.TRVCT_Id, 0, c.TRVCTD_FileName,  c.TRVCTD_FileLocation);
                                if (contactExistsP > 0)
                                {
                                    data.message = "Add";
                                }
                                else
                                {
                                    data.message = "notAdded";
                                }

                            }
                        }


                        
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

        public VahicalCertificateDTO edit(VahicalCertificateDTO data)
        {
            try
            {
              var geteditdata = _context.VahicalCertificateDMO.Where(a => a.MI_Id == data.MI_Id && a.TRVCT_Id == data.TRVCT_Id).OrderByDescending(a => a.TRVCT_Id).ToList();
                data.geteditdata = geteditdata.ToArray();

                data.documentdetails = _context.VahicalCertificateDocumentDMO.Where(a => a.TRVCT_Id == data.TRVCT_Id).ToArray();

                if (geteditdata.Count > 0)
                {
                    if (geteditdata[0].TRVCT_SMSAlertToNo != null && geteditdata[0].TRVCT_SMSAlertToNo != "")
                    {
                        List<string> mobilevv = new List<string>(geteditdata[0].TRVCT_SMSAlertToNo.Split(','));
                        mobilevv.Reverse();




                        data.mobilenolist = mobilevv.ToArray();


                    }

                    if (geteditdata[0].TRVCT_eMailAlertTo != null && geteditdata[0].TRVCT_eMailAlertTo != "")
                    {
                        List<string> eevv = new List<string>(geteditdata[0].TRVCT_eMailAlertTo.Split(','));
                        eevv.Reverse();




                        data.emailist = eevv.ToArray();


                    }


                }
             

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char getdata" + ex.Message);
            }
            return data;
        }
    }
}
