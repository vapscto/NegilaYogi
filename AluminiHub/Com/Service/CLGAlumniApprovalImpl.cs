using AlumniHub.Com.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Service
{
    public class CLGAlumniApprovalImpl : CLGAlumniApprovalInterface
    {
        private static ConcurrentDictionary<string, CLGAlumniStudentDTO> _login =
      new ConcurrentDictionary<string, CLGAlumniStudentDTO>();

        public AlumniContext _AlumniContext;
        private readonly DomainModelMsSqlServerContext _db;
        public CLGAlumniApprovalImpl(AlumniContext AlumniContext, DomainModelMsSqlServerContext db)
        {
            _AlumniContext = AlumniContext;
            _db = db;
        }
        public CLGAlumniStudentDTO Get_Intial_data(CLGAlumniStudentDTO data)
        {
            try
            {
                data.rolename = _db.MasterRoleType.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;

                if (data.rolename.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    var details = (from a in _AlumniContext.CLGAlumniUserRegistrationDMO
                                   from b in _AlumniContext.CLGAlumni_User_LoginDMO
                                   where (a.ALCSREG_Id == b.ALCSREG_Id && b.IVRMUL_Id == data.userid)
                                   select new CLGAlumniStudentDTO
                                   {
                                       ALCMST_Id = a.AMCST_Id,
                                      
                                   }
          ).ToList();
                    data.ALCMST_Id = details.FirstOrDefault().ALCMST_Id;                  

                    getstudata(data);
                    data.alumnitrue = true;
                }
                else
                {
                    List<MasterAcademic> year = new List<MasterAcademic>();
                    year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                    data.fillyear = year.ToArray();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLGAlumnistudents";
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
               }
               }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<CLGAlumniStudentDTO> Getstudentlist(CLGAlumniStudentDTO data)
        {
            try
            {
                List<CLGAlumniStudentDTO> details = new List<CLGAlumniStudentDTO>();

                details = (from a in _AlumniContext.CLGAlumni_M_StudentDMO
                           where (a.ASMAY_Id_Left == data.ASMAY_Id)
                           select new CLGAlumniStudentDTO
                           {
                               ALCMST_Id = a.ALCMST_Id,

                           }
          ).ToList();

                List<long?> AMCSTids = new List<long?>();
                foreach (var item in details)
                {
                    AMCSTids.Add(item.ALCMST_Id);
                }


                data.stu_name = (from a in _AlumniContext.CLGAlumni_M_StudentDMO
                                 where (a.ASMAY_Id_Left == data.ASMAY_Id && AMCSTids.Contains(a.ALCMST_Id))
                                 select new CLGAlumniStudentDTO
                                 {
                                     AMCST_ID = a.ALCMST_Id,
                                     AMCST_FirstName = a.ALCMST_FirstName + ' ' + a.ALCMST_MiddleName + ' ' + a.ALCMST_LastName,
                                 }
          ).ToList().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<CLGAlumniStudentDTO> Getstudentlistapp(CLGAlumniStudentDTO data)
        {

            List<long> Course = new List<long>();
            List<long> Branch = new List<long>();
            List<long> Sem = new List<long>();


            data.stu_name = (from a in _AlumniContext.CLGAlumniUserRegistrationDMO
                             where (a.MI_Id == data.MI_Id && a.ALCSREG_LeftYear == data.ASMAY_Id && a.ALCSREG_LeftCourse == data.AMCO_Id && a.ALCSREG_ApprovedFlag == false)
                             select new CLGAlumniStudentDTO
                             {
                                 membername = a.ALCSREG_MemberName,
                                 ALCSREG_Id = a.ALCSREG_Id
                             }
                            ).ToArray();
            return data;
        }

        public CLGAlumniStudentDTO checkstudent(CLGAlumniStudentDTO data)
        {
            try
            {
                List<CLGAlumniStudentDTO> abc = new List<CLGAlumniStudentDTO>();                

                    abc = (from a in _AlumniContext.CLGAlumniUserRegistrationDMO
                           where (a.MI_Id == data.MI_Id && a.ALCSREG_Id == data.ALCSREG_Id)
                           select new CLGAlumniStudentDTO
                           {
                               ALCSREG_Id = a.ALCSREG_Id,
                               membername = a.ALCSREG_MemberName,
                               ASMAY_Id_Join = a.ALCSREG_AdmittedYear,
                               ASMAY_Id_Left = a.ALCSREG_LeftYear,
                               CourseJoin = a.ALCSREG_AdmittedCourse,
                               CourseLeft = a.ALCSREG_LeftCourse,
                               BranchJoin = a.ALCSREG_AdmisstedBranch,
                               BranchLeft = a.ALCSREG_LeftBranch,
                               AMCST_ID = a.AMCST_Id,
                               //SemJoin = a.ALCSREG_AdmittedSemester,
                               //SemLeft = a.ALCSREG_LeftSemester
                           }).Distinct().ToList();
                    if (abc.Count() > 0)
                    {
                    string name = abc.FirstOrDefault().membername.ToUpper();
                    data.studentDetails = (from b in _AlumniContext.CLGAlumni_M_StudentDMO
                                           where (((b.ALCMST_FirstName.ToUpper() == null || b.ALCMST_FirstName.ToUpper() == "0" ? "" : b.ALCMST_FirstName.ToUpper()) + " " + (b.ALCMST_MiddleName.ToUpper() == null || b.ALCMST_MiddleName.ToUpper() == "0" ? "" : b.ALCMST_MiddleName.ToUpper()) + " " + (b.ALCMST_LastName.ToUpper() == null || b.ALCMST_LastName.ToUpper() == "0" ? "" : b.ALCMST_LastName.ToUpper())).Trim().Contains(name))
                                           select new CLGAlumniStudentDTO
                                           {
                                               membername = ((b.ALCMST_FirstName.ToUpper() == null || b.ALCMST_FirstName.ToUpper() == "0" ? "" : b.ALCMST_FirstName.ToUpper()) + " " + (b.ALCMST_MiddleName.ToUpper() == null || b.ALCMST_MiddleName.ToUpper() == "0" ? "" : b.ALCMST_MiddleName.ToUpper()) + " " + (b.ALCMST_LastName.ToUpper() == null || b.ALCMST_LastName.ToUpper() == "0" ? "" : b.ALCMST_LastName.ToUpper())).Trim(),

                                               courseadmitted = abc.FirstOrDefault().CourseJoin != 0 ? _AlumniContext.MasterCourseDMO.Where(l => l.MI_Id == data.MI_Id && l.AMCO_Id == abc.FirstOrDefault().CourseJoin).FirstOrDefault().AMCO_CourseName : "--",

                                               branchadmitted = abc.FirstOrDefault().BranchJoin != 0 ? _AlumniContext.ClgMasterBranchDMO.Where(l => l.MI_Id == data.MI_Id && l.AMB_Id == abc.FirstOrDefault().BranchJoin).FirstOrDefault().AMB_BranchName : "--",

                                               yeardmitted = abc.FirstOrDefault().ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Where(l => l.MI_Id == data.MI_Id && l.ASMAY_Id == abc.FirstOrDefault().ASMAY_Id_Join).FirstOrDefault().ASMAY_Year : "--",
                                               ALCMST_Id = b.ALCMST_Id,
                                               ALCMST_AdmNo = b.ALCMST_AdmNo,
                                               ALCSREG_Id = abc.FirstOrDefault().ALCSREG_Id
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

        public CLGAlumniStudentDTO aproovedata(CLGAlumniStudentDTO data)
        {
            try
            {
                if (data.ALCSREG_Id > 0)
                {                    
                        var trnspresltt = _AlumniContext.CLGAlumniUserRegistrationDMO.Single(a=>a.ALCSREG_Id== data.ALCSREG_Id);
                        trnspresltt.ALCSREG_ApprovedFlag = true;
                        trnspresltt.AMCST_Id = data.ALCMST_Id;
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGAlumniStudentDTO searchfilter(CLGAlumniStudentDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                data.fillstudent = (/*from a in _AlumniContext.AdmissionStudentDMO*/
                                    from a in _AlumniContext.CLGAlumni_M_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id_Left == data.ASMAY_Id && ((a.ALCMST_FirstName.Trim().ToUpper() + ' ' + a.ALCMST_MiddleName.Trim().ToUpper() + ' ' + a.ALCMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || (a.ALCMST_FirstName.Trim().ToUpper() + a.ALCMST_MiddleName.Trim().ToUpper() + ' ' + a.ALCMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.ALCMST_FirstName.ToUpper().Contains(data.searchfilter) || a.ALCMST_MiddleName.ToUpper().Contains(data.searchfilter) || a.ALCMST_LastName.ToUpper().Contains(data.searchfilter)))
                                    select new CLGAlumniStudentDTO
                                    {
                                        AMCST_ID = a.ALCMST_Id,
                                        //ALCMST_Id = a.ALCMST_Id,
                                        AMCST_FirstName = ((a.ALCMST_FirstName == null ? "" : a.ALCMST_FirstName.ToUpper()) + " " + (a.ALCMST_MiddleName == null ? "" : a.ALCMST_MiddleName.ToUpper()) + " " + (a.ALCMST_LastName == null ? "" : a.ALCMST_LastName.ToUpper()) + ':' + a.ALCMST_AdmNo).Trim(),
                                    }
           ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGAlumniStudentDTO getstudata(CLGAlumniStudentDTO data)
        {
            try
            {
                List<CLGAlumniStudentDTO> studnetdata = new List<CLGAlumniStudentDTO>();

                studnetdata = (from a in _AlumniContext.CLGAlumni_M_StudentDMO                           
                               where (a.ALCMST_Id == data.ALCMST_Id
                            )
                               select new CLGAlumniStudentDTO
                               {
                                   AMCST_FirstName = ((a.ALCMST_FirstName == null ? "" : a.ALCMST_FirstName.ToUpper()) + " " + (a.ALCMST_MiddleName == null ? "" : a.ALCMST_MiddleName.ToUpper()) + " " + (a.ALCMST_LastName == null ? "" : a.ALCMST_LastName.ToUpper())),

                                   courseadmitted = a.AMCO_JOIN_Id != 0 ? _AlumniContext.MasterCourseDMO.Single(l => l.MI_Id == data.MI_Id && l.AMCO_Id == a.AMCO_JOIN_Id).AMCO_CourseName : "--",

                                   courseleft = a.AMCO_Left_Id != 0 ? _AlumniContext.MasterCourseDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.AMCO_Id == a.AMCO_Left_Id).AMCO_CourseName : "--",

                                   branchadmitted = a.AMCO_JOIN_Id != 0 ? _AlumniContext.ClgMasterBranchDMO.Single(l => l.MI_Id == data.MI_Id && l.AMB_Id == a.AMB_JOIN_Id).AMB_BranchName : "--",

                                   branchleft = a.AMCO_Left_Id != 0 ? _AlumniContext.ClgMasterBranchDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.AMB_Id == a.AMB_Id_Left).AMB_BranchName : "--",

                                   yeardmitted = a.ASMAY_Id_Join != 0 ? _AlumniContext.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.ASMAY_Id_Join).ASMAY_Year : "--",

                                   yearleft = a.ASMAY_Id_Left != 0 ? _AlumniContext.AcademicYear.Single(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASMAY_Id_Left).ASMAY_Year : "--",

                                   ALCMST_AdmNo = a.ALCMST_AdmNo,
                                   //ALCMST_PhoneNo = a.ALCMST_PhoneNo,
                                   ALCMST_PerStreet = a.ALCMST_PerStreet,
                                   ALCMST_PerArea = a.ALCMST_PerArea,
                                   ALCMST_PerAdd3 = a.ALCMST_PerAdd3,
                                   ALCMST_PerState = a.ALCMST_PerState,
                                   ALCMST_FatherName = a.ALCMST_FatherName,
                                   ALCMST_DOB = a.ALCMST_DOB,
                                   ALCMST_MobileNo = a.ALCMST_MobileNo,
                                   ALCMST_emailId = a.ALCMST_emailId,
                                   ALCMST_PerCity = a.ALCMST_PerCity,
                                   ALCMST_PerPincode = a.ALCMST_PerPincode,
                                   IVRMMC_Id = Convert.ToInt64(a.IVRMMC_Id),
                                   ALCMST_Id = a.ALCMST_Id
                                 

                               }
        ).Distinct().ToList();

                data.studentDetails = studnetdata.ToArray();

                data.studentproffession = (from a in _AlumniContext.CLGAlumni_College_Student_Profession
                                           where (a.ALCMST_Id == data.ALCMST_Id)
                                           select a).ToArray();

                data.studentqualification = (from a in _AlumniContext.CLGAlumni_College_Student_Qulaification
                                             where (a.ALCMST_Id == data.ALCMST_Id)
                                             select a).ToArray();

                data.studentachievement = (from a in _AlumniContext.CLGAlumni_College_Student_Achivement
                                           where (a.ALCMST_Id == data.ALCMST_Id)
                                           select a).ToArray();

                List<Country> allCountry = new List<Country>();
                allCountry = _AlumniContext.Country.ToList();

                data.countryDrpDown = allCountry.ToArray();

                List<State> allstate = new List<State>();
                allstate = _AlumniContext.state.Where(t => t.IVRMMC_Id == studnetdata.FirstOrDefault().IVRMMC_Id).ToList();
                data.statedropdown = allstate.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGAlumniStudentDTO savedata(CLGAlumniStudentDTO data)
        {
            try
            {

                var Alumnidetails = (from a in _AlumniContext.CLGAlumni_M_StudentDMO
                                     where (a.ALCMST_Id == data.ALCMST_Id)
                                     select new CLGAlumniStudentDTO
                                     {
                                         ALCMST_Id = a.ALCMST_Id,
                                     }
                                                ).ToList();
                if (Alumnidetails.Count() >0)
                {
                    var resultalumi = _AlumniContext.CLGAlumni_M_StudentDMO.Single(t => t.ALCMST_Id == data.ALCMST_Id);
                    resultalumi.ALCMST_MobileNo = data.ALCMST_MobileNo;
                    resultalumi.ALCMST_PerStreet = data.ALCMST_PerStreet;
                    resultalumi.ALCMST_emailId = data.ALCMST_emailId;
                    resultalumi.ALCMST_PerCity = data.ALCMST_PerCity;
                    resultalumi.ALCMST_PerPincode = data.ALCMST_PerPincode;
                    resultalumi.IVRMMC_Id = data.ALCMST_PerCountry;
                    resultalumi.ALCMST_PerState = data.ALCMST_PerState;
                    resultalumi.ALCMST_PerArea = data.ALCMST_PerArea;
                    resultalumi.ALCMST_PerAdd3 = data.ALCMST_PerAdd3;
                    resultalumi.UpdatedDate = DateTime.Now;
                    resultalumi.ALCMST_DOB = Convert.ToDateTime(data.ALCMST_DOB);
                    resultalumi.ALCMST_Date = DateTime.Now;
                    resultalumi.ALCMST_PaymentDate = DateTime.Now;
                    _AlumniContext.Update(resultalumi);

                    var aluministudetproffession = (from a in _AlumniContext.CLGAlumni_College_Student_Profession
                                                    where (a.ALCMST_Id == data.ALCMST_Id)
                                                   select a).Distinct().ToList();
                    if (aluministudetproffession.Count>0)
                    {
                        foreach (var item in aluministudetproffession)
                        {
                            _AlumniContext.Remove(item);
                        }
                    }

                    for (int p = 0; p < data.professionalDetails.Length; p++)
                    {
                        CLGAlumni_College_Student_Profession prp = new CLGAlumni_College_Student_Profession();

                        prp.MI_Id = data.MI_Id;
                        prp.ALCMST_Id = Convert.ToInt64(data.ALCMST_Id);
                        prp.ALCSPR_CompanyName = data.professionalDetails[p].ALCSPR_CompanyName;
                        prp.ALCSPR_CompanyAddress = data.professionalDetails[p].ALCSPR_CompanyAddress;
                        prp.ALCSPR_Designation = data.professionalDetails[p].ALCSPR_Designation;
                        prp.ALCSPR_EmailId = data.professionalDetails[p].ALCSPR_EmailId;
                        prp.ALCSPR_Phone = data.professionalDetails[p].ALCSPR_Phone;
                        prp.ALCSPR_OtherDetails = data.professionalDetails[p].ALCSPR_OtherDetails;
                        prp.ALCSPR_WorkingSince = data.professionalDetails[p].ALCSPR_WorkingSince;
                        prp.CreatedDate = DateTime.Now;
                        prp.UpdatedDate = DateTime.Now;
                        prp.ALCSPR_ActiveFlg = true;

                        _AlumniContext.Add(prp);
                    }



                    var aluministudetachievement = (from a in _AlumniContext.CLGAlumni_College_Student_Achivement
                                                    where (a.ALCMST_Id == data.ALCMST_Id)
                                                    select a 
                                             ).Distinct().ToList();
                    if (aluministudetachievement.Count > 0) {
                        foreach (var item in aluministudetachievement)
                        {
                            _AlumniContext.Remove(item);
                        }
                    }
                    for (int a = 0; a < data.achievementsDetails.Length; a++)
                    {
                        CLGAlumni_College_Student_Achivement ach = new CLGAlumni_College_Student_Achivement();

                       ach.MI_Id = data.MI_Id;
                       ach.ALCMST_Id = Convert.ToInt64(data.ALCMST_Id);
                       ach.ALCSAC_Achievement = data.achievementsDetails[a].ALCSAC_Achievement;
                       ach.ALCSAC_Remarks = data.achievementsDetails[a].ALCSAC_Remarks;
                       ach.ALCSAC_ActiveFlg = true;
                       ach.CreatedDate = DateTime.Now;
                        ach.UpdatedDate = DateTime.Now;

                        _AlumniContext.Add(ach);
                    }

                    var aluministudetqualification = (from a in _AlumniContext.CLGAlumni_College_Student_Qulaification
                                                      where (a.ALCMST_Id == data.ALCMST_Id)
                                                      select a
                                               ).Distinct().ToList();
                    if (aluministudetqualification.Count() > 0)
                    {
                        foreach (var item in aluministudetqualification)
                        {
                            _AlumniContext.Remove(item);
                        }
                    }
                    for (int s = 0; s < data.qualificationDetails.Length; s++)
                        {                        
                        CLGAlumni_College_Student_Qulaification qua = new CLGAlumni_College_Student_Qulaification();
                        qua.MI_Id = data.MI_Id;
                        qua.ALCMST_Id = Convert.ToInt64(data.ALCMST_Id);
                        qua.ALCSQU_Qulification = data.qualificationDetails[s].ALCSQU_Qulification;
                        qua.ALCSQU_University = data.qualificationDetails[s].ALCSQU_University;
                        qua.ALCSQU_OtherDetails = data.qualificationDetails[s].ALCSQU_OtherDetails;
                        qua.ALCSQU_YearOfPassing =Convert.ToInt64(data.qualificationDetails[s].ALCSQU_YearOfPassing);
                        qua.ALCSQU_ActiveFlg = true;
                        qua.CreatedDate = DateTime.Now;
                        qua.UpdatedDate = DateTime.Now;

                        _AlumniContext.Add(qua);
                    }

                        var contactExistzs = _AlumniContext.SaveChanges();

                    if (contactExistzs > 0)
                    {
                        data.returnval = "Records saved Successfully";
                    }
                    else
                    {
                        data.returnval = "Records not saved Successfully";
                    }

                }
                else
                {                  

                    CLGAlumni_M_StudentDMO resultalumi = new CLGAlumni_M_StudentDMO();
                    resultalumi.ALCMST_FirstName = data.ALCMST_FirstName;
                    resultalumi.ALCMST_AdmNo = data.ALCMST_AdmNo;
                    resultalumi.ALCMST_FatherName = data.ALCMST_FatherName;
                    resultalumi.ALCMST_emailId = data.ALCMST_emailId;
                    resultalumi.AMCO_JOIN_Id = data.AMCO_JOIN_Id;
                    resultalumi.AMCO_Left_Id = data.AMCO_Left_Id;
                    resultalumi.ASMAY_Id_Join = data.ASMAY_Id_Join;
                    resultalumi.ASMAY_Id_Left = data.ASMAY_Id_Left;
                    resultalumi.AMB_JOIN_Id = data.AMB_JOIN_Id;
                    resultalumi.AMB_Id_Left = data.AMB_Id_Left;
                    resultalumi.AMSE_JOIN_Id = data.AMSE_JOIN_Id;
                    resultalumi.AMSE_Id_Left = data.AMSE_Id_Left;
                    resultalumi.ALCMST_MobileNo = data.ALCMST_MobileNo;
                    resultalumi.ALCMST_PerStreet = data.ALCMST_PerStreet;                   
                    resultalumi.ALCMST_PerCity = data.ALCMST_PerCity;
                    resultalumi.ALCMST_PerPincode = data.ALCMST_PerPincode;
                    resultalumi.IVRMMC_Id = data.ALCMST_PerCountry;
                    resultalumi.ALCMST_PerState = data.ALCMST_PerState;
                    resultalumi.ALCMST_PerArea = data.ALCMST_PerArea;
                    resultalumi.ALCMST_PerAdd3 = data.ALCMST_PerAdd3;
                    resultalumi.MI_Id = data.MI_Id;
                    resultalumi.UpdatedDate = DateTime.Now;
                    resultalumi.ALCMST_DOB = Convert.ToDateTime(data.ALCMST_DOB);
                    resultalumi.ALCMST_Date = DateTime.Now;
                    resultalumi.ALCMST_PaymentDate = DateTime.Now;
                    _AlumniContext.Add(resultalumi);
                    var savenew  = _AlumniContext.SaveChanges();
                    data.ALCMST_Id = resultalumi.ALCMST_Id;

                    var aluministudetproffession = (from a in _AlumniContext.CLGAlumni_College_Student_Profession
                                                    where (a.ALCMST_Id == data.ALCMST_Id)
                                                    select new CLGAlumniStudentDTO
                                                    {
                                                        ALCMST_Id = a.ALCMST_Id,
                                                    }).ToList();
                    if (aluministudetproffession.Count > 0)
                    {
                        foreach (var item in aluministudetproffession)
                        {
                            _AlumniContext.Remove(item);
                        }
                    }

                    for (int p = 0; p < data.professionalDetails.Length; p++)
                    {
                        CLGAlumni_College_Student_Profession prp = new CLGAlumni_College_Student_Profession();

                        prp.MI_Id = data.MI_Id;
                        prp.ALCMST_Id = Convert.ToInt64(data.ALCMST_Id);
                        prp.ALCSPR_CompanyName = data.professionalDetails[p].ALCSPR_CompanyName;
                        prp.ALCSPR_CompanyAddress = data.professionalDetails[p].ALCSPR_CompanyAddress;
                        prp.ALCSPR_Designation = data.professionalDetails[p].ALCSPR_Designation;
                        prp.ALCSPR_EmailId = data.professionalDetails[p].ALCSPR_EmailId;
                        prp.ALCSPR_Phone = data.professionalDetails[p].ALCSPR_Phone;
                        prp.ALCSPR_OtherDetails = data.professionalDetails[p].ALCSPR_OtherDetails;
                        prp.ALCSPR_WorkingSince = data.professionalDetails[p].ALCSPR_WorkingSince;
                        prp.CreatedDate = DateTime.Now;
                        prp.UpdatedDate = DateTime.Now;
                        prp.ALCSPR_ActiveFlg = true;

                        _AlumniContext.Add(prp);
                    }



                    var aluministudetachievement = (from a in _AlumniContext.CLGAlumni_College_Student_Achivement
                                                    where (a.ALCMST_Id == data.ALCMST_Id)
                                                    select new CLGAlumniStudentDTO
                                                    {
                                                        ALCMST_Id = a.ALCMST_Id,
                                                    }
                                             ).ToList();

                    if (aluministudetachievement.Count > 0)
                    {
                        foreach (var item in aluministudetachievement)
                        {
                            _AlumniContext.Remove(item);
                        }
                    }                 

                    for (int a = 0; a < data.achievementsDetails.Length; a++)
                    {
                        CLGAlumni_College_Student_Achivement ach = new CLGAlumni_College_Student_Achivement();

                        ach.MI_Id = data.MI_Id;
                        ach.ALCMST_Id = Convert.ToInt64(data.ALCMST_Id);
                        ach.ALCSAC_Achievement = data.achievementsDetails[a].ALCSAC_Achievement;
                        ach.ALCSAC_Remarks = data.achievementsDetails[a].ALCSAC_Remarks;
                        ach.ALCSAC_ActiveFlg = true;
                        ach.CreatedDate = DateTime.Now;
                        ach.UpdatedDate = DateTime.Now;

                        _AlumniContext.Add(ach);
                    }

                    var aluministudetqualification = (from a in _AlumniContext.CLGAlumni_College_Student_Qulaification
                                                      where (a.ALCMST_Id == data.ALCMST_Id)
                                                      select new CLGAlumniStudentDTO
                                                      {
                                                          ALCMST_Id = a.ALCMST_Id,
                                                      }
                                              ).ToList();
                    if (aluministudetqualification.Count() > 0)
                    {
                        foreach (var item in aluministudetqualification)
                        {
                            _AlumniContext.Remove(item);
                        }
                    }
                    for (int s = 0; s < data.qualificationDetails.Length; s++)
                    {
                        CLGAlumni_College_Student_Qulaification qua = new CLGAlumni_College_Student_Qulaification();
                        qua.MI_Id = data.MI_Id;
                        qua.ALCMST_Id = Convert.ToInt64(data.ALCMST_Id);
                        qua.ALCSQU_Qulification = data.qualificationDetails[s].ALCSQU_Qulification;
                        qua.ALCSQU_University = data.qualificationDetails[s].ALCSQU_University;
                        qua.ALCSQU_OtherDetails = data.qualificationDetails[s].ALCSQU_OtherDetails;
                        qua.ALCSQU_YearOfPassing = Convert.ToInt64(data.qualificationDetails[s].ALCSQU_YearOfPassing);
                        qua.ALCSQU_ActiveFlg = true;
                        qua.CreatedDate = DateTime.Now;
                        qua.UpdatedDate = DateTime.Now;

                        _AlumniContext.Add(qua);
                    }

                    var contactExistzs = _AlumniContext.SaveChanges();

                    if (contactExistzs > 0)
                    {
                        data.returnval = "Records saved Successfully";
                    }
                    else
                    {
                        data.returnval = "Records not saved Successfully";
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




