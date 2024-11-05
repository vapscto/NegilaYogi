using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace PortalHub.com.vaps.Student.Services
{
    public class ClassWorkDownloadImpl : Interfaces.ClassWorkDownloadInterface
    {
        private static ConcurrentDictionary<string, IVRM_ClassWorkDTO> _login =
           new ConcurrentDictionary<string, IVRM_ClassWorkDTO>();
        private PortalContext _workcontext;
        public ClassWorkDownloadImpl(PortalContext workcontext)
        {
            _workcontext = workcontext;
        }

        public IVRM_ClassWorkDTO getloaddata(IVRM_ClassWorkDTO data)
        {
            try
            {
                var classSectionData = (from d in _workcontext.AcademicYearDMO
                                        from a in _workcontext.School_M_Class
                                        from b in _workcontext.School_M_Section
                                        from c in _workcontext.School_Adm_Y_StudentDMO
                                        where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                        a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                        select new ExamDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }
                            ).ToList();
                data.classSectionlist = classSectionData.ToArray();
                var asmayid = 1;
                data.subjectlist = (from a in _workcontext.Exm_Category_ClassDMO
                                    from b in _workcontext.Exm_Yearly_CategoryDMO
                                    from c in _workcontext.Exm_Yearly_Category_GroupDMO
                                    from d in _workcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    from e in _workcontext.IVRM_Master_SubjectsDMO
                                    from f in _workcontext.School_Adm_Y_StudentDMO

                                    where (b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.EYCG_Id == c.EYCG_Id && d.EYCGS_ActiveFlg == true && e.MI_Id == a.MI_Id && e.ISMS_ActiveFlag == 1 && e.ISMS_ExamFlag == 1 && e.ISMS_Id == d.ISMS_Id
                                    && a.MI_Id == data.MI_Id && f.ASMCL_Id == a.ASMCL_Id && a.ASMAY_Id == asmayid /*data.ASMAY_Id*/ && a.ASMCL_Id == classSectionData.FirstOrDefault().ASMCL_Id && a.ASMS_Id == classSectionData.FirstOrDefault().ASMS_Id && a.ECAC_ActiveFlag == true && f.AMST_Id == data.AMST_Id)
                                    select new IVRM_ClassWorkDTO
                                    {
                                        ISMS_Id = e.ISMS_Id,
                                        ISMS_SubjectName = e.ISMS_SubjectName,
                                        ISMS_SubjectCode = e.ISMS_SubjectCode
                                    }
                  ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_ClassWorkDTO getwork(IVRM_ClassWorkDTO data)
        {
            try
            {
                data.worklist = (from d in _workcontext.AcademicYearDMO
                                 from a in _workcontext.School_M_Class
                                 from b in _workcontext.School_M_Section
                                 from c in _workcontext.School_Adm_Y_StudentDMO
                                 from e in _workcontext.IVRM_ClassWorkDMO
                                 from f in _workcontext.IVRM_Master_SubjectsDMO
                                 where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && e.ISMS_Id == f.ISMS_Id
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id && d.MI_Id == data.MI_Id
                                        && c.ASMAY_Id == data.ASMAY_Id && f.ISMS_Id == data.ISMS_Id)
                                 select new IVRM_ClassWorkDTO
                                 {
                                     ICW_Id = e.ICW_Id,
                                     ICW_Assignment = e.ICW_Assignment,
                                     ICW_Attachment = e.ICW_Attachment,
                                     ICW_Topic = e.ICW_Topic,
                                     ICW_SubTopic = e.ICW_SubTopic,
                                     ICW_FromDate = e.ICW_FromDate,
                                     ICW_ToDate = e.ICW_ToDate,
                                 }
                 ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
