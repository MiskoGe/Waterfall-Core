using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;

namespace Waterfall
{
	public static class Resources
	{
        //=-=FONTS=-=
        //PSF
        [ManifestResourceStream(ResourceName = "WaterfallCore.Resources.Fonts.PSF.zap-ext-light16.psf")] public static byte[] zap_ext_light16;

		//=-=TXT=-=
		[ManifestResourceStream(ResourceName = "Waterfall.Resources.Commands.Help.Installer.txt")] public static byte[] InstallerHelp;
		[ManifestResourceStream(ResourceName = "Waterfall.Resources.Commands.Other.ASCIILogo.txt")] public static byte[] ASCIILogo;

    }
}
