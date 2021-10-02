using Celeritas.Game;
using Cinemachine;

namespace Celeritas.UI
{
	public class MainCinemachineCamera : Singleton<MainCinemachineCamera>
	{
		public CinemachineVirtualCamera VirtualCamera { get; private set; }

		protected override void Awake()
		{
			VirtualCamera = GetComponent<CinemachineVirtualCamera>();
			base.Awake();
		}
	}
}