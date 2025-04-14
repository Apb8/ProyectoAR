using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        Vector3 lookDirection = transform.position - Camera.main.transform.position;
        lookDirection.y = 0; // evitar que gire hacia arriba/abajo pero se puede quitar
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
