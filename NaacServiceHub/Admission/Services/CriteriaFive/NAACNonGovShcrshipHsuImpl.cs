using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACNonGovShcrshipHsuImpl : Interface.NAACNonGovShcrshipHsuInterface
    {
        public GeneralContext _context;
        public NAACNonGovShcrshipHsuImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACNonGovShcrshipHsuDTO loaddata(NAACNonGovShcrshipHsuDTO data)
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
                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                data.alldatalist = (from a in _context.Academic
                                    from b in _context.NAAC_HSU_511_NonGovScholarshipDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC512NGSCH_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACNonGovShcrshipHsuDTO
                                    {
                                        MI_Id = a.MI_Id,
                                        NCAC512NGSCH_Id = b.NCAC512NGSCH_Id,
                                        NCAC512NGSCH_SchemeName = b.NCAC512NGSCH_SchemeName,
                                        NCAC512NGSCH_NoOfStudents = b.NCAC512NGSCH_NoOfStudents,
                                        NCAC512NGSCH_ActiveFlg = b.NCAC512NGSCH_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC512NGSCH_StatusFlg = b.NCAC512NGSCH_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC512NGSCH_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACNonGovShcrshipHsuDTO save(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {
                if (data.NCAC512NGSCH_Id == 0)
                {
                    var duplicate = _context.NAAC_HSU_511_NonGovScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC512NGSCH_SchemeName == data.NCAC512NGSCH_SchemeName && t.NCAC512NGSCH_Year == data.NCAC512NGSCH_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_HSU_511_NonGovScholarshipDMO obj1 = new NAAC_HSU_511_NonGovScholarshipDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC512NGSCH_Year = data.NCAC512NGSCH_Year;
                        obj1.NCAC512NGSCH_SchemeName = data.NCAC512NGSCH_SchemeName;
                        obj1.NCAC512NGSCH_NoOfStudents = data.NCAC512NGSCH_NoOfStudents;
                        obj1.NCAC512NGSCH_ActiveFlg = true;
                        obj1.NCAC512NGSCH_CreatedBy = data.UserId;
                        obj1.NCAC512NGSCH_UpdatedBy = data.UserId;
                        obj1.NCAC512NGSCH_CreatedDate = DateTime.Now;
                        obj1.NCAC512NGSCH_UpdatedDate = DateTime.Now;
                        obj1.NCAC512NGSCH_StatusFlg = "";
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_HSU_511_NonGovScholarship_FilesDMO obb = new NAAC_HSU_511_NonGovScholarship_FilesDMO();


                                    obb.NCAC512NGSCH_Id = obj1.NCAC512NGSCH_Id;
                                    obb.NCAC512NGSCHF_FileName = item.cfilename;
                                    obb.NCAC512NGSCHF_FilePath = item.cfilepath;
                                    obb.NCAC512NGSCHF_Filedesc = item.cfiledesc;
                                    obb.NCAC512NGSCHF_StatusFlg = "";
                                    obb.NCAC512NGSCHF_ActiveFlg = true;

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
                else if (data.NCAC512NGSCH_Id > 0)
                {
                    var duplicate = _context.NAAC_HSU_511_NonGovScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC512NGSCH_SchemeName == data.NCAC512NGSCH_SchemeName && t.NCAC512NGSCH_Year == data.NCAC512NGSCH_Year && t.NCAC512NGSCH_Id !=data.NCAC512NGSCH_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {



                        var update = _context.NAAC_HSU_511_NonGovScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC512NGSCH_Year = data.NCAC512NGSCH_Year;
                        update.NCAC512NGSCH_SchemeName = data.NCAC512NGSCH_SchemeName;
                        update.NCAC512NGSCH_NoOfStudents = data.NCAC512NGSCH_NoOfStudents;
                        update.NCAC512NGSCH_ActiveFlg = true;
                        update.NCAC512NGSCH_UpdatedBy = data.UserId;
                        update.NCAC512NGSCH_UpdatedDate = DateTime.Now;
                        _context.Update(update);



                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_HSU_511_NonGovScholarship_FilesDMO.Where(t => t.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id && !Fid.Contains(t.NCAC512NGSCHF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_HSU_511_NonGovScholarship_FilesDMO.Single(t => t.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id && t.NCAC512NGSCHF_Id == item2.NCAC512NGSCHF_Id);
                                    deactfile.NCAC512NGSCHF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_HSU_511_NonGovScholarship_FilesDMO.Where(t => t.NCAC512NGSCHF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC512NGSCH_Id = data.NCAC512NGSCH_Id;
                                    filesdata.NCAC512NGSCHF_FileName = item.cfilename;
                                    filesdata.NCAC512NGSCHF_FilePath = item.cfilepath;
                                    filesdata.NCAC512NGSCHF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC512NGSCHF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_HSU_511_NonGovScholarship_FilesDMO obb = new NAAC_HSU_511_NonGovScholarship_FilesDMO();
                                            obb.NCAC512NGSCH_Id = data.NCAC512NGSCH_Id;
                                            obb.NCAC512NGSCHF_FileName = item.cfilename;
                                            obb.NCAC512NGSCHF_FilePath = item.cfilepath;
                                            obb.NCAC512NGSCHF_Filedesc = item.cfiledesc;
                                            obb.NCAC512NGSCHF_ActiveFlg = true;
                                            obb.NCAC512NGSCHF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_HSU_511_NonGovScholarship_FilesDMO.Where(t => t.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_HSU_511_NonGovScholarship_FilesDMO.Single(t => t.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id && t.NCAC512NGSCHF_Id == item.NCAC512NGSCHF_Id);
                                    deactfile.NCAC512NGSCHF_ActiveFlg = false;
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
        public NAACNonGovShcrshipHsuDTO deactiveStudent(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {
                var u = _context.NAAC_HSU_511_NonGovScholarshipDMO.Where(t => t.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id).SingleOrDefault();
                if (data.NCAC512NGSCH_ActiveFlg == true)
                {
                    u.NCAC512NGSCH_ActiveFlg = false;
                }
                else if (u.NCAC512NGSCH_ActiveFlg == false)
                {
                    u.NCAC512NGSCH_ActiveFlg = true;
                }
                u.NCAC512NGSCH_UpdatedDate = DateTime.Now;
                u.NCAC512NGSCH_UpdatedBy = data.UserId;
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
        public NAACNonGovShcrshipHsuDTO EditData(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_HSU_511_NonGovScholarshipDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC512NGSCH_Year && b.MI_Id == data.MI_Id && b.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id)
                                 select new NAACNonGovShcrshipHsuDTO
                                 {
                                     NCAC512NGSCH_Id = b.NCAC512NGSCH_Id,
                                     NCAC512NGSCH_SchemeName = b.NCAC512NGSCH_SchemeName,
                                     NCAC512NGSCH_NoOfStudents = b.NCAC512NGSCH_NoOfStudents,
                                     NCAC512NGSCH_ActiveFlg = b.NCAC512NGSCH_ActiveFlg,
                                     NCAC512NGSCH_Year = b.NCAC512NGSCH_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC512NGSCH_StatusFlg = b.NCAC512NGSCH_StatusFlg,
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_HSU_511_NonGovScholarship_FilesDMO

                                  where (a.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id && a.NCAC512NGSCHF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC512NGSCHF_FileName,
                                      cfilepath = a.NCAC512NGSCHF_FilePath,
                                      cfiledesc = a.NCAC512NGSCHF_Filedesc,
                                      cfileid = a.NCAC512NGSCHF_Id,
                                      status = a.NCAC512NGSCHF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAACNonGovShcrshipHsuDTO viewuploadflies(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {



                data.editfiles = (from a in _context.NAAC_HSU_511_NonGovScholarship_FilesDMO

                                  where (a.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id && a.NCAC512NGSCHF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC512NGSCH_Id,
                                      cfileid = a.NCAC512NGSCHF_Id,
                                      cfilename = a.NCAC512NGSCHF_FileName,
                                      cfilepath = a.NCAC512NGSCHF_FilePath,
                                      cfiledesc = a.NCAC512NGSCHF_Filedesc,
                                      status = a.NCAC512NGSCHF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACNonGovShcrshipHsuDTO deleteuploadfile(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {


                if (data.NCAC512NGSCHF_Id > 0)
                {
                    var deletefile = _context.NAAC_HSU_511_NonGovScholarship_FilesDMO.Where(e => e.NCAC512NGSCHF_Id == data.NCAC512NGSCHF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC512NGSCHF_ActiveFlg = false;
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


        public NAACNonGovShcrshipHsuDTO getcomment(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_HSU_511_NonGovScholarship_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC512NGSCHC_RemarksBy == b.Id && a.NCAC512NGSCH_Id == data.NCAC512NGSCH_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC512NGSCHC_Remarks,
                                        commentid = a.NCAC512NGSCHC_Id,
                                        status = a.NCAC512NGSCHC_StatusFlg,
                                        createddate = a.NCAC512NGSCHC_CreatedDate,
                                        activeflag = a.NCAC512NGSCHC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACNonGovShcrshipHsuDTO getfilecomment(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_HSU_511_NonGovScholarship_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC512NGSCHFC_RemarksBy == b.Id && a.NCAC512NGSCHF_Id == data.NCAC512NGSCHF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC512NGSCHFC_Remarks,
                                        commentid = a.NCAC512NGSCHFC_Id,
                                        status = a.NCAC512NGSCHFC_StatusFlg,
                                        createddate = a.NCAC512NGSCHFC_CreatedDate,
                                        activeflag = a.NCAC512NGSCHFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACNonGovShcrshipHsuDTO savemedicaldatawisecomments(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {
                NAAC_HSU_511_NonGovScholarship_CommentsDMO cm = new NAAC_HSU_511_NonGovScholarship_CommentsDMO();
                cm.NCAC512NGSCHC_Remarks = data.Remarks;
                cm.NCAC512NGSCHC_RemarksBy = data.UserId;
                cm.NCAC512NGSCHC_StatusFlg = "";
                cm.NCAC512NGSCHC_ActiveFlag = true;
                cm.NCAC512NGSCHC_CreatedBy = data.UserId;
                cm.NCAC512NGSCHC_CreatedDate = DateTime.Now;
                cm.NCAC512NGSCHC_UpdatedBy = data.UserId;
                cm.NCAC512NGSCHC_UpdatedDate = DateTime.Now;
                cm.NCAC512NGSCH_Id = data.filefkid;
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
        public NAACNonGovShcrshipHsuDTO savefilewisecomments(NAACNonGovShcrshipHsuDTO data)
        {
            try
            {
                NAAC_HSU_511_NonGovScholarship_File_CommentsDMO cm = new NAAC_HSU_511_NonGovScholarship_File_CommentsDMO();
                cm.NCAC512NGSCHFC_Remarks = data.Remarks;
                cm.NCAC512NGSCHFC_RemarksBy = data.UserId;
                cm.NCAC512NGSCHFC_StatusFlg = "";
                cm.NCAC512NGSCHFC_ActiveFlag = true;
                cm.NCAC512NGSCHFC_CreatedBy = data.UserId;
                cm.NCAC512NGSCHFC_CreatedDate = DateTime.Now;
                cm.NCAC512NGSCHFC_UpdatedBy = data.UserId;
                cm.NCAC512NGSCHFC_UpdatedDate = DateTime.Now;
                cm.NCAC512NGSCHF_Id = data.filefkid;
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
