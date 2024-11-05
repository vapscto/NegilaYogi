using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACQualifyImpl : Interface.NAACQualifyInterface
    {
        public GeneralContext _context;
        public NAACQualifyImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACQualifyDTO loaddata(NAACQualifyDTO data)
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




                data.alldatatab2 = _context.NAAC_AC_523_QAMastersDMO.Where(t => t.MI_Id == data.MI_Id ).Distinct().OrderByDescending(t => t.NCAC523QAMA_Id).ToArray();

                data.examlist = _context.NAAC_AC_523_QAMastersDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QAMA_ActiveFlg==true).Distinct().OrderBy(t => t.NCAC523QAMA_ExamName).ToArray();

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.NAAC_AC_523_QAMastersDMO
                                    from c in _context.NAAC_AC_523_QualExamsDMO
                                    where (a.MI_Id == b.MI_Id && c.NCAC523QE_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.MI_Id==c.MI_Id && b.NCAC523QAMA_Id== c.NCAC523QAMA_Id)
                                    select new NAACQualifyDTO
                                    {
                                        NCAC523QE_Id = c.NCAC523QE_Id,
                                        MI_Id = c.MI_Id,
                                        NCAC523QAMA_Id = b.NCAC523QAMA_Id,
                                        NCAC523QAMA_ExamName = b.NCAC523QAMA_ExamName,
                                        NCAC523QE_NoOfStudents = c.NCAC523QE_NoOfStudents,
                                        NCAC523QE_NoOfStudentsappearing = c.NCAC523QE_NoOfStudentsappearing,
                                        NCAC523QE_ActiveFlg = c.NCAC523QE_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC523QE_StatusFlg = c.NCAC523QE_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC523QE_Id).ToArray();



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        
        public NAACQualifyDTO save1(NAACQualifyDTO data)
        {
            try
            {
                if (data.NCAC523QAMA_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_523_QAMastersDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QAMA_ExamName  == data.NCAC523QAMA_ExamName).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_523_QAMastersDMO obj1 = new NAAC_AC_523_QAMastersDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC523QAMA_ExamName = data.NCAC523QAMA_ExamName;
                        obj1.NCAC523QAMA_ExamDes = data.NCAC523QAMA_ExamDes;
                        obj1.NCAC523QAMA_ActiveFlg = true;
                        obj1.NCAC523QAMA_CreatedBy = data.UserId;
                        obj1.NCAC523QAMA_UpdatedBy = data.UserId;
                        obj1.NCAC523QAMA_CreatedDate = DateTime.Now;
                        obj1.NCAC523QAMA_UpdatedDate = DateTime.Now;
                        _context.Add(obj1);

                      

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
                else if (data.NCAC523QAMA_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_523_QAMastersDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QAMA_ExamName == data.NCAC523QAMA_ExamName  && t.NCAC523QAMA_Id != data.NCAC523QAMA_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var update = _context.NAAC_AC_523_QAMastersDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QAMA_Id == data.NCAC523QAMA_Id).SingleOrDefault();
                      
                        update.NCAC523QAMA_ExamName = data.NCAC523QAMA_ExamName;
                        update.NCAC523QAMA_ExamDes = data.NCAC523QAMA_ExamDes;
                        update.NCAC523QAMA_UpdatedBy = data.UserId;
                        update.NCAC523QAMA_UpdatedDate = DateTime.Now;
                        _context.Update(update);



            

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
        public NAACQualifyDTO deactiveStudent1(NAACQualifyDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_523_QAMastersDMO.Where(t => t.NCAC523QAMA_Id == data.NCAC523QAMA_Id).SingleOrDefault();
                if (u.NCAC523QAMA_ActiveFlg == true)
                {
                    u.NCAC523QAMA_ActiveFlg = false;
                }
                else if (u.NCAC523QAMA_ActiveFlg == false)
                {
                    u.NCAC523QAMA_ActiveFlg = true;
                }
                u.NCAC523QAMA_UpdatedDate = DateTime.Now;
                u.NCAC523QAMA_UpdatedBy = data.UserId;
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
        public NAACQualifyDTO EditData1(NAACQualifyDTO data)
        {
            try
            {
                data.editlist = _context.NAAC_AC_523_QAMastersDMO.Where(r => r.NCAC523QAMA_Id == data.NCAC523QAMA_Id).Distinct().ToArray();

                

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACQualifyDTO save(NAACQualifyDTO data)
        {
            try
            {
                if (data.NCAC523QE_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_523_QualExamsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QAMA_Id == data.NCAC523QAMA_Id && t.NCAC523QE_Year == data.NCAC523QE_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_523_QualExamsDMO obj1 = new NAAC_AC_523_QualExamsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC523QE_Year = data.NCAC523QE_Year;
                        obj1.NCAC523QAMA_Id = data.NCAC523QAMA_Id;
                        obj1.NCAC523QE_NoOfStudents = data.NCAC523QE_NoOfStudents;
                        obj1.NCAC523QE_NoOfStudentsappearing = data.NCAC523QE_NoOfStudentsappearing;
                        obj1.NCAC523QE_ActiveFlg = true;
                        obj1.NCAC523QE_CreatedBy = data.UserId;
                        obj1.NCAC523QE_UpdatedBy = data.UserId;
                        obj1.NCAC523QE_CreatedDate = DateTime.Now;
                        obj1.NCAC523QE_UpdatedDate = DateTime.Now;
                        obj1.NCAC523QE_StatusFlg ="";
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_523_QualExamsFilesDMO obb = new NAAC_AC_523_QualExamsFilesDMO();


                                    obb.NCAC523QE_Id = obj1.NCAC523QE_Id;
                                    obb.NCAC523QEF_FileName = item.cfilename;
                                    obb.NCAC523QEF_FilePath = item.cfilepath;
                                    obb.NCAC523QEF_Filedesc = item.cfiledesc;
                                    obb.NCAC523QEF_StatusFlg = "";
                                    obb.NCAC523QEF_ActiveFlg = true;

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
                else if (data.NCAC523QE_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_523_QualExamsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QE_Year == data.NCAC523QE_Year && t.NCAC523QAMA_Id == data.NCAC523QAMA_Id && t.NCAC523QE_Id !=data.NCAC523QE_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _context.NAAC_AC_523_QualExamsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC523QE_Id == data.NCAC523QE_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC523QE_Year = data.NCAC523QE_Year;
                        update.NCAC523QAMA_Id = data.NCAC523QAMA_Id;
                        update.NCAC523QE_NoOfStudents = data.NCAC523QE_NoOfStudents;
                        update.NCAC523QE_NoOfStudentsappearing = data.NCAC523QE_NoOfStudentsappearing;
                        update.NCAC523QE_UpdatedBy = data.UserId;
                        update.NCAC523QE_UpdatedDate = DateTime.Now;
                  
                  
                        _context.Update(update);



                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_523_QualExamsFilesDMO.Where(t => t.NCAC523QE_Id == data.NCAC523QE_Id && !Fid.Contains(t.NCAC523QEF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_523_QualExamsFilesDMO.Single(t => t.NCAC523QE_Id == data.NCAC523QE_Id && t.NCAC523QEF_Id == item2.NCAC523QEF_Id);
                                    deactfile.NCAC523QEF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_523_QualExamsFilesDMO.Where(t => t.NCAC523QEF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC523QE_Id = data.NCAC523QE_Id;
                                    filesdata.NCAC523QEF_FileName = item.cfilename;
                                    filesdata.NCAC523QEF_FilePath = item.cfilepath;
                                    filesdata.NCAC523QEF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC523QEF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_523_QualExamsFilesDMO obb = new NAAC_AC_523_QualExamsFilesDMO();
                                            obb.NCAC523QE_Id = data.NCAC523QE_Id;
                                            obb.NCAC523QEF_FileName = item.cfilename;
                                            obb.NCAC523QEF_FilePath = item.cfilepath;
                                            obb.NCAC523QEF_Filedesc = item.cfiledesc;
                                            obb.NCAC523QEF_ActiveFlg = true;
                                            obb.NCAC523QEF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_523_QualExamsFilesDMO.Where(t => t.NCAC523QE_Id == data.NCAC523QE_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_523_QualExamsFilesDMO.Single(t => t.NCAC523QE_Id == data.NCAC523QE_Id && t.NCAC523QEF_Id == item.NCAC523QEF_Id);
                                    deactfile.NCAC523QEF_ActiveFlg = false;
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
        public NAACQualifyDTO deactiveStudent(NAACQualifyDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_523_QualExamsDMO.Where(t => t.NCAC523QE_Id == data.NCAC523QE_Id).SingleOrDefault();
                if (u.NCAC523QE_ActiveFlg == true)
                {
                    u.NCAC523QE_ActiveFlg = false;
                }
                else if (u.NCAC523QE_ActiveFlg == false)
                {
                    u.NCAC523QE_ActiveFlg = true;
                }
                u.NCAC523QE_UpdatedDate = DateTime.Now;
                u.NCAC523QE_UpdatedBy = data.UserId;
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
        public NAACQualifyDTO EditData(NAACQualifyDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_523_QualExamsDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC523QE_Year && b.MI_Id == data.MI_Id && b.NCAC523QE_Id == data.NCAC523QE_Id )
                                 select new NAACQualifyDTO
                                 {
                                     NCAC523QE_Id = b.NCAC523QE_Id,
                                     NCAC523QAMA_Id = b.NCAC523QAMA_Id,
                                     NCAC523QE_NoOfStudents = b.NCAC523QE_NoOfStudents,
                                     NCAC523QE_ActiveFlg = b.NCAC523QE_ActiveFlg,
                                     NCAC523QE_Year = b.NCAC523QE_Year,
                                     NCAC523QE_NoOfStudentsappearing = b.NCAC523QE_NoOfStudentsappearing,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC523QE_StatusFlg=b.NCAC523QE_StatusFlg
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_523_QualExamsFilesDMO

                                  where (a.NCAC523QE_Id == data.NCAC523QE_Id  && a.NCAC523QEF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC523QEF_FileName,
                                      cfilepath = a.NCAC523QEF_FilePath,
                                      cfiledesc = a.NCAC523QEF_Filedesc,
                                      cfileid = a.NCAC523QEF_Id,
                                      status = a.NCAC523QEF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAACQualifyDTO viewuploadflies(NAACQualifyDTO data)
        {
            try
            {



                data.editfiles = (from a in _context.NAAC_AC_523_QualExamsFilesDMO

                                  where (a.NCAC523QE_Id == data.NCAC523QE_Id && a.NCAC523QEF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC523QE_Id,
                                      cfileid = a.NCAC523QEF_Id,
                                      cfilename = a.NCAC523QEF_FileName,
                                      cfilepath = a.NCAC523QEF_FilePath,
                                      cfiledesc = a.NCAC523QEF_Filedesc,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACQualifyDTO deleteuploadfile(NAACQualifyDTO data)
        {
            try
            {


                if (data.NCAC523QEF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_523_QualExamsFilesDMO.Where(e => e.NCAC523QEF_Id == data.NCAC523QEF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC523QEF_ActiveFlg = false;
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

        public NAACQualifyDTO getcomment(NAACQualifyDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_523_QualExams_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC523QEC_RemarksBy == b.Id && a.NCAC523QE_Id == data.NCAC523QE_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC523QEC_Remarks,
                                        commentid = a.NCAC523QEC_Id,
                                        status = a.NCAC523QEC_StatusFlg,
                                        createddate = a.NCAC523QEC_CreatedDate,
                                        activeflag = a.NCAC523QEC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACQualifyDTO getfilecomment(NAACQualifyDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_523_QualExams_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC523QEFC_RemarksBy == b.Id && a.NCAC523QEF_Id == data.NCAC523QEF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC523QEFC_Remarks,
                                        commentid = a.NCAC523QEFC_Id,
                                        status = a.NCAC523QEFC_StatusFlg,
                                        createddate = a.NCAC523QEFC_CreatedDate,
                                        activeflag = a.NCAC523QEFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACQualifyDTO savemedicaldatawisecomments(NAACQualifyDTO data)
        {
            try
            {
                NAAC_AC_523_QualExams_CommentsDMO cm = new NAAC_AC_523_QualExams_CommentsDMO();
                cm.NCAC523QEC_Remarks = data.Remarks;
                cm.NCAC523QEC_RemarksBy = data.UserId;
                cm.NCAC523QEC_StatusFlg = "";
                cm.NCAC523QEC_ActiveFlag = true;
                cm.NCAC523QEC_CreatedBy = data.UserId;
                cm.NCAC523QEC_CreatedDate = DateTime.Now;
                cm.NCAC523QEC_UpdatedBy = data.UserId;
                cm.NCAC523QEC_UpdatedDate = DateTime.Now;
                cm.NCAC523QE_Id = data.filefkid;
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
        public NAACQualifyDTO savefilewisecomments(NAACQualifyDTO data)
        {
            try
            {
                NAAC_AC_523_QualExams_File_CommentsDMO cm = new NAAC_AC_523_QualExams_File_CommentsDMO();
                cm.NCAC523QEFC_Remarks = data.Remarks;
                cm.NCAC523QEFC_RemarksBy = data.UserId;
                cm.NCAC523QEFC_StatusFlg = "";
                cm.NCAC523QEFC_ActiveFlag = true;
                cm.NCAC523QEFC_CreatedBy = data.UserId;
                cm.NCAC523QEFC_CreatedDate = DateTime.Now;
                cm.NCAC523QEFC_UpdatedBy = data.UserId;
                cm.NCAC523QEFC_UpdatedDate = DateTime.Now;
                cm.NCAC523QEF_Id = data.filefkid;
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
