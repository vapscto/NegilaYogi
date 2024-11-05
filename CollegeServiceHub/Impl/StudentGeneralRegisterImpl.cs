
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
    public class StudentGeneralRegisterImpl : Interface.StudentGeneralRegisterInterface
    {

        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public StudentGeneralRegisterImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public StudentGeneralRegisterDTO getdetails(StudentGeneralRegisterDTO data)
        {
            try
            {

                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();

                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.quotalist = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQ_Id).ToArray();

                data.check_list = _clgadmctxt.IVRM_College_ReportDMO.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public StudentGeneralRegisterDTO onselectAcdYear(StudentGeneralRegisterDTO data)
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
        public StudentGeneralRegisterDTO onselectCourse(StudentGeneralRegisterDTO data)
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
        public StudentGeneralRegisterDTO onselectBranch(StudentGeneralRegisterDTO data)
        {
            try
            {
                if (data.AMB_Id > 0)
                {
                    var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                        from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                        from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
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
        public async Task<StudentGeneralRegisterDTO> onreport(StudentGeneralRegisterDTO data)
        {
            List<StudentGeneralRegisterDTO> AllInOne = new List<StudentGeneralRegisterDTO>();
            try
            {
                string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                string secid = "";
                string quotaid = "";
                string gendername = "";


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

                if (data.ACMS_Id == 0)
                {
                    var listsec = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).ToList();
                    for (int k = 0; k < listsec.Count(); k++)
                    {
                        if (k == 0)
                        {
                            secid = listsec[k].ACMS_Id.ToString();
                        }
                        else
                        {
                            secid = secid + ',' + listsec[k].ACMS_Id.ToString();
                        }
                    }
                }
                else
                {
                    secid = data.ACMS_Id.ToString();
                }


                if (data.ACQ_Id == 0)
                {
                    var listacq = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id && a.ACQ_ActiveFlg == true).ToList();
                    for (int l = 0; l < listacq.Count(); l++)
                    {
                        if (l == 0)
                        {
                            quotaid = listacq[l].ACQ_Id.ToString();
                        }
                        else
                        {
                            quotaid = quotaid + ',' + listacq[l].ACQ_Id.ToString();
                        }
                    }
                }
                else
                {
                    quotaid = data.ACQ_Id.ToString();
                }
                if (data.gender == "All")
                {
                    gendername = "Male,Female,Other";
                }
                else
                {
                    gendername = data.gender;
                }


                for (int i = 0; i < data.TempararyArrayListcoloumn.Length; i++)
                {
                    string Id = data.TempararyArrayListcoloumn[i].IVRM_CLG_PAR;

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

                    if (name == "AMCST_AadharNo")
                    {
                        if (name == "AMCST_AadharNo")
                        {
                            if (name != "")
                            {
                                name = "(CONVERT(varchar(50), AMCST_AadharNo)) AMCST_AadharNo";
                            }
                            else
                            {
                                name = "(CONVERT(varchar(50), AMCST_AadharNo)) AMCST_AadharNo";
                            }
                        }
                    }

                    IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                }

                string column = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_Register_Report_Modify";
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
                    cmd.Parameters.Add(new SqlParameter("@acms_id",
                       SqlDbType.VarChar)
                    {
                        Value = secid
                    });
                    cmd.Parameters.Add(new SqlParameter("@column",
                       SqlDbType.VarChar)
                    {
                        Value = column
                    });
                    cmd.Parameters.Add(new SqlParameter("@gender",
                       SqlDbType.VarChar)
                    {
                        Value = gendername
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACQ_Id",
                       SqlDbType.VarChar)
                    {
                        Value = quotaid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                    {
                        Value = data.Flag
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