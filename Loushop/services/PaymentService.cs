using Loushop.Dtos.requestDto.Zibal;
using Loushop.Dtos.ResponseDto.Zibal;
using Loushop.Dtos;
using System.Net.Http;
using System.Threading.Tasks;
using Loushop.ExternalServices;
using Loushop.Dtos.RequestDto;
using Loushop.Models;

namespace Loushop.services
{


    public interface IPaymentService
    {
        Task<ZibalRequestResponseDto> Request(PaymentDto paymentDto);
    }
    public class PaymentService:IPaymentService
    {
        readonly private IZibalService zibalService;
        public PaymentService(IZibalService zibalService)
        {
            this.zibalService = zibalService;
        }
        public async Task<ZibalRequestResponseDto> Request(PaymentDto paymentDto)
        {
            //بررسی اینکه آیا یوزر ای دی که داده درست باشه

            var zibalRequestModel = new ZibalRequestDto
            {
                merchant = "zibal",
                amount = (long) paymentDto.Amount,
                callbackUrl = "http://localhost:44369/Home/OnlinePayment/" + paymentDto.OrderId,           
                description = "پرداخت",
                orderId = "09927556211"
            };

            return await zibalService.Request(zibalRequestModel);          

        }
    }
}
