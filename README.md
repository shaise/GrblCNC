# GrblCNC
A C# Clone to LinuxCNC that works with GRBL CNC driver  
#### This project is done in parallel with a compatible Grbl driver firmware
* [Click here for project page](https://github.com/shaise/grblHAL_CNC3040)
```diff
- This is a work in progress, only basic stuff are operational  
```
## Software
### Screen preview 
![GrblCNC interface](Media/grblcnc1.png?raw=true)

## Hardware
* [Click here for firmware project page](https://github.com/shaise/grblHAL_CNC3040)
* [Click here for project progress](PROGRESS.md)
* [Click here for list of added capabilities](EXTENTIONS.md)
### GRBL for Chinese CNC3040-like machines. WIP. 
### Based on STM32F103C8T6 "Bluepill" boards with customizes grblHAL firmware 
#### Project goals: 
* Develop simple open hardware plug to replace the vanishing parallel port
    - Support configurable pin assignments
    - Support extra controls not available on CNC3040 such as mist, flood, spindle RPM reader
* Add support for more axis. (at least add A axis)
* Support other features that LinuxCNC have

## Development Environment
|                      |                          |
|----------------------|--------------------------|
| **IDE**              | STM32CubeIDE v1.2.0      |
| **Controller board** | STM32F103C8T6 (Bluepill) |

## Reference
- GRBL CNC controller ported to STM32 controller (and others)
```https://github.com/terjeio/grblHAL```
- GRBL CNC controller
```https://github.com/gnea/grbl```

## Pin Diagram

![STM32 Pin Diagram](Media/pindiag.png?raw=true)

## GRBL dongle for CNC3040 concept art:
![Concept PCB](Media/GrblCnc3040.jpg?raw=true)
### Back side
![Concept PCB_back](Media/GrblCnc3040_back.jpg?raw=true)

