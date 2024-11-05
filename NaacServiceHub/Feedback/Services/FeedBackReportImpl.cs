using DataAccessMsSqlServerProvider.FeedBack;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Services
{
    public class FeedBackReportImpl : Interface.FeedBackReportInterface
    {
        public FeedBackContext _context;

        public FeedBackReportImpl(FeedBackContext context)
        {
            _context = context;
        }

        public FeedBackReportDTO getdetails(FeedBackReportDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackReportDTO onchangeradio(FeedBackReportDTO data)
        {
            try
            {
                data.feedbacktype = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_StakeHolderFlag == data.flag && a.FMTY_ActiveFlag == true).OrderBy(a => a.FMTY_FTOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackReportDTO getreport(FeedBackReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "College_Feedback_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.flag });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = data.Type });

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
                        data.getreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.getquestions = (from a in _context.Feedback_Type_QuestionsDMO
                                     from b in _context.FeedBackMasterTypeDMO
                                     from c in _context.Feedback_Master_QuestionsDMO
                                     where (a.FMTY_Id == b.FMTY_Id && a.FMQE_Id == c.FMQE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                     && c.MI_Id == data.MI_Id && b.FMTY_StakeHolderFlag == data.flag && a.FMTY_Id == data.Type && a.FMTQ_ActiveFlag == true
                                     && b.FMTY_ActiveFlag == true && c.FMQE_ActiveFlag == true)
                                     select c).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                data.getoptions = (from a in _context.Feedback_Type_OptionsDMO
                                   from b in _context.FeedBackMasterTypeDMO
                                   from c in _context.Feedback_Master_OptionsDMO
                                   where (a.FMTY_Id == b.FMTY_Id && a.FMOP_Id == c.FMOP_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                   && c.MI_Id == data.MI_Id && b.FMTY_StakeHolderFlag == data.flag && a.FMTY_Id == data.Type && a.FMTO_ActiveFlag == true
                                   && b.FMTY_ActiveFlag == true && c.FMOP_ActiveFlag == true)
                                   select c).Distinct().OrderBy(a => a.FMOP_FOOrder).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackReportDTO onchangeyear(FeedBackReportDTO data)
        {
            try
            {
                data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  where (a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == b.AMCO_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                  && c.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                  select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackReportDTO onchangefeedback(FeedBackReportDTO data)
        {
            try
            {
                if (data.Flagtype == "remarks")
                {
                    data.getquestions = (from a in _context.Feedback_Type_QuestionsDMO
                                         from b in _context.FeedBackMasterTypeDMO
                                         from c in _context.Feedback_Master_QuestionsDMO
                                         where (a.FMTY_Id == b.FMTY_Id && a.FMQE_Id == c.FMQE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                         && c.MI_Id == data.MI_Id && b.FMTY_StakeHolderFlag == data.flag && a.FMTY_Id == data.Type && a.FMTQ_ActiveFlag == true
                                         && b.FMTY_ActiveFlag == true && c.FMQE_ActiveFlag == true && c.FMQE_ManualEntryFlg == true)
                                         select c).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();
                }
                else
                {
                    data.getquestions = (from a in _context.Feedback_Type_QuestionsDMO
                                         from b in _context.FeedBackMasterTypeDMO
                                         from c in _context.Feedback_Master_QuestionsDMO
                                         where (a.FMTY_Id == b.FMTY_Id && a.FMQE_Id == c.FMQE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                         && c.MI_Id == data.MI_Id && b.FMTY_StakeHolderFlag == data.flag && a.FMTY_Id == data.Type && a.FMTQ_ActiveFlag == true
                                         && b.FMTY_ActiveFlag == true && c.FMQE_ActiveFlag == true && c.FMQE_ManualEntryFlg == false)
                                         select c).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackReportDTO getreportnew(FeedBackReportDTO data)
        {
            try
            {

                if (data.Flagtype != "total")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        if (data.FMTY_StakeHolderFlag == "Alumni")
                        {
                            cmd.CommandText = "College_Feedback_Report_CourseWise_ALumni";
                        }
                        else
                        {
                            cmd.CommandText = "College_Feedback_Report_CourseWise_New";
                        }

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.AMCO_Id) });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.FMTY_StakeHolderFlag });
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = data.FMTY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FlagType", SqlDbType.VarChar) { Value = data.Flagtype });
                        cmd.Parameters.Add(new SqlParameter("@FMQE_Id", SqlDbType.VarChar) { Value = data.FMQE_Id });
                        cmd.Parameters.Add(new SqlParameter("@GraphType", SqlDbType.VarChar) { Value = data.graphtype });

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
                            data.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                else if (data.Flagtype == "total")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        if (data.FMTY_StakeHolderFlag == "Alumni")
                        {
                            cmd.CommandText = "College_Feedback_Report_CourseWise_Total_alumni";
                        }
                        else
                        {
                            cmd.CommandText = "College_Feedback_Report_CourseWise_Total";
                        }

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.FMTY_StakeHolderFlag });
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = data.FMTY_Id });
                        cmd.Parameters.Add(new SqlParameter("@FlagType", SqlDbType.VarChar) { Value = data.Flagtype });
                        cmd.Parameters.Add(new SqlParameter("@REPORT", SqlDbType.VarChar) { Value = data.reportnew });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });

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
                            data.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                var getquestionflag = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id
                && a.FMTY_StakeHolderFlag == data.FMTY_StakeHolderFlag && a.FMTY_ActiveFlag == true).ToList();


                if (getquestionflag.FirstOrDefault().FMTY_QuestionwiseOptionFlg == true)
                {
                    data.getoptions = (from a in _context.Feedback_Type_Questions_OptionsDMO
                                       from b in _context.Feedback_Type_QuestionsDMO
                                       from c in _context.FeedBackMasterTypeDMO
                                       from d in _context.Feedback_Master_OptionsDMO
                                       from e in _context.Feedback_Master_QuestionsDMO
                                       where (a.FMTQ_Id == b.FMTQ_Id && b.FMTY_Id == c.FMTY_Id && b.FMQE_Id == e.FMQE_Id && a.FMOP_Id == d.FMOP_Id
                                       && b.MI_Id == data.MI_Id && c.FMTY_StakeHolderFlag == data.FMTY_StakeHolderFlag && c.FMTY_Id == data.FMTY_Id
                                       && b.FMTY_Id == data.FMTY_Id && a.FMTQO_ActiveFlag == true && c.FMTY_ActiveFlag == true && d.FMOP_ActiveFlag == true
                                       && b.FMTQ_ActiveFlag == true && e.FMQE_ActiveFlag == true)
                                       select new FeedBackReportDTO
                                       {
                                           FMQE_Id = b.FMQE_Id,
                                           FMOP_Id = a.FMOP_Id,
                                           FMOP_FeedbackOptions = d.FMOP_FeedbackOptions,
                                           FMOP_FOOrder = d.FMOP_FOOrder,
                                           FMQE_FQOrder = e.FMQE_FQOrder
                                       }).Distinct().OrderBy(a => a.FMQE_FQOrder).ThenBy(a => a.FMOP_FOOrder).ToArray();
                }
                else
                {
                    data.getoptions = (from a in _context.Feedback_Type_OptionsDMO
                                       from b in _context.FeedBackMasterTypeDMO
                                       from c in _context.Feedback_Master_OptionsDMO
                                       where (a.FMTY_Id == b.FMTY_Id && a.FMOP_Id == c.FMOP_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                       && c.MI_Id == data.MI_Id && b.FMTY_StakeHolderFlag == data.FMTY_StakeHolderFlag && a.FMTY_Id == data.FMTY_Id
                                       && a.FMTO_ActiveFlag == true
                                       && b.FMTY_ActiveFlag == true && c.FMOP_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.FMOP_FOOrder).ToArray();
                }


                data.getquestions = (from a in _context.Feedback_Type_QuestionsDMO
                                     from b in _context.FeedBackMasterTypeDMO
                                     from c in _context.Feedback_Master_QuestionsDMO
                                     where (a.FMTY_Id == b.FMTY_Id && a.FMQE_Id == c.FMQE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                     && c.MI_Id == data.MI_Id && b.FMTY_StakeHolderFlag == data.FMTY_StakeHolderFlag && a.FMTY_Id == data.FMTY_Id && a.FMTQ_ActiveFlag == true
                                     && b.FMTY_ActiveFlag == true && c.FMQE_ActiveFlag == true && c.FMQE_ManualEntryFlg == false)
                                     select c).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();



                if (data.Flagtype == "question")
                {
                    data.getcoursenew = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                         from b in _context.MasterCourseDMO
                                         from c in _context.AcademicYear
                                         where (a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == b.AMCO_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                         && c.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                         select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackReportDTO getstudentlist(FeedBackReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    if (data.FMTY_StakeHolderFlag == "Alumni")
                    {
                        cmd.CommandText = "College_Feedback_Report_CourseWise_Total_alumni";
                    }
                    else
                    {
                        cmd.CommandText = "College_Feedback_Report_CourseWise_Total";
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.FMTY_StakeHolderFlag });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = data.FMTY_Id });
                    cmd.Parameters.Add(new SqlParameter("@FlagType", SqlDbType.VarChar) { Value = data.Flagtype });
                    cmd.Parameters.Add(new SqlParameter("@REPORT", SqlDbType.VarChar) { Value = data.reportnew });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });

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
                        data.getstudentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
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
