﻿using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MailServices.Models.Interfaces;

namespace MailServices.Services.Interfaces
{
    public interface IMailService
    {
        #region Properties

        string UniqueName { get; }

        string DisplayName { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Send mail asynchronously.
        /// </summary>
        /// <returns></returns>
        Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string subject, string content, bool isHtmlContent = false,
            ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Send mail asynchronously.
        /// </summary>
        /// <returns></returns>
        Task SendMailAsync(IMailAddress sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies,
            string templateName,
            ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Send mail asynchronously.
        /// </summary>
        /// <returns></returns>
        Task SendMailAsync(string sender, IMailAddress[] recipients, IMailAddress[] carbonCopies,
            IMailAddress[] blindCarbonCopies, string templateName,
            ExpandoObject additionalSubjectData = null,
            ExpandoObject additionalContentData = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get sender from unique key asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IMailAddress> GetSenderAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get mail content by template name asynchronously.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IMailContent> GetMailContentAsync(string name, CancellationToken cancellationToken = default);

        #endregion
    }
}