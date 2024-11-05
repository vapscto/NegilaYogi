using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface CLGStudentRouteMappingInterface
    {
        CLGStudentRouteMappingDTO getdata(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO savedata(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO geteditdata(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO getstudents(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO checkduplicateno(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO viewrecordspopup(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO getreportedit(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO getreporteditbuspass(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO savedatabuspass(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO deactivate(CLGStudentRouteMappingDTO data);
        CLGStudentRouteMappingDTO SearchByColumn(CLGStudentRouteMappingDTO data);

    }
}
