using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgNoticeBoardInterface
    {
        ClgNoticeBoardDTO getloaddata(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO getbranchdata(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO getsemdata(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO savedata(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO getmultiuploadedfile(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO getNoticedata(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO editdetails(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO deactive(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO deactivedetails(ClgNoticeBoardDTO data);

        ClgNoticeBoardDTO Getdata_class(ClgNoticeBoardDTO dto);

        ClgNoticeBoardDTO getreportnotice(ClgNoticeBoardDTO data);

        ClgNoticeBoardDTO Getdataview(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO getstudent(ClgNoticeBoardDTO data);
        //course
        ClgNoticeBoardDTO getcoursedata(ClgNoticeBoardDTO data);

        //Akash
        
        ClgNoticeBoardDTO Deptselectiondetails(ClgNoticeBoardDTO data);
        ClgNoticeBoardDTO Desgselectiondetails(ClgNoticeBoardDTO data);

    }
}
