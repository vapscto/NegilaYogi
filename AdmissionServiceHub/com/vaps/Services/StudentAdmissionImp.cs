using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;

using System;
using CommonLibrary;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentAdmissionImp : Interfaces.StudentAdmissionInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login = new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        private readonly AdmissionFormContext _AdmissionFormContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<StudentAdmissionImp> _log;
        public StudentAdmissionImp(AdmissionFormContext AdmissionFormContext, DomainModelMsSqlServerContext db, ILogger<StudentAdmissionImp> loggerFactor, UserManager<ApplicationUser> UserManager)
        {
            _AdmissionFormContext = AdmissionFormContext;
            _db = db;
            _log = loggerFactor;
            _UserManager = UserManager;
        }
        public async Task<Adm_M_StudentDTO> GetData(Adm_M_StudentDTO Adm_M_StudentDTO)
        {
            try
            {
                List<MasterAcademic> allyearonload = new List<MasterAcademic>();
                allyearonload = await _AdmissionFormContext.year.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id && d.Is_Active == true && d.ASMAY_Year != null).OrderByDescending(d => d.ASMAY_Order).ToListAsync();
                Adm_M_StudentDTO.academicYearOnLoad = allyearonload.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _AdmissionFormContext.year.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id && d.Is_Active == true && d.ASMAY_Id == Adm_M_StudentDTO.ASMAY_Id && d.ASMAY_Year != null).OrderByDescending(d => d.ASMAY_Order).ToListAsync();
                Adm_M_StudentDTO.AllAcademicYear = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = await _AdmissionFormContext.School_M_Class.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id && d.ASMCL_ActiveFlag == true && d.ASMCL_ClassName != null).OrderBy(d => d.ASMCL_Order).ToListAsync();
                Adm_M_StudentDTO.AllClass = allclass.ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = await _AdmissionFormContext.AdmSection.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id && d.ASMC_ActiveFlag == 1
                && d.ASMC_SectionName != null).OrderBy(d => d.ASMC_Order).ToListAsync();
                Adm_M_StudentDTO.mastersection = allsection.ToArray();

                List<Country> Allcountry = new List<Country>();
                Allcountry = await _AdmissionFormContext.Country.Where(c => c.IVRMMC_CountryName != null).ToListAsync();
                Adm_M_StudentDTO.AllCountry = Allcountry.ToArray();


                List<Country> allCountryforbirthplace = new List<Country>();
                allCountryforbirthplace = await _AdmissionFormContext.Country.Where(c => c.IVRMMC_CountryName != null).ToListAsync();
                Adm_M_StudentDTO.allcountryforbirthplace = allCountryforbirthplace.ToArray();


                //  long country_id = _AdmissionFormContext.Country.Single(s=>s.IVRMMC_Id).Where(c => c.IVRMMC_Default==true).ToList();

                long country_id = _AdmissionFormContext.Country.Where(t => t.IVRMMC_Default == 1).Select(t => t.IVRMMC_Id).FirstOrDefault();

                List<State> AllState = new List<State>();
                AllState = _AdmissionFormContext.State.Where(t => t.IVRMMC_Id == country_id).ToList();
                Adm_M_StudentDTO.AllState = AllState.ToArray();


                //country code
                var Allcountrycode = _AdmissionFormContext.Country.Where(c => c.IVRMMC_CountryPhCode != null).ToList();
                if (Allcountrycode.Count > 0)
                {
                    Adm_M_StudentDTO.AllCountrycode = Allcountrycode.ToArray();
                }



                List<MasterReligionDMO> AllReligion = new List<MasterReligionDMO>();
                AllReligion = await _AdmissionFormContext.Religion.Where(r => r.Is_Active == true && r.IVRMMR_Name != null).ToListAsync();
                Adm_M_StudentDTO.AllReligion = AllReligion.ToArray();

                List<mastercasteDMO> AllCaste = new List<mastercasteDMO>();
                AllCaste = await _AdmissionFormContext.Caste.Where(c => c.MI_Id == Adm_M_StudentDTO.MI_Id && c.IMC_CasteName != null).ToListAsync();
                Adm_M_StudentDTO.AllCaste = AllCaste.ToArray();

                List<CasteCategory> AllcasteCategory = new List<CasteCategory>();
                AllcasteCategory = await _AdmissionFormContext.CasteCategory.Where(c => c.IMCC_CategoryName != null).ToListAsync();
                Adm_M_StudentDTO.AllcasteCategory = AllcasteCategory.ToArray();

                List<MasterReference> AllRefrence = new List<MasterReference>();
                AllRefrence = await _AdmissionFormContext.MasterReference.Where(m => m.PAMR_ReferenceName != null).ToListAsync();
                Adm_M_StudentDTO.AllRefrence = AllRefrence.ToArray();

                List<MasterSource> AllSources = new List<MasterSource>();
                AllSources = await _AdmissionFormContext.MasterSource.Where(m => m.PAMS_SourceName != null).ToListAsync();
                Adm_M_StudentDTO.AllSources = AllSources.ToArray();

                List<MasterAcademic> allyearforreadmit = new List<MasterAcademic>();
                allyearforreadmit = await _AdmissionFormContext.year.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id && d.Is_Active == true && d.ASMAY_Year != null).OrderByDescending(a => a.ASMAY_Id).ToListAsync();
                Adm_M_StudentDTO.academicyearforreadmit = allyearforreadmit.ToArray();

                Adm_M_StudentDTO.adm_m_stud_cat = await (from m in _db.mastercategory
                                                         from n in _db.Masterclasscategory
                                                         where m.AMC_Id == n.AMC_Id && n.MI_Id == Adm_M_StudentDTO.MI_Id &&
                                                         n.Is_Active == true
                                                         select new MasterClassCategoryDTO
                                                         {
                                                             ASMCC_Id = n.ASMCC_Id,
                                                             className = m.AMC_Name
                                                         }).ToArrayAsync();

                List<Adm_M_StudentDTO> adm_m_student1 = new List<Adm_M_StudentDTO>();
                adm_m_student1 = await (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                        from cls in _AdmissionFormContext.School_M_Class
                                        where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == Adm_M_StudentDTO.MI_Id && adm_stu.AMST_ActiveFlag == 1
                                        && adm_stu.AMST_SOL != "Del")
                                        select new Adm_M_StudentDTO
                                        {
                                            AMST_FirstName = adm_stu.AMST_FirstName,
                                            AMST_MiddleName = adm_stu.AMST_MiddleName,
                                            AMST_LastName = adm_stu.AMST_LastName,
                                            AMST_Date = adm_stu.AMST_Date,
                                            AMST_Sex = adm_stu.AMST_Sex,
                                            AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                            AMST_AdmNo = adm_stu.AMST_AdmNo,
                                            AMST_emailId = adm_stu.AMST_emailId,
                                            stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                            AMST_Id = adm_stu.AMST_Id,
                                            Class = cls.ASMCL_ClassName,
                                            AMST_SOL = adm_stu.AMST_SOL,
                                            AMST_Photoname = adm_stu.AMST_Photoname,
                                            studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                            (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                            (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim(),
                                        }).OrderByDescending(d => d.AMST_Id).Take(10).ToListAsync();


                Adm_M_StudentDTO.adm_m_student = adm_m_student1.ToArray();

                Adm_M_StudentDTO.AllActivity = await (from act in _AdmissionFormContext.MasterActivityDMO
                                                      select new MasterActivityDMO
                                                      {
                                                          AMA_Id = act.AMA_Id,
                                                          AMA_Activity = act.AMA_Activity,
                                                          AMA_Activity_Desc = act.AMA_Activity_Desc,
                                                      }).ToArrayAsync();



                List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
                admissionconfigurationsettings = await _db.AdmissionStandardDMO.Where(t => t.MI_Id == Adm_M_StudentDTO.MI_Id).ToListAsync();
                Adm_M_StudentDTO.admissioncongigurationList = admissionconfigurationsettings.ToArray();

                Adm_M_StudentDTO.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == Adm_M_StudentDTO.MI_Id).ToArrayAsync();

                //Government Bond.
                Adm_M_StudentDTO.govtBondList = await _AdmissionFormContext.GovernmentBond.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id).ToArrayAsync();
                //Master Board.
                Adm_M_StudentDTO.boardList = await _AdmissionFormContext.MasterBorad.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id && d.Is_Active == true).ToArrayAsync();

                //Fee Concession
                Adm_M_StudentDTO.concessionList = await _AdmissionFormContext.Fee_Master_ConcessionDMO.Where(d => d.MI_Id == Adm_M_StudentDTO.MI_Id
                && d.FMCC_ActiveFlag == true).ToArrayAsync();

                //School Type
                Adm_M_StudentDTO.Schooltypelist = await _AdmissionFormContext.MasterSchoolType.Where(d => d.Is_Active == true).ToArrayAsync();

                Adm_M_StudentDTO.masterstream = await _AdmissionFormContext.Adm_School_Master_Stream.Where(a => a.MI_Id == Adm_M_StudentDTO.MI_Id
                && a.ASMST_ActiveFlag == true).OrderBy(a => a.ASMST_Order).ToArrayAsync();
                //adde
                Adm_M_StudentDTO.yearlist = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == Adm_M_StudentDTO.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<MasterDocumentDMO> MasterDocumentDMO = new List<MasterDocumentDMO>();
                MasterDocumentDMO = await _AdmissionFormContext.MasterDocumentDMO.Where(t => t.MI_Id == Adm_M_StudentDTO.MI_Id).ToListAsync();
                Adm_M_StudentDTO.DocumentList = MasterDocumentDMO.ToArray();
            }
            catch (Exception e)
            {
                _log.LogInformation("GetData() in admission form");
                _log.LogDebug(e.Message);
            }

            return Adm_M_StudentDTO;
        }

        public Adm_M_StudentDTO getdpstate(int id)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            List<State> AllState = new List<State>();
            AllState = _AdmissionFormContext.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
            Adm_M_StudentDTO.AllState = AllState.ToArray();



            return Adm_M_StudentDTO;
        }

        public Adm_M_StudentDTO getdpdistrict(int id)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            List<DistrictDMO> AllDistrict = new List<DistrictDMO>();
            AllDistrict = _AdmissionFormContext.DistrictDMO.Where(t => t.IVRMMS_Id.Equals(id)).ToList();
            Adm_M_StudentDTO.AllDistrict = AllDistrict.ToArray();



            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO onchangebithplacecountry(int id)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            List<State> AllState = new List<State>();
            AllState = _AdmissionFormContext.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
            Adm_M_StudentDTO.allStateForBirthPlace = AllState.ToArray();



            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO onchangenationality(int id)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            List<State> AllState = new List<State>();
            AllState = _AdmissionFormContext.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
            Adm_M_StudentDTO.allStateonchangeNationality = AllState.ToArray();



            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO getdpcities(int id)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            List<City> AllCity = new List<City>();
            AllCity = _AdmissionFormContext.City.Where(t => t.IVRMMS_Id.Equals(id)).ToList().ToList();
            Adm_M_StudentDTO.AllCity = AllCity.ToArray();

            return Adm_M_StudentDTO;
        }

        // EDIT DETAILS
        public Adm_M_StudentDTO GetSelectedRowDetails(Adm_M_StudentDTO Adm_M_StudentDTO)
        {
            //Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();
            try
            {
                List<MasterStudentBondDMO> MasterStudentBondDMO = new List<MasterStudentBondDMO>();
                MasterStudentBondDMO = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.AMST_Id == Adm_M_StudentDTO.AMST_Id).ToList().ToList();
                Adm_M_StudentDTO.BondList = MasterStudentBondDMO.ToArray();

                List<StudentPrevSchoolDMO> StudentPrevSchoolDMO = new List<StudentPrevSchoolDMO>();
                StudentPrevSchoolDMO = _AdmissionFormContext.StudentPrevSchoolDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList().ToList();
                Adm_M_StudentDTO.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();

                List<StudentGuardianDMO> StudentGuardianDMO = new List<StudentGuardianDMO>();
                StudentGuardianDMO = _AdmissionFormContext.StudentGuardianDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList().ToList();
                Adm_M_StudentDTO.StudentGuardianDetails = StudentGuardianDMO.ToArray();

                List<StudentSiblingDMO> StudentSiblingDMO = new List<StudentSiblingDMO>();
                StudentSiblingDMO = _AdmissionFormContext.StudentSiblingDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList().ToList();
                Adm_M_StudentDTO.StudentSiblingDetails = StudentSiblingDMO.ToArray();

                List<Adm_M_Student> adm_m_student = new List<Adm_M_Student>();
                adm_m_student = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_Id == Adm_M_StudentDTO.AMST_Id).ToList();
                Adm_M_StudentDTO.adm_m_student = adm_m_student.ToArray();

                if (adm_m_student.FirstOrDefault().IMCC_Id != null && adm_m_student.FirstOrDefault().IMCC_Id > 0)
                {

                    Adm_M_StudentDTO.AllCaste = (from m in _AdmissionFormContext.CasteCategory
                                                 from n in _AdmissionFormContext.Caste
                                                 where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == adm_m_student.FirstOrDefault().IMCC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                                 select new Adm_M_StudentDTO
                                                 {
                                                     IMC_Id = n.IMC_Id,
                                                     IMC_CasteName = n.IMC_CasteName
                                                 }).ToArray();
                }
                else
                {
                    Adm_M_StudentDTO.AllCaste = (from m in _AdmissionFormContext.CasteCategory
                                                 from n in _AdmissionFormContext.Caste
                                                 where m.IMCC_Id == n.IMCC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                                 select new Adm_M_StudentDTO
                                                 {
                                                     IMC_Id = n.IMC_Id,
                                                     IMC_CasteName = n.IMC_CasteName
                                                 }).ToArray();
                }

                List<MasterDocumentDTO> result = new List<MasterDocumentDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Documents";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = adm_m_student.FirstOrDefault().MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Adm_M_StudentDTO.AMST_Id
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
                                result.Add(new MasterDocumentDTO
                                {
                                    AMSMD_DocumentName = dataReader["AMSMD_DocumentName"].ToString(),
                                    AMSMD_Id = Convert.ToInt64(dataReader["AMSMD_Id"]),
                                    AMSMD_FLAG = Convert.ToBoolean(dataReader["AMSMD_FLAG"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var StudDocumentList = (from sp in _AdmissionFormContext.StudentDocumentDMO
                                        from cp in _AdmissionFormContext.MasterDocumentDMO
                                        where (sp.AMSMD_Id == cp.AMSMD_Id && sp.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                        select new MasterDocumentDTO
                                        {
                                            AMSMD_DocumentName = cp.AMSMD_DocumentName,
                                            AMSMD_Id = cp.AMSMD_Id,
                                            AMSTD_Id = sp.AMSTD_Id,
                                            AMST_Id = sp.AMST_Id,
                                            Document_Path = sp.AMSTD_DOC_Path,
                                            AMSMD_FLAG = cp.AMSMD_FLAG
                                        }).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    StudDocumentList.Add(result[i]);
                }

                Adm_M_StudentDTO.DocumentList = StudDocumentList.ToArray();

                List<StudentAchivementDMO> StudentAchivementDMO = new List<StudentAchivementDMO>();
                StudentAchivementDMO = _AdmissionFormContext.StudentAchivementDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList();
                Adm_M_StudentDTO.StudentAchivementDetails = StudentAchivementDMO.ToArray();

                List<StudentReferenceDMO> StudentReferenceDMO = new List<StudentReferenceDMO>();
                StudentReferenceDMO = _AdmissionFormContext.StudentReferenceDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList();
                Adm_M_StudentDTO.StudentReferenceDetails = StudentReferenceDMO.ToArray();

                List<StudentSourceDMO> StudentSourceDMO = new List<StudentSourceDMO>();
                StudentSourceDMO = _AdmissionFormContext.StudentSourceDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList();
                Adm_M_StudentDTO.StudentSourceDetails = StudentSourceDMO.ToArray();

                List<StudentActitvityDMO> StudentActitvityDMO = new List<StudentActitvityDMO>();
                StudentActitvityDMO = _AdmissionFormContext.StudentActitvityDMO.Where(t => t.AMST_Id.Equals(Adm_M_StudentDTO.AMST_Id)).ToList();
                Adm_M_StudentDTO.StudentActivityDetails = StudentActitvityDMO.ToArray();

                List<DistrictDMO> DistrictDMO = new List<DistrictDMO>();
                DistrictDMO = _AdmissionFormContext.DistrictDMO.ToList();
                Adm_M_StudentDTO.AllDistrict = DistrictDMO.ToArray();

                var asmccid = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_Id == Adm_M_StudentDTO.AMST_Id).ToList();

                Adm_M_StudentDTO.stud_catg_edit = (from m in _db.mastercategory
                                                   from n in _db.Masterclasscategory
                                                   where m.AMC_Id == n.AMC_Id && n.ASMCC_Id == asmccid.FirstOrDefault().AMC_Id
                                                   select new MasterClassCategoryDTO
                                                   {
                                                       ASMCC_Id = n.ASMCC_Id,
                                                       className = m.AMC_Name
                                                   }).ToArray();

                Adm_M_StudentDTO.classsection = (from a in _db.School_Adm_Y_StudentDMO
                                                 from b in _db.Adm_M_Student
                                                 from c in _db.School_M_Class
                                                 from d in _db.School_M_Section
                                                 from e in _db.AcademicYear
                                                 where a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                                 && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == Adm_M_StudentDTO.MI_Id && a.ASMAY_Id == Adm_M_StudentDTO.ASMAY_Id
                                                 && b.AMST_Id == Adm_M_StudentDTO.AMST_Id
                                                 select new Adm_M_StudentDTO
                                                 {
                                                     classname = c.ASMCL_ClassName,
                                                     sectionname = d.ASMC_SectionName,
                                                     ASMAY_Year = e.ASMAY_Year,
                                                     ASMCL_Id = a.ASMCL_Id,
                                                     asms_id = a.ASMS_Id
                                                 }).ToArray();


                if (adm_m_student.FirstOrDefault().AMST_SOL == "L")
                {
                    Adm_M_StudentDTO.classsection = (from a in _db.Student_TC
                                                     from b in _db.Adm_M_Student
                                                     from c in _db.School_M_Class
                                                     from d in _db.School_M_Section
                                                     from e in _db.AcademicYear
                                                     where a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                                     && a.ASMAY_Id == e.ASMAY_Id && b.MI_Id == Adm_M_StudentDTO.MI_Id && a.ASTC_DeletedFlag != true
                                                     && b.AMST_Id == Adm_M_StudentDTO.AMST_Id
                                                     select new Adm_M_StudentDTO
                                                     {
                                                         classname = c.ASMCL_ClassName,
                                                         sectionname = d.ASMC_SectionName,
                                                         ASMAY_Year = e.ASMAY_Year,
                                                         ASMCL_Id = a.ASMCL_Id,
                                                         asms_id = Convert.ToInt64(a.ASMS_Id)
                                                     }).ToArray();
                }

                Adm_M_StudentDTO.multiplemobileno = (from a in _db.Adm_M_Student
                                                     from b in _db.Adm_M_Student_FatherMobileNo
                                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                     select new Adm_M_Student_TempDTO
                                                     {
                                                         AMSTFMNO_Id = b.AMSTFMNO_Id,
                                                         AMST_FatherMobleNo = b.AMST_FatherMobile_No
                                                     }).ToArray();

                Adm_M_StudentDTO.multipleemailid = (from a in _db.Adm_M_Student
                                                    from b in _db.Adm_Master_Father_Email
                                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                    select new Adm_M_Student_Eamil
                                                    {
                                                        AMSTFEMAIL_Id = b.AMSTFEMAIL_Id,
                                                        AMST_FatheremailId = b.AMST_FatheremailId
                                                    }).ToArray();


                Adm_M_StudentDTO.multiplemobilenomother = (from a in _db.Adm_M_Student
                                                           from b in _db.Adm_M_Mother_MobileNo
                                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                           select new Adm_M_Mother_MobileNo1
                                                           {
                                                               AMSTMMNO_Id = b.AMSTMMNO_Id,
                                                               AMST_MotherMobileNo = b.AMST_MotherMobileNo,
                                                               AMSTMMNO_CoutryCode = b.AMSTMMNO_CoutryCode
                                                           }).ToArray();

                Adm_M_StudentDTO.multipleemailidmother = (from a in _db.Adm_M_Student
                                                          from b in _db.Adm_M_Mother_Emailid
                                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                          select new Adm_M_Mother_Emailid1
                                                          {
                                                              AMSTMEMAIL_Id = b.AMSTMEMAIL_Id,
                                                              AMST_MotherEmailId = b.AMST_MotheremailId
                                                          }).ToArray();

                Adm_M_StudentDTO.Adm_M_Student_MobileNoDTO = (from a in _db.Adm_M_Student
                                                              from b in _db.Adm_M_Student_MobileNo
                                                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                              select new Adm_M_Student_MobileNoDTO
                                                              {
                                                                  AMSTSMS_Id = b.AMSTSMS_Id,
                                                                  AMST_MobileNo = b.AMSTSMS_MobileNo,
                                                                  AMSTSMS_CountryCode = b.AMSTSMS_CountryCode

                                                              }).ToArray();

                Adm_M_StudentDTO.Adm_M_Student_EmailIdDTO = (from a in _db.Adm_M_Student
                                                             from b in _db.Adm_M_Student_Email_Id
                                                             where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                             select new Adm_M_Student_EmailIdDTO
                                                             {
                                                                 AMSTE_Id = b.AMSTE_Id,
                                                                 AMST_emailId = b.AMSTE_EmailId
                                                             }).ToArray();


                Adm_M_StudentDTO.Adm_M_Student_ECSDTo = (from a in _db.Adm_M_Student
                                                         from b in _db.Adm_Student_EcsDetailsDMO
                                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == Adm_M_StudentDTO.MI_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                                         select new Adm_M_Student_ECSDTo
                                                         {
                                                             ASECS_Id = b.ASECS_Id,
                                                             AMST_Id = b.AMST_Id,
                                                             ASECS_AccountHolderName = b.ASECS_AccountHolderName,
                                                             ASECS_AccountNo = b.ASECS_AccountNo,
                                                             ASECS_AccountType = b.ASECS_AccountType,
                                                             ASECS_BankName = b.ASECS_BankName,
                                                             ASECS_Branch = b.ASECS_Branch,
                                                             ASECS_MICRNo = b.ASECS_MICRNo
                                                         }).ToArray();


                if (adm_m_student.FirstOrDefault().AMST_FatherNationality != null && adm_m_student.FirstOrDefault().AMST_FatherNationality > 0)
                {
                    Adm_M_StudentDTO.fatherstatelist = _AdmissionFormContext.State.Where(a => a.IVRMMC_Id == adm_m_student.FirstOrDefault().AMST_FatherNationality).
                        ToArray();
                }

                if (adm_m_student.FirstOrDefault().AMST_MotherNationality != null && adm_m_student.FirstOrDefault().AMST_MotherNationality > 0)
                {
                    Adm_M_StudentDTO.motherstatelist = _AdmissionFormContext.State.Where(a => a.IVRMMC_Id == adm_m_student.FirstOrDefault().AMST_MotherNationality).
                        ToArray();
                }

                if (adm_m_student.FirstOrDefault().AMST_PlaceOfBirthCountry != null && adm_m_student.FirstOrDefault().AMST_PlaceOfBirthCountry > 0)
                {
                    Adm_M_StudentDTO.allStateForBirthPlace = _AdmissionFormContext.State.Where(t => t.IVRMMC_Id == adm_m_student.FirstOrDefault().AMST_PlaceOfBirthCountry).ToArray();
                }

                if (adm_m_student.FirstOrDefault().AMST_Nationality != null && adm_m_student.FirstOrDefault().AMST_Nationality > 0)
                {
                    Adm_M_StudentDTO.allStateonchangeNationality = _AdmissionFormContext.State.Where(t => t.IVRMMC_Id == adm_m_student.FirstOrDefault().AMST_Nationality).ToArray();
                }
                Adm_M_StudentDTO.masterstream = _AdmissionFormContext.Adm_School_Master_Stream.Where(a => a.MI_Id == Adm_M_StudentDTO.MI_Id
           && a.ASMST_ActiveFlag == true).OrderBy(a => a.ASMST_Order).ToArray();


                Adm_M_StudentDTO.streamexams = (from a in _db.Adm_School_Master_CE
                                                from b in _db.Adm_School_Stream_Class_CE
                                                where (a.ASMCE_Id == b.ASMCE_Id && a.MI_Id == b.MI_Id && b.ASMCL_Id == adm_m_student.FirstOrDefault().ASMCL_Id && b.ASMST_Id == adm_m_student.FirstOrDefault().ASMST_Id)
                                                select new StudentApplicationDTO
                                                {
                                                    ASMCE_CEName = a.ASMCE_CEName,
                                                    ASMCE_Id = a.ASMCE_Id,
                                                    ASSTCLCE_CompulsoryFlg = b.ASSTCLCE_CompulsoryFlg
                                                }
       ).Distinct().ToArray();

                var streamexams = (from a in _db.Adm_Master_Student_CE
                                   where (a.AMST_Id == adm_m_student.FirstOrDefault().AMST_Id)
                                   select new StudentApplicationDTO
                                   {
                                       ASMCE_Id = a.ASMCE_Id
                                   }
          ).Distinct().ToArray();
                if (streamexams.Count() > 0)
                {
                    Adm_M_StudentDTO.ASMCE_Id = streamexams.FirstOrDefault().ASMCE_Id;
                }


                //Adm_M_StudentDTO.ASMCE_Id = _db.Adm_studentAttendanceStudents.Where(s => s.AMST_Id == Adm_M_StudentDTO.AMST_Id && ).Count();

                Adm_M_StudentDTO.ASMCE_Id = (from a in _db.Adm_studentAttendanceStudents
                                             from b in _db.Adm_studentAttendance
                                             where (a.ASA_Id == b.ASA_Id && b.ASMAY_Id == Adm_M_StudentDTO.ASMAY_Id && a.AMST_Id == Adm_M_StudentDTO.AMST_Id)
                                             select a).Count();
                if (Adm_M_StudentDTO.ASMCE_Id == 0)
                {
                    Adm_M_StudentDTO.ASMCE_Id = _db.Fee_Y_Payment_School_StudentDMO.Where(m => m.ASMAY_Id == Adm_M_StudentDTO.ASMAY_Id && m.AMST_Id == Adm_M_StudentDTO.AMST_Id).Count();
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Adm_M_StudentDTO;
        }

        // save and update activity 
        public Adm_M_StudentDTO ActivityAddUppdate(Adm_M_StudentDTO actadd)
        {
            string str = "0";
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //add & update Activity details
                if (actadd.SelectedActivityDetails != null)
                {
                    foreach (StudentActivityDTO actv in actadd.SelectedActivityDetails)
                    {
                        str = str + "," + actv.AMA_Id;
                    }

                    List<StudentActivityDTO> result = new List<StudentActivityDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getActivity";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = actadd.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = actadd.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@str", SqlDbType.VarChar) { Value = str });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new StudentActivityDTO
                                    {
                                        AMA_Id = Convert.ToInt64(dataReader["AMA_Id"])
                                    });
                                    actadd.activityIds = result.ToArray();
                                }
                            }
                            if (actadd.activityIds != null)
                            {
                                foreach (StudentActivityDTO act in actadd.activityIds)
                                {
                                    var Activityresult = _AdmissionFormContext.StudentActitvityDMO.Where(t => t.AMA_Id == act.AMA_Id && t.AMST_Id == actadd.AMST_Id && t.MI_Id == actadd.MI_Id).ToList();
                                    if (Activityresult.Any())
                                    {
                                        _AdmissionFormContext.Remove(Activityresult.ElementAt(0));
                                        _AdmissionFormContext.SaveChanges();
                                    }
                                }
                            }
                            foreach (StudentActivityDTO mob in actadd.SelectedActivityDetails)
                            {
                                mob.AMST_Id = actadd.AMST_Id;
                                mob.MI_Id = actadd.MI_Id;
                                var Activityresult = _AdmissionFormContext.StudentActitvityDMO.Where(t => t.AMA_Id == mob.AMA_Id && t.MI_Id == actadd.MI_Id && t.AMST_Id == actadd.AMST_Id).ToList();
                                if (Activityresult.Count == 0)
                                {
                                    StudentActitvityDMO Activity = Mapper.Map<StudentActitvityDMO>(mob);
                                    Activity.CreatedDate = indiantime0;
                                    Activity.UpdatedDate = indiantime0;
                                    Activity.AMSA_CreatedBy = actadd.userid;
                                    Activity.AMSA_UpdatedBy = actadd.userid;
                                    _AdmissionFormContext.Add(Activity);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Student Activities  error");
                            _log.LogDebug(ex.Message);
                            _AdmissionFormContext.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Student Activities  error");
                _log.LogDebug(ex.Message);
            }
            return actadd;
        }

        // save and update Refference details
        public Adm_M_StudentDTO RefferenceDetailsAddUpdate(Adm_M_StudentDTO refdetadd)
        {
            string str = "0";
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //add & update Reference details
                if (refdetadd.SelectedRefrenceDetails != null)
                {
                    foreach (StudentReferenceDTO refer in refdetadd.SelectedRefrenceDetails)
                    {
                        str = str + "," + refer.PAMR_Id;
                    }
                    List<StudentReferenceDTO> result = new List<StudentReferenceDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getReference";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = refdetadd.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = refdetadd.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@str", SqlDbType.VarChar) { Value = str });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new StudentReferenceDTO
                                    {
                                        PAMR_Id = Convert.ToInt64(dataReader["PAMR_Id"])
                                    });
                                    refdetadd.referenceIds = result.ToArray();
                                }
                            }
                            if (refdetadd.referenceIds != null)
                            {
                                foreach (StudentReferenceDTO act1 in refdetadd.referenceIds)
                                {
                                    var Referenceresult = _AdmissionFormContext.StudentReferenceDMO.Where(t => t.PAMR_Id == act1.PAMR_Id
                                    && t.MI_Id == refdetadd.MI_Id && t.AMST_Id == refdetadd.AMST_Id).ToList();
                                    if (Referenceresult.Any())
                                    {
                                        _AdmissionFormContext.Remove(Referenceresult.ElementAt(0));
                                        _AdmissionFormContext.SaveChanges();
                                    }
                                }
                            }

                            foreach (StudentReferenceDTO mob in refdetadd.SelectedRefrenceDetails)
                            {
                                mob.AMST_Id = refdetadd.AMST_Id;
                                mob.MI_Id = refdetadd.MI_Id;
                                var Activityresult1 = _AdmissionFormContext.StudentReferenceDMO.Where(t => t.PAMR_Id == mob.PAMR_Id && t.MI_Id == refdetadd.MI_Id && t.AMST_Id == refdetadd.AMST_Id).ToList();
                                if (Activityresult1.Count == 0)
                                {
                                    StudentReferenceDMO Reference = Mapper.Map<StudentReferenceDMO>(mob);
                                    Reference.CreatedDate = indiantime0;
                                    Reference.UpdatedDate = indiantime0;
                                    Reference.AMSTR_CreatedBy = refdetadd.userid;
                                    Reference.AMSTR_UpdatedBy = refdetadd.userid;
                                    _AdmissionFormContext.Add(Reference);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Student Reference  error");
                            _log.LogDebug(ex.Message);
                            _AdmissionFormContext.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception e)
            {

                _log.LogInformation("Student Reference  error");
                _log.LogDebug(e.Message);
            }
            return refdetadd;
        }

        // save and update Source details
        public Adm_M_StudentDTO SourceDetailsAddUpdate(Adm_M_StudentDTO sourcedetadd)
        {
            string str = "0";
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                //add & update Source details
                if (sourcedetadd.SelectedSourceDetails != null)
                {
                    foreach (StudentSourceDTO src in sourcedetadd.SelectedSourceDetails)
                    {
                        str = str + "," + src.PAMS_Id;
                    }
                    List<StudentSourceDTO> result = new List<StudentSourceDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getSource";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = sourcedetadd.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = sourcedetadd.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@str", SqlDbType.VarChar) { Value = str });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new StudentSourceDTO
                                    {
                                        PAMS_Id = Convert.ToInt64(dataReader["PAMS_Id"])
                                    });
                                    sourcedetadd.sourceIds = result.ToArray();
                                }
                            }
                            if (sourcedetadd.sourceIds != null)
                            {
                                foreach (StudentSourceDTO source in sourcedetadd.sourceIds)
                                {
                                    var Sourceresult = _AdmissionFormContext.StudentSourceDMO.Where(t => t.PAMS_Id == source.PAMS_Id && t.MI_Id == sourcedetadd.MI_Id && t.AMST_Id == sourcedetadd.AMST_Id).ToList();
                                    if (Sourceresult.Any())
                                    {
                                        _AdmissionFormContext.Remove(Sourceresult.ElementAt(0));
                                    }
                                }
                                _AdmissionFormContext.SaveChanges();
                            }
                            foreach (StudentSourceDTO mob in sourcedetadd.SelectedSourceDetails)
                            {
                                mob.AMST_Id = sourcedetadd.AMST_Id;
                                mob.MI_Id = sourcedetadd.MI_Id;
                                var Sourceresult1 = _AdmissionFormContext.StudentSourceDMO.Where(t => t.PAMS_Id == mob.PAMS_Id && t.MI_Id == sourcedetadd.MI_Id && t.AMST_Id == sourcedetadd.AMST_Id).ToList();
                                if (Sourceresult1.Count == 0)
                                {
                                    StudentSourceDMO Source = Mapper.Map<StudentSourceDMO>(mob);
                                    Source.CreatedDate = indiantime0;
                                    Source.UpdatedDate = indiantime0;
                                    Source.AMSTS_CreatedBy = sourcedetadd.userid;
                                    Source.AMSTS_UpdatedBy = sourcedetadd.userid;
                                    _AdmissionFormContext.Add(Source);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Student Source  error");
                            _log.LogDebug(ex.Message);
                            _AdmissionFormContext.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Source  error");
                _log.LogDebug(e.Message);
            }
            return sourcedetadd;
        }

        //save and update Siblings details
        public Adm_M_StudentDTO SiblingsAddUpdate(Adm_M_StudentDTO sib)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //add & update Siblings details
                if (sib.SelectedSiblingDetails != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (AdmittedStudentSiblingDTO ph in sib.SelectedSiblingDetails)
                    {
                        temparr.Add(ph.AMSTS_Id);
                    }

                    Array siblings_Noresultremove = _AdmissionFormContext.StudentSiblingDMO.Where(t => !temparr.Contains(t.AMSTS_Id) && t.AMST_Id == sib.AMST_Id).ToArray();

                    foreach (StudentSiblingDMO ph1 in siblings_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    foreach (AdmittedStudentSiblingDTO mob in sib.SelectedSiblingDetails)
                    {
                        if (mob.AMSTS_SiblingsName != null && mob.AMSTS_SiblingsName != "")
                        {
                            mob.AMST_Id = sib.AMST_Id;
                            mob.MI_Id = sib.MI_Id;
                            StudentSiblingDMO sibling = Mapper.Map<StudentSiblingDMO>(mob);
                            if (sibling.AMSTS_Id > 0)
                            {
                                var siblingNoresult = _AdmissionFormContext.StudentSiblingDMO.Single(t => t.AMSTS_Id == mob.AMSTS_Id);
                                siblingNoresult.UpdatedDate = indiantime0;
                                siblingNoresult.AMSTS_UpdatedBy = sib.userid;
                                siblingNoresult.CreatedDate = siblingNoresult.CreatedDate;
                                Mapper.Map(mob, siblingNoresult);
                                _AdmissionFormContext.Update(siblingNoresult);
                            }
                            else
                            {
                                sibling.CreatedDate = indiantime0;
                                sibling.UpdatedDate = indiantime0;
                                sibling.AMSTS_UpdatedBy = sib.userid;
                                sibling.AMSTS_CreatedBy = sib.userid;
                                _AdmissionFormContext.Add(sibling);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student sibling  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return sib;
        }

        // save and update Achivement
        public Adm_M_StudentDTO AchivementAddUpdate(Adm_M_StudentDTO achadd)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                // save Achivement                
                if (achadd.SelectedAchivementDetails != null && achadd.SelectedAchivementDetails != "")
                {
                    List<StudentAchivementDMO> Allname10 = new List<StudentAchivementDMO>();
                    Allname10 = _AdmissionFormContext.StudentAchivementDMO.Where(t => t.MI_Id == achadd.MI_Id && t.AMST_Id == achadd.AMST_Id).ToList();
                    if (Allname10.Count > 0)
                    {
                        var result10 = _AdmissionFormContext.StudentAchivementDMO.Single(t => t.MI_Id == achadd.MI_Id && t.AMST_Id == achadd.AMST_Id);
                        result10.MI_Id = achadd.MI_Id;
                        result10.AMST_Id = achadd.AMST_Id;
                        result10.AMSTEC_Extracurricular = achadd.SelectedAchivementDetails;
                        result10.UpdatedDate = indiantime0;
                        result10.AMSTEC_UpdatedBy = achadd.userid;
                        _AdmissionFormContext.Update(result10);
                    }
                    else
                    {
                        StudentAchivementDMO MM10 = new StudentAchivementDMO();
                        MM10.MI_Id = achadd.MI_Id;
                        MM10.AMST_Id = achadd.AMST_Id;
                        MM10.AMSTEC_Extracurricular = achadd.SelectedAchivementDetails;
                        MM10.CreatedDate = indiantime0;
                        MM10.UpdatedDate = indiantime0;
                        MM10.AMSTEC_CreatedBy = achadd.userid;
                        MM10.AMSTEC_UpdatedBy = achadd.userid;
                        _AdmissionFormContext.Add(MM10);
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Achievement  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return achadd;
        }

        //save and update bond details
        public Adm_M_StudentDTO BondAddUpdate(Adm_M_StudentDTO bondadd)
        {
            string str = "0";
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                //add & update Bond details
                if (bondadd.SelectedBondDetails != null)
                {
                    foreach (GovernmentBondDTO govt in bondadd.SelectedBondDetails)
                    {
                        str = str + "," + govt.IMGB_Id;
                    }
                    List<GovernmentBondDTO> result = new List<GovernmentBondDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getGovtBonds";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = bondadd.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = bondadd.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@str", SqlDbType.VarChar) { Value = str });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new GovernmentBondDTO
                                    {
                                        IMGB_Id = Convert.ToInt64(dataReader["IMGB_Id"])
                                    });
                                    bondadd.bondIds = result.ToArray();
                                }
                            }
                            if (bondadd.bondIds != null)
                            {
                                foreach (GovernmentBondDTO act in bondadd.bondIds)
                                {
                                    var Activityresult = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.IMGB_Id == act.IMGB_Id && t.AMST_Id == bondadd.AMST_Id && t.MI_Id == bondadd.MI_Id).ToList();
                                    if (Activityresult.Any())
                                    {
                                        _AdmissionFormContext.Remove(Activityresult.ElementAt(0));
                                        _AdmissionFormContext.SaveChanges();
                                    }
                                }
                            }
                            foreach (GovernmentBondDTO mob in bondadd.SelectedBondDetails)
                            {
                                mob.AMST_Id = bondadd.AMST_Id;
                                mob.MI_Id = bondadd.MI_Id;

                                var bondresult = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.IMGB_Id == mob.IMGB_Id && t.MI_Id == bondadd.MI_Id && t.AMST_Id == bondadd.AMST_Id).ToList();
                                if (bondresult.Count > 0 && bondresult.FirstOrDefault().AMSTB_BandNo != mob.AMSTB_BandNo)
                                {
                                    var updatedata = _AdmissionFormContext.MasterStudentBondDMO.Single(d => d.AMSTB_Id == bondresult.FirstOrDefault().AMSTB_Id);
                                    bondresult.FirstOrDefault().AMSTB_BandNo = mob.AMSTB_BandNo;
                                    _AdmissionFormContext.Update(updatedata);
                                }
                                else if (bondresult.Count == 0)
                                {
                                    MasterStudentBondDMO Bond = Mapper.Map<MasterStudentBondDMO>(mob);
                                    Bond.AMSTB_BondName = mob.IMGB_Name;
                                    Bond.AMSTB_BandNo = mob.AMSTB_BandNo;
                                    Bond.CreatedDate = indiantime0;
                                    Bond.UpdatedDate = indiantime0;
                                    Bond.AMSTB_CreatedBy = bondadd.userid;
                                    Bond.AMSTB_UpdatedBy = bondadd.userid;
                                    _AdmissionFormContext.Add(Bond);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Student Bond  error");
                            _log.LogDebug(ex.Message);
                            _AdmissionFormContext.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Bond  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return bondadd;
        }

        //save and update Prev School details
        public Adm_M_StudentDTO PrevSchooldetailsAddUpdate(Adm_M_StudentDTO prevadd)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                // save Prev School details
                if (prevadd.SelectedPrevSchoolDetails != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (AdmittedStudentPrevSchoolDTO ph in prevadd.SelectedPrevSchoolDetails)
                    {
                        temparr.Add(ph.AMSTPS_Id);
                    }

                    Array previous_Noresultremove = _AdmissionFormContext.StudentPrevSchoolDMO.Where(t => !temparr.Contains(t.AMSTPS_Id) && t.AMST_Id == prevadd.AMST_Id).ToArray();

                    foreach (StudentPrevSchoolDMO ph1 in previous_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    foreach (AdmittedStudentPrevSchoolDTO mob in prevadd.SelectedPrevSchoolDetails)
                    {
                        if (mob.AMSTPS_PrvSchoolName != null && mob.AMSTPS_PrvSchoolName != "")
                        {
                            mob.AMST_Id = prevadd.AMST_Id;
                            mob.MI_Id = prevadd.MI_Id;
                            StudentPrevSchoolDMO PrevSchool = Mapper.Map<StudentPrevSchoolDMO>(mob);
                            if (PrevSchool.AMSTPS_Id > 0)
                            {
                                var PrevSchoolresult = _AdmissionFormContext.StudentPrevSchoolDMO.Single(t => t.AMSTPS_Id == mob.AMSTPS_Id);
                                PrevSchoolresult.UpdatedDate = indiantime0;
                                // PrevSchoolresult.AMSTPS_UpdatedBy = prevadd.userid;
                                Mapper.Map(mob, PrevSchoolresult);
                                _AdmissionFormContext.Update(PrevSchoolresult);
                            }
                            else
                            {
                                PrevSchool.CreatedDate = indiantime0;
                                PrevSchool.UpdatedDate = indiantime0;
                                //  PrevSchool.AMSTPS_CreatedBy = prevadd.userid;
                                //  PrevSchool.AMSTPS_UpdatedBy = prevadd.userid;
                                _AdmissionFormContext.Add(PrevSchool);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student PrevSchool  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return prevadd;
        }

        //save and update gaurdian details
        public Adm_M_StudentDTO GuardianAddUpdate(Adm_M_StudentDTO guardadd)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (guardadd.SelectedGuardianDetails != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (AdmittedStudentGuardianDTO ph in guardadd.SelectedGuardianDetails)
                    {
                        temparr.Add(ph.AMSTG_Id);
                    }

                    Array guardian_Noresultremove = _AdmissionFormContext.StudentGuardianDMO.Where(t => !temparr.Contains(t.AMSTG_Id) && t.AMST_Id == guardadd.AMST_Id).ToArray();

                    foreach (StudentGuardianDMO ph1 in guardian_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    foreach (AdmittedStudentGuardianDTO mob in guardadd.SelectedGuardianDetails)
                    {
                        if (mob.AMSTG_GuardianName != null && mob.AMSTG_GuardianName != "")
                        {
                            mob.AMST_Id = guardadd.AMST_Id;
                            mob.MI_Id = guardadd.MI_Id;
                            StudentGuardianDMO Guardian = Mapper.Map<StudentGuardianDMO>(mob);
                            if (Guardian.AMSTG_Id > 0)
                            {
                                var Guardianresult = _AdmissionFormContext.StudentGuardianDMO.Single(t => t.AMSTG_Id == mob.AMSTG_Id);
                                Guardianresult.UpdatedDate = indiantime0;
                                Guardianresult.AMSTG_UpdatedBy = guardadd.userid;
                                Guardianresult.AMSTG_GuardianPhoto = mob.AMSTG_GuardianPhoto;
                                Guardianresult.AMSTG_GuardianSign = mob.AMSTG_GuardianSign;
                                Guardianresult.AMSTG_Fingerprint = mob.AMSTG_Fingerprint;
                                Mapper.Map(mob, Guardianresult);
                                _AdmissionFormContext.Update(Guardianresult);
                            }
                            else
                            {
                                Guardian.CreatedDate = indiantime0;
                                Guardian.UpdatedDate = indiantime0;
                                Guardian.AMSTG_UpdatedBy = guardadd.userid;
                                Guardian.AMSTG_CreatedBy = guardadd.userid;
                                _AdmissionFormContext.Add(Guardian);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Student Guardian  error");
                _log.LogDebug(ex.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return guardadd;
        }

        //save and update Documents details
        public Adm_M_StudentDTO stud_doc_upload(Adm_M_StudentDTO docsupld)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                //store student documents
                if (docsupld.Uploaded_documentList.Count() > 0)
                {
                    foreach (MasterDocumentDTO dto in docsupld.Uploaded_documentList)
                    {
                        dto.MI_Id = docsupld.MI_Id;
                        dto.AMST_Id = docsupld.AMST_Id;

                        if (dto.Document_Path != null && dto.Document_Path != "")
                        {
                            StudentDocumentDMO document = new StudentDocumentDMO();
                            if (dto.AMSTD_Id > 0)
                            {
                                var documentNoresult = _AdmissionFormContext.StudentDocumentDMO.Single(t => t.AMSTD_Id == dto.AMSTD_Id);
                                documentNoresult.AMSMD_Id = dto.AMSMD_Id;
                                documentNoresult.AMSTD_DOC_Name = dto.AMSMD_DocumentName;
                                documentNoresult.AMSTD_DOC_Path = dto.Document_Path;
                                documentNoresult.UpdatedDate = indiantime0;
                                documentNoresult.AMSTD_UpdatedBy = docsupld.userid;
                                _AdmissionFormContext.Update(documentNoresult);
                            }
                            else
                            {
                                document.AMSMD_Id = dto.AMSMD_Id;
                                document.AMSTD_DOC_Name = dto.AMSMD_DocumentName;
                                document.AMSTD_DOC_Path = dto.Document_Path;
                                document.AMST_Id = docsupld.AMST_Id;
                                document.MI_Id = docsupld.MI_Id;
                                document.CreatedDate = indiantime0;
                                document.UpdatedDate = indiantime0;
                                document.AMSTD_CreatedBy = docsupld.userid;
                                document.AMSTD_UpdatedBy = docsupld.userid;
                                _AdmissionFormContext.Add(document);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return docsupld;
        }

        //save and update multiple father mobile number
        public Adm_M_StudentDTO father_mobile_no(Adm_M_StudentDTO datamobileno)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (datamobileno.multiplemobileno != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (Adm_M_Student_TempDTO ph in datamobileno.multiplemobileno)
                    {
                        temparr.Add(ph.AMSTFMNO_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _AdmissionFormContext.Adm_M_Student_FatherMobileNo.Where(t => !temparr.Contains(t.AMSTFMNO_Id) && t.AMST_Id == datamobileno.AMST_Id).ToArray();
                    foreach (Adm_M_Student_FatherMobileNo ph1 in Phone_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_M_Student_TempDTO ph in datamobileno.multiplemobileno)
                    {
                        ph.AMST_Id = datamobileno.AMST_Id;
                        ph.MI_Id = datamobileno.MI_Id;
                        Adm_M_Student_FatherMobileNo phone = Mapper.Map<Adm_M_Student_FatherMobileNo>(ph);
                        if (phone.AMSTFMNO_Id > 0)
                        {
                            var Phone_Noresult = _AdmissionFormContext.Adm_M_Student_FatherMobileNo.Single(t => t.AMSTFMNO_Id == ph.AMSTFMNO_Id);
                            Phone_Noresult.UpdatedDate = indiantime0;
                            Phone_Noresult.AMST_FatherMobile_No = ph.AMST_FatherMobleNo;
                            Phone_Noresult.AMSTFMNO_UpdatedBy = datamobileno.userid;
                            Phone_Noresult.ACSTSMS_CoutryCode = ph.ACSTSMS_CoutryCode;
                            Mapper.Map(ph, Phone_Noresult);
                            _AdmissionFormContext.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMST_FatherMobleNo != null)
                            {
                                Adm_M_Student_FatherMobileNo phone1 = Mapper.Map<Adm_M_Student_FatherMobileNo>(ph);
                                phone1.CreatedDate = indiantime0;
                                phone1.UpdatedDate = indiantime0;
                                phone1.AMSTFMNO_UpdatedBy = datamobileno.userid;
                                phone1.AMSTFMNO_CreatedBy = datamobileno.userid;
                                phone1.AMST_FatherMobile_No = ph.AMST_FatherMobleNo;
                                phone1.ACSTSMS_CoutryCode = ph.ACSTSMS_CoutryCode;
                                _AdmissionFormContext.Add(phone1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student_Father_Mobile error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return datamobileno;
        }


        //save and update multiple father email ids
        public Adm_M_StudentDTO father_email_ids(Adm_M_StudentDTO dataemailid)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                // save Prev School details
                if (dataemailid.multipleemailid != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (Adm_M_Student_Eamil ph in dataemailid.multipleemailid)
                    {
                        temparr.Add(ph.AMSTFEMAIL_Id);
                    }

                    //removing email number 
                    Array Phone_Noresultremove = _AdmissionFormContext.Adm_Master_Father_Email.Where(t => !temparr.Contains(t.AMSTFEMAIL_Id) && t.AMST_Id == dataemailid.AMST_Id).ToArray();
                    foreach (Adm_Master_Father_Email ph1 in Phone_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_M_Student_Eamil ph in dataemailid.multipleemailid)
                    {
                        ph.AMST_Id = dataemailid.AMST_Id;
                        ph.MI_Id = dataemailid.MI_Id;
                        Adm_Master_Father_Email phone = Mapper.Map<Adm_Master_Father_Email>(ph);
                        if (phone.AMSTFEMAIL_Id > 0)
                        {
                            var Phone_Noresult = _AdmissionFormContext.Adm_Master_Father_Email.Single(t => t.AMSTFEMAIL_Id == ph.AMSTFEMAIL_Id);
                            Phone_Noresult.UpdatedDate = indiantime0;
                            Phone_Noresult.AMSTFEMAIL_UpdatedBy = dataemailid.userid;
                            Phone_Noresult.AMST_FatheremailId = ph.AMST_FatheremailId;
                            Mapper.Map(ph, Phone_Noresult);
                            _AdmissionFormContext.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMST_FatheremailId != null)
                            {
                                Adm_Master_Father_Email phone1 = Mapper.Map<Adm_Master_Father_Email>(ph);
                                phone1.CreatedDate = indiantime0;
                                phone1.UpdatedDate = indiantime0;
                                phone1.AMSTFEMAIL_CreatedBy = dataemailid.userid;
                                phone1.AMSTFEMAIL_UpdatedBy = dataemailid.userid;
                                phone1.AMST_FatheremailId = ph.AMST_FatheremailId;
                                _AdmissionFormContext.Add(phone1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student_Father_Emailids  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return dataemailid;
        }

        //save and update multiple mother mobile number
        public Adm_M_StudentDTO mother_mobile_no(Adm_M_StudentDTO datamobilenomother)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (datamobilenomother.multiplemobilenomother != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (Adm_M_Mother_MobileNo1 ph in datamobilenomother.multiplemobilenomother)
                    {
                        temparr.Add(ph.AMSTMMNO_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _AdmissionFormContext.Adm_M_Mother_MobileNo.Where(t => !temparr.Contains(t.AMSTMMNO_Id) && t.AMST_Id == datamobilenomother.AMST_Id).ToArray();
                    foreach (Adm_M_Mother_MobileNo ph1 in Phone_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_M_Mother_MobileNo1 ph in datamobilenomother.multiplemobilenomother)
                    {
                        ph.AMST_Id = datamobilenomother.AMST_Id;
                        ph.MI_Id = datamobilenomother.MI_Id;
                        Adm_M_Mother_MobileNo phone = Mapper.Map<Adm_M_Mother_MobileNo>(ph);
                        if (phone.AMSTMMNO_Id > 0)
                        {
                            var Phone_Noresult = _AdmissionFormContext.Adm_M_Mother_MobileNo.Single(t => t.AMSTMMNO_Id == ph.AMSTMMNO_Id);
                            Phone_Noresult.UpdatedDate = indiantime0;
                            Phone_Noresult.AMSTMMNO_UpdatedBy = datamobilenomother.userid;
                            Phone_Noresult.AMST_MotherMobileNo = ph.AMST_MotherMobileNo;
                            Phone_Noresult.AMSTMMNO_CoutryCode = ph.AMSTMMNO_CoutryCode;
                            Mapper.Map(ph, Phone_Noresult);
                            _AdmissionFormContext.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMST_MotherMobileNo != null)
                            {
                                Adm_M_Mother_MobileNo phone1 = Mapper.Map<Adm_M_Mother_MobileNo>(ph);
                                phone1.CreatedDate = indiantime0;
                                phone1.UpdatedDate = indiantime0;
                                phone1.AMST_MotherMobileNo = ph.AMST_MotherMobileNo;
                                phone1.AMSTMMNO_CoutryCode = ph.AMSTMMNO_CoutryCode;
                                phone1.AMSTMMNO_CreatedBy = datamobilenomother.userid;
                                phone1.AMSTMMNO_UpdatedBy = datamobilenomother.userid;
                                _AdmissionFormContext.Add(phone1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student_Father_Mobile error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return datamobilenomother;
        }


        //saving and updating mother email ids
        public Adm_M_StudentDTO mother_email_id(Adm_M_StudentDTO dataemailidmother)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (dataemailidmother.multipleemailidmother != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (Adm_M_Mother_Emailid1 ph in dataemailidmother.multipleemailidmother)
                    {
                        temparr.Add(ph.AMSTMEMAIL_Id);
                    }

                    //removing email number 
                    Array Phone_Noresultremove = _AdmissionFormContext.Adm_M_Mother_Emailid.Where(t => !temparr.Contains(t.AMSTMEMAIL_Id) && t.AMST_Id == dataemailidmother.AMST_Id).ToArray();
                    foreach (Adm_M_Mother_Emailid ph1 in Phone_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_M_Mother_Emailid1 ph in dataemailidmother.multipleemailidmother)
                    {
                        ph.AMST_Id = dataemailidmother.AMST_Id;
                        ph.MI_Id = dataemailidmother.MI_Id;
                        Adm_M_Mother_Emailid phone = Mapper.Map<Adm_M_Mother_Emailid>(ph);
                        if (phone.AMSTMEMAIL_Id > 0)
                        {
                            var Phone_Noresult = _AdmissionFormContext.Adm_M_Mother_Emailid.Single(t => t.AMSTMEMAIL_Id == ph.AMSTMEMAIL_Id);
                            Phone_Noresult.UpdatedDate = indiantime0;
                            Phone_Noresult.AMSTMEMAIL_CreatedBy = dataemailidmother.userid;
                            Phone_Noresult.AMST_MotheremailId = ph.AMST_MotherEmailId;
                            Mapper.Map(ph, Phone_Noresult);
                            _AdmissionFormContext.Update(Phone_Noresult);
                        }
                        else
                        {
                            if (ph.AMST_MotherEmailId != null)
                            {
                                Adm_M_Mother_Emailid phone1 = Mapper.Map<Adm_M_Mother_Emailid>(ph);
                                phone1.CreatedDate = indiantime0;
                                phone1.UpdatedDate = indiantime0;
                                phone1.AMSTMEMAIL_CreatedBy = dataemailidmother.userid;
                                phone1.AMSTMEMAIL_UpdatedBy = dataemailidmother.userid;
                                phone1.AMST_MotheremailId = ph.AMST_MotherEmailId;
                                _AdmissionFormContext.Add(phone1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student_Mother_Emailids  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return dataemailidmother;
        }

        //saving and updating student mobile
        public Adm_M_StudentDTO student_mobile_no(Adm_M_StudentDTO datastdmobile)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (datastdmobile.Adm_M_Student_MobileNoDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all mobilenumbers
                    foreach (Adm_M_Student_MobileNoDTO ph in datastdmobile.Adm_M_Student_MobileNoDTO)
                    {
                        temparr.Add(ph.AMSTSMS_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _AdmissionFormContext.Adm_M_Student_MobileNo.Where(t => !temparr.Contains(t.AMSTSMS_Id) && t.AMST_Id == datastdmobile.AMST_Id).ToArray();
                    foreach (Adm_M_Student_MobileNo ph1 in Phone_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_M_Student_MobileNoDTO ph in datastdmobile.Adm_M_Student_MobileNoDTO)
                    {
                        ph.AMST_Id = datastdmobile.AMST_Id;
                        Adm_M_Student_MobileNo phone = Mapper.Map<Adm_M_Student_MobileNo>(ph);
                        if (phone.AMSTSMS_Id > 0)
                        {
                            var Phone_Noresult = _AdmissionFormContext.Adm_M_Student_MobileNo.Single(t => t.AMSTSMS_Id == ph.AMSTSMS_Id);
                            Phone_Noresult.UpdatedDate = indiantime0;
                            Phone_Noresult.AMSTSMS_UpdatedBy = datastdmobile.userid;
                            Phone_Noresult.AMSTSMS_MobileNo = ph.AMST_MobileNo;
                            Phone_Noresult.AMSTSMS_CountryCode = ph.AMSTSMS_CountryCode;
                            Mapper.Map(ph, Phone_Noresult);
                            _AdmissionFormContext.Update(Phone_Noresult);
                        }
                        else
                        {
                            phone.CreatedDate = indiantime0;
                            phone.UpdatedDate = indiantime0;
                            phone.AMSTSMS_UpdatedBy = datastdmobile.userid;
                            phone.AMSTSMS_CreatedBy = datastdmobile.userid;
                            phone.AMSTSMS_MobileNo = ph.AMST_MobileNo;
                            _AdmissionFormContext.Add(phone);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student_Mobile error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return datastdmobile;
        }

        //saving and updating emailids
        public Adm_M_StudentDTO student_email_id(Adm_M_StudentDTO datastdemail)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (datastdemail.Adm_M_Student_EmailIdDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (Adm_M_Student_EmailIdDTO ph in datastdemail.Adm_M_Student_EmailIdDTO)
                    {
                        temparr.Add(ph.AMSTE_Id);
                    }

                    //removing email number 
                    Array Phone_Noresultremove = _AdmissionFormContext.Adm_M_Student_Email_Id.Where(t => !temparr.Contains(t.AMSTE_Id) && t.AMST_Id == datastdemail.AMST_Id).ToArray();
                    foreach (Adm_M_Student_Email_Id ph1 in Phone_Noresultremove)
                    {
                        _AdmissionFormContext.Remove(ph1);
                    }

                    //updating and saving 

                    foreach (Adm_M_Student_EmailIdDTO ph in datastdemail.Adm_M_Student_EmailIdDTO)
                    {
                        ph.AMST_Id = datastdemail.AMST_Id;

                        Adm_M_Student_Email_Id phone = Mapper.Map<Adm_M_Student_Email_Id>(ph);
                        if (phone.AMSTE_Id > 0)
                        {
                            var Phone_Noresult = _AdmissionFormContext.Adm_M_Student_Email_Id.Single(t => t.AMSTE_Id == ph.AMSTE_Id);
                            Phone_Noresult.UpdatedDate = indiantime0;
                            Phone_Noresult.AMSTE_UpdatedBy = datastdemail.userid;
                            Phone_Noresult.AMSTE_EmailId = ph.AMST_emailId;
                            Mapper.Map(ph, Phone_Noresult);
                            _AdmissionFormContext.Update(Phone_Noresult);
                        }
                        else
                        {
                            phone.CreatedDate = indiantime0;
                            phone.UpdatedDate = indiantime0;
                            phone.AMSTE_UpdatedBy = datastdemail.userid;
                            phone.AMSTE_CreatedBy = datastdemail.userid;
                            phone.AMSTE_EmailId = ph.AMST_emailId;
                            _AdmissionFormContext.Add(phone);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student_Emailids  error");
                _log.LogDebug(e.Message);
                _AdmissionFormContext.Database.RollbackTransaction();
            }
            return datastdemail;
        }
        public Adm_M_StudentDTO DeleteBondEntry(int ID)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            MasterStudentBondDMO MasterStudentBondDMO = new MasterStudentBondDMO();

            List<MasterStudentBondDMO> masters11 = new List<MasterStudentBondDMO>();
            masters11 = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.AMSTB_Id.Equals(ID)).ToList();

            if (masters11.Any())
            {
                for (int i = 0; i < masters11.Count; i++)
                {
                    _AdmissionFormContext.Remove(masters11.ElementAt(i));
                    _AdmissionFormContext.SaveChanges();
                }
            }

            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO checkDuplicate(Adm_M_StudentDTO dto)
        {

            try

            {

                if (dto.AMST_Id > 0)
                {
                    var studRecord = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_Id == dto.AMST_Id).ToList();

                    var duplicateaadhar = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_AadharNo == dto.AMST_AadharNo && t.MI_Id == dto.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL.Equals("S") && t.AMST_Id != dto.AMST_Id).ToList();
                    if (duplicateaadhar.Count > 0)
                    {
                        dto.duplicateEmailId = duplicateaadhar.Count;
                    }
                    else
                    {
                        dto.duplicateEmailId = 0;
                    }

                    //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                    if (dto.admRegManualFlag == "true" && dto.preventdupRegNo == "Yes")
                    {
                        dto.duplicateRegNo = _AdmissionFormContext.Adm_M_Student.Where(d => d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL.Equals("S") && d.AMST_RegistrationNo.Equals(dto.AMST_RegistrationNo) && d.AMST_Id != dto.AMST_Id).Count();

                    }
                    if (dto.admAdmManualFlag == "true" && dto.preventdupAdmNo == "Yes")
                    {
                        dto.duplicateAdmNo = _AdmissionFormContext.Adm_M_Student.Where(d => d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL.Equals("S") && d.AMST_AdmNo.Equals(dto.AMST_AdmNo) && d.AMST_Id != dto.AMST_Id).Count();
                    }
                }
                else
                {
                    dto.duplicateAdharNo = _AdmissionFormContext.Adm_M_Student.Where(d => d.AMST_AadharNo == dto.AMST_AadharNo && d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL.Equals("S")).ToList().Count;

                    //Check Duplicate Adm.No Or Reg.No.If set to prevent duplicate in Transaction Numbering.
                    if (dto.admRegManualFlag == "true" && dto.preventdupRegNo == "Yes")
                    {
                        dto.duplicateRegNo = _AdmissionFormContext.Adm_M_Student.Where(d => d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL.Equals("S") && d.AMST_RegistrationNo.Equals(dto.AMST_RegistrationNo)).Count();
                    }

                    if (dto.admAdmManualFlag == "true" && dto.preventdupAdmNo == "Yes")
                    {
                        dto.duplicateAdmNo = _AdmissionFormContext.Adm_M_Student.Where(d => d.MI_Id == dto.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL.Equals("S") && d.AMST_AdmNo.Equals(dto.AMST_AdmNo)).Count();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public Adm_M_StudentDTO DeleteEntry(Adm_M_StudentDTO Adm_M_StudentDTO)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkamst_id = _AdmissionFormContext.SchoolYearWiseStudent.Where(t => t.AMST_Id == Adm_M_StudentDTO.AMST_Id && t.AMAY_ActiveFlag == 1).ToList();
                if (checkamst_id.Count > 0)
                {
                    Adm_M_StudentDTO.message = "Sorry...You Can't Delete This Record.Section Is Already Allotted For This Student";
                }
                else
                {
                    var result = _AdmissionFormContext.Adm_M_Student.Single(t => t.AMST_Id == Adm_M_StudentDTO.AMST_Id);
                    result.AMST_ActiveFlag = 0;
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = Adm_M_StudentDTO.userid;
                    result.AMST_SOL = "Del";
                    _AdmissionFormContext.Update(result);
                    var flag = _AdmissionFormContext.SaveChanges();
                    if (flag > 0)
                    {


                        Adm_M_StudentDTO.returnval = true;
                        var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == result.MI_Id);

                        string emailids = "";
                        long mobilenos = 0;
                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                        {
                            emailids = result.AMST_MotherEmailId;
                            mobilenos = Convert.ToInt64(result.AMST_MotherMobileNo);
                        }
                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                        {
                            emailids = result.AMST_FatheremailId;
                            mobilenos = Convert.ToInt64(result.AMST_FatherMobleNo);
                        }
                        else
                        {
                            emailids = result.AMST_emailId;
                            mobilenos = Convert.ToInt64(result.AMST_MobileNo);
                        }

                        try
                        {
                            SMS sms = new SMS(_db);
                            string s = sms.sendSms(result.MI_Id, mobilenos, "ADMISSION_REGISTRATION_DELETE", result.AMST_Id).Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        try
                        {
                            Email Email = new Email(_db);
                            string m = Email.sendmail(result.MI_Id, emailids, "ADMISSION_REGISTRATION_DELETE", result.AMST_Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Adm_M_StudentDTO.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error in Student deletion");
                _log.LogDebug(ex.Message);
            }
            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO getcaste(Adm_M_StudentDTO dto)
        {
            try
            {
                if (dto.IMCC_Id != null)
                {
                    dto.AllCaste = (from m in _AdmissionFormContext.CasteCategory
                                    from n in _AdmissionFormContext.Caste
                                    where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == dto.IMCC_Id && n.MI_Id == dto.MI_Id
                                    select new Adm_M_StudentDTO
                                    {
                                        IMC_Id = n.IMC_Id,
                                        IMC_CasteName = n.IMC_CasteName
                                    }).ToArray();
                }
                else
                {
                    dto.AllCaste = _AdmissionFormContext.Caste.Where(a => a.MI_Id == dto.MI_Id).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public Adm_M_StudentDTO checkbiometriccode(Adm_M_StudentDTO data)
        {
            try
            {
                if (data.AMST_Id > 0)
                {
                    var checkresult = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_BiometricId == data.AMST_BiometricId
                    && a.AMST_Id != data.AMST_Id).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var checkresult = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_BiometricId == data.AMST_BiometricId).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO checkrfcardduplicate(Adm_M_StudentDTO data)
        {
            try
            {
                if (data.AMST_Id > 0)
                {
                    var checkresult = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_RFCardNo == data.AMST_RFCardNo
                    && a.AMST_Id != data.AMST_Id).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var checkresult = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_RFCardNo == data.AMST_RFCardNo).ToList();

                    if (checkresult.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO onchangefathernationality(Adm_M_StudentDTO data)
        {
            try
            {
                data.fatherstatelist = _AdmissionFormContext.State.Where(a => a.IVRMMC_Id == data.AMST_FatherNationality).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Adm_M_StudentDTO onchangemothernationality(Adm_M_StudentDTO data)
        {
            try
            {
                data.motherstatelist = _AdmissionFormContext.State.Where(a => a.IVRMMC_Id == data.AMST_MotherNationality).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //getting the student how taken the tc 
        public Adm_M_StudentDTO yearwisetcstd(Adm_M_StudentDTO data)
        {
            try
            {
                var checkflag = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();


                if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         where (a.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ":" + (m.AMST_AdmNo == null ? " " : m.AMST_AdmNo)).Trim(),

                                         }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         where (a.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                         }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         where (a.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((m.AMST_AdmNo == null ? " " : m.AMST_AdmNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                         }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         where (a.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                         }).Distinct().ToArray();
                }
                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         from n in _AdmissionFormContext.SchoolYearWiseStudent
                                         where (a.AMST_Id == m.AMST_Id && n.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString()) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                         }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         from n in _AdmissionFormContext.SchoolYearWiseStudent
                                         where (a.AMST_Id == m.AMST_Id && n.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString())).Trim(),

                                         }).Distinct().ToArray();
                }

                else
                {
                    data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                                         from m in _AdmissionFormContext.Adm_M_Student
                                         where (a.AMST_Id == m.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                                         select new Adm_M_StudentDTO
                                         {
                                             AMST_Id = m.AMST_Id,
                                             AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                         }).Distinct().ToArray();
                }

                //data.StudentList1 = (from a in _AdmissionFormContext.Student_TC
                //                     from b in _AdmissionFormContext.Adm_M_Student
                //                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == data.ASMAY_Id)
                //                     select new Adm_M_StudentDTO
                //                     {
                //                         AMST_Id = b.AMST_Id,
                //                         AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                     }
                //                   ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Student admission/readmit-TC form error");
                _log.LogDebug(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public Adm_M_StudentDTO addtocart(Adm_M_StudentDTO ID)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();

            List<MasterStudentBondDMO> MasterStudentBondDMO = new List<MasterStudentBondDMO>();
            MasterStudentBondDMO = _AdmissionFormContext.MasterStudentBondDMO.Where(t => t.AMST_Id.Equals(ID.AMST_Id)).ToList().ToList();
            Adm_M_StudentDTO.BondList = MasterStudentBondDMO.ToArray();

            List<StudentPrevSchoolDMO> StudentPrevSchoolDMO = new List<StudentPrevSchoolDMO>();
            StudentPrevSchoolDMO = _AdmissionFormContext.StudentPrevSchoolDMO.Where(t => t.AMST_Id.Equals(ID.AMST_Id)).ToList().ToList();
            Adm_M_StudentDTO.PrevSchoolDetails = StudentPrevSchoolDMO.ToArray();

            List<StudentGuardianDMO> StudentGuardianDMO = new List<StudentGuardianDMO>();
            StudentGuardianDMO = _AdmissionFormContext.StudentGuardianDMO.Where(t => t.AMST_Id.Equals(ID.AMST_Id)).ToList().ToList();
            Adm_M_StudentDTO.StudentGuardianDetails = StudentGuardianDMO.ToArray();

            List<StudentSiblingDMO> StudentSiblingDMO = new List<StudentSiblingDMO>();
            StudentSiblingDMO = _AdmissionFormContext.StudentSiblingDMO.Where(t => t.AMST_Id.Equals(ID.AMST_Id)).ToList().ToList();
            Adm_M_StudentDTO.StudentSiblingDetails = StudentSiblingDMO.ToArray();

            List<Adm_M_Student> adm_m_student = new List<Adm_M_Student>();
            adm_m_student = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_Id == ID.AMST_Id).ToList();
            Adm_M_StudentDTO.adm_m_student = adm_m_student.ToArray();

            //Adm_M_StudentDTO.AllCaste = (from m in _AdmissionFormContext.CasteCategory
            //                             from n in _AdmissionFormContext.Caste
            //                             where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == adm_m_student.FirstOrDefault().IMCC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
            //                             select new Adm_M_StudentDTO
            //                             {
            //                                 IMC_Id = n.IMC_Id,
            //                                 IMC_CasteName = n.IMC_CasteName
            //                             }).ToArray();

            if (adm_m_student.FirstOrDefault().IMCC_Id != null && adm_m_student.FirstOrDefault().IMCC_Id > 0)
            {

                Adm_M_StudentDTO.AllCaste = (from m in _AdmissionFormContext.CasteCategory
                                             from n in _AdmissionFormContext.Caste
                                             where m.IMCC_Id == n.IMCC_Id && n.IMCC_Id == adm_m_student.FirstOrDefault().IMCC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                             select new Adm_M_StudentDTO
                                             {
                                                 IMC_Id = n.IMC_Id,
                                                 IMC_CasteName = n.IMC_CasteName
                                             }).ToArray();
            }
            else
            {
                Adm_M_StudentDTO.AllCaste = (from m in _AdmissionFormContext.CasteCategory
                                             from n in _AdmissionFormContext.Caste
                                             where m.IMCC_Id == n.IMCC_Id && n.MI_Id == adm_m_student.FirstOrDefault().MI_Id
                                             select new Adm_M_StudentDTO
                                             {
                                                 IMC_Id = n.IMC_Id,
                                                 IMC_CasteName = n.IMC_CasteName
                                             }).ToArray();
            }


            var studephoto = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_Id == ID.AMST_Id).Select(t => t.AMST_Photoname).FirstOrDefault();
            Adm_M_StudentDTO.AMST_Photoname = studephoto;
            Adm_M_StudentDTO.AMST_Father_FingerPrint = adm_m_student.FirstOrDefault().AMST_Father_FingerPrint;
            Adm_M_StudentDTO.AMST_Father_Signature = adm_m_student.FirstOrDefault().AMST_Father_Signature;
            Adm_M_StudentDTO.ANST_FatherPhoto = adm_m_student.FirstOrDefault().ANST_FatherPhoto;
            Adm_M_StudentDTO.ANST_MotherPhoto = adm_m_student.FirstOrDefault().ANST_MotherPhoto;
            Adm_M_StudentDTO.AMST_Mother_Signature = adm_m_student.FirstOrDefault().AMST_Mother_Signature;
            Adm_M_StudentDTO.AMST_Mother_FingerPrint = adm_m_student.FirstOrDefault().AMST_Mother_FingerPrint;

            List<MasterDocumentDTO> result = new List<MasterDocumentDTO>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Student_Documents";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                {
                    Value = adm_m_student.FirstOrDefault().MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.BigInt)
                {
                    Value = ID.AMST_Id
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
                            result.Add(new MasterDocumentDTO
                            {
                                AMSMD_DocumentName = dataReader["AMSMD_DocumentName"].ToString(),
                                AMSMD_Id = Convert.ToInt64(dataReader["AMSMD_Id"]),
                                AMSMD_FLAG = Convert.ToBoolean(dataReader["AMSMD_FLAG"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            var StudDocumentList = (from sp in _AdmissionFormContext.StudentDocumentDMO
                                    from cp in _AdmissionFormContext.MasterDocumentDMO
                                    where (sp.AMSMD_Id == cp.AMSMD_Id && sp.AMST_Id == ID.AMST_Id)
                                    select new MasterDocumentDTO
                                    {
                                        AMSMD_DocumentName = cp.AMSMD_DocumentName,
                                        AMSMD_Id = cp.AMSMD_Id,
                                        AMSTD_Id = sp.AMSTD_Id,
                                        AMST_Id = sp.AMST_Id,
                                        Document_Path = sp.AMSTD_DOC_Path,
                                        AMSMD_FLAG = cp.AMSMD_FLAG
                                    }).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                StudDocumentList.Add(result[i]);
            }
            Adm_M_StudentDTO.DocumentList = StudDocumentList.ToArray();

            List<StudentAchivementDMO> StudentAchivementDMO = new List<StudentAchivementDMO>();
            StudentAchivementDMO = _AdmissionFormContext.StudentAchivementDMO.Where(t => t.AMST_Id.Equals(ID)).ToList();
            Adm_M_StudentDTO.StudentAchivementDetails = StudentAchivementDMO.ToArray();

            List<StudentReferenceDMO> StudentReferenceDMO = new List<StudentReferenceDMO>();
            StudentReferenceDMO = _AdmissionFormContext.StudentReferenceDMO.Where(t => t.AMST_Id.Equals(ID)).ToList();
            Adm_M_StudentDTO.StudentReferenceDetails = StudentReferenceDMO.ToArray();

            List<StudentSourceDMO> StudentSourceDMO = new List<StudentSourceDMO>();
            StudentSourceDMO = _AdmissionFormContext.StudentSourceDMO.Where(t => t.AMST_Id.Equals(ID)).ToList();
            Adm_M_StudentDTO.StudentSourceDetails = StudentSourceDMO.ToArray();

            List<StudentActitvityDMO> StudentActitvityDMO = new List<StudentActitvityDMO>();
            StudentActitvityDMO = _AdmissionFormContext.StudentActitvityDMO.Where(t => t.AMST_Id.Equals(ID)).ToList();
            Adm_M_StudentDTO.StudentActivityDetails = StudentActitvityDMO.ToArray();

            var asmccid = _AdmissionFormContext.Adm_M_Student.Where(t => t.AMST_Id == ID.AMST_Id).ToList();

            Adm_M_StudentDTO.stud_catg_edit = (from m in _db.mastercategory
                                               from n in _db.Masterclasscategory
                                               where m.AMC_Id == n.AMC_Id && n.ASMCC_Id == asmccid.FirstOrDefault().AMC_Id
                                               select new MasterClassCategoryDTO
                                               {
                                                   ASMCC_Id = n.ASMCC_Id,
                                                   className = m.AMC_Name
                                               }
             ).ToArray();



            Adm_M_StudentDTO.multiplemobileno = (from a in _db.Adm_M_Student
                                                 from b in _db.Adm_M_Student_FatherMobileNo
                                                 where (a.AMST_Id == b.AMST_Id && a.MI_Id == ID.MI_Id && a.AMST_Id == ID.AMST_Id)
                                                 select new Adm_M_Student_TempDTO
                                                 {
                                                     AMSTFMNO_Id = b.AMSTFMNO_Id,
                                                     AMST_FatherMobleNo = b.AMST_FatherMobile_No,
                                                     ACSTSMS_CoutryCode = b.ACSTSMS_CoutryCode
                                                 }).ToArray();

            Adm_M_StudentDTO.multipleemailid = (from a in _db.Adm_M_Student
                                                from b in _db.Adm_Master_Father_Email
                                                where (a.AMST_Id == b.AMST_Id && a.MI_Id == ID.MI_Id && a.AMST_Id == ID.AMST_Id)
                                                select new Adm_M_Student_Eamil
                                                {
                                                    AMSTFEMAIL_Id = b.AMSTFEMAIL_Id,
                                                    AMST_FatheremailId = b.AMST_FatheremailId
                                                }).ToArray();


            Adm_M_StudentDTO.multiplemobilenomother = (from a in _db.Adm_M_Student
                                                       from b in _db.Adm_M_Mother_MobileNo
                                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == ID.MI_Id && a.AMST_Id == ID.AMST_Id)
                                                       select new Adm_M_Mother_MobileNo1
                                                       {
                                                           AMSTMMNO_Id = b.AMSTMMNO_Id,
                                                           AMST_MotherMobileNo = b.AMST_MotherMobileNo,
                                                           AMSTMMNO_CoutryCode = b.AMSTMMNO_CoutryCode

                                                       }).ToArray();


            Adm_M_StudentDTO.multipleemailidmother = (from a in _db.Adm_M_Student
                                                      from b in _db.Adm_M_Mother_Emailid
                                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == ID.MI_Id && a.AMST_Id == ID.AMST_Id)
                                                      select new Adm_M_Mother_Emailid1
                                                      {
                                                          AMSTMEMAIL_Id = b.AMSTMEMAIL_Id,
                                                          AMST_MotherEmailId = b.AMST_MotheremailId
                                                      }).ToArray();

            Adm_M_StudentDTO.Adm_M_Student_MobileNoDTO = (from a in _db.Adm_M_Student
                                                          from b in _db.Adm_M_Student_MobileNo
                                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == ID.MI_Id && a.AMST_Id == ID.AMST_Id)
                                                          select new Adm_M_Student_MobileNoDTO
                                                          {
                                                              AMSTSMS_Id = b.AMSTSMS_Id,
                                                              AMST_MobileNo = b.AMSTSMS_MobileNo
                                                          }).ToArray();

            Adm_M_StudentDTO.Adm_M_Student_EmailIdDTO = (from a in _db.Adm_M_Student
                                                         from b in _db.Adm_M_Student_Email_Id
                                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == ID.MI_Id && a.AMST_Id == ID.AMST_Id)
                                                         select new Adm_M_Student_EmailIdDTO
                                                         {
                                                             AMSTE_Id = b.AMSTE_Id,
                                                             AMST_emailId = b.AMSTE_EmailId
                                                         }).ToArray();

            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO searchByColumn(Adm_M_StudentDTO adm)
        {
            try
            {
                adm.EnteredData = adm.EnteredData.ToUpper();
                //string mobno = "";
                //string email = "";
                List<Adm_M_Student> students = new List<Adm_M_Student>();
                List<long> list11 = new List<long>();
                if (adm.SearchColumn == "" || adm.SearchColumn == null)
                {
                    adm.SearchColumn = "0";
                }
                long asmayId = 0;
                List<Adm_M_StudentDTO> adm_m_student1 = new List<Adm_M_StudentDTO>();
                List<Adm_M_Student_TempMobileNo> query = new List<Adm_M_Student_TempMobileNo>();
                if (adm.ASMAY_Id == 0)
                {
                    switch (adm.SearchColumn)
                    {
                        case "0":
                            //adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                            //                  from cls in _AdmissionFormContext.School_M_Class
                            //                  where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del"
                            //                  && (adm_stu.AMST_FirstName.ToUpper().Contains(adm.EnteredData)
                            //                  || adm_stu.AMST_MiddleName.ToUpper().Contains(adm.EnteredData)
                            //                  || adm_stu.AMST_LastName.ToUpper().Contains(adm.EnteredData)
                            //                  || (adm_stu.AMST_FirstName.ToUpper().Trim() + ' ' + adm_stu.AMST_LastName.ToUpper().Trim()).Contains(adm.EnteredData)
                            //                  || (adm_stu.AMST_FirstName.ToUpper().Trim() + ' ' + adm_stu.AMST_MiddleName.ToUpper().Trim() + ' '
                            //                  + adm_stu.AMST_LastName.ToUpper().Trim()).
                            //                  Contains(adm.EnteredData)))
                            //                  group new { adm_stu, cls }
                            //                   by new { adm_stu.AMST_Id } into g
                            //                  select new Adm_M_StudentDTO
                            //                  {
                            //                      AMST_Date = g.FirstOrDefault().adm_stu.AMST_Date,
                            //                      AMST_Sex = g.FirstOrDefault().adm_stu.AMST_Sex,
                            //                      AMST_RegistrationNo = g.FirstOrDefault().adm_stu.AMST_RegistrationNo,
                            //                      AMST_AdmNo = g.FirstOrDefault().adm_stu.AMST_AdmNo,
                            //                      AMST_emailId = g.FirstOrDefault().adm_stu.AMST_emailId,
                            //                      stdmobilenumber = Convert.ToString(g.FirstOrDefault().adm_stu.AMST_MobileNo),
                            //                      AMST_Id = g.FirstOrDefault().adm_stu.AMST_Id,
                            //                      Class = g.FirstOrDefault().cls.ASMCL_ClassName,
                            //                      AMST_SOL = g.FirstOrDefault().adm_stu.AMST_SOL,
                            //                      AMST_Photoname = g.FirstOrDefault().adm_stu.AMST_Photoname,
                            //                      studentname = ((g.FirstOrDefault().adm_stu.AMST_FirstName == null || g.FirstOrDefault().adm_stu.AMST_FirstName == "" ?
                            //                      "" : g.FirstOrDefault().adm_stu.AMST_FirstName) + (g.FirstOrDefault().adm_stu.AMST_MiddleName == null ||
                            //                      g.FirstOrDefault().adm_stu.AMST_MiddleName == "" ? "" : " " + g.FirstOrDefault().adm_stu.AMST_MiddleName) +
                            //                      (g.FirstOrDefault().adm_stu.AMST_LastName == null || g.FirstOrDefault().adm_stu.AMST_LastName == "" ? "" : " " +
                            //                      g.FirstOrDefault().adm_stu.AMST_LastName)).Trim()
                            //                  }).ToList();


                            using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "StudentName_Search";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                                {
                                    Value = adm.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                SqlDbType.VarChar)
                                {
                                    Value = adm.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@EnteredData",
                              SqlDbType.VarChar)
                                {
                                    Value = adm.EnteredData
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

                                            adm_m_student1.Add(new Adm_M_StudentDTO
                                            {
                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                AMST_Date = Convert.ToDateTime(dataReader["AMST_Date"]),
                                                AMST_Sex = Convert.ToString(dataReader["AMST_Sex"]),
                                                AMST_RegistrationNo = Convert.ToString(dataReader["AMST_RegistrationNo"]),
                                                AMST_AdmNo = Convert.ToString(dataReader["AMST_AdmNo"]),
                                                AMST_emailId = Convert.ToString(dataReader["AMST_emailId"]),
                                                stdmobilenumber = Convert.ToString(dataReader["AMST_MobileNo"]),
                                                Class = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                                AMST_SOL = Convert.ToString(dataReader["AMST_SOL"]),
                                                studentname = Convert.ToString(dataReader["studentname"]),
                                                AMST_Photoname = Convert.ToString(dataReader["AMST_Photoname"])
                                            });
                                        }
                                    }
                                    //   adm.studentlist123 = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }






                            //multiple mobile number feteching   start
                            //var query = (from a in _AdmissionFormContext.Adm_M_Student
                            //             from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                            //             where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_SOL != "Del"
                            //             && (a.AMST_FirstName.ToUpper().Contains(adm.EnteredData)
                            //                  || a.AMST_MiddleName.ToUpper().Contains(adm.EnteredData)
                            //                  || a.AMST_LastName.ToUpper().Contains(adm.EnteredData)
                            //                  || (a.AMST_FirstName.ToUpper().Trim() + ' ' + a.AMST_LastName.ToUpper().Trim()).Contains(adm.EnteredData)
                            //                  || (a.AMST_FirstName.ToUpper().Trim() + ' ' + a.AMST_MiddleName.ToUpper().Trim() + ' '
                            //                  + a.AMST_LastName.ToUpper().Trim()).
                            //                  Contains(adm.EnteredData)))
                            //             select new Adm_M_Student_TempMobileNo
                            //             {
                            //                 UserName = a.AMST_Id,
                            //                 Role = b.AMSTSMS_MobileNo
                            //             }).ToList();


                            using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "StudentMobileNo_Search";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                                {
                                    Value = adm.MI_Id
                                });
                                // cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                //SqlDbType.VarChar)
                                // {
                                //     Value = adm.ASMAY_Id
                                // });

                                cmd.Parameters.Add(new SqlParameter("@EnteredData",
                              SqlDbType.VarChar)
                                {
                                    Value = adm.EnteredData
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

                                            query.Add(new Adm_M_Student_TempMobileNo
                                            {
                                                UserName = Convert.ToInt64(dataReader["AMST_Id"]),
                                                Role = Convert.ToString(dataReader["AMSTSMS_MobileNo"])

                                            });
                                        }
                                    }
                                    //   adm.studentlist123 = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            int count = query.Count() + 1;
                            Adm_M_Student_TempMobileNo[] temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            string value = null;
                            Dictionary<long, string> tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            List<Adm_M_Student_TempMobileNo> list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            var query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                          from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMST_Id,
                                              Roleemail = b.AMSTE_EmailId
                                          }).ToList();


                            int count1 = query1.Count() + 1;
                            Adm_M_Student_TempEmailId[] temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            string value1 = null;
                            Dictionary<long, string> tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            List<Adm_M_Student_TempEmailId> list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }



                            break;
                        case "1":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del"
                                              //&&  adm_stu.AMST_ActiveFlag == 1 
                                              && adm_stu.AMST_FirstName.ToUpper().Contains(adm.EnteredData))

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname
                                              }).OrderByDescending(d => d.AMST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_FirstName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_FirstName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "2":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.AMST_SOL != "Del"
                                              //&& adm_stu.AMST_ActiveFlag == 1 
                                              && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_MiddleName.ToUpper().Contains(adm.EnteredData))
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_MiddleName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_MiddleName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "3":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_LastName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1 
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_LastName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_LastName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "4":
                            try
                            {
                                DateTime date = DateTime.ParseExact(adm.EnteredData, "dd/MM/yyyy",
                                   CultureInfo.InvariantCulture);

                                adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                                  from cls in _AdmissionFormContext.School_M_Class
                                                  where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.AMST_SOL != "Del" && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                  // && adm_stu.AMST_ActiveFlag == 1

                                                  select new Adm_M_StudentDTO
                                                  {
                                                      //AMST_FirstName = adm_stu.AMST_FirstName,
                                                      //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                      //AMST_LastName = adm_stu.AMST_LastName,
                                                      AMST_Date = adm_stu.AMST_Date,
                                                      AMST_Sex = adm_stu.AMST_Sex,
                                                      AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                      AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                      AMST_emailId = adm_stu.AMST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                      AMST_Id = adm_stu.AMST_Id,
                                                      Class = cls.ASMCL_ClassName,
                                                      AMST_SOL = adm_stu.AMST_SOL,
                                                      AMST_Photoname = adm_stu.AMST_Photoname,
                                                      studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMST_Id).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _AdmissionFormContext.Adm_M_Student
                                         from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMST_Id,
                                             Role = b.AMSTSMS_MobileNo
                                         }).OrderByDescending(d => d.UserName).ToList();

                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {
                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }
                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                //end

                                //multiple email ids start 
                                query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                          from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_SOL != "Del" && a.AMST_ActiveFlag == 0 && a.AMST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMST_Id,
                                              Roleemail = b.AMSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }

                                //end
                                adm.adm_m_student = adm_m_student1.ToArray();

                                if (adm.adm_m_student.Length > 0)
                                {
                                    adm.count = adm.adm_m_student.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                adm.message = "Please Enter date in dd/MM/yyyy format";
                                adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                                  from cls in _AdmissionFormContext.School_M_Class
                                                  where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del")
                                                  //&& adm_stu.AMST_ActiveFlag == 1
                                                  select new Adm_M_StudentDTO
                                                  {
                                                      //AMST_FirstName = adm_stu.AMST_FirstName,
                                                      //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                      //AMST_LastName = adm_stu.AMST_LastName,
                                                      AMST_Date = adm_stu.AMST_Date,
                                                      AMST_Sex = adm_stu.AMST_Sex,
                                                      AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                      AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                      AMST_emailId = adm_stu.AMST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                      AMST_Id = adm_stu.AMST_Id,
                                                      Class = cls.ASMCL_ClassName,
                                                      AMST_SOL = adm_stu.AMST_SOL,
                                                      AMST_Photoname = adm_stu.AMST_Photoname,
                                                      studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMST_Id).Take(10).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _AdmissionFormContext.Adm_M_Student
                                         from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMST_Id,
                                             Role = b.AMSTSMS_MobileNo
                                         }).OrderByDescending(d => d.UserName).Take(10).ToList();
                                //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                                //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {

                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }

                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }

                                //end

                                //multiple email ids start 
                                query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                          from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMST_Id,
                                              Roleemail = b.AMSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }

                                adm.adm_m_student = adm_m_student1.ToArray();

                                if (adm.adm_m_student.Length > 0)
                                {
                                    adm.count = adm.adm_m_student.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }
                            }

                            break;
                        case "5":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.AMST_SOL != "Del" && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_Sex.ToUpper().Equals(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Sex.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Sex.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "6":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_RegistrationNo.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_RegistrationNo.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_RegistrationNo.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "7":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_AdmNo.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()

                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_AdmNo.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_AdmNo.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "8":

                            List<Adm_M_StudentDTO> GrpId = new List<Adm_M_StudentDTO>();

                            // List<long> list11 = new List<long>();

                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.ASMAY_Id == adm.ASMAY_Id && b.AMSTE_EmailId.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            for (int k = 0; k < list1.Count; k++)
                            {
                                list11.Add(list1[k].UserNameemail);
                            }



                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && list11.Contains(adm_stu.AMST_Id))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && list11.Contains(a.AMST_Id))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end                       

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "9":
                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.ASMAY_Id == adm.ASMAY_Id && b.AMSTSMS_MobileNo.ToString().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();

                            for (int k = 0; k < list.Count; k++)
                            {
                                list11.Add(list[k].UserName);
                            }

                            //end here 
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && list11.Contains(adm_stu.AMST_Id))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()

                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && list11.Contains(a.AMST_Id))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }
                            //end

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "10":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && cls.ASMCL_ClassName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     from c in _AdmissionFormContext.School_M_Class
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMCL_Id == c.ASMCL_Id && c.ASMCL_ClassName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      from c in _AdmissionFormContext.School_M_Class
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMCL_Id == c.ASMCL_Id && c.ASMCL_ClassName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "11":
                            if (adm.EnteredData.Equals("active", StringComparison.OrdinalIgnoreCase))

                            {
                                adm.EnteredData = "S";
                            }
                            else if (adm.EnteredData.Equals("deactive", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "D";
                            }
                            else if (adm.EnteredData.Equals("left", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "L";
                            }
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_SOL.ToUpper().Equals(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_SOL.ToUpper().Equals(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_SOL.ToUpper().Equals(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();



                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "12":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_FatherName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_FatherName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_FatherName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "13":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_MotherName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_MotherName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_MotherName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();



                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                        case "14":
                            var academicYearId = _db.AcademicYear.Where(d => d.ASMAY_Year.Equals(adm.EnteredData) && d.MI_Id == adm.MI_Id).ToList();
                            if (academicYearId.Count > 0)
                            {
                                asmayId = academicYearId.FirstOrDefault().ASMAY_Id;
                            }
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == asmayId)
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();
                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                        case "15":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_BPLCardNo.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_BPLCardNo.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_BPLCardNo.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                        default:
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del")
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).Take(10).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).Take(10).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();
                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                    }
                }

                //-----------------Else part is when search option is based on the year------------------//
                else
                {
                    switch (adm.SearchColumn)
                    {
                        case "0":
                            //adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                            //                  from cls in _AdmissionFormContext.School_M_Class
                            //                  where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id
                            //                  && adm_stu.AMST_SOL != "Del" &&
                            //                  (adm_stu.AMST_FirstName.ToUpper().Contains(adm.EnteredData)
                            //                  || adm_stu.AMST_MiddleName.ToUpper().Contains(adm.EnteredData)
                            //                  || adm_stu.AMST_LastName.ToUpper().Contains(adm.EnteredData)
                            //                  || (adm_stu.AMST_FirstName.ToUpper().Trim() + ' ' + adm_stu.AMST_LastName.ToUpper().Trim()).Contains(adm.EnteredData)
                            //                  || (adm_stu.AMST_FirstName.ToUpper().Trim() + ' '
                            //                  + adm_stu.AMST_MiddleName.ToUpper().Trim() + ' ' + adm_stu.AMST_LastName.ToUpper().Trim()).
                            //                  Contains(adm.EnteredData)))

                            //                  group new { adm_stu, cls }
                            //                   by new { adm_stu.AMST_Id } into g

                            //                  select new Adm_M_StudentDTO
                            //                  {
                            //                      AMST_Date = g.FirstOrDefault().adm_stu.AMST_Date,
                            //                      AMST_Sex = g.FirstOrDefault().adm_stu.AMST_Sex,
                            //                      AMST_RegistrationNo = g.FirstOrDefault().adm_stu.AMST_RegistrationNo,
                            //                      AMST_AdmNo = g.FirstOrDefault().adm_stu.AMST_AdmNo,
                            //                      AMST_emailId = g.FirstOrDefault().adm_stu.AMST_emailId,
                            //                      stdmobilenumber = Convert.ToString(g.FirstOrDefault().adm_stu.AMST_MobileNo),
                            //                      AMST_Id = g.FirstOrDefault().adm_stu.AMST_Id,
                            //                      Class = g.FirstOrDefault().cls.ASMCL_ClassName,
                            //                      AMST_SOL = g.FirstOrDefault().adm_stu.AMST_SOL,
                            //                      AMST_Photoname = g.FirstOrDefault().adm_stu.AMST_Photoname,

                            //                      studentname = ((g.FirstOrDefault().adm_stu.AMST_FirstName == null || g.FirstOrDefault().adm_stu.AMST_FirstName == "" ?
                            //                      "" : g.FirstOrDefault().adm_stu.AMST_FirstName) + (g.FirstOrDefault().adm_stu.AMST_MiddleName == null ||
                            //                      g.FirstOrDefault().adm_stu.AMST_MiddleName == "" ? "" : " " + g.FirstOrDefault().adm_stu.AMST_MiddleName) +
                            //                      (g.FirstOrDefault().adm_stu.AMST_LastName == null || g.FirstOrDefault().adm_stu.AMST_LastName == "" ? "" : " " +
                            //                      g.FirstOrDefault().adm_stu.AMST_LastName)).Trim()
                            //                  }).ToList();


                            //multiple mobile number feteching   start
                            //var query = (from a in _AdmissionFormContext.Adm_M_Student
                            //             from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                            //             where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id &&
                            //             (a.AMST_FirstName.ToUpper().Contains(adm.EnteredData)
                            //                  || a.AMST_MiddleName.ToUpper().Contains(adm.EnteredData)
                            //                  || a.AMST_LastName.ToUpper().Contains(adm.EnteredData)
                            //                  || (a.AMST_FirstName.ToUpper().Trim() + ' ' + a.AMST_LastName.ToUpper().Trim()).Contains(adm.EnteredData)
                            //                  || (a.AMST_FirstName.ToUpper().Trim() + ' '
                            //                  + a.AMST_MiddleName.ToUpper().Trim() + ' ' + a.AMST_LastName.ToUpper().Trim()).
                            //                  Contains(adm.EnteredData)))
                            //             select new Adm_M_Student_TempMobileNo
                            //             {
                            //                 UserName = a.AMST_Id,
                            //                 Role = b.AMSTSMS_MobileNo
                            //             }).ToList();


                            using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "StudentName_Search";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                                {
                                    Value = adm.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                              SqlDbType.VarChar)
                                {
                                    Value = adm.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@EnteredData",
                              SqlDbType.VarChar)
                                {
                                    Value = adm.EnteredData
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

                                            adm_m_student1.Add(new Adm_M_StudentDTO
                                            {
                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                AMST_Date = Convert.ToDateTime(dataReader["AMST_Date"]),
                                                AMST_Sex = Convert.ToString(dataReader["AMST_Sex"]),
                                                AMST_RegistrationNo = Convert.ToString(dataReader["AMST_RegistrationNo"]),
                                                AMST_AdmNo = Convert.ToString(dataReader["AMST_AdmNo"]),
                                                AMST_emailId = Convert.ToString(dataReader["AMST_emailId"]),
                                                stdmobilenumber = Convert.ToString(dataReader["AMST_MobileNo"]),
                                                Class = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                                AMST_SOL = Convert.ToString(dataReader["AMST_SOL"]),
                                                studentname = Convert.ToString(dataReader["studentname"]),

                                            });
                                        }
                                    }
                                    //   adm.studentlist123 = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "StudentMobileNo_Search";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                                {
                                    Value = adm.MI_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@EnteredData",
                              SqlDbType.VarChar)
                                {
                                    Value = adm.EnteredData
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

                                            query.Add(new Adm_M_Student_TempMobileNo
                                            {
                                                UserName = Convert.ToInt64(dataReader["AMST_Id"]),
                                                Role = Convert.ToString(dataReader["AMSTSMS_MobileNo"])

                                            });
                                        }
                                    }
                                    //   adm.studentlist123 = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            int count = query.Count() + 1;
                            Adm_M_Student_TempMobileNo[] temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            string value = null;
                            Dictionary<long, string> tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            List<Adm_M_Student_TempMobileNo> list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            var query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                          from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMST_Id,
                                              Roleemail = b.AMSTE_EmailId
                                          }).ToList();


                            int count1 = query1.Count() + 1;
                            Adm_M_Student_TempEmailId[] temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            string value1 = null;
                            Dictionary<long, string> tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            List<Adm_M_Student_TempEmailId> list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }



                            break;
                        case "1":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_FirstName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_FirstName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_FirstName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "2":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_MiddleName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_MiddleName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_MiddleName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "3":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_LastName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_LastName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_LastName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "4":
                            try
                            {
                                DateTime date = DateTime.ParseExact(adm.EnteredData, "dd/MM/yyyy",
                                   CultureInfo.InvariantCulture);

                                adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                                  from cls in _AdmissionFormContext.School_M_Class
                                                  where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                                  //&& adm_stu.AMST_ActiveFlag == 1

                                                  select new Adm_M_StudentDTO
                                                  {
                                                      //AMST_FirstName = adm_stu.AMST_FirstName,
                                                      //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                      //AMST_LastName = adm_stu.AMST_LastName,
                                                      AMST_Date = adm_stu.AMST_Date,
                                                      AMST_Sex = adm_stu.AMST_Sex,
                                                      AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                      AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                      AMST_emailId = adm_stu.AMST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                      AMST_Id = adm_stu.AMST_Id,
                                                      Class = cls.ASMCL_ClassName,
                                                      AMST_SOL = adm_stu.AMST_SOL,
                                                      AMST_Photoname = adm_stu.AMST_Photoname,
                                                      studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMST_Id).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _AdmissionFormContext.Adm_M_Student
                                         from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMST_Id,
                                             Role = b.AMSTSMS_MobileNo
                                         }).OrderByDescending(d => d.UserName).ToList();

                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {
                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }
                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                //end

                                //multiple email ids start 
                                query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                          from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd")))
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMST_Id,
                                              Roleemail = b.AMSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }

                                //end
                                adm.adm_m_student = adm_m_student1.ToArray();

                                if (adm.adm_m_student.Length > 0)
                                {
                                    adm.count = adm.adm_m_student.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                adm.message = "Please Enter date in dd/MM/yyyy format";
                                adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                                  from cls in _AdmissionFormContext.School_M_Class
                                                  where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del")
                                                  //&& adm_stu.AMST_ActiveFlag == 1
                                                  select new Adm_M_StudentDTO
                                                  {
                                                      //AMST_FirstName = adm_stu.AMST_FirstName,
                                                      //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                      //AMST_LastName = adm_stu.AMST_LastName,
                                                      AMST_Date = adm_stu.AMST_Date,
                                                      AMST_Sex = adm_stu.AMST_Sex,
                                                      AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                      AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                      AMST_emailId = adm_stu.AMST_emailId,
                                                      stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                      AMST_Id = adm_stu.AMST_Id,
                                                      Class = cls.ASMCL_ClassName,
                                                      AMST_SOL = adm_stu.AMST_SOL,
                                                      AMST_Photoname = adm_stu.AMST_Photoname,
                                                      studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                                  }).OrderByDescending(d => d.AMST_Id).Take(10).ToList();

                                //multiple mobile number feteching   start
                                query = (from a in _AdmissionFormContext.Adm_M_Student
                                         from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                         select new Adm_M_Student_TempMobileNo
                                         {
                                             UserName = a.AMST_Id,
                                             Role = b.AMSTSMS_MobileNo
                                         }).OrderByDescending(d => d.UserName).Take(10).ToList();
                                //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                                //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                                count = query.Count() + 1;
                                temp = new Adm_M_Student_TempMobileNo[count];

                                query.CopyTo(temp);

                                value = null;
                                tempDictionary = new Dictionary<long, string>();
                                for (int i = 0; i < query.Count(); i++)
                                {
                                    if (query[i].UserName == temp[i].UserName)
                                    {
                                        if (!tempDictionary.ContainsKey(query[i].UserName))
                                        {
                                            tempDictionary.Add(query[i].UserName, query[i].Role);
                                        }
                                        else
                                        {

                                            tempDictionary.TryGetValue(query[i].UserName, out value);
                                            value = value + ", " + query[i].Role;
                                            tempDictionary[query[i].UserName] = value;
                                        }
                                    }
                                }

                                list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                                adm.stdmobile = list.ToArray();
                                //end here 

                                //assigning the mobile number to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list.Count())
                                    {
                                        if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].stdmobilenumber = list[k].Role;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }

                                //end

                                //multiple email ids start 
                                query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                          from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                          select new Adm_M_Student_TempEmailId
                                          {
                                              UserNameemail = a.AMST_Id,
                                              Roleemail = b.AMSTE_EmailId
                                          }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                                count1 = query1.Count() + 1;
                                temp1 = new Adm_M_Student_TempEmailId[count1];

                                query1.CopyTo(temp1);

                                value1 = null;
                                tempDictionary1 = new Dictionary<long, string>();
                                for (int i = 0; i < query1.Count(); i++)
                                {
                                    if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                    {
                                        if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                        {
                                            tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                        }
                                        else
                                        {
                                            tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                            value1 = value1 + ", " + query1[i].Roleemail;
                                            tempDictionary1[query1[i].UserNameemail] = value1;
                                        }
                                    }
                                }
                                list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                                adm.stdemail = list1.ToArray();
                                //end

                                //assigning the email ids to main list start
                                for (int k = 0; k < adm_m_student1.Count(); k++)
                                {
                                    if (k < list1.Count())
                                    {
                                        if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                        {
                                            adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                        }
                                        else
                                        {
                                            adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                        }
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }

                                adm.adm_m_student = adm_m_student1.ToArray();

                                if (adm.adm_m_student.Length > 0)
                                {
                                    adm.count = adm.adm_m_student.Length;
                                }
                                else
                                {
                                    adm.count = 0;
                                }
                            }

                            break;
                        case "5":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_Sex.ToUpper().Equals(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()

                                              }).OrderByDescending(d => d.AMST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Sex.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_Sex.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "6":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_RegistrationNo.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()

                                              }).OrderByDescending(d => d.AMST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_RegistrationNo.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_RegistrationNo.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "7":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_SOL != "Del" && adm_stu.AMST_AdmNo.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_AdmNo.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.AMST_AdmNo.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "8":

                            List<Adm_M_StudentDTO> GrpId = new List<Adm_M_StudentDTO>();

                            // List<long> list11 = new List<long>();

                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.ASMAY_Id == adm.ASMAY_Id && b.AMSTE_EmailId.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            for (int k = 0; k < list1.Count; k++)
                            {
                                list11.Add(list1[k].UserNameemail);
                            }



                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && list11.Contains(adm_stu.AMST_Id))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && list11.Contains(a.AMST_Id))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }
                            //end                       

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "9":
                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && a.ASMAY_Id == adm.ASMAY_Id && b.AMSTSMS_MobileNo.ToString().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();

                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {
                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }
                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();

                            for (int k = 0; k < list.Count; k++)
                            {
                                list11.Add(list[k].UserName);
                            }

                            //end here 
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && list11.Contains(adm_stu.AMST_Id))
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_ActiveFlag == 0 && a.AMST_SOL != "Del" && list11.Contains(a.AMST_Id))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }
                            //end

                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "10":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == adm.ASMAY_Id && cls.ASMCL_ClassName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     from c in _AdmissionFormContext.School_M_Class
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMCL_Id == c.ASMCL_Id && c.ASMCL_ClassName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      from c in _AdmissionFormContext.School_M_Class
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMCL_Id == c.ASMCL_Id && c.ASMCL_ClassName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "11":
                            if (adm.EnteredData.Equals("active", StringComparison.OrdinalIgnoreCase))

                            {
                                adm.EnteredData = "S";
                            }
                            else if (adm.EnteredData.Equals("deactive", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "D";
                            }
                            else if (adm.EnteredData.Equals("left", StringComparison.OrdinalIgnoreCase))
                            {
                                adm.EnteredData = "L";
                            }
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_SOL.ToUpper().Equals(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_SOL.ToUpper().Equals(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_SOL.ToUpper().Equals(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();



                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "12":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_FatherName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()

                                              }).OrderByDescending(d => d.AMST_Id).ToList();


                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_FatherName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_FatherName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "13":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_MotherName.ToUpper().Contains(adm.EnteredData))
                                              //&& adm_stu.AMST_ActiveFlag == 1

                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_MotherName.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_MotherName.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();



                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;


                        case "14":
                            var academicYearId = _db.AcademicYear.Where(d => d.ASMAY_Year.Equals(adm.EnteredData) && d.MI_Id == adm.MI_Id).ToList();
                            if (academicYearId.Count > 0)
                            {
                                asmayId = academicYearId.FirstOrDefault().ASMAY_Id;
                            }
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del" && adm_stu.ASMAY_Id == asmayId)
                                              //&& adm_stu.AMST_ActiveFlag == 1
                                              select new Adm_M_StudentDTO
                                              {
                                                  //AMST_FirstName = adm_stu.AMST_FirstName,
                                                  //AMST_MiddleName = adm_stu.AMST_MiddleName,
                                                  //AMST_LastName = adm_stu.AMST_LastName,
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();
                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.ASMAY_Id.Equals(asmayId))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();


                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;
                        case "15":
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del"
                                              && adm_stu.ASMAY_Id == adm.ASMAY_Id && adm_stu.AMST_BPLCardNo.ToUpper().Contains(adm.EnteredData))
                                              select new Adm_M_StudentDTO
                                              {
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).ToList();



                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_BPLCardNo.ToUpper().Contains(adm.EnteredData))
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).ToList();
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id && a.AMST_BPLCardNo.ToUpper().Contains(adm.EnteredData))
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();

                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                        default:
                            adm_m_student1 = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              from cls in _AdmissionFormContext.School_M_Class
                                              where (adm_stu.ASMCL_Id == cls.ASMCL_Id && adm_stu.MI_Id == adm.MI_Id && adm_stu.AMST_SOL != "Del")
                                              select new Adm_M_StudentDTO
                                              {
                                                  AMST_Date = adm_stu.AMST_Date,
                                                  AMST_Sex = adm_stu.AMST_Sex,
                                                  AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                                  AMST_AdmNo = adm_stu.AMST_AdmNo,
                                                  AMST_emailId = adm_stu.AMST_emailId,
                                                  stdmobilenumber = Convert.ToString(adm_stu.AMST_MobileNo),
                                                  AMST_Id = adm_stu.AMST_Id,
                                                  Class = cls.ASMCL_ClassName,
                                                  AMST_SOL = adm_stu.AMST_SOL,
                                                  AMST_Photoname = adm_stu.AMST_Photoname,
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName) +
                                                  (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName) +
                                                  (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim()
                                              }).OrderByDescending(d => d.AMST_Id).Take(10).ToList();

                            //multiple mobile number feteching   start
                            query = (from a in _AdmissionFormContext.Adm_M_Student
                                     from b in _AdmissionFormContext.Adm_M_Student_MobileNo
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                     select new Adm_M_Student_TempMobileNo
                                     {
                                         UserName = a.AMST_Id,
                                         Role = b.AMSTSMS_MobileNo
                                     }).OrderByDescending(d => d.UserName).Take(10).ToList();
                            //Adm_M_StudentDTO.stdmobile = query.GroupBy(cc => cc.UserName).Select(dd => new { UserName = dd.Key, Role = string.Join(",", dd.Select(ee => ee.Role).ToList()) });

                            //  string.Join(",", query.Where(o => o.UserName == 00).Select(o => o.Role));.
                            count = query.Count() + 1;
                            temp = new Adm_M_Student_TempMobileNo[count];

                            query.CopyTo(temp);

                            value = null;
                            tempDictionary = new Dictionary<long, string>();
                            for (int i = 0; i < query.Count(); i++)
                            {
                                if (query[i].UserName == temp[i].UserName)
                                {
                                    if (!tempDictionary.ContainsKey(query[i].UserName))
                                    {
                                        tempDictionary.Add(query[i].UserName, query[i].Role);
                                    }
                                    else
                                    {

                                        tempDictionary.TryGetValue(query[i].UserName, out value);
                                        value = value + ", " + query[i].Role;
                                        tempDictionary[query[i].UserName] = value;
                                    }
                                }
                            }

                            list = tempDictionary.Select(p => new Adm_M_Student_TempMobileNo { UserName = p.Key, Role = p.Value }).ToList();
                            adm.stdmobile = list.ToArray();
                            //end here 

                            //assigning the mobile number to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list.Count())
                                {
                                    if (list[k].UserName == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].stdmobilenumber = list[k].Role;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].stdmobilenumber = adm_m_student1[k].stdmobilenumber;
                                }
                            }

                            //end

                            //multiple email ids start 
                            query1 = (from a in _AdmissionFormContext.Adm_M_Student
                                      from b in _AdmissionFormContext.Adm_M_Student_Email_Id
                                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == adm.MI_Id)
                                      select new Adm_M_Student_TempEmailId
                                      {
                                          UserNameemail = a.AMST_Id,
                                          Roleemail = b.AMSTE_EmailId
                                      }).OrderByDescending(d => d.UserNameemail).Take(10).ToList();


                            count1 = query1.Count() + 1;
                            temp1 = new Adm_M_Student_TempEmailId[count1];

                            query1.CopyTo(temp1);

                            value1 = null;
                            tempDictionary1 = new Dictionary<long, string>();
                            for (int i = 0; i < query1.Count(); i++)
                            {
                                if (query1[i].UserNameemail == temp1[i].UserNameemail)
                                {
                                    if (!tempDictionary1.ContainsKey(query1[i].UserNameemail))
                                    {
                                        tempDictionary1.Add(query1[i].UserNameemail, query1[i].Roleemail);
                                    }
                                    else
                                    {
                                        tempDictionary1.TryGetValue(query1[i].UserNameemail, out value1);
                                        value1 = value1 + ", " + query1[i].Roleemail;
                                        tempDictionary1[query1[i].UserNameemail] = value1;
                                    }
                                }
                            }
                            list1 = tempDictionary1.Select(p => new Adm_M_Student_TempEmailId { UserNameemail = p.Key, Roleemail = p.Value }).ToList();
                            adm.stdemail = list1.ToArray();
                            //end

                            //assigning the email ids to main list start
                            for (int k = 0; k < adm_m_student1.Count(); k++)
                            {
                                if (k < list1.Count())
                                {
                                    if (list1[k].UserNameemail == adm_m_student1[k].AMST_Id)
                                    {
                                        adm_m_student1[k].AMST_emailId = list1[k].Roleemail;
                                    }
                                    else
                                    {
                                        adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                    }
                                }
                                else
                                {
                                    adm_m_student1[k].AMST_emailId = adm_m_student1[k].AMST_emailId;
                                }
                            }

                            //end
                            adm.adm_m_student = adm_m_student1.ToArray();
                            if (adm.adm_m_student.Length > 0)
                            {
                                adm.count = adm.adm_m_student.Length;
                            }
                            else
                            {
                                adm.count = 0;
                            }
                            break;

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return adm;
        }
        public Adm_M_StudentDTO StateByCountryName(Adm_M_StudentDTO ct)
        {
            try
            {
                var statename = _AdmissionFormContext.Country.Where(d => d.IVRMMC_CountryName.Equals(ct.countryName)).ToList();
                if (statename.Count > 0)
                {
                    ct.prevStateList = _AdmissionFormContext.State.Where(d => d.IVRMMC_Id == statename.FirstOrDefault().IVRMMC_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ct;
        }
        public Adm_M_StudentDTO getmaxminage(Adm_M_StudentDTO stu)
        {
            try
            {
                string date = Convert.ToString(stu.AMST_Date);

                stu.message = "";
                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_Id == stu.ASMCL_Id).ToList();
                stu.fillclass = allclass.ToArray();

                stu.studentCategory = (from m in _db.mastercategory
                                       from n in _db.Masterclasscategory
                                       where m.AMC_Id == n.AMC_Id && n.MI_Id == stu.MI_Id && n.ASMCL_Id == stu.ASMCL_Id
                                       && n.ASMAY_Id == stu.ASMAY_Id && n.Is_Active == true
                                       select new MasterClassCategoryDTO
                                       {
                                           ASMCC_Id = n.ASMCC_Id,
                                           className = m.AMC_Name
                                       }
               ).ToArray();

                if (stu.admflag.Equals("adm"))
                {
                    var classcapacity = _db.Adm_M_Student.Where(d => d.AMST_SOL != "D" && d.ASMCL_Id == stu.ASMCL_Id && d.MI_Id == stu.MI_Id && d.ASMAY_Id == stu.ASMAY_Id).Count();

                    if (classcapacity >= allclass[0].ASMCL_MaxCapacity)
                    {
                        stu.admclassCapacity = "MaxCapacity";
                    }
                }
                else
                {
                    var AdmissionCount = _AdmissionFormContext.Enq.Where(d => d.PASR_Adm_Confirm_Flag == true && d.ASMCL_Id == stu.ASMCL_Id && d.MI_Id == stu.MI_Id).Count();

                    if (AdmissionCount >= allclass[0].ASMCL_MaxCapacity)
                    {
                        stu.message = "MaxCapacity";
                    }
                }


                using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Electives_groups";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = stu.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = stu.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                    {
                        Value = stu.ASMCL_Id
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
                        stu.electivelist_Groups = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Electives";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = stu.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = stu.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                    {
                        Value = stu.ASMCL_Id
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
                        stu.electivelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return stu;
        }
        public async Task<Adm_M_StudentDTO> savefirsttab(Adm_M_StudentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                string regno = "";
                string admno = "";
                string tpinno = "";

                //student mobile saving
                string stdmobileno = "";
                string stdmobilenosms = "";
                string stdemailsms = "";
                string stdemailids = "";
                var count = data.Adm_M_Student_MobileNoDTO.Length;

                if (data.Adm_M_Student_MobileNoDTO != null && data.Adm_M_Student_MobileNoDTO.Length > 0)
                {
                    stdmobilenosms = data.Adm_M_Student_MobileNoDTO[0].AMST_MobileNo;
                    stdmobileno = data.Adm_M_Student_MobileNoDTO[0].AMST_MobileNo;
                }

                if (data.Adm_M_Student_EmailIdDTO != null && data.Adm_M_Student_EmailIdDTO.Length > 0)
                {
                    stdemailsms = data.Adm_M_Student_EmailIdDTO[0].AMST_emailId;
                    stdemailids = data.Adm_M_Student_EmailIdDTO[0].AMST_emailId;
                }

                if (data.AMST_Id > 0)
                {
                    data.message = "Update";
                    data.returnval = false;
                    var result = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id);
                    result.MI_Id = data.MI_Id;
                    result.AMST_Date = Convert.ToDateTime(data.AMST_Date);
                    result.AMST_DOB = Convert.ToDateTime(data.AMST_DOB);
                    result.IMCC_Id = data.IMCC_Id;
                    result.IVRMMR_Id = data.IVRMMR_Id;
                    result.IC_Id = data.IMC_Id;
                    result.AMST_AadharNo = data.AMST_AadharNo;
                    result.AMST_AdmNo = data.AMST_AdmNo;
                    result.AMST_DOB_Words = data.AMST_DOB_Words;
                    result.AMST_LastName = data.AMST_LastName;
                    result.AMST_MiddleName = data.AMST_MiddleName;
                    result.AMST_FirstName = data.AMST_FirstName;
                    result.AMST_StuBankAccNo = data.AMST_StuBankAccNo;
                    result.AMST_BankName = data.AMST_BankName;
                    result.AMST_BranchName = data.AMST_BranchName;
                    result.AMST_State = data.AMST_State;
                    result.AMST_StuCasteCertiNo = data.AMST_StuCasteCertiNo;
                    result.AMST_StuBankIFSC_Code = data.AMST_StuBankIFSC_Code;
                    result.AMST_SubCasteIMC_Id = data.AMST_SubCasteIMC_Id;
                    result.AMST_Tribe = data.AMST_Tribe;
                    result.AMST_Sex = data.AMST_Sex;
                    result.AMST_ECSFlag = data.AMST_ECSFlag;
                    result.AMST_RegistrationNo = data.AMST_RegistrationNo;
                    result.AMST_BirthCertNO = data.AMST_BirthCertNO;
                    result.AMST_BirthPlace = data.AMST_BirthPlace;
                    result.AMST_BPLCardNo = data.AMST_BPLCardNo;
                    result.AMST_BPLCardFlag = data.AMST_BPLCardFlag;
                    result.AMST_Nationality = data.AMST_Nationality;
                    result.AMC_Id = data.AMC_Id;
                    result.ASMCL_Id = data.ASMCL_Id;
                    result.ASMAY_Id = data.ASMAY_Id;
                    result.IVRMMR_Id = data.IVRMMR_Id;
                    result.PASR_Age = data.PASR_Age;
                    result.AMST_MotherTongue = data.AMST_MotherTongue;
                    result.AMST_BloodGroup = data.AMST_BloodGroup;
                    result.AMST_Concession_Type = data.AMST_Concession_Type;
                    result.AMST_GovtAdmno = data.AMST_GovtAdmno;
                    result.AMST_LanguageSpoken = data.AMST_LanguageSpoken;
                    result.AMST_StudentPANNo = data.AMST_StudentPANNo;
                    result.AMST_GPSTrackingId = data.AMST_GPSTrackingId;
                    result.AMST_BiometricId = data.AMST_BiometricId;
                    result.AMST_RFCardNo = data.AMST_RFCardNo;
                    result.AMST_MOInstruction = data.AMST_MOInstruction;
                    result.ASMST_Id = data.ASMST_Id;
                    result.AMST_MobileNo = Convert.ToInt64(stdmobilenosms);
                    result.AMST_emailId = stdemailids;
                    result.AMST_SOL = result.AMST_SOL;
                    result.AMST_ActiveFlag = result.AMST_ActiveFlag;
                    result.AMST_Photoname = data.AMST_Photoname;
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = data.userid;
                    //This is used for Pen no..because already existing DTO taken...so name is differ
                    //result.AMST_PENNo = data.AMST_AppDownloadedDeviceId;
                    result.AMST_CoutryCode = data.Adm_M_Student_MobileNoDTO[0].AMSTSMS_CountryCode;
                    _AdmissionFormContext.Update(result);

                    student_mobile_no(data);
                    student_email_id(data);
                    Studentexam(data);

                    if (data.AMST_ECSFlag == 1)
                    {
                        saveupdate_ecsdetails(data);
                    }
                    else if (data.AMST_ECSFlag == 0)
                    {
                        var resecs = _AdmissionFormContext.Adm_Student_EcsDetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToList();
                        if (resecs.Count() > 0)
                        {
                            _AdmissionFormContext.Remove(resecs[0]);
                        }
                    }
                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        try
                        {
                            var classid = _db.School_Adm_Y_StudentDMO.Where(s => s.ASMAY_Id == data.ASMAY_Id && s.AMST_Id == data.AMST_Id && s.ASMCL_Id == data.ASMCL_Id).Count();
                            if (classid > 1)
                            {

                            }
                            else
                            {
                                var resulty = _db.School_Adm_Y_StudentDMO.Single(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id);
                                resulty.ASMCL_Id = Convert.ToInt64(data.ASMCL_Id);
                                resulty.ASMS_Id = Convert.ToInt64(data.asms_id);
                                resulty.ASYST_UpdatedBy = data.userid;
                                resulty.UpdatedDate = indiantime0;

                                _db.Update(resulty);
                                _db.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        data.message = "Update";
                        data.returnval = true;

                        try
                        {
                            var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == data.AMST_Id).Select(a => a.STD_APP_ID).ToList();
                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByIdAsync(getstdappid[0].ToString());
                            user.PhoneNumber = stdmobileno;
                            user.UserImagePath = data.AMST_Photoname;
                            user.Email = stdemailids;
                            user.NormalizedEmail = stdemailids;
                            var i = await _UserManager.UpdateAsync(user);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    {
                        regno = data.AMST_RegistrationNo;
                    }
                    else
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        data.transnumconfigsettings.MI_Id = data.MI_Id;
                        data.transnumconfigsettings.ASMAY_Id = data.ASMAY_Id;
                        regno = a.GenerateNumber(data.transnumconfigsettings);
                    }

                    if (data.admissionNumbering.IMN_AutoManualFlag == "Manual")
                    {
                        admno = data.AMST_AdmNo;
                    }
                    else
                    {
                        GenerateTransactionNumbering b = new GenerateTransactionNumbering(_db);
                        data.admissionNumbering.MI_Id = data.MI_Id;
                        data.admissionNumbering.ASMAY_Id = data.ASMAY_Id;
                        admno = b.GenerateNumber(data.admissionNumbering);
                    }

                    string dateday = data.AMST_DOB.ToString("dd");
                    string datemonth = data.AMST_DOB.ToString("MM");
                    string dateyear = data.AMST_DOB.ToString("yyyy");
                    dateyear = dateyear.Substring(2, 2);
                    string date = "";
                    date = dateyear + datemonth + dateyear + "001";
                    long tpin = 0;
                    int i1 = 0;
                    var checktpinexists = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Tpin.Equals(date)).ToList();
                    while (checktpinexists.Count > 0)
                    {
                        if (i1 == 0)
                        {
                            tpin = Convert.ToInt64(date) + 1;
                            i1 = i1 + 1;
                        }
                        else
                        {
                            tpin = Convert.ToInt64(tpin) + 1;
                        }

                        string date1 = Convert.ToString(tpin);
                        checktpinexists = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Tpin.Equals(date1.ToString())).ToList();
                    }
                    if (tpin == 0)
                    {
                        tpinno = Convert.ToString(date);
                    }
                    else
                    {
                        tpinno = Convert.ToString(tpin);
                    }

                    data.message = "Add";
                    data.returnval = false;

                    Adm_M_Student result = new Adm_M_Student
                    {
                        MI_Id = data.MI_Id,
                        AMST_RegistrationNo = regno,
                        AMST_AdmNo = admno,
                        AMST_Date = Convert.ToDateTime(data.AMST_Date),
                        AMST_DOB = Convert.ToDateTime(data.AMST_DOB),
                        IMCC_Id = data.IMCC_Id,
                        IVRMMR_Id = data.IVRMMR_Id,
                        IC_Id = data.IMC_Id,
                        AMST_AadharNo = data.AMST_AadharNo,
                        AMST_State = data.AMST_State,
                        AMST_DOB_Words = data.AMST_DOB_Words,
                        AMST_LastName = data.AMST_LastName,
                        AMST_MiddleName = data.AMST_MiddleName,
                        AMST_FirstName = data.AMST_FirstName,
                        AMST_StuBankAccNo = data.AMST_StuBankAccNo,
                        AMST_StuCasteCertiNo = data.AMST_StuCasteCertiNo,
                        AMST_StuBankIFSC_Code = data.AMST_StuBankIFSC_Code,
                        AMST_SubCasteIMC_Id = data.AMST_SubCasteIMC_Id,
                        AMST_Sex = data.AMST_Sex,
                        AMST_ECSFlag = data.AMST_ECSFlag,
                        AMST_BirthCertNO = data.AMST_BirthCertNO,
                        AMST_BirthPlace = data.AMST_BirthPlace,
                        AMST_BPLCardNo = data.AMST_BPLCardNo,
                        AMST_BPLCardFlag = data.AMST_BPLCardFlag,
                        AMST_Nationality = data.AMST_Nationality,
                        AMC_Id = data.AMC_Id,
                        ASMCL_Id = data.ASMCL_Id,
                        ASMAY_Id = data.ASMAY_Id,
                        PASR_Age = data.PASR_Age,
                        AMST_BankName = data.AMST_BankName,
                        AMST_BranchName = data.AMST_BranchName,
                        AMST_MotherTongue = data.AMST_MotherTongue,
                        AMST_BloodGroup = data.AMST_BloodGroup,
                        AMST_Concession_Type = data.AMST_Concession_Type,
                        AMST_GovtAdmno = data.AMST_GovtAdmno,
                        AMST_LanguageSpoken = data.AMST_LanguageSpoken,
                        AMST_StudentPANNo = data.AMST_StudentPANNo,
                        AMST_GPSTrackingId = data.AMST_GPSTrackingId,
                        ASMST_Id = data.ASMST_Id,
                        AMST_BiometricId = data.AMST_BiometricId,
                        AMST_RFCardNo = data.AMST_RFCardNo,
                        AMST_MOInstruction = data.AMST_MOInstruction,
                        AMST_MobileNo = Convert.ToInt64(stdmobilenosms),
                        AMST_emailId = stdemailids,
                        AMST_SOL = "S",
                        AMST_Tpin = tpinno,
                        AMST_ActiveFlag = 1,
                        AMST_Photoname = data.AMST_Photoname,
                        CreatedDate = indiantime0,
                        UpdatedDate = indiantime0,
                        AMST_UpdatedBy = data.userid,
                        AMST_CreatedBy = data.userid,
                        //This is used for Pen no..because already existing DTO taken...so name is differ

                        //AMST_PENNo = data.AMST_AppDownloadedDeviceId,
                        AMST_CoutryCode = data.Adm_M_Student_MobileNoDTO[0].AMSTSMS_CountryCode,
                    };
                    _AdmissionFormContext.Add(result);

                    data.AMST_Id = result.AMST_Id;
                    student_mobile_no(data);
                    student_email_id(data);
                    Studentexam(data);

                    if (data.AMST_ECSFlag == 1)
                    {
                        saveupdate_ecsdetails(data);
                    }

                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        data.AMST_Id = result.AMST_Id;
                        data.message = "Add";
                        data.returnval = true;

                        if (data.asms_id > 0)
                        {
                            int MaxCapacity = _AdmissionFormContext.AdmSection.SingleOrDefault(s => s.ASMS_Id == data.asms_id).ASMC_MaxCapacity;

                            if (MaxCapacity == 0)
                            {
                                data.Messagesection = "Zero Capacity";

                            }
                            var countff = (from m in _AdmissionFormContext.Adm_M_Student
                                           from nd in _AdmissionFormContext.SchoolYearWiseStudent
                                           where (m.AMST_Id == nd.AMST_Id && m.MI_Id == data.MI_Id && nd.AMST_Id== data.AMST_Id)
                                           select new Adm_M_StudentDTO
                                           {
                                               AMST_Id = nd.AMST_Id
                                           }).ToList();
                            //add Student Section details
                            try
                            {
                                var createdcount = _AdmissionFormContext.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag.Equals(true) && t.ASMCL_Id == data.ASMCL_Id
                                && t.ASMS_Id == data.asms_id && t.ASMAY_Id == data.ASMAY_Id).ToList();

                                if (createdcount.Count < MaxCapacity)
                                {

                                    var rollno = _AdmissionFormContext.Master_Numbering.Where(s => s.IMN_Flag == "RollNumber" && s.MI_Id == data.MI_Id).ToList();

                                    if (countff.Count == 1)
                                    {
                                        if (rollno.FirstOrDefault().IMN_AutoManualFlag == "Auto")
                                        {
                                            // Generate_RollNumber

                                            var confirmstatusadmission = _db.Database.ExecuteSqlCommand("Generate_RollNumber @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.asms_id);
                                            if (confirmstatusadmission > 0)
                                            {
                                            }
                                            //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                            //{
                                            //    cmd.CommandText = "Generate_RollNumber";
                                            //    cmd.CommandType = CommandType.StoredProcedure;
                                            //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                            //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                            //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                                            //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.asms_id });

                                            //    if (cmd.Connection.State != ConnectionState.Open)
                                            //        cmd.Connection.Open();

                                            //    var retObject = new List<dynamic>();

                                            //    try
                                            //    {
                                            //        using (var dataReader = cmd.ExecuteReader())
                                            //        {
                                            //            while (dataReader.Read())
                                            //            {
                                            //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            //                {
                                            //                    dataRow.Add(
                                            //                        dataReader.GetName(iFiled),
                                            //                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                            //                    );
                                            //                }
                                            //                retObject.Add((ExpandoObject)dataRow);
                                            //            }
                                            //        }
                                            //        data.viewstudentaddressdetails = retObject.ToArray();
                                            //    }
                                            //    catch (Exception ex)
                                            //    {
                                            //        Console.WriteLine(ex.Message);
                                            //    }
                                            //}
                                        }


                                    }
                                    else if (countff.Count == 0)
                                    {
                                        if (countff.Count > 0)
                                        {
                                            var resultyearly = (from m in _AdmissionFormContext.Adm_M_Student
                                                                from ndd in _AdmissionFormContext.SchoolYearWiseStudent
                                                                where (m.AMST_Id == ndd.AMST_Id && m.MI_Id == data.MI_Id && ndd.ASMAY_Id == data.ASMAY_Id
                                                                && ndd.ASMCL_Id == data.ASMCL_Id && ndd.ASMS_Id == data.asms_id)
                                                                select new Adm_M_StudentDTO
                                                                {
                                                                    AMST_Id = ndd.AMST_Id,
                                                                    AMAY_RollNo = ndd.AMAY_RollNo
                                                                }).ToList();
                                            if (resultyearly.Count > 0)
                                            {
                                                var rollNo = resultyearly.OrderByDescending(e => e.AMAY_RollNo).First().AMAY_RollNo;
                                                data.AMAY_RollNo = rollNo + 1;
                                            }
                                            else
                                            {
                                                data.AMAY_RollNo = 1;
                                            }
                                        }
                                        else
                                        {
                                            data.AMAY_RollNo = 1;
                                        }
                                        School_Adm_Y_StudentDMO sct = new School_Adm_Y_StudentDMO
                                        {
                                            ASMAY_Id = data.ASMAY_Id,
                                            ASMCL_Id = Convert.ToInt64(data.ASMCL_Id),
                                            ASMS_Id = Convert.ToInt64(data.asms_id),
                                            AMST_Id = data.AMST_Id,
                                            AMAY_RollNo = data.AMAY_RollNo,
                                            AMAY_ActiveFlag = 1,
                                            LoginId = Convert.ToInt64(data.userid),
                                            AMAY_DateTime = indiantime0,
                                            ASYST_CreatedBy = data.userid,
                                            ASYST_UpdatedBy = data.userid,
                                            CreatedDate = indiantime0,
                                            UpdatedDate = indiantime0
                                        };
                                        _AdmissionFormContext.Add(sct);

                                        var ii = _AdmissionFormContext.SaveChanges();
                                    }


                                }
                                else
                                {
                                    data.Messagesection = "Maximum limit for this section is exceeded.";
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                data.Messagesection = "Failed";
                            }
                        }

                        var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == data.MI_Id);
                        var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).ToList();

                        string emailids = "";
                        long mobilenos = 0;

                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                        {
                            emailids = studDet.FirstOrDefault().AMST_MotherEmailId;
                            mobilenos = Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo);
                        }

                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                        {
                            emailids = studDet.FirstOrDefault().AMST_FatheremailId;
                            mobilenos = Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo);
                        }
                        else
                        {
                            emailids = studDet.FirstOrDefault().AMST_emailId;
                            mobilenos = studDet.FirstOrDefault().AMST_MobileNo;
                        }
                        string TemplateName = "";
                        TemplateName = data.Edit_flag == false ? "ADMISSION_REGISTRATION" : "ADMISSION_REGISTRATION_UPDATE";

                        try
                        {
                            SMS sms = new SMS(_db);
                            string s = sms.sendSms(data.MI_Id, mobilenos, TemplateName, data.AMST_Id).Result;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        try
                        {
                            Email Email = new Email(_db);
                            string m = Email.sendmail(data.MI_Id, emailids, TemplateName, data.AMST_Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Admission Error Master Driver savedata" + ex.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO saveupdate_ecsdetails(Adm_M_StudentDTO mdd)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            if (mdd.Adm_M_Student_ECS.ASECS_Id > 0)
            {
                var resultecs = _AdmissionFormContext.Adm_Student_EcsDetailsDMO.Single(a => a.MI_Id == mdd.MI_Id && a.ASECS_Id == mdd.Adm_M_Student_ECS.ASECS_Id);
                resultecs.ASECS_AccountHolderName = mdd.Adm_M_Student_ECS.ASECS_AccountHolderName;
                resultecs.ASECS_AccountNo = mdd.Adm_M_Student_ECS.ASECS_AccountNo;
                resultecs.ASECS_AccountType = mdd.Adm_M_Student_ECS.ASECS_AccountType;
                resultecs.ASECS_BankName = mdd.Adm_M_Student_ECS.ASECS_BankName;
                resultecs.ASECS_Branch = mdd.Adm_M_Student_ECS.ASECS_Branch;
                resultecs.ASECS_MICRNo = mdd.Adm_M_Student_ECS.ASECS_MICRNo;
                resultecs.ASECS_ActiveFlg = true;
                resultecs.UpdatedDate = indiantime0;
                resultecs.ASECS_UpdatedBy = mdd.userid;
                var d = _AdmissionFormContext.Update(resultecs);
            }
            else
            {
                Adm_Student_EcsDetailsDMO resultecss = new Adm_Student_EcsDetailsDMO();
                resultecss.MI_Id = mdd.MI_Id;
                resultecss.ASECS_AccountHolderName = mdd.Adm_M_Student_ECS.ASECS_AccountHolderName;
                resultecss.ASECS_AccountNo = mdd.Adm_M_Student_ECS.ASECS_AccountNo;
                resultecss.ASECS_AccountType = mdd.Adm_M_Student_ECS.ASECS_AccountType;
                resultecss.ASECS_BankName = mdd.Adm_M_Student_ECS.ASECS_BankName;
                resultecss.ASECS_Branch = mdd.Adm_M_Student_ECS.ASECS_Branch;
                resultecss.ASECS_MICRNo = mdd.Adm_M_Student_ECS.ASECS_MICRNo;
                resultecss.AMST_Id = mdd.AMST_Id;
                resultecss.ASECS_ActiveFlg = true;
                resultecss.CreatedDate = indiantime0;
                resultecss.UpdatedDate = indiantime0;
                resultecss.ASECS_CreatedBy = mdd.userid;
                resultecss.ASECS_UpdatedBy = mdd.userid;
                var d = _AdmissionFormContext.Add(resultecss);
            }
            return mdd;
        }
        public Adm_M_StudentDTO savesecondtab(Adm_M_StudentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.message = "Update";
                data.returnval = false;

                if (data.AMST_Id > 0)
                {
                    var result = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id);
                    result.AMST_PerStreet = data.AMST_PerStreet;
                    result.AMST_PerArea = data.AMST_PerArea;
                    result.AMST_PerCity = data.AMST_PerCity;
                    result.AMST_PerState = data.AMST_PerState;
                    result.AMST_PerDistrict = data.AMST_PerDistrict;
                    result.AMST_PerCountry = data.AMST_PerCountry;
                    result.AMST_PerPincode = data.AMST_PerPincode;
                    result.AMST_ConStreet = data.AMST_ConStreet;
                    result.AMST_ConArea = data.AMST_ConArea;
                    result.AMST_ConCity = data.AMST_ConCity;
                    result.AMST_ConState = data.AMST_ConState;
                    result.AMST_ConDistrict = data.AMST_ConDistrict;
                    result.AMST_ConCountry = data.AMST_ConCountry;
                    result.AMST_ConPincode = data.AMST_ConPincode;
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = data.userid;
                    _AdmissionFormContext.Update(result);
                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        data.message = "Update";
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<Adm_M_StudentDTO> savethirdtab(Adm_M_StudentDTO mas)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                mas.message = "Update";
                mas.returnval = false;

                if (mas.AMST_Id > 0)
                {
                    var result = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == mas.MI_Id && a.AMST_Id == mas.AMST_Id);
                    result.AMST_FatherAliveFlag = mas.AMST_FatherAliveFlag;
                    result.AMST_FatherName = mas.AMST_FatherName;
                    result.AMST_FatherSurname = mas.AMST_FatherSurname;
                    result.AMST_FatherAadharNo = mas.AMST_FatherAadharNo;
                    result.AMST_FatherEducation = mas.AMST_FatherEducation;
                    result.AMST_FatherOfficeAdd = mas.AMST_FatherOfficeAdd;
                    result.AMST_FatherOccupation = mas.AMST_FatherOccupation;
                    result.AMST_FatherDesignation = mas.AMST_FatherDesignation;
                    result.AMST_FatherBankAccNo = mas.AMST_FatherBankAccNo;
                    result.AMST_FatherBankIFSC_Code = mas.AMST_FatherBankIFSC_Code;
                    result.AMST_FatherCasteCertiNo = mas.AMST_FatherCasteCertiNo;
                    result.AMST_FatherNationality = mas.AMST_FatherNationality;
                    result.AMST_FatherReligion = mas.AMST_FatherReligion;
                    result.AMST_FatherCaste = mas.AMST_FatherCaste;
                    result.AMST_FatherSubCaste = mas.AMST_FatherSubCaste;
                    result.AMST_FatherMonIncome = mas.AMST_FatherMonIncome;
                    result.AMST_FatherAnnIncome = mas.AMST_FatherAnnIncome;
                    result.AMST_FatherMobleNo = mas.AMST_FatherMobleNo;
                    result.AMST_FatheremailId = mas.AMST_FatheremailId;
                    result.AMST_FatherPANNo = mas.AMST_FatherPANNo;
                    result.AMST_FatherChurchAffiliation = mas.AMST_FatherChurchAffiliation;
                    result.AMST_FatherSelfEmployedFlg = mas.AMST_FatherSelfEmployedFlg;
                    result.AMST_FatherPresentAddress = mas.AMST_FatherPresentAddress;
                    result.AMST_FatherPresentCity = mas.AMST_FatherPresentCity;
                    result.AMST_FatherPresentState = mas.AMST_FatherPresentState;
                    result.AMST_FatherPresentPS = mas.AMST_FatherPresentPS;
                    result.AMST_FatherPresentPO = mas.AMST_FatherPresentPO;
                    result.AMST_FatherPresentPinCode = mas.AMST_FatherPresentPinCode;
                    result.AMST_FatherPermanentAddress = mas.AMST_FatherPermanentAddress;
                    result.AMST_FatherPermanentCity = mas.AMST_FatherPermanentCity;
                    result.AMST_FatherPermanentState = mas.AMST_FatherPermanentState;
                    result.AMST_FatherPermanentPS = mas.AMST_FatherPermanentPS;
                    result.AMST_FatherPermanentPO = mas.AMST_FatherPermanentPO;
                    result.AMST_FatherPermanentPinCode = mas.AMST_FatherPermanentPinCode;
                    result.AMST_FatherMaritalStatus = mas.AMST_FatherMaritalStatus;
                    result.AMST_FatherBankName = mas.AMST_FatherBankName;
                    result.AMST_FatherBankBranch = mas.AMST_FatherBankBranch;
                    result.AMST_FatherHomePhNo = mas.AMST_FatherHomePhNo;
                    result.AMST_FatherOfficePhNo = mas.AMST_FatherOfficePhNo;
                    result.AMST_FatherTribe = mas.AMST_FatherTribe;
                    result.AMST_FatherPassingYear = mas.AMST_FatherPassingYear;

                    //Mother details
                    result.AMST_MotherAliveFlag = mas.MotherAlive;
                    result.AMST_MotherName = mas.MotherName;
                    result.AMST_MotherSurname = mas.MotherSurname;
                    result.AMST_MotherAadharNo = mas.MotherAadharNo;
                    result.AMST_MotherEducation = mas.MotherEducation;
                    result.AMST_MotherOfficeAdd = mas.MotherOfficesAddress;
                    result.AMST_MotherOccupation = mas.MotherOcupation;
                    result.AMST_MotherDesignation = mas.MotherDesignation;
                    result.AMST_MotherBankAccNo = mas.MotherBankAccountNo;
                    result.AMST_MotherBankIFSC_Code = mas.MotherIFSCcode;
                    result.AMST_MotherCasteCertiNo = mas.MotherCasteCertificateNo;
                    result.AMST_MotherReligion = mas.AMST_MotherReligion;
                    result.AMST_MotherCaste = mas.AMST_MotherCaste;
                    result.AMST_MotherSubCaste = mas.AMST_MotherSubCaste;
                    result.AMST_MotherMonIncome = mas.MotherMonthlyIncome;
                    result.AMST_MotherAnnIncome = mas.MotherAnnualIncome;
                    result.AMST_MotherMobileNo = mas.AMST_MotherMobileNo;
                    result.AMST_MotherEmailId = mas.AMST_MotherEmailId;
                    result.AMST_MotherNationality = mas.AMST_MotherNationality;
                    result.AMST_MotherPANNo = mas.AMST_MotherPANNo;
                    result.AMST_MotherChurchAffiliation = mas.AMST_MotherChurchAffiliation;
                    result.AMST_MotherSelfEmployedFlg = mas.AMST_MotherSelfEmployedFlg;
                    result.AMST_MotherPresentAddress = mas.AMST_MotherPresentAddress;
                    result.AMST_MotherPresentCity = mas.AMST_MotherPresentCity;
                    result.AMST_MotherPresentState = mas.AMST_MotherPresentState;
                    result.AMST_MotherPresentPS = mas.AMST_MotherPresentPS;
                    result.AMST_MotherPresentPO = mas.AMST_MotherPresentPO;
                    result.AMST_MotherPresentPinCode = mas.AMST_MotherPresentPinCode;
                    result.AMST_MotherPermanentAddress = mas.AMST_MotherPermanentAddress;
                    result.AMST_MotherPermanentCity = mas.AMST_MotherPermanentCity;
                    result.AMST_MotherPermanentState = mas.AMST_MotherPermanentState;
                    result.AMST_MotherPermanentPS = mas.AMST_MotherPermanentPS;
                    result.AMST_MotherPermanentPO = mas.AMST_MotherPermanentPO;
                    result.AMST_MotherPermanentPinCode = mas.AMST_MotherPermanentPinCode;
                    result.AMST_MotherMaritalStatus = mas.AMST_MotherMaritalStatus;
                    result.AMST_MotherBankName = mas.AMST_MotherBankName;
                    result.AMST_MotherBankBranch = mas.AMST_MotherBankBranch;
                    result.AMST_MotherPassingYear = mas.AMST_MotherPassingYear;
                    result.AMST_MotherOfficePhNo = mas.AMST_MotherOfficePhNo;
                    result.AMST_MotherHomePhNo = mas.AMST_MotherHomePhNo;
                    result.AMST_MotherTribe = mas.AMST_MotherTribe;
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = mas.userid;

                    //father mobile saving
                    string fathermobile = "";
                    for (int j = 0; j < mas.multiplemobileno.Length; j++)
                    {
                        fathermobile = Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);

                        if (j == 0)
                        {
                            if (fathermobile != null && fathermobile != "")
                            {
                                fathermobile = Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                            }
                        }
                    }

                    //father email id saving
                    string fatheremail = "";
                    for (int j = 0; j < mas.multipleemailid.Length; j++)
                    {
                        fatheremail = Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        if (j == 0)
                        {
                            fatheremail = Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        }
                    }

                    //mother mobile saving
                    string mothermobile = "";
                    for (int j = 0; j < mas.multiplemobilenomother.Length; j++)
                    {
                        mothermobile = Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                        if (j == 0)
                        {
                            if (mothermobile != null)
                            {
                                mothermobile = Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                            }
                        }
                    }

                    //mother email id saving
                    string motheremail = "";
                    for (int j = 0; j < mas.multipleemailidmother.Length; j++)
                    {
                        motheremail = Convert.ToString(mas.multipleemailidmother[0].AMST_MotherEmailId);
                        if (j == 0)
                        {
                            motheremail = Convert.ToString(mas.multipleemailidmother[0].AMST_MotherEmailId);
                        }
                    }
                    if (fathermobile != "")
                    {
                        result.AMST_FatherMobleNo = Convert.ToInt64(fathermobile);
                    }
                    result.AMST_FatheremailId = fatheremail;

                    if (mothermobile != "")
                    {
                        result.AMST_MotherMobileNo = Convert.ToInt64(mothermobile);
                    }

                    result.AMST_MotherEmailId = motheremail;

                    _AdmissionFormContext.Update(result);

                    father_mobile_no(mas);
                    father_email_ids(mas);
                    mother_mobile_no(mas);
                    mother_email_id(mas);


                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.message = "Update";
                        mas.returnval = true;
                        try
                        {
                            var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == mas.AMST_Id).Select(a => a.FAT_APP_ID).ToList();

                            ApplicationUser user = new ApplicationUser();
                            user = await _UserManager.FindByIdAsync(getstdappid[0].ToString());
                            user.PhoneNumber = fathermobile;
                            user.Email = fatheremail;
                            user.NormalizedEmail = fatheremail;
                            var i = await _UserManager.UpdateAsync(user);

                            var getmappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == mas.AMST_Id).Select(a => a.MOT_APP_ID).ToList();
                            ApplicationUser user1 = new ApplicationUser();
                            user1 = await _UserManager.FindByIdAsync(getmappid[0].ToString());
                            user1.PhoneNumber = mothermobile;
                            user1.Email = motheremail;
                            user.NormalizedEmail = motheremail;
                            var ii = await _UserManager.UpdateAsync(user1);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mas;
        }
        public Adm_M_StudentDTO savefourthtab(Adm_M_StudentDTO mas)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (mas.AMST_Id > 0)
                {
                    var result = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == mas.MI_Id && a.AMST_Id == mas.AMST_Id);
                    result.AMST_NoOfElderBrothers = mas.AMST_NoOfElderBrothers;
                    result.AMST_NoOfYoungerBrothers = mas.AMST_NoOfYoungerBrothers;
                    result.AMST_NoOfElderSisters = mas.AMST_NoOfElderSisters;
                    result.AMST_NoOfYoungerSisters = mas.AMST_NoOfYoungerSisters;
                    result.AMST_Noofbrothers = mas.AMST_Noofbrothers;

                    result.AMST_NoOfSiblings = mas.AMST_NoOfSiblings;
                    result.AMST_NoOfSiblingsSchool = mas.AMST_NoOfSiblingsSchool;
                    result.AMST_NoOfDependencies = mas.AMST_NoOfDependencies;

                    result.AMST_ChurchName = mas.AMST_ChurchName;
                    result.AMST_ChurchAddress = mas.AMST_ChurchAddress;

                    result.AMST_Noofsisters = mas.AMST_Noofsisters;
                    result.AMST_ChurchBaptisedDate = mas.AMST_ChurchBaptisedDate;
                    result.AMST_Village = mas.AMST_Village;
                    result.AMST_Town = mas.AMST_Town;
                    result.AMST_Distirct = mas.AMST_Distirct;
                    result.AMST_Taluk = mas.AMST_Taluk;
                    result.AMST_PlaceOfBirthState = mas.AMST_PlaceOfBirthState;
                    result.AMST_PlaceOfBirthCountry = mas.AMST_PlaceOfBirthCountry;

                    ActivityAddUppdate(mas);
                    RefferenceDetailsAddUpdate(mas);
                    SourceDetailsAddUpdate(mas);
                    BondAddUpdate(mas);
                    SiblingsAddUpdate(mas);
                    AchivementAddUpdate(mas);
                    PrevSchooldetailsAddUpdate(mas);
                    GuardianAddUpdate(mas);
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = mas.userid;
                    _AdmissionFormContext.Update(result);
                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.message = "Update";
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.message = "Update";
                        mas.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mas;
        }
        public Adm_M_StudentDTO savesixthtab(Adm_M_StudentDTO mas)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (mas.AMST_Id > 0)
                {
                    var result = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == mas.MI_Id && a.AMST_Id == mas.AMST_Id);
                    result.AMST_Studentillness = mas.AMST_Studentillness;
                    result.AMST_Illnessdetails = mas.AMST_Illnessdetails;
                    result.AMST_UnderAge = mas.AMST_UnderAge;
                    result.AMST_MedicalComplaints = mas.AMST_MedicalComplaints;
                    result.AMST_Boarding = mas.AMST_Boarding;
                    result.AMST_LastPlayGrndAttnd = mas.AMST_LastPlayGrndAttnd;
                    result.AMST_AdmissionReason = mas.AMST_AdmissionReason;
                    result.AMST_OtherResidential_Addr = mas.AMST_OtherResidential_Addr;
                    result.AMST_SchoolDISECode = mas.AMST_SchoolDISECode;
                    result.AMST_FirstLanguage = mas.AMST_FirstLanguage;
                    result.AMST_Thirdlanguage = mas.AMST_Thirdlanguage;
                    result.AMST_OverAge = mas.AMST_OverAge;
                    result.AMST_OtherInformations = mas.AMST_OtherInformations;
                    result.AMST_ExtraActivity = mas.AMST_ExtraActivity;
                    result.AMST_Stayingwith = mas.AMST_Stayingwith;
                    result.AMST_MaritalStatus = mas.AMST_MaritalStatus;
                    result.AMST_OtherPermanentAddr = mas.AMST_OtherPermanentAddr;
                    result.AMST_Domicile = mas.AMST_Domicile;
                    result.AMST_SecondLanguage = mas.AMST_SecondLanguage;
                    result.AMST_TransferrableJobFlg = mas.AMST_TransferrableJobFlg;
                    result.AMST_VaccinatedFlg = mas.AMST_VaccinatedFlg;
                    result.AMST_Tcflag = mas.AMST_Tcflag;
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = mas.userid;
                    _AdmissionFormContext.Update(result);
                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.message = "Update";
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.message = "Update";
                        mas.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mas;
        }
        public Adm_M_StudentDTO savefinaltab(Adm_M_StudentDTO mas)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (mas.AMST_Id > 0)
                {
                    var result = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == mas.MI_Id && a.AMST_Id == mas.AMST_Id);
                    stud_doc_upload(mas);
                    result.AMST_GymReqdFlag = mas.AMST_GymReqdFlag;
                    result.AMST_HostelReqdFlag = mas.AMST_HostelReqdFlag;
                    result.AMST_TransportReqdFlag = mas.AMST_TransportReqdFlag;
                    result.ANST_FatherPhoto = mas.ANST_FatherPhoto;
                    result.ANST_MotherPhoto = mas.ANST_MotherPhoto;
                    result.AMST_Father_Signature = mas.AMST_Father_Signature;
                    result.AMST_Father_FingerPrint = mas.AMST_Father_FingerPrint;
                    result.AMST_Mother_Signature = mas.AMST_Mother_Signature;
                    result.AMST_Mother_FingerPrint = mas.AMST_Mother_FingerPrint;
                    result.UpdatedDate = indiantime0;
                    result.AMST_UpdatedBy = mas.userid;
                    _AdmissionFormContext.Update(result);
                    int n = _AdmissionFormContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.message = "Update";
                        mas.returnval = true;
                    }
                    else
                    {
                        mas.message = "Update";
                        mas.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mas;
        }
        public async Task<ImportStudentWrapperDTO> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            ImportStudentWrapperDTO respdto = new ImportStudentWrapperDTO();
            //string resp = ""; 
            //Creating Student and parents login as well as Sending user name and password code starts.
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _UserManager.FindByNameAsync(name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = name, Email = emailid, PhoneNumber = mobile };
                    user.Entry_Date = DateTime.Now;
                    user.EmailConfirmed = true;
                    var result = await _UserManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        // Student Roles
                        string studentRole = roles;
                        var id = _db.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = roletype;
                        var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);
                        _db.SaveChanges();
                        respdto.useridapp = role.UserId;
                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        _db.Add(mas1);
                        var res = _db.SaveChanges();
                        if (res > 0)
                        {
                            respdto.resp = "Success";
                        }
                        else
                        {
                            respdto.resp = "";
                        }

                    }
                    else
                    {
                        respdto.resp = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Admission form error");
                _log.LogDebug(e.Message);
            }
            return respdto;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }
        public bool AddStudentUserLogin(long mi_id, string username, long amstId)
        {
            StudentUserLoginDMO dmo = new StudentUserLoginDMO();
            dmo.AMST_Id = amstId;
            dmo.CreatedDate = DateTime.Now;
            dmo.IVRMSTUUL_ActiveFlag = 1;
            dmo.IVRMSTUUL_Password = "Password@123";
            dmo.IVRMSTUUL_UserName = username;
            dmo.MI_Id = mi_id;
            dmo.UpdatedDate = DateTime.Now;
            _AdmissionFormContext.Add(dmo);
            var flag = _AdmissionFormContext.SaveChanges();
            if (flag > 0)
            {
                StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
                inst.AMST_Id = amstId;
                inst.CreatedDate = DateTime.Now;
                inst.IVRMSTUULI_ActiveFlag = 1;
                inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
                inst.UpdatedDate = DateTime.Now;
                _AdmissionFormContext.Add(inst);
                var flag1 = _AdmissionFormContext.SaveChanges();
                if (flag1 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool AddStudentApplogin(long userid, long fatherid, long motherid, long amstId)
        {
            StudentAppUserLoginDMO dmo = new StudentAppUserLoginDMO();
            dmo.AMST_ID = amstId;
            //  dmo.CreatedDate = DateTime.Now;
            dmo.STD_APP_ID = Convert.ToInt32(userid);
            dmo.FAT_APP_ID = Convert.ToInt32(fatherid);
            dmo.MOT_APP_ID = Convert.ToInt32(motherid);

            //   dmo.UpdatedDate = DateTime.Now;
            _AdmissionFormContext.Add(dmo);
            var flag = _AdmissionFormContext.SaveChanges();
            if (flag > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        /*  Admission Cancel OR WithDraw   */
        public Adm_M_StudentDTO OnLoadAdmissionCancel(Adm_M_StudentDTO data)
        {
            try
            {
                List<MasterAcademic> allyearonload = new List<MasterAcademic>();
                allyearonload = _AdmissionFormContext.year.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true
                && d.ASMAY_Year != null).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.academicYearOnLoad = allyearonload.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _AdmissionFormContext.year.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true && d.ASMAY_Id == data.ASMAY_Id
                && d.ASMAY_Year != null).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.AllAcademicYear = allyear.ToArray();

                data.getstudentdetailslist = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              where (adm_stu.MI_Id == data.MI_Id && adm_stu.AMST_ActiveFlag == 1 && adm_stu.AMST_SOL == "S"
                                              && adm_stu.ASMAY_Id == data.ASMAY_Id)
                                              select new Adm_M_StudentDTO
                                              {
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName)
                                                  + (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName)
                                                  + (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)
                                                  + (adm_stu.AMST_AdmNo == null || adm_stu.AMST_AdmNo == "" ? "" : " : " + adm_stu.AMST_AdmNo)).Trim(),
                                                  AMST_Id = adm_stu.AMST_Id
                                              }).Distinct().ToArray();


                data.getwdstudentdetails = (from a in _AdmissionFormContext.Adm_M_Student
                                            from b in _AdmissionFormContext.Adm_AdmissionCancelDMO
                                            from c in _AdmissionFormContext.AcademicYear
                                            from d in _AdmissionFormContext.School_M_Class
                                            where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id
                                            && b.MI_Id == data.MI_Id)
                                            select new Adm_M_StudentDTO
                                            {
                                                studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName)
                                                  + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName)
                                                  + (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_Id = a.AMST_Id,
                                                AACA_Id = b.AACA_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                AACA_ACReason = b.AACA_ACReason,
                                                AACA_CancellationFee = b.AACA_CancellationFee,
                                                AACA_ToRefundAmount = b.AACA_ToRefundAmount,
                                                AACA_ACDate = b.AACA_ACDate,
                                                AMST_Date = a.AMST_Date,
                                                ASMAY_Year = c.ASMAY_Year,
                                                ASMAY_Id = b.ASMAY_Id,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = b.ASMS_Id != null ? _AdmissionFormContext.AdmSection.Where(e => e.ASMS_Id == b.ASMS_Id && e.MI_Id == data.MI_Id).FirstOrDefault().ASMC_SectionName : "",
                                            }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                _log.LogInformation("GetData() in admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO OnChangeAdmissionCancelYear(Adm_M_StudentDTO data)
        {
            try
            {
                data.getstudentdetailslist = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              where (adm_stu.MI_Id == data.MI_Id && adm_stu.AMST_ActiveFlag == 1 && adm_stu.AMST_SOL == "S"
                                              && adm_stu.ASMAY_Id == data.ASMAY_Id)
                                              select new Adm_M_StudentDTO
                                              {
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName)
                                                  + (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName)
                                                  + (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)
                                                  + (adm_stu.AMST_AdmNo == null || adm_stu.AMST_AdmNo == "" ? "" : " : " + adm_stu.AMST_AdmNo)).Trim(),
                                                  AMST_Id = adm_stu.AMST_Id
                                              }).Distinct().ToArray();

                data.getwdstudentdetails = (from a in _AdmissionFormContext.Adm_M_Student
                                            from b in _AdmissionFormContext.Adm_AdmissionCancelDMO
                                            from c in _AdmissionFormContext.AcademicYear
                                            from d in _AdmissionFormContext.School_M_Class
                                            where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id
                                            && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                            select new Adm_M_StudentDTO
                                            {
                                                studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName)
                                                  + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName)
                                                  + (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_Id = a.AMST_Id,
                                                AACA_Id = b.AACA_Id,
                                                AMSTAdmNo = a.AMST_AdmNo,
                                                AACA_ACReason = b.AACA_ACReason,
                                                AACA_CancellationFee = b.AACA_CancellationFee,
                                                AACA_ToRefundAmount = b.AACA_ToRefundAmount,
                                                AACA_ACDate = b.AACA_ACDate,
                                                AMST_Date = a.AMST_Date,
                                                ASMAY_Year = c.ASMAY_Year,
                                                ASMAY_Id = b.ASMAY_Id,
                                                ASMCL_ClassName = d.ASMCL_ClassName
                                            }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                _log.LogInformation("GetData() in admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO OnChangeAdmissionCancelStudent(Adm_M_StudentDTO data)
        {
            try
            {
                data.getstudentdetails = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                          from adm_cls in _AdmissionFormContext.School_M_Class
                                          where (adm_stu.ASMCL_Id == adm_cls.ASMCL_Id && adm_stu.MI_Id == data.MI_Id && adm_stu.AMST_ActiveFlag == 1
                                          && adm_stu.AMST_SOL == "S" && adm_stu.ASMAY_Id == data.ASMAY_Id && adm_stu.AMST_Id == data.AMST_Id)
                                          select new Adm_M_StudentDTO
                                          {
                                              studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName)
                                              + (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName)
                                              + (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim(),
                                              AMST_Id = adm_stu.AMST_Id,
                                              PhotoName = adm_stu.AMST_Photoname,
                                              ASMCL_Id = adm_stu.ASMCL_Id,
                                              classname = adm_cls.ASMCL_ClassName,
                                              AMST_Date = adm_stu.AMST_Date,

                                              AMST_FatherName = ((adm_stu.AMST_FatherName == null || adm_stu.AMST_FatherName == "" ? "" : adm_stu.AMST_FatherName)
                                              + (adm_stu.AMST_FatherSurname == null || adm_stu.AMST_FatherSurname == "" ? "" : " " + adm_stu.AMST_FatherSurname)),

                                              AMST_MotherName = ((adm_stu.AMST_MotherName == null || adm_stu.AMST_MotherName == "" ? "" : adm_stu.AMST_MotherName)
                                              + (adm_stu.AMST_MotherSurname == null || adm_stu.AMST_MotherSurname == "" ? "" : " " + adm_stu.AMST_MotherSurname)),

                                              AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                              AMST_AdmNo = adm_stu.AMST_AdmNo,

                                          }).Distinct().ToArray();



            }
            catch (Exception e)
            {
                _log.LogInformation("GetData() in admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO SaveAdmissionCancelStudent(Adm_M_StudentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.AACA_Id > 0)
                {
                    var result = _AdmissionFormContext.Adm_AdmissionCancelDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id
                    && a.AACA_Id == data.AACA_Id);
                    result.AACA_ACReason = data.AACA_ACReason;
                    result.AACA_CancellationFee = data.AACA_CancellationFee;
                    result.AACA_ToRefundAmount = data.AACA_ToRefundAmount;
                    result.AACA_UpdatedBy = data.userid;
                    result.AACA_UpdatedDate = indiantime0;
                    result.AACA_ACDate = data.AACA_ACDate;
                    _AdmissionFormContext.Update(result);

                    var i = _AdmissionFormContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    var getstudent = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                      && a.AMST_Id == data.AMST_Id).ToList();

                    var getyearlystudent = _AdmissionFormContext.SchoolYearWiseStudent.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                    && a.AMAY_ActiveFlag == 1).ToList();

                    long? ASMS_Id = null;
                    if (getyearlystudent.Count > 0)
                    {
                        ASMS_Id = getyearlystudent.FirstOrDefault().ASMS_Id;
                    }

                    Adm_AdmissionCancelDMO adm_AdmissionCancelDMO = new Adm_AdmissionCancelDMO
                    {
                        AMST_Id = data.AMST_Id,
                        MI_Id = data.MI_Id,
                        ASMAY_Id = data.ASMAY_Id,
                        ASMCL_Id = Convert.ToInt64(getstudent.FirstOrDefault().ASMCL_Id),
                        ASMS_Id = ASMS_Id,
                        AACA_ACReason = data.AACA_ACReason,
                        AACA_ACDate = data.AACA_ACDate,
                        AACA_CancellationFee = data.AACA_CancellationFee,
                        AACA_ToRefundAmount = data.AACA_ToRefundAmount,
                        AACA_ActiveFlag = true,
                        AACA_CreatedBy = data.userid,
                        AACA_UpdatedBy = data.userid,
                        AACA_CreatedDate = indiantime0,
                        AACA_UpdatedDate = indiantime0
                    };

                    _AdmissionFormContext.Add(adm_AdmissionCancelDMO);

                    var getstudentdetails = _AdmissionFormContext.Adm_M_Student.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.AMST_Id == data.AMST_Id);

                    getstudentdetails.AMST_SOL = "WD";
                    getstudentdetails.AMST_ActiveFlag = 0;
                    getstudentdetails.UpdatedDate = indiantime0;
                    getstudentdetails.AMST_UpdatedBy = data.userid;
                    _AdmissionFormContext.Update(getstudentdetails);

                    var i = _AdmissionFormContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("GetData() in admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO EditAdmissionCancelStudent(Adm_M_StudentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.getstudentdetailslist = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                              where (adm_stu.MI_Id == data.MI_Id && adm_stu.AMST_ActiveFlag == 1 && (adm_stu.AMST_SOL == "S"
                                              || adm_stu.AMST_SOL == "WD") && adm_stu.ASMAY_Id == data.ASMAY_Id)
                                              select new Adm_M_StudentDTO
                                              {
                                                  studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName)
                                                  + (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName)
                                                  + (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)
                                                  + (adm_stu.AMST_AdmNo == null || adm_stu.AMST_AdmNo == "" ? "" : " : " + adm_stu.AMST_AdmNo)).Trim(),
                                                  AMST_Id = adm_stu.AMST_Id
                                              }).Distinct().ToArray();

                data.geteditdetails = _AdmissionFormContext.Adm_AdmissionCancelDMO.Where(a => a.MI_Id == data.MI_Id && a.AACA_Id == data.AACA_Id).ToArray();

                data.getstudentdetails = (from adm_stu in _AdmissionFormContext.Adm_M_Student
                                          from adm_cls in _AdmissionFormContext.School_M_Class
                                          where (adm_stu.ASMCL_Id == adm_cls.ASMCL_Id && adm_stu.MI_Id == data.MI_Id && adm_stu.AMST_ActiveFlag == 1
                                          && (adm_stu.AMST_SOL == "S" || adm_stu.AMST_SOL == "WD") && adm_stu.ASMAY_Id == data.ASMAY_Id && adm_stu.AMST_Id == data.AMST_Id)
                                          select new Adm_M_StudentDTO
                                          {
                                              studentname = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName)
                                                  + (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName)
                                                  + (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)
                                                  + (adm_stu.AMST_AdmNo == null || adm_stu.AMST_AdmNo == "" ? "" : " : " + adm_stu.AMST_AdmNo)).Trim(),

                                              AMST_FirstName = ((adm_stu.AMST_FirstName == null || adm_stu.AMST_FirstName == "" ? "" : adm_stu.AMST_FirstName)
                                              + (adm_stu.AMST_MiddleName == null || adm_stu.AMST_MiddleName == "" ? "" : " " + adm_stu.AMST_MiddleName)
                                              + (adm_stu.AMST_LastName == null || adm_stu.AMST_LastName == "" ? "" : " " + adm_stu.AMST_LastName)).Trim(),
                                              AMST_Id = adm_stu.AMST_Id,
                                              PhotoName = adm_stu.AMST_Photoname,
                                              ASMCL_Id = adm_stu.ASMCL_Id,
                                              classname = adm_cls.ASMCL_ClassName,
                                              AMST_Date = adm_stu.AMST_Date,

                                              AMST_FatherName = ((adm_stu.AMST_FatherName == null || adm_stu.AMST_FatherName == "" ? "" : adm_stu.AMST_FatherName)
                                              + (adm_stu.AMST_FatherSurname == null || adm_stu.AMST_FatherSurname == "" ? "" : " " + adm_stu.AMST_FatherSurname)),

                                              AMST_MotherName = ((adm_stu.AMST_MotherName == null || adm_stu.AMST_MotherName == "" ? "" : adm_stu.AMST_MotherName)
                                              + (adm_stu.AMST_MotherSurname == null || adm_stu.AMST_MotherSurname == "" ? "" : " " + adm_stu.AMST_MotherSurname)),

                                              AMST_RegistrationNo = adm_stu.AMST_RegistrationNo,
                                              AMST_AdmNo = adm_stu.AMST_AdmNo,

                                          }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                _log.LogInformation("GetData() in admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }

        // Admission Cancel Report
        public Adm_M_StudentDTO OnLoadAdmissionCancelReport(Adm_M_StudentDTO data)
        {
            try
            {
                List<MasterAcademic> allyearonload = new List<MasterAcademic>();
                allyearonload = _AdmissionFormContext.year.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true
                && d.ASMAY_Year != null && d.ASMAY_Id == data.ASMAY_Id).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.academicYearOnLoad = allyearonload.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _AdmissionFormContext.year.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true
                && d.ASMAY_Year != null).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.AllAcademicYear = allyear.ToArray();

                data.getwdstudentdetails = (from a in _AdmissionFormContext.Adm_M_Student
                                            from b in _AdmissionFormContext.Adm_AdmissionCancelDMO
                                            from c in _AdmissionFormContext.AcademicYear
                                            from d in _AdmissionFormContext.School_M_Class
                                            where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id
                                            && b.MI_Id == data.MI_Id)
                                            select new Adm_M_StudentDTO
                                            {
                                                studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName)
                                                  + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName)
                                                  + (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_Id = a.AMST_Id,
                                                AACA_Id = b.AACA_Id,
                                                AMST_AdmNo = a.AMST_AdmNo,
                                                AACA_ACReason = b.AACA_ACReason,
                                                AACA_CancellationFee = b.AACA_CancellationFee,
                                                AACA_ToRefundAmount = b.AACA_ToRefundAmount,
                                                AACA_ACDate = b.AACA_ACDate,
                                                AMST_Date = a.AMST_Date,
                                                ASMAY_Year = c.ASMAY_Year,
                                                ASMAY_Id = b.ASMAY_Id,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                ASMC_SectionName = b.ASMS_Id != null ? _AdmissionFormContext.AdmSection.Where(e => e.ASMS_Id == b.ASMS_Id
                                                && e.MI_Id == data.MI_Id).FirstOrDefault().ASMC_SectionName : "",
                                            }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                _log.LogInformation("admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO OnChangeAdmissionCancelReportYear(Adm_M_StudentDTO data)
        {
            try
            {
                data.getwdstudentdetails = (from a in _AdmissionFormContext.Adm_M_Student
                                            from b in _AdmissionFormContext.Adm_AdmissionCancelDMO
                                            from c in _AdmissionFormContext.AcademicYear
                                            from d in _AdmissionFormContext.School_M_Class
                                            where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id
                                            && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                            select new Adm_M_StudentDTO
                                            {
                                                studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName)
                                                  + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName)
                                                  + (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                                AMST_Id = a.AMST_Id,
                                                AACA_Id = b.AACA_Id,
                                                AMSTAdmNo = a.AMST_AdmNo,
                                                AACA_ACReason = b.AACA_ACReason,
                                                AACA_CancellationFee = b.AACA_CancellationFee,
                                                AACA_ToRefundAmount = b.AACA_ToRefundAmount,
                                                AACA_ACDate = b.AACA_ACDate,
                                                AMST_Date = a.AMST_Date,
                                                ASMAY_Year = c.ASMAY_Year,
                                                ASMAY_Id = b.ASMAY_Id,
                                                ASMCL_ClassName = d.ASMCL_ClassName
                                            }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                _log.LogInformation("in admission form");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public Adm_M_StudentDTO ViewStudentProfile(Adm_M_StudentDTO data)
        {
            try
            {
                data.viewstudentjoineddetails = (from a in _AdmissionFormContext.Adm_M_Student
                                                 from b in _AdmissionFormContext.AcademicYear
                                                 from c in _AdmissionFormContext.School_M_Class
                                                 where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.AMST_Id == data.AMST_Id
                                                 && a.MI_Id == data.MI_Id)
                                                 select new Adm_M_StudentDTO
                                                 {
                                                     studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                     (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                     (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                                     AMST_AdmNo = a.AMST_AdmNo,
                                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                     ASMAY_Year = b.ASMAY_Year,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     AMST_Photoname = a.AMST_Photoname,
                                                     AMST_Sex = a.AMST_Sex,
                                                     AMST_SOL = a.AMST_SOL,
                                                     AMST_Date = a.AMST_Date,
                                                     AMST_DOB = a.AMST_DOB,
                                                 }).Distinct().ToArray();

                data.viewstudentdetails = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToArray();

                data.viewstudentacademicyeardetails = (from a in _AdmissionFormContext.SchoolYearWiseStudent
                                                       from b in _AdmissionFormContext.AcademicYear
                                                       from c in _AdmissionFormContext.School_M_Class
                                                       from d in _AdmissionFormContext.AdmSection
                                                       where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                                       && a.AMST_Id == data.AMST_Id)
                                                       select new Adm_M_StudentDTO
                                                       {
                                                           ASMAY_Year = b.ASMAY_Year,
                                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                                           ASMC_SectionName = d.ASMC_SectionName,
                                                           order = b.ASMAY_Order,
                                                           ASMAY_Id = a.ASMAY_Id,
                                                           AMAY_RollNo = a.AMAY_RollNo,
                                                           Status_Flag = a.ASMAY_Id == data.ASMAY_Id ? "Current Year" : ""
                                                       }).Distinct().OrderByDescending(a => a.order).ToArray();


                data.viewstudentguardiandetails = _AdmissionFormContext.StudentGuardianDMO.Where(a => a.AMST_Id == data.AMST_Id).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendancetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentWise_Address_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentaddressdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.viewstudentsubjectdetails = (from a in _AdmissionFormContext.StudentMappingDMO
                //                                  from b in _AdmissionFormContext.IVRM_Master_SubjectsDMO
                //                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                //                                  select new Adm_M_StudentDTO
                //                                  {
                //                                      ISMS_Id = a.ISMS_Id,
                //                                      ISMS_SubjectName = b.ISMS_SubjectName,
                //                                      subjorder = b.ISMS_OrderFlag,
                //                                      ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag
                //                                  }).Distinct().OrderBy(a => a.subjorder).ToArray();



                data.viewstudentsubjectdetails = (from a in _AdmissionFormContext.StudentMappingDMO
                                                  from b in _AdmissionFormContext.IVRM_Master_SubjectsDMO
                                                  from c in _AdmissionFormContext.SchoolYearWiseStudent
                                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == c.AMST_Id)
                                                  select new Adm_M_StudentDTO
                                                  {
                                                      ISMS_Id = a.ISMS_Id,
                                                      ISMS_SubjectName = b.ISMS_SubjectName,
                                                      subjorder = b.ISMS_OrderFlag,
                                                      ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag
                                                  }).Distinct().OrderBy(a => a.subjorder).ToArray();
                //adedd new
                using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_LibraryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
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
                        data.librarydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                _log.LogInformation("in admission form");
                _log.LogDebug(e.Message);
            }
            return data;
        }

        // No use 
        public Adm_M_StudentDTO SaveData(Adm_M_StudentDTO mas)
        {

            try
            {
                Adm_M_Student MM2 = Mapper.Map<Adm_M_Student>(mas);
                if (mas.AMST_Id > 0)
                {
                    mas.Edit_flag = true;
                    var result = _AdmissionFormContext.Adm_M_Student.Single(t => t.MI_Id == mas.MI_Id && t.AMST_Id == mas.AMST_Id);

                    mas.AMST_SOL = result.AMST_SOL;
                    mas.AMST_Date = Convert.ToDateTime(mas.AMST_Date);
                    mas.AMST_DOB = Convert.ToDateTime(mas.AMST_DOB);
                    // result.AMST_PaymentDate = Convert.ToDateTime(result.AMST_PaymentDate);
                    mas.UpdatedDate = DateTime.Now;
                    mas.AMST_ActiveFlag = result.AMST_ActiveFlag;
                    mas.AMST_PaymentFlag = result.AMST_PaymentFlag;
                    mas.AMST_PaymentType = result.AMST_PaymentType;
                    mas.AMST_PaymentDate = result.AMST_PaymentDate;
                    mas.AMST_AmountPaid = result.AMST_AmountPaid;
                    mas.AMST_ReceiptNo = result.AMST_ReceiptNo;
                    mas.AMST_ApplStatus = result.AMST_ApplStatus;
                    mas.AMST_FinalpaymentFlag = result.AMST_FinalpaymentFlag;
                    mas.AMST_Noofbrothers = result.AMST_Noofbrothers;
                    mas.AMST_Noofsisters = result.AMST_Noofsisters;

                    result.AMST_Photoname = mas.AMST_Photoname;
                    result.ANST_FatherPhoto = mas.ANST_FatherPhoto;
                    result.ANST_MotherPhoto = mas.ANST_MotherPhoto;
                    result.AMST_Father_Signature = mas.AMST_Father_Signature;
                    result.AMST_Father_FingerPrint = mas.AMST_Father_FingerPrint;
                    result.AMST_Mother_Signature = mas.AMST_Mother_Signature;
                    result.AMST_Mother_FingerPrint = mas.AMST_Mother_FingerPrint;
                    result.IC_Id = mas.IMC_Id;
                    result.AMST_SubCasteIMC_Id = mas.AMST_SubCasteIMC_Id;
                    result.AMST_MotherReligion = mas.AMST_MotherReligion;
                    result.AMST_MotherCaste = mas.AMST_MotherCaste;
                    result.AMST_MotherSubCaste = mas.AMST_MotherSubCaste;
                    result.AMST_FatherReligion = mas.AMST_FatherReligion;
                    result.AMST_FatherCaste = mas.AMST_FatherCaste;
                    result.AMST_FatherSubCaste = mas.AMST_FatherSubCaste;

                    string stdmobileno = "";
                    string stdmobilenosms = "";
                    string stdemailsms = "";

                    var count = mas.Adm_M_Student_MobileNoDTO.Length;
                    for (int i = 0; i < count; i++)
                    {
                        stdmobilenosms = mas.Adm_M_Student_MobileNoDTO[0].AMST_MobileNo;
                        if (i == 0)
                        {
                            stdmobileno = mas.Adm_M_Student_MobileNoDTO[i].AMST_MobileNo;
                        }
                    }

                    //student email id saving
                    string stdemailids = "";
                    for (int j = 0; j < mas.Adm_M_Student_EmailIdDTO.Length; j++)
                    {
                        stdemailsms = mas.Adm_M_Student_EmailIdDTO[0].AMST_emailId;
                        if (j == 0)
                        {
                            stdemailids = mas.Adm_M_Student_EmailIdDTO[j].AMST_emailId;
                        }
                    }

                    //father mobile saving
                    string fathermobile = "";
                    for (int j = 0; j < mas.multiplemobileno.Length; j++)
                    {
                        fathermobile = Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                        if (j == 0)
                        {
                            if (fathermobile != "")
                            {
                                fathermobile = Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                            }
                        }
                    }

                    //father email id saving
                    string fatheremail = "";
                    for (int j = 0; j < mas.multipleemailid.Length; j++)
                    {
                        fatheremail = Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        if (j == 0)
                        {
                            fatheremail = Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        }
                    }

                    //mother mobile saving
                    string mothermobile = "";
                    for (int j = 0; j < mas.multiplemobilenomother.Length; j++)
                    {
                        mothermobile = Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                        if (j == 0)
                        {
                            if (mothermobile != null)
                            {
                                mothermobile = Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                            }
                        }
                    }

                    //mother email id saving
                    string motheremail = "";
                    for (int j = 0; j < mas.multipleemailidmother.Length; j++)
                    {
                        motheremail = Convert.ToString(mas.multipleemailidmother[0].AMST_MotherEmailId);
                        if (j == 0)
                        {
                            motheremail = Convert.ToString(mas.multipleemailidmother[0].AMST_MotherEmailId);
                        }
                    }

                    mas.AMST_MobileNo = Convert.ToInt64(stdmobilenosms);
                    mas.AMST_emailId = stdemailsms;

                    if (fathermobile != "")
                    {
                        mas.AMST_FatherMobleNo = Convert.ToInt64(fathermobile);
                    }
                    mas.AMST_FatheremailId = fatheremail;

                    if (mothermobile != "")
                    {
                        mas.AMST_MotherMobileNo = Convert.ToInt64(mothermobile);
                    }

                    mas.AMST_MotherEmailId = fatheremail;
                    result.AMST_UpdatedBy = mas.userid;
                    Mapper.Map(mas, result);
                    _AdmissionFormContext.Update(result);
                }

                else
                {
                    //student mobile saving
                    string stdmobileno = "";
                    string stdmobilenosms = "";
                    string stdemailsms = "";
                    var count = mas.Adm_M_Student_MobileNoDTO.Length;
                    for (int i = 0; i < count; i++)
                    {
                        stdmobilenosms = mas.Adm_M_Student_MobileNoDTO[0].AMST_MobileNo;
                        if (i == 0)
                        {
                            stdmobileno = mas.Adm_M_Student_MobileNoDTO[i].AMST_MobileNo;
                        }
                    }

                    //student email id saving
                    string stdemailids = "";
                    for (int j = 0; j < mas.Adm_M_Student_EmailIdDTO.Length; j++)
                    {
                        stdemailsms = mas.Adm_M_Student_EmailIdDTO[0].AMST_emailId;
                        if (j == 0)
                        {
                            stdemailids = mas.Adm_M_Student_EmailIdDTO[j].AMST_emailId;
                        }
                    }


                    //father mobile saving
                    string fathermobile = "";
                    for (int j = 0; j < mas.multiplemobileno.Length; j++)
                    {
                        fathermobile = Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                        if (j == 0)
                        {
                            fathermobile = Convert.ToString(mas.multiplemobileno[0].AMST_FatherMobleNo);
                        }
                    }

                    //father email id saving
                    string fatheremail = "";
                    for (int j = 0; j < mas.multipleemailid.Length; j++)
                    {
                        fatheremail = Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        if (j == 0)
                        {
                            fatheremail = Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        }
                        else
                        {
                            //fatheremail = fatheremail + ',' + Convert.ToString(mas.multipleemailid[0].AMST_FatheremailId);
                        }
                    }

                    //mother mobile saving
                    string mothermobile = "";
                    for (int j = 0; j < mas.multiplemobilenomother.Length; j++)
                    {
                        mothermobile = Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                        if (j == 0)
                        {
                            mothermobile = Convert.ToString(mas.multiplemobilenomother[0].AMST_MotherMobileNo);
                        }
                    }

                    //mother email id saving
                    string motheremail = "";
                    for (int j = 0; j < mas.multipleemailidmother.Length; j++)
                    {
                        motheremail = Convert.ToString(mas.multipleemailidmother[0].AMST_MotherEmailId);
                        if (j == 0)
                        {
                            motheremail = Convert.ToString(mas.multipleemailidmother[0].AMST_MotherEmailId);
                        }
                    }
                    mas.Edit_flag = false;
                    MM2.CreatedDate = DateTime.Now;
                    MM2.UpdatedDate = DateTime.Now;

                    MM2.AMST_UpdatedBy = mas.userid;
                    MM2.AMST_CreatedBy = mas.userid;

                    MM2.AMST_Date = Convert.ToDateTime(mas.AMST_Date);
                    MM2.AMST_DOB = Convert.ToDateTime(mas.AMST_DOB);
                    MM2.IC_Id = mas.IMC_Id;
                    MM2.AMST_SubCasteIMC_Id = mas.AMST_SubCasteIMC_Id;
                    MM2.AMST_MotherReligion = mas.AMST_MotherReligion;
                    MM2.AMST_MotherCaste = mas.AMST_MotherCaste;
                    MM2.AMST_MotherSubCaste = mas.AMST_MotherSubCaste;
                    MM2.AMST_FatherReligion = mas.AMST_FatherReligion;
                    MM2.AMST_FatherCaste = mas.AMST_FatherCaste;
                    MM2.AMST_FatherSubCaste = mas.AMST_FatherSubCaste;

                    MM2.AMST_MobileNo = Convert.ToInt64(stdmobilenosms);
                    MM2.AMST_emailId = stdemailids;

                    if (fathermobile != "")
                    {
                        MM2.AMST_FatherMobleNo = Convert.ToInt64(fathermobile);
                    }
                    MM2.AMST_FatheremailId = fatheremail;

                    if (mothermobile != "")
                    {
                        MM2.AMST_MotherMobileNo = Convert.ToInt64(mothermobile);
                    }


                    MM2.AMST_MotherEmailId = motheremail;
                    MM2.AMST_PaymentFlag = 0;
                    MM2.AMST_AmountPaid = 0;
                    MM2.AMST_ActiveFlag = 1;
                    MM2.AMST_FinalpaymentFlag = 0;
                    MM2.AMST_SOL = "S";
                    MM2.AMST_Photoname = mas.AMST_Photoname;

                    if (mas.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    {
                        //Manual regNo.
                    }
                    else
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);

                        mas.transnumconfigsettings.MI_Id = MM2.MI_Id;
                        mas.transnumconfigsettings.ASMAY_Id = MM2.ASMAY_Id;
                        //MM2.AMC_Id = mas.AMC_Id;
                        MM2.AMST_RegistrationNo = a.GenerateNumber(mas.transnumconfigsettings);
                    }
                    if (mas.admissionNumbering.IMN_AutoManualFlag == "Manual")
                    {
                        //Manual Adm No.
                    }
                    else
                    {
                        GenerateTransactionNumbering b = new GenerateTransactionNumbering(_db);
                        mas.admissionNumbering.MI_Id = MM2.MI_Id;
                        mas.admissionNumbering.ASMAY_Id = MM2.ASMAY_Id;
                        MM2.AMST_AdmNo = b.GenerateNumber(mas.admissionNumbering);
                    }

                    // mas.transnumconfigsettings = new Master_NumberingDTO();

                    //--------------TPIN GENERATION--------------//

                    string date = MM2.AMST_DOB.ToString("yyyyMMdd");
                    long tpin = 0;
                    int i1 = 0;
                    var checktpinexists = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == mas.MI_Id && a.AMST_Tpin.Equals(date)).ToList();
                    while (checktpinexists.Count > 0)
                    {
                        if (i1 == 0)
                        {
                            tpin = Convert.ToInt64(date) + 1;
                            i1 = i1 + 1;
                        }
                        else
                        {
                            tpin = Convert.ToInt64(tpin) + 1;
                        }

                        string date1 = Convert.ToString(tpin);
                        checktpinexists = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == mas.MI_Id && a.AMST_Tpin.Equals(date1.ToString())).ToList();
                    }
                    if (tpin == 0)
                    {
                        MM2.AMST_Tpin = Convert.ToString(date);
                    }
                    else
                    {
                        MM2.AMST_Tpin = Convert.ToString(tpin);
                    }
                    _AdmissionFormContext.Add(MM2);
                    mas.AMST_Id = MM2.AMST_Id;
                }

                try
                {
                    ActivityAddUppdate(mas);
                    RefferenceDetailsAddUpdate(mas);
                    SourceDetailsAddUpdate(mas);
                    BondAddUpdate(mas);
                    SiblingsAddUpdate(mas);
                    AchivementAddUpdate(mas);
                    PrevSchooldetailsAddUpdate(mas);
                    GuardianAddUpdate(mas);
                    stud_doc_upload(mas);
                    father_mobile_no(mas);
                    father_email_ids(mas);
                    mother_mobile_no(mas);
                    mother_email_id(mas);
                    student_mobile_no(mas);
                    student_email_id(mas);

                    var flag = _AdmissionFormContext.SaveChanges();
                    if (flag > 0)
                    {
                        mas.count = flag;
                        mas.returnval = true;
                        if (mas.Edit_flag == true)
                        {
                            mas.message = "updated";
                        }
                        else
                        {
                            mas.message = "saved";
                        }
                    }
                    else
                    {
                        mas.count = -1;
                        mas.returnval = false;
                    }
                }
                catch (Exception e)
                {
                    _log.LogInformation("Student Admission form error");
                    _log.LogDebug(e.Message);
                }


                //  mas.returnval = true;
                if (mas.returnval == true)
                {
                    var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == mas.MI_Id);
                    var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == mas.MI_Id && t.AMST_Id == MM2.AMST_Id).ToList();

                    if (admConfig.ASC_DefaultSMS_Flag == "M")
                    {
                        SMS sms = new SMS(_db);
                        if (mas.Edit_flag == false)
                        {
                            string s = sms.sendSms(mas.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "ADMISSION_REGISTRATION", MM2.AMST_Id).Result;
                        }
                        else
                        {
                            string s = sms.sendSms(mas.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "ADMISSION_REGISTRATION_UPDATE", mas.AMST_Id).Result;
                        }

                        Email Email = new Email(_db);
                        if (mas.Edit_flag == false)
                        {
                            string m = Email.sendmail(mas.MI_Id, studDet.FirstOrDefault().AMST_MotherEmailId, "ADMISSION_REGISTRATION", MM2.AMST_Id);
                        }
                        else
                        {
                            string m = Email.sendmail(mas.MI_Id, studDet.FirstOrDefault().AMST_MotherEmailId, "ADMISSION_REGISTRATION_UPDATE", mas.AMST_Id);
                        }

                    }
                    else if (admConfig.ASC_DefaultSMS_Flag == "F")
                    {
                        SMS sms = new SMS(_db);
                        if (mas.Edit_flag == false)
                        {
                            string s = sms.sendSms(mas.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "ADMISSION_REGISTRATION", MM2.AMST_Id).Result;
                        }
                        else
                        {
                            string s = sms.sendSms(mas.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "ADMISSION_REGISTRATION_UPDATE", mas.AMST_Id).Result;
                        }

                        Email Email = new Email(_db);
                        if (mas.Edit_flag == false)
                        {
                            string m = Email.sendmail(mas.MI_Id, studDet.FirstOrDefault().AMST_FatheremailId, "ADMISSION_REGISTRATION", MM2.AMST_Id);
                        }
                        else
                        {
                            string m = Email.sendmail(mas.MI_Id, studDet.FirstOrDefault().AMST_FatheremailId, "ADMISSION_REGISTRATION_UPDATE", mas.AMST_Id);
                        }
                    }
                    else
                    {
                        SMS sms = new SMS(_db);
                        if (mas.Edit_flag == false)
                        {
                            string s = sms.sendSms(mas.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, "ADMISSION_REGISTRATION", MM2.AMST_Id).Result;
                        }
                        else
                        {
                            string s = sms.sendSms(mas.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, "ADMISSION_REGISTRATION_UPDATE", mas.AMST_Id).Result;
                        }

                        Email Email = new Email(_db);
                        if (mas.Edit_flag == false)
                        {
                            string m = Email.sendmail(mas.MI_Id, studDet.FirstOrDefault().AMST_emailId, "ADMISSION_REGISTRATION", MM2.AMST_Id);
                        }
                        else
                        {
                            string m = Email.sendmail(mas.MI_Id, studDet.FirstOrDefault().AMST_emailId, "ADMISSION_REGISTRATION_UPDATE", mas.AMST_Id);
                        }

                    }
                    if (mas.Edit_flag == false)
                    {
                        long stduserid = 0;
                        long fatuserid = 0;
                        long motuserid = 0;
                        string res = "";
                        Dictionary<string, long> temp = new Dictionary<string, long>();
                        generateOTP otp = new generateOTP();
                        if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
                        {

                            string studotp = otp.getFourDigitOTP();
                            string StudentName = "";
                            if (studDet.FirstOrDefault().AMST_FirstName.Length > 4)
                            {
                                StudentName = studDet.FirstOrDefault().AMST_FirstName.Substring(0, 3) + studotp;
                            }
                            else
                            {
                                StudentName = studDet.FirstOrDefault().AMST_FirstName + studotp;
                            }
                            StudentName = Regex.Replace(StudentName, @"\s+", "");
                            ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, mas.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                            stduserid = response.useridapp;
                            res = response.resp;
                            if (stduserid == 0)
                            {
                                StudentName = StudentName + 1;
                                ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, mas.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                stduserid = response1.useridapp;
                                res = response1.resp;
                                temp.Add("studentid", stduserid);
                            }
                            else
                            {
                                temp.Add("studentid", stduserid);

                            }
                            bool val = AddStudentUserLogin(mas.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
                            if (res == "Success" && val == true)
                            {
                                if (studDet.FirstOrDefault().AMST_MobileNo.ToString() != "" && studDet.FirstOrDefault().AMST_MobileNo.ToString() != null)
                                {
                                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                                    string s = sms.sendSms(mas.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, StudentName, "Password@123").Result;
                                }

                                EmailWithoutTemplate Email = new EmailWithoutTemplate(_db);
                                string m = Email.EmailWthtTmp(mas.MI_Id, StudentName, studDet.FirstOrDefault().AMST_emailId, "Password@123");
                            }
                            studotp = "";
                        }
                        else
                        {
                            temp.Add("studentid", 0);
                        }
                        if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
                        {
                            string fathrotp = otp.getFourDigitOTPFather();
                            string fathName = "";
                            if (studDet.FirstOrDefault().AMST_FatherName.Length > 4)
                            {
                                fathName = studDet.FirstOrDefault().AMST_FatherName.Substring(0, 3) + fathrotp;
                            }
                            else
                            {
                                fathName = studDet.FirstOrDefault().AMST_FatherName + fathrotp;

                            }
                            // string FatherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + fathrotp;
                            fathName = Regex.Replace(fathName, @"\s+", "");
                            if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
                            {
                                mas.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
                            }
                            else
                            {
                                mas.AMST_FatherMobleNo = 0;
                            }
                            ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, mas.MI_Id, "PARENTS", "PARENTS", mas.AMST_FatherMobleNo.ToString()).Result;
                            fatuserid = response.useridapp;
                            res = response.resp;
                            if (fatuserid == 0)
                            {
                                fathName = fathName + 1;
                                ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, mas.MI_Id, "PARENTS", "PARENTS", mas.AMST_FatherMobleNo.ToString()).Result;
                                fatuserid = response1.useridapp;
                                res = response1.resp;
                                temp.Add("Fatherid", fatuserid);
                            }
                            else
                            {
                                temp.Add("Fatherid", fatuserid);
                            }
                            bool val = AddStudentUserLogin(mas.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
                            if (res == "Success" && val == true)
                            {
                                if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "" && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null)
                                {
                                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                                    string s = sms.sendSms(mas.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), fathName, "Password@123").Result;
                                }
                                EmailWithoutTemplate Email = new EmailWithoutTemplate(_db);
                                string m = Email.EmailWthtTmp(mas.MI_Id, fathName, studDet.FirstOrDefault().AMST_FatheremailId, "Password@123");
                            }
                            fathrotp = "";
                        }
                        else
                        {
                            temp.Add("Fatherid", 0);
                        }
                        if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
                        {
                            string motherotp = otp.getFourDigitOTPMother();
                            string MotherName = "";
                            if (studDet.FirstOrDefault().AMST_FatherName.Length > 4)
                            {
                                MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0, 3) + motherotp;
                            }
                            else
                            {
                                MotherName = studDet.FirstOrDefault().AMST_FatherName + motherotp;

                            }
                            // string MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + motherotp;
                            MotherName = Regex.Replace(MotherName, @"\s+", "");
                            if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
                            {
                                mas.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
                            }
                            else
                            {
                                mas.AMST_MotherMobileNo = 0;
                            }
                            ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, mas.MI_Id, "PARENTS", "PARENTS", mas.AMST_MotherMobileNo.ToString()).Result;
                            motuserid = response.useridapp;
                            res = response.resp;
                            if (motuserid == 0)
                            {
                                MotherName = MotherName + 1;
                                ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, mas.MI_Id, "PARENTS", "PARENTS", mas.AMST_MotherMobileNo.ToString()).Result;
                                motuserid = response1.useridapp;
                                res = response1.resp;
                                temp.Add("motherid", motuserid);
                            }
                            else
                            {
                                temp.Add("motherid", motuserid);
                            }
                            bool val = AddStudentUserLogin(mas.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
                            if (res == "Success" && val == true)
                            {
                                if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "" && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null)
                                {
                                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                                    string s = sms.sendSms(mas.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), MotherName, "Password@123").Result;
                                }
                                EmailWithoutTemplate Email = new EmailWithoutTemplate(_db);
                                string m = Email.EmailWthtTmp(mas.MI_Id, MotherName, studDet.FirstOrDefault().AMST_MotherEmailId, "Password@123");
                            }
                            motherotp = "";
                        }
                        else
                        {
                            temp.Add("motherid", 0);
                        }
                        if (temp.Count != 0)
                        {
                            long uid = 0;
                            long fid = 0;
                            long mid = 0;
                            if (temp["studentid"] != 0)
                            {
                                uid = temp["studentid"];
                            }
                            if (temp["Fatherid"] != 0)
                            {
                                fid = temp["Fatherid"];
                            }
                            if (temp["motherid"] != 0)
                            {
                                mid = temp["motherid"];
                            }

                            bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Student Admission form error");
                _log.LogDebug(ex.Message);
                //  mas.returnval = false;
            }

            return mas;
        }

        //stream exams

        public async Task<Adm_M_StudentDTO> GetSubjectsofinstitute(Adm_M_StudentDTO stu)
        {

            var Acdemic_preadmission = _AdmissionFormContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


            stu.ASMAY_Id = Acdemic_preadmission;
            //
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Exam_Preadmission_Group_SubjectList";
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
                cmd.Parameters.Add(new SqlParameter("@ASMST_Id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMST_Id)
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
                cmd1.CommandText = "Exam_Preadmission_GroupList";
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
                cmd1.Parameters.Add(new SqlParameter("@ASMST_Id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(stu.ASMST_Id)
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

            stu.streamexams = (from a in _db.Adm_School_Master_CE
                               from b in _db.Adm_School_Stream_Class_CE
                               where (a.ASMCE_Id == b.ASMCE_Id && a.MI_Id == b.MI_Id && b.ASMCL_Id == stu.ASMCL_Id && b.ASMST_Id == stu.ASMST_Id)
                               select new StudentApplicationDTO
                               {
                                   ASMCE_CEName = a.ASMCE_CEName,
                                   ASMCE_Id = a.ASMCE_Id,
                                   ASSTCLCE_CompulsoryFlg = b.ASSTCLCE_CompulsoryFlg
                               }
          ).Distinct().ToArray();

            return stu;
        }


        public void Studentexam(Adm_M_StudentDTO stu)
        {
            try
            {
                //var examcounts = _db.PA_School_Application_CE.Where(t => t.PASR_Id == stu.pasR_Id).ToList();
                //if (examcounts.Count>0)
                //{
                var duplicatecheck = _db.Adm_Master_Student_CE.Where(d => d.AMST_Id == stu.AMST_Id).ToList();

                if (duplicatecheck.Count() == 0)
                {
                    Adm_Master_Student_CE siblingsave = new Adm_Master_Student_CE();
                    siblingsave.AMST_Id = stu.AMST_Id;
                    siblingsave.ASMCE_Id = Convert.ToInt64(stu.ASMCE_Id);
                    siblingsave.AMSTCE_CreatedBy = stu.userid;
                    siblingsave.AMSTCE_UpdatedBy = stu.userid;
                    siblingsave.AMSTCE_ActiveFlg = true;
                    siblingsave.CreatedDate = DateTime.Now;
                    siblingsave.UpdatedDate = DateTime.Now;
                    _db.Add(siblingsave);
                    _db.SaveChanges();
                }
                else
                {
                    var updateasmce = _db.Adm_Master_Student_CE.Single(t => t.AMST_Id == stu.AMST_Id);
                    updateasmce.ASMCE_Id = Convert.ToInt64(stu.ASMCE_Id);
                    updateasmce.AMSTCE_UpdatedBy = stu.userid;
                    updateasmce.UpdatedDate = DateTime.Now;
                    _db.Update(updateasmce);
                    _db.SaveChanges();
                }
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}