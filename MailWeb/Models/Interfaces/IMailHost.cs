using System;

namespace MailWeb.Models.Interfaces
{
    public interface IMailHost
    {
        #region Properties
        
        string Type { get; }
        
        #endregion
    }
}