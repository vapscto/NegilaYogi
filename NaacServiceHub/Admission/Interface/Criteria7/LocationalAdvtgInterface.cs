using PreadmissionDTOs.NAAC.Admission.Criteria7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
  public  interface LocationalAdvtgInterface
    {
        Task<LocationalAdvtgDTO> loaddata(LocationalAdvtgDTO data);
        LocationalAdvtgDTO savedatatab1(LocationalAdvtgDTO data);
        LocationalAdvtgDTO getdata(LocationalAdvtgDTO data);
        LocationalAdvtgDTO edittab1(LocationalAdvtgDTO data);
        LocationalAdvtgDTO deactivYTab1(LocationalAdvtgDTO data);
        LocationalAdvtgDTO deleteuploadfile(LocationalAdvtgDTO data);
        LocationalAdvtgDTO viewuploadflies(LocationalAdvtgDTO data);
        LocationalAdvtgDTO getcomment(LocationalAdvtgDTO data);
        LocationalAdvtgDTO getfilecomment(LocationalAdvtgDTO data);
        LocationalAdvtgDTO savecomments(LocationalAdvtgDTO data);
        LocationalAdvtgDTO savefilewisecomments(LocationalAdvtgDTO data);
    }
}
