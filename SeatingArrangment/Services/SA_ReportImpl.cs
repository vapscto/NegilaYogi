using DataAccessMsSqlServerProvider.SeatingArrangment;
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
    public class SA_ReportImpl : Interface.SA_ReportInterface
    {
        SAMasterBuildingContext _context;
        public SA_ReportImpl(SAMasterBuildingContext _cont)
        {
            _context = _cont;
        }
        public SA_ReportDTO GetExamDateloaddata(SA_ReportDTO data)
        {
            try
            {
                data.Getyearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.Getexamlisst = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.Getcourselist = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                      from b in _context.MasterCourseDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                      && b.MI_Id == data.MI_Id)
                                      select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();              

                data.Getuniversityexamlist = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAUE_ActiveFlag == true).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                data.Getslotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).OrderBy(a => a.ESAESLOT_SlotName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SA_ReportDTO OnChangeyear(SA_ReportDTO data)
        {
            try
            {

                data.Getcourselist = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                      from b in _context.MasterCourseDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                      && b.MI_Id == data.MI_Id)
                                      select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SA_ReportDTO GetAbsentStudentReport(SA_ReportDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SeatingArrangement_Exam_Student_Absent_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAUE_Id", SqlDbType.VarChar) { Value = data.ESAUE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAESLOT_Id", SqlDbType.VarChar) { Value = data.ESAESLOT_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAEDATE_ExamDate", SqlDbType.VarChar) { Value = data.ESAEDATE_ExamDate });

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
                        data.GetReportList = retObject.ToArray();
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
        public SA_ReportDTO GetMalpracticeStudentReport(SA_ReportDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SeatingArrangement_Exam_Student_Malpractice_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAUE_Id", SqlDbType.VarChar) { Value = data.ESAUE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAESLOT_Id", SqlDbType.VarChar) { Value = data.ESAESLOT_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAEDATE_ExamDate", SqlDbType.VarChar) { Value = data.ESAEDATE_ExamDate });

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
                        data.GetReportList = retObject.ToArray();
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