using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Shared;
using System;
using System.Collections.Generic;

namespace GeradorDeTiagoes.Domain.Entities
{
    public class Subject : BaseEntity<Subject>
    {
        public string Name { get; set; }

        public string GradeLevel { get; set; } 

        public Guid DisciplineId { get; set; }

        public Discipline Discipline { get; set; }

        public List<Question> Questions { get; set; } = new();

        public Subject()
        {
            Id = Guid.NewGuid();
        }

        public Subject(string name, string gradeLevel, Guid disciplineId)
        {
            Id = Guid.NewGuid();
            Name = name;
            GradeLevel = gradeLevel;
            DisciplineId = disciplineId;
        }

        public override void Update(Subject editedEntity)
        {
            Name = editedEntity.Name;
            GradeLevel = editedEntity.GradeLevel;
            DisciplineId = editedEntity.DisciplineId;
        }
    }
}
