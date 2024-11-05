using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services
{
    public class Naac_VACImpl : Interface.Naac_VACInterface
    {

        public GeneralContext _GeneralContext;
        public Naac_VACImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async Task<NAAC_AC_VAC_DTO> loaddata(NAAC_AC_VAC_DTO data)
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

                data.introyearlist = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.discontyearlist = (from t in _GeneralContext.Academic
                                        where (t.MI_Id == data.MI_Id && t.Is_Active == true)
                                        select new NAAC_AC_VAC_DTO
                                        {
                                            discontid = t.ASMAY_Id,
                                            discontyearnm = t.ASMAY_Year,
                                        }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.detailsyearlist = (from b in _GeneralContext.Academic
                                        where (b.MI_Id == data.MI_Id && b.Is_Active == true)
                                        select new NAAC_AC_VAC_DTO
                                        {
                                            detailsyearid = b.ASMAY_Id,
                                            detailsyearname = b.ASMAY_Year,
                                        }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.completedyearlist = (from y in _GeneralContext.Academic
                                          where (y.MI_Id == data.MI_Id && y.Is_Active == true)
                                          select new NAAC_AC_VAC_DTO
                                          {
                                              completedyearid = y.ASMAY_Id,
                                              completedyearname = y.ASMAY_Year,
                                          }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.ccourseNamelist = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_ActiveFlg == true).Distinct().OrderBy(t => t.NCACVAC132_Id).ToArray();

                data.alldata = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                from b in _GeneralContext.Academic
                                where (a.MI_Id == data.MI_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true)
                                select new NAAC_AC_VAC_DTO
                                {

                                    NCACVAC132_IntroYear = a.NCACVAC132_IntroYear,
                                    NCACVAC132_Id = a.NCACVAC132_Id,
                                    NCACVAC132_DiscontinuedFlg = a.NCACVAC132_DiscontinuedFlg,
                                    NCACVAC132_DiscontinuedYear = a.NCACVAC132_DiscontinuedYear,
                                    NCACVAC132_ActiveFlg = a.NCACVAC132_ActiveFlg,
                                    NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                                    NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                                    NCACVAC132_StatusFlg = a.NCACVAC132_StatusFlg,
                                    ASMAY_Year = b.ASMAY_Year,
                                    MI_Id = a.MI_Id,
                                    discontinuedyear = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.NCACVAC132_DiscontinuedYear).FirstOrDefault().ASMAY_Year),
                                }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();



                data.alldatatab2 = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                    from c in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                    from d in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                    from b in _GeneralContext.Academic
                                    from s in _GeneralContext.Adm_Master_College_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == c.NCACVAC132_Id && s.AMCST_Id == d.AMCST_Id && s.AMCST_SOL == "S" && c.NCACVAC132D_Id == d.NCACVAC132D_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true && a.NCACVAC132_ActiveFlg == true)
                                    select new NAAC_AC_VAC_DTO
                                    {

                                        NCACVAC132_Id = a.NCACVAC132_Id,
                                        NCACVAC132D_Id = c.NCACVAC132D_Id,
                                        NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                                        NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                                        ASMAY_Year = b.ASMAY_Year,
                                        MI_Id = a.MI_Id,
                                        NCACVAC132D_StatusFlg = c.NCACVAC132D_StatusFlg,
                                       

                                        NCACVAC132D_Date = c.NCACVAC132D_Date,
                                        NCACVAC132D_NoOfStdCompleted = c.NCACVAC132D_NoOfStdCompleted,
                                        NCACVAC132D_NoOfStudentsEnr = c.NCACVAC132D_NoOfStudentsEnr,
                                        NCACVAC132D_Year = c.NCACVAC132D_Year,
                                        ASMAY_Id = b.ASMAY_Id,
                                        // NCACVAC132DS_CompletedFlg = d.NCACVAC132DS_CompletedFlg,
                                        // completedyear = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == d.NCACVAC132DS_CompletedYear).FirstOrDefault().ASMAY_Year),
                                        // AMCST_Id = d.AMCST_Id,
                                        // studentname = s.AMCST_FirstName + (string.IsNullOrEmpty(s.AMCST_MiddleName) ? "" : ' ' + s.AMCST_MiddleName) + (string.IsNullOrEmpty(s.AMCST_LastName) ? "" : ' ' + s.AMCST_LastName),
                                        NCACVAC132D_ActiveFlg = c.NCACVAC132D_ActiveFlg,
                                 
                                    }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();

                data.studentlist1= (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                    from c in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                    from d in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                    from b in _GeneralContext.Academic
                                    from s in _GeneralContext.Adm_Master_College_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == c.NCACVAC132_Id && s.AMCST_Id == d.AMCST_Id && s.AMCST_SOL == "S" && c.NCACVAC132D_Id == d.NCACVAC132D_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true && a.NCACVAC132_ActiveFlg == true)
                                    select new NAAC_AC_VAC_DTO
                                    {

                                        NCACVAC132_Id = a.NCACVAC132_Id,
                                        NCACVAC132D_Id = c.NCACVAC132D_Id,
                                        NCACVAC132DS_Id = d.NCACVAC132DS_Id,
                                        MI_Id = a.MI_Id,
                                         studentname = s.AMCST_FirstName + (string.IsNullOrEmpty(s.AMCST_MiddleName) ? "" : ' ' + s.AMCST_MiddleName) + (string.IsNullOrEmpty(s.AMCST_LastName) ? "" : ' ' + s.AMCST_LastName),
                                        NCACVAC132D_ActiveFlg = c.NCACVAC132D_ActiveFlg,

                                    }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO savedatatab1(NAAC_AC_VAC_DTO data)
        {
            try
            {
                if (data.NCACVAC132_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_CourseName == data.NCACVAC132_CourseName && t.NCACVAC132_CourseCode == data.NCACVAC132_CourseCode && t.NCACVAC132_IntroYear == data.NCACVAC132_IntroYear).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_VAC_132_DMO obj1 = new NAAC_AC_VAC_132_DMO();
                        //obj1.NCACVAC132_Id = data.NCACVAC132_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCACVAC132_CourseName = data.NCACVAC132_CourseName;
                        obj1.NCACVAC132_CourseCode = data.NCACVAC132_CourseCode;
                        obj1.NCACVAC132_IntroYear = data.NCACVAC132_IntroYear;
                        obj1.NCACVAC132_ActiveFlg = true;
                        obj1.NCACVAC132_CreatedBy = data.UserId;
                        obj1.NCACVAC132_UpdatedBy = data.UserId;
                        obj1.NCACVAC132_StatusFlg = "";
                        obj1.NCACVAC132_CreatedDate = DateTime.Now;
                        obj1.NCACVAC132_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj1);
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[j].cfilepath !=null)
                                {
                                    NAAC_AC_VAC_132_Files_DMO obj2 = new NAAC_AC_VAC_132_Files_DMO();
                                    obj2.NCACVAC132F_FileName = data.filelist[j].cfilename;
                                    obj2.NCACVAC132F_Filedesc = data.filelist[j].cfiledesc;
                                    obj2.NCACVAC132F_FilePath = data.filelist[j].cfilepath;
                                    obj2.NCACVAC132F_StatusFlg = "";
                                    obj2.NCACVAC132F_ActiveFlg = true;
                                    obj2.NCACVAC132_Id = obj1.NCACVAC132_Id;

                                    _GeneralContext.Add(obj2);
                                }
                               
                            }
                        }
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

                }
                else if (data.NCACVAC132_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_Id != data.NCACVAC132_Id && t.NCACVAC132_CourseName == data.NCACVAC132_CourseName && t.NCACVAC132_CourseCode == data.NCACVAC132_CourseCode && t.NCACVAC132_IntroYear == data.NCACVAC132_IntroYear).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_Id == data.NCACVAC132_Id).SingleOrDefault();

                        update.NCACVAC132_CourseName = data.NCACVAC132_CourseName;
                        update.NCACVAC132_CourseCode = data.NCACVAC132_CourseCode;
                        update.NCACVAC132_IntroYear = data.NCACVAC132_IntroYear;
                        update.NCACVAC132_UpdatedBy = data.UserId;
                        update.NCACVAC132_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update);

                        
                        if (data.filelist.Length > 0)
                        {
                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCACVAC132F_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_VAC_132_Files_DMO.Where(t => t.NCACVAC132_Id == data.NCACVAC132_Id && !Fid.Contains(t.NCACVAC132F_Id)).Distinct().ToList();
                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_VAC_132_Files_DMO.Single(t => t.NCACVAC132_Id == data.NCACVAC132_Id && t.NCACVAC132F_Id == item2.NCACVAC132F_Id);
                                    deactfile.NCACVAC132F_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);
                                }
                            }


                            foreach (NAAC_AC_VAC_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCACVAC132F_Id > 0 && DocumentsDTO.NCACVAC132F_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_VAC_132_Files_DMO.Where(t => t.NCACVAC132F_Id == DocumentsDTO.NCACVAC132F_Id).FirstOrDefault();
                                        filesdata.NCACVAC132F_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCACVAC132F_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCACVAC132F_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);
                                        
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCACVAC132F_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_VAC_132_Files_DMO obj2 = new NAAC_AC_VAC_132_Files_DMO();
                                            obj2.NCACVAC132F_FileName = DocumentsDTO.cfilename;
                                            obj2.NCACVAC132F_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCACVAC132F_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCACVAC132F_StatusFlg = "";
                                            obj2.NCACVAC132F_ActiveFlg = true;
                                            obj2.NCACVAC132_Id = data.NCACVAC132_Id;
                                            _GeneralContext.Add(obj2);                                            
                                        }
                                    }
                                }
                            }
                        }
                        int flag = _GeneralContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = true;
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
        //added by sanjeev
        public NAAC_AC_VAC_DTO saveadvance(NAAC_AC_VAC_DTO data)
        {
            try
            {
                if (data.advimppln.Length > 0)
                {
                    var Listarray = new ArrayList();
                    var duplicatevalue = new ArrayList();
                    var rowno = 1;
                    foreach (var I in data.advimppln)
                    {
                        data.ASMAY_Id = 0;
                        data.ASMAY_Id = _GeneralContext.Academic.Where(R => R.ASMAY_Year == I.IntroductionYear && R.MI_Id == data.MI_Id && R.Is_Active == true).Select(P => P.ASMAY_Id).FirstOrDefault();
                       if(data.ASMAY_Id >0)
                        {
                            var duplicate = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_CourseName == I.CourseName && t.NCACVAC132_CourseCode == I.CourseCode && t.NCACVAC132_IntroYear == data.ASMAY_Id).ToList();
                            if (duplicate.Count > 0)
                            {
                                Listarray.Add(I);
                                
                            }
                            else
                            {
                                NAAC_AC_VAC_132_DMO obj1 = new NAAC_AC_VAC_132_DMO();                             
                                obj1.MI_Id = data.MI_Id;
                                obj1.NCACVAC132_CourseName = I.CourseName;
                                obj1.NCACVAC132_CourseCode = I.CourseCode;
                                obj1.NCACVAC132_IntroYear = data.ASMAY_Id;
                                obj1.NCACVAC132_ActiveFlg = true;
                                obj1.NCACVAC132_CreatedBy = data.UserId;
                                obj1.NCACVAC132_UpdatedBy = data.UserId;
                                obj1.NCACVAC132_StatusFlg = "";
                                obj1.NCACVAC132_FromExelImportFlag = true;
                                obj1.NCACVAC132_FreezeFlag = true;
                                obj1.NCACVAC132_CreatedDate = DateTime.Now;
                                obj1.NCACVAC132_UpdatedDate = DateTime.Now;
                                _GeneralContext.Add(obj1);                                                             
                            }
                        }
                        else
                        {
                            Listarray.Add(I);
                        }

                    }
                    data.modalExcel = Listarray.ToArray();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //close
        public NAAC_AC_VAC_DTO deactivYTab1(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_Id == data.NCACVAC132_Id).SingleOrDefault();

                if (result.NCACVAC132_ActiveFlg == true)
                {
                    result.NCACVAC132_ActiveFlg = false;
                }
                else if (result.NCACVAC132_ActiveFlg == false)
                {
                    result.NCACVAC132_ActiveFlg = true;
                }

                result.NCACVAC132_UpdatedDate = DateTime.Now;
                result.NCACVAC132_UpdatedBy = data.UserId;

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
        public NAAC_AC_VAC_DTO editTab1(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_Id == data.NCACVAC132_Id).ToList();

                data.editlisttab1 = edit.ToArray();

                data.editMainSActFileslist = (from a in _GeneralContext.NAAC_AC_VAC_132_Files_DMO
                                              where (a.NCACVAC132_Id == data.NCACVAC132_Id&&a.NCACVAC132F_ActiveFlg==true)
                                              select new NAAC_AC_VAC_DTO
                                              {
                                                  cfilename = a.NCACVAC132F_FileName,
                                                  cfilepath = a.NCACVAC132F_FilePath,
                                                  cfiledesc = a.NCACVAC132F_Filedesc,
                                                  NCACVAC132F_Id = a.NCACVAC132F_Id,
                                                  NCACVAC132_Id = a.NCACVAC132_Id,
                                                  NCACVAC132F_StatusFlg = a.NCACVAC132F_StatusFlg,

                                              }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_VAC_DTO getcommentmaster(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_VAC_132_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACVAC132C_RemarksBy == b.Id && a.NCACVAC132_Id == data.NCACVAC132_Id)
                                    select new NAAC_AC_VAC_DTO
                                    {
                                        NCACVAC132C_Remarks = a.NCACVAC132C_Remarks,
                                        NCACVAC132C_Id = a.NCACVAC132C_Id,
                                        NCACVAC132C_RemarksBy = a.NCACVAC132C_RemarksBy,
                                        NCACVAC132C_StatusFlg = a.NCACVAC132C_StatusFlg,
                                        NCACVAC132C_ActiveFlag = a.NCACVAC132C_ActiveFlag,
                                        NCACVAC132C_CreatedBy = a.NCACVAC132C_CreatedBy,
                                        NCACVAC132C_CreatedDate = a.NCACVAC132C_CreatedDate,
                                        NCACVAC132C_UpdatedBy = a.NCACVAC132C_UpdatedBy,
                                        NCACVAC132C_UpdatedDate = a.NCACVAC132C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACVAC132C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_VAC_DTO getfilecommentmaster(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_VAC_132_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACVAC132FC_RemarksBy == b.Id && a.NCACVAC132F_Id == data.NCACVAC132F_Id)
                                     select new NAAC_AC_VAC_DTO
                                     {
                                         NCACVAC132F_Id = a.NCACVAC132F_Id,
                                         NCACVAC132FC_Remarks = a.NCACVAC132FC_Remarks,
                                         NCACVAC132FC_Id = a.NCACVAC132FC_Id,
                                         NCACVAC132FC_RemarksBy = a.NCACVAC132FC_RemarksBy,
                                         NCACVAC132FC_StatusFlg = a.NCACVAC132FC_StatusFlg,
                                         NCACVAC132FC_ActiveFlag = a.NCACVAC132FC_ActiveFlag,
                                         NCACVAC132FC_CreatedBy = a.NCACVAC132FC_CreatedBy,
                                         NCACVAC132FC_CreatedDate = a.NCACVAC132FC_CreatedDate,
                                         NCACVAC132FC_UpdatedBy = a.NCACVAC132FC_UpdatedBy,
                                         NCACVAC132FC_UpdatedDate = a.NCACVAC132FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACVAC132FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO savemedicaldatawisecommentsmaster(NAAC_AC_VAC_DTO data)
        {
            try
            {
                NAAC_AC_VAC_132_Comments_DMO obj1 = new NAAC_AC_VAC_132_Comments_DMO();
                obj1.NCACVAC132C_Remarks = data.Remarks;
                obj1.NCACVAC132C_RemarksBy = data.UserId;
                obj1.NCACVAC132C_StatusFlg = "";
                obj1.NCACVAC132_Id = data.filefkid;
                obj1.NCACVAC132C_ActiveFlag = true;
                obj1.NCACVAC132C_CreatedBy = data.UserId;
                obj1.NCACVAC132C_UpdatedBy = data.UserId;
                obj1.NCACVAC132C_CreatedDate = DateTime.Now;
                obj1.NCACVAC132C_UpdatedDate = DateTime.Now;
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

        // for file adding
        public NAAC_AC_VAC_DTO savefilewisecommentsmaster(NAAC_AC_VAC_DTO data)
        {
            try
            {
                NAAC_AC_VAC_132_File_Comments_DMO obj1 = new NAAC_AC_VAC_132_File_Comments_DMO();
                obj1.NCACVAC132FC_Remarks = data.Remarks;
                obj1.NCACVAC132FC_RemarksBy = data.UserId;
                obj1.NCACVAC132FC_StatusFlg = "";
                obj1.NCACVAC132F_Id = data.filefkid;
                obj1.NCACVAC132FC_ActiveFlag = true;
                obj1.NCACVAC132FC_CreatedBy = data.UserId;
                obj1.NCACVAC132FC_UpdatedBy = data.UserId;
                obj1.NCACVAC132FC_UpdatedDate = DateTime.Now;
                obj1.NCACVAC132FC_CreatedDate = DateTime.Now;
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



        public NAAC_AC_VAC_DTO get_student(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S"
                                    && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1)
                                    select new NAAC_AC_VAC_DTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();

                data.ccourseNamelist = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_IntroYear== data.ASMAY_Id && t.NCACVAC132_ActiveFlg == true).Distinct().OrderBy(t => t.NCACVAC132_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
       
        public NAAC_AC_VAC_DTO savedatatab2(NAAC_AC_VAC_DTO data)
        {
            try
            {
                if (data.NCACVAC132D_Id == 0)
                {

                    NAAC_AC_VAC_132_Details_DMO obj2 = new NAAC_AC_VAC_132_Details_DMO();
                    obj2.NCACVAC132D_Id = data.NCACVAC132D_Id;
                    obj2.NCACVAC132_Id = data.NCACVAC132_Id;
                    obj2.NCACVAC132D_Date = data.NCACVAC132D_Date;
                    obj2.NCACVAC132D_Year = data.NCACVAC132D_Year;
                    obj2.NCACVAC132D_NoOfStudentsEnr = data.NCACVAC132D_NoOfStudentsEnr;
                    obj2.NCACVAC132D_ActiveFlg = true;
                    obj2.NCACVAC132D_CreatedBy = data.UserId;
                    obj2.NCACVAC132D_UpdatedBy = data.UserId;
                    obj2.NCACVAC132D_CreatedDate = DateTime.Now;
                    obj2.NCACVAC132D_UpdatedDate = DateTime.Now;
                    obj2.NCACVAC132D_StatusFlg = "";
                    _GeneralContext.Add(obj2);

                    if (data.filelist_student.Length > 0)
                    {
                        for (int j = 0; j < data.filelist_student.Length; j++)
                        {
                            if (data.filelist_student[j].cfilepath !=null)
                            {
                                NAAC_AC_VAC_132_Details_FilesDMO obj23 = new NAAC_AC_VAC_132_Details_FilesDMO();
                                obj23.NCACVAC132DF_FileName = data.filelist_student[j].cfilename;
                                obj23.NCACVAC132DF_Filedesc = data.filelist_student[j].cfiledesc;
                                obj23.NCACVAC132DF_FilePath = data.filelist_student[j].cfilepath;
                                obj23.NCACVAC132DF_StatusFlg = "";
                                obj23.NCACVAC132DF_ActiveFlg = true;
                                obj23.NCACVAC132D_Id = obj2.NCACVAC132D_Id;

                                _GeneralContext.Add(obj23);
                            }                           
                        }
                    }

                    for (int i = 0; i < data.studentlstdata.Length; i++)
                    {
                        NAAC_AC_VAC_132_Details_Students_DMO obj3 = new NAAC_AC_VAC_132_Details_Students_DMO();
                        obj3.NCACVAC132DS_Id = data.NCACVAC132DS_Id;
                        obj3.NCACVAC132D_Id = obj2.NCACVAC132D_Id;
                        obj3.AMCST_Id = data.studentlstdata[i].AMCST_Id;
                        obj3.NCACVAC132DS_ActiveFlg = true;
                        obj3.NCACVAC132DS_CreatedBy = data.UserId;
                        obj3.NCACVAC132DS_UpdatedBy = data.UserId;
                        obj3.NCACVAC132DS_StatusFlg = "";
                        obj3.NCACVAC132DS_CreatedDate = DateTime.Now;
                        obj3.NCACVAC132DS_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj3);
                    }

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
                else if (data.NCACVAC132D_Id > 0)
                {

                    var update = _GeneralContext.NAAC_AC_VAC_132_Details_DMO.Where(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id).SingleOrDefault();
                    update.NCACVAC132_Id = data.NCACVAC132_Id;
                    update.NCACVAC132D_Date = data.NCACVAC132D_Date;
                    update.NCACVAC132D_Year = data.NCACVAC132D_Year;
                    update.NCACVAC132D_NoOfStudentsEnr = data.NCACVAC132D_NoOfStudentsEnr;
                    update.NCACVAC132D_UpdatedBy = data.UserId;
                    update.NCACVAC132D_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);

                    //var CountRemoveFiles = _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO.Where(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id).ToList();
                    //if (CountRemoveFiles.Count > 0)
                    //{
                    //    foreach (var RemoveFiles in CountRemoveFiles)
                    //    {
                    //        _GeneralContext.Remove(RemoveFiles);
                    //    }
                    //}
                    if (data.filelist_student.Length > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.filelist_student)
                        {
                            Fid.Add(item.NCACVAC132DF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO.Where(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id && !Fid.Contains(t.NCACVAC132DF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO.Single(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id && t.NCACVAC132DF_Id == item2.NCACVAC132DF_Id);
                                deactfile.NCACVAC132DF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }



                        foreach (NAAC_AC_VAC_DTO DocumentsDTO in data.filelist_student)
                        {
                            if (DocumentsDTO.NCACVAC132DF_Id > 0 && DocumentsDTO.NCACVAC132DF_StatusFlg != "approved")
                            {
                                if (DocumentsDTO.cfilepath != null)
                                {
                                    var filesdata = _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO.Where(t => t.NCACVAC132DF_Id == DocumentsDTO.NCACVAC132DF_Id).FirstOrDefault();
                                    filesdata.NCACVAC132DF_Filedesc = DocumentsDTO.cfiledesc;
                                    filesdata.NCACVAC132DF_FileName = DocumentsDTO.cfilename;
                                    filesdata.NCACVAC132DF_FilePath = DocumentsDTO.cfilepath;
                                    _GeneralContext.Update(filesdata);                                    
                                }
                            }
                            else
                            {

                                if (DocumentsDTO.NCACVAC132DF_Id == 0)
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {
                                        NAAC_AC_VAC_132_Details_FilesDMO obj2 = new NAAC_AC_VAC_132_Details_FilesDMO();
                                        obj2.NCACVAC132DF_FileName = DocumentsDTO.cfilename;
                                        obj2.NCACVAC132DF_Filedesc = DocumentsDTO.cfiledesc;
                                        obj2.NCACVAC132DF_FilePath = DocumentsDTO.cfilepath;
                                        obj2.NCACVAC132DF_StatusFlg = "";
                                        obj2.NCACVAC132DF_ActiveFlg = true;
                                        obj2.NCACVAC132D_Id = data.NCACVAC132D_Id;
                                        _GeneralContext.Add(obj2);
                                        
                                    }
                                }
                            }
                        }



                        //for (int k = 0; k < data.filelist_student.Length; k++)
                        //{
                        //    if (data.filelist_student[0].cfilepath != null)
                        //    {
                         //      NAAC_AC_VAC_132_Details_FilesDMO obj2 = new NAAC_AC_VAC_132_Details_FilesDMO();
                        //        obj2.NCACVAC132DF_FileName = data.filelist_student[k].cfilename;
                        //        obj2.NCACVAC132DF_Filedesc = data.filelist_student[k].cfiledesc;
                        //        obj2.NCACVAC132DF_FilePath = data.filelist_student[k].cfilepath;
                        //        obj2.NCACVAC132D_Id = update.NCACVAC132D_Id;

                        //        _GeneralContext.Add(obj2);
                        //    }                              
                        //}
                    }

                    for (int i = 0; i < data.studentlstdata.Length; i++)
                    {
                        var obj3 = _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO.Where(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id && t.NCACVAC132DS_Id == data.NCACVAC132DS_Id).Single();
                        obj3.NCACVAC132D_Id = data.NCACVAC132D_Id;
                        obj3.AMCST_Id = data.studentlstdata[i].AMCST_Id;
                        obj3.NCACVAC132DS_UpdatedBy = data.UserId;
                        obj3.NCACVAC132DS_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(obj3);
                    }

                    int flag = _GeneralContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO deactivYTab2(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_VAC_132_Details_DMO.Where(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id).SingleOrDefault();

                var stulist = _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO.Where(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id&&t.NCACVAC132DS_ActiveFlg==true).ToList();

                if (stulist.Count > 0)
                {
                    data.message = "cantdeact";
                }
                else
                {
                    if (result.NCACVAC132D_ActiveFlg == true)
                    {
                        result.NCACVAC132D_ActiveFlg = false;
                    }
                    else if (result.NCACVAC132D_ActiveFlg == false)
                    {
                        result.NCACVAC132D_ActiveFlg = true;
                    }

                    result.NCACVAC132D_UpdatedDate = DateTime.Now;
                    result.NCACVAC132D_UpdatedBy = data.UserId;

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
                
                //data.alldatatab2 = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                //                    from c in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                //                    from d in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                //                    from b in _GeneralContext.Academic
                //                    from st in _GeneralContext.Adm_Master_College_StudentDMO
                //                    where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == c.NCACVAC132_Id && st.AMCST_Id == d.AMCST_Id && st.AMCST_SOL == "S" && c.NCACVAC132D_Id == d.NCACVAC132D_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true && a.NCACVAC132_ActiveFlg == true)
                //                    select new NAAC_AC_VAC_DTO
                //                    {

                //                        NCACVAC132_Id = a.NCACVAC132_Id,
                //                        NCACVAC132D_Id = c.NCACVAC132D_Id,
                //                        NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                //                        NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                //                        ASMAY_Year = b.ASMAY_Year,

                //                        NCACVAC132D_Date = c.NCACVAC132D_Date,
                //                        NCACVAC132D_NoOfStdCompleted = c.NCACVAC132D_NoOfStdCompleted,
                //                        NCACVAC132D_NoOfStudentsEnr = c.NCACVAC132D_NoOfStudentsEnr,
                //                        NCACVAC132D_Year = c.NCACVAC132D_Year,
                //                        ASMAY_Id = b.ASMAY_Id,
                //                        MI_Id = a.MI_Id,
                //                        NCACVAC132D_StatusFlg = c.NCACVAC132D_StatusFlg,
                //                       // NCACVAC132DS_CompletedFlg = d.NCACVAC132DS_CompletedFlg,
                //                        NCACVAC132D_ActiveFlg = c.NCACVAC132D_ActiveFlg,
                //                      //  completedyear = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == d.NCACVAC132DS_CompletedYear).FirstOrDefault().ASMAY_Year),
                //                       // AMCST_Id = d.AMCST_Id,
                //                       // studentname = st.AMCST_FirstName + (string.IsNullOrEmpty(st.AMCST_MiddleName) ? "" : ' ' + st.AMCST_MiddleName) + (string.IsNullOrEmpty(st.AMCST_LastName) ? "" : ' ' + st.AMCST_LastName),
                //                       // NCACVAC132DS_ActiveFlg = d.NCACVAC132DS_ActiveFlg,
                //                       // NCACVAC132DS_Id = d.NCACVAC132DS_Id,
                //                    }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        public NAAC_AC_VAC_DTO deactivYTabstudent(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO.Where(t => t.NCACVAC132DS_Id == data.NCACVAC132DS_Id).SingleOrDefault();

                if (result.NCACVAC132DS_ActiveFlg == true)
                {
                    result.NCACVAC132DS_ActiveFlg = false;
                }
                else if (result.NCACVAC132DS_ActiveFlg == false)
                {
                    result.NCACVAC132DS_ActiveFlg = true;
                }

                result.NCACVAC132DS_UpdatedDate = DateTime.Now;
                result.NCACVAC132DS_UpdatedBy = data.UserId;

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
                data.viewstudentlist = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                        from c in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                        from d in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                        from b in _GeneralContext.Academic
                                        from sss in _GeneralContext.Adm_Master_College_StudentDMO
                                        where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == c.NCACVAC132_Id && sss.AMCST_Id == d.AMCST_Id && sss.AMCST_SOL == "S" && c.NCACVAC132D_Id == d.NCACVAC132D_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true && a.NCACVAC132_ActiveFlg == true && c.NCACVAC132D_Id == data.NCACVAC132D_Id)
                                        select new NAAC_AC_VAC_DTO
                                        {

                                            NCACVAC132_Id = a.NCACVAC132_Id,
                                            NCACVAC132D_Id = c.NCACVAC132D_Id,
                                            NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                                            NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                                            ASMAY_Year = b.ASMAY_Year,
                                            MI_Id = a.MI_Id,
                                            NCACVAC132DS_Id = d.NCACVAC132DS_Id,

                                            NCACVAC132D_Date = c.NCACVAC132D_Date,
                                            NCACVAC132D_NoOfStdCompleted = c.NCACVAC132D_NoOfStdCompleted,
                                            NCACVAC132D_NoOfStudentsEnr = c.NCACVAC132D_NoOfStudentsEnr,
                                            NCACVAC132D_Year = c.NCACVAC132D_Year,
                                            ASMAY_Id = b.ASMAY_Id,
                                            NCACVAC132DS_CompletedFlg = d.NCACVAC132DS_CompletedFlg,
                                            NCACVAC132DS_StatusFlg = d.NCACVAC132DS_StatusFlg,
                                            completedyear = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == d.NCACVAC132DS_CompletedYear).FirstOrDefault().ASMAY_Year),
                                            AMCST_Id = d.AMCST_Id,
                                            studentname = sss.AMCST_FirstName + (string.IsNullOrEmpty(sss.AMCST_MiddleName) ? "" : ' ' + sss.AMCST_MiddleName) + (string.IsNullOrEmpty(sss.AMCST_LastName) ? "" : ' ' + sss.AMCST_LastName),
                                            NCACVAC132DS_ActiveFlg = d.NCACVAC132DS_ActiveFlg,

                                        }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();
                //data.alldatatab2 = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                //                    from c in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                //                    from d in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                //                    from b in _GeneralContext.Academic
                //                    from st in _GeneralContext.Adm_Master_College_StudentDMO
                //                    where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == c.NCACVAC132_Id && st.AMCST_Id == d.AMCST_Id && st.AMCST_SOL == "S" && c.NCACVAC132D_Id == d.NCACVAC132D_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true && a.NCACVAC132_ActiveFlg == true)
                //                    select new NAAC_AC_VAC_DTO
                //                    {

                //                        NCACVAC132_Id = a.NCACVAC132_Id,
                //                        NCACVAC132D_Id = c.NCACVAC132D_Id,
                //                        NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                //                        NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                //                        ASMAY_Year = b.ASMAY_Year,

                //                        NCACVAC132D_Date = c.NCACVAC132D_Date,
                //                        NCACVAC132D_NoOfStdCompleted = c.NCACVAC132D_NoOfStdCompleted,
                //                        NCACVAC132D_NoOfStudentsEnr = c.NCACVAC132D_NoOfStudentsEnr,
                //                        NCACVAC132D_Year = c.NCACVAC132D_Year,
                //                        ASMAY_Id = b.ASMAY_Id,
                //                        MI_Id = a.MI_Id,
                //                        NCACVAC132D_StatusFlg = c.NCACVAC132D_StatusFlg,
                //                       // NCACVAC132DS_CompletedFlg = d.NCACVAC132DS_CompletedFlg,
                //                        NCACVAC132D_ActiveFlg = c.NCACVAC132D_ActiveFlg,
                //                      //  completedyear = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == d.NCACVAC132DS_CompletedYear).FirstOrDefault().ASMAY_Year),
                //                       // AMCST_Id = d.AMCST_Id,
                //                       // studentname = st.AMCST_FirstName + (string.IsNullOrEmpty(st.AMCST_MiddleName) ? "" : ' ' + st.AMCST_MiddleName) + (string.IsNullOrEmpty(st.AMCST_LastName) ? "" : ' ' + st.AMCST_LastName),
                //                       // NCACVAC132DS_ActiveFlg = d.NCACVAC132DS_ActiveFlg,
                //                       // NCACVAC132DS_Id = d.NCACVAC132DS_Id,
                //                    }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        public NAAC_AC_VAC_DTO edittab2(NAAC_AC_VAC_DTO data)
        {
            try
            {
                List<long> details_id = new List<long>();
                List<long> amcstids = new List<long>();
                var edit = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                            from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                            from c in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO

                            where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == b.NCACVAC132_Id && b.NCACVAC132D_Id == c.NCACVAC132D_Id && b.NCACVAC132D_Id == data.NCACVAC132D_Id && c.NCACVAC132DS_Id == data.NCACVAC132DS_Id)
                            select new NAAC_AC_VAC_DTO
                            {
                                NCACVAC132D_Id = b.NCACVAC132D_Id,
                                NCACVAC132_Id = b.NCACVAC132_Id,
                                NCACVAC132D_Date = b.NCACVAC132D_Date,
                                NCACVAC132D_Year = b.NCACVAC132D_Year,
                                NCACVAC132D_NoOfStudentsEnr = b.NCACVAC132D_NoOfStudentsEnr,
                                NCACVAC132D_NoOfStdCompleted = b.NCACVAC132D_NoOfStdCompleted,
                                NCACVAC132D_ActiveFlg = b.NCACVAC132D_ActiveFlg,
                                NCACVAC132DS_Id = c.NCACVAC132DS_Id,
                                NCACVAC132D_StatusFlg = b.NCACVAC132D_StatusFlg,
                                MI_Id = a.MI_Id,
                            }).ToList();

                data.editlisttab2 = edit.ToArray();
                if (edit.Count > 0)
                {
                    foreach (var item in edit)
                    {
                        details_id.Add(item.NCACVAC132D_Id);
                    }
                }

                var list2 = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                             from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                             where (a.NCACVAC132D_Id == data.NCACVAC132D_Id && a.NCACVAC132DS_Id == data.NCACVAC132DS_Id && b.NCACVAC132D_Id == a.NCACVAC132D_Id)
                             select new NAAC_AC_VAC_DTO
                             {
                                 AMCST_Id = a.AMCST_Id,
                             }).ToList();
                if (list2.Count > 0)
                {
                    foreach (var item in list2)
                    {
                        amcstids.Add(item.AMCST_Id);
                    }
                }

                data.studentedit = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.NCACVAC132D_Year && amcstids.Contains(a.AMCST_Id) && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1)
                                    select new NAAC_AC_VAC_DTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();


                data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S"
                                    && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1)
                                    select new NAAC_AC_VAC_DTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();

                data.completeflage = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                      where (a.NCACVAC132D_Id == data.NCACVAC132D_Id && a.NCACVAC132DS_Id == data.NCACVAC132DS_Id)
                                      select new NAAC_AC_VAC_DTO
                                      {
                                          NCACVAC132DS_CompletedFlg = a.NCACVAC132DS_CompletedFlg,
                                          NCACVAC132DS_CompletedYear = a.NCACVAC132DS_CompletedYear,
                                      }).Distinct().ToArray();

                data.editStudentActFileslist = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO
                                                where (a.NCACVAC132D_Id == data.NCACVAC132D_Id&&a.NCACVAC132DF_ActiveFlg==true)
                                                select new NAAC_AC_VAC_DTO
                                                {
                                                    cfilename = a.NCACVAC132DF_FileName,
                                                    cfilepath = a.NCACVAC132DF_FilePath,
                                                    cfiledesc = a.NCACVAC132DF_Filedesc,
                                                    NCACVAC132DF_StatusFlg = a.NCACVAC132DF_StatusFlg,
                                                    NCACVAC132DF_Id = a.NCACVAC132DF_Id,
                                                    NCACVAC132D_Id = a.NCACVAC132D_Id,

                                                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view student
        public NAAC_AC_VAC_DTO viewstudent(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.viewstudentlist = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                    from c in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                    from d in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                    from b in _GeneralContext.Academic
                                    from s in _GeneralContext.Adm_Master_College_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.NCACVAC132_Id == c.NCACVAC132_Id && s.AMCST_Id == d.AMCST_Id && s.AMCST_SOL == "S" && c.NCACVAC132D_Id == d.NCACVAC132D_Id && a.NCACVAC132_IntroYear == b.ASMAY_Id && b.Is_Active == true && a.NCACVAC132_ActiveFlg == true&&c.NCACVAC132D_Id==data.NCACVAC132D_Id)
                                    select new NAAC_AC_VAC_DTO
                                    {

                                        NCACVAC132_Id = a.NCACVAC132_Id,
                                        NCACVAC132D_Id = c.NCACVAC132D_Id,
                                        NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                                        NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                                        ASMAY_Year = b.ASMAY_Year,
                                        MI_Id = a.MI_Id,
                                        NCACVAC132DS_Id = d.NCACVAC132DS_Id,

                                        NCACVAC132D_Date = c.NCACVAC132D_Date,
                                        NCACVAC132D_NoOfStdCompleted = c.NCACVAC132D_NoOfStdCompleted,
                                        NCACVAC132D_NoOfStudentsEnr = c.NCACVAC132D_NoOfStudentsEnr,
                                        NCACVAC132D_Year = c.NCACVAC132D_Year,
                                        ASMAY_Id = b.ASMAY_Id,
                                        NCACVAC132DS_CompletedFlg = d.NCACVAC132DS_CompletedFlg,
                                        completedyear = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == d.NCACVAC132DS_CompletedYear).FirstOrDefault().ASMAY_Year),
                                        AMCST_Id = d.AMCST_Id,
                                        studentname = s.AMCST_FirstName + (string.IsNullOrEmpty(s.AMCST_MiddleName) ? "" : ' ' + s.AMCST_MiddleName) + (string.IsNullOrEmpty(s.AMCST_LastName) ? "" : ' ' + s.AMCST_LastName),
                                        NCACVAC132DS_ActiveFlg = d.NCACVAC132DS_ActiveFlg,

                                    }).Distinct().OrderByDescending(t => t.NCACVAC132_Id).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_VAC_DTO getcomment(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACVAC132DC_RemarksBy == b.Id && a.NCACVAC132D_Id == data.NCACVAC132D_Id)
                                    select new NAAC_AC_VAC_DTO
                                    {
                                        NCACVAC132DC_Remarks = a.NCACVAC132DC_Remarks,
                                        NCACVAC132DC_Id = a.NCACVAC132DC_Id,
                                        NCACVAC132DC_RemarksBy = a.NCACVAC132DC_RemarksBy,
                                        NCACVAC132DC_StatusFlg = a.NCACVAC132DC_StatusFlg,
                                        NCACVAC132DC_ActiveFlag = a.NCACVAC132DC_ActiveFlag,
                                        NCACVAC132DC_CreatedBy = a.NCACVAC132DC_CreatedBy,
                                        NCACVAC132DC_CreatedDate = a.NCACVAC132DC_CreatedDate,
                                        NCACVAC132DC_UpdatedBy = a.NCACVAC132DC_UpdatedBy,
                                        NCACVAC132DC_UpdatedDate = a.NCACVAC132DC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACVAC132DC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_VAC_DTO getfilecomment(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACVAC132DFC_RemarksBy == b.Id && a.NCACVAC132DF_Id == data.NCACVAC132DF_Id)
                                     select new NAAC_AC_VAC_DTO
                                     {
                                         NCACVAC132DF_Id = a.NCACVAC132DF_Id,
                                         NCACVAC132DFC_Remarks = a.NCACVAC132DFC_Remarks,
                                         NCACVAC132DFC_Id = a.NCACVAC132DFC_Id,
                                         NCACVAC132DFC_RemarksBy = a.NCACVAC132DFC_RemarksBy,
                                         NCACVAC132DFC_StatusFlg = a.NCACVAC132DFC_StatusFlg,
                                         NCACVAC132DFC_ActiveFlag = a.NCACVAC132DFC_ActiveFlag,
                                         NCACVAC132DFC_CreatedBy = a.NCACVAC132DFC_CreatedBy,
                                         NCACVAC132DFC_CreatedDate = a.NCACVAC132DFC_CreatedDate,
                                         NCACVAC132DFC_UpdatedBy = a.NCACVAC132DFC_UpdatedBy,
                                         NCACVAC132DFC_UpdatedDate = a.NCACVAC132DFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACVAC132DFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO savemedicaldatawisecomments(NAAC_AC_VAC_DTO data)
        {
            try
            {
                NAAC_AC_VAC_132_Details_Comments_DMO obj1 = new NAAC_AC_VAC_132_Details_Comments_DMO();
                obj1.NCACVAC132DC_Remarks = data.Remarks;
                obj1.NCACVAC132DC_RemarksBy = data.UserId;
                obj1.NCACVAC132DC_StatusFlg = "";
                obj1.NCACVAC132D_Id = data.filefkid;
                obj1.NCACVAC132DC_ActiveFlag = true;
                obj1.NCACVAC132DC_CreatedBy = data.UserId;
                obj1.NCACVAC132DC_UpdatedBy = data.UserId;
                obj1.NCACVAC132DC_CreatedDate = DateTime.Now;
                obj1.NCACVAC132DC_UpdatedDate = DateTime.Now;
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

        // for file adding
        public NAAC_AC_VAC_DTO savefilewisecomments(NAAC_AC_VAC_DTO data)
        {
            try
            {
                NAAC_AC_VAC_132_Details_File_Comments_DMO obj1 = new NAAC_AC_VAC_132_Details_File_Comments_DMO();
                obj1.NCACVAC132DFC_Remarks = data.Remarks;
                obj1.NCACVAC132DFC_RemarksBy = data.UserId;
                obj1.NCACVAC132DFC_StatusFlg = "";
                obj1.NCACVAC132DF_Id = data.filefkid;
                obj1.NCACVAC132DFC_ActiveFlag = true;
                obj1.NCACVAC132DFC_CreatedBy = data.UserId;
                obj1.NCACVAC132DFC_UpdatedBy = data.UserId;
                obj1.NCACVAC132DFC_UpdatedDate = DateTime.Now;
                obj1.NCACVAC132DFC_CreatedDate = DateTime.Now;
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

        public NAAC_AC_VAC_DTO get_Mappedstudentlist(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.mappedstudentlist = (from a in _GeneralContext.NAAC_AC_VAC_132_DMO
                                          from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                          from c in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                          from d in _GeneralContext.Adm_Master_College_StudentDMO
                                          from y in _GeneralContext.Adm_College_Yearly_StudentDMO
                                          where (y.ASMAY_Id == data.NCACVAC132D_Year && y.AMCST_Id == d.AMCST_Id && a.NCACVAC132_Id == data.NCACVAC132_Id && b.NCACVAC132D_Year == data.NCACVAC132D_Year && a.MI_Id == data.MI_Id && a.NCACVAC132_Id == b.NCACVAC132_Id && b.NCACVAC132D_Id == c.NCACVAC132D_Id && d.AMCST_Id == c.AMCST_Id && d.AMCST_SOL == "S")
                                          select new NAAC_AC_VAC_DTO
                                          {
                                              AMCST_Id = c.AMCST_Id,
                                              NCACVAC132D_Id = b.NCACVAC132D_Id,
                                              ASMAY_Id = d.ASMAY_Id,
                                              studentname = ((d.AMCST_FirstName == null ? " " : d.AMCST_FirstName) + " " + (d.AMCST_MiddleName == null ? " " : d.AMCST_MiddleName) + " " + (d.AMCST_LastName == null ? " " : d.AMCST_LastName)).Trim(),
                                              AMCST_AdmNo = d.AMCST_AdmNo,
                                              NCACVAC132_CourseName = a.NCACVAC132_CourseName,
                                              NCACVAC132_CourseCode = a.NCACVAC132_CourseCode,
                                              NCACVAC132DS_ActiveFlg = c.NCACVAC132DS_ActiveFlg,
                                              NCACVAC132DS_Id = c.NCACVAC132DS_Id,
                                          }).Distinct().OrderBy(t => t.studentname).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public NAAC_AC_VAC_DTO get_Continuedflagdata(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var get_Continuedflagdata = _GeneralContext.NAAC_AC_VAC_132_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACVAC132_Id == data.NCACVAC132_Id).Distinct().ToList();

                data.get_Continuedflagdata = get_Continuedflagdata.ToArray();
                long s = 0;
                if (get_Continuedflagdata.Count > 0)
                {
                    s = get_Continuedflagdata.SingleOrDefault().NCACVAC132_IntroYear;
                }

                int year_order = (from a in _GeneralContext.Academic
                                  where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == s)
                                  select a.ASMAY_Order).SingleOrDefault();
               
                data.discontyearlist = (from t in _GeneralContext.Academic
                                        where (t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Order >= year_order)
                                        select new NAAC_AC_VAC_DTO
                                        {
                                            discontid = t.ASMAY_Id,
                                            discontyearnm = t.ASMAY_Year,
                                        }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO saveContinued(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var update2 = _GeneralContext.NAAC_AC_VAC_132_DMO.Single(t => t.MI_Id == data.MI_Id && t.NCACVAC132_Id == data.NCACVAC132_Id);
                update2.NCACVAC132_DiscontinuedFlg = true;
                update2.NCACVAC132_DiscontinuedYear = data.NCACVAC132_DiscontinuedYear;
                update2.NCACVAC132_UpdatedBy = data.UserId;
                update2.NCACVAC132_UpdatedDate = DateTime.Now;
                _GeneralContext.Update(update2);
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
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
        public NAAC_AC_VAC_DTO get_Completedflagdata(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var comStudentFdata = (from a in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                       from b in _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO
                                       from c in _GeneralContext.NAAC_AC_VAC_132_DMO
                                       where (a.NCACVAC132_Id == data.NCACVAC132_Id && b.NCACVAC132DS_Id == data.NCACVAC132DS_Id && a.NCACVAC132D_Id == b.NCACVAC132D_Id && c.NCACVAC132_Id == a.NCACVAC132_Id)
                                       select new NAAC_AC_VAC_DTO
                                       {
                                           NCACVAC132D_Id = a.NCACVAC132D_Id,
                                           NCACVAC132D_NoOfStudentsEnr = a.NCACVAC132D_NoOfStudentsEnr,
                                           AMCST_Id = b.AMCST_Id,
                                           NCACVAC132DS_Id = b.NCACVAC132DS_Id,
                                           NCACVAC132D_Year = a.NCACVAC132D_Year,
                                       }).Distinct().ToList();
                data.countOfStudentEntry = comStudentFdata.ToArray();
                List<long> amcstids = new List<long>();
                if (comStudentFdata.Count > 0)
                {
                    foreach (var item in comStudentFdata)
                    {
                        amcstids.Add(item.NCACVAC132D_Year);
                    }
                }

                data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    where (b.AMCST_Id == a.AMCST_Id && a.MI_Id == data.MI_Id && amcstids.Contains(b.ASMAY_Id) && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true)
                                    select new NAAC_AC_VAC_DTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();

               // data.comStudentFdata = comStudentFdata.ToArray();
                long s = 0;
                if (comStudentFdata.Count > 0)
                {
                    s = comStudentFdata.SingleOrDefault().NCACVAC132D_Year;
                }

                if (s > 0)
                {
                    int year_order = (from a in _GeneralContext.Academic
                                      where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == s)
                                      select a.ASMAY_Order).SingleOrDefault();

                    data.completedyearlist = (from y in _GeneralContext.Academic
                                              where (y.MI_Id == data.MI_Id && y.Is_Active == true && y.ASMAY_Order >= year_order)
                                              select new NAAC_AC_VAC_DTO
                                              {
                                                  completedyearid = y.ASMAY_Id,
                                                  completedyearname = y.ASMAY_Year,
                                              }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                }
                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO saveCompletedflag(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_VAC_132_Details_DMO.Single(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id);

                result.NCACVAC132D_NoOfStdCompleted = data.NCACVAC132D_NoOfStdCompleted;
                result.NCACVAC132D_UpdatedBy = data.UserId;
                result.NCACVAC132D_UpdatedDate = DateTime.Now;

                _GeneralContext.Update(result);

                if (data.studentlstdata.Length > 0)
                {
                    for (int i = 0; i < data.studentlstdata.Length; i++)
                    {
                        var obj3 = _GeneralContext.NAAC_AC_VAC_132_Details_Students_DMO.Single(t => t.NCACVAC132D_Id == data.NCACVAC132D_Id && t.NCACVAC132DS_Id == data.NCACVAC132DS_Id);
                        obj3.NCACVAC132D_Id = result.NCACVAC132D_Id;
                        //obj3.AMCST_Id = data.studentlstdata[i].AMCST_Id;
                        obj3.NCACVAC132DS_CompletedFlg = true;
                        obj3.NCACVAC132DS_CompletedYear = data.NCACVAC132DS_CompletedYear;
                        obj3.NCACVAC132DS_UpdatedBy = data.UserId;
                        obj3.NCACVAC132DS_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(obj3);
                    }

                }
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO viewuploadfliesmain(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.viewuploadfliesmain = (from t in _GeneralContext.NAAC_AC_VAC_132_Files_DMO
                                            from b in _GeneralContext.NAAC_AC_VAC_132_DMO
                                            where (t.NCACVAC132_Id == data.NCACVAC132_Id && t.NCACVAC132_Id == b.NCACVAC132_Id && b.MI_Id == data.MI_Id&&t.NCACVAC132F_ActiveFlg==true)
                                            select new NAAC_AC_VAC_DTO
                                            {
                                                cfilename = t.NCACVAC132F_FileName,
                                                cfilepath = t.NCACVAC132F_FilePath,
                                                cfiledesc = t.NCACVAC132F_Filedesc,
                                                NCACVAC132F_Id = t.NCACVAC132F_Id,
                                                NCACVAC132_Id = b.NCACVAC132_Id,
                                                NCACVAC132F_StatusFlg = t.NCACVAC132F_StatusFlg,
                                            }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO deletemainfile(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_AC_VAC_132_Files_DMO.Where(t => t.NCACVAC132F_Id == data.NCACVAC132F_Id).SingleOrDefault();
                res.NCACVAC132F_ActiveFlg = false;
                _GeneralContext.Update(res);

                //var result = _GeneralContext.NAAC_AC_VAC_132_Files_DMO.Where(t => t.NCACVAC132F_Id == data.NCACVAC132F_Id).ToList();


                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _GeneralContext.Remove(resultid);
                //    }
                //}
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.deletemainfile = (from t in _GeneralContext.NAAC_AC_VAC_132_Files_DMO
                                       from b in _GeneralContext.NAAC_AC_VAC_132_DMO
                                       where (t.NCACVAC132_Id == data.NCACVAC132_Id && t.NCACVAC132_Id == b.NCACVAC132_Id && b.MI_Id == data.MI_Id&&t.NCACVAC132F_ActiveFlg==true)
                                       select new NAAC_AC_VAC_DTO
                                       {
                                           cfilename = t.NCACVAC132F_FileName,
                                           cfilepath = t.NCACVAC132F_FilePath,
                                           cfiledesc = t.NCACVAC132F_Filedesc,
                                           NCACVAC132F_Id = t.NCACVAC132F_Id,
                                           NCACVAC132_Id = b.NCACVAC132_Id,
                                           NCACVAC132F_StatusFlg = t.NCACVAC132F_StatusFlg,
                                       }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO viewuploadfliesstudent(NAAC_AC_VAC_DTO data)
        {
            try
            {
                data.viewuploadfliesstudent = (from t in _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO
                                               from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                               from c in _GeneralContext.NAAC_AC_VAC_132_DMO
                                               where (c.MI_Id == data.MI_Id && c.NCACVAC132_Id == b.NCACVAC132_Id && t.NCACVAC132D_Id == data.NCACVAC132D_Id && t.NCACVAC132D_Id == b.NCACVAC132D_Id&&t.NCACVAC132DF_ActiveFlg==true)
                                               select new NAAC_AC_VAC_DTO
                                               {
                                                   cfilename = t.NCACVAC132DF_FileName,
                                                   cfilepath = t.NCACVAC132DF_FilePath,
                                                   cfiledesc = t.NCACVAC132DF_Filedesc,
                                                   NCACVAC132DF_Id = t.NCACVAC132DF_Id,
                                                   NCACVAC132D_Id = b.NCACVAC132D_Id,
                                                   NCACVAC132DF_StatusFlg = t.NCACVAC132DF_StatusFlg,
                                               }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_VAC_DTO deletestudentfiles(NAAC_AC_VAC_DTO data)
        {
            try
            {
                var res = _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO.Where(t => t.NCACVAC132DF_Id == data.NCACVAC132DF_Id).SingleOrDefault();
                res.NCACVAC132DF_ActiveFlg = false;
                _GeneralContext.Update(res);


                //var result = _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO.Where(t => t.NCACVAC132DF_Id == data.NCACVAC132DF_Id).ToList();
                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _GeneralContext.Remove(resultid);
                //    }
                //}
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadfliesstudent = (from t in _GeneralContext.NAAC_AC_VAC_132_Details_FilesDMO
                                               from b in _GeneralContext.NAAC_AC_VAC_132_Details_DMO
                                               from c in _GeneralContext.NAAC_AC_VAC_132_DMO
                                               where (t.NCACVAC132D_Id == data.NCACVAC132D_Id && t.NCACVAC132D_Id == b.NCACVAC132D_Id && c.MI_Id == data.MI_Id && c.NCACVAC132_Id == b.NCACVAC132_Id&&t.NCACVAC132DF_ActiveFlg==true)
                                               select new NAAC_AC_VAC_DTO
                                               {
                                                   cfilename = t.NCACVAC132DF_FileName,
                                                   cfilepath = t.NCACVAC132DF_FilePath,
                                                   cfiledesc = t.NCACVAC132DF_Filedesc,
                                                   NCACVAC132DF_Id = t.NCACVAC132DF_Id,
                                                   NCACVAC132D_Id = b.NCACVAC132D_Id,
                                                   NCACVAC132DF_StatusFlg = t.NCACVAC132DF_StatusFlg,
                                               }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }



    }
}
