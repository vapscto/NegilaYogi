using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface TaskCreationFromClintInterface
    {
        Task<TaskCreationFromClintDTO> getdetails(TaskCreationFromClintDTO data);
        TaskCreationFromClintDTO savedata(TaskCreationFromClintDTO data);
        TaskCreationFromClintDTO deactive(TaskCreationFromClintDTO data);

    }
}
