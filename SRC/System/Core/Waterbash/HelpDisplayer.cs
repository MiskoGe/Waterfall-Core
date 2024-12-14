using System.Collections.Generic;

namespace Waterfall.System.Core.Waterbash
{
	public class HelpDisplayer
	{
		int maxLongLength;
		int maxBonusParameterLength;
		public HelpDisplayer(Watershell shell, string usage, List<ParamInfo> @params)
		{
			shell.CWriteLine("Usage: " + usage);
			for (int i = 0; i < @params.Count; i++)
			{
				if (maxLongLength < @params[i].longParam.Length)
					maxLongLength = @params[i].longParam.Length;
				if (maxBonusParameterLength < @params[i].bonusParams.Length)
					maxBonusParameterLength = @params[i].bonusParams.Length;
			}
			maxLongLength++;
			maxBonusParameterLength++;
			for (int i = 0; i < @params.Count; i++)
			{
				if (@params[i].shortParam != ' ')
					shell.CWrite($"  -{@params[i].shortParam}, ");
				else
					shell.CWrite($"      ");

				if (@params[i].longParam != "")
					shell.CWrite($"--{@params[i].longParam}" + new string(' ', maxLongLength - @params[i].longParam.Length));
				else
					shell.CWrite(new string(' ', maxLongLength + 2));

				if (@params[i].bonusParams != "")
					shell.CWrite($"<{@params[i].bonusParams}>" + new string(' ', maxBonusParameterLength - @params[i].bonusParams.Length));
				else
					shell.CWrite(new string(' ', maxBonusParameterLength + 2));

				shell.CWriteLine(@params[i].description);


			}
		}
	}
}
