using RotiNet.Modelos;
using RotiNet.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using RotiNet.Class;

namespace RotiNet.ViewModels
{
    public class AddEditClienteViewModel : ObservableObject
    {
        private readonly GenericService<Cliente> clienteService = new GenericService<Cliente>();

        private Cliente cliente;
        public Cliente Cliente
        {
            get => cliente;
            set
            {
                cliente = value ?? new Cliente(); // Asigna un nuevo Cliente si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditClienteViewModel()
        {
            Cliente = new Cliente(); // Asegura que Cliente esté inicializado al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Cliente == null)
            {
                Cliente = new Cliente(); // Crea una nueva instancia de Cliente si es null
            }

            try
            {
                // Verifica si es un cliente nuevo o existente
                if (Cliente.Id == 0)
                {
                    await clienteService.AddAsync(Cliente); // Agrega el cliente nuevo
                }
                else
                {
                    await clienteService.UpdateAsync(Cliente); // Actualiza el cliente existente
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAClientes"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar cliente: {ex.Message}");
                // Opcional: Mostrar alerta en la UI sobre el error
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAClientes"));
        }
    }
}
