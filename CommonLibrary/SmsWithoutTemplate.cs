using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class SmsWithoutTemplate
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public SmsWithoutTemplate(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public async Task<string> sendSms(long MI_Id, long mobileNo, string userName, string pwd)
        {

            try
            {
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", "Login Credential.....Your UserName is: " + userName + " and" + " Password Is: " + pwd);

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();
                return "Success";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        public async Task<string> sendsmsfromPortal(long MI_Id, long mobileNo, string message)
        {

            try
            {
                message = message.Replace("&", "%26");
                message = message.Replace("#", "%23");
                message = message.Replace("'", "%27");
                message = message.Replace("$", "%24");
                message = message.Replace("+", "%2B");
                message = message.Replace("@", "%40");
                message = message.Replace("?", "%3F");
                message = message.Replace(">", "%3E");
                message = message.Replace("=", "%3D");
                message = message.Replace(";", "%3B");
                message = message.Replace("<", "%3C");

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();


             

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", message);

                if (url.Contains("entityid"))
                {
                    url = url.Replace("&entity_id=entityid&template_id=templateid", "");   
                }
            

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();

                if (responseparameters != null)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

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
                            Value = message
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "GENERAL SMS&EMAIL"
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
                            Value = responseparameters
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
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
        public async Task<string> sendsmsfromPortal_new(long MI_Id, long mobileNo, string message, string Template, string module_name)
        {

            try
            {
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
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

                if (Template == "PRINCIPAL" || Template == "MANAGER")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, message.ToString());
                    sms = result;
                }

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", sms);

                url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);


                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();

                if (responseparameters != null)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

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
                            Value = module_name
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
                            Value = responseparameters
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
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
        public async Task<string> SendwhatsappfromPortal(long MI_Id, long mobileNo, string message, string whatsappfile, string whatsapp_filetype)
        {
            try
            {
                string s = "";

                message = message.Replace("&", "%26");
                message = message.Replace("#", "%23");
                message = message.Replace("'", "%27");
                message = message.Replace("$", "%24");
                message = message.Replace("+", "%2B");
                message = message.Replace("@", "%40");
                message = message.Replace("?", "%3F");
                message = message.Replace(">", "%3E");
                message = message.Replace("=", "%3D");
                message = message.Replace(";", "%3B");
                message = message.Replace("<", "%3C");

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                #region WhatsApp Text Message
                string url = institutionName[0].MI_WhatsAppTextUrl.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", message);    
                
                try
                {
                    if(whatsappfile==null || whatsappfile == "")
                    {
                        s = await Store_WhatsApp_Sent_Details(MI_Id, "General WhatsApp", PHNO, message, url,"text");
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                #endregion

                if (whatsappfile !=null && whatsappfile != "")
                {
                    #region Whats PDF File Attach
                    if (whatsapp_filetype.ToUpper() == "PDF")
                    {
                        string url_pdf = institutionName[0].MI_WhatsAppPdfUrl.ToString();

                        url_pdf = url_pdf.Replace("PHNO", PHNO);

                        url_pdf = url_pdf.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_pdf,"pdf");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region WhatsApp Image File Attach
                    if (whatsapp_filetype.ToUpper() == "JPG" || whatsapp_filetype.ToUpper() == "PNG")
                    {
                        string url_image = institutionName[0].MI_WhatsAppImageUrl.ToString();

                        url_image = url_image.Replace("PHNO", PHNO);

                        url_image = url_image.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_image,"image");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region WhatsApp Audio File Attach
                    if (whatsapp_filetype.ToUpper() == "MP3")
                    {
                        string url_audio = institutionName[0].MI_WhatsAppAudioUrl.ToString();

                        url_audio = url_audio.Replace("PHNO", PHNO);

                        url_audio = url_audio.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_audio,"audio");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region WhatsApp Video File Attach
                    if (whatsapp_filetype.ToUpper() == "MP4" || whatsapp_filetype.ToUpper() == "WMV")
                    {
                        string url_video = institutionName[0].MI_WhatsAppVideoUrl.ToString();

                        url_video = url_video.Replace("PHNO", PHNO);

                        url_video = url_video.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_video,"video");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }

        public async Task<string> Send_StudentIllness_Whatsapp(long MI_Id, long mobileNo, string message, string whatsappfile, string whatsapp_filetype, 
            string Template , long HMTILL_Id, long AMST_Id)
        {
            try
            {
                string s = "";

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
                    //result = sms.Replace(ParamaetersName[0].ISMP_NAME, HMTILL_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER_STUDENT_ILLNESS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HMTILL_Id", SqlDbType.BigInt) { Value = HMTILL_Id });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar) { Value = Template });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = AMST_Id });

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
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();        

                #region WhatsApp Text Message
                string url = institutionName[0].MI_WhatsAppTextUrl.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", sms);

                try
                {
                    if (whatsappfile == null || whatsappfile == "")
                    {
                        s = await Store_WhatsApp_Sent_Details(MI_Id, "StudentIllness_Whatsapp", PHNO, sms, url, "text");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                #endregion

                if (whatsappfile != null && whatsappfile != "")
                {
                    #region Whats PDF File Attach
                    if (whatsapp_filetype.ToUpper() == "PDF")
                    {
                        string url_pdf = institutionName[0].MI_WhatsAppPdfUrl.ToString();

                        url_pdf = url_pdf.Replace("PHNO", PHNO);

                        url_pdf = url_pdf.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_pdf,"pdf");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region WhatsApp Image File Attach
                    if (whatsapp_filetype.ToUpper() == "JPG" || whatsapp_filetype.ToUpper() == "PNG")
                    {
                        string url_image = institutionName[0].MI_WhatsAppImageUrl.ToString();

                        url_image = url_image.Replace("PHNO", PHNO);

                        url_image = url_image.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_image,"image");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region WhatsApp Audio File Attach
                    if (whatsapp_filetype.ToUpper() == "MP3")
                    {
                        string url_audio = institutionName[0].MI_WhatsAppAudioUrl.ToString();

                        url_audio = url_audio.Replace("PHNO", PHNO);

                        url_audio = url_audio.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_audio,"audio");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region WhatsApp Video File Attach
                    if (whatsapp_filetype.ToUpper() == "MP4" || whatsapp_filetype.ToUpper() == "WMV")
                    {
                        string url_video = institutionName[0].MI_WhatsAppVideoUrl.ToString();

                        url_video = url_video.Replace("PHNO", PHNO);

                        url_video = url_video.Replace("MESSAGE", whatsappfile);

                        try
                        {
                            s = await Store_WhatsApp_Sent_Details(MI_Id, whatsappfile, PHNO, message, url_video,"video");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
        public async Task<string> Store_WhatsApp_Sent_Details(long MI_Id, string Template, string PHNO, string sms,  string url,string type)
        {
            try
            {
                //System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                //System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                //Stream stream = response.GetResponseStream();
                //StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                //string responseparameters = readStream.ReadToEnd();

                Dictionary<String, object> paytmParams = new Dictionary<String, object>();

                paytmParams.Add("mobile", PHNO);

                paytmParams.Add("message", sms);

                if (type == "image")
                {
                    paytmParams.Add("imgurl", Template);
                }
                else if (type == "pdf")
                {
                    paytmParams.Add("pdfurl", Template);
                }
                else if (type == "video")
                {
                    paytmParams.Add("vidUrl", Template);
                }



                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.Accept = "application/json";
                connection.Headers.Add("Authorization", "Bearer " + "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiOWE0YmQyYjQ0MTY1YmI3NmE2YzdlMzBiOGI3MDk2OThjNWI0MTJhNjZkMmJhNTI0YTgwZGMwZjFmMDljODA5MjYyZGI4NGUxNmM3MDk5OWYiLCJpYXQiOjE2NjY5Mzc1ODIuMjEzNTE4LCJuYmYiOjE2NjY5Mzc1ODIuMjEzNTIxLCJleHAiOjE2OTg0NzM1ODIuMjA2NTY3LCJzdWIiOiI2OSIsInNjb3BlcyI6W119.AtIoOWTm8LlA38zMqbuj_895-qvVLeCHKtxHycrPhFdIzOs4E17_IAkvWlJyu-nYJLueCcP4BjUzaK1_iTeGcnxMOJTcz--NhCHcI9-5MbKGjAPJum3rz8ZpyCQs8knQUvaA9mbfLWnncFTlWRdrMWTMIeo-dF6xsRbVJaqEFRyNDip414KGZq2_9aI0qKGlnOmO6X2rHwkUKPM7iWfJSRdZswCyNhAuGkYm-GWCGVnk406xVJvnMBUIbwYhg86oWwEkHBHI3bCl2vpIXR3LlhrcDjehTwpRQRMbaglMA_elzOHeKugZxW2B-XveidvSxU1DKlX8ZDf-1QNsmXfhUWPL0R4qiY9y8VUhaif3b0L0aQXpsKayqocXUxCqgpr45SE82IAq-brgg90LX5nHNybZK1cepZY0lWFJM0uRPmgGkdYFoM3ul6paJhdAUBegFMwQB-XxYIJUIhv2J7uMwEf_ukri-cHWvoIDC9MH1ILGP6qb9DMnomv2S_QTT5kPNWj-SXP8ganh8YApkWnZPqSuxGGR2yt_rPlEzfvmmam2Nc3ZirBGhsXr-5L5N5UVYvGTB1T_YlVIpx9IaozIhIiri80b1oRe4X4M11_tNJiKDsdz7n5wQ-SpW9OL4kCKLSOD_ygqxQ42E1CwYgXZ-VwF1NrPPUSkzG2q35utF0c");

                connection.Method = "POST";
                // connection.DefaultRequestHeaders ["Bearer Token"] = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiOWE0YmQyYjQ0MTY1YmI3NmE2YzdlMzBiOGI3MDk2OThjNWI0MTJhNjZkMmJhNTI0YTgwZGMwZjFmMDljODA5MjYyZGI4NGUxNmM3MDk5OWYiLCJpYXQiOjE2NjY5Mzc1ODIuMjEzNTE4LCJuYmYiOjE2NjY5Mzc1ODIuMjEzNTIxLCJleHAiOjE2OTg0NzM1ODIuMjA2NTY3LCJzdWIiOiI2OSIsInNjb3BlcyI6W119.AtIoOWTm8LlA38zMqbuj_895-qvVLeCHKtxHycrPhFdIzOs4E17_IAkvWlJyu-nYJLueCcP4BjUzaK1_iTeGcnxMOJTcz--NhCHcI9-5MbKGjAPJum3rz8ZpyCQs8knQUvaA9mbfLWnncFTlWRdrMWTMIeo-dF6xsRbVJaqEFRyNDip414KGZq2_9aI0qKGlnOmO6X2rHwkUKPM7iWfJSRdZswCyNhAuGkYm-GWCGVnk406xVJvnMBUIbwYhg86oWwEkHBHI3bCl2vpIXR3LlhrcDjehTwpRQRMbaglMA_elzOHeKugZxW2B-XveidvSxU1DKlX8ZDf-1QNsmXfhUWPL0R4qiY9y8VUhaif3b0L0aQXpsKayqocXUxCqgpr45SE82IAq-brgg90LX5nHNybZK1cepZY0lWFJM0uRPmgGkdYFoM3ul6paJhdAUBegFMwQB-XxYIJUIhv2J7uMwEf_ukri-cHWvoIDC9MH1ILGP6qb9DMnomv2S_QTT5kPNWj-SXP8ganh8YApkWnZPqSuxGGR2yt_rPlEzfvmmam2Nc3ZirBGhsXr-5L5N5UVYvGTB1T_YlVIpx9IaozIhIiri80b1oRe4X4M11_tNJiKDsdz7n5wQ-SpW9OL4kCKLSOD_ygqxQ42E1CwYgXZ-VwF1NrPPUSkzG2q35utF0c";


                //connection.Headers["x-client-secret"] = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();
                //connection.Headers["x-client-id"] = paymentdetails.FirstOrDefault().FPGD_SaltKey.ToString();


                var myContent = JsonConvert.SerializeObject(paytmParams);
                String postData = myContent;



                using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                }

                string responseData = string.Empty;


                using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();

                    JObject joResponse1 = JObject.Parse(responseData);

                }


                //var client = new RestClient(url);
                //client.Timeout = -1;
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiOWE0YmQyYjQ0MTY1YmI3NmE2YzdlMzBiOGI3MDk2OThjNWI0MTJhNjZkMmJhNTI0YTgwZGMwZjFmMDljODA5MjYyZGI4NGUxNmM3MDk5OWYiLCJpYXQiOjE2NjY5Mzc1ODIuMjEzNTE4LCJuYmYiOjE2NjY5Mzc1ODIuMjEzNTIxLCJleHAiOjE2OTg0NzM1ODIuMjA2NTY3LCJzdWIiOiI2OSIsInNjb3BlcyI6W119.AtIoOWTm8LlA38zMqbuj_895-qvVLeCHKtxHycrPhFdIzOs4E17_IAkvWlJyu-nYJLueCcP4BjUzaK1_iTeGcnxMOJTcz--NhCHcI9-5MbKGjAPJum3rz8ZpyCQs8knQUvaA9mbfLWnncFTlWRdrMWTMIeo-dF6xsRbVJaqEFRyNDip414KGZq2_9aI0qKGlnOmO6X2rHwkUKPM7iWfJSRdZswCyNhAuGkYm-GWCGVnk406xVJvnMBUIbwYhg86oWwEkHBHI3bCl2vpIXR3LlhrcDjehTwpRQRMbaglMA_elzOHeKugZxW2B-XveidvSxU1DKlX8ZDf-1QNsmXfhUWPL0R4qiY9y8VUhaif3b0L0aQXpsKayqocXUxCqgpr45SE82IAq-brgg90LX5nHNybZK1cepZY0lWFJM0uRPmgGkdYFoM3ul6paJhdAUBegFMwQB-XxYIJUIhv2J7uMwEf_ukri-cHWvoIDC9MH1ILGP6qb9DMnomv2S_QTT5kPNWj-SXP8ganh8YApkWnZPqSuxGGR2yt_rPlEzfvmmam2Nc3ZirBGhsXr-5L5N5UVYvGTB1T_YlVIpx9IaozIhIiri80b1oRe4X4M11_tNJiKDsdz7n5wQ-SpW9OL4kCKLSOD_ygqxQ42E1CwYgXZ-VwF1NrPPUSkzG2q35utF0c");
                //request.AlwaysMultipartFormData = true;
                //request.AddParameter("mobile", "9513295108");
                //request.AddParameter("message", "Hi");
                //IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);


                // if (responseparameters != null)
                if (responseData != null)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.NVarChar) { Value = PHNO });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar) { Value = sms });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar) { Value = Template });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = "Delivered" });
                        cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar) { Value = responseData });
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
            }
            return "";
        }
    }
}

