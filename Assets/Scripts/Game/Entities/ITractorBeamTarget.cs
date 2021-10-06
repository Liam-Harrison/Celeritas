using Celeritas.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	/// <summary>
	/// For any entity that the tractor beam can throw around
	/// </summary>
	interface ITractorBeamTarget
	{
		public Rigidbody2D Rigidbody { get; }

		public EntityStatBar Health { get; }

	}
}
