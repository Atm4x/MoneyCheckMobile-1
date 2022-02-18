using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace MoneyCheck.Helpers
{
    public class MailHelper
    {
        public static void SendMail(String sendTo)
        {
            var fromAddress = new MailAddress("money.check.sup@gmail.com", "Money Check Organization");
            var toAddress = new MailAddress(sendTo, "New User");
            const string fromPassword = "mc472917";
            const string subject = "Осуществлён вход в MoneyCheck";
            string body = $"<h3>{TimeHelper.GetTimePhrase()}, {sendTo}!</h3><br/> На ваш аккаунт был осуществлён вход {DateTime.Now.ToString("f") + ", " + DateTime.Now.ToString("ddd") }.<br/>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.SendCompleted += (s, ee) =>
                {
                    smtp.Dispose();
                    message.Dispose();
                };
                smtp.SendAsync(message, null);
            }
        } 
    }
}
