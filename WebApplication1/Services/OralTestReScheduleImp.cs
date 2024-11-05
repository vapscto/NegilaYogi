using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Net.NetworkInformation;
using System.Net;
using DomainModel.Model.com.vapstech.Portals.Employee;

namespace WebApplication1.Services
{
    public class OralTestReScheduleImp : Interfaces.OralTestReScheduleInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
        new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly OralTestScheduleContext _OralTestScheduleContext;
        private readonly DomainModelMsSqlServerContext _context;
        public ScheduleReportContext _ScheduleReportContext;
        public StudentApplicationContext _StudentApplicationContext;
        public FeeGroupContext _feecontext;
        public OralTestReScheduleImp(StudentApplicationContext StudentApplicationContext, OralTestScheduleContext OralTestScheduleContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext, ScheduleReportContext DomainModelContext)
        {
            _OralTestScheduleContext = OralTestScheduleContext;
            _context = db;
            _StudentApplicationContext = StudentApplicationContext;
            _feecontext = feecontext;
            _ScheduleReportContext = DomainModelContext;
        }


        public StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                List<long> studentlistof = new List<long>();
                StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;

                List<StudentApplication> lorg5 = new List<StudentApplication>();
                List<StudentApplication> Allname1 = new List<StudentApplication>();
                List<long> Allname2 = new List<long>();

