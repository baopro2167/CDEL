using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailS
{
    public interface IEmailService
    {
        Task SendAsync(EmailDTO emailDTO);
    }
}
