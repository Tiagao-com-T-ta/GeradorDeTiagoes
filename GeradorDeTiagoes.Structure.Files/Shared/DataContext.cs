using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Files.Shared
{
    public class DataContext
    {
        private string folder = "C:\\temp";
        private string file = "Gerador-Tiagoes-Data.json";

        // declarar aqui
        public List<Discipline> Disciplines { get; set; }
        public List<Test> Tests { get; set; }
        public List<Subject> Subjects { get; set; }

        public DataContext()
        {
            // inicializar aqui
            Disciplines = new List<Discipline>();
            Tests = new List<Test>();
            Subjects = new List<Subject>;
        }

        public DataContext(bool loadData) : this()
        {
            if (loadData)
            {
                LoadData();
            }
        }


        public void SaveData()
        {
            string fullPath = Path.Combine(folder, file);

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();


            jsonOptions.WriteIndented = true;
            jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

            string json = JsonSerializer.Serialize(this, jsonOptions);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(fullPath, json);

        }

        public void LoadData()
        {
            string fullPath = Path.Combine(folder, file);

            if (!File.Exists(fullPath)) return;

            string json = File.ReadAllText(fullPath);

            if (string.IsNullOrWhiteSpace(json)) return;

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

            DataContext? dataContext = JsonSerializer.Deserialize<DataContext>(json, jsonOptions)!;

            if (dataContext == null) return;

            //carregar aqui
            Disciplines = dataContext.Disciplines;
            Tests = dataContext.Tests;
            Subjects = dataContext.Subjects;
        }
    }
}
