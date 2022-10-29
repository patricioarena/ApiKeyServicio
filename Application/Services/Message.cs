namespace Application.Services
{
    /// <summary>
    /// Clase para estandarizar mensajes.
    /// </summary>
    public static class Message
    {
        public const string null_Client = "Cliente es null o no exite";
        public const string null_Key = "Apikey es null o no exite";
        public const string null_App = "Application es null o no exite";
        public const string null_relationship = "No hay relación entre el cliente y la clave.";
        public const string null_relationship2 = "La unión entre la aplicación y la clave no existe";
        public const string null_ip = "El rango de IP es nulo o no está configurado correctamente";
        public const string ip_out_range = "La IP fuera de rango asignada para clave o desconocida";
    }
}
