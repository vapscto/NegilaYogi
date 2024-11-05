using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestMarksBindDataDTO : CommonParamDTO
    {
        public Array studentDetails { get; set; }

        public Array WrittenTestSchedule { get; set; }

        public Array SubjectNames { get; set; }

        public Array OralTestBy { get; set; }

        public int OralTestByPerson { get; set; }

        public Array SelectedSubjectNames { get; set; }

        public Array SubjectWiseWrittenMarks { get; set; }

        public Array MasterConfiguration { get; set; }

        public Array OralTestSchedule { get; set; }

        public Array WirettenTestSubjectWiseStudentMarks { get; set; }

        public long Id { get; set; }
        public int OralTestScheduleAppFlag { get; set; }

        public string SelctedDataMood { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }

        public int PAOTM_OralBy { get; set; }

        public decimal Oral_MaxMarks { get; set; }

        //----------binding in single dto---------------//

        //--subject details----//

        public long PAMS_Id { get; set; }

        public string PAMS_SubjectName { get; set; }

        public decimal PAMS_MaxMarks { get; set; }

        //------------------//

        public long PAOTM_Id { get; set; }

        //------student details------//

        public long PASR_Id { get; set; }

        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }

        //-------------------------//

        //------marks details------//

        public decimal ObtMarks { get; set; }

        public List<StudentApplicationDTO> SelectedStudentDetails { get; set; }

        public string flagsubject { get; set; }

        public bool Chq_config { get; set; }

        //-------------------------//
    }
}
