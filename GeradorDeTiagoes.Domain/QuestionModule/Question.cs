using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeradorDeTiagoes.Domain.QuestionModule
{
    public class Question : BaseEntity<Question>
    {
        public string Statement { get; set; }

        public Guid SubjectId { get; set; }

        public Subject Subject { get; set; }

        public List<Alternative> Alternatives { get; set; } = new();

        public Question() { }

        public Question(string statement, Guid subjectId, List<Alternative> alternatives)
        {
            Id = Guid.NewGuid();
            Statement = statement;
            SubjectId = subjectId;
            Alternatives = alternatives;

            Validate();
        }

        public override void Update(Question updated)
        {
            Statement = updated.Statement;
            SubjectId = updated.SubjectId;
            Alternatives = updated.Alternatives;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Statement))
                throw new ArgumentException("O enunciado da questão é obrigatório.");

            if (Alternatives == null || Alternatives.Count < 2 || Alternatives.Count > 4)
                throw new ArgumentException("A questão deve ter entre 2 e 4 alternativas.");

            var correctCount = Alternatives.Count(a => a.IsCorrect);
            if (correctCount == 0)
                throw new ArgumentException("A questão deve ter uma alternativa correta.");
            if (correctCount > 1)
                throw new ArgumentException("A questão não pode ter mais de uma alternativa correta.");
        }
    }
}
