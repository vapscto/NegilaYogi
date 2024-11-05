using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class HRResumeUploadService : Interfaces.HRResumeUploadInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HRResumeUploadService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Resume_UploadDTO SaveUpdate(HR_Resume_UploadDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Resume_UploadDMO dmoObj = Mapper.Map<HR_Resume_UploadDMO>(dto);

                var duplicatecountresult = _HRMSContext.HR_Resume_UploadDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRRUP_PersonName.Equals(dto.HRRUP_PersonName) && t.HRRUP_PhoneNo.Equals(dto.HRRUP_PhoneNo) && t.HRRUP_EmailId == dto.HRRUP_EmailId).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRRUP_Id > 0)
                    {
                        var QulaificationName = _HRMSContext.HR_Resume_UploadDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRRUP_PersonName.Equals(dto.HRRUP_PersonName)).Count();
                        if (QulaificationName == 0)
                        {
                            var result = _HRMSContext.HR_Resume_UploadDMO.Single(t => t.HRRUP_Id == dmoObj.HRRUP_Id);
                            result.HRRUP_Date = DateTime.Now;
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }
                    else
                    {
                        var QulaificationName = _HRMSContext.HR_Resume_UploadDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRRUP_PersonName.Equals(dto.HRRUP_PersonName)).Count();
                        if (QulaificationName == 0)
                        {
                            dmoObj.HRRUP_ActiveFlag = true;
                            dmoObj.HRRUP_PersonName = dto.HRRUP_PersonName;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRRUP_PhoneNo = dto.HRRUP_PhoneNo;
                            dmoObj.HRRUP_EmailId = dto.HRRUP_EmailId;
                            dmoObj.HRRUP_Qualification = dto.HRRUP_Qualification;
                            dmoObj.HRRUP_Experience = dto.HRRUP_Experience;
                            dmoObj.HRRUP_Designation = dto.HRRUP_Designation;
                            dmoObj.HRRUP_DocName = dto.HRRUP_DocName;
                            dmoObj.HRRUP_DocPath = dto.HRRUP_DocPath;
                            dmoObj.HRRUP_Date = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "AllDuplicate";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Resume_UploadDTO AdmissionEnquirymail(HR_Resume_UploadDTO data)
        {
            //HR_Resume_UploadDTO obj = new HR_Resume_UploadDTO();
            string str = "";
            try
            {
                str += "<p> <span style='font-weight:bold'>Admission Enquiry Details</span></p><p><span style='float: center;padding-left:30%;'></span></p> <table width='30%' id = 'tablepaging' class='yui' style='border-collapse: collapse; width: 30%;'><tr> <td style = 'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'>NAME : </td><td align=left style='font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'>" + data.HRRUP_PersonName + "</td></tr><tr> <td style = 'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;' > EMAIL : </td><td align = left style='font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'>" + data.HRRUP_EmailId + "</td></tr><tr> <td style = 'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'>PHONE : </td><td align=left style='font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'>" + data.HRRUP_PhoneNo + "</td></tr><tr> <td style='border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'>CLASS : </td><td align=left style='font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'>" + data.Class + "</td></tr><tr> <td style='border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'>SUBJECT : </td><td align = leftstyle='fontsize:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'>" + data.Subject + "</td></tr></table> ";

                SendEmail(data.HRRUP_EmailId, "Admission Enquiry", str, data.MI_Id);
                data.retrunMsg = "Email sent Successfully.";
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
                data.retrunMsg = "Something went wrong !!";
            }
            return data;
        }

        public void SendEmail(string mailid, string subject, string body, long id)
        {
            //mailid = "goutamkumar@vapstech.com";
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("AdmissionEnquiry", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _HRMSContext.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",SqlDbType.VarChar)
                        {
                            Value = "Staff"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            //return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
