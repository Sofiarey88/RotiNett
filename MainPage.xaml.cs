using CommunityToolkit.Mvvm.Messaging;
using RotiNet.Class;
using RotiNet.View;
using RotiNet.View.Clientes;
using RotiNet.View.Pedidos;
using RotiNet.View.Productos;
using RotiNet.View.Proveedores;

namespace RotiNet
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            //código para preparar la recepción de mensajes y la llamada al método RecibirMensaje
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "AbrirAddEditClienteView")
            {
                await Navigation.PushAsync(new AddEditClienteView(m.Cliente));
            }
            if (m.Value == "AbrirClientes")
            {
                await Navigation.PushAsync(new ClienteView());
            }
            if (m.Value == "VolverAClientes")
            {
                await Navigation.PopAsync();
            }

            if (m.Value == "AbrirAddEditProveedorView")
            {
                await Navigation.PushAsync(new AddEditProveedorView(m.Proveedor));
            }
            if (m.Value == "AbrirProveedores")
            {
                await Navigation.PushAsync(new ProveedorView());
            }
            if (m.Value == "VolverAProveedores")
            {
                await Navigation.PopAsync();
            }
            if(m.Value == "AbrirAddEditProductoView")
            {
                await Navigation.PushAsync(new AddEditProductoView(m.Producto));
            }
            if (m.Value == "AbrirProductos")
            {
                await Navigation.PushAsync(new ProductoView());
            }
            if (m.Value == "VolverAProductos")
            {
                await Navigation.PopAsync();
            }
            if (m.Value == "AbrirAddEditPedidoView")
            {
                await Navigation.PushAsync(new AddEditPedidoView(m.Pedido));
            }
            if (m.Value == "AbrirPedidos")
            {
                await Navigation.PushAsync(new PedidoView());
            }
            if (m.Value == "VolverAPedidos")
            {
                await Navigation.PopAsync();
            }


        }



        private async  void BtnClientes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClienteView());
        }

        private async void BtnProveedores_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProveedorView());

        }

        private  async void BtnProductos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductoView());
        }

        private async void BtnVentas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PedidoView());
        }
    }

}
