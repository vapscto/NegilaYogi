using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface Chatgptinterface
    {
        Task<ChatgptDTO> ChatGPT(ChatgptDTO data);
        Task<string> WhatsAppCall(ChatgptDTO data);
       

    }
}

