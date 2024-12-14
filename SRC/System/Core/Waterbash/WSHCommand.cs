using System;
using System.Collections.Generic;

namespace Waterfall.System.Core.Waterbash
{
	public abstract class WSHCommand
	{
		public Dictionary<string, Action<Watershell>> paramActions;
		public List<ParamInfo> paramHelp;
		public Dictionary<string, int> paramPriorities;
		public int MinimumParamsLength = 0;
		public string HelpParam = "--help";
        public virtual string HelpNote { get; set; } = "";
        public virtual bool IsAllowedParam(string param)
		{
			return paramActions.ContainsKey(param);
		}
		public WSHCommand()
		{
			paramActions = new Dictionary<string, Action<Watershell>>();
		}
		public virtual void Execute(string[] Params, Watershell myShell) { }

		public virtual void Run(Watershell myShell) { }

		public virtual void HandleOwnInput(Watershell myShell, string content) { }

		public string[] SortParams(string[] Params)
		{
			// Dictionary to store parameters and their custom values
			Dictionary<string, string> customValues = new Dictionary<string, string>();

			// Iterate through Params array to populate customValues
			for (int i = 0; i < Params.Length; i++)
			{
				string currentParam = Params[i];
				string customValue = null;

				// Check if the next item is a custom value
				if (i + 1 < Params.Length && !Params[i + 1].StartsWith("-"))
				{
					customValue = Params[i + 1];
					i++; // Skip the next param as it is a custom value
				}

				// Store the parameter and its custom value in the dictionary
				if (!customValues.ContainsKey(currentParam))
				{
					customValues[currentParam] = customValue;
				}
			}

			// List to store sorted parameters in correct order
			List<string> orderedParams = new List<string>();

			// Iterate through paramPriorities to maintain the order of sortedParams
			foreach (var param in paramPriorities)
			{
				if (customValues.ContainsKey(param.Key))
				{
					orderedParams.Add(param.Key); // Add the parameter
					if (customValues[param.Key] != null)
					{
						orderedParams.Add(customValues[param.Key]); // Add the custom value if exists
					}
				}
			}

			// Convert orderedParams list to array and return
			return orderedParams.ToArray();
		}
	}
	public class ParamInfo
	{
		public char shortParam = ' ';
		public string longParam = "";
		public string bonusParams = "";
		public string description = "";
	}
	public class CustomParam
	{
		public string Param { get; set; }
		public string CustomValue { get; set; }

		public CustomParam(string param, string customValue)
		{
			Param = param;
			CustomValue = customValue;
		}
	}
}
