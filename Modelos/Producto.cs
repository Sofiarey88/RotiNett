namespace RotiNet.Modelos
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; } = 0m; // Inicializa con el valor predeterminado de decimal
    }
}
