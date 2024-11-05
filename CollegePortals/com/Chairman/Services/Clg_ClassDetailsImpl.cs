using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Chairman.Services
{
    public class Clg_ClassDetailsImpl:Interfaces.Clg_ClassDetailsInterface
    {

        public CollegeportalContext _context;
        public Clg_ClassDetailsImpl(CollegeportalContext w)
        {
            _context = w;
        }
        public Clg_ClassDetails_DTO loaddata(Clg_ClassDetails_DTO data)
        {
            try
            {



                data.allacademicyear = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Clg_ClassDetails_DTO getcourse(Clg_ClassDetails_DTO data)
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


        public Clg_ClassDetails_DTO report(Clg_ClassDetails_DTO data)
        {
            try
            {

                data.reportlist = (from a in _context.AcademicYear
                                   from b in _context.MasterCourseDMO
                                   from c in _context.Adm_Master_College_StudentDMO
                                   where (a.ASMAY_Id == data.ASMAY_Id && c.AMCO_Id == data.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == c.AMCO_Id)
                                   select new Clg_ClassDetails_DTO
                                   {

                                       AMCST_FirstName = ((c.AMCST_FirstName == null || c.AMCST_FirstName == "" ? "" : c.AMCST_FirstName)
+ (c.AMCST_MiddleName == null || c.AMCST_MiddleName == "" ? "" :
" " + c.AMCST_MiddleName)
+ (c.AMCST_LastName == null || c.AMCST_MiddleName == "" ? "" :
" " + c.AMCST_MiddleName)
+ (c.AMCST_AdmNo == null || c.AMCST_AdmNo == "" ? "" :
" : " + c.AMCST_AdmNo)).Trim(),
                                   }).Distinct().ToArray();
                 data.categorylist = (from a in _context.ClgMasterCategoryDMO
                                      from b in _context.ClgMasterCourseCategoryMapDMO
                                      from c in _context.Adm_Master_College_StudentDMO
                                      from d in _context.MasterCourseDMO
                                      where (a.AMCOC_Id == b.AMCOC_Id && b.AMCOC_Id == c.AMCOC_Id && c.AMCO_Id == d.AMCO_Id && b.AMCO_Id == c.AMCO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id && c.ASMAY_Id == data.ASMAY_Id)
                                      select new Clg_ClassDetails_DTO
                                      {
                                          AMCST_FirstName = ((c.AMCST_FirstName == null || c.AMCST_FirstName == "" ? "" : c.AMCST_FirstName)
+ (c.AMCST_MiddleName == null || c.AMCST_MiddleName == "" ? "" :
" " + c.AMCST_MiddleName)
+ (c.AMCST_LastName == null || c.AMCST_MiddleName == "" ? "" :
" " + c.AMCST_MiddleName)
+ (c.AMCST_AdmNo == null || c.AMCST_AdmNo == "" ? "" :
" : " + c.AMCST_AdmNo)).Trim(),

                                         
                                      }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
