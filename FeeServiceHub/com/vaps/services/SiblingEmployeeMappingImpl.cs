using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vaps.admission;
using System.Dynamic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.services
{
    [Route("api/[controller]")]
    public class SiblingEmployeeMappingImpl : interfaces.SiblingEmployeeMappingInterface
    {
        private static ConcurrentDictionary<string, Adm_M_Sibling> _login =
          new ConcurrentDictionary<string, Adm_M_Sibling>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<SiblingEmployeeMappingImpl> _logger;
        public SiblingEmployeeMappingImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<SiblingEmployeeMappingImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public Adm_M_Sibling initialdata(Adm_M_Sibling data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.fillacademic = allyear.Distinct().ToArray();

                List<MasterEmployee> staffdata = new List<MasterEmployee>();
                staffdata = _YearlyFeeGroupMappingContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_ActiveFlag == true).ToList();
                data.fillstaff = staffdata.Distinct().ToArray();

                List<long> yearid = new List<long>();

                var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.allstudentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id
                                       //&& b.ASMAY_Id == data.ASMAY_ID 
                                       && a.MI_Id == data.MI_Id && yearid.Contains(b.ASMAY_Id))
                                       select new Adm_M_Sibling
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

        // ON CHANGE OF RADIO BUTTON STUDENT OR STAFF
        public Adm_M_Sibling selectacade(Adm_M_Sibling data)
        {
            try
            {
                data.getclassdetails = _YearlyFeeGroupMappingContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).
                    ToArray();

                if (data.radiobtnval == "stud")
                {
                    List<long> amstid = new List<long>();

                    var getmappedlist = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.MI_Id == data.MI_Id).ToList();

                    List<long> yearid = new List<long>();

                    var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                    var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

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

                    data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                    from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from d in _YearlyFeeGroupMappingContext.AcademicYear
                                    from e in _YearlyFeeGroupMappingContext.School_M_Class
                                    where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.AMST_Concession_Type == b.FMCC_Id
                                    && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && c.AMAY_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id)
                                    //&& !amstid.Contains(a.AMST_Id)  && c.ASMAY_Id == data.ASMAY_ID
                                    && b.FMCC_ConcessionFlag == "S" && b.FMCC_ActiveFlag == true)
                                    select new Adm_M_Sibling
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


                    var getpercentage = _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMCC_ActiveFlag == true && a.FMCC_ConcessionFlag == "S").ToList();
                    if (getpercentage.Count() == 0)
                    {
                        data.returnval = "No Data";
                    }

                    data.getdisplaydetails = (from a in _YearlyFeeGroupMappingContext.Adm_M_Sibling
                                              from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                              from c in _YearlyFeeGroupMappingContext.School_M_Class
                                              from e in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                              from d in _YearlyFeeGroupMappingContext.AcademicYear
                                              where (b.AMST_Id == e.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && e.ASMAY_Id == d.ASMAY_Id && a.MI_Id == b.MI_Id
                                              && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id
                                              && b.MI_Id == data.MI_Id && a.AMSTS_SiblingsOrder == 1 && a.AMSTS_TCIssuesFlag == "0"
                                              && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1 && yearid.Contains(e.ASMAY_Id)
                                              //&& e.ASMAY_Id == data.ASMAY_ID
                                              )
                                              select new Adm_M_Sibling
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
                    var getpercentage = _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMCC_ActiveFlag == true && a.FMCC_ConcessionFlag == "E").ToList();
                    if (getpercentage.Count() == 0)
                    {
                        data.returnval = "No Data";
                    }

                    data.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                    from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                    from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                    where (a.HRMD_Id == b.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                    select new Adm_M_Sibling
                                    {
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                        (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                        (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                        (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)),
                                        HRMD_DepartmentName = b.HRMD_DepartmentName,
                                        HRMDES_DesignationName = c.HRMDES_DesignationName,
                                    }).ToArray();



                    data.getdisplaydetails = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                              from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                              from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                              from d in _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO
                                              where (d.HRME_Id == a.HRME_Id && a.HRMD_Id == b.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == data.MI_Id
                                              && a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true && d.AMSTE_Left == 0)
                                              select new Adm_M_Sibling
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                                  (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                                  (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                                  (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)),
                                                  HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                  HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              }).Distinct().ToArray();

                }

                else if (data.radiobtnval == "R")
                {
                    var getpercentage = _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMCC_ActiveFlag == true && a.FMCC_ConcessionFlag == "R").ToList();
                    if (getpercentage.Count() == 0)
                    {
                        data.returnval = "No Data";
                    }


                    data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                    where (a.AMST_Concession_Type == b.FMCC_Id && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1
                                    && b.FMCC_ConcessionFlag == "R" && b.FMCC_ActiveFlag == true)
                                    select new Adm_M_Sibling
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
        public Adm_M_Sibling onstudentnamechange(Adm_M_Sibling data)
        {
            try
            {
                //var checkfeegroupmapping = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Siblings_Employee_Student_Fee_Group_Checking @p0,@p1",
                //                                    data.AMST_Id, data.ASMAY_ID);

                //if (checkfeegroupmapping ==0)
                //{
                //    data.countmapping = 0;
                //}

                List<long> yearid = new List<long>();

                var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Siblings_Employee_Student_Fee_Group_Checking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_ID
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
                        data.countmapping = retObject.Count();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                List<long> amstid = new List<long>();

                var getmappedlist = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.MI_Id == data.MI_Id).ToList();


                foreach (var c in getmappedlist)
                {
                    amstid.Add(c.AMSTS_Siblings_AMST_ID);
                }

                var checkstudent = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.AMSTS_Siblings_AMST_ID == data.AMST_Id && a.AMSTS_TCIssuesFlag == "0").ToList();

                if (checkstudent.Count() > 0)
                {
                    var checkselectedstudent = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.AMSTS_Siblings_AMST_ID == data.AMST_Id
                    && a.AMSTS_SiblingsOrder == 1 && a.AMSTS_TCIssuesFlag == "0").ToList();
                    if (checkselectedstudent.Count() == 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var amstidd = checkselectedstudent.FirstOrDefault().AMST_Id;

                        var getamst = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.AMST_Id == amstidd && a.AMSTS_TCIssuesFlag == "0").ToList();

                        List<long> stdid = new List<long>();
                        foreach (var x in getamst)
                        {
                            stdid.Add(x.AMSTS_Siblings_AMST_ID);
                        }



                        data.getsudentlist = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                              from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                              from d in _YearlyFeeGroupMappingContext.AcademicYear
                                              from e in _YearlyFeeGroupMappingContext.School_M_Class
                                              where (a.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id
                                              && a.AMST_Concession_Type == b.FMCC_Id && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1
                                              && a.AMST_Id != data.AMST_Id && c.AMAY_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id)
                                              //c.ASMAY_Id == data.ASMAY_ID
                                              && !amstid.Contains(a.AMST_Id)
                                              && b.FMCC_ConcessionFlag == "S" && b.FMCC_ActiveFlag == true)
                                              select new Adm_M_Sibling
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


                        data.getstudentlistsaved = (from a in _YearlyFeeGroupMappingContext.Adm_M_Sibling
                                                    from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                    from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                    from d in _YearlyFeeGroupMappingContext.AcademicYear
                                                    from e in _YearlyFeeGroupMappingContext.School_M_Class
                                                    where (b.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id
                                                    && a.AMSTS_Siblings_AMST_ID == b.AMST_Id && a.AMSTS_TCIssuesFlag == "0" && a.AMST_Id == data.AMST_Id
                                                    && b.MI_Id == data.MI_Id && yearid.Contains(c.ASMAY_Id)
                                                    //&& c.ASMAY_Id == data.ASMAY_ID 
                                                    && c.AMAY_ActiveFlag == 1)
                                                    select new Adm_M_Sibling
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
                    data.getsudentlist = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                          from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          from d in _YearlyFeeGroupMappingContext.AcademicYear
                                          from e in _YearlyFeeGroupMappingContext.School_M_Class
                                          where (a.AMST_Id == c.AMST_Id && c.ASMAY_Id == d.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id &&
                                          a.AMST_Concession_Type == b.FMCC_Id && b.FMCC_ConcessionFlag == "S" && b.FMCC_ActiveFlag == true
                                          && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_Id != data.AMST_Id
                                          && c.AMAY_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id)
                                          //c.ASMAY_Id == data.ASMAY_ID
                                          && !amstid.Contains(a.AMST_Id)
                                          )
                                          select new Adm_M_Sibling
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
        public Adm_M_Sibling onselectstaff(Adm_M_Sibling data)
        {
            try
            {
                List<long> amstid = new List<long>();

                List<long> yearid = new List<long>();

                var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.getstudentlistsaved = (from b in _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO
                                            from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                            from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.AcademicYear
                                            from e in _YearlyFeeGroupMappingContext.School_M_Class
                                            where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && c.ASMAY_Id == d.ASMAY_Id && c.AMAY_ActiveFlag == 1
                                            && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && a.AMST_Id == b.AMST_Id && b.AMSTE_Left == 0 && b.HRME_Id == data.HRME_Id
                                            //&& c.ASMAY_Id == data.ASMAY_ID 
                                            && yearid.Contains(c.ASMAY_Id))
                                            select new Adm_M_Sibling
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


                var getsavestudent = (from a in _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO
                                      from b in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      where (a.AMST_Id == b.AMST_Id && b.AMST_SOL == "S" && b.MI_Id == data.MI_Id)
                                      select new Adm_M_Sibling
                                      {
                                          AMST_Id = b.AMST_Id

                                      }).Distinct().ToList();

                foreach (var c in getsavestudent)
                {
                    amstid.Add(c.AMST_Id);
                }

                data.getsudentlist = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                      from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                      from d in _YearlyFeeGroupMappingContext.AcademicYear
                                      from e in _YearlyFeeGroupMappingContext.School_M_Class
                                      where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == e.ASMCL_Id && c.ASMAY_Id == d.ASMAY_Id && c.AMAY_ActiveFlag == 1
                                      && a.AMST_Concession_Type == b.FMCC_Id && b.FMCC_ConcessionFlag == "E" && b.FMCC_ActiveFlag == true &&
                                      a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && yearid.Contains(c.ASMAY_Id)
                                      //&& c.ASMAY_Id == data.ASMAY_ID

                                      && !amstid.Contains(a.AMST_Id)
                                      )
                                      select new Adm_M_Sibling
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
        public Adm_M_Sibling onstudentnamechangerte(Adm_M_Sibling data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                var checkrte = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(a => a.AMST_Id == data.AMST_Id
                //&& a.ASMAY_Id == data.ASMAY_ID
                && yearid.Contains(a.ASMAY_Id)
                 && a.MI_Id == data.MI_Id && a.FSS_ConcessionAmount > 0).ToList();
                if (checkrte.Count() > 0)
                {
                    data.returnval = "Duplicate";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Adm_M_Sibling sved(Adm_M_Sibling data)
        {
            try
            {
                int count = 0;

                if (data.radiobtnval == "stud")
                {
                    var getpercentage = _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMCC_ActiveFlag == true
                    && a.FMCC_ConcessionFlag == "S").ToList();
                    decimal percentage = 0;

                    for (int i = 0; i < data.savesiblingsDTO.Count(); i++)
                    {
                        if (i == 0)
                        {
                            percentage = 0;
                        }
                        else
                        {
                            percentage = percentage + Convert.ToDecimal(getpercentage.FirstOrDefault().FMCC_ConcessionApplLimit);
                        }

                        if (data.savesiblingsDTO[i].AMSTS_Id > 0)
                        {
                            var checkdetails = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Single(a => a.AMSTS_Id == data.savesiblingsDTO[i].AMSTS_Id);
                            checkdetails.AMSTS_SiblingsRelation = data.savesiblingsDTO[i].AMSTG_SiblingsRelation;
                            checkdetails.AMCL_Id = data.savesiblingsDTO[i].AMCL_Id;
                            checkdetails.UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Update(checkdetails);
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
                            _YearlyFeeGroupMappingContext.Adm_M_Sibling.Add(sib);
                        }
                    }
                }

                else if (data.radiobtnval == "stfoth")
                {
                    var getpercentage = _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMCC_ActiveFlag == true
                     && a.FMCC_ConcessionFlag == "E").ToList();

                    List<long> empid = new List<long>();
                    foreach (var c in data.savestudentemployeeDTO)
                    {
                        empid.Add(c.AMSTE_Id);
                    }

                    Array resultremove = _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO.Where(a => a.HRME_Id == data.HRME_Id && !empid.Contains(a.AMSTE_Id)).ToArray();

                    foreach (Adm_M_Employee_StudentDMO d in resultremove)
                    {
                        _YearlyFeeGroupMappingContext.Remove(d);
                    }

                    for (int i = 0; i < data.savestudentemployeeDTO.Length; i++)
                    {
                        if (data.savestudentemployeeDTO[i].AMSTE_Id > 0)
                        {
                            var checkresult = _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO.Single(a => a.AMSTE_Id == data.savestudentemployeeDTO[i].AMSTE_Id);
                            checkresult.AMST_Id = data.savestudentemployeeDTO[i].AMST_Id;
                            checkresult.ASMCL_Id = data.savestudentemployeeDTO[i].ASMCL_Id;
                            checkresult.AMSTE_SiblingsOrder = data.savestudentemployeeDTO[i].AMSTE_SiblingsOrder;
                            checkresult.UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Update(checkresult);
                        }
                        else
                        {
                            count += 1;
                            Adm_M_Employee_StudentDMO sib = new Adm_M_Employee_StudentDMO();
                            sib.AMST_Id = data.savestudentemployeeDTO[i].AMST_Id;
                            sib.ASMCL_Id = data.savestudentemployeeDTO[i].ASMCL_Id;
                            sib.AMSTE_SiblingsOrder = data.savestudentemployeeDTO[i].AMSTE_SiblingsOrder;
                            sib.HRME_Id = data.HRME_Id;
                            sib.AMSTE_Concessionpercentage = Convert.ToDecimal(getpercentage.FirstOrDefault().FMCC_ConcessionApplLimit);
                            sib.AMSTE_Left = 0;
                            sib.CreatedDate = DateTime.Now;
                            sib.UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(sib);
                        }
                    }
                }

                if (data.radiobtnval != "R")
                {
                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();

                            if (count > 0)
                            {
                                var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("SAVE_CONCESSION_FOR_SIBLINGS @p0,@p1,@p2,@p3,@p4",
                                                    data.MI_Id, data.ASMAY_ID, data.AMST_Id, data.HRME_Id, data.radiobtnval);

                                if (outputval >= 1)
                                {
                                    data.returnval = "true";
                                }

                                else
                                {
                                    if (data.radiobtnval == "stud")
                                    {

                                        var getsaveddetails = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToList();

                                        if (getsaveddetails.Count() > 0)
                                        {
                                            foreach (var c in getsaveddetails)
                                            {
                                                var getdetails = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.AMSTS_Siblings_AMST_ID == c.AMSTS_Siblings_AMST_ID);
                                                _YearlyFeeGroupMappingContext.Remove(getdetails);
                                            }

                                            var i = _YearlyFeeGroupMappingContext.SaveChanges();
                                            if (i > 0)
                                            {
                                                data.returnval = "false";
                                            }
                                            else
                                            {
                                                data.returnval = "false";
                                            }
                                        }
                                    }
                                    else if (data.radiobtnval == "stfoth")
                                    {
                                        var getsaveddetailsstaff = _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO.Where(a => a.HRME_Id == data.HRME_Id).ToList();

                                        if (getsaveddetailsstaff.Count() > 0)
                                        {
                                            foreach (var c in getsaveddetailsstaff)
                                            {
                                                var getdetails = _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO.Single(a => a.HRME_Id == data.HRME_Id && a.AMST_Id == c.AMST_Id);
                                                _YearlyFeeGroupMappingContext.Remove(getdetails);
                                            }

                                            var i = _YearlyFeeGroupMappingContext.SaveChanges();
                                            if (i > 0)
                                            {
                                                data.returnval = "false";
                                            }
                                            else
                                            {
                                                data.returnval = "false";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                data.returnval = "true";
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                            data.returnval = "false";
                        }
                    }
                }
                else if (data.radiobtnval == "R")
                {
                    //using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    //{
                    try
                    {
                        var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("SAVE_CONCESSION_FOR_SIBLINGS @p0,@p1,@p2,@p3,@p4",
                                            data.MI_Id, data.ASMAY_ID, data.AMST_Id, data.HRME_Id, data.radiobtnval);

                        if (outputval >= 1)
                        {
                            data.returnval = "true";
                        }
                        else
                        {
                            data.returnval = "false";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //dbCtxTxn.Rollback();
                        data.returnval = "false";
                    }
                    //}
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "false";
            }
            return data;
        }
        public Adm_M_Sibling deletedta(Adm_M_Sibling data)
        {
            var getamstid = _YearlyFeeGroupMappingContext.Adm_M_Sibling.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToList();

            List<long> amstid = new List<long>();

            foreach (var x in getamstid)
            {
                amstid.Add(x.AMSTS_Siblings_AMST_ID);
            }

            List<long> yearid = new List<long>();

            var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

            var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

            foreach (var c in getpreadmission_year)
            {
                yearid.Add(c.ASMAY_Id);
            }
            foreach (var c in getcut_year)
            {
                yearid.Add(c.ASMAY_Id);
            }

            var checkreceipt = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(a => amstid.Contains(a.AMST_Id) && yearid.Contains(a.ASMAY_Id)
            //&& a.ASMAY_Id == data.ASMAY_ID 
            && a.FSS_PaidAmount > 0).ToList();

            if (checkreceipt.Count() > 0)
            {
                data.message = "Mapped";
                return data;
            }
            else
            {
                var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DELETE_CONCESSION_FOR_SIBLINGS  @p0,@p1,@p2,@p3,@p4",
                    data.MI_Id, data.ASMAY_ID, data.AMST_Id, data.HRME_Id, "stud");

                if (outputval >= 1)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }
            return data;
        }
        public Adm_M_Sibling DeletRecordemployee(Adm_M_Sibling data)
        {
            var getamstid = _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO.Where(a => a.HRME_Id == data.HRME_Id).ToList();

            List<long> amstid = new List<long>();

            foreach (var x in getamstid)
            {
                amstid.Add(x.AMST_Id);
            }

            List<long> yearid = new List<long>();

            var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

            var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

            foreach (var c in getpreadmission_year)
            {
                yearid.Add(c.ASMAY_Id);
            }
            foreach (var c in getcut_year)
            {
                yearid.Add(c.ASMAY_Id);
            }



            var checkreceipt = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(a => amstid.Contains(a.AMST_Id) && yearid.Contains(a.ASMAY_Id)
            //&& a.ASMAY_Id == data.ASMAY_ID 
            && a.FSS_PaidAmount > 0).ToList();

            if (checkreceipt.Count() > 0)
            {
                data.message = "Mapped";
                return data;
            }
            else
            {
                var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DELETE_CONCESSION_FOR_SIBLINGS @p0,@p1,@p2,@p3,@p4",
                    data.MI_Id, data.ASMAY_ID, data.AMST_Id, data.HRME_Id, "stfoth");

                if (outputval >= 1)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }
            return data;
        }

        public Adm_M_Sibling viewsiblingdetails(Adm_M_Sibling data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.getviewdetails = (from b in _YearlyFeeGroupMappingContext.Adm_M_Sibling
                                       from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from c in _YearlyFeeGroupMappingContext.School_M_Class
                                       from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       from e in _YearlyFeeGroupMappingContext.AcademicYear
                                       where (a.AMST_Id == d.AMST_Id && d.ASMAY_Id == e.ASMAY_Id && a.AMST_Id == b.AMSTS_Siblings_AMST_ID && d.ASMCL_Id == c.ASMCL_Id
                                       && b.AMST_Id == data.AMST_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                       && b.AMSTS_TCIssuesFlag == "0"
                                       //&& d.ASMAY_Id == data.ASMAY_ID 
                                       && yearid.Contains(d.ASMAY_Id))
                                       select new Adm_M_Sibling
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

        public Adm_M_Sibling viewsiblingdetailsemployee(Adm_M_Sibling data)
        {
            try
            {
                List<long> yearid = new List<long>();

                var getpreadmission_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Pre_ActiveFlag == 1).ToList();

                var getcut_year = _YearlyFeeGroupMappingContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_ID).ToList();

                foreach (var c in getpreadmission_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }
                foreach (var c in getcut_year)
                {
                    yearid.Add(c.ASMAY_Id);
                }


                data.getviewdetails = (from b in _YearlyFeeGroupMappingContext.Adm_M_Employee_StudentDMO
                                       from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                       from c in _YearlyFeeGroupMappingContext.School_M_Class
                                       from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       from e in _YearlyFeeGroupMappingContext.AcademicYear
                                       where (a.AMST_Id == d.AMST_Id && d.ASMAY_Id == e.ASMAY_Id && a.AMST_Id == b.AMST_Id && d.ASMCL_Id == c.ASMCL_Id
                                       && b.HRME_Id == data.HRME_Id && b.AMSTE_Left == 0 && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                                       select new Adm_M_Sibling
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

        public Adm_M_Sibling checkfeegroup(Adm_M_Sibling data)
        {
            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Siblings_Employee_Student_Fee_Group_Checking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_ID
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
                        data.countmapping = retObject.Count();
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
            return data;
        }

        // Dont Refer Below Codes 
        public Adm_M_Sibling selectacadeold(Adm_M_Sibling data)
        {
            //try
            //{
            //    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
            //                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
            //                            select new Adm_M_Sibling
            //                            {
            //                                HRME_EmployeeFirstName = (a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName +
            //                                a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName +
            //                                a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName),
            //                                HRME_Id = a.HRME_Id,
            //                            }).ToArray();

            //    if (data.radiobtnval == "stud")
            //    {
            //        data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
            //                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
            //                        from c in _YearlyFeeGroupMappingContext.School_M_Class
            //                        from d in _YearlyFeeGroupMappingContext.school_M_Section
            //                        from e in _YearlyFeeGroupMappingContext.Adm_M_Sibling
            //                        where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && e.AMST_Id == b.AMST_Id
            //                        && b.ASMAY_Id == data.ASMAY_ID && a.MI_Id == data.MI_Id)
            //                        select new Adm_M_Sibling
            //                        {
            //                            AMST_FirstName = (a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName +
            //                                a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName +
            //                                a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName),
            //                            AMST_Id = a.AMST_Id,
            //                            ASMCL_ClassName = c.ASMCL_ClassName,
            //                            ASMC_SectionName = d.ASMC_SectionName,
            //                            //AMST_ORDER = e.AMST_ORDER,
            //                            //AMSS_ID = e.AMSS_ID
            //                        }).ToArray();
            //    }

            //    else if (data.radiobtnval == "stfoth")
            //    {
            //        data.alldata = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
            //                        from b in _YearlyFeeGroupMappingContext.HR_Master_Department
            //                        from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
            //                        from d in _YearlyFeeGroupMappingContext.Adm_M_Sibling

            //                        from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
            //                        from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
            //                        from g in _YearlyFeeGroupMappingContext.School_M_Class
            //                        from h in _YearlyFeeGroupMappingContext.school_M_Section

            //                        where (a.HRME_Id == d.HRME_Id && a.HRMD_Id == b.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == data.MI_Id && d.ASMAY_ID == data.ASMAY_ID && d.AMST_Id == e.AMST_Id && e.AMST_Id == f.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id)
            //                        select new Adm_M_Sibling
            //                        {
            //                            HRME_Id = a.HRME_Id,
            //                            HRME_EmployeeFirstName = (a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName +
            //                                a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName +
            //                                a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName),
            //                            HRMD_DepartmentName = b.HRMD_DepartmentName,
            //                            HRMDES_DesignationName = c.HRMDES_DesignationName,

            //                            AMST_FirstName = (e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : e.AMST_FirstName +
            //                                e.AMST_MiddleName == null || e.AMST_MiddleName == "" ? "" : " " + e.AMST_MiddleName +
            //                                e.AMST_LastName == null || e.AMST_LastName == "" ? "" : " " + e.AMST_LastName),
            //                            AMST_Id = e.AMST_Id,
            //                            ASMCL_ClassName = g.ASMCL_ClassName,
            //                            ASMC_SectionName = h.ASMC_SectionName,
            //                            AMSS_ID = d.AMSS_ID
            //                        }).ToArray();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }
    }
}
