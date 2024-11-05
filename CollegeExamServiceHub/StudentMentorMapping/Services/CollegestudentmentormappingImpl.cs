using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Exam.StudentMentorMapping;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.StudentMentorMapping.Services
{
    public class CollegestudentmentormappingImpl : Interface.CollegestudentmentormappingInterface
    {
        public StudentMentorMappingContext _context;

        public CollegestudentmentormappingImpl(StudentMentorMappingContext _cont)
        {
            _context = _cont;
        }

        public CollegestudentmentormappingDTO Getdetails(CollegestudentmentormappingDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getdetails = (from a in _context.Clg_Student_Mentor_UserDMO
                                   from h in _context.Colleg_Student_Mentor_DetailsDMO
                                   from b in _context.MasterCourseDMO
                                   from c in _context.ClgMasterBranchDMO
                                   from d in _context.CLG_Adm_Master_SemesterDMO
                                   from e in _context.Adm_College_Master_SectionDMO
                                   from f in _context.HR_Master_Employee_DMO
                                   from g in _context.AcademicYear
                                   where (a.ASMAY_Id == g.ASMAY_Id && h.AMCO_Id == b.AMCO_Id && h.AMB_Id == c.AMB_Id && h.AMSE_Id == d.AMSE_Id
                                   && h.ACMS_Id == e.ACMS_Id && a.HRME_Id == f.HRME_Id && a.AMMEC_Id==h.AMMEC_Id && a.MI_Id==data.MI_Id)
                                   select new CollegestudentmentormappingDTO
                                   {
                                       AMMEC_Id = a.AMMEC_Id,
                                       AMCO_Id = h.AMCO_Id,
                                       AMSE_Id = h.AMSE_Id,
                                       AMB_Id = h.AMB_Id,
                                       ACMS_Id = h.ACMS_Id,
                                       HRME_Id = a.HRME_Id,
                                       ASMAY_Id = a.ASMAY_Id,
                                       coursename = b.AMCO_CourseName,
                                       branchname = c.AMB_BranchName,
                                       semestername = d.AMSE_SEMName,
                                       sectioname = e.ACMS_SectionName,
                                       yearname = g.ASMAY_Year,
                                       employeename = ((f.HRME_EmployeeFirstName == null ? " " : f.HRME_EmployeeFirstName) + (f.HRME_EmployeeMiddleName == null ||
                                       f.HRME_EmployeeMiddleName == "" ? "" : " " + f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == "" ? "" : " " + f.HRME_EmployeeLastName))
                                   }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegestudentmentormappingDTO onchangeyear(CollegestudentmentormappingDTO data)
        {
            try
            {
                data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  where (a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == b.AMCO_Id && a.ACAYC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                  && a.ASMAY_Id == data.ASMAY_Id)
                                  select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegestudentmentormappingDTO getbranch(CollegestudentmentormappingDTO data)
        {
            try
            {
                data.getbranch = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  from e in _context.ClgMasterBranchDMO
                                  where (a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == b.AMCO_Id && d.ACAYC_Id == a.ACAYC_Id
                                  && a.ACAYC_ActiveFlag == true && d.AMB_Id == e.AMB_Id && d.ACAYCB_ActiveFlag == true
                                  && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                  select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegestudentmentormappingDTO getsemester(CollegestudentmentormappingDTO data)
        {
            try
            {
                data.getsemester = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                    from b in _context.MasterCourseDMO
                                    from c in _context.AcademicYear
                                    from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from e in _context.ClgMasterBranchDMO
                                    from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from g in _context.CLG_Adm_Master_SemesterDMO
                                    where (a.ASMAY_Id == c.ASMAY_Id && a.AMCO_Id == b.AMCO_Id && d.ACAYC_Id == a.ACAYC_Id
                                    && a.ACAYC_ActiveFlag == true && d.AMB_Id == e.AMB_Id && f.ACAYCB_Id == d.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id
                                    && f.ACAYCBS_ActiveFlag == true && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                    && d.AMB_Id == data.AMB_Id)
                                    select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegestudentmentormappingDTO getsection(CollegestudentmentormappingDTO data)
        {
            try
            {
                data.getsection = _context.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegestudentmentormappingDTO getemployee(CollegestudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Employee_Student_Details_Course_Branch_Semester";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECSMU_Id",
               SqlDbType.VarChar)
                    {
                        Value = 0
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
                        data.getemployeedetails = retObject.ToArray();
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
        public CollegestudentmentormappingDTO getstudentdata(CollegestudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Employee_Student_Details_Course_Branch_Semester";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECSMU_Id",
               SqlDbType.VarChar)
                    {
                        Value = 0
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
                        data.getstudentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Employee_Student_Details_Course_Branch_Semester";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECSMU_Id",
                   SqlDbType.VarChar)
                    {
                        Value = 0
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
                        data.getsavedstudentlist = retObject.ToArray();
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
        public CollegestudentmentormappingDTO savedata(CollegestudentmentormappingDTO data)
        {
            try
            {
                if (data.AMMEC_Id > 0)
                {
                    var checkresult = _context.Clg_Student_Mentor_UserDMO.Single(a => a.MI_Id == data.MI_Id && a.AMMEC_Id == data.AMMEC_Id);
                    checkresult.UpdatedDate = DateTime.Now;
                    checkresult.AMMEC_UpdatedBy = data.Userid;
                    _context.Update(checkresult);

                    if (data.CollegestudentmentormappingtempDTO.Count() > 0)
                    {
                        List<long> temparr = new List<long>();
                        for (int k1 = 0; k1 < data.CollegestudentmentormappingtempDTO.Count(); k1++)
                        {
                            temparr.Add(data.CollegestudentmentormappingtempDTO[k1].AMCST_Id);
                        }

                        Array Phone_Noresultremove = _context.Colleg_Student_Mentor_DetailsDMO.Where(t => !temparr.Contains(t.AMCST_Id)
                        && t.AMMEC_Id == data.AMMEC_Id).ToArray();

                        foreach (Colleg_Student_Mentor_DetailsDMO ph1 in Phone_Noresultremove)
                        {
                            _context.Remove(ph1);
                        }


                        for (int k = 0; k < data.CollegestudentmentormappingtempDTO.Count(); k++)
                        {
                            var checkresult1 = _context.Colleg_Student_Mentor_DetailsDMO.Where(a => a.AMCST_Id == data.CollegestudentmentormappingtempDTO[k].AMCST_Id
                              && a.AMMEC_Id == data.AMMEC_Id).ToList();

                            if (checkresult1.Count() > 0)
                            {
                                var checkresult11 = _context.Colleg_Student_Mentor_DetailsDMO.Single(a => a.AMCST_Id == data.CollegestudentmentormappingtempDTO[k].AMCST_Id && a.AMMEC_Id == data.AMMEC_Id);

                                checkresult11.UpdatedDate = DateTime.Now;
                                checkresult11.AMMECM_UpdatedBy = data.Userid;
                                _context.Update(checkresult11);
                            }
                            else
                            {
                                Colleg_Student_Mentor_DetailsDMO dmo11 = new Colleg_Student_Mentor_DetailsDMO();
                                dmo11.AMCST_Id = data.CollegestudentmentormappingtempDTO[k].AMCST_Id;
                                dmo11.AMMEC_Id = data.AMMEC_Id;
                                dmo11.AMCO_Id = data.AMCO_Id;
                                dmo11.AMB_Id = data.AMB_Id;
                                dmo11.AMSE_Id = data.AMSE_Id;
                                dmo11.ACMS_Id = data.ACMS_Id;
                                dmo11.UpdatedDate = DateTime.Now;
                                dmo11.CreatedDate = DateTime.Now;
                                dmo11.AMMECM_CreatedBy = data.Userid;
                                dmo11.AMMECM_UpdatedBy = data.Userid;
                                dmo11.AMMECM_Activeflag = true;
                                _context.Add(dmo11);
                            }
                        }
                        int i = _context.SaveChanges();
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
                else
                {
                    if (data.CollegestudentmentormappingtempDTO.Count() > 0)
                    {
                        Clg_Student_Mentor_UserDMO dmo = new Clg_Student_Mentor_UserDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.HRME_Id = data.HRME_Id;
                        dmo.AMMEC_Activeflag = true;
                        dmo.AMMEC_CreatedBy = data.Userid;
                        dmo.AMMEC_UpdatedBy = data.Userid;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.CreatedDate = DateTime.Now;

                        _context.Add(dmo);

                        for (int k = 0; k < data.CollegestudentmentormappingtempDTO.Count(); k++)
                        {
                            Colleg_Student_Mentor_DetailsDMO dmo1 = new Colleg_Student_Mentor_DetailsDMO();

                            dmo1.AMCST_Id = data.CollegestudentmentormappingtempDTO[k].AMCST_Id;
                            dmo1.AMMEC_Id = dmo.AMMEC_Id;
                            dmo1.AMCO_Id = data.AMCO_Id;
                            dmo1.AMB_Id = data.AMB_Id;
                            dmo1.AMSE_Id = data.AMSE_Id;
                            dmo1.ACMS_Id = data.ACMS_Id;                            
                            dmo1.UpdatedDate = DateTime.Now;
                            dmo1.CreatedDate = DateTime.Now;
                            dmo1.AMMECM_CreatedBy = data.Userid;
                            dmo1.AMMECM_UpdatedBy = data.Userid;
                            dmo1.AMMECM_Activeflag = true;
                            _context.Add(dmo1);
                        }

                        int i = _context.SaveChanges();
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public CollegestudentmentormappingDTO viewrecordspopup(CollegestudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Employee_Student_Details_Course_Branch_Semester";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECSMU_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.AMMEC_Id
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
                        data.getstudentdata = retObject.ToArray();
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
                data.returnval = false;
            }
            return data;
        }
        public CollegestudentmentormappingDTO Deletedata(CollegestudentmentormappingDTO data)
        {
            try
            {
                var checkdetails = _context.Colleg_Student_Mentor_DetailsDMO.Where(a => a.AMCST_Id == data.AMCST_Id && a.AMMEC_Id == data.AMMEC_Id && a.AMMECM_Id == data.AMMECM_Id).ToList();
                if (checkdetails.Count() > 0)
                {
                    var result = _context.Colleg_Student_Mentor_DetailsDMO.Single(a => a.AMCST_Id == data.AMCST_Id && a.AMMEC_Id == data.AMMEC_Id && a.AMMECM_Id == data.AMMECM_Id);
                    _context.Remove(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Employee_Student_Details_Course_Branch_Semester";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECSMU_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.AMMEC_Id
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
                        data.getstudentdata = retObject.ToArray();
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
                data.returnval = false;
            }
            return data;
        }

        //Report
        public CollegestudentmentormappingDTO Getreportdetails(CollegestudentmentormappingDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegestudentmentormappingDTO getreport(CollegestudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Employee_Student_Details_Course_Branch_Semester_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.getreportdata = retObject.ToArray();
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
    }
}
