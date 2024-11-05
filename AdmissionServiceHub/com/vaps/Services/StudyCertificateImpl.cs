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
using CommonLibrary;



namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudyCertificateImpl : Interfaces.StudyCertificateInterface
    {
        private static ConcurrentDictionary<string, StudycertificateDTO> _login =
            new ConcurrentDictionary<string, StudycertificateDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        ILogger<StudyCertificateImpl> _log;
        public StudyCertificateImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext, ILogger<StudyCertificateImpl> adm)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
            _log = adm;
        }

        public StudycertificateDTO getdetails(StudycertificateDTO stu)
        {
            StudycertificateDTO acdmc = new StudycertificateDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == stu.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                acdmc.AllAcademicYear = allacademic.ToArray();

                acdmc.allclasslist = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                acdmc.allsectionlist = _DomainModelMsSqlServerContext.School_M_Section.Where(t => t.MI_Id == stu.MI_Id && t.ASMC_ActiveFlag == 1).ToArray();


                //acdmc.adm_m_student = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                //                       from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                //                       where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S" && a.MI_Id == stu.MI_Id)
                //                       select new StudycertificateDTO
                //                       {
                //                           AMST_Id = a.AMST_Id,
                //                           AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                //                       }).ToArray();

                acdmc.MasterCompany = (from a in _DomainModelMsSqlServerContext.Institution
                                       where (a.MI_Id == stu.MI_Id)
                                       select new StudycertificateDTO
                                       {
                                           companyname = a.IVRMMCT_Name,
                                           MI_Id = a.MI_Id,
                                       }).ToArray();



                var cat = _DomainModelMsSqlServerContext.GenConfig.Where(g => g.MI_Id == stu.MI_Id && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    stu.category_list = _DomainModelMsSqlServerContext.mastercategory.Where(f => f.MI_Id == stu.MI_Id && f.AMC_ActiveFlag == 1).ToArray();
                    stu.categoryflag = true;
                }
                else
                {
                    stu.categoryflag = false;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }
        public StudycertificateDTO getstudlist(StudycertificateDTO stu)
        {

            try
            {
                List<StudycertificateDTO> stulist = new List<StudycertificateDTO>();
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
                                    select new StudycertificateDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo
                                    }
             ).ToArray();

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
        public async System.Threading.Tasks.Task<StudycertificateDTO> getStudDetails(StudycertificateDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<StudycertificateDTO> HFClist = new List<StudycertificateDTO>();
            try
            {

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
                                        select new StudycertificateDTO
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
        public StudycertificateDTO onacademicyearchange(StudycertificateDTO data)
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

                List<StudycertificateDTO> stulist = new List<StudycertificateDTO>();
                data.fillstudlist = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                                     from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == data.AMST_SOL && a.AMST_SOL == flag && a.AMST_ActiveFlag == flagactive && b.AMAY_ActiveFlag == amatactiveflag && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                     select new StudycertificateDTO
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

        public StudycertificateDTO searchfilter(StudycertificateDTO data)
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
                                         &&
                                         ((b.AMST_FirstName != null ? b.AMST_FirstName.Trim().ToUpper().Trim() : " " + " " + b.AMST_MiddleName != null ? b.AMST_MiddleName.Trim().ToUpper() : " " + " " + b.AMST_LastName != null ? b.AMST_LastName.Trim().ToUpper() : " ").Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter))
                                         )
                                         select new StudycertificateDTO
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
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id && a.AMAY_ActiveFlag == amatactiveflag 
                                         &&
                                         ((b.AMST_FirstName != null ? b.AMST_FirstName.Trim().ToUpper().Trim() : " " + " " + b.AMST_MiddleName != null ? b.AMST_MiddleName.Trim().ToUpper() : " " + " " + b.AMST_LastName != null ? b.AMST_LastName.Trim().ToUpper() : " ").Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter))
                                         )
                                         select new StudycertificateDTO
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

        public StudycertificateDTO Studdetailsconduct(StudycertificateDTO data)
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
                                        from f in _DomainModelMsSqlServerContext.mastercategory
                                        where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMC_Id==f.AMC_Id)
                                        select new StudycertificateDTO
                                        {
                                            AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                            dob = b.AMST_DOB,

                                            dobwords = b.AMST_DOB_Words,

                                            AMST_AdmNo = b.AMST_AdmNo,

                                            fathername = ((b.AMST_FatherName == null || b.AMST_FatherName == "0" ? "" : b.AMST_FatherName) + " " +
                                            (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "0" ? "" : b.AMST_FatherSurname)).Trim(),

                                            joinedyear = Convert.ToDateTime(b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ASMAY_Id).FirstOrDefault().ASMAY_From_Date : DateTime.Now),

                                            leftyear = Convert.ToDateTime(a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id).FirstOrDefault().ASMAY_To_Date : DateTime.Now),

                                            joinedclass = b.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == b.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                            leftclass = a.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == a.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",
                                        }).ToArray();
                }
                else
                {
                    data.studentlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                        from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                        from c in _DomainModelMsSqlServerContext.School_M_Class
                                        from d in _DomainModelMsSqlServerContext.School_M_Section
                                        from e in _DomainModelMsSqlServerContext.AcademicYear
                                        where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id && a.AMST_Id == data.AMST_Id)
                                        select new StudycertificateDTO
                                        {
                                            AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                            dob = b.AMST_DOB,

                                            dobwords = b.AMST_DOB_Words,

                                            AMST_AdmNo = b.AMST_AdmNo,

                                            fathername = ((b.AMST_FatherName == null || b.AMST_FatherName == "0" ? "" : b.AMST_FatherName) + " " +
                                            (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "0" ? "" : b.AMST_FatherSurname)).Trim(),

                                            joinedyear = Convert.ToDateTime(b.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ASMAY_Id).FirstOrDefault().ASMAY_From_Date : DateTime.Now),

                                            leftyear = Convert.ToDateTime(a.ASMAY_Id != 0 ? _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id).FirstOrDefault().ASMAY_To_Date : DateTime.Now),

                                            joinedclass = b.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == b.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",

                                            leftclass = a.ASMCL_Id != 0 ? _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == a.ASMCL_Id).FirstOrDefault().ASMCL_ClassName : "",
                                        }).ToArray();
                }


                if (data.AMC_Id == null || data.AMC_Id == 0)
                {
                    data.AMC_Id = 0;

                }


                var amcid = data.AMC_Id.ToString();

                data.AMC_logo = _DomainModelMsSqlServerContext.mastercategory.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.MI_Id && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();




                if (data.save_flag == "Yes")
                {
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    StudycertificateReportDMO _report = new StudycertificateReportDMO();
                    _report.AMST_Id = data.AMST_Id;
                    _report.MI_Id = data.MI_Id;
                    _report.ASC_No = 2;
                    _report.ASC_ReportType = "Conduct Certificate";
                    _report.ASC_Date = indiantime0;
                    _report.CreatedDate = indiantime0;
                    _report.UpdatedDate = indiantime0;
                    _report.ASC_Updatedby = data.userid;
                    _report.ASC_Createdby = data.userid;
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

        //Endorsement SRKVS
        public StudycertificateDTO searchfilterSRKVS(StudycertificateDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
              
             

             //   if (data.allorindid == "A")
             //   {
             //       data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
             //                            from b in _DomainModelMsSqlServerContext.Adm_M_Student
             //                            where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && ((b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
             //                            + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
             //                            select new StudycertificateDTO
             //                            {
             //                                AMST_Id = a.AMST_Id,
             //                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

             //                            }
             //).ToArray();
             //   }
             //   else
             //   {
                    data.fillstudlist = (from a in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                                         from b in _DomainModelMsSqlServerContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.ASMCL_Id == data.ASMCL_Id && a.AMAY_ActiveFlag == 1 && ((b.AMST_FirstName.Trim().ToUpper() + ' ' + b.AMST_MiddleName.Trim().ToUpper() + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new StudycertificateDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                         }
             ).ToArray();
                //}



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

        public async System.Threading.Tasks.Task<StudycertificateDTO> getStudDetailsSRKVS(StudycertificateDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<StudycertificateDTO> HFClist = new List<StudycertificateDTO>();

            //Array amst_email =[];
            var AMST_EMAIL = "";
            try
            {

                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_EndorsementData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                    //cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                    cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = stuDTO.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = stuDTO.ASMCL_Id });
                   // cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = stuDTO.ASMC_Id });
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
                       // AMST_EMAIL = stuDTO.studentlist
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                stuDTO.MasterCompany = (from a in _DomainModelMsSqlServerContext.Institution
                                        where (a.MI_Id == stuDTO.MI_Id)
                                        select new StudycertificateDTO
                                        {
                                            companyname = a.IVRMMCT_Name,
                                            MI_Id = a.MI_Id,
                                        }).ToArray();

                stuDTO.academicList1 = _DomainModelMsSqlServerContext.AcademicYear.Where(a => a.MI_Id == stuDTO.MI_Id && a.ASMAY_Id == stuDTO.ASMAY_Id).ToArray();

                stuDTO.principalsign = _DomainModelMsSqlServerContext.GenConfig.Where(a => a.MI_Id == stuDTO.MI_Id).ToArray();


                if (stuDTO.mailflag == true)
                {
                    var caution_amt = "";
                    var demand_amt = "";
                    var from_btw_time = "";
                    var to_btw_time = "";
                    var from_time = "";
                    var brought_admission = "";
                    var first_installment = "";
                    var selected_date = "";
                    var paid_onor_before = "";
                    DateTime brughtcon = DateTime.Now;
                    DateTime selectedcon = DateTime.Now;
                    DateTime paid_onorcon = DateTime.Now;
                    if (stuDTO.studentlist.Length > 0)
                    {
                        try
                        {
                            if (stuDTO.mailParameters.Length > 0)
                            {
                              
                              
                                {
                                    foreach (var ue in stuDTO.mailParameters)
                                    {

                                        caution_amt = ue.caution_amt;
                                        demand_amt = ue.demand_amt;
                                        from_btw_time = ue.from_btw_time;
                                        to_btw_time = ue.to_btw_time;
                                        from_time = ue.before_time;
                                        first_installment = ue.first_installemnt;
                                        brughtcon =  Convert.ToDateTime(ue.brought_admission.Date.ToString("yyyy-MM-dd"));
                                        brought_admission = brughtcon.ToString("yyyy-MM-dd");
                                        selectedcon = Convert.ToDateTime(ue.selected_date.Date.ToString("yyyy-MM-dd"));
                                        selected_date = selectedcon.ToString("yyyy-MM-dd");
                                        paid_onorcon = Convert.ToDateTime(ue.paid_onor_before.Date.ToString("yyyy-MM-dd"));
                                        paid_onor_before = paid_onorcon.ToString("yyyy-MM-dd");
                                    }

                                }
                            }

                                Email Email = new Email(_DomainModelMsSqlServerContext);
                            Email.sendEndorsementcerti(stuDTO.MI_Id, stuDTO.AMST_emailId, stuDTO.Template, stuDTO.mailParameters, stuDTO.ASMCL_Id, stuDTO.ASMAY_Id, stuDTO.AMST_Id, stuDTO.userid, caution_amt,demand_amt, from_btw_time, to_btw_time,from_time,brought_admission,selected_date,first_installment,paid_onor_before);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

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