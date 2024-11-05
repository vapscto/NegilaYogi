using AdmissionServiceHub.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class JSHSAdmissionCertificateImpl : Interfaces.JSHSAdmissionCertificateInterface
    {
        private static ConcurrentDictionary<string, JSHSAdmissionCertificate_DTO> _login =
           new ConcurrentDictionary<string, JSHSAdmissionCertificate_DTO>();

        public AdmissionFormContext _DomainModelMsSqlServerContext;
        ILogger<JSHSAdmissionCertificateImpl> _log;
        public JSHSAdmissionCertificateImpl(AdmissionFormContext DomainModelMsSqlServerContext, ILogger<JSHSAdmissionCertificateImpl> adm)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
            _log = adm;
        }
        public JSHSAdmissionCertificate_DTO getdata(JSHSAdmissionCertificate_DTO data)
        {

            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();

                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.AllAcademicYear = allacademic.ToArray();                 

                data.MasterCompany = (from a in _DomainModelMsSqlServerContext.Institution
                                      where (a.MI_Id == data.MI_Id)
                                      select new StudycertificateDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          MI_Id = a.MI_Id,
                                      }).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public JSHSAdmissionCertificate_DTO searchfilter(JSHSAdmissionCertificate_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (data.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (data.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    flag = "D";
                    flagactive = 1;
                    amatactiveflag = 1;
                }

                if (data.allorindid == "A")
                {
                    data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                         from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag 
                                         && (
                                         //(b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
                                         //+ ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) 
                                         //||
                                         b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) 
                                         || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) 
                                         || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new JSHSAdmissionCertificate_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                             (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                             (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName) +
                                             (b.AMST_AdmNo == null || b.AMST_AdmNo == "" ? "" : " : " + b.AMST_AdmNo)).Trim(),

                                         }).ToArray();
                }
                else
                {
                    data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                         from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper() + ' ' + b.AMST_MiddleName.Trim().ToUpper() + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new JSHSAdmissionCertificate_DTO
                                         {

                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                             (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                             (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName) +
                                             (b.AMST_AdmNo == null || b.AMST_AdmNo == "" ? "" : " : " + b.AMST_AdmNo)).Trim(),

                                         }).ToArray();
                }

                if (data.fillstudlist.Length > 0)
                {
                    data.count = data.fillstudlist.Length;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("searcherror in studycertificate: '" + ex.Message + "'");
            }
            return data;
        }
        public JSHSAdmissionCertificate_DTO onchangeyear(JSHSAdmissionCertificate_DTO data)
        {
            try
            {
                data.allclasslist = (from a in _DomainModelMsSqlServerContext.Masterclasscategory
                                     from c in _DomainModelMsSqlServerContext.School_M_Class
                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.Is_Active == true
                                     && c.ASMCL_ActiveFlag == true)
                                     select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public JSHSAdmissionCertificate_DTO onchangeclass(JSHSAdmissionCertificate_DTO data)
        {
            try
            {
                data.allsectionlist = (from a in _DomainModelMsSqlServerContext.Masterclasscategory
                                       from c in _DomainModelMsSqlServerContext.School_M_Class
                                       from d in _DomainModelMsSqlServerContext.AdmSchoolMasterClassCatSec
                                       from e in _DomainModelMsSqlServerContext.AdmSection
                                       where (a.ASMCL_Id == c.ASMCL_Id && a.ASMCC_Id == d.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.MI_Id == data.MI_Id && a.Is_Active == true && d.ASMCCS_ActiveFlg == true && e.ASMC_ActiveFlag == 1
                                       && c.ASMCL_ActiveFlag == true)
                                       select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public JSHSAdmissionCertificate_DTO onchangesection(JSHSAdmissionCertificate_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (data.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (data.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    flag = "D";
                    flagactive = 1;
                    amatactiveflag = 1;
                }

                data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                     from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                     where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
                                     + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                     select new JSHSAdmissionCertificate_DTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),
                                     }).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<JSHSAdmissionCertificate_DTO> getStudData(JSHSAdmissionCertificate_DTO stuDTO)
        {
            List<JSHSAdmissionCertificate_DTO> HFClist = new List<JSHSAdmissionCertificate_DTO>();
            try
            {
                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_jshs_Study_certificate_modified";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = stuDTO.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = stuDTO.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = stuDTO.ASMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@studid", SqlDbType.VarChar) { Value = stuDTO.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = stuDTO.MI_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        stuDTO.studentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                stuDTO.MasterCompany = (from a in _DomainModelMsSqlServerContext.Institution
                                        where (a.MI_Id == stuDTO.MI_Id)
                                        select new JSHSAdmissionCertificate_DTO
                                        {
                                            companyname = a.IVRMMCT_Name,
                                            MI_Id = a.MI_Id,
                                        }).ToArray();

                stuDTO.academicList1 = _DomainModelMsSqlServerContext.AcademicYear.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id).ToArray();

                stuDTO.principalsign = _DomainModelMsSqlServerContext.GenConfig.Where(a => a.MI_Id == stuDTO.MI_Id).ToArray();


                if (stuDTO.save_flag == "yes")
                {
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    string Reporttype = "";

                    if (Convert.ToInt32(stuDTO.radiobutton) == 0)
                    {
                        Reporttype = "Bonafide Certificate";
                    }
                    else if (Convert.ToInt32(stuDTO.radiobutton) == 2)
                    {
                        Reporttype = "Conduct Certificate";
                    }

                    StudycertificateReportDMO _report = new StudycertificateReportDMO();
                    _report.AMST_Id = stuDTO.AMST_Id;
                    _report.MI_Id = stuDTO.MI_Id;
                    _report.ASMAY_Id = stuDTO.ASMAY_Id;
                    _report.ASC_Date = indiantime0;
                    _report.ASC_ReportType = Reporttype;
                    _report.ASC_No = Convert.ToInt32(stuDTO.radiobutton);
                    _report.CreatedDate = indiantime0;
                    _report.UpdatedDate = indiantime0;
                    _report.ASC_Createdby = stuDTO.userId;
                    _report.ASC_Updatedby = stuDTO.userId;
                    _DomainModelMsSqlServerContext.Add(_report);
                    int i = _DomainModelMsSqlServerContext.SaveChanges();
                    if (i > 0)
                    {
                        stuDTO.message = "Record Saved Successfully";
                        var count = (from a in _DomainModelMsSqlServerContext.StudycertificateReportDMO
                                     where a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.ASC_No == Convert.ToInt32(stuDTO.radiobutton)
                                     select new JSHSAdmissionCertificate_DTO
                                     {
                                         AMST_Id = a.AMST_Id
                                     }).ToList().Count();

                        stuDTO.count = count;

                    }
                    else
                    {
                        stuDTO.message = "Record Not Saved Successfully";
                    }

                }
                else
                {
                    var count = (from a in _DomainModelMsSqlServerContext.StudycertificateReportDMO
                                 where a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.ASC_No == 1
                                 select new VikasaAdmissionreportDTO
                                 {
                                     AMST_Id = a.AMST_Id
                                 }).ToList().Count();

                    stuDTO.count = count;

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }
    }
}
