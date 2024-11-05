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
    public class statewisestudentadmissionImpl : Interface.statewisestudentadmissionInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;
        public statewisestudentadmissionImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }
        public statewisestudentadmissionDTO getdetails(statewisestudentadmissionDTO data)
        {
            try
            {

                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();

                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                data.seclist = _clgadmctxt.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.statelist = _clgadmctxt.State.ToArray();

                data.countrylist = _clgadmctxt.Country.ToArray();

                data.religionlist = _clgadmctxt.Religion.ToArray();

                data.castecategory = _clgadmctxt.CasteCategory.ToArray();

                data.mastercaste = (from a in _clgadmctxt.CasteCategory
                                    from b in _clgadmctxt.Caste
                                    where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == data.MI_Id)
                                    select new statewisestudentadmissionDTO
                                    {
                                        IMC_Id = b.IMC_Id,
                                        IMC_CasteName = b.IMC_CasteName
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public statewisestudentadmissionDTO onselectAcdYear(statewisestudentadmissionDTO data)
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
        public statewisestudentadmissionDTO onselectCourse(statewisestudentadmissionDTO data)
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
        public statewisestudentadmissionDTO onselectBranch(statewisestudentadmissionDTO data)
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

        // State Wise Student Admission Report 
        public async Task<statewisestudentadmissionDTO> onreport(statewisestudentadmissionDTO data)
        {
            List<statewisestudentadmissionDTO> AllInOne = new List<statewisestudentadmissionDTO>();
            try
            {
                //string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                string secid = "";
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

                if (data.flag=="Course")
                {
                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Coursewise_Student_Admission_Details";
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
                            data.statestudentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else
                {

                    
                    using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "College_Statewise_Student_Admission_Details";
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

                        cmd.Parameters.Add(new SqlParameter("@stateid",
                            SqlDbType.VarChar)
                        {
                            Value = data.IVRMMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                            SqlDbType.VarChar)
                        {
                            Value = data.flag
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
                            data.statestudentlist = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }

             



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        // Country Wise Student Admission Report 
        public async Task<statewisestudentadmissionDTO> onreportcountry(statewisestudentadmissionDTO data)
        {
            List<statewisestudentadmissionDTO> AllInOne = new List<statewisestudentadmissionDTO>();
            try
            {
               //string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                string secid = "";
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

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Countrywise_Student_Admission_Details";
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

                    cmd.Parameters.Add(new SqlParameter("@countryid",
                        SqlDbType.VarChar)
                    {
                        Value = data.IVRMMC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                        SqlDbType.VarChar)
                    {
                        Value = data.flag
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
                        data.statestudentlist = retObject.ToArray();

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

        // Religion Rural Urban Student Admission Report 
        public async Task<statewisestudentadmissionDTO> onreportreligionruralurban(statewisestudentadmissionDTO data)
        {
            List<statewisestudentadmissionDTO> AllInOne = new List<statewisestudentadmissionDTO>();
            try
            {
                //string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                string secid = "";
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

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Religion_Urban_Rural_wise_Student_Admission_Details";
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

                    cmd.Parameters.Add(new SqlParameter("@religionid",
                        SqlDbType.VarChar)
                    {
                        Value = data.IVRMMR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                        SqlDbType.VarChar)
                    {
                        Value = data.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@ruralurban",
                      SqlDbType.VarChar)
                    {
                        Value = data.ruralurban
                    });
                    cmd.Parameters.Add(new SqlParameter("@flagreligionrural",
                      SqlDbType.VarChar)
                    {
                        Value = data.flagreligionrural
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
                        data.statestudentlist = retObject.ToArray();

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

        // Category Caste Student Admission Report 
        public async Task<statewisestudentadmissionDTO> CategoryCasteWiseStudentReport(statewisestudentadmissionDTO data)
        {
            List<statewisestudentadmissionDTO> AllInOne = new List<statewisestudentadmissionDTO>();
            try
            {
                //string IVRM_CLM_coloumn = "";
                string courseid = "";
                //string branchid = "";
                string semid = "";
                string secid = "";
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

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Category_Caste_Wise_Student_Admission_Report";
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

                    cmd.Parameters.Add(new SqlParameter("@imcc_id",
                        SqlDbType.VarChar)
                    {
                        Value = data.IMCC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@imc_id",
                        SqlDbType.VarChar)
                    {
                        Value = data.IMC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@categorycasteflag",
                      SqlDbType.VarChar)
                    {
                        Value = data.categorycasteflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@addresstype",
                      SqlDbType.VarChar)
                    {
                        Value = data.flag
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
                        data.statestudentlist = retObject.ToArray();

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

        // Birthday report
        public async Task<statewisestudentadmissionDTO> onreportbirthday(statewisestudentadmissionDTO data)
        {
            List<statewisestudentadmissionDTO> AllInOne = new List<statewisestudentadmissionDTO>();
            try
            {
                //string IVRM_CLM_coloumn = "";
                //string courseid = "";
                //string branchid = "";
                //string semid = "";
                //string secid = "";
                //string quotaid = "";
                //string gendername = "";

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                DateTime todatecon = DateTime.Now;
                string contodate = "";
                todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = todatecon.ToString("yyyy-MM-dd");

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_Birthday_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Monthid",
                       SqlDbType.VarChar)
                    {
                        Value = data.months
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag",
                        SqlDbType.VarChar)
                    {
                        Value = data.flag
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
                        data.statestudentlist = retObject.ToArray();

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
