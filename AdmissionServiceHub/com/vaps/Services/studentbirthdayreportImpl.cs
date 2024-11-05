using System;
using System.Linq;
using PreadmissionDTOs.com.vaps.admission;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using System.Collections.Generic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class studentbirthdayreportImpl:Interfaces.studentbirthdayreportInterface
    {

        private static ConcurrentDictionary<string, studentbirthdayreportDTO> _login =
      new ConcurrentDictionary<string, studentbirthdayreportDTO>();

        public studentbirthdayreportContext _studentbirthdayreportContext;

        public studentbirthdayreportImpl(studentbirthdayreportContext studentbirthdayreportcontext)
        {
            _studentbirthdayreportContext = studentbirthdayreportcontext;
        }

        //public DemoClassDTO savedemodetails(DemoClassDTO demo)
        //{
        //    throw new NotImplementedException();
        //}
        public studentbirthdayreportDTO getdetails(studentbirthdayreportDTO data)
        {
            //studentbirthdayreportDTO org = new studentbirthdayreportDTO();
            try
            {
                DateTime dt = Convert.ToDateTime(data.months);
                DateTime dt1 = Convert.ToDateTime(data.day);

                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _studentbirthdayreportContext.year.Where(d=> d.Is_Active == true && d.MI_Id==data.MI_ID).OrderByDescending(d=> d.ASMAY_Order).ToList();
                data.accyear = allyear.ToArray();
                //DateTime dt = Convert.ToDateTime(data.months);

                //int dt=Convert.ToInt16(data.months);

                //data.studentDetails = (from b in _studentbirthdayreportContext.AdmClass
                //                        from c in _studentbirthdayreportContext.admsection
                //                        from a in _studentbirthdayreportContext.student
                //                        from d in _studentbirthdayreportContext.yearstudent

                //                      where (b.ASMCL_Id == d.ASMCL_Id && c.ASMC_Id == d.ASMS_Id && a.AMST_Id == d.AMST_Id)
                //                       //where a.AMST_DOB.Month = dt
                //                       //where a.AMST_DOB.Month = dt
                //                       select new studentbirthdayreportDTO
                // {
                //    //AMST_FirstName = a.AMST_FirstName,
                //    //                       AMST_AdmNo = a.AMST_AdmNo,
                //    //                       AMST_DOB = a.AMST_DOB,
                //    //                       class_name = b.ASMCL_ClassName,
                //    //                       section_name = c.ASMC_SectionName






                //                           //List < studentbirthdayreportDMO > lorg = new List<studentbirthdayreportDMO>();

                //                           ////List<StudentReferenceDMO> lorg1 = new List<StudentReferenceDMO>();

                //                           //lorg = _studentbirthdayreportContext.studentbirthdayreportDMO.ToList();
                //                           //org.studentDetails = lorg.ToArray();



                //                       }
                //                 ).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }





    }
}
