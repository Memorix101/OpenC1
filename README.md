# OpenC1

OpenC1 is a ground-up remake of the classic driving and wrecking game by Stainless Software.

[Project page](http://1amstudios.com/projects/openc1)

#### Building

```
dotnet build OpenC1_MonoGame.sln
```

Content is compiled automatically by MGCB during the build and copied next to the executable.

Requirements: .NET SDK (any current version) and a .NET Framework 4.8 runtime.
MGCB 3.8.0 is a `netcoreapp3.1` tool; `MonoGame.Content.targets` runs it with
`DOTNET_ROLL_FORWARD=Major`, so no ancient runtime needs to be installed.


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