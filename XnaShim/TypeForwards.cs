using System.Runtime.CompilerServices;

// Die einzigen XNA-Typen, die StillDesign.PhysX.Net in seinen Signaturen verwendet.
// Ermittelt aus der TypeRef-Tabelle von Lib/StillDesign.PhysX.Net.dll.
// Layout stimmt mit XNA 3.0 ueberein (Vector3 = 3 float, Quaternion = 4 float,
// Matrix = 16 float, Plane = Vector3 + float), daher ist auch das Marshalling
// zum nativen PhysX-Code unveraendert gueltig.
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Matrix))]
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Plane))]
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Quaternion))]
[assembly: TypeForwardedTo(typeof(Microsoft.Xna.Framework.Vector3))]
