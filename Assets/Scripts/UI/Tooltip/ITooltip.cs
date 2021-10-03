using Celeritas.Game.Actions;
using Celeritas.Game.Entities;

namespace Celeritas.UI.Tooltips
{
	public interface ITooltip
	{
		public ModuleEntity TooltipEntity { get; }
		public GameAction TooltipAction { get; }
	}
}