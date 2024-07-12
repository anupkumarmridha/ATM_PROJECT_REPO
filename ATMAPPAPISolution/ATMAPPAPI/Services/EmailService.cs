
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using ATMAPPAPI.Repositoris;

namespace ATMAPPAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly ConcurrentDictionary<string, (string Otp, DateTime Timestamp)> otpStore
        = new ConcurrentDictionary<string, (string Otp, DateTime Timestamp)>();

        private readonly IMemoryCache _cache;
        private readonly ICardOperations _cardOperations;

        public EmailService(IMemoryCache cache, ICardOperations cardOperations)
        {
            _cardOperations = cardOperations;
            _cache = cache;
        }


        public async Task<string> SendOTPMail(string accountNo)
        {
            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                var pin = GenerateOTP();
                otpStore[cardInfo.Email] = (pin, DateTime.UtcNow);
                _cache.Set("OtpStore", otpStore);


                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("i62175973@gmail.com", "emou qcfn elfe nldy"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("i62175973@gmail.com"),
                    Subject = "Your OTP Code",
                    Body = $"Your OTP code is: {pin}",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(cardInfo.Email);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    return "Email sent successfully.";
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error Sending OTP!");
                }
            }
            throw new InvalidOperationException("Account not found");
         
        }



        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public async Task<string> VerifyOtp(string accountNo, string enteredOtp)
        {
            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                var otpStore = _cache.Get<ConcurrentDictionary<string, (string, DateTime)>>("OtpStore");
                if(otpStore != null)
                {
                    if (otpStore.TryGetValue(cardInfo.Email, out var otpData))
                    {
                        if ((DateTime.UtcNow - otpData.Item2).TotalHours > 2)
                        {
                            return "OTP has expired.";
                        }

                        if (otpData.Item1 == enteredOtp)
                        {
                            return "OTP verified successfully.";
                        }
                        else
                        {
                            return "Invalid OTP.";
                        }
                    }
                    else
                    {
                        return "OTP not found for the given email.";
                    }
                }
                else
                {
                    throw new InvalidOperationException("OTP not found for the given email.");
                }
            }
            throw new InvalidOperationException("Account not found");
           
        }


        public async Task<string> SendTransactionMail(string toEmailAddress, string accountNo, string transactionType, decimal amount)
        {
  
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("i62175973@gmail.com", "emou qcfn elfe nldy"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("i62175973@gmail.com"),
                Subject = "Transaction",
                Body = $"Your A/c No. {accountNo} has been {transactionType}ed with Rs.{amount}.00 done using ATM transaction. - India Bank",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmailAddress);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return "Email sent successfully.";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Something went wrong!");
            }
        }
    }
}
