﻿using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.COE;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.COE;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Services
{
    public class COEMailSMSImpl : Interfaces.COEMailSMSInterface
    {
        int MI_ID = 0;
        public COEContext _coe;
        public DomainModelMsSqlServerContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        public COEMailSMSImpl(COEContext coe, DomainModelMsSqlServerContext db, IHostingEnvironment hostingEnvironment)
        {
            _coe = coe;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }
        public COEReportDTO getdatanew(COEReportDTO data)
        {
            try
            {
                SendEmail(Convert.ToInt32(data.MI_Id), data.ASMAY_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public void getdata(int id)
        {
            COEReportDTO data = new COEReportDTO();
            data.MI_Id = id;

            try
            {
                MI_ID = id;
                long asmay_id = 0;
                asmay_id = _coe.AcademicYear.Where(t => t.MI_Id == MI_ID && t.ASMAY_From_Date <= DateTime.Today && t.ASMAY_To_Date >= DateTime.Today && t.Is_Active == true).Select(t => t.ASMAY_Id).FirstOrDefault();
                SendEmail(MI_ID, asmay_id);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        void SendEmail(int MI_ID, long ASMAY_Id)
        {
            List<COE_EventsDMO> arr = new List<COE_EventsDMO>();
            List<string> list = new List<string>();
            list.Add("Saturday");
            list.Add("Sunday");
            try
            {

                List<COE_EventsDMO> que1 = new List<COE_EventsDMO>();
                //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID)
                //&& Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date)
                //&& Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();                //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID)
                //&& Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date)
                //&& Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "COE_Auto_Scheduler_Trigger";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_ID) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                que1.Add(new COE_EventsDMO
                                {
                                    COEE_Id = Convert.ToInt32(dataReader["COEE_Id"]),
                                    COEE_ActiveFlag = Convert.ToBoolean(dataReader["COEE_ActiveFlag"]),
                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),
                                    ASMAY_Id = Convert.ToInt64(dataReader["ASMAY_Id"]),
                                    COEME_Id = Convert.ToInt32(dataReader["COEME_Id"]),
                                    COEE_RepeatFlag = Convert.ToBoolean(dataReader["COEE_RepeatFlag"]),
                                    COEE_ReminderSchedule = Convert.ToString(dataReader["COEE_ReminderSchedule"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                //var que1 = _coe.COE_EventsDMO.Where(a => a.MI_Id == MI_ID && a.COEE_ActiveFlag == true && a.ASMAY_Id == ASMAY_Id
                //&& (Convert.ToDateTime(a.COEE_ReminderDate.Value.Date) == Convert.ToDateTime(System.DateTime.Today.Date))
                //&& (a.COEE_SMSActiveFlag == true || a.COEE_MailActiveFlag == true)).Select(d => new COE_EventsDMO
                //{
                //    COEE_Id = d.COEE_Id,
                //    COEE_ActiveFlag = d.COEE_ActiveFlag,
                //    MI_Id = d.MI_Id,
                //    ASMAY_Id = d.ASMAY_Id,
                //    COEME_Id = d.COEME_Id,
                //    COEE_RepeatFlag = d.COEE_RepeatFlag,
                //    COEE_ReminderSchedule = d.COEE_ReminderSchedule

                //}).OrderBy(d => d.COEE_Id).ToList();



                if (que1.Count > 0)
                {
                    for (int i = 0; i <= que1.Count; i++)
                    {
                        var co_id = que1[i].COEE_Id;
                        if (que1[i].COEE_RepeatFlag == true)
                        {
                            if (que1[i].COEE_ReminderSchedule.Equals("Remainder Date"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.COEE_ReminderDate.Value.Date == DateTime.Now.Date && d.MI_Id == MI_ID && d.COEE_Id == co_id).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();

                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Daily"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Every Week (Mon – Fri)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && !list.Contains(DateTime.Now.DayOfWeek.ToString())).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Weekly (on Even Day)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate.Value.DayOfWeek.ToString() == DateTime.Now.DayOfWeek.ToString()).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();

                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Every 2 weeks (on Even Day)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id &&
                                d.COEE_EStartDate == DateTime.Now).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Monthly (on Even Day)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate == DateTime.Now).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Monthly (on Date)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate.Value.Day == DateTime.Now.Day).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Upto Start Date"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && DateTime.Now.Date >= d.COEE_ReminderDate.Value.Date && DateTime.Now <= d.COEE_EStartDate).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Upto End Date"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && DateTime.Now.Date >= d.COEE_ReminderDate.Value.Date && DateTime.Now.Date <= d.COEE_EEndDate.Value.Date).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                        }
                        else
                        {
                            arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id
                            && d.COEE_ReminderDate.Value.Date == DateTime.Now.Date).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        }


                        if (arr.Count > 0)
                        {
                            for (int m = 0; m < arr.Count; m++)
                            {
                                int coeid = Convert.ToInt32(arr[m].COEE_Id);
                                if (arr[m].COEE_StudentFlag == true)
                                {
                                    SEND_class_I(coeid, MI_ID);
                                }
                                if (arr[m].COEE_EmployeeFlag == true)
                                {
                                    SEND_staff(coeid, MI_ID);
                                }
                                if (arr[m].COEE_AlumniFlag == true)
                                {
                                    SEND_Alumini(coeid, MI_ID);
                                }
                                if (arr[m].COEE_OtherFlag == true)
                                {
                                    SEND_Other(coeid, MI_ID);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        void SendEmailbackup(int MI_ID)
        {
            List<COE_EventsDMO> arr = new List<COE_EventsDMO>();
            List<string> list = new List<string>();
            list.Add("Saturday");
            list.Add("Sunday");
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID)
                && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date)
                && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();


                // var que1 = _coe.COE_EventsDMO.Where(d => d.MI_Id == MI_ID && d.COEE_ActiveFlag == true).OrderBy(d => d.COEE_Id).ToList();
                var que1 = _coe.COE_EventsDMO.Where(d => d.MI_Id == MI_ID && d.COEE_ActiveFlag == true && d.ASMAY_Id == acd_Id).
                    Select(d => new COE_EventsDMO
                    {
                        COEE_Id = d.COEE_Id,
                        COEE_ActiveFlag = d.COEE_ActiveFlag,
                        MI_Id = d.MI_Id,
                        ASMAY_Id = d.ASMAY_Id,
                        COEME_Id = d.COEME_Id,
                        COEE_RepeatFlag = d.COEE_RepeatFlag,
                        COEE_ReminderSchedule = d.COEE_ReminderSchedule

                    }).OrderBy(d => d.COEE_Id).ToList();

                if (que1.Count > 0)
                {
                    for (int i = 0; i <= que1.Count; i++)
                    {
                        var co_id = que1[i].COEE_Id;

                        //var records = _coe.COE_Events_SMSMailPN_DMO.Where(a=>a.COEE_Id== co_id).Distinct().ToList();
                        //if (records.Count > 0)
                        //{
                        //    var duplicate = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id.Equals(co_id)).Distinct().ToList();
                        //    if (duplicate.Count == 0)
                        //    {
                        //        if (que1[i].COEE_RepeatFlag == true)
                        //        {
                        //            if (que1[i].COEE_ReminderSchedule.Equals("Remainder Date"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.COEE_ReminderDate.Value.Date == DateTime.Now.Date && d.MI_Id == MI_ID && d.COEE_Id == co_id).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();

                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Daily"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Every Week (Mon – Fri)"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && !list.Contains(DateTime.Now.DayOfWeek.ToString())).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Weekly (on Even Day)"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate.Value.DayOfWeek.ToString() == DateTime.Now.DayOfWeek.ToString()).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();

                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Every 2 weeks (on Even Day)"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id &&
                        //                d.COEE_EStartDate == DateTime.Now).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Monthly (on Even Day)"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate == DateTime.Now).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Monthly (on Date)"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate.Value.Day == DateTime.Now.Day).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Upto Start Date"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && DateTime.Now.Date >= d.COEE_ReminderDate.Value.Date && DateTime.Now <= d.COEE_EStartDate).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //            else if (que1[i].COEE_ReminderSchedule.Equals("Upto End Date"))
                        //            {
                        //                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && DateTime.Now.Date >= d.COEE_ReminderDate.Value.Date && DateTime.Now.Date <= d.COEE_EEndDate.Value.Date).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //            }
                        //        }
                        //        else
                        //        {
                        //            arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_ReminderDate.Value.Date == DateTime.Now.Date).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        //        }

                        //        if (arr.Count > 0)
                        //        {
                        //            for (int m = 0; m < arr.Count; m++)
                        //            {
                        //                int coeid = Convert.ToInt32(arr[m].COEE_Id);
                        //                if (arr[m].COEE_StudentFlag == true)
                        //                {
                        //                    SEND_class_I(coeid, MI_ID);
                        //                }
                        //                if (arr[m].COEE_EmployeeFlag == true)
                        //                {
                        //                    SEND_staff(coeid, MI_ID);
                        //                }
                        //                if (arr[m].COEE_AlumniFlag == true)
                        //                {
                        //                    SEND_Alumini(coeid, MI_ID);
                        //                }
                        //                if (arr[m].COEE_OtherFlag == true)
                        //                {
                        //                    SEND_Other(coeid, MI_ID);
                        //                }
                        //            }
                        //        }

                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }
                        //}
                        //else
                        //{
                        if (que1[i].COEE_RepeatFlag == true)
                        {
                            if (que1[i].COEE_ReminderSchedule.Equals("Remainder Date"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.COEE_ReminderDate.Value.Date == DateTime.Now.Date && d.MI_Id == MI_ID && d.COEE_Id == co_id).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();

                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Daily"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Every Week (Mon – Fri)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && !list.Contains(DateTime.Now.DayOfWeek.ToString())).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Weekly (on Even Day)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate.Value.DayOfWeek.ToString() == DateTime.Now.DayOfWeek.ToString()).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();

                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Every 2 weeks (on Even Day)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id &&
                                d.COEE_EStartDate == DateTime.Now).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Monthly (on Even Day)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate == DateTime.Now).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Monthly (on Date)"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_EStartDate.Value.Day == DateTime.Now.Day).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Upto Start Date"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && DateTime.Now.Date >= d.COEE_ReminderDate.Value.Date && DateTime.Now <= d.COEE_EStartDate).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                            else if (que1[i].COEE_ReminderSchedule.Equals("Upto End Date"))
                            {
                                arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && DateTime.Now.Date >= d.COEE_ReminderDate.Value.Date && DateTime.Now.Date <= d.COEE_EEndDate.Value.Date).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                            }
                        }
                        else
                        {
                            arr = _coe.COE_EventsDMO.Where(d => d.COEE_ActiveFlag == true && d.MI_Id == MI_ID && d.COEE_Id == co_id && d.COEE_ReminderDate.Value.Date == DateTime.Now.Date).Select(d => new COE_EventsDMO { COEE_Id = d.COEE_Id, COEE_StudentFlag = d.COEE_StudentFlag, COEE_EmployeeFlag = d.COEE_EmployeeFlag, COEE_AlumniFlag = d.COEE_AlumniFlag, COEE_OtherFlag = d.COEE_OtherFlag }).ToList();
                        }
                        if (arr.Count > 0)
                        {
                            for (int m = 0; m < arr.Count; m++)
                            {
                                int coeid = Convert.ToInt32(arr[m].COEE_Id);
                                if (arr[m].COEE_StudentFlag == true)
                                {
                                    SEND_class_I(coeid, MI_ID);
                                }
                                if (arr[m].COEE_EmployeeFlag == true)
                                {
                                    SEND_staff(coeid, MI_ID);
                                }
                                if (arr[m].COEE_AlumniFlag == true)
                                {
                                    SEND_Alumini(coeid, MI_ID);
                                }
                                if (arr[m].COEE_OtherFlag == true)
                                {
                                    SEND_Other(coeid, MI_ID);
                                }
                            }
                        }
                        //}

                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        void SEND_Alumini(int coeid, int MI_ID)
        {
            var que3 = _coe.COE_Events_ClassesDMO.Where(d => d.COEE_Id == coeid).OrderBy(d => d.ASMCL_Id).ToList();

            if (que3.Count > 0)
            {
                for (int j = 0; j < que3.Count; j++)
                {
                    string stud_class = que3[j].ASMCL_Id.ToString();
                    var que6 = (from m in _db.Adm_M_Student
                                from n in _db.School_Adm_Y_StudentDMO
                                where m.AMST_Id == n.AMST_Id && m.MI_Id == MI_ID && m.AMST_SOL.Equals("L")
                                select m).ToList();

                    if (que6.Count > 0)
                    {
                        for (int k = 0; k < que6.Count; k++)
                        {
                            string amst_FName = que6[k].AMST_FirstName.ToString();
                            string amst_LName = que6[k].AMST_LastName.ToString();
                            string amst_mobile = Convert.ToString(que6[k].AMST_MobileNo);
                            string amst_Email = Convert.ToString(que6[k].AMST_emailId);
                            long amst_Admno = Convert.ToInt64(que6[k].AMST_Id);

                            var que7 = _coe.COE_EventsDMO.Where(d => d.MI_Id == MI_ID && d.COEE_ActiveFlag == true && d.COEE_Id == coeid).OrderBy(d => d.COEE_Id).ToList();
                            if (que7.Count > 0)
                            {
                                bool mail_active = que7[0].COEE_MailActiveFlag;
                                bool sms_active = que7[0].COEE_SMSActiveFlag;
                                string sms_message = que7[0].COEE_SMSMessage.ToString();
                                string Email_header = que7[0].COEE_MailHeader.ToString();
                                string Email_subject = que7[0].COEE_MailSubject.ToString();
                                string Email_messagedata = que7[0].COEE_Mail_Message.ToString();
                                // string Email_message = Email_messagedata.Replace("\n", "<br/>");
                                string Email_message = "";
                                if (Email_messagedata.Contains("\\n"))
                                {
                                    Email_message = Email_messagedata.Replace("\\n", "<br/>");
                                }
                                else
                                {
                                    Email_message = Email_messagedata.Replace("\n", "<br/>");
                                }
                                string Email_footer = que7[0].COEE_MailFooter.ToString();
                                string type = "Student";
                                string Template = "COE";

                                if ((sms_message == "" || sms_message == null) && (Email_message == "" || Email_message == null))
                                {
                                    List<long> coeme_ids = new List<long>();
                                    if (que7.Count > 0)
                                    {
                                        foreach (var item in que7)
                                        {
                                            coeme_ids.Add(item.COEME_Id);
                                        }
                                    }
                                    var eventnamelist = _coe.COE_Master_EventsDMO.Where(t => t.MI_Id == MI_ID && coeme_ids.Contains(t.COEME_Id)).Distinct().FirstOrDefault();

                                    string eventname = eventnamelist.COEME_EventName.ToString();

                                    DateTime Sdate = Convert.ToDateTime(que7[0].COEE_EStartDate);

                                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                    DateTime indianTime = TimeZoneInfo.ConvertTime(Sdate, INDIAN_ZONE);

                                    if (sms_active == true)
                                    {
                                        if (amst_mobile != "" && amst_mobile != null)
                                        {
                                            Send_SMS_AluminiStudent_ForEmpty(amst_FName, amst_mobile, amst_Admno, eventname, indianTime, MI_ID, Template, coeid, type);
                                        }
                                    }
                                    if (mail_active == true)
                                    {
                                        if (amst_Email != "" && amst_Email != null)
                                        {
                                            Send_Email1_AluminiStudent_ForEmpty(amst_FName, amst_Admno, Email_header, Email_subject, eventname, amst_Email, Email_footer, indianTime, MI_ID, Template, coeid, type);
                                        }
                                    }
                                }
                                else
                                {
                                    if (sms_active == true)
                                    {
                                        if (amst_mobile != "" && amst_mobile != null)
                                        {
                                            Send_SMS(amst_FName, amst_mobile, amst_Admno, sms_message, MI_ID, type, Template, coeid);
                                        }
                                    }
                                    if (mail_active == true)
                                    {
                                        if (amst_Email != "" && amst_Email != null)
                                        {
                                            Send_Email(amst_FName, amst_Admno, Email_header, Email_subject, Email_message, amst_Email, Email_footer, MI_ID, type, Template, coeid);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        void SEND_staff(int coeid, int MI_ID)
        {
            var que3 = _coe.COE_Events_EmployeesDMO.Where(d => d.COEE_Id == coeid).ToList();

            if (que3.Count > 0)
            {
                for (int j = 0; j < que3.Count; j++)
                {
                    long emp_type = que3[j].HRMGT_Id;
                    //   var que66 =  _db.HR_Master_Employee_DMO.Where(d=>d.HRME_LeftFlag==false && d.HRMGT_Id == emp_type).Select(t=>t.HRME_Id).ToList();
                    var que6 = (from d in _db.HR_Master_Employee_DMO
                                from b in _db.Multiple_Email_DMO
                                from c in _db.Multiple_Mobile_DMO
                                where (d.HRME_Id == b.HRME_Id && d.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && d.MI_Id == MI_ID && d.HRME_ActiveFlag == true && d.HRME_LeftFlag == false && d.HRMD_Id == emp_type)
                                select new BirthDayDTO
                                {
                                    HRME_Id = d.HRME_Id,
                                    employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                    //HRME_EmployeeLastName = string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? " ": d.HRME_EmployeeLastName,
                                    // HRME_EmployeeLastName = d.HRME_EmployeeLastName==null ? " " : d.HRME_EmployeeLastName,
                                    HRME_MobileNo = c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                                    HRME_EmailId = (string.IsNullOrEmpty(b.HRMEM_EmailId) ? "" : b.HRMEM_EmailId),
                                    //HRME_MobileNo = d.HRME_MobileNo,
                                    // HRME_EmailId= d.HRME_EmailId
                                }).Distinct().ToList();

                    if (que6.Count > 0)
                    {
                        for (int k = 0; k < que6.Count; k++)
                        {
                            string amst_FName = que6[k].employeeName;
                            string amst_mobile = Convert.ToString(que6[k].HRME_MobileNo);
                            string amst_Email = Convert.ToString(que6[k].HRME_EmailId);
                            long amst_Admno = Convert.ToInt64(que6[k].HRME_Id);

                            var que7 = _coe.COE_EventsDMO.Where(d => d.MI_Id == MI_ID && d.COEE_ActiveFlag == true && d.COEE_Id == coeid).OrderBy(d => d.COEE_Id).ToList();
                            if (que7.Count > 0)
                            {
                                bool mail_active = que7[0].COEE_MailActiveFlag;
                                bool sms_active = que7[0].COEE_SMSActiveFlag;
                                string sms_message = que7[0].COEE_SMSMessage.ToString();
                                string Email_header = que7[0].COEE_MailHeader.ToString();
                                string Email_subject = que7[0].COEE_MailSubject.ToString();
                                string Email_messagedata = que7[0].COEE_Mail_Message.ToString();
                                //string Email_message = Email_messagedata.Replace("\n", "<br/>");
                                string Email_message = "";
                                if (Email_messagedata.Contains("\\n"))
                                {
                                    Email_message = Email_messagedata.Replace("\\n", "<br/>");
                                }
                                else 
                                {
                                    Email_message = Email_messagedata.Replace("\n", "<br/>");
                                }
                                string Email_footer = que7[0].COEE_MailFooter.ToString();
                                string type = "Employee";
                                string Template = "COE";


                                if ((sms_message == "" || sms_message == null) && (Email_message == "" || Email_message == null))
                                {
                                    List<long> coeme_ids = new List<long>();
                                    if (que7.Count > 0)
                                    {
                                        foreach (var item in que7)
                                        {
                                            coeme_ids.Add(item.COEME_Id);
                                        }
                                    }
                                    var eventnamelist = _coe.COE_Master_EventsDMO.Where(t => t.MI_Id == MI_ID && coeme_ids.Contains(t.COEME_Id)).Distinct().FirstOrDefault();

                                    string eventname = eventnamelist.COEME_EventName.ToString();
                                    DateTime Sdate = Convert.ToDateTime(que7[0].COEE_EStartDate);
                                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                    DateTime indianTime = TimeZoneInfo.ConvertTime(Sdate, INDIAN_ZONE);

                                    if (sms_active == true)
                                    {
                                        if (amst_mobile != "" && amst_mobile != null)
                                        {
                                            Send_SMS_Employee_ForEmpty(amst_FName, amst_mobile, amst_Admno, eventname, indianTime, MI_ID, type, Template, coeid);
                                        }
                                    }
                                    if (mail_active == true)
                                    {
                                        if (amst_Email != "" && amst_Email != null)
                                        {
                                            Send_Email1_Employee_ForEmpty(amst_FName, amst_Admno, Email_header, Email_subject, eventname, indianTime, amst_Email, Email_footer, MI_ID, type, Template, coeid);
                                        }

                                    }
                                }
                                else
                                {
                                    if (sms_active == true)
                                    {
                                        if (amst_mobile != "" && amst_mobile != null)
                                        {
                                            Send_SMS(amst_FName, amst_mobile, amst_Admno, sms_message, MI_ID, type, Template, coeid);
                                        }

                                    }
                                    if (mail_active == true)
                                    {
                                        if (amst_Email != "" && amst_Email != null)
                                        {
                                            Send_Email(amst_FName, amst_Admno, Email_header, Email_subject, Email_message, amst_Email, Email_footer, MI_ID, type, Template, coeid);
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        void SEND_class_I(int coeid, int MI_ID)
        {
            var que3 = _coe.COE_Events_ClassesDMO.Where(d => d.COEE_Id == coeid).OrderBy(d => d.ASMCL_Id).ToList();
            var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

            if (que3.Count > 0)
            {
                for (int j = 0; j < que3.Count; j++)
                {
                    try
                    {
                        string stud_class = que3[j].ASMCL_Id.ToString();
                        var que6 = (from m in _db.Adm_M_Student
                                    from n in _db.School_Adm_Y_StudentDMO
                                    where m.AMST_Id == n.AMST_Id && m.MI_Id == MI_ID && m.AMST_SOL.Equals("S") && n.ASMCL_Id == Convert.ToInt64(stud_class)
                                    && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMAY_Id == acd_Id
                                    select m).ToList();

                        if (que6.Count > 0)
                        {
                            for (int k = 0; k < que6.Count; k++)
                            {
                                try
                                {
                                    string amst_FName = que6[k].AMST_FirstName.ToString();
                                    string amst_LName = que6[k].AMST_LastName.ToString();
                                    string amst_MName = que6[k].AMST_MiddleName.ToString();
                                    string amst_mobile = Convert.ToString(que6[k].AMST_MobileNo);
                                    string amst_Email = Convert.ToString(que6[k].AMST_emailId);
                                    long amst_Admno = Convert.ToInt64(que6[k].AMST_Id);
                                    amst_FName = amst_FName + (string.IsNullOrEmpty(amst_MName) ? "" : ' ' + amst_MName) + (string.IsNullOrEmpty(amst_LName) ? "" : ' ' + amst_LName);
                                    var que7 = _coe.COE_EventsDMO.Where(d => d.MI_Id == MI_ID && d.COEE_ActiveFlag == true && d.COEE_Id == coeid).OrderBy(d => d.COEE_Id).ToList();
                                    if (que7.Count > 0)
                                    {
                                        bool mail_active = que7[0].COEE_MailActiveFlag;
                                        bool sms_active = que7[0].COEE_SMSActiveFlag;
                                        string sms_message = que7[0].COEE_SMSMessage.ToString();
                                        string Email_header = que7[0].COEE_MailHeader.ToString();
                                        string Email_subject = que7[0].COEE_MailSubject.ToString();
                                        // string Email_message = que7[0].COEE_Mail_Message.ToString();
                                        string Email_messagedata = que7[0].COEE_Mail_Message.ToString();
                                        // string Email_message = Email_messagedata.Replace("\\n", "\n");
                                        string Email_message = "";
                                        if (Email_messagedata.Contains("\\n"))
                                        {
                                            Email_message = Email_messagedata.Replace("\\n", "<br/>");
                                        }
                                        else
                                        {
                                            Email_message = Email_messagedata.Replace("\n", "<br/>");
                                        }
                                        
                                        string Email_footer = que7[0].COEE_MailFooter.ToString();
                                        string type = "Student";
                                        string Template = "COE";

                                        if ((sms_message == "" || sms_message == null) && (Email_message == "" || Email_message == null))
                                        {
                                            List<long> coeme_ids = new List<long>();
                                            if (que7.Count > 0)
                                            {
                                                foreach (var item in que7)
                                                {
                                                    coeme_ids.Add(item.COEME_Id);
                                                }
                                            }
                                            var eventnamelist = _coe.COE_Master_EventsDMO.Where(t => t.MI_Id == MI_ID && coeme_ids.Contains(t.COEME_Id)).Distinct().FirstOrDefault();

                                            string eventname = eventnamelist.COEME_EventName.ToString();
                                            //string startdate = que7[0].COEE_EStartDate.ToString();

                                            DateTime Sdate = Convert.ToDateTime(que7[0].COEE_EStartDate);

                                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                            DateTime indianTime = TimeZoneInfo.ConvertTime(Sdate, INDIAN_ZONE);

                                            if (sms_active == true)
                                            {
                                                if (amst_mobile != null && amst_mobile != "")
                                                {
                                                    Send_SMS_Student_ForEmpty(amst_FName, amst_mobile, amst_Admno, eventname, indianTime, MI_ID, type, Template, coeid);
                                                }

                                            }
                                            if (mail_active == true)
                                            {
                                                if (amst_Email != "" && amst_Email != null)
                                                {
                                                    Send_Email1_Student_ForEmpty(amst_FName, amst_Admno, Email_header, Email_subject, eventname, indianTime, amst_Email, Email_footer, MI_ID, type, Template, coeid);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (sms_active == true)
                                            {
                                                if (amst_mobile != null && amst_mobile != "")
                                                {
                                                    Send_SMS(amst_FName, amst_mobile, amst_Admno, sms_message, MI_ID, type, Template, coeid);
                                                }
                                            }
                                            if (mail_active == true)
                                            {
                                                if (amst_Email != "" && amst_Email != null)
                                                {
                                                    Send_Email(amst_FName, amst_Admno, Email_header, Email_subject, Email_message, amst_Email, Email_footer, MI_ID, type, Template, coeid);
                                                }
                                            }

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }

        }

        void SEND_Other(int coeid, int MI_ID)
        {
            var que3 = _coe.COE_Events_OthersDMO.Where(d => d.COEE_Id == coeid).OrderBy(d => d.COEEO_Id).ToList();

            if (que3.Count > 0)
            {

                for (int k = 0; k < que3.Count; k++)
                {
                    string amst_FName = que3[k].COEEO_Name;
                    string amst_mobile = Convert.ToString(que3[k].COEEO_MobileNo);
                    string amst_Email = Convert.ToString(que3[k].COEEO_Emailid);

                    var que7 = _coe.COE_EventsDMO.Where(d => d.MI_Id == MI_ID && d.COEE_ActiveFlag == true && d.COEE_Id == coeid).OrderBy(d => d.COEE_Id).ToList();
                    if (que7.Count > 0)
                    {
                        bool mail_active = que7[0].COEE_MailActiveFlag;
                        bool sms_active = que7[0].COEE_SMSActiveFlag;
                        string sms_message = que7[0].COEE_SMSMessage.ToString();
                        string Email_header = que7[0].COEE_MailHeader.ToString();
                        string Email_subject = que7[0].COEE_MailSubject.ToString();
                        // string Email_message = que7[0].COEE_Mail_Message.ToString();
                        string Email_messagedata = que7[0].COEE_Mail_Message.ToString();
                        // string Email_message = Email_messagedata.Replace("\n", "<br/>");
                        string Email_message = "";
                        if (Email_messagedata.Contains("\\n"))
                        {
                            Email_message = Email_messagedata.Replace("\\n", "<br/>");
                        }
                        else
                        {
                            Email_message = Email_messagedata.Replace("\n", "<br/>");
                        }
                        string Email_footer = que7[0].COEE_MailFooter.ToString();
                        string Template = "COE";


                        if ((sms_message == "" || sms_message == null) && (Email_message == "" || Email_message == null))
                        {
                            List<long> coeme_ids = new List<long>();
                            if (que7.Count > 0)
                            {
                                foreach (var item in que7)
                                {
                                    coeme_ids.Add(item.COEME_Id);
                                }
                            }
                            var eventnamelist = _coe.COE_Master_EventsDMO.Where(t => t.MI_Id == MI_ID && coeme_ids.Contains(t.COEME_Id)).Distinct().FirstOrDefault();

                            string eventname = eventnamelist.COEME_EventName.ToString();
                            //string startdate = que7[0].COEE_EStartDate.ToString();
                            DateTime Sdate = Convert.ToDateTime(que7[0].COEE_EStartDate);

                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime indianTime = TimeZoneInfo.ConvertTime(Sdate, INDIAN_ZONE);

                            if (sms_active == true)
                            {
                                if (amst_mobile != "" && amst_mobile != null)
                                {
                                    Send_SMS_Others_ForEmpty(amst_FName, amst_mobile, eventname, indianTime, MI_ID, Template, coeid);
                                }

                            }
                            if (mail_active == true)
                            {
                                if (amst_Email != "" && amst_Email != null)
                                {
                                    Send_Email1_Others_ForEmpty(amst_FName, eventname, indianTime, amst_Email, MI_ID, Template, coeid, Email_subject);
                                }
                            }
                        }
                        else
                        {
                            if (sms_active == true)
                            {
                                if (amst_mobile != "" && amst_mobile != null)
                                {
                                    Send_SMS1(amst_FName, amst_mobile, sms_message, MI_ID, Template, coeid);
                                }
                            }
                            if (mail_active == true)
                            {
                                if (amst_Email != "" && amst_Email != null)
                                {
                                    Send_Email1(amst_FName, Email_message, amst_Email, MI_ID, Template, coeid, Email_subject);
                                }
                            }
                        }

                    }
                }
            }

        }

        public string Send_SMS(string amst_FName, string amst_mobile, long amst_Admno, string sms_message, int MID, string type, string Template, int coeid)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                sms = sms_message;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = sms_message + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "BIRTHDAY_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = amst_Admno
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                          SqlDbType.VarChar)
                        {
                            Value = type
                        });
                        cmd.Parameters.Add(new SqlParameter("@coe_id",
                          SqlDbType.VarChar)
                        {
                            Value = coeid
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

                    for (int j = 0; j < variables.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(variables[j].Value, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;


                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    // string url = "http://trans.kapsystem.com/api/web2sms.php?workingkey=Ada954b9c07a35a318e0207b6c7f97f0d&sender=JSHSCH&to=PHNO&message=MESSAGE";
                    string PHNO = amst_mobile.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = type;
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    var countrow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                    //    if (countrow.Count > 0)
                    //    {
                    //        var updatesmsflag = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();

                    //        updatesmsflag.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        updatesmsflag.COEESMSMPNSMS_Flg = true;
                    //        _coe.Update(updatesmsflag);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        COE_Events_SMSMailPN_DMO obj1 = new COE_Events_SMSMailPN_DMO();

                    //        obj1.COEE_Id = coeid;
                    //        obj1.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        obj1.COEESMSMPNSMS_Flg = true;
                    //        _coe.Add(obj1);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //}
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_Email(string amst_FName, long amst_Admno, string Email_header, string Email_subject, string Email_message, string amst_Email, string Email_footer, int MID, string type, string Template, int coeid)
        {
            //try
            //{
            //    string accountname = "";
            //    string accesskey = "";
            //    string Mailmsg = "";

            //    string date1 = "";

            //   ////

            //    Dictionary<string, string> val = new Dictionary<string, string>();

            //    var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

            //    if (template.Count == 0)
            //    {
            //        return "Email Template not Mapped to the selected Institution";
            //    }

            //    var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

            //    var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

            //    var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


            //    Mailmsg = Email_message;


            //    string result = "";
            //    List<Match> variables = new List<Match>();

            //    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
            //    {
            //        variables.Add(match);
            //    }

            //    if (variables.Count == 0)
            //    {
            //        Mailmsg = Email_message;
            //    }
            //    else
            //    {
            //        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            //        {
            //            cmd.CommandText = "BIRTHDAY_PARAMETER_chikkatti";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add(new SqlParameter("@UserID",
            //                SqlDbType.BigInt)
            //            {
            //                Value = amst_Admno
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@template",
            //               SqlDbType.VarChar)
            //            {
            //                Value = "COE"
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@type",
            //                SqlDbType.VarChar)
            //            {
            //                Value = type
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@coe_id",
            //               SqlDbType.VarChar)
            //            {
            //                Value = coeid
            //            });



            //            if (cmd.Connection.State != ConnectionState.Open)
            //                cmd.Connection.Open();

            //            try
            //            {
            //                using (var dataReader = cmd.ExecuteReader())
            //                {
            //                    while (dataReader.Read())
            //                    {
            //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                        {
            //                            dataRow.Add(
            //                                dataReader.GetName(iFiled),
            //                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                            );
            //                            var datatype = dataReader.GetFieldType(iFiled);
            //                            if (datatype.Name == "DateTime")
            //                            {
            //                                var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
            //                                val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
            //                            }
            //                            else
            //                            {
            //                                val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                return ex.Message;
            //            }

            //        }


            //        for (int j = 0; j < ParamaetersName.Count; j++)
            //        {
            //            for (int p = 0; p < val.Count; p++)
            //            {
            //                if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
            //                {
            //                    result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
            //                    Mailmsg = result;
            //                }
            //            }
            //        }


            //        Mailmsg = result;
            //    }



            //    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            //    alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

            //    if (alldetails.Count > 0)
            //    {
            //        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
            //         //string SendingEmail = "Vapsregister1@gmail.com";
            //        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
            //       // string SendingEmailPassword = "vaps@123";


            //        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
            //        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
            //        string Subject = Email_subject;
            //        string mailcc = alldetails[0].IVRM_mailcc;
            //        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
            //        {
            //            //string Attechement = alldetails[0].IVRMMD_Attechement.ToString();

            //            string Attechement = "https://chikkatti.blob.core.windows.net/files/6/COE_Images_Videos/7d3f12fe-5240-40da-824b-28e3a8b376c7.jpg";
            //        }


            //        //Sending mail using SendGrid API.
            //        //Date:07-02-2017.

            //        var message = new SendGridMessage();
            //        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
            //        message.Subject = Subject;
            //        //var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
            //        //if (img.Count > 0)
            //        //{
            //        //    for (int i = 0; i < img.Count; i++)
            //        //    {
            //        //        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
            //        //        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
            //        //        Stream stream = response.GetResponseStream();
            //        //        message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
            //        //    }
            //        //}
            //        ////  string Attechementt = "https://chikkatti.blob.core.windows.net/files/6/COE_Images_Videos/7d3f12fe-5240-40da-824b-28e3a8b376c7.jpg";
            //        //string Attechementt = "https://dcampusstrg.blob.core.windows.net/files/17/Trnsportdocuments/9d419e05-719d-460d-8ea8-c6bfb1fbf4bf.png";
            //        //System.Net.HttpWebRequest request = System.Net.WebRequest.Create(Attechementt) as HttpWebRequest;
            //        //System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
            //        //Stream stream = response.GetResponseStream();

            //        //message.AddAttachment(stream.ToString(), Attechementt.ToString());


            //        if (mailcc != null && mailcc != "")
            //        {
            //            string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

            //            if (mail_id.Length > 0)
            //            {
            //                for (int i = 0; i < mail_id.Length; i++)
            //                {
            //                    message.AddBcc(mail_id[i]);
            //                }

            //            }
            //        }
            //     message.AddTo("punithnmsd2@gmail.com");
            //        message.AddTo("kanavisanjeev@gmail.com");


            //        //var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
            //        //if (vido.Count > 0)
            //        //{

            //        //    for (int i = 0; i < vido.Count; i++)
            //        //    {
            //        //        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
            //        //        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
            //        //        Stream stream = response.GetResponseStream();
            //        //        message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
            //        //    }
            //        //}

            //        string name = "";
            //        if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
            //        {
            //            //var query1 = _db.HR_Master_Employee_DMO.Single(d => d.HRME_Id == amst_Admno);
            //            var query1 = (from d in _db.HR_Master_Employee_DMO
            //                          where (d.HRME_LeftFlag == false && d.HRME_Id == amst_Admno)
            //                          select new BirthDayDTO
            //                          {
            //                              HRME_Id = d.HRME_Id,
            //                              employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)

            //                          }).Distinct().ToList();

            //            name = query1.FirstOrDefault().employeeName;

            //            date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

            //        }
            //        else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
            //        {
            //            var query1 = _db.Adm_M_Student.Where(d => d.AMST_Id == amst_Admno).Select(i => new Adm_M_Student { AMST_FirstName = i.AMST_FirstName, AMST_MiddleName = i.AMST_MiddleName, AMST_LastName = i.AMST_LastName }).ToList();
            //            name = query1.FirstOrDefault().AMST_FirstName + ' ' + query1.FirstOrDefault().AMST_MiddleName ?? " " + ' ' + query1.FirstOrDefault().AMST_LastName;

            //            date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");


            //        }
            //        //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
            //        //{
            //        //    message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);


            //        //    message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);



            //        //}
            //        //else
            //        //{

            //        //    message.HtmlContent = Mailmsg;
            //        //}


            //        if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
            //        {
            //            message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
            //            message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);

            //            var imgrepl = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
            //            if (imgrepl.Count > 0)
            //            {
            //                message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bCOE_IMAGE\b", imgrepl[0].COEEI_Images, RegexOptions.IgnoreCase);
            //            }
            //        }
            //        else
            //        {
            //            message.HtmlContent = Mailmsg;
            //        }
            //        //message.HtmlContent = Mailmsg;
            //        if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
            //        {
            //            var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
            //            //var client = new SendGridClient("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");

            //            client.SendEmailAsync(message).Wait();


            //            // message.AddTo(Email);
            //            //message.HtmlContent = Mailmsg;
            //            //var client = new SendGridClient("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");

            //            //client.SendEmailAsync(message).Wait();

            //        }
            //        else
            //        {
            //            return "Sendgrid key is not available";
            //        }

            //        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            //        {
            //            var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

            //            var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

            //            var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

            //            cmd.CommandText = "IVRM_Email_Outgoing_1";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add(new SqlParameter("@EmailId",
            //                SqlDbType.NVarChar)
            //            {
            //                Value = amst_Email
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@Message",
            //               SqlDbType.NVarChar)
            //            {
            //                Value = Mailmsg
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@module",
            //            SqlDbType.VarChar)
            //            {
            //                Value = "Calendar of Event"
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@MI_Id",
            //           SqlDbType.BigInt)
            //            {
            //                Value = MID
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@type",
            //             SqlDbType.VarChar)
            //            {
            //                Value = type
            //            });

            //            if (cmd.Connection.State != ConnectionState.Open)
            //                cmd.Connection.Open();

            //            try
            //            {
            //                using (var dataReader = cmd.ExecuteReader())
            //                {
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                return ex.Message;
            //            }
            //        }
            //    }

            //    //var updaterow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

            //    //if (updaterow.Count > 0)
            //    //{
            //    //    var updaterow2 = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();
            //    //    updaterow2.COEESMSMPNMail_Flg = true;
            //    //    updaterow2.COEESMSMPNMail_Date = DateTime.Now.Date;

            //    //    _coe.Update(updaterow2);
            //    //    int row = _coe.SaveChanges();
            //    //}
            //    //else
            //    //{
            //    //    COE_Events_SMSMailPN_DMO obj2 = new COE_Events_SMSMailPN_DMO();
            //    //    obj2.COEESMSMPNMail_Flg = true;
            //    //    obj2.COEESMSMPNMail_Date = DateTime.Now.Date;

            //    //    _coe.Add(obj2);
            //    //    int row = _coe.SaveChanges();
            //    //}

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return ex.Message;
            //}




            try
            {
                string Mailmsg = "";
                string date1 = "";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                Mailmsg = Email_message;
                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = Email_message;
                }
                else
                {
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "BIRTHDAY_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@UserID",
                    //        SqlDbType.BigInt)
                    //    {
                    //        Value = amst_Admno
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@template",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = "COE"
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@type",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = type
                    //    });


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "BIRTHDAY_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = amst_Admno
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = "COE"
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                            SqlDbType.VarChar)
                        {
                            Value = type
                        });
                        cmd.Parameters.Add(new SqlParameter("@coe_id",
                           SqlDbType.VarChar)
                        {
                            Value = coeid
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
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.
                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    // if (Template != "COE")
                    // {
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
                    //}
                    // message.AddTo("rakeshrd307@gmail.com");
                    //message.AddTo("punithnmsd2@gmail.com");
                 
                    message.AddTo(amst_Email);
                    //message.AddTo("kanavisanjeev@gmail.com");
                    //message.AddTo("punithnmsd2@gmail.com");
                    //var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    //if (img.Count > 0)
                    //{
                    //    for (int i = 0; i < img.Count; i++)
                    //    {
                    //        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                    //        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                    //        Stream stream = response.GetResponseStream();
                    //        message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
                    //    }
                    //}
                    var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {
                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }
                    string name = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        //var query1 = _db.HR_Master_Employee_DMO.Single(d => d.HRME_Id == amst_Admno);
                        var query1 = (from d in _db.HR_Master_Employee_DMO
                                      where (d.HRME_LeftFlag == false && d.HRME_Id == amst_Admno)
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = d.HRME_Id,
                                          employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)
                                      }).Distinct().ToList();
                        name = query1.FirstOrDefault().employeeName;
                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");
                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.Adm_M_Student.Where(d => d.AMST_Id == amst_Admno).Select(i => new Adm_M_Student { AMST_FirstName = i.AMST_FirstName, AMST_MiddleName = i.AMST_MiddleName, AMST_LastName = i.AMST_LastName }).ToList();
                        name = query1.FirstOrDefault().AMST_FirstName + ' ' + query1.FirstOrDefault().AMST_MiddleName ?? " " + ' ' + query1.FirstOrDefault().AMST_LastName;
                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");
                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);

                        var imgrepl = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                        if (imgrepl.Count > 0)
                        {
                            message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bCOE_IMAGE\b", imgrepl[0].COEEI_Images, RegexOptions.IgnoreCase);
                        }
                    }
                    else
                    {
                        message.HtmlContent = Mailmsg;
                    }
                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = type
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

            return "success";
        }

        public string Send_SMS1(string amst_FName, string amst_mobile, string sms_message, int MID, string Template, int coeid)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                //   var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                //   var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();



                sms = sms_message;

                string result = "";

                List<Match> variables = new List<Match>();
                List<SMS_MAIL_PARAMETER_DMO> ParamaetersName = new List<SMS_MAIL_PARAMETER_DMO>();


                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    SMS_MAIL_PARAMETER_DMO param = new SMS_MAIL_PARAMETER_DMO();
                    variables.Add(match);
                    param.ISMP_NAME = match.ToString();
                    ParamaetersName.Add(param);
                }
                if (ParamaetersName.Count == 0)
                {
                    sms = sms_message + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "COE_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amst_FName",
                            SqlDbType.VarChar)
                        {
                            Value = amst_FName
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = sms_message
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid",
                          SqlDbType.BigInt)
                        {
                            Value = MID
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
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);

                                sms = result;
                            }
                        }
                    }

                    sms = result;
                    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;

                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = amst_mobile.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = "Other";
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    var countrow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                    //    if (countrow.Count > 0)
                    //    {
                    //        var updatesmsflag = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();

                    //        updatesmsflag.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        updatesmsflag.COEESMSMPNSMS_Flg = true;
                    //        _coe.Update(updatesmsflag);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        COE_Events_SMSMailPN_DMO obj1 = new COE_Events_SMSMailPN_DMO();

                    //        obj1.COEE_Id = coeid;
                    //        obj1.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        obj1.COEESMSMPNSMS_Flg = true;
                    //        _coe.Add(obj1);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_Email1(string amst_FName, string Email_message, string amst_Email, int MID, string Template, int coeid, string Email_subject)
        {
            string Mailmsg = "";

            string date1 = "";


            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                Mailmsg = Email_message;
                string result = "";
                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();
                List<Match> variables = new List<Match>();
                List<SMS_MAIL_PARAMETER_DMO> ParamaetersName = new List<SMS_MAIL_PARAMETER_DMO>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    SMS_MAIL_PARAMETER_DMO param = new SMS_MAIL_PARAMETER_DMO();
                    variables.Add(match);
                    param.ISMP_NAME = match.ToString();
                    ParamaetersName.Add(param);
                }

                if (ParamaetersName.Count == 0)
                {
                    Mailmsg = Email_message;
                }
                else
                {

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "COE_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amst_FName",
                            SqlDbType.VarChar)
                        {
                            Value = amst_FName
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Email_message
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid",
                           SqlDbType.BigInt)
                        {
                            Value = MID
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
                }



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

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
                    //message.AddTo("rakeshrd307@gmail.com");
                    message.AddTo(amst_Email);

                    var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (img.Count > 0)
                    {
                        for (int i = 0; i < img.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.jpg");

                        }
                    }

                    var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }



                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);


                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);

                        //  message.EnableSpamCheck();
                    }
                    else
                    {

                        message.HtmlContent = Mailmsg;
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {

                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = "Other"
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

                //var updaterow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                //if (updaterow.Count > 0)
                //{
                //    var updaterow2 = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();
                //    updaterow2.COEESMSMPNMail_Flg = true;
                //    updaterow2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Update(updaterow2);
                //    int row = _coe.SaveChanges();
                //}
                //else
                //{
                //    COE_Events_SMSMailPN_DMO obj2 = new COE_Events_SMSMailPN_DMO();
                //    obj2.COEESMSMPNMail_Flg = true;
                //    obj2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Add(obj2);
                //    int row = _coe.SaveChanges();
                //}


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_SMS_Others_ForEmpty(string amst_FName, string amst_mobile, string eventname, DateTime startdate, int MID, string Template, int coeid)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals("COE") && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();



                //sms = sms_message;
                sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }


                val.Add("[EVENT]", eventname);
                val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@eventname",
                //        SqlDbType.VarChar)
                //    {
                //        Value = eventname
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@startdate",
                //       SqlDbType.VarChar)
                //    {
                //        Value = startdate
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

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
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                    var datatype = dataReader.GetFieldType(iFiled);
                //                    if (datatype.Name == "DateTime")
                //                    {
                //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                //                    }
                //                    else
                //                    {
                //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        return ex.Message;
                //    }

                //}

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
                sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    // string url = "http://trans.kapsystem.com/api/web2sms.php?workingkey=Ada954b9c07a35a318e0207b6c7f97f0d&sender=JSHSCH&to=PHNO&message=MESSAGE";
                    string PHNO = amst_mobile.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = "Other";
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    var countrow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                    //    if (countrow.Count > 0)
                    //    {
                    //        var updatesmsflag = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();

                    //        updatesmsflag.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        updatesmsflag.COEESMSMPNSMS_Flg = true;
                    //        _coe.Update(updatesmsflag);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        COE_Events_SMSMailPN_DMO obj1 = new COE_Events_SMSMailPN_DMO();

                    //        obj1.COEE_Id = coeid;
                    //        obj1.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        obj1.COEESMSMPNSMS_Flg = true;
                    //        _coe.Add(obj1);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_Email1_Others_ForEmpty(string amst_FName, string eventname, DateTime startdate, string amst_Email, int MID, string Template, int coeid, string Email_subject)
        {
            string Mailmsg = "";

            string date1 = "";


            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                //Mailmsg = Email_message;
                // Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                string result = "";
                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                Mailmsg = template.FirstOrDefault().ISES_Mail_Message;

                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (variables.Count == 0)
                {
                    Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                }
                else
                {
                    //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}

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


                }



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

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
                    //message.AddTo("rakeshrd307@gmail.com");
                    message.AddTo(amst_Email);

                    var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (img.Count > 0)
                    {
                        for (int i = 0; i < img.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.jpg");

                        }
                    }

                    var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }



                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Mailmsg; //Regex.Replace(Mailmsg, @"Mailmsg", Mailmsg, RegexOptions.IgnoreCase);


                        //message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);

                        //  message.EnableSpamCheck();
                    }
                    else
                    {

                        message.HtmlContent = Mailmsg;
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {

                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = "Other"
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

            //var updaterow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

            //if (updaterow.Count > 0)
            //{
            //    var updaterow2 = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();
            //    updaterow2.COEESMSMPNMail_Flg = true;
            //    updaterow2.COEESMSMPNMail_Date = DateTime.Now.Date;

            //    _coe.Update(updaterow2);
            //    int row = _coe.SaveChanges();
            //}
            //else
            //{
            //    COE_Events_SMSMailPN_DMO obj2 = new COE_Events_SMSMailPN_DMO();
            //    obj2.COEESMSMPNMail_Flg = true;
            //    obj2.COEESMSMPNMail_Date = DateTime.Now.Date;

            //    _coe.Add(obj2);
            //    int row = _coe.SaveChanges();
            //}



            return "success";
        }

        public string Send_SMS_AluminiStudent_ForEmpty(string amst_FName, string amst_mobile, long amst_Admno, string eventname, DateTime startdate, int MI_ID, string Template, int coeid, string type)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_ID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_ID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_ID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                //else
                //{
                //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "BIRTHDAY_PARAMETER";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(new SqlParameter("@UserID",
                //            SqlDbType.BigInt)
                //        {
                //            Value = amst_Admno
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@template",
                //           SqlDbType.VarChar)
                //        {
                //            Value = Template
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@type",
                //          SqlDbType.VarChar)
                //        {
                //            Value = type
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //                while (dataReader.Read())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                        var datatype = dataReader.GetFieldType(iFiled);
                //                        if (datatype.Name == "DateTime")
                //                        {
                //                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                //                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                //                        }
                //                        else
                //                        {
                //                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            return ex.Message;
                //        }

                //    }

                //    for (int j = 0; j < variables.Count; j++)
                //    {
                //        for (int p = 0; p < val.Count; p++)
                //        {
                //            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                //            {
                //                result = sms.Replace(variables[j].Value, val.Values.ToArray()[p]);
                //                sms = result;
                //            }
                //        }
                //    }

                //    sms = result;
                //    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;


                //}

                else
                {
                    //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}

                    for (int j = 0; j < variables.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(variables[j].Value, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;

                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_ID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    //  string url = "http://trans.kapsystem.com/api/web2sms.php?workingkey=Ada954b9c07a35a318e0207b6c7f97f0d&sender=JSHSCH&to=PHNO&message=MESSAGE";

                    string PHNO = amst_mobile.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_ID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = type;
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    var countrow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                    //    if (countrow.Count > 0)
                    //    {
                    //        var updatesmsflag = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();

                    //        updatesmsflag.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        updatesmsflag.COEESMSMPNSMS_Flg = true;
                    //        _coe.Update(updatesmsflag);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        COE_Events_SMSMailPN_DMO obj1 = new COE_Events_SMSMailPN_DMO();

                    //        obj1.COEE_Id = coeid;
                    //        obj1.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        obj1.COEESMSMPNSMS_Flg = true;
                    //        _coe.Add(obj1);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //}
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_Email1_AluminiStudent_ForEmpty(string amst_FName, long amst_Admno, string Email_header, string Email_subject, string eventname, string amst_Email, string Email_footer, DateTime startdate, int MI_ID, string Template, int coeid, string type)
        {
            try
            {
                string accountname = "";
                string accesskey = "";
                string Mailmsg = "";

                string date1 = "";

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_ID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_ID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_ID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                Mailmsg = template.FirstOrDefault().ISES_Mail_Message;


                string result = "";
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                }
                else
                {
                    //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}


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
                }



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_ID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

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
                    // message.AddTo("rakeshrd307@gmail.com");
                    message.AddTo(amst_Email);

                    var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (img.Count > 0)
                    {
                        for (int i = 0; i < img.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
                        }
                    }

                    var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }

                    string name = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        //var query1 = _db.HR_Master_Employee_DMO.Single(d => d.HRME_Id == amst_Admno);
                        var query1 = (from d in _db.HR_Master_Employee_DMO
                                      where (d.HRME_LeftFlag == false && d.HRME_Id == amst_Admno)
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = d.HRME_Id,
                                          employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)

                                      }).Distinct().ToList();

                        name = query1.FirstOrDefault().employeeName;

                        //date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.Adm_M_Student.Where(d => d.AMST_Id == amst_Admno).Select(i => new Adm_M_Student { AMST_FirstName = i.AMST_FirstName, AMST_MiddleName = i.AMST_MiddleName, AMST_LastName = i.AMST_LastName }).ToList();
                        name = query1.FirstOrDefault().AMST_FirstName + ' ' + query1.FirstOrDefault().AMST_MiddleName ?? " " + ' ' + query1.FirstOrDefault().AMST_LastName;

                        // date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");


                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Mailmsg;// Regex.Replace(Mailmsg, @"Mailmsg", Mailmsg, RegexOptions.IgnoreCase);


                        //message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);



                    }
                    else
                    {

                        message.HtmlContent = Mailmsg;
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_ID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = type
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

                //var updaterow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                //if (updaterow.Count > 0)
                //{
                //    var updaterow2 = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();
                //    updaterow2.COEESMSMPNMail_Flg = true;
                //    updaterow2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Update(updaterow2);
                //    int row = _coe.SaveChanges();
                //}
                //else
                //{
                //    COE_Events_SMSMailPN_DMO obj2 = new COE_Events_SMSMailPN_DMO();
                //    obj2.COEESMSMPNMail_Flg = true;
                //    obj2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Add(obj2);
                //    int row = _coe.SaveChanges();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_SMS_Employee_ForEmpty(string amst_FName, string amst_mobile, long amst_Admno, string eventname, DateTime startdate, int MID, string type, string Template, int coeid)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}

                    for (int j = 0; j < variables.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(variables[j].Value, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;


                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    //  string url = "http://trans.kapsystem.com/api/web2sms.php?workingkey=Ada954b9c07a35a318e0207b6c7f97f0d&sender=JSHSCH&to=PHNO&message=MESSAGE";

                    string PHNO = amst_mobile.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = type;
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    var countrow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                    //    if (countrow.Count > 0)
                    //    {
                    //        var updatesmsflag = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();

                    //        updatesmsflag.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        updatesmsflag.COEESMSMPNSMS_Flg = true;
                    //        _coe.Update(updatesmsflag);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        COE_Events_SMSMailPN_DMO obj1 = new COE_Events_SMSMailPN_DMO();

                    //        obj1.COEE_Id = coeid;
                    //        obj1.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        obj1.COEESMSMPNSMS_Flg = true;
                    //        _coe.Add(obj1);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //}
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_Email1_Employee_ForEmpty(string amst_FName, long amst_Admno, string Email_header, string Email_subject, string eventname, DateTime startdate, string amst_Email, string Email_footer, int MID, string type, string Template, int coeid)
        {
            try
            {
                string accountname = "";
                string accesskey = "";
                string Mailmsg = "";

                string date1 = "";

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                Mailmsg = template.FirstOrDefault().ISES_Mail_Message;


                string result = "";
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                }
                else
                {
                    //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //val.Add("[INSTUITENAME]", institutionName.ToString());
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}


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
                }



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    // string SendingEmail = "roopa@vapstech.com";
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

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
                    // message.AddTo("rakeshrd307@gmail.com");
                     message.AddTo(amst_Email);
                    //message.AddTo("punithnmsd2@gmail.com");

                    //var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    //if (img.Count > 0)
                    //{
                    //    for (int i = 0; i < img.Count; i++)
                    //    {
                    //        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                    //        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                    //        Stream stream = response.GetResponseStream();
                    //        message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
                    //    }
                    //}

                    var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }

                    string name = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        //var query1 = _db.HR_Master_Employee_DMO.Single(d => d.HRME_Id == amst_Admno);
                        var query1 = (from d in _db.HR_Master_Employee_DMO
                                      where (d.HRME_LeftFlag == false && d.HRME_Id == amst_Admno)
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = d.HRME_Id,
                                          employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)

                                      }).Distinct().ToList();

                        name = query1.FirstOrDefault().employeeName;

                        // date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.Adm_M_Student.Where(d => d.AMST_Id == amst_Admno).Select(i => new Adm_M_Student { AMST_FirstName = i.AMST_FirstName, AMST_MiddleName = i.AMST_MiddleName, AMST_LastName = i.AMST_LastName }).ToList();
                        name = query1.FirstOrDefault().AMST_FirstName + ' ' + query1.FirstOrDefault().AMST_MiddleName ?? " " + ' ' + query1.FirstOrDefault().AMST_LastName;

                        //   date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");


                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Mailmsg;// Regex.Replace(Mailmsg, @"Mailmsg", Mailmsg, RegexOptions.IgnoreCase);

                        // message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                        //var imgrepl = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                        //if (imgrepl.Count > 0)
                        //{
                        //    message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bCOE_IMAGE\b", imgrepl[0].COEEI_Images, RegexOptions.IgnoreCase);
                        //}

                    }
                    else
                    {

                        message.HtmlContent = Mailmsg;
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = type
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

                //var updaterow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                //if (updaterow.Count > 0)
                //{
                //    var updaterow2 = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();
                //    updaterow2.COEESMSMPNMail_Flg = true;
                //    updaterow2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Update(updaterow2);
                //    int row = _coe.SaveChanges();
                //}
                //else
                //{
                //    COE_Events_SMSMailPN_DMO obj2 = new COE_Events_SMSMailPN_DMO();
                //    obj2.COEESMSMPNMail_Flg = true;
                //    obj2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Add(obj2);
                //    int row = _coe.SaveChanges();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


        public string Send_SMS_Student_ForEmpty(string amst_FName, string amst_mobile, long amst_Admno, string eventname, DateTime startdate, int MID, string type, string Template, int coeid)
        {
            string sms = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    //startdate = Convert.ToDateTime(startdate).Date.ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}

                    for (int j = 0; j < variables.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(variables[j].Value, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;


                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    // string url = "http://trans.kapsystem.com/api/web2sms.php?workingkey=Ada954b9c07a35a318e0207b6c7f97f0d&sender=JSHSCH&to=PHNO&message=MESSAGE";

                    string PHNO = amst_mobile.ToString();
                    //string PHNO = "7411981530";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = type;
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    var countrow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                    //    if (countrow.Count > 0)
                    //    {
                    //        var updatesmsflag = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();

                    //        updatesmsflag.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        updatesmsflag.COEESMSMPNSMS_Flg = true;
                    //        _coe.Update(updatesmsflag);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        COE_Events_SMSMailPN_DMO obj1 = new COE_Events_SMSMailPN_DMO();

                    //        obj1.COEE_Id = coeid;
                    //        obj1.COEESMSMPNSMS_Date = DateTime.Now.Date;
                    //        obj1.COEESMSMPNSMS_Flg = true;
                    //        _coe.Add(obj1);
                    //        int row = _coe.SaveChanges();
                    //    }
                    //}
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string Send_Email1_Student_ForEmpty(string amst_FName, long amst_Admno, string Email_header, string Email_subject, string eventname, DateTime startdate, string amst_Email, string Email_footer, int MID, string type, string Template, int coeid)
        {
            try
            {
                string accountname = "";
                string accesskey = "";
                string Mailmsg = "";

                string date1 = "";

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                Mailmsg = template.FirstOrDefault().ISES_Mail_Message;


                string result = "";
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                }
                else
                {
                    //startdate = (startdate)..ToString("dd-MM-yyyy");
                    val.Add("[EVENT]", eventname);
                    val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //val.Add("[DATE]", startdate.ToString("dd'/'MM'/'yyyy"));
                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "COE_SMS_Email_PARAMETER";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@eventname",
                    //        SqlDbType.VarChar)
                    //    {
                    //        Value = eventname
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@startdate",
                    //       SqlDbType.VarChar)
                    //    {
                    //        Value = startdate
                    //    });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

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
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}


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
                }



                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

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
                    // message.AddTo("rakeshrd307@gmail.com");
                    message.AddTo(amst_Email);
                   

                    var img = _coe.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (img.Count > 0)
                    {
                        for (int i = 0; i < img.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
                        }
                    }

                    var vido = _coe.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }

                    string name = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        //var query1 = _db.HR_Master_Employee_DMO.Single(d => d.HRME_Id == amst_Admno);
                        var query1 = (from d in _db.HR_Master_Employee_DMO
                                      where (d.HRME_LeftFlag == false && d.HRME_Id == amst_Admno)
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = d.HRME_Id,
                                          employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)

                                      }).Distinct().ToList();

                        name = query1.FirstOrDefault().employeeName;

                        // date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.Adm_M_Student.Where(d => d.AMST_Id == amst_Admno).Select(i => new Adm_M_Student { AMST_FirstName = i.AMST_FirstName, AMST_MiddleName = i.AMST_MiddleName, AMST_LastName = i.AMST_LastName }).ToList();
                        name = query1.FirstOrDefault().AMST_FirstName + ' ' + query1.FirstOrDefault().AMST_MiddleName ?? " " + ' ' + query1.FirstOrDefault().AMST_LastName;

                        //  date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");


                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Mailmsg;// Regex.Replace(Mailmsg, @"Mailmsg", Mailmsg, RegexOptions.IgnoreCase);


                        // message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }
                    else
                    {

                        message.HtmlContent = Mailmsg;
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                         SqlDbType.VarChar)
                        {
                            Value = type
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

                //var updaterow = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().ToList();

                //if (updaterow.Count > 0)
                //{
                //    var updaterow2 = _coe.COE_Events_SMSMailPN_DMO.Where(t => t.COEE_Id == coeid).Distinct().SingleOrDefault();
                //    updaterow2.COEESMSMPNMail_Flg = true;
                //    updaterow2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Update(updaterow2);
                //    int row = _coe.SaveChanges();
                //}
                //else
                //{
                //    COE_Events_SMSMailPN_DMO obj2 = new COE_Events_SMSMailPN_DMO();
                //    obj2.COEESMSMPNMail_Flg = true;
                //    obj2.COEESMSMPNMail_Date = DateTime.Now.Date;

                //    _coe.Add(obj2);
                //    int row = _coe.SaveChanges();
                //}

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
