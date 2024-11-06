using cMainatoS5B.Model;

namespace cMainatoS5B.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // Cargar datos de la base de datos al iniciar la página
        LoadPersonas();
    }
    private async void LoadPersonas()
    {
        // Llama al servicio de base de datos para obtener la lista de personas
        personasListView.ItemsSource = await App.Database.GetPersonasAsync();
    }
    private async void OnEliminarPersonaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var persona = button?.CommandParameter as Persona;

        if (persona != null)
        {
            bool confirmacion = await DisplayAlert("Confirmación", $"¿Estás seguro de que deseas eliminar a {persona.Nombre}?", "Sí", "No");
            if (confirmacion)
            {
                await App.Database.DeletePersonaAsync(persona);
                LoadPersonas(); // Recargar la lista después de la eliminación
            }
        }
    }

    private async void OnEditarPersonaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var persona = button?.CommandParameter as Persona;

        if (persona != null)
        {
            var nuevoNombre = await DisplayPromptAsync("Editar Persona", "Introduce el nuevo nombre:", initialValue: persona.Nombre);
            if (!string.IsNullOrEmpty(nuevoNombre))
            {
                var nuevaEdadString = await DisplayPromptAsync("Editar Persona", "Introduce la nueva edad:", initialValue: persona.Edad.ToString());
                if (int.TryParse(nuevaEdadString, out int nuevaEdad))
                {
                    persona.Nombre = nuevoNombre;
                    persona.Edad = nuevaEdad;
                    await App.Database.SavePersonaAsync(persona); // Guarda los cambios en la base de datos
                    LoadPersonas(); // Recargar la lista después de la modificación
                }
                else
                {
                    await DisplayAlert("Error", "Edad no válida", "OK");
                }
            }
        }
    }
    private async void OnPersonaSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Obtiene la persona seleccionada y navega a una nueva página para detalles o edición (puedes personalizar esto)
        var personaSeleccionada = e.SelectedItem as Persona;
        await DisplayAlert("Persona Seleccionada", $"Nombre: {personaSeleccionada.Nombre}\nEdad: {personaSeleccionada.Edad}", "OK");

        // Deselecciona el elemento
        personasListView.SelectedItem = null;
    }

    private async void OnAgregarPersonaClicked(object sender, EventArgs e)
    {
        // Navega a una página o abre un formulario para agregar una nueva persona (puedes personalizar esto)
        var nombre = await DisplayPromptAsync("Agregar Persona", "Introduce el nombre:");
        if (!string.IsNullOrEmpty(nombre))
        {
            var edadString = await DisplayPromptAsync("Agregar Persona", "Introduce la edad:");
            if (int.TryParse(edadString, out int edad))
            {
                var nuevaPersona = new Persona { Nombre = nombre, Edad = edad };
                await App.Database.SavePersonaAsync(nuevaPersona);
                LoadPersonas(); // Recarga la lista
            }
            else
            {
                await DisplayAlert("Error", "Edad no válida", "OK");
            }
        }
    }

}