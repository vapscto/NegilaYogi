using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates
{
    public class TaskCreationFromClintDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TaskCreationFromClintDTO, TaskCreationFromClintDTO> COMMM = new CommonDelegate<TaskCreationFromClintDTO, TaskCreationFromClintDTO>();

        public TaskCreationFromClintDTO getdetails(TaskCreationFromClintDTO data)
        {
            return COMMM.POSTData(data, "TaskCreationFromClintFacade/getdetails/");
        }

        public TaskCreationFromClintDTO savedata(TaskCreationFromClintDTO data)
        {
            return COMMM.POSTData(data, "TaskCreationFromClintFacade/savedata");
        }
        public TaskCreationFromClintDTO deactive(TaskCreationFromClintDTO data)
        {
            return COMMM.POSTData(data, "TaskCreationFromClintFacade/deactive");
        }


    }
}
