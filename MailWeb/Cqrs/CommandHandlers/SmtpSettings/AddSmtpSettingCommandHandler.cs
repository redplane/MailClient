﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.SmtpSettings;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;
using MailWeb.ViewModels.MailSettings;
using MediatR;

namespace MailWeb.Cqrs.CommandHandlers.SmtpSettings
{
    public class AddSmtpSettingCommandHandler : IRequestHandler<AddSmtpSettingCommand, MailSettingViewModel>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        #endregion

        #region Constructor

        public AddSmtpSettingCommandHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual async Task<MailSettingViewModel> Handle(AddSmtpSettingCommand command,
            CancellationToken cancellationToken)
        {
            var basicMailSetting = new BasicMailSetting(Guid.NewGuid(), command.UniqueName);
            basicMailSetting.DisplayName = command.DisplayName;
            basicMailSetting.Timeout = command.Timeout;

            var smtpHost = new SmtpHost();
            smtpHost.Ssl = command.Ssl;
            smtpHost.HostName = command.HostName;
            smtpHost.Port = command.Port;
            smtpHost.Username = command.Username;
            smtpHost.Password = command.Password;

            basicMailSetting.MailHost = smtpHost;

            _dbContext.BasicMailSettings
                .Add(basicMailSetting);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new MailSettingViewModel(basicMailSetting);
        }

        #endregion
    }
}