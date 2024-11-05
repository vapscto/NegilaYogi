using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class PromotionReportDetailsImpl : Interfaces.PromotionReportDetailsInterface
    {
        public ExamContext _context;
        public PromotionReportDetailsImpl(ExamContext u)
        {
            _context = u;
        }

        public PromotionReportDetailsDTO getdata(PromotionReportDetailsDTO data)
        {
            try
            {
                data.allAcademicYear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        
        public PromotionReportDetailsDTO onchangeyear(PromotionReportDetailsDTO data)
        {
            try
            {
                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new PromotionReportDetailsDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        data.allclasslist = (from a in _context.ClassTeacherMappingDMO
                                             from b in _context.AdmissionClass
                                             from c in _context.AcademicYear
                                             where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                             select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
                else
                {
                    data.allclasslist = (from a in _context.Exm_Category_ClassDMO
                                         from b in _context.AdmissionClass
                                         from c in _context.AcademicYear
                                         where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                         && a.ECAC_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                         select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public PromotionReportDetailsDTO onchangeclass(PromotionReportDetailsDTO data)
        {
            try
            {
                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new JSHSExamReportsDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        data.allsectionlist = (from a in _context.ClassTeacherMappingDMO
                                               from b in _context.School_M_Section
                                               from c in _context.AcademicYear
                                               from d in _context.AdmissionClass
                                               where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                               && a.ASMCL_Id == data.ASMCL_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true
                                               && b.ASMC_ActiveFlag == 1)
                                               select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.allsectionlist = (from a in _context.Exm_Category_ClassDMO
                                           from b in _context.School_M_Section
                                           from c in _context.AcademicYear
                                           from d in _context.AdmissionClass
                                           where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true && b.ASMC_ActiveFlag == 1 && a.ASMCL_Id == data.ASMCL_Id)
                                           select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PromotionReportDetailsDTO onchangesection(PromotionReportDetailsDTO data)
        {
            try
            {
                string order = "";

                var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                {
                    order = "AMST_FirstName";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                {
                    order = "AMST_AdmNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                {
                    order = "AMAY_RollNo";
                }
                else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                {
                    order = "AMST_RegistrationNo";
                }
                else
                {
                    order = "AMST_FirstName";
                }

                List<int?> ids = new List<int?>();
                if (data.Left_Flag == true || data.Deactive_Flag == true)
                {
                    ids.Add(0);
                }

                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");

                if (data.Left_Flag == true)
                {
                    sol.Add("L");
                }
                if (data.Deactive_Flag == true)
                {
                    sol.Add("D");
                }

                List<PromotionReportDetailsDTO> studentList = new List<PromotionReportDetailsDTO>();

                studentList = (from a in _context.Adm_M_Student
                               from b in _context.School_Adm_Y_StudentDMO
                               where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id 
                               && b.ASMAY_Id == data.ASMAY_Id && ids.Contains(b.AMAY_ActiveFlag) && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                               select new PromotionReportDetailsDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                   (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                   (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)+
                                   (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)).Trim(),
                                   AMST_AdmNo = a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : a.AMST_AdmNo,
                                   AMAY_RollNo = b.AMAY_RollNo,
                                   AMST_RegistrationNo = a.AMST_RegistrationNo == null || a.AMST_RegistrationNo == "" ? "" : a.AMST_RegistrationNo,
                                   AMST_DOB = a.AMST_DOB,
                                   AMST_SOL = a.AMST_SOL
                               }).Distinct().ToList();

                var propertyInfo = typeof(PromotionReportDetailsDTO).GetProperty(order);
                studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                data.studentlistdetails = studentList.ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = getemcaid > 0 ? _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true && a.EMCA_Id == getemcaid).Select(a => a.EYC_Id).FirstOrDefault() : 0;

                var getpromotiondetails = _context.Exm_M_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && a.EYC_Id == geteycid).ToList();

                data.getpromotiondetails = getpromotiondetails.ToArray();

                data.subjectwisetotal = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PromotionReportDetailsDTO Report(PromotionReportDetailsDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PromotionReportDetails_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "all" });
                    cmd.Parameters.Add(new SqlParameter("@reporttype", SqlDbType.VarChar) { Value = data.reporttype });

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
                        data.reportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PromotionReportDetails_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "iin" });
                    cmd.Parameters.Add(new SqlParameter("@reporttype", SqlDbType.VarChar) { Value = data.reporttype });

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
                        data.reportdata1 = retObject.ToArray();
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
        public PromotionReportDetailsDTO getpromotioncumulativereport(PromotionReportDetailsDTO data)
        {
            try
            {
                data.configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = getemcaid > 0 ? _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true && a.EMCA_Id == getemcaid).Select(a => a.EYC_Id).FirstOrDefault() : 0;

                var getpromotiondetails = _context.Exm_M_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && a.EYC_Id == geteycid).ToList();

                data.getpromotiondetails = getpromotiondetails.ToArray();

                if (getpromotiondetails != null && getpromotiondetails.Count > 0)
                {
                    string order = "AMST_FirstName";
                    var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                    if (get_configuration != null && get_configuration.Count > 0)
                    {
                        if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                        {
                            order = "AMST_FirstName";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                        {
                            order = "AMST_AdmNo";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                        {
                            order = "AMAY_RollNo";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                        {
                            order = "AMST_RegistrationNo";
                        }
                        else
                        {
                            order = "AMST_FirstName";
                        }
                    }                    
                    string AmstIds = string.Join(",", data.AMST_Ids);

                    List<PromotionReportDetailsDTO> studentList = new List<PromotionReportDetailsDTO>();

                    studentList = (from a in _context.Adm_M_Student
                                   from b in _context.School_Adm_Y_StudentDMO
                                   where (a.MI_Id == data.MI_Id && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id  && b.ASMCL_Id == data.ASMCL_Id 
                                   && b.ASMS_Id == data.ASMS_Id && data.AMST_Ids.Contains(b.AMST_Id))
                                   select new PromotionReportDetailsDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " +
                                       (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                       AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                       AMAY_RollNo = b.AMAY_RollNo,
                                       AMST_RegistrationNo = a.AMST_RegistrationNo == null ? "" : a.AMST_RegistrationNo,
                                       AMST_DOB = a.AMST_DOB
                                   }).Distinct().ToList();

                    var propertyInfo = typeof(PromotionReportDetailsDTO).GetProperty(order);
                    studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                    data.studentlist = studentList.ToArray();

                    if (getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "P" || getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "M")
                    {
                        var getsubjectlist = (from a in _context.Exm_M_PromotionDMO
                                              from b in _context.Exm_M_Promotion_SubjectsDMO
                                              from c in _context.IVRM_School_Master_SubjectsDMO                                              
                                              where (a.EMP_Id == b.EMP_Id && b.ISMS_Id == c.ISMS_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                              && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && b.EMP_Id == getpromotiondetails.FirstOrDefault().EMP_Id)
                                              select new PromotionReportDetailsDTO
                                              {
                                                  ISMS_Id = b.ISMS_Id,
                                                  ISMS_SubjectName = c.ISMS_SubjectName,
                                                  subjorder = b.EMPS_SubjOrder != null ? Convert.ToInt64(b.EMPS_SubjOrder) : c.ISMS_OrderFlag,
                                                  maxmarks = b.EMPS_MaxMarks,
                                                  apptoresult = b.EMPS_AppToResultFlg
                                              }).Distinct().OrderBy(a => a.subjorder).ToArray();

                        data.getsubjectlist = getsubjectlist.ToArray();


                        var getsubjectwisegrouplist = (from a in _context.Exm_M_PromotionDMO
                                                       from b in _context.Exm_M_Promotion_SubjectsDMO
                                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                                       from d in _context.Exm_M_Prom_Subj_GroupDMO
                                                       where (a.EMP_Id == b.EMP_Id && b.ISMS_Id == c.ISMS_Id && b.EMPS_Id == d.EMPS_Id && a.EMP_ActiveFlag == true
                                                       && b.EMPS_ActiveFlag == true && d.EMPSG_ActiveFlag == true
                                                       && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && b.EMP_Id == getpromotiondetails.FirstOrDefault().EMP_Id)
                                                       select new PromotionReportDetailsDTO
                                                       {
                                                           ISMS_Id = b.ISMS_Id,
                                                           subjorder = b.EMPS_SubjOrder != null ? Convert.ToInt64(b.EMPS_SubjOrder) : c.ISMS_OrderFlag,
                                                           groupname = d.EMPSG_GroupName,
                                                           displayname = d.EMPSG_DisplayName,
                                                           grouporder = d.EMPSG_Order != null ? d.EMPSG_Order : 0
                                                       }).Distinct().OrderBy(a => a.subjorder).ThenBy(a => a.grouporder).ToArray();

                        data.getsubjectgrouplist = getsubjectwisegrouplist.ToArray();
                    }

                    if (getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "T")
                    {
                        data.getexamlist = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                            from b in _context.Exm_Yearly_CategoryDMO
                                            from c in _context.exammasterDMO
                                            where (a.EYC_Id == b.EYC_Id && a.EME_Id == c.EME_Id && a.EYCE_ActiveFlg == true && b.EYC_ActiveFlg == true
                                            && c.EME_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_Id == geteycid)
                                            select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();



                        data.getsubjectlist = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                               from b in _context.Exm_Yearly_CategoryDMO
                                               from c in _context.exammasterDMO
                                               from d in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                               from e in _context.IVRM_School_Master_SubjectsDMO
                                               where (a.EYC_Id == b.EYC_Id && a.EME_Id == c.EME_Id && a.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id
                                               && a.EYCE_ActiveFlg == true && b.EYC_ActiveFlg == true && d.EYCES_ActiveFlg == true
                                               && c.EME_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_Id == geteycid)
                                               select new PromotionReportDetailsDTO
                                               {
                                                   ISMS_Id = d.ISMS_Id,
                                                   ISMS_SubjectName = e.ISMS_SubjectName,
                                                   subjorder = Convert.ToInt64(d.EYCES_SubjectOrder)
                                               }).Distinct().OrderBy(a => a.subjorder).ToArray();
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Promotion_Cumulative_Report_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = AmstIds });

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
                            data.reportdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    data.subjectwisetotal = (from a in _context.Exm_Stu_MP_Promo_SubjectwiseDMO
                                             from b in _context.IVRM_School_Master_SubjectsDMO
                                             from c in _context.School_Adm_Y_StudentDMO
                                             from d in _context.Adm_M_Student
                                             where (a.ISMS_Id == b.ISMS_Id && a.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && data.AMST_Ids.Contains(c.AMST_Id))
                                             select new PromotionReportDetailsDTO
                                             {
                                                 ISMS_Id = a.ISMS_Id,
                                                 ISMS_SubjectName = b.ISMS_SubjectName,
                                                 AMST_Id = a.AMST_Id,
                                                 ESTMPPS_MaxMarks = a.ESTMPPS_MaxMarks,
                                                 ESTMPPS_ObtainedMarks = a.ESTMPPS_ObtainedMarks,
                                                 ESTMPPS_ObtainedGrade = a.ESTMPPS_ObtainedGrade,
                                                 ESTMPPS_GradePoints = a.ESTMPPS_GradePoints,
                                                 ESTMPPS_PassFailFlg = a.ESTMPPS_PassFailFlg,
                                                 ESTMPPS_ClassRank = a.ESTMPPS_ClassRank,
                                                 ESTMPPS_SectionRank = a.ESTMPPS_SectionRank
                                             }).Distinct().ToArray();

                    data.studentwisetotal = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && data.AMST_Ids.Contains(a.AMST_Id)).Distinct().ToArray();

                    data.studenwise_remarks = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EPRD_Promotionflag == "PE").ToArray();

                    data.studentwise_individualexamremarks = (from a in _context.Exm_ProgressCard_RemarksDMO
                                                              from b in _context.masterexam
                                                              where (a.EME_ID == b.EME_Id && a.EMER_ActiveFlag == true && a.MI_Id == data.MI_Id
                                                              && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                                              select new PromotionReportDetailsDTO
                                                              {
                                                                  AMST_Id = a.AMST_Id,
                                                                  EME_Id = a.EME_ID,
                                                                  EME_ExamName = b.EME_ExamName,
                                                                  EMER_Remarks = a.EMER_Remarks,
                                                                  EME_ExamOrder = b.EME_ExamOrder,
                                                              }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PromotionReportDetailsDTO getpromotioncumulativereport_format2(PromotionReportDetailsDTO data)
        {
            try
            {
                data.configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.masterinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = getemcaid > 0 ? _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true && a.EMCA_Id == getemcaid).Select(a => a.EYC_Id).FirstOrDefault() : 0;

                var getpromotiondetails = _context.Exm_M_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && a.EYC_Id == geteycid).ToList();

                data.getpromotiondetails = getpromotiondetails.ToArray();

                if (getpromotiondetails != null && getpromotiondetails.Count > 0)
                {
                    string order = "AMST_FirstName";

                    var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                    if (get_configuration != null && get_configuration.Count > 0)
                    {
                        if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                        {
                            order = "AMST_FirstName";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                        {
                            order = "AMST_AdmNo";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                        {
                            order = "AMAY_RollNo";
                        }
                        else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                        {
                            order = "AMST_RegistrationNo";
                        }
                        else
                        {
                            order = "AMST_FirstName";
                        }
                    }


                    List<string> sol = new List<string>();
                    sol.Add("S");
                    sol.Add("L");
                    sol.Add("D");

                    List<int?> activeflag = new List<int?>();
                    activeflag.Add(0);
                    activeflag.Add(1);

                    string AmstIds = string.Join(",", data.AMST_Ids);

                    List<PromotionReportDetailsDTO> studentList = new List<PromotionReportDetailsDTO>();

                    studentList = (from a in _context.Adm_M_Student
                                   from b in _context.School_Adm_Y_StudentDMO
                                   where (a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && activeflag.Contains(a.AMST_ActiveFlag) && b.AMST_Id == a.AMST_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && activeflag.Contains(b.AMAY_ActiveFlag) && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                                   && data.AMST_Ids.Contains(b.AMST_Id))
                                   select new PromotionReportDetailsDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                       (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                       (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                       AMST_AdmNo = a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : a.AMST_AdmNo,
                                       AMAY_RollNo = b.AMAY_RollNo,
                                       AMST_RegistrationNo = a.AMST_RegistrationNo == null || a.AMST_RegistrationNo == "" ? "" : a.AMST_RegistrationNo,
                                       AMST_DOB = a.AMST_DOB
                                   }).Distinct().ToList();

                    var propertyInfo = typeof(PromotionReportDetailsDTO).GetProperty(order);
                    studentList = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                    data.studentlist = studentList.ToArray();

                    data.getsubjectlist = (from a in _context.Exm_M_PromotionDMO
                                           from b in _context.Exm_M_Promotion_SubjectsDMO
                                           from c in _context.Exm_Yearly_CategoryDMO
                                           from e in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.EMP_Id == b.EMP_Id && a.EYC_Id == c.EYC_Id && b.ISMS_Id == e.ISMS_Id && a.EMP_ActiveFlag == true
                                           && b.EMPS_ActiveFlag == true && c.EYC_ActiveFlg == true && a.EYC_Id == geteycid
                                           && b.EMP_Id == getpromotiondetails.FirstOrDefault().EMP_Id)
                                           select new PromotionReportDetailsDTO
                                           {
                                               ISMS_Id = b.ISMS_Id,
                                               ISMS_SubjectName = e.ISMS_SubjectName,
                                               subjorder = Convert.ToInt64(b.EMPS_SubjOrder),
                                               apptoresult = b.EMPS_AppToResultFlg
                                           }).Distinct().OrderBy(a => a.subjorder).ToArray();


                    if (getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "P" || getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "M")
                    {
                        data.getexamlist = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                            && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true
                                            && a.EYC_Id == geteycid && b.EMP_Id == getpromotiondetails.FirstOrDefault().EMP_Id)
                                            select e).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                    }

                    if (getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "T")
                    {
                        data.getexamlist = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                            from b in _context.Exm_Yearly_CategoryDMO
                                            from c in _context.exammasterDMO
                                            where (a.EYC_Id == b.EYC_Id && a.EME_Id == c.EME_Id && a.EYCE_ActiveFlg == true && b.EYC_ActiveFlg == true
                                            && c.EME_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_Id == geteycid)
                                            select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Promotion_Cumulative_Report_Format2";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = AmstIds });

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
                            data.reportdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    if (getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "P" || getpromotiondetails.FirstOrDefault().EMP_MarksPerFlg == "M")
                    {
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Exam_Promotion_Cumulative_ExamWiseTotal_Report_Format2";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = AmstIds });

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
                                data.getexamwiseavgmarkspromotion = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }

                    data.subjectwisetotal = (from a in _context.Exm_Stu_MP_Promo_SubjectwiseDMO
                                             from b in _context.IVRM_School_Master_SubjectsDMO
                                             from c in _context.School_Adm_Y_StudentDMO
                                             from d in _context.Adm_M_Student
                                             where (a.ISMS_Id == b.ISMS_Id && a.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && activeflag.Contains(c.AMAY_ActiveFlag)
                                             && sol.Contains(d.AMST_SOL) && activeflag.Contains(d.AMST_ActiveFlag) && data.AMST_Ids.Contains(c.AMST_Id))
                                             select new PromotionReportDetailsDTO
                                             {
                                                 ISMS_Id = a.ISMS_Id,
                                                 ISMS_SubjectName = b.ISMS_SubjectName,
                                                 AMST_Id = a.AMST_Id,
                                                 ESTMPPS_MaxMarks = a.ESTMPPS_MaxMarks,
                                                 ESTMPPS_ObtainedMarks = a.ESTMPPS_ObtainedMarks,
                                                 ESTMPPS_ObtainedGrade = a.ESTMPPS_ObtainedGrade,
                                                 ESTMPPS_GradePoints = a.ESTMPPS_GradePoints,
                                                 ESTMPPS_PassFailFlg = a.ESTMPPS_PassFailFlg,
                                                 ESTMPPS_ClassRank = a.ESTMPPS_ClassRank,
                                                 ESTMPPS_SectionRank = a.ESTMPPS_SectionRank
                                             }).Distinct().ToArray();

                    data.studentwisetotal = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && data.AMST_Ids.Contains(a.AMST_Id)).Distinct().ToArray();


                    data.studenwise_remarks = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EPRD_Promotionflag == "PE").ToArray();

                    data.studentwise_individualexamremarks = (from a in _context.Exm_ProgressCard_RemarksDMO
                                                              from b in _context.masterexam
                                                              where (a.EME_ID == b.EME_Id && a.EMER_ActiveFlag == true && a.MI_Id == data.MI_Id
                                                              && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                                              select new PromotionReportDetailsDTO
                                                              {
                                                                  AMST_Id = a.AMST_Id,
                                                                  EME_Id = a.EME_ID,
                                                                  EME_ExamName = b.EME_ExamName,
                                                                  EMER_Remarks = a.EMER_Remarks,
                                                                  EME_ExamOrder = b.EME_ExamOrder,
                                                              }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PromotionReportDetailsDTO onpageload(PromotionReportDetailsDTO data)
        {
            try
            {
                data.allclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public PromotionReportDetailsDTO PromotionPerformanceReport(PromotionReportDetailsDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Promotion_Performance_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.allorindi });

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
                        data.reportdata = retObject.ToArray();
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
    }
}