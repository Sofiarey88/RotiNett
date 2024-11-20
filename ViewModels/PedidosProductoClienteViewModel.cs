using RotiNet.Modelos;
using System;

namespace RotiNet.ViewModels
{
    // Clase que encapsula el Cliente, Pedido y Producto para facilitar la visualización en la UI
    public class PedidosProductoClienteViewModel
    {
        // Cliente asociado con el pedido
        public Cliente Cliente { get; }

        // Pedido específico realizado
        public Pedido Pedido { get; }

        // Producto asociado al pedido
        public Producto Producto { get; }

        // Constructor para inicializar las propiedades con los valores proporcionados
        public PedidosProductoClienteViewModel(Cliente cliente, Pedido pedido, Producto producto)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo");
            Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido), "El pedido no puede ser nulo");
            Producto = producto ?? throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo");
        }

        // Constructor vacío opcional, útil si el framework lo requiere
        public PedidosProductoClienteViewModel() { }
    }
}
