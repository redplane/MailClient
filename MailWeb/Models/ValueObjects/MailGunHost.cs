using System;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models.ValueObjects
{
    public class MailGunHost : MailHost
    {
        #region Constructor

        public MailGunHost()
        {
            
        }
        
        #endregion
        
        #region Properties

        public string ApiKey { get; set; }
        
        public string Domain { get; set; }
        
        #endregion
    }
}