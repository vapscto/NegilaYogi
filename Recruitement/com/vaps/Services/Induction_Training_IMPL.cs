using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Induction_Training_IMPL : Interfaces.Induction_Training_Interface
    {
        public VMSContext _vmsconte;
        public DomainModelMsSqlServerContext _dsc;
        private readonly DomainModelMsSqlServerContext _db;

        public Induction_Training_IMPL(VMSContext vm, DomainModelMsSqlServerContext ds, DomainModelMsSqlServerContext dm)
        {
            _vmsconte = vm;
            _dsc = ds;
            _db = dm;
        }
        HR_Master_Building MB = new HR_Master_Building();
        HR_Master_Floor MF = new HR_Master_Floor();
        HR_Master_Room MR = new HR_Master_Room();
        HR_Training_Create_DMO HI = new HR_Training_Create_DMO();
        HR_Master_External_Trainer_Creation_DMO HT = new HR_Master_External_Trainer_Creation_DMO();
        HR_Master_Employee_DMO ME = new HR_Master_Employee_DMO();
        public HR_Training_Create_DTO getalldata(HR_Training_Create_DTO HID)
        {
            List<HR_Training_Create_DTO> hr = new List<HR_Training_Create_DTO>();
            List<HR_Training_Create_DTO> hr1 = new List<HR_Training_Create_DTO>();
            try
            {
                HID.buillist = _vmsconte.HR_Master_Building_con.Where(a => a.HRMB_ActiveFlag == true && a.MI_Id == HID.MI_Id).Distinct().ToArray();
                HID.deptlist = _vmsconte.HR_Master_Department.Where(d => d.HRMD_ActiveFlag == true && d.MI_Id == HID.MI_Id).Distinct().ToArray();
                HID.tname_list = _vmsconte.HR_Master_External_Trainer_Creation_DMO_con.ToList().ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "participates_Employee_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(HID.MI_Id)
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
                        HID.trinee_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                // HID.trinee_list = _vmsconte.Hr_Master_Employee_con.Where(a => a.MI_Id == HID.MI_Id).Distinct().ToArray();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "program_dd_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(HID.MI_Id)
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
                        HID.program_dd_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                HID.training_creation_list = (from a in _vmsconte.HR_Training_Create_DMO_con
                                              from b in _vmsconte.HR_Master_Building_con
                                              where (a.MI_Id == HID.MI_Id && a.HRMB_Id == b.HRMB_Id)
                                              select new HR_Training_Create_DTO
                                              {
                                                  HRTCR_Id = a.HRTCR_Id,
                                                  HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                                  HRTCR_StartDate = a.HRTCR_StartDate,
                                                  HRTCR_EndDate = a.HRTCR_EndDate,
                                                  HRMB_BuildingName = b.HRMB_BuildingName,
                                                  HRMB_Id = a.HRMB_Id,
                                                  HRTCR_InternalORExternalFlg = a.HRTCR_InternalORExternalFlg,
                                                  HRTCR_ActiveFlag = a.HRTCR_ActiveFlag
                                              }).OrderByDescending(a=>a.HRTCR_StartDate).ToList().ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Induction_Training_Create_List_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(HID.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(HID.userId)
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
                                hr.Add(new HR_Training_Create_DTO
                                {
                                    HRTCR_PrgogramName = (dataReader["HRTCR_PrgogramName"]).ToString(),
                                    HRTCR_StartDate = (DateTime)dataReader["MinDate"],
                                    HRTCR_EndDate = (DateTime)dataReader["MaxDate"],
                                    HRTCR_InternalORExternalFlg = Convert.ToBoolean((dataReader["HRTCR_InternalORExternalFlg"])),
                                    HRTCR_ActiveFlag = Convert.ToBoolean((dataReader["HRTCR_ActiveFlag"])),
                                    HRTCR_Id = Convert.ToInt32((dataReader["HRTCR_Id"]).ToString()),
                                    HRTCR_StatusFlg = Convert.ToInt32((dataReader["HRTCR_StatusFlg"]).ToString()),
                                });
                            }
                        }
                        HID.training_details_list = hr.OrderByDescending(t=>t.HRTCR_StartDate).ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())

                {
                    cmd.CommandText = "Induction_Training_Create_check_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(HID.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(HID.userId)
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
                                hr1.Add(new HR_Training_Create_DTO
                                {

                                    checkadmin = Convert.ToInt32((dataReader["checkadmin"]).ToString()),
                                });
                            }
                            
                        }
                        HID.training_details_Check = hr1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return HID;
        }
        public Hr_Master_Employee_DTO getEmpDD(Hr_Master_Employee_DTO dto)
        {

            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "participates_Employee_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.participates_Employee_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

               // dto.participates_Employee_list = _dsc.HR_Master_Employee_DMO.Where(a => a.HRMD_Id == dto.HRMD_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Master_Floor_DTO getFloorDD(HR_Master_Floor_DTO dto)
        {
            HR_Master_Floor_DTO HFD = new HR_Master_Floor_DTO();
            try
            {
                HFD.floor_list = _vmsconte.HR_Master_Floor_con.Where(a => a.HRMB_Id == dto.HRMB_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return HFD;
        }
        public HR_Master_Room_DTO getRoomDD(HR_Master_Room_DTO dto)
        {
            HR_Master_Room_DTO HRD = new HR_Master_Room_DTO();
            try
            {
                HRD.room_list = _vmsconte.HR_Master_Room_con.Where(a => a.HRMB_Id == dto.HRMB_Id && a.HRMF_Id == dto.HRMF_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return HRD;
        }
        public HR_Training_Create_DTO get_trainer(HR_Training_Create_DTO dto)
        {
            try
            {
                dto.topiclist = _vmsconte.HR_Master_TrainingTopicDMO.Where(t => t.HRMTT_ActiveFlg == true).ToArray();
                var result = _vmsconte.HR_Training_Create_DMO_con.Single(a => a.HRTCR_Id == dto.HRTCR_Id && a.MI_Id == dto.MI_Id);
                if (result.HRTCR_InternalORExternalFlg == false)
                {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Induction_trainer_proc";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.HRTCR_Id)
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
                            dto.trinee_list = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }
                else
                {
                    dto.trinee_list = (from a in _vmsconte.HR_Master_External_Trainer_Creation_DMO_con.Where(a => a.MI_Id == dto.MI_Id)
                                       select new HR_Training_Create_DTO
                                       {
                                           HRME_Id = a.HRMETR_Id,
                                           HRME_EmployeeFirstName = a.HRMETR_Name
                                       }).Distinct().ToArray();


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO SaveEdit_Induction(HR_Training_Create_DTO dto)
        {
            long empid = 0;
            long counttrue = 0;
            var deviceidsnew = "";
            var devicenew = "";
            try
            {
                List<long> employeeids = new List<long>();
                if (dto.HRTCR_Id > 0)

                {
                    var result = _vmsconte.HR_Training_Create_DMO_con.Single(a => a.HRTCR_Id == dto.HRTCR_Id);
                    result.HRTCR_PrgogramName = dto.HRTCR_PrgogramName;
                    result.HRMD_Id = dto.HRMD_Id;
                    result.HRTCR_ProgramDesc = dto.HRTCR_ProgramDesc;
                    result.HRTCR_CostFeeFlg = dto.HRTCR_CostFeeFlg;
                    result.HRTCR_Cost = dto.HRTCR_Cost;
                    result.HRMB_Id = dto.HRMB_Id;
                    result.HRMF_Id = dto.HRMF_Id;
                    result.HRMR_Id = dto.HRMR_Id;
                    result.HRTCR_StartDate = dto.HRTCR_StartDate;
                    result.HRTCR_EndDate = dto.HRTCR_EndDate;
                    result.MI_Id = dto.MI_Id;
                    result.HRTCR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    result.HRTCR_InternalORExternalFlg = dto.HRTCR_InternalORExternalFlg;
                    _vmsconte.Update(result);

                    var dlt = _vmsconte.HR_Training_Create_Participants_DMO_con.Where(a => a.HRTCR_Id == dto.HRTCR_Id).ToList();
                    foreach (var item in dlt)
                    {
                        var dlt1 = _vmsconte.HR_Training_Create_Participants_DMO_con.Single(a => a.HRTCRP_Id == item.HRTCRP_Id);
                        _vmsconte.Remove(dlt1);
                    }
                    foreach (var it in dto.emplyee)
                    {

                        employeeids.Add(it.HRME_Id);
                        var getemployeedetails = (from a in _vmsconte.Hr_Master_Employee_con

                                                  where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == dto.MI_Id && a.HRME_Id == it.HRME_Id)
                                                  select new HR_Training_Create_DTO
                                                  {
                                                      employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                      + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),

                                                  }).Distinct().ToList().ToArray();
                        HR_Training_Create_Participants_DMO icd = new HR_Training_Create_Participants_DMO();
                        icd.HRME_Id = it.HRME_Id;
                        icd.HRTCR_Id = result.HRTCR_Id;
                        _vmsconte.Add(icd);

                    }
                    var devicelist = (from a in _vmsconte.Hr_Master_Employee_con
                                      where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                      select new HR_Training_Create_DTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToList();
                    dto.deviceArray = devicelist.ToArray();
                    if (devicelist.Count > 0)
                    {
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
                    }

                    try
                    {
                        if (dto.sound == "")
                        {
                            dto.sound = "default";
                        }
                        if (dto.CHECK_Notification == true)
                        {
                            callnotification(devicenew, employeeids, dto.MI_Id, dto.sound);
                            SendEmailTrainee(dto, "Training Notification", "Training Notification", dto.MI_Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }

                else
                {

                    HR_Training_Create_DMO tcd = new HR_Training_Create_DMO();
                    tcd.HRTCR_PrgogramName = dto.HRTCR_PrgogramName;
                    tcd.HRMD_Id = dto.HRMD_Id;
                    tcd.HRTCR_ProgramDesc = dto.HRTCR_ProgramDesc;
                    tcd.HRTCR_CostFeeFlg = dto.HRTCR_CostFeeFlg;
                    tcd.HRTCR_Cost = dto.HRTCR_Cost;
                    tcd.HRMB_Id = dto.HRMB_Id;
                    tcd.HRMF_Id = dto.HRMF_Id;
                    tcd.HRMR_Id = dto.HRMR_Id;
                    tcd.HRTCR_ActiveFlag = true;
                    tcd.HRTCR_StartDate = dto.HRTCR_StartDate;
                    tcd.HRTCR_EndDate = dto.HRTCR_EndDate;
                    tcd.MI_Id = dto.MI_Id;
                    tcd.HRTCR_CreatedBy = dto.userId;
                    tcd.CreatedDate = DateTime.Now;
                    tcd.HRTCR_InternalORExternalFlg = dto.HRTCR_InternalORExternalFlg;
                    _vmsconte.Add(tcd);


                    foreach (var it in dto.emplyee)
                    {
                        employeeids.Add(it.HRME_Id);
                        var getemployeedetails = (from a in _vmsconte.Hr_Master_Employee_con

                                                  where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == dto.MI_Id && a.HRME_Id == it.HRME_Id)
                                                  select new HR_Training_Create_DTO
                                                  {
                                                      employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                      + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),

                                                  }).Distinct().ToList().ToArray();

                        HR_Training_Create_Participants_DMO icd = new HR_Training_Create_Participants_DMO();
                        icd.HRME_Id = it.HRME_Id;
                        icd.HRTCR_Id = tcd.HRTCR_Id;
                        _vmsconte.Add(icd);



                    }


                    var devicelist = (from a in _vmsconte.Hr_Master_Employee_con
                                      where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                      select new HR_Training_Create_DTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToList();
                    dto.deviceArray = devicelist.ToArray();
                    if (devicelist.Count > 0)
                    {
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
                    }

                    try
                    {
                        if (dto.sound == "")
                        {
                            dto.sound = "default";
                        }
                        if (dto.CHECK_Notification == true)
                        {
                            callnotification(devicenew, employeeids, dto.MI_Id, dto.sound);
                            SendEmailTrainee(dto, "Training Notification", "Training Notification", dto.MI_Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public string callnotification(string devicenew, List<long> empids, long mi_id, string sound)
        {
            long AMST_Id = 0;
            try
            {
                HR_Training_Create_DTO data = new HR_Training_Create_DTO();
                var employees = (from a in _vmsconte.Hr_Master_Employee_con
                                 where (a.HRME_ActiveFlag == true && empids.Contains(a.HRME_Id) && a.MI_Id == mi_id)
                                 select new HR_Training_Create_DTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                     HRME_MobileNo=a.HRME_MobileNo,
                                     HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                 }).Distinct().ToList().ToArray();

                foreach (var nt in employees)
                {
                    string url = "";
                    url = "https://fcm.googleapis.com/fcm/send";

                    List<string> notificationparams = new List<string>();
                    string daata = "";
                    long notId = 1;
                    string message = "You are Select Induction Training";


                    daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + nt.HRME_EmployeeFirstName + '"' + " , " + '"' + "body" + '"' + ":" + '"' + message + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";


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
                     
                    //
                    PushNotification push_noti = new PushNotification(_db);
                    
                    push_noti.Insert_PushNotification(mi_id, nt.HRME_EmployeeFirstName, message, nt.HRME_MobileNo,nt.HRME_AppDownloadedDeviceId,nt.HRME_Id, AMST_Id);


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

            return "success";
        }
        public HR_Training_Create_DTO SaveEdit_training_details(HR_Training_Create_DTO dto)
        {
            try
            {
                var ldate = "";
                foreach (var cd1 in dto.trainingdetails)
                {                  
                   ldate = cd1.HRTCRINTTR_StartDate.ToString();
                }

                var result = _vmsconte.HR_Training_Create_DMO_con.Single(a => a.HRTCR_Id == dto.HRTCR_Id && a.MI_Id == dto.MI_Id);
                if (result.HRTCR_InternalORExternalFlg == false)
                {
                    var result1 = _vmsconte.HR_Training_Create_IntTrainer_DMO_con.Where(a => a.HRTCR_Id == dto.HRTCR_Id).ToList();
                    foreach (var item in result1)
                    {
                        var res = _vmsconte.HR_Training_Create_IntTrainer_DMO_con.Single(a => a.HRTCRINTTR_Id == item.HRTCRINTTR_Id);
                        _vmsconte.Remove(res);
                    }
                    foreach (var cd in dto.trainingdetails)
                    {
                        HR_Training_Create_IntTrainer_DMO tci = new HR_Training_Create_IntTrainer_DMO();

                        tci.HRTCR_Id = dto.HRTCR_Id;
                        tci.HRME_Id = cd.HRME_Id;
                        tci.HRTCRINTTR_TrainingDesc = cd.HRTCRD_Content;
                        tci.HRTCRINTTR_StartTime = cd.HRTCRD_StartTime;
                        tci.HRTCRINTTR_EndTime = cd.HRTCRD_EndTime;
                        tci.HRTCRINTTR_StartDate = cd.HRTCRINTTR_StartDate;
                        tci.HRTCRINTTR_EndDate =Convert.ToDateTime(ldate);
                        tci.HRMTT_Id = cd.HRMTT_Id;
                        tci.HRTCRINTTR_ActiveFlg = true;
                        _vmsconte.HR_Training_Create_IntTrainer_DMO_con.Add(tci);
                    }
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
                else
                {
                    var result1 = _vmsconte.HR_Training_Create_ExtTrainer_DMO_con.Where(a => a.HRTCR_Id == dto.HRTCR_Id).ToList();
                    foreach (var item in result1)
                    {
                        var res = _vmsconte.HR_Training_Create_ExtTrainer_DMO_con.Single(a => a.HRTCREXTTR_Id == item.HRTCREXTTR_Id);
                        _vmsconte.Remove(res);
                    }
                    foreach (var cd in dto.trainingdetails)
                    {
                        HR_Training_Create_ExtTrainer_DMO tci = new HR_Training_Create_ExtTrainer_DMO();
                        tci.HRTCR_Id = dto.HRTCR_Id;
                        tci.HRME_Id = cd.HRME_Id;
                        tci.HRTCREXTTR_TrainingDesc = cd.HRTCRD_Content;
                        tci.HRTCREXTTR_StartTime = cd.HRTCRD_StartTime;
                        tci.HRTCREXTTR_EndTime = cd.HRTCRD_EndTime;
                        tci.HRTCREXTTR_StartDate = cd.HRTCRINTTR_StartDate;
                        tci.HRTCREXTTR_EndDate = Convert.ToDateTime(ldate);
                        tci.HRMTT_Id = cd.HRMTT_Id;
                        tci.HRTCREXTTR_ActiveFlg = true;
                        _vmsconte.HR_Training_Create_ExtTrainer_DMO_con.Add(tci);
                    }

                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }

                if(dto.Notification == true)
                {
                    SendEmailTrainer(dto, "Training Notification", "Training Notification", dto.MI_Id);
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO SaveEvalution_trinee_rating(HR_Training_Create_DTO dto)
        {
            try
            {
                foreach (var it in dto.trainingdetails)
                {

                    var result = _vmsconte.HR_Training_Create_DMO_con.Where(a => a.HRTCR_Id == it.HRTCR_Id).ToList();

                    {

                        HR_Training_Status_DMO ci = new HR_Training_Status_DMO();
                        ci.HRTCR_Id = it.HRTCR_Id;
                        ci.MI_Id = dto.MI_Id;
                        if (result[0].HRTCR_InternalORExternalFlg == false)
                        {
                            ci.HRTSTS_IntORExtFlg = "0";
                        }
                        else
                        {
                            ci.HRTSTS_IntORExtFlg = "1";
                        }
                        ci.HRTSTS_TrainerId = dto.HRMET_Id;
                        ci.HRME_Id = it.HRME_Id;
                        ci.HRTSTS_Rating = it.HRTCRD_Rating;
                        ci.HRTSTS_TrainerRemarks = it.HRTCRD_TrainerRemarks;
                        _vmsconte.HR_Training_Status_DMO_con.Add(ci);

                        if (result[0].HRTCR_InternalORExternalFlg == false)
                        {
                            var rest = _vmsconte.HR_Training_Create_IntTrainer_DMO_con.Single(a => a.HRME_Id == dto.HRMET_Id && a.HRTCR_Id == it.HRTCR_Id);
                            rest.HRTCRINTTR_Rating = 1;
                            _vmsconte.Update(rest);
                        }
                        else
                        {
                            var rest = _vmsconte.HR_Training_Create_ExtTrainer_DMO_con.Single(a => a.HRME_Id == dto.HRMET_Id && a.HRTCR_Id==it.HRTCR_Id);
                            rest.HRTCREXTTR_Rating = 1;
                            _vmsconte.Update(rest);
                        }
                    }


                }
                _vmsconte.SaveChanges();
                dto.returnvalue = "Update";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO Training_Views(HR_Training_Create_DTO dto)
        {

            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "training_view_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.n_id)
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
                        dto.induction_view_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "induction_view_list_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.n_id)
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
                        dto.induction_view_list_details = retObject.ToArray();
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
            return dto;
        }
        public HR_Training_Create_DTO EveGet(HR_Training_Create_DTO dto)
        {
            try
            {

                dto.evaluation_participants_list = (
                                               from a in _vmsconte.Hr_Master_Employee_con
                                               from c in _vmsconte.HR_Training_Create_Participants_DMO_con
                                               where (c.HRTCR_Id == dto.HRTCR_Id && c.HRME_Id == a.HRME_Id)
                                               select new HR_Training_Create_DTO
                                               {
                                                   HRME_Id = c.HRME_Id,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),

                                                   HRTCR_Id = c.HRTCR_Id

                                               }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToList().ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "evaluation_training_list";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRTCR_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.userId)
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
                        dto.evaluation_trainer_list = retObject.ToArray();
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

            return dto;
        }
        public HR_Training_Create_DTO update_status(HR_Training_Create_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Training_Create_DMO_con.Single(a => a.HRTCR_Id == dto.HRTCR_Id);
                if (dto.Status == 2)
                {
                    result.HRTCR_StatusFlg = Convert.ToInt32(dto.Status);
                    result.HRTCR_ActiveFlag = true;
                    _vmsconte.Update(result);

                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else if (dto.Status == 0)
                {
                    result.HRTCR_StatusFlg = Convert.ToInt32(dto.Status);
                    result.HRTCR_ActiveFlag = false;
                    _vmsconte.Update(result);

                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    result.HRTCR_StatusFlg = Convert.ToInt32(dto.Status);
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO edit_induction_create(HR_Training_Create_DTO dto)
        {
            try
            {

                dto.Training_create_Details = (from a in _vmsconte.HR_Training_Create_DMO_con
                                               from b in _vmsconte.HR_Master_Building_con
                                               from c in _vmsconte.HR_Master_Floor_con
                                               from d in _vmsconte.HR_Master_Room_con
                                               from e in _vmsconte.HR_Master_Department
                                               where (a.HRTCR_Id == dto.HRTCR_Id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id && a.HRMD_Id == e.HRMD_Id && e.MI_Id == dto.MI_Id)
                                               select new HR_Training_Create_DTO
                                               {
                                                   HRTCR_Id = a.HRTCR_Id,
                                                   HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                                   HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                   HRMD_Id = a.HRMD_Id,
                                                   HRTCR_ProgramDesc = a.HRTCR_ProgramDesc,
                                                   HRTCR_CostFeeFlg = a.HRTCR_CostFeeFlg,
                                                   HRTCR_Cost = a.HRTCR_Cost,
                                                   HRMB_Id = a.HRMB_Id,
                                                   HRMB_BuildingName = b.HRMB_BuildingName,
                                                   HRMF_Id = a.HRMF_Id,
                                                   HRMF_FloorName = c.HRMF_FloorName,
                                                   HRMR_Id = a.HRMR_Id,
                                                   HRMR_RoomName = d.HRMR_RoomName,
                                                   HRTCR_InternalORExternalFlg = a.HRTCR_InternalORExternalFlg,
                                                   HRTCR_StartDate = Convert.ToDateTime(a.HRTCR_StartDate),
                                                   HRTCR_EndDate = Convert.ToDateTime(a.HRTCR_EndDate)

                                               }).ToList().ToArray();

                dto.Training_create_Trainee_list = (
                                                    from b in _vmsconte.HR_Training_Create_Participants_DMO_con
                                                    from a in _vmsconte.Hr_Master_Employee_con
                                                    where (a.MI_Id == dto.MI_Id && b.HRTCR_Id == dto.HRTCR_Id && b.HRME_Id == a.HRME_Id)
                                                    select new HR_Training_Create_DTO
                                                    {
                                                        HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),

                                                        HRME_Id = b.HRME_Id

                                                    }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToList().ToArray();
                dto.participates_Employee_list = _dsc.HR_Master_Employee_DMO.Where(a => a.HRMD_Id == dto.HRMD_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO edit_training_details(HR_Training_Create_DTO dto)
        {
            try
            {
                var tt= _vmsconte.Institution.Where(a=>a.MI_ActiveFlag==1).ToList();
                
                 List<long> reslt = new List<long>();
                foreach (var it in tt)
                {
                    reslt.Add(it.MI_Id);     
                    }

             

                    var check = _vmsconte.HR_Training_Create_DMO_con.Single(a => a.HRTCR_Id == dto.HRTCR_Id);
                if (check.HRTCR_InternalORExternalFlg == true)
                {
                    dto.Training_create_Details_list = (
                                                        from b in _vmsconte.HR_Training_Create_DMO_con
                                                        from c in _vmsconte.HR_Master_External_Trainer_Creation_DMO_con
                                                        from d in _vmsconte.HR_Training_Create_ExtTrainer_DMO_con
                                                        where (c.MI_Id == dto.MI_Id && b.HRTCR_Id == dto.HRTCR_Id && d.HRME_Id == c.HRMETR_Id && d.HRTCR_Id == b.HRTCR_Id)
                                                        select new HR_Training_Create_DTO
                                                        {
                                                            HRME_EmployeeFirstName = c.HRMETR_Name,
                                                            HRME_Id = d.HRME_Id,
                                                            HRTCRINTTR_StartDate = Convert.ToDateTime(d.HRTCREXTTR_StartDate),
                                                            HRTCRD_StartTime = d.HRTCREXTTR_StartTime,
                                                            HRTCRD_EndTime = d.HRTCREXTTR_EndTime,
                                                            HRTCRD_Content = d.HRTCREXTTR_TrainingDesc,
                                                            HRTCR_Id = b.HRTCR_Id,
                                                            HRTCR_PrgogramName = b.HRTCR_PrgogramName
                                                        }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToList().ToArray();
                       
                    dto.trinee_list = (from a in _vmsconte.HR_Master_External_Trainer_Creation_DMO_con
                                       where (reslt.Contains(a.MI_Id))
                                       select new HR_Master_External_Trainer_Creation_DTO
                                       {
                                           HRME_Id = a.HRMETR_Id,
                                           HRME_EmployeeFirstName = a.HRMETR_Name
                                       }).Distinct().ToArray();

                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "program_dd_list_proc";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
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
                            dto.program_dd_list = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                    dto.program_dd_list_one =(from a in _vmsconte.HR_Training_Create_DMO_con
                                              where (a.MI_Id == dto.MI_Id && a.HRTCR_Id == dto.HRTCR_Id)
                                              select new HR_Training_Create_DTO
                                              {
                                                  HRTCR_PrgogramName=a.HRTCR_PrgogramName,
                                                  HRTCR_Id=a.HRTCR_Id
                                              }).ToList().ToArray();

                }
                else
                {
                    dto.Training_create_Details_list = (
                                                        from b in _vmsconte.HR_Training_Create_DMO_con
                                                        from a in _vmsconte.Hr_Master_Employee_con
                                                        from d in _vmsconte.HR_Training_Create_IntTrainer_DMO_con
                                                        where ( b.HRTCR_Id == dto.HRTCR_Id && d.HRME_Id == a.HRME_Id && d.HRTCR_Id == b.HRTCR_Id)
                                                        select new HR_Training_Create_DTO
                                                        {
                                                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),

                                                            HRME_Id = a.HRME_Id,
                                                            HRTCRINTTR_StartDate = Convert.ToDateTime(d.HRTCRINTTR_StartDate),
                                                            HRTCRD_StartTime = d.HRTCRINTTR_StartTime,
                                                            HRTCRD_EndTime = d.HRTCRINTTR_EndTime,
                                                            HRTCRD_Content = d.HRTCRINTTR_TrainingDesc,
                                                            HRTCR_Id = b.HRTCR_Id,
                                                            HRTCR_PrgogramName = b.HRTCR_PrgogramName
                                                        }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToList().ToArray();

                    //dto.trinee_list = _vmsconte.Hr_Master_Employee_con.Where(a => a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false).Distinct().ToArray();
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "participates_Employee_list_proc";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
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
                            dto.trinee_list = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "program_dd_list_proc";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(dto.MI_Id)
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
                            dto.program_dd_list = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    dto.program_dd_list_one = (from a in _vmsconte.HR_Training_Create_DMO_con
                                               where ( a.HRTCR_Id == dto.HRTCR_Id)
                                               select new HR_Training_Create_DTO
                                               {
                                                   HRTCR_PrgogramName = a.HRTCR_PrgogramName,
                                                   HRTCR_Id = a.HRTCR_Id
                                               }).ToList().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO deactivate_Induction_create(HR_Training_Create_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Training_Create_DMO_con.Single(a => a.HRTCR_Id == dto.HRTCR_Id);
                if (dto.HRTCR_ActiveFlag == true)
                {
                    result.HRTCR_ActiveFlag = false;
                    _vmsconte.Update(result);
                    dto.HRTCR_ActiveFlag = false;
                }
                else
                {
                    result.HRTCR_ActiveFlag = true;
                    _vmsconte.Update(result);
                    dto.HRTCR_ActiveFlag = true;
                }
                _vmsconte.SaveChanges();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Training_Create_DTO GetInductionReport(HR_Training_Create_DTO dto)
        {
            dto.instlogo = _vmsconte.Institution.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.MI_Logo).FirstOrDefault();
            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Induction_Program_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.MI_Id)
                });

                cmd.Parameters.Add(new SqlParameter("@ALL", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.ALL)
                });
                cmd.Parameters.Add(new SqlParameter("@OPEN", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.OPEN)
                });

                cmd.Parameters.Add(new SqlParameter("@RUNNING", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.RUNNING)
                });

                cmd.Parameters.Add(new SqlParameter("@COMPLETE", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.COMPLETE)
                });

                cmd.Parameters.Add(new SqlParameter("@START_DATE", SqlDbType.DateTime)
                {
                    Value = dto.START_DATE
                });

                cmd.Parameters.Add(new SqlParameter("@END_DATE", SqlDbType.DateTime)
                {
                    Value = dto.END_DATE
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
                    dto.induction_training_report = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return dto;
            }
        }
        public HR_Training_Create_DTO print_trainer_list(HR_Training_Create_DTO dto)
        {

            using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "print_induction_trainer_list";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.MI_Id)
                });

                cmd.Parameters.Add(new SqlParameter("@HRTCR_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(dto.HRTCR_Id)
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
                    dto.print_trainer_list = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return dto;
            }
        }
        public void SendEmailTrainee(HR_Training_Create_DTO obj, string subject, string body, long id)
        {
            try
            {
                string buildingname = _vmsconte.HR_Master_Building_con.Where(t=>t.HRMB_Id == obj.HRMB_Id).Select(t=>t.HRMB_BuildingName).FirstOrDefault();
                string floorname = _vmsconte.HR_Master_Floor_con.Where(t => t.HRMF_Id == obj.HRMF_Id).Select(t => t.HRMF_FloorName).FirstOrDefault();
                string roomname = _vmsconte.HR_Master_Room_con.Where(t => t.HRMR_Id == obj.HRMR_Id).Select(t => t.HRMR_RoomName).FirstOrDefault();

                for (int j = 0; j < obj.emplyee.Length; j++)
                {
                    string traineemailid = (from a in _vmsconte.HR_Master_Employee_DMO
                                            from b in _vmsconte.Multiple_Email_DMO
                                            where (a.HRME_Id == b.HRME_Id && a.HRME_Id == obj.emplyee[j].HRME_Id && b.HRMEM_DeFaultFlag == "default")
                                            select b.HRMEM_EmailId).FirstOrDefault();

                    var trainername = _vmsconte.HR_Master_Employee_DMO.Where(t => t.HRME_Id == obj.emplyee[j].HRME_Id).Select(t => new {t.HRME_EmployeeFirstName,t.HRME_EmployeeMiddleName,t.HRME_EmployeeLastName }).FirstOrDefault();

                    string mailid = traineemailid;
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _dsc.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Trainee_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                    var institutionName = _dsc.Institution.Where(i => i.MI_Id == id).ToList();

                    string Mailmsg = body;
                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _dsc.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        string mailcc = alldetails[0].IVRM_mailcc;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string Subject = subject;
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

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

                        message.AddTo(mailid);

                        if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                        {
                            message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                            message.HtmlContent = message.HtmlContent.Replace("[TRAINEE NAME]", (trainername.HRME_EmployeeFirstName + " " + (trainername.HRME_EmployeeMiddleName == null ? "": trainername.HRME_EmployeeMiddleName) + " " + (trainername.HRME_EmployeeLastName == null ? "": trainername.HRME_EmployeeLastName)));
                            message.HtmlContent = message.HtmlContent.Replace("[DATEFROM]", obj.HRTCR_StartDate.ToString());
                            message.HtmlContent = message.HtmlContent.Replace("[DATETO]", obj.HRTCR_EndDate.ToString());
                            message.HtmlContent = message.HtmlContent.Replace("[VENUE]", buildingname);
                            message.HtmlContent = message.HtmlContent.Replace("[FLOOR]", floorname);
                            message.HtmlContent = message.HtmlContent.Replace("[ROOM]", roomname);
                        }
                        else
                        {
                            message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                        }

                        if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                        {
                            var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                            client.SendEmailAsync(message).Wait();
                        }
                        else
                        {
                            // return "Sendgrid key is not available";
                        }

                        using (var cmd = _dsc.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _dsc.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Trainee_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _dsc.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _dsc.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing_1";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                            {
                                Value = mailid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                            {
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                            {
                                Value = subject
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                            {
                                Value = "Staff"
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
                                //return ex.Message;
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
        public void SendEmailTrainer(HR_Training_Create_DTO obj, string subject, string body, long id)
        {
            try
            {
                var programdata = (from a in _vmsconte.HR_Training_Create_DMO_con
                                   from b in _vmsconte.HR_Master_Building_con
                                   from c in _vmsconte.HR_Master_Floor_con
                                   from d in _vmsconte.HR_Master_Room_con
                                   where (a.HRTCR_Id == obj.HRTCR_Id && a.MI_Id == id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMR_Id == d.HRMR_Id)
                                   select new HR_Training_Create_DTO
                                   {
                                       HRMB_BuildingName = b.HRMB_BuildingName,
                                       HRMF_FloorName = c.HRMF_FloorName,
                                       HRMR_RoomName = d.HRMR_RoomName
                                   }).FirstOrDefault();

                for (int j = 0; j < obj.trainingdetails.Length; j++)
                {
                    string trainermailid = (from a in _vmsconte.HR_Master_Employee_DMO
                                            from b in _vmsconte.Multiple_Email_DMO
                                            where (a.HRME_Id == b.HRME_Id && a.HRME_Id == obj.trainingdetails[j].HRME_Id && b.HRMEM_DeFaultFlag == "default")
                                            select b.HRMEM_EmailId).FirstOrDefault();

                    string triningcontent = _vmsconte.HR_Master_TrainingTopicDMO.Where(t => t.HRMTT_Id == obj.trainingdetails[j].HRMTT_Id).Select(t => t.HRMTT_Topic).FirstOrDefault();

                    var trainername = _vmsconte.HR_Master_Employee_DMO.Where(t => t.HRME_Id == obj.trainingdetails[j].HRME_Id).Select(t => new { t.HRME_EmployeeFirstName, t.HRME_EmployeeMiddleName, t.HRME_EmployeeLastName }).FirstOrDefault();

                    string mailid = trainermailid;
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _dsc.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Trainer_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                    var institutionName = _dsc.Institution.Where(i => i.MI_Id == id).ToList();

                    string Mailmsg = body;
                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _dsc.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        string mailcc = alldetails[0].IVRM_mailcc;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string Subject = subject;
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

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

                        message.AddTo(mailid);

                        if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                        {
                            message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                            message.HtmlContent = message.HtmlContent.Replace("[TRAINER NAME]", (trainername.HRME_EmployeeFirstName + " " + (trainername.HRME_EmployeeMiddleName == null ? "" : trainername.HRME_EmployeeMiddleName) + " " + (trainername.HRME_EmployeeLastName == null ? "" : trainername.HRME_EmployeeLastName)));
                            message.HtmlContent = message.HtmlContent.Replace("[TOPIC]", triningcontent);
                            message.HtmlContent = message.HtmlContent.Replace("[DATE]", Convert.ToDateTime(obj.trainingdetails[j].HRTCRINTTR_StartDate).ToShortDateString());
                            message.HtmlContent = message.HtmlContent.Replace("[TIMEFROM]", obj.trainingdetails[j].HRTCRD_StartTime.ToString());
                            message.HtmlContent = message.HtmlContent.Replace("[TIMETO]", obj.trainingdetails[j].HRTCRD_EndTime.ToString());
                            message.HtmlContent = message.HtmlContent.Replace("[VENUE]", programdata.HRMB_BuildingName + " " + programdata.HRMF_FloorName + " " + programdata.HRMR_RoomName);
                        }
                        else
                        {
                            message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                        }

                        if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                        {
                            var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                            client.SendEmailAsync(message).Wait();
                        }
                        else
                        {
                            // return "Sendgrid key is not available";
                        }

                        using (var cmd = _dsc.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _dsc.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Trainer_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _dsc.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _dsc.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing_1";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                            {
                                Value = mailid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                            {
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                            {
                                Value = subject
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                            {
                                Value = "Staff"
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
                                //return ex.Message;
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
    }
}