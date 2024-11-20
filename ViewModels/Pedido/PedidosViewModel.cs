using CommunityToolkit.Mvvm.Messaging;
using RotiNet.Class;
using RotiNet.Modelos;
using RotiNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RotiNet.ViewModels
{
    public class PedidosViewModel : NotificationObject
    {
        GenericService<Cliente> clienteService = new GenericService<Cliente>();
        GenericService<Producto> productoService = new GenericService<Producto>();
        GenericService<Pedido> pedidoService = new GenericService<Pedido>();

        private bool activityStart;
        public bool ActivityStart
        {
            get { return activityStart; }
            set
            {
                if (activityStart != value)
                {
                    activityStart = value;
                    OnPropertyChanged(); // Notifica el cambio
                }
            }
        }

        private ObservableCollection<PedidoProductoClienteViewModel> pedidos;
        public ObservableCollection<PedidoProductoClienteViewModel> Pedidos
        {
            get { return pedidos; }
            set
            {
                if (pedidos != value)
                {
                    pedidos = value;
                    OnPropertyChanged(); // Notifica el cambio
                }
            }
        }

        private PedidoProductoClienteViewModel pedidoCurrent;
        public PedidoProductoClienteViewModel PedidoCurrent
        {
            get { return pedidoCurrent; }
            set
            {
                if (pedidoCurrent != value)
                {
                    pedidoCurrent = value;
                    OnPropertyChanged(); // Notifica el cambio
                    EditarCommand.ChangeCanExecute();
                    EliminarCommand.ChangeCanExecute();
                }
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged(); // Notifica el cambio
                }
            }
        }

        public Command AgregarCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }

        public PedidosViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerPedidos());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAPedidos")
            {
                await RefreshPedidos(this);
            }
        }

        private async Task RefreshPedidos(object obj)
        {
            IsRefreshing = true;
            await ObtenerPedidos();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return PedidoCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un producto",
                $"Está seguro que desea eliminar el pedido de {PedidoCurrent.Cliente.Nombre}",
                "Si",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await pedidoService.DeleteAsync(PedidoCurrent.Pedido.Id);
                await ObtenerPedidos();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return PedidoCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditPedidoView") { Pedido = PedidoCurrent.Pedido });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditPedidoView"));
        }

        public async Task ObtenerPedidos()
        {
            ActivityStart = true;
            var pedidos = await pedidoService.GetAllAsync();
            var pedidosConDetalles = new ObservableCollection<PedidoProductoClienteViewModel>();

            foreach (var pedido in pedidos)
            {
                // Obtener Cliente asociado al Pedido
                var cliente = await clienteService.GetByIdAsync(pedido.ClienteId);

                // Obtener Producto(s) asociados al Pedido
                var productos = await productoService.GetAllAsync(); // O filtrar productos por pedido si necesario
                var producto = productos.FirstOrDefault(p => p.Id == pedido.ProductoId); // Asumiendo que Pedido tiene ProductoId

                // Crear el ViewModel combinado con Pedido, Cliente y Producto
                var pedidosProductoClienteViewModel = new PedidoProductoClienteViewModel
                {
                    Pedido = pedido,
                    Cliente = cliente,
                    Producto = producto
                };

                pedidosConDetalles.Add(pedidosProductoClienteViewModel);
            }

            Pedidos = pedidosConDetalles;
            ActivityStart = false;
        }
    }

    // ViewModel para combinar Pedido, Cliente y Produzcto
    public class PedidoProductoClienteViewModel
    {
        public Pedido Pedido { get; set; }
        public Cliente Cliente { get; set; }
        public Producto Producto { get; set; }
    }
}
