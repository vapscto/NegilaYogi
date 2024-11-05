using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Services
{
    public class StudentProgressCardReportImpl : Interfaces.StudentProgressCardReportInterface
    {
        private PortalContext _context;
        public StudentProgressCardReportImpl(PortalContext _con)
        {
            _context = _con;
        }

        // JSHS Student Progress Card Report Display In Student Portal
        public StudentProgressCardReportDTO getdetails(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();


                var getclasslist = (from a in _context.School_Adm_Y_StudentDMO
                                    from b in _context.Adm_M_Student
                                    where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                    && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                    select new StudentProgressCardReportDTO
                                    {
                                        ASMCL_Id = a.ASMCL_Id,
                                    }).Distinct().ToList();

                List<long> classid = new List<long>();

                foreach (var c in getclasslist)
                {
                    classid.Add(c.ASMCL_Id);
                }

                data.getclass = _context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && classid.Contains(a.ASMCL_Id) && a.ASMCL_ActiveFlag == true).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();


                var getyearlist = (from a in _context.School_Adm_Y_StudentDMO
                                   from b in _context.Adm_M_Student
                                   where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                   && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                   select new StudentProgressCardReportDTO
                                   {
                                       ASMAY_Id = a.ASMAY_Id,
                                   }).Distinct().ToList();

                List<long> yearid = new List<long>();

                foreach (var c in getyearlist)
                {
                    yearid.Add(c.ASMAY_Id);
                }

                data.getyear = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && yearid.Contains(a.ASMAY_Id) && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Term_List_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = getflag });

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
                            data.getexamtermlist = retObject.ToArray();
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
        public StudentProgressCardReportDTO onchangeclass(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Term_List_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = getflag });

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
                            data.getexamtermlist = retObject.ToArray();
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
        public StudentProgressCardReportDTO getreport(StudentProgressCardReportDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    if (data.examorterm == "Exam")
                    {
                        try
                        {

                            List<long> emeidnew = new List<long>();

                            var getexamlist = (from a in _context.Exm_Category_ClassDMO
                                               from b in _context.Exm_Master_CategoryDMO
                                               from c in _context.Exm_Yearly_CategoryDMO
                                               from d in _context.Exm_Yearly_Category_ExamsDMO
                                               from e in _context.ExmStudentMarksProcessDMO
                                               where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EME_Id == e.EME_Id
                                               && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                               && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id
                                               && e.ASMS_Id == data.ASMS_Id && e.ASMAY_Id == data.ASMAY_Id && e.AMST_Id == data.AMST_Id && e.EME_Id == data.EME_Id
                                               && ((d.EYCE_MarksPublishDate == null && e.ESTMP_PublishToStudentFlg == true)
                                               || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == false
                                               && Convert.ToDateTime(indiantime0.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))
                                               || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == true
                                               && Convert.ToDateTime(indiantime0.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))))
                                               select new exammasterDMO
                                               {
                                                   EME_Id = d.EME_Id
                                               }).Distinct().ToList();

                            if (getexamlist.Count > 0)
                            {
                                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                                   from b in _context.HR_Master_Employee_DMO
                                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                                   select new StudentProgressCardReportDTO
                                                   {
                                                       HRME_Id = a.HRME_Id,
                                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) + " " +
                                                       (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " +
                                                       (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName)).Trim(),
                                                   }).Distinct().ToArray();

                                List<StudentProgressCardReportDTO> result = new List<StudentProgressCardReportDTO>();


                                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Details";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 450000000;
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject = new List<dynamic>();

                                    try
                                    {
                                        using (var dataReader = cmd.ExecuteReader())
                                        {
                                            while (dataReader.Read())
                                            {
                                                result.Add(new StudentProgressCardReportDTO
                                                {
                                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                                    ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),

                                                    ASMAY_Year = (dataReader["ASMAY_Year"].ToString() == null || dataReader["ASMAY_Year"].ToString() == "" ? "" : dataReader["ASMAY_Year"].ToString()),

                                                    AMST_FatherName = (dataReader["AMST_FatherName"].ToString() == null || dataReader["AMST_FatherName"].ToString() == "" ? "" : dataReader["AMST_FatherName"].ToString()),
                                                    AMST_MotherName = (dataReader["AMST_MotherName"].ToString() == null || dataReader["AMST_MotherName"].ToString() == "" ? "" : dataReader["AMST_MotherName"].ToString()),

                                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                                    ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                                    ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                                    ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                                    ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                                    EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                                    EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                                    ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                                    ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                                    ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                                    ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                                    ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                                    ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                                    ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                                    EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                                    ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString())
                                                });

                                                data.savelist = result.OrderBy(t => t.EYCES_SubjectOrder).ToList();
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }

                                var from_date = (from a in _context.Exm_Category_ClassDMO
                                                 from b in _context.Exm_Yearly_CategoryDMO
                                                 from c in _context.Exm_Yearly_Category_ExamsDMO
                                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                                                 && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id
                                                 && b.ASMAY_Id == data.ASMAY_Id)
                                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                                var to_date = (from a in _context.Exm_Category_ClassDMO
                                               from b in _context.Exm_Yearly_CategoryDMO
                                               from c in _context.Exm_Yearly_Category_ExamsDMO
                                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                               select c.EYCE_AttendanceToDate).Max();

                                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Portal_JSHS_Exam_Student_Attendance_Details";
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject1 = new List<dynamic>();

                                    try
                                    {
                                        using (var dataReader = cmd.ExecuteReader())
                                        {
                                            while (dataReader.Read())
                                            {
                                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                                {
                                                    dataRow1.Add(
                                                        dataReader.GetName(iFiled1),
                                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                    );
                                                }

                                                retObject1.Add((ExpandoObject)dataRow1);
                                            }
                                        }
                                        data.Present_attendence = retObject1.ToArray();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                                data.savelisttot = _context.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.AMST_Id == data.AMST_Id).Distinct().ToArray();

                                data.subjlist = data.savelist.Distinct<StudentProgressCardReportDTO>(new portalprogressEqualityComparerjhs()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();


                                List<int> grade = new List<int>();
                                foreach (StudentProgressCardReportDTO x in data.subjlist)
                                {
                                    grade.Add(x.EMGR_Id);
                                }

                                data.grade_details = (from a in _context.Exm_Master_GradeDMO
                                                      from b in _context.Exm_Master_Grade_DetailsDMO
                                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                                      select b
                                                     ).Distinct().ToArray();

                                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true
                                && a.AMST_Id == data.AMST_Id).Distinct().ToArray();

                                data.getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                                          from b in _context.Adm_M_Student
                                                          from c in _context.School_M_Class
                                                          from d in _context.School_M_Section
                                                          from e in _context.AcademicYearDMO
                                                          where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id
                                                          && a.ASMS_Id == d.ASMS_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                                          && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_Id
                                                          && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id)
                                                          select new StudentProgressCardReportDTO
                                                          {
                                                              AMST_Id = a.AMST_Id,
                                                              photoname = b.AMST_Photoname
                                                          }).Distinct().ToArray();
                            }
                            else
                            {
                                data.message = "NotPublished";
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    else if (data.examorterm == "Term")
                    {
                        var getgradeid = _context.CCE_Exam_M_TermsDMO.Where(a => a.ECT_Id == data.ECT_Id && a.ECT_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                        && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Select(a => a.EMGR_Id).FirstOrDefault();

                        var gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ECT_Id == data.ECT_Id).ToList();

                        data.gettermdetails = gettermdetails.ToArray();


                        var getemeids = _context.Exm_CCE_TERMS_EXAMSDMO.Where(a => a.ECT_Id == data.ECT_Id
                        && a.ECTEX_ActiveFlag == true).Select(a => a.EME_Id).ToList();

                        var get_publishexamcount = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ESTMP_PublishToStudentFlg == true
                        && getemeids.Contains(a.EME_Id)).Select(a => a.EME_Id).ToList();

                        data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                                   from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                                   from c in _context.exammasterDMO
                                                   where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                   && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && b.ECT_Id == data.ECT_Id)
                                                   select new StudentProgressCardReportDTO
                                                   {
                                                       ECT_Id = b.ECT_Id,
                                                       EME_Id = b.EME_Id,
                                                       EME_ExamName = c.EME_ExamName,
                                                       EME_ExamOrder = c.EME_ExamOrder,
                                                       ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue

                                                   }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                        var Count_CheckDetails = 0;

                        if (getspflag == "1")
                        {
                            if (indiantime0.Date >= gettermdetails.FirstOrDefault().ECT_PublishDate.Value.Date)
                            {
                                if (getemeids.Count == get_publishexamcount.Count)
                                {
                                    Count_CheckDetails = 1;
                                }
                            }
                        }
                        else
                        {
                            if (indiantime0.Date >= geteycid.FirstOrDefault().EYC_MarksPublishDate.Value.Date)
                            {
                                if (getemeids.Count == get_publishexamcount.Count)
                                {
                                    Count_CheckDetails = 1;
                                }
                            }
                        }

                        if (Count_CheckDetails > 0)
                        {
                            // STUDENT DETAILS //
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentdetailsreport = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // STUDENT WISE SUBJECT DETAILS //
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwisesubjectlist = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // STUDENT WISE SKILLS LIST
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 3 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwiseskillslist = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // STUDENT WISE ACTIVITES LIST
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 4 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwiseactiviteslist = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // STUDENT WISE SPORTS LIST
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 5 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwisesportsdetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // STUDENT WISE ATTENDANCE LIST
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 6 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwiseattendancedetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // STUDENT WISE TERM REMARKS LIST
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Individual_Term_Student_And_Others_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 7 });
                                cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwisetermwisedetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            // GET STUDENT WISE EXAM MARKS LIST
                            if (getspflag == "1")
                            {
                                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Portal_JSHS_Exam_Individual_CCE_Term_Report";
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                      SqlDbType.VarChar)
                                    {
                                        Value = data.MI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                                 SqlDbType.VarChar)
                                    {
                                        Value = data.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                                        SqlDbType.VarChar)
                                    {
                                        Value = data.ASMCL_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                                        SqlDbType.VarChar)
                                    {
                                        Value = data.ASMS_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                                        SqlDbType.VarChar)
                                    {
                                        Value = data.ECT_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                                    SqlDbType.VarChar)
                                    {
                                        Value = data.AMST_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                                    SqlDbType.VarChar)
                                    {
                                        Value = getgradeid
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
                                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                                {
                                                    dataRow1.Add(
                                                        dataReader.GetName(iFiled1),
                                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                    );
                                                }
                                                retObject.Add((ExpandoObject)dataRow1);
                                            }
                                        }
                                        data.getstudentmarksdetails = retObject.ToArray();
                                    }
                                    catch (Exception ee)
                                    {
                                        Console.WriteLine(ee.Message);
                                    }
                                }
                            }

                            else if (getspflag == "2")
                            {
                                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id
                                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                                        select new StudentProgressCardReportDTO
                                                        {
                                                            EMPG_GroupName = c.EMPSG_GroupName,
                                                            EMPG_DistplayName = c.EMPSG_DisplayName,
                                                            EMPSG_PercentValue = a.EMP_MarksPerFlg == "M" ? c.EMPSG_MarksValue : c.EMPSG_PercentValue,
                                                        }).Distinct().OrderBy(a => a.EMPG_GroupName).ToArray();

                                data.getpromotionmarksdetails = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id).ToArray();


                                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Portal_JSHS_CCE_Exam_IX_Term_Report";
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ECT_Id", SqlDbType.VarChar) { Value = data.ECT_Id });
                                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject = new List<dynamic>();

                                    try
                                    {
                                        using (var dataReader = cmd.ExecuteReader())
                                        {
                                            while (dataReader.Read())
                                            {
                                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                                {
                                                    dataRow1.Add(
                                                        dataReader.GetName(iFiled1),
                                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                    );
                                                }
                                                retObject.Add((ExpandoObject)dataRow1);
                                            }
                                        }
                                        data.getstudentmarksdetails = retObject.ToArray();
                                    }
                                    catch (Exception ee)
                                    {
                                        Console.WriteLine(ee.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            data.message = "NotPublished";
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

        // BGHS Studnet Progress Card Report Display In Student Portal
        public StudentProgressCardReportDTO Bghsgetdetails(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = GetStudentDetails(data.AMST_Id, data.MI_Id, data.ASMAY_Id);
                data.getstudentdetails = getstudentdetails.ToArray();

                data.getclass = GetClassDetails(data.AMST_Id, data.MI_Id);

                data.getyear = GetYearDetails(data.AMST_Id, data.MI_Id);


                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true && a.EPCFT_ExamFlag == data.EPCFT_ExamFlag).Distinct().ToList();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Term_List_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = getflag });

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
                            data.getexamtermlist = retObject.ToArray();
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
        public StudentProgressCardReportDTO Bghsonchangeclass(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true && a.EPCFT_ExamFlag == data.EPCFT_ExamFlag).Distinct().ToList();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Term_List_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = getflag });

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
                            data.getexamtermlist = retObject.ToArray();
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
        public StudentProgressCardReportDTO Bghsgetreport(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    if (data.examorterm == "Exam")
                    {
                        try
                        {
                            data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                            data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                               from b in _context.HR_Master_Employee_DMO
                                               where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                               && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                               select new StudentProgressCardReportDTO
                                               {
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) + " " +
                                                   (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " +
                                                   (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName)).Trim(),
                                               }).Distinct().ToArray();

                            List<StudentProgressCardReportDTO> result = new List<StudentProgressCardReportDTO>();

                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Details";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 450000000;
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                                {
                                    Value = data.ASMCL_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                                {
                                    Value = data.ASMS_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                                {
                                    Value = data.EME_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                                {
                                    Value = data.AMST_Id
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
                                            result.Add(new StudentProgressCardReportDTO
                                            {
                                                ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                                ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                                ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                                EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                                ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                                ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),

                                                ASMAY_Year = (dataReader["ASMAY_Year"].ToString() == null || dataReader["ASMAY_Year"].ToString() == "" ? "" : dataReader["ASMAY_Year"].ToString()),

                                                AMST_FatherName = (dataReader["AMST_FatherName"].ToString() == null || dataReader["AMST_FatherName"].ToString() == "" ? "" : dataReader["AMST_FatherName"].ToString()),
                                                AMST_MotherName = (dataReader["AMST_MotherName"].ToString() == null || dataReader["AMST_MotherName"].ToString() == "" ? "" : dataReader["AMST_MotherName"].ToString()),

                                                AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                                AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                                AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                                AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                                AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                                ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                                ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                                ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                                ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                                ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                                ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                                ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                                ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                                EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                                EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                                EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                                EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                                classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                                classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                                graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                                ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                                ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                                ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                                ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                                ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                                ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                                ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                                EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                                EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                                EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                                ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString())
                                            });

                                            data.savelist = result.OrderBy(t => t.EYCES_SubjectOrder).ToList();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            var from_date = (from a in _context.Exm_Category_ClassDMO
                                             from b in _context.Exm_Yearly_CategoryDMO
                                             from c in _context.Exm_Yearly_Category_ExamsDMO
                                             where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                                             && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id
                                             && b.ASMAY_Id == data.ASMAY_Id)
                                             select c.EYCE_AttendanceFromDate).FirstOrDefault();

                            var to_date = (from a in _context.Exm_Category_ClassDMO
                                           from b in _context.Exm_Yearly_CategoryDMO
                                           from c in _context.Exm_Yearly_Category_ExamsDMO
                                           where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                           select c.EYCE_AttendanceToDate).Max();

                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Portal_JSHS_Exam_Student_Attendance_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                                {
                                    Value = data.ASMCL_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                                {
                                    Value = data.ASMS_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date)
                                {
                                    Value = from_date
                                });

                                cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date)
                                {
                                    Value = to_date
                                });

                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                                {
                                    Value = data.AMST_Id
                                });


                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject1 = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }

                                            retObject1.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.Present_attendence = retObject1.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            data.savelisttot = _context.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.AMST_Id == data.AMST_Id).Distinct().ToArray();

                            data.subjlist = data.savelist.Distinct<StudentProgressCardReportDTO>(new portalprogressEqualityComparerjhs()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();


                            List<int> grade = new List<int>();
                            foreach (StudentProgressCardReportDTO x in data.subjlist)
                            {
                                grade.Add(x.EMGR_Id);
                            }

                            data.grade_details = (from a in _context.Exm_Master_GradeDMO
                                                  from b in _context.Exm_Master_Grade_DetailsDMO
                                                  where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                                  select b
                                                 ).Distinct().ToArray();

                            data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true
                            && a.AMST_Id == data.AMST_Id).Distinct().ToArray();

                            data.getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                                      from b in _context.Adm_M_Student
                                                      from c in _context.School_M_Class
                                                      from d in _context.School_M_Section
                                                      from e in _context.AcademicYearDMO
                                                      where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                                      && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id
                                                      && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id)
                                                      select new StudentProgressCardReportDTO
                                                      {
                                                          AMST_Id = a.AMST_Id,
                                                          photoname = b.AMST_Photoname

                                                      }).Distinct().ToArray();

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    else if (data.examorterm == "Promotion" && data.spflag == "1")
                    {
                        /* 9th Class Promotion Report */
                        data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                        data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                        && a.ECT_ActiveFlag == true).OrderBy(a => a.ECT_TermName).Distinct().ToArray();

                        data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                                   from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                                   from c in _context.exammasterDMO
                                                   where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                   && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true
                                                   && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id)
                                                   select new StudentProgressCardReportDTO
                                                   {
                                                       ECT_Id = b.ECT_Id,
                                                       EME_Id = b.EME_Id,
                                                       EME_ExamName = c.EME_ExamName,
                                                       EME_ExamOrder = c.EME_ExamOrder,
                                                       ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                                   }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                        // STUDENT DETAILS //
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Exam_BGHS_Student_SubjectWise_Marks_Details";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT SUBJECT WISE DETAILS //
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Exam_BGHS_Student_SubjectWise_Marks_Details";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesubjectlist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE ATTENDANCE DETAILS //
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Exam_BGHS_Student_SubjectWise_Marks_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 3 });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwiseattendancedetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE MARKS DETAILS 
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Exam_BGHS_Student_SubjectWise_Marks_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 4 });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentmarksdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // CLASS TEACHER NAME
                        data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                           from b in _context.HR_Master_Employee_DMO
                                           where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                           select new StudentProgressCardReportDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                               (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                               (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                           }).Distinct().ToArray();
                    }

                    else if (data.examorterm == "Promotion" && data.spflag == "2")
                    {
                        // STUDENT DETAILS //
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Exam_Portal_BGHS_Student_SubjectWise_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = "" });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE SUBJECT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Exam_Portal_BGHS_Student_SubjectWise_Details_Promotion";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMCL_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMS_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                            {
                                Value = "3"
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                            {
                                Value = data.AMST_Id
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
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesubjectlist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE MARKS DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Exam_Portal_BGHS_Studentwise_Marks_Details_Promotion_New";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentmarksdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        data.getexamdetails = (from a in _context.Exm_M_PromotionDMO
                                               from b in _context.Exm_M_Promotion_SubjectsDMO
                                               from c in _context.Exm_M_Prom_Subj_GroupDMO
                                               where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id
                                               && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                               select new StudentProgressCardReportDTO
                                               {
                                                   EMPG_GroupName = c.EMPSG_GroupName,
                                                   EMPSG_Order = c.EMPSG_Order,
                                                   EMPG_DistplayName = c.EMPSG_DisplayName,
                                                   EMPSG_MarksValue = c.EMPSG_MarksValue

                                               }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                        data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                                    from b in _context.Exm_M_Promotion_SubjectsDMO
                                                    from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                    from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                                    from e in _context.exammasterDMO
                                                    where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                                    && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                                    && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                                    select new StudentProgressCardReportDTO
                                                    {
                                                        EMPG_GroupName = c.EMPSG_GroupName,
                                                        EME_Id = d.EME_Id,
                                                        EME_ExamName = e.EME_ExamName,
                                                        EME_ExamOrder = e.EME_ExamOrder,
                                                        EMPG_DistplayName = c.EMPSG_DisplayName
                                                    }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                        List<long> emeid = new List<long>();

                        var getemeids = (from a in _context.Exm_M_PromotionDMO
                                         from b in _context.Exm_M_Promotion_SubjectsDMO
                                         from c in _context.Exm_M_Prom_Subj_GroupDMO
                                         from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                         from e in _context.exammasterDMO
                                         where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                         && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                         && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                         select new StudentProgressCardReportDTO
                                         {
                                             EME_Id = d.EME_Id,
                                         }).Distinct().ToList();

                        foreach (var c in getemeids)
                        {
                            emeid.Add(c.EME_Id);
                        }

                        data.getclassteacher = (from a in _context.ClassTeacherMappingDMO
                                                from b in _context.HR_Master_Employee_DMO
                                                where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                                select new StudentProgressCardReportDTO
                                                {
                                                    classteachername = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                                    (b.HRME_EmployeeMiddleName == null ? "" : " " + b.HRME_EmployeeMiddleName) +
                                                    (b.HRME_EmployeeLastName == null ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                                }).Distinct().ToArray();


                        data.getexammaxmarks = (from a in _context.Exm_Category_ClassDMO
                                                from b in _context.Exm_Master_CategoryDMO
                                                from c in _context.Exm_Yearly_CategoryDMO
                                                from d in _context.Exm_Yearly_Category_ExamsDMO
                                                from e in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                from f in _context.exammasterDMO
                                                from g in _context.IVRM_Master_SubjectsDMO
                                                where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EYCE_Id == e.EYCE_Id
                                                && d.EME_Id == f.EME_Id && e.ISMS_Id == g.ISMS_Id && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true
                                                && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true && e.EYCES_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id
                                                && c.MI_Id == data.MI_Id && emeid.Contains(d.EME_Id) && e.EYCES_AplResultFlg == true)
                                                select new StudentProgressCardReportDTO
                                                {
                                                    EME_Id = d.EME_Id,
                                                    EME_ExamName = f.EME_ExamName,
                                                    EME_ExamOrder = f.EME_ExamOrder,
                                                    EYCES_MaxMarks = e.EYCES_MaxMarks
                                                }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Stmary Malda student progress card report display in student portal
        public StudentProgressCardReportDTO stmarygetdetails(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = GetStudentDetails(data.AMST_Id, data.MI_Id, data.ASMAY_Id);
                data.getstudentdetails = getstudentdetails.ToArray();

                data.getclass = GetClassDetails(data.AMST_Id, data.MI_Id);

                data.getyear = GetYearDetails(data.AMST_Id, data.MI_Id);


                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Term_List_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = getflag });

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
                            data.getexamtermlist = retObject.ToArray();
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
        public StudentProgressCardReportDTO stmaryonchangeclass(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = GetStudentDetailsOnChangeClass(data.AMST_Id, data.MI_Id, data.ASMAY_Id, data.ASMCL_Id);
                data.getstudentdetails = getstudentdetails.ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                List<EXM_ProgressCard_FormatsDMO> getexamflag = new List<EXM_ProgressCard_FormatsDMO>();

                if (data.EPCFT_ExamFlag != null && data.EPCFT_ExamFlag != "")
                {
                    getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                    && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true && a.EPCFT_ExamFlag == data.EPCFT_ExamFlag).Distinct().ToList();
                }
                else
                {
                    getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                    && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();
                }


                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_JSHS_Exam_Get_Exam_Term_List_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = getflag });

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
                            data.getexamtermlist = retObject.ToArray();
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
        public StudentProgressCardReportDTO stmarygetreport(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                data.getyear = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();


                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                   && a.IMCT_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                   select new StudentProgressCardReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();
                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    // STUDENT LIST
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_Student_Exam_Promotion_Final_Report_Malda";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 450000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                        {
                            Value = 1
                        });

                        cmd.Parameters.Add(new SqlParameter("@Student_Id", SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    // SUBJECT LIST
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_Student_Exam_Promotion_Final_Report_Malda";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 450000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                        {
                            Value = 3
                        });

                        cmd.Parameters.Add(new SqlParameter("@Student_Id", SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getsubjectlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    // EXAM LIST
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_Student_Exam_Promotion_Final_Report_Malda";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 450000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                        {
                            Value = 2
                        });

                        cmd.Parameters.Add(new SqlParameter("@Student_Id", SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getexamlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    // SAVED LIST
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_Student_Exam_Promotion_Final_Report_Malda";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 450000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                        {
                            Value = 4
                        });

                        cmd.Parameters.Add(new SqlParameter("@Student_Id", SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getsavedlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    data.remarks = _context.ExamPromotionRemarksDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EPRD_Promotionflag == "PE").ToArray();

                    var EMCA_Ids = _context.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                    var EYC_Ids = _context.Exm_Yearly_CategoryDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Ids
                    && t.EYC_ActiveFlg == true).EYC_Id;

                    var EMGR_Ids = _context.Exm_M_PromotionDMO.Single(t => t.EYC_Id == EYC_Ids && t.EMP_ActiveFlag == true).EMGR_Id;

                    data.grade_detailslist = _context.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Ids && t.EMGD_ActiveFlag == true).Distinct().OrderByDescending(t => t.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // HHS student progress card report display in student portal
        public StudentProgressCardReportDTO HHSStudentProgressCardReport(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                data.getyear = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                   && a.IMCT_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                   select new StudentProgressCardReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    var EPCFT_ExamwiseFlg = getexamflag.FirstOrDefault().EPCFT_ExamwiseFlg;
                    data.examflag = EPCFT_ExamwiseFlg;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    try
                    {
                        List<int?> ids = new List<int?>();
                        ids.Add(0);
                        ids.Add(1);

                        List<string> sol = new List<string>();
                        sol.Add("S");
                        sol.Add("L");
                        sol.Add("D");

                        data.stu_details = (from f in _context.Adm_M_Student
                                            from h in _context.School_Adm_Y_StudentDMO
                                            from c in _context.School_M_Class
                                            from s in _context.School_M_Section
                                            from y in _context.AcademicYearDMO
                                            where (h.AMST_Id == f.AMST_Id && ids.Contains(f.AMST_ActiveFlag) && sol.Contains(f.AMST_SOL) && ids.Contains(h.AMAY_ActiveFlag)
                                            && h.ASMAY_Id == y.ASMAY_Id && h.ASMAY_Id == y.ASMAY_Id && h.ASMCL_Id == c.ASMCL_Id && h.ASMS_Id == s.ASMS_Id
                                            && h.ASMAY_Id == y.ASMAY_Id && h.AMST_Id == data.AMST_Id && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id
                                            && h.ASMS_Id == data.ASMS_Id)
                                            select new StudentProgressCardReportDTO
                                            {
                                                AMST_Id = h.AMST_Id,
                                                Stuname = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                AMST_AdmNo = f.AMST_AdmNo,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = s.ASMC_SectionName,
                                                AMST_FatherName = ((f.AMST_FatherName != null && f.AMST_FatherName != "" ? f.AMST_FatherName : "") +
                                        (f.AMST_FatherSurname != null && f.AMST_FatherSurname != "" ? "" : " " + f.AMST_FatherSurname)).Trim(),
                                                AMST_MotherName = ((f.AMST_MotherName != null && f.AMST_MotherName != "" ? f.AMST_MotherName : "") +
                                        (f.AMST_MotherSurname != null && f.AMST_MotherSurname != "" ? "" : " " + f.AMST_MotherSurname)).Trim(),
                                                AMAY_RollNo = h.AMAY_RollNo,
                                                AMST_RegistrationNo = f.AMST_RegistrationNo,
                                                AMST_PerStreet = f.AMST_PerStreet,
                                                AMST_PerArea = f.AMST_PerArea,
                                                AMST_PerCity = f.AMST_PerCity,
                                                AMST_DOB = f.AMST_DOB,
                                                ASMAY_Year = y.ASMAY_Year,
                                                IVRMMS_Name = f.AMST_PerState != null && f.AMST_PerState > 0 ? _context.state.Where(a => a.IVRMMS_Id == f.AMST_PerState).Distinct().FirstOrDefault().IVRMMS_Name : "",

                                                IVRMMC_CountryName = f.AMST_PerCountry != null && f.AMST_PerCountry > 0 ? _context.country.Where(a => a.IVRMMC_Id == f.AMST_PerCountry).Distinct().FirstOrDefault().IVRMMC_CountryName : "",

                                                AMST_PerPincode = f.AMST_PerPincode,
                                                AMST_Photoname = f.AMST_Photoname,
                                                AMST_EmailId = f.AMST_emailId,
                                                AMST_Mobile = f.AMST_MobileNo
                                            }).Distinct().ToArray();

                        List<int> emeids = new List<int>();

                        var getemeids = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id
                        && a.ESTMP_PublishToStudentFlg == true).Select(a => a.EME_Id).Distinct().ToList();

                        var EMCA_Id = _context.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;

                        var EYC_Id = _context.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;

                        var EMGR_Id = _context.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true).Select(a => a.EMGR_Id).FirstOrDefault();

                        //Eaxm Subject Sub-Subject Marks
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Student_Exam_Promotion_Final_Report_HHS";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar){Value = data.ASMAY_Id});
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar){Value = data.ASMCL_Id});
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar){Value = data.ASMS_Id});
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar){Value = data.AMST_Id});
                            cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar){Value = "1"});
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.eam_sub_mrks_list = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        //Total Percentage and Rank
                        var exmrank = (from t in _context.ExmStudentMarksProcessDMO
                                       from b in _context.exammasterDMO
                                       where (t.EME_Id == b.EME_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                                       && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && b.EME_ActiveFlag == true && getemeids.Contains(t.EME_Id))
                                       select new StudentProgressCardReportDTO
                                       {
                                           EME_FinalExamFlag = b.EME_FinalExamFlag,
                                           ESTMP_SectionRank = Convert.ToInt32(t.ESTMP_SectionRank),
                                           ESTMP_Percentage = Convert.ToDecimal(t.ESTMP_Percentage),
                                           EME_Id = t.EME_Id,
                                           ESTMP_TotalObtMarks = Convert.ToDecimal(t.ESTMP_TotalObtMarks),
                                           ESTMP_TotalGrade = t.ESTMP_TotalGrade
                                       }).Distinct().ToList();

                        data.exmTPR = exmrank.ToArray();

                        data.promotionrank = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id).ToArray();

                        data.remarkslist = _context.exammasterRemarkDMO.Where(a => a.MI_Id == data.MI_Id && a.EPCR_ActiveFlag == true).OrderBy(a => a.EPCR_RemarksOrder).ToArray();

                        //Grade Deatails 

                        data.grade_detailslist = _context.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Id && t.EMGD_ActiveFlag == true).OrderByDescending(a => a.EMGD_From).ToArray();

                        data.examwiseremarks = (from a in _context.Exm_ProgressCard_RemarksDMO
                                                from b in _context.AcademicYearDMO
                                                from c in _context.School_M_Class
                                                from d in _context.School_M_Section
                                                from e in _context.School_Adm_Y_StudentDMO
                                                from f in _context.Adm_M_Student
                                                from g in _context.exammasterDMO
                                                where (a.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id
                                                 && a.ASMS_Id == d.ASMS_Id && e.ASMAY_Id == b.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == d.ASMS_Id
                                                 && a.EME_ID == g.EME_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                 && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && e.ASMAY_Id == data.ASMAY_Id
                                                 && e.ASMCL_Id == data.ASMCL_Id && e.ASMS_Id == data.ASMS_Id && e.AMST_Id == data.AMST_Id
                                                 && sol.Contains(f.AMST_SOL) && ids.Contains(f.AMST_ActiveFlag) && ids.Contains(f.AMST_ActiveFlag)
                                                 && a.EMER_ActiveFlag == true && getemeids.Contains(a.EME_ID))
                                                select new StudentProgressCardReportDTO
                                                {
                                                    EME_ExamName = g.EME_ExamName,
                                                    EMER_Remarks = a.EMER_Remarks,
                                                    EME_ExamOrder = g.EME_ExamOrder,
                                                    AMST_Id = a.AMST_Id
                                                }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Student_Exam_Promotion_Final_Report_HHS";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar){Value = data.ASMAY_Id});
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar){Value = data.ASMCL_Id});
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar){Value = data.ASMS_Id});
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar){Value = data.AMST_Id});
                            cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar){Value = "2"});

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.marksprocess_Pro_markscal = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.examflag == 1)
                        {
                            var exam_subexam = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                                from b in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                from c in _context.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                from d in _context.exammasterDMO
                                                from e in _context.mastersubexamDMO
                                                from f in _context.ExmStudentMarksProcessSubjectwiseDMO
                                                where (a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true && a.EYCE_SubExamFlg == true && b.EYCE_Id == a.EYCE_Id
                                                && b.EYCES_ActiveFlg == true && b.EYCES_SubExamFlg == true && c.EYCES_Id == b.EYCES_Id && c.EYCESSS_ActiveFlg == true
                                                && d.MI_Id == data.MI_Id && d.EME_Id == a.EME_Id && d.EME_ActiveFlag == true && e.MI_Id == data.MI_Id
                                                && e.EMSE_Id == c.EMSE_Id && e.EMSE_ActiveFlag == true && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id
                                                && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.EME_Id == a.EME_Id && f.ISMS_Id == b.ISMS_Id
                                                && f.AMST_Id == data.AMST_Id && getemeids.Contains(f.EME_Id))
                                                select new StudentProgressCardReportDTO
                                                {
                                                    EYCE_Id = a.EYCE_Id,
                                                    EME_Id = a.EME_Id,
                                                    EME_ExamName = d.EME_ExamName,
                                                    EYCESSE_Id = c.EYCESSS_Id,
                                                    EMSE_Id = e.EMSE_Id,
                                                    EMSE_SubExamName = e.EMSE_SubExamName,
                                                    EYCES_Id = b.EYCES_Id,
                                                    ISMS_Id = b.ISMS_Id,
                                                    ESTMPS_Id = f.ESTMPS_Id
                                                }).Distinct().ToList();


                            var exam_subexam_W = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                                  from b in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                  from c in _context.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                  from d in _context.exammasterDMO
                                                  from e in _context.mastersubexamDMO
                                                  where (a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true && a.EYCE_SubExamFlg == true && b.EYCE_Id == a.EYCE_Id
                                                  && b.EYCES_ActiveFlg == true && b.EYCES_SubExamFlg == true && c.EYCES_Id == b.EYCES_Id && c.EYCESSS_ActiveFlg == true
                                                  && d.MI_Id == data.MI_Id && d.EME_Id == a.EME_Id && d.EME_ActiveFlag == true && e.MI_Id == data.MI_Id
                                                  && e.EMSE_Id == c.EMSE_Id && e.EMSE_ActiveFlag == true && getemeids.Contains(a.EME_Id))
                                                  select new StudentProgressCardReportDTO
                                                  {
                                                      EYCE_Id = a.EYCE_Id,
                                                      EME_Id = a.EME_Id,
                                                      EME_ExamName = d.EME_ExamName,
                                                      EMSE_Id = e.EMSE_Id,
                                                      EMSE_SubExamName = e.EMSE_SubExamName,
                                                      EYCESSS_MaxMarks = c.EYCESSS_MaxMarks
                                                  }).Distinct().ToList();
                            data.exam_subexam_W = exam_subexam_W.OrderBy(t => t.EME_Id).ToArray();
                            data.exam_subexam = exam_subexam.ToArray();

                            data.personality_status = (from a in _context.Exm_PersonalityDMO
                                                       from b in _context.Exm_Student_PersonalityDMO
                                                       from c in _context.exammasterDMO
                                                       from e in _context.School_Adm_Y_StudentDMO
                                                       from f in _context.Adm_M_Student
                                                       where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.EP_Id == a.EP_Id && b.EME_Id == c.EME_Id
                                                       && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                       && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                       && b.ASMS_Id == data.ASMS_Id && getemeids.Contains(b.EME_Id))
                                                       select new StudentProgressCardReportDTO
                                                       {
                                                           EP_Id = a.EP_Id,
                                                           EP_PersonlaityName = a.EP_PersonlaityName,
                                                           EPCR_RemarksName = b.ESPM_Remarks
                                                       }).Distinct().ToArray();


                            data.personalitymonth = (from a in _context.Exm_PersonalityDMO
                                                     from b in _context.Exm_Student_PersonalityDMO
                                                     from c in _context.exammasterDMO
                                                     from e in _context.School_Adm_Y_StudentDMO
                                                     from f in _context.Adm_M_Student
                                                     where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.EP_Id == a.EP_Id && b.EME_Id == c.EME_Id
                                                             && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                             && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                             && b.ASMS_Id == data.ASMS_Id && getemeids.Contains(b.EME_Id))
                                                     select new StudentProgressCardReportDTO
                                                     {
                                                         EME_Id = b.EME_Id,
                                                         EME_ExamName = c.EME_ExamName,
                                                         EME_ExamOrder = c.EME_ExamOrder
                                                     }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                            //  --------------------CO - CURRICULAR ACTIVITIES

                            data.co_curricular_activity = (from a in _context.exammasterCoCulrricularDMO
                                                           from b in _context.Exm_Student_CoCurricularDMO
                                                           from c in _context.exammasterDMO
                                                           from e in _context.School_Adm_Y_StudentDMO
                                                           from f in _context.Adm_M_Student
                                                           where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.ECC_Id == a.ECC_Id && b.EME_Id == c.EME_Id
                                                           && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                           && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                           && b.ASMS_Id == data.ASMS_Id && getemeids.Contains(b.EME_Id))
                                                           select new StudentProgressCardReportDTO
                                                           {
                                                               ECC_Id = a.ECC_Id,
                                                               ECC_CoCurricularName = a.ECC_CoCurricularName,
                                                               EME_Id = b.EME_Id,
                                                               EME_ExamName = c.EME_ExamName,
                                                               EPCR_RemarksName = b.ESCOM_Remarks
                                                           }).Distinct().ToArray();


                            data.cocurillarymonth = (from a in _context.exammasterCoCulrricularDMO
                                                     from b in _context.Exm_Student_CoCurricularDMO
                                                     from c in _context.exammasterDMO
                                                     from e in _context.School_Adm_Y_StudentDMO
                                                     from f in _context.Adm_M_Student
                                                     where (f.AMST_Id == e.AMST_Id && a.MI_Id == b.MI_Id && b.ECC_Id == a.ECC_Id && b.EME_Id == c.EME_Id
                                                     && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.AMST_Id == e.AMST_Id
                                                     && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                     && b.ASMS_Id == data.ASMS_Id && getemeids.Contains(b.EME_Id))
                                                     select new StudentProgressCardReportDTO
                                                     {
                                                         EME_Id = b.EME_Id,
                                                         EME_ExamName = c.EME_ExamName,
                                                         EME_ExamOrder = c.EME_ExamOrder
                                                     }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                            //Atteadence

                            var list1 = _context.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();
                            data.yearlistdate = list1.ToArray();

                            data.strtdate = Convert.ToDateTime(list1[0].ASMAY_From_Date.ToString());
                            data.enddate = Convert.ToDateTime(list1[0].ASMAY_To_Date.ToString());


                            //find the middle date
                            double totaldays = 0;
                            double days = 0;
                            totaldays = (data.enddate - data.strtdate).TotalDays;
                            if (totaldays > 1)
                            {
                                days = (totaldays / 2);
                            }
                            data.middledate = data.strtdate.AddDays(days);


                            ///Final attendance
                            List<StudentProgressCardReportDTO> result = new List<StudentProgressCardReportDTO>();
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "StudentAttendance_HHS_Report";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                                cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date){Value = data.strtdate});
                                cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date){Value = data.enddate});
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = data.AMST_Id});

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            result.Add(new StudentProgressCardReportDTO
                                            {
                                                totalpresentday = Convert.ToDecimal(dataReader["TotalPresentDays"].ToString()),
                                                totalworkingday = Convert.ToDecimal(dataReader["TotalSchoolDays"].ToString()),
                                            });
                                        }
                                    }
                                    data.fullyearatt = result.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            //half att
                            List<StudentProgressCardReportDTO> result1 = new List<StudentProgressCardReportDTO>();
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "StudentAttendance_HHS_Report";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                                cmd.Parameters.Add(new SqlParameter("@from",SqlDbType.Date){Value = data.strtdate});
                                cmd.Parameters.Add(new SqlParameter("@to",SqlDbType.Date){Value = data.middledate});
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = data.AMST_Id});

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            result1.Add(new StudentProgressCardReportDTO
                                            {
                                                totalpresentday = Convert.ToDecimal(dataReader["TotalPresentDays"].ToString()),
                                                totalworkingday = Convert.ToDecimal(dataReader["TotalSchoolDays"].ToString()),

                                            });
                                        }
                                    }

                                    data.halfyearatt = result1.ToArray();
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
                                cmd.CommandText = "HHS_Get_Exam_Attendance";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar){Value = data.ASMAY_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar){Value = data.ASMCL_Id});
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar){Value = data.ASMS_Id});
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar){Value = data.AMST_Id});

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(
                                                    dataReader.GetName(iFiled1),
                                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.Present_attendence = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                        var checkpromotion = _context.Exm_Student_MP_PromotionDMO.Where(p => p.MI_Id == data.MI_Id
                         && p.AMST_Id == data.AMST_Id && p.ESTMPP_PublishToStudentFlg == true && p.ASMAY_Id == data.ASMAY_Id
                         && p.ASMCL_Id == data.ASMCL_Id && p.ASMS_Id == data.ASMS_Id).ToList();

                        if (checkpromotion.Count > 0)
                        {
                            data.promotionstatus = _context.ExamPromotionRemarksDMO.Where(p => p.MI_Id == data.MI_Id
                            && p.AMST_Id == data.AMST_Id && p.EPRD_Promotionflag == "PE" && p.ASMAY_Id == data.ASMAY_Id
                            && p.ASMCL_Id == data.ASMCL_Id && p.ASMS_Id == data.ASMS_Id).Distinct().ToArray();
                        }
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

        // Stjames Student Progress card report display in student portal
        public StudentProgressCardReportDTO Get_Stjames_Progresscard_Report(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                         && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true
                && a.AMST_Id == data.AMST_Id).Distinct().ToArray();

                data.studentwisemarks = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == data.AMST_Id).Distinct().ToArray();

                data.getexamdetails = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true
                && a.EME_Id == data.EME_Id).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    var EPCFT_ExamwiseFlg = getexamflag.FirstOrDefault().EPCFT_ExamwiseFlg;
                    data.examflag = EPCFT_ExamwiseFlg;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    //STUDENT DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Stjames_Portal_Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    if (data.spflag == "1")
                    {
                        // STUDENT WISE SUBJECT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Stjames_Portal_Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesubjectlist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // EXAM WISE SUBSUBJECT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Stjames_Portal_Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getexamwisesubsubjectlist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE MARKS DETAILS               
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Stjames_Portal_Exam_SubSubject_ProgressCard_Report";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMCL_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                            {
                                Value = data.EME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                            {
                                Value = data.AMST_Id
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
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentmarksdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }
                    }
                    else if (data.spflag == "2")
                    {
                        // STUDENT WISE MARKS DETAILS               
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Stjames_Portal_Exam_Subject_SubSubj_Marks_ProgressCard_Report";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMCL_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                            {
                                Value = data.EME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                            {
                                Value = "1"
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                            {
                                Value = data.AMST_Id
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
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentmarksdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE MARKS DETAILS               
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Stjames_Portal_Exam_Subject_SubSubj_Marks_ProgressCard_Report";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMCL_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                            {
                                Value = data.ASMS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                            {
                                Value = data.EME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                            {
                                Value = "2"
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                            {
                                Value = data.AMST_Id
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
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesubjectlistnew = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }
                    }
                }

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.EMER_ActiveFlag == true
                && a.EME_ID == data.EME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Notredame Student Progress Card Report Display In Student Portal
        public StudentProgressCardReportDTO NDS_Get_Progresscard_Report(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    var EPCFT_ExamwiseFlg = getexamflag.FirstOrDefault().EPCFT_ExamwiseFlg;
                    data.examflag = EPCFT_ExamwiseFlg;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    if (data.examflag == 1 || data.examflag == 2 || data.examflag == 3)
                    {
                        // STUDENT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "NDS_Portal_EXAM_GET_1_5_STUDENT_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        //STUDENT WISE SUBJECT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "NDS_Portal_EXAM_GET_1_5_STUDENT_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesubjectlist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE SKILLS LIST
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "NDS_Portal_EXAM_GET_1_5_STUDENT_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwiseskillslist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE ACTIVITES LIST
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "NDS_Portal_EXAM_GET_1_5_STUDENT_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwiseactiviteslist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE ATTENDANCE LIST
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "NDS_Portal_EXAM_GET_1_5_STUDENT_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "6" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwiseattendancedetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        // STUDENT WISE SPORTS LIST
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "NDS_Portal_EXAM_GET_1_5_STUDENT_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "7" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesportsdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        data.getsubjectwisetotaldetails = _context.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                         && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id).ToArray();

                        data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.ECT_ActiveFlag == true).ToArray();

                        data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                                   from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                                   from c in _context.exammasterDMO
                                                   where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                                   && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                                   && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id)
                                                   select new StudentProgressCardReportDTO
                                                   {
                                                       ECT_Id = b.ECT_Id,
                                                       EME_Id = b.EME_Id,
                                                       EME_ExamName = c.EME_ExamName,
                                                       EME_ExamOrder = c.EME_ExamOrder,
                                                       ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                                   }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                        data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                                from b in _context.Exm_M_Promotion_SubjectsDMO
                                                from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id
                                                && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                                && c.EMPSG_ActiveFlag == true)
                                                select new StudentProgressCardReportDTO
                                                {
                                                    EMPG_GroupName = c.EMPSG_GroupName,
                                                    EMPSG_Order = c.EMPSG_Order,
                                                    EMPG_DistplayName = c.EMPSG_DisplayName,
                                                    EMPSG_MarksValue = c.EMPSG_MarksValue
                                                }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                        data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                                    from b in _context.Exm_M_Promotion_SubjectsDMO
                                                    from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                    from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                                    from e in _context.exammasterDMO
                                                    where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                                    && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id
                                                    && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true
                                                    && d.EMPSGE_ActiveFlg == true)
                                                    select new StudentProgressCardReportDTO
                                                    {
                                                        EMPG_GroupName = c.EMPSG_GroupName,
                                                        EME_Id = d.EME_Id,
                                                        EME_ExamName = e.EME_ExamName,
                                                        EME_ExamCode = e.EME_ExamCode,
                                                        EME_ExamOrder = e.EME_ExamOrder,
                                                        EMPG_DistplayName = c.EMPSG_DisplayName,
                                                        EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                                    }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                        data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && a.AMST_Id == data.AMST_Id).ToArray();

                        data.getparticipatedetails = _context.Exm_Student_TermAchievementsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id).ToArray();

                        data.examwiseremarks = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECTERE_Indi_OverAllFlag == "IE" && a.AMST_Id == data.AMST_Id).ToArray();

                        //STUDENT WISE MARKS DETAILS
                        if (getspflag == "1")
                        {
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_Exam_1_5_ProgressCard_Report_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentmarksdetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }
                        }

                        else if (getspflag == "2")
                        {
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_Exam_9_ProgressCard_Report_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentmarksdetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }
                        }
                    }

                    else if (data.examflag == 4)
                    {
                        var getemeids = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id
                     && a.ESTMP_PublishToStudentFlg == true).Select(a => a.EME_Id).Distinct().ToList();

                        if (getemeids.Count > 0)
                        {
                            string emeids = "";
                            foreach (var c in getemeids)
                            {
                                if (emeids == "")
                                {
                                    emeids = c.ToString();
                                }
                                else
                                {
                                    emeids = emeids + "," + c.ToString();
                                }
                            }

                            data.getexam = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id
                            && getemeids.Contains(a.EME_Id)).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                            // CLASS TEACHER NAME
                            data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                               from b in _context.HR_Master_Employee_DMO
                                               where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                               && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                               select new StudentProgressCardReportDTO
                                               {
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                                   (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                                   (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                               }).Distinct().ToArray();

                            // STUDENT DETAILS
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentdetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            //STUDENT WISE SUBJECT DETAILS
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwisesubjectlist = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            //STUDENT WISE SUBJECT SUBSUBJECT DETAILS
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwisesubjectsubsubjectlist = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            //STUDENT WISE ATTENDANCE
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentwiseattendancedetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            //STUDENT WISE MARKS DETAILS
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "NDS_Portal_EXAM_GET_Jr_Sr_KG_ProgressCard_Report_Details";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();

                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                            {
                                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                            }
                                            retObject.Add((ExpandoObject)dataRow1);
                                        }
                                    }
                                    data.getstudentmarksdetails = retObject.ToArray();
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }

                            data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                             && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && a.AMST_Id == data.AMST_Id).ToArray();
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

        //BCEHS
        public StudentProgressCardReportDTO Get_BCEHS_Progresscard_Report(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                var yearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToList();

                data.ASMAY_Year = yearlist.FirstOrDefault().ASMAY_Year;

                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    var EPCFT_ExamwiseFlg = getexamflag.FirstOrDefault().EPCFT_ExamwiseFlg;
                    data.examflag = EPCFT_ExamwiseFlg;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    data.getpublishmarks = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == data.AMST_Id
                    && a.ESTMP_PublishToStudentFlg == true).ToArray();

                    if (data.getpublishmarks != null && data.getpublishmarks.Length > 0)
                    {
                        data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                           from b in _context.HR_Master_Employee_DMO
                                           where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                           select new StudentProgressCardReportDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                           }).Distinct().ToArray();

                        List<StudentProgressCardReportDTO> result = new List<StudentProgressCardReportDTO>();

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Baldwins_Exam_Get_Exam_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 450000000;
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt) { Value = data.EME_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        result.Add(new StudentProgressCardReportDTO
                                        {
                                            ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                            ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                            ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                            EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                            ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                            ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),
                                            AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                            AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                            AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                            AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                            AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                            ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                            ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                            ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                            ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                            ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                            ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                            ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                            ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                            EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                            EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                            EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                            EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                            classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                            classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                            graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                            ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                            ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                            ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                            ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                            ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                            ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                            ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                            EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                            EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                            EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                            ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString())
                                        });

                                        data.savelist = result.OrderBy(t => t.EYCES_SubjectOrder).ToList();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        var from_date = (from a in _context.Exm_Category_ClassDMO
                                         from b in _context.Exm_Yearly_CategoryDMO
                                         from c in _context.Exm_Yearly_Category_ExamsDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                                         && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id
                                         && b.ASMAY_Id == data.ASMAY_Id)
                                         select c.EYCE_AttendanceFromDate).FirstOrDefault();

                        var to_date = (from a in _context.Exm_Category_ClassDMO
                                       from b in _context.Exm_Yearly_CategoryDMO
                                       from c in _context.Exm_Yearly_Category_ExamsDMO
                                       where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                                       && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id
                                       && b.ASMAY_Id == data.ASMAY_Id)
                                       select c.EYCE_AttendanceToDate).Max();

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Baldwins_StudentAttendance_W";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {

                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }

                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.Work_attendence = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);

                            }
                        }

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Baldwin_StudentAttendance_P";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject1 = new List<dynamic>();

                            try
                            {

                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.Present_attendence = retObject1.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        data.savelisttot = _context.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();

                        data.subjlist = data.savelist.Distinct<StudentProgressCardReportDTO>(new portalprogressEqualityComparerjhs()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();

                        List<int> grade = new List<int>();
                        foreach (StudentProgressCardReportDTO x in data.subjlist)
                        {
                            grade.Add(x.EMGR_Id);
                        }

                        data.grade_details = (from a in _context.Exm_Master_GradeDMO
                                              from b in _context.Exm_Master_Grade_DetailsDMO
                                              where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                              select b).Distinct().ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //BIS
        public StudentProgressCardReportDTO BISStudentProgressCardReport(StudentProgressCardReportDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                         select new StudentProgressCardReportDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id
                                         }).Distinct().ToList();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var getexamflag = _context.EXM_ProgressCard_FormatsDMO.Where(a => a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EPCFT_ActiveFlg == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == getstudentdetails.FirstOrDefault().ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;

                var yearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToList();

                data.ASMAY_Year = yearlist.FirstOrDefault().ASMAY_Year;

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var Emp_id = _context.Exm_M_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id
                && a.EMP_ActiveFlag == true).Select(a => a.EMP_Id).FirstOrDefault();

                if (getexamflag.Count > 0)
                {
                    var getflag = getexamflag.FirstOrDefault().EPCFT_ExamFlag;
                    data.examorterm = getflag;

                    var getspflag = getexamflag.FirstOrDefault().EPCFT_SPFlag;
                    data.spflag = getspflag;

                    var EPCFT_ExamwiseFlg = getexamflag.FirstOrDefault().EPCFT_ExamwiseFlg;
                    data.examflag = EPCFT_ExamwiseFlg;

                    data.htmlstring = getexamflag.FirstOrDefault().EPCFT_ProgressCardFormat;

                    data.getpublishmarks = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.ESTMPP_PublishToStudentFlg == true).ToArray();

                    if (data.getpublishmarks != null && data.getpublishmarks.Length > 0)
                    {

                        // CLASS TEACHER NAME
                        data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                           from b in _context.HR_Master_Employee_DMO
                                           where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                           select new StudentProgressCardReportDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                               (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                               (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                           }).Distinct().ToArray();

                        // STUDENT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "PORTAL_BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        //STUDENT WISE SUBJECT DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "PORTAL_BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentwisesubjectlist = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        //STUDENT WISE MARKS DETAILS
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "PORTAL_BIS_Exam_ProgressCard_Report_Details_Modify";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.getstudentmarksdetails = retObject.ToArray();
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee.Message);
                            }
                        }

                        var from_date = _context.AcademicYearDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_From_Date;

                        var to_date = _context.AcademicYearDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_To_Date;

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Baldwins_StudentAttendance_W";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();

                            try
                            {

                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }

                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.Work_attendence = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);

                            }
                        }

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_Baldwin_StudentAttendance_P";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject1 = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                        }
                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.Present_attendence = retObject1.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        var getpromotionnonapplicablesubjects = _context.Exm_M_Promotion_SubjectsDMO.Where(a => a.EMP_Id == Emp_id && a.EMPS_ActiveFlag == true
                        && a.EMPS_AppToResultFlg == false).Select(a => a.ISMS_Id).ToList();

                        var nonapplicablesubject_examwisemarks = _context.ExmStudentMarksProcessSubjectwiseDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && getpromotionnonapplicablesubjects.Contains(a.ISMS_Id)).ToArray();

                        data.nonapplicablesubject_examwisemarks = nonapplicablesubject_examwisemarks.ToArray();

                        data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                                from b in _context.Exm_M_Promotion_SubjectsDMO
                                                from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id
                                                && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                                && c.EMPSG_ActiveFlag == true)
                                                select new StudentProgressCardReportDTO
                                                {
                                                    EMPG_GroupName = c.EMPSG_GroupName,
                                                    EMPSG_Order = c.EMPSG_Order,
                                                    EMPG_DistplayName = c.EMPSG_DisplayName,
                                                    EMPSG_PercentValue = a.EMP_MarksPerFlg == "P" ? c.EMPSG_PercentValue : c.EMPSG_MarksValue
                                                }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                        data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                                    from b in _context.Exm_M_Promotion_SubjectsDMO
                                                    from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                    from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                                    from e in _context.exammasterDMO
                                                    where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                                    && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id
                                                    && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true 
                                                    && d.EMPSGE_ActiveFlg == true)
                                                    select new StudentProgressCardReportDTO
                                                    {
                                                        EMPG_GroupName = c.EMPSG_GroupName,
                                                        EME_Id = d.EME_Id,
                                                        EME_ExamName = e.EME_ExamName,
                                                        EME_ExamCode = e.EME_ExamCode,
                                                        EME_ExamOrder = e.EME_ExamOrder,
                                                        EMPG_DistplayName = c.EMPSG_DisplayName,
                                                        EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                                    }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                        data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                         && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE").ToArray();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* Common Functions Load Time Default Current Year Details */
        public Array GetClassDetails(long AMST_Id, long MI_Id)
        {
            var getclasslist = (from a in _context.School_Adm_Y_StudentDMO
                                from b in _context.Adm_M_Student
                                where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                && a.AMST_Id == AMST_Id && b.AMST_Id == AMST_Id)
                                select new StudentProgressCardReportDTO
                                {
                                    ASMCL_Id = a.ASMCL_Id,
                                }).Distinct().ToList();

            List<long> classid = new List<long>();

            foreach (var c in getclasslist)
            {
                classid.Add(c.ASMCL_Id);
            }

            var classlist = _context.School_M_Class.Where(a => a.MI_Id == MI_Id && classid.Contains(a.ASMCL_Id) && a.ASMCL_ActiveFlag == true).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            return classlist;
        }
        public Array GetYearDetails(long AMST_Id, long MI_Id)
        {
            var getyearlist = (from a in _context.School_Adm_Y_StudentDMO
                               from b in _context.Adm_M_Student
                               where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                               && a.AMST_Id == AMST_Id && b.AMST_Id == AMST_Id)
                               select new StudentProgressCardReportDTO
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                               }).Distinct().ToList();

            List<long> yearid = new List<long>();

            foreach (var c in getyearlist)
            {
                yearid.Add(c.ASMAY_Id);
            }

            var getyear = _context.AcademicYearDMO.Where(a => a.MI_Id == MI_Id && yearid.Contains(a.ASMAY_Id) && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            return getyear;
        }
        public List<StudentProgressCardReportDTO> GetStudentDetails(long AMST_Id, long MI_Id, long ASMAY_Id)
        {
            var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                     from b in _context.Adm_M_Student
                                     where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                     && a.ASMAY_Id == ASMAY_Id && a.AMST_Id == AMST_Id && b.AMST_Id == AMST_Id)
                                     select new StudentProgressCardReportDTO
                                     {
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMS_Id = a.ASMS_Id,
                                         ASMAY_Id = a.ASMAY_Id
                                     }).Distinct().ToList();

            return getstudentdetails;
        }

        /* Common Functions On Change Of Class */
        public List<StudentProgressCardReportDTO> GetStudentDetailsOnChangeClass(long AMST_Id, long MI_Id, long ASMAY_Id, long ASMCL_Id)
        {

            var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                     from b in _context.Adm_M_Student
                                     where (a.AMST_Id == b.AMST_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                     && a.ASMCL_Id == ASMCL_Id && a.AMST_Id == AMST_Id && b.AMST_Id == AMST_Id)
                                     select new StudentProgressCardReportDTO
                                     {
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMS_Id = a.ASMS_Id,
                                         ASMAY_Id = a.ASMAY_Id
                                     }).Distinct().ToList();

            return getstudentdetails;
        }
    }

    class portalprogressEqualityComparerjhs : IEqualityComparer<StudentProgressCardReportDTO>
    {
        public bool Equals(StudentProgressCardReportDTO b1, StudentProgressCardReportDTO b2)
        {
            if (b2 == null && b1 == null)
                return true;
            else if (b1 == null | b2 == null)
                return false;
            else if (b1.ISMS_Id == b2.ISMS_Id)
                return true;
            else
                return false;
        }
        public int GetHashCode(StudentProgressCardReportDTO bx)
        {
            int hCode = Convert.ToInt32(bx.ISMS_Id);
            return hCode.GetHashCode();
        }
    }



}
