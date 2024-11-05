using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface KMLogBookInterface
    {
        KMLogBookDTO getdata(int id);
        KMLogBookDTO getreportdata(int id);
        KMLogBookDTO savedata(KMLogBookDTO data);
        KMLogBookDTO getkmreport(KMLogBookDTO data);
        KMLogBookDTO edit(KMLogBookDTO data);
        KMLogBookDTO Onvahiclechange(KMLogBookDTO data);
        KMLogBookDTO vehicletypechange(KMLogBookDTO data);
        KMLogBookDTO deleterecord(KMLogBookDTO data);
    }
}
