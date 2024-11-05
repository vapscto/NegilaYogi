using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAACSportsImpl : Interface.NAACSportsInterface
    {
        public GeneralContext _context;
        public NAACSportsImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACSportsDTO loaddata(NAACSportsDTO data)
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
                                    from b in _context.NAAC_AC_531_SportsCADMO
                                    from c in _context.NAAC_AC_531_SportsCA_StudentsDMO
                                    from m in _context.Adm_Master_College_StudentDMO
                                    from n in _context.Adm_College_Yearly_StudentDMO

                                    where (a.MI_Id == b.MI_Id && b.NCAC531SPCA_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && b.NCAC531SPCA_Id == c.NCAC531SPCA_Id && a.MI_Id==b.MI_Id && m.MI_Id==a.MI_Id && n.ASMAY_Id==a.ASMAY_Id && m.AMCST_Id==n.AMCST_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 & m.AMCST_Id == c.AMCST_Id)
                                    select new NAACSportsDTO
                                    {
                                        MI_Id = c.MI_Id,
                                        NCAC531SPCA_Id = c.NCAC531SPCA_Id,
                                        NCAC531SPCAS_Id = c.NCAC531SPCAS_Id,
                                        NCAC531SPCAS_AwardName = c.NCAC531SPCAS_AwardName,
                                        NCAC531SPCA_NoOfStudents = b.NCAC531SPCA_NoOfStudents,
                                        NCAC531SPCAS_NatOrInterNatFlg=c.NCAC531SPCAS_NatOrInterNatFlg,
                                        NCAC531SPCAS_SportsCAIEEEFlg=c.NCAC531SPCAS_SportsCAIEEEFlg,
                                        NCAC531SPCA_ActiveFlg = b.NCAC531SPCA_ActiveFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                        AMCST_MiddleName = m.AMCST_MiddleName,
                                        AMCST_LastName = m.AMCST_LastName,
                                        AMCST_AdmNo = m.AMCST_AdmNo,
                                        AMCST_Id = m.AMCST_Id,
                                        NCAC531SPCAS_StatusFlg = c.NCAC531SPCAS_StatusFlg,
                                    }).Distinct().OrderByDescending(t => t.NCAC531SPCA_Id).ToArray();



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }




       
        public NAACSportsDTO save(NAACSportsDTO data)
        {
            try
            {
                if (data.NCAC531SPCA_Id == 0)
                {
                    var duplicate = (from a in _context.NAAC_AC_531_SportsCADMO
                                     from b in _context.NAAC_AC_531_SportsCA_StudentsDMO
                                     where a.MI_Id == data.MI_Id && a.NCAC531SPCA_Id == b.NCAC531SPCA_Id && a.NCAC531SPCA_Year == data.NCAC531SPCA_Year && b.NCAC531SPCAS_AwardName == data.NCAC531SPCAS_AwardName && b.AMCST_Id == data.AMCST_Id select a).Distinct().ToList();


                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_531_SportsCADMO obj1 = new NAAC_AC_531_SportsCADMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC531SPCA_Year = data.NCAC531SPCA_Year;
                        obj1.NCAC531SPCA_NoOfStudents = data.NCAC531SPCA_NoOfStudents;
                        obj1.NCAC531SPCA_ActiveFlg = true;
                        obj1.NCAC531SPCA_CreatedBy = data.UserId;
                        obj1.NCAC531SPCA_UpdatedBy = data.UserId;
                        obj1.NCAC531SPCA_CreatedDate = DateTime.Now;
                        obj1.NCAC531SPCA_UpdatedDate = DateTime.Now;
                        obj1.NCAC531SPCA_StatusFlg = "";
                        _context.Add(obj1);


                        NAAC_AC_531_SportsCA_StudentsDMO obj2 = new NAAC_AC_531_SportsCA_StudentsDMO();
                        obj2.MI_Id = data.MI_Id;
                        obj2.NCAC531SPCA_Id = obj1.NCAC531SPCA_Id;
                        obj2.NCAC531SPCAS_AwardName = data.NCAC531SPCAS_AwardName;
                        obj2.AMCST_Id = data.AMCST_Id;
                        obj2.NCAC531SPCAS_NatOrInterNatFlg = data.NCAC531SPCAS_NatOrInterNatFlg;
                        obj2.NCAC531SPCAS_SportsCAIEEEFlg = data.NCAC531SPCAS_SportsCAIEEEFlg;
                        obj2.NCAC531SPCAS_ActiveFlg = true;
                        obj2.NCAC531SPCAS_CreatedBy = data.UserId;
                        obj2.NCAC531SPCAS_UpdatedBy = data.UserId;
                        obj2.NCAC531SPCAS_CreatedDate = DateTime.Now;
                        obj2.NCAC531SPCAS_UpdatedDate = DateTime.Now;
                        obj2.NCAC531SPCAS_StatusFlg = "";
                        _context.Add(obj2);


                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    NAAC_AC_531_SportsCAFilesDMO obb = new NAAC_AC_531_SportsCAFilesDMO();


                                    obb.NCAC531SPCA_Id = obj1.NCAC531SPCA_Id;
                                    obb.NCAC531SPCAF_FileName = item.cfilename;
                                    obb.NCAC531SPCAF_FilePath = item.cfilepath;
                                    obb.NCAC531SPCAF_Filedesc = item.cfiledesc;
                                    obb.NCAC531SPCAF_StatusFlg = "";
                                    obb.NCAC531SPCAF_ActiveFlg = true;

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
                else if (data.NCAC531SPCA_Id > 0)
                {
                    var duplicate = (from a in _context.NAAC_AC_531_SportsCADMO
                                    from b in _context.NAAC_AC_531_SportsCA_StudentsDMO
                                    where a.MI_Id == data.MI_Id && a.NCAC531SPCA_Id == b.NCAC531SPCA_Id && a.NCAC531SPCA_Year == data.NCAC531SPCA_Year && b.NCAC531SPCAS_AwardName == data.NCAC531SPCAS_AwardName && b.AMCST_Id == data.AMCST_Id &&  a.NCAC531SPCA_Id != data.NCAC531SPCA_Id select a).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {


                        //var removefile = _context.NAAC_AC_531_SportsCAFilesDMO.Where(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id).Distinct().ToList();
                        //if (removefile.Count > 0)
                        //{
                        //    foreach (var item in removefile)
                        //    {
                        //        _context.Remove(item);
                        //    }
                        //}


                        var update = _context.NAAC_AC_531_SportsCADMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC531SPCA_Id == data.NCAC531SPCA_Id).SingleOrDefault();
                        update.MI_Id = data.MI_Id;
                        update.NCAC531SPCA_Year = data.NCAC531SPCA_Year;
                        update.NCAC531SPCA_NoOfStudents = data.NCAC531SPCA_NoOfStudents;
                        update.NCAC531SPCA_ActiveFlg = true;

                        update.NCAC531SPCA_UpdatedBy = data.UserId;

                        update.NCAC531SPCA_UpdatedDate = DateTime.Now;
                        _context.Update(update);


                        var update1 = _context.NAAC_AC_531_SportsCA_StudentsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC531SPCA_Id == data.NCAC531SPCA_Id).SingleOrDefault();
                        update1.MI_Id = data.MI_Id;
                        update1.NCAC531SPCA_Id = data.NCAC531SPCA_Id;
                        update1.NCAC531SPCAS_AwardName = data.NCAC531SPCAS_AwardName;
                        update1.AMCST_Id = data.AMCST_Id;
                        update1.NCAC531SPCAS_NatOrInterNatFlg = data.NCAC531SPCAS_NatOrInterNatFlg;
                        update1.NCAC531SPCAS_SportsCAIEEEFlg = data.NCAC531SPCAS_SportsCAIEEEFlg;
                        update1.NCAC531SPCAS_ActiveFlg = true;
                        update1.NCAC531SPCAS_UpdatedBy = data.UserId;
                        update1.NCAC531SPCAS_UpdatedDate = DateTime.Now;
                        _context.Update(update1);
                        if (data.filelist.Length > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.cfileid);
                            }
                            var removefile1 = _context.NAAC_AC_531_SportsCAFilesDMO.Where(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id && !Fid.Contains(t.NCAC531SPCAF_Id)).Distinct().ToList();

                            if (removefile1.Count > 0)
                            {
                                foreach (var item2 in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_531_SportsCAFilesDMO.Single(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id && t.NCAC531SPCAF_Id == item2.NCAC531SPCAF_Id);
                                    deactfile.NCAC531SPCAF_ActiveFlg = false;
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
                                    var filesdata = _context.NAAC_AC_531_SportsCAFilesDMO.Where(t => t.NCAC531SPCAF_Id == item.cfileid).FirstOrDefault();
                                    filesdata.NCAC531SPCA_Id = data.NCAC531SPCA_Id;
                                    filesdata.NCAC531SPCAF_FileName = item.cfilename;
                                    filesdata.NCAC531SPCAF_FilePath = item.cfilepath;
                                    filesdata.NCAC531SPCAF_Filedesc = item.cfiledesc;
                                    filesdata.NCAC531SPCAF_ActiveFlg = true;
                                    _context.Update(filesdata);


                                }
                                else
                                {
                                    if (item.cfileid == 0)
                                    {
                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            NAAC_AC_531_SportsCAFilesDMO obb = new NAAC_AC_531_SportsCAFilesDMO();
                                            obb.NCAC531SPCA_Id = data.NCAC531SPCA_Id;
                                            obb.NCAC531SPCAF_FileName = item.cfilename;
                                            obb.NCAC531SPCAF_FilePath = item.cfilepath;
                                            obb.NCAC531SPCAF_Filedesc = item.cfiledesc;
                                            obb.NCAC531SPCAF_ActiveFlg = true;
                                            obb.NCAC531SPCAF_StatusFlg = "";

                                            _context.Add(obb);

                                        }
                                    }
                                }
                            }



                        }
                        else
                        {

                            var removefile1 = _context.NAAC_AC_531_SportsCAFilesDMO.Where(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id).Distinct().ToList();
                            if (removefile1.Count > 0)
                            {
                                foreach (var item in removefile1)
                                {
                                    var deactfile = _context.NAAC_AC_531_SportsCAFilesDMO.Single(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id && t.NCAC531SPCAF_Id == item.NCAC531SPCAF_Id);
                                    deactfile.NCAC531SPCAF_ActiveFlg = false;
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
        public NAACSportsDTO deactiveStudent(NAACSportsDTO data)
        {
            try
            {
                var u = _context.NAAC_AC_531_SportsCADMO.Where(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id).SingleOrDefault();
                var s = _context.NAAC_AC_531_SportsCA_StudentsDMO.Where(t => t.NCAC531SPCA_Id == data.NCAC531SPCA_Id).SingleOrDefault();

                if (u.NCAC531SPCA_ActiveFlg == true)
                {
                    u.NCAC531SPCA_ActiveFlg = false;
                    s.NCAC531SPCAS_ActiveFlg = false;
                }
                else if (u.NCAC531SPCA_ActiveFlg == false)
                {
                    u.NCAC531SPCA_ActiveFlg = true;
                    s.NCAC531SPCAS_ActiveFlg = true;
                }
                u.NCAC531SPCA_UpdatedDate = DateTime.Now;
                u.NCAC531SPCA_UpdatedBy = data.UserId;
                s.NCAC531SPCAS_UpdatedDate = DateTime.Now;
                s.NCAC531SPCAS_UpdatedBy = data.UserId;
                _context.Update(u);
                _context.Update(s);
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
        public NAACSportsDTO EditData(NAACSportsDTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_AC_531_SportsCADMO
                                 from c in _context.NAAC_AC_531_SportsCA_StudentsDMO
                                 from m in _context.Adm_Master_College_StudentDMO
                                 from n in _context.Adm_College_Yearly_StudentDMO

                                 where (a.MI_Id == b.MI_Id && b.NCAC531SPCA_Year == a.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && b.NCAC531SPCA_Id == c.NCAC531SPCA_Id && a.MI_Id == b.MI_Id && m.MI_Id == a.MI_Id && n.ASMAY_Id == a.ASMAY_Id && m.AMCST_Id == n.AMCST_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && b.NCAC531SPCA_Id==data.NCAC531SPCA_Id && n.AMCST_Id==c.AMCST_Id)
                                 select new NAACSportsDTO
                                 {
                                     NCAC531SPCA_Id = c.NCAC531SPCA_Id,
                                     NCAC531SPCAS_Id = c.NCAC531SPCAS_Id,
                                     NCAC531SPCAS_AwardName = c.NCAC531SPCAS_AwardName,
                                     NCAC531SPCA_NoOfStudents = b.NCAC531SPCA_NoOfStudents,
                                     NCAC531SPCAS_NatOrInterNatFlg = c.NCAC531SPCAS_NatOrInterNatFlg,
                                     NCAC531SPCAS_SportsCAIEEEFlg = c.NCAC531SPCAS_SportsCAIEEEFlg,
                                     NCAC531SPCA_ActiveFlg = b.NCAC531SPCA_ActiveFlg,
                                     ASMAY_Year = a.ASMAY_Year,
                                     AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                     AMCST_MiddleName = m.AMCST_MiddleName,
                                     AMCST_LastName = m.AMCST_LastName,
                                     AMCST_AdmNo = m.AMCST_AdmNo,
                                     AMCST_Id = m.AMCST_Id,
                                     NCAC531SPCA_Year = b.NCAC531SPCA_Year,
                                     AMCO_Id = n.AMCO_Id,
                                     AMB_Id = n.AMB_Id,
                                     AMSE_Id = n.AMSE_Id,
                                     ACMS_Id = n.ACMS_Id,
                                     NCAC531SPCAS_StatusFlg = c.NCAC531SPCAS_StatusFlg,
                                 }).Distinct().OrderByDescending(t => t.NCAC531SPCA_Id).ToArray();

                data.editfiles = (from a in _context.NAAC_AC_531_SportsCAFilesDMO

                                  where (a.NCAC531SPCA_Id == data.NCAC531SPCA_Id && a.NCAC531SPCAF_ActiveFlg == true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      cfilename = a.NCAC531SPCAF_FileName,
                                      cfilepath = a.NCAC531SPCAF_FilePath,
                                      cfiledesc = a.NCAC531SPCAF_Filedesc,
                                      cfileid = a.NCAC531SPCAF_Id,
                                      status = a.NCAC531SPCAF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }


        public NAACSportsDTO viewuploadflies(NAACSportsDTO data)
        {
            try
            {



                data.editfiles = (from a in _context.NAAC_AC_531_SportsCAFilesDMO

                                  where (a.NCAC531SPCA_Id == data.NCAC531SPCA_Id && a.NCAC531SPCAF_ActiveFlg==true)
                                  select new NAACCriteriaFivefileDTO
                                  {
                                      gridid = a.NCAC531SPCA_Id,
                                      cfileid = a.NCAC531SPCAF_Id,
                                      cfilename = a.NCAC531SPCAF_FileName,
                                      cfilepath = a.NCAC531SPCAF_FilePath,
                                      cfiledesc = a.NCAC531SPCAF_Filedesc,
                                      status = a.NCAC531SPCAF_StatusFlg,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAACSportsDTO deleteuploadfile(NAACSportsDTO data)
        {
            try
            {


                if (data.NCAC531SPCAF_Id > 0)
                {
                    var deletefile = _context.NAAC_AC_531_SportsCAFilesDMO.Where(e => e.NCAC531SPCAF_Id == data.NCAC531SPCAF_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            item.NCAC531SPCAF_ActiveFlg = false;
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

        public NAACSportsDTO get_course(NAACSportsDTO data)
        {
            try
            {
                data.courselist = (from a in _context.MasterCourseDMO
                                   from b in _context.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAACSportsDTO get_branch(NAACSportsDTO data)
        {
            try
            {
                data.branchlist = (from a in _context.ClgMasterBranchDMO
                                   from b in _context.CLG_Adm_College_AY_CourseDMO
                                   from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                   where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                   select a).Distinct().OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAACSportsDTO get_sems(NAACSportsDTO data)
        {
            try
            {
                data.semisterlist = (from a in _context.CLG_Adm_Master_SemesterDMO
                                     from b in _context.CLG_Adm_College_AY_CourseDMO
                                     from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select a).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAACSportsDTO get_section(NAACSportsDTO data)
        {
            try
            {
                data.sectionlist = (from a in _context.Adm_College_Yearly_StudentDMO
                                    from b in _context.Adm_College_Master_SectionDMO
                                    where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id
                                    select new NAACSportsDTO
                                    {
                                        ACMS_Id = b.ACMS_Id,
                                        ACMS_SectionName = b.ACMS_SectionName,
                                        ACMS_Order = b.ACMS_Order
                                    }).Distinct().OrderBy(t => t.ACMS_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public NAACSportsDTO GetStudentDetails(NAACSportsDTO data)
        {
            try
            {
                data.studentlist = (from m in _context.Adm_Master_College_StudentDMO
                                    from n in _context.Adm_College_Yearly_StudentDMO
                                    where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                    select new NAACSportsDTO
                                    {
                                        AMCST_Id = m.AMCST_Id,
                                        MI_Id = m.MI_Id,
                                        ASMAY_Id = m.ASMAY_Id,
                                        AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                        AMCST_MiddleName = m.AMCST_MiddleName,
                                        AMCST_LastName = m.AMCST_LastName,
                                        AMCST_AdmNo = m.AMCST_AdmNo

                                    }).Distinct().OrderBy(t => t.AMCST_FirstName).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAACSportsDTO getcomment(NAACSportsDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_531_SportsCA_Students_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC531SPCASC_RemarksBy == b.Id && a.NCAC531SPCAS_Id == data.NCAC531SPCAS_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC531SPCASC_Remarks,
                                        commentid = a.NCAC531SPCASC_Id,
                                        status = a.NCAC531SPCASC_StatusFlg,
                                        createddate = a.NCAC531SPCASC_CreatedDate,
                                        activeflag = a.NCAC531SPCASC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACSportsDTO getfilecomment(NAACSportsDTO data)
        {
            try
            {

                data.commentlist = (from a in _context.NAAC_AC_531_SportsCA_File_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCAC531SPCAFC_RemarksBy == b.Id && a.NCAC531SPCAF_Id == data.NCAC531SPCAF_Id)
                                    select new commentsdto
                                    {
                                        remarks = a.NCAC531SPCAFC_Remarks,
                                        commentid = a.NCAC531SPCAFC_Id,
                                        status = a.NCAC531SPCAFC_StatusFlg,
                                        createddate = a.NCAC531SPCAFC_CreatedDate,
                                        activeflag = a.NCAC531SPCAFC_ActiveFlag,
                                        username = b.UserName,
                                    }).Distinct().OrderByDescending(t => t.createddate).ToArray();



            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
        public NAACSportsDTO savemedicaldatawisecomments(NAACSportsDTO data)
        {
            try
            {
                NAAC_AC_531_SportsCA_Students_CommentsDMO cm = new NAAC_AC_531_SportsCA_Students_CommentsDMO();
                cm.NCAC531SPCASC_Remarks = data.Remarks;
                cm.NCAC531SPCASC_RemarksBy = data.UserId;
                cm.NCAC531SPCASC_StatusFlg = "";
                cm.NCAC531SPCASC_ActiveFlag = true;
                cm.NCAC531SPCASC_CreatedBy = data.UserId;
                cm.NCAC531SPCASC_CreatedDate = DateTime.Now;
                cm.NCAC531SPCASC_UpdatedBy = data.UserId;
                cm.NCAC531SPCASC_UpdatedDate = DateTime.Now;
                cm.NCAC531SPCAS_Id = data.filefkid;
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
        public NAACSportsDTO savefilewisecomments(NAACSportsDTO data)
        {
            try
            {
                NAAC_AC_531_SportsCA_File_CommentsDMO cm = new NAAC_AC_531_SportsCA_File_CommentsDMO();
                cm.NCAC531SPCAFC_Remarks = data.Remarks;
                cm.NCAC531SPCAFC_RemarksBy = data.UserId;
                cm.NCAC531SPCAFC_StatusFlg = "";
                cm.NCAC531SPCAFC_ActiveFlag = true;
                cm.NCAC531SPCAFC_CreatedBy = data.UserId;
                cm.NCAC531SPCAFC_CreatedDate = DateTime.Now;
                cm.NCAC531SPCAFC_UpdatedBy = data.UserId;
                cm.NCAC531SPCAFC_UpdatedDate = DateTime.Now;
                cm.NCAC531SPCAF_Id = data.filefkid;
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
