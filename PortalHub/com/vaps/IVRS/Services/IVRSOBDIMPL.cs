using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.IVRS;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class IVRSOBDIMPL : Interfaces.IVRSOBDInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;
        public IVRSOBDIMPL(DomainModelMsSqlServerContext db, PortalContext ivrs)
        {
            _db = db;
            _ivrs = ivrs;
        }
        public IVRSOBD getdetails(IVRSOBD data)
        {
            data.clas_list = _ivrs.School_M_Class.Where(f => f.MI_Id == data.MI_ID && f.ASMCL_ActiveFlag == true).ToArray();
            data.sect_list = _ivrs.School_M_Section.Where(f => f.MI_Id == data.MI_ID && f.ASMC_ActiveFlag == 1).ToArray();
            data.acs_lst = _ivrs.AcademicYearDMO.Where(f => f.MI_Id == data.MI_ID && f.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToArray();
            data.schoolname = _ivrs.Institution_master.Where(g => g.MI_Id == data.MI_ID && g.MI_ActiveFlag == 1).Select(b => b.MI_Name).FirstOrDefault();
            return data;
        }

        public IVRSOBD initiatecalls(IVRSOBD data)
        {
            try
            {
                for (int i = 0; i < data.selected_list.Length; i++)
                {
                    string URL = "https://api-voice.kaleyra.com/v1/?api_key=Afb8af1f8c1ee8fc9af6fff35fce864d2&method=voice.call&format=json&numbers=" + data.selected_list[i].IVRS_MobileNo + "&play=ivr:" + data.ivrid + "callback = http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                    var client = new RestClient("https://api-voice.kaleyra.com/v1/?api_key=Afb8af1f8c1ee8fc9af6fff35fce864d2&method=voice.call&format=json&numbers=" + data.selected_list[i].IVRS_MobileNo + "&play=ivr:" + data.ivrid + "&callback=http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    JObject root2 = JObject.Parse(response.Content);
                    string status = (String)root2["status"];
                    string message = (String)root2["message"];
                    if(status=="200")
                    {
                        string uniqueid = (String)root2["data"]["id"];
                    }
                    
                    if(status=="200" && message=="OK")
                    {
                        data.returnMsg = "sucess";
                    }
                    else
                    {
                        data.returnMsg = "Failure";
                    }

                    //var outputval = _ivrs.Database.ExecuteSqlCommand("Insert_IVRS_Logs @p0,@p1,@p2,@p3,@p4", data.MI_ID, status, message, uniqueid, URL);

                    //var urlget = _ivrs.IVRM_IVRS_ConfigurationDMO.Where(t => t.IIVRSC_MI_Id == data.MI_ID && t.IIVRSC_VirtualNo == data.IMCS_VirtualNo).Select(d => d.IIVRSC_URL).FirstOrDefault();

                    //Dictionary<string, object> dict = new Dictionary<string, object>();

                    //IVRS_Call_DetailsDMO cald = new IVRS_Call_DetailsDMO();
                    //cald.IMCD_VirtualNo = data.IMCS_VirtualNo;
                    //cald.IMCD_MI_Id = data.MI_ID;
                    //cald.IMCD_SchoolName = data.schoolname;
                    //cald.IMCD_URL = urlget;
                    //cald.IMCD_MobileNo = data.AMST_MobileNo;
                    //cald.IMCD_InOutFlg = data.IMCD_InOutFlg;
                    //cald.IMCD_DateTime = DateTime.Now;
                    //cald.IMCD_CallStatus = data.IMCD_CallStatus;
                    //cald.IMCD_CallDuration = data.IMCD_CallDuration;
                    //cald.IMCD_PulseCount = data.IMCD_PulseCount;
                    //cald.IMCD_IVRSText = "";
                    //cald.IMCD_IVRSVoiceURL = "";
                    //cald.IMCD_CreatedBy = 1;
                    //cald.IMCD_UpdatedBy = 1;
                    //cald.IMCD_ActiveFlg = true;
                    //cald.CreatedDate = DateTime.Now;
                    //cald.UpdatedDate = DateTime.Now;
                    //_ivrs.Add(cald);
                    //var contactExists1 = _ivrs.SaveChanges();
                    //if (contactExists1 == 1)
                    //{
                    //    dict.Add("Message", "sucess");
                    //    data.returnMsg = "sucess";
                    //}
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRSOBD initiatecallsmobiglitz(IVRSOBD data)
        {
            try
            {
                for (int i = 0; i < data.selected_list.Length; i++)
                {
                  //  string URL = "https://api-voice.kaleyra.com/v1/?api_key=Afb8af1f8c1ee8fc9af6fff35fce864d2&method=voice.call&format=json&numbers=" + data.selected_list[i].IVRS_MobileNo + "&play=ivr:" + data.ivrid + "callback = http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                    var client = new RestClient("http://bulk.mobiglitz.com/voice/voice_upload.php?username=demo&api_password=8a75bjifry5wgdinm&voice_id=" + data.ivrid + "&to="+ data.selected_list[i].IVRS_MobileNo);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.StatusDescription);
                    //var Data = JsonConvert.DeserializeObject<data>(result.Content);
                    string content = response.Content;
                    string status = response.StatusCode.ToString();

                    //string content = JObject.Parse(response.Content).ToString();
                    //string status = JObject.Parse(response.StatusCode).ToString();
                    //string message = (String)root2["message"];
                    //if (status == "200")
                    //{
                    //    string uniqueid = (String)root2["data"]["id"];
                    //}

                    if (status == "OK" || status == "Ok")
                    {
                        data.returnMsg = "sucess";
                    }
                    else
                    {
                        data.returnMsg = "Failure";
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRSOBD ivrgetstudetails(IVRSOBD data)
        {
            try
            {
                if (data.ASMS_ID > 0)
                {
                    data.maindata = (from a in _ivrs.AdmissionStudentDMO
                                     from b in _ivrs.School_Adm_Y_StudentDMO
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.ASMAY_Id == data.ASMAY_ID && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id == data.ASMS_ID)
                                     select new IVRSOBD
                                     {
                                         AMST_Id = a.AMST_Id,
                                         studentName = a.AMST_FirstName,
                                         AMST_AdmNo = a.AMST_AdmNo,
                                         AMST_emailId = a.AMST_emailId,
                                         AMST_MobileNo = a.AMST_MobileNo
                                     }).ToArray();
                }
                else
                {
                    data.maindata = (from a in _ivrs.AdmissionStudentDMO
                                     from b in _ivrs.School_Adm_Y_StudentDMO
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.ASMAY_Id == data.ASMAY_ID && b.ASMCL_Id == data.ASMCL_ID)
                                     select new IVRSOBD
                                     {
                                         AMST_Id = a.AMST_Id,
                                         studentName = a.AMST_FirstName,
                                         AMST_AdmNo = a.AMST_AdmNo,
                                         AMST_emailId = a.AMST_emailId,
                                         AMST_MobileNo = a.AMST_MobileNo
                                     }).ToArray();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}
