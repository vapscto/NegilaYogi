using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class KMLogBookDelegate
    {
        CommonDelegate<KMLogBookDTO, KMLogBookDTO> _com = new CommonDelegate<KMLogBookDTO, KMLogBookDTO>();
        public KMLogBookDTO getdata(int id)
        {
            return _com.GetDataByIdTransport(id, "KMLogBookFacade/getdata/");
        }
        public KMLogBookDTO getreportdata(int id)
        {
            return _com.GetDataByIdTransport(id, "KMLogBookFacade/getreportdata/");
        }
        public KMLogBookDTO savedata(KMLogBookDTO data)
        {
            return _com.POSTDataTransport(data, "KMLogBookFacade/savedata/");
        }
        public KMLogBookDTO getkmreport(KMLogBookDTO data)
        {
            return _com.POSTDataTransport(data, "KMLogBookFacade/getkmreport/");
        }
        public KMLogBookDTO edit(KMLogBookDTO data)
        {
            return _com.POSTDataTransport(data, "KMLogBookFacade/edit/");
        }
        public KMLogBookDTO Onvahiclechange(KMLogBookDTO data)
        {
            return _com.POSTDataTransport(data, "KMLogBookFacade/Onvahiclechange/");
        }
        public KMLogBookDTO vehicletypechange(KMLogBookDTO data)
        {
            return _com.POSTDataTransport(data, "KMLogBookFacade/vehicletypechange/");
        }
        public KMLogBookDTO deleterecord(KMLogBookDTO data)
        {
            return _com.POSTDataTransport(data, "KMLogBookFacade/deleterecord/");
        }
        
    }
}
