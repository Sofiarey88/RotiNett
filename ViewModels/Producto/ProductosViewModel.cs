using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.Communication;
using RotiNet.Class;
using RotiNet.Modelos;
using RotiNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace RotiNet.ViewModels
{
    public class 
        ProductosViewModel : NotificationObject // Assuming ObservableObject is your base class for ViewModel

    {
        

        GenericService<Producto> productoService = new GenericService<Producto>();

        private bool activityStart;
        public bool ActivityStart
        {
            get { return activityStart; }
            set
            {
                activityStart = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Producto> productos;
        public ObservableCollection<Producto> Productos
        {
            get { return productos; }
            set
            {
                productos = value;
                OnPropertyChanged();
            }
        }

        private Producto productoCurrent;
        public  Producto ProductoCurrent
        {
            get { return productoCurrent; }
            set
            {
                productoCurrent = value;
                OnPropertyChanged();
                EditarCommand.ChangeCanExecute();
                EliminarCommand.ChangeCanExecute();
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public Command AgregarCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }

        public ProductosViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerProductos());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAProductos")
            {
                await RefreshProductos(this);
            }
        }

        private async Task RefreshProductos(object obj)
        {
            IsRefreshing = true;
            await ObtenerProductos();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return ProductoCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un producto",
                $"Está seguro que desea eliminar el producto {ProductoCurrent.Nombre}",
                "Si",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await productoService.DeleteAsync(ProductoCurrent.Id);
                await ObtenerProductos();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return ProductoCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditProductoView") { Producto = ProductoCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditClienteView"));
        }

        public async Task ObtenerProductos()
        {
            ActivityStart = true;
            var productos = await productoService.GetAllAsync();
            Productos = new ObservableCollection<Producto>(productos);
            ActivityStart = false;
        }
    }
}
