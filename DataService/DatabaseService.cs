using cMainatoS5B.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cMainatoS5B.DataService
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Persona>().Wait(); // Crear la tabla 'Persona' si no existe
        }

        public Task<int> DeletePersonaAsync(Persona persona)
        {
            return _database.DeleteAsync(persona); // Aquí se usa '_database' en lugar de 'database'
        }

        public Task<int> SavePersonaAsync(Persona persona)
        {
            if (persona.Id != 0)
            {
                return _database.UpdateAsync(persona); // Actualiza la persona si tiene un Id
            }
            else
            {
                return _database.InsertAsync(persona); // Inserta una nueva persona si no tiene Id
            }
        }

        public Task<List<Persona>> GetPersonasAsync() => _database.Table<Persona>().ToListAsync();

        public Task<Persona> GetPersonaAsync(int id) => _database.Table<Persona>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public static class AppConstants
    {
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, "personas.db3");
    }

}
