using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Celeritas.Game.Entities
{
	public class LootController : Singleton<LootController>
	{
		public void LootDrop(float dropValue, bool isBoss, string dropType)
		{
			Debug.Log(dropValue + " and " + isBoss + " and " + dropType);
		}
	}
}