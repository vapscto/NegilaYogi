using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface QuestionPaperTypeInterface
    {
        QuestionPaperTypeDTO getalldetails(QuestionPaperTypeDTO dto);
        QuestionPaperTypeDTO savedetails(QuestionPaperTypeDTO dto);
        QuestionPaperTypeDTO editData(int id);
        QuestionPaperTypeDTO deactivate(QuestionPaperTypeDTO dto);

    }
}
