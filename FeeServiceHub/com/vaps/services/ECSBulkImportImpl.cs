using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using CommonLibrary;
using System.IO;
using System.Reflection.Metadata;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using SendGrid;

namespace FeeServiceHub.com.vaps.services
{
    public class ECSBulkImportImpl:ECSBulkImportInterface
    {

        public FeeGroupContext _FeeGroupContext;
        ILogger<ECSBulkImportImpl> _ecsimpl;
        private DomainModelMsSqlServerContext _dbsms;
        public ECSBulkImportImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext dbsms)
        {
            _FeeGroupContext = frgContext;
            _dbsms = dbsms;
        }
        


        public ECSBulkImportDTO getdata123(ECSBulkImportDTO data)
        {

            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();//&& t.ASMAY_Id==data.ASMAY_Id
                data.yearlist = year.Distinct().ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
              

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
             

       
          


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public async Task<ECSBulkImportDTO> checkvalidation(ECSBulkImportDTO data)
        {
            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            string studentadmno = "Student Admission Number";
            int age = 0;
            var admtot = "";
            for (int i = 0; i < data.newlstget1.Length; i++)
                {  try
                        {
                            var admno = "";
                            admno = data.newlstget1[i].Admno.Trim().ToString();
                            var studname = "";
                            studname = data.newlstget1[i].PayeeName.Trim().ToString();
                            var trnsid = "";
                            trnsid = data.newlstget1[i].Transactionid.Trim().ToString();
                            long totamt = 0;
                            totamt = data.newlstget1[i].Amount;
                            long fineamt = 0;
                            fineamt = data.newlstget1[i].Fine;
                            string transdate_str = data.newlstget1[i].Transdate;
                           // var  transdate_str = transdate.ToString("dd/MM/yyyy");

                    if ((admno != null) && (studname != null) && (trnsid != null) && (totamt != 0) && (transdate_str != ""))
                    {
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "JSH_fee_Import";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@amst_adm_no",
                               SqlDbType.VarChar)
                            {
                                Value = admno
                            });
                            cmd.Parameters.Add(new SqlParameter("@transactionid",
                           SqlDbType.VarChar)
                            {
                                Value = trnsid
                            });
                            cmd.Parameters.Add(new SqlParameter("@guardianname",
                           SqlDbType.VarChar)
                            {
                                Value = studname
                            });

                            cmd.Parameters.Add(new SqlParameter("@paidamount",
                              SqlDbType.VarChar)
                            {
                                Value = totamt
                            });
                            cmd.Parameters.Add(new SqlParameter("@paiddate",
                               SqlDbType.VarChar)
                            {
                                Value = transdate_str
                            });

                            cmd.Parameters.Add(new SqlParameter("@fineamt",
                          SqlDbType.VarChar)
                            {
                                Value = fineamt
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
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
                                    data.stuStatus = "Data imported Successfully";
                                }
                            }

                            catch (Exception ex)
                            {
                                data.stuStatus = "Data is not in correct format";

                            }
                        }
                        if (data.SMS == "1")
                        {
                            try
                            {
                                int y = 0;
                                string msg = "";
                                string msg1 = "";

                                var admConfig = _FeeGroupContext.AdmissionStandardDMO.Single(t => t.MI_Id == data.MI_Id);

                                var studDet = _FeeGroupContext.AdmissionStudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_AdmNo == admno).ToList();
                                
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MobileNo) != null)
                                {
                                    y = y + 1;
                                    SMS sms = new SMS(_dbsms);
                                    string s = sms.sendSms(data.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, "ECS_Import", studDet.FirstOrDefault().AMST_Id).Result;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if(data.Email=="1")
                        {
                            try
                            {
                                int y = 0;
                                string msg = "";
                                string msg1 = "";

                                var admConfig = _FeeGroupContext.AdmissionStandardDMO.Single(t => t.MI_Id == data.MI_Id);

                                var studDet = _FeeGroupContext.AdmissionStudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_AdmNo == admno).ToList();
                             var institutionName = _FeeGroupContext.master_institution.Where(h => h.MI_Id == data.MI_Id).ToList();

                                var template = _FeeGroupContext.sMSEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "ECS_Import" && e.ISES_MailActiveFlag == true).ToList();
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_emailId) != null)
                                {
                                    y = y + 1;
                                    Email Email = new Email(_dbsms);
                                    string m = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMST_emailId, "ECS_Import", studDet.FirstOrDefault().AMST_Id);
                                }

                                //string Email_subject = "Attachment File";
                                //var message = new SendGrid.SendGridMessage();
                                //string Mailmsg = "Test";
                                //List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                                //alldetails = _FeeGroupContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();

                                //if (alldetails.Count > 0)
                                //{
                                //    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                                //    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                                //    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                                //    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                                //    string Subject = Email_subject;
                                //    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                                //    {
                                //        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                                //    }


                                //    //Sending mail using SendGrid API.
                                //    //Date:07-02-2017.

                                //    //var message = new SendGrid.SendGridMessage();
                                //    message.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                //    message.Subject = Subject;

                                //    message.AddTo(studDet.FirstOrDefault().AMST_emailId);

                                //    var img = _FeeGroupContext.COE_Events_ImagesDMO.Where(w => w.COEE_Id == 3).ToList();
                                //    if (img.Count > 0)
                                //    {
                                //        for (int j = 0; j < img.Count; j++)
                                //        {
                                //            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                                //            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                //            Stream stream = response.GetResponseStream();
                                //            message.AddAttachment(stream, "mail.pdf");
                                //        }
                                //    }

                                //}


                                //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                                //{
                                //    message.Html = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                                //}
                                //else
                                //{

                                //    // message.Html = "HAPPY BIRTHDAY DEAR" + " " + name + "<br/> No Template Found";
                                //}


                                //if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                                //{
                                //    var client = new Web(alldetails.FirstOrDefault().IVRM_sendgridkey);
                                //    client.DeliverAsync(message).Wait();

                                //}
                                //else
                                //{
                                //    //return "Sendgrid key is not available";
                                //}




                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {

                        admtot += admno + " , ";
                        admtot = admtot.Substring(0, (admtot.Length - 1));
                        data.stuStatus = "Data is not imported for admission nos , " + admtot.TrimEnd().TrimStart();
                    }
                }
                        catch (Exception ex)
                        {

                             Console.WriteLine(ex.Message);
                         }
                }
            return data;
        }
        


    }
}
