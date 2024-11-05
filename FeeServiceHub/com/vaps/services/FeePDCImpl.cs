using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeePDCImpl : interfaces.FeePDCInterface
    {
        private static ConcurrentDictionary<string, FeePDCDTO> _login =
       new ConcurrentDictionary<string, FeePDCDTO>();

        public FeeGroupContext _FeeGroupHeadContext;
        public DomainModelMsSqlServerContext _context;


        public FeePDCImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext db)
        {

            _FeeGroupHeadContext = frgContext;
            _context = db;

        }

        public FeePDCDTO SaveGroupData(FeePDCDTO FGpage)
        {
            bool returnresult = false;
            FeePDCDMO feepge = Mapper.Map<FeePDCDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FPDC_Id > 0)
                {

                    //var result1 = _FeeGroupHeadContext.FeePDCDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.FMT_Id == feepge.FMT_Id && t.FMH_Order == feepge.FMH_Order).ToList();
                    var result1 = _FeeGroupHeadContext.FeePDCDMO.Where(t => t.FPDC_Id != feepge.FPDC_Id && t.FMT_Id == feepge.FMT_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.AMST_Id==feepge.AMST_Id).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {


                        var result = _FeeGroupHeadContext.FeePDCDMO.Single(t => t.FPDC_Id == feepge.FPDC_Id);
                        result.FMT_Id = FGpage.FMT_Id;
                        result.ASMAY_Id = FGpage.ASMAY_Id;
                        result.AMST_Id = FGpage.AMST_Id;
                        result.FPDC_ChequeDate = FGpage.FPDC_ChequeDate;
                        result.FCSPDC_Amount = FGpage.FCSPDC_Amount;
                        result.FMBANK_Id = FGpage.FMBANK_Id;
                        result.FPDC_Currency = FGpage.FPDC_Currency;
                        result.FPDC_Narration = FGpage.FPDC_Narration;
                        result.FPDC_Status = "Pending";
                        result.FPDC_ActiveFlg = FGpage.FPDC_ActiveFlg;
                        result.FPDC_CreatedDate = FGpage.FPDC_CreatedDate;
                        result.FPDC_UpdatedDate = FGpage.FPDC_UpdatedDate;
                        result.FPDC_Updatedby = FGpage.user_id;
                        result.FPDC_CreatedBy = FGpage.user_id;

                        _FeeGroupHeadContext.Update(result);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                            FGpage.message = "Update";
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    // var result = _FeeGroupHeadContext.FeePDCDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.FMT_Id== feepge.FMT_Id && t.FMH_Order==feepge.FMH_Order).ToList();
                    var result = _FeeGroupHeadContext.FeePDCDMO.Where(t => t.ASMAY_Id == feepge.ASMAY_Id && t.FMT_Id == feepge.FMT_Id && t.AMST_Id==feepge.AMST_Id && t.FMBANK_Id==feepge.FMBANK_Id && t.FPDC_ChequeNo==feepge.FPDC_ChequeNo && t.FPDC_ChequeDate==feepge.FPDC_ChequeDate).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.FMT_Id = feepge.FMT_Id;
                        feepge.ASMAY_Id = feepge.ASMAY_Id;
                        feepge.AMST_Id = feepge.AMST_Id;
                        feepge.MI_Id = feepge.MI_Id;
                        feepge.FPDC_ChequeNo = feepge.FPDC_ChequeNo;
                        feepge.FPDC_ChequeDate = feepge.FPDC_ChequeDate;
                        feepge.FCSPDC_Amount = feepge.FCSPDC_Amount;
                        feepge.FMBANK_Id = feepge.FMBANK_Id;
                        feepge.FPDC_Currency = feepge.FPDC_Currency;
                        feepge.FPDC_Narration = feepge.FPDC_Narration;
                        feepge.FPDC_Status = "Pending";
                        feepge.FPDC_ActiveFlg = feepge.FPDC_ActiveFlg;
                        feepge.FPDC_CreatedDate = DateTime.Now;
                        feepge.FPDC_UpdatedDate = DateTime.Now;

                        _FeeGroupHeadContext.Add(feepge);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {


                            returnresult = true;
                            FGpage.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }

                FGpage.GroupHeadData = (from A in _FeeGroupHeadContext.FeePDCDMO
                                       from B in _FeeGroupHeadContext.AcademicYear
                                       from C in _FeeGroupHeadContext.feeTr
                                       from D in _FeeGroupHeadContext.Adm_M_Student
                                       from E in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                       from F in _FeeGroupHeadContext.Fee_Master_BankDMO
                                       from G in _FeeGroupHeadContext.School_M_Class
                                       from H in _FeeGroupHeadContext.school_M_Section
                                       where (A.ASMAY_Id == B.ASMAY_Id && A.FMT_Id == C.FMT_Id && C.MI_Id == feepge.MI_Id && A.AMST_Id == D.AMST_Id && A.AMST_Id == E.AMST_Id && A.ASMAY_Id == E.ASMAY_Id && A.FMBANK_Id == F.FMBANK_Id && E.ASMCL_Id == G.ASMCL_Id && E.ASMS_Id == H.ASMS_Id)
                                       select new FeePDCDTO
                                       {
                                           FPDC_ChequeNo = A.FPDC_ChequeNo,
                                           FPDC_ChequeDate = A.FPDC_ChequeDate,
                                           FCSPDC_Amount = A.FCSPDC_Amount,
                                           FMBANK_Id = A.FMBANK_Id,
                                           FPDC_Currency = A.FPDC_Currency,

                                           FPDC_Narration = A.FPDC_Narration,
                                           FPDC_Status = A.FPDC_Status,
                                           FPDC_ActiveFlg = A.FPDC_ActiveFlg,
                                           FPDC_CreatedDate = A.FPDC_CreatedDate,
                                           FPDC_UpdatedDate = A.FPDC_UpdatedDate,
                                           ASMAY_Year = B.ASMAY_Year,
                                           FPDC_Id = A.FPDC_Id,
                                           FMT_Id = A.FMT_Id,
                                           ASMAY_Id = A.ASMAY_Id,
                                           FMT_Name = C.FMT_Name,
                                           AMST_FirstName = D.AMST_FirstName + ' ' + D.AMST_MiddleName + ' ' + D.AMST_LastName,
                                           classname = G.ASMCL_ClassName,
                                           sectionname = H.ASMC_SectionName,
                                           FMBANK_BankName = F.FMBANK_BankName,
                                           ASMS_Id = H.ASMS_Id,
                                           ASMCL_Id = G.ASMCL_Id,
                                           AMST_Id = A.AMST_Id,


                                       }
                        ).ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }


        public FeePDCDTO getdetails(int id)
        {
            FeePDCDTO FGRDT = new FeePDCDTO();

            try
            {
               
                FGRDT.GroupHeadData = (from A in _FeeGroupHeadContext.FeePDCDMO
                                       from B in _FeeGroupHeadContext.AcademicYear
                                       from C in _FeeGroupHeadContext.feeTr
                                       from D in _FeeGroupHeadContext.Adm_M_Student
                                       from E in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                       from F in _FeeGroupHeadContext.Fee_Master_BankDMO
                                       from G in _FeeGroupHeadContext.School_M_Class
                                       from H in _FeeGroupHeadContext.school_M_Section
                                       where (A.ASMAY_Id == B.ASMAY_Id && A.FMT_Id == C.FMT_Id && C.MI_Id == id && A.AMST_Id==D.AMST_Id && A.AMST_Id==E.AMST_Id && A.ASMAY_Id==E.ASMAY_Id && A.FMBANK_Id==F.FMBANK_Id && E.ASMCL_Id==G.ASMCL_Id && E.ASMS_Id==H.ASMS_Id)
                                       select new FeePDCDTO
                                       {
                                           FPDC_ChequeNo = A.FPDC_ChequeNo,
                                           FPDC_ChequeDate = A.FPDC_ChequeDate,
                                           FCSPDC_Amount = A.FCSPDC_Amount,
                                           FMBANK_Id = A.FMBANK_Id,
                                           FPDC_Currency = A.FPDC_Currency,

                                           FPDC_Narration = A.FPDC_Narration,
                                           FPDC_Status = A.FPDC_Status,
                                           FPDC_ActiveFlg = A.FPDC_ActiveFlg,
                                           FPDC_CreatedDate = A.FPDC_CreatedDate,
                                           FPDC_UpdatedDate = A.FPDC_UpdatedDate,
                                           ASMAY_Year = B.ASMAY_Year,
                                           FPDC_Id = A.FPDC_Id,
                                           FMT_Id=A.FMT_Id,
                                           ASMAY_Id = A.ASMAY_Id,
                                           FMT_Name = C.FMT_Name,
                                           AMST_FirstName =D.AMST_FirstName + ' ' + D.AMST_MiddleName + ' ' + D.AMST_LastName,
                                           classname =G.ASMCL_ClassName,
                                           sectionname = H.ASMC_SectionName,
                                           FMBANK_BankName=F.FMBANK_BankName,
                                           ASMS_Id=H.ASMS_Id,
                                           ASMCL_Id=G.ASMCL_Id,
                                           AMST_Id=A.AMST_Id,
                                           

                                       }
                            ).ToArray();






                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(o => o.ASMAY_Order).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();

                FGRDT.termlist = _FeeGroupHeadContext.feeTr.Where(t => t.MI_Id == id && t.FMT_ActiveFlag == true).ToArray();

                FGRDT.FillBank = _FeeGroupHeadContext.Fee_Master_BankDMO.Where(t => t.MI_Id == id && t.FMBANK_ActiveFlg == true).ToArray();


                FGRDT.fillclass = (
                                  from b in _FeeGroupHeadContext.School_M_Class
                                  where (b.MI_Id == id)
                                  select new FeeStudentAdjustmentDTO
                                  {
                                      ASMCL_Id = b.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                FGRDT.fillsection = (
                                    from b in _FeeGroupHeadContext.school_M_Section
                                    where (b.MI_Id == id)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        ASMS_Id = b.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();



            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeePDCDTO EditgroupDetails(int id)
        {
            FeePDCDTO FMG = new FeePDCDTO();
            try
            {
                List<FeePDCDMO> masterfeegroup = new List<FeePDCDMO>();
                masterfeegroup = _FeeGroupHeadContext.FeePDCDMO.Where(t => t.FPDC_Id.Equals(id)).ToList();
                FMG.GroupHeadData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeePDCDTO GetGroupSearchData(FeePDCDTO mas)
        {

            FeePDCDTO FGRDT = new FeePDCDTO();
            try
            {
                List<FeePDCDMO> feegrp = new List<FeePDCDMO>();
                feegrp = _FeeGroupHeadContext.FeePDCDMO.OrderBy(t => t.FPDC_Id).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeePDCDTO getpageedit(int id)
        {
            FeePDCDTO page = new FeePDCDTO();
            try
            {
                List<FeePDCDMO> lorg = new List<FeePDCDMO>();
                lorg = _FeeGroupHeadContext.FeePDCDMO.Where(t => t.FPDC_Id.Equals(id)).ToList();
                page.GroupHeadData = lorg.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public FeePDCDTO deactivate(FeePDCDTO acd)
        {
            try
            {
                FeePDCDMO feepge = Mapper.Map<FeePDCDMO>(acd);
                if (feepge.FPDC_Id > 0)
                {

                    var result = _FeeGroupHeadContext.FeePDCDMO.Single(t => t.FPDC_Id == feepge.FPDC_Id);



                    _FeeGroupHeadContext.Remove(result);
                    var flag = _FeeGroupHeadContext.SaveChanges();
                    if (flag == 1)
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }


                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        public FeePDCDTO PDCRemainder(FeePDCDTO data)        {

            var template = _context.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "PDCRemainder"
            && e.ISES_MailActiveFlag == true).ToList();            List<FeePDCDTO> devicelist = new List<FeePDCDTO>();            List<FeePDCDTO> deviceidsnew = new List<FeePDCDTO>();            try            {                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "PDC_Remainder";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",                  SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });

                    cmd.Parameters.Add(new SqlParameter("@alertdays",                  SqlDbType.BigInt)                    {                        Value = template[0].ISES_AlertBeforeDays                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                devicelist.Add(new FeePDCDTO                                {                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),                                    AMST_EmailId = Convert.ToString(dataReader["AMST_EmailId"]),                                    FPDC_Id = Convert.ToInt64(dataReader["FPDC_Id"]),

                                });                            }                        }                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                int k = 0;                foreach (var deviceid in devicelist)                {                    string m = sendmail(deviceid.MI_Id, template[0].ISES_Template_Name,  deviceid.AMST_EmailId, deviceid.FPDC_Id);                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return data;        }


        public string sendmail(long MI_Id, string Template,  string Email, long FPDC_Id)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();


                string Resultsms = Mailcontent;
                string result = Mailmsg;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeePDCREMAINDERS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@FPDC_Id",
                  SqlDbType.VarChar)
                    {
                        Value = FPDC_Id
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
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();

                    var message = new SendGridMessage();
                    string mailcc = "";
                    string mailbcc = "";

                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    //message.AddTo("hemanth@unnathimarketing.com");
                    message.AddTo(Email)

;
                    //**************** EMAIL CC DETAILS *************//

                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //**************** EMAIL BCC DETAILS *************//

                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                        foreach (var d in ccmaildetails)
                        {
                            message.AddCc(d);
                        }
                    }

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }





                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = Mailcontent
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }

    }
}
