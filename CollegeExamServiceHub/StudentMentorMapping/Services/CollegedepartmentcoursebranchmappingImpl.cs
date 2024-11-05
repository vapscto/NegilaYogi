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
    public class CollegedepartmentcoursebranchmappingImpl : Interface.CollegedepartmentcoursebranchmappingInterface
    {
        public StudentMentorMappingContext _context;
        public CollegedepartmentcoursebranchmappingImpl(StudentMentorMappingContext context)
        {
            _context = context;
        }

        public CollegedepartmentcoursebranchmappingDTO Getdetails(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                data.deptlist = _context.HR_Master_Department_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(a => a.HRMD_Order).ToArray();

                data.courselist = _context.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                data.getdetails = (from a in _context.Clg_Adm_Dept_Course_Branch_MappingDMO
                                   from b in _context.MasterCourseDMO                                  
                                   from d in _context.HR_Master_Department_DMO
                                   where (a.HRMD_Id == d.HRMD_Id && a.AMCO_Id == b.AMCO_Id && a.MI_Id == data.MI_Id)
                                   select new CollegedepartmentcoursebranchmappingDTO
                                   {
                                       ADCO_Id = a.ADCO_Id,
                                       AMCO_CourseName = b.AMCO_CourseName,                                                                          
                                       AMCO_Id = a.AMCO_Id,
                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                       HRMD_Id = a.HRMD_Id,
                                       ADCO_ActiveFlag = a.ADCO_ActiveFlag

                                   }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO getbranch(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                data.branchlist = (from a in _context.MasterCourseDMO
                                   from b in _context.ClgMasterBranchDMO
                                   from c in _context.Adm_Course_Branch_MappingDMO
                                   where (a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && a.MI_Id == data.MI_Id && c.AMCOBM_ActiveFlg == true
                                   && c.AMCO_Id == data.AMCO_Id && a.AMCO_Id == data.AMCO_Id && a.AMCO_ActiveFlag == true && b.AMB_ActiveFlag == true)
                                   select b).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO getsemester(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                data.semesterlist = (from a in _context.MasterCourseDMO
                                     from b in _context.ClgMasterBranchDMO
                                     from c in _context.Adm_Course_Branch_MappingDMO
                                     from d in _context.AdmCourseBranchSemesterMappingDMO
                                     from e in _context.CLG_Adm_Master_SemesterDMO
                                     where (a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && e.AMSE_Id == d.AMSE_Id
                                     && a.MI_Id == data.MI_Id && c.AMCOBM_ActiveFlg == true && d.AMCOBM_Id == c.AMCOBM_Id
                                     && c.AMCO_Id == data.AMCO_Id && a.AMCO_Id == data.AMCO_Id && a.AMCO_ActiveFlag == true
                                     && b.AMB_ActiveFlag == true && d.AMCOBMS_ActiveFlg == true && e.AMSE_ActiveFlg == true && c.AMB_Id == data.AMB_Id)
                                     select e).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO savedetails(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                if (data.semesterselecteddetails.Count() > 0)
                {
                    var checkduplicate = _context.Clg_Adm_Dept_Course_Branch_MappingDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.HRMD_Id == data.HRMD_Id && a.AMCO_Id == data.AMCO_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        var result = _context.Clg_Adm_Dept_Course_Branch_MappingDMO.Single(a => a.MI_Id == data.MI_Id
                     && a.HRMD_Id == data.HRMD_Id && a.AMCO_Id == data.AMCO_Id);
                        result.ADCO_UpdatedBy = data.Userid;
                        result.UpdatedDate = DateTime.Now;

                        _context.Update(result);

                        for (int k = 0; k < data.semesterselecteddetails.Count(); k++)
                        {
                            var checkduplicate1 = _context.Clg_Adm_Dept_Course_Branch_Semester_MappingDMO.Where(a => a.ADCO_Id == checkduplicate.FirstOrDefault().ADCO_Id && a.AMSE_Id == data.semesterselecteddetails[k].AMSE_Id && a.AMB_Id==data.AMB_Id).ToList();


                            if (checkduplicate1.Count() > 0)
                            {
                                var resilt1 = _context.Clg_Adm_Dept_Course_Branch_Semester_MappingDMO.Single(a => a.ADCO_Id == checkduplicate.FirstOrDefault().ADCO_Id && a.AMSE_Id == data.semesterselecteddetails[k].AMSE_Id);
                                resilt1.ADCOBS_UpdatedBy = data.Userid;
                                resilt1.UpdatedDate = DateTime.Now;
                                _context.Update(resilt1);
                            }
                            else
                            {
                                Clg_Adm_Dept_Course_Branch_Semester_MappingDMO dmo1 = new Clg_Adm_Dept_Course_Branch_Semester_MappingDMO();
                                dmo1.ADCO_Id = checkduplicate.FirstOrDefault().ADCO_Id;
                                dmo1.AMSE_Id = data.semesterselecteddetails[k].AMSE_Id;
                                dmo1.AMB_Id = data.AMB_Id;
                                dmo1.ADCOBS_UpdatedBy = data.Userid;
                                dmo1.ADCOBS_CreatedBy = data.Userid;
                                dmo1.ADCOBS_ActiveFlag = true;
                                dmo1.CreatedDate = DateTime.Now;
                                dmo1.UpdatedDate = DateTime.Now;
                                _context.Add(dmo1);
                            }
                        }
                    }
                    else
                    {
                        Clg_Adm_Dept_Course_Branch_MappingDMO dmo = new Clg_Adm_Dept_Course_Branch_MappingDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.HRMD_Id = data.HRMD_Id;
                        dmo.AMCO_Id = data.AMCO_Id;                       
                        dmo.ADCO_UpdatedBy = data.Userid;
                        dmo.ADCO_CreatedBy = data.Userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.ADCO_ActiveFlag = true;
                        _context.Add(dmo);
                        for (int k = 0; k < data.semesterselecteddetails.Count(); k++)
                        {
                            Clg_Adm_Dept_Course_Branch_Semester_MappingDMO dmo1 = new Clg_Adm_Dept_Course_Branch_Semester_MappingDMO();
                            dmo1.ADCO_Id = dmo.ADCO_Id;
                            dmo1.AMSE_Id = data.semesterselecteddetails[k].AMSE_Id;
                            dmo1.AMB_Id = data.AMB_Id;
                            dmo1.ADCOBS_CreatedBy = data.Userid;
                            dmo1.ADCOBS_UpdatedBy = data.Userid;
                            dmo1.ADCOBS_ActiveFlag = true;
                            dmo1.CreatedDate = DateTime.Now;
                            dmo1.UpdatedDate = DateTime.Now;

                            _context.Add(dmo1);

                        }
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO viewrecordspopup(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                data.getsemdetails = (from a in _context.Clg_Adm_Dept_Course_Branch_MappingDMO
                                      from b in _context.MasterCourseDMO
                                      from c in _context.ClgMasterBranchDMO
                                      from d in _context.HR_Master_Department_DMO
                                      from e in _context.Clg_Adm_Dept_Course_Branch_Semester_MappingDMO
                                      from f in _context.CLG_Adm_Master_SemesterDMO
                                      where (a.HRMD_Id == d.HRMD_Id && a.AMCO_Id == b.AMCO_Id && e.AMSE_Id == f.AMSE_Id && a.ADCO_Id == e.ADCO_Id
                                      && e.AMB_Id == c.AMB_Id && a.MI_Id == data.MI_Id && e.ADCO_Id == data.ADCO_Id && a.AMCO_Id == data.AMCO_Id
                                      && a.HRMD_Id == data.HRMD_Id)
                                      select new CollegedepartmentcoursebranchmappingDTO
                                      {
                                          ADCO_Id = a.ADCO_Id,
                                          AMCO_CourseName = b.AMCO_CourseName,
                                          AMB_BranchName = c.AMB_BranchName,
                                          AMB_Id = e.AMB_Id,
                                          AMCO_Id = a.AMCO_Id,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName,
                                          HRMD_Id = a.HRMD_Id,
                                          ADCO_ActiveFlag = a.ADCO_ActiveFlag,
                                          AMSE_Id = f.AMSE_Id,
                                          AMSE_SEMName = f.AMSE_SEMName,
                                          ADCOBS_Id = e.ADCOBS_Id,
                                          ADCOBS_ActiveFlag = e.ADCOBS_ActiveFlag

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO semesterdeactive(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                var result = _context.Clg_Adm_Dept_Course_Branch_Semester_MappingDMO.Single(a => a.ADCOBS_Id == data.ADCOBS_Id);
                if (result.ADCOBS_ActiveFlag == true)
                {
                    result.ADCOBS_ActiveFlag = false;
                }
                else
                {
                    result.ADCOBS_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.ADCOBS_UpdatedBy = data.Userid;
                _context.Update(result);
                int i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                data.getsemdetails = (from a in _context.Clg_Adm_Dept_Course_Branch_MappingDMO
                                      from b in _context.MasterCourseDMO
                                      from c in _context.ClgMasterBranchDMO
                                      from d in _context.HR_Master_Department_DMO
                                      from e in _context.Clg_Adm_Dept_Course_Branch_Semester_MappingDMO
                                      from f in _context.CLG_Adm_Master_SemesterDMO
                                      where (a.HRMD_Id == d.HRMD_Id && a.AMCO_Id == b.AMCO_Id && e.AMSE_Id == f.AMSE_Id && a.ADCO_Id == e.ADCO_Id
                                      && e.AMB_Id == c.AMB_Id && a.MI_Id == data.MI_Id && e.ADCO_Id == data.ADCO_Id && a.AMCO_Id == data.AMCO_Id
                                      && a.HRMD_Id == data.HRMD_Id)
                                      select new CollegedepartmentcoursebranchmappingDTO
                                      {
                                          ADCO_Id = a.ADCO_Id,
                                          AMCO_CourseName = b.AMCO_CourseName,
                                          AMB_BranchName = c.AMB_BranchName,
                                          AMB_Id = e.AMB_Id,
                                          AMCO_Id = a.AMCO_Id,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName,
                                          HRMD_Id = a.HRMD_Id,
                                          ADCO_ActiveFlag = a.ADCO_ActiveFlag,
                                          AMSE_Id = f.AMSE_Id,
                                          AMSE_SEMName = f.AMSE_SEMName,
                                          ADCOBS_Id = e.ADCOBS_Id,
                                          ADCOBS_ActiveFlag = e.ADCOBS_ActiveFlag

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO deactivate(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                var result = _context.Clg_Adm_Dept_Course_Branch_MappingDMO.Single(a => a.ADCO_Id == data.ADCO_Id);
                if (result.ADCO_ActiveFlag == true)
                {
                    result.ADCO_ActiveFlag = false;
                }
                else
                {
                    result.ADCO_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.ADCO_UpdatedBy = data.Userid;
                _context.Update(result);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO Getdetailsreport(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {
                data.deptlist = _context.HR_Master_Department_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(a => a.HRMD_Order).ToArray();

                data.courselist = _context.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegedepartmentcoursebranchmappingDTO getreport(CollegedepartmentcoursebranchmappingDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Department_Course_Branch_Semster_Mapping_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.HRMD_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
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
                        data.getreport = retObject.ToArray();
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
