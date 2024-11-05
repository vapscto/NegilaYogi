using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Services
{
    public class TransferCertificateImpl : Interfaces.TransferCertificateInterface
    {
        public DomainModelMsSqlServerContext _db;
        private PortalContext _PortalContext;
        public TransferCertificateImpl(PortalContext studentDashboardContext, DomainModelMsSqlServerContext db)
        {
            _PortalContext = studentDashboardContext;
            _db = db;
        }
        public TransferCertificate_DTO getdetails(TransferCertificate_DTO data)
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
                                           select new TransferCertificate_DTO
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
                                     select new TransferCertificate_DTO
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
                                         //ACERTAPP_Id = cr.ACERTAPP_Id,
                                         //ASCA_Reason = a.ASCA_Reason,
                                         //ASMS_Id = sc.ASMS_Id,
                                         //ASMCL_Id = cl.ASMCL_Id,
                                         //AMST_RegistrationNo = c.AMST_RegistrationNo,
                                         //AMST_MobileNo = c.AMST_MobileNo,
                                         //AMST_emailId = c.AMST_emailId,
                                     }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();

                    data.certificatelist = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ACERTAPP_ActiveFlg == true).ToArray();


                }

                else if (data.roletype.Equals("Principal", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("chairman", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("admin", StringComparison.OrdinalIgnoreCase) || data.roletype.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    //data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                    //var clstchname = (from a in _db.Adm_SchAttLoginUserClass
                    //                  from b in _db.Adm_SchAttLoginUser
                    //                  from c in _db.School_M_Class
                    //                  where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                    //                  && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                    //                  && b.HRME_Id == data.HRME_Id
                    //                  && c.ASMCL_ActiveFlag == true)
                    //                  select new TransferCertificate_DTO
                    //                  {
                    //                      ASMCL_Id = c.ASMCL_Id,
                    //                      ASMCL_ClassName = c.ASMCL_ClassName,
                    //                      ASMS_Id = a.ASMS_Id,
                    //                  }
                    //              ).Distinct().ToArray();


                    data.applylist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.School_M_Class
                                      from d in _PortalContext.School_M_Section
                                      from e in _PortalContext.Adm_M_Student
                                      from cr in _PortalContext.Adm_Certificates_Apply_DMO_con
                                      where (a.MI_Id == c.MI_Id && b.AMST_Id == a.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.AMST_Id == e.AMST_Id && a.MI_Id == data.MI_Id && e.AMST_SOL == "S" && a.ASCA_ActiveFlg == true && a.ASCA_Status == "Pending" && a.ASCA_CertificateType == cr.ACERTAPP_CertificateCode)
                                      select new TransferCertificate_DTO
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
                                          select new TransferCertificate_DTO
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

        public async Task<TransferCertificate_DTO> tcApply(TransferCertificate_DTO data)
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
                                         select new ExamDTO
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
                                           select new TransferCertificate_DTO
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

        public TransferCertificate_DTO deactiveY(TransferCertificate_DTO data)
        {
            try
            {
                var result = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id);

                if (result.ASCA_ActiveFlg == true)
                {
                    result.ASCA_ActiveFlg = false;
                }
                else if (result.ASCA_ActiveFlg == false)
                {
                    result.ASCA_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _PortalContext.Update(result);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TransferCertificate_DTO editdata(TransferCertificate_DTO data)
        {
            try
            {
                var result = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id);
                if (result.ASCA_Status == "Approved")
                {
                    data.message = "Approved";
                }
                else
                {

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TransferCertificateEdit";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASCA_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASCA_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.editdata = retObject.ToArray();
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

        public async Task<TransferCertificate_DTO> certificateApproved(TransferCertificate_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASCA_Id > 0)
                {
                    var update = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id).SingleOrDefault();

                    update.ASCA_Status = "Approved";
                    update.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(update);

                    Adm_Students_Certificate_Approve_DMO obj2 = new Adm_Students_Certificate_Approve_DMO();

                    obj2.ASCA_Id = update.ASCA_Id;
                    obj2.MI_Id = data.MI_Id;
                    obj2.IVRMALU_Id = data.UserId;
                    obj2.ASCAP_Status = "Approved";
                    obj2.ASCAP_ApproveReason = data.ASCAP_ApproveReason;
                    obj2.ASCAP_ApproveDate = DateTime.Now;
                    obj2.ASCAP_ActiveFlg = true;
                    obj2.CreatedDate = DateTime.Now;
                    obj2.UpdatedDate = DateTime.Now;

                    _PortalContext.Add(obj2);
                    _PortalContext.Update(update);
                    int rowAffected = _PortalContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        var asmstid = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(a => a.ASCA_Id == data.ASCA_Id).AMST_Id;
                        var mobilemail = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == asmstid && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S").Distinct().ToList();


                        long mobileno = 0;
                        var emailid = "";
                        if (mobilemail.Count > 0)
                        {
                            mobileno = mobilemail.FirstOrDefault().AMST_MobileNo;
                            SMS sms = new SMS(_db);
                            s = await sms.sendCertificateSms(data.MI_Id, mobileno, "TCSTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (mobilemail.Count > 0)
                        {
                            emailid = mobilemail.FirstOrDefault().AMST_emailId;
                            Email Email = new Email(_db);
                            m = Email.sendCertificatemail(data.MI_Id, emailid, "TCSTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                        //SMS sms = new SMS(_db);


                        ////   s = await sms.sendSms(data.MI_Id, mobileno, "TCSTATUS", asmstid);
                        //Email Email = new Email(_db);
                        //// m = Email.sendmail(data.MI_Id, emailid, "TCSTATUS", asmstid);
                        //if (s == "success" && m == "success")
                        //{
                        //    data.returnval = true;
                        //}
                        //else
                        //{
                        //    data.returnval = false;
                        //}
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<TransferCertificate_DTO> certificateRejected(TransferCertificate_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASCA_Id > 0)
                {
                    var update = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id).SingleOrDefault();

                    update.ASCA_Status = "Rejected";
                    update.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(update);

                    Adm_Students_Certificate_Approve_DMO obj2 = new Adm_Students_Certificate_Approve_DMO();

                    obj2.ASCA_Id = update.ASCA_Id;
                    obj2.MI_Id = data.MI_Id;
                    obj2.IVRMALU_Id = data.UserId;
                    obj2.ASCAP_Status = "Rejected";
                    obj2.ASCAP_ApproveReason = data.ASCAP_ApproveReason;
                    obj2.ASCAP_ApproveDate = DateTime.Now;
                    obj2.ASCAP_ActiveFlg = true;
                    obj2.CreatedDate = DateTime.Now;
                    obj2.UpdatedDate = DateTime.Now;

                    _PortalContext.Add(obj2);
                    _PortalContext.Update(update);
                    int rowAffected = _PortalContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        var asmstid = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(a => a.ASCA_Id == data.ASCA_Id).AMST_Id;
                        var mobilemail = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == asmstid && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S").Distinct().ToList();


                        long mobileno = 0;
                        var emailid = "";
                        if (mobilemail.Count > 0)
                        {
                            mobileno = mobilemail.FirstOrDefault().AMST_MobileNo;
                            SMS sms = new SMS(_db);
                            s = await sms.sendCertificateSms(data.MI_Id, mobileno, "TCSTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (mobilemail.Count > 0)
                        {
                            emailid = mobilemail.FirstOrDefault().AMST_emailId;
                            Email Email = new Email(_db);
                            m = Email.sendCertificatemail(data.MI_Id, emailid, "TCSTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TransferCertificate_DTO CheckApproved_ststus(TransferCertificate_DTO dto)
        {
            try
            {
                //dto.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                dto.HRME_Id = 253;

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tc_Approval_status_check";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
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
                        dto.totalcount = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.ct_approval = (from a in _PortalContext.Adm_TC_CT_Approval_DMO_con
                                   from b in _PortalContext.HR_Master_Employee_DMO
                                   where a.AMST_Id == dto.AMST_Id && b.HRME_Id == dto.HRME_Id
                                   select new TransferCertificate_DTO
                                   {

                                       employeename = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                       ATCCTAPP_ApprovedDate = a.ATCCTAPP_ApprovedDate,
                                       ATCCTAPP_Remarks = a.ATCCTAPP_Remarks
                                   }).ToArray();

                dto.library_approval = (from a in _PortalContext.Adm_TC_Library_Approval_DMO_con
                                        from b in _PortalContext.HR_Master_Employee_DMO
                                        where a.AMST_Id == dto.AMST_Id && b.HRME_Id == dto.HRME_Id
                                        select new TransferCertificate_DTO
                                        {

                                            employeename = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                            ATCLIBAPP_ApprovedDate = a.ATCLIBAPP_ApprovedDate,
                                            ATCLIBAPP_Remarks = a.ATCLIBAPP_Remarks
                                        }).ToArray();

                dto.fee_approval = (from a in _PortalContext.Adm_TC_Fee_Approval_DMO_con
                                    from b in _PortalContext.HR_Master_Employee_DMO
                                    where a.AMST_Id == dto.AMST_Id && b.HRME_Id == dto.HRME_Id
                                    select new TransferCertificate_DTO
                                    {

                                        employeename = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                        ATCFAPP_ApprovedDate = a.ATCFAPP_ApprovedDate,
                                        ATCFAPP_Remarks = a.ATCFAPP_Remarks
                                    }).ToArray();

                dto.pda_approval = (from a in _PortalContext.Adm_TC_PDA_Approval_DMO_con
                                    from b in _PortalContext.HR_Master_Employee_DMO
                                    where a.AMST_Id == dto.AMST_Id && b.HRME_Id == dto.HRME_Id
                                    select new TransferCertificate_DTO
                                    {

                                        employeename = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                        ATCPDAAPP_ApprovedDate = a.ATCPDAAPP_ApprovedDate,
                                        ATCPDAAPP_Remarks = a.ATCPDAAPP_Remarks
                                    }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        //==========================================
        public TransferCertificate_DTO savedetails_certificate(TransferCertificate_DTO dto)
        {
            try
            {

                if (dto.ACERTAPP_Id > 0)
                {
                    var reslt = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.ACERTAPP_CertificateCode == dto.AMCT_Certificate_code && a.ACERTAPP_CertificateName == dto.ACERTAPP_CertificateName && a.MI_Id == dto.MI_Id).ToList();

                    var reslt1 = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.ACERTAPP_Id == dto.ACERTAPP_Id && a.MI_Id == dto.MI_Id).ToList();

                    if (reslt == reslt1 || reslt.Count <= 0)
                    {


                        var result = _PortalContext.Adm_Certificates_Apply_DMO_con.Single(a => a.ACERTAPP_Id == dto.ACERTAPP_Id);
                        result.ACERTAPP_CertificateName = dto.AMCT_Certificate_Name;
                        result.ACERTAPP_CertificateCode = dto.AMCT_Certificate_code;
                        result.ACERTAPP_ApprovaReqlFlg = dto.ACERTAPP_ApprovaReqlFlg;
                        result.ACERTAPP_OnlineDownloadFlg = dto.ACERTAPP_OnlineDownloadFlg;
                        result.UpdatedDate = DateTime.Now;
                        result.ACERTAPP_UpdatedBy = dto.UserId;
                        _PortalContext.Update(result);
                        var pt = _PortalContext.SaveChanges();
                        if (pt > 0)
                        {
                            dto.returnvalues = "Update";
                        }
                        else
                        {
                            dto.returnvalues = "Error";
                        }
                    }

                    else
                    {
                        dto.returnvalues = "Duplicate";
                    }
                }


                else
                {
                    var reslt = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.ACERTAPP_CertificateCode == dto.AMCT_Certificate_code && a.ACERTAPP_CertificateName == dto.AMCT_Certificate_Name && a.MI_Id == dto.MI_Id).ToList();

                    if (reslt.Count == 0)
                    {
                        Adm_Certificates_Apply_DMO amc = new Adm_Certificates_Apply_DMO();
                        amc.ACERTAPP_CertificateName = dto.AMCT_Certificate_Name;
                        amc.ACERTAPP_CertificateCode = dto.AMCT_Certificate_code;
                        amc.ACERTAPP_ApprovaReqlFlg = dto.ACERTAPP_ApprovaReqlFlg;
                        amc.ACERTAPP_OnlineDownloadFlg = dto.ACERTAPP_OnlineDownloadFlg;
                        amc.MI_Id = dto.MI_Id;
                        amc.ACERTAPP_ActiveFlg = true;
                        amc.CreatedDate = DateTime.Now;
                        amc.ACERTAPP_CreatedBy = dto.UserId;
                        _PortalContext.Add(amc);
                        var pt = _PortalContext.SaveChanges();
                        if (pt > 0)
                        {
                            dto.returnvalues = "Add";
                        }
                        else
                        {
                            dto.returnvalues = "Error";
                        }
                    }
                    else
                    {
                        dto.returnvalues = "Duplicate";
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public TransferCertificate_DTO edit_certificate(TransferCertificate_DTO dto)
        {
            try
            {

                var certificate = _PortalContext.Adm_Certificates_Apply_DMO_con.Where(a => a.ACERTAPP_Id == dto.ACERTAPP_Id).ToList();
                dto.get_details = certificate.ToArray();
                dto.get_certificate_dd = _PortalContext.Adm_Master_Certificate_DMO_con.Where(a => a.AMCT_Certificate_Name == certificate[0].ACERTAPP_CertificateName && a.AMCT_Certificate_code == certificate[0].ACERTAPP_CertificateCode).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public TransferCertificate_DTO deactive_certificate(TransferCertificate_DTO dto)
        {
            try
            {

                if (dto.ACERTAPP_ActiveFlg == true)
                {
                    var result = _PortalContext.Adm_Certificates_Apply_DMO_con.Single(a => a.ACERTAPP_Id == dto.ACERTAPP_Id);
                    result.ACERTAPP_ActiveFlg = false;
                    _PortalContext.Update(result);
                    _PortalContext.SaveChanges();

                    dto.returnvalues = "true";
                }
                else
                {
                    var result = _PortalContext.Adm_Certificates_Apply_DMO_con.Single(a => a.ACERTAPP_Id == dto.ACERTAPP_Id);
                    result.ACERTAPP_ActiveFlg = true;
                    _PortalContext.Update(result);
                    _PortalContext.SaveChanges();
                    dto.returnvalues = "true";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


    }
}
