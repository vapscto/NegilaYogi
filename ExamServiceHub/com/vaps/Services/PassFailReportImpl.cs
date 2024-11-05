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
    public class PassFailReportImpl : PassFailReportInterface
    {
        public ExamContext _examctxt;
        public PassFailReportImpl(ExamContext obj)
        {
            _examctxt = obj;
        }
        public PassFailReportDTO getdetails(PassFailReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.catlist = (
                                from d in _examctxt.Exm_Master_CategoryDMO
                                where (d.MI_Id == data.MI_Id && d.EMCA_ActiveFlag == true)
                                select d).Distinct().ToArray();

                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.studentlist = (from a in _examctxt.Adm_M_Student
                                    from b in _examctxt.School_Adm_Y_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                    select new PassFailReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim()
                                    }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public PassFailReportDTO onselectCategory(PassFailReportDTO data)
        {
            try
            {
                data.ctlist = (from a in _examctxt.Exm_Category_ClassDMO
                               from b in _examctxt.Exm_Master_CategoryDMO
                               from c in _examctxt.AdmissionClass
                               where (a.EMCA_Id == b.EMCA_Id && c.ASMCL_Id == a.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                               && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true && c.ASMCL_ActiveFlag == true && a.EMCA_Id == data.EMCA_Id)
                               select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

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
        public PassFailReportDTO onselectclass(PassFailReportDTO data)
        {
            try
            {
                data.seclist = (from a in _examctxt.Exm_Category_ClassDMO
                                from b in _examctxt.Exm_Master_CategoryDMO
                                from c in _examctxt.AdmissionClass
                                from d in _examctxt.School_M_Section
                                where (a.EMCA_Id == b.EMCA_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.MI_Id == data.MI_Id
                                && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMC_ActiveFlag == 1
                                && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true && a.EMCA_Id == data.EMCA_Id)
                                select d).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PassFailReportDTO onselectSection(PassFailReportDTO data)
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

                if (data.ASMS_Id > 0)
                {
                    data.studentlist = (from a in _examctxt.Adm_M_Student
                                        from b in _examctxt.School_Adm_Y_StudentDMO
                                        where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id
                                        && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                        select new PassFailReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                            (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " :" + a.AMST_AdmNo)).Trim()
                                        }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
                }
                else
                {
                    data.studentlist = (from a in _examctxt.Adm_M_Student
                                        from b in _examctxt.School_Adm_Y_StudentDMO
                                        where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id
                                        && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id)
                                        select new PassFailReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                            (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " :" + a.AMST_AdmNo)).Trim()
                                        }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PassFailReportDTO onreport(PassFailReportDTO data)
        {
            try
            {
                List<PassFailReportDTO> result = new List<PassFailReportDTO>();

                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Pass_Fail_OverAll_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.Int) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.Int) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.report_type });
                    cmd.Parameters.Add(new SqlParameter("@eme_id", SqlDbType.Int) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.Int) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@emca_id", SqlDbType.Int) { Value = data.EMCA_Id });
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

                data.getinstitution = _examctxt.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
