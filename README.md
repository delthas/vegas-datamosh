# vegas-datamosh
**A Sony/MAGIX Vegas Pro datamoshing script, using FFmpeg and Avidemux**

## Description
This script lets you do datamoshing, by repeating some P-frames multiple times. This adds weird movement to the video, and corrupts the video data because the next P-frames refer to invalid data. You just need to select a part of the timeline, then the script will render it, datamosh it with avidemux, and add the datamoshed result to the timeline. The script will repeat the P-frames at the beginning of the selection and simply append the next frames.

Since an MPEG4 stream always starts with an I-frame, the script will use the frame before the frame at the start of the selection as the I-frame. So you probably don't want to start the selection on the same frame an event starts.

Also, as always this kind of datamoshing works better when there is movement in the video in the frames that are repeated, i.e. at the beginning of the selection. Make sure to start the selection on frames with a lot of movement for best results.

The script attempts to keep a very high video quality, by using uncompressed and very high bitrate formats. Obviously the result is not lossless but it should not be too pixelated.

## Setup
Download the latest [release](../../releases/) and unpack it into your ```C:\Users\<user>\Documents\Vegas Script Menu``` folder. (If the folder does not exist, create it.)

Two entries will be added in the ```Tools -> Scripting``` submenu in Vegas. If you use Sony Vegas <= 13, use Datamosh, otherwise use Datamosh14.

You can add the datamoshing script as a toolbar button rather than having to click inside the ```Tools -> Scripting``` submenu, by adding it to the toolbar using the ```Options -> Customize Toolbar``` menu.

## Usage
To use, make sure you have added some video or audio clips into Vegas' timeline. Then, choose the region you want to datamosh using the loop region selectors (press I in the timeline to start the selection and O to end it). Then start the script by clicking on it in ```Tools -> Scripting```. The script may ask you to restart Vegas the first time you use the plugin, and may ask you to choose the folder which will contain the generated datamoshed files. It will then let you choose some datamoshing options, then render and generate the file. On my computer the process takes about as long as the length of the part that is generated.


The datamoshing options are:
- ```Frame block size```: the number of consecutive P-frames to repeat (from 1 to 5 is usually good)
- ```Frame block repeat```: the number of times to duplicate the P-frames (from 5 to 50 is usually good)

**Simply explained: this script repeats N P-frames M times. N is the ```Frame block size```, M is the  ```Frame block repeat```.

If you need to report an error please open an issue on this repository.


## License
All the code is licensed under MIT. The releases include binaries of FFmpeg and Avidemux and the Xvid codec, which are under the LGPL+GPL and GPL and GPL licenses respectively.