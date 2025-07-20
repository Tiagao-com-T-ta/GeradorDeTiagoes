using System;

namespace GeradorDeTiagoes.Domain.QuestionModule
{
    public class Alternative
    {
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public Alternative() { }

        public Alternative(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }
    }
}
