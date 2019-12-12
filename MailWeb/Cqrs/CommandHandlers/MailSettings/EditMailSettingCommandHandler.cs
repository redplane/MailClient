using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailWeb.Cqrs.Commands.MailSettings;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MailWeb.Models.MailHosts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.CommandHandlers.MailSettings
{
    public class EditMailSettingCommandHandler : IRequestHandler<EditMailSettingCommand, IBasicMailSetting>
    {
        #region Properties

        private readonly MailManagementDbContext _dbContext;

        #endregion

        #region Constructor

        public EditMailSettingCommandHandler(MailManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual async Task<IBasicMailSetting> Handle(EditMailSettingCommand command,
            CancellationToken cancellationToken)
        {
            // Find the mail settings.
            var mailSetting = await _dbContext.MailSettings
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (mailSetting == null)
                throw new Exception("Mail setting is not found.");

            if (command.Timeout != null && command.Timeout.HasModified)
                mailSetting.Timeout = command.Timeout.Value;

            if (command.DisplayName != null && command.DisplayName.HasModified)
                mailSetting.DisplayName = command.DisplayName.Value;

            if (command.MailHost != null)
            {
                if (command.MailHost is EditSmtpHostModel editSmtpMailHost)
                    UpdateSmtpIntoMailSetting(mailSetting, editSmtpMailHost);

                if (command.MailHost is EditMailGunHostModel editMailGunHost)
                    UpdateMailGunIntoMailSetting(mailSetting, editMailGunHost);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return mailSetting;
        }

        protected virtual void UpdateSmtpIntoMailSetting(MailSetting mailSetting, EditSmtpHostModel editSmtpHost)
        {
            if (!(mailSetting.MailHost is SmtpHost smtpHost))
                return;

            if (editSmtpHost.HostName != null && editSmtpHost.HostName.HasModified)
                smtpHost.HostName = editSmtpHost.HostName.Value;

            if (editSmtpHost.Port != null && editSmtpHost.Port.HasModified)
                smtpHost.Port = editSmtpHost.Port.Value;

            if (editSmtpHost.Username != null && editSmtpHost.Username.HasModified)
                smtpHost.Username = editSmtpHost.Username.Value;

            if (editSmtpHost.Password != null && editSmtpHost.Password.HasModified)
                smtpHost.Password = editSmtpHost.Password.Value;

            if (editSmtpHost.Ssl != null && editSmtpHost.Ssl.HasModified)
                smtpHost.Ssl = editSmtpHost.Ssl.Value;

            mailSetting.MailHost = smtpHost;
        }

        protected virtual void UpdateMailGunIntoMailSetting(MailSetting mailSetting,
            EditMailGunHostModel editMailGun)
        {
            var mailGunHost = (MailGunHost) mailSetting.MailHost;

            if (editMailGun.Domain != null && editMailGun.Domain.HasModified)
                mailGunHost.Domain = editMailGun.Domain.Value;

            if (editMailGun.ApiKey != null && editMailGun.ApiKey.HasModified)
                mailGunHost.ApiKey = editMailGun.ApiKey.Value;

            mailSetting.MailHost = mailGunHost;
        }

        #endregion
    }
}