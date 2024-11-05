using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class collegeadmissionImportImpl : Interface.collegeadmissionImportInterface
    {
        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        ILogger<collegeadmissionImportImpl> _acdimpl;
        private readonly ClgAdmissionContext _AdmissionFormContext;

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;

        private readonly ILogger _log;

        public collegeadmissionImportImpl(ILogger<collegeadmissionImportImpl> acdimpl, DomainModelMsSqlServerContext DomainModelMsSqlServerContext, ClgAdmissionContext AdmissionFormContext, DomainModelMsSqlServerContext db, ILogger<collegeadmissionImportImpl> loggerFactor, UserManager<ApplicationUser> UserManager)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
            _acdimpl = acdimpl;


            _AdmissionFormContext = AdmissionFormContext;
            _db = db;
            _log = loggerFactor;
            _UserManager = UserManager;
        }
        public async Task<string> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            string resp = "";
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
                        var id2 = _db.MasterRoleType.Where(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.FirstOrDefault().IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);
                        _db.SaveChanges();

                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        _db.Add(mas1);
                        _db.SaveChanges();
                        resp = "Success";

                    }
                    else
                    {
                        resp = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Admission form error");
                _log.LogDebug(e.Message);
            }
            return resp;

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

        public async Task<CollegeImportStudentWrapperDTO> getdetails(CollegeImportStudentWrapperDTO stu)
        {

            // Adm_Master_College_StudentDMO enq1 = Mapper.Map<Adm_Master_College_StudentDMO>(stu);
            _acdimpl.LogInformation("entered try block");
            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            string studentadmno = "Student Admission Number";
            int age = 0;
            var flag = 0;
            try
            {

                List<CollegeImportStudentDTO> failedlist = new List<CollegeImportStudentDTO>();
                for (int i = 0; i < stu.newlstget1.Length; i++)
                {


                    var checkadmnoexists = _AdmissionFormContext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCST_AdmNo.Trim() == stu.newlstget1[i].AMSTAdmNo.Trim()).ToList();
                    if (checkadmnoexists.Count > 0)
                    {

                    }
                    else
                    {


                        try
                        {
                            _acdimpl.LogInformation("entered for loop");



                            Adm_Master_College_StudentDMO enq = new Adm_Master_College_StudentDMO();
                            enq.MI_Id = stu.MI_Id;
                            enq.AMCST_ActiveFlag = true;
                            // var IVRMMRId1 = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(stu.newlstget[i].Class.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();

                            if (stu.newlstget1[i].AMSTAdmNo != null && stu.newlstget1[i].AMSTAdmNo != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()), @"^[a-zA-Z0-9/\-@]*$")))
                                    {
                                        enq.AMCST_AdmNo = stu.newlstget1[i].AMSTAdmNo;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Student Admission Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student Admission Number can not be null or Empty";
                                    return stu;
                                }
                            }


                            long yearid = 0;
                            try
                            {
                                var year = "";
                                if (stu.newlstget1[i].year != null && stu.newlstget1[i].year != "")
                                {
                                    year = stu.newlstget1[i].year.Trim().ToString();
                                    if ((year != null))
                                    {
                                        if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                        {
                                            var check_yearid = _AdmissionFormContext.AcademicYear.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                            if (check_yearid.Count() > 0)
                                            {
                                                enq.ASMAY_Id = check_yearid.FirstOrDefault().ASMAY_Id;
                                                yearid = check_yearid.FirstOrDefault().ASMAY_Id;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }

                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "StudentYear can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            //--------Register Number ------------//
                            try
                            {
                                var regno = "";
                                if (stu.newlstget1[i].AMSTRegistrationNo != null && stu.newlstget1[i].AMSTRegistrationNo != "")
                                {
                                    regno = stu.newlstget1[i].AMSTRegistrationNo.Trim().ToString();
                                    if ((regno != null))
                                    {
                                        if ((Regex.IsMatch(Convert.ToString(regno), @"^[a-zA-Z0-9/\-@]*$")))
                                        {
                                            enq.AMCST_RegistrationNo = stu.newlstget1[i].AMSTRegistrationNo;
                                        }

                                        else
                                        {
                                            stu.stuStatus = "Student Registration Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            if (stu.newlstget1[i].FirstName != null && stu.newlstget1[i].FirstName != "")
                            {
                                if ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].FirstName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$")) && ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMCST_FirstName = stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Student First Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student First Name can not be null for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }

                            if (stu.newlstget1[i].MiddleName != null && stu.newlstget1[i].MiddleName != "")
                            {
                                var middlename = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                var middlename1 = stu.newlstget1[i].MiddleName.Trim();

                                if ((Regex.IsMatch(middlename, @"^[a-zA-Z.\s]+$")))
                                {
                                    if (stu.newlstget1[i].MiddleName.TrimEnd().TrimStart() == "NULL")
                                    {
                                        enq.AMCST_MiddleName = "";
                                    }
                                    else
                                    {
                                        enq.AMCST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                    }

                                }
                                else
                                {
                                    if (stu.newlstget1[i].MiddleName.TrimEnd().TrimStart() == "NULL")
                                    {
                                        enq.AMCST_MiddleName = "";
                                    }
                                    else
                                    {
                                        enq.AMCST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                    }
                                }
                            }

                            if (stu.newlstget1[i].LastName != null && stu.newlstget1[i].LastName != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].LastName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$"))))
                                {
                                    if (stu.newlstget1[i].LastName.TrimEnd().TrimStart() == "NULL")
                                    {
                                        enq.AMCST_LastName = "";
                                    }
                                    else
                                    {
                                        enq.AMCST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                    }

                                }
                                else
                                {
                                    if (stu.newlstget1[i].LastName.TrimEnd().TrimStart() == "NULL")
                                    {
                                        enq.AMCST_LastName = "";
                                    }
                                    else
                                    {
                                        enq.AMCST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                    }
                                }
                            }



                            //---Date Of Admissiom----//

                            var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

                            DateTime scheduleDate_DOB;
                            DateTime scheduleDate_DOJ;
                            string readAddMeeting = "";
                            bool validDate;
                            //DateTime scheduleDate_RET;
                            //DateTime scheduleDate_PF;
                            //DateTime scheduleDate_ESI;
                            //DateTime scheduleDate_DOL;
                            if (stu.newlstget1[i].amstdate != null && stu.newlstget1[i].amstdate != "")
                            {
                                readAddMeeting = stu.newlstget1[i].amstdate.TrimEnd().TrimStart();

                                validDate = DateTime.TryParseExact(
                                   readAddMeeting,
                                   dateFormats,
                                   DateTimeFormatInfo.InvariantInfo,
                                   DateTimeStyles.None,
                                   out scheduleDate_DOJ);
                                if (validDate)
                                {
                                    enq.AMCST_Date = scheduleDate_DOJ;
                                }
                                else
                                {
                                    stu.stuStatus = "Date Of Join is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //----gender---//
                            if (stu.newlstget1[i].Gender != null && stu.newlstget1[i].Gender != "")
                            {
                                string gender = stu.newlstget1[i].Gender.TrimEnd().TrimStart();

                                if (gender != null && gender != "")
                                {
                                    if (((Regex.IsMatch(gender, @"^([a-zA-Z/s])*$")) && (gender.Equals("Male") || gender.Equals("Female") || gender.Equals("Other") || gender.Equals("MALE") || gender.Equals("FEMALE")) && gender.Length <= 6))
                                    {
                                        enq.AMCST_Sex = gender;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Gender is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Sudent Gender is required for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--Date Of Birth--//
                            if (stu.newlstget1[i].DOB != null && stu.newlstget1[i].DOB != "")
                            {
                                readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();
                                validDate = DateTime.TryParseExact(
                                   readAddMeeting,
                                   dateFormats,
                                   DateTimeFormatInfo.InvariantInfo,
                                   DateTimeStyles.None,
                                   out scheduleDate_DOB);
                                if (validDate)
                                {
                                    enq.AMCST_DOB = scheduleDate_DOB;
                                }
                                else
                                {
                                    stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //--DOB Words --//
                            if (stu.newlstget1[i].AMSTDOBWords != null && stu.newlstget1[i].AMSTDOBWords != "")
                            {
                                if (stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != null && stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != "")
                                {
                                    enq.AMCST_DOBin_words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    enq.AMCST_DOBin_words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                                }

                                DateTime fromdatecon = DateTime.Now;
                                string confromdate = "";
                                fromdatecon = Convert.ToDateTime(stu.newlstget1[i].DOB);
                                //confromdate = fromdatecon.ToString();
                                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                //--Age calculation--//
                                age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(confromdate).Year);

                                if (age > 0)
                                {
                                    age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(confromdate).Date.AddYears(age));
                                }
                                else
                                {
                                    age = 0;
                                }
                                enq.AMCST_Age = age;
                            }


                            long classid;
                            //--getting class id from class name --//

                            var check_class = _AdmissionFormContext.MasterCourseDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCO_CourseName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Course.TrimEnd().TrimStart().ToLower()).ToList();
                            if (check_class.Count > 0)
                            {
                                var class_id = _AdmissionFormContext.MasterCourseDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCO_CourseName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Course.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                enq.AMCO_Id = class_id.AMCO_Id;
                                classid = class_id.AMCO_Id;
                            }
                            else
                            {
                                stu.stuStatus = "Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                            var check_branch = _AdmissionFormContext.ClgMasterBranchDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMB_BranchName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Branch.TrimEnd().TrimStart().ToLower()).ToList();
                            if (check_class.Count > 0)
                            {
                                var class_id = _AdmissionFormContext.ClgMasterBranchDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMB_BranchName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Branch.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                enq.AMB_Id = class_id.AMB_Id;
                            }
                            else
                            {
                                stu.stuStatus = "Branch is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                            var check_sem = _AdmissionFormContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMSE_SEMName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Sem.TrimEnd().TrimStart().ToLower()).ToList();
                            if (check_class.Count > 0)
                            {
                                var class_id = _AdmissionFormContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMSE_SEMName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Sem.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                enq.AMSE_Id = class_id.AMSE_Id;
                            }
                            else
                            {
                                stu.stuStatus = "Semeister is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                            var class_category = _AdmissionFormContext.mastercategory.Where(t => t.MI_Id == stu.MI_Id).FirstOrDefault();
                            enq.AMCOC_Id = class_category.AMCOC_Id;

                            enq.AMCST_BloodGroup = stu.newlstget1[i].BloodGroup;


                            //----Mother Tongue----//
                            if (stu.newlstget1[i].MotherTongue != null && stu.newlstget1[i].MotherTongue != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart(), @"^[a-zA-Z\s]+$")) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == null) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == "")))
                                {
                                    enq.AMCST_MotherTongue = stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Student MotherTongue is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //---Religion---//
                            if (stu.newlstget1[i].Religion != null && stu.newlstget1[i].Religion != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Religion, @"^[a-zA-Z\s]+$"))))
                                {
                                    var get_religionid = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name.Equals(stu.newlstget1[i].Religion.TrimEnd().TrimStart())).ToList();
                                    if (get_religionid.Count() > 0)
                                    {
                                        enq.IVRMMR_Id = get_religionid.FirstOrDefault().IVRMMR_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Religion is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }

                                }
                                else
                                {
                                    stu.stuStatus = "Student Religion is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--caste --//
                            long get_casteid1 = 0;
                            if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z\s]+$"))))
                                {
                                    var get_casteid = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName.Equals(stu.newlstget1[i].Caste.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).ToList();
                                    if (get_casteid.Count() > 0)
                                    {
                                        enq.IMC_Id = get_casteid.FirstOrDefault().IMC_Id;
                                        get_casteid1 = get_casteid.FirstOrDefault().IMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student caste is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }

                                }
                                else
                                {
                                    stu.stuStatus = "Student caste is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //geting caste category id
                            //var castecategoryid = "";
                            if (stu.newlstget1[i].CasteCategory != null && stu.newlstget1[i].CasteCategory != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].CasteCategory, @"^[a-zA-Z0-9\s]+$"))))
                                {
                                    var get_castecategoryid = (from a in _AdmissionFormContext.CasteCategory
                                                               from b in _AdmissionFormContext.Caste
                                                               where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == stu.MI_Id && b.IMC_Id == Convert.ToInt64(get_casteid1))
                                                               select new
                                                               {
                                                                   castecategoryid = a.IMCC_Id
                                                               }).ToList();
                                    if (get_castecategoryid.Count() > 0)
                                    {
                                        enq.IMCC_Id = get_castecategoryid.FirstOrDefault().castecategoryid;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student caste category is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student caste category is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //----------------Permanent Address--------------------//

                            //street
                            if (stu.newlstget1[i].PermanentStreet != null && stu.newlstget1[i].PermanentStreet != "")
                            {
                                if (stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\;\.()\.\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart()).Length <= 150)))

                                    {
                                        enq.AMCST_PerStreet = stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }


                            //area
                            if (stu.newlstget1[i].PermanentArea != null && stu.newlstget1[i].PermanentArea != "")
                            {
                                if (stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\;\().\:\,/&#])*$")) && ((stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMCST_PerArea = stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //city
                            if (stu.newlstget1[i].PermanentCity != null && stu.newlstget1[i].PermanentCity != "")
                            {
                                if (stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\()\,\'\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMCST_PerCity = stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //permanent country
                            long countryid = 0;
                            if (stu.newlstget1[i].PermanentCountry != null && stu.newlstget1[i].PermanentCountry != "")
                            {
                                if (stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()));
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMCST_ConCountryId = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            countryid = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Permanent Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //permanent state
                            if (stu.newlstget1[i].Permanentstate != null && stu.newlstget1[i].Permanentstate != "")
                            {
                                if (stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != null && stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_stateid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid);
                                        if (get_stateid.Count() > 0)
                                        {
                                            enq.AMCST_PerState = get_stateid.FirstOrDefault().IVRMMS_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Permanent Pincode 
                            if (stu.newlstget1[i].PermanentPincode != null && stu.newlstget1[i].PermanentPincode != "")
                            {
                                if (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != "")
                                {
                                    if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()).Length == 6)))
                                    {
                                        enq.AMCST_PerPincode = Convert.ToInt32(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart());
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //---End of the permanent address------//


                            //-------------------------Communication Address--------------------------//
                            //street
                            if (stu.newlstget1[i].PresentStreet != null && stu.newlstget1[i].PresentStreet != "")
                            {
                                if (stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\()\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMCST_ConStreet = stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //area
                            if (stu.newlstget1[i].PresentArea != null && stu.newlstget1[i].PresentArea != "")
                            {
                                if (stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\()\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentArea.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMCST_ConArea = stu.newlstget1[i].PresentArea.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                //else
                                //{
                                //    stu.stuStatus = "Present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                //    return stu;
                                //}
                            }

                            //city
                            if (stu.newlstget1[i].PresentCity != null && stu.newlstget1[i].PresentCity != "")
                            {
                                if (stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\'\,()\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentCity.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMCST_ConCity = stu.newlstget1[i].PresentCity.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "present city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                            }
                            //else
                            //{
                            //    stu.stuStatus = "present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            //    return stu;
                            //}

                            //persent country
                            long countryid1 = 0;
                            if (stu.newlstget1[i].PresentCountry != null && stu.newlstget1[i].PresentCountry != "")
                            {
                                if (stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()));
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMCST_ConCountryId = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Present Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //present state
                            if (stu.newlstget1[i].PresentState != null && stu.newlstget1[i].PresentState != "")
                            {
                                if (stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentState.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentState.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].PresentState.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid1);
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMCST_ConState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "present state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //present Pincode 
                            if (stu.newlstget1[i].PresentPincode != null && stu.newlstget1[i].PresentPincode != "")
                            {
                                if (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != "")
                                {
                                    if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()).Length == 6)))
                                    {
                                        enq.AMCST_ConPincode = Convert.ToInt32(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart());
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //---End of the Communication address------//

                            ///-------------Aadhar Number -----------
                            if (stu.newlstget1[i].AadharNo != null && stu.newlstget1[i].AadharNo != "")
                            {
                                if (stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != null && stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$")) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0)) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
                                    {
                                        enq.AMCST_AadharNo = Convert.ToInt64(stu.newlstget1[i].AadharNo.TrimEnd().TrimStart());
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Aadhar Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Aadhar Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--- Student Mobile Number --//
                            if (stu.newlstget1[i].MobileNo != null && stu.newlstget1[i].MobileNo != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
                                {
                                    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                    if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                    {
                                        enq.AMCST_MobileNo = Convert.ToInt64(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart());
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--- Student Email id--//
                            if (stu.newlstget1[i].EmailID != null && stu.newlstget1[i].EmailID != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != ""))
                                {
                                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                    Match match = regex.Match(stu.newlstget1[i].EmailID.TrimEnd().TrimStart());
                                    if (match.Success)
                                    {
                                        enq.AMCST_emailId = stu.newlstget1[i].EmailID.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //----------------Father Details-------------//
                            if (stu.newlstget1[i].FatherName != null && stu.newlstget1[i].FatherName != "")
                            {
                                if ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].FatherName.TrimEnd().TrimStart(), @"^[a-zA-Z\-.\s]+$")) && ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMCST_FatherName = stu.newlstget1[i].FatherName.TrimEnd().TrimStart();
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Father Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Father Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }


                            //Father Mobile no
                            if (stu.newlstget1[i].Fathermobileno != null && stu.newlstget1[i].Fathermobileno != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != ""))
                                {
                                    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                    if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                    {
                                        enq.AMCST_FatherMobleNo = Convert.ToInt64(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart());
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Father Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Father Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Father Email id
                            if (stu.newlstget1[i].FatherEmailId != null && stu.newlstget1[i].FatherEmailId != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != ""))
                                {
                                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                    Match match = regex.Match(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart());
                                    if (match.Success)
                                    {
                                        enq.AMCST_FatheremailId = stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Father Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Father Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //-------Mother Details ----------//

                            if (stu.newlstget1[i].MotherName != null && stu.newlstget1[i].MotherName != "")
                            {
                                if ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].MotherName.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMCST_MotherName = stu.newlstget1[i].MotherName.TrimEnd().TrimStart();
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Mother Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }


                            //Father Mobile no
                            if (stu.newlstget1[i].MotherMobileNo != null && stu.newlstget1[i].MotherMobileNo != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != ""))
                                {
                                    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                    if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                    {
                                        enq.AMCST_MotherMobleNo = Convert.ToInt64(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart());
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Mother Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Father Email id
                            if (stu.newlstget1[i].MotherEmailId != null && stu.newlstget1[i].MotherEmailId != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != ""))
                                {
                                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                    Match match = regex.Match(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart());
                                    if (match.Success)
                                    {
                                        enq.AMCST_MotheremailId = stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart();
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Mother Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //student nationality
                            //long countryid12 = 0;
                            if (stu.newlstget1[i].StudentNationality != null && stu.newlstget1[i].StudentNationality != "")
                            {
                                if (stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != null && stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\:\,])*$")) && ((stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()).Length <= 50)))
                                    {
                                        var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()));
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMCST_Nationality = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "student Nationality Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            if (stu.newlstget1[i].Quota != null && stu.newlstget1[i].Quota != "")
                            {
                                var check_qouta = _AdmissionFormContext.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == stu.MI_Id
                         && a.ACQ_QuotaName.Trim() == stu.newlstget1[i].Quota.Trim()).ToList();

                                if (check_qouta.Count() > 0)
                                {
                                    var check_qouta1 = _AdmissionFormContext.Clg_Adm_College_QuotaDMO.Single(a => a.MI_Id == stu.MI_Id
                                                           && a.ACQ_QuotaName.Trim() == stu.newlstget1[i].Quota.Trim());
                                    enq.ACQ_Id = check_qouta1.ACQ_Id;
                                }
                                else
                                {
                                    stu.stuStatus = "Quota is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            if (stu.newlstget1[i].QuotaCategory != null && stu.newlstget1[i].QuotaCategory != "")
                            {
                                var check_qouta_category = _AdmissionFormContext.Clg_Adm_College_Quota_CategoryDMO.Where(a => a.MI_Id == stu.MI_Id
                      && a.ACQC_CategoryName.Trim() == stu.newlstget1[i].QuotaCategory.Trim()).ToList();

                                if (check_qouta_category.Count() > 0)
                                {
                                    var check_qouta_category1 = _AdmissionFormContext.Clg_Adm_College_Quota_CategoryDMO.Single(a => a.MI_Id == stu.MI_Id
                                                           && a.ACQC_CategoryName.Trim() == stu.newlstget1[i].QuotaCategory.Trim());
                                    enq.ACQC_Id = check_qouta_category1.ACQC_Id;
                                }
                                else
                                {
                                    stu.stuStatus = "Quota category is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }


                            enq.AMCST_SOL = "S";
                            enq.ACMB_Id = 1;
                            enq.ACST_Id = 1;
                            enq.ACSS_Id = _AdmissionFormContext.AdmCollegeSubjectSchemeDMO.Where(s => s.MI_Id == stu.MI_Id).Select(d => d.ACSS_Id).FirstOrDefault();
                            //enq.ACQ_Id = ;
                            enq.CreatedDate = DateTime.Now;
                            enq.UpdatedDate = DateTime.Now;

                            _AdmissionFormContext.Add(enq);


                            try
                            {
                                if (enq.AMCST_FatheremailId != null && enq.AMCST_FatheremailId != "")
                                {
                                    AdmCollegeStudentParentsEmailIdDMO enq_faheremail = new AdmCollegeStudentParentsEmailIdDMO();
                                    enq_faheremail.ACSTPE_EmailId = enq.AMCST_FatheremailId;
                                    enq_faheremail.AMCST_Id = enq.AMCST_Id;
                                    enq_faheremail.ACSTPE_Flag = "F";
                                    enq_faheremail.CreatedDate = DateTime.Now;
                                    enq_faheremail.UpdatedDate = DateTime.Now;
                                    _AdmissionFormContext.Add(enq_faheremail);
                                    // int n = _AdmissionFormContext.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("Father emailid saving in new table" + ex.Message);
                            }

                            try
                            {
                                if (enq.AMCST_MotheremailId != null && enq.AMCST_MotheremailId != "")
                                {
                                    AdmCollegeStudentParentsEmailIdDMO enq_motheemail = new AdmCollegeStudentParentsEmailIdDMO();
                                    enq_motheemail.ACSTPE_EmailId = enq.AMCST_MotheremailId;
                                    enq_motheemail.AMCST_Id = enq.AMCST_Id;
                                    enq_motheemail.ACSTPE_Flag = "M";
                                    enq_motheemail.CreatedDate = DateTime.Now;
                                    enq_motheemail.UpdatedDate = DateTime.Now;
                                    _AdmissionFormContext.Add(enq_motheemail);
                                    //int n = _AdmissionFormContext.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("Mother emailid saving in new table" + ex.Message);
                            }

                            try
                            {
                                if (enq.AMCST_MotherMobleNo != null)
                                {
                                    AdmCollegeStudentParentsMobileNoDMO enq_mothermobile = new AdmCollegeStudentParentsMobileNoDMO();
                                    enq_mothermobile.ACSTPMN_MobileNo = Convert.ToInt64(enq.AMCST_MotherMobleNo);
                                    enq_mothermobile.AMCST_Id = enq.AMCST_Id;
                                    enq_mothermobile.ACSTPMN_Flag = "M";
                                    enq_mothermobile.CreatedDate = DateTime.Now;
                                    enq_mothermobile.UpdatedDate = DateTime.Now;
                                    _AdmissionFormContext.Add(enq_mothermobile);
                                    // int n2 = _AdmissionFormContext.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("Mother Mobileno saving in new table" + ex.Message);
                            }

                            try
                            {
                                if (enq.AMCST_FatherMobleNo != null)
                                {
                                    AdmCollegeStudentParentsMobileNoDMO enq_mothermobile = new AdmCollegeStudentParentsMobileNoDMO();
                                    enq_mothermobile.ACSTPMN_MobileNo = Convert.ToInt64(enq.AMCST_FatherMobleNo);
                                    enq_mothermobile.AMCST_Id = enq.AMCST_Id;
                                    enq_mothermobile.ACSTPMN_Flag = "F";
                                    enq_mothermobile.CreatedDate = DateTime.Now;
                                    enq_mothermobile.UpdatedDate = DateTime.Now;
                                    _AdmissionFormContext.Add(enq_mothermobile);
                                    //int n2 = _AdmissionFormContext.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("Father Mobileno saving in new table" + ex.Message);
                            }
                            try
                            {
                                if (enq.AMCST_MobileNo != null)
                                {
                                    AdmCollegeStudentSMSNoDMO enq_studentmobile = new AdmCollegeStudentSMSNoDMO();
                                    enq_studentmobile.AMCST_Id = enq.AMCST_Id;
                                    enq_studentmobile.ACSTSMS_MobileNo = Convert.ToInt64(enq.AMCST_MobileNo);
                                    enq_studentmobile.CreatedDate = DateTime.Now;
                                    enq_studentmobile.UpdatedDate = DateTime.Now;
                                    _AdmissionFormContext.Add(enq_studentmobile);
                                    //  int n4 = _DomainModelMsSqlServerContext.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("Student Mobileno saving in new table" + ex.Message);
                            }
                            try
                            {
                                if (enq.AMCST_emailId != null && enq.AMCST_emailId != "")
                                {
                                    AdmCollegeStudentEmailIdDMO enq_studentemail = new AdmCollegeStudentEmailIdDMO();
                                    enq_studentemail.AMCST_Id = enq.AMCST_Id;
                                    enq_studentemail.ACSTE_EmailId = enq.AMCST_emailId;
                                    enq_studentemail.CreatedDate = DateTime.Now;
                                    enq_studentemail.UpdatedDate = DateTime.Now;
                                    _AdmissionFormContext.Add(enq_studentemail);
                                    // int n4 = _DomainModelMsSqlServerContext.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("Student Email saving in new table" + ex.Message);

                            }



                            long yearid_current = 0;
                            long courseid_current = 0;
                            long branch_current = 0;
                            long semester_current = 0;
                            long section_current = 0;
                            try
                            {
                                var year = "";
                                if (stu.newlstget1[i].CurrentYear != null && stu.newlstget1[i].CurrentYear != "")
                                {
                                    year = stu.newlstget1[i].CurrentYear.Trim().ToString();

                                    if ((year != null))
                                    {
                                        if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                        {
                                            var check_yearid = _AdmissionFormContext.AcademicYear.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                            if (check_yearid.Count() > 0)
                                            {
                                                yearid_current = check_yearid.FirstOrDefault().ASMAY_Id;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Current Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Current Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Current Year can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            try
                            {
                                var current_course = "";
                                if (stu.newlstget1[i].CurrentCourse != null && stu.newlstget1[i].CurrentCourse != "")
                                {
                                    current_course = stu.newlstget1[i].CurrentCourse.Trim().ToString();

                                    if (current_course != null)
                                    {
                                        var check_classid = _AdmissionFormContext.MasterCourseDMO.Where(t => t.AMCO_CourseName.Equals(current_course)
                                        && t.MI_Id == stu.MI_Id).ToList();
                                        if (check_classid.Count() > 0)
                                        {
                                            courseid_current = check_classid.FirstOrDefault().AMCO_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Current Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Current Class can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Current Class can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            try
                            {
                                var current_branch = "";
                                if (stu.newlstget1[i].CurrentBranch != null && stu.newlstget1[i].CurrentBranch != "")
                                {
                                    current_branch = stu.newlstget1[i].CurrentBranch.Trim().ToString();

                                    if (current_branch != null)
                                    {
                                        var check_sectionid = _AdmissionFormContext.ClgMasterBranchDMO.Where(t => t.AMB_BranchName.Equals(current_branch)
                                        && t.MI_Id == stu.MI_Id).ToList();
                                        if (check_sectionid.Count() > 0)
                                        {
                                            branch_current = check_sectionid.FirstOrDefault().AMB_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Current Branch is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Current Branch can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Current Branch can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            try
                            {
                                var current_semester = "";
                                if (stu.newlstget1[i].CurrentSem != null && stu.newlstget1[i].CurrentSem != "")
                                {
                                    current_semester = stu.newlstget1[i].CurrentSem.Trim().ToString();

                                    if (current_semester != null)
                                    {
                                        var check_sectionid = _AdmissionFormContext.CLG_Adm_Master_SemesterDMO.Where(t => t.AMSE_SEMName.Equals(current_semester)
                                        && t.MI_Id == stu.MI_Id).ToList();
                                        if (check_sectionid.Count() > 0)
                                        {
                                            semester_current = check_sectionid.FirstOrDefault().AMSE_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Current Semester is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Current Semester can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Current Semester can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }


                            try
                            {
                                var Current_Section = "";
                                if (stu.newlstget1[i].CurrentSection != null && stu.newlstget1[i].CurrentSection != "")
                                {
                                    Current_Section = stu.newlstget1[i].CurrentSection.Trim().ToString();

                                    if (Current_Section != null)
                                    {
                                        var check_sectionid = _AdmissionFormContext.Adm_College_Master_SectionDMO.Where(t => t.ACMS_SectionName.Equals(Current_Section)
                                        && t.MI_Id == stu.MI_Id).ToList();
                                        if (check_sectionid.Count() > 0)
                                        {
                                            section_current = check_sectionid.FirstOrDefault().ACMS_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Current Section is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Current Section can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Current Semester can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            if (yearid_current > 0 && courseid_current > 0 && branch_current > 0 && semester_current > 0 && section_current > 0)
                            {
                                Adm_College_Yearly_StudentDMO Adm_College_Yearly_StudentDMO = new Adm_College_Yearly_StudentDMO();
                                Adm_College_Yearly_StudentDMO.ASMAY_Id = yearid_current;
                                Adm_College_Yearly_StudentDMO.AMCO_Id = courseid_current;
                                Adm_College_Yearly_StudentDMO.AMB_Id = branch_current;
                                Adm_College_Yearly_StudentDMO.AMSE_Id = semester_current;
                                Adm_College_Yearly_StudentDMO.ACMS_Id = section_current;
                                Adm_College_Yearly_StudentDMO.AMCST_Id = enq.AMCST_Id;
                                Adm_College_Yearly_StudentDMO.ACYST_RollNo = 1;
                                Adm_College_Yearly_StudentDMO.ACYST_ActiveFlag = 1;
                                Adm_College_Yearly_StudentDMO.LoginId = stu.userid;
                                //Adm_College_Yearly_StudentDMO.cre = stu.userid;
                                //Adm_College_Yearly_StudentDMO.ASYST_UpdatedBy = stu.userid;
                                Adm_College_Yearly_StudentDMO.ACYST_DateTime = DateTime.Now;
                                Adm_College_Yearly_StudentDMO.CreatedDate = DateTime.Now;
                                Adm_College_Yearly_StudentDMO.UpdatedDate = DateTime.Now;
                                _AdmissionFormContext.Add(Adm_College_Yearly_StudentDMO);

                                sucesscount += 1;

                                //var i1 = _AdmissionFormContext.SaveChanges();
                            }

                            //var id = _DomainModelMsSqlServerContext.SaveChanges();


                            if (sucesscount >= 1)
                            {
                                flag = _AdmissionFormContext.SaveChanges();
                                stu.stuStatus = "true";
                            }
                            else
                            {
                                // stu.stuStatus = "false";
                                failcount = failcount + 1;
                                string name = failnames;
                                finalnames += name + ",";
                                failedlist.Add(stu.newlstget1[i]);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            failcount = failcount + 1;
                            string name = failnames;
                            finalnames += name + ",";
                            failedlist.Add(stu.newlstget1[i]);
                            //failnames = failnames + "," + failnames;
                            continue;

                        }
                    }
                }
                stu.failedlist = failedlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            if (finalnames != "" && failcount > 0)
            {
                stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "' And Failed List Names :' " + finalnames + "'";
            }
            else
            {
                stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + 0 + "'";
            }


            //catch (Exception e)
            //{
            //    stu.stuStatus = "false";
            //    Console.WriteLine(e.Message);
            //}

            return stu;
        }

        public async Task<CollegeImportStudentWrapperDTO> checkvalidation(CollegeImportStudentWrapperDTO stu)
        {
            // Adm_Master_College_StudentDMO enq1 = Mapper.Map<Adm_Master_College_StudentDMO>(stu);
            _acdimpl.LogInformation("entered try block");
            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            string studentadmno = "Student Admission Number";
            int age = 0;
            try
            {

                List<CollegeImportStudentDTO> failedlist = new List<CollegeImportStudentDTO>();
                for (int i = 0; i < stu.newlstget1.Length; i++)
                {
                    try
                    {
                        _acdimpl.LogInformation("entered for loop");
                        Adm_Master_College_StudentDMO enq = new Adm_Master_College_StudentDMO();
                        enq.MI_Id = stu.MI_Id;
                        enq.AMCST_ActiveFlag = true;
                        // var IVRMMRId1 = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(stu.newlstget[i].Class.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();

                        if (stu.newlstget1[i].AMSTAdmNo != null && stu.newlstget1[i].AMSTAdmNo != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()) != null))
                            {
                                if ((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()), @"^[a-zA-Z0-9/\-@\s]*$")))
                                {
                                    enq.AMCST_AdmNo = stu.newlstget1[i].AMSTAdmNo;
                                }

                                else
                                {
                                    stu.stuStatus = "Student Admission Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Student Admission Number can not be null or Empty";
                                return stu;
                            }
                        }


                        long yearid = 0;
                        try
                        {
                            var year = "";
                            if (stu.newlstget1[i].year != null && stu.newlstget1[i].year != "")
                            {
                                year = stu.newlstget1[i].year.Trim().ToString();
                                if ((year != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                    {
                                        var check_yearid = _AdmissionFormContext.AcademicYear.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                        if (check_yearid.Count() > 0)
                                        {
                                            enq.ASMAY_Id = check_yearid.FirstOrDefault().ASMAY_Id;
                                            yearid = check_yearid.FirstOrDefault().ASMAY_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }

                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    return stu;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            stu.stuStatus = "StudentYear can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                            return stu;
                        }

                        //--------Register Number ------------//
                        try
                        {
                            var regno = "";
                            if (stu.newlstget1[i].AMSTRegistrationNo != null && stu.newlstget1[i].AMSTRegistrationNo != "")
                            {
                                regno = stu.newlstget1[i].AMSTRegistrationNo.Trim().ToString();
                                if ((regno != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(regno), @"^[a-zA-Z0-9/\-@]*$")))
                                    {
                                        enq.AMCST_RegistrationNo = stu.newlstget1[i].AMSTRegistrationNo;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Student Registration Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    return stu;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                            return stu;
                        }
                        if (stu.newlstget1[i].FirstName != null && stu.newlstget1[i].FirstName != "")
                        {
                            if ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != ""))
                            {
                                if ((Regex.IsMatch(stu.newlstget1[i].FirstName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$")) && ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart()).Length <= 50))
                                {
                                    enq.AMCST_FirstName = stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    stu.stuStatus = "Student First Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Student First Name can not be null for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                return stu;
                            }
                        }

                        if (stu.newlstget1[i].MiddleName != null && stu.newlstget1[i].MiddleName != "")
                        {
                            var middlename = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                            var middlename1 = stu.newlstget1[i].MiddleName.Trim();

                            if ((Regex.IsMatch(middlename, @"^[a-zA-Z.\s]+$")))
                            {
                                if (stu.newlstget1[i].MiddleName.TrimEnd().TrimStart() == "NULL")
                                {
                                    enq.AMCST_MiddleName = "";
                                }
                                else
                                {
                                    enq.AMCST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                }

                            }
                            else
                            {
                                //if (stu.newlstget1[i].MiddleName.TrimEnd().TrimStart() == "NULL")
                                //{
                                enq.AMCST_MiddleName = "";
                                //}
                                //else
                                //{
                                //    enq.AMCST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                //}
                            }
                        }


                        if (stu.newlstget1[i].LastName != null && stu.newlstget1[i].LastName != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].LastName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$"))))
                            {
                                if (stu.newlstget1[i].LastName.TrimEnd().TrimStart() == "NULL")
                                {
                                    enq.AMCST_LastName = "";
                                }
                                else
                                {
                                    enq.AMCST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                }

                            }
                        }

                        else
                        {
                            //if (stu.newlstget1[i].LastName.TrimEnd().TrimStart() == "NULL")
                            //{
                            enq.AMCST_LastName = "";
                            //}
                            //else
                            //{
                            //    enq.AMCST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                            //}
                        }


                        //---Date Of Admissiom----//

                        var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

                        DateTime scheduleDate_DOB;
                        DateTime scheduleDate_DOJ;
                        string readAddMeeting = "";
                        bool validDate;
                        //DateTime scheduleDate_RET;
                        //DateTime scheduleDate_PF;
                        //DateTime scheduleDate_ESI;
                        //DateTime scheduleDate_DOL;
                        if (stu.newlstget1[i].amstdate != null && stu.newlstget1[i].amstdate != "")
                        {
                            readAddMeeting = stu.newlstget1[i].amstdate.TrimEnd().TrimStart();

                            validDate = DateTime.TryParseExact(
                               readAddMeeting,
                               dateFormats,
                               DateTimeFormatInfo.InvariantInfo,
                               DateTimeStyles.None,
                               out scheduleDate_DOJ);
                            if (validDate)
                            {
                                enq.AMCST_Date = scheduleDate_DOJ;
                            }
                            else
                            {
                                stu.stuStatus = "Date Of Join is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //----gender---//

                        string gender = "";

                        if (stu.newlstget1[i].Gender != null && stu.newlstget1[i].Gender != "")
                        {
                            gender = stu.newlstget1[i].Gender.TrimEnd().TrimStart();
                            if (gender != null && gender != "")
                            {
                                if (((Regex.IsMatch(gender, @"^([a-zA-Z/s])*$")) && (gender.Equals("Male") || gender.Equals("Female") || gender.Equals("Other") || gender.Equals("MALE") || gender.Equals("FEMALE")) && gender.Length <= 6))
                                {
                                    enq.AMCST_Sex = gender;
                                }
                                else
                                {
                                    stu.stuStatus = "Student Gender is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                        }


                        else
                        {
                            stu.stuStatus = "Sudent Gender is required for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }

                        //--Date Of Birth--//
                        readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();
                        validDate = DateTime.TryParseExact(
                           readAddMeeting,
                           dateFormats,
                           DateTimeFormatInfo.InvariantInfo,
                           DateTimeStyles.None,
                           out scheduleDate_DOB);
                        if (validDate)
                        {
                            enq.AMCST_DOB = scheduleDate_DOB;
                        }
                        else
                        {
                            stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }


                        //--DOB Words --//
                        //if (stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != null && stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != "")
                        //{
                        //    enq.AMCST_DOBin_words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                        //}
                        //else
                        //{
                        //    enq.AMCST_DOBin_words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                        //}

                        DateTime fromdatecon = DateTime.Now;
                        string confromdate = "";
                        if (stu.newlstget1[i].DOB != null && stu.newlstget1[i].DOB != "")
                        {
                            fromdatecon = Convert.ToDateTime(stu.newlstget1[i].DOB);
                            //confromdate = fromdatecon.ToString();
                            confromdate = fromdatecon.ToString("yyyy-MM-dd");
                            //--Age calculation--//
                            age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(confromdate).Year);

                            if (age > 0)
                            {
                                age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(confromdate).Date.AddYears(age));
                            }
                            else
                            {
                                age = 0;
                            }
                            enq.AMCST_Age = age;
                        }





                        long classid;
                        //--getting class id from class name --//
                        var check_class = _AdmissionFormContext.MasterCourseDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCO_CourseName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Course.TrimEnd().TrimStart().ToLower()).ToList();
                        if (check_class.Count > 0)
                        {
                            var class_id = _AdmissionFormContext.MasterCourseDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMCO_CourseName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Course.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                            enq.AMCO_Id = class_id.AMCO_Id;
                            classid = class_id.AMCO_Id;
                        }
                        else
                        {
                            stu.stuStatus = "Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }

                        var check_branch = _AdmissionFormContext.ClgMasterBranchDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMB_BranchName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Branch.TrimEnd().TrimStart().ToLower()).ToList();
                        if (check_branch.Count > 0)
                        {
                            var class_id = _AdmissionFormContext.ClgMasterBranchDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMB_BranchName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Branch.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                            enq.AMB_Id = class_id.AMB_Id;
                        }
                        else
                        {
                            stu.stuStatus = "Branch is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }

                        var check_sem = _AdmissionFormContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == stu.MI_Id && t.AMSE_SEMName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Sem.TrimEnd().TrimStart().ToLower()).ToList();
                        if (check_sem.Count > 0)
                        {
                            var class_id = _AdmissionFormContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == stu.MI_Id &&
                            t.AMSE_SEMName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Sem.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                            enq.AMSE_Id = class_id.AMSE_Id;
                        }
                        else
                        {
                            stu.stuStatus = "Semeister is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }

                        // var class_category = _AdmissionFormContext.mastercategory.Where(t => t.MI_Id == stu.MI_Id && t.AMCOC_Id==1).FirstOrDefault();
                        enq.AMCOC_Id = 2;


                        if (stu.newlstget1[i].Quota != null && stu.newlstget1[i].Quota != "")
                        {
                            var check_qouta = _AdmissionFormContext.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == stu.MI_Id
                     && a.ACQ_QuotaName.Trim() == stu.newlstget1[i].Quota.Trim()).ToList();

                            if (check_qouta.Count() > 0)
                            {
                                var check_qouta1 = _AdmissionFormContext.Clg_Adm_College_QuotaDMO.Single(a => a.MI_Id == stu.MI_Id
                                                       && a.ACQ_QuotaName.Trim() == stu.newlstget1[i].Quota.Trim());
                                enq.ACQ_Id = check_qouta1.ACQ_Id;
                            }
                            else
                            {
                                stu.stuStatus = "Quota is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        if (stu.newlstget1[i].QuotaCategory != null && stu.newlstget1[i].QuotaCategory != "")
                        {
                            var check_qouta_category = _AdmissionFormContext.Clg_Adm_College_Quota_CategoryDMO.Where(a => a.MI_Id == stu.MI_Id
                  && a.ACQC_CategoryName.Trim() == stu.newlstget1[i].QuotaCategory.Trim()).ToList();

                            if (check_qouta_category.Count() > 0)
                            {
                                var check_qouta_category1 = _AdmissionFormContext.Clg_Adm_College_Quota_CategoryDMO.Single(a => a.MI_Id == stu.MI_Id
                                                       && a.ACQC_CategoryName.Trim() == stu.newlstget1[i].QuotaCategory.Trim());
                                enq.ACQC_Id = check_qouta_category1.ACQC_Id;
                            }
                            else
                            {
                                stu.stuStatus = "Quota category is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }


                        if (stu.newlstget1[i].BloodGroup != null && stu.newlstget1[i].BloodGroup != "")
                        {
                            enq.AMCST_BloodGroup = stu.newlstget1[i].BloodGroup;
                        }



                        //----Mother Tongue----//
                        if (stu.newlstget1[i].MotherTongue != null && stu.newlstget1[i].MotherTongue != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart(), @"^[a-zA-Z\s]+$")) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == null) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == "")))
                            {
                                enq.AMCST_MotherTongue = stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart();
                            }
                            else
                            {
                                stu.stuStatus = "Student MotherTongue is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //---Religion---//
                        if (stu.newlstget1[i].Religion != null && stu.newlstget1[i].Religion != null)
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].Religion, @"^[a-zA-Z\s]+$"))))
                            {
                                var get_religionid = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name.Equals(stu.newlstget1[i].Religion.TrimEnd().TrimStart())).ToList();
                                if (get_religionid.Count() > 0)
                                {
                                    enq.IVRMMR_Id = get_religionid.FirstOrDefault().IVRMMR_Id;
                                }
                                else
                                {
                                    stu.stuStatus = "Student Religion is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }
                            else
                            {
                                stu.stuStatus = "Student Religion is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //--caste --//
                        long get_casteid1 = 0;
                        if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z\s]+$"))))
                            {
                                var get_casteid = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName.Equals(stu.newlstget1[i].Caste.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).ToList();
                                if (get_casteid.Count() > 0)
                                {
                                    enq.IMC_Id = get_casteid.FirstOrDefault().IMC_Id;
                                    get_casteid1 = get_casteid.FirstOrDefault().IMC_Id;
                                }
                                else
                                {
                                    stu.stuStatus = "Student caste is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }
                            else
                            {
                                stu.stuStatus = "Student caste is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //geting caste category id
                        //var castecategoryid = "";
                        if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z0-9\s]+$"))))
                            {
                                var get_castecategoryid = (from a in _AdmissionFormContext.CasteCategory
                                                           from b in _AdmissionFormContext.Caste
                                                           where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == stu.MI_Id && b.IMC_Id == Convert.ToInt64(get_casteid1))
                                                           select new
                                                           {
                                                               castecategoryid = a.IMCC_Id
                                                           }).ToList();
                                if (get_castecategoryid.Count() > 0)
                                {
                                    enq.IMCC_Id = get_castecategoryid.FirstOrDefault().castecategoryid;
                                }
                                else
                                {
                                    stu.stuStatus = "Student caste category is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Student caste category is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }


                        if (stu.newlstget1[i].subcaste != null && stu.newlstget1[i].subcaste != "")
                        {
                            enq.AMCST_StudentSubCaste = stu.newlstget1[i].subcaste;
                        }


                        //----------------Permanent Address--------------------//

                        //street
                        if (stu.newlstget1[i].PermanentStreet != null && stu.newlstget1[i].PermanentStreet != "")
                        {
                            if (stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\;\.()\.\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart()).Length <= 150)))

                                {
                                    enq.AMCST_PerStreet = stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //area
                        if (stu.newlstget1[i].PermanentArea != null && stu.newlstget1[i].PermanentArea != "")
                        {
                            if (stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\;\().\:\,/&#])*$")) && ((stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMCST_PerArea = stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //city
                        if (stu.newlstget1[i].PermanentCity != null && stu.newlstget1[i].PermanentCity != "")
                        {
                            if (stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\()\,\'\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMCST_PerCity = stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //permanent country
                        long countryid = 0;
                        if (stu.newlstget1[i].PermanentCountry != null && stu.newlstget1[i].PermanentCountry != "")
                        {
                            if (stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()));
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMCST_ConCountryId = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        countryid = get_countryid.FirstOrDefault().IVRMMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //permanent state
                        if (stu.newlstget1[i].Permanentstate != null && stu.newlstget1[i].Permanentstate != "")
                        {
                            if (stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != null && stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid);
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMCST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //Permanent Pincode 
                        if (stu.newlstget1[i].PermanentPincode != null && stu.newlstget1[i].PermanentPincode != "")
                        {
                            if (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != "")
                            {
                                if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()).Length == 6)))
                                {
                                    enq.AMCST_PerPincode = Convert.ToInt32(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //---End of the permanent address------//


                        //-------------------------Communication Address--------------------------//
                        //street
                        if (stu.newlstget1[i].PresentStreet != null && stu.newlstget1[i].PresentStreet != "")
                        {
                            if (stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\()\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMCST_ConStreet = stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Present Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Present Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //area
                        if (stu.newlstget1[i].PresentArea != null && stu.newlstget1[i].PresentArea != "")
                        {
                            if (stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\()\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentArea.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMCST_ConArea = stu.newlstget1[i].PresentArea.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Present Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            //else
                            //{
                            //    stu.stuStatus = "Present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            //    return stu;
                            //}
                        }

                        //city
                        if (stu.newlstget1[i].PresentCity != null && stu.newlstget1[i].PresentCity != "")
                        {
                            if (stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\'\,()\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentCity.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMCST_ConCity = stu.newlstget1[i].PresentCity.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "present city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            //else
                            //{
                            //    stu.stuStatus = "present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            //    return stu;
                            //}
                        }


                        //persent country
                        long countryid1 = 0;
                        if (stu.newlstget1[i].PresentCountry != null && stu.newlstget1[i].PresentCountry != "")
                        {
                            if (stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()));
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMCST_ConCountryId = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Present Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //present state
                        if (stu.newlstget1[i].PresentState != null && stu.newlstget1[i].PresentState != "")
                        {
                            if (stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentState.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentState.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].PresentState.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid1);
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMCST_ConState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "present state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //present Pincode 
                        if (stu.newlstget1[i].PresentPincode != null && stu.newlstget1[i].PresentPincode != "")
                        {
                            if (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != "")
                            {
                                if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()).Length == 6)))
                                {
                                    enq.AMCST_ConPincode = Convert.ToInt32(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Present Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //---End of the Communication address------//

                        ///-------------Aadhar Number -----------
                        if (stu.newlstget1[i].AadharNo != null && stu.newlstget1[i].AadharNo != "")
                        {
                            if (stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != null && stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$")) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0)) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
                                {
                                    enq.AMCST_AadharNo = Convert.ToInt64(stu.newlstget1[i].AadharNo.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Aadhar Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Aadhar Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //--- Student Mobile Number --//
                        if (stu.newlstget1[i].MobileNo != null && stu.newlstget1[i].MobileNo != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
                            {
                                string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                {
                                    enq.AMCST_MobileNo = Convert.ToInt64(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //--- Student Email id--//
                        if (stu.newlstget1[i].EmailID != null && stu.newlstget1[i].EmailID != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != ""))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(stu.newlstget1[i].EmailID.TrimEnd().TrimStart());
                                if (match.Success)
                                {
                                    enq.AMCST_emailId = stu.newlstget1[i].EmailID.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //----------------Father Details-------------//
                        if (stu.newlstget1[i].FatherName != null && stu.newlstget1[i].FatherName != "")
                        {
                            if ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != ""))
                            {
                                if ((Regex.IsMatch(stu.newlstget1[i].FatherName.TrimEnd().TrimStart(), @"^[a-zA-Z\-.\s]+$")) && ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart()).Length <= 50))
                                {
                                    enq.AMCST_FatherName = stu.newlstget1[i].FatherName.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    stu.stuStatus = "Father Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Father Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                return stu;
                            }

                        }

                        //Father Mobile no
                        if (stu.newlstget1[i].Fathermobileno != null && stu.newlstget1[i].Fathermobileno != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != ""))
                            {
                                string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                {
                                    enq.AMCST_FatherMobleNo = Convert.ToInt64(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Father Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Father Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //Father Email id
                        if (stu.newlstget1[i].FatherEmailId != null && stu.newlstget1[i].FatherEmailId != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != ""))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart());
                                if (match.Success)
                                {
                                    enq.AMCST_FatheremailId = stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Father Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Father Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //-------Mother Details ----------//

                        if (stu.newlstget1[i].MotherName != null && stu.newlstget1[i].MotherName != "")
                        {
                            if ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != ""))
                            {
                                if ((Regex.IsMatch(stu.newlstget1[i].MotherName.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart()).Length <= 50))
                                {
                                    enq.AMCST_MotherName = stu.newlstget1[i].MotherName.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    stu.stuStatus = "Mother Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mother Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                return stu;
                            }
                        }


                        //Father Mobile no
                        if (stu.newlstget1[i].MotherMobileNo != null && stu.newlstget1[i].MotherMobileNo != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != ""))
                            {
                                string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                {
                                    enq.AMCST_MotherMobleNo = Convert.ToInt64(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mother Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //Father Email id
                        if (stu.newlstget1[i].MotherEmailId != null && stu.newlstget1[i].MotherEmailId != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != ""))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart());
                                if (match.Success)
                                {
                                    enq.AMCST_MotheremailId = stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mother Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //long countryid12 = 0;
                        if (stu.newlstget1[i].StudentNationality != null && stu.newlstget1[i].StudentNationality != "")
                        {
                            if (stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != null && stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\:\,])*$")) && ((stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()).Length <= 50)))
                                {
                                    var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()));
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMCST_Nationality = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "student Nationality Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        enq.AMCST_SOL = "S";
                        enq.ACMB_Id = 1;
                        enq.ACST_Id = 1;
                        enq.ACSS_Id = _AdmissionFormContext.AdmCollegeSubjectSchemeDMO.Where(s => s.MI_Id == stu.MI_Id).Select(d => d.ACSS_Id).FirstOrDefault();
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;

                        //_AdmissionFormContext.Add(enq);
                        //var flag = _AdmissionFormContext.SaveChanges();
                        //if (flag >= 1)
                        //{
                        //    try
                        //    {
                        //        AdmCollegeStudentParentsEmailIdDMO enq_faheremail = new AdmCollegeStudentParentsEmailIdDMO();
                        //        enq_faheremail.ACSTPE_EmailId = enq.AMCST_FatheremailId;
                        //        enq_faheremail.AMCST_Id = enq.AMCST_Id;
                        //        enq_faheremail.ACSTPE_Flag = "F";
                        //        enq_faheremail.CreatedDate = DateTime.Now;
                        //        enq_faheremail.UpdatedDate = DateTime.Now;
                        //        _AdmissionFormContext.Add(enq_faheremail);
                        //        int n = _AdmissionFormContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        _acdimpl.LogInformation("Father emailid saving in new table" + ex.Message);
                        //    }

                        //    try
                        //    {
                        //        AdmCollegeStudentParentsEmailIdDMO enq_motheemail = new AdmCollegeStudentParentsEmailIdDMO();
                        //        enq_motheemail.ACSTPE_EmailId = enq.AMCST_MotheremailId;
                        //        enq_motheemail.AMCST_Id = enq.AMCST_Id;
                        //        enq_motheemail.ACSTPE_Flag = "M";
                        //        enq_motheemail.CreatedDate = DateTime.Now;
                        //        enq_motheemail.UpdatedDate = DateTime.Now;
                        //        _AdmissionFormContext.Add(enq_motheemail);
                        //        int n = _AdmissionFormContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        _acdimpl.LogInformation("Mother emailid saving in new table" + ex.Message);
                        //    }

                        //    try
                        //    {
                        //        AdmCollegeStudentParentsMobileNoDMO enq_mothermobile = new AdmCollegeStudentParentsMobileNoDMO();
                        //        enq_mothermobile.ACSTPMN_MobileNo = Convert.ToInt64(enq.AMCST_MotherMobleNo);
                        //        enq_mothermobile.AMCST_Id = enq.AMCST_Id;
                        //        enq_mothermobile.ACSTPMN_Flag = "M";
                        //        enq_mothermobile.CreatedDate = DateTime.Now;
                        //        enq_mothermobile.UpdatedDate = DateTime.Now;
                        //        _AdmissionFormContext.Add(enq_mothermobile);
                        //        int n2 = _AdmissionFormContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        _acdimpl.LogInformation("Mother Mobileno saving in new table" + ex.Message);
                        //    }

                        //    try
                        //    {
                        //        AdmCollegeStudentParentsMobileNoDMO enq_mothermobile = new AdmCollegeStudentParentsMobileNoDMO();
                        //        enq_mothermobile.ACSTPMN_MobileNo = Convert.ToInt64(enq.AMCST_FatherMobleNo);
                        //        enq_mothermobile.AMCST_Id = enq.AMCST_Id;
                        //        enq_mothermobile.ACSTPMN_Flag = "F";
                        //        enq_mothermobile.CreatedDate = DateTime.Now;
                        //        enq_mothermobile.UpdatedDate = DateTime.Now;
                        //        _AdmissionFormContext.Add(enq_mothermobile);
                        //        int n2 = _AdmissionFormContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        _acdimpl.LogInformation("Father Mobileno saving in new table" + ex.Message);
                        //    }
                        //    try
                        //    {
                        //        AdmCollegeStudentSMSNoDMO enq_studentmobile = new AdmCollegeStudentSMSNoDMO();
                        //        enq_studentmobile.AMCST_Id = enq.AMCST_Id;
                        //        enq_studentmobile.ACSTSMS_MobileNo = Convert.ToInt64(enq.AMCST_MobileNo);
                        //        enq_studentmobile.CreatedDate = DateTime.Now;
                        //        enq_studentmobile.UpdatedDate = DateTime.Now;
                        //        _DomainModelMsSqlServerContext.Add(enq_studentmobile);
                        //        int n4 = _DomainModelMsSqlServerContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        _acdimpl.LogInformation("Student Mobileno saving in new table" + ex.Message);
                        //    }
                        //    try
                        //    {
                        //        AdmCollegeStudentEmailIdDMO enq_studentemail = new AdmCollegeStudentEmailIdDMO();
                        //        enq_studentemail.AMCST_Id = enq.AMCST_Id;
                        //        enq_studentemail.ACSTE_EmailId = enq.AMCST_emailId;
                        //        enq_studentemail.CreatedDate = DateTime.Now;
                        //        enq_studentemail.UpdatedDate = DateTime.Now;
                        //        _DomainModelMsSqlServerContext.Add(enq_studentemail);
                        //        int n4 = _DomainModelMsSqlServerContext.SaveChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        _acdimpl.LogInformation("Student Email saving in new table" + ex.Message);

                        //    }

                        //    stu.stuStatus = "true";
                        //    sucesscount = sucesscount + 1;
                        //}
                        //else
                        //{
                        //    stu.stuStatus = "false";
                        //    failcount = failcount + 1;
                        //    string name = failnames;
                        //    finalnames += name + ",";
                        //    failedlist.Add(stu.newlstget1[i]);
                        //}

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        failcount = failcount + 1;
                        string name = failnames;
                        finalnames += name + ",";
                        failedlist.Add(stu.newlstget1[i]);
                        continue;
                    }
                }
                stu.failedlist = failedlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return stu;
        }
    }
}
