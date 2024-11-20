using RotiNet.Modelos;
using RotiNet.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using RotiNet.Class;

namespace RotiNet.ViewModels
{
    public class AddEditProveedorViewModel : ObservableObject
    {
        private readonly GenericService<Proveedor> proveedorService = new GenericService<Proveedor>();

        private Proveedor proveedor;
        public Proveedor Proveedor
        {
            get => proveedor;
            set
            {
                proveedor = value ?? new Proveedor(); // Asigna un nuevo Cliente si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditProveedorViewModel()
        {
            Proveedor = new Proveedor(); // Asegura que Cliente esté inicializado al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Proveedor == null)
            {
                Proveedor = new Proveedor(); // Crea una nueva instancia de Cliente si es null
            }

            try
            {
                // Verifica si es un cliente nuevo o existente
                if (Proveedor.Id == 0)
                {
                    await proveedorService.AddAsync(Proveedor); // Agrega el cliente nuevo
                }
                else
                {
                    await proveedorService.UpdateAsync(Proveedor); // Actualiza el cliente existente
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAProveedores"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar proveedor: {ex.Message}");
                // Opcional: Mostrar alerta en la UI sobre el error
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAProveedores"));
        }
    }
}
