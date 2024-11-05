using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text;
using System.IO;

namespace WebApplication1.Services
{
    public class RegistrationReportImpl : Interfaces.RegistrationReportInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
         new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly RegistrationReportContext _RegistrationReportContext;
        private readonly UserManager<ApplicationUser> _UserManager;

        public RegistrationReportImpl(RegistrationReportContext RegistrationReportContext, UserManager<ApplicationUser> UserManager)
        {
            _RegistrationReportContext = RegistrationReportContext;
            _UserManager = UserManager;
        }     

    }
}
