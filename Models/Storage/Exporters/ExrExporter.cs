using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using TinyEXR;
using TinyEXR.V3;

namespace VoronoiNoiseGenerator.Models;

public sealed class ExrExporter : IExporter
{
    /// <summary>
    /// Gets the descriptor for this exporter.
    /// </summary>
    public static ExporterDescriptor Descriptor { get; } = new(
        "OpenEXR",
        typeof(ExrExporter),
        new FilePickerFileType("OpenEXR") { Patterns = ["*.exr"], AppleUniformTypeIdentifiers = ["com.ilm.openexr-image"], MimeTypes = ["image/x-exr"] }
    );

    /// <inheritdoc/>
    public async Task ExportAsync(TextureBuffer texture, IStorageFile file)
    {
        // Reinterpret the Vector4 pixel buffer as a flat interleaved float
        // array — same 16 bytes per pixel, no copy, no per-pixel conversion.
        ReadOnlySpan<Vector4> pixels = texture.AsSpan();
        ReadOnlySpan<float> floats = MemoryMarshal.Cast<Vector4, float>(pixels);

        // TinyEXR's high-level API expects an owned float[]; materialise once here.
        float[] rgba = floats.ToArray();

        ResultCode result = Exr.SaveEXRToMemory(
            rgba,
            texture.Width,
            texture.Height,
            components: 4,
            asFp16: false,
            out byte[] encoded);

        if (result != ResultCode.Success)
            throw new InvalidOperationException($"EXR encode failed: {result}");

        using var stream = await file.OpenWriteAsync();
        await stream.WriteAsync(encoded);
    }
}