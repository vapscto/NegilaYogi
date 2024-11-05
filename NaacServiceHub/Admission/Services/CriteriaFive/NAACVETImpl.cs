using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACVETImpl : Interface.NAACVETInterface
    {
        public GeneralContext _context;
        public NAACVETImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACVETDTO loaddata(NAACVETDTO data)
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
                                    from b in _context.NAAC_AC_515_VETDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC515VET_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACVETDTO
                                    {
                                        NCAC515VET_Id = b.NCAC515VET_Id,
                                        MI_Id = b.MI_Id,
                                        NCAC515VET_VETProgramName = b.NCAC515VET_VETProgramName,
                                        NCAC515VET_NoOfStudents = b.NCAC515VET_NoOfStudents,
                                        NCAC515VET_ActiveFlg = b.NCAC515VET_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC515VET_StatusFlg = b.NCAC515VET_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC515VET_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACVETDTO save(NAACVETDTO data)
        {
            try
            {
                if (data.NCAC515VET_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_515_VETDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC515VET_VETProgramName == data.NCAC515VET_VETProgramName && t.NCAC515VET_Year == data.NCAC515VET_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_515_VETDMO obj1 = new NAAC_AC_515_VETDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC515VET_Year = data.NCAC515VET_Year;
                        obj1.NCAC515VET_VETProgramName = data.NCAC515VET_VETProgramName;
                        obj1.NCAC515VET_NoOfStudents = data.NCAC515VET_NoOfStudents;
                        obj1.NCAC515VET_ActiveFlg = true;
                        obj1.NCAC515VET_CreatedBy = data.UserId;
                        obj1.NCAC515VET_UpdatedBy = data.UserId;
                        obj1.NCAC515VET_CreatedDate = DateTime.Now;
                        obj1.NCAC515VET_UpdatedDate = DateTime.Now;
                        obj1.NCAC515VET_StatusFlg ="";
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_515_VETFilesDMO obb = new NAAC_AC_515_VETFilesDMO();
                                    
                                    obb.NCAC515VET_Id = obj1.NCAC515VET_Id;
                                    obb.NCAC515VETF_FileName = item.cfilename;
                                    obb.NCAC515VETF_FilePath = item.cfilepath;
                                    obb.NCAC515VETF_Filedesc = item.cfiledesc;
                                    obb.NCAC515VETF_StatusFlg = "";
                                    obb.NCAC515VETF_ActiveFlg = true;

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
                else if (data.NCAC515VET_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_515_VETDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC515VET_VETProgramName == data.NCAC515VET_VETProgramName && t.NCAC515VET_Year == data.NCAC515VET_Year && t.NCAC515VET_Id !=data.NCAC515VET_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var removefile = _context.NAAC_AC_515_VETFilesDMO.Where(t => t.NCAC515VET_Id == data.NCAC515VET_Id).Distinct().ToList();
                        if (removefile.Count > 0)
                        {
                            foreach (var item in removefile)
                            {
                                _context.Remove(item);
                            }
                        }
                            var update = _context.NAAC_AC_515_VETDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC515VET_Id == data.NCAC515VET_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC515VET_Year = data.NCAC515VET_Year;
                        update.NCAC515VET_VETProgramName = data.NCAC515VET_VETProgramName;
                        update.NCAC515VET_NoOfStudents = data.NCAC515VET_NoOfStudents;
                        update.NCAC515VET_ActiveFlg = true;
                        update.NCAC515VET_UpdatedBy = data.UserId;
                        update.NCAC515VET_UpdatedDate = DateTime.Now;
                        _context.Update(update);



                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_515_VETFilesDMO.Where(t => t.NCAC515VET_Id == data.NCAC515VET_Id && !Fid.Contains(t.NCAC515VETF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_515_VETFilesDMO.Single(t => t.NCAC515VET_Id == data.NCAC515VET_Id && t.NCAC515VETF_Id == item2.NCAC515VETF_Id);
                                    deactfile.NCAC515VETF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_515_VETFilesDMO.Where(t => t.NCAC515VETF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC515VET_Id = data.NCAC515VET_Id;
                                    filesdata.NCAC515VETF_FileName = item.cfilename;
                                    filesdata.NCAC515VETF_FilePath = item.cfilepath;
                                    filesdata.NCAC515VETF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC515VETF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_515_VETFilesDMO obb = new NAAC_AC_515_VETFilesDMO();
                                            obb.NCAC515VET_Id = data.NCAC515VET_Id;
                                            obb.NCAC515VETF_FileName = item.cfilename;
                                            obb.NCAC515VETF_FilePath = item.cfilepath;
                                            obb.NCAC515VETF_Filedesc = item.cfiledesc;
                                            obb.NCAC515VETF_ActiveFlg = true;
                                            obb.NCAC515VETF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_515_VETFilesDMO.Where(t => t.NCAC515VET_Id == data.NCAC515VET_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_515_VETFilesDMO.Single(t => t.NCAC515VET_Id == data.NCAC515VET_Id && t.NCAC515VETF_Id == item.NCAC515VETF_Id);
                                    deactfile.NCAC515VETF_ActiveFlg = false;
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
        public NAACVETDTO deactiveStudent(NAACVETDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_515_VETDMO.Where(t => t.NCAC515VET_Id == data.NCAC515VET_Id).SingleOrDefault();
                if (data.NCAC515VET_ActiveFlg == true)
                {
                    u.NCAC515VET_ActiveFlg = false;
                }
                else if (u.NCAC515VET_ActiveFlg == false)
                {
                    u.NCAC515VET_ActiveFlg = true;
                }
                u.NCAC515VET_UpdatedDate = DateTime.Now;
                u.NCAC515VET_UpdatedBy = data.UserId;
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
        public NAACVETDTO EditData(NAACVETDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_515_VETDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC515VET_Year && b.MI_Id == data.MI_Id && b.NCAC515VET_Id == data.NCAC515VET_Id)
                                 select new NAACVETDTO
                                 {
                                     NCAC515VET_Id = b.NCAC515VET_Id,
                                     NCAC515VET_VETProgramName = b.NCAC515VET_VETProgramName,
                                     NCAC515VET_NoOfStudents = b.NCAC515VET_NoOfStudents,
                                     NCAC515VET_ActiveFlg = b.NCAC515VET_ActiveFlg,
                                     NCAC515VET_Year = b.NCAC515VET_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC515VET_StatusFlg=b.NCAC515VET_StatusFlg
                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_515_VETFilesDMO

                                  where (a.NCAC515VET_Id == data.NCAC515VET_Id && a.NCAC515VETF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC515VETF_FileName,
                                      cfilepath = a.NCAC515VETF_FilePath,
                                      cfiledesc = a.NCAC515VETF_Filedesc,
                                      cfileid=a.NCAC515VETF_Id,
                                      status=a.NCAC515VETF_StatusFlg
                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACVETDTO viewuploadflies(NAACVETDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_515_VETFilesDMO

                                  where (a.NCAC515VET_Id == data.NCAC515VET_Id && a.NCAC515VETF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC515VET_Id,
                                      cfileid = a.NCAC515VETF_Id,
                                      cfilename = a.NCAC515VETF_FileName,
                                      cfilepath = a.NCAC515VETF_FilePath,
                                      cfiledesc = a.NCAC515VETF_Filedesc,
                                      status=a.NCAC515VETF_StatusFlg

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACVETDTO deleteuploadfile(NAACVETDTO data)
        {
            try
            {


                if (data.NCAC515VETF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_515_VETFilesDMO.Where(e => e.NCAC515VETF_Id == data.NCAC515VETF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC515VETF_ActiveFlg = false;
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

        public NAACVETDTO getcomment(NAACVETDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_515_VET_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC515VETC_RemarksBy == b.Id && a.NCAC515VET_Id == data.NCAC515VET_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC515VETC_Remarks,
                                        commentid = a.NCAC515VETC_Id,
                                        status = a.NCAC515VETC_StatusFlg,
                                        createddate = a.NCAC515VETC_CreatedDate,
                                        activeflag = a.NCAC515VETC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACVETDTO getfilecomment(NAACVETDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_515_VET_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC515VETFC_RemarksBy == b.Id && a.NCAC515VETF_Id == data.NCAC515VETF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC515VETFC_Remarks,
                                        commentid = a.NCAC515VETFC_Id,
                                        status = a.NCAC515VETFC_StatusFlg,
                                        createddate = a.NCAC515VETFC_CreatedDate,
                                        activeflag = a.NCAC515VETFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACVETDTO savemedicaldatawisecomments(NAACVETDTO data)
        {
            try
            {
                NAAC_AC_515_VET_CommentsDMO cm = new NAAC_AC_515_VET_CommentsDMO();
                cm.NCAC515VETC_Remarks = data.Remarks;
                cm.NCAC515VETC_RemarksBy = data.UserId;
                cm.NCAC515VETC_StatusFlg = "";
                cm.NCAC515VETC_ActiveFlag = true;
                cm.NCAC515VETC_CreatedBy = data.UserId;
                cm.NCAC515VETC_CreatedDate = DateTime.Now;
                cm.NCAC515VETC_UpdatedBy = data.UserId;
                cm.NCAC515VETC_UpdatedDate = DateTime.Now;
                cm.NCAC515VET_Id = data.filefkid;
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
        public NAACVETDTO savefilewisecomments(NAACVETDTO data)
        {
            try
            {
                NAAC_AC_515_VET_File_CommentsDMO cm = new NAAC_AC_515_VET_File_CommentsDMO();
                cm.NCAC515VETFC_Remarks = data.Remarks;
                cm.NCAC515VETFC_RemarksBy = data.UserId;
                cm.NCAC515VETFC_StatusFlg = "";
                cm.NCAC515VETFC_ActiveFlag = true;
                cm.NCAC515VETFC_CreatedBy = data.UserId;
                cm.NCAC515VETFC_CreatedDate = DateTime.Now;
                cm.NCAC515VETFC_UpdatedBy = data.UserId;
                cm.NCAC515VETFC_UpdatedDate = DateTime.Now;
                cm.NCAC515VETF_Id = data.filefkid;
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
