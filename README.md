# Azure-Kinect-Recorder
Record audio and image information from one or more Azure Kinect DK's simultaneously with one button pressed.

## Can Do and Cannot Do
### Can Do
Record color image

Record audios of all seven channels for each device
### Cannot Do
Record depth and IR images

Playback the recorded video and audio file

Configure the resolution and FPS(frame per second) of color image (only ___3840x2160 30 FPS___ is supported)

Set the directory where to store the recorded file. Currently the records are stored in ___Videos library/Kinect___.

## Dependencies of this project
[NAudio](https://github.com/naudio/NAudio) for recording the audio

[k4a.net](https://github.com/bibigone/k4a.net) for image recording

## Prerequisite
Install the Kinect sensor SDK first in which there is the driver for running the hardware. Follow this [webpage](https://docs.microsoft.com/en-us/azure/kinect-dk/set-up-azure-kinect-dk).

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
GPL (GNU General Public License) version 2 or any later version.
