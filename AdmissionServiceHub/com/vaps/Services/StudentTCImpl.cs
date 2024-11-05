using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using CommonLibrary;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Alumni;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentTCImpl : Interfaces.StudentTCInterface
    {
        public DomainModelMsSqlServerContext _db;
        public ActivateDeactivateContext _act;
        ILogger<StudentTCImpl> _tcimpl;
        public StudentTCImpl(DomainModelMsSqlServerContext db, ActivateDeactivateContext act, ILogger<StudentTCImpl> _impltc)
        {
            _db = db;
            _act = act;
            _tcimpl = _impltc;
        }

        public async Task<StudentTCDTO> GetStudentInitialData(StudentTCDTO DTO)
        {
            StudentTCDTO attdto = new StudentTCDTO();
            List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
            admissionconfigurationsettings = _db.AdmissionStandardDMO.AsNoTracking().Where(t => t.MI_Id == DTO.MI_Id).ToList();
            attdto.admissioncongigurationList = admissionconfigurationsettings.ToArray();

            List<StudentTCDTO> ln = new List<StudentTCDTO>();

            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _db.AcademicYear.Where(t => t.MI_Id == DTO.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            attdto.academicList = allyear.ToArray();

            attdto.currentYear = _db.AcademicYear.Where(a => a.MI_Id == DTO.MI_Id && a.Is_Active == true && a.ASMAY_Id == DTO.ASMAY_Id).ToArray();

            List<School_M_Class> allclass = new List<School_M_Class>();
            allclass = _db.admissioncls.Where(s => s.MI_Id == DTO.MI_Id && s.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToList();
            attdto.classlist = allclass.ToArray();

            List<School_M_Section> allsection = new List<School_M_Section>();
            allsection = _db.Section.Where(y => y.MI_Id == DTO.MI_Id && y.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToList();
            attdto.SectionList = allsection.ToArray();

            attdto.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == attdto.MI_Id && t.IMN_Flag.Equals("tcno")).ToArrayAsync();

            //getting payment type for tc
            attdto.tcpermanentpayment = await _db.AdmissionStandardDMO.Where(t => t.MI_Id == attdto.MI_Id).ToArrayAsync();

            attdto.classlistnew = _db.School_M_Class.Where(a => a.MI_Id == attdto.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();
            attdto.Qualifiedclass = _db.Master_ExamQualified_ClassDMO.Where(q => q.MI_ID == attdto.MI_Id).ToArray();
            

            return attdto;
        }
        public StudentTCDTO getStatusDetails(StudentTCDTO status)
        {
            List<StudentTCDTO> ln = new List<StudentTCDTO>();
            StudentTCDTO attdto = new StudentTCDTO();
            return attdto;
        }
        public StudentTCDTO gettcDetails(StudentTCDTO MIID)
        {
            StudentTCDTO studDto = new StudentTCDTO();

            //************ CHECKING THE TC APPROVAL PROCESS ************//

            var gettcapprovadetails = _db.Adm_Certificates_Apply_DMO.Where(a => a.MI_Id == MIID.MI_Id && a.ACERTAPP_ActiveFlg == true
            && a.ACERTAPP_CertificateCode == "TC").ToList();

            if (gettcapprovadetails.Count > 0)
            {
                studDto.getapprovalmasterdetails = gettcapprovadetails.ToArray();

                if (gettcapprovadetails.FirstOrDefault().ACERTAPP_ApprovaReqlFlg == true)
                {
                    var getapprovadetails = _db.Adm_Students_Certificate_Apply_DMO.Where(a => a.MI_Id == MIID.MI_Id && a.AMST_Id == MIID.AMST_Id
                     && a.ASCA_ActiveFlg == true && a.ASCA_CertificateType == "TC" && a.ASMAY_Id == MIID.ASMAY_Id).ToList();

                    if (getapprovadetails.Count > 0)
                    {
                        studDto.getstudentapplieddetails = getapprovadetails.ToArray();

                        var getapprovalresultdetils = _db.Adm_Students_Certificate_Approve_DMO.Where(a => a.MI_Id == MIID.MI_Id
                         && a.ASCA_Id == getapprovadetails.FirstOrDefault().ASCA_Id && a.ASCAP_ActiveFlg == true).ToList();

                        studDto.getapprovalresultdetails = getapprovalresultdetils.ToArray();
                    }
                }
            }

            // CHECK TC GENERATED OR NOT

            var tcgeneratedornot = _db.Student_TC.Where(a => a.MI_Id == MIID.MI_Id && a.AMST_Id == MIID.AMST_Id && a.ASMAY_Id == MIID.ASMAY_Id
            && a.ASTC_DeletedFlag != true).ToList();

            if (tcgeneratedornot.Count > 0)
            {
                studDto.tcgeneratedornot = "Generated";
            }
            else
            {
                studDto.tcgeneratedornot = "";
            }


            // STUDENT GENERAL DETAILS
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    List<StudentTCDTO> stud = new List<StudentTCDTO>();
                    cmd.CommandText = "StudentTcDetailsById";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (MIID.AMST_Id > 0 && MIID.Status_flag != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt) { Value = MIID.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = MIID.Status_flag });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MIID.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = MIID.ASMAY_Id });
                    }
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

                        studDto.studentListById = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            //sp for attendance
            try
            {
                List<StudentTCDTO> result = new List<StudentTCDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Get_Toalclassheld_Attendance_Student_TC";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MIID.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = Convert.ToString(MIID.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.VarChar) { Value = Convert.ToString(MIID.AMST_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMST_SOL", SqlDbType.VarChar) { Value = MIID.Status_flag });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                result.Add(new StudentTCDTO
                                {
                                    countclass = Convert.ToInt32(dataReader1["Classes"]),
                                    attclasscount = Convert.ToInt32(dataReader1["Attendance"])
                                });

                            }
                        }
                        if (result.Count > 0)
                        {
                            studDto.countclass = result.FirstOrDefault().countclass;
                            studDto.attclasscount = result.FirstOrDefault().attclasscount;
                        }
                        else
                        {
                            //studDto.message = "Please Enter The Number Of Class Held For Particular Month In Master Class Held";
                        }
                    }
                    catch (Exception ex)
                    {
                        _tcimpl.LogInformation("TC attendance count sp error :'" + ex.Message + "'");
                    }
                }

                var dataatt = (from a in _db.Adm_studentAttendance
                               from b in _db.Adm_studentAttendanceStudents
                               where a.ASA_Id == b.ASA_Id && a.MI_Id == MIID.MI_Id && b.AMST_Id == MIID.AMST_Id && a.ASMAY_Id == MIID.ASMAY_Id
                               select new StudentTCDTO
                               {
                                   todateatt = a.ASA_ToDate
                               }).Distinct().ToList();

                studDto.todateatt = dataatt.Max(x => x.todateatt);
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC attendance count: '" + ex.Message + "'");
            }

            // Exam Details
            try
            {
                //studDto.get_elective_subjects = (from a in _db.StudentMappingDMO
                //                                 from b in _db.AcademicYear
                //                                 from c in _db.Adm_M_Student
                //                                 from d in _db.SchoolYearWiseStudent
                //                                 from e in _db.MasterSubjectList
                //                                 where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id
                //                                 && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id && a.ESTSU_ElecetiveFlag == true
                //                                 && e.ISMS_LanguageFlg == 0)
                //                                 select new StudentTCDTO
                //                                 {
                //                                     subjectname = e.ISMS_SubjectName,
                //                                     order = e.ISMS_OrderFlag
                //                                 }).Distinct().OrderBy(a => a.order).ToArray();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "TC_Generation";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = MIID.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = MIID.ASMAY_Id });


                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = MIID.AMST_Id });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(                                        dataReader.GetName(iFiled1),                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }                        studDto.get_elective_subjects = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

                studDto.get_elective_subjects_language = (from a in _db.StudentMappingDMO
                                                          from b in _db.AcademicYear
                                                          from c in _db.Adm_M_Student
                                                          from d in _db.SchoolYearWiseStudent
                                                          from e in _db.MasterSubjectList
                                                          where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id
                                                          && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id
                                                          && a.ESTSU_ElecetiveFlag == true && e.ISMS_LanguageFlg == 1)
                                                          select new StudentTCDTO
                                                          {
                                                              subjectname = e.ISMS_SubjectName,
                                                              order = e.ISMS_OrderFlag
                                                          }).Distinct().OrderBy(a => a.order).ToArray();

                studDto.get_elective_subjects_common = (from a in _db.StudentMappingDMO
                                                        from b in _db.AcademicYear
                                                        from c in _db.Adm_M_Student
                                                        from d in _db.SchoolYearWiseStudent
                                                        from e in _db.MasterSubjectList
                                                        where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id
                                                        && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id)
                                                        select new StudentTCDTO
                                                        {
                                                            subjectname = e.ISMS_SubjectName,
                                                            order = e.ISMS_OrderFlag
                                                        }).Distinct().OrderBy(a => a.order).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_student_Get_Exam_Details_Tc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (MIID.AMST_Id > 0 && MIID.Status_flag != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MIID.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = MIID.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt) { Value = MIID.AMST_Id });
                    }
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

                        studDto.getexamdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC Exam count: '" + ex.Message + "'");
            }

            //Fee Due
            try
            {
                List<StudentTCDTO> result = new List<StudentTCDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "StudentTcFeeDetailsById";
                    cmd.CommandText = "StudentTcFeeDetailsById_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt) { Value = MIID.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@Status_Flag", SqlDbType.VarChar) { Value = MIID.Status_flag });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MIID.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = MIID.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                result.Add(new StudentTCDTO
                                {
                                    feetobepaid = Convert.ToInt32(dataReader1["FSS_ToBePaid"]),
                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            studDto.feetobepaid = result.FirstOrDefault().feetobepaid;
                        }
                        else
                        {
                            studDto.feetobepaid = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        _tcimpl.LogInformation("TC Fee count sp error :'" + ex.Message + "'");
                    }
                }

                if (studDto.feetobepaid > 0)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentTcFeeDetailsById_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt) { Value = MIID.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@Status_Flag", SqlDbType.VarChar) { Value = MIID.Status_flag });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MIID.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = MIID.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "2" });

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
                                        dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            studDto.viewstudentfeedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                studDto.getconcession = (from a in _db.Adm_M_Student
                                         from b in _db.Fee_Master_ConcessionDMO
                                         where (a.AMST_Concession_Type == b.FMCC_Id && a.MI_Id == MIID.MI_Id && b.MI_Id == MIID.MI_Id && a.AMST_Id == MIID.AMST_Id)
                                         select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC Fee count: '" + ex.Message + "'");
            }

            // Library Due Details
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_CountIssueBook_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = MIID.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = MIID.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = MIID.AMST_Id
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
                        studDto.count_issuebooks = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _tcimpl.LogInformation("TC Libray count sp error :'" + ex.Message + "'");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC Libray Error :'" + ex.Message + "'");
                Console.WriteLine(ex.Message);
            }

            // PDA Due Details
            try
            {
                studDto.pdadata = _db.PDA_StatusDMO.Where(t => t.MI_Id == MIID.MI_Id && t.ASMAY_Id == MIID.ASMAY_Id && t.AMST_Id == MIID.AMST_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC PDA Error :'" + ex.Message + "'");
                Console.WriteLine(ex.Message);
            }

            // GET STUDENT JOINED DETAILS

            studDto.getjoineddetails = (from a in _db.Adm_M_Student
                                        from b in _db.School_M_Class
                                        where (a.ASMCL_Id == b.ASMCL_Id && a.AMST_Id == MIID.AMST_Id)
                                        select new StudentTCDTO
                                        {
                                            ASMCL_ClassName = b.ASMCL_ClassName

                                        }).Distinct().ToArray();

            return studDto;
        }
        public async Task<StudentTCDTO> saveTcdet(StudentTCDTO tc_dto)
        {
            var Tc_Amst_Count = 0;

            try
            {
                var Admission_Config = _db.AdmissionStandardDMO.Where(a => a.MI_Id == tc_dto.MI_Id).ToList();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //update tc 
                StudentTC tc_dmo = Mapper.Map<StudentTC>(tc_dto);
                Tc_Amst_Count = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id && d.ASTC_DeletedFlag != true).Count();

                if (Tc_Amst_Count > 0)
                {
                    var Tc_Amst_Count1 = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id && d.ASTC_DeletedFlag != true).Single();

                    if (Tc_Amst_Count1.ASTC_ActiveFlag == "T")
                    {
                        var update_tc = _db.Student_TC.Single(d => d.ASTC_Id == Tc_Amst_Count1.ASTC_Id && d.ASTC_DeletedFlag != true);

                        update_tc.ASTC_TemporaryFlag = 0;
                        update_tc.ASTC_ActiveFlag = "L";
                        update_tc.UpdatedDate = indiantime0;
                        update_tc.ASTC_UpdatedBy = tc_dto.UserId;
                        update_tc.ASTC_AttendedDays = Convert.ToInt64(tc_dto.ASTC_AttendedDays);
                        update_tc.ASTC_Conduct = tc_dto.ASTC_Conduct;
                        update_tc.ASTC_LastAttendedDate = Convert.ToDateTime(tc_dto.ASTC_LastAttendedDate);
                        update_tc.ASTC_TCDate = Convert.ToDateTime(tc_dto.ASTC_TCDate);
                        update_tc.ASTC_TCApplicationDate = Convert.ToDateTime(tc_dto.ASTC_TCApplicationDate);
                        update_tc.ASTC_TCIssueDate = Convert.ToDateTime(tc_dto.ASTC_TCIssueDate);
                        update_tc.ASTC_PromotionDate = Convert.ToDateTime(tc_dto.ASTC_PromotionDate);
                        _db.Student_TC.Update(update_tc);

                        var update_ADM = _db.Adm_M_Student.Single(d => d.AMST_Id == tc_dto.AMST_Id);
                        var update_admy = _db.School_Adm_Y_StudentDMO.Single(d => d.AMST_Id == tc_dto.AMST_Id && d.AMAY_ActiveFlag == 1
                        && d.ASMAY_Id == tc_dto.ASMAY_Id);

                        update_ADM.AMST_SOL = "L";
                        update_ADM.AMST_ActiveFlag = 0;                        
                        update_ADM.UpdatedDate = indiantime0;
                        update_ADM.AMST_UpdatedBy = tc_dto.UserId;

                        update_admy.AMAY_ActiveFlag = 0;
                        update_admy.ASYST_UpdatedBy = tc_dto.UserId;
                        update_admy.UpdatedDate = indiantime0;

                        _db.Adm_M_Student.Update(update_ADM);
                        _db.School_Adm_Y_StudentDMO.Update(update_admy);                      

                        var update_flag1 = _db.SaveChanges();

                        if (update_flag1 >= 1)
                        {
                            tc_dto.returnval = true;

                            // Concession Procedure Related To Fee
                            var getconcesstionflag = (from a in _db.Adm_M_Student
                                                      from b in _db.Fee_Master_ConcessionDMO
                                                      where (a.AMST_Concession_Type == b.FMCC_Id && a.AMST_Id == tc_dto.AMST_Id && a.MI_Id == tc_dto.MI_Id)
                                                      select b).Distinct().ToList();
                            try
                            {
                                if (getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "S" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "E" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "R")
                                {
                                    var outputval = _db.Database.ExecuteSqlCommand("DELETE_STUDENT_ON_TC  @p0,@p1,@p2,@p3",
                                     tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id, getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper());

                                    if (outputval >= 1)
                                    {
                                        tc_dto.returnval = true;
                                    }
                                    else
                                    {
                                        tc_dto.returnval = true;
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            try
                            {
                                // Delete Fee For General 

                                var checkfeeflagtc = _db.FeeMasterConfigurationDMO.Where(a => a.MI_Id == tc_dto.MI_Id).ToList();

                                var getconcesstionflag1 = (from a in _db.Adm_M_Student
                                                           from b in _db.Fee_Master_ConcessionDMO
                                                           where (a.AMST_Concession_Type == b.FMCC_Id && a.AMST_Id == tc_dto.AMST_Id && a.MI_Id == tc_dto.MI_Id)
                                                           select b).Distinct().ToList();

                                if (checkfeeflagtc.FirstOrDefault().FMC_AUTO_FEE_MAP_TC == true && getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "G")
                                {
                                    // [DELETE_FEE_FOR_GENERAL_TC_TAKEN_STUDENTS] @MI_ID AS BIGINT,@ASMAY_ID AS BIGINT,@AMST_ID AS BIGINT , @FLAG AS VARCHAR

                                    var outputval1 = _db.Database.ExecuteSqlCommand("DELETE_FEE_FOR_GENERAL_TC_TAKEN_STUDENTS  @p0,@p1,@p2,@p3",
                                            tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id, getconcesstionflag1.FirstOrDefault().FMCC_ConcessionFlag.ToUpper());

                                    if (outputval1 >= 1)
                                    {
                                        tc_dto.returnval = true;
                                    }
                                    else
                                    {
                                        tc_dto.returnval = true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            // Updating The Role id and Role Type From Student To Alumni

                            var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == tc_dto.AMST_Id).Select(a => a.STD_APP_ID).ToList();
                            try
                            {                               
                                var getroleid = _db.applicationRole.Where(a => a.Name.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                var getroletypeid = _db.MasterRoleType.Where(a => a.IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                if (getroleid.Count > 0 && getroletypeid.Count > 0)
                                {
                                    var getapplicationuserrole = _db.appUserRole.Where(a => a.UserId == getstdappid[0]).ToList();
                                    if (getapplicationuserrole.Count() > 0)
                                    {
                                        var getapplicationuserrolenew = _db.appUserRole.Single(a => a.UserId == getstdappid[0]);

                                        getapplicationuserrolenew.RoleId = getroleid.FirstOrDefault().Id;
                                        getapplicationuserrolenew.RoleTypeId = getroletypeid.FirstOrDefault().IVRMRT_Id;

                                        _db.Update(getapplicationuserrolenew);
                                        var id = _db.SaveChanges();
                                        if (id > 0)
                                        {
                                            tc_dto.returnval = true;
                                        }
                                        else
                                        {
                                            tc_dto.returnval = true;
                                        }
                                    }
                                }
                                else
                                {
                                    var chckuser = _db.UserRoleWithInstituteDMO.Where(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]).ToList();

                                    if (chckuser.Count() > 0)
                                    {
                                        var updatecheckuser = _db.UserRoleWithInstituteDMO.Single(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]);

                                        updatecheckuser.Activeflag = 0;
                                        _db.Update(updatecheckuser);
                                        var i = _db.SaveChanges();
                                        if (i > 0)
                                        {
                                            tc_dto.returnval = true;
                                        }
                                        else
                                        {
                                            tc_dto.returnval = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            // Transfering TC Students To Alumni

                            try
                            {
                                var outputval = _db.Database.ExecuteSqlCommand("Alumini_Students_Insert  @p0,@p1,@p2", tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id);

                                if (outputval >= 1)
                                {
                                    tc_dto.returnval = true;
                                    var studDetails = _db.Alumni_M_StudentDMO.Where(t => t.MI_Id == tc_dto.MI_Id && t.AMST_ID == tc_dto.AMST_Id).ToList();
                                    if (studDetails.Count > 0)
                                    {
                                        AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();
                                        Alumni.MI_Id = studDetails.FirstOrDefault().MI_Id;
                                        Alumni.ALSREG_Photo = studDetails.FirstOrDefault().ALMST_StudentPhoto;
                                        Alumni.ALSREG_ApprovedFlag = true;
                                        Alumni.ALSREG_MembershipCategory = 0;
                                        Alumni.ALSREG_MemberName = studDetails.FirstOrDefault().ALMST_FirstName;
                                        Alumni.ALSREG_EmailId = studDetails.FirstOrDefault().ALMST_emailId;
                                        Alumni.ALSREG_MobileNo = Convert.ToInt64(studDetails.FirstOrDefault().ALMST_MobileNo);
                                        Alumni.ALSREG_AdmittedYear = Convert.ToInt64(studDetails.FirstOrDefault().ASMAY_Id_Join);
                                        Alumni.ALSREG_LeftYear = Convert.ToInt64(studDetails.FirstOrDefault().ASMAY_Id_Left);
                                        Alumni.ALSREG_LeftClass = Convert.ToInt64(studDetails.FirstOrDefault().ASMCL_Id_Left);
                                        Alumni.ALSREG_AdmittedClass = Convert.ToInt64(studDetails.FirstOrDefault().ASMCL_Id_Join);
                                        Alumni.ALSREG_Date = DateTime.Now;
                                        Alumni.CreatedDate = indiantime0;
                                        Alumni.UpdatedDate = indiantime0;
                                        Alumni.ALMST_Id = studDetails.FirstOrDefault().ALMST_Id;
                                        Alumni.ALSREG_CreatedBy = tc_dto.UserId;
                                        Alumni.ALSREG_UpdatedBy = tc_dto.UserId;
                                        Alumni.ALSREG_ActiveFlg = true;
                                        _db.Add(Alumni);
                                        _db.SaveChanges();

                                        Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
                                        alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
                                        alumniluser.IVRMUL_Id = getstdappid[0];
                                        _db.Add(alumniluser);
                                        _db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    tc_dto.returnval = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }                            
                        }
                        else
                        {
                            tc_dto.returnval = false;
                        }
                    }

                    if (Tc_Amst_Count1.ASTC_ActiveFlag == "L")
                    {
                        var update_tc1 = _db.Student_TC.Single(d => d.ASTC_Id == Tc_Amst_Count1.ASTC_Id && d.ASTC_DeletedFlag != true);

                        update_tc1.ASTC_AttendedDays = Convert.ToInt64(tc_dto.ASTC_AttendedDays);
                        update_tc1.ASTC_Conduct = tc_dto.ASTC_Conduct;
                        update_tc1.ASTC_TCNO = tc_dto.ASTC_TCNO;
                        update_tc1.ASTC_LastAttendedDate = Convert.ToDateTime(tc_dto.ASTC_LastAttendedDate);
                        update_tc1.ASTC_TCDate = Convert.ToDateTime(tc_dto.ASTC_TCDate);
                        update_tc1.ASTC_TCIssueDate = Convert.ToDateTime(tc_dto.ASTC_TCIssueDate);
                        update_tc1.ASTC_TCApplicationDate = Convert.ToDateTime(tc_dto.ASTC_TCApplicationDate);
                        update_tc1.ASTC_PromotionDate = Convert.ToDateTime(tc_dto.ASTC_PromotionDate);

                        update_tc1.ASTC_ElectivesStudied = tc_dto.ASTC_ElectivesStudied;
                        update_tc1.ASTC_ExtraActivities = tc_dto.ASTC_ExtraActivities;
                        update_tc1.ASTC_FeeConcession = tc_dto.ASTC_FeeConcession;
                        update_tc1.ASTC_FeePaid = tc_dto.ASTC_FeePaid;
                        update_tc1.ASTC_LanguageStudied = tc_dto.ASTC_LanguageStudied;
                        update_tc1.ASTC_LastExamDetails = tc_dto.ASTC_LastExamDetails;
                        update_tc1.ASTC_LeavingReason = tc_dto.ASTC_LeavingReason;
                        update_tc1.ASTC_MedicallyExam = tc_dto.ASTC_MedicallyExam;
                        update_tc1.ASTC_MediumOfINStruction = tc_dto.ASTC_MediumOfINStruction;
                        update_tc1.ASTC_NCCDetails = tc_dto.ASTC_NCCDetails;
                        update_tc1.ASTC_Qual_Class = tc_dto.ASTC_Qual_Class;
                        update_tc1.ASTC_Qual_PromotionFlag = tc_dto.ASTC_Qual_PromotionFlag;
                        update_tc1.ASTC_Remarks = tc_dto.ASTC_Remarks;
                        update_tc1.ASTC_Result = tc_dto.ASTC_Result;
                        update_tc1.ASTC_ResultDetails = tc_dto.ASTC_ResultDetails;
                        update_tc1.ASTC_Scholarship = tc_dto.ASTC_Scholarship;
                        update_tc1.ASTC_WorkingDays = Convert.ToInt64(tc_dto.ASTC_WorkingDays);
                        update_tc1.Last_Class_Studied = tc_dto.Last_Class_Studied;
                        update_tc1.UpdatedDate = indiantime0;
                        update_tc1.ASTC_UpdatedBy = tc_dto.UserId;

                        _db.Student_TC.Update(update_tc1);

                        var update_flag1 = _db.SaveChanges();

                        if (update_flag1 >= 1)
                        {
                            tc_dto.returnval = true;
                        }
                        else
                        {
                            tc_dto.returnval = false;
                        }
                    }
                }

                else
                {
                    if (tc_dto.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        tc_dto.transnumconfigsettings.MI_Id = tc_dmo.MI_Id;
                        tc_dto.transnumconfigsettings.ASMAY_Id = tc_dmo.ASMAY_Id;
                        tc_dmo.ASTC_TCNO = a.GenerateNumber(tc_dto.transnumconfigsettings);
                    }

                    if (tc_dto.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    {
                        var tcnoduplicate = _db.Student_TC.Where(d => d.ASTC_TCNO == tc_dto.ASTC_TCNO && d.MI_Id == tc_dto.MI_Id).Count();
                        if (tcnoduplicate > 0)
                        {
                            tc_dto.tcflagexists = "TC No. Already Exists";
                            return tc_dto;
                        }
                    }

                    var studDet = _db.Adm_M_Student.Where(d => d.AMST_Id == tc_dto.AMST_Id).ToList();
                    var section = _act.school_Adm_Y_StudentDMO.Where(d => d.AMST_Id == tc_dto.AMST_Id && d.ASMAY_Id == tc_dto.ASMAY_Id
                    && d.AMAY_ActiveFlag == 1).ToList();

                    if (section.Count == 0)
                    {
                        tc_dmo.ASMCL_Id = 0;
                        tc_dmo.ASMS_Id = 0;
                    }
                    else
                    {
                        for (int i = 0; i < section.Count; i++)
                        {
                            tc_dmo.ASMCL_Id = section[i].ASMCL_Id;
                            tc_dmo.ASMS_Id = section[i].ASMS_Id;
                        }
                    }

                    tc_dmo.ASMAY_Id = tc_dto.ASMAY_Id;
                    tc_dmo.IMC_Id = studDet.FirstOrDefault().IC_Id;
                    tc_dmo.MI_Id = studDet.FirstOrDefault().MI_Id;

                    if (tc_dto.ASTC_ActiveFlag == "S" || tc_dto.ASTC_ActiveFlag == "D")
                    {
                        if (tc_dto.ASTC_TemporaryFlag == 1)
                        {
                            tc_dmo.ASTC_ActiveFlag = "T";
                            tc_dmo.ASTC_TemporaryFlag = 1;
                            tc_dmo.ASTC_DeletedFlag = false;
                            tc_dmo.ASTC_ReadmitRemarks = "";
                            tc_dmo.CreatedDate = indiantime0;
                            tc_dmo.UpdatedDate = indiantime0;
                            tc_dmo.ASTC_CreatedBy = tc_dto.UserId;
                            tc_dmo.ASTC_UpdatedBy = tc_dto.UserId;

                            _db.Student_TC.Add(tc_dmo);
                            var flag = _db.SaveChanges();
                            if (flag == 1)
                            {
                                tc_dto.returnval = true;
                            }
                            else
                            {
                                tc_dto.returnval = false;
                            }
                        }

                        else
                        {
                            tc_dmo.ASTC_ActiveFlag = "L";
                            tc_dmo.ASTC_DeletedFlag = false;
                            tc_dmo.ASTC_ReadmitRemarks = "";
                            tc_dmo.ASTC_TemporaryFlag = 0;
                            tc_dmo.CreatedDate = indiantime0;
                            tc_dmo.UpdatedDate = indiantime0;
                            tc_dmo.ASTC_CreatedBy = tc_dto.UserId;
                            tc_dmo.ASTC_UpdatedBy = tc_dto.UserId;
                            _db.Student_TC.Add(tc_dmo);
                            var flag = _db.SaveChanges();

                            if (flag == 1)
                            {
                                tc_dto.returnval = true;

                                try
                                {
                                    // Concession Procedure Related To Fee

                                    var getconcesstionflag = (from a in _db.Adm_M_Student
                                                              from b in _db.Fee_Master_ConcessionDMO
                                                              where (a.AMST_Concession_Type == b.FMCC_Id && a.AMST_Id == tc_dto.AMST_Id && a.MI_Id == tc_dto.MI_Id)
                                                              select b).Distinct().ToList();

                                    if (getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "S" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "E" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "R")
                                    {
                                        var outputval = _db.Database.ExecuteSqlCommand("DELETE_STUDENT_ON_TC  @p0,@p1,@p2,@p3",
                                         tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id, getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper());

                                        if (outputval >= 1)
                                        {
                                            tc_dto.returnval = true;
                                        }
                                        else
                                        {
                                            tc_dto.returnval = true;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                // Delete Fee For General 
                                try
                                {
                                    var checkfeeflagtc = _db.FeeMasterConfigurationDMO.Where(a => a.MI_Id == tc_dto.MI_Id).ToList();

                                    var getconcesstionflag1 = (from a in _db.Adm_M_Student
                                                               from b in _db.Fee_Master_ConcessionDMO
                                                               where (a.AMST_Concession_Type == b.FMCC_Id && a.AMST_Id == tc_dto.AMST_Id && a.MI_Id == tc_dto.MI_Id)
                                                               select b).Distinct().ToList();

                                    if (checkfeeflagtc.FirstOrDefault().FMC_AUTO_FEE_MAP_TC == true 
                                        && getconcesstionflag1.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "G")
                                    {
                                        // [DELETE_FEE_FOR_GENERAL_TC_TAKEN_STUDENTS] @MI_ID AS BIGINT,@ASMAY_ID AS BIGINT,@AMST_ID AS BIGINT , @FLAG AS VARCHAR

                                        var outputval1 = _db.Database.ExecuteSqlCommand("DELETE_FEE_FOR_GENERAL_TC_TAKEN_STUDENTS  @p0,@p1,@p2,@p3",
                                                tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id, getconcesstionflag1.FirstOrDefault().FMCC_ConcessionFlag.ToUpper());

                                        if (outputval1 >= 1)
                                        {
                                            tc_dto.returnval = true;
                                        }
                                        else
                                        {
                                            tc_dto.returnval = true;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    tc_dto.returnval = true;
                                    Console.WriteLine(ex.Message);
                                }

                                var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == tc_dto.AMST_Id).Select(a => a.STD_APP_ID).ToList();
                                try
                                {
                                    // Updating The Role id and Role Type From Student To Alumni

                                    var getroleid = _db.applicationRole.Where(a => a.Name.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                    var getroletypeid = _db.MasterRoleType.Where(a => a.IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                    if (getroleid.Count > 0 && getroletypeid.Count > 0)
                                    {
                                        var getapplicationuserrole = _db.appUserRole.Where(a => a.UserId == getstdappid[0]).ToList();
                                        if (getapplicationuserrole.Count() > 0)
                                        {
                                            var getapplicationuserrolenew = _db.appUserRole.Single(a => a.UserId == getstdappid[0]);

                                            getapplicationuserrolenew.RoleId = getroleid.FirstOrDefault().Id;
                                            getapplicationuserrolenew.RoleTypeId = getroletypeid.FirstOrDefault().IVRMRT_Id;

                                            _db.Update(getapplicationuserrolenew);
                                            var id = _db.SaveChanges();
                                            if (id > 0)
                                            {
                                                tc_dto.returnval = true;
                                            }
                                            else
                                            {
                                                tc_dto.returnval = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var chckuser = _db.UserRoleWithInstituteDMO.Where(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]).ToList();
                                        if (chckuser.Count() > 0)
                                        {
                                            var updatecheckuser = _db.UserRoleWithInstituteDMO.Single(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]);
                                            updatecheckuser.Activeflag = 0;

                                            _db.Update(updatecheckuser);
                                            var i = _db.SaveChanges();
                                            if (i > 0)
                                            {
                                                tc_dto.returnval = true;
                                            }
                                            else
                                            {
                                                tc_dto.returnval = true;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            else
                            {
                                tc_dto.returnval = false;
                            }
                        }
                    }

                    var update_ADM = _db.Adm_M_Student.FirstOrDefault(d => d.AMST_Id == tc_dto.AMST_Id);
                    var update_admy = _db.School_Adm_Y_StudentDMO.FirstOrDefault(d => d.AMST_Id == tc_dto.AMST_Id && d.AMAY_ActiveFlag == 1
                    && d.ASMAY_Id == tc_dto.ASMAY_Id);

                    if (update_ADM.AMST_SOL == "S" || update_ADM.AMST_SOL == "D")
                    {
                        if (tc_dto.ASTC_TemporaryFlag == 1)
                        {
                            update_ADM.AMST_SOL = "T";
                            update_ADM.AMST_ActiveFlag = 1;
                            update_admy.AMAY_ActiveFlag = 1;
                        }
                        else
                        {
                            update_ADM.AMST_SOL = "L";
                            update_ADM.AMST_ActiveFlag = 0;
                            update_admy.AMAY_ActiveFlag = 0;
                        }

                        update_ADM.UpdatedDate = indiantime0;
                        update_ADM.AMST_UpdatedBy = tc_dto.UserId;

                        update_admy.ASYST_UpdatedBy = tc_dto.UserId;
                        update_admy.UpdatedDate = indiantime0;

                        _db.Adm_M_Student.Update(update_ADM);
                        _db.School_Adm_Y_StudentDMO.Update(update_admy);

                        var update_flag = _db.SaveChanges();

                        if (update_flag >= 1)
                        {
                            tc_dto.returnval = true;
                        }
                        else
                        {
                            tc_dto.returnval = false;
                        }

                        try
                        {                         
                            if (tc_dto.ASTC_TemporaryFlag != 1)
                            {
                                using (var cmd = _act.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Alumini_Students_Insert";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt){Value = tc_dto.MI_Id});
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt){Value = tc_dto.ASMAY_Id});
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.NVarChar){Value = tc_dto.AMST_Id});

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var outputval = cmd.ExecuteNonQuery();

                                    if (outputval >= 1)
                                    {
                                        tc_dto.returnval = true;

                                        var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == tc_dto.AMST_Id).Select(a => a.STD_APP_ID).ToList();

                                        var studDetails = _db.Alumni_M_StudentDMO.Where(t => t.MI_Id == tc_dto.MI_Id && t.AMST_ID == tc_dto.AMST_Id).ToList();

                                        if (studDetails.Count > 0)
                                        {
                                            AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();
                                            Alumni.MI_Id = studDetails.FirstOrDefault().MI_Id;
                                            Alumni.ALSREG_Photo = studDetails.FirstOrDefault().ALMST_StudentPhoto;
                                            Alumni.ALSREG_ApprovedFlag = true;
                                            Alumni.ALSREG_MembershipCategory = 0;
                                            Alumni.ALSREG_MemberName = studDetails.FirstOrDefault().ALMST_FirstName;
                                            Alumni.ALSREG_EmailId = studDetails.FirstOrDefault().ALMST_emailId;
                                            Alumni.ALSREG_MobileNo = Convert.ToInt64(studDetails.FirstOrDefault().ALMST_MobileNo);
                                            Alumni.ALSREG_AdmittedYear = Convert.ToInt64(studDetails.FirstOrDefault().ASMAY_Id_Join);
                                            Alumni.ALSREG_LeftYear = Convert.ToInt64(studDetails.FirstOrDefault().ASMAY_Id_Left);
                                            Alumni.ALSREG_LeftClass = Convert.ToInt64(studDetails.FirstOrDefault().ASMCL_Id_Left);
                                            Alumni.ALSREG_AdmittedClass = Convert.ToInt64(studDetails.FirstOrDefault().ASMCL_Id_Join);
                                            Alumni.ALSREG_Date = DateTime.Now;
                                            Alumni.CreatedDate = indiantime0;
                                            Alumni.UpdatedDate = indiantime0;
                                            Alumni.ALMST_Id = studDetails.FirstOrDefault().ALMST_Id;
                                            Alumni.ALSREG_CreatedBy = tc_dto.UserId;
                                            Alumni.ALSREG_UpdatedBy = tc_dto.UserId;
                                            Alumni.ALSREG_ActiveFlg = true;
                                            _db.Add(Alumni);
                                            _db.SaveChanges();

                                            Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
                                            alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
                                            alumniluser.IVRMUL_Id = getstdappid[0];
                                            _db.Add(alumniluser);
                                            _db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        tc_dto.returnval = true;
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
               
                if (tc_dto.returnval == true && tc_dto.Email_flag.Equals("Yes"))
                {
                    var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == tc_dto.MI_Id);
                    var studDetail = _db.Adm_M_Student.Where(t => t.MI_Id == tc_dto.MI_Id && t.AMST_Id == tc_dto.AMST_Id).ToList();

                    var Tc_Amst_Count1 = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id && d.ASTC_DeletedFlag != true).Single();
                    var update_tc1 = _db.Student_TC.Single(d => d.ASTC_Id == Tc_Amst_Count1.ASTC_Id && d.ASTC_DeletedFlag != true);

                    if (admConfig.ASC_DefaultSMS_Flag == "M") //To Mother MAIL/SMS
                    {

                        SMS sms = new SMS(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        Email Email = new Email(_db);

                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                    }
                    else if (admConfig.ASC_DefaultSMS_Flag == "F")  //To Father MAIL/SMS
                    {
                        SMS sms = new SMS(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        Email Email = new Email(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                    }
                    else     //To Student MAIL/SMS
                    {
                        SMS sms = new SMS(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        Email Email = new Email(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tc_dto;
        }
        public StudentTCDTO chk_tc_dup(StudentTCDTO tc_no_dup)
        {
            try
            {
                var tcnoduplicate = _db.Student_TC.Where(d => d.ASTC_TCNO == tc_no_dup.ASTC_TCNO && d.MI_Id == tc_no_dup.MI_Id).Count();
                if (tcnoduplicate > 0)
                {
                    tc_no_dup.tcflagexists = "TC No. Already Exists";
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tc_no_dup;
        }
        public async Task<StudentTCDTO> getstudent_name_list(StudentTCDTO get_stu_name_list)
        {
            get_stu_name_list.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == get_stu_name_list.MI_Id && t.IMN_Flag.Equals("tcno")).ToArrayAsync();

            //get_stu_name_list.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == get_stu_name_list.MI_Id).ToArrayAsync();

            StudentTCDTO attdto = new StudentTCDTO();
            List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
            admissionconfigurationsettings = _db.AdmissionStandardDMO.AsNoTracking().Where(t => t.MI_Id == get_stu_name_list.MI_Id).ToList();
            get_stu_name_list.admissioncongigurationList = admissionconfigurationsettings.ToArray();

            if (admissionconfigurationsettings.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
            {
                get_stu_name_list.adm_num_flag = "A";
            }
            else if (admissionconfigurationsettings.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
            {
                get_stu_name_list.adm_num_flag = "N";
            }
            get_stu_name_list.tcpermanentpayment = await _db.AdmissionStandardDMO.Where(t => t.MI_Id == get_stu_name_list.MI_Id).ToArrayAsync();

            get_stu_name_list.classlistnew = _db.School_M_Class.Where(a => a.MI_Id == get_stu_name_list.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();

            get_stu_name_list.Qualifiedclass = _db.Master_ExamQualified_ClassDMO.Where(q => q.MI_ID == get_stu_name_list.MI_Id).ToArray();
            get_stu_name_list.get_elective_subjects = (from a in _db.StudentMappingDMO
                                                       from b in _db.AcademicYear
                                                       from c in _db.Adm_M_Student
                                                       from d in _db.SchoolYearWiseStudent
                                                       from e in _db.MasterSubjectList
                                                       where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == get_stu_name_list.MI_Id && a.ASMAY_Id == get_stu_name_list.ASMAY_Id && a.AMST_Id == get_stu_name_list.AMST_Id && a.ESTSU_ElecetiveFlag == true)
                                                       select new StudentTCDTO
                                                       {
                                                           subjectname = e.ISMS_SubjectName
                                                       }).Distinct().ToArray();


            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _db.AcademicYear.Where(t => t.MI_Id == get_stu_name_list.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            get_stu_name_list.academicList = allyear.ToArray();

            get_stu_name_list.currentYear = _db.AcademicYear.Where(a => a.MI_Id == get_stu_name_list.MI_Id && a.Is_Active == true && a.ASMAY_Id == get_stu_name_list.ASMAY_Id).ToArray();

            get_stu_name_list.studentlist = (from b in _db.Adm_M_Student
                                             from c in _db.AcademicYear
                                             from d in _db.admissioncls
                                             from e in _db.Section
                                             where (b.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id
                                             && b.AMST_SOL.Equals("L") && b.AMST_ActiveFlag == 0 && b.ASMAY_Id == get_stu_name_list.ASMAY_Id)
                                             select new StudentTCDTO
                                             {
                                                 AMST_Id = b.AMST_Id,
                                                 AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) + " " +
                                                 (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName)).Trim(),
                                             }).Distinct().OrderBy(b => b.AMST_Id).ToArray();

            try
            {
                List<StudentTCDTO> ln = new List<StudentTCDTO>();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return get_stu_name_list;
        }
        public StudentTCDTO searchfilter(StudentTCDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                int activeflag = 0;
                int activeflagyear = 0;

                if (data.AMST_SOL == "S")
                {
                    activeflag = 1;
                    activeflagyear = 1;
                }
                else if (data.AMST_SOL == "L")
                {
                    activeflag = 0;
                    activeflagyear = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    activeflag = 1;
                    activeflagyear = 1;
                }
                else if (data.AMST_SOL == "T")
                {
                    activeflag = 1;
                    activeflagyear = 1;
                }
                //if (data.allorindividual == "All")
                //{
                //    if (data.flag == "A")
                //    {
                //        data.studentlistsearch = (from a in _db.Adm_M_Student
                //                                  from b in _db.SchoolYearWiseStudent
                //                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && (a.AMST_AdmNo.StartsWith(data.searchfilter)))
                //                                  select new StudentTCDTO
                //                                  {
                //                                      AMST_Id = a.AMST_Id,
                //                                      StudentName = (a.AMST_AdmNo + ':' + (a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                //                                  }).ToArray();
                //    }
                //    else if (data.flag == "N")
                //    {
                //        data.studentlistsearch = (from a in _db.Adm_M_Student
                //                                  from b in _db.SchoolYearWiseStudent
                //                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag 
                //                                  && b.AMAY_ActiveFlag == activeflagyear 
                //                                  &&
                //                                  ((a.AMST_FirstName != null ? a.AMST_FirstName.Trim().ToUpper().Trim() : " " + " " + a.AMST_MiddleName != null ? a.AMST_MiddleName.Trim().ToUpper() : " " + " " + a.AMST_LastName != null ? a.AMST_LastName.Trim().ToUpper() : " ").Contains(data.searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                //                                  select new StudentTCDTO
                //                                  {
                //                                      AMST_Id = a.AMST_Id,
                //                                      StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                //                                  }).ToArray();
                //    }

                //}
                //else
                //{
                //    if (data.flag == "A")
                //    {
                //        data.studentlistsearch = (from a in _db.Adm_M_Student
                //                                  from b in _db.SchoolYearWiseStudent
                //                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && (a.AMST_AdmNo.StartsWith(data.searchfilter)))
                //                                  select new StudentTCDTO
                //                                  {
                //                                      AMST_Id = a.AMST_Id,
                //                                      StudentName = (a.AMST_AdmNo + ':' + (a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                //                                  }).ToArray();
                //    }

                //    else if (data.flag == "N")
                //    {
                //        data.studentlistsearch = (from a in _db.Adm_M_Student
                //                                 from b in _db.SchoolYearWiseStudent
                //                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear &&
                //                                   ((a.AMST_FirstName != null ? a.AMST_FirstName.Trim().ToUpper().Trim() : " " + " " + a.AMST_MiddleName != null ? a.AMST_MiddleName.Trim().ToUpper() : " " + " " + a.AMST_LastName != null ? a.AMST_LastName.Trim().ToUpper() : " ").Contains(data.searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                //                                  select new StudentTCDTO
                //                                  {
                //                                      AMST_Id = a.AMST_Id,
                //                                      StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                //                                  }).ToArray();
                //    }
                //}



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_TC_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NAME",
                    SqlDbType.VarChar)
                    {
                        //Value = Class_Id
                        Value = data.searchfilter
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_SOL",
               SqlDbType.VarChar)
                    {
                        Value = data.AMST_SOL
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                    {
                        Value = data.flag
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
                        data.studentlistsearch = retObject.ToArray();
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

        // TC Cancel
        public StudentTCDTO GetTCCancelDetails(StudentTCDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.academicList = allyear.ToArray();

                data.currentYear = _db.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.getstudenttcdetails = (from a in _db.Student_TC
                                            from b in _db.Adm_M_Student
                                            from c in _db.AcademicYear
                                            from d in _db.admissioncls
                                            from e in _db.Section
                                            where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                            && b.AMST_SOL.Equals("L") && b.AMST_ActiveFlag == 0 && a.ASTC_ActiveFlag == "L" && a.ASMAY_Id == data.ASMAY_Id
                                            && a.ASTC_DeletedFlag == false)
                                            select new StudentTCDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) + " " +
                                                (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName) +
                                                ':' + a.ASTC_TCNO).Trim(),
                                                ASTC_TCDate = a.ASTC_TCDate
                                            }).Distinct().OrderBy(a => a.ASTC_TCDate).ToArray();

                data.getdeletedtcdetails = (from a in _db.Student_TC
                                            from b in _db.Adm_M_Student
                                            where (a.AMST_Id == b.AMST_Id && a.ASTC_ActiveFlag == "L" && a.ASMAY_Id == data.ASMAY_Id && a.ASTC_DeletedFlag == true)
                                            select new StudentTCDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) + " " +
                                                (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName)).Trim(),
                                                ASTC_TCApplicationDate = a.ASTC_TCApplicationDate,
                                                ASTC_TCIssueDate = a.ASTC_TCIssueDate,
                                                ASTC_TCDate = a.ASTC_TCDate,
                                                ASTC_TCNO = a.ASTC_TCNO,
                                                TC_CancelReason = a.ASTC_ReadmitRemarks,

                                                leftclassname = a.ASMCL_Id > 0 ? _db.School_M_Class.Where(e => e.MI_Id == data.MI_Id
                                                   && a.ASMCL_Id == e.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                                leftyearname = a.ASMAY_Id > 0 ? _db.AcademicYear.Where(f => f.MI_Id == data.MI_Id
                                                && a.ASMAY_Id == f.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                                leftsectionname = a.ASMS_Id > 0 ? _db.School_M_Section.Where(g => g.MI_Id == data.MI_Id
                                                && a.ASMS_Id == g.ASMS_Id).FirstOrDefault().ASMC_SectionName : "",
                                                AMST_AdmNo = b.AMST_AdmNo,
                                                AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                UpdatedDate = a.UpdatedDate
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentTCDTO OnChangeAcademicYear(StudentTCDTO data)
        {
            try
            {
                data.getstudenttcdetails = (from a in _db.Student_TC
                                            from b in _db.Adm_M_Student
                                            from c in _db.AcademicYear
                                            from d in _db.admissioncls
                                            from e in _db.Section
                                            where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                            && b.AMST_SOL.Equals("L") && b.AMST_ActiveFlag == 0 && a.ASTC_ActiveFlag == "L" && a.ASMAY_Id == data.ASMAY_Id
                                            && a.ASTC_DeletedFlag == false)
                                            select new StudentTCDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) + " " +
                                                (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName) +
                                                ':' + a.ASTC_TCNO).Trim(),
                                                ASTC_TCDate = a.ASTC_TCDate
                                            }).Distinct().OrderBy(a => a.ASTC_TCDate).ToArray();


                data.getdeletedtcdetails = (from a in _db.Student_TC
                                            from b in _db.Adm_M_Student
                                            where (a.AMST_Id == b.AMST_Id && a.ASTC_ActiveFlag == "L" && a.ASMAY_Id == data.ASMAY_Id && a.ASTC_DeletedFlag == true)
                                            select new StudentTCDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) + " " +
                                                (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName)).Trim(),
                                                ASTC_TCApplicationDate = a.ASTC_TCApplicationDate,
                                                ASTC_TCIssueDate = a.ASTC_TCIssueDate,
                                                ASTC_TCDate = a.ASTC_TCDate,
                                                ASTC_TCNO = a.ASTC_TCNO,
                                                TC_CancelReason = a.ASTC_ReadmitRemarks,

                                                leftclassname = a.ASMCL_Id > 0 ? _db.School_M_Class.Where(e => e.MI_Id == data.MI_Id
                                                   && a.ASMCL_Id == e.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                                leftyearname = a.ASMAY_Id > 0 ? _db.AcademicYear.Where(f => f.MI_Id == data.MI_Id
                                                && a.ASMAY_Id == f.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                                leftsectionname = a.ASMS_Id > 0 ? _db.School_M_Section.Where(g => g.MI_Id == data.MI_Id
                                                && a.ASMS_Id == g.ASMS_Id).FirstOrDefault().ASMC_SectionName : "",

                                                AMST_AdmNo = b.AMST_AdmNo,
                                                AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                UpdatedDate = a.UpdatedDate

                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentTCDTO OnStudentNameChange(StudentTCDTO data)
        {
            try
            {
                data.gettcdetailsbystudentid = (from a in _db.Student_TC
                                                from b in _db.Adm_M_Student
                                                where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                                select new StudentTCDTO
                                                {
                                                    AMST_Id = a.AMST_Id,
                                                    AMST_FirstName = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) + " " +
                                                (b.AMST_MiddleName == null ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? "" : b.AMST_LastName) +
                                                ':' + a.ASTC_TCNO).Trim(),
                                                    ASTC_TCDate = a.ASTC_TCDate,
                                                    ASTC_TCNO = a.ASTC_TCNO,
                                                    ASTC_TCApplicationDate = a.ASTC_TCApplicationDate,
                                                    ASTC_TCIssueDate = a.ASTC_TCIssueDate,

                                                    joinedclassname = b.ASMCL_Id > 0 ? _db.School_M_Class.Where(c => c.MI_Id == data.MI_Id
                                                    && b.ASMCL_Id == c.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                                    joinedyearname = b.ASMAY_Id > 0 ? _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id
                                                    && b.ASMAY_Id == d.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                                    leftclassname = a.ASMCL_Id > 0 ? _db.School_M_Class.Where(e => e.MI_Id == data.MI_Id
                                                    && a.ASMCL_Id == e.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                                    leftyearname = a.ASMAY_Id > 0 ? _db.AcademicYear.Where(f => f.MI_Id == data.MI_Id
                                                    && a.ASMAY_Id == f.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                                    leftsectionname = a.ASMS_Id > 0 ? _db.School_M_Section.Where(g => g.MI_Id == data.MI_Id
                                                    && a.ASMS_Id == g.ASMS_Id).FirstOrDefault().ASMC_SectionName : "",

                                                    studentphotopath = b.AMST_Photoname,
                                                    AMST_AdmNo = b.AMST_AdmNo,
                                                    AMST_RegistrationNo = b.AMST_RegistrationNo,
                                                }).Distinct().ToArray();

                data.studenttcdetails = _db.Student_TC.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.getadm_m_student_details = _db.Adm_M_Student.Where(a => a.AMST_Id == data.AMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentTCDTO SaveTCCancelDetails(StudentTCDTO data)
        {
            try
            {

                using (var cmd = _act.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_TC_Cancellation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar){Value = data.AMST_Id});
                    cmd.Parameters.Add(new SqlParameter("@Remarks", SqlDbType.VarChar){Value = data.TC_CancelReason});
                    cmd.Parameters.Add(new SqlParameter("@UserId_Update", SqlDbType.VarChar){Value = data.UserId});

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var outputval = cmd.ExecuteNonQuery();

                    if (outputval >= 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                //var outputval1 = _db.Database.ExecuteSqlCommand("Adm_Student_TC_Cancellation  @p0,@p1,@p2,@p3,@p3", data.MI_Id, data.ASMAY_Id, data.AMST_Id, data.TC_CancelReason,data.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


        public async Task<StudentTCDTO> sourcecntdata(StudentTCDTO data)
        {
            try
            {

                string asmcl_ids = "0";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    if (data.classlsttwo.Length > 0)
                    {
                        foreach (var ue in data.classlsttwo)
                        {
                            asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Sourcewiseadmissioncount1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
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
                                        dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentDetails = retObject.ToArray();
                     
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentTCDTO getallsourcedetails(StudentTCDTO data)
        {
            try
            {

                StudentTCDTO dto = new StudentTCDTO();
                data.accyear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();

                data.classlist = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).OrderByDescending(d => d.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //MotherTongueWise
        public async Task<StudentTCDTO> languagecntdata(StudentTCDTO data)
        {
            try
            {

                string asmcl_ids = "0";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    if (data.classlsttwo.Length > 0)
                    {
                        foreach (var ue in data.classlsttwo)
                        {
                            asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Languagewiseadmissioncount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
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
                                        dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentDetails = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<StudentTCDTO> statecntdata(StudentTCDTO data)
        {
            try
            {

                string asmcl_ids = "0";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    if (data.classlsttwo.Length > 0)
                    {
                        foreach (var ue in data.classlsttwo)
                        {
                            asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Languagewiseadmissioncount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
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
                                        dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentDetails = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}

