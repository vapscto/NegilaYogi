using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface MasterModulesInterface 
    {
        MasterModulesDTO MasterModulesData(MasterModulesDTO mas);
        //  bool GetMasterModulesData(Int32 IVRMM_Id);

        MasterModulesDTO MasterDeleteModulesData(int ID);

        MasterModulesDTO GetSelectedRowDetails(int ID);

        MasterModulesDTO GetMasterModulesData(MasterModulesDTO MasterModulesDTO);
    }
}
