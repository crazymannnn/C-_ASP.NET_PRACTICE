using Ecommerce.Webmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.IService
{
    public interface IEmailService
    {
        void SendEmail(CreateEmailRequest request);
    }
}
