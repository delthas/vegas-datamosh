﻿// Datamoshes a part of a video quickly and automatically.
//

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sony.Vegas;

namespace VegasDatamosh {
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

    public void FromVegas(Vegas vegas) {
      var start = vegas.Transport.LoopRegionStart;
      var length = vegas.Transport.LoopRegionLength;

      try {
        var frameRate = vegas.Project.Video.FrameRate;
        var frameRateInt = (int) Math.Round(frameRate * 1000);

        var scriptDirectory = Path.GetDirectoryName(Script.File);
        if (scriptDirectory == null) {
          MessageBox.Show("Couldn't get script directory path!");
          return;
        }

        const string xvidCheckPath = @"C:\Program Files (x86)\Xvid\uninstall.exe";
        if (!File.Exists(xvidCheckPath)) {
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
        
        var frameCount = (string) Registry.GetValue(
          "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
          "FrameCount", "");
        var defaultCount = 1;
        if (frameCount != "") {
          try {
            var value = int.Parse(frameCount);
            if (value > 0) {
              defaultCount = value;
            }
          }
          catch (Exception) {
            // ignore
          }
        }

        var prompt = new Form {
          Width = 500,
          Height = 140,
          Text = "Datamoshing Parameters"
        };
        var textLabel = new Label {Left = 10, Top = 10, Text = "Frame count"};
        var inputBox =
          new NumericUpDown {Left = 200, Top = 10, Width = 200, Minimum = 1, Maximum = 1000000000, Value = defaultCount};
        var textLabel2 = new Label {Left = 10, Top = 40, Text = "Frames repeats"};
        var inputBox2 = new NumericUpDown {
          Left = 200,
          Top = 40,
          Width = 200,
          Value = 1,
          Minimum = 1,
          Maximum = 1000000000,
          Text = ""
        };
        var confirmation = new Button {Text = "OK", Left = 200, Width = 100, Top = 70};
        confirmation.Click += (sender, e) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(textLabel);
        prompt.Controls.Add(inputBox);
        prompt.Controls.Add(textLabel2);
        prompt.Controls.Add(inputBox2);
        inputBox2.Select();
        prompt.AcceptButton = confirmation;
        if (prompt.ShowDialog() != DialogResult.OK) {
          return;
        }
        var size = (int) inputBox.Value;
        var repeat = (int) inputBox2.Value;

        if (repeat <= 0) {
          MessageBox.Show("Frames repeats must be > 0!");
          return;
        }

        if (length.FrameCount < size) {
          MessageBox.Show("The selection must be as long as the frame count!");
          return;
        }

        if (start.FrameCount < 1) {
          MessageBox.Show("The selection mustn't start on the first frame of the project!");
          return;
        }
        
        if (defaultCount != size) {
          Registry.SetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
            "FrameCount", size.ToString(), RegistryValueKind.String);
        }

        VideoTrack videoTrack = null;
        for (var i = vegas.Project.Tracks.Count - 1; i >= 0; i--) {
          videoTrack = vegas.Project.Tracks[i] as VideoTrack;
          if (videoTrack != null) {
            break;
          }
        }

        AudioTrack audioTrack = null;
        for (var i = 0; i < vegas.Project.Tracks.Count; i++) {
          audioTrack = vegas.Project.Tracks[i] as AudioTrack;
          if (audioTrack != null) {
            break;
          }
        }

        if (videoTrack == null && audioTrack == null) {
          MessageBox.Show("No tracks found!");
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

        var path = Path.Combine(vegas.TemporaryFilesPath, Path.GetFileNameWithoutExtension(vegas.Project.FilePath) +
                                                          "-" +
                                                          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) +
                                                          ".avi");
        var pathEncoded = Path.Combine(vegas.TemporaryFilesPath,
          Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) + ".avi");
        var pathDatamoshedBase = Path.Combine(finalFolder,
          Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8));
        var pathDatamoshed = pathDatamoshedBase + ".avi";
        var pathEncodedEscape = pathEncoded.Replace("\\", "/");
        var pathDatamoshedEscape = pathDatamoshed.Replace("\\", "/");

        var renderArgs = new RenderArgs {
          OutputFile = path,
          Start = Timecode.FromFrames(start.FrameCount - 1),
          Length = Timecode.FromFrames(length.FrameCount + 1),
          RenderTemplate = template
        };
        var status = vegas.Render(renderArgs);
        if (status != RenderStatus.Complete) {
          MessageBox.Show("Unexpected render status: " + status);
          return;
        }

        string[] datamoshConfig = {
          "var input=\"" + pathEncodedEscape + "\";",
          "var output=\"" + pathDatamoshedEscape + "\";",
          "var size=" + size + ";",
          "var repeat=" + repeat + ";"
        };

        File.WriteAllLines(Path.Combine(scriptDirectory, "_internal", "config_datamosh.js"), datamoshConfig);

        var encode = new Process {
          StartInfo = {
            UseShellExecute = false,
            FileName = Path.Combine(scriptDirectory, "_internal", "ffmpeg", "ffmpeg.exe"),
            WorkingDirectory = Path.Combine(scriptDirectory, "_internal"),
            Arguments = "-y -hide_banner -nostdin -i \"" + path +
                        "\" -c:v libxvid -q:v 1 -g 1M -flags +mv4+qpel -mpeg_quant 1 -c:a copy \"" + pathEncoded +
                        "\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
          }
        };
        encode.Start();
        var output = encode.StandardOutput.ReadToEnd();
        var error = encode.StandardError.ReadToEnd();
        Debug.WriteLine(output);
        Debug.WriteLine("---------------------");
        Debug.WriteLine(error);
        encode.WaitForExit();

        File.Delete(path);
        File.Delete(path + ".sfl");

        var datamosh = new Process {
          StartInfo = {
            UseShellExecute = false,
            FileName = Path.Combine(scriptDirectory, "_internal", "avidemux", "avidemux2_cli.exe"),
            WorkingDirectory = Path.Combine(scriptDirectory, "_internal"),
            Arguments = "--nogui --run avidemux_datamosh.js",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
          }
        };
        datamosh.Start();
        datamosh.StandardInput.WriteLine("n");
        output = datamosh.StandardOutput.ReadToEnd();
        error = datamosh.StandardError.ReadToEnd();
        Debug.WriteLine(output);
        Debug.WriteLine("---------------------");
        Debug.WriteLine(error);
        datamosh.WaitForExit();

        File.Delete(pathEncoded);
        File.Delete(pathEncoded + ".sfl");

        var media = vegas.Project.MediaPool.AddMedia(pathDatamoshed);
        media.TimecodeIn = Timecode.FromFrames(1);

        VideoEvent videoEvent = null;
        if (videoTrack != null) {
          videoEvent =
            videoTrack.AddVideoEvent(start, Timecode.FromFrames(1 + length.FrameCount + (repeat - 1) * size));
          videoEvent.AddTake(media.GetVideoStreamByIndex(0));
        }

        AudioEvent audioEvent = null;
        if (audioTrack != null) {
          audioEvent =
            audioTrack.AddAudioEvent(start, Timecode.FromFrames(1 + length.FrameCount + (repeat - 1) * size));
          audioEvent.AddTake(media.GetAudioStreamByIndex(0));
        }

        if (videoTrack != null && audioTrack != null) {
          var group = new TrackEventGroup();
          vegas.Project.Groups.Add(group);
          group.Add(videoEvent);
          group.Add(audioEvent);
        }
      }
      catch (Exception e) {
        MessageBox.Show("Unexpected exception: " + e.Message);
        Debug.WriteLine(e);
      }
    }
  }
}