using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
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
    public class GeneralSiblingEmployeeMappingImpl : Interfaces.GeneralSiblingEmployeeMappingInterface
    {
        public AdmissionFormContext _AdmissionFormContext;

        public GeneralSiblingEmployeeMappingImpl(AdmissionFormContext _Admission)
        {
            _AdmissionFormContext = _Admission;
        }

        public GeneralSiblingEmployeeMappingDTO getalldetails(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                data.fillacademic = _AdmissionFormContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.fillstaff = _AdmissionFormContext.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true).ToArray();

                List<long> yearid = new List<long>();

                var getpreadmission_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.allstudentdata = (from a in _AdmissionFormContext.Adm_M_Student
                                       from b in _AdmissionFormContext.SchoolYearWiseStudent
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && yearid.Contains(b.ASMAY_Id))
                                       select new GeneralSiblingEmployeeMappingDTO
                                       {
                                           AMST_FirstName = (a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName +
                                            a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName +
                                            a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName),
                                           AMST_Id = a.AMST_Id,
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GeneralSiblingEmployeeMappingDTO selectradio(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                data.getclassdetails = _AdmissionFormContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).
                    ToArray();

                if (data.radiobtnval == "stud")
                {
                    List<long> amstid = new List<long>();

                    var getmappedlist = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                    List<long> yearid = new List<long>();

                    var getpreadmission_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                    var getcut_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                    foreach (var c in getpreadmission_year)
                    {
                        yearid.Add(c.ASMAY_Id);
                    }
                    foreach (var c in getcut_year)
                    {
                        yearid.Add(c.ASMAY_Id);
                    }


                    foreach (var c in getmappedlist)
                    {
                        amstid.Add(c.AMSTS_Siblings_AMST_ID);
                    }

                    data.alldata = (from a in _AdmissionFormContext.Adm_M_Student
                                    from c in _AdmissionFormContext.SchoolYearWiseStudent
                                    from d in _AdmissionFormContext.AcademicYear
                                    from e in _AdmissionFormContext.School_M_Class
                                    where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id
                                    && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && c.AMAY_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id))

                                    select new GeneralSiblingEmployeeMappingDTO
                                    {
                                        AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                            (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)),

                                        sibilingname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                      (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                      (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)),
                                        AMST_Id = a.AMST_Id,
                                        AMCL_Id = Convert.ToInt64(c.ASMCL_Id)
                                    }).Distinct().ToArray();


                    data.getdisplaydetails = (from a in _AdmissionFormContext.StudentSiblingDMO
                                              from b in _AdmissionFormContext.Adm_M_Student
                                              from c in _AdmissionFormContext.School_M_Class
                                              from e in _AdmissionFormContext.SchoolYearWiseStudent
                                              from d in _AdmissionFormContext.AcademicYear
                                              where (b.AMST_Id == e.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && e.ASMAY_Id == d.ASMAY_Id && a.MI_Id == b.MI_Id
                                              && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id && a.AMSTS_SiblingsOrder == 1 && a.AMSTS_TCIssuesFlag == "0"
                                              && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1 && yearid.Contains(e.ASMAY_Id))
                                              select new GeneralSiblingEmployeeMappingDTO
                                              {
                                                  AMST_Id = a.AMST_Id,
                                                  AMST_AdmNo = b.AMST_AdmNo,
                                                  AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                                  (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                                  (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)),
                                                  AMSTS_SiblingsOrder = a.AMSTS_SiblingsOrder,
                                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                                  AMSTS_Id = a.AMSTS_Id
                                              }).Distinct().ToArray();


                }

                else if (data.radiobtnval == "stfoth")
                {
                    data.alldata = (from a in _AdmissionFormContext.HR_Master_Employee_DMO
                                    from b in _AdmissionFormContext.HR_Master_Department
                                    from c in _AdmissionFormContext.HR_Master_Designation
                                    where (a.HRMD_Id == b.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                    && a.HRME_LeftFlag == false)
                                    select new GeneralSiblingEmployeeMappingDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                        (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                        (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                        (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)),
                                        HRMD_DepartmentName = b.HRMD_DepartmentName,
                                        HRMDES_DesignationName = c.HRMDES_DesignationName,
                                    }).ToArray();



                    data.getdisplaydetails = (from a in _AdmissionFormContext.HR_Master_Employee_DMO
                                              from b in _AdmissionFormContext.HR_Master_Department
                                              from c in _AdmissionFormContext.HR_Master_Designation
                                              from d in _AdmissionFormContext.Adm_M_Employee_StudentDMO
                                              where (d.HRME_Id == a.HRME_Id && a.HRMD_Id == b.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == data.MI_Id
                                              && a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true && d.AMSTE_Left == 0)
                                              select new GeneralSiblingEmployeeMappingDTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? ""
                                                  : a.HRME_EmployeeFirstName) +
                                                  (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                                  (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                                  (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)),
                                                  HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                  HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              }).Distinct().ToArray();

                }

                else if (data.radiobtnval == "R")
                {
                    data.alldata = (from a in _AdmissionFormContext.Adm_M_Student
                                    from b in _AdmissionFormContext.Fee_Master_ConcessionDMO
                                    where (a.AMST_Concession_Type == b.FMCC_Id && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1
                                    && b.FMCC_ConcessionFlag == "R" && b.FMCC_ActiveFlag == true)
                                    select new GeneralSiblingEmployeeMappingDTO
                                    {
                                        AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName)
                                        + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName)
                                        + (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)
                                        + (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)),
                                        AMST_Id = a.AMST_Id,
                                    }).Distinct().ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GeneralSiblingEmployeeMappingDTO onstudentnamechange(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getpreadmission_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                List<long> amstid = new List<long>();

                var getmappedlist = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.MI_Id == data.MI_Id).ToList();


                foreach (var c in getmappedlist)
                {
                    amstid.Add(c.AMSTS_Siblings_AMST_ID);
                }

                var checkstudent = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.AMSTS_Siblings_AMST_ID == data.AMST_Id && a.AMSTS_TCIssuesFlag == "0").ToList();

                if (checkstudent.Count() > 0)
                {
                    var checkselectedstudent = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.AMSTS_Siblings_AMST_ID == data.AMST_Id
                    && a.AMSTS_SiblingsOrder == 1 && a.AMSTS_TCIssuesFlag == "0").ToList();
                    if (checkselectedstudent.Count() == 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var amstidd = checkselectedstudent.FirstOrDefault().AMST_Id;

                        var getamst = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.AMST_Id == amstidd && a.AMSTS_TCIssuesFlag == "0").ToList();

                        List<long> stdid = new List<long>();
                        foreach (var x in getamst)
                        {
                            stdid.Add(x.AMSTS_Siblings_AMST_ID);
                        }


                        data.getsudentlist = (from a in _AdmissionFormContext.Adm_M_Student
                                              from c in _AdmissionFormContext.SchoolYearWiseStudent
                                              from d in _AdmissionFormContext.AcademicYear
                                              from e in _AdmissionFormContext.School_M_Class
                                              where (a.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id
                                              && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_Id != data.AMST_Id
                                              && c.AMAY_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id) && !amstid.Contains(a.AMST_Id))
                                              select new GeneralSiblingEmployeeMappingDTO
                                              {
                                                  AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                      (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                      (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                                      (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)),
                                                  sibilingname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                      (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                      (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)),
                                                  AMST_Id = a.AMST_Id,
                                                  AMCL_Id = Convert.ToInt64(c.ASMCL_Id)
                                              }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


                        data.getstudentlistsaved = (from a in _AdmissionFormContext.StudentSiblingDMO
                                                    from b in _AdmissionFormContext.Adm_M_Student
                                                    from c in _AdmissionFormContext.SchoolYearWiseStudent
                                                    from d in _AdmissionFormContext.AcademicYear
                                                    from e in _AdmissionFormContext.School_M_Class
                                                    where (b.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id
                                                    && a.AMSTS_Siblings_AMST_ID == b.AMST_Id && a.AMSTS_TCIssuesFlag == "0" && a.AMST_Id == data.AMST_Id
                                                    && b.MI_Id == data.MI_Id && yearid.Contains(c.ASMAY_Id) && c.AMAY_ActiveFlag == 1)
                                                    select new GeneralSiblingEmployeeMappingDTO
                                                    {
                                                        AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                                        (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                                        (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName) +
                                                        (b.AMST_AdmNo == null || b.AMST_AdmNo == "" ? "" : " : " + b.AMST_AdmNo)),
                                                        AMST_Id = b.AMST_Id,
                                                        AMSTS_SiblingsOrder = a.AMSTS_SiblingsOrder,
                                                        AMSTG_SiblingsRelation = a.AMSTS_SiblingsRelation,
                                                        sibilingname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                                      (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                                      (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName)),
                                                        AMCL_Id = Convert.ToInt64(c.ASMCL_Id),
                                                        AMSTS_Id = a.AMSTS_Id
                                                    }).Distinct().OrderBy(a => a.AMSTS_SiblingsOrder).ToArray();
                    }
                }

                else
                {
                    data.getsudentlist = (from a in _AdmissionFormContext.Adm_M_Student
                                          from c in _AdmissionFormContext.SchoolYearWiseStudent
                                          from d in _AdmissionFormContext.AcademicYear
                                          from e in _AdmissionFormContext.School_M_Class
                                          where (a.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id
                                          && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_Id != data.AMST_Id
                                          && c.AMAY_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id) && !amstid.Contains(a.AMST_Id))
                                          select new GeneralSiblingEmployeeMappingDTO
                                          {
                                              AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                  (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                  (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                                  (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)),
                                              sibilingname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                      (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                      (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)),
                                              AMCL_Id = Convert.ToInt64(c.ASMCL_Id),
                                              AMST_Id = a.AMST_Id,
                                          }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public GeneralSiblingEmployeeMappingDTO onselectstaff(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                List<long> amstid = new List<long>();

                List<long> yearid = new List<long>();

                var getpreadmission_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.getstudentlistsaved = (from b in _AdmissionFormContext.Adm_M_Employee_StudentDMO
                                            from a in _AdmissionFormContext.Adm_M_Student
                                            from c in _AdmissionFormContext.SchoolYearWiseStudent
                                            from d in _AdmissionFormContext.AcademicYear
                                            from e in _AdmissionFormContext.School_M_Class
                                            where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && c.ASMAY_Id == d.ASMAY_Id && c.AMAY_ActiveFlag == 1
                                            && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_Id == b.AMST_Id && b.AMSTE_Left == 0
                                            && b.HRME_Id == data.HRME_Id && yearid.Contains(c.ASMAY_Id))
                                            select new GeneralSiblingEmployeeMappingDTO
                                            {
                                                AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                  (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                  (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                                  (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)),
                                                AMCL_Id = Convert.ToInt64(c.ASMCL_Id),
                                                AMST_Id = a.AMST_Id,
                                                AMSTE_Id = b.AMSTE_Id,
                                                AMSTE_SiblingsOrder = b.AMSTE_SiblingsOrder
                                            }).Distinct().OrderBy(a => a.AMSTE_SiblingsOrder).ToArray();


                var getsavestudent = (from a in _AdmissionFormContext.Adm_M_Employee_StudentDMO
                                      from b in _AdmissionFormContext.Adm_M_Student
                                      where (a.AMST_Id == b.AMST_Id && b.AMST_SOL == "S" && b.MI_Id == data.MI_Id)
                                      select new GeneralSiblingEmployeeMappingDTO
                                      {
                                          AMST_Id = b.AMST_Id

                                      }).Distinct().ToList();

                foreach (var c in getsavestudent)
                {
                    amstid.Add(c.AMST_Id);
                }

                data.getsudentlist = (from a in _AdmissionFormContext.Adm_M_Student
                                      from c in _AdmissionFormContext.SchoolYearWiseStudent
                                      from d in _AdmissionFormContext.AcademicYear
                                      from e in _AdmissionFormContext.School_M_Class
                                      where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && c.ASMAY_Id == d.ASMAY_Id && c.AMAY_ActiveFlag == 1
                                      && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id)
                                      && !amstid.Contains(a.AMST_Id))
                                      select new GeneralSiblingEmployeeMappingDTO
                                      {
                                          AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                              (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                              (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                              (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)),
                                          AMST_Id = a.AMST_Id,
                                          AMCL_Id = Convert.ToInt64(c.ASMCL_Id)
                                      }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }         
        public GeneralSiblingEmployeeMappingDTO savedata(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                int count = 0;

                if (data.radiobtnval == "stud")
                {

                    for (int i = 0; i < data.savesiblingsDTO.Count(); i++)
                    {

                        if (data.savesiblingsDTO[i].AMSTS_Id > 0)
                        {
                            var checkdetails = _AdmissionFormContext.StudentSiblingDMO.Single(a => a.AMSTS_Id == data.savesiblingsDTO[i].AMSTS_Id);
                            checkdetails.AMSTS_SiblingsRelation = data.savesiblingsDTO[i].AMSTG_SiblingsRelation;
                            checkdetails.AMCL_Id = data.savesiblingsDTO[i].AMCL_Id;
                            checkdetails.UpdatedDate = DateTime.Now;
                            _AdmissionFormContext.Update(checkdetails);
                        }
                        else
                        {
                            count += 1;
                            StudentSiblingDMO sib = new StudentSiblingDMO();
                            sib.AMST_Id = data.AMST_Id;
                            sib.MI_Id = data.MI_Id;
                            sib.AMSTS_Siblings_AMST_ID = data.savesiblingsDTO[i].AMSTS_SiblingsAMST_Id;
                            sib.AMSTS_SiblingsName = data.savesiblingsDTO[i].AMSTS_SiblingsName;
                            sib.AMSTS_SiblingsRelation = data.savesiblingsDTO[i].AMSTG_SiblingsRelation;
                            sib.AMSTS_SiblingsOrder = data.savesiblingsDTO[i].AMSTS_SiblingsOrder;
                            sib.AMCL_Id = data.savesiblingsDTO[i].AMCL_Id;
                            sib.AMSTS_TCIssuesFlag = "0";
                            sib.CreatedDate = DateTime.Now;
                            sib.UpdatedDate = DateTime.Now;
                            _AdmissionFormContext.Add(sib);
                        }
                    }

                    var i2 = _AdmissionFormContext.SaveChanges();
                    if (i2 > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }

                else if (data.radiobtnval == "stfoth")
                {
                    List<long> empid = new List<long>();
                    foreach (var c in data.savestudentemployeeDTO)
                    {
                        empid.Add(c.AMSTE_Id);
                    }

                    Array resultremove = _AdmissionFormContext.Adm_M_Employee_StudentDMO.Where(a => a.HRME_Id == data.HRME_Id && !empid.Contains(a.AMSTE_Id)).ToArray();

                    foreach (Adm_M_Employee_StudentDMO d in resultremove)
                    {
                        _AdmissionFormContext.Remove(d);
                    }

                    for (int i = 0; i < data.savestudentemployeeDTO.Length; i++)
                    {
                        if (data.savestudentemployeeDTO[i].AMSTE_Id > 0)
                        {
                            var checkresult = _AdmissionFormContext.Adm_M_Employee_StudentDMO.Single(a => a.AMSTE_Id == data.savestudentemployeeDTO[i].AMSTE_Id);
                            checkresult.AMST_Id = data.savestudentemployeeDTO[i].AMST_Id;
                            checkresult.ASMCL_Id = data.savestudentemployeeDTO[i].ASMCL_Id;
                            checkresult.AMSTE_SiblingsOrder = data.savestudentemployeeDTO[i].AMSTE_SiblingsOrder;
                            checkresult.UpdatedDate = DateTime.Now;
                            _AdmissionFormContext.Update(checkresult);
                        }
                        else
                        {
                            count += 1;
                            Adm_M_Employee_StudentDMO sib = new Adm_M_Employee_StudentDMO();
                            sib.AMST_Id = data.savestudentemployeeDTO[i].AMST_Id;
                            sib.ASMCL_Id = data.savestudentemployeeDTO[i].ASMCL_Id;
                            sib.AMSTE_SiblingsOrder = data.savestudentemployeeDTO[i].AMSTE_SiblingsOrder;
                            sib.HRME_Id = data.HRME_Id;
                            sib.AMSTE_Concessionpercentage = 0;
                            sib.AMSTE_Left = 0;
                            sib.CreatedDate = DateTime.Now;
                            sib.UpdatedDate = DateTime.Now;
                            _AdmissionFormContext.Add(sib);
                        }
                    }

                    var i1 = _AdmissionFormContext.SaveChanges();
                    if (i1 > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "false";
            }
            return data;
        }
        public GeneralSiblingEmployeeMappingDTO deleterec(GeneralSiblingEmployeeMappingDTO data)
        {
            var getamstid = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToList();

            List<long> amstid = new List<long>();

            foreach (var x in getamstid)
            {
                amstid.Add(x.AMSTS_Siblings_AMST_ID);
            }

            Array resultremove = _AdmissionFormContext.StudentSiblingDMO.Where(a => a.MI_Id == data.MI_Id && amstid.Contains(a.AMSTS_Siblings_AMST_ID)).ToArray();

            foreach (StudentSiblingDMO d in resultremove)
            {
                _AdmissionFormContext.Remove(d);
            }

            var i = _AdmissionFormContext.SaveChanges();

            if (i > 0)
            {
                data.returnval = "true";
            }
            else
            {
                data.returnval = "false";
            }

            return data;
        }
        public GeneralSiblingEmployeeMappingDTO DeletRecordemployee(GeneralSiblingEmployeeMappingDTO data)
        {
            var getamstid = _AdmissionFormContext.Adm_M_Employee_StudentDMO.Where(a => a.HRME_Id == data.HRME_Id).ToList();

            List<long> amstid = new List<long>();

            foreach (var x in getamstid)
            {
                amstid.Add(x.AMST_Id);
            }



            Array resultremove = _AdmissionFormContext.Adm_M_Employee_StudentDMO.Where(a => a.HRME_Id == data.HRME_Id).ToArray();

            foreach (Adm_M_Employee_StudentDMO d in resultremove)
            {
                _AdmissionFormContext.Remove(d);
            }
            var outputval = _AdmissionFormContext.SaveChanges();

            if (outputval >= 1)
            {
                data.returnval = "true";
            }
            else
            {
                data.returnval = "false";
            }              
            
            return data;
        }
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetails(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getpreadmission_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.getviewdetails = (from b in _AdmissionFormContext.StudentSiblingDMO
                                       from a in _AdmissionFormContext.Adm_M_Student
                                       from c in _AdmissionFormContext.School_M_Class
                                       from d in _AdmissionFormContext.SchoolYearWiseStudent
                                       from e in _AdmissionFormContext.AcademicYear
                                       where (a.AMST_Id == d.AMST_Id && d.ASMAY_Id == e.ASMAY_Id && a.AMST_Id == b.AMSTS_Siblings_AMST_ID && d.ASMCL_Id == c.ASMCL_Id
                                       && b.AMST_Id == data.AMST_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                       && b.AMSTS_TCIssuesFlag == "0" && yearid.Contains(d.ASMAY_Id))
                                       select new GeneralSiblingEmployeeMappingDTO
                                       {
                                           AMSTS_SiblingsAMST_Id = b.AMSTS_Siblings_AMST_ID,
                                           AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)),
                                           AMSTS_SiblingsOrder = b.AMSTS_SiblingsOrder,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMSTG_SiblingsRelation = b.AMSTS_SiblingsRelation,
                                           ASMCL_ClassName = c.ASMCL_ClassName,

                                       }).Distinct().OrderBy(a => a.AMSTS_SiblingsOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetailsemployee(GeneralSiblingEmployeeMappingDTO data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getpreadmission_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _AdmissionFormContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.getviewdetails = (from b in _AdmissionFormContext.Adm_M_Employee_StudentDMO
                                       from a in _AdmissionFormContext.Adm_M_Student
                                       from c in _AdmissionFormContext.School_M_Class
                                       from d in _AdmissionFormContext.SchoolYearWiseStudent
                                       from e in _AdmissionFormContext.AcademicYear
                                       where (a.AMST_Id == d.AMST_Id && d.ASMAY_Id == e.ASMAY_Id && a.AMST_Id == b.AMST_Id && d.ASMCL_Id == c.ASMCL_Id
                                       && b.HRME_Id == data.HRME_Id && b.AMSTE_Left == 0 && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                       select new GeneralSiblingEmployeeMappingDTO
                                       {
                                           AMST_Id = b.AMST_Id,
                                           AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)),
                                           AMSTE_SiblingsOrder = b.AMSTE_SiblingsOrder,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                       }).Distinct().OrderBy(a => a.AMSTE_SiblingsOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
