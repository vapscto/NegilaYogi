using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;
using DomainModel.Model.com.vaps.admission;
using CommonLibrary;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.admission;

namespace CollegePreadmission.Services
{
    public class CollegeStudentappImpl :Interfaces.CollegeStudentappInterface
    {

        private static ConcurrentDictionary<string, StudentApplicationDTO> _login =
             new ConcurrentDictionary<string, StudentApplicationDTO>();

        public StudentApplicationContext _StudentApplicationContext;
        // Added on 9-11-2016
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;

        public FeeGroupContext _feecontext;
        //public DomainModelMsSqlServerContext _context;
        public ProspectusContext _ProspectusContext;

        // added on 9-11-2016
        public CollegeStudentappImpl(StudentApplicationContext StudentApplicationContext, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext db, FeeGroupContext feecontext, ProspectusContext ProspectusContext)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _UserManager = UserManager;
            _db = db;
            _feecontext = feecontext;
            _ProspectusContext = ProspectusContext;
        }

        public async Task<CountryDTO> countrydrp(CountryDTO stu)
        {
            try
            {
                List<Country> allCountry = new List<Country>();
                allCountry = await _StudentApplicationContext.country.ToListAsync();
                stu.countryDrpDown = allCountry.ToArray();

                List<City> allcity = new List<City>();
                allcity = await _StudentApplicationContext.city.ToListAsync();
                stu.citydro = allcity.ToArray();

                List<Route> allroute = new List<Route>();
                allroute = await _StudentApplicationContext.route.Where(t => t.MI_Id == stu.MI_Id).ToListAsync();
                stu.routedrp = allroute.ToArray();

                List<Location> alllocation = new List<Location>();
                alllocation = await _StudentApplicationContext.location.Where(t => t.MI_Id == stu.MI_Id).ToListAsync();
                stu.locationdrp = alllocation.ToArray();

                List<Fee_Master_ConcessionDMO> Master_Concessionlist = new List<Fee_Master_ConcessionDMO>();
                Master_Concessionlist = await _StudentApplicationContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                stu.categorydrp = Master_Concessionlist.ToArray();



                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                stu.admissioncatdrp = allclass.ToArray();



                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id.Equals(stu.MI_Id) && t.ASMAY_Id.Equals(stu.ASMAY_Id) && t.ASMAY_ActiveFlag == 1).ToListAsync();
                stu.academicdrp = allyear.ToArray();

                List<School_M_Section> allSection = new List<School_M_Section>();
                allSection = await _StudentApplicationContext.Section.Where(t => t.MI_Id.Equals(stu.MI_Id) && t.ASMC_ActiveFlag == 1).ToListAsync();
                stu.sectiondropdown = allSection.ToArray();

                // Syllabus list
                List<Pre_Adm_Syllabus> SyllabusList = new List<Pre_Adm_Syllabus>();
                SyllabusList = await _StudentApplicationContext.Pre_Adm_Syllabus.ToListAsync();
                stu.syllabuslist = SyllabusList.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        // Added on 19-9-2016
        [Route("country")]
        public async Task<CountryDTO> getIndependentDropDowns(CountryDTO stu)
        {
            try
            {
                //var Acdemic_preadmission = _db.AcademicYear.Where(t => t.MI_Id.Equals(stu.MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                DateTime startdate = Convert.ToDateTime(_StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());


                stu.ASMAY_Id = Acdemic_preadmission;
                stu.ASMAY_PreAdm_F_Date = startdate;


                List<MasterAcademic> allyear = new List<MasterAcademic>();
                List<MasterAcademic> allyearget = new List<MasterAcademic>();
                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == stu.roleId).IVRMRT_Role;

                if (rolename == "OnlinePreadmissionUser")
                {
                    allyearget = (from a in _feecontext.AcademicYear
                                  where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                  select new MasterAcademic
                                  {
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMAY_Year = a.ASMAY_Year
                                  }
                     ).ToList();

                    allyear = (from a in _feecontext.AcademicYear
                               where (a.MI_Id == stu.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                   ).ToList();


                    if (allyear.Count == 0)
                    {


                        List<Preadmission_Special_Registration> alllocation = new List<Preadmission_Special_Registration>();
                        alllocation = _StudentApplicationContext.Preadmission_Special_Registration.Where(t => t.ID == stu.Id).ToList();
                        if (alllocation.Count > 0)
                        {
                            allyear = (from a in _feecontext.AcademicYear
                                       where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                       select new MasterAcademic
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           ASMAY_Year = a.ASMAY_Year
                                       }
                ).ToList();
                        }
                    }


                    stu.countrole = false;


                }
                else
                {
                    //   allyear = (from a in _feecontext.AcademicYear
                    //              where (a.MI_Id == stu.MI_Id)
                    //              select new MasterAcademic
                    //              {
                    //                  ASMAY_Id = a.ASMAY_Id,
                    //                  ASMAY_Year = a.ASMAY_Year
                    //              }
                    //).ToList();

                    allyear = (from a in _feecontext.AcademicYear
                               where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                   ).ToList();

                    stu.countrole = true;


                }
                stu.academicdrp = allyear.ToArray();

                if (allyear.Count > 0)
                {

                    List<Country> allCountry = new List<Country>();
                    //allCountry = await _StudentApplicationContext.country.Include(State => State.vstate).ToListAsync();
                    allCountry = await _StudentApplicationContext.country.ToListAsync();
                    stu.countryDrpDown = allCountry.ToArray();


                    List<Route> allroute = new List<Route>();
                    allroute = await _StudentApplicationContext.route.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.routedrp = allroute.ToArray();

                    List<Location> alllocation = new List<Location>();
                    alllocation = await _StudentApplicationContext.location.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.locationdrp = alllocation.ToArray();


                    List<Fee_Master_ConcessionDMO> Master_Concessionlist = new List<Fee_Master_ConcessionDMO>();
                    Master_Concessionlist = await _StudentApplicationContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.categorydrp = Master_Concessionlist.ToArray();




                    List<Caste> allcaste = new List<Caste>();
                    allcaste = await _StudentApplicationContext.caste.Where(f => f.MI_Id == stu.MI_Id).ToListAsync();
                    stu.castedrp = allcaste.ToArray();





                    stu.subcastedrp = (from a in _StudentApplicationContext.caste
                                       from d in _StudentApplicationContext.subcaste
                                       where (a.IMC_Id == d.IMC_ID && a.MI_Id == stu.MI_Id)
                                       select d).ToArray();


                    List<Religion> allreligion = new List<Religion>();
                    allreligion = await _StudentApplicationContext.religion.ToListAsync();
                    stu.religiondrp = allreligion.ToArray();

                    List<CasteCategory> allcc = new List<CasteCategory>();
                    allcc = await _StudentApplicationContext.castecategory.ToListAsync();
                    stu.castecategorydrp = allcc.ToArray();




                    //SECTION dropdownlist added on 14/12/2016

                    List<School_M_Section> allSection = new List<School_M_Section>();

                    allSection = await _StudentApplicationContext.Section.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.sectiondropdown = allSection.ToArray();

                    List<MasterDocumentDMO> MasterDocumentDMO = new List<MasterDocumentDMO>();
                    MasterDocumentDMO = await _StudentApplicationContext.MasterDocumentDMO.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.DocumentList = MasterDocumentDMO.ToArray();

                    // Syllabus list
                    List<Pre_Adm_Syllabus> SyllabusList = new List<Pre_Adm_Syllabus>();
                    SyllabusList = await _StudentApplicationContext.Pre_Adm_Syllabus.Where(t => t.MI_ID == stu.MI_Id).ToListAsync();
                    stu.syllabuslist = SyllabusList.ToArray();



                    ProspectusDTO dto = new ProspectusDTO();
                    List<ProspectusFilePath> prospectus = new List<ProspectusFilePath>();
                    prospectus = _db.ProspectusFilePath.Where(d => d.MI_ID == stu.MI_Id).ToList();
                    if (prospectus.Count > 0)
                    {
                        stu.prospectusfilePath = prospectus.FirstOrDefault().IPPC_Path;
                    }

                    stu.precutdate = "True";
                }
                else
                {
                    stu.precutdate = "false";
                }

                var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleId).ToList();
                stu.roleName = rolelist[0].IVRMRT_Role;
                if (rolelist[0].IVRMRT_Role.Equals("ADMIN") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR") || rolelist[0].IVRMRT_Role.Equals("Admission End User"))
                {

                    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                    allRegStudent = await _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.PASR_Adm_Confirm_Flag == false && d.ASMAY_Id == stu.ASMAY_Id).ToListAsync();
                    stu.registrationList = allRegStudent.ToArray();

                    stu.registrationListhealth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                  from b in _StudentApplicationContext.Enq
                                                  where (a.PASR_Id == b.pasr_id && b.MI_Id == stu.MI_Id && b.PASR_Adm_Confirm_Flag == false)
                                                  select b
                      ).ToList().ToArray();



                }
                else
                {

                    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                    allRegStudent = await _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.Id.Equals(stu.Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id) && d.PASR_Adm_Confirm_Flag == false).ToListAsync();
                    stu.registrationList = allRegStudent.ToArray();

