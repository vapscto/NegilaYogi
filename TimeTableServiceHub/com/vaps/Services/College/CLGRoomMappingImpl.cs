using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace TimeTableServiceHub.com.vaps.Services
{
    public class CLGRoomMappingImpl : Interfaces.CLGRoomMappingInterface
    {

        private static ConcurrentDictionary<string, CLGRoomMappingDTO> _login =
               new ConcurrentDictionary<string, CLGRoomMappingDTO>();


        public TTContext _ttcategorycontext;
        public CLGRoomMappingImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public CLGRoomMappingDTO getdetails(CLGRoomMappingDTO data)
        {
           
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true).OrderByDescending(r=>r.ASMAY_Order).ToList().ToArray();
                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();


                data.subjectlist = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();

                data.roomlist = _ttcategorycontext.TT_Master_RoomDMO.Where(r => r.MI_Id == data.MI_Id && r.TTMRM_ActiveFlg == true).Distinct().OrderBy(r=>r.TTMRM_RoomName).ToArray();
                //data.staffDrpDwn = (from f in _ttcategorycontext.HR_Master_Employee_DMO
                //                    from TT_Master_Staff_AbbreviationDMO in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                //                    where (f.MI_Id.Equals(data.MI_Id) && f.HRME_ActiveFlag.Equals(true) && TT_Master_Staff_AbbreviationDMO.HRME_Id == f.HRME_Id && TT_Master_Staff_AbbreviationDMO.MI_Id == data.MI_Id)
                //                    select new TTStaffReplacementInUnallocatedPeriodDTO
                //                    {
                //                        HRME_Id = f.HRME_Id,
                //                        staffNamelst = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == "  " || f.HRME_EmployeeMiddleName == "0" ? "  " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? "  " : f.HRME_EmployeeLastName),
                //                    }
                //                ).Distinct().OrderBy(f=> f.staffNamelst).ToArray();

                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
               
    data.sectionlist = _ttcategorycontext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToList().Distinct().ToArray();

                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_ROOM_MAPPING";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.datalst = retObject.ToArray();
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

        public CLGRoomMappingDTO get_roomfacility(CLGRoomMappingDTO data)
        {
            try
            {
                data.facilitylist = (from a in _ttcategorycontext.TT_Master_FacilitiesDMO
                                     from b in _ttcategorycontext.TT_Master_Room_FacilitiesDMO
                                     where (a.MI_Id.Equals(data.MI_Id) && a.TTMFA_ActiveFlg.Equals(true) && a.TTMFA_Id==b.TTMFA_Id && b.TTMRMFA_ActiveFlg==true && b.TTMRM_Id==data.TTMRM_Id)
                                 select new CLGRoomMappingDTO
                                 {
                                     TTMRM_Id = b.TTMRM_Id,
                                     TTMFA_FacilityName = a.TTMFA_FacilityName,
                                     TTMFA_FacilityDesc = a.TTMFA_FacilityDesc,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }
        public CLGRoomMappingDTO get_catg(CLGRoomMappingDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new CLGRoomMappingDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        public CLGRoomMappingDTO deactiveY(CLGRoomMappingDTO data)
        {
            try
         {

                var editlist = _ttcategorycontext.CLGTT_Course_Subject_RoomDMO.Single(e => e.TTCSRMC_Id == data.TTCSRMC_Id);

                if (editlist.TTCSRMC_ActiveFlg==true)
                {
                    editlist.TTCSRMC_ActiveFlg = false;
                }
                else
                {
                    editlist.TTCSRMC_ActiveFlg = true;
                }
                editlist.TTCSRMC_UpdatedBy = data.User_Id;
                editlist.TTCSRMC_UpdatedDate = DateTime.Now;
                _ttcategorycontext.Update(editlist);
                int a = _ttcategorycontext.SaveChanges();
                if (a > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public CLGRoomMappingDTO editdata(CLGRoomMappingDTO data)
        {
            try
         {

                var editlist = _ttcategorycontext.CLGTT_Course_Subject_RoomDMO.Where(e => e.TTCSRMC_Id == data.TTCSRMC_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();

           var catlist=     (from a in _ttcategorycontext.CLGTT_Category_CourseBranchDMO
                 where a.ASMAY_Id==editlist[0].ASMAY_Id && a.AMCO_Id== editlist[0].AMCO_Id
                 && a.AMB_Id== editlist[0].AMB_Id select new CLGRoomMappingDTO {TTMC_Id=a.TTMC_Id }

                                                 ).Distinct().ToList();


                data.TTMC_Id = catlist[0].TTMC_Id;
                data.courselist = (from a in _ttcategorycontext.CLGTT_Category_CourseBranchDMO
                                   from b in _ttcategorycontext.MasterCourseDMO
                                   where b.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == editlist[0].ASMAY_Id && a.TTMC_Id == catlist[0].TTMC_Id && a.TTCC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                   select b
                                ).Distinct().ToArray();

                data.branchlist = (from a in _ttcategorycontext.CLG_Adm_College_AY_CourseDMO
                                   from b in _ttcategorycontext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _ttcategorycontext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == editlist[0].ASMAY_Id && a.AMCO_Id == editlist[0].AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
                data.semisterlist = (from a in _ttcategorycontext.CLG_Adm_Master_SemesterDMO
                                     from b in _ttcategorycontext.CLG_Adm_College_AY_CourseDMO
                                     from c in _ttcategorycontext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _ttcategorycontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == editlist[0].ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == editlist[0].AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == editlist[0].AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select a).Distinct().ToArray();



                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_ROOM_MAPPING_PERIOD_TT_EDIT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTCSRMC_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TTCSRMC_Id
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
                        data.perioddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public CLGRoomMappingDTO getdays(CLGRoomMappingDTO data)
        {
            try
            {
                data.daylist = (from a in _ttcategorycontext.TT_Master_DayDMO
                                   from b in _ttcategorycontext.CLGTT_Master_Day_CourseBranchDMO
                                   where a.TTMD_Id == b.TTMD_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && a.TTMD_ActiveFlag.Equals(true) && b.TTMDC_ActiveFlag == true
                                   select a
                               ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }     

        public CLGRoomMappingDTO getpossiblePeriod(CLGRoomMappingDTO data)
        {

            try
            {
                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_ROOM_MAPPING_PERIOD_TT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TTMD_Id
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
                        data.perioddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_ROOM_MAPPING_PERIOD_TT_COMAPARE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TTMD_Id
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
                        data.existingperioddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }



            


            return data;
        }

        public CLGRoomMappingDTO savedetail(CLGRoomMappingDTO data)
        {
            try
            {
                data.sscnt = 0;
                data.ffcnt = 0;
                if (data.TTCSRMC_Id>0)
                {

                    var checkexist = _ttcategorycontext.CLGTT_Course_Subject_RoomDMO.Single(f => f.TTCSRMC_Id == data.TTCSRMC_Id);
                    

                        if (data.savedata.Length > 0)
                        {

                            foreach (var item in data.savedata)
                            {
                                var dupcheck = _ttcategorycontext.CLGTT_Course_Subject_RoomDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == item.ASMAY_Id && r.AMCO_Id == item.AMCO_Id && r.AMB_Id == item.AMB_Id && r.AMSE_Id == item.AMSE_Id && r.ACMS_Id == item.ACMS_Id && r.ISMS_Id == item.ISMS_Id && r.TTMRM_Id == item.TTMRM_Id && r.TTCSRMC_Id !=data.TTCSRMC_Id && r.TTMD_Id==item.TTMD_Id && r.TTMP_Id == item.TTMP_Id).ToList();
                                if (dupcheck.Count > 0)
                                {
                                    data.ffcnt += 1;
                                }
                                else
                                {
                                    checkexist.MI_Id = data.MI_Id;
                                    checkexist.ASMAY_Id = item.ASMAY_Id;
                                    checkexist.AMCO_Id = item.AMCO_Id;
                                    checkexist.AMB_Id = item.AMB_Id;
                                    checkexist.AMSE_Id = item.AMSE_Id;
                                    checkexist.ACMS_Id = item.ACMS_Id;
                                    checkexist.ISMS_Id = item.ISMS_Id;
                                    checkexist.TTMD_Id = item.TTMD_Id;
                                    checkexist.TTMP_Id = item.TTMP_Id;
                                    checkexist.TTMRM_Id = item.TTMRM_Id;
                                    checkexist.TTCSRMC_ActiveFlg = true;
                                  
                                    checkexist.TTCSRMC_UpdatedBy = data.User_Id;
                                    
                                    checkexist.TTCSRMC_UpdatedDate = DateTime.Now;
                                    _ttcategorycontext.Update(checkexist);
                                    data.sscnt += 1;

                                }


                            }
                        }

                        int a = _ttcategorycontext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                  

                }
                else
                {

                    if (data.savedata.Length>0)
                    {
                        
                        foreach (var item in data.savedata)
                        {
                            var dupcheck = _ttcategorycontext.CLGTT_Course_Subject_RoomDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == item.ASMAY_Id && r.AMCO_Id == item.AMCO_Id && r.AMB_Id == item.AMB_Id && r.AMSE_Id == item.AMSE_Id && r.ACMS_Id == item.ACMS_Id && r.ISMS_Id == item.ISMS_Id && r.TTMRM_Id == item.TTMRM_Id && r.TTMD_Id==item.TTMD_Id && r.TTMP_Id==item.TTMP_Id).ToList();
                            if (dupcheck.Count>0)
                            {
                                data.ffcnt += 1;
                            }
                            else
                            {
                                CLGTT_Course_Subject_RoomDMO obj = new CLGTT_Course_Subject_RoomDMO();

                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = item.ASMAY_Id;
                                obj.AMCO_Id = item.AMCO_Id;
                                obj.AMB_Id = item.AMB_Id;
                                obj.AMSE_Id = item.AMSE_Id;
                                obj.ACMS_Id = item.ACMS_Id;
                                obj.ISMS_Id = item.ISMS_Id;
                                obj.TTMRM_Id = item.TTMRM_Id;
                                obj.TTMD_Id = item.TTMD_Id;
                                obj.TTMP_Id = item.TTMP_Id;
                                obj.TTCSRMC_ActiveFlg = true;
                                obj.TTCSRMC_CreatedBy = data.User_Id;
                                obj.TTCSRMC_UpdatedBy = data.User_Id;
                                obj.TTCSRMC_CreatedDate = DateTime.Now; 
                                obj.TTCSRMC_UpdatedDate = DateTime.Now;
                                _ttcategorycontext.Add(obj);
                                data.sscnt += 1;

                            }


                        }
                    }

                    int a = _ttcategorycontext.SaveChanges();
                    if (a>0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
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

