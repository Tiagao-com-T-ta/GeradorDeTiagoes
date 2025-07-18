using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Domain.Shared;
using System;

namespace GeradorDeTiagoes.Domain.Entities
{
    public class Test : BaseEntity<Test>
    {
        public string Title { get; set; }

        public string GradeLevel { get; set; }

        public Guid DisciplineId { get; set; }

        public Guid? SubjectId { get; set; } 

        public int QuestionCount { get; set; }

        public bool IsRecovery { get; set; }

        public Discipline Discipline { get; set; }
        public Subject Subject { get; set; }
        public List<Question> Questions { get; set; } = new();

        public Test()
        {
            Id = Guid.NewGuid();
        }

        public Test(string title, string gradeLevel, Guid disciplineId, Guid? subjectId, int questionCount, bool isRecovery)
        {
            Id = Guid.NewGuid();
            Title = title;
            GradeLevel = gradeLevel;
            DisciplineId = disciplineId;
            SubjectId = subjectId;
            QuestionCount = questionCount;
            IsRecovery = isRecovery;
        }

        public override void Update(Test editedEntity)
        {
            Title = editedEntity.Title;
            GradeLevel = editedEntity.GradeLevel;
            DisciplineId = editedEntity.DisciplineId;
            SubjectId = editedEntity.SubjectId;
            QuestionCount = editedEntity.QuestionCount;
            IsRecovery = editedEntity.IsRecovery;
        }
    }
}
