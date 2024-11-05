using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACGovtShcrShipImpl : Interface.NAACGovtShcrShipInterface
    {
        public GeneralContext _context;
        public NAACGovtShcrShipImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACGovtShcrShipDTO loaddata(NAACGovtShcrShipDTO data)
       {
            try
            {

                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where a.MI_Id==b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag==1
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
                                    from b in _context.NAAC_AC_511_GovScholarshipDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC511GSCH_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACGovtShcrShipDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC511GSCH_Id = b.NCAC511GSCH_Id,
                                        NCAC511GSCH_SchemeName = b.NCAC511GSCH_SchemeName,
                                        NCAC511GSCH_NoOfStudents = b.NCAC511GSCH_NoOfStudents,
                                        NCAC511GSCH_ActiveFlg = b.NCAC511GSCH_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC511GSCH_StatusFlg = b.NCAC511GSCH_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC511GSCH_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACGovtShcrShipDTO save(NAACGovtShcrShipDTO data)
        {
            try
            {
                if (data.NCAC511GSCH_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_511_GovScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC511GSCH_SchemeName == data.NCAC511GSCH_SchemeName && t.NCAC511GSCH_Year == data.NCAC511GSCH_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_511_GovScholarshipDMO obj1 = new NAAC_AC_511_GovScholarshipDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC511GSCH_Year = data.NCAC511GSCH_Year;
                        obj1.NCAC511GSCH_SchemeName = data.NCAC511GSCH_SchemeName;
                        obj1.NCAC511GSCH_NoOfStudents = data.NCAC511GSCH_NoOfStudents;
                        obj1.NCAC511GSCH_StatusFlg = "";
                        obj1.NCAC511GSCH_ActiveFlg = true;
                        obj1.NCAC511GSCH_CreatedBy = data.UserId;
                        obj1.NCAC511GSCH_UpdatedBy = data.UserId;
                        obj1.NCAC511GSCH_CreatedDate = DateTime.Now;
                        obj1.NCAC511GSCH_UpdatedDate = DateTime.Now;
                        _context.Add(obj1);

                        if (data.filelist.Length>0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath !=null && item.cfilepath !="")
                                {
                                    NAAC_AC_511_GovScholarshipFilesDMO obb = new NAAC_AC_511_GovScholarshipFilesDMO();


                                    obb.NCAC511GSCH_Id = obj1.NCAC511GSCH_Id;
                                    obb.NCAC511GSCHF_FileName = item.cfilename;
                                    obb.NCAC511GSCHF_FilePath = item.cfilepath;
                                    obb.NCAC511GSCHF_Filedesc = item.cfiledesc;
                                    obb.NCAC511GSCHF_ActiveFlg = true;
                                    obb.NCAC511GSCHF_StatusFlg = "";

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
                else if (data.NCAC511GSCH_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_511_GovScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC511GSCH_SchemeName == data.NCAC511GSCH_SchemeName && t.NCAC511GSCH_Year == data.NCAC511GSCH_Year && t.NCAC511GSCH_Id !=data.NCAC511GSCH_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        //var removefile = _context.NAAC_AC_511_GovScholarshipFilesDMO.Where(t => t.NCAC511GSCH_Id == data.NCAC511GSCH_Id ).Distinct().ToList();
                        //if (removefile.Count>0)
                        //{
                        //    foreach (var item in removefile)
                        //    {
                        //        _context.Remove(item);
                        //    }
                        //}
                       

                        

                        var update = _context.NAAC_AC_511_GovScholarshipDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC511GSCH_Id == data.NCAC511GSCH_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC511GSCH_Year = data.NCAC511GSCH_Year;
                        update.NCAC511GSCH_SchemeName = data.NCAC511GSCH_SchemeName;
                        update.NCAC511GSCH_NoOfStudents = data.NCAC511GSCH_NoOfStudents;
                        update.NCAC511GSCH_ActiveFlg = true;
                        update.NCAC511GSCH_UpdatedBy = data.UserId;
                        update.NCAC511GSCH_UpdatedDate = DateTime.Now;
                        _context.Update(update);
                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_511_GovScholarshipFilesDMO.Where(t => t.NCAC511GSCH_Id == data.NCAC511GSCH_Id && !Fid.Contains(t.NCAC511GSCHF_Id)).Distinct().ToList();

                            if (removefile1.Count>0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_511_GovScholarshipFilesDMO.Single(t => t.NCAC511GSCH_Id == data.NCAC511GSCH_Id && t.NCAC511GSCHF_Id == item2.NCAC511GSCHF_Id);
                                    deactfile.NCAC511GSCHF_ActiveFlg = false;
                                    _context.Update(deactfile);

                                }
                               
                            }



                            foreach (var item in data.filelist)
                            {
                                if (item.status==null)
                                {
                                    item.status = "";
                                }

                                if (item.cfileid > 0 && item.status.ToLower() != "approved")
                                {
                                    var filesdata = _context.NAAC_AC_511_GovScholarshipFilesDMO.Where(t => t.NCAC511GSCHF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC511GSCH_Id = data.NCAC511GSCH_Id;
                                    filesdata.NCAC511GSCHF_FileName = item.cfilename;
                                    filesdata.NCAC511GSCHF_FilePath = item.cfilepath;
                                    filesdata.NCAC511GSCHF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC511GSCHF_ActiveFlg = true;
                                    _context.Update(filesdata);
                                 

                                }
                                else
                                {
                                    if (item.cfileid ==0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                    {
                                        NAAC_AC_511_GovScholarshipFilesDMO obb = new NAAC_AC_511_GovScholarshipFilesDMO();
                                        obb.NCAC511GSCH_Id = data.NCAC511GSCH_Id;
                                        obb.NCAC511GSCHF_FileName = item.cfilename;
                                        obb.NCAC511GSCHF_FilePath = item.cfilepath;
                                        obb.NCAC511GSCHF_Filedesc = item.cfiledesc;
                                        obb.NCAC511GSCHF_ActiveFlg = true;
                                        obb.NCAC511GSCHF_StatusFlg = "";

                                        _context.Add(obb);

                                    }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile = _context.NAAC_AC_511_GovScholarshipFilesDMO.Where(t => t.NCAC511GSCH_Id == data.NCAC511GSCH_Id).Distinct().ToList();
                            if (removefile.Count > 0)
                            {
                                foreach (var item in removefile)
                                {
                                    var deactfile = _context.NAAC_AC_511_GovScholarshipFilesDMO.Single(t => t.NCAC511GSCH_Id == data.NCAC511GSCH_Id && t.NCAC511GSCHF_Id == item.NCAC511GSCHF_Id);
                                    deactfile.NCAC511GSCHF_ActiveFlg = false;
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
        public NAACGovtShcrShipDTO deactiveStudent(NAACGovtShcrShipDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_511_GovScholarshipDMO.Where(t => t.NCAC511GSCH_Id == data.NCAC511GSCH_Id).SingleOrDefault();
                if (data.NCAC511GSCH_ActiveFlg == true)
                {
                    u.NCAC511GSCH_ActiveFlg = false;
                }
                else if (u.NCAC511GSCH_ActiveFlg == false)
                {
                    u.NCAC511GSCH_ActiveFlg = true;
                }
                u.NCAC511GSCH_UpdatedDate = DateTime.Now;
                u.NCAC511GSCH_UpdatedBy = data.UserId;
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
        public NAACGovtShcrShipDTO EditData(NAACGovtShcrShipDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_511_GovScholarshipDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC511GSCH_Year && b.MI_Id == data.MI_Id && b.NCAC511GSCH_Id == data.NCAC511GSCH_Id)
                                 select new NAACGovtShcrShipDTO
                                 {
                                     NCAC511GSCH_Id = b.NCAC511GSCH_Id,
                                     NCAC511GSCH_SchemeName = b.NCAC511GSCH_SchemeName,
                                     NCAC511GSCH_NoOfStudents = b.NCAC511GSCH_NoOfStudents,
                                     NCAC511GSCH_ActiveFlg = b.NCAC511GSCH_ActiveFlg,
                                     NCAC511GSCH_Year = b.NCAC511GSCH_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC511GSCH_StatusFlg = b.NCAC511GSCH_StatusFlg
                                 }).Distinct().ToArray();


                data.editfiles=(from a in _context.NAAC_AC_511_GovScholarshipFilesDMO

                                 where (a.NCAC511GSCH_Id == data.NCAC511GSCH_Id && a.NCAC511GSCHF_ActiveFlg==true)
                                 select new NAACCriteriaFivefileDTO
                                 {
                                    cfilename=a.NCAC511GSCHF_FileName,
                                    cfilepath=a.NCAC511GSCHF_FilePath,
                                    cfiledesc=a.NCAC511GSCHF_Filedesc,
                                    status=a.NCAC511GSCHF_StatusFlg,
                                    cfileid=a.NCAC511GSCHF_Id,

                                 }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGovtShcrShipDTO viewuploadflies(NAACGovtShcrShipDTO data)
        {
            try
            {
                


                data.editfiles=(from a in _context.NAAC_AC_511_GovScholarshipFilesDMO

                                 where (a.NCAC511GSCH_Id == data.NCAC511GSCH_Id && a.NCAC511GSCHF_ActiveFlg==true)
                                 select new NAACCriteriaFivefileDTO
                                 {
                                     gridid = a.NCAC511GSCH_Id,
                                    cfileid=a.NCAC511GSCHF_Id,
                                    cfilename=a.NCAC511GSCHF_FileName,
                                    cfilepath=a.NCAC511GSCHF_FilePath,
                                    cfiledesc=a.NCAC511GSCHF_Filedesc,
                                    status=a.NCAC511GSCHF_StatusFlg,

                                 }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGovtShcrShipDTO getcomment(NAACGovtShcrShipDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_511_GovScholarship_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC511GSCHC_RemarksBy == b.Id && a.NCAC511GSCH_Id == data.NCAC511GSCH_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC511GSCHC_Remarks,
                                        commentid = a.NCAC511GSCHC_Id,
                                        status = a.NCAC511GSCHC_StatusFlg,
                                        createddate=a.NCAC511GSCHC_CreatedDate,
                               activeflag=a.NCAC511GSCHC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t=>t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGovtShcrShipDTO getfilecomment(NAACGovtShcrShipDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_511_GovScholarship_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC511GSCHFC_RemarksBy == b.Id && a.NCAC511GSCHF_Id == data.NCAC511GSCHF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC511GSCHFC_Remarks,
                                        commentid = a.NCAC511GSCHFC_Id,
                                        status = a.NCAC511GSCHFC_StatusFlg,
                                        createddate=a.NCAC511GSCHFC_CreatedDate,
                               activeflag=a.NCAC511GSCHFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t=>t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACGovtShcrShipDTO deleteuploadfile(NAACGovtShcrShipDTO data)
        {
            try
            {


                if (data.NCAC511GSCHF_Id>0)
                {
                    var deletefile = _context.NAAC_AC_511_GovScholarshipFilesDMO.Where(e => e.NCAC511GSCHF_Id == data.NCAC511GSCHF_Id).ToList();

                    if (deletefile.Count>0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC511GSCHF_ActiveFlg = false;
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
        public NAACGovtShcrShipDTO savemedicaldatawisecomments(NAACGovtShcrShipDTO data)
        {
            try
            {
                NAAC_AC_511_GovScholarship_CommentsDMO cm = new NAAC_AC_511_GovScholarship_CommentsDMO();
                cm.NCAC511GSCHC_Remarks = data.Remarks;
                cm.NCAC511GSCHC_RemarksBy = data.UserId;
                cm.NCAC511GSCHC_StatusFlg = "";
                cm.NCAC511GSCHC_ActiveFlag = true;
                cm.NCAC511GSCHC_CreatedBy = data.UserId;
                cm.NCAC511GSCHC_CreatedDate = DateTime.Now;
                cm.NCAC511GSCHC_UpdatedBy = data.UserId;
                cm.NCAC511GSCHC_UpdatedDate = DateTime.Now;
                cm.NCAC511GSCH_Id = data.filefkid;
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
        public NAACGovtShcrShipDTO savefilewisecomments(NAACGovtShcrShipDTO data)
        {
            try
            {
                NAAC_AC_511_GovScholarship_File_CommentsDMO cm = new NAAC_AC_511_GovScholarship_File_CommentsDMO();
                cm.NCAC511GSCHFC_Remarks = data.Remarks;
                cm.NCAC511GSCHFC_RemarksBy = data.UserId;
                cm.NCAC511GSCHFC_StatusFlg = "";
                cm.NCAC511GSCHFC_ActiveFlag = true;
                cm.NCAC511GSCHFC_CreatedBy = data.UserId;
                cm.NCAC511GSCHFC_CreatedDate = DateTime.Now;
                cm.NCAC511GSCHFC_UpdatedBy = data.UserId;
                cm.NCAC511GSCHFC_UpdatedDate = DateTime.Now;
                cm.NCAC511GSCHF_Id = data.filefkid;
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
