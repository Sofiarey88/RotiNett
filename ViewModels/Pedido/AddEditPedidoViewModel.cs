using RotiNet.Modelos;
using RotiNet.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using RotiNet.Class;
using System;
using System.Collections.ObjectModel;

namespace RotiNet.ViewModels
{
    public class AddEditPedidoViewModel : ObservableObject
    {
        // Servicios
        private GenericService<Pedido> pedidoService = new GenericService<Pedido>();
        private GenericService<Cliente> clienteService = new GenericService<Cliente>();
        private GenericService<Producto> productoService = new GenericService<Producto>();

        // Propiedades de Pedido, Cliente y Producto
        private Pedido pedido;
        public Pedido Pedido
        {
            get => pedido;
            set
            {
                pedido = value ?? new Pedido(); // Asegura que Pedido esté inicializado
                OnPropertyChanged();
            }
        }

        private Cliente cliente;
        public Cliente Cliente
        {
            get => cliente;
            set
            {
                cliente = value ?? new Cliente(); // Asegura que Cliente esté inicializado
                OnPropertyChanged();
            }
        }

        private Producto producto;
        public Producto Producto
        {
            get => producto;
            set
            {
                producto = value ?? new Producto(); // Asegura que Producto esté inicializado
                OnPropertyChanged();
            }
        }

        // Comandos
        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private ObservableCollection<Cliente> clientes;

        public ObservableCollection<Cliente> Clientes
        {
            get { return clientes; }
            set
            {
                clientes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Producto> productos;
        public ObservableCollection<Producto> Productos
        {
            get
            { return productos; }
            set
            {
                productos = value;
                OnPropertyChanged();
            }
        }




        // Constructor para creación de nuevo pedido
        public AddEditPedidoViewModel()
        {
            Pedido = new Pedido(); // Inicializa el pedido vacío
            Cliente = new Cliente(); // Inicializa el cliente vacío
            Producto = new Producto(); // Inicializa el producto vacío

            ObtenerListas(); // Cargar listas de clientes y productos
            // Comandos
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private async void ObtenerListas()
        {
            try
            {
                // Cargar la lista de clientes
                var clientes = await clienteService.GetAllAsync();
                Clientes = new ObservableCollection<Cliente>(clientes);
                // Cargar la lista de productos
                var productos = await productoService.GetAllAsync();
                Productos = new ObservableCollection<Producto>(productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las listas: {ex.Message}");
            }

        }

        // Constructor para editar pedido existente
        public AddEditPedidoViewModel(int pedidoId)
        {
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            ObtenerListas(); // Cargar listas de clientes y productos
            CargarDatosPedido(pedidoId); // Carga los datos cuando se pasa el pedidoId
        }

        // Método para cargar los datos del pedido, cliente y producto
        private async Task CargarDatosPedido(int pedidoId)
        {
            if (pedidoId > 0)
            {
                try
                {
                    // Cargar los datos del pedido desde el servicio
                    Pedido = await pedidoService.GetByIdAsync(pedidoId);
                    //if (Pedido != null)
                    //{
                    //    // Cargar los datos del cliente asociado al pedido
                    //    Cliente = await clienteService.GetByIdAsync(Pedido.ClienteId);

                    //    // Cargar los datos del producto asociado al pedido
                    //    Producto = await productoService.GetByIdAsync(Pedido.ProductoId);
                    //}
                    if (Pedido == null)
                    {
                        Console.WriteLine($"No se encontró el pedido con Id {pedidoId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar los datos del pedido: {ex.Message}");
                }
            }
        }

        // Método para guardar el pedido (nuevo o existente)
        private async Task Guardar()
        {
            if (Pedido == null || Cliente == null || Producto == null)
            {
                Console.WriteLine("Faltan datos para completar el pedido.");
                return;
            }

            try
            {
                if (Pedido.Id == 0)
                {
                    pedido.ClienteId = Cliente.Id;
                    pedido.ProductoId = Producto.Id;
                    // El pedido es nuevo, lo agregamos
                    await pedidoService.AddAsync(Pedido);
                }
                else
                {
                    pedido.ClienteId = Cliente.Id;
                    pedido.ProductoId = Producto.Id;
                    // El pedido ya existe, lo actualizamos
                    await pedidoService.UpdateAsync(Pedido);
                }

                // Enviar mensaje para actualizar la lista de pedidos
                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAPedidos"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el pedido: {ex.Message}");
            }
        }

        // Método para cancelar la operación
        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAPedidos"));
        }
    }
}