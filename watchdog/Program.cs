using System;
using System.IO.MemoryMappedFiles;

try {
    var mmf = MemoryMappedFile.OpenExisting("heartbeat");
    using var accessor = mmf.CreateViewAccessor();
    byte value = accessor.ReadByte(0);

    if (value == 1) {
        accessor.Write(0, (byte)0);
        Environment.Exit(0);
    }
    else {
        Environment.Exit(1);
    }
}
catch {
    Environment.Exit(1);
}
