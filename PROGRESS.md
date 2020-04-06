## GRBL for Chinese CNC3040-like machines. WIP.
### Project progress:

* 06 Apr 2020:  Support of g54-59 offsets
  * Display gcode parser state
	* Working coordinate display on main 3D window
	* Coordinate touch off and tool probing
	* Error display similar to LinuxCNC
* 28 Mar 2020:  Basic Sender program available
  * move to grblHal as the grbl driver for its more comprehensive gcode support
* 18 Mar 2020:  Migration to STM32CubeIDE 
* 05 Mar 2020:  A and B axis basic functionality added
  * Add option to manualy home each axis (similar to LinuxCNC) 
* 04 Mar 2020:  Complete PCB editing of dongle. 
* 02 Mar 2020:  Fix some initial bugs related to using 8bit vars for 16bit ports.
* 01 Mar 2020:  Reorder pin assignments.
