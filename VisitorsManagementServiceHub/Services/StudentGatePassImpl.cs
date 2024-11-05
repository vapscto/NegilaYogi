using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
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


namespace VisitorsManagementServiceHub.Services
{
    public class StudentGatePassImpl : Interfaces.StudentGatePassInterface
    {
        public DomainModelMsSqlServerContext _db;
        public VisitorsManagementContext _visctxt;
        public DomainModelMsSqlServerContext _context;
        private readonly UserManager<ApplicationUser> _UserManager;
        public StudentGatePassImpl(DomainModelMsSqlServerContext db, VisitorsManagementContext context, DomainModelMsSqlServerContext pad, UserManager<ApplicationUser> usermanager)
        {
            _db = db;
            _visctxt = context;
            _context = pad;
            _UserManager = usermanager;
        }
        public StudentGatePass_DTO getdetails(StudentGatePass_DTO dTO)
        {
            try
            {
                dTO.yearlist = _db.AcademicYear.Where(t => t.MI_Id == dTO.MI_Id && t.ASMAY_ActiveFlag == 1 && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                //dTO.alldata = (from a in _visctxt.Gate_Pass_Student_DMO
                //               from b in _visctxt.Adm_M_Student
                //               from c in _visctxt.School_Adm_Y_StudentDMO
                //               from d in _visctxt.admissionClass
                //               from e in _visctxt.masterSection
                //               where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && a.MI_Id == dTO.MI_Id  && b.AMST_SOL == "S" && c.AMAY_ActiveFlag == 1)
                //               select new StudentGatePass_DTO
                //               {
                //                   AMST_Id = a.AMST_Id,
                //                   GPHS_Id = a.GPHS_Id,
                //                   GPHS_GatePassNo = a.GPHS_GatePassNo,
                //                   GPHS_IDCardNo = a.GPHS_IDCardNo,
                //                   GPHS_DateTime = a.GPHS_DateTime,
                //                   GPHS_Remarks = a.GPHS_Remarks,
                //                   ASMCL_ClassName = d.ASMCL_ClassName,
                //                   ASMC_SectionName = e.ASMC_SectionName,
                //                   ASMCL_Id = d.ASMCL_Id,
                //                   ASMS_Id = e.ASMS_Id,
                //                   ASMAY_Id = c.ASMAY_Id,
                //                   studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) ? "" : ' ' + b.AMST_LastName),
                //                   AMST_AdmNo = b.AMST_AdmNo,
                //                   GPHS_ActiveFlg = a.GPHS_ActiveFlg,
                //                   GPHS_ReceiverName = a.GPHS_ReceiverName,
                //                   GPHS_ReceiverPhoneNo = a.GPHS_ReceiverPhoneNo,
                //                   GPHS_ReceiverIdProof = a.GPHS_ReceiverIdProof,
                //                   GPHS_ReceiverIdProofNo = a.GPHS_ReceiverIdProofNo,
                //                   GPHS_SentFlg = a.GPHS_SentFlg,
                //               }).Distinct().OrderByDescending(t => t.GPHS_Id).ToArray();



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VM_Student_GatePass_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dTO.MI_Id
                    });                 


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        dTO.alldata = retObject.ToArray();

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
            return dTO;
        }
        public StudentGatePass_DTO get_class(StudentGatePass_DTO dto)
        {
            try
            {
                dto.classList = (from a in _db.School_Adm_Y_StudentDMO
                                 from b in _db.School_M_Class
                                 where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true)
                                 select new StudentGatePass_DTO
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMCL_ClassName = b.ASMCL_ClassName
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public StudentGatePass_DTO get_section(StudentGatePass_DTO dto)
        {
            try
            {
                dto.SectionList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.School_M_Section
                                   where (a.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1)
                                   select new StudentGatePass_DTO
                                   {
                                       ASMS_Id = b.ASMS_Id,
                                       ASMC_SectionName = b.ASMC_SectionName
                                   }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public StudentGatePass_DTO get_student(StudentGatePass_DTO dto)
        {
            try
            {
                dto.StudentList = (from a in _db.School_Adm_Y_StudentDMO
                                   from c in _db.School_M_Class
                                   from s in _db.School_M_Section
                                   from b in _db.Adm_M_Student
                                   where (b.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                   select new StudentGatePass_DTO
                                   {
                                       AMST_Id = b.AMST_Id,
                                       studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                       AMST_AdmNo = b.AMST_AdmNo,
                                       ASMCL_Id = c.ASMCL_Id,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMS_Id = s.ASMS_Id,
                                       ASMC_SectionName = s.ASMC_SectionName
                                   }).Distinct().OrderBy(b => b.studentname).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<StudentGatePass_DTO> saverecordAsync(StudentGatePass_DTO data)
        {
            try
            {
                string genotp = "";
                string studSecretCode = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();

                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "StudentGatePass").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }
                if (data.GPHS_Id == 0)
                {
                    ApplicationUser user = new ApplicationUser();
                    var studentdata = (from t in _context.SchoolYearWiseStudent
                                       from a in _context.Adm_M_Student
                                       where (t.AMST_Id == a.AMST_Id && a.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.ASMAY_Id == data.ASMAY_Id)
                                       select a).Distinct().ToList();

                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    //genotp = generate.getOTP();

                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    }

                    var Duplicate = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.GPHS_GatePassNo == data.trans_id && t.GPHS_IDCardNo == data.GPHS_IDCardNo && t.GPHS_DateTime == data.GPHS_DateTime).ToList();

                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        string SecretCode = "";
                        var checkSecretcode = (from b in _visctxt.Adm_M_Student
                                               where (b.MI_Id == data.MI_Id && b.AMST_Id == data.AMST_Id /*amst_ids*/
                                               && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                               select new StudentGatePass_DTO
                                               {
                                                   AMST_SecretCode = b.AMST_SecretCode,
                                               }).Distinct().ToList();

                        SecretCode = checkSecretcode.SingleOrDefault().AMST_SecretCode;
                        if (SecretCode == null || SecretCode == "")
                        {
                            studSecretCode = generate.getFourDigitOTP();

                            var stud = _visctxt.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).Single();

                            stud.AMST_SecretCode = studSecretCode;
                            stud.UpdatedDate = DateTime.Now;

                            _visctxt.Update(stud);
                        }

                        Gate_Pass_Student_DMO obj = new Gate_Pass_Student_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.AMST_Id = data.AMST_Id;
                        obj.GPHS_GatePassNo = data.trans_id;
                        obj.GPHS_IDCardNo = data.GPHS_IDCardNo;
                        obj.GPHS_DateTime = data.GPHS_DateTime;
                        obj.GPHS_Remarks = data.GPHS_Remarks;
                        obj.GPHS_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.GPHS_CreatedBy = data.UserId;
                        obj.GPHS_UpdatedBy = data.UserId;
                        obj.GPHS_ReceiverName = data.GPHS_ReceiverName;
                        obj.GPHS_ReceiverPhoneNo = data.GPHS_ReceiverPhoneNo;
                        obj.GPHS_ReceiverIdProof = data.GPHS_ReceiverIdProof;
                        obj.GPHS_ReceiverIdProofNo = data.GPHS_ReceiverIdProofNo;
                        obj.GPHS_SecretCode = studSecretCode;
                        obj.GPHS_OTP = data.newotpsave;
                        obj.GPHS_SentFlg = false;
                        _visctxt.Add(obj);
                        int rowAffected = _visctxt.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            var rowdata = _context.Gate_Pass_Student_DMO.OrderByDescending(d => d.GPHS_Id).First();
                            long? mobileNo = studentdata.SingleOrDefault().AMST_MobileNo;
                            long AMST_Id = studentdata.SingleOrDefault().AMST_Id;
                            long UserID = rowdata.GPHS_Id;

                            /* SMS FOR DRIVER AND GENERAL MANAGER WHEN GATE PASS GENERATE FOR STUDENT */
                            long? Driver_mobileno = 0;
                            var check_template = _context.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id
                            && a.ISES_Template_Name == "Student_GatePass_Driver_GM").ToList();

                            if (check_template.Count > 0)
                            {
                                var Get_Driver_Details = (from a in _visctxt.TR_Student_RouteDMO
                                                          from b in _visctxt.VehicleRouteDMo
                                                          from c in _visctxt.VehicleDriver
                                                          from d in _visctxt.MasterDriverDMO
                                                          where (a.TRMR_Drop_Route == b.TRMR_Id && c.TRMV_Id == b.TRMV_Id && d.TRMD_Id == c.TRMD_Id
                                                          && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.TRSR_ActiveFlg == true
                                                          && b.TRVR_ActiveFlg == true && c.TRVD_ActiveFlg == true && d.TRMD_ActiveFlg == true
                                                          && c.TRVD_Date.Date <= indianTime.Date)
                                                          select new StudentGatePass_DTO
                                                          {
                                                              driver_mobileno = d.TRMD_MobileNo,
                                                              driver_emailid = d.TRMD_EmailId,
                                                              vehicle_date = c.TRVD_Date
                                                          }).Distinct().OrderByDescending(a => a.vehicle_date).Take(1).ToList();

                                if (Get_Driver_Details.Count > 0)
                                {
                                    Driver_mobileno = Get_Driver_Details.FirstOrDefault().driver_mobileno;
                                }


                                string s = await SendSMSForMobile_Student_GatePass_Driver_GM(data.MI_Id, Convert.ToInt64(Driver_mobileno), "Student_GatePass_Driver_GM",
                                     data.ASMAY_Id, AMST_Id);
                            }
                            else
                            {
                                data.returnval = true;
                            }

                            //SMS sms = new SMS(_context);
                            //string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(genotp), UserID, AMST_Id);

                            //string s = await SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(genotp), UserID, AMST_Id);
                            //data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.GPHS_Id > 0)
                {
                    var Duplicate = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id != data.GPHS_Id && t.AMST_Id == data.AMST_Id && t.GPHS_GatePassNo == data.GPHS_GatePassNo && t.GPHS_IDCardNo == data.GPHS_IDCardNo && t.GPHS_DateTime == data.GPHS_DateTime).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.GPHS_Id == data.GPHS_Id);
                        //result.AMST_Id = data.AMST_Id;
                        result.GPHS_IDCardNo = data.GPHS_IDCardNo;
                        result.GPHS_DateTime = data.GPHS_DateTime;
                        result.GPHS_Remarks = data.GPHS_Remarks;
                        result.UpdatedDate = DateTime.Now;
                        result.GPHS_UpdatedBy = data.UserId;
                        result.GPHS_ReceiverName = data.GPHS_ReceiverName;
                        result.GPHS_ReceiverPhoneNo = data.GPHS_ReceiverPhoneNo;
                        result.GPHS_ReceiverIdProof = data.GPHS_ReceiverIdProof;
                        result.GPHS_ReceiverIdProofNo = data.GPHS_ReceiverIdProofNo;

                        _visctxt.Update(result);
                        int rowAffected = _visctxt.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO deactive(StudentGatePass_DTO data)
        {
            try
            {
                var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.GPHS_Id == data.GPHS_Id && t.MI_Id == data.MI_Id);

                if (result.GPHS_ActiveFlg == true)
                {
                    result.GPHS_ActiveFlg = false;
                }
                else if (result.GPHS_ActiveFlg == false)
                {
                    result.GPHS_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _visctxt.Update(result);
                int rowAffected = _visctxt.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO editrecord(StudentGatePass_DTO data)
        {
            try
            {
                data.editlist = (from a in _visctxt.Gate_Pass_Student_DMO
                                 from b in _visctxt.Adm_M_Student
                                 from c in _visctxt.School_Adm_Y_StudentDMO
                                 from d in _visctxt.admissionClass
                                 from e in _visctxt.masterSection
                                 where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && a.GPHS_Id == data.GPHS_Id && b.AMST_SOL == "S" && c.AMAY_ActiveFlag == 1)
                                 select new StudentGatePass_DTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     GPHS_Id = a.GPHS_Id,
                                     GPHS_GatePassNo = a.GPHS_GatePassNo,
                                     GPHS_IDCardNo = a.GPHS_IDCardNo,
                                     GPHS_DateTime = a.GPHS_DateTime,
                                     GPHS_Remarks = a.GPHS_Remarks,
                                     ASMCL_Id = d.ASMCL_Id,
                                     ASMS_Id = e.ASMS_Id,
                                     ASMAY_Id = c.ASMAY_Id,
                                     GPHS_ReceiverName = a.GPHS_ReceiverName,
                                     GPHS_ReceiverPhoneNo = a.GPHS_ReceiverPhoneNo,
                                     GPHS_ReceiverIdProof = a.GPHS_ReceiverIdProof,
                                     GPHS_ReceiverIdProofNo = a.GPHS_ReceiverIdProofNo,

                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO checkstudentdata(StudentGatePass_DTO data)
        {
            try
            {
                var singlestudent = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).ToList();
                data.singlestudentdata = singlestudent.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO get_otpverification(StudentGatePass_DTO data)
        {
            try
            {
                string otp = "";

                var gatepassid = (from a in _visctxt.Gate_Pass_Student_DMO
                                  where (a.GPHS_Id == data.GPHS_Id)
                                  select new StudentGatePass_DTO
                                  {
                                      GPHS_OTP = a.GPHS_OTP
                                  }).Distinct().ToList();

                otp = gatepassid.Select(t => t.GPHS_OTP).Single();

                if (otp == data.GPHS_OTP)
                {
                    DateTime datechkkkkk = new DateTime();
                    datechkkkkk = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).UpdatedDate;
                    string checkmobnokk = datechkkkkk.ToString();

                    if (checkmobnokk != "")
                    {
                        DateTime newdate = DateTime.Now;
                        TimeSpan ts = DateTime.Now - datechkkkkk;
                        //double ts2 = ts.TotalHours;
                        double t22 = ts.TotalMinutes;

                        if (15 <= t22)
                        {
                            data.message = "Your Time is Over. Please Resend Your OTP!";
                        }
                        else
                        {
                            var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);

                            result.GPHS_SentFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            result.GPHS_UpdatedBy = data.UserId;
                            _visctxt.Update(result);
                            int rowAffected = _visctxt.SaveChanges();

                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                    else
                    {
                        data.message = "Please Register Your Mobile Number!";
                    }
                }
                else
                {
                    data.message = "Please Enter Valid OTP Number!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentGatePass_DTO> resendotp(StudentGatePass_DTO data)
        {
            try
            {
                string studSecretCode = "";
                if (data.GPHS_Id > 0)
                {
                    DateTime datechkkkkk = new DateTime();
                    datechkkkkk = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).UpdatedDate;
                    string checkmobnokk = datechkkkkk.ToString();
                    if (checkmobnokk != "")
                    {
                        DateTime newdate = DateTime.Now;
                        TimeSpan ts = DateTime.Now - datechkkkkk;
                        //double ts2 = ts.TotalHours;
                        double t22 = ts.TotalMinutes;

                        if (15 <= t22)
                        {
                            string genotp = "";
                            ApplicationUser user = new ApplicationUser();

                            var gphsid = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).Select(t => t.AMST_Id).Distinct().ToList();
                            var studentdata = (from x in _context.Adm_M_Student
                                               where (x.MI_Id == data.MI_Id && gphsid.Contains(x.AMST_Id)
                                               && x.AMST_SOL == "S")
                                               select x).Distinct().ToList();
                            long? mobileNo = new long?();
                            mobileNo = studentdata.SingleOrDefault().AMST_MobileNo;
                            //mobileNo = Convert.ToInt64("9771237044");
                            long AMST_Id = studentdata.SingleOrDefault().AMST_Id;
                            long UserID = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).SingleOrDefault().GPHS_Id;
                            CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                            genotp = generate.getOTP();

                            //SMS sms = new SMS(_context);
                            //string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(genotp), UserID, AMST_Id);

                            string s = await SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", genotp, UserID, AMST_Id);

                            data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";

                            string SecretCode = "";
                            var amstids = (from t in _visctxt.Gate_Pass_Student_DMO
                                           where (t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id)
                                           select new StudentGatePass_DTO
                                           {
                                               AMST_Id = t.AMST_Id
                                           }).Distinct().ToList();
                            long amst_ids = amstids.SingleOrDefault().AMST_Id;
                            var checkSecretcode = (from b in _visctxt.Adm_M_Student
                                                   where (b.MI_Id == data.MI_Id && b.AMST_Id.Equals(amst_ids) && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                                   select new StudentGatePass_DTO
                                                   {
                                                       AMST_SecretCode = b.AMST_SecretCode,
                                                   }).Distinct().ToList();

                            SecretCode = checkSecretcode.Select(t => t.AMST_SecretCode).SingleOrDefault();
                            if (SecretCode == null || SecretCode == "")
                            {
                                studSecretCode = generate.getFourDigitOTP();

                                var stud = _visctxt.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id.Equals(amst_ids)).SingleOrDefault();

                                stud.AMST_SecretCode = studSecretCode;
                                stud.UpdatedDate = DateTime.Now;

                                _visctxt.Update(stud);
                            }

                            var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);

                            result.UpdatedDate = DateTime.Now;
                            result.GPHS_UpdatedBy = data.UserId;
                            result.GPHS_OTP = genotp;

                            _visctxt.Update(result);
                            int rowAffected = _visctxt.SaveChanges();

                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            var mobileotp = (from t in _visctxt.Gate_Pass_Student_DMO where (t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id) select t).Distinct().ToList();

                            var gphsid22 = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).Select(t => t.AMST_Id).Distinct().ToList();
                            var studentdata22 = (from f in _context.Adm_M_Student
                                                 where (f.MI_Id == data.MI_Id && gphsid22.Contains(f.AMST_Id)
                                                 && f.AMST_SOL == "S")
                                                 select f).Distinct().ToList();

                            long? mobileNo = studentdata22.SingleOrDefault().AMST_MobileNo;
                            //mobileNo = Convert.ToInt64("9581586484");
                            long AMST_Id = studentdata22.SingleOrDefault().AMST_Id;
                            string gatepassotp = mobileotp.SingleOrDefault().GPHS_OTP;

                            long UserID = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).SingleOrDefault().GPHS_Id;

                            //SMS sms = new SMS(_context);
                            //string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(gatepassotp), UserID, AMST_Id);

                            string s = await SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", gatepassotp, UserID, AMST_Id);

                            data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";
                        }
                    }
                    else
                    {
                        data.message = "Please Register Your Mobile Number!";
                    }
                }

                var getotpid = _context.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).Select(t => t.GPHS_Id).Distinct().ToList();
                data.otpid = getotpid.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO get_otpverification22(StudentGatePass_DTO data)
        {
            try
            {
                string otp = "";
                var gatepassid = (from a in _visctxt.Gate_Pass_Student_DMO
                                  where (a.GPHS_Id == data.GPHS_Id)
                                  select new StudentGatePass_DTO
                                  {
                                      GPHS_OTP = a.GPHS_OTP
                                  }).Distinct().ToList();

                otp = gatepassid.Select(t => t.GPHS_OTP).Single();

                if (otp == data.GPHS_OTP)
                {
                    DateTime datechkkkkk = new DateTime();
                    datechkkkkk = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).UpdatedDate;
                    string checkmobnokk = datechkkkkk.ToString();
                    if (checkmobnokk != "")
                    {
                        DateTime newdate = DateTime.Now;
                        TimeSpan ts = DateTime.Now - datechkkkkk;
                        //double ts2 = ts.TotalHours;
                        double t22 = ts.TotalMinutes;
                        if (15 <= t22)
                        {
                            data.message = "Your Time is Over. Again Resend Your OTP!";
                        }
                        else
                        {
                            var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);
                            result.GPHS_SentFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            result.GPHS_UpdatedBy = data.UserId;
                            _visctxt.Update(result);
                            int rowAffected = _visctxt.SaveChanges();
                            if (rowAffected > 0)
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
                else
                {
                    data.message = "Please Enter Valid OTP Number!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO printbutton(StudentGatePass_DTO data)
        {
            try
            {
                #region
                //var  currentstuddatalist = (from a in _visctxt.Gate_Pass_Student_DMO
                //                        from b in _visctxt.Adm_M_Student
                //                        from y in _visctxt.School_Adm_Y_StudentDMO
                //                        from c in _visctxt.admissionClass
                //                        from s in _visctxt.masterSection
                //                        where (a.GPHS_Id == data.GPHS_Id && y.ASMS_Id == s.ASMS_Id
                //                        && a.MI_Id==data.MI_Id && a.AMST_Id == b.AMST_Id && y.AMST_Id == a.AMST_Id
                //                        && y.ASMAY_Id == data.ASMAY_Id && b.MI_Id==a.MI_Id && y.ASMCL_Id == c.ASMCL_Id )
                //                        select new StudentGatePass_DTO
                //                        {
                //                            GPHS_Id = a.GPHS_Id,
                //                            GPHS_GatePassNo = a.GPHS_GatePassNo,
                //                            GPHS_IDCardNo = a.GPHS_IDCardNo,
                //                            GPHS_DateTime = a.GPHS_DateTime,
                //                            GPHS_Remarks = a.GPHS_Remarks,
                //                            studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) ? "" : ' ' + b.AMST_LastName),
                //                            AMST_AdmNo = b.AMST_AdmNo,
                //                            ASMC_SectionName = s.ASMC_SectionName,
                //                            ASMCL_ClassName = c.ASMCL_ClassName,
                //                            AMST_MobileNo = b.AMST_MobileNo,
                //                            GPHS_ReceiverName = a.GPHS_ReceiverName,
                //                            GPHS_ReceiverPhoneNo = a.GPHS_ReceiverPhoneNo,
                //                            GPHS_ReceiverIdProof = a.GPHS_ReceiverIdProof,
                //                            GPHS_ReceiverIdProofNo = a.GPHS_ReceiverIdProofNo,
                //                            GPHS_SentFlg = a.GPHS_SentFlg,
                //                            AMST_Photoname = b.AMST_Photoname,

                //                        }).Distinct().OrderByDescending(t => t.GPHS_Id).ToList();

                //data.currentstuddata = currentstuddatalist.ToArray();
                #endregion

                using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VM_GATE_PASS_DETAILS_PRINT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@GPHS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.GPHS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
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
                        data.currentstuddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();

                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                string accountname = "", accesskey = "", html = "";
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }
                #region
                //if (currentstuddatalist.Count > 0)
                //{
                //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + data.MI_Id, currentstuddatalist[0].AMC_PAApplicationName + ".html", 0);
                //}
                //else
                //{
                //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + data.MI_Id, "Admissionform.html", 0);
                //}
                //string pathToHTMLFile = @"E:\Aman\2019 Code\November\IVRM_VS_Code_19112019\IVRM_VS_19th_NOV_2019\IVRMUX\wwwroot\StudentGatePassJSH.html";
                //string pathToHTMLFile = @"E:\Aman\2019 Code\November\IVRM_VS_Code_19112019\IVRM_VS_19th_NOV_2019\IVRMUX\wwwroot\gatepassstudent.html";
                //data.htmldata = System.IO.File.ReadAllText(pathToHTMLFile);
                #endregion
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "gatepass/" + data.MI_Id, "StudentGatePass.html", 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                data.htmldata = html;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentGatePass_DTO> getotp(StudentGatePass_DTO data)
        {
            try
            {
                var genotp = "";

                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();

                data.genotp = genotp;

                var studentdata = (from t in _context.SchoolYearWiseStudent
                                   from a in _context.Adm_M_Student
                                   where (t.AMST_Id == a.AMST_Id && a.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.ASMAY_Id == data.ASMAY_Id)
                                   select new StudentGatePass_DTO
                                   {
                                       AMST_MobileNo = a.AMST_MobileNo,
                                       AMST_Id = a.AMST_Id
                                   }).Distinct().ToList();


                long? mobileNo = studentdata.SingleOrDefault().AMST_MobileNo;
                //mobileNo = Convert.ToInt64("9581586484");
                long AMST_Id = studentdata.SingleOrDefault().AMST_Id;

                //SMS sms = new SMS(_context);
                //string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(genotp), UserID, AMST_Id);

                string s = await SendSMSForMobilewithoutuserid(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", genotp, data.ASMAY_Id, data.AMST_Id, data.GPHS_Remarks);

                data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<string> SendSMSForMobile(long MI_Id, long mobileNo, string Template, string OTP, long UserID, long AMST_Id)
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
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STUDENT_GATEPASS_SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                           SqlDbType.BigInt)
                        {
                            Value = AMST_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@template",
                        //   SqlDbType.VarChar)
                        //{
                        //    Value = Template
                        //});
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
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
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
                    //for (int j = 0; j < ParamaetersName.Count; j++)
                    //{
                    //    for (int p = 0; p < val.Count; p++)
                    //    {
                    //        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                    //        {
                    //            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                    //            sms = result;
                    //        }
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
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

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


                    if (template[0].ISES_EnableSMSCCFlg == true && template[0].ISES_SMSCCMobileNo != "" && template[0].ISES_SMSCCMobileNo != null)
                    {
                        string[] ccmobileno = template[0].ISES_SMSCCMobileNo.Split(',');
                        foreach (var c in ccmobileno)
                        {
                            if (c != PHNO)
                            {
                                string urlcc = alldetails[0].IVRMSD_URL.ToString();
                                string PHNOcc = c.ToString();
                                urlcc = urlcc.Replace("PHNO", PHNOcc);
                                urlcc = urlcc.Replace("MESSAGE", sms);
                                urlcc = urlcc.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                                urlcc = urlcc.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                                System.Net.HttpWebRequest requestcc = System.Net.WebRequest.Create(urlcc) as HttpWebRequest;
                                System.Net.HttpWebResponse responsecc = await requestcc.GetResponseAsync() as System.Net.HttpWebResponse;
                                Stream streamcc = responsecc.GetResponseStream();
                                StreamReader readStreamcc = new StreamReader(streamcc, Encoding.UTF8);
                                string responseparameterscc = readStreamcc.ReadToEnd();
                                var myContentcc = JsonConvert.SerializeObject(responseparameterscc);
                                dynamic responsedatacc = JsonConvert.DeserializeObject(myContentcc);
                            }
                        }

                    }


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

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_StudentGatePass";
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
                            Value = "Visitors Management"
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
                        cmd.Parameters.Add(new SqlParameter("@To_Flag",
              SqlDbType.VarChar)
                        {
                            Value = "StudentGatePass"
                        });
                        cmd.Parameters.Add(new SqlParameter("@System_Ip",
                   SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@network_Ip",
                SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });
                        cmd.Parameters.Add(new SqlParameter("@MacAddress_Ip",
              SqlDbType.VarChar)
                        {
                            Value = myIP1
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
        public async Task<string> SendSMSForMobilewithoutuserid(long MI_Id, long mobileNo, string Template, string OTP, long ASMAY_Id, long AMST_Id, string REMARKS)
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
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STUDENT_GATEPASS_SMSMAILPARAMETER_WITHOUT_USERID";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@OTP", SqlDbType.VarChar) { Value = OTP });
                        cmd.Parameters.Add(new SqlParameter("@REMARKS", SqlDbType.VarChar) { Value = REMARKS });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = ASMAY_Id });

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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
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

                    string PHNO = mobileNo.ToString();

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


                    if (template[0].ISES_EnableSMSCCFlg == true && template[0].ISES_SMSCCMobileNo != "" && template[0].ISES_SMSCCMobileNo != null)
                    {
                        string[] ccmobileno = template[0].ISES_SMSCCMobileNo.Split(',');
                        foreach (var c in ccmobileno)
                        {
                            if (c != PHNO)
                            {
                                string urlcc = alldetails[0].IVRMSD_URL.ToString();
                                string PHNOcc = c.ToString();
                                urlcc = urlcc.Replace("PHNO", PHNOcc);
                                urlcc = urlcc.Replace("MESSAGE", sms);
                                urlcc = urlcc.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                                urlcc = urlcc.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                                System.Net.HttpWebRequest requestcc = System.Net.WebRequest.Create(urlcc) as HttpWebRequest;
                                System.Net.HttpWebResponse responsecc = await requestcc.GetResponseAsync() as System.Net.HttpWebResponse;
                                Stream streamcc = responsecc.GetResponseStream();
                                StreamReader readStreamcc = new StreamReader(streamcc, Encoding.UTF8);
                                string responseparameterscc = readStreamcc.ReadToEnd();
                                var myContentcc = JsonConvert.SerializeObject(responseparameterscc);
                                dynamic responsedatacc = JsonConvert.DeserializeObject(myContentcc);
                            }
                        }
                    }

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

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_StudentGatePass";
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
                            Value = "Visitors Management"
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
                        cmd.Parameters.Add(new SqlParameter("@To_Flag",
              SqlDbType.VarChar)
                        {
                            Value = "StudentGatePass"
                        });
                        cmd.Parameters.Add(new SqlParameter("@System_Ip",
                   SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@network_Ip",
                SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });
                        cmd.Parameters.Add(new SqlParameter("@MacAddress_Ip",
              SqlDbType.VarChar)
                        {
                            Value = myIP1
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
        public async Task<string> SendSMSForMobile_Student_GatePass_Driver_GM(long MI_Id, long mobileNo, string Template, long ASMAY_Id, long AMST_Id)
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
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STUDENT_GATEPASS_SMSMAILPARAMETER_WITHOUT_USERID";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@OTP", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@REMARKS", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = ASMAY_Id });

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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
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
                    string messageid = "";
                    string PHNO = mobileNo.ToString();
                    if (mobileNo.ToString().Length == 10)
                    {
                        string url = alldetails[0].IVRMSD_URL.ToString();

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
                        messageid = responsedata;
                        try
                        {
                            string s = await Store_SMS_Sent_Details(MI_Id, Template, PHNO, sms, messageid);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                    }

                    if (template[0].ISES_EnableSMSCCFlg == true && template[0].ISES_SMSCCMobileNo != "" && template[0].ISES_SMSCCMobileNo != null)
                    {
                        string[] ccmobileno = template[0].ISES_SMSCCMobileNo.Split(',');
                        foreach (var c in ccmobileno)
                        {
                            if (c != PHNO)
                            {
                                string urlcc = alldetails[0].IVRMSD_URL.ToString();
                                string PHNOcc = c.ToString();
                                urlcc = urlcc.Replace("PHNO", PHNOcc);
                                urlcc = urlcc.Replace("MESSAGE", sms);
                                urlcc = urlcc.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                                urlcc = urlcc.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                                System.Net.HttpWebRequest requestcc = System.Net.WebRequest.Create(urlcc) as HttpWebRequest;
                                System.Net.HttpWebResponse responsecc = await requestcc.GetResponseAsync() as System.Net.HttpWebResponse;
                                Stream streamcc = responsecc.GetResponseStream();
                                StreamReader readStreamcc = new StreamReader(streamcc, Encoding.UTF8);
                                string responseparameterscc = readStreamcc.ReadToEnd();
                                var myContentcc = JsonConvert.SerializeObject(responseparameterscc);
                                dynamic responsedatacc = JsonConvert.DeserializeObject(myContentcc);
                                string messageidcc = responsedatacc;

                                try
                                {
                                    string s = await Store_SMS_Sent_Details(MI_Id, Template, PHNOcc, sms, messageidcc);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
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
        public async Task<StudentGatePass_DTO> GetStudDetails(StudentGatePass_DTO data)
        {
            try
            {
                #region Student Details
                using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STUDENT_GATE_PASS_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.detailsforstudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                var duplicate = (from a in _context.Gate_Pass_Student_DMO
                                 where (a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.GPHS_DateTime.Value.Date == data.GPHS_DateTime.Value.Date
                                 && a.GPHS_ActiveFlg == true)
                                 select a).Distinct().ToList();
                if (duplicate.Count > 0)
                {
                    data.message = "Gate Pass Already Generated!";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public async Task<string> Store_SMS_Sent_Details(long MI_Id, string Template, string PHNO, string sms, string messageid)
        {
            try
            {
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

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    cmd.CommandText = "IVRM_SMS_Outgoing_StudentGatePass";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.NVarChar)
                    {
                        Value = PHNO
                    });
                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                    {
                        Value = sms
                    });
                    cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                    {
                        Value = "Visitors Management"
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar)
                    {
                        Value = "Delivered"
                    });

                    cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar)
                    {
                        Value = messageid
                    });
                    cmd.Parameters.Add(new SqlParameter("@To_Flag", SqlDbType.VarChar)
                    {
                        Value = "StudentGatePass"
                    });
                    cmd.Parameters.Add(new SqlParameter("@System_Ip", SqlDbType.VarChar)
                    {
                        Value = remoteIpAddress
                    });

                    cmd.Parameters.Add(new SqlParameter("@network_Ip", SqlDbType.VarChar)
                    {
                        Value = sMacAddress
                    });
                    cmd.Parameters.Add(new SqlParameter("@MacAddress_Ip", SqlDbType.VarChar)
                    {
                        Value = myIP1
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
    }
}