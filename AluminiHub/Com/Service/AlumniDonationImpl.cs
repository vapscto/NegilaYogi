using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Razorpay.Api;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;
using System.Net;
using System.IO;
using SendGrid;
using System.Net.Mail;
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using CommonLibrary;
using PreadmissionDTOs;

namespace AlumniHub.Com.Service
{
    public class AlumniDonationImpl : Interface.AlumniDonationInterface
    {
        public AlumniContext _AlumniContext;
        public DomainModelMsSqlServerContext _context;
        public AlumniDonationImpl(AlumniContext AlumniContext)
        {
            _AlumniContext = AlumniContext;

        }

        public AlumniStudentDTO Pageload(AlumniStudentDTO data)
        {
            try
            {


                var regid = _AlumniContext.Alumni_User_LoginDMO.Single(a => a.IVRMUL_Id == data.userid);

                //using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                //{

                //    cmd.CommandText = "ALU_AlumniDetails";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //SqlDbType.BigInt)
                //    {
                //        Value = data.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ALSREG_Id",
                // SqlDbType.BigInt)
                //    {
                //        Value = regid.ALSREG_Id
                //    });
                //     if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] 
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.almdetails = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                data.almdetails = (from a in _AlumniContext.Alumni_M_StudentDMO
                                   from c in _AlumniContext.School_M_Class
                                   from d in _AlumniContext.AcademicYear
                                   from e in _AlumniContext.AlumniUserRegistrationDMO
                                   from f in _AlumniContext.state
                                   from g in _AlumniContext.Country
                                   where (a.ASMCL_Id_Left == c.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id_Left == d.ASMAY_Id && a.ALMST_Id == e.ALMST_Id && e.ALSREG_Id == regid.ALSREG_Id && a.ALMST_ConState == f.IVRMMS_Id && g.IVRMMC_Id == a.ALMST_ConCountryId)
                                   select new AlumniStudentDTO
                                   {
                                       ALMST_FirstName = ((a.ALMST_FirstName == null || a.ALMST_FirstName == "0" ? "" : a.ALMST_FirstName) +
                               (a.ALMST_MiddleName == null || a.ALMST_MiddleName == "0" ? "" : " " + a.ALMST_MiddleName) +
                               (a.ALMST_LastName == null || a.ALMST_LastName == "0" ? "" : " " + a.ALMST_LastName)).Trim(),
                                       ALMST_RegistrationNo = a.ALMST_RegistrationNo,
                                       ALMST_AdmNo = a.ALMST_AdmNo,
                                       classleft = c.ASMCL_ClassName,
                                       leftyear = d.ASMAY_Year,
                                       ALMST_MobileNo = a.ALMST_MobileNo,
                                       ALMST_emailId = a.ALMST_emailId,
                                       ALMST_StudentPhoto = a.ALMST_StudentPhoto,
                                       ALMST_Id = a.ALMST_Id,
                                       ALSREG_Id = e.ALSREG_Id,
                                       ALMST_ConStreet = a.ALMST_ConStreet,
                                       ALMST_ConArea = a.ALMST_ConArea,
                                       ALMST_ConAdd3 = a.ALMST_ConAdd3,
                                       ALMST_Taluk = a.ALMST_Taluk,
                                       ALMST_ConCity = a.ALMST_ConCity,
                                       ALMST_District = a.ALMST_District,
                                       IVRMMS_Name = f.IVRMMS_Name,
                                       IVRMMC_CountryName = g.IVRMMC_CountryName,
                                       ALMST_ConPincode = a.ALMST_ConPincode,
                                       ALMST_StudentPANCard = a.ALMST_StudentPANCard,
                                       ALMST_FatherName = a.ALMST_FatherName,
                                   }
).Distinct().ToArray();

                List<Alumni_Master_Donation> masnum = new List<Alumni_Master_Donation>();

                var dn = _AlumniContext.Alumni_Master_Donation_con.Where(t => t.MI_Id == data.MI_Id && t.ALMDON_RegistrationFeeFlag == true).ToArray();

                var getid = (from a in _AlumniContext.Alumni_User_LoginDMO
                             from b in _AlumniContext.Alumni_Donation_con
                             where a.ALSREG_Id == b.ALSREG_Id && a.IVRMUL_Id == data.userid && b.ALMDON_Id == dn[0].ALMDON_Id
                             select new AlumniStudentDTO
                             {
                                 ALDON_Id = b.ALDON_Id
                             }).ToList();
                if (getid.Count > 0)
                {
                    masnum = _AlumniContext.Alumni_Master_Donation_con.Where(t => t.MI_Id == data.MI_Id && t.ALMDON_ActiveFlag == true && t.ALMDON_RegistrationFeeFlag == false).ToList();
                }
                else
                {
                    masnum = _AlumniContext.Alumni_Master_Donation_con.Where(t => t.MI_Id == data.MI_Id && t.ALMDON_ActiveFlag == true).ToList();
                }

                data.fillalumnidonationdetails = masnum.ToArray();

                data.fillpaymentgateway = (from a in _AlumniContext.PAYUDETAILS
                                           from b in _AlumniContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_Id == b.IMPG_Id && a.IMPG_ActiveFlg == true && b.MI_Id == data.MI_Id)
                                           select new AlumniStudentDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               IMPG_PGFlag = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image,
                                               FPGD_PGName = b.FPGD_PGName
                                           }
).Distinct().ToArray();
                data.imagenew = _AlumniContext.Inst.Where(a => a.MI_Id == data.MI_Id).First().MI_Logo;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO getamount(AlumniStudentDTO dto)
        {
            try
            {
                dto.getamountlist = _AlumniContext.Alumni_Master_Donation_con.Where(a => a.MI_Id == dto.MI_Id && a.ALMDON_Id == dto.ALMDON_Id && a.ALMDON_ActiveFlag == true).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public AlumniStudentDTO getpayment_details(AlumniStudentDTO dto)
        {
            try
            {
                dto.paymentgateway = _AlumniContext.Fee_PaymentGateway_DetailsDMO.Where(a => a.MI_Id == dto.MI_Id && a.FPGD_PGName == dto.FPGD_PGName && a.FPGD_PGActiveFlag == "1").ToArray();
                GenerateOrderId(dto);
                dto.institution = _AlumniContext.Inst.Where(a => a.MI_Id == dto.MI_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        public AlumniStudentDTO GenerateOrderId(AlumniStudentDTO data)
        {
            try
            {
                List<AlumniStudentDTO> hr = new List<AlumniStudentDTO>();
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniStudentdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.ALMST_Id
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
                                hr.Add(new AlumniStudentDTO
                                {

                                    ALMST_FirstName = (dataReader["ALMST_FirstName"]).ToString(),
                                    ALMST_PerStreet = (dataReader["ALMST_PerStreet"]).ToString(),
                                    ALMST_PerArea = (dataReader["ALMST_PerArea"]).ToString(),
                                    ALMST_PerAdd3 = (dataReader["ALMST_PerAdd3"]).ToString(),
                                    ALMST_PerCity = (dataReader["ALMST_PerCity"]).ToString(),
                                    ALMST_PerPincode = Convert.ToInt32((dataReader["ALMST_PerPincode"])),

                                });
                            }
                        }
                        data.ALMST_FirstName = hr[0].ALMST_FirstName.ToString();
                        data.ALMST_PerStreet = hr[0].ALMST_PerStreet.ToString();
                        data.ALMST_PerArea = hr[0].ALMST_PerArea.ToString();
                        data.ALMST_PerAdd3 = hr[0].ALMST_PerAdd3.ToString();
                        data.ALMST_PerCity = hr[0].ALMST_PerCity.ToString();
                        data.ALMST_PerPincode = hr[0].ALMST_PerPincode;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _AlumniContext.Fee_PaymentGateway_DetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == data.FPGD_PGName && t.FPGD_PGActiveFlag == "1").Distinct().ToList();

                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("notes_1", data.ALMST_FirstName);
                transfersnotes.Add("notes_2", data.ALMST_PerStreet);
                transfersnotes.Add("notes_3", data.ALMST_PerArea);
                transfersnotes.Add("notes_4", data.ALMST_PerAdd3);
                transfersnotes.Add("notes_5", data.ALMST_PerCity);
                transfersnotes.Add("notes_6", data.ALMST_PerPincode);

                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", 1 * 100);
                input.Add("amount", data.ALMDON_Amount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("payment_capture", 1);
                input.Add("notes", transfersnotes);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                data.orderId = order["id"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AlumniStudentDTO paymentsave(AlumniStudentDTO dto)
        {
            try
            {
                var count = _AlumniContext.Alumni_Donation_con.Max(a => a.ALDON_ReceiptNo);
                if (count == null)
                {
                    if (dto.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        dto.transnumbconfigurationsettingsss.MI_Id = dto.MI_Id;
                        dto.transnumbconfigurationsettingsss.ASMAY_Id = dto.ASMAY_Id;

                        if (dto.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                        {
                            var acyr = _AlumniContext.AcademicYear.Where(t => t.ASMAY_Id.Equals(dto.ASMAY_Id)).FirstOrDefault();
                            string AcadYear = acyr.ASMAY_Year;

                            if (dto.transnumbconfigurationsettingsss.IMN_PrefixParticular != "" && dto.transnumbconfigurationsettingsss.IMN_PrefixParticular != null)
                            {
                                dto.trans_id = dto.transnumbconfigurationsettingsss.IMN_PrefixParticular;
                            }
                            else if (dto.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode == true)
                            {
                                dto.trans_id = AcadYear;
                            }


                            if (dto.transnumbconfigurationsettingsss.IMN_WidthNumeric != "" && dto.transnumbconfigurationsettingsss.IMN_WidthNumeric != null)
                            {

                                if (dto.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag == "Yes")
                                {
                                    if (dto.transnumbconfigurationsettingsss.IMN_StartingNo != "" && dto.transnumbconfigurationsettingsss.IMN_StartingNo != null)
                                    {
                                        dto.trans_id = dto.trans_id + "/" + dto.transnumbconfigurationsettingsss.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(dto.transnumbconfigurationsettingsss.IMN_WidthNumeric) - 0, '0');
                                    }
                                    else
                                    {
                                        dto.transnumbconfigurationsettingsss.IMN_StartingNo = "0";
                                        dto.trans_id = dto.trans_id + "/" + dto.transnumbconfigurationsettingsss.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(dto.transnumbconfigurationsettingsss.IMN_WidthNumeric) - 0, '0');
                                    }
                                }
                                else if (dto.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag == "Numeric")
                                {
                                    if (dto.transnumbconfigurationsettingsss.IMN_StartingNo != "" && dto.transnumbconfigurationsettingsss.IMN_StartingNo != null)
                                    {
                                        dto.trans_id = dto.trans_id + dto.transnumbconfigurationsettingsss.IMN_StartingNo.ToString();
                                    }
                                }
                                else
                                {
                                    if (dto.transnumbconfigurationsettingsss.IMN_StartingNo != "" && dto.transnumbconfigurationsettingsss.IMN_StartingNo != null)
                                    {
                                        dto.trans_id = dto.trans_id + "/" + dto.transnumbconfigurationsettingsss.IMN_StartingNo.ToString();
                                    }
                                }


                            }
                            else
                            {
                                if (dto.transnumbconfigurationsettingsss.IMN_StartingNo != "" && dto.transnumbconfigurationsettingsss.IMN_StartingNo != null)
                                {
                                    dto.trans_id = dto.trans_id + "/" + dto.transnumbconfigurationsettingsss.IMN_StartingNo.ToString();
                                }
                            }

                            if (dto.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode == true)
                            {
                                dto.trans_id = dto.trans_id + "/" + AcadYear;
                            }
                            else if (dto.transnumbconfigurationsettingsss.IMN_SuffixParticular != "" && dto.transnumbconfigurationsettingsss.IMN_SuffixParticular != null)
                            {
                                dto.trans_id = dto.trans_id + "/" + dto.transnumbconfigurationsettingsss.IMN_SuffixParticular;
                            }

                            dto.trans_id = checkDublicateandIncreamentForAdmissionRegNumber(dto.trans_id, dto.transnumbconfigurationsettingsss);
                        }
                    }
                }
                else
                {
                    var count1 = _AlumniContext.Alumni_Donation_con.LastOrDefault();

                    string[] lastRecordArray = count1.ALDON_ReceiptNo.Split('/');
                    if (lastRecordArray != null)
                    {
                        int staringNumberInc = 0;
                        string lastfield = "";
                        string firstfield = lastRecordArray.ElementAt(0);
                        staringNumberInc = Convert.ToInt32(lastRecordArray.ElementAt(1));
                        staringNumberInc = staringNumberInc + 1;
                        var acyr = _AlumniContext.AcademicYear.Where(t => t.ASMAY_Id.Equals(dto.ASMAY_Id)).FirstOrDefault();
                        lastfield = acyr.ASMAY_Year;

                        dto.trans_id = firstfield + "/" + staringNumberInc + "/" + lastfield;

                    }
                }
                //if (dto.ALMST_StudentPANCard != null)
                //{
                //    var result = _AlumniContext.Alumni_M_StudentDMO.Single(a => a.ALMST_Id == dto.ALMST_Id && a.MI_Id == dto.MI_Id);
                //    result.ALMST_StudentPANCard = dto.ALMST_StudentPANCard;
                //    _AlumniContext.Update(result);
                //    _AlumniContext.SaveChanges();
                //}
                Alumni_Donation ad = new Alumni_Donation();
                ad.ALMDON_Id = dto.ALMDON_Id;
                ad.ALSREG_Id = dto.ALSREG_Id;
                ad.ALDON_DonorName = dto.ALDON_DonorName;
                ad.ALDON_Amount = dto.ALDON_Amount;
                ad.ALDON_Date = dto.ALDON_Date;
                ad.ALDON_ReceiptNo = dto.trans_id;
                ad.ALDON_ModeOfPayment = "O";
                ad.ALDON_ReferenceNo = dto.ReceiptNo;
                ad.ALDON_DonarPANNo = dto.ALMST_StudentPANCard;
                ad.ALDON_NRIFlg = dto.ALDON_NRIFlg;
                ad.ALDON_Towards = dto.ALDON_Towards;
                ad.ALDON_ActiveFlag = true;
                ad.ALDON_CreatedBy = dto.userid;
                ad.ALDON_CreatedDate = DateTime.Today;
                _AlumniContext.Add(ad);
                var res = _AlumniContext.SaveChanges();


                if (res > 0)
                {

                    dto.returnval = "true";
                    var TemplateName = "AlumniDonation";
                    var result = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.ALMST_Id == dto.ALMST_Id).ToList();
                    email_trigger(dto.ALSREG_Id, dto.ALDON_Amount, result[0].ALMST_emailId, result[0].MI_Id, TemplateName, dto.ReceiptNo, dto.orderId, dto.Template, dto.ALDON_DonorName, dto.trans_id, dto.ALMDON_RegistrationFeeFlag);

                }
                else
                {
                    dto.returnval = "false";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public void email_trigger(long ALSREG_Id, decimal amount, string email, long MI_Id, string TemplateName, string ReceiptNo, string orderId, string Template, string ALDON_DonorName, string trans_id, bool ALMDON_RegistrationFeeFlag)
        {
            try
            {
                var TempName = "";
                var TempNamepdf = "";
                if (ALMDON_RegistrationFeeFlag == false)
                {
                    TempName = "DonationReceipt";
                    TempNamepdf = "DonationReceipt.Pdf";
                }
                else
                {
                    TempName = "AlumniRegistration";
                    TempNamepdf = "AlumniRegistration.Pdf";
                }


                string modified1 = Template.Replace("orderid", trans_id);
                string modified = modified1.Replace("paymentid", ReceiptNo);

                string mailid = email;
                var institutionName = _AlumniContext.Inst.Where(i => i.MI_Id == MI_Id).ToList();


                var temp = _AlumniContext.smsEmailSetting.Where(a => a.MI_Id == MI_Id &&
                                a.ISES_Template_Name == TempName).ToList();


                string temp1 = temp[0].ISES_MailHTMLTemplate;
                //string Mailmsg1 = "Dear ALDONDonorName ,<br><br> Thank you for your great generosity! We, at Janaseva Trust, greatly appreciate your donation, and your support. Your support helps to further our work and mission in providing quality education and social activities of Janaseva Trust.<br> You can find your donation receipt and corresponding details attached along with this email. <br> Your support is invaluable to us, thank you again! If you have specific questions about our work and mission be sure to visit our website www.janasevatrust.in or reach out to 9241216119 / 9480609078. <br><br> Thanking you. <br><br>Sincerely,<br>Sri.Nirmal Kumar<br>Hon.Secretary, Janaseva Trust";

                string Mailmsg = temp1.Replace("ALDONDonorName", ALDON_DonorName);




                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _AlumniContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();



                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = temp[0].ISES_MailCCId;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string Subject = temp[0].ISES_MailSubject;


                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(mailid);
                    string mailbcc = "";
                    if (temp[0].ISES_MailCCId != null && temp[0].ISES_MailCCId != "")
                    {
                        string[] ccmail = temp[0].ISES_MailCCId.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }






                    var img = _AlumniContext.Inst.Where(a => a.MI_Id == MI_Id).ToList();
                    var webClient = new WebClient();






                    StringBuilder sb = new StringBuilder(modified);
                    StringReader sr = new StringReader(sb.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();

                        htmlparser.Parse(sr);
                        pdfDoc.Close();

                        byte[] bytess = memoryStream.ToArray();
                        memoryStream.Close();

                        var file = Convert.ToBase64String(bytess);

                        string emp;
                        emp = Convert.ToString(sr);
                        string v = emp.Replace("System.IO.StringReader", TempNamepdf);
                        string v1 = emp.Replace("System.IO.StringReader", "80GForm.Pdf");
                        try
                        {


                            if (img[0].MI_80GRegNo != null)
                            {
                                byte[] imageBytes = webClient.DownloadData(img[0].MI_80GRegNo);
                                string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);

                                message.AddAttachment(v, file);
                                message.AddAttachment(v1, fileContentsAsBase64, null, null, null);
                                message.HtmlContent = Mailmsg;
                                message.AddCc(temp[0].ISES_MailCCId);
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();
                            }
                            else
                            {

                                message.AddAttachment(v, file);
                                message.AddCc(mailcc);
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();
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

        }
        public AlumniStudentDTO getdonationreport(AlumniStudentDTO dto)
        {
            try
            {

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniDonationReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                    { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                    { Value = dto.todate });



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
                        dto.donationlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        //===============master donation
        public AlumniStudentDTO getdata_donation(AlumniStudentDTO dto)
        {
            try
            {
                dto.alumnidonationlist = _AlumniContext.Alumni_Master_Donation_con.Where(t => t.MI_Id == dto.MI_Id).ToArray();


            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public AlumniStudentDTO save_donation(AlumniStudentDTO dto)
        {
            try
            {

                if (dto.ALMDON_Id > 0)
                {
                    var dup1 = _AlumniContext.Alumni_Master_Donation_con.Where(a => a.ALMDON_RegistrationFeeFlag == dto.ALMDON_RegistrationFeeFlag && a.MI_Id == dto.MI_Id).ToList();
                    if (dup1.Count > 0)
                    {
                        dto.message = "exist";
                    }
                    else
                    {
                        var dup = _AlumniContext.Alumni_Master_Donation_con.Where(a => a.ALMDON_DonationName == dto.ALMDON_DonationName && a.MI_Id == dto.MI_Id && a.ALMDON_Id != dto.ALMDON_Id && a.ALMDON_RegistrationFeeFlag == dto.ALMDON_RegistrationFeeFlag).ToList();
                        if (dup.Count > 0)
                        {
                            dto.message = "Duplicate";
                        }
                        else
                        {
                            var md = _AlumniContext.Alumni_Master_Donation_con.Single(a => a.ALMDON_Id == dto.ALMDON_Id && a.MI_Id == dto.MI_Id);
                            md.ALMDON_DonationName = dto.ALMDON_DonationName;
                            md.ALMDON_Amount = dto.ALMDON_Amount;
                            md.ALMDON_UpdatedDate = DateTime.Today;
                            md.ALMDON_UpdatedBy = dto.userid;
                            _AlumniContext.Update(md);
                            var cnt = _AlumniContext.SaveChanges();
                            if (cnt > 0)
                            {
                                dto.message = "Update";
                            }
                            else
                            {
                                dto.message = "Error";
                            }
                        }
                    }
                }
                else
                {
                    var dup1 = _AlumniContext.Alumni_Master_Donation_con.Where(a => a.ALMDON_RegistrationFeeFlag == dto.ALMDON_RegistrationFeeFlag && a.MI_Id == dto.MI_Id).ToList();
                    if (dup1.Count > 0)
                    {
                        dto.message = "exist";
                    }
                    else
                    {
                        var dup = _AlumniContext.Alumni_Master_Donation_con.Where(a => a.ALMDON_DonationName == dto.ALMDON_DonationName && a.MI_Id == dto.MI_Id && a.ALMDON_RegistrationFeeFlag == dto.ALMDON_RegistrationFeeFlag).ToList();
                        if (dup.Count > 0)
                        {
                            dto.message = "Duplicate";
                        }
                        else
                        {
                            Alumni_Master_Donation md = new Alumni_Master_Donation();
                            md.MI_Id = dto.MI_Id;
                            md.ALMDON_DonationName = dto.ALMDON_DonationName;
                            md.ALMDON_Amount = dto.ALMDON_Amount;
                            md.ALMDON_ActiveFlag = true;
                            md.ALMDON_RegistrationFeeFlag = dto.ALMDON_RegistrationFeeFlag;
                            md.ALMDON_CreatedDate = DateTime.Today;
                            md.ALMDON_CreatedBy = dto.userid;
                            _AlumniContext.Add(md);
                            var cnt = _AlumniContext.SaveChanges();
                            if (cnt > 0)
                            {
                                dto.message = "Add";
                            }
                            else
                            {
                                dto.message = "Error";
                            }
                        }
                    }
                }

            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public AlumniStudentDTO edit_donation(AlumniStudentDTO dto)
        {
            try
            {
                dto.edit_donation_list = _AlumniContext.Alumni_Master_Donation_con.Where(t => t.MI_Id == dto.MI_Id && t.ALMDON_Id == dto.ALMDON_Id).ToArray();
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public AlumniStudentDTO deactive_donation(AlumniStudentDTO dto)
        {
            try
            {
                if (dto.ALMDON_ActiveFlag == true)
                {
                    var md = _AlumniContext.Alumni_Master_Donation_con.Single(a => a.ALMDON_Id == dto.ALMDON_Id && a.MI_Id == dto.MI_Id);
                    md.ALMDON_ActiveFlag = false;
                    md.ALMDON_UpdatedDate = DateTime.Today;
                    md.ALMDON_UpdatedBy = dto.userid;
                    _AlumniContext.Update(md);
                    var cnt = _AlumniContext.SaveChanges();
                    if (cnt > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    var md = _AlumniContext.Alumni_Master_Donation_con.Single(a => a.ALMDON_Id == dto.ALMDON_Id && a.MI_Id == dto.MI_Id);
                    md.ALMDON_ActiveFlag = true;
                    md.ALMDON_UpdatedDate = DateTime.Today;
                    md.ALMDON_UpdatedBy = dto.userid;
                    _AlumniContext.Update(md);
                    var cnt = _AlumniContext.SaveChanges();
                    if (cnt > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }

        public AlumniStudentDTO alumnidetails(AlumniStudentDTO dto)
        {
            try
            {
                var dtl = "";
                if (dto.alumniregister == "AlumniDetails")
                {
                    dtl = "Details";
                }
                else if (dto.alumniregister == "AlumniPayment")
                {
                    dtl = "Payment";
                }
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniRegistrationReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                    { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                    { Value = dto.todate });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    { Value = dtl });



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
                        dto.alumnidetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public string checkDublicateandIncreamentForAdmissionRegNumber(string GeneratedNumber, Master_NumberingDTO en)
        {
            List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();

            int count = 0;
            int staringNumberInc = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _AlumniContext.Alumni_Donation_con.Where(imp => imp.ALDON_ReceiptNo.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _AlumniContext.Alumni_Donation_con.Where(imp => imp.ALDON_ReceiptNo.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string lastfield = "";
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    var acyr = _AlumniContext.AcademicYear.Where(t => t.ASMAY_Id.Equals(en.ASMAY_Id)).FirstOrDefault();
                    lastfield = acyr.ASMAY_Year;


                    staringNumberInc = staringNumber + 1;
                    GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;

                }

                GeneratedNumber = checkDublicateandIncreamentForAdmissionRegNumber(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public AlumniStudentDTO getdonationprint(AlumniStudentDTO dto)
        {
            try
            {
                var role = _AlumniContext.IVRM_Role_Type.Where(a => a.IVRMRT_Id == dto.roleId).ToList();
                dto.flag = role[0].IVRMRT_Role;
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniDonationReportPrint";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                    { Value = dto.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                    { Value = dto.todate });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    { Value = dto.userid });
                    cmd.Parameters.Add(new SqlParameter("@Role", SqlDbType.VarChar)
                    { Value = role[0].IVRMRT_Role });



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
                        dto.donationlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}