                    if (stu.registrationList.Length > 0)
                    {
                        stu.classcategoryList = (from m in _db.Masterclasscategory
                                                 from c in _db.mastercategory
                                                 from o in _db.AcademicYear
                                                 from p in _db.School_M_Class
                                                 where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && p.ASMCL_Id == allRegStudent[0].ASMCL_Id && m.ASMCL_Id == allRegStudent[0].ASMCL_Id && o.ASMAY_Id == stu.ASMAY_Id && p.MI_Id == stu.MI_Id)
                                                 select new StudentApplicationDTO
                                                 {
                                                     applicationhtml = c.AMC_PAApplicationName,
                                                     asmcl_id = p.ASMCL_Id,
                                                     AMC_Id = c.AMC_Id
                                                 }).ToArray();
                    }

                    stu.registrationListhealth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                  from b in _StudentApplicationContext.Enq
                                                  where (a.PASR_Id == b.pasr_id && b.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.Id == stu.Id && b.PASR_Adm_Confirm_Flag == false && a.ASMAY_Id == stu.ASMAY_Id)
                                                  select b
                      ).ToList().ToArray();


                }

                stu.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R").ToArray();

                // 30-9-2016
                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = await _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToListAsync();
                stu.mstConfig = mstConfig.ToArray();


                List<PA_School_Application_ProspectusDMO> allRegStudentnow = new List<PA_School_Application_ProspectusDMO>();
                allRegStudentnow = _db.PA_School_Application_ProspectusDMO.ToList();
                stu.studentDetailsTEmp = allRegStudentnow.ToArray();

                List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                saa = _db.MasterAreaDMO.Where(r => r.MI_Id == stu.MI_Id).ToList();
                stu.areaList = saa.ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                {
                    temparr.Add(allRegStudentnow[i].PASP_Id);
                }



                //srkvs deepak
                if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 1)
                {
                    stu.prospectuslist = (from a in _db.prospectus
                                          from b in _db.Payment_PA_ProspectusDMO
                                          where (a.PASP_Id == b.PASP_Id && !temparr.Contains(a.PASP_Id))
                                          select a
                        ).ToList().ToArray();
                }
                //end srkvs deepak




                stu.caste_doc_maplist = _StudentApplicationContext.Preadmission_Cast_Doc_MappingDMO.Where(t => t.MI_Id == stu.MI_Id).ToList().Distinct().ToArray();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                //*****MAXMINAGE****
                //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
                stu.admissioncatdrp = allclass.ToArray();

                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                stu.admissioncatdrpall = allclass.ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == stu.MI_Id).ToList();
                stu.statuslist = status.ToArray();

                await Getcountofstudents(stu);



            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            return stu;
        }

        public async Task<CountryDTO> Getcountofstudents(CountryDTO stu)
        {

            // Student Roles
            string studentRole = "OnlinePreadmissionUser";
            var id = _db.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "totalcountReport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RoleId",
                    SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt32(id.Id)
                });
                cmd.Parameters.Add(new SqlParameter("@RoleTypeId",
                   SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(id2.IVRMRT_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@year",
            SqlDbType.BigInt)
                {
                    Value = stu.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
         SqlDbType.VarChar)
                {
                    Value = stu.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@predate",
            SqlDbType.DateTime)
                {
                    Value = stu.ASMAY_PreAdm_F_Date
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //  var data = cmd.ExecuteNonQuery();

                try
                {
                    //   var data = cmd.ExecuteNonQuery();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    stu.totalcountDetails = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            }

            return stu;
        }

        public async Task<StudentApplicationDTO> GetSubjectsofinstitute(StudentApplicationDTO stu)
        {

            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


            stu.ASMAY_Id = Acdemic_preadmission;
            //
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Exam_Preadmission_Group_SubjectList ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(stu.MI_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMCL_Id)
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObjectz = new List<dynamic>();
                //  var data = cmd.ExecuteNonQuery();

                try
                {
                    //   var data = cmd.ExecuteNonQuery();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }

                            retObjectz.Add((ExpandoObject)dataRow);
                        }
                    }
                    stu.electivegrouplist = retObjectz.ToArray();
                }
                catch (Exception ex)
                {

                }
            }



            //
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Exam_Preadmission_GroupList  ";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(stu.MI_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMCL_Id)
                });

                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();
                //  var data = cmd1.ExecuteNonQuery();

                try
                {
                    //   var data = cmd1.ExecuteNonQuery();

                    using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                    {
                        while (await dataReader1.ReadAsync())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                            {
                                var datatype1 = dataReader1.GetFieldType(iFiled1);
                                if (datatype1.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader1[iFiled1]).Date).ToString("d");
                                    dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? "Not Available" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow1.Add(
                                   dataReader1.GetName(iFiled1),
                                   dataReader1.IsDBNull(iFiled1) ? "Not Available" : dataReader1[iFiled1] // use null instead of {}
                               );
                                }
                            }

                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    stu.electivesubgrouplist = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            }

            return stu;
        }

        public StudentApplicationDTO GetSubjects(StudentApplicationDTO stu)
        {
            //
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Exam_Preadmission_Group_SubjectList ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(stu.MI_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMCL_Id)
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObjectz = new List<dynamic>();
                //  var data = cmd.ExecuteNonQuery();

                try
                {
                    //   var data = cmd.ExecuteNonQuery();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }

                            retObjectz.Add((ExpandoObject)dataRow);
                        }
                    }
                    stu.electivegrouplist = retObjectz.ToArray();
                }
                catch (Exception ex)
                {

                }
            }



            //
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Exam_Preadmission_GroupList  ";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToString(stu.MI_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMCL_Id)
                });

                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();
                //  var data = cmd1.ExecuteNonQuery();

                try
                {
                    //   var data = cmd1.ExecuteNonQuery();

                    using (var dataReader1 = cmd1.ExecuteReader())
                    {
                        while (dataReader1.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                            {
                                var datatype1 = dataReader1.GetFieldType(iFiled1);
                                if (datatype1.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader1[iFiled1]).Date).ToString("d");
                                    dataRow1.Add(
                                    dataReader1.GetName(iFiled1),
                                    dataReader1.IsDBNull(iFiled1) ? "Not Available" : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow1.Add(
                                   dataReader1.GetName(iFiled1),
                                   dataReader1.IsDBNull(iFiled1) ? "Not Available" : dataReader1[iFiled1] // use null instead of {}
                               );
                                }
                            }

                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    stu.electivesubgrouplist = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            }

            return stu;
        }


        public async Task<CountryDTO> Dashboarddetails(CountryDTO stu)
        {
            try
            {


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                stu.ASMAY_Id = Acdemic_preadmission;



                List<MasterAcademic> allyear = new List<MasterAcademic>();
                List<MasterAcademic> allyearget = new List<MasterAcademic>();
                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == stu.roleId).IVRMRT_Role;

                if (rolename == "OnlinePreadmissionUser")
                {
                    allyearget = (from a in _feecontext.AcademicYear
                                  where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                  select new MasterAcademic
                                  {
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMAY_Year = a.ASMAY_Year
                                  }
                     ).ToList();

                    allyear = (from a in _feecontext.AcademicYear
                               where (a.MI_Id == stu.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                   ).ToList();

                    if (allyear.Count == 0)
                    {


                        List<Preadmission_Special_Registration> alllocation = new List<Preadmission_Special_Registration>();
                        alllocation = _StudentApplicationContext.Preadmission_Special_Registration.Where(t => t.ID == stu.Id).ToList();
                        if (alllocation.Count > 0)
                        {
                            allyear = (from a in _feecontext.AcademicYear
                                       where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                       select new MasterAcademic
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           ASMAY_Year = a.ASMAY_Year
                                       }
                ).ToList();
                        }
                    }

                    stu.countrole = false;


                }
                else
                {


                    allyear = (from a in _feecontext.AcademicYear
                               where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year
                               }
                   ).ToList();

                    stu.countrole = true;


                }
                stu.academicdrp = allyear.ToArray();

                if (allyear.Count > 0)
                {

                    ProspectusDTO dto = new ProspectusDTO();
                    List<ProspectusFilePath> prospectus = new List<ProspectusFilePath>();
                    prospectus = _db.ProspectusFilePath.Where(d => d.MI_ID == stu.MI_Id).ToList();
                    if (prospectus.Count > 0)
                    {
                        stu.prospectusfilePath = prospectus.FirstOrDefault().IPPC_Path;
                    }

                    stu.precutdate = "True";
                }
                else
                {
                    stu.precutdate = "false";
                }

                List<StudentApplication> allRegStudent = new List<StudentApplication>();
                allRegStudent = await _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.Id.Equals(stu.Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToListAsync();
                stu.registrationList = allRegStudent.ToArray();

                stu.registrationListhealth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                              from b in _StudentApplicationContext.Enq
                                              where (a.PASR_Id == b.pasr_id && b.MI_Id == stu.MI_Id && b.Id == stu.Id && b.ASMAY_Id == stu.ASMAY_Id)
                                              select b
                  ).ToList().ToArray();




                stu.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R").ToArray();

                // 30-9-2016
                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = await _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToListAsync();
                stu.mstConfig = mstConfig.ToArray();


                List<PA_School_Application_ProspectusDMO> allRegStudentnow = new List<PA_School_Application_ProspectusDMO>();
                allRegStudentnow = _db.PA_School_Application_ProspectusDMO.ToList();
                stu.studentDetailsTEmp = allRegStudentnow.ToArray();

                List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                saa = _db.MasterAreaDMO.Where(r => r.MI_Id == stu.MI_Id).ToList();
                stu.areaList = saa.ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                {
                    temparr.Add(allRegStudentnow[i].PASP_Id);
                }



                //srkvs deepak
                if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 1)
                {
                    stu.prospectuslist = (from a in _db.prospectus
                                          from b in _db.Payment_PA_ProspectusDMO
                                          where (a.PASP_Id == b.PASP_Id && !temparr.Contains(a.PASP_Id))
                                          select a
                        ).ToList().ToArray();
                }
                //end srkvs deepak


                stu.caste_doc_maplist = _StudentApplicationContext.Preadmission_Cast_Doc_MappingDMO.Where(t => t.MI_Id == stu.MI_Id).ToList().Distinct().ToArray();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                //*****MAXMINAGE****
                //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
                stu.admissioncatdrp = allclass.ToArray();

                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                stu.admissioncatdrpall = allclass.ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == stu.MI_Id).ToList();
                stu.statuslist = status.ToArray();

            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            return stu;
        }

        public StudentApplicationDTO getStudentEditData(StudentApplicationDTO dt)
        {

            try
            {

                var qry1 = (from rg in _StudentApplicationContext.Enq.Where(rg => rg.pasr_id == dt.pasR_Id) select rg);
                var qry2 = (from st_grdn in _StudentApplicationContext.st_grdn.Where(st_grdn => st_grdn.PASR_Id == dt.pasR_Id) select st_grdn);
                var qry3 = (from prevsch in _StudentApplicationContext.stprev.Where(prevsch => prevsch.PASR_Id == dt.pasR_Id) select prevsch);
                var qry4 = (from sblng in _StudentApplicationContext.stsblng.Where(sblng => sblng.PASR_Id == dt.pasR_Id) select sblng);
                //var qry10 = (from sblng in _StudentApplicationContext.PA_School_Application_ElectiveSujects.Where(sblng => sblng.PASR_Id == dt.pasR_Id) select sblng);
                var qry5 = (from trns in _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Where(trns => trns.PASR_Id == dt.pasR_Id) select trns);

                var qry6 = (from trxDoc in _StudentApplicationContext.trxDoc.Where(trns => trns.PASR_Id == dt.pasR_Id) select trxDoc);




                dt.StudentReg_DTObj = qry1.ToArray();
                dt.StudentGuardian_DTObj = qry2.ToArray();
                dt.StudentPrevSch_DTObj = qry3.ToArray();
                dt.StudentSbling_DTObj = qry4.ToArray();

                dt.StudentTrns_DTObj = qry5.ToArray();







                if (dt.StudentTrns_DTObj.Length > 0)
                {
                    dt.routelist = (
                                     from b in _StudentApplicationContext.MasterRouteDMO
                                     where (b.TRMR_Id == qry5.FirstOrDefault().PASTA_PickUp_TRMR_Id)
                                     select new StudentBuspassFormDTO
                                     {
                                         TRMR_Id = b.TRMR_Id,
                                         TRMR_RouteName = b.TRMR_RouteName
                                     }
               ).ToList().ToArray();

                    dt.locationlist = (
                                         from c in _StudentApplicationContext.MasterLocationDMO
                                         where (c.TRML_Id == qry5.FirstOrDefault().PASTA_PickUp_TRML_Id)
                                         select new StudentBuspassFormDTO
                                         {
                                             TRML_Id = c.TRML_Id,
                                             TRML_LocationName = c.TRML_LocationName
                                         }
                ).ToList().ToArray();


                }


                dt.studentDocuments_DTObj = qry6.ToArray();

                var currentasmsy = qry1.FirstOrDefault().ASMAY_Id;
                dt.ASMCL_Id = qry1.FirstOrDefault().ASMCL_Id;
                dt.ASMAY_Id = qry1.FirstOrDefault().ASMAY_Id;

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(dt.MI_Id) && d.ASMAY_Id.Equals(currentasmsy)).ToList();


                //srkvs deepak
                if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 1)
                {
                    dt.prospectuslist = _db.prospectus.Where(d => d.MI_ID.Equals(dt.MI_Id) && d.ASMAY_Id.Equals(currentasmsy)).ToList().ToArray();

                    //srkvs deepak
                    var PASP_ProspectusNo = qry1.FirstOrDefault().PASR_RegistrationNo;
                    var PASP_Id = (from a in _db.PA_School_Application_ProspectusDMO

                                   where (a.PASR_Id == dt.pasR_Id)
                                   select a.PASP_Id).ToList();
                    if (PASP_Id.Count > 0)
                    {
                        dt.PASP_Id = Convert.ToInt64(PASP_Id.FirstOrDefault());
                    }
                }
                //end srkvs deepak

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = (from a in _feecontext.AcademicYear
                           where (a.ASMAY_Id == currentasmsy)
                           select new MasterAcademic
                           {
                               ASMAY_Id = a.ASMAY_Id,
                               ASMAY_Year = a.ASMAY_Year
                           }
                 ).ToList();
                dt.academicdrp = allyear.ToArray();

                dt.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == dt.pasR_Id).ToArray();
                if (dt.prospectusPaymentlist.Length > 0)
                {
                    dt.editflag = "True";
                }
                else
                {
                    dt.editflag = "False";
                }

                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == dt.roleid).IVRMRT_Role;

                if (rolename == "OnlinePreadmissionUser")
                {
                    dt.roletypefind = false;
                }
                else
                {
                    dt.roletypefind = true;
                }

                //if(dt.StudentSubjects_DTObj.Length>0)
                //{

                //}

                var qry10 = (from Docs in _StudentApplicationContext.PA_School_Application_ElectiveSujects
                             from mDocs in _StudentApplicationContext.Enq
                             from subjects in _StudentApplicationContext.MasterSubjectDMO
                             from groupid in _StudentApplicationContext.Exm_Master_GroupDMO
                             from groupmap in _StudentApplicationContext.Exm_Master_Group_SubjectsDMO
                             where (Docs.PASR_Id == mDocs.pasr_id && Docs.ISMS_Id == subjects.ISMS_Id && subjects.ISMS_Id == groupmap.ISMS_Id && groupmap.EMG_Id == groupid.EMG_Id && Docs.PASR_Id == dt.pasR_Id)
                             select new Preadmissionelectives
                             {
                                 EMG_Id = groupmap.EMG_Id,
                                 ismS_Id = Convert.ToInt16(Docs.ISMS_Id),
                                 EMG_MinAplSubjects = groupid.EMG_MinAplSubjects,
                                 EMG_MaxAplSubjects = groupid.EMG_MaxAplSubjects,
                                 EMG_GroupName = groupid.EMG_GroupName
                             });
                dt.StudentSubjects_DTObj = qry10.ToArray();

                GetSubjects(dt);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;
        }

        public StudentApplicationDTO paynow(StudentApplicationDTO dt)
        {

            try
            {
                //List< StudentApplicationDTO> numberring = new List<StudentApplicationDTO>();

                // int paymentdetails = _ProspectusContext.enquiry.Where(t => t.ASMAY_Id == dt.ASMAY_Id && t.MI_Id == dt.MI_Id).FirstOrDefault().ISPAC_ApplFeeFlag;

                // List<Master_Numbering> masnum = new List<Master_Numbering>();

                // masnum = _db.Master_Numbering.Where(t => t.MI_Id == dt.MI_Id && t.IMN_Flag.Equals("Online")).ToList();

                //     dt.transnumbconfigurationsettingsss = masnum;
                //  dt.transnumbconfigurationsettingsss = masnum;

                var alreadyExistEmailId = _StudentApplicationContext.Enq.Where(d => d.pasr_id == dt.pasR_Id).ToList();
                var stuid = _StudentApplicationContext.Enq.Single(d => d.pasr_id == dt.pasR_Id).ASMAY_Id;

                dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASMCL_Id;
                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASMAY_Id;
                dt.PASR_FirstName = alreadyExistEmailId.FirstOrDefault().PASR_FirstName;
                dt.PASR_FirstName = ((alreadyExistEmailId.FirstOrDefault().PASR_FirstName == null || alreadyExistEmailId.FirstOrDefault().PASR_FirstName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PASR_FirstName) + " " + (alreadyExistEmailId.FirstOrDefault().PASR_MiddleName == null || alreadyExistEmailId.FirstOrDefault().PASR_MiddleName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PASR_MiddleName) + " " + (alreadyExistEmailId.FirstOrDefault().PASR_LastName == null || alreadyExistEmailId.FirstOrDefault().PASR_LastName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PASR_LastName)).Trim();
                dt.PASR_emailId = alreadyExistEmailId.FirstOrDefault().PASR_emailId;
                dt.PASR_MobileNo = alreadyExistEmailId.FirstOrDefault().PASR_MobileNo;


                if (dt.configurationsettings.ISPAC_ApplFeeFlag == 1)
                {
                    dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == dt.pasR_Id).Count();

                    if (dt.payementcheck == 0)
                    {
                        dt.paydet = paymentPart(dt);
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;
        }
        public StudentApplicationDTO getstudentprintData(StudentApplicationDTO dt)
        {

            try
            {


                var qry1 = (from rg in _StudentApplicationContext.Enq.Where(rg => rg.pasr_id == dt.pasR_Id) select rg);
                var qry2 = (from st_grdn in _StudentApplicationContext.st_grdn.Where(st_grdn => st_grdn.PASR_Id == dt.pasR_Id) select st_grdn);
                var qry3 = (from prevsch in _StudentApplicationContext.stprev.Where(prevsch => prevsch.PASR_Id == dt.pasR_Id) select prevsch);
                var qry4 = (from sblng in _StudentApplicationContext.stsblng.Where(sblng => sblng.PASR_Id == dt.pasR_Id) select sblng);
                var qry5 = (from trns in _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Where(trns => trns.PASR_Id == dt.pasR_Id) select trns);


                // var qry6 = (from Docs in _StudentApplicationContext.trxDoc.Where(Doc => Doc.PASR_Id == dt.pasR_Id) select Docs);


                var qry6 = (from Docs in _StudentApplicationContext.trxDoc
                            from mDocs in _StudentApplicationContext.MasterDocumentDMO
                            where (mDocs.AMSMD_Id == Docs.AMSMD_Id && Docs.PASR_Id == dt.pasR_Id)
                            select new PreadmissionSchoolRegistrationDocumentsDTO
                            {
                                AMSMD_Id = mDocs.AMSMD_Id,
                                Document_Path = Docs.Document_Path,
                                PASRD_Id = Docs.PASRD_Id,
                                PASR_Id = Docs.PASR_Id,
                                AMSMD_DocumentName = mDocs.AMSMD_DocumentName,
                                AMSMD_FLAG = mDocs.AMSMD_FLAG
                            });

                dt.StudentReg_DTObj = qry1.ToArray();
                dt.StudentGuardian_DTObj = qry2.ToArray();
                dt.StudentPrevSch_DTObj = qry3.ToArray();
                dt.StudentSbling_DTObj = qry4.ToArray();
                dt.StudentTrns_DTObj = qry5.ToArray();

                dt.studentDocuments_DTObj = qry6.ToArray();
                dt.ASMCL_Id = qry1.FirstOrDefault().ASMCL_Id;

                var asmayid = (from a in _StudentApplicationContext.Enq
                               where (a.pasr_id == dt.pasR_Id)
                               select new StudentApplicationDTO
                               {
                                   ASMAYid = a.ASMAY_Id
                               }).ToArray();


                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id.Equals(dt.MI_Id) && t.ASMAY_Id.Equals(asmayid[0].ASMAYid)).ToList();
                dt.academicdrp = allyear.ToArray();



                dt.studentClass = (from a in _StudentApplicationContext.Enq
                                   from b in _StudentApplicationContext.AdmissionClass
                                   where (a.ASMCL_Id == b.ASMCL_Id && a.pasr_id == dt.pasR_Id)
                                   select new StudentApplicationDTO
                                   {
                                       ASMCL_ClassName = b.ASMCL_ClassName
                                   }).ToArray();

                dt.studentReligion = (from a in _StudentApplicationContext.Enq
                                      from c in _StudentApplicationContext.religion
                                      where (a.Religion_Id == c.IVRMMR_Id && a.pasr_id == dt.pasR_Id)
                                      select new StudentApplicationDTO
                                      {
                                          IVRMMR_Name = c.IVRMMR_Name
                                      }).ToArray();

                dt.studentcastecategory = (from a in _StudentApplicationContext.Enq
                                           from e in _StudentApplicationContext.castecategory
                                           where (a.CasteCategory_Id == e.IMCC_Id && a.pasr_id == dt.pasR_Id)
                                           select new StudentApplicationDTO
                                           {
                                               IMCC_CategoryName = e.IMCC_CategoryName
                                           }).ToArray();

                dt.fathercaste = (from a in _StudentApplicationContext.Enq
                                  from d in _StudentApplicationContext.caste
                                  where (a.PASR_FatherCaste == d.IMC_Id && a.pasr_id == dt.pasR_Id)
                                  select new StudentApplicationDTO
                                  {
                                      IMC_CasteName = d.IMC_CasteName
                                  }).ToArray();

                //dt.fathersubcaste = (from a in _StudentApplicationContext.Enq
                //                  from d in _StudentApplicationContext.subcaste
                //                  where (a.PASR_Fathersubcaste == d.IMSC_ID && a.pasr_id == dt.pasR_Id)
                //                  select new StudentApplicationDTO
                //                  {
                //                      IMC_CasteName = d.IMSC_Caste_Name
                //                  }).ToArray();

                //dt.mothersubcaste = (from a in _StudentApplicationContext.Enq
                //                  from d in _StudentApplicationContext.subcaste
                //                  where (a.PASR_Mothersubcatse == d.IMSC_ID && a.pasr_id == dt.pasR_Id)
                //                  select new StudentApplicationDTO
                //                  {
                //                      IMC_CasteName = d.IMSC_Caste_Name
                //                  }).ToArray();

                //dt.subcaste = (from a in _StudentApplicationContext.Enq
                //                  from d in _StudentApplicationContext.subcaste
                //                  where (a.PASR_Subcaste == d.IMSC_ID && a.pasr_id == dt.pasR_Id)
                //                  select new StudentApplicationDTO
                //                  {
                //                      IMC_CasteName = d.IMSC_Caste_Name
                //                  }).ToArray();

                dt.sylabusss = (from a in _StudentApplicationContext.Enq
                                from d in _StudentApplicationContext.Pre_Adm_Syllabus
                                where (a.PASL_ID == d.PASL_ID && a.pasr_id == dt.pasR_Id)
                                select new StudentApplicationDTO
                                {
                                    IMC_CasteName = d.PASL_Name
                                }).ToArray();

                dt.motherreligion = (from a in _StudentApplicationContext.Enq
                                     from d in _StudentApplicationContext.religion
                                     where (a.PASR_MotherReligion == d.IVRMMR_Id && a.pasr_id == dt.pasR_Id)
                                     select new StudentApplicationDTO
                                     {
                                         IMC_CasteName = d.IVRMMR_Name
                                     }).ToArray();

                dt.fatherreligion = (from a in _StudentApplicationContext.Enq
                                     from d in _StudentApplicationContext.religion
                                     where (a.PASR_FatherReligion == d.IVRMMR_Id && a.pasr_id == dt.pasR_Id)
                                     select new StudentApplicationDTO
                                     {
                                         IMC_CasteName = d.IVRMMR_Name
                                     }).ToArray();

                dt.mothercaste = (from a in _StudentApplicationContext.Enq
                                  from d in _StudentApplicationContext.caste
                                  where (a.PASR_MotherCaste == d.IMC_Id && a.pasr_id == dt.pasR_Id)
                                  select new StudentApplicationDTO
                                  {
                                      IMC_CasteName = d.IMC_CasteName
                                  }).ToArray();

                dt.studentcaste = (from a in _StudentApplicationContext.Enq
                                   from d in _StudentApplicationContext.caste
                                   where (a.Caste_Id == d.IMC_Id && a.pasr_id == dt.pasR_Id)
                                   select new StudentApplicationDTO
                                   {
                                       IMC_CasteName = d.IMC_CasteName
                                   }).ToArray();



                dt.studentperstate = (from a in _StudentApplicationContext.Enq
                                      from b in _StudentApplicationContext.state
                                      where (a.PASR_PerState == Convert.ToString(b.IVRMMS_Id) && a.pasr_id == dt.pasR_Id)
                                      select new StudentApplicationDTO
                                      {
                                          PASR_PerStaten = b.IVRMMS_Name
                                      }).ToArray();

                dt.studentpercountry = (from a in _StudentApplicationContext.Enq
                                        from b in _StudentApplicationContext.country
                                        where (a.PASR_PerCountry == Convert.ToString(b.IVRMMC_Id) && a.pasr_id == dt.pasR_Id)
                                        select new StudentApplicationDTO
                                        {
                                            PASR_PerCountryn = b.IVRMMC_CountryName
                                        }).ToArray();
                dt.studentconstate = (from a in _StudentApplicationContext.Enq
                                      from b in _StudentApplicationContext.state
                                      where (a.PASR_ConState == b.IVRMMS_Id && a.pasr_id == dt.pasR_Id)
                                      select new StudentApplicationDTO
                                      {
                                          PASR_ConStaten = b.IVRMMS_Name
                                      }).ToArray();
                dt.studentconcountry = (from a in _StudentApplicationContext.Enq
                                        from b in _StudentApplicationContext.country
                                        where (a.PASR_ConCountry == Convert.ToString(b.IVRMMC_Id) && a.pasr_id == dt.pasR_Id)
                                        select new StudentApplicationDTO
                                        {
                                            PASR_ConCountryn = b.IVRMMC_CountryName
                                        }).ToArray();

                dt.studentnationalitys = (from a in _StudentApplicationContext.Enq
                                          from b in _StudentApplicationContext.country
                                          where (a.PASR_Nationality == Convert.ToString(b.IVRMMC_Id) && a.pasr_id == dt.pasR_Id)
                                          select new StudentApplicationDTO
                                          {
                                              studentnationality = b.IVRMMC_Nationality
                                          }).ToArray();
                dt.fathernationalitys = (from a in _StudentApplicationContext.Enq
                                         from b in _StudentApplicationContext.country
                                         where (a.PASR_FatherNationality == b.IVRMMC_Id && a.pasr_id == dt.pasR_Id)
                                         select new StudentApplicationDTO
                                         {
                                             fathernationality = b.IVRMMC_Nationality
                                         }).ToArray();

                dt.mothernationalitys = (from a in _StudentApplicationContext.Enq
                                         from b in _StudentApplicationContext.country
                                         where (a.PASR_MotherNationality == b.IVRMMC_Id && a.pasr_id == dt.pasR_Id)
                                         select new StudentApplicationDTO
                                         {
                                             mothernationality = b.IVRMMC_Nationality

                                         }).ToArray();




                dt.concessioncategory = (from a in _StudentApplicationContext.Enq
                                         from b in _StudentApplicationContext.Fee_Master_ConcessionDMO
                                         where (b.FMCC_Id == a.FMCC_ID && a.pasr_id == dt.pasR_Id)
                                         select new StudentApplicationDTO
                                         {
                                             concessioncats = b.FMCC_ConcessionName

                                         }).ToArray();


                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == dt.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                dt.ASMAY_Id = Acdemic_preadmission;

                var helathflag = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id == dt.MI_Id && d.ASMAY_Id == dt.ASMAY_Id).Select(d => d.ISPAC_Healthapp);
                if (helathflag.FirstOrDefault() == 1)
                {
                    StudentHelthcertificateDTO health = new StudentHelthcertificateDTO();
                    health.MI_Id = dt.MI_Id;
                    health.pasr_id = dt.pasR_Id;


                    health.PASHD_Id = _StudentApplicationContext.StudentHelthcertificate.Single(d => d.PASR_Id.Equals(dt.pasR_Id)).PASHD_Id;
                    dt.studenthelthDTO = printgethelthData(health).studenthelthDTO;
                }


                var qry10 = (from Docs in _StudentApplicationContext.PA_School_Application_ElectiveSujects
                             from mDocs in _StudentApplicationContext.Enq
                             from subjects in _StudentApplicationContext.MasterSubjectDMO
                             from groupid in _StudentApplicationContext.Exm_Master_GroupDMO
                             from groupmap in _StudentApplicationContext.Exm_Master_Group_SubjectsDMO
                             where (Docs.PASR_Id == mDocs.pasr_id && Docs.ISMS_Id == subjects.ISMS_Id && subjects.ISMS_Id == groupmap.ISMS_Id && groupmap.EMG_Id == groupid.EMG_Id && Docs.PASR_Id == dt.pasR_Id)
                             select new Preadmissionelectives
                             {
                                 EMG_Id = groupmap.EMG_Id,
                                 ismS_Id = Convert.ToInt16(Docs.ISMS_Id),
                                 EMG_MinAplSubjects = groupid.EMG_MinAplSubjects,
                                 EMG_MaxAplSubjects = groupid.EMG_MaxAplSubjects,
                                 EMG_GroupName = groupid.EMG_GroupName
                             });
                dt.StudentSubjects_DTObj = qry10.ToArray();

                GetSubjects(dt);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;
        }

        public StudentApplicationDTO getdashboardpage(StudentApplicationDTO dt)
        {

            var dashboardn = _StudentApplicationContext.dashboard.Single(t => t.PAPG_MIID.Equals(dt.MI_Id) && t.PAPG_TYPE.Equals("Admissionform")).PAPG_PAGENAME;

            if (dashboardn == "" || dashboardn == null)
            {
                dt.dashboardpage = "hutchings";
            }
            else
            {
                dt.dashboardpage = dashboardn;
            }


            return dt;
        }

        public StudentApplicationDTO searchdata(StudentApplicationDTO stu)
        {
            StudentApplicationDTO searchresult = new StudentApplicationDTO();
            List<StudentApplication> qry = new List<StudentApplication>();

            try
            {
                if (stu.searchType == 1)
                {
                    qry = _StudentApplicationContext.Enq.Where(d => d.PASR_FirstName.Contains(stu.SearchData)).Select(d => d).ToList();
                }
                else if (stu.searchType == 2)
                {
                    qry = _StudentApplicationContext.Enq.Where(d => d.PASR_emailId.Contains(stu.SearchData)).Select(d => d).ToList();
                }
                else if (stu.searchType == 3)
                {
                    qry = _StudentApplicationContext.Enq.Where(d => d.PASR_FatherName.Contains(stu.SearchData)).Select(d => d).ToList();

                }
                searchresult.StudentReg_DTObj = qry.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return searchresult;
        }

        public void deleterec(int id)
        {
            try
            {
                StudentApplicationDTO stu = new StudentApplicationDTO();
                List<StudentGuardian> strgdn = _StudentApplicationContext.st_grdn.Where(st_grdn => st_grdn.PASR_Id == id).ToList();
                List<StudentPreviousSchool> stprevsch = _StudentApplicationContext.stprev.Where(prevsch => prevsch.PASR_Id == id).ToList();
                List<StudentSibling> stsblng = _StudentApplicationContext.stsblng.Where(sblng => sblng.PASR_Id == id).ToList();
                List<StudentTransport> sttrn = _StudentApplicationContext.sttrns.Where(trns => trns.PASR_Id == id).ToList();
                List<StudentTrnxDoc> trxdoc = _StudentApplicationContext.trxDoc.Where(rg => rg.PASR_Id == id).ToList();
                List<long> trxdocList = _StudentApplicationContext.trxDoc.Where(rg => rg.PASR_Id == id).Select(d => d.PASRD_Id).ToList();
                // List<StudentUploadImage> mstdoc = _StudentApplicationContext.mstDoc.Where(d=>trxdocList.Contains(d.PASMD_Id)).ToList();
                List<StudentApplication> st = _StudentApplicationContext.Enq.Where(rg => rg.pasr_id == id).ToList();

                if (strgdn.Count() > 0)
                {
                    for (int i = 0; i < strgdn.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(strgdn.ElementAt(i));
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                if (stprevsch.Count() > 0)
                {
                    for (int i = 0; i < stprevsch.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(stprevsch.ElementAt(i));
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                if (stsblng.Count() > 0)
                {
                    for (int i = 0; i < stsblng.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(stsblng.ElementAt(i));
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                if (sttrn.Count() > 0)
                {
                    for (int i = 0; i < sttrn.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(sttrn.ElementAt(i));
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                if (trxdoc.Count() > 0)
                {
                    for (int i = 0; i < trxdoc.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(trxdoc.ElementAt(i));
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                //if (mstdoc.Count() > 0)
                //{
                //    for (int i = 0; i < mstdoc.Count(); i++)
                //    {
                //        _StudentApplicationContext.Remove(mstdoc.ElementAt(i));
                //        _StudentApplicationContext.SaveChanges();
                //    }
                //}
                if (st.Count() > 0)
                {
                    for (int i = 0; i < st.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(st.ElementAt(i));
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                //List<StudentApplication> allRegStudent = new List<StudentApplication>();
                //allRegStudent = (from rg in _StudentApplicationContext.Enq select rg).ToList();
                //stu.registrationList = allRegStudent.ToArray();
            }
            catch (Exception ex)
            {

            }
        }


        public CountryDTO ActivateDactivate(CountryDTO data)
        {

            try
            {
                var result = _StudentApplicationContext.Enq.Single(t => t.pasr_id == data.pasr_id);
                if (result.PASR_ActiveFlag == 1)
                {
                    result.PASR_ActiveFlag = 0;
                    result.UpdatedDate = DateTime.Now;
                    _StudentApplicationContext.Update(result);
                    _StudentApplicationContext.SaveChanges();
                    data.returnval = "true";
                }
                else
                {
                    result.PASR_ActiveFlag = 1;
                    result.UpdatedDate = DateTime.Now;
                    _StudentApplicationContext.Update(result);
                    _StudentApplicationContext.SaveChanges();
                    data.returnval = "false";
                }

                if (result != null)
                {
                    var result2 = _StudentApplicationContext.UserRoleWithInstituteDMO.Single(t => t.Id == result.Id);
                    if (result2.Activeflag == 1)
                    {
                        result2.Activeflag = 0;
                        result2.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Update(result2);
                        _StudentApplicationContext.SaveChanges();
                        data.returnval = "true";
                    }
                    else
                    {
                        result2.Activeflag = 1;
                        result2.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Update(result2);
                        _StudentApplicationContext.SaveChanges();
                        data.returnval = "false";
                    }
                }



                //var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleId).ToList();
                //data.roleName = rolelist[0].IVRMRT_Role;
                //if (rolelist[0].IVRMRT_Role.Equals("ADMIN") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR") || rolelist[0].IVRMRT_Role.Equals("Admission End User"))
                //{

                //    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                //    allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(data.MI_Id) && d.PASR_Adm_Confirm_Flag == false && d.ASMAY_Id == data.ASMAY_Id).ToList();
                //    data.registrationList = allRegStudent.ToArray();

                //    data.registrationListhealth = (from a in _StudentApplicationContext.StudentHelthcertificate
                //                                  from b in _StudentApplicationContext.Enq
                //                                  where (a.PASR_Id == b.pasr_id && b.MI_Id == data.MI_Id && b.PASR_Adm_Confirm_Flag == false)
                //                                  select b
                //      ).ToList().ToArray();



                //}
                //else
                //{

                //    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                //    allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(data.MI_Id) && d.Id.Equals(data.Id) && d.ASMAY_Id.Equals(data.ASMAY_Id) && d.PASR_Adm_Confirm_Flag == false).ToList();
                //    data.registrationList = allRegStudent.ToArray();

                //    if (data.registrationList.Length > 0)
                //    {
                //        data.classcategoryList = (from m in _db.Masterclasscategory
                //                                 from c in _db.mastercategory
                //                                 from o in _db.AcademicYear
                //                                 from p in _db.School_M_Class
                //                                 where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && p.ASMCL_Id == allRegStudent[0].ASMCL_Id && m.ASMCL_Id == allRegStudent[0].ASMCL_Id && o.ASMAY_Id == data.ASMAY_Id && p.MI_Id == data.MI_Id)
                //                                 select new StudentApplicationDTO
                //                                 {
                //                                     applicationhtml = c.AMC_PAApplicationName,
                //                                     asmcl_id = p.ASMCL_Id,
                //                                     AMC_Id = c.AMC_Id
                //                                 }).ToArray();
                //    }

                //    data.registrationListhealth = (from a in _StudentApplicationContext.StudentHelthcertificate
                //                                  from b in _StudentApplicationContext.Enq
                //                                  where (a.PASR_Id == b.pasr_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.Id == data.Id && b.PASR_Adm_Confirm_Flag == false && a.ASMAY_Id == data.ASMAY_Id)
                //                                  select b
                //      ).ToList().ToArray();


                //}


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<CityDTO> getCityByCountry(int id)
        {
            CityDTO allcity = new CityDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var ct = await _StudentApplicationContext.city.Where(d => d.IVRMMC_Id == id).ToListAsync();
                allcity.cityDrpDown = ct.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return allcity;
        }


        public async Task<StateDTO> getroutes(int id)
        {
            StateDTO data = new StateDTO();
            try
            {


                data.routelist = (from a in _StudentApplicationContext.MasterAreaDMO
                                  from b in _StudentApplicationContext.MasterRouteDMO
                                  where (a.TRMA_Id == b.TRMA_Id && a.TRMA_Id == id)
                                  select new StudentBuspassFormDTO
                                  {
                                      TRMR_Id = b.TRMR_Id,
                                      TRMR_RouteName = b.TRMR_RouteName
                                  }
                 ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StateDTO> getrouteslocation(int id)
        {
            StateDTO data = new StateDTO();
            try
            {
                data.locationlist = (from a in _StudentApplicationContext.Route_Location
                                     from b in _StudentApplicationContext.MasterRouteDMO
                                     from c in _StudentApplicationContext.MasterLocationDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && b.TRMR_Id == id)
                                     select new StudentBuspassFormDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                 ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StateDTO> getStateByCountry(int id)
        {
            StateDTO allstate = new StateDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var st = await _StudentApplicationContext.state.Where(d => d.IVRMMC_Id == id).ToListAsync();
                allstate.stateDrpDown = st.ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allstate;
        }

        public async Task<StateDTO> getdprospectusdetails(int id)
        {
            StateDTO allstate = new StateDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var st = await _db.prospectus.Where(d => d.PASP_Id == id).ToListAsync();
                allstate.prospectusdetails = st.ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allstate;
        }

        public async Task<StateDTO> getdpstatesubcatse(int id)
        {
            StateDTO allstate = new StateDTO();
            try
            {
                List<subCaste> subCaste = new List<subCaste>();
                subCaste = await _StudentApplicationContext.subcaste.Where(f => f.IMC_ID == id).ToListAsync();
                allstate.subcastedrp = subCaste.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allstate;
        }

        public async Task<StateDTO> getdpstatesubcatsefather(int id)
        {
            StateDTO allstate = new StateDTO();
            try
            {
                List<subCaste> subCaste = new List<subCaste>();
                subCaste = await _StudentApplicationContext.subcaste.Where(f => f.IMC_ID == id).ToListAsync();
                allstate.subcastedrpf = subCaste.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allstate;
        }

        public StudentApplicationDTO getapplicationhtml(StudentApplicationDTO stu)
        {
            try
            {
                //List<Adm_M_Category> Adm_M_Category = new List<Adm_M_Category>();
                //Adm_M_Category = _StudentApplicationContext.Adm_M_Category.Where(f => f.MI_Id == id).ToListAsync();
                //allstate.subcastedrpf = subCaste.ToArray();

                stu.classcategoryList = (from m in _db.Masterclasscategory
                                         from c in _db.mastercategory
                                         from o in _db.AcademicYear
                                         from p in _db.School_M_Class
                                         where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && p.ASMCL_Id == stu.ASMCL_Id && m.ASMCL_Id == stu.ASMCL_Id && o.ASMAY_Id == stu.ASMAY_Id && p.MI_Id == stu.MI_Id)
                                         select new StudentApplicationDTO
                                         {
                                             applicationhtml = c.AMC_PAApplicationName,
                                             asmcl_id = p.ASMCL_Id,
                                             AMC_Id = c.AMC_Id
                                         }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stu;
        }

        public async Task<StateDTO> getdpstatesubcatsemother(int id)
        {
            StateDTO allstate = new StateDTO();
            try
            {
                List<subCaste> subCaste = new List<subCaste>();
                subCaste = await _StudentApplicationContext.subcaste.Where(f => f.IMC_ID == id).ToListAsync();
                allstate.subcastedrpm = subCaste.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allstate;
        }

        public async Task<CityDTO> getCityByState(int id)
        {
            CityDTO allcity = new CityDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var ct = await _StudentApplicationContext.city.Where(d => d.IVRMMS_Id == id).ToListAsync();
                allcity.cityDrpDown = ct.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return allcity;
        }
        //public CountryDTO getAreaByCity(int id)
        //{
        //    CountryDTO location = new CountryDTO();
        //    location.locationdrp = _StudentApplicationContext.location.Where(d => d.CMC_Id == id).ToList().ToArray();
        //    return location;
        //}

        // Added on 9-11-2016 to create login for parents & guardian
        public async Task<bool> createLogin(regis reg, string mrole, string rtrole)
        {
            string returnData = null;
            string studentRole = null;
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _UserManager.FindByEmailAsync(reg.Email_id);
                if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
                {
                    user = new ApplicationUser { UserName = reg.username, Email = reg.Email_id };
                    //
                    var result = await _UserManager.CreateAsync(user, reg.password);
                    if (result.Succeeded)
                    {
                        try
                        {
                            SendEmail(user.UserName, user.Email);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        // Student Roles
                        studentRole = mrole;
                        var id = _db.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = rtrole;
                        var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                        //
                        //Int id2 = id2.
                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };

                        //added by 02/02/2017
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);
                        _db.SaveChanges();

                        returnData = "created";
                    }
                    else
                    {
                        returnData = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }
                else
                {
                    returnData = "Email id Already exist .. !!";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return true;
        }

        protected void SendEmail(string username, string emailId)
        {
            string body = this.PopulateBody(username, "VapsTech");
            this.SendHtmlFormattedEmail(emailId, "VapsTech", body);
        }

        private string PopulateBody(string userName, string institutionName)
        {
            string body = string.Empty;
            string[] res = System.IO.File.ReadAllLines(@"..\corewebapi18072016\wwwroot\EmailTemplate\RegistrationSuccessEmail.html");
            string str = string.Join("", res);
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            System.IO.MemoryStream stream = new MemoryStream(byteArray);

            using (StreamReader reader = new StreamReader(stream))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{InstitutionName}", institutionName);
            return body;
        }

        private void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vapstech", "vapstech123@gmail.com"));
            message.To.Add(new MailboxAddress("", recepientEmail));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note:for from email address turn on lesssecureapps.
                // for that go to the following url.
                //  http://www.google.com/settings/security/lesssecureapps

                // Note: only needed if the SMTP server requires authentication

                client.Authenticate("vapstech123@gmail.com", "vaps@123");
                client.Send(message);
                client.Disconnect(true);
            }
        }
        // Added on 9-11-2016 to create login for parents & guardian

        // Added on 19-9-2016

        public async Task<StudentApplicationDTO> studdet(StudentApplicationDTO stu)
        {
            try
            {

                //srkvs deepak
                var prospectosNumber = _db.prospectus.Where(d => d.PASP_Id == stu.PASP_Id).Select(d => d.PASP_ProspectusNo).ToList();
                stu.PASP_ProspectusNo = prospectosNumber.FirstOrDefault();
                string gjhghj = stu.PASR_Emisno;
                //  var countstu = _StudentApplicationContext.Enq.Where(t => t.ASMAY_Id == stu.ASMAY_Id && t.ASMCL_Id == stu.ASMCL_Id && t.Id == stu.Id).Count();

                AdmissionStatus AdmissionStatus = new AdmissionStatus();
                // AdmissionStatus = _StudentApplicationContext.AdmissionStatus.SingleOrDefault(d => d.MI_Id == stu.MI_Id && d.PAMS_StatusFlag.Contains("SELECT"));
                //stu.PAMS_Id = AdmissionStatus.PAMS_Id;


                var classcategory = _StudentApplicationContext.Masterclasscategory.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_Id == stu.ASMCL_Id && d.ASMAY_Id == stu.ASMAY_Id).ToList();

                stu.AMC_Id = classcategory.FirstOrDefault().AMC_Id;

                var castecat = _StudentApplicationContext.caste.Where(d => d.MI_Id == stu.MI_Id && d.IMC_Id == stu.Caste_Id).ToList();

                stu.CasteCategory_Id = castecat.FirstOrDefault().IMCC_Id;

                var helathflag = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id == stu.MI_Id && d.ASMAY_Id == stu.ASMAY_Id).Select(d => d.ISPAC_Healthapp);

                var AdmissionStatusflag = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id == stu.MI_Id && d.ASMAY_Id == stu.ASMAY_Id).Select(d => d.ISPAC_DefaultStatusFlag);
                if (AdmissionStatusflag.FirstOrDefault() == 1)
                {
                    AdmissionStatus = _StudentApplicationContext.AdmissionStatus.FirstOrDefault(d => d.MI_Id == stu.MI_Id && d.PAMST_StatusFlag.Contains("ACCEPTED"));
                    if (AdmissionStatus != null)

                    {
                        stu.PAMS_Id = AdmissionStatus.PAMST_Id;
                    }
                    else
                    {
                        stu.message = "Admission Status is not set.Kindly Contact Administrator!!!!!";
                        return stu;
                    }
                }
                else
                {
                    AdmissionStatus = _StudentApplicationContext.AdmissionStatus.FirstOrDefault(d => d.MI_Id == stu.MI_Id && d.PAMST_StatusFlag.Contains("INP"));

                    if (AdmissionStatus != null)
                    {
                        stu.PAMS_Id = AdmissionStatus.PAMST_Id;
                    }
                    else
                    {
                        stu.message = "Admission Status is not set.Kindly Contact Administrator!!!!!";
                        return stu;
                    }

                }
                //stu.PAMS_Id = stu.PAMS_Id;
                if (stu.pasR_Id == 0)
                {
                    var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleid).ToList();

                    if (rolelist[0].IVRMRT_Role.Equals("ADMIN") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR") || rolelist[0].IVRMRT_Role.Equals("Admission End User"))
                    {

                        //  stu.Id = 0;
                        //stu.PASR_DOB = Convert.ToDateTime(stu.PASR_DOB);
                        StudentApplication enq = Mapper.Map<StudentApplication>(stu);

                        long Defaultstatus = _StudentApplicationContext.AdmissionStatus.Single(d => d.PAMST_StatusFlag.Equals("INP") && d.MI_Id == stu.MI_Id).PAMST_Id;

                        enq.PASR_Date = DateTime.Now;
                        if (stu.transnumconfigsettings != null)
                        {
                            if (stu.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                stu.transnumconfigsettings.MI_Id = enq.MI_Id;
                                stu.transnumconfigsettings.ASMAY_Id = enq.ASMAY_Id;
                                enq.PASR_RegistrationNo = a.GenerateNumber(stu.transnumconfigsettings);

                            }
                            else if (stu.transnumconfigsettings.IMN_AutoManualFlag == "serial")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                stu.transnumconfigsettings.MI_Id = enq.MI_Id;
                                stu.transnumconfigsettings.ASMAY_Id = enq.ASMAY_Id;
                                enq.PASR_RegistrationNo = a.GenerateNumber(stu.transnumconfigsettings);

                            }

                            else
                            {
                                enq.PASR_RegistrationNo = stu.PASR_RegistrationNo;
                            }

                        }
                        else
                        {
                            stu.message = "Registration Number is not Generated. kindly contact Admin";
                        }

                        //srkvs
                        if (stu.PASP_ProspectusNo == null)
                        {
                            enq.PASR_RegistrationNo = enq.PASR_RegistrationNo;
                        }
                        else
                        {
                            enq.PASR_RegistrationNo = stu.PASP_ProspectusNo;
                        }




                        if (stu.manualAdmFlag.Equals("1"))
                        {
                            var isduplicateAdmNo = _StudentApplicationContext.Enq.Where(d => d.MI_Id == stu.MI_Id && d.ASMAY_Id == stu.ASMAY_Id && d.pasr_id != stu.pasR_Id && d.PASR_Applicationno.Equals(stu.ApplicationNo.Trim())).ToList();
                            if (isduplicateAdmNo.Count > 0)
                            {
                                stu.message = "Application no. already exist.Please enter different one";
                                return stu;
                            }
                            enq.PASR_Applicationno = stu.ApplicationNo.Trim();
                        }
                        else
                        {
                            enq.PASR_Applicationno = enq.PASR_RegistrationNo;
                        }

                        // enq.PASR_RegistrationNo = GenerateRegistrationNumber(enq.MI_Id,enq.ASMAY_Id);     //based on master setting generate registration number

                        enq.PAMS_Id = Defaultstatus;


                        //if (AdmissionStatusflag.FirstOrDefault() != 1)
                        //{
                        //    enq.PASRAPS_ID = 787926;
                        //}

                        enq.PASR_DOBWords = stu.PASR_DOBWords;
                        enq.PASR_ActiveFlag = 1;
                        //added by 02/02/2017

                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Add(enq);
                        _StudentApplicationContext.SaveChanges();
                        stu.pasR_Id = enq.pasr_id;

                        PreadmissionSchoolRegistrationStudentLogin MM2 = new PreadmissionSchoolRegistrationStudentLogin();

                        MM2.PASR_Id = stu.pasR_Id;
                        MM2.Preadmission_School_Registration_PASR_Id = stu.pasR_Id;
                        MM2.IVRMOUL_Id = stu.Id;
                        MM2.IVRM_Others_User_Login_IVRMOUL_Id = stu.Id;
                        MM2.PASRSTUL_Date = DateTime.Now;
                        MM2.PASRSTUL_EntryType = Convert.ToString(DateTime.Now);
                        MM2.CreatedDate = DateTime.Now;
                        MM2.UpdatedDate = DateTime.Now;
                        MM2.PASRSTUL_MAACAdd = stu.PASRSTUL_MAACAdd;
                        MM2.PASRSTUL_NetIp = stu.PASRSTUL_NetIp;
                        MM2.PASRSTUL_IPAdd = "";
                        _StudentApplicationContext.Add(MM2);
                        _StudentApplicationContext.SaveChanges();



                        List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                        mstConfig = await _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToListAsync();
                        stu.mstConfig = mstConfig.ToArray();

                        //srkvs deepak
                        if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 1)
                        {
                            PA_School_Application_ProspectusDMO prosp = new PA_School_Application_ProspectusDMO();

                            prosp.PASR_Id = stu.pasR_Id;
                            prosp.PASP_Id = stu.PASP_Id;

                            _StudentApplicationContext.Add(prosp);
                            _StudentApplicationContext.SaveChanges();
                        }


                        if (stu.Usercreatonflag.Equals("1"))
                        {
                            //Creating user credential code starts. Added on 02-02-2018.
                            var duplicatecheck = _StudentApplicationContext.Enq.Where(d => d.MI_Id == stu.MI_Id && d.PASR_emailId.Equals(stu.PASR_emailId) && d.PASR_MobileNo == stu.PASR_MobileNo).ToList();
                            if (duplicatecheck.Count == 1)
                            {
                                if (stu.pasR_Id > 0)
                                {
                                    string errmsg = "";
                                    string usrname = "";
                                    string uname = "";
                                    if (stu.PASR_FirstName != null && stu.PASR_MiddleName != null)
                                    {
                                        usrname = stu.PASR_FirstName + stu.PASR_MiddleName;
                                    }
                                    else if (stu.PASR_FirstName != null && stu.PASR_LastName != null)
                                    {
                                        usrname = stu.PASR_FirstName + stu.PASR_LastName;
                                    }
                                    else
                                    {
                                        usrname = stu.PASR_FirstName;
                                    }
                                    if (usrname.Length == 3)
                                    {
                                        usrname = usrname.Substring(0, 3) + stu.pasR_Id;
                                    }
                                    else if (usrname.Length == 2)
                                    {
                                        usrname = usrname.Substring(0, 2) + stu.pasR_Id;
                                    }
                                    else if (usrname.Length == 1)
                                    {
                                        usrname = usrname.Substring(0, 1) + usrname.Substring(0, 1) + stu.pasR_Id;
                                    }

                                    try
                                    {
                                        ApplicationUser user = new ApplicationUser();
                                        user = await _UserManager.FindByNameAsync(usrname);
                                        if (user == null)
                                        {
                                            user = new ApplicationUser { UserName = usrname, Email = stu.PASR_emailId, PhoneNumber = stu.PASR_MobileNo.ToString() };
                                            user.Entry_Date = DateTime.Now;
                                            user.EmailConfirmed = true;
                                            user.Name = stu.PASR_FirstName;
                                            user.CreatedDate = DateTime.Now;
                                            user.UpdatedDate = DateTime.Now;
                                            var result = await _UserManager.CreateAsync(user, "Password@123");
                                            if (result.Succeeded)
                                            {
                                                //get Role.
                                                string studentRole = "OnlinePreadmissionUser";
                                                var id = _db.applicationRole.Single(d => d.Name == studentRole);
                                                //

                                                //get Role Type.
                                                string studentRoleType = "OnlinePreadmissionUser";
                                                var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                                                //

                                                // Save application role.
                                                var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                                                role.CreatedDate = DateTime.Now;
                                                role.UpdatedDate = DateTime.Now;
                                                _db.appUserRole.Add(role);
                                                _db.SaveChanges();

                                                // Save User Login Institutionwise.
                                                UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                                                mas1.Id = user.Id;
                                                mas1.MI_Id = stu.MI_Id;
                                                mas1.Activeflag = 1;
                                                mas1.CreatedDate = DateTime.Now;
                                                mas1.UpdatedDate = DateTime.Now;
                                                _db.Add(mas1);
                                                _db.SaveChanges();
                                                if (user.Id > 0)
                                                {
                                                    var query = _StudentApplicationContext.Enq.Single(d => d.pasr_id == stu.pasR_Id);
                                                    query.Id = user.Id;
                                                    _StudentApplicationContext.Update(query);
                                                    var count = _StudentApplicationContext.SaveChanges();
                                                    if (count > 0)
                                                    {
                                                        if (stu.PASR_MobileNo.ToString() != null && stu.PASR_MobileNo.ToString() != "")
                                                        {
                                                            SMS sms = new SMS(_db);
                                                            string sm = sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "StaffUserCreation", user.Id).Result;
                                                        }

                                                        Email Email = new Email(_db);
                                                        string s = Email.sendmail(stu.MI_Id, stu.PASR_emailId, "StaffUserCreation", user.Id);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                errmsg = result.Errors.FirstOrDefault().Description.ToString();
                                            }
                                        }
                                        else
                                        {
                                            stu.message = "User already exist";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        Console.WriteLine(errmsg);
                                    }

                                }
                            }
                            else
                            {
                                var query = _StudentApplicationContext.Enq.Single(d => d.pasr_id == stu.pasR_Id);
                                query.Id = duplicatecheck.FirstOrDefault().Id;
                                _StudentApplicationContext.Update(query);
                                var count = _StudentApplicationContext.SaveChanges();
                            }
                        }

                        Studentpointssaveupdate(stu);

                        if (enq.PASR_emailId != "" || enq.PASR_emailId != null)
                        {
                            using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "PreUser_Email_Update";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                                {
                                    Value = Convert.ToString(enq.PASR_emailId)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(stu.pasR_Id)
                                });

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
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                        if (enq.PASR_MobileNo != 0)
                        {
                            using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "PreUser_Mobile_Update";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar)
                                {
                                    Value = Convert.ToString(enq.PASR_MobileNo)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(stu.pasR_Id)
                                });

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
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                        //if (enq.PASR_Student_Pic_Path != "" || enq.PASR_Student_Pic_Path != null)
                        //{
                        //    using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                        //    {
                        //        cmd.CommandText = "PreUser_Photo_Update";
                        //        cmd.CommandType = CommandType.StoredProcedure;
                        //        cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar)
                        //        {
                        //            Value = Convert.ToString(enq.PASR_Student_Pic_Path)
                        //        });

                        //        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                        //        {
                        //            Value = Convert.ToInt64(stu.pasR_Id)
                        //        });

                        //        if (cmd.Connection.State != ConnectionState.Open)
                        //            cmd.Connection.Open();

                        //        var retObject = new List<dynamic>();
                        //        try
                        //        {
                        //            using (var dataReader = cmd.ExecuteReader())
                        //            {
                        //                while (dataReader.Read())
                        //                {
                        //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //                    {
                        //                        dataRow.Add(
                        //                            dataReader.GetName(iFiled),
                        //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                        //                        );
                        //                    }

                        //                    retObject.Add((ExpandoObject)dataRow);
                        //                }
                        //            }
                        //            // retObject.Add((ExpandoObject)dataRow);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine(ex.Message);
                        //        }
                        //    }
                        //}

                        stu.message = "Successfully Submitted the Application!!";

                        stu.healthflag = true;

                    }
                    else
                    {
                        int countstu = _StudentApplicationContext.Enq.Where(t => t.ASMAY_Id == stu.ASMAY_Id && t.ASMCL_Id == stu.ASMCL_Id && t.Id == stu.Id).Count();
                        if (countstu < stu.configurationsettings.ISPAC_NoofApplications)
                        {
                            //stu.PASR_DOB = Convert.ToDateTime(stu.PASR_DOB);
                            StudentApplication enq = Mapper.Map<StudentApplication>(stu);

                            long Defaultstatus = _StudentApplicationContext.AdmissionStatus.Single(d => d.PAMST_StatusFlag.Equals("INP") && d.MI_Id == stu.MI_Id).PAMST_Id;

                            enq.PASR_Date = DateTime.Now;




                            if (stu.transnumconfigsettings != null)
                            {
                                if (stu.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                                {
                                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                    stu.transnumconfigsettings.MI_Id = enq.MI_Id;
                                    stu.transnumconfigsettings.ASMAY_Id = enq.ASMAY_Id;
                                    enq.PASR_RegistrationNo = a.GenerateNumber(stu.transnumconfigsettings);

                                }
                                else if (stu.transnumconfigsettings.IMN_AutoManualFlag == "serial")
                                {
                                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                    stu.transnumconfigsettings.MI_Id = enq.MI_Id;
                                    stu.transnumconfigsettings.ASMAY_Id = enq.ASMAY_Id;
                                    enq.PASR_RegistrationNo = a.GenerateNumber(stu.transnumconfigsettings);

                                }
                                //else
                                //{
                                //    enq.PASR_RegistrationNo = stu.PASR_RegistrationNo;
                                //}



                            }
                            else
                            {
                                stu.message = "Registration Number is not Generated. kindly contact Admin";
                            }


                            //srkvs deepak
                            if (stu.PASP_ProspectusNo == null)
                            {
                                if (stu.transnumconfigsettings != null)
                                {
                                    if (stu.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                                    {
                                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                        stu.transnumconfigsettings.MI_Id = enq.MI_Id;
                                        stu.transnumconfigsettings.ASMAY_Id = enq.ASMAY_Id;
                                        enq.PASR_RegistrationNo = a.GenerateNumber(stu.transnumconfigsettings);

                                    }
                                    else if (stu.transnumconfigsettings.IMN_AutoManualFlag == "serial")
                                    {
                                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                        stu.transnumconfigsettings.MI_Id = enq.MI_Id;
                                        stu.transnumconfigsettings.ASMAY_Id = enq.ASMAY_Id;
                                        enq.PASR_RegistrationNo = a.GenerateNumber(stu.transnumconfigsettings);

                                    }
                                    else
                                    {
                                        enq.PASR_RegistrationNo = stu.PASR_RegistrationNo;
                                    }

                                }
                                else
                                {
                                    stu.message = "Registration Number is not Generated. kindly contact Admin";
                                }
                            }
                            else
                            {
                                enq.PASR_RegistrationNo = stu.PASP_ProspectusNo;
                            }

                            // enq.PASR_RegistrationNo = GenerateRegistrationNumber(enq.MI_Id,enq.ASMAY_Id);     //based on master setting generate registration number

                            enq.PAMS_Id = Defaultstatus;


                            //if (helathflag.FirstOrDefault() == 1)
                            //{
                            //    enq.PASRAPS_ID = 787926;
                            //}

                            enq.PASR_DOBWords = stu.PASR_DOBWords;
                            //added by 02/02/2017
                            enq.PASR_Applicationno = enq.PASR_RegistrationNo;
                            enq.CreatedDate = DateTime.Now;
                            enq.UpdatedDate = DateTime.Now;
                            _StudentApplicationContext.Add(enq);
                            _StudentApplicationContext.SaveChanges();
                            stu.pasR_Id = enq.pasr_id;

                            PreadmissionSchoolRegistrationStudentLogin MM2 = new PreadmissionSchoolRegistrationStudentLogin();

                            MM2.PASR_Id = stu.pasR_Id;
                            MM2.Preadmission_School_Registration_PASR_Id = stu.pasR_Id;
                            MM2.IVRMOUL_Id = stu.Id;
                            MM2.IVRM_Others_User_Login_IVRMOUL_Id = stu.Id;
                            MM2.PASRSTUL_Date = DateTime.Now;
                            MM2.PASRSTUL_EntryType = Convert.ToString(DateTime.Now);
                            MM2.CreatedDate = DateTime.Now;
                            MM2.UpdatedDate = DateTime.Now;
                            MM2.PASRSTUL_MAACAdd = stu.PASRSTUL_MAACAdd;
                            MM2.PASRSTUL_NetIp = stu.PASRSTUL_NetIp;
                            MM2.PASRSTUL_IPAdd = "";
                            _StudentApplicationContext.Add(MM2);
                            _StudentApplicationContext.SaveChanges();

                            Studentpointssaveupdate(stu);


                            if (enq.PASR_emailId != "" || enq.PASR_emailId != null)
                            {
                                using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "PreUser_Email_Update";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(enq.PASR_emailId)
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(stu.pasR_Id)
                                    });

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
                                        // retObject.Add((ExpandoObject)dataRow);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }

                            if (enq.PASR_MobileNo != 0)
                            {
                                using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "PreUser_Mobile_Update";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(enq.PASR_MobileNo)
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(stu.pasR_Id)
                                    });

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
                                        // retObject.Add((ExpandoObject)dataRow);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }

                            //if (enq.PASR_Student_Pic_Path != "" || enq.PASR_Student_Pic_Path != null)
                            //{
                            //    using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                            //    {
                            //        cmd.CommandText = "PreUser_Photo_Update";
                            //        cmd.CommandType = CommandType.StoredProcedure;
                            //        cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar)
                            //        {
                            //            Value = Convert.ToString(enq.PASR_Student_Pic_Path)
                            //        });

                            //        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            //        {
                            //            Value = Convert.ToInt64(stu.pasR_Id)
                            //        });

                            //        if (cmd.Connection.State != ConnectionState.Open)
                            //            cmd.Connection.Open();

                            //        var retObject = new List<dynamic>();
                            //        try
                            //        {
                            //            using (var dataReader = cmd.ExecuteReader())
                            //            {
                            //                while (dataReader.Read())
                            //                {
                            //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            //                    {
                            //                        dataRow.Add(
                            //                            dataReader.GetName(iFiled),
                            //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                            //                        );
                            //                    }

                            //                    retObject.Add((ExpandoObject)dataRow);
                            //                }
                            //            }
                            //            // retObject.Add((ExpandoObject)dataRow);
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            Console.WriteLine(ex.Message);
                            //        }
                            //    }
                            //}

                            stu.message = "Successfully Submitted the Application!!";


                        }

                        else
                        {
                            stu.message = "You have Already Submitted " + countstu.ToString() + " Form for this class!!";
                        }
                    }




                }
                // }
                else if (stu.pasR_Id > 0)
                {
                    var stureg = _StudentApplicationContext.Enq.Single(s => s.pasr_id == stu.pasR_Id);
                    var id = _StudentApplicationContext.Enq.Single(s => s.pasr_id == stu.pasR_Id).Id;

                    if (stu.manualAdmFlag.Equals("1"))
                    {
                        var isduplicateAdmNo = _StudentApplicationContext.Enq.Where(d => d.MI_Id == stu.MI_Id && d.ASMAY_Id == stu.ASMAY_Id && d.pasr_id != stu.pasR_Id && d.PASR_Applicationno.Equals(stu.ApplicationNo.Trim())).ToList();
                        if (isduplicateAdmNo.Count > 0)
                        {
                            stu.message = "Application no. already exist.Please enter different one";
                            return stu;
                        }
                        stu.PASR_Applicationno = stu.ApplicationNo.Trim();
                    }
                    else
                    {
                        stu.PASR_Applicationno = stu.PASR_RegistrationNo;
                    }

                    //added by 02/02/2017
                    stu.CreatedDate = stureg.CreatedDate;
                    stu.UpdatedDate = DateTime.Now;

                    stu.Id = id;
                    stu.PAMS_Id = stureg.PAMS_Id;
                    stu.PASRAPS_ID = stureg.PASRAPS_ID;
                    stu.PASR_ActiveFlag = stureg.PASR_ActiveFlag;
                    var classcategoryy = _StudentApplicationContext.Masterclasscategory.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_Id == stu.ASMCL_Id && d.ASMAY_Id == stu.ASMAY_Id).ToList();

                    stu.AMC_Id = classcategoryy.FirstOrDefault().AMC_Id;

                    var castecatt = _StudentApplicationContext.caste.Where(d => d.MI_Id == stu.MI_Id && d.IMC_Id == stu.Caste_Id).ToList();

                    stu.CasteCategory_Id = castecatt.FirstOrDefault().IMCC_Id;
                    stu.PASR_PaymentFlag = stureg.PASR_PaymentFlag;
                    stu.PASR_FinalpaymentFlag = stureg.PASR_FinalpaymentFlag;
                    stu.PASR_Adm_Confirm_Flag = stureg.PASR_ParentConcessionFlag;
                    stureg.PASR_Emisno = stu.PASR_Emisno;

                    Mapper.Map(stu, stureg);

                    _StudentApplicationContext.Update(stureg);
                    _StudentApplicationContext.SaveChanges();

                    if (stu.PASR_emailId != "" || stu.PASR_emailId != null)
                    {
                        using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "PreUser_Email_Update";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                            {
                                Value = Convert.ToString(stu.PASR_emailId)
                            });

                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(stu.pasR_Id)
                            });

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
                                // retObject.Add((ExpandoObject)dataRow);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    if (stu.PASR_MobileNo != 0)
                    {
                        using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "PreUser_Mobile_Update";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar)
                            {
                                Value = Convert.ToString(stu.PASR_MobileNo)
                            });

                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(stu.pasR_Id)
                            });

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
                                // retObject.Add((ExpandoObject)dataRow);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    //if (stu.PASR_Student_Pic_Path != "" || stu.PASR_Student_Pic_Path != null)
                    //{
                    //    using (var cmd = _StudentApplicationContext.Database.GetDbConnection().CreateCommand())
                    //    {
                    //        cmd.CommandText = "PreUser_Photo_Update";
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.Add(new SqlParameter("@photo", SqlDbType.VarChar)
                    //        {
                    //            Value = Convert.ToString(stu.PASR_Student_Pic_Path)
                    //        });

                    //        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                    //        {
                    //            Value = Convert.ToInt64(stu.pasR_Id)
                    //        });

                    //        if (cmd.Connection.State != ConnectionState.Open)
                    //            cmd.Connection.Open();

                    //        var retObject = new List<dynamic>();
                    //        try
                    //        {
                    //            using (var dataReader = cmd.ExecuteReader())
                    //            {
                    //                while (dataReader.Read())
                    //                {
                    //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                    {
                    //                        dataRow.Add(
                    //                            dataReader.GetName(iFiled),
                    //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                        );
                    //                    }

                    //                    retObject.Add((ExpandoObject)dataRow);
                    //                }
                    //            }
                    //            // retObject.Add((ExpandoObject)dataRow);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Console.WriteLine(ex.Message);
                    //        }
                    //    }
                    //}

                    stu.message = "Successfully updated";
                }

                if (stu.pasR_Id > 0)
                {
                    PreviousSchoolAddUpdate(stu);
                    StudentTransportAddUpdate(stu);
                    SiblingsAddUpdate(stu);
                    Electivesubjects(stu);
                    studentdocumentsAddUpdate(stu);
                    //  ParentsLoginCreation(stu);
                    StudentguardianAddUpdate(stu);


                    if (stu.configurationsettings.ISPAC_ApplFeeFlag == 1 && helathflag.FirstOrDefault() != 1)
                    {
                        stu.paymentapplicable = "Pay";
                        stu.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == stu.pasR_Id).Count();

                        if (stu.payementcheck == 0)
                        {
                            stu.paydet = paymentPart(stu);
                        }
                    }

                    else if (stu.configurationsettings.ISPAC_ApplFeeFlag == 0)
                    {


                        List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                        mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                        if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                        {
                            Email Email = new Email(_db);

                            Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_REGISTRATION", stu.pasR_Id);
                        }

                        if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                        {
                            SMS sms = new SMS(_db);
                            await sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_REGISTRATION", stu.pasR_Id);
                        }

                        stu.paymentapplicable = "NoPay";

                        if (helathflag.FirstOrDefault() == 1)
                        {
                            stu.Extraforms = true;
                        }
                        else
                        {
                            stu.Extraforms = false;
                        }
                    }
                    else if (stu.configurationsettings.ISPAC_ApplFeeFlag == 1 && helathflag.FirstOrDefault() == 1
                        )
                    {
                        stu.paymentapplicable = "NoPay";


                        if (helathflag.FirstOrDefault() == 1)
                        {
                            stu.Extraforms = true;
                        }
                        else
                        {
                            stu.Extraforms = false;
                        }

                    }




                }

                List<StudentApplication> allRegStudent = new List<StudentApplication>();
                allRegStudent = await _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.Id.Equals(stu.Id)).ToListAsync();
                stu.registrationList = allRegStudent.ToArray();

                if (helathflag.FirstOrDefault() == 1)
                {
                    stu.healthflag = true;
                }
                else
                {
                    stu.healthflag = false;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                stu.message = "Student not registered, Kindly contact Administrator.";
            }
            return stu;
        }
        public void studentdocumentsAddUpdate(StudentApplicationDTO stu)
        {
            try
            {
                //store student documents
                if (stu.selectedDocuments.Count() > 0)
                {
                    foreach (PreadmissionSchoolRegistrationDocumentsDTO mob in stu.selectedDocuments)
                    {
                        mob.PASR_Id = stu.pasR_Id;

                        if (mob.Document_Path != null && mob.Document_Path != "")
                        {
                            StudentTrnxDoc document = Mapper.Map<StudentTrnxDoc>(mob);
                            if (document.PASRD_Id > 0)
                            {
                                var documentNoresult = _StudentApplicationContext.trxDoc.Single(t => t.PASRD_Id == mob.PASRD_Id);
                                documentNoresult.PASRD_Id = mob.PASRD_Id;
                                documentNoresult.PASR_Id = mob.PASR_Id;
                                documentNoresult.AMSMD_Id = mob.AMSMD_Id;
                                documentNoresult.Document_Path = mob.Document_Path;


                                //added by 02/02/2017

                                documentNoresult.UpdatedDate = DateTime.Now;
                                _StudentApplicationContext.Update(documentNoresult);
                            }
                            else
                            {

                                //added by 02/02/2017
                                document.CreatedDate = DateTime.Now;
                                document.UpdatedDate = DateTime.Now;
                                _StudentApplicationContext.Add(document);
                            }
                            _StudentApplicationContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void ParentsLoginCreation(StudentApplicationDTO stu)
        {
            try
            {
                // create login for father and mother if they have email ids... on 9-11-2016
                if (stu.PASR_FatheremailId != null) // check father mail id should not be null bcoz it is used as username while login 
                {
                    // call function that generate login
                    if (stu.PASR_FatheremailId != null)
                    {
                        regis reg = new regis();
                        reg.Email_id = stu.PASR_FatheremailId;
                        reg.password = "Password@123";
                        reg.username = stu.PASR_FatheremailId;
                        await createLogin(reg, "Parents", "Parents");
                    }
                }
                if (stu.PASR_MotheremailId != null) // check mother mail id should not be null bcoz it is used as username while login
                {
                    // call function that generate login
                    if (stu.PASR_MotheremailId != null)
                    {
                        regis reg = new regis();
                        reg.Email_id = stu.PASR_MotheremailId;
                        reg.password = "Password@123";
                        reg.username = stu.PASR_MotheremailId;
                        await createLogin(reg, "Parents", "Parents");
                    }
                }
                // create login for father and mother if they have email ids... on 9-11-2016
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



        }

        public void SiblingsAddUpdate(StudentApplicationDTO stu)
        {
            try
            {
                List<long> temparr = new List<long>();


                if (stu.siblingsDetails.Count() > 0)
                {
                    //
                    foreach (StudentSiblingDTO mob in stu.siblingsDetails)
                    {
                        if (mob.PASRS_SiblingsName != null)
                            temparr.Add(mob.PASRS_Id);
                    }

                    if (temparr.Count() > 0)
                    {
                        Array siblingNoresultremove = _StudentApplicationContext.StudentSibling.Where(t => !temparr.Contains(t.PASRS_Id) && t.PASR_Id == stu.pasR_Id).ToArray();
                        foreach (StudentSibling ph1 in siblingNoresultremove)
                        {
                            _StudentApplicationContext.Remove(ph1);
                        }
                    }

                    //add & update Siblings details

                    foreach (StudentSiblingDTO mob in stu.siblingsDetails)
                    {
                        if (mob.PASRS_SiblingsName != null)
                        {
                            mob.PASR_Id = stu.pasR_Id;
                            StudentSibling sibling = Mapper.Map<StudentSibling>(mob);
                            if (sibling.PASRS_Id > 0)
                            {
                                var siblingNoresult = _StudentApplicationContext.StudentSibling.Single(t => t.PASRS_Id == mob.PASRS_Id);
                                //added by 02/02/2017

                                siblingNoresult.UpdatedDate = DateTime.Now;

                                Mapper.Map(mob, siblingNoresult);

                                _StudentApplicationContext.Update(siblingNoresult);
                            }
                            else
                            {  //added by 02/02/2017
                                sibling.CreatedDate = DateTime.Now;
                                sibling.UpdatedDate = DateTime.Now;
                                _StudentApplicationContext.Add(sibling);
                            }
                            _StudentApplicationContext.SaveChanges();
                        }

                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public void Electivesubjects(StudentApplicationDTO stu)
        {
            try
            {
                //List<long> temparr = new List<long>();


                if (stu.elesubsubject.Count() > 0)
                {

                    Array siblingNoresultremove = _StudentApplicationContext.PA_School_Application_ElectiveSujects.Where(t => t.PASR_Id == stu.pasR_Id).ToArray();
                    foreach (PA_School_Application_ElectiveSujects ph1 in siblingNoresultremove)
                    {
                        _StudentApplicationContext.Remove(ph1);
                    }


                    //add & update Siblings details

                    foreach (Preadmissionelectives mob in stu.elesubsubject)
                    {
                        if (mob.EMG_GroupName != null)
                        {

                            PA_School_Application_ElectiveSujects sibling = new PA_School_Application_ElectiveSujects();

                            sibling.PASR_Id = stu.pasR_Id;
                            sibling.ISMS_Id = Convert.ToInt64(mob.ismS_Id);
                            sibling.PASAES_ActiveFlg = true;
                            sibling.PASAES_CreatedBy = stu.Id;
                            sibling.PASAES_UpdatedBy = stu.Id;
                            sibling.PASAES_CreatedDate = DateTime.Now;
                            sibling.PASAES_UpdatedDate = DateTime.Now;
                            sibling.PASAES_TransferredToExmFlg = false;

                            _StudentApplicationContext.Add(sibling);
                            _StudentApplicationContext.SaveChanges();
                        }

                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }


        public void StudentTransportAddUpdate(StudentApplicationDTO stu)
        {

            try
            {
                // Student Transport starts
                if (stu.trmR_Idp != 0 && stu.trmL_Idp != 0)
                {
                    PA_Student_Transport_ApplicationDMO trnsp = new PA_Student_Transport_ApplicationDMO();

                    var trnspreslt = _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Where(s => s.PASR_Id == stu.pasR_Id).ToList();

                    if (trnspreslt.Count > 0)
                    {
                        var trnspresltt = _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Single(s => s.PASR_Id == stu.pasR_Id);


                        //added by 02/02/2017
                        trnspresltt.TRMA_Id = stu.trmA_Id;
                        trnspresltt.PASTA_PickUp_TRMR_Id = stu.trmR_Idp;
                        trnspresltt.PASTA_PickUp_TRML_Id = stu.trmL_Idp;
                        trnspresltt.UpdatedDate = DateTime.Now;

                        _StudentApplicationContext.Update(trnspresltt);
                        _StudentApplicationContext.SaveChanges();
                    }
                    else
                    {

                        //added by 02/02/2017
                        trnsp.TRMA_Id = stu.trmA_Id;
                        trnsp.PASTA_PickUp_TRMR_Id = stu.trmR_Idp;
                        trnsp.PASTA_PickUp_TRML_Id = stu.trmL_Idp;
                        trnsp.MI_Id = stu.MI_Id;
                        trnsp.PASR_Id = stu.pasR_Id;
                        trnsp.CreatedDate = DateTime.Now;
                        trnsp.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Add(trnsp);
                        _StudentApplicationContext.SaveChanges();
                    }
                }
                // Student Transport ends
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void PreviousSchoolAddUpdate(StudentApplicationDTO stu)
        {
            try
            {
                List<long> temparr = new List<long>();
                if (stu.PreviousSchoolList.Count() > 0)
                {
                    // Remove PreviousSchool
                    foreach (StudentPrevSchoolDTO mob in stu.PreviousSchoolList)
                    {
                        temparr.Add(mob.PASRPS_Id);
                    }

                    if (temparr.Count() > 0)
                    {
                        Array PrevSchoolNoresultremove = _StudentApplicationContext.stprev.Where(t => !temparr.Contains(t.PASRPS_Id) && t.PASR_Id == stu.pasR_Id).ToArray();
                        foreach (StudentPreviousSchool ph1 in PrevSchoolNoresultremove)
                        {
                            _StudentApplicationContext.Remove(ph1);
                        }
                    }
                    //  add & update PreviousSchool details

                    foreach (StudentPrevSchoolDTO mob in stu.PreviousSchoolList)
                    {
                        StudentPreviousSchool PrevSch = Mapper.Map<StudentPreviousSchool>(mob);
                        PrevSch.PASR_Id = stu.pasR_Id;

                        if (PrevSch.PASRPS_Id > 0)
                        {
                            var PrevSchreslt = _StudentApplicationContext.stprev.Single(s => s.PASRPS_Id == mob.PASRPS_Id);
                            Mapper.Map(mob, PrevSchreslt);

                            //added by 02/02/2017

                            PrevSchreslt.UpdatedDate = DateTime.Now;
                            _StudentApplicationContext.Update(PrevSchreslt);
                            _StudentApplicationContext.SaveChanges();
                        }
                        else
                        {

                            //added by 02/02/2017
                            PrevSch.CreatedDate = DateTime.Now;
                            PrevSch.UpdatedDate = DateTime.Now;
                            _StudentApplicationContext.Add(PrevSch);
                            _StudentApplicationContext.SaveChanges();
                        }


                    }
                }
                // Previous School ends
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public async void StudentguardianAddUpdate(StudentApplicationDTO stu)
        {

            try
            {
                // Student guardian stars
                if (stu.PASRG_GuardianName != null)
                {
                    StudentGuardian gua = Mapper.Map<StudentGuardian>(stu);
                    gua.PASR_Id = stu.pasR_Id;

                    if (stu.PASRG_Id > 0)
                    {
                        var stuguardn = _StudentApplicationContext.st_grdn.Single(g => g.PASR_Id == stu.pasR_Id);
                        Mapper.Map(stu, stuguardn);

                        //added by 02/02/2017

                        stuguardn.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Update(stuguardn);
                        _StudentApplicationContext.SaveChanges();
                    }
                    else
                    {

                        //added by 02/02/2017
                        gua.CreatedDate = DateTime.Now;
                        gua.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Add(gua);
                        _StudentApplicationContext.SaveChanges();
                        // create login for guardian... on 9-11-2016
                        if (stu.PASRG_emailid != null)
                        {
                            regis reg = new regis();
                            reg.Email_id = stu.PASRG_emailid;
                            reg.password = "Password@123";
                            reg.username = stu.PASRG_emailid;
                            await createLogin(reg, "Guardian", "Guardian");
                        }
                        // create login for guardian... on 9-11-2016
                    }
                }
                // Student guardian ends
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        public async void Studentpointssaveupdate(StudentApplicationDTO stu)
        {

            try
            {
                PointsDMO stud = new PointsDMO();
                List<PointsDMO> studentpoints = new List<PointsDMO>();
                studentpoints = _StudentApplicationContext.PointsDMO.Where(g => g.PASR_Id == stu.pasR_Id).ToList();
                int income = 0;



                int agepoints = 0;
                int castepoints = 0;

                List<MasterAcademic> result = new List<MasterAcademic>();
                result = _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_PreAdm_F_Date <= DateTime.Today && t.ASMAY_PreAdm_T_Date >= DateTime.Today && t.Is_Active == true).ToList();
                stu.fill_asy = result.ToArray();


                DateTime _cut_of_date = Convert.ToDateTime(result[0].ASMAY_Cut_Of_Date);

                List<Preadmission_App_Points_AgeDMO> allclass = new List<Preadmission_App_Points_AgeDMO>();
                allclass = _StudentApplicationContext.PAstudentage.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_Id == stu.ASMCL_Id).ToList();
                DateTime Min_date = DateTime.Today;
                DateTime Max_date = DateTime.Today;
                for (int i = 0; i < allclass.Count(); i++)
                {

                    int Min_d = allclass[i].PAAPA_Mindays;
                    int Min_m = allclass[i].PAAPA_MinMonth;
                    int Min_Y = allclass[i].PAAPA_MinAge;
                    int Max_d = allclass[i].PAAPA_Maxdays;
                    int Max_m = allclass[i].PAAPA_MaxMonth;
                    int Max_Y = allclass[i].PAAPA_MaxAge;

                    Min_date = _cut_of_date.AddYears(-Min_Y).AddMonths(-Min_m).AddDays(-Min_d);
                    Max_date = _cut_of_date.AddYears(-Max_Y).AddMonths(-Max_m).AddDays(-Max_d);
                    DateTime dob = Convert.ToDateTime(stu.PASR_DOB);
                    if (Max_date <= dob && Min_date >= dob)
                    {
                        agepoints = allclass[i].PAAPA_Points;
                    }
                }

                decimal totalsalary = Convert.ToDecimal(stu.PASR_FatherIncome) +Convert.ToDecimal(stu.PASR_MotherIncome);

                var incomepoints = _StudentApplicationContext.PAstudentincome.Where(t => t.MI_Id == stu.MI_Id && t.PAAPI_FromIncome <= totalsalary && t.PAAPI_ToIncome >= totalsalary).ToList();

                if (incomepoints.Count() > 0)
                {
                    income = incomepoints[0].PAAPI_Points;
                }

                var castepointss = _StudentApplicationContext.PAstudentcatse.Where(t => t.MI_Id == stu.MI_Id && t.IMC_Id == stu.Caste_Id).ToList();

                if (castepointss.Count() > 0)
                {
                    castepoints = castepointss[0].PAAPC_Points;
                }


                if (studentpoints.Count == 0)
                {

                    stud.PASR_Id = stu.pasR_Id;

                    stud.PASAP_AGE = agepoints;
                    stud.PASAP_INCOME = income;
                    stud.PASAP_CASTE = castepoints;
                    stud.PASAP_QA = 0;
                    stud.PASAP_ADRESS = 0;
                    stud.PASAP_TOTAL = agepoints + income + castepoints;

                    _StudentApplicationContext.Add(stud);
                    _StudentApplicationContext.SaveChanges();


                }
                else if (studentpoints.Count > 0)
                {
                    var studentpointsupdate = _StudentApplicationContext.PointsDMO.Single(g => g.PASR_Id == stu.pasR_Id);

                    studentpointsupdate.PASAP_AGE = agepoints;
                    studentpointsupdate.PASAP_INCOME = income;
                    studentpointsupdate.PASAP_CASTE = castepoints;
                    studentpointsupdate.PASAP_QA = 0;
                    studentpointsupdate.PASAP_ADRESS = 0;
                    studentpointsupdate.PASAP_TOTAL = agepoints + income + castepoints;

                    _StudentApplicationContext.Update(studentpoints);
                    _StudentApplicationContext.SaveChanges();

                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public StudentApplicationDTO getmaxminage(StudentApplicationDTO stu)
        {

            try
            {





                StudentApplicationDTO newdto = new StudentApplicationDTO();
                List<classdto> termsList = new List<classdto>();
                List<classdto> termsList2 = new List<classdto>();
                List<MasterAcademic> result = new List<MasterAcademic>();
                result = _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.Is_Active == true && t.ASMAY_Pre_ActiveFlag == 1).ToList();
                stu.fill_asy = result.ToArray();


                DateTime _cut_of_date = Convert.ToDateTime(result[0].ASMAY_Cut_Of_Date);

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();

                List<MasterConfiguration> config = new List<MasterConfiguration>();
                config = _StudentApplicationContext.masterConfig.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == result.FirstOrDefault().ASMAY_Id).ToList();



                var AdmissionStatusflag = config.FirstOrDefault().ISPAC_DefaultStatusFlag;

                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == stu.roleid).IVRMRT_Role;
                int maxage = 1;
                int minage = 1;
                if (rolename == "OnlinePreadmissionUser")
                {
                    maxage = config.FirstOrDefault().ISPAC_DOBMaxAgeFlag;
                    minage = config.FirstOrDefault().ISPAC_DOBMinAgeFlag;
                }
                else
                {
                    maxage = 0;
                    minage = 0;
                }


                DateTime Min_date = DateTime.Today;
                DateTime Max_date = DateTime.Today;
                if (maxage == 1 && minage == 1)
                {
                    for (int i = 0; i < allclass.Count(); i++)
                    {
                        int Min_d = allclass[i].ASMCL_MinAgeDays;
                        int Min_m = allclass[i].ASMCL_MinAgeMonth;
                        int Min_Y = allclass[i].ASMCL_MinAgeYear;
                        int Max_d = allclass[i].ASMCL_MaxAgeDays;
                        int Max_m = allclass[i].ASMCL_MaxAgeMonth;
                        int Max_Y = allclass[i].ASMCL_MaxAgeYear;

                        Min_date = _cut_of_date.AddYears(-Min_Y).AddMonths(-Min_m).AddDays(-Min_d);
                        Max_date = _cut_of_date.AddYears(-Max_Y).AddMonths(-Max_m).AddDays(-Max_d);
                        termsList2.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, mindate = Min_date, maxdate = Max_date, ASMCL_ClassName = allclass[i].ASMCL_ClassName });
                        DateTime dob = stu.dateString;
                        if (Max_date <= dob && Min_date >= dob)
                        {
                            //termsList.Add(allclass[i]);
                            termsList.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, mindate = Min_date, maxdate = Max_date, ASMCL_ClassName = allclass[i].ASMCL_ClassName });
                        }

                    }
                    //stu.maxdate = Max_date;

                    //stu.mindate = Min_date;
                }
                else if (maxage == 1)
                {
                    for (int i = 0; i < allclass.Count(); i++)
                    {
                        int Min_d = allclass[i].ASMCL_MinAgeDays;
                        int Min_m = allclass[i].ASMCL_MinAgeMonth;
                        int Min_Y = allclass[i].ASMCL_MinAgeYear;
                        int Max_d = allclass[i].ASMCL_MaxAgeDays;
                        int Max_m = allclass[i].ASMCL_MaxAgeMonth;
                        int Max_Y = allclass[i].ASMCL_MaxAgeYear;

                        Min_date = _cut_of_date.AddYears(-Min_Y).AddMonths(-Min_m).AddDays(-Min_d);
                        Max_date = _cut_of_date.AddYears(-Max_Y).AddMonths(-Max_m).AddDays(-Max_d);
                        termsList2.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, mindate = Min_date, maxdate = Max_date, ASMCL_ClassName = allclass[i].ASMCL_ClassName });
                        DateTime dob = stu.dateString;
                        if (Max_date <= dob)
                        {
                            termsList.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, mindate = Min_date, maxdate = Max_date, ASMCL_ClassName = allclass[i].ASMCL_ClassName });
                        }

                    }
                    //stu.maxdate = Max_date;

                    //stu.mindate = Min_date;
                }
                else if (minage == 1)
                {
                    for (int i = 0; i < allclass.Count(); i++)
                    {
                        int Min_d = allclass[i].ASMCL_MinAgeDays;
                        int Min_m = allclass[i].ASMCL_MinAgeMonth;
                        int Min_Y = allclass[i].ASMCL_MinAgeYear;
                        int Max_d = allclass[i].ASMCL_MaxAgeDays;
                        int Max_m = allclass[i].ASMCL_MaxAgeMonth;
                        int Max_Y = allclass[i].ASMCL_MaxAgeYear;

                        Min_date = _cut_of_date.AddYears(-Min_Y).AddMonths(-Min_m).AddDays(-Min_d);
                        Max_date = _cut_of_date.AddYears(-Max_Y).AddMonths(-Max_m).AddDays(-Max_d);
                        termsList2.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, mindate = Min_date, maxdate = Max_date, ASMCL_ClassName = allclass[i].ASMCL_ClassName });
                        DateTime dob = stu.dateString;
                        if (Min_date >= dob)
                        {
                            termsList.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, mindate = Min_date, maxdate = Max_date, ASMCL_ClassName = allclass[i].ASMCL_ClassName });

                        }

                    }
                    //stu.maxdate = Max_date;

                    //stu.mindate = Min_date;
                }
                else if (maxage == 0 && minage == 0)
                {
                    for (int i = 0; i < allclass.Count(); i++)
                    {
                        termsList.Add(new classdto() { ASMCL_Id = allclass[i].ASMCL_Id, ASMCL_ClassName = allclass[i].ASMCL_ClassName });
                    }
                }
                stu.Class_list = termsList.ToArray();

                stu.Class_list2 = termsList2.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stu;
        }

        // Added on 10-11-2016
        /*
        public CommonDTO getOrgAndInstName(CommonDTO cm)
        {
            try
            {
                var org = _StudentApplicationContext.Organisation.Single(d=>d.MO_Id == cm.IVRM_MO_Id);
                var inst = _StudentApplicationContext.Inst.Single(d=>d.MI_Id == cm.IVRM_MI_Id);
                cm.orgName = org.MO_Name;
                cm.instName = inst.MI_Name;
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cm;
        }
        */
        // Added on 10-11-2016


        public PaymentDetails payuresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "success")
            {


                stu.MI_Id = Convert.ToInt64(response.udf3);
                stu.PASR_MobileNo = response.phone;
                stu.pasR_Id = Convert.ToInt64(response.udf2);
                stu.PASR_emailId = response.email;
                stu.ASMAY_Id = Convert.ToInt64(response.udf5);


                data.MI_Id = Convert.ToInt64(response.udf3);
                data.ASMCL_ID = Convert.ToInt64(response.udf4);
                data.ASMAY_Id = Convert.ToInt64(response.udf5);

                string recno = get_grp_reptno(data);

                var confirmstatus = 0;

                if (recno != "0")
                {
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf6, recno);
                }
                else
                {
                    recno = response.txnid;
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf6, recno);
                }



                if (confirmstatus > 0)
                {

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                    if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    {
                        Email Email = new Email(_db);

                        Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_REGISTRATION", stu.pasR_Id);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    {
                        SMS sms = new SMS(_db);
                        sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_REGISTRATION", stu.pasR_Id);

                    }
                }



            }
            else
            {
                dto.status = response.status;



            }

            return response;
        }
        //  aymentpart for AGGREGTOR
        public Array paymentPart(StudentApplicationDTO enq)
        {
            Payment pay = new Payment(_db);
            ProspectusDTO data = new ProspectusDTO();
            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                // ProspectusDTO ProspectusDTO = new ProspectusDTO();
                var FeeAmountresult = (from a in _feecontext.feeYCC

                                       from c in _feecontext.feeYCCC
                                       from d in _feecontext.FeeAmountEntryDMO

                                       from g in _feecontext.FeeHeadDMO
                                       where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_Id && g.FMH_Flag == "R" && c.ASMCL_Id == enq.ASMCL_Id)
                                       select new FeeAmountEntryDMO
                                       {
                                           FMA_Id = d.FMA_Id,
                                           FMA_Amount = d.FMA_Amount
                                       }
            ).FirstOrDefault();

                try
                {
                    // string ids = enq.ftiidss;

                    using (var cmd1 = _feecontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Preadmission_Split_Payment_Registration";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = enq.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                        SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                        SqlDbType.VarChar)
                        {
                            Value = enq.pasR_Id
                        });

                        //cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                        // SqlDbType.VarChar)
                        //{
                        //    Value = enq.multiplegroups
                        //});

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd1.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new FeeSlplitOnlinePayment
                                    {
                                        name = "splitId" + autoinc.ToString(),
                                        merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                        value = dataReader["balance"].ToString(),
                                        commission = "0",
                                        description = "Online Payment",
                                    });

                                    autoinc = autoinc + 1;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                if (FeeAmountresult != null)
                {


                    //string fmaid = Convert.ToString(FeeAmountresult.FMA_Id);

                    //PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                    //// PaymentDetailsDto.amount = paymentdetails.FirstOrDefault().IVRMOP_PROS_AMOUNT;
                    //PaymentDetailsDto.amount = FeeAmountresult.FMA_Amount;
                    ////PaymentDetailsDto.trans_id = "200";
                    //PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                    //PaymentDetailsDto.productinfo = "StudentApplication";
                    //PaymentDetailsDto.firstname = enq.PASR_FirstName;
                    //PaymentDetailsDto.email = enq.PASR_emailId;
                    //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                    //PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                    //PaymentDetailsDto.phone = enq.PASR_MobileNo;
                    //PaymentDetailsDto.udf1 = "Rs.";
                    //PaymentDetailsDto.udf2 = enq.pasR_Id.ToString();
                    //PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                    //PaymentDetailsDto.udf4 = enq.ASMAY_Id.ToString();
                    //PaymentDetailsDto.udf5 = enq.ASMCL_Id.ToString();
                    //PaymentDetailsDto.udf6 = fmaid;
                    //PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
                    //PaymentDetailsDto.status = "success";
                    //PaymentDetailsDto.service_provider = "payu_paisa";

                    //PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);







                    PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }

                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);

                    PaymentDetailsDto.productinfo = payinfo;
                    PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                    PaymentDetailsDto.firstname = enq.PASR_FirstName;


                    PaymentDetailsDto.email = enq.PASR_emailId;

                    PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                    PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                    PaymentDetailsDto.phone = enq.PASR_MobileNo;
                    PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                    PaymentDetailsDto.udf2 = Convert.ToString(enq.pasR_Id);
                    PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                    PaymentDetailsDto.udf4 = enq.ASMCL_Id.ToString();
                    PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                    PaymentDetailsDto.udf6 = enq.ASMCL_Id.ToString();
                    // PaymentDetailsDto.transaction_response_url = "";
                    PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/StudentApplication/paymentresponse/";
                    PaymentDetailsDto.status = "success";
                    PaymentDetailsDto.service_provider = "payu_paisa";

                    PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);



                    FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                    feepaydet.MI_Id = enq.MI_Id;
                    feepaydet.ASMAY_ID = enq.ASMAY_Id;

                    feepaydet.FTCU_Id = 1;
                    feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                    feepaydet.FYP_Bank_Name = "";
                    feepaydet.FYP_Bank_Or_Cash = "O";
                    feepaydet.FYP_DD_Cheque_No = "";
                    feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
                    feepaydet.FYP_Date = DateTime.Now;
                    feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
                    feepaydet.FYP_Tot_Waived_Amt = 0;
                    feepaydet.FYP_Tot_Fine_Amt = 0;
                    feepaydet.FYP_Tot_Concession_Amt = 0;
                    feepaydet.FYP_Remarks = "Preadmission Registration Payment";
                    feepaydet.FYP_Chq_Bounce = "CL";
                    feepaydet.DOE = DateTime.Now;
                    feepaydet.CreatedDate = DateTime.Now;
                    feepaydet.UpdatedDate = DateTime.Now;
                    feepaydet.user_id = enq.Id;
                    feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                    feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                    feepaydet.FYP_PaymentReference_Id = "";

                    _feecontext.FeePaymentDetailsDMO.Add(feepaydet);
                    _feecontext.SaveChanges();

                    PaymentDetailsDto.paymentdetails = "True";

                }
                else
                {
                    PaymentDetailsDto.paymentdetails = "false";
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        //normal account
        //public Array paymentPart(StudentApplicationDTO enq)
        //{
        //    Payment pay = new Payment(_db);
        //    ProspectusDTO data = new ProspectusDTO();
        //    List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
        //    PaymentDetails PaymentDetailsDto = new PaymentDetails();
        //    //enq.ASMAY_Id = 7;
        //    try
        //    {
        //        paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
        //        // ProspectusDTO ProspectusDTO = new ProspectusDTO();
        //        var FeeAmountresult = (from a in _feecontext.feeYCC

        //                               from c in _feecontext.feeYCCC
        //                               from d in _feecontext.FeeAmountEntryDMO

        //                               from g in _feecontext.FeeHeadDMO
        //                               where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_Id && g.FMH_Flag == "R" && c.ASMCL_Id == enq.ASMCL_Id)
        //                               select new FeeAmountEntryDMO
        //                               {
        //                                   FMA_Id = d.FMA_Id,
        //                                   FMA_Amount = d.FMA_Amount
        //                               }
        //    ).FirstOrDefault();


        //        // data.feedata = result.;
        //        //_ProspectusContext.Database.ExecuteSqlCommand("Insert_fee_tables @p0,@p1,@p2,@p3", enq.MI_ID, enq.ASMAY_Id, enq.PASP_Id,enq.ASMCL_Id);


        //        if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
        //        {
        //            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
        //            enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
        //            enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
        //            PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
        //        }

        //        if (FeeAmountresult != null)
        //        {


        //            string fmaid = Convert.ToString(FeeAmountresult.FMA_Id);

        //            PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY.Trim();
        //            // PaymentDetailsDto.amount = paymentdetails.FirstOrDefault().IVRMOP_PROS_AMOUNT;
        //            PaymentDetailsDto.amount = FeeAmountresult.FMA_Amount;
        //            //PaymentDetailsDto.trans_id = "200";
        //            PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
        //            PaymentDetailsDto.productinfo = "StudentApplication";
        //            PaymentDetailsDto.firstname = enq.PASR_FirstName;
        //            PaymentDetailsDto.email = enq.PASR_emailId;
        //            PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
        //            PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL.Trim();
        //            PaymentDetailsDto.phone = enq.PASR_MobileNo;
        //            PaymentDetailsDto.udf1 = "Rs.";
        //            PaymentDetailsDto.udf2 = enq.pasR_Id.ToString();
        //            PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
        //            PaymentDetailsDto.udf4 = enq.ASMAY_Id.ToString();
        //            PaymentDetailsDto.udf5 = enq.ASMCL_Id.ToString();
        //            PaymentDetailsDto.udf6 = fmaid;
        //            PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
        //            PaymentDetailsDto.status = "success";
        //            PaymentDetailsDto.service_provider = "payu_paisa";

        //            PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);


        //            FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
        //            feepaydet.MI_Id = enq.MI_Id;
        //            feepaydet.ASMAY_ID = enq.ASMAY_Id;

        //            feepaydet.FTCU_Id = 1;
        //            feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
        //            feepaydet.FYP_Bank_Name = "";
        //            feepaydet.FYP_Bank_Or_Cash = "O";
        //            feepaydet.FYP_DD_Cheque_No = "";
        //            feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
        //            feepaydet.FYP_Date = DateTime.Now;
        //            feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
        //            feepaydet.FYP_Tot_Waived_Amt = 0;
        //            feepaydet.FYP_Tot_Fine_Amt = 0;
        //            feepaydet.FYP_Tot_Concession_Amt = 0;
        //            feepaydet.FYP_Remarks = "Preadmission Registration Payment";
        //            feepaydet.FYP_Chq_Bounce = "CL";
        //            feepaydet.DOE = DateTime.Now;
        //            feepaydet.CreatedDate = DateTime.Now;
        //            feepaydet.UpdatedDate = DateTime.Now;
        //            feepaydet.user_id = enq.Id;
        //            feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
        //            feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
        //            feepaydet.FYP_PaymentReference_Id = "";

        //            _feecontext.FeePaymentDetailsDMO.Add(feepaydet);
        //            _feecontext.SaveChanges();

        //            PaymentDetailsDto.paymentdetails = "True";

        //        }
        //        else
        //        {
        //            PaymentDetailsDto.paymentdetails = "false";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //    return PaymentDetailsDto.PaymentDetailsList;

        //}


        public async Task<StudentHelthcertificateDTO> savehealthcertificatedetail(StudentHelthcertificateDTO stu)
        {
            StudentHelthcertificateDMO pge = Mapper.Map<StudentHelthcertificateDMO>(stu);
            try
            {
                if (pge.PASHD_Id > 0)
                {
                    var result = _StudentApplicationContext.StudentHelthcertificate.Single(t => t.PASHD_Id == pge.PASHD_Id);
                    result.MI_Id = pge.MI_Id;
                    result.PASHD_VaccinationDate = pge.PASHD_VaccinationDate;
                    result.PASHD_FitsFlag = pge.PASHD_FitsFlag;
                    result.PASHD_FitsDate = pge.PASHD_FitsDate;
                    result.PASHD_Illness = pge.PASHD_Illness;
                    result.PASHD_HepatitisB = pge.PASHD_HepatitisB;
                    result.PASHD_TyphoidFever = pge.PASHD_TyphoidFever;
                    result.PASHD_Ailment = pge.PASHD_Ailment;
                    result.PASHD_Allergy = pge.PASHD_Allergy;
                    result.PASHD_HealthProblem = pge.PASHD_HealthProblem;
                    result.PASHD_BloodGroup = pge.PASHD_BloodGroup;
                    result.PASHD_UpdateDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;

                    Mapper.Map(stu, result);

                    _StudentApplicationContext.Update(result);
                    var contactExists = _StudentApplicationContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        stu.returnval = true;
                    }
                    else
                    {
                        stu.returnval = false;
                    }

                    stu.updated = true;




                }
                else
                {
                    pge.PASHD_EntryDate = DateTime.Now;
                    pge.PASHD_UpdateDate = DateTime.Now;
                    pge.CreatedDate = DateTime.Now;
                    pge.UpdatedDate = DateTime.Now;
                    _StudentApplicationContext.Add(pge);
                    var contactExists = _StudentApplicationContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        stu.returnval = true;
                    }
                    else
                    {
                        stu.returnval = false;
                    }
                    stu.updated = false;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return stu;
        }

        public async Task<StudentHelthcertificateDTO> getstudata(StudentHelthcertificateDTO stu)
        {
            try
            {

                var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleId).ToList();
                if (rolelist[0].IVRMRT_Role.Equals("ADMIN") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR") || rolelist[0].IVRMRT_Role.Equals("Admission End User"))
                {

                    List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                    allRegStudent = await _StudentApplicationContext.StudentHelthcertificate.Where(d => d.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.studentDetailsTEmp = allRegStudent.ToArray();

                    List<long> temparr = new List<long>();
                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudent[i].PASR_Id);
                    }

                    stu.studentDetails = (from a in _StudentApplicationContext.Enq
                                          where (!temparr.Contains(a.pasr_id) && a.MI_Id == stu.MI_Id && a.PASR_Adm_Confirm_Flag == false)
                                          select new StudentHelthcertificateDTO
                                          {
                                              PASR_Id = a.pasr_id,
                                              PASR_FirstName = a.PASR_FirstName,
                                              PASR_MiddleName = a.PASR_MiddleName,
                                              PASR_LastName = a.PASR_LastName
                                          }
                     ).ToList().ToArray();
                }
                else
                {
                    List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                    allRegStudent = await _StudentApplicationContext.StudentHelthcertificate.Where(d => d.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.studentDetailsTEmp = allRegStudent.ToArray();

                    List<long> temparr = new List<long>();
                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudent[i].PASR_Id);
                    }

                    stu.studentDetails = (from a in _StudentApplicationContext.Enq
                                          where (!temparr.Contains(a.pasr_id) && a.Id == stu.Id && a.PASR_Adm_Confirm_Flag == false)
                                          select new StudentHelthcertificateDTO
                                          {
                                              PASR_Id = a.pasr_id,
                                              PASR_FirstName = a.PASR_FirstName,
                                              PASR_MiddleName = a.PASR_MiddleName,
                                              PASR_LastName = a.PASR_LastName
                                          }
                     ).ToList().ToArray();
                }


                if (rolelist[0].IVRMRT_Role.Equals("ADMIN") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR") || rolelist[0].IVRMRT_Role.Equals("Admission End User"))
                {
                    stu.studenthelthDetails = (from a in _StudentApplicationContext.Enq
                                               from b in _StudentApplicationContext.StudentHelthcertificate
                                               where (a.pasr_id == b.PASR_Id && a.MI_Id == stu.MI_Id && a.PASR_Adm_Confirm_Flag == false)
                                               select new StudentHelthcertificateDTO
                                               {
                                                   PASHD_Id = b.PASHD_Id,
                                                   PASR_FirstName = a.PASR_FirstName,
                                                   PASR_MiddleName = a.PASR_MiddleName,
                                                   PASR_LastName = a.PASR_LastName,
                                                   PASR_EMAIL = a.PASR_emailId,
                                                   PASR_Id = a.pasr_id
                                               }
                   ).ToList().ToArray();
                }
                else
                {
                    stu.studenthelthDetails = (from a in _StudentApplicationContext.Enq
                                               from b in _StudentApplicationContext.StudentHelthcertificate
                                               where (a.pasr_id == b.PASR_Id && a.MI_Id == stu.MI_Id && a.Id == stu.Id && a.PASR_Adm_Confirm_Flag == false)
                                               select new StudentHelthcertificateDTO
                                               {
                                                   PASHD_Id = b.PASHD_Id,
                                                   PASR_FirstName = a.PASR_FirstName,
                                                   PASR_MiddleName = a.PASR_MiddleName,
                                                   PASR_LastName = a.PASR_LastName,
                                                   PASR_EMAIL = a.PASR_emailId,
                                                   PASR_Id = a.pasr_id
                                               }
                  ).ToList().ToArray();
                }




                stu.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R").ToArray();


            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            return stu;
        }
        public StudentHelthcertificateDTO getEdithelthData(StudentHelthcertificateDTO stu)
        {

            try
            {
                var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleId).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("ADMIN") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR") || rolelist[0].IVRMRT_Role.Equals("Admission End User"))
                {
                    List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                    allRegStudent = _StudentApplicationContext.StudentHelthcertificate.Where(d => d.MI_Id.Equals(stu.MI_Id)).ToList();
                    stu.studentDetailsTEmp = allRegStudent.ToArray();

                    List<long> temparr = new List<long>();
                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudent[i].PASR_Id);
                    }

                    stu.studentDetails = (from a in _StudentApplicationContext.Enq
                                          where (temparr.Contains(a.pasr_id))
                                          select new StudentHelthcertificateDTO
                                          {
                                              PASR_Id = a.pasr_id,
                                              PASR_FirstName = a.PASR_FirstName,
                                              PASR_MiddleName = a.PASR_MiddleName,
                                              PASR_LastName = a.PASR_LastName
                                          }
                     ).ToList().ToArray();
                }
                else
                {
                    List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                    allRegStudent = _StudentApplicationContext.StudentHelthcertificate.Where(d => d.MI_Id.Equals(stu.MI_Id)).ToList();
                    stu.studentDetailsTEmp = allRegStudent.ToArray();

                    List<long> temparr = new List<long>();
                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudent[i].PASR_Id);
                    }

                    stu.studentDetails = (from a in _StudentApplicationContext.Enq
                                          where (temparr.Contains(a.pasr_id) && a.Id == stu.Id)
                                          select new StudentHelthcertificateDTO
                                          {
                                              PASR_Id = a.pasr_id,
                                              PASR_FirstName = a.PASR_FirstName,
                                              PASR_MiddleName = a.PASR_MiddleName,
                                              PASR_LastName = a.PASR_LastName,
                                          }
                     ).ToList().ToArray();
                }

                stu.studenthelth_DTObj = (from a in _StudentApplicationContext.Enq
                                          from b in _StudentApplicationContext.StudentHelthcertificate
                                          where (a.pasr_id == b.PASR_Id && a.MI_Id == stu.MI_Id && b.PASHD_Id.Equals(stu.PASHD_Id))
                                          select b
                      ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return stu;
        }
        public StudentHelthcertificateDTO deletehelthdetails(StudentHelthcertificateDTO dt)
        {
            try
            {
                List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                allRegStudent = _StudentApplicationContext.StudentHelthcertificate.Where(d => d.PASHD_Id.Equals(dt.PASHD_Id)).ToList();

                if (allRegStudent.Count() > 0)
                {
                    for (int i = 0; i < allRegStudent.Count(); i++)
                    {
                        _StudentApplicationContext.Remove(allRegStudent.ElementAt(i));
                        var contactExists = _StudentApplicationContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            dt.returnval = true;
                        }
                        else
                        {
                            dt.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public StudentHelthcertificateDTO printgethelthData(StudentHelthcertificateDTO stu)
        {

            try
            {
                stu.studenthelthDTO = (from a in _StudentApplicationContext.Enq
                                       from b in _StudentApplicationContext.StudentHelthcertificate
                                       where (a.pasr_id == b.PASR_Id && a.MI_Id == stu.MI_Id && b.PASHD_Id.Equals(stu.PASHD_Id))
                                       select new StudentHelthcertificateDTO
                                       {
                                           PASR_FirstName = a.PASR_FirstName,
                                           PASR_MiddleName = a.PASR_MiddleName,
                                           PASR_LastName = a.PASR_LastName,
                                           PASR_FatherName = a.PASR_FatherName,
                                           PASR_Age = a.PASR_Age,
                                           PASHD_Id = b.PASHD_Id,
                                           PASHD_VaccinationDate = b.PASHD_VaccinationDate,
                                           PASHD_FitsFlag = b.PASHD_FitsFlag,
                                           PASHD_FitsDate = b.PASHD_FitsDate,
                                           PASHD_Illness = b.PASHD_Illness,
                                           PASHD_HepatitisB = b.PASHD_HepatitisB,
                                           PASHD_TyphoidFever = b.PASHD_TyphoidFever,
                                           PASHD_Ailment = b.PASHD_Ailment,
                                           PASHD_Allergy = b.PASHD_Allergy,
                                           PASHD_HealthProblem = b.PASHD_HealthProblem,
                                           PASHD_BloodGroup = b.PASHD_BloodGroup,
                                           PASR_Id = b.PASR_Id,

                                           PASHD_ChickenpoxFlag = b.PASHD_ChickenpoxFlag,
                                           PASHD_ChickenpoxDate = b.PASHD_ChickenpoxDate,
                                           PASHD_DiptheriaFlag = b.PASHD_DiptheriaFlag,
                                           PASHD_DiptheriaDate = b.PASHD_DiptheriaDate,
                                           PASHD_EpidemicFlag = b.PASHD_EpidemicFlag,
                                           PASHD_EpidemicDate = b.PASHD_EpidemicDate,
                                           PASHD_MeaslesFlag = b.PASHD_MeaslesFlag,
                                           PASHD_MeaslesDate = b.PASHD_MeaslesDate,
                                           PASHD_MumpsFlag = b.PASHD_MumpsFlag,
                                           PASHD_MumpsDate = b.PASHD_MumpsDate,
                                           PASHD_RingwormFlag = b.PASHD_RingwormFlag,
                                           PASHD_RingwormDate = b.PASHD_RingwormDate,
                                           PASHD_ScarletFlag = b.PASHD_ScarletFlag,
                                           PASHD_ScarletDate = b.PASHD_ScarletDate,
                                           PASHD_SmallPoxFlag = b.PASHD_SmallPoxFlag,
                                           PASHD_SmallPoxDate = b.PASHD_SmallPoxDate,
                                           PASHD_WhoopingFlag = b.PASHD_WhoopingFlag,
                                           PASHD_WhoopingDate = b.PASHD_WhoopingDate
                                       }
                      ).ToList().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return stu;
        }

        public string get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < feemasnum.Count; i++)
                {
                    data.auto_receipt_flag = feemasnum[i].FMC_AutoReceiptFeeGroupFlag;
                }

                if (data.auto_receipt_flag == 1)
                {

                    var FeeAmountresult = (from a in _feecontext.feeYCC
                                           from c in _feecontext.feeYCCC
                                           from d in _feecontext.FeeAmountEntryDMO


                                           from g in _feecontext.FeeHeadDMO
                                           where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && g.FMH_Flag == "R" && c.ASMCL_Id == data.ASMCL_ID)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMH_Id = d.FMH_Id,
                                           }
           ).ToList();

                    List<long> HeadId = new List<long>();
                    foreach (var item in FeeAmountresult)
                    {
                        HeadId.Add(item.FMH_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _feecontext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                    var final_rept_no = "";
                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                    list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
                                from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    //FGAR_Name = b.FGAR_Name,
                                    //FMG_Id = c.FMG_Id
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {


                        using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = groupidss
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        }

                        //data.auto_FYP_Receipt_No = final_rept_no;

                        //data.FYP_Receipt_No = final_rept_no;
                    }
                }
                else
                {
                    data.FYP_Receipt_No = "0";
                }

                //else if (data.automanualreceiptno == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data.FYP_Receipt_No;
        }

        public StudentApplicationDTO fill_prospectus(StudentApplicationDTO data)
        {

            try
            {
                data.prospectusDatalist = (from a in _db.prospectus
                                           from b in _db.Payment_PA_ProspectusDMO
                                           where (a.PASP_Id == b.PASP_Id && a.PASP_Id == data.PASP_Id)
                                           select a
                        ).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
