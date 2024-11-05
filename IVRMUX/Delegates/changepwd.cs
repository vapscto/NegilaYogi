using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class changepwdDelegate 
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<regis, regis> COMMM = new CommonDelegate<regis, regis>();
        CommonDelegate<StateDTO, StateDTO> COMMMM = new CommonDelegate<StateDTO, StateDTO>();
        CommonDelegate<CityDTO, CityDTO> COMM = new CommonDelegate<CityDTO, CityDTO>();
        
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PreadmissionFacade/" + resource).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("{0}\t${1}\t{2}", product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return product;
        }

        public regis saveEnqdetails(regis en)
        {
            return COMMM.POSTData(en, "changepwdFacade/");
        }
    }
}















































