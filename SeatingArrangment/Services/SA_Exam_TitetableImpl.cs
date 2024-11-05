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
    public class SA_Exam_TitetableImpl : Interface.SA_Exam_TitetableInterface
    {
        SAMasterBuildingContext _context;
        public SA_Exam_TitetableImpl(SAMasterBuildingContext cn)
        {
            _context = cn;
        }
        public SA_Exam_TitetableDTO load_TT(SA_Exam_TitetableDTO dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.ASMAY_ActiveFlag == 1 && a.MI_Id == dto.MI_Id).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.courselist = _context.MasterCourseDMO.Where(a => a.AMCO_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMCO_Order).ToArray();

                dto.branchlist = _context.ClgMasterBranchDMO.Where(a => a.AMB_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMB_Order).ToArray();

                dto.semesterlist = _context.CLG_Adm_Master_SemesterDMO.Where(a => a.AMSE_ActiveFlg == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMSE_SEMOrder).ToArray();

                dto.examslotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.ESAESLOT_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();
                dto.subjectschemalist = _context.AdmCollegeSubjectSchemeDMO.Where(a => a.ACST_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.subjectlist = _context.IVRM_Master_SubjectsDMO.Where(a => a.ISMS_ActiveFlag == 1 && a.MI_Id == dto.MI_Id).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SA_MasterList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
             SqlDbType.VarChar)
                    {
                        Value = "TT"
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
                        dto.satimetablelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception eee)
            {
                Console.Write(eee.Message);
            }
            return dto;
        }
        public SA_Exam_TitetableDTO Save_TT(SA_Exam_TitetableDTO dto)
        {
            try
            {
                if (dto.ESAETT_Id > 0)
                {
                    var SE = _context.Exam_SA_ETTDMO.Single(a => a.ESAETT_Id == dto.ESAETT_Id && a.MI_Id == dto.MI_Id);

                    SE.ASMAY_Id = dto.ASMAY_Id;
                    SE.AMCO_Id = dto.AMCO_Id;
                    SE.EME_Id = dto.EME_Id;
                    SE.AMB_Id = dto.AMB_Id;
                    SE.AMSE_Id = dto.AMSE_Id;
                    SE.ESAUE_Id = dto.ESAUE_Id;
                    SE.ESAETT_FromDate = dto.ESAETT_FromDate;
                    SE.ESAETT_ToDate = dto.ESAETT_ToDate;
                    SE.ESAETT_UpdatedDate = DateTime.Today;
                    SE.ESAETT_UpdatedBy = dto.UserId;
                    _context.Update(SE);

                    if (dto.examdetailsarray.Length > 0)
                    {
                        var result = _context.Exam_SA_ETT_DetailsDMO.Where(a => a.ESAETT_Id == dto.ESAETT_Id).ToList();
                        if (result.Count > 0)
                        {
                            foreach (var item in result)
                            {
                                var result1 = _context.Exam_SA_ETT_DetailsDMO.Single(a => a.ESAETT_Id == dto.ESAETT_Id && a.ESAETTD_Id == item.ESAETTD_Id);
                                _context.Remove(result1);

                            }
                        }

                        foreach (var item in dto.examdetailsarray)
                        {
                            Exam_SA_ETT_DetailsDMO ED = new Exam_SA_ETT_DetailsDMO();

                            ED.ESAETT_Id = dto.ESAETT_Id;
                            ED.ESAESLOT_Id = item.ESAESLOT_Id;
                            ED.ACSS_Id = item.ACSS_Id;
                            ED.ISMS_Id = item.ISMS_Id;
                            ED.ESAETT_ExamDate = item.ESAETT_ExamDate;
                            ED.ESAETTD_ActiveFlg = true;
                            ED.ESAETTD_CreatedDate = DateTime.Today;
                            ED.ESAETTD_CreatedBy = dto.UserId;
                            _context.Add(ED);
                        }
                    }

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
                    Exam_SA_ETTDMO SE = new Exam_SA_ETTDMO();


                    SE.MI_Id = dto.MI_Id;
                    SE.ASMAY_Id = dto.ASMAY_Id;
                    SE.AMCO_Id = dto.AMCO_Id;
                    SE.EME_Id = dto.EME_Id;
                    SE.AMB_Id = dto.AMB_Id;
                    SE.AMSE_Id = dto.AMSE_Id;
                    SE.ESAUE_Id = dto.ESAUE_Id;
                    SE.ESAETT_FromDate = dto.ESAETT_FromDate;
                    SE.ESAETT_ToDate = dto.ESAETT_ToDate;
                    SE.ESAETT_ActiveFlg = true;
                    SE.ESAETT_CreatedDate = DateTime.Today;
                    SE.ESAETT_CreatedBy = dto.UserId;
                    _context.Add(SE);
                    dto.ESAETT_Id = SE.ESAETT_Id;
                    if (dto.examdetailsarray.Length > 0)
                    {
                        foreach (var item in dto.examdetailsarray)
                        {
                            Exam_SA_ETT_DetailsDMO ED = new Exam_SA_ETT_DetailsDMO();

                            ED.ESAETT_Id = SE.ESAETT_Id;
                            ED.ESAESLOT_Id = item.ESAESLOT_Id;
                            ED.ACSS_Id = item.ACSS_Id;
                            ED.ISMS_Id = item.ISMS_Id;
                            ED.ESAETT_ExamDate = item.ESAETT_ExamDate;
                            ED.ESAETTD_ActiveFlg = true;
                            ED.ESAETTD_CreatedDate = DateTime.Today;
                            ED.ESAETTD_CreatedBy = dto.UserId;
                            _context.Add(ED);
                        }
                    }
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
            catch (Exception eee)
            {
                Console.Write(eee.Message);
            }
            return dto;
        }
        public SA_Exam_TitetableDTO Edit_TT(SA_Exam_TitetableDTO dto)
        {
            try
            {
                dto.yearlst = _context.AcademicYearDMO.Where(a => a.ASMAY_ActiveFlag == 1 && a.MI_Id == dto.MI_Id).ToArray();

                dto.examlist = _context.exammasterDMO.Where(a => a.EME_ActiveFlag == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.university_examlist = _context.Exam_SA_University_ExamDMO.Where(a => a.ESAUE_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                dto.courselist = _context.MasterCourseDMO.Where(a => a.AMCO_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMCO_Order).ToArray();

                dto.branchlist = _context.ClgMasterBranchDMO.Where(a => a.AMB_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMB_Order).ToArray();

                dto.semesterlist = _context.CLG_Adm_Master_SemesterDMO.Where(a => a.AMSE_ActiveFlg == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.AMSE_SEMOrder).ToArray();

                dto.edit_tt_list = _context.Exam_SA_ETTDMO.Where(a => a.ESAETT_Id == dto.ESAETT_Id && a.MI_Id == dto.MI_Id).ToArray();

                dto.examslotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.ESAESLOT_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();
                dto.subjectschemalist = _context.AdmCollegeSubjectSchemeDMO.Where(a => a.ACST_ActiveFlg == true && a.MI_Id == dto.MI_Id).ToArray();

                dto.subjectlist = _context.IVRM_Master_SubjectsDMO.Where(a => a.ISMS_ActiveFlag == 1 && a.MI_Id == dto.MI_Id).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                dto.edit_tt_details = _context.Exam_SA_ETT_DetailsDMO.Where(a => a.ESAETT_Id == dto.ESAETT_Id).ToArray();
             //   using (var cmd = _context.Database.GetDbConnection().CreateCommand())
             //   {
             //       cmd.CommandText = "SA_ExamTT_Edit";
             //       cmd.CommandType = CommandType.StoredProcedure;

             //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
             //SqlDbType.BigInt)
             //       {
             //           Value = dto.MI_Id
             //       });

             //       cmd.Parameters.Add(new SqlParameter("@ESAETT_Id",
             //SqlDbType.BigInt)
             //       {
             //           Value = dto.ESAETT_Id
             //       });
             //       cmd.Parameters.Add(new SqlParameter("@Type",
             //SqlDbType.VarChar)
             //       {
             //           Value = "TT"
             //       });
             //       if (cmd.Connection.State != ConnectionState.Open)
             //           cmd.Connection.Open();
             //       var retObject = new List<dynamic>();
             //       try
             //       {
             //           using (var dataReader = cmd.ExecuteReader())
             //           {
             //               while (dataReader.Read())
             //               {
             //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
             //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
             //                   {
             //                       dataRow.Add(
             //                           dataReader.GetName(iFiled),
             //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
             //                       );
             //                   }
             //                   retObject.Add((ExpandoObject)dataRow);
             //               }
             //           }
             //           dto.edit_tt_details = retObject.ToArray();
             //       }
             //       catch (Exception ex)
             //       {
             //           Console.WriteLine(ex.Message);
             //       }
             //   }

            }
            catch (Exception eee)
            {
                Console.Write(eee.Message);
            }
            return dto;
        }
        public SA_Exam_TitetableDTO Deactive_TT(SA_Exam_TitetableDTO dto)
        {
            try
            {
                if (dto.ESAETT_ActiveFlg == true)
                {
                    var result = _context.Exam_SA_ETTDMO.Single(a => a.ESAETT_Id == dto.ESAETT_Id && a.MI_Id == dto.MI_Id);
                    result.ESAETT_ActiveFlg = false;
                    result.ESAETT_UpdatedDate = DateTime.Today;
                    result.ESAETT_UpdatedBy = dto.UserId;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    var result = _context.Exam_SA_ETTDMO.Single(a => a.ESAETT_Id == dto.ESAETT_Id && a.MI_Id == dto.MI_Id);
                    result.ESAETT_ActiveFlg = true;
                    result.ESAETT_UpdatedDate = DateTime.Today;
                    result.ESAETT_UpdatedBy = dto.UserId;
                    _context.Update(result);
                    var sv = _context.SaveChanges();
                    if (sv > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }

            catch (Exception eee)
            {
                Console.Write(eee.Message);
            }
            return dto;
        }

        public SA_Exam_TitetableDTO viewTTdetails(SA_Exam_TitetableDTO dto)
        {
            try
            {



                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SA_ExamTTDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ESAETT_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.ESAETT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
             SqlDbType.VarChar)
                    {
                        Value = "TTD"
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
                        dto.view_tt_details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception eee)
            {
                Console.Write(eee.Message);
            }
            return dto;
        }
    }
}
