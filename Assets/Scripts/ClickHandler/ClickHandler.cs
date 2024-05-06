using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButtonDown(0)) // Check if the left mouse button was clicked or on touch input
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null) {
                hit.collider.gameObject.GetComponent<PieceController>().OnPieceClicked();
            }
        }
    }
}
