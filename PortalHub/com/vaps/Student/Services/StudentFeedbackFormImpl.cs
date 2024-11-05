using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Chairman;
using DomainModel.Model.com.vapstech.Portals.Student;

namespace PortalHub.com.vaps.Student.Services
{
    public class StudentFeedbackFormImpl : Interfaces.StudentFeedbackFormInterface
    {
        private static ConcurrentDictionary<string, StudentFeedbackFormDTO> _login =
           new ConcurrentDictionary<string, StudentFeedbackFormDTO>();
        private PortalContext _PortalContext;
        public StudentFeedbackFormImpl(PortalContext portalcontext)
        {
            _PortalContext = portalcontext;
        }
        public StudentFeedbackFormDTO getloaddata(StudentFeedbackFormDTO data)
        {
            try
            {
                data.instname = _PortalContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.get_feedback = _PortalContext.Adm_School_Student_GFeedbackDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id==data.AMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentFeedbackFormDTO savefeedback(StudentFeedbackFormDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var clsecids = (from d in _PortalContext.AcademicYearDMO
                                from a in _PortalContext.School_M_Class
                                from b in _PortalContext.School_M_Section
                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                select new ExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id,
                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                    ASMS_Id = c.ASMS_Id,
                                }
                                  ).Distinct().ToList();

                Adm_School_Student_GFeedbackDMO feedback = new Adm_School_Student_GFeedbackDMO();
                feedback.MI_Id = data.MI_Id;
                feedback.AMST_Id = data.AMST_Id;
                feedback.ASMAY_Id = data.ASMAY_Id;
                feedback.ASMCL_Id = clsecids.FirstOrDefault().ASMCL_Id;
                feedback.ASMS_Id = clsecids.FirstOrDefault().ASMS_Id;
                feedback.ASGFE_FeedBack = data.ASGFE_FeedBack;
                feedback.ASGFE_FeedbackDate = indianTime;
                feedback.ASGFE_ActiveFlag = true;
                feedback.ASGFE_CreatedBy = data.AMST_Id;
                feedback.ASGFE_UpdatedBy = data.AMST_Id;

                feedback.CreatedDate = DateTime.Now;
                feedback.UpdatedDate = DateTime.Now;
                _PortalContext.Add(feedback);

                var contactExists = _PortalContext.SaveChanges();
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

        public StudentFeedbackFormDTO deactive(StudentFeedbackFormDTO data)
        {
            try
            {
                var result = _PortalContext.Adm_School_Student_GFeedbackDMO.Single(t => t.ASGFE_Id == data.ASGFE_Id);

                if (result.ASGFE_ActiveFlag == true)
                {
                    result.ASGFE_ActiveFlag = false;
                }
                else if (result.ASGFE_ActiveFlag == false)
                {
                    result.ASGFE_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _PortalContext.Update(result);
                int returnval = _PortalContext.SaveChanges();
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
