using System;

namespace GeradorDeTiagoes.Domain.QuestionModule
{
    public class Alternative
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public Alternative() { }

        public Alternative(string text, bool isCorrect, Guid id)
        {
            Text = text;
            IsCorrect = isCorrect;
            Id = id;
        }
    }
}
