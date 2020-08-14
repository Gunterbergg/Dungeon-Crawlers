using UnityEngine;

namespace DungeonCrawlers.UI
{
	public interface ICameraHandler
	{
		bool Enabled { get; set; }
		Rect LimitBounds { get; set; }
		bool LimitBoundsActive { get; set; }
		float CameraMaxSize { get; set; }
		float CameraMinSize { get; set; }

		void MoveTo(Vector3 vec);
		void MoveBy(Vector3 vec);

		void LerpMoveTo(Vector3 vec, float time);
		void LerpMoveBy(Vector3 vec, float time);
		
		void SetSize(float size);
		void ChangeSize(float size);

		void LerpSetSize(float size, float time);
		void LerpChangeSize(float size, float time);

		void SetView(Rect view);
		void LerpSetView(Rect bounds, float time);

		void MaxSize();
		void MinSize();
	}
}
