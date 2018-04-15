﻿// MAGIX Vegas (>=14) script to render a part of a video
// quickly and automatically.
//
// Author: delthas
// Date: 2018-04-14
// License: MIT
// Source: https://github.com/delthas/vegas-datamosh
// Documentation: https://github.com/delthas/vegas-datamosh
// Version: 1.1.0
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ScriptPortal.Vegas;

namespace VegasRender {
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
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x40, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    private static readonly byte[] Array3 = {
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf0, 0x3f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x00, 0x44,
      0x49, 0x42, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd0, 0x3f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x55,
      0x00, 0x6e, 0x00, 0x63, 0x00, 0x6f, 0x00, 0x6d, 0x00, 0x70, 0x00, 0x72, 0x00, 0x41, 0x00, 0x6c, 0x00, 0x70, 0x00,
      0x68, 0x00, 0x61, 0x00, 0x20, 0x00
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
      var name = "UncomprAlpha " + frameString;
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
          "RenderClipFolder", "");
        while (string.IsNullOrEmpty(finalFolder) || !Directory.Exists(finalFolder)) {
          MessageBox.Show("Select the folder to put generated rendered clips into.\n" +
                          "(As they are stored uncompressed with alpha, they can take a lot of space (think 1 GB/minute). " +
                          "Choose a location with a lot of available space and go remove some clips there if you need space.)");
          changed = true;
          var dialog = new CommonOpenFileDialog {
            IsFolderPicker = true,
            EnsurePathExists = true,
            EnsureFileExists = false,
            AllowNonFileSystemItems = false,
            DefaultFileName = "Select Folder",
            Title = "Select the folder to put generated rendered clips into"
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
            "RenderClipFolder", finalFolder, RegistryValueKind.String);
        }

        var path = Path.Combine(vegas.TemporaryFilesPath, Path.GetFileNameWithoutExtension(vegas.Project.FilePath) +
                                                          "-" +
                                                          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) +
                                                          ".avi");
        var pathEncoded = Path.Combine(vegas.TemporaryFilesPath,
          Path.GetFileNameWithoutExtension(vegas.Project.FilePath) + "-" +
          Guid.NewGuid().ToString("B").ToUpper().Substring(1, 8) + ".avi");

        var renderArgs = new RenderArgs {
          OutputFile = path,
          Start = Timecode.FromFrames(start.FrameCount),
          Length = Timecode.FromFrames(length.FrameCount),
          RenderTemplate = template
        };
        var status = vegas.Render(renderArgs);
        if (status != RenderStatus.Complete) {
          MessageBox.Show("Unexpected render status: " + status);
          return;
        }

        File.Delete(pathEncoded + ".sfl");

        var media = vegas.Project.MediaPool.AddMedia(path);
        
        VideoEvent videoEvent = null;
        if (videoTrack != null) {
          videoEvent =
            videoTrack.AddVideoEvent(start, length);
          ((VideoStream) videoEvent.AddTake(media.GetVideoStreamByIndex(0)).MediaStream).AlphaChannel =
            VideoAlphaType.Straight;
        }

        AudioEvent audioEvent = null;
        if (audioTrack != null) {
          audioEvent =
            audioTrack.AddAudioEvent(start, length);
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