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
using System.Net;
using System.Net.NetworkInformation;
using DomainModel.Model.com.vapstech.Portals.Employee;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace WebApplication1.Services
{
    public class OralTestScheduleImp : Interfaces.OralTestScheduleInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
        new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly OralTestScheduleContext _OralTestScheduleContext;
        private readonly DomainModelMsSqlServerContext _context;
        public StudentApplicationContext _StudentApplicationContext;
        public FeeGroupContext _feecontext;
        public OralTestScheduleImp(StudentApplicationContext StudentApplicationContext, OralTestScheduleContext OralTestScheduleContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext)
        {
            _OralTestScheduleContext = OralTestScheduleContext;
            _context = db;
            _StudentApplicationContext = StudentApplicationContext;
            _feecontext = feecontext;
        }
        public StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO StudentDetailsDTO)//int IVRMM_Id
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                StudentDetailsDTO.classlist = allclass.ToArray();

                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).OrderBy(t => t.PAOTS_ScheduleDate).ToList();
                StudentDetailsDTO.OralTestSchedule = Allname.ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_VC_Orl_Schedules";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.MI_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        StudentDetailsDTO.VcOralTestSchedule = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Orl_Schedules_Count";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.ASMAY_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        StudentDetailsDTO.OralTestSchedulecount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Orl_Schedules_Overall";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.ASMAY_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        StudentDetailsDTO.overallOralTestSchedulecount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                StudentDetailsDTO.stafflist = (from a in _context.Staff_User_Login
                                               from b in _context.HR_Master_Employee_DMO
                                               where a.MI_Id == b.MI_Id && a.Emp_Code == b.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1 && b.HRME_ActiveFlag == true
                                               && a.MI_Id == StudentDetailsDTO.MI_Id && a.Id != StudentDetailsDTO.Id
                                               select new StudentDetailsDTO
                                               {
                                                   userid = a.Id,
                                                   HRME_Id = b.HRME_Id,
                                                   HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                               }).Distinct().OrderBy(r => r.HRME_EmployeeFirstName).ToArray();
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
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            OralTestScheduleDTO OralTestScheduleDTO = new OralTestScheduleDTO();
            List<OralTestScheduleStudentInsertDMO> masters = new List<OralTestScheduleStudentInsertDMO>();
            masters = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();

            List<OralTestScheduleDMO> schedule = new List<OralTestScheduleDMO>();
            schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(ID)).ToList();
            if (masters.Any())
            {
                try
                {
                    int j = 0;
                    while (j < masters.Count())
                    {
                        List<OralTestScheduleStudentInsertDMO> masters3 = new List<OralTestScheduleStudentInsertDMO>();
                        masters3 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(masters[j].PAOTS_Id)).ToList();
                        _OralTestScheduleContext.Remove(masters3.ElementAt(0));
                        int deletedata = _OralTestScheduleContext.SaveChanges();                        

                        List<LMS_Live_MeetingDMO> livemeet = new List<LMS_Live_MeetingDMO>();
                        livemeet = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_PlannedDate == schedule.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == masters[j].PAOTSS_Time && t.LMSLMEET_PlannedEndTime == masters[j].PAOTSS_Time_To).ToList();
                        if (livemeet.Count > 0)
                        {
                            List<LMS_Live_Meeting_PAStudentDMO> livemeetstudentdelete = new List<LMS_Live_Meeting_PAStudentDMO>();
                            livemeetstudentdelete = _context.LMS_Live_Meeting_PAStudentDMO.Where(t => t.PASR_Id.Equals(masters[j].PASR_Id) && t.LMSLMEET_Id == livemeet.FirstOrDefault().LMSLMEET_Id).ToList();
                            _context.Remove(livemeetstudentdelete.ElementAt(0));
                            int deletestudent = _context.SaveChanges();

                            List<LMS_Live_Meeting_StaffOthersDMO> livemeetstaffdelete = new List<LMS_Live_Meeting_StaffOthersDMO>();
                            livemeetstaffdelete = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == livemeet.FirstOrDefault().LMSLMEET_Id).ToList();
                            if (livemeetstaffdelete.Count > 0)
                            {
                                for (int i = 0; i < livemeetstaffdelete.Count(); i++)
                                {
                                    _context.Remove(livemeetstaffdelete.ElementAt(i));
                                    _context.SaveChanges();
                                }
                            }
                        }

                        List<OralTestScheduleStudentInsertDMO> livemeetstatus = new List<OralTestScheduleStudentInsertDMO>();
                        livemeetstatus = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PASR_Id.Equals(masters[j].PASR_Id)).OrderByDescending(t => t.PAOTSS_Id).ToList();
                        if (livemeetstatus.Count > 0)
                        {
                            var result = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Single(t => t.PAOTS_Id == livemeetstatus.FirstOrDefault().PAOTS_Id && t.PASR_Id.Equals(masters[j].PASR_Id));
                            result.UpdatedDate = indianTime;
                            result.PAOTSS_StatusFlag = 1;
                            _OralTestScheduleContext.Update(result);
                            _OralTestScheduleContext.SaveChanges();
                        }

                        List<LMS_Live_MeetingDMO> livemeetdelete = new List<LMS_Live_MeetingDMO>();
                        livemeetdelete = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_PlannedDate == schedule.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == masters[j].PAOTSS_Time && t.LMSLMEET_PlannedEndTime == masters[j].PAOTSS_Time_To).ToList();
                        if (livemeetdelete.Any())
                        {
                            _context.Remove(livemeetdelete.ElementAt(0));
                            _context.SaveChanges();
                        }


                        if (deletedata > 0)
                        {
                            var student = _StudentApplicationContext.Enq.Where(d => d.pasr_id.Equals(masters[j].PASR_Id)).ToList();

                            Email Email = new Email(_context);
                            string m = Email.sendmail(student.FirstOrDefault().MI_Id, student.FirstOrDefault().PASR_emailId, "DELETE_ORAL_TEST_SCHEDULE", masters[j].PASR_Id);

                            SMS sms = new SMS(_context);
                            sms.sendSms(student.FirstOrDefault().MI_Id, student.FirstOrDefault().PASR_MobileNo, "DELETE_ORAL_TEST_SCHEDULE", masters[j].PASR_Id);
                        }
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
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
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
                _OralTestScheduleContext.Remove(masters.ElementAt(0));
                _OralTestScheduleContext.SaveChanges();
            }
            else
            {

            }
            return OralTestScheduleDTO;
        }
        public StudentDetailsDTO classwisestudent(StudentDetailsDTO StudentDetailsDTO)
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                StudentDetailsDTO.ASMAY_Id = Acdemic_preadmission;


                List<StudentApplication> lorg2 = new List<StudentApplication>();
                List<StudentApplication> lorg5 = new List<StudentApplication>();
                List<StudentApplication> lorgfinal = new List<StudentApplication>();
                Array[] showdata1 = new Array[1];

                List<OralTestScheduleStudentInsertDMO> lorg1 = new List<OralTestScheduleStudentInsertDMO>();
                lorg1 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.ToList();
                List<long> Allname2 = new List<long>();
                Allname2 = lorg1.Select(t => t.PASR_Id).ToList();

                List<StudentApplication> Allname1 = new List<StudentApplication>();

                List<long> studentlistof = new List<long>();

                List<StudentApplication> Allname5 = new List<StudentApplication>();
                if (StudentDetailsDTO.ASMCL_ID != 0)
                {
                    lorg5 = _OralTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id).ToList();
                }
                else
                {
                    lorg5 = _OralTestScheduleContext.StudentApplication.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMAY_Id == StudentDetailsDTO.ASMAY_Id).ToList();
                }


                studentlistof = lorg5.Select(t => t.pasr_id).ToList();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                StudentDetailsDTO.classlist = allclass.ToArray();


                List<long> Allname3 = new List<long>();
                Allname5 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && t.PASRAPS_ID == 787928).ToList();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.MI_Id) && d.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                {

                    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                    {
                        //foreach (var a in lorg5)
                        //{
                        Allname3 = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" &&
                        studentlistof.Contains(t.PASA_Id)).Select(t => t.PASA_Id).ToList();

                        //if (StudentDetailsDTO.payementcheck == 1)
                        //{

                        //    Allname3.Add(a.pasr_id);

                        //}
                        //}

                        //foreach (var a in lorg1)
                        //{
                        //    Allname2.Add(a.PASR_Id);
                        //}

                        if (StudentDetailsDTO.ASMCL_ID != 0)

                        {
                            Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)
                            && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID).ToList();

                            StudentDetailsDTO.studentDetails = Allname1.ToArray();
                        }
                        else
                        {
                            Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && Allname3.Contains(t.pasr_id)).ToList();

                            StudentDetailsDTO.studentDetails = Allname1.ToArray();
                        }


                    }
                }
                else
                {
                    //foreach (var a in lorg1)
                    //{
                    //    Allname2.Add(a.PASR_Id);
                    //}
                    if (StudentDetailsDTO.ASMCL_ID != 0)
                    {
                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id) && t.ASMCL_Id == StudentDetailsDTO.ASMCL_ID).ToList();
                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }
                    else
                    {
                        Allname1 = _OralTestScheduleContext.StudentApplication.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id) && t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.PASR_Adm_Confirm_Flag == false && !Allname2.Contains(t.pasr_id)).ToList();

                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }


                }

                Array[] showdata = new Array[50];
                List<OralTestScheduleDMO> Allname = new List<OralTestScheduleDMO>();
                Allname = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.MI_Id) && t.ASMAY_Id.Equals(StudentDetailsDTO.ASMAY_Id)).ToList();
                StudentDetailsDTO.OralTestSchedule = Allname.ToArray();

                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == StudentDetailsDTO.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                StudentDetailsDTO.admissioncatdrpall = allclass.ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_VC_Orl_Schedules";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.MI_Id
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                       dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                   );
                                    }
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        StudentDetailsDTO.VcOralTestSchedule = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Orl_Schedules_Count";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.ASMAY_Id
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                       dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                   );
                                    }
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        StudentDetailsDTO.OralTestSchedulecount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Orl_Schedules_Overall";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.ASMAY_Id
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                       dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                   );
                                    }
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        StudentDetailsDTO.overallOralTestSchedulecount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return StudentDetailsDTO;
        }
        public OralTestScheduleDTO removestudents(OralTestScheduleDTO data)
        {
            try
            {
                if (data.DeleteStudentData.Count > 0)
                {
                    List<OralTestScheduleDMO> schedule = new List<OralTestScheduleDMO>();
                    schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(data.DeleteStudentData.FirstOrDefault().PAOTS_Id)).ToList();

                    int j = 0;
                    while (j < data.DeleteStudentData.Count())
                    {
                        List<OralTestScheduleStudentInsertDMO> masters3 = new List<OralTestScheduleStudentInsertDMO>();
                        masters3 = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTSS_Id.Equals(data.DeleteStudentData[j].PAOTSS_Id)).ToList();
                        _OralTestScheduleContext.Remove(masters3.ElementAt(0));
                        int deletedata = _OralTestScheduleContext.SaveChanges();

                        List<LMS_Live_MeetingDMO> livemeet = new List<LMS_Live_MeetingDMO>();
                        livemeet = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_PlannedDate == schedule.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == masters3[0].PAOTSS_Time && t.LMSLMEET_PlannedEndTime == masters3[0].PAOTSS_Time_To).ToList();
                        if (livemeet.Count > 0)
                        {
                            List<LMS_Live_Meeting_PAStudentDMO> livemeetstudentdelete = new List<LMS_Live_Meeting_PAStudentDMO>();
                            livemeetstudentdelete = _context.LMS_Live_Meeting_PAStudentDMO.Where(t => t.PASR_Id.Equals(masters3[0].PASR_Id) && t.LMSLMEET_Id == livemeet.FirstOrDefault().LMSLMEET_Id).ToList();
                            _context.Remove(livemeetstudentdelete.ElementAt(0));
                            int deletestudent = _context.SaveChanges();

                            List<LMS_Live_Meeting_StaffOthersDMO> livemeetstaffdelete = new List<LMS_Live_Meeting_StaffOthersDMO>();
                            livemeetstaffdelete = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == livemeet.FirstOrDefault().LMSLMEET_Id).ToList();
                            if (livemeetstaffdelete.Count > 0)
                            {
                                for (int i = 0; i < livemeetstaffdelete.Count(); i++)
                                {
                                    _context.Remove(livemeetstaffdelete.ElementAt(i));
                                    _context.SaveChanges();
                                }
                            }
                        }

                        List<LMS_Live_MeetingDMO> livemeetdelete = new List<LMS_Live_MeetingDMO>();
                        livemeetdelete = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_PlannedDate == schedule.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == masters3[0].PAOTSS_Time && t.LMSLMEET_PlannedEndTime == masters3[0].PAOTSS_Time_To).ToList();
                        if (livemeetdelete.Any())
                        {
                            _context.Remove(livemeetdelete.ElementAt(0));
                            _context.SaveChanges();
                        }

                        if (deletedata > 0)
                        {
                            var student = _StudentApplicationContext.Enq.Where(d => d.pasr_id.Equals(masters3[0].PASR_Id)).ToList();

                            Email Email = new Email(_context);
                            string m = Email.sendmail(student.FirstOrDefault().MI_Id, student.FirstOrDefault().PASR_emailId, "DELETE_ORAL_TEST_SCHEDULE", masters3[0].PASR_Id);

                            SMS sms = new SMS(_context);
                            sms.sendSms(student.FirstOrDefault().MI_Id, student.FirstOrDefault().PASR_MobileNo, "DELETE_ORAL_TEST_SCHEDULE", masters3[0].PASR_Id);
                        }
                        j++;
                    }

                    List<OralTestScheduleStudentInsertDMO> delete = new List<OralTestScheduleStudentInsertDMO>();
                    delete = _OralTestScheduleContext.OralTestScheduleStudentInsertDMO.Where(t => t.PAOTS_Id.Equals(data.DeleteStudentData[0].PAOTS_Id)).ToList();
                    if (delete.Count == 0)
                    {
                        List<OralTestScheduleDMO> masters1 = new List<OralTestScheduleDMO>();
                        masters1 = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id.Equals(data.DeleteStudentData[0].PAOTS_Id)).ToList();
                        if (masters1.Any())
                        {
                            _OralTestScheduleContext.Remove(masters1.ElementAt(0));
                            _OralTestScheduleContext.SaveChanges();
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
        public async Task<OralTestScheduleDTO> OralTestScheduleData(OralTestScheduleDTO mas)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            var time = indianTime.ToString("hh:mm tt");


            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
            mas.ASMAY_Id = Acdemic_preadmission;
            OralTestScheduleDTO vc = new OralTestScheduleDTO();
            long trnsno = 0;
            long senderid = mas.IVRMSTAUL_Id;
            OralTestScheduleDMO MM = Mapper.Map<OralTestScheduleDMO>(mas);
            long paotss = 0;
            if (mas.PAOTS_Id > 0)
            {
                var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_Id != mas.PAOTS_Id && t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedule == 0)
                {
                    var result = _OralTestScheduleContext.OralTestScheduleDMO.Single(t => t.PAOTS_Id == mas.PAOTS_Id);
                    result.PAOTS_ScheduleName = mas.PAOTS_ScheduleName;
                    result.PAOTS_ScheduleDate = mas.PAOTS_ScheduleDate;
                    result.PAOTS_ScheduleTime = mas.PAOTS_ScheduleTime;
                    result.PAOTS_ScheduleTimeTo = mas.PAOTS_ScheduleTimeTo;
                    result.PAOTS_AM_PM = mas.PAOTS_AM_PM;
                    result.PAOTS_Remarks = mas.PAOTS_Remarks;
                    result.PAOTS_EntryDate = result.PAOTS_EntryDate;  //added by 02/02/2017
                    result.PAOTS_Skills = mas.PAOTS_Skills;
                    result.PAOTS_Superviser = mas.PAOTS_Superviser;
                    result.UpdatedDate = DateTime.Now;
                    _OralTestScheduleContext.Update(result);
                    _OralTestScheduleContext.SaveChanges();
                    mas.PAOTS_Id = MM.PAOTS_Id;
                    var othd = mas.PAOTS_ScheduleTime;
                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {
                            OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                            MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                            MM1.PAOTS_Id = mas.PAOTS_Id;
                            //added by 02/02/2017
                            MM1.CreatedDate = DateTime.Now;
                            MM1.UpdatedDate = DateTime.Now;
                            //added on 20/10/17
                            MM1.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                            if (mas.PAOTS_TPFlag == false)
                            {
                                string iDate = mas.PAOTS_ScheduleTime;
                                DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                var addmin = mas.PAOTS_TimePeriod * j;
                                var start_tm = oDate.AddMinutes(addmin);
                                string finalstattim = start_tm.ToString("H:mm");
                                var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                string finalendtim = end_tm.ToString("H:mm");
                                MM1.PAOTSS_Time = finalstattim;
                                MM1.PAOTSS_Time_To = finalendtim;
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
                                MM1.PAOTSS_Time = finalstattim;
                                MM1.PAOTSS_Time_To = finalendtim;
                            }
                            var n = _OralTestScheduleContext.Add(MM1);
                            _OralTestScheduleContext.SaveChanges();

                            if (n != null)
                            {
                                Email Email = new Email(_context);
                                string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                paotss = MM1.PAOTSS_Id;

                                SMS sms = new SMS(_context);
                                string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                                SMS smsS = new SMS(_context);
                                string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                                // SMS NEW TABLES CODE START
                                //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);
                                // SMS NEW TABLES CODE END
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
                            string m = Email.sendmail(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            SMS sms = new SMS(_context);
                            string sss = await sms.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                            SMS smsS = new SMS(_context);
                            string sssS = await smsS.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                            // SMS NEW TABLES CODE START
                            //await sms.sendSmsnewTemplet(mas.SelectedStudentDataForEdit[j].MI_Id, mas.SelectedStudentDataForEdit[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);
                            // SMS NEW TABLES CODE END
                            k++;
                        }
                    }
                }
                else
                {
                    mas.returnvalue = "false";
                }
                mas.PAOTS_Id = 0;
            }
            else
            {
                try
                {
                    MM.IVRMSTAUL_Id = Convert.ToInt32(mas.IVRMSTAUL_Id);
                    MM.PAOTS_EntryDate = indianTime;
                    var schedule = _OralTestScheduleContext.OralTestScheduleDMO.Where(t => t.PAOTS_ScheduleName == mas.PAOTS_ScheduleName && t.PAOTS_ScheduleDate == mas.PAOTS_ScheduleDate && t.MI_Id == mas.MI_Id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                    if (schedule == 0)
                    {
                        //added by 02/02/2017
                        MM.CreatedDate = indianTime;
                        MM.UpdatedDate = indianTime;
                        _OralTestScheduleContext.Add(MM);
                        _OralTestScheduleContext.SaveChanges();
                        mas.PAOTS_Id = MM.PAOTS_Id;

                        int j = 0;
                        if (mas.SelectedStudentData != null)
                        {
                            while (j < mas.SelectedStudentData.Count())
                            {
                                OralTestScheduleStudentInsertDMO MM1 = new OralTestScheduleStudentInsertDMO();
                                MM1.PASR_Id = mas.SelectedStudentData[j].PASR_Id;
                                MM1.PAOTS_Id = mas.PAOTS_Id;
                                MM1.PAOTSS_StatusFlag = 1;
                                //added by 02/02/2017
                                MM1.CreatedDate = indianTime;
                                MM1.UpdatedDate = indianTime;
                                //added on 20/10/17
                                MM1.PAOTSS_Date = mas.PAOTS_ScheduleDate;
                                if (mas.PAOTS_TPFlag == false)
                                {
                                    if (mas.autoschedule == true)
                                    {
                                        string iDate = mas.PAOTS_ScheduleTime;
                                        string todate = mas.PAOTS_ScheduleTimeTo;
                                        DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                        DateTime ToendDate = DateTime.ParseExact(todate, "HH:mm", null);
                                        string finalstattim = oDate.ToString("H:mm");
                                        string finalendtim = ToendDate.ToString("H:mm");
                                        MM1.PAOTSS_Time = finalstattim;
                                        MM1.PAOTSS_Time_To = finalendtim;
                                    }
                                    else
                                    {
                                        string iDate = mas.PAOTS_ScheduleTime;
                                        DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                        var addmin = mas.PAOTS_TimePeriod * j;
                                        var start_tm = oDate.AddMinutes(addmin);
                                        string finalstattim = start_tm.ToString("H:mm");
                                        var end_tm = start_tm.AddMinutes(mas.PAOTS_TimePeriod);
                                        string finalendtim = end_tm.ToString("H:mm");
                                        MM1.PAOTSS_Time = finalstattim;
                                        MM1.PAOTSS_Time_To = finalendtim;
                                    }
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
                                    MM1.PAOTSS_Time = finalstattim;
                                    MM1.PAOTSS_Time_To = finalendtim;
                                }
                                _OralTestScheduleContext.Add(MM1);
                                int n = _OralTestScheduleContext.SaveChanges();
                                if (mas.SelectedStudentData[j].vcmeeting == true)
                                {
                                    vc = mas;
                                    vc.PASR_Id = MM1.PASR_Id;
                                    vc.PlannedDate = Convert.ToDateTime(mas.PAOTS_ScheduleDate);
                                    vc.PlannedStartTime = MM1.PAOTSS_Time;
                                    vc.PlannedEndTime = MM1.PAOTSS_Time_To;
                                    vc.MeetingTopic = mas.PAOTS_ScheduleName;
                                    vc.meetingid = mas.PAOTS_ScheduleName + "-" + DateTime.Now.ToString() + j + "0" + j;
                                    vc.meetingid = vc.meetingid.Replace(" ", "");
                                    vc.meetingid = vc.meetingid.Replace(":", "-");
                                    vc.meetingid = vc.meetingid.Replace("/", "-");
                                    vc.meetingid = vc.meetingid.Replace(".", "-");
                                    createvcmeeting(vc);

                                }
                                if (n > 0)
                                {
                                    if (mas.SelectedStudentData[j].vcmeeting == false)
                                    {
                                        Email Email = new Email(_context);
                                        string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                        SMS sms = new SMS(_context);
                                        string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id);

                                        SMS smsS = new SMS(_context);
                                        string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PASR_Id);

                                        // SMS NEW TABLES CODE START
                                        //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);
                                        // SMS NEW TABLES CODE END
                                    }
                                    else if (mas.SelectedStudentData[j].vcmeeting == true)
                                    {
                                        Email Email = new Email(_context);
                                        string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "VC_ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                        SMS sms = new SMS(_context);
                                        string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "VC_ORAL_TEST_SCHEDULE", MM1.PASR_Id);
                                    }
                                }
                                j++;
                            }
                        }
                    }
                    else
                    {
                        mas.returnvalue = "false";
                    }
                    mas.PAOTS_Id = 0;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return mas;
        }
        public async Task<OralTestScheduleDTO> ReseOralTestScheduleData(OralTestScheduleDTO mas)
        {
            try
            {
                int j = 0;
                if (mas.SelectedStudentData != null)
                {
                    while (j < mas.SelectedStudentData.Count())
                    {
                        if (mas.SelectedStudentData[j].vcmeeting == false)
                        {
                            Email Email = new Email(_context);
                            string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "ORAL_TEST_SCHEDULE", mas.SelectedStudentData[j].PASR_Id);
                            SMS sms = new SMS(_context);
                            string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", mas.SelectedStudentData[j].PASR_Id);

                            SMS smsS = new SMS(_context);
                            string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE_1", mas.SelectedStudentData[j].PASR_Id);
                        }
                        else if (mas.SelectedStudentData[j].vcmeeting == true)
                        {
                            Email Email = new Email(_context);
                            string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_emailId, "VC_ORAL_TEST_SCHEDULE", mas.SelectedStudentData[j].PASR_Id);
                            SMS sms = new SMS(_context);
                            string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "VC_ORAL_TEST_SCHEDULE", mas.SelectedStudentData[j].PASR_Id);
                        }
                        j++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
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
                obj.User_Id = data.userid;
                obj.HRME_Id = 0;
                obj.LMSLMEET_CreatedBy = data.userid;
                obj.LMSLMEET_UpdatedBy = data.userid;
                obj.LMSLMEET_MeetingURL = meetingurl;
                obj.LMSLMEET_CanOthersStartFlg = data.anybodystart;
                obj.LMSLMEET_StartedByUserId = 0;
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
                obj1.LMSLMEETPASTD_UpdatedBy = data.userid;
                obj1.LMSLMEETPASTD_CreatedBy = data.userid;
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
                if (data.pavidc == true)
                {
                    saveparticipants(data);
                }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public OralTestScheduleDTO saveparticipants(OralTestScheduleDTO data)
        {
            try
            {
                if (data.pavidc == true)
                {
                    if (data.stafflag == true)
                    {
                        if (data.stfids.Length > 0)
                        {
                            foreach (var item in data.stfids)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                sobj.User_Id = Convert.ToInt64(item.UserId);
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
                    if (data.managerflg == true)
                    {
                        var users = (from c in _context.ApplicationUser
                                     from f in _context.appUserRole
                                     from d in _context.MasterRoleType
                                     from a in _context.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.Id != data.userid && c.Id == f.UserId && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                   ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            if (users.Count > 0)
                            {
                                foreach (var item in users)
                                {
                                    LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                    sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                    sobj.User_Id = item.Id;
                                    sobj.HRME_Id = 0;
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
                    if (data.principalflg == true)
                    {
                        var users = (from c in _context.ApplicationUser
                                     from f in _context.appUserRole
                                     from d in _context.MasterRoleType
                                     from a in _context.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && a.Id != data.userid && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                  ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            if (users.Count > 0)
                            {
                                foreach (var item in users)
                                {
                                    LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                    sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                    sobj.User_Id = item.Id;
                                    sobj.HRME_Id = 0;
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public OralTestScheduleDTO checkaddparticipants(OralTestScheduleDTO data)
        {
            try
            {
                if (data.checkadd == "CK")
                {
                    List<OralTestScheduleDTO> getvcschedule = new List<OralTestScheduleDTO>();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Takepreadmissionschedulename";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@schids",
                        SqlDbType.VarChar)
                        {
                            Value = data.PAOTS_Id
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
                                    getvcschedule.Add(new OralTestScheduleDTO
                                    {
                                        LMSLMEET_Id = Convert.ToInt64(dataReader["LMSLMEET_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (getvcschedule.Count > 0)
                    {
                        data.LMSLMEET_Id = getvcschedule.FirstOrDefault().LMSLMEET_Id;
                        var anybodystart = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_Id == getvcschedule.FirstOrDefault().LMSLMEET_Id).ToList();
                        if (anybodystart.Count > 0)
                        {
                            data.anybodystart = anybodystart.FirstOrDefault().LMSLMEET_CanOthersStartFlg;
                        }

                        var stafflist = (from c in _context.ApplicationUser
                                     from f in _context.LMS_Live_Meeting_StaffOthersDMO
                                     where (c.Id == f.User_Id && f.LMSLMEET_Id == getvcschedule.FirstOrDefault().LMSLMEET_Id
                                  )
                                     select new OralTestScheduleDTO
                                     {                                         
                                         username = c.Name,
                                         User_Id = c.Id
                                     }
                                   ).Distinct().ToList();

                        //var stafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == getvcschedule.FirstOrDefault().LMSLMEET_Id && t.HRME_Id>0).ToList();
                        if (stafflist.Count > 0)
                        {
                            data.stafflag = true;
                            data.mappedstafflist = stafflist.ToArray();
                        }

                        List<long> mangers = new List<long>();
                        var users = (from c in _context.ApplicationUser
                                     from f in _context.appUserRole
                                     from d in _context.MasterRoleType
                                     from a in _context.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.Id != data.userid && c.Id == f.UserId && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                   ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            mangers = users.Select(t => t.Id).ToList();

                            var duplicatecheck = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == getvcschedule.FirstOrDefault().LMSLMEET_Id && mangers.Contains(t.User_Id)).ToList();

                            if (duplicatecheck.Count > 0)
                            {
                                data.managerflg = true;
                            }
                        }

                        List<long> principal = new List<long>();
                        var usersp = (from c in _context.ApplicationUser
                                      from f in _context.appUserRole
                                      from d in _context.MasterRoleType
                                      from a in _context.UserRoleWithInstituteDMO
                                      where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && a.Id != data.userid && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase)
                                   )
                                      select new StaffLoginDTO
                                      {
                                          rolenamess = d.IVRMRT_Role,
                                          User_Name = c.UserName,
                                          Id = c.Id
                                      }
                                  ).Distinct().ToList();

                        if (usersp.Count > 0)
                        {
                            principal = usersp.Select(t => t.Id).ToList();
                            var checkprincipal = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == getvcschedule.FirstOrDefault().LMSLMEET_Id && principal.Contains(t.User_Id)).ToList();
                            if (checkprincipal.Count > 0)
                            {
                                data.principalflg = true;
                            }
                        }

                        data.stafflist = (from a in _context.Staff_User_Login
                                          from b in _context.HR_Master_Employee_DMO
                                          where a.MI_Id == b.MI_Id && a.Emp_Code == b.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1 && b.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && a.Id != data.userid
                                          select new OralTestScheduleDTO
                                          {
                                              userid = a.Id,
                                              HRME_Id = b.HRME_Id,
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                          }).Distinct().OrderBy(r => r.HRME_EmployeeFirstName).ToArray();
                    }
                }
                else if (data.checkadd == "SV")
                {
                    List<OralTestScheduleDTO> getvcschedule = new List<OralTestScheduleDTO>();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ViewPreadmission_schedulewise_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@schids",
                        SqlDbType.VarChar)
                        {
                            Value = data.PAOTS_Id
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
                                    getvcschedule.Add(new OralTestScheduleDTO
                                    {
                                        LMSLMEET_Id = Convert.ToInt64(dataReader["LMSLMEET_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (getvcschedule.Count > 0)
                    {
                        foreach (var schedule in getvcschedule)
                        {
                            data.LMSLMEET_Id = schedule.LMSLMEET_Id;
                            if (data.stafflag == true)
                            {
                                List<long?> staffs = new List<long?>();
                                if (data.stfids.Length > 0)
                                {
                                    staffs = data.stfids.Select(t => t.UserId).ToList();

                                    var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && !staffs.Contains(t.User_Id) && (t.LMSLMEETSTFOTH_LoginTime == null || t.LMSLMEETSTFOTH_LoginTime == "") && t.HRME_Id>0).ToList();
                                    if (deletstafflist.Count > 0)
                                    {
                                        for (int i = 0; i < deletstafflist.Count(); i++)
                                        {
                                            _context.Remove(deletstafflist.ElementAt(i));
                                            _context.SaveChanges();
                                        }
                                    }

                                    foreach (var item in data.stfids)
                                    {
                                        var stafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && t.User_Id == item.UserId).ToList();
                                        if (stafflist.Count == 0)
                                        {
                                            LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                            sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                            sobj.User_Id = Convert.ToInt64(item.UserId);
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
                            else
                            {
                                var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && (t.LMSLMEETSTFOTH_LoginTime == null || t.LMSLMEETSTFOTH_LoginTime == "") && t.HRME_Id > 0).ToList();
                                if (deletstafflist.Count > 0)
                                {
                                    for (int i = 0; i < deletstafflist.Count(); i++)
                                    {
                                        _context.Remove(deletstafflist.ElementAt(i));
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            if (data.managerflg == true)
                            {
                                List<long> manegers = new List<long>();
                                var users = (from c in _context.ApplicationUser
                                             from f in _context.appUserRole
                                             from d in _context.MasterRoleType
                                             from a in _context.UserRoleWithInstituteDMO
                                             where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.Id != data.userid && c.Id == f.UserId && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase)
                                          )
                                             select new StaffLoginDTO
                                             {
                                                 rolenamess = d.IVRMRT_Role,
                                                 User_Name = c.UserName,
                                                 Id = c.Id
                                             }
                                           ).Distinct().ToList();

                                if (users.Count > 0)
                                {
                                    if (users.Count > 0)
                                    {
                                        manegers = users.Select(t => t.Id).ToList();
                                        var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && manegers.Contains(t.User_Id) && (t.LMSLMEETSTFOTH_LoginTime == null || t.LMSLMEETSTFOTH_LoginTime == "")).ToList();
                                        if (deletstafflist.Count > 0)
                                        {
                                            for (int i = 0; i < deletstafflist.Count(); i++)
                                            {
                                                _context.Remove(deletstafflist.ElementAt(i));
                                                _context.SaveChanges();
                                            }
                                        }

                                        foreach (var item in users)
                                        {
                                            var stafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && t.User_Id == item.Id).ToList();
                                            if (stafflist.Count == 0)
                                            {
                                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                                sobj.User_Id = item.Id;
                                                sobj.HRME_Id = 0;
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

                            }
                            else
                            {
                                List<long> manegers = new List<long>();
                                var users = (from c in _context.ApplicationUser
                                             from f in _context.appUserRole
                                             from d in _context.MasterRoleType
                                             from a in _context.UserRoleWithInstituteDMO
                                             where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.Id != data.userid && c.Id == f.UserId && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase)
                                          )
                                             select new StaffLoginDTO
                                             {
                                                 rolenamess = d.IVRMRT_Role,
                                                 User_Name = c.UserName,
                                                 Id = c.Id
                                             }
                                           ).Distinct().ToList();

                                if (users.Count > 0)
                                {
                                    if (users.Count > 0)
                                    {
                                        manegers = users.Select(t => t.Id).ToList();
                                        var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && manegers.Contains(t.User_Id) && (t.LMSLMEETSTFOTH_LoginTime == null || t.LMSLMEETSTFOTH_LoginTime == "")).ToList();
                                        if (deletstafflist.Count > 0)
                                        {
                                            for (int i = 0; i < deletstafflist.Count(); i++)
                                            {
                                                _context.Remove(deletstafflist.ElementAt(i));
                                                _context.SaveChanges();
                                            }
                                        }                                     
                                    }
                                }
                            }
                            if (data.principalflg == true)
                            {
                                List<long> principals = new List<long>();
                                var users = (from c in _context.ApplicationUser
                                             from f in _context.appUserRole
                                             from d in _context.MasterRoleType
                                             from a in _context.UserRoleWithInstituteDMO
                                             where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && a.Id != data.userid && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase)
                                          )
                                             select new StaffLoginDTO
                                             {
                                                 rolenamess = d.IVRMRT_Role,
                                                 User_Name = c.UserName,
                                                 Id = c.Id
                                             }
                                          ).Distinct().ToList();

                                if (users.Count > 0)
                                {
                                    if (users.Count > 0)
                                    {
                                        principals = users.Select(t => t.Id).ToList();

                                        var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && principals.Contains(t.User_Id) && (t.LMSLMEETSTFOTH_LoginTime == null || t.LMSLMEETSTFOTH_LoginTime == "")).ToList();
                                        if (deletstafflist.Count > 0)
                                        {
                                            for (int i = 0; i < deletstafflist.Count(); i++)
                                            {
                                                _context.Remove(deletstafflist.ElementAt(i));
                                                _context.SaveChanges();
                                            }
                                        }

                                        foreach (var item in users)
                                        {
                                            var stafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && t.User_Id == item.Id).ToList();
                                            if (stafflist.Count == 0)
                                            {
                                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                                sobj.User_Id = item.Id;
                                                sobj.HRME_Id = 0;
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
                            }
                            else
                            {
                                List<long> principals = new List<long>();
                                var users = (from c in _context.ApplicationUser
                                             from f in _context.appUserRole
                                             from d in _context.MasterRoleType
                                             from a in _context.UserRoleWithInstituteDMO
                                             where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && a.Id != data.userid && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase)
                                          )
                                             select new StaffLoginDTO
                                             {
                                                 rolenamess = d.IVRMRT_Role,
                                                 User_Name = c.UserName,
                                                 Id = c.Id
                                             }
                                          ).Distinct().ToList();

                                if (users.Count > 0)
                                {
                                    if (users.Count > 0)
                                    {
                                        principals = users.Select(t => t.Id).ToList();

                                        var deletstafflist = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && principals.Contains(t.User_Id) && (t.LMSLMEETSTFOTH_LoginTime == null || t.LMSLMEETSTFOTH_LoginTime == "")).ToList();
                                        if (deletstafflist.Count > 0)
                                        {
                                            for (int i = 0; i < deletstafflist.Count(); i++)
                                            {
                                                _context.Remove(deletstafflist.ElementAt(i));
                                                _context.SaveChanges();
                                            }
                                        }
                                      
                                    }
                                }
                            }
                            var result = _context.LMS_Live_MeetingDMO.Single(t => t.LMSLMEET_Id == data.LMSLMEET_Id);
                            result.LMSLMEET_CanOthersStartFlg = data.anybodystart;
                            _context.Update(result);

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
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public String Getmetingid(OralTestScheduleDTO data)
        {
            string meetingid = "";
            try
            {
                meetingid = data.PASR_Id.ToString();

            }
            catch (Exception ex)
            {

                throw;
            }

            return meetingid;
        }
    }
}
