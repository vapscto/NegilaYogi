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
    public class StudentActiveInactiveReportImpl:Interface.StudentActiveInactiveReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public StudentActiveInactiveReportImpl(ClgAdmissionContext clgadmctxt)
        {
            _clgadmctxt = clgadmctxt;
        }
        public StudentActiveInactiveReportDTO getdata(StudentActiveInactiveReportDTO data)
        {
            try
            {
                data.yearlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentActiveInactiveReportDTO onacademicyearchange(StudentActiveInactiveReportDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentActiveInactiveReportDTO oncoursechange(StudentActiveInactiveReportDTO data)
        {
            try
            {
                var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                  from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag
                                  && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentActiveInactiveReportDTO onbranchchange(StudentActiveInactiveReportDTO data)
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
                                    }).Distinct().ToList();

                data.semesterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentActiveInactiveReportDTO onchangesemester(StudentActiveInactiveReportDTO data)
        {
            try
            {
                data.sectionlist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }        
        public StudentActiveInactiveReportDTO getreport(StudentActiveInactiveReportDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_Active_Inactive_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.AMB_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.AMSE_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.ACMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_SOL", SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.AMCST_SOL)
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
                            data.getreport = retObject.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
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
