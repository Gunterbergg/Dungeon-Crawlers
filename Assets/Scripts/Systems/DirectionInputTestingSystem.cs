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
				(sender, args) => {
					string data =
						args.Data.ToString() + "\n" +
						DirectionInputHelper.GetEightDirection(args.Data).ToString() + "\n" +
						"Right angle diff:\n" + Mathf.Atan2(Mathf.Abs(args.Data.x), Mathf.Abs(args.Data.y)) * Mathf.Rad2Deg;
					directionOutput.GetInterface<IOutputHandler<string>>().Output(data);
				};
		}
	} 
}