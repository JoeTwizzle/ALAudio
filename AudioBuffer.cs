using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace ALAudio
{
    public class AudioBuffer:IDisposable
    {
        public int Handle { get; }
        public AudioBuffer()
        {
            Handle = AL.GenBuffer();
        }

        /// <summary>
        /// Loads a .wav audio file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="channels">Number of audio channels</param>
        /// <param name="bits">Bits per sample</param>
        /// <param name="rate">Sample rate</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using BinaryReader reader = new BinaryReader(stream);
            // RIFF header
            string signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
                throw new NotSupportedException("Specified stream is not a wave file.");

            int riff_chunck_size = reader.ReadInt32();

            string format = new string(reader.ReadChars(4));
            if (format != "WAVE")
                throw new NotSupportedException("Specified stream is not a wave file.");

            // WAVE header
            string format_signature = new string(reader.ReadChars(4));
            if (format_signature != "fmt ")
                throw new NotSupportedException("Specified wave file is not supported.");

            int format_chunk_size = reader.ReadInt32();
            int audio_format = reader.ReadInt16();
            int num_channels = reader.ReadInt16();
            int sample_rate = reader.ReadInt32();
            int byte_rate = reader.ReadInt32();
            int block_align = reader.ReadInt16();
            int bits_per_sample = reader.ReadInt16();

            string data_signature = new string(reader.ReadChars(4));
            if (data_signature != "data")
                throw new NotSupportedException("Specified wave file is not supported.");

            int data_chunk_size = reader.ReadInt32();

            channels = num_channels;
            bits = bits_per_sample;
            rate = sample_rate;

            return reader.ReadBytes((int)reader.BaseStream.Length);
        }

        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public void Init(string file)
        {
            Init(File.OpenRead(file));
        }

        public void Init(Stream stream)
        {
            var data = LoadWave(stream, out int channels, out int bits, out int rate);
            var format = GetSoundFormat(channels, bits);
            AL.BufferData(Handle, format, ref data[0], data.Length, rate);
        }

        public void Init(byte[] data, ALFormat format, int sampleRate)
        {
            AL.BufferData(Handle, format, ref data[0], data.Length, sampleRate);
        }

        public void Dispose()
        {
            AL.DeleteBuffer(Handle);
        }
    }
}
