using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.NAAC;
using PreadmissionDTOs.NAAC.Admission;
using DomainModel.Model.NAAC.Admission;

namespace NaacServiceHub.Admission.Services
{
    public class IVRM_BranchSubjectMappingImpl : Interface.IVRM_BranchSubjectMappingInterface
    {
        public GeneralContext _GeneralContext;
        public IVRM_BranchSubjectMappingImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public IVRM_Master_Subjects_Branch_DTO loaddata(IVRM_Master_Subjects_Branch_DTO data)
        {
            try
            {
                data.cousrselist = _GeneralContext.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).Distinct().OrderBy(t => t.AMCO_Id).ToArray();

                data.branchlist = _GeneralContext.ClgMasterBranchDMO.Where(t => t.MI_Id == data.MI_Id && t.AMB_ActiveFlag == true).Distinct().OrderBy(t => t.AMB_Id).ToArray();
                data.subjectlist = _GeneralContext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1).Distinct().OrderBy(t => t.ISMS_Id).ToArray();

                data.alldata = (from a in _GeneralContext.MasterCourseDMO
                                from b in _GeneralContext.ClgMasterBranchDMO
                                from s in _GeneralContext.IVRM_School_Master_SubjectsDMO
                                from c in _GeneralContext.IVRM_Master_Subjects_Branch_DMO

                                where (a.MI_Id == b.MI_Id && a.MI_Id == s.MI_Id && s.ISMS_Id == c.ISMS_Id && a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.AMB_ActiveFlag == true && s.ISMS_ActiveFlag == 1)
                                select new IVRM_Master_Subjects_Branch_DTO
                                {
                                    AMCO_CourseName = a.AMCO_CourseName,
                                    AMB_BranchName = b.AMB_BranchName,
                                    ISMS_SubjectName=s.ISMS_SubjectName,
                                    IMSBR_ActiveFlg=c.IMSBR_ActiveFlg,
                                    IMSBR_Id=c.IMSBR_Id,
                                }).Distinct().OrderByDescending(t => t.IMSBR_Id).ToArray();
            }
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
            }
            return data;
        }

        public IVRM_Master_Subjects_Branch_DTO savedata(IVRM_Master_Subjects_Branch_DTO data)
        {
            try
            {
                if (data.IMSBR_Id == 0)
                {
                    var dupliacte = _GeneralContext.IVRM_Master_Subjects_Branch_DMO.Where(t => t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.ISMS_Id == data.ISMS_Id).ToList();

                    if (dupliacte.Count>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_Master_Subjects_Branch_DMO obj = new IVRM_Master_Subjects_Branch_DMO();

                        obj.IMSBR_Id = data.IMSBR_Id;
                        obj.ISMS_Id = data.ISMS_Id;
                        obj.AMCO_Id = data.AMCO_Id;
                        obj.AMB_Id = data.AMB_Id;
                        obj.IMSBR_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj);
                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                    
                }
                else if (data.IMSBR_Id > 0)
                {
                    var dupliacte = _GeneralContext.IVRM_Master_Subjects_Branch_DMO.Where(t => t.AMCO_Id == data.AMCO_Id && t.IMSBR_Id!=data.IMSBR_Id && t.AMB_Id == data.AMB_Id && t.ISMS_Id == data.ISMS_Id).ToList();

                    if (dupliacte.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.IVRM_Master_Subjects_Branch_DMO.Single(t => t.IMSBR_Id == data.IMSBR_Id);

                        update.ISMS_Id = data.ISMS_Id;
                        update.AMCO_Id = data.AMCO_Id;
                        update.AMB_Id = data.AMB_Id;
                        update.UpdatedDate = DateTime.Now;

                        _GeneralContext.Update(update);
                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
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
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
            }
            return data;
        }

        public IVRM_Master_Subjects_Branch_DTO editdata(IVRM_Master_Subjects_Branch_DTO data)
        {
            try
            {
                var edit = _GeneralContext.IVRM_Master_Subjects_Branch_DMO.Where(t => t.IMSBR_Id == data.IMSBR_Id);

                data.editlist = edit.ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_Master_Subjects_Branch_DTO deactiveY(IVRM_Master_Subjects_Branch_DTO data)
        {
            try
            {
                var result = _GeneralContext.IVRM_Master_Subjects_Branch_DMO.SingleOrDefault(t => t.IMSBR_Id == data.IMSBR_Id);
                if (result.IMSBR_ActiveFlg == true)
                {
                    result.IMSBR_ActiveFlg = false;
                }
                else if (result.IMSBR_ActiveFlg == false)
                {
                    result.IMSBR_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;

                _GeneralContext.Update(result);
              int s =  _GeneralContext.SaveChanges();
                if (s>0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_Master_Subjects_Branch_DTO get_Branch(IVRM_Master_Subjects_Branch_DTO data)
        {
            try
            {
                data.branchlist = (from c in _GeneralContext.MasterCourseDMO
                                   from a in  _GeneralContext.ClgMasterBranchDMO                                  
                                   from b in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from d in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   where ( a.MI_Id==c.MI_Id && c.AMCO_Id == c.AMCO_Id  && b.ACAYC_Id==d.ACAYC_Id && b.AMB_Id==a.AMB_Id  && a.MI_Id == data.MI_Id && a.MI_Id==d.MI_Id && c.AMCO_Id==d.AMCO_Id && a.MI_Id==data.MI_Id &&  d.ASMAY_Id==data.ASMAY_Id && c.AMCO_Id==data.AMCO_Id && a.AMB_ActiveFlag == true) select a).Distinct().OrderBy(t => t.AMB_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_Master_Subjects_Branch_DTO get_subject(IVRM_Master_Subjects_Branch_DTO data)
        {
            try
            {
                //data.subjectlist = _GeneralContext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1).Distinct().OrderBy(t => t.ISMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
