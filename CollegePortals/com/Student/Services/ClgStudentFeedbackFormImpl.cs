using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;

namespace CollegePortals.com.Student.Services
{
    public class ClgStudentFeedbackFormImpl : Interfaces.ClgStudentFeedbackFormInterface
    {
        private static ConcurrentDictionary<string, ClgStudentFeedbackFormDTO> _login =
           new ConcurrentDictionary<string, ClgStudentFeedbackFormDTO>();
        private CollegeportalContext _ClgPortalContext;
        public ClgStudentFeedbackFormImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }
        public ClgStudentFeedbackFormDTO getloaddata(ClgStudentFeedbackFormDTO data)
        {
            try
            {
                data.instname = _ClgPortalContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.get_feedback = _ClgPortalContext.Adm_College_Student_GFeedbackDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgStudentFeedbackFormDTO savefeedback(ClgStudentFeedbackFormDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var cbsids = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                              from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                              from c in _ClgPortalContext.MasterCourseDMO
                              from d in _ClgPortalContext.ClgMasterBranchDMO
                              from e in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                              from f in _ClgPortalContext.academicYearDMO
                              where (a.AMCST_Id == b.AMCST_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCST_Id == data.AMCST_Id)
                              select new ClgStudentFeedbackFormDTO
                              {
                                  AMCST_Id = b.AMCST_Id,
                                  AMCO_Id = b.AMCO_Id,
                                  AMB_Id = b.AMB_Id,
                                  AMSE_Id = b.AMSE_Id
                              }
                             ).Distinct().ToList();
                
                Adm_College_Student_GFeedbackDMO feedback = new Adm_College_Student_GFeedbackDMO();
                feedback.MI_Id = data.MI_Id;
                feedback.AMCST_Id = data.AMCST_Id;
                feedback.ASMAY_Id = data.ASMAY_Id;
                feedback.AMCO_Id = cbsids.FirstOrDefault().AMCO_Id;
                feedback.AMB_Id = cbsids.FirstOrDefault().AMB_Id;
                feedback.AMSE_Id = cbsids.FirstOrDefault().AMSE_Id;
                feedback.ACSGFE_Feedback = data.ACSGFE_Feedback;
                feedback.ACSGFE_FeedbackDate = indianTime;
                feedback.ACSGFE_ActiveFlag = true;
                feedback.ACSGFE_CreatedBy = data.AMCST_Id;
                feedback.ACSGFE_UpdatedBy = data.AMCST_Id;

                feedback.CreatedDate = DateTime.Now;
                feedback.UpdatedDate = DateTime.Now;
                _ClgPortalContext.Add(feedback);

                var contactExists = _ClgPortalContext.SaveChanges();
                if (contactExists > 0)
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
                data.message = "Error";
            }
            return data;
        }

        public ClgStudentFeedbackFormDTO deactive(ClgStudentFeedbackFormDTO data)
        {
            try
            {
                var result = _ClgPortalContext.Adm_College_Student_GFeedbackDMO.Single(t => t.ACSGFE_Id == data.ACSGFE_Id);

                if (result.ACSGFE_ActiveFlag == true)
                {
                    result.ACSGFE_ActiveFlag = false;
                }
                else if (result.ACSGFE_ActiveFlag == false)
                {
                    result.ACSGFE_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ClgPortalContext.Update(result);
                int returnval = _ClgPortalContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
    }
}
