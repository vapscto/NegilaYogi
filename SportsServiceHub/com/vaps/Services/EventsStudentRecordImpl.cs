using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Sports;
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
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SportsServiceHub.com.vaps.Services
{
    public class EventsStudentRecordImpl : Interfaces.EventsStudentRecordInterface
    {
        public DomainModelMsSqlServerContext _db;
        public SportsContext _context;

        public EventsStudentRecordImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }

        public async Task<EventsStudentRecordDTO> getDetails(EventsStudentRecordDTO data)
        {
            try
            {
                data.academicYear = _context.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.classList = _context.admissionClass.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToArray();

                data.houselist = _context.SportMasterHouseDMO.Where(t => t.MI_Id == data.MI_Id && t.SPCCMH_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                data.groupsportdata = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_SCCFlag == "Sports" && d.SPCCMSCCG_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                var compitionlevel = _context.SportMasterCompitionLevelDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMCL_ActiveFlag == true).ToList();
                if (compitionlevel.Count > 0)
                {
                    data.compitionLevelList = compitionlevel.OrderBy(t => t.SPCCMCL_CompitionLevel).ToArray();
                }
                data.categoryList = (from a in _context.MasterCompitionCategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                     select new EventsStudentRecordDTO
                                     {
                                         SPCCMCC_Id = a.SPCCMCC_Id,
                                         SPCCMCC_CompitionCategory = a.SPCCMCC_CompitionCategory,
                                     }).Distinct().ToArray();

                data.categoryListtt2 = (from a in _context.MasterCompitionCategoryDMO
                                        where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                        select new EventsStudentRecordDTO
                                        {
                                            comcat_ids = a.SPCCMCC_Id,
                                            comptcatename = a.SPCCMCC_CompitionCategory,
                                        }).Distinct().ToArray();

                data.categoryListttCls = (from a in _context.MasterCompitionCategoryDMO
                                          where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                          select new EventsStudentRecordDTO
                                          {
                                              comcat_idcls = a.SPCCMCC_Id,
                                              comptcatenamecls = a.SPCCMCC_CompitionCategory,
                                          }).Distinct().ToArray();


                var sportsCC = _context.MasterSportsCCNameDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCC_ActiveFlag == true).ToList();
                if (sportsCC.Count > 0)
                {
                    data.sportsCCList = sportsCC.OrderBy(t => t.SPCCMSCC_SportsCCName).ToArray();
                }

                var uom = _context.SportMasterUOMDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMUOM_ActiveFlag == true).ToList();
                if (uom.Count > 0)
                {
                    data.uomList = uom.OrderBy(t => t.SPCCMUOM_UOMName).ToArray();
                }
                //

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_StudentTarnsAlldata";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        }
                        data.eventsStudentRecordList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.getMasterEvent = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == 0).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_StudentTarnsAlldata_SRKVS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        }
                        data.eventsStudentRecordListSRKVS = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.eventmappingList = _context.MasterEventVenueDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCMEV_ActiveFlag == true).Distinct().ToArray();
                data.eventname = _context.MasterEventsDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCME_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<EventsStudentRecordDTO> saveRecord(EventsStudentRecordDTO obj)
        {
            try
            {
                if (obj.SPCCEST_Id == 0)
                {
                    List<long> amstids = new List<long>();
                    if (obj.student1.Count() > 0)
                    {
                        foreach (var it in obj.student1)
                        {
                            amstids.Add(it.amsT_Id);
                        }
                    }

                    var checkduplicate = (from a in _context.SPCC_Events_Students_DMO
                                          from b in _context.EventsStudentRecordDMO
                                          where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && a.MI_Id == obj.MI_Id && a.SPCCME_Id == obj.SPCCME_Id && a.SPCCMCL_Id == obj.SPCCMCL_Id && a.SPCCMCC_Id == obj.SPCCMCC_Id && a.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && a.SPCCMSCC_Id == obj.SPCCMSCC_Id && a.SPCCMUOM_Id == obj.SPCCMUOM_Id && a.SPCCEST_House_Class_Flag == obj.SPCCEST_House_Class_Flag && a.SPCCEST_OldRecord == obj.SPCCEST_OldRecord && a.SPCCEST_Remarks == obj.SPCCEST_Remarks && amstids.Contains(b.AMST_Id) && a.ASMAY_Id == obj.ASMAY_Id && a.ASMCL_Id == obj.ASMCL_Id)
                                          select a).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        SPCC_Events_Students_DMO mapp = new SPCC_Events_Students_DMO();

                        //mapp.SPCCEST_Id = obj.SPCCEST_Id;
                        mapp.MI_Id = obj.MI_Id;
                        mapp.SPCCME_Id = obj.SPCCME_Id;
                        mapp.SPCCMCL_Id = obj.SPCCMCL_Id;
                        mapp.SPCCMCC_Id = obj.SPCCMCC_Id;
                        mapp.SPCCMSCC_Id = obj.SPCCMSCC_Id;
                        mapp.SPCCMUOM_Id = obj.SPCCMUOM_Id;
                        mapp.SPCCEST_House_Class_Flag = obj.SPCCEST_House_Class_Flag;
                        mapp.SPCCEST_OldRecord = obj.SPCCEST_OldRecord;
                        mapp.SPCCEST_Remarks = obj.SPCCEST_Remarks;
                        mapp.SPCCMSCCG_Id = obj.SPCCMSCCG_Id;
                        mapp.SPCCEST_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.ASMAY_Id = obj.ASMAY_Id;
                        //mapp.ASMCL_Id = obj.ASMCL_Id;
                        //mapp.ASMS_Id = obj.ASMS_Id;
                        _context.Add(mapp);

                        for (int i = 0; i < obj.student1.Length; i++)
                        {

                            EventsStudentRecordDMO mapp2 = new EventsStudentRecordDMO();

                            mapp2.SPCCEST_Id = mapp.SPCCEST_Id;
                            //mapp2.SPCCESTR_Id = obj.SPCCESTR_Id;
                            mapp2.MI_Id = obj.MI_Id;
                            mapp2.AMST_Id = obj.student1[i].amsT_Id;
                            if (obj.student1[i].spccestR_Rank != "")
                            {
                                mapp2.SPCCESTR_Rank = obj.student1[i].spccestR_Rank;
                            }
                            else
                            {
                                mapp2.SPCCESTR_Rank = "0";
                            }
                            mapp2.SPCCESTR_Points = Convert.ToDouble(obj.student1[i].spccestR_Points);

                            if (obj.student1[i].spccestR_RecordBrokenFlag == null)
                            {
                                mapp2.SPCCESTR_RecordBrokenFlag = "false";
                            }
                            else
                            {
                                mapp2.SPCCESTR_RecordBrokenFlag = obj.student1[i].spccestR_RecordBrokenFlag;
                            }
                            mapp2.SPCCESTR_Remarks = obj.student1[i].spccestR_Remarks;
                            mapp2.SPCCESTR_ActiveFlag = true;
                            mapp2.CreatedDate = DateTime.Now;
                            mapp2.UpdatedDate = DateTime.Now;

                            _context.Add(mapp2);
                        }

                        int rowAffected = _context.SaveChanges();

                        if (rowAffected > 0)
                        {
                            obj.returnVal = "Saved!";

                            for (int i = 0; i < obj.student1.Count(); i++)
                            {
                                var temp_usr = obj.student1[i].amsT_Id;
                                string place = "";
                                string rank = obj.student1[i].spccestR_Rank;
                                if (rank == "1")
                                {
                                    place = "First";
                                }
                                else if (rank == "2")
                                {
                                    place = "Second";
                                }
                                else if (rank == "3")
                                {
                                    place = "Third";
                                }
                                long? parent_no = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatherMobleNo;

                                var fName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FirstName;
                                var mName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_MiddleName;
                                var lName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_LastName;

                                string studentName = fName + " " + mName + " " + lName;

                                var mail_id = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatheremailId;

                                string evename = _context.MasterEventsDMO.Single(t => t.MI_Id == obj.MI_Id && t.SPCCME_Id == obj.SPCCME_Id).SPCCME_EventName;
                                string Template = "";

                                var eventdate = (from c in _context.EventsMappingDMO
                                                 from d in _context.MasterEventsDMO
                                                 where (c.SPCCME_Id == obj.SPCCME_Id && c.ASMAY_Id == obj.ASMAY_Id
                                                 && c.MI_Id == obj.MI_Id && d.SPCCME_Id == c.SPCCME_Id
                                                 && d.MI_Id == c.MI_Id)
                                                 select new EventsStudentRecordDTO
                                                 {
                                                     SPCCE_StartDate = c.SPCCE_StartDate
                                                 }).Distinct().ToList();

                                obj.SPCCE_StartDate = eventdate.SingleOrDefault().SPCCE_StartDate;

                                //if (obj.SPCCESTR_Rank == "1" || obj.SPCCESTR_Rank == "2" || obj.SPCCESTR_Rank == "3")
                                if (obj.student1[i].spccestR_Rank == "1" || obj.student1[i].spccestR_Rank == "2" || obj.student1[i].spccestR_Rank == "3")
                                {
                                    Template = "Sportswinners";
                                }
                                else
                                {
                                    Template = "Sports";
                                }
                                if (obj.sendmail == true)
                                {
                                    sendmail(obj.MI_Id, studentName, evename, Template, mail_id, place, Convert.ToDateTime(obj.SPCCE_StartDate));

                                }
                                if (obj.sendsms == true)
                                {
                                    //SMS sss = new SMS(_db);
                                    //sss.sendSms1(obj.MI_Id, studentName, evename, Template, parent_no);

                                    string val2 = await sendSmsAsync(obj.MI_Id, studentName, evename, Template, parent_no, place, Convert.ToDateTime(obj.SPCCE_StartDate));
                                }
                            }
                        }
                        else
                        {
                            obj.returnVal = "Save Failed!";
                        }
                    }
                }
                else if (obj.SPCCEST_Id > 0)
                {

                    var checkduplicate = _context.SPCC_Events_Students_DMO.Where(d => d.SPCCEST_Id != d.SPCCEST_Id && d.MI_Id == obj.MI_Id && d.SPCCME_Id == obj.SPCCME_Id && d.SPCCMCL_Id == obj.SPCCMCL_Id && d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && d.SPCCMCC_Id == obj.SPCCMCC_Id && d.SPCCMSCC_Id == obj.SPCCMSCC_Id && d.SPCCMUOM_Id == obj.SPCCMUOM_Id && d.SPCCEST_House_Class_Flag == obj.SPCCEST_House_Class_Flag && d.SPCCEST_OldRecord == obj.SPCCEST_OldRecord && d.SPCCEST_Remarks == obj.SPCCEST_Remarks && d.ASMAY_Id == obj.ASMAY_Id && d.ASMCL_Id == obj.ASMCL_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.SPCC_Events_Students_DMO.Where(d => d.SPCCEST_Id == obj.SPCCEST_Id && d.MI_Id == obj.MI_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.SPCC_Events_Students_DMO.Where(d => d.SPCCEST_Id == obj.SPCCEST_Id && d.MI_Id == obj.MI_Id).SingleOrDefault();

                            update.SPCCEST_Id = update.SPCCEST_Id;
                            update.SPCCME_Id = obj.SPCCME_Id;
                            update.SPCCMCL_Id = obj.SPCCMCL_Id;
                            update.SPCCMCC_Id = obj.SPCCMCC_Id;
                            update.SPCCMSCC_Id = obj.SPCCMSCC_Id;

                            update.SPCCMUOM_Id = obj.SPCCMUOM_Id;
                            update.SPCCEST_House_Class_Flag = obj.SPCCEST_House_Class_Flag;
                            update.SPCCEST_OldRecord = obj.SPCCEST_OldRecord;
                            update.SPCCEST_Remarks = obj.SPCCEST_Remarks;
                            update.SPCCMSCCG_Id = obj.SPCCMSCCG_Id;
                            update.UpdatedDate = DateTime.Now;
                            update.ASMAY_Id = obj.ASMAY_Id;
                            //update.ASMCL_Id = obj.ASMCL_Id;
                            //update.ASMS_Id = obj.ASMS_Id;

                            _context.Add(update);

                            List<long> amstids = new List<long>();
                            if (obj.student1.Count() > 0)
                            {
                                foreach (var it in obj.student1)
                                {
                                    amstids.Add(it.amsT_Id);
                                }
                            }
                            var resultclass = _context.EventsStudentRecordDMO.Where(d => d.SPCCEST_Id == obj.SPCCEST_Id & d.MI_Id == obj.MI_Id && amstids.Contains(d.AMST_Id));
                            if (resultclass.Count() > 0)
                            {
                                foreach (var item in resultclass)
                                {
                                    _context.Remove(item);
                                }
                            }
                            for (int i = 0; i < obj.student1.Length; i++)
                            {
                                EventsStudentRecordDMO update2 = new EventsStudentRecordDMO();

                                update2.SPCCEST_Id = update.SPCCEST_Id;
                                //update2.SPCCESTR_Id = obj.SPCCESTR_Id;
                                update2.MI_Id = obj.MI_Id;
                                update2.AMST_Id = obj.student1[i].amsT_Id;
                                update2.SPCCESTR_Rank = obj.student1[i].spccestR_Rank;
                                update2.SPCCESTR_Points = Convert.ToDouble(obj.student1[i].spccestR_Points);

                                if (obj.student1[i].spccestR_RecordBrokenFlag == null)
                                {
                                    update2.SPCCESTR_RecordBrokenFlag = "false";
                                }
                                else
                                {
                                    update2.SPCCESTR_RecordBrokenFlag = obj.student1[i].spccestR_RecordBrokenFlag;
                                }

                                update2.SPCCESTR_Remarks = obj.student1[i].spccestR_Remarks;
                                update2.UpdatedDate = DateTime.Now;
                                update2.CreatedDate = DateTime.Now;
                                update2.SPCCESTR_ActiveFlag = true;

                                _context.Add(update2);
                            }
                            _context.Update(update);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "updated";

                                for (int i = 0; i < obj.student1.Count(); i++)
                                {
                                    var temp_usr = obj.student1[i].amsT_Id;

                                    string place = "";
                                    string rank = obj.student1[i].spccestR_Rank;
                                    if (rank == "1")
                                    {
                                        place = "First";
                                    }
                                    else if (rank == "2")
                                    {
                                        place = "Second";
                                    }
                                    else if (rank == "3")
                                    {
                                        place = "Third";
                                    }

                                    long? parent_no = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatherMobleNo;

                                    var fName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FirstName;
                                    var mName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_MiddleName;
                                    var lName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_LastName;

                                    string studentName = fName + " " + mName + " " + lName;

                                    var mail_id = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatheremailId;

                                    string evename = _context.MasterEventsDMO.Single(t => t.MI_Id == obj.MI_Id && t.SPCCME_Id == obj.SPCCME_Id).SPCCME_EventName;
                                    string Template = "";
                                    var eventdate = (from c in _context.EventsMappingDMO
                                                     from d in _context.MasterEventsDMO
                                                     where (c.SPCCME_Id == obj.SPCCME_Id && c.ASMAY_Id == obj.ASMAY_Id
                                                     && c.MI_Id == obj.MI_Id && d.SPCCME_Id == c.SPCCME_Id
                                                     && d.MI_Id == c.MI_Id)
                                                     select new EventsStudentRecordDTO
                                                     {
                                                         SPCCE_StartDate = c.SPCCE_StartDate
                                                     }).Distinct().ToList();

                                    obj.SPCCE_StartDate = eventdate.SingleOrDefault().SPCCE_StartDate;

                                    //if (obj.SPCCESTR_Rank == "1" || obj.SPCCESTR_Rank == "2" || obj.SPCCESTR_Rank == "3")
                                    if (obj.student1[i].spccestR_Rank == "1" || obj.student1[i].spccestR_Rank == "2" || obj.student1[i].spccestR_Rank == "3")
                                    {
                                        Template = "Sportswinners";
                                    }
                                    else
                                    {
                                        Template = "Sports";
                                    }
                                    if (obj.sendmail == true)
                                    {
                                        sendmail(obj.MI_Id, studentName, evename, Template, mail_id, place, Convert.ToDateTime(obj.SPCCE_StartDate));

                                    }
                                    if (obj.sendsms == true)
                                    {
                                        //SMS sss = new SMS(_db);
                                        //sss.sendSms1(obj.MI_Id, studentName, evename, Template, parent_no);

                                        string val2 = await sendSmsAsync(obj.MI_Id, studentName, evename, Template, parent_no, place, Convert.ToDateTime(obj.SPCCE_StartDate));
                                    }
                                }
                            }
                            else
                            {
                                obj.returnVal = "updateFailed";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public async void sendmail(long MI_Id, string studentName, string evename, string Template, string mail_id, string place, DateTime SPCCE_StartDate)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime stardate = Convert.ToDateTime(SPCCE_StartDate);
                string startdate = stardate.ToString("dd'/'mm'/'yyyy");

                val.Add("[NAME]", studentName);
                val.Add("[PLACE]", place);
                val.Add("[EVENT]", evename);
                val.Add("[DATE]", startdate);

                #region
                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Sports_Email";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@studentName",
                //          SqlDbType.VarChar)
                //    {
                //        Value = studentName
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@place",
                //          SqlDbType.VarChar)
                //    {
                //        Value = place
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@event",
                //          SqlDbType.VarChar)
                //    {
                //        Value = evename
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@eventdate", 
                //        SqlDbType.VarChar)
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
                //        Console.WriteLine(ex.Message);
                //    }

                //}
                #endregion

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                            Mailmsg = result_value;
                        }
                    }
                }
                Mailmsg = result_value;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }

                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.
                    try
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        //mail_id = "amanrce@gmail.com";
                        message.AddTo(mail_id);

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Sports" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_For_Sports";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
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
                            Console.WriteLine(ex.Message);
                            // return ex.Message;
                        }

                    }
                }
                //Mails Sending end
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            // return "success";
        }

        public async Task<EventsStudentRecordDTO> EditDetails(EventsStudentRecordDTO data)
        {
            try
            {
                var editdata = (from a in _context.SPCC_Events_Students_DMO
                                from b in _context.EventsStudentRecordDMO
                                from c in _context.SportStudentHouseDivisionDMO
                                from d in _context.SportMasterHouseDMO
                                where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && b.AMST_Id == c.AMST_Id && c.SPCCMH_Id == d.SPCCMH_Id && a.SPCCEST_Id == data.SPCCEST_Id && b.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && b.SPCCESTR_Id == data.SPCCESTR_Id)
                                select new EventsStudentRecordDTO
                                {
                                    SPCCEST_Id = a.SPCCEST_Id,
                                    SPCCME_Id = a.SPCCME_Id,
                                    SPCCMCL_Id = a.SPCCMCL_Id,
                                    SPCCMCC_Id = a.SPCCMCC_Id,
                                    SPCCMSCC_Id = a.SPCCMSCC_Id,
                                    SPCCMUOM_Id = a.SPCCMUOM_Id,
                                    SPCCEST_House_Class_Flag = a.SPCCEST_House_Class_Flag,
                                    SPCCEST_OldRecord = a.SPCCEST_OldRecord,
                                    SPCCEST_Remarks = a.SPCCEST_Remarks,
                                    SPCCMSCCG_Id = a.SPCCMSCCG_Id,
                                    SPCCMH_Id = c.SPCCMH_Id,
                                    SPCCMH_HouseName = d.SPCCMH_HouseName,
                                    SPCCESTR_Id = b.SPCCESTR_Id,
                                    housename = d.SPCCMH_HouseName,
                                }).Distinct().ToList();

                data.editDetails = editdata.ToArray();

                List<long> groupid = new List<long>();
                if (data.editDetails.Length > 0)
                {
                    foreach (var item in editdata)
                    {
                        groupid.Add(item.SPCCMSCCG_Id);
                    }
                }

                data.groupsportdata = (from d in _context.MasterSportsCCGroupDMO
                                       where (d.MI_Id == data.MI_Id && groupid.Contains(d.SPCCMSCCG_Id))
                                       select new EventsStudentRecordDTO
                                       {
                                           SPCCMSCCG_SCCFlag = d.SPCCMSCCG_SCCFlag,
                                           SPCCMSCCG_Id = d.SPCCMSCCG_Id,
                                       }).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                var student = (
                               //from h1 in _context.SportMasterHouseDMO
                               from h in _context.SPCC_Events_Students_DMO
                               from y in _context.admissionyearstudent
                               from st in _context.Adm_M_Student
                               from c in _context.admissionClass
                               from s in _context.masterSection
                               from esr in _context.EventsStudentRecordDMO
                               from gconfn in _context.GenConfig
                               where (esr.SPCCEST_Id == h.SPCCEST_Id && y.ASMAY_Id == h.ASMAY_Id && y.ASMCL_Id == c.ASMCL_Id && y.ASMS_Id == s.ASMS_Id
                               && y.AMST_Id == esr.AMST_Id && esr.AMST_Id == st.AMST_Id && st.AMST_SOL.Equals("S") && y.AMAY_ActiveFlag == 1 && y.AMST_Id == esr.AMST_Id && h.MI_Id == data.MI_Id
                               && y.ASMAY_Id == data.ASMAY_Id && esr.SPCCESTR_Id == data.SPCCESTR_Id && c.MI_Id == gconfn.MI_Id)
                               select new EventsStudentRecordDTO
                               {
                                   AMST_Id = st.AMST_Id,
                                   studentName = st.AMST_FirstName + (string.IsNullOrEmpty(st.AMST_MiddleName) ? "" : ' ' + st.AMST_MiddleName) + (string.IsNullOrEmpty(st.AMST_LastName) ? "" : ' ' + st.AMST_LastName),
                                   AMST_AdmNo = st.AMST_AdmNo,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMC_SectionName = s.ASMC_SectionName,
                                   ASMS_Id = s.ASMS_Id,
                                   //SPCCMH_HouseName = h1.SPCCMH_HouseName,
                                   SPCCESTR_Rank = esr.SPCCESTR_Rank,
                                   SPCCESTR_Points = esr.SPCCESTR_Points,
                                   SPCCESTR_RecordBrokenFlag = esr.SPCCESTR_RecordBrokenFlag,
                                   SPCCESTR_Remarks = esr.SPCCESTR_Remarks,
                                   SPCCESTR_ActiveFlag = esr.SPCCESTR_ActiveFlag,
                                   SPCCESTR_Id = esr.SPCCESTR_Id,
                                   IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg,


                               }).Distinct().OrderBy(n => n.AMST_Id).ToList();

                data.editstulist = student.ToArray();

                var yearlsdt = (from a in _db.SchoolYearWiseStudent
                                from b in _db.Adm_M_Student
                                from c in _db.School_M_Class
                                from s in _db.School_M_Section
                                where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == s.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && b.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                                select new EventsStudentRecordDTO
                                {
                                    ASMCL_Id = a.ASMCL_Id,
                                    ASMS_Id = a.ASMS_Id,
                                    ASMAY_Id = a.ASMAY_Id,
                                    AMST_Id = a.AMST_Id,
                                }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                data.editClsSecYear = yearlsdt.ToArray();

                data.eventname = (from a in _context.MasterEventsDMO
                                  from b in _context.EventsMappingDMO
                                  where (a.MI_Id == b.MI_Id && a.SPCCME_Id == b.SPCCME_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                          && a.SPCCME_ActiveFlag == true && b.SPCCE_ActiveFlag == true)
                                  select a).Distinct().OrderBy(t => t.SPCCME_Id).ToArray();

                List<long> clssids = new List<long>();
                if (yearlsdt.Length > 0)
                {
                    foreach (var item in yearlsdt)
                    {
                        clssids.Add(item.ASMCL_Id);
                    }
                }
                data.sectionList = (from a in _context.masterSection
                                    from c in _context.admissionClass
                                    from b in _context.admissionyearstudent
                                    where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == a.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && clssids.Contains(b.ASMCL_Id) && b.AMAY_ActiveFlag == 1 && a.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                                    select a).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

                data.classList = (from c in _context.admissionClass
                                  from b in _context.admissionyearstudent
                                  where (b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true && c.ASMCL_ActiveFlag == true)
                                  select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();


                try
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentMappingAgeReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.SPCCMCC_Id
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
                            }
                            var studentList = retObject.ToList();


                            List<long> house_ids = new List<long>();
                            List<long> amst_ids = new List<long>();
                            foreach (var item in studentList)
                            {
                                house_ids.Add(item.SPCCMH_Id);
                            }
                            data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                            foreach (var item2 in studentList)
                            {
                                amst_ids.Add(item2.AMST_Id);
                            }
                            List<long> class_ids = new List<long>();
                            foreach (var item3 in studentList)
                            {
                                class_ids.Add(item3.ASMCL_Id);
                            }
                            List<long> section_ids = new List<long>();
                            foreach (var item4 in studentList)
                            {
                                section_ids.Add(item4.ASMS_Id);
                            }

                            data.studentList = (from a in _context.Adm_M_Student
                                                from b in _context.SportStudentHouseDivisionDMO
                                                from c in _context.SportMasterHouseDMO
                                                from y in _context.admissionyearstudent
                                                from d in _context.admissionClass
                                                from e in _context.masterSection
                                                from gconfn in _context.GenConfig

                                                where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id
                                                && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id
                                                && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id
                                                && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id)
                                                && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1
                                                && d.MI_Id == gconfn.MI_Id)
                                                select new EventsStudentRecordDTO
                                                {
                                                    AMST_Id = a.AMST_Id,
                                                    AMST_AdmNo = a.AMST_AdmNo,
                                                    SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                    ASMCL_ClassName = d.ASMCL_ClassName,
                                                    ASMC_SectionName = e.ASMC_SectionName,
                                                    studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                    IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                                }).Distinct().OrderBy(t => t.studentName).ToArray();

                            data.categoryListtt2 = (from a in _context.MasterCompitionCategoryDMO
                                                    where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                                    select new EventsStudentRecordDTO
                                                    {
                                                        comcat_ids = a.SPCCMCC_Id,
                                                        comptcatename = a.SPCCMCC_CompitionCategory,
                                                    }).Distinct().ToArray();

                            data.houselistedit = (from a in _context.SportMasterHouseDMO
                                                  from b in _context.SportStudentHouseDivisionDMO
                                                  where (a.MI_Id == b.MI_Id && a.SPCCMH_Id == b.SPCCMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.SPCCMH_ActiveFlag == true && a.SPCCMH_ActiveFlag == true)
                                                  select new EventsStudentRecordDTO
                                                  {
                                                      house_id = a.SPCCMH_Id,
                                                      housename = a.SPCCMH_HouseName
                                                  }).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                            data.categoryListttCls = (from a in _context.MasterCompitionCategoryDMO
                                                      where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                                      select new EventsStudentRecordDTO
                                                      {
                                                          comcat_idcls = a.SPCCMCC_Id,
                                                          comptcatenamecls = a.SPCCMCC_CompitionCategory,
                                                      }).Distinct().ToArray();

                            data.houselistclass = (from a in _context.SportMasterHouseDMO
                                                   from b in _context.SportStudentHouseDivisionDMO
                                                   where (a.MI_Id == b.MI_Id && a.SPCCMH_Id == b.SPCCMH_Id && b.MI_Id == data.MI_Id
                                                   && b.ASMAY_Id == data.ASMAY_Id && b.SPCCMH_ActiveFlag == true && a.SPCCMH_ActiveFlag == true)
                                                   select new EventsStudentRecordDTO
                                                   {
                                                       house_idcls = a.SPCCMH_Id,
                                                       housenamecls = a.SPCCMH_HouseName
                                                   }).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                            data.listof_class = (from c in _context.admissionClass
                                                 from b in _context.admissionyearstudent
                                                 where (b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                                                 select new EventsStudentRecordDTO
                                                 {
                                                     class_id = c.ASMCL_Id,
                                                     classname = c.ASMCL_ClassName,
                                                 }).Distinct().OrderBy(c => c.class_id).ToArray();

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

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        // Editdata 

        public async Task<EventsStudentRecordDTO> editdata(EventsStudentRecordDTO data)
        {
            try
            {
                var editdata = (from a in _context.SPCC_Events_Students_DMO
                                from b in _context.EventsStudentRecordDMO
                                from c in _context.SportStudentHouseDivisionDMO
                                from d in _context.SportMasterHouseDMO
                                where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && b.AMST_Id == c.AMST_Id && c.SPCCMH_Id == d.SPCCMH_Id && a.SPCCEST_Id == data.SPCCEST_Id && b.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && b.SPCCESTR_Id == data.SPCCESTR_Id)
                                select new EventsStudentRecordDTO
                                {
                                    SPCCEST_Id = a.SPCCEST_Id,
                                    SPCCME_Id = a.SPCCME_Id,
                                    SPCCMCL_Id = a.SPCCMCL_Id,
                                    SPCCMCC_Id = a.SPCCMCC_Id,
                                    SPCCMSCC_Id = a.SPCCMSCC_Id,
                                    SPCCMUOM_Id = a.SPCCMUOM_Id,
                                    SPCCEST_House_Class_Flag = a.SPCCEST_House_Class_Flag,
                                    SPCCEST_OldRecord = a.SPCCEST_OldRecord,
                                    SPCCEST_Remarks = a.SPCCEST_Remarks,
                                    SPCCMSCCG_Id = a.SPCCMSCCG_Id,
                                    SPCCMH_Id = c.SPCCMH_Id,
                                    SPCCMH_HouseName = d.SPCCMH_HouseName,
                                    SPCCESTR_Id = b.SPCCESTR_Id,
                                    housename = d.SPCCMH_HouseName,
                                }).Distinct().ToList();

                data.editDetails = editdata.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //EditDetailsSRKVS
        public async Task<EventsStudentRecordDTO> EditDetailsSRKVS(EventsStudentRecordDTO data)
        {
            try
            {

                if (data.sportsName == "SRKVS")
                {

                    data.eventmappingList = (from a in _context.EventsMappingDMO
                                             from b in _context.MasterEventsDMO
                                             from c in _context.MasterEventVenueDMO
                                             from y in _context.AcademicYear
                                             from z in _context.SPCC_Events_Students_DMO

                                             where (a.SPCCME_Id == b.SPCCME_Id && a.SPCCMEV_Id == c.SPCCMEV_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == y.ASMAY_Id && a.MI_Id == data.MI_Id && b.SPCCME_Id == data.SPCCME_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new EventsMappingDTO
                                             {
                                                 SPCCE_Id = a.SPCCE_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                                 SPCCE_StartDate = a.SPCCE_StartDate,
                                                 SPCCE_EndDate = a.SPCCE_EndDate,
                                                 SPCCE_StartTime = a.SPCCE_StartTime,
                                                 SPCCE_EndTime = a.SPCCE_EndTime,
                                                 SPCCE_Remarks = a.SPCCE_Remarks,
                                                 SPCCE_ActiveFlag = a.SPCCE_ActiveFlag,
                                                 SPCCME_Id = b.SPCCME_Id,
                                                 SPCCME_EventName = b.SPCCME_EventName,
                                                 SPCCMEV_Id = c.SPCCMEV_Id,
                                                 SPCCMEV_EventVenue = c.SPCCMEV_EventVenue,
                                                 SPCCE_SponsorFlag = a.SPCCE_SponsorFlag,
                                                 StartDate = z.SPCCEST_StardDate,
                                                 EndDate = z.SPCCEST_EndDate,
                                                 //SPCCMEV_Id=z.SPCCMEV_Id,
                                                 // SPCCESTR_RecordBrokenFlag = z.spccestR_RecordBrokenFlag,
                                                 ASMAY_Year = y.ASMAY_Year,
                                                 SPCCEST_Remarks = z.SPCCEST_Remarks
                                             }).ToArray();
                }
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SPC_StudentSports_WithoutMappingList_srkvs";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SPCCEST_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.SPCCEST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSAY_Id",
                      SqlDbType.BigInt)
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
                            }
                            data.editstulist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                // data.listof_class = _context.admissionClass.Where(R => R.MI_Id == data.MI_Id && R.ASMCL_ActiveFlag==true).Distinct().ToArray();

                data.eventname = (from a in _context.MasterEventsDMO
                                  from b in _context.EventsMappingDMO
                                  where (a.MI_Id == b.MI_Id && a.SPCCME_Id == b.SPCCME_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.SPCCME_ActiveFlag == true && b.SPCCE_ActiveFlag == true)
                                  select a).Distinct().OrderBy(t => t.SPCCME_Id).ToArray();

                data.listof_class = (from c in _context.admissionClass
                                     from b in _context.admissionyearstudent
                                     where (b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                                     select new EventsStudentRecordDTO
                                     {
                                         class_id = c.ASMCL_Id,
                                         classname = c.ASMCL_ClassName,
                                     }).Distinct().OrderBy(c => c.class_id).ToArray();
                //data.sectionList = (from a in _context.masterSection
                //                    from c in _context.admissionClass
                //                    from b in _context.admissionyearstudent
                //                    where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == a.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && clssids.Contains(b.ASMCL_Id) && b.AMAY_ActiveFlag == 1 && a.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                //                    select a).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                data.sectionList = _context.masterSection.Where(R => R.MI_Id == data.MI_Id && R.ASMC_ActiveFlag == 1).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public EventsStudentRecordDTO deactivate(EventsStudentRecordDTO obj)
        {

            try
            {
                var query = _context.SPCC_Events_Students_DMO.Where(t => t.MI_Id == obj.MI_Id && t.SPCCEST_Id == obj.SPCCEST_Id).SingleOrDefault();

                if (query.SPCCEST_ActiveFlag == true)
                {
                    query.SPCCEST_ActiveFlag = false;
                }
                else
                {
                    query.SPCCEST_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _context.Update(query);
                var contactExists = _context.SaveChanges();
                if (contactExists > 0)
                {
                    obj.returnVal2 = true;
                }
                else
                {
                    obj.returnVal2 = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return obj;
        }

        public async Task<EventsStudentRecordDTO> getStudents(EventsStudentRecordDTO obj)
        {
            try
            {
                if (obj.sportsName == "SRKVS")
                {
                    string classss_idss = "0";
                    string sect_ids = "0";
                    if (obj.clslistdat234 != null && obj.clslistdat234.Length > 0)
                    {
                        foreach (var item2 in obj.clslistdat234)
                        {
                            classss_idss = classss_idss + "," + item2.class_id;
                        }
                    }
                    if (obj.sectonlst != null && obj.sectonlst.Length > 0)
                    {
                        foreach (var it in obj.sectonlst)
                        {
                            sect_ids = sect_ids + "," + it.ASMS_Id;
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SPC_Get_Studentdata_SRKVS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                        {
                            Value = obj.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = obj.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                        {
                            Value = classss_idss
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                         SqlDbType.VarChar
                         )
                        {
                            Value = sect_ids
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
                            }
                            obj.studentList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SPC_Get_Studentdata";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = obj.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = obj.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.BigInt)
                        {
                            Value = obj.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                         SqlDbType.BigInt)
                        {
                            Value = obj.ASMS_Id
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
                            }
                            obj.studentList = retObject.ToArray();
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
            return obj;
        }

        public EventsStudentRecordDTO getevent(EventsStudentRecordDTO data)
        {
            try
            {


                //data.eventname =(from a in  _context.MasterEventsDMO
                //                 from b in _context.
                //                 where(a.SPCCME_Id== && a.MI_Id == data.MI_Id && a.act == true /*&& d.SPCCME_Flag == data.radiotype*/).Select(d => new EventsSponsorDTO { SPCCME_Id = d.SPCCME_Id, spccmE_EventName = d.SPCCME_EventName }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EventsStudentRecordDTO onChangeActivities(EventsStudentRecordDTO data)
        {
            try
            {
                data.groupsportdata = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_SCCFlag == data.SPCCMSCCG_SCCFlag && d.SPCCMSCCG_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EventsStudentRecordDTO onhousechage(EventsStudentRecordDTO obj)
        {
            try
            {
                List<long> sect_ids = new List<long>();
                if (obj.sectonlst.Count() > 0)
                {
                    foreach (var it in obj.sectonlst)
                    {
                        sect_ids.Add(it.ASMS_Id);
                    }
                }


                var student = (from h1 in _context.SportMasterHouseDMO
                               from h in _context.SportStudentHouseDivisionDMO
                               from y in _context.admissionyearstudent
                               from st in _context.Adm_M_Student
                               from c in _context.admissionClass
                               from s in _context.masterSection
                               where (h1.SPCCMH_Id == h.SPCCMH_Id && y.ASMAY_Id == h.ASMAY_Id && y.ASMCL_Id == h.ASMCL_Id && y.ASMS_Id == h.ASMS_Id && y.AMST_Id == h.AMST_Id && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.AMST_Id == st.AMST_Id && st.AMST_SOL.Equals("S") && y.AMAY_ActiveFlag == 1 && h.SPCCMH_ActiveFlag == true && h.MI_Id == obj.MI_Id && y.ASMAY_Id == obj.ASMAY_Id && h.ASMCL_Id == obj.ASMCL_Id && h.SPCCMH_Id == obj.SPCCMH_Id && sect_ids.Contains(h.ASMS_Id) /*h.ASMS_Id==obj.ASMS_Id*/)
                               select new EventsStudentRecordDTO
                               {
                                   AMST_Id = st.AMST_Id,
                                   studentName = st.AMST_FirstName + (string.IsNullOrEmpty(st.AMST_MiddleName) ? "" : ' ' + st.AMST_MiddleName) + (string.IsNullOrEmpty(st.AMST_LastName) ? "" : ' ' + st.AMST_LastName),
                                   AMST_AdmNo = st.AMST_AdmNo,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMC_SectionName = s.ASMC_SectionName,
                                   SPCCMH_Id = h.SPCCMH_Id,
                                   SPCCMH_HouseName = h1.SPCCMH_HouseName

                               }).Distinct().OrderBy(n => n.studentName).ToList();
                if (student.Count > 0)
                {
                    obj.studentList = student.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsStudentRecordDTO get_student_info(EventsStudentRecordDTO obj)
        {
            try
            {
                List<long> amstids = new List<long>();
                if (obj.studentids.Count() > 0)
                {
                    foreach (var it in obj.studentids)
                    {
                        amstids.Add(it.amsT_Id);
                    }
                }

                var student = (from h1 in _context.SportMasterHouseDMO
                               from h in _context.SportStudentHouseDivisionDMO
                               from y in _context.admissionyearstudent
                               from st in _context.Adm_M_Student
                               from c in _context.admissionClass
                               from s in _context.masterSection
                               where (h1.SPCCMH_Id == h.SPCCMH_Id && y.ASMAY_Id == h.ASMAY_Id && y.ASMCL_Id == h.ASMCL_Id && y.ASMS_Id == h.ASMS_Id && y.AMST_Id == h.AMST_Id && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id && h.AMST_Id == st.AMST_Id && st.AMST_SOL.Equals("S") && y.AMAY_ActiveFlag == 1 && h.SPCCMH_ActiveFlag == true && h.MI_Id == obj.MI_Id && h.ASMAY_Id == obj.ASMAY_Id && h.ASMCL_Id == obj.ASMCL_Id && h.ASMS_Id == obj.ASMS_Id && amstids.Contains(y.AMST_Id))
                               select new EventsStudentRecordDTO
                               {
                                   AMST_Id = st.AMST_Id,
                                   studentName = st.AMST_FirstName + (string.IsNullOrEmpty(st.AMST_MiddleName) ? "" : ' ' + st.AMST_MiddleName) + (string.IsNullOrEmpty(st.AMST_LastName) ? "" : ' ' + st.AMST_LastName),
                                   AMST_AdmNo = st.AMST_AdmNo,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMC_SectionName = s.ASMC_SectionName,
                                   SPCCMH_HouseName = h1.SPCCMH_HouseName,
                                   SPCCMH_Id = h1.SPCCMH_Id,

                               }).Distinct().OrderBy(n => n.studentName).ToList();



                if (student.Count > 0)
                {
                    obj.studentList = student.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsStudentRecordDTO get_modeldata(EventsStudentRecordDTO obj)
        {
            try
            {
                obj.modlastudlist = (from a in _context.SPCC_Events_Students_DMO
                                     from b in _context.EventsStudentRecordDMO
                                     from y in _context.admissionyearstudent
                                     from st in _context.Adm_M_Student
                                     from c in _context.admissionClass
                                     from s in _context.masterSection
                                     where (a.SPCCEST_Id == b.SPCCEST_Id && b.AMST_Id == y.AMST_Id && y.ASMCL_Id == c.ASMCL_Id && y.ASMS_Id == s.ASMS_Id && st.AMST_Id == y.AMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == obj.MI_Id && a.SPCCEST_Id == obj.SPCCEST_Id && y.ASMAY_Id == obj.ASMAY_Id && y.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true && s.ASMC_ActiveFlag == 1)
                                     select new EventsStudentRecordDTO
                                     {

                                         SPCCESTR_Id = b.SPCCESTR_Id,
                                         SPCCESTR_Points = b.SPCCESTR_Points,
                                         SPCCESTR_Rank = b.SPCCESTR_Rank,
                                         SPCCESTR_Remarks = b.SPCCESTR_Remarks,
                                         SPCCESTR_ActiveFlag = b.SPCCESTR_ActiveFlag,
                                         SPCCESTR_RecordBrokenFlag = b.SPCCESTR_RecordBrokenFlag,
                                         AMST_Id = st.AMST_Id,
                                         studentName = st.AMST_FirstName + (string.IsNullOrEmpty(st.AMST_MiddleName) ? "" : ' ' + st.AMST_MiddleName) + (string.IsNullOrEmpty(st.AMST_LastName) ? "" : ' ' + st.AMST_LastName),
                                         AMST_AdmNo = st.AMST_AdmNo,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = s.ASMC_SectionName,

                                     }).Distinct().OrderBy(t => t.AMST_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsStudentRecordDTO get_SportsName(EventsStudentRecordDTO obj)
        {
            try
            {
                var sportsCC = _context.MasterSportsCCNameDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && d.SPCCMSCC_ActiveFlag == true).ToList();
                if (sportsCC.Count > 0)
                {
                    obj.sportsCCList = sportsCC.ToArray();
                }

                //var sportsCC = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_Under == obj.SPCCMSCCG_Id && d.SPCCMSCCG_ActiveFlag == true).ToList();
                //if (sportsCC.Count > 0)
                //{
                //    obj.sportsCCList = sportsCC.ToArray();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsStudentRecordDTO get_uom_Name(EventsStudentRecordDTO obj)
        {
            try
            {
                var uom = (from a in _context.SportMasterUOMDMO
                           from b in _context.MasterSportsCCNameUOM_DMO
                           from c in _context.MasterSportsCCNameDMO
                           where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.SPCCMUOM_Id == b.SPCCMUOM_Id && b.SPCCMSCC_Id == c.SPCCMSCC_Id && a.MI_Id == obj.MI_Id && b.SPCCMSCC_Id == obj.SPCCMSCC_Id && b.SPCCMSCCUOM_ActiveFlag == true)
                           select new EventsStudentRecordDTO
                           {
                               SPCCMUOM_Id = a.SPCCMUOM_Id,
                               SPCCMUOM_UOMName = a.SPCCMUOM_UOMName,
                           }).Distinct().OrderBy(t => t.SPCCMUOM_Id).ToList();

                if (uom.Count > 0)
                {
                    obj.uomList = uom.ToArray();
                }
                //SPCC_Master_SportsCCName
                //obj.getsubsubevent = (from a in _context.MasterSportsCCNameDMO
                //                      from b in _context.MasterSportsCCGroupDMO
                //                      where (a.SPCCMSCCG_Id == b.SPCCMSCCG_Id && a.MI_Id == obj.MI_Id && b.SPCCMSCCG_ActiveFlag == true && b.SPCCMSCCG_Under == obj.SPCCMSCCG_Id && a.SPCCMSCC_Id == obj.SPCCMSCC_Id)
                //                      select b

                //                      ).Distinct().ToArray();
                // obj.getsubsubevent = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                obj.getsubsubevent = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Level == obj.SPCCMSCC_Id).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();
                //obj.getsubsubevent=_context.

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsStudentRecordDTO Deactivatestud(EventsStudentRecordDTO obj)
        {

            try
            {
                var query = _context.EventsStudentRecordDMO.Where(t => t.MI_Id == obj.MI_Id && t.SPCCESTR_Id == obj.SPCCESTR_Id).SingleOrDefault();

                if (query.SPCCESTR_ActiveFlag == true)
                {
                    query.SPCCESTR_ActiveFlag = false;
                }
                else
                {
                    query.SPCCESTR_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _context.Update(query);
                var contactExists = _context.SaveChanges();
                if (contactExists > 0)
                {
                    obj.returnVal2 = true;
                }
                else
                {
                    obj.returnVal2 = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return obj;
        }

        public EventsStudentRecordDTO classChange(EventsStudentRecordDTO obj)
        {
            try
            {
                obj.sectionList = (from a in _context.masterSection
                                   from c in _context.admissionClass
                                   from b in _context.admissionyearstudent
                                   where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == a.ASMS_Id && a.MI_Id == obj.MI_Id && b.ASMAY_Id == obj.ASMAY_Id && b.ASMCL_Id == obj.ASMCL_Id && b.AMAY_ActiveFlag == 1 && a.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                                   select a).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsStudentRecordDTO get_class_house(EventsStudentRecordDTO obj)
        {
            try
            {
                obj.classList = (from c in _context.admissionClass
                                 from b in _context.admissionyearstudent
                                 where (b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == obj.MI_Id && b.ASMAY_Id == obj.ASMAY_Id && b.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                                 select c).Distinct().OrderBy(c => c.ASMCL_Order).ToArray();

                obj.listof_class = (from c in _context.admissionClass
                                    from b in _context.admissionyearstudent
                                    where (b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == obj.MI_Id && b.ASMAY_Id == obj.ASMAY_Id && b.AMAY_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true)
                                    select new EventsStudentRecordDTO
                                    {
                                        class_id = c.ASMCL_Id,
                                        classname = c.ASMCL_ClassName,
                                    }).Distinct().OrderBy(c => c.class_id).ToArray();

                obj.houselist = (from a in _context.SportMasterHouseDMO
                                 from b in _context.SportStudentHouseDivisionDMO
                                 where (a.MI_Id == b.MI_Id && a.SPCCMH_Id == b.SPCCMH_Id && b.MI_Id == obj.MI_Id && b.SPCCMH_ActiveFlag == true && a.SPCCMH_ActiveFlag == true)
                                 select new EventsStudentRecordDTO { house_id = a.SPCCMH_Id, housename = a.SPCCMH_HouseName }).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                obj.houselistclass = (from a in _context.SportMasterHouseDMO
                                      from b in _context.SportStudentHouseDivisionDMO
                                      where (a.MI_Id == b.MI_Id && a.SPCCMH_Id == b.SPCCMH_Id && b.MI_Id == obj.MI_Id && b.ASMAY_Id == obj.ASMAY_Id && b.SPCCMH_ActiveFlag == true && a.SPCCMH_ActiveFlag == true)
                                      select new EventsStudentRecordDTO { house_idcls = a.SPCCMH_Id, housenamecls = a.SPCCMH_HouseName }).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                obj.eventname = (from a in _context.MasterEventsDMO
                                 from b in _context.EventsMappingDMO
                                 where (a.MI_Id == b.MI_Id && a.SPCCME_Id == b.SPCCME_Id && a.MI_Id == obj.MI_Id && b.ASMAY_Id == obj.ASMAY_Id && a.SPCCME_ActiveFlag == true && b.SPCCE_ActiveFlag == true)
                                 select a).Distinct().OrderBy(t => t.SPCCME_Id).ToArray();

                obj.sectionList = (from a in _context.masterSection
                                   from b in _context.admissionyearstudent
                                   where (b.ASMS_Id == a.ASMS_Id && a.MI_Id == obj.MI_Id && b.ASMAY_Id == obj.ASMAY_Id && b.AMAY_ActiveFlag == 1 && a.ASMC_ActiveFlag == 1)
                                   select a).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public async Task<EventsStudentRecordDTO> get_StudentAgeFilter(EventsStudentRecordDTO data)
        {
            //data.SPCCMCC_CCAgeFlag = _context.MasterCompitionCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.SPCCMCC_Id == data.SPCCMCC_Id).SingleOrDefault().SPCCMCC_CCAgeFlag;

            //if (data.SPCCMCC_CCAgeFlag == true)
            //{
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentMappingAgeReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.SPCCMCC_Id
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
                        }
                        var studentList = retObject.ToList();

                        List<long> house_ids = new List<long>();
                        List<long> amst_ids = new List<long>();
                        foreach (var item in studentList)
                        {
                            house_ids.Add(item.SPCCMH_Id);
                        }
                        data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                        foreach (var item2 in studentList)
                        {
                            amst_ids.Add(item2.AMST_Id);
                        }
                        List<long> class_ids = new List<long>();
                        foreach (var item3 in studentList)
                        {
                            class_ids.Add(item3.ASMCL_Id);
                        }
                        List<long> section_ids = new List<long>();
                        foreach (var item4 in studentList)
                        {
                            section_ids.Add(item4.ASMS_Id);
                        }

                        data.studentList = (from a in _context.Adm_M_Student
                                            from b in _context.SportStudentHouseDivisionDMO
                                            from c in _context.SportMasterHouseDMO
                                            from y in _context.admissionyearstudent
                                            from d in _context.admissionClass
                                            from e in _context.masterSection
                                            from gconfn in _context.GenConfig

                                            where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                            select new EventsStudentRecordDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = e.ASMC_SectionName,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                            }).Distinct().OrderBy(t => t.studentName).ToArray();

                        data.classList = (from a in _context.admissionClass
                                          from b in _context.admissionyearstudent
                                          where (a.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && class_ids.Contains(b.ASMCL_Id))
                                          select a).Distinct().ToArray();

                        data.sectionList = (from a in _context.masterSection
                                            from b in _context.admissionyearstudent
                                            where (a.MI_Id == data.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && section_ids.Contains(b.ASMS_Id))
                                            select a).Distinct().ToArray();

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
            //}
            //else {

            //}

            return data;
        }

        public async Task<EventsStudentRecordDTO> houseWiseCompStudentList(EventsStudentRecordDTO data)
        {
            try
            {
                string house_idss = "0";

                List<long> hous_ids = new List<long>();

                foreach (var item in data.houslistdat)
                {
                    hous_ids.Add(Convert.ToInt32(item.SPCCMH_Id));
                }
                for (int s = 0; s < hous_ids.Count(); s++)
                {
                    house_idss = house_idss + ',' + hous_ids[s].ToString();
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HouseAgeEdit";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                    SqlDbType.VarChar)
                    {
                        Value = house_idss
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.SPCCMCC_Id
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
                        }
                        var studentList = retObject.ToList();

                        List<long> house_ids = new List<long>();
                        List<long> amst_ids = new List<long>();
                        foreach (var item in studentList)
                        {
                            house_ids.Add(item.SPCCMH_Id);
                        }
                        data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                        foreach (var item2 in studentList)
                        {
                            amst_ids.Add(item2.AMST_Id);
                        }
                        List<long> class_ids = new List<long>();
                        foreach (var item3 in studentList)
                        {
                            class_ids.Add(item3.ASMCL_Id);
                        }
                        List<long> section_ids = new List<long>();
                        foreach (var item4 in studentList)
                        {
                            section_ids.Add(item4.ASMS_Id);
                        }

                        data.studentList = (from a in _context.Adm_M_Student
                                            from b in _context.SportStudentHouseDivisionDMO
                                            from c in _context.SportMasterHouseDMO
                                            from y in _context.admissionyearstudent
                                            from d in _context.admissionClass
                                            from e in _context.masterSection
                                            from gconfn in _context.GenConfig

                                            where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                            select new EventsStudentRecordDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = e.ASMC_SectionName,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                            }).Distinct().OrderBy(t => t.studentName).ToArray();

                        data.classList = (from a in _context.admissionClass
                                          from b in _context.admissionyearstudent
                                          where (a.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && class_ids.Contains(b.ASMCL_Id))
                                          select a).Distinct().ToArray();

                        data.sectionList = (from a in _context.masterSection
                                            from b in _context.admissionyearstudent
                                            where (a.MI_Id == data.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && section_ids.Contains(b.ASMS_Id))
                                            select a).Distinct().ToArray();


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

        public async Task<EventsStudentRecordDTO> classWiseCompStudentList(EventsStudentRecordDTO data)
        {
            try
            {
                string house_idss = "0";
                string classss_idss = "0";
                List<long> hous_ids = new List<long>();
                List<long> cls_ids = new List<long>();
                foreach (var item2 in data.clslistdat)
                {

                    cls_ids.Add(item2.ASMCL_Id);
                }
                for (int c = 0; c < cls_ids.Count(); c++)
                {
                    classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                }

                if (data.SPCCEST_House_Class_Flag == "CC")
                {
                    foreach (var item in data.houslistdat)
                    {

                        hous_ids.Add(Convert.ToInt32(item.SPCCMH_Id));
                    }
                    for (int h = 0; h < hous_ids.Count(); h++)
                    {
                        house_idss = house_idss + ',' + hous_ids[h].ToString();
                    }
                }
                else if (data.SPCCEST_House_Class_Flag == "House")
                {
                    foreach (var item in data.houslistdattt2)
                    {

                        hous_ids.Add(Convert.ToInt32(item.house_id));
                    }
                    for (int h = 0; h < hous_ids.Count(); h++)
                    {
                        house_idss = house_idss + ',' + hous_ids[h].ToString();
                    }

                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ClassHouseComAgeEdit";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                    SqlDbType.VarChar)
                    {
                        Value = house_idss
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.SPCCMCC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@class_id",
                  SqlDbType.VarChar)
                    {
                        Value = classss_idss
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
                        }
                        var studentList = retObject.ToList();

                        List<long> house_ids = new List<long>();
                        List<long> amst_ids = new List<long>();
                        foreach (var item in studentList)
                        {
                            house_ids.Add(item.SPCCMH_Id);
                        }
                        data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                        foreach (var item2 in studentList)
                        {
                            amst_ids.Add(item2.AMST_Id);
                        }
                        List<long> class_ids = new List<long>();
                        foreach (var item3 in studentList)
                        {
                            class_ids.Add(item3.ASMCL_Id);
                        }
                        List<long> section_ids = new List<long>();
                        foreach (var item4 in studentList)
                        {
                            section_ids.Add(item4.ASMS_Id);
                        }

                        data.studentList = (from a in _context.Adm_M_Student
                                            from b in _context.SportStudentHouseDivisionDMO
                                            from c in _context.SportMasterHouseDMO
                                            from y in _context.admissionyearstudent
                                            from d in _context.admissionClass
                                            from e in _context.masterSection
                                            from gconfn in _context.GenConfig

                                            where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                            select new EventsStudentRecordDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = e.ASMC_SectionName,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                            }).Distinct().OrderBy(t => t.studentName).ToArray();

                        data.sectionList = (from a in _context.masterSection
                                            from b in _context.admissionyearstudent
                                            where (a.MI_Id == data.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && section_ids.Contains(b.ASMS_Id))
                                            select a).Distinct().ToArray();


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

        public async Task<EventsStudentRecordDTO> sectionWiseCompStudentList(EventsStudentRecordDTO data)
        {
            try
            {
                string house_idss = "0";
                string classss_idss = "0";
                List<long> cls_ids = new List<long>();
                List<long> hous_ids = new List<long>();

                string sectionsss_idss = "0";
                List<long> sect_ids = new List<long>();



                foreach (var item2 in data.sectonlst)
                {
                    sect_ids.Add(item2.ASMS_Id);
                }
                for (int c = 0; c < sect_ids.Count(); c++)
                {
                    sectionsss_idss = sectionsss_idss + ',' + sect_ids[c].ToString();
                }


                if (data.SPCCEST_House_Class_Flag == "CC")
                {
                    foreach (var item2 in data.clslistdat)
                    {
                        cls_ids.Add(item2.ASMCL_Id);
                    }
                    for (int c = 0; c < cls_ids.Count(); c++)
                    {
                        classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                    }
                    foreach (var item in data.houslistdat)
                    {
                        hous_ids.Add(Convert.ToInt32(item.SPCCMH_Id));
                    }
                    for (int h = 0; h < hous_ids.Count(); h++)
                    {
                        house_idss = house_idss + ',' + hous_ids[h].ToString();
                    }
                }
                else if (data.SPCCEST_House_Class_Flag == "House")
                {
                    foreach (var item2 in data.clslistdat)
                    {
                        cls_ids.Add(item2.ASMCL_Id);
                    }
                    for (int c = 0; c < cls_ids.Count(); c++)
                    {
                        classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                    }
                    foreach (var item in data.houslistdattt2)
                    {
                        hous_ids.Add(Convert.ToInt32(item.house_id));
                    }
                    for (int h = 0; h < hous_ids.Count(); h++)
                    {
                        house_idss = house_idss + ',' + hous_ids[h].ToString();
                    }

                }
                else if (data.SPCCEST_House_Class_Flag == "CS")
                {
                    foreach (var item2 in data.clslistdat234)
                    {
                        cls_ids.Add(item2.class_id);
                    }
                    for (int c = 0; c < cls_ids.Count(); c++)
                    {
                        classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                    }
                    //foreach (var item in data.houslistdattt2)
                    //{
                    //    hous_ids.Add(Convert.ToInt32(item.house_id));
                    //}
                    //for (int h = 0; h < hous_ids.Count(); h++)
                    //{
                    //    house_idss = house_idss + ',' + hous_ids[h].ToString();
                    //}


                }

                if (data.SPCCEST_House_Class_Flag != "CS")
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SectionClassHouseComAgeEdit";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                        SqlDbType.VarChar)
                        {
                            Value = house_idss
                        });

                        cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.SPCCMCC_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@class_id",
                  SqlDbType.VarChar)
                        {
                            Value = classss_idss
                        });
                        cmd.Parameters.Add(new SqlParameter("@section_id",
                  SqlDbType.VarChar)
                        {
                            Value = sectionsss_idss
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
                            }
                            var studentList = retObject.ToList();

                            List<long> house_ids = new List<long>();
                            List<long> amst_ids = new List<long>();
                            foreach (var item in studentList)
                            {
                                house_ids.Add(item.SPCCMH_Id);
                            }
                            data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                            foreach (var item2 in studentList)
                            {
                                amst_ids.Add(item2.AMST_Id);
                            }
                            List<long> class_ids = new List<long>();
                            foreach (var item3 in studentList)
                            {
                                class_ids.Add(item3.ASMCL_Id);
                            }
                            List<long> section_ids = new List<long>();
                            foreach (var item4 in studentList)
                            {
                                section_ids.Add(item4.ASMS_Id);
                            }

                            data.studentList = (from a in _context.Adm_M_Student
                                                from b in _context.SportStudentHouseDivisionDMO
                                                from c in _context.SportMasterHouseDMO
                                                from y in _context.admissionyearstudent
                                                from d in _context.admissionClass
                                                from e in _context.masterSection
                                                from gconfn in _context.GenConfig

                                                where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                                select new EventsStudentRecordDTO
                                                {
                                                    AMST_Id = a.AMST_Id,
                                                    AMST_AdmNo = a.AMST_AdmNo,
                                                    SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                    ASMCL_ClassName = d.ASMCL_ClassName,
                                                    ASMC_SectionName = e.ASMC_SectionName,
                                                    studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                    IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                                }).Distinct().OrderBy(t => t.studentName).ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else if (data.SPCCEST_House_Class_Flag == "CS")
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SectionClassCompCat_AgeFilter";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        //cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = house_idss
                        //});

                        cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.SPCCMCC_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@clss_Ids",
                  SqlDbType.VarChar)
                        {
                            Value = classss_idss
                        });
                        cmd.Parameters.Add(new SqlParameter("@sect_Ids",
                  SqlDbType.VarChar)
                        {
                            Value = sectionsss_idss
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
                            }
                            var studentList = retObject.ToList();

                            List<long> house_ids = new List<long>();
                            List<long> amst_ids = new List<long>();
                            foreach (var item in studentList)
                            {
                                house_ids.Add(item.SPCCMH_Id);
                            }
                            data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                            foreach (var item2 in studentList)
                            {
                                amst_ids.Add(item2.AMST_Id);
                            }
                            List<long> class_ids = new List<long>();
                            foreach (var item3 in studentList)
                            {
                                class_ids.Add(item3.ASMCL_Id);
                            }
                            List<long> section_ids = new List<long>();
                            foreach (var item4 in studentList)
                            {
                                section_ids.Add(item4.ASMS_Id);
                            }

                            data.studentList = (from a in _context.Adm_M_Student
                                                from b in _context.SportStudentHouseDivisionDMO
                                                from c in _context.SportMasterHouseDMO
                                                from y in _context.admissionyearstudent
                                                from d in _context.admissionClass
                                                from e in _context.masterSection
                                                from gconfn in _context.GenConfig

                                                where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                                select new EventsStudentRecordDTO
                                                {
                                                    AMST_Id = a.AMST_Id,
                                                    AMST_AdmNo = a.AMST_AdmNo,
                                                    SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                    ASMCL_ClassName = d.ASMCL_ClassName,
                                                    ASMC_SectionName = e.ASMC_SectionName,
                                                    studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                    IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                                }).Distinct().OrderBy(t => t.studentName).ToArray();

                            data.houselistclass = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select new EventsStudentRecordDTO { house_idcls = a.SPCCMH_Id, housenamecls = a.SPCCMH_HouseName }).Distinct().ToArray();


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
            return data;
        }

        public async Task<EventsStudentRecordDTO> houseWiseCompcatClssSectStudentList(EventsStudentRecordDTO data)
        {
            try
            {
                string house_idss = "0";
                string classss_idss = "0";
                List<long> cls_ids = new List<long>();
                List<long> hous_ids = new List<long>();

                string sectionsss_idss = "0";
                List<long> sect_ids = new List<long>();

                foreach (var item2 in data.sectonlst)
                {
                    sect_ids.Add(item2.ASMS_Id);
                }
                for (int c = 0; c < sect_ids.Count(); c++)
                {
                    sectionsss_idss = sectionsss_idss + ',' + sect_ids[c].ToString();
                }

                foreach (var item in data.hosueclssecllist)
                {
                    hous_ids.Add(Convert.ToInt32(item.house_idcls));
                }
                for (int s = 0; s < hous_ids.Count(); s++)
                {
                    house_idss = house_idss + ',' + hous_ids[s].ToString();
                }

                foreach (var item2 in data.clslistdat234)
                {
                    cls_ids.Add(item2.class_id);
                }
                for (int c = 0; c < cls_ids.Count(); c++)
                {
                    classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HouseSectionClassCompCat_AgeFilter";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@clss_Ids",
                    SqlDbType.VarChar)
                    {
                        Value = classss_idss
                    });
                    cmd.Parameters.Add(new SqlParameter("@sect_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = sectionsss_idss
                    });
                    cmd.Parameters.Add(new SqlParameter("@house_Ids",
                SqlDbType.VarChar)
                    {
                        Value = house_idss
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.SPCCMCC_Id
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
                        }
                        var studentList = retObject.ToList();

                        List<long> house_ids = new List<long>();
                        List<long> amst_ids = new List<long>();
                        foreach (var item in studentList)
                        {
                            house_ids.Add(item.SPCCMH_Id);
                        }
                        data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                        foreach (var item2 in studentList)
                        {
                            amst_ids.Add(item2.AMST_Id);
                        }
                        List<long> class_ids = new List<long>();
                        foreach (var item3 in studentList)
                        {
                            class_ids.Add(item3.ASMCL_Id);
                        }
                        List<long> section_ids = new List<long>();
                        foreach (var item4 in studentList)
                        {
                            section_ids.Add(item4.ASMS_Id);
                        }

                        data.studentList = (from a in _context.Adm_M_Student
                                            from b in _context.SportStudentHouseDivisionDMO
                                            from c in _context.SportMasterHouseDMO
                                            from y in _context.admissionyearstudent
                                            from d in _context.admissionClass
                                            from e in _context.masterSection
                                            from gconfn in _context.GenConfig

                                            where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                            select new EventsStudentRecordDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = e.ASMC_SectionName,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                            }).Distinct().OrderBy(t => t.studentName).ToArray();



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

        public async Task<EventsStudentRecordDTO> get_houseCatAgeFilter(EventsStudentRecordDTO data)
        {
            try
            {
                string house_idss = "0";

                List<long> hous_ids = new List<long>();

                foreach (var item in data.houslistdattt2)
                {
                    hous_ids.Add(Convert.ToInt32(item.house_id));
                }
                for (int s = 0; s < hous_ids.Count(); s++)
                {
                    house_idss = house_idss + ',' + hous_ids[s].ToString();
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HouseAgeEdit";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                    SqlDbType.VarChar)
                    {
                        Value = house_idss
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.SPCCMCC_Id
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
                        }
                        var studentList = retObject.ToList();

                        List<long> house_ids = new List<long>();
                        List<long> amst_ids = new List<long>();
                        foreach (var item in studentList)
                        {
                            house_ids.Add(item.SPCCMH_Id);
                        }
                        data.houselist = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select a).Distinct().ToArray();
                        foreach (var item2 in studentList)
                        {
                            amst_ids.Add(item2.AMST_Id);
                        }
                        List<long> class_ids = new List<long>();
                        foreach (var item3 in studentList)
                        {
                            class_ids.Add(item3.ASMCL_Id);
                        }
                        List<long> section_ids = new List<long>();
                        foreach (var item4 in studentList)
                        {
                            section_ids.Add(item4.ASMS_Id);
                        }

                        data.studentList = (from a in _context.Adm_M_Student
                                            from b in _context.SportStudentHouseDivisionDMO
                                            from c in _context.SportMasterHouseDMO
                                            from y in _context.admissionyearstudent
                                            from d in _context.admissionClass
                                            from e in _context.masterSection
                                            from gconfn in _context.GenConfig

                                            where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                            select new EventsStudentRecordDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = e.ASMC_SectionName,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                            }).Distinct().OrderBy(t => t.studentName).ToArray();

                        data.classList = (from a in _context.admissionClass
                                          from b in _context.admissionyearstudent
                                          where (a.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && class_ids.Contains(b.ASMCL_Id))
                                          select a).Distinct().ToArray();

                        data.sectionList = (from a in _context.masterSection
                                            from b in _context.admissionyearstudent
                                            where (a.MI_Id == data.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && section_ids.Contains(b.ASMS_Id))
                                            select a).Distinct().ToArray();


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

        public async Task<EventsStudentRecordDTO> comcatwise_classAgefilter(EventsStudentRecordDTO data)
        {
            try
            {
                string classss_idss = "0";
                List<long> cls_ids = new List<long>();
                foreach (var item2 in data.clslistdat234)
                {
                    cls_ids.Add(item2.class_id);
                }
                for (int c = 0; c < cls_ids.Count(); c++)
                {
                    classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ClassCompCat_AgeFilter";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@clss_Ids",
                    SqlDbType.VarChar)
                    {
                        Value = classss_idss
                    });

                    cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.SPCCMCC_Id
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
                        }
                        var studentList = retObject.ToList();

                        List<long> house_ids = new List<long>();
                        List<long> amst_ids = new List<long>();
                        foreach (var item in studentList)
                        {
                            house_ids.Add(item.SPCCMH_Id);
                        }
                        data.houselistclass = (from a in _context.SportMasterHouseDMO.Where(a => a.MI_Id == data.MI_Id && house_ids.Contains(a.SPCCMH_Id)) select new EventsStudentRecordDTO { house_idcls = a.SPCCMH_Id, housenamecls = a.SPCCMH_HouseName }).Distinct().ToArray();
                        foreach (var item2 in studentList)
                        {
                            amst_ids.Add(item2.AMST_Id);
                        }
                        List<long> class_ids = new List<long>();
                        foreach (var item3 in studentList)
                        {
                            class_ids.Add(item3.ASMCL_Id);
                        }
                        List<long> section_ids = new List<long>();
                        foreach (var item4 in studentList)
                        {
                            section_ids.Add(item4.ASMS_Id);
                        }

                        data.studentList = (from a in _context.Adm_M_Student
                                            from b in _context.SportStudentHouseDivisionDMO
                                            from c in _context.SportMasterHouseDMO
                                            from y in _context.admissionyearstudent
                                            from d in _context.admissionClass
                                            from e in _context.masterSection
                                            from gconfn in _context.GenConfig

                                            where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && b.SPCCMH_Id == c.SPCCMH_Id && b.ASMAY_Id == y.ASMAY_Id && b.ASMCL_Id == y.ASMCL_Id && b.ASMS_Id == y.ASMS_Id && y.AMST_Id == b.AMST_Id && y.ASMCL_Id == d.ASMCL_Id && y.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && amst_ids.Contains(a.AMST_Id) && class_ids.Contains(b.ASMCL_Id) && section_ids.Contains(b.ASMS_Id) && a.AMST_SOL == "S" && y.AMAY_ActiveFlag == 1 && d.MI_Id == gconfn.MI_Id)
                                            select new EventsStudentRecordDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                SPCCMH_HouseName = c.SPCCMH_HouseName,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = e.ASMC_SectionName,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                IVRMGC_SportsPointsDropdownFlg = gconfn.IVRMGC_SportsPointsDropdownFlg

                                            }).Distinct().OrderBy(t => t.studentName).ToArray();


                        data.sectionList = (from a in _context.masterSection
                                            from b in _context.admissionyearstudent
                                            where (a.MI_Id == data.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && section_ids.Contains(b.ASMS_Id))
                                            select a).Distinct().OrderBy(t => t.ASMC_Order).ToArray();


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

        //Kiosk Sports Winner List.
        public EventsStudentRecordDTO.SportsWinnersDTO kioskSportsWinners(EventsStudentRecordDTO kiosk)
        {
            EventsStudentRecordDTO.SportsWinnersDTO obj = new EventsStudentRecordDTO.SportsWinnersDTO();
            try
            {

                List<EventsStudentRecordDTO.SportsWinnersDTO> result = new List<EventsStudentRecordDTO.SportsWinnersDTO>();

                // var mo_id = _db.Organisation.Where(t => t.MO_Id == kiosk.MI_Id).Select(d => d.MO_Id).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sports_Winners_Kiosk";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MO_Id", SqlDbType.Int) { Value = kiosk.MO_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new EventsStudentRecordDTO.SportsWinnersDTO
                                {
                                    eventName = dataReader["SPCCME_EventName"].ToString(),
                                    studentName = dataReader["AMST_FirstName"].ToString(),
                                    sportsName = dataReader["SPCCMSCC_SportsCCName"].ToString(),
                                    SPCCESTR_Place = Convert.ToInt32(dataReader["SPCCESTR_Place"].ToString()),
                                    studentPhotoPath = dataReader["AMST_Photoname"].ToString(),
                                    miName = dataReader["MI_Name"].ToString()


                                });
                                obj.winnerList = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public async Task<string> sendSmsAsync(long MI_Id, string studentName, string evename, string Template, long? parent_no, string place, DateTime? SPCCE_StartDate)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                DateTime stardate = Convert.ToDateTime(SPCCE_StartDate);
                string startdate = stardate.ToString("dd'/'MM'/'yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, studentName);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[PLACE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, place);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[EVENT]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, evename);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, startdate);
                        sms = result;
                    }
                }
                sms = result;


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = parent_no.ToString();
                    //PHNO = "9771237044";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

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


                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Sports";
                    dmo2.To_FLag = "";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = "Delivered";
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public async Task<EventsStudentRecordDTO> get_ComCatgrylistClassWise(EventsStudentRecordDTO data)
        {
            try
            {
                string classss_idss = "0";
                List<long> cls_ids = new List<long>();
                foreach (var item2 in data.clslistdat234)
                {
                    cls_ids.Add(item2.class_id);
                }
                for (int c = 0; c < cls_ids.Count(); c++)
                {
                    classss_idss = classss_idss + ',' + cls_ids[c].ToString();
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ClassRadioWiseCompCat_AgeFilter";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = classss_idss
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
                        }
                        data.categoryListttCls = retObject.ToArray();
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

        //added By sanjeev SRKVS
        public async Task<EventsStudentRecordDTO> saveRecordSRKVS(EventsStudentRecordDTO obj)
        {
            try
            {
                if (obj.SPCCEST_Id == 0)
                {
                    List<long> amstids = new List<long>();
                    if (obj.student1.Count() > 0)
                    {
                        foreach (var it in obj.student1)
                        {
                            amstids.Add(it.amsT_Id);
                        }
                    }
                    //old
                    //var checkduplicate = (from a in _context.SPCC_Events_Students_DMO
                    //                      from b in _context.EventsStudentRecordDMO
                    //                      where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && a.MI_Id == obj.MI_Id && a.SPCCME_Id == obj.SPCCME_Id && a.SPCCMCL_Id == obj.SPCCMCL_Id && a.SPCCMCC_Id == obj.SPCCMCC_Id && a.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && a.SPCCMSCC_Id == obj.SPCCMSCC_Id && a.SPCCMUOM_Id == obj.SPCCMUOM_Id && a.SPCCEST_House_Class_Flag == obj.SPCCEST_House_Class_Flag && a.SPCCEST_OldRecord == obj.SPCCEST_OldRecord && a.SPCCEST_Remarks == obj.SPCCEST_Remarks && amstids.Contains(b.AMST_Id) && a.ASMAY_Id == obj.ASMAY_Id && a.ASMCL_Id == obj.ASMCL_Id)
                    //                      select a).ToList();

                    //new 
                    var checkduplicate = (from a in _context.SPCC_Events_Students_DMO
                                          from b in _context.EventsStudentRecordDMO
                                          where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && a.MI_Id == obj.MI_Id && a.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && a.SPCCMSCC_Id == obj.SPCCMSCC_Id && a.SPCCMUOM_Id == obj.SPCCMUOM_Id && a.SPCCEST_House_Class_Flag == obj.SPCCEST_House_Class_Flag && a.SPCCEST_OldRecord == obj.SPCCEST_OldRecord && a.SPCCEST_Remarks == obj.SPCCEST_Remarks && amstids.Contains(b.AMST_Id) && a.ASMAY_Id == obj.ASMAY_Id && a.ASMCL_Id == obj.ASMCL_Id)
                                          select a).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }

                    else
                    {
                        SPCC_Events_Students_DMO mapp = new SPCC_Events_Students_DMO();

                        //mapp.SPCCEST_Id = obj.SPCCEST_Id;
                        mapp.MI_Id = obj.MI_Id;
                        mapp.SPCCME_Id = obj.SPCCME_Id;
                        mapp.SPCCMCL_Id = obj.SPCCMCL_Id;
                        mapp.SPCCMCC_Id = obj.SPCCMCC_Id;
                        mapp.SPCCMSCC_Id = obj.SPCCMSCC_Id;
                        mapp.SPCCMUOM_Id = obj.SPCCMUOM_Id;
                        mapp.SPCCEST_House_Class_Flag = obj.SPCCEST_House_Class_Flag;
                        mapp.SPCCEST_OldRecord = obj.SPCCEST_OldRecord;
                        mapp.SPCCEST_Remarks = obj.SPCCEST_Remarks;
                        mapp.SPCCMSCCG_Id = obj.SPCCMSCCG_Id;
                        mapp.SPCCEST_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.ASMAY_Id = obj.ASMAY_Id;
                        mapp.SPCCEST_Id = obj.SPCCEST_Id;
                        mapp.SPCCMEV_Id = obj.SPCCMEV_Id;
                        mapp.SPCCEST_StardDate = obj.StartDate;
                        mapp.SPCCEST_EndDate = obj.EndDate;
                        // mapp.ASMCL_Id = obj.ASMCL_Id;                    
                        _context.Add(mapp);
                        if (obj.student1 != null && obj.student1.Length > 0)
                        {
                            for (int i = 0; i < obj.student1.Length; i++)
                            {
                                EventsStudentRecordDMO mapp2 = new EventsStudentRecordDMO();
                                mapp2.SPCCEST_Id = mapp.SPCCEST_Id;

                                mapp2.MI_Id = obj.MI_Id;
                                mapp2.AMST_Id = obj.student1[i].amsT_Id;
                                mapp2.SPCCESTR_Rank = "0";
                                mapp2.SPCCESTR_Points = 1;

                                if (obj.student1[i].spccestR_RecordBrokenFlag == null)
                                {
                                    mapp2.SPCCESTR_RecordBrokenFlag = "false";
                                }
                                else
                                {
                                    mapp2.SPCCESTR_RecordBrokenFlag = obj.student1[i].spccestR_RecordBrokenFlag;
                                }
                                mapp2.SPCCESTR_Remarks = null;
                                mapp2.SPCCESTR_ActiveFlag = true;
                                mapp2.CreatedDate = DateTime.Now;
                                mapp2.UpdatedDate = DateTime.Now;

                                _context.Add(mapp2);
                            }
                        }


                        int rowAffected = _context.SaveChanges();

                        if (rowAffected > 0)
                        {
                            obj.returnVal = "Saved!";

                            for (int i = 0; i < obj.student1.Count(); i++)
                            {
                                var temp_usr = obj.student1[i].amsT_Id;
                                string place = "";
                                string rank = obj.student1[i].spccestR_Rank;
                                if (rank == "1")
                                {
                                    place = "First";
                                }
                                else if (rank == "2")
                                {
                                    place = "Second";
                                }
                                else if (rank == "3")
                                {
                                    place = "Third";
                                }
                                long? parent_no = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatherMobleNo;

                                var fName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FirstName;
                                var mName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_MiddleName;
                                var lName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_LastName;

                                string studentName = fName + " " + mName + " " + lName;

                                var mail_id = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatheremailId;

                                string evename = _context.MasterEventsDMO.Single(t => t.MI_Id == obj.MI_Id && t.SPCCME_Id == obj.SPCCME_Id).SPCCME_EventName;
                                string Template = "";

                                var eventdate = (from c in _context.EventsMappingDMO
                                                 from d in _context.MasterEventsDMO
                                                 where (c.SPCCME_Id == obj.SPCCME_Id && c.ASMAY_Id == obj.ASMAY_Id
                                                 && c.MI_Id == obj.MI_Id && d.SPCCME_Id == c.SPCCME_Id
                                                 && d.MI_Id == c.MI_Id)
                                                 select new EventsStudentRecordDTO
                                                 {
                                                     SPCCE_StartDate = c.SPCCE_StartDate
                                                 }).Distinct().ToList();

                                obj.SPCCE_StartDate = eventdate.SingleOrDefault().SPCCE_StartDate;

                                //if (obj.SPCCESTR_Rank == "1" || obj.SPCCESTR_Rank == "2" || obj.SPCCESTR_Rank == "3")
                                if (obj.student1[i].spccestR_Rank == "1" || obj.student1[i].spccestR_Rank == "2" || obj.student1[i].spccestR_Rank == "3")
                                {
                                    Template = "Sportswinners";
                                }
                                else
                                {
                                    Template = "Sports";
                                }
                                if (obj.sendmail == true)
                                {
                                    sendmail(obj.MI_Id, studentName, evename, Template, mail_id, place, Convert.ToDateTime(obj.SPCCE_StartDate));

                                }
                                if (obj.sendsms == true)
                                {
                                    //SMS sss = new SMS(_db);
                                    //sss.sendSms1(obj.MI_Id, studentName, evename, Template, parent_no);

                                    string val2 = await sendSmsAsync(obj.MI_Id, studentName, evename, Template, parent_no, place, Convert.ToDateTime(obj.SPCCE_StartDate));
                                }
                            }
                        }
                        else
                        {
                            obj.returnVal = "Save Failed!";
                        }
                    }
                }
                else if (obj.SPCCEST_Id > 0)
                {
                    List<long> AMST_IdList = new List<long>();

                    var mapp = _context.SPCC_Events_Students_DMO.Where(d => d.SPCCEST_Id == obj.SPCCEST_Id && d.MI_Id == obj.MI_Id).SingleOrDefault();

                    mapp.MI_Id = obj.MI_Id;
                    mapp.SPCCME_Id = obj.SPCCME_Id;
                    mapp.SPCCMCL_Id = obj.SPCCMCL_Id;
                    mapp.SPCCMCC_Id = obj.SPCCMCC_Id;
                    mapp.SPCCMSCC_Id = obj.SPCCMSCC_Id;
                    mapp.SPCCMUOM_Id = obj.SPCCMUOM_Id;
                    mapp.SPCCEST_House_Class_Flag = obj.SPCCEST_House_Class_Flag;
                    mapp.SPCCEST_OldRecord = obj.SPCCEST_OldRecord;
                    mapp.SPCCEST_Remarks = obj.SPCCEST_Remarks;
                    mapp.SPCCMSCCG_Id = obj.SPCCMSCCG_Id;
                    mapp.SPCCEST_ActiveFlag = true;
                    mapp.CreatedDate = DateTime.Now;
                    mapp.UpdatedDate = DateTime.Now;
                    mapp.ASMAY_Id = obj.ASMAY_Id;
                    mapp.SPCCMEV_Id = obj.SPCCMEV_Id;
                    mapp.SPCCEST_StardDate = obj.StartDate;
                    mapp.SPCCEST_EndDate = obj.EndDate;

                    _context.Update(mapp);


                    var resultclass = _context.EventsStudentRecordDMO.Where(d => d.SPCCEST_Id == obj.SPCCEST_Id & d.MI_Id == obj.MI_Id && d.SPCCESTR_Rank == "0");
                    if (resultclass.Count() > 0)
                    {
                        foreach (var item in resultclass)
                        {
                            _context.Remove(item);

                        }
                        int M = _context.SaveChanges();
                    }


                    if (obj.student1 != null && obj.student1.Length > 0)
                    {

                        for (int i = 0; i < obj.student1.Length; i++)
                        {
                            var duplicate = _context.EventsStudentRecordDMO.Where(R => R.SPCCEST_Id == mapp.SPCCEST_Id && R.AMST_Id == obj.student1[i].amsT_Id).ToList();
                            if (duplicate.Count > 0)
                            {

                            }
                            else
                            {
                                EventsStudentRecordDMO mapp2 = new EventsStudentRecordDMO();
                                mapp2.SPCCEST_Id = mapp.SPCCEST_Id;

                                mapp2.MI_Id = obj.MI_Id;
                                mapp2.AMST_Id = obj.student1[i].amsT_Id;
                                mapp2.SPCCESTR_Rank = "0";
                                mapp2.SPCCESTR_Points = 1;

                                if (obj.student1[i].spccestR_RecordBrokenFlag == null)
                                {
                                    mapp2.SPCCESTR_RecordBrokenFlag = "false";
                                }
                                else
                                {
                                    mapp2.SPCCESTR_RecordBrokenFlag = obj.student1[i].spccestR_RecordBrokenFlag;
                                }
                                mapp2.SPCCESTR_Remarks = null;
                                mapp2.SPCCESTR_ActiveFlag = true;
                                mapp2.CreatedDate = DateTime.Now;
                                mapp2.UpdatedDate = DateTime.Now;

                                _context.Add(mapp2);
                            }

                        }
                    }

                    int s = _context.SaveChanges();
                    if (s > 0)
                    {
                        obj.returnVal = "updated";

                        for (int i = 0; i < obj.student1.Count(); i++)
                        {
                            var temp_usr = obj.student1[i].amsT_Id;

                            string place = "";
                            string rank = obj.student1[i].spccestR_Rank;
                            if (rank == "1")
                            {
                                place = "First";
                            }
                            else if (rank == "2")
                            {
                                place = "Second";
                            }
                            else if (rank == "3")
                            {
                                place = "Third";
                            }

                            long? parent_no = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatherMobleNo;

                            var fName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FirstName;
                            var mName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_MiddleName;
                            var lName = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_LastName;

                            string studentName = fName + " " + mName + " " + lName;

                            var mail_id = _context.Adm_M_Student.Single(t => t.MI_Id == obj.MI_Id && t.AMST_Id == temp_usr).AMST_FatheremailId;

                            string evename = _context.MasterEventsDMO.Single(t => t.MI_Id == obj.MI_Id && t.SPCCME_Id == obj.SPCCME_Id).SPCCME_EventName;
                            string Template = "";
                            var eventdate = (from c in _context.EventsMappingDMO
                                             from d in _context.MasterEventsDMO
                                             where (c.SPCCME_Id == obj.SPCCME_Id && c.ASMAY_Id == obj.ASMAY_Id
                                             && c.MI_Id == obj.MI_Id && d.SPCCME_Id == c.SPCCME_Id
                                             && d.MI_Id == c.MI_Id)
                                             select new EventsStudentRecordDTO
                                             {
                                                 SPCCE_StartDate = c.SPCCE_StartDate
                                             }).Distinct().ToList();

                            obj.SPCCE_StartDate = eventdate.SingleOrDefault().SPCCE_StartDate;

                            //if (obj.SPCCESTR_Rank == "1" || obj.SPCCESTR_Rank == "2" || obj.SPCCESTR_Rank == "3")
                            if (obj.student1[i].spccestR_Rank == "1" || obj.student1[i].spccestR_Rank == "2" || obj.student1[i].spccestR_Rank == "3")
                            {
                                Template = "Sportswinners";
                            }
                            else
                            {
                                Template = "Sports";
                            }
                            if (obj.sendmail == true)
                            {
                                sendmail(obj.MI_Id, studentName, evename, Template, mail_id, place, Convert.ToDateTime(obj.SPCCE_StartDate));

                            }
                            if (obj.sendsms == true)
                            {
                                //SMS sss = new SMS(_db);
                                //sss.sendSms1(obj.MI_Id, studentName, evename, Template, parent_no);

                                string val2 = await sendSmsAsync(obj.MI_Id, studentName, evename, Template, parent_no, place, Convert.ToDateTime(obj.SPCCE_StartDate));
                            }
                        }
                    }
                    else
                    {
                        obj.returnVal = "updated";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public async Task<EventsStudentRecordDTO> UpdateStudentSRKVS(EventsStudentRecordDTO obj)
        {
            try
            {
                if (obj.student1 != null && obj.student1.Length > 0)
                {
                    foreach (var d in obj.student1)
                    {
                        if (d.MultipleBroken != null && d.MultipleBroken.Length > 0)
                        {

                            foreach (var F in d.MultipleBroken)
                            {
                                var updates = _context.EventsStudentRecordDMO.Where(R => R.SPCCESTR_Id == F.spccestR_Id).ToList();
                                if (updates.Count > 0)
                                {
                                    var Multiupdate = _context.EventsStudentRecordDMO.Where(R => R.SPCCESTR_Id == F.spccestR_Id).FirstOrDefault();
                                    if (d.spccestR_Rank != "" && d.spccestR_Rank != "0")
                                    {
                                        Multiupdate.SPCCESTR_Rank = d.spccestR_Rank;
                                    }
                                    else
                                    {
                                        Multiupdate.SPCCESTR_Rank = "0";
                                    }

                                    Multiupdate.SPCCESTR_Points = F.indexValue;
                                    // Multiupdate.SPCCESTR_RecordBrokenFlag = d.spccestR_RecordBrokenFlag;
                                    Multiupdate.SPCCESTR_Remarks = d.spccestR_Remarks;
                                    Multiupdate.SPCCESTR_Value = F.spccestR_Value;
                                    Multiupdate.SPCCMUOM_Id = d.spccmuoM_Id;
                                    Multiupdate.SPCCESTR_EventRecordFlg = d.spccestR_EventRecordFlg;
                                    Multiupdate.UpdatedDate = DateTime.Now;
                                    _context.Update(Multiupdate);
                                }
                                else
                                {
                                    EventsStudentRecordDMO mapp2 = new EventsStudentRecordDMO();
                                    mapp2.SPCCEST_Id = d.spccesT_Id;
                                    mapp2.MI_Id = obj.MI_Id;
                                    mapp2.AMST_Id = d.amsT_Id;
                                    mapp2.SPCCESTR_Rank = d.spccestR_Rank;
                                    //mapp2.SPCCESTR_Points = d.spccestR_Points;
                                    mapp2.SPCCESTR_Points = F.indexValue;
                                    mapp2.SPCCESTR_Value = F.spccestR_Value;
                                    mapp2.SPCCESTR_RecordBrokenFlag = d.spccestR_RecordBrokenFlag;
                                    mapp2.SPCCESTR_Remarks = d.spccestR_Remarks;
                                    mapp2.SPCCESTR_ActiveFlag = true;
                                    mapp2.SPCCMUOM_Id = d.spccmuoM_Id;
                                    mapp2.CreatedDate = DateTime.Now;
                                    mapp2.UpdatedDate = DateTime.Now;
                                    _context.Add(mapp2);
                                }
                                if (d.spccestR_RecordBrokenFlag == "true")
                                {
                                    var contactExistsP1 = _db.Database.ExecuteSqlCommand("SPC_StudentRecordBroken_Transaction @p0, @p1,@p2,@p3", obj.MI_Id, d.amsT_Id, d.spccesT_Id, obj.UserId);
                                    if (contactExistsP1 > 0)
                                    {
                                        obj.returnVal = "Saved";

                                    }
                                    else
                                    {
                                        obj.returnVal = "Notsaved";

                                    }
                                }

                            }
                        }
                        else
                        {



                            var updates = _context.EventsStudentRecordDMO.Where(R => R.SPCCESTR_Id == d.spccestR_Id).FirstOrDefault();
                            if (d.spccestR_Rank != "" && d.spccestR_Rank != "0")
                            {
                                updates.SPCCESTR_Rank = d.spccestR_Rank;
                            }
                            else
                            {
                                updates.SPCCESTR_Rank = "0";
                            }
                            // updates.SPCCESTR_Rank = d.spccestR_Rank;
                            if (d.spccmscC_MultiAttemptFlg == false)
                            {
                                updates.SPCCESTR_Points = 1;

                            }

                            updates.SPCCESTR_RecordBrokenFlag = d.spccestR_RecordBrokenFlag;
                            updates.SPCCESTR_Remarks = d.spccestR_Remarks;
                            updates.SPCCESTR_Value = d.spccestR_Value;
                            updates.SPCCMUOM_Id = d.spccmuoM_Id;
                            updates.SPCCESTR_EventRecordFlg = d.spccestR_EventRecordFlg;
                            updates.UpdatedDate = DateTime.Now;
                            _context.Update(updates);
                        }

                    }
                    //added By sanjeev 
                    int rowAffected = _context.SaveChanges();
                    if (rowAffected > 0)
                    {
                        obj.returnVal = "Saved";
                    }
                    else
                    {
                        obj.returnVal = "Notsaved";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                obj.returnVal = "admin";
            }
            return obj;
        }






        public async Task<EventsStudentRecordDTO> MasterDeleteEventsStudent(EventsStudentRecordDTO obj)
        {
            try
            {
                if (obj.SPCCEST_Id > 0)
                {
                    var SPCC_Eventnts = _context.SPCC_Events_Students_DMO.Where(R => R.SPCCEST_Id == obj.SPCCEST_Id).FirstOrDefault();
                    if (SPCC_Eventnts.SPCCEST_ActiveFlag == true)
                    {
                        SPCC_Eventnts.SPCCEST_ActiveFlag = false;
                    }
                    else
                    {
                        SPCC_Eventnts.SPCCEST_ActiveFlag = true;
                    }
                    _context.Update(SPCC_Eventnts);
                    var flag = _context.SaveChanges();
                    if (flag > 0)
                    {
                        obj.returnVal = "Update";
                    }
                    else
                    {
                        obj.returnVal = "Not Update";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }


    }
}




