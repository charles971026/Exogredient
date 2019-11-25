﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TeamA.Exogredient.Services
{
    public class AdminFunctionalityService
    {
        private readonly string _sendingEmail = "exogredient.system@gmail.com";

        // HACK: replace with actual password
        private readonly string _sendingEmailPassword = "****";

        private readonly string _receivingEmail = "TEAMA.CS491@gmail.com";

        /// <summary>
        /// Uses gmail smtp to send mail from a known email address to any other address.
        /// Email subject is the current day in UTC.
        /// </summary>
        /// <param name="message">The message you want to send.</param>
        /// <returns>A bool representing whether the process succeeded.</returns>
        public async Task<bool> NotifySystemAdminAsync(string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                // TODO: learn invariant culture: https://stackoverflow.com/questions/10348952/how-i-can-convert-datetime-now-in-c-sharp-to-yyyy-mm-dd-hhmmss-sssssss
                string title = DateTime.UtcNow.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);

                mail.From = new MailAddress(_sendingEmail);
                mail.To.Add(_receivingEmail);
                mail.Subject = title;
                mail.Body = $"{message}";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_sendingEmail, _sendingEmailPassword);
                SmtpServer.EnableSsl = true;

                await SmtpServer.SendMailAsync(mail);

                SmtpServer.Dispose();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}