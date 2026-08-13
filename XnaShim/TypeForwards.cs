using System.Runtime.CompilerServices;

// The only XNA types StillDesign.PhysX.Net uses in its signatures, taken from the
// TypeRef table of Lib/StillDesign.PhysX.Net.dll.
// Their layout matches XNA 3.0 (Vector3 = 3 floats, Quaternion = 4 floats,
// Matrix = 16 floats, Plane = Vector3 + float), so marshalling to the native PhysX
// code stays valid.
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Matrix))]
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Plane))]
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Quaternion))]
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Vector3))]
