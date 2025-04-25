using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaceGames {

  public class ZoomAndPan : MonoBehaviour
  {
      public float zoomSpeed = 0.1f;
      public float minZoom = 1f;
      public float maxZoom = 5f;
      public RectTransform layoutGroup;

      private void Update()
      {
          // Zoom In/Out
          float scrollValue = Input.GetAxis("Mouse ScrollWheel");
          float newScale = transform.localScale.x + scrollValue * zoomSpeed;
          newScale = Mathf.Clamp(newScale, minZoom, maxZoom);
          if(newScale != transform.localScale.x)
          {
              transform.localScale = new Vector3(newScale, newScale, 1f);
              LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
          }
      }
  }

}
