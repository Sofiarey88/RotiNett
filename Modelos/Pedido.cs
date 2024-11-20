namespace RotiNet.Modelos
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }

        public int ProductoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;

        // Relación con Cliente
        public Cliente? Cliente { get; set; }

        public Producto? Producto { get; set; } 
    }
}

