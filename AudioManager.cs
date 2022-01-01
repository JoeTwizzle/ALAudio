using OpenTK.Audio.OpenAL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL.Extensions;
using OpenTK.Audio.OpenAL.Extensions.Creative.EFX;
using OpenTK.Audio.OpenAL.Extensions.EXT.Double;
using OpenTK.Audio.OpenAL.Extensions.EXT.Float32;
using OpenTK.Audio.OpenAL.Extensions.EXT.FloatFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALAudio
{
    public class AudioManager : IDisposable
    {
        public readonly ALDevice Device;
        public readonly ALContext Context;
        public AudioManager()
        {
            var devices = ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier);
            Console.WriteLine($"Devices: {string.Join(", ", devices)}");

            // Get the default Device, then go though all devices and select the AL soft Device if it exists.
            string deviceName = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);
            foreach (var d in devices)
            {
                if (d.Contains("OpenAL Soft"))
                {
                    deviceName = d;
                }
            }

            var allDevices = OpenTK.Audio.OpenAL.Extensions.Creative.EnumerateAll.EnumerateAll.GetStringList(OpenTK.Audio.OpenAL.Extensions.Creative.EnumerateAll.GetEnumerateAllContextStringList.AllDevicesSpecifier);
            Console.WriteLine($"All Devices: {string.Join(", ", allDevices)}");

            Device = ALC.OpenDevice(deviceName);
            Context = ALC.CreateContext(Device, (int[])null);
            ALC.MakeContextCurrent(Context);
            ALC.GetInteger(Device, AlcGetInteger.MajorVersion, 1, out int alcMajorVersion);
            ALC.GetInteger(Device, AlcGetInteger.MinorVersion, 1, out int alcMinorVersion);
            string alcExts = ALC.GetString(Device, AlcGetString.Extensions);

            var attrs = ALC.GetContextAttributes(Device);
            Console.WriteLine($"Attributes: {attrs}");

            string exts = AL.Get(ALGetString.Extensions);
            string rend = AL.Get(ALGetString.Renderer);
            string vend = AL.Get(ALGetString.Vendor);
            string vers = AL.Get(ALGetString.Version);

            Console.WriteLine($"Vendor: {vend}, \nVersion: {vers}, \nRenderer: {rend}, \nExtensions: {exts}, \nALC Version: {alcMajorVersion}.{alcMinorVersion}, \nALC Extensions: {alcExts}");

            Console.WriteLine("Available devices: ");
            var list = OpenTK.Audio.OpenAL.Extensions.Creative.EnumerateAll.EnumerateAll.GetStringList(OpenTK.Audio.OpenAL.Extensions.Creative.EnumerateAll.GetEnumerateAllContextStringList.AllDevicesSpecifier);
            foreach (var item in list)
            {
                Console.WriteLine("  " + item);
            }
        }

        public void Dispose()
        {
            ALC.DestroyContext(Context);
            ALC.CloseDevice(Device);
        }
    }
}
