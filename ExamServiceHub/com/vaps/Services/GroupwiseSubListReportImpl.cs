using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using ExamServiceHub.com.vaps.Interfaces;
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
    public class GroupwiseSubListReportImpl : GroupwiseSubListReportInterface
    {
        public ExamContext _examctxt;
        public MasterSubjectContext _subctxt;
        public GroupwiseSubListReportImpl(ExamContext obj, MasterSubjectContext obj1)
        {
            _examctxt = obj;
            _subctxt = obj1;
        }
        public GroupwiseSubListReportDTO getdetails(GroupwiseSubListReportDTO data)
        {

            try
            {
                data.getyear = _examctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.group = (from a in _examctxt.Exm_Master_GroupDMO
                              where (a.MI_Id == data.MI_Id && a.EMG_ActiveFlag == true)
                              select a).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public GroupwiseSubListReportDTO onchangeyear(GroupwiseSubListReportDTO data)
        {
            try
            {
                data.groupname = (from a in _examctxt.AcademicYear
                                  from b in _examctxt.Exm_Yearly_CategoryDMO
                                  from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                  from d in _examctxt.Exm_Master_CategoryDMO
                                  from e in _examctxt.Exm_Master_GroupDMO
                                  where (a.ASMAY_Id == b.ASMAY_Id && b.EMCA_Id == d.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EMG_Id == e.EMG_Id && b.EYC_ActiveFlg == true
                                  && c.EYCG_ActiveFlg == true && d.EMCA_ActiveFlag == true && e.EMG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                                  select e).Distinct().ToArray();

                data.examlist = (from a in _examctxt.Exm_Yearly_Category_ExamsDMO
                                 from b in _examctxt.Exm_Yearly_CategoryDMO
                                 from c in _examctxt.masterexam
                                 where (a.EYC_Id == b.EYC_Id && a.EME_Id == c.EME_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.EYC_ActiveFlg == true 
                                 && a.EYCE_ActiveFlg == true)
                                 select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GroupwiseSubListReportDTO onreport(GroupwiseSubListReportDTO data)
        {
            try
            {
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Subject_Group_Wise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMG_Id", SqlDbType.VarChar) { Value = data.EMG_Id });
                    cmd.Parameters.Add(new SqlParameter("@report_type", SqlDbType.VarChar) { Value = data.report_type });
                    cmd.Parameters.Add(new SqlParameter("@examwiseorwithout", SqlDbType.VarChar) { Value = data.examwiseorwithout });
                    cmd.Parameters.Add(new SqlParameter("@masteryearly", SqlDbType.VarChar) { Value = data.masteryearly });
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.subjectname = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.institution = _examctxt.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
