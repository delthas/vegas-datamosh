// MAGIX Vegas (>=14) script to set random automation
// values for video effects quickly and automatically.
//
// Author: delthas
// Date: 2018-11-29
// License: MIT
// Source: https://github.com/delthas/vegas-datamosh
// Documentation: https://github.com/delthas/vegas-datamosh
// Version: 1.3.0
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using ScriptPortal.Vegas;

namespace VegasAutomator {
  public class EntryPoint {
    private static readonly Random Random = new Random();

    public void FromVegas(Vegas vegas) {
      var events = vegas.Project.Tracks
        .SelectMany(track => track.Events)
        .Where(t => t.Selected)
        .Where(t => t.IsVideo())
        .Cast<VideoEvent>()
        .ToList();

      var effects = events
        .SelectMany(ev => ev.Effects)
        .Where(ev => !ev.Bypass)
        .Where(ev => {
          try {
            return ev.IsOFX;
          } catch (COMException) {
            // vegas api throwing an exception if not ofx
            return false;
          }
        })
        .Select(ev => ev.OFXEffect)
        .GroupBy(ev => ev.Label)
        .Select(ev => ev.First())
        .ToList();

      var parameterEnabled = new HashSet<Tuple<string, string>>();

      foreach (var effect in effects) {
        foreach (var parameter in effect.Parameters) {
          if (parameter.Label == null) {
            continue;
          }
          if(!(parameter is OFXChoiceParameter
            || parameter is OFXDouble2DParameter
            || parameter is OFXDouble3DParameter
            || parameter is OFXDoubleParameter
            || parameter is OFXInteger2DParameter
            || parameter is OFXInteger3DParameter
            || parameter is OFXIntegerParameter
            || parameter is OFXRGBAParameter
            || parameter is OFXRGBParameter)) {
              continue;
          }

          var key = effect.Label.Trim() + " - " + parameter.Label.Trim();
          string hashed;
          using (MD5 md5 = MD5.Create()) {
            hashed = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", "");
          }
          var renderChecked = (string) Registry.GetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
            "Automate_" + hashed, "");
          var defaultCheck = renderChecked == "True";
          
          var prompt = new Form {
            Width = 300,
            Height = 150,
            Text = "Automator Parameters",
            KeyPreview = true
          };
          var textLabel = new Label {Left = 10, Top = 10, Width = 280, Text = key};
          var textLabel2 = new Label {Left = 80, Top = 45, Text = "Scramble"};
          var inputBox = new CheckBox {
            Left = 200,
            Top = 40,
            Width = 240,
            Checked = defaultCheck
          };
          var confirmation = new Button {Text = "OK", Left = 110, Width = 100, Top = 75};
          confirmation.Click += (sender, e) => {
            prompt.DialogResult = DialogResult.OK;
            prompt.Close();
          };
          prompt.KeyPress += (sender, args) => {
            if (args.KeyChar != ' ') return;
            inputBox.Checked = !inputBox.Checked;
            args.Handled = true;
          };
          prompt.KeyUp += (sender, args) => {
            if (args.KeyCode != Keys.Space) return;
            args.Handled = true;
          };
          prompt.Controls.Add(confirmation);
          prompt.Controls.Add(textLabel);
          prompt.Controls.Add(inputBox);
          prompt.Controls.Add(textLabel2);
          prompt.AcceptButton = confirmation;
          inputBox.Select();
          if (prompt.ShowDialog() != DialogResult.OK) {
            return;
          }
          
          if (defaultCheck != inputBox.Checked) {
            Registry.SetValue(
              "HKEY_CURRENT_USER\\SOFTWARE\\Sony Creative Software\\Custom Presets",
              "Automate_" + hashed, inputBox.Checked.ToString(), RegistryValueKind.String);
          }
          
          if (inputBox.Checked) {
            parameterEnabled.Add(new Tuple<string, string>(effect.Label, parameter.Name));
          }
        }
      }
      
      if (parameterEnabled.Count == 0) {
        return;
      }

      foreach (var ev in events) {
        foreach (var effect in ev.Effects) {
          if (effect.Bypass) {
            continue;
          }
          try {
            if (!effect.IsOFX) {
              continue;
            }
          } catch (COMException) {
            // vegas api throwing an exception if not ofx
            continue;
          }
          var ofx = effect.OFXEffect;
          foreach (var parameter in ofx.Parameters) {
            if (!parameterEnabled.Contains(new Tuple<string, string>(ofx.Label, parameter.Name))) {
              continue;
            }

            if(parameter is OFXChoiceParameter) {
                var p = parameter as OFXChoiceParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), p.Choices[Random.Next(0, p.Choices.Length)]);
                }
            } else if (parameter is OFXDouble2DParameter) {
                var p = parameter as OFXDouble2DParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), new OFXDouble2D {
                    X = p.DisplayMin.X + (p.DisplayMax.X - p.DisplayMin.X) * Random.NextDouble(),
                    Y = p.DisplayMin.Y + (p.DisplayMax.Y - p.DisplayMin.Y) * Random.NextDouble()
                  });
                }
            } else if (parameter is OFXDouble3DParameter) {
                var p = parameter as OFXDouble3DParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), new OFXDouble3D {
                    X = p.DisplayMin.X + (p.DisplayMax.X - p.DisplayMin.X) * Random.NextDouble(),
                    Y = p.DisplayMin.Y + (p.DisplayMax.Y - p.DisplayMin.Y) * Random.NextDouble(),
                    Z = p.DisplayMin.Z + (p.DisplayMax.Z - p.DisplayMin.Z) * Random.NextDouble()
                  });
                }
            } else if (parameter is OFXDoubleParameter) {
                var p = parameter as OFXDoubleParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), p.DisplayMin + (p.DisplayMax - p.DisplayMin) * Random.NextDouble());
                }
            } else if (parameter is OFXInteger2DParameter) {
                var p = parameter as OFXInteger2DParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), new OFXInteger2D {
                    X = Random.Next(p.DisplayMin.X, p.DisplayMax.X),
                    Y = Random.Next(p.DisplayMin.Y, p.DisplayMax.Y)
                  });
                }
            } else if (parameter is OFXInteger3DParameter) {
                var p = parameter as OFXInteger3DParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), new OFXInteger3D {
                    X = Random.Next(p.DisplayMin.X, p.DisplayMax.X),
                    Y = Random.Next(p.DisplayMin.Y, p.DisplayMax.Y),
                    Z = Random.Next(p.DisplayMin.Z, p.DisplayMax.Z)
                  });
                }
            } else if (parameter is OFXIntegerParameter) {
                var p = parameter as OFXIntegerParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), Random.Next(p.DisplayMin, p.DisplayMax));
                }
            } else if (parameter is OFXRGBAParameter) {
                var p = parameter as OFXRGBAParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), new OFXColor(Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
                }
            } else if (parameter is OFXRGBParameter) {
                var p = parameter as OFXRGBParameter;
                for (int i = 0; i < ev.Length.FrameCount; i++) {
                  p.SetValueAtTime(Timecode.FromFrames(i), new OFXColor(Random.NextDouble(), Random.NextDouble(), Random.NextDouble()));
                }
            }
          }
        }      
      }
    }
  }
}
