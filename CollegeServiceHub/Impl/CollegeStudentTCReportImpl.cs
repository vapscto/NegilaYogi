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
    public class CollegeStudentTCReportImpl : Interface.CollegeStudentTCReportInterface
    {
        public ClgAdmissionContext _clgadmctxt;

        public CollegeStudentTCReportImpl(ClgAdmissionContext _dmctxt)
        {
            _clgadmctxt = _dmctxt;
        }

        public CollegeStudentTCReportDTO getalldetails(CollegeStudentTCReportDTO data)
        {
            try
            {
                data.getyear = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudentTCReportDTO onchangeyear(CollegeStudentTCReportDTO data)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudentTCReportDTO onchangecourse(CollegeStudentTCReportDTO data)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudentTCReportDTO onchangebranch(CollegeStudentTCReportDTO data)
        {
            try
            {
                if (data.AMB_Id == 0)
                {
                    data.getsemester = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from b in _clgadmctxt.MasterCourseDMO
                                        from c in _clgadmctxt.AcademicYear
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from e in _clgadmctxt.ClgMasterBranchDMO
                                        from f in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        from g in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                        && g.AMSE_Id == f.AMSE_Id && f.ACAYCB_Id == d.ACAYCB_Id && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && d.ACAYCB_ActiveFlag == true
                                        && e.AMB_ActiveFlag == true && g.AMSE_ActiveFlg == true && f.ACAYCBS_ActiveFlag == true)
                                        select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }
                else
                {

                    data.getsemester = (from a in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from b in _clgadmctxt.MasterCourseDMO
                                        from c in _clgadmctxt.AcademicYear
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from e in _clgadmctxt.ClgMasterBranchDMO
                                        from f in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        from g in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                        && g.AMSE_Id == f.AMSE_Id && f.ACAYCB_Id == d.ACAYCB_Id && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.AMCO_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id
                                        && d.ACAYCB_ActiveFlag == true && e.AMB_ActiveFlag == true && g.AMSE_ActiveFlg == true && f.ACAYCBS_ActiveFlag == true)
                                        select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudentTCReportDTO onchangesemester(CollegeStudentTCReportDTO data)
        {
            try
            {
                data.getsection = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudentTCReportDTO Getreportdetails(CollegeStudentTCReportDTO data)
        {
            try
            {
                string IVRM_CLM_coloumn = "";
                //string courseid = "";
                //string branchid = "";
                //string semid = "";
                //string secid = "";
                //string quotaid = "";
                //string gendername = "";

                for (int i = 0; i < data.TempararyArrayheadList.Length; i++)
                {
                    string Id = data.TempararyArrayheadList[i].columnID;

                    string name = Id;


                    if (name == "ACSTC_TCDate")
                    {
                        if (name == "ACSTC_TCDate")
                        {
                            if (name != "")
                            {
                                name = "(CONVERT(varchar(10), ACSTC_TCDate, 103)) ACSTC_TCDate";
                            }
                            else
                            {
                                name = "(CONVERT(varchar(10), ACSTC_TCDate, 103)) ACSTC_TCDate";
                            }
                        }
                    }

                    if (name == "AMCST_Date")
                    {
                        if (name == "AMCST_Date")
                        {
                            if (name != "")
                            {
                                name = "(CONVERT(varchar(10), AMCST_Date, 103)) AMCST_Date";
                            }
                            else
                            {
                                name = "(CONVERT(varchar(10), AMCST_Date, 103)) AMCST_Date";
                            }
                        }
                    }

                    if (name == "AMCST_DOB")
                    {
                        if (name == "AMCST_DOB")
                        {
                            if (name != "")
                            {
                                name = "(CONVERT(varchar(10), AMCST_DOB, 103)) AMCST_DOB";
                            }
                            else
                            {
                                name = "(CONVERT(varchar(10), AMCST_DOB, 103)) AMCST_DOB";
                            }
                        }
                    }

                    if (name == "AMCST_FirstName")
                    {
                        if (name == "AMCST_FirstName")
                        {
                            if (name != "")
                            {
                                name = "AMCST_FirstName = CASE WHEN  AMCST_FirstName is null or AMCST_FirstName=''  then '' else AMCST_FirstName end+CASE WHEN  AMCST_MiddleName is null or AMCST_MiddleName = '' or AMCST_MiddleName = '0' then ''  ELSE ' ' + AMCST_MiddleName END +  CASE WHEN AMCST_LastName is null or AMCST_LastName = '' or AMCST_LastName = '0' then ''  ELSE ' '  + AMCST_LastName END";
                            }
                            else
                            {
                                name = "AMCST_FirstName = CASE WHEN  AMCST_FirstName is null or AMCST_FirstName=''  then '' else AMCST_FirstName end+CASE WHEN  AMCST_MiddleName is null or AMCST_MiddleName = '' or AMCST_MiddleName = '0' then ''  ELSE ' ' + AMCST_MiddleName END +  CASE WHEN AMCST_LastName is null or AMCST_LastName = '' or AMCST_LastName = '0' then ''  ELSE ' '  + AMCST_LastName END";
                            }
                        }
                    }

                    if (name == "AMCST_PerAdd3")
                    {
                        if (name == "AMCST_PerAdd3")
                        {
                            if (name != "")
                            {
                                name = "AMCST_PerAdd3 = CASE WHEN  REPLACE(AMCST_PerStreet,',','') is null or REPLACE(AMCST_PerStreet,',','')='' then '' else REPLACE(AMCST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMCST_PerArea, ',', '') is null or REPLACE(AMCST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMCST_PerCity,',', '') is null or REPLACE(AMCST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMCST_perpincode as varchar(max)) is null or CAST(AMCST_perpincode as varchar(max))= '' or CAST(AMCST_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMCST_perpincode as varchar(max)) END ";
                            }
                            else
                            {
                                name = "AMCST_PerAdd3 = CASE WHEN  REPLACE(AMCST_PerStreet,',','') is null or REPLACE(AMCST_PerStreet,',','')='' then '' else REPLACE(AMCST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMCST_PerArea, ',', '') is null or REPLACE(AMCST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMCST_PerCity,',', '') is null or REPLACE(AMCST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMCST_perpincode as varchar(max)) is null or CAST(AMCST_perpincode as varchar(max))= '' or CAST(AMCST_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMCST_perpincode as varchar(max)) END ";
                            }
                        }
                    }

                    if (name == "AMCST_ConStreet")
                    {
                        if (name == "AMCST_ConStreet")
                        {
                            if (name != "")
                            {
                                name = "AMCST_ConStreet = CASE WHEN  REPLACE(AMCST_ConStreet,',','') is null or REPLACE(AMCST_ConStreet,',','')='' then '' else REPLACE(AMCST_ConStreet,',','') end+CASE WHEN  REPLACE(AMCST_ConArea, ',', '') is null or REPLACE(AMCST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMCST_ConCity,',', '') is null or REPLACE(AMCST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMCST_ConPincode as varchar(max)) is null or CAST(AMCST_ConPincode as varchar(max))= '' or CAST(AMCST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMCST_ConPincode as varchar(max)) END";
                            }
                            else
                            {
                                name = "AMCST_ConStreet = CASE WHEN  REPLACE(AMCST_ConStreet,',','') is null or REPLACE(AMCST_ConStreet,',','')='' then '' else REPLACE(AMCST_ConStreet,',','') end+CASE WHEN  REPLACE(AMCST_ConArea, ',', '') is null or REPLACE(AMCST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMCST_ConCity,',', '') is null or REPLACE(AMCST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMCST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMCST_ConPincode as varchar(max)) is null or CAST(AMCST_ConPincode as varchar(max))= '' or CAST(AMCST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMCST_ConPincode as varchar(max)) END";
                            }
                        }
                    }

                    if (name == "AMCST_mobileno")
                    {
                        if (name == "AMCST_mobileno")
                        {
                            if (name != "")
                            {
                                name = "(ISNULL(cast(AMCST_MobileNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_emailId,'')) as AMCST_mobileno";
                            }
                            else
                            {
                                name = "(ISNULL(cast(AMCST_MobileNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_emailId,'')) as AMCST_mobileno";
                            }
                        }
                    }

                    if (name == "AMCST_FatherMobleNo")
                    {
                        if (name == "AMCST_FatherMobleNo")
                        {
                            if (name != "")
                            {
                                name = "(ISNULL(cast(AMCST_FatherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')) as AMCST_FatherMobleNo";
                            }
                            else
                            {
                                name = "(ISNULL(cast(AMCST_FatherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')) as AMCST_FatherMobleNo";
                            }
                        }
                    }

                    if (name == "AMCST_MotherMobleNo")
                    {
                        if (name == "AMCST_MotherMobleNo")
                        {
                            if (name != "")
                            {
                                name = "(ISNULL(cast(AMCST_MotherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_MotheremailId,'')) as AMCST_MotherMobleNo";
                            }
                            else
                            {
                                name = "(ISNULL(cast(AMCST_MotherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_MotheremailId,'')) as AMCST_MotherMobleNo";
                            }
                        }
                    }

                    IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                }

                string column = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_TC_Report_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@column",
                       SqlDbType.NVarChar)
                    {
                        Value = column
                    });
                    cmd.Parameters.Add(new SqlParameter("@ALLORINDI",
                      SqlDbType.VarChar)
                    {
                        Value = data.allorindi
                    });
                    cmd.Parameters.Add(new SqlParameter("@PERORTEMP",
                      SqlDbType.VarChar)
                    {
                        Value = data.tcperortemp
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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

        // TC Custom Report
        public CollegeStudentTCReportDTO onchangeyeartc(CollegeStudentTCReportDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_TC_Custom_Report_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TEMPORPERTC",
                       SqlDbType.VarChar)
                    {
                        Value = data.tctemporper
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                     SqlDbType.VarChar)
                    {
                        Value = 1
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getstudent = retObject.ToArray();

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
        public CollegeStudentTCReportDTO stdnamechange(CollegeStudentTCReportDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_TC_Custom_Report_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TEMPORPERTC",
                       SqlDbType.VarChar)
                    {
                        Value = data.tctemporper
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                     SqlDbType.VarChar)
                    {
                        Value = 2
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getstudent = retObject.ToArray();

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
        public CollegeStudentTCReportDTO getTcdetails(CollegeStudentTCReportDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_TC_Custom_Report_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TEMPORPERTC",
                       SqlDbType.VarChar)
                    {
                        Value = data.tctemporper
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                     SqlDbType.VarChar)
                    {
                        Value = 3
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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

                data.academicList1 = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.MasterCompany = (from a in _clgadmctxt.Institution
                                      where (a.MI_Id == data.MI_Id)
                                      select new CollegeStudentTCReportDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          MI_Id = a.MI_Id,
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
