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
    public class HHSStudyCertificateImpl : Interfaces.HHSStudyCertificateInterface
    {
        private static ConcurrentDictionary<string, HHSStudyCertificateDTO> _login =
             new ConcurrentDictionary<string, HHSStudyCertificateDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        ILogger<HHSStudyCertificateImpl> _log;
        public HHSStudyCertificateImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext, ILogger<HHSStudyCertificateImpl> adm)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
            _log = adm;
        }
        public HHSStudyCertificateDTO getdetails(HHSStudyCertificateDTO data)
        {
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.AllAcademicYear = allacademic.ToArray();

                data.allclasslist = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                data.allsectionlist = _DomainModelMsSqlServerContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToArray();

                data.MasterCompany = (from a in _DomainModelMsSqlServerContext.Institution
                                      where (a.MI_Id == data.MI_Id)
                                      select new HHSStudyCertificateDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          MI_Id = a.MI_Id,
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("HHSStudyCertificate Getdata" + ex.Message);
            }
            return data;
        }
        public HHSStudyCertificateDTO getstudlist(HHSStudyCertificateDTO stu)
        {
            try
            {
                List<HHSStudyCertificateDTO> stulist = new List<HHSStudyCertificateDTO>();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (stu.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (stu.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }
                else if (stu.AMST_SOL == "D")
                {
                    flag = "D";
                    flagactive = 1;
                    amatactiveflag = 1;
                }

                stu.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                    from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == flag && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag && a.MI_Id == stu.MI_Id)
                                    select new HHSStudyCertificateDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo
                                    }).ToArray();




                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == stu.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToList();
                stu.AllAcademicYear = allacademic.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
        public async System.Threading.Tasks.Task<HHSStudyCertificateDTO> getStudDetails(HHSStudyCertificateDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<HHSStudyCertificateDTO> HFClist = new List<HHSStudyCertificateDTO>();
            try
            {
                stuDTO.studentlist1 = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                       from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                       from c in _DomainModelMsSqlServerContext.School_M_Class
                                       from d in _DomainModelMsSqlServerContext.School_M_Section
                                       from e in _DomainModelMsSqlServerContext.AcademicYear
                                       where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.AMST_Id == stuDTO.AMST_Id)
                                       select new StudycertificateDTO
                                       {
                                           AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                           joinedyear = Convert.ToDateTime(b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.ASMAY_Id == b.ASMAY_Id && t.MI_Id == stuDTO.MI_Id).FirstOrDefault().ASMAY_From_Date : DateTime.Now),

                                           leftyear = Convert.ToDateTime(a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == stuDTO.MI_Id && t.ASMAY_Id == a.ASMAY_Id).FirstOrDefault().ASMAY_To_Date : DateTime.Now),

                                           joinedclass = b.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == stuDTO.MI_Id && t.ASMCL_Id == b.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                           leftclass = a.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == stuDTO.MI_Id && t.ASMCL_Id == a.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",
                                       }).ToArray();

                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Study_certificate_modified";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                    //cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
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
                                        select new HHSStudyCertificateDTO
                                        {
                                            companyname = a.MI_Name,
                                            address = ((a.MI_Address1 == null || a.MI_Address1 == "" ? "" : a.MI_Address1) + (a.MI_Address2 == null || a.MI_Address2 == "" || a.MI_Address2 == "0" ? "" : "," + a.MI_Address2) + (a.MI_Address3 == null || a.MI_Address3 == "" || a.MI_Address3 == "0" ? "" : "," + a.MI_Address3) + (Convert.ToString(a.MI_Pincode) == null || Convert.ToString(a.MI_Pincode) == "" || Convert.ToString(a.MI_Pincode) == "0" ? "" : "-" + Convert.ToString(a.MI_Pincode))).Trim(),
                                            MI_Id = a.MI_Id,
                                            milogo = a.MI_Logo
                                        }).ToArray();

                stuDTO.academicList1 = _DomainModelMsSqlServerContext.AcademicYear.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id).ToArray();

                stuDTO.studentreportcount = _DomainModelMsSqlServerContext.StudycertificateReportDMO.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.AMST_Id == stuDTO.AMST_Id).ToArray();

                stuDTO.leftstudentdetails = _DomainModelMsSqlServerContext.Student_TC.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id
                && a.AMST_Id == stuDTO.AMST_Id).ToArray();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                
                if (stuDTO.save_flag == "yes")
                {
                    StudycertificateReportDMO _report = new StudycertificateReportDMO();
                    _report.AMST_Id = stuDTO.AMST_Id;
                    _report.MI_Id = stuDTO.MI_Id;
                    _report.ASMAY_Id = stuDTO.ASMAY_Id;
                    _report.ASC_Date = indiantime0;
                    _report.ASC_No = 0;
                    _report.ASC_ReportType = "Bonafide Certificate";
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
                                     where a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.ASC_No == 0
                                     select new VikasaAdmissionreportDTO
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }
        public async System.Threading.Tasks.Task<HHSStudyCertificateDTO> MigrationCertificateStuddetails(HHSStudyCertificateDTO stuDTO)
        {            
            List<HHSStudyCertificateDTO> HFClist = new List<HHSStudyCertificateDTO>();
            try
            {
                stuDTO.studentlist1 = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                       from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                       from c in _DomainModelMsSqlServerContext.School_M_Class
                                       from d in _DomainModelMsSqlServerContext.School_M_Section
                                       from e in _DomainModelMsSqlServerContext.AcademicYear
                                       where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.AMST_Id == stuDTO.AMST_Id)
                                       select new StudycertificateDTO
                                       {
                                           AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                           joinedyear = Convert.ToDateTime(b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.ASMAY_Id == b.ASMAY_Id && t.MI_Id == stuDTO.MI_Id).FirstOrDefault().ASMAY_From_Date : DateTime.Now),

                                           leftyear = Convert.ToDateTime(a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == stuDTO.MI_Id && t.ASMAY_Id == a.ASMAY_Id).FirstOrDefault().ASMAY_To_Date : DateTime.Now),

                                           joinedclass = b.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == stuDTO.MI_Id && t.ASMCL_Id == b.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                           leftclass = a.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == stuDTO.MI_Id && t.ASMCL_Id == a.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",
                                       }).ToArray();

                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Study_certificate_modified";
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
                                        select new HHSStudyCertificateDTO
                                        {
                                            companyname = a.MI_Name,
                                            address = ((a.MI_Address1 == null || a.MI_Address1 == "" ? "" : a.MI_Address1) + (a.MI_Address2 == null || a.MI_Address2 == "" || a.MI_Address2 == "0" ? "" : "," + a.MI_Address2) + (a.MI_Address3 == null || a.MI_Address3 == "" || a.MI_Address3 == "0" ? "" : "," + a.MI_Address3) + (Convert.ToString(a.MI_Pincode) == null || Convert.ToString(a.MI_Pincode) == "" || Convert.ToString(a.MI_Pincode) == "0" ? "" : "-" + Convert.ToString(a.MI_Pincode))).Trim(),
                                            MI_Id = a.MI_Id,
                                            milogo = a.MI_Logo
                                        }).ToArray();

                stuDTO.academicList1 = _DomainModelMsSqlServerContext.AcademicYear.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id).ToArray();

                stuDTO.studentreportcount = _DomainModelMsSqlServerContext.StudycertificateReportDMO.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id && a.AMST_Id == stuDTO.AMST_Id).ToArray();

                stuDTO.leftstudentdetails = _DomainModelMsSqlServerContext.Student_TC.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id
                && a.AMST_Id == stuDTO.AMST_Id).ToArray();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                
                if (stuDTO.save_flag == "yes")
                {
                    StudycertificateReportDMO _report = new StudycertificateReportDMO();
                    _report.AMST_Id = stuDTO.AMST_Id;
                    _report.MI_Id = stuDTO.MI_Id;
                    _report.ASMAY_Id = stuDTO.ASMAY_Id;
                    _report.ASC_Date = indiantime0;
                    _report.ASC_No = 10;
                    _report.ASC_ReportType = "Migration Certificate";
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
                                     where a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id
                                     select new VikasaAdmissionreportDTO
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }
        public HHSStudyCertificateDTO onacademicyearchange(HHSStudyCertificateDTO data)
        {
            try
            {
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

                List<HHSStudyCertificateDTO> stulist = new List<HHSStudyCertificateDTO>();
                data.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == data.AMST_SOL && a.AMST_SOL == flag && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                     select new HHSStudyCertificateDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),

                                     }
             ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HHSStudyCertificateDTO searchfilter(HHSStudyCertificateDTO data)
        {
            try
            {
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                data.searchfilter = data.searchfilter.ToUpper();
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
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && 
                                         b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag)
                                         //&& (
                                         //(
                                         //b.AMST_FirstName.Trim().ToUpper() 
                                         //+ ' ' + b.AMST_MiddleName.Trim().ToUpper() + ' ' +
                                         //b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) 
                                        // || 
                                         //b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter)
                                        // || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) 
                                         //|| b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         //)
                                         select new HHSStudyCertificateDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             //AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),
                                             AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName) + ':' + (b.AMST_AdmNo == null ? " " : b.AMST_AdmNo)).Trim(),
                                         }
             ).ToArray();
                }
                else
                {
                    data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                         from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id && a.AMAY_ActiveFlag == amatactiveflag) //&& ((b.AMST_FirstName.Trim().ToUpper() + ' ' + b.AMST_MiddleName.Trim().ToUpper() + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new HHSStudyCertificateDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName) + ':' + (b.AMST_AdmNo == null ? " " : b.AMST_AdmNo)).Trim(),

                                         }
             ).ToArray();
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
                _log.LogInformation("searcherror in HHSStudycertificate: '" + ex.Message + "'");
            }
            return data;
        }
        public HHSStudyCertificateDTO getstudentname(HHSStudyCertificateDTO data)
        {
            try
            {
                data.studentreportcount = _DomainModelMsSqlServerContext.StudycertificateReportDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("searcherror in HHSStudycertificate: '" + ex.Message + "'");
            }
            return data;
        }

        //Certificate Generated Report
        public HHSStudyCertificateDTO CertificateGeneratedReportLoad(HHSStudyCertificateDTO data)
        {
            try
            {
                data.GetReportTypes = (from a in _DomainModelMsSqlServerContext.StudycertificateReportDMO
                                       where (a.MI_Id == data.MI_Id)
                                       select new HHSStudyCertificateDTO
                                       {
                                           ReportType = a.ASC_ReportType
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("searcherror in HHSStudycertificate: '" + ex.Message + "'");
            }
            return data;
        }
        public HHSStudyCertificateDTO GetCertificateGeneratedReport(HHSStudyCertificateDTO data)
        {
            try
            {
                DateTime todateconfrom = DateTime.Now;
                string fromdate = "";
                todateconfrom = Convert.ToDateTime(data.FromDate.Date.ToString("yyyy-MM-dd"));
                fromdate = todateconfrom.ToString("yyyy-MM-dd");

                DateTime todateconto = DateTime.Now;
                string todate = "";
                todateconto = Convert.ToDateTime(data.ToDate.Date.ToString("yyyy-MM-dd"));
                todate = todateconto.ToString("yyyy-MM-dd");


                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Certificate_Generated_ReportDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@Toate", SqlDbType.VarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@Report_Type", SqlDbType.VarChar) { Value = data.Report_Type });
                    cmd.Parameters.Add(new SqlParameter("@Report_Name", SqlDbType.VarChar) { Value = data.Report_Name });

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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetReportDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("searcherror in HHSStudycertificate: '" + ex.Message + "'");
            }
            return data;
        }

    }
}