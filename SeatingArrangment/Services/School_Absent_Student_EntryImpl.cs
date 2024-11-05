using DataAccessMsSqlServerProvider.SeatingArrangment;
using DomainModel.Model.SeatingArrangment;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Services
{
    public class School_Absent_Student_EntryImpl : Interface.School_Absent_Student_EntryInterface
    {
        public SAMasterBuildingContext _context;

        public School_Absent_Student_EntryImpl(SAMasterBuildingContext _cont)
        {
            _context = _cont;
        }

        public School_Absent_Student_EntryDTO GetAbsentStudentLoadData(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetYearList = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetClassList = (from a in _context.Exm_Category_ClassDMO
                                     from b in _context.School_M_Class
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

               // data.GetsavedDetails =(from a in _context.School_Exam_SA_Absent_Student_SchoolDMO)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public School_Absent_Student_EntryDTO OnChangeYear(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetClassList = (from a in _context.Exm_Category_ClassDMO
                                     from b in _context.School_M_Class
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO OnChangeClass(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetSectionList = (from a in _context.Exm_Category_ClassDMO
                                       from b in _context.School_M_Class
                                       from c in _context.School_M_Section
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.ECAC_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO OnChangeSection(School_Absent_Student_EntryDTO data)
        {
            try
            {
                var EMCAId = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var EYCID = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                 && a.EMCA_Id == EMCAId && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.GetSubjectList = (from a in _context.Exm_Yearly_Category_GroupDMO
                                       from b in _context.Exm_Yearly_Category_Group_SubjectsDMO
                                       from c in _context.IVRM_Master_SubjectsDMO
                                       where (a.EYCG_Id == b.EYCG_Id && b.ISMS_Id == c.ISMS_Id && a.EYC_Id == EYCID && a.EYCG_ActiveFlg == true
                                       && b.EYCGS_ActiveFlg == true)
                                       select c).Distinct().ToArray();

                data.GetExamList = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                    from b in _context.exammasterDMO
                                    where (a.EME_Id == b.EME_Id && a.EYC_Id == EYCID && a.EYCE_ActiveFlg == true)
                                    select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.GetRoomList = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAROOM_ActiveFlg == true).ToArray();

                data.GetSlotList = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO SearchData(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetStudentList = (from a in _context.StudentMappingDMO
                                       from b in _context.School_Adm_Y_Student
                                       from c in _context.Adm_M_Student
                                       where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.ASMS_Id == data.ASMS_Id && a.ISMS_Id == data.ISMS_Id && a.ESTSU_ActiveFlg == true && a.MI_Id == data.MI_Id
                                       && c.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id)
                                       select new School_Absent_Student_EntryDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           Studentname = ((c.AMST_FirstName == null ? "" : c.AMST_FirstName) +
                                           (c.AMST_MiddleName == null || c.AMST_MiddleName == "" ? "" : " " + c.AMST_MiddleName) +
                                           (c.AMST_LastName == null || c.AMST_LastName == "" ? "" : " " + c.AMST_LastName)),
                                           AMST_AdmNo = c.AMST_AdmNo
                                       }).Distinct().OrderBy(a => a.Studentname).ToArray();

                data.GetSavedStudentList = _context.School_Exam_SA_Absent_Student_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                 && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && a.ESAROOM_Id == data.ESAROOM_Id
                 && a.ESAESLOT_Id == data.ESAESLOT_Id && a.ESAABSTUSCH_ExamDate == data.ESAABSTUSCH_ExamDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO SaveData(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.tempstudents!=null && data.tempstudents.Length > 0)
                {
                    List<long> ids = new List<long>();

                    foreach(var d in data.tempstudents)
                    {
                        ids.Add(d.ESAABSTUSCH_Id);
                    }

                    Array getremove = _context.School_Exam_SA_Absent_Student_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && !ids.Contains(a.ESAABSTUSCH_Id)
                    && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id 
                    && a.ESAROOM_Id == data.ESAROOM_Id && a.ESAESLOT_Id == data.ESAESLOT_Id && a.ESAABSTUSCH_ExamDate == data.ESAABSTUSCH_ExamDate).ToArray();

                    foreach(School_Exam_SA_Absent_Student_SchoolDMO  ph in getremove)
                    {
                        _context.Remove(ph);
                    }

                    foreach(var dd in data.tempstudents)
                    {
                        if (dd.ESAABSTUSCH_Id > 0)
                        {
                            var result = _context.School_Exam_SA_Absent_Student_SchoolDMO.Single(a => a.ESAABSTUSCH_Id == dd.ESAABSTUSCH_Id);
                            result.ESAABSTUSCH_UpdatedBy = data.UserId;
                            result.ESAABSTUSCH_UpdatedDate = indiantime0;
                            _context.Update(result);
                        }
                        else
                        {
                            School_Exam_SA_Absent_Student_SchoolDMO school_Exam_SA_Absent_Student_SchoolDMO = new School_Exam_SA_Absent_Student_SchoolDMO();
                            school_Exam_SA_Absent_Student_SchoolDMO.MI_Id = data.MI_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ASMAY_Id = data.ASMAY_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.EME_Id = data.EME_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ASMCL_Id = data.ASMCL_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ASMS_Id = data.ASMS_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.AMST_Id = dd.AMST_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ISMS_Id = data.ISMS_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAROOM_Id = data.ESAROOM_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAESLOT_Id = data.ESAESLOT_Id;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAABSTUSCH_ExamDate = data.ESAABSTUSCH_ExamDate;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAABSTUSCH_ActiveFlg = true;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAABSTUSCH_CreatedDate = indiantime0;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAABSTUSCH_UpdatedDate = indiantime0;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAABSTUSCH_CreatedBy = data.UserId;
                            school_Exam_SA_Absent_Student_SchoolDMO.ESAABSTUSCH_UpdatedBy = data.UserId;
                            _context.Add(school_Exam_SA_Absent_Student_SchoolDMO);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Absent Report
        public School_Absent_Student_EntryDTO GetAbsentStudentReportLoadData(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetYearList = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetClassList = (from a in _context.Exm_Category_ClassDMO
                                     from b in _context.School_M_Class
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO OnChangeYearAbsentReport(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetClassList = (from a in _context.Exm_Category_ClassDMO
                                     from b in _context.School_M_Class
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public School_Absent_Student_EntryDTO OnChangeClassAbsentReport(School_Absent_Student_EntryDTO data)
        {
            try
            {
                data.GetSectionList = (from a in _context.Exm_Category_ClassDMO
                                       from b in _context.School_M_Class
                                       from c in _context.School_M_Section
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.ECAC_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO OnChangeSectionAbsentReport(School_Absent_Student_EntryDTO data)
        {
            try
            {
                var EMCAId = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var EYCID = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                 && a.EMCA_Id == EMCAId && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();                 

                data.GetExamList = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                    from b in _context.exammasterDMO
                                    where (a.EME_Id == b.EME_Id && a.EYC_Id == EYCID && a.EYCE_ActiveFlg == true)
                                    select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Absent_Student_EntryDTO GetSchoolAbsentStudentReport(School_Absent_Student_EntryDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Seatingarragment_School_Absent_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id }); 

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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetAbsentReportList = retObject.ToArray();
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