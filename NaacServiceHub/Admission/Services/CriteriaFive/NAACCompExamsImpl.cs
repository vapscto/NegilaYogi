using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACCompExamsImpl : Interface.NAACCompExamsInterface
    {
        public GeneralContext _context;
        public NAACCompExamsImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACCompExamsDTO loaddata(NAACCompExamsDTO data)
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
                                    from b in _context.NAAC_AC_514_CompExamsDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC514CPEX_ImpYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACCompExamsDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC514CPEX_Id = b.NCAC514CPEX_Id,
                                        NCAC514CPEX_ExamSchemeName = b.NCAC514CPEX_ExamSchemeName,
                                        NCAC514CPEX_NoOfStudents = b.NCAC514CPEX_NoOfStudents,
                                        NCAC514CPEX_ActiveFlg = b.NCAC514CPEX_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC514CPEX_StatusFlg = b.NCAC514CPEX_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC514CPEX_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACCompExamsDTO save(NAACCompExamsDTO data)
        {
            try
            {
                if (data.NCAC514CPEX_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_514_CompExamsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC514CPEX_ExamSchemeName == data.NCAC514CPEX_ExamSchemeName && t.NCAC514CPEX_ImpYear == data.NCAC514CPEX_ImpYear).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_514_CompExamsDMO obj1 = new NAAC_AC_514_CompExamsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC514CPEX_ImpYear = data.NCAC514CPEX_ImpYear;
                        obj1.NCAC514CPEX_ExamSchemeName = data.NCAC514CPEX_ExamSchemeName;
                        obj1.NCAC514CPEX_NoOfStudents = data.NCAC514CPEX_NoOfStudents;
                        obj1.NCAC514CPEX_ActiveFlg = true;
                        obj1.NCAC514CPEX_CreatedBy = data.UserId;
                        obj1.NCAC514CPEX_UpdatedBy = data.UserId;
                        obj1.NCAC514CPEX_CreatedDate = DateTime.Now;
                        obj1.NCAC514CPEX_UpdatedDate = DateTime.Now;
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_514_CompExamsFilesDMO obb = new NAAC_AC_514_CompExamsFilesDMO();


                                    obb.NCAC514CPEX_Id = obj1.NCAC514CPEX_Id;
                                    obb.NCAC514CPEXF_FileName = item.cfilename;
                                    obb.NCAC514CPEXF_FilePath = item.cfilepath;
                                    obb.NCAC514CPEXF_Filedesc = item.cfiledesc;

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
                else if (data.NCAC514CPEX_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_514_CompExamsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC514CPEX_ExamSchemeName == data.NCAC514CPEX_ExamSchemeName && t.NCAC514CPEX_ImpYear == data.NCAC514CPEX_ImpYear && t.NCAC514CPEX_Id !=data.NCAC514CPEX_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _context.NAAC_AC_514_CompExamsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC514CPEX_Id == data.NCAC514CPEX_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC514CPEX_ImpYear = data.NCAC514CPEX_ImpYear;
                        update.NCAC514CPEX_ExamSchemeName = data.NCAC514CPEX_ExamSchemeName;
                        update.NCAC514CPEX_NoOfStudents = data.NCAC514CPEX_NoOfStudents;
                        update.NCAC514CPEX_ActiveFlg = true;
                        update.NCAC514CPEX_UpdatedBy = data.UserId;
                        update.NCAC514CPEX_UpdatedDate = DateTime.Now;
                        _context.Update(update);


                           if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_514_CompExamsFilesDMO.Where(t => t.NCAC514CPEX_Id == data.NCAC514CPEX_Id && !Fid.Contains(t.NCAC514CPEXF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_514_CompExamsFilesDMO.Single(t => t.NCAC514CPEX_Id == data.NCAC514CPEX_Id && t.NCAC514CPEXF_Id == item2.NCAC514CPEXF_Id);
                                    deactfile.NCAC514CPEXF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_514_CompExamsFilesDMO.Where(t => t.NCAC514CPEXF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC514CPEX_Id = data.NCAC514CPEX_Id;
                                    filesdata.NCAC514CPEXF_FileName = item.cfilename;
                                    filesdata.NCAC514CPEXF_FilePath = item.cfilepath;
                                    filesdata.NCAC514CPEXF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC514CPEXF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_514_CompExamsFilesDMO obb = new NAAC_AC_514_CompExamsFilesDMO();
                                            obb.NCAC514CPEX_Id = data.NCAC514CPEX_Id;
                                            obb.NCAC514CPEXF_FileName = item.cfilename;
                                            obb.NCAC514CPEXF_FilePath = item.cfilepath;
                                            obb.NCAC514CPEXF_Filedesc = item.cfiledesc;
                                            obb.NCAC514CPEXF_ActiveFlg = true;
                                            obb.NCAC514CPEXF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile = _context.NAAC_AC_514_CompExamsFilesDMO.Where(t => t.NCAC514CPEX_Id == data.NCAC514CPEX_Id).Distinct().ToList();
                            if (removefile.Count > 0)
                            {
                                foreach (var item in removefile)
                                {
                                    var deactfile = _context.NAAC_AC_514_CompExamsFilesDMO.Single(t => t.NCAC514CPEX_Id == data.NCAC514CPEX_Id && t.NCAC514CPEXF_Id == item.NCAC514CPEXF_Id);
                                    deactfile.NCAC514CPEXF_ActiveFlg = false;
                                    _context.Update(removefile);
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
        public NAACCompExamsDTO deactiveStudent(NAACCompExamsDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_514_CompExamsDMO.Where(t => t.NCAC514CPEX_Id == data.NCAC514CPEX_Id).SingleOrDefault();
                if (data.NCAC514CPEX_ActiveFlg == true)
                {
                    u.NCAC514CPEX_ActiveFlg = false;
                }
                else if (u.NCAC514CPEX_ActiveFlg == false)
                {
                    u.NCAC514CPEX_ActiveFlg = true;
                }
                u.NCAC514CPEX_UpdatedDate = DateTime.Now;
                u.NCAC514CPEX_UpdatedBy = data.UserId;
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
        public NAACCompExamsDTO EditData(NAACCompExamsDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_514_CompExamsDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC514CPEX_ImpYear && b.MI_Id == data.MI_Id && b.NCAC514CPEX_Id == data.NCAC514CPEX_Id)
                                 select new NAACCompExamsDTO
                                 {
                                     NCAC514CPEX_Id = b.NCAC514CPEX_Id,
                                     NCAC514CPEX_ExamSchemeName = b.NCAC514CPEX_ExamSchemeName,
                                     NCAC514CPEX_NoOfStudents = b.NCAC514CPEX_NoOfStudents,
                                     NCAC514CPEX_ActiveFlg = b.NCAC514CPEX_ActiveFlg,
                                     NCAC514CPEX_ImpYear = b.NCAC514CPEX_ImpYear,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC514CPEX_StatusFlg=b.NCAC514CPEX_StatusFlg
                                 }).Distinct().ToArray();


                data.editfiles = (from a in _context.NAAC_AC_514_CompExamsFilesDMO

                                  where (a.NCAC514CPEX_Id == data.NCAC514CPEX_Id && a.NCAC514CPEXF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC514CPEXF_FileName,
                                      cfilepath = a.NCAC514CPEXF_FilePath,
                                      cfiledesc = a.NCAC514CPEXF_Filedesc,
                                      status = a.NCAC514CPEXF_StatusFlg,
                                      cfileid = a.NCAC514CPEXF_Id,
                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACCompExamsDTO viewuploadflies(NAACCompExamsDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_514_CompExamsFilesDMO

                                  where (a.NCAC514CPEX_Id == data.NCAC514CPEX_Id && a.NCAC514CPEXF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC514CPEX_Id,
                                      cfileid = a.NCAC514CPEXF_Id,
                                      cfilename = a.NCAC514CPEXF_FileName,
                                      cfilepath = a.NCAC514CPEXF_FilePath,
                                      cfiledesc = a.NCAC514CPEXF_Filedesc,
                                      status = a.NCAC514CPEXF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACCompExamsDTO deleteuploadfile(NAACCompExamsDTO data)
        {
            try
            {


                if (data.NCAC514CPEXF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_514_CompExamsFilesDMO.Where(e => e.NCAC514CPEXF_Id == data.NCAC514CPEXF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC514CPEXF_ActiveFlg = false;
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

        public NAACCompExamsDTO getcomment(NAACCompExamsDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_514_CompExams_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC514CPEXC_RemarksBy == b.Id && a.NCAC514CPEX_Id == data.NCAC514CPEX_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC514CPEXC_Remarks,
                                        commentid = a.NCAC514CPEXC_Id,
                                        status = a.NCAC514CPEXC_StatusFlg,
                                        createddate = a.NCAC514CPEXC_CreatedDate,
                                        activeflag = a.NCAC514CPEXC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACCompExamsDTO getfilecomment(NAACCompExamsDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_514_CompExams_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC514CPEXFC_RemarksBy == b.Id && a.NCAC514CPEXF_Id == data.NCAC514CPEXF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC514CPEXFC_Remarks,
                                        commentid = a.NCAC514CPEXFC_Id,
                                        status = a.NCAC514CPEXFC_StatusFlg,
                                        createddate = a.NCAC514CPEXFC_CreatedDate,
                                        activeflag = a.NCAC514CPEXFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACCompExamsDTO savemedicaldatawisecomments(NAACCompExamsDTO data)
        {
            try
            {
                NAAC_AC_514_CompExams_CommentsDMO cm = new NAAC_AC_514_CompExams_CommentsDMO();
                cm.NCAC514CPEXC_Remarks = data.Remarks;
                cm.NCAC514CPEXC_RemarksBy = data.UserId;
                cm.NCAC514CPEXC_StatusFlg = "";
                cm.NCAC514CPEXC_ActiveFlag = true;
                cm.NCAC514CPEXC_CreatedBy = data.UserId;
                cm.NCAC514CPEXC_CreatedDate = DateTime.Now;
                cm.NCAC514CPEXC_UpdatedBy = data.UserId;
                cm.NCAC514CPEXC_UpdatedDate = DateTime.Now;
                cm.NCAC514CPEX_Id = data.filefkid;
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
        public NAACCompExamsDTO savefilewisecomments(NAACCompExamsDTO data)
        {
            try
            {
                NAAC_AC_514_CompExams_File_CommentsDMO cm = new NAAC_AC_514_CompExams_File_CommentsDMO();
                cm.NCAC514CPEXFC_Remarks = data.Remarks;
                cm.NCAC514CPEXFC_RemarksBy = data.UserId;
                cm.NCAC514CPEXFC_StatusFlg = "";
                cm.NCAC514CPEXFC_ActiveFlag = true;
                cm.NCAC514CPEXFC_CreatedBy = data.UserId;
                cm.NCAC514CPEXFC_CreatedDate = DateTime.Now;
                cm.NCAC514CPEXFC_UpdatedBy = data.UserId;
                cm.NCAC514CPEXFC_UpdatedDate = DateTime.Now;
                cm.NCAC514CPEXF_Id = data.filefkid;
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
