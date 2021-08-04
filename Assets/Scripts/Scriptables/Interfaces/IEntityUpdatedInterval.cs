using Celeritas.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scriptables.Interfaces
{
	/// <summary>
	/// Called when the entity is updated after a second has elapsed (since the last call)
	/// </summary>
	public interface IEntityUpdatedInterval
	{
		void OnEntityUpdatedAfterInterval(Entity entity, ushort level);
	}
}
