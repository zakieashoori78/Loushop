using Loushop.Dtos;
using Loushop.Dtos.requestDto.Zibal;
using Loushop.Dtos.ResponseDto.Zibal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Loushop.ExternalServices
{
    public interface IZibalService
    {
        Task<ZibalRequestResponseDto> Request(ZibalRequestDto data);
        //Task<BaseResponse> Verify(string trackId);
    }
    public class ZibalService : IZibalService
    {
        private readonly HttpClient httpClient;
        private readonly ZibalURL zibalURL;

        public ZibalService(HttpClient httpClient, IOptions<ZibalURL> zibalURL)
        {
            this.httpClient = httpClient;
            this.zibalURL = zibalURL.Value;
        }

        public async Task<ZibalRequestResponseDto> Request(ZibalRequestDto data)
        {
            using var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await httpClient.PostAsync("https://gateway.zibal.ir/v1/request", content);

            // Ensure success response
            if (response.IsSuccessStatusCode)
            {
                // Parse the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                // Return the successful response
                return new ZibalRequestResponseDto
                {
                    trackId = (long)responseData.trackId,
                    result = (long)responseData.result,
                    message = (string)responseData.message,
                    payLink = "https://gateway.zibal.ir/start/" + responseData.trackId,
                    method = "Get"
                };
            }
            else
            {
                // Handle errors and return a failure response
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ZibalRequestResponseDto();
            }
        }
        //public async Task<BaseResponse> Request(ZibalRequestDto data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await httpClient.PostAsync("https://gateway.zibal.ir/v1/request", content);


        //    return new BaseResponse { Result = true };
        //    //var response = await httpClient.Execute<ZibalRequestDto, ZibalRequestResponseDto>(data, null, zibalURL.Request, HttpMethod.Post);
        //    //if (response.Success)
        //    //{
        //    //    return BaseResponse<ZibalRequestResponseDto>.GetSuccess(new ZibalRequestResponseDto
        //    //    {
        //    //        trackId = response.Data.trackId,
        //    //        BaseResponse = response.Data.BaseResponse,
        //    //        message = response.Data.message,
        //    //        payLink = zibalURL.Start + response.Data.trackId,
        //    //        method = "Get"
        //    //    });
        //    //}

        //    // return BaseResponse<ZibalRequestResponseDto>.GetFailure(response.Error, BaseResponseStatus.Failure);
        //}

        //public async Task<BaseResponse> Verify(string trackId)
        //{
        //    ZibalVerifyRequestDto Request = new ZibalVerifyRequestDto(); // define Request
        //    Request.merchant = "";
        //    Request.trackId = trackId;

        //    var response = await httpClient.Execute<ZibalVerifyRequestDto, ZibalVerifyResponseDto>(Request, null, zibalURL.Verify, HttpMethod.Post);
        //    if (response.Success)
        //    {
        //        return BaseResponse<ZibalVerifyResponseDto>.GetSuccess(response.Data);
        //    }

        //    return BaseResponse<ZibalVerifyResponseDto>.GetFailure(response.Error, BaseResponseStatus.Failure);
        //}



    }
}
