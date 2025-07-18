using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GeradorDeTiagoes.Domain.QuestionModule
{
    public class Question : BaseEntity<Question>
    {
        public string Text { get; set; }
        public Subject Subject { get; set; }
        public List<Alternative> Alternatives { get; set; }

        
        public Question() { }
        
        public Question(string text, Subject subject, List<Alternative> alternatives)
        {
            Id = Guid.NewGuid();
            Text = text;
            Subject = subject;
            Alternatives = alternatives;
        }

        public override void Update(Question editedEntity)
        {
            Text = editedEntity.Text;
            Subject = editedEntity.Subject;
            Alternatives = editedEntity.Alternatives;
        }
    }

    public class Alternative
    {
        public string Text { get; set; }
        public bool isCorrect { get; set; }

        public Alternative(string text, bool isCorrect)
        {
            Text = text;
            this.isCorrect = isCorrect;
        }
    }
}
