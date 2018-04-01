//AD
// Avidemux script to datamosh a part of a video
// quickly and automatically.
//
// Author: delthas
// Date: 2018-04-01
// License: MIT
// Source: https://github.com/delthas/vegas-datamosh
// Documentation: https://github.com/delthas/vegas-datamosh
// Version: 1.0.0
//
include("config_datamosh.js");
var app = new Avidemux();
app.load(input);
var len = app.markerB+1;
app.clearSegments();
app.addSegment(0,0,1);
for (i = 0; i < repeat; i++) {
    app.addSegment(0,1,size);
}
app.addSegment(0,1+size,len-1-size);

app.markerA=0;
app.markerB=len-1 + size*(repeat-1);

app.setContainer("AVI");
app.video.codec("copy","CQ=4","");

app.audio.normalizeMode=0;
app.audio.normalizeValue=0;
app.audio.delay=0;
app.audio.mixer="NONE";
app.setContainer("AVI");
app.save(output);
app.exit();
