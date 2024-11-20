using RotiNet.Modelos;
using RotiNet.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using RotiNet.Class;

namespace RotiNet.ViewModels
{
    public class AddEditProductoViewModel : ObservableObject
    {
        private readonly GenericService<Producto>  productoService = new GenericService<Producto>();

        private Producto producto;
        public Producto Producto
        {
            get => producto;
            set
            {
                producto = value ?? new Producto(); // Asigna un nuevo Cliente si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditProductoViewModel()
        {
            Producto = new Producto(); // Asegura que Cliente esté inicializado al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Producto == null)
            {
                Producto = new Producto(); // Crea una nueva instancia de Cliente si es null
            }

            try
            {
                // Verifica si es un cliente nuevo o existente
                if (Producto.Id == 0)
                {
                    await productoService.AddAsync(Producto); // Agrega el cliente nuevo
                }
                else
                {
                    await productoService.UpdateAsync(Producto); // Actualiza el cliente existente
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAProductos"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar producto: {ex.Message}");
                // Opcional: Mostrar alerta en la UI sobre el error
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAProductos"));
        }
    }
}
