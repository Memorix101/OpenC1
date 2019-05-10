# OpenC1

OpenC1 is a ground-up remake of the classic driving and wrecking game by Stainless Software.

[Project page](http://1amstudios.com/projects/openc1)

#### Important Notes
 * Ported from XNA 3.0 to MonoGame (DesktopGL)
 * **Running in custom MonoGame build (to fake XNA 3.0 for the Nvidia Physx .Net Wrapper)**
 * **The content from the content folder needs to be built with the custom build Pipeline tool first before debugging otherwise it will throw an exception! Just run the `run_content_pipeline.bat`. You can find it in `` OpenC1_MonoGame\MonoGame-develop\Tools\Pipeline\bin\Windows\AnyCPU\Debug ``**

 #### Konwn Issues
* BasiscEffect2.fx is currently broken and needs to be ported to MonoGame as well!
* Cannot get in-game (where the 3D stuff happens)😕
  * The parses seem to misbehave ... not sure ... 🧐
 
#### Thanks to: 
  Stainless Software (the original developers - of course!), 
  <br/>
  Toshiba-3, 
  <br/>
  www.stilldesign.co.nz (PhysX.Net), 
  <br/>
  Neale Davidson (Fontana font),
  <br/>
  Shayde (txt file decryption),
  <br/>
  PhysXInfo.com, 
  <br/>
  cwaboard.com