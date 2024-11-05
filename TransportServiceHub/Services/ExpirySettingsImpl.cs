using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Transport;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class ExpirySettingsImpl : Interfaces.ExpirySettingsInterface
    {
        public TransportContext _context;
        ILogger<ExpirySettingsImpl> _areaimpl;
public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public ExpirySettingsImpl(ILogger<ExpirySettingsImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;

        }

        public ExpirySettingsDTO getdata(int id)
        {
            ExpirySettingsDTO data = new ExpirySettingsDTO();
            try
            {
                data.getdatadetails = _context.TransportStandardsDMO.Where(a => a.MI_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public ExpirySettingsDTO savedata(ExpirySettingsDTO data)
        {
            try
            {
                if (data.TRC_Id > 0)
                {
                        var result = _context.TransportStandardsDMO.Single(a => a.MI_Id == data.MI_Id && a.TRC_Id == data.TRC_Id);
                    
                    result.TRC_DLExpReminderDays = data.TRC_DLExpReminderDays;
                    result.TRC_EmmisionExpMonths = data.TRC_EmmisionExpMonths;
                    result.TRC_EmmisionExpDays = data.TRC_EmmisionExpDays;
                    result.TRC_TaxExpMonths = data.TRC_TaxExpMonths;
                    result.TRC_TaxExpDays = data.TRC_TaxExpDays;
                    result.TRC_FitnessExpMonths = data.TRC_FitnessExpMonths;
                    result.TRC_FitnessExpDays = data.TRC_FitnessExpDays;
                    result.TRC_SpeedControlMonths = data.TRC_SpeedControlMonths;
                    result.TRC_SpeedControlDays = data.TRC_SpeedControlDays;
                    result.TRC_PermitMonths = data.TRC_PermitMonths;
                    result.TRC_PermitDays = data.TRC_PermitDays;
                    result.TRC_CeaseFireMonths = data.TRC_CeaseFireMonths;
                    result.TRC_CeaseFireDays = data.TRC_CeaseFireDays;
                    result.TRC_InsuranceMonths = data.TRC_InsuranceMonths;
                    result.TRC_InsuranceDays = data.TRC_InsuranceDays;
                    result.TRC_GreenTaxMonths = data.TRC_GreenTaxMonths;
                    result.TRC_GreenTaxDays = data.TRC_GreenTaxDays;
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
                    TransportStandardsDMO areadmo = new TransportStandardsDMO();
                    areadmo.MI_Id = data.MI_Id;
                    areadmo.TRC_DLExpReminderDays = data.TRC_DLExpReminderDays;
                    areadmo.TRC_EmmisionExpMonths = data.TRC_EmmisionExpMonths;
                    areadmo.TRC_EmmisionExpDays = data.TRC_EmmisionExpDays;
                    areadmo.TRC_TaxExpMonths = data.TRC_TaxExpMonths;
                    areadmo.TRC_TaxExpDays = data.TRC_TaxExpDays;
                    areadmo.TRC_FitnessExpMonths = data.TRC_FitnessExpMonths;
                    areadmo.TRC_FitnessExpDays = data.TRC_FitnessExpDays;
                    areadmo.TRC_SpeedControlMonths = data.TRC_SpeedControlMonths;
                    areadmo.TRC_SpeedControlDays = data.TRC_SpeedControlDays;
                    areadmo.TRC_PermitMonths = data.TRC_PermitMonths;
                    areadmo.TRC_PermitDays = data.TRC_PermitDays;
                    areadmo.TRC_CeaseFireMonths = data.TRC_CeaseFireMonths;
                    areadmo.TRC_CeaseFireDays = data.TRC_CeaseFireDays;
                    areadmo.TRC_InsuranceMonths = data.TRC_InsuranceMonths;
                    areadmo.TRC_InsuranceDays = data.TRC_InsuranceDays;
                    areadmo.TRC_GreenTaxMonths = data.TRC_GreenTaxMonths;
                    areadmo.TRC_GreenTaxDays = data.TRC_GreenTaxDays;
                    areadmo.UpdatedDate = DateTime.Now;
                    areadmo.CreatedDate = DateTime.Now;
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
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }
        public ExpirySettingsDTO getdatadetails(ExpirySettingsDTO data)
        {
            try
            {
                var result = _context.TransportStandardsDMO.Single(a => a.MI_Id == data.MI_Id );/*&& a.TRC_Id == data.TRC_Id*/
                var diverlic = _context.MasterDriverDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMD_ActiveFlg == true).ToList();
                DateTime fromtoday = DateTime.Now.Date;
                //for driver license
                DateTime dltotoday = DateTime.Now.Date;
                dltotoday = fromtoday.AddDays(result.TRC_DLExpReminderDays);
                if (diverlic.Count>0)
                {
                 var DLmainreminderlist= diverlic.Where(t => t.TRMD_DLExpiryDate.Date >= fromtoday && t.TRMD_DLExpiryDate.Date <= dltotoday).ToList();
                    
                    var DLmainexpiredlist = diverlic.Where(t => t.TRMD_DLExpiryDate.Date <= fromtoday).ToList();
                    data.DLmainreminderlist = DLmainreminderlist.ToArray();
                    data.DLmainexpiredlist = DLmainexpiredlist.ToArray();
                }
                //For vahical insurance
                DateTime instotoday = DateTime.Now.Date;
                instotoday = fromtoday.AddDays(result.TRC_InsuranceDays);
                var insuranceReminder = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Insurance" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday &&a.TRVCT_ValidTillDate<= instotoday
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType=a.TRVCT_CertificateType,
                                             TRMV_VehicleNo=b.TRMV_VehicleNo,
                                             TRMV_VehicleName=b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate=a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate=a.TRVCT_ValidTillDate,
                                             remainingdays= (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                         }).ToList();


                var insuranceExpired = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Insurance" && b.TRMV_ActiveFlag == true &&  a.TRVCT_ValidTillDate <= fromtoday
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                         }).ToList();
                //end 
                //for Vehicle Emission Test
                DateTime emstotoday = DateTime.Now.Date;
                emstotoday = fromtoday.AddDays(result.TRC_EmmisionExpDays);
                var EmissionReminder = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Emission Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= emstotoday
                                        select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                             remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                         }).ToList();


                var EmissionExpired = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Emission Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                        }).ToList();

                //end

                //for Vehicle CeaseFire
                DateTime CeaseFiretotoday = DateTime.Now.Date;
                CeaseFiretotoday = fromtoday.AddDays(result.TRC_CeaseFireDays);
                var CeaseFireReminder = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle CeaseFire" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= CeaseFiretotoday
                                         select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                            remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                        }).ToList();


                var CeaseFireExpired = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle CeaseFire" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                        }).ToList();

                //end
                //for Vehicle Tax
                DateTime VehicleTaxtotoday = DateTime.Now.Date;
                VehicleTaxtotoday = fromtoday.AddDays(result.TRC_TaxExpDays);
                var VehicleTaxReminder = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= VehicleTaxtotoday
                                          select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                             remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                         }).ToList();


                var VehicleTaxExpired = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                        }).ToList();

                //end

                //for Vehicle Speed
                DateTime VehicleSpeedtotoday = DateTime.Now.Date;
                VehicleSpeedtotoday = fromtoday.AddDays(result.TRC_SpeedControlDays);
                var VehicleSpeedReminder = (from a in _context.VahicalCertificateDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Speed" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= VehicleSpeedtotoday
                                            select new ExpirySettingsDTO
                                          {
                                              TRVCT_CertificateType = a.TRVCT_CertificateType,
                                              TRMV_VehicleNo = b.TRMV_VehicleNo,
                                              TRMV_VehicleName = b.TRMV_VehicleName,
                                              TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                              TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                              remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                          }).ToList();


                var VehicleSpeedExpired = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Speed" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                         }).ToList();

                //end

                //for Vehicle Fitness Test
                DateTime Vehiclefitnesstotoday = DateTime.Now.Date;
                Vehiclefitnesstotoday = fromtoday.AddDays(result.TRC_FitnessExpDays);
                var VehiclefitnessReminder = (from a in _context.VahicalCertificateDMO
                                            from b in _context.Master_VehicleDMO
                                            where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Fitness Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Vehiclefitnesstotoday
                                              select new ExpirySettingsDTO
                                            {
                                                TRVCT_CertificateType = a.TRVCT_CertificateType,
                                                TRMV_VehicleNo = b.TRMV_VehicleNo,
                                                TRMV_VehicleName = b.TRMV_VehicleName,
                                                TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                                TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                                remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                            }).ToList();


                var VehiclefitnessExpired = (from a in _context.VahicalCertificateDMO
                                           from b in _context.Master_VehicleDMO
                                           where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Fitness Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                           select new ExpirySettingsDTO
                                           {
                                               TRVCT_CertificateType = a.TRVCT_CertificateType,
                                               TRMV_VehicleNo = b.TRMV_VehicleNo,
                                               TRMV_VehicleName = b.TRMV_VehicleName,
                                               TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                               TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                           }).ToList();

                //end
                //for Vehicle Permit
                DateTime Permittotoday = DateTime.Now.Date;
                Permittotoday = fromtoday.AddDays(result.TRC_PermitDays);
                var PermitReminder = (from a in _context.VahicalCertificateDMO
                                              from b in _context.Master_VehicleDMO
                                              where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Permit" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Permittotoday
                                              select new ExpirySettingsDTO
                                              {
                                                  TRVCT_CertificateType = a.TRVCT_CertificateType,
                                                  TRMV_VehicleNo = b.TRMV_VehicleNo,
                                                  TRMV_VehicleName = b.TRMV_VehicleName,
                                                  TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                                  TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                                  remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                              }).ToList();


                var PermitExpired = (from a in _context.VahicalCertificateDMO
                                             from b in _context.Master_VehicleDMO
                                             where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Permit" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                             select new ExpirySettingsDTO
                                             {
                                                 TRVCT_CertificateType = a.TRVCT_CertificateType,
                                                 TRMV_VehicleNo = b.TRMV_VehicleNo,
                                                 TRMV_VehicleName = b.TRMV_VehicleName,
                                                 TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                                 TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                             }).ToList();

                //end

                //for Vehicle Green Tax
                DateTime Greentotoday = DateTime.Now.Date;
                Greentotoday = fromtoday.AddDays(result.TRC_GreenTaxDays);
                var GreenReminder = (from a in _context.VahicalCertificateDMO
                                      from b in _context.Master_VehicleDMO
                                      where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Green Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Greentotoday
                                      select new ExpirySettingsDTO
                                      {
                                          TRVCT_CertificateType = a.TRVCT_CertificateType,
                                          TRMV_VehicleNo = b.TRMV_VehicleNo,
                                          TRMV_VehicleName = b.TRMV_VehicleName,
                                          TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                          TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                          remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                      }).ToList();


                var GreenExpired = (from a in _context.VahicalCertificateDMO
                                     from b in _context.Master_VehicleDMO
                                     where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Green Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                     select new ExpirySettingsDTO
                                     {
                                         TRVCT_CertificateType = a.TRVCT_CertificateType,
                                         TRMV_VehicleNo = b.TRMV_VehicleNo,
                                         TRMV_VehicleName = b.TRMV_VehicleName,
                                         TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                         TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                     }).ToList();

                //end

                List<ExpirySettingsDTO> vahicalexpreminder = new List<ExpirySettingsDTO>();
                List<ExpirySettingsDTO> vahicalexp = new List<ExpirySettingsDTO>();

                if (insuranceReminder.Count>0)
                {
                    foreach (var item in insuranceReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (EmissionReminder.Count > 0)
                {
                    foreach (var item in EmissionReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (CeaseFireReminder.Count > 0)
                {
                    foreach (var item in CeaseFireReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (VehicleTaxReminder.Count > 0)
                {
                    foreach (var item in VehicleTaxReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (VehicleSpeedReminder.Count > 0)
                {
                    foreach (var item in VehicleSpeedReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (VehiclefitnessReminder.Count > 0)
                {
                    foreach (var item in VehiclefitnessReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (PermitReminder.Count > 0)
                {
                    foreach (var item in PermitReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }
                if (GreenReminder.Count > 0)
                {
                    foreach (var item in GreenReminder)
                    {
                        vahicalexpreminder.Add(item);
                    }
                }

                data.vahicalexpreminder = vahicalexpreminder.ToArray();


                if (insuranceExpired.Count > 0)
                {
                    foreach (var item in insuranceExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (EmissionExpired.Count > 0)
                {
                    foreach (var item in EmissionExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (CeaseFireExpired.Count > 0)
                {
                    foreach (var item in CeaseFireExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (VehicleTaxExpired.Count > 0)
                {
                    foreach (var item in VehicleTaxExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (VehicleSpeedExpired.Count > 0)
                {
                    foreach (var item in VehicleSpeedExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (VehiclefitnessExpired.Count > 0)
                {
                    foreach (var item in VehiclefitnessExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (PermitExpired.Count > 0)
                {
                    foreach (var item in PermitExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }
                if (GreenExpired.Count > 0)
                {
                    foreach (var item in GreenExpired)
                    {
                        vahicalexp.Add(item);
                    }
                }

                data.vahicalexp = vahicalexp.ToArray();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public ExpirySettingsDTO getsmsdetails(ExpirySettingsDTO data)
        {
            try
            {
                var result = _context.TransportStandardsDMO.Single(a => a.MI_Id == data.MI_Id);/*&& a.TRC_Id == data.TRC_Id*/
                var diverlic = _context.MasterDriverDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMD_ActiveFlg == true).ToList();
                DateTime fromtoday = DateTime.Now.Date;
                //for driver license
                DateTime dltotoday = DateTime.Now.Date;
                dltotoday = fromtoday.AddDays(result.TRC_DLExpReminderDays);
                if (diverlic.Count > 0)
                {
                    var DLmainreminderlist = diverlic.Where(t => t.TRMD_DLExpiryDate.Date >= fromtoday && t.TRMD_DLExpiryDate.Date <= dltotoday).ToList();

                    var DLmainexpiredlist = diverlic.Where(t => t.TRMD_DLExpiryDate.Date <= fromtoday).ToList();
                    data.DLmainreminderlist = DLmainreminderlist.ToArray();
                    data.DLmainexpiredlist = DLmainexpiredlist.ToArray();
                }
                //For vahical insurance
                DateTime instotoday = DateTime.Now.Date;
                instotoday = fromtoday.AddDays(result.TRC_InsuranceDays);
                var insuranceReminder = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Insurance" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= instotoday
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                             remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                         }).ToList();


                var insuranceExpired = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Insurance" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                        }).ToList();
                //end 
                //for Vehicle Emission Test
                DateTime emstotoday = DateTime.Now.Date;
                emstotoday = fromtoday.AddDays(result.TRC_EmmisionExpDays);
                var EmissionReminder = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Emission Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= emstotoday
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                            remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                        }).ToList();


                var EmissionExpired = (from a in _context.VahicalCertificateDMO
                                       from b in _context.Master_VehicleDMO
                                       where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Emission Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                       select new ExpirySettingsDTO
                                       {
                                           TRVCT_CertificateType = a.TRVCT_CertificateType,
                                           TRMV_VehicleNo = b.TRMV_VehicleNo,
                                           TRMV_VehicleName = b.TRMV_VehicleName,
                                           TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                           TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                       }).ToList();

                //end

                //for Vehicle CeaseFire
                DateTime CeaseFiretotoday = DateTime.Now.Date;
                CeaseFiretotoday = fromtoday.AddDays(result.TRC_CeaseFireDays);
                var CeaseFireReminder = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle CeaseFire" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= CeaseFiretotoday
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                             remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                         }).ToList();


                var CeaseFireExpired = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle CeaseFire" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                                            TRMV_VehicleName = b.TRMV_VehicleName,
                                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                        }).ToList();

                //end
                //for Vehicle Tax
                DateTime VehicleTaxtotoday = DateTime.Now.Date;
                VehicleTaxtotoday = fromtoday.AddDays(result.TRC_TaxExpDays);
                var VehicleTaxReminder = (from a in _context.VahicalCertificateDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= VehicleTaxtotoday
                                          select new ExpirySettingsDTO
                                          {
                                              TRVCT_CertificateType = a.TRVCT_CertificateType,
                                              TRMV_VehicleNo = b.TRMV_VehicleNo,
                                              TRMV_VehicleName = b.TRMV_VehicleName,
                                              TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                              TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                              remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                          }).ToList();


                var VehicleTaxExpired = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                                             TRMV_VehicleName = b.TRMV_VehicleName,
                                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                         }).ToList();

                //end

                //for Vehicle Speed
                DateTime VehicleSpeedtotoday = DateTime.Now.Date;
                VehicleSpeedtotoday = fromtoday.AddDays(result.TRC_SpeedControlDays);
                var VehicleSpeedReminder = (from a in _context.VahicalCertificateDMO
                                            from b in _context.Master_VehicleDMO
                                            where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Speed" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= VehicleSpeedtotoday
                                            select new ExpirySettingsDTO
                                            {
                                                TRVCT_CertificateType = a.TRVCT_CertificateType,
                                                TRMV_VehicleNo = b.TRMV_VehicleNo,
                                                TRMV_VehicleName = b.TRMV_VehicleName,
                                                TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                                TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                                remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                            }).ToList();


                var VehicleSpeedExpired = (from a in _context.VahicalCertificateDMO
                                           from b in _context.Master_VehicleDMO
                                           where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Speed" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                           select new ExpirySettingsDTO
                                           {
                                               TRVCT_CertificateType = a.TRVCT_CertificateType,
                                               TRMV_VehicleNo = b.TRMV_VehicleNo,
                                               TRMV_VehicleName = b.TRMV_VehicleName,
                                               TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                               TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                           }).ToList();

                //end

                //for Vehicle Fitness Test
                DateTime Vehiclefitnesstotoday = DateTime.Now.Date;
                Vehiclefitnesstotoday = fromtoday.AddDays(result.TRC_FitnessExpDays);
                var VehiclefitnessReminder = (from a in _context.VahicalCertificateDMO
                                              from b in _context.Master_VehicleDMO
                                              where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Fitness Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Vehiclefitnesstotoday
                                              select new ExpirySettingsDTO
                                              {
                                                  TRVCT_CertificateType = a.TRVCT_CertificateType,
                                                  TRMV_VehicleNo = b.TRMV_VehicleNo,
                                                  TRMV_VehicleName = b.TRMV_VehicleName,
                                                  TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                                  TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                                  remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                              }).ToList();


                var VehiclefitnessExpired = (from a in _context.VahicalCertificateDMO
                                             from b in _context.Master_VehicleDMO
                                             where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Fitness Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                             select new ExpirySettingsDTO
                                             {
                                                 TRVCT_CertificateType = a.TRVCT_CertificateType,
                                                 TRMV_VehicleNo = b.TRMV_VehicleNo,
                                                 TRMV_VehicleName = b.TRMV_VehicleName,
                                                 TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                                 TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                             }).ToList();

                //end
                //for Vehicle Permit
                DateTime Permittotoday = DateTime.Now.Date;
                Permittotoday = fromtoday.AddDays(result.TRC_PermitDays);
                var PermitReminder = (from a in _context.VahicalCertificateDMO
                                      from b in _context.Master_VehicleDMO
                                      where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Permit" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Permittotoday
                                      select new ExpirySettingsDTO
                                      {
                                          TRVCT_CertificateType = a.TRVCT_CertificateType,
                                          TRMV_VehicleNo = b.TRMV_VehicleNo,
                                          TRMV_VehicleName = b.TRMV_VehicleName,
                                          TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                          TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                          remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                      }).ToList();


                var PermitExpired = (from a in _context.VahicalCertificateDMO
                                     from b in _context.Master_VehicleDMO
                                     where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Permit" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                     select new ExpirySettingsDTO
                                     {
                                         TRVCT_CertificateType = a.TRVCT_CertificateType,
                                         TRMV_VehicleNo = b.TRMV_VehicleNo,
                                         TRMV_VehicleName = b.TRMV_VehicleName,
                                         TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                         TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                     }).ToList();

                //end

                //for Vehicle Green Tax
                DateTime Greentotoday = DateTime.Now.Date;
                Greentotoday = fromtoday.AddDays(result.TRC_GreenTaxDays);
                var GreenReminder = (from a in _context.VahicalCertificateDMO
                                     from b in _context.Master_VehicleDMO
                                     where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Green Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Greentotoday
                                     select new ExpirySettingsDTO
                                     {
                                         TRVCT_CertificateType = a.TRVCT_CertificateType,
                                         TRMV_VehicleNo = b.TRMV_VehicleNo,
                                         TRMV_VehicleName = b.TRMV_VehicleName,
                                         TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                         TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                                         remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                                     }).ToList();


                var GreenExpired = (from a in _context.VahicalCertificateDMO
                                    from b in _context.Master_VehicleDMO
                                    where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Green Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                                    select new ExpirySettingsDTO
                                    {
                                        TRVCT_CertificateType = a.TRVCT_CertificateType,
                                        TRMV_VehicleNo = b.TRMV_VehicleNo,
                                        TRMV_VehicleName = b.TRMV_VehicleName,
                                        TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                                        TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                                    }).ToList();

                //end

               

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public ExpirySettingsDTO jshsgetsmsdetails(ExpirySettingsDTO data)
        {
            try
            {
                var result = _context.TransportStandardsDMO.Single(a => a.MI_Id == data.MI_Id);/*&& a.TRC_Id == data.TRC_Id*/
                var diverlic = _context.MasterDriverDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMD_ActiveFlg == true).ToList();
                DateTime fromtoday = DateTime.Now.Date;
                //for driver license
                DateTime dltotoday = DateTime.Now.Date;
                dltotoday = fromtoday.AddDays(result.TRC_DLExpReminderDays);
                if (diverlic.Count > 0)
                {
                    var DLmainreminderlist = diverlic.Where(t => t.TRMD_DLExpiryDate.Date >= fromtoday && t.TRMD_DLExpiryDate.Date <= dltotoday).ToList();

                    if (DLmainreminderlist.Count >0)
                    {
                        foreach (var item in DLmainreminderlist)
                        {
                            if (item.TRMD_MobileNo!=null && item.TRMD_MobileNo!=0)
                            {
                                long mob = Convert.ToInt64(item.TRMD_MobileNo);
                               
                               string a =  sendSms_exp(data.MI_Id, mob, "TRNREMINDER", item.TRMD_Id, "LICENCE");
                            }
                          
                            if (item.TRMD_EmailId != null && item.TRMD_EmailId != "")
                            {
                                
                             string a = sendmail_exp(data.MI_Id, item.TRMD_EmailId, "TRNREMINDER", item.TRMD_Id, "LICENCE");
                            }
                        }
                    }


                    var DLmainexpiredlist = diverlic.Where(t => t.TRMD_DLExpiryDate.Date <= fromtoday).ToList();
                    if (DLmainexpiredlist.Count>0)
                    {
                        
                            foreach (var item in DLmainexpiredlist)
                            {
                                if (item.TRMD_MobileNo != null && item.TRMD_MobileNo != 0)
                                {
                                    long mob = Convert.ToInt64(item.TRMD_MobileNo);

                                  string a = sendSms_exp(data.MI_Id, mob, "TRNREMINDER", item.TRMD_Id, "LICENCE");
                                }

                                if (item.TRMD_EmailId != null && item.TRMD_EmailId != "")
                                {

                                    string a = sendmail_exp(data.MI_Id, item.TRMD_EmailId, "TRNREMINDER", item.TRMD_Id, "LICENCE");
                                }
                            }
                       
                    }
                   
                }
                //For vahical insurance
                DateTime instotoday = DateTime.Now.Date;
                instotoday = fromtoday.AddDays(result.TRC_InsuranceDays);
                var insuranceReminder = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Insurance" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= instotoday && a.MI_Id==data.MI_Id
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_Id = a.TRVCT_Id,
                                             TRMV_Id = a.TRMV_Id,
                                             TRVCT_SMSAlertToNo = a.TRVCT_SMSAlertToNo,
                                             TRVCT_eMailAlertTo = a.TRVCT_eMailAlertTo,
                                         }).Distinct().ToList();


                if (insuranceReminder.Count>0)
                {
                    foreach (var item in insuranceReminder)
                    {

                        if (item.TRVCT_SMSAlertToNo != null && item.TRVCT_SMSAlertToNo != "")
                        {
                            List<string> mobilevv = new List<string>(item.TRVCT_SMSAlertToNo.Split(','));
                            mobilevv.Reverse();
                            if (mobilevv.Count > 0)
                            {
                                foreach (var item1 in mobilevv)
                                {
                                    string a = sendSms_exp(data.MI_Id, Convert.ToInt64(item1), "TRNREMINDER", item.TRVCT_Id, "INSURANCE");
                                }

                            }

                        }

                        if (item.TRVCT_eMailAlertTo != null && item.TRVCT_eMailAlertTo != "")
                        {
                            List<string> emvv = new List<string>(item.TRVCT_eMailAlertTo.Split(','));
                            emvv.Reverse();
                            if (emvv.Count > 0)
                            {
                                foreach (var item1 in emvv)
                                {
                                    string a2 = sendmail_exp(data.MI_Id, item1, "TRNREMINDER", item.TRVCT_Id, "INSURANCE");
                                }

                            }

                        }
                        
                    }
                }


                var insuranceExpired = (from a in _context.VahicalCertificateDMO
                                        from b in _context.Master_VehicleDMO
                                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Insurance" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday && a.MI_Id == data.MI_Id
                                        select new ExpirySettingsDTO
                                        {
                                            TRVCT_Id = a.TRVCT_Id,
                                            TRMV_Id = a.TRMV_Id,
                                            TRVCT_SMSAlertToNo = a.TRVCT_SMSAlertToNo,
                                            TRVCT_eMailAlertTo = a.TRVCT_eMailAlertTo,

                                        }).Distinct().ToList();



                if (insuranceExpired.Count > 0)
                {
                    foreach (var item in insuranceExpired)
                    {


                        if (item.TRVCT_SMSAlertToNo != null && item.TRVCT_SMSAlertToNo != "")
                        {
                            List<string> mobilevv = new List<string>(item.TRVCT_SMSAlertToNo.Split(','));
                            mobilevv.Reverse();
                            if (mobilevv.Count > 0)
                            {
                                foreach (var item1 in mobilevv)
                                {
                                    string a = sendSms_exp(data.MI_Id, Convert.ToInt64(item1), "TRNREMINDER", item.TRVCT_Id, "INSURANCE");
                                }

                            }

                        }

                        if (item.TRVCT_eMailAlertTo != null && item.TRVCT_eMailAlertTo != "")
                        {
                            List<string> emvv = new List<string>(item.TRVCT_eMailAlertTo.Split(','));
                            emvv.Reverse();
                            if (emvv.Count > 0)
                            {
                                foreach (var item1 in emvv)
                                {
                                    string a2 = sendmail_exp(data.MI_Id, item1, "TRNREMINDER", item.TRVCT_Id, "INSURANCE");
                                }

                            }

                        }



                    }
                }



                //end 




                //for Vehicle Tax
                DateTime VehicleTaxtotoday = DateTime.Now.Date;
                VehicleTaxtotoday = fromtoday.AddDays(result.TRC_TaxExpDays);
                var VehicleTaxReminder = (from a in _context.VahicalCertificateDMO
                                          from b in _context.Master_VehicleDMO
                                          where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= VehicleTaxtotoday && a.MI_Id == data.MI_Id
                                          select new ExpirySettingsDTO
                                          {
                                              TRVCT_Id = a.TRVCT_Id,
                                              TRMV_Id = a.TRMV_Id,
                                              TRVCT_SMSAlertToNo = a.TRVCT_SMSAlertToNo,
                                              TRVCT_eMailAlertTo = a.TRVCT_eMailAlertTo,

                                          }).Distinct().ToList();


                if (VehicleTaxReminder.Count > 0)
                {
                    foreach (var item in VehicleTaxReminder)
                    {


                        if (item.TRVCT_SMSAlertToNo != null && item.TRVCT_SMSAlertToNo != "")
                        {
                            List<string> mobilevv = new List<string>(item.TRVCT_SMSAlertToNo.Split(','));
                            mobilevv.Reverse();
                            if (mobilevv.Count > 0)
                            {
                                foreach (var item1 in mobilevv)
                                {
                                    string a = sendSms_exp(data.MI_Id, Convert.ToInt64(item1), "TRNREMINDER", item.TRMV_Id, "TAX");
                                }

                            }

                        }

                        if (item.TRVCT_eMailAlertTo != null && item.TRVCT_eMailAlertTo != "")
                        {
                            List<string> emvv = new List<string>(item.TRVCT_eMailAlertTo.Split(','));
                            emvv.Reverse();
                            if (emvv.Count > 0)
                            {
                                foreach (var item1 in emvv)
                                {
                                    string a2 = sendmail_exp(data.MI_Id, item1, "TRNREMINDER", item.TRMV_Id, "TAX");
                                }

                            }

                        }


                    }
                }



                var VehicleTaxExpired = (from a in _context.VahicalCertificateDMO
                                         from b in _context.Master_VehicleDMO
                                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday && a.MI_Id == data.MI_Id
                                         select new ExpirySettingsDTO
                                         {
                                             TRVCT_Id = a.TRVCT_Id,
                                             TRMV_Id = a.TRMV_Id,
                                             TRVCT_SMSAlertToNo = a.TRVCT_SMSAlertToNo,
                                             TRVCT_eMailAlertTo = a.TRVCT_eMailAlertTo,

                                         }).ToList();




                if (VehicleTaxExpired.Count > 0)
                {
                    foreach (var item in VehicleTaxExpired)
                    {
                        if (item.TRVCT_SMSAlertToNo != null && item.TRVCT_SMSAlertToNo != "")
                        {
                            List<string> mobilevv = new List<string>(item.TRVCT_SMSAlertToNo.Split(','));
                            mobilevv.Reverse();
                            if (mobilevv.Count > 0)
                            {
                                foreach (var item1 in mobilevv)
                                {
                              string a = sendSms_exp(data.MI_Id, Convert.ToInt64(item1), "TRNREMINDER", item.TRMV_Id, "TAX");
                                }

                            }

                        }

                        if (item.TRVCT_eMailAlertTo != null && item.TRVCT_eMailAlertTo != "")
                        {
                            List<string> emvv = new List<string>(item.TRVCT_eMailAlertTo.Split(','));
                            emvv.Reverse();
                            if (emvv.Count > 0)
                            {
                                foreach (var item1 in emvv)
                                {
                                   string a2 = sendmail_exp(data.MI_Id, item1, "TRNREMINDER", item.TRMV_Id, "TAX");
                                }

                            }

                        }
                    }
                }
















                ////for Vehicle Emission Test
                //DateTime emstotoday = DateTime.Now.Date;
                //emstotoday = fromtoday.AddDays(result.TRC_EmmisionExpDays);
                //var EmissionReminder = (from a in _context.VahicalCertificateDMO
                //                        from b in _context.Master_VehicleDMO
                //                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Emission Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= emstotoday
                //                        select new ExpirySettingsDTO
                //                        {
                //                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                            TRMV_VehicleName = b.TRMV_VehicleName,
                //                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                            remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                //                        }).ToList();


                //var EmissionExpired = (from a in _context.VahicalCertificateDMO
                //                       from b in _context.Master_VehicleDMO
                //                       where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Emission Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                //                       select new ExpirySettingsDTO
                //                       {
                //                           TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                           TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                           TRMV_VehicleName = b.TRMV_VehicleName,
                //                           TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                           TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                //                       }).ToList();

                ////end

                ////for Vehicle CeaseFire
                //DateTime CeaseFiretotoday = DateTime.Now.Date;
                //CeaseFiretotoday = fromtoday.AddDays(result.TRC_CeaseFireDays);
                //var CeaseFireReminder = (from a in _context.VahicalCertificateDMO
                //                         from b in _context.Master_VehicleDMO
                //                         where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle CeaseFire" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= CeaseFiretotoday
                //                         select new ExpirySettingsDTO
                //                         {
                //                             TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                             TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                             TRMV_VehicleName = b.TRMV_VehicleName,
                //                             TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                             TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                             remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                //                         }).ToList();


                //var CeaseFireExpired = (from a in _context.VahicalCertificateDMO
                //                        from b in _context.Master_VehicleDMO
                //                        where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle CeaseFire" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                //                        select new ExpirySettingsDTO
                //                        {
                //                            TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                            TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                            TRMV_VehicleName = b.TRMV_VehicleName,
                //                            TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                            TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                //                        }).ToList();

                ////end


                ////end

                ////for Vehicle Speed
                //DateTime VehicleSpeedtotoday = DateTime.Now.Date;
                //VehicleSpeedtotoday = fromtoday.AddDays(result.TRC_SpeedControlDays);
                //var VehicleSpeedReminder = (from a in _context.VahicalCertificateDMO
                //                            from b in _context.Master_VehicleDMO
                //                            where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Speed" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= VehicleSpeedtotoday
                //                            select new ExpirySettingsDTO
                //                            {
                //                                TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                                TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                                TRMV_VehicleName = b.TRMV_VehicleName,
                //                                TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                                TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                                remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                //                            }).ToList();


                //var VehicleSpeedExpired = (from a in _context.VahicalCertificateDMO
                //                           from b in _context.Master_VehicleDMO
                //                           where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Speed" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                //                           select new ExpirySettingsDTO
                //                           {
                //                               TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                               TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                               TRMV_VehicleName = b.TRMV_VehicleName,
                //                               TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                               TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                //                           }).ToList();

                ////end

                ////for Vehicle Fitness Test
                //DateTime Vehiclefitnesstotoday = DateTime.Now.Date;
                //Vehiclefitnesstotoday = fromtoday.AddDays(result.TRC_FitnessExpDays);
                //var VehiclefitnessReminder = (from a in _context.VahicalCertificateDMO
                //                              from b in _context.Master_VehicleDMO
                //                              where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Fitness Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Vehiclefitnesstotoday
                //                              select new ExpirySettingsDTO
                //                              {
                //                                  TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                                  TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                                  TRMV_VehicleName = b.TRMV_VehicleName,
                //                                  TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                                  TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                                  remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                //                              }).ToList();


                //var VehiclefitnessExpired = (from a in _context.VahicalCertificateDMO
                //                             from b in _context.Master_VehicleDMO
                //                             where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Fitness Test" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                //                             select new ExpirySettingsDTO
                //                             {
                //                                 TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                                 TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                                 TRMV_VehicleName = b.TRMV_VehicleName,
                //                                 TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                                 TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                //                             }).ToList();

                ////end
                ////for Vehicle Permit
                //DateTime Permittotoday = DateTime.Now.Date;
                //Permittotoday = fromtoday.AddDays(result.TRC_PermitDays);
                //var PermitReminder = (from a in _context.VahicalCertificateDMO
                //                      from b in _context.Master_VehicleDMO
                //                      where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Permit" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Permittotoday
                //                      select new ExpirySettingsDTO
                //                      {
                //                          TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                          TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                          TRMV_VehicleName = b.TRMV_VehicleName,
                //                          TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                          TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                          remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                //                      }).ToList();


                //var PermitExpired = (from a in _context.VahicalCertificateDMO
                //                     from b in _context.Master_VehicleDMO
                //                     where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Permit" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                //                     select new ExpirySettingsDTO
                //                     {
                //                         TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                         TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                         TRMV_VehicleName = b.TRMV_VehicleName,
                //                         TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                         TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                //                     }).ToList();

                ////end

                ////for Vehicle Green Tax
                //DateTime Greentotoday = DateTime.Now.Date;
                //Greentotoday = fromtoday.AddDays(result.TRC_GreenTaxDays);
                //var GreenReminder = (from a in _context.VahicalCertificateDMO
                //                     from b in _context.Master_VehicleDMO
                //                     where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Green Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate >= fromtoday && a.TRVCT_ValidTillDate <= Greentotoday
                //                     select new ExpirySettingsDTO
                //                     {
                //                         TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                         TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                         TRMV_VehicleName = b.TRMV_VehicleName,
                //                         TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                         TRVCT_ValidTillDate = a.TRVCT_ValidTillDate,
                //                         remainingdays = (Convert.ToDateTime(a.TRVCT_ValidTillDate) - fromtoday).Days

                //                     }).ToList();


                //var GreenExpired = (from a in _context.VahicalCertificateDMO
                //                    from b in _context.Master_VehicleDMO
                //                    where a.MI_Id == b.MI_Id && a.TRMV_Id == b.TRMV_Id && a.TRVCT_CertificateType == "Vehicle Green Tax" && b.TRMV_ActiveFlag == true && a.TRVCT_ValidTillDate <= fromtoday
                //                    select new ExpirySettingsDTO
                //                    {
                //                        TRVCT_CertificateType = a.TRVCT_CertificateType,
                //                        TRMV_VehicleNo = b.TRMV_VehicleNo,
                //                        TRMV_VehicleName = b.TRMV_VehicleName,
                //                        TRVCT_ObtainedDate = a.TRVCT_ObtainedDate,
                //                        TRVCT_ValidTillDate = a.TRVCT_ValidTillDate

                //                    }).ToList();

                ////end



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public string sendmail_exp(long MI_Id, string Email, string Template, long ID, string type)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                //     string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, ID.ToString());
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "EXPIRY_SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TEMPLATE",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                         SqlDbType.VarChar)
                        {
                            Value = type
                        });
                        cmd.Parameters.Add(new SqlParameter("@ID",
                         SqlDbType.BigInt)
                        {
                            Value = ID
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;


                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as System.Net.HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string sendSms_exp(long MI_Id, long mobileNo, string Template, long ID, string type)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, ID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "EXPIRY_SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TEMPLATE",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                         SqlDbType.VarChar)
                        {
                            Value = type
                        });
                        cmd.Parameters.Add(new SqlParameter("@ID",
                         SqlDbType.BigInt)
                        {
                            Value = ID
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }


                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
