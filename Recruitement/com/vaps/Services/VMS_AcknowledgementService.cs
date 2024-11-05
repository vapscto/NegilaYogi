using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class VMS_AcknowledgementService:Interfaces.VMS_AcknowledgementInterface
    {

        public VMSContext _VMSContext;
        public VMS_AcknowledgementService(VMSContext d)
        {
            _VMSContext = d;
        }


        public HR_VMS_AcknowledgementDTO loaddata(HR_VMS_AcknowledgementDTO data)
        {
            try
            {

                data.yearlist = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
                //data.insti = (from a in _VMSContext.HR_Master_Department
                //              where (a.HRMD_Id == 4)
                //              select new HR_VMS_AcknowledgementDTO
                //              {
                //                  HRMD_DepartmentName = a.HRMD_DepartmentName,

                //              }).Distinct().ToArray();

                //data.year = (from a in _VMSContext.HR_Master_Course
                //             where (a.HRMC_Id == 1)
                //             select new HR_VMS_AcknowledgementDTO
                //             {
                //                 HRMC_QulaificationName = a.HRMC_QulaificationName,

                //             }).Distinct().ToArray();


                data.mix = (from a in _VMSContext.HR_Master_Employee_DMO
                from b in _VMSContext.Staff_User_Login
                from c in _VMSContext.ApplicationUserDMO
                where (a.MI_Id == 5 && b.MI_Id == 5 && a.MI_Id == b.MI_Id && a.HRME_Id == b.Emp_Code && b.Id == c.Id && c.Id == 154 && b.Emp_Code == a.HRME_Id)
                select new HR_VMS_AcknowledgementDTO
                {

                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,

                }).Distinct().ToArray();
                // data.aaaa = _VMSContext.HR_Master_Employee_DMO.Where(t => t.MI_Id == 5 && t.HRME_Id == 15).Distinct().ToArray();
                // //data.aaaa = _VMSContext.HR_Master_Employee_DMO.Where(t=>t.MI_Id>1).Distinct().ToArray();

                // data.institution = _VMSContext.Institution.Where(t => t.MI_Id == 3).Distinct().ToArray();


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
