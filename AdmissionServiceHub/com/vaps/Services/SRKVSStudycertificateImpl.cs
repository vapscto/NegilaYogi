using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;

using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SRKVSStudycertificateImpl : Interfaces.SRKVSStudyCertificateInterface
    {



        private static ConcurrentDictionary<string, SRKVSStudycertificateDTO> _login =
            new ConcurrentDictionary<string, SRKVSStudycertificateDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        ILogger<SRKVSStudycertificateImpl> _log;
        public SRKVSStudycertificateImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext, ILogger<SRKVSStudycertificateImpl> adm)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
            _log = adm;
        }

        public SRKVSStudycertificateDTO getdetails(SRKVSStudycertificateDTO stu)
        {
            SRKVSStudycertificateDTO acdmc = new SRKVSStudycertificateDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == stu.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                acdmc.AllAcademicYear = allacademic.ToArray();

                acdmc.allclasslist = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                acdmc.allsectionlist = _DomainModelMsSqlServerContext.School_M_Section.Where(t => t.MI_Id == stu.MI_Id && t.ASMC_ActiveFlag == 1).ToArray();

                acdmc.MasterCompany = (from a in _DomainModelMsSqlServerContext.Institution
                                       where (a.MI_Id == stu.MI_Id)
                                       select new SRKVSStudycertificateDTO
                                       {
                                           companyname = a.IVRMMCT_Name,
                                           MI_Id = a.MI_Id,
                                       }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }


        public SRKVSStudycertificateDTO getstudlist(SRKVSStudycertificateDTO stu)
        {
            try
            {
                List<SRKVSStudycertificateDTO> stulist = new List<SRKVSStudycertificateDTO>();
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
                                    select new SRKVSStudycertificateDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo
                                    }).ToArray();

                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == stu.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                stu.AllAcademicYear = allacademic.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        public async System.Threading.Tasks.Task<SRKVSStudycertificateDTO> getStudDetails(SRKVSStudycertificateDTO stuDTO)
        {
            List<SRKVSStudycertificateDTO> HFClist = new List<SRKVSStudycertificateDTO>();
            try
            {

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
                                        select new SRKVSStudycertificateDTO
                                        {
                                            companyname = a.IVRMMCT_Name,
                                            MI_Id = a.MI_Id,
                                        }).ToArray();

                stuDTO.academicList1 = _DomainModelMsSqlServerContext.AcademicYear.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id).ToArray();

                stuDTO.principalsign = _DomainModelMsSqlServerContext.GenConfig.Where(a => a.MI_Id == stuDTO.MI_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }

        public SRKVSStudycertificateDTO onacademicyearchange(SRKVSStudycertificateDTO data)
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

                List<SRKVSStudycertificateDTO> stulist = new List<SRKVSStudycertificateDTO>();
                data.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == data.AMST_SOL && a.AMST_SOL == flag && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                     select new SRKVSStudycertificateDTO
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

        public SRKVSStudycertificateDTO searchfilter(SRKVSStudycertificateDTO data)
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
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
                                         + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new SRKVSStudycertificateDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                         }
             ).ToArray();
                }
                else
                {
                    data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                         from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper() + ' ' + b.AMST_MiddleName.Trim().ToUpper() + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new SRKVSStudycertificateDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

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
                _log.LogInformation("searcherror in studycertificate: '" + ex.Message + "'");
            }
            return data;
        }

        public SRKVSStudycertificateDTO Studdetailsconduct(SRKVSStudycertificateDTO data)
        {
            try
            {
                if (data.ASMCL_Id == 0 && data.ASMC_Id == 0)
                {
                    data.studentlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                        from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                        from c in _DomainModelMsSqlServerContext.School_M_Class
                                        from d in _DomainModelMsSqlServerContext.School_M_Section
                                        from e in _DomainModelMsSqlServerContext.AcademicYear
                                        where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                        select new SRKVSStudycertificateDTO
                                        {
                                            AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                            dob = b.AMST_DOB,

                                            dobwords = b.AMST_DOB_Words,

                                            AMST_AdmNo = b.AMST_AdmNo,

                                            fathername = ((b.AMST_FatherName == null || b.AMST_FatherName == "0" ? "" : b.AMST_FatherName) + " " + (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "0" ? "" : b.AMST_FatherSurname)).Trim(),

                                            joinedyear = Convert.ToDateTime(b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ASMAY_Id).FirstOrDefault().ASMAY_From_Date : DateTime.Now),

                                            leftyear = Convert.ToDateTime(a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id).FirstOrDefault().ASMAY_To_Date : DateTime.Now),

                                            joinedclass = b.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == b.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                            leftclass = a.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == a.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                            joinedyearname = b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                                          && t.ASMAY_Id == b.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                            leftyearname = a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                                            && t.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                            mothertounge = b.AMST_MotherTongue,

                                            gender = b.AMST_Sex
                                        }).ToArray();
                }
                else
                {
                    data.studentlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                        from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                        from c in _DomainModelMsSqlServerContext.School_M_Class
                                        from d in _DomainModelMsSqlServerContext.School_M_Section
                                        from e in _DomainModelMsSqlServerContext.AcademicYear
                                        where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id
                                        && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id
                                        && a.AMST_Id == data.AMST_Id)
                                        select new SRKVSStudycertificateDTO
                                        {
                                            AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                            dob = b.AMST_DOB,

                                            dobwords = b.AMST_DOB_Words,

                                            AMST_AdmNo = b.AMST_AdmNo,

                                            fathername = ((b.AMST_FatherName == null || b.AMST_FatherName == "0" ? "" : b.AMST_FatherName) + " " + (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "0" ? "" : b.AMST_FatherSurname)).Trim(),

                                            mothername = ((b.AMST_MotherName == null || b.AMST_MotherName == "0" ? "" : b.AMST_MotherName) + " " + (b.AMST_MotherSurname == null || b.AMST_MotherSurname == "0" ? "" : b.AMST_MotherSurname)).Trim(),

                                            joinedyear = Convert.ToDateTime(b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ASMAY_Id).FirstOrDefault().ASMAY_From_Date : DateTime.Now),

                                            leftyear = Convert.ToDateTime(a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id).FirstOrDefault().ASMAY_To_Date : DateTime.Now),

                                            joinedyearname = b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                                            && t.ASMAY_Id == b.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                            leftyearname = a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                                            && t.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id).FirstOrDefault().ASMAY_Year : "",

                                            joinedclass = b.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id
                                            && t.ASMCL_Id == b.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                            leftclass = a.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == a.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id).FirstOrDefault().ASMCL_ClassName : "",

                                            mothertounge = b.AMST_MotherTongue,
                                            gender = b.AMST_Sex

                                        }).ToArray();
                }

                if (data.save_flag == "Yes")
                {
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    string Reporttype = "";
                    Reporttype = "Bonafide Certificate";
                    StudycertificateReportDMO _report = new StudycertificateReportDMO();
                    _report.AMST_Id = data.AMST_Id;
                    _report.MI_Id = data.MI_Id;
                    _report.ASC_No = 0;
                    _report.ASC_Date = indiantime0;
                    _report.ASC_ReportType = Reporttype;
                    _report.CreatedDate = indiantime0;
                    _report.UpdatedDate = indiantime0;
                    _report.ASC_Createdby = data.userid;
                    _report.ASC_Updatedby = data.userid;
                    _DomainModelMsSqlServerContext.Add(_report);
                    int i = _DomainModelMsSqlServerContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Record Saved Successfully";
                    }
                    else
                    {
                        data.message = "Record Not Saved Successfully";
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
