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
    public class ProveedoresViewModel : NotificationObject // Assuming ObservableObject is your base class for ViewModel

    {


        GenericService<Proveedor> proveedorService = new GenericService<Proveedor>();

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

        private ObservableCollection<Proveedor> proveedores;
        public ObservableCollection<Proveedor> Proveedores
        {
            get { return proveedores; }
            set
            {
                proveedores = value;
                OnPropertyChanged();
            }
        }

        private Proveedor proveedorCurrent;
        public Proveedor ProveedorCurrent
        {
            get { return proveedorCurrent; }
            set
            {
                proveedorCurrent = value;
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

        public ProveedoresViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerProveedores());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAProveedores")
            {
                await RefreshProveedores(this);
            }
        }

        private async Task RefreshProveedores(object obj)
        {
            IsRefreshing = true;
            await ObtenerProveedores();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return ProveedorCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un proveedor",
                $"Está seguro que desea eliminar el proveedor {ProveedorCurrent.Nombre}",
                "Si",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await proveedorService.DeleteAsync(ProveedorCurrent.Id);
                await ObtenerProveedores();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return ProveedorCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditProveedorView") { Proveedor = ProveedorCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditProveedorView"));
        }

        public async Task ObtenerProveedores()
        {
            ActivityStart = true;
            var proveedores = await proveedorService.GetAllAsync();
            Proveedores = new ObservableCollection<Proveedor>(proveedores);
            ActivityStart = false;
        }
    }
}
