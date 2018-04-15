// Sony Vegas (<=13) script to scramble clips/events
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
using System.Linq;
using System.Windows.Forms;
using Sony.Vegas;

namespace VegasScramble {
  public class EntryPoint {
    private static readonly Random Random = new Random();

    public void FromVegas(Vegas vegas) {
      var events = vegas.Project.Tracks
        .SelectMany(track => track.Events)
        .Where(t => t.Selected)
        .GroupBy(
          t => new {
            StartFrameCount = t.Start.FrameCount,
            LengthFrameCount = t.Length.FrameCount
          })
        .Select(grp => grp.ToList())
        .ToList();

      var prompt = new Form {
        Width = 500,
        Height = 110,
        Text = "Scrambling Parameters"
      };
      var textLabel = new Label {Left = 10, Top = 10, Text = "Scramble size"};
      var inputBox = new NumericUpDown {
        Left = 200,
        Top = 10,
        Width = 200,
        Minimum = 1,
        Maximum = 1000000000,
        Text = ""
      };
      var confirmation = new Button {Text = "OK", Left = 200, Width = 100, Top = 40};
      confirmation.Click += (sender, e) => {
        prompt.DialogResult = DialogResult.OK;
        prompt.Close();
      };
      prompt.Controls.Add(confirmation);
      prompt.Controls.Add(textLabel);
      prompt.Controls.Add(inputBox);
      inputBox.Select();
      prompt.AcceptButton = confirmation;
      if (prompt.ShowDialog() != DialogResult.OK) {
        return;
      }

      var size = (int) inputBox.Value;

      if (size <= 0) {
        MessageBox.Show("Scrambling size must be > 0!");
        return;
      }

      try {
        foreach (var e in events) {
          var order = new List<int>();
          var startFrameCount = e[0].Start.FrameCount;
          var endFrameCount = e[0].End.FrameCount;
          var n = (int) (endFrameCount - startFrameCount);
          var l = n / size;
          if(l == 0) continue;
          if (n % size != 0) {
            ++l;
          }
          for (var i = 0; i < l; i++) {
            order.Add(i);
          }

          
          for (var i = 0; i < l - 1; i++) {
            var k = i + 1 + Random.Next(l - i - 1);
            var v = order[k];
            order[k] = order[i];
            order[i] = v;
          }

          foreach (var evt in e) {
            int offset;
            for (var i = l - 1; i > 0; i--) {
              var other = evt.Split(Timecode.FromFrames(i * size));
              offset = order[i] > order[l - 1] ? -(size - n % size) : 0;
              other.Start = Timecode.FromFrames(startFrameCount + offset + order[i] * size);
            }
            offset = order[0] > order[l - 1] ? -(size - n % size) : 0;
            evt.Start = Timecode.FromFrames(startFrameCount + offset + order[0] * size);
          }
        }
      }
      catch (Exception e) {
        MessageBox.Show("Unexpected exception: " + e.Message);
        Debug.WriteLine(e);
      }
    }
  }
}