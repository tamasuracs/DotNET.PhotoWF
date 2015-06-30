using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoWF.Common
{
    /// <summary>
    /// Helper class storing JPG metadata
    /// </summary>
    public class JpgMeta : IDisposable
    {
        Stream _stream;

        public BitmapDecoder Decoder
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JpgMeta(Stream stream_, BitmapDecoder decoder_)
        {
            _stream = stream_;
            Decoder = decoder_;
        }

        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();
        }
    }
}
