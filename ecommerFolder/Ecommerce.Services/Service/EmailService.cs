using Ecommerce.Services.IService;
using Ecommerce.Webmodels;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using Ecommerce.Repositories.IRepositories;

namespace Ecommerce.Services.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepository;
        public EmailService(IConfiguration iconfiguration, IProductRepository productRepository) {
            _configuration = iconfiguration;
            _productRepository = productRepository;
        }
        public void SendEmail(CreateEmailRequest request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("duynguyen120504@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Order Confirmation";
            // Build HTML content with inline CSS
            string htmlContent = @"
    <html>
        <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <h2 style='color: #4CAF50;'>Thank you for your order!</h2>
            <p style='font-size: 14px;'>We are delighted to serve you. Your order has been successfully placed, and here are the details:</p>
            <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                <thead>
                    <tr>
                        <th style='text-align: left; padding: 8px; border: 1px solid #ddd; background-color: #f2f2f2;'>Product Name</th>
                        <th style='text-align: left; padding: 8px; border: 1px solid #ddd; background-color: #f2f2f2;'>Quantity</th>
                    </tr>
                </thead>
                <tbody>";

            // Dynamically add rows for each product
            int total = 0;
            foreach (var orderedproduct in request.OrderProducts)
            {
                var product = _productRepository.GetByID(orderedproduct.ProductId);
                htmlContent += $@"
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{product.Name}</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{orderedproduct.Quantity}</td>
                    </tr>";
                total += product.Price * orderedproduct.Quantity;
            }

            htmlContent += $@"
                <tr>
                    <td style='padding: 8px; border: 1px solid #ddd; text-align: right; font-weight: bold;'>Total:</td>
                    <td style='padding: 8px; border: 1px solid #ddd; font-weight: bold;'>${total}</td>
                </tr>";


            htmlContent += @"
                </tbody>
            </table>         
            <p style='margin-top: 20px; font-size: 14px;'>We hope you enjoy your purchase! If you have any questions, feel free to contact us.</p>
            <p style='font-size: 14px;'>Best regards,<br>Chroma Shop Service</p>
            
        </body>
    </html>";

            // Attach the HTML content to the email body
            email.Body = new TextPart(TextFormat.Html) { Text = htmlContent };



            string apppassword = "equx wkrd giiz drkv";
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("duynguyen120504@gmail.com", apppassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
