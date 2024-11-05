using DataAccessMsSqlServerProvider;
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
    public class BloodGroupWiseStudentDetailsReportImpl:Interface.BloodGroupWiseStudentDetailsReportInterface
    {
        public ClgAdmissionContext _Context;
       
        public BloodGroupWiseStudentDetailsReportImpl(ClgAdmissionContext a)
        {
            _Context = a;
           
        }      
        
       
        public BloodGroupWiseStudentDetailsReportDTO loaddata(BloodGroupWiseStudentDetailsReportDTO data)
        {
            try
            {
                data.allacademicyear = _Context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().ToArray();
                data.sectionlist = _Context.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public BloodGroupWiseStudentDetailsReportDTO getcourse(BloodGroupWiseStudentDetailsReportDTO data)
        {
            try
            {
                data.courselist = (from a in _Context.MasterCourseDMO
                                   from b in _Context.CLG_Adm_College_AY_CourseDMO
                                   where (b.AMCO_Id == a.AMCO_Id&&a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag  )
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public BloodGroupWiseStudentDetailsReportDTO getbranch(BloodGroupWiseStudentDetailsReportDTO data)
        {
            try
            {
                var branches = (from a in _Context.ClgMasterBranchDMO
                                   from b in _Context.CLG_Adm_College_AY_CourseDMO
                                   from c in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                   where (b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.AMB_Id == a.AMB_Id && a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag   && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag)
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
                data.branchlist = branches.OrderByDescending(t=>t.AMB_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public BloodGroupWiseStudentDetailsReportDTO getsemester(BloodGroupWiseStudentDetailsReportDTO data)
        {
            try
            {
                List<long> branch_ids = new List<long>();
                if (data.branchess.Length > 0)
                {
                    foreach (var item in data.branchess)
                    {
                        branch_ids.Add(item.AMB_Id);
                    }
                }
                var semisters = (from a in _Context.CLG_Adm_Master_SemesterDMO
                                    from b in _Context.CLG_Adm_College_AY_CourseDMO
                                    from c in _Context.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _Context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id &&d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag  && c.MI_Id == data.MI_Id && branch_ids.Contains(c.AMB_Id) && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id&& d.ACAYCBS_ActiveFlag)
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
                data.semesterlist = semisters.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public BloodGroupWiseStudentDetailsReportDTO Report(BloodGroupWiseStudentDetailsReportDTO data)
        {
            try
            {
                data.year = (from a in _Context.AcademicYear
                             where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                             select new BloodGroupWiseStudentDetailsReportDTO
                             {
                                 ASMAY_Id = a.ASMAY_Id,
                                 ASMAY_Year = a.ASMAY_Year,

                             }).Distinct().ToArray();
                data.sem = (from a in _Context.CLG_Adm_Master_SemesterDMO
                            where (a.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id)
                            select new BloodGroupWiseStudentDetailsReportDTO
                            {
                                AMSE_Id = a.AMSE_Id,
                                AMSE_SEMName = a.AMSE_SEMName,

                            }).Distinct().ToArray();
               
                string branch_ids = "0";
                for (int d = 0; d < data.branchess.Count(); d++)
                {
                    branch_ids = branch_ids + ',' + data.branchess[d].AMB_Id;
                }
                string sectionids = "0";
                for (int d = 0; d < data.clsidlist.Count(); d++)
                {
                    sectionids = sectionids + ',' + data.clsidlist[d].ACMS_Id;
                }
                string bloodids = "'0'";
                for(int d = 0; d < data.blood1.Count(); d++)
                {
                    bloodids =bloodids+ ','  + "'"+data.blood1[d].name+"'";
                }
                //data.all = (from a in _Context.Adm_College_Yearly_StudentDMO
                //            from b in _Context.MasterCourseDMO
                //            from c in _Context.CLG_Adm_Master_SemesterDMO
                //            from d in _Context.ClgMasterBranchDMO
                //            from e in _Context.Adm_Master_College_StudentDMO
                //            from f in _Context.AcademicYear
                //            where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == c.AMSE_Id && a.AMCO_Id == data.AMCO_Id && c.AMSE_Id == data.AMSE_Id && branch_ids.Contains(d.AMB_Id) && bloodids.Contains(e.AMCST_BloodGroup))
                //            select new BloodGroupWiseStudentDetailsReportDTO
                //            {
                //                AMCO_CourseName = b.AMCO_CourseName,
                //                AMSE_SEMName = c.AMSE_SEMName,
                //                AMB_BranchName = d.AMB_BranchName,
                //                ASMAY_Year = f.ASMAY_Year,
                        

                //            }).Distinct().ToArray();
                //var www = (from a in _Context.ClgMasterBranchDMO
                //           from b in _Context.MasterCourseDMO
                //           where (a.AMB_Id == data.AMB_Id && b.AMCO_Id == data.AMCO_Id && a.MI_Id == b.MI_Id)).ToList();

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "[BloodGroupWiseStudentDetailsReport]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_IDs", SqlDbType.VarChar) { Value = branch_ids });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_IDs", SqlDbType.VarChar) { Value = sectionids });
                    cmd.Parameters.Add(new SqlParameter("@blood_IDs", SqlDbType.VarChar) { Value = bloodids });
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
                        data.studentDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
