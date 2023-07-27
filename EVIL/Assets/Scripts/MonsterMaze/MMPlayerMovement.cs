using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMPlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask wall;
    [SerializeField] private Material black;
    [SerializeField] private Material dither;
    private MeshRenderer[] upDown;
    private MeshRenderer[] leftRight;
    private float rotation = 0;

    void Awake()
    {
        Transform upDownObj = GameObject.Find("UpDown").transform;
        upDown = new MeshRenderer[upDownObj.childCount];

        for (int i = 0; i < upDown.Length; i++)
            upDown[i] = upDownObj.GetChild(i).GetComponent<MeshRenderer>();

        Transform leftRightObj = GameObject.Find("LeftRight").transform;
        leftRight = new MeshRenderer[leftRightObj.childCount];

        for (int i = 0; i < leftRight.Length; i++)
            leftRight[i] = leftRightObj.GetChild(i).GetComponent<MeshRenderer>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        bool a = Input.GetKeyDown(KeyCode.A);
        bool d = Input.GetKeyDown(KeyCode.D);

        if (a || d)
        {
            if (a)
            {
                transform.Rotate(Vector3.up * -90f);
                rotation -= 90f;
            }
            if (d)
            {
                transform.Rotate(Vector3.up * 90f);
                rotation += 90f;
            }

            if (rotation % 360 == 0 || rotation % 180 == 0)
            {
                foreach (MeshRenderer renderer in upDown)
                    renderer.material = black;
                foreach (MeshRenderer renderer in leftRight)
                    renderer.material = dither;
            }
            else
            {
                foreach (MeshRenderer renderer in upDown)
                    renderer.material = dither;
                foreach (MeshRenderer renderer in leftRight)
                    renderer.material = black;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!WallCheck())
            {
                transform.Translate(Vector3.forward * 10f);
            }
        }
    }

    private bool WallCheck()
    {
        return Physics.Raycast(transform.position, transform.forward, 6f, wall);
    }
}
