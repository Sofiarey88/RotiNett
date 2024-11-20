using RotiNet.Modelos;
using RotiNet.ViewModels;

namespace RotiNet.View.Pedidos;

public partial class AddEditPedidoView : ContentPage
{
    AddEditPedidoViewModel viewModel;

    public AddEditPedidoView()
	{
		InitializeComponent();
	}

    public AddEditPedidoView(Pedido pedido)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditPedidoViewModel;
        viewModel.Pedido = pedido;
    }
}