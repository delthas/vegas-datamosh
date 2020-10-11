// Datamoshes a part of a video quickly and automatically (mosh a clip onto another).
//

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sony.Vegas;

namespace VegasDatamix {
  public class EntryPoint {
    private static readonly byte[] Array1 = {
      0x42, 0x02, 0x00, 0x00, 0x02, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x7b, 0x28, 0x9d, 0xea, 0x5a, 0xc8, 0xd3, 0x11, 0xbb, 0x3a, 0x00, 0x50, 0xda, 0x1a,
      0x5b, 0x06
    };

    private static readonly byte[] Array2 = {
      0xc8, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00,
      0x00, 0x11, 0x00, 0x00, 0x00, 0x80, 0xbb, 0x00, 0x00, 0x02, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xee, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc8, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x28,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x40, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    private static readonly byte[] Array3 = {
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf0, 0x3f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x44,
      0x49, 0x42, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd0, 0x3f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x55,
      0x00, 0x6e, 0x00, 0x63, 0x00, 0x6f, 0x00, 0x6d, 0x00, 0x70, 0x00, 0x72, 0x00, 0x65, 0x00, 0x73, 0x00, 0x73, 0x00,
      0x65, 0x00, 0x64, 0x00, 0x20, 0x00
    };

    private static readonly byte[] Array4 = {0x00, 0x00, 0x00, 0x00};

    private static void GetStandardTemplates(Vegas vegas) {
      GetTemplate(vegas, 12000);
      GetTemplate(vegas, 12500);
      GetTemplate(vegas, 14000);
      GetTemplate(vegas, 14985);
      GetTemplate(vegas, 15000);
      GetTemplate(vegas, 16000);
      GetTemplate(vegas, 23976);
      GetTemplate(vegas, 24000);
      GetTemplate(vegas, 25000);
      GetTemplate(vegas, 29970);
      GetTemplate(vegas, 30000);
      GetTemplate(vegas, 50000);
      GetTemplate(vegas, 59940);
      GetTemplate(vegas, 60000);
    }

    private static RenderTemplate GetTemplate(Vegas vegas, int frameRate) {
      if (frameRate >= 100 * 1000) {
        throw new ArgumentException("Frame rate must be < 100!");
      }

      var frameString = (frameRate / 1000).ToString("00") + "." + (frameRate % 1000).ToString("000");
      var name = "Uncompressed " + frameString;
      var template = vegas.Renderers.FindByRendererID(0).Templates
        .FindByName(name);
      if (template != null) {
        return template;
      }

      var appData = Environment.GetEnvironmentVariable("APPDATA");
      if (appData == null) {
        throw new IOException("APPDATA not set!");
      }

      var folder = Path.Combine(appData, "Sony", "Render Templates", "avi");
      Directory.CreateDirectory(folder);
      var file = Path.Combine(folder, name + ".sft2");
      if (File.Exists(file)) {
        return null;
      }

      using (var writer = new BinaryWriter(new FileStream(file, FileMode.Create, FileAccess.Write))) {
        writer.Write(Array1, 0, Array1.Length);
        var guid = Guid.NewGuid().ToByteArray();
        writer.Write(guid, 0, guid.Length);
        writer.Write(Array2, 0, Array2.Length);
        writer.Write((double) frameRate / 1000);
        writer.Write(Array3, 0, Array3.Length);
        var chars = frameString.ToCharArray();
        foreach (var ch in chars) {
          writer.Write((byte) ch);
          writer.Write((byte) 0x00);
        }

        writer.Write(Array4, 0, Array4.Length);
        return null;
      }
    }

    private void Encode(Vegas vegas, string scriptDirectory, RenderArgs renderArgs, string pathEncoded) {
      var status = vegas.Render(renderArgs);
      if (status != RenderStatus.Complete) {
        MessageBox.Show("Unexpected render status: " + status);
        return;
      }

      var encode = new Process {
        StartInfo = {
          UseShellExecute = false,
          FileName = Path.Combine(scriptDirectory, "_internal", "ffmpeg", "ffmpeg.exe"),
          WorkingDirectory = Path.Combine(scriptDirectory, "_internal"),
          Arguments = "-y -hide_banner -nostdin -i \"" + renderArgs.OutputFile +
                      "\" -c:v libxvid -q:v 1 -g 1M -flags +mv4+qpel -mpeg_quant 1 -c:a copy \"" + pathEncoded +
                      "\"",
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          CreateNoWindow = true
        }
      };
      encode.Start();
      var error = encode.StandardError.ReadToEnd();
      var output = encode.StandardOutput.ReadToEnd();
      Debug.WriteLine(output);
      Debug.WriteLine("---------------------");
      Debug.WriteLine(error);
      encode.WaitForExit();

      File.Delete(renderArgs.OutputFile);
      File.Delete(renderArgs.OutputFile + ".sfl");
    }

    public void FromVegas(Vegas vegas) {
      var start = vegas.Transport.LoopRegionStart;
      var length = vegas.Transport.LoopRegionLength;

      if (start.FrameCount == 0) {
        MessageBox.Show("Selection must start at frame >= 1!");
        return;
      }
      if (length.FrameCount <= 1) {
        MessageBox.Show("Selection length must be > 1 frame!");
        return;
      }

      try {
        var frameRate = vegas.Project.Video.FrameRate;
        var frameRateInt = (int) Math.Round(frameRate * 1000);

        var scriptDirectory = Path.GetDirectoryName(Script.File);
        if (scriptDirectory == null) {
          MessageBox.Show("Couldn't get script directory path!");
          return;
        }

        var xvidPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        if (string.IsNullOrEmpty(xvidPath)) {
          xvidPath = Environment.GetEnvironmentVariable("ProgramFiles");
        }
        if (string.IsNullOrEmpty(xvidPath)) {
          MessageBox.Show("Couldn't get Xvid install path!");
          return;
        }
        xvidPath += @"\Xvid\uninstall.exe";
        if (!File.Exists(xvidPath)) {
          MessageBox.Show(
            "Xvid codec not installed. The script will install it now and may ask for admin access to install it.");
          var xvid = new Process {
            StartInfo = {
              UseShellExecute = true,
              FileName = Path.Combine(scriptDirectory, "_internal", "xvid", "xvid.exe"),
              WorkingDirectory = Path.Combine(scriptDirectory, "_internal"),
              Arguments =
                "--unattendedmodeui none  --mode unattended  --AutoUpdater no --decode_divx DIVX  --decode_3ivx 3IVX --decode_divx DIVX --decode_other MPEG-4",
              CreateNoWindow = true,
              Verb = "runas"
            }
          };
          try {
            xvid.Start();
          }
          catch (Win32Exception e) {
            if (e.NativeErrorCode == 1223) {
              MessageBox.Show("Admin privilege for Xvid installation refused.");
              return;
            }

            throw;
          }

          xvid.WaitForExit();
          GetStandardTemplates(vegas);
          GetTemplate(vegas, frameRateInt);
          MessageBox.Show(
            "Xvid installed and render template generated for the current frame rate. Please restart Sony Vegas and run the script again.");
          return;
        }

        var template = GetTemplate(vegas, frameRateInt);
        if (template == null) {
          GetStandardTemplates(vegas);
          GetTemplate(vegas, frameRateInt);
          MessageBox.Show(
            "Render template generated for the current frame rate. Please restart Sony Vegas and run the script again.");
          return;
        }

        VideoTrack videoTrack = null;
        for (var i = vegas.Project.Tracks.Count - 1; i >= 0; i--) {
          videoTrack = vegas.Project.Tracks[i] as VideoTrack;
          if (videoTrack != null) {
            break;
          }
        }

        if (videoTrack == null) {
          MessageBox.Show("No track found!");
          return;
        }

        var changed = false;
        var finalFolder = (string) Registry.GetValue(
          "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
          "ClipFolder", "");
        while (string.IsNullOrEmpty(finalFolder) || !Directory.Exists(finalFolder)) {
          MessageBox.Show("Select the folder to put generated datamoshed clips into.");
          changed = true;
          var dialog = new CommonOpenFileDialog {
            IsFolderPicker = true,
            EnsurePathExists = true,
            EnsureFileExists = false,
            AllowNonFileSystemItems = false,
            DefaultFileName = "Select Folder",
            Title = "Select the folder to put generated datamoshed clips into"
          };

          if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
            MessageBox.Show("No folder selected");
            return;
          }

          finalFolder = dialog.FileName;
        }

        if (changed) {
          Registry.SetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
            "ClipFolder", finalFolder, RegistryValueKind.String);
        }

        var pathSrc = Path.Combine(vegas.TemporaryFilesPath, Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) + ".avi");
        var pathEncodedSrc = Path.Combine(vegas.TemporaryFilesPath,
          Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) + ".avi");
        Encode(vegas, scriptDirectory, new RenderArgs {
          OutputFile = pathSrc,
          Start = Timecode.FromFrames(start.FrameCount - 1),
          Length = Timecode.FromFrames(1),
          RenderTemplate = template
        }, pathEncodedSrc);
        
