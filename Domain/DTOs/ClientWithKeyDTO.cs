namespace Domain.DTOs
{
    public class ClientWithKeyDTO
    {
        public string client { get; set; }
        public string ipStart { get; set; }
        public string ipEnd { get; set; }
        public string referer { get; set; }
    }
}