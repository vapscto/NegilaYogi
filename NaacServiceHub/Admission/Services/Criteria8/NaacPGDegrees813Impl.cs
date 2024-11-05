using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission.Criteria8;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria8
{
    public class NaacPGDegrees813Impl:Interface.Criteria8.NaacPGDegrees813Interface
    {

        public GeneralContext _context;
        public NaacPGDegrees813Impl(GeneralContext y)
        {
            _context = y;
        }

        public NAAC_MC_813_PGDegrees_DTO loaddata(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_MC_813_PGDegrees_DMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCMC813PGDE_JoinYear == a.ASMAY_Id && b.NCMC813PGDE_CompletedYear==a.ASMAY_Id)
                                 select new NAAC_MC_813_PGDegrees_DTO
                                 {
                                     NCMC813PGDE_Id = b.NCMC813PGDE_Id,
                                     NCMC813PGDE_JoinYear = b.NCMC813PGDE_JoinYear,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMC813PGDE_NoOfTeachers = b.NCMC813PGDE_NoOfTeachers,
                                     NCMC813PGDE_CompletedYear=b.NCMC813PGDE_CompletedYear,
                                     NCMC813PGDE_DegreeFromInst=b.NCMC813PGDE_DegreeFromInst,
                                     NCMC813PGDE_ActiveFlg = b.NCMC813PGDE_ActiveFlg,
                                    MI_Id=data.MI_Id,

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_MC_813_PGDegrees_DTO save(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                if (data.NCMC813PGDE_Id == 0)
                {
                    var duplicate = _context.NAAC_MC_813_PGDegrees_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC813PGDE_JoinYear == data.ASMAY_Id && t.NCMC813PGDE_Id != 0 && t.NCMC813PGDE_NoOfTeachers == data.NCMC813PGDE_NoOfTeachers && t.NCMC813PGDE_CompletedYear==data.ASMAY_Id && t.NCMC813PGDE_DegreeFromInst==data.NCMC813PGDE_DegreeFromInst).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_813_PGDegrees_DMO rrr = new NAAC_MC_813_PGDegrees_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCMC813PGDE_NoOfTeachers = data.NCMC813PGDE_NoOfTeachers;
                        rrr.NCMC813PGDE_CompletedYear = data.NCMC813PGDE_CompletedYear;
                        rrr.NCMC813PGDE_DegreeFromInst = data.NCMC813PGDE_DegreeFromInst;
                        rrr.NCMC813PGDE_JoinYear = data.NCMC813PGDE_JoinYear;
                        rrr.NCMC813PGDE_CreatedDate = DateTime.Now;
                        rrr.NCMC813PGDE_UpdatedDate = DateTime.Now;
                        rrr.NCMC813PGDE_ActiveFlg = true;
                       // rrr.NCMC813PGDE_StatusFlg = "";
                        rrr.NCMC813PGDE_CreatedBy = data.UserId;
                        rrr.NCMC813PGDE_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {

                               
                                NAAC_MC_813_PGDegrees_Files_DMO obj2 = new NAAC_MC_813_PGDegrees_Files_DMO();
                                obj2.NCMC813PGDE_Id = rrr.NCMC813PGDE_Id;
                                obj2.MI_Id = data.MI_Id;
                                obj2.NCMC813PGDEF_FileName = data.filelist[i].cfilename;
                                obj2.NCMC813PGDEF_FileDesc = data.filelist[i].cfiledesc;
                                obj2.NCMC813PGDEF_FilePath = data.filelist[i].cfilepath;
                                obj2.NCMC813PGDEF_CreatedBy = data.UserId;
                                obj2.NCMC813PGDEF_UpdatedBy = data.UserId;
                                obj2.NCMC813PGDEF_CreatedDate= DateTime.Now;
                                obj2.NCMC813PGDEF_UpdatedDate = DateTime.Now;
                                obj2.NCMC813PGDEF_ActiveFlg = true;
                                //obj2.NCMC813PGDEF_StatusFlg = "";
                                _context.Add(obj2);
                                }
                            }
                        }

                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCMC813PGDE_Id > 0)
                {
                    var duplicate = _context.NAAC_MC_813_PGDegrees_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC813PGDE_NoOfTeachers == data.NCMC813PGDE_NoOfTeachers  && t.NCMC813PGDE_JoinYear == data.ASMAY_Id && t.NCMC813PGDE_Id != data.NCMC813PGDE_Id && t.NCMC813PGDE_DegreeFromInst==data.NCMC813PGDE_DegreeFromInst && t.NCMC813PGDE_CompletedYear==data.ASMAY_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_MC_813_PGDegrees_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC813PGDE_Id == data.NCMC813PGDE_Id).SingleOrDefault();

                        yy.NCMC813PGDE_UpdatedBy = data.UserId;
                        yy.NCMC813PGDE_NoOfTeachers = data.NCMC813PGDE_NoOfTeachers;
                        yy.NCMC813PGDE_CompletedYear = data.NCMC813PGDE_CompletedYear;
                        yy.NCMC813PGDE_DegreeFromInst = data.NCMC813PGDE_DegreeFromInst;
                        yy.NCMC813PGDE_UpdatedDate = DateTime.Now;
                        yy.NCMC813PGDE_JoinYear = data.NCMC813PGDE_JoinYear;
                        yy.MI_Id = data.MI_Id;
                        _context.Update(yy);

                        var CountRemoveFiles = _context.NAAC_MC_813_PGDegrees_Files_DMO.Where(t => t.NCMC813PGDE_Id == data.NCMC813PGDE_Id).ToList();
                        if (CountRemoveFiles.Count > 0)
                        {
                            foreach (var RemoveFiles in CountRemoveFiles)
                            {
                                _context.Remove(RemoveFiles);
                            }
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {

                                 

                                    NAAC_MC_813_PGDegrees_Files_DMO obj2 = new NAAC_MC_813_PGDegrees_Files_DMO();
                                    obj2.NCMC813PGDE_Id = yy.NCMC813PGDE_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.NCMC813PGDEF_FileName = data.filelist[i].cfilename;
                                    obj2.NCMC813PGDEF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCMC813PGDEF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCMC813PGDEF_CreatedBy = data.UserId;
                                    obj2.NCMC813PGDEF_UpdatedBy = data.UserId;
                                        obj2.NCMC813PGDEF_ActiveFlg = true;
                                        
                                    obj2.NCMC813PGDEF_CreatedDate = DateTime.Now;
                                    obj2.NCMC813PGDEF_UpdatedDate = DateTime.Now;
                                    _context.Add(obj2);
                                    }
                                }
                            }
                        }



                        else if (CountRemoveFiles.Count == 0)
                        {
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {

                                  
                                    NAAC_MC_813_PGDegrees_Files_DMO obj2 = new NAAC_MC_813_PGDegrees_Files_DMO();
                                    obj2.NCMC813PGDE_Id = yy.NCMC813PGDE_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.NCMC813PGDEF_FileName = data.filelist[i].cfilename;
                                    obj2.NCMC813PGDEF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCMC813PGDEF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCMC813PGDEF_CreatedBy = data.UserId;
                                    obj2.NCMC813PGDEF_UpdatedBy = data.UserId;
                                    obj2.NCMC813PGDEF_CreatedDate = DateTime.Now;
                                        obj2.NCMC813PGDEF_ActiveFlg = true;
                                        
                                    obj2.NCMC813PGDEF_UpdatedDate = DateTime.Now;
                                    _context.Add(obj2);
                                    }
                                }
                            }
                        }
                       
                        var r = _context.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_813_PGDegrees_DTO deactive(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                var u = _context.NAAC_MC_813_PGDegrees_DMO.Where(t => t.NCMC813PGDE_Id == data.NCMC813PGDE_Id).SingleOrDefault();
                if (u.NCMC813PGDE_ActiveFlg == true)
                {
                    u.NCMC813PGDE_ActiveFlg = false;
                }
                else if (u.NCMC813PGDE_ActiveFlg == false)
                {
                    u.NCMC813PGDE_ActiveFlg = true;
                }
                u.NCMC813PGDE_UpdatedDate = DateTime.Now;
                u.NCMC813PGDE_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _context.Update(u);
                int o = _context.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_MC_813_PGDegrees_DTO EditData(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_MC_813_PGDegrees_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMC813PGDE_JoinYear && b.MI_Id == data.MI_Id && b.NCMC813PGDE_Id == data.NCMC813PGDE_Id && a.ASMAY_Id==b.NCMC813PGDE_CompletedYear)
                                 select new NAAC_MC_813_PGDegrees_DTO
                                 {
                                     NCMC813PGDE_Id = b.NCMC813PGDE_Id,
                                     NCMC813PGDE_NoOfTeachers = b.NCMC813PGDE_NoOfTeachers,
                                     NCMC813PGDE_JoinYear = b.NCMC813PGDE_JoinYear,
                                     NCMC813PGDE_CompletedYear = b.NCMC813PGDE_CompletedYear,
                                     NCMC813PGDE_DegreeFromInst = b.NCMC813PGDE_DegreeFromInst,
                                    MI_Id=data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMC813PGDE_StatusFlg = b.NCMC813PGDE_StatusFlg,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_MC_813_PGDegrees_Files_DMO
                                      where (a.NCMC813PGDE_Id == data.NCMC813PGDE_Id)
                                      select new NAAC_MC_813_PGDegrees_DTO
                                      {
                                          cfilename = a.NCMC813PGDEF_FileName,
                                          cfilepath = a.NCMC813PGDEF_FilePath,
                                          cfiledesc = a.NCMC813PGDEF_FileDesc,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAAC_MC_813_PGDegrees_DTO viewuploadflies(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_MC_813_PGDegrees_Files_DMO
                                        where (a.NCMC813PGDE_Id == data.NCMC813PGDE_Id && a.NCMC813PGDEF_ActiveFlg==true)
                                        select new NAAC_MC_813_PGDegrees_DTO
                                        {
                                            NCMC813PGDE_Id = a.NCMC813PGDE_Id,
                                            NCMC813PGDEF_Id = a.NCMC813PGDEF_Id,
                                            cfilename = a.NCMC813PGDEF_FileName,
                                            cfilepath = a.NCMC813PGDEF_FilePath,
                                            cfiledesc = a.NCMC813PGDEF_FileDesc,
                                          
                                           
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_MC_813_PGDegrees_DTO deleteuploadfile(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                var res = _context.NAAC_MC_813_PGDegrees_Files_DMO.Where(t => t.NCMC813PGDEF_Id == data.NCMC813PGDEF_Id).SingleOrDefault();
                // _context.Remove(res);
                res.NCMC813PGDEF_ActiveFlg = false;
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _context.NAAC_MC_813_PGDegrees_Files_DMO
                                        where (a.NCMC813PGDE_Id == data.NCMC813PGDE_Id && a.NCMC813PGDEF_ActiveFlg==true)
                                        select new NAAC_MC_813_PGDegrees_DTO
                                        {
                                            NCMC813PGDE_Id = a.NCMC813PGDE_Id,
                                            NCMC813PGDEF_Id = a.NCMC813PGDEF_Id,
                                            cfilename = a.NCMC813PGDEF_FileName,
                                            cfilepath = a.NCMC813PGDEF_FilePath,
                                            cfiledesc = a.NCMC813PGDEF_FileDesc,
                                          
                                           
                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //public NAAC_MC_813_PGDegrees_DTO checkyear(NAAC_MC_813_PGDegrees_DTO data)
        //{
        //    if (data.ASMAY_Id!=0)
        //    {
        //        string check = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.ASMAY_Year).Distinct().SingleOrDefault();

        //        string check2= _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.NCMC813PGDE_CompletedYear).Select(t => t.ASMAY_Year).Distinct().SingleOrDefault();

        //        if (check!="" || check2 !="")
        //        {
        //            if (check2 >= check )
        //            {

        //            }
        //        }
        //    }

        //    return data;
        //}
        public NAAC_MC_813_PGDegrees_DTO getcomment(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_MC_813_PGDegrees_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCMC813PGDEC_RemarksBy == b.Id && a.NCMC813PGDE_Id == data.NCMC813PGDE_Id)
                                    select new NAAC_MC_813_PGDegrees_DTO
                                    {

                                        NCMC813PGDEC_Remarks = a.NCMC813PGDEC_Remarks,
                                        NCMC813PGDE_Id = a.NCMC813PGDE_Id,
                                        NCMC813PGDEC_RemarksBy = a.NCMC813PGDEC_RemarksBy,
                                        NCMC813PGDEC_StatusFlg = a.NCMC813PGDEC_StatusFlg,
                                        NCMC813PGDEC_ActiveFlag = a.NCMC813PGDEC_ActiveFlag,
                                        NCMC813PGDEC_CreatedBy = a.NCMC813PGDEC_CreatedBy,
                                        NCMC813PGDEC_CreatedDate = a.NCMC813PGDEC_CreatedDate,
                                        NCMC813PGDEC_UpdatedBy = a.NCMC813PGDEC_UpdatedBy,
                                        NCMC813PGDEC_UpdatedDate = a.NCMC813PGDEC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_813_PGDegrees_DTO getfilecomment(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_MC_813_PGDegrees_File_CommentsDMO
                                     from b in _context.ApplUser
                                     where (a.NCMC813PGDEFC_RemarksBy == b.Id && a.NCMC813PGDEF_Id == data.NCMC813PGDEF_Id)
                                     select new NAAC_MC_813_PGDegrees_DTO
                                     {
                                         NCMC813PGDEF_Id = a.NCMC813PGDEF_Id,
                                         NCMC813PGDEFC_Remarks = a.NCMC813PGDEFC_Remarks,
                                         NCMC813PGDEFC_Id = a.NCMC813PGDEFC_Id,
                                         NCMC813PGDEFC_RemarksBy = a.NCMC813PGDEFC_RemarksBy,
                                         NCMC813PGDEFC_StatusFlg = a.NCMC813PGDEFC_StatusFlg,
                                         NCMC813PGDEFC_ActiveFlag = a.NCMC813PGDEFC_ActiveFlag,
                                         NCMC813PGDEFC_CreatedBy = a.NCMC813PGDEFC_CreatedBy,
                                         NCMC813PGDEFC_CreatedDate = a.NCMC813PGDEFC_CreatedDate,
                                         NCMC813PGDEFC_UpdatedBy = a.NCMC813PGDEFC_UpdatedBy,
                                         NCMC813PGDEFC_UpdatedDate = a.NCMC813PGDEFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_813_PGDegrees_DTO savecomments(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                NAAC_MC_813_PGDegrees_CommentsDMO obj1 = new NAAC_MC_813_PGDegrees_CommentsDMO();
                obj1.NCMC813PGDEC_Remarks = data.Remarks;
                obj1.NCMC813PGDEC_RemarksBy = data.UserId;
                obj1.NCMC813PGDEC_StatusFlg = "";
                obj1.NCMC813PGDE_Id = data.filefkid;
                obj1.NCMC813PGDEC_ActiveFlag = true;
                obj1.NCMC813PGDEC_CreatedBy = data.UserId;
                obj1.NCMC813PGDEC_UpdatedBy = data.UserId;
                obj1.NCMC813PGDEC_CreatedDate = DateTime.Now;
                obj1.NCMC813PGDEC_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_813_PGDegrees_DTO savefilewisecomments(NAAC_MC_813_PGDegrees_DTO data)
        {
            try
            {
                NAAC_MC_813_PGDegrees_File_CommentsDMO obj1 = new NAAC_MC_813_PGDegrees_File_CommentsDMO();
                obj1.NCMC813PGDEFC_Remarks = data.Remarks;
                obj1.NCMC813PGDEFC_RemarksBy = data.UserId;
                obj1.NCMC813PGDEFC_StatusFlg = "";
                obj1.NCMC813PGDEF_Id = data.filefkid;
                obj1.NCMC813PGDEFC_ActiveFlag = true;
                obj1.NCMC813PGDEFC_CreatedBy = data.UserId;
                obj1.NCMC813PGDEFC_UpdatedBy = data.UserId;
                obj1.NCMC813PGDEFC_CreatedDate = DateTime.Now;
                obj1.NCMC813PGDEFC_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
