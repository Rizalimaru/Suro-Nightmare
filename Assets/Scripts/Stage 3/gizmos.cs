using UnityEngine;

public class gizmoss : MonoBehaviour
{
    public Transform[] points;

    private void OnDrawGizmos()
    {
        if (points == null || points.Length < 2) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < points.Length - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
                Gizmos.DrawSphere(points[i].position, 0.1f);
            }
        }

        // Gambar garis terakhir ke pertama untuk loop
        Gizmos.DrawLine(points[points.Length - 1].position, points[0].position);
        Gizmos.DrawSphere(points[points.Length - 1].position, 0.1f);
    }
}
