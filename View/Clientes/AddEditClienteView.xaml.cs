using RotiNet.Modelos; 
using RotiNet.ViewModels;
using Microsoft.Maui.Controls;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace RotiNet.View.Clientes
{
    public partial class AddEditClienteView : ContentPage
    {
        AddEditClienteViewModel viewModel;

        public AddEditClienteView()
        {
            InitializeComponent();
            
        }

        
        public AddEditClienteView(Cliente cliente)
        {
            InitializeComponent();
            viewModel = this.BindingContext as AddEditClienteViewModel;
            viewModel.Cliente = cliente;
        }
    }
}