        var pathClip = Path.Combine(vegas.TemporaryFilesPath, Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
                                                             Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) + ".avi");
        var pathEncodedClip = Path.Combine(vegas.TemporaryFilesPath,
          Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) + ".avi");
        Encode(vegas, scriptDirectory, new RenderArgs {
          OutputFile = pathClip,
          Start = start,
          Length = length,
          RenderTemplate = template
        }, pathEncodedClip);

        var pathDatamixed = Path.Combine(finalFolder,
          Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8)) + ".avi";

        string[] datamoshConfig = {
          "var input0=\"" + pathEncodedSrc.Replace("\\", "/") + "\";",
          "var input1=\"" + pathEncodedClip.Replace("\\", "/") + "\";",
          "var output=\"" + pathDatamixed.Replace("\\", "/") + "\";"
        };

        File.WriteAllLines(Path.Combine(scriptDirectory, "_internal", "config_datamix.js"), datamoshConfig);

        var datamosh = new Process {
          StartInfo = {
            UseShellExecute = false,
            FileName = Path.Combine(scriptDirectory, "_internal", "avidemux", "avidemux2_cli.exe"),
            WorkingDirectory = Path.Combine(scriptDirectory, "_internal"),
            Arguments = "--nogui --run avidemux_datamix.js",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
          }
        };
        datamosh.Start();
        datamosh.StandardInput.WriteLine("n");
        var output = datamosh.StandardOutput.ReadToEnd();
        var error = datamosh.StandardError.ReadToEnd();
        Debug.WriteLine(output);
        Debug.WriteLine("---------------------");
        Debug.WriteLine(error);
        datamosh.WaitForExit();

        File.Delete(pathEncodedSrc);
        File.Delete(pathEncodedSrc + ".sfl");
        File.Delete(pathEncodedClip);
        File.Delete(pathEncodedClip + ".sfl");

        var media = vegas.Project.MediaPool.AddMedia(pathDatamixed);
        media.TimecodeIn = Timecode.FromFrames(1);

        var videoEvent = videoTrack.AddVideoEvent(start, Timecode.FromFrames(length.FrameCount - 1));
        videoEvent.AddTake(media.GetVideoStreamByIndex(0));
      }
      catch (Exception e) {
        MessageBox.Show("Unexpected exception: " + e.Message);
        Debug.WriteLine(e);
      }
    }
  }
}
