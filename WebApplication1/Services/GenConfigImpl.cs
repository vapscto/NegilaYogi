using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class GenConfigImpl : Interfaces.GenConfigInterface
    {
        private static ConcurrentDictionary<string, GeneralConfigDTO> _login =
              new ConcurrentDictionary<string, GeneralConfigDTO>();

        public DomainModelMsSqlServerContext _db;

        public GenConfigImpl(DomainModelMsSqlServerContext dmoc)
        {
            _db = dmoc;
        }
        public GeneralConfigDTO Configurationget(GeneralConfigDTO data)
        {
            GeneralConfigDTO cdto = new GeneralConfigDTO();
            try
            {
                cdto.AcademicList = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToArray();




                cdto.mstConfigList = (from a in _db.Institute
                                      from b in _db.GenConfig
                                      where a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id
                                      select new GeneralConfigDTO
                                      {
                                          Instuitename = a.MI_Name,
                                          MI_Id = data.MI_Id,
                                          IVRMGC_Id = b.IVRMGC_Id,
                                      }).ToArray();

                cdto.pagelist = (from a in _db.Institution_Module_Page
                                 from b in _db.Institution_Module
                                 from c in _db.masterPage
                                 where a.IVRMIM_Id == b.IVRMIM_Id && a.IVRMP_Id == c.IVRMP_Id && b.MI_Id == data.MI_Id && a.IVRMIMP_Flag == 1
                                 select new GeneralConfigDTO
                                 {
                                     IVRMP_Id = c.IVRMP_Id,
                                     IVRMMP_PageName = c.IVRMMP_PageName,
                                 }
                              ).Distinct().OrderBy(t => t.IVRMMP_PageName).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }
        public GeneralConfigDTO geteditdata(GeneralConfigDTO data)
        {
            GeneralConfigDTO cdto = new GeneralConfigDTO();
            try
            {
                cdto.AcademicList = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToArray();
                cdto.mstConfigList = _db.GenConfig.Where(t => t.IVRMGC_Id == data.IVRMGC_Id).ToArray();
                if (data.MI_Id == 0)
                {
                    data.MI_Id = _db.GenConfig.Where(t => t.IVRMGC_Id == data.IVRMGC_Id).Select(d => d.MI_Id).FirstOrDefault();
                }
                cdto.usernamearray = _db.IVRM_Custom_UserName_PasswordDMO.Where(s => s.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }

        public GeneralConfigDTO getcontent(GeneralConfigDTO data)
        {

            try
            {
                data.pagelist = (from a in _db.Institution_Module_Page
                                 from b in _db.Institution_Module
                                 from c in _db.masterPage
                                 where a.IVRMIM_Id == b.IVRMIM_Id && a.IVRMP_Id == c.IVRMP_Id && b.MI_Id == data.MI_Id && a.IVRMP_Id == data.IVRMP_Id && a.IVRMIMP_Flag == 1
                                 select new GeneralConfigDTO
                                 {
                                     IVRMP_Id = c.IVRMP_Id,
                                     IVRMMP_PageName = c.IVRMMP_PageName,
                                     IVRMIMP_DisplayContent = a.IVRMIMP_DisplayContent,
                                 }
                               ).Distinct().OrderBy(t => t.IVRMMP_PageName).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }

        public GeneralConfigDTO saveMasterConfig(GeneralConfigDTO mstConfigData)
        {
            try
            {
                GeneralConfigDMO mstConf = Mapper.Map<GeneralConfigDMO>(mstConfigData);

                if (mstConfigData.IVRMGC_Id > 0)
                {
                    mstConf.IVRMGC_MobileValOTPFlag = mstConfigData.IVRMGC_MobileValOTPFlag;
                    mstConf.IVRMGC_emailValOTPFlag = mstConfigData.IVRMGC_emailValOTPFlag;
                    mstConf.IVRMGC_StudentPhotoPath = mstConfigData.IVRMGC_StudentPhotoPath;
                    mstConf.IVRMGC_StaffPhotoPath = mstConfigData.IVRMGC_StaffPhotoPath;
                    mstConf.IVRMGC_ComTrasaNoFlag = mstConfigData.IVRMGC_ComTrasaNoFlag;
                    mstConf.IVRMGC_SMSDomain = mstConfigData.IVRMGC_SMSDomain;
                    mstConf.IVRMGC_SMSURL = mstConfigData.IVRMGC_SMSURL;
                    mstConf.IVRMGC_SMSUserName = mstConfigData.IVRMGC_SMSUserName;
                    mstConf.IVRMGC_SMSPassword = mstConfigData.IVRMGC_SMSPassword;
                    mstConf.IVRMGC_SMSSenderId = mstConfigData.IVRMGC_SMSSenderId;
                    mstConf.IVRMGC_SMSWorkingKey = mstConfigData.IVRMGC_SMSWorkingKey;
                    mstConf.IVRMGC_SMSFooter = mstConfigData.IVRMGC_SMSFooter;
                    mstConf.IVRMGC_SMSActiveFlag = mstConfigData.IVRMGC_SMSActiveFlag;
                    mstConf.IVRMGC_emailUserName = mstConfigData.IVRMGC_emailUserName;
                    mstConf.IVRMGC_emailPassword = mstConfigData.IVRMGC_emailPassword;
                    mstConf.IVRMGC_HostName = mstConfigData.IVRMGC_HostName;
                    mstConf.IVRMGC_PortNo = mstConfigData.IVRMGC_PortNo;
                    mstConf.IVRMGC_MailGenralDesc = mstConfigData.IVRMGC_MailGenralDesc;
                    mstConf.IVRMGC_Webiste = mstConfigData.IVRMGC_Webiste;
                    mstConf.IVRMGC_emailid = mstConfigData.IVRMGC_emailid;
                    mstConf.IVRMGC_emailFooter = mstConfigData.IVRMGC_emailFooter;
                    mstConf.IVRMGC_CCMail = mstConfigData.IVRMGC_CCMail;
                    mstConf.IVRMGC_BCCMail = mstConfigData.IVRMGC_BCCMail;
                    mstConf.IVRMGC_ToMail = mstConfigData.IVRMGC_ToMail;
                    mstConf.IVRMGC_EmailActiveFlag = mstConfigData.IVRMGC_EmailActiveFlag;
                    mstConf.IVRMGC_Pagination = mstConfigData.IVRMGC_Pagination;
                    mstConf.IVRMGC_ReminderDays = mstConfigData.IVRMGC_ReminderDays;
                    mstConf.IVRMGC_ClassCapacity = mstConfigData.IVRMGC_ClassCapacity;
                    mstConf.IVRMGC_SectionCapacity = mstConfigData.IVRMGC_SectionCapacity;
                    mstConf.IVRMGC_SCLockingPeriod = mstConfigData.IVRMGC_SCLockingPeriod;
                    mstConf.IVRMGC_SCActive = mstConfigData.IVRMGC_SCActive;
                    mstConf.IVRMGC_FPActive = mstConfigData.IVRMGC_FPActive;
                    mstConf.IVRMGC_OnlineFPActive = mstConfigData.IVRMGC_OnlineFPActive;
                    mstConf.IVRMGC_FaceReaderActive = mstConfigData.IVRMGC_FaceReaderActive;
                    mstConf.IVRMGC_DefaultStudentSelection = mstConfigData.IVRMGC_DefaultStudentSelection;
                    mstConf.IVRMGC_PagePagination = mstConfigData.IVRMGC_PagePagination;
                    mstConf.IVRMGC_ReportPagination = mstConfigData.IVRMGC_ReportPagination;
                    mstConf.IVRMGC_AdmnoColumnDisplay = mstConfigData.IVRMGC_AdmnoColumnDisplay;
                    mstConf.IVRMGC_RegnoColumnDisplay = mstConfigData.IVRMGC_RegnoColumnDisplay;
                    mstConf.IVRMGC_RollnoColumnDisplay = mstConfigData.IVRMGC_RollnoColumnDisplay;
                    mstConf.IVRMGC_FPLockingPeriod = mstConfigData.IVRMGC_FPLockingPeriod;
                    mstConf.IVRMGC_Disclaimer = mstConfigData.IVRMGC_Disclaimer;
                    mstConf.IVRMGC_PrincipalSign = mstConfigData.IVRMGC_PrincipalSign;
                    mstConf.IVRMGC_ManagerSign = mstConfigData.IVRMGC_ManagerSign;
                    mstConf.IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag = mstConfigData.IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag;
                    mstConf.IVRMGC_TransportRequired = mstConfigData.IVRMGC_TransportRequired;
                    mstConf.IVRMGC_OTPMobileNo = mstConfigData.IVRMGC_OTPMobileNo;
                    mstConf.IVRMGC_OTPMailId = mstConfigData.IVRMGC_OTPMailId;
                    //  mstConf.IVRMGC_OnlinePaymentCompany = mstConfigData.IVRMGC_OnlinePaymentCompany;
                    mstConf.IVRMGC_SendGrid_Key = mstConfigData.IVRMGC_SendGrid_Key;
                    mstConf.IVRMGC_Classwise_Payment = mstConfigData.IVRMGC_Classwise_Payment;
                    mstConf.IVRMGC_APIOrSMTPFlg = mstConfigData.IVRMGC_APIOrSMTPFlg;
                    mstConf.IVRMGC_SportsPointsDropdownFlg = mstConfigData.IVRMGC_SportsPointsDropdownFlg;

                    mstConf.IVRMGC_EnableSTSUBTIntFlg = mstConfigData.IVRMGC_EnableSTSUBTIntFlg;
                    mstConf.IVRMGC_EnableSTCTIntFlg = mstConfigData.IVRMGC_EnableSTCTIntFlg;
                    mstConf.IVRMGC_EnableSTHODIntFlg = mstConfigData.IVRMGC_EnableSTHODIntFlg;
                    mstConf.IVRMGC_EnableSTPrincipalIntFlg = mstConfigData.IVRMGC_EnableSTPrincipalIntFlg;
                    mstConf.IVRMGC_EnableSTASIntFlg = mstConfigData.IVRMGC_EnableSTASIntFlg;
                    mstConf.IVRMGC_EnableSTECIntFlg = mstConfigData.IVRMGC_EnableSTECIntFlg;
                    mstConf.IVRMGC_GMRDTOALLFlg = mstConfigData.IVRMGC_GMRDTOALLFlg;
                    mstConf.IVRMGC_EnableStaffwiseIntFlg = mstConfigData.IVRMGC_EnableStaffwiseIntFlg;
                    mstConf.IVRMGC_EnableCTSTIntFlg = mstConfigData.IVRMGC_EnableCTSTIntFlg;
                    mstConf.IVRMGC_EnableHODSTIntFlg = mstConfigData.IVRMGC_EnableHODSTIntFlg;
                    mstConf.IVRMGC_EnablePrincipalSTIntFlg = mstConfigData.IVRMGC_EnablePrincipalSTIntFlg;
                    mstConf.IVRMGC_EnableASSTIntFlg = mstConfigData.IVRMGC_EnableASSTIntFlg;
                    mstConf.IVRMGC_EnableECSTIntFlg = mstConfigData.IVRMGC_EnableECSTIntFlg;
                    mstConf.IVRMGC_StudentDataChangeAlertFlg = mstConfigData.IVRMGC_StudentDataChangeAlertFlg;
                    mstConf.IVRMGC_StudentDataChangeAlertDays = mstConfigData.IVRMGC_StudentDataChangeAlertDays;
                    mstConf.IVRMGC_CatLogoFlg = mstConfigData.IVRMGC_CatLogoFlg;
                    mstConf.IVRMGC_PasswordExpiryDuration = mstConfigData.IVRMGC_PasswordExpiryDuration;
                    mstConf.IVRMGC_AttShortageAlertDays = mstConfigData.IVRMGC_AttShortageAlertDays;
                    mstConf.IVRMGC_AttendanceShortagePercent = mstConfigData.IVRMGC_AttendanceShortagePercent;
                    mstConf.IVRMGC_AttendanceShortageAlertFlg = mstConfigData.IVRMGC_AttendanceShortageAlertFlg;
                    //
                    //mstConf.IVRMGC_StudentLoginCred = mstConfigData.IVRMGC_StudentLoginCred;
                    //mstConf.IVRMGC_FatherLoginCred = mstConfigData.IVRMGC_FatherLoginCred;
                    //mstConf.IVRMGC_MotherLoginCred = mstConfigData.IVRMGC_MotherLoginCred;
                    //mstConf.IVRMGC_GuardianLoginCred = mstConfigData.IVRMGC_GuardianLoginCred;
                    //mstConf.IVRMGC_AutoCreateStudentCredFlg = mstConfigData.IVRMGC_AutoCreateStudentCredFlg;
                    //mstConf.IVRMGC_UserNameOptionsFlg = mstConfigData.IVRMGC_UserNameOptionsFlg;




                    if (mstConfigData.pageids != null)
                    {

                        foreach (var item1 in mstConfigData.pageids)
                        {

                            var pagelist = (from a in _db.Institution_Module_Page
                                            from b in _db.Institution_Module
                                            from c in _db.masterPage
                                            where a.IVRMIM_Id == b.IVRMIM_Id && a.IVRMP_Id == c.IVRMP_Id && b.MI_Id == mstConfigData.MI_Id && a.IVRMP_Id == item1.IVRMP_Id && a.IVRMIMP_Flag == 1
                                            select new GeneralConfigDTO
                                            {
                                                IVRMIMP_Id = a.IVRMIMP_Id,

                                            }
                               ).Distinct().ToList();

                            if (pagelist.Count > 0)
                            {
                                foreach (var item in pagelist)
                                {
                                    var res = _db.Institution_Module_Page.Single(e => e.IVRMIMP_Id == item.IVRMIMP_Id);

                                    res.IVRMIMP_DisplayContent = item1.IVRMIMP_DisplayContent;

                                    _db.Update(res);
                                }
                            }
                        }

                    }
                    if (mstConfigData.usernameConfig != null)
                    {
                        if (mstConfigData.usernameConfig.Length > 0)
                        {
                            foreach (var item in mstConfigData.usernameConfig)
                            {
                                if (item.IVRMCUNP_Id != null && item.IVRMCUNP_Id > 0)
                                {
                                    var resuser = _db.IVRM_Custom_UserName_PasswordDMO.Single(e => e.IVRMCUNP_Id == item.IVRMCUNP_Id);
                                    resuser.MI_Id = mstConfigData.MI_Id;
                                    resuser.IVRMCUNP_Length = item.IVRMCUNP_Length;
                                    resuser.IVRMCUNP_FromOrderFlg = item.IVRMCUNP_FromOrderFlg;
                                    resuser.IVRMCUNP_Order = item.IVRMCUNP_Order;
                                    resuser.IVRMCUNP_Field = item.IVRMCUNP_Field;
                                    resuser.IVRMCUNP_UpdatedDate = DateTime.Now;
                                    _db.Update(resuser);
                                }
                                else
                                {
                                    IVRM_Custom_UserName_PasswordDMO UserName_Password = new IVRM_Custom_UserName_PasswordDMO();
                                    UserName_Password.IVRMCUNP_Length = item.IVRMCUNP_Length;
                                    UserName_Password.IVRMCUNP_Order = item.IVRMCUNP_Order;
                                    UserName_Password.IVRMCUNP_FromOrderFlg = item.IVRMCUNP_FromOrderFlg;
                                    UserName_Password.IVRMCUNP_Field = item.IVRMCUNP_Field;
                                    UserName_Password.MI_Id = mstConfigData.MI_Id;
                                    UserName_Password.IVRMCUNP_ActiveFlag = true;
                                    UserName_Password.IVRMCUNP_CreatedDate = DateTime.Now;
                                    UserName_Password.IVRMCUNP_UpdatedDate = DateTime.Now;
                                    UserName_Password.IVRMCUNP_CreatedBy = 0;
                                    UserName_Password.IVRMCUNP_UpdatedBy = 0;
                                    _db.Add(UserName_Password);
                                }



                            }
                        }
                    }








                    mstConf.UpdatedDate = DateTime.Now;
                    _db.Update(mstConf);
                    _db.SaveChanges();
                    mstConfigData.returnval = "Successfully saved the record"; // To display the message 


                    var Emilcheck = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == mstConf.MI_Id).ToList();
                    if (Emilcheck.Count() > 0)
                    {
                        EMAIL_DETAILS_DMO Emailupdate = new EMAIL_DETAILS_DMO();
                        Emailupdate = _db.EMAIL_DETAILS_DMO.Single(t => t.MI_ID == mstConf.MI_Id);

                        Emailupdate.IVRMMD_HOSTNAME = mstConfigData.IVRMGC_HostName;
                        Emailupdate.IVRMMD_Mail_ID = mstConfigData.IVRMGC_emailUserName;
                        Emailupdate.IVRMMD_Mail_PASSWORD = mstConfigData.IVRMGC_emailPassword;
                        Emailupdate.IVRMMD_PORTNO = mstConfigData.IVRMGC_PortNo.ToString();
                        if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail == "" || mstConfigData.IVRMGC_BCCMail == null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail;
                        }
                        else if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail != "" && mstConfigData.IVRMGC_BCCMail != null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail + "," + mstConfigData.IVRMGC_BCCMail;
                        }
                        _db.Update(Emailupdate);
                        _db.SaveChanges();
                    }
                    else
                    {
                        EMAIL_DETAILS_DMO Emailupdate = new EMAIL_DETAILS_DMO();

                        Emailupdate.IVRMMD_HOSTNAME = mstConfigData.IVRMGC_HostName;
                        Emailupdate.IVRMMD_Mail_ID = mstConfigData.IVRMGC_emailUserName;
                        Emailupdate.IVRMMD_Mail_PASSWORD = mstConfigData.IVRMGC_emailPassword;
                        Emailupdate.IVRMMD_PORTNO = mstConfigData.IVRMGC_PortNo.ToString();
                        if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail == "" || mstConfigData.IVRMGC_BCCMail == null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail;
                        }
                        else if ((mstConfigData.IVRMGC_CCMail != "" && mstConfigData.IVRMGC_CCMail != null) && (mstConfigData.IVRMGC_BCCMail != "" && mstConfigData.IVRMGC_BCCMail != null))
                        {
                            Emailupdate.IVRM_mailcc = mstConfigData.IVRMGC_CCMail + "," + mstConfigData.IVRMGC_BCCMail;
                        }
                        Emailupdate.MI_ID = mstConf.MI_Id;
                        _db.Add(Emailupdate);
                        _db.SaveChanges();

                    }

                    var Smscheck = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID == mstConf.MI_Id).ToList();
                    if (Smscheck.Count() > 0)
                    {
                        SMS_DETAILS_DMO Smsupdate = new SMS_DETAILS_DMO();
                        Smsupdate = _db.SMS_DETAILS_DMO.Single(t => t.MI_ID == mstConf.MI_Id);
                        Smsupdate.IVRMSD_DOMIN = mstConfigData.IVRMGC_SMSDomain;
                        Smsupdate.IVRMSD_URL = mstConfigData.IVRMGC_SMSURL;
                        Smsupdate.IVRMSD_USERNAME = mstConfigData.IVRMGC_SMSUserName;
                        Smsupdate.IVRMSD_PASSWORD = mstConfigData.IVRMGC_SMSPassword;
                        Smsupdate.IVRMSD_WORKINGKEY = mstConfigData.IVRMGC_SMSWorkingKey;
                        Smsupdate.IVRMSD_SENDERID = mstConfigData.IVRMGC_SMSSenderId;
                        _db.Update(Smsupdate);
                        _db.SaveChanges();
                    }
                    else
                    {
                        SMS_DETAILS_DMO Smsupdate = new SMS_DETAILS_DMO();
                        Smsupdate.IVRMSD_DOMIN = mstConfigData.IVRMGC_SMSDomain;
                        Smsupdate.IVRMSD_URL = mstConfigData.IVRMGC_SMSURL;
                        Smsupdate.IVRMSD_USERNAME = mstConfigData.IVRMGC_SMSUserName;
                        Smsupdate.IVRMSD_PASSWORD = mstConfigData.IVRMGC_SMSPassword;
                        Smsupdate.IVRMSD_WORKINGKEY = mstConfigData.IVRMGC_SMSWorkingKey;
                        Smsupdate.IVRMSD_SENDERID = mstConfigData.IVRMGC_SMSSenderId;
                        Smsupdate.MI_ID = mstConf.MI_Id;
                        _db.Add(Smsupdate);
                        _db.SaveChanges();
                    }

                }
                else
                {
                    var mstConfigList = _db.GenConfig.Where(t => t.MI_Id == mstConf.MI_Id).ToArray();
                    if (mstConfigList.Count() > 0)
                    {
                        mstConfigData.returnval = "Already Record Is Avilable Same Instuite !";
                    }
                    else
                    {

                        if (mstConfigData.pageids != null)
                        {

                            foreach (var item1 in mstConfigData.pageids)
                            {

                                var pagelist = (from a in _db.Institution_Module_Page
                                                from b in _db.Institution_Module
                                                from c in _db.masterPage
                                                where a.IVRMIM_Id == b.IVRMIM_Id && a.IVRMP_Id == c.IVRMP_Id && b.MI_Id == mstConfigData.MI_Id && a.IVRMP_Id == item1.IVRMP_Id && a.IVRMIMP_Flag == 1
                                                select new GeneralConfigDTO
                                                {
                                                    IVRMIMP_Id = a.IVRMIMP_Id,

                                                }
                                   ).Distinct().ToList();

                                if (pagelist.Count > 0)
                                {
                                    foreach (var item in pagelist)
                                    {
                                        var res = _db.Institution_Module_Page.Single(e => e.IVRMIMP_Id == item.IVRMIMP_Id);

                                        res.IVRMIMP_DisplayContent = item1.IVRMIMP_DisplayContent;

                                        _db.Update(res);
                                    }
                                }
                            }

                        }

                        if (mstConfigData.usernameConfig != null)
                        {
                            if (mstConfigData.usernameConfig.Length > 0)
                            {
                                foreach (var item in mstConfigData.usernameConfig)
                                {

                                    var duplicate = _db.IVRM_Custom_UserName_PasswordDMO.Where(s => s.IVRMCUNP_Length == item.IVRMCUNP_Length && s.IVRMCUNP_Order == item.IVRMCUNP_Order && s.IVRMCUNP_FromOrderFlg == item.IVRMCUNP_FromOrderFlg && s.IVRMCUNP_Field == item.IVRMCUNP_Field && s.MI_Id == mstConfigData.MI_Id).ToArray();
                                    if (duplicate.Length == 0)
                                    {
                                        IVRM_Custom_UserName_PasswordDMO UserName_Password = new IVRM_Custom_UserName_PasswordDMO();
                                        UserName_Password.IVRMCUNP_Length = item.IVRMCUNP_Length;
                                        UserName_Password.IVRMCUNP_Order = item.IVRMCUNP_Order;
                                        UserName_Password.IVRMCUNP_FromOrderFlg = item.IVRMCUNP_FromOrderFlg;
                                        UserName_Password.IVRMCUNP_Field = item.IVRMCUNP_Field;
                                        UserName_Password.MI_Id = mstConfigData.MI_Id;
                                        UserName_Password.IVRMCUNP_CreatedDate = DateTime.Now;
                                        UserName_Password.IVRMCUNP_UpdatedDate = DateTime.Now;
                                        UserName_Password.IVRMCUNP_CreatedBy = 0;
                                        UserName_Password.IVRMCUNP_UpdatedBy = 0;
                                        _db.Add(UserName_Password);
                                    }

                                }
                            }
                        }


                        mstConf.CreatedDate = DateTime.Now;
                        mstConf.UpdatedDate = DateTime.Now;
                        _db.Add(mstConf);
                        var contactExists = _db.SaveChanges();
                        if (contactExists == 1)
                        {
                            mstConfigData.returnval = "Record Saved Successfully";
                        }
                        else
                        {
                            mstConfigData.returnval = "Record Not Saved";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return mstConfigData;
        }


        public GeneralConfigDTO deleteUserNameconfig(GeneralConfigDTO id)
        {
            List<IVRM_Custom_UserName_PasswordDMO> st = _db.IVRM_Custom_UserName_PasswordDMO.Where(rg => rg.IVRMCUNP_Id == id.IVRMCUNP_Id).ToList();

            if (st.Count() > 0)
            {
                for (int i = 0; i < st.Count(); i++)
                {
                    _db.Remove(st.ElementAt(i));
                    _db.SaveChanges();
                    //id.message = "Success";
                }
            }
            return id;
        }

    }
}
