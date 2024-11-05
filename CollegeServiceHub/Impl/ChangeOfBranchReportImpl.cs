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
    public class ChangeOfBranchReportImpl : Interface.ChangeOfBranchReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public ChangeOfBranchReportImpl(ClgAdmissionContext n)
        {
            _clgadmctxt = n;
        }
        public ChangeOfBranchReportDTO loaddata(ChangeOfBranchReportDTO data)
        {
            try
            {
                data.academiclist = _clgadmctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ChangeOfBranchReportDTO getcourse(ChangeOfBranchReportDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
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
        public ChangeOfBranchReportDTO getbranch(ChangeOfBranchReportDTO data)
        {
            try
            {
                data.branchlist = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
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
        public async Task<ChangeOfBranchReportDTO> Report(ChangeOfBranchReportDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ChangeOfBranchReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });


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
                        data.reportdata = retObject.ToArray();
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
