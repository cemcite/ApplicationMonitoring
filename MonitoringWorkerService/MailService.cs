using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NuevoSoftware.ApplicationMonitoring.Data;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using NuevoSoftware.ApplicationMonitoring.ClientBusiness;

namespace NuevoSoftware.ApplicationMonitoring.MonitoringWorkerService
{
    public class MailService : HostedService
    {
        HttpClient restClient;
        public MailService()
        {
            restClient = new HttpClient();
        }
        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                using (NSAppMonDBContext context = new NSAppMonDBContext())
                {
                    List<NsapplicationsT> applicationList = context.NsapplicationsT.Include(entity => entity.User).Where(entity => entity.IsActive == true).ToList();
                    foreach (NsapplicationsT item in applicationList)
                    {
                        bool result = StatusHelper.GetStatus(item.Url);
                        if (result) continue;
                        else
                        {
                            MailAddress fromAddress = new MailAddress("nsapplicationmonitoring@gmail.com");
                            MailAddress toAddress = new MailAddress(item.User.Mail);
                            const string subject = "Application Monitoring | Nuevo Software";
                            using (var smtp = new SmtpClient
                            {
                                Host = "smtp.gmail.com",
                                Port = 587,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                Credentials = new NetworkCredential(fromAddress.Address, "ApplicationMonitoring")
                            })
                            using (var message = new MailMessage(fromAddress, toAddress)
                            {
                                Subject = subject,
                                Body = string.Format("{0} is down.\nURL: {1}", item.Name, item.Url)
                            })
                            {
                                smtp.Send(message);
                            }
                            continue;
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(60), cToken); //ToDo
                }
            }
        }
    }
}
