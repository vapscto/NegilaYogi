using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam.StudentMentor;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam.StudentMentor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.StudentMentor.Services
{
    public class SchoolstudentmentormappingImpl : Interface.SchoolstudentmentormappingInterface
    {
        public StudentMentorContext _context;


        public SchoolstudentmentormappingImpl(StudentMentorContext _cont)
        {
            _context = _cont;
        }

        public SchoolstudentmentormappingDTO Getdetails(SchoolstudentmentormappingDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getdetails = (from a in _context.AcademicYear
                                   from b in _context.AdmissionClass
                                   from c in _context.School_M_Section
                                   from d in _context.HR_Master_Employee_DMO
                                   from e in _context.School_Adm_Master_MentorDMO
                                   from f in _context.School_Adm_Master_Mentor_MenteeDMO
                                   where (a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && c.ASMS_Id == f.ASMS_Id
                                   && d.HRME_Id == e.HRME_Id && e.AMME_Id == f.AMME_Id && a.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id)
                                   select new SchoolstudentmentormappingDTO
                                   {
                                       ASMAY_Year = a.ASMAY_Year,
                                       ASMCL_ClassName = b.ASMCL_ClassName,
                                       ASMC_SectionName = c.ASMC_SectionName,
                                       AMME_Id = e.AMME_Id,
                                       ASMAY_Id = e.ASMAY_Id,
                                       ASMCL_Id = f.ASMCL_Id,
                                       ASMS_Id = f.ASMS_Id,
                                       HRME_Id = e.HRME_Id,
                                       employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                                       + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                                       + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName))
                                   }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolstudentmentormappingDTO onchangeyear(SchoolstudentmentormappingDTO data)
        {
            try
            {
                data.getclass = (from a in _context.Masterclasscategory
                                 from b in _context.AdmissionClass
                                 from c in _context.AcademicYear
                                 where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.Is_Active == true && b.ASMCL_ActiveFlag == true
                                 && c.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                 select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolstudentmentormappingDTO getsection(SchoolstudentmentormappingDTO data)
        {
            try
            {
                data.getsection = (from a in _context.Masterclasscategory
                                   from b in _context.AdmissionClass
                                   from c in _context.AcademicYear
                                   from d in _context.AdmSchoolMasterClassCatSec
                                   from e in _context.School_M_Section
                                   where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.Is_Active == true
                                   && e.ASMS_Id == d.ASMS_Id && a.ASMCC_Id == d.ASMCC_Id && e.ASMC_ActiveFlag == 1
                                   && b.ASMCL_ActiveFlag == true && c.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                   select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolstudentmentormappingDTO getemployee(SchoolstudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Get_Employee_Student_Details_Class_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMME_Id",
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
                        data.getemployee = retObject.ToArray();
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
        public SchoolstudentmentormappingDTO getstudentdata(SchoolstudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Get_Employee_Student_Details_Class_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMME_Id",
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
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Get_Employee_Student_Details_Class_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMME_Id",
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
                        data.getsaveddetails = retObject.ToArray();
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
        public SchoolstudentmentormappingDTO savedata(SchoolstudentmentormappingDTO data)
        {
            try
            {
                if (data.AMME_Id > 0)
                {
                    var checkdetails = _context.School_Adm_Master_MentorDMO.Single(a => a.MI_Id == data.MI_Id && a.AMME_Id == data.AMME_Id);
                    checkdetails.AMME_UpdatedBy = data.Userid;
                    checkdetails.UpdatedDate = DateTime.Now;
                    _context.Update(checkdetails);
                    if (data.SchoolstudentmentormappingtempDTO.Count() > 0)
                    {
                        List<long> temparr = new List<long>();
                        for (int k1 = 0; k1 < data.SchoolstudentmentormappingtempDTO.Count(); k1++)
                        {
                            temparr.Add(data.SchoolstudentmentormappingtempDTO[k1].AMST_Id);
                        }

                        Array Phone_Noresultremove = _context.School_Adm_Master_Mentor_MenteeDMO.Where(t => !temparr.Contains(t.AMST_Id)
                        && t.AMME_Id == data.AMME_Id).ToArray();

                        foreach (School_Adm_Master_Mentor_MenteeDMO ph1 in Phone_Noresultremove)
                        {
                            _context.Remove(ph1);
                        }

                        for (int k = 0; k < data.SchoolstudentmentormappingtempDTO.Count(); k++)
                        {
                            var checkresult1 = _context.School_Adm_Master_Mentor_MenteeDMO.Where(a => a.AMST_Id == data.SchoolstudentmentormappingtempDTO[k].AMST_Id
                              && a.AMME_Id == data.AMME_Id).ToList();

                            if (checkresult1.Count() > 0)
                            {
                                var checkresult11 = _context.School_Adm_Master_Mentor_MenteeDMO.Single(a => a.AMST_Id == data.SchoolstudentmentormappingtempDTO[k].AMST_Id && a.AMME_Id == data.AMME_Id);

                                checkresult11.UpdatedDate = DateTime.Now;
                                checkresult11.AMMEM_UpdatedBy = data.Userid;
                                _context.Update(checkresult11);
                            }
                            else
                            {
                                School_Adm_Master_Mentor_MenteeDMO dmo11 = new School_Adm_Master_Mentor_MenteeDMO();
                                dmo11.AMST_Id = data.SchoolstudentmentormappingtempDTO[k].AMST_Id;
                                dmo11.AMME_Id = data.AMME_Id;
                                dmo11.ASMCL_Id = data.ASMCL_Id;
                                dmo11.ASMS_Id = data.ASMS_Id;
                                dmo11.UpdatedDate = DateTime.Now;
                                dmo11.CreatedDate = DateTime.Now;
                                dmo11.AMMEM_CreatedBy = data.Userid;
                                dmo11.AMMEM_UpdatedBy = data.Userid;
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
                    if (data.SchoolstudentmentormappingtempDTO.Count() > 0)
                    {
                        School_Adm_Master_MentorDMO dmo = new School_Adm_Master_MentorDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.HRME_Id = data.HRME_Id;
                        dmo.AMME_CreatedBy = data.Userid;
                        dmo.AMME_UpdatedBy = data.Userid;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.CreatedDate = DateTime.Now;

                        _context.Add(dmo);

                        for (int k = 0; k < data.SchoolstudentmentormappingtempDTO.Count(); k++)
                        {
                            School_Adm_Master_Mentor_MenteeDMO dmo1 = new School_Adm_Master_Mentor_MenteeDMO();

                            dmo1.AMST_Id = data.SchoolstudentmentormappingtempDTO[k].AMST_Id;
                            dmo1.AMME_Id = dmo.AMME_Id;
                            dmo1.ASMCL_Id = data.ASMCL_Id;
                            dmo1.ASMS_Id = data.ASMS_Id;
                            dmo1.UpdatedDate = DateTime.Now;
                            dmo1.CreatedDate = DateTime.Now;
                            dmo1.AMMEM_CreatedBy = data.Userid;
                            dmo1.AMMEM_UpdatedBy = data.Userid;
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
            }
            return data;
        }
        public SchoolstudentmentormappingDTO viewrecordspopup(SchoolstudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Get_Employee_Student_Details_Class_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMME_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.AMME_Id
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
            }
            return data;
        }
        public SchoolstudentmentormappingDTO Deletedata(SchoolstudentmentormappingDTO data)
        {
            try
            {
                var checkdetails = _context.School_Adm_Master_Mentor_MenteeDMO.Where(a => a.AMST_Id == data.AMST_Id && a.AMME_Id == data.AMME_Id && a.AMMEM_Id == data.AMMEM_Id).ToList();
                if (checkdetails.Count() > 0)
                {
                    var result = _context.School_Adm_Master_Mentor_MenteeDMO.Where(a => a.AMST_Id == data.AMST_Id && a.AMME_Id == data.AMME_Id && a.AMMEM_Id == data.AMMEM_Id);
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
                    cmd.CommandText = "School_Get_Employee_Student_Details_Class_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMME_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.AMME_Id
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
            }
            return data;
        }
        public SchoolstudentmentormappingDTO onreport(SchoolstudentmentormappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Get_Employee_Student_Details_Class_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 5
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMME_Id",
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
