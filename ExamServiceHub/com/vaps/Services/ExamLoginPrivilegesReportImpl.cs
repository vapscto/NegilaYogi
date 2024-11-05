using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
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
    public class ExamLoginPrivilegesReportImpl : ExamLoginPrivilegesReportInterface
    {

        private readonly ExamContext _examctxt;
        private readonly subjectmasterContext _subctxt;
        public ExamLoginPrivilegesReportImpl(ExamContext obj, subjectmasterContext obj1)
        {
            _examctxt = obj;
            _subctxt = obj1;
        }
        public ExamLoginPrivilegesReportDTO getdetails(ExamLoginPrivilegesReportDTO data)
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
        public ExamLoginPrivilegesReportDTO onselectAcdYear(ExamLoginPrivilegesReportDTO data)
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamLoginPrivilegesReportDTO onchangecategory(ExamLoginPrivilegesReportDTO data)
        {
            try
            {
                List<long> classid = new List<long>();

                var ctlistlist = (from c in _examctxt.AdmissionClass
                                  from d in _examctxt.Exm_Category_ClassDMO
                                  from e in _examctxt.AcademicYear
                                  where (d.ASMCL_Id == c.ASMCL_Id && d.ASMAY_Id == e.ASMAY_Id && c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id
                                  && d.ASMAY_Id == data.ASMAY_Id && d.ECAC_ActiveFlag == true && d.EMCA_Id == data.EMCA_Id)
                                  select c).Distinct().OrderBy(t => t.ASMCL_Order).ToList();

                data.ctlist = ctlistlist.ToArray();

                foreach (var c in ctlistlist)
                {
                    classid.Add(c.ASMCL_Id);
                }

                if (data.report_type == "all")
                {
                    data.stafflist = (from a in _examctxt.Exm_Login_PrivilegeDMO
                                      from b in _examctxt.Staff_User_Login
                                      from c in _examctxt.HR_Master_Employee_DMO
                                      from d in _examctxt.Exm_Login_Privilege_SubjectsDMO
                                      where (a.Login_Id == b.IVRMSTAUL_Id && a.ELP_Id == d.ELP_Id && b.Emp_Code == c.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.ELP_ActiveFlg == true && c.HRME_ActiveFlag == true && d.ELPs_ActiveFlg == true && classid.Contains(d.ASMCL_Id))
                                      select new ExamLoginPrivilegesReportDTO
                                      {
                                          HRME_Id = c.HRME_Id,
                                          HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : c.HRME_EmployeeFirstName) +
                                          (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName) +
                                          (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName) +
                                          (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : ":" + c.HRME_EmployeeCode)).Trim()
                                      }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamLoginPrivilegesReportDTO onselectclass(ExamLoginPrivilegesReportDTO data)
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
        public ExamLoginPrivilegesReportDTO onchangesection(ExamLoginPrivilegesReportDTO data)
        {
            try
            {
                data.stafflist = (from a in _examctxt.Exm_Login_PrivilegeDMO
                                  from b in _examctxt.Staff_User_Login
                                  from c in _examctxt.HR_Master_Employee_DMO
                                  from d in _examctxt.Exm_Login_Privilege_SubjectsDMO
                                  where (a.Login_Id == b.IVRMSTAUL_Id && a.ELP_Id == d.ELP_Id && b.Emp_Code == c.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                  && a.ELP_ActiveFlg == true && c.HRME_ActiveFlag == true && d.ELPs_ActiveFlg == true && d.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id)
                                  select new ExamLoginPrivilegesReportDTO
                                  {
                                      HRME_Id = c.HRME_Id,
                                      HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : c.HRME_EmployeeFirstName) +
                                      (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName) +
                                      (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName) +
                                      (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : ":" + c.HRME_EmployeeCode)).Trim()
                                  }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamLoginPrivilegesReportDTO onreport(ExamLoginPrivilegesReportDTO data)
        {
            try
            {
                List<ExamLoginPrivilegesReportDTO> result = new List<ExamLoginPrivilegesReportDTO>(); 
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Login_Privilege_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.VarChar) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@report_type", SqlDbType.VarChar) { Value = data.report_type });
                    cmd.Parameters.Add(new SqlParameter("@check_type", SqlDbType.VarChar) { Value = data.check_type });
                    cmd.Parameters.Add(new SqlParameter("@emca_id", SqlDbType.VarChar) { Value = data.EMCA_Id });

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
