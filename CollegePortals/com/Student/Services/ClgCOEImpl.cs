using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;

namespace CollegePortals.com.Student.Services
{
    public class ClgCOEImpl : Interfaces.ClgCOEInterface
    {
        private static ConcurrentDictionary<string, ClgStudentDashboardDTO> _login =
           new ConcurrentDictionary<string, ClgStudentDashboardDTO>();
        private CollegeportalContext _ClgPortalContext;
        public ClgCOEImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }
        public ClgStudentDashboardDTO getloaddata(ClgStudentDashboardDTO data)
        {
            try
            {
                data.yearlist = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                                 from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                 from c in _ClgPortalContext.academicYearDMO
                                 where (a.AMCST_Id == b.AMCST_Id && b.ASMAY_Id == c.ASMAY_Id && b.AMCST_Id == data.AMCST_Id
                                 && a.MI_Id == data.MI_Id && b.ACYST_ActiveFlag == 1 && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true)

                                 select new ClgPortalFeeDTO
                                 {
                                     ASMAY_Id = c.ASMAY_Id,
                                     ASMAY_Year = c.ASMAY_Year,
                                     ASMAY_Order = c.ASMAY_Order
                                 }).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();

                data.calenderlist = _ClgPortalContext.IVRM_Month_DMO.Select(a=>a).Distinct().ToArray();

                data.currentyear = _ClgPortalContext.academicYearDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgStudentDashboardDTO getcoedata(ClgStudentDashboardDTO data)
        {
            try
            {

                #region flag Details
                if (data.flag != "" && data.flag != null)
                {
                    var roletyp = _ClgPortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                }

                if (data.roletype.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.coereportlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                          from n in _ClgPortalContext.COE_EventsDMO
                                          from y in _ClgPortalContext.COE_Events_CourseBranchDMO
                                          from o in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                          from i in _ClgPortalContext.COE_Events_Images.Where(img => n.COEE_Id == img.COEE_Id).DefaultIfEmpty()
                                          where m.COEME_Id == n.COEME_Id && n.MI_Id==m.MI_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id &&
                                          o.AMCST_Id == data.AMCST_Id && n.COEE_EStartDate.Value.Month == data.month && n.COEE_Id == y.COEE_Id &&
                                          o.AMCO_Id == y.AMCO_Id && o.AMB_Id == y.AMB_Id && n.COEE_EStartDate.Value.Year == DateTime.Now.Year
                                          select new ClgStudentDashboardDTO
                                          {
                                              COEME_Id = m.COEME_Id,
                                              COEME_EventName = m.COEME_EventName,
                                              COEME_EventDesc = m.COEME_EventDesc,
                                              COEE_EStartDate = n.COEE_EStartDate,
                                              COEE_EEndDate = n.COEE_EEndDate,
                                              COEEI_Images = i.COEEI_Images,
                                              ASMAY_Id = o.ASMAY_Id,
                                          }).Distinct().OrderBy(c => c.COEME_Id).ToArray();
                }
                else if (data.roletype.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.coereportlist = (from m in _ClgPortalContext.COE_Master_EventsDMO
                                          from n in _ClgPortalContext.COE_EventsDMO
                                          from y in _ClgPortalContext.COE_Events_EmployeesDMO
                                          from o in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                          where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && o.ASMAY_Id == data.ASMAY_Id && o.AMCST_Id == data.AMCST_Id && n.COEE_EStartDate.Value.Month == data.month && n.COEE_Id == y.COEE_Id && n.COEE_EStartDate.Value.Year == DateTime.Now.Year
                                          select new ClgStudentDashboardDTO
                                          {
                                              COEME_Id = m.COEME_Id,
                                              COEME_EventName = m.COEME_EventName,
                                              COEME_EventDesc = m.COEME_EventDesc,
                                              COEE_EStartDate = n.COEE_EStartDate,
                                              COEE_EEndDate = n.COEE_EEndDate,
                                              ASMAY_Id = o.ASMAY_Id,
                                          }).Distinct().OrderBy(c => c.COEME_Id).ToArray();
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}