using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Provides functionality for migrating renderer settings from one type to another.
/// Useful for preserving settings when the renderer type changes in a UI.
/// </summary>
public static class RendererSettingsMigrator
{
    /// <summary>
    /// A cache to store property pairs between source and destination types.
    /// Avoids repeated reflection lookups.
    /// </summary>
    private static readonly ConcurrentDictionary<(Type Source, Type Destination), (PropertyInfo Src, PropertyInfo Dst)[]> _cache = new();

    /// <summary>
    /// Migrate the settings from an old instance to a new instance.
    /// </summary>
    /// <param name="oldSettings">The old settings instance to migrate from.</param>
    /// <param name="destinationType">The type of the new settings instance to migrate to.</param>
    /// <returns>The new settings instance with migrated values. If the old and destination types are the same, the old settings are returned.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the new settings instance cannot be created.</exception>
    public static IRendererSettings Migrate(IRendererSettings? oldSettings, Type destinationType)
    {
        // Create a new instance of the destination type. Ensure it implements IRendererSettings.
        IRendererSettings newSettings = Activator.CreateInstance(destinationType) as IRendererSettings
            ?? throw new InvalidOperationException($"Could not create an instance of type {destinationType.FullName}.");

        // Cannot migrate if the old settings are null, so return the new settings.
        if (oldSettings is null)
            return newSettings;

        // If the old settings are already of the destination type, return them directly.
        if (oldSettings.GetType() == destinationType)
            return oldSettings;

        // Get or calculate the property pairs between the old and new settings types.
        var pairs = _cache.GetOrAdd((oldSettings.GetType(), destinationType), CalculatePairs);
        foreach (var (src, dst) in pairs)
            dst.SetValue(newSettings, src.GetValue(oldSettings));

        return newSettings;
    }

    /// <summary>
    /// Helper method to calculate matching property pairs between source and destination types.
    /// </summary>
    /// <param name="types">A tuple containing the source and destination types.</param>
    /// <returns>An array of tuples containing matching property pairs (source property, destination property).</returns>
    private static (PropertyInfo, PropertyInfo)[] CalculatePairs((Type Source, Type Destination) types)
    {
        var sourceProps = types.Source.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => p.CanRead);
        var destProps = types.Destination.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(p => p.CanWrite)
                                          .ToDictionary(p => p.Name);

        return sourceProps
            .Where(sp => destProps.TryGetValue(sp.Name, out var dp) && dp.PropertyType == sp.PropertyType)
            .Select(sp => (sp, destProps[sp.Name]))
            .ToArray();
    }
}