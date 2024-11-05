using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
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
using paytm.security;
using paytm.util;
using Org.BouncyCastle.Crypto;
using DomainModel.Model.com.vapstech.Fee;
using Razorpay.Api;
using Payment = CommonLibrary.Payment;
using System.Net;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.admission;
using easebuzz_.net;

namespace WebApplication1.Services
{
    public class StudentApplicationImpl : Interfaces.StudentApplicationInterface
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
        public StudentApplicationImpl(StudentApplicationContext StudentApplicationContext, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext db, FeeGroupContext feecontext, ProspectusContext ProspectusContext)
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
                List<Adm_School_Master_Stream> SyllabusList = new List<Adm_School_Master_Stream>();
                SyllabusList = await _StudentApplicationContext.Master_Streams.Where(t => t.MI_Id == stu.MI_Id && t.ASMST_ActiveFlag == true).ToListAsync();
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
                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == stu.roleId).IVRMRT_Role;
                if (!rolename.Equals("Student", StringComparison.OrdinalIgnoreCase) && !rolename.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    //var Acdemic_preadmission = _db.AcademicYear.Where(t => t.MI_Id.Equals(stu.MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    List<Preadmission_Special_Registration> Specialuser = new List<Preadmission_Special_Registration>();
                    Specialuser = _StudentApplicationContext.Preadmission_Special_Registration.Where(t => t.ID == stu.Id).ToList();


                    List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
                    admissionconfigurationsettings = _db.AdmissionStandardDMO.Where(t => t.MI_Id == stu.MI_Id).ToList();
                    stu.admissioncongigurationList = admissionconfigurationsettings.ToArray();

                    var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                    DateTime startdate = Convert.ToDateTime(_StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());

                    stu.ASMAY_Id = Acdemic_preadmission;
                    stu.ASMAY_PreAdm_F_Date = startdate;

                    List<AdmissionClass> allclass = new List<AdmissionClass>();
                    List<MasterAcademic> allyear = new List<MasterAcademic>();
                    List<MasterAcademic> allyearget = new List<MasterAcademic>();

                    if (rolename.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                    {
                        allyearget = (from a in _feecontext.AcademicYear
                                      where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                      select new MasterAcademic
                                      {
                                          ASMAY_Id = a.ASMAY_Id,
                                          ASMAY_Year = a.ASMAY_Year
                                      }
                         ).ToList();

                        //allyear = (from a in _db.AcademicYear
                        //           where (a.MI_Id == stu.MI_Id && TimeZoneInfo.ConvertTime(a.ASMAY_PreAdm_F_Date.Value, INDIAN_ZONE) <= indianTime && TimeZoneInfo.ConvertTime(a.ASMAY_PreAdm_T_Date.Value, INDIAN_ZONE) >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                        //           select new MasterAcademic
                        //           {
                        //               ASMAY_Id = a.ASMAY_Id,
                        //               ASMAY_Year = a.ASMAY_Year
                        //           }
                        //).ToList();

                        allyear = (from a in _db.AcademicYear
                                   where (a.MI_Id == stu.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                                   select new MasterAcademic
                                   {
                                       ASMAY_Id = a.ASMAY_Id,
                                       ASMAY_Year = a.ASMAY_Year,
                                       ASMAY_Order = a.ASMAY_Order
                                   }
                      ).OrderByDescending(d => d.ASMAY_Order).ToList();


                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
                        stu.admissioncatdrp = allclass.ToArray();

                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                        stu.admissioncatdrpall = allclass.ToArray();

                        if (Specialuser.Count > 0)
                        {
                            allyear = (from a in _feecontext.AcademicYear
                                       where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                       select new MasterAcademic
                                       {
                                           ASMAY_Id = a.ASMAY_Id,
                                           ASMAY_Year = a.ASMAY_Year
                                       }
                                      ).ToList();

                            allclass.Clear();

                            allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                            stu.admissioncatdrp = allclass.ToArray();

                            allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                            stu.admissioncatdrpall = allclass.ToArray();
                            stu.specialuser = true;
                        }
                        ApplicationUser user = new ApplicationUser();
                        user = await _UserManager.FindByNameAsync(stu.username);
                        if (user != null)
                        {
                            stu.useremail = user.Email;
                            stu.mobilenumber = user.PhoneNumber;
                            stu.stuusername = user.Name;
                            stu.useimagepath = user.UserImagePath;
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

                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                        stu.admissioncatdrp = allclass.ToArray();

                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                        stu.admissioncatdrpall = allclass.ToArray();

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

                        List<MasterLocationDMO> location = new List<MasterLocationDMO>();
                        location = _StudentApplicationContext.MasterLocationDMO.Where(r => r.MI_Id == stu.MI_Id && r.TRML_ActiveFlg == true).OrderBy(t => t.TRML_LocationName).ToList();
                        stu.locationdrp = location.ToArray();

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
                        allreligion = await _StudentApplicationContext.religion.Where(t => t.Is_Active == true).ToListAsync();
                        stu.religiondrp = allreligion.ToArray();

                        List<CasteCategory> allcc = new List<CasteCategory>();
                        allcc = await _StudentApplicationContext.castecategory.ToListAsync();
                        stu.castecategorydrp = allcc.ToArray();
                        //SECTION dropdownlist added on 14/12/2016

                        List<School_M_Section> allSection = new List<School_M_Section>();

                        allSection = await _StudentApplicationContext.Section.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                        stu.sectiondropdown = allSection.ToArray();

                        //List<MasterDocumentDMO> MasterDocumentDMO = new List<MasterDocumentDMO>();
                        //MasterDocumentDMO = await _StudentApplicationContext.MasterDocumentDMO.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                        //stu.DocumentList = MasterDocumentDMO.ToArray();

                        stu.syllabuslist = (from a in _db.Master_stream
                                            from b in _db.Master_stream_class
                                            where (a.ASMST_Id == b.ASMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == stu.MI_Id && b.ASSTCL_ActiveFlag == true && a.ASMST_ActiveFlag == true)
                                            select new StudentApplicationDTO
                                            {
                                                ASMST_StreamName = a.ASMST_StreamName,
                                                ASMST_Id = a.ASMST_Id
                                            }
                                           ).Distinct().ToArray();

                        if (stu.syllabuslist.Length == 0)
                        {
                            //// Syllabus list
                            List<Adm_School_Master_Stream> SyllabusList = new List<Adm_School_Master_Stream>();
                            SyllabusList = await _StudentApplicationContext.Master_Streams.Where(t => t.MI_Id == stu.MI_Id && t.ASMST_StreamCode == "OTH").ToListAsync();
                            stu.syllabuslistoth = SyllabusList.ToArray();
                        }

                        List<MasterReference> StudentReferenceDMO = new List<MasterReference>();
                        StudentReferenceDMO = _StudentApplicationContext.Preadmission_Master_Reference.ToList();
                        stu.StudentReferenceDetails = StudentReferenceDMO.ToArray();

                        List<MasterSource> StudentSourceDMO = new List<MasterSource>();
                        StudentSourceDMO = _StudentApplicationContext.Preadmission_Master_Source.ToList();
                        stu.StudentSourceDetails = StudentSourceDMO.ToArray();


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

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = await _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToListAsync();
                    stu.mstConfig = mstConfig.ToArray();

                    List<PA_School_Application_ProspectusDMO> allRegStudentnow = new List<PA_School_Application_ProspectusDMO>();
                    allRegStudentnow = _db.PA_School_Application_ProspectusDMO.ToList();
                    stu.studentDetailsTEmp = allRegStudentnow.ToArray();

                    List<long> temparr = new List<long>();
                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudentnow[i].PASP_Id);
                    }

                    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                    var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleId).ToList();
                    stu.roleName = rolelist[0].IVRMRT_Role;
                    //if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                    //  || rolelist[0].IVRMRT_Role.Equals("PRINCIPAL", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                    if (!rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                    {
                        allRegStudent = await _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.PASR_Adm_Confirm_Flag == false && d.ASMAY_Id == stu.ASMAY_Id).ToListAsync();
                        stu.registrationList = allRegStudent.ToArray();

                        List<StudentApplicationDTO> userdetails = new List<StudentApplicationDTO>();
                        userdetails = (from a in _StudentApplicationContext.ApplicationUser
                                       from d in _StudentApplicationContext.Enq
                                       where (a.Id == d.Id && d.MI_Id.Equals(stu.MI_Id) && d.PASR_Adm_Confirm_Flag == false && d.ASMAY_Id == stu.ASMAY_Id)
                                       select new StudentApplicationDTO
                                       {
                                           pasR_Id = d.pasr_id,
                                           stusername = a.UserName,
                                           Id = a.Id
                                       }
                       ).ToList();

                        stu.userdetails = userdetails.ToArray();
                        stu.registrationListhealth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                      from b in _StudentApplicationContext.Enq
                                                      where (a.PASR_Id == b.pasr_id && b.MI_Id == stu.MI_Id && b.PASR_Adm_Confirm_Flag == false)
                                                      select b
                          ).ToList().ToArray();

                        if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 0)
                        {
                            var userapplications = _db.StudentApplication.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id).ToList();

                            if (userapplications.Count > 0)
                            {
                                List<string> usrtemparr = new List<string>();
                                for (int i = 0; i < userapplications.Count; i++)
                                {
                                    usrtemparr.Add(userapplications[i].PASR_RegistrationNo);
                                }

                                stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && !usrtemparr.Contains(t.PASP_ProspectusNo)).ToList().ToArray();
                            }
                            else
                            {
                                stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id).ToList().ToArray();
                            }
                        }
                        else
                        {
                            var userapplications = _db.StudentApplication.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id).ToList();

                            if (userapplications.Count > 0)
                            {
                                List<string> usrtemparr = new List<string>();
                                for (int i = 0; i < userapplications.Count; i++)
                                {
                                    usrtemparr.Add(userapplications[i].PASR_RegistrationNo);
                                }

                                stu.prospectuslist = (from a in _db.prospectus
                                                      from b in _db.Payment_PA_ProspectusDMO
                                                      where (a.PASP_Id == b.PASP_Id && a.MI_ID == stu.MI_Id && a.ASMAY_Id == stu.ASMAY_Id && !usrtemparr.Contains(a.PASP_ProspectusNo) && !temparr.Contains(a.PASP_Id) && a.ASMAY_Id == stu.ASMAY_Id)
                                                      select a
                                    ).ToList().ToArray();

                                //stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.id == stu.Id && !usrtemparr.Contains(t.PASP_ProspectusNo) && !temparr.Contains(t.PASP_Id)).ToList().ToArray();
                            }
                            else
                            {
                                stu.prospectuslist = (from a in _db.prospectus
                                                      from b in _db.Payment_PA_ProspectusDMO
                                                      where (a.PASP_Id == b.PASP_Id && a.MI_ID == stu.MI_Id && a.ASMAY_Id == stu.ASMAY_Id && !temparr.Contains(a.PASP_Id) && a.ASMAY_Id == stu.ASMAY_Id)
                                                      select a
                                   ).ToList().ToArray();

                                //stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.id == stu.Id && !temparr.Contains(t.PASP_Id)).ToList().ToArray();
                            }
                        }
                    }
                    else
                    {
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

                        if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 0)
                        {
                            var userapplications = _db.StudentApplication.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.Id == stu.Id).ToList();

                            if (userapplications.Count > 0)
                            {
                                List<string> usrtemparr = new List<string>();
                                for (int i = 0; i < userapplications.Count; i++)
                                {
                                    usrtemparr.Add(userapplications[i].PASR_RegistrationNo);
                                }

                                stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.id == stu.Id && !usrtemparr.Contains(t.PASP_ProspectusNo)).ToList().ToArray();
                            }
                            else
                            {
                                stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.id == stu.Id).ToList().ToArray();
                            }
                        }
                        else
                        {
                            var userapplications = _db.StudentApplication.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.Id == stu.Id).ToList();

                            if (userapplications.Count > 0)
                            {
                                List<string> usrtemparr = new List<string>();
                                for (int i = 0; i < userapplications.Count; i++)
                                {
                                    usrtemparr.Add(userapplications[i].PASR_RegistrationNo);
                                }

                                stu.prospectuslist = (from a in _db.prospectus
                                                      from b in _db.Payment_PA_ProspectusDMO
                                                      where (a.PASP_Id == b.PASP_Id && a.MI_ID == stu.MI_Id && a.ASMAY_Id == stu.ASMAY_Id && a.id == stu.Id && !usrtemparr.Contains(a.PASP_ProspectusNo) && !temparr.Contains(a.PASP_Id) && a.ASMAY_Id == stu.ASMAY_Id)
                                                      select a
                                    ).ToList().ToArray();

                                //stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.id == stu.Id && !usrtemparr.Contains(t.PASP_ProspectusNo) && !temparr.Contains(t.PASP_Id)).ToList().ToArray();
                            }
                            else
                            {
                                stu.prospectuslist = (from a in _db.prospectus
                                                      from b in _db.Payment_PA_ProspectusDMO
                                                      where (a.PASP_Id == b.PASP_Id && a.MI_ID == stu.MI_Id && a.ASMAY_Id == stu.ASMAY_Id && a.id == stu.Id && !temparr.Contains(a.PASP_Id) && a.ASMAY_Id == stu.ASMAY_Id)
                                                      select a
                                   ).ToList().ToArray();

                                //stu.prospectuslist = _db.prospectus.Where(t => t.MI_ID == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.id == stu.Id && !temparr.Contains(t.PASP_Id)).ToList().ToArray();
                            }
                        }

                    }

                    var checkapplicationstypes = (from m in _db.Masterclasscategory
                                                  from c in _db.mastercategory
                                                  from o in _db.AcademicYear
                                                  from p in _db.School_M_Class
                                                  where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && o.ASMAY_Id == stu.ASMAY_Id && p.MI_Id == stu.MI_Id && p.ASMCL_PreadmFlag == 1 && c.AMC_Type == "School")
                                                  select new StudentApplicationDTO
                                                  {
                                                      ASMCL_Id = p.ASMCL_Id
                                                  }).Distinct().ToList();

                    if (checkapplicationstypes.Count > 0)
                    {
                        stu.multipleapplications = true;
                    }

                    var checkapplicationstypec = (from m in _db.Masterclasscategory
                                                  from c in _db.mastercategory
                                                  from o in _db.AcademicYear
                                                  from p in _db.School_M_Class
                                                  where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && o.ASMAY_Id == stu.ASMAY_Id && p.MI_Id == stu.MI_Id && p.ASMCL_PreadmFlag == 1 && c.AMC_Type == "College")
                                                  select new StudentApplicationDTO
                                                  {
                                                      ASMCL_Id = p.ASMCL_Id
                                                  }).Distinct().ToList();

                    if (checkapplicationstypec.Count > 0)
                    {
                        stu.multipleapplicationsc = true;
                    }

                    List<long> studentslist = new List<long>();
                    for (int i = 0; i < allRegStudent.Count; i++)
                    {
                        studentslist.Add(allRegStudent[i].pasr_id);
                    }

                    stu.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && studentslist.Contains(t.PASA_Id)).ToArray();

                    // 30-9-2016

                    List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                    saa = _db.MasterAreaDMO.Where(r => r.MI_Id == stu.MI_Id && r.TRMA_ActiveFlg == true).ToList();
                    stu.areaList = saa.ToArray();

                    List<MasterSource> allsource = new List<MasterSource>();
                    allsource = _ProspectusContext.source.ToList();
                    stu.sourcedropDown = allsource.ToArray();

                    ////srkvs deepak
                    //if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 1)
                    //{
                    //    stu.prospectuslist = (from a in _db.prospectus
                    //                          from b in _db.Payment_PA_ProspectusDMO
                    //                          where (a.PASP_Id == b.PASP_Id && !temparr.Contains(a.PASP_Id) && a.ASMAY_Id== stu.ASMAY_Id)
                    //                          select a
                    //        ).ToList().ToArray();
                    //}
                    //end srkvs deepak

                    stu.caste_doc_maplist = _StudentApplicationContext.Preadmission_Cast_Doc_MappingDMO.Where(t => t.MI_Id == stu.MI_Id).ToList().Distinct().ToArray();

                    List<AdmissionStatus> status = new List<AdmissionStatus>();
                    status = _db.status.Where(t => t.MI_Id == stu.MI_Id).ToList();
                    stu.statuslist = status.ToArray();

                    List<PA_Master_Vaccines> Vaccines = new List<PA_Master_Vaccines>();
                    Vaccines = _db.PA_Master_Vaccines.Where(t => t.MI_Id == stu.MI_Id && t.PAMVA_ActiveFlg == true).ToList();
                    stu.vaccines = Vaccines.ToArray();

                    await Getcountofstudents(stu);

                    stu.fillpaymentgateway = (from a in _db.PAYUDETAILS
                                              from b in _db.Fee_PaymentGateway_Details
                                              where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == stu.MI_Id && b.FPGD_PGActiveFlag == "1")
                                              select new CountryDTO
                                              {
                                                  FPGD_Id = a.IMPG_Id,
                                                  FPGD_PGName = a.IMPG_PGFlag,
                                                  FPGD_Image = b.FPGD_Image
                                              }
                                             ).Distinct().ToArray();
                }
                else
                {
                    stu.precutdate = "Not";
                };
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
        public async Task<StudentApplicationDTO> getstreams(StudentApplicationDTO stu)
        {
            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            stu.syllabuslist = (from a in _db.Master_stream
                                from b in _db.Master_stream_class
                                where (a.ASMST_Id == b.ASMST_Id && a.MI_Id == b.MI_Id && b.ASMCL_Id == stu.ASMCL_Id && b.ASSTCL_ActiveFlag == true && a.ASMST_ActiveFlag == true)
                                select new StudentApplicationDTO
                                {
                                    ASMST_StreamName = a.ASMST_StreamName,
                                    ASMST_Id = a.ASMST_Id,
                                    ASMST_Order = a.ASMST_Order
                                }
           ).Distinct().OrderBy(t => t.ASMST_Order).ToArray();

            if (stu.syllabuslist.Length == 0)
            {
                //// Syllabus list
                List<Adm_School_Master_Stream> SyllabusList = new List<Adm_School_Master_Stream>();
                SyllabusList = await _StudentApplicationContext.Master_Streams.Where(t => t.MI_Id == stu.MI_Id && t.ASMST_StreamCode == "OTH").ToListAsync();
                stu.syllabuslistoth = SyllabusList.ToArray();
            }

            var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleid).ToList();

            if (rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
            {
                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(Acdemic_preadmission)).ToList();

                int countstu = _StudentApplicationContext.Enq.Where(t => t.ASMAY_Id == Acdemic_preadmission && t.ASMCL_Id == stu.ASMCL_Id && t.Id == stu.Id).Count();

                if (countstu >= mstConfig.FirstOrDefault().ISPAC_NoofApplications)
                {
                    stu.message = "You have Already Submitted " + countstu.ToString() + " Form for this class!!";
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

                List<Preadmission_Special_Registration> alllocation = new List<Preadmission_Special_Registration>();
                alllocation = _StudentApplicationContext.Preadmission_Special_Registration.Where(t => t.ID == stu.Id).ToList();

                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                stu.ASMAY_Id = Acdemic_preadmission;

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                List<MasterAcademic> allyearget = new List<MasterAcademic>();
                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == stu.roleId).IVRMRT_Role;

                if (rolename.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                {
                    allyearget = (from a in _feecontext.AcademicYear
                                  where (a.MI_Id == stu.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == stu.MI_Id)
                                  select new MasterAcademic
                                  {
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMAY_Year = a.ASMAY_Year
                                  }
                     ).ToList();

                    allyear = (from a in _db.AcademicYear
                               where (a.MI_Id == stu.MI_Id && a.ASMAY_PreAdm_F_Date <= indianTime && a.ASMAY_PreAdm_T_Date >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                               select new MasterAcademic
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMAY_Year = a.ASMAY_Year,
                                   ASMAY_Order = a.ASMAY_Order
                               }
                                ).OrderByDescending(d => d.ASMAY_Order).ToList();

                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
                    stu.admissioncatdrp = allclass.ToArray();

                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                    stu.admissioncatdrpall = allclass.ToArray();
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

                        allclass.Clear();
                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                        stu.admissioncatdrp = allclass.ToArray();

                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                        stu.admissioncatdrpall = allclass.ToArray();
                        stu.specialuser = true;
                    }
                    stu.countrole = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PreStudent_Meeting_Profile";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = stu.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@userid",
                           SqlDbType.BigInt)
                        {
                            Value = stu.Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@date",
                            SqlDbType.DateTime)
                        {
                            Value = indianTime.Date
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            stu.meetinglist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
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

                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                    stu.admissioncatdrp = allclass.ToArray();

                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id).ToList();
                    stu.admissioncatdrpall = allclass.ToArray();
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
                saa = _db.MasterAreaDMO.Where(r => r.MI_Id == stu.MI_Id && r.TRMA_ActiveFlg == true).ToList();
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
                                          where (a.PASP_Id == b.PASP_Id && !temparr.Contains(a.PASP_Id) && a.ASMAY_Id == stu.ASMAY_Id)
                                          select a
                        ).ToList().ToArray();
                }
                //end srkvs deepak
                stu.caste_doc_maplist = _StudentApplicationContext.Preadmission_Cast_Doc_MappingDMO.Where(t => t.MI_Id == stu.MI_Id).ToList().Distinct().ToArray();
                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == stu.MI_Id).ToList();
                stu.statuslist = status.ToArray();

                stu.fillpaymentgateway = (from a in _db.PAYUDETAILS
                                          from b in _db.Fee_PaymentGateway_Details
                                          where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == stu.MI_Id && b.FPGD_PGActiveFlag == "1")
                                          select new CountryDTO
                                          {
                                              FPGD_Id = a.IMPG_Id,
                                              FPGD_PGName = a.IMPG_PGFlag,
                                              FPGD_Image = b.FPGD_Image
                                          }
                                        ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            var checkapplicationstypes = (from m in _db.Masterclasscategory
                                          from c in _db.mastercategory
                                          from o in _db.AcademicYear
                                          from p in _db.School_M_Class
                                          where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && o.ASMAY_Id == stu.ASMAY_Id && p.MI_Id == stu.MI_Id && p.ASMCL_PreadmFlag == 1 && c.AMC_Type == "School")
                                          select new StudentApplicationDTO
                                          {
                                              ASMCL_Id = p.ASMCL_Id
                                          }).Distinct().ToList();

            if (checkapplicationstypes.Count > 0)
            {
                stu.multipleapplications = true;
            }

            var checkapplicationstypec = (from m in _db.Masterclasscategory
                                          from c in _db.mastercategory
                                          from o in _db.AcademicYear
                                          from p in _db.School_M_Class
                                          where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && o.ASMAY_Id == stu.ASMAY_Id && p.MI_Id == stu.MI_Id && p.ASMCL_PreadmFlag == 1 && c.AMC_Type == "College")
                                          select new StudentApplicationDTO
                                          {
                                              ASMCL_Id = p.ASMCL_Id
                                          }).Distinct().ToList();

            if (checkapplicationstypec.Count > 0)
            {
                stu.multipleapplicationsc = true;
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

                var qry7 = (from source in _StudentApplicationContext.PreadmissionStudnetSource.Where(trns => trns.PASR_Id == dt.pasR_Id) select source);

                var qry9 = (from source in _StudentApplicationContext.Preadmission_School_Registration_Employee.Where(trns => trns.PASR_Id == dt.pasR_Id) select source);

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == dt.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                dt.admissioncatdrp = allclass.ToArray();

                dt.StudentReg_DTObj = qry1.ToArray();
                dt.StudentGuardian_DTObj = qry2.ToArray();
                dt.StudentPrevSch_DTObj = qry3.ToArray();
                dt.StudentSbling_DTObj = qry4.ToArray();
                dt.StudentTrns_DTObj = qry5.ToArray();
                dt.Studentsource_DTObj = qry7.ToArray();
                dt.studentEmploye = qry9.ToArray();

                //List<MasterSource> allsource = new List<MasterSource>();
                //allsource = _ProspectusContext.source.ToList();
                //dt.sourcedropDown = allsource.ToArray();

                if (dt.studentEmploye != null && dt.studentEmploye.Length > 0)
                {
                    dt.fillstaff = (from a in _db.HR_Master_Employee_DMO
                                    where a.MI_Id == dt.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                    select new StaffLoginDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        stafname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName.ToUpper()) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName.ToUpper()) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName.ToUpper())).Trim(),
                                    }).ToArray();
                }

                if (dt.Studentsource_DTObj.Length > 0)
                {
                    dt.PAMSId = qry7.FirstOrDefault().PAMS_Id;
                }

                if (dt.StudentTrns_DTObj.Length > 0)
                {

                    List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                    saa = _db.MasterAreaDMO.Where(r => r.MI_Id == dt.MI_Id && r.TRMA_ActiveFlg == true).ToList();
                    dt.areaList = saa.ToArray();

                    List<MasterRouteDMO> route = new List<MasterRouteDMO>();
                    route = _StudentApplicationContext.MasterRouteDMO.Where(r => r.MI_Id == dt.MI_Id && r.TRMR_ActiveFlg == true).ToList();
                    dt.routelist = route.ToArray();

                    //List<MasterLocationDMO> locat = new List<MasterLocationDMO>();
                    //locat = _StudentApplicationContext.MasterLocationDMO.Where(r => r.MI_Id == dt.MI_Id && r.TRML_ActiveFlg == true).ToList();
                    //dt.locationlist = locat.ToArray();

                    dt.locationlist = (from a in _StudentApplicationContext.Route_Location
                                       from b in _StudentApplicationContext.MasterRouteDMO
                                       from c in _StudentApplicationContext.MasterLocationDMO
                                       from d in _StudentApplicationContext.MasterAreaDMO
                                       where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && d.TRMA_Id == b.TRMA_Id && d.TRMA_Id == qry5.FirstOrDefault().TRMA_Id && b.TRMR_ActiveFlg == true && c.TRML_ActiveFlg == true)
                                       select new StudentBuspassFormDTO
                                       {
                                           TRML_Id = c.TRML_Id,
                                           TRML_LocationName = c.TRML_LocationName
                                       }
                 ).Distinct().ToList().ToArray();


                    //     dt.routelist = (
                    //                      from b in _StudentApplicationContext.MasterRouteDMO
                    //                      where (b.TRMR_Id == qry5.FirstOrDefault().PASTA_PickUp_TRMR_Id)
                    //                      select new StudentBuspassFormDTO
                    //                      {
                    //                          TRMR_Id = b.TRMR_Id,
                    //                          TRMR_RouteName = b.TRMR_RouteName
                    //                      }
                    //).ToList().ToArray();

                    //    dt.routelist = (from a in _StudentApplicationContext.MasterAreaDMO
                    //                    from b in _StudentApplicationContext.MasterRouteDMO
                    //                    where (a.TRMA_Id == b.TRMA_Id && b.TRMR_ActiveFlg==true && a.TRMA_Id == qry5.FirstOrDefault().TRMA_Id)
                    //                    select new StudentBuspassFormDTO
                    //                    {
                    //                        TRMR_Id = b.TRMR_Id, 
                    //                        TRMR_RouteName = b.TRMR_RouteName
                    //                    }
                    //).OrderBy(t => t.TRMR_order).ToList().ToArray();

                    //    dt.locationlist = (
                    //                         from c in _StudentApplicationContext.MasterLocationDMO
                    //                         from d in _StudentApplicationContext.Route_Location
                    //                         where (c.TRML_Id == d.TRML_Id && c.TRML_ActiveFlg==true && d.TRMR_Id == qry5.FirstOrDefault().PASTA_PickUp_TRMR_Id)
                    //                         select new StudentBuspassFormDTO
                    //                         {
                    //                             TRML_Id = c.TRML_Id,
                    //                             TRML_LocationName = c.TRML_LocationName

                    //                         }
                    //).OrderBy(t => t.TRML_LocationName).ToList().ToArray();


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
                var concession = _feecontext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == dt.MI_Id).ToList();
                if (concession.Count() > 0)
                {
                    var concessiontype = _StudentApplicationContext.Fee_Master_ConcessionDMO.Where(t => t.FMCC_Id == qry1.FirstOrDefault().FMCC_ID).ToList();
                    if (concessiontype.Count() > 0)
                    {
                        if (concessiontype.FirstOrDefault().FMCC_ConcessionFlag == "S")
                        {
                            var Concessionconfirm = _StudentApplicationContext.StudentSibling.Where(t => (t.PASRS_Status != "R" || t.PASRS_Status != "C") && t.PASR_Id == dt.pasR_Id).ToArray();
                            if (Concessionconfirm.Count() > 0)
                            {
                                dt.concessionconfirm = true;
                                dt.concessionconfirmsibling = true;
                            }
                            else
                            {
                                dt.concessionconfirm = false;
                            }
                        }
                        else if (concessiontype.FirstOrDefault().FMCC_ConcessionFlag == "E")
                        {
                            var Concessionconfirm = _StudentApplicationContext.Preadmission_School_Registration_Employee.Where(t => t.PSRE_ActiveFlag != null && t.PASR_Id == dt.pasR_Id).ToArray();
                            if (Concessionconfirm.Count() > 0)
                            {
                                dt.concessionconfirm = true;
                            }
                            else
                            {
                                dt.concessionconfirm = false;
                            }
                        }
                        else if (concessiontype.FirstOrDefault().FMCC_ConcessionFlag == "R")
                        {
                            var Concessionconfirm = _StudentApplicationContext.PA_Student_Sibblings.Where(t => (t.PASS_ActiveFlag == true && t.PASS_ConcessionAmt == 100) && t.PASR_Id == dt.pasR_Id).ToArray();
                            if (Concessionconfirm.Count() > 0)
                            {
                                dt.concessionconfirm = true;
                            }
                            else
                            {
                                dt.concessionconfirm = false;
                            }
                        }
                    }

                }

                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == dt.roleid).IVRMRT_Role;

                if (rolename.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
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
                             where (Docs.PASR_Id == mDocs.pasr_id && Docs.ISMS_Id == subjects.ISMS_Id && Docs.ISMS_Id == groupmap.ISMS_Id && subjects.ISMS_Id == groupmap.ISMS_Id && groupmap.EMG_Id == groupid.EMG_Id && Docs.EMG_Id == groupid.EMG_Id && Docs.EMG_Id == groupmap.EMG_Id && Docs.PASR_Id == dt.pasR_Id)
                             select new Preadmissionelectives
                             {
                                 EMG_Id = groupmap.EMG_Id,
                                 ismS_Id = Convert.ToInt16(Docs.ISMS_Id),
                                 EMG_MinAplSubjects = groupid.EMG_MinAplSubjects,
                                 EMG_MaxAplSubjects = groupid.EMG_MaxAplSubjects,
                                 EMG_GroupName = groupid.EMG_GroupName
                             });
                dt.StudentSubjects_DTObj = qry10.ToArray();
                dt.ASMST_Id = qry1.FirstOrDefault().ASMST_Id;
                GetSubjects(dt);

                if (qry1.Count() > 0)
                {
                    dt.syllabuslist = (from a in _db.Master_stream
                                       from b in _db.Master_stream_class
                                       where (a.ASMST_Id == b.ASMST_Id && a.MI_Id == b.MI_Id && b.ASMCL_Id == qry1.FirstOrDefault().ASMCL_Id)
                                       select new StudentApplicationDTO
                                       {
                                           ASMST_StreamName = a.ASMST_StreamName,
                                           ASMST_Id = a.ASMST_Id
                                       }
          ).Distinct().ToArray();
                }

                if (dt.syllabuslist.Length == 0)
                {
                    //// Syllabus list
                    List<Adm_School_Master_Stream> SyllabusList = new List<Adm_School_Master_Stream>();
                    SyllabusList = _StudentApplicationContext.Master_Streams.Where(t => t.MI_Id == dt.MI_Id && t.ASMST_StreamCode == "OTH").ToList();
                    dt.syllabuslistoth = SyllabusList.ToArray();
                }

                dt.streamexams = (from a in _db.Adm_School_Master_CE
                                  from b in _db.Adm_School_Stream_Class_CE
                                  where (a.ASMCE_Id == b.ASMCE_Id && a.MI_Id == b.MI_Id && b.ASMCL_Id == qry1.FirstOrDefault().ASMCL_Id && b.ASMST_Id == qry1.FirstOrDefault().ASMST_Id)
                                  select new StudentApplicationDTO
                                  {
                                      ASMCE_CEName = a.ASMCE_CEName,
                                      ASMCE_Id = a.ASMCE_Id,
                                      ASSTCLCE_CompulsoryFlg = b.ASSTCLCE_CompulsoryFlg
                                  }
       ).Distinct().ToArray();

                var streamexams = (from a in _db.PA_School_Application_CE
                                   where (a.PASR_Id == qry1.FirstOrDefault().pasr_id)
                                   select new StudentApplicationDTO
                                   {
                                       ASMCE_Id = a.ASMCE_Id
                                   }
          ).Distinct().ToArray();
                if (streamexams.Count() > 0)
                {
                    dt.ASMCE_Id = streamexams.FirstOrDefault().ASMCE_Id;
                }

                List<MasterReference> StudentReferenceDMO = new List<MasterReference>();
                StudentReferenceDMO = _StudentApplicationContext.Preadmission_Master_Reference.ToList();
                dt.StudentReferenceDetails = StudentReferenceDMO.ToArray();

                List<MasterSource> StudentSourceDMO = new List<MasterSource>();
                StudentSourceDMO = _StudentApplicationContext.Preadmission_Master_Source.ToList();
                dt.StudentSourceDetails = StudentSourceDMO.ToArray();

                List<PA_School_Application_Reference> edit_StudentReferenceDMO = new List<PA_School_Application_Reference>();
                edit_StudentReferenceDMO = _StudentApplicationContext.PA_School_Application_Reference.Where(t => t.PASR_Id.Equals(dt.pasR_Id)).ToList();
                dt.edit_StudentReferenceDetails = edit_StudentReferenceDMO.ToArray();

                List<PA_School_Application_Source> edit_StudentSourceDMO = new List<PA_School_Application_Source>();
                edit_StudentSourceDMO = _StudentApplicationContext.PA_School_Application_Source.Where(t => t.PASR_Id.Equals(dt.pasR_Id)).ToList();
                dt.edit_StudentSourceDetails = edit_StudentSourceDMO.ToArray();

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

                if (dt.offon == 1)
                {
                    var alreadyExistEmailId = _StudentApplicationContext.Enq.Where(d => d.pasr_id == dt.pasR_Id).ToList();
                    var stuid = _StudentApplicationContext.Enq.Single(d => d.pasr_id == dt.pasR_Id).ASMAY_Id;

                    dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASMCL_Id;
                    dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASMAY_Id;
                    //dt.PASR_FirstName = alreadyExistEmailId.FirstOrDefault().PASR_FirstName;
                    dt.PASR_FirstName = ((alreadyExistEmailId.FirstOrDefault().PASR_FirstName == null || alreadyExistEmailId.FirstOrDefault().PASR_FirstName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PASR_FirstName) + " " + (alreadyExistEmailId.FirstOrDefault().PASR_MiddleName == null || alreadyExistEmailId.FirstOrDefault().PASR_MiddleName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PASR_MiddleName) + " " + (alreadyExistEmailId.FirstOrDefault().PASR_LastName == null || alreadyExistEmailId.FirstOrDefault().PASR_LastName == "0" ? "" : alreadyExistEmailId.FirstOrDefault().PASR_LastName)).Trim();
                    dt.PASR_emailId = alreadyExistEmailId.FirstOrDefault().PASR_emailId;
                    dt.PASR_MobileNo = alreadyExistEmailId.FirstOrDefault().PASR_MobileNo;
                    dt.PASR_RegistrationNo = alreadyExistEmailId.FirstOrDefault().PASR_RegistrationNo;
                    dt.PASR_PerCity = alreadyExistEmailId.FirstOrDefault().PASR_PerCity;
                    dt.PASR_FatherPresentAddress = alreadyExistEmailId.FirstOrDefault().PASR_FatherPresentAddress;
                    if (dt.configurationsettings.ISPAC_ApplFeeFlag == 1)
                    {
                        dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == dt.pasR_Id).Count();

                        if (dt.payementcheck == 0)
                        {
                            dt.paydet = paymentPart(dt);
                        }
                    }
                }
                else if (dt.offon == 2)
                {
                    var alreadyExistEmailId = _StudentApplicationContext.Enq.Where(d => d.pasr_id == dt.pasR_Id).ToList();

                    dt.PASR_emailId = alreadyExistEmailId.FirstOrDefault().PASR_emailId;
                    dt.PASR_MobileNo = alreadyExistEmailId.FirstOrDefault().PASR_MobileNo;
                    Email Email = new Email(_db);
                    Email.sendmail(dt.MI_Id, dt.PASR_emailId, "STUDENT_OFFLINE_MSG", dt.pasR_Id);

                    SMS sms = new SMS(_db);
                    sms.sendSms(dt.MI_Id, dt.PASR_MobileNo, "STUDENT_OFFLINE_MSG", dt.pasR_Id);
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
                var qry4 = (from sblng in _StudentApplicationContext.stsblng.Where(sblng => sblng.PASR_Id == dt.pasR_Id && sblng.PASRS_SiblingsName != null && sblng.PASRS_SiblingsName != "" && sblng.PASRS_SiblingsName != "NAN" && sblng.PASRS_SiblingsName != "NA" && sblng.PASRS_SiblingsName != "na" && sblng.PASRS_SiblingsName != "No") select sblng);
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
                dt.ASMAY_Id = qry1.FirstOrDefault().ASMAY_Id;

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
                                from d in _StudentApplicationContext.Master_Streams
                                where (a.ASMST_Id == d.ASMST_Id && a.pasr_id == dt.pasR_Id)
                                select new StudentApplicationDTO
                                {
                                    IMC_CasteName = d.ASMST_StreamName
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


                if (qry5.Count() > 0)
                {


                    if (qry5.FirstOrDefault().TRMA_Id > 0)
                    {
                        var trmaid = qry5.FirstOrDefault().TRMA_Id;
                        List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                        saa = _db.MasterAreaDMO.Where(r => r.MI_Id == dt.MI_Id && r.TRMA_Id == trmaid).ToList();

                        if (saa.Count() > 0)
                        {
                            dt.studentarea = saa.FirstOrDefault().TRMA_AreaName;
                        }

                    }

                    if (qry5.FirstOrDefault().PASTA_PickUp_TRMR_Id > 0)
                    {
                        var studentroutelist = (
                                     from b in _StudentApplicationContext.MasterRouteDMO
                                     where (b.TRMR_Id == qry5.FirstOrDefault().PASTA_PickUp_TRMR_Id)
                                     select new MasterRouteDMO
                                     {
                                         TRMR_Id = b.TRMR_Id,
                                         TRMR_RouteName = b.TRMR_RouteName,
                                         TRMR_order = b.TRMR_order
                                     }
               ).OrderBy(t => t.TRMR_order).ToList().ToList();

                        if (studentroutelist.Count() > 0)
                        {
                            dt.studentroue = studentroutelist.FirstOrDefault().TRMR_RouteName;
                        }
                    }

                    if (qry5.FirstOrDefault().PASTA_PickUp_TRML_Id > 0)
                    {
                        var studentlocationlist = (
                                     from b in _StudentApplicationContext.MasterLocationDMO
                                     where (b.TRML_Id == qry5.FirstOrDefault().PASTA_PickUp_TRML_Id)
                                     select new MasterLocationDMO
                                     {
                                         TRML_Id = b.TRML_Id,
                                         TRML_LocationName = b.TRML_LocationName
                                     }
               ).Distinct().ToList().ToList();

                        if (studentlocationlist.Count() > 0)
                        {
                            dt.studentlocation = studentlocationlist.FirstOrDefault().TRML_LocationName;
                        }


                    }
                }

                var qry7 = (from source in _StudentApplicationContext.PreadmissionStudnetSource.Where(trns => trns.PASR_Id == dt.pasR_Id) select source);
                dt.Studentsource_DTObj = qry7.ToArray();



                if (dt.Studentsource_DTObj.Length > 0)
                {
                    var sourceid = qry7.FirstOrDefault().PAMS_Id;
                    var studentsource = (
                                    from b in _ProspectusContext.source
                                    where (b.PAMS_Id == sourceid)
                                    select new MasterSource
                                    {
                                        PAMS_SourceName = b.PAMS_SourceName
                                    }
              ).ToList().ToList();

                    if (studentsource.Count() > 0)
                    {
                        dt.studentsource = studentsource.FirstOrDefault().PAMS_SourceName;
                    }

                }

                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == dt.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                //dt.ASMAY_Id = Acdemic_preadmission;

                var helathflag = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id == dt.MI_Id && d.ASMAY_Id == dt.ASMAY_Id).Select(d => d.ISPAC_Healthapp);
                if (helathflag.FirstOrDefault() == 1)
                {
                    StudentHelthcertificateDTO health = new StudentHelthcertificateDTO();
                    health.MI_Id = dt.MI_Id;
                    health.pasr_id = dt.pasR_Id;


                    health.PASHD_Id = _StudentApplicationContext.StudentHelthcertificate.Single(d => d.PASR_Id.Equals(dt.pasR_Id)).PASHD_Id;
                    dt.studenthelthDTO = printgethelthData(health).studenthelthDTO;


                    dt.vaccines = (from a in _StudentApplicationContext.PA_Student_Vaccines
                                   from c in _StudentApplicationContext.PA_Master_Vaccines
                                   from b in _StudentApplicationContext.StudentHelthcertificate
                                   where (a.PASHD_Id == b.PASHD_Id && a.PAMVA_Id == c.PAMVA_Id && c.MI_Id == dt.MI_Id && b.MI_Id == dt.MI_Id && b.PASHD_Id.Equals(health.PASHD_Id))
                                   select new StudentHelthcertificateDTO
                                   {
                                       PAMVA_Id = c.PAMVA_Id,
                                       PAMVA_Vaccines = c.PAMVA_Vaccines,
                                       PAMVA_YesNoFlg = a.PASVA_YesNoNAFlg
                                   }
                   ).ToList().ToArray();
                }


                var qry10 = (from Docs in _StudentApplicationContext.PA_School_Application_ElectiveSujects
                             from mDocs in _StudentApplicationContext.Enq
                             from subjects in _StudentApplicationContext.MasterSubjectDMO
                             from groupid in _StudentApplicationContext.Exm_Master_GroupDMO
                             from groupmap in _StudentApplicationContext.Exm_Master_Group_SubjectsDMO
                             where (Docs.PASR_Id == mDocs.pasr_id && Docs.ISMS_Id == subjects.ISMS_Id && Docs.ISMS_Id == groupmap.ISMS_Id && subjects.ISMS_Id == groupmap.ISMS_Id && groupmap.EMG_Id == groupid.EMG_Id && Docs.EMG_Id == groupid.EMG_Id && Docs.EMG_Id == groupmap.EMG_Id && Docs.PASR_Id == dt.pasR_Id)
                             select new Preadmissionelectives
                             {
                                 EMG_Id = groupmap.EMG_Id,
                                 ismS_Id = Convert.ToInt16(Docs.ISMS_Id),
                                 EMG_MinAplSubjects = groupid.EMG_MinAplSubjects,
                                 EMG_MaxAplSubjects = groupid.EMG_MaxAplSubjects,
                                 EMG_GroupName = groupid.EMG_GroupName
                             });
                dt.StudentSubjects_DTObj = qry10.ToArray();
                dt.ASMST_Id = qry1.FirstOrDefault().ASMST_Id;
                GetSubjects(dt);

                dt.streamexamsprint = (from a in _db.Adm_School_Master_CE
                                       from b in _db.Adm_School_Stream_Class_CE
                                       where (a.ASMCE_Id == b.ASMCE_Id && a.MI_Id == b.MI_Id && b.ASMCL_Id == dt.ASMCL_Id && b.ASMST_Id == dt.ASMST_Id)
                                       select new StudentApplicationDTO
                                       {
                                           ASMCE_CEName = a.ASMCE_CEName,
                                           ASMCE_Id = a.ASMCE_Id,
                                           ASSTCLCE_CompulsoryFlg = b.ASSTCLCE_CompulsoryFlg
                                       }
    ).Distinct().ToArray();

                var studentstrem = (from a in _db.Master_stream
                                    from b in _db.StudentApplication
                                    where (a.ASMST_Id == b.ASMST_Id && b.pasr_id == dt.pasR_Id)
                                    select new StudentApplicationDTO
                                    {
                                        studentstream = a.ASMST_StreamName

                                    }
    ).Distinct().ToArray();
                if (studentstrem.Count() > 0)
                {
                    dt.studentstream = studentstrem.FirstOrDefault().studentstream;
                }

                var streamexams = (from a in _db.PA_School_Application_CE
                                   where (a.PASR_Id == qry1.FirstOrDefault().pasr_id)
                                   select new StudentApplicationDTO
                                   {
                                       ASMCE_Id = a.ASMCE_Id
                                   }
          ).Distinct().ToArray();
                if (streamexams.Count() > 0)
                {
                    dt.ASMCEId = streamexams.FirstOrDefault().ASMCE_Id;
                }

                var streamCE = (from a in _db.PA_School_Application_CE
                                from b in _db.Adm_School_Master_CE
                                where (a.PASR_Id == qry1.FirstOrDefault().pasr_id && b.ASMCE_Id == a.ASMCE_Id)
                                select new StudentApplicationDTO
                                {
                                    ASMCE_Id = a.ASMCE_Id,
                                    ASMCE_CEName=b.ASMCE_CEName

                                }).ToArray();
                if(streamCE!=null && streamCE.Length>0)
                {
                    dt.streamCEprint = streamCE.ToArray();
                }

               


                List<MasterReference> StudentReferenceDMO = new List<MasterReference>();
                StudentReferenceDMO = _StudentApplicationContext.Preadmission_Master_Reference.ToList();
                dt.StudentReferenceDetails = StudentReferenceDMO.ToArray();

                List<MasterSource> StudentSourceDMO = new List<MasterSource>();
                StudentSourceDMO = _StudentApplicationContext.Preadmission_Master_Source.ToList();
                dt.StudentSourceDetails = StudentSourceDMO.ToArray();

                List<PA_School_Application_Reference> edit_StudentReferenceDMO = new List<PA_School_Application_Reference>();
                edit_StudentReferenceDMO = _StudentApplicationContext.PA_School_Application_Reference.Where(t => t.PASR_Id.Equals(dt.pasR_Id)).ToList();
                dt.edit_StudentReferenceDetails = edit_StudentReferenceDMO.ToArray();

                List<PA_School_Application_Source> edit_StudentSourceDMO = new List<PA_School_Application_Source>();
                edit_StudentSourceDMO = _StudentApplicationContext.PA_School_Application_Source.Where(t => t.PASR_Id.Equals(dt.pasR_Id)).ToList();
                dt.edit_StudentSourceDetails = edit_StudentSourceDMO.ToArray();
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
        public StudentApplicationDTO getemployees(StudentApplicationDTO dt)
        {
            dt.fillstaff = (from a in _db.HR_Master_Employee_DMO
                            where a.MI_Id == dt.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                            select new StaffLoginDTO
                            {
                                HRME_Id = a.HRME_Id,
                                stafname = ((a.HRME_EmployeeFirstName == null ? "" : a.HRME_EmployeeFirstName.ToUpper()) + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName.ToUpper()) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName.ToUpper())).Trim(),
                            }).ToArray();


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
                                  where (a.TRMA_Id == b.TRMA_Id && a.TRMA_Id == id && b.TRMR_ActiveFlg == true)
                                  select new StudentBuspassFormDTO
                                  {
                                      TRMR_Id = b.TRMR_Id,
                                      TRMR_RouteName = b.TRMR_RouteName,
                                      TRMR_order = b.TRMR_order
                                  }
                 ).OrderBy(t => t.TRMR_order).ToList().ToArray();

                data.locationlist = (from a in _StudentApplicationContext.Route_Location
                                     from b in _StudentApplicationContext.MasterRouteDMO
                                     from c in _StudentApplicationContext.MasterLocationDMO
                                     from d in _StudentApplicationContext.MasterAreaDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && d.TRMA_Id == b.TRMA_Id && d.TRMA_Id == id && b.TRMR_ActiveFlg == true && c.TRML_ActiveFlg == true)
                                     select new StudentBuspassFormDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                 ).Distinct().ToList().ToArray();
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
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && b.TRMR_Id == id && b.TRMR_ActiveFlg == true && c.TRML_ActiveFlg == true)
                                     select new StudentBuspassFormDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                 ).Distinct().ToList().ToArray();
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

        //district
        public async Task<DistrictDTO> getDistrictByState(int id)
        {
            DistrictDTO alldistrict = new DistrictDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var st = await _StudentApplicationContext.DistrictDMO.Where(d => d.IVRMMS_Id == id).ToListAsync();
                alldistrict.districtDrpDown = st.ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return alldistrict;
        }
        public async Task<StateDTO> getdprospectusdetails(int id)
        {
            StateDTO allstate = new StateDTO();
            try
            {
                //List<City> allcity = new List<City>();
                var st = await _db.prospectus.Where(d => d.PASP_Id == id).ToListAsync();
                allstate.prospectusdetails = st.ToArray();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(st.FirstOrDefault().MI_ID) && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToListAsync();
                allstate.admissioncatdrp = allclass.ToArray();

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
        public async Task<StudentApplicationDTO> studdet(StudentApplicationDTO stu)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //srkvs deepak
                var prospectosNumber = _db.prospectus.Where(d => d.PASP_Id == stu.PASP_Id).Select(d => d.PASP_ProspectusNo).ToList();
                stu.PASP_ProspectusNo = prospectosNumber.FirstOrDefault();
                string gjhghj = stu.PASR_Emisno;
                //  var countstu = _StudentApplicationContext.Enq.Where(t => t.ASMAY_Id == stu.ASMAY_Id && t.ASMCL_Id == stu.ASMCL_Id && t.Id == stu.Id).Count();

                AdmissionStatus AdmissionStatus = new AdmissionStatus();
                // AdmissionStatus = _StudentApplicationContext.AdmissionStatus.SingleOrDefault(d => d.MI_Id == stu.MI_Id && d.PAMS_StatusFlag.Contains("SELECT"));
                //stu.PAMS_Id = AdmissionStatus.PAMS_Id;

                List<MasterAcademic> Acresult = new List<MasterAcademic>();
                Acresult = _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.Is_Active == true && t.ASMAY_Pre_ActiveFlag == 1).ToList();


                var classmaxage = _StudentApplicationContext.AdmissionClass.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_Id == stu.ASMCL_Id).ToList();
                DateTime _cut_of_date = Convert.ToDateTime(Acresult[0].ASMAY_Cut_Of_Date);
                DateTime Min_date = DateTime.Today;
                DateTime Max_date = DateTime.Today;

                if (classmaxage != null)
                {
                    int Min_d = classmaxage.FirstOrDefault().ASMCL_MinAgeDays;
                    int Min_m = classmaxage.FirstOrDefault().ASMCL_MinAgeMonth;
                    int Min_Y = classmaxage.FirstOrDefault().ASMCL_MinAgeYear;
                    int Max_d = classmaxage.FirstOrDefault().ASMCL_MaxAgeDays;
                    int Max_m = classmaxage.FirstOrDefault().ASMCL_MaxAgeMonth;
                    int Max_Y = classmaxage.FirstOrDefault().ASMCL_MaxAgeYear;

                    Max_date = _cut_of_date.AddYears(-Min_Y).AddMonths(-Min_m).AddDays(-Min_d);
                    Min_date = _cut_of_date.AddYears(-Max_Y).AddMonths(-Max_m).AddDays(-Max_d);

                    if (Max_date < stu.PASR_DOB)
                    {
                        stu.PASR_UnderAge = Convert.ToInt16((stu.PASR_DOB - Max_date).TotalDays);

                    }
                    else if (Min_date > stu.PASR_DOB)
                    {
                        stu.PASR_OverAge = Convert.ToInt16((Min_date - stu.PASR_DOB).TotalDays);
                    }

                }

                var classcategory = _StudentApplicationContext.Masterclasscategory.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_Id == stu.ASMCL_Id && d.ASMAY_Id == stu.ASMAY_Id).ToList();
                if (classcategory.Count > 0)
                {
                    stu.AMC_Id = classcategory.FirstOrDefault().AMC_Id;
                    if (stu.Caste_Id != 0)
                    {
                        var castecat = _StudentApplicationContext.caste.Where(d => d.MI_Id == stu.MI_Id && d.IMC_Id == stu.Caste_Id).ToList();
                        if (stu.CasteCategory_Id == 0)



                        {
                            stu.CasteCategory_Id = castecat.FirstOrDefault().IMCC_Id;
                        }

                    }
                    //else
                    //{
                    //    stu.CasteCategory_Id = 0;
                    //}
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

                        // if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                        //|| rolelist[0].IVRMRT_Role.Equals("PRINCIPAL", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))

                        if (!rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))

                        {
                            stu.PASR_FirstName = stu.PASR_FirstName.ToUpper();
                            if (stu.PASR_MiddleName != null && stu.PASR_MiddleName != "")
                            {
                                stu.PASR_MiddleName = stu.PASR_MiddleName.ToUpper();
                            }
                            if (stu.PASR_LastName != null && stu.PASR_LastName != "")
                            {
                                stu.PASR_LastName = stu.PASR_LastName.ToUpper();
                            }

                            StudentApplication enq = Mapper.Map<StudentApplication>(stu);

                            long Defaultstatus = _StudentApplicationContext.AdmissionStatus.Single(d => d.PAMST_StatusFlag.Equals("INP") && d.MI_Id == stu.MI_Id).PAMST_Id;


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
                            enq.PAMS_Id = Defaultstatus;
                            enq.PASR_DOBWords = stu.PASR_DOBWords;
                            enq.PASR_ActiveFlag = 0;
                                //added by 02/02/2017

                                //TBHS
                                //enq.PASA_MedicalFitForSportsFlg = stu.PASA_MedicalFitForSportsFlg;
                                //enq.PASA_HealthIssueFlg = stu.PASA_HealthIssueFlg;
                                //enq.PASA_JoiningReasons = stu.PASA_JoiningReasons;

                            enq.PASR_Date = indianTime;
                            enq.CreatedDate = indianTime;
                            enq.UpdatedDate = indianTime;
                            _StudentApplicationContext.Add(enq);
                            _StudentApplicationContext.SaveChanges();
                            stu.pasR_Id = enq.pasr_id;

                            PreadmissionSchoolRegistrationStudentLogin MM2 = new PreadmissionSchoolRegistrationStudentLogin();

                            MM2.PASR_Id = stu.pasR_Id;
                            MM2.Preadmission_School_Registration_PASR_Id = stu.pasR_Id;
                            MM2.IVRMOUL_Id = stu.Id;
                            MM2.IVRM_Others_User_Login_IVRMOUL_Id = stu.Id;
                            MM2.PASRSTUL_Date = indianTime;
                            MM2.PASRSTUL_EntryType = Convert.ToString(DateTime.Now);
                            MM2.CreatedDate = indianTime;
                            MM2.UpdatedDate = indianTime;
                            MM2.PASRSTUL_MAACAdd = stu.PASRSTUL_MAACAdd;
                            MM2.PASRSTUL_NetIp = stu.PASRSTUL_NetIp;
                            MM2.PASRSTUL_IPAdd = "";
                            _StudentApplicationContext.Add(MM2);
                            _StudentApplicationContext.SaveChanges();

                            List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                            mstConfig = await _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToListAsync();
                            stu.mstConfig = mstConfig.ToArray();
                            //srkvs deepak
                            //if (mstConfig.FirstOrDefault().ISPAC_ProsptFeeApp == 1)
                            //{
                            //    PA_School_Application_ProspectusDMO prosp = new PA_School_Application_ProspectusDMO();
                            //    prosp.PASR_Id = stu.pasR_Id;
                            //    prosp.PASP_Id = stu.PASP_Id;
                            //    _StudentApplicationContext.Add(prosp);
                            //    _StudentApplicationContext.SaveChanges();
                            //}

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
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                            stu.message = "Successfully Submitted the Application!!";
                            stu.healthflag = true;
                            }
                            else
                            {
                                stu.message = "Registration Number is not Generated. kindly contact Admin";
                            }
                        }
                        else
                        {
                            int countstu = _StudentApplicationContext.Enq.Where(t => t.ASMAY_Id == stu.ASMAY_Id && t.ASMCL_Id == stu.ASMCL_Id && t.Id == stu.Id).Count();
                            if (countstu < stu.configurationsettings.ISPAC_NoofApplications)
                            {
                                StudentApplication enq = Mapper.Map<StudentApplication>(stu);
                                long Defaultstatus = _StudentApplicationContext.AdmissionStatus.Single(d => d.PAMST_StatusFlag.Equals("INP") && d.MI_Id == stu.MI_Id).PAMST_Id;
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

                                enq.PAMS_Id = Defaultstatus;
                                enq.PASR_DOBWords = stu.PASR_DOBWords;
                                //added by 02/02/2017
                                enq.PASR_Applicationno = enq.PASR_RegistrationNo;
                                enq.PASR_Date = indianTime;
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
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                                stu.message = "Successfully Submitted the Application!!";

                                }
                                else
                                {
                                    stu.message = "Registration Number is not Generated. kindly contact Admin";
                                }
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
                        if (stu.Caste_Id != 0)
                        {
                            var castecatt = _StudentApplicationContext.caste.Where(d => d.MI_Id == stu.MI_Id && d.IMC_Id == stu.Caste_Id).ToList();
                            if (stu.CasteCategory_Id == 0)
                            {
                                stu.CasteCategory_Id = castecatt.FirstOrDefault().IMCC_Id;
                            }
                        }
                        //else
                        //{
                        //    stu.CasteCategory_Id = 0;
                        //}

                        //TBHS
                        //stureg.PASA_MedicalFitForSportsFlg = stu.PASA_MedicalFitForSportsFlg;
                        //stureg.PASA_HealthIssueFlg = stu.PASA_HealthIssueFlg;
                        //stureg.PASA_JoiningReasons = stu.PASA_JoiningReasons;
                        //stureg.PASR_Stayingwith = stu.PASR_Stayingwith;

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
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
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
                        Sourceofstudent(stu);
                        employeemap(stu);
                        StudentRTE(stu);
                        Studentexam(stu);

                        RefferenceDetailsAddUpdate(stu);
                        SourceDetailsAddUpdate(stu);

                        if (stu.configurationsettings.ISPAC_ApplFeeFlag == 1 && helathflag.FirstOrDefault() != 1)
                        {
                            stu.paymentapplicable = "Pay";
                            stu.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == stu.pasR_Id).Count();

                            //if (stu.payementcheck == 0)
                            //{
                            //    stu.paydet = paymentPart(stu);
                            //}
                        }
                        else if (stu.configurationsettings.ISPAC_ApplFeeFlag == 0)
                        {


                            List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                            mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                            //if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                            //{
                            //    Email Email = new Email(_db);

                            //    Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_REGISTRATION", stu.pasR_Id);
                            //}

                            //if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                            //{
                            //    SMS sms = new SMS(_db);
                            //    await sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_REGISTRATION", stu.pasR_Id);
                            //}

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

                        if (stu.updateform == false)
                        {
                            Email Email = new Email(_db);
                            Email.sendmail(stu.MI_Id, stu.PASR_emailId, "STUDENT_APP_FILL", stu.pasR_Id);

                            SMS sms = new SMS(_db);
                            await sms.sendSms(stu.MI_Id, stu.PASR_MobileNo, "STUDENT_APP_FILL", stu.pasR_Id);
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
                else
                {
                    stu.message = "Kindy do category Mapping.";
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
                        if (mob.EMG_GroupName != null && mob.ismS_Id != null)
                        {

                            PA_School_Application_ElectiveSujects sibling = new PA_School_Application_ElectiveSujects();

                            sibling.PASR_Id = stu.pasR_Id;
                            sibling.ISMS_Id = Convert.ToInt64(mob.ismS_Id);
                            sibling.EMG_Id = mob.EMG_Id;
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
        public void Sourceofstudent(StudentApplicationDTO stu)
        {
            try
            {
                //List<long> temparr = new List<long>();


                if (stu.PAMSId > 0)
                {


                    Array siblingNoresultremove = _StudentApplicationContext.PreadmissionStudnetSource.Where(t => t.PASR_Id == stu.pasR_Id).ToArray();
                    foreach (PreadmissionStudnetSource ph1 in siblingNoresultremove)
                    {
                        _StudentApplicationContext.Remove(ph1);
                    }

                    if (stu.PAMS_Id > 0)
                    {

                        PreadmissionStudnetSource sibling = new PreadmissionStudnetSource();

                        sibling.PASR_Id = stu.pasR_Id;
                        sibling.PAMS_Id = stu.PAMSId;
                        sibling.CreatedDate = DateTime.Now;
                        sibling.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Add(sibling);
                        _StudentApplicationContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void employeemap(StudentApplicationDTO stu)
        {
            try
            {
                //List<long> temparr = new List<long>();


                if (stu.Staffid > 0)
                {


                    Array siblingNoresultremove = _StudentApplicationContext.Preadmission_School_Registration_Employee.Where(t => t.PASR_Id == stu.pasR_Id).ToArray();
                    foreach (Preadmission_School_Registration_Employee ph1 in siblingNoresultremove)
                    {
                        _StudentApplicationContext.Remove(ph1);
                    }

                    if (stu.Staffid > 0)
                    {

                        Preadmission_School_Registration_Employee siblingcc = new Preadmission_School_Registration_Employee();

                        siblingcc.PASR_Id = stu.pasR_Id;
                        siblingcc.HRME_ID = stu.Staffid;
                        siblingcc.PSRE_ActiveFlag = null;
                        _StudentApplicationContext.Add(siblingcc);
                        _StudentApplicationContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void StudentRTE(StudentApplicationDTO stu)
        {
            try
            {
                var concessiontype = _StudentApplicationContext.Fee_Master_ConcessionDMO.Single(t => t.FMCC_Id == stu.FMCC_ID).FMCC_ConcessionFlag;
                if (concessiontype == "R")
                {
                    var duplicatecheck = _StudentApplicationContext.PA_Student_Sibblings.Where(d => d.PASR_Id == stu.pasR_Id).ToList();

                    if (duplicatecheck.Count() == 0)
                    {
                        PA_Student_Sibblings siblingsave = new PA_Student_Sibblings();
                        siblingsave.MI_Id = stu.MI_Id;
                        siblingsave.PASR_Id = stu.pasR_Id;
                        siblingsave.PASS_ActiveFlag = false;
                        siblingsave.PASS_ConcessionAmt = 100;
                        siblingsave.PASS_ConcessionPer = null;
                        siblingsave.CreatedDate = DateTime.Now;
                        siblingsave.UpdatedDate = DateTime.Now;
                        _StudentApplicationContext.Add(siblingsave);
                        _StudentApplicationContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void Studentexam(StudentApplicationDTO stu)
        {
            try
            {
                //var examcounts = _db.PA_School_Application_CE.Where(t => t.PASR_Id == stu.pasR_Id).ToList();
                //if (examcounts.Count>0)
                //{
                var duplicatecheck = _db.PA_School_Application_CE.Where(d => d.PASR_Id == stu.pasR_Id).ToList();

                if (duplicatecheck.Count() == 0)
                {
                    PA_School_Application_CE siblingsave = new PA_School_Application_CE();
                    siblingsave.PASR_Id = stu.pasR_Id;
                    siblingsave.ASMCE_Id = Convert.ToInt64(stu.ASMCE_Id);
                    siblingsave.PASACE_CreatedBy = stu.Id;
                    siblingsave.PASACE_UpdatedBy = stu.Id;
                    siblingsave.PASACE_ActiveFlg = true;
                    siblingsave.CreatedDate = DateTime.Now;
                    siblingsave.UpdatedDate = DateTime.Now;
                    _db.Add(siblingsave);
                    _db.SaveChanges();
                }
                else
                {
                    var updateasmce = _db.PA_School_Application_CE.Single(t => t.PASR_Id == stu.pasR_Id);
                    updateasmce.ASMCE_Id = Convert.ToInt64(stu.ASMCE_Id);
                    updateasmce.PASACE_UpdatedBy = stu.Id;
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
        public void StudentTransportAddUpdate(StudentApplicationDTO stu)
        {

            try
            {
                // Student Transport starts
                if (stu.trmA_Id != 0 && (stu.trmL_Idp != 0 && stu.trmL_Idp != null))
                {
                    PA_Student_Transport_ApplicationDMO trnsp = new PA_Student_Transport_ApplicationDMO();

                    var trnspreslt = _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Where(s => s.PASR_Id == stu.pasR_Id).ToList();

                    if (trnspreslt.Count > 0)
                    {
                        var trnspresltt = _StudentApplicationContext.PA_Student_Transport_ApplicationDMO.Single(s => s.PASR_Id == stu.pasR_Id);


                        //added by 02/02/2017
                        trnspresltt.TRMA_Id = stu.trmA_Id;
                        trnspresltt.PASTA_PickUp_TRMR_Id = stu.trmR_Idp;
                        trnspresltt.PASTA_Drop_TRMR_Id = stu.trmR_Idp;
                        trnspresltt.PASTA_PickUp_TRML_Id = stu.trmL_Idp;
                        trnspresltt.PASTA_Drop_TRML_Id = stu.trmL_Idp;
                        trnspresltt.UpdatedDate = DateTime.Now;

                        _StudentApplicationContext.Update(trnspresltt);
                        _StudentApplicationContext.SaveChanges();
                    }
                    else
                    {

                        //added by 02/02/2017
                        trnsp.TRMA_Id = stu.trmA_Id;
                        trnsp.PASTA_PickUp_TRMR_Id = stu.trmR_Idp;
                        trnsp.PASTA_Drop_TRMR_Id = stu.trmR_Idp;
                        trnsp.PASTA_PickUp_TRML_Id = stu.trmL_Idp;
                        trnsp.PASTA_Drop_TRML_Id = stu.trmL_Idp;
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
                        if (stu.PASRG_emailid != null)
                        {
                            regis reg = new regis();
                            reg.Email_id = stu.PASRG_emailid;
                            reg.password = "Password@123";
                            reg.username = stu.PASRG_emailid;
                            await createLogin(reg, "Guardian", "Guardian");
                        }
                    }
                }

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
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                int agepoints = 0;
                int castepoints = 0;

                List<MasterAcademic> result = new List<MasterAcademic>();
                result = _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && TimeZoneInfo.ConvertTime(t.ASMAY_PreAdm_F_Date.Value, INDIAN_ZONE) <= indianTime && TimeZoneInfo.ConvertTime(t.ASMAY_PreAdm_T_Date.Value, INDIAN_ZONE) >= indianTime && t.Is_Active == true).ToList();
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

                decimal totalsalary = Convert.ToDecimal(stu.PASR_FatherIncome) + Convert.ToDecimal(stu.PASR_MotherIncome);

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
        public StudentApplicationDTO classoverunderage(StudentApplicationDTO stu)
        {

            if (stu.ASMST_Id == 0)
            {

                List<MasterAcademic> Acresult = new List<MasterAcademic>();
                Acresult = _StudentApplicationContext.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.Is_Active == true && t.ASMAY_Pre_ActiveFlag == 1).ToList();


                var classmaxageu = _StudentApplicationContext.AdmissionClass.Where(d => d.MI_Id == stu.MI_Id && d.ASMCL_Id == stu.ASMCL_Id).ToList();
                DateTime _cut_of_date = Convert.ToDateTime(Acresult[0].ASMAY_Cut_Of_Date);
                DateTime Min_date = DateTime.Today;
                DateTime Max_date = DateTime.Today;

                if (classmaxageu != null)
                {
                    int Min_d = classmaxageu.FirstOrDefault().ASMCL_MinAgeDays;
                    int Min_m = classmaxageu.FirstOrDefault().ASMCL_MinAgeMonth;
                    int Min_Y = classmaxageu.FirstOrDefault().ASMCL_MinAgeYear;
                    int Max_d = classmaxageu.FirstOrDefault().ASMCL_MaxAgeDays;
                    int Max_m = classmaxageu.FirstOrDefault().ASMCL_MaxAgeMonth;
                    int Max_Y = classmaxageu.FirstOrDefault().ASMCL_MaxAgeYear;

                    Max_date = _cut_of_date.AddYears(-Min_Y).AddMonths(-Min_m).AddDays(-Min_d);
                    Min_date = _cut_of_date.AddYears(-Max_Y).AddMonths(-Max_m).AddDays(-Max_d);

                    if (Max_date < stu.dateString)
                    {
                        stu.PASR_UnderAge = Convert.ToInt16((stu.dateString - Max_date).TotalDays);
                        stu.message = "UnderAge by " + stu.PASR_UnderAge + " days.";

                    }
                    else if (Min_date > stu.dateString)
                    {
                        stu.PASR_OverAge = Convert.ToInt16((Min_date - stu.dateString).TotalDays);
                        stu.message = "OverAge by " + stu.PASR_OverAge + " days.";
                    }

                }
            }
            else
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Preadmission_Board_Class";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(stu.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(stu.ASMST_Id)
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObjectz = new List<dynamic>();


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
                        stu.Class_list = retObjectz.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return stu;
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
                List<AdmissionClass> lessclass1 = new List<AdmissionClass>();

                List<MasterConfiguration> config = new List<MasterConfiguration>();
                config = _StudentApplicationContext.masterConfig.Where(t => t.MI_Id == stu.MI_Id && t.ASMAY_Id == result.FirstOrDefault().ASMAY_Id).ToList();

                List<Preadmission_Special_Registration> Specialuser = new List<Preadmission_Special_Registration>();
                Specialuser = _StudentApplicationContext.Preadmission_Special_Registration.Where(t => t.ID == stu.Id).ToList();

                var AdmissionStatusflag = config.FirstOrDefault().ISPAC_DefaultStatusFlag;

                string rolename = _feecontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == stu.roleid).IVRMRT_Role;
                if (Specialuser.Count > 0)
                {
                    rolename = "Special";
                }
                int maxage = 1;
                int minage = 1;
                if (rolename.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                {
                    maxage = config.FirstOrDefault().ISPAC_DOBMaxAgeFlag;
                    minage = config.FirstOrDefault().ISPAC_DOBMinAgeFlag;
                    if (Specialuser.Count > 0)
                    {
                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                    }
                    else
                    {
                        allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
                    }
                }
                else
                {
                    maxage = 0;
                    minage = 0;
                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
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

                if (config.FirstOrDefault().ISPAC_ApplNoIncrementFlg == true)
                {
                    if (stu.Class_list.Length == 1)
                    {
                        long asmclidof = termsList.FirstOrDefault().ASMCL_Id;

                        var cls_orderid = (from a in _StudentApplicationContext.AdmissionClass
                                           where (a.ASMCL_Id == asmclidof && a.MI_Id == stu.MI_Id)
                                           select new StudentBuspassFormDTO
                                           {
                                               cls_Order = a.ASMCL_Order - 1
                                           }
             ).ToList().ToArray();
                        if (cls_orderid.Length > 0)
                        {
                            var class_Id = (from a in _StudentApplicationContext.AdmissionClass
                                            where (a.ASMCL_Order == cls_orderid[0].cls_Order && a.MI_Id == stu.MI_Id)
                                            select new StudentBuspassFormDTO
                                            {
                                                ASMCL_Id = a.ASMCL_Id,
                                                ASMCL_ClassName = a.ASMCL_ClassName
                                            }
                        ).ToList().ToArray();

                            if (class_Id.Length > 0 && class_Id.FirstOrDefault().ASMCL_Id != 0)
                            {
                                List<long> tempclass = new List<long>();
                                tempclass.Add(asmclidof);
                                tempclass.Add(Convert.ToInt64(class_Id.FirstOrDefault().ASMCL_Id));
                                if (tempclass.Count > 0)
                                {
                                    lessclass1 = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && tempclass.Contains(t.ASMCL_Id)).OrderByDescending(g => g.ASMCL_Order).ToList();
                                    if (lessclass1.Count() > 0)
                                    {
                                        stu.Class_list = lessclass1.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }

                stu.Class_list2 = termsList2.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stu;
        }
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
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf6, recno);
                }
                else
                {
                    recno = response.txnid;
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf6, recno);
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
        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {
            try
            {
                PaymentDetails dto = new PaymentDetails();
                StudentApplicationDTO stu = new StudentApplicationDTO();
                FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _db.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);
                response.order_id = payment.Attributes["order_id"];

                //single account added on 17/12/2019

                var accountvalidation = (from a in _db.Fee_PaymentGateway_Details
                                         where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();

                //single account added on 17/12/2019

                var fetchfmhotid = (from a in _db.Fee_M_Online_TransactionDMO
                                    where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Amount = a.FMOT_Amount,
                                        MI_Id = a.MI_Id,
                                        ASMAY_Id = a.ASMAY_ID,
                                        PASR_Id = a.PASR_Id,
                                        FMOT_Receipt_no = a.FMOT_Receipt_no
                                    }).ToArray();
                var fetchstudentdeatils = (from a in _db.StudentApplication
                                           where (a.pasr_id == Convert.ToInt64(fetchfmhotid[0].PASR_Id))
                                           select new FeeStudentTransactionDTO
                                           {
                                               pasR_MobileNo = a.PASR_MobileNo,
                                               pasR_emailId = a.PASR_emailId,
                                               ASMCL_ID = a.ASMCL_Id,
                                               PASR_RegistrationNo = a.PASR_RegistrationNo,
                                               PASR_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                               PASR_Id = a.pasr_id
                                           }).ToArray();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();
                Dictionary<String, object> transfers = new Dictionary<String, object>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    transfers.Clear();
                    var fetchaccountid = (from a in _feecontext.FeeAmountEntryDMO
                                          from b in _feecontext.FeeHeadDMO
                                          from c in _feecontext.Fee_OnlinePayment_Mapping
                                          from d in _feecontext.Fee_PaymentGateway_Details
                                          from e in _feecontext.PAYUDETAILS
                                          from f in _feecontext.feeYCC
                                          from g in _feecontext.feeYCCC
                                          where (a.FMH_Id == b.FMH_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == c.fmg_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && a.FMCC_Id == f.FMCC_Id && f.FYCC_Id == g.FYCC_Id && a.ASMAY_Id == f.ASMAY_Id && a.MI_Id == fetchfmhotid[0].MI_Id && a.ASMAY_Id == fetchfmhotid[0].ASMAY_Id && g.ASMCL_Id == fetchstudentdeatils[0].ASMCL_ID && b.FMH_Flag == "R" && e.IMPG_PGFlag == "RAZORPAY")
                                          select new FeeStudentTransactionDTO
                                          {
                                              FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                          }).Distinct().ToArray();

                    transfersnotes.Add("notes_1", fetchstudentdeatils[0].PASR_FirstName);
                    transfersnotes.Add("notes_2", fetchstudentdeatils[0].PASR_RegistrationNo);
                    transfersnotes.Add("notes_3", fetchstudentdeatils[0].PASR_Id);
                    transfersnotes.Add("notes_4", fetchstudentdeatils[0].pasR_MobileNo);
                    transfersnotes.Add("notes_5", fetchstudentdeatils[0].pasR_emailId);

                    transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                    transfers.Add("amount", (Convert.ToInt32(fetchfmhotid.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString());
                    transfers.Add("currency", "INR");
                    transfers.Add("notes", transfersnotes);

                    if (accountvalidation.Count() > 1)
                    {
                        Razorpay.Api.Transfer payment1 = client.Transfer.Create(transfers);
                        transferAPI trapay = new transferAPI();
                        if (payment1.Attributes["id"] != "")
                        {
                            trapay.transfer_id = payment1.Attributes["id"];
                            trapay.entity = payment1.Attributes["entity"];
                            trapay.source = payment1.Attributes["source"];
                            trapay.recipient = payment1.Attributes["recipient"];
                            trapay.amount = payment1.Attributes["amount"];
                            trapay.created_at = payment1.Attributes["created_at"];

                            FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                            fet.TRANSFER_ID = trapay.transfer_id;
                            fet.ENTITY = trapay.entity;
                            fet.SOURCE = trapay.source;
                            fet.RECIPIENT = trapay.recipient;
                            fet.AMOUNT = (Convert.ToInt32(trapay.amount) / 100).ToString();
                            fet.CREATED_AT = trapay.created_at;
                            fet.ORDER_ID = response.order_id;

                            fet.PAYMENT_ID = response.razorpay_payment_id;
                            fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                            fet.SETTLEMENT_FLAG = "0";

                            fet.CREATED_BY = indianTime;
                            fet.UPDATED_BY = indianTime;
                            _db.Add(fet);
                            var contactExists = _db.SaveChanges();
                            if (contactExists == 1)
                            {
                                response.status = "success";
                            }
                            else
                            {
                                response.status = "Failure";
                            }
                        }
                    }

                    else
                    {
                        FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                        fet.TRANSFER_ID = "";
                        fet.ENTITY = "";
                        fet.SOURCE = "";
                        fet.RECIPIENT = "";
                        fet.AMOUNT = (Convert.ToInt32(fetchfmhotid.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString();
                        fet.CREATED_AT = indianTime.ToString();
                        fet.ORDER_ID = response.order_id;

                        fet.PAYMENT_ID = response.razorpay_payment_id;
                        fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                        fet.SETTLEMENT_FLAG = "0";

                        fet.CREATED_BY = indianTime;
                        fet.UPDATED_BY = indianTime;
                        _db.Add(fet);
                        var contactExists = _db.SaveChanges();
                        if (contactExists == 1)
                        {
                            response.status = "success";
                        }
                        else
                        {
                            response.status = "Failure";
                        }
                    }
                }
                //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
                if (response.status == "success")
                {
                    stu.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    stu.PASR_MobileNo = fetchstudentdeatils[0].pasR_MobileNo;
                    stu.pasR_Id = Convert.ToInt64(fetchfmhotid[0].PASR_Id);
                    stu.PASR_emailId = fetchstudentdeatils[0].pasR_emailId;
                    stu.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    data.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    data.ASMCL_ID = Convert.ToInt64(fetchstudentdeatils[0].ASMCL_ID);
                    data.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    response.amount = fetchfmhotid.FirstOrDefault().FMA_Amount;
                    //response.responseupdate = fetchfmhotid.FirstOrDefault().FMOT_Receipt_no;
                    string recno = get_grp_reptno(data);
                    var confirmstatus = 0;
                    if (recno != "0")
                    {
                        response.responseupdate = recno;
                        confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", stu.MI_Id, data.ASMCL_ID, stu.pasR_Id, stu.ASMAY_Id, response.amount, response.order_id, response.razorpay_payment_id, response.udf6, recno);
                    }
                    else
                    {
                        response.responseupdate = response.txnid;
                        recno = response.txnid;
                        confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", stu.MI_Id, data.ASMCL_ID, stu.pasR_Id, stu.ASMAY_Id, response.amount, response.order_id, response.razorpay_payment_id, response.udf6, recno);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }
        //  aymentpart for AGGREGTOR
        public Array paymentPart(StudentApplicationDTO enq)
        {
            Payment pay = new Payment(_db);
            ProspectusDTO data = new ProspectusDTO();
            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;
            string orderId = "";
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

            // List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                //paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
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

                        cmd1.Parameters.Add(new SqlParameter("@paymentgateway",
                      SqlDbType.VarChar)
                        {
                            Value = enq.onlinepaygteway
                        });

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


                

                if (FeeAmountresult != null)
                {


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

                    if (enq.onlinepaygteway == "PAYU")
                    {
                        PaymentDetailsDto.trans_id = "PAYU" + enq.MI_Id + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond)).Trim();

                        PaymentDetailsDto.productinfo = payinfo;
                        PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                        PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                        PaymentDetailsDto.firstname = enq.PASR_FirstName;

                        PaymentDetailsDto.email = enq.PASR_emailId;

                        PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                        PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL;
                        PaymentDetailsDto.phone = enq.PASR_MobileNo;
                        PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                        PaymentDetailsDto.udf2 = Convert.ToString(enq.pasR_Id);
                        PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                        PaymentDetailsDto.udf4 = enq.ASMCL_Id.ToString();
                        PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                        PaymentDetailsDto.udf6 = enq.ASMCL_Id.ToString();
                        PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/StudentApplication/paymentresponse/";
                        PaymentDetailsDto.status = "success";
                        PaymentDetailsDto.service_provider = "payu_paisa";

                        PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                    }

                    else if (enq.onlinepaygteway == "PAYTM")
                    {
                        PaymentDetailsDto.trans_id = "CUST" + enq.MI_Id + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond)).Trim();

                        List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                        paymentdet = (from a in _feecontext.Fee_PaymentGateway_Details
                                      where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                                      select new FeeStudentTransactionDTO
                                      {
                                          merchantid = a.FPGD_MerchantId,
                                          merchantkey = a.FPGD_AuthorisationKey,
                                          merchanturl = a.FPGD_URL
                                      }
                   ).ToList();

                        List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                        PAYMENTPARAMDETAILS = (from a in _feecontext.PAYUDETAILS
                                               where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   IMPG_IndustryType = a.IMPG_IndustryType,
                                                   IMPG_Website = a.IMPG_Website
                                               }
                        ).ToList();

                        // For Testing Purpose
                        totpayableamount = Convert.ToInt32(totpayableamount);
                        // totpayableamount = 5000;

                        PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                        Dictionary<string, string> parameters = new Dictionary<string, string>();

                        parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                        parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
                        parameters.Add("CUST_ID", enq.MI_Id.ToString());
                        parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
                        parameters.Add("CHANNEL_ID", "WEB");
                        parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
                        parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);
                        parameters.Add("MOBILE_NO", enq.PASR_MobileNo.ToString());
                        parameters.Add("EMAIL", enq.PASR_emailId.Trim());
                        parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + enq.ASMCL_Id.ToString() + "_" + enq.ASMCL_Id.ToString().Trim() + "_" + enq.ASMAY_Id.ToString() + "_" + Convert.ToString(enq.pasR_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + enq.PASR_emailId.Trim() + "_" + enq.PASR_MobileNo.ToString() + "_" + totpayableamount.ToString());
                        string url = paymentdet.FirstOrDefault().merchanturl;
                        //parameters.Add("REQUEST_TYPE", "DEFAULT");
                        parameters.Add("CALLBACK_URL", "http://localhost:57606/api/StudentApplication/paymentresponsepaytm/");
                        //parameters.Add("CALLBACK_URL", "https://secure.paytm.in/oltp-web/invoiceResponse");
                        // parameters.Add("CALLBACK_URL", "https://securegw-stage.paytm.in/theia/processTransaction");

                        string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

                        aa.MID = paymentdet.FirstOrDefault().merchantid;
                        aa.ORDER_ID = PaymentDetailsDto.trans_id;
                        aa.CUST_ID = enq.MI_Id.ToString();
                        aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
                        aa.CHANNEL_ID = "WEB";
                        aa.INDUSTRY_TYPE_ID = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType;
                        aa.WEBSITE = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website;
                        aa.MOBILE_NO = Convert.ToInt64(enq.PASR_MobileNo);
                        aa.EMAIL = enq.PASR_emailId;
                        aa.payu_URL = url;
                        aa.CHECKSUMHASH = checksum;
                        aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + enq.ASMCL_Id.ToString() + "_" + enq.ASMCL_Id.ToString().Trim() + "_" + enq.ASMAY_Id.ToString() + "_" + Convert.ToString(enq.pasR_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + enq.PASR_emailId.Trim() + "_" + enq.PASR_MobileNo + "_" + totpayableamount.ToString();

                        List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
                        paydet.Add(aa);

                        PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();
                    }
                    else if (enq.onlinepaygteway == "RAZORPAY")
                    {

                        List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                        instidet = _feecontext.MOBILE_INSTITUTION.Where(t => t.MI_ID == enq.MI_Id).ToList();


                        List<PaymentDetails> paydet = new List<PaymentDetails>();

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetailsrazorpay = new List<Fee_PaymentGateway_DetailsDMO>();

                        paymentdetailsrazorpay = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                        List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                        PAYMENTPARAMDETAILS = (from a in _feecontext.PAYUDETAILS
                                               where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   IMPG_IndustryType = a.IMPG_IndustryType,
                                                   IMPG_Website = a.IMPG_Website
                                               }
                        ).ToList();

                        // For Testing Purpose
                        totpayableamount = Convert.ToInt32(totpayableamount);



                        Dictionary<string, object> input = new Dictionary<string, object>();
                        //input.Add("amount", 1 * 100);
                        input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
                        input.Add("currency", "INR");
                        input.Add("receipt", "");
                        input.Add("payment_capture", 1);

                        string key = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        string secret = paymentdetailsrazorpay.FirstOrDefault().FPGD_AuthorisationKey;

                        RazorpayClient client = new RazorpayClient(key, secret);

                        Razorpay.Api.Order order = client.Order.Create(input);
                        orderId = order["id"].ToString();

                        PaymentDetails aa = new PaymentDetails();

                        //added for change in receiptno
                        PaymentDetailsDto.trans_id = orderId;

                        aa.trans_id = orderId;
                        aa.IVRMOP_MERCHANT_KEY = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        aa.FMA_Amount = totpayableamount;
                        aa.splitpayinformation = payinfo;

                        aa.firstname = enq.PASR_FirstName;
                        aa.mobile = enq.PASR_MobileNo.ToString();
                        aa.email = enq.PASR_emailId.ToString();
                        aa.PASR_RegistrationNo = enq.PASR_RegistrationNo.ToString();
                        if (enq.PASR_PerCity != null && enq.PASR_PerCity != "")
                        {
                            aa.stuaddress = enq.PASR_PerCity.ToString();
                        }
                        else
                        {
                            aa.stuaddress = enq.PASR_FatherPresentAddress.ToString();
                        }

                        aa.institutioname = instidet[0].INSTITUTION_NAME;
                        aa.institulogo = instidet[0].INSTITUTION_LOGO;
                        aa.PASR_ID = enq.pasR_Id;
                        paydet.Add(aa);
                        PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();
                    }

                    else if (enq.onlinepaygteway == "EASEBUZZ")
                    {

                        List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                        instidet = _feecontext.MOBILE_INSTITUTION.Where(t => t.MI_ID == enq.MI_Id).ToList();


                        List<PaymentDetails> paydet = new List<PaymentDetails>();

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetailsrazorpay = new List<Fee_PaymentGateway_DetailsDMO>();

                        paymentdetailsrazorpay = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                        List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                        PAYMENTPARAMDETAILS = (from a in _feecontext.PAYUDETAILS
                                               where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   IMPG_IndustryType = a.IMPG_IndustryType,
                                                   IMPG_Website = a.IMPG_Website
                                               }
                        ).ToList();

                        // For Testing Purpose
                        totpayableamount = Convert.ToInt32(totpayableamount);

                         

                        //Dictionary<string, object> input = new Dictionary<string, object>();
                        ////input.Add("amount", 1 * 100);
                        //input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
                        //input.Add("currency", "INR");
                        //input.Add("receipt", "");
                        //input.Add("payment_capture", 1);

                        //string key = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        //string secret = paymentdetailsrazorpay.FirstOrDefault().FPGD_AuthorisationKey;

                        Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();

                        if (result.Count > 0)
                        {

                            for (var j = 0; j < result.Count; j++)
                            {

                                transfersnotescash.Add(result[j].merchantId.ToString(), Convert.ToInt64(result[j].value));

                            }


                        }
                        var myContent = JsonConvert.SerializeObject(transfersnotescash);
                        String postData = myContent;
                        //String postData = res;
                        string split_payments = postData;

                        string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                        string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                        string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                        string env = "prod";
                        Easebuzz t1 = new Easebuzz(secret, key, env);
                        //split_payments = "";
                        // sub_merchant_id = "";
                        string surl = "http://localhost:57606/api/StudentApplication/paymentresponseeasybuzz/";
                        string furl = "http://localhost:57606/api/StudentApplication/paymentresponseeasybuzz/";
                        PaymentDetailsDto.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

                        string strForm = t1.initiatePaymentAPI((totpayableamount).ToString(), enq.PASR_FirstName,  enq.PASR_emailId.ToString(), enq.PASR_MobileNo.ToString(), Convert.ToString(enq.pasR_Id), 
                            surl, furl, PaymentDetailsDto.trans_id, Convert.ToString(enq.ASMAY_Id), Convert.ToString(enq.pasR_Id), enq.MI_Id.ToString(), enq.ASMCL_Id.ToString(), enq.ASMAY_Id.ToString(), "", "", "", "", "", "", split_payments, sub_merchant_id);

                        enq.multiplegroups = strForm;
                        PaymentDetails aa = new PaymentDetails();

                        //added for change in receiptno
                      
                        aa.trans_id = PaymentDetailsDto.trans_id;
                        aa.IVRMOP_MERCHANT_KEY = paymentdetailsrazorpay.FirstOrDefault().FPGD_SaltKey;
                        aa.FMA_Amount = totpayableamount;
                        aa.splitpayinformation = payinfo;

                        aa.firstname = enq.PASR_FirstName;
                        aa.mobile = enq.PASR_MobileNo.ToString();
                        aa.email = enq.PASR_emailId.ToString();
                        aa.PASR_RegistrationNo = enq.PASR_RegistrationNo.ToString();
                        if (enq.PASR_PerCity != null && enq.PASR_PerCity != "")
                        {
                            aa.stuaddress = enq.PASR_PerCity.ToString();
                        }
                        else
                        {
                            aa.stuaddress = enq.PASR_FatherPresentAddress.ToString();
                        }

                        aa.institutioname = instidet[0].INSTITUTION_NAME;
                        aa.institulogo = instidet[0].INSTITUTION_LOGO;
                        aa.PASR_ID = enq.pasR_Id;
                        paydet.Add(aa);
                        PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();
                    }


                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();
                   

                    onlinemtrans.FMOT_Trans_Id = PaymentDetailsDto.trans_id;
                    onlinemtrans.FMOT_Amount = totpayableamount;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = enq.pasR_Id;
                    onlinemtrans.AMST_Id = 0;

                    onlinemtrans.FMOT_Receipt_no = PaymentDetailsDto.trans_id;
                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.MI_Id = enq.MI_Id;
                    onlinemtrans.ASMAY_ID = enq.ASMAY_Id;
                    onlinemtrans.FMOT_PayGatewayType = enq.onlinepaygteway;
                    _feecontext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);
                    _feecontext.SaveChanges();

                    //FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                    //feepaydet.MI_Id = enq.MI_Id;
                    //feepaydet.ASMAY_ID = enq.ASMAY_Id;

                    //feepaydet.FTCU_Id = 1;
                    //feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                    //feepaydet.FYP_Bank_Name = "";
                    //feepaydet.FYP_Bank_Or_Cash = "O";
                    //feepaydet.FYP_DD_Cheque_No = "";
                    //feepaydet.FYP_DD_Cheque_Date = indianTime;
                    //feepaydet.FYP_Date = indianTime;
                    //feepaydet.FYP_Tot_Amount = totpayableamount;
                    //feepaydet.FYP_Tot_Waived_Amt = 0;
                    //feepaydet.FYP_Tot_Fine_Amt = 0;
                    //feepaydet.FYP_Tot_Concession_Amt = 0;
                    //feepaydet.FYP_Remarks = "Preadmission Registration Payment";
                    //feepaydet.FYP_Chq_Bounce = "CL";
                    //feepaydet.DOE = indianTime;
                    //feepaydet.CreatedDate = indianTime;
                    //feepaydet.UpdatedDate = indianTime;
                    //feepaydet.user_id = enq.Id;
                    //feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                    //feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                    //feepaydet.FYP_PaymentReference_Id = "";
                    //feepaydet.FYP_PayGatewayType = enq.onlinepaygteway;
                    //_feecontext.FeePaymentDetailsDMO.Add(feepaydet);
                    //_feecontext.SaveChanges();

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
        public static String generateCheckSum(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            try
            {
                foreach (string key in parameters.Keys)
                {
                    // below code snippet is mandatory, so that no one can use your checksumgeneration url for other purpose .
                    if (parameters[key].Trim().ToUpper().Contains("REFUND") || parameters[key].Trim().Contains("|"))
                    {
                        parameter.Add(key.Trim(), "");
                    }

                    //if (parameters.ContainsKey(key.Trim()))
                    //{
                    //    parameters[key.Trim()] = parameters[key].Trim();
                    //}
                    else
                    {
                        parameter.Add(key.Trim(), parameters[key].Trim());
                    }
                }

                String checkSumParams = SecurityUtils.createCheckSumString(parameter);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);

                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);

                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }
        }
        public static String generateCheckSumForRefund(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }
        public static String generateCheckSumByJson(String masterKey, string json)
        {
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = json;
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;

                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                hashedCheckSum += salt;

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }
        public static Boolean verifyCheckSumByjson(String masterKey, string json, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = json;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }
        public static Boolean verifyCheckSum(String masterKey, Dictionary<String, String> parameters, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }
        private static void validateGenerateCheckSumInput(String masterKey)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

        }
        private static void validateVerifyCheckSumInput(String masterKey, String checkSum)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

            if (checkSum == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "checkSum");
            }
        }
        public static string Encrypt(String CardDetails, String masterKey)
        {
            return Crypto.Encrypt(CardDetails, masterKey);
        }
        public static String Decrypt(String carddetails, String masterKey)
        {
            return Crypto.Decrypt(carddetails, masterKey);
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

            var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
            stu.ASMAY_Id = Acdemic_preadmission;
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
                        stu.PASHD_Id = pge.PASHD_Id;
                        stu.returnval = true;
                    }
                    else
                    {
                        stu.returnval = false;
                    }

                    if (stu.StudentVaccines.Length > 0)
                    {
                        studentvaccines(stu);
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
                        stu.PASHD_Id = pge.PASHD_Id;
                        stu.returnval = true;
                    }
                    else
                    {
                        stu.returnval = false;
                    }

                    if (stu.StudentVaccines.Length > 0)
                    {
                        studentvaccines(stu);
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
        public void studentvaccines(StudentHelthcertificateDTO stu)
        {
            try
            {
                //List<long> temparr = new List<long>();


                if (stu.StudentVaccines.Count() > 0)
                {

                    Array siblingNoresultremove = _StudentApplicationContext.PA_Student_Vaccines.Where(t => t.PASHD_Id == stu.PASHD_Id).ToArray();
                    foreach (PA_Student_Vaccines ph1 in siblingNoresultremove)
                    {
                        _StudentApplicationContext.Remove(ph1);
                    }


                    //add & update Siblings details

                    foreach (StudentVaccines mob in stu.StudentVaccines)
                    {
                        if (mob.PAMVA_Vaccines != null)
                        {

                            PA_Student_Vaccines sibling = new PA_Student_Vaccines();

                            sibling.PASHD_Id = stu.PASHD_Id;
                            sibling.PAMVA_Id = mob.PAMVA_Id;
                            sibling.PASVA_YesNoNAFlg = mob.PAMVA_YesNoFlg;
                            sibling.PASVA_ActiveFlg = true;
                            sibling.PASVA_CreatedBy = stu.Id;
                            sibling.PASVA_UpdatedBy = stu.Id;
                            sibling.PASVA_Date = DateTime.Now;
                            sibling.UpdatedDate = DateTime.Now;
                            sibling.CreatedDate = DateTime.Now;

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
        public async Task<StudentHelthcertificateDTO> getstudata(StudentHelthcertificateDTO stu)
        {
            try
            {
                var Acdemic_preadmission = _StudentApplicationContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == stu.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                stu.ASMAY_Id = Acdemic_preadmission;
                List<AdmissionClass> allclass = new List<AdmissionClass>();

                var rolelist = _StudentApplicationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.roleId).ToList();
                //if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                //|| rolelist[0].IVRMRT_Role.Equals("PRINCIPAL", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                if (!rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                {

                    List<StudentHelthcertificateDMO> allRegStudent = new List<StudentHelthcertificateDMO>();
                    allRegStudent = await _StudentApplicationContext.StudentHelthcertificate.Where(d => d.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                    stu.studentDetailsTEmp = allRegStudent.ToArray();

                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                    stu.studetailslist = allclass.ToArray();

                    List<long> temparr = new List<long>();
                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudent[i].PASR_Id);
                    }

                    stu.studentDetails = (from a in _StudentApplicationContext.Enq
                                          where (!temparr.Contains(a.pasr_id) && a.MI_Id == stu.MI_Id && a.ASMAY_Id == stu.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false)
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

                    allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_PreadmFlag == 1).ToList();
                    stu.studetailslist = allclass.ToArray();

                    List<long> temparr = new List<long>();

                    for (int i = 0; i < stu.studentDetailsTEmp.Length; i++)
                    {
                        temparr.Add(allRegStudent[i].PASR_Id);
                    }

                    stu.studentDetails = (from a in _StudentApplicationContext.Enq
                                          where (!temparr.Contains(a.pasr_id) && a.Id == stu.Id && a.ASMAY_Id == stu.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false)
                                          select new StudentHelthcertificateDTO
                                          {
                                              PASR_Id = a.pasr_id,
                                              PASR_FirstName = a.PASR_FirstName,
                                              PASR_MiddleName = a.PASR_MiddleName,
                                              PASR_LastName = a.PASR_LastName
                                          }
                     ).ToList().ToArray();
                }


                //if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                //  || rolelist[0].IVRMRT_Role.Equals("PRINCIPAL", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                //{
                if (!rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                { 
                    stu.studenthelthDetails = (from a in _StudentApplicationContext.Enq
                                               from b in _StudentApplicationContext.StudentHelthcertificate
                                               where (a.pasr_id == b.PASR_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.ASMAY_Id == stu.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false)
                                               select new StudentHelthcertificateDTO
                                               {
                                                   PASHD_Id = b.PASHD_Id,
                                                   PASR_FirstName = a.PASR_FirstName,
                                                   PASR_MiddleName = a.PASR_MiddleName,
                                                   PASR_LastName = a.PASR_LastName,
                                                   PASR_EMAIL = a.PASR_emailId,
                                                   PASR_Id = a.pasr_id,

                                               }
                   ).OrderBy(t => t.pasr_id).ToList().ToArray();

                    stu.countrole = true;
                }
                else
                {
                    stu.studenthelthDetails = (from a in _StudentApplicationContext.Enq
                                               from b in _StudentApplicationContext.StudentHelthcertificate
                                               where (a.pasr_id == b.PASR_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.ASMAY_Id == stu.ASMAY_Id && a.PASR_Adm_Confirm_Flag == false && a.Id == stu.Id)
                                               select new StudentHelthcertificateDTO
                                               {
                                                   PASHD_Id = b.PASHD_Id,
                                                   PASR_FirstName = a.PASR_FirstName,
                                                   PASR_MiddleName = a.PASR_MiddleName,
                                                   PASR_LastName = a.PASR_LastName,
                                                   PASR_EMAIL = a.PASR_emailId,
                                                   PASR_Id = a.pasr_id
                                               }
                  ).OrderBy(t => t.pasr_id).ToList().ToArray();
                    stu.countrole = false;
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

                //if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                //  || rolelist[0].IVRMRT_Role.Equals("PRINCIPAL", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Chairman", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                //{
                if (!rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
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

                stu.vaccines = (from a in _StudentApplicationContext.PA_Student_Vaccines
                                from c in _StudentApplicationContext.PA_Master_Vaccines
                                from b in _StudentApplicationContext.StudentHelthcertificate
                                where (a.PASHD_Id == b.PASHD_Id && a.PAMVA_Id == c.PAMVA_Id && c.MI_Id == stu.MI_Id && b.MI_Id == stu.MI_Id && b.PASHD_Id.Equals(stu.PASHD_Id))
                                select new StudentHelthcertificateDTO
                                {
                                    PAMVA_Id = c.PAMVA_Id,
                                    PAMVA_Vaccines = c.PAMVA_Vaccines,
                                    PAMVA_YesNoFlg = a.PASVA_YesNoNAFlg
                                }
                     ).ToList().ToArray();

                //List<PA_Student_Vaccines> studentvaacinlist = new List<PA_Student_Vaccines>();
                //studentvaacinlist = _StudentApplicationContext.PA_Student_Vaccines.Where(d => d.PASHD_Id.Equals(stu.PASHD_Id)).ToList();
                //stu.vaccines = studentvaacinlist.ToArray();

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
                                           PASHD_AllergyFlg = b.PASHD_AllergyFlg,
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
                                           PASHD_WhoopingDate = b.PASHD_WhoopingDate,
                                           PASHD_CronicDesease = b.PASHD_CronicDesease,
                                           PASHD_MEDetails = b.PASHD_MEDetails,
                                           PASHD_MEContactNo = b.PASHD_MEContactNo
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
                //}
                //else
                //{
                //    data.FYP_Receipt_No = "0";
                //}

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
        public PaymentDetails.PAYTM paytmresponse(PaymentDetails.PAYTM response)
        {
            PaymentDetails dto = new PaymentDetails();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "TXN_SUCCESS")
            {

                string[] tokens = response.MERC_UNQ_REF.Split('_');

                dto.udf3 = tokens[6];
                dto.udf2 = tokens[4];
                dto.udf5 = tokens[0];
                dto.amount = Convert.ToDecimal(tokens[9]);

                dto.udf4 = tokens[2];
                dto.txnid = tokens[5];
                dto.email = tokens[7];
                dto.mobile = tokens[8];
                data.PASR_Id = Convert.ToInt64(tokens[4]);
                string paytmtranid = response.txnid.ToString();


                data.MI_Id = Convert.ToInt64(dto.udf3);
                data.ASMCL_ID = Convert.ToInt64(dto.udf4);
                data.ASMAY_Id = Convert.ToInt64(dto.udf5);

                string recno = get_grp_reptno(data);

                var confirmstatus = 0;

                if (recno != "0")
                {
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", dto.udf3, dto.udf4, dto.udf2, dto.udf5, dto.amount, dto.txnid, paytmtranid, dto.udf6, recno);
                }
                else
                {
                    recno = response.txnid;
                    confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", dto.udf3, dto.udf4, dto.udf2, dto.udf5, dto.amount, dto.txnid, paytmtranid, dto.udf6, recno);
                }



                if (confirmstatus > 0)
                {

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(data.MI_Id) && d.ASMAY_Id.Equals(data.ASMAY_Id)).ToList();

                    if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    {
                        Email Email = new Email(_db);

                        Email.sendmail(data.MI_Id, dto.email, "STUDENT_REGISTRATION", data.PASR_Id);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    {
                        SMS sms = new SMS(_db);
                        sms.sendSms(data.MI_Id, Convert.ToInt64(dto.mobile), "STUDENT_REGISTRATION", data.PASR_Id);

                    }
                }
            }
            else
            {
                dto.status = response.status;
            }

            return response;
        }

        public PaymentDetails.easybuzz getpaymentresponseeasybuzz(PaymentDetails.easybuzz response)
        {
            PaymentDetails dto = new PaymentDetails();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "success")
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _feecontext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == Convert.ToInt64(response.UDF3) && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();

                string txnid = response.txnid.ToString();
                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                string paymentref = "";
                Easebuzz t = new Easebuzz(secret, key, env);
                // Easebuzz t = new Easebuzz();
                var res = t.transactionAPI(txnid, response.amount, response.email, response.phone);

                JObject joResponse1 = JObject.Parse(res);
                
                paymentref = joResponse1["msg"]["easepayid"].ToString();



                data.MI_Id = Convert.ToInt64(response.UDF3);
                data.ASMCL_ID = Convert.ToInt64(response.UDF4);
                data.ASMAY_Id = Convert.ToInt64(response.UDF5);

                string recno = get_grp_reptno(data);

                var confirmstatus = 0;

                if (recno != "0")
                {
                    //confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.UDF3,response.UDF4, response.UDF2, response.UDF5, Convert.ToInt64(response.amount), response.txnid, paymentref, response.UDF5, recno);
                }
                else
                {
                    recno = response.txnid;
                    // confirmstatus = _ProspectusContext.Database.ExecuteSqlCommand("Preadmission_Application_online @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", response.UDF3, response.UDF4, response.UDF2, response.UDF5, response.amount, response.txnid, paymentref, response.UDF5, recno);
                }



                if (confirmstatus > 0)
                {

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(data.MI_Id) && d.ASMAY_Id.Equals(data.ASMAY_Id)).ToList();

                    if (mstConfig.FirstOrDefault().ISPAC_ApplMailFlag == 1)
                    {
                        Email Email = new Email(_db);

                        Email.sendmail(data.MI_Id, dto.email, "STUDENT_REGISTRATION", data.PASR_Id);
                    }

                    if (mstConfig.FirstOrDefault().ISPAC_ApplSMSFlag == 1)
                    {
                        SMS sms = new SMS(_db);
                        sms.sendSms(data.MI_Id, Convert.ToInt64(dto.mobile), "STUDENT_REGISTRATION", data.PASR_Id);

                    }
                }
            }
            else
            {
                dto.status = response.status;
            }

            return response;
        }

        public FeeStudentTransactionDTO Razorpaypaymentsettlementresponse(FeeStudentTransactionDTO response)
        {
            try
            {
                PaymentDetails response1 = new PaymentDetails();
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                try
                {
                    Dictionary<string, object> input = new Dictionary<string, object>();
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    Int32 unixTimestampstart = 0;
                    Int32 unixTimestampend = 0;
                    if (response.date != null && response.date != "" && response.date != "0" && response.date != "1")
                    {
                        //CURRENT DAY - 1 
                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(response.date))).AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(response.date) - 1)).AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }
                    else
                    {
                        //CURRENT DAY
                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }

                    // supported option filters (from, to, count, skip)

                    input.Add("from", unixTimestampstart.ToString());
                    input.Add("to", unixTimestampend.ToString());
                    input.Add("count", 100);
                    //input.Add("skip", 0);

                    List<Order> orders = client.Order.All(input);
                    string id = "";
                    foreach (var x in orders)
                    {
                        id = x.Attributes["id"];
                        if (x.Attributes["status"] == "paid")
                        {
                            var registrationpayment = (from a in _feecontext.Fee_M_Online_TransactionDMO
                                                       where (a.MI_Id == response.MI_Id && a.FMOT_Trans_Id == id && a.AMST_Id == 0)
                                                       select new FeeStudentTransactionDTO
                                                       {
                                                           order_id = a.FMOT_Trans_Id
                                                       }
                         ).ToList();

                            if (registrationpayment.Count > 0)
                            {
                                var pendingpayments = (from a in _feecontext.FeePaymentDetailsDMO
                                                       from b in _feecontext.FEE_RAZOR_TRANSFER_API_DETAILS
                                                       from c in _feecontext.FeeTransactionPaymentDMO
                                                       from d in _feecontext.FeeAmountEntryDMO
                                                       from e in _feecontext.FeeHeadDMO
                                                       where (a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && a.MI_Id == response.MI_Id && b.ORDER_ID == id && a.fyp_transaction_id == id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && a.FYP_PayGatewayType == "RAZORPAY" && e.FMH_Flag == "R")
                                                       select new FeeStudentTransactionDTO
                                                       {
                                                           order_id = a.fyp_transaction_id
                                                       }
                             ).ToList();

                                if (pendingpayments.Count == 0)
                                {
                                    response1.order_id = id;
                                    response1.IVRMOP_MIID = response.MI_Id;

                                    //FETCH PAYMENT BASED ON ORDER-ID

                                    string method = "GET";
                                    string url = "https://api.razorpay.com/v1/orders/ID/payments";
                                    url = url.Replace("ID", id);
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                    request.Method = method.ToString();
                                    request.ContentLength = 0;
                                    request.ContentType = "application/json";

                                    string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                                    request.UserAgent = "razorpay-dot-net/" + userAgent;

                                    string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                                    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                        Encoding.UTF8.GetBytes(authString));

                                    System.Net.WebResponse resp = request.GetResponseAsync().Result;
                                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                                    string s = sr.ReadToEnd().Trim();
                                    JObject joResponse1 = JObject.Parse(s);
                                    JArray array1 = (JArray)joResponse1["items"];
                                    foreach (JObject root1 in array1)
                                    {
                                        if ((String)root1["status"] == "captured")
                                        {
                                            response1.razorpay_payment_id = (String)root1["id"];
                                            razorgetpaymentresponse(response1);
                                        }
                                    }
                                    //FETCH PAYMENT BASED ON ORDER-ID
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        private static string getAppDetailsUa()
        {
            List<Dictionary<string, string>> appsDetails = RazorpayClient.AppsDetails;

            string appsDetailsUa = string.Empty;

            foreach (Dictionary<string, string> appsDetail in appsDetails)
            {
                string appUa = string.Empty;

                if (appsDetail.ContainsKey("title"))
                {
                    appUa = appsDetail["title"];

                    if (appsDetail.ContainsKey("version"))
                    {
                        appUa += appsDetail["version"];
                    }
                }

                appsDetailsUa += appUa;
            }

            return appsDetailsUa;
        }
        public PaymentDetails paytmresponsse(PaymentDetails data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                indianTime = indianTime.AddDays(-data.date);

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _feecontext.Fee_PaymentGateway_Details
                              where (a.MI_Id == data.MI_Id && a.FPGD_PGName == "PAYTM")
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<FeeStudentTransactionDTO> pendingtransactions = new List<FeeStudentTransactionDTO>();

                pendingtransactions = (from a in _feecontext.Fee_M_Online_TransactionDMO
                                       where (a.MI_Id == data.MI_Id && a.FMOT_PayGatewayType == "PAYTM" && a.AMST_Id == 0 && Convert.ToDateTime(a.FMOT_Date.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd")))
                                       select new FeeStudentTransactionDTO
                                       {
                                           trans_id = a.FMOT_Trans_Id,
                                       }
     ).Distinct().ToList();

                if (pendingtransactions.Count > 0)
                {
                    for (int p = 0; p < pendingtransactions.Count; p++)
                    {
                        PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();
                        Dictionary<string, string> parameters = new Dictionary<string, string>();

                        parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                        parameters.Add("ORDER_ID", pendingtransactions[p].trans_id);

                        string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);
                        string url = "https://securegw.paytm.in/order/status";

                        Dictionary<String, String> paytmParams = new Dictionary<String, String>();
                        paytmParams.Add("MID", paymentdet.FirstOrDefault().merchantid);
                        paytmParams.Add("ORDER_ID", pendingtransactions[p].trans_id);
                        paytmParams.Add("CHECKSUMHASH", checksum);

                        var myContent = JsonConvert.SerializeObject(paytmParams);

                        String postData = myContent;
                        HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                        connection.ContentType = "application/json";

                        connection.Method = "POST";

                        using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                        {
                            requestWriter.Write(postData);
                        }
                        string responseData = string.Empty;

                        using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                        {
                            responseData = responseReader.ReadToEnd();
                        }

                        JObject joResponse1 = JObject.Parse(responseData);
                        string txnstatus = (string)joResponse1["STATUS"];

                        if (txnstatus == "TXN_SUCCESS")
                        {
                            var txnstatusexists = (from a in _feecontext.FeePaymentDetailsDMO
                                                   where (a.MI_Id == data.MI_Id && a.fyp_transaction_id == (string)joResponse1["ORDERID"])
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       fyp_transaction_id = a.fyp_transaction_id,
                                                   }
                                       ).ToList();
                            if (txnstatusexists.Count == 0)
                            {
                                PaymentDetails.PAYTM response = new PaymentDetails.PAYTM();
                                response.MERC_UNQ_REF = (string)joResponse1["MERC_UNQ_REF"];
                                response.txnid = (string)joResponse1["TXNID"];
                                response.BANKTXNID = (string)joResponse1["BANKTXNID"];
                                paytmresponse(response);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //added by roopa

        // save and update Refference details
        public StudentApplicationDTO RefferenceDetailsAddUpdate(StudentApplicationDTO refdetadd)
        {
            string str = "0";
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //add & update Reference details
                if (refdetadd.SelectedRefrenceDetails != null)
                {
                    foreach (PA_StudentReferenceDTO refer in refdetadd.SelectedRefrenceDetails)
                    {
                        str = str + "," + refer.PAMR_Id;
                    }
                    List<PA_StudentReferenceDTO> result = new List<PA_StudentReferenceDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PA_getReference";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = refdetadd.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@PASR_Id", SqlDbType.BigInt) { Value = refdetadd.pasR_Id });
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
                                    result.Add(new PA_StudentReferenceDTO
                                    {
                                        PAMR_Id = Convert.ToInt64(dataReader["PAMR_Id"])
                                    });
                                    refdetadd.referenceIds = result.ToArray();
                                }
                            }
                            if (refdetadd.referenceIds != null)
                            {
                                foreach (PA_StudentReferenceDTO act1 in refdetadd.referenceIds)
                                {
                                    var Referenceresult = _StudentApplicationContext.PA_School_Application_Reference.Where(t => t.PAMR_Id == act1.PAMR_Id
                                     && t.PASR_Id == refdetadd.pasR_Id).ToList();
                                    if (Referenceresult.Any())
                                    {
                                        _StudentApplicationContext.Remove(Referenceresult.ElementAt(0));
                                        _StudentApplicationContext.SaveChanges();
                                    }
                                }
                            }

                            foreach (PA_StudentReferenceDTO mob in refdetadd.SelectedRefrenceDetails)
                            {
                               // mob.PASR_Id = refdetadd.pasR_Id;
                                // mob.MI_Id = refdetadd.MI_Id;
                                var Activityresult1 = _StudentApplicationContext.PA_School_Application_Reference.Where(t => t.PAMR_Id == mob.PAMR_Id && t.PASR_Id == refdetadd.pasR_Id).ToList();
                                if (Activityresult1.Count == 0)
                                {
                                    PA_School_Application_Reference Reference = new PA_School_Application_Reference();
                                    Reference.PASR_Id = refdetadd.pasR_Id;
                                    Reference.PAMR_Id = mob.PAMR_Id;
                                    Reference.PASAR_ActiveFlg = true;
                                    Reference.PASAR_CreatedDate = indiantime0;
                                    Reference.PASAR_UpdatedDate = indiantime0;
                                    Reference.PASAR_CreatedBy = 0;
                                    Reference.PASAR_UpdatedBy = 0;
                                    _StudentApplicationContext.Add(Reference);
                                   
                                }
                            }
                            _StudentApplicationContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            //_log.LogInformation("Student Reference  error");
                            //_log.LogDebug(ex.Message);
                            _StudentApplicationContext.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception e)
            {

                //_log.LogInformation("Student Reference  error");
                //_log.LogDebug(e.Message);
            }
            return refdetadd;
        }

        // save and update Source details
        public StudentApplicationDTO SourceDetailsAddUpdate(StudentApplicationDTO sourcedetadd)
        {
            string str = "0";
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                //add & update Source details
                if (sourcedetadd.SelectedSourceDetails != null)
                {
                    foreach (PA_StudentSourceDTO src in sourcedetadd.SelectedSourceDetails)
                    {
                        str = str + "," + src.PAMS_Id;
                    }
                    List<PA_StudentSourceDTO> result = new List<PA_StudentSourceDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PA_getSource";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = sourcedetadd.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@PASR_Id", SqlDbType.BigInt) { Value = sourcedetadd.pasR_Id });
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
                                    result.Add(new PA_StudentSourceDTO
                                    {
                                        PAMS_Id = Convert.ToInt64(dataReader["PAMS_Id"])
                                    });
                                    sourcedetadd.sourceIds = result.ToArray();
                                }
                            }
                            if (sourcedetadd.sourceIds != null)
                            {
                                foreach (PA_StudentSourceDTO source in sourcedetadd.sourceIds)
                                {
                                    var Sourceresult = _StudentApplicationContext.PA_School_Application_Source.Where(t => t.PAMS_Id == source.PAMS_Id && t.PASR_Id == sourcedetadd.pasR_Id).ToList();
                                    if (Sourceresult.Any())
                                    {
                                        _StudentApplicationContext.Remove(Sourceresult.ElementAt(0));
                                    }
                                }
                                _StudentApplicationContext.SaveChanges();
                            }
                            foreach (PA_StudentSourceDTO mob in sourcedetadd.SelectedSourceDetails)
                            {
                                //mob.PASR_Id = sourcedetadd.pasR_Id;
                                // mob.MI_Id = sourcedetadd.MI_Id;
                                var Sourceresult1 = _StudentApplicationContext.PA_School_Application_Source.Where(t => t.PAMS_Id == mob.PAMS_Id && t.PASR_Id == sourcedetadd.pasR_Id).ToList();
                                if (Sourceresult1.Count == 0)
                                {
                                    PA_School_Application_Source Source = new PA_School_Application_Source();
                                    Source.PASR_Id = sourcedetadd.pasR_Id;
                                    Source.PAMS_Id = mob.PAMS_Id;
                                    Source.PASAS_ActiveFlg = true;
                                    Source.PASAS_CreatedDate = indiantime0;
                                    Source.PASAS_CreatedDate = indiantime0;
                                    Source.PASAS_UpdatedDate = indiantime0;
                                    Source.PASAS_CreatedBy =0;
                                    Source.PASAS_UpdatedBy = 0;
                                    _StudentApplicationContext.Add(Source);
                                }
                            }

                            _StudentApplicationContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                            _StudentApplicationContext.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return sourcedetadd;
        }
    }
}



