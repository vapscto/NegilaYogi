using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACEncDevSchemeImpl : Interface.NAACEncDevSchemeInterface
    {
        public GeneralContext _context;
        public NAACEncDevSchemeImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACEncDevSchemeDTO loaddata(NAACEncDevSchemeDTO data)
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
                                    from b in _context.NAAC_AC_513_DevSchemesDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC513INSCH_ImpYear == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACEncDevSchemeDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC513INSCH_Id = b.NCAC513INSCH_Id,
                                        NCAC513INSCH_DevSchemeName = b.NCAC513INSCH_DevSchemeName,
                                        NCAC513INSCH_NoOfStudents = b.NCAC513INSCH_NoOfStudents,
                                        NCAC513INSCH_ActiveFlg = b.NCAC513INSCH_ActiveFlg,
                                        NCAC513INSCH_AgencyDetails = b.NCAC513INSCH_AgencyDetails,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC513INSCH_StatusFlg = b.NCAC513INSCH_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC513INSCH_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACEncDevSchemeDTO save(NAACEncDevSchemeDTO data)
        {
            try
            {
                if (data.NCAC513INSCH_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_513_DevSchemesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC513INSCH_DevSchemeName == data.NCAC513INSCH_DevSchemeName && t.NCAC513INSCH_ImpYear == data.NCAC513INSCH_ImpYear).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_513_DevSchemesDMO obj1 = new NAAC_AC_513_DevSchemesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC513INSCH_ImpYear = data.NCAC513INSCH_ImpYear;
                        obj1.NCAC513INSCH_DevSchemeName = data.NCAC513INSCH_DevSchemeName;
                        obj1.NCAC513INSCH_NoOfStudents = data.NCAC513INSCH_NoOfStudents;
                        obj1.NCAC513INSCH_AgencyDetails = data.NCAC513INSCH_AgencyDetails;
                        obj1.NCAC513INSCH_ActiveFlg = true;
                        obj1.NCAC513INSCH_CreatedBy = data.UserId;
                        obj1.NCAC513INSCH_UpdatedBy = data.UserId;
                        obj1.NCAC513INSCH_CreatedDate = DateTime.Now;
                        obj1.NCAC513INSCH_UpdatedDate = DateTime.Now;
                        obj1.NCAC513INSCH_StatusFlg = "";
                        _context.Add(obj1);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_513_DevSchemeFilesDMO obb = new NAAC_AC_513_DevSchemeFilesDMO();


                                    obb.NCAC513INSCH_Id = obj1.NCAC513INSCH_Id;
                                    obb.NCAC513INSCHF_FileName = item.cfilename;
                                    obb.NCAC513INSCHF_FilePath = item.cfilepath;
                                    obb.NCAC513INSCHF_Filedesc = item.cfiledesc;
                                    obb.NCAC513INSCHF_StatusFlg = "";
                                    obb.NCAC513INSCHF_ActiveFlg =true;

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
                else if (data.NCAC513INSCH_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_513_DevSchemesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC513INSCH_DevSchemeName == data.NCAC513INSCH_DevSchemeName && t.NCAC513INSCH_ImpYear == data.NCAC513INSCH_ImpYear && t.NCAC513INSCH_Id !=data.NCAC513INSCH_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        //var removefile = _context.NAAC_AC_513_DevSchemeFilesDMO.Where(t => t.NCAC513INSCH_Id == data.NCAC513INSCH_Id).Distinct().ToList();
                        //if (removefile.Count > 0)
                        //{
                        //    foreach (var item in removefile)
                        //    {
                        //        _context.Remove(item);
                        //    }
                        //}



                        var update = _context.NAAC_AC_513_DevSchemesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC513INSCH_Id == data.NCAC513INSCH_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC513INSCH_ImpYear = data.NCAC513INSCH_ImpYear;
                        update.NCAC513INSCH_DevSchemeName = data.NCAC513INSCH_DevSchemeName;
                        update.NCAC513INSCH_NoOfStudents = data.NCAC513INSCH_NoOfStudents;
                        update.NCAC513INSCH_AgencyDetails = data.NCAC513INSCH_AgencyDetails;
                        update.NCAC513INSCH_ActiveFlg = true;
                        update.NCAC513INSCH_UpdatedBy = data.UserId;
                        update.NCAC513INSCH_UpdatedDate = DateTime.Now;
                        _context.Update(update);

                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_513_DevSchemeFilesDMO.Where(t => t.NCAC513INSCH_Id == data.NCAC513INSCH_Id && !Fid.Contains(t.NCAC513INSCHF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_513_DevSchemeFilesDMO.Single(t => t.NCAC513INSCH_Id == data.NCAC513INSCH_Id && t.NCAC513INSCHF_Id == item2.NCAC513INSCHF_Id);
                                    deactfile.NCAC513INSCHF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_513_DevSchemeFilesDMO.Where(t => t.NCAC513INSCHF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC513INSCH_Id = data.NCAC513INSCH_Id;
                                    filesdata.NCAC513INSCHF_FileName = item.cfilename;
                                    filesdata.NCAC513INSCHF_FilePath = item.cfilepath;
                                    filesdata.NCAC513INSCHF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC513INSCHF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_513_DevSchemeFilesDMO obb = new NAAC_AC_513_DevSchemeFilesDMO();
                                            obb.NCAC513INSCH_Id = data.NCAC513INSCH_Id;
                                            obb.NCAC513INSCHF_FileName = item.cfilename;
                                            obb.NCAC513INSCHF_FilePath = item.cfilepath;
                                            obb.NCAC513INSCHF_Filedesc = item.cfiledesc;
                                            obb.NCAC513INSCHF_ActiveFlg = true;
                                            obb.NCAC513INSCHF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile = _context.NAAC_AC_513_DevSchemeFilesDMO.Where(t => t.NCAC513INSCH_Id == data.NCAC513INSCH_Id).Distinct().ToList();
                            if (removefile.Count > 0)
                            {
                                foreach (var item in removefile)
                                {
                                    var deactfile = _context.NAAC_AC_513_DevSchemeFilesDMO.Single(t => t.NCAC513INSCH_Id == data.NCAC513INSCH_Id && t.NCAC513INSCHF_Id == item.NCAC513INSCHF_Id);
                                    deactfile.NCAC513INSCHF_ActiveFlg = false;
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
        public NAACEncDevSchemeDTO deactiveStudent(NAACEncDevSchemeDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_513_DevSchemesDMO.Where(t => t.NCAC513INSCH_Id == data.NCAC513INSCH_Id).SingleOrDefault();
                if (data.NCAC513INSCH_ActiveFlg == true)
                {
                    u.NCAC513INSCH_ActiveFlg = false;
                }
                else if (u.NCAC513INSCH_ActiveFlg == false)
                {
                    u.NCAC513INSCH_ActiveFlg = true;
                }
                u.NCAC513INSCH_UpdatedDate = DateTime.Now;
                u.NCAC513INSCH_UpdatedBy = data.UserId;
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
        public NAACEncDevSchemeDTO EditData(NAACEncDevSchemeDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_513_DevSchemesDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC513INSCH_ImpYear && b.MI_Id == data.MI_Id && b.NCAC513INSCH_Id == data.NCAC513INSCH_Id)
                                 select new NAACEncDevSchemeDTO
                                 {
                                     NCAC513INSCH_Id = b.NCAC513INSCH_Id,
                                     NCAC513INSCH_DevSchemeName = b.NCAC513INSCH_DevSchemeName,
                                     NCAC513INSCH_NoOfStudents = b.NCAC513INSCH_NoOfStudents,
                                     NCAC513INSCH_ActiveFlg = b.NCAC513INSCH_ActiveFlg,
                                     NCAC513INSCH_ImpYear = b.NCAC513INSCH_ImpYear,
                                     NCAC513INSCH_AgencyDetails = b.NCAC513INSCH_AgencyDetails,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC513INSCH_StatusFlg = b.NCAC513INSCH_StatusFlg
                                    
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_513_DevSchemeFilesDMO

                                  where (a.NCAC513INSCH_Id == data.NCAC513INSCH_Id && a.NCAC513INSCHF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC513INSCHF_FileName,
                                      cfilepath = a.NCAC513INSCHF_FilePath,
                                      cfiledesc = a.NCAC513INSCHF_Filedesc,
                                      status=a.NCAC513INSCHF_StatusFlg,
                                      cfileid=a.NCAC513INSCHF_Id

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACEncDevSchemeDTO viewuploadflies(NAACEncDevSchemeDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_513_DevSchemeFilesDMO

                                  where (a.NCAC513INSCH_Id == data.NCAC513INSCH_Id && a.NCAC513INSCHF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC513INSCH_Id,
                                      cfileid = a.NCAC513INSCHF_Id,
                                      cfilename = a.NCAC513INSCHF_FileName,
                                      cfilepath = a.NCAC513INSCHF_FilePath,
                                      cfiledesc = a.NCAC513INSCHF_Filedesc,
                                      status = a.NCAC513INSCHF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACEncDevSchemeDTO deleteuploadfile(NAACEncDevSchemeDTO data)
        {
            try
            {


                if (data.NCAC513INSCHF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_513_DevSchemeFilesDMO.Where(e => e.NCAC513INSCHF_Id == data.NCAC513INSCHF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC513INSCHF_ActiveFlg = false;
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

        public NAACEncDevSchemeDTO getcomment(NAACEncDevSchemeDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_513_DevSchemes_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC513INSCHC_RemarksBy == b.Id && a.NCAC513INSCH_Id == data.NCAC513INSCH_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC513INSCHC_Remarks,
                                        commentid = a.NCAC513INSCHC_Id,
                                        status = a.NCAC513INSCHC_StatusFlg,
                                        createddate = a.NCAC513INSCHC_CreatedDate,
                                        activeflag = a.NCAC513INSCHC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACEncDevSchemeDTO getfilecomment(NAACEncDevSchemeDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_513_DevSchemes_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC513INSCHFC_RemarksBy == b.Id && a.NCAC513INSCHF_Id == data.NCAC513INSCHF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC513INSCHFC_Remarks,
                                        commentid = a.NCAC513INSCHFC_Id,
                                        status = a.NCAC513INSCHFC_StatusFlg,
                                        createddate = a.NCAC513INSCHFC_CreatedDate,
                                        activeflag = a.NCAC513INSCHFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAACEncDevSchemeDTO savemedicaldatawisecomments(NAACEncDevSchemeDTO data)
        {
            try
            {
                NAAC_AC_513_DevSchemes_CommentsDMO cm = new NAAC_AC_513_DevSchemes_CommentsDMO();
                cm.NCAC513INSCHC_Remarks = data.Remarks;
                cm.NCAC513INSCHC_RemarksBy = data.UserId;
                cm.NCAC513INSCHC_StatusFlg = "";
                cm.NCAC513INSCHC_ActiveFlag = true;
                cm.NCAC513INSCHC_CreatedBy = data.UserId;
                cm.NCAC513INSCHC_CreatedDate = DateTime.Now;
                cm.NCAC513INSCHC_UpdatedBy = data.UserId;
                cm.NCAC513INSCHC_UpdatedDate = DateTime.Now;
                cm.NCAC513INSCH_Id = data.filefkid;
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
        public NAACEncDevSchemeDTO savefilewisecomments(NAACEncDevSchemeDTO data)
        {
            try
            {
                NAAC_AC_513_DevSchemes_File_CommentsDMO cm = new NAAC_AC_513_DevSchemes_File_CommentsDMO();
                cm.NCAC513INSCHFC_Remarks = data.Remarks;
                cm.NCAC513INSCHFC_RemarksBy = data.UserId;
                cm.NCAC513INSCHFC_StatusFlg = "";
                cm.NCAC513INSCHFC_ActiveFlag = true;
                cm.NCAC513INSCHFC_CreatedBy = data.UserId;
                cm.NCAC513INSCHFC_CreatedDate = DateTime.Now;
                cm.NCAC513INSCHFC_UpdatedBy = data.UserId;
                cm.NCAC513INSCHFC_UpdatedDate = DateTime.Now;
                cm.NCAC513INSCHF_Id = data.filefkid;
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
