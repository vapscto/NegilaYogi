using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.NetworkInformation;
using DomainModel.Model.com.vapstech.Birthday;
using System.Globalization;
using CommonLibrary;

namespace VisitorsManagementServiceHub.Services
{
    public class AddVisitorsImpl : AddVisitorsInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public AddVisitorsImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext dbctxt)
        {
            visctxt = context;
            _db = dbctxt;
        }
        public AddVisitorsDTO getDetails(AddVisitorsDTO data)
        {
            try
            {
                //var gridoptionslist = visctxt.AddVisitorsDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                //if (gridoptionslist.Count > 0)
                //{
                //    data.gridoptions = gridoptionslist.ToArray();
                //    data.count = gridoptionslist.Count;
                //}
                //else
                //{
                //    data.count = 0;
                //}

                //var gridoptionslist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                //                       from e in visctxt.MasterEmployee
                //                       where (a.VMMV_ToMeet == e.HRME_Id && a.MI_Id == data.MI_Id)
                //                       select new AddVisitorsDTO
                //                       {
                //                           VMMV_Id = a.VMMV_Id,
                //                           VMMV_VisitorName = a.VMMV_VisitorName,
                //                           VMMV_CardNo = a.VMMV_CardNo,
                //                           VMMV_CkeckedInOutStatus = a.VMMV_CkeckedInOutStatus,
                //                           VMMV_EntryDateTime = DateTime.ParseExact(a.VMMV_EntryDateTime, "HH:mm", CultureInfo.InvariantCulture).ToString("hh:mm tt"),
                //                           VMMV_IdentityCardType = a.VMMV_IdentityCardType,
                //                           VMMV_FromAddress = a.VMMV_FromAddress,
                //                           VMMV_FromPlace = a.VMMV_FromPlace,
                //                           VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                //                           VMMV_MeetingLocation = a.VMMV_MeetingLocation,
                //                           VMMV_MeetingPurpose = a.VMMV_MeetingPurpose,
                //                           VMMV_Remarks = a.VMMV_Remarks,
                //                           VMMV_VisitorContactNo = a.VMMV_VisitorContactNo,
                //                           VMMV_VisitorEmailid = a.VMMV_VisitorEmailid,
                //                           VMMV_MeetingDuration = a.VMMV_MeetingDuration,
                //                           createddate = a.CreatedDate,
                //                           VMMV_BlocekFlg = a.VMMV_BlocekFlg,

                //                           empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),

                //                           count_subvisitors = visctxt.Multiple_VisitorDMO.Where(z => z.VMMV_Id == a.VMMV_Id).Count(),
                //                           count_documents = visctxt.VM_Master_Visitor_FileDMO.Where(z => z.VMMV_Id == a.VMMV_Id).Count(),

                //                       }).Distinct().OrderByDescending(a => a.createddate).ToList();

                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AddVisitor_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
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
                        data.gridoptions = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.count = data.gridoptions.Length;

                //if (gridoptionslist.Count > 0)
                //{
                //    data.gridoptions = gridoptionslist.ToArray();
                //    data.count = gridoptionslist.Count;
                //}
                //else
                //{
                //    data.count = 0;
                //}

                data.emplist = (from a in visctxt.MasterEmployee
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new AddVisitorsDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName +
                                    (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) +
                                    (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName) +
                                    (string.IsNullOrEmpty(a.HRME_EmployeeCode) ? "" : " : " + a.HRME_EmployeeCode),
                                    HRME_Id = a.HRME_Id
                                }).Distinct().ToArray();

                data.emplistautjorizedby = (from a in _db.HR_Master_Employee_DMO
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                            select new AddVisitorsDTO
                                            {
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName +
                                    (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) +
                                    (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName) +
                                    (string.IsNullOrEmpty(a.HRME_EmployeeCode) ? "" : " : " + a.HRME_EmployeeCode),
                                                HRME_Id = a.HRME_Id,
                                            }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public AddVisitorsDTO EditDetails(AddVisitorsDTO id)
        {
            AddVisitorsDTO resp = new AddVisitorsDTO();
            try
            {
                var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                where (a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id)
                                select a).ToList();
                resp.editDetails = editData.ToArray();

                resp.editmultivisitor = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == id.VMMV_Id).ToArray();

                resp.edituploaddocument = visctxt.VM_Master_Visitor_FileDMO.Where(a => a.VMMV_Id == id.VMMV_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }
        public AddVisitorsDTO deactivate(AddVisitorsDTO obj)
        {
            try
            {
                var query = visctxt.Visitor_Management_MasterVisitor_DMO.Where(d => d.VMMV_Id == obj.VMMV_Id).ToList();
                if (query.Count > 0)
                {
                    var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == obj.VMMV_Id);
                    update.UpdatedDate = DateTime.Now;
                    if (update.VMMV_ActiveFlag == true)
                    {
                        update.VMMV_ActiveFlag = false;
                    }
                    else
                    {
                        update.VMMV_ActiveFlag = true;
                    }
                    visctxt.Update(update);
                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        obj.value = true;
                        if (obj.VMMV_ActiveFlag == false)
                        {
                            obj.msg = "Record Activated Successfully";
                        }
                        else if (obj.VMMV_ActiveFlag == true)
                        {
                            obj.msg = "Record Deactivated Successfully";
                        }
                    }
                    else
                    {
                        obj.value = false;
                        obj.msg = "activationFailed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public async Task<AddVisitorsDTO> saveDataAsync(AddVisitorsDTO data)
        {
            try
            {
               
                if (data.VMMV_Id == 0)
                {
                    var checkduplicate = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_VisitorName == data.VMMV_VisitorName && a.VMMV_VisitorContactNo == data.VMMV_VisitorContactNo && a.VMMV_VisitorEmailid == data.VMMV_VisitorEmailid && a.VMMV_EntryDateTime == data.VMMV_EntryDateTime && a.VMMV_MeetingDateTime == data.VMMV_MeetingDateTime && a.VMMV_ToMeet == data.VMMV_ToMeet).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }
                    else
                    {
                        Visitor_Management_MasterVisitor_DMO mapp = new Visitor_Management_MasterVisitor_DMO();

                        mapp.MI_Id = data.MI_Id;
                        mapp.VMMV_VisitorName = data.VMMV_VisitorName;
                        mapp.VMMV_VisitorContactNo = data.VMMV_VisitorContactNo;
                        mapp.VMMV_VisitorEmailid = data.VMMV_VisitorEmailid;
                        mapp.VMMV_IdentityCardType = data.VMMV_IdentityCardType;
                        mapp.VMMV_CardNo = data.VMMV_CardNo;
                        mapp.VMMV_CardImage = data.VMMV_CardImage;
                        mapp.VMMV_FromPlace = data.VMMV_FromPlace;
                        mapp.VMMV_VisitTypeFlg = data.VMMV_VisitTypeFlg;
                        mapp.VMMV_FromAddress = data.VMMV_FromAddress;
                        mapp.VMMV_ToMeet = data.VMMV_ToMeet;
                        mapp.VMMV_PersonToMeet = data.VMMV_PersonToMeet;
                        mapp.VMMV_MeetingPurpose = data.VMMV_MeetingPurpose;
                        mapp.VMMV_EntryDateTime = data.VMMV_EntryDateTime;
                        mapp.VMMV_MeetingDateTime = data.VMMV_MeetingDateTime;
                        mapp.VMMV_MeetingDuration = data.VMMV_MeetingDuration;
                        mapp.VMMV_MeetingLocation = data.VMMV_MeetingLocation;
                        mapp.VMMV_PersonsAccompanying = data.VMMV_PersonsAccompanying;
                        mapp.VMMV_AuthorisationBy = data.VMMV_AuthorisationBy;
                        mapp.VMMV_VisitorPhoto = data.VMMV_VisitorPhoto;
                        mapp.VMMV_VisitorFingerPrint = data.VMMV_VisitorFingerPrint;
                        mapp.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                        mapp.VMMV_VehicleNo = data.VMMV_VehicleNo;
                        mapp.VMMV_Remarks = data.VMMV_Remarks;
                        mapp.VMMV_CkeckedInOutStatus = "Checked In";
                        mapp.VMMV_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.VMMV_CreatedBy = data.UserId;
                        mapp.VMMV_UpdatedBy = data.UserId;
                        mapp.VMMV_IDCardNo = data.VMMV_IDCardNo;
                        mapp.VMMV_BlocekFlg = false;

                        visctxt.Add(mapp);
                        visctxt.SaveChanges();
                        if (data.multivisitor != null && data.multivisitor.Length > 0)
                        {
                            foreach (var item in data.multivisitor)
                            {
                                if (item.VMMVVI_VisitorName != null && item.VMMVVI_VisitorName != "")
                                {
                                    Multiple_VisitorDMO mvd = new Multiple_VisitorDMO();

                                    mvd.VMMV_Id = mapp.VMMV_Id;
                                    mvd.VMMVVI_VisitorName = item.VMMVVI_VisitorName;
                                    mvd.VMMVVI_VisitorAddress = item.VMMVVI_VisitorAddress;
                                    mvd.VMMVVI_VisitorEmailId = item.VMMVVI_VisitorEmailId;
                                    mvd.VMMVVI_VisitorContactNo = item.VMMVVI_VisitorContactNo;
                                    mvd.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                    mvd.VMMVVI_CreatedDate = DateTime.Now;
                                    mvd.VMMVVI_CreatedBy = data.UserId;
                                    mvd.VMMVVI_UpdatedBy = data.UserId;
                                    mvd.VMMVVI_Updateddate = DateTime.Now;
                                    mvd.VMMVVI_VisitorCardNo = item.VMMVVI_VisitorCardNo;
                                    mvd.VMMVVI_VisitorPhoto = item.VMMVVI_VisitorPhoto;
                                    mvd.VMMVVI_DocumentUpload = item.VMMVVI_DocumentUpload;
                                    mvd.VMMVVI_IDCardNo = item.VMMVVI_IDCardNo;
                                    mvd.VMMVVI_IDCardReturnedFlg = false;
                                    visctxt.Add(mvd);
                                }
                            }
                        }

                        if (data.uploaddocments != null && data.uploaddocments.Length > 0)
                        {
                            foreach (var c in data.uploaddocments)
                            {
                                VM_Master_Visitor_FileDMO vM_Master_Visitor_FileDMO = new VM_Master_Visitor_FileDMO();
                                vM_Master_Visitor_FileDMO.VMMV_Id = mapp.VMMV_Id;
                                vM_Master_Visitor_FileDMO.VMMVFL_FileName = c.VMMVFL_FileName;
                                vM_Master_Visitor_FileDMO.VMMVFL_FilePath = c.VMMVFL_FilePath;
                                vM_Master_Visitor_FileDMO.VMMVFL_ActiveFlg = true;
                                vM_Master_Visitor_FileDMO.VMMVFL_FileRemarks = c.VMMVFL_FileRemarks;
                                vM_Master_Visitor_FileDMO.VMMVFL_CreatedBy = data.UserId;
                                vM_Master_Visitor_FileDMO.VMMVFL_CreatedDate = DateTime.Now;
                                vM_Master_Visitor_FileDMO.VMMVFL_UpdatedBy = data.UserId;
                                vM_Master_Visitor_FileDMO.VMMVFL_Updateddate = DateTime.Now;
                                visctxt.Add(vM_Master_Visitor_FileDMO);

                            }
                        }

                        string[] split = data.VMMV_EntryDateTime.Split(":");

                        data.fhrors = Convert.ToInt32(split[0]);
                        data.fminutes = Convert.ToInt32(split[1]);

                        Visitor_Management_Visitor_ToMeetDMO management_Visitor_ToMeetDMO = new Visitor_Management_Visitor_ToMeetDMO();
                        management_Visitor_ToMeetDMO.MI_Id = data.MI_Id;
                        management_Visitor_ToMeetDMO.VMMV_Id = mapp.VMMV_Id;
                        management_Visitor_ToMeetDMO.VMVTMT_ToMeet_HRME_Id = data.VMMV_ToMeet;
                        management_Visitor_ToMeetDMO.VMVTMT_DateTime = data.VMMV_MeetingDateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                        management_Visitor_ToMeetDMO.HRME_Id = data.HRME_Id;
                        management_Visitor_ToMeetDMO.VMVTMT_MetFlg = false;
                        management_Visitor_ToMeetDMO.VMVTMT_Location = data.VMMV_MeetingLocation;
                        management_Visitor_ToMeetDMO.VMVTMT_Flg = true;
                        management_Visitor_ToMeetDMO.VMVTMT_CreatedBy = data.UserId;
                        management_Visitor_ToMeetDMO.VMVTMT_UpdatedBy = data.UserId;
                        management_Visitor_ToMeetDMO.VMVTMT_CreatedDate = DateTime.Now;
                        management_Visitor_ToMeetDMO.VMVTMT_UpdatedDate = DateTime.Now;
                        visctxt.Add(management_Visitor_ToMeetDMO);

                        Visitor_Management_Visitor_Appointment_DMO obj2 = new Visitor_Management_Visitor_Appointment_DMO();
                        obj2.VMVAP_Id = data.VMVAP_Id;
                        obj2.MI_Id = data.MI_Id;
                        obj2.VMMV_Id = mapp.VMMV_Id;
                        visctxt.Add(obj2);

                        if (data.VMAP_Id > 0)
                        {
                            //Visitor_Management_Visitor_Appointment_DMO obj2 = new Visitor_Management_Visitor_Appointment_DMO();
                            //obj2.VMAP_Id = data.VMAP_Id;
                            //obj2.MI_Id = data.MI_Id;
                            //obj2.VMMV_Id = mapp.VMMV_Id;
                            //visctxt.Add(obj2);

                            var checkresultcount = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_Id == data.VMAP_Id).ToList();

                            if (checkresultcount.Count > 0)
                            {
                                var checkresult = visctxt.Visitor_Management_Appointment_DMO.Single(a => a.VMAP_Id == data.VMAP_Id);
                                checkresult.VMAP_ChekInOutStatus = "Check In";
                                checkresult.UpdatedDate = DateTime.Now;
                                checkresult.VMAP_UpdatedBy = data.UserId;
                                visctxt.Update(checkresult);
                            }
                        }

                        int s = visctxt.SaveChanges();
                        if (s > 0)
                        {
                            data.returnVal = "saved";

                            var vis_list1 = visctxt.Visitor_Management_MasterVisitor_DMO.OrderByDescending(d => d.VMMV_Id).First();

                            var name = (from a in visctxt.HR_Master_Employee_DMO
                                        from b in visctxt.Multiple_Mobile_DMO
                                        from c in visctxt.Multiple_Email_DMO
                                        where (b.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && a.MI_Id == data.MI_Id && b.HRMEMNO_DeFaultFlag == "default" && c.HRMEM_DeFaultFlag == "default" && a.HRME_Id.Equals(data.VMMV_ToMeet) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                        select new AddVisitorsDTO
                                        {
                                            empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                            HRMEMNO_MobileNo = b.HRMEMNO_MobileNo,
                                            HRMEM_EmailId = c.HRMEM_EmailId,
                                        }).Distinct().SingleOrDefault();

                            string empname = name.empname.ToString();
                            string empmobileno = name.HRMEMNO_MobileNo.ToString();
                            string empemailid = name.HRMEM_EmailId.ToString();

                            DateTime dateTime = DateTime.ParseExact(data.VMMV_EntryDateTime, "HH:mm", CultureInfo.InvariantCulture);
                            string entrydatetime = dateTime.ToString("hh:mm tt");

                            if (data.SMS_Required == true)
                            {
                                if (data.VMMV_VisitorContactNo > 0)
                                {
                                    string smsval = await sendSms(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, entrydatetime.ToString(), empname, data.VMMV_VisitorContactNo, "Visitor");
                                }

                                if (empmobileno != "" || empmobileno != null)
                                {
                                    string smsval2 = await EmpSendSMSAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, entrydatetime.ToString(), empmobileno, empname, "Visitor");
                                }

                            }

                            if (data.Email_Required == true)
                            {
                                if (data.VMMV_VisitorEmailid != null || data.VMMV_VisitorEmailid != "")
                                {
                                    sendmail(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, entrydatetime.ToString(), empname, data.VMMV_VisitorEmailid, "Visitor");
                                }

                                if (empemailid != "" || empemailid != null)
                                {
                                    EmpSendEmailAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, entrydatetime.ToString(), empemailid, empname, "Visitor");
                                }
                            }

                            data.vis_list = (from b in visctxt.Visitor_Management_MasterVisitor_DMO
                                             from a in visctxt.MasterEmployee
                                             where (b.VMMV_Id == vis_list1.VMMV_Id && b.MI_Id == a.MI_Id && a.HRME_Id == vis_list1.VMMV_ToMeet)
                                             select new AddVisitorsDTO
                                             {
                                                 empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                 VMMV_VisitorContactNo = b.VMMV_VisitorContactNo,
                                                 VMMV_VisitorEmailid = b.VMMV_VisitorEmailid,
                                                 VMMV_VisitorName = b.VMMV_VisitorName,
                                                 VMMV_EntryDateTime = b.VMMV_EntryDateTime,
                                                 VMMV_MeetingDateTime = b.VMMV_MeetingDateTime,
                                                 VMMV_MeetingPurpose = b.VMMV_MeetingPurpose,
                                                 VMMV_FromAddress = b.VMMV_FromAddress,
                                                 VMMV_Remarks = b.VMMV_Remarks,
                                                 VMMV_FromPlace = b.VMMV_FromPlace,
                                                 VMMV_MeetingLocation = b.VMMV_MeetingLocation,
                                                 VMMV_Id = b.VMMV_Id,
                                                 VMMV_VisitorPhoto = b.VMMV_VisitorPhoto
                                             }).ToList().ToArray();

                        }
                        else
                        {
                            data.returnVal = "savingFailed";
                        }
                    }

                }

                else if (data.VMMV_Id > 0)
                {

                    var checkduplicate = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.VMMV_Id != data.VMMV_Id && a.MI_Id == data.MI_Id && a.VMMV_VisitorName == data.VMMV_VisitorName && a.VMMV_VisitorContactNo == data.VMMV_VisitorContactNo && a.VMMV_VisitorEmailid == data.VMMV_VisitorEmailid && a.VMMV_EntryDateTime == data.VMMV_EntryDateTime && a.VMMV_MeetingDateTime == data.VMMV_MeetingDateTime && a.VMMV_ToMeet == data.VMMV_ToMeet).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.returnVal = "duplicate";
                    }

                    else
                    {
                        var query = visctxt.Visitor_Management_MasterVisitor_DMO.Where(d => d.VMMV_Id == data.VMMV_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id);

                            update.UpdatedDate = DateTime.Now;
                            update.VMMV_VisitorName = data.VMMV_VisitorName;
                            update.VMMV_VisitorContactNo = data.VMMV_VisitorContactNo;
                            update.VMMV_VisitorEmailid = data.VMMV_VisitorEmailid;
                            update.VMMV_CardNo = data.VMMV_CardNo;
                            update.VMMV_CardImage = data.VMMV_CardImage;
                            update.VMMV_FromPlace = data.VMMV_FromPlace;
                            update.VMMV_VisitorPhoto = data.VMMV_VisitorPhoto;
                            update.VMMV_VisitTypeFlg = data.VMMV_VisitTypeFlg;
                            update.VMMV_FromAddress = data.VMMV_FromAddress;
                            update.VMMV_ToMeet = data.VMMV_ToMeet;
                            update.VMMV_PersonToMeet = data.VMMV_PersonToMeet;
                            update.VMMV_MeetingPurpose = data.VMMV_MeetingPurpose;
                            update.VMMV_EntryDateTime = data.VMMV_EntryDateTime;
                            update.VMMV_MeetingDateTime = data.VMMV_MeetingDateTime;
                            update.VMMV_MeetingDuration = data.VMMV_MeetingDuration;
                            update.VMMV_MeetingLocation = data.VMMV_MeetingLocation;
                            update.VMMV_PersonsAccompanying = data.VMMV_PersonsAccompanying;
                            update.VMMV_AuthorisationBy = data.VMMV_AuthorisationBy;
                            update.VMMV_CkeckedInOutStatus = data.VMMV_CkeckedInOutStatus;
                            update.VMMV_VisitorFingerPrint = data.VMMV_VisitorFingerPrint;
                            update.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                            update.VMMV_Remarks = data.VMMV_Remarks;
                            update.VMMV_VehicleNo = data.VMMV_VehicleNo;
                            update.VMMV_UpdatedBy = data.UserId;

                            visctxt.Update(update);


                            if (data.multivisitor != null && data.multivisitor.Length > 0)
                            {
                                List<long> ids = new List<long>();

                                foreach (var c in data.multivisitor)
                                {
                                    ids.Add(c.VMMVVI_Id);
                                }

                                Array getvalues = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == data.VMMV_Id && !ids.Contains(a.VMMVVI_Id)).ToArray();

                                foreach (var d in getvalues)
                                {
                                    visctxt.Remove(d);
                                }


                                foreach (var item in data.multivisitor)
                                {
                                    if (item.VMMVVI_VisitorName != null && item.VMMVVI_VisitorName != "")
                                    {
                                        if (item.VMMVVI_Id > 0)
                                        {
                                            var result = visctxt.Multiple_VisitorDMO.Single(a => a.VMMV_Id == data.VMMV_Id && a.VMMVVI_Id == item.VMMVVI_Id);
                                            result.VMMVVI_VisitorName = item.VMMVVI_VisitorName;
                                            result.VMMVVI_VisitorAddress = item.VMMVVI_VisitorAddress;
                                            result.VMMVVI_VisitorEmailId = item.VMMVVI_VisitorEmailId;
                                            result.VMMVVI_VisitorContactNo = item.VMMVVI_VisitorContactNo;
                                            result.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                            result.VMMVVI_UpdatedBy = data.UserId;
                                            result.VMMVVI_Updateddate = DateTime.Now;
                                            result.VMMVVI_VisitorCardNo = item.VMMVVI_VisitorCardNo;
                                            result.VMMVVI_VisitorPhoto = item.VMMVVI_VisitorPhoto;
                                            result.VMMVVI_DocumentUpload = item.VMMVVI_DocumentUpload;
                                            result.VMMVVI_IDCardNo = item.VMMVVI_IDCardNo;
                                            visctxt.Update(result);
                                        }
                                        else
                                        {
                                            Multiple_VisitorDMO mvd = new Multiple_VisitorDMO();

                                            mvd.VMMV_Id = data.VMMV_Id;
                                            mvd.VMMVVI_VisitorName = item.VMMVVI_VisitorName;
                                            mvd.VMMVVI_VisitorAddress = item.VMMVVI_VisitorAddress;
                                            mvd.VMMVVI_VisitorEmailId = item.VMMVVI_VisitorEmailId;
                                            mvd.VMMVVI_VisitorContactNo = item.VMMVVI_VisitorContactNo;
                                            mvd.VMMVVI_Remarks = item.VMMVVI_Remarks;
                                            mvd.VMMVVI_CreatedDate = DateTime.Now;
                                            mvd.VMMVVI_CreatedBy = data.UserId;
                                            mvd.VMMVVI_UpdatedBy = data.UserId;
                                            mvd.VMMVVI_Updateddate = DateTime.Now;
                                            mvd.VMMVVI_VisitorCardNo = item.VMMVVI_VisitorCardNo;
                                            mvd.VMMVVI_DocumentUpload = item.VMMVVI_DocumentUpload;
                                            mvd.VMMVVI_VisitorPhoto = item.VMMVVI_VisitorPhoto;
                                            mvd.VMMVVI_IDCardNo = item.VMMVVI_IDCardNo;
                                            mvd.VMMVVI_IDCardReturnedFlg = false;
                                            visctxt.Add(mvd);
                                        }
                                    }
                                }
                            }

                            if (data.uploaddocments != null && data.uploaddocments.Length > 0)
                            {
                                List<long> ids = new List<long>();

                                foreach (var c in data.uploaddocments)
                                {
                                    ids.Add(c.VMMVFL_Id);
                                }

                                Array getvalues = visctxt.VM_Master_Visitor_FileDMO.Where(a => a.VMMV_Id == data.VMMV_Id && !ids.Contains(a.VMMVFL_Id)).ToArray();

                                foreach (var d in getvalues)
                                {
                                    visctxt.Remove(d);
                                }


                                foreach (var c in data.uploaddocments)
                                {
                                    if (c.VMMVFL_Id > 0)
                                    {
                                        var result = visctxt.VM_Master_Visitor_FileDMO.Single(a => a.VMMV_Id == data.VMMV_Id && a.VMMVFL_Id == c.VMMVFL_Id);
                                        result.VMMVFL_FileName = c.VMMVFL_FileName;
                                        result.VMMVFL_FilePath = c.VMMVFL_FilePath;
                                        result.VMMVFL_FileRemarks = c.VMMVFL_FileRemarks;
                                        result.VMMVFL_UpdatedBy = data.UserId;
                                        result.VMMVFL_Updateddate = DateTime.Now;
                                        visctxt.Update(result);
                                    }
                                    else
                                    {
                                        VM_Master_Visitor_FileDMO vM_Master_Visitor_FileDMO = new VM_Master_Visitor_FileDMO();
                                        vM_Master_Visitor_FileDMO.VMMV_Id = data.VMMV_Id;
                                        vM_Master_Visitor_FileDMO.VMMVFL_FileName = c.VMMVFL_FileName;
                                        vM_Master_Visitor_FileDMO.VMMVFL_FilePath = c.VMMVFL_FilePath;
                                        vM_Master_Visitor_FileDMO.VMMVFL_FileRemarks = c.VMMVFL_FileRemarks;
                                        vM_Master_Visitor_FileDMO.VMMVFL_ActiveFlg = true;
                                        vM_Master_Visitor_FileDMO.VMMVFL_CreatedBy = data.UserId;
                                        vM_Master_Visitor_FileDMO.VMMVFL_CreatedDate = DateTime.Now;
                                        vM_Master_Visitor_FileDMO.VMMVFL_UpdatedBy = data.UserId;
                                        vM_Master_Visitor_FileDMO.VMMVFL_Updateddate = DateTime.Now;
                                        visctxt.Add(vM_Master_Visitor_FileDMO);
                                    }
                                }
                            }

                            string[] split = data.VMMV_EntryDateTime.Split(":");

                            data.fhrors = Convert.ToInt32(split[0]);
                            data.fminutes = Convert.ToInt32(split[1]);

                            var checkresult = visctxt.Visitor_Management_Visitor_ToMeetDMO.Where(a => a.MI_Id == data.MI_Id && a.VMMV_Id == data.VMMV_Id).ToList();

                            if (checkresult.Count > 0)
                            {
                                var checkresultupdate = visctxt.Visitor_Management_Visitor_ToMeetDMO.Single(a => a.MI_Id == data.MI_Id
                                && a.VMMV_Id == data.VMMV_Id && a.VMVTMT_Id == checkresult.FirstOrDefault().VMVTMT_Id);
                                checkresultupdate.VMVTMT_ToMeet_HRME_Id = data.VMMV_ToMeet;
                                checkresultupdate.VMVTMT_Location = data.VMMV_MeetingLocation;
                                checkresultupdate.HRME_Id = data.HRME_Id;
                                checkresultupdate.VMVTMT_DateTime = data.VMMV_MeetingDateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                                checkresultupdate.VMVTMT_UpdatedBy = data.UserId;
                                checkresultupdate.VMVTMT_UpdatedDate = DateTime.Now;
                                visctxt.Update(checkresultupdate);
                            }
                            else
                            {
                                Visitor_Management_Visitor_ToMeetDMO management_Visitor_ToMeetDMO = new Visitor_Management_Visitor_ToMeetDMO();
                                management_Visitor_ToMeetDMO.MI_Id = data.MI_Id;
                                management_Visitor_ToMeetDMO.VMMV_Id = data.VMMV_Id;
                                management_Visitor_ToMeetDMO.VMVTMT_ToMeet_HRME_Id = data.VMMV_ToMeet;
                                management_Visitor_ToMeetDMO.VMVTMT_Location = data.VMMV_MeetingLocation;
                                management_Visitor_ToMeetDMO.HRME_Id = data.HRME_Id;
                                management_Visitor_ToMeetDMO.VMVTMT_DateTime = data.VMMV_MeetingDateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                                management_Visitor_ToMeetDMO.VMVTMT_MetFlg = false;
                                management_Visitor_ToMeetDMO.VMVTMT_Flg = true;
                                management_Visitor_ToMeetDMO.VMVTMT_CreatedBy = data.UserId;
                                management_Visitor_ToMeetDMO.VMVTMT_UpdatedBy = data.UserId;
                                management_Visitor_ToMeetDMO.VMVTMT_CreatedDate = DateTime.Now;
                                management_Visitor_ToMeetDMO.VMVTMT_UpdatedDate = DateTime.Now;
                                visctxt.Add(management_Visitor_ToMeetDMO);

                            }

                            int s = visctxt.SaveChanges();
                            if (s > 0)
                            {
                                data.returnVal = "updated";
                            }
                            else
                            {
                                data.returnVal = "updateFailed";
                            }
                        }
                    }

                }

                data.institution = visctxt.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO GetMultiVisitorDetails(AddVisitorsDTO data)
        {
            try
            {
                data.getmultivisitorlist = visctxt.Multiple_VisitorDMO.Where(e => e.VMMV_Id == data.VMMV_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO GetVisitorDetails(AddVisitorsDTO data)
        {
            try
            {
                var gridoptionslist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                       from e in visctxt.MasterEmployee
                                       where (a.VMMV_ToMeet == e.HRME_Id && a.MI_Id == data.MI_Id && a.VMMV_Id == data.VMMV_Id)
                                       select new AddVisitorsDTO
                                       {
                                           VMMV_Id = a.VMMV_Id,
                                           VMMV_VisitorName = a.VMMV_VisitorName,
                                           VMMV_CardNo = a.VMMV_CardNo,
                                           VMMV_CkeckedInOutStatus = a.VMMV_CkeckedInOutStatus,
                                           VMMV_EntryDateTime = DateTime.ParseExact(a.VMMV_EntryDateTime, "HH:mm", CultureInfo.InvariantCulture).ToString("hh:mm tt"),
                                           VMMV_IdentityCardType = a.VMMV_IdentityCardType,
                                           VMMV_FromAddress = a.VMMV_FromAddress,
                                           VMMV_FromPlace = a.VMMV_FromPlace,
                                           VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                           VMMV_MeetingLocation = a.VMMV_MeetingLocation,
                                           VMMV_MeetingPurpose = a.VMMV_MeetingPurpose,
                                           VMMV_Remarks = a.VMMV_Remarks,
                                           VMMV_VisitorContactNo = a.VMMV_VisitorContactNo,
                                           VMMV_VisitorEmailid = a.VMMV_VisitorEmailid,
                                           VMMV_MeetingDuration = a.VMMV_MeetingDuration,
                                           createddate = a.CreatedDate,

                                           empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName)

                                       }).Distinct().OrderByDescending(a => a.createddate).ToList();

                data.viewdetails = gridoptionslist.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public async Task<AddVisitorsDTO> UpdateStatus(AddVisitorsDTO data)
        {
            try
            {
                var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id && d.MI_Id == data.MI_Id);
                update.UpdatedDate = DateTime.Now;
                update.VMMV_ExitDate = data.VMMV_ExitDate;
                update.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                update.VMMV_CkeckedInOutStatus = "Checked Out";
                update.VMMV_UpdatedBy = data.UserId;
                visctxt.Update(update);

                var checkresultcount = visctxt.Visitor_Management_Visitor_Appointment_DMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToList();

                if (checkresultcount.Count > 0)
                {
                    var checkresultcountd = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_Id == checkresultcount.FirstOrDefault().VMAP_Id).ToList();

                    if (checkresultcountd.Count > 0)
                    {
                        DateTime fromdatecon1 = DateTime.Now;
                        string confromdate1 = "";

                        fromdatecon1 = Convert.ToDateTime(data.VMMV_ExitDate.Value.Date.ToString("yyyy-MM-dd"));
                        confromdate1 = fromdatecon1.ToString("yyyy-MM-dd") + " " + data.VMMV_ExitDateTime;

                        var checkresult = visctxt.Visitor_Management_Appointment_DMO.Single(a => a.VMAP_Id == checkresultcount.FirstOrDefault().VMAP_Id);
                        checkresult.VMAP_ChekInOutStatus = "Check Out";
                        checkresult.VMAP_ExitDateTime = confromdate1;
                        checkresult.UpdatedDate = DateTime.Now;
                        checkresult.VMAP_UpdatedBy = data.UserId;
                        visctxt.Update(checkresult);
                    }
                }

                int s = visctxt.SaveChanges();
                if (s > 0)
                {
                    data.returnVal = "updated";
                    string visitorname = "";
                    var contactno = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == data.VMMV_Id).VMMV_VisitorContactNo;
                    var mailId = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == data.VMMV_Id).VMMV_VisitorEmailid;

                    var name = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                where (a.VMMV_Id == data.VMMV_Id)
                                select new AppointmentStatusDTO { VMMV_VisitorName = a.VMMV_VisitorName }).Distinct().SingleOrDefault();

                    visitorname = name.VMMV_VisitorName.ToString();

                    var Template = "VisitorStatus";

                    if (data.SMS_Required_Update == true)
                    {
                        if (contactno != 0)
                        {
                            if (update.VMMV_CkeckedInOutStatus == "Checked Out")
                            {
                                string smsval = await sendSms(data.MI_Id, contactno, Template, visitorname);
                            }
                        }
                    }
                    if (data.Email_Required_Update == true)
                    {
                        if (mailId != "" && mailId != null)
                        {
                            if (update.VMMV_CkeckedInOutStatus == "Checked Out")
                            {
                                sendmail(data.MI_Id, mailId, Template, visitorname);
                            }
                        }
                    }
                }
                else
                {
                    data.returnVal = "updateFailed";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO BlockOrUblockVisitor(AddVisitorsDTO data)
        {
            try
            {
                var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id && d.MI_Id == data.MI_Id);
                update.UpdatedDate = DateTime.Now;
                update.VMMV_BlocekFlg = update.VMMV_BlocekFlg == true ? false : true;
                update.VMMV_UpdatedBy = data.UserId;
                visctxt.Update(update);
                int s = visctxt.SaveChanges();

                if (s > 0)
                {
                    data.returnVal = "updated";
                }
                else
                {
                    data.returnVal = "updateFailed";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO GetVisitorMultiDocuments(AddVisitorsDTO data)
        {
            try
            {
                data.viewdocumentdetails = visctxt.VM_Master_Visitor_FileDMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO GetVisitorIdCardDetails(AddVisitorsDTO data)
        {
            try
            {
                data.editidcardDetails = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToArray();

                data.editmutlivisitoridcardDetails = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO UpdateIDCardDetails(AddVisitorsDTO data)
        {
            try
            {
                if (data.VMMV_Id > 0)
                {

                    var result = visctxt.Visitor_Management_MasterVisitor_DMO.Single(a => a.VMMV_Id == data.VMMV_Id);
                    result.UpdatedDate = DateTime.Now;
                    result.VMMV_UpdatedBy = data.UserId;
                    if (data.VMMV_IDCardReturnedDateTime != null)
                    {
                        result.VMMV_IDCardReturnedDateTime = data.VMMV_IDCardReturnedDateTime.Value.Date.AddHours(data.return_hh).AddMinutes(data.return_mm);
                    }
                    result.VMMV_IDCardReturnedFlg = data.VMMV_IDCardReturnedFlg;
                    visctxt.Update(result);

                    foreach (var c in data.multivisitor)
                    {
                        var result_visitor = visctxt.Multiple_VisitorDMO.Single(a => a.VMMV_Id == data.VMMV_Id && a.VMMVVI_Id == c.VMMVVI_Id);
                        result_visitor.VMMVVI_Updateddate = DateTime.Now;
                        result_visitor.VMMVVI_UpdatedBy = data.UserId;
                        if (data.VMMV_IDCardReturnedDateTime != null)
                        {
                            result_visitor.VMMVVI_IDCardReturnedDateTime = c.VMMVVI_IDCardReturnedDateTime.Value.Date.AddHours(Convert.ToInt64(c.totimehr)).AddMinutes(Convert.ToInt64(c.totimemin));
                        }
                        result_visitor.VMMVVI_IDCardReturnedFlg = c.VMMVVI_IDCardReturnedFlg;
                        visctxt.Update(result_visitor);
                    }

                    var i = visctxt.SaveChanges();
                    if (i > 0)
                    {
                        data.returnVal = "Update";
                    }
                    else
                    {
                        data.returnVal = "Failed";
                    }
                }
            }
            catch (Exception e)
            {
                data.returnVal = "Failed";
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO SearchPreviousVisitor(AddVisitorsDTO data)
        {
            try
            {
                data.getpreviousvisitorlist = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id
                && a.VMMV_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                data.returnVal = "Failed";
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public AddVisitorsDTO AddPreviousVisitorDetails(AddVisitorsDTO data)
        {
            try
            {
                data.getpreviousvisitordetails = visctxt.Visitor_Management_MasterVisitor_DMO.Where(a => a.MI_Id == data.MI_Id
                && a.VMMV_ActiveFlag == true && a.VMMV_Id == data.VMMV_Id).Distinct().ToArray();

                data.getpreviousvisitor_multivisitors = visctxt.Multiple_VisitorDMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToArray();

                data.getpreviousvisitor_documents = visctxt.VM_Master_Visitor_FileDMO.Where(a => a.VMMV_Id == data.VMMV_Id).ToArray();
            }
            catch (Exception e)
            {
                data.returnVal = "Failed";
                Console.WriteLine(e.Message);
            }

            return data;
        }

        //Assign Details
        public AddVisitorsDTO getAssignDetails(AddVisitorsDTO data)
        {
            try
            {
                var gethrmeid = _db.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                data.emplist = (from a in _db.HR_Master_Employee_DMO
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                && a.HRME_Id != gethrmeid.FirstOrDefault().Emp_Code && a.MI_Id==data.MI_Id)
                                select new AddVisitorsDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                }).Distinct().ToArray();

                data.emplistautjorizedby = (from a in _db.HR_Master_Employee_DMO
                                            where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id)
                                            select new AddVisitorsDTO
                                            {
                                                HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                                HRME_Id = a.HRME_Id,
                                            }).Distinct().ToArray();

                data.visitorlist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    from b in visctxt.Visitor_Management_Visitor_ToMeetDMO
                                    from c in visctxt.Institute
                                    where (a.VMMV_Id == b.VMMV_Id && c.MI_Id == a.MI_Id && a.VMMV_CkeckedInOutStatus != "Checked Out"
                                    && b.VMVTMT_ToMeet_HRME_Id == gethrmeid.FirstOrDefault().Emp_Code && b.VMVTMT_MetFlg == false && a.MI_Id == data.MI_Id)
                                    select new AddVisitorsDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName
                                    }).Distinct().ToArray();

                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Get_Visitor_Details_Emp";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = gethrmeid.FirstOrDefault().Emp_Code });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.assigned_visitorlist = retObject.ToArray();
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
        public AddVisitorsDTO getVisitorAssignDetails(AddVisitorsDTO data)
        {
            try
            {

                var gethrmeid = _db.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Get_Visitor_Details_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@VMMV_Id", SqlDbType.VarChar)
                    {
                        Value = data.VMMV_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = gethrmeid.FirstOrDefault().Emp_Code
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
                        data.gridoptions = retObject.ToArray();
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
        public async Task<AddVisitorsDTO> saveAssignedData(AddVisitorsDTO data)
        {
            try
            {             
                var gethrmeid = _db.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                var checkduplicate = visctxt.Visitor_Management_Visitor_ToMeetDMO.Where(a => a.VMMV_Id == data.VMMV_Id
                && a.VMVTMT_ToMeet_HRME_Id == data.VMVTMT_ToMeet_HRME_Id && a.HRME_Id == gethrmeid.FirstOrDefault().Emp_Code).ToList();

                if (checkduplicate.Count > 0)
                {
                    var checkresult = visctxt.Visitor_Management_Visitor_ToMeetDMO.Single(a => a.VMMV_Id == data.VMMV_Id
                    && a.VMVTMT_ToMeet_HRME_Id == data.VMVTMT_ToMeet_HRME_Id && a.HRME_Id == gethrmeid.FirstOrDefault().Emp_Code);
                    checkresult.VMVTMT_UpdatedBy = data.UserId;
                    checkresult.VMVTMT_UpdatedDate = DateTime.Now;
                    visctxt.Update(checkresult);
                }
                else
                {
                    Visitor_Management_Visitor_ToMeetDMO visitor_Management_Visitor_ToMeetDMO = new Visitor_Management_Visitor_ToMeetDMO();
                    visitor_Management_Visitor_ToMeetDMO.VMMV_Id = data.VMMV_Id;
                    visitor_Management_Visitor_ToMeetDMO.MI_Id = data.MI_Id;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_ToMeet_HRME_Id = data.VMVTMT_ToMeet_HRME_Id;
                    visitor_Management_Visitor_ToMeetDMO.HRME_Id = gethrmeid.FirstOrDefault().Emp_Code;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_DateTime = data.VMVTMT_DateTime.AddHours(data.fhrors).AddMinutes(data.fminutes);
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_Flg = true;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_UpdatedBy = data.UserId;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_UpdatedDate = DateTime.Now;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_CreatedBy = data.UserId;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_CreatedDate = DateTime.Now;
                    visitor_Management_Visitor_ToMeetDMO.VMVTMT_Location = data.VMVTMT_Location;
                    visctxt.Add(visitor_Management_Visitor_ToMeetDMO);
                }


                var getresult = visctxt.Visitor_Management_Visitor_ToMeetDMO.Single(a => a.VMVTMT_Id == data.VMVTMT_Id);

                getresult.VMVTMT_MetFlg = data.VMVTMT_MetFlg;
                getresult.VMVTMT_Remarks = data.VMVTMT_Remarks;

                getresult.VMVTMT_DateTime = data.VMVTMT_DateTime;
                visctxt.Update(getresult);

                var i = visctxt.SaveChanges();
                if (i > 0)
                {
                    data.returnVal = "saved";

                    var vis_list1 = visctxt.Visitor_Management_MasterVisitor_DMO.OrderByDescending(d => d.VMMV_Id == data.VMMV_Id).First();

                    data.VMMV_VisitorName = vis_list1.VMMV_VisitorName;
                    data.VMMV_MeetingDateTime = vis_list1.VMMV_MeetingDateTime;
                    data.VMMV_EntryDateTime = vis_list1.VMMV_EntryDateTime;

                    string empname = "";
                    string empmobileno = "";
                    string empemailid = "";

                    if (data.SMS_Required == true || data.Email_Required==true)
                    {
                        var name = (from a in _db.HR_Master_Employee_DMO
                                    from b in _db.Multiple_Mobile_DMO
                                    from c in _db.Multiple_Email_DMO
                                    where (b.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && a.MI_Id == data.MI_Id && b.HRMEMNO_DeFaultFlag == "default"
                                    && c.HRMEM_DeFaultFlag == "default" && a.HRME_Id.Equals(data.VMVTMT_ToMeet_HRME_Id) && a.HRME_ActiveFlag == true
                                    && a.HRME_LeftFlag == false)
                                    select new AddVisitorsDTO
                                    {
                                        empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                        HRMEMNO_MobileNo = b.HRMEMNO_MobileNo,
                                        HRMEM_EmailId = c.HRMEM_EmailId,
                                    }).Distinct().SingleOrDefault();

                        empname = name.empname.ToString();
                        empmobileno = name.HRMEMNO_MobileNo.ToString();
                        empemailid = name.HRMEM_EmailId.ToString();
                    }                      

                    if (data.SMS_Required == true)
                    {
                        if (empmobileno != "" || empmobileno != null)
                        {
                            //SMS sentsmsstaff = new SMS(_db);

                            //var e = sentsmsstaff.EmpSendSMSAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empmobileno, empname, "Visitor").Result;

                            string smsval2 = await EmpSendSMSAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empmobileno, empname, "Visitor");
                        }
                    }

                    if (data.Email_Required == true)
                    {
                        if (empemailid != "" || empemailid != null)
                        {
                            //Email email1 = new Email(_db);
                            //email1.EmpSendEmail(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString(), empemailid, empname, "Visitor");

                            string e = EmpSendEmailAsync(data.MI_Id, data.VMMV_VisitorName, data.VMMV_MeetingDateTime, data.VMMV_EntryDateTime.ToString().ToString(), empemailid, empname, "Visitor").ToString();
                        }
                    }
                }
                else
                {
                    data.returnVal = "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO GetVisitorAssginDetails(AddVisitorsDTO id)
        {
            try
            {
                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Visitor_Get_Visitor_Assigned_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = id.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@VMMV_Id", SqlDbType.VarChar)
                    {
                        Value = id.VMMV_Id
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
                        id.visitorassigndetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }

        // Appointment Visitor
        public AddVisitorsDTO SearchAppVisitors(AddVisitorsDTO data)
        {
            try
            {
                var getvmap_id = visctxt.Visitor_Management_Visitor_Appointment_DMO.Where(a => a.MI_Id == data.MI_Id).Select(a => a.VMAP_Id).ToList();

                var getappvistiordetails = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.MI_Id == data.MI_Id
                && a.VMAP_ActiveFlag == true && !getvmap_id.Contains(a.VMAP_Id) && a.VMAP_Status == "Approved").ToArray();
                data.getappvistiordetails = getappvistiordetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AddVisitorsDTO GetAppointmentVisitorDetails(AddVisitorsDTO id)
        {
            try
            {
                id.getAppointmentdetails = visctxt.Visitor_Management_Appointment_DMO.Where(a => a.VMAP_Id == id.VMAP_Id).ToArray();

                id.getAppointment_visitordetails = visctxt.Visitor_Management_Appointment_VisitorsDMO.Where(a => a.VMAP_Id == id.VMAP_Id).ToArray();

                id.getAppointment_filesdetails = visctxt.Visitor_Management_Appointment_FilesDMO.Where(a => a.VMAP_Id == id.VMAP_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }


        public async Task<string> sendSms(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empname, long VMMV_VisitorContactNo, string Template)
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

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                // var Date_Visit1 = (Convert.ToDateTime(Date_Visit).Date).ToString();

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
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

                    string PHNO = VMMV_VisitorContactNo.ToString();

                    //PHNO = "9771237044";
                    url = url.Replace("PHNO", PHNO);
                    //PHNO = "9581586484";
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

                    #region
                    //      using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //      {
                    //          var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    //          var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    //          var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    //          cmd.CommandText = "VISITOR_SMS_Outgoing";
                    //          cmd.CommandType = CommandType.StoredProcedure;
                    //          cmd.Parameters.Add(new SqlParameter("@MobileNo",
                    //              SqlDbType.NVarChar)
                    //          {
                    //              Value = PHNO
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@Message",
                    //             SqlDbType.NVarChar)
                    //          {
                    //              Value = sms
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@module",
                    //          SqlDbType.VarChar)
                    //          {
                    //              Value = modulename[0]
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //         SqlDbType.BigInt)
                    //          {
                    //              Value = MI_Id
                    //          });

                    //          cmd.Parameters.Add(new SqlParameter("@status",
                    //     SqlDbType.VarChar)
                    //          {
                    //              Value = "Delivered"
                    //          });

                    //          cmd.Parameters.Add(new SqlParameter("@Message_id",
                    //  SqlDbType.VarChar)
                    //          {
                    //              Value = messageid
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@To_FLag",
                    //SqlDbType.VarChar)
                    //          {
                    //              Value = "VISITOR"
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@System_Ip",
                    //  SqlDbType.VarChar)
                    //          {
                    //              Value = remoteIpAddress
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@network_Ip",
                    //  SqlDbType.VarChar)
                    //          {
                    //              Value = myIP1
                    //          });
                    //          cmd.Parameters.Add(new SqlParameter("@MacAddress_Ip",
                    //  SqlDbType.VarChar)
                    //          {
                    //              Value = sMacAddress
                    //          });

                    //          if (cmd.Connection.State != ConnectionState.Open)
                    //              cmd.Connection.Open();

                    //          try
                    //          {
                    //              using (var dataReader = cmd.ExecuteReader())
                    //              {
                    //              }
                    //          }
                    //          catch (Exception ex)
                    //          {
                    //              return ex.Message;
                    //          }
                    //      }
                    #endregion

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "VISITOR";
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
        public async void sendmail(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empname, string VMMV_VisitorEmailid, string Template)
        {

            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        Mailmsg = result_value;
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
                        //VMMV_VisitorEmailid = "amanrce@gmail.com";
                        message.AddTo(VMMV_VisitorEmailid);
                        //VMMV_VisitorEmailid = "maruthi.globalqtytrainig.com";
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
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = VMMV_VisitorEmailid
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
                        cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        SqlDbType.VarChar)
                        {
                            Value = "VISITOR"
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
        public async Task<string> EmpSendSMSAsync(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empmobileno, string empname, string Template)
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

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                // var Date_Visit1 = (Convert.ToDateTime(Date_Visit).Date).ToString();

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        sms = result;
                    }
                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = empmobileno;

                    //PHNO = "9771237044";
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
                    #region
                    //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //    {
                    //        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    //        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    //        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    //        cmd.CommandText = "VISITOR_SMS_Outgoing";
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                    //            SqlDbType.NVarChar)
                    //        {
                    //            Value = PHNO
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@Message",
                    //           SqlDbType.NVarChar)
                    //        {
                    //            Value = sms
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@module",
                    //        SqlDbType.VarChar)
                    //        {
                    //            Value = modulename[0]
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //       SqlDbType.BigInt)
                    //        {
                    //            Value = MI_Id
                    //        });

                    //        cmd.Parameters.Add(new SqlParameter("@status",
                    //   SqlDbType.VarChar)
                    //        {
                    //            Value = "Delivered"
                    //        });

                    //        cmd.Parameters.Add(new SqlParameter("@Message_id",
                    //SqlDbType.VarChar)
                    //        {
                    //            Value = messageid
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@To_FLag",
                    //SqlDbType.VarChar)
                    //        {
                    //            Value = "VISITOR"
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@System_Ip",
                    //SqlDbType.VarChar)
                    //        {
                    //            Value = remoteIpAddress
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@network_Ip",
                    //SqlDbType.VarChar)
                    //        {
                    //            Value = myIP1
                    //        });
                    //        cmd.Parameters.Add(new SqlParameter("@MacAddress_Ip",
                    //SqlDbType.VarChar)
                    //        {
                    //            Value = sMacAddress
                    //        });

                    //        if (cmd.Connection.State != ConnectionState.Open)
                    //            cmd.Connection.Open();

                    //        try
                    //        {
                    //            using (var dataReader = cmd.ExecuteReader())
                    //            {
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            return ex.Message;
                    //        }
                    //    }
                    #endregion

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "VISITOR";
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
        public async Task<string> EmpSendEmailAsync(long MI_Id, string VMMV_VisitorName, DateTime? VMMV_MeetingDateTime, string VMMV_EntryDateTime, string empemailid, string empname, string Template)
        {

            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime? date = VMMV_MeetingDateTime;
                string Date_Visit1 = date?.ToString("dd-MM-yyyy");

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, empname);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[DATE]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, Date_Visit1);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TIME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_EntryDateTime);
                        Mailmsg = result_value;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[TO_MEET]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, VMMV_VisitorName);
                        Mailmsg = result_value;
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
                        //empemailid = "amanrce@gmail.com";
                        message.AddTo(empemailid);

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
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = empemailid
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
                        cmd.Parameters.Add(new SqlParameter("@To_FLag",
                       SqlDbType.VarChar)
                        {
                            Value = "VISITOR"
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


        // STATUS UPDATE SMS AND EMAIL
        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, string visitorname)
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

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, visitorname);
                        sms = result;
                    }
                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    //PHNO = "9581586484";
                    url = url.Replace("PHNO", PHNO);
                    //PHNO = "9581586484";
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
                    dmo2.To_FLag = "VISITOR";
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
        public string sendmail(long MI_Id, string Email, string Template, string visitorname)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, visitorname);
                        Mailmsg = result_value;
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
                        //VMMV_VisitorEmailid = "amanrce@gmail.com";
                        message.AddTo(Email);

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
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
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
                        cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        SqlDbType.VarChar)
                        {
                            Value = "VISITOR"
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
    }
}