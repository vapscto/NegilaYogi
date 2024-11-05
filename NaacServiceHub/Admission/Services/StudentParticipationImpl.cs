using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Medical;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class StudentParticipationImpl : Interface.StudentParticipationInterface
    {

        public GeneralContext _GeneralContext;
        public StudentParticipationImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async Task<NAAC_AC_SParticipation_123_Students_DTO> loaddata(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }

                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new NAAC_AC_SParticipation_123_Students_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_GET_NAAC_AC_SParticipation_123";
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_SParticipation_123_Students_DTO savedata(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                if (data.NCACSP123_Id == 0)
                {

                    NAAC_AC_SParticipation_123_DMO obj = new NAAC_AC_SParticipation_123_DMO();

                    obj.MI_Id = data.MI_Id;
                    obj.NCACSP123_AddOnProgramName = data.NCACSP123_AddOnProgramName;
                    obj.NCACSP123_NoOfStudParticipated = data.NCACSP123_NoOfStudParticipated;
                    obj.NCACSP123_Year = data.ASMAY_Id;
                    obj.NCACSP123_Date = data.NCACSP123_Date;
                    obj.NCACSP123_ActiveFlg = true;
                    obj.NCACSP123_CreatedBy = data.UserId;
                    obj.NCACSP123_UpdatedBy = data.UserId;
                    obj.NCACSP123_CreatedDate = DateTime.Now;
                    obj.NCACSP123_UpdatedDate = DateTime.Now;
                    obj.NCACSP123_StatusFlg = "";
                    obj.NCACSP123_Remarks = "";
                  

                    _GeneralContext.Add(obj);
                    for (int t = 0; t < data.studentlstdata.Length; t++)
                    {
                        NAAC_AC_SParticipation_123_Students_DMO obj2 = new NAAC_AC_SParticipation_123_Students_DMO();


                        obj2.NCACSP123_Id = obj.NCACSP123_Id;
                        obj2.AMCST_Id = data.studentlstdata[t].AMCST_Id;
                        obj2.NCACSP123S_ActiveFlg = true;
                        obj2.NCACSP123S_CreatedBy = data.UserId;
                        obj2.NCACSP123S_UpdatedBy = data.UserId;
                        obj2.NCACSP123S_CreatedDate = DateTime.Now;
                        obj2.NCACSP123S_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj2);
                    }
                    if (data.filelist.Length > 0)
                    {
                        for (int j = 0; j < data.filelist.Length; j++)
                        {
                            if (data.filelist[0].cfilepath != null)
                            {
                                NAAC_AC_SParticipation_123_FilesDMO obj2 = new NAAC_AC_SParticipation_123_FilesDMO();

                                obj2.NCACSP123F_FileName = data.filelist[j].cfilename;
                                obj2.NCACSP123F_Filedesc = data.filelist[j].cfiledesc;
                                obj2.NCACSP123F_FilePath = data.filelist[j].cfilepath;
                                obj2.NCACSP123_Id = obj.NCACSP123_Id;
                                obj2.NCACSP123F_StatusFlg = "";
                                obj2.NCACSP123F_ActiveFlg = true;
                                obj2.NCACSP123F_Remarks = "";
                                
                                _GeneralContext.Add(obj2);
                            }
                        }
                    }
                    int s = _GeneralContext.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "saved";
                        data.returnval = true;
                    }
                    else
                    {
                        data.msg = "notsaved";
                        data.returnval = false;
                    }


                }
                else if (data.NCACSP123_Id > 0)
                {

                    var update1 = _GeneralContext.NAAC_AC_SParticipation_123_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSP123_Id == data.NCACSP123_Id).SingleOrDefault();

                    update1.NCACSP123_Year = data.ASMAY_Id;
                    update1.NCACSP123_AddOnProgramName = data.NCACSP123_AddOnProgramName;
                    update1.NCACSP123_NoOfStudParticipated = data.NCACSP123_NoOfStudParticipated;
                    update1.NCACSP123_Date = data.NCACSP123_Date;
                    update1.NCACSP123_UpdatedBy = data.UserId;
                    update1.NCACSP123_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update1);

                    for (int t = 0; t < data.studentlstdata.Length; t++)
                    {
                        var update2 = _GeneralContext.NAAC_AC_SParticipation_123_Students_DMO.Single(ab => ab.NCACSP123S_Id == data.NCACSP123S_Id);

                        update2.NCACSP123_Id = update1.NCACSP123_Id;
                        update2.AMCST_Id = data.studentlstdata[t].AMCST_Id;
                        update2.NCACSP123S_UpdatedBy = data.UserId;
                        update2.NCACSP123S_UpdatedDate = DateTime.Now;

                        _GeneralContext.Update(update2);
                    }
                    var CountRemoveFiles = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Where(t => t.NCACSP123_Id == data.NCACSP123_Id).ToList();

                    List<long> temparr = new List<long>();
                    //getting all mobilenumbers
                    foreach (var c in data.filelist)
                    {
                        temparr.Add(c.cfileid);
                    }


                    var Phone_Noresultremove = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Where(t => !temparr.Contains(t.NCACSP123F_Id)
                    && t.NCACSP123_Id == data.NCACSP123_Id).ToList();

                    foreach (var ph1 in Phone_Noresultremove)
                    {
                        var resultremove112 = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Single(a => a.NCACSP123F_Id == ph1.NCACSP123F_Id);
                        resultremove112.NCACSP123F_ActiveFlg = false;
                        _GeneralContext.Update(resultremove112);

                    }

                    if (data.filelist.Length > 0)
                    {
                        for (int k = 0; k < data.filelist.Length; k++)
                        {
                            var resultupload = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Where(a => a.NCACSP123_Id == data.NCACSP123_Id
                            && a.NCACSP123F_Id == data.filelist[k].cfileid).ToList();
                            if (resultupload.Count > 0)
                            {
                                var resultupdateupload = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Single(a => a.NCACSP123_Id == data.NCACSP123_Id
                                && a.NCACSP123F_Id == data.filelist[k].cfileid);
                                resultupdateupload.NCACSP123F_Filedesc = data.filelist[k].cfiledesc;
                                resultupdateupload.NCACSP123F_FileName = data.filelist[k].cfilename;
                                resultupdateupload.NCACSP123F_FilePath = data.filelist[k].cfilepath;
                                _GeneralContext.Update(resultupdateupload);
                            }
                            else
                            {
                                if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                {
                                    NAAC_AC_SParticipation_123_FilesDMO obj2 = new NAAC_AC_SParticipation_123_FilesDMO();
                                    obj2.NCACSP123F_FileName = data.filelist[k].cfilename;
                                    obj2.NCACSP123F_Filedesc = data.filelist[k].cfiledesc;
                                    obj2.NCACSP123F_FilePath = data.filelist[k].cfilepath;
                                    obj2.NCACSP123_Id = data.NCACSP123_Id;
                                    obj2.NCACSP123F_ActiveFlg = true;
                                    obj2.NCACSP123F_StatusFlg = "";
                                    obj2.NCACSP123F_Remarks = "";
                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                        
                    }
                    int row = _GeneralContext.SaveChanges();

                    if (row > 0)
                    {
                        data.msg = "updated";
                        data.returnval = true;
                    }
                    else
                    {
                        data.msg = "notupdated";
                        data.returnval = false;
                    }


                    //if (data.filelist.Length > 0)
                    //{
                    //    var CountRemoveFiles = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Where(t => t.NCACSP123_Id == data.NCACSP123_Id && t.NCACSP123F_StatusFlg != "Approved").ToList();
                    //    if (CountRemoveFiles.Count > 0)
                    //    {
                    //        foreach (var RemoveFiles in CountRemoveFiles)
                    //        {
                    //            _GeneralContext.Remove(RemoveFiles);
                    //        }
                    //    }

                    //    foreach (NAAC_AC_SParticipation_123_Students_DTO DTO in data.filelist)
                    //    {
                    //        if (DTO.NCACSP123_Id > 0 && DTO.NCACSP123F_StatusFlg != "Approved")
                    //        {
                    //            if (DTO.cfilepath != null)
                    //            {
                    //                var filesdata = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Where(b => b.NCACSP123_Id == DTO.NCACSP123_Id).FirstOrDefault();
                    //                filesdata.NCACSP123F_Filedesc = DTO.cfiledesc;
                    //                filesdata.NCACSP123F_FileName = DTO.cfilename;
                    //                filesdata.NCACSP123F_FilePath = DTO.cfilepath;

                    //                _GeneralContext.Update(filesdata);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (DTO.cfilepath != null)
                    //            {
                    //                NAAC_AC_SParticipation_123_FilesDMO obj2 = new NAAC_AC_SParticipation_123_FilesDMO();

                    //                obj2.NCACSP123F_FileName = DTO.cfilename;
                    //                obj2.NCACSP123F_Filedesc = DTO.cfiledesc;
                    //                obj2.NCACSP123F_FilePath = DTO.cfilepath;
                    //                obj2.NCACSP123_Id = data.NCACSP123_Id;
                    //                obj2.NCACSP123F_StatusFlg = "";
                    //                obj2.NCACSP123F_ActiveFlg =true;

                    //                _GeneralContext.Add(obj2);

                    //            }
                    //        }
                    //    }
                    //    int s = _GeneralContext.SaveChanges();
                    //    if (s > 0)
                    //    {
                    //        data.msg = "updated";
                    //        data.returnval = true;
                    //    }
                    //    else
                    //    {
                    //        data.msg = "notupdated";
                    //        data.returnval = false;
                    //    }
                    //}                    
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public async Task<NAAC_AC_SParticipation_123_Students_DTO> editdata(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_EditSParticipation_123";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACSP123_Year
                    });

                    cmd.Parameters.Add(new SqlParameter("@NCACSP123_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.NCACSP123_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACSP123S_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACSP123S_Id
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
                        var edit = retObject.ToList();



                        data.editlist = edit.ToArray();

                        List<long> amcoids = new List<long>();
                        List<long> ambids = new List<long>();
                        foreach (var item in edit)
                        {
                            amcoids.Add(item.AMCO_Id);
                        }
                        foreach (var item in edit)
                        {
                            ambids.Add(item.AMB_Id);
                        }
                        data.branchlist = (from a in _GeneralContext.MasterCourseDMO
                                           from d in _GeneralContext.ClgMasterBranchDMO
                                           from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                           from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                           where (a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == d.AMB_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.NCACSP123_Year && amcoids.Contains(a.AMCO_Id))
                                           select d).Distinct().OrderBy(t => t.AMB_Order).ToArray();


                        data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                            from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                            from c in _GeneralContext.MasterCourseDMO
                                            from d in _GeneralContext.ClgMasterBranchDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == b.ASMAY_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.NCACSP123_Year && amcoids.Contains(b.AMCO_Id) && ambids.Contains(b.AMB_Id) && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && c.AMCO_ActiveFlag == true && d.AMB_ActiveFlag == true)
                                            select new NAAC_AC_SParticipation_123_Students_DTO
                                            {
                                                studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                                AMCST_Id = a.AMCST_Id,
                                                AMCST_AdmNo = a.AMCST_AdmNo,
                                            }).Distinct().OrderBy(t => t.studentname).ToArray();

                        data.editFileslist = (from a in _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO
                                              where (a.NCACSP123_Id == data.NCACSP123_Id && a.NCACSP123F_ActiveFlg==true)
                                              select new NAAC_AC_SParticipation_123_Students_DTO
                                              {
                                                  cfilename = a.NCACSP123F_FileName,
                                                  cfilepath = a.NCACSP123F_FilePath,
                                                  cfiledesc = a.NCACSP123F_Filedesc,
                                                  cfileid = a.NCACSP123F_Id,
                                                  cfilestatus = a.NCACSP123F_StatusFlg,                                                 
                                                  cfileactive = a.NCACSP123F_ActiveFlg
                                              }).Distinct().ToArray();



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                try
                {
                    using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Naac_get_Clg_CourseBranch";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.NCACSP123_Year
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
                            data.courselist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public async Task<NAAC_AC_SParticipation_123_Students_DTO> deactivY(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_SParticipation_123_Students_DMO.Where(t => t.NCACSP123S_Id == data.NCACSP123S_Id).SingleOrDefault();

                if (result.NCACSP123S_ActiveFlg == true)
                {
                    result.NCACSP123S_ActiveFlg = false;
                }
                else if (result.NCACSP123S_ActiveFlg == false)
                {
                    result.NCACSP123S_ActiveFlg = true;
                }

                result.NCACSP123S_UpdatedDate = DateTime.Now;
                result.NCACSP123S_UpdatedBy = data.UserId;

                _GeneralContext.Update(result);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_SParticipation_123_Students_DTO get_student(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                data.AMCO_Id = (from t in _GeneralContext.Adm_Course_Branch_MappingDMO
                                where (t.AMB_Id == data.AMB_Id && t.MI_Id == data.MI_Id)
                                select t).Single().AMCO_Id;

                data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    from c in _GeneralContext.MasterCourseDMO
                                    from d in _GeneralContext.ClgMasterBranchDMO

                                    where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == b.ASMAY_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && c.AMCO_ActiveFlag == true && d.AMB_ActiveFlag == true)
                                    select new NAAC_AC_SParticipation_123_Students_DTO
                                    {
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_SParticipation_123_Students_DTO get_branch(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                data.AMCO_Id = (from t in _GeneralContext.Adm_Course_Branch_MappingDMO
                                where (t.AMB_Id == data.AMB_Id && t.MI_Id == data.MI_Id)
                                select t).Single().AMCO_Id;

                data.branchlist = (from a in _GeneralContext.MasterCourseDMO
                                   from d in _GeneralContext.ClgMasterBranchDMO
                                   from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == d.AMB_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                   select d).Distinct().OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public async Task<NAAC_AC_SParticipation_123_Students_DTO> get_MappedStudentList(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_MappedSParticipation_123";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = (data.MI_Id).ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                    {
                        Value = (data.ASMAY_Id).ToString()
                    });

                    cmd.Parameters.Add(new SqlParameter("@NCACSP123_Id",
                    SqlDbType.VarChar)
                    {
                        Value = (data.NCACSP123_Id).ToString()
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
                        data.mappedstudentlist = retObject.ToArray();
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
        public NAAC_AC_SParticipation_123_Students_DTO viewuploadflies(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_SParticipation_123_DMO
                                        where (t.NCACSP123_Id == data.NCACSP123_Id && t.NCACSP123_Id == b.NCACSP123_Id && b.MI_Id == data.MI_Id  && t.NCACSP123F_ActiveFlg==true)
                                        select new NAAC_AC_SParticipation_123_Students_DTO
                                        {
                                            cfilename = t.NCACSP123F_FileName,
                                            cfilepath = t.NCACSP123F_FilePath,
                                            cfiledesc = t.NCACSP123F_Filedesc,
                                            NCACSP123F_Id = t.NCACSP123F_Id,
                                            NCACSP123_Id = b.NCACSP123_Id,
                                            NCACSP123F_StatusFlg = t.NCACSP123F_StatusFlg,
                                            NCACSP123F_ApprovedFlg = t.NCACSP123F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_SParticipation_123_Students_DTO deleteuploadfile(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO.Where(t => t.NCACSP123F_Id == data.NCACSP123F_Id).ToList();
                if (result.Count > 0)
                {
                    foreach (var resultid in result)
                    {
                        _GeneralContext.Remove(resultid);
                    }
                }
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_SParticipation_123_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_SParticipation_123_DMO
                                        where (t.NCACSP123_Id == data.NCACSP123_Id && t.NCACSP123_Id == b.NCACSP123_Id && b.MI_Id == data.MI_Id && t.NCACSP123F_ActiveFlg==true)
                                        select new NAAC_AC_SParticipation_123_Students_DTO
                                        {
                                            cfilename = t.NCACSP123F_FileName,
                                            cfilepath = t.NCACSP123F_FilePath,
                                            cfiledesc = t.NCACSP123F_Filedesc,
                                            NCACSP123F_Id = t.NCACSP123F_Id,
                                            NCACSP123_Id = b.NCACSP123_Id,
                                            NCACSP123F_StatusFlg = t.NCACSP123F_StatusFlg,
                                            NCACSP123F_ApprovedFlg = t.NCACSP123F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public async Task<NAAC_AC_SParticipation_123_Students_DTO> get_coursebrnch(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                //data.courselist = _GeneralContext.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_get_Clg_CourseBranch";
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
                        data.courselist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        //add row wise comments
        public NAAC_AC_SParticipation_123_Students_DTO savemedicaldatawisecomments(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                NAAC_AC_SParticipation_123_Comments_DMO obj1 = new NAAC_AC_SParticipation_123_Comments_DMO();

                obj1.NCACSP123C_Remarks = data.Remarks;
                obj1.NCACSP123C_RemarksBy = data.UserId;
                obj1.NCACSP123C_StatusFlg = "";
                obj1.NCACSP123_Id = data.filefkid;
                obj1.NCACSP123C_ActiveFlag = true;
                obj1.NCACSP123C_CreatedBy = data.UserId;
                obj1.NCACSP123C_UpdatedBy = data.UserId;
                obj1.NCACSP123C_CreatedDate = DateTime.Now;
                obj1.NCACSP123C_UpdatedDate = DateTime.Now;

                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //add file wise comments
        public NAAC_AC_SParticipation_123_Students_DTO savefilewisecomments(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                NAAC_AC_SParticipation_123_File_Comments_DMO obj1 = new NAAC_AC_SParticipation_123_File_Comments_DMO();

                obj1.NCACSP123FC_Remarks = data.Remarks;
                obj1.NCACSP123FC_RemarksBy = data.UserId;
                obj1.NCACSP123FC_StatusFlg = "";
                obj1.NCACSP123F_Id = data.filefkid;
                obj1.NCACSP123FC_ActiveFlag = true;
                obj1.NCACSP123FC_CreatedBy = data.UserId;
                obj1.NCACSP123FC_UpdatedBy = data.UserId;
                obj1.NCACSP123FC_CreatedDate = DateTime.Now;
                obj1.NCACSP123FC_UpdatedDate = DateTime.Now;

                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        // view row wise comments
        public NAAC_AC_SParticipation_123_Students_DTO getcomment(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_SParticipation_123_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACSP123C_RemarksBy == b.Id && a.NCACSP123_Id == data.NCACSP123_Id)
                                    select new NAAC_AC_SParticipation_123_Students_DTO
                                    {
                                        NCACSP123C_Remarks = a.NCACSP123C_Remarks,
                                        NCACSP123_Id = a.NCACSP123_Id,
                                        NCACSP123C_Id = a.NCACSP123C_Id,
                                        NCACSP123C_RemarksBy = a.NCACSP123C_RemarksBy,
                                        NCACSP123C_StatusFlg = a.NCACSP123C_StatusFlg,
                                        NCACSP123C_ActiveFlag = a.NCACSP123C_ActiveFlag,
                                        NCACSP123C_CreatedBy = a.NCACSP123C_CreatedBy,
                                        NCACSP123C_CreatedDate = a.NCACSP123C_CreatedDate,
                                        NCACSP123C_UpdatedBy = a.NCACSP123C_UpdatedBy,
                                        NCACSP123C_UpdatedDate = a.NCACSP123C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACSP123C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_AC_SParticipation_123_Students_DTO getfilecomment(NAAC_AC_SParticipation_123_Students_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_SParticipation_123_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACSP123FC_RemarksBy == b.Id && a.NCACSP123F_Id == data.NCACSP123F_Id)
                                     select new NAAC_AC_SParticipation_123_Students_DTO
                                     {
                                         NCACSP123F_Id = a.NCACSP123F_Id,
                                         NCACSP123FC_Remarks = a.NCACSP123FC_Remarks,
                                         NCACSP123FC_Id = a.NCACSP123FC_Id,
                                         NCACSP123FC_RemarksBy = a.NCACSP123FC_RemarksBy,
                                         NCACSP123FC_StatusFlg = a.NCACSP123FC_StatusFlg,
                                         NCACSP123FC_ActiveFlag = a.NCACSP123FC_ActiveFlag,
                                         NCACSP123FC_CreatedBy = a.NCACSP123FC_CreatedBy,
                                         NCACSP123FC_CreatedDate = a.NCACSP123FC_CreatedDate,
                                         NCACSP123FC_UpdatedBy = a.NCACSP123FC_UpdatedBy,
                                         NCACSP123FC_UpdatedDate = a.NCACSP123FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACSP123FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
