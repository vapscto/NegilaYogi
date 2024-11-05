using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface MasterSmsEmailParameterInterface
    {
        MasterSmsEmailParameterDTO Savedata(MasterSmsEmailParameterDTO mas);

        MasterSmsEmailParameterDTO deletedata(int ID);

        MasterSmsEmailParameterDTO edit(MasterSmsEmailParameterDTO ID);

        MasterSmsEmailParameterDTO GetcastecategoryData(MasterSmsEmailParameterDTO data);

        //HTML TEMPLATE
        MasterSmsEmailParameterDTO htmlSavedata(MasterSmsEmailParameterDTO mas);

        MasterSmsEmailParameterDTO htmldeletedata(MasterSmsEmailParameterDTO ID);

        MasterSmsEmailParameterDTO htmledit(MasterSmsEmailParameterDTO ID);

        MasterSmsEmailParameterDTO htmlGetcastecategoryData(MasterSmsEmailParameterDTO data);
    }
}
