using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgExamMonthEndReportImpl:Interfaces.ClgExamMonthEndReportInterface
    {
        public ClgExamContext _examctxt;
        
        public ClgExamMonthEndReportImpl(ClgExamContext obj)
        {
            _examctxt = obj;
           
        }


        public ClgExamMonthEndReportDTO getdetails(ClgExamMonthEndReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.examlist = _examctxt.col_exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToArray();
                data.monthlist = _examctxt.Month.Where(a => a.Is_Active == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public ClgExamMonthEndReportDTO onreport(ClgExamMonthEndReportDTO data)
        {
            try
            {

                List<ClgExamMonthEndReportDTO> result = new List<ClgExamMonthEndReportDTO>();

                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Exam_Month_End_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@eme_id", SqlDbType.Int) { Value = data.EME_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new ClgExamMonthEndReportDTO
                                {
                                    ASMCL_ClassName = dataReader["Course"].ToString(),
                                    Pass = Convert.ToInt32(dataReader["Pass"].ToString()),
                                    Fail = Convert.ToInt32(dataReader["Fail"].ToString())
                                });
                                data.datareport = result.ToArray();
                            }
                        }
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
            return data;
        }


    }
}
