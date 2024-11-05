using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface OtherCollegeStudentEntryInterface
    {
        Fee_Master_College_OtherStudentsDTO getdetails(int id);
        Fee_Master_College_OtherStudentsDTO save(Fee_Master_College_OtherStudentsDTO data);
        Fee_Master_College_OtherStudentsDTO edit(int id);
        Fee_Master_College_OtherStudentsDTO delete(int id);
    }
}
