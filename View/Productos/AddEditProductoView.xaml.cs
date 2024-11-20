using RotiNet.Modelos;
using RotiNet.ViewModels;

namespace RotiNet.View.Productos;

public partial class AddEditProductoView : ContentPage
{
    AddEditProductoViewModel viewModel;
    public AddEditProductoView()
	{
		InitializeComponent();
	}

	public AddEditProductoView (Producto producto)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditProductoViewModel;
        viewModel.Producto = producto;
    }
}