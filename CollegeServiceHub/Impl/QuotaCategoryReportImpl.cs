using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
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
    public class QuotaCategoryReportImpl : QuotaCategoryReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public QuotaCategoryReportImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }
        public QuotaCategoryReportDTO getdetails(QuotaCategoryReportDTO data)
        {
            try
            {
                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
                data.quotalist = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQ_Id).ToArray();
                data.catlist = _clgadmctxt.Clg_Adm_College_Quota_CategoryDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQC_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public QuotaCategoryReportDTO onselectAcdYear(QuotaCategoryReportDTO data)
        {
            try
            {

                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   from c in _clgadmctxt.AcademicYear
                                   where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                   select a
                                  ).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public QuotaCategoryReportDTO onselectCourse(QuotaCategoryReportDTO data)
        {
            try
            {

                data.branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                   from d in _clgadmctxt.AcademicYear
                                   where (a.AMB_Id == c.AMB_Id && b.ACAYC_Id == c.ACAYC_Id && b.ASMAY_Id == d.ASMAY_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id)
                                   select a).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public QuotaCategoryReportDTO onselectBranch(QuotaCategoryReportDTO data)
        {
            try
            {
                if (data.AMB_Id != 0)
                {
                    data.semlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    where (b.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && c.ACAYC_Id == d.ACAYC_Id
                                    && b.ACAYCB_Id == d.ACAYCB_Id && c.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id
                                    && c.ASMAY_Id == data.ASMAY_Id)
                                    select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }
                else
                {
                    data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public QuotaCategoryReportDTO onreport(QuotaCategoryReportDTO data)
        {
            try
            {
                string branchid = "";
                string semid = "";
                string acqid = "";
                string acqcid = "";

                if (data.AMB_Id == 0)
                {
                    for (int b = 0; b < data.branchid.Count(); b++)
                    {
                        if (b == 0)
                        {
                            branchid = data.branchid[b].AMB_Id.ToString();
                        }
                        else
                        {
                            branchid = branchid + ',' + data.branchid[b].AMB_Id.ToString();
                        }
                    }
                }
                else
                {
                    branchid = data.AMB_Id.ToString();
                }

                if (data.AMSE_Id == 0)
                {
                    for (int c = 0; c < data.semester.Count(); c++)
                    {
                        if (c == 0)
                        {
                            semid = data.semester[c].AMSE_Id.ToString();
                        }
                        else
                        {
                            semid = semid + ',' + data.semester[c].AMSE_Id.ToString();
                        }
                    }
                }
                else
                {
                    semid = data.AMSE_Id.ToString();
                }

                if (data.ACQ_Id == 0)
                {
                    for (int d = 0; d < data.quota.Count(); d++)
                    {
                        if (d == 0)
                        {
                            acqid = data.quota[d].ACQ_Id.ToString();
                        }
                        else
                        {
                            acqid = acqid + ',' + data.quota[d].ACQ_Id.ToString();
                        }
                    }
                }
                else
                {
                    acqid = data.ACQ_Id.ToString();
                }


                if (data.ACQC_Id == 0)
                {
                    for (int e = 0; e < data.quotacategry.Count(); e++)
                    {
                        if (e == 0)
                        {
                            acqcid = data.quotacategry[e].ACQC_Id.ToString();
                        }
                        else
                        {
                            acqcid = acqcid + ',' + data.quotacategry[e].ACQC_Id.ToString();
                        }
                    }
                }
                else
                {
                    acqcid = data.ACQC_Id.ToString();
                }



                List<QuotaCategoryReportDTO> result = new List<QuotaCategoryReportDTO>();

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Quota_Category";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.VarChar) { Value = semid });
                    cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.VarChar) { Value = branchid });
                    cmd.Parameters.Add(new SqlParameter("@acms_id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@acq_id", SqlDbType.VarChar) { Value = acqid });
                    cmd.Parameters.Add(new SqlParameter("@acqc_id", SqlDbType.VarChar) { Value = acqcid });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        //using (var dataReader = cmd.ExecuteReader())
                        //{
                        //    while (dataReader.Read())
                        //    {
                        //        result.Add(new QuotaCategoryReportDTO
                        //        {
                        //            std_name = dataReader["std_name"].ToString(),
                        //            AMCST_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString(),
                        //            AMCST_DOB = Convert.ToDateTime(dataReader["AMCST_DOB"].ToString()),
                        //            AMCST_FatherName = dataReader["AMCST_FatherName"].ToString(),
                        //            AMCST_FatherOccupation = dataReader["AMCST_FatherOccupation"].ToString(),
                        //            AMCST_Sex = dataReader["AMCST_Sex"].ToString(),
                        //            std_address = dataReader["std_address"].ToString(),
                        //            ACQ_QuotaName = dataReader["ACQ_QuotaName"].ToString(),
                        //            AMCST_MobileNo = Convert.ToInt32(dataReader["AMCST_MobileNo"].ToString()),
                        //            ACQC_CategoryName = dataReader["ACQC_CategoryName"].ToString()
                        //        });
                        //       // data.datareport = result.ToArray();
                        //    }
                        //}
                        using (var dataReader =  cmd.ExecuteReader())
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
                        data.datareport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.masterinst = _clgadmctxt.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
