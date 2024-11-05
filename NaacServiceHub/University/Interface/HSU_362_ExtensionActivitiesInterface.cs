using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface HSU_362_ExtensionActivitiesInterface
    {
        HSU_362_ExtensionActivitiesDTO loaddata(HSU_362_ExtensionActivitiesDTO data);
        HSU_362_ExtensionActivitiesDTO save(HSU_362_ExtensionActivitiesDTO data);
        HSU_362_ExtensionActivitiesDTO deactive(HSU_362_ExtensionActivitiesDTO data);
        HSU_362_ExtensionActivitiesDTO EditData(HSU_362_ExtensionActivitiesDTO data);
        HSU_362_ExtensionActivitiesDTO viewuploadflies(HSU_362_ExtensionActivitiesDTO data);
        HSU_362_ExtensionActivitiesDTO deleteuploadfile(HSU_362_ExtensionActivitiesDTO data);
    }
}
