using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Documents;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Services
{
    public class NAACGeneralCriteriaImpl : Interface.NAACGeneralCriteriaInterface
    {
        public GeneralContext _context;
        public NAACGeneralCriteriaImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACGeneralCriteriaDTO loaddata(NAACGeneralCriteriaDTO data)
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


                data.criterialist = (from a in _context.NaacDocumentUploadDMO
                                     where (a.NAACSL_ActiveFlag == true)
                                     select new NAACGeneralCriteriaDTO
                                     {
                                         NAACSL_Id = a.NAACSL_Id,
                                         NAACSL_SLNo = a.NAACSL_SLNo,
                                         NAACSL_SLNoDescription = a.NAACSL_SLNoDescription,
                                     }).Distinct().ToArray();


                data.alldatalist = (from a in _context.NaacDocumentUploadDMO
                                    from b in _context.NAAC_AC_Criteria_GeneralDMO
                                    where ( b.NAACSL_Id == a.NAACSL_Id && a.NAACSL_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                    select new NAACGeneralCriteriaDTO
                                    {
                                        MI_Id = b.MI_Id,
                                        NAACSL_Id = b.NAACSL_Id,
                                        NCACCRGEN_Id = b.NCACCRGEN_Id,
                                        NCACCRGEN_CriteriaDescription = b.NCACCRGEN_CriteriaDescription,
                                        NCACCRGEN_ActiveFlg = b.NCACCRGEN_ActiveFlg,
                                        NAACSL_SLNo = a.NAACSL_SLNo,
                                        NAACSL_SLNoDescription = a.NAACSL_SLNoDescription,
                                    }).Distinct().OrderByDescending(t => t.NCACCRGEN_Id).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACGeneralCriteriaDTO save(NAACGeneralCriteriaDTO data)
        {
            try
            {
                if (data.NCACCRGEN_Id == 0)
                {
                    var duplicate = _context.NAAC_AC_Criteria_GeneralDMO.Where(t => t.MI_Id == data.MI_Id && t.NCACCRGEN_CriteriaDescription.ToLower() == data.NCACCRGEN_CriteriaDescription.ToLower()).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_Criteria_GeneralDMO obj1 = new NAAC_AC_Criteria_GeneralDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.MT_Id = data.MT_Id;
                        obj1.NAACSL_Id = data.NAACSL_Id;
                        obj1.NCACCRGEN_CriteriaDescription = data.NCACCRGEN_CriteriaDescription;
                        obj1.NCACCRGEN_ActiveFlg = true;
                        obj1.NCACCRGEN_CreatedBy = data.UserId;
                        obj1.NCACCRGEN_UpdatedBy = data.UserId;
                        obj1.NCACCRGEN_CreatedDate = DateTime.Now;
                        obj1.NCACCRGEN_UpdatedDate = DateTime.Now;
                        _context.Add(obj1);

                        if (data.filelist.Length>0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath !=null && item.cfilepath !="")
                                {
                                    NAAC_AC_Criteria_General_FilesDMO obb = new NAAC_AC_Criteria_General_FilesDMO();
                                    obb.NCACCRGEN_Id = obj1.NCACCRGEN_Id;
                                    obb.NCACCRGENAF_FileName = item.cfilename;
                                    obb.NCACCRGENAF_FilePath = item.cfilepath;
                                    obb.NCACCRGENAF_AdditionalFileDesc = item.cfiledesc;
                                    obb.NCACCRGENAF_ActiveFlg = true;
                                    obb.NCACCRGENAF_UpdatedBy = data.UserId;
                                    obb.NCACCRGENAF_CreatedBy = data.UserId;
                                    obb.NCACCRGENAF_UpdatedDate = DateTime.Now;
                                    obb.NCACCRGENAF_CreatedDate = DateTime.Now;
                                    _context.Add(obb);
                                }
                               

                            }
                        }
                        if (data.linklist.Length > 0)
                        {
                            foreach (var item in data.linklist)
                            {

                                if (item.linkname != null && item.linkname != "")
                                {
                                    NAAC_AC_Criteria_General_LinkDMO obb = new NAAC_AC_Criteria_General_LinkDMO();
                                    obb.NCACCRGEN_Id = obj1.NCACCRGEN_Id;
                                    obb.NCACCRGENLI_LinkName = item.linkname;
                                    obb.NCACCRGENLI_LinkDescription = item.linkdesc;
                                    obb.NCACCRGENLI_ActiveFlg = true;
                                    obb.NCACCRGENLI_UpdatedBy = data.UserId;
                                    obb.NCACCRGENLI_CreatedBy = data.UserId;
                                    obb.NCACCRGENLI_UpdatedDate = DateTime.Now;
                                    obb.NCACCRGENLI_CreatedDate = DateTime.Now;
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
                else if (data.NCACCRGEN_Id > 0)
                {
                    var duplicate = _context.NAAC_AC_Criteria_GeneralDMO.Where(t => t.MI_Id == data.MI_Id && t.NCACCRGEN_CriteriaDescription == data.NCACCRGEN_CriteriaDescription && t.NAACSL_Id == data.NAACSL_Id && t.NCACCRGEN_Id != data.NCACCRGEN_Id).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var removefile = _context.NAAC_AC_Criteria_General_FilesDMO.Where(t => t.NCACCRGEN_Id == data.NCACCRGEN_Id).Distinct().ToList();
                        if (removefile.Count>0)
                        {
                            foreach (var item in removefile)
                            {
                                _context.Remove(item);
                            }
                        }


                        var removelink = _context.NAAC_AC_Criteria_General_LinkDMO.Where(t => t.NCACCRGEN_Id == data.NCACCRGEN_Id).Distinct().ToList();
                        if (removelink.Count > 0)
                        {
                            foreach (var item in removelink)
                            {
                                _context.Remove(item);
                            }
                        }
                        var update = _context.NAAC_AC_Criteria_GeneralDMO.Where(t => t.MI_Id == data.MI_Id && t.NCACCRGEN_Id == data.NCACCRGEN_Id).SingleOrDefault();
                      
                        update.MI_Id = data.MI_Id;
                        update.MT_Id = data.MT_Id;
                        update.NAACSL_Id = data.NAACSL_Id;
                        update.NCACCRGEN_CriteriaDescription = data.NCACCRGEN_CriteriaDescription;
                        update.NCACCRGEN_UpdatedBy = data.UserId;
                        
                        update.NCACCRGEN_UpdatedDate = DateTime.Now;
                        _context.Update(update);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_Criteria_General_FilesDMO obb = new NAAC_AC_Criteria_General_FilesDMO();
                                    obb.NCACCRGEN_Id = data.NCACCRGEN_Id;
                                 
                                    obb.NCACCRGENAF_FileName = item.cfilename;
                                    obb.NCACCRGENAF_FilePath = item.cfilepath;
                                    obb.NCACCRGENAF_AdditionalFileDesc = item.cfiledesc;
                                    obb.NCACCRGENAF_ActiveFlg = true;
                                    obb.NCACCRGENAF_UpdatedBy = data.UserId;
                                    obb.NCACCRGENAF_CreatedBy = data.UserId;
                                    obb.NCACCRGENAF_UpdatedDate = DateTime.Now;
                                    obb.NCACCRGENAF_CreatedDate = DateTime.Now;
                                    _context.Add(obb);
                                }


                            }
                        }
                        if (data.linklist.Length > 0)
                        {
                            foreach (var item in data.linklist)
                            {

                                if (item.linkname != null && item.linkname != "")
                                {
                                    NAAC_AC_Criteria_General_LinkDMO obb = new NAAC_AC_Criteria_General_LinkDMO();
                                    obb.NCACCRGEN_Id = data.NCACCRGEN_Id;
                                    obb.NCACCRGENLI_LinkName = item.linkname;
                                    obb.NCACCRGENLI_LinkDescription = item.linkdesc;
                                    obb.NCACCRGENLI_ActiveFlg = true;
                                    obb.NCACCRGENLI_UpdatedBy = data.UserId;
                                    obb.NCACCRGENLI_CreatedBy = data.UserId;
                                    obb.NCACCRGENLI_UpdatedDate = DateTime.Now;
                                    obb.NCACCRGENLI_CreatedDate = DateTime.Now;
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
        public NAACGeneralCriteriaDTO deactiveStudent(NAACGeneralCriteriaDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_Criteria_GeneralDMO.Where(t => t.NCACCRGEN_Id == data.NCACCRGEN_Id).SingleOrDefault();
                if (data.NCACCRGEN_ActiveFlg == true)
                {
                    u.NCACCRGEN_ActiveFlg = false;
                }
                else if (u.NCACCRGEN_ActiveFlg == false)
                {
                    u.NCACCRGEN_ActiveFlg = true;
                }
                u.NCACCRGEN_UpdatedDate = DateTime.Now;
                u.NCACCRGEN_UpdatedBy = data.UserId;
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
        public NAACGeneralCriteriaDTO EditData(NAACGeneralCriteriaDTO data)
        {
            try
            {
                data.editlist = (from a in _context.NaacDocumentUploadDMO
                                 from b in _context.NAAC_AC_Criteria_GeneralDMO
                                 where ( a.NAACSL_Id == b.NAACSL_Id && b.MI_Id == data.MI_Id && b.NCACCRGEN_Id == data.NCACCRGEN_Id)
                                 select new NAACGeneralCriteriaDTO
                                 {
                                     NAACSL_Id = b.NAACSL_Id,
                                     NCACCRGEN_Id = b.NCACCRGEN_Id,
                                     NCACCRGEN_CriteriaDescription = b.NCACCRGEN_CriteriaDescription,
                                     NCACCRGEN_ActiveFlg = b.NCACCRGEN_ActiveFlg,
                                     NAACSL_SLNo = a.NAACSL_SLNo,
                                     NAACSL_SLNoDescription = a.NAACSL_SLNoDescription,
                                 }).Distinct().ToArray();


                data.editfiles=(from a in _context.NAAC_AC_Criteria_General_FilesDMO

                                where (a.NCACCRGEN_Id == data.NCACCRGEN_Id)
                                 select new NAACCriteriaFivefileDTO
                                 {
                                    cfilename=a.NCACCRGENAF_FileName,
                                    cfilepath=a.NCACCRGENAF_FilePath,
                                    cfiledesc=a.NCACCRGENAF_AdditionalFileDesc,
                                 }).Distinct().ToArray();

                data.editlink = (from a in _context.NAAC_AC_Criteria_General_LinkDMO
                                  where (a.NCACCRGEN_Id == data.NCACCRGEN_Id)
                                  select new linkdto
                                  {
                                      linkname = a.NCACCRGENLI_LinkName,
                                      linkdesc = a.NCACCRGENLI_LinkDescription,
                                      linkid = a.NCACCRGENLI_Id,
                                  }).Distinct().ToArray();


            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGeneralCriteriaDTO viewuploadflies(NAACGeneralCriteriaDTO data)
        {
            try
            {
                data.editfiles=(from a in _context.NAAC_AC_Criteria_General_FilesDMO
                                where (a.NCACCRGEN_Id == data.NCACCRGEN_Id)
                                 select new NAACCriteriaFivefileDTO
                                 {
                                     gridid = a.NCACCRGEN_Id,
                                    cfileid=a.NCACCRGENF_Id,
                                    cfilename=a.NCACCRGENAF_FileName,
                                    cfilepath=a.NCACCRGENAF_FilePath,
                                    cfiledesc=a.NCACCRGENAF_AdditionalFileDesc,

                                 }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACGeneralCriteriaDTO viewlink(NAACGeneralCriteriaDTO data)
        {
            try
            {
                data.editfiles=(from a in _context.NAAC_AC_Criteria_General_LinkDMO
                                where (a.NCACCRGEN_Id == data.NCACCRGEN_Id)
                                 select new linkdto
                                 {
                                     gridid = a.NCACCRGEN_Id,
                                    linkid=a.NCACCRGENLI_Id,
                                    linkname=a.NCACCRGENLI_LinkName,
                                    linkdesc=a.NCACCRGENLI_LinkDescription,
                                 }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACGeneralCriteriaDTO deleteuploadfile(NAACGeneralCriteriaDTO data)
        {
            try
            {


                if (data.NCACCRGENF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_Criteria_General_FilesDMO.Where(e => e.NCACCRGENF_Id == data.NCACCRGENF_Id).ToList();

                    if (deletefile.Count>0)
                    {
                        foreach (var item in deletefile)
                        {
                            _context.Remove(item);
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
        public NAACGeneralCriteriaDTO deletelink(NAACGeneralCriteriaDTO data)
        {
            try
            {


                if (data.NCACCRGENLI_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_Criteria_General_LinkDMO.Where(e => e.NCACCRGENLI_Id == data.NCACCRGENLI_Id).ToList();

                    if (deletefile.Count>0)
                    {
                        foreach (var item in deletefile)
                        {
                            _context.Remove(item);
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

    }
}
