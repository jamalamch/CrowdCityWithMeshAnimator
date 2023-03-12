//
//  Created by jiadong chen    
//  https://www.jiadongchen.com
//
//

using UnityEngine;

public struct GPUBoid
{
    public Vector3 pos, rot, flockPos;
	public float speed, nearbyDis;
}

public struct GPUFreeBoid
{
    public Vector3 pos, rot;
    public float deg, noise;
}