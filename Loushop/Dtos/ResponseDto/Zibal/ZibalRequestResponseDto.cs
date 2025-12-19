namespace Loushop.Dtos.ResponseDto.Zibal
{
    public class ZibalRequestResponseDto
    {
        public long trackId { get; set; }
        public long result { get; set; }
        public string? message { get; set; }
        public string? payLink { get; set; }
        public string? method { get; set; }
    }
}
