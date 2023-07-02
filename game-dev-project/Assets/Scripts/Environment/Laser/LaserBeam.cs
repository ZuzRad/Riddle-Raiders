using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{
    private Vector3 pos, dir;

    GameObject laserObj;
    LineRenderer laser;
    private List<Vector3> laserPositions = new List<Vector3>();

    public LaserBeam(Vector3 pos, Vector3 dir, Material material)
    {
        laser = new LineRenderer();
        
        laserObj = new GameObject();
        laserObj.name = "Laser Beam";
        this.pos = pos;
        this.dir = dir;
        laser = laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        laser.startWidth = 0.1f;
        laser.endWidth = 0.1f;
        laser.material = material;
        laser.startColor = new Color32(245, 207, 130, 255);
        laser.endColor = new Color32(245, 207, 130, 255);
        laser.sortingLayerName = "Laser line";
        CastRay(pos, dir, laser);
    }
    
    void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser)
    {
        laserPositions.Add(pos);
        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, 1))
        {
            CheckHit(hit, dir, laser);
        }
        else
        {
           laserPositions.Add(ray.GetPoint(30));
           UpdateLaser();
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer laser)
    {
        LaserReciever laserReciever = hitInfo.collider.gameObject.GetComponent<LaserReciever>();

        if (laserReciever) 
        {
            laserReciever.isActive = true;
        }
       
        if (hitInfo.collider.transform.parent.GetComponent<Mirror>())
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
            CastRay(pos, dir, laser);
        } 
        else
        {
            laserPositions.Add(hitInfo.point);
            UpdateLaser();
        }
    }
    
    void UpdateLaser()
    {
        int count = 0;

        laser.positionCount = laserPositions.Count;

        foreach (var position in laserPositions)
        {
            laser.SetPosition(count, position);
            count++;
        }
    }
}
