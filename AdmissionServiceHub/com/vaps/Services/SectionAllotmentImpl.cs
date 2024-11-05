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
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;
using CommonLibrary;
using DomainModel.Model.com.vapstech.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SectionAllotmentImpl : Interfaces.SectionAllotmentInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
            new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public DomainModelMsSqlServerContext _Context;
        public SectionAllotmentImpl(DomainModelMsSqlServerContext Admissiondbcontext)
        {
            _Context = Admissiondbcontext;
        }
        public SchoolYearWiseStudentDTO GetDropDownList(SchoolYearWiseStudentDTO stu)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _Context.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();

                stu.YearList = year.ToArray();

                List<School_M_Class> classList = new List<School_M_Class>();
                classList = _Context.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToList();
                stu.classList = classList.ToArray();

                List<School_M_Section> School_M_SectionList = new List<School_M_Section>();
                School_M_SectionList = _Context.School_M_Section.Where(t => t.MI_Id == stu.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToList();
                stu.sectionList = School_M_SectionList.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }
        public SchoolYearWiseStudentDTO GetStudentListByYear(long id)
        {
            SchoolYearWiseStudentDTO stu = new SchoolYearWiseStudentDTO();
            try
            {
                stu.StudentList = (from s in _Context.Adm_M_Student
                                   where !(from ys in _Context.SchoolYearWiseStudent
                                           where ys.AMAY_ActiveFlag == 0
                                           select ys.AMST_Id).Contains(s.AMST_Id) && s.ASMAY_Id == id
                                   select s).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }
        public SchoolYearWiseStudentDTO saveSctionAllotment(SchoolYearWiseStudentDTO secAllt)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                int MaxCapacity = _Context.School_M_Section.SingleOrDefault(s => s.ASMS_Id == secAllt.ASMS_Id).ASMC_MaxCapacity;

                if (MaxCapacity == 0)
                {
                    secAllt.returnMsg = "Selected Section Contains Zero Maximum Capacity.Please Contact Administrator";
                    return secAllt;
                }
                var count = (from m in _Context.Adm_M_Student
                             from n in _Context.SchoolYearWiseStudent
                             where (m.AMST_Id == n.AMST_Id && m.MI_Id == secAllt.MI_Id)
                             select new SchoolYearWiseStudentDTO
                             {
                                 AMST_Id = n.AMST_Id
                             }).ToList();
                //add Student Section details
                if (secAllt.SectionAllotmentType == "New")
                {
                    try
                    {
                        if (secAllt.resultData.Count() > 0)
                        {
                            foreach (SchoolYearWiseStudentDTO ph in secAllt.resultData)
                            {
                                var createdcount = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag.Equals(1) && t.ASMCL_Id == secAllt.ASMCL_Id && t.ASMS_Id == secAllt.ASMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();

                                if (createdcount.Count < MaxCapacity)
                                {
                                    if (count.Count > 0)
                                    {
                                        var result = (from m in _Context.Adm_M_Student
                                                      from n in _Context.SchoolYearWiseStudent
                                                      where (m.AMST_Id == n.AMST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                      && n.ASMCL_Id == secAllt.ASMCL_Id && n.ASMS_Id == secAllt.ASMS_Id)
                                                      select new SchoolYearWiseStudentDTO
                                                      {
                                                          AMST_Id = n.AMST_Id,
                                                          AMAY_RollNo = n.AMAY_RollNo
                                                      }).ToList();
                                        if (result.Count > 0)
                                        {
                                            var rollNo = result.OrderByDescending(e => e.AMAY_RollNo).First().AMAY_RollNo;
                                            secAllt.AMAY_RollNo = rollNo + 1;
                                        }
                                        else
                                        {
                                            secAllt.AMAY_RollNo = 1;
                                        }
                                    }
                                    else
                                    {
                                        secAllt.AMAY_RollNo = 1;
                                    }

                                    School_Adm_Y_StudentDMO sct = Mapper.Map<School_Adm_Y_StudentDMO>(ph);
                                    sct.ASMAY_Id = secAllt.ASMAY_Id;
                                    sct.ASMCL_Id = secAllt.ASMCL_Id;
                                    sct.ASMS_Id = secAllt.ASMS_Id;
                                    sct.AMAY_RollNo = secAllt.AMAY_RollNo;
                                    sct.AMAY_ActiveFlag = 1;
                                    sct.LoginId = secAllt.LoginId;
                                    sct.AMAY_DateTime = indiantime0;
                                    sct.CreatedDate = indiantime0;
                                    sct.UpdatedDate = indiantime0;
                                    sct.ASYST_CreatedBy = secAllt.LoginId;
                                    sct.ASYST_UpdatedBy = secAllt.LoginId;
                                    _Context.Add(sct);

                                    var flag = _Context.SaveChanges();
                                    if (flag > 0)
                                    {
                                        secAllt.returnVal = true;

                                        var admConfig = _Context.AdmissionStandardDMO.Single(t => t.MI_Id == secAllt.MI_Id);
                                        var studDet = _Context.Adm_M_Student.Where(t => t.MI_Id == secAllt.MI_Id && t.AMST_Id == ph.AMST_Id).ToList();

                                        string emailids = "";
                                        long mobileids = 0;

                                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                                        {
                                            emailids = studDet.FirstOrDefault().AMST_MotherEmailId;
                                            mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo);
                                        }
                                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                                        {
                                            emailids = studDet.FirstOrDefault().AMST_FatheremailId;
                                            mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo);
                                        }
                                        else
                                        {
                                            emailids = studDet.FirstOrDefault().AMST_emailId;
                                            mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo);
                                        }

                                        try
                                        {
                                            SMS sms = new SMS(_Context);
                                            string s = sms.sendsmssection(secAllt.MI_Id, mobileids, "SECTION_ALLOTMENT", ph.AMST_Id, secAllt.ASMAY_Id).Result;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        try
                                        {
                                            Email Email = new Email(_Context);
                                            string m = Email.sendmailsection(secAllt.MI_Id, emailids, "SECTION_ALLOTMENT", ph.AMST_Id, secAllt.ASMAY_Id);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        secAllt.returnVal = false;
                                    }
                                }
                                else
                                {
                                    secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                    return secAllt;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Change Section details
                if (secAllt.SectionAllotmentType == "Change")
                {
                    try
                    {
                        if (secAllt.resultData.Count() > 0)
                        {
                            foreach (SchoolYearWiseStudentDTO ph in secAllt.resultData)
                            {

                                var Old_ASMS_Id = _Context.SchoolYearWiseStudent.Where(s => s.ASMAY_Id == secAllt.ASMAY_Id && s.ASMCL_Id == secAllt.ASMCL_Id && s.AMST_Id == ph.AMST_Id && s.AMAY_ActiveFlag == 1).Select(d => d.ASMS_Id).FirstOrDefault();
                                var createdcount = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag.Equals(1) && t.ASMCL_Id == secAllt.ASMCL_Id
                                && t.ASMS_Id == secAllt.ASMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();

                                //var attendance_entryhappen = (from m in _Context.Adm_studentAttendance
                                //                              from t in _Context.Adm_studentAttendanceStudents
                                //                              where (m.ASA_Id == t.ASA_Id && m.MI_Id == secAllt.MI_Id && m.ASMAY_Id == ph.ASMAY_Id && m.ASMCL_Id == ph.ASMCL_Id && m.ASMS_Id == ph.ASMS_Id && t.AMST_Id == ph.AMST_Id)
                                //                              select new SchoolYearWiseStudentDTO
                                //                              {
                                //                                  AMST_Id = t.AMST_Id,
                                //                              }).ToList();

                                //  var fee_studentstatus_entryhappen = _Context.feestudentstatus.Where(fs => fs.MI_Id == secAllt.MI_Id && fs.ASMAY_Id == ph.ASMAY_Id && fs.AMST_Id == ph.AMST_Id).ToList();
                                // string str = "";

                                //if (attendance_entryhappen.Count > 0)     // || fee_studentstatus_entryhappen.Count > 0)
                                //{
                                //    str += ph.AMST_FirstName + ",";

                                //    // return secAllt;
                                //}
                                //if (str != null && str != "")
                                //{
                                //    secAllt.returnMsg += "For " + str + "    Attendance Entry /Fee Mapping  Is Already Done So You Can not Change The Section";
                                //}
                                //if (attendance_entryhappen.Count == 0)  //&& fee_studentstatus_entryhappen.Count == 0)
                                //{
                                if (createdcount.Count < MaxCapacity)
                                {
                                    if (count.Count > 0)
                                    {
                                        var result11 = (from m in _Context.Adm_M_Student
                                                        from n in _Context.SchoolYearWiseStudent
                                                        where (m.AMST_Id == n.AMST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                        && n.ASMCL_Id == secAllt.ASMCL_Id && n.ASMS_Id == secAllt.ASMS_Id)
                                                        select new SchoolYearWiseStudentDTO
                                                        {
                                                            AMST_Id = n.AMST_Id,
                                                            AMAY_RollNo = n.AMAY_RollNo
                                                        }).ToList();
                                        if (result11.Count > 0)
                                        {
                                            var rollNo = result11.OrderByDescending(e => e.AMAY_RollNo).First().AMAY_RollNo;
                                            secAllt.AMAY_RollNo = rollNo + 1;
                                        }
                                        else
                                        {
                                            secAllt.AMAY_RollNo = 1;
                                        }
                                    }
                                    else
                                    {
                                        secAllt.AMAY_RollNo = 1;
                                    }

                                    School_Adm_Y_StudentDMO sct = Mapper.Map<School_Adm_Y_StudentDMO>(ph);
                                    var result = _Context.SchoolYearWiseStudent.SingleOrDefault(t => t.ASYST_Id == sct.ASYST_Id);
                                    result.ASMS_Id = secAllt.ASMS_Id;
                                    result.AMAY_RollNo = secAllt.AMAY_RollNo;
                                    result.CreatedDate = result.CreatedDate;
                                    result.UpdatedDate = indiantime0;
                                    result.ASYST_UpdatedBy = secAllt.LoginId;
                                    result.LoginId = result.LoginId;
                                    result.ASMCL_Id = result.ASMCL_Id;
                                    result.AMST_Id = result.AMST_Id;
                                    result.AMAY_PassFailFlag = result.AMAY_PassFailFlag;
                                    result.AMAY_DateTime = result.AMAY_DateTime;
                                    result.AMAY_ActiveFlag = result.AMAY_ActiveFlag;
                                    result.ASMAY_Id = result.ASMAY_Id;
                                    _Context.Update(result);
                                    var flag = _Context.SaveChanges();
                                    if (flag > 0)
                                    {


                                        secAllt.returnVal = true;

                                        var attendanceDone = (from m in _Context.Adm_studentAttendance
                                                              from n in _Context.Adm_studentAttendanceStudents
                                                              where (m.ASA_Id == n.ASA_Id && n.AMST_Id == ph.AMST_Id && m.ASMCL_Id == secAllt.ASMCL_Id && m.ASMS_Id == Old_ASMS_Id && m.ASMAY_Id == secAllt.ASMAY_Id)
                                                              select n
                                                  ).ToList();


                                        if (attendanceDone.Count > 0)
                                        {
                                            var confirmstatus = 0;
                                            //var confirmstatus = 0;
                                            confirmstatus = _Context.Database.ExecuteSqlCommand("Adm_AttendanceAutotransfer_sectionChange @p0,@p1,@p2,@p3,@p4,@p5", ph.AMST_Id, secAllt.ASMCL_Id, secAllt.ASMS_Id, Old_ASMS_Id, secAllt.ASMAY_Id, secAllt.MI_Id);
                                            if (confirmstatus != 0)
                                            {
                                                secAllt.returnValattendance = true;
                                            }
                                            else
                                            {
                                                secAllt.returnValattendance = false;
                                            }

                                        }
                                        else
                                        {
                                            secAllt.returnVal = false;
                                        }


                                        //       using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                                        //       {
                                        //           cmd.CommandText = "Adm_ClassSection_Update";
                                        //           cmd.CommandType = CommandType.StoredProcedure;
                                        //           cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                        //   SqlDbType.BigInt)
                                        //           {
                                        //               Value = secAllt.MI_Id
                                        //           });
                                        //           cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                        //  SqlDbType.BigInt)
                                        //           {
                                        //               Value = secAllt.ASMAY_Id
                                        //           });
                                        //           cmd.Parameters.Add(new SqlParameter("@ASMCL_ID",
                                        //  SqlDbType.BigInt)
                                        //           {
                                        //               Value = secAllt.ASMCL_Id
                                        //           });
                                        //           cmd.Parameters.Add(new SqlParameter("@ASMS_ID",
                                        //  SqlDbType.BigInt)
                                        //           {
                                        //               Value = secAllt.ASMS_Id
                                        //           });
                                        //           cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                                        //SqlDbType.BigInt)
                                        //           {
                                        //               Value = result.AMST_Id
                                        //           });

                                        //           if (cmd.Connection.State != ConnectionState.Open)
                                        //               cmd.Connection.Open();

                                        //           var retObject = new List<dynamic>();
                                        //           try
                                        //           {
                                        //               using (var dataReader = cmd.ExecuteReader())
                                        //               {
                                        //                   while (dataReader.Read())
                                        //                   {
                                        //                       var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        //                       for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        //                       {
                                        //                           dataRow.Add(
                                        //                               dataReader.GetName(iFiled),
                                        //                              dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        //                           );
                                        //                       }
                                        //                       retObject.Add((ExpandoObject)dataRow);
                                        //                   }
                                        //               }
                                        //               secAllt.section_update = retObject.ToArray();
                                        //           }
                                        //           catch (Exception ex)
                                        //           {
                                        //               Console.WriteLine(ex.Message);
                                        //           }
                                        //       }




                                        var admConfig = _Context.AdmissionStandardDMO.Single(t => t.MI_Id == secAllt.MI_Id);
                                        var studDet = _Context.Adm_M_Student.Where(t => t.MI_Id == secAllt.MI_Id && t.AMST_Id == ph.AMST_Id).ToList();

                                        string emailids = "";
                                        long mobileids = 0;

                                        if (admConfig.ASC_DefaultSMS_Flag == "M")
                                        {
                                            emailids = studDet.FirstOrDefault().AMST_MotherEmailId;
                                            mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo);
                                        }
                                        else if (admConfig.ASC_DefaultSMS_Flag == "F")
                                        {
                                            emailids = studDet.FirstOrDefault().AMST_FatheremailId;
                                            mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo);
                                        }
                                        else
                                        {
                                            emailids = studDet.FirstOrDefault().AMST_emailId;
                                            mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo);
                                        }
                                        try
                                        {
                                            SMS sms = new SMS(_Context);
                                            string s = sms.sendsmssection(secAllt.MI_Id, mobileids, "SECTION_ALLOTMENT_CHANGE", ph.AMST_Id, secAllt.ASMAY_Id).Result;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        try
                                        {
                                            Email Email = new Email(_Context);
                                            string m = Email.sendmailsection(secAllt.MI_Id, emailids, "SECTION_ALLOTMENT_CHANGE", ph.AMST_Id, secAllt.ASMAY_Id);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        secAllt.returnVal = false;
                                    }
                                }
                                else
                                {
                                    secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                    return secAllt;
                                }
                                //}
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                // delete  Section details

                if (secAllt.SectionAllotmentType == "Delete")
                {
                    try
                    {
                        if (secAllt.resultData.Count() > 0)
                        {
                            foreach (SchoolYearWiseStudentDTO ph in secAllt.resultData)
                            {
                                try
                                {
                                    var attendance_entryhappen = (from m in _Context.Adm_studentAttendance
                                                                  from t in _Context.Adm_studentAttendanceStudents
                                                                  where (m.ASA_Id == t.ASA_Id && m.MI_Id == secAllt.MI_Id && m.ASMAY_Id == ph.ASMAY_Id
                                                                  && m.ASMCL_Id == ph.ASMCL_Id && m.ASMS_Id == ph.ASMS_Id && t.AMST_Id == ph.AMST_Id)
                                                                  select new SchoolYearWiseStudentDTO
                                                                  {
                                                                      AMST_Id = t.AMST_Id,
                                                                  }).ToList();

                                    var fee_studentstatus_entryhappen = _Context.feestudentstatus.Where(fs => fs.MI_Id == secAllt.MI_Id && fs.ASMAY_Id == ph.ASMAY_Id
                                    && fs.AMST_Id == ph.AMST_Id).ToList();

                                    var fee_stdmasterinst = (from a in _Context.studentgroupinstallmapping
                                                             from b in _Context.studentgroupmapping
                                                             where a.FMSG_Id == b.FMSG_Id && b.MI_Id == secAllt.MI_Id && b.ASMAY_Id == secAllt.ASMAY_Id
                                                             && b.AMST_Id == ph.AMST_Id && b.FMSG_ActiveFlag.Contains("Y")
                                                             select new SchoolYearWiseStudentDTO
                                                             {
                                                                 AMST_Id = b.AMST_Id,
                                                             }).ToList();
                                    string str = "";

                                    if (attendance_entryhappen.Count > 0 || fee_studentstatus_entryhappen.Count > 0 || fee_stdmasterinst.Count > 0)
                                    {
                                        str += ph.AMST_FirstName + ",";

                                        // return secAllt;
                                    }
                                    if (str != null && str != "")
                                    {
                                        secAllt.returnMsg += "For " + str + "    Attendance Entry /Fee Mapping  Is Already Done So You Can not Delete The Section";
                                    }

                                    if (attendance_entryhappen.Count == 0 && fee_studentstatus_entryhappen.Count == 0 && fee_stdmasterinst.Count == 0)
                                    {
                                        School_Adm_Y_StudentDMO sct = Mapper.Map<School_Adm_Y_StudentDMO>(ph);
                                        var result = _Context.SchoolYearWiseStudent.Single(t => t.ASYST_Id == sct.ASYST_Id);
                                        _Context.Remove(result);
                                        var flag = _Context.SaveChanges();
                                        if (flag > 0)
                                        {
                                            secAllt.returnVal = true;
                                        }
                                        else
                                        {
                                            secAllt.returnVal = false;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //add Student Section details
                if (secAllt.SectionAllotmentType == "Promotion")
                {
                    try
                    {
                        if (secAllt.resultData.Count() > 0)
                        {
                            foreach (SchoolYearWiseStudentDTO ph in secAllt.resultData)
                            {
                                try
                                {

                                    var createdcount = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag == 1 && t.ASMCL_Id == secAllt.ASMCL_Id && t.ASMS_Id == secAllt.ASMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();

                                    if (createdcount.Count < MaxCapacity)
                                    {
                                        School_Adm_Y_StudentDMO sct = Mapper.Map<School_Adm_Y_StudentDMO>(ph);
                                        if (sct.ASYST_Id > 0)
                                        {
                                            var result = _Context.SchoolYearWiseStudent.Single(t => t.ASYST_Id == ph.ASYST_Id);
                                            result.AMAY_ActiveFlag = 1;
                                            result.UpdatedDate = indiantime0;
                                            result.ASYST_UpdatedBy = secAllt.LoginId;
                                            result.AMAY_DateTime = result.AMAY_DateTime;
                                            result.ASMAY_Id = result.ASMAY_Id;
                                            result.AMAY_PassFailFlag = result.AMAY_PassFailFlag;
                                            result.AMAY_RollNo = result.AMAY_RollNo;
                                            result.AMST_Id = result.AMST_Id;
                                            result.ASMCL_Id = result.ASMCL_Id;
                                            result.ASMS_Id = result.ASMS_Id;
                                            result.CreatedDate = result.CreatedDate;
                                            result.LoginId = result.LoginId;
                                            _Context.Update(result);
                                            _Context.SaveChanges();
                                        }
                                        if (count.Count > 0)
                                        {
                                            var result = (from m in _Context.Adm_M_Student
                                                          from n in _Context.SchoolYearWiseStudent
                                                          where (m.AMST_Id == n.AMST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                          && n.ASMCL_Id == secAllt.ASMCL_Id && n.ASMS_Id == secAllt.ASMS_Id)
                                                          select new SchoolYearWiseStudentDTO
                                                          {
                                                              AMST_Id = n.AMST_Id,
                                                              AMAY_RollNo = n.AMAY_RollNo
                                                          }).ToList();
                                            if (result.Count > 0)
                                            {
                                                var rollNo = result.OrderByDescending(e => e.AMAY_RollNo).First().AMAY_RollNo;
                                                secAllt.AMAY_RollNo = rollNo + 1;
                                            }
                                            else
                                            {
                                                secAllt.AMAY_RollNo = 1;
                                            }
                                        }
                                        else
                                        {
                                            secAllt.AMAY_RollNo = 1;
                                        }
                                        School_Adm_Y_StudentDMO sct1 = new School_Adm_Y_StudentDMO
                                        {
                                            AMST_Id = sct.AMST_Id,
                                            ASMAY_Id = secAllt.ASMAY_Id,
                                            ASMCL_Id = secAllt.ASMCL_Id,
                                            ASMS_Id = secAllt.ASMS_Id,
                                            LoginId = secAllt.LoginId,
                                            AMAY_RollNo = secAllt.AMAY_RollNo,
                                            AMAY_ActiveFlag = 1,
                                            AMAY_PassFailFlag = 0,
                                            AMAY_DateTime = indiantime0,
                                            CreatedDate = indiantime0,
                                            UpdatedDate = indiantime0,
                                            ASYST_UpdatedBy = secAllt.LoginId,
                                            ASYST_CreatedBy = secAllt.LoginId
                                        };

                                        _Context.Add(sct1);
                                        var flag = _Context.SaveChanges();
                                        if (flag > 0)
                                        {
                                            secAllt.returnVal = true;

                                            var admConfig = _Context.AdmissionStandardDMO.Single(t => t.MI_Id == secAllt.MI_Id);
                                            var studDet = _Context.Adm_M_Student.Where(t => t.MI_Id == secAllt.MI_Id && t.AMST_Id == ph.AMST_Id).ToList();

                                            // Fee Auto Mapping For Promotion
                                            var feeflg = _Context.FeeMasterConfigurationDMO.Where(a => a.MI_Id == secAllt.MI_Id).ToList();

                                            if (feeflg.FirstOrDefault().FMC_Areawise_FeeFlg == 1)
                                            {
                                                try
                                                {
                                                    var confirmstatus = _Context.Database.ExecuteSqlCommand("Automapping_Promotion_Opening_Balance @p0,@p1,@p2,@p3",
                                ph.AMST_Id, secAllt.MI_Id, secAllt.ASMAY_Id, secAllt.LoginId);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }





                                            string emailids = "";
                                            long mobileids = 0;

                                            if (admConfig.ASC_DefaultSMS_Flag == "M")
                                            {
                                                emailids = studDet.FirstOrDefault().AMST_MotherEmailId;
                                                mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo);
                                            }
                                            else if (admConfig.ASC_DefaultSMS_Flag == "F")
                                            {
                                                emailids = studDet.FirstOrDefault().AMST_FatheremailId;
                                                mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo);
                                            }
                                            else
                                            {
                                                emailids = studDet.FirstOrDefault().AMST_emailId;
                                                mobileids = Convert.ToInt64(studDet.FirstOrDefault().AMST_MobileNo);
                                            }
                                            try
                                            {
                                                SMS sms = new SMS(_Context);
                                                string s = sms.sendsmssection(secAllt.MI_Id, mobileids, "SECTION_ALLOTMENT_PROMOTED", ph.AMST_Id,
                                                    secAllt.ASMAY_Id).Result;
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            try
                                            {
                                                Email Email = new Email(_Context);
                                                string m = Email.sendmailsection(secAllt.MI_Id, emailids, "SECTION_ALLOTMENT_PROMOTED", ph.AMST_Id, secAllt.ASMAY_Id);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            secAllt.returnVal = false;
                                        }
                                    }
                                    else
                                    {
                                        secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                        return secAllt;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //add Student Section details
                //if (secAllt.SectionAllotmentType == "YearLoss")
                //{
                //    try
                //    {
                //        if (secAllt.resultData.Count() > 0)
                //        {
                //            foreach (SchoolYearWiseStudentDTO ph in secAllt.resultData)
                //            {
                //                try
                //                {
                //                    var createdcount = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag.Equals(1) && t.ASMCL_Id == secAllt.ASMCL_Id && t.ASMS_Id == secAllt.ASMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();

                //                    if (createdcount.Count < MaxCapacity)
                //                    {
                //                        School_Adm_Y_StudentDMO sct = Mapper.Map<School_Adm_Y_StudentDMO>(ph);

                //                        if (sct.ASYST_Id > 0)
                //                        {
                //                            var result = _Context.SchoolYearWiseStudent.Single(t => t.ASYST_Id == ph.ASYST_Id);                                        
                //                            result.UpdatedDate = indiantime0;
                //                            result.ASYST_UpdatedBy = secAllt.LoginId;
                //                            result.AMAY_DateTime = result.AMAY_DateTime;
                //                            result.ASMAY_Id = result.ASMAY_Id;
                //                            result.AMAY_PassFailFlag = result.AMAY_PassFailFlag;
                //                            result.AMAY_RollNo = result.AMAY_RollNo;
                //                            result.AMST_Id = result.AMST_Id;
                //                            result.ASMCL_Id = result.ASMCL_Id;
                //                            result.ASMS_Id = result.ASMS_Id;
                //                            result.CreatedDate = result.CreatedDate;
                //                            result.LoginId = result.LoginId;
                //                            _Context.Update(result);
                //                            _Context.SaveChanges();
                //                        }
                //                        if (count.Count > 0)
                //                        {
                //                            var result = (from m in _Context.Adm_M_Student
                //                                          from n in _Context.SchoolYearWiseStudent
                //                                          where (m.AMST_Id == n.AMST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                //                                          && n.ASMCL_Id == secAllt.ASMCL_Id && n.ASMS_Id == secAllt.ASMS_Id)
                //                                          select new SchoolYearWiseStudentDTO
                //                                          {
                //                                              AMST_Id = n.AMST_Id,
                //                                              AMAY_RollNo = n.AMAY_RollNo
                //                                          }).ToList();
                //                            if (result.Count > 0)
                //                            {
                //                                var rollNo = result.OrderByDescending(e => e.AMAY_RollNo).First().AMAY_RollNo;
                //                                secAllt.AMAY_RollNo = rollNo + 1;
                //                            }
                //                            else
                //                            {
                //                                secAllt.AMAY_RollNo = 1;
                //                            }

                //                        }
                //                        else
                //                        {
                //                            secAllt.AMAY_RollNo = 1;
                //                        }
                //                        School_Adm_Y_StudentDMO sct1 = new School_Adm_Y_StudentDMO
                //                        {
                //                            AMST_Id = sct.AMST_Id,
                //                            ASMAY_Id = secAllt.ASMAY_Id,
                //                            ASMCL_Id = secAllt.ASMCL_Id,
                //                            ASMS_Id = secAllt.ASMS_Id,
                //                            LoginId = secAllt.LoginId,
                //                            AMAY_RollNo = secAllt.AMAY_RollNo,
                //                            AMAY_ActiveFlag = 1,
                //                            AMAY_PassFailFlag = 0,
                //                            AMAY_DateTime = indiantime0,
                //                            CreatedDate = indiantime0,
                //                            UpdatedDate = indiantime0,
                //                            ASYST_UpdatedBy = secAllt.LoginId,
                //                            ASYST_CreatedBy = secAllt.LoginId
                //                        };

                //                        _Context.Add(sct1);
                //                        var flag = _Context.SaveChanges();
                //                        if (flag > 0)
                //                        {
                //                            secAllt.returnVal = true;
                //                        }
                //                        else
                //                        {
                //                            secAllt.returnVal = false;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                //                        return secAllt;
                //                    }
                //                }
                //                catch (Exception ex)
                //                {
                //                    Console.WriteLine(ex.Message);
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //    }
                //}

                //added By Roopa
                if (secAllt.SectionAllotmentType == "YearLoss")
                {
                    try
                    {
                        if (secAllt.resultData.Count() > 0)
                        {
                            secAllt.categoryflag = false;
                            foreach (SchoolYearWiseStudentDTO ph in secAllt.resultData)
                            {
                                try
                                {
                                    //var attendance_entryhappen = (from m in _Context.Adm_studentAttendance
                                    //                              from t in _Context.Adm_studentAttendanceStudents
                                    //                              where (m.ASA_Id == t.ASA_Id && m.MI_Id == secAllt.MI_Id && m.ASMAY_Id == ph.ASMAY_Id
                                    //                              && m.ASMCL_Id == ph.ASMCL_Id && m.ASMS_Id == ph.ASMS_Id && t.AMST_Id == ph.AMST_Id)
                                    //                              select new SchoolYearWiseStudentDTO
                                    //                              {
                                    //                                  AMST_Id = t.AMST_Id,
                                    //                              }).ToList();
                                    //var fee_studentstatus_entryhappen = _Context.feestudentstatus.Where(fs => fs.MI_Id == secAllt.MI_Id && fs.ASMAY_Id == ph.ASMAY_Id
                                    //&& fs.AMST_Id == ph.AMST_Id).ToList();

                                    //var fee_stdmasterinst = (from a in _Context.studentgroupinstallmapping
                                    //                         from b in _Context.studentgroupmapping
                                    //                         where a.FMSG_Id == b.FMSG_Id && b.MI_Id == secAllt.MI_Id && b.ASMAY_Id == secAllt.ASMAY_Id
                                    //                         && b.AMST_Id == ph.AMST_Id && b.FMSG_ActiveFlag.Contains("Y")
                                    //                         select new SchoolYearWiseStudentDTO
                                    //                         {
                                    //                             AMST_Id = b.AMST_Id,
                                    //                         }).ToList();
                                    //string str = "";
                                    //if (attendance_entryhappen.Count > 0 || fee_studentstatus_entryhappen.Count > 0 || fee_stdmasterinst.Count > 0)
                                    //{
                                    //    str += ph.AMST_FirstName + "," + ph.AMST_AdmNo;
                                    //    secAllt.categoryflag = true;
                                    //}
                                    //if (str != null && str != "")
                                    //{
                                    //    secAllt.returnMsg += "For " + str + " Attendance Entry /Fee Mapping Is Already Done So You Can not Year Loss The Section";
                                    //}
                                    var createdcount = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag.Equals(1) && t.ASMCL_Id == secAllt.ASMCL_Id && t.ASMS_Id == secAllt.ASMS_Id && t.ASMAY_Id == secAllt.ASMAY_Id).ToList();
                                    //if (attendance_entryhappen.Count == 0 && fee_studentstatus_entryhappen.Count == 0 && fee_stdmasterinst.Count == 0 && secAllt.categoryflag == false)
                                    //{
                                    if (createdcount.Count < MaxCapacity)
                                    {
                                        School_Adm_Y_StudentDMO sct = Mapper.Map<School_Adm_Y_StudentDMO>(ph);

                                        if (sct.ASYST_Id > 0)
                                        {
                                            var result = _Context.SchoolYearWiseStudent.Single(t => t.ASYST_Id == ph.ASYST_Id);
                                            result.UpdatedDate = indiantime0;
                                            result.ASYST_UpdatedBy = secAllt.LoginId;
                                            result.AMAY_DateTime = result.AMAY_DateTime;
                                            result.ASMAY_Id = result.ASMAY_Id;
                                            result.AMAY_PassFailFlag = result.AMAY_PassFailFlag;
                                            result.AMAY_RollNo = result.AMAY_RollNo;
                                            result.AMST_Id = result.AMST_Id;
                                            result.ASMCL_Id = result.ASMCL_Id;
                                            result.ASMS_Id = result.ASMS_Id;
                                            result.CreatedDate = result.CreatedDate;
                                            result.LoginId = result.LoginId;
                                            _Context.Update(result);
                                        }
                                        if (count.Count > 0)
                                        {
                                            var result = (from m in _Context.Adm_M_Student
                                                          from n in _Context.SchoolYearWiseStudent
                                                          where (m.AMST_Id == n.AMST_Id && m.MI_Id == secAllt.MI_Id && n.ASMAY_Id == secAllt.ASMAY_Id
                                                          && n.ASMCL_Id == secAllt.ASMCL_Id && n.ASMS_Id == secAllt.ASMS_Id)
                                                          select new SchoolYearWiseStudentDTO
                                                          {
                                                              AMST_Id = n.AMST_Id,
                                                              AMAY_RollNo = n.AMAY_RollNo
                                                          }).ToList();
                                            if (result.Count > 0)
                                            {
                                                var rollNo = result.OrderByDescending(e => e.AMAY_RollNo).First().AMAY_RollNo;
                                                secAllt.AMAY_RollNo = rollNo + 1;
                                            }
                                            else
                                            {
                                                secAllt.AMAY_RollNo = 1;
                                            }

                                        }
                                        else
                                        {
                                            secAllt.AMAY_RollNo = 1;
                                        }
                                        School_Adm_Y_StudentDMO sct1 = new School_Adm_Y_StudentDMO
                                        {
                                            AMST_Id = sct.AMST_Id,
                                            ASMAY_Id = secAllt.ASMAY_Id,
                                            ASMCL_Id = secAllt.ASMCL_Id,
                                            ASMS_Id = secAllt.ASMS_Id,
                                            LoginId = secAllt.LoginId,
                                            AMAY_RollNo = secAllt.AMAY_RollNo,
                                            AMAY_ActiveFlag = 1,
                                            AMAY_PassFailFlag = 0,
                                            AMAY_DateTime = indiantime0,
                                            CreatedDate = indiantime0,
                                            UpdatedDate = indiantime0,
                                            ASYST_UpdatedBy = secAllt.LoginId,
                                            ASYST_CreatedBy = secAllt.LoginId
                                        };

                                        _Context.Add(sct1);
                                    }

                                    else
                                    {
                                        secAllt.returnMsg = "Maximum limit for this section is exceeded.";
                                        return secAllt;
                                    }
                                    // }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (secAllt.categoryflag == false)
                            {
                                var flag = _Context.SaveChanges();
                                if (flag > 0)
                                {
                                    secAllt.returnVal = true;
                                }
                                else
                                {
                                    secAllt.returnVal = false;
                                }
                            }
                            else
                            {
                                secAllt.returnVal = false;
                            }



                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return secAllt;
        }

        // GetstudentdetailsbyYearandclass
        public SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass(SchoolYearWiseStudentDTO stu)
        {
            try
            {
                string pattern = "\\s+";
                string replacement = " ";
                SchoolYearWiseStudentDTO dto = new SchoolYearWiseStudentDTO();

                if (stu.ASMS_Id > 0)
                {
                    var studentlist = (from m in _Context.Adm_M_Student
                                       from n in _Context.SchoolYearWiseStudent
                                       from o in _Context.School_M_Class
                                       from p in _Context.School_M_Section
                                       where (m.AMST_Id == n.AMST_Id && n.ASMCL_Id == o.ASMCL_Id && n.ASMS_Id == p.ASMS_Id
                                       && m.MI_Id == stu.MI_Id && m.AMST_ActiveFlag == 1 && m.AMST_SOL.Equals("S") && n.AMAY_ActiveFlag == 1 && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id && n.ASMS_Id == stu.ASMS_Id)
                                       group new { m, n, o, p }
                                       by new { m.AMST_FirstName, m.AMST_MiddleName, m.AMST_LastName, n.AMST_Id, o.ASMCL_ClassName, p.ASMC_SectionName } into g
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           StudentName = g.FirstOrDefault().m.AMST_FirstName.Trim() +
                                           (g.FirstOrDefault().m.AMST_MiddleName == null || g.FirstOrDefault().m.AMST_MiddleName == "" || g.FirstOrDefault().m.AMST_MiddleName == "0" ? "" : " " + g.FirstOrDefault().m.AMST_MiddleName) +
                                         (g.FirstOrDefault().m.AMST_LastName == null || g.FirstOrDefault().m.AMST_LastName == "" || g.FirstOrDefault().m.AMST_LastName == "0" ?
                                         "" : " " + g.FirstOrDefault().m.AMST_LastName),
                                           ASMCL_ClassName = g.FirstOrDefault().o.ASMCL_ClassName,
                                           ASMC_SectionName = g.FirstOrDefault().p.ASMC_SectionName,
                                           AMAY_RollNo = g.FirstOrDefault().n.AMAY_RollNo,
                                           AMST_AdmNo = g.FirstOrDefault().m.AMST_AdmNo
                                       }).ToList();

                    for (int i = 0; i < studentlist.Count; i++)
                    {
                        Regex rx = new Regex(pattern);
                        studentlist[i].StudentName = rx.Replace(studentlist[i].StudentName, replacement);
                    }
                    stu.sectionAllotedStudentList = studentlist.ToArray();
                    if (stu.sectionAllotedStudentList.Length > 0)
                    {
                        stu.count = stu.sectionAllotedStudentList.Length;
                    }
                    else
                    {
                        stu.count = 0;
                    }
                }
                else
                {
                    var studentlist = (from m in _Context.Adm_M_Student
                                       from n in _Context.SchoolYearWiseStudent
                                       from o in _Context.School_M_Class
                                       from p in _Context.School_M_Section
                                       where (m.AMST_Id == n.AMST_Id && n.ASMCL_Id == o.ASMCL_Id && n.ASMS_Id == p.ASMS_Id
                                       && m.MI_Id == stu.MI_Id && m.AMST_ActiveFlag == 1 && m.AMST_SOL.Equals("S") && n.AMAY_ActiveFlag == 1 && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id)
                                       group new { m, n, o, p }
                                        by new { m.AMST_FirstName, m.AMST_MiddleName, m.AMST_LastName, n.AMST_Id, o.ASMCL_ClassName, p.ASMC_SectionName } into g
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           StudentName = g.FirstOrDefault().m.AMST_FirstName.Trim() +
                                           (g.FirstOrDefault().m.AMST_MiddleName == null || g.FirstOrDefault().m.AMST_MiddleName == "" || g.FirstOrDefault().m.AMST_MiddleName == "0" ? "" : " " + g.FirstOrDefault().m.AMST_MiddleName) +
                                         (g.FirstOrDefault().m.AMST_LastName == null || g.FirstOrDefault().m.AMST_LastName == "" || g.FirstOrDefault().m.AMST_LastName == "0" ? "" : " " + g.FirstOrDefault().m.AMST_LastName),
                                           ASMCL_ClassName = g.FirstOrDefault().o.ASMCL_ClassName,
                                           ASMC_SectionName = g.FirstOrDefault().p.ASMC_SectionName,
                                           AMAY_RollNo = g.FirstOrDefault().n.AMAY_RollNo,
                                           AMST_AdmNo = g.FirstOrDefault().m.AMST_AdmNo
                                       }).ToList();

                    for (int i = 0; i < studentlist.Count; i++)
                    {
                        Regex rx = new Regex(pattern);
                        studentlist[i].StudentName = rx.Replace(studentlist[i].StudentName, replacement);
                    }
                    stu.sectionAllotedStudentList = studentlist.ToArray();

                    if (stu.sectionAllotedStudentList.Length > 0)
                    {
                        stu.count = stu.sectionAllotedStudentList.Length;
                    }
                    else
                    {
                        stu.count = 0;
                    }
                }
                // New Section details
                if (stu.SectionAllotmentType == "New")
                {

                    try
                    {
                        var stdyear = _Context.SchoolYearWiseStudent.Where(t => t.AMAY_ActiveFlag == 1).Select(d => d.AMST_Id).ToArray();

                        if (stdyear.Length > 0)
                        {
                            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "adm_sectionallotment_change_promotion_yearloss";
                                cmd.CommandType = CommandType.StoredProcedure;

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = stu.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.Int) { Value = stu.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = stu.MI_Id });

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
                                                    dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                                );
                                            }
                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    stu.StudentList = retObject.ToArray();
                                }

                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                }
                            }
                            if (stu.StudentList.Length > 0)
                            {
                                stu.studentlistCount = stu.StudentList.Length;
                            }
                            else
                            {
                                stu.studentlistCount = 0;
                            }
                        }
                        else
                        {
                            stu.StudentList = _Context.Adm_M_Student.Where(t => t.AMST_ActiveFlag == 1 &&
                            t.MI_Id == stu.MI_Id && t.ASMAY_Id == stu.ASMAY_Id && t.ASMCL_Id == stu.ASMCL_Id && t.AMST_SOL.Equals("S")).ToArray();
                            if (stu.StudentList.Length > 0)
                            {
                                stu.studentlistCount = stu.StudentList.Length;
                            }
                            else
                            {
                                stu.studentlistCount = 0;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }


                // Change Section details
                if (stu.SectionAllotmentType == "Change")
                {

                    try
                    {
                        stu.StudentListYear = (from sy in _Context.SchoolYearWiseStudent
                                               from s in _Context.Adm_M_Student
                                               from c in _Context.School_M_Class
                                               from sec in _Context.School_M_Section
                                               from y in _Context.AcademicYear
                                               where sy.AMST_Id == s.AMST_Id && sy.ASMCL_Id == c.ASMCL_Id &&
                                               sy.ASMS_Id == sec.ASMS_Id && sy.ASMAY_Id == y.ASMAY_Id &&
                                               s.AMST_ActiveFlag == 1 && s.AMST_SOL.Equals("S") &&
                                               sy.ASMAY_Id == stu.ASMAY_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMS_Id == stu.ASMS_Id
                                               && sy.AMAY_ActiveFlag == 1 && s.MI_Id == stu.MI_Id
                                               select new SchoolYearWiseStudentDTO
                                               {
                                                   ASYST_Id = sy.ASYST_Id,
                                                   AMST_Id = sy.AMST_Id,
                                                   AMST_FirstName = s.AMST_FirstName,
                                                   AMST_MiddleName = s.AMST_MiddleName,
                                                   AMST_LastName = s.AMST_LastName,
                                                   ASMCL_Id = sy.ASMCL_Id,
                                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                                   ASMS_Id = sy.ASMS_Id,
                                                   ASMC_SectionName = sec.ASMC_SectionName,
                                                   AMAY_RollNo = sy.AMAY_RollNo,
                                                   ASMAY_Id = sy.ASMAY_Id,
                                                   ASMAY_Year = y.ASMAY_Year,
                                                   AMAY_PassFailFlag = Convert.ToInt32(sy.AMAY_PassFailFlag),
                                                   AMAY_ActiveFlag = sy.AMAY_ActiveFlag,
                                                   LoginId = Convert.ToInt64(sy.LoginId),
                                                   AMAY_DateTime = Convert.ToDateTime(sy.AMAY_DateTime),
                                                   CreatedDate = sy.CreatedDate,
                                                   UpdatedDate = sy.UpdatedDate,
                                                   AMST_AdmNo = s.AMST_AdmNo


                                               }).ToArray();
                        if (stu.StudentListYear.Length > 0)
                        {
                            stu.studentListYearCount = stu.StudentListYear.Length;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }



                // Delete Section details
                if (stu.SectionAllotmentType == "Delete")
                {
                    try
                    {
                        stu.StudentListYear = (from sy in _Context.SchoolYearWiseStudent
                                               from s in _Context.Adm_M_Student
                                               from c in _Context.School_M_Class
                                               from sec in _Context.School_M_Section
                                               from y in _Context.AcademicYear
                                               where sy.AMST_Id == s.AMST_Id && sy.ASMCL_Id == c.ASMCL_Id &&
                                               sy.ASMS_Id == sec.ASMS_Id && sy.ASMAY_Id == y.ASMAY_Id &&
                                               s.AMST_ActiveFlag == 1 && s.AMST_SOL.Equals("S") &&
                                               sy.ASMAY_Id == stu.ASMAY_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMS_Id == stu.ASMS_Id
                                                && sy.AMAY_ActiveFlag == 1 && s.MI_Id == stu.MI_Id
                                               select new SchoolYearWiseStudentDTO
                                               {
                                                   ASYST_Id = sy.ASYST_Id,
                                                   AMST_Id = sy.AMST_Id,
                                                   AMST_FirstName = s.AMST_FirstName,
                                                   AMST_MiddleName = s.AMST_MiddleName,
                                                   AMST_LastName = s.AMST_LastName,
                                                   ASMCL_Id = sy.ASMCL_Id,
                                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                                   ASMS_Id = sy.ASMS_Id,
                                                   ASMC_SectionName = sec.ASMC_SectionName,
                                                   AMAY_RollNo = sy.AMAY_RollNo,
                                                   ASMAY_Id = sy.ASMAY_Id,
                                                   ASMAY_Year = y.ASMAY_Year,
                                                   AMST_AdmNo = s.AMST_AdmNo

                                               }).ToArray();
                        if (stu.StudentListYear.Length > 0)
                        {
                            stu.studentListYearCount = stu.StudentListYear.Length;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                //  Section details
                if (stu.SectionAllotmentType == "Promotion")
                {
                    List<SchoolYearWiseStudentDTO> SchoolYearWiseStudentDTO = new List<SchoolYearWiseStudentDTO>();
                    List<Temp_SchoolYearWiseStudentDTO> result = new List<Temp_SchoolYearWiseStudentDTO>();

                    try
                    {
                        var classid = stu.ASMCL_Id;
                        SchoolYearWiseStudentDTO = (from sy in _Context.SchoolYearWiseStudent
                                                    from s in _Context.Adm_M_Student
                                                    from c in _Context.School_M_Class
                                                    from sec in _Context.School_M_Section
                                                    from y in _Context.AcademicYear
                                                    where sy.AMST_Id == s.AMST_Id && sy.ASMCL_Id == c.ASMCL_Id &&
                                                    sy.ASMS_Id == sec.ASMS_Id && sy.ASMAY_Id == y.ASMAY_Id &&
                                                    s.AMST_ActiveFlag == 1 && s.AMST_SOL.Equals("S") &&
                                                    sy.ASMAY_Id == stu.ASMAY_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMS_Id == stu.ASMS_Id
                                                     && sy.AMAY_ActiveFlag == 1 && s.MI_Id == stu.MI_Id
                                                    select new SchoolYearWiseStudentDTO
                                                    {
                                                        ASYST_Id = sy.ASYST_Id,
                                                        AMST_Id = sy.AMST_Id,
                                                        AMST_FirstName = s.AMST_FirstName,
                                                        AMST_MiddleName = s.AMST_MiddleName,
                                                        AMST_LastName = s.AMST_LastName,
                                                        ASMCL_Id = sy.ASMCL_Id,
                                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                                        ASMS_Id = sy.ASMS_Id,
                                                        ASMC_SectionName = sec.ASMC_SectionName,
                                                        AMAY_RollNo = sy.AMAY_RollNo,
                                                        ASMAY_Id = sy.ASMAY_Id,
                                                        ASMAY_Year = y.ASMAY_Year,
                                                        AMST_AdmNo = s.AMST_AdmNo

                                                    }).ToList();
                        if (SchoolYearWiseStudentDTO.Count > 0)
                        {
                            stu.studentListYearCount = SchoolYearWiseStudentDTO.Count;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }

                        int NoOfYears = stu.NoOfYears;

                        int order = 0;

                        string Selectedyear = _Context.AcademicYear.Single(y => y.ASMAY_Id == stu.ASMAY_Id).ASMAY_Year;

                        int Selectedyearorder = _Context.AcademicYear.Single(y => y.ASMAY_Id == stu.ASMAY_Id).ASMAY_Order;

                        string[] SelectedyearArray = Selectedyear.Split('-');

                        int firstfield = Convert.ToInt32(SelectedyearArray.ElementAt(0)) + NoOfYears;
                        int lastfield = Convert.ToInt32(SelectedyearArray.ElementAt(1)) + NoOfYears;
                        string ConvertedYear = firstfield + "-" + lastfield;

                        order = Selectedyearorder + NoOfYears;

                        List<MasterAcademic> year = new List<MasterAcademic>();
                        year = _Context.AcademicYear.Where(y => y.ASMAY_Order == order && y.MI_Id == stu.MI_Id && y.Is_Active == true).OrderBy(t => t.ASMAY_Order).ToList();

                        stu.YearList = year.ToArray();

                        List<School_M_Class> classl = new List<School_M_Class>();
                        classl = _Context.School_M_Class.Where(c => c.ASMCL_Id == stu.ASMCL_Id && c.ASMCL_ActiveFlag == true && c.MI_Id == stu.MI_Id).ToList();
                        int classOrder = classl.FirstOrDefault().ASMCL_Order + stu.NoOfYears;

                        var classll = _Context.School_M_Class.Where(c => c.ASMCL_Order == classOrder && c.MI_Id == stu.MI_Id && c.ASMCL_ActiveFlag == true && c.MI_Id == stu.MI_Id).ToList();
                        if (classll.Count > 0)
                        {
                            stu.ASMCL_Id = classll.FirstOrDefault().ASMCL_Id;
                        }
                        else
                        {
                            stu.ASMCL_Id = 0;
                        }

                        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Adm_Classwisestudentpromoteddetails";
                            cmd.CommandType = CommandType.StoredProcedure;

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            cmd.Parameters.Add(new SqlParameter("@promotedyear", SqlDbType.VarChar) { Value = year.FirstOrDefault().ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@promotedclass", SqlDbType.VarChar) { Value = classll.FirstOrDefault().ASMCL_Id });

                            cmd.Parameters.Add(new SqlParameter("@presentyear", SqlDbType.VarChar) { Value = stu.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@presentclass", SqlDbType.VarChar) { Value = classid });
                            cmd.Parameters.Add(new SqlParameter("@presentsection", SqlDbType.VarChar) { Value = stu.ASMS_Id });

                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = stu.MI_Id });

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
                                                dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                stu.StudentListYear = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                        var present_student_list = SchoolYearWiseStudentDTO.Select(a => a.AMST_Id).ToList();
                        var amstId = result.Select(d => d.Amst_Id).ToList();
                        List<SchoolYearWiseStudentDTO> list1 = new List<SchoolYearWiseStudentDTO>();
                        for (int i = 0; i < amstId.Count; i++)
                        {
                            var S = present_student_list.Contains(amstId[i]);
                            if (S == true)
                            {
                                var query1 = SchoolYearWiseStudentDTO.Where(d => d.AMST_Id == amstId[i]).ToArray();
                                list1.CopyTo(query1);
                            }
                        }
                        stu.student1 = list1.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                //year loss details
                if (stu.SectionAllotmentType == "YearLoss")
                {
                    try
                    {
                        stu.StudentListYear = (from sy in _Context.SchoolYearWiseStudent
                                               from s in _Context.Adm_M_Student
                                               from c in _Context.School_M_Class
                                               from sec in _Context.School_M_Section
                                               from y in _Context.AcademicYear
                                               where sy.AMST_Id == s.AMST_Id && sy.ASMCL_Id == c.ASMCL_Id &&
                                               sy.ASMS_Id == sec.ASMS_Id && sy.ASMAY_Id == y.ASMAY_Id &&
                                               s.AMST_ActiveFlag == 1 && s.AMST_SOL.Equals("S") &&
                                               sy.ASMAY_Id == stu.ASMAY_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMS_Id == stu.ASMS_Id
                                                && sy.AMAY_ActiveFlag == 1 && s.MI_Id == stu.MI_Id
                                               select new SchoolYearWiseStudentDTO
                                               {
                                                   ASYST_Id = sy.ASYST_Id,
                                                   AMST_Id = sy.AMST_Id,
                                                   AMST_FirstName = s.AMST_FirstName,
                                                   AMST_MiddleName = s.AMST_MiddleName,
                                                   AMST_LastName = s.AMST_LastName,
                                                   ASMCL_Id = sy.ASMCL_Id,
                                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                                   ASMS_Id = sy.ASMS_Id,
                                                   ASMC_SectionName = sec.ASMC_SectionName,
                                                   AMAY_RollNo = sy.AMAY_RollNo,
                                                   ASMAY_Id = sy.ASMAY_Id,
                                                   ASMAY_Year = y.ASMAY_Year,
                                                   AMST_AdmNo = s.AMST_AdmNo

                                               }).ToArray();
                        if (stu.StudentListYear.Length > 0)
                        {
                            stu.studentListYearCount = stu.StudentListYear.Length;
                        }
                        else
                        {
                            stu.studentListYearCount = 0;
                        }

                        int NoOfYears = stu.NoOfYears;

                        string Selectedyear = _Context.AcademicYear.Single(y => y.ASMAY_Id == stu.ASMAY_Id).ASMAY_Year;

                        string[] SelectedyearArray = Selectedyear.Split('-');

                        int firstfield = Convert.ToInt32(SelectedyearArray.ElementAt(0)) + NoOfYears;
                        int lastfield = Convert.ToInt32(SelectedyearArray.ElementAt(1)) + NoOfYears;
                        string ConvertedYear = firstfield + "-" + lastfield;


                        List<MasterAcademic> year = new List<MasterAcademic>();
                        year = _Context.AcademicYear.Where(y => y.ASMAY_Year.Equals(ConvertedYear) && y.MI_Id == stu.MI_Id
                        && y.Is_Active == true).OrderBy(t => t.ASMAY_Order).ToList();

                        stu.YearList = year.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }

        //update roll no get details by class section
        public SchoolYearWiseStudentDTO GetStudentListByURN(SchoolYearWiseStudentDTO data)
        {
            try
            {
                //string pattern = "\\s+";
                //string replacement = " ";
                var studentlisturn = (from a in _Context.School_Adm_Y_StudentDMO
                                      from b in _Context.Adm_M_Student
                                      from c in _Context.School_M_Class
                                      from d in _Context.School_M_Section
                                      from e in _Context.AcademicYear
                                      where a.AMST_Id == b.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id &&
                                      a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id &&
                                      a.AMAY_ActiveFlag == 1 && b.AMST_SOL.Equals("S") && b.AMST_ActiveFlag == 1
                                      select new Student_Update_RollNumber
                                      {
                                          StudentName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) + (b.AMST_MiddleName.Trim() == null || b.AMST_MiddleName.Trim() == "" ? "" : " " + b.AMST_MiddleName) + (b.AMST_LastName.Trim() == null || b.AMST_LastName.Trim() == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                          AMST_AdmNo = b.AMST_AdmNo,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          AMST_Id = a.AMST_Id,
                                          ASYST_Id = a.ASYST_Id,
                                          AMAY_RollNo = a.AMAY_RollNo,
                                          pdays = a.AMAY_RollNo
                                      }).OrderBy(a => a.StudentName).ToList();

                data.studentlisturn = studentlisturn.ToArray();
                if (data.studentlisturn.Length > 0)
                {
                    data.count = data.studentlisturn.Length;
                }
                else
                {
                    data.count = 0;
                }
                var studentlisturn1 = (from a in _Context.School_Adm_Y_StudentDMO
                                       from b in _Context.Adm_M_Student
                                       from c in _Context.School_M_Class
                                       from d in _Context.School_M_Section
                                       from e in _Context.AcademicYear
                                       where a.AMST_Id == b.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id &&
                                       a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id &&
                                       a.AMAY_ActiveFlag == 1 && b.AMST_SOL.Equals("S") && b.AMST_ActiveFlag == 1
                                       select new Student_Update_RollNumber
                                       {
                                           StudentName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) + (b.AMST_MiddleName.Trim() == null || b.AMST_MiddleName.Trim() == "" ? "" : " " + b.AMST_MiddleName) + (b.AMST_LastName.Trim() == null || b.AMST_LastName.Trim() == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                           AMST_AdmNo = b.AMST_AdmNo,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           ASMC_SectionName = d.ASMC_SectionName,
                                           AMST_Id = a.AMST_Id,
                                           ASYST_Id = a.ASYST_Id,
                                           AMAY_RollNo = a.AMAY_RollNo,
                                           pdays = a.AMAY_RollNo,
                                       }).OrderBy(a => a.StudentName).ToList();

                data.studentlisturn1 = studentlisturn1.ToArray();
                if (data.studentlisturn1.Length > 0)
                {
                    data.count = data.studentlisturn1.Length;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Student_Update_RollNumber GetStudentListByURNsave(Student_Update_RollNumber data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                for (int i = 0; i < data.studentlisturn.Count(); i++)
                {
                    School_Adm_Y_StudentDMO std = new School_Adm_Y_StudentDMO();
                    var result = _Context.SchoolYearWiseStudent.Single(t => t.AMST_Id == Convert.ToInt64(data.studentlisturn[i].AMST_Id)
                    && t.ASYST_Id == Convert.ToInt64(data.studentlisturn[i].ASYST_Id));
                    result.AMAY_RollNo = Convert.ToInt64(data.studentlisturn[i].pdays);
                    result.UpdatedDate = indiantime0;
                    result.ASYST_CreatedBy = data.UserId;
                    _Context.Update(result);
                }
                var contactExists = _Context.SaveChanges();
                if (contactExists >= 1)
                {
                    data.returnal = true;
                }
                else
                {
                    data.returnal = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Change Class

        public SchoolYearWiseStudentDTO GetChangeClassDetails(SchoolYearWiseStudentDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _Context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.YearList = year.ToArray();

                data.classList = _Context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                data.sectionList = _Context.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolYearWiseStudentDTO GetStudentListByYearCLS(SchoolYearWiseStudentDTO data)
        {
            try
            {
                var studentlisturn = (from a in _Context.School_Adm_Y_StudentDMO
                                      from b in _Context.Adm_M_Student
                                      from c in _Context.School_M_Class
                                      from d in _Context.School_M_Section
                                      from e in _Context.AcademicYear
                                      where a.AMST_Id == b.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id &&
                                      a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id_CLS_Old && a.ASMS_Id == data.ASMS_Id &&
                                      a.AMAY_ActiveFlag == 1 && b.AMST_SOL.Equals("S") && b.AMST_ActiveFlag == 1
                                      select new SchoolYearWiseStudentDTO
                                      {
                                          StudentName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                          (b.AMST_MiddleName.Trim() == null || b.AMST_MiddleName.Trim() == "" ? "" : " " + b.AMST_MiddleName) +
                                          (b.AMST_LastName.Trim() == null || b.AMST_LastName.Trim() == "" ? "" : " " + b.AMST_LastName) +
                                          (b.AMST_AdmNo.Trim() == null || b.AMST_AdmNo.Trim() == "" ? "" : "/" + b.AMST_AdmNo)).Trim(),
                                          AMST_Id = a.AMST_Id
                                      }).OrderBy(a => a.StudentName).ToList();

                data.sectionAllotedStudentList = studentlisturn.ToArray();

                data.newclasslist = _Context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                && a.ASMCL_Id != data.ASMCL_Id_CLS_Old).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolYearWiseStudentDTO onstudentnamechange(SchoolYearWiseStudentDTO data)
        {
            try
            {
                var studentlisturn = (from a in _Context.School_Adm_Y_StudentDMO
                                      from b in _Context.Adm_M_Student
                                      from c in _Context.School_M_Class
                                      from d in _Context.School_M_Section
                                      from e in _Context.AcademicYear
                                      where a.AMST_Id == b.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id
                                      && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id_CLS_Old && a.ASMS_Id == data.ASMS_Id
                                      && a.AMAY_ActiveFlag == 1 && b.AMST_SOL.Equals("S") && b.AMST_ActiveFlag == 1 && a.AMST_Id == data.AMST_Id
                                      select new SchoolYearWiseStudentDTO
                                      {
                                          StudentName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                          (b.AMST_MiddleName.Trim() == null || b.AMST_MiddleName.Trim() == "" ? "" : " " + b.AMST_MiddleName) +
                                          (b.AMST_LastName.Trim() == null || b.AMST_LastName.Trim() == "" ? "" : " " + b.AMST_LastName)).Trim(),
                                          AMST_AdmNo = b.AMST_AdmNo,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          AMST_Id = a.AMST_Id,
                                          ASYST_Id = a.ASYST_Id,
                                          AMAY_RollNo = a.AMAY_RollNo
                                      }).OrderBy(a => a.StudentName).ToList();

                data.studentdetails = studentlisturn.ToArray();


                var checkattendance = (from a in _Context.Adm_studentAttendance
                                       from b in _Context.Adm_studentAttendanceStudents
                                       where (a.ASA_Id == b.ASA_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id_CLS_Old && a.ASMS_Id == data.ASMS_Id
                                       && b.AMST_Id == data.AMST_Id && a.ASA_Activeflag == true && a.MI_Id == data.MI_Id)
                                       select b).Distinct().ToList();
                data.Attendancecount = checkattendance.Count;


                var checkexam = _Context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id_CLS_Old && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id).Distinct().ToList();
                data.Examcount = checkexam.Count;

                List<long> ASMCLIDS = new List<long>();
                ASMCLIDS.Add(data.ASMCL_Id_CLS_Old);
                ASMCLIDS.Add(data.ASMCL_Id_CLS_New);
                var checkfee = (from a in _Context.FeeYearlyClassCategoryClassDMO
                                from b in _Context.FeeYearlyClassCategoryDMO
                                from c in _Context.FeeClassCategoryDMO
                                from d in _Context.School_M_Class
                                where (a.FYCC_Id == b.FYCC_Id && b.FMCC_Id == c.FMCC_Id && a.ASMCL_Id == d.ASMCL_Id && b.FYCC_ActiveFlag == true
                                && b.ASMAY_Id == data.ASMAY_Id && ASMCLIDS.Contains(a.ASMCL_Id))
                                select new SchoolYearWiseStudentDTO
                                {
                                    ASMCL_ClassName = d.ASMCL_ClassName,
                                    FMCC_ClassCategoryName = c.FMCC_ClassCategoryName,
                                    FeeDeleteFlag = data.ASMCL_Id_CLS_Old == a.ASMCL_Id ? 1 : 0,
                                    ASMCL_Id = a.ASMCL_Id
                                }).Distinct().ToList();

                data.Feecount = checkfee.Count;
                data.FeeCategoryDetails = checkfee.ToArray();


                var attendanceCheck = (from a in _Context.Adm_studentAttendance
                                       from b in _Context.Adm_studentAttendanceStudents
                                       where (a.ASA_Id == b.ASA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMST_Id == data.AMST_Id)
                                       select b.ASA_Id).Count();

                var examMarksCheck = _Context.ExamMarksDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.MI_Id && s.AMST_Id == data.AMST_Id && s.ASMCL_Id == data.ASMCL_Id
                                          && s.ASMS_Id == data.ASMS_Id).Count();


                if (attendanceCheck > 0)
                {
                    data.attendanceDone = true;
                    //var booksTaken=_Context.LIB_Book_Transaction_StudentDMO
                }
                else
                {
                    data.attendanceDone = false;
                }
                if (examMarksCheck > 0)
                {
                    data.examMarksDone = true;
                }
                else
                {
                    data.examMarksDone = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SchoolYearWiseStudentDTO DeleteFeeMapping(SchoolYearWiseStudentDTO data)
        {
            try
            {
                var cnt = _Context.feestudentstatus.Where(t => t.AMST_Id == data.AMST_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FSS_PaidAmount > 0).ToList();

                if (cnt.Count == 0)
                {
                    var confirmstatus = _Context.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingClassChange @p0,@p1,@p2", data.MI_Id, data.AMST_Id, data.ASMAY_Id);

                    if (confirmstatus > 0)
                    {
                        data.returnVal = true;
                    }
                    else
                    {
                        data.returnVal = false;
                    }
                    data.flag = "true";
                }
                else
                {
                    data.flag = "false";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //public SchoolYearWiseStudentDTO DeleteFeeMapping(SchoolYearWiseStudentDTO data)
        //{
        //    try
        //    {
        //        var confirmstatus = _Context.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingClassChange @p0,@p1,@p2", data.MI_Id, data.AMST_Id, data.ASMAY_Id);

        //        if (confirmstatus > 0)
        //        {
        //            data.returnVal = true;
        //        }
        //        else
        //        {
        //            data.returnVal = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        public SchoolYearWiseStudentDTO SaveClassChange(SchoolYearWiseStudentDTO data)
        {
            try
            {


                var getresult = _Context.SchoolYearWiseStudent.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
          && a.ASYST_Id == data.ASYST_Id).ToList();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (getresult.Count > 0)
                {
                    var result = _Context.SchoolYearWiseStudent.Single(a => a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.ASYST_Id == data.ASYST_Id);
                    result.ASMCL_Id = data.ASMCL_Id_CLS_New;
                    result.LoginId = data.LoginId;
                    result.UpdatedDate = indiantime0;
                    result.ASYST_CreatedBy = data.userId;
                    _Context.Update(result);

                    Adm_School_Student_COCDMO adm_School_Student_COCDMO = new Adm_School_Student_COCDMO
                    {
                        AMST_Id = data.AMST_Id,
                        ASMAY_Id = data.ASMAY_Id,
                        ASMCL_Id_Old = data.ASMCL_Id_CLS_Old,
                        ASMCL_Id_New = data.ASMCL_Id_CLS_New,
                        ASSCOC_Remarks = data.Remarks,
                        CreatedBy = data.LoginId,
                        UpdatedBy = data.LoginId,
                        CreatedDate = indiantime0,
                        UpdatedDate = indiantime0
                    };
                    _Context.Add(adm_School_Student_COCDMO);

                    var i = _Context.SaveChanges();
                    if (i > 0)
                    {
                        if (data.attendanceDone == true)
                        {
                            var confirmstatus = 0;
                            //var confirmstatus = 0;
                            confirmstatus = _Context.Database.ExecuteSqlCommand("Adm_AttendanceAutotransfer @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMCL_Id_CLS_Old, data.ASMCL_Id_CLS_New, data.ASMS_Id, data.ASMAY_Id, data.MI_Id);
                            if (confirmstatus != 0)
                            {
                                data.returnValattendance = true;
                            }
                            else
                            {
                                data.returnValattendance = false;
                            }

                        }
                        else
                        {
                            data.returnVal = false;
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

        //added By Kavita 
        public SchoolYearWiseStudentDTO SaveClassFeeChange(SchoolYearWiseStudentDTO data)
        {
            try
            {
                var confirmstatus = _Context.Database.ExecuteSqlCommand("ClassChangeFeeUpdate @p0,@p1,@p2,@p3,@p4,@p5", data.ASMAY_Id, data.MI_Id, data.ASMCL_Id_CLS_New, data.AMST_Id, data.LoginId, data.ASMCL_Id_CLS_Old);

                if (confirmstatus > 0)
                {
                    data.returnVal = true;
                }
                else
                {
                    data.returnVal = false;
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