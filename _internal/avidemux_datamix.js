//AD
// Datamosh a part of a video quickly and automatically (mosh a clip onto another).
//

include("config_datamix.js");
var app = new Avidemux();
app.load(input1);
var len = app.markerB+1;
app.append(input0);
app.clearSegments();
app.addSegment(1,0,1);
app.addSegment(0,1,len-1);

app.markerA=0;
app.markerB=len-1;

app.setContainer("AVI");
app.video.codec("copy","CQ=4","");

app.audio.normalizeMode=0;
app.audio.normalizeValue=0;
app.audio.delay=0;
app.audio.mixer="NONE";
app.setContainer("AVI");
app.save(output);
app.exit();
