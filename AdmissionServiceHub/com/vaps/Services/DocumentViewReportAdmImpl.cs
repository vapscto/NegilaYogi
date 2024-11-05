using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class DocumentViewReportAdmImpl : Interfaces.DocumentViewReportAdmInterface
    {
        private readonly AdmissionFormContext _AdmissionFormContext;
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<DocumentViewReportAdmImpl> _log;

        public DocumentViewReportAdmImpl(AdmissionFormContext AdmissionFormContext, DomainModelMsSqlServerContext db, ILogger<DocumentViewReportAdmImpl> log)
        {
            _AdmissionFormContext = AdmissionFormContext;
            _db = db;
            _log = log;
        }
        public DocumentViewReportAdmDTO getdetails(int id)
        {
            DocumentViewReportAdmDTO data = new DocumentViewReportAdmDTO();
            try
            {
                data.year = _AdmissionFormContext.year.Where(a => a.MI_Id == id && a.Is_Active == true).OrderByDescending(a=>a.ASMAY_Order).ToArray();
                data.classname = _AdmissionFormContext.School_M_Class.Where(a => a.MI_Id == id && a.ASMCL_ActiveFlag == true).OrderBy(a=>a.ASMCL_Order).ToArray();
                data.section = _AdmissionFormContext.AdmSection.Where(a => a.MI_Id == id && a.ASMC_ActiveFlag == 1).OrderBy(a=>a.ASMC_Order).ToArray();
                data.document = _AdmissionFormContext.MasterDocumentDMO.Where(a => a.MI_Id == id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public DocumentViewReportAdmDTO getstudent(DocumentViewReportAdmDTO data)
        {
            try
            {
                var config = _AdmissionFormContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                if (config.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName) + ":" + (e.AMST_AdmNo == null ? " " : e.AMST_AdmNo)).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }
                else if (config.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((e.AMST_RegistrationNo == null ? " " : e.AMST_RegistrationNo) + ':' + (e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }
                else if (config.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((e.AMST_AdmNo == null ? " " : e.AMST_AdmNo) + ':' + (e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }

                else if (config.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName) + ':' + (e.AMST_RegistrationNo == null ? " " : e.AMST_RegistrationNo)).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }

                else if (config.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((a.AMAY_RollNo.ToString() == null ? " " : a.AMAY_RollNo.ToString()) + ':' + (e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }

                else if (config.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName) + ':' + (a.AMAY_RollNo.ToString() == null ? " " : a.AMAY_RollNo.ToString())).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }

                else
                {
                    data.studentdetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                           from e in _AdmissionFormContext.Adm_M_Student
                                           from c in _AdmissionFormContext.School_M_Class
                                           from d in _AdmissionFormContext.AdmSection
                                           where a.AMST_Id == e.AMST_Id && a.ASMAY_Id == data.asmaY_Id && a.ASMCL_Id == data.asmcL_Id && a.ASMS_Id == data.asmS_Id 
                                           //&& a.AMAY_ActiveFlag == 1 && e.AMST_SOL.Equals("S") && e.AMST_ActiveFlag == 1 
                                           && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id
                                           select new DocumentViewReportAdmDTO
                                           {
                                               studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName) + ":" + (e.AMST_AdmNo == null ? " " : e.AMST_AdmNo)).Trim(),
                                               AMST_Id = e.AMST_Id
                                           }).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public DocumentViewReportAdmDTO getreport(DocumentViewReportAdmDTO data)
        {
            try
            {
                using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Document_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.asmaY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.asmcL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.asmS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@STDORDOC", SqlDbType.VarChar) { Value = data.STDORDOC });
                    cmd.Parameters.Add(new SqlParameter("@SUBORNOT", SqlDbType.VarChar) { Value = data.SUBORNOT });
                    cmd.Parameters.Add(new SqlParameter("@AMSMD_Id", SqlDbType.VarChar) { Value = data.AMSMD_Id });


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
                        data.studentlistreport = retObject.ToArray();
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
