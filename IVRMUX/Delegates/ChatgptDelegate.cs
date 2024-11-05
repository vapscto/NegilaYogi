using CommonLibrary;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates
{
    public class ChatgptDelegate
    {

  
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<regis, regis> COMMM = new CommonDelegate<regis, regis>();
        CommonDelegate<StateDTO, StateDTO> COMMMM = new CommonDelegate<StateDTO, StateDTO>();
        CommonDelegate<ChatgptDTO, string> COMM = new CommonDelegate<ChatgptDTO, string>();



        public ChatgptDTO ChatGPT(ChatgptDTO data)
        {
            return COMM.POSTData(data, "ChatgptFacade/chatagpt");
        }

        public ChatgptDTO Whatsapp(ChatgptDTO data)
        {
            return COMM.POSTData(data, "ChatgptFacade/Whatsapp");
        }

       
    }
}
