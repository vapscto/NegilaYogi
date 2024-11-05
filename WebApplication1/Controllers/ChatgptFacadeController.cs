using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.Extensions.Logging;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{

    [Route("api/ChatgptFacade")]
    public class ChatgptFacadeController : Controller
    {
        public Chatgptinterface _chatgpt;

        public ChatgptFacadeController(Chatgptinterface data)
        {
            _chatgpt = data;
        }

        [Route("chatagpt")]
        public async Task<ChatgptDTO> chatagpt([FromBody]ChatgptDTO data)
        {
            return await _chatgpt.ChatGPT(data);
        }
        [Route("Whatsapp")]
        public async Task<string> Whatsapp([FromBody]ChatgptDTO data)
        {
            return await _chatgpt.WhatsAppCall(data);
        }
       
      


    }
}