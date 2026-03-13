using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    /// <summary>
    /// MVP12: Minimal sequential file reader for READ ... AT END / NOT AT END.
    /// StreamReader-based, returns bool from ReadNext(), exposes CurrentRecord.
    /// </summary>
    public sealed class SequentialFileReader : IDisposable
    {
        private readonly StreamReader _reader;
        private bool _disposed;

        /// <summary>
        /// Current record after successful ReadNext(). Empty when EOF or before first ReadNext.
        /// </summary>
        public string CurrentRecord { get; private set; } = string.Empty;

        public SequentialFileReader(string path)
            : this(path, Encoding.ASCII)
        {
        }

        public SequentialFileReader(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("MVP12: path must not be null or empty.", nameof(path));
            }

            _reader = new StreamReader(path, encoding);
        }

        /// <summary>
        /// Reads next record. Returns true if record was read, false on EOF.
        /// On EOF, CurrentRecord is set to empty string.
        /// </summary>
        public bool ReadNext()
        {
            string line = _reader.ReadLine();
            if (line == null)
            {
                CurrentRecord = string.Empty;
                return false;
            }

            CurrentRecord = line;
            return true;
        }

        /// <summary>
        /// MVP12: READ INTO is not supported for sequential input.
        /// Throws NotSupportedException when encountered.
        /// </summary>
        public void ReadInto()
        {
            throw new NotSupportedException("MVP12: READ INTO is not supported for sequential input.");
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _reader.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
