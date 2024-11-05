using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Preadmission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace CollegePreadmission.Services
{
    public class OralTestScheduleClgImpl:Interfaces.OralTestScheduleClgInterface
    {
        private static ConcurrentDictionary<string, DocumentViewClgDTO> _login =
       new ConcurrentDictionary<string, DocumentViewClgDTO>();


        //private readonly DomainModelMsSqlServerContext _context;
        //public StudentApplicationContext _StudentApplicationContext;
        //public FeeGroupContext _feecontext;

        ClgAdmissionContext _context;
        CollegepreadmissionContext _precontext;
        private readonly DomainModelMsSqlServerContext _db;
        CollFeeGroupContext _feecontext;

        public OralTestScheduleClgImpl(ClgAdmissionContext context, DomainModelMsSqlServerContext db, CollFeeGroupContext feecontext,
            CollegepreadmissionContext precontext)
        {
            _context = context;
            _db = db;
            _precontext = precontext;
            _feecontext = feecontext;
        }
        public DocumentViewClgDTO GetOralTestScheduleData(DocumentViewClgDTO StudentDetailsDTO)//int IVRMM_Id
        {
            try
            {
                var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.mi_id).Select(d => d.ASMAY_Id).FirstOrDefault();
                StudentDetailsDTO.asmay_id = Acdemic_preadmission;

                List<MasterCourseDMO> allcourse = new List<MasterCourseDMO>();
                allcourse = _context.MasterCourseDMO.Where(t => t.MI_Id == StudentDetailsDTO.mi_id && t.AMCO_ActiveFlag == true).ToList();
                StudentDetailsDTO.allcourse = allcourse.ToArray();

                List<OralTestScheduleClgDMO> Allname = new List<OralTestScheduleClgDMO>();
                Allname = _precontext.OralTestScheduleClgDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id)).OrderBy(t => t.PAOTSC_ScheduleDate).ToList();
                StudentDetailsDTO.OralTestScheduleclg = Allname.ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_VC_Orl_Schedules_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.mi_id
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
                        StudentDetailsDTO.vcOralTestScheduleClg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Orl_Schedules_Count_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.mi_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.asmay_id
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
                        StudentDetailsDTO.OralTestScheduleClgcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Preadmission_Orl_Schedules_Overall";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@miid",
                //        SqlDbType.BigInt)
                //    {
                //        Value = StudentDetailsDTO.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = StudentDetailsDTO.ASMAY_Id
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
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
                //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        StudentDetailsDTO.overallOralTestSchedulecountClg = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write(ex.Message);
                //    }
                //}

                StudentDetailsDTO.stafflist = (from a in _context.Staff_User_Login
                                               from b in _context.HR_Master_Employee_DMO
                                               where a.MI_Id == b.MI_Id && a.Emp_Code == b.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1 && b.HRME_ActiveFlag == true
                                               && a.MI_Id == StudentDetailsDTO.mi_id && a.Id != StudentDetailsDTO.user_id
                                               select new DocumentViewClgDTO
                                               {
                                                   user_id = a.Id,
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
        public DocumentViewClgDTO coursewisestudent(DocumentViewClgDTO StudentDetailsDTO)
        {
            try
            {
                var Acdemic_preadmission = _precontext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == StudentDetailsDTO.mi_id).Select(d => d.ASMAY_Id).FirstOrDefault();

                StudentDetailsDTO.asmay_id = Acdemic_preadmission;


                List<PA_College_Application> lorg2 = new List<PA_College_Application>();
                List<PA_College_Application> lorg5 = new List<PA_College_Application>();
                List<PA_College_Application> lorgfinal = new List<PA_College_Application>();
                Array[] showdata1 = new Array[1];

                List<OralTestScheduleStudentInsertClgDMO> lorg1 = new List<OralTestScheduleStudentInsertClgDMO>();
                lorg1 = _precontext.OralTestScheduleStudentInsertClgDMO.ToList();
                List<long> Allname2 = new List<long>();
                Allname2 = lorg1.Select(t => t.PACA_Id).ToList();

                List<PA_College_Application> Allname1 = new List<PA_College_Application>();

                List<long> studentlistof = new List<long>();

                List<PA_College_Application> Allname5 = new List<PA_College_Application>();
                if (StudentDetailsDTO.AMCO_Id != 0)
                {
                    lorg5 = _precontext.PA_College_Application.Where(t => t.MI_Id == StudentDetailsDTO.mi_id && t.AMCO_Id == StudentDetailsDTO.AMCO_Id && t.ASMAY_Id == StudentDetailsDTO.asmay_id).ToList();
                }
                else
                {
                    lorg5 = _precontext.PA_College_Application.Where(t => t.MI_Id == StudentDetailsDTO.mi_id && t.ASMAY_Id == StudentDetailsDTO.asmay_id).ToList();
                }


                studentlistof = lorg5.Select(t => t.PACA_Id).ToList();

                List<MasterCourseDMO> allcourse = new List<MasterCourseDMO>();
                allcourse = _precontext.MasterCourseDMO.Where(t => t.MI_Id == StudentDetailsDTO.mi_id && t.AMCO_ActiveFlag == true).ToList();
                StudentDetailsDTO.allcourse = allcourse.ToArray();


                List<long> Allname3 = new List<long>();
                Allname5 = _precontext.PA_College_Application.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id) && t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.PACA_ApplStatus == "787928").ToList();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _precontext.masterConfig.Where(d => d.MI_Id.Equals(StudentDetailsDTO.mi_id) && d.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id)).ToList();
                StudentDetailsDTO.mstConfig = mstConfig.ToArray();

                if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                {

                    if (mstConfig[0].ISPAC_ApplFeeFlag == 1)
                    {
                        //foreach (var a in lorg5)
                        //{
                        Allname3 = _precontext.Fee_Y_Payment_PA_Application.Where(t => t.FYPPA_Type == "R" &&
                        studentlistof.Contains(t.PACA_Id)).Select(t => t.PACA_Id).ToList();

                        //if (StudentDetailsDTO.payementcheck == 1)
                        //{

                        //    Allname3.Add(a.pasr_id);

                        //}
                        //}

                        //foreach (var a in lorg1)
                        //{
                        //    Allname2.Add(a.PASR_Id);
                        //}

                        if (StudentDetailsDTO.AMCO_Id != 0)

                        {
                            Allname1 = _precontext.PA_College_Application.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id) && t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.PACA_AdmStatus != 5 && !Allname2.Contains(t.PACA_Id) && Allname3.Contains(t.PACA_Id)
                            && t.AMCO_Id == StudentDetailsDTO.AMCO_Id).ToList();

                            StudentDetailsDTO.studentDetails = Allname1.ToArray();
                        }
                        else
                        {
                            Allname1 = _precontext.PA_College_Application.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id) && t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.PACA_AdmStatus != 5 && !Allname2.Contains(t.PACA_Id) && Allname3.Contains(t.PACA_Id)).ToList();

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
                    if (StudentDetailsDTO.AMCO_Id != 0)
                    {
                        Allname1 = _precontext.PA_College_Application.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id) && t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.PACA_AdmStatus != 5 && !Allname2.Contains(t.PACA_Id) && t.AMCO_Id == StudentDetailsDTO.AMCO_Id).ToList();
                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }
                    else
                    {
                        Allname1 = _precontext.PA_College_Application.Where(t => t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id) && t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.PACA_AdmStatus != 5 && !Allname2.Contains(t.PACA_Id)).ToList();

                        StudentDetailsDTO.studentDetails = Allname1.ToArray();
                    }


                }

                Array[] showdata = new Array[50];
                List<OralTestScheduleClgDMO> Allname = new List<OralTestScheduleClgDMO>();
                Allname = _precontext.OralTestScheduleClgDMO.Where(t => t.MI_Id.Equals(StudentDetailsDTO.mi_id) && t.ASMAY_Id.Equals(StudentDetailsDTO.asmay_id)).ToList();
                StudentDetailsDTO.OralTestScheduleclg = Allname.ToArray();

                allcourse = _precontext.MasterCourseDMO.Where(t => t.MI_Id == StudentDetailsDTO.mi_id && t.AMCO_ActiveFlag == true).ToList();
                StudentDetailsDTO.admissioncatdrpall = allcourse.ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_VC_Orl_Schedules_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.mi_id
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
                        StudentDetailsDTO.vcOralTestScheduleClg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Orl_Schedules_Count_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.mi_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = StudentDetailsDTO.asmay_id
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
                        StudentDetailsDTO.OralTestScheduleClgcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Preadmission_Orl_Schedules_Overall";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@miid",
                //        SqlDbType.BigInt)
                //    {
                //        Value = StudentDetailsDTO.MI_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //        SqlDbType.BigInt)
                //    {
                //        Value = StudentDetailsDTO.ASMAY_Id
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    var datatype = dataReader.GetFieldType(iFiled);
                //                    if (datatype.Name == "DateTime")
                //                    {
                //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                //                        dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? "" : dateval  // use null instead of {}
                //                    );
                //                    }
                //                    else
                //                    {
                //                        dataRow.Add(
                //                       dataReader.GetName(iFiled),
                //                       dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                //                   );
                //                    }
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        StudentDetailsDTO.overallOralTestSchedulecount = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return StudentDetailsDTO;
        }


        public DocumentViewClgDTO GetSelectedRowDetails(int ID)
        {

            DocumentViewClgDTO OralTestScheduleDTO = new DocumentViewClgDTO();
            List<OralTestScheduleClgDMO> lorg = new List<OralTestScheduleClgDMO>();
            lorg = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_Id.Equals(ID)).ToList();
            OralTestScheduleDTO.OralTestScheduleclg = lorg.ToArray();

            DocumentViewClgDTO StudentDetailsDTO = new DocumentViewClgDTO();

            List<OralTestScheduleStudentInsertClgDMO> lorg1 = new List<OralTestScheduleStudentInsertClgDMO>();
            List<PA_College_Application> lorg2 = new List<PA_College_Application>();
            List<PA_College_Application> lorgfinal = new List<PA_College_Application>();
            lorg1 = _precontext.OralTestScheduleStudentInsertClgDMO.Where(t => t.PAOTSCS_Id.Equals(ID)).ToList();
            int j = 0;
            while (j < lorg1.Count())
            {
                lorg2.AddRange(_precontext.PA_College_Application.Where(t => t.PACA_Id.Equals(lorg1[j].PACA_Id)).ToList());
                j++;
            }

            OralTestScheduleDTO.SelectedStudentDetails = lorg2.ToArray();
            OralTestScheduleDTO.OralTestScheduleclg = lorg.ToArray();

            return OralTestScheduleDTO;
        }

        public async Task<DocumentViewClgDTO> OralTestScheduleData(DocumentViewClgDTO mas)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            var time = indianTime.ToString("hh:mm tt");


            var Acdemic_preadmission = _precontext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == mas.mi_id).Select(d => d.ASMAY_Id).FirstOrDefault();
            mas.asmay_id = Acdemic_preadmission;
            DocumentViewClgDTO vc = new DocumentViewClgDTO();
            long trnsno = 0;
            long senderid = mas.IVRMSTAUL_Id;
            OralTestScheduleClgDMO MM = Mapper.Map<OralTestScheduleClgDMO>(mas);
            long paotss = 0;
            if (mas.PAOTSC_Id > 0)
            {
                var schedule = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_Id != mas.PAOTSC_Id && t.PAOTSC_ScheduleName == mas.PAOTSC_ScheduleName && t.PAOTSC_ScheduleDate == mas.PAOTSC_ScheduleDate && t.MI_Id == mas.mi_id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                if (schedule == 0)
                {
                    var result = _precontext.OralTestScheduleClgDMO.Single(t => t.PAOTSC_Id == mas.PAOTSC_Id);
                    result.PAOTSC_ScheduleName = mas.PAOTSC_ScheduleName;
                    result.PAOTSC_ScheduleDate = mas.PAOTSC_ScheduleDate;
                    result.PAOTSC_ScheduleFromTime = mas.PAOTSC_ScheduleFromTime;
                    result.PAOTSC_ScheduleToTime = mas.PAOTSC_ScheduleToTime;
                    result.PAOTSC_To_AM_PM = mas.PAOTSC_To_AM_PM;
                    result.PAOTSC_Remarks = mas.PAOTSC_Remarks;
                    result.PAOTSC_Date = result.PAOTSC_Date;
                    //  result.PAOTS_Skills = mas.PAOTS_Skills;
                    // result.PAOTS_Superviser = mas.PAOTS_Superviser;
                    // result.UpdatedDate = DateTime.Now;
                    _precontext.Update(result);
                    _precontext.SaveChanges();
                    mas.PAOTSC_Id = MM.PAOTSC_Id;
                    var othd = mas.PAOTSC_ScheduleFromTime;
                    int j = 0;
                    if (mas.SelectedStudentData != null)
                    {
                        while (j < mas.SelectedStudentData.Count())
                        {
                            OralTestScheduleStudentInsertClgDMO MM1 = new OralTestScheduleStudentInsertClgDMO();
                            MM1.PACA_Id = mas.SelectedStudentData[j].PACA_Id;
                            MM1.PAOTSCS_Id = mas.PAOTSC_Id;
                            //added by 02/02/2017
                            //  MM1.CreatedDate = DateTime.Now;
                            //   MM1.UpdatedDate = DateTime.Now;
                            //added on 20/10/17
                            MM1.PAOTSCS_Date = mas.PAOTSC_ScheduleDate;
                            //if (mas.PAOTSC_TPFlag == false)
                            //{
                            //    string iDate = mas.PAOTSC_ScheduleFromTime;
                            //    DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                            //    var addmin = mas.PAOTSC_TimePeriod * j;
                            //    var start_tm = oDate.AddMinutes(addmin);
                            //    string finalstattim = start_tm.ToString("H:mm");
                            //    var end_tm = start_tm.AddMinutes(mas.PAOTSC_TimePeriod);
                            //    string finalendtim = end_tm.ToString("H:mm");
                            //    MM1.PAOTSCS_Time = finalstattim;
                            //    MM1.PAOTSCS_Time_To = finalendtim;
                            //}
                            //else
                            //{
                            //    string iDate = mas.PAOTSC_ScheduleFromTime;
                            //    DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                            //    var addhr = mas.PAOTSC_TimePeriod * j;
                            //    var start_tm = oDate.AddHours(addhr);
                            //    string finalstattim = start_tm.ToString("H:mm");
                            //    var end_tm = start_tm.AddHours(mas.PAOTSC_TimePeriod);
                            //    string finalendtim = end_tm.ToString("H:mm");
                            //    //var paotss_tm = oDate.AddHours(addhr);
                            //    //string finaltim = paotss_tm.ToString("H:mm");
                            //    MM1.PAOTSCS_Time = finalstattim;
                            //    MM1.PAOTSCS_Time_To = finalendtim;
                            //}
                            var n = _precontext.Add(MM1);
                            _precontext.SaveChanges();

                            if (n != null)
                            {
                                //Email Email = new Email(_context);
                                //string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_emailId, "ORAL_TEST_SCHEDULE", MM1.PACA_Id);
                                //paotss = MM1.PAOTSS_Id;

                                //SMS sms = new SMS(_context);
                                //string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PACA_Id);

                                //SMS smsS = new SMS(_context);
                                //string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PACA_Id);

                                //  // SMS NEW TABLES CODE START
                                // //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);
                                //  // SMS NEW TABLES CODE END
                            }
                            j++;
                        }
                    }
                    int k = 0;
                    if (mas.SelectedStudentDataForEdit != null)
                    {
                        while (k < mas.SelectedStudentDataForEdit.Count())
                        {
                            OralTestScheduleStudentInsertClgDMO MM1 = new OralTestScheduleStudentInsertClgDMO();
                            MM1.PACA_Id = mas.SelectedStudentDataForEdit[k].PACA_Id;
                            MM1.PAOTSC_Id = mas.PAOTSC_Id;

                            //Email Email = new Email(_context);
                            //string m = Email.sendmail(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PACA_emailId, "ORAL_TEST_SCHEDULE", MM1.PACA_Id);

                            //SMS sms = new SMS(_context);
                            //string sss = await sms.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PACA_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PACA_Id);

                            //SMS smsS = new SMS(_context);
                            //string sssS = await smsS.sendSms(mas.SelectedStudentDataForEdit[k].MI_Id, mas.SelectedStudentDataForEdit[k].PACA_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PACA_Id);

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
                mas.PAOTSC_Id = 0;
            }
            else
            {
                try
                {
                    MM.IVRMSTAUL_Id = Convert.ToInt32(mas.IVRMSTAUL_Id);
                    MM.PAOTSC_Date = indianTime;
                    var schedule = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_ScheduleName == mas.PAOTSC_ScheduleName && t.PAOTSC_ScheduleDate == mas.PAOTSC_ScheduleDate && t.MI_Id == mas.mi_id && t.ASMAY_Id == mas.ASMAY_Id).Count();
                    if (schedule == 0)
                    {
                        //added by 02/02/2017
                        //MM.CreatedDate = indianTime;
                        //MM.UpdatedDate = indianTime;
                        _precontext.Add(MM);
                        _precontext.SaveChanges();
                        mas.PAOTSC_Id = MM.PAOTSC_Id;

                        int j = 0;
                        if (mas.SelectedStudentData != null)
                        {
                            while (j < mas.SelectedStudentData.Count())
                            {
                                OralTestScheduleStudentInsertClgDMO MM1 = new OralTestScheduleStudentInsertClgDMO();
                                MM1.PACA_Id = mas.SelectedStudentData[j].PACA_Id;
                                MM1.PAOTSC_Id = mas.PAOTSC_Id;
                                MM1.PAOTSCS_StatusFlag = 1;
                                //added by 02/02/2017
                                //MM1.CreatedDate = indianTime;
                                //MM1.UpdatedDate = indianTime;
                                //added on 20/10/17
                                MM1.PAOTSCS_Date = mas.PAOTSC_ScheduleDate;
                                if (mas.PAOTSC_TPFlag == "0")
                                {
                                    if (mas.autoschedule == true)
                                    {
                                        string iDate = Convert.ToString(mas.PAOTSC_ScheduleFromTime);
                                        string todate = mas.PAOTSC_ScheduleFromTime;
                                        DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                        DateTime ToendDate = DateTime.ParseExact(todate, "HH:mm", null);
                                        string finalstattim = oDate.ToString("H:mm");
                                        string finalendtim = ToendDate.ToString("H:mm");
                                        MM1.PAOTSCS_Time = finalstattim;
                                        MM1.PAOTSCS_Time_To = finalendtim;
                                    }
                                    else
                                    {
                                        string iDate = Convert.ToString(mas.PAOTSC_ScheduleFromTime);
                                        DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                        var addmin = Convert.ToInt32(mas.PAOTSC_TimePeriod) * j;
                                        var start_tm = oDate.AddMinutes(addmin);
                                        string finalstattim = start_tm.ToString("H:mm");
                                        var end_tm = start_tm.AddMinutes(Convert.ToInt32(mas.PAOTSC_TimePeriod));
                                        string finalendtim = end_tm.ToString("H:mm");
                                        MM1.PAOTSCS_Time = finalstattim;
                                        MM1.PAOTSCS_Time_To = finalendtim;
                                    }
                                }
                                else
                                {
                                    string iDate = mas.PAOTSC_ScheduleFromTime;
                                    DateTime oDate = DateTime.ParseExact(iDate, "HH:mm", null);
                                    var addhr = Convert.ToInt32(mas.PAOTSC_TimePeriod) * j;
                                    var start_tm = oDate.AddHours(addhr);
                                    string finalstattim = start_tm.ToString("H:mm");
                                    var end_tm = start_tm.AddHours(Convert.ToInt32(mas.PAOTSC_TimePeriod));
                                    string finalendtim = end_tm.ToString("H:mm");
                                    //var paotss_tm = oDate.AddHours(addhr);
                                    //string finaltim = paotss_tm.ToString("H:mm");
                                    MM1.PAOTSCS_Time = finalstattim;
                                    MM1.PAOTSCS_Time_To = finalendtim;
                                }
                                _precontext.Add(MM1);
                                int n = _precontext.SaveChanges();
                                if (mas.SelectedStudentData[j].vcmeeting == true)
                                {
                                    vc = mas;
                                    vc.paca_id = MM1.PACA_Id
   ;
                                    vc.PlannedDate = Convert.ToDateTime(mas.PAOTSC_ScheduleDate);
                                    vc.PlannedStartTime = MM1.PAOTSCS_Time;
                                    vc.PlannedEndTime = MM1.PAOTSCS_Time_To;
                                    vc.MeetingTopic = mas.PAOTSC_ScheduleName;
                                    vc.meetingid = mas.PAOTSC_ScheduleName + "-" + DateTime.Now.ToString() + j + "0" + j;
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
                                        //Email Email = new Email(_context);
                                        //string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_emailId, "ORAL_TEST_SCHEDULE", MM1.PACA_Id);
                                        //SMS sms = new SMS(_context);
                                        //string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PACA_Id);

                                        //SMS smsS = new SMS(_context);
                                        //string sssS = await smsS.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_MobileNo, "ORAL_TEST_SCHEDULE_1", MM1.PACA_Id);

                                        // SMS NEW TABLES CODE START
                                        //await sms.sendSmsnewTemplet(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PASR_MobileNo, "ORAL_TEST_SCHEDULE", MM1.PASR_Id, trnsno, studentempflag, senderid);
                                        // SMS NEW TABLES CODE END
                                    }
                                    else if (mas.SelectedStudentData[j].vcmeeting == true)
                                    {
                                        //Email Email = new Email(_context);
                                        //string m = Email.sendmail(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_emailId, "VC_ORAL_TEST_SCHEDULE", MM1.PACA_Id);
                                        //SMS sms = new SMS(_context);
                                        //string sss = await sms.sendSms(mas.SelectedStudentData[j].MI_Id, mas.SelectedStudentData[j].PACA_MobileNo, "VC_ORAL_TEST_SCHEDULE", MM1.PACA_Id);
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
                    mas.PAOTSC_Id = 0;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return mas;
        }

        public DocumentViewClgDTO createvcmeeting(DocumentViewClgDTO data)
        {
            try
            {
                var duplicatecheck = _db.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_MeetingId == data.meetingid).ToList();
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
                var teamcredentials = _context.Institution.Where(c => c.MI_Id == data.mi_id).ToList();
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
                obj.MI_Id = data.mi_id;
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
                _db.Add(obj);
                int res = _db.SaveChanges();
                data.LMSLMEET_Id = obj.LMSLMEET_Id;

                LMS_Live_Meeting_PAStudent_CollegeDMO obj1 = new LMS_Live_Meeting_PAStudent_CollegeDMO();
                obj1.LMSLMEET_Id = data.LMSLMEET_Id;
                obj1.PACA_Id = data.paca_id;
                obj1.LMSLMEETPASTDC_ActiveFlg = true;
                obj1.LMSLMEETPASTDC_CreatedDate = DateTime.Now;
                obj1.LMSLMEETPASTDC_UpdatedDate = DateTime.Now;
                obj1.LMSLMEETPASTDC_UpdatedBy = data.IVRMSTAUL_Id;
                obj1.LMSLMEETPASTDC_CreatedBy = data.IVRMSTAUL_Id;
                _precontext.Add(obj1);
                int ress = _precontext.SaveChanges();
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

        public DocumentViewClgDTO OralTestScheduleDeletesData(int ID)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            DocumentViewClgDTO OralTestScheduleDTO = new DocumentViewClgDTO();
            List<OralTestScheduleStudentInsertClgDMO> masters = new List<OralTestScheduleStudentInsertClgDMO>();
            masters = _precontext.OralTestScheduleStudentInsertClgDMO.Where(t => t.PAOTSCS_Id.Equals(ID)).ToList();

            List<OralTestScheduleClgDMO> schedule = new List<OralTestScheduleClgDMO>();
            schedule = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_Id.Equals(ID)).ToList();
            if (masters.Any())
            {
                try
                {
                    int j = 0;
                    while (j < masters.Count())
                    {
                        List<OralTestScheduleStudentInsertClgDMO> masters3 = new List<OralTestScheduleStudentInsertClgDMO>();
                        masters3 = _precontext.OralTestScheduleStudentInsertClgDMO.Where(t => t.Equals(masters[j].PAOTSCS_Id)).ToList();
                        _precontext.Remove(masters3.ElementAt(0));
                        int deletedata = _precontext.SaveChanges();

                        //List<LMS_Live_MeetingDMO> livemeet = new List<LMS_Live_MeetingDMO>();
                        //livemeet = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_PlannedDate == schedule.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == masters[j].PAOTSS_Time && t.LMSLMEET_PlannedEndTime == masters[j].PAOTSS_Time_To).ToList();
                        //if (livemeet.Count > 0)
                        //{
                        //    List<LMS_Live_Meeting_PAStudentDMO> livemeetstudentdelete = new List<LMS_Live_Meeting_PAStudentDMO>();
                        //    livemeetstudentdelete = _context.LMS_Live_Meeting_PAStudentDMO.Where(t => t.PASR_Id.Equals(masters[j].PASR_Id) && t.LMSLMEET_Id == livemeet.FirstOrDefault().LMSLMEET_Id).ToList();
                        //    _context.Remove(livemeetstudentdelete.ElementAt(0));
                        //    int deletestudent = _context.SaveChanges();

                        //    List<LMS_Live_Meeting_StaffOthersDMO> livemeetstaffdelete = new List<LMS_Live_Meeting_StaffOthersDMO>();
                        //    livemeetstaffdelete = _context.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == livemeet.FirstOrDefault().LMSLMEET_Id).ToList();
                        //    if (livemeetstaffdelete.Count > 0)
                        //    {
                        //        for (int i = 0; i < livemeetstaffdelete.Count(); i++)
                        //        {
                        //            _context.Remove(livemeetstaffdelete.ElementAt(i));
                        //            _context.SaveChanges();
                        //        }
                        //    }
                        //}

                        List<OralTestScheduleStudentInsertClgDMO> livemeetstatus = new List<OralTestScheduleStudentInsertClgDMO>();
                        livemeetstatus = _precontext.OralTestScheduleStudentInsertClgDMO.Where(t => t.PACA_Id.Equals(masters[j].PACA_Id)).OrderByDescending(t => t.PAOTSCS_Id).ToList();
                        if (livemeetstatus.Count > 0)
                        {
                            var result = _precontext.OralTestScheduleStudentInsertClgDMO.Single(t => t.PAOTSCS_Id == livemeetstatus.FirstOrDefault().PAOTSCS_Id && t.PACA_Id.Equals(masters[j].PACA_Id));
                           // result.UpdatedDate = indianTime;
                            result.PAOTSCS_StatusFlag = 1;
                            _precontext.Update(result);
                            _precontext.SaveChanges();
                        }

                        //List<LMS_Live_MeetingDMO> livemeetdelete = new List<LMS_Live_MeetingDMO>();
                        //livemeetdelete = _context.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_PlannedDate == schedule.FirstOrDefault().PAOTS_ScheduleDate && t.LMSLMEET_PlannedStartTime == masters[j].PAOTSS_Time && t.LMSLMEET_PlannedEndTime == masters[j].PAOTSS_Time_To).ToList();
                        //if (livemeetdelete.Any())
                        //{
                        //    _context.Remove(livemeetdelete.ElementAt(0));
                        //    _context.SaveChanges();
                        //}


                        if (deletedata > 0)
                        {
                            var student = _precontext.PA_College_Application.Where(d => d.PACA_Id.Equals(masters[j].PACA_Id)).ToList();

                            //Email Email = new Email(_context);
                            //string m = Email.sendmail(student.FirstOrDefault().MI_Id, student.FirstOrDefault().PASR_emailId, "DELETE_ORAL_TEST_SCHEDULE", masters[j].PACA_Id);

                            //SMS sms = new SMS(_context);
                            //sms.sendSms(student.FirstOrDefault().MI_Id, student.FirstOrDefault().PASR_MobileNo, "DELETE_ORAL_TEST_SCHEDULE", masters[j].PACA_Id);
                        }
                        j++;
                    }

                    List<OralTestScheduleClgDMO> masters1 = new List<OralTestScheduleClgDMO>();
                    masters1 = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_Id.Equals(ID)).ToList();
                    if (masters1.Any())
                    {
                        _precontext.Remove(masters1.ElementAt(0));
                        _precontext.SaveChanges();
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
                List<OralTestScheduleClgDMO> masters1 = new List<OralTestScheduleClgDMO>();
                masters1 = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_Id.Equals(ID)).ToList();
                if (masters1.Any())
                {
                    _precontext.Remove(masters1.ElementAt(0));
                    _precontext.SaveChanges();
                }
                else
                {

                }
            }
            return OralTestScheduleDTO;
        }

        public DocumentViewClgDTO checkaddparticipants(DocumentViewClgDTO data)
        {
            try
            {
                long studentmeeting = 0;
                var schedulelist = _precontext.OralTestScheduleClgDMO.Where(t => t.PAOTSC_Id == data.PAOTSC_Id).ToList();
                if (schedulelist.Count() > 0)
                {
                    var vcschedule = _db.LMS_Live_MeetingDMO.Where(t => t.LMSLMEET_MeetingTopic == schedulelist.FirstOrDefault().PAOTSC_ScheduleName && t.LMSLMEET_PlannedDate == schedulelist.FirstOrDefault().PAOTSC_ScheduleDate && t.LMSLMEET_PlannedStartTime == schedulelist.FirstOrDefault().PAOTSC_ScheduleFromTime).ToList();
                    if (vcschedule.Count > 0)
                    {
                        studentmeeting = vcschedule.FirstOrDefault().LMSLMEET_Id;
                    }
                }
                if (data.LMSLMEET_Id > 0)
                {
                    var deletstafflist = _db.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == studentmeeting).ToList();
                    if (deletstafflist.Count > 0)
                    {
                        foreach (var item in deletstafflist)
                        {
                            var stafflist = _db.LMS_Live_Meeting_StaffOthersDMO.Where(t => t.LMSLMEET_Id == data.LMSLMEET_Id && t.User_Id == item.User_Id).ToList();
                            if (stafflist.Count == 0)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                sobj.User_Id = Convert.ToInt64(item.User_Id);
                                sobj.HRME_Id = Convert.ToInt64(item.HRME_Id);
                                sobj.LMSLMEETSTFOTH_CreatedBy = Convert.ToInt64(data.user_id);
                                sobj.LMSLMEETSTFOTH_UpdatedBy = Convert.ToInt64(data.user_id);
                                sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                _db.Add(sobj);
                            }

                        }
                    }
                }
                int res = _db.SaveChanges();
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


        public async Task<DocumentViewClgDTO> scheduleGetreportdetails(DocumentViewClgDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViewPreadmission_schedulewise_ReportCollege";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@clgids",
                    SqlDbType.VarChar)
                    {
                        Value = data.PAOTSC_Id
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
                        data.schedulelist = retObject.ToArray();
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

        public async Task<DocumentViewClgDTO> remarksGetreportdetails(DocumentViewClgDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViewPreadmission_Remarkswise_ReportCollege";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@clgids",
                    SqlDbType.VarChar)
                    {
                        Value = data.PAOTSC_Id
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
                        data.remarkschedulelist = retObject.ToArray();
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
    }


}
