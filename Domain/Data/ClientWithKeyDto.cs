namespace Domain.Data
{
    public class ClientWithKeyDto
    {
        public string clientName { get; set; }
        public string ipStart { get; set; } = "0.0.0.0";
        public string ipEnd { get; set; }  = "0.0.0.0";
        public string referer { get; set; }
    }
}