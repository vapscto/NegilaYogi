using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using System.IO;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class SmsEmailSettingImpl : Interfaces.SmsEmailSettingInterface
    {
        private static ConcurrentDictionary<string, SMSEmailSettingDTO> _login =
           new ConcurrentDictionary<string, SMSEmailSettingDTO>();

        public DomainModelMsSqlServerContext _AcademicContext;

        public SmsEmailSettingImpl(DomainModelMsSqlServerContext academiccontext)
        {
            _AcademicContext = academiccontext;
        }
        public SMSEmailSettingDTO getallDetails(SMSEmailSettingDTO data)
        {
            try
            {

                //to get All Institution Module Dropdown.

                data.institutionModuleList = (from a in _AcademicContext.Institution_Module
                                               from b in _AcademicContext.masterModule
                                               where (a.IVRMM_Id == b.IVRMM_Id && a.MI_Id == data.MI_Id)
                                               select new SMSEmailSettingDTO { IVRMIM_Id = a.IVRMIM_Id, IVRMM_ModuleName = b.IVRMM_ModuleName }
                                    ).ToArray();


                //to get All Institution  ModulePage Dropdown.
                data.institutionPageList = (from a in _AcademicContext.Institution_Module_Page
                                             from b in _AcademicContext.masterPage
                                             where (a.IVRMP_Id == b.IVRMP_Id)
                                             select new SMSEmailSettingDTO { IVRMIMP_Id = a.IVRMIMP_Id, IVRMMP_PageName = b.IVRMMP_PageName }
                                    ).ToArray();

                //to get All ModulePage header Dropdown.
                List<SmsEmailHeader> pageWiseHeader = new List<SmsEmailHeader>();
                pageWiseHeader = _AcademicContext.smsEmailHeader.Where(p => p.MI_Id == data.MI_Id).ToList();
                data.pageWiseHeaderList = pageWiseHeader.ToArray();




                data.emailtemplatelist = _AcademicContext.IVRM_Master_HTMLTemplatesDMO.Where(e => e.MI_Id == data.MI_Id && e.ISMHTML_ActiveFlg == true).OrderBy(e => e.ISMHTML_HTMLName).Distinct().ToArray();

                data.rolelist = _AcademicContext.MasterRoleType.OrderBy(a => a.IVRMRT_Role).Distinct().ToArray();



                data.emailSmsSettingList = (from m in _AcademicContext.smsEmailSetting
                                           //  from n in _AcademicContext.SMS_MAIL_SAVED_PARAMETER_DMO
                                             where (
                                             //m.ISES_Id == n.ISES_Id &&
                                             m.MI_Id == data.MI_Id)
                                             group m by m.ISES_Id into g
                                             select new SmsEmailDTO
                                             {
                                                 ISES_Id = g.FirstOrDefault().ISES_Id,
                                                 ISES_MailActiveFlag = g.FirstOrDefault().ISES_MailActiveFlag,
                                                 ISES_MailBody = g.FirstOrDefault().ISES_MailBody,
                                                 ISES_MailFooter = g.FirstOrDefault().ISES_MailFooter,
                                                 ISES_MailHTMLTemplate = g.FirstOrDefault().ISES_MailHTMLTemplate,
                                                 ISES_MailSubject = g.FirstOrDefault().ISES_MailSubject,
                                                 ISES_Mail_Message = g.FirstOrDefault().ISES_Mail_Message,
                                                 ISES_SMSActiveFlag = g.FirstOrDefault().ISES_SMSActiveFlag,
                                                 ISES_SMSMessage = g.FirstOrDefault().ISES_SMSMessage,
                                                 ISES_Template_Name = g.FirstOrDefault().ISES_Template_Name,
                                                 IVRMIMP_Id = g.FirstOrDefault().IVRMIMP_Id,
                                                 IVRMIM_Id = g.FirstOrDefault().IVRMIM_Id,
                                                 IVRMSTAUL_Id = g.FirstOrDefault().IVRMSTAUL_Id,
                                                 MI_Id = g.FirstOrDefault().MI_Id

                                             }).Distinct().OrderByDescending(w=>w.ISES_Id).ToArray();



                if (data.emailSmsSettingList.Length > 0)
                {
                    data.count = data.emailSmsSettingList.Length;
                }
                else
                {
                    data.count = 0;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public SMSEmailSettingDTO getmodulePage(SMSEmailSettingDTO page)
        {
            try
            {

                page.institutionPageList = (from a in _AcademicContext.Institution_Module_Page
                                            from b in _AcademicContext.masterPage
                                            where (a.IVRMP_Id == b.IVRMP_Id && a.IVRMIM_Id == page.IVRMIM_Id)
                                            select new SMSEmailSettingDTO { IVRMIMP_Id = a.IVRMIMP_Id, IVRMMP_PageName = b.IVRMMP_PageName }
                                    ).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public SMSEmailSettingDTO getHeader(SMSEmailSettingDTO header)
        {
            try
            {
                List<SmsEmailHeader> pageWiseHeader = new List<SmsEmailHeader>();
                pageWiseHeader = _AcademicContext.smsEmailHeader.Where(p => p.MI_Id == header.MI_Id && p.IVRMIM_Id == header.IVRMIM_Id && p.IVRMIMP_Id == header.IVRMIMP_Id).ToList();
                header.pageWiseHeaderList = pageWiseHeader.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return header;
        }
        public SmsEmailDTO save(SmsEmailDTO data)
        {
            long id = Convert.ToInt64(data.ISES_Template_Name);
            var isesTemplateName = _AcademicContext.smsEmailHeader.Where(h => h.IVRMHE_Id == id).FirstOrDefault().IVRMHE_Name;
            try
            {

                if (data.ISES_Id == 0)
                {
                    var dupcheck = _AcademicContext.smsEmailSetting.Where(q => q.MI_Id == data.MI_Id && q.ISES_Template_Name.ToLower().Trim() == isesTemplateName.ToLower().Trim()).ToList();
                    if (dupcheck.Count>0)
                    {
                        data.message = "dup";
                    }
                    else
                    {
                        SMSEmailSetting obj = new SMSEmailSetting();
                        obj.MI_Id = data.MI_Id;
                        obj.IVRMIM_Id = data.IVRMIM_Id;
                        obj.ISES_Template_Name = isesTemplateName;
                        obj.ISES_SMSMessage = data.ISES_SMSMessage;
                        obj.ISES_SMSActiveFlag = data.ISES_SMSActiveFlag;
                        obj.ISES_MailSubject = data.ISES_MailSubject;
                        obj.ISES_MailFooter = data.ISES_MailFooter;
                        obj.ISES_IVRSTextMsg = data.ISES_IVRSTextMsg;
                        obj.ISES_PNMessage = data.ISES_PNMessage;
                        obj.ISES_AlertBeforeDays = data.ISES_AlertBeforeDays;
                        obj.ISES_IVRSVoiceFile = data.ISES_IVRSVoiceFile;
                        obj.ISES_TemplateId = data.ISES_TemplateId;
                        if (data.ISES_PNActiveFlg == null)
                        {
                            data.ISES_PNActiveFlg = false;
                        }
                        obj.ISES_PNActiveFlg = data.ISES_PNActiveFlg;

                        if (data.ISMHTML_Id>0)
                        {
                            var emailtemplate = _AcademicContext.IVRM_Master_HTMLTemplatesDMO.Where(h => h.ISMHTML_Id == data.ISMHTML_Id).FirstOrDefault().ISMHTML_HTMLTemplate;
                            obj.ISES_MailBody = emailtemplate;
                            obj.ISES_Mail_Message = emailtemplate;
                            obj.ISES_MailHTMLTemplate = emailtemplate;

                        }
                        obj.ISES_MailActiveFlag = data.ISES_MailActiveFlag;
                        obj.IVRMSTAUL_Id = data.IVRMSTAUL_Id;
                        obj.IVRMIMP_Id = data.IVRMIMP_Id;
                        obj.ISES_EnableSMSCCFlg = data.ISES_EnableSMSCCFlg;
                        obj.ISES_EnableMailCCFlg = data.ISES_EnableMailCCFlg;
                        obj.ISES_EnableMailBCCFlg = data.ISES_EnableMailBCCFlg;

                        


                        if (data.mobile_list_dto.Length>0)
                        {
                            string mob = "";
                            for (int i = 0; i < data.mobile_list_dto.Length; i++)
                            {
                                if (i == 0)
                                {
                                    mob = data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                                }
                                else
                                {
                                    mob = mob + "," + data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                                }
                            }


                            obj.ISES_SMSCCMobileNo = mob;
                        }


                        if (data.email_list_dto.Length > 0)
                        {
                            string eml = "";
                            for (int i = 0; i < data.email_list_dto.Length; i++)
                            {
                                if (i == 0)
                                {
                                    eml = data.email_list_dto[i].HRMEM_EmailId.ToString();
                                }
                                else
                                {
                                    eml = eml + "," + data.email_list_dto[i].HRMEM_EmailId.ToString();
                                }
                            }
                            
                            obj.ISES_MailCCId = eml;
                        }
                        if (data.email_list_dtocc.Length > 0)
                        {
                            string eml1 = "";
                            for (int i = 0; i < data.email_list_dtocc.Length; i++)
                            {
                                if (i == 0)
                                {
                                    eml1 = data.email_list_dtocc[i].HRMEM_EmailId.ToString();
                                }
                                else
                                {
                                    eml1 = eml1 + "," + data.email_list_dtocc[i].HRMEM_EmailId.ToString();
                                }
                            }


                            obj.ISES_MailBCCId = eml1;
                        }

                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _AcademicContext.Add(obj);


                        if (data.roleids != null)
                        {
                            foreach (var item in data.roleids)
                            {
                                SMS_Email_Setting_RoleTypeDMO rt = new SMS_Email_Setting_RoleTypeDMO();
                                rt.ISES_Id = obj.ISES_Id;
                                rt.IVRMRT_Id = item.IVRMRT_Id;
                                rt.ISESRT_ActiveFlg = true;
                                rt.ISESRT_CreatedBy = data.IVRMSTAUL_Id;
                                rt.ISESRT_UpdatedBy = data.IVRMSTAUL_Id;
                                rt.ISESRT_CreatedDate = DateTime.Now;
                                rt.ISESRT_UpdatedDate = DateTime.Now;
                                _AcademicContext.Add(rt);
                            }
                        }


                        if (data.filelist.Length>0)
                        {

                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath!=null && item.cfilepath!="")
                                {
                                    IVRM_EMAIL_ATT_DMO f1 = new IVRM_EMAIL_ATT_DMO();
                                    f1.ISES_Id = obj.ISES_Id;
                                    f1.IVRM_Att_Name = item.cfilename;
                                    f1.IVRM_Att_Path = item.cfilepath;
                                    _AcademicContext.Add(f1);
                                }
                               
                            }
                        }



                      

                     var getparameter = (from a in _AcademicContext.SmsEmailParameterMappingDMO
                                            where a.IVRMHE_Id == id
                                            select new SmsEmailDTO
                                            {
                                                ISMP_ID=a.ISMP_ID

                                            }).Distinct().ToList();


                        if (getparameter.Count>0)
                        {
                            foreach (var pp in getparameter)
                            {
                                SMS_MAIL_SAVED_PARAMETER_DMO sp = new SMS_MAIL_SAVED_PARAMETER_DMO();
                                sp.ISMP_ID = pp.ISMP_ID;
                                sp.MI_Id = data.MI_Id;
                                sp.ISES_Id = obj.ISES_Id;
                                sp.Flag = "S";
                                sp.CreatedDate = DateTime.Now;
                                sp.UpdatedDate = DateTime.Now;
                                _AcademicContext.Add(sp);

                                SMS_MAIL_SAVED_PARAMETER_DMO sp1 = new SMS_MAIL_SAVED_PARAMETER_DMO();
                                sp1.ISMP_ID = pp.ISMP_ID;
                                sp1.MI_Id = data.MI_Id;
                                sp1.ISES_Id = obj.ISES_Id;
                                sp1.Flag = "M";
                                sp1.CreatedDate = DateTime.Now;
                                sp1.UpdatedDate = DateTime.Now;
                                _AcademicContext.Add(sp1);




                            }

                           
                        }



                        int res = _AcademicContext.SaveChanges();
                        if (res>0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                        
    }       }
                else
                {
                    
                        var dupcheck = _AcademicContext.smsEmailSetting.Where(q => q.MI_Id == data.MI_Id && q.ISES_Template_Name.ToLower().Trim() == isesTemplateName.ToLower().Trim() && q.ISES_Id!=data.ISES_Id).ToList();
                        if (dupcheck.Count > 0)
                        {
                            data.message = "dup";
                        }
                        else
                        {

                        var rest = _AcademicContext.smsEmailSetting.Single(a => a.ISES_Id == data.ISES_Id);
                            rest.MI_Id = data.MI_Id;
                            rest.IVRMIM_Id = data.IVRMIM_Id;
                            rest.ISES_Template_Name = isesTemplateName;
                            rest.ISES_SMSMessage = data.ISES_SMSMessage;
                            rest.ISES_SMSActiveFlag = data.ISES_SMSActiveFlag;
                            rest.ISES_MailSubject = data.ISES_MailSubject;
                            rest.ISES_MailFooter = data.ISES_MailFooter;
                            rest.ISES_IVRSVoiceFile = data.ISES_IVRSVoiceFile;
                            rest.ISES_TemplateId = data.ISES_TemplateId;

                            if (data.ISMHTML_Id > 0)
                            {
                                var emailtemplate = _AcademicContext.IVRM_Master_HTMLTemplatesDMO.Where(h => h.ISMHTML_Id == data.ISMHTML_Id).FirstOrDefault().ISMHTML_HTMLTemplate;
                                rest.ISES_MailBody = emailtemplate;
                                rest.ISES_Mail_Message = emailtemplate;
                                rest.ISES_MailHTMLTemplate = emailtemplate;

                            }
                           rest.ISES_MailActiveFlag = data.ISES_MailActiveFlag;
                           rest.IVRMSTAUL_Id = data.IVRMSTAUL_Id;
                           rest.IVRMIMP_Id = data.IVRMIMP_Id;
                           rest.ISES_EnableSMSCCFlg = data.ISES_EnableSMSCCFlg;
                           rest.ISES_EnableMailCCFlg = data.ISES_EnableMailCCFlg;
                           rest.ISES_EnableMailBCCFlg = data.ISES_EnableMailBCCFlg;
                           rest.ISES_IVRSTextMsg = data.ISES_IVRSTextMsg;
                           rest.ISES_PNMessage = data.ISES_PNMessage;
                           rest.ISES_AlertBeforeDays = data.ISES_AlertBeforeDays;
                        if (data.ISES_PNActiveFlg==null)
                        {
                            data.ISES_PNActiveFlg = false;
                        }
                           rest.ISES_PNActiveFlg = data.ISES_PNActiveFlg;
                      

                            if (data.mobile_list_dto.Length > 0)
                            {
                                string mob = "";
                                for (int i = 0; i < data.mobile_list_dto.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        mob = data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                                    }
                                    else
                                    {
                                        mob = mob + "," + data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                                    }
                                }


                            rest.ISES_SMSCCMobileNo = mob;
                            }
                        else
                        {
                            rest.ISES_SMSCCMobileNo = "";
                        }


                            if (data.email_list_dto.Length > 0)
                            {
                                string eml = "";
                                for (int i = 0; i < data.email_list_dto.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        eml = data.email_list_dto[i].HRMEM_EmailId.ToString();
                                    }
                                    else
                                    {
                                        eml = eml + "," + data.email_list_dto[i].HRMEM_EmailId.ToString();
                                    }
                                }

                            rest.ISES_MailCCId = eml;
                            }
                        else
                        {
                            rest.ISES_MailCCId = "";
                        }
                            if (data.email_list_dtocc.Length > 0)
                            {
                                string eml1 = "";
                                for (int i = 0; i < data.email_list_dtocc.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        eml1 = data.email_list_dtocc[i].HRMEM_EmailId.ToString();
                                    }
                                    else
                                    {
                                        eml1 = eml1 + "," + data.email_list_dtocc[i].HRMEM_EmailId.ToString();
                                    }
                                }


                            rest.ISES_MailBCCId = eml1;
                            }
                        else
                        {
                            rest.ISES_MailBCCId = "";
                        }


                        rest.UpdatedDate = DateTime.Now;

                            _AcademicContext.Update(rest);


                        var remfile = _AcademicContext.IVRM_EMAIL_ATT_DMO.Where(r => r.ISES_Id == data.ISES_Id).ToList();
                        if (remfile.Count>0)
                        {
                            foreach (var item in remfile)
                            {
                                _AcademicContext.Remove(item);
                            }
                        }


                            if (data.filelist.Length > 0)
                            {

                                foreach (var item in data.filelist)
                                {
                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    IVRM_EMAIL_ATT_DMO f1 = new IVRM_EMAIL_ATT_DMO();
                                    f1.ISES_Id = data.ISES_Id;
                                    f1.IVRM_Att_Name = item.cfilename;
                                    f1.IVRM_Att_Path = item.cfilepath;
                                    _AcademicContext.Add(f1);
                                }
                            }
                            }


                        var remtype = _AcademicContext.SMS_Email_Setting_RoleTypeDMO.Where(r => r.ISES_Id == data.ISES_Id).ToList();
                        if (remfile.Count > 0)
                        {
                            foreach (var item1 in remtype)
                            {
                                _AcademicContext.Remove(item1);
                            }
                        }

                        if (data.roleids != null)
                        {
                            foreach (var item in data.roleids)
                            {
                                SMS_Email_Setting_RoleTypeDMO rt = new SMS_Email_Setting_RoleTypeDMO();
                                rt.ISES_Id = data.ISES_Id;
                                rt.IVRMRT_Id = item.IVRMRT_Id;
                                rt.ISESRT_ActiveFlg = true;
                                rt.ISESRT_CreatedBy = data.IVRMSTAUL_Id;
                                rt.ISESRT_UpdatedBy = data.IVRMSTAUL_Id;
                                rt.ISESRT_CreatedDate = DateTime.Now;
                                rt.ISESRT_UpdatedDate = DateTime.Now;
                                _AcademicContext.Add(rt);
                            }
                        }


                        var remparam = _AcademicContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(r => r.ISES_Id == data.ISES_Id).ToList();
                        if (remfile.Count > 0)
                        {
                            foreach (var item1 in remparam)
                            {
                                _AcademicContext.Remove(item1);
                            }
                        }

                        var getparameter = (from a in _AcademicContext.SmsEmailParameterMappingDMO
                                                where a.IVRMHE_Id == id
                                                select new SmsEmailDTO
                                                {
                                                    ISMP_ID = a.ISMP_ID

                                                }).Distinct().ToList();


                            if (getparameter.Count > 0)
                            {
                                foreach (var pp in getparameter)
                                {
                                    SMS_MAIL_SAVED_PARAMETER_DMO sp = new SMS_MAIL_SAVED_PARAMETER_DMO();
                                    sp.ISMP_ID = pp.ISMP_ID;
                                    sp.MI_Id = data.MI_Id;
                                    sp.ISES_Id = data.ISES_Id;
                                    sp.Flag = "S";
                                    sp.CreatedDate = DateTime.Now;
                                    sp.UpdatedDate = DateTime.Now;
                                    _AcademicContext.Add(sp);

                                    SMS_MAIL_SAVED_PARAMETER_DMO sp1 = new SMS_MAIL_SAVED_PARAMETER_DMO();
                                    sp1.ISMP_ID = pp.ISMP_ID;
                                    sp1.MI_Id = data.MI_Id;
                                    sp1.ISES_Id = data.ISES_Id;
                                    sp1.Flag = "M";
                                    sp1.CreatedDate = DateTime.Now;
                                    sp1.UpdatedDate = DateTime.Now;
                                    _AcademicContext.Add(sp1);




                                }


                            }



                            int res = _AcademicContext.SaveChanges();
                            if (res > 0)
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

            catch (Exception ee)
            {
                throw ee;
                
            }
            return data;
        }
        public SmsEmailDTO getdetails(int id)
        {
            SmsEmailDTO data = new SmsEmailDTO();
            try
            {
              
                var emaillist= _AcademicContext.smsEmailSetting.Where(e => e.ISES_Id == id).ToList();

                data.emailSmsSettingList = emaillist.ToArray();


                List<SmsEmailHeader> pageWiseHeader = new List<SmsEmailHeader>();
                pageWiseHeader = _AcademicContext.smsEmailHeader.Where(p =>p.IVRMIM_Id == emaillist[0].IVRMIM_Id && p.IVRMIMP_Id == emaillist[0].IVRMIMP_Id).ToList();
                data.pageWiseHeaderList = pageWiseHeader.ToArray();


                if (emaillist.Count > 0)
                {
                    if (emaillist[0].ISES_SMSCCMobileNo != null && emaillist[0].ISES_SMSCCMobileNo != "")
                    {
                        List<string> mobilevv = new List<string>(emaillist[0].ISES_SMSCCMobileNo.Split(','));
                        mobilevv.Reverse();
                        data.mobilenolist = mobilevv.ToArray();


                    }

                    if (emaillist[0].ISES_MailBCCId != null && emaillist[0].ISES_MailBCCId != "")
                    {
                        List<string> eevv = new List<string>(emaillist[0].ISES_MailBCCId.Split(','));
                        eevv.Reverse();
                        data.emailistbcc = eevv.ToArray();
                    }

                    if (emaillist[0].ISES_MailCCId != null && emaillist[0].ISES_MailCCId != "")
                    {
                        List<string> eevv = new List<string>(emaillist[0].ISES_MailCCId.Split(','));
                        eevv.Reverse();
                        data.emailistmcc = eevv.ToArray();
                    }
                }




                data.editfiles = (from a in _AcademicContext.IVRM_EMAIL_ATT_DMO

                                  where (a.ISES_Id == id)
                                  select new emailfiledto
                                  {
                                      cfilename = a.IVRM_Att_Name,
                                      cfilepath = a.IVRM_Att_Path,
                                      cfileid = a.IVRM_EA,
                                  }).Distinct().ToArray();

                data.institutionPageList = (from a in _AcademicContext.Institution_Module_Page
                                            from b in _AcademicContext.masterPage
                                            where (a.IVRMP_Id == b.IVRMP_Id)
                                            select new SMSEmailSettingDTO { IVRMIMP_Id = a.IVRMIMP_Id, IVRMMP_PageName = b.IVRMMP_PageName }
                                    ).ToArray();

                var templname = _AcademicContext.smsEmailSetting.Where(e => e.ISES_Id == id).FirstOrDefault().ISES_Template_Name.ToString();


                data.IVRMHE_Id = _AcademicContext.smsEmailHeader.Where(p => p.IVRMHE_Name.Trim().ToLower() == templname.Trim().ToLower()).FirstOrDefault().IVRMHE_Id;



                if (emaillist[0].ISES_MailHTMLTemplate !="" && emaillist[0].ISES_MailHTMLTemplate != null)
                {
                   var templist = _AcademicContext.IVRM_Master_HTMLTemplatesDMO.Where(t => t.ISMHTML_HTMLTemplate == emaillist[0].ISES_MailHTMLTemplate).ToList();
                    if (templist.Count>0)
                    {
                        data.ISMHTML_Id = templist.FirstOrDefault().ISMHTML_Id;
                    }

                    
                }

                data.parameterList = (from m in _AcademicContext.SMS_MAIL_PARAMETER_DMO
                                     from n in _AcademicContext.SmsEmailParameterMappingDMO
                                     where m.ISMP_ID == n.ISMP_ID && n.IVRMHE_Id == data.IVRMHE_Id
                                      select new SMS_MAIL_PARAMETER_DTO
                                     {
                                         ISMP_ID = n.ISMP_ID,
                                         ISMP_NAME = m.ISMP_NAME
                                     }).ToArray();

                data.rolelist = _AcademicContext.SMS_Email_Setting_RoleTypeDMO.Where(e => e.ISES_Id == id).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public SmsEmailDTO deleterec(int id)
        {
            SmsEmailDTO org = new SmsEmailDTO();
            List<SMSEmailSetting> lorg = new List<SMSEmailSetting>(); // Mapper.Map<Organisation>(org);
            //_OrganisationContext.Remove(enq);
            //_OrganisationContext.SaveChanges();

            try
            {
                lorg = _AcademicContext.smsEmailSetting.Where(t => t.ISES_Id.Equals(id)).ToList();

                if (lorg.Any())
                {
                    _AcademicContext.Remove(lorg.ElementAt(0));
                    var flag = _AcademicContext.SaveChanges();
                    if (flag > 0)
                    {
                        org.returnval = true;
                    }
                    else
                    {
                        org.returnval = false;
                    }
                }
                org.emailSmsSettingList = (from m in _AcademicContext.smsEmailSetting
                                           from n in _AcademicContext.SMS_MAIL_SAVED_PARAMETER_DMO
                                           where (m.ISES_Id == n.ISES_Id && m.MI_Id == lorg.FirstOrDefault().MI_Id)
                                           group m by m.ISES_Id into g
                                           select new SmsEmailDTO
                                           {
                                               ISES_Id = g.FirstOrDefault().ISES_Id,
                                               ISES_MailActiveFlag = g.FirstOrDefault().ISES_MailActiveFlag,
                                               ISES_MailBody = g.FirstOrDefault().ISES_MailBody,
                                               ISES_MailFooter = g.FirstOrDefault().ISES_MailFooter,
                                               ISES_MailHTMLTemplate = g.FirstOrDefault().ISES_MailHTMLTemplate,
                                               ISES_MailSubject = g.FirstOrDefault().ISES_MailSubject,
                                               ISES_Mail_Message = g.FirstOrDefault().ISES_Mail_Message,
                                               ISES_SMSActiveFlag = g.FirstOrDefault().ISES_SMSActiveFlag,
                                               ISES_SMSMessage = g.FirstOrDefault().ISES_SMSMessage,
                                               ISES_Template_Name = g.FirstOrDefault().ISES_Template_Name,
                                               IVRMIMP_Id = g.FirstOrDefault().IVRMIMP_Id,
                                               IVRMIM_Id = g.FirstOrDefault().IVRMIM_Id,
                                               IVRMSTAUL_Id = g.FirstOrDefault().IVRMSTAUL_Id,
                                               MI_Id = g.FirstOrDefault().MI_Id

                                           }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }
        public SMS_MAIL_PARAMETER_DTO getParameter(int headerId)
        {
            SMS_MAIL_PARAMETER_DTO dto = new SMS_MAIL_PARAMETER_DTO();
            try
            {
                dto.parameterList = (from m in _AcademicContext.SMS_MAIL_PARAMETER_DMO
                                     from n in _AcademicContext.SmsEmailParameterMappingDMO
                                     where m.ISMP_ID == n.ISMP_ID && n.IVRMHE_Id == headerId
                                     select new SMS_MAIL_PARAMETER_DTO
                                     {
                                         ISMP_ID = n.ISMP_ID,
                                         ISMP_NAME = m.ISMP_NAME
                                     }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public SmsEmailDTO activedeactivesms(SmsEmailDTO data)
        {
            try
            {
                if (data.ISES_Id > 0)
                {
                    //  SMSEmailSetting result1 = new SMSEmailSetting();
                    var result1 = _AcademicContext.smsEmailSetting.Single(t => t.MI_Id == data.MI_Id && t.ISES_Id == data.ISES_Id);
                    if (result1.ISES_SMSActiveFlag == false)
                    {
                        result1.ISES_SMSActiveFlag = true;
                    }
                    else
                    {
                        result1.ISES_SMSActiveFlag = false;
                    }
                    result1.UpdatedDate = DateTime.Now;

                    _AcademicContext.Update(result1);
                    int i = _AcademicContext.SaveChanges();
                    if (i > 0)
                    {
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

        public SmsEmailDTO activedeactiveemail(SmsEmailDTO data)
        {
            try
            {
                if (data.ISES_Id > 0)
                {
                    SMSEmailSetting result1 = new SMSEmailSetting();
                    var result = _AcademicContext.smsEmailSetting.Single(t => t.MI_Id == data.MI_Id && t.ISES_Id == data.ISES_Id);
                   
                        if (result.ISES_MailActiveFlag == false)
                        {
                        result.ISES_MailActiveFlag = true;
                        }
                        else
                        {
                        result.ISES_MailActiveFlag = false;
                        }
                        result.UpdatedDate = DateTime.Now;

                        _AcademicContext.Update(result);
                        int dataa = _AcademicContext.SaveChanges();
                        if (dataa > 0)
                        {
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
        public SmsEmailDTO viewtempate(SmsEmailDTO data)
        {
            try
            {
                if (data.ISMHTML_Id > 0)
                {
                    data.emailtemplatelist = _AcademicContext.IVRM_Master_HTMLTemplatesDMO.Where(t =>  t.ISMHTML_Id == data.ISMHTML_Id).ToArray();
                   
                        
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
