# OpenC1

OpenC1 is a ground-up remake of the classic driving and wrecking game by Stainless Software.

[Project page](http://1amstudios.com/projects/openc1)

### Important Notes
 * Ported from XNA 3.0 to MonoGame (DesktopGL)
 * Running in custom MonoGame build (to fake XNA 3.0 for the Physix .Net Wrapper)
 * The content from the content folder needs to be built with the custom build Pipeline tool first before debugging otherwise it will throw an exception! You can fint it in
 ``` OpenC1_MonoGame\MonoGame-develop\Tools\Pipeline\bin\Windows\AnyCPU\Debug ```
 * BasiscEffect2.fx is currently disabled and needs to be ported to MonoGame as well!
 * Screenshot feature not yet reimplemented

 ## Konwn Issues
 * Cannot get in-game (where the 3D stuff happens) :(

Keys:
 * Up, Down, Left, Right - Accelerate, brake, steer
 * Space - Handbrake
 * Backspace - Repair
 * C - Change camera
 * R - Reset vehicle
 * F4 - Edit modes (for debugging)
 * P - Take screenshot
 
Thanks to:
  Stainless Software (the original developers - of course!),
  Toshiba-3,
  www.stilldesign.co.nz (PhysX.Net), 
  Neale Davidson (Fontana font),
  Shayde (txt file decryption),
  PhysXInfo.com, 
  cwaboard.com