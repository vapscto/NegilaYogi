using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeStudyCertificateReportImpl : Interface.CollegeStudyCertificateReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;

        public CollegeStudyCertificateReportImpl(ClgAdmissionContext _clgadmc)
        {
            _clgadmctxt = _clgadmc;
        }

        public CollegeStudyCertificateReportDTO getdata(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                data.getyear = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CollegeStudyCertificateReportDTO onchangeyear(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                data.getcourse = (from a in _clgadmctxt.MasterCourseDMO
                                  from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id
                                  && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CollegeStudyCertificateReportDTO onchangecourse(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                  from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                  && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                  && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided

                                  }).Distinct().ToList();
                data.getbranch = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CollegeStudyCertificateReportDTO onchangebranch(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && c.ACAYC_Id == b.ACAYC_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id
                                    && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id
                                    && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id
                                    && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)

                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();

                data.getsemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CollegeStudyCertificateReportDTO onchangesemester(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                data.getsection = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CollegeStudyCertificateReportDTO searchfilter(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                var flag = "";
                bool flagactive = false;
                int amatactiveflag = 0;

                if (data.AMCST_SOL == "S")
                {
                    flag = "S";
                    flagactive = true;
                    amatactiveflag = 1;

                }
                else if (data.AMCST_SOL == "L")
                {
                    flag = "L";
                    flagactive = false;
                    amatactiveflag = 0;
                }
                else if (data.AMCST_SOL == "D")
                {
                    flag = "D";
                    flagactive = true;
                    amatactiveflag = 1;
                }

                //if (data.allorindid == "A")
                //{
                //    data.getstudentlist = (from a in _clgadmctxt.Adm_College_Yearly_StudentDMO
                //                           from b in _clgadmctxt.Adm_Master_College_StudentDMO
                //                           where (a.AMCST_Id == b.AMCST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                //                           && b.AMCST_SOL == flag && b.AMCST_ActiveFlag == flagactive && a.ACYST_ActiveFlag == amatactiveflag
                //                           && ((b.AMCST_FirstName.Trim().ToUpper() + ' ' + b.AMCST_MiddleName.Trim().ToUpper() + ' ' + b.AMCST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMCST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMCST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMCST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                //                           select new CollegeStudyCertificateReportDTO
                //                           {
                //                               AMCST_Id = a.AMCST_Id,
                //                               studentname = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "0" ? "" : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "0" ? "" : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null || b.AMCST_LastName == "0" ? "" : b.AMCST_LastName)).Trim(),

                //                           }).Distinct().OrderBy(a => a.studentname).ToArray();
                //}
                //else
                //{
                //data.getstudentlist = (from a in _clgadmctxt.Adm_College_Yearly_StudentDMO
                //                       from b in _clgadmctxt.Adm_Master_College_StudentDMO
                //                           where (a.AMCST_Id == b.AMCST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                //                           && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id
                //                           && b.AMCST_SOL == flag && b.AMCST_ActiveFlag == flagactive && a.ACYST_ActiveFlag == amatactiveflag
                //                           && ((b.AMCST_FirstName.Trim().ToUpper() + ' ' + b.AMCST_MiddleName.Trim().ToUpper() + ' ' + b.AMCST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMCST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMCST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMCST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                //                           select new CollegeStudyCertificateReportDTO
                //                           {
                //                               AMCST_Id = a.AMCST_Id,
                //                               studentname = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "0" ? "" : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "0" ? "" : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null || b.AMCST_LastName == "0" ? "" : b.AMCST_LastName)).Trim(),

                                           //                           }).Distinct().OrderBy(a => a.studentname).ToArray();
                                           //}



                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentCollege_TC_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NAME",
                    SqlDbType.VarChar)
                    {
                        //Value = Class_Id
                        Value = data.searchfilter
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_SOL",
               SqlDbType.VarChar)
                    {
                        Value = data.AMCST_SOL
                    });

                //    cmd.Parameters.Add(new SqlParameter("@Flag",
                //SqlDbType.VarChar)
                //    {
                //        Value = flag
                //    });



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
                        data.getstudentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




                if (data.getstudentlist.Length > 0)
                {
                    data.count = data.getstudentlist.Length;
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
        public CollegeStudyCertificateReportDTO onclickreport(CollegeStudyCertificateReportDTO data)
        {
            try
            {
                var logo = _clgadmctxt.Institution.Where(s => s.MI_Id == data.MI_Id && s.MI_ActiveFlag == 1).Select(m => m.MI_Logo).FirstOrDefault();


                if(logo!="")
                {
                    data.logo = logo;

                }
                var flag = "";
                bool flagactive = false;
                int amatactiveflag = 0;

                if (data.AMCST_SOL == "S")
                {
                    flag = "S";
                    flagactive = true;
                    amatactiveflag = 1;

                }
                else if (data.AMCST_SOL == "L")
                {
                    flag = "L";
                    flagactive = false;
                    amatactiveflag = 0;
                }
                else if (data.AMCST_SOL == "D")
                {
                    flag = "D";
                    flagactive = true;
                    amatactiveflag = 1;
                }

                if (data.allorindid == "A")
                {
                    data.getstudentdetailslist = (from a in _clgadmctxt.Adm_College_Yearly_StudentDMO
                                                  from b in _clgadmctxt.Adm_Master_College_StudentDMO
                                                  from c in _clgadmctxt.MasterCourseDMO
                                                  from d in _clgadmctxt.ClgMasterBranchDMO
                                                  from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                                  from f in _clgadmctxt.AcademicYear
                                                  from g in _clgadmctxt.Adm_College_Master_SectionDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id
                                                  && a.ASMAY_Id == f.ASMAY_Id && g.ACMS_Id == a.ACMS_Id && a.ACYST_ActiveFlag == amatactiveflag
                                                  && b.AMCST_SOL == flag && b.AMCST_ActiveFlag == flagactive
                                                  && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                                                  select new CollegeStudyCertificateReportDTO
                                                  {
                                                      studentname = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "0" ? "" : b.AMCST_FirstName) + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "0" || b.AMCST_MiddleName == "" ? "" : " " + b.AMCST_MiddleName) + (b.AMCST_LastName == null || b.AMCST_LastName == "0" || b.AMCST_LastName == "" ? "" : " " + b.AMCST_LastName)).Trim(),
                                                      coursename = c.AMCO_CourseName,
                                                      branchname = d.AMB_BranchName,
                                                      fathername = ((b.AMCST_FatherName == null || b.AMCST_FatherName == "0" ? "" : b.AMCST_FatherName) 
                                                      + (b.AMCST_FatherSurname == null || b.AMCST_FatherSurname == "0" || b.AMCST_FatherSurname == "" ? "" :
                                                      " " + b.AMCST_FatherSurname)).Trim(),
                                                      religion = b.IVRMMR_Id != null && b.IVRMMR_Id > 0 ? _clgadmctxt.Religion.Where(a => a.IVRMMR_Id == b.IVRMMR_Id).Distinct().FirstOrDefault().IVRMMR_Name : "",
                                                      castecat = b.IMCC_Id != null && b.IMCC_Id > 0 ? _clgadmctxt.CasteCategory.Where(a => a.IMCC_Id == b.IMCC_Id).Distinct().FirstOrDefault().IMCC_CategoryName : "",
                                                      dob = b.AMCST_DOB,
                                                      semestername = e.AMSE_SEMName,
                                                      sectionname = g.ACMS_SectionName,
                                                      acadamicyear = f.ASMAY_Year,
                                                      yearname = f.ASMAY_Year,
                                                      registrationno = b.AMCST_RegistrationNo,
                                                      admno = b.AMCST_AdmNo,
                                                      gender=b.AMCST_Sex
                                                  }).Distinct().ToArray();
                }
                else
                {
                    data.getstudentdetailslist = (from a in _clgadmctxt.Adm_College_Yearly_StudentDMO
                                                  from b in _clgadmctxt.Adm_Master_College_StudentDMO
                                                  from c in _clgadmctxt.MasterCourseDMO
                                                  from d in _clgadmctxt.ClgMasterBranchDMO
                                                  from e in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                                  from f in _clgadmctxt.AcademicYear
                                                  from g in _clgadmctxt.Adm_College_Master_SectionDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id
                                                  && a.ASMAY_Id == f.ASMAY_Id && g.ACMS_Id == a.ACMS_Id && a.ACYST_ActiveFlag == amatactiveflag
                                                  && b.AMCST_SOL == flag && b.AMCST_ActiveFlag == flagactive
                                                  && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                                  && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id)
                                                  select new CollegeStudyCertificateReportDTO
                                                  {
                                                      studentname = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "0" ? "" : b.AMCST_FirstName) + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "0" || b.AMCST_MiddleName == "" ? "" : " " + b.AMCST_MiddleName) + (b.AMCST_LastName == null || b.AMCST_LastName == "0" || b.AMCST_LastName == "" ? "" : " " + b.AMCST_LastName)).Trim(),
                                                      coursename = c.AMCO_CourseName,
                                                      fathername = ((b.AMCST_FatherName == null || b.AMCST_FatherName == "0" ? "" : b.AMCST_FatherName)
                                                      + (b.AMCST_FatherSurname == null || b.AMCST_FatherSurname == "0" || b.AMCST_FatherSurname == "" ? "" :
                                                      " " + b.AMCST_FatherSurname)).Trim(),
                                                      dob = b.AMCST_DOB,
                                                      branchname = d.AMB_BranchName,
                                                      classstudying = c.AMCO_CourseName,
                                                      religion = b.IVRMMR_Id != null && b.IVRMMR_Id > 0 ? _clgadmctxt.Religion.Where(a => a.IVRMMR_Id == b.IVRMMR_Id).Distinct().FirstOrDefault().IVRMMR_Name : "",
                                                      castecat = b.IMCC_Id != null && b.IMCC_Id > 0 ? _clgadmctxt.CasteCategory.Where(a => a.IMCC_Id == b.IMCC_Id).Distinct().FirstOrDefault().IMCC_CategoryName : "",
                                                      acadamicyear = f.ASMAY_Year,
                                                      semestername = e.AMSE_SEMName,
                                                      sectionname = g.ACMS_SectionName,
                                                      yearname = f.ASMAY_Year,
                                                      registrationno = b.AMCST_RegistrationNo,
                                                      admno = b.AMCST_AdmNo,
                                                      gender = b.AMCST_Sex
                                                  }).Distinct().ToArray();
                }


                data.academicList1 = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
