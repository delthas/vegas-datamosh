# vegas-datamosh
**A Sony/MAGIX Vegas Pro datamoshing script, using FFmpeg and Avidemux**

## Setup
Download the latest [release](../../releases/) and unpack it into your ```C:\Users\<user>\Documents\Vegas Script Menu``` folder.

Two entries will be added in the ```Tools -> Scripting``` submenu in Vegas. If you use Sony Vegas <= 13, use Datamosh, otherwise use Datamosh14.


## Usage
To use, make sure you have added some video or audio clips into Vegas' timeline. Then, choose the region you want to datamosh using the loop region selectors (press I in the timeline to start the selection and O to end it). Then start the script by clicking on it in ```Tools -> Scripting```. The script may ask you to restart Vegas the first time you use the plugin, and may ask you to choose the folder which will contain the generated datamoshed files. It will then let you choose some datamoshing options, then render and generate the file. On my computer the process takes about as long as the length of the part that is generated.


The datamoshing options are:
- ```Frame block size```: the number of consecutive I-frames to repeat (1 is usually good)
- ```Frame block repeat```: the number of times to duplicate the I-frames (from 10 to 100 is usually good)


If you need to report an error please open an issue on this repository.


## License
All the code is licensed under MIT. The releases include binaries of FFmpeg and Avidemux, which are under the LGPL+GPL and GPL licenses respectively.