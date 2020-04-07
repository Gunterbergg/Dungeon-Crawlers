using DungeonCrawlers.UI;
using UnityEngine;

namespace DungeonCrawlers.Systems
{
	public class DirectionInputTestingSystem : MonoBehaviour 
	{
		public UserView directionInput;
		public UserView directionOutput;

		public void Start() {
			directionInput.GetInterface<IDirectionInput>().Input +=
				(direction) => {
					string data =
						direction.ToString() + "\n" +
						DirectionInputHelper.GetEightDirection(direction).ToString() + "\n" +
						"Right angle diff:\n" + Mathf.Atan2(Mathf.Abs(direction.x), Mathf.Abs(direction.y)) * Mathf.Rad2Deg;
					directionOutput.GetInterface<IOutputHandler<string>>().Output(data);
				};
		}
	} 
}