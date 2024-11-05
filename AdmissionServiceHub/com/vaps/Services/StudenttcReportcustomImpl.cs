using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudenttcReportcustomImpl : Interfaces.StudenttcReportcustomInterface
    {
        public StudentAttendancecustomReportContext _db;
        public StudenttcReportcustomImpl(StudentAttendancecustomReportContext db)
        {
            _db = db;
        }
        public async Task<StudentAttendanceReportDTO> getInitailData(int mi_id)
        {
            StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(y => y.MI_Id == mi_id && y.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToListAsync();
                ctdo.academicList = allyear.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _db.admissionClass.Where(c => c.ASMCL_ActiveFlag == true && c.MI_Id == mi_id).OrderBy(d => d.ASMCL_Order).ToList();
                ctdo.fillclass = classname.ToArray();

                List<School_M_Section> secname = new List<School_M_Section>();
                secname = _db.masterSection.Where(s => s.MI_Id == mi_id && s.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToList();
                ctdo.fillsec = secname.ToArray();


                ctdo.MasterCompany = (from a in _db.Institution
                                      where (a.MI_Id == mi_id)
                                      select new StudentAttendanceReportDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          miid = a.MI_Id,
                                      }).ToArray();

                var cat = _db.GenConfig.Where(g => g.MI_Id == ctdo.miid && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    ctdo.category_list = _db.category.Where(f => f.MI_Id == ctdo.miid && f.AMC_ActiveFlag == 1).ToArray();
                    ctdo.categoryflag = true;
                }
                else
                {
                    ctdo.categoryflag = false;
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public StudentAttendanceReportDTO getstuddetails(StudentAttendanceReportDTO data)
        {
            try
            {
                if (data.regornamedetails == "regno")
                {
                    data.studentlist = (from a in _db.StudentTcList
                                        from b in _db.School_Adm_Y_StudentDMO
                                        from c in _db.AdmissionStudentDMO
                                        where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && c.AMST_SOL == "L" && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id
                                        && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMC_Id)
                                        select new StudentAttendanceReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((c.AMST_AdmNo == null ? " " : c.AMST_AdmNo) + ':' +
                                            (c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                        }).Distinct().ToArray();
                }

                else if (data.regornamedetails == "stdname")
                {
                    data.studentlist = (from a in _db.StudentTcList
                                        from b in _db.School_Adm_Y_StudentDMO
                                        from c in _db.AdmissionStudentDMO
                                        where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && c.AMST_SOL == "L" && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id
                                        && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMC_Id)
                                        select new StudentAttendanceReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((c.AMST_FirstName == null && c.AMST_FirstName !="" ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null && c.AMST_MiddleName!="" ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null && c.AMST_LastName !=""? " " : c.AMST_LastName) + ':' + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                        }).Distinct().ToArray();
                }
                else
                {
                    data.studentlist = (from a in _db.StudentTcList
                                        from b in _db.School_Adm_Y_StudentDMO
                                        from c in _db.AdmissionStudentDMO
                                        where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && c.AMST_SOL == "L" && b.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id)
                                        select new StudentAttendanceReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = ((c.AMST_FirstName == null && c.AMST_FirstName != "" ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null && c.AMST_MiddleName != "" ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null && c.AMST_LastName != "" ? " " : c.AMST_LastName) + ':' + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                            fromdate = Convert.ToDateTime(a.CreatedDate)
                                        }).Distinct().OrderByDescending(a => a.fromdate).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO getstudlist(int id)
        {
            StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            ctdo.studentList = (from a in _db.admissionStduent
                                from b in _db.admissionyearstudent
                                where a.AMST_Id == b.AMST_Id && a.AMST_SOL == "L" && b.ASMAY_Id == id
                                select new StudentAttendanceReportDTO
                                {
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = a.AMST_FirstName,
                                    AMST_MiddleName = a.AMST_MiddleName,
                                    AMST_LastName = a.AMST_LastName,
                                    AMST_AdmNo = a.AMST_AdmNo

                                }).ToArray();
            return ctdo;
        }
        public StudentAttendanceReportDTO getTCdata(StudentAttendanceReportDTO data)
        {
            try
            {

                //data.studentTCList = (from a in _db.admissionStduent
                //                      from b in _db.StudentTcList
                //                      from c in _db.masterSection
                //                      from d in _db.admissionClass
                //                      from na in _db.Country
                //                      from ims in _db.State
                //                      from imd in _db.DistrictDMO
                //                      where (a.AMST_Id == b.AMST_Id
                //                      && b.ASMS_Id == c.ASMS_Id
                //                      && b.ASMCL_Id == d.ASMCL_Id
                //                      && a.AMST_Nationality == na.IVRMMC_Id
                //                      && ims.IVRMMS_Id==a.AMST_PerState
                //                      &&  imd.IVRMMS_Id==ims.IVRMMS_Id
                //                      && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                //                      select new StudentAttendanceReportDTO
                //                      {
                //                          AMST_FirstName = a.AMST_FirstName,
                //                          AMST_MiddleName = a.AMST_MiddleName,
                //                          AMST_LastName = a.AMST_LastName,
                //                          AMST_AdmNo = a.AMST_AdmNo,
                //                          AMST_RegistrationNo = a.AMST_RegistrationNo,
                //                          AMST_FatherName = ((a.AMST_FatherName == null || a.AMST_FatherName == "" ? "" : a.AMST_FatherName) +
                //                          (a.AMST_FatherSurname == null || a.AMST_FatherSurname == "" ? "" : " " + a.AMST_FatherSurname)).Trim(),
                //                          AMST_MotherName = ((a.AMST_MotherName == null || a.AMST_MotherName == "" ? "" : a.AMST_MotherName) +
                //                          (a.AMST_MotherSurname == null || a.AMST_MotherSurname == "" ? "" : " " + a.AMST_MotherSurname)).Trim(),
                //                          Nationality = (na.IVRMMC_Nationality).ToString(),
                //                          AMST_BirthPlace = a.AMST_BirthPlace,
                //                          ASTC_LastAttendedDate = b.ASTC_LastAttendedDate,
                //                          AMST_Sex = a.AMST_Sex,
                //                          AMST_DOB = a.AMST_DOB.Date,
                //                          AMST_DOB_Words = a.AMST_DOB_Words,
                //                          AMST_Date = a.AMST_Date.Date,
                //                          astC_TCNO = b.ASTC_TCNO,
                //                          AMST_emailId = a.AMST_emailId,
                //                          AMST_AadharNo = a.AMST_AadharNo,
                //                          AMST_MobileNo = a.AMST_MobileNo,
                //                          ASMCL_Id = d.ASMCL_Id,
                //                          Last_Class_Studied = d.ASMCL_ClassName,
                //                          astC_LeavingReason = b.ASTC_LeavingReason,
                //                          astC_TCIssueDate = b.ASTC_TCDate,
                //                          AMST_PerCity = a.AMST_PerCity,
                //                          AMST_PerStreet = a.AMST_PerStreet,
                //                          AMST_PerArea = a.AMST_PerArea,
                //                          AMST_ConStreet = a.AMST_ConStreet,
                //                          AMST_ConArea = a.AMST_ConArea,
                //                          AMST_ConCity = a.AMST_ConCity,
                //                          ASTC_Remarks = b.ASTC_Remarks,
                //                          Religion = a.IVRMMR_Id != 0 || a.IVRMMR_Id != null ? _db.religion.Where(t => t.IVRMMR_Id == a.IVRMMR_Id).FirstOrDefault().IVRMMR_Name : "",
                //                          caste = a.IC_Id != 0 || a.IC_Id != null ? _db.caste.Where(t => t.MI_Id == data.miid && t.IMC_Id == a.IC_Id).FirstOrDefault().IMC_CasteName : "",
                //                          ASTC_Conduct = b.ASTC_Conduct,
                //                          ASMC_SectionName = c.ASMC_SectionName,
                //                          ASTC_Qual_PromotionFlag = b.ASTC_Qual_Class,
                //                          Fee_Due_Amnt = b.Fee_Due_Amnt,
                //                          Library_Due_Amnt = b.Library_Due_Amnt,
                //                          Store_Canteen_Due = b.Store_Canteen_Due,
                //                          PDA_Due = b.PDA_Due,
                //                          classname = d.ASMCL_ClassName,
                //                          qualificlass = b.ASTC_Qual_Class,
                //                          tcdatess = b.ASTC_TCDate,
                //                          name = b.ASTC_LanguageStudied,
                //                          AMC_Name = b.ASTC_ElectivesStudied,
                //                          govtno = a.AMST_BPLCardNo,
                //                          statename = ims.IVRMMS_Name,
                //                          district=imd.IVRMMD_Name,
                //                          mediumOfInstruction=b.ASTC_MediumOfINStruction,
                //                          subjectstudied=b.ASTC_SubjectsStudied,
                //                          electivestudied=b.ASTC_ElectivesStudied,
                //                          noof_daysattended=b.ASTC_AttendedDays,
                //                          noof_schooldays=b.ASTC_WorkingDays,
                //                          fee_concesion=b.ASTC_FeeConcession,
                //                          promotionflag=b.ASTC_Qual_PromotionFlag,
                //                          medicallyexamined=b.ASTC_MedicallyExam,
                //                          ASTC_FeePaid=b.ASTC_FeePaid


                //                      }).ToArray();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.studentTCList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.MasterCompany = (from a in _db.Institution
                                      where (a.MI_Id == data.miid)
                                      select new StudentAttendanceReportDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          miid = a.MI_Id,
                                      }).ToArray();

                data.academicList1 = _db.academicYear.Where(a => a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.previousschool = _db.StudentPrevSchoolDMO.Where(a => a.MI_Id == data.miid && a.AMST_Id == data.AMST_Id).ToArray();

                var getnextclass1 = (from a in _db.StudentTcList
                                     where (a.MI_Id == data.miid && a.AMST_Id == data.AMST_Id)
                                     select new StudentAttendanceReportDTO
                                     {
                                         classid = a.ASMCL_Id,
                                     }).Distinct().ToArray();

                var classnext = getnextclass1.FirstOrDefault().classid + 1;

                data.getnextclass = _db.admissionClass.Where(a => a.MI_Id == data.miid && a.ASMCL_Id == Convert.ToInt64(classnext.ToString())).ToArray();

                data.classnamejoin = (from a in _db.admissionStduent
                                      from b in _db.admissionClass
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.miid && a.AMST_Id == data.AMST_Id)
                                      select new StudentAttendanceReportDTO
                                      {
                                          joinclassid = a.ASMCL_Id,
                                          classjoinname = b.ASMCL_ClassName
                                      }).ToArray();

                data.studenttcdetails = _db.StudentTcList.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.getadm_m_student_details = _db.admissionStduent.Where(a => a.AMST_Id == data.AMST_Id).ToArray();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StudentAttendanceReportDTO getTcdetailsbwmc(StudentAttendanceReportDTO data)
        {
            try
            {
                data.studentTCList = (from a in _db.admissionStduent
                                      from b in _db.StudentTcList
                                      from c in _db.masterSection
                                      from d in _db.admissionClass
                                      from r in _db.religion
                                      from cas in _db.caste
                                      from con in _db.Country
                                      where (a.AMST_Id == b.AMST_Id
                                      && b.ASMS_Id == c.ASMS_Id
                                      && a.AMST_Id == data.AMST_Id
                                      && b.ASMCL_Id == d.ASMCL_Id
                                      && a.IVRMMR_Id == r.IVRMMR_Id
                                      && a.IC_Id == cas.IMC_Id
                                      && con.IVRMMC_Id == a.AMST_Nationality)
                                      select new StudentAttendanceReportDTO
                                      {
                                          AMST_FirstName = a.AMST_FirstName,
                                          AMST_MiddleName = a.AMST_MiddleName,
                                          AMST_LastName = a.AMST_LastName,
                                          AMST_AdmNo = a.AMST_AdmNo,
                                          AMST_RegistrationNo = a.AMST_RegistrationNo,
                                          AMST_FatherName = ((a.AMST_FatherName == null || a.AMST_FatherName == "" ? "" : a.AMST_FatherName) +
                                          (a.AMST_FatherSurname == null || a.AMST_FatherSurname == "" ? "" : " " + a.AMST_FatherSurname)).Trim(),
                                          AMST_MotherName = ((a.AMST_MotherName == null || a.AMST_MotherName == "" ? "" : a.AMST_MotherName) +
                                          (a.AMST_MotherSurname == null || a.AMST_MotherSurname == "" ? "" : " " + a.AMST_MotherSurname)).Trim(),

                                          Nationality = con.IVRMMC_Nationality,
                                          AMST_BirthPlace = a.AMST_BirthPlace,
                                          ASTC_LastAttendedDate = b.ASTC_LastAttendedDate,
                                          AMST_Sex = a.AMST_Sex,
                                          AMST_DOB = a.AMST_DOB.Date,
                                          AMST_DOB_Words = a.AMST_DOB_Words,
                                          AMST_Date = a.AMST_Date.Date,
                                          astC_TCNO = b.ASTC_TCNO,
                                          AMST_emailId = a.AMST_emailId,
                                          AMST_AadharNo = a.AMST_AadharNo,
                                          AMST_MobileNo = a.AMST_MobileNo,
                                          ASMCL_Id = d.ASMCL_Id,
                                          Last_Class_Studied = d.ASMCL_ClassName,
                                          astC_LeavingReason = b.ASTC_LeavingReason,
                                          astC_TCIssueDate = b.ASTC_TCDate,
                                          AMST_PerCity = a.AMST_PerCity,
                                          AMST_PerStreet = a.AMST_PerStreet,
                                          AMST_PerArea = a.AMST_PerArea,
                                          AMST_ConStreet = a.AMST_ConStreet,
                                          AMST_ConArea = a.AMST_ConArea,
                                          AMST_ConCity = a.AMST_ConCity,
                                          ASTC_Remarks = b.ASTC_Remarks,
                                          Religion = r.IVRMMR_Name,
                                          caste = cas.IMC_CasteName,
                                          ASTC_Conduct = b.ASTC_Conduct,
                                          ASMC_SectionName = c.ASMC_SectionName,
                                          ASTC_Qual_PromotionFlag = b.ASTC_Qual_Class,
                                          Fee_Due_Amnt = b.Fee_Due_Amnt,
                                          Library_Due_Amnt = b.Library_Due_Amnt,
                                          Store_Canteen_Due = b.Store_Canteen_Due,
                                          PDA_Due = b.PDA_Due,
                                          classname = d.ASMCL_ClassName,
                                          qualificlass = b.ASTC_Qual_Class,
                                          tcdatess = b.ASTC_TCDate,
                                          name = b.ASTC_LanguageStudied,
                                          AMC_Name = b.ASTC_ElectivesStudied,
                                          AMST_SubCasteIMC_Id=a.AMST_SubCasteIMC_Id
                                      }).ToArray();

                data.MasterCompany = (from a in _db.Institution
                                      where (a.MI_Id == data.miid)
                                      select new StudentAttendanceReportDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          miid = a.MI_Id,
                                      }).ToArray();

                data.academicList1 = _db.academicYear.Where(a => a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.previousschool = _db.StudentPrevSchoolDMO.Where(a => a.MI_Id == data.miid && a.AMST_Id == data.AMST_Id).ToArray();
                var getnextclass1 = (from a in _db.StudentTcList
                                     where (a.MI_Id == data.miid && a.AMST_Id == data.AMST_Id)
                                     select new StudentAttendanceReportDTO
                                     {
                                         classid = a.ASMCL_Id,
                                     }).Distinct().ToArray();

                var classnext = getnextclass1.FirstOrDefault().classid + 1;
                data.getnextclass = _db.admissionClass.Where(a => a.MI_Id == data.miid && a.ASMCL_Id == Convert.ToInt64(classnext.ToString())).ToArray();

                data.classnamejoin = (from a in _db.admissionStduent
                                      from b in _db.admissionClass
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.miid && a.AMST_Id == data.AMST_Id)
                                      select new StudentAttendanceReportDTO
                                      {
                                          joinclassid = a.ASMCL_Id,
                                          classjoinname = b.ASMCL_ClassName
                                      }).ToArray();

                data.studenttcdetails = _db.StudentTcList.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.getadm_m_student_details = _db.admissionStduent.Where(a => a.AMST_Id == data.AMST_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
