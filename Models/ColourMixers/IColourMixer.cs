namespace VoronoiNoiseGenerator.Models;

public interface IColourMixer
{
    public ColourMixerDescriptor Descriptor { get; }
    public Colour Mix(Colour color1, Colour color2);
}