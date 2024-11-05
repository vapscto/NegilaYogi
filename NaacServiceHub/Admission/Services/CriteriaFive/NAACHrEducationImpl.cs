using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACHrEducationImpl : Interface.NAACHrEducationInterface
    {
        public GeneralContext _context;
        public NAACHrEducationImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACHrEducationDTO loaddata(NAACHrEducationDTO data)
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
                                    from b in _context.NAAC_AC_522_HrEducationDMO
                                    from c in _context.MasterCourseDMO
                                    from d in _context.ClgMasterBranchDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC522HRED_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && b.NCAC522HRED_GraduatedProgram==c.AMCO_Id && b.NCAC522HRED_GraduatedDept==d.AMB_Id)
                                    select new NAACHrEducationDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC522HRED_Id = b.NCAC522HRED_Id,
                                        NCAC522HRED_HrEduEnrollStudentNo = b.NCAC522HRED_HrEduEnrollStudentNo,
                                        NCAC522HRED_AdmittedDept = b.NCAC522HRED_AdmittedDept,
                                        NCAC522HRED_AdmittedProgram = b.NCAC522HRED_AdmittedProgram,
                                        NCAC522HRED_ActiveFlg = b.NCAC522HRED_ActiveFlg,
                                        NCAC522HRED_InstitutionName = b.NCAC522HRED_InstitutionName,
                                        AMCO_CourseName = c.AMCO_CourseName,
                                        AMCO_Id = c.AMCO_Id,
                                        AMB_Id = d.AMB_Id,
                                        AMB_BranchName = d.AMB_BranchName,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC522HRED_StatusFlg = b.NCAC522HRED_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC522HRED_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACHrEducationDTO save(NAACHrEducationDTO data)
        {
            try
            {
                if (data.NCAC522HRED_Id == 0)
                {
                    //var duplicate = _context.NAAC_AC_522_HrEducationDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC521PLA_EmployerName == data.NCAC521PLA_EmployerName && t.NCAC521PLA_Year == data.ASMAY_Id && t.NCAC521PLA_GradCourse==data.AMCO_Id && t.NCAC521PLA_GradBranch==data.AMB_Id).ToList();
                    //if (duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        NAAC_AC_522_HrEducationDMO obj1 = new NAAC_AC_522_HrEducationDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC522HRED_Year = data.ASMAY_Id;
                    obj1.NCAC522HRED_HrEduEnrollStudentNo = data.NCAC522HRED_HrEduEnrollStudentNo;
                    obj1.NCAC522HRED_AdmittedDept = data.NCAC522HRED_AdmittedDept;
                    obj1.NCAC522HRED_AdmittedProgram = data.NCAC522HRED_AdmittedProgram;
                    obj1.NCAC522HRED_ActiveFlg = true;
                    obj1.NCAC522HRED_InstitutionName = data.NCAC522HRED_InstitutionName;
                    obj1.NCAC522HRED_GraduatedProgram = data.AMCO_Id;
                    obj1.NCAC522HRED_GraduatedDept = data.AMB_Id;
                    obj1.NCAC522HRED_StatusFlg = "";
                    obj1.NCAC522HRED_CreatedBy = data.UserId;
                        obj1.NCAC522HRED_UpdatedBy = data.UserId;
                        obj1.NCAC522HRED_CreatedDate = DateTime.Now;
                        obj1.NCAC522HRED_UpdatedDate = DateTime.Now;
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_522_HrEducationFilesDMO obb = new NAAC_AC_522_HrEducationFilesDMO();
                                    
                                    obb.NCAC522HRED_Id = obj1.NCAC522HRED_Id;
                                    obb.NCAC522HREDF_FileName = item.cfilename;
                                    obb.NCAC522HREDF_FilePath = item.cfilepath;
                                    obb.NCAC522HREDF_Filedesc = item.cfiledesc;
                                    obb.NCAC522HREDF_StatusFlg = "";
                                    obb.NCAC522HREDF_ActiveFlg = true;

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
                    //}
                }
                else if (data.NCAC522HRED_Id > 0)
                {
                    //var duplicate = _context.NAAC_AC_522_HrEducationDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC521PLA_EmployerName == data.NCAC521PLA_EmployerName && t.NCAC521PLA_Year == data.ASMAY_Id && t.NCAC521PLA_GradCourse == data.AMCO_Id && t.NCAC521PLA_GradBranch == data.AMB_Id && t.NCAC522HRED_Id != data.NCAC522HRED_Id).ToList();
                    //if (duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{
                        
                            var update = _context.NAAC_AC_522_HrEducationDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC522HRED_Id == data.NCAC522HRED_Id).SingleOrDefault();
                    update.NCAC522HRED_Year = data.ASMAY_Id;
                    update.NCAC522HRED_HrEduEnrollStudentNo = data.NCAC522HRED_HrEduEnrollStudentNo;
                    update.NCAC522HRED_AdmittedDept = data.NCAC522HRED_AdmittedDept;
                    update.NCAC522HRED_AdmittedProgram = data.NCAC522HRED_AdmittedProgram;
                    update.NCAC522HRED_InstitutionName = data.NCAC522HRED_InstitutionName;
                    update.NCAC522HRED_GraduatedProgram = data.AMCO_Id;
                    update.NCAC522HRED_GraduatedDept = data.AMB_Id;
                    update.NCAC522HRED_UpdatedBy = data.UserId;
                    update.NCAC522HRED_UpdatedDate = DateTime.Now;
                    _context.Update(update);



                    if (data.filelist.Length > 0)
                    {

                        List<long> Fid = new List<long>();
                        foreach (var item in data.filelist)
                        {
                            Fid.Add(item.cfileid);
                        }
                        var removefile1 = _context.NAAC_AC_522_HrEducationFilesDMO.Where(t => t.NCAC522HRED_Id == data.NCAC522HRED_Id && !Fid.Contains(t.NCAC522HREDF_Id)).Distinct().ToList();

                        if (removefile1.Count > 0)
                        {
                            foreach (var item2 in removefile1)
                            {
                                var deactfile = _context.NAAC_AC_522_HrEducationFilesDMO.Single(t => t.NCAC522HRED_Id == data.NCAC522HRED_Id && t.NCAC522HREDF_Id == item2.NCAC522HREDF_Id);
                                deactfile.NCAC522HREDF_ActiveFlg = false;
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
                                var filesdata = _context.NAAC_AC_522_HrEducationFilesDMO.Where(t => t.NCAC522HREDF_Id == item.cfileid).FirstOrDefault();
                                filesdata.NCAC522HRED_Id = data.NCAC522HRED_Id;
                                filesdata.NCAC522HREDF_FileName = item.cfilename;
                                filesdata.NCAC522HREDF_FilePath = item.cfilepath;
                                filesdata.NCAC522HREDF_Filedesc = item.cfiledesc;
                                filesdata.NCAC522HREDF_ActiveFlg = true;
                                _context.Update(filesdata);


                            }
                            else
                            {
                                if (item.cfileid == 0)
                                {
                                    if (item.cfilepath != null && item.cfilepath != "")
                                    {
                                        NAAC_AC_522_HrEducationFilesDMO obb = new NAAC_AC_522_HrEducationFilesDMO();
                                        obb.NCAC522HRED_Id = data.NCAC522HRED_Id;
                                        obb.NCAC522HREDF_FileName = item.cfilename;
                                        obb.NCAC522HREDF_FilePath = item.cfilepath;
                                        obb.NCAC522HREDF_Filedesc = item.cfiledesc;
                                        obb.NCAC522HREDF_ActiveFlg = true;
                                        obb.NCAC522HREDF_StatusFlg = "";

                                        _context.Add(obb);

                                    }
                                }
                            }
                        }



                    }
                    else
                    {

                        var removefile1 = _context.NAAC_AC_522_HrEducationFilesDMO.Where(t => t.NCAC522HRED_Id == data.NCAC522HRED_Id).Distinct().ToList();
                        if (removefile1.Count > 0)
                        {
                            foreach (var item in removefile1)
                            {
                                var deactfile = _context.NAAC_AC_522_HrEducationFilesDMO.Single(t => t.NCAC522HRED_Id == data.NCAC522HRED_Id && t.NCAC522HREDF_Id == item.NCAC522HREDF_Id);
                                deactfile.NCAC522HREDF_ActiveFlg = false;
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
                   // }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACHrEducationDTO deactiveStudent(NAACHrEducationDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_522_HrEducationDMO.Where(t => t.NCAC522HRED_Id == data.NCAC522HRED_Id).SingleOrDefault();
                if (u.NCAC522HRED_ActiveFlg == true)
                {
                    u.NCAC522HRED_ActiveFlg = false;
                }
                else if (u.NCAC522HRED_ActiveFlg == false)
                {
                    u.NCAC522HRED_ActiveFlg = true;
                }
                u.NCAC522HRED_UpdatedDate = DateTime.Now;
                u.NCAC522HRED_UpdatedBy = data.UserId;
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
        public NAACHrEducationDTO EditData(NAACHrEducationDTO data)
        {
            try
            {
              

                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_522_HrEducationDMO
                                 from c in _context.MasterCourseDMO
                                 from d in _context.ClgMasterBranchDMO
                 where (a.MI_Id == b.MI_Id && b.NCAC522HRED_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && b.NCAC522HRED_GraduatedProgram == c.AMCO_Id && b.NCAC522HRED_GraduatedDept == d.AMB_Id && b.NCAC522HRED_Id==data.NCAC522HRED_Id)
                 select new NAACHrEducationDTO
                 {
                     NCAC522HRED_Id = b.NCAC522HRED_Id,
                     NCAC522HRED_HrEduEnrollStudentNo = b.NCAC522HRED_HrEduEnrollStudentNo,
                     NCAC522HRED_AdmittedDept = b.NCAC522HRED_AdmittedDept,
                     NCAC522HRED_AdmittedProgram = b.NCAC522HRED_AdmittedProgram,
                     NCAC522HRED_ActiveFlg = b.NCAC522HRED_ActiveFlg,
                     NCAC522HRED_InstitutionName = b.NCAC522HRED_InstitutionName,
                     AMCO_CourseName = c.AMCO_CourseName,
                     AMB_BranchName = d.AMB_BranchName,
                     AMCO_Id = c.AMCO_Id,
                     AMB_Id = d.AMB_Id,
                     ASMAY_Year = a.ASMAY_Year,
                     ASMAY_Id = a.ASMAY_Id,
                     NCAC522HRED_StatusFlg = b.NCAC522HRED_StatusFlg,
                 }).Distinct().OrderByDescending(t => t.NCAC522HRED_Id).ToArray();



                data.editfiles = (from a in _context.NAAC_AC_522_HrEducationFilesDMO

                                  where (a.NCAC522HRED_Id == data.NCAC522HRED_Id && a.NCAC522HREDF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC522HREDF_FileName,
                                      cfilepath = a.NCAC522HREDF_FilePath,
                                      cfiledesc = a.NCAC522HREDF_Filedesc,
                                      cfileid = a.NCAC522HREDF_Id,
                                      status = a.NCAC522HREDF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACHrEducationDTO viewuploadflies(NAACHrEducationDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_522_HrEducationFilesDMO

                                  where (a.NCAC522HRED_Id == data.NCAC522HRED_Id && a.NCAC522HREDF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC522HRED_Id,
                                      cfileid = a.NCAC522HREDF_Id,
                                      cfilename = a.NCAC522HREDF_FileName,
                                      cfilepath = a.NCAC522HREDF_FilePath,
                                      cfiledesc = a.NCAC522HREDF_Filedesc,
                                      status = a.NCAC522HREDF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACHrEducationDTO deleteuploadfile(NAACHrEducationDTO data)
        {
            try
            {


                if (data.NCAC522HREDF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_522_HrEducationFilesDMO.Where(e => e.NCAC522HREDF_Id == data.NCAC522HREDF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC522HREDF_ActiveFlg = false;
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


        public NAACHrEducationDTO get_course(NAACHrEducationDTO data)
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

        public NAACHrEducationDTO get_branch(NAACHrEducationDTO data)
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

        public NAACHrEducationDTO getcomment(NAACHrEducationDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_522_HrEducation_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC522HREDC_RemarksBy == b.Id && a.NCAC522HRED_Id == data.NCAC522HRED_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC522HREDC_Remarks,
                                        commentid = a.NCAC522HREDC_Id,
                                        status = a.NCAC522HREDC_StatusFlg,
                                        createddate = a.NCAC522HREDC_CreatedDate,
                                        activeflag = a.NCAC522HREDC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACHrEducationDTO getfilecomment(NAACHrEducationDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_522_HrEducation_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC522HREDFC_RemarksBy == b.Id && a.NCAC522HREDF_Id == data.NCAC522HREDF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC522HREDFC_Remarks,
                                        commentid = a.NCAC522HREDFC_Id,
                                        status = a.NCAC522HREDFC_StatusFlg,
                                        createddate = a.NCAC522HREDFC_CreatedDate,
                                        activeflag = a.NCAC522HREDFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACHrEducationDTO savemedicaldatawisecomments(NAACHrEducationDTO data)
        {
            try
            {
                NAAC_AC_522_HrEducation_CommentsDMO cm = new NAAC_AC_522_HrEducation_CommentsDMO();
                cm.NCAC522HREDC_Remarks = data.Remarks;
                cm.NCAC522HREDC_RemarksBy = data.UserId;
                cm.NCAC522HREDC_StatusFlg = "";
                cm.NCAC522HREDC_ActiveFlag = true;
                cm.NCAC522HREDC_CreatedBy = data.UserId;
                cm.NCAC522HREDC_CreatedDate = DateTime.Now;
                cm.NCAC522HREDC_UpdatedBy = data.UserId;
                cm.NCAC522HREDC_UpdatedDate = DateTime.Now;
                cm.NCAC522HRED_Id = data.filefkid;
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
        public NAACHrEducationDTO savefilewisecomments(NAACHrEducationDTO data)
        {
            try
            {
                NAAC_AC_522_HrEducation_File_CommentsDMO cm = new NAAC_AC_522_HrEducation_File_CommentsDMO();
                cm.NCAC522HREDFC_Remarks = data.Remarks;
                cm.NCAC522HREDFC_RemarksBy = data.UserId;
                cm.NCAC522HREDFC_StatusFlg = "";
                cm.NCAC522HREDFC_ActiveFlag = true;
                cm.NCAC522HREDFC_CreatedBy = data.UserId;
                cm.NCAC522HREDFC_CreatedDate = DateTime.Now;
                cm.NCAC522HREDFC_UpdatedBy = data.UserId;
                cm.NCAC522HREDFC_UpdatedDate = DateTime.Now;
                cm.NCAC522HREDF_Id = data.filefkid;
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
