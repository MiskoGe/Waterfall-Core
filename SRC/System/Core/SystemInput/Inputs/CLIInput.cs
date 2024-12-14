using Waterfall.System.Processes;

namespace Waterfall.System.Core.SystemInput.Inputs
{
	public class CLIInput : KeyboardInput
	{
		public CLIhost myHost;
		public override void HandleEnter()
		{
			if (myHost.CLIBash.toUpdate == null)
				myHost.CLIBash.ExecuteCommand(CurrentInput);
			else
			{
				myHost.CLIBash.HandleInput(CurrentInput);
				CurrentInput = "";
			}
		}
	}
}
