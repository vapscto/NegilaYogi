using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
using CommonLibrary;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamImportImpl : Interfaces.ExamImportInterface
    {
        public ExamContext _examcontext;
        ILogger<ExamImportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ExamImportImpl(ExamContext ttcategory, DomainModelMsSqlServerContext db, ILogger<ExamImportImpl> _acdim)
        {
            _examcontext = ttcategory;
            _db = db;
            _acdimpl = _acdim;
        }
        public ExamMarksDTO getdetails(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {

                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examcontext.AcademicYear.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                EM.Acdlist = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;

        }

        public ExamMarksDTO onselectAcdYear(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {


                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true && t.Is_Active == true && t.ASMAY_Id == id.ASMAY_Id).Select(t => t.ASMCL_Id).ToArray();

                var classexmid = (from e in _examcontext.Staff_User_Login
                                  from f in _examcontext.Exm_Login_PrivilegeDMO
                                  from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                  where (e.Id == id.Id && id.MI_Id == id.MI_Id &&
                                    f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id && f.MI_Id == id.MI_Id && classid.Contains(i.ASMCL_Id)
                                    && f.ELP_Id == i.ELP_Id)
                                  select new ExamMarksDTO
                                  {
                                      ASMCL_Id = i.ASMCL_Id
                                  }).Distinct().Select(t => t.ASMCL_Id).ToArray();

                List<AdmissionClass> clist = new List<AdmissionClass>();
                clist = _examcontext.AdmissionClass.Where(t => t.MI_Id == id.MI_Id && t.ASMCL_ActiveFlag == true && classexmid.Contains(t.ASMCL_Id)).ToList();
                EM.ctlist = clist.OrderBy(t=>t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }

        public ExamMarksDTO onselectclass(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true && t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == id.Id && id.MI_Id == id.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id && f.MI_Id == id.MI_Id && secid.Contains(i.ASMS_Id)
                                       && f.ELP_Id == i.ELP_Id)
                                     select new ExamMarksDTO
                                     {
                                         ASMS_Id = i.ASMS_Id
                                     }).Distinct().Select(t => t.ASMS_Id).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == id.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                EM.seclist = seclist.OrderBy(t=>t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }

        public ExamMarksDTO onselectSection(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var emeid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EME_Id).ToArray();

                List<exammasterDMO> examlist = new List<exammasterDMO>();
                examlist = _examcontext.exammasterDMO.Where(t => t.MI_Id == id.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                EM.examlist = examlist.OrderBy(t=>t.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }

        public ExamMarksDTO onselectExam(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).Select(t => t.ISMS_Id).ToArray();

                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == id.Id && id.MI_Id == id.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == id.ASMAY_Id && f.MI_Id == id.MI_Id && i.ASMS_Id == id.ASMS_Id && i.ASMCL_Id == id.ASMCL_Id && f.ELP_Id == i.ELP_Id && subid.Contains(i.ISMS_Id))
                                     select new ExamMarksDTO
                                     {
                                         ISMS_Id = i.ISMS_Id
                                     }).Distinct().Select(t => t.ISMS_Id).ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == id.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && sectionexamid.Contains(c.ISMS_Id)).ToList();
                EM.subjectlist = subjects.OrderBy(t=>t.ISMS_OrderFlag).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }

        public async Task<ExamMarksDTO> onsearch(ExamMarksDTO id)
        {
            ExamMarksDTO EM = new ExamMarksDTO();
            try
            {
                MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                EM.marksdeleteflag = MarksCalcReset.MarksCalculationResetFlag(id.ASMAY_Id, id.ASMCL_Id, id.ASMS_Id, id.MI_Id, id.EME_Id);

                var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == id.ASMAY_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMS_Id == id.ASMS_Id && t.MI_Id == id.MI_Id).Select(t => t.EMCA_Id).ToArray();

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).Select(t => t.ISMS_Id).ToArray();

                var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                EM.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                EM.EMGR_Id = subMorGFlag[0].EMGR_Id;

                if (EM.subMorGFlag == "G")
                {
                    EM.gradname = (from a in _examcontext.Exm_Master_GradeDMO
                                   from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                   where (a.MI_Id == id.MI_Id && a.EMGR_Id == EM.EMGR_Id && b.EMGR_Id == EM.EMGR_Id)
                                   select new ExamMarksDTO
                                   {
                                       grade = b.EMGD_Name
                                   }).Select(b => b.grade).ToArray();
                }


                //EM.studentList = (from e in _examcontext.Adm_M_Student
                //                  from f in _examcontext.School_Adm_Y_StudentDMO
                //                  from i in _examcontext.ExamMarksDMO
                //                  where (e.AMST_Id == f.AMST_Id
                //                  && f.ASMCL_Id == id.ASMCL_Id && f.ASMS_Id == id.ASMS_Id && f.ASMAY_Id == id.ASMAY_Id && e.MI_Id == id.MI_Id
                //                  && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 &&
                //                  i.MI_Id == id.MI_Id && i.ASMAY_Id == id.ASMAY_Id && i.ASMCL_Id == id.ASMCL_Id && i.ASMS_Id == id.ASMS_Id && i.ISMS_Id == id.ISMS_Id && i.EME_Id == id.EME_Id && i.AMST_Id == e.AMST_Id)
                //                  select new StudentAttTempDTO
                //                  {
                //                      amsT_Id = e.AMST_Id,
                //                      studentname = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                //                      amsT_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                //                      amaY_RollNo = f.AMAY_RollNo

                //                  }).Distinct().ToArray();

                //if (EM.studentList.Length > 0)
                //{
                //    EM.studentList = (from e in _examcontext.Adm_M_Student
                //                      from i in _examcontext.ExamMarksDMO
                //                      from f in _examcontext.School_Adm_Y_StudentDMO
                //                      from g in _examcontext.IVRM_School_Master_SubjectsDMO
                //                      from h in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO

                //                      where (e.AMST_Id == f.AMST_Id
                //                      && f.ASMCL_Id == id.ASMCL_Id && f.ASMS_Id == id.ASMS_Id && f.ASMAY_Id == id.ASMAY_Id && e.MI_Id == id.MI_Id
                //                      && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 &&
                //                      g.ISMS_Id == h.ISMS_Id
                //                      && g.MI_Id == id.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && subid.Contains(g.ISMS_Id) && g.ISMS_Id == id.ISMS_Id && eyceid.Contains(h.EYCE_Id)) &&
                //                      i.MI_Id == id.MI_Id && i.ASMAY_Id == id.ASMAY_Id && i.ASMCL_Id == id.ASMCL_Id && i.ASMS_Id == id.ASMS_Id && i.ISMS_Id == id.ISMS_Id && i.EME_Id == id.EME_Id && i.AMST_Id == e.AMST_Id
                //                      select new ExamMarksDTO
                //                      {
                //                          AMST_Id = e.AMST_Id,
                //                          studentname = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                //                          amsT_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                //                          amaY_RollNo = f.AMAY_RollNo,
                //                          SubjectName = g.ISMS_SubjectName,
                //                          TotalMarks = h.EYCES_MaxMarks,
                //                          MinMarks = h.EYCES_MinMarks,
                //                          obtainmarks = (i.ESTM_Flg == "" ? i.ESTM_Marks.ToString() : i.ESTM_Flg)


                //                      }).Distinct().ToArray();
                //}
                //else
                //{
                //    EM.studentList = (from e in _examcontext.Adm_M_Student
                //                      from f in _examcontext.School_Adm_Y_StudentDMO
                //                      from g in _examcontext.IVRM_School_Master_SubjectsDMO
                //                      from h in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO

                //                      where (e.AMST_Id == f.AMST_Id
                //                      && f.ASMCL_Id == id.ASMCL_Id && f.ASMS_Id == id.ASMS_Id && f.ASMAY_Id == id.ASMAY_Id && e.MI_Id == id.MI_Id
                //                      && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 &&
                //                      g.ISMS_Id == h.ISMS_Id
                //                      && g.MI_Id == id.MI_Id && g.ISMS_ActiveFlag == 1 && g.ISMS_ExamFlag == 1 && subid.Contains(g.ISMS_Id) && g.ISMS_Id == id.ISMS_Id && eyceid.Contains(h.EYCE_Id))
                //                      select new ExamMarksDTO
                //                      {
                //                          AMST_Id = e.AMST_Id,
                //                          studentname = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                //                          amsT_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                //                          amaY_RollNo = f.AMAY_RollNo,
                //                          SubjectName = g.ISMS_SubjectName,
                //                          TotalMarks = h.EYCES_MaxMarks,
                //                          MinMarks = h.EYCES_MinMarks,
                //                          obtainmarks = "0"
                //                      }).Distinct().ToArray();
                //}

                List<ExamMarksDTO> result = new List<ExamMarksDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_get_Marks_Entry";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(id.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(id.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(id.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(id.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(id.EME_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(id.ISMS_Id)
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
                                result.Add(new ExamMarksDTO
                                {

                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    studentname = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    amsT_AdmNo = dataReader["AMST_AdmNo"].ToString() == null ? "" : dataReader["AMST_AdmNo"].ToString(),
                                    amaY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString()),
                                    SubjectName = dataReader["ISMS_SubjectName"].ToString(),
                                    TotalMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString()),
                                    MarksEnterFor = Convert.ToDecimal(dataReader["EYCES_MarksEntryMax"].ToString()),
                                    MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString()),
                                    obtainmarks = (dataReader["ESTM_Flg"].ToString() == "" ? dataReader["ESTM_Marks"].ToString() : dataReader["ESTM_Flg"].ToString())

                                });

                                EM.studentList = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }

        public ExamImportStudentDTO ImportMarks(ExamImportStudentDTO stu)
        {

           
            try
            {


                try
                {


                    MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                    stu.marksdeleteflag = MarksCalcReset.MarksCalculationReset(stu.ASMAY_Id, stu.ASMCL_Id, stu.ASMS_Id, stu.MI_Id, stu.EME_Id);

                    var catid = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == stu.ASMAY_Id && t.ASMCL_Id == stu.ASMCL_Id && t.ASMS_Id == stu.ASMS_Id && t.MI_Id == stu.MI_Id).Select(t => t.EMCA_Id).ToArray();

                    var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                    var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                    var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                    var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                    stu.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                    stu.EMGR_Id = subMorGFlag[0].EMGR_Id;

                    string msg = "";

                    for (int i = 0; i < stu.newlstget.Count; i++)
                    {
                        if (stu.newlstget[i].AMST_Id != 0)
                        {
                            var checkduplicates = _examcontext.ExamMarksDMO.Where(d => d.ASMAY_Id == stu.ASMAY_Id && d.ASMCL_Id == stu.ASMCL_Id && d.ASMS_Id == stu.ASMS_Id && d.AMST_Id == stu.newlstget[i].AMST_Id && d.EME_Id == stu.EME_Id && d.ISMS_Id == stu.ISMS_Id && d.ESTM_ActiveFlg == true && d.MI_Id == stu.MI_Id).ToList();

                            if (checkduplicates.Count > 0)
                            {

                                var result = _examcontext.ExamMarksDMO.Single(d => d.ASMAY_Id == stu.ASMAY_Id && d.ASMCL_Id == stu.ASMCL_Id && d.ASMS_Id == stu.ASMS_Id && d.AMST_Id == stu.newlstget[i].AMST_Id && d.EME_Id == stu.EME_Id && d.ISMS_Id == stu.ISMS_Id && d.ESTM_ActiveFlg == true && d.MI_Id == stu.MI_Id);


                                result.MI_Id = stu.MI_Id;
                                result.ASMCL_Id = stu.ASMCL_Id;
                                result.ASMAY_Id = stu.ASMAY_Id;
                                result.ASMS_Id = stu.ASMS_Id;
                                result.EME_Id = stu.EME_Id;
                                result.ISMS_Id = stu.ISMS_Id;
                                result.AMST_Id = stu.newlstget[i].AMST_Id;

                                if (stu.newlstget[i].Obtain_Marks == "AB" || stu.newlstget[i].Obtain_Marks == "ab" || stu.newlstget[i].Obtain_Marks == "L" || stu.newlstget[i].Obtain_Marks == "l" || stu.newlstget[i].Obtain_Marks == "M" || stu.newlstget[i].Obtain_Marks == "m")
                                {
                                    result.ESTM_Flg = stu.newlstget[i].Obtain_Marks;
                                    result.ESTM_Marks = Convert.ToDecimal("0");
                                    result.ESTM_Grade = "null";
                                    if (stu.subMorGFlag == "M")
                                    {
                                        result.ESTM_MarksGradeFlg = "M";
                                    }
                                    else if (stu.subMorGFlag == "G")
                                    {
                                        result.ESTM_MarksGradeFlg = "G";
                                    }
                                }
                                else
                                {
                                    if (stu.subMorGFlag == "M")
                                    {
                                        result.ESTM_MarksGradeFlg = "M";
                                        result.ESTM_Marks = Convert.ToDecimal(stu.newlstget[i].Obtain_Marks);
                                        result.ESTM_Grade = "null";
                                    }
                                    else if (stu.subMorGFlag == "G")
                                    {
                                        result.ESTM_MarksGradeFlg = "G";
                                        result.ESTM_Marks = Convert.ToDecimal("0");
                                        result.ESTM_Grade = stu.newlstget[i].Obtain_Marks; // stu.ESTM_Grade;
                                    }
                                }

                                result.UpdatedDate = DateTime.Now;
                                result.Id = stu.Id;
                                result.IP4 = stu.IP4;




                                _examcontext.Update(result);
                                int flag = _examcontext.SaveChanges();
                                if (flag == 1)
                                {
                                    stu.messagesaveupdate = "true";
                                }
                                else
                                {
                                    stu.messagesaveupdate = "true";
                                }
                            }
                            else
                            {
                                ExamMarksDMO MM = Mapper.Map<ExamMarksDMO>(stu);
                                MM.MI_Id = stu.MI_Id;
                                MM.ASMCL_Id = stu.ASMCL_Id;
                                MM.ASMAY_Id = stu.ASMAY_Id;
                                MM.ASMS_Id = stu.ASMS_Id;
                                MM.EME_Id = stu.EME_Id;
                                MM.ISMS_Id = stu.ISMS_Id;
                                MM.AMST_Id = stu.newlstget[i].AMST_Id;

                                if (stu.newlstget[i].Obtain_Marks == "AB" || stu.newlstget[i].Obtain_Marks == "ab" || stu.newlstget[i].Obtain_Marks == "L" || stu.newlstget[i].Obtain_Marks == "l" || stu.newlstget[i].Obtain_Marks == "M" || stu.newlstget[i].Obtain_Marks == "m")
                                {
                                    MM.ESTM_Flg = stu.newlstget[i].Obtain_Marks;
                                    MM.ESTM_Marks = Convert.ToDecimal("0");
                                    MM.ESTM_Grade = "null";
                                    if (stu.subMorGFlag == "M")
                                    {
                                        MM.ESTM_MarksGradeFlg = "M";
                                    }
                                    else if (stu.subMorGFlag == "G")
                                    {
                                        MM.ESTM_MarksGradeFlg = "G";
                                    }
                                }
                                else
                                {
                                    if (stu.subMorGFlag == "M")
                                    {
                                        MM.ESTM_Marks = Convert.ToDecimal(stu.newlstget[i].Obtain_Marks);
                                        MM.ESTM_MarksGradeFlg = "M";
                                        MM.ESTM_Grade = "null";
                                    }
                                    else if (stu.subMorGFlag == "G")
                                    {
                                        MM.ESTM_Marks = Convert.ToDecimal("0");
                                        MM.ESTM_MarksGradeFlg = "G";
                                        MM.ESTM_Grade = stu.newlstget[i].Obtain_Marks; // id.ESTM_Grade;
                                    }
                                }




                                MM.CreatedDate = DateTime.Now;
                                MM.UpdatedDate = DateTime.Now;
                                MM.Id = stu.Id;
                                MM.IP4 = stu.IP4;
                                //MM.ESTM_Grade = id.ESTM_Grade;
                                MM.ESTM_ActiveFlg = true;

                                _examcontext.Add(MM);
                                int flag = _examcontext.SaveChanges();
                                if (flag == 1)
                                {
                                    stu.messagesaveupdate = "true";
                                }
                                else
                                {
                                    stu.messagesaveupdate = "true";
                                }
                            }
                        }

                    }



                }
                catch (Exception ex)
                {
                    // failcount = failcount + 1;
                    // string name = failnames;
                    //finalnames += name + ",";
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "' And Failed List Names :' " + finalnames + "'";

            return stu;
        }


    }
}
