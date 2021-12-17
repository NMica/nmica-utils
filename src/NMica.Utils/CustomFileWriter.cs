using System;
using System.IO;

namespace NMica.Utils
{
    public class CustomFileWriter
    {
        private readonly StreamWriter _streamWriter;
        private readonly int _indentationFactor;
        private readonly string _commentPrefix;
        private int _indentation;

        public CustomFileWriter(StreamWriter streamWriter, int indentationFactor, string commentPrefix)
        {
            _streamWriter = streamWriter;
            _indentationFactor = indentationFactor;
            _commentPrefix = commentPrefix;
        }

        public void WriteLine(string text = null)
        {
            _streamWriter.WriteLine(
                text != null
                    ? $"{new string(c: ' ', _indentation * _indentationFactor)}{text}"
                    : string.Empty);
        }

        public void WriteComment(string text = null)
        {
            WriteLine($"{_commentPrefix} {text}".TrimEnd());
        }

        public void Write(Action<CustomFileWriter> writer)
        {
            writer(this);
        }

        public IDisposable Indent()
        {
            return DelegateDisposable.CreateBracket(
                () => _indentation++,
                () => _indentation--);
        }
    }
}
