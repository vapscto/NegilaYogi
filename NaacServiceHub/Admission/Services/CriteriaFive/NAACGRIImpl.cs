using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACGRIImpl : Interface.NAACGRIInterface
    {
        public GeneralContext _context;
        public NAACGRIImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACGRIDTO loaddatamed(NAACGRIDTO data)
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

              var  alldatalist = (//from a in _context.Academic
                                    from b in _context.NAAC_AC_516_GRIDMO
                                    where (b.MI_Id == b.MI_Id 
                                    //&& b.NCAC516GRI_Year == a.ASMAY_Id && a.Is_Active == true 
                                    && b.MI_Id == data.MI_Id)
                                    select new NAACGRIDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC516GRI_Id = b.NCAC516GRI_Id,
                                        NCAC516GRI_GRIAPP = b.NCAC516GRI_GRIAPP,
                                        NCAC516GRI_GRIRED = b.NCAC516GRI_GRIRED,
                                        NCAC516GRI_AvgTime = b.NCAC516GRI_AvgTime,
                                        NCAC516GRI_ActiveFlg = b.NCAC516GRI_ActiveFlg,
                                        NCAC516GRI_AdpOfguidelinesofRegbodiesFlg = b.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg,
                                        NCAC516GRI_StusgrvOnline_OR_offlineFlg = b.NCAC516GRI_StusgrvOnline_OR_offlineFlg,
                                        NCAC516GRI_CommitteewithminutesFlg = b.NCAC516GRI_CommitteewithminutesFlg,
                                        NCAC516GRI_RecordOfActionTakenFlg = b.NCAC516GRI_RecordOfActionTakenFlg,
                                        NCAC516GRI_StatusFlg=b.NCAC516GRI_StatusFlg
                                    }).Distinct().OrderByDescending(t => t.NCAC516GRI_Id).ToList();

                data.alldatalist = alldatalist.ToArray();
              var alldatalist1= alldatalist.Take(1).ToList();
                data.alldatalist1 = alldatalist1.ToArray();

                if (alldatalist1.Count> 0)
                {
                    data.NCAC516GRI_Id = alldatalist1[0].NCAC516GRI_Id;
                    data.editfiles = (from a in _context.NAAC_AC_516_GRIFilesDMO

                                      where (a.NCAC516GRI_Id == data.NCAC516GRI_Id && a.NCAC516GRIF_ActiveFlg==true)
                                      select new NAACCriteriaFivefileDTO
                                      {
                                          cfilename = a.NCAC516GRIF_FileName,
                                          cfilepath = a.NCAC516GRIF_FilePath,
                                          cfiledesc = a.NCAC516GRIF_Filedesc,
                                          status = a.NCAC516GRIF_StatusFlg,
                                          cfileid = a.NCAC516GRIF_Id,
                                      }).Distinct().ToArray();
                }




             

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACGRIDTO loaddata(NAACGRIDTO data)
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
                                    from b in _context.NAAC_AC_516_GRIDMO
                                    where (a.MI_Id == b.MI_Id && b.NCAC516GRI_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                                    select new NAACGRIDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NCAC516GRI_Id = b.NCAC516GRI_Id,
                                        NCAC516GRI_GRIAPP = b.NCAC516GRI_GRIAPP,
                                        NCAC516GRI_GRIRED = b.NCAC516GRI_GRIRED,
                                        NCAC516GRI_AvgTime = b.NCAC516GRI_AvgTime,
                                        NCAC516GRI_ActiveFlg = b.NCAC516GRI_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC516GRI_AdpOfguidelinesofRegbodiesFlg = b.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg,
                                        NCAC516GRI_StusgrvOnline_OR_offlineFlg = b.NCAC516GRI_StusgrvOnline_OR_offlineFlg,
                                        NCAC516GRI_CommitteewithminutesFlg = b.NCAC516GRI_CommitteewithminutesFlg,
                                        NCAC516GRI_RecordOfActionTakenFlg = b.NCAC516GRI_RecordOfActionTakenFlg,
                                        NCAC516GRI_StatusFlg = b.NCAC516GRI_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC516GRI_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACGRIDTO save(NAACGRIDTO data)
        {
            try
            {
                if (data.NCAC516GRI_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_516_GRIDMO.Where(t => t.MI_Id == data.MI_Id  && t.NCAC516GRI_Year == data.NCAC516GRI_Year).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_516_GRIDMO obj1 = new NAAC_AC_516_GRIDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC516GRI_Year = data.NCAC516GRI_Year;
                        obj1.NCAC516GRI_GRIAPP = data.NCAC516GRI_GRIAPP;
                        obj1.NCAC516GRI_GRIRED = data.NCAC516GRI_GRIRED;
                        obj1.NCAC516GRI_AvgTime = data.NCAC516GRI_AvgTime;
                       
                        obj1.NCAC516GRI_ActiveFlg = true;
                        obj1.NCAC516GRI_CreatedBy = data.UserId;
                        obj1.NCAC516GRI_UpdatedBy = data.UserId;
                        obj1.NCAC516GRI_CreatedDate = DateTime.Now;
                        obj1.NCAC516GRI_UpdatedDate = DateTime.Now;
                        obj1.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg = data.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg;
                        obj1.NCAC516GRI_StusgrvOnline_OR_offlineFlg = data.NCAC516GRI_StusgrvOnline_OR_offlineFlg;
                        obj1.NCAC516GRI_CommitteewithminutesFlg = data.NCAC516GRI_CommitteewithminutesFlg;
                        obj1.NCAC516GRI_RecordOfActionTakenFlg = data.NCAC516GRI_RecordOfActionTakenFlg;
                        obj1.NCAC516GRI_StatusFlg = "";
                        _context.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_516_GRIFilesDMO obb = new NAAC_AC_516_GRIFilesDMO();
                                    obb.NCAC516GRI_Id = obj1.NCAC516GRI_Id;
                                    obb.NCAC516GRIF_FileName = item.cfilename;
                                    obb.NCAC516GRIF_FilePath = item.cfilepath;
                                    obb.NCAC516GRIF_Filedesc = item.cfiledesc;
                                    obb.NCAC516GRIF_StatusFlg = "";
                                    obb.NCAC516GRIF_ActiveFlg = false;
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
                else if (data.NCAC516GRI_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_516_GRIDMO.Where(t => t.MI_Id == data.MI_Id  && t.NCAC516GRI_Year == data.NCAC516GRI_Year && t.NCAC516GRI_Id !=data.NCAC516GRI_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var update = _context.NAAC_AC_516_GRIDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC516GRI_Id == data.NCAC516GRI_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC516GRI_Year = data.NCAC516GRI_Year;
                        update.NCAC516GRI_GRIAPP = data.NCAC516GRI_GRIAPP;
                        update.NCAC516GRI_GRIRED = data.NCAC516GRI_GRIRED;
                        update.NCAC516GRI_AvgTime = data.NCAC516GRI_AvgTime;
                        update.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg = data.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg;
                        update.NCAC516GRI_StusgrvOnline_OR_offlineFlg = data.NCAC516GRI_StusgrvOnline_OR_offlineFlg;
                        update.NCAC516GRI_CommitteewithminutesFlg = data.NCAC516GRI_CommitteewithminutesFlg;
                        update.NCAC516GRI_RecordOfActionTakenFlg = data.NCAC516GRI_RecordOfActionTakenFlg;
                        update.NCAC516GRI_ActiveFlg = true;
                        update.NCAC516GRI_UpdatedBy = data.UserId;
                        update.NCAC516GRI_UpdatedDate = DateTime.Now;
                        _context.Update(update);


                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_516_GRIFilesDMO.Where(t => t.NCAC516GRI_Id == data.NCAC516GRI_Id && !Fid.Contains(t.NCAC516GRIF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_516_GRIFilesDMO.Single(t => t.NCAC516GRI_Id == data.NCAC516GRI_Id && t.NCAC516GRIF_Id == item2.NCAC516GRIF_Id);
                                    deactfile.NCAC516GRIF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_516_GRIFilesDMO.Where(t => t.NCAC516GRIF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC516GRI_Id = data.NCAC516GRI_Id;
                                    filesdata.NCAC516GRIF_FileName = item.cfilename;
                                    filesdata.NCAC516GRIF_FilePath = item.cfilepath;
                                    filesdata.NCAC516GRIF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC516GRIF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_516_GRIFilesDMO obb = new NAAC_AC_516_GRIFilesDMO();
                                            obb.NCAC516GRI_Id = data.NCAC516GRI_Id;
                                            obb.NCAC516GRIF_FileName = item.cfilename;
                                            obb.NCAC516GRIF_FilePath = item.cfilepath;
                                            obb.NCAC516GRIF_Filedesc = item.cfiledesc;
                                            obb.NCAC516GRIF_ActiveFlg = true;
                                            obb.NCAC516GRIF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_516_GRIFilesDMO.Where(t => t.NCAC516GRI_Id == data.NCAC516GRI_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_516_GRIFilesDMO.Single(t => t.NCAC516GRI_Id == data.NCAC516GRI_Id && t.NCAC516GRIF_Id == item.NCAC516GRIF_Id);
                                    deactfile.NCAC516GRIF_ActiveFlg = false;
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
        public NAACGRIDTO deactiveStudent(NAACGRIDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_516_GRIDMO.Where(t => t.NCAC516GRI_Id == data.NCAC516GRI_Id).SingleOrDefault();
                if (data.NCAC516GRI_ActiveFlg == true)
                {
                    u.NCAC516GRI_ActiveFlg = false;
                }
                else if (u.NCAC516GRI_ActiveFlg == false)
                {
                    u.NCAC516GRI_ActiveFlg = true;
                }
                u.NCAC516GRI_UpdatedDate = DateTime.Now;
                u.NCAC516GRI_UpdatedBy = data.UserId;
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
        public NAACGRIDTO EditData(NAACGRIDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_516_GRIDMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC516GRI_Year && b.MI_Id == data.MI_Id && b.NCAC516GRI_Id == data.NCAC516GRI_Id)
                                 select new NAACGRIDTO
                                 {
                                     NCAC516GRI_Id = b.NCAC516GRI_Id,
                                     NCAC516GRI_GRIAPP = b.NCAC516GRI_GRIAPP,
                                     NCAC516GRI_GRIRED = b.NCAC516GRI_GRIRED,
                                     NCAC516GRI_AvgTime = b.NCAC516GRI_AvgTime,
                                     NCAC516GRI_ActiveFlg = b.NCAC516GRI_ActiveFlg,
                                     NCAC516GRI_Year = b.NCAC516GRI_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCAC516GRI_AdpOfguidelinesofRegbodiesFlg = b.NCAC516GRI_AdpOfguidelinesofRegbodiesFlg,
                                     NCAC516GRI_StusgrvOnline_OR_offlineFlg = b.NCAC516GRI_StusgrvOnline_OR_offlineFlg,
                                     NCAC516GRI_CommitteewithminutesFlg = b.NCAC516GRI_CommitteewithminutesFlg,
                                     NCAC516GRI_RecordOfActionTakenFlg = b.NCAC516GRI_RecordOfActionTakenFlg,
                                     NCAC516GRI_StatusFlg=b.NCAC516GRI_StatusFlg
                                 }).Distinct().ToArray();


                data.editfiles = (from a in _context.NAAC_AC_516_GRIFilesDMO

                                  where (a.NCAC516GRI_Id == data.NCAC516GRI_Id && a.NCAC516GRIF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC516GRIF_FileName,
                                      cfilepath = a.NCAC516GRIF_FilePath,
                                      cfiledesc = a.NCAC516GRIF_Filedesc,
                                      cfileid = a.NCAC516GRIF_Id,
                                      status = a.NCAC516GRIF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACGRIDTO viewuploadflies(NAACGRIDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.NAAC_AC_516_GRIFilesDMO

                                  where (a.NCAC516GRI_Id == data.NCAC516GRI_Id && a.NCAC516GRIF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC516GRI_Id,
                                      cfileid = a.NCAC516GRIF_Id,
                                      cfilename = a.NCAC516GRIF_FileName,
                                      cfilepath = a.NCAC516GRIF_FilePath,
                                      cfiledesc = a.NCAC516GRIF_Filedesc,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACGRIDTO deleteuploadfile(NAACGRIDTO data)
        {
            try
            {


                if (data.NCAC516GRIF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_516_GRIFilesDMO.Where(e => e.NCAC516GRIF_Id == data.NCAC516GRIF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC516GRIF_ActiveFlg = false;
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
        public NAACGRIDTO getcomment(NAACGRIDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_516_GRI_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC516GRIC_RemarksBy == b.Id && a.NCAC516GRI_Id == data.NCAC516GRI_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC516GRIC_Remarks,
                                        commentid = a.NCAC516GRIC_Id,
                                        status = a.NCAC516GRIC_StatusFlg,
                                        createddate = a.NCAC516GRIC_CreatedDate,
                                        activeflag = a.NCAC516GRIC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGRIDTO getfilecomment(NAACGRIDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_516_GRI_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC516GRIFC_RemarksBy == b.Id && a.NCAC516GRIF_Id == data.NCAC516GRIF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC516GRIFC_Remarks,
                                        commentid = a.NCAC516GRIFC_Id,
                                        status = a.NCAC516GRIFC_StatusFlg,
                                        createddate = a.NCAC516GRIFC_CreatedDate,
                                        activeflag = a.NCAC516GRIFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGRIDTO savemedicaldatawisecomments(NAACGRIDTO data)
        {
            try
            {
                NAAC_AC_516_GRI_CommentsDMO cm = new NAAC_AC_516_GRI_CommentsDMO();
                cm.NCAC516GRIC_Remarks = data.Remarks;
                cm.NCAC516GRIC_RemarksBy = data.UserId;
                cm.NCAC516GRIC_StatusFlg = "";
                cm.NCAC516GRIC_ActiveFlag = true;
                cm.NCAC516GRIC_CreatedBy = data.UserId;
                cm.NCAC516GRIC_CreatedDate = DateTime.Now;
                cm.NCAC516GRIC_UpdatedBy = data.UserId;
                cm.NCAC516GRIC_UpdatedDate = DateTime.Now;
                cm.NCAC516GRI_Id = data.filefkid;
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
        public NAACGRIDTO savefilewisecomments(NAACGRIDTO data)
        {
            try
            {
                NAAC_AC_516_GRI_File_CommentsDMO cm = new NAAC_AC_516_GRI_File_CommentsDMO();
                cm.NCAC516GRIFC_Remarks = data.Remarks;
                cm.NCAC516GRIFC_RemarksBy = data.UserId;
                cm.NCAC516GRIFC_StatusFlg = "";
                cm.NCAC516GRIFC_ActiveFlag = true;
                cm.NCAC516GRIFC_CreatedBy = data.UserId;
                cm.NCAC516GRIFC_CreatedDate = DateTime.Now;
                cm.NCAC516GRIFC_UpdatedBy = data.UserId;
                cm.NCAC516GRIFC_UpdatedDate = DateTime.Now;
                cm.NCAC516GRIF_Id = data.filefkid;
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
