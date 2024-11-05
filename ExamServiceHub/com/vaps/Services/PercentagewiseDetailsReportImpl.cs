using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
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
    public class PercentagewiseDetailsReportImpl : PercentagewiseDetailsReportInterface
    {
        public ExamContext _examctxt;
        public PercentagewiseDetailsReportImpl(ExamContext obj)
        {
            _examctxt = obj;
        }
        public PercentagewiseDetailsReportDTO getdetails(PercentagewiseDetailsReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public PercentagewiseDetailsReportDTO onselectAcdYear(PercentagewiseDetailsReportDTO data)
        {
            try
            {
                data.catlist = (from a in _examctxt.Exm_Master_CategoryDMO
                                from b in _examctxt.Exm_Yearly_CategoryDMO
                                from c in _examctxt.AcademicYear
                                where (a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == c.ASMAY_Id && a.EMCA_ActiveFlag == true && b.EYC_ActiveFlg == true && a.MI_Id == data.MI_Id
                                && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PercentagewiseDetailsReportDTO onselectCategory(PercentagewiseDetailsReportDTO data)
        {
            try
            {
                var ctlistlist = (from c in _examctxt.AdmissionClass
                                  from d in _examctxt.Exm_Category_ClassDMO
                                  from e in _examctxt.AcademicYear
                                  where (d.ASMCL_Id == c.ASMCL_Id && d.ASMAY_Id == e.ASMAY_Id && c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id
                                  && d.ASMAY_Id == data.ASMAY_Id && d.ECAC_ActiveFlag == true && d.EMCA_Id == data.EMCA_Id)
                                  select c).Distinct().OrderBy(t => t.ASMCL_Order).ToList();

                data.ctlist = ctlistlist.ToArray();

                data.examlist = (from a in _examctxt.Exm_Master_CategoryDMO
                                 from b in _examctxt.Exm_Yearly_CategoryDMO
                                 from c in _examctxt.Exm_Yearly_Category_ExamsDMO
                                 from d in _examctxt.AcademicYear
                                 from e in _examctxt.masterexam
                                 where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && b.ASMAY_Id == d.ASMAY_Id && c.EME_Id == e.EME_Id
                                 && a.EMCA_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCE_ActiveFlg == true && d.Is_Active == true
                                 && e.EME_ActiveFlag == true && b.EMCA_Id == data.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                 select e).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PercentagewiseDetailsReportDTO onselectclass(PercentagewiseDetailsReportDTO data)
        {
            try
            {
                data.seclist = (from c in _examctxt.School_M_Section
                                from d in _examctxt.Exm_Category_ClassDMO
                                from e in _examctxt.AdmissionClass
                                from f in _examctxt.AcademicYear
                                where (c.ASMS_Id == d.ASMS_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMAY_Id == f.ASMAY_Id && c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1
                                && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id && d.EMCA_Id == data.EMCA_Id)
                                select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PercentagewiseDetailsReportDTO onselectSection(PercentagewiseDetailsReportDTO data)
        {
            try
            {
                List<Exm_Category_ClassDMO> emcaid = new List<Exm_Category_ClassDMO>();
                List<long> emca_id = new List<long>();

                if (data.ASMS_Id > 0)
                {
                    emcaid = (from a in _examctxt.Exm_Category_ClassDMO
                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true)
                              select a).Distinct().ToList();
                }
                else
                {
                    emcaid = (from a in _examctxt.Exm_Category_ClassDMO
                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true)
                              select a).Distinct().ToList();
                }

                foreach (var c in emcaid)
                {
                    emca_id.Add(c.EMCA_Id);
                }

                List<long> eycid = new List<long>();
                var EYC_Id = _examctxt.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && emca_id.Contains(t.EMCA_Id)
                && t.EYC_ActiveFlg == true).ToList();

                foreach (var d in EYC_Id)
                {
                    eycid.Add(d.EYC_Id);
                }

                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && eycid.Contains(b.EYC_Id) && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();

                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PercentagewiseDetailsReportDTO onreport(PercentagewiseDetailsReportDTO data)
        {
            try
            {
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Percentage_Wise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.Int) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.Int) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMCA_Id", SqlDbType.Int) { Value = data.EMCA_Id });
                    cmd.Parameters.Add(new SqlParameter("@REPORT_TYPE", SqlDbType.VarChar) { Value = data.report_type });
                    cmd.Parameters.Add(new SqlParameter("@PERCENTAGE_FLAG", SqlDbType.Int) { Value = data.percent });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.Int) { Value = data.EME_Id });

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
                        data.datareport = retObject.ToArray();
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
