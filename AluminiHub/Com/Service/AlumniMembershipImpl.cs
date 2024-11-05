using AlumniHub.Com.Interface;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PreadmissionDTOs.com.vaps.Alumni.AlumniStudentDTO;

namespace AlumniHub.Com.Service
{
    public class AlumniMembershipImpl : AlumniMembershipInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, AlumniStudentDTO> _login =
      new ConcurrentDictionary<string, AlumniStudentDTO>();

        public AlumniContext _AlumniContext;
        private readonly DomainModelMsSqlServerContext _db;
        public AlumniMembershipImpl(AlumniContext AlumniContext, DomainModelMsSqlServerContext db)
        {
            _AlumniContext = AlumniContext;
            _db = db;
        }
        public AlumniStudentDTO Get_Intial_data(AlumniStudentDTO data)
        {
            try
            {
                data.flag = "false";
                data.rolename = _db.MasterRoleType.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;
                if (data.rolename.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag = "true";
                    var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                                   from b in _AlumniContext.Alumni_User_LoginDMO
                                   where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == data.userid)
                                   select new AlumniStudentDTO
                                   {
                                       ALMST_Id = Convert.ToInt64(a.ALMST_Id)
                                   }
          ).ToList();
                    data.ALMST_Id = details.FirstOrDefault().ALMST_Id;

                    getstudata(data);
                    data.alumnitrue = true;
                    data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
                                           where (a.ALMST_Id == data.ALMST_Id
                                        )
                                           select new AlumniStudentDTO
                                           {
                                               AMST_FirstName = ((a.ALMST_FirstName == null || a.ALMST_FirstName == "0" ? "" : a.ALMST_FirstName) + " " + (a.ALMST_MiddleName == null || a.ALMST_MiddleName == "0" ? "" : a.ALMST_MiddleName) + " " + (a.ALMST_LastName == null || a.ALMST_LastName == "0" ? "" : a.ALMST_LastName)).Trim(),

                                               AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == a.ASMCL_Id_Left).ASMCL_ClassName : "--",

                                               AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == a.ASMCL_Id_Join).ASMCL_ClassName : "--",

