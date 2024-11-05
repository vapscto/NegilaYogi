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
    public class SAMasterSuperintendentImpl : Interface.SAMasterSuperintendent_Interface
    {
        public SAMasterBuildingContext _context;
        public SAMasterSuperintendentImpl(SAMasterBuildingContext context)
        {
            _context = context;
        }
        // ===============superintendent==================

        public SAMasterSuperintendent load_sup(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a=>a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.superintendentgridlist = (from a in _context.Exam_SA_SuperintendentDMO
                                              from b in _context.exammasterDMO
                                              from c in _context.Exam_SA_University_ExamDMO
                                              from d in _context.AcademicYearDMO
                                              where a.ASMAY_Id == d.ASMAY_Id && a.EME_Id == b.EME_Id && a.ESAUE_Id == c.ESAUE_Id && a.MI_Id == dto.MI_Id
                                              select new SAMasterSuperintendent
                                              {
                                                  ESASINTDNT_Id = a.ESASINTDNT_Id,
                                                  ASMAY_Id = a.ASMAY_Id,
                                                  ASMAY_Year = d.ASMAY_Year,
                                                  EME_Id = a.EME_Id,
                                                  EME_IVRSExamName = b.EME_IVRSExamName,
                                                  ESAUE_Id = a.ESAUE_Id,
                                                  ESAUE_ExamName = c.ESAUE_ExamName,
                                                  ESASINTDNT_ChiefSupName = a.ESASINTDNT_ChiefSupName,
                                                  ESASINTDNT_ChiefSupCollege = a.ESASINTDNT_ChiefSupCollege,
                                                  ESASINTDNT_DeptChiefSupName = a.ESASINTDNT_DeptChiefSupName,
                                                  ESASINTDNT_DeptChiefSupCollege = a.ESASINTDNT_DeptChiefSupCollege,
                                                  ESASINTDNT_FromDate = a.ESASINTDNT_FromDate,
                                                  ESASINTDNT_ToDate = a.ESASINTDNT_ToDate,
                                                  ESASINTDNT_ActiveFlg = a.ESASINTDNT_ActiveFlg,
                                              }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent Save_sup(SAMasterSuperintendent dto)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                if (dto.ESASINTDNT_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_SuperintendentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.EME_Id == dto.EME_Id
                    && a.ESAUE_Id == dto.ESAUE_Id && a.ESASINTDNT_Id != dto.ESASINTDNT_Id
                    && ((a.ESASINTDNT_FromDate <= dto.ESASINTDNT_FromDate && a.ESASINTDNT_ToDate > dto.ESASINTDNT_FromDate)
                    || (a.ESASINTDNT_FromDate < dto.ESASINTDNT_ToDate && a.ESASINTDNT_ToDate >= dto.ESASINTDNT_ToDate))).Distinct().ToList();

                    if (checkduplicate.Count > 0)
                    {
                        dto.message = "Duplicate";
                    }
                    else
                    {
                        var ss = _context.Exam_SA_SuperintendentDMO.Single(a => a.ESASINTDNT_Id == dto.ESASINTDNT_Id);
                        ss.ASMAY_Id = dto.ASMAY_Id;
                        ss.EME_Id = dto.EME_Id;
                        ss.ESAUE_Id = dto.ESAUE_Id;
                        ss.ESASINTDNT_ChiefSupName = dto.ESASINTDNT_ChiefSupName;
                        ss.ESASINTDNT_ChiefSupCollege = dto.ESASINTDNT_ChiefSupCollege;
                        ss.ESASINTDNT_DeptChiefSupName = dto.ESASINTDNT_DeptChiefSupName;
                        ss.ESASINTDNT_DeptChiefSupCollege = dto.ESASINTDNT_DeptChiefSupCollege;
                        ss.ESASINTDNT_FromDate = dto.ESASINTDNT_FromDate;
                        ss.ESASINTDNT_ToDate = dto.ESASINTDNT_ToDate;
                        ss.ESASINTDNT_UpdatedDate = indiantime0;
                        ss.ESASINTDNT_UpdatedBy = dto.UserId;
                        _context.Update(ss);
                        var sv = _context.SaveChanges();
                        if (sv > 0)
                        {
                            dto.message = "Update";
                        }
                        else
                        {
                            dto.message = "Error";
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Exam_SA_SuperintendentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.EME_Id == dto.EME_Id
                   && a.ESAUE_Id == dto.ESAUE_Id
                   && ((a.ESASINTDNT_FromDate <= dto.ESASINTDNT_FromDate && a.ESASINTDNT_ToDate > dto.ESASINTDNT_FromDate)
                   || (a.ESASINTDNT_FromDate < dto.ESASINTDNT_ToDate && a.ESASINTDNT_ToDate >= dto.ESASINTDNT_ToDate))).Distinct().ToList();

                    if (checkduplicate.Count > 0)
                    {
                        dto.message = "Duplicate";
                    }
                    else
                    {
                        Exam_SA_SuperintendentDMO ss = new Exam_SA_SuperintendentDMO();
                        ss.MI_Id = dto.MI_Id;
                        ss.ASMAY_Id = dto.ASMAY_Id;
                        ss.EME_Id = dto.EME_Id;
                        ss.ESAUE_Id = dto.ESAUE_Id;
                        ss.ESASINTDNT_ChiefSupName = dto.ESASINTDNT_ChiefSupName;
                        ss.ESASINTDNT_ChiefSupCollege = dto.ESASINTDNT_ChiefSupCollege;
                        ss.ESASINTDNT_DeptChiefSupName = dto.ESASINTDNT_DeptChiefSupName;
                        ss.ESASINTDNT_DeptChiefSupCollege = dto.ESASINTDNT_DeptChiefSupCollege;
                        ss.ESASINTDNT_FromDate = dto.ESASINTDNT_FromDate;
                        ss.ESASINTDNT_ToDate = dto.ESASINTDNT_ToDate;
                        ss.ESASINTDNT_ActiveFlg = true;
                        ss.ESASINTDNT_CreatedDate = indiantime0;
                        ss.ESASINTDNT_CreatedBy = dto.UserId;
                        _context.Add(ss);
                        var sv = _context.SaveChanges();
                        if (sv > 0)
                        {
                            dto.message = "Add";
                        }
                        else
                        {
                            dto.message = "Error";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent Edit_sup(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.edit_suplist = _context.Exam_SA_SuperintendentDMO.Where(a => a.MI_Id == dto.MI_Id && a.ESASINTDNT_Id == dto.ESASINTDNT_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent ActiveDeactive_sup(SAMasterSuperintendent dto)
        {
            try
            {
                if (dto.ESASINTDNT_ActiveFlg == true)
                {
                    var result = _context.Exam_SA_SuperintendentDMO.Single(a => a.ESASINTDNT_Id == dto.ESASINTDNT_Id);
                    result.ESASINTDNT_ActiveFlg = false;
                    result.ESASINTDNT_UpdatedBy = dto.UserId;
                    result.ESASINTDNT_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sq = _context.SaveChanges();
                    if (sq > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "error";
                    }

                }
                else
                {
                    var result = _context.Exam_SA_SuperintendentDMO.Single(a => a.ESASINTDNT_Id == dto.ESASINTDNT_Id);
                    result.ESASINTDNT_ActiveFlg = true;
                    result.ESASINTDNT_UpdatedBy = dto.UserId;
                    result.ESASINTDNT_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sq = _context.SaveChanges();
                    if (sq > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "error";
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //=========Absent Student======================

        public SAMasterSuperintendent load_AS(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.roomlist = _context.Exam_SA_RoomDMO.Where(a => a.ESAROOM_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.slotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.ESAESLOT_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SA_MasterList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "AS"
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
                        dto.absentstudentlist = retObject.ToArray();
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
            return dto;
        }
        public SAMasterSuperintendent Save_AS(SAMasterSuperintendent dto)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                if (dto.ESAABSTU_Id > 0)
                {
                    var lt = _context.Exam_SA_Absent_StudentDMO.Single(a => a.ESAABSTU_Id == dto.ESAABSTU_Id && a.MI_Id == dto.MI_Id);
                    lt.ASMAY_Id = dto.ASMAY_Id;
                    lt.EME_Id = dto.EME_Id;
                    lt.ESAUE_Id = dto.ESAUE_Id;
                    lt.AMCO_Id = dto.AMCO_Id;
                    lt.AMB_Id = dto.AMB_Id;
                    lt.AMSE_Id = dto.AMSE_Id;
                    lt.AMCST_Id = dto.AMCST_Id;
                    lt.ISMS_Id = dto.ISMS_Id;
                    lt.ESAROOM_Id = dto.ESAROOM_Id;
                    lt.ESAESLOT_Id = dto.ESAESLOT_Id;
                    lt.ESAABSTU_LABTheoryFlg = dto.ESAABSTU_LABTheoryFlg;
                    lt.ESAABSTU_StudentUSN = dto.ESAABSTU_StudentUSN;
                    lt.ESAABSTU_ExamDate = dto.ESAABSTU_ExamDate;
                    lt.ESAABSTU_UpdatedDate = DateTime.Today;
                    lt.ESAABSTU_UpdatedBy = dto.UserId;
                    _context.Update(lt);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    Exam_SA_Absent_StudentDMO lt = new Exam_SA_Absent_StudentDMO();
                    lt.MI_Id = dto.MI_Id;
                    lt.ASMAY_Id = dto.ASMAY_Id;
                    lt.EME_Id = dto.EME_Id;
                    lt.ESAUE_Id = dto.ESAUE_Id;
                    lt.AMCO_Id = dto.AMCO_Id;
                    lt.AMB_Id = dto.AMB_Id;
                    lt.AMSE_Id = dto.AMSE_Id;
                    lt.AMCST_Id = dto.AMCST_Id;
                    lt.ISMS_Id = dto.ISMS_Id;
                    lt.ESAROOM_Id = dto.ESAROOM_Id;
                    lt.ESAESLOT_Id = dto.ESAESLOT_Id;
                    lt.ESAABSTU_ActiveFlg = true;
                    lt.ESAABSTU_LABTheoryFlg = dto.ESAABSTU_LABTheoryFlg;
                    lt.ESAABSTU_StudentUSN = dto.ESAABSTU_StudentUSN;
                    lt.ESAABSTU_ExamDate = dto.ESAABSTU_ExamDate;
                    lt.ESAABSTU_CreatedDate = DateTime.Today;
                    lt.ESAABSTU_CreatedBy = dto.UserId;
                    _context.Add(lt);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent Edit_AS(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();
                dto.courselist = _context.MasterCourseDMO.Where(a => a.AMCO_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMCO_Order).ToArray();
                dto.branchlist = _context.ClgMasterBranchDMO.Where(a => a.AMB_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMB_Order).ToArray();

                dto.semesterlist = _context.CLG_Adm_Master_SemesterDMO.Where(a => a.AMSE_ActiveFlg == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMSE_SEMOrder).ToArray();

                dto.subjectlist = _context.IVRM_Master_SubjectsDMO.Where(a => a.ISMS_ActiveFlag == 1 && a.MI_Id == dto.MI_Id).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                dto.roomlist = _context.Exam_SA_RoomDMO.Where(a => a.ESAROOM_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.slotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.ESAESLOT_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.studentlist = (from a in _context.Adm_Master_College_StudentDMO
                                   where a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id
                                   select new SAMasterSuperintendent
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                       AMCST_FirstName = a.AMCST_FirstName,
                                       AMCST_MiddleName = a.AMCST_MiddleName,
                                       AMCST_LastName = a.AMCST_LastName
                                   }).ToArray();
                dto.edit_aslist = _context.Exam_SA_Absent_StudentDMO.Where(a => a.ESAABSTU_Id == dto.ESAABSTU_Id && a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent DeleteAbsentStudent(SAMasterSuperintendent dto)
        {
            try
            {
                var getresult = _context.Exam_SA_Absent_StudentDMO.Single(a => a.MI_Id == dto.MI_Id && a.ESAABSTU_Id == dto.ESAABSTU_Id);
                _context.Remove(getresult);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    dto.message = "true";
                }
                else
                {
                    dto.message = "false";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //=========Malpractice Student======================
        public SAMasterSuperintendent load_MPS(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.roomlist = _context.Exam_SA_RoomDMO.Where(a => a.ESAROOM_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.slotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.ESAESLOT_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SA_MasterList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "MPS"
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
                        dto.malpracticestudentlist = retObject.ToArray();
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
            return dto;
        }
        public SAMasterSuperintendent Save_MPS(SAMasterSuperintendent dto)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                if (dto.ESAMALSTU_Id > 0)
                {
                    var lt = _context.Exam_SA_Malpractice_StudentDMO.Single(a => a.ESAMALSTU_Id == dto.ESAMALSTU_Id && a.MI_Id == dto.MI_Id);
                    lt.ASMAY_Id = dto.ASMAY_Id;
                    lt.EME_Id = dto.EME_Id;
                    lt.ESAUE_Id = dto.ESAUE_Id;
                    lt.AMCO_Id = dto.AMCO_Id;
                    lt.AMB_Id = dto.AMB_Id;
                    lt.AMSE_Id = dto.AMSE_Id;
                    lt.AMCST_Id = dto.AMCST_Id;
                    lt.ISMS_Id = dto.ISMS_Id;
                    lt.ESAROOM_Id = dto.ESAROOM_Id;
                    lt.ESAESLOT_Id = dto.ESAESLOT_Id;
                    lt.ESAMALSTU_LABTHEORYFlg = dto.ESAMALSTU_LABTHEORYFlg;
                    lt.ESAMALSTU_StudentUSN = dto.ESAMALSTU_StudentUSN;
                    lt.ESAMALSTU_ExamDate = dto.ESAMALSTU_ExamDate;
                    lt.ESAMALSTU_UpdatedDate = DateTime.Today;
                    lt.ESAMALSTU_UpdatedBy = dto.UserId;
                    _context.Update(lt);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    Exam_SA_Malpractice_StudentDMO lt = new Exam_SA_Malpractice_StudentDMO();
                    lt.MI_Id = dto.MI_Id;
                    lt.ASMAY_Id = dto.ASMAY_Id;
                    lt.EME_Id = dto.EME_Id;
                    lt.ESAUE_Id = dto.ESAUE_Id;
                    lt.AMCO_Id = dto.AMCO_Id;
                    lt.AMB_Id = dto.AMB_Id;
                    lt.AMSE_Id = dto.AMSE_Id;
                    lt.AMCST_Id = dto.AMCST_Id;
                    lt.ISMS_Id = dto.ISMS_Id;
                    lt.ESAROOM_Id = dto.ESAROOM_Id;
                    lt.ESAESLOT_Id = dto.ESAESLOT_Id;
                    lt.ESAMALSTU_ActiveFlg = true;
                    lt.ESAMALSTU_LABTHEORYFlg = dto.ESAMALSTU_LABTHEORYFlg;
                    lt.ESAMALSTU_StudentUSN = dto.ESAMALSTU_StudentUSN;
                    lt.ESAMALSTU_ExamDate = dto.ESAMALSTU_ExamDate;
                    lt.ESAMALSTU_CreatedDate = DateTime.Today;
                    lt.ESAMALSTU_CreatedBy = dto.UserId;
                    _context.Add(lt);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent Edit_MPS(SAMasterSuperintendent dto)
        {
            try
            {
                var geteditdetails = _context.Exam_SA_Malpractice_StudentDMO.Where(a => a.ESAMALSTU_Id == dto.ESAMALSTU_Id && a.MI_Id == dto.MI_Id).ToList();

                dto.edit_mpslist = geteditdetails.ToArray();

                dto.ASMAY_Id = geteditdetails.FirstOrDefault().ASMAY_Id;
                dto.AMCO_Id = geteditdetails.FirstOrDefault().AMCO_Id;
                dto.AMB_Id = geteditdetails.FirstOrDefault().AMB_Id;
                dto.AMSE_Id = geteditdetails.FirstOrDefault().AMSE_Id;
                dto.ISMS_Id = geteditdetails.FirstOrDefault().ISMS_Id;
                dto.AMCST_Id = geteditdetails.FirstOrDefault().AMCST_Id;

                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();


                dto.courselist = (from a in _context.MasterCourseDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == dto.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id
                                  && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();


                dto.branchlist = (from a in _context.ClgMasterBranchDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == dto.MI_Id && a.AMB_ActiveFlag && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == dto.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == dto.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select a).Distinct().ToArray();

                dto.semesterlist = (from a in _context.CLG_Adm_Master_SemesterDMO
                                    from b in _context.CLG_Adm_College_AY_CourseDMO
                                    from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == dto.MI_Id && c.ACAYC_Id == b.ACAYC_Id && a.AMSE_ActiveFlg && b.MI_Id == dto.MI_Id
                                    && b.ASMAY_Id == dto.ASMAY_Id && b.ACAYC_ActiveFlag && c.MI_Id == dto.MI_Id && b.AMCO_Id == dto.AMCO_Id
                                    && c.AMB_Id == dto.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == dto.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select a).Distinct().ToArray();

                dto.subjectlist = (from a in _context.Exm_Col_Yearly_SchemeDMO
                                   from b in _context.Exm_Col_Yearly_Scheme_GroupDMO
                                   from c in _context.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                   from d in _context.IVRM_Master_SubjectsDMO
                                   where (a.ECYS_Id == b.ECYS_Id && b.ECYSG_Id == c.ECYSG_Id && c.ISMS_Id == d.ISMS_Id && a.AMCO_Id == dto.AMCO_Id
                                   && a.AMB_Id == dto.AMB_Id && a.AMSE_Id == dto.AMSE_Id && a.ECYS_ActiveFlag == true && b.ECYSG_ActiveFlag == true
                                   && c.ECYSGS_ActiveFlag == true)
                                   select new SAMasterSuperintendent
                                   {
                                       ISMS_Id = d.ISMS_Id,
                                       ISMS_SubjectName = ((d.ISMS_SubjectName == null ? "" : d.ISMS_SubjectName) + (d.ISMS_SubjectCode == null ? "" : ":"
                                       + d.ISMS_SubjectCode)),
                                       ISMS_OrderFlag = d.ISMS_OrderFlag
                                   }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                dto.studentlist = (from a in _context.Adm_Master_College_StudentDMO
                                   from b in _context.Adm_College_Yearly_StudentDMO
                                   where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == dto.AMCO_Id && b.AMB_Id == dto.AMB_Id && a.AMSE_Id == dto.AMSE_Id
                                   && b.AMCST_Id == dto.AMCST_Id)
                                   select new SAMasterSuperintendent
                                   {
                                       AMCST_Id = b.AMCST_Id,
                                       AMCST_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) +
                                       (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                       (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName) +
                                       (a.AMCST_AdmNo == null || a.AMCST_AdmNo == "" ? "" : " : " + a.AMCST_AdmNo)).Trim()
                                   }).Distinct().OrderBy(a => a.AMCST_FirstName).ToArray();


                dto.roomlist = _context.Exam_SA_RoomDMO.Where(a => a.ESAROOM_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.slotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.ESAESLOT_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent DeleteMalPraticeStudent(SAMasterSuperintendent dto)
        {
            try
            {
                var getresult = _context.Exam_SA_Malpractice_StudentDMO.Single(a => a.MI_Id == dto.MI_Id && a.ESAMALSTU_Id == dto.ESAMALSTU_Id);

                _context.Remove(getresult);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    dto.message = "true";
                }
                else
                {
                    dto.message = "false";
                }

            }
            catch (Exception ee)
            {
                dto.message = "false";
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //=========Chief coordinator======================

        public SAMasterSuperintendent load_CC(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SA_MasterList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "CC"
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
                        dto.chiefcoordinatorlist = retObject.ToArray();
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
            return dto;
        }
        public SAMasterSuperintendent Save_CC(SAMasterSuperintendent dto)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                if (dto.ESACHCRD_Id > 0)
                {
                    var lt = _context.Exam_SA_ChiefCoordinatorDMO.Single(a => a.ESACHCRD_Id == dto.ESACHCRD_Id && a.MI_Id == dto.MI_Id);
                    lt.ASMAY_Id = dto.ASMAY_Id;
                    lt.EME_Id = dto.EME_Id;
                    lt.ESAUE_Id = dto.ESAUE_Id;
                    lt.ESACHCRD_ChiefCordName = dto.ESACHCRD_ChiefCordName;
                    lt.ESACHCRD_Add1 = dto.ESACHCRD_Add1;
                    lt.ESACHCRD_Add2 = dto.ESACHCRD_Add2;
                    lt.ESACHCRD_Add3 = dto.ESACHCRD_Add3;
                    lt.ESACHCRD_Add4 = dto.ESACHCRD_Add4;
                    lt.ESACHCRD_Add5 = dto.ESACHCRD_Add5;
                    lt.ESACHCRD_Add6 = dto.ESACHCRD_Add6;
                    lt.ESACHCRD_UpdatedDate = DateTime.Today;
                    lt.ESACHCRD_UpdatedBy = dto.UserId;
                    _context.Update(lt);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    Exam_SA_ChiefCoordinatorDMO lt = new Exam_SA_ChiefCoordinatorDMO();
                    lt.MI_Id = dto.MI_Id;
                    lt.ASMAY_Id = dto.ASMAY_Id;
                    lt.EME_Id = dto.EME_Id;
                    lt.ESAUE_Id = dto.ESAUE_Id;
                    lt.ESACHCRD_ChiefCordName = dto.ESACHCRD_ChiefCordName;
                    lt.ESACHCRD_Add1 = dto.ESACHCRD_Add1;
                    lt.ESACHCRD_Add2 = dto.ESACHCRD_Add2;
                    lt.ESACHCRD_Add3 = dto.ESACHCRD_Add3;
                    lt.ESACHCRD_Add4 = dto.ESACHCRD_Add4;
                    lt.ESACHCRD_Add5 = dto.ESACHCRD_Add5;
                    lt.ESACHCRD_Add6 = dto.ESACHCRD_Add6;
                    lt.ESACHCRD_CreatedBy = dto.UserId;
                    lt.ESACHCRD_CreatedDate = DateTime.Today;
                    lt.ESACHCRD_ActiveFlg = true;
                    _context.Add(lt);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent Edit_CC(SAMasterSuperintendent dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.Is_Active == true && a.MI_Id == dto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.EME_ExamOrder).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.edit_cclist = _context.Exam_SA_ChiefCoordinatorDMO.Where(a => a.ESACHCRD_Id == dto.ESACHCRD_Id && a.MI_Id == dto.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent ActiveDeactive_CC(SAMasterSuperintendent dto)
        {
            try
            {
                if (dto.ESACHCRD_ActiveFlg == true)
                {
                    var result = _context.Exam_SA_ChiefCoordinatorDMO.Single(a => a.ESACHCRD_Id == dto.ESACHCRD_Id && a.MI_Id == dto.MI_Id);
                    result.ESACHCRD_ActiveFlg = false;
                    result.ESACHCRD_UpdatedBy = dto.UserId;
                    result.ESACHCRD_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sq = _context.SaveChanges();
                    if (sq > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "error";
                    }

                }
                else
                {

                    var result = _context.Exam_SA_ChiefCoordinatorDMO.Single(a => a.ESACHCRD_Id == dto.ESACHCRD_Id && a.MI_Id == dto.MI_Id);
                    result.ESACHCRD_ActiveFlg = true;
                    result.ESACHCRD_UpdatedBy = dto.UserId;
                    result.ESACHCRD_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var sq = _context.SaveChanges();
                    if (sq > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "error";
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //************ General Selection ************** //
        public SAMasterSuperintendent GetCourse(SAMasterSuperintendent dto)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                dto.courselist = (from a in _context.MasterCourseDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == dto.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id
                                  && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent GetBranch(SAMasterSuperintendent dto)
        {
            try
            {
                var branchlist = (from a in _context.ClgMasterBranchDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == dto.MI_Id && a.AMB_ActiveFlag && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == dto.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == dto.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select a).Distinct().ToList();

                dto.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent GetSemester(SAMasterSuperintendent dto)
        {
            try
            {
                var semisterlist = (from a in _context.CLG_Adm_Master_SemesterDMO
                                    from b in _context.CLG_Adm_College_AY_CourseDMO
                                    from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == dto.MI_Id && c.ACAYC_Id == b.ACAYC_Id && a.AMSE_ActiveFlg && b.MI_Id == dto.MI_Id
                                    && b.ASMAY_Id == dto.ASMAY_Id && b.ACAYC_ActiveFlag && c.MI_Id == dto.MI_Id && b.AMCO_Id == dto.AMCO_Id
                                    && c.AMB_Id == dto.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == dto.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select a).Distinct().ToList();

                dto.semesterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent GetSubject(SAMasterSuperintendent dto)
        {
            try
            {
                dto.subjectlist = (from a in _context.Exm_Col_Yearly_SchemeDMO
                                   from b in _context.Exm_Col_Yearly_Scheme_GroupDMO
                                   from c in _context.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                   from d in _context.IVRM_Master_SubjectsDMO
                                   where (a.ECYS_Id == b.ECYS_Id && b.ECYSG_Id == c.ECYSG_Id && c.ISMS_Id == d.ISMS_Id && a.AMCO_Id == dto.AMCO_Id
                                   && a.AMB_Id == dto.AMB_Id && a.AMSE_Id == dto.AMSE_Id && a.ECYS_ActiveFlag == true && b.ECYSG_ActiveFlag == true
                                   && c.ECYSGS_ActiveFlag == true )
                                   select new SAMasterSuperintendent
                                   {
                                       ISMS_Id = d.ISMS_Id,
                                       ISMS_SubjectName = ((d.ISMS_SubjectName == null ? "" : d.ISMS_SubjectName) + (d.ISMS_SubjectCode == null ? "" : ":"
                                       + d.ISMS_SubjectCode)),
                                       ISMS_OrderFlag = d.ISMS_OrderFlag
                                   }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public SAMasterSuperintendent GetStudent(SAMasterSuperintendent data)
        {
            try
            {
                List<long> ids = new List<long>();
                if (data.Flag == "Absent")
                {
                    var getstudentmalpraticelist = _context.Exam_SA_Malpractice_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.EME_Id == data.EME_Id && a.ESAUE_Id == data.ESAUE_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                    && a.ISMS_Id == data.ISMS_Id && a.ESAMALSTU_ActiveFlg == true).Distinct().Select(a => a.AMCST_Id).ToList();

                    foreach (var c in getstudentmalpraticelist)
                    {
                        ids.Add(c);
                    }
                }
                else if (data.Flag == "Malpratice")
                {
                    var getstudentabsentlist = _context.Exam_SA_Absent_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.EME_Id == data.EME_Id && a.ESAUE_Id == data.ESAUE_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                    && a.ISMS_Id == data.ISMS_Id && a.ESAABSTU_ActiveFlg == true).Distinct().Select(a => a.AMCST_Id).ToList();

                    foreach (var c in getstudentabsentlist)
                    {
                        ids.Add(c);
                    }
                }

                List<long> studentids = new List<long>();

                var checksubjects = _context.Exm_Col_Studentwise_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_Id == data.AMSE_Id
                && a.ECSTSU_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.AMB_Id == data.AMB_Id && a.ISMS_Id == data.ISMS_Id
                && a.AMCO_Id == data.AMCO_Id).Distinct().Select(a => a.AMCST_Id).ToList();

                foreach (var c in checksubjects)
                {
                    studentids.Add(c);
                }

                var getstudentdetails = (from a in _context.Adm_Master_College_StudentDMO
                                         from b in _context.Adm_College_Yearly_StudentDMO
                                         where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                                         && studentids.Contains(b.AMCST_Id) && !ids.Contains(b.AMCST_Id))
                                         select new SAMasterSuperintendent
                                         {
                                             AMCST_Id = b.AMCST_Id,
                                             AMCST_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) +
                                             (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                             (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName) +
                                             (a.AMCST_AdmNo == null || a.AMCST_AdmNo == "" ? "" : " : " + a.AMCST_AdmNo)).Trim()
                                         }).Distinct().OrderBy(a => a.AMCST_FirstName).ToArray();

                data.studentlist = getstudentdetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}