using RotiNet.Modelos;
using RotiNet.ViewModels;

namespace RotiNet.View.Proveedores;

public partial class AddEditProveedorView : ContentPage
{
    AddEditProveedorViewModel viewModel;
    public AddEditProveedorView()
	{
		InitializeComponent();
	}
    public AddEditProveedorView(Proveedor proveedor)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditProveedorViewModel;
        viewModel.Proveedor = proveedor;
    }
}