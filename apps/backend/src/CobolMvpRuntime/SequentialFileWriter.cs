using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    /// <summary>
    /// MVP13: Minimal sequential file writer for WRITE record-name.
    /// StreamWriter-based, WriteLine for record output.
    /// </summary>
    public sealed class SequentialFileWriter : IDisposable
    {
        private readonly StreamWriter _writer;
        private bool _disposed;

        public SequentialFileWriter(string path)
            : this(path, Encoding.ASCII)
        {
        }

        public SequentialFileWriter(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("MVP13: path must not be null or empty.", nameof(path));
            }

            _writer = new StreamWriter(path, false, encoding);
        }

        /// <summary>
        /// Writes a record (line) to the file.
        /// </summary>
        public void WriteLine(string record)
        {
            _writer.WriteLine(record);
        }

        /// <summary>
        /// Flushes buffered data to the file.
        /// </summary>
        public void Flush()
        {
            _writer.Flush();
        }

        /// <summary>
        /// MVP13: WRITE ... FROM is not supported for sequential output.
        /// Throws NotSupportedException when encountered.
        /// </summary>
        public void WriteFrom()
        {
            throw new NotSupportedException("MVP13: WRITE ... FROM is not supported for sequential output.");
        }

        /// <summary>
        /// MVP13: WRITE AFTER ADVANCING is not supported.
        /// Throws NotSupportedException when encountered.
        /// </summary>
        public void WriteAfterAdvancing(int lines)
        {
            throw new NotSupportedException("MVP13: WRITE AFTER ADVANCING is not supported for sequential output.");
        }

        /// <summary>
        /// MVP13: WRITE BEFORE ADVANCING is not supported.
        /// Throws NotSupportedException when encountered.
        /// </summary>
        public void WriteBeforeAdvancing(int lines)
        {
            throw new NotSupportedException("MVP13: WRITE BEFORE ADVANCING is not supported for sequential output.");
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _writer.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
