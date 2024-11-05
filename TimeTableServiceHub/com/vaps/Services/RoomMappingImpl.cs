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
    public class RoomMappingImpl : Interfaces.RoomMappingInterface
    {

        private static ConcurrentDictionary<string, RoomMappingDTO> _login =
               new ConcurrentDictionary<string, RoomMappingDTO>();


        public TTContext _ttcategorycontext;
        public RoomMappingImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public RoomMappingDTO getdetails(RoomMappingDTO data)
        {
           
            try
            {
                data.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true).OrderByDescending(r=>r.ASMAY_Order).ToList().ToArray();
                data.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();


                data.subjectlist = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(e => e.ISMS_SubjectName).ToArray();

                data.roomlist = _ttcategorycontext.TT_Master_RoomDMO.Where(r => r.MI_Id == data.MI_Id && r.TTMRM_ActiveFlg == true).Distinct().OrderBy(r=>r.TTMRM_RoomName).ToArray();
               

                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
                data.daylist = _ttcategorycontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
               
    data.sectionlist = _ttcategorycontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList().Distinct().ToArray();

                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_ROOM_MAPPING";
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

        public RoomMappingDTO get_catg(RoomMappingDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new RoomMappingDTO
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

        public RoomMappingDTO deactiveY(RoomMappingDTO data)
        {
            try
         {

                var editlist = _ttcategorycontext.TT_Class_Subject_RoomDMO.Single(e => e.TTCSRM_Id == data.TTCSRM_Id);

                if (editlist.TTCSRM_ActiveFlg==true)
                {
                    editlist.TTCSRM_ActiveFlg = false;
                }
                else
                {
                    editlist.TTCSRM_ActiveFlg = true;
                }
                editlist.TTCSRM_UpdatedBy = data.User_Id;
                editlist.TTCSRM_UpdatedDate = DateTime.Now;
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
        public RoomMappingDTO editdata(RoomMappingDTO data)
        {
            try
         {

                var editlist = _ttcategorycontext.TT_Class_Subject_RoomDMO.Where(e => e.TTCSRM_Id == data.TTCSRM_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();

           var catlist=     (from a in _ttcategorycontext.TT_Category_Class_DMO
                 where a.ASMAY_Id==editlist[0].ASMAY_Id && a.ASMCL_Id== editlist[0].ASMCL_Id && a.TTCC_ActiveFlag==true
                  select new RoomMappingDTO {TTMC_Id=a.TTMC_Id }

                                                 ).Distinct().ToList();


                data.TTMC_Id = catlist[0].TTMC_Id;
                data.courselist = (from a in _ttcategorycontext.TT_Category_Class_DMO
                                   from b in _ttcategorycontext.School_M_Class
                                   where b.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == editlist[0].ASMAY_Id && a.TTMC_Id == catlist[0].TTMC_Id && a.TTCC_ActiveFlag == true && b.ASMCL_ActiveFlag == true
                                   select b
                                ).Distinct().ToArray();


                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_ROOM_MAPPING_PERIOD_TT_EDIT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTCSRM_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.TTCSRM_Id
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


        public RoomMappingDTO getreport(RoomMappingDTO data)
        {
            try
            {


              

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }     

        public RoomMappingDTO getpossiblePeriod(RoomMappingDTO data)
        {
            try
            {
                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_ROOM_MAPPING_PERIOD_TT";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
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
                    cmd.CommandText = "TT_SCHOOL_GET_ROOM_MAPPING_PERIOD_TT_COMAPARE";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
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

        public RoomMappingDTO savedetail(RoomMappingDTO data)
        {
            try
            {
                data.sscnt = 0;
                data.ffcnt = 0;
                if (data.TTCSRM_Id>0)
                {

                    var checkexist = _ttcategorycontext.TT_Class_Subject_RoomDMO.Single(f => f.TTCSRM_Id == data.TTCSRM_Id);
                    
                       
                        if (data.savedata.Length > 0)
                        {

                            foreach (var item in data.savedata)
                            {
                                var dupcheck = _ttcategorycontext.TT_Class_Subject_RoomDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == item.ASMAY_Id && r.ASMCL_Id == item.ASMCL_Id  && r.ASMS_Id == item.ASMS_Id && r.ISMS_Id == item.ISMS_Id && r.TTMRM_Id == item.TTMRM_Id && r.TTCSRM_Id !=data.TTCSRM_Id && r.TTMP_Id==item.TTMP_Id && r.TTMD_Id==item.TTMD_Id).ToList();
                                if (dupcheck.Count > 0)
                                {
                                    data.ffcnt += 1;
                                }
                                else
                                {
                                    checkexist.MI_Id = data.MI_Id;
                                    checkexist.ASMAY_Id = item.ASMAY_Id;
                                    checkexist.ASMCL_Id = item.ASMCL_Id;
                                    checkexist.ASMS_Id = item.ASMS_Id;
                                    checkexist.ISMS_Id = item.ISMS_Id;
                                    checkexist.TTMRM_Id = item.TTMRM_Id;
                                    checkexist.TTMD_Id = item.TTMD_Id;
                                    checkexist.TTMP_Id = item.TTMP_Id;
                                    checkexist.TTCSRM_ActiveFlg = true;
                                    checkexist.TTCSRM_UpdatedBy = data.User_Id;
                                checkexist.TTCSRM_UpdatedDate = DateTime.Now;
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
                            var dupcheck = _ttcategorycontext.TT_Class_Subject_RoomDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == item.ASMAY_Id && r.ASMCL_Id == item.ASMCL_Id &&   r.ASMS_Id == item.ASMS_Id && r.ISMS_Id == item.ISMS_Id && r.TTMRM_Id == item.TTMRM_Id && r.TTMP_Id == item.TTMP_Id && r.TTMD_Id == item.TTMD_Id).ToList();
                            if (dupcheck.Count>0)
                            {
                                data.ffcnt += 1;
                            }
                            else
                            {
                                TT_Class_Subject_RoomDMO obj = new TT_Class_Subject_RoomDMO();

                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = item.ASMAY_Id;
                                obj.ASMCL_Id = item.ASMCL_Id;
                                obj.ASMS_Id = item.ASMS_Id;
                                obj.ISMS_Id = item.ISMS_Id;
                                obj.TTMD_Id = item.TTMD_Id;
                                obj.TTMP_Id = item.TTMP_Id;
                                obj.TTMRM_Id = item.TTMRM_Id;
                                obj.TTCSRM_ActiveFlg = true;
                                obj.TTCSRM_CreatedBy = data.User_Id;
                                obj.TTCSRM_UpdatedBy = data.User_Id;
                                obj.TTCSRM_CreatedDate = DateTime.Now; 
                                obj.TTCSRM_UpdatedDate = DateTime.Now;
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

