using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Chairman;

namespace PortalHub.com.vaps.Student.Services
{
    public class Stu_FeedbackImpl : Interfaces.Stu_FeedbackInterface
    {
        private static ConcurrentDictionary<string, Stu_FeedbackDTO> _login =
           new ConcurrentDictionary<string, Stu_FeedbackDTO>();
        private PortalContext _Examcontext;
        public Stu_FeedbackImpl(PortalContext Feecontext)
        {
            _Examcontext = Feecontext;
        }
        public Stu_FeedbackDTO getloaddata(Stu_FeedbackDTO data)
        {
            try
            {
                data.instname = _Examcontext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();


                data.studetiallist = (from a in _Examcontext.Adm_M_Student
                                      from b in _Examcontext.School_Adm_Y_StudentDMO
                                      where (b.AMST_Id == a.AMST_Id && a.AMST_Id == data.AMST_Id &&
                                      a.MI_Id == data.MI_Id && a.ASMAY_Id==data.ASMAY_Id)
                                      select new Stu_FeedbackDTO
                                      {
                                        AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                          AMST_MobileNo= a.AMST_MobileNo,
                                          AMST_emailId=a.AMST_emailId,
                                          AMST_Id=a.AMST_Id
                                      }
                             ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Stu_FeedbackDTO savecomment(Stu_FeedbackDTO data)
        {
            try
            {
                Adm_feedbackDMO fed = new Adm_feedbackDMO();
                fed.adm_name = data.adm_name;
                fed.adm_comment = data.adm_comment;
                fed.adm_emailid = data.adm_emailid;
                fed.adm_contactno = data.adm_contactno;
                fed.adm_date = DateTime.Now;
               
                _Examcontext.Add(fed);
                var contactExists = _Examcontext.SaveChanges();
                if (contactExists == 1)
                {

                    data.submsg = true;
                }
                else
                {
                    data.submsg = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Stu_FeedbackDTO getexamdetails(Stu_FeedbackDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
