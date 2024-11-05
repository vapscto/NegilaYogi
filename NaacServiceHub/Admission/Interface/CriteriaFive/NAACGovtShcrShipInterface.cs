using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACGovtShcrShipInterface
    {
        NAACGovtShcrShipDTO loaddata(NAACGovtShcrShipDTO data);
        NAACGovtShcrShipDTO save(NAACGovtShcrShipDTO data);
        NAACGovtShcrShipDTO deactiveStudent(NAACGovtShcrShipDTO data);
        NAACGovtShcrShipDTO EditData(NAACGovtShcrShipDTO obj);
        NAACGovtShcrShipDTO viewuploadflies(NAACGovtShcrShipDTO obj);
        NAACGovtShcrShipDTO deleteuploadfile(NAACGovtShcrShipDTO obj);
        NAACGovtShcrShipDTO savemedicaldatawisecomments(NAACGovtShcrShipDTO obj);
        NAACGovtShcrShipDTO getcomment(NAACGovtShcrShipDTO obj);
        NAACGovtShcrShipDTO getfilecomment(NAACGovtShcrShipDTO obj);
        NAACGovtShcrShipDTO savefilewisecomments(NAACGovtShcrShipDTO obj);

    }
}
