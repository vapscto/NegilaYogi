using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterLibraryImpl : Interfaces.MasterLibraryInterface
    {

        public LibraryContext _LibraryContext;
        public MasterLibraryImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }



        public async Task<LIB_Master_Library_DTO> getdetails(LIB_Master_Library_DTO data)
        {

            try
            {

                string COL_SCHFLG = "";
                var SCHCOLFLAG = (from a in _LibraryContext.Institute
                                  where a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1
                                  select new CirculationParameterDTO
                                  {
                                      MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                  }).Distinct().ToList();
                if (SCHCOLFLAG.Count > 0)
                {
                    COL_SCHFLG = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                    data.MI_SchoolCollegeFlag = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                }

                #region get all data for grid(table)

                data.librylist = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAL_ActiveFlag == true).Distinct().OrderBy(t => t.LMAL_Id).ToArray();

                data.liballdata = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(t => t.LMAL_Id).ToArray();

                data.classlist = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();


                data.role = _LibraryContext.IVRM_Role_Type.Distinct().ToArray();

                if (data.MI_SchoolCollegeFlag == "S") //For School Only
                {
                    data.alldata = (from b in _LibraryContext.LIB_Master_Library_DMO
                                    from c in _LibraryContext.LIB_User_Library_DMO
                                    from apr in _LibraryContext.ApplicationUserRole
                                    from apu in _LibraryContext.ApplicationUser
                                    from d in _LibraryContext.LIB_Library_Class_DMO
                                    where (b.MI_Id == c.MI_Id && c.MI_Id == d.MI_Id && b.LMAL_Id == c.LMAL_Id && b.LMAL_Id == d.LMAL_Id && apr.UserId == apu.Id && apu.Id == c.IVRMUL_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new LIB_Master_Library_DTO
                                    {
                                        LMAL_Id = b.LMAL_Id,
                                        LUL_Id = c.LUL_Id,
                                        LMAL_LibraryName = b.LMAL_LibraryName,
                                        LMAL_ActiveFlag = b.LMAL_ActiveFlag,
                                        HRME_EmployeeFirstName = apu.UserName,
                                        IVRMUL_Id = c.IVRMUL_Id,
                                        LUL_ActiveFlg = c.LUL_ActiveFlg,

                                    }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();
                }
                else if (data.MI_SchoolCollegeFlag == "C")//For College Only
                {
                    data.alldata = (from b in _LibraryContext.LIB_Master_Library_DMO
                                    from c in _LibraryContext.LIB_User_Library_DMO
                                    from apr in _LibraryContext.ApplicationUserRole
                                    from apu in _LibraryContext.ApplicationUser
                                    where (b.MI_Id == c.MI_Id && b.LMAL_Id == c.LMAL_Id && apr.UserId == apu.Id && apu.Id == c.IVRMUL_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                    select new LIB_Master_Library_DTO
                                    {
                                        LMAL_Id = b.LMAL_Id,
                                        LUL_Id = c.LUL_Id,
                                        LMAL_LibraryName = b.LMAL_LibraryName,
                                        LMAL_ActiveFlag = b.LMAL_ActiveFlag,
                                        HRME_EmployeeFirstName = apu.UserName,
                                        IVRMUL_Id = c.IVRMUL_Id,
                                        LUL_ActiveFlg = c.LUL_ActiveFlg,

                                    }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();
                }




                var clascount = _LibraryContext.LIB_Library_Class_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                if (clascount.Count > 0)
                {
                    data.mappedclass = (from a in _LibraryContext.LIB_Master_Library_DMO
                                        from b in _LibraryContext.LIB_Library_Class_DMO
                                        where (a.MI_Id == b.MI_Id && a.LMAL_Id == b.LMAL_Id && a.MI_Id == data.MI_Id && a.LMAL_ActiveFlag == true)
                                        select new LIB_Master_Library_DTO { LMAL_Id = a.LMAL_Id, LMAL_LibraryName = a.LMAL_LibraryName }).Distinct().ToArray();
                }


                #endregion



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO Savedata(LIB_Master_Library_DTO data)
        {
            try
            {
                if (data.LMAL_Id > 0)
                {
                    var Duplicate = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAL_Id != data.LMAL_Id && t.LMAL_LibraryName == data.LMAL_LibraryName).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.LIB_Master_Library_DMO.Single(t => t.LMAL_Id == data.LMAL_Id && t.MI_Id == data.MI_Id);

                        update.LMAL_LibraryName = data.LMAL_LibraryName;
                        update.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update);

                        //var update2 = _LibraryContext.LIB_User_Library_DMO.Where(t => t.LMAL_Id == data.LMAL_Id).SingleOrDefault();

                        //update2.IVRMUL_Id = data.IVRMUL_Id;
                        //update2.LMAL_Id = update.LMAL_Id;
                        //update2.UpdatedDate = DateTime.Now;
                        //_LibraryContext.Update(update2);


                        int rowAffected = _LibraryContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            data.liballdata = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(t => t.LMAL_Id).ToArray();
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var Duplicate = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAL_LibraryName == data.LMAL_LibraryName).ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        LIB_Master_Library_DMO Obj = new LIB_Master_Library_DMO();

                        Obj.MI_Id = data.MI_Id;
                        Obj.LMAL_LibraryName = data.LMAL_LibraryName;
                        Obj.LMAL_ActiveFlag = true;
                        Obj.CreatedDate = DateTime.Now;
                        Obj.UpdatedDate = DateTime.Now;

                        _LibraryContext.Add(Obj);

                        //LIB_User_Library_DMO obj2 = new LIB_User_Library_DMO();

                        //    obj2.MI_Id = data.MI_Id;
                        //    obj2.LMAL_Id = Obj.LMAL_Id;
                        //    obj2.IVRMUL_Id = data.IVRMUL_Id;
                        //    obj2.LUL_ActiveFlg = true;
                        //    obj2.CreatedDate = DateTime.Now;
                        //    obj2.UpdatedDate = DateTime.Now;

                        //    _LibraryContext.Add(obj2);

                        int rowAffected = _LibraryContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            data.liballdata = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(t => t.LMAL_Id).ToArray();
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO deactiveY(LIB_Master_Library_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_Master_Library_DMO.Single(t => t.MI_Id == data.MI_Id && t.LMAL_Id == data.LMAL_Id);

                if (result.LMAL_ActiveFlag == true)
                {
                    result.LMAL_ActiveFlag = false;
                }
                else if (result.LMAL_ActiveFlag == false)
                {
                    result.LMAL_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
                int rowAffected = _LibraryContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO saveclassdata(LIB_Master_Library_DTO data)
        {
            try
            {
                if (data.LMAL_Id > 0)
                {
                    List<long> amstids = new List<long>();
                    if (data.classlst.Count() > 0)
                    {
                        foreach (var it in data.classlst)
                        {
                            amstids.Add(it.ASMCL_Id);
                        }
                    }


                    var resultclass = _LibraryContext.LIB_Library_Class_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAL_Id == data.LMAL_Id).ToList();
                    if (resultclass.Count > 0)
                    {
                        foreach (var item in resultclass)
                        {
                            _LibraryContext.Remove(item);
                        }
                    }

                    foreach (var ss in data.classlst)
                    {
                        LIB_Library_Class_DMO obj2 = new LIB_Library_Class_DMO();
                        //obj2.LLC_Id = data.LLC_Id;
                        obj2.MI_Id = data.MI_Id;
                        obj2.ASMCL_Id = ss.ASMCL_Id;
                        obj2.UpdatedDate = DateTime.Now;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.LLC_ActiveFlg = true;
                        obj2.LMAL_Id = data.LMAL_Id;

                        _LibraryContext.Add(obj2);
                    }


                    int rowAffected = _LibraryContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.returnval = true;

                        var clascount = _LibraryContext.LIB_Library_Class_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                        if (clascount.Count > 0)
                        {
                            data.mappedclass = (from a in _LibraryContext.LIB_Master_Library_DMO
                                                from b in _LibraryContext.LIB_Library_Class_DMO
                                                where (a.MI_Id == b.MI_Id && a.LMAL_Id == b.LMAL_Id && a.MI_Id == data.MI_Id && a.LMAL_ActiveFlag == true)
                                                select new LIB_Master_Library_DTO { LMAL_Id = a.LMAL_Id, LMAL_LibraryName = a.LMAL_LibraryName }).Distinct().ToArray();
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO deactiveYstf(LIB_Master_Library_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_User_Library_DMO.Single(t => t.MI_Id == data.MI_Id && t.LUL_Id == data.LUL_Id);

                if (result.LUL_ActiveFlg == true)
                {
                    result.LUL_ActiveFlg = false;
                }
                else if (result.LUL_ActiveFlg == false)
                {
                    result.LUL_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
                int rowAffected = _LibraryContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO EditstaffData(LIB_Master_Library_DTO data)
        {
            try
            {
                var edit = (from a in _LibraryContext.LIB_Master_Library_DMO
                            from b in _LibraryContext.LIB_User_Library_DMO
                                //from c in _LibraryContext.LIB_Library_Class_DMO
                            from d in _LibraryContext.ApplicationUserRole
                            from e in _LibraryContext.IVRM_Role_Type
                            where (a.MI_Id == b.MI_Id /*&& a.MI_Id == c.MI_Id*/ && a.LMAL_Id == b.LMAL_Id /*&& a.LMAL_Id == c.LMAL_Id*/ && b.MI_Id == data.MI_Id && a.LMAL_Id == data.LMAL_Id && d.RoleTypeId == e.IVRMRT_Id && b.IVRMUL_Id == d.UserId && b.LUL_Id == data.LUL_Id)
                            select new LIB_Master_Library_DTO
                            {
                                LMAL_Id = a.LMAL_Id,
                                LUL_Id = b.LUL_Id,
                                //LLC_Id = c.LLC_Id,
                                //ASMCL_Id =c.ASMCL_Id,
                                IVRMUL_Id = b.IVRMUL_Id,
                                IVRMRT_Id = d.RoleTypeId,
                            }).Distinct().ToList();
                if (edit.Count > 0)
                {
                    data.editlist = edit.Distinct().ToArray();
                }
                data.classlist = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<LIB_Master_Library_DTO> modalclsslst(LIB_Master_Library_DTO data)
        {
            try
            {

                #region for gettting Library staff with class details
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_staffANDClass";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.IVRMUL_Id
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
                        data.clssslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO deactivclsdata(LIB_Master_Library_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_Library_Class_DMO.Single(t => t.MI_Id == data.MI_Id && t.LLC_Id == data.LLC_Id);

                if (result.LLC_ActiveFlg == true)
                {
                    result.LLC_ActiveFlg = false;
                }
                else if (result.LLC_ActiveFlg == false)
                {
                    result.LLC_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
                int rowAffected = _LibraryContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<LIB_Master_Library_DTO> getusername(LIB_Master_Library_DTO data)
        {
            try
            {
                //data.usnam = (from a in _LibraryContext.ApplicationUserRole
                //              from b in _LibraryContext.IVRM_Role_Type
                //              from c in _LibraryContext.ApplicationUser
                //              from d in _LibraryContext.UserRoleWithInstituteDMO
                //              where (c.Id == d.Id && a.RoleTypeId == b.IVRMRT_Id && b.IVRMRT_Id == data.IVRMRT_Id && a.UserId == c.Id && d.MI_Id == data.MI_Id)
                //              select new LIB_Master_Library_DTO
                //              {
                //                  UserId = a.UserId,
                //                  NormalizedUserName = c.NormalizedUserName,
                //                 UserName=c.UserName,
                //              }).ToArray();

                #region for gettting Library staff details
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_Staff_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.IVRMRT_Id
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
                        data.stafflist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public LIB_Master_Library_DTO check_userclass(LIB_Master_Library_DTO data)
        {
            try
            {
                //var lib_id = (from a in _LibraryContext.LIB_User_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.IVRMUL_Id == data.IVRMUL_Id) select a.LMAL_Id).SingleOrDefault();

                data.clsdata = (from a in _LibraryContext.LIB_Master_Library_DMO
                                from b in _LibraryContext.LIB_User_Library_DMO
                                from c in _LibraryContext.LIB_Library_Class_DMO
                                where (a.LMAL_Id == b.LMAL_Id && b.LMAL_Id == c.LMAL_Id && b.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == data.LMAL_Id && b.LUL_ActiveFlg == true)
                                select new LIB_Master_Library_DTO
                                {
                                    ASMCL_Id = c.ASMCL_Id,
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO EditclassData(LIB_Master_Library_DTO data)
        {
            try
            {
                var edit = (from a in _LibraryContext.LIB_Master_Library_DMO
                            from b in _LibraryContext.LIB_Library_Class_DMO
                            where (a.MI_Id == b.MI_Id && a.LMAL_Id == b.LMAL_Id && b.MI_Id == data.MI_Id && a.LMAL_Id == data.LMAL_Id)
                            select new LIB_Master_Library_DTO
                            {
                                LMAL_Id = a.LMAL_Id,
                                LLC_Id = b.LLC_Id,
                                ASMCL_Id = b.ASMCL_Id,

                            }).Distinct().ToList();
                if (edit.Count > 0)
                {
                    data.editlist = edit.Distinct().ToArray();
                }
                data.classlist = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LIB_Master_Library_DTO get_MappedClasslist(LIB_Master_Library_DTO data)
        {
            try
            {
                data.listclassdetails = (from a in _LibraryContext.LIB_Library_Class_DMO
                                         from b in _LibraryContext.LIB_Master_Library_DMO
                                         from c in _LibraryContext.Adm_School_M_ClassDMO
                                         where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.LMAL_Id == b.LMAL_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id && b.LMAL_Id == data.LMAL_Id)
                                         select new LIB_Master_Library_DTO
                                         {
                                             LMAL_Id = b.LMAL_Id,
                                             LMAL_LibraryName = b.LMAL_LibraryName,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             LLC_Id = a.LLC_Id,
                                             LLC_ActiveFlg = a.LLC_ActiveFlg,
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public LIB_Master_Library_DTO savestaffdata(LIB_Master_Library_DTO data)
        {
            try
            {
                string COL_SCHFLG = "";
                var SCHCOLFLAG = (from a in _LibraryContext.Institute
                                  where a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1
                                  select new CirculationParameterDTO
                                  {
                                      MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                  }).Distinct().ToList();
                if (SCHCOLFLAG.Count > 0)
                {
                    COL_SCHFLG = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                    data.MI_SchoolCollegeFlag = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                }
                if (data.LUL_Id == 0)
                {

                    var resultclass = _LibraryContext.LIB_User_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAL_Id == data.LMAL_Id && t.IVRMUL_Id == data.IVRMUL_Id).ToList();
                  
                    if (resultclass.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        LIB_User_Library_DMO obj2 = new LIB_User_Library_DMO();

                        obj2.MI_Id = data.MI_Id;
                        obj2.IVRMUL_Id = data.IVRMUL_Id;
                        obj2.UpdatedDate = DateTime.Now;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.LUL_ActiveFlg = true;
                        obj2.LMAL_Id = data.LMAL_Id;

                        _LibraryContext.Add(obj2);



                        int rowAffected = _LibraryContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;

                            if (data.MI_SchoolCollegeFlag == "S") //For School Only
                            {
                                data.alldata = (from b in _LibraryContext.LIB_Master_Library_DMO
                                                from c in _LibraryContext.LIB_User_Library_DMO
                                                from apr in _LibraryContext.ApplicationUserRole
                                                from apu in _LibraryContext.ApplicationUser
                                                from d in _LibraryContext.LIB_Library_Class_DMO
                                                where (b.MI_Id == c.MI_Id && c.MI_Id == d.MI_Id && b.LMAL_Id == c.LMAL_Id && b.LMAL_Id == d.LMAL_Id && apr.UserId == apu.Id && apu.Id == c.IVRMUL_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                                select new LIB_Master_Library_DTO
                                                {
                                                    LMAL_Id = b.LMAL_Id,
                                                    LUL_Id = c.LUL_Id,
                                                    LMAL_LibraryName = b.LMAL_LibraryName,
                                                    LMAL_ActiveFlag = b.LMAL_ActiveFlag,
                                                    HRME_EmployeeFirstName = apu.UserName,
                                                    IVRMUL_Id = c.IVRMUL_Id,
                                                    LUL_ActiveFlg = c.LUL_ActiveFlg,

                                                }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();
                            }
                            else if (data.MI_SchoolCollegeFlag == "C")//For College Only
                            {
                                data.alldata = (from b in _LibraryContext.LIB_Master_Library_DMO
                                                from c in _LibraryContext.LIB_User_Library_DMO
                                                from apr in _LibraryContext.ApplicationUserRole
                                                from apu in _LibraryContext.ApplicationUser
                                                where (b.MI_Id == c.MI_Id && b.LMAL_Id == c.LMAL_Id && apr.UserId == apu.Id && apu.Id == c.IVRMUL_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                                select new LIB_Master_Library_DTO
                                                {
                                                    LMAL_Id = b.LMAL_Id,
                                                    LUL_Id = c.LUL_Id,
                                                    LMAL_LibraryName = b.LMAL_LibraryName,
                                                    LMAL_ActiveFlag = b.LMAL_ActiveFlag,
                                                    HRME_EmployeeFirstName = apu.UserName,
                                                    IVRMUL_Id = c.IVRMUL_Id,
                                                    LUL_ActiveFlg = c.LUL_ActiveFlg,

                                                }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }


                }
                else if (data.LUL_Id > 0)
                {
                    var Duplicate = _LibraryContext.LIB_User_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LUL_Id != data.LUL_Id && t.LMAL_Id == data.LMAL_Id && t.IVRMUL_Id == data.IVRMUL_Id).ToList();

                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var resultclass = _LibraryContext.LIB_User_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LUL_Id == data.LUL_Id).SingleOrDefault();
                        resultclass.IVRMUL_Id = data.IVRMUL_Id;
                        resultclass.UpdatedDate = DateTime.Now;
                        resultclass.LMAL_Id = data.LMAL_Id;
                        _LibraryContext.Update(resultclass);

                        int rowAffected = _LibraryContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;

                            if (data.MI_SchoolCollegeFlag == "S") //For School Only
                            {
                                data.alldata = (from b in _LibraryContext.LIB_Master_Library_DMO
                                                from c in _LibraryContext.LIB_User_Library_DMO
                                                from apr in _LibraryContext.ApplicationUserRole
                                                from apu in _LibraryContext.ApplicationUser
                                                from d in _LibraryContext.LIB_Library_Class_DMO
                                                where (b.MI_Id == c.MI_Id && c.MI_Id == d.MI_Id && b.LMAL_Id == c.LMAL_Id && b.LMAL_Id == d.LMAL_Id && apr.UserId == apu.Id && apu.Id == c.IVRMUL_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                                select new LIB_Master_Library_DTO
                                                {
                                                    LMAL_Id = b.LMAL_Id,
                                                    LUL_Id = c.LUL_Id,
                                                    LMAL_LibraryName = b.LMAL_LibraryName,
                                                    LMAL_ActiveFlag = b.LMAL_ActiveFlag,
                                                    HRME_EmployeeFirstName = apu.UserName,
                                                    IVRMUL_Id = c.IVRMUL_Id,
                                                    LUL_ActiveFlg = c.LUL_ActiveFlg,

                                                }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();
                            }
                            else if (data.MI_SchoolCollegeFlag == "C")//For College Only
                            {
                                data.alldata = (from b in _LibraryContext.LIB_Master_Library_DMO
                                                from c in _LibraryContext.LIB_User_Library_DMO
                                                from apr in _LibraryContext.ApplicationUserRole
                                                from apu in _LibraryContext.ApplicationUser
                                                where (b.MI_Id == c.MI_Id && b.LMAL_Id == c.LMAL_Id && apr.UserId == apu.Id && apu.Id == c.IVRMUL_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                                select new LIB_Master_Library_DTO
                                                {
                                                    LMAL_Id = b.LMAL_Id,
                                                    LUL_Id = c.LUL_Id,
                                                    LMAL_LibraryName = b.LMAL_LibraryName,
                                                    LMAL_ActiveFlag = b.LMAL_ActiveFlag,
                                                    HRME_EmployeeFirstName = apu.UserName,
                                                    IVRMUL_Id = c.IVRMUL_Id,
                                                    LUL_ActiveFlg = c.LUL_ActiveFlg,

                                                }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();
                            }
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