                                               AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id_Join).ASMAY_Year : "--",

                                               AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASMAY_Id_Left).ASMAY_Year : "--",

                                               ALMST_AdmNo = a.ALMST_AdmNo,
                                               ALMST_PhoneNo = a.ALMST_PhoneNo,
                                               ALMST_PerStreet = a.ALMST_PerStreet,
                                               ALMST_PerArea = a.ALMST_PerArea,
                                               ALMST_PerCountry = a.ALMST_ConCountryId,
                                               ALMST_ConState = a.ALMST_ConState,
                                               ALMST_ConStreet = a.ALMST_ConStreet,
                                               ALMST_ConArea = a.ALMST_ConArea,
                                               ALMST_FatherName = a.ALMST_FatherName,
                                               ALMST_DOB = a.ALMST_DOB,
                                               ALMST_MobileNo = a.ALMST_MobileNo,
                                               ALMST_emailId = a.ALMST_emailId,
                                               ALMST_ConCity = a.ALMST_ConCity,
                                               ALMST_PerPincode = a.ALMST_PerPincode,
                                               ALMST_ConPincode = a.ALMST_ConPincode,
                                               ALMST_BloodGroup = a.ALMST_BloodGroup,
                                               ALMST_Marital_Status = a.ALMST_Marital_Status,
                                               ALMST_StudentPhoto = a.ALMST_StudentPhoto,
                                               ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
                                               IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
                                               ASMAY_Id_Left = a.ASMAY_Id_Left,
                                               ASMAY_Id_Join = a.ASMAY_Id_Join,
                                               ASMCL_Id_Join = a.ASMCL_Id_Join,
                                               ASMCL_Id_Left = a.ASMCL_Id_Left,
                                               ALMST_Id = a.ALMST_Id,
                                               ALMST_SpouseName = a.ALMST_SpouseName,
                                               ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
                                               ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
                                               ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
                                               ALMST_SpouseProfession = a.ALMST_SpouseProfession,
                                               ALMST_NickName = a.ALMST_NickName,
                                               ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
                                               ALMST_MembershipId = a.ALMST_MembershipId,
                                               ALMST_FullAddess = a.ALMST_FullAddess,
                                               ALMST_AmountPaid = a.ALMST_AmountPaid,
                                               ALMST_MembershipCategory = a.ALMST_MembershipCategory,
                                           }

        ).Distinct().ToArray();

                    List<Country> allCountry = new List<Country>();
                    allCountry = _AlumniContext.Country.ToList();
                    data.countryDrpDown = allCountry.ToArray();

                    List<State> allstate = new List<State>();
                    allstate = _AlumniContext.state.ToList();
                    data.statedropdown = allstate.ToArray();
                    List<Alumni_M_StudentDMO> studentdet = new List<Alumni_M_StudentDMO>();
                    studentdet = (from a in _AlumniContext.Alumni_M_StudentDMO
                                  where (a.ALMST_Id == data.ALMST_Id
                               )
                                  select new Alumni_M_StudentDMO
                                  {
                                      ALMST_PerCountry = a.ALMST_ConCountryId,
                                      ALMST_ConState = a.ALMST_ConState
                                  }
       ).ToList();
                    if (studentdet.Count > 0)
                    {
                        data.ALMST_PerCountry = studentdet.FirstOrDefault().ALMST_PerCountry;
                        data.ALMST_ConState = studentdet.FirstOrDefault().ALMST_ConState;
                    }

                    if (data.ALMST_ConState > 0)
                    {
                        char[] trimChars = { '*', '@', ' ', ',' };

                        data.placelist = (from a in _AlumniContext.Alumni_M_StudentDMO
                                          where a.ALMST_ConState == data.ALMST_ConState && a.MI_Id == data.MI_Id
                                          && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
                                          select new AlumniStudentDTO
                                          {
                                              ALMST_ConCity = a.ALMST_ConCity.Trim(trimChars)
                                          }).Distinct().ToArray();
                    }
                    else
                    {
                        char[] trimChars = { '*', '@', ' ', ',' };

                        data.placelist = (from a in _AlumniContext.Alumni_M_StudentDMO
                                          where a.MI_Id == data.MI_Id
                                          && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
                                          select new AlumniStudentDTO
                                          {
                                              ALMST_ConCity = a.ALMST_ConCity.Trim(trimChars)
                                          }).Distinct().ToArray();
                        data.placelistqua = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
                                             where a.MI_Id == data.MI_Id && a.ALSQU_Place != null && a.ALSQU_Place != ""
                                             select new AlumniStudentDTO
                                             {
                                                 ALMST_ConCity = a.ALSQU_Place.Trim(trimChars)
                                             }).Distinct().ToArray();
                    }
                    char[] trimChars1 = { '*', '@', ' ', ',' };
                    data.placelistqua = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
                                         where a.MI_Id == data.MI_Id && a.ALSQU_Place != null && a.ALSQU_Place != ""
                                         select new AlumniStudentDTO
                                         {
                                             ALMST_ConCity = a.ALSQU_Place.Trim(trimChars1)
                                         }).Distinct().ToArray();
                    data.studentquali = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
                                             // from b in _AlumniContext.state
                                         where a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id
                                         // && a.IVRMMS_Id == b.IVRMMS_Id 
                                         select new AlumniStudentDTO
                                         {
                                             ALSQU_Qulification = a.ALSQU_Qulification,
                                             ALSQU_YearOfPassing = a.ALSQU_YearOfPassing,
                                             ALSQU_University = a.ALSQU_University,
                                             ALSQU_OtherDetails = a.ALSQU_OtherDetails,
                                             // IVRMMS_Name = b.IVRMMS_Name,
                                             ALMST_PerState = a.IVRMMS_Id,
                                             ALSQU_Place = a.ALSQU_Place,
                                             ALMST_Id = a.ALMST_Id,
                                             ALSQU_Percentage = a.ALSQU_Percentage
                                         }).ToArray();
                    data.studentprof = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

                    data.studentachive = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

                    List<MasterAcademic> year = new List<MasterAcademic>();
                    year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                    data.fillyear = year.ToArray();

                    List<School_M_Class> classname = new List<School_M_Class>();
                    classname = _db.admissioncls.ToList();
                    data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                }
                else
                {
                    List<MasterAcademic> year = new List<MasterAcademic>();
                    year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                    data.fillyear = year.ToArray();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Getstudentdetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                            data.almdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "alumni_student_details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = "Alumni" });

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
                            data.alumnilist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "alumni_student_details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = "AlumniNew" });

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
                            data.alumnilistnew = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    List<School_M_Class> classname = new List<School_M_Class>();
                    classname = _db.admissioncls.ToList();
                    data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                    data.alumnitrue = false;

                    List<Country> allCountry = new List<Country>();
                    allCountry = _AlumniContext.Country.ToList();
                    data.countryDrpDown = allCountry.ToArray();

                    List<State> allstate = new List<State>();
                    allstate = _AlumniContext.state.ToList();
                    data.statedropdown = allstate.ToArray();

                    List<DistrictDMO> alldistrict = new List<DistrictDMO>();
                    alldistrict = _AlumniContext.DistrictDMO.ToList();
                    data.districtdropdown = alldistrict.ToArray();
                }

                List<Alumni_Master_MembershipCategoryDMO> membcategry = new List<Alumni_Master_MembershipCategoryDMO>();
                membcategry = _AlumniContext.Alumni_Master_MembershipCategoryDMO.ToList();
                data.membercategory = membcategry.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<AlumniStudentDTO> Getstudentlist(AlumniStudentDTO data)
        {
            try
            {
                List<AlumniStudentDTO> details = new List<AlumniStudentDTO>();

                details = (from a in _AlumniContext.Alumni_M_StudentDMO
                           from b in _AlumniContext.AdmissionStudentDMO
                           where (a.AMST_ID == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id_Left == data.ASMAY_Id)
                           select new AlumniStudentDTO
                           {
                               AMST_ID = a.AMST_ID,
                           }
          ).ToList();

                List<long?> amstids = new List<long?>();
                foreach (var item in details)
                {
                    amstids.Add(item.AMST_ID);
                }


                data.stu_name = (from a in _AlumniContext.Alumni_M_StudentDMO
                                 from b in _AlumniContext.AdmissionStudentDMO
                                 where (a.AMST_ID == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id_Left == data.ASMAY_Id && amstids.Contains(b.AMST_Id))
                                 select new AlumniStudentDTO
                                 {
                                     AMST_ID = b.AMST_Id,
                                     AMST_FirstName = b.AMST_FirstName + ' ' + b.AMST_MiddleName + ' ' + b.AMST_LastName,
                                 }
          ).ToList().ToArray();

                data.fillstudent = (/*from a in _AlumniContext.AdmissionStudentDMO*/
                                   from a in _AlumniContext.Alumni_M_StudentDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id_Left == data.ASMAY_Id && a.ALMST_ActiveFlag == true)
                                   select new AlumniStudentDTO
                                   {
                                       ALMST_Id = a.ALMST_Id,
                                       AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper()) + ':' + a.ALMST_AdmNo).Trim(),
                                   }
          ).OrderBy(a => a.AMST_FirstName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<AlumniStudentDTO> Getstudentlistapp(AlumniStudentDTO data)
        {
            data.stu_name = (from a in _AlumniContext.AlumniUserRegistrationDMO
                             where (a.MI_Id == data.MI_Id && a.ALSREG_LeftYear == data.ASMAY_Id && a.ALSREG_ApprovedFlag == false)
                             select new AlumniStudentDTO
                             {
                                 membername = a.ALSREG_MemberName,
                                 ALSREG_Id = a.ALSREG_Id
                             }
                             ).ToArray();
            return data;
        }

        public AlumniStudentDTO checkstudent(AlumniStudentDTO data)
        {
            try
            {
                List<AlumniStudentDTO> abc = new List<AlumniStudentDTO>();

                abc = (from a in _AlumniContext.AlumniUserRegistrationDMO
                       where (a.MI_Id == data.MI_Id && a.ALSREG_Id == data.ALSREG_Id)
                       select new AlumniStudentDTO
                       {
                           membername = a.ALSREG_MemberName,
                           ASMCL_Id_Left = a.ALSREG_LeftClass,
                           ASMCL_Id_Join = a.ALSREG_AdmittedClass,
                           ASMAY_Id_Join = a.ALSREG_AdmittedYear,
                           ASMAY_Id_Left = a.ALSREG_LeftYear,
                           ALMST_Id = Convert.ToInt64(a.ALMST_Id),
                           ALSREG_Id = a.ALSREG_Id
                       }
                              ).ToList();

                if (abc.Count() > 0)
                {

                    string name = abc.FirstOrDefault().membername;

                    data.studentDetails = (from b in _AlumniContext.Alumni_M_StudentDMO
                                           where (((b.ALMST_FirstName == null || b.ALMST_FirstName == "0" ? "" : b.ALMST_FirstName).Trim().Contains(name)) ||
                                               ((b.ALMST_MiddleName == null || b.ALMST_MiddleName == "0" ? "" : b.ALMST_MiddleName).Trim().Contains(name)) || ((b.ALMST_LastName == null || b.ALMST_LastName == "0" ? "" : b.ALMST_LastName).Trim().Contains(name)) || ((b.ALMST_FirstName == null || b.ALMST_FirstName == "0" ? "" : b.ALMST_FirstName) + " " + (b.ALMST_MiddleName == null || b.ALMST_MiddleName == "0" ? "" : b.ALMST_MiddleName) + " " + (b.ALMST_LastName == null || b.ALMST_LastName == "0" ? "" : b.ALMST_LastName)).Trim().Contains(name))
                                           select new AlumniStudentDTO
                                           {
                                               membername = ((b.ALMST_FirstName == null || b.ALMST_FirstName == "0" ? "" : b.ALMST_FirstName) + " " + (b.ALMST_MiddleName == null || b.ALMST_MiddleName == "0" ? "" : b.ALMST_MiddleName) + " " + (b.ALMST_LastName == null || b.ALMST_LastName == "0" ? "" : b.ALMST_LastName)).Trim(),

                                               classnameadmitted = abc.FirstOrDefault().ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Where(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == abc.FirstOrDefault().ASMCL_Id_Join).FirstOrDefault().ASMCL_ClassName : "--",

                                               classnameleft = abc.FirstOrDefault().ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Where(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == abc.FirstOrDefault().ASMCL_Id_Left).FirstOrDefault().ASMCL_ClassName : "--",

                                               yeardmitted = abc.FirstOrDefault().ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Where(l => l.MI_Id == data.MI_Id && l.ASMAY_Id == abc.FirstOrDefault().ASMAY_Id_Join).FirstOrDefault().ASMAY_Year : "--",

                                               yearleft = abc.FirstOrDefault().ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Where(l => l.MI_Id == data.MI_Id && l.ASMAY_Id == abc.FirstOrDefault().ASMAY_Id_Left).FirstOrDefault().ASMAY_Year : "--",
                                               ALMST_Id = b.ALMST_Id,
                                               ALMST_AdmNo = b.ALMST_AdmNo,
                                               ALSREG_Id = abc.FirstOrDefault().ALSREG_Id

                                           }
                                               ).Distinct().ToArray();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public AlumniStudentDTO aproovedata(AlumniStudentDTO data)
        {
            try
            {
                if (data.APP_FLG == "Approval")
                {
                    foreach (var item in data.alumnistudentarray)
                    {
                        var rt = _AlumniContext.AlumniUserRegistrationDMO.Single(a => a.ALSREG_Id == item.ALSREG_Id);

                        Alumni_M_StudentDMO ams = new Alumni_M_StudentDMO();
                        ams.ALMST_FirstName = rt.ALSREG_MemberName;
                        ams.MI_Id = data.MI_Id;
                        ams.ALMST_StudentPhoto = rt.ALSREG_Photo;
                        ams.ALMST_emailId = rt.ALSREG_EmailId;
                        ams.ALMST_MobileNo = rt.ALSREG_MobileNo;
                        ams.ASMAY_Id_Join = rt.ALSREG_AdmittedYear;
                        ams.ALMST_AdmNo = rt.ALSREG_AdmissionNo;
                        ams.ASMAY_Id_Left = rt.ALSREG_LeftYear;
                        ams.ASMCL_Id_Join = rt.ALSREG_AdmittedClass;
                        ams.ASMCL_Id_Left = rt.ALSREG_LeftClass;
                        ams.ALMST_Date = DateTime.Today;
                        ams.ALMST_DOB = DateTime.Today;
                        ams.CreatedDate = DateTime.Today;
                        _AlumniContext.Add(ams);
                        //_AlumniContext.SaveChanges();
                        //var almstid = ams.ALMST_Id;

                        var trnspresltt = _AlumniContext.AlumniUserRegistrationDMO.Single(s => s.ALSREG_Id == item.ALSREG_Id);
                        trnspresltt.ALMST_Id = ams.ALMST_Id;
                        trnspresltt.ALSREG_ApprovedFlag = true;
                        trnspresltt.UpdatedDate = DateTime.Now;
                        _AlumniContext.Update(trnspresltt);
                    }
                    var suc = _AlumniContext.SaveChanges();
                    if (suc > 0)
                    {
                        data.returnval = "True";
                    }
                    else
                    {
                        data.returnval = "False";
                    }
                }
                else if (data.APP_FLG == "Search")
                {
                    if (data.ALSREG_Id > 0)
                    {
                        var trnspresltt = _AlumniContext.AlumniUserRegistrationDMO.Single(s => s.ALSREG_Id == data.ALSREG_Id);
                        trnspresltt.ALSREG_ApprovedFlag = true;
                        trnspresltt.ALMST_Id = data.ALMST_Id;
                        trnspresltt.UpdatedDate = DateTime.Now;
                        _AlumniContext.Update(trnspresltt);
                        var suc = _AlumniContext.SaveChanges();
                        if (suc > 0)
                        {
                            data.returnval = "True";
                        }
                        else
                        {
                            data.returnval = "False";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public AlumniStudentDTO searchfilter(AlumniStudentDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                data.fillstudent = (/*from a in _AlumniContext.AdmissionStudentDMO*/
                                    from a in _AlumniContext.Alumni_M_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id_Left == data.ASMAY_Id && ((a.ALMST_FirstName.Trim().ToUpper() + ' ' + a.ALMST_MiddleName.Trim().ToUpper() + ' ' + a.ALMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.ALMST_FirstName.Trim().ToUpper() + a.ALMST_MiddleName.Trim().ToUpper() + ' ' + a.ALMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.ALMST_FirstName.ToUpper().Contains(data.searchfilter) || a.ALMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.ALMST_LastName.ToUpper().Contains(data.searchfilter)))
                                    select new AlumniStudentDTO
                                    {
                                        ALMST_Id = a.ALMST_Id,
                                        AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper()) + ':' + a.ALMST_AdmNo).Trim(),
                                    }
           ).OrderBy(a => a.AMST_FirstName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public AlumniStudentDTO getstudata(AlumniStudentDTO data)
        {
            try
            {
                //var stucount = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Where(a => a.ALMST_Id == data.ALMST_Id).Count();
                //if (stucount > 0)
                //{
                //    data.studentCunt = stucount;
                //}



                long? ALMSTRADM_Id = 0;
                var stucount = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Where(a => a.ALMST_ID == data.ALMST_Id).Count();
                if (stucount >= 1)
                {
                    var stucount1 = (from a in _AlumniContext.Alumni_M_StudentDMO
                                     from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
                                     where a.ALMST_Id == b.ALMST_ID && a.ALMST_Id == data.ALMST_Id
                                     select a.ALMST_Id).Count();
                    data.studentCunt = stucount1 + stucount;


                    var sturedamit = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.ALMST_Id == data.ALMST_Id && a.ASMCL_Id_Left == data.ASMCL_Id_Left && a.ASMCL_Id_Join == data.ASMCL_Id_Join && a.ASMAY_Id_Join == data.ASMAY_Id_Join && a.ASMAY_Id_Left == data.ASMAY_Id_Left).Count();
                    if (sturedamit == 0)
                    {
                        var sturedamit_child = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Where(a => a.ALMST_ID == data.ALMST_Id && a.ALMSTRADM_ClassLeft == data.ASMCL_Id_Left && a.ALMSTRADM_ClassJoined == data.ASMCL_Id_Join && a.ALMSTRADM_YearLeft == data.ASMAY_Id_Left && a.ALMSTRADM_YearJoined == data.ASMAY_Id_Join).Count();
                        if (sturedamit_child > 0)
                        {
                            ALMSTRADM_Id = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Where(a => a.ALMST_ID == data.ALMST_Id && a.ALMSTRADM_ClassLeft == data.ASMCL_Id_Left && a.ALMSTRADM_ClassJoined == data.ASMCL_Id_Join && a.ALMSTRADM_YearLeft == data.ASMAY_Id_Left && a.ALMSTRADM_YearJoined == data.ASMAY_Id_Join).Select(b => b.ALMSTRADM_Id).FirstOrDefault();
                        }
                    }

                }
                else
                {

                    stucount = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.ALMST_Id == data.ALMST_Id).Count();
                    data.studentCunt = stucount;
                }

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _db.admissioncls.ToList();
                data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                if (ALMSTRADM_Id > 0 && data.studentCunt > 1)
                {

                    data.Readmitted_student = true;



                    if (data.ALMSTRADM_Id > 0 && data.studentCunt > 1)
                    {


                        data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
                                               from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
                                               where (a.ALMST_Id == data.ALMST_Id && b.ALMSTRADM_Id == ALMSTRADM_Id
                                            )
                                               select new AlumniStudentDTO
                                               {
                                                   AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

                                                   AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == b.ALMSTRADM_ClassLeft).ASMCL_ClassName : "--",


                                                   AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == b.ALMSTRADM_ClassJoined).ASMCL_ClassName : "--",


                                                   AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ALMSTRADM_YearJoined).ASMAY_Year : "--",


                                                   AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == b.ALMSTRADM_YearLeft).ASMAY_Year : "--",

                                                   ALMST_AdmNo = a.ALMST_AdmNo,
                                                   ALMST_PhoneNo = a.ALMST_PhoneNo,
                                                   ALMST_PerStreet = a.ALMST_PerStreet,
                                                   ALMST_PerArea = a.ALMST_PerArea,
                                                   ALMST_PerCountry = a.ALMST_ConCountryId,
                                                   ALMST_ConState = a.ALMST_ConState,
                                                   ALMST_ConStreet = a.ALMST_ConStreet,
                                                   ALMST_ConArea = a.ALMST_ConArea,
                                                   ALMST_FatherName = a.ALMST_FatherName,
                                                   ALMST_DOB = a.ALMST_DOB,
                                                   ALMST_MobileNo = a.ALMST_MobileNo,
                                                   ALMST_emailId = a.ALMST_emailId,
                                                   ALMST_ConCity = a.ALMST_ConCity,
                                                   ALMST_PerPincode = a.ALMST_PerPincode,
                                                   ALMST_ConPincode = a.ALMST_ConPincode,
                                                   ALMST_BloodGroup = a.ALMST_BloodGroup,
                                                   ALMST_Marital_Status = a.ALMST_Marital_Status,
                                                   ALMST_StudentPhoto = a.ALMST_StudentPhoto,
                                                   ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
                                                   IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
                                                   Readmitted_ASMAY_Id_Left = b.ALMSTRADM_YearLeft,
                                                   Readmitted_ASMAY_Id_Join = b.ALMSTRADM_YearJoined,
                                                   Readmitted_ASMCL_Id_Join = b.ALMSTRADM_ClassJoined,
                                                   Readmitted_ASMCL_Id_Left = b.ALMSTRADM_ClassLeft,
                                                   ASMAY_Id_Left = a.ASMAY_Id_Left,
                                                   ASMAY_Id_Join = a.ASMAY_Id_Join,
                                                   ASMCL_Id_Join = a.ASMCL_Id_Join,
                                                   ASMCL_Id_Left = a.ASMCL_Id_Left,
                                                   ALMST_Id = a.ALMST_Id,
                                                   ALMST_SpouseName = a.ALMST_SpouseName,
                                                   ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
                                                   ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
                                                   ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
                                                   ALMST_SpouseProfession = a.ALMST_SpouseProfession,
                                                   ALMST_NickName = a.ALMST_NickName,
                                                   ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
                                                   ALMST_MembershipId = a.ALMST_MembershipId,
                                                   ALMST_FullAddess = a.ALMST_FullAddess,
                                                   ALMST_AmountPaid = a.ALMST_AmountPaid,
                                                   ALMST_MembershipCategory = a.ALMST_MembershipCategory,
                                                   ALMST_PerDistrict = a.ALMST_PerDistrict
                                               }
 ).Distinct().ToArray();




                    }

                    //                  data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
                    //                                     from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
                    //                                     where (a.ALMST_Id == data.ALMST_Id && b.ALMSTRADM_Id == data.ALMSTRADM_Id
                    //                                  )
                    //                                     select new AlumniStudentDTO
                    //                                     {
                    //                                         AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

                    //                                         AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == b.ALMSTRADM_ClassLeft).ASMCL_ClassName : "--",


                    //                                         AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == b.ALMSTRADM_ClassJoined).ASMCL_ClassName : "--",


                    //                                         AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ALMSTRADM_YearJoined).ASMAY_Year : "--",


                    //                                         AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == b.ALMSTRADM_YearLeft).ASMAY_Year : "--",

                    //                                         ALMST_AdmNo = a.ALMST_AdmNo,
                    //                                         ALMST_PhoneNo = a.ALMST_PhoneNo,
                    //                                         ALMST_PerStreet = a.ALMST_PerStreet,
                    //                                         ALMST_PerArea = a.ALMST_PerArea,
                    //                                         ALMST_PerCountry = a.ALMST_ConCountryId,
                    //                                         ALMST_ConState = a.ALMST_ConState,
                    //                                         ALMST_ConStreet = a.ALMST_ConStreet,
                    //                                         ALMST_ConArea = a.ALMST_ConArea,
                    //                                         ALMST_FatherName = a.ALMST_FatherName,
                    //                                         ALMST_DOB = a.ALMST_DOB,
                    //                                         ALMST_MobileNo = a.ALMST_MobileNo,
                    //                                         ALMST_emailId = a.ALMST_emailId,
                    //                                         ALMST_ConCity = a.ALMST_ConCity,
                    //                                         ALMST_PerPincode = a.ALMST_PerPincode,
                    //                                         ALMST_ConPincode = a.ALMST_ConPincode,
                    //                                         ALMST_BloodGroup = a.ALMST_BloodGroup,
                    //                                         ALMST_Marital_Status = a.ALMST_Marital_Status,
                    //                                         ALMST_StudentPhoto = a.ALMST_StudentPhoto,
                    //                                         ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
                    //                                         IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
                    //                                         Readmitted_ASMAY_Id_Left = b.ALMSTRADM_YearLeft,
                    //                                         Readmitted_ASMAY_Id_Join = b.ALMSTRADM_YearJoined,
                    //                                         Readmitted_ASMCL_Id_Join = b.ALMSTRADM_ClassJoined,
                    //                                         Readmitted_ASMCL_Id_Left = b.ALMSTRADM_ClassLeft,
                    //                                         ASMAY_Id_Left = a.ASMAY_Id_Left,
                    //                                         ASMAY_Id_Join = a.ASMAY_Id_Join,
                    //                                         ASMCL_Id_Join = a.ASMCL_Id_Join,
                    //                                         ASMCL_Id_Left = a.ASMCL_Id_Left,
                    //                                         ALMST_Id = a.ALMST_Id,
                    //                                         ALMST_SpouseName = a.ALMST_SpouseName,
                    //                                         ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
                    //                                         ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
                    //                                         ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
                    //                                         ALMST_SpouseProfession = a.ALMST_SpouseProfession,
                    //                                         ALMST_NickName = a.ALMST_NickName,
                    //                                         ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
                    //                                         ALMST_MembershipId = a.ALMST_MembershipId,
                    //                                         ALMST_FullAddess = a.ALMST_FullAddess,
                    //                                         ALMST_AmountPaid = a.ALMST_AmountPaid,
                    //                                         ALMST_MembershipCategory = a.ALMST_MembershipCategory,
                    //                                         ALMST_PerDistrict = a.ALMST_PerDistrict
                    //                                     }
                    //).Distinct().ToArray();
                }
                else
                {
                    data.Readmitted_student = false;
                    data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
                                           where (a.ALMST_Id == data.ALMST_Id
                                        )
                                           select new AlumniStudentDTO
                                           {
                                               AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

                                               AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == a.ASMCL_Id_Left).ASMCL_ClassName : "--",


                                               AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == a.ASMCL_Id_Join).ASMCL_ClassName : "--",


                                               AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id_Join).ASMAY_Year : "--",


                                               AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASMAY_Id_Left).ASMAY_Year : "--",

                                               ALMST_AdmNo = a.ALMST_AdmNo,
                                               ALMST_PhoneNo = a.ALMST_PhoneNo,
                                               ALMST_PerStreet = a.ALMST_PerStreet,
                                               ALMST_PerArea = a.ALMST_PerArea,
                                               ALMST_PerCountry = a.ALMST_ConCountryId,
                                               ALMST_ConState = a.ALMST_ConState,
                                               ALMST_ConStreet = a.ALMST_ConStreet,
                                               ALMST_ConArea = a.ALMST_ConArea,
                                               ALMST_FatherName = a.ALMST_FatherName,
                                               ALMST_DOB = a.ALMST_DOB,
                                               ALMST_MobileNo = a.ALMST_MobileNo,
                                               ALMST_emailId = a.ALMST_emailId,
                                               ALMST_ConCity = a.ALMST_ConCity,
                                               ALMST_PerPincode = a.ALMST_PerPincode,
                                               ALMST_ConPincode = a.ALMST_ConPincode,
                                               ALMST_BloodGroup = a.ALMST_BloodGroup,
                                               ALMST_Marital_Status = a.ALMST_Marital_Status,
                                               ALMST_StudentPhoto = a.ALMST_StudentPhoto,
                                               ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
                                               IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
                                               ASMAY_Id_Left = a.ASMAY_Id_Left,
                                               ASMAY_Id_Join = a.ASMAY_Id_Join,
                                               ASMCL_Id_Join = a.ASMCL_Id_Join,
                                               ASMCL_Id_Left = a.ASMCL_Id_Left,
                                               ALMST_Id = a.ALMST_Id,
                                               ALMST_SpouseName = a.ALMST_SpouseName,
                                               ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
                                               ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
                                               ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
                                               ALMST_SpouseProfession = a.ALMST_SpouseProfession,
                                               ALMST_NickName = a.ALMST_NickName,
                                               ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
                                               ALMST_MembershipId = a.ALMST_MembershipId,
                                               ALMST_FullAddess = a.ALMST_FullAddess,
                                               ALMST_AmountPaid = a.ALMST_AmountPaid,
                                               ALMST_MembershipCategory = a.ALMST_MembershipCategory,
                                               ALMST_PerDistrict = a.ALMST_PerDistrict
                                           }
      ).Distinct().ToArray();
                }

                //        data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
                //                               where (a.ALMST_Id == data.ALMST_Id
                //                            )
                //                               select new AlumniStudentDTO
                //                               {
                //                                   AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

                //                                   AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == a.ASMCL_Id_Left).ASMCL_ClassName : "--",


                //                                   AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == a.ASMCL_Id_Join).ASMCL_ClassName : "--",


                //                                   AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id_Join).ASMAY_Year : "--",


                //                                   AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASMAY_Id_Left).ASMAY_Year : "--",

                //                                   ALMST_AdmNo = a.ALMST_AdmNo,
                //                                   ALMST_PhoneNo = a.ALMST_PhoneNo,
                //                                   ALMST_PerStreet = a.ALMST_PerStreet,
                //                                   ALMST_PerArea = a.ALMST_PerArea,
                //                                   ALMST_PerCountry = a.ALMST_ConCountryId,
                //                                   ALMST_ConState = a.ALMST_ConState,
                //                                   ALMST_ConStreet = a.ALMST_ConStreet,
                //                                   ALMST_ConArea = a.ALMST_ConArea,
                //                                   ALMST_FatherName = a.ALMST_FatherName,
                //                                   ALMST_DOB = a.ALMST_DOB,
                //                                   ALMST_MobileNo = a.ALMST_MobileNo,
                //                                   ALMST_emailId = a.ALMST_emailId,
                //                                   ALMST_ConCity = a.ALMST_ConCity,
                //                                   ALMST_PerPincode = a.ALMST_PerPincode,
                //                                   ALMST_ConPincode = a.ALMST_ConPincode,
                //                                   ALMST_BloodGroup = a.ALMST_BloodGroup,
                //                                   ALMST_Marital_Status = a.ALMST_Marital_Status,
                //                                   ALMST_StudentPhoto = a.ALMST_StudentPhoto,
                //                                   ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
                //                                   IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
                //                                   ASMAY_Id_Left = a.ASMAY_Id_Left,
                //                                   ASMAY_Id_Join = a.ASMAY_Id_Join,
                //                                   ASMCL_Id_Join = a.ASMCL_Id_Join,
                //                                   ASMCL_Id_Left = a.ASMCL_Id_Left,
                //                                   ALMST_Id = a.ALMST_Id,
                //                                   ALMST_SpouseName = a.ALMST_SpouseName,
                //                                   ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
                //                                   ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
                //                                   ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
                //                                   ALMST_SpouseProfession = a.ALMST_SpouseProfession,
                //                                   ALMST_NickName = a.ALMST_NickName,
                //                                   ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
                //                                   ALMST_MembershipId = a.ALMST_MembershipId,
                //                                   ALMST_FullAddess = a.ALMST_FullAddess,
                //                                   ALMST_AmountPaid = a.ALMST_AmountPaid,
                //                                   ALMST_MembershipCategory = a.ALMST_MembershipCategory,
                //                                   ALMST_PerDistrict = a.ALMST_PerDistrict
                //                               }
                //).Distinct().ToArray();

                data.studentquali = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
                                         // from b in _AlumniContext.state
                                     where a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id
                                     // && a.IVRMMS_Id == b.IVRMMS_Id 
                                     select new AlumniStudentDTO
                                     {
                                         ALSQU_Qulification = a.ALSQU_Qulification,
                                         ALSQU_YearOfPassing = a.ALSQU_YearOfPassing,
                                         ALSQU_University = a.ALSQU_University,
                                         ALSQU_OtherDetails = a.ALSQU_OtherDetails,
                                         // IVRMMS_Name = b.IVRMMS_Name,
                                         ALMST_PerState = a.IVRMMS_Id,
                                         ALSQU_Place = a.ALSQU_Place,
                                         ALMST_Id = a.ALMST_Id,
                                         ALSQU_Percentage = a.ALSQU_Percentage
                                     }).ToArray();

                data.studentprof = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

                data.studentachive = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

                List<Alumni_M_StudentDMO> studentdet = new List<Alumni_M_StudentDMO>();
                studentdet = (from a in _AlumniContext.Alumni_M_StudentDMO
                              where (a.ALMST_Id == data.ALMST_Id
                           )
                              select new Alumni_M_StudentDMO
                              {
                                  ALMST_PerCountry = a.ALMST_ConCountryId,
                                  ALMST_ConState = a.ALMST_ConState
                              }
       ).ToList();

                data.saveddata = (from a in _AlumniContext.Alumni_M_StudentDMO
                                  from b in _AlumniContext.Alumini_details
                                  where (b.ALMST_ID == data.ALMST_Id)
                                  select b).ToArray();

                data.ALMST_PerCountry = studentdet.FirstOrDefault().ALMST_PerCountry;
                data.ALMST_ConState = studentdet.FirstOrDefault().ALMST_ConState;


                List<Country> allCountry = new List<Country>();
                allCountry = _AlumniContext.Country.ToList();
                data.countryDrpDown = allCountry.ToArray();

                List<State> allstate = new List<State>();
                allstate = _AlumniContext.state.Where(t => t.IVRMMC_Id == studentdet.FirstOrDefault().ALMST_PerCountry).ToList();
                data.statedropdown = allstate.ToArray();

                char[] trimChars = { '*', '@', ' ', ',' };

                data.placelist = (from a in _AlumniContext.Alumni_M_StudentDMO
                                  where a.MI_Id == data.MI_Id && a.ALMST_ConState == data.ALMST_ConState && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
                                  select new AlumniStudentDTO
                                  {
                                      ALMST_ConCity = a.ALMST_ConCity.Trim(trimChars)
                                  }).Distinct().ToArray();

                data.placelistqua = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
                                     where a.MI_Id == data.MI_Id && a.ALSQU_Place != null && a.ALSQU_Place != ""
                                     select new AlumniStudentDTO
                                     {
                                         ALMST_ConCity = a.ALSQU_Place.Trim(trimChars)
                                     }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //     public AlumniStudentDTO getstudata(AlumniStudentDTO data)
        //     {
        //         try
        //         {
        //             //var stucount = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.ALMST_Id == data.ALMST_Id).Count();
        //             //if (stucount > 0)
        //             //{
        //             //    data.studentCunt = stucount;
        //             //}


        //             var stucount = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Where(a => a.ALMST_ID == data.ALMST_Id).Count();
        //             if (stucount >= 1)
        //             {
        //                 var stucount1 = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                                  from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
        //                                  where a.ALMST_Id == b.ALMST_ID && a.ALMST_Id == data.ALMST_Id
        //                                  select a.ALMST_Id).Count();
        //                 data.studentCunt = stucount1 + stucount;
        //             }
        //             else
        //             {
        //                 data.studentCunt = stucount;
        //             }

        //             List<School_M_Class> classname = new List<School_M_Class>();
        //             classname = _db.admissioncls.ToList();
        //             data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();

        //             List<MasterAcademic> year = new List<MasterAcademic>();
        //             year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).OrderByDescending(t => t.ASMAY_Order).ToList();
        //             data.fillyear = year.ToArray();

        //             if (data.ALMSTRADM_Id > 0 || data.studentCunt > 1)
        //             {

        //                 data.Readmitted_student = true;

        //                 if (data.ALMSTRADM_Id > 0 && data.studentCunt == 1)
        //                 {
        //                     data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                                            from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
        //                                            where (a.ALMST_Id == data.ALMST_Id && b.ALMSTRADM_Id == data.ALMSTRADM_Id
        //                                         )
        //                                            select new AlumniStudentDTO
        //                                            {
        //                                                AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

        //                                                AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == b.ALMSTRADM_ClassLeft).ASMCL_ClassName : "--",


        //                                                AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == b.ALMSTRADM_ClassJoined).ASMCL_ClassName : "--",


        //                                                AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ALMSTRADM_YearJoined).ASMAY_Year : "--",


        //                                                AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == b.ALMSTRADM_YearLeft).ASMAY_Year : "--",

        //                                                ALMST_AdmNo = a.ALMST_AdmNo,
        //                                                ALMST_PhoneNo = a.ALMST_PhoneNo,
        //                                                ALMST_PerStreet = a.ALMST_PerStreet,
        //                                                ALMST_PerArea = a.ALMST_PerArea,
        //                                                ALMST_PerCountry = a.ALMST_ConCountryId,
        //                                                ALMST_ConState = a.ALMST_ConState,
        //                                                ALMST_ConStreet = a.ALMST_ConStreet,
        //                                                ALMST_ConArea = a.ALMST_ConArea,
        //                                                ALMST_FatherName = a.ALMST_FatherName,
        //                                                ALMST_DOB = a.ALMST_DOB,
        //                                                ALMST_MobileNo = a.ALMST_MobileNo,
        //                                                ALMST_emailId = a.ALMST_emailId,
        //                                                ALMST_ConCity = a.ALMST_ConCity,
        //                                                ALMST_PerPincode = a.ALMST_PerPincode,
        //                                                ALMST_ConPincode = a.ALMST_ConPincode,
        //                                                ALMST_BloodGroup = a.ALMST_BloodGroup,
        //                                                ALMST_Marital_Status = a.ALMST_Marital_Status,
        //                                                ALMST_StudentPhoto = a.ALMST_StudentPhoto,
        //                                                ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
        //                                                IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
        //                                                Readmitted_ASMAY_Id_Left = b.ALMSTRADM_YearLeft,
        //                                                Readmitted_ASMAY_Id_Join = b.ALMSTRADM_YearJoined,
        //                                                Readmitted_ASMCL_Id_Join = b.ALMSTRADM_ClassJoined,
        //                                                Readmitted_ASMCL_Id_Left = b.ALMSTRADM_ClassLeft,
        //                                                ASMAY_Id_Left = a.ASMAY_Id_Left,
        //                                                ASMAY_Id_Join = a.ASMAY_Id_Join,
        //                                                ASMCL_Id_Join = a.ASMCL_Id_Join,
        //                                                ASMCL_Id_Left = a.ASMCL_Id_Left,
        //                                                ALMST_Id = a.ALMST_Id,
        //                                                ALMST_SpouseName = a.ALMST_SpouseName,
        //                                                ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
        //                                                ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
        //                                                ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
        //                                                ALMST_SpouseProfession = a.ALMST_SpouseProfession,
        //                                                ALMST_NickName = a.ALMST_NickName,
        //                                                ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
        //                                                ALMST_MembershipId = a.ALMST_MembershipId,
        //                                                ALMST_FullAddess = a.ALMST_FullAddess,
        //                                                ALMST_AmountPaid = a.ALMST_AmountPaid,
        //                                                ALMST_MembershipCategory = a.ALMST_MembershipCategory,
        //                                                ALMST_PerDistrict = a.ALMST_PerDistrict
        //                                            }
        //  ).Distinct().ToArray();
        //                 }
        //                 else
        //                 {
        //                     data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                                            from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
        //                                            where (a.ALMST_Id == b.ALMST_ID && a.ALMST_Id == data.ALMST_Id
        //                                         )
        //                                            select new AlumniStudentDTO
        //                                            {
        //                                                AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

        //                                                AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == b.ALMSTRADM_ClassLeft).ASMCL_ClassName : "--",


        //                                                AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == b.ALMSTRADM_ClassJoined).ASMCL_ClassName : "--",


        //                                                AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ALMSTRADM_YearJoined).ASMAY_Year : "--",


        //                                                AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == b.ALMSTRADM_YearLeft).ASMAY_Year : "--",

        //                                                ALMST_AdmNo = a.ALMST_AdmNo,
        //                                                ALMST_PhoneNo = a.ALMST_PhoneNo,
        //                                                ALMST_PerStreet = a.ALMST_PerStreet,
        //                                                ALMST_PerArea = a.ALMST_PerArea,
        //                                                ALMST_PerCountry = a.ALMST_ConCountryId,
        //                                                ALMST_ConState = a.ALMST_ConState,
        //                                                ALMST_ConStreet = a.ALMST_ConStreet,
        //                                                ALMST_ConArea = a.ALMST_ConArea,
        //                                                ALMST_FatherName = a.ALMST_FatherName,
        //                                                ALMST_DOB = a.ALMST_DOB,
        //                                                ALMST_MobileNo = a.ALMST_MobileNo,
        //                                                ALMST_emailId = a.ALMST_emailId,
        //                                                ALMST_ConCity = a.ALMST_ConCity,
        //                                                ALMST_PerPincode = a.ALMST_PerPincode,
        //                                                ALMST_ConPincode = a.ALMST_ConPincode,
        //                                                ALMST_BloodGroup = a.ALMST_BloodGroup,
        //                                                ALMST_Marital_Status = a.ALMST_Marital_Status,
        //                                                ALMST_StudentPhoto = a.ALMST_StudentPhoto,
        //                                                ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
        //                                                IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
        //                                                Readmitted_ASMAY_Id_Left = b.ALMSTRADM_YearLeft,
        //                                                Readmitted_ASMAY_Id_Join = b.ALMSTRADM_YearJoined,
        //                                                Readmitted_ASMCL_Id_Join = b.ALMSTRADM_ClassJoined,
        //                                                Readmitted_ASMCL_Id_Left = b.ALMSTRADM_ClassLeft,
        //                                                ASMAY_Id_Left = a.ASMAY_Id_Left,
        //                                                ASMAY_Id_Join = a.ASMAY_Id_Join,
        //                                                ASMCL_Id_Join = a.ASMCL_Id_Join,
        //                                                ASMCL_Id_Left = a.ASMCL_Id_Left,
        //                                                ALMST_Id = a.ALMST_Id,
        //                                                ALMST_SpouseName = a.ALMST_SpouseName,
        //                                                ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
        //                                                ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
        //                                                ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
        //                                                ALMST_SpouseProfession = a.ALMST_SpouseProfession,
        //                                                ALMST_NickName = a.ALMST_NickName,
        //                                                ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
        //                                                ALMST_MembershipId = a.ALMST_MembershipId,
        //                                                ALMST_FullAddess = a.ALMST_FullAddess,
        //                                                ALMST_AmountPaid = a.ALMST_AmountPaid,
        //                                                ALMST_MembershipCategory = a.ALMST_MembershipCategory,
        //                                                ALMST_PerDistrict = a.ALMST_PerDistrict
        //                                            }
        //).Distinct().ToArray();
        //                 }
        //                 //                  data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                 //                                     from b in _AlumniContext.Alumni_Master_Student_ReadmitDMO
        //                 //                                     where (a.ALMST_Id == data.ALMST_Id && b.ALMSTRADM_Id == data.ALMSTRADM_Id
        //                 //                                  )
        //                 //                                     select new AlumniStudentDTO
        //                 //                                     {
        //                 //                                         AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

        //                 //                                         AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == b.ALMSTRADM_ClassLeft).ASMCL_ClassName : "--",


        //                 //                                         AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == b.ALMSTRADM_ClassJoined).ASMCL_ClassName : "--",


        //                 //                                         AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == b.ALMSTRADM_YearJoined).ASMAY_Year : "--",


        //                 //                                         AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == b.ALMSTRADM_YearLeft).ASMAY_Year : "--",

        //                 //                                         ALMST_AdmNo = a.ALMST_AdmNo,
        //                 //                                         ALMST_PhoneNo = a.ALMST_PhoneNo,
        //                 //                                         ALMST_PerStreet = a.ALMST_PerStreet,
        //                 //                                         ALMST_PerArea = a.ALMST_PerArea,
        //                 //                                         ALMST_PerCountry = a.ALMST_ConCountryId,
        //                 //                                         ALMST_ConState = a.ALMST_ConState,
        //                 //                                         ALMST_ConStreet = a.ALMST_ConStreet,
        //                 //                                         ALMST_ConArea = a.ALMST_ConArea,
        //                 //                                         ALMST_FatherName = a.ALMST_FatherName,
        //                 //                                         ALMST_DOB = a.ALMST_DOB,
        //                 //                                         ALMST_MobileNo = a.ALMST_MobileNo,
        //                 //                                         ALMST_emailId = a.ALMST_emailId,
        //                 //                                         ALMST_ConCity = a.ALMST_ConCity,
        //                 //                                         ALMST_PerPincode = a.ALMST_PerPincode,
        //                 //                                         ALMST_ConPincode = a.ALMST_ConPincode,
        //                 //                                         ALMST_BloodGroup = a.ALMST_BloodGroup,
        //                 //                                         ALMST_Marital_Status = a.ALMST_Marital_Status,
        //                 //                                         ALMST_StudentPhoto = a.ALMST_StudentPhoto,
        //                 //                                         ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
        //                 //                                         IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
        //                 //                                         Readmitted_ASMAY_Id_Left = b.ALMSTRADM_YearLeft,
        //                 //                                         Readmitted_ASMAY_Id_Join = b.ALMSTRADM_YearJoined,
        //                 //                                         Readmitted_ASMCL_Id_Join = b.ALMSTRADM_ClassJoined,
        //                 //                                         Readmitted_ASMCL_Id_Left = b.ALMSTRADM_ClassLeft,
        //                 //                                         ASMAY_Id_Left = a.ASMAY_Id_Left,
        //                 //                                         ASMAY_Id_Join = a.ASMAY_Id_Join,
        //                 //                                         ASMCL_Id_Join = a.ASMCL_Id_Join,
        //                 //                                         ASMCL_Id_Left = a.ASMCL_Id_Left,
        //                 //                                         ALMST_Id = a.ALMST_Id,
        //                 //                                         ALMST_SpouseName = a.ALMST_SpouseName,
        //                 //                                         ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
        //                 //                                         ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
        //                 //                                         ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
        //                 //                                         ALMST_SpouseProfession = a.ALMST_SpouseProfession,
        //                 //                                         ALMST_NickName = a.ALMST_NickName,
        //                 //                                         ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
        //                 //                                         ALMST_MembershipId = a.ALMST_MembershipId,
        //                 //                                         ALMST_FullAddess = a.ALMST_FullAddess,
        //                 //                                         ALMST_AmountPaid = a.ALMST_AmountPaid,
        //                 //                                         ALMST_MembershipCategory = a.ALMST_MembershipCategory,
        //                 //                                         ALMST_PerDistrict = a.ALMST_PerDistrict
        //                 //                                     }
        //                 //).Distinct().ToArray();
        //             }
        //             else
        //             {
        //                 data.Readmitted_student = false;
        //                 data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                                        where (a.ALMST_Id == data.ALMST_Id
        //                                     )
        //                                        select new AlumniStudentDTO
        //                                        {
        //                                            AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

        //                                            AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == a.ASMCL_Id_Left).ASMCL_ClassName : "--",


        //                                            AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == a.ASMCL_Id_Join).ASMCL_ClassName : "--",


        //                                            AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id_Join).ASMAY_Year : "--",


        //                                            AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASMAY_Id_Left).ASMAY_Year : "--",

        //                                            ALMST_AdmNo = a.ALMST_AdmNo,
        //                                            ALMST_PhoneNo = a.ALMST_PhoneNo,
        //                                            ALMST_PerStreet = a.ALMST_PerStreet,
        //                                            ALMST_PerArea = a.ALMST_PerArea,
        //                                            ALMST_PerCountry = a.ALMST_ConCountryId,
        //                                            ALMST_ConState = a.ALMST_ConState,
        //                                            ALMST_ConStreet = a.ALMST_ConStreet,
        //                                            ALMST_ConArea = a.ALMST_ConArea,
        //                                            ALMST_FatherName = a.ALMST_FatherName,
        //                                            ALMST_DOB = a.ALMST_DOB,
        //                                            ALMST_MobileNo = a.ALMST_MobileNo,
        //                                            ALMST_emailId = a.ALMST_emailId,
        //                                            ALMST_ConCity = a.ALMST_ConCity,
        //                                            ALMST_PerPincode = a.ALMST_PerPincode,
        //                                            ALMST_ConPincode = a.ALMST_ConPincode,
        //                                            ALMST_BloodGroup = a.ALMST_BloodGroup,
        //                                            ALMST_Marital_Status = a.ALMST_Marital_Status,
        //                                            ALMST_StudentPhoto = a.ALMST_StudentPhoto,
        //                                            ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
        //                                            IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
        //                                            ASMAY_Id_Left = a.ASMAY_Id_Left,
        //                                            ASMAY_Id_Join = a.ASMAY_Id_Join,
        //                                            ASMCL_Id_Join = a.ASMCL_Id_Join,
        //                                            ASMCL_Id_Left = a.ASMCL_Id_Left,
        //                                            ALMST_Id = a.ALMST_Id,
        //                                            ALMST_SpouseName = a.ALMST_SpouseName,
        //                                            ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
        //                                            ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
        //                                            ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
        //                                            ALMST_SpouseProfession = a.ALMST_SpouseProfession,
        //                                            ALMST_NickName = a.ALMST_NickName,
        //                                            ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
        //                                            ALMST_MembershipId = a.ALMST_MembershipId,
        //                                            ALMST_FullAddess = a.ALMST_FullAddess,
        //                                            ALMST_AmountPaid = a.ALMST_AmountPaid,
        //                                            ALMST_MembershipCategory = a.ALMST_MembershipCategory,
        //                                            ALMST_PerDistrict = a.ALMST_PerDistrict
        //                                        }
        //   ).Distinct().ToArray();
        //             }

        //             //        data.studentDetails = (from a in _AlumniContext.Alumni_M_StudentDMO
        //             //                               where (a.ALMST_Id == data.ALMST_Id
        //             //                            )
        //             //                               select new AlumniStudentDTO
        //             //                               {
        //             //                                   AMST_FirstName = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName.ToUpper()) + " " + (a.ALMST_MiddleName == null ? "" : a.ALMST_MiddleName.ToUpper()) + " " + (a.ALMST_LastName == null ? "" : a.ALMST_LastName.ToUpper())),

        //             //                                   AMST_CLASS_LEFT = a.ASMCL_Id_Left != 0 ? _AlumniContext.School_M_Class.Single(l => l.MI_Id == data.MI_Id && l.ASMCL_Id == a.ASMCL_Id_Left).ASMCL_ClassName : "--",


        //             //                                   AMST_CLASS_JOIN = a.ASMCL_Id_Join != 0 ? _AlumniContext.School_M_Class.Single(ld => ld.MI_Id == data.MI_Id && ld.ASMCL_Id == a.ASMCL_Id_Join).ASMCL_ClassName : "--",


        //             //                                   AMST_JOIN_YEAR = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id_Join).ASMAY_Year : "--",


        //             //                                   AMST_JOIN_LEFT = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASMAY_Id_Left).ASMAY_Year : "--",

        //             //                                   ALMST_AdmNo = a.ALMST_AdmNo,
        //             //                                   ALMST_PhoneNo = a.ALMST_PhoneNo,
        //             //                                   ALMST_PerStreet = a.ALMST_PerStreet,
        //             //                                   ALMST_PerArea = a.ALMST_PerArea,
        //             //                                   ALMST_PerCountry = a.ALMST_ConCountryId,
        //             //                                   ALMST_ConState = a.ALMST_ConState,
        //             //                                   ALMST_ConStreet = a.ALMST_ConStreet,
        //             //                                   ALMST_ConArea = a.ALMST_ConArea,
        //             //                                   ALMST_FatherName = a.ALMST_FatherName,
        //             //                                   ALMST_DOB = a.ALMST_DOB,
        //             //                                   ALMST_MobileNo = a.ALMST_MobileNo,
        //             //                                   ALMST_emailId = a.ALMST_emailId,
        //             //                                   ALMST_ConCity = a.ALMST_ConCity,
        //             //                                   ALMST_PerPincode = a.ALMST_PerPincode,
        //             //                                   ALMST_ConPincode = a.ALMST_ConPincode,
        //             //                                   ALMST_BloodGroup = a.ALMST_BloodGroup,
        //             //                                   ALMST_Marital_Status = a.ALMST_Marital_Status,
        //             //                                   ALMST_StudentPhoto = a.ALMST_StudentPhoto,
        //             //                                   ALMST_FamilyPhoto = a.ALMST_FamilyPhoto,
        //             //                                   IVRMMC_Id = Convert.ToInt64(a.ALMST_ConCountryId),
        //             //                                   ASMAY_Id_Left = a.ASMAY_Id_Left,
        //             //                                   ASMAY_Id_Join = a.ASMAY_Id_Join,
        //             //                                   ASMCL_Id_Join = a.ASMCL_Id_Join,
        //             //                                   ASMCL_Id_Left = a.ASMCL_Id_Left,
        //             //                                   ALMST_Id = a.ALMST_Id,
        //             //                                   ALMST_SpouseName = a.ALMST_SpouseName,
        //             //                                   ALMST_SpouseContactNo = a.ALMST_SpouseContactNo,
        //             //                                   ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
        //             //                                   ALMST_SpouseQulaification = a.ALMST_SpouseQulaification,
        //             //                                   ALMST_SpouseProfession = a.ALMST_SpouseProfession,
        //             //                                   ALMST_NickName = a.ALMST_NickName,
        //             //                                   ALMST_WeddingAnniversaryDate = a.ALMST_WeddingAnniversaryDate,
        //             //                                   ALMST_MembershipId = a.ALMST_MembershipId,
        //             //                                   ALMST_FullAddess = a.ALMST_FullAddess,
        //             //                                   ALMST_AmountPaid = a.ALMST_AmountPaid,
        //             //                                   ALMST_MembershipCategory = a.ALMST_MembershipCategory,
        //             //                                   ALMST_PerDistrict = a.ALMST_PerDistrict
        //             //                               }
        //             //).Distinct().ToArray();

        //             data.studentquali = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
        //                                      // from b in _AlumniContext.state
        //                                  where a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id
        //                                  // && a.IVRMMS_Id == b.IVRMMS_Id 
        //                                  select new AlumniStudentDTO
        //                                  {
        //                                      ALSQU_Qulification = a.ALSQU_Qulification,
        //                                      ALSQU_YearOfPassing = a.ALSQU_YearOfPassing,
        //                                      ALSQU_University = a.ALSQU_University,
        //                                      ALSQU_OtherDetails = a.ALSQU_OtherDetails,
        //                                      // IVRMMS_Name = b.IVRMMS_Name,
        //                                      ALMST_PerState = a.IVRMMS_Id,
        //                                      ALSQU_Place = a.ALSQU_Place,
        //                                      ALMST_Id = a.ALMST_Id,
        //                                      ALSQU_Percentage = a.ALSQU_Percentage
        //                                  }).ToArray();

        //             data.studentprof = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

        //             data.studentachive = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == data.ALMST_Id && a.MI_Id == data.MI_Id).ToArray();

        //             List<Alumni_M_StudentDMO> studentdet = new List<Alumni_M_StudentDMO>();
        //             studentdet = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                           where (a.ALMST_Id == data.ALMST_Id
        //                        )
        //                           select new Alumni_M_StudentDMO
        //                           {
        //                               ALMST_PerCountry = a.ALMST_ConCountryId,
        //                               ALMST_ConState = a.ALMST_ConState
        //                           }
        //    ).ToList();

        //             data.saveddata = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                               from b in _AlumniContext.Alumini_details
        //                               where (b.ALMST_ID == data.ALMST_Id)
        //                               select b).ToArray();

        //             data.ALMST_PerCountry = studentdet.FirstOrDefault().ALMST_PerCountry;
        //             data.ALMST_ConState = studentdet.FirstOrDefault().ALMST_ConState;


        //             List<Country> allCountry = new List<Country>();
        //             allCountry = _AlumniContext.Country.ToList();
        //             data.countryDrpDown = allCountry.ToArray();

        //             List<State> allstate = new List<State>();
        //             allstate = _AlumniContext.state.Where(t => t.IVRMMC_Id == studentdet.FirstOrDefault().ALMST_PerCountry).ToList();
        //             data.statedropdown = allstate.ToArray();

        //             char[] trimChars = { '*', '@', ' ', ',' };

        //             data.placelist = (from a in _AlumniContext.Alumni_M_StudentDMO
        //                               where a.MI_Id == data.MI_Id && a.ALMST_ConState == data.ALMST_ConState && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
        //                               select new AlumniStudentDTO
        //                               {
        //                                   ALMST_ConCity = a.ALMST_ConCity.Trim(trimChars)
        //                               }).Distinct().ToArray();

        //             data.placelistqua = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
        //                                  where a.MI_Id == data.MI_Id && a.ALSQU_Place != null && a.ALSQU_Place != ""
        //                                  select new AlumniStudentDTO
        //                                  {
        //                                      ALMST_ConCity = a.ALSQU_Place.Trim(trimChars)
        //                                  }).Distinct().ToArray();

        //         }
        //         catch (Exception ex)
        //         {
        //             Console.WriteLine(ex.Message);
        //         }
        //         return data;
        //     }

        public AlumniStudentDTO svedata(AlumniStudentDTO sddto)
        {
            try
            {

                var aluministudet = (from a in _AlumniContext.Alumni_M_StudentDMO
                                     where (a.ALMST_Id == sddto.ALMST_Id)
                                     select new AlumniStudentDTO
                                     {
                                         ALMST_Id = a.ALMST_Id,
                                     }
       ).ToList();
                var alureg = _AlumniContext.AlumniUserRegistrationDMO.Where(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id).ToList();
                if (aluministudet.Count() == 0)
                {

                    var resultalumi = _AlumniContext.Alumni_M_StudentDMO.Single(t => t.ALMST_Id == sddto.ALMST_Id);
                    resultalumi.ALMST_DOB = sddto.ALMST_DOB;
                    resultalumi.ALMST_MobileNo = sddto.ALMST_MobileNo;
                    resultalumi.AMST_ID = sddto.AMST_ID;
                    resultalumi.ALMST_FatherName = sddto.ALMST_FatherName;
                    resultalumi.ALMST_PhoneNo = sddto.ALMST_PhoneNo;
                    resultalumi.ALMST_emailId = sddto.ALMST_emailId;
                    resultalumi.ALMST_BloodGroup = sddto.ALMST_BloodGroup;
                    resultalumi.ALMST_Marital_Status = sddto.ALMST_Marital_Status;
                    resultalumi.ALMST_ConCountryId = sddto.ALMST_PerCountry;
                    resultalumi.IVRMMC_Id = sddto.ALMST_PerCountry;
                    resultalumi.ALMST_ConState = sddto.ALMST_PerState;
                    resultalumi.ALMST_PerDistrict = sddto.ALMST_PerDistrict;
                    resultalumi.ALMST_ConCity = sddto.ALMST_ConCity;
                    resultalumi.ALMST_ConArea = sddto.ALMST_ConArea;
                    resultalumi.ALMST_ConStreet = sddto.ALMST_ConStreet;
                    resultalumi.ALMST_StudentPhoto = sddto.ALMST_StudentPhoto;
                    resultalumi.ALMST_FamilyPhoto = sddto.ALMST_FamilyPhoto;
                    resultalumi.ALMST_ConPincode = sddto.ALMST_ConPincode;
                    resultalumi.ALMST_AdmNo = sddto.ALMST_AdmNo;
                    resultalumi.ASMAY_Id_Join = sddto.ASMAY_Id_Join;
                    resultalumi.ASMAY_Id_Left = sddto.ASMAY_Id_Left;
                    resultalumi.ASMCL_Id_Join = sddto.ASMCL_Id_Join;
                    resultalumi.ASMCL_Id_Left = sddto.ASMCL_Id_Left;
                    resultalumi.ALMST_NickName = sddto.ALMST_NickName;
                    resultalumi.ALMST_SpouseName = sddto.ALMST_SpouseName;
                    resultalumi.ALMST_SpouseContactNo = sddto.ALMST_SpouseContactNo;
                    resultalumi.ALMST_SpouseEmailId = sddto.ALMST_SpouseEmailId;
                    resultalumi.ALMST_SpouseQulaification = sddto.ALMST_SpouseQulaification;
                    resultalumi.ALMST_SpouseProfession = sddto.ALMST_SpouseProfession;
                    resultalumi.ALMST_WeddingAnniversaryDate = sddto.ALMST_WeddingAnniversaryDate;
                    resultalumi.ALMST_MembershipId = sddto.ALMST_MembershipId;
                    resultalumi.ALMST_MembershipCategory = sddto.ALMST_MembershipCategory;
                    resultalumi.ALMST_AmountPaid = sddto.ALMST_AmountPaid;
                    resultalumi.ALMST_FullAddess = sddto.ALMST_FullAddess;
                    resultalumi.ALMST_PerDistrict = sddto.ALMST_PerDistrict;
                    _AlumniContext.Update(resultalumi);
                    if (sddto.qualification_array[0].ALMST_PUC_PASSED_OUT != 0 && sddto.qualification_array[0].ALMST_PUC_INS_NAME != null || sddto.qualification_array != null)
                    {
                        var checkdata = _AlumniContext.Alumni_Student_Qulaification_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id).ToList();
                        if (checkdata.Count > 0)
                        {
                            foreach (var item in checkdata)
                            {
                                var result = _AlumniContext.Alumni_Student_Qulaification_DMO_con.Single(a => a.ALMST_Id == item.ALMST_Id && a.MI_Id == sddto.MI_Id && a.ALSQU_Id == item.ALSQU_Id);
                                _AlumniContext.Remove(result);
                                _AlumniContext.SaveChanges();
                            }
                        }

                        foreach (var item in sddto.qualification_array)
                        {
                            Alumni_Student_Qulaification_DMO sq = new Alumni_Student_Qulaification_DMO();
                            sq.MI_Id = sddto.MI_Id;
                            sq.ALSQU_Qulification = item.Qualification;
                            sq.ALSQU_YearOfPassing = item.ALMST_PUC_PASSED_OUT;
                            sq.ALSQU_University = item.ALMST_PUC_INS_NAME;
                            sq.ALSQU_OtherDetails = item.ALSQU_OtherDetails;
                            sq.ALSQU_Percentage = item.ALSQU_Percentage;
                            sq.ALSQU_ActiveFlg = true;
                            sq.ALMST_Id = sddto.ALMST_Id;
                            sq.IVRMMS_Id = Convert.ToInt64(item.ALMST_PerState);
                            sq.ALSQU_Place = item.ALMST_PLACE;
                            sq.CreatedDate = DateTime.Now;
                            _AlumniContext.Add(sq);
                        }
                    }
                    if (sddto.ALSPR_CompanyName != null)
                    {
                        var pamst = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id).ToList();
                        if (pamst.Count > 0)
                        {
                            var sp = _AlumniContext.Alumni_Student_Profession_DMO_con.Single(a => a.ALMST_Id == sddto.ALMST_Id);
                            sp.MI_Id = sddto.MI_Id;
                            sp.ALSPR_CompanyName = sddto.ALSPR_CompanyName;
                            sp.ALSPR_CompanyAddress = sddto.ALSPR_CompanyAddress;
                            sp.ALSPR_Designation = sddto.ALSPR_Designation;
                            sp.ALSPR_EmailId = sddto.ALSPR_EmailId;
                            sp.ALSPR_WorkingSince = sddto.ALSPR_WorkingSince;
                            sp.ALSPR_OtherDetails = sddto.ALSPR_OtherDetails;
                            sp.ALMST_Id = sddto.ALMST_Id;
                            sp.UpdatedDate = DateTime.Now;
                            _AlumniContext.Update(sp);
                        }
                        else
                        {
                            Alumni_Student_Profession_DMO sp = new Alumni_Student_Profession_DMO();
                            sp.MI_Id = sddto.MI_Id;
                            sp.ALSPR_CompanyName = sddto.ALSPR_CompanyName;
                            sp.ALSPR_CompanyAddress = sddto.ALSPR_CompanyAddress;
                            sp.ALSPR_Designation = sddto.ALSPR_Designation;
                            sp.ALSPR_EmailId = sddto.ALSPR_EmailId;
                            sp.ALSPR_WorkingSince = sddto.ALSPR_WorkingSince;
                            sp.ALSPR_OtherDetails = sddto.ALSPR_OtherDetails;
                            sp.ALMST_Id = sddto.ALMST_Id;
                            sp.ALSPR_ActiveFlg = true;
                            sp.CreatedDate = DateTime.Now;
                            _AlumniContext.Add(sp);
                        }
                    }
                    if (sddto.ALSAC_Achievement != null)
                    {
                        var ache = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id).ToList();
                        if (ache.Count > 0)
                        {
                            var sa = _AlumniContext.Alumni_Student_Achivement_DMO_con.Single(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id);

                            sa.MI_Id = sddto.MI_Id;
                            sa.ALMST_Id = sddto.ALMST_Id;
                            sa.ALSAC_Achievement = sddto.ALSAC_Achievement;
                            sa.ALSAC_Remarks = sddto.ALSAC_Remarks;
                            sa.ALSAC_ActiveFlg = true;
                            sa.UpdatedDate = DateTime.Now;
                            _AlumniContext.Update(sa);
                        }
                        else
                        {
                            Alumni_Student_Achivement_DMO sa = new Alumni_Student_Achivement_DMO();
                            sa.MI_Id = sddto.MI_Id;
                            sa.ALMST_Id = sddto.ALMST_Id;
                            sa.ALSAC_Achievement = sddto.ALSAC_Achievement;
                            sa.ALSAC_Remarks = sddto.ALSAC_Remarks;
                            sa.ALSAC_ActiveFlg = true;
                            sa.CreatedDate = DateTime.Now;
                            _AlumniContext.Add(sa);
                        }
                    }


                    var contactExistzs = _AlumniContext.SaveChanges();

                    if (sddto.ALMST_emailId != "" || sddto.ALMST_emailId != null)
                    {
                        using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Alumni_Email_Update";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                            {
                                Value = Convert.ToString(sddto.ALMST_emailId)
                            });
                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(sddto.userid)
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
                    if (sddto.ALMST_MobileNo != 0)
                    {
                        using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Alumni_Mobile_Update";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar)
                            {
                                Value = sddto.ALMST_MobileNo
                            });
                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(sddto.userid)
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

                    if (contactExistzs > 0)
                    {
                        sddto.returnval = "Records updated Successfully";
                        if (sddto.Readmitted_student == true)
                        {
                            Readmitted_svedata(sddto);
                        }
                    }
                    else
                    {
                        sddto.returnval = "Records not updated Successfully";
                    }

                }

                else
                {

                    var resultalumi = _AlumniContext.Alumni_M_StudentDMO.Single(t => t.ALMST_Id == sddto.ALMST_Id);

                    resultalumi.ALMST_DOB = sddto.ALMST_DOB;
                    resultalumi.ALMST_MobileNo = sddto.ALMST_MobileNo;
                    resultalumi.AMST_ID = sddto.AMST_ID;
                    resultalumi.ALMST_FatherName = sddto.ALMST_FatherName;
                    resultalumi.ALMST_PhoneNo = sddto.ALMST_PhoneNo;
                    resultalumi.ALMST_emailId = sddto.ALMST_emailId;
                    resultalumi.ALMST_BloodGroup = sddto.ALMST_BloodGroup;
                    resultalumi.ALMST_Marital_Status = sddto.ALMST_Marital_Status;
                    resultalumi.ALMST_ConCountryId = sddto.ALMST_PerCountry;
                    resultalumi.ALMST_PerDistrict = sddto.ALMST_PerDistrict;
                    resultalumi.IVRMMC_Id = sddto.ALMST_PerCountry;
                    resultalumi.ALMST_ConState = sddto.ALMST_PerState;
                    resultalumi.ALMST_ConCity = sddto.ALMST_ConCity;
                    resultalumi.ALMST_ConArea = sddto.ALMST_ConArea;
                    resultalumi.ALMST_ConStreet = sddto.ALMST_ConStreet;
                    resultalumi.ALMST_StudentPhoto = sddto.ALMST_StudentPhoto;
                    resultalumi.ALMST_FamilyPhoto = sddto.ALMST_FamilyPhoto;
                    resultalumi.ALMST_ConPincode = sddto.ALMST_ConPincode;
                    resultalumi.ALMST_AdmNo = sddto.ALMST_AdmNo;
                    resultalumi.ASMAY_Id_Join = sddto.ASMAY_Id_Join;
                    resultalumi.ASMAY_Id_Left = sddto.ASMAY_Id_Left;
                    resultalumi.ASMCL_Id_Join = sddto.ASMCL_Id_Join;
                    resultalumi.ASMCL_Id_Left = sddto.ASMCL_Id_Left;
                    resultalumi.ALMST_NickName = sddto.ALMST_NickName;
                    resultalumi.ALMST_SpouseName = sddto.ALMST_SpouseName;
                    resultalumi.ALMST_SpouseContactNo = sddto.ALMST_SpouseContactNo;
                    resultalumi.ALMST_SpouseEmailId = sddto.ALMST_SpouseEmailId;
                    resultalumi.ALMST_SpouseQulaification = sddto.ALMST_SpouseQulaification;
                    resultalumi.ALMST_SpouseProfession = sddto.ALMST_SpouseProfession;
                    resultalumi.ALMST_WeddingAnniversaryDate = sddto.ALMST_WeddingAnniversaryDate;
                    resultalumi.ALMST_MembershipId = sddto.ALMST_MembershipId;
                    resultalumi.ALMST_MembershipCategory = sddto.ALMST_MembershipCategory;
                    resultalumi.ALMST_AmountPaid = sddto.ALMST_AmountPaid;
                    resultalumi.ALMST_FullAddess = sddto.ALMST_FullAddess;
                    resultalumi.ALMST_PerDistrict = sddto.ALMST_PerDistrict;
                    _AlumniContext.Update(resultalumi);

                    if (sddto.qualification_array[0].ALMST_PUC_PASSED_OUT != 0 && sddto.qualification_array[0].ALMST_PUC_INS_NAME != null || sddto.qualification_array != null)
                    {
                        var checkdata = _AlumniContext.Alumni_Student_Qulaification_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id).ToList();
                        if (checkdata.Count > 0)
                        {
                            foreach (var item in checkdata)
                            {
                                var result = _AlumniContext.Alumni_Student_Qulaification_DMO_con.Single(a => a.ALMST_Id == item.ALMST_Id && a.MI_Id == sddto.MI_Id && a.ALSQU_Id == item.ALSQU_Id);
                                _AlumniContext.Remove(result);
                                _AlumniContext.SaveChanges();
                            }
                        }



                        foreach (var item in sddto.qualification_array)
                        {
                            Alumni_Student_Qulaification_DMO sq = new Alumni_Student_Qulaification_DMO();
                            sq.MI_Id = sddto.MI_Id;
                            sq.ALSQU_Qulification = item.Qualification;
                            sq.ALSQU_YearOfPassing = item.ALMST_PUC_PASSED_OUT;
                            sq.ALSQU_University = item.ALMST_PUC_INS_NAME;
                            sq.ALSQU_OtherDetails = item.ALSQU_OtherDetails;
                            sq.ALSQU_Percentage = item.ALSQU_Percentage;
                            sq.ALSQU_ActiveFlg = true;
                            sq.ALMST_Id = sddto.ALMST_Id;
                            sq.IVRMMS_Id = Convert.ToInt64(item.ALMST_PerState);
                            sq.ALSQU_Place = item.ALMST_PLACE;
                            sq.CreatedDate = DateTime.Now;
                            _AlumniContext.Add(sq);
                        }
                    }
                    if (sddto.ALSPR_CompanyName != null)
                    {
                        var pamst = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id).ToList();
                        if (pamst.Count > 0)
                        {
                            var sp = _AlumniContext.Alumni_Student_Profession_DMO_con.Single(a => a.ALMST_Id == sddto.ALMST_Id);
                            sp.MI_Id = sddto.MI_Id;
                            sp.ALSPR_CompanyName = sddto.ALSPR_CompanyName;
                            sp.ALSPR_CompanyAddress = sddto.ALSPR_CompanyAddress;
                            sp.ALSPR_Designation = sddto.ALSPR_Designation;
                            sp.ALSPR_EmailId = sddto.ALSPR_EmailId;
                            sp.ALSPR_WorkingSince = sddto.ALSPR_WorkingSince;
                            sp.ALSPR_OtherDetails = sddto.ALSPR_OtherDetails;
                            sp.ALMST_Id = sddto.ALMST_Id;
                            sp.UpdatedDate = DateTime.Now;
                            _AlumniContext.Update(sp);
                        }
                        else
                        {
                            Alumni_Student_Profession_DMO sp = new Alumni_Student_Profession_DMO();
                            sp.MI_Id = sddto.MI_Id;
                            sp.ALSPR_CompanyName = sddto.ALSPR_CompanyName;
                            sp.ALSPR_CompanyAddress = sddto.ALSPR_CompanyAddress;
                            sp.ALSPR_Designation = sddto.ALSPR_Designation;
                            sp.ALSPR_EmailId = sddto.ALSPR_EmailId;
                            sp.ALSPR_WorkingSince = sddto.ALSPR_WorkingSince;
                            sp.ALSPR_OtherDetails = sddto.ALSPR_OtherDetails;
                            sp.ALMST_Id = sddto.ALMST_Id;
                            sp.ALSPR_ActiveFlg = true;
                            sp.CreatedDate = DateTime.Now;
                            _AlumniContext.Add(sp);
                        }
                    }
                    if (sddto.ALSAC_Achievement != null)
                    {
                        var ache = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id).ToList();
                        if (ache.Count > 0)
                        {
                            var sa = _AlumniContext.Alumni_Student_Achivement_DMO_con.Single(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id);

                            sa.MI_Id = sddto.MI_Id;
                            sa.ALMST_Id = sddto.ALMST_Id;
                            sa.ALSAC_Achievement = sddto.ALSAC_Achievement;
                            sa.ALSAC_Remarks = sddto.ALSAC_Remarks;
                            sa.ALSAC_ActiveFlg = true;
                            sa.UpdatedDate = DateTime.Now;
                            _AlumniContext.Update(sa);
                        }
                        else
                        {
                            Alumni_Student_Achivement_DMO sa = new Alumni_Student_Achivement_DMO();
                            sa.MI_Id = sddto.MI_Id;
                            sa.ALMST_Id = sddto.ALMST_Id;
                            sa.ALSAC_Achievement = sddto.ALSAC_Achievement;
                            sa.ALSAC_Remarks = sddto.ALSAC_Remarks;
                            sa.ALSAC_ActiveFlg = true;
                            sa.CreatedDate = DateTime.Now;
                            _AlumniContext.Add(sa);
                        }
                    }
                    if (sddto.ALMST_emailId != "" || sddto.ALMST_emailId != null)
                    {
                        using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Alumni_Email_Update";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                            {
                                Value = Convert.ToString(sddto.ALMST_emailId)
                            });
                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(sddto.userid)
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
                    if (sddto.ALMST_MobileNo != 0)
                    {
                        using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Alumni_Mobile_Update";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar)
                            {
                                Value = sddto.ALMST_MobileNo
                            });
                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(sddto.userid)
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

                    var contactExistzs = _AlumniContext.SaveChanges();
                    sddto.ALMST_Id = resultalumi.ALMST_Id;
                    if (contactExistzs > 0)
                    {
                        sddto.returnval = "Records updated Successfully";
                        if (sddto.Readmitted_student == true)
                        {
                            Readmitted_svedata(sddto);
                        }
                    }
                    else
                    {
                        sddto.returnval = "Records not updated Successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sddto;
        }

        public AlumniStudentDTO svedatanewalumni(AlumniStudentDTO sddto)
        {
            try
            {
                Alumni_M_StudentDMO resultalumi = new Alumni_M_StudentDMO();
                resultalumi.ALMST_FirstName = sddto.ALMST_FirstName;
                resultalumi.ALMST_MiddleName = sddto.ALMST_MiddleName;
                resultalumi.ALMST_LastName = sddto.ALMST_LastName;
                resultalumi.ALMST_DOB = sddto.ALMST_DOB;
                resultalumi.ALMST_NickName = sddto.ALMST_NickName;
                resultalumi.ALMST_FatherName = sddto.ALMST_FatherName;
                resultalumi.ALMST_MobileNo = sddto.ALMST_MobileNo;
                resultalumi.ALMST_AdmNo = sddto.ALMST_AdmNo;
                resultalumi.ALMST_ConCountryId = sddto.ALMST_ConCountryId;
                resultalumi.IVRMMC_Id = sddto.ALMST_ConCountryId;
                resultalumi.ALMST_ConState = sddto.ALMST_ConState;
                resultalumi.ALMST_ConCity = sddto.ALMST_ConCity;
                resultalumi.ALMST_ConArea = sddto.ALMST_ConArea;
                resultalumi.ALMST_ConStreet = sddto.ALMST_ConStreet;
                resultalumi.ALMST_Marital_Status = sddto.ALMST_Marital_Status;
                resultalumi.ASMAY_Id_Join = sddto.ASMAY_Id_Join;
                resultalumi.ASMAY_Id_Left = sddto.ASMAY_Id_Left;
                resultalumi.ASMCL_Id_Join = sddto.ASMCL_Id_Join;
                resultalumi.ASMCL_Id_Left = sddto.ASMCL_Id_Left;
                resultalumi.ALMST_PhoneNo = sddto.ALMST_PhoneNo;
                resultalumi.ALMST_emailId = sddto.ALMST_emailId;
                resultalumi.ALMST_ConPincode = sddto.ALMST_ConPincode;
                resultalumi.ALMST_StudentPhoto = sddto.ALMST_StudentPhoto;
                resultalumi.ALMST_FamilyPhoto = sddto.ALMST_FamilyPhoto;
                resultalumi.ALMST_BloodGroup = sddto.ALMST_BloodGroup;
                resultalumi.ALMST_NickName = sddto.ALMST_NickName;
                resultalumi.ALMST_SpouseName = sddto.ALMST_SpouseName;
                resultalumi.ALMST_SpouseContactNo = sddto.ALMST_SpouseContactNo;
                resultalumi.ALMST_SpouseEmailId = sddto.ALMST_SpouseEmailId;
                resultalumi.ALMST_SpouseQulaification = sddto.ALMST_SpouseQulaification;
                resultalumi.ALMST_SpouseProfession = sddto.ALMST_SpouseProfession;
                resultalumi.ALMST_ActiveFlag = true;
                resultalumi.ALMST_Date = DateTime.Today;
                resultalumi.CreatedDate = DateTime.Today;
                resultalumi.MI_Id = sddto.MI_Id;
                _AlumniContext.Add(resultalumi);
                var contactExistzs = _AlumniContext.SaveChanges();
                sddto.ALMST_Id = resultalumi.ALMST_Id;

                if (sddto.qualification_array[0].ALMST_PUC_PASSED_OUT != 0 && sddto.qualification_array[0].ALMST_PUC_INS_NAME != null)
                {
                    foreach (var item in sddto.qualification_array)
                    {
                        Alumni_Student_Qulaification_DMO sq = new Alumni_Student_Qulaification_DMO();
                        sq.MI_Id = sddto.MI_Id;
                        sq.ALSQU_Qulification = item.Qualification;
                        sq.ALSQU_YearOfPassing = item.ALMST_PUC_PASSED_OUT;
                        sq.ALSQU_University = item.ALMST_PUC_INS_NAME;
                        sq.ALSQU_OtherDetails = item.ALSQU_OtherDetails;
                        sq.ALSQU_Percentage = item.ALSQU_Percentage;
                        sq.ALSQU_ActiveFlg = true;
                        sq.ALMST_Id = sddto.ALMST_Id;
                        sq.IVRMMS_Id = Convert.ToInt64(item.ALMST_PerState);
                        sq.ALSQU_Place = item.ALMST_PLACE;
                        sq.CreatedDate = DateTime.Now;
                        _AlumniContext.Add(sq);
                    }
                }
                if (sddto.ALSPR_CompanyName != null)
                {
                    var pamst = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id).ToList();
                    if (pamst.Count > 0)
                    {
                        var sp = _AlumniContext.Alumni_Student_Profession_DMO_con.Single(a => a.ALMST_Id == sddto.ALMST_Id);
                        sp.MI_Id = sddto.MI_Id;
                        sp.ALSPR_CompanyName = sddto.ALSPR_CompanyName;
                        sp.ALSPR_CompanyAddress = sddto.ALSPR_CompanyAddress;
                        sp.ALSPR_Designation = sddto.ALSPR_Designation;
                        sp.ALSPR_EmailId = sddto.ALSPR_EmailId;
                        sp.ALSPR_WorkingSince = sddto.ALSPR_WorkingSince;
                        sp.ALSPR_OtherDetails = sddto.ALSPR_OtherDetails;
                        sp.ALMST_Id = sddto.ALMST_Id;
                        sp.UpdatedDate = DateTime.Now;
                        _AlumniContext.Update(sp);
                    }
                    else
                    {
                        Alumni_Student_Profession_DMO sp = new Alumni_Student_Profession_DMO();
                        sp.MI_Id = sddto.MI_Id;
                        sp.ALSPR_CompanyName = sddto.ALSPR_CompanyName;
                        sp.ALSPR_CompanyAddress = sddto.ALSPR_CompanyAddress;
                        sp.ALSPR_Designation = sddto.ALSPR_Designation;
                        sp.ALSPR_EmailId = sddto.ALSPR_EmailId;
                        sp.ALSPR_WorkingSince = sddto.ALSPR_WorkingSince;
                        sp.ALSPR_OtherDetails = sddto.ALSPR_OtherDetails;
                        sp.ALMST_Id = sddto.ALMST_Id;
                        sp.ALSPR_ActiveFlg = true;
                        sp.CreatedDate = DateTime.Now;
                        _AlumniContext.Add(sp);
                    }
                }
                if (sddto.ALSAC_Achievement != null)
                {
                    var ache = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id).ToList();
                    if (ache.Count > 0)
                    {
                        var sa = _AlumniContext.Alumni_Student_Achivement_DMO_con.Single(a => a.ALMST_Id == sddto.ALMST_Id && a.MI_Id == sddto.MI_Id);

                        sa.MI_Id = sddto.MI_Id;
                        sa.ALMST_Id = sddto.ALMST_Id;
                        sa.ALSAC_Achievement = sddto.ALSAC_Achievement;
                        sa.ALSAC_Remarks = sddto.ALSAC_Remarks;
                        sa.ALSAC_ActiveFlg = true;
                        sa.UpdatedDate = DateTime.Now;
                        _AlumniContext.Update(sa);
                    }
                    else
                    {
                        Alumni_Student_Achivement_DMO sa = new Alumni_Student_Achivement_DMO();
                        sa.MI_Id = sddto.MI_Id;
                        sa.ALMST_Id = sddto.ALMST_Id;
                        sa.ALSAC_Achievement = sddto.ALSAC_Achievement;
                        sa.ALSAC_Remarks = sddto.ALSAC_Remarks;
                        sa.ALSAC_ActiveFlg = true;
                        sa.CreatedDate = DateTime.Now;
                        _AlumniContext.Add(sa);
                    }
                }


                var contactExistzs4 = _AlumniContext.SaveChanges();

                if (sddto.ALMST_emailId != "" || sddto.ALMST_emailId != null)
                {
                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Alumni_Email_Update";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                        {
                            Value = Convert.ToString(sddto.ALMST_emailId)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(sddto.userid)
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
                if (sddto.ALMST_MobileNo != 0)
                {
                    using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Alumni_Mobile_Update";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar)
                        {
                            Value = sddto.ALMST_MobileNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(sddto.userid)
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
                if (contactExistzs > 0 || contactExistzs4 > 0)
                {
                    sddto.returnval = "Records updated Successfully";
                }
                else
                {
                    sddto.returnval = "Records not updated Successfully";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sddto;
        }

        public AlumniStudentDTO onchangecountry(AlumniStudentDTO sddto)
        {
            try
            {
                List<State> allstate = new List<State>();
                allstate = _AlumniContext.state.Where(t => t.IVRMMC_Id == sddto.ALMST_PerCountry).ToList();
                sddto.statedropdown = allstate.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sddto;
        }

        public AlumniStudentDTO onchangedistrict(AlumniStudentDTO sddto)
        {
            try
            {
                List<DistrictDMO> alldistrict = new List<DistrictDMO>();
                alldistrict = _AlumniContext.DistrictDMO.Where(t => t.IVRMMS_Id == sddto.ALMST_PerCountry).ToList();
                sddto.districtdropdown = alldistrict.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sddto;
        }



        public AlumniStudentDTO viewData(AlumniStudentDTO dto)
        {
            try
            {
                dto.qualification = _AlumniContext.Alumni_Student_Qulaification_DMO_con.Where(a => a.ALMST_Id == dto.ALMST_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();

                dto.profession = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.ALMST_Id == dto.ALMST_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();

                dto.achivement = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.ALMST_Id == dto.ALMST_Id && a.MI_Id == dto.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public AlumniStudentDTO onchangestate(AlumniStudentDTO dto)
        {
            try
            {
                char[] trimChars = { '*', '@', ' ', ',' };

                dto.placelist = (from a in _AlumniContext.Alumni_M_StudentDMO
                                 where a.ALMST_ConState == dto.ALMST_PerState && a.MI_Id == dto.MI_Id
                                  && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
                                 select new AlumniStudentDTO
                                 {
                                     ALMST_ConCity = a.ALMST_ConCity.Trim()
                                 }).Distinct().ToArray();

                dto.placelistqua = (from a in _AlumniContext.Alumni_Student_Qulaification_DMO_con
                                    where a.MI_Id == dto.MI_Id && a.ALSQU_Place != null && a.ALSQU_Place != ""
                                    select new AlumniStudentDTO
                                    {
                                        ALMST_ConCity = a.ALSQU_Place.Trim()
                                    }).Distinct().ToArray();

                //dto.placelistqua = (from a in _AlumniContext.Alumni_M_StudentDMO
                //                    where a.ALMST_ConState == dto.ALMST_PerState && a.MI_Id == dto.MI_Id
                //                     && a.ALMST_ConCity != null && a.ALMST_ConCity != ""
                //                    select new AlumniStudentDTO
                //                    {
                //                        ALMST_ConCity = a.ALMST_ConCity.Trim()
                //                    }).Distinct().ToArray();

                dto.districtdropdown = _AlumniContext.DistrictDMO.Where(a => a.IVRMMS_Id == dto.ALMST_PerState).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        //membership id generate
        public string GenerateNumber(AlumniStudentDTO dto)
        {
            List<AlumniStudentDTO> hr = new List<AlumniStudentDTO>();
            string GeneratedNumber = "";

            var almi = "ALU";

            int lastfieldnew = 0;
            using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "FindLastValues";
                cmd.CommandType = CommandType.StoredProcedure;

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            hr.Add(new AlumniStudentDTO
                            {

                                ALMST_MembershipId = (dataReader["ALMST_MembershipId"]).ToString(),

                            });
                        }
                    }
                    dto.almst = hr[0].ALMST_MembershipId.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (dto.almst == null || dto.almst == "0")
            {
                lastfieldnew = 1;
            }
            else
            {
                string[] lastRecordArray = dto.almst.Split('/');
                if (lastRecordArray != null)
                {
                    string firstfield = lastRecordArray.ElementAt(0);
                    string staringNumber = lastRecordArray.ElementAt(1);
                    int lastfield = Convert.ToInt32(lastRecordArray.ElementAt(2));
                    lastfieldnew = lastfield + 1;


                }
            }

            var result = _AlumniContext.Master_Numbering.Where(a => a.MI_Id == dto.MI_Id && a.IMN_Flag == "Alumni").ToList();
            GeneratedNumber = almi + "/" + dto.ALMST_Id + "/" + lastfieldnew.ToString().PadLeft(Convert.ToInt32(result[0].IMN_WidthNumeric) - 0, '0');

            return GeneratedNumber;

        }

        public AlumniStudentDTO deactive(AlumniStudentDTO dto)
        {
            try
            {
                if (dto.ALMST_ActiveFlag == false)
                {
                    var result = _AlumniContext.Alumni_M_StudentDMO.Single(a => a.ALMST_Id == dto.ALMST_Id && a.MI_Id == dto.MI_Id);
                    result.ALMST_ActiveFlag = true;
                    _AlumniContext.Update(result);
                    var con = _AlumniContext.SaveChanges();

                    if (con > 0)
                    {
                        dto.returnval = "true";
                    }
                    else
                    {
                        dto.returnval = "error";
                    }
                }
                else
                {
                    var result = _AlumniContext.Alumni_M_StudentDMO.Single(a => a.ALMST_Id == dto.ALMST_Id && a.MI_Id == dto.MI_Id);
                    result.ALMST_ActiveFlag = false;
                    _AlumniContext.Update(result);
                    var con = _AlumniContext.SaveChanges();

                    if (con > 0)
                    {
                        dto.returnval = "true";
                    }
                    else
                    {
                        dto.returnval = "error";
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public void AlumniWedding(int id) // scheduler for all client
        {
            try
            {

                MI_ID = id;

                SendEmail(MI_ID);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        public string SendEmail(int MI_ID)
        {
            string re = "";
            long trnsno = 0;
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var que1 = (from a in _AlumniContext.Alumni_M_StudentDMO
                            where a.MI_Id == MI_ID && Convert.ToDateTime(a.ALMST_WeddingAnniversaryDate) == Convert.ToDateTime(System.DateTime.Today.Date)
                            select new AlumniStudentDTO
                            {
                                ALMST_Id = a.ALMST_Id,
                                ALMST_emailId = a.ALMST_emailId,
                                ALMST_SpouseEmailId = a.ALMST_SpouseEmailId,
                                ALMST_MobileNo = a.ALMST_MobileNo,
                                ALMST_SpouseContactNo = a.ALMST_SpouseContactNo
                            }).Distinct().ToList();
                if (que1.Count > 0)
                {

                    for (int i = 0; i < que1.Count; i++)
                    {
                        try
                        {
                            long id = que1[i].ALMST_Id;
                            string email = Convert.ToString(que1[i].ALMST_emailId);
                            string email2 = Convert.ToString(que1[i].ALMST_SpouseEmailId);
                            string mobileNo = Convert.ToString(que1[i].ALMST_MobileNo);
                            string mobileNo1 = Convert.ToString(que1[i].ALMST_SpouseContactNo);
                            // string email = "rakeshrd307@gmail.com";
                            // string mobileNo = "9686061628";
                            string Template = "WeddingAniversary";
                            string type = "Alumni";
                            //string val = sendmail(MI_ID, email, Template, id, type, trnsno);
                            // string x = sendSms(MI_ID, mobileNo, Template, id, type, trnsno);
                            if (email != "" && email != null)
                            {
                                string val = sendmail(MI_ID, email, email2, Template, id, type);
                            }
                            if (mobileNo != "" && mobileNo != null)
                            {
                                string x = sendSms(MI_ID, mobileNo, mobileNo1, Template, id, type);
                            }

                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return re;
        }

        public string sendmail(long MI_Id, string Email, string email2, string Template, long UserID, string type)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;

                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Alumni_Aniversary_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                    {
                        Value = type
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))

                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }

                Mailmsg = result;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string mailcc = alldetails[0].IVRM_mailcc;

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;


                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }


                    message.AddTo(Email, email2);

                    message.HtmlContent = Mailmsg;


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }

                    else
                    {
                        return "Sendgrid key is not available";
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string sendSms(long MI_Id, string mobileNo, string mobileNo1, string Template, long UserID, string type)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Alumni_Aniversary_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });

                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                    {
                        Value = type
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);

                            sms = result;
                        }
                    }
                }

                sms = result;
                try
                {
                    sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string mobil = mobileNo + "," + mobileNo1;
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = mobil;

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);




                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Alumni";
                    dmo2.To_FLag = type;
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }
                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();



                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }



        //readmitted student 


        public AlumniStudentDTO Readmitted_svedata(AlumniStudentDTO sddto)
        {
            try
            {

                //var alureadmit = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Where(a => a.ALMST_ID == sddto.ALMST_Id && a.ALMSTRADM_Id == sddto.ALMSTRADM_Id).ToList();
                //if (alureadmit.Count() == 0)
                //{
                //    Alumni_Master_Student_ReadmitDMO Readmit = new Alumni_Master_Student_ReadmitDMO();
                //    //var resultalumi = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Single(t => t.ALMST_Id == sddto.ALMST_Id);

                //    Readmit.ALMST_ID = sddto.ALMST_Id;
                //    Readmit.ALMSTRADM_YearLeft = sddto.Readmitted_ASMAY_Id_Left;
                //    Readmit.ALMSTRADM_YearJoined = sddto.Readmitted_ASMAY_Id_Join;
                //    Readmit.ALMSTRADM_ClassJoined = sddto.Readmitted_ASMCL_Id_Join;
                //    Readmit.ALMSTRADM_ClassLeft = sddto.Readmitted_ASMCL_Id_Left;
                //    Readmit.ALMSTRADM_ActiveFlg = 1;
                //    Readmit.ALMSTRADM_CreatedDate = DateTime.Now;
                //    Readmit.ALMSTRADM_UpdatredDate = DateTime.Now;
                //    Readmit.ALMSTRADM_CreatedBy = sddto.userid;
                //    Readmit.ALMSTRADM_UpdatedBy = sddto.userid;

                //    _AlumniContext.Add(Readmit);

                //    var contactExistzs = _AlumniContext.SaveChanges();
                //    if (contactExistzs > 0)
                //    {
                //        sddto.returnval = "Records updated Successfully";
                //    }
                //    else
                //    {
                //        sddto.returnval = "Records not updated Successfully";
                //    }
                //}
                //else
                //{
                //    var resultalumi_readmit = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Single(a => a.ALMST_ID == sddto.ALMST_Id && a.ALMSTRADM_Id == sddto.ALMSTRADM_Id);

                //    resultalumi_readmit.ALMSTRADM_YearLeft = sddto.Readmitted_ASMAY_Id_Left;
                //    resultalumi_readmit.ALMSTRADM_YearJoined = sddto.Readmitted_ASMAY_Id_Join;
                //    resultalumi_readmit.ALMSTRADM_ClassJoined = sddto.Readmitted_ASMCL_Id_Join;
                //    resultalumi_readmit.ALMSTRADM_ClassLeft = sddto.Readmitted_ASMCL_Id_Left;
                //    resultalumi_readmit.ALMSTRADM_ActiveFlg = 1;
                //    resultalumi_readmit.ALMSTRADM_UpdatredDate = DateTime.Now;
                //    resultalumi_readmit.ALMSTRADM_UpdatedBy = sddto.userid;
                //    _AlumniContext.Update(resultalumi_readmit);

                //    var contactExistzs = _AlumniContext.SaveChanges();
                //    if (contactExistzs > 0)
                //    {
                //        sddto.returnval = "Records updated Successfully";
                //    }
                //    else
                //    {
                //        sddto.returnval = "Records not updated Successfully";
                //    }
                //}




                if (sddto.ReadmittedStudentDTO != null && sddto.ReadmittedStudentDTO.Length > 0)
                {
                    foreach (ReadmittedStudentDTOO mob in sddto.ReadmittedStudentDTO)
                    {
                        if (sddto.ALMST_Id != null && sddto.ALMST_Id != 0)
                        {
                            mob.ALMST_Id = sddto.ALMST_Id;
                            Alumni_Master_Student_ReadmitDMO readmitDetails = Mapper.Map<Alumni_Master_Student_ReadmitDMO>(mob);
                            var readmitDetailsdup = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Count(t => t.ALMST_ID == sddto.ALMST_Id && t.ALMSTRADM_ActiveFlg == true);
                            if (readmitDetailsdup > 0)
                            {
                                // var compExamDetailsresult = _precontext.PA_College_Student_CEMarksClgDMO.FirstOrDefault(t => t.PACSTCEM_Id == mob.PACSTCEM_Id && t.PACSTCEM_ActiveFlg == true);

                                //compExamDetailsresult.PACSTCEM_ActiveFlg = true;
                                Mapper.Map(mob, readmitDetails);
                                _AlumniContext.Update(readmitDetails);
                            }
                            else
                            {  //added by 02/02/2017
                                readmitDetails.ALMSTRADM_UpdatredDate = DateTime.Now;
                                readmitDetails.ALMSTRADM_CreatedDate = DateTime.Now;
                                readmitDetails.ALMSTRADM_ActiveFlg = true;
                                readmitDetails.ALMSTRADM_CreatedBy = sddto.userid;
                                readmitDetails.ALMSTRADM_UpdatedBy = sddto.userid;
                                _AlumniContext.Add(readmitDetails);
                            }
                            _AlumniContext.SaveChanges();
                        }

                    }
                }
                else
                {
                    if (sddto.ALMST_Id != null && sddto.ALMST_Id != 0)
                    {


                        var readmitDetailsdup = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Count(t => t.ALMST_ID == sddto.ALMST_Id && t.ALMSTRADM_Id == sddto.ALMSTRADM_Id && t.ALMSTRADM_ActiveFlg == true);
                        if (readmitDetailsdup > 0)
                        {

                            var resultalumi = _AlumniContext.Alumni_Master_Student_ReadmitDMO.Single(t => t.ALMSTRADM_Id == sddto.ALMSTRADM_Id);
                            resultalumi.ALMSTRADM_YearJoined = sddto.Readmitted_ASMAY_Id_Join;
                            resultalumi.ALMSTRADM_YearLeft = sddto.Readmitted_ASMAY_Id_Left;
                            resultalumi.ALMSTRADM_ClassJoined = sddto.Readmitted_ASMCL_Id_Join;
                            resultalumi.ALMSTRADM_ClassLeft = sddto.Readmitted_ASMCL_Id_Left;
                            resultalumi.ALMSTRADM_ActiveFlg = true;
                            resultalumi.ALMSTRADM_UpdatredDate = DateTime.Now;
                            resultalumi.ALMSTRADM_UpdatedBy = sddto.userid;

                            _AlumniContext.Update(resultalumi);
                        }

                        _AlumniContext.SaveChanges();
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sddto;
        }

        public AlumniStudentDTO EditAlumniHomepages(AlumniStudentDTO data)
        {
            try
            {
                data.editdatalist = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id).ToArray();
                data.qualificationAlmStlist = _AlumniContext.Alumni_Student_Qulaification_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id).ToArray();
                data.achivementALMSTDetails = _AlumniContext.Alumni_Student_Achivement_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id).ToArray();
                data.professionaldetailslist = _AlumniContext.Alumni_Student_Profession_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id).ToArray();
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AlumniStudentDTO AlumniHomepageActiveDeactives(AlumniStudentDTO data)
        {
            try
            {
                data.returnvals = false;



                var Result = _AlumniContext.Alumni_M_StudentDMO.Single(a => a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id);
                Result.ALMST_ActiveFlag = Result.ALMST_ActiveFlag == true ? false : true;
                Result.ALMST_UpdatedBy = data.ALMST_UpdatedBy;
                _AlumniContext.Update(Result);
                var i = _AlumniContext.SaveChanges();
                if (i > 0)
                {
                    data.returnvals = true;
                }

                data.deactiveactivelist = _AlumniContext.Alumni_M_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.ALMST_Id == data.ALMST_Id).Select(a => a.ALMST_ActiveFlag).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}




