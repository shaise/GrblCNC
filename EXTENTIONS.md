## GRBL for Chinese CNC3040-like machines. WIP.
### Extensions to the original Grbl 

#### Command Extentions:
* $H\<axis\> : When auto home is not enabled, this command can reset the current axis position to 0.
 Similar to pressing Home on this axis in LinuxCNC. For example, to reset X axis position: $HX

#### Report Extentions:
* '|Hs:\<val\>' : This reports the current bitmask home state of each axis. When the axis is homed, 
 the coresponding bit will be 1. For example Hs:5 means Axis X and Z were homed.
 
