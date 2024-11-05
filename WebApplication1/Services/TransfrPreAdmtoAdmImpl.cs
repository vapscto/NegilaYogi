using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;
using CommonLibrary;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.Transport;

namespace WebApplication1.Services
{
    public class TransfrPreAdmtoAdmImpl : Interfaces.TransfrPreAdmtoAdmInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
             new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public TransfrPreAdmtoAdmContext _db;
        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _feecontext;

        public TransfrPreAdmtoAdmImpl(TransfrPreAdmtoAdmContext trantoadmi, DomainModelMsSqlServerContext context, FeeGroupContext feecontext)
        {
            _db = trantoadmi;
            _context = context;
            _feecontext = feecontext;
        }

        public async Task<Adm_M_StudentDTO> expoadmi(Adm_M_StudentDTO data)
        // public Adm_M_StudentDTO expoadmi(Adm_M_StudentDTO data)
        {
            int j = 0;
            data.returnMsg = "";
            try
            {
                if (data.studentdetails.Count() > 0)
                {
                    data.studentdetails = data.studentdetails.OrderBy(t => t.Name).ToList();
                }
                while (j < data.studentdetails.Count())
                {

                    if (data.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        if (data.configurationsettings.ISPAC_AdmissionTransfer == 1)
                        {

                            var studDet = _db.StudentApplication.Where(t => t.MI_Id == data.MI_Id && t.pasr_id == data.studentdetails[j].AMST_Id).ToList();


                            var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", data.studentdetails[j].AMST_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id);


                            if (confirmstatusadmission > 0)
                            {
                                var confirmstatus = 0;

                                var getstudentamstid = _context.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                if (getstudentamstid.Count() > 0)
                                {
                                    var Siblingexists = _context.PA_Student_Sibblings.Where(d => d.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                    if (Siblingexists.Count() > 0)
                                    {
                                        var studentconcession = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                        if (studentconcession.Count() > 0 && studentconcession.FirstOrDefault().FMCC_ID != null && studentconcession.FirstOrDefault().FMCC_ID != 0)
                                        {
                                            var concessiontype = _context.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == studentconcession.FirstOrDefault().FMCC_ID).FMCC_ConcessionFlag;
                                            if (concessiontype == "S")
                                            {
                                                var SiblingAMSTexists = _context.PA_Student_Sibblings_Details.Where(d => d.PASS_Id == Siblingexists.FirstOrDefault().PASS_Id).ToList();
                                                if (SiblingAMSTexists.Count() > 0)
                                                {
                                                    for (int i = 0; i < SiblingAMSTexists.Count; i++)
                                                    {
                                                        var checkamstexists = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == SiblingAMSTexists[i].PASSD_SibblingAMST_Id && d.AMSTS_TCIssuesFlag == "0").ToList();

                                                        if (checkamstexists.Count() == 0)
                                                        {
                                                            var STudentdetails = _context.Adm_M_Student.Where(d => d.AMST_Id == SiblingAMSTexists[i].PASSD_SibblingAMST_Id && d.AMST_SOL == "S").ToList();
                                                            if (STudentdetails.Count() > 0)
                                                            {
                                                                StudentSiblingDMO siblingstudent = new StudentSiblingDMO();
                                                                siblingstudent.AMST_Id = STudentdetails.FirstOrDefault().AMST_Id;
                                                                siblingstudent.AMSTS_SiblingsName = ((STudentdetails.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                siblingstudent.AMSTS_SiblingsOrder = 1;
                                                                siblingstudent.AMSTS_SiblingsRelation = "";
                                                                siblingstudent.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                siblingstudent.AMSTS_Siblings_AMST_ID = STudentdetails.FirstOrDefault().AMST_Id;
                                                                siblingstudent.AMCL_Id = Convert.ToInt64(STudentdetails.FirstOrDefault().ASMCL_Id);
                                                                siblingstudent.AMSTS_TCIssuesFlag = "0";
                                                                siblingstudent.CreatedDate = DateTime.Now;
                                                                siblingstudent.UpdatedDate = DateTime.Now;
                                                                _db.Add(siblingstudent);
                                                                confirmstatus = _db.SaveChanges();
                                                                if (confirmstatus > 0)
                                                                {
                                                                    data.returnval = true;
                                                                }
                                                                else
                                                                {
                                                                    data.returnval = false;
                                                                }
                                                                var STudentdetails1 = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                                if (STudentdetails1.Count() > 0)
                                                                {
                                                                    StudentSiblingDMO siblingstudent1 = new StudentSiblingDMO();
                                                                    siblingstudent1.AMST_Id = STudentdetails.FirstOrDefault().AMST_Id;
                                                                    siblingstudent1.AMSTS_SiblingsName = ((STudentdetails1.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails1.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails1.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails1.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails1.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails1.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                    siblingstudent1.AMSTS_SiblingsOrder = 2;
                                                                    siblingstudent1.AMSTS_SiblingsRelation = "";
                                                                    siblingstudent1.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                    siblingstudent1.AMSTS_Siblings_AMST_ID = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                    siblingstudent1.AMCL_Id = Convert.ToInt64(STudentdetails1.FirstOrDefault().ASMCL_Id);
                                                                    siblingstudent1.AMSTS_TCIssuesFlag = "0";
                                                                    siblingstudent1.CreatedDate = DateTime.Now;
                                                                    siblingstudent1.UpdatedDate = DateTime.Now;
                                                                    _db.Add(siblingstudent1);
                                                                    confirmstatus = _db.SaveChanges();
                                                                    if (confirmstatus > 0)
                                                                    {
                                                                        data.returnval = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.returnval = false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (checkamstexists.Count() > 0)
                                                        {
                                                            var checknewstudentexists = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == getstudentamstid.FirstOrDefault().AMST_Id && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();

                                                            if (checknewstudentexists.Count() == 0)
                                                            {
                                                                var getamstorder = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == checkamstexists.FirstOrDefault().AMSTS_Siblings_AMST_ID && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();
                                                                if (getamstorder.Count() > 0)
                                                                {
                                                                    var STudentdetails = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                                    if (STudentdetails.Count() > 0)
                                                                    {
                                                                        var orderupdate = _context.StudentSiblingDMO.Where(d => d.AMST_Id == getamstorder.FirstOrDefault().AMST_Id && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();

                                                                        StudentSiblingDMO siblingstudent = new StudentSiblingDMO();
                                                                        siblingstudent.AMST_Id = orderupdate.FirstOrDefault().AMST_Id;
                                                                        siblingstudent.AMSTS_SiblingsName = ((STudentdetails.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                        siblingstudent.AMSTS_SiblingsOrder = orderupdate.FirstOrDefault().AMSTS_SiblingsOrder + 1;
                                                                        siblingstudent.AMSTS_SiblingsRelation = "";
                                                                        siblingstudent.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                        siblingstudent.AMSTS_Siblings_AMST_ID = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                        siblingstudent.AMCL_Id = Convert.ToInt64(STudentdetails.FirstOrDefault().ASMCL_Id);
                                                                        siblingstudent.AMSTS_TCIssuesFlag = "0";
                                                                        siblingstudent.CreatedDate = DateTime.Now;
                                                                        siblingstudent.UpdatedDate = DateTime.Now;
                                                                        _db.Add(siblingstudent);
                                                                        confirmstatus = _db.SaveChanges();
                                                                        if (confirmstatus > 0)
                                                                        {
                                                                            data.returnval = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            data.returnval = false;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (concessiontype == "E")
                                            {
                                                var SiblingAMSTexists = _context.PAStudentEmployee.Where(d => d.PASS_Id == Siblingexists.FirstOrDefault().PASS_Id).ToList();
                                                if (SiblingAMSTexists.Count() > 0)
                                                {
                                                    var Checkemployeeexists = _context.Adm_M_Employee_StudentDMO.Where(d => d.HRME_Id == SiblingAMSTexists.FirstOrDefault().HRME_Id && d.AMSTE_Left == 0).ToList();
                                                    if (Checkemployeeexists.Count() == 0)
                                                    {
                                                        Adm_M_Employee_StudentDMO siblingemployee = new Adm_M_Employee_StudentDMO();
                                                        siblingemployee.HRME_Id = Convert.ToInt64(SiblingAMSTexists.FirstOrDefault().HRME_Id);
                                                        siblingemployee.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                        siblingemployee.ASMCL_Id = studentconcession.FirstOrDefault().ASMCL_Id;
                                                        siblingemployee.AMSTE_Left = 0;
                                                        siblingemployee.AMSTE_Concessionpercentage = 0;
                                                        siblingemployee.CreatedDate = DateTime.Now;
                                                        siblingemployee.UpdatedDate = DateTime.Now;
                                                        siblingemployee.AMSTE_SiblingsOrder = 1;
                                                        _context.Add(siblingemployee);
                                                        confirmstatus = _context.SaveChanges();
                                                        if (confirmstatus > 0)
                                                        {
                                                            data.returnval = true;
                                                        }
                                                        else
                                                        {
                                                            data.returnval = false;
                                                        }
                                                    }
                                                    else if (Checkemployeeexists.Count() > 0)
                                                    {
                                                        var Checkemployestudentxists = _context.Adm_M_Employee_StudentDMO.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                        if (Checkemployestudentxists.Count() == 0)
                                                        {
                                                            var employeeduplicate = _context.Adm_M_Employee_StudentDMO.Where(d => d.HRME_Id == SiblingAMSTexists.FirstOrDefault().HRME_Id && d.AMSTE_Left == 0).OrderByDescending(t => t.AMSTE_SiblingsOrder).ToList();
                                                            if (employeeduplicate.Count() > 0)
                                                            {
                                                                Adm_M_Employee_StudentDMO siblingemployee = new Adm_M_Employee_StudentDMO();
                                                                siblingemployee.HRME_Id = Convert.ToInt64(SiblingAMSTexists.FirstOrDefault().HRME_Id);
                                                                siblingemployee.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                siblingemployee.ASMCL_Id = studentconcession.FirstOrDefault().ASMCL_Id;
                                                                siblingemployee.AMSTE_Left = 0;
                                                                siblingemployee.AMSTE_Concessionpercentage = 0;
                                                                siblingemployee.CreatedDate = DateTime.Now;
                                                                siblingemployee.UpdatedDate = DateTime.Now;
                                                                siblingemployee.AMSTE_SiblingsOrder = employeeduplicate.FirstOrDefault().AMSTE_SiblingsOrder + 1;
                                                                _context.Add(siblingemployee);
                                                                confirmstatus = _context.SaveChanges();
                                                                if (confirmstatus > 0)
                                                                {
                                                                    data.returnval = true;
                                                                }
                                                                else
                                                                {
                                                                    data.returnval = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    var Studenttransport = _context.PA_Student_Transport_ApplicationDMO.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                    if (Studenttransport.Count() > 0)
                                    {
                                        var StudentDetails = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                        if (StudentDetails.Count() > 0)
                                        {
                                            var feegroupid = _context.TR_Location_FeeGroup_MappingDMO.Where(g => g.MI_Id == StudentDetails.FirstOrDefault().MI_Id && g.ASMAY_Id == StudentDetails.FirstOrDefault().ASMAY_Id && g.TRML_Id == Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id && g.TRLFM_ActiveFlag == true).ToList();
                                            if(feegroupid.Count()>0)
                                            {
                                                TR_student_LocMappingDMO Transportdata = new TR_student_LocMappingDMO();
                                                Transportdata.ASMAY_Id = StudentDetails.FirstOrDefault().ASMAY_Id;
                                                Transportdata.MI_Id = StudentDetails.FirstOrDefault().MI_Id;
                                                Transportdata.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                Transportdata.TRMR_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                                Transportdata.TRML_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                                Transportdata.FMG_Id = feegroupid.FirstOrDefault().FMG_Id;
                                                Transportdata.TRSLM_ActiveFlag = true;
                                                Transportdata.CreatedDate = DateTime.Now;
                                                Transportdata.UpdatedDate = DateTime.Now;
                                                _context.Add(Transportdata);
                                                confirmstatus = _context.SaveChanges();
                                                if (confirmstatus > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }


                                                TR_Student_RouteDMO object123 = new TR_Student_RouteDMO();
                                                object123.MI_Id = StudentDetails.FirstOrDefault().MI_Id;
                                                object123.ASMAY_Id = StudentDetails.FirstOrDefault().ASMAY_Id;
                                                object123.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                object123.TRSR_Date = DateTime.Now.Date;
                                                object123.TRMR_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                                object123.TRSR_PickUpLocation = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                                object123.TRSR_PickUpMobileNo = Convert.ToInt64(studDet.FirstOrDefault().PASR_FatherMobleNo);
                                                object123.TRMR_Drop_Route = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                                object123.TRSR_DropLocation = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                                object123.TRSR_DropMobileNo = Convert.ToInt64(studDet.FirstOrDefault().PASR_FatherMobleNo);
                                                object123.TRSR_ApplicationNo = 0;
                                                object123.TRSR_PickupSession = 0;
                                                object123.TRSR_DropSession = 0;
                                                object123.TRSR_ActiveFlg = true;
                                                object123.CreatedDate = DateTime.Now;
                                                object123.UpdatedDate = DateTime.Now;
                                                object123.ASTA_Id = 0;
                                                _context.Add(object123);
                                                _context.SaveChanges();

                                                TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                                                oobj.TRSR_Id = object123.TRSR_Id;
                                                oobj.FMG_Id = feegroupid.FirstOrDefault().FMG_Id;
                                                oobj.TRSRFG_ActiveFlg = true;
                                                _context.Add(oobj);
                                                _context.SaveChanges();
                                            }
                                          
                                        }
                                    }

                                    if (data.returnval == true)
                                    {
                                        var Updatedetails = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id && d.AMST_SOL == "S").ToList();
                                        if (Updatedetails.Count() > 0)
                                        {
                                            var studentconcession = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();

                                            if (studentconcession.FirstOrDefault().FMCC_ID != null && studentconcession.FirstOrDefault().FMCC_ID > 0)
                                            {
                                                var concessiontype = _context.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == studentconcession.FirstOrDefault().FMCC_ID).FMCC_ConcessionFlag;

                                                if (concessiontype != "E")
                                                {
                                                    var regstatus = _db.Database.ExecuteSqlCommand("AUTOFEEMAPPING_AFTER_TRANSFER  @p0,@p1,@p2,@p3,@p4,@p5", getstudentamstid.FirstOrDefault().AMST_Id, data.studentdetails[j].AMST_Id, Updatedetails.FirstOrDefault().MI_Id, Updatedetails.FirstOrDefault().ASMAY_Id, 0, concessiontype);
                                                }
                                                else if (concessiontype == "E")
                                                {
                                                    var Sibemployee = _context.PAStudentEmployee.Where(d => d.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                                    if (Sibemployee.Count() > 0)
                                                    {
                                                        var regstatus = _db.Database.ExecuteSqlCommand("AUTOFEEMAPPING_AFTER_TRANSFER  @p0,@p1,@p2,@p3,@p4,@p5", getstudentamstid.FirstOrDefault().AMST_Id, data.studentdetails[j].AMST_Id, Updatedetails.FirstOrDefault().MI_Id, Updatedetails.FirstOrDefault().ASMAY_Id, Sibemployee.FirstOrDefault().HRME_Id, concessiontype);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                List<Adm_M_Student> getcurr = new List<Adm_M_Student>();
                                long mobileno = 0;
                                string MailId = "";
                                var getdetails = (from a in _db.StudentApplication
                                                  where (a.pasr_id == data.studentdetails[j].AMST_Id)
                                                  select new Adm_M_StudentDTO
                                                  {
                                                      AMST_MobileNo = a.PASR_MobileNo,
                                                      AMST_emailId = a.PASR_emailId
                                                  }).ToList();

                                mobileno = getdetails.FirstOrDefault().AMST_MobileNo;
                                MailId = getdetails.FirstOrDefault().AMST_emailId;

                                var AdmstudDet = _context.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();

                                SMS sms = new SMS(_context);
                                string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                Email Email = new Email(_context);
                                string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));
                            }
                            j++;
                        }
                        else if (data.configurationsettings.ISPAC_AdmissionTransfer == 0)
                        {
                            data.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == data.studentdetails[j].AMST_Id).Count();
                            if (data.payementcheck != 0)
                            {
                                var studDet = _db.StudentApplication.Where(t => t.MI_Id == data.MI_Id && t.pasr_id == data.studentdetails[j].AMST_Id).ToList();

                                var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", data.studentdetails[j].AMST_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id);
                                if (confirmstatusadmission > 0)
                                {
                                    var confirmstatus = 0;
                                    var getstudentamstid = _context.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                    if (getstudentamstid.Count() > 0)
                                    {
                                        var Siblingexists = _context.PA_Student_Sibblings.Where(d => d.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                        if (Siblingexists.Count() > 0)
                                        {
                                            var studentconcession = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                            if (studentconcession.Count() > 0 && studentconcession.FirstOrDefault().FMCC_ID != null && studentconcession.FirstOrDefault().FMCC_ID != 0)
                                            {
                                                var concessiontype = _context.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == studentconcession.FirstOrDefault().FMCC_ID).FMCC_ConcessionFlag;
                                                if (concessiontype != "E")
                                                {
                                                    var SiblingAMSTexists = _context.PA_Student_Sibblings_Details.Where(d => d.PASS_Id == Siblingexists.FirstOrDefault().PASS_Id).ToList();
                                                    if (SiblingAMSTexists.Count() > 0)
                                                    {
                                                        for (int i = 0; i < SiblingAMSTexists.Count; i++)
                                                        {
                                                            var checkamstexists = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == SiblingAMSTexists[i].PASSD_SibblingAMST_Id && d.AMSTS_TCIssuesFlag == "0").ToList();

                                                            if (checkamstexists.Count() == 0)
                                                            {
                                                                var STudentdetails = _context.Adm_M_Student.Where(d => d.AMST_Id == SiblingAMSTexists[i].PASSD_SibblingAMST_Id && d.AMST_SOL == "S").ToList();
                                                                if (STudentdetails.Count() > 0)
                                                                {
                                                                    StudentSiblingDMO siblingstudent = new StudentSiblingDMO();
                                                                    siblingstudent.AMST_Id = STudentdetails.FirstOrDefault().AMST_Id;
                                                                    siblingstudent.AMSTS_SiblingsName = ((STudentdetails.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                    siblingstudent.AMSTS_SiblingsOrder = 1;
                                                                    siblingstudent.AMSTS_SiblingsRelation = "";
                                                                    siblingstudent.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                    siblingstudent.AMSTS_Siblings_AMST_ID = STudentdetails.FirstOrDefault().AMST_Id;
                                                                    siblingstudent.AMCL_Id = Convert.ToInt64(STudentdetails.FirstOrDefault().ASMCL_Id);
                                                                    siblingstudent.AMSTS_TCIssuesFlag = "0";
                                                                    siblingstudent.CreatedDate = DateTime.Now;
                                                                    siblingstudent.UpdatedDate = DateTime.Now;
                                                                    _db.Add(siblingstudent);
                                                                    confirmstatus = _db.SaveChanges();
                                                                    if (confirmstatus > 0)
                                                                    {
                                                                        data.returnval = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.returnval = false;
                                                                    }
                                                                    var STudentdetails1 = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                                    if (STudentdetails1.Count() > 0)
                                                                    {
                                                                        StudentSiblingDMO siblingstudent1 = new StudentSiblingDMO();
                                                                        siblingstudent1.AMST_Id = STudentdetails.FirstOrDefault().AMST_Id;
                                                                        siblingstudent1.AMSTS_SiblingsName = ((STudentdetails1.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails1.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails1.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails1.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails1.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails1.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                        siblingstudent1.AMSTS_SiblingsOrder = 2;
                                                                        siblingstudent1.AMSTS_SiblingsRelation = "";
                                                                        siblingstudent1.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                        siblingstudent1.AMSTS_Siblings_AMST_ID = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                        siblingstudent1.AMCL_Id = Convert.ToInt64(STudentdetails1.FirstOrDefault().ASMCL_Id);
                                                                        siblingstudent1.AMSTS_TCIssuesFlag = "0";
                                                                        siblingstudent1.CreatedDate = DateTime.Now;
                                                                        siblingstudent1.UpdatedDate = DateTime.Now;
                                                                        _db.Add(siblingstudent1);
                                                                        confirmstatus = _db.SaveChanges();
                                                                        if (confirmstatus > 0)
                                                                        {
                                                                            data.returnval = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            data.returnval = false;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else if (checkamstexists.Count() > 0)
                                                            {
                                                                var checknewstudentexists = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == getstudentamstid.FirstOrDefault().AMST_Id && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();

                                                                if (checknewstudentexists.Count() == 0)
                                                                {
                                                                    var getamstorder = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == checkamstexists.FirstOrDefault().AMSTS_Siblings_AMST_ID && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();
                                                                    if (getamstorder.Count() > 0)
                                                                    {
                                                                        var STudentdetails = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                                        if (STudentdetails.Count() > 0)
                                                                        {
                                                                            var orderupdate = _context.StudentSiblingDMO.Where(d => d.AMST_Id == getamstorder.FirstOrDefault().AMST_Id && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();

                                                                            StudentSiblingDMO siblingstudent = new StudentSiblingDMO();
                                                                            siblingstudent.AMST_Id = orderupdate.FirstOrDefault().AMST_Id;
                                                                            siblingstudent.AMSTS_SiblingsName = ((STudentdetails.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                            siblingstudent.AMSTS_SiblingsOrder = orderupdate.FirstOrDefault().AMSTS_SiblingsOrder + 1;
                                                                            siblingstudent.AMSTS_SiblingsRelation = "";
                                                                            siblingstudent.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                            siblingstudent.AMSTS_Siblings_AMST_ID = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                            siblingstudent.AMCL_Id = Convert.ToInt64(STudentdetails.FirstOrDefault().ASMCL_Id);
                                                                            siblingstudent.AMSTS_TCIssuesFlag = "0";
                                                                            siblingstudent.CreatedDate = DateTime.Now;
                                                                            siblingstudent.UpdatedDate = DateTime.Now;
                                                                            _db.Add(siblingstudent);
                                                                            confirmstatus = _db.SaveChanges();
                                                                            if (confirmstatus > 0)
                                                                            {
                                                                                data.returnval = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                data.returnval = false;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var SiblingAMSTexists = _context.PAStudentEmployee.Where(d => d.PASS_Id == Siblingexists.FirstOrDefault().PASS_Id).ToList();
                                                    if (SiblingAMSTexists.Count() > 0)
                                                    {
                                                        var Checkemployeeexists = _context.Adm_M_Employee_StudentDMO.Where(d => d.HRME_Id == SiblingAMSTexists.FirstOrDefault().HRME_Id && d.AMSTE_Left == 0).ToList();
                                                        if (Checkemployeeexists.Count() == 0)
                                                        {
                                                            Adm_M_Employee_StudentDMO siblingemployee = new Adm_M_Employee_StudentDMO();
                                                            siblingemployee.HRME_Id = Convert.ToInt64(SiblingAMSTexists.FirstOrDefault().HRME_Id);
                                                            siblingemployee.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                            siblingemployee.ASMCL_Id = studentconcession.FirstOrDefault().ASMCL_Id;
                                                            siblingemployee.AMSTE_Left = 0;
                                                            siblingemployee.AMSTE_Concessionpercentage = 0;
                                                            siblingemployee.CreatedDate = DateTime.Now;
                                                            siblingemployee.UpdatedDate = DateTime.Now;
                                                            siblingemployee.AMSTE_SiblingsOrder = 1;
                                                            _context.Add(siblingemployee);
                                                            confirmstatus = _context.SaveChanges();
                                                            if (confirmstatus > 0)
                                                            {
                                                                data.returnval = true;
                                                            }
                                                            else
                                                            {
                                                                data.returnval = false;
                                                            }
                                                        }
                                                        else if (Checkemployeeexists.Count() > 0)
                                                        {
                                                            var Checkemployestudentxists = _context.Adm_M_Employee_StudentDMO.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                            if (Checkemployestudentxists.Count() == 0)
                                                            {
                                                                var employeeduplicate = _context.Adm_M_Employee_StudentDMO.Where(d => d.HRME_Id == SiblingAMSTexists.FirstOrDefault().HRME_Id && d.AMSTE_Left == 0).OrderByDescending(t => t.AMSTE_SiblingsOrder).ToList();
                                                                if (employeeduplicate.Count() > 0)
                                                                {
                                                                    Adm_M_Employee_StudentDMO siblingemployee = new Adm_M_Employee_StudentDMO();
                                                                    siblingemployee.HRME_Id = Convert.ToInt64(SiblingAMSTexists.FirstOrDefault().HRME_Id);
                                                                    siblingemployee.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                    siblingemployee.ASMCL_Id = studentconcession.FirstOrDefault().ASMCL_Id;
                                                                    siblingemployee.AMSTE_Left = 0;
                                                                    siblingemployee.AMSTE_Concessionpercentage = 0;
                                                                    siblingemployee.CreatedDate = DateTime.Now;
                                                                    siblingemployee.UpdatedDate = DateTime.Now;
                                                                    siblingemployee.AMSTE_SiblingsOrder = employeeduplicate.FirstOrDefault().AMSTE_SiblingsOrder + 1;
                                                                    _context.Add(siblingemployee);
                                                                    confirmstatus = _context.SaveChanges();
                                                                    if (confirmstatus > 0)
                                                                    {
                                                                        data.returnval = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.returnval = false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        var Studenttransport = _context.PA_Student_Transport_ApplicationDMO.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                        if (Studenttransport.Count() > 0)
                                        {
                                            var StudentDetails = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                            if (StudentDetails.Count() > 0)
                                            {
                                                var feegroupid = _context.TR_Location_FeeGroup_MappingDMO.Where(g => g.MI_Id == StudentDetails.FirstOrDefault().MI_Id && g.ASMAY_Id == StudentDetails.FirstOrDefault().ASMAY_Id && g.TRML_Id == Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id && g.TRLFM_ActiveFlag == true).ToList();

                                                TR_student_LocMappingDMO Transportdata = new TR_student_LocMappingDMO();
                                                Transportdata.ASMAY_Id = StudentDetails.FirstOrDefault().ASMAY_Id;
                                                Transportdata.MI_Id = StudentDetails.FirstOrDefault().MI_Id;
                                                Transportdata.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                Transportdata.TRMR_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                                Transportdata.TRML_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                                Transportdata.FMG_Id = feegroupid.FirstOrDefault().FMG_Id;
                                                Transportdata.TRSLM_ActiveFlag = true;
                                                Transportdata.CreatedDate = DateTime.Now;
                                                Transportdata.UpdatedDate = DateTime.Now;
                                                _context.Add(Transportdata);
                                                confirmstatus = _context.SaveChanges();
                                                if (confirmstatus > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }


                                                TR_Student_RouteDMO object123 = new TR_Student_RouteDMO();
                                                object123.MI_Id = StudentDetails.FirstOrDefault().MI_Id;
                                                object123.ASMAY_Id = StudentDetails.FirstOrDefault().ASMAY_Id;
                                                object123.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                object123.TRSR_Date = DateTime.Now.Date;
                                                object123.TRMR_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                                object123.TRSR_PickUpLocation = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                                object123.TRSR_PickUpMobileNo = Convert.ToInt64(studDet.FirstOrDefault().PASR_FatherMobleNo);
                                                object123.TRMR_Drop_Route = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                                object123.TRSR_DropLocation = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                                object123.TRSR_DropMobileNo = Convert.ToInt64(studDet.FirstOrDefault().PASR_FatherMobleNo);
                                                object123.TRSR_ApplicationNo = 0;
                                                object123.TRSR_PickupSession = 0;
                                                object123.TRSR_DropSession = 0;
                                                object123.TRSR_ActiveFlg = true;
                                                object123.CreatedDate = DateTime.Now;
                                                object123.UpdatedDate = DateTime.Now;
                                                object123.ASTA_Id = 0;
                                                _context.Add(object123);
                                                _context.SaveChanges();

                                                TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                                                oobj.TRSR_Id = object123.TRSR_Id;
                                                oobj.FMG_Id = feegroupid.FirstOrDefault().FMG_Id;
                                                oobj.TRSRFG_ActiveFlg = true;
                                                _context.Add(oobj);
                                                _context.SaveChanges();
                                            }
                                        }
                                        if (data.returnval == true)
                                        {
                                            var Updatedetails = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id && d.AMST_SOL == "S").ToList();
                                            if (Updatedetails.Count() > 0)
                                            {
                                                var studentconcession = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                                if (studentconcession.FirstOrDefault().FMCC_ID != null && studentconcession.FirstOrDefault().FMCC_ID > 0)
                                                {
                                                    var concessiontype = _context.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == studentconcession.FirstOrDefault().FMCC_ID).FMCC_ConcessionFlag;

                                                    if (concessiontype != "E")
                                                    {
                                                        var regstatus = _db.Database.ExecuteSqlCommand("AUTOFEEMAPPING_AFTER_TRANSFER  @p0,@p1,@p2,@p3,@p4,@p5", getstudentamstid.FirstOrDefault().AMST_Id, data.studentdetails[j].AMST_Id, Updatedetails.FirstOrDefault().MI_Id, Updatedetails.FirstOrDefault().ASMAY_Id, 0, concessiontype);
                                                    }
                                                    else if (concessiontype == "E")
                                                    {
                                                        var Sibemployee = _context.PAStudentEmployee.Where(d => d.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                                        if (Sibemployee.Count() > 0)
                                                        {
                                                            var regstatus = _db.Database.ExecuteSqlCommand("AUTOFEEMAPPING_AFTER_TRANSFER  @p0,@p1,@p2,@p3,@p4,@p5", getstudentamstid.FirstOrDefault().AMST_Id, data.studentdetails[j].AMST_Id, Updatedetails.FirstOrDefault().MI_Id, Updatedetails.FirstOrDefault().ASMAY_Id, Sibemployee.FirstOrDefault().HRME_Id, concessiontype);
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }

                                    List<Adm_M_Student> getcurr = new List<Adm_M_Student>();
                                    long mobileno = 0;
                                    string MailId = "";

                                    var getdetails = (from a in _db.StudentApplication
                                                      where (a.pasr_id == data.studentdetails[j].AMST_Id)
                                                      select new Adm_M_StudentDTO
                                                      {
                                                          AMST_MobileNo = a.PASR_MobileNo,
                                                          AMST_emailId = a.PASR_emailId
                                                      }).ToList();

                                    mobileno = getdetails.FirstOrDefault().AMST_MobileNo;
                                    MailId = getdetails.FirstOrDefault().AMST_emailId;

                                    var AdmstudDet = _context.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();

                                    SMS sms = new SMS(_context);
                                    string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                    Email Email = new Email(_context);
                                    string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                                }
                                j++;
                            }
                            else
                            {
                                data.returnMsg = "Student Payment is not done";
                                return data;
                            }
                        }
                    }
                    else
                    {

                        var studDet = _db.StudentApplication.Where(t => t.MI_Id == data.MI_Id && t.pasr_id == data.studentdetails[j].AMST_Id).ToList();


                        var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", data.studentdetails[j].AMST_Id, Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id), data.MI_Id);
                        if (confirmstatusadmission > 0)
                        {
                            var confirmstatus = 0;
                            var getstudentamstid = _context.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                            if (getstudentamstid.Count() > 0)
                            {
                                var Siblingexists = _context.PA_Student_Sibblings.Where(d => d.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                if (Siblingexists.Count() > 0)
                                {
                                    var studentconcession = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                    if (studentconcession.Count() > 0 && studentconcession.FirstOrDefault().FMCC_ID != null && studentconcession.FirstOrDefault().FMCC_ID != 0)
                                    {
                                        var concessiontype = _context.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == studentconcession.FirstOrDefault().FMCC_ID).FMCC_ConcessionFlag;
                                        if (concessiontype != "E")
                                        {
                                            var SiblingAMSTexists = _context.PA_Student_Sibblings_Details.Where(d => d.PASS_Id == Siblingexists.FirstOrDefault().PASS_Id).ToList();
                                            if (SiblingAMSTexists.Count() > 0)
                                            {
                                                for (int i = 0; i < SiblingAMSTexists.Count; i++)
                                                {
                                                    var checkamstexists = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == SiblingAMSTexists[i].PASSD_SibblingAMST_Id && d.AMSTS_TCIssuesFlag == "0").ToList();

                                                    if (checkamstexists.Count() == 0)
                                                    {
                                                        var STudentdetails = _context.Adm_M_Student.Where(d => d.AMST_Id == SiblingAMSTexists[i].PASSD_SibblingAMST_Id && d.AMST_SOL == "S").ToList();
                                                        if (STudentdetails.Count() > 0)
                                                        {
                                                            StudentSiblingDMO siblingstudent = new StudentSiblingDMO();
                                                            siblingstudent.AMST_Id = STudentdetails.FirstOrDefault().AMST_Id;
                                                            siblingstudent.AMSTS_SiblingsName = ((STudentdetails.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                            siblingstudent.AMSTS_SiblingsOrder = 1;
                                                            siblingstudent.AMSTS_SiblingsRelation = "";
                                                            siblingstudent.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                            siblingstudent.AMSTS_Siblings_AMST_ID = STudentdetails.FirstOrDefault().AMST_Id;
                                                            siblingstudent.AMCL_Id = Convert.ToInt64(STudentdetails.FirstOrDefault().ASMCL_Id);
                                                            siblingstudent.AMSTS_TCIssuesFlag = "0";
                                                            siblingstudent.CreatedDate = DateTime.Now;
                                                            siblingstudent.UpdatedDate = DateTime.Now;
                                                            _db.Add(siblingstudent);
                                                            confirmstatus = _db.SaveChanges();
                                                            if (confirmstatus > 0)
                                                            {
                                                                data.returnval = true;
                                                            }
                                                            else
                                                            {
                                                                data.returnval = false;
                                                            }

                                                            var STudentdetails1 = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                            if (STudentdetails1.Count() > 0)
                                                            {
                                                                StudentSiblingDMO siblingstudent1 = new StudentSiblingDMO();
                                                                siblingstudent1.AMST_Id = STudentdetails.FirstOrDefault().AMST_Id;
                                                                siblingstudent1.AMSTS_SiblingsName = ((STudentdetails1.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails1.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails1.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails1.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails1.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails1.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                siblingstudent1.AMSTS_SiblingsOrder = 2;
                                                                siblingstudent1.AMSTS_SiblingsRelation = "";
                                                                siblingstudent1.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                siblingstudent1.AMSTS_Siblings_AMST_ID = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                siblingstudent1.AMCL_Id = Convert.ToInt64(STudentdetails1.FirstOrDefault().ASMCL_Id);
                                                                siblingstudent1.AMSTS_TCIssuesFlag = "0";
                                                                siblingstudent1.CreatedDate = DateTime.Now;
                                                                siblingstudent1.UpdatedDate = DateTime.Now;
                                                                _db.Add(siblingstudent1);
                                                                confirmstatus = _db.SaveChanges();
                                                                if (confirmstatus > 0)
                                                                {
                                                                    data.returnval = true;
                                                                }
                                                                else
                                                                {
                                                                    data.returnval = false;
                                                                }

                                                            }
                                                        }

                                                    }
                                                    else if (checkamstexists.Count() > 0)
                                                    {
                                                        var checknewstudentexists = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == getstudentamstid.FirstOrDefault().AMST_Id && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();

                                                        if (checknewstudentexists.Count() == 0)
                                                        {
                                                            var getamstorder = _context.StudentSiblingDMO.Where(d => d.AMSTS_Siblings_AMST_ID == checkamstexists.FirstOrDefault().AMSTS_Siblings_AMST_ID && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();
                                                            if (getamstorder.Count() > 0)
                                                            {
                                                                var STudentdetails = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                                if (STudentdetails.Count() > 0)
                                                                {
                                                                    var orderupdate = _context.StudentSiblingDMO.Where(d => d.AMST_Id == getamstorder.FirstOrDefault().AMST_Id && d.AMSTS_TCIssuesFlag == "0").OrderByDescending(t => t.AMSTS_SiblingsOrder).ToList();

                                                                    StudentSiblingDMO siblingstudent = new StudentSiblingDMO();
                                                                    siblingstudent.AMST_Id = orderupdate.FirstOrDefault().AMST_Id;
                                                                    siblingstudent.AMSTS_SiblingsName = ((STudentdetails.FirstOrDefault().AMST_FirstName == null ? "" : STudentdetails.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_MiddleName == null ? "" : STudentdetails.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (STudentdetails.FirstOrDefault().AMST_LastName == null ? "" : STudentdetails.FirstOrDefault().AMST_LastName.ToUpper())).Trim();
                                                                    siblingstudent.AMSTS_SiblingsOrder = orderupdate.FirstOrDefault().AMSTS_SiblingsOrder + 1;
                                                                    siblingstudent.AMSTS_SiblingsRelation = "";
                                                                    siblingstudent.MI_Id = STudentdetails.FirstOrDefault().MI_Id;
                                                                    siblingstudent.AMSTS_Siblings_AMST_ID = getstudentamstid.FirstOrDefault().AMST_Id;
                                                                    siblingstudent.AMCL_Id = Convert.ToInt64(STudentdetails.FirstOrDefault().ASMCL_Id);
                                                                    siblingstudent.AMSTS_TCIssuesFlag = "0";
                                                                    siblingstudent.CreatedDate = DateTime.Now;
                                                                    siblingstudent.UpdatedDate = DateTime.Now;
                                                                    _db.Add(siblingstudent);
                                                                    confirmstatus = _db.SaveChanges();
                                                                    if (confirmstatus > 0)
                                                                    {
                                                                        data.returnval = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.returnval = false;
                                                                    }
                                                                }

                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var SiblingAMSTexists = _context.PAStudentEmployee.Where(d => d.PASS_Id == Siblingexists.FirstOrDefault().PASS_Id).ToList();
                                            if (SiblingAMSTexists.Count() > 0)
                                            {
                                                var Checkemployeeexists = _context.Adm_M_Employee_StudentDMO.Where(d => d.HRME_Id == SiblingAMSTexists.FirstOrDefault().HRME_Id && d.AMSTE_Left == 0).ToList();
                                                if (Checkemployeeexists.Count() == 0)
                                                {
                                                    Adm_M_Employee_StudentDMO siblingemployee = new Adm_M_Employee_StudentDMO();
                                                    siblingemployee.HRME_Id = Convert.ToInt64(SiblingAMSTexists.FirstOrDefault().HRME_Id);
                                                    siblingemployee.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                    siblingemployee.ASMCL_Id = studentconcession.FirstOrDefault().ASMCL_Id;
                                                    siblingemployee.AMSTE_Left = 0;
                                                    siblingemployee.AMSTE_Concessionpercentage = 0;
                                                    siblingemployee.CreatedDate = DateTime.Now;
                                                    siblingemployee.UpdatedDate = DateTime.Now;
                                                    siblingemployee.AMSTE_SiblingsOrder = 1;
                                                    _context.Add(siblingemployee);
                                                    confirmstatus = _context.SaveChanges();
                                                    if (confirmstatus > 0)
                                                    {
                                                        data.returnval = true;
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                    }
                                                }
                                                else if (Checkemployeeexists.Count() > 0)
                                                {
                                                    var Checkemployestudentxists = _context.Adm_M_Employee_StudentDMO.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id).ToList();
                                                    if (Checkemployestudentxists.Count() == 0)
                                                    {
                                                        var employeeduplicate = _context.Adm_M_Employee_StudentDMO.Where(d => d.HRME_Id == SiblingAMSTexists.FirstOrDefault().HRME_Id && d.AMSTE_Left == 0).OrderByDescending(t => t.AMSTE_SiblingsOrder).ToList();
                                                        if (employeeduplicate.Count() > 0)
                                                        {
                                                            Adm_M_Employee_StudentDMO siblingemployee = new Adm_M_Employee_StudentDMO();
                                                            siblingemployee.HRME_Id = Convert.ToInt64(SiblingAMSTexists.FirstOrDefault().HRME_Id);
                                                            siblingemployee.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                                            siblingemployee.ASMCL_Id = studentconcession.FirstOrDefault().ASMCL_Id;
                                                            siblingemployee.AMSTE_Left = 0;
                                                            siblingemployee.AMSTE_Concessionpercentage = 0;
                                                            siblingemployee.CreatedDate = DateTime.Now;
                                                            siblingemployee.UpdatedDate = DateTime.Now;
                                                            siblingemployee.AMSTE_SiblingsOrder = employeeduplicate.FirstOrDefault().AMSTE_SiblingsOrder + 1;
                                                            _context.Add(siblingemployee);
                                                            confirmstatus = _context.SaveChanges();
                                                            if (confirmstatus > 0)
                                                            {
                                                                data.returnval = true;
                                                            }
                                                            else
                                                            {
                                                                data.returnval = false;
                                                            }

                                                        }
                                                    }


                                                }
                                            }
                                        }

                                    }
                                }

                                var Studenttransport = _context.PA_Student_Transport_ApplicationDMO.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                if (Studenttransport.Count() > 0)
                                {
                                    var StudentDetails = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();
                                    if (StudentDetails.Count() > 0)
                                    {
                                        var feegroupid = _context.TR_Location_FeeGroup_MappingDMO.Where(g => g.MI_Id == StudentDetails.FirstOrDefault().MI_Id && g.ASMAY_Id == StudentDetails.FirstOrDefault().ASMAY_Id && g.TRML_Id == Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id && g.TRLFM_ActiveFlag == true).ToList();

                                        TR_student_LocMappingDMO Transportdata = new TR_student_LocMappingDMO();
                                        Transportdata.ASMAY_Id = StudentDetails.FirstOrDefault().ASMAY_Id;
                                        Transportdata.MI_Id = StudentDetails.FirstOrDefault().MI_Id;
                                        Transportdata.AMST_Id = getstudentamstid.FirstOrDefault().AMST_Id;
                                        Transportdata.TRMR_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRMR_Id);
                                        Transportdata.TRML_Id = Convert.ToInt64(Studenttransport.FirstOrDefault().PASTA_PickUp_TRML_Id);
                                        Transportdata.FMG_Id = feegroupid.FirstOrDefault().FMG_Id;
                                        Transportdata.TRSLM_ActiveFlag = true;
                                        Transportdata.CreatedDate = DateTime.Now;
                                        Transportdata.UpdatedDate = DateTime.Now;
                                        _context.Add(Transportdata);
                                        confirmstatus = _context.SaveChanges();
                                        if (confirmstatus > 0)
                                        {
                                            data.returnval = true;
                                        }
                                        else
                                        {
                                            data.returnval = false;
                                        }
                                    }
                                }

                                if (data.returnval == true)
                                {
                                    var Updatedetails = _context.Adm_M_Student.Where(d => d.AMST_Id == getstudentamstid.FirstOrDefault().AMST_Id && d.AMST_SOL == "S").ToList();
                                    if (Updatedetails.Count() > 0)
                                    {
                                        var studentconcession = _context.StudentApplication.Where(t => t.pasr_id == data.studentdetails[j].AMST_Id).ToList();

                                        if (studentconcession.FirstOrDefault().FMCC_ID != null && studentconcession.FirstOrDefault().FMCC_ID > 0)
                                        {
                                            var concessiontype = _context.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == studentconcession.FirstOrDefault().FMCC_ID).FMCC_ConcessionFlag;

                                            if (concessiontype != "E")
                                            {
                                                var regstatus = _db.Database.ExecuteSqlCommand("AUTOFEEMAPPING_AFTER_TRANSFER  @p0,@p1,@p2,@p3,@p4,@p5", getstudentamstid.FirstOrDefault().AMST_Id, data.studentdetails[j].AMST_Id, Updatedetails.FirstOrDefault().MI_Id, Updatedetails.FirstOrDefault().ASMAY_Id, 0, concessiontype);
                                            }
                                            else if (concessiontype == "E")
                                            {
                                                var Sibemployee = _context.PAStudentEmployee.Where(d => d.PASR_Id == data.studentdetails[j].AMST_Id).ToList();
                                                if (Sibemployee.Count() > 0)
                                                {
                                                    var regstatus = _db.Database.ExecuteSqlCommand("AUTOFEEMAPPING_AFTER_TRANSFER  @p0,@p1,@p2,@p3,@p4,@p5", getstudentamstid.FirstOrDefault().AMST_Id, data.studentdetails[j].AMST_Id, Updatedetails.FirstOrDefault().MI_Id, Updatedetails.FirstOrDefault().ASMAY_Id, Sibemployee.FirstOrDefault().HRME_Id, concessiontype);
                                                }
                                            }
                                        }

                                    }
                                }
                            }

                            List<Adm_M_Student> getcurr = new List<Adm_M_Student>();
                            long mobileno = 0;
                            string MailId = "";

                            var getdetails = (from a in _db.StudentApplication
                                              where (a.pasr_id == data.studentdetails[j].AMST_Id)
                                              select new Adm_M_StudentDTO
                                              {
                                                  AMST_MobileNo = a.PASR_MobileNo,
                                                  AMST_emailId = a.PASR_emailId
                                              }).ToList();

                            mobileno = getdetails.FirstOrDefault().AMST_MobileNo;
                            MailId = getdetails.FirstOrDefault().AMST_emailId;

                            var AdmstudDet = _context.Adm_Master_Student_PA.Where(t => t.PASR_Id == data.studentdetails[j].AMST_Id).ToList();


                            SMS sms = new SMS(_context);

                            string s = await sms.sendSms(data.MI_Id, mobileno, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));

                            Email Email = new Email(_context);

                            string m = Email.sendmail(data.MI_Id, MailId, "ExportToAdmission", Convert.ToInt64(AdmstudDet.FirstOrDefault().AMST_Id));
                        }
                        else
                        {

                        }
                        j++;
                    }

                }
                data.returnMsg = "true";
            }
            catch (Exception e)
            {
                data.returnMsg = "false";
                Console.WriteLine(e.Message);
                return data;
            }
            return data;
        }

        public Adm_M_StudentDTO getAcademicdata(int id)
        {
            Adm_M_StudentDTO data = new Adm_M_StudentDTO();
            try
            {
                List<MasterAcademic> aca = new List<MasterAcademic>();
                aca = _db.MasterAcademic.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.fillacademicyr = aca.ToArray();

                List<School_M_Class> cls = new List<School_M_Class>();
                cls = _db.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = cls.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public Adm_M_StudentDTO getserdata(Adm_M_StudentDTO data)
        {
            try
            {

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _db.masterConfig.Where(d => d.MI_Id.Equals(data.MI_Id) && d.ASMAY_Id.Equals(data.ASMAY_Id)).ToList();

                if (data.ASMCL_Id == 0)
                {
                    if (mstConfig.FirstOrDefault().ISPAC_AdmissionTransfer == 1)
                    {
                        data.preAdmtoAdmStuList = (from a in _db.StudentApplication
                                                   from b in _db.AdmissionStatus
                                                   from c in _db.School_M_Class
                                                   where (a.PAMS_Id == b.PAMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.PASR_Adm_Confirm_Flag == false)
                                                   select new Adm_M_StudentDTO
                                                   {
                                                       Name = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim(),
                                                       AMST_FirstName = a.PASR_FirstName,
                                                       AMST_MiddleName = a.PASR_MiddleName,
                                                       AMST_LastName = a.PASR_LastName,
                                                       AMST_RegistrationNo = a.PASR_RegistrationNo,
                                                       classname = c.ASMCL_ClassName,
                                                       AMST_Id = a.pasr_id
                                                   }
                    ).OrderBy(a => a.Name).ToArray();
                    }
                    else
                    {
                        data.preAdmtoAdmStuList = (from a in _db.StudentApplication
                                                   from b in _db.AdmissionStatus
                                                   from c in _db.School_M_Class
                                                   where (a.PAMS_Id == b.PAMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.PASR_Adm_Confirm_Flag == false && a.PASR_FinalpaymentFlag == 1)
                                                   select new Adm_M_StudentDTO
                                                   {
                                                       Name = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim(),
                                                       AMST_FirstName = a.PASR_FirstName,
                                                       AMST_MiddleName = a.PASR_MiddleName,
                                                       AMST_LastName = a.PASR_LastName,
                                                       AMST_RegistrationNo = a.PASR_RegistrationNo,
                                                       classname = c.ASMCL_ClassName,
                                                       AMST_Id = a.pasr_id
                                                   }
                    ).OrderBy(a => a.Name).ToArray();
                    }
                }
                else
                {
                    if (mstConfig.FirstOrDefault().ISPAC_AdmissionTransfer == 1)
                    {
                        data.preAdmtoAdmStuList = (from a in _db.StudentApplication
                                                   from b in _db.AdmissionStatus
                                                   from c in _db.School_M_Class
                                                   where (a.PAMS_Id == b.PAMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.PASR_Adm_Confirm_Flag == false && a.ASMCL_Id == data.ASMCL_Id)
                                                   select new Adm_M_StudentDTO
                                                   {
                                                       Name = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim(),
                                                       AMST_FirstName = a.PASR_FirstName,
                                                       AMST_MiddleName = a.PASR_MiddleName,
                                                       AMST_LastName = a.PASR_LastName,
                                                       AMST_RegistrationNo = a.PASR_RegistrationNo,
                                                       classname = c.ASMCL_ClassName,
                                                       AMST_Id = a.pasr_id
                                                   }
                    ).OrderBy(a => a.Name).ToArray();
                    }
                    else
                    {
                        data.preAdmtoAdmStuList = (from a in _db.StudentApplication
                                                   from b in _db.AdmissionStatus
                                                   from c in _db.School_M_Class
                                                   where (a.PAMS_Id == b.PAMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.PAMST_StatusFlag == "CNF" && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.PASR_Adm_Confirm_Flag == false && a.ASMCL_Id == data.ASMCL_Id && a.PASR_FinalpaymentFlag == 1)
                                                   select new Adm_M_StudentDTO
                                                   {
                                                       Name = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim(),
                                                       AMST_FirstName = a.PASR_FirstName,
                                                       AMST_MiddleName = a.PASR_MiddleName,
                                                       AMST_LastName = a.PASR_LastName,
                                                       AMST_RegistrationNo = a.PASR_RegistrationNo,
                                                       classname = c.ASMCL_ClassName,
                                                       AMST_Id = a.pasr_id
                                                   }
                   ).OrderBy(a => a.Name).ToArray();
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        //protected void Mail(string MailText, string MailId)
        //{
        //    string body = this.PopulateBody(MailText);
        //    this.SendHtmlFormattedEmail(MailId, body);
        //}

        //private string PopulateBody(string MailText)
        //{
        //    string body = string.Empty;
        //    string[] res = System.IO.File.ReadAllLines(@"..\corewebapi18072016\wwwroot\EmailTemplate\ManualReportEmail.html");
        //    string str = string.Join("", res);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(str);
        //    System.IO.MemoryStream stream = new MemoryStream(byteArray);

        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    body = body.Replace("{MailContent}", MailText);
        //    return body;
        //}

        //private void SendHtmlFormattedEmail(string recepientEmail, string body)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Vapstech", "vapstech123@gmail.com"));
        //    message.To.Add(new MailboxAddress("", recepientEmail));
        //    message.Subject = "subject";
        //    var bodyBuilder = new BodyBuilder();
        //    bodyBuilder.HtmlBody = body;
        //    message.Body = bodyBuilder.ToMessageBody();
        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.gmail.com", 587);

        //        client.AuthenticationMechanisms.Remove("XOAUTH2");

        //        client.Authenticate("vapstech123@gmail.com", "vaps@123");
        //        client.Send(message);
        //        client.Disconnect(true);
        //    }
        //}

        //public async Task<string> sendSms(string workingKey, string sender, long mobileNo, string message)
        //{
        //    System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=" + workingKey + "&sender=" + sender + "&to=" + mobileNo + "&message=" + message) as HttpWebRequest;
        //    //optional
        //    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
        //    Stream stream = response.GetResponseStream();
        //    return "success";
        //}

    }
}






