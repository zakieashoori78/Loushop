namespace Loushop.Dtos.ResponseDto.Zibal
{
    public class ZibalInquiryResponseDto
    {
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public string? paidAt { get; set; }
        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public int? status { get; set; }
        /// <summary>
        /// مبلغ سفارش (به ریال)
        /// </summary>
        public int? amount { get; set; }
        /// <summary>
        /// نتیجه درخواست.
        /// </summary>
        public int? result { get; set; }
        /// <summary>
        /// پیغام حاوی نتیجه درخواست
        /// </summary>
        public string? message { get; set; }
        /// <summary>
        /// شناسه مرجع تراکنش (در صورت موفقیت‌آمیز بودن پرداخت)
        /// </summary>
        public string? refNumber { get; set; }
        /// <summary>
        /// توضیحات تراکنش (در صورت موفقیت‌آمیز بودن پرداخت)
        /// </summary>
        public string? description { get; set; }
        /// <summary>
        /// شماره کارت پرداخت کننده (Mask شده)
        /// </summary>
        public string? cardNumber { get; set; }
        /// <summary>
        /// شناسه سفارش (در صورت موفقیت‌آمیز بودن پرداخت)
        /// </summary>
        public string? orderId { get; set; }

        public string? createdAt { get; set; }

        public InquiryResponseMultiplexingInfo[]? multiplexingInfos { get; set; }
    }

    public class InquiryResponseMultiplexingInfo
    {
        public string amount { get; set; }
        public string bankAccount { get; set; }
    }
}
