using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Alumni;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CommonLibrary
{
    public class PushNotification
    {

     
        private DomainModelMsSqlServerContext _db;
        private PortalContext _PortalContext;
        private CollegeportalContext PortalContextclg;
        public AlumniContext _AlumniContext;

        public PushNotification(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }

        public PushNotification(PortalContext portalContext)
        {
            _PortalContext = portalContext;
        }
         public PushNotification(AlumniContext AlumniContext)
        {
            _AlumniContext = AlumniContext;
        }

        public PushNotification(CollegeportalContext portalContextclg)
        {
            PortalContextclg = portalContextclg;
        }
      
        public string Insert_PushNotification_classwork(long icw_Id, long mi_id, string devicenew, IVRM_ClassWorkDTO dto, string header_flg)
        {
           

            try
            {
           var classwork1 = _db.IVRM_ClassWorkDMO_con.Where(h => h.MI_Id == mi_id && h.ICW_ActiveFlag == true && h.ICW_Id == icw_Id).Distinct().ToList();
              

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

             
                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
               sdd.PNSD_HeaderName = classwork1[0].ICW_Topic;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                sdd.PNSD_SMSMessage = classwork1[0].ICW_SubTopic;
                sdd.PNSD_Header_Flg = header_flg;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _db.Add(sdd);               

                foreach (var item in dto.devicelist12)
                {
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = item.AMST_MobileNo;
                    ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                    ss.PNSDDE_ReadStatus = "1";
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                    _db.Add(ss);
                }

                foreach(var item1 in dto.devicelist12)
                {
                    PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.AMST_Id = item1.AMST_Id;
                     _db.Add(sds);
                }
             
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        
        public string Insert_PushNotification_homework( long ihw_Id, long mi_id, IVRM_Homework_DTO dto, string header_flg)
        {


            try
            {
                
                
                var classwork1 = _PortalContext.IVRM_Homework_DMO.Where(h => h.MI_Id == mi_id && h.IHW_ActiveFlag == true && h.IHW_Id == ihw_Id).Distinct().ToList();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = classwork1[0].IHW_Topic;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                sdd.PNSD_SMSMessage = classwork1[0].IHW_Assignment;
                sdd.PNSD_Header_Flg = header_flg;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);

                foreach (var item in dto.devicelist12)
                {
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = item.AMST_MobileNo;
                    ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                    ss.PNSDDE_ReadStatus = "1";
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                    _PortalContext.Add(ss);
                }

                foreach (var item1 in dto.devicelist12)
                {
                    PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.AMST_Id = item1.AMST_Id;
                    _PortalContext.Add(sds);
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        
        public string Insert_PushNotification_noticeboard( long INTB_Id, long mi_id, IVRM_Homework_DTO dto, string header_flg)
        {


            try
            {
                string title = "";
                var classwork1 = _PortalContext.IVRM_NoticeBoardDMO.Where(h => h.MI_Id == mi_id && h.INTB_ActiveFlag == true && h.INTB_Id == INTB_Id).Distinct().ToList();
                if (classwork1[0].NTB_TTSylabusFlg == "S")
                {
                    title = "Syllabus";
                }
                else if (classwork1[0].NTB_TTSylabusFlg == "TT")
                {
                    title = "TimeTable";
                }
                else if (classwork1[0].NTB_TTSylabusFlg == "P")
                {
                    title = "Portion";
                }
                else if (classwork1[0].NTB_TTSylabusFlg == "O")
                {
                    title = "Other";
                }

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = title;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                sdd.PNSD_SMSMessage = classwork1[0].INTB_Title;
                sdd.PNSD_Header_Flg = header_flg;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);

                foreach (var item in dto.devicelist12)
                {
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = item.AMST_MobileNo;
                    ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                    ss.PNSDDE_ReadStatus = "1";
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                    _PortalContext.Add(ss);
                }

                foreach (var item1 in dto.devicelist12)
                {
                    PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.AMST_Id = item1.AMST_Id;
                    _PortalContext.Add(sds);
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        
        // for clg interaction used

        public string Insert_PushNotification_interaction1(string subject, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {


            try
            {
                string title = "";
                var classwork1 = _PortalContext.IVRM_School_Master_InteractionsDMO.Where(h => h.ISMINT_Id == istint_Id).Distinct().ToList();
                if (dto.roleflg == "student")
                {
                    title = "Student";
                }
                else
                {
                    title = "Staff";
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = classwork1[0].ISMINT_Subject;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = classwork1[0].ISMINT_Interaction;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
                if (dto.userflag == "Student")
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.AMCST_MobileNo;
                        ss.PNSDDE_DeviceId = item.AMCST_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.AMST_Id = item1.AMCST_Id;
                        _PortalContext.Add(sds);
                    }
                }
                else
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                        ss.PNSDDE_DeviceId = item.HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.HRME_Id = item1.HRME_Id;
                        _PortalContext.Add(sds);
                    }
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }


        public string Insert_PushNotification_interaction( string subject, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {


            try
            {
                string title = "";
                var classwork1 = _PortalContext.IVRM_School_Master_InteractionsDMO.Where(h => h.ISMINT_Id == istint_Id ).Distinct().ToList();
                if (dto.roleflg== "student" || dto.roleflg == "Student")
                {
                    title = "Student";
                }
                else 
                {
                    title = "Staff";
                }
         

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = classwork1[0].ISMINT_Subject; 
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = classwork1[0].ISMINT_Interaction;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
                if (dto.userflag == "Student")
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.AMST_MobileNo;
                        ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.AMST_Id = item1.AMST_Id;
                        _PortalContext.Add(sds);
                    }
                }
                else
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                        ss.PNSDDE_DeviceId = item.HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.HRME_Id = item1.HRME_Id;
                        _PortalContext.Add(sds);
                    }
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }

        //save(send) interaction details
        public string Insert_PushNotification_interaction_send(string subject, string ISMINT_Interaction, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {


            try
            {
                string title = "";
                var classwork1 = _PortalContext.IVRM_School_Master_InteractionsDMO.Where(h => h.ISMINT_Id == istint_Id).Distinct().ToList();
                if (dto.roleflg == "student" || dto.roleflg == "Student")
                {
                    title = "Student";
                }
                else
                {
                    title = "Staff";
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = subject;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = ISMINT_Interaction;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_Header_Flg = "Interaction";
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
                if (dto.userflag == "Student")
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.AMST_MobileNo;
                        ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.AMST_Id = item1.AMST_Id;
                        _PortalContext.Add(sds);
                    }
                }
                else
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                        ss.PNSDDE_DeviceId = item.HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.HRME_Id = item1.HRME_Id;
                        _PortalContext.Add(sds);
                    }
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        // for clg interaction
        public string Insert_PushNotification_interaction_replay1(string subject, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {


            try
            {
                string title = "";
                var classwork1 = _PortalContext.IVRM_School_Transaction_InteractionsDMO.Where(h => h.ISTINT_Id == istint_Id).Distinct().ToList();
                var classwork2 = _PortalContext.IVRM_School_Master_InteractionsDMO.Where(h => h.ISMINT_Id == classwork1[0].ISMINT_Id).Distinct().ToList();
                if (dto.roleflg == "student")
                {
                    title = "Student";
                }
                else
                {
                    title = "Staff";
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = classwork2[0].ISMINT_Subject;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = classwork1[0].ISTINT_Interaction;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_Header_Flg = "Interaction";
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
                if (dto.userflag == "Student")
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.AMCST_MobileNo;
                        ss.PNSDDE_DeviceId = item.AMCST_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.AMST_Id = item1.AMCST_Id;
                        _PortalContext.Add(sds);
                    }
                }
                else
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                        ss.PNSDDE_DeviceId = item.HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.HRME_Id = item1.HRME_Id;
                        _PortalContext.Add(sds);
                    }
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        public string Insert_PushNotification_interaction_replay(string subject, long istint_Id, long mi_id, IVRM_School_InteractionsDTO dto)
        {


            try
            {
                string title = "";
                var classwork1 = _PortalContext.IVRM_School_Transaction_InteractionsDMO.Where(h => h.ISTINT_Id == istint_Id).Distinct().ToList();
                var classwork2 = _PortalContext.IVRM_School_Master_InteractionsDMO.Where(h => h.ISMINT_Id == classwork1[0].ISMINT_Id).Distinct().ToList();
                if (dto.roleflg == "student" || dto.roleflg == "Student")
                {
                    title = "Student";
                }
                else
                {
                    title = "Staff";
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = classwork2[0].ISMINT_Subject;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = classwork1[0].ISTINT_Interaction;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_Header_Flg = "Interaction";
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
                if (dto.userflag == "Student")
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.AMST_MobileNo;
                        ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.AMST_Id = item1.AMST_Id;
                        _PortalContext.Add(sds);
                    }
                }
                else
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                        ss.PNSDDE_DeviceId = item.HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.HRME_Id = item1.HRME_Id;
                        _PortalContext.Add(sds);
                    }
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        public string Insert_PushNotification_alumni_interaction_replay(string subject, long istint_Id, long mi_id, Alumni_School_Interactions_DTO dto)
        {


            try
            {
                string title = "";
                var classwork1 = _AlumniContext.Alumni_School_Transaction_Interactions_DMO_con.Where(h => h.ALSTINT_Id == istint_Id).Distinct().ToList();
                var classwork2 = _AlumniContext.Alumni_School_Master_Interactions_DMO_con.Where(h => h.ALSMINT_Id == classwork1[0].ALSMINT_Id).Distinct().ToList();
                if (dto.roleflg == "alumni" || dto.roleflg == "Alumni")
                {
                    title = "Alumni";
                }
                else
                {
                    title = "Staff";
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = classwork2[0].ALSMINT_Subject;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = classwork1[0].ALSTINT_Interaction;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_Header_Flg = "Interaction";
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
                if (dto.userflag == "Student")
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.ALMST_MobileNo;
                        ss.PNSDDE_DeviceId = item.ALMST_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.AMST_Id = item1.ALMST_Id;
                        _PortalContext.Add(sds);
                    }
                }
                else
                {
                    foreach (var item in dto.devicelist12)
                    {
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                        ss.PNSDDE_DeviceId = item.HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);
                    }

                    foreach (var item1 in dto.devicelist12)
                    {
                        PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                        sds.PNSD_Id = sdd.PNSD_Id;
                        sds.HRME_Id = item1.HRME_Id;
                        _PortalContext.Add(sds);
                    }
                }

                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        public string Insert_PushNotification_asset_tagging(long MI_Id, string employeename, string message, long?HRME_MobileNo, string HRME_AppDownloadedDeviceId, long HRME_Id)
        {


            try
            {
                string title = "";
               
               
                    title = "Staff";
                var header = "Asset Expire";
              


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = MI_Id;
                sdd.PNSD_HeaderName = header;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = title;
                sdd.PNSD_Header_Flg = title;
                sdd.PNSD_SMSMessage = message;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _PortalContext.Add(sdd);
               
                        PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                        ss.PNSD_Id = sdd.PNSD_Id;
                        ss.PNSDDE_MobileNo = HRME_MobileNo;
                        ss.PNSDDE_DeviceId = HRME_AppDownloadedDeviceId;
                        ss.PNSDDE_ReadStatus = "1";
                        ss.PNSDDE_DeliveryDate = indianTime;
                        ss.PNSDDE_DeliveryTime = time;
                        ss.PNSDDE_MakeUnreadFlg = false;
                        ss.PNSDDE_ApprovalLevel = "0";
                        _PortalContext.Add(ss);


                PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                sds.PNSD_Id = sdd.PNSD_Id;
                sds.HRME_Id = HRME_Id;
                _PortalContext.Add(sds);


                _PortalContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }

        public string Insert_PushNotification_leaveapply(long HRELAP_Id, long mi_id, string devicenew, LeaveCreditDTO dto, string header_flg)
        {
            try
            {
                var leavedetails = _db.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_Id == HRELAP_Id).FirstOrDefault();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                DateTime fromdate = DateTime.Parse(leavedetails.HRELAP_FromDate.ToString());
                DateTime todate = DateTime.Parse(leavedetails.HRELAP_ToDate.ToString());

                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = "Leave Apply";
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                sdd.PNSD_SMSMessage = "Leave applied successfully from " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                sdd.PNSD_Header_Flg = header_flg;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _db.Add(sdd);

                foreach (var item in dto.devicelist12)
                {
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();
                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                    ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                    ss.PNSDDE_ReadStatus = "1";
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                    _db.Add(ss);
                }
                foreach (var item1 in dto.devicelist12)
                {
                    PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.HRME_Id = item1.HRME_Id;
                    _db.Add(sds);
                }
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public string Insert_PushNotification(long MI_Id, string Header_Name, string Message, long? MobileNo, string HRME_AppDownloadedDeviceId, long HRME_Id, long AMST_Id)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = MI_Id;
                sdd.PNSD_HeaderName = Header_Name;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                sdd.PNSD_SMSMessage = Message;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _db.Add(sdd);

                PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();
                ss.PNSD_Id = sdd.PNSD_Id;
                ss.PNSDDE_MobileNo = MobileNo;
                ss.PNSDDE_DeviceId = HRME_AppDownloadedDeviceId;
                ss.PNSDDE_ReadStatus = "1";
                ss.PNSDDE_DeliveryDate = indianTime;
                ss.PNSDDE_DeliveryTime = time;
                ss.PNSDDE_MakeUnreadFlg = false;
                ss.PNSDDE_ApprovalLevel = "0";
                _db.Add(ss);

                PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                sds.PNSD_Id = sdd.PNSD_Id;
                sds.HRME_Id = HRME_Id;
                _db.Add(sds);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public string Call_PushNotificationGeneral(string devicenew,long MI_Id, long sentuserid, long revieveduserid, long TransactionId, string Message,string Type,string redirecturl)
        {
            try
            {
                //Call notication
                //PushNotification push_noti = new PushNotification(_db);
                //push_noti.Call_PushNotificationGeneral(devicenew, MI_Id, sentuserid, revieveduserid, TransactionId, Message, Type, redirecturl);



                string url = "";
                string daata = "";


                var key = _PortalContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == MI_Id).MAAN_AuthenticationKey;
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();

                string sound = "default";
                string notId = "4";



                Dictionary<string, object> input = new Dictionary<string, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("body", Message);
                transfersnotes.Add("title", Type);

                input.Add("to", devicenew);
                input.Add("notification", transfersnotes);

                var myContent = JsonConvert.SerializeObject(input);
                String postdata = myContent;


                //var mycontent = notificationparams[0];
                //string postdata = mycontent.ToString();


                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }


                var contactExistsP = _PortalContext.Database.ExecuteSqlCommand("PN_Save_Pushnotification @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", MI_Id, sentuserid, revieveduserid, TransactionId, Message, Type, redirecturl, devicenew);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string Clg_Call_PushNotificationGeneral(string devicenew, long MI_Id, long sentuserid, long revieveduserid, long TransactionId, string Message, string Type, string redirecturl)
        {
            try
            {
                //Call notication
                //PushNotification push_noti = new PushNotification(_db);
                //push_noti.Call_PushNotificationGeneral(devicenew, MI_Id, sentuserid, revieveduserid, TransactionId, Message, Type, redirecturl);



                string url = "";
                string daata = "";


                var key = PortalContextclg.MobileApplAuthenticationDMO.Single(a => a.MI_Id == MI_Id).MAAN_AuthenticationKey;
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();

                string sound = "default";
                string notId = "4";



                Dictionary<string, object> input = new Dictionary<string, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("body", Message);
                transfersnotes.Add("title", Type);

                input.Add("to", devicenew);
                input.Add("notification", transfersnotes);

                var myContent = JsonConvert.SerializeObject(input);
                String postdata = myContent;


                //var mycontent = notificationparams[0];
                //string postdata = mycontent.ToString();


                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }


                var contactExistsP = PortalContextclg.Database.ExecuteSqlCommand("PN_Save_Pushnotification @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", MI_Id, sentuserid, revieveduserid, TransactionId, Message, Type, redirecturl, devicenew);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


    }
}
