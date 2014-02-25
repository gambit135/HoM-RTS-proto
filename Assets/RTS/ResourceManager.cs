using UnityEngine;
using System.Collections;

namespace RTS
{
	public static class ResourceManager {

		private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
		public static Vector3 getInvalidPosition {
			get{
				return invalidPosition;
			}
		}

		/**
		 * Used for scrolling across the screen when mouse approaches an edge on the screen
		 */
		public static float ScrollSpeed{ get { return 25; } }
		/**
		 * Used to define the edges on the screen, on pixels, known as scrolling area.
		 * When the cursor enters this area, the screen will scroll on that particular direction
		 * Also used for ZOOMING
		 */
		public static int ScrollWidth { get { return 50; } }


		/**
		 * Used when calling RotateCamera(). 
		 * Defines how fast the camera will rotate
		 * */
		public static float RotateSpeed{ get { return 100; } }


		//public static float RotateAmount { get { return 10; } }

		//Camera "height" limits
		public static float MinCameraHeight { get { return 10; } }
		public static float MaxCameraHeight { get { return 40; } }
	}
}
