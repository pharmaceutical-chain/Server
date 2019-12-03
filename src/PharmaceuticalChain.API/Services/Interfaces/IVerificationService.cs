using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IVerificationService
    {

        /// <summary>
        /// Aimed to be called by third-party retailer clients. 
        /// Before printing an invoice, they ask our server to create a specific URL for that invoice.
        /// The URL can be printed or informed to the drug buyers to let them scan/review the drugs they purchased.
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="batchIds"></param>
        /// <returns></returns>
        string CreateVerificatorLink(Guid retailerId, List<Guid> batchIds);

        /// <summary>
        /// Verificator links are usually long. Use a shortner to make it shorter. Save data to embed to QR codes.
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        Task<string> CreateShortenedLink(string longUrl);

        /// <summary>
        /// Take a short link and embed it into a QR code.
        /// This QR code will be used to print on bills for end-users to verify their drugs with Verificator.
        /// </summary>
        /// <returns>Return a QR code with an URL. That URL navigates to a specific Verificator website for a drug purchasing bill.</returns>
        Task<string> CreateQRCode();
    }
}
