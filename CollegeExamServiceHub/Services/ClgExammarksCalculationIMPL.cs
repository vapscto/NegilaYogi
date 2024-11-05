using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgExammarksCalculationIMPL:Interfaces.ClgExammarksCalculationInterface
    {
        private readonly ClgExamContext _examcontext;

        ILogger<ClgExammarksCalculationIMPL> _acdimpl;
        public ClgExammarksCalculationIMPL(ClgExamContext cpContext, ILogger<ClgExammarksCalculationIMPL> _acdim)
        {
            _examcontext = cpContext;
            _acdimpl = _acdim;
        }

        public ClgMarksCalculationsDTO Getdetails(ClgMarksCalculationsDTO data)
        {           
            try
            {
                data.courseslist = _examcontext.MasterCourseDMO.Where(c => c.MI_Id == data.MI_Id && c.AMCO_ActiveFlag == true).OrderBy(a=>a.AMCO_Order).ToList().Distinct().ToArray();
                data.branchlist = _examcontext.ClgMasterBranchDMO.Where(c => c.MI_Id == data.MI_Id && c.AMB_ActiveFlag == true).OrderBy(a=>a.AMB_Order).ToList().Distinct().ToArray();
                data.examlist = _examcontext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList().Distinct().ToArray();
                data.yearlist= _examcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList().ToArray();
                data.semesters = _examcontext.CLG_Adm_Master_SemesterDMO.Where(c => c.MI_Id == data.MI_Id && c.AMSE_ActiveFlg == true).OrderBy(a=>a.AMSE_SEMOrder).ToList().Distinct().ToArray();
                data.sectionlist = _examcontext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).OrderBy(a=>a.ACMS_Order).ToList().Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public ClgMarksCalculationsDTO Calculation(ClgMarksCalculationsDTO exm)
        {
            try
            {
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_IndSubjects_SubsExmMarksCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = exm.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt){Value = exm.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.BigInt){Value = exm.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.BigInt) { Value = exm.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.BigInt) { Value = exm.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.BigInt) { Value = exm.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",SqlDbType.Int){ Value = exm.EME_Id});

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var a = cmd.ExecuteNonQuery();

                    exm.returnval = true;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                exm.returnval = false;
            }

            return exm;
        }
    }
}
