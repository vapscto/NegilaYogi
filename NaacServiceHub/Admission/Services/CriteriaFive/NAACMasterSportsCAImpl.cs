using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACMasterSportsCAImpl : Interface.NAACMasterSportsCAInterface
    {
        public GeneralContext _context;
        public NAACMasterSportsCAImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACMasterSportsCADTO loaddata(NAACMasterSportsCADTO data)
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
                                    from b in _context.NAAC_AC_533_SportsCA_ActivitiesDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC533SPCAA_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACMasterSportsCADTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC533SPCAA_Id = b.NCAC533SPCAA_Id,
                                        NCAC533SPCAA_NameOfActivities = b.NCAC533SPCAA_NameOfActivities,
                                        NCAC533SPCAA_ActiveFlg = b.NCAC533SPCAA_ActiveFlg,
                                        NCAC533SPCAA_ActType = b.NCAC533SPCAA_ActType,
                                        NCAC533SPCAA_ActLevel = b.NCAC533SPCAA_ActLevel,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC533SPCAA_StatusFlg = b.NCAC533SPCAA_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC533SPCAA_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACMasterSportsCADTO save(NAACMasterSportsCADTO data)
        {
            try
            {
                if (data.NCAC533SPCAA_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_533_SportsCA_ActivitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC533SPCAA_NameOfActivities == data.NCAC533SPCAA_NameOfActivities && t.NCAC533SPCAA_Year == data.NCAC533SPCAA_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_533_SportsCA_ActivitiesDMO obj1 = new NAAC_AC_533_SportsCA_ActivitiesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC533SPCAA_Year = data.NCAC533SPCAA_Year;
                        obj1.NCAC533SPCAA_NameOfActivities = data.NCAC533SPCAA_NameOfActivities;
                        obj1.NCAC533SPCAA_ActiveFlg = true;
                        obj1.NCAC533SPCAA_CreatedBy = data.UserId;
                        obj1.NCAC533SPCAA_UpdatedBy = data.UserId;
                        obj1.NCAC533SPCAA_CreatedDate = DateTime.Now;
                        obj1.NCAC533SPCAA_UpdatedDate = DateTime.Now;
                        obj1.NCAC533SPCAA_ActType = data.NCAC533SPCAA_ActType;
                        obj1.NCAC533SPCAA_ActLevel = data.NCAC533SPCAA_ActLevel;
                        obj1.NCAC533SPCAA_StatusFlg = "";
                        _context.Add(obj1);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_533_SportsCA_ActivitiesFilesDMO obb = new NAAC_AC_533_SportsCA_ActivitiesFilesDMO();


                                    obb.NCAC533SPCAA_Id = obj1.NCAC533SPCAA_Id;
                                    obb.NCAC533SPCAAF_FileName = item.cfilename;
                                    obb.NCAC533SPCAAF_FilePath = item.cfilepath;
                                    obb.NCAC533SPCAAF_Filedesc = item.cfiledesc;
                                    obb.NCAC533SPCAAF_StatusFlg = "";
                                    obb.NCAC533SPCAAF_ActiveFlg = true;

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
                else if (data.NCAC533SPCAA_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_533_SportsCA_ActivitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC533SPCAA_NameOfActivities == data.NCAC533SPCAA_NameOfActivities && t.NCAC533SPCAA_Year == data.NCAC533SPCAA_Year && t.NCAC533SPCAA_Id !=data.NCAC533SPCAA_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                       
                        var update = _context.NAAC_AC_533_SportsCA_ActivitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC533SPCAA_Year = data.NCAC533SPCAA_Year;
                        update.NCAC533SPCAA_NameOfActivities = data.NCAC533SPCAA_NameOfActivities;
                        update.NCAC533SPCAA_ActiveFlg = true;
                        update.NCAC533SPCAA_UpdatedBy = data.UserId;
                        update.NCAC533SPCAA_UpdatedDate = DateTime.Now;
                        update.NCAC533SPCAA_ActType = data.NCAC533SPCAA_ActType;
                        update.NCAC533SPCAA_ActLevel = data.NCAC533SPCAA_ActLevel;
                        _context.Update(update);

                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO.Where(t => t.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id && !Fid.Contains(t.NCAC533SPCAAF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO.Single(t => t.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id && t.NCAC533SPCAAF_Id == item2.NCAC533SPCAAF_Id);
                                    deactfile.NCAC533SPCAAF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO.Where(t => t.NCAC533SPCAAF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC533SPCAA_Id = data.NCAC533SPCAA_Id;
                                    filesdata.NCAC533SPCAAF_FileName = item.cfilename;
                                    filesdata.NCAC533SPCAAF_FilePath = item.cfilepath;
                                    filesdata.NCAC533SPCAAF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC533SPCAAF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_533_SportsCA_ActivitiesFilesDMO obb = new NAAC_AC_533_SportsCA_ActivitiesFilesDMO();
                                            obb.NCAC533SPCAA_Id = data.NCAC533SPCAA_Id;
                                            obb.NCAC533SPCAAF_FileName = item.cfilename;
                                            obb.NCAC533SPCAAF_FilePath = item.cfilepath;
                                            obb.NCAC533SPCAAF_Filedesc = item.cfiledesc;
                                            obb.NCAC533SPCAAF_ActiveFlg = true;
                                            obb.NCAC533SPCAAF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO.Where(t => t.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO.Single(t => t.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id && t.NCAC533SPCAAF_Id == item.NCAC533SPCAAF_Id);
                                    deactfile.NCAC533SPCAAF_ActiveFlg = false;
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
        public NAACMasterSportsCADTO deactiveStudent(NAACMasterSportsCADTO data)
        {
            try
            {
                var u = _context.NAAC_AC_533_SportsCA_ActivitiesDMO.Where(t => t.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id).SingleOrDefault();
                if (data.NCAC533SPCAA_ActiveFlg == true)
                {
                    u.NCAC533SPCAA_ActiveFlg = false;
                }
                else if (u.NCAC533SPCAA_ActiveFlg == false)
                {
                    u.NCAC533SPCAA_ActiveFlg = true;
                }
                u.NCAC533SPCAA_UpdatedDate = DateTime.Now;
                u.NCAC533SPCAA_UpdatedBy = data.UserId;
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
        public NAACMasterSportsCADTO EditData(NAACMasterSportsCADTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_533_SportsCA_ActivitiesDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC533SPCAA_Year && b.MI_Id == data.MI_Id && b.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id)
                                 select new NAACMasterSportsCADTO
                                 {
                                     NCAC533SPCAA_Id = b.NCAC533SPCAA_Id,
                                     NCAC533SPCAA_NameOfActivities = b.NCAC533SPCAA_NameOfActivities,
                                    
                                     NCAC533SPCAA_ActiveFlg = b.NCAC533SPCAA_ActiveFlg,
                                     NCAC533SPCAA_Year = b.NCAC533SPCAA_Year,
                                   
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC533SPCAA_ActType = b.NCAC533SPCAA_ActType,
                                     NCAC533SPCAA_ActLevel = b.NCAC533SPCAA_ActLevel,
                                     NCAC533SPCAA_StatusFlg = b.NCAC533SPCAA_StatusFlg,

                                 }).Distinct().ToArray();

                data.editfiles = (from a in _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO

                                  where (a.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id && a.NCAC533SPCAAF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC533SPCAAF_FileName,
                                      cfilepath = a.NCAC533SPCAAF_FilePath,
                                      cfiledesc = a.NCAC533SPCAAF_Filedesc,
                                      cfileid = a.NCAC533SPCAAF_Id,
                                      status = a.NCAC533SPCAAF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACMasterSportsCADTO viewuploadflies(NAACMasterSportsCADTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO

                                  where (a.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id && a.NCAC533SPCAAF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC533SPCAA_Id,
                                      cfileid = a.NCAC533SPCAAF_Id,
                                      cfilename = a.NCAC533SPCAAF_FileName,
                                      cfilepath = a.NCAC533SPCAAF_FilePath,
                                      cfiledesc = a.NCAC533SPCAAF_Filedesc,
                                      status = a.NCAC533SPCAAF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACMasterSportsCADTO deleteuploadfile(NAACMasterSportsCADTO data)
        {
            try
            {


                if (data.NCAC533SPCAAF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_533_SportsCA_ActivitiesFilesDMO.Where(e => e.NCAC533SPCAAF_Id == data.NCAC533SPCAAF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC533SPCAAF_ActiveFlg = false;

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

        public NAACMasterSportsCADTO getcomment(NAACMasterSportsCADTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_533_SportsCA_Activities_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC533SPCAAC_RemarksBy == b.Id && a.NCAC533SPCAA_Id == data.NCAC533SPCAA_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC533SPCAAC_Remarks,
                                        commentid = a.NCAC533SPCAAC_Id,
                                        status = a.NCAC533SPCAAC_StatusFlg,
                                        createddate = a.NCAC533SPCAAC_CreatedDate,
                                        activeflag = a.NCAC533SPCAAC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACMasterSportsCADTO getfilecomment(NAACMasterSportsCADTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_533_SportsCA_Activities_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC533SPCAAFC_RemarksBy == b.Id && a.NCAC533SPCAAF_Id == data.NCAC533SPCAAF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC533SPCAAFC_Remarks,
                                        commentid = a.NCAC533SPCAAFC_Id,
                                        status = a.NCAC533SPCAAFC_StatusFlg,
                                        createddate = a.NCAC533SPCAAFC_CreatedDate,
                                        activeflag = a.NCAC533SPCAAFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACMasterSportsCADTO savemedicaldatawisecomments(NAACMasterSportsCADTO data)
        {
            try
            {
                NAAC_AC_533_SportsCA_Activities_CommentsDMO cm = new NAAC_AC_533_SportsCA_Activities_CommentsDMO();
                cm.NCAC533SPCAAC_Remarks = data.Remarks;
                cm.NCAC533SPCAAC_RemarksBy = data.UserId;
                cm.NCAC533SPCAAC_StatusFlg = "";
                cm.NCAC533SPCAAC_ActiveFlag = true;
                cm.NCAC533SPCAAC_CreatedBy = data.UserId;
                cm.NCAC533SPCAAC_CreatedDate = DateTime.Now;
                cm.NCAC533SPCAAC_UpdatedBy = data.UserId;
                cm.NCAC533SPCAAC_UpdatedDate = DateTime.Now;
                cm.NCAC533SPCAA_Id = data.filefkid;
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
        public NAACMasterSportsCADTO savefilewisecomments(NAACMasterSportsCADTO data)
        {
            try
            {
                NAAC_AC_533_SportsCA_Activities_File_CommentsDMO cm = new NAAC_AC_533_SportsCA_Activities_File_CommentsDMO();
                cm.NCAC533SPCAAFC_Remarks = data.Remarks;
                cm.NCAC533SPCAAFC_RemarksBy = data.UserId;
                cm.NCAC533SPCAAFC_StatusFlg = "";
                cm.NCAC533SPCAAFC_ActiveFlag = true;
                cm.NCAC533SPCAAFC_CreatedBy = data.UserId;
                cm.NCAC533SPCAAFC_CreatedDate = DateTime.Now;
                cm.NCAC533SPCAAFC_UpdatedBy = data.UserId;
                cm.NCAC533SPCAAFC_UpdatedDate = DateTime.Now;
                cm.NCAC533SPCAAF_Id = data.filefkid;
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
