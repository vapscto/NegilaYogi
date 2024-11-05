using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.MobileApp;
using DomainModel.Model.com.vapstech.Portals.Student;
using CommonLibrary;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.Employee;

namespace PortalHub.com.vaps.MobileApp.Services
{
    public class AdmissionImpl : Interfaces.AdmissionInterface
    {
        private static ConcurrentDictionary<string, AdmissionDTO.getAcademicyear> _login =
         new ConcurrentDictionary<string, AdmissionDTO.getAcademicyear>();

        private readonly PortalContext _PortalContext;
        ILogger<AdmissionImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public AdmissionImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _PortalContext = cpContext;
            _db = db;
        }

        public AdmissionDTO.getAcademicyear getAcademicYear(AdmissionDTO.getAcademicyear data)
        {
            try
            {
                data.getAcaYrLst = (from a in _PortalContext.AcademicYearDMO
                                    where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                    select new AdmissionDTO.getAcademicyear
                                    {
                                        ASMAY_Id = a.ASMAY_Id,
                                        ASMAY_Year = a.ASMAY_Year,
                                        ASMAY_From_Date = a.ASMAY_From_Date,
                                        ASMAY_To_Date = a.ASMAY_To_Date,
                                        ASMAY_Order = a.ASMAY_Order
                                    }
                          ).OrderBy(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.getClass getclass(AdmissionDTO.getClass data)
        {
            try
            {
                data.getclass = (from a in _PortalContext.School_M_Class
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true)
                                 select new AdmissionDTO.getClass
                                 {
                                     ASMCL_Id = a.ASMCL_Id,
                                     ASMCL_ClassName = a.ASMCL_ClassName,
                                     ASMCL_ClassCode = a.ASMCL_ClassCode,
                                     ASMCL_Order = a.ASMCL_Order,
                                     ASMCL_MaxCapacity = a.ASMCL_MaxCapacity
                                 }
                          ).OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.getSection getsection(AdmissionDTO.getSection data)
        {
            try
            {
                data.getsection = (from a in _PortalContext.School_M_Section
                                   where (a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1)
                                   select new AdmissionDTO.getSection
                                   {
                                       ASMS_Id = a.ASMS_Id,
                                       ASMC_SectionName = a.ASMC_SectionName,
                                       ASMC_SectionCode = a.ASMC_SectionCode,
                                       ASMC_Order = a.ASMC_Order,
                                       ASMC_MaxCapacity = a.ASMC_MaxCapacity
                                   }
                          ).OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.getClass AcademicyearwiseClass(AdmissionDTO.getClass data)
        {
            try
            {
                data.getclass = (from a in _PortalContext.AcademicYearDMO
                                 from b in _PortalContext.School_M_Class
                                 from c in _PortalContext.School_Adm_Y_StudentDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true && c.AMAY_ActiveFlag == 1 && a.MI_Id == b.MI_Id && c.ASMCL_Id == b.ASMCL_Id && c.ASMAY_Id == data.ASMAY_Id)
                                 select new AdmissionDTO.getClass
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMCL_ClassName = b.ASMCL_ClassName,
                                     ASMCL_ClassCode = b.ASMCL_ClassCode,
                                     ASMCL_Order = b.ASMCL_Order,
                                     ASMCL_MaxCapacity = b.ASMCL_MaxCapacity
                                 }
                          ).OrderBy(t => t.ASMCL_Order).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.getSection AcademicyearwiseClassSection(AdmissionDTO.getSection data)
        {
            try
            {
                data.getsection = (from a in _PortalContext.AcademicYearDMO
                                   from b in _PortalContext.School_M_Class
                                   from d in _PortalContext.School_M_Section
                                   from c in _PortalContext.School_Adm_Y_StudentDMO
                                   where (a.MI_Id == b.MI_Id && c.ASMCL_Id == b.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && a.ASMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true && c.AMAY_ActiveFlag == 1 && d.ASMC_ActiveFlag == 1 && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id)
                                   select new AdmissionDTO.getSection
                                   {
                                       ASMS_Id = d.ASMS_Id,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       ASMC_SectionCode = d.ASMC_SectionCode,
                                       ASMC_Order = d.ASMC_Order,
                                       ASMC_MaxCapacity = d.ASMC_MaxCapacity
                                   }
                          ).OrderBy(t => t.ASMC_Order).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.getstudent AcademicyearwiseClassSectionStudent(AdmissionDTO.getstudent data)
        {
            try
            {
                data.getstudentdetails = (from a in _PortalContext.AcademicYearDMO
                                          from b in _PortalContext.School_M_Class
                                          from d in _PortalContext.School_M_Section
                                          from c in _PortalContext.School_Adm_Y_StudentDMO
                                          from e in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == b.MI_Id && c.ASMAY_Id == a.ASMAY_Id && c.ASMCL_Id == b.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && c.AMST_Id == e.AMST_Id && a.ASMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true && c.AMAY_ActiveFlag == 1 && d.ASMC_ActiveFlag == 1 && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id && e.AMST_SOL == "S" && e.AMST_ActiveFlag == 1)
                                          select new AdmissionDTO.getstudent
                                          {
                                              AMST_Id = c.AMST_Id,
                                              ASMCL_Id = b.ASMCL_Id,
                                              ASMS_Id = d.ASMS_Id,
                                              ASMAY_Id = a.ASMAY_Id,
                                              AMST_FirstName = e.AMST_FirstName,
                                              AMST_MiddleName = e.AMST_MiddleName,
                                              AMST_LastName = e.AMST_LastName,
                                              AMST_AdmNo = e.AMST_AdmNo,
                                              AMST_RegistrationNo = e.AMST_RegistrationNo,
                                              AMST_DOB = Convert.ToDateTime(e.AMST_DOB),
                                              AMST_BloodGroup = e.AMST_BloodGroup,
                                              AMST_AadharNo = e.AMST_AadharNo,
                                              AMST_MobileNo = e.AMST_MobileNo,
                                              AMST_emailId = e.AMST_emailId,
                                              AMST_Photoname = e.AMST_Photoname,
                                              AMST_Tpin = e.AMST_Tpin,
                                              ASMCL_ClassName = b.ASMCL_ClassName,
                                              ASMC_SectionName = d.ASMC_SectionName,
                                              ASMAY_Year = a.ASMAY_Year
                                          }
                          ).OrderBy(t => t.ASMCL_ClassName).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Apply Certificate
        public AdmissionDTO.getCertificateApply getOnloadCertificateapply(AdmissionDTO.getCertificateApply data)
        {
            try
            {
                if (data.flag != "" && data.flag != null)
                {
                    var roletyp = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                }

                if (data.roletype.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    data.studentdetails = (from a in _PortalContext.Adm_M_Student
                                           from b in _PortalContext.School_Adm_Y_StudentDMO
                                           from c in _PortalContext.School_M_Class
                                           from s in _PortalContext.School_M_Section
                                           where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                           select new AdmissionDTO.getCertificateApply
                                           {
                                               ASMCL_Id = c.ASMCL_Id,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMS_Id = b.ASMS_Id,
                                               ASMC_SectionName = s.ASMC_SectionName,
                                               AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                               AMST_RegistrationNo = a.AMST_RegistrationNo,
                                               AMST_MobileNo = a.AMST_MobileNo,
                                               AMST_emailId = a.AMST_emailId,
                                               AMST_Id = a.AMST_Id,
                                           }).Distinct().ToArray();

                    data.studlist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                     from b in _PortalContext.School_Adm_Y_StudentDMO
                                     from c in _PortalContext.Adm_M_Student
                                     from cl in _PortalContext.School_M_Class
                                     from sc in _PortalContext.School_M_Section
                                     from cr in _PortalContext.Adm_Certificates_Apply_DMO_con
                                     where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == cl.ASMCL_Id && b.ASMS_Id == sc.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && c.AMST_SOL == "S" && a.ASCA_CertificateType == cr.ACERTAPP_CertificateCode)
                                     select new AdmissionDTO.getCertificateApply
                                     {
                                         AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                         ASMCL_ClassName = cl.ASMCL_ClassName,
                                         ASMC_SectionName = sc.ASMC_SectionName,
                                         ACERTAPP_CertificateName = cr.ACERTAPP_CertificateName,
                                         ACERTAPP_CertificateCode = cr.ACERTAPP_CertificateCode,
                                         ASCA_ApplyDate = a.ASCA_ApplyDate,
                                         ASCA_Status = a.ASCA_Status,
                                         AMST_Id = a.AMST_Id,
                                         ASCA_Id = a.ASCA_Id,
                                         ASCA_ActiveFlg = a.ASCA_ActiveFlg,
                                         ASCA_Reason = a.ASCA_Reason,
                                     }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();

                    data.certificatelist = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ACERTAPP_ActiveFlg == true).ToArray();


                }

                else if (data.roletype.Equals("Principal", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("chairman", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("admin", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;



                    data.applylist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.School_M_Class
                                      from d in _PortalContext.School_M_Section
                                      from e in _PortalContext.Adm_M_Student
                                      from cr in _PortalContext.Adm_Certificates_Apply_DMO_con
                                      where (a.MI_Id == c.MI_Id && b.AMST_Id == a.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.AMST_Id == e.AMST_Id && a.MI_Id == data.MI_Id && e.AMST_SOL == "S" && a.ASCA_ActiveFlg == true && a.ASCA_Status == "Pending" && a.ASCA_CertificateType == cr.ACERTAPP_CertificateCode)
                                      select new AdmissionDTO.getCertificateApply
                                      {
                                          AMST_Id = a.AMST_Id,
                                          AMST_AdmNo = e.AMST_AdmNo,
                                          AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                          ASMS_Id = d.ASMS_Id,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ACERTAPP_Id = cr.ACERTAPP_Id,
                                          ACERTAPP_CertificateName = cr.ACERTAPP_CertificateName,
                                          ACERTAPP_CertificateCode = cr.ACERTAPP_CertificateCode,
                                          ASMCL_Id = c.ASMCL_Id,
                                          MI_Id = c.MI_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          AMST_RegistrationNo = e.AMST_RegistrationNo,
                                          AMST_emailId = e.AMST_emailId,
                                          AMST_MobileNo = e.AMST_MobileNo,
                                          ASCA_Status = a.ASCA_Status,
                                          ASCA_Id = a.ASCA_Id,
                                          ASCA_Reason = a.ASCA_Reason,
                                          ASCA_ApplyDate = a.ASCA_ApplyDate,
                                      }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();


                    data.aply_aprvlist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                          from b in _PortalContext.Adm_Students_Certificate_Approve_DMO
                                          from std in _PortalContext.Adm_M_Student
                                          from Y in _PortalContext.School_Adm_Y_StudentDMO
                                          from cl in _PortalContext.School_M_Class
                                          from sc in _PortalContext.School_M_Section
                                          from year in _PortalContext.AcademicYearDMO
                                          from cr in _PortalContext.Adm_Certificates_Apply_DMO_con
                                          where (a.ASCA_Id == b.ASCA_Id && a.AMST_Id == std.AMST_Id && a.ASMAY_Id == Y.ASMAY_Id && a.AMST_Id == Y.AMST_Id &&
                                          cl.ASMCL_Id == Y.ASMCL_Id && sc.ASMS_Id == Y.ASMS_Id && Y.ASMAY_Id == year.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASCA_ActiveFlg == true && std.AMST_SOL == "S" && Y.AMAY_ActiveFlag == 1 && a.ASCA_CertificateType == cr.ACERTAPP_CertificateCode)
                                          select new AdmissionDTO.getCertificateApply
                                          {
                                              AMST_FirstName = ((std.AMST_FirstName == null ? " " : std.AMST_FirstName) + " " + (std.AMST_MiddleName == null ? " " : std.AMST_MiddleName) + " " + (std.AMST_LastName == null ? " " : std.AMST_LastName)).Trim(),
                                              ASMC_SectionName = sc.ASMC_SectionName,
                                              ASMCL_ClassName = cl.ASMCL_ClassName,
                                              AMST_AdmNo = std.AMST_AdmNo,
                                              AMST_RegistrationNo = std.AMST_RegistrationNo,
                                              ASCA_Status = a.ASCA_Status,
                                              ACERTAPP_Id = cr.ACERTAPP_Id,
                                              ACERTAPP_CertificateName = cr.ACERTAPP_CertificateName,
                                              ACERTAPP_CertificateCode = cr.ACERTAPP_CertificateCode,
                                              ASCA_Reason = a.ASCA_Reason,
                                              ASCA_ApplyDate = a.ASCA_ApplyDate,
                                              ASCAP_ApproveDate = b.ASCAP_ApproveDate,
                                              ASCAP_ApproveReason = b.ASCAP_ApproveReason,
                                              ASMAY_Year = year.ASMAY_Year,
                                          }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();

                }


                data.certificate_dropdown = _PortalContext.Adm_Master_Certificate_DMO_con.Where(a => a.AMCT_ActiceFlag == true).ToArray();
                data.get_certificate = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<AdmissionDTO.saveCertificateApply> applyCertifateApplySave(AdmissionDTO.saveCertificateApply data)
        {
            try
            {
                var rslt = _PortalContext.Adm_Certificates_Apply_DMO_con.Single(a => a.ACERTAPP_Id == data.ACERTAPP_Id && a.MI_Id == data.MI_Id);
                string s = "";
                string m = "";


                if (data.ASCA_Id > 0)
                {
                    var Duplicate = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id != data.ASCA_Id && t.ASCA_CertificateType == data.ASCA_CertificateType && t.ASCA_Status == "Pending" && t.ASMAY_Id == data.ASMAY_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id).SingleOrDefault();

                        update.ASCA_CertificateType = rslt.ACERTAPP_CertificateCode;
                        update.ASCA_Reason = data.ASCA_Reason;
                        update.ASCA_ApplyDate = data.ASCA_ApplyDate;
                        update.ASCA_Status = "Pending";

                        update.UpdatedDate = DateTime.Now;
                        _PortalContext.Update(update);

                        int rowAffected = _PortalContext.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var Duplicate = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_CertificateType == rslt.ACERTAPP_CertificateCode && t.AMST_Id == data.AMST_Id && t.ASCA_Status == "Pending" || t.ASCA_Status == "Approved" && t.ASMAY_Id == data.ASMAY_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        Adm_Students_Certificate_Apply_DMO obj = new Adm_Students_Certificate_Apply_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.AMST_Id = data.AMST_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.ASCA_CertificateType = rslt.ACERTAPP_CertificateCode;
                        obj.ASCA_Reason = data.ASCA_Reason;
                        obj.ASCA_ApplyDate = data.ASCA_ApplyDate;
                        obj.ASCA_Status = "Pending";
                        obj.ASCA_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _PortalContext.Add(obj);

                        int rowAffected = _PortalContext.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            var clsid = (from d in _PortalContext.AcademicYearDMO
                                         from a in _PortalContext.School_M_Class
                                         from c in _PortalContext.School_Adm_Y_StudentDMO
                                         where (a.ASMCL_Id == c.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                         select new AdmissionDTO.saveCertificateApply
                                         {
                                             ASMCL_Id = c.ASMCL_Id,
                                             ASMCL_ClassName = a.ASMCL_ClassName,
                                         }
                                ).Distinct().ToList();

                            var hrmeids = (from a in _PortalContext.School_M_Class
                                           from b in _PortalContext.HR_Master_Employee_DMO
                                           from c in _PortalContext.IVRM_PrincipalDMO
                                           from d in _PortalContext.IVRM_Principal_ClassDMO
                                           where (a.ASMCL_Id == d.ASMCL_Id && b.HRME_Id == c.IVRMUL_Id && a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id && a.ASMCL_Id == clsid.FirstOrDefault().ASMCL_Id)
                                           select new AdmissionDTO.saveCertificateApply
                                           {
                                               HRME_Id = c.IVRMUL_Id
                                           }
                                          ).Distinct().ToArray();

                            data.HRME_Id = hrmeids.FirstOrDefault().HRME_Id;

                            long mobileno = 0;
                            var emailid = "";
                            var mobile = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEMNO_DeFaultFlag == "default").ToList();
                            var email = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == data.HRME_Id && e.HRMEM_DeFaultFlag == "default").ToList();

                            if (mobile.Count > 0)
                            {
                                mobileno = _PortalContext.Multiple_Mobile_DMO.Single(p => p.HRME_Id == data.HRME_Id && p.HRMEMNO_DeFaultFlag == "default").HRMEMNO_MobileNo;
                                SMS sms = new SMS(_db);
                                s = await sms.sendCertificateSms(data.MI_Id, mobileno, "TCREQUEST", data.AMST_Id, data.HRME_Id, data.ASMAY_Id, obj.ASCA_Id);
                            }
                            if (email.Count > 0)
                            {
                                emailid = _PortalContext.Multiple_Email_DMO.Single(e => e.HRME_Id == data.HRME_Id && e.HRMEM_DeFaultFlag == "default").HRMEM_EmailId;
                                Email Email = new Email(_db);
                                m = Email.sendCertificatemail(data.MI_Id, emailid, "TCREQUEST", data.AMST_Id, data.HRME_Id, data.ASMAY_Id, obj.ASCA_Id);
                            }
                            if (s == "success" && m == "success")
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }

                        }
                        else
                        {
                            data.returnval = false;
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

        public AdmissionDTO.getCertificateDetails getCertificateDetails(AdmissionDTO.getCertificateDetails data)
        {
            try
            {


                if (data.roletype.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;

                    data.studlist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                     from b in _PortalContext.School_Adm_Y_StudentDMO
                                     from c in _PortalContext.Adm_M_Student
                                     from cl in _PortalContext.School_M_Class
                                     from sc in _PortalContext.School_M_Section
                                     from cr in _PortalContext.Adm_Certificates_Apply_DMO_con
                                     where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == cl.ASMCL_Id && b.ASMS_Id == sc.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && c.AMST_SOL == "S" && a.ASCA_CertificateType == cr.ACERTAPP_CertificateCode)
                                     select new AdmissionDTO.getCertificateApply
                                     {
                                         AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                         ASMCL_ClassName = cl.ASMCL_ClassName,
                                         ASMC_SectionName = sc.ASMC_SectionName,
                                         ACERTAPP_CertificateName = cr.ACERTAPP_CertificateName,
                                         ACERTAPP_CertificateCode = cr.ACERTAPP_CertificateCode,
                                         ASCA_ApplyDate = a.ASCA_ApplyDate,
                                         ASCA_Status = a.ASCA_Status,
                                         AMST_Id = a.AMST_Id,
                                         ASCA_Id = a.ASCA_Id,
                                         ASCA_ActiveFlg = a.ASCA_ActiveFlg,
                                         ASCA_Reason = a.ASCA_Reason,
                                     }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Student Feedback
        public AdmissionDTO.getloadFeedbackdata getloadFeedbackdata(AdmissionDTO.getloadFeedbackdata data)
        {
            try
            {
                data.instname = _PortalContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();
                // data.get_feedback = _PortalContext.Adm_School_Student_GFeedbackDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionDTO.saveFeedbackFormDTO savefeedback(AdmissionDTO.saveFeedbackFormDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var clsecids = (from d in _PortalContext.AcademicYearDMO
                                from a in _PortalContext.School_M_Class
                                from b in _PortalContext.School_M_Section
                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                select new AdmissionDTO.saveFeedbackFormDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id,
                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                    ASMS_Id = c.ASMS_Id,
                                }).Distinct().ToList();

                Adm_School_Student_GFeedbackDMO feedback = new Adm_School_Student_GFeedbackDMO();
                feedback.MI_Id = data.MI_Id;
                feedback.AMST_Id = data.AMST_Id;
                feedback.ASMAY_Id = data.ASMAY_Id;
                feedback.ASMCL_Id = clsecids.FirstOrDefault().ASMCL_Id;
                feedback.ASMS_Id = clsecids.FirstOrDefault().ASMS_Id;
                feedback.ASGFE_FeedBack = data.ASGFE_FeedBack;
                feedback.ASGFE_FeedbackDate = indianTime;
                feedback.ASGFE_ActiveFlag = true;
                feedback.ASGFE_CreatedBy = data.AMST_Id;
                feedback.ASGFE_UpdatedBy = data.AMST_Id;
                feedback.CreatedDate = DateTime.Now;
                feedback.UpdatedDate = DateTime.Now;
                _PortalContext.Add(feedback);

                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
            }
            return data;
        }



        //Interaction
        public async Task<AdmissionDTO.OnloadInteractionsDTO> getInteractionloaddata(AdmissionDTO.OnloadInteractionsDTO data)
        {
            try
            {


                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                    data.userhrmE_Id = data.AMST_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HOD", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.userhrmE_Id = data.HRME_Id;
                }
                else
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }





                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_Inbox";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@roleflg",
                    SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getinboxmsg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_ReadCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@roleflg",
                    SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getinboxmsg_readflg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<AdmissionDTO.replyInteractionsDTO> intractionreply(AdmissionDTO.replyInteractionsDTO data)
        {
            try
            {
                var composeedby = _PortalContext.IVRM_School_Master_InteractionsDMO.Single(q => q.ISMINT_Id == data.ISMINT_Id);

                var composeedto = _PortalContext.IVRM_School_Transaction_InteractionsDMO.Where(q => q.ISMINT_Id == data.ISMINT_Id).ToList();

                data.composeedto = composeedto.FirstOrDefault().ISTINT_ToFlg.ToLower();


                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();

                data.rolename = rolet.FirstOrDefault().IVRMRT_Role.ToLower();
                long cmpid = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.ISMINT_ComposedByFlg = composeedby.ISMINT_ComposedByFlg.ToLower();

                    if (data.ISMINT_ComposedByFlg == "student")
                    {
                        var trans = _PortalContext.IVRM_School_Transaction_InteractionsDMO.Where(q => q.ISMINT_Id == data.ISMINT_Id && q.ISTINT_ToFlg.ToLower() == "staff" && q.ISTINT_ActiveFlag == true).ToList();
                        if (trans.Count > 0)
                        {
                            cmpid = trans.FirstOrDefault().ISTINT_ToId;
                        }

                    }
                    else
                    {

                        cmpid = composeedby.ISMINT_ComposedById;

                    }

                    data.typelistrole = (from a in _PortalContext.IVRM_Role_Type
                                         from b in _PortalContext.Staff_User_Login
                                         from d in _PortalContext.ApplicationUserRole
                                         where b.MI_Id == data.MI_Id && b.Emp_Code == cmpid
                                         && d.UserId == b.Id && d.RoleTypeId == a.IVRMRT_Id
                                         select a).Distinct().ToArray();



                    data.typelist = (from a in _PortalContext.HOD_DMO

                                     where a.IHOD_ActiveFlag == true && a.HRME_Id == cmpid && a.MI_Id == data.MI_Id
                                     select a).Distinct().ToArray();



                    if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                    {
                        var clsid = (from a in _PortalContext.Adm_M_Student
                                     from b in _PortalContext.School_M_Class
                                     from c in _PortalContext.School_M_Section
                                     from d in _PortalContext.School_Adm_Y_StudentDMO
                                     from e in _PortalContext.AcademicYearDMO
                                     where (a.AMST_Id == d.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMST_Id == data.AMST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                     select new AdmissionDTO.replyInteractionsDTO
                                     {
                                         ASMCL_Id = b.ASMCL_Id,
                                         ASMS_Id = c.ASMS_Id,
                                         ASMC_SectionName = c.ASMC_SectionName,
                                         ASMCL_ClassName = b.ASMCL_ClassName
                                     }
                             ).Distinct().ToArray();


                        if (clsid.Length > 0)
                        {
                            data.asmclid = clsid.FirstOrDefault().ASMCL_Id;
                            data.asmsid = clsid.FirstOrDefault().ASMS_Id;
                        }


                        data.classteacherlist = (from a in _PortalContext.ClassTeacherMappingDMO
                                                 where a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.ASMCL_Id == data.asmclid && a.ASMS_Id == data.asmsid && a.HRME_Id == cmpid && a.ASMAY_Id == data.ASMAY_Id

                                                 select a
                                              ).Distinct().ToArray();


                        data.subteacherlist = (from a in _PortalContext.Exm_Login_PrivilegeDMO
                                               from b in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                               from c in _PortalContext.Staff_User_Login
                                               where a.ELP_Id == b.ELP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ELP_ActiveFlg == true && b.ASMCL_Id == data.asmclid && b.ASMS_Id == data.asmsid && a.Login_Id == c.IVRMSTAUL_Id && c.Emp_Code == cmpid && a.MI_Id == c.MI_Id && a.ELP_Flg.ToLower() == "st"
                                               select a).Distinct().Take(1).ToArray();

                    }





                }
                else
                {


                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                    data.ISMINT_ComposedByFlg = composeedby.ISMINT_ComposedByFlg.ToLower();


                    data.typelistrole = (from a in _PortalContext.IVRM_Role_Type
                                         from b in _PortalContext.Staff_User_Login
                                         from d in _PortalContext.ApplicationUserRole
                                         where b.MI_Id == data.MI_Id && b.Emp_Code == data.HRME_Id
                                         && d.UserId == b.Id && d.RoleTypeId == a.IVRMRT_Id
                                         select a).Distinct().ToArray();



                    data.typelist = (from a in _PortalContext.HOD_DMO

                                     where a.IHOD_ActiveFlag == true && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                                     select a).Distinct().ToArray();



                    data.classteacherlist = (from a in _PortalContext.ClassTeacherMappingDMO
                                             where a.MI_Id == data.MI_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id

                                             select a
                                          ).Distinct().ToArray();


                    data.subteacherlist = (from a in _PortalContext.Exm_Login_PrivilegeDMO
                                           from b in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                           from c in _PortalContext.Staff_User_Login
                                           where a.ELP_Id == b.ELP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ELP_ActiveFlg == true && a.Login_Id == c.IVRMSTAUL_Id && c.Emp_Code == data.HRME_Id && a.MI_Id == c.MI_Id && a.ELP_Flg.ToLower() == "st"
                                           select a).Distinct().Take(1).ToArray();



                }


                var configflaglist = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMGC_GMRDTOALLFlg == true).Distinct().ToList();


                int cnt = 0;
                if (cnt == 0)
                {
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_Interaction_View_Reply";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.ISMINT_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
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
                            data.viewmessage = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    if (composeedby.ISMINT_GroupOrIndFlg.ToLower() == "group")
                    {

                        if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                        {
                            data.HRME_Id1 = data.AMST_Id;
                        }
                        else
                        {
                            data.HRME_Id1 = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                        }


                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Interaction_View_Reply_Group";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ISMINT_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@STORHRMEID",
                          SqlDbType.BigInt)
                            {
                                Value = data.HRME_Id1
                            });
                            cmd.Parameters.Add(new SqlParameter("@SRole",
                          SqlDbType.Char)
                            {
                                Value = rolet.FirstOrDefault().IVRMRT_Role
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
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
                                data.viewmessage = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "IVRM_Interaction_View_Reply";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ISMINT_Id
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
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
                                data.viewmessage = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }




                //var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                else
                {
                    data.AMST_Id = 0;
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                long loginuserid = 0;
                if (data.HRME_Id == 0)
                {
                    loginuserid = data.AMST_Id;
                }
                else if (data.AMST_Id == 0)
                {
                    loginuserid = data.HRME_Id;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_CreatedBy";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@loginuserid",
                    SqlDbType.BigInt)
                    {
                        Value = loginuserid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMINT_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.ISMINT_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_msgcreator = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                var rmv = 0;

                long composedid = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    composedid = (from a in _PortalContext.ApplicationUser
                                  from b in _PortalContext.StudentUserLoginDMO
                                  where (a.UserName == b.IVRMSTUUL_UserName && a.Id == data.UserId && b.MI_Id == data.MI_Id)
                                  select b.AMST_Id).Distinct().FirstOrDefault();
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    composedid = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                else
                {
                    composedid = (from a in _PortalContext.ApplicationUser
                                  from b in _PortalContext.Staff_User_Login
                                  where (a.UserName == b.IVRMSTAUL_UserName && a.Id == data.UserId && b.MI_Id == data.MI_Id)
                                  select b.Emp_Code).Distinct().FirstOrDefault();
                }


                var result = _PortalContext.IVRM_School_Transaction_InteractionsDMO.Where(a => a.ISMINT_Id == data.ISMINT_Id && a.ISTINT_ToId == composedid).ToArray();

                if (result.Length > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.ISTINT_ReadFlg != true)
                        {
                            item.ISTINT_ReadFlg = true;
                            item.UpdatedDate = DateTime.Today;
                            _PortalContext.Update(item);
                        }


                    }
                    rmv = _PortalContext.SaveChanges();
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionDTO.replysaveInteractionsDTO intractionreplysave(AdmissionDTO.replysaveInteractionsDTO data)
        {
            try
            {
                string image = "";
                if (data.images_paths != null)
                {
                    foreach (var ig in data.images_paths)
                    {
                        image = ig;
                    }
                }
                long toId = 0;
                long sentoId = 0;
                var sentoflg = "";
                long byId = 0;
                var toflg = "";
                var byflg = "";
                var groupOrIndFlg = "";
                int level_no = 0;
                var level_order = 0;
                long composeby = 0;
                string composeflag = "";
                string notiSubject = "";

                List<AdmissionDTO.replysaveInteractionsDTO> deviceid = new List<AdmissionDTO.replysaveInteractionsDTO>();
                List<AdmissionDTO.replysaveInteractionsDTO> deviceiddddd = new List<AdmissionDTO.replysaveInteractionsDTO>();
                data.deviceids = deviceid.ToArray();
                List<AdmissionDTO.replysaveInteractionsDTO> devicelist = new List<AdmissionDTO.replysaveInteractionsDTO>();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                List<long> device_ids = new List<long>();
                List<long> device_grp = new List<long>();

                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                    composeby = data.AMST_Id;
                    composeflag = "Student";
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("HOD", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase))
                {
                    var empid = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    composeby = empid.FirstOrDefault().Emp_Code;
                    composeflag = "Staff";
                }

                var comp_id = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                               where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id)
                               select new AdmissionDTO.replysaveInteractionsDTO
                               {
                                   ISMINT_Id = b.ISMINT_Id,
                                   ISTINT_Id = a.ISTINT_Id,
                                   ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg,
                                   ISTINT_ComposedById = a.ISTINT_ComposedById,
                                   ISTINT_ToFlg = a.ISTINT_ToFlg,
                                   ISTINT_ToId = a.ISTINT_ToId,
                                   ISMINT_GroupOrIndFlg = b.ISMINT_GroupOrIndFlg,
                               }).Distinct().OrderBy(o => o.ISTINT_Id).ToList();

                toId = comp_id.FirstOrDefault().ISTINT_ToId;
                toflg = comp_id.FirstOrDefault().ISTINT_ToFlg;
                byId = comp_id.FirstOrDefault().ISTINT_ComposedById;
                byflg = comp_id.FirstOrDefault().ISTINT_ComposedByFlg;
                groupOrIndFlg = comp_id.FirstOrDefault().ISMINT_GroupOrIndFlg;

                if (composeby == byId)
                {
                    byId = toId;
                }

                var getuserid = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                                 from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                                 where (a.ISMINT_Id == b.ISMINT_Id && a.ISMINT_Id == data.ISMINT_Id && b.MI_Id == data.MI_Id && a.ISTINT_InteractionOrder == 1)
                                 select new AdmissionDTO.replysaveInteractionsDTO
                                 {
                                     ISMINT_Id = b.ISMINT_Id,
                                     ISTINT_Id = a.ISTINT_Id,
                                     ISTINT_ComposedByFlg = a.ISTINT_ComposedByFlg,
                                     ISTINT_ComposedById = a.ISTINT_ComposedById,
                                     ISTINT_ToFlg = a.ISTINT_ToFlg,
                                     ISTINT_ToId = a.ISTINT_ToId,
                                     ISMINT_GroupOrIndFlg = b.ISMINT_GroupOrIndFlg,
                                 }).Distinct().OrderBy(o => o.ISTINT_Id).ToList();

                if (composeflag == "Staff")
                {
                    if (composeby == byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            device_ids.Add(byId);
                        }
                        else
                        {
                            sentoId = toId;
                            sentoflg = "Staff";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else if (composeby == byId && composeflag == byflg && toflg == "Student")
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        else
                        {
                            sentoId = toId;
                            sentoflg = "Student";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else if (composeby != byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            device_ids.Add(byId);
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else
                    {
                        if (groupOrIndFlg == "Group")
                        {
                            sentoId = toId;
                            sentoflg = "Student";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ISTINT_ToId);
                            }
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Student";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ComposedById);
                        }
                    }
                    if (byflg == "Staff")
                    {
                        if (toflg == "Student")
                        {
                            var deviceidsgg = (from a in _PortalContext.Adm_M_Student
                                               where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                               select new AdmissionDTO.replysaveInteractionsDTO
                                               {
                                                   AMST_Id = a.AMST_Id,
                                                   studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                                   AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                                   AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                               }).Distinct().ToList();

                            var deviceidGrpddd = (from a in _PortalContext.HR_Master_Employee_DMO
                                                  where (a.MI_Id == data.MI_Id && device_grp.Contains(a.HRME_Id))
                                                  select new AdmissionDTO.replysaveInteractionsDTO
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                      HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  }).Distinct().ToList();

                            data.deviceids = deviceidsgg.ToArray();
                            data.deviceidGrp = deviceidGrpddd.ToArray();

                            for (int i = 0; i < deviceidsgg.Count; i++)
                            {
                                devicelist.Add(deviceidsgg[i]);
                            }

                            for (int j = 0; j < deviceidGrpddd.Count; j++)
                            {
                                devicelist.Add(deviceidGrpddd[j]);
                            }

                        }
                        else
                        {
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new AdmissionDTO.replysaveInteractionsDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              }).Distinct().ToArray();
                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new AdmissionDTO.replysaveInteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          }).Distinct().ToList();

                            var d1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new AdmissionDTO.replysaveInteractionsDTO
                                      {
                                          HRME_MobileNo = a.HRME_MobileNo,
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      }).Distinct().ToList();
                            data.devicelist12 = d1;
                        }
                    }
                    else if (byflg == "Student")
                    {
                        data.deviceids = (from a in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                          select new AdmissionDTO.replysaveInteractionsDTO
                                          {
                                              AMST_Id = a.AMST_Id,
                                              studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                              AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                          }).Distinct().ToArray();
                        devicelist = (from a in _PortalContext.Adm_M_Student
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                      select new AdmissionDTO.replysaveInteractionsDTO
                                      {
                                          AMST_Id = a.AMST_Id,
                                          studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                          AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                      }).Distinct().ToList();

                        var d2 = (from a in _PortalContext.Adm_M_Student
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                  select new AdmissionDTO.replysaveInteractionsDTO
                                  {
                                      HRME_MobileNo = a.AMST_MobileNo,
                                      AMST_Id = a.AMST_Id,
                                      studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                      AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                  }).Distinct().ToList();
                        data.devicelist12 = d2;
                    }
                }
                else if (composeflag == "Student")
                {
                    if (composeby == byId && composeflag == byflg && toflg == "Staff")
                    {
                        if (groupOrIndFlg == "Group" && byflg == "Student")
                        {
                            sentoId = byId;
                            sentoflg = "Student";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ISTINT_ToId);
                            }
                        }
                        else if (groupOrIndFlg == "Group" && byflg == "Staff")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        else
                        {
                            sentoId = toId;
                            sentoflg = "Staff";
                        }
                        foreach (var r in getuserid)
                        {
                            device_ids.Add(r.ISTINT_ToId);
                        }
                    }
                    else
                    {
                        if (groupOrIndFlg == "Group" && byflg == "Student")
                        {
                            sentoId = byId;
                            sentoflg = "Student";

                        }
                        else if (groupOrIndFlg == "Group" && byflg == "Staff")
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                            foreach (var r in getuserid)
                            {
                                device_grp.Add(r.ISTINT_ComposedById);
                            }
                        }
                        else
                        {
                            sentoId = byId;
                            sentoflg = "Staff";
                        }
                        if (groupOrIndFlg == "Group")
                        {
                            foreach (var r in getuserid)
                            {
                                device_ids.Add(r.ISTINT_ToId);
                            }
                        }
                        else
                        {
                            foreach (var r in getuserid)
                            {
                                device_ids.Add(r.ISTINT_ComposedById);
                            }
                        }
                    }

                    if (groupOrIndFlg == "Group" && byflg == "Student")
                    {
                        data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new AdmissionDTO.replysaveInteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new AdmissionDTO.replysaveInteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      }).Distinct().ToList();
                        var d3 = (from a in _PortalContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                  select new AdmissionDTO.replysaveInteractionsDTO
                                  {
                                      HRME_MobileNo = a.HRME_MobileNo,
                                      HRME_Id = a.HRME_Id,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                  }).Distinct().ToList();
                        data.devicelist12 = d3;
                    }
                    else if (groupOrIndFlg == "Group" && byflg == "Staff")
                    {
                        var deviceidsgg = (from a in _PortalContext.Adm_M_Student
                                           where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                           select new AdmissionDTO.replysaveInteractionsDTO
                                           {
                                               AMST_Id = a.AMST_Id,
                                               studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                               AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                               AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId
                                           }).Distinct().ToList();

                        var deviceidGrpddd = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_grp.Contains(a.HRME_Id))
                                              select new AdmissionDTO.replysaveInteractionsDTO
                                              {
                                                  HRME_MobileNo = a.HRME_MobileNo,
                                                  HRME_Id = a.HRME_Id,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              }).Distinct().ToList();
                        data.devicelist12 = deviceidGrpddd;

                        data.deviceids = deviceidsgg.ToArray();
                        data.deviceidGrp = deviceidGrpddd.ToArray();

                        for (int i = 0; i < deviceidsgg.Count; i++)
                        {
                            devicelist.Add(deviceidsgg[i]);
                        }

                        for (int j = 0; j < deviceidGrpddd.Count; j++)
                        {
                            devicelist.Add(deviceidGrpddd[j]);
                        }

                    }
                    else
                    {
                        data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new AdmissionDTO.replysaveInteractionsDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                          }).Distinct().ToArray();

                        devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                      select new AdmissionDTO.replysaveInteractionsDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                      }).Distinct().ToList();
                        var d4 = (from a in _PortalContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                  select new AdmissionDTO.replysaveInteractionsDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,

                                  }).Distinct().ToList();
                        data.devicelist12 = d4;
                    }

                }


                var orderno = (from a in _PortalContext.IVRM_School_Transaction_InteractionsDMO
                               from b in _PortalContext.IVRM_School_Master_InteractionsDMO
                               where (a.ISMINT_Id == b.ISMINT_Id && b.MI_Id == data.MI_Id && a.ISMINT_Id == data.ISMINT_Id)
                               select new AdmissionDTO.replysaveInteractionsDTO
                               {
                                   ISTINT_InteractionOrder = a.ISTINT_InteractionOrder
                               }).Distinct().ToList();

                level_no = orderno.LastOrDefault().ISTINT_InteractionOrder;
                if (level_no <= 0)
                {

                    level_order = 1;
                }
                else
                {
                    level_order = level_no + 1;
                }

                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                intrans.ISMINT_Id = data.ISMINT_Id;
                intrans.ISTINT_ToId = sentoId;
                intrans.ISTINT_ToFlg = sentoflg;
                intrans.ISTINT_ComposedById = composeby;
                intrans.ISTINT_Interaction = data.ISTINT_Interaction;
                intrans.ISTINT_DateTime = indianTime;
                intrans.ISTINT_ComposedByFlg = composeflag;
                intrans.ISTINT_InteractionOrder = level_order;
                intrans.ISTINT_ActiveFlag = true;
                intrans.ISTINT_CreatedBy = data.UserId;
                intrans.ISTINT_UpdatedBy = data.UserId;
                intrans.ISTINT_ISPIPAddress = data.ISMINT_MACAddress;
                intrans.ISTINT_MACAddress = data.ISMINT_ISPIPAddress;
                intrans.CreatedDate = indianTime;
                intrans.UpdatedDate = indianTime;
                intrans.ISTINT_Attachment = image;
                intrans.ISTINT_ReadFlg = false;
                _PortalContext.Add(intrans);

                var contactExists = _PortalContext.SaveChanges();

                long ISTINT_Id3 = 0;
                var ISTINT_Id1 = _PortalContext.IVRM_School_Transaction_InteractionsDMO.OrderByDescending(a => a.ISTINT_Id).ToList();
                var ISTINT_Id2 = ISTINT_Id1.FirstOrDefault().ISTINT_Id;
                ISTINT_Id3 = ISTINT_Id2;

                if (contactExists > 0)
                {
                    data.returnval = true;
                    if (groupOrIndFlg == "Individual" && byflg == "Staff")
                    {
                        var employeedata = _PortalContext.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == intrans.ISTINT_ComposedById).Distinct().ToList();
                        notiSubject = employeedata.FirstOrDefault().HRME_EmployeeFirstName + ' ' + employeedata.FirstOrDefault().HRME_EmployeeMiddleName + ' ' + employeedata.FirstOrDefault().HRME_EmployeeLastName;

                    }
                    else if (groupOrIndFlg == "Individual" && byflg == "Student")
                    {
                        var studentdata = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_Id == intrans.ISTINT_ComposedById).Distinct().ToList();
                        notiSubject = studentdata.FirstOrDefault().AMST_FirstName + ' ' + studentdata.FirstOrDefault().AMST_MiddleName + ' ' + studentdata.FirstOrDefault().AMST_LastName;
                    }
                    else if (groupOrIndFlg == "Group")
                    {
                        notiSubject = "Group Message";
                    }

                    //============================== Notification
                    var deviceidsnew = "";
                    var devicenew = "";

                    if (devicelist.Count > 0)
                    {
                        int k = 0;
                        foreach (var device_id in devicelist)
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + device_id.AppDownloadedDeviceId + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + device_id.AppDownloadedDeviceId + '"';
                            }
                        }
                        devicenew = "[" + deviceidsnew + "]";

                        //callnotification(devicenew, notiSubject, intrans.ISTINT_Id, data.MI_Id, ISTINT_Id3, data);
                    }
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<AdmissionDTO.ComposeOnloadInteractionsDTO> intractioncomposeOnload(AdmissionDTO.ComposeOnloadInteractionsDTO data)
        {
            try
            {
                var type = _PortalContext.ApplicationUserRole.Where(a => a.UserId == data.UserId).ToList();
                var role = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == type[0].RoleTypeId).ToArray();

                data.configflag = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    var clsid = (from a in _PortalContext.Adm_M_Student
                                 from b in _PortalContext.School_M_Class
                                 from c in _PortalContext.School_M_Section
                                 from d in _PortalContext.School_Adm_Y_StudentDMO
                                 from e in _PortalContext.AcademicYearDMO
                                 where (a.AMST_Id == d.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMST_Id == data.AMST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                 select new AdmissionDTO.ComposeOnloadInteractionsDTO
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMS_Id = c.ASMS_Id,
                                     ASMC_SectionName = c.ASMC_SectionName,
                                     ASMCL_ClassName = b.ASMCL_ClassName
                                 }
                         ).Distinct().ToArray();


                    if (clsid.Length > 0)
                    {
                        data.asmclid = clsid.FirstOrDefault().ASMCL_Id;
                        data.asmsid = clsid.FirstOrDefault().ASMS_Id;
                    }

                }
                else if (data.roleflg != "student")
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_filter";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@IINTS_Flag",
                    SqlDbType.VarChar)
                    {
                        Value = data.userflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@roletype",
                   SqlDbType.VarChar)
                    {
                        Value = role[0].IVRMRT_Role
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<AdmissionDTO.composeOnselectOFTeacher> composeOnselectOFTeacher(AdmissionDTO.composeOnselectOFTeacher data)
        {
            try
            {
                var type = _PortalContext.ApplicationUserRole.Where(a => a.UserId == data.UserId).ToList();
                var role = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == type[0].RoleTypeId).ToArray();

                data.configflag = _PortalContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    var clsid = (from a in _PortalContext.Adm_M_Student
                                 from b in _PortalContext.School_M_Class
                                 from c in _PortalContext.School_M_Section
                                 from d in _PortalContext.School_Adm_Y_StudentDMO
                                 from e in _PortalContext.AcademicYearDMO
                                 where (a.AMST_Id == d.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ASMAY_Id == e.ASMAY_Id && a.MI_Id == b.MI_Id && c.MI_Id == e.MI_Id && a.AMST_Id == data.AMST_Id && d.ASMAY_Id == data.ASMAY_Id)
                                 select new AdmissionDTO.composeOnselectOFTeacher
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMS_Id = c.ASMS_Id,
                                     ASMC_SectionName = c.ASMC_SectionName,
                                     ASMCL_ClassName = b.ASMCL_ClassName
                                 }
                         ).Distinct().ToArray();


                    if (clsid.Length > 0)
                    {
                        data.asmclid = clsid.FirstOrDefault().ASMCL_Id;
                        data.asmsid = clsid.FirstOrDefault().ASMS_Id;
                    }

                }
                else if (data.roleflg != "student")
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_filter";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@IINTS_Flag",
                    SqlDbType.VarChar)
                    {
                        Value = data.userflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@roletype",
                   SqlDbType.VarChar)
                    {
                        Value = role[0].IVRMRT_Role
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public AdmissionDTO.composeOnsubmitOFStudent ComposeStudentSubmit(AdmissionDTO.composeOnsubmitOFStudent data)
        {
            try
            {

                string image = "";
                if (data.images_paths != null)
                {
                    foreach (var ig in data.images_paths)
                    {
                        image = ig;
                    }
                }
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                long ismint_Id = 0;
                long istint_Id = 0;
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                List<AdmissionDTO.composeOnsubmitOFStudent> devicelist = new List<AdmissionDTO.composeOnsubmitOFStudent>();
                //IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();

                MM = _db.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "InteractionStudent").ToList();
                if (MM.Count() > 0)
                {

                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                int level_no = 1;


                if (data.roleflg.Equals("student", StringComparison.OrdinalIgnoreCase))
                {


                    IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                    inter.MI_Id = data.MI_Id;
                    inter.ISMINT_InteractionId = data.trans_id;
                    inter.ASMAY_Id = data.ASMAY_Id;
                    inter.ISMINT_ComposedByFlg = data.roleflg;
                    inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                    inter.ISMINT_ComposedById = data.AMST_Id;
                    inter.ISMINT_Subject = data.ISMINT_Subject;
                    inter.ISMINT_DateTime = indianTime;
                    inter.ISMINT_Interaction = data.ISMINT_Interaction;
                    inter.ISMINT_ActiveFlag = true;
                    inter.ISMINT_CreatedBy = data.UserId;
                    inter.ISMINT_UpdatedBy = data.UserId;
                    inter.CreatedDate = indianTime;
                    inter.UpdatedDate = indianTime;
                    inter.ISMINT_Attachment = image;

                    inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                    inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                    _PortalContext.Add(inter);

                    //IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                    data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                      where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                      select new AdmissionDTO.composeOnsubmitOFStudent
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_MobileNo = a.HRME_MobileNo,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                          employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToArray();

                    var devlist1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                    where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                    select new AdmissionDTO.composeOnsubmitOFStudent
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                        employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                    }).Distinct().ToList();

                    AdmissionDTO.composeOnsubmitOFStudent dto = new AdmissionDTO.composeOnsubmitOFStudent();
                    data.devicelist12 = devlist1;

                    devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                  select new AdmissionDTO.composeOnsubmitOFStudent
                                  {
                                      HRME_Id = a.HRME_Id,
                                      AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                      employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToList();
                    IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                    intrans.ISMINT_Id = inter.ISMINT_Id;
                    intrans.ISTINT_ToId = data.ISTINT_ToId;
                    intrans.ISTINT_ToFlg = "Staff";
                    intrans.ISTINT_ComposedById = data.AMST_Id;
                    intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                    intrans.ISTINT_DateTime = indianTime;
                    intrans.ISTINT_ComposedByFlg = data.roleflg;
                    intrans.ISTINT_InteractionOrder = level_no;
                    intrans.ISTINT_ActiveFlag = true;
                    intrans.ISTINT_CreatedBy = data.UserId;
                    intrans.ISTINT_UpdatedBy = data.UserId;
                    intrans.ISTINT_Attachment = image;
                    intrans.CreatedDate = DateTime.Now;
                    intrans.UpdatedDate = DateTime.Now;
                    intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                    intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                    _PortalContext.Add(intrans);
                    ismint_Id = inter.ISMINT_Id;
                    istint_Id = intrans.ISTINT_Id;
                    var contactExists = _PortalContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        //============================== Notification
                        var deviceidsnew = "";
                        var devicenew = "";
                        //ismint_Id = intrans.ISMINT_Id;
                        //istint_Id = intrans.ISTINT_Id;
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";

                            //callnotificationNew(devicenew, data.ISMINT_Subject, istint_Id, data.MI_Id, dto);
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    if (data.ISMINT_GroupOrIndFlg == "Group")
                    {
                        IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                        inter.MI_Id = data.MI_Id;
                        inter.ISMINT_InteractionId = data.trans_id;
                        inter.ASMAY_Id = data.ASMAY_Id;
                        inter.ISMINT_ComposedByFlg = data.roleflg;
                        inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                        inter.ISMINT_ComposedById = data.HRME_Id;
                        inter.ISMINT_Subject = data.ISMINT_Subject;
                        inter.ISMINT_DateTime = indianTime;
                        inter.ISMINT_Interaction = data.ISMINT_Interaction;
                        inter.ISMINT_ActiveFlag = true;
                        inter.ISMINT_CreatedBy = data.HRME_Id;
                        inter.ISMINT_UpdatedBy = data.HRME_Id;
                        inter.CreatedDate = indianTime;
                        inter.UpdatedDate = indianTime;
                        inter.ISMINT_Attachment = image;
                        inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                        inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                        _PortalContext.Add(inter);

                        if (data.userflag == "Student")
                        {
                            List<long> device_ids = new List<long>();
                            foreach (var s in data.arrayStudent)
                            {
                                device_ids.Add(s.AMST_Id);

                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();

                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = s.AMST_Id;
                                intrans.ISTINT_ToFlg = "Student";
                                intrans.ISTINT_ComposedById = data.HRME_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.ISTINT_Attachment = image;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                                intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                                _PortalContext.Add(intrans);
                                istint_Id = intrans.ISTINT_Id;
                            }
                            ismint_Id = inter.ISMINT_Id;
                            data.deviceids = (from a in _PortalContext.Adm_M_Student
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                              select new AdmissionDTO.composeOnsubmitOFStudent
                                              {
                                                  HRME_MobileNo = a.AMST_MobileNo,
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                                  studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                              }).Distinct().ToArray();
                            var devi = (from a in _PortalContext.Adm_M_Student
                                        where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                        select new AdmissionDTO.composeOnsubmitOFStudent
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                            studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                        }).Distinct().ToList();
                            data.devicelist12 = devi;

                            devicelist = (from a in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.AMST_Id))
                                          select new AdmissionDTO.composeOnsubmitOFStudent
                                          {
                                              AMST_Id = a.AMST_Id,
                                              AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                              studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                          }).Distinct().ToList();

                        }
                        else if (data.userflag == "Teachers")
                        {

                            List<long> device_ids = new List<long>();
                            foreach (var t in data.arrayTeachers)
                            {
                                device_ids.Add(t.HRME_Id);
                                IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                                intrans.ISMINT_Id = inter.ISMINT_Id;
                                intrans.ISTINT_ToId = t.HRME_Id;
                                intrans.ISTINT_ToFlg = "Staff";
                                intrans.ISTINT_ComposedById = data.HRME_Id;
                                intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                                intrans.ISTINT_DateTime = indianTime;
                                intrans.ISTINT_ComposedByFlg = data.roleflg;
                                intrans.ISTINT_InteractionOrder = level_no;
                                intrans.ISTINT_ActiveFlag = true;
                                intrans.ISTINT_CreatedBy = data.UserId;
                                intrans.ISTINT_UpdatedBy = data.UserId;
                                intrans.CreatedDate = DateTime.Now;
                                intrans.UpdatedDate = DateTime.Now;
                                intrans.ISTINT_Attachment = image;
                                intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                                intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                                _PortalContext.Add(intrans);
                                istint_Id = intrans.ISTINT_Id;
                            }
                            ismint_Id = inter.ISMINT_Id;
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                              select new AdmissionDTO.composeOnsubmitOFStudent
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist = (from a in _PortalContext.HR_Master_Employee_DMO
                                           where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                           select new AdmissionDTO.composeOnsubmitOFStudent
                                           {
                                               HRME_MobileNo = a.HRME_MobileNo,
                                               HRME_Id = a.HRME_Id,
                                               HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                               employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                           }).Distinct().ToList();
                            data.devicelist12 = devlist;

                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && device_ids.Contains(a.HRME_Id))
                                          select new AdmissionDTO.composeOnsubmitOFStudent
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                        }
                        else
                        {
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new AdmissionDTO.composeOnsubmitOFStudent
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                            where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                            select new AdmissionDTO.composeOnsubmitOFStudent
                                            {
                                                HRME_Id = a.HRME_Id,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                            }).Distinct().ToList();
                            data.devicelist12 = devlist1;


                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                          select new AdmissionDTO.composeOnsubmitOFStudent
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                        }
                    }
                    else if (data.ISMINT_GroupOrIndFlg == "Individual")
                    {
                        var level_order = 1;
                        if (data.userflag == "Student")
                        {
                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }

                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            inter.ISMINT_Attachment = image;
                            inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                            inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(inter);

                            data.deviceids = (from a in _PortalContext.Adm_M_Student
                                              where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                              select new AdmissionDTO.composeOnsubmitOFStudent
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                                  studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                              }).Distinct().ToArray();

                            var slist = (from a in _PortalContext.Adm_M_Student
                                         where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                         select new AdmissionDTO.composeOnsubmitOFStudent
                                         {
                                             AMST_MobileNo = a.AMST_MobileNo,
                                             AMST_Id = a.AMST_Id,
                                             AMST_AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                             studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                         }).Distinct().ToList();
                            data.devicelist12 = slist;

                            devicelist = (from a in _PortalContext.Adm_M_Student
                                          where (a.MI_Id == data.MI_Id && a.AMST_Id == data.student_Id)
                                          select new AdmissionDTO.composeOnsubmitOFStudent
                                          {
                                              AMST_Id = a.AMST_Id,
                                              AppDownloadedDeviceId = a.AMST_AppDownloadedDeviceId,
                                              studentName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "  " : "  " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "  " : "  " + a.AMST_LastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.student_Id;
                            intrans.ISTINT_ToFlg = "Student";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                        }
                        else if (data.userflag == "Teachers")
                        {
                            data.trans_id = "";
                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }
                            //if (orderno == 0)
                            //{
                            //    orderno = orderno + 1;
                            //    level_order = level_no;
                            //}
                            //else
                            //{
                            //    level_order = level_order + 1;
                            //}
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                            inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            inter.ISMINT_Attachment = image;

                            _PortalContext.Add(inter);

                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                              select new AdmissionDTO.composeOnsubmitOFStudent
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();
                            var sss = (from a in _PortalContext.HR_Master_Employee_DMO
                                       where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                       select new AdmissionDTO.composeOnsubmitOFStudent
                                       {
                                           HRME_MobileNo = a.HRME_MobileNo,
                                           HRME_Id = a.HRME_Id,
                                           HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                           employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                       }).Distinct().ToList();
                            data.devicelist12 = sss;
                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == data.employee_Id)
                                          select new AdmissionDTO.composeOnsubmitOFStudent
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.employee_Id;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_order;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                        }
                        else
                        {
                            IVRM_School_Master_InteractionsDMO inter = new IVRM_School_Master_InteractionsDMO();
                            inter.MI_Id = data.MI_Id;
                            inter.ISMINT_InteractionId = data.trans_id;
                            inter.ASMAY_Id = data.ASMAY_Id;
                            inter.ISMINT_ComposedByFlg = data.roleflg;
                            inter.ISMINT_GroupOrIndFlg = data.ISMINT_GroupOrIndFlg;
                            inter.ISMINT_ComposedById = data.HRME_Id;
                            inter.ISMINT_Subject = data.ISMINT_Subject;
                            inter.ISMINT_DateTime = indianTime;
                            inter.ISMINT_Interaction = data.ISMINT_Interaction;
                            inter.ISMINT_ActiveFlag = true;
                            inter.ISMINT_CreatedBy = data.UserId;
                            inter.ISMINT_UpdatedBy = data.UserId;
                            inter.CreatedDate = indianTime;
                            inter.UpdatedDate = indianTime;
                            inter.ISMINT_Attachment = image;
                            inter.ISMINT_MACAddress = data.ISMINT_MACAddress;
                            inter.ISMINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            _PortalContext.Add(inter);
                            data.deviceids = (from a in _PortalContext.HR_Master_Employee_DMO
                                              where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                              select new AdmissionDTO.composeOnsubmitOFStudent
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                  employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                              }).Distinct().ToArray();

                            var devlist1 = (from a in _PortalContext.HR_Master_Employee_DMO
                                            where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                            select new AdmissionDTO.composeOnsubmitOFStudent
                                            {
                                                HRME_MobileNo = a.HRME_MobileNo,
                                                HRME_Id = a.HRME_Id,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                            }).Distinct().ToList();
                            data.devicelist12 = devlist1;

                            devicelist = (from a in _PortalContext.HR_Master_Employee_DMO
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == data.ISTINT_ToId)
                                          select new AdmissionDTO.composeOnsubmitOFStudent
                                          {
                                              HRME_Id = a.HRME_Id,
                                              AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                              employeeName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? "  " : "  " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? "  " : "  " + a.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().ToList();
                            IVRM_School_Transaction_InteractionsDMO intrans = new IVRM_School_Transaction_InteractionsDMO();
                            intrans.ISMINT_Id = inter.ISMINT_Id;
                            intrans.ISTINT_ToId = data.ISTINT_ToId;
                            intrans.ISTINT_ToFlg = "Staff";
                            intrans.ISTINT_ComposedById = data.HRME_Id;
                            intrans.ISTINT_Interaction = data.ISMINT_Interaction;
                            intrans.ISTINT_DateTime = indianTime;
                            intrans.ISTINT_ComposedByFlg = data.roleflg;
                            intrans.ISTINT_InteractionOrder = level_no;
                            intrans.ISTINT_ActiveFlag = true;
                            intrans.ISTINT_CreatedBy = data.UserId;
                            intrans.ISTINT_UpdatedBy = data.UserId;
                            intrans.ISTINT_ISPIPAddress = data.ISMINT_ISPIPAddress;
                            intrans.ISTINT_MACAddress = data.ISMINT_MACAddress;
                            intrans.CreatedDate = DateTime.Now;
                            intrans.UpdatedDate = DateTime.Now;
                            intrans.ISTINT_Attachment = image;
                            _PortalContext.Add(intrans);
                            ismint_Id = inter.ISMINT_Id;
                            istint_Id = intrans.ISTINT_Id;
                        }
                    }
                    var contactExists = _PortalContext.SaveChanges();



                    long istint_Id3 = 0;
                    var istint_Id1 = _PortalContext.IVRM_School_Master_InteractionsDMO.OrderByDescending(a => a.ISMINT_Id).ToList();
                    var istint_Id2 = istint_Id1.FirstOrDefault().ISMINT_Id;
                    istint_Id3 = istint_Id2;
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        //============================== Notification
                        var deviceidsnew = "";
                        var devicenew = "";
                        // ismint_Id = intrans.ISMINT_Id;
                        //istint_Id = intrans.ISTINT_Id;                        
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";
                            // callnotificationNew(devicenew, data.ISMINT_Subject, istint_Id3, data.MI_Id, data);
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //added by kavita
        //class wise timetable
        public AdmissionDTO.ttLoadData ttgetloaddata(AdmissionDTO.ttLoadData data)
        {
            try
            {


                data.getyear = (from d in _PortalContext.AcademicYearDMO
                                from a in _PortalContext.School_M_Class
                                from b in _PortalContext.School_M_Section
                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                select new AdmissionDTO.ttLoadData
                                {
                                    ASMCL_Id = c.ASMCL_Id,
                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                    ASMS_Id = c.ASMS_Id,
                                    ASMC_SectionName = b.ASMC_SectionName,
                                    ASMAY_Id = c.ASMAY_Id,
                                    ASMAY_Year = d.ASMAY_Year
                                }
                           ).OrderBy(y => y.ASMAY_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionDTO.ttGetStudent getStudentTT(AdmissionDTO.ttGetStudent data)
        {
            List<AdmissionDTO.ttGetStudent> list = new List<AdmissionDTO.ttGetStudent>();

            try
            {
                var clssec1 = (from a in _PortalContext.Adm_M_Student
                               from b in _PortalContext.School_Adm_Y_StudentDMO
                               from c in _PortalContext.School_M_Class
                               from s in _PortalContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new AdmissionDTO.ttGetStudent
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                data.alldata = clssec1.ToArray();

                var temp_class = clssec1.FirstOrDefault().ASMCL_Id;
                var temp_section = clssec1.FirstOrDefault().ASMS_Id;


                var cateid = _PortalContext.TT_Category_Class_DMO.Where(c => c.ASMCL_Id == temp_class && c.ASMAY_Id == data.ASMAY_Id && c.TTCC_ActiveFlag == true).Distinct().ToList();

                long temp_cate = cateid.FirstOrDefault().TTMC_Id;

                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                //allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList();

                allperiods = (from a in _PortalContext.TT_Master_PeriodDMO
                              from b in _PortalContext.TT_Master_Period_ClasswiseDMO
                              where a.MI_Id == data.MI_Id && a.TTMP_Id == b.TTMP_Id && a.TTMP_ActiveFlag == true && b.TTMPC_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == temp_class
                              select a).Distinct().ToList();


                data.periodslst = allperiods.ToArray();

                data.gridweeks = _PortalContext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
                List<AdmissionDTO.ttGetStudent> breaks = new List<AdmissionDTO.ttGetStudent>();
                List<TTBreakTimeSettingsDMO> breaks_all = new List<TTBreakTimeSettingsDMO>();

                try
                {

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_weekly_timetable";
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
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                            data.TT = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                //foreach (AdmissionDTO.ttGetStudent dto in data.getStudentTT)
                //{
                //    list.Add(dto);
                //}

                data.Break_list = (from a in _PortalContext.TTBreakTimeSettingsDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == temp_class && a.TTMB_ActiveFlag == true && a.TTMC_Id == temp_cate)
                                   select new AdmissionDTO.ttGetStudent
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                       TTMB_AfterPeriod = a.TTMB_AfterPeriod,
                                       type = a.TTMB_BreakName
                                   }).Distinct().OrderBy(x => x.TTMB_AfterPeriod).ToArray();

                List<TTBreakTimeSettingsDMO> break_cls = new List<TTBreakTimeSettingsDMO>();
                break_cls = _PortalContext.TTBreakTimeSettingsDMO.AsNoTracking().Where(b => b.ASMCL_Id == temp_class && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == temp_cate && b.MI_Id == data.MI_Id && b.TTMB_ActiveFlag == true).OrderBy(x => x.TTMB_AfterPeriod).ToList();
                data.Break_list_all = break_cls.ToArray();


                foreach (AdmissionDTO.ttGetStudent dto in data.Break_list)
                {
                    breaks.Add(dto);
                }
                foreach (TTBreakTimeSettingsDMO dmo in data.Break_list_all)
                {
                    breaks_all.Add(dmo);
                }

                //  data.TT = list.ToArray();
                data.TT_Break_list = breaks.ToArray();
                data.TT_Break_list_all = breaks_all.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //class wise timetable
        //attendance
        public AdmissionDTO.attGetLoadData Attgetloaddata(AdmissionDTO.attGetLoadData data)
        {
            try
            {
                data.attyearlist = (from d in _PortalContext.AcademicYearDMO
                                    from a in _PortalContext.School_M_Class
                                    from b in _PortalContext.School_M_Section
                                    from c in _PortalContext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new AdmissionDTO.attGetLoadData
                                    {
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        ASMAY_Order = d.ASMAY_Order
                                    }
                             ).OrderByDescending(T => T.ASMAY_Order).ToArray();

                data.currentyear = (from a in _PortalContext.School_Adm_Y_StudentDMO
                                    from b in _PortalContext.AcademicYearDMO
                                    where (b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new AdmissionDTO.attGetLoadData
                                    {
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMAY_Year = b.ASMAY_Year,
                                        ASMAY_Order = b.ASMAY_Order
                                    }
                           ).OrderByDescending(T => T.ASMAY_Order).ToArray();
                data.status = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }

        public AdmissionDTO.attGetdetails attGetdetails(AdmissionDTO.attGetdetails data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
             SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.attList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //attendance

        //added by sanjeev
        //onclick_LIB
        public AdmissionDTO.onclick_LIB onclick_LIB(AdmissionDTO.onclick_LIB data)
        {
            try
            {
                data.status = true;

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_LibraryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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
                        data.librarydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_TRANSACTION_GRAPHS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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
                        data.librarygraphs = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }

        //onclick_LIBstaff
        public AdmissionDTO.onclick_LIB onclick_LIBstaff(AdmissionDTO.onclick_LIB data)
        {
            try
            {
                data.status = true;

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_LibraryDetails_Staff";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
              SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.librarydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_TRANSACTION_GRAPHS_Staff";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
              SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.librarygraphs = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        public AdmissionDTO.getCoedata getloaddataCoe(AdmissionDTO.getCoedata data)
        {
            try
            {
                data.status = true;
                data.currentyear = (from a in _PortalContext.School_Adm_Y_StudentDMO
                                    from b in _PortalContext.AcademicYearDMO
                                    where (b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.mI_ID
                                    )
                                    select new AdmissionDTO.getCoedata
                                    {
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMAY_Year = b.ASMAY_Year,
                                        ASMAY_Order = b.ASMAY_Order
                                    }
                           ).OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.coeyearlist = (from d in _PortalContext.AcademicYearDMO
                                    from a in _PortalContext.School_M_Class
                                    from b in _PortalContext.School_M_Section
                                    from c in _PortalContext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.mI_ID && b.MI_Id == data.mI_ID && d.MI_Id == data.mI_ID)
                                    select new AdmissionDTO.getCoedata
                                    {
                                        ASMCL_Id = c.ASMCL_Id,
                                        ASMCL_ClassName = a.ASMCL_ClassName,
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        ASMAY_Order = d.ASMAY_Order
                                    }
                             ).OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        //getcoedata
        public AdmissionDTO.getCoedata getcoedata(AdmissionDTO.getCoedata data)
        {
            try
            {
                data.status = true;
                var clssec1 = (from a in _PortalContext.Adm_M_Student
                               from b in _PortalContext.School_Adm_Y_StudentDMO
                               from c in _PortalContext.School_M_Class
                               from s in _PortalContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.mI_ID
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new AdmissionDTO.getCoedata
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                data.coereportlist = (from a in _PortalContext.COE_Master_EventsDMO
                                      from b in _PortalContext.COE_EventsDMO
                                      from c in _PortalContext.School_Adm_Y_StudentDMO
                                      from d in _PortalContext.Adm_M_Student
                                      from e in _PortalContext.COE_Events_ClassesDMO
                                      from f in _PortalContext.COE_Events_ImagesDMO
                                      from g in _PortalContext.COE_Events_VideosDMO
                                      where (a.COEME_Id == b.COEME_Id && b.COEE_Id == e.COEE_Id && b.COEE_Id == f.COEE_Id && b.COEE_Id == g.COEE_Id && b.ASMAY_Id == c.ASMAY_Id && c.AMST_Id == d.AMST_Id && c.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.MI_Id == data.mI_ID && b.COEE_EStartDate.Value.Month == data.month)
                                      select new AdmissionDTO.getCoedata
                                      {
                                          COEME_EventName = a.COEME_EventName,
                                          COEME_EventDesc = a.COEME_EventDesc,
                                          COEE_EStartDate = b.COEE_EStartDate,
                                          COEE_EEndDate = b.COEE_EEndDate,
                                          COEEI_Images = f.COEEI_Images,
                                          COEEV_Videos = g.COEEV_Videos,
                                          COEE_EStartTime = b.COEE_EStartTime,
                                          COEE_EEndTime = b.COEE_EEndTime,
                                          ASMAY_Id = b.ASMAY_Id
                                      }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }

        //added by roopa
        public AdmissionDTO.onclickClasswork onclick_Classwork_load(AdmissionDTO.onclickClasswork dto)
        {
            try
            {

                dto.yearlist = _PortalContext.AcademicYearDMO.Where(a => a.MI_Id == dto.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();


                var clssec1 = _PortalContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
               && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_StudentDashboard";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });


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
                            dto.studetailslist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_Student_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Classwork" });

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
                            dto.assignmentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public AdmissionDTO.onclick_Homework_load onclick_Homework_load(AdmissionDTO.onclick_Homework_load dto)
        {
            try
            {
                dto.yearlist = _PortalContext.AcademicYearDMO.Where(a => a.MI_Id == dto.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var clssec1 = _PortalContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
                  && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_Student_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Homework" });

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
                            dto.homeworklist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public AdmissionDTO.onclick_notice onclick_notice(AdmissionDTO.onclick_notice dto)
        {
            try
            {



                var clssec1 = _PortalContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
                && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = Class_Id
                            // Value=dto.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.VarChar)
                        {
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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
                            dto.noticelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //save getClassworksave
        public AdmissionDTO.getClassworksave savecls_doc(AdmissionDTO.getClassworksave dto)
        {
            try
            {
                var check = _PortalContext.IVRM_ClassWork_Upload_DMO_con.Where(a => a.ICW_Id == dto.ICW_Id && a.AMST_Id == dto.AMST_Id).ToList();
                if (check.Count > 0)
                {
                    var getclassworkattach = _PortalContext.IVRM_ClassWork_Upload_Attatchment_DMO_con.Where(a => a.ICWUPL_Id == check.FirstOrDefault().ICWUPL_Id).ToList();

                    foreach (var d in getclassworkattach)
                    {
                        _PortalContext.Remove(d);
                    }

                    //_studentDashboardContext.Remove(check);
                }

                IVRM_ClassWork_Upload_DMO iVRM_ClassWork_Upload_DMO = new IVRM_ClassWork_Upload_DMO();
                iVRM_ClassWork_Upload_DMO.AMST_Id = dto.AMST_Id;
                iVRM_ClassWork_Upload_DMO.ICW_Id = dto.ICW_Id;
                iVRM_ClassWork_Upload_DMO.ICWUPL_Date = DateTime.Now;
                iVRM_ClassWork_Upload_DMO.ICWUPL_ActiveFlag = true;
                iVRM_ClassWork_Upload_DMO.CreatedDate = DateTime.Now;
                iVRM_ClassWork_Upload_DMO.UpdatedDate = DateTime.Now;
                _PortalContext.Add(iVRM_ClassWork_Upload_DMO);

                foreach (var item in dto.uploaddoc_array)
                {
                    if (item.DCO_Attachment != null && item.DCO_Attachment != "")
                    {
                        IVRM_ClassWork_Upload_Attatchment_DMO cud = new IVRM_ClassWork_Upload_Attatchment_DMO();
                        cud.ICWUPL_Id = iVRM_ClassWork_Upload_DMO.ICWUPL_Id;
                        cud.ICWUPLATT_FileName = item.Doc_FileName;
                        cud.ICWUPLATT_Attachment = item.DCO_Attachment;
                        cud.ICWUPLATT_ActiveFlag = true;
                        cud.ICWUPLATT_CreatedDate = DateTime.Now;
                        cud.ICWUPLATT_UpdatedDate = DateTime.Now;
                        _PortalContext.Add(cud);
                    }
                }

                var result = _PortalContext.IVRM_ClassWorkDMO.Single(a => a.ICW_Id == dto.ICW_Id && a.MI_Id == dto.MI_Id);
                result.ICW_FilePath = "1";
                _PortalContext.Update(result);
                var std = _PortalContext.SaveChanges();
                if (std > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public AdmissionDTO.gethomeworksave savehome_doc(AdmissionDTO.gethomeworksave dto)
        {
            try
            {
                var check = _PortalContext.IVRM_HomeWork_Upload_DMO_con.Where(a => a.IHW_Id == dto.IHW_Id && a.AMST_Id == dto.AMST_Id).ToList();
                if (check.Count > 0)
                {
                    var gethomeworkattach = _PortalContext.IVRM_HomeWork_Upload_Attatchment_DMO_con.Where(a => a.IHWUPL_Id == check.FirstOrDefault().IHWUPL_Id).ToList();

                    foreach (var d in gethomeworkattach)
                    {
                        _PortalContext.Remove(d);
                    }

                    //_studentDashboardContext.Remove(check);
                }

                IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();
                cud.AMST_Id = dto.AMST_Id;
                cud.IHW_Id = dto.IHW_Id;
                cud.IHWUPL_Date = DateTime.Now;
                cud.CreatedDate = DateTime.Now;
                cud.UpdatedDate = DateTime.Now;
                cud.IHWUPL_ActiveFlag = true;
                _PortalContext.Add(cud);

                foreach (var item in dto.uploaddoc_array)
                {
                    if (item.DCO_Attachment != null && item.DCO_Attachment != "")
                    {
                        IVRM_HomeWork_Upload_Attatchment_DMO iVRM_HomeWork_Upload_Attatchment_DMO = new IVRM_HomeWork_Upload_Attatchment_DMO();
                        iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPL_Id = cud.IHWUPL_Id;
                        iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_FileName = item.Doc_FileName;
                        iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_Attachment = item.DCO_Attachment;
                        iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_ActiveFlag = true;
                        iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_CreatedDate = DateTime.Now;
                        iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_UpdatedDate = DateTime.Now;
                        _PortalContext.Add(iVRM_HomeWork_Upload_Attatchment_DMO);
                    }
                }
                var result = _PortalContext.IVRM_Homework_DMO.Single(a => a.IHW_Id == dto.IHW_Id && a.MI_Id == dto.MI_Id);
                result.IHW_FilePath = "1";
                _PortalContext.Update(result);
                var std = _PortalContext.SaveChanges();
                if (std > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public AdmissionDTO.stdDashboardLoad stdDashboardDet(AdmissionDTO.stdDashboardLoad data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_BirthdayCnt";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    //

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
                        data.birthdayList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_COECnt";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.calList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_AttendanceCnt";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                        data.attendanceList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_FeesCnt";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.feesList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_timetable";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.timeTableList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_examMarks";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.examList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionDTO.getstudent stdFeeDue(AdmissionDTO.getstudent data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FEE_Balance_Amount_Show_in_portal";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
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
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (data.getstudentdetails.Length > 0)
                {
                    //var fee = _PortalContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "FEEPENDING").ToList();
                    //if (fee.Count() > 0)
                    //{
                    //    //data.feedetails = fee[0].ISES_MailHTMLTemplate;
                    //   // data.AMST_Photoname = duadatecollect(data.MI_Id, data.AMST_Id, data.ASMCL_Id, data.ASMAY_Id, "FEEPENDING");
                    //}
                    //data.feedetails = fee[0].ISES_MailHTMLTemplate;

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "StudentportalFeeDue";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
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
                            data.studentDue = retObject.ToArray();
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

        //attendance
        //public AdmissionDTO.getstudent stdFeeDue(AdmissionDTO.getstudent data)
        //{
        //    try
        //    {
        //        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "FEE_Balance_Amount_Show_in_portal";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //     SqlDbType.VarChar)
        //            {
        //                Value = data.MI_Id
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //     SqlDbType.VarChar)
        //            {
        //                Value = data.AMST_Id
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
        //            SqlDbType.VarChar)
        //            {
        //                Value = data.ASMCL_Id
        //            });
        //            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //        SqlDbType.VarChar)
        //            {
        //                Value = data.ASMAY_Id
        //            });

        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();
        //            try
        //            {
        //                using (var dataReader = cmd.ExecuteReader())
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                        {
        //                            dataRow.Add(
        //                                dataReader.GetName(iFiled),
        //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                            );
        //                        }

        //                        retObject.Add((ExpandoObject)dataRow);
        //                    }
        //                }
        //                data.getstudentdetails = retObject.ToArray();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }

        //        if (data.getstudentdetails.Length > 0)
        //        {
        //            var fee = _PortalContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "FEEPENDING").ToList();
        //            if (fee.Count() > 0)
        //            {
        //                //data.feedetails = fee[0].ISES_MailHTMLTemplate;
        //                data.AMST_Photoname = duadatecollect(data.MI_Id, data.AMST_Id, data.ASMCL_Id, data.ASMAY_Id, "FEEPENDING");
        //            }
        //            //data.feedetails = fee[0].ISES_MailHTMLTemplate;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}

        public AdmissionDTO.daywiseTimetable daywiseTimetable(AdmissionDTO.daywiseTimetable data)
        {
            try
            {

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_timetable_daywise";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@dayname",
             SqlDbType.VarChar)
                    {
                        Value = data.dayname
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
                        data.daywiseTimeTableList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.AcademicFeesData AcademicwiseFeesDetails(AdmissionDTO.AcademicFeesData data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AcademicWiseFeesManagerDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.AcademicFeesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.AcademicFeesData AcademicwiseClassFeesDetails(AdmissionDTO.AcademicFeesData data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AcademicWiseFeesClasswiseManager";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
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
                        data.AcademicFeesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.versiondetails Mobileversion_control(AdmissionDTO.versiondetails data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MobileAppVersioncontrol";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Version",
                     SqlDbType.VarChar)
                    {
                        Value = data.version
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
                        data.versiondetailslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string duadatecollect(long MI_Id, long AMST_Id, long Class_Id_t, long ASMAY_Id, string templatename)
        {

            Dictionary<string, string> val = new Dictionary<string, string>();
            var template = _PortalContext.SMSEmailSetting.Where(a => a.MI_Id == MI_Id && a.ISES_Template_Name == templatename).ToList();

            var Paramaeters = _PortalContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

            var ParamaetersName = _PortalContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


            string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

            string result = Mailmsg;

            using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "FEE_Balance_Amount_Show_in_portalnotification";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id",
         SqlDbType.VarChar)
                {
                    Value = MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
         SqlDbType.VarChar)
                {
                    Value = AMST_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                {
                    Value = Class_Id_t
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            SqlDbType.VarChar)
                {
                    Value = ASMAY_Id
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
                        //result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                        Mailmsg = result;
                    }
                }
            }
            Mailmsg = result;


            return Mailmsg;
        }
        public AdmissionDTO.stdDashboardExam stdDashboardExam(AdmissionDTO.stdDashboardExam data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STD_Dashboard_MarksExamwise";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
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
                        data.examgraphList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.userDashboardLoad UserDashboardDetails(AdmissionDTO.userDashboardLoad data)
        {
            try
            {
                if (data.RoleType == "Student")
                {

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_Dashboard_BirthdayCnt";
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
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
                        });
                        //

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
                            data.birthdayList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_Dashboard_COECnt";
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
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                            data.calList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_Dashboard_AttendanceCnt";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
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
                            data.attendanceList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_Dashboard_FeesCnt";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                            data.feesList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_Dashboard_timetable";
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
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                            data.timeTableList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STD_Dashboard_examMarks";
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
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                            data.examList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else if (data.RoleType == "Staff")
                {
                    var HRME_Id = _PortalContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                    //COE
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Staff_Dashboard_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                         SqlDbType.VarChar)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                        {
                            Value = "COE"
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
                            data.calList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //Leave
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Staff_Dashboard_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                         SqlDbType.VarChar)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                        {
                            Value = "LEAVE"
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
                            data.leaveDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //TimeTable
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Staff_Dashboard_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                         SqlDbType.VarChar)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                        {
                            Value = "TimeTable"
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
                            data.timeTableDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //LOP
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Staff_Dashboard_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                         SqlDbType.VarChar)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar)
                        {
                            Value = "LOP"
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
                            data.lopDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //Punch
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Employee_Punch_Detail";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                           SqlDbType.BigInt)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                       SqlDbType.VarChar)
                        {
                            Value = DateTime.Now.Date.ToString("yyyy/MM/dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                      SqlDbType.NVarChar)
                        {
                            Value = DateTime.Now.Date.ToString("yyyy/MM/dd")
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();


                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {

                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.punchDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.RoleType == "Manager")
                {

                    data.ASMAY_Year = _PortalContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1 && t.ASMAY_From_Date >= DateTime.Now && t.ASMAY_To_Date <= DateTime.Now).Select(t => t.ASMAY_Year).FirstOrDefault();


                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ManagerDashboardCOE";
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
                            data.calList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ManagerDashboardLeaveDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
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
                            data.ManagerDashboardLeaveDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ManagerDashboardFeesDetails";
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
                            data.ManagerDashboardFeesDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ManagerDashboardFeesTotalbalance";
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
                            data.ManagerdashFeetotal = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ManagerDashboardPreadmissionDetails";
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
                            data.ManagerDashboardPreadmissionDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_LeaveApproval";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.leaveDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    data.leavecount = data.leaveDetails.Length;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionDTO.UserProfileDetailsDTO UserProfileDetails(AdmissionDTO.UserProfileDetailsDTO data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "User_profileDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@usercode",
                     SqlDbType.BigInt)
                    {
                        Value = data.usercode
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                     SqlDbType.BigInt)
                    {
                        Value = data.type
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
                        data.getuserdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Staff Dashboard
        public AdmissionDTO.staffDashboardLoad staffDashboardDetails(AdmissionDTO.staffDashboardLoad data)
        {
            try
            {

                var HRME_Id = _PortalContext.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();



                //COE
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                    {
                        Value = "COE"
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
                        data.calList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Leave
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                    {
                        Value = "LEAVE"
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
                        data.leaveDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //TimeTable
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                    {
                        Value = "TimeTable"
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
                        data.timeTableDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //LOP
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                    {
                        Value = "LOP"
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
                        data.lopDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Punch
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fo_Employee_Punch_Detail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.BigInt)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                    {
                        Value = DateTime.Now.Date.ToString("yyyy/MM/dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.NVarChar)
                    {
                        Value = DateTime.Now.Date.ToString("yyyy/MM/dd")
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.punchDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //Manager Dashboard
        public AdmissionDTO.ManagerDashboard ManagerDashboardDetails(AdmissionDTO.ManagerDashboard data)
        {
            try
            {


                data.ASMAY_Year = _PortalContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1 && t.ASMAY_From_Date >= DateTime.Now && t.ASMAY_To_Date <= DateTime.Now).Select(t => t.ASMAY_Year).FirstOrDefault();


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ManagerDashboardCOE";
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
                        data.ManagerDashboardCOE = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ManagerDashboardLeaveDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                        data.ManagerDashboardLeaveDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ManagerDashboardFeesDetails";
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
                        data.ManagerDashboardFeesDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ManagerDashboardFeesTotalbalance";
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
                        data.ManagerdashFeetotal = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ManagerDashboardPreadmissionDetails";
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
                        data.ManagerDashboardPreadmissionDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_LeaveApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader1.GetName(iFiled),
                                       dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_leavestatus = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.leavecount = data.get_leavestatus.Length;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //PushNotification
        public AdmissionDTO.PushNotification PushNotification(AdmissionDTO.PushNotification data)
        {
            try
            {

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_PushNotification";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.GetDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<AdmissionDTO.ShortageOFAttandence> shortageOfAttendanceAlert(AdmissionDTO.ShortageOFAttandence data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "AttendanceStudent_perc_shortageAlert";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.studentAttendanceList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                //if (stutList.Count > 0)
                //{

                //    //    ctdo.stutList = ctdo.stutList;
                //    for (int k = 0; k < stutList.Count; k++)
                //    {
                //        long MI_id = ctdo.miid;
                //        string mobileno = stutList[k].AMST_MobileNo.ToString();
                //        long AMST_Id = stutList[k].AMST_Id;

                //        if (mobileno.Length == 10)
                //        {

                //            try
                //            {
                //                //  sendSms(MI_id, mobileno, "Attendance_Auto_Schedular_EOD", AMST_Id, confromdate, ctdo.ASMAY_Id);
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine(ex.Message);
                //            }
                //        }

                //    }

                //    //if (ctdo.studentAttendanceList.Length == y)
                //    //{
                //    //    ctdo.message = "SMS Sent Successfully";
                //    //}
                //    //else
                //    //{
                //    //    ctdo.message = "SMS Sent Successfully , And Failed List '" + msg1 + "'";
                //    //}
                //}


            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.staffProfileDTO staffProfile(AdmissionDTO.staffProfileDTO data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_profile";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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
                        data.staffdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmissionDTO.PushNotificationonload PushNotificationonload(AdmissionDTO.PushNotificationonload data)
        {
            try
            {


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_NotificateInbox ";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@flag ",
             SqlDbType.VarChar)
                    {
                        Value = data.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Id ",
                     SqlDbType.VarChar)
                    {
                        Value = data.userid
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                        data.getpushnotifications = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionDTO.PushNotificationonload NotificationonloadRead(AdmissionDTO.PushNotificationonload data)
        {
            try
            {
                var contactExistsP = _PortalContext.Database.ExecuteSqlCommand("IVRM_NotificateUpdate @p0,@p1,@p2", data.PNSD_Id, data.ReadFlg, data.PNSDDE_Id);
                if (contactExistsP > 0)
                {
                    data.retrunMsg = "updated";
                }
                else
                {
                    data.retrunMsg = "notupdated";
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
