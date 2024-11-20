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
    public class ClientesViewModel : NotificationObject // Assuming ObservableObject is your base class for ViewModel

    {


        GenericService<Cliente> clienteService = new GenericService<Cliente>();

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

        private Cliente clienteCurrent;
        public Cliente ClienteCurrent
        {
            get { return clienteCurrent; }
            set
            {
                clienteCurrent = value;
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

        public ClientesViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerClientes());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAClientes")
            {
                await RefreshClientes(this);
            }
        }

        private async Task RefreshClientes(object obj)
        {
            IsRefreshing = true;
            await ObtenerClientes();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return ClienteCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un cliente",
                $"Está seguro que desea eliminar el cliente {ClienteCurrent.Nombre}",
                "Si",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await clienteService.DeleteAsync(ClienteCurrent.Id);
                await ObtenerClientes();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return ClienteCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditClienteView") { Cliente = ClienteCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditClienteView"));
        }

        public async Task ObtenerClientes()
        {
            ActivityStart = true;
            var clientes = await clienteService.GetAllAsync();
            Clientes = new ObservableCollection<Cliente>(clientes);
            ActivityStart = false;
        }
    }
}