                List<StudentApplication> Allname5 = new List<StudentApplication>();
                lorg5 = _OralTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id).ToList();

                studentlistof = lorg5.Select(t => t.pasr_id).ToList();

                List<long> Allname3 = new List<long>();
                Allname5 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.PASRAPS_ID == 787928).ToList();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.MI_Id) && d.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                {
                    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                    {
                        Allname3 = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" &&
                        studentlistof.Contains(t.PASA_Id)).Select(t => t.PASA_Id).ToList();

                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)
                        && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID).ToList();

                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }
                }
                else
                {

                    Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();

                    StudentDetailsDTO.studentDetails = Allname1.ToArray();
                }
                Array[] showdata = new Array[50];
                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.OralTestSchedule = Allname.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return StudentDetailsDTO;
        }

        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();
            List<OralTestScheduleDMO> lorg = new List<OralTestScheduleDMO>();
            lorg = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            OralTestScheduleDTO.OralTestSchedule = lorg.ToArray();

            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
            List<StudentApplication> lorg2 = new List<StudentApplication>();
            List<StudentApplication> lorgfinal = new List<StudentApplication>();
            lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            int j = 0;
            while (j < lorg1.Count())
            {
                lorg2.AddRange(_OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(lorg1[j].PASR_Id)).ToList());
                j++;
            }

            OralTestScheduleDTO.SelectedStudentDetails = lorg2.ToArray();
            OralTestScheduleDTO.OralTestSchedule = lorg.ToArray();

            return OralTestScheduleDTO;
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            //int ID1 = Convert.ToInt32(ID);
            StudentDetailsDTO StudentDetailsDTO = new StudentDetailsDTO();

            List<StudentApplication> lorg = new List<StudentApplication>();


            lorg = _OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id.Equals(ID)).ToList();



            StudentDetailsDTO.studentDetails = lorg.ToArray();

            return StudentDetailsDTO;
        }

        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();


            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();



            if (masters.Any())
            {

                int j = 0;
                while (j < masters.Count())
                {
                    //List<OralTestScheduleStudentInsertDMO> masters3 = new List<OralTestScheduleStudentInsertDMO>();
                    //masters3 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(masters[j].PAOTS_Id)).ToList();
                    var MM1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Single(t => t.PASR_Id == masters[j].PASR_Id);
                    MM1.PAOTSS_StatusFlag = 0;
                    _OralTestScheduleContext.Update(MM1);
                    _OralTestScheduleContext.SaveChanges();

                    j++;
                }

                List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();


                if (masters1.Any())
                {
                    _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }
            else
            {
                List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
                if (masters1.Any())
                {
                    _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                    _OralTestScheduleContext.SaveChanges();

                }
                else
                {

                }
            }


            return OralTestScheduleDTO;
        }

        public OralTestScheduleDTO OralTestScheduleDeletesStudentData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PASR_Id.Equals(OralTestScheduleDTO.PASR_Id) && t.PAOTS_Id.Equals(OralTestScheduleDTO.PAOTS_Id)).ToList();
            if (masters.Any())
            {
                masters.FirstOrDefault().PAOTSS_StatusFlag = 0;
                _OralTestScheduleContext.Update(masters);
                _OralTestScheduleContext.SaveChanges();
            }
            else
            {

            }
            return OralTestScheduleDTO;
        }
        public async Task<OralTestScheduleDTO> OralTestScheduleData(OralTestScheduleDTO mas)
        {

            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            mas.ASMAY_Id = Acdemic_preadmission;

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
           
            OralTestScheduleDTO vc = new OralTestScheduleDTO();
            //OralTestScheduleDMO MM = Mapper.Map<OralTestScheduleDMO>(mas);
            long paotss = 0;
            if (mas.PAOTS_Id > 0)
            {
                var schedulenew = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedulenew == 0)
                {
                    //added by 02/02/2017
                    OralTestScheduleDMO MM = new OralTestScheduleDMO();
                    MM.ASMAY_Id = mas.ASMAY_Id;
                    MM.MI_Id = mas.MI_Id;
                    MM.IVRMSTAUL_Id =Convert.ToInt32(mas.IVRMSTAUL_Id);
                    MM.PAOTS_EntryDate = indianTime;
                    MM.PAOTS_ScheduleDate = mas.PAOTS_ScheduleDate;
                    MM.PAOTS_ScheduleTime = mas.PAOTS_ScheduleTime;
                    MM.PAOTS_ScheduleTimeTo = mas.PAOTS_ScheduleTimeTo;
                    MM.PAOTS_ScheduleName = mas.PAOTS_ScheduleName;
                    MM.CreatedDate = indianTime;
                    MM.UpdatedDate = indianTime;
                    _OralTestScheduleContext.Add(MM);
                    _OralTestScheduleContext.SaveChanges();
                    mas.New_PAOTS_Id = MM.PAOTS_Id;

                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {

                            var MM1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Single(t => t.PASR_Id == mas.SelectedStudentData[j].PASR_Id && t.PAOTSS_StatusFlag == 1);

                            var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Single(t => t.PAOTS_Id == mas.PAOTS_Id);

                            var studentlist = _OralTestScheduleContext.StudentApplication.Where(t => t.pasr_id == mas.SelectedStudentData[j].PASR_Id).ToList();
                            //added by 02/02/2017                     

                            MM1.UpdatedDate = indianTime;
                            MM1.PAOTSS_StatusFlag = 2;
                            var n = _OralTestScheduleContext.Update(MM1);
                            _OralTestScheduleContext.SaveChanges();

                            OralTestScheduleStudentInsertDMO MM2 = new OralTestScheduleStudentInsertDMO();
                            MM2.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                            MM2.PAOTS_Id = mas.New_PAOTS_Id;
                            //added by 02/02/2017
                            MM2.CreatedDate = DateTime.Now;
                            MM2.UpdatedDate = DateTime.Now;
                            MM2.PAOTSS_StatusFlag = 1;
                            MM2.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                            if (mas.PAOTS_TPFlag == false)
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addmin = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddMinutes(addmin);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                MM2.PAOTSS_Time = finalstattim;
                                MM2.PAOTSS_Time_To = finalendtim;
                            }
                            else
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addhr = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddHours(addhr);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddHours(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                //var paotss_tm = oDate.AddHours(addhr);
                                //string finaltim = paotss_tm.ToString("H:mm");
                                MM2.PAOTSS_Time = finalstattim;
                                MM2.PAOTSS_Time_To = finalendtim;
                            }

                            var n1 = _OralTestScheduleContext.Add(MM2);
                            int n2 = _OralTestScheduleContext.SaveChanges();
                            if (mas.SelectedStudentData[j].vcmeeting == true)
                            {
                                vc = mas;
                                vc.PASR_Id = MM1.PASR_Id;
                                vc.PlannedDate = Convert.ToDateTime(mas.PAOTS_ScheduleDate);
                                vc.PlannedStartTime = MM2.PAOTSS_Time;
                                vc.PlannedEndTime = MM2.PAOTSS_Time_To;
                                vc.PAOTS_Id = mas.PAOTS_Id;
                                vc.MeetingTopic = mas.PAOTS_ScheduleName;
                                vc.meetingid = mas.PAOTS_ScheduleName + "-" + DateTime.Now.ToString() + j + "0" + j;
                                vc.meetingid = vc.meetingid.Replace(" ", "");
                                vc.meetingid = vc.meetingid.Replace(":", "-");
                                vc.meetingid = vc.meetingid.Replace("/", "-");
                                vc.meetingid = vc.meetingid.Replace(".", "-");
                                createvcmeeting(vc);
                            }
                            //if (n1 != null)
                            //{
                            //    Email Email = new Email(_context);
                            //    string m = Email.sendmail(mas.MI_Id, studentlist.FirstOrDefault().PASR_emailId, "ORAL_TEST_SCHEDULE", MM2.PASR_Id);
                            //    paotss = MM2.PAOTSS_Id;

                            //    SMS sms = new SMS(_context);
                            //    string sss = await sms.sendSms(mas.MI_Id, studentlist.FirstOrDefault().PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM2.PASR_Id);
                            //}
                            if (n2 > 0)
                            {
                                if (mas.SelectedStudentData[j].vcmeeting == false)
                                {
                                    Email Email = new Email(_context);
                                    string m = Email.sendmail(mas.MI_Id, studentlist.FirstOrDefault().PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                    SMS sms = new SMS(_context);
                                    string sss = await sms.sendSms(mas.MI_Id, studentlist.FirstOrDefault().PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                                    SMS smsS = new SMS(_context);
                                    string sssS = await smsS.sendSms(mas.MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                                    // SMS NEW TABLES CODE START
                                    //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);
                                    // SMS NEW TABLES CODE END
                                }
                                else if (mas.SelectedStudentData[j].vcmeeting == true)
                                {
                                    Email Email = new Email(_context);
                                    string m = Email.sendmail(mas.MI_Id, studentlist.FirstOrDefault().PASR_emailId, "VC_ORAL_TEST_RESCHEDULE", MM1.PASR_Id);
                                    SMS sms = new SMS(_context);
                                    string sss = await sms.sendSms(mas.MI_Id, studentlist.FirstOrDefault().PASR_MobileNo, "VC_ORAL_TEST_RESCHEDULE", MM1.PASR_Id);
                                }
                            }
                            j++;
                        }
                    }
                    int k = 0;
                    if (mas.SelectedStudentDataForEdit != null)
                    {
                        while (k < mas.SelectedStudentDataForEdit.Count())
                        {
                            OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentDataForEdit[k].PASR_Id;
                            MM1.PAOTS_Id = mas.PAOTS_Id;

                            Email Email = new Email(_context);
                            string m = Email.sendmail(mas.MI_Id, mas.SelectedStudentDataForEdit[k].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            SMS sms = new SMS(_context);
                            string sss = await sms.sendSms(mas.MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                            k++;
                        }
                    }                    
                    mas.PAOTS_Id = 0;
                }
                else
                {
                    mas.returnval = false;
                }
            }
            return mas;
        }
        public OralTestScheduleDTO createvcmeeting(OralTestScheduleDTO data)
        {
            try
            {
                var duplicatecheck = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_MeetingId == data.meetingid).ToList();
                if (duplicatecheck.Count() > 0)
                {
                    data.meetingid = data.meetingid + "-" + DateTime.Now.ToShortTimeString();
                    data.meetingid = data.meetingid.Replace(" ", "");
                    data.meetingid = data.meetingid.Replace(":", "-");
                    data.meetingid = data.meetingid.Replace("/", "-");
                    data.meetingid = data.meetingid.Replace(".", "-");
                }

                string meetingurl = "";
                //string meetingid = Getmetingid(data);
                var teamcredentials = _context.Institution.Where(c => c.MI_Id == data.MI_Id).ToList();
                //if (data.LMSLMEET_Id == 0)
                //{
                //string meetingid = Getmetingid(data);
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();
                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                remoteIpAddress = addr[addr.Length - 1].ToString();
                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                LMS_Live_MeetingDMO obj = new LMS_Live_MeetingDMO();
                obj.MI_Id = data.MI_Id;
                obj.LMSLMEET_MeetingId = data.meetingid;
                obj.LMSLMEET_PlannedDate = data.PlannedDate;
                obj.LMSLMEET_PlannedEndTime = data.PlannedEndTime;
                obj.LMSLMEET_PlannedStartTime = data.PlannedStartTime;
                obj.LMSLMEET_MeetingTopic = data.MeetingTopic;
                obj.LMSLMEET_PMACAddress = sMacAddress;
                obj.LMSLMEET_PIPAddress = myIP1;
                obj.User_Id = data.IVRMSTAUL_Id;
                obj.HRME_Id = 0;
                obj.LMSLMEET_CanOthersStartFlg = false;
                obj.LMSLMEET_StartedByUserId = 0;
                obj.LMSLMEET_CreatedBy = data.IVRMSTAUL_Id;
                obj.LMSLMEET_UpdatedBy = data.IVRMSTAUL_Id;
                obj.LMSLMEET_MeetingURL = meetingurl;
                obj.LMSLMEET_CreatedDate = DateTime.Now;
                obj.LMSLMEET_UpdatedDate = DateTime.Now;
                obj.LMSLMEET_ActiveFlg = true;
                _context.Add(obj);
                int res = _context.SaveChanges();
                data.LMSLMEET_Id = obj.LMSLMEET_Id;

                LMS_Live_Meeting_PAStudentDMO obj1 = new LMS_Live_Meeting_PAStudentDMO();
                obj1.LMSLMEET_Id = data.LMSLMEET_Id;
                obj1.PASR_Id = data.PASR_Id;
                obj1.LMSLMEETPASTD_ActiveFlg = true;
                obj1.LMSLMEETPASTD_CreatedDate = DateTime.Now;
                obj1.LMSLMEETPASTD_UpdatedDate = DateTime.Now;
                obj1.LMSLMEETPASTD_UpdatedBy = data.IVRMSTAUL_Id;
                obj1.LMSLMEETPASTD_CreatedBy = data.IVRMSTAUL_Id;
                _context.Add(obj1);
                int ress = _context.SaveChanges();
                if (res > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

                checkaddparticipants(data);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public async Task<ScheduleReportDTO> Getreportdetails(ScheduleReportDTO data)
        {
            try
            {
                string name = "";
                string IVRM_CLM_coloumn = "";

                using (var cmd = _ScheduleReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_schedulelist_students";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@schids",
                     SqlDbType.VarChar)
                    {
                        Value = data.schids
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {

                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Avialable" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.allreports = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public OralTestScheduleDTO checkaddparticipants(OralTestScheduleDTO data)
        {
            try
            {
                long studentmeeting = 0;
                var schedulelist = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id == data.PAOTS_Id).ToList();
                if (schedulelist.Count() > 0)
                {
                    var vcschedule = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_MeetingTopic == schedulelist.FirstOrDefault().PAOTS_ScheduleName && t.LMSLMEET_PlannedDate == schedulelist.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == schedulelist.FirstOrDefault().PAOTS_ScheduleTime).ToList();
                    if (vcschedule.Count > 0)
                    {
                        studentmeeting = vcschedule.FirstOrDefault().LMSLMEET_Id;
                    }
                }
                if (data.LMSLMEET_Id > 0)
                {
                    var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == studentmeeting).ToList();
                    if (deletstafflist.Count > 0)
                    {
                        foreach (var item in deletstafflist)
                        {
                            var stafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && t.User_Id == item.User_Id).ToList();
                            if (stafflist.Count == 0)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                sobj.User_Id = Convert.ToInt64(item.User_Id);
                                sobj.HRME_Id = Convert.ToInt64(item.HRME_Id);
                                sobj.LMSLMEETSTFOTH_CreatedBy = Convert.ToInt64(data.userid);
                                sobj.LMSLMEETSTFOTH_UpdatedBy = Convert.ToInt64(data.userid);
                                sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                _context.Add(sobj);
                            }

                        }
                    }
                }
                int res = _context.SaveChanges();
                if (res > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

    }
}
