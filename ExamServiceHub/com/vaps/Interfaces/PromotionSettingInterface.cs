
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface PromotionSettingInterface
    {
        PromotionSettingDTO savedetails(PromotionSettingDTO data);
        PromotionSettingDTO get_category(PromotionSettingDTO data);
        PromotionSettingDTO get_subjects(PromotionSettingDTO data);
        PromotionSettingDTO deactivate(PromotionSettingDTO data);
        PromotionSettingDTO deactivate_sub(PromotionSettingDTO data);
        PromotionSettingDTO deactive_sub_grp_exm(PromotionSettingDTO data);
        PromotionSettingDTO deactive_sub_grp(PromotionSettingDTO data);
        PromotionSettingDTO getalldetailsviewrecords(int id);
        PromotionSettingDTO getalldetailsviewrecords_sub_grp_exms(int id);
        PromotionSettingDTO getalldetailsviewrecords_subgrps(int id);
        PromotionSettingDTO editdetails(int id);
        PromotionSettingDTO Getdetails(PromotionSettingDTO data);
        PromotionSettingDTO GetSubjectExamMaks(PromotionSettingDTO data);
        PromotionSettingDTO SetSubjectOrder(PromotionSettingDTO data);
        PromotionSettingDTO SaveSubjectOrder(PromotionSettingDTO data);
    }
}
