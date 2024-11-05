using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
//using OpenAI_API.Chat;

using IVRMUX.Delegates;

namespace corewebapi18072016.Controllers
{

    [Route("api/[controller]")]
    public class ChatgptController : Controller
    {
        ChatgptDelegate cg = new ChatgptDelegate();
        [HttpPost]
        [Route("chatgpt")]
        public ChatgptDTO chatgpt([FromBody] ChatgptDTO data)
        {
            return cg.ChatGPT(data);
        }


        [Route("whatsapp")]
        public ChatgptDTO whatsapp([FromBody] ChatgptDTO data)
        {
            return cg.Whatsapp(data);
        }


    }
}