using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamMonthEndReportImpl : ExamMonthEndReportInterface
    {
        public ExamContext _examctxt;
        public LMContext _lmctxt;
        public ExamMonthEndReportImpl(ExamContext obj, LMContext obj1)
        {
            _examctxt = obj;
            _lmctxt = obj1;
        }


        public ExamMonthEndReportDTO getdetails(ExamMonthEndReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.examlist = _examctxt.masterexam.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(t=>t.EME_ExamOrder).ToArray();

                //var examlist = (from a in _examctxt.masterexam
                //                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                //                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && b.EYCE_ActiveFlg == true)
                //                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                //data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.monthlist = _lmctxt.IVRM_Month_DMO.Where(a => a.Is_Active == true).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public ExamMonthEndReportDTO onreport(ExamMonthEndReportDTO data)
        {
            try
            {

                List<ExamMonthEndReportDTO> result = new List<ExamMonthEndReportDTO>();
                 
                    using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Month_End_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@eme_id", SqlDbType.Int) { Value = data.EME_Id });
                        //cmd.Parameters.Add(new SqlParameter("@gender", SqlDbType.VarChar) { Value = data.genderflage });
                    if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new ExamMonthEndReportDTO
                                    {
                                        ASMCL_ClassName = dataReader["class"].ToString(), 
                                        Pass = Convert.ToInt32(dataReader["Pass"].ToString()),
                                        Fail = Convert.ToInt32(dataReader["Fail"].ToString()),
                                        //male = Convert.ToInt32(dataReader["male"].ToString()),
                                        //female = Convert.ToInt32(dataReader["female"].ToString())
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
