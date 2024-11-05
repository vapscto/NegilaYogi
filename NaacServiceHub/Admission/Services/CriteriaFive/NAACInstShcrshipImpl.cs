using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACInstShcrshipImpl : Interface.NAACInstShcrshipInterface
    {
        public GeneralContext _context;
        public NAACInstShcrshipImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACInstShcrshipDTO loaddata(NAACInstShcrshipDTO data)
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
                                    from b in _context.NAAC_AC_512_InstScholarshipDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC512INSCH_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACInstShcrshipDTO
                                    {
                                        MI_Id = a.MI_Id,
                                        NCAC512INSCH_Id = b.NCAC512INSCH_Id,
                                        NCAC512INSCH_SchemeName = b.NCAC512INSCH_SchemeName,
                                        NCAC512INSCH_NoOfStudents = b.NCAC512INSCH_NoOfStudents,
                                        NCAC512INSCH_ActiveFlg = b.NCAC512INSCH_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                    }).Distinct().OrderByDescending(t => t.NCAC512INSCH_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACInstShcrshipDTO save(NAACInstShcrshipDTO data)
        {
            try
            {
                if (data.NCAC512INSCH_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_512_InstScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC512INSCH_SchemeName == data.NCAC512INSCH_SchemeName && t.NCAC512INSCH_Year == data.NCAC512INSCH_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_512_InstScholarshipDMO obj1 = new NAAC_AC_512_InstScholarshipDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC512INSCH_Year = data.NCAC512INSCH_Year;
                        obj1.NCAC512INSCH_SchemeName = data.NCAC512INSCH_SchemeName;
                        obj1.NCAC512INSCH_NoOfStudents = data.NCAC512INSCH_NoOfStudents;
                        obj1.NCAC512INSCH_ActiveFlg = true;
                        obj1.NCAC512INSCH_CreatedBy = data.UserId;
                        obj1.NCAC512INSCH_UpdatedBy = data.UserId;
                        obj1.NCAC512INSCH_CreatedDate = DateTime.Now;
                        obj1.NCAC512INSCH_UpdatedDate = DateTime.Now;
                        obj1.NCAC512INSCH_StatusFlg = "";
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_512_InstScholarshipFilesDMO obb = new NAAC_AC_512_InstScholarshipFilesDMO();


                                    obb.NCAC512INSCH_Id = obj1.NCAC512INSCH_Id;
                                    obb.NCAC512INSCHF_FileName = item.cfilename;
                                    obb.NCAC512INSCHF_FilePath = item.cfilepath;
                                    obb.NCAC512INSCHF_Filedesc = item.cfiledesc;
                                    obb.NCAC512INSCHF_StatusFlg = "";
                                    obb.NCAC512INSCHF_ActiveFlg = true;

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
                else if (data.NCAC512INSCH_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_512_InstScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC512INSCH_SchemeName == data.NCAC512INSCH_SchemeName && t.NCAC512INSCH_Year == data.NCAC512INSCH_Year && t.NCAC512INSCH_Id !=data.NCAC512INSCH_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        

                        var update = _context.NAAC_AC_512_InstScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC512INSCH_Id == data.NCAC512INSCH_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC512INSCH_Year = data.NCAC512INSCH_Year;
                        update.NCAC512INSCH_SchemeName = data.NCAC512INSCH_SchemeName;
                        update.NCAC512INSCH_NoOfStudents = data.NCAC512INSCH_NoOfStudents;
                        update.NCAC512INSCH_ActiveFlg = true;
                        update.NCAC512INSCH_UpdatedBy = data.UserId;
                        update.NCAC512INSCH_UpdatedDate = DateTime.Now;
                        _context.Update(update);

                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_512_InstScholarshipFilesDMO.Where(t => t.NCAC512INSCH_Id == data.NCAC512INSCH_Id && !Fid.Contains(t.NCAC512INSCHF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_512_InstScholarshipFilesDMO.Single(t => t.NCAC512INSCH_Id == data.NCAC512INSCH_Id && t.NCAC512INSCHF_Id == item2.NCAC512INSCHF_Id);
                                    deactfile.NCAC512INSCHF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_512_InstScholarshipFilesDMO.Where(t => t.NCAC512INSCHF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC512INSCH_Id = data.NCAC512INSCH_Id;
                                    filesdata.NCAC512INSCHF_FileName = item.cfilename;
                                    filesdata.NCAC512INSCHF_FilePath = item.cfilepath;
                                    filesdata.NCAC512INSCHF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC512INSCHF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_512_InstScholarshipFilesDMO obb = new NAAC_AC_512_InstScholarshipFilesDMO();
                                            obb.NCAC512INSCH_Id = data.NCAC512INSCH_Id;
                                            obb.NCAC512INSCHF_FileName = item.cfilename;
                                            obb.NCAC512INSCHF_FilePath = item.cfilepath;
                                            obb.NCAC512INSCHF_Filedesc = item.cfiledesc;
                                            obb.NCAC512INSCHF_ActiveFlg = true;
                                            obb.NCAC512INSCHF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile = _context.NAAC_AC_512_InstScholarshipFilesDMO.Where(t => t.NCAC512INSCH_Id == data.NCAC512INSCH_Id).Distinct().ToList();
                            if (removefile.Count > 0)
                            {
                                foreach (var item in removefile)
                                {
                                    var deactfile = _context.NAAC_AC_512_InstScholarshipFilesDMO.Single(t => t.NCAC512INSCH_Id == data.NCAC512INSCH_Id && t.NCAC512INSCHF_Id == item.NCAC512INSCHF_Id);
                                    deactfile.NCAC512INSCHF_ActiveFlg = false;
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
        public NAACInstShcrshipDTO deactiveStudent(NAACInstShcrshipDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_512_InstScholarshipDMO.Where(t => t.NCAC512INSCH_Id == data.NCAC512INSCH_Id).SingleOrDefault();
                if (data.NCAC512INSCH_ActiveFlg == true)
                {
                    u.NCAC512INSCH_ActiveFlg = false;
                }
                else if (u.NCAC512INSCH_ActiveFlg == false)
                {
                    u.NCAC512INSCH_ActiveFlg = true;
                }
                u.NCAC512INSCH_UpdatedDate = DateTime.Now;
                u.NCAC512INSCH_UpdatedBy = data.UserId;
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
        public NAACInstShcrshipDTO EditData(NAACInstShcrshipDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_512_InstScholarshipDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC512INSCH_Year && b.MI_Id == data.MI_Id && b.NCAC512INSCH_Id == data.NCAC512INSCH_Id)
                                 select new NAACInstShcrshipDTO
                                 {
                                     NCAC512INSCH_Id = b.NCAC512INSCH_Id,
                                     NCAC512INSCH_SchemeName = b.NCAC512INSCH_SchemeName,
                                     NCAC512INSCH_NoOfStudents = b.NCAC512INSCH_NoOfStudents,
                                     NCAC512INSCH_ActiveFlg = b.NCAC512INSCH_ActiveFlg,
                                     NCAC512INSCH_Year = b.NCAC512INSCH_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC512INSCH_StatusFlg = b.NCAC512INSCH_StatusFlg
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_512_InstScholarshipFilesDMO

                                  where (a.NCAC512INSCH_Id == data.NCAC512INSCH_Id && a.NCAC512INSCHF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC512INSCHF_FileName,
                                      cfilepath = a.NCAC512INSCHF_FilePath,
                                      cfiledesc = a.NCAC512INSCHF_Filedesc,
                                      status = a.NCAC512INSCHF_StatusFlg,
                                      cfileid = a.NCAC512INSCHF_Id,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAACInstShcrshipDTO viewuploadflies(NAACInstShcrshipDTO data)
        {
            try
            {



                data.editfiles = (from a in _context.NAAC_AC_512_InstScholarshipFilesDMO

                                  where (a.NCAC512INSCH_Id == data.NCAC512INSCH_Id && a.NCAC512INSCHF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC512INSCH_Id,
                                      cfileid = a.NCAC512INSCHF_Id,
                                      cfilename = a.NCAC512INSCHF_FileName,
                                      cfilepath = a.NCAC512INSCHF_FilePath,
                                      cfiledesc = a.NCAC512INSCHF_Filedesc,
                                      status = a.NCAC512INSCHF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACInstShcrshipDTO deleteuploadfile(NAACInstShcrshipDTO data)
        {
            try
            {


                if (data.NCAC512INSCHF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_512_InstScholarshipFilesDMO.Where(e => e.NCAC512INSCHF_Id == data.NCAC512INSCHF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC512INSCHF_ActiveFlg = false;
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
        public NAACInstShcrshipDTO getcomment(NAACInstShcrshipDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_512_InstScholarship_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC512INSCHC_RemarksBy == b.Id && a.NCAC512INSCH_Id == data.NCAC512INSCH_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC512INSCHC_Remarks,
                                        commentid = a.NCAC512INSCHC_Id,
                                        status = a.NCAC512INSCHC_StatusFlg,
                                        createddate = a.NCAC512INSCHC_CreatedDate,
                                        activeflag = a.NCAC512INSCHC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACInstShcrshipDTO getfilecomment(NAACInstShcrshipDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_512_InstScholarship_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC512INSCHFC_RemarksBy == b.Id && a.NCAC512INSCHF_Id == data.NCAC512INSCHF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC512INSCHFC_Remarks,
                                        commentid = a.NCAC512INSCHFC_Id,
                                        status = a.NCAC512INSCHFC_StatusFlg,
                                        createddate = a.NCAC512INSCHFC_CreatedDate,
                                        activeflag = a.NCAC512INSCHFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

       
        public NAACInstShcrshipDTO savemedicaldatawisecomments(NAACInstShcrshipDTO data)
        {
            try
            {
                NAAC_AC_512_InstScholarship_CommentsDMO cm = new NAAC_AC_512_InstScholarship_CommentsDMO();
                cm.NCAC512INSCHC_Remarks = data.Remarks;
                cm.NCAC512INSCHC_RemarksBy = data.UserId;
                cm.NCAC512INSCHC_StatusFlg = "";
                cm.NCAC512INSCHC_ActiveFlag = true;
                cm.NCAC512INSCHC_CreatedBy = data.UserId;
                cm.NCAC512INSCHC_CreatedDate = DateTime.Now;
                cm.NCAC512INSCHC_UpdatedBy = data.UserId;
                cm.NCAC512INSCHC_UpdatedDate = DateTime.Now;
                cm.NCAC512INSCH_Id = data.filefkid;
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
        public NAACInstShcrshipDTO savefilewisecomments(NAACInstShcrshipDTO data)
        {
            try
            {
                NAAC_AC_512_InstScholarship_File_CommentsDMO cm = new NAAC_AC_512_InstScholarship_File_CommentsDMO();
                cm.NCAC512INSCHFC_Remarks = data.Remarks;
                cm.NCAC512INSCHFC_RemarksBy = data.UserId;
                cm.NCAC512INSCHFC_StatusFlg = "";
                cm.NCAC512INSCHFC_ActiveFlag = true;
                cm.NCAC512INSCHFC_CreatedBy = data.UserId;
                cm.NCAC512INSCHFC_CreatedDate = DateTime.Now;
                cm.NCAC512INSCHFC_UpdatedBy = data.UserId;
                cm.NCAC512INSCHFC_UpdatedDate = DateTime.Now;
                cm.NCAC512INSCHF_Id = data.filefkid;
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
