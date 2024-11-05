using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class VisitorAppointmentImpl : Interfaces.VisitorAppointmentInterface
    {
        public VisitorsManagementContext _VisitorContext;
        public DomainModelMsSqlServerContext _db;
        public VisitorAppointmentImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext db)
        {
            _VisitorContext = context;
            _db = db;
        }
        public Visitor_Management_Appointment_DTO getDetails(Visitor_Management_Appointment_DTO data)
        {
            try
            {
                data.emplist = (from t in _VisitorContext.MasterEmployee
                                where (t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id==data.MI_Id)
                                select new Visitor_Management_Appointment_DTO
                                {
                                    HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    HRME_Id = t.HRME_Id,
                                }).Distinct().OrderBy(T => T.HRME_EmployeeFirstName).Distinct().ToArray();

                using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_VISITOR_APPOINTMENT";
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
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getdata = retObject.ToArray();
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
            return data;
        }
        public Visitor_Management_Appointment_DTO EditDetails(Visitor_Management_Appointment_DTO data)
        {
       
            try
            {
                var editdetails = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                   from b in _VisitorContext.MasterEmployee
                                   where a.VMAP_HRME_Id == b.HRME_Id && a.VMAP_Id == data.VMAP_Id
                                   select new Visitor_Management_Appointment_DTO
                                   {
                                       HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                       HRME_Id = b.HRME_Id,
                                       VMAP_Id =a.VMAP_Id,
        MI_Id = a.MI_Id,
                                       VMAP_VisitorName = a.VMAP_VisitorName,
                                       VMAP_VisitorContactNo = a.VMAP_VisitorContactNo,
                                       VMAP_VisitorEmailid = a.VMAP_VisitorEmailid,
                                       VMAP_FromPlace =a.VMAP_FromPlace,
         VMAP_FromAddress = a.VMAP_FromAddress,
                                       VMAP_MeetingDuration = a.VMAP_MeetingDuration,
                                       VMAP_MeetingLocation =a.VMAP_MeetingLocation,
         //VMAP_MeetingDateTime = a.VMAP_MeetingDateTime,
                                       VMAP_MeetingPurpose = a.VMAP_MeetingPurpose,
                                       VMAP_PersonsAccompanying = a.VMAP_PersonsAccompanying,
                                       VMAP_AuthorisationBy = a.VMAP_AuthorisationBy,
                                       VMAP_ToMeet = a.VMAP_ToMeet,
                                       VMAP_EntryDateTime = a.VMAP_EntryDateTime,
                                       VMAP_VisitTypeFlg = a.VMAP_VisitTypeFlg,
                                       VMAP_Remarks = a.VMAP_Remarks,
                                       VMAP_ActiveFlag = a.VMAP_ActiveFlag,
                                       VMAP_CreatedBy = a.VMAP_CreatedBy,
                                       VMAP_UpdatedBy = a.VMAP_UpdatedBy,
                                       //VMAP_ReminderSchedule = a.VMAP_ReminderSchedule,
                                       //VMAP_ReminderFlag = a.VMAP_ReminderFlag,
                                       //VMAP_RepeatFlag = a.VMAP_RepeatFlag,
                                       VMAP_HRME_Id = a.VMAP_HRME_Id,
                                       VMAP_Status = a.VMAP_Status,
                                       VMAP_MeetingTiming = a.VMAP_MeetingTiming,
                                       //VMAP_ReminderDate = a.VMAP_ReminderDate,
                                       //VMAP_ChekInOutStatus = a.VMAP_ChekInOutStatus,
                                       //VMAP_ExitDateTime = a.VMAP_ExitDateTime,
                                       VMAP_MeetingFromTime = a.VMAP_MeetingFromTime,
                                       VMAP_MeetingToTime = a.VMAP_MeetingToTime,
                                       VMAP_RequestFromTime = a.VMAP_RequestFromTime,
                                       VMAP_RequestToTime = a.VMAP_RequestToTime,

                                   }).Distinct().ToList();


                data.editDetails = editdetails.ToArray();
                //data.editDetails = _VisitorContext.Visitor_Management_Appointment_DMO.Where(a => a.MI_Id == data.MI_Id && a.VMAP_Id == data.VMAP_Id).ToArray();


                data.editfiles = (from a in _VisitorContext.Visitor_Management_Appointment_FilesDMO

                                  where (a.VMAP_Id == data.VMAP_Id)
                                  select new vmsappdocDTO
                                  {
                                      cfilename = a.VMAPVF_FileName,
                                      cfilepath = a.VMAPVF_FilePath,
                                      cfiledesc = a.VMAPVF_Filedesc,

                                  }).Distinct().ToArray();

                data.extravisitor = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(e => e.VMAP_Id == data.VMAP_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Visitor_Management_Appointment_DTO deactivate(Visitor_Management_Appointment_DTO obj)
        {
            try
            {
                var result = _VisitorContext.Visitor_Management_Appointment_DMO.Single(t => t.VMAP_Id == obj.VMAP_Id && t.MI_Id == obj.MI_Id);

                if (result.VMAP_ActiveFlag == true)
                {
                    result.VMAP_ActiveFlag = false;
                }
                else if (result.VMAP_ActiveFlag == false)
                {
                    result.VMAP_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _VisitorContext.Update(result);

                int rowAffected = _VisitorContext.SaveChanges();
                if (rowAffected > 0)
                {
                    obj.returnval = true;
                }
                else
                {
                    obj.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public Visitor_Management_Appointment_DTO saveData(Visitor_Management_Appointment_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.VMAP_Id > 0)
                {
                    //var Duplicate = _VisitorContext.Visitor_Management_Appointment_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMAP_Id != data.VMAP_Id && t.VMAP_VisitorName == data.VMAP_VisitorName && t.VMAP_VisitorContactNo == data.VMAP_VisitorContactNo && t.VMAP_VisitorEmailid == data.VMAP_VisitorEmailid).ToList();
                    //if (Duplicate.Count > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        var update = _VisitorContext.Visitor_Management_Appointment_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMAP_Id == data.VMAP_Id).Single();

                        update.VMAP_VisitorName = data.VMAP_VisitorName;
                        update.VMAP_VisitorContactNo = data.VMAP_VisitorContactNo;
                        update.VMAP_VisitorEmailid = data.VMAP_VisitorEmailid;
                        update.VMAP_FromPlace = data.VMAP_FromPlace;
                        update.VMAP_FromAddress = data.VMAP_FromAddress;
                        update.VMAP_MeetingDuration = data.VMAP_MeetingDuration;
                        update.VMAP_MeetingLocation = data.VMAP_MeetingLocation;
                        update.VMAP_MeetingPurpose = data.VMAP_MeetingPurpose;
                        update.VMAP_EntryDateTime = data.VMAP_EntryDateTime;
                        update.VMAP_ToMeet = data.VMAP_ToMeet;
                        update.VMAP_PersonsAccompanying = data.VMAP_PersonsAccompanying;
                        update.VMAP_AuthorisationBy = data.VMAP_AuthorisationBy;
                        update.VMAP_VisitTypeFlg = data.VMAP_VisitTypeFlg;
                        update.VMAP_Remarks = data.VMAP_Remarks; ;
                        update.UpdatedDate = indianTime;
                        update.VMAP_UpdatedBy = data.UserId;
                        update.VMAP_MeetingFromTime = data.VMAP_RequestFromTime;
                        update.VMAP_MeetingToTime = data.VMAP_RequestToTime;
                        update.VMAP_MeetingTiming = data.VMAP_RequestFromTime;
                        update.VMAP_RequestToTime = data.VMAP_RequestToTime;
                        update.VMAP_RequestFromTime = data.VMAP_RequestFromTime;
                        update.VMAP_HRME_Id = data.VMAP_HRME_Id;


                        _VisitorContext.Update(update);

                    var filelist = _VisitorContext.Visitor_Management_Appointment_FilesDMO.Where(t => t.VMAP_Id == data.VMAP_Id).ToList();
                    if (filelist.Count > 0)
                    {
                        foreach (var item in filelist)
                        {
                            _VisitorContext.Remove(item);
                        }
                        
                    }




                    if (data.filelist.Length > 0)
                    {
                        foreach (var item in data.filelist)
                        {

                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                Visitor_Management_Appointment_FilesDMO obb = new Visitor_Management_Appointment_FilesDMO();
                              
                                obb.VMAP_Id = data.VMAP_Id;
                                obb.VMAPVF_FileName = item.cfilename;
                                obb.VMAPVF_FilePath = item.cfilepath;
                                obb.VMAPVF_Filedesc = item.cfiledesc;
                               // obb.TRMVDO_DocumentName = item.name;
                                obb.VMAPVF_ActiveFlg = true;
                                obb.VMAPVF_CreatedBy = data.UserId;
                                obb.VMAPVF_UpdatedBy = data.UserId;
                                obb.VMAPVF_CreatedDate = indianTime;
                                obb.VMAPVF_Updateddate = indianTime;
                                _VisitorContext.Add(obb);
                            }


                        }
                    }

                    if (data.visitordto != null && data.visitordto.Length>0)
                    {

                        List<long> idss = new List<long>();
                        foreach (var dd in data.visitordto)
                        {
                            idss.Add(dd.VMAPVI_Id);
                        }

                        var notexlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(e => e.VMAP_Id == data.VMAP_Id && !idss.Contains(e.VMAPVI_Id)).ToList(); ;
                        if (notexlist.Count>0)
                        {
                            foreach (var itemw in notexlist)
                            {
                                _VisitorContext.Remove(itemw);
                            }
                        }

                        foreach (var item in data.visitordto)
                        {
                            if (item.VMAPVI_Id>0)
                            {
                                var exsistvis = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Single(e => e.VMAPVI_Id == item.VMAPVI_Id);
                             exsistvis.VMAPVI_VisitorName = item.VMAPVI_VisitorName;
                                exsistvis.VMAPVI_VisitorEmailId = item.VMAPVI_VisitorEmailId;
                                exsistvis.VMAPVI_VisitorContactNo = item.VMAPVI_VisitorContactNo;
                                exsistvis.VMAPVI_VisitorAddress = item.VMAPVI_VisitorAddress;
                                exsistvis.VMAPVI_Remarks = item.VMAPVI_Remarks;
                                exsistvis.VMAPVI_UpdatedBy = data.UserId;
                                exsistvis.VMAPVI_Updateddate = indianTime;
                                _VisitorContext.Update(exsistvis);

                            }
                            else
                            {
                                if (item.VMAPVI_VisitorName != null && item.VMAPVI_VisitorName != "")
                                {
                                    Visitor_Management_Appointment_VisitorsDMO obb = new Visitor_Management_Appointment_VisitorsDMO();

                                    obb.VMAP_Id = data.VMAP_Id;
                                    obb.VMAPVI_VisitorName = item.VMAPVI_VisitorName;
                                    obb.VMAPVI_VisitorEmailId = item.VMAPVI_VisitorEmailId;
                                    obb.VMAPVI_VisitorContactNo = item.VMAPVI_VisitorContactNo;
                                    obb.VMAPVI_VisitorAddress = item.VMAPVI_VisitorAddress;
                                    obb.VMAPVI_Remarks = item.VMAPVI_Remarks;
                                    obb.VMAPVI_CreatedBy = data.UserId;
                                    obb.VMAPVI_UpdatedBy = data.UserId;
                                    obb.VMAPVI_CreatedDate = indianTime;
                                    obb.VMAPVI_Updateddate = indianTime;
                                    _VisitorContext.Add(obb);
                                }
                            }
                        }
                    }
                    else
                    {
                        var vislist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(t => t.VMAP_Id == data.VMAP_Id).ToList();
                        if (vislist.Count > 0)
                        {
                            foreach (var item in vislist)
                            {
                                _VisitorContext.Remove(item);
                            }

                        }
                    }


                    //var resultclass = _VisitorContext.Visitor_Management_Visitor_Appointment_DMO.Where(t => t.VMAP_Id == data.VMAP_Id).ToList();
                    //if (resultclass.Count > 0)
                    //{
                    //    foreach (var item in resultclass)
                    //    {
                    //        _VisitorContext.Remove(item);
                    //    }

                    //    Visitor_Management_Visitor_Appointment_DMO obj2 = new Visitor_Management_Visitor_Appointment_DMO();

                    //    obj2.VMVAP_Id = data.VMVAP_Id;
                    //    obj2.MI_Id = data.MI_Id;
                    //    obj2.VMAP_Id = update.VMAP_Id;

                    //    _VisitorContext.Add(obj2);
                    //}

         //           _VisitorContext.Update(update);
//
                        int rowAffected = _VisitorContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                        data.returnval21 = "Update";
                        if (data.VMAP_HRME_Id>0)
                        {
                            //EMAIL
                            List<string> toemail = new List<string>();

                            List<string> tomobile = new List<string>();

                            string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == data.VMAP_Id).FirstOrDefault().VMAP_VisitorEmailid;
                            if (emailid != "")
                            {
                                toemail.Add(emailid);

                            }

                            var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == data.VMAP_Id).ToList();
                            if (allvisitorlist.Count > 0)
                            {
                                foreach (var item in allvisitorlist)
                                {

                                    if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                    {

                                        toemail.Add(item.VMAPVI_VisitorEmailId);

                                    }
                                    if (item.VMAPVI_VisitorContactNo != null && item.VMAPVI_VisitorContactNo != "" && item.VMAPVI_VisitorContactNo != "0")
                                    {

                                        tomobile.Add(item.VMAPVI_VisitorContactNo);

                                    }



                                }
                            }





                            var staffemaillist = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == data.VMAP_HRME_Id && d.HRMEM_DeFaultFlag == "default").ToList();
                            if (staffemaillist.Count>0)
                            {
                                string staffemail = staffemaillist[0].HRMEM_EmailId;



                                if ( staffemail!=null && staffemail != "" )
                                {

                                    sendmailVisitor(data.MI_Id, data.VMAP_Id, "STAFFAPPOINTMENTREQUEST", staffemail, toemail);
                                }


                            }
                            
                            //


                            //sms
                            var moblist = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == data.VMAP_HRME_Id && d.HRMEMNO_DeFaultFlag == "default").Distinct().ToList();
                            if (moblist.Count>0)
                            {
                                long staffmobileno = moblist[0].HRMEMNO_MobileNo;
                                if (staffmobileno>0)
                                {
                                    
                                    string x = sendSmsStaff(data.MI_Id, data.VMAP_Id, staffmobileno, "STAFFAPPOINTMENTREQUEST").Result;
                                }

                            }
                          //END


                        }


                           
                        }
                        else
                        {
                            data.returnval21 = "Not Update";
                        }
                    //}
                }
                else
                {
                    //var Duplicate = _VisitorContext.Visitor_Management_Appointment_DMO.Where(t => t.MI_Id == data.MI_Id && t.VMAP_VisitorName == data.VMAP_VisitorName && t.VMAP_VisitorContactNo == data.VMAP_VisitorContactNo && t.VMAP_VisitorEmailid == data.VMAP_VisitorEmailid).ToList();
                    //if (Duplicate.Count > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        Visitor_Management_Appointment_DMO obj = new Visitor_Management_Appointment_DMO();
                       // obj.VMAP_Id = data.VMAP_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.VMAP_VisitorName = data.VMAP_VisitorName;
                        obj.VMAP_VisitorContactNo = data.VMAP_VisitorContactNo;
                        obj.VMAP_VisitorEmailid = data.VMAP_VisitorEmailid;
                        obj.VMAP_FromPlace = data.VMAP_FromPlace;
                        obj.VMAP_FromAddress = data.VMAP_FromAddress;
                        obj.VMAP_EntryDateTime = data.VMAP_EntryDateTime;
                        obj.VMAP_ToMeet = data.VMAP_ToMeet;
                        obj.VMAP_MeetingDuration = data.VMAP_MeetingDuration;
                        obj.VMAP_MeetingLocation = data.VMAP_MeetingLocation;
                        obj.VMAP_MeetingPurpose = data.VMAP_MeetingPurpose;
                        obj.VMAP_PersonsAccompanying = data.VMAP_PersonsAccompanying;
                        obj.VMAP_AuthorisationBy = data.VMAP_AuthorisationBy;
                        obj.VMAP_VisitTypeFlg = data.VMAP_VisitTypeFlg;
                        obj.VMAP_Remarks = data.VMAP_Remarks;
                        obj.VMAP_Status = "Waiting";
                        obj.VMAP_ActiveFlag = true;
                        obj.CreatedDate = indianTime;
                        obj.UpdatedDate = indianTime;
                        obj.VMAP_UpdatedBy = data.UserId;
                        obj.VMAP_CreatedBy = data.UserId;
                    obj.VMAP_MeetingFromTime = data.VMAP_RequestFromTime;
                    obj.VMAP_MeetingToTime = data.VMAP_RequestToTime;
                    obj.VMAP_MeetingTiming = data.VMAP_RequestFromTime;
                    obj.VMAP_RequestToTime = data.VMAP_RequestToTime;
                    obj.VMAP_RequestFromTime = data.VMAP_RequestFromTime;
                    obj.VMAP_HRME_Id = data.VMAP_HRME_Id;
                    _VisitorContext.Add(obj);


                    if (data.filelist.Length > 0)
                    {
                        foreach (var item in data.filelist)
                        {

                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                Visitor_Management_Appointment_FilesDMO obb = new Visitor_Management_Appointment_FilesDMO();

                                obb.VMAP_Id = obj.VMAP_Id;
                                obb.VMAPVF_FileName = item.cfilename;
                                obb.VMAPVF_FilePath = item.cfilepath;
                                obb.VMAPVF_Filedesc = item.cfiledesc;
                                // obb.TRMVDO_DocumentName = item.name;
                                obb.VMAPVF_ActiveFlg = true;
                                obb.VMAPVF_CreatedBy = data.UserId;
                                obb.VMAPVF_UpdatedBy = data.UserId;
                                obb.VMAPVF_CreatedDate = indianTime;
                                obb.VMAPVF_Updateddate = indianTime;
                                _VisitorContext.Add(obb);
                            }


                        }
                    }


                    if (data.visitordto != null && data.visitordto.Length > 0)
                    {
                        foreach (var item in data.visitordto)
                        {
                            if (item.VMAPVI_Id > 0)
                            {
                                var exsistvis = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Single(e => e.VMAPVI_Id == item.VMAPVI_Id);
                                exsistvis.VMAPVI_VisitorName = item.VMAPVI_VisitorName;
                                exsistvis.VMAPVI_VisitorEmailId = item.VMAPVI_VisitorEmailId;
                                exsistvis.VMAPVI_VisitorContactNo = item.VMAPVI_VisitorContactNo;
                                exsistvis.VMAPVI_VisitorAddress = item.VMAPVI_VisitorAddress;
                                exsistvis.VMAPVI_Remarks = item.VMAPVI_Remarks;
                                exsistvis.VMAPVI_UpdatedBy = data.UserId;
                                exsistvis.VMAPVI_Updateddate = indianTime;
                                _VisitorContext.Update(exsistvis);

                            }
                            else
                            {
                                if (item.VMAPVI_VisitorName != null && item.VMAPVI_VisitorName != "")
                                {
                                    Visitor_Management_Appointment_VisitorsDMO obb = new Visitor_Management_Appointment_VisitorsDMO();

                                    obb.VMAP_Id = obj.VMAP_Id;
                                    obb.VMAPVI_VisitorName = item.VMAPVI_VisitorName;
                                    obb.VMAPVI_VisitorEmailId = item.VMAPVI_VisitorEmailId;
                                    obb.VMAPVI_VisitorContactNo = item.VMAPVI_VisitorContactNo;
                                    obb.VMAPVI_VisitorAddress = item.VMAPVI_VisitorAddress;
                                    obb.VMAPVI_Remarks = item.VMAPVI_Remarks;
                                    obb.VMAPVI_CreatedBy = data.UserId;
                                    obb.VMAPVI_UpdatedBy = data.UserId;
                                    obb.VMAPVI_CreatedDate = indianTime;
                                    obb.VMAPVI_Updateddate = indianTime;
                                    _VisitorContext.Add(obb);
                                }
                            }
                        }
                    }
                    else
                    {
                        var vislist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(t => t.VMAP_Id == data.VMAP_Id).ToList();
                        if (vislist.Count > 0)
                        {
                            foreach (var item in vislist)
                            {
                                _VisitorContext.Remove(item);
                            }

                        }
                    }

                    //Visitor_Management_Visitor_Appointment_DMO obj2 = new Visitor_Management_Visitor_Appointment_DMO();

                    //obj2.VMVAP_Id = data.VMVAP_Id;
                    //obj2.MI_Id = data.MI_Id;
                    //obj2.VMAP_Id = obj.VMAP_Id;
                    //_VisitorContext.Add(obj2);

                    int rowAffected = _VisitorContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                        var visitorlist = _VisitorContext.Visitor_Management_Appointment_DMO.Where(e => e.VMAP_Id == obj.VMAP_Id).ToList();
                        long primaryid = visitorlist.FirstOrDefault().VMAP_Id;
                            data.returnval21 = "Save";
                        if (data.VMAP_HRME_Id > 0)
                        {
                            //EMAIL
                            List<string> toemail = new List<string>();

                            List<string> tomobile = new List<string>();

                            string emailid = _VisitorContext.Visitor_Management_Appointment_DMO.Where(d => d.VMAP_Id == primaryid).FirstOrDefault().VMAP_VisitorEmailid;
                            if (emailid != "")
                            {
                                toemail.Add(emailid);

                            }

                            var allvisitorlist = _VisitorContext.Visitor_Management_Appointment_VisitorsDMO.Where(d => d.VMAP_Id == primaryid).ToList();
                            if (allvisitorlist.Count > 0)
                            {
                                foreach (var item in allvisitorlist)
                                {

                                    if (item.VMAPVI_VisitorEmailId != null && item.VMAPVI_VisitorEmailId != "" && item.VMAPVI_VisitorEmailId != "0")
                                    {

                                        toemail.Add(item.VMAPVI_VisitorEmailId);

                                    }
                                    if (item.VMAPVI_VisitorContactNo != null && item.VMAPVI_VisitorContactNo != "" && item.VMAPVI_VisitorContactNo != "0")
                                    {

                                        tomobile.Add(item.VMAPVI_VisitorContactNo);

                                    }



                                }
                            }





                            var staffemaillist = _VisitorContext.Emp_Email_Id.Where(d => d.HRME_Id == data.VMAP_HRME_Id && d.HRMEM_DeFaultFlag == "default").ToList();
                            if (staffemaillist.Count > 0)
                            {
                                string staffemail = staffemaillist[0].HRMEM_EmailId;



                                if (staffemail != null && staffemail != "")
                                {

                                    sendmailVisitor(data.MI_Id, primaryid, "STAFFAPPOINTMENTREQUEST", staffemail, toemail);
                                }


                            }



                            //


                            //sms
                            var moblist = _VisitorContext.Emp_MobileNo.Where(d => d.HRME_Id == data.VMAP_HRME_Id && d.HRMEMNO_DeFaultFlag == "default").Distinct().ToList();
                            if (moblist.Count > 0)
                            {
                                long staffmobileno = moblist[0].HRMEMNO_MobileNo;
                                if (staffmobileno > 0)
                                {

                                    string x = sendSmsStaff(data.MI_Id, primaryid, staffmobileno, "STAFFAPPOINTMENTREQUEST").Result;
                                }

                            }
                            //END


                        }

                        notify(primaryid,data.UserId);

                    }
                        else
                        {
                            data.returnval21 = "Not Save";
                        }
                    //}
                }

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Visitor_Management_Appointment_DTO visitormailtrigger(Visitor_Management_Appointment_DTO data)
        {
            try
            {
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


                var template = "VisitorNotification";
                var miname = _VisitorContext.Institute.Where(a => a.MI_ActiveFlag == 1).ToList();
                foreach (var mi in miname)
                {
                    var vmn = _VisitorContext.Visitor_Management_Appointment_DMO.Where(a => a.MI_Id == mi.MI_Id && a.VMAP_Status == "Waiting").ToList();
                    if (vmn.Count > 0)
                    {
                        foreach (var vm in vmn)
                        {
                            var mailsetting = _VisitorContext.SMS_Sent_Details.Where(a => a.SSD_TransactionId == vm.VMAP_Id && a.SSD_HeaderName == "VisitorNotification").ToList();
                            if (mailsetting.Count > 0)
                            {
                                var mailsetting1 = _VisitorContext.SMS_Sent_Details.Where(a => a.SSD_TransactionId == vm.VMAP_Id && a.SSD_HeaderName == "VisitorNotification").ToList();



                                if (mailsetting1.Count == 1)
                                {
                                    var rr = mailsetting1.LastOrDefault().SSD_Senttime;

                                    var newTime = rr.Add(new TimeSpan(1, 1, 0));
                                    var newTime1 = new TimeSpan(0, 60, 0);
                                    var newTime2 = new TimeSpan(2, 00, 0);
                                    if (newTime >= newTime1 && newTime < newTime2)
                                    {
                                        var emd = (from a in _VisitorContext.SMSMasterApprovalDMO
                                                   from b in _VisitorContext.MasterEmployee
                                                   where a.SMA_HeaderName == "VisitorNotification" && a.SMA_Level == 2 && a.IVRMUL_Id == b.HRME_Id
                                                   select new Visitor_Management_Appointment_DTO
                                                   {
                                                       IVRMUL_Id = a.IVRMUL_Id,
                                                       HRME_EmailId = b.HRME_EmailId
                                                   }).ToList();
                                        var msg = sendmailapprovalnotification(mi.MI_Id, template, vm.VMAP_Id, emd[0].IVRMUL_Id, emd[0].HRME_EmailId);
                                        if (msg == "Success")
                                        {


                                            SMS_Sent_Details sd = new SMS_Sent_Details();
                                            sd.MI_ID = mi.MI_Id;
                                            sd.SSD_HeaderName = "VisitorNotification";
                                            sd.SSD_SentDate = DateTime.Today;
                                            sd.SSD_Senttime = new TimeSpan(2, 14, 18);
                                            sd.SSD_TransactionId = vm.VMAP_Id;
                                            sd.SSD_ToFlag = "Y";
                                            sd.SSD_SystemIP = myIP1;
                                           // sd.SSD_NetworkIP = "0";
                                            sd.SSD_MAACAddress = sMacAddress;
                                            sd.SSD_SchedulerFlag = true;
                                            _VisitorContext.Add(sd);
                                            _VisitorContext.SaveChanges();

                                        }
                                    }
                                }
                                else
                                {
                                    var rr = mailsetting1.LastOrDefault().SSD_Senttime;

                                    var newTime = rr.Add(new TimeSpan(1, 1, 0));
                                    var newTime1 = new TimeSpan(2, 00, 0);
                                    var newTime2 = new TimeSpan(3, 00, 0);
                                    if (newTime >= newTime1 && newTime < newTime2)
                                    {
                                        var emd = (from a in _VisitorContext.SMSMasterApprovalDMO
                                                   from b in _VisitorContext.MasterEmployee
                                                   where a.SMA_HeaderName == "VisitorNotification" && a.SMA_Level == 3 && a.IVRMUL_Id == b.HRME_Id
                                                   select new Visitor_Management_Appointment_DTO
                                                   {
                                                       IVRMUL_Id = a.IVRMUL_Id,
                                                       HRME_EmailId = b.HRME_EmailId
                                                   }).ToList();
                                        var msg = sendmailapprovalnotification(mi.MI_Id, template, vm.VMAP_Id, emd[0].IVRMUL_Id, emd[0].HRME_EmailId);
                                        if (msg == "Success")
                                        {


                                            SMS_Sent_Details sd = new SMS_Sent_Details();
                                            sd.MI_ID = mi.MI_Id;
                                            sd.SSD_HeaderName = "VisitorNotification";
                                            sd.SSD_SentDate = DateTime.Today;
                                            sd.SSD_Senttime = new TimeSpan(2, 14, 18);
                                            sd.SSD_TransactionId = vm.VMAP_Id;
                                            sd.SSD_ToFlag = "Y";
                                            sd.SSD_SystemIP = myIP1;
                                           // sd.SSD_NetworkIP = "0";
                                            sd.SSD_MAACAddress = sMacAddress;
                                            sd.SSD_SchedulerFlag = true;
                                            _VisitorContext.Add(sd);
                                            _VisitorContext.SaveChanges();

                                        }
                                    }
                                }
                            }

                            else
                            {
                                var emd = (from a in _VisitorContext.SMSMasterApprovalDMO
                                           from b in _VisitorContext.MasterEmployee
                                           where a.SMA_HeaderName == "VisitorNotification" && a.SMA_Level == 1 && a.IVRMUL_Id == b.HRME_Id
                                           select new Visitor_Management_Appointment_DTO
                                           {
                                               IVRMUL_Id = a.IVRMUL_Id,
                                               HRME_EmailId = b.HRME_EmailId
                                           }).ToList();
                                var msg = sendmailapprovalnotification(mi.MI_Id, template, vm.VMAP_Id, emd[0].IVRMUL_Id, emd[0].HRME_EmailId);
                                if (msg == "Success")
                                {

                                    SMS_Sent_Details sd = new SMS_Sent_Details();
                                    sd.MI_ID = mi.MI_Id;
                                    sd.SSD_HeaderName = "VisitorNotification";
                                    sd.SSD_SentDate = DateTime.Today;
                                    sd.SSD_Senttime = new TimeSpan(2, 14, 18);
                                    sd.SSD_TransactionId = vm.VMAP_Id;
                                    sd.SSD_ToFlag = "Y";
                                    sd.SSD_SystemIP = myIP1;
                                   // sd.SSD_NetworkIP = "0";
                                    sd.SSD_MAACAddress = sMacAddress;
                                    sd.SSD_SchedulerFlag = true;
                                    _VisitorContext.Add(sd);
                                    _VisitorContext.SaveChanges();

                                }

                            }
                        }
                    }



                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Visitor_Management_Appointment_DTO visitorappoinmentstatus(Visitor_Management_Appointment_DTO data)
        {
            try
            {

                using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "visitorapprovalstatus";
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
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
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getdata = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Visitor_Management_Appointment_DTO viewuploadflies(Visitor_Management_Appointment_DTO data)
        {
            try
            {
                data.editfiles = (from a in _VisitorContext.Visitor_Management_Appointment_FilesDMO

                                  where (a.VMAP_Id == data.VMAP_Id)
                                  select new vmsappdocDTO
                                  {
                                      gridid = a.VMAP_Id,
                                      cfileid = a.VMAPVF_Id,
                                      cfilename = a.VMAPVF_FileName,
                                      cfilepath = a.VMAPVF_FilePath,
                                      cfiledesc = a.VMAPVF_Filedesc
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public Visitor_Management_Appointment_DTO deleteuploadfile(Visitor_Management_Appointment_DTO data)
        {
            try
            {
                if (data.VMAPVF_Id > 0)
                {
                    var deletefile = _VisitorContext.Visitor_Management_Appointment_FilesDMO.Where(e => e.VMAPVF_Id == data.VMAPVF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            _VisitorContext.Remove(item);
                        }


                        int y = _VisitorContext.SaveChanges();
                        if (y > 0)
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
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }



        public Visitor_Management_Appointment_DTO todayappointments(Visitor_Management_Appointment_DTO data)
            
        {
            try
            {
                var employeelist = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                    from b in _VisitorContext.MasterEmployee
                                    from c in _VisitorContext.Emp_Email_Id
                                    where a.VMAP_HRME_Id == b.HRME_Id && b.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default" && ((Convert.ToDateTime(a.VMAP_EntryDateTime).Date==DateTime.Now.Date) || (Convert.ToDateTime(a.VMAP_MeetingDateTime).Date == DateTime.Now.Date))
                                    select c
                                    //select new Visitor_Management_Appointment_DTO
                                    //{
                                    //    HRMEEM_Id = c.HRMEEM_Id,
                                    //    HRME_Id = c.HRME_Id,
                                    //    HRME_EmailId = c.HRMEM_EmailId
                                    //}
                                  ).Distinct().ToList();

                if (employeelist.Count > 0)
                {
                    foreach (var item in employeelist)
                    {
                        string stra = "";
                        string strw = "";
                        string strc = "";
                        int rownumber = 0;
                        int rownumber1 = 0;
                        int rownumber2 = 0;

                        if (item.HRMEM_EmailId != null && item.HRMEM_EmailId != "" && item.HRMEM_EmailId != "test@gmail.com")
                        {

                      
                            List<Visitor_Management_Appointment_DTO> approvelist = new List<Visitor_Management_Appointment_DTO>();
                            List<Visitor_Management_Appointment_DTO> waitinglist = new List<Visitor_Management_Appointment_DTO>();
                            List<Visitor_Management_Appointment_DTO> cancellist = new List<Visitor_Management_Appointment_DTO>();

                            using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "TODAYS_APPOINTMENT_LIST_APPROVED";
                                cmd.CommandType = CommandType.StoredProcedure;


                                cmd.Parameters.Add(new SqlParameter("@fromdate",
                              SqlDbType.Date)
                                {
                                    Value = DateTime.Now.Date
                                });
                                cmd.Parameters.Add(new SqlParameter("@UserId",
                              SqlDbType.VarChar)
                                {
                                    Value = data.UserId
                                });
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                             SqlDbType.VarChar)
                                {
                                    Value = item.HRME_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@TYPE",
                            SqlDbType.VarChar)
                                {
                                    Value = "A"
                                });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            approvelist.Add(new Visitor_Management_Appointment_DTO
                                            {
                                                VMAP_VisitorName = (dataReader["VMAP_VisitorName"].ToString()),
                                                VMAP_MeetingDateTime = Convert.ToString(dataReader["VMAP_MeetingDateTime1"].ToString()),
                                                VMAP_MeetingToTime = Convert.ToString(dataReader["VMAP_MeetingToTime"].ToString()),
                                                VMAP_MeetingFromTime = Convert.ToString(dataReader["VMAP_MeetingTiming"].ToString()),
                                                VMAP_MeetingPurpose = dataReader["VMAP_MeetingPurpose"].ToString(),
                                                VMAP_VisitorEmailid = dataReader["VMAP_VisitorEmailid"].ToString(),
                                                VMAP_MeetingLocation = dataReader["VMAP_MeetingLocation"].ToString(),
                                                requestedby = dataReader["createdby"].ToString(),
                                                VMAP_VisitorContactNo = Convert.ToInt64(dataReader["VMAP_VisitorContactNo"].ToString()),


                                            });

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }


                            using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "TODAYS_APPOINTMENT_LIST_APPROVED";
                                cmd.CommandType = CommandType.StoredProcedure;


                                cmd.Parameters.Add(new SqlParameter("@fromdate",
                              SqlDbType.Date)
                                {
                                    Value = DateTime.Now.Date
                                });
                                cmd.Parameters.Add(new SqlParameter("@UserId",
                              SqlDbType.VarChar)
                                {
                                    Value = data.UserId
                                });
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                             SqlDbType.VarChar)
                                {
                                    Value = item.HRME_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@TYPE",
                             SqlDbType.VarChar)
                                {
                                    Value = "W"
                                });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            waitinglist.Add(new Visitor_Management_Appointment_DTO
                                            {
                                                VMAP_VisitorName = (dataReader["VMAP_VisitorName"].ToString()),
                                                VMAP_MeetingDateTime = Convert.ToString(dataReader["VMAP_EntryDateTime1"].ToString()),
                                                VMAP_MeetingToTime = Convert.ToString(dataReader["VMAP_RequestToTime"].ToString()),
                                                VMAP_MeetingFromTime = Convert.ToString(dataReader["VMAP_RequestFromTime"].ToString()),
                                                VMAP_MeetingPurpose = dataReader["VMAP_MeetingPurpose"].ToString(),
                                                VMAP_VisitorEmailid = dataReader["VMAP_VisitorEmailid"].ToString(),
                                                VMAP_MeetingLocation = dataReader["VMAP_MeetingLocation"].ToString(),
                                                requestedby = dataReader["createdby"].ToString(),
                                                VMAP_VisitorContactNo = Convert.ToInt64(dataReader["VMAP_VisitorContactNo"].ToString()),


                                            });

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }

                            using (var cmd = _VisitorContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "TODAYS_APPOINTMENT_LIST_APPROVED";
                                cmd.CommandType = CommandType.StoredProcedure;


                                cmd.Parameters.Add(new SqlParameter("@fromdate",
                              SqlDbType.Date)
                                {
                                    Value = DateTime.Now.Date
                                });
                                cmd.Parameters.Add(new SqlParameter("@UserId",
                              SqlDbType.VarChar)
                                {
                                    Value = data.UserId
                                });
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                             SqlDbType.VarChar)
                                {
                                    Value = item.HRME_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@TYPE",
                             SqlDbType.VarChar)
                                {
                                    Value = "C"
                                });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            cancellist.Add(new Visitor_Management_Appointment_DTO
                                            {
                                                VMAP_VisitorName = (dataReader["VMAP_VisitorName"].ToString()),
                                                VMAP_MeetingDateTime = Convert.ToString(dataReader["VMAP_EntryDateTime1"].ToString()),
                                                VMAP_MeetingToTime = Convert.ToString(dataReader["VMAP_RequestToTime"].ToString()),
                                                VMAP_MeetingFromTime = Convert.ToString(dataReader["VMAP_RequestFromTime"].ToString()),
                                                VMAP_MeetingPurpose = dataReader["VMAP_MeetingPurpose"].ToString(),
                                                VMAP_VisitorEmailid = dataReader["VMAP_VisitorEmailid"].ToString(),
                                                VMAP_MeetingLocation = dataReader["VMAP_MeetingLocation"].ToString(),
                                                requestedby = dataReader["createdby"].ToString(),
                                                VMAP_VisitorContactNo = Convert.ToInt64(dataReader["VMAP_VisitorContactNo"].ToString()),


                                            });

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }

                            if (approvelist != null && approvelist.Count > 0)
                            {

                                stra = stra + "<p> <span style=" + "'font-weight:bold'" + ">Approved Appointment List :<span style=" + "'float: center;padding-left:10%;'" + "></span> </span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > VISITOR NAME</th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DATE/TIME</th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > PURPOSE </th>" +
                                            "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > REQUESTED BY </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > VENUE </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMAIL / MOBILE NO </th>" +
                                             "</tr>";
                                for (int j = 0; j < approvelist.Count; j++)
                                {
                                    rownumber += 1;
                                    stra = stra + "<tr> " +
                                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:13px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-weight:bold;font-family: sans-serif;'" + " >" + approvelist[j].VMAP_VisitorName + "</td>" +
                                            "<td align=" + "left " + "style=" + "'font-size:13px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + approvelist[j].VMAP_MeetingDateTime + "</br>" + approvelist[j].VMAP_MeetingFromTime + " TO " + approvelist[j].VMAP_MeetingToTime + "</td>" +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + approvelist[j].VMAP_MeetingPurpose + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + approvelist[j].requestedby + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + approvelist[j].VMAP_MeetingLocation + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + approvelist[j].VMAP_VisitorEmailid + "</br>" + approvelist[j].VMAP_VisitorContactNo + "</td> " +
                                             "</tr>";
                                }
                                stra = stra + "</table>";

                            }


                            if (waitinglist != null && waitinglist.Count > 0)
                            {

                                strw = strw + "<p> <span style=" + "'font-weight:bold'" + ">Pending Appointment List For Approval  :<span style=" + "'float: center;padding-left:10%;'" + "></span> </span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > VISITOR NAME</th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DATE / TIME </th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > PURPOSE </th>" +
                                            "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > REQUESTED BY </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > VENUE </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMAIL / MOBILE NO </th>" +
                                             "</tr>";
                                for (int j = 0; j < waitinglist.Count; j++)
                                {
                                    rownumber1 += 1;
                                    strw = strw + "<tr> " +
                                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:13px;font-weight:bold;'" + " > " + rownumber1 + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-weight:bold;font-family: sans-serif;'" + " >" + waitinglist[j].VMAP_VisitorName + "</td>" +
                                            "<td align=" + "left " + "style=" + "'font-size:13px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + waitinglist[j].VMAP_MeetingDateTime + "</br>" + waitinglist[j].VMAP_MeetingFromTime + " TO " + waitinglist[j].VMAP_MeetingToTime + "</td>" +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + waitinglist[j].VMAP_MeetingPurpose + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + waitinglist[j].requestedby + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + waitinglist[j].VMAP_MeetingLocation + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + waitinglist[j].VMAP_VisitorEmailid + "</br>" + waitinglist[j].VMAP_VisitorContactNo + "</td> " +
                                             "</tr>";
                                }
                                strw = strw + "</table>";

                            }
                            if (cancellist != null && cancellist.Count > 0)
                            {

                                strc = strc + "<p> <span style=" + "'font-weight:bold'" + ">Cancelled/Rejected Appointment List For Approval  :<span style=" + "'float: center;padding-left:10%;'" + "></span> </span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > VISITOR NAME</th>" +
                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DATE/TIME</th>" +

                                           "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > PURPOSE </th>" +
                                            "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > REQUESTED BY </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > VENUE </th>" +
                                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMAIL / MOBILE NO </th>" +
                                             "</tr>";
                                for (int j = 0; j < cancellist.Count; j++)
                                {
                                    rownumber2 += 1;
                                    strc = strc + "<tr> " +
                                            "<td style=" + "'border: 1px solid black; text-align:center;font-size:13px;font-weight:bold;'" + " > " + rownumber2 + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-weight:bold;font-family: sans-serif;'" + " >" + cancellist[j].VMAP_VisitorName + "</td>" +
                                            "<td align=" + "left " + "style=" + "'font-size:13px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + cancellist[j].VMAP_MeetingDateTime + "</br>" + cancellist[j].VMAP_MeetingFromTime + " TO " + cancellist[j].VMAP_MeetingToTime + "</td>" +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + cancellist[j].VMAP_MeetingPurpose + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + cancellist[j].requestedby + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + cancellist[j].VMAP_MeetingLocation + "</td> " +
                                            "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + cancellist[j].VMAP_VisitorEmailid + "</br>" + cancellist[j].VMAP_VisitorContactNo + "</td> " +
                                             "</tr>";
                                }
                                strc = strc + "</table>";

                            }



                            sendmailcommulative(data.MI_Id, data.UserId, "DAILYAPPOINTMENT", item.HRMEM_EmailId, stra, strw, strc, item.HRME_Id);


                        }
                    }


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return data;
        }



        //EMAIL

        #region   email
        public string sendmailcommulative(long MI_Id, long UserID, string Template, string emailid, string apr, string wait, string cancel, long HRME_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TODAY_APPOINTMENT_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                           SqlDbType.BigInt)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
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

                Mailmsg = Mailmsg.Replace("[APPROVAL]", apr);
                Mailmsg = Mailmsg.Replace("[PENDING]", wait);
                Mailmsg = Mailmsg.Replace("[CANCEL]", cancel);

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
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(emailid);




                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }




                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {

                                emailMessage.To.Add(new MailAddress(emailid));



                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
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
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Inventory"
                        //});

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
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion


        //END


        //SMS START
        public async Task<string> sendSmsStaff(long MI_Id, long UserID, long mobileno, string Template)
        {
            try
            {
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

                if (variables.Count == 0)
                {
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_Email_PARAMETER_REQUEST";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
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
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileno.ToString();


                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
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
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "Visitor Appointment";
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
                // return ex.Message;
            }
            return "success";
        }

        //SMSEND


        //EMAIL

        #region  visitor sms and email
        public string sendmailVisitor(long MI_Id, long UserID,  string Template,string emailid ,List<string> ToCCEmail)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_Email_PARAMETER_REQUEST";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
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
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(emailid);



                        var fileattachment = _VisitorContext.Visitor_Management_Appointment_FilesDMO.Where(i => i.VMAP_Id == UserID).ToList();
                        if (fileattachment.Count > 0)
                        {
                            for (int i = 0; i < fileattachment.Count; i++)
                            {
                                if (fileattachment[i].VMAPVF_FilePath != null && fileattachment[i].VMAPVF_FilePath != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(fileattachment[i].VMAPVF_FilePath);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(fileattachment[i].VMAPVF_FileName, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }




                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }

                        foreach (var itemcc in ToCCEmail)
                        {
                            message.AddCc(itemcc);
                        }
                       

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {

                                emailMessage.To.Add(new MailAddress(emailid));
                               


                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                foreach (var itemcc in ToCCEmail)
                                {
                                    emailMessage.CC.Add(itemcc);
                                }
                               

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
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
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Visitor Appointment"
                        //});

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
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion


        //END

        public string notify(long AppointmentId, long UserId)
        {
            try
            {
                string deviceidsnew = "";
                string devicenew = "";
                string sound = "";
                var devicelist = (from a in _VisitorContext.Visitor_Management_Appointment_DMO
                                  from e in _VisitorContext.MasterEmployee
                                  where (a.VMAP_HRME_Id == e.HRME_Id && e.HRME_AppDownloadedDeviceId != "" && e.HRME_AppDownloadedDeviceId != null && a.VMAP_Id == AppointmentId)
                                  select new AppointmentApprovalStatus_DTO
                                  {
                                      HRME_Id = e.HRME_Id,
                                      HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                  }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                if (devicelist.Count > 0)
                {
                    string titletext = "Appointment Request";
                    string Body = "";
                    int k = 0;
                    foreach (var deviceid in devicelist)
                    {
                        if (k == 0)
                        {
                            deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                            k = 1;
                        }
                        else
                        {
                            deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                        }
                    }
                    devicenew = "[" + deviceidsnew + "]";


                    var apointmentdetails = _VisitorContext.Visitor_Management_Appointment_DMO.Where(w => w.VMAP_Id == AppointmentId).ToList();
                    if (apointmentdetails.Count > 0)
                    {

                            var date1 = Convert.ToDateTime(apointmentdetails[0].VMAP_EntryDateTime).Date.ToString("dd/MM/yyyy");

                            DateTime dateTime = DateTime.ParseExact(apointmentdetails[0].VMAP_RequestFromTime, "HH:mm", CultureInfo.InvariantCulture);
                            string entrydatetime = dateTime.ToString("hh:mm tt");

                            Body = "Appointment Requested at " + " " + entrydatetime + " " + "on"+" " + date1;
                 
                     

                        if (sound == "")
                        {
                            sound = "default";
                        }
                        long hrid = Convert.ToInt64(apointmentdetails[0].VMAP_HRME_Id);
                        callnotification(devicenew, hrid, apointmentdetails[0].VMAP_Id, titletext, apointmentdetails[0].MI_Id, sound, Body);
                        //Insertnotification(apointmentdetails[0].MI_Id, hrid, apointmentdetails[0].VMAP_Id, Body, UserId, "APPOINTMENT");

                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return "";
        }

        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound, string body)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + body + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        //public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, long userid, string type)
        //{
        //    try
        //    {
        //        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

        //        //ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();
        //        ISM_NotificationsDMO push = new ISM_NotificationsDMO();
        //        push.MI_Id = MI_Id;
        //        push.HRME_Id = empid;
        //        push.ISMNO_Notification = titletext;
        //        push.ISMNO_NoticationDate = indianTime;
        //        push.ISMNO_NotificationType = type;
        //        push.ISMNO_ReadFlg = false;
        //        push.ISMNO_MakeUnReadFlg = false;
        //        push.ISMNO_ActiveFlag = true;
        //        push.ISMNO_CreatedBy = userid;
        //        push.ISMNO_UpdatedBy = userid;
        //        push.CreatedDate = indianTime;
        //        push.UpdatedDate = indianTime;
        //        _VisitorContext.Add(push);
        //        var contactExists = _VisitorContext.SaveChanges();
        //        if (contactExists > 0)
        //        {
        //            //  data.returnval = true;
        //        }
        //        else
        //        {
        //            // data.returnval = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ex.Message;
        //    }
        //    return "success";
        //}










        public async Task<string> sendSms(long MI_Id, string VMAP_VisitorName, DateTime? VMAP_EntryDateTime, string VMAP_MeetingDateTime, string empname, long VMAP_VisitorContactNo, string Template)
        {
            try
            {
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

                DateTime? date = VMAP_EntryDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                // var Date_Visit1 = (Convert.ToDateTime(Date_Visit).Date).ToString();

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMAP_VisitorName);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMAP_MeetingDateTime);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        sms = result;
                    }
                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = VMAP_VisitorContactNo.ToString();

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = modulename[0]
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
                            Value = messageid
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
                // return ex.Message;
            }
            return "success";
        }

        public async void sendmail(long MI_Id, string VMAP_VisitorName, DateTime? VMAP_EntryDateTime, string VMAP_MeetingDateTime, string empname, string VMAP_VisitorEmailid, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_SMSMessage;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                #region
                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Visitors_Email";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@AMVM_Name",
                //          SqlDbType.VarChar)
                //    {
                //        Value = VMAP_VisitorName
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@Date_Visit",
                //          SqlDbType.DateTime)
                //    {
                //        Value = VMAP_EntryDateTime
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@Time_Visit",
                //          SqlDbType.VarChar)
                //    {
                //        Value = VMAP_MeetingDateTime
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@AMVM_To_Meet",
                //         SqlDbType.VarChar)
                //    {
                //        Value = empname
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

                //for (int j = 0; j < ParamaetersName.Count; j++)
                //{
                //    for (int p = 0; p < val.Count; p++)
                //    {
                //        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                //        {
                //            result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                //            Mailmsg = result_value;
                //        }
                //    }
                //}
                //Mailmsg = result_value;
                #endregion

                DateTime? date = VMAP_EntryDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                // var Date_Visit1 = (Convert.ToDateTime(Date_Visit).Date).ToString();

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMAP_VisitorName);
                        Mailmsg = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        Mailmsg = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMAP_MeetingDateTime);
                        Mailmsg = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        Mailmsg = result;
                    }
                }
                Mailmsg = result;

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
                        message.AddTo(VMAP_VisitorEmailid);
                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);

                    }

                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();
                    //    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();
                    //    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();
                    //}

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = VMAP_VisitorEmailid
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
                        //cmd.Parameters.Add(new SqlParameter("@type",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = type
                        //});

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
                        }
                    }
                }
                //Mails Sending end
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string sendmailapprovalnotification(long MI_Id, string template1, long VMAP_Id, long HRME_Id, string Email)
        {
            try
            {
                string result_value = "";
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == template1 && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg1 = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                DateTime dt = DateTime.Today;
                DateTime dt1 = dt.Date;
                Mailmsg = Mailmsg1.Replace("[DATE]", dt1.ToString("dd/mm/yyyy"));

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                #region
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitors_EmailStatus_Trigger";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMAP_Id",
                          SqlDbType.BigInt)
                    {
                        Value = VMAP_Id
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
                        Console.WriteLine(ex.Message);
                    }

                }

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

                #endregion


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
                        message.AddTo(Email);
                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);

                    }

                }
                //Mails Sending end
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Success";
        }
    }
}