using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
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
    public class ClgAttendanceSMSDetailsReportImpl:Interface.ClgAttendanceSMSDetailsReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public ClgAttendanceSMSDetailsReportImpl(ClgAdmissionContext _context)
        {
            _clgadmctxt = _context;
        }
        public ClgAttendanceSMSDetailsReport_DTO loaddata(ClgAttendanceSMSDetailsReport_DTO data)
        {
            try
            {
                data.yearlist = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().ToArray();
                data.sectionlist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClgAttendanceSMSDetailsReport_DTO getcourse(ClgAttendanceSMSDetailsReport_DTO data)
        {
            try
            {
                data.getcourse = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from b in _clgadmctxt.MasterCourseDMO
                                  from c in _clgadmctxt.AcademicYear
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                  && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id)
                                  select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClgAttendanceSMSDetailsReport_DTO getbranch(ClgAttendanceSMSDetailsReport_DTO data)
        {
            try
            {
                data.getbranch = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                  from b in _clgadmctxt.MasterCourseDMO
                                  from c in _clgadmctxt.AcademicYear
                                  from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                  from e in _clgadmctxt.ClgMasterBranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                  && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                  && d.ACAYCB_ActiveFlag == true && e.AMB_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id)
                                  select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ClgAttendanceSMSDetailsReport_DTO getsemester(ClgAttendanceSMSDetailsReport_DTO data)
        {
            try
            {
                List<long> branchid = new List<long>();               
                foreach (var branch in data.selectedbranchlist)
                {
                    branchid.Add(branch.AMB_Id);
                }
                data.getsemester = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from b in _clgadmctxt.MasterCourseDMO
                                    from c in _clgadmctxt.AcademicYear
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from e in _clgadmctxt.ClgMasterBranchDMO
                                    from f in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from g in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                    && g.AMSE_Id == f.AMSE_Id && f.ACAYCB_Id == d.ACAYCB_Id && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                    && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id&& branchid.Contains(e.AMB_Id) 
                                    && d.ACAYCB_ActiveFlag == true && e.AMB_ActiveFlag == true && g.AMSE_ActiveFlg == true && f.ACAYCBS_ActiveFlag == true)
                                    select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<ClgAttendanceSMSDetailsReport_DTO> showdetails(ClgAttendanceSMSDetailsReport_DTO data)
        {
            try
            {
                string year_ids = "0";
                List<long> year1_ids = new List<long>();
                foreach (var item in data.selectedbranchlist)
                {
                    year1_ids.Add(item.AMB_Id);
                }
                for (int s = 0; s < year1_ids.Count(); s++)
                {
                    year_ids = year_ids + ',' + year1_ids[s].ToString();
                }
                // type
                string type_ids = "0";
                List<long> type1_ids = new List<long>();
                foreach (var item in data.selectedsemesterlist)
                {
                    type1_ids.Add(item.AMSE_Id);
                }
                for (int s = 0; s < type1_ids.Count(); s++)
                {
                    type_ids = type_ids + ',' + type1_ids[s].ToString();
                }
                //int r = 0;
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_Attendance_SMSDetailsReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                    SqlDbType.VarChar)
                    {
                        Value = year_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    SqlDbType.VarChar)
                    {
                        Value = type_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.reportlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
