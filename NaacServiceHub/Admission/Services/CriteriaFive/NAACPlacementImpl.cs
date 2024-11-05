using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACPlacementImpl : Interface.NAACPlacementInterface
    {
        public GeneralContext _context;
        public NAACPlacementImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACPlacementDTO loaddata(NAACPlacementDTO data)
        {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                /////////////////
                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.NAAC_AC_521_PlacementDMO
                                    from c in _context.MasterCourseDMO
                                    from d in _context.ClgMasterBranchDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC521PLA_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && b.NCAC521PLA_GradCourse==c.AMCO_Id && b.NCAC521PLA_GradBranch==d.AMB_Id)
                                    select new NAACPlacementDTO
                                    {
                                        NCAC521PLA_Id = b.NCAC521PLA_Id,
                                        MI_Id = b.MI_Id,
                                        NCAC521PLA_EmployerName = b.NCAC521PLA_EmployerName,
                                        NCAC521PLA_NoOfStudents = b.NCAC521PLA_NoOfStudents,
                                        NCAC521PLA_Package = b.NCAC521PLA_Package,
                                        NCAC521PLA_ActiveFlg = b.NCAC521PLA_ActiveFlg,
                                        AMCO_CourseName = c.AMCO_CourseName,
                                        AMB_BranchName = d.AMB_BranchName,
                                        NCAC521PLA_NoOfstudentsselfemployed = b.NCAC521PLA_NoOfstudentsselfemployed,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC521PLA_StatusFlg = b.NCAC521PLA_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC521PLA_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACPlacementDTO save(NAACPlacementDTO data)
        {
            try
            {
                if (data.NCAC521PLA_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_521_PlacementDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC521PLA_EmployerName == data.NCAC521PLA_EmployerName && t.NCAC521PLA_Year == data.ASMAY_Id && t.NCAC521PLA_GradCourse==data.AMCO_Id && t.NCAC521PLA_GradBranch==data.AMB_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_521_PlacementDMO obj1 = new NAAC_AC_521_PlacementDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC521PLA_Year = data.ASMAY_Id;
                        obj1.NCAC521PLA_EmployerName = data.NCAC521PLA_EmployerName;
                        obj1.NCAC521PLA_NoOfStudents = data.NCAC521PLA_NoOfStudents;
                        obj1.NCAC521PLA_NoOfstudentsselfemployed = data.NCAC521PLA_NoOfstudentsselfemployed;
                        obj1.NCAC521PLA_Package = data.NCAC521PLA_Package;
                        obj1.NCAC521PLA_GradCourse = data.AMCO_Id;
                        obj1.NCAC521PLA_GradBranch = data.AMB_Id;
                        obj1.NCAC521PLA_ActiveFlg = true;
                        obj1.NCAC521PLA_CreatedBy = data.UserId;
                        obj1.NCAC521PLA_UpdatedBy = data.UserId;
                        obj1.NCAC521PLA_CreatedDate = DateTime.Now;
                        obj1.NCAC521PLA_UpdatedDate = DateTime.Now;
                        obj1.NCAC521PLA_StatusFlg = "";
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_521_PlacementFilesDMO obb = new NAAC_AC_521_PlacementFilesDMO();
                                    
                                    obb.NCAC521PLA_Id = obj1.NCAC521PLA_Id;
                                    obb.NCAC521PLAF_FileName = item.cfilename;
                                    obb.NCAC521PLAF_FilePath = item.cfilepath;
                                    obb.NCAC521PLAF_Filedesc = item.cfiledesc;
                                    obb.NCAC521PLAF_StatusFlg = "";
                                    obb.NCAC521PLAF_ActiveFlg = true;

                                    _context.Add(obb);
                                }


                            }
                        }

                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval =false;
                        }
                    }
                }
                else if (data.NCAC521PLA_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_521_PlacementDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC521PLA_EmployerName == data.NCAC521PLA_EmployerName && t.NCAC521PLA_Year == data.ASMAY_Id && t.NCAC521PLA_GradCourse == data.AMCO_Id && t.NCAC521PLA_GradBranch == data.AMB_Id && t.NCAC521PLA_Id != data.NCAC521PLA_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                       
                            var update = _context.NAAC_AC_521_PlacementDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC521PLA_Id == data.NCAC521PLA_Id).SingleOrDefault();
                        update.NCAC521PLA_Year = data.ASMAY_Id;
                        update.NCAC521PLA_EmployerName = data.NCAC521PLA_EmployerName;
                        update.NCAC521PLA_NoOfStudents = data.NCAC521PLA_NoOfStudents;
                        update.NCAC521PLA_NoOfstudentsselfemployed = data.NCAC521PLA_NoOfstudentsselfemployed;
                        update.NCAC521PLA_Package = data.NCAC521PLA_Package;
                        update.NCAC521PLA_GradCourse = data.AMCO_Id;
                        update.NCAC521PLA_GradBranch = data.AMB_Id;
                        update.NCAC521PLA_ActiveFlg = true;
                        update.NCAC521PLA_UpdatedBy = data.UserId;
                        update.NCAC521PLA_UpdatedDate = DateTime.Now;
                        _context.Update(update);



                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_521_PlacementFilesDMO.Where(t => t.NCAC521PLA_Id == data.NCAC521PLA_Id && !Fid.Contains(t.NCAC521PLAF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_521_PlacementFilesDMO.Single(t => t.NCAC521PLA_Id == data.NCAC521PLA_Id && t.NCAC521PLAF_Id == item2.NCAC521PLAF_Id);
                                    deactfile.NCAC521PLAF_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }

                            }



                            foreach (var item in data.filelist)
                            {
                                if (item.status == null)
                                {
                                    item.status = "";
                                }

                                if (item.cfileid > 0 && item.status.ToLower() != "approved")
                                {
                                    var filesdata = _context.NAAC_AC_521_PlacementFilesDMO.Where(t => t.NCAC521PLAF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC521PLA_Id = data.NCAC521PLA_Id;
                                    filesdata.NCAC521PLAF_FileName = item.cfilename;
                                    filesdata.NCAC521PLAF_FilePath = item.cfilepath;
                                    filesdata.NCAC521PLAF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC521PLAF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_521_PlacementFilesDMO obb = new NAAC_AC_521_PlacementFilesDMO();
                                            obb.NCAC521PLA_Id = data.NCAC521PLA_Id;
                                            obb.NCAC521PLAF_FileName = item.cfilename;
                                            obb.NCAC521PLAF_FilePath = item.cfilepath;
                                            obb.NCAC521PLAF_Filedesc = item.cfiledesc;
                                            obb.NCAC521PLAF_ActiveFlg = true;
                                            obb.NCAC521PLAF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_521_PlacementFilesDMO.Where(t => t.NCAC521PLA_Id == data.NCAC521PLA_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_521_PlacementFilesDMO.Single(t => t.NCAC521PLA_Id == data.NCAC521PLA_Id && t.NCAC521PLAF_Id == item.NCAC521PLAF_Id);
                                    deactfile.NCAC521PLAF_ActiveFlg = false;
                                    _context.Update(removefile1);
                                }
                            }
                        }

                        int y = _context.SaveChanges();
                        if (y > 0)
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
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACPlacementDTO deactiveStudent(NAACPlacementDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_521_PlacementDMO.Where(t => t.NCAC521PLA_Id == data.NCAC521PLA_Id).SingleOrDefault();
                if (u.NCAC521PLA_ActiveFlg == true)
                {
                    u.NCAC521PLA_ActiveFlg = false;
                }
                else if (u.NCAC521PLA_ActiveFlg == false)
                {
                    u.NCAC521PLA_ActiveFlg = true;
                }
                u.NCAC521PLA_UpdatedDate = DateTime.Now;
                u.NCAC521PLA_UpdatedBy = data.UserId;
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
        public NAACPlacementDTO EditData(NAACPlacementDTO data)
        {
            try
            {
              

                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_521_PlacementDMO
                                 from c in _context.MasterCourseDMO
                                 from d in _context.ClgMasterBranchDMO
                 where (a.MI_Id == b.MI_Id && b.NCAC521PLA_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && b.NCAC521PLA_GradCourse == c.AMCO_Id && b.NCAC521PLA_GradBranch == d.AMB_Id && b.NCAC521PLA_Id==data.NCAC521PLA_Id)
                 select new NAACPlacementDTO
                 {
                     NCAC521PLA_Id = b.NCAC521PLA_Id,
                     NCAC521PLA_EmployerName = b.NCAC521PLA_EmployerName,
                     NCAC521PLA_NoOfStudents = b.NCAC521PLA_NoOfStudents,
                     NCAC521PLA_Package = b.NCAC521PLA_Package,
                     NCAC521PLA_NoOfstudentsselfemployed = b.NCAC521PLA_NoOfstudentsselfemployed,
                     NCAC521PLA_ActiveFlg = b.NCAC521PLA_ActiveFlg,
                     AMCO_CourseName = c.AMCO_CourseName,
                     AMB_BranchName = d.AMB_BranchName,
                     NCAC521PLA_GradCourse = b.NCAC521PLA_GradCourse,
                     NCAC521PLA_GradBranch = b.NCAC521PLA_GradBranch,
                     ASMAY_Year = a.ASMAY_Year,
                     ASMAY_Id = a.ASMAY_Id,
                     NCAC521PLA_StatusFlg = b.NCAC521PLA_StatusFlg,
                 }).Distinct().OrderByDescending(t => t.NCAC521PLA_Id).ToArray();



                data.editfiles = (from a in _context.NAAC_AC_521_PlacementFilesDMO

                                  where (a.NCAC521PLA_Id == data.NCAC521PLA_Id && a.NCAC521PLAF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC521PLAF_FileName,
                                      cfilepath = a.NCAC521PLAF_FilePath,
                                      cfiledesc = a.NCAC521PLAF_Filedesc,
                                      cfileid = a.NCAC521PLAF_Id,
                                      status = a.NCAC521PLAF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACPlacementDTO viewuploadflies(NAACPlacementDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_521_PlacementFilesDMO

                                  where (a.NCAC521PLA_Id == data.NCAC521PLA_Id && a.NCAC521PLAF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC521PLA_Id,
                                      cfileid = a.NCAC521PLAF_Id,
                                      cfilename = a.NCAC521PLAF_FileName,
                                      cfilepath = a.NCAC521PLAF_FilePath,
                                      cfiledesc = a.NCAC521PLAF_Filedesc,
                                      status = a.NCAC521PLAF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACPlacementDTO deleteuploadfile(NAACPlacementDTO data)
        {
            try
            {


                if (data.NCAC521PLAF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_521_PlacementFilesDMO.Where(e => e.NCAC521PLAF_Id == data.NCAC521PLAF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC521PLAF_ActiveFlg =false;
                            _context.Update(item);
                        }


                        int y = _context.SaveChanges();
                        if (y > 0)
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
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAACPlacementDTO get_course(NAACPlacementDTO data)
        {
            try
            {
                data.courselist = (from a in _context.MasterCourseDMO
                                   from b in _context.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag==true && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAACPlacementDTO get_branch(NAACPlacementDTO data)
        {
            try
            {
                data.branchlist = (from a in _context.ClgMasterBranchDMO
                                   from b in _context.CLG_Adm_College_AY_CourseDMO
                                   from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                   where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag==true)
                                   select a).Distinct().OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }



        public NAACPlacementDTO getcomment(NAACPlacementDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_521_Placement_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC521PLAC_RemarksBy == b.Id && a.NCAC521PLA_Id == data.NCAC521PLA_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC521PLAC_Remarks,
                                        commentid = a.NCAC521PLAC_Id,
                                        status = a.NCAC521PLAC_StatusFlg,
                                        createddate = a.NCAC521PLAC_CreatedDate,
                                        activeflag = a.NCAC521PLAC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACPlacementDTO getfilecomment(NAACPlacementDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_521_Placement_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC521PLAFC_RemarksBy == b.Id && a.NCAC521PLAF_Id == data.NCAC521PLAF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC521PLAFC_Remarks,
                                        commentid = a.NCAC521PLAFC_Id,
                                        status = a.NCAC521PLAFC_StatusFlg,
                                        createddate = a.NCAC521PLAFC_CreatedDate,
                                        activeflag = a.NCAC521PLAFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACPlacementDTO savemedicaldatawisecomments(NAACPlacementDTO data)
        {
            try
            {
                NAAC_AC_521_Placement_CommentsDMO cm = new NAAC_AC_521_Placement_CommentsDMO();
                cm.NCAC521PLAC_Remarks = data.Remarks;
                cm.NCAC521PLAC_RemarksBy = data.UserId;
                cm.NCAC521PLAC_StatusFlg = "";
                cm.NCAC521PLAC_ActiveFlag = true;
                cm.NCAC521PLAC_CreatedBy = data.UserId;
                cm.NCAC521PLAC_CreatedDate = DateTime.Now;
                cm.NCAC521PLAC_UpdatedBy = data.UserId;
                cm.NCAC521PLAC_UpdatedDate = DateTime.Now;
                cm.NCAC521PLA_Id = data.filefkid;
                _context.Add(cm);
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
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACPlacementDTO savefilewisecomments(NAACPlacementDTO data)
        {
            try
            {
                NAAC_AC_521_Placement_File_CommentsDMO cm = new NAAC_AC_521_Placement_File_CommentsDMO();
                cm.NCAC521PLAFC_Remarks = data.Remarks;
                cm.NCAC521PLAFC_RemarksBy = data.UserId;
                cm.NCAC521PLAFC_StatusFlg = "";
                cm.NCAC521PLAFC_ActiveFlag = true;
                cm.NCAC521PLAFC_CreatedBy = data.UserId;
                cm.NCAC521PLAFC_CreatedDate = DateTime.Now;
                cm.NCAC521PLAFC_UpdatedBy = data.UserId;
                cm.NCAC521PLAFC_UpdatedDate = DateTime.Now;
                cm.NCAC521PLAF_Id = data.filefkid;
                _context.Add(cm);
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
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


    }
}
