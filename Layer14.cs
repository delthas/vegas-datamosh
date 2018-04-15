﻿// MAGIX Vegas (>=14) script to do multilayering on a part of a video
// quickly and automatically.
//
// Author: delthas
// Date: 2018-04-15
// License: MIT
// Source: https://github.com/delthas/vegas-datamosh
// Documentation: https://github.com/delthas/vegas-datamosh
// Version: 1.2.1
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ScriptPortal.Vegas;

namespace VegasLayering {
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
      var videoTrackIndex = -1;
      VideoTrack videoTrackStart = null;
      VideoEvent videoEvent = null;
      for (var i = 0; i < vegas.Project.Tracks.Count; i++) {
        var track = vegas.Project.Tracks[i];
        if (!track.IsVideo())
          continue;
        foreach (var trackEvent in track.Events) {
          if (!trackEvent.Selected) continue;
          if (videoEvent != null) {
            MessageBox.Show("Only a single video event can be selected!");
            return;
          }

          videoTrackIndex = i;
          videoTrackStart = (VideoTrack) track;
          videoEvent = (VideoEvent) trackEvent;
        }
      }

      if (videoEvent == null) {
        MessageBox.Show("Select a video event to be layered!");
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

        var template = GetTemplate(vegas, frameRateInt);
        if (template == null) {
          GetStandardTemplates(vegas);
          GetTemplate(vegas, frameRateInt);
          MessageBox.Show(
            "Render template generated for the current frame rate. Please restart Sony Vegas and run the script again.");
          return;
        }

        var layeringCount = (string) Registry.GetValue(
          "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
          "LayerCount", "");
        var defaultCount = 1;
        if (layeringCount != "") {
          try {
            var value = int.Parse(layeringCount);
            if (value > 0) {
              defaultCount = value;
            }
          }
          catch (Exception) {
            // ignore
          }
        }

        var renderChecked = (string) Registry.GetValue(
          "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
          "RenderLayer", "");
        var defaultCheck = renderChecked == "True";
        var prompt = new Form {
          Width = 500,
          Height = 170,
          Text = "Layering Parameters",
          KeyPreview = true
        };
        var textLabel = new Label {Left = 10, Top = 10, Text = "Layer count"};
        var inputBox = new NumericUpDown {
          Left = 200,
          Top = 10,
          Width = 200,
          Minimum = 1,
          Maximum = 1000000000,
          Value = defaultCount
        };
        var textLabel2 = new Label {Left = 10, Top = 40, Text = "Layering offset"};
        var inputBox2 =
          new NumericUpDown {Left = 200, Top = 40, Width = 200, Minimum = -1000000000, Maximum = 1000000000, Text = ""};
        var textLabel3 = new Label {Left = 10, Top = 70, Text = "Render"};
        var inputBox3 = new CheckBox {
          Left = 200,
          Top = 70,
          Width = 200,
          Checked = defaultCheck
        };
        var confirmation = new Button {Text = "OK", Left = 200, Width = 100, Top = 100};
        confirmation.Click += (sender, e) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };
        prompt.KeyPress += (sender, args) => {
          if (args.KeyChar != ' ') return;
          inputBox3.Checked = !inputBox3.Checked;
          args.Handled = true;
        };
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(textLabel);
        prompt.Controls.Add(inputBox);
        prompt.Controls.Add(textLabel2);
        prompt.Controls.Add(inputBox2);
        prompt.Controls.Add(textLabel3);
        prompt.Controls.Add(inputBox3);
        inputBox2.Select();
        prompt.AcceptButton = confirmation;
        if (prompt.ShowDialog() != DialogResult.OK) {
          return;
        }
        var count = (int) inputBox.Value;
        var offset = (int) inputBox2.Value;
        var render = inputBox3.Checked;

        if (offset == 0) {
          MessageBox.Show("Layering offset must not be 0!");
          return;
        }

        if (count <= 0) {
          MessageBox.Show("Layer count must be > 0!");
          return;
        }
        
        if (defaultCount != count) {
          Registry.SetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
            "LayerCount", count.ToString(), RegistryValueKind.String);
        }
        if (defaultCheck != render) {
          Registry.SetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
            "RenderLayer", render.ToString(), RegistryValueKind.String);
        }

        var newTracks = new List<VideoTrack>();
        var newEvents = new List<VideoEvent>();
        var current = 0;
        var baseOffset = offset > 0 ? 0 : -count * offset;

        for (var i = videoTrackIndex - 1; i >= 0 && current < count; i--) {
          var videoTrack = vegas.Project.Tracks[i] as VideoTrack;
          if (videoTrack == null) continue;
          newEvents.Add((VideoEvent) videoEvent.Copy(videoTrack, Timecode.FromFrames(videoEvent.Start.FrameCount + baseOffset + (++current) * offset)));
        }

        for (; current < count;) {
          var videoTrack = vegas.Project.AddVideoTrack();
          newTracks.Add(videoTrack);
          newEvents.Add((VideoEvent) videoEvent.Copy(videoTrack, Timecode.FromFrames(videoEvent.Start.FrameCount + baseOffset + (++current) * offset)));
        }

        var start = videoEvent.Start;
        if (offset < 0) {
          videoEvent.Start = Timecode.FromFrames(videoEvent.Start.FrameCount + baseOffset);
        }

        if (!render) return;
        var changed = false;
        var finalFolder = (string) Registry.GetValue(
          "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
          "LayerClipFolder", "");
        while (string.IsNullOrEmpty(finalFolder) || !Directory.Exists(finalFolder)) {
          MessageBox.Show("Select the folder to put generated layered clips into.\n" +
                          "(As they are stored uncompressed with alpha, they can take a lot of space (think 1 GB/minute). " +
                          "Choose a location with a lot of available space and go remove some clips there if you need space.)");
          changed = true;
          var dialog = new CommonOpenFileDialog {
            IsFolderPicker = true,
            EnsurePathExists = true,
            EnsureFileExists = false,
            AllowNonFileSystemItems = false,
            DefaultFileName = "Select Folder",
            Title = "Select the folder to put generated layered clips into"
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
            "LayerClipFolder", finalFolder, RegistryValueKind.String);
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
          Length = Timecode.FromFrames(videoEvent.Length.FrameCount + count * Math.Abs(offset)),
          RenderTemplate = template
        };
        var status = vegas.Render(renderArgs);
        if (status != RenderStatus.Complete) {
          MessageBox.Show("Unexpected render status: " + status);
          return;
        }

        File.Delete(pathEncoded + ".sfl");

        var media = vegas.Project.MediaPool.AddMedia(path);
        var newVideoEvent = videoTrackStart.AddVideoEvent(start,
          Timecode.FromFrames(videoEvent.Length.FrameCount + count * Math.Abs(offset)));
        ((VideoStream) newVideoEvent.AddTake(media.GetVideoStreamByIndex(0)).MediaStream).AlphaChannel =
          VideoAlphaType.Straight;
        videoEvent.Track.Events.Remove(videoEvent);

        foreach (var newEvent in newEvents) {
          newEvent.Track.Events.Remove(newEvent);
        }
        foreach (var newTrack in newTracks) {
          vegas.Project.Tracks.Remove(newTrack);
        }
      }
      catch (Exception e) {
        MessageBox.Show("Unexpected exception: " + e.Message);
        Debug.WriteLine(e);
      }
    }
  }
}