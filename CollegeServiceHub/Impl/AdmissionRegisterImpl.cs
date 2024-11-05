
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
    public class AdmissionRegisterImpl : Interface.AdmissionRegisterInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public AdmissionRegisterImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public AdmissionRegisterDTO getdetails(AdmissionRegisterDTO data)
        {
            try
            {

                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
                data.quotalist = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQ_Id).ToArray();
                data.check_list = _clgadmctxt.AdmissionRegisterDMO.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public AdmissionRegisterDTO onselectAcdYear(AdmissionRegisterDTO data)
        {
            try
            {

                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionRegisterDTO onselectCourse(AdmissionRegisterDTO data)
        {
            try
            {
                if (data.AMCO_Id == 0)
                {
                    data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag == true).OrderBy(a => a.AMB_Order).ToArray();
                }
                else
                {
                    var branchlist = (from a in _clgadmctxt.ClgMasterBranchDMO
                                      from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                      from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                      where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
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
                    data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmissionRegisterDTO onselectBranch(AdmissionRegisterDTO data)
        {
            try
            {
                if (data.AMB_Id > 0)
                {
                    //&& b.AMCO_Id == data.AMCO_Id
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag  && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
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
                    data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }
                else
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true)
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
                    data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<AdmissionRegisterDTO> onreport(AdmissionRegisterDTO data)
        {
            List<AdmissionRegisterDTO> AllInOne = new List<AdmissionRegisterDTO>();
            try
            {
                string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                //string secid = "";
                //string quotaid = "";
                //string gendername = "";


                if (data.AMCO_Id == 0)
                {
                    var list = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (i == 0)
                        {
                            courseid = list[i].AMCO_Id.ToString();
                        }
                        else
                        {
                            courseid = courseid + ',' + list[i].AMCO_Id.ToString();
                        }
                    }
                }
                else
                {
                    courseid = data.AMCO_Id.ToString();
                }


                if (data.AMSE_Id == 0)
                {
                    var listsem = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).ToList();
                    for (int j = 0; j < listsem.Count(); j++)
                    {
                        if (j == 0)
                        {
                            semid = listsem[j].AMSE_Id.ToString();
                        }
                        else
                        {
                            semid = semid + ',' + listsem[j].AMSE_Id.ToString();
                        }
                    }
                }
                else
                {
                    semid = data.AMSE_Id.ToString();
                }

                for (int i = 0; i < data.TempararyArrayListcoloumn.Length; i++)
                {
                    string Id = data.TempararyArrayListcoloumn[i].IVRM_CLGREG_PAR;

                    string name = Id;


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
                    if (name == "ACSTPS_PreviousTCNo")
                    {
                        if (name == "ACSTPS_PreviousTCNo")
                        {
                            if (name != "")
                            {
                                name = "(ACSTPS_PreviousTCNo +  ' & '  +convert(varchar(10),ACSTPS_PreviousTCDate,103) ) as ACSTPS_PreviousTCNo";
                            }
                            else
                            {
                                name = "(ACSTPS_PreviousTCNo + ' & '  +convert(varchar(10),ACSTPS_PreviousTCDate,103) ) as ACSTPS_PreviousTCNo";
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


                    if (name == "TCNO")
                    {
                        if (name == "TCNO")
                        {
                            if (name != "")
                            {
                                name = " '' as TCNO";
                            }
                            else
                            {
                                name = "'' as TCNO";
                            }
                        }
                    }

                    if (name == "ACSTPS_LeftYear")
                    {
                        if (name == "ACSTPS_LeftYear")
                        {
                            if (name != "")
                            {
                                name = "(select cls.ASMAY_Year from Adm_School_M_Academic_Year cls where cls.asmay_id=pr.ACSTPS_LeftYear)  as  ACSTPS_LeftYear";
                            }
                            else
                            {
                                name = "(select cls.ASMAY_Year from Adm_School_M_Academic_Year cls where cls.asmay_id=pr.ACSTPS_LeftYear)  as  ACSTPS_LeftYear";
                            }
                        }
                    }

                    if (name == "studentsignature")
                    {
                        if (name == "studentsignature")
                        {
                            if (name != "")
                            {
                                name = " '' as studentsignature";
                            }
                            else
                            {
                                name = "'' as studentsignature";
                            }
                        }
                    }
                    IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                }

                string column = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Register_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amco_id",
                       SqlDbType.VarChar)
                    {
                        Value = courseid
                    });
                    cmd.Parameters.Add(new SqlParameter("@amb_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amse_id",
                       SqlDbType.VarChar)
                    {
                        Value = semid
                    });
                    cmd.Parameters.Add(new SqlParameter("@column",
                       SqlDbType.VarChar)
                    {
                        Value = column
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.studentlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public async Task<AdmissionRegisterDTO> onreportnew(AdmissionRegisterDTO data)
        {
            List<AdmissionRegisterDTO> AllInOne = new List<AdmissionRegisterDTO>();
            try
            {
                string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                //string secid = "";
                //string quotaid = "";
                //string gendername = "";


                if (data.AMCO_Id == 0)
                {
                    var list = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (i == 0)
                        {
                            courseid = list[i].AMCO_Id.ToString();
                        }
                        else
                        {
                            courseid = courseid + ',' + list[i].AMCO_Id.ToString();
                        }
                    }
                }
                else
                {
                    courseid = data.AMCO_Id.ToString();
                }


                if (data.AMSE_Id == 0)
                {
                    var listsem = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true).ToList();
                    for (int j = 0; j < listsem.Count(); j++)
                    {
                        if (j == 0)
                        {
                            semid = listsem[j].AMSE_Id.ToString();
                        }
                        else
                        {
                            semid = semid + ',' + listsem[j].AMSE_Id.ToString();
                        }
                    }
                }
                else
                {
                    semid = data.AMSE_Id.ToString();
                }

                for (int i = 0; i < data.TempararyArrayListcoloumn.Length; i++)
                {
                    string Id = data.TempararyArrayListcoloumn[i].IVRM_CLGREG_PAR;

                    string name = Id;


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
                    if (name == "ACSTPS_PreviousTCNo")
                    {
                        if (name == "ACSTPS_PreviousTCNo")
                        {
                            if (name != "")
                            {
                                name = "(ACSTPS_PreviousTCNo +  ' & '  +convert(varchar(10),ACSTPS_PreviousTCDate,103) ) as ACSTPS_PreviousTCNo";
                            }
                            else
                            {
                                name = "(ACSTPS_PreviousTCNo + ' & '  +convert(varchar(10),ACSTPS_PreviousTCDate,103) ) as ACSTPS_PreviousTCNo";
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

                    if (name == "IMC_CasteName")
                    {
                        if (name == "IMC_CasteName")
                        {
                            if (name != "")
                            {
                                name = "( isnull(IMC_CasteName,'') +  ' & ' + isnull(IMCC_CategoryName,'')) as IMC_CasteName";
                            }
                            else
                            {
                                name = "( isnull(IMC_CasteName,'') +  ' & ' + isnull(IMCC_CategoryName,'')) as IMC_CasteName";
                            }
                        }
                    }

                    if (name == "AMCST_mobileno")
                    {
                        if (name == "AMCST_mobileno")
                        {
                            if (name != "")
                            {
                                name = "case when len((ISNULL(cast(AMCST_MobileNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')))= 3 or " +
                                    "len((ISNULL(cast(AMCST_MobileNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_emailId, ''))) = 4 then '' else " +
                                    "(ISNULL(cast(AMCST_MobileNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_emailId, '')) end AMCST_mobileno";
                            }
                            else
                            {
                                name = "case when len((ISNULL(cast(AMCST_MobileNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')))= 3 or " +
                                    "len((ISNULL(cast(AMCST_MobileNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_emailId, ''))) = 4 then '' else " +
                                    "(ISNULL(cast(AMCST_MobileNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_emailId, '')) end AMCST_mobileno";
                            }
                        }
                    }

                    if (name == "AMCST_FatherMobleNo")
                    {
                        if (name == "AMCST_FatherMobleNo")
                        {
                            if (name != "")
                            {
                                name = "case when len((ISNULL(cast(AMCST_FatherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')))= 3 or " +
                                    "len((ISNULL(cast(AMCST_FatherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_FatheremailId, ''))) = 4 then '' else " +
                                    "(ISNULL(cast(AMCST_FatherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_FatheremailId, '')) end AMCST_FatherMobleNo";
                            }
                            else
                            {
                                name = "case when len((ISNULL(cast(AMCST_FatherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')))= 3 or " +
                                    "len((ISNULL(cast(AMCST_FatherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_FatheremailId, ''))) = 4 then '' else " +
                                    "(ISNULL(cast(AMCST_FatherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_FatheremailId, '')) end AMCST_FatherMobleNo";
                            }
                        }
                    }

                    if (name == "AMCST_MotherMobleNo")
                    {
                        if (name == "AMCST_MotherMobleNo")
                        {
                            if (name != "")
                            {
                                //name = "(ISNULL(cast(AMCST_MotherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_MotheremailId,'')) as AMCST_MotherMobleNo";
                                name = "case when len((ISNULL(cast(AMCST_MotherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')))= 3 or " +
                                    "len((ISNULL(cast(AMCST_MotherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_MotheremailId, ''))) = 4 then '' else " +
                                    "(ISNULL(cast(AMCST_MotherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_MotheremailId, '')) end AMCST_MotherMobleNo";
                            }
                            else
                            {
                                name = "case when len((ISNULL(cast(AMCST_MotherMobleNo as varchar(max)),'') +   '  &  ' + ISNULL(AMCST_FatheremailId,'')))= 3 or " +
                                    "len((ISNULL(cast(AMCST_MotherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_MotheremailId, ''))) = 4 then '' else " +
                                    "(ISNULL(cast(AMCST_MotherMobleNo as varchar(max)), '') + '  &  ' + ISNULL(AMCST_MotheremailId, '')) end AMCST_MotherMobleNo";
                            }
                        }
                    }

                    if (name == "ACSTPS_LeftYear")
                    {
                        if (name == "ACSTPS_LeftYear")
                        {
                            if (name != "")
                            {
                                name = "(select cls.ASMAY_Year from Adm_School_M_Academic_Year cls where cls.asmay_id=pr.ACSTPS_LeftYear)  as  ACSTPS_LeftYear";
                            }
                            else
                            {
                                name = "(select cls.ASMAY_Year from Adm_School_M_Academic_Year cls where cls.asmay_id=pr.ACSTPS_LeftYear)  as  ACSTPS_LeftYear";
                            }
                        }
                    }

                    if (name == "TCNO")
                    {
                        if (name == "TCNO")
                        {
                            if (name != "")
                            {
                                name = " '' as TCNO";
                            }
                            else
                            {
                                name = "'' as TCNO";
                            }
                        }
                    }
                    if (name == "studentsignature")
                    {
                        if (name == "studentsignature")
                        {
                            if (name != "")
                            {
                                name = " '' as studentsignature";
                            }
                            else
                            {
                                name = "'' as studentsignature";
                            }
                        }
                    }
                    IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                }

                string column = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Register_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amco_id",
                       SqlDbType.VarChar)
                    {
                        Value = courseid
                    });
                    cmd.Parameters.Add(new SqlParameter("@amb_id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amse_id",
                       SqlDbType.VarChar)
                    {
                        Value = semid
                    });
                    cmd.Parameters.Add(new SqlParameter("@column",
                       SqlDbType.VarChar)
                    {
                        Value = column
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.studentlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
    }
}
