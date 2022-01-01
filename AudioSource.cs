using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace ALAudio
{
    public class AudioSource : IDisposable
    {
        public int Handle { get; }

        public AudioSource()
        {
            Handle = AL.GenSource();
        }

        public void SetBuffer(AudioBuffer audioBuffer)
        {
            AL.BindBufferToSource(Handle, audioBuffer.Handle);
        }

        public void SetLoop(bool state)
        {
            AL.Source(Handle, ALSourceb.Looping, state);
        }

        public void Play()
        {
            AL.SourcePlay(Handle);
        }

        public void Pause()
        {
            AL.SourcePause(Handle);
        }

        public void Rewind()
        {
            AL.SourceRewind(Handle);
        }

        public void SetPosition(Vector3 Position)
        {
            AL.Source(Handle, ALSource3f.Position, ref Position);
        }

        public void SetPitch(float pitch)
        {
            AL.Source(Handle, ALSourcef.Pitch, pitch);
        }

        public void SetVolume(float volume)
        {
            AL.Source(Handle, ALSourcef.Gain, volume);
        }

        public void Stop()
        {
            AL.SourceStop(Handle);
        }

        public void Dispose()
        {
            AL.SourceStop(Handle);
            AL.DeleteSource(Handle);
        }
    }
}